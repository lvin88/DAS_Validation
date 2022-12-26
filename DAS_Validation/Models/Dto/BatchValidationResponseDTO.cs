namespace DAS_Validation.Models.Dto
{
    public class BatchValidationResponseDTO
    {
        public List<TicketDTO> TicketDTO { get; set; }
        public object Error { get; set; }
    }
}
