using System;

namespace MED.DAL.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? DeleteTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
