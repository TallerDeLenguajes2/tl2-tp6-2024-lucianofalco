using System.ComponentModel.DataAnnotations;

public class ViewLogin
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string username { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string password { get; set; }
    public bool autentificacion {get;set;}


    public ViewLogin()
    {
    }
}