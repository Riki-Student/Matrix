using System;
using System.Collections.Generic;

namespace MatrixWebAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<AccountsUser> AccountsUsers { get; set; } = new List<AccountsUser>();
}
