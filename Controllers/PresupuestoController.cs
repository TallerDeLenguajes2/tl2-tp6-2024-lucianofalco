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
    private readonly IClientesRpository _repoCliente;
    private readonly IProductoRepository _repoProducto ;

    private readonly IPresupuestoRepositoy _repoPresupuesto;

    public PresupuestoController(ILogger<PresupuestoController> logger , IPresupuestoRepositoy repoPresupuesto, IProductoRepository repoProducto , IClientesRpository repoCliente)
    {
        _logger = logger;
        _repoPresupuesto =repoPresupuesto ;
        _repoCliente = repoCliente ;
        _repoProducto = repoProducto ;
    }


    [HttpGet]
    public IActionResult Index()
    {
        ViewData["esAdmin"] = HttpContext.Session.GetString("Roll")=="Admin";
        ViewData["esCliente"] = HttpContext.Session.GetString("Roll")=="Cliente";
        return View(_repoPresupuesto.ListarPresupuesto());
    }

    [HttpGet]
    public IActionResult AltaPresupuesto(){
        var model = new ViewAltaPresupuesto() ;
        model.clientes = _repoCliente.ListarClientes();
        return View(model);
    }

    [HttpPost]
    public IActionResult CrearPresupuesto(ViewAltaPresupuesto altaPresupuestoVM){
       
        var p = new Presupuesto(altaPresupuestoVM);
        _repoPresupuesto.CrearPresupuesto(p);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id){

        var presupuestoVM = new ViewAltaPresupuesto();
        presupuestoVM.clientes = _repoCliente.ListarClientes();
        var presupuesto  = _repoPresupuesto.GetPresupuesto(id);
        presupuestoVM.idCliente = presupuesto.cliente.idCliente; 
        presupuestoVM.Fecha = presupuesto.FechaCreacion;
        return View(presupuestoVM);
    }
    
    [HttpPost]
    public IActionResult ModificarPresupuestoPost(int id , ViewAltaPresupuesto p){
        var presupuestoVM = new Presupuesto(p);
        var pr = _repoPresupuesto.ModificarPresupuesto(id , presupuestoVM);
        return RedirectToAction("Index");
    }

     [HttpGet]
    public IActionResult EliminarPresupuesto(int id){

        var p = _repoPresupuesto.GetPresupuesto(id);
        return View(p);
    }
    [HttpPost]
    public IActionResult EliminarPresupuestoPost(int id){
        var p = _repoPresupuesto.EliminarPresupuesto(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult VerDetalle(int id){
        var p = _repoPresupuesto.GetPresupuesto(id);
        return View(p);
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id){
        var model = new viewAgregarProductoAlPresupuesto();
        model.productos = _repoProducto.ListarProductos();
        model.idPre = id ;
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProductoPost(viewAgregarProductoAlPresupuesto model){
        var pd = _repoPresupuesto.AgregarProducto(model.idPre , model.idPro , model.cantidad);
        return RedirectToAction("Index");
    }

}
