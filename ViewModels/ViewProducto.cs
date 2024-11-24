public class ViewProducto
{

    public string Descripcion {get;set;}
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



