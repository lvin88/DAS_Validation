using DAS_Validation.Models;
using DAS_Validation.Models.Dto;

namespace DAS_Validation.Repository.IRepository
{
    public interface ITicketRepository
    {
        Task<ValidationResponseDTO> Validate(ValidationRequestDTO validationRequestDTO);
        Task<BatchValidationResponseDTO> ValidateBatch(BatchValidationRequestDTO batchValidationRequestDTO);
    }
}
