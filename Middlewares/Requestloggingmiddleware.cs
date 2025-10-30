
using Serilog;
using SerilogILogger = Serilog.ILogger;
using SeriLogEventLevel = Serilog.Events.LogEventLevel;
using System.Diagnostics;

namespace CryptoTrading.API.Middleware
{
    partial class Requestloggingmiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SerilogILogger _logger;
        public Requestloggingmiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Log.ForContext<Requestloggingmiddleware>();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            // 記錄請求開始
            _logger.Information("HTTP {RequestMethod} {RequestPath} started from {RemoteIpAddress}",
            request.Method,
            request.Path,
            context.Connection.RemoteIpAddress);
            try
            {
                await _next(context);
                stopwatch.Stop();

                var statusCode = context.Response.StatusCode;
                var loglevel = GetLogLevel(statusCode);

                _logger.Write(
                    loglevel,
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms",
                    request.Method,
                    request.Path,
                    statusCode,
                    stopwatch.Elapsed.TotalMilliseconds
                );
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                _logger.Error(
                    ex,
                    "HTTP {RequestMethod} {RequestPath} failed after {Elapsed:0.0000} ms with exception: {ExceptionMessage}",
                    request.Method,
                    request.Path,
                    stopwatch.Elapsed.TotalMilliseconds,
                    ex.Message
                );
            
                throw;
            }
        }
        private SeriLogEventLevel GetLogLevel(int statusCode)
        {
            return statusCode switch
            {
                >= 500 => SeriLogEventLevel.Error,
                >= 400 => SeriLogEventLevel.Warning,
                _ => SeriLogEventLevel.Information
            };
        }
    }

}