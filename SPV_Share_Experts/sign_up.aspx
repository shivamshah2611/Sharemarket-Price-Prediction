<%@ Page Title="" Language="C#" MasterPageFile="~/main1.master" AutoEventWireup="true" CodeFile="sign_up.aspx.cs" Inherits="sign_up" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="big">
        <h1>User Signup</h1>
        
        <div class="row gtr-uniform">
            <div class="col-3 col-12-xsmall">
                <asp:Label ID="Label1" runat="server" Text="Email ID"></asp:Label>
            </div>
            <div class="col-6 col-12-xsmall">
                <asp:TextBox ID="email_id_TXT" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="email_id_TXT" ErrorMessage="Enter a valid Email ID" ValidationExpression="\S+@\S+\.\S+"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row gtr-uniform">
            <div class="col-3 col-12-xsmall">
                <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
            </div>
            <div class="col-6 col-12-xsmall">
                <asp:TextBox ID="pass_TXT" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="pass_TXT" ErrorMessage="Between 8 and 20 char Long" ValidationExpression="\w{8,20}"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row gtr-uniform">
            <div class="col-3 col-12-xsmall">
                <asp:Label ID="Label3" runat="server" Text="Name"></asp:Label>
            </div>
            <div class="col-6 col-12-xsmall">
                <asp:TextBox ID="name_TXT" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="name_TXT" ErrorMessage="Name cannot be empty"></asp:RequiredFieldValidator>
            </div>
        </div>
        <asp:Button ID="sign_up_BTN" runat="server" Text="Sign Up" OnClick="sign_up_BTN_Click" CssClass="button" />
    </div>
   
</asp:Content>

