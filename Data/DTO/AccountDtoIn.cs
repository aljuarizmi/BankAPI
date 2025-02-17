namespace BankAPI.Data.DTO;
public class AccountDtoIn{//Se cambió el nombre de la clase AccountDTO=>AccountDtoIn con click derecho y "Rename Symbol"
    public int Id { get; set; }

    public int AccountType { get; set; }

    public int? ClientId { get; set; }

    public decimal Balance { get; set; }
}