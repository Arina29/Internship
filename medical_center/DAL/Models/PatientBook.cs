using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MED.DAL.Models
{
    public class PatientBook
    {
        [Key]
        [Column(Order = 1)]
        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
