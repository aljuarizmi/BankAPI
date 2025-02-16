using BankAPI.Data;
using BankAPI.Data.BankModels;
using Microsoft.EntityFrameworkCore;
namespace BankAPI.Services;

public class AccountService{
    private readonly BankContext _context;
    private readonly AccountTypeService accountTypeService;
    private readonly ClientService clientService;
    public AccountService(BankContext context){
        _context=context;
    }
    public async Task<IEnumerable<Account>> GetAllAsync(){
        return await _context.Accounts.ToListAsync();
    }
    public async Task<Account?> GetByIdAsync(int id){
        return await _context.Accounts.FindAsync(id);
    }
    public async Task<Account> CreateAsync(Account account){
        _context.Accounts.Add(account);
       await _context.SaveChangesAsync();
        //Se llama a la funcion getbyid para devolver el id creado
        return account;
    }
    public async Task UpdateAsync(int id,Account account){
        var existingAccount=await GetByIdAsync(id);
        if(existingAccount is not null){
            existingAccount.AccountType=account.AccountType;
            existingAccount.ClientId=account.ClientId;
            existingAccount.Balance=account.Balance;
           await _context.SaveChangesAsync();
        }
    }
    public async Task DeleteAsync(int id){
        var accountToDelete=await GetByIdAsync(id);
        if(accountToDelete is not null){
            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<string> ValidateAccount(Account account){
        string result="Valid";
        var accountType=await accountTypeService.GetByIdAsync(account.AccountType);
        if(accountType is null){
            result=$"El tipo de cuenta {account.AccountType} no existe";
        }
        var clientID=account.ClientId.GetValueOrDefault();
        var client=await clientService.GetByIdAsync(clientID);
        if(client is null){
            result=$"El cliente {clientID} no existe";
        }
        return result;
    }
}