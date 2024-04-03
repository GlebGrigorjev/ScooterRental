using FluentAssertions;
using Moq;
using Moq.AutoMock;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalCalculatorServiceTests
    {
        private AutoMocker _mocker;
        private Mock<IRentedScooterArchive> _rentedScooterArchiveMock;
        private Mock<IScooterService> _scooterServiceMock;
        private Mock<IProfitArchive> _profitArchiveMock;
        private RentalCalculatorService _rentalCalculatorService;

        [TestInitialize]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _scooterServiceMock = _mocker.GetMock<IScooterService>();
            _rentedScooterArchiveMock = _mocker.GetMock<IRentedScooterArchive>();
            _profitArchiveMock = _mocker.GetMock<IProfitArchive>();
            _mocker.Use(_rentedScooterArchiveMock.Object);
            _mocker.Use(_profitArchiveMock.Object);
            _rentalCalculatorService = _mocker.CreateInstance<RentalCalculatorService>();
        }

        [TestMethod]
        public void CalculateRent_SingleDayRental_ReturnsCorrectValue()
        {
            Scooter scooter = new Scooter("1", 0.2m) { IsRented = true };
            var now = new DateTime(2024, 4, 3, 10, 30, 0);
            var endRent = now.AddHours(1).AddMinutes(23).AddSeconds(45);
            var rentalRecord = new RentedScooter(scooter.Id, now, scooter.PricePerMinute) { RentEnd = endRent };
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter);
            _rentedScooterArchiveMock.Setup(archive => archive.EndRental(scooter.Id, It.IsAny<DateTime>())).Returns(rentalRecord);

            decimal result = _rentalCalculatorService.CalculateRent(rentalRecord);

            result.Should().Be(16.60m);
        }

        [TestMethod]
        public void CalculateRent_Rented_Today_Returned_Three_Days_After_Should_Return_Proper_Value()
        {
            Scooter scooter = new Scooter("1", 0.2m) { IsRented = true };
            var now = new DateTime(2024, 4, 3, 10, 30, 0);
            var endRent = now.AddDays(3).AddHours(3).AddMinutes(23).AddSeconds(45);
            var rentalRecord = new RentedScooter(scooter.Id, now, scooter.PricePerMinute) { RentEnd = endRent };
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter);
            _rentedScooterArchiveMock.Setup(archive => archive.EndRental(scooter.Id, It.IsAny<DateTime>())).Returns(rentalRecord);

            decimal result = _rentalCalculatorService.CalculateRent(rentalRecord);

            result.Should().Be(80.00m);
        }

        [TestMethod]
        public void CalculateIncome_Year_Not_Provided_Only_Finished_Rents_Included_Returns_Correct_Sum()
        {
            _profitArchiveMock.SetupGet(p => p.Profits).Returns(new Dictionary<int, decimal>
            {
                { 2022, 50.0m },
                { 2023, 75.0m },
            });
            _rentedScooterArchiveMock.SetupGet(r => r.RentedScooters).Returns(new List<RentedScooter>
            {
                new RentedScooter("1", DateTime.Now.AddDays(-2), 0.2m) { RentEnd = DateTime.Now.AddDays(-1) },
                new RentedScooter("2", DateTime.Now.AddDays(-1), 0.3m) { RentEnd = DateTime.Now },
            });

            decimal result = _rentalCalculatorService.CalculateIncome(null, false);

            result.Should().Be(125.0m);
        }

        [TestMethod]
        public void CalculateIncome_Year_Not_Provided_Unfinished_Rents_Included_Returns_Correct_Sum()
        {
            _profitArchiveMock.SetupGet(p => p.Profits).Returns(new Dictionary<int, decimal>
            {
                { 2022, 50.0m }
            });
            _rentedScooterArchiveMock.SetupGet(r => r.RentedScooters).Returns(new List<RentedScooter>
            {
                new RentedScooter("1", DateTime.Now.AddDays(-2), 0.1m) { RentEnd = DateTime.Now },
                new RentedScooter("2", DateTime.Now.AddHours(-5), 0.1m) { RentEnd = DateTime.Now },
            });

            decimal result = _rentalCalculatorService.CalculateIncome(null, true);

            result.Should().Be(130.0m);
        }

        [TestMethod]
        public void CalculateIncome_Year_Provided_Finished_Rents_Included_Returns_Correct_Sum()
        {
            _profitArchiveMock.SetupGet(p => p.Profits).Returns(new Dictionary<int, decimal>
            {
                { 2022, 50.0m },
                { 2023, 55.0m },
                { 2021, 150.0m }
            });
            _rentedScooterArchiveMock.SetupGet(r => r.RentedScooters).Returns(new List<RentedScooter>
            {
                new RentedScooter("1", DateTime.Now.AddDays(-2), 0.1m) { RentEnd = DateTime.Now.AddDays(-1) },
                new RentedScooter("2", DateTime.Now.AddHours(-5), 0.1m) { RentEnd = DateTime.Now},
            });

            decimal result = _rentalCalculatorService.CalculateIncome(2022, true);

            result.Should().Be(50.0m);
        }

        [TestMethod]
        public void CalculateIncome_Year_Provided_Finished_Rents_Not_Included_Returns_Correct_Sum()
        {
            _profitArchiveMock.SetupGet(p => p.Profits).Returns(new Dictionary<int, decimal>
            {
                { 2022, 50.0m },
                { 2023, 55.0m },
                { 2021, 150.0m }
            });
            _rentedScooterArchiveMock.SetupGet(r => r.RentedScooters).Returns(new List<RentedScooter>
            {
                new RentedScooter("1", DateTime.Now.AddDays(-2), 0.1m) { RentEnd = DateTime.Now.AddDays(-1) },
                new RentedScooter("2", DateTime.Now.AddHours(-5), 0.1m) { RentEnd = DateTime.Now},
            });

            decimal result = _rentalCalculatorService.CalculateIncome(2022, false);

            result.Should().Be(50.0m);
        }

        [TestMethod]
        public void CalculateIncome_Current_Year_Provided_Unfinished_Rents_Included_Returns_Correct_Sum()
        {
            _profitArchiveMock.SetupGet(p => p.Profits).Returns(new Dictionary<int, decimal>
            {
                { 2022, 50.0m },
                { 2023, 55.0m },
                { 2024, 150.0m }
            });
            _rentedScooterArchiveMock.SetupGet(r => r.RentedScooters).Returns(new List<RentedScooter>
            {
                new RentedScooter("2", DateTime.Now.AddHours(-5), 0.1m) { RentEnd = DateTime.Now}
            });

            decimal result = _rentalCalculatorService.CalculateIncome(2024, true);

            result.Should().Be(170.0m);
        }

        [TestMethod]
        public void CalculateIncome_Current_Year_Provided_Finished_Rents_Not_Included_Returns_Correct_Sum()
        {
            _profitArchiveMock.SetupGet(p => p.Profits).Returns(new Dictionary<int, decimal>
            {
                { 2022, 50.0m },
                { 2023, 55.0m },
                { 2024, 150.0m }
            });
            _rentedScooterArchiveMock.SetupGet(r => r.RentedScooters).Returns(new List<RentedScooter>
            {
                new RentedScooter("2", DateTime.Now.AddHours(-5), 0.1m) { RentEnd = DateTime.Now}
            });

            decimal result = _rentalCalculatorService.CalculateIncome(2024, false);

            result.Should().Be(150.0m);
        }

        [TestMethod]
        public void CalculateIncome_Invalid_Year_Provided_InvalidYearProvidedException_Expected()
        {
            _profitArchiveMock.SetupGet(p => p.Profits).Returns(new Dictionary<int, decimal>
            {
                { 2022, 50.0m },
                { 2023, 55.0m },
                { 2024, 150.0m }
            });
            _rentedScooterArchiveMock.SetupGet(r => r.RentedScooters).Returns(new List<RentedScooter>
            {
                new RentedScooter("2", DateTime.Now.AddHours(-5), 0.1m) { RentEnd = DateTime.Now}
            });

            Action action = () => _rentalCalculatorService.CalculateIncome(2037, false);

            action.Should().Throw<InvalidYearProvidedException>();
        }
    }
}
