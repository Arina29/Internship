using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using MED.DAL.Models;

namespace MED.DAL.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext Context;
        private IDbSet<TEntity> _dbSet => Context.Set<TEntity>();
        public IQueryable<TEntity> Entities => _dbSet;

        public GenericRepository(ApplicationDbContext _context)
        {
            Context = _context;
        }

        public bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Any(predicate);
        }

        public TEntity Get(Guid? id)
        {
            if (id != null)
                return Context.Set<TEntity>().Find(id);
            else
                throw new ArgumentNullException(nameof(id));
        }

        public TEntity GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return (TEntity)Context.Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        public List<TEntity> GetAllBy(Expression<Func<TEntity, bool>> predicate)
        {
            var query = Context.Set<TEntity>().Where(predicate);
            var list = query.AsNoTracking().ToList();
            return list;
        }

        public List<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsNoTracking().ToList();
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            if (!Context.Set<TEntity>().Any(predicate))
                return new List<TEntity>();
            var list = Context.Set<TEntity>().Where(predicate).AsNoTracking().ToList();
            return list;
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            Context.Set<TEntity>().AsNoTracking();
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
            Context.SaveChanges();
        }

        public DbEntityEntry<TEntity> EnsureAttachedEF(TEntity entity)
        {
            var e = Context.Entry(entity);
            if (e.State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
                e = Context.Entry(entity);
            }

            return e;
        }

        public void Remove(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            try
            {
                Context.Set<TEntity>().Attach(entity);
                Context.Set<TEntity>().Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = null;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
            Context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Context.Set<TEntity>().Attach(item);
                Context.Set<TEntity>().RemoveRange(entities);
            }
            Context.SaveChanges();
        }

        public void Edit(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            Context.Set<TEntity>().AsNoTracking();
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges(); //save it
        }
    }
}
