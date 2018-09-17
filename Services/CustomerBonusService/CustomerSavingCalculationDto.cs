namespace OrderFormService
{
    using System.Collections.Generic;

    public class CustomerSavingCalculationDto
    {
        public long CalculationId { get; set; }
        public int? CustomerId { get; set; }
        public string Ipaddress { get; set; }
        public string UserId { get; set; }
        public string Cvr { get; set; }
        public string Email { get; set; }
        public string IndustryType { get; set; }
        public int? Employees { get; set; }
        public string Currency { get; set; }
        public double? CurrencyRate { get; set; }

        public List<CustomerSavingCalculationDetailDto> CustomerSavingCalculationsDetail { get; set; }
    }
}