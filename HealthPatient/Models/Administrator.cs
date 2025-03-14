using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Administrator
{
    public int AdministratorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Patronymic { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}
