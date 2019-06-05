using System;

namespace MED.Presentation.Models
{
    public class ReviewViewModel
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        public Guid DoctorId { get; set; }

        public string Review { get; set; }

        public decimal Rating { get; set; }

        public string PatientName { get; set; }

        public DateTime CreateTime { get; set; }

        public byte[] PatientImage { get; set; }

    }
}