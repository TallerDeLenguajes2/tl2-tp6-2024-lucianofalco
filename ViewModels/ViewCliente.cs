public class ViewCliente
{
    public string Nombre {get;set;}
    public string Telefono {get;set;}
    public string Email {get;set;}
    public int idCliente {get;set;}

    public ViewCliente(Cliente c)
    {
        this.Nombre = c.Nombre ; 
        this.Telefono = c.Telefono ; 
        this.Email = c.Email;
        this.idCliente = c.idCliente;

    }
    public ViewCliente()
    {
    }
}