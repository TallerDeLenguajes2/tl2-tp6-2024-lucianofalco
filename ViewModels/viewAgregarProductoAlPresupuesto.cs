// 3) Incorpore los ViewModels
// b) Para Alta de producto a presupuesto: Incluir el listado de los productos para que
//se pueda seleccionar entre ellos.


using System.ComponentModel.DataAnnotations;

public class viewAgregarProductoAlPresupuesto
{


    public int idPre { get; set; }
    public int idPro { get; set; }

    public List<Producto> productos { get; set; }
    public int cantidad { get; set; }

    public viewAgregarProductoAlPresupuesto()
    {
        productos = new List<Producto>() ;
    }
}