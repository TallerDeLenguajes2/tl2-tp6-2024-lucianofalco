
// Crear un nuevo Producto. (recibe un objeto Producto)
// ● Modificar un Producto existente. (recibe un Id y un objeto Producto)
// ● Listar todos los Productos registrados. (devuelve un List de Producto)
// ● Obtener detalles de un Productos por su ID. (recibe un Id y devuelve un
// Producto)
// ● Eliminar un Producto por ID
using System.Runtime.CompilerServices;

public interface IProductoRepository
{
    Producto CrearProducto(Producto p);
    Producto ModificarProducto(int id , string nombre);
    List<Producto> ListarProductos();
    Producto EliminarProducto(int id);

}