using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentedScooterArchiveTests
    {
        private RentedScooterArchive _archive;
        private List<RentedScooter> _rentedScooterList;

        [TestInitialize]
        public void Setup()
        {
            _archive = new RentedScooterArchive();
            _rentedScooterList = new List<RentedScooter>();
            _archive.RentedScooters = _rentedScooterList;
        }

        [TestMethod]
        public void AddrentedScooter_Scooter_Provided_List_Of_Rented_Scooters_Should_Not_Be_Empty()
        {
            RentedScooter rentedScooterTest = new RentedScooter("1", DateTime.Now, 0.2m);

            _archive.AddrentedScooter(rentedScooterTest);
            var result = _rentedScooterList.Count();

            result.Should().NotBe(0);
        }

        [TestMethod]
        public void EndRental_Valid_Id_Provided_RentedScooter_Returned()
        {
            RentedScooter rentedScooterTest1 = new RentedScooter("1", DateTime.Now, 0.2m);
            RentedScooter rentedScooterTest2 = new RentedScooter("2", DateTime.Now, 0.5m);
            _archive.AddrentedScooter(rentedScooterTest1);
            _archive.AddrentedScooter(rentedScooterTest2);

            var result = _archive.EndRental("1", DateTime.Now.AddMinutes(20));

            result.Should().BeOfType<RentedScooter>();
        }

        [TestMethod]
        public void EndRental_Invalid_Id_Provided_ScooterNotFoundException_Expected()
        {
            Action action = () => _archive.EndRental("1", DateTime.Now.AddMinutes(20));

            action.Should().Throw<ScooterNotFoundException>();
        }
    }
}
