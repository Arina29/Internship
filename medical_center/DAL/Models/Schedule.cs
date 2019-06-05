using System;

namespace MED.DAL.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }

        public Guid? DoctorScheduleId { get; set; }

        public virtual DoctorSchedule DoctorSchedule { get; set; }

        public string Day { get; set; }

        public DateTime LunchStart { get; set; }

        public DateTime LunchEnd { get; set; }

        public DateTime WorkStart { get; set; }

        public DateTime WorkEnd { get; set; }

        public bool IsWorking { get; set; }

    }
}