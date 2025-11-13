namespace CryptoTrading.API.Middleware
{
    public static class ApiMiddlewareExtensions
    {
        /// <summary>
        /// 添加全域錯誤處理中介軟體
        /// </summary>
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        /// <summary>
        /// 添加 Serilog 請求日誌中介軟體
        /// </summary>
        public static IApplicationBuilder UseSerilogRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
