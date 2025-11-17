using System;
using System.Collections.Generic;

namespace CryptoTrading.Core.Entities;

public partial class TestTable
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
