namespace ScooterRental
{
    public class ProfitArchive : IProfitArchive
    {
        public Dictionary<int, decimal> Profits { get; set; }

        public ProfitArchive()
        {
            Profits = new Dictionary<int, decimal>();
        }

        public void AddScooterProfit(RentedScooter rentalRecord, decimal result)
        {
            int year = rentalRecord.RentEnd.Year;

            if (Profits.ContainsKey(year))
            {
                Profits[year] += result;
            }
            else
            {
                Profits.Add(year, result);
            }
        }
    }
}