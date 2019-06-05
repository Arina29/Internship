using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MED.DAL.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        IQueryable<TEntity> Entities { get; }
        TEntity Get(Guid? id);
        bool Exist(Expression<Func<TEntity, bool>> predicate);

        TEntity GetBy(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetAll();
        List<TEntity> GetAllBy(Expression<Func<TEntity, bool>> predicate);

        List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void Edit(TEntity entity);
    }
}
