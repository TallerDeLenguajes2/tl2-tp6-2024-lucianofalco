/*En Clientes:
● Nombre (string) es obligatorio
● Email (string) validar el tipo email
● Telefono (string) validar el tipo teléfono
*/
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class ViewCliente
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
    public string Nombre { get; set; }

    [Phone(ErrorMessage = "El teléfono debe tener un formato válido.")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El email es obligatorio.")]
    
    [EmailAddress(ErrorMessage = "Debe ser un email vá lido.")]
    public string Email { get; set; }

    public int IdCliente { get; set; }

    public ViewCliente(Cliente c)
    {
        Nombre = c.Nombre;
        Telefono = c.Telefono;
        Email = c.Email;
        IdCliente = c.idCliente;
    }

    public ViewCliente()
    {
    }
}
