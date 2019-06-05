using System;

namespace MED.Presentation.ViewModels
{
    public class AnalysisViewModel
    {
        public Guid? Id { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? DeleteTime { get; set; }

        public bool IsDeleted { get; set; }

        public string DoctorProfession { get; set; }

        public byte[] File { get; set; }
    }
}