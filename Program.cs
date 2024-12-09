var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IClientesRpository , ClienteRepository>();
builder.Services.AddSingleton<IPresupuestoRepositoy , PresupuestosRepository>();
builder.Services.AddSingleton<IProductoRepository , ProductoRepository>();
builder.Services.AddSingleton<IUsarioRepository , UsuarioRepository>();
var CadenaDeConexion = builder.Configuration.GetConnectionString("SqliteConexion")!.ToString();
builder.Services.AddSingleton(CadenaDeConexion);
// Habilitar servicios de sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible desde HTTP, no JavaScript
    options.Cookie.IsEssential = true; // Necesario incluso si el usuario no acepta cookies
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();

/*
    por que cuando esta vacio el formulario se rompe
    por que hay 2 controladores por vistas. se puede resolver de otra forma ? 
    por que los metodos solamente pueden ser post y get
    con que criterio hago los endpoints
    como implementarlo con ventanas modal
    como implementarlo con iconos
    consultar por el appseting.json en el caso del stringConnection
*/