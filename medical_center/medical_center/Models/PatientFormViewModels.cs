using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MED.DAL.Models;

namespace MED.Presentation.Models
{
    public class PatientFormViewModels
    {
        public Guid? Id { get; set; }

        public string UserId { get; set; }

        public Guid? BookId { get; set; }

        public DateTime CreateTime { get; set; }

        public int Age { get; set; }

        [Display(Name = "Gender")]
        public IEnumerable<Gender> Genders { get; set; }

        [Required(ErrorMessage = "Please select your gender")]
        public byte SelectedGender { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name length is not valid")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Surname length is not valid")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Mobile Phone")]
        [RegularExpression("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid phone number")]
        public string Mobile { get; set; }
        
        [Required(ErrorMessage = "Enter your email address")]
        [Display(Name = "Email address")]
        [EmailAddress]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Enter your date of birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter your IDNP")]
        [Display(Name = "IDNP")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "IDNP should contain 13 digits")]
        public string IDNP { get; set; }

        [Display(Name = "Account Image")]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "Account Image")]
        public byte[] Image1 { get; set; }

        public BookViewModel Diagnosis { get; set; }

        public string Gender { get; set; }

        public MedicalBookViewModel MedicalBook { get; set; }
    }
}