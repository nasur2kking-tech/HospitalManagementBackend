using HospitalManagement.API.Middleware;

namespace HospitalManagement.API.Extensions
{
    public static class AppExtensions
    {
        public static void UseApplicationPipeline(
            this WebApplication app)
        {
            var env = app.Environment;

            // =====================================
            // GLOBAL MIDDLEWARE
            // =====================================

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<LoggingMiddleware>();


            // =====================================
            // SWAGGER
            // =====================================

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "HMS API v1");

                c.RoutePrefix = "swagger";
            });


            // =====================================
            // SECURITY
            // =====================================

            if (env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }


            // =====================================
            // ROUTING
            // =====================================

            app.UseRouting();


            // =====================================
            // CORS
            // =====================================

            app.UseCors("AllowFrontend");


            // =====================================
            // AUTHENTICATION
            // =====================================

            app.UseAuthentication();

            app.UseAuthorization();


            // =====================================
            // ENDPOINTS
            // =====================================

            app.MapControllers();

            app.MapHealthChecks("/health");
        }
    }
}