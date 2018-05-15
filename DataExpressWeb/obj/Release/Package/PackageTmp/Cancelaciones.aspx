<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cancelaciones.aspx.cs" Inherits="DataExpressWeb.Cancelaciones" EnableEventValidation="false" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function btnDPDF_Click(command) {
            $.ajax({
                type: "POST",
                url: "./Cancelaciones.aspx/btnDPDF_Click",
                data: '{param: "' + command + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    eval(response.d);
                },
                failure: function (response) {
                    alertBootBox('No se pudo generar el PDF<br/>Razón:' + response.d, 4);
                }
            });
        }
        function btnDPDF2_Click(command) {
            $.ajax({
                type: "POST",
                url: "./Cancelaciones.aspx/btnDPDF2_Click",
                data: '{param: "' + command + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    eval(response.d);
                },
                failure: function (response) {
                    alertBootBox('No se pudo generar el PDF<br/>Razón:' + response.d, 4);
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CONSULTAR DOCUMENTOS CANCELADOS O EN PROCESO DE CANCELACIÓN
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server" Visible="false">
    <asp:HiddenField ID="hfIdAcciones" runat="server" Value="" />
    <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div align="center">
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" OnClientClick="return LimpiarFiltros('bodyFiltros', 'FiltrosC2');" Text="Filtros">Filtros</asp:LinkButton>
                <asp:LinkButton ID="bActualizar" runat="server" CssClass="btn btn-primary" OnClick="Button1_Click1" Text="Limpiar Filtros">Limpiar Filtros</asp:LinkButton>
            </div>
            <div class="rowsSpaced">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <asp:Label ID="lCount" runat="server" Text="Registros" Visible="False"></asp:Label>
                    </div>
                    <div class="col-md-2" align="right"><strong>Registros por página:</strong></div>
                    <div class="col-md-1">
                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" CssClass="form-control" Style="text-align: center;">
                            <asp:ListItem Value="10" Text="10" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="25" Text="25"></asp:ListItem>
                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <br />
            <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvFacturas_PageIndexChanged" OnSelectedIndexChanged="gvFacturas_SelectedIndexChanged" OnPageIndexChanging="gvFacturas_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvFacturas_RowDataBound" Font-Size="Smaller" OnPreRender="gvFacturas_PreRender" GridLines="None" OnSorting="gvFacturas_SelectedIndexChanged" fixed-height>
                <Columns>
                    <asp:TemplateField HeaderText="ESTADO">
                        <ItemTemplate>
                            <asp:Image runat="server" ImageUrl='<%# string.Format("~/Imagenes/CAN{0}.png", Eval("EstadoCancelacion")) %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("EstadoCancelacion").ToString() == "1" ? "Pendiente" : (Eval("EstadoCancelacion").ToString() == "2" ? "Encolado" : Eval("EstadoCancelacion").ToString() == "3" ? "Exitoso" : Eval("EstadoCancelacion").ToString() == "4" ? "Datos Erroneos" : Eval("EstadoCancelacion").ToString() == "5" ? "Error en Solicitud" : "Desconocido") %>'></asp:Image>
                        </ItemTemplate>
                        <ControlStyle Height="18px" Width="18px" />
                        <HeaderStyle Width="1%" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RFC" SortExpression="RFCREC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("RFCREC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("RFCREC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EMPLEADO" SortExpression="empleado">
                        <ItemTemplate>
                            <asp:Label ID="Label113" runat="server" Text='<%# Bind("empleado") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CANCELÓ" SortExpression="empleadoCancelacion">
                        <ItemTemplate>
                            <asp:Label ID="Label114" runat="server" Text='<%# Bind("empleadoCancelacion") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TIPO" SortExpression="TIPODOC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox13" runat="server" Text='<%# Bind("TIPODOC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label13" runat="server" Text='<%# Bind("TIPODOC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FOLFAC" HeaderText="FOLIO" InsertVisible="False" ReadOnly="True" SortExpression="FOLFAC" HeaderStyle-Width="10%">
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="serie" HeaderText="SERIE" InsertVisible="False" ReadOnly="True" SortExpression="serie" HeaderStyle-Width="10%">
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField SortExpression="codigoControl">
                        <HeaderTemplate>
                            <asp:Label ID="codigoControl" SortExpression="codigoControl" runat="server" Text="RESERVA/TICKET" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("codigoControl").ToString() == "" ? "N/A" : Eval("codigoControl").ToString() %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TOTAL" SortExpression="TOTAL">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("FOLFAC") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FECHA EMISIÓN" SortExpression="FECHA">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("FECHA").ToString()) ? DateTime.Parse(Eval("FECHA").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FECHA").ToString()).ToString("HH:mm:ss") : "" %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FECHA SOLICITUD" SortExpression="FECHASOL">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("FechaSolicitudCancelacion").ToString()) ? DateTime.Parse(Eval("FechaSolicitudCancelacion").ToString()).ToString("s") : "" %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FECHA CANCELACIÓN" SortExpression="FECHACANC">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("FechaCancelacion").ToString()) ? DateTime.Parse(Eval("FechaCancelacion").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FechaCancelacion").ToString()).ToString("HH:mm:ss") : (Eval("EstadoCancelacion").ToString() == "1" ? "Pendiente" : (Eval("EstadoCancelacion").ToString() == "2" ? "Encolado" : Eval("EstadoCancelacion").ToString() == "3" ? "Exitoso" : Eval("EstadoCancelacion").ToString() == "4" ? "Datos Erroneos" : Eval("EstadoCancelacion").ToString() == "5" ? "Error en Solicitud" : "Desconocido")) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="XML" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlXML" runat="server" NavigateUrl='<%# string.Format("~/download.aspx?file={0}", Eval("XMLARC")) %>'>
                                <asp:Image ID="imgXML" runat="server" AlternateText="XML" ImageUrl="~/imagenes/xml.png" Style="height: 30px" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PDF" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="btnDPDF" runat="server" Height="19" BorderStyle="None" BorderWidth="0" NavigateUrl='<%# string.Format("javascript:btnDPDF_Click(\"{0}\");", Eval("PDFARC").ToString().Replace(@"\", "/") + "|" + Eval("FOLFAC") + "|" + Eval("EDOFAC") + Eval("TIPO") + "|" + Eval("ID") + "|" + Eval("UUID")) %>'>
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/imagenes/pdf.png" Style="height: 30px" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="XML CAN" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# string.Format("~/download.aspx?file={0}", Eval("xmlCancelacion")) %>' Visible='<%# Eval("EstadoCancelacion").ToString() == "3" && System.IO.File.Exists(Server.MapPath("") + @"\" + Eval("xmlCancelacion").ToString()) %>'>
                                <asp:Image ID="Image4" runat="server" AlternateText="XML" ImageUrl="~/imagenes/xml.png" Style="height: 30px" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PDF CAN" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="btnDPDF2" runat="server" Height="19" BorderStyle="None" BorderWidth="0" NavigateUrl='<%# string.Format("javascript:btnDPDF2_Click(\"{0}\");", Eval("ID") + "|" + Eval("UUID")) %>' Visible='<%# Eval("EstadoCancelacion").ToString() == "3" && System.IO.File.Exists(Server.MapPath("") + @"\" + Eval("xmlCancelacion").ToString()) %>'>
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/imagenes/pdf.png" Style="height: 30px" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle CssClass="table" />
                <EmptyDataTemplate>
                    No existen datos.
                </EmptyDataTemplate>
                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="bBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="PA_facturas_basico" SelectCommandType="StoredProcedure" OnSelected="SqlDataSource1_Selected">
        <SelectParameters>
            <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
            <asp:Parameter DefaultValue="-" Name="SUCURSAL" Type="String" />
            <asp:Parameter DefaultValue="-" Name="RFC" Type="String" />
            <asp:Parameter DefaultValue="-" Name="ROL" Type="Boolean" />
            <asp:Parameter DefaultValue="-" Name="empleado" Type="String" />
            <asp:Parameter DefaultValue="-" Name="ROLSUCURSAL" Type="Boolean" />
            <asp:Parameter DefaultValue="100" Name="TOP" Type="String" />
            <asp:Parameter DefaultValue="-" Name="CNSptoemi" Type="Boolean" />
            <asp:Parameter DefaultValue="-" Name="PTOEMII" Type="String" />
            <asp:Parameter DefaultValue="-" Name="CNSComp" Type="String" />
            <asp:Parameter DefaultValue="Dat_General.tipo = 'C'" Name="QUERYSTRING" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" SelectCommand="PA_facturas_basico" SelectCommandType="StoredProcedure" OnSelected="SqlDataSource1_Selected">
        <SelectParameters>
            <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
            <asp:Parameter DefaultValue="-" Name="SUCURSAL" Type="String" />
            <asp:Parameter DefaultValue="-" Name="RFC" Type="String" />
            <asp:Parameter DefaultValue="-" Name="ROL" Type="Boolean" />
            <asp:Parameter DefaultValue="-" Name="empleado" Type="String" />
            <asp:Parameter DefaultValue="-" Name="ROLSUCURSAL" Type="Boolean" />
            <asp:Parameter DefaultValue="100" Name="TOP" Type="String" />
            <asp:Parameter DefaultValue="Dat_General.tipo = 'C'" Name="QUERYSTRING" Type="String" />
        </SelectParameters>

    </asp:SqlDataSource>
    <div class="modal fade " id="FiltrosC2" style="align-content: center;">
        <div class="modal-dialog modal-lg" style="align-content: center;">
            <div class="modal-content " style="align-content: center;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header " style="align-content: center;">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Filtros</h4>
                        </div>
                        <div class="modal-body " style="align-content: center;" id="bodyFiltros">

                            <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%; display: inline;" id="rowFiltroRecep" runat="server">

                                <div style="display: inline; float: left; margin-left: 2px; width: 22%;">
                                    <h5>RFC Receptor:</h5>
                                    <asp:TextBox ID="tbRFC" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" OnTextChanged="tbRFC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRFC" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 77%;">
                                    <h5>Razón Social Receptor:</h5>
                                    <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" Style="text-align: center" placeholder="Nombre *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
                                <div style="display: inline; float: left; margin-left: 2px; width: 35%;">
                                    <h5>Comprobante:</h5>
                                    <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="SqlDataSourceTipoDoc" Style="text-align: center" DataTextField="Descripcion" DataValueField="codigo" AppendDataBoundItems="True" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoDocumento_SelectedIndexChanged1">
                                        <asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" SelectCommand="SELECT codigo, Descripcion, tipo FROM Cat_Catalogo1_C WHERE (tipo = 'Comprobante')"></asp:SqlDataSource>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 20%;">
                                    <h5>
                                        <asp:Label ID="lPtoEmi" runat="server" Text="Serie:"></asp:Label>
                                    </h5>
                                    <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataSourcePtoEmi" Style="text-align: center" CssClass="form-control" DataTextField="serie" DataValueField="idSerie" AppendDataBoundItems="true">
                                        <asp:ListItem Selected="True" Value="0" Text="Todas"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSourcePtoEmi" runat="server" SelectCommand="SELECT s.* FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipoDoc = @tipo AND m.idEmpleado = @idUser">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="0" Name="tipo" />
                                            <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlDataSourcePtoEmiCliente" runat="server" SelectCommand="SELECT * FROM Cat_Series WHERE tipoDoc = @tipo">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="0" Name="tipo" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 15%;">
                                    <h5>Folio:<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationGroup="Folio" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator></h5>
                                    <asp:TextBox ID="tbFolioAnterior" runat="server" CssClass="form-control" placeholder="1,2,3-10 *" Style="text-align: center"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbFolioAnterior_Filter" runat="server" FilterMode="ValidChars" TargetControlID="tbFolioAnterior" ValidChars="0123456789,-" />
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 29%;">
                                    <h5>Estado:</h5>
                                    <asp:DropDownList ID="ddlEstado" runat="server" AppendDataBoundItems="True" Style="text-align: center" CssClass="form-control" placeholder="D:\ *">
                                        <asp:ListItem Value="">Selecciona el Estado</asp:ListItem>
                                        <%--<asp:ListItem Value="E0">Cancelado</asp:ListItem>--%>
                                        <asp:ListItem Value="0">Exitosas</asp:ListItem>
                                        <asp:ListItem Value="1,2">Pendientes</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
                                <div style="display: inline; float: left; margin-left: 2px; width: 20.5%;">
                                    <h5>Desde:</h5>
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFechaInicial" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar-o"></i>
                                        </span>
                                    </div>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 20.5%;">
                                    <h5>Hasta:</h5>
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFechaFinal" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar-o"></i>
                                        </span>
                                    </div>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 58%;">
                                    <h5>Reservación/Ticket:</h5>
                                    <asp:TextBox ID="tbControl" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                            <asp:LinkButton ID="bBuscar" CssClass="btn btn-primary" runat="server" OnClick="bBuscar_Click" Text="Buscar" ValidationGroup="Folio">Buscar</asp:LinkButton>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <div class="modal fade" id="ValidarArc" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="z-index: 1500">
        <div class="modal-dialog modal-lg">

            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div id="Validacion" class="modal-body">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">
                        Cerrar
                    </button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
