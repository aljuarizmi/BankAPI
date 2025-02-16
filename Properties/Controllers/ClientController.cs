using Microsoft.AspNetCore.Mvc;
using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class ClientController:ControllerBase{
    private readonly BankContext _context;
    //Constructor
    public ClientController(BankContext context){
        _context=context;
    }
    [HttpGet]
    public IEnumerable<Client> Get(){
        return _context.Clients.ToList();
    }
    [HttpGet("{id}")]
    public ActionResult<Client> GetById(int id){
        var client=_context.Clients.Find(id);
        if (client is null){
            return NotFound();
        }
        return client;
    }
    [HttpPost]
    public IActionResult Create(Client client){
        _context.Clients.Add(client);
        _context.SaveChanges();
        //Se llama a la funcion getbyid para devolver el id creado
        return CreatedAtAction(nameof(GetById),new {id=client.Id},client);
    }
    [HttpPut]
    public IActionResult Update(int id,Client client){
        if(id!=client.Id){
            return BadRequest();
        }
        var existingCLient=_context.Clients.Find(id);
        if(existingCLient is null){
            return NotFound();
        }
        existingCLient.Name=client.Name;
        existingCLient.PhoneNumber=client.PhoneNumber;
        existingCLient.Email=client.Email;
        _context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id,Client client){
        var existingCLient=_context.Clients.Find(id);
        if(existingCLient is null){
            return NotFound();
        }
        _context.Clients.Remove(existingCLient);
        _context.SaveChanges();
        return Ok();
    }
}