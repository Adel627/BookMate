using System.ComponentModel.DataAnnotations;

namespace BookMate.web.Core.Models
{
    public class Governorate:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Area> Areas { get; set; } = new List<Area>();
    }
}
