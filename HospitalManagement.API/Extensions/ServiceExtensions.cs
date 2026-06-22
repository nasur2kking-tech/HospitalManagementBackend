using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Application.Services;

using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Identity;
using HospitalManagement.Infrastructure.Repositories.Implementations;
using HospitalManagement.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace HospitalManagement.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // =====================================
            // DATABASE
            // =====================================

            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception(
                    "Database connection string is missing.");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)));

            // =====================================
            // REPOSITORIES
            // =====================================

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPatientRepository, PatientRepository>();

            services.AddScoped<IDoctorRepository, DoctorRepository>();

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            services.AddScoped<IBillingRepository, BillingRepository>();

            services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();

            services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();

            // =====================================
            // SERVICES
            // =====================================

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IPatientService, PatientService>();

            services.AddScoped<IDoctorService, DoctorService>();

            services.AddScoped<IAppointmentService, AppointmentService>();

            services.AddScoped<IBillingService, BillingService>();

            services.AddScoped<IMedicalRecordService, MedicalRecordService>();

            services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();

            services.AddScoped<IReportService, ReportService>();

            // =====================================
            // SECURITY
            // =====================================

            services.AddScoped<IJwtGenerator, JwtGenerator>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            var jwtSection = configuration.GetSection("Jwt");

            var key =
                Encoding.UTF8.GetBytes(jwtSection["Key"]!);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.SaveToken = true;

                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,

                            ValidateIssuer = true,

                            ValidateAudience = true,

                            ValidateLifetime = true,

                            ValidIssuer =
                                jwtSection["Issuer"],

                            ValidAudience =
                                jwtSection["Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(key),

                            ClockSkew =
                                TimeSpan.Zero
                        };
                });

            services.AddAuthorization();

            // =====================================
            // CORS
            // =====================================

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowFrontend",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // =====================================
            // HEALTH CHECKS
            // =====================================

            services.AddHealthChecks();

            return services;
        }
    }
}