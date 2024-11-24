public class Cliente
{
    public int idCliente { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }

    public Cliente()
    {
    }
    public Cliente(ViewCliente c)
    {
        this.Nombre = c.Nombre ; 
        this.Telefono = c.Telefono ; 
        this.Email = c.Email;
        this.idCliente = c.IdCliente;
    }

}