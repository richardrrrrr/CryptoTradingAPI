# 🧱 CryptoTrading.Backtester — Architecture & Development Workflow  
加密貨幣回測系統 — 架構與開發流程文件  
_Last updated: 2025-10-27_

---

## 🧩 1. Design Goals 設計目標

### ✅ 需要（此階段目標）
- ✅ 從 Binance API 拉取 K 線資料（歷史資料）
- ✅ 高效能的回測引擎
- ✅ 可量測的效能分析點（記錄各階段執行時間與資源使用）
- ✅ 高可測試性（能對策略與回測結果進行單元測試）

---

## 🧱 2. System Overview 系統架構總覽

CryptoTrading/
├── CryptoTrading.Core/                     # 🔥 核心邏輯（純商業邏輯，不依賴外部）
│   ├── Backtest/
│   │   ├── BacktestEngine.cs              # 控制整個回測流程
│   │   ├── StrategyBase.cs                # 所有策略的基底類別
│   │   ├── MovingAverageStrategy.cs       # 範例策略（均線策略）
│   │   └── PerformanceAnalyzer.cs         # 分析報酬、夏普值、回撤等績效
│   │
│   ├── Models/
│   │   ├── Candle.cs                      # 單根 K 線資料結構
│   │   ├── TradeRecord.cs                 # 單筆交易記錄
│   │   └── BacktestResult.cs              # 回測結果彙總
│   │
│   ├── Utils/
│   │   ├── PerformanceTimer.cs            # 測量執行時間工具
│   │   └── MathHelper.cs                  # 數學與統計輔助函式
│
├── CryptoTrading.Infrastructure/          # 🌐 外部資源（API、DB、Cache、Logging）
│   ├── Binance/
│   │   ├── BinanceApiClient.cs            # 呼叫 Binance API
│   │   ├── BinanceEndpoints.cs            # 統一管理 URL
│   │   └── BinanceDataMapper.cs           # JSON 轉成 Candle 模型
│   │
│   ├── Http/
│   │   └── HttpHelper.cs                  # 包裝 HttpClient 請求（含錯誤/重試/Log）
│   │
│   ├── Logging/
│   │   ├── NLog.config                    # NLog 設定檔（集中管理 log 輸出）
│   │   └── LoggingExtensions.cs           # NLog 啟動設定與依賴注入擴充
│   │
│   └── Exceptions/
│       └── ApiException.cs                # 自訂例外（包裝 API 錯誤）
│
├── CryptoTrading.WebApi/                  # 🌐 Web 入口層（提供 API、Middleware）
│   ├── Middlewares/
│   │   ├── RequestLoggingMiddleware.cs    # 記錄請求/回應與錯誤
│   │   └── ErrorHandlingMiddleware.cs     # 捕捉例外，統一回傳 JSON 錯誤格式
│   │
│   ├── Controllers/
│   │   └── BinanceController.cs           # 範例：對外暴露 Binance 查詢 API
│   │
│   ├── appsettings.json                   # 環境設定
│   ├── Program.cs                         # 程式進入點（註冊 DI / Middleware / Serilog）
│   └── Serilog.config                     # WebAPI 專用 log 設定（引用 Infrastructure/Serilog.config）
│
└── Tests/                                 # 🧪 單元測試與整合測試
    ├── BacktestEngineTests.cs             # 測試策略與回測邏輯
    ├── BinanceApiTests.cs                 # 測試 API 呼叫與資料轉換
    └── MiddlewareTests.cs                 # 測試 Middleware 行為與 log 輸出


## 🧠 3.核心模組設計說明
| 模組                      | 功能                             | 備註              |
| ----------------------- | ------------------------------ | --------------- |
| **BinanceApiClient**    | 呼叫 Binance REST API，拉取歷史 K 線資料 | 使用 `HttpClient` |
| **BinanceDataMapper**   | 將 JSON array 轉成 `List<Candle>` | 封裝資料轉換邏輯        |
| **BacktestEngine**      | 控制整個回測流程                       | 可注入策略           |
| **StrategyBase**        | 定義策略介面（`GenerateSignal`）       | 可換策略            |
| **PerformanceAnalyzer** | 計算回測績效                         | 報酬率、最大回撤等       |
| **PerformanceTimer**    | 測量回測效能                         | stopwatch-based |

## 🧩4. 核心模組設計說明
| 模組                      | 功能                             | 備註              |
| ----------------------- | ------------------------------ | --------------- |
| **BinanceApiClient**    | 呼叫 Binance REST API，拉取歷史 K 線資料 | 使用 `HttpClient` |
| **BinanceDataMapper**   | 將 JSON array 轉成 `List<Candle>` | 封裝資料轉換邏輯        |
| **BacktestEngine**      | 控制整個回測流程                       | 可注入策略           |
| **StrategyBase**        | 定義策略介面（`GenerateSignal`）       | 可換策略            |
| **PerformanceAnalyzer** | 計算回測績效                         | 報酬率、最大回撤等       |
| **PerformanceTimer**    | 測量回測效能                         | stopwatch-based |

## 5. 資料流
flowchart TD
    A[BinanceApiClient] -->|REST JSON| B[BinanceDataMapper]
    B -->|List<Candle>| C[BacktestEngine]
    C --> D[StrategyBase]
    D --> E[TradeRecord]
    E --> F[PerformanceAnalyzer]
    F -->|Result| G[ConsoleRunner]
## 🚀6. 開發流程建議
| 階段                      | 目標                                         | 任務                                          |
| ----------------------- | ------------------------------------------ | ------------------------------------------- |
| **1️⃣ 初始化專案架構**         | 分出 `Core`、`Infrastructure`、`ConsoleRunner` | 建立 solution 並加入專案參考                         |
| **2️⃣ Binance API 客戶端** | 從 Binance 取得歷史資料                           | 撰寫 `BinanceApiClient` + `BinanceDataMapper` |
| **3️⃣ 策略引擎**            | 實作 `StrategyBase` 與一個基本策略（如均線）             | 建立 `IStrategy` 與策略實例                        |
| **4️⃣ 回測模組**            | 撰寫 `BacktestEngine` + 效能分析                 | 加入 Stopwatch 或 BenchmarkDotNet              |
| **5️⃣ 效能測試**            | 分析回測瓶頸                                     | 使用 `PerformanceTimer.MeasureAsync()`        |
| **6️⃣ 結果報告**            | 計算總報酬、勝率、最大回撤                              | 實作 `PerformanceAnalyzer`                    |
| **7️⃣ 擴充方向**            | 支援多幣種 / 多策略                                | 可加入併發處理與快取機制                                |
## 🧮 7. 效能優化方向
| 問題           | 解法                          |
| ------------ | --------------------------- |
| API 資料太多、拉取慢 | 分段請求 + 並行下載（`Task.WhenAll`） |
| 計算過程 CPU 吃重  | 使用 `Parallel.ForEach` 或分批處理 |
| IO 過多        | 將 Candle 快取到本地 JSON 檔       |
| 想持續分析效能      | 加入 `BenchmarkDotNet` 專案     |

## ⚙️ 8. Performance Analysis 效能分析建議

| 🧩 目標 / 層面        | 🎯 分析重點                    | 🧰 推薦工具 / 方法                                                   | 💡 說明與應用場景                                                                 |
| ----------------- | -------------------------- | -------------------------------------------------------------- | -------------------------------------------------------------------------- |
| **CPU / 計算效能**    | 找出最耗時的函式、演算法、策略運算是否可平行化    | `BenchmarkDotNet`, `dotnet-trace`, **Visual Studio Profiler**  | 比較不同策略的速度、計算瓶頸與平行化可行性。BenchmarkDotNet 適合做微觀比較；VS Profiler 適合整體分析。          |
| **記憶體 / GC 分析**   | 檢查哪裡頻繁分配導致 GC、是否有暫存物件重複建立  | `dotnet-counters`, `dotMemory`, **PerfView**                   | 觀察回測過程的 GC 次數、記憶體峰值。特別適合偵測 Candle 物件或 List<> 重複配置問題。                       |
| **I/O / 網路效能**    | Binance API 呼叫延遲、JSON 解析開銷 | `dotnet-trace`, `HttpClientFactory`, `System.Diagnostics`      | 檢查 API 請求時間與 JSON 反序列化耗時。可利用 `System.Diagnostics.Activity` 標記 API latency。 |
| **整體 CPU/Memory** | 監控回測運行時的 CPU / 記憶體變化       | `dotnet trace`, `dotnet-counters`                              | 分析整體效能瓶頸、找出是否 CPU-bound 或 memory-bound。可長時間監控整個回測過程。                       |
| **方法層級耗時分析**      | 檢查哪個方法最耗時                  | `dotnet-counters monitor`                                      | 以即時 CLI 方式顯示當前最耗資源的函式區塊。適合快速定位 Hot Path。                                   |
| **微觀時間分析**        | 測量單一策略或單一方法執行時間            | `Stopwatch`（封裝為 `PerformanceTimer`）                            | 用於細節級效能分析，例如策略運算迴圈的平均耗時。低干擾、可即時印出結果。                                       |
| **批次效能測試 / 策略對比** | 比較多個策略或參數組合的執行速度           | `BenchmarkDotNet` 或自寫迴圈 + 平均耗時計算                               | 用於策略優化與多組測試，比較「不同參數組合」的平均回測時間與資源消耗。                                        |
| **平行化與多執行緒效率**    | 檢測多任務並行回測的 CPU 利用率         | `dotnet-counters`, `System.Threading.Channels`                 | 驗證是否有效利用多核心 CPU。觀察 ThreadPool 與 Task 負載情況。                                 |
| **系統級長期監控（部署後）**  | 長期監控服務 CPU、Memory、IO 狀態    | Windows Performance Monitor, **Grafana + Prometheus / Zabbix** | 若未來部署成長時間服務（例如自動回測排程），可透過系統級監控追蹤效能趨勢。                                      |


## ✅ 9. Summary 總結

| 面向         | 建議                                    |
| ---------- | ------------------------------------- |
| 是否需要 DB？   | ❌ 不需要。直接用 Binance API 拿資料即可。          |
| API 資料取得位置 | Infrastructure 層（與外部世界互動）             |
| 回測邏輯放哪？    | Core 層（純演算法與邏輯）                       |
| 效能測試在哪做？   | ConsoleRunner 層（可視化 + Stopwatch）      |
| 未來要擴展？     | 加上 WebAPI / Scheduler / ReportService |
