using System;
using System.Collections.Generic;
using System.Linq;
using MED.DAL.Models;
using MED.DAL.UnitOfWork;

namespace MED.BLL.Implimentation
{
    public class UserBll : IUserBll
    {
        IUnitOfWork _context;

        public UserBll(IUnitOfWork unitOfWork)
        {
            _context = unitOfWork;
        }

        public BaseModel InitializeBaseModel(BaseModel model)
        {
            model.CreateTime = DateTime.Now;
            model.IsDeleted = false;
            return model;
        }

        public void SaveOrUpdate(Patient entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
                InitializeBaseModel(entity);
                _context.PacientRepository.Add(entity);

            }
            else
            {
                _context.PacientRepository.Edit(entity);
            }
            
        }

        public void AddNewBook(Book entity, Guid Id)
        {

            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            _context.BookRepository.Add(entity);

            if (Id != Guid.Empty)
            { 
                var book = new PatientBook()
                {
                    PatientId = Id,
                    BookId = entity.Id
                };
                _context.PatientBookRepository.Add(book);
            }
        }

        public IEnumerable<DiseaseType> GetDiseaseTypes()
        {
            var diseaseTypes = new List<DiseaseType>
            {
                new DiseaseType {Id = 1, Name = "Infectious"},
                new DiseaseType {Id = 2, Name = "Chronic"},
                new DiseaseType {Id = 3, Name = "Neither"}
            };
            return diseaseTypes.AsEnumerable();
        }

        public IEnumerable<Gender> GetGenderTypes()
        {
            var genders = new List<Gender>
            {
                new Gender {Id = 1, Name = "Female"},
                new Gender {Id = 2, Name = "Male"},
                new Gender {Id = 3, Name = "Other"}
            };
            return genders.AsEnumerable();
        }

        public int CalculateAges(Patient entity)
        {
            int years = DateTime.Now.Year - entity.DateOfBirth.Year;
            if ((entity.DateOfBirth.Month > DateTime.Now.Month) || (entity.DateOfBirth.Month == DateTime.Now.Month && entity.DateOfBirth.Day > DateTime.Now.Day))
                years--;
            return years;
        }

        public IEnumerable<Book> PatientBook(Guid? Id)
        {
            var books = _context.PatientBookRepository.Find(b => b.PatientId == Id);
            var diagnosis = new List<Book>();
            if(books == null)
                return new List<Book>().AsEnumerable();

            foreach (var item in books)
                diagnosis.Add(_context.BookRepository.GetBy(x=>x.Id == item.BookId));
            return diagnosis.AsEnumerable();
        }

        public void DeleteAppointment(Guid Id)
        {
            var appointment = _context.AppointmentRepository.Get(Id);
            _context.AppointmentRepository.Remove(appointment);
        }
    }
}
