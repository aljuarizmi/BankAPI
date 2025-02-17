using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTO;
using Microsoft.EntityFrameworkCore;
namespace BankAPI.Services;
public class AccountTypeService{
    private readonly BankContext _context;
    public AccountTypeService(BankContext context){
        _context=context;
    }
    public AccountType? GetById(int id){
        return _context.AccountTypes.Find(id);
    }
    public AccountType Create(AccountTypeDTO accountType){
        var newAccountType=new AccountType();
        newAccountType.Name=accountType.Name;
        _context.AccountTypes.Add(newAccountType);
       _context.SaveChangesAsync();
        //Se llama a la funcion getbyid para devolver el id creado
        return newAccountType;
    }
}