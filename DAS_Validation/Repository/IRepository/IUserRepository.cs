using DAS_Validation.Models.Dto;

namespace DAS_Validation.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
