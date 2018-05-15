<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Notificacion.aspx.cs" Inherits="DataExpressWeb.Notificacion" %>

<asp:Content ID="cpCabecera" runat="server" ContentPlaceHolderID="cpCabecera">
    <%--<script type="text/javascript">
        function BtnError() {
            history.go(-1);
        }
    </script>--%>
</asp:Content>
<asp:Content ID="cpCuerpoSup" runat="server" ContentPlaceHolderID="cpCuerpoSup">
    <div class="row">
        <div class="col-md-12">
            <div class="alert alert-danger">
                <span><strong>No se ha podido ingresar a la página.</strong> Se ha enviado una notificación a la mesa de ayuda (</span><a href="mailto:helpdesk@dataexpressintmx.com">helpdesk@dataexpressintmx.com</a><span>)</span>&nbsp;
                <button id="bDetallesError" runat="server" class="btn btn-primary btn-xs" type="button" data-toggle="collapse" data-target="#collapseError" aria-expanded="false" aria-controls="collapseError" visible="false">
                    <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>&nbsp;Ver Detalles
                </button>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:LinkButton ID="Button1" runat="server" CssClass="btn btn-primary" OnClick="BtnError">
                        <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>&nbsp;Regresar
            </asp:LinkButton>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="collapse" id="collapseError">
                <br />
                <pre style="text-align: left !important;" class="alert alert-danger">
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                    </pre>
            </div>
        </div>
    </div>
</asp:Content>
