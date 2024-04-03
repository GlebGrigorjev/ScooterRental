namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IScooterService _scooterService;
        private readonly IRentedScooterArchive _archive;
        private readonly IRentalCalculatorService _calculatorService;
        public string Name { get; }

        public RentalCompany(string name,
            IScooterService scooterService,
            IRentedScooterArchive archive,
            IRentalCalculatorService calculatorService)
        {
            _archive = archive;
            _scooterService = scooterService;
            Name = name;
            _calculatorService = calculatorService;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            return _calculatorService.CalculateIncome(year, includeNotCompletedRentals);
        }

        public decimal EndRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            var rentalRecord = _archive.EndRental(scooter.Id, DateTime.Now);

            scooter.IsRented = false;

            return _calculatorService.CalculateRent(rentalRecord);
        }

        public void StartRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            _archive.AddrentedScooter(new RentedScooter(scooter.Id, DateTime.Now, scooter.PricePerMinute));

            scooter.IsRented = true;
        }
    }
}