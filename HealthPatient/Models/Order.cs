using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? DoctorId { get; set; }

    public int? ItemId { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ItemsInShop? Item { get; set; }
}
