using FluentValidation;
using FluentValidation.AspNetCore;
using HospitalManagement.API.Extensions;
using HospitalManagement.API.Filters;
using HospitalManagement.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// =========================
// 🔹 CONTROLLERS + FILTERS
// =========================
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// =========================
// 🔹 FLUENT VALIDATION
// =========================
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDto>();

// =========================
// 🔹 AUTOMAPPER
// =========================
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// =========================
// 🌐 CORS (🔥 VERY IMPORTANT)
// =========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()   // for now (dev)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// =========================
// 🔥 CORE SERVICES
// =========================
builder.Services.AddApplicationServices(builder.Configuration);

// =========================
// 🚀 BUILD APP
// =========================
var app = builder.Build();

// =========================
// 🔥 PIPELINE
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔥 IMPORTANT ORDER
app.UseCors("AllowFrontend");   // ✅ MUST BE BEFORE AUTH

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();