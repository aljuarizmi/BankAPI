using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using System.Threading.Tasks;
using BankAPI.Data.DTO;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers;
[Authorize]
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
    [HttpGet]
    public async Task<IEnumerable<AccountDtoOut>> GetAll(){
        return await accountService.GetAll();
    }
    //REST asincrono
    [HttpGet("async/{id}")]
    public async Task<ActionResult<AccountDtoOut>> GetById(int id){
        var account=await accountService.GetDtoBId(id);
        if (account is null){
            return NotFound();
        }
        return account;
    }
    //Servicio asincrono
    [Authorize(Policy="SuperAdmin")]
    [HttpPut("async/{id}")]
    public async Task<IActionResult> UpdateAsync(int id,AccountDtoIn account){
        if(id!=account.Id){
            return BadRequest(new{message=$"El ID {id} de la URL no coincide con el ID({account.Id}) del cuerpo de la solicitud"});
        }
        var accountToUpdate=accountService.GetById(id);
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
    [Authorize(Policy="SuperAdmin")]
    [HttpPost]
    public async Task<IActionResult> Create(AccountDtoIn account){
        string validationResult=await accountService.ValidateAccount(account);
        if(!validationResult.Equals("Valid")){
            return BadRequest(new{message=validationResult});
        }
        var newAccount =accountService.Create(account);
        //Se llama a la funcion getbyid para devolver el id creado
        return CreatedAtAction(nameof(GetById),new {id=newAccount.Id},newAccount);
    }
    [Authorize(Policy="SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id){
        var accountToDelete=accountService.GetById(id);
        if(accountToDelete is not null){
            await accountService.DeleteAsync(id);
            return Ok();
        }else{
            return NotFound();
        }
    }
}