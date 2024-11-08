/*
    ● PresupuestosDetalle
        ○ Productos producto
        ○ int cantidad

*/
public class PresupuestoDetalle
{
    Producto producto ; 
    int cantidad ;

    public PresupuestoDetalle(){
        Producto = new Producto();
    }
    public PresupuestoDetalle(Producto producto, int cantidad)
    {
        this.producto = producto;
        this.cantidad = cantidad;
    }

    public Producto Producto { get => producto; set => producto = value; }
    public int Cantidad { get => cantidad; set => cantidad = value; }
}