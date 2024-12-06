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
        var model = new ViewAltaPresupuesto() ;
        model.clientes = repoCliete.ListarClientes();
        return View(model);
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
        var presupuestoVM = new ViewAltaPresupuesto();
        presupuestoVM.clientes = repoClientes.ListarClientes();
        var presupuesto  = repoPresupuesto.GetPresupuesto(id);
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
        var model = new viewAgregarProductoAlPresupuesto();
        model.productos = repoProductos.ListarProductos();
        model.idPre = id ;
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProductoPost(viewAgregarProductoAlPresupuesto model){
        var pd = repoPresupuesto.AgregarProducto(model.idPre , model.idPro , model.cantidad);
        return RedirectToAction("Index");
    }

}
