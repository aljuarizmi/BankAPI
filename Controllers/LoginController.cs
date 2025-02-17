using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTO;
namespace BankAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoginController:ControllerBase{
    private readonly LoginService loginService;
    //Constructor
    public LoginController(LoginService loginService){
        this.loginService=loginService;
    }
    [HttpPost("authenticate")]
    public async Task<IActionResult> Login(AdminDto adminDto){
        var admin=await loginService.GetAdmin(adminDto);
        if(admin==null){
            return BadRequest(new{message="Credenciales inválidas."});
        }
        //Generamos el token
        string jwtToken=loginService.GenerateToken(admin);

        return Ok(new{token=jwtToken});
    }
}