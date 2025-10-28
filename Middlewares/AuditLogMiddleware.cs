using System.Text;

namespace CryptoTrading.API.Middleware
{
    public class AuditLogMiddleware
    {
        //RequestDelegate
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditLogMiddleware> _logger;

        public AuditLogMiddleware (RequestDelegate requestDelegate, ILogger<AuditLogMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

    }
}