namespace ScooterRental
{
    public interface IProfitArchive
    {
        public Dictionary<int, decimal> Profits { get; set; }
        public void AddScooterProfit(RentedScooter rentalRecord, decimal result);
    }
}