using System.Diagnostics;
using System.Net.Cache;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_lucianofalco.Models;

namespace tl2_tp6_2024_lucianofalco.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;

    private readonly IUsarioRepository _repoUser;

    public UsuarioController(ILogger<UsuarioController> logger , IUsarioRepository repoUser)
    {
        _logger = logger;
        _repoUser = repoUser;
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View(new ViewUser());
    }

    [HttpPost]
    public IActionResult Index(ViewUser model)
    {
        if (ModelState.IsValid)
        {
            var usuario =_repoUser.BuscarUsuario(model.Usuario , model.Password);
            if (usuario is not null)
            {
                var userVm = new ViewUser(usuario);
                return View(userVm);
            }
        }
        return RedirectToAction("Login");
    }




    [HttpGet]
    public IActionResult AltaUser()
    {
        return View(new ViewUser());
    }

    [HttpPost]
    public IActionResult CrearUser(ViewUser uVM)
    {
        if (ModelState.IsValid)
        {
            var c = new User(uVM);
            _repoUser.AgregarUsuario(c);
            return RedirectToAction("Index");
        }
        else return View(new ViewUser());
    }

    [HttpGet]
    public IActionResult ModificarUser(int id)
    {
        var user = _repoUser.ListarUsuarios().Find(u => u.Id == id);
        var model = new ViewUser(user);
        return View(model);
    }

    [HttpPost]
    public IActionResult ModificarUserPost(int id, ViewUser UserVM)
    {
        if(ModelState.IsValid){
        var c = new User();
        _repoUser.ModificarUsuario(id, c);
        return RedirectToAction("Index");
        }
        else return View(new ViewUser());
    }

    [HttpGet]
    public IActionResult EliminarUser(int id)
    {
        var p = _repoUser.GetUsuario(id);
        if (p == null)
        {
            return RedirectToAction("Index", new { error = "Usuario no encontrado" });
        }
        return View(p);
    }
    [HttpPost]
    public IActionResult EliminarUserPost(int id)
    {
        var p = _repoUser.EliminarUsuario(id);
        return RedirectToAction("Index");
    }
}