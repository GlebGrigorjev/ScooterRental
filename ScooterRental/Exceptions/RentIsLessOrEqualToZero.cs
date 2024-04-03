namespace ScooterRental.Exceptions
{
    public class RentIsLessOrEqualToZero : Exception
    {
        public RentIsLessOrEqualToZero() : base("Rent Is Less Or Equal To Zero")
        {

        }
    }
}