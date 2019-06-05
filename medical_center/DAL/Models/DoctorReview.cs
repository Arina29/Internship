using System;

namespace MED.DAL.Models
{
    public class DoctorReview
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        public Patient Patient { get; set; }

        public Guid DoctorId { get; set; }

        public Doctors Doctor { get; set; }

        public string Review { get; set; }

        public decimal Rating { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
