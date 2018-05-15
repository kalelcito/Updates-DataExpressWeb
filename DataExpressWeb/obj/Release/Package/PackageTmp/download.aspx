<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="download.aspx.cs" Inherits="DataExpressWeb.FormularioWeb11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div id="divLost" runat="server" style="display: none;">
        <div class="alert alert-danger">
            <span><b>El archivo que intentó descargar no existe.</b> Favor de comunicarse con la Mesa de ayuda (</span><a href="mailto:helpdesk@dataexpressintmx.com">helpdesk@dataexpressintmx.com</a><span>)</span><br />
            <span>Path:
                <asp:Label ID="lblPath" runat="server" Text="-"></asp:Label></span>
        </div>
        <p>
            <asp:LinkButton ID="Button1" runat="server" Text="Regresar" CssClass="btn btn-primary" OnClick="BtnError">
            Regresar
            </asp:LinkButton>
        </p>
    </div>
</asp:Content>
