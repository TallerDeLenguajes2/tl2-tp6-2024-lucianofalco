/*
● Presupuestos
    ○ int IdPresupuesto
    ○ string nombreDestinatario
    ○ List<PresupuestoDetalle> detalle

*/

public class Presupuesto
{
    public int idPresupuesto {get; set;} 
    public Cliente cliente {get; set;}
    public List<PresupuestoDetalle> detalles {get; set;}
    public DateTime FechaCreacion {get; set;} 

    public Presupuesto(int idPresupuesto , Cliente cliente , List<PresupuestoDetalle> detalles , DateTime FechaCreacion )
    {
        this.idPresupuesto = idPresupuesto ; 
        this.cliente = cliente ; 
        this.detalles = detalles;
        this.FechaCreacion = FechaCreacion ;
    }
    public Presupuesto(int idPresupuesto , Cliente cliente, DateTime FechaCreacion )
    {
        this.idPresupuesto = idPresupuesto ; 
        this.cliente = cliente ; 
        this.FechaCreacion = FechaCreacion ;
    }
    public Presupuesto()
    {
        detalles = new List<PresupuestoDetalle>();
        cliente = new Cliente();
    }
    public Presupuesto(ViewAltaPresupuesto viewAltaPresupuestoVm)
    {
        cliente = new Cliente();
        cliente.idCliente = viewAltaPresupuestoVm.idCliente;
        FechaCreacion = viewAltaPresupuestoVm.Fecha ;
    }
    /*
    ○ Metodos
        ■ MontoPresupuesto ()
        ■ MontoPresupuestoConIva()
        ■ CantidadProductos ()
    */

    public double MontoPresupuesto(){
        double total = 0 ;
        foreach (var d in detalles)
        {
            total += d.Producto.Precio * d.Cantidad ; 
        }
        return total ;
    }

    public double MontoPresupuestoConIva(){
        return MontoPresupuesto() * 1.21 ;
    }

    public int CantidadProductos(){
        int cantidad = 0 ;
        foreach (var d in detalles)
        {
            cantidad += d.Cantidad ; 
        }
        return cantidad ;
    }
}