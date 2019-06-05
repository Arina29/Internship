using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MED.DAL.Models;

namespace MED.Presentation.ViewModels
{
    public class BookViewModel
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name= "Diagnosis date" )]
        public DateTime DiagnosticDate { get; set; }

        [Display(Name = "Diagnostic")]
        public string Diagnostics { get; set; }

        [Display(Name = "Recipe")]
        public string Recipe { get; set; }

        [Display(Name = "Disease Type")]
        public IEnumerable<DiseaseType> DiseaseTypes { get; set; }

        public byte DiseaseType { get; set; }

        [Display(Name = "Symptoms")]
        public string Symptoms { get; set; }

        public Guid DoctorId { get; set; }

        public string DiseaseTypeName { get; set; }

        public string Doctor { get; set; }

        public HttpPostedFileBase  File { get; set; }

        public byte[] AnalysisFile { get; set; }

        public IEnumerable<AnalysisViewModel> Analysis { get; set; }
    }
}