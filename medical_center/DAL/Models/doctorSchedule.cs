using System;

namespace MED.DAL.Models
{
    public class DoctorSchedule
    {
        public Guid Id { get; set; }

        public Guid? DoctorId { get; set; }

        public virtual Doctors Doctor { get; set; }
    }
}
