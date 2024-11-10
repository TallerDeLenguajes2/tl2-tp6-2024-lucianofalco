
using Microsoft.Data.Sqlite;

public class PresupuestosRepository : IPresupuestoRepositoy
{
    private string connectionString;
    public PresupuestosRepository()
    {
        connectionString = "Data Source=bd/Tienda.db;";
    }

    public PresupuestoDetalle AgregarProducto(int idPre, int idpro, int cantidad)  // PresupuestoDetalle agregarProducto(int idpre , int idpro, int cantidad);
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
        Presupuesto presupuestoSinDetalle = null;
        bool tieneProductos = false;
        var detalles = new List<PresupuestoDetalle>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Consulta para obtener detalles y productos
            string queryString = "select * from Presupuestos pre " +
                                 "inner join PresupuestosDetalle pd on pd.idPresupuesto = pre.idPresupuesto " +
                                 "inner join Productos pro on pro.idProducto = pd.idProducto " +
                                 "where pre.idPresupuesto = @id;";
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);

            // Consulta para obtener presupuesto sin detalles
            string selectPresupuesto = "select * from Presupuestos where idPresupuesto = @id;";
            var commandSelect = new SqliteCommand(selectPresupuesto, connection);
            commandSelect.Parameters.AddWithValue("@id", id);

            // Intentamos leer el presupuesto base (sin detalles)
            using (var leer = commandSelect.ExecuteReader())
            {
                if (leer.Read()) // Aseguramos que haya una fila
                {
                    presupuestoSinDetalle = new Presupuesto
                    {
                        IdPresupuesto = Convert.ToInt32(leer["idPresupuesto"]),
                        NombreDestinatario = leer["NombreDestinatario"].ToString(),
                        FechaCreacion = Convert.ToDateTime(leer["FechaCreacion"]),
                        Detalles = new List<PresupuestoDetalle>()  // Inicialización de la lista
                    };
                }
            }

            // Intentamos leer el detalle del presupuesto
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tieneProductos = true;

                    // Solo inicializar `presupuesto` la primera vez
                    if (presupuesto == null)
                    {
                        presupuesto = new Presupuesto
                        {
                            IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                            NombreDestinatario = reader["NombreDestinatario"].ToString(),
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                            Detalles = new List<PresupuestoDetalle>()
                        };
                    }

                    // Extraemos y asignamos detalles de productos
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
                presupuesto.Detalles.Add(detalle);
            }
        }

        // Retornar el presupuesto con o sin detalles según corresponda
        if (tieneProductos)
        {
            return presupuesto; // Si hay productos, devolvemos el presupuesto con detalles
        }
        else
        {
            return presupuestoSinDetalle; // Si no hay productos, devolvemos el presupuesto sin detalles
        }
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

    public Presupuesto ModificarPresupuesto(int idPresupuesto, Presupuesto p)
    { // agregado en tp 6
        Presupuesto presupuesto = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string querystring = "UPDATE Presupuestos SET FechaCreacion = @FechaCreacion , NombreDestinatario = @NombreDestinatario WHERE idPresupuesto = @idPresupuesto;";
            var command = new SqliteCommand(querystring, connection);
            command.Parameters.AddWithValue("@FechaCreacion", p.FechaCreacion.ToString("yyyy-MM-dd")); // p.FechaCreacion.ToString("yyyy-MM-dd")
            command.Parameters.AddWithValue("@NombreDestinatario", p.NombreDestinatario);
            command.Parameters.AddWithValue("@idPresupuesto", p.IdPresupuesto);
            command.ExecuteNonQuery();

            string selecquery = "select * from presupuestos where idPresupuesto = @idPresupuesto ;";
            var commandSelect = new SqliteCommand(selecquery, connection);
            commandSelect.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            using (var reader = commandSelect.ExecuteReader())
            {
                while (reader.Read())
                {

                    presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                }
            }
            connection.Close();
        }
        return presupuesto;
    }
}