using System.Diagnostics;
using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_lucianofalco.Models;

namespace tl2_tp6_2024_lucianofalco.Controllers;

public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;
   // private ClienteRepository repoClientes;
    private readonly IClientesRpository _repoClientes;

    public ClienteController(ILogger<ClienteController> logger , IClientesRpository repoClientes)
    {
        _logger = logger;
        _repoClientes = repoClientes;
    }


    [HttpGet]
    public IActionResult Index()
    {
        ViewData["EsAdmin"] = HttpContext.Session.GetString("Roll") == "Admin";
        return View(_repoClientes.ListarClientes());
    }

    [HttpGet]
    public IActionResult AltaCliente()
    {
        return View(new ViewCliente());
    }

    [HttpPost]
    public IActionResult CrearCliente(ViewCliente cliente)
    {
        if (ModelState.IsValid)
        {
            var c = new Cliente(cliente);
            _repoClientes.AgregarCliente(c);
            return RedirectToAction("Index");
        }
        else return View(new ViewCliente());
    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        var cliente = _repoClientes.ListarClientes().Find(c => c.idCliente == id);
        ViewCliente model = new ViewCliente(cliente);
        return View(model);
    }

    [HttpPost]
    public IActionResult ModificarClientePost(int id, ViewCliente clienteVM)
    {
        if(ModelState.IsValid){
        var c = new Cliente(clienteVM);
        _repoClientes.ModificarCliente(id, c);
        return RedirectToAction("Index");
        }
        else return View(new ViewCliente());
    }

    [HttpGet]
    public IActionResult EliminarCliente(int id)
    {
        var p = _repoClientes.GetCliente(id);
        if (p == null)
        {
            return RedirectToAction("Index", new { error = "Producto no encontrado" });
        }
        return View(p);
    }
    [HttpPost]
    public IActionResult EliminarClientePost(int id)
    {
        var p = _repoClientes.EliminarCliente(id);
        return RedirectToAction("Index");
    }
}