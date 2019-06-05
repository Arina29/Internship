using System;
using System.Collections.Generic;

namespace MED.Presentation.Models
{
    public class MedicalBookViewModel
    {
        public Guid? Id { get; set; }

        public IEnumerable<BookViewModel> Book { get; set; }

        public IEnumerable<AnalysisViewModel> Analysis { get; set; }
    }
}