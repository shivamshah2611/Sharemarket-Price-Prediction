using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for user
/// </summary>
public class User
{
    private int id;
    private string email;
    private string pass;
    private string name; 
    
    public User()
    { }
    public User(string email,string pass,string name)
    {
        this.Email = email;
        this.Pass = pass;
        this.Name = name;
    }

    public string Email { 
        get{ return email;} 
        set{ email = value;}
    }
    public string Pass { 
        get{ return pass;}
        set{ pass = value;}
    }
    public string Name { 
        get{ return name; }
        set{ name = value;}
    }
    public int Id { 
        get {return id; }
        set{ id = value; }
    }
}