using System;
using System.Collections.Generic;

namespace MED.DAL.Models
{
    public class Doctors : BaseModel
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }
       
        public int Experience { get; set; }

        public Guid SpecializationId { get; set; }

        public Specializations Specialization { get; set; }

        public virtual ICollection<Images> Images { get; set; }

        public string Studies { get; set; }

        public decimal ConsultationPrice { get; set; }
    }
}
