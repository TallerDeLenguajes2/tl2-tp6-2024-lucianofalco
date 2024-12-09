using System.Security.Principal;

public class User
{

    public int Id {get;set;}
    public string Nombre {get;set;}
    public string Usuario {get;set;}

    public string Password{get;set;}
    public string Roll {get;set;}

    public User()
    {
    }

    
    public User(ViewUser uVM)
    {
        this.Nombre= uVM.Nombre ;
        this.Usuario= uVM.Usuario ;
        this.Password= uVM.Password ;
        this.Roll= uVM.Roll ;
    }

    
    
}