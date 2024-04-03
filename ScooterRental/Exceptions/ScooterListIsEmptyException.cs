namespace ScooterRental.Exceptions
{
    public class ScooterListIsEmptyException : Exception
    {
        public ScooterListIsEmptyException() : base("List of scooters is empty!")
        {

        }
    }
}