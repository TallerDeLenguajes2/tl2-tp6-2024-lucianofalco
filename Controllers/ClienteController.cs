using System.Diagnostics;
using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_lucianofalco.Models;

namespace tl2_tp6_2024_lucianofalco.Controllers;

public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;
    // private ClienteRepository repoProd;

    public ClienteController(ILogger<ClienteController> logger)
    {
        _logger = logger;
        // repoProd = new ClienteRepository();
    }
}