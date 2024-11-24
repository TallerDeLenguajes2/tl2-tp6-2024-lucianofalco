using System.Diagnostics;
using System.Net.Cache;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_tp6_2024_lucianofalco.Models;

namespace tl2_tp6_2024_lucianofalco.Controllers;

public class PresupuestoController : Controller
{
    private readonly ILogger<PresupuestoController> _logger;
    private PresupuestosRepository repoPresupuesto;

    public PresupuestoController(ILogger<PresupuestoController> logger)
    {
        _logger = logger;
        repoPresupuesto = new PresupuestosRepository();
    }


    [HttpGet]
    public IActionResult Index()
    {
        return View(repoPresupuesto.ListarPresupuesto());
    }

    [HttpGet]
    public IActionResult AltaPresupuesto(){
        ClienteRepository repoCliete = new ClienteRepository();
        List<Cliente> clientes = repoCliete.ListarClientes();
        ViewData["Clientes"] = clientes.Select(c => new SelectListItem
        {
            Value = c.idCliente.ToString(), 
            Text = c.Nombre
        }).ToList();

        return View();
    }

    [HttpPost]
    public IActionResult CrearPresupuesto(ViewAltaPresupuesto altaPresupuestoVM){
       
        var p = new Presupuesto(altaPresupuestoVM);
        repoPresupuesto.CrearPresupuesto(p);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id){

        ClienteRepository repoClientes = new ClienteRepository();
        List<Cliente> Clientes = repoClientes.ListarClientes();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.idCliente.ToString(), 
            Text = c.Nombre
        }).ToList();

        var presupuesto  = repoPresupuesto.GetPresupuesto(id);
        var presupuestoVM = new ViewAltaPresupuesto();
        presupuestoVM.idCliente = presupuesto.cliente.idCliente; 
        presupuestoVM.Fecha = presupuesto.FechaCreacion;
        return View(presupuestoVM);
    }
    
    [HttpPost]
    public IActionResult ModificarPresupuestoPost(int id , ViewAltaPresupuesto p){
        var presupuestoVM = new Presupuesto(p);
        var pr = repoPresupuesto.ModificarPresupuesto(id , presupuestoVM);
        return RedirectToAction("Index");
    }

     [HttpGet]
    public IActionResult EliminarPresupuesto(int id){

        var p = repoPresupuesto.GetPresupuesto(id);
        return View(p);
    }
    [HttpPost]
    public IActionResult EliminarPresupuestoPost(int id){
        var p = repoPresupuesto.EliminarPresupuesto(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult VerDetalle(int id){
        var p = repoPresupuesto.GetPresupuesto(id);
        return View(p);
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id){
        ProductoRepository repoProductos = new ProductoRepository();
        List<Producto> productos = repoProductos.ListarProductos();
        ViewData["Productos"] = productos.Select(p => new SelectListItem
        {
            Value = p.IdProducto.ToString(), 
            Text = p.Descripcion 
        }).ToList();

        var model = new viewAgregarProductoAlPresupuesto();
        model.idPre = id ;

        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProductoPost(viewAgregarProductoAlPresupuesto model){
        var pd = repoPresupuesto.AgregarProducto(model.idPre , model.idPro , model.cantidad);
        return RedirectToAction("Index");
    }

}
