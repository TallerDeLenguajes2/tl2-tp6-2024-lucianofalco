
using Microsoft.Data.Sqlite;

public class PresupuestosRepository : IPresupuestoRepositoy
{
    private string connectionString;
    public PresupuestosRepository()
    {
        connectionString = "Data Source=bd/Tienda.db;";
    }

    public PresupuestoDetalle agregarProducto(int idPre, int idpro, int cantidad)  // PresupuestoDetalle agregarProducto(int idpre , int idpro, int cantidad);
    {
        PresupuestoDetalle pd = null;
        ProductoRepository productoRepository = new ProductoRepository();
        Producto producto = productoRepository.ListarProductos().Find(p => p.IdProducto == idpro);
        if (producto != null)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string insertarPresupuestoDetalleQuery = "INSERT INTO presupuestosdetalle (idPresupuesto, idProducto , Cantidad) VALUES (@idpre , @idpro, @cant);";
                string verificarCorrespondencia = "SELECT * FROM presupuestosdetalle WHERE idPresupuesto = @idpre AND idProducto = @idpro;";
                var commandVerificar = new SqliteCommand(verificarCorrespondencia, connection);
                commandVerificar.Parameters.AddWithValue("@idpre", idPre);
                commandVerificar.Parameters.AddWithValue("@idpro", idpro);
                bool registroExiste = false;
                using (var leer = commandVerificar.ExecuteReader())
                {
                    if (leer.Read())
                    {
                        registroExiste = true;
                    }
                }

                if (!registroExiste)
                {
                    var command = new SqliteCommand(insertarPresupuestoDetalleQuery, connection);
                    command.Parameters.AddWithValue("@idpre", idPre);
                    command.Parameters.AddWithValue("@idpro", idpro);
                    command.Parameters.AddWithValue("@cant", cantidad);
                    command.ExecuteNonQuery();

                    string nuevaConsulta = "select * from presupuestosdetalle where idProducto == @idpro and idPresupuesto == @idpre; ";
                    var newCommand = new SqliteCommand(nuevaConsulta, connection);
                    newCommand.Parameters.AddWithValue("@idpre", idPre);
                    newCommand.Parameters.AddWithValue("@idpro", idpro);
                    using (var leer = newCommand.ExecuteReader())
                    {
                        while (leer.Read())
                        {
                            pd = new PresupuestoDetalle();
                            pd.Producto.IdProducto = Convert.ToInt32(leer["idProducto"]);
                            pd.Cantidad = Convert.ToInt32(leer["Cantidad"]);
                        }
                    }
                }
                connection.Close();
            }

        }
        return pd;
    }

    public void CrearPresupuesto(Presupuesto p)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string queryString = $"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@Nombre, @Fecha);";
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@Nombre", p.NombreDestinatario);
            command.Parameters.AddWithValue("@Fecha", p.FechaCreacion.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public Presupuesto EliminarPresupuesto(int id)
    {
        Presupuesto presupuesto = GetPresupuesto(id);
        if (presupuesto is not null)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string queryString = $"DELETE FROM Presupuestos WHERE idPresupuesto = @id ;";
                var command = new SqliteCommand(queryString, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return presupuesto;
    }

    public Presupuesto GetPresupuesto(int id)
    {
        Presupuesto presupuesto = null;
        var detalles = new List<PresupuestoDetalle>();
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string queryString = $"select * from Presupuestos pre inner join PresupuestosDetalle pd on pd.idPresupuesto = pre.idPresupuesto inner join Productos pro on pro.idProducto = pd.idProducto where pre.idPresupuesto = @id ;";
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //creo presupuesto
                    presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);


                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);

                    // guardo valores para instanciar un producto
                    string descripcion = reader["Descripcion"].ToString();
                    int idProducto = Convert.ToInt32(reader["idProducto"]);
                    double precio = Convert.ToDouble(reader["Precio"]);

                    // creo producto
                    var producto = new Producto(idProducto, descripcion, precio);

                    //guardo valores para instanciar un detalle
                    int cantidad = Convert.ToInt32(reader["Cantidad"]);
                    //creo detalle
                    var detalle = new PresupuestoDetalle(producto, cantidad);
                    detalles.Add(detalle); // agrego el detalle a la lista
                }
            }
            connection.Close();
        }
        foreach (var d in detalles)
        {
            presupuesto.Detalles.Add(d);
        }
        return presupuesto;
    }

    public List<Presupuesto> ListarPresupuesto()
    {
        Presupuesto presupuesto = null;
        List<Presupuesto> presupuestos = new List<Presupuesto>();
        string connectionString = "Data Source=bd/Tienda.db;";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string queryString = "select * from presupuestos";
            var command = new SqliteCommand(queryString, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    presupuestos.Add(presupuesto);

                }
            }
            connection.Close();
        }
        return presupuestos;
    }
}