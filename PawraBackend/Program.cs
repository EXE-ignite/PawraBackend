
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Service;
using Pawra.DAL;
using Pawra.DAL.Data;
using Pawra.DAL.Repository;
using System.Text;

namespace PawraBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Database Configuration
            var conString = builder.Configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<PawraDBContext>(options =>
                options.UseNpgsql(conString, 
                    b => b.MigrationsAssembly("Pawra.DAL")));

            // JWT Authentication Configuration
            var jwtKey = builder.Configuration["JwtSettings:Key"] 
                ?? throw new InvalidOperationException("JWT Key not found in configuration.");
            var key = Encoding.UTF8.GetBytes(jwtKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = System.Security.Claims.ClaimTypes.Role,
                    NameClaimType = System.Security.Claims.ClaimTypes.Name
                };

                // Debug logging for JWT events
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].ToString();
                        Console.WriteLine($"JWT Message Received. Auth Header: {authHeader}");
                        
                        // Custom token extraction
                        if (!string.IsNullOrEmpty(authHeader))
                        {
                            // Remove 'Bearer ' prefix if exists (case-insensitive)
                            var token = authHeader;
                            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                token = token.Substring(7);
                            }
                            // Remove quotes if they exist
                            token = token.Trim('\'', '"', ' ');
                            
                            if (!string.IsNullOrEmpty(token) && token.Contains("."))
                            {
                                context.Token = token;
                                Console.WriteLine($"JWT Token extracted: {token.Substring(0, Math.Min(50, token.Length))}...");
                            }
                        }
                        
                        Console.WriteLine($"JWT Token final: {(string.IsNullOrEmpty(context.Token) ? "NULL/EMPTY" : "SET")}");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"JWT Authentication Failed: {context.Exception.Message}");
                        Console.WriteLine($"JWT Exception Stack: {context.Exception.StackTrace}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
                        Console.WriteLine($"JWT Token Validated. Claims: {string.Join(", ", claims ?? Array.Empty<string>())}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine($"JWT Challenge - Error: '{context.Error}', ErrorDescription: '{context.ErrorDescription}', AuthFailure: {context.AuthenticateFailure?.Message}");
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();

            // AutoMapper Configuration
            builder.Services.AddAutoMapper(typeof(Pawra.BLL.Mappings.MappingProfile));

            // Register UnitOfWork
            builder.Services.AddScoped<Pawra.DAL.Interfaces.IUnitOfWork, Pawra.DAL.UnitOfWork.UnitOfWork>();

            // Register Repositories (optional - if used directly)
            builder.Services.AddScoped(typeof(BaseRepository<>));
            builder.Services.AddScoped<Pawra.DAL.Interfaces.IAccountRoleRepository, Pawra.DAL.Repository.AccountRoleRepository>();

            // Register Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAccountRoleService, AccountRoleService>();

            // Swagger Configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header sử dụng Bearer scheme. Nhập 'Bearer' [space] và sau đó nhập token của bạn.",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            // Auto-migrate and seed database in development
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PawraDBContext>();
                    try
                    {
                        dbContext.Database.Migrate();
                        dbContext.EnsureSeedData();
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogWarning(ex, "Could not run migrations. Database may need to be set up manually or user needs appropriate permissions.");
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
