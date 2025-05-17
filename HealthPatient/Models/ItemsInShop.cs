using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class ItemsInShop
{
    public int IdItem { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Cost { get; set; }

    public int? CountInStock { get; set; }

    public string? Image { get; set; }

    public int? Discount { get; set; }
}
