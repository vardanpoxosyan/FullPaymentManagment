using APICourse.DTO;

namespace APICourse.Service
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDTO registerDTO,CancellationToken cancellationToken);
        Task<LoginResponseDto> LoginAsync(LoginDTO loginDTO,CancellationToken cancellationToken);
    }
}
