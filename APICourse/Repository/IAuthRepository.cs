using APICourse.Models;

namespace APICourse.Repository
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User user,CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email,CancellationToken cancellationToken);
    }
}
