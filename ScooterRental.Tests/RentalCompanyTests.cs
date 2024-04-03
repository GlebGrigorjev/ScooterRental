using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalCompanyTests
    {
        private AutoMocker _mocker;
        private RentalCompany _company;
        private Mock<IScooterService> _scooterServiceMock;
        private Mock<IRentedScooterArchive> _rentedScooterArchiveMock;
        private Mock<IRentalCalculatorService> _rentedCalculatorMock;
        private const string _defaultcompanyName = "test";

        [TestInitialize]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _scooterServiceMock = _mocker.GetMock<IScooterService>();
            _rentedScooterArchiveMock = _mocker.GetMock<IRentedScooterArchive>();
            _rentedCalculatorMock = _mocker.GetMock<IRentalCalculatorService>();
            _company = new RentalCompany(_defaultcompanyName, _scooterServiceMock.Object, _rentedScooterArchiveMock.Object, _rentedCalculatorMock.Object);
        }

        [TestMethod]
        public void StartRent_Rent_Existing_ScooterIsRented()
        {
            Scooter scooter = new Scooter("1", 0.2m);
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter);

            _company.StartRent("1");

            scooter.IsRented.Should().BeTrue();
        }

        [TestMethod]
        public void EndRent_StopRent_Existing_ScooterRentStopped()
        {
            Scooter scooter = new Scooter("1", 0.2m) { IsRented = true };
            var now = DateTime.Now;
            var rentalRecord = new RentedScooter(scooter.Id, now.AddMinutes(-20), scooter.PricePerMinute) { RentEnd = now };
            _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(scooter);
            _rentedScooterArchiveMock.Setup(archive => archive.EndRental(scooter.Id, It.IsAny<DateTime>())).Returns(rentalRecord);
            _rentedCalculatorMock.Setup(calculator => calculator.CalculateRent(rentalRecord)).Returns(5);

            var result = _company.EndRent("1");

            scooter.IsRented.Should().BeFalse();
        }
    }
}
