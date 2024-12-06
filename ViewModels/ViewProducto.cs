/*4) Añadir las siguientes validaciones:
En Productos:
● Descripción (string): opcional, con una longitud máxima de 250 caracteres.
● Precio (decimal): requerido y debe ser un valor positivo.
*/


// 3) Incorpore los ViewModels
// c) Para Productos: Implementa validaciones en el modelo Producto para asegurar la
// integridad de los datos y evitar la inserción de registros incompletos o incorrectos.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class ViewProducto
{

    [StringLength(250 , ErrorMessage = "El tamaño no debe superar los 250 caracteres")] [ Required(ErrorMessage = "El campo es obligatorio")]
    public string Descripcion {get;set;}

    [Range(1,int.MaxValue , ErrorMessage ="El numero debe ser positivo")]  [Required(ErrorMessage ="El campo es obligatorio")]
    public double Precio {get;set;}

    public ViewProducto(Producto producto)
    {
        Descripcion = producto.Descripcion;
        Precio = producto.Precio;
    }
    public ViewProducto()
    {
    }

    

}



