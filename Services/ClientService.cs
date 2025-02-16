using System.Threading.Tasks;
using BankAPI.Data;
using BankAPI.Data.BankModels;
using Microsoft.EntityFrameworkCore;
namespace BankAPI.Services;

public class ClientService{
    private readonly BankContext _context;
    public ClientService(BankContext context){
        _context=context;
    }
    public IEnumerable<Client> GetAll(){
        return _context.Clients.ToList();
    }
    //Una funcion asincrona
    public async Task<IEnumerable<Client>> GetAllAsync(){
        return await _context.Clients.ToListAsync();
    }
    //Una funcion asincrona
    public async Task<Client?> GetByIdAsync(int id){
        return await _context.Clients.FindAsync(id);
    }
    public Client? GetById(int id){
        return _context.Clients.Find(id);
    }
    //Una funcion asincrona
    public async Task<Client> CreateAsync(Client client){
        _context.Clients.Add(client);
       await _context.SaveChangesAsync();
        //Se llama a la funcion getbyid para devolver el id creado
        return client;
    }
    public Client Create(Client client){
        _context.Clients.Add(client);
        _context.SaveChanges();
        //Se llama a la funcion getbyid para devolver el id creado
        return client;
    }
    //Una funcion asincrona
    public async Task UpdateAsync(int id,Client client){
        var existingCLient=await GetByIdAsync(id);
        if(existingCLient is not null){
            existingCLient.Name=client.Name;
            existingCLient.PhoneNumber=client.PhoneNumber;
            existingCLient.Email=client.Email;
           await _context.SaveChangesAsync();
        }
    }
    public void Update(int id,Client client){
        var existingCLient=_context.Clients.Find(id);
        if(existingCLient is not null){
            existingCLient.Name=client.Name;
            existingCLient.PhoneNumber=client.PhoneNumber;
            existingCLient.Email=client.Email;
            _context.SaveChanges();
        }
    }
    //Una funcion asincrona
    public async Task DeleteAsync(int id){
        var clientToDelete=await GetByIdAsync(id);
        if(clientToDelete is not null){
            _context.Clients.Remove(clientToDelete);
            await _context.SaveChangesAsync();
        }
    }
    public void Delete(int id){
        var clientToDelete=GetById(id);
        if(clientToDelete is not null){
            _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();
        }
    }
}
