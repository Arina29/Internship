using System.Data.Entity;
using System.Linq;
using MED.DAL.Models;
using MED.DAL.Repository;

namespace MED.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<Doctors> DoctorsRepository => new GenericRepository<Doctors>(_context);
        public IGenericRepository<Patient> PacientRepository => new GenericRepository<Patient>(_context);
        public IGenericRepository<Appointment> AppointmentRepository => new GenericRepository<Appointment>(_context);
        public IGenericRepository<Book> BookRepository => new GenericRepository<Book>(_context);
        public IGenericRepository<DoctorProcedures> DoctorProcRepository => new GenericRepository<DoctorProcedures>(_context);
        public IGenericRepository<Images> ImagesRepository => new GenericRepository<Images>(_context);
        public IGenericRepository<PatientBook> PatientBookRepository => new GenericRepository<PatientBook>(_context);
        public IGenericRepository<Procedure> ProcedureReposiory => new GenericRepository<Procedure>(_context);
        public IGenericRepository<Schedule> ScheduleRepository => new GenericRepository<Schedule>(_context);
        public IGenericRepository<Specializations> SpecializationRepository => new GenericRepository<Specializations>(_context);
        public IGenericRepository<DoctorSchedule> DoctorScheduleRepository => new GenericRepository<DoctorSchedule>(_context);
        public IGenericRepository<DoctorReview> DoctorReviewRepository => new GenericRepository<DoctorReview>(_context);
        public IGenericRepository<PatientAnalysis> PatientAnalysisRepository => new GenericRepository<PatientAnalysis>(_context);


        public UnitOfWork()
        {
            _context = new ApplicationDbContext();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void RejectChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
