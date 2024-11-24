public class ViewAltaPresupuesto
{
    public int idCliente {get;set;} 
    // public int idPresupuesto ;
    public DateTime Fecha {get;set;}
    public List<Cliente> clientes  {get;set;}
    public ViewAltaPresupuesto()
    {
        clientes = new List<Cliente>();
    }   
}