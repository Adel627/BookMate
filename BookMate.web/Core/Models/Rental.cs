namespace BookMate.web.Core.Models
{
    public class Rental:BaseModel
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public Subscriber? Subscriber { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public bool PenaltyPaid { get; set; }
       public ICollection<RentalBook> RentalBooks { get; set; } = new List<RentalBook>();
    }
}
