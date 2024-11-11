/*
● Productos
    ○ int idProducto
    ○ string descripcion
    ○ int precio


*/
public class Producto
{
    private int idProducto ;
    private string descripcion ; 
    private double precio ;

    public Producto(){
        
    }

    public Producto(int idProducto, string descripcion, double precio)
    {
        this.idProducto = idProducto;
        this.descripcion = descripcion;
        this.precio = precio;
    }

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public double Precio { get => precio; set => precio = value; }
}