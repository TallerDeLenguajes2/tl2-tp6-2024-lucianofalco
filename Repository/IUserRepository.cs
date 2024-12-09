public interface IUsarioRepository
{

    List<User> ListarUsuarios();

    User BuscarUsuario(string usuario , string password);

    User GetUsuario(int id);

    User AgregarUsuario(User user);

    User ModificarUsuario(int id, User nuevoUsuario);

    User EliminarUsuario(int id);
}