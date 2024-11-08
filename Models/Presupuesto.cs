/*
● Presupuestos
    ○ int IdPresupuesto
    ○ string nombreDestinatario
    ○ List<PresupuestoDetalle> detalle

*/

public class Presupuesto
{
    int idPresupuesto ; 
    string nombreDestinario ; 
    List<PresupuestoDetalle> detalles ;
    public DateTime FechaCreacion {get; set;} 

    public Presupuesto()
    {
        detalles = new List<PresupuestoDetalle>();
    }

    public Presupuesto(string nombreDestinario, DateTime FechaCreacion , List<PresupuestoDetalle> detalles)
    {
        this.nombreDestinario = nombreDestinario;
        this.FechaCreacion = FechaCreacion ; 
        this.Detalles = detalles ; 
    }
    public Presupuesto(string nombreDestinario, DateTime FechaCreacion)
    {
        this.nombreDestinario = nombreDestinario;
        this.FechaCreacion = FechaCreacion ; 
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinario; set => nombreDestinario = value; }
    public List<PresupuestoDetalle> Detalles { get => detalles; set => detalles = value; }


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