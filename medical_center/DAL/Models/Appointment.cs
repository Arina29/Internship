using System;

namespace MED.DAL.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }

        public Guid DoctorId { get; set; }

        public virtual Doctors  Doctor{ get; set; }

        public Guid PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public bool Canceled { get; set; }

        public string Message { get; set; }

        public DateTime AppointmentDate { get; set; }
    }
}
