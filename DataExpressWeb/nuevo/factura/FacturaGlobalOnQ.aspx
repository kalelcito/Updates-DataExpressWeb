<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FacturaGlobalOnQ.aspx.cs" Inherits="DataExpressWeb.nuevo.factura.FacturaGlobalOnQ" Async="true" AsyncTimeout="99999" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
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
        function UploadStarted(sender, args) {
            $('#<%= divPanelMaster.ClientID %>').css('display', 'none');
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            if (ext != 'xml' && ext != 'XML') {
                var err = new Error();
                err.message = 'Solo se aceptan archivos con extensión ".xml"';
                throw (err);
                return false;
            }
            return true;
        }
    </script>
    <style type="text/css">
        .panel {
            background: rgba(0,0,0,0) !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    FACTURA GLOBAL - MATCH CON REPORTE DE ONQ
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">XML MENSUAL ONQ</div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-5">
                    <asp:AsyncFileUpload ID="fileOnq" runat="server" Style="text-align: center;" OnUploadedComplete="fileOnq_UploadedComplete" OnClientUploadError="UploadError" OnClientUploadStarted="UploadStarted" />
                </div>
                <div class="col-md-1">
                    <asp:LinkButton ID="bCargarFile" runat="server" CssClass="btn btn-primary" Style="text-align: center;" OnClick="bCargarFile_Click">
                <i class="fa fa-upload" aria-hidden="true"></i>&nbsp;Cargar
                    </asp:LinkButton>
                </div>
                <div class="col-md-3"></div>
            </div>
            <br />
            <div id="divPanelMaster" runat="server" style="display: none;">
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8">
                        <asp:Label ID="lCountFacturas" runat="server" Text="Registros"></asp:Label>
                    </div>
                    <div class="col-md-2" style="text-align: right">
                        <asp:LinkButton ID="bGenerarExcel" runat="server" CssClass="btn btn-primary" Style="text-align: center;" OnClick="bGenerarExcel_Click">
                <i class="fa fa-download" aria-hidden="true"></i>&nbsp;Reporte
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvFacturas_PageIndexChanging" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" OnSorting="gvFacturas_Sorting" fixed-height>
                            <Columns>
                                <asp:BoundField DataField="NumeroConfirmacion" HeaderText="# Conf" SortExpression="NumeroConfirmacion" />
                                <asp:BoundField DataField="Habitacion" HeaderText="Habitacion" SortExpression="Habitacion" />
                                <asp:BoundField DataField="TotalPagar" HeaderText="Total Pagar" SortExpression="TotalPagar" />
                                <asp:BoundField DataField="TotalConfirmacion" HeaderText="Total Reporte" SortExpression="TotalConfirmacion" />
                                <asp:BoundField DataField="DiferenciaMontos" HeaderText="Diferencia" SortExpression="DiferenciaMontos" />
                                <asp:BoundField DataField="Usuario" HeaderText="Empleado" SortExpression="Usuario" />
                                <asp:BoundField DataField="FechaEmision" HeaderText="Fecha Emision" SortExpression="FechaEmision" />
                                <asp:BoundField DataField="Fechatimbrado" HeaderText="Fecha Timbre" SortExpression="Fechatimbrado" />
                                <asp:BoundField DataField="Uuid" HeaderText="UUID" SortExpression="Uuid" />
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="Id" runat="server" Value='<%# Bind("Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="table empty" />
                            <EmptyDataTemplate>
                                No existen datos.
                            </EmptyDataTemplate>
                            <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpPie" runat="server">
</asp:Content>
