using CryptoTrading.API.Middleware;
using CryptoTrading.Data;
using CryptoTrading.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/startup-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Log.Information("🚀 Starting application setup...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddUserSecrets<Program>();

    builder.Services.AddDbContext<CryptoTradingDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
    // 這裡再掛上正式 Logger（會取代前面的 Bootstrap）
    builder.Host.UseSerilog(
        (context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .WriteTo.Console()
                .WriteTo.File("logs/runtime-.txt", rollingInterval: RollingInterval.Day);
        }
    );

    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHttpsRedirection();
    }

    app.UseSerilogRequestLogging();
    app.UseErrorHandling();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("✅ Application running in {Environment}", app.Environment.EnvironmentName);
    app.Run();
}
catch (Exception ex)
{
    // (3) 這裡的 Log.Fatal() 用的還是 Bootstrap Logger → 能記錄啟動階段的例外
    Log.Fatal(ex, "💥 Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
