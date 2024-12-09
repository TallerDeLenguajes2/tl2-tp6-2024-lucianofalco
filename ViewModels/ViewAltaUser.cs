using System.ComponentModel.DataAnnotations;
public class ViewUser
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
    public string Nombre  {get;set;} 

    [Required(ErrorMessage = "El Usuario es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
    public string Usuario {get;set;} 
    public string Password {get;set;}
    public string Roll {get;set;}

    public ViewUser()
    {
    }
    
    public ViewUser(User user)
    {
        this.Nombre = user.Nombre ;
        this.Usuario = user.Usuario ; 
        this.Roll = user.Roll;
        this.Password = user.Password;
    }


}