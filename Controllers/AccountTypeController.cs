using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTO;
namespace BankAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountTypeController:ControllerBase{
    private readonly AccountTypeService accountTypeService;
    public AccountTypeController(AccountTypeService accountTypeService){
        this.accountTypeService=accountTypeService;
    }
    [HttpGet("{id}")]
    public ActionResult<AccountType> GetById(int id){
        var account= accountTypeService.GetById(id);
        if (account is null){
            return NotFound();
        }
        return account;
    }
    [HttpPost]
    public IActionResult Create(AccountTypeDTO accountType){
        var newAccountType = accountTypeService.Create(accountType);
        //Se llama a la funcion getbyid para devolver el id creado
        return CreatedAtAction(nameof(GetById),new {id=newAccountType.Id},newAccountType);
    }
}