using BankAPI.Data;
using BankAPI.Data.BankModels;
namespace BankAPI.Services;

public class ClientService{
    private readonly BankContext _context;
    public ClientService(BankContext context){
        _context=context;
    }
    public IEnumerable<Client> GetAll(){
        return _context.Clients.ToList();
    }
    public Client? GetById(int id){
        return _context.Clients.Find(id);
    }
    public Client Create(Client client){
        _context.Clients.Add(client);
        _context.SaveChanges();
        //Se llama a la funcion getbyid para devolver el id creado
        return client;
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
    public void Delete(int id){
        var clientToDelete=GetById(id);
        if(clientToDelete is not null){
            _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();
        }
    }
}
