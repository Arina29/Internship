using System;

namespace MED.DAL.Models
{
    public class Book : BaseModel
    {
        public DateTime DiagnosticsDate { get; set; }        

        public string Diagnostics { get; set; }

        public byte DiseaseType { get; set; }

        public string Symptoms { get; set; }

        public string Recipe { get; set; }

        public Guid DoctorId { get; set; }

        public virtual Doctors Doctor { get; set; }

    }
}
