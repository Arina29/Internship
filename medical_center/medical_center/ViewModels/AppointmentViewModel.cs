using System;
using System.ComponentModel.DataAnnotations;

namespace MED.Presentation.ViewModels
{
    public class AppointmentViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "First name")]
        public string Name { get; set; }

        [Display(Name = "Last name")]
        public string Surname { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Mail { get; set; }

        [Display(Name = "Contact number")]
        [RegularExpression("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid phone number")]
        public string Mobile { get; set; }

        [Display(Name = "IDNP")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "IDNP should contain 13 digits")]
        public string IDNP { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        public Guid DoctorId { get; set; }

        public Guid PatientId { get; set; }

        public int[] WorkingDays { get; set; }

        public bool IsCanceled { get; set; }

        public string Doctor { get; set; }

        public string DoctorSpecialty { get; set; }

        public string Day { get; set; }
    }
}