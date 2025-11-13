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
