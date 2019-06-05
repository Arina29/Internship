using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MED.DAL.Models
{
    public class DoctorProcedures
    {
        [Key]
        [Column(Order = 1)]
        public Guid DoctorId { get; set; }
        public virtual Doctors Doctor { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ProcedureId { get; set; }
        public virtual Procedure Procedure { get; set; }
    }
}
