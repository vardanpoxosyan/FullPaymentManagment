using APICourse.Data;
using APICourse.DTO;
using APICourse.Models;
using APICourse.Repository;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APICourse.Service
{
    public sealed class AuthService(IAuthRepository authRepository,IMapper mapper,IConfiguration configuration) : IAuthService
    {
        public async Task<LoginResponseDto> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            var user = await authRepository.GetByEmailAsync(loginDTO.Email, cancellationToken);
            if (user == null)
            {
                throw new Exception("can't null");
            };
            var checkPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password,user.PasswordHash);
            if (!checkPassword)
            {
                throw new Exception("Invalid Email or Password");
            }
            var token = GenarateToken(user);
            return new LoginResponseDto { AccessToken = token, Role = user.Role, IsSuccess = true };
        }
        private string GenarateToken(User user)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role),
            };
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claim,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpirationMinutes"]))
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<RegisterResponseDto> RegisterAsync(RegisterDTO registerDTO,CancellationToken cancellationToken)
        {
            var findUser = await authRepository.GetByEmailAsync(registerDTO.Email,cancellationToken);
            if (findUser != null)
            {
                throw new Exception("User Already Exists");
            }
            var user = new User
            {
                UserName= registerDTO.UserName,
                Email=registerDTO.Email,
                PasswordHash=BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
            };
            await authRepository.RegisterAsync(user,cancellationToken);
            var respone=mapper.Map<RegisterResponseDto>(user);
            return respone;
        }
    }
}
