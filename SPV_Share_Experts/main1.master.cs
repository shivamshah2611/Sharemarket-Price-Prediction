using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class main1 : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["name"]!=null)
        {
            nameLBL.Text = "Welcome, " + Session["name"].ToString()+".";
        }
        if (Session["id"] != null)
        {
            signoutBTN.Visible = true;
            signinBTN.Visible = false;
        }
        else
        {
            signoutBTN.Visible = false;
            signinBTN.Visible = true;
        }
    }
    protected void signinBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("login.aspx");
    }
    protected void signoutBTN_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("home.aspx");
    }
    protected void adminPageBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("adminLoginPage.aspx");
    }
}
