using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MED.DAL.Models;

namespace MED.Presentation.Models
{
    public class DoctorFormViewModels
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your Surname")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Mail { get; set; }

        [Display(Name = "Specialization")]
        public List<Specializations> Specialization { get; set; }

        [Display(Name = "Experience years")]
        public int Experience { get; set; }

        public Guid SelectedSpecialization { get; set; }

        public string Review { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Account Image")]
        public HttpPostedFileBase Image { get; set; }

        public string ImagePath { get; set; }

        public decimal Rating { get; set; }

        public DateTime CreateTime { get; set; }

        public string Specialty { get; set; }

        public List<ScheduleViewModel> Schedule { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }

        public byte[] PatientImage { get; set; }

        [Display(Name = "Professional Information")]
        public string Studies { get; set; }

        [Display(Name = "Consultation Price")]
        public string ConsultationPrice { get; set; }
    }
}