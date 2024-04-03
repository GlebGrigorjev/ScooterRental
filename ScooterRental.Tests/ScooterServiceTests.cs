using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class ScooterServiceTests
    {
        private ScooterService _scooterService;
        private List<Scooter> _scooters;

        [TestInitialize]
        public void Setup()
        {
            _scooters = new List<Scooter>();
            _scooterService = new ScooterService(_scooters);
        }

        [TestMethod]
        public void AddScooter_Valid_Data_Provided_ScooterAdded()
        {
            _scooterService.AddScooter("1", 0.1m);

            _scooters.Count.Should().Be(1);
        }

        [TestMethod]
        public void AddScooter_Invalid_Price_Provided_InvalidePriceException_Expected()
        {
            Action action = () => _scooterService.AddScooter("1", 0.0m);

            action.Should().Throw<InvalidPriceException>();
        }

        [TestMethod]
        public void AddScooter_Invalid_ID_Provided_InvalideIDeException_Expected()
        {
            Action action = () => _scooterService.AddScooter("", 0.11m);

            action.Should().Throw<InvalidIdException>();
        }

        [TestMethod]
        public void AddScooter_Duplicate_Scooter_Provided_DuplicateScooterProvidedException_Expected()
        {
            _scooters.Add(new Scooter("1", 0.25m));

            Action action = () => _scooterService.AddScooter("1", 0.11m);

            action.Should().Throw<DuplicateScooterProvidedException>();
        }

        [TestMethod]
        public void RemoveScooter_Existing_Scooter_Id_Provided_Scooter_Removed()
        {
            _scooters.Add(new Scooter("1", 0.25m));

            _scooterService.RemoveScooter("1");

            _scooters.Count.Should().Be(0);
        }

        [TestMethod]
        public void RemoveScooter_NonExisting_Scooter_Id_Provided_ScooterNotFoundException_Expected()
        {
            Action action = () => _scooterService.RemoveScooter("1");

            action.Should().Throw<ScooterNotFoundException>();

        }

        [TestMethod]
        public void GetScooterById_Valid_Id_Provided_Scooter_Assigned_To_The_Provided_Id_Returned()
        {
            _scooters.Add(new Scooter("1", 0.25m));

            var result = _scooterService.GetScooterById("1");

            result.Should().BeOfType<Scooter>();
        }

        [TestMethod]
        public void GetScooterById_Invalid_InvalideIDeException_Expected()
        {
            Action action = () => _scooterService.GetScooterById("1");

            action.Should().Throw<ScooterNotFoundException>();
        }

        [TestMethod]
        public void GetScooters_If_List_Is_Not_Empty_List_Of_Scooters_To_Be_Returned()
        {
            _scooters.Add(new Scooter("1", 0.25m));
            _scooters.Add(new Scooter("2", 0.5m));

            var result = _scooterService.GetScooters();

            result.Should().BeOfType<List<Scooter>>();
        }

        [TestMethod]
        public void GetScooters_If_List_Is_Empty_ScooterListIsEmptyException_expected()
        {
            Action action = () => _scooterService.GetScooters();

            action.Should().Throw<ScooterListIsEmptyException>();
        }
    }
}