using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankAPI.Data.BankModels;

public partial class Client
{
    public Client(){
        Accounts=new HashSet<Account>();
    }
    public int Id { get; set; }
    [MaxLength(200,ErrorMessage ="El nombre debe ser menor o igual a 40 caracteres")]
    public string Name { get; set; } = null!;
    [MaxLength(40,ErrorMessage ="El número de telefono debe ser menor o igual a 40 caracteres")]
    public string PhoneNumber { get; set; } = null!;
    [MaxLength(50,ErrorMessage ="El mail debe ser menor o igual a 50 caracteres")]
    [EmailAddress(ErrorMessage ="El formato de correo es incorrecto")]
    public string? Email { get; set; }

    public DateTime RegDate { get; set; }
    [JsonIgnore]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
