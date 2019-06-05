using System;

namespace MED.DAL.Models
{
    public class PatientAnalysis : BaseModel
    {
        public byte[] File { get; set; }

        public Guid PatientId { get; set; }

        public Patient Patient { get; set; }

    }
}
