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
        return View();
    }

    [HttpPost]
    public IActionResult CrearCliente(Cliente cliente)
    {
        Cliente c = repoClientes.AgregarCliente(cliente);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        var c = repoClientes.ListarClientes().Find(c=>c.idCliente == id) ;
        return View(c);
    }

        [HttpPost]
    public IActionResult ModificarClientePost(int id , Cliente cliente){
        var c = repoClientes.ModificarCliente(id , cliente);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EliminarCliente(int id){
        var p = repoClientes.GetCliente(id);
        return View(p);
    }
    [HttpPost]
    public IActionResult EliminarClientePost(int id){
        var p = repoClientes.EliminarCliente(id);
        return RedirectToAction("Index");
    }
}