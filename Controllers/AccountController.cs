using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using System.Threading.Tasks;

namespace BankAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController:ControllerBase{
    private readonly AccountService accountService;
    private readonly AccountTypeService accountTypeService;
    private readonly ClientService clientService;
    public AccountController(AccountService accountService,AccountTypeService accountTypeService,ClientService clientService){
        this.accountService=accountService;
        this.accountTypeService=accountTypeService;
        this.clientService=clientService;
    }
    //REST asincrono
    [HttpGet("async")]
    public async Task<IEnumerable<Account>> GetAllAsync(){
        return await accountService.GetAllAsync();
    }
    //REST asincrono
    [HttpGet("async/{id}")]
    public async Task<ActionResult<Account>> GetByIdAsync(int id){
        var account=await accountService.GetByIdAsync(id);
        if (account is null){
            return NotFound();
        }
        return account;
    }
    //Servicio asincrono
    [HttpPut("async/{id}")]
    public async Task<IActionResult> UpdateAsync(int id,Account account){
        if(id!=account.Id){
            return BadRequest(new{message=$"El ID {id} de la URL no coincide con el ID({account.Id}) del cuerpo de la solicitud"});
        }
        var accountToUpdate=await accountService.GetByIdAsync(id);
        if(accountToUpdate is not null){
            string validationResult=await accountService.ValidateAccount(account);
            if(!validationResult.Equals("Valid")){
                return BadRequest(new{message=validationResult});
            }
            await accountService.UpdateAsync(id,account);
            return NoContent();
        }else{
            return NotFound();
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync(Account account){
        string validationResult=await accountService.ValidateAccount(account);
        if(!validationResult.Equals("Valid")){
            return BadRequest(new{message=validationResult});
        }
        var newAccount =await accountService.CreateAsync(account);
        //Se llama a la funcion getbyid para devolver el id creado
        return CreatedAtAction(nameof(GetByIdAsync),new {id=newAccount.Id},newAccount);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id){
        var accountToDelete=await accountService.GetByIdAsync(id);
        if(accountToDelete is not null){
            await accountService.DeleteAsync(id);
            return Ok();
        }else{
            return NotFound();
        }
    }
}