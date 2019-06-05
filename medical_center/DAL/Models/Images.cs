using System;

namespace MED.DAL.Models
{
    public class Images
    {
        public Guid Id { get; set; }

        public Guid DoctorId { get; set; }

        public virtual Doctors Doctor { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string Extension { get; set; }
    }
}
