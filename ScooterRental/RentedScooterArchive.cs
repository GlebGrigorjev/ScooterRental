using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class RentedScooterArchive : IRentedScooterArchive
    {
        public List<RentedScooter> RentedScooters { get; set; }

        public void AddrentedScooter(RentedScooter scooter)
        {
            RentedScooters.Add(scooter);
        }

        public RentedScooter EndRental(string scooterId, DateTime rentEnd)
        {
            foreach (var rentedScooter in RentedScooters)
            {
                if (scooterId == rentedScooter.ScooterId)
                {
                    return rentedScooter;
                }
            }

            throw new ScooterNotFoundException();
        }
    }
}