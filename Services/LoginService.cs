using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace BankAPI.Services;
public class LoginService{
    private readonly BankContext _context;
    //private readonly LoginService loginService;
    private IConfiguration config;
    public LoginService(BankContext context,IConfiguration config){
        this._context=context;
        //this.loginService=loginService;
        this.config=config;
    }
    public async Task<Administrator?> GetAdmin(AdminDto admin){
        return await _context.Administrators.SingleOrDefaultAsync(x => x.Email==admin.Email && x.Pwd==admin.Pwd);
    }
    public string GenerateToken(Administrator admin){
        //definimos un arreglo claim de objetos tipo Claim
        var claims=new[]{
            new Claim(ClaimTypes.Name,admin.Name),
            new Claim(ClaimTypes.Email,admin.Email)
        };
        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
        var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);
        var securityToken=new JwtSecurityToken(
            claims:claims,
            expires:DateTime.Now.AddMinutes(60),
            signingCredentials:creds
        );
        string token=new JwtSecurityTokenHandler().WriteToken(securityToken);
        return token;
    }
}