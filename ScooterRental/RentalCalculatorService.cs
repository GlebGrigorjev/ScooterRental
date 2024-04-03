using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class RentalCalculatorService : IRentalCalculatorService
    {
        private readonly IRentedScooterArchive _rentedScooterArchive;
        private readonly IProfitArchive _profitArchive;

        public RentalCalculatorService(IRentedScooterArchive rentedScooterArchive,
            IProfitArchive profitArchive)
        {
            _rentedScooterArchive = rentedScooterArchive;
            _profitArchive = profitArchive;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            if (year.HasValue && year > DateTime.Now.Year)
                throw new InvalidYearProvidedException();

            decimal totalSum = 0;

            if (!includeNotCompletedRentals)
            {
                if (year.HasValue)
                    return _profitArchive.Profits.GetValueOrDefault(year.Value, 0);

                return _profitArchive.Profits.Sum(x => x.Value);
            }

            if (!year.HasValue)
            {
                totalSum = _profitArchive.Profits.Sum(x => x.Value);

                foreach (var scooter in _rentedScooterArchive.RentedScooters)
                {
                    totalSum += CalculateRent(scooter);
                }
            }
            else
            {
                totalSum += _profitArchive.Profits.GetValueOrDefault(year.Value, 0);

                if (year == DateTime.Now.Year)
                {
                    foreach (var scooter in _rentedScooterArchive.RentedScooters)
                    {
                        totalSum += CalculateRent(scooter);
                    }
                }
            }

            return totalSum;
        }

        public decimal CalculateRent(RentedScooter rentalRecord)
        {
            var start = rentalRecord.RentStart;
            var end = rentalRecord.RentEnd;
            decimal pricePerMinute = rentalRecord.PricePerMinute;
            DateTime currentDay = start.Date;
            DateTime nextDay = currentDay.AddDays(1);

            decimal totalPrice = 0;

            while (start < end)
            {
                DateTime midnight = nextDay;
                TimeSpan timeDifferenceToMidnight = midnight - start;

                int minutesDifference = (int)Math.Min(timeDifferenceToMidnight.TotalMinutes, (end - start).TotalMinutes);

                decimal priceForCurrentDay = Math.Min(minutesDifference * pricePerMinute, 20);

                totalPrice += priceForCurrentDay;

                start = midnight;
                currentDay = start.Date;
                nextDay = currentDay.AddDays(1);
            }

            _profitArchive.AddScooterProfit(rentalRecord, totalPrice);

            return totalPrice;
        }
    }
}