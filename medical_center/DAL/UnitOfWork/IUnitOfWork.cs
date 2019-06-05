using System;
using MED.DAL.Models;
using MED.DAL.Repository;

namespace MED.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Doctors> DoctorsRepository { get; }
        IGenericRepository<Patient> PacientRepository { get; }
        IGenericRepository<Appointment> AppointmentRepository { get; }
        IGenericRepository<Book> BookRepository { get; }
        IGenericRepository<DoctorProcedures> DoctorProcRepository { get; }
        IGenericRepository<Images> ImagesRepository { get; }
        IGenericRepository<PatientBook> PatientBookRepository { get; }
        IGenericRepository<Procedure> ProcedureReposiory { get; }
        IGenericRepository<Schedule> ScheduleRepository { get; }
        IGenericRepository<Specializations> SpecializationRepository { get; }
        IGenericRepository<DoctorSchedule> DoctorScheduleRepository { get; }
        IGenericRepository<DoctorReview> DoctorReviewRepository { get; }
        IGenericRepository<PatientAnalysis> PatientAnalysisRepository { get; }
        /// <summary>
        /// Commits all changes
        /// </summary>
        void Commit();
        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void RejectChanges();
        void Dispose();
    }
}
