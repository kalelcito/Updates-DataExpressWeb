<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="~/recepcion/SubirArchivos.aspx.cs" Inherits="DataExpressWeb.recepcion.SubirArchivos" Async="true" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .dropzone {
            border: 2px dashed #0087F7;
            border-radius: 5px;
            /*background: transparent;*/
            background: rgba(0,0,0,0.1) !important;
            padding: 0;
        }

            .dropzone .dz-preview.dz-image-preview {
                background: transparent;
            }
    </style>
    <script type="text/javascript">
        function xmlUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1).toUpperCase();
            if (ext != 'xml' && ext != 'XML') {
                var err = new Error();
                err.message = 'Solo se aceptan archivos con extensión ".xml"';
                throw (err);
                return false;
            }
            return true;
        }
        function pdfUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1).toUpperCase();
            if (ext != 'pdf' && ext != 'PDF') {
                var err = new Error();
                err.message = 'Solo se aceptan archivos con extensión ".pdf"';
                throw (err);
                return false;
            }
            return true;
        }
        function ordenUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            return true;
        }
        function UploadError(sender, args) {
            var ctrlText = sender.get_element().getElementsByTagName("input");
            var ctrlSpan = sender.get_element().getElementsByTagName("span");
            for (var i = 0; i < ctrlText.length; i++) {
                if (ctrlText[i].type == "text" || ctrlText[i].type == "hidden") {
                    ctrlText[i].value = "";
                }
            }
            alertBootBox(args.get_errorMessage(), 4);
        }
        function sendFilesFromDropZone(id, objList) {
            $("div#divLoading").addClass('show');
            var result = false;
            var data = "{'id':'" + id + "', 'data':'" + JSON.stringify(objList) + "'}";
            $.ajax({
                type: "POST",
                url: "./SubirArchivos.aspx/DropZone_UploadedComplete",
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                timeout: 1200000,
                success: function (response, textStatus, jqXHR) {
                    $("div#divLoading").removeClass('show');
                    eval(response.d);
                    result = true;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("div#divLoading").removeClass('show');
                    alertBootBox('No se pudieron cargar los archivos, intente quitarlos y volverlos a cargar, de lo contrario no serán registrados:<br/><br/>' + errorThrown, 4);
                },
                complete: function (jqXHR, textStatus) {
                    $("div#divLoading").removeClass('show');
                }
            });
            return result;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    SUBIR ARCHIVOS
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4"><strong>COMPROBANTES A SUBIR:</strong></div>
                <div class="col-md-4"></div>
            </div>
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-2">
                    <asp:TextBox ID="tbComprobantes" runat="server" CssClass="form-control input-number" Style="text-align: center;" min="1" max="10" Width="100%" Text="1"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:LinkButton ID="lbCargar" runat="server" CssClass="btn btn-primary" OnClientClick="" OnClick="tbComprobantes_TextChanged" Width="100%">Cargar</asp:LinkButton>
                </div>
                <div class="col-md-4"></div>
            </div>
            <br />
            <asp:Panel ID="panelComprobantes" runat="server" Width="100%" Style="max-height: 350px" ScrollBars="Vertical">
            </asp:Panel>
            <br />
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <asp:LinkButton ID="lbSubir" runat="server" CssClass="btn btn-primary" OnClientClick="" OnClick="lbSubir_Click" Visible="false" Style="width: 100%;">Subir</asp:LinkButton>
                </div>
                <div class="col-md-4"></div>
            </div>
            <asp:SqlDataSource ID="SqlDataSourceTipoProveedor" runat="server" SelectCommand="SELECT IdTipo, nombre FROM Cat_TiposProveedor WHERE visible = 'True' AND eliminado = 'False'"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
