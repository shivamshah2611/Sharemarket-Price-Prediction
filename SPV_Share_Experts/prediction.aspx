<%@ Page Title="" Language="C#" MasterPageFile="~/main1.master" AutoEventWireup="true" CodeFile="prediction.aspx.cs" Inherits="prediction" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script>
        document.getElementById("predictmenu").className = "active";
    </script>
    <script>
        $(function () {
    $("#ContentPlaceHolder1_CodesList").on("change", function () {
        $("#ContentPlaceHolder1_stockcodeTXT").val($(this).val());
    });
        }); 
    </script>
    <div class="big">
        <div class="row gtr-uniform" style="padding-top:20px">
            <div class="col-3 col-12-xsmall">
                <asp:Label ID="Label1" runat="server" Text="Stock Code"></asp:Label>
            </div>
            <div class="col-6 col-12-xsmall">
                <asp:TextBox ID="stockcodeTXT" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="stockcodeTXT" ErrorMessage="Stock Code Cannot be Empty" ValidationGroup="Group"></asp:RequiredFieldValidator>
            <br />
                 <asp:Button ID="predict" runat="server" OnClick="predict_Click" Text="Predict"  ValidationGroup="Group"/>
            </div>
            <div class="col-3 col-12-xsmall">
            <asp:DropDownList ID="CodesList" runat="server">
            </asp:DropDownList>
                <br />
                 <asp:Button ID="saveBTN" runat="server" CssClass="button small" OnClick="saveBTN_Click" ValidationGroup="Group" Text="Save Code" />
                 <asp:Button ID="deleteBTN" runat="server" CssClass="button small" Text="Delete Code" ValidationGroup="Group" OnClick="deleteBTN_Click" />
            </div>
        </div>
        <br />
         <br />
         <br />

            <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
            <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
            <br/>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            <br/>

        </div>

  
</asp:Content>

