namespace CodeChallengeGrupoZap.Domain.Entities
{
    public class PricingInfos
    {
        public string Period { get; set; }
        public string YearlyIptu { get; set; }
        public double Price { get; set; }
        public double RentalTotalPrice { get; set; }
        public string BusinessType { get; set; }
        public int MonthlyCondoFee { get; set; }
    }
}