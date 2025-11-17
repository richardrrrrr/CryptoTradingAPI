--scaffold語法，記得把Entities移到Core裡面 namespace CryptoTrading.Core.Entities;
--dotnet ef dbcontext scaffold Name=ConnectionStrings:DefaultConnection ` Microsoft.EntityFrameworkCore.SqlServer ` -o ../CryptoTrading.Infrastructure/Data ` -c CryptoTradingDbContext ` --context-dir Data ` --force

CREATE TABLE dbo.TestTable
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE()
);

INSERT INTO dbo.TestTable
    (Name)
VALUES
    (N'Alice'),
    (N'Bob'),
    (N'Charlie');

------------------------------------------------------------ K線table
CREATE TABLE [dbo].[BinanceKline] (
    [Id]        BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Symbol]    VARCHAR(20) NOT NULL,
    [Interval]  VARCHAR(10) NOT NULL,

    [OpenTime]  DATETIME2   NOT NULL,
    [CloseTime] DATETIME2   NOT NULL,

    [OpenPrice]  DECIMAL(18, 8) NOT NULL,
    [HighPrice]  DECIMAL(18, 8) NOT NULL,
    [LowPrice]   DECIMAL(18, 8) NOT NULL,
    [ClosePrice] DECIMAL(18, 8) NOT NULL,

    [Volume]     DECIMAL(18, 8) NOT NULL,
    [Trades]     INT NULL,

    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_BinanceKline_CreatedAt DEFAULT (SYSUTCDATETIME())
);

-- 一根K線唯一索引（避免重複）
CREATE UNIQUE INDEX IX_BinanceKline_Symbol_Interval_OpenTime
ON [dbo].[BinanceKline] ([Symbol], [Interval], [OpenTime]);

