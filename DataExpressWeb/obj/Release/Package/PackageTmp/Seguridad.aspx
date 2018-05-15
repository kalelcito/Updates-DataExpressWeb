<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
CodeBehind="Seguridad.aspx.cs" Inherits="DataExpressWeb.Seguridad" %>

<asp:Content ID="cpCabecera" runat="server" ContentPlaceHolderID="cpCabecera">
    <style type="text/css">
        .style1 { font-size: small; }
    </style>
</asp:Content>
<asp:Content ID="cpCuerpoSup" runat="server" ContentPlaceHolderID="cpCuerpoSup">
    <p>
        <br />
    </p>
    <div class="alert alert-danger">
        <span>Se estas ingresando a un sitio sin autorización, por favor consultar con su administrador.</span> 
    </div>
    <p>
        <br />
    </p>
</asp:Content>