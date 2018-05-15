<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="helpdesk.aspx.cs" Inherits="DataExpressWeb.configuracion.Herramientas.Helpdesk" ValidateRequest="false" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function limpiarFiles() {
            clearContent("<%= FileUpload1.ClientID %>");
        }
        function clearContent(id) {
            var asyncFileUpload = $get(id);
            var txts = asyncFileUpload.getElementsByTagName("input");
            for (var i = 0; i < txts.length; i++) {
                if (txts[i].type == "text") {
                    txts[i].value = "";
                    txts[i].style.backgroundColor = "white";
                }
            }
        }
        function attachmentUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1).toLowerCase();
            if (ext != 'txt' && ext != 'jpg' && ext != 'jpeg' && ext != 'png' && ext != 'log' && ext != 'xml' && ext != 'zip' && ext != 'rar' && ext != 'pdf') {
                limpiarFiles();
                alertBootBox('Solo se aceptan archivos con las siguientes extensiones:<br/>.txt<br/>.jpg<br/>.jpeg<br/>.png<br/>.log<br/>.xml<br/>.pdf<br/>.zip<br/>.rar<br/>Intentelo de nuevo.', 4);
                return false;
            }
            return true;
        }
        function uploadError(sender) {
            limpiarFiles();
            alertBootBox('El archivo no se pudo cargar en el servidor. Intentelo de nuevo.', 4);
        }
        window.onload = function () {
            $('#divContainer').css('display', 'none');
        }
        function validarIncidencia() {
            var codTick = $('#<%= tbCodTick.ClientID %>');
            var email = $('#<%= tbEmail.ClientID %>');
            var descripcion = $('#<%= Asunto0.ClientID %>');
            var mensaje = $('#<%= tbCodTick.ClientID %>');
            if (codTick.val() == '') {
                alertBootBox("Tiene que especificar un código de Ticket", 4);
                return false;
            } else if (email.val() == '') {
                alertBootBox("Tiene que especificar un E-Mail", 4);
                return false;
            } else if (descripcion.val() == '') {
                alertBootBox("Tiene que especificar una descripción", 4);
                return false;
            } else if ($('#summernoteDiv').summernote('isEmpty')) {
                alertBootBox("Tiene que especificar un Mensaje", 4);
                return false;
            }
            save();
            return true;
        }
        function validarConsultar() {
            var codTick = $('#<%= tbCodTick.ClientID %>');
            if (codTick.val() == '') {
                alertBootBox("Tiene que especificar un código de Ticket", 4);
                return false;
            } else {
                save();
                return true;
            }
        }
        function actualizaPagina() {
            window.location.href = ResolveUrl('~/configuracion/Herramientas/helpdesk.aspx');
        }
        function save() {
            setSn(ReformatTextEditor(), '#<%= txtEditor.ClientID %>');
        }
        function recargarEditor() {
            var val = $('#<%= txtEditor.ClientID %>').val();
            setSn(val, '#<%= txtEditor.ClientID %>');
        }
    </script>
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="cpTitulo" runat="server">
    SOLICITAR TICKET DE SOPORTE
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lbNombreEmp" runat="server" Text="Nombre de la Empresa" Font-Size="X-Large" Font-Bold="true"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lbNbC" runat="server" Text="Nombre" CssClass="" Font-Bold="true"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lbFchC" runat="server" Text="Fecha" Style="text-align: left" Font-Bold="true"></asp:Label>
                </div>
            </div>
            <br />
            <div class="rowsSpaced">
                <div class="row">
                    <div class="col-md-12">
                        <asp:LinkButton ID="btNIncidencia" runat="server" CssClass="btn btn-primary" Text="Nueva Incidencia" OnClick="btNIncidencia_Click"></asp:LinkButton>
                        <asp:Button ID="Limpiar" runat="server" CssClass="btn btn-primary" OnClientClick="actualizaPagina()" UseSubmitBehavior="false" Text="Limpiar"></asp:Button>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lbCodigoT" runat="server" Text="Codigo de Ticket:" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4"></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="tbCodTick" runat="server" Style="text-align: center" CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:LinkButton ID="btConsultarTck" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="btConsultarTck_Click" OnClientClick="return validarConsultar();"></asp:LinkButton>
                    </div>
                </div>
            </div>
            <div id="dibStatus" runat="server" visible="false">
                <br />
            </div>
            <div id="divNueva" runat="server" visible="false">
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lbEmail" runat="server" Text="E-Mail:"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <asp:TextBox ID="tbEmail" runat="server" Style="text-align: center" CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="col-md-6"></div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lbDescripcionC" runat="server" Text="Descripción:"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <asp:TextBox ID="Asunto0" runat="server" Style="text-align: center" CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="col-md-3"></div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lbMsjC" runat="server" Text="Mensaje:"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div id="divContainer" class="col-md-12">
                        <asp:HiddenField ID="txtEditor" runat="server" Value="" />
                        <div id="summernoteDiv"></div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lblArchivo" runat="server" Text="Archivo:"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <asp:AsyncFileUpload OnClientUploadError="uploadError" runat="server" ID="FileUpload1" OnClientUploadStarted="attachmentUpload" OnUploadedComplete="FileUpload1_UploadedComplete" Style="text-align: center" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <asp:LinkButton ID="btEnvio" runat="server" CssClass="btn btn-primary" Text="Enviar Incidencia" OnClientClick="return validarIncidencia();" OnClick="btEnvio_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
