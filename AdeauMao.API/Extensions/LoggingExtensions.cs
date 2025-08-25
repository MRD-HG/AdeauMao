using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace AdeauMao.API.Extensions
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddSerilogLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddSerilog();

            return services;
        }

        public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "AdeauMao.API")
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName);

            // Console logging
            loggerConfiguration.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");

            // File logging
            var logsPath = Path.Combine(builder.Environment.ContentRootPath, "Logs");
            Directory.CreateDirectory(logsPath);

            loggerConfiguration.WriteTo.File(
                path: Path.Combine(logsPath, "adeaumao-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                formatter: new JsonFormatter(),
                restrictedToMinimumLevel: LogEventLevel.Information);

            // Error file logging
            loggerConfiguration.WriteTo.File(
                path: Path.Combine(logsPath, "adeaumao-errors-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 90,
                formatter: new JsonFormatter(),
                restrictedToMinimumLevel: LogEventLevel.Error);

            // Development specific logging
            if (builder.Environment.IsDevelopment())
            {
                loggerConfiguration.MinimumLevel.Debug();
                loggerConfiguration.WriteTo.Debug();
            }

            // Production specific logging
            if (builder.Environment.IsProduction())
            {
                // Add structured logging for production monitoring
                loggerConfiguration.WriteTo.File(
                    path: Path.Combine(logsPath, "adeaumao-audit-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 365,
                    formatter: new JsonFormatter(),
                    restrictedToMinimumLevel: LogEventLevel.Information);
            }

            Log.Logger = loggerConfiguration.CreateLogger();
            builder.Host.UseSerilog();

            return builder;
        }

        public static IApplicationBuilder UseSerilogRequestLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                options.GetLevel = (httpContext, elapsed, ex) => GetLogLevel(httpContext, elapsed, ex);
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
                    diagnosticContext.Set("ClientIP", GetClientIpAddress(httpContext));
                    
                    if (httpContext.User.Identity?.IsAuthenticated == true)
                    {
                        diagnosticContext.Set("UserId", httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                        diagnosticContext.Set("UserName", httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value);
                    }
                };
            });

            return app;
        }

        private static LogEventLevel GetLogLevel(HttpContext httpContext, double elapsed, Exception? ex)
        {
            if (ex != null) return LogEventLevel.Error;

            if (httpContext.Response.StatusCode >= 500) return LogEventLevel.Error;
            if (httpContext.Response.StatusCode >= 400) return LogEventLevel.Warning;
            if (elapsed > 4000) return LogEventLevel.Warning;

            return LogEventLevel.Information;
        }

        private static string? GetClientIpAddress(HttpContext httpContext)
        {
            // Check for forwarded IP first (in case of proxy/load balancer)
            var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }

            var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
            {
                return realIp;
            }

            return httpContext.Connection.RemoteIpAddress?.ToString();
        }
    }

    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred during request processing");
                throw;
            }
            finally
            {
                stopwatch.Stop();

                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    var logLevel = context.Response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
                    
                    _logger.Log(logLevel, 
                        "API Request: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms - User: {User}",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds,
                        context.User.Identity?.Name ?? "Anonymous");
                }
            }
        }
    }
}

