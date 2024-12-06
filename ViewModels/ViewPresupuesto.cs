// 3) Incorpore los ViewModels
// a) Para Presupuestos: debe incluir una la lista de clientes para poder elegirlos en el
// formulario de alta de presupuestos


using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


public class ViewAltaPresupuesto
{
    [Required(ErrorMessage = "Seleccione un cliente.")]
    public int idCliente { get; set; }
    
    [Required(ErrorMessage = "Ingrese una fecha válida.")]
    public DateTime Fecha { get; set; }
    public List<Cliente> clientes { get; set; }
    public ViewAltaPresupuesto()
    {
        clientes = new List<Cliente>();
    }
}