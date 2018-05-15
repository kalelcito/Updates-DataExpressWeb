<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InterfazExcel.aspx.cs" Inherits="DataExpressWeb.nuevo.InterfazExcel" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function FileUploadStarted(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            if (ext != 'xls' && ext != 'XLS' && ext != 'xlsx' && ext != 'XLSX') {
                var err = new Error();
                err.message = 'Solo se aceptan archivos con extensiones ".xls" y ".xlsx"';
                throw (err);
                return false;
            }
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    Interfaz Oracle/Excel
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="rowsSpaced">
                <div class="row">
                    <div class="col-md-4">
                        CARTERA DE CLIENTES (.XLS)<asp:LinkButton ID="lbDownloadCartera" runat="server" OnClick="lbDownloadCartera_Click" data-toggle="tooltip" data-placement="top" title="Descargar cartera existente" Visible="false">
                                <%--<span class="glyphicon glyphicon-comment" aria-hidden="true"></span></asp:HyperLink>--%>
                                    <i class="fa fa-download fa-2x" aria-hidden="true"></i></asp:LinkButton>
                    </div>
                    <div class="col-md-4">
                        CARTERA DE INTERCOMPAÑÍAS (.XLS)<asp:LinkButton ID="lbDownloadIntercompany" runat="server" OnClick="lbDownloadIntercompany_Click" data-toggle="tooltip" data-placement="top" title="Descargar cartera existente" Visible="false">
                                <%--<span class="glyphicon glyphicon-comment" aria-hidden="true"></span></asp:HyperLink>--%>
                                    <i class="fa fa-download fa-2x" aria-hidden="true"></i></asp:LinkButton>
                    </div>
                    <div class="col-md-4">ARCHIVO DE INTERFAZ (.XLS)</div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="FileUploadStarted" runat="server" ID="fileCartera" OnUploadedComplete="fileCartera_UploadedComplete" accept="application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                    </div>
                    <div class="col-md-4">
                        <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="FileUploadStarted" runat="server" ID="fileIntercomp" OnUploadedComplete="fileIntercomp_UploadedComplete" accept="application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                    </div>
                    <div class="col-md-4">
                        <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="FileUploadStarted" runat="server" ID="fileInterfaz" OnUploadedComplete="fileInterfaz_UploadedComplete" accept="application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:LinkButton ID="lbProcesar" runat="server" CssClass="btn btn-primary" OnClick="lbProcesar_Click">Procesar</asp:LinkButton>
                    </div>
                </div>
            </div>
            <br />
            <div id="divFacturas" runat="server" style="display: none;">
                <asp:Label ID="lCountFacturas" runat="server" Text="Registros"></asp:Label>
                <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvFacturas_PageIndexChanging" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" OnSorting="gvFacturas_Sorting" fixed-height>
                    <Columns>
                        <asp:BoundField DataField="CuentaContable" HeaderText="Cta Contable" SortExpression="CuentaContable" />
                        <asp:BoundField DataField="NombreHuesped" HeaderText="Huesped" SortExpression="NombreHuesped" />
                        <asp:BoundField DataField="BookingId" HeaderText="Referencia" SortExpression="BookingId" />
                        <asp:BoundField DataField="NroFactura" HeaderText="No Factura" SortExpression="NroFactura" />
                        <asp:BoundField DataField="FechaFactura" HeaderText="Fecha" SortExpression="FechaFactura" />
                        <asp:BoundField DataField="In" HeaderText="Check-In" SortExpression="In" />
                        <asp:BoundField DataField="Out" HeaderText="Check-Out" SortExpression="Out" />
                        <asp:BoundField DataField="TotalMxp" HeaderText="Total" SortExpression="TotalMxp" />
                    </Columns>
                    <EmptyDataRowStyle CssClass="table empty" />
                    <EmptyDataTemplate>
                        No existen datos.
                    </EmptyDataTemplate>
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                </asp:GridView>
            </div>
            <div id="divClientes" runat="server" style="display: none;">
                <asp:Label ID="lCountClientes" runat="server" Text="Registros"></asp:Label>
                <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvClientes_PageIndexChanging" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" OnSorting="gvClientes_Sorting" fixed-height>
                    <Columns>
                        <asp:BoundField DataField="Codigo" HeaderText="Código" SortExpression="Codigo" />
                        <asp:BoundField DataField="Rfc" HeaderText="RFC" SortExpression="Rfc" />
                        <asp:BoundField DataField="RazonSocial" HeaderText="Razon Social" SortExpression="RazonSocial" />
                        <asp:TemplateField HeaderText="Existente">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbExistente" runat="server" Checked='<%# Eval("Existente") %>' Enabled="false" />
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
            <div class="row" id="rowFacturar" runat="server" style="display: none;">
                <br />
                <div class="col-md-12">
                    <asp:LinkButton ID="lbFacturar" runat="server" CssClass="btn btn-primary" OnClick="lbFacturar_Click">Facturar</asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpPie" runat="server">
</asp:Content>
