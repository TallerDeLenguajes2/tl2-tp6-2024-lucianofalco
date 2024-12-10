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
        var model = new ViewLogin();
        model.autentificacion = HttpContext.Session.GetString("autentificacion") == "true" ;
        return View(model);
    }

    [HttpPost]
    public IActionResult Index(ViewLogin model)
    {
        var usuario =_repoUser.BuscarUsuario(model.username , model.password);
            if (usuario is not null)
            {
                HttpContext.Session.SetString("autentificacion" , "true");
                HttpContext.Session.SetString("Usuario" , usuario.Usuario);
                HttpContext.Session.SetString("Password" , usuario.Password);
                HttpContext.Session.SetString("Roll" , usuario.Roll);
                HttpContext.Session.SetString("Nombre" , usuario.Nombre);
                ViewData["esAdmin"] = HttpContext.Session.GetString("Roll")=="Admin";
                ViewData["esCliente"] = HttpContext.Session.GetString("Roll")=="Cliente";
                if (usuario.Roll == "Admin")
                {
                    return RedirectToAction("PanelAdministracion");
                }
                return View(new ViewUser(usuario));
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
    public IActionResult PanelAdministracion()
    {
        ViewData["esAdmin"] = HttpContext.Session.GetString("Roll") == "Admin";
        return View(_repoUser.ListarUsuarios());
    }
    [HttpPost]
    public IActionResult EliminarUserPost(int id)
    {
        var p = _repoUser.EliminarUsuario(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Salir()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}