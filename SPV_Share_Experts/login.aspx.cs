using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["id"] != null)
        {
            Response.Redirect("home.aspx");
        }
      
    }

    protected void loginBTN_Click(object sender, EventArgs e)
    {
        User u = new User();
        u.Email = email_id_TXT.Text;
        u.Pass = pass_TXT.Text;

        DatabaseOperations db = new DatabaseOperations(Server.MapPath("~"));
        bool code=db.VerifyUser(ref u);

        if (code)
        {
            Session["id"] = u.Id;
            Session["name"] = u.Name;
            Response.Redirect("home.aspx");
        }
        else
        {
            Response.Write("<script>alert('Login Unsuccessful')</script>");
        }
    }
}