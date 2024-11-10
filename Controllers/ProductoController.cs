using System.Diagnostics;
using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_lucianofalco.Models;

namespace tl2_tp6_2024_lucianofalco.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;
    private ProductoRepository repoProd;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        repoProd = new ProductoRepository();
    }
    //Listar, Crear, Modificar y Eliminar Productos

    public IActionResult Index()
    {

        return View(repoProd.ListarProductos());
    }

    [HttpPost]
    public IActionResult CrearProducto(Producto producto)
    {
        Producto p = repoProd.CrearProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AltaProducto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ModificarProducto(int id , string descripcion)
    {
        var productoModificado = repoProd.ModificarProducto(id , descripcion);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        var productoModificado = repoProd.ListarProductos().Find(p => p.IdProducto == id);
        return View(productoModificado);
    }

    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        Producto p = repoProd.ListarProductos().Find(p => p.IdProducto == id);
        return View(p);
    }

    [HttpPost]
    public IActionResult EliminarProductoPorId(int id)
    {
        Producto productoEliminado = repoProd.EliminarProducto(id);
        return RedirectToAction("Index") ;
    }
}
