using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using System.Threading.Tasks;

namespace BankAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class ClientController:ControllerBase{
    private readonly ClientService _service;
    //Constructor
    public ClientController(ClientService client){
        _service=client;
    }
    /// <summary>
    /// Obtiene la lista de clientes disponibles.
    /// </summary>
    /// <remarks>
    /// Este servicio retorna una lista de clientes registrados en el sistema.
    /// </remarks>
    /// <returns>Lista de clientes</returns>
    /// <response code="200">Retorna la lista de clientes</response>
    /// <response code="400">Si la petición es incorrecta</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IEnumerable<Client> GetAll(){
        return _service.GetAll();
    }
    //REST asincrono
    [HttpGet("async")]
    public async Task<IEnumerable<Client>> GetAllAsync(){
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public ActionResult<Client> GetById(int id){
        var client=_service.GetById(id);
        if (client is null){
            return NotFound();
        }
        return client;
    }
    //REST asincrono
    [HttpGet("async/{id}")]
    public async Task<ActionResult<Client>> GetByIdAsync(int id){
        var client=await _service.GetByIdAsync(id);
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
    [HttpPut("{id}")]
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
    //Servicio asincrono
    [HttpPut("async/{id}")]
    public async Task<IActionResult> UpdateAsync(int id,Client client){
        if(id!=client.Id){
            return BadRequest(new{message=$"El ID {id} de la URL no coincide con el ID({client.Id}) del cuerpo de la solicitud"});
        }
        var clientToUpdate=await _service.GetByIdAsync(id);
        if(clientToUpdate is not null){
            await _service.UpdateAsync(id,client);
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
    //Esto genera error si no tiene un método HTTP explícito
    /*public NotFoundObjectResult ClientNotFound(int id){
        return NotFound(new {message=$"El cliente con ID={id} no existe."});
    }*/
}