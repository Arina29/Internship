using System;
using System.Collections.Generic;
using MED.DAL.Models;

namespace MED.BLL.Implimentation
{
    public interface IUserBll
    {
        BaseModel InitializeBaseModel(BaseModel model);

        void SaveOrUpdate(Patient entity);

        void AddNewBook(Book entity, Guid Id);

        IEnumerable<DiseaseType> GetDiseaseTypes();

        IEnumerable<Gender> GetGenderTypes();

        int CalculateAges(Patient entity);

        IEnumerable<Book> PatientBook(Guid? Id);

        void DeleteAppointment(Guid Id);
    }
}
