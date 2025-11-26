using System;
using Binance.Net.Clients;
using CryptoExchange.Net.Authentication;
using CryptoTrading.API.Services;
using CryptoTrading.Core.Interfaces;
using CryptoTrading.Infrastructure.Binance;
using CryptoTrading.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bn = Binance.Net;

namespace CryptoTrading.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Strongly typed configuration that allows injection of any class without IOptions
        /// 綁定設定檔區段並直接註冊為 Singleton，可直接注入設定物件。
        /// </summary>
        /// <typeparam name="TOptions">設定類別型別</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>回傳已綁定的設定物件</returns>
        public static TOptions ConfigureInfrastructureOptions<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration
        )
            where TOptions : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var config = new TOptions();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }

        /// <summary>
        /// Strongly typed configuration overload that supports manual configuration via lambda.
        /// 允許使用 Action<TOptions> 動態設定。
        /// </summary>
        /// <typeparam name="TOptions">設定類別型別</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configureOptions">設定配置動作</param>
        /// <returns>IServiceCollection 以支援鏈式呼叫</returns>
        public static IServiceCollection ConfigureInfrastructureOptions<TOptions>(
            this IServiceCollection services,
            Action<TOptions> configureOptions
        )
            where TOptions : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            if (Activator.CreateInstance(typeof(TOptions)) is TOptions options)
            {
                configureOptions(options);
                services.AddSingleton(options);
                return services;
            }

            throw new Exception($"{typeof(TOptions).FullName} is invalid!");
        }

        /// <summary>
        /// 註冊 Infrastructure 層相關服務，例如 Binance、DB、Cache 等。
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">應用設定</param>
        public static void AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddOptions<BinanceOptions>()
                .Bind(configuration.GetSection("BinanceOptions"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton(sp =>
                sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<BinanceOptions>>().Value
            );
            // ✅ 手動註冊 BinanceRestClient
            services.AddSingleton<BinanceRestClient>(sp =>
            {
                var opt = sp.GetRequiredService<BinanceOptions>();
                var creds = new ApiCredentials(opt.ApiKey, opt.ApiSecret);

                var client = new BinanceRestClient(options =>
                {
                    options.ApiCredentials = creds;
                    options.Environment = opt.UseTestnet
                        ? Bn.BinanceEnvironment.Testnet
                        : Bn.BinanceEnvironment.Testnet;
                });

                return client;
            });

            // ✅ 註冊自定義 BinanceService
            services.AddScoped<IBinanceService, BinanceService>();
        }

        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IBinanceDataApplicationService, BinanceDataApplicationService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBinanceRepository, BinanceRepository>();
        }
    }
}
