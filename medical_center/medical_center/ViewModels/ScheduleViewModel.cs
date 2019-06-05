using System;
using System.ComponentModel.DataAnnotations;

namespace MED.Presentation.ViewModels
{
    public class ScheduleViewModel
    {
        public Guid Id { get; set; }

        public string Day { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime LunchStart { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime LunchEnd { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime WorkStart { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime WorkEnd { get; set; }

        public bool IsWorking { get; set; }
    }
}