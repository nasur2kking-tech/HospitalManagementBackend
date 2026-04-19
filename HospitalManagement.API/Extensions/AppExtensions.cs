using HospitalManagement.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace HospitalManagement.API.Extensions
{
    public static class AppExtensions
    {
        public static void UseApplicationPipeline(this WebApplication app)
        {
            var env = app.Environment;

            // =========================
            // 🔥 GLOBAL MIDDLEWARE
            // =========================
            app.UseMiddleware<ExceptionMiddleware>(); // MUST be FIRST
            app.UseMiddleware<LoggingMiddleware>();

            // =========================
            // 🔹 SWAGGER (ONLY DEV)
            // =========================
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HMS API v1");
                    c.RoutePrefix = "swagger";
                });
            }

            // =========================
            // 🔹 SECURITY
            // =========================
            app.UseHttpsRedirection();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            // =========================
            // 🔹 CORS
            // =========================
            app.UseCors("AllowFrontend");

            // =========================
            // 🔹 AUTH
            // =========================
            app.UseAuthentication();
            app.UseAuthorization();

            // =========================
            // 🔹 ENDPOINTS
            // =========================
            app.MapControllers();
            app.MapHealthChecks("/health");
        }
    }
}
