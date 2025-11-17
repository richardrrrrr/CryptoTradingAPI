using System.Data;
using CryptoTrading.Core.Interfaces;
using CryptoTrading.Core.Entities;
using CryptoTrading.Infrastructure;
using Dapper;
using CryptoTrading.Infrastructure.Data;


namespace CryptoTrading.Infrastructure.Repositories;

public class KlineRepository(CryptoTradingDbContext dbContext) : EFIntegrationBase(dbContext), IKlineRepository
{
    public Task BulkUpsertAsync(IEnumerable<BinanceKline> klines)
    {
       
    }
}