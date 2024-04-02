using System;
using System.Collections.Generic;

namespace MatrixWebAPI.Models;

public partial class AccountsUser
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public int UserId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
