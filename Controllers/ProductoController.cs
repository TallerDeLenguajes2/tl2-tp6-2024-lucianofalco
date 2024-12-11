using System.Diagnostics;
using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_lucianofalco.Models;

namespace tl2_tp6_2024_lucianofalco.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;

    private readonly IProductoRepository _repoProd;

    public ProductoController(ILogger<ProductoController> logger, IProductoRepository repoProd)
    {
        _logger = logger;
        _repoProd = repoProd;
    }

    public IActionResult Index()
    {
        ViewData["esAdmin"] = isAdmin();
        ViewData["esCliente"] = isClient();
        if (isAdmin() || isClient())
        {
            return View(_repoProd.ListarProductos());
        }
        else  return RedirectToAction("Salir" , "Usuario");
    }

    private bool isAdmin() => HttpContext.Session.GetString("Roll") == "Admin";
    private bool isClient() => HttpContext.Session.GetString("Roll") == "Cliente";

    [HttpPost]
    public IActionResult CrearProducto(ViewProducto productoVM)
    {
        if (ModelState.IsValid)
        {
            var p = new Producto(productoVM);
            _repoProd.CrearProducto(p);
            return RedirectToAction("Index");
        }
        else return View("AltaProducto", productoVM);
    }

    [HttpGet]
    public IActionResult AltaProducto()
    {
        return View(new ViewProducto());
    }

    [HttpPost]
    public IActionResult ModificarProducto(int id, ViewProducto productoVM)
    {
        if (ModelState.IsValid)
        {
            var producto = new Producto(productoVM);
            var productoModificado = _repoProd.ModificarProducto(id, producto);
            return RedirectToAction("Index");
        }

        return View(productoVM);
    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {

        var productoExistente = _repoProd.ListarProductos().Find(p => p.IdProducto == id);

        if (productoExistente == null)
        {
            return RedirectToAction("Index", new { error = "Producto no encontrado" });
        }

        var productoVM = new ViewProducto(productoExistente);


        return View(productoVM);
    }


    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        Producto p = _repoProd.ListarProductos().Find(p => p.IdProducto == id);
        if (p == null)
        {
            return RedirectToAction("Index", new { error = "Producto no encontrado" });
        }
        return View(p);
    }

    [HttpPost]
    public IActionResult EliminarProductoPorId(int id)
    {
        Producto productoEliminado = _repoProd.EliminarProducto(id);
        return RedirectToAction("Index");
    }
}
