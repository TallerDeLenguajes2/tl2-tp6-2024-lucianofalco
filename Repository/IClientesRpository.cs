public interface IClientesRpository
{
    List<Cliente> ListarClientes() ;

    Cliente GetCliente(int id);

    Cliente AgregarCliente(Cliente cliente);

    Cliente ModificarCliente(int id , Cliente nuevocliente);

    Cliente EliminarCliente(int id);
}