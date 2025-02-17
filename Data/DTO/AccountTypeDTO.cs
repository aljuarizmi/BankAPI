namespace BankAPI.Data.DTO;
public class AccountTypeDTO{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? RegDate { get; set; }
}