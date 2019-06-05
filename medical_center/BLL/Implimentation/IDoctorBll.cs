using System;
using System.Collections.Generic;
using System.Web;
using MED.DAL.Models;

namespace MED.BLL.Implimentation
{
    public interface IDoctorBll
    {
        BaseModel InitializeBaseModel(BaseModel model);

        void SaveOrUpdate(Doctors entity);

        Images SaveImage(HttpPostedFileBase img, Doctors entity);

        List<Schedule> GetDoctorSchedule(string UserId);

        void InitializeDoctorSchedule(Guid Id);

        List<Schedule> SortByDayWeek(List<Schedule> schedule);

        List<Schedule> GetDoctorWorkingDays(string UserId);

        bool VerifyDate(DateTime Date, Guid DoctorId);

        int[] GetWorkingDays(Guid Id);

        string[] GetWorkingHours(DateTime day, Guid DoctorId);

        void AddPatientAnalysis(byte[] file, Guid PatientId);
    }
}
