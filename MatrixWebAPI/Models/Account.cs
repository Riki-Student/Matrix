using System;
using System.Collections.Generic;

namespace MatrixWebAPI.Models;

public partial class Account
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Website { get; set; }

    public virtual ICollection<AccountsUser> AccountsUsers { get; set; } = new List<AccountsUser>();
}
