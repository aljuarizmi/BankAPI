using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;

namespace BankAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class ClientController:ControllerBase{
    private readonly ClientService _service;
    //Constructor
    public ClientController(ClientService client){
        _service=client;
    }
    [HttpGet]
    public IEnumerable<Client> GetAll(){
        return _service.GetAll();
    }
    [HttpGet("{id}")]
    public ActionResult<Client> GetById(int id){
        var client=_service.GetById(id);
        if (client is null){
            return NotFound();
        }
        return client;
    }
    [HttpPost]
    public IActionResult Create(Client client){
        var newClient =_service.Create(client);
        //Se llama a la funcion getbyid para devolver el id creado
        return CreatedAtAction(nameof(GetById),new {id=newClient.Id},newClient);
    }
    [HttpPut]
    public IActionResult Update(int id,Client client){
        if(id!=client.Id){
            return BadRequest();
        }
        var clientToUpdate=_service.GetById(id);
        if(clientToUpdate is not null){
            _service.Update(id,client);
            return NoContent();
        }else{
            return NotFound();
        }

    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id){
        var clientToDelete=_service.GetById(id);
        if(clientToDelete is not null){
            _service.Delete(id);
            return Ok();
        }else{
            return NotFound();
        }
    }
}