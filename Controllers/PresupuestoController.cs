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
        return View();
    }

    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuesto presupuesto){
        repoPresupuesto.CrearPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id){
        var p = repoPresupuesto.GetPresupuesto(id);
        return View(p);
    }
    
    [HttpPost]
    public IActionResult ModificarPresupuestoPost(int id , Presupuesto p){
        var pr = repoPresupuesto.ModificarPresupuesto(id , p);
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

        return View(id);
    }

    [HttpPost]
    public IActionResult AgregarProductoPost(int id , int idpro , int cantidad){
        var pd = repoPresupuesto.AgregarProducto(id , idpro , cantidad);
        return RedirectToAction("Index");
    }


    
}
