namespace ScooterRental
{
    public interface IRentedScooterArchive
    {
        public List<RentedScooter> RentedScooters { get; set; }
        void AddrentedScooter(RentedScooter scooter);
        RentedScooter EndRental(string scooterId, DateTime rentEnd);
    }
}