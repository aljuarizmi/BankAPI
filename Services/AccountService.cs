using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTO;
using Microsoft.EntityFrameworkCore;
namespace BankAPI.Services;

public class AccountService{
    private readonly BankContext _context;
    private readonly AccountTypeService accountTypeService;
    private readonly ClientService clientService;
    public AccountService(BankContext context,AccountTypeService accountTypeService,ClientService clientService){
        _context=context;
        this.accountTypeService=accountTypeService;
        this.clientService=clientService;
    }
    public async Task<IEnumerable<Account>> GetAllAsync(){
        return await _context.Accounts.ToListAsync();
    }
    public Account? GetById(int id){
        return _context.Accounts.Find(id);
    }
    public Account Create(AccountDTO account){
        var newAccount=new Account();
        newAccount.AccountType=account.AccountType;
        newAccount.ClientId=account.ClientId;
        newAccount.Balance=account.Balance;
        _context.Accounts.Add(newAccount);
       _context.SaveChangesAsync();
        //Se llama a la funcion getbyid para devolver el id creado
        return newAccount;
    }
    public async Task UpdateAsync(int id,AccountDTO account){
        var existingAccount=GetById(id);
        if(existingAccount is not null){
            existingAccount.AccountType=account.AccountType;
            existingAccount.ClientId=account.ClientId;
            existingAccount.Balance=account.Balance;
           await _context.SaveChangesAsync();
        }
    }
    public async Task DeleteAsync(int id){
        var accountToDelete=GetById(id);
        if(accountToDelete is not null){
            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<string> ValidateAccount(AccountDTO account){
        string result="Valid";
        //var accountType=await accountTypeService.GetByIdAsync(account.AccountType);
        var accountType=accountTypeService.GetById(account.AccountType);
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