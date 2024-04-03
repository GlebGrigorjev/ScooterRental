using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooterList;

        public ScooterService(List<Scooter> scooters)
        {
            _scooterList = scooters;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new InvalidIdException();

            if (pricePerMinute <= 0)
                throw new InvalidPriceException();

            if (_scooterList.Any(scooter => scooter.Id == id))
                throw new DuplicateScooterProvidedException();

            _scooterList.Add(new Scooter(id, pricePerMinute));
        }

        public Scooter GetScooterById(string scooterId)
        {
           var scooter = _scooterList.FirstOrDefault(s => s.Id == scooterId);

            if (scooter == null)
                throw new ScooterNotFoundException();

            return scooter;
        }

        public IList<Scooter> GetScooters()
        {
            if (_scooterList.Count == 0)
                throw new ScooterListIsEmptyException();

            return _scooterList;
        }

        public void RemoveScooter(string id)
        {
            var scooter = _scooterList.SingleOrDefault(s => s.Id == id);

            if (scooter == null)
                throw new ScooterNotFoundException();

            _scooterList.Remove(scooter);
        }
    }
}