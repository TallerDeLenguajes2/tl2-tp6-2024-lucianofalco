
using Microsoft.Data.Sqlite;

public class PresupuestosRepository : IPresupuestoRepositoy
{
    private string connectionString;
    public PresupuestosRepository()
    {
        connectionString = "Data Source=bd/Tienda.db;";
    }

    public PresupuestoDetalle AgregarProducto(int idPre, int idpro, int cantidad)
    {
        PresupuestoDetalle pd = null;
        ProductoRepository productoRepository = new ProductoRepository();
        Producto producto = productoRepository.ListarProductos().Find(p => p.IdProducto == idpro);
        Presupuesto presupuesto = ListarPresupuesto().Find(p => p.idPresupuesto == idPre);
        if (producto != null && presupuesto != null)
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
            string queryString = $"INSERT INTO Presupuestos (FechaCreacion , idCliente) VALUES (@Fecha , @idCliente);";
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idCliente", p.cliente.idCliente);
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

                string deleteDetailsQuery = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id;";
                var deleteDetailsCommand = new SqliteCommand(deleteDetailsQuery, connection);
                deleteDetailsCommand.Parameters.AddWithValue("@id", id);
                deleteDetailsCommand.ExecuteNonQuery();

                string deletePresupuestoQuery = "DELETE FROM Presupuestos WHERE idPresupuesto = @id;";
                var deletePresupuestoCommand = new SqliteCommand(deletePresupuestoQuery, connection);
                deletePresupuestoCommand.Parameters.AddWithValue("@id", id);
                deletePresupuestoCommand.ExecuteNonQuery();

                connection.Close();
            }
        }
        return presupuesto;
    }

    public Presupuesto GetPresupuesto(int id)
    {
        Presupuesto presupuesto = null;
        Presupuesto presupuestoSinDetalle = null;
        bool tieneProductos = false;
        var detalles = new List<PresupuestoDetalle>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string queryString = "select * from Presupuestos pre " +
                                 "inner join PresupuestosDetalle pd on pd.idPresupuesto = pre.idPresupuesto " +
                                 "inner join Productos pro on pro.idProducto = pd.idProducto " +
                                 "inner join clientes c on c.idCliente = pre.idCliente " +
                                 "where pre.idPresupuesto = @id;";
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);

            string selectPresupuesto = "select * from Presupuestos p inner join clientes c on c.idCliente = p.idCliente where idPresupuesto = @id;";
            var commandSelect = new SqliteCommand(selectPresupuesto, connection);
            commandSelect.Parameters.AddWithValue("@id", id);

            using (var leer = commandSelect.ExecuteReader())
            {
                if (leer.Read())
                {
                    presupuestoSinDetalle = new Presupuesto();
                    presupuestoSinDetalle.idPresupuesto = Convert.ToInt32(leer["idPresupuesto"]);
                    presupuestoSinDetalle.FechaCreacion = Convert.ToDateTime(leer["FechaCreacion"]);
                    presupuestoSinDetalle.cliente.idCliente = Convert.ToInt32(leer["idCliente"]);
                    presupuestoSinDetalle.cliente.Nombre = leer["Nombre"].ToString();
                    presupuestoSinDetalle.cliente.Telefono = leer["Telefono"].ToString();
                    presupuestoSinDetalle.cliente.Email = leer["Email"].ToString();
                    detalles = new List<PresupuestoDetalle>();
                }
            }

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tieneProductos = true;

                    if (presupuesto == null)
                    {
                        presupuesto = new Presupuesto();

                        presupuesto.idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                        presupuesto.cliente.idCliente = Convert.ToInt32(reader["idCliente"]);
                        presupuesto.cliente.Nombre = reader["Nombre"].ToString();
                        presupuesto.cliente.Telefono = reader["Telefono"].ToString();
                        presupuesto.cliente.Email = reader["Email"].ToString();
                        detalles = new List<PresupuestoDetalle>();
                    }

                    string descripcion = reader["Descripcion"].ToString();
                    int idProducto = Convert.ToInt32(reader["idProducto"]);
                    double precio = Convert.ToDouble(reader["Precio"]);
                    var producto = new Producto(idProducto, descripcion, precio);

                    int cantidad = Convert.ToInt32(reader["Cantidad"]);
                    var detalle = new PresupuestoDetalle(producto, cantidad);
                    detalles.Add(detalle);
                }
            }

            connection.Close();
        }

        if (presupuesto != null)
        {
            foreach (var detalle in detalles)
            {
                presupuesto.detalles.Add(detalle);
            }
        }


        if (tieneProductos)
        {
            return presupuesto;
        }
        else
        {
            return presupuestoSinDetalle;
        }
    }

    public List<Presupuesto> ListarPresupuesto()
    {
        Presupuesto presupuesto = null;
        Cliente cliente = null;
        List<Presupuesto> presupuestos = new List<Presupuesto>();
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string queryString = "select * from presupuestos p left join clientes c on c.idCliente = p.idCliente;";
            var command = new SqliteCommand(queryString, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cliente = new Cliente();
                    int idP = Convert.ToInt32(reader["idPresupuesto"]);
                    var fecha = Convert.ToDateTime(reader["FechaCreacion"]);
                    cliente.idCliente = Convert.ToInt32(reader["idCliente"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    presupuesto = new Presupuesto(idP, cliente, fecha);
                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }

    public Presupuesto ModificarPresupuesto(int idPresupuesto, Presupuesto p)
    {
        Presupuesto presupuesto = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();


            string querystring = "UPDATE Presupuestos SET FechaCreacion = @FechaCreacion , idCliente = @idCliente WHERE idPresupuesto = @idPresupuesto;";
            var command = new SqliteCommand(querystring, connection);
            command.Parameters.AddWithValue("@FechaCreacion", p.FechaCreacion.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@idCliente", p.cliente.idCliente);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.ExecuteNonQuery();

            string selectquery = "SELECT * FROM Presupuestos WHERE idPresupuesto = @idPresupuesto;";
            var commandSelect = new SqliteCommand(selectquery, connection);
            commandSelect.Parameters.AddWithValue("@idPresupuesto", idPresupuesto); // Cambiado a idPresupuesto
            using (var reader = commandSelect.ExecuteReader())
            {
                if (reader.Read())
                {
                    presupuesto = new Presupuesto
                    {
                        idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                        FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                        cliente = new Cliente { idCliente = Convert.ToInt32(reader["idCliente"]) } // Crea cliente nuevo
                    };
                }
            }

            connection.Close();
        }
        return presupuesto;
    }


}