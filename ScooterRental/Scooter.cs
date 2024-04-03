namespace ScooterRental
{
    public class Scooter
    {
        public string Id { get; }
        public decimal PricePerMinute { get; }
        public bool IsRented { get; set; }

        public Scooter(string id, decimal pricePerMinute)
        {
            Id = id;
            PricePerMinute = pricePerMinute;
        }
    }
}