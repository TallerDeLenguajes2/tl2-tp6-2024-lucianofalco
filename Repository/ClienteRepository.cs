
using Microsoft.Data.Sqlite;

public class ClienteRepository : IClientesRpository
{
    private  string connectionString;
    public ClienteRepository()
    {
        connectionString = "Data Source=bd/Tienda.db";
    }

    public Cliente AgregarCliente(Cliente c)
    {
        Cliente cliente = null;
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string querystring = "INSERT INTO Clientes (Nombre, Telefono , Email) VALUES (@Nombre, @Telefono , @Email);";

            var command = new SqliteCommand(querystring, connection);
            command.Parameters.AddWithValue("@Nombre", c.Nombre);
            command.Parameters.AddWithValue("@Telefono", c.Telefono);
            command.Parameters.AddWithValue("@Email", c.Email);
            command.ExecuteNonQuery();
            string selectProducto = "select * from clientes where idCliente = @id";
            var commandSelect = new SqliteCommand(selectProducto, connection);
            commandSelect.Parameters.AddWithValue("@id", c.idCliente);
            using (var reader = commandSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    cliente = new Cliente();
                    cliente.idCliente = Convert.ToInt32(reader["idCliente"]);
                    cliente.Nombre= reader["Nombre"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                    cliente.Email = reader["Email"].ToString();
                }
            }
            connection.Close();
        }
        return cliente;
    }

    public Cliente EliminarCliente(int id)
    {
        Cliente clienteEliminado = ListarClientes().Find(p => p.idCliente == id);
        if (clienteEliminado is not null)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string deleteRelated = "DELETE FROM clientes WHERE idCliente = @id";
                using (var commandRelated = new SqliteCommand(deleteRelated, connection))
                {
                    commandRelated.Parameters.AddWithValue("@id", id);
                    commandRelated.ExecuteNonQuery();
                }
            }
        }
        return clienteEliminado;
    }

    public Cliente GetCliente(int id)
    {
        Cliente cliente = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string queryString = "Select * from clientes where idCliente = @id ; ";
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id" , id);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cliente = new Cliente();
                    cliente.idCliente = Convert.ToInt32(reader["IdCliente"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                    cliente.Email = reader["Email"].ToString();
                }
            }
            connection.Close();
        }
        return cliente;
    }

    public List<Cliente> ListarClientes()
    {
        List<Cliente> clientes = new List<Cliente>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string queryString = "Select * from clientes; ";
            var command = new SqliteCommand(queryString, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var cliente = new Cliente();
                    cliente.idCliente = Convert.ToInt32(reader["IdCliente"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    clientes.Add(cliente);
                }
            }
            connection.Close();
        }
        return clientes;
    }

    public Cliente ModificarCliente(int id, Cliente nuevocliente)
    {
       Cliente cliente = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string querystring = "UPDATE Clientes SET Email = @Email , Telefono = @Telefono, Nombre = @Nombre WHERE idCliente = @idCliente;" ; 

            var command = new SqliteCommand(querystring, connection);
            command.Parameters.AddWithValue("@Email", nuevocliente.Email);
            command.Parameters.AddWithValue("@Telefono", nuevocliente.Telefono);
            command.Parameters.AddWithValue("@Nombre", nuevocliente.Nombre);
            command.Parameters.AddWithValue("@idCliente", nuevocliente.idCliente);
            command.ExecuteNonQuery();

            string selecquery = "select * from clientes where idCliente = @idCliente ;";
            var commandSelect = new SqliteCommand(selecquery, connection);
            commandSelect.Parameters.AddWithValue("@idCliente", nuevocliente.idCliente);
            using (var reader = commandSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    cliente = new Cliente();
                    cliente.idCliente = Convert.ToInt32(reader["idCliente"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                    cliente.Email = reader["Email"].ToString();
                }
            }
            connection.Close();
        }
        return cliente;
    }
}