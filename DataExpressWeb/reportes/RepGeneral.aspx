<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RepGeneral.aspx.cs" Inherits="DataExpressWeb.RepGeneral" Async="true" AsyncTimeout="9999" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
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

        .style5 {
        }

        .style6 {
            font-weight: bold;
            text-align: center;
        }

        .style7 {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function validateRepGeneral() {
            var valid = Boolean(Page_ClientValidate());
            if (!valid) {
                alertBootBox('Verifique que los datos sean correctos', 4);
            }
            return valid;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="rowsSpaced">
                <div class="row" align="center">
                    <div class="col-md-4"></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReporte" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlReporte_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">Seleccionar Reporte</asp:ListItem>
                            <asp:ListItem Value="General">General</asp:ListItem>
                            <asp:ListItem Value="Email">E-Mail</asp:ListItem>
                            <asp:ListItem Value="Conceptos">Conceptos</asp:ListItem>
                            <asp:ListItem Value="Contabilidad">Contabilidad</asp:ListItem>
                            <asp:ListItem Value="Tickets">Tickets</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceTipDoc" runat="server" SelectCommand="(SELECT '0' AS codigo, 'Todos' AS Descripcion) UNION (SELECT codigo, Descripcion FROM Cat_Catalogo1_C WHERE (tipo = 'Comprobante'))"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row" align="center">
                    <div class="col-md-4"></div>
                    <div class="col-md-1">
                        <strong>Emisor:</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlEmisor" runat="server" DataSourceID="SqlDataSourceEmisor" Style="text-align: center" CssClass="form-control bootstrap-select" DataTextField="RFCEMI" DataValueField="RFCEMI">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceEmisor" runat="server" SelectCommand="(SELECT '0' AS IDEEMI, 'Todos' AS RFCEMI) UNION (SELECT DISTINCT RFCEMI AS IDEEMI, RFCEMI FROM Cat_Emisor) ORDER BY IDEEMI, RFCEMI"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row" align="center">
                    <div class="col-md-4"></div>
                    <div class="col-md-1">
                        <strong>Receptor:</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlReceptor" runat="server" DataSourceID="SqlDataSourceReceptor" Style="text-align: center" CssClass="form-control bootstrap-select" DataTextField="RFCREC" DataValueField="RFCREC">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceReceptor" runat="server" SelectCommand="(SELECT '0' AS IDEREC, 'Todos' AS RFCREC) UNION (SELECT DISTINCT RFCREC AS IDEREC, RFCREC FROM Cat_Receptor) ORDER BY IDEREC, RFCREC ASC"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row" align="center">
                    <div class="col-md-4"></div>
                    <div class="col-md-1">
                        <strong>Desde:</strong>
                    </div>
                    <div class="col-md-3">
                        <div class="input-group date">
                            <asp:TextBox ID="tbFechaInicial" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="true" OnTextChanged="tbFechaInicial_TextChanged"></asp:TextBox>
                            <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                        </div>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row" align="center">
                    <div class="col-md-4"></div>
                    <div class="col-md-1">
                        <strong>Hasta:</strong>
                    </div>
                    <div class="col-md-3">
                        <div class="input-group date">
                            <asp:TextBox ID="tbFechaFinal" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                        </div>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row" align="center">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <strong><asp:Label ID="Label4" runat="server" Text="Tipo de Documento:"></asp:Label></strong>
                    </div>
                    <div class="col-md-5"></div>
                </div>
            </div>
            <div class="row" align="center">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlTipDoc" runat="server" DataSourceID="SqlDataSourceTipDoc" Style="text-align: center" CssClass="form-control" DataTextField="Descripcion" DataValueField="codigo" AutoPostBack="true" OnSelectedIndexChanged="ddlTipDoc_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="col-md-4"></div>
            </div>
            <div class="row" align="center">
                <div class="col-md-4"></div>
                <div class="col-md-2">
                    <strong>Serie:</strong>
                </div>
                <div class="col-md-2">
                    <strong>Estado:</strong>
                </div>
                <div class="col-md-4"></div>
            </div>
            <div class="row" align="center">
                <div class="col-md-4"></div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataSourcePtoEmi" CssClass="form-control" Style="text-align: center" DataTextField="serie" DataValueField="idSerie" AppendDataBoundItems="True">
                        <asp:ListItem Selected="True" Value="0">Todas</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSourcePtoEmi" runat="server" SelectCommand="SELECT idSerie, serie FROM Cat_Series WHERE tipoDoc = @tipo">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="0" Name="tipo" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlEstado" runat="server" Style="text-align: center" CssClass="form-control">
                        <asp:ListItem Value="0" Selected="True">Todos</asp:ListItem>
                        <asp:ListItem Value="E1">Autorizados</asp:ListItem>
                        <%--<asp:ListItem Value="E2">Pendiente</asp:ListItem>--%>
                        <asp:ListItem Value="E4">Anulados por Nota de Crédito</asp:ListItem>
                        <asp:ListItem Value="N0">No Autorizados</asp:ListItem>
                        <asp:ListItem Value="C0">Cancelados</asp:ListItem>
                        <asp:ListItem Value="C2">En proceso de Cancelación</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4"></div>
            </div>
            <div class="rowsSpaced">
                <div class="row" align="center">

                    <div class="col-md-4"></div>
                    <div class="col-md-2">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="bGenerar" CssClass="btn btn-primary" runat="server" OnClick="bGenerar_Click" Text="Generar">Generar</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="Button1_Click" Text="Limpiar">Limpiar</asp:LinkButton>
                    </div>
                    <div class="col-md-4"></div>

                </div>
                <div class="row" align="center">
                    <%--inicia--%>
                    <asp:GridView ID="gvRepTickets" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" DataSourceID="SqlDataSourceRepTickets">
                        <Columns>
                            <asp:TemplateField HeaderText="FECHA DE SOLICITUD" SortExpression="FechaSolicitud">
                                <ItemTemplate>
                                    <asp:Label ID="fechaSol" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("fechaSol").ToString()) ? DateTime.Parse(Eval("fechaSol").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("fechaSol").ToString()).ToString("hh:mm:ss") : "" %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="nuemberDocs" HeaderText="N° DE tikets" SortExpression="nuemberDocs" />
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
                                    <asp:Label ID="fechaFin" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("fechaFin").ToString()) ? DateTime.Parse(Eval("fechaFin").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("fechaFin").ToString()).ToString("hh:mm:ss") : "" %>' Visible='<%# Eval("DirDescarga").ToString() != null %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FECHA REPORTE">
                                <ItemTemplate>
                                    <asp:Label ID="FechaReporte" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("rango").ToString()) ? DateTime.Parse(Eval("rango").ToString()).ToString("MM/yyyy")  : "" %>' Visible='<%# Eval("DirDescarga").ToString() != null %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descargar" SortExpression="DirDescarga">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lbDesc" runat="server" CssClass="btn btn-primary" NavigateUrl='<%# string.Format("~/download.aspx.?file={0}",Eval("DirDescarga")) %>' EnableEventValidation="false" Visible='<%# Eval("status").ToString() == "1" %>'>Descargar</asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="table" />
                        <EmptyDataTemplate>
                            <strong>No existen datos.</strong>
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
                    <asp:SqlDataSource ID="SqlDataSourceRepTickets" runat="server" SelectCommand="SELECT * FROM Dat_Rep_Tickets ORDER BY 1 DESC"></asp:SqlDataSource>
                    <div class="col-md-12">
                        <asp:LinkButton ID="bActRepTik" CssClass="btn btn-primary" runat="server" OnClick="bActRepTik_Click" Text="Limpiar">Actualizar</asp:LinkButton>
                    </div>
                    <%--termina--%>
                </div>
                <div class="row" align="center">

                    <div class="col-md-4"></div>
                    <div class="col-md-4">

                        <asp:Label ID="Label2" runat="server" ForeColor="Red" Style="text-align: center">
                        </asp:Label>

                    </div>
                    <div class="col-md-4"></div>
                </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress9" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel9">
                <ProgressTemplate>
                    <div id="Background"></div>
                    <div id="Progress">
                        <h6>
                            <p style="text-align: center">
                                <b>Generando Reporte, Espere un momento... </b>
                            </p>
                        </h6>
                        <div class="progress progress-striped active">
                            <div class="progress-bar" style="width: 100%"></div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    INFORMES
</asp:Content>
