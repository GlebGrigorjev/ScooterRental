namespace ScooterRental.Exceptions
{
    public class DuplicateScooterProvidedException : Exception
    {
        public DuplicateScooterProvidedException() : base("Duplicate scooter provided") { }
    }
}