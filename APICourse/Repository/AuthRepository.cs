using APICourse.Data;
using APICourse.Models;
using Microsoft.EntityFrameworkCore;

namespace APICourse.Repository
{
    public sealed class AuthRepository(AppDbContext _context) : IAuthRepository
    {
        public async Task<User?> GetByEmailAsync(string email,CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.", nameof(email));
            }
            return await _context.Users.
                AsNoTracking().FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
        }
        public async Task<User> RegisterAsync(User user,CancellationToken cancellationToken)
        {
            if (user==null)
            {
                throw new ArgumentNullException(nameof(user));  
            }
            await _context.Users.AddAsync(user,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }
    }
}
