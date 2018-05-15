<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FacturaGlobalMicros.aspx.cs" Inherits="DataExpressWeb.FacturaGlobalMicros" Async="true" AsyncTimeout="6000" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function realizarComprobante() {
            if (confirm('¿Seguro/a que desea facturar los Tickets seleccionados?')) {
                $('#<%= progressCrear.ClientID %>').css('display', 'inline');
                return true;
            }
            return false;
        }
        function facturar() {
            if (confirm('¿Seguro/a que desea facturar el comprobante con los conceptos originales de OnQ?')) {
                realizarComprobante();
                return true;
            } else {
                return false;
            }
        }
        function changeText(html, id) {
            $(id).val(html).change();
            return false;
        }
    </script>
    <style type="text/css">
        #Background {
            background-color: #F0F0F0;
            bottom: 0px;
            filter: alpha(opacity=70);
            left: 0px;
            margin: 0;
            overflow: hidden;
            padding: 0;
            position: fixed;
            right: 0px;
            top: 0px;
            z-index: 100000;
        }

        #Progress {
            background-color: #FFFFFF;
            background-position: center;
            background-repeat: no-repeat;
            border: 1px solid Gray;
            filter: alpha(opacity=70);
            height: 20%;
            left: 40%;
            position: fixed;
            top: 40%;
            width: 20%;
            z-index: 100001;
        }

        .bgRow {
            background-color: transparent;
        }

        .norRow {
            background-color: #337AB7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    FACTURA GLOBAL MICROS
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div class="rowsSpaced">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="rowsSpaced">
                    <div class="row">
                        <%--  <div class="col-md-2">
                            <h5>Año:</h5>
                            <div class="input-group date">
                                <asp:TextBox ID="tbFecha" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">ss
                                    <i class="fa fa-calendar-o"></i>
                                </span>
                            </div>
                        </div>--%>
                        <div class="col-md-2">
                            <h5>Serie Sucursal:</h5>
                            <asp:TextBox ID="tbsucursal" runat="server" MaxLength="10" CssClass="form-control upper" Style="text-align: center" placeholder="AAAAAAAAAA" ReadOnly="false"></asp:TextBox>
                            <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="tbsucursal" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZ"></asp:FilteredTextBoxExtender>--%>
                        </div>
                        <div class="col-md-2">
                            <h5>Año:</h5>
                            <asp:TextBox ID="tbFecha" runat="server" MaxLength="4" CssClass="form-control" Style="text-align: center" placeholder="2000" ReadOnly="false"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="tbFecha" ValidChars="0123456789"></asp:FilteredTextBoxExtender>

                        </div>
                        <%-- <div class="col-md-2">
                            <h5>Mes:</h5>
                            <div class="input-group date">
                                <asp:TextBox ID="tbFechaFin" runat="server" Style="text-align: center" placeholder="" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">
                                    <i class="fa fa-calendar-o"></i>
                                </span>
                            </div>
                        </div>--%>
                        <%-- <div class="col-md-2">
                            <h5>Mes:</h5>
                            <asp:TextBox ID="tbFechaFin" runat="server" MaxLength="2" CssClass="form-control" Style="text-align: center" placeholder="01" ReadOnly="false"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filtrotbdescrnot" runat="server" Enabled="True" TargetControlID="tbFechaFin" ValidChars="0123456789"></asp:FilteredTextBoxExtender>
                        </div>--%>
                        <div class="col-md-2">
                            <h5>Mes:</h5>
                            <asp:DropDownList ID="FechaFin" runat="server" Style="text-align: center;" CssClass="form-control" placeholder="Selecione mes " AppendDataBoundItems="true">
                                <asp:ListItem Value="01">Enero</asp:ListItem>
                                <asp:ListItem Value="02">Febrero</asp:ListItem>
                                <asp:ListItem Value="03">Marzo</asp:ListItem>
                                <asp:ListItem Value="04">Abril</asp:ListItem>
                                <asp:ListItem Value="05">Mayo</asp:ListItem>
                                <asp:ListItem Value="06">Junio</asp:ListItem>
                                <asp:ListItem Value="07">Julio</asp:ListItem>
                                <asp:ListItem Value="08">Agosto</asp:ListItem>
                                <asp:ListItem Value="09">Septiembre</asp:ListItem>
                                <asp:ListItem Value="10">Octubre</asp:ListItem>
                                <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                <asp:ListItem Value="12">Diciembre</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <h5>Serie Generación:</h5>
                            <asp:DropDownList ID="ddlSerie" runat="server" Style="text-align: center;" CssClass="form-control" DataSourceID="SqlDataSeries" DataTextField="serie" DataValueField="idSerie"></asp:DropDownList>
                        </div>

                        <div class="col-md-2">
                            <h5>&nbsp;</h5>
                            <asp:Button ID="bFacturar" runat="server" Text="Facturar" OnClick="GenerarTickets_Click" CssClass="btn btn-primary" Style="width: 100%;" />
                        </div>
                        <div class="col-md-2">
                            <h5>&nbsp;</h5>
                            <asp:Button ID="lbReporte" runat="server" Text="Generar Reporte" OnClick="GenerarReporte_Click" CssClass="btn btn-primary" Style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row form-inline">
                        <fieldset>
                            <div class="col-md-12">
                            </div>
                            <%-- <div class="form-group">
                                <asp:LinkButton ID="bLimpiarFiltros" runat="server" CssClass="btn btn-primary" Text="Limpiar Filtros"></asp:LinkButton>
                            </div>--%>
                            <%--  <div class="form-group">
                                <asp:LinkButton ID="bFacturar_click" runat="server" CssClass="btn btn-primary" Text="Facturar"></asp:LinkButton>
                            </div>--%>
                            <div class="form-group">
                                <h5>&nbsp;</h5>
                                <asp:Button ID="lbActualizar" runat="server" Text="Actualizar" OnClick="Actualizar_Click" CssClass="btn btn-primary" Style="width: 100%;" />
                            </div>

                        </fieldset>
                    </div>
                </div>
                <%-- <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <asp:Label ID="lCount" runat="server" Text="Registros" Visible="true"></asp:Label>
                    </div>
                </div>--%>
                <br />

            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:GridView ID="gvFacGlobWeb" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" DataSourceID="SqlDataSourceFacGlob">
                    <Columns>
                        <asp:TemplateField HeaderText="FECHA DE SOLICITUD" SortExpression="FechaSolicitud">
                            <ItemTemplate>
                                <asp:Label ID="FechaSoltd" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("FechaSolicitud").ToString()) ? DateTime.Parse(Eval("FechaSolicitud").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FechaSolicitud").ToString()).ToString("hh:mm:ss") : "" %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="nroTiquets" HeaderText="N° DE tikets" SortExpression="nroTiquets" />
                        <%-- <asp:BoundField DataField="Status" HeaderText="STATUS" SortExpression="Status" />--%>
                        <asp:TemplateField HeaderText="STATUS" SortExpression="status" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image runat="server" ImageUrl='<%# string.Format("~/Imagenes/{0}.png", Eval("status")) %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("status").ToString() == "1" ? "GENERADO" : (Eval("status").ToString() == "2" ? "En Proceso" : "Desconocido") %>' AlternateText='<%# Eval("status").ToString() %>'></asp:Image>
                            </ItemTemplate>
                            <ControlStyle Height="18px" Width="18px" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="FECHA FINAL" SortExpression="fechaFin">
                            <ItemTemplate>
                                <asp:Label ID="FechaFIn" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("fechaFin").ToString()) ? DateTime.Parse(Eval("fechaFin").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("fechaFin").ToString()).ToString("hh:mm:ss") : "" %>' Visible='<%# Eval("info").ToString() != null %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FECHA REPORTE">
                            <ItemTemplate>
                                <asp:Label ID="FechaReporte" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("info").ToString()) ? System.IO.Path.GetFileNameWithoutExtension(Eval("info").ToString()).Split(new[] { "_" }, StringSplitOptions.None)[1] : "-" %>'></asp:Label>
                                <%--<asp:Label ID="FechaReporte" runat="server" Text='<%# Eval-("info").ToString() %>'></asp:Label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descargar" SortExpression="info">
                            <ItemTemplate>
                                <asp:HyperLink ID="lbDescZip" runat="server" CssClass="btn btn-primary" NavigateUrl='<%# string.Format("~/download.aspx.?file={0}",Eval("info")) %>' EnableEventValidation="false" Visible='<%# Eval("status").ToString() == "1" %>'>Descargar</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle CssClass="table" />
                    <EmptyDataTemplate>
                        No existen datos.
                    </EmptyDataTemplate>
                    <FooterStyle />
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                    <PagerSettings />
                    <RowStyle />
                    <SelectedRowStyle />
                    <SortedAscendingCellStyle />
                    <SortedAscendingHeaderStyle />
                    <SortedDescendingCellStyle />
                    <SortedDescendingHeaderStyle />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:SqlDataSource ID="SqlDataSourceFacGlob" runat="server" SelectCommand="select FechaSolicitud,nroTiquets,CASE (status) WHEN 4 THEN 0 WHEN 5 THEN 1 ELSE 2 END AS status,fechaFin,info from Dat_FacGlobalWeb  where status >= 4 order by 1 desc"></asp:SqlDataSource>
        <div id="progressCrear" runat="server" style="display: none;">
            <div id="Background"></div>
            <div id="Progress">
                <h6>
                    <p style="text-align: center">
                        <b>Generando Comprobante(s) Electronico(s), Espere un momento... </b>
                    </p>
                </h6>
                <div class="progress progress-striped active">
                    <div class="progress-bar" style="width: 100%"></div>
                </div>
            </div>
        </div>
        <asp:SqlDataSource ID="SqlDataSeries" runat="server" SelectCommand="SELECT DISTINCT s.serie AS idSerie, s.serie FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipo = 2 AND tipoDoc = '01' AND m.idEmpleado = @idUser">
            <SelectParameters>
                <asp:SessionParameter Name="idUser" SessionField="idUser" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpPie" runat="server">
</asp:Content>
