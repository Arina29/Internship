namespace MED.DAL.Models
{
    public class Procedure : BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
