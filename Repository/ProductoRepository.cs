using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.Data.Sqlite;

public class ProductoRepository : IProductoRepository
{
    private string connectionString;

    public ProductoRepository()
    {
        connectionString = "Data Source=bd/Tienda.db;";
    }

    public Producto CrearProducto(Producto p)
    {
        Producto producto = null;
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string querystring = "INSERT INTO Productos (Descripcion, Precio) VALUES (@nombre, @precio);";
            var command = new SqliteCommand(querystring, connection);
            command.Parameters.AddWithValue("@nombre", p.Descripcion);
            command.Parameters.AddWithValue("@precio", p.Precio);
            command.ExecuteNonQuery();
            string selectProducto = "select * from productos where idProducto = @id";
            var commandSelect = new SqliteCommand(selectProducto, connection);
            commandSelect.Parameters.AddWithValue("@id", p.IdProducto);
            using (var reader = commandSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToDouble(reader["Precio"]);
                }
            }
            connection.Close();
        }
        return producto;
    }
public Producto EliminarProducto(int id)
{
    Producto productoEliminado = ListarProductos().Find(p => p.IdProducto == id);
    if (productoEliminado is not null)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string deleteRelated = "DELETE FROM PresupuestosDetalle WHERE idProducto = @id";
            using (var commandRelated = new SqliteCommand(deleteRelated, connection))
            {
                commandRelated.Parameters.AddWithValue("@id", id);
                commandRelated.ExecuteNonQuery();
            }

            // Luego, eliminar el producto
            string querystring = "DELETE FROM Productos WHERE idProducto = @id";
            using (var command = new SqliteCommand(querystring, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
    return productoEliminado;
}


    public List<Producto> ListarProductos()
    {
        List<Producto> productos = new List<Producto>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string queryString = "Select * from productos; ";
            var command = new SqliteCommand(queryString, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    var producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                    productos.Add(producto);
                }
            }
            connection.Close();
        }
        return productos;
    }

    public Producto ModificarProducto(int id, string nombre)
    {
        Producto producto = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string querystring = "UPDATE Productos SET Descripcion = @Descripcion WHERE idProducto = @idProducto;";
            var command = new SqliteCommand(querystring, connection);
            command.Parameters.AddWithValue("@Descripcion", nombre);
            command.Parameters.AddWithValue("@idProducto", id);
            command.ExecuteNonQuery();

            string selecquery = "select * from productos where idProducto = @idProducto ;";
            var commandSelect = new SqliteCommand(selecquery, connection);
            commandSelect.Parameters.AddWithValue("@idProducto", id);
            using (var reader = commandSelect.ExecuteReader())
            {
                while (reader.Read())
                {

                    producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                }
            }
            connection.Close();
        }
        return producto;
    }
}