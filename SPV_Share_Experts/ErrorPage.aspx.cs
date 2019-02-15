using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["ex"]!=null)
        {
            Label1.Text = "Error";
            Label2.Text = Session["ex"].ToString();
            Session["ex"] = null;
        }
        else
        {
            Response.Redirect("home.aspx");
        }
    }
}