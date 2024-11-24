using System.Diagnostics;
using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_lucianofalco.Models;

namespace tl2_tp6_2024_lucianofalco.Controllers;

public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;
    private ClienteRepository repoClientes;

    public ClienteController(ILogger<ClienteController> logger)
    {
        _logger = logger;
        repoClientes = new ClienteRepository();
    }


    [HttpGet]
    public IActionResult Index()
    {
        return View(repoClientes.ListarClientes());
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
            repoClientes.AgregarCliente(c);
            return RedirectToAction("Index");
        }
        else return View(new ViewCliente());
    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        var cliente = repoClientes.ListarClientes().Find(c => c.idCliente == id);
        ViewCliente model = new ViewCliente(cliente);
        return View(model);
    }

    [HttpPost]
    public IActionResult ModificarClientePost(int id, ViewCliente clienteVM)
    {
        if(ModelState.IsValid){
        var c = new Cliente(clienteVM);
        repoClientes.ModificarCliente(id, c);
        return RedirectToAction("Index");
        }
        else return View(new ViewCliente());
    }

    [HttpGet]
    public IActionResult EliminarCliente(int id)
    {
        var p = repoClientes.GetCliente(id);
        if (p == null)
        {
            return RedirectToAction("Index", new { error = "Producto no encontrado" });
        }
        return View(p);
    }
    [HttpPost]
    public IActionResult EliminarClientePost(int id)
    {
        var p = repoClientes.EliminarCliente(id);
        return RedirectToAction("Index");
    }
}