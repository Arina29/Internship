using System;

namespace MED.DAL.Models
{
    public class Patient : BaseModel
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mobile { get; set; }

        public string Mail { get; set; }

        public string IDNP { get; set; }

        public byte Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public byte[] Image { get; set; }
    }
}
