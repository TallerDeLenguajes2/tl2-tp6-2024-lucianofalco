
using Microsoft.Data.Sqlite;

public class UsuarioRepository : IUsarioRepository
{
    private string connectionString;
    public UsuarioRepository()
    {
        connectionString = "Data Source=bd/Tienda.db";
    }

    public User BuscarUsuario(string usuario , string password){
        return ListarUsuarios().Find(u => u.Usuario == usuario && u.Password == password) ;
    }

    public User AgregarUsuario(User user){
        User usuario = null;
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string querystring = "INSERT INTO Users (Nombre, Usuario , Password , ROL) VALUES ('@Nombre', '@Usuario' , '@Password' , '@ROL');";

            var command = new SqliteCommand(querystring, connection);
            command.Parameters.AddWithValue("@Nombre", user.Nombre);
            command.Parameters.AddWithValue("@Usuario", user.Usuario );
            command.Parameters.AddWithValue("@Password", user.Password );
            command.Parameters.AddWithValue("@ROL", user.Roll);
            command.ExecuteNonQuery();
            string selectUsuario = "select * from Users where idCliente = @id";
            var commandSelect = new SqliteCommand(selectUsuario, connection);
            commandSelect.Parameters.AddWithValue("@id", user.Id);
            using (var reader = commandSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                     usuario = new User();
                     usuario.Id = Convert.ToInt32(reader["id_User"]);
                     usuario.Nombre= reader["Nombre"].ToString();
                     usuario.Usuario= reader["Usuario"].ToString();
                     usuario.Password = reader["Password"].ToString();
                     usuario.Roll = reader["ROL"].ToString();
                }
            }
            connection.Close();
        }
        return usuario;
    }

    public User EliminarUsuario(int id)
    {
        User UsuarioEliminado = ListarUsuarios().Find(u => u.Id == id);
        if (UsuarioEliminado is not null)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string deleteRelated = "DELETE FROM Users WHERE id_User = @id";
                using (var commandRelated = new SqliteCommand(deleteRelated, connection))
                {
                    commandRelated.Parameters.AddWithValue("@id", id);
                    commandRelated.ExecuteNonQuery();
                }
            }
        }
        return UsuarioEliminado;
    }

    public User GetUsuario(int id)
    {
        User usuario = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string queryString = "Select * from Users where id_User = @id ; ";
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id" , id);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario = new User();
                    usuario.Id = Convert.ToInt32(reader["Id_User"]);
                    usuario.Nombre = reader["Nombre"].ToString();
                    usuario.Usuario = reader["Usuario"].ToString();
                    usuario.Password = reader["Password"].ToString();
                    usuario.Roll = reader["ROL"].ToString();
                }
            }
            connection.Close();
        }
        return usuario;
    }

    public List<User> ListarUsuarios()
    {
        List<User> usuarios = new List<User>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string queryString = "Select * from Users; ";
            var command = new SqliteCommand(queryString, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var us = new User();
                    us.Id = Convert.ToInt32(reader["Id_User"]);
                    us.Nombre = reader["Nombre"].ToString();
                    us.Usuario = reader["Usuario"].ToString();
                    us.Password = reader["Password"].ToString();
                    us.Roll = reader["ROL"].ToString();
                    usuarios.Add(us);
                }
            }
            connection.Close();
        }
        return usuarios;
    }

    public User ModificarUsuario(int id, User nuevoUsuario)
    {
        User usuario = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string querystring = "UPDATE Users SET Nombre = @Nombre , Usuario = @Usuario, Password = @Password , ROL = @Rol WHERE id_User = @Id_User;" ; 

            var command = new SqliteCommand(querystring, connection);
            command.Parameters.AddWithValue("@Nombre", nuevoUsuario.Nombre);
            command.Parameters.AddWithValue("@Usuario", nuevoUsuario.Usuario);
            command.Parameters.AddWithValue("@Password", nuevoUsuario.Password);
            command.Parameters.AddWithValue("@ROL", nuevoUsuario.Roll);
            command.Parameters.AddWithValue("@Id_User", nuevoUsuario.Id);
            command.ExecuteNonQuery();

            string selecquery = "select * from Users where id_User = @id_User ;";
            var commandSelect = new SqliteCommand(selecquery, connection);
            commandSelect.Parameters.AddWithValue("@id_User", nuevoUsuario.Id);
            using (var reader = commandSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario = new User();
                    usuario.Id = Convert.ToInt32(reader["id_User"]);
                    usuario.Nombre = reader["Nombre"].ToString();
                    usuario.Usuario = reader["Usuario"].ToString();
                    usuario.Password = reader["Password"].ToString();
                    usuario.Roll = reader["ROL"].ToString();
                }
            }
            connection.Close();
        }
        return usuario;
    }
}