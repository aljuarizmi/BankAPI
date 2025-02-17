using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BankAPI.Data.BankModels;

public partial class AccountType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    [JsonIgnore]
    public DateTime RegDate { get; set; }
    
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
