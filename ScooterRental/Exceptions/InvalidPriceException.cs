namespace ScooterRental.Exceptions
{
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException() : base("Price is not valid")
        {

        }
    }
}