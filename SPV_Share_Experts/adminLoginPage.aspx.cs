using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class adminLoginPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
        DatabaseOperations db = new DatabaseOperations(Server.MapPath("~"));
        bool code = db.VerifyAdmin(username.Text.Trim(),password.Text.Trim());

        if (code)
        {
            Response.Redirect("home.aspx");
        }
        else
        {
            Response.Write("<script>alert('Login Unsuccessful')</script>");
        }
    }
}