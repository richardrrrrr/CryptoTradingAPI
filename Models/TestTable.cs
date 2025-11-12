using System;
using System.Collections.Generic;

namespace CryptoTrading.Models;

public partial class TestTable
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}


