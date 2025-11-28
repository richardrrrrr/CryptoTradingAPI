using System.Data.Common;
using CryptoTrading.Infrastructure.Data;
using Dapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrading.Infrastructure;

public class EFIntegrationBase
{
    protected readonly CryptoTradingDbContext _dbContext;
    protected readonly DbConnection _conn;

    public EFIntegrationBase(CryptoTradingDbContext dbContext)
    {
        _dbContext = dbContext;
        // _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _conn = _dbContext.Database.GetDbConnection();
        // 讓 Dapper 能正確處理 C# 的 DateOnly TimeOnly
        SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new SqlTimeOnlyTypeHandler());
    }

    public virtual async Task BulkInsert<T>(IEnumerable<T> data)
        where T : class
    {
        var bulkconfig = new BulkConfig
        {
            BatchSize = 50000,
            BulkCopyTimeout = 1500,
            SqlBulkCopyOptions = EFCore.BulkExtensions.SqlBulkCopyOptions.KeepIdentity,
        };
        using var trans = _dbContext.Database.BeginTransaction();
        await _dbContext.BulkInsertAsync(data, bulkconfig);
        trans.Commit();
    }
}
