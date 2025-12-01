using CryptoTrading.Core.Interfaces;
using CryptoTrading.Core.Models;
using Elastic.Clients.Elasticsearch;

namespace CryptoTrading.Infrastructure.ES;

public class EslaticsearchService(ElasticsearchClient es) : IEslaticsearchService
{
    private readonly ElasticsearchClient _es = es;

    public async Task SyncToEsAsync(IEnumerable<Kline> klines)
    {
        foreach (var kline in klines)
        {
            //IndexAsync 把一筆 JSON 文件寫入 ES
            await _es.IndexAsync(
                kline,
                i =>
                    //指定 ES 的儲存位置（index名稱）
                    i.Index("binance-klines")
                        //指定 ES 裡這筆資料的 primary key（文件 Id）
                        .Id($"{kline.Symbol}-{kline.Interval}-{kline.OpenTime:O}")
            );
        }
    }
}
