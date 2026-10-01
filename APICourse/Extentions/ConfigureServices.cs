using APICourse.Data;
using APICourse.Mapping;
using APICourse.Repository;
using APICourse.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;

namespace APICourse.Extentions
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection servces, IConfiguration configuration)
        {
            servces.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return servces;
        }
        public static IServiceCollection JwtAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),

                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        RoleClaimType = ClaimTypes.Role
                    };
                });
            services.AddAuthorization();
            return services;
        }
        public static IServiceCollection AddSwaggerLock(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token"
                });
                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });
            });
            return services;
        }
        public static IServiceCollection ConfigureRepository(this IServiceCollection servces)
        {
            servces.AddScoped<IAuthRepository, AuthRepository>();
            return servces;
        }
        public static IServiceCollection ConfigureService(this IServiceCollection servces)
        {
            servces.AddScoped<IAuthService, AuthService>();
            return servces;
        }
        public static IServiceCollection ConfigureMapping(this IServiceCollection servces) =>
            servces.AddAutoMapper(s => { }, typeof(MappingProfile));
    }
}
