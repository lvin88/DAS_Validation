namespace DAS_Validation.Models.Dto
{
    public class TicketDTO
    {
        public string UserID { get; set; }
        public string Barcode { get; set; }
        public double PrizeAmount { get; set; }
        public double TaxAmount { get; set; }
        public string ValidationResult { get; set; }
    }
}
