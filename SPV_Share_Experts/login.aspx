<%@ Page Title="" Language="C#" MasterPageFile="~/main1.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        document.getElementById("loginmenu").className = "active";
        document.getElementById("sign").style.display= "none";
    </script>
    <div class="big">
        <h1>User Login</h1>
        <div class="row gtr-uniform">
            <div class="col-3 col-12-xsmall">
                <asp:Label ID="Label3" runat="server" Text="Email ID"></asp:Label>
            </div>
            <div class="col-6 col-12-xsmall">
                <asp:TextBox ID="email_id_TXT" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="email_id_TXT" ErrorMessage="Enter a valid Email ID" ValidationExpression="\S+@\S+\.\S+"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row gtr-uniform">
            <div class="col-3 col-12-xsmall">
                <asp:Label ID="Label4" runat="server" Text="Password"></asp:Label>
            </div>
            <div class="col-6 col-12-xsmall">
                <asp:TextBox ID="pass_TXT" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="pass_TXT" ErrorMessage="Between 8 and 20 char Long" ValidationExpression="\w{8,20}"></asp:RegularExpressionValidator>
            </div>
        </div>
         <asp:Button ID="sign_up_BTN" runat="server" Text="Login" OnClick="loginBTN_Click" CssClass="button" />
        <br />
        <br />
<a href="sign_up.aspx" class="nav">Dont Have A account?<br />
        Sign Up!
    </a>

      
    </div>
    
</asp:Content>

