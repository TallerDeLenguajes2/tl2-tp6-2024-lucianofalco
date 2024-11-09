using System.Diagnostics;
using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_lucianofalco.Models;

namespace tl2_tp6_2024_lucianofalco.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;
    private ProductoRepository repoProd ;

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
    public IActionResult ModificarProducto()
    {
        return View();
    }

    public IActionResult EliminarProducto()
    {
        return View();
    }

    public IActionResult CrearProducto()
    {
        return View();
    }


}
