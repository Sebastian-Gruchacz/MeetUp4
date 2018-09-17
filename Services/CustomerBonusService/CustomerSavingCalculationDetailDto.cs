namespace OrderFormService
{
    public class CustomerSavingCalculationDetailDto
    {
        public long CalculationDetailId { get; set; }
        public long CalculationId { get; set; }
        public int? PotentialModelId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }
        public bool IsNegotiable { get; set; }
        public double? NegotiatedTimeSaving { get; set; }
        public double? NonNegotiatedTimeSaving { get; set; }
        public double? AvgCostPerHour { get; set; }
        public double? AvgConsumptionPerEmployee { get; set; }
        public double? TotalAvgConsumption { get; set; }
        public double? TotalAvgConsumptionRounded { get; set; }
        public double? NegotiatedSavingPercentage { get; set; }
        public double? NonNegotiatedSavingPercentage { get; set; }
    }
}