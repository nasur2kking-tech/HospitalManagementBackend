using FluentValidation;
using FluentValidation.AspNetCore;

using HospitalManagement.API.Extensions;
using HospitalManagement.Application.Mappings;
using HospitalManagement.Application.Validators;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// CONTROLLERS
// ======================================================

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

// ======================================================
// FLUENT VALIDATION
// ======================================================

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<PatientValidator>();

// ======================================================
// AUTOMAPPER
// ======================================================

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// ======================================================
// APPLICATION + INFRASTRUCTURE SERVICES
// ======================================================

builder.Services.AddApplicationServices(
    builder.Configuration);

// ======================================================
// SWAGGER
// ======================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Hospital Management API",
            Version = "v1"
        });

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Enter JWT token.\n\nExample:\nBearer eyJhbGc..."
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

// ======================================================
// BUILD APPLICATION
// ======================================================

var app = builder.Build();

// ======================================================
// STATIC FILES
// ======================================================

var uploadsPath =
    Path.Combine(
        builder.Environment.ContentRootPath,
        "Uploads");

if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider =
            new PhysicalFileProvider(
                uploadsPath),

        RequestPath = "/Uploads"
    });

// ======================================================
// CUSTOM PIPELINE
// ======================================================

app.UseApplicationPipeline();

// ======================================================
// RUN APPLICATION
// ======================================================

app.Run();