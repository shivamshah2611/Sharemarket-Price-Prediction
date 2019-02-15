using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

public partial class sign_up : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["id"]!= null)
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void sign_up_BTN_Click(object sender, EventArgs e)
    {
        string email = email_id_TXT.Text;
        string pass = pass_TXT.Text;
        string name = name_TXT.Text;

        string hashpass = GetHashPass(pass);

        User u = new User(email, hashpass, name);
        DatabaseOperations db = new DatabaseOperations(Server.MapPath("~"));
        int code=db.InsertUser(ref u);

        if (code == 1)
        {
            Session["id"] = u.Id;
            Session["name"] = u.Name;
            Response.Redirect("home.aspx");
        }
        else
        {
            Response.Write("<script>alert('Sign Up Unsuccessful!<br> Try Again!')</script>");
        }
    }

    string GetHashPass(string pass)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        string savedPasswordHash = Convert.ToBase64String(hashBytes);
        return savedPasswordHash;
    }
}