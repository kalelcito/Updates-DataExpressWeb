<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentosPendientes.aspx.cs" Inherits="DataExpressWeb.DocumentosPendientes" EnableEventValidation="false" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function SelectAll(checkbox) {
            var bool = !checkbox.checked;
            $('#<%= gvFacturas.ClientID %> tr').slice(1).each(function (index) {
                var chk = $(this).find("input[type='checkbox']");
                var edo = $(this).find("img:first").attr('src');
                $(chk).prop("checked", bool).change();
            });
        }
        function AlertEdoFac(edofac) {
            switch (edofac) {
                case "4E":
                case "1E":
                    return true;
                    break;
                case "0N":
                    alertBootBox("El comprobante seleccionado no fue autorizado o está cancelado", 4);
                    break;
            }
            return false;
        }
        function realizarComprobante() {
            $('#<%= progressCrear.ClientID %>').css('display', 'inline');
        }
        function facturar() {
            if (confirm('¿Seguro/a que desea facturar el comprobante con los conceptos originales de OnQ?')) {
                realizarComprobante();
                return true;
            } else {
                return false;
            }
        }
        var counterBlink = 0;
        function setBG(gridId) {
            var id = "#" + gridId;
            $(id).find("tr").each(function () {
                var css = $(this).attr("class");
                if (css != null && css == "bgRow")
                    $(this).addClass("norRow").removeClass("bgRow");
                else if (css != null && css == "norRow") {
                    $(this).addClass("bgRow").removeClass("norRow");
                    counterBlink++;
                }
            });
            if (counterBlink < 3) {
                setTimeout("setBG('" + gridId + "');", 1000);
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

<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    DOCUMENTOS PENDIENTES POR FACTURAR
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div class="rowsSpaced">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="rowsSpaced">
                    <div class="row form-inline">
                        <fieldset>
                            <div class="col-md-12">
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" OnClientClick="return LimpiarFiltros('bodyFiltros', 'FiltrosC2');" Text="Filtros"></asp:LinkButton>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="bActualizar" runat="server" CssClass="btn btn-primary" OnClick="Button1_Click1" Text="Limpiar Filtros">Limpiar Filtros</asp:LinkButton>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary" OnClick="bBuscar_Click" Text="Actualizar"></asp:LinkButton>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="lbFacturarTodos" runat="server" CssClass="btn btn-primary" OnClick="lbFacturarTodos_Click" Text="Facturar Seleccionados"></asp:LinkButton>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="modal fade " id="FiltrosC2" style="align-content: center;">
            <div class="modal-dialog modal-lg" style="align-content: center;">
                <div class="modal-content " style="align-content: center;">
                    <div class="modal-header " style="align-content: center;">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Filtros</h4>
                    </div>
                    <div class="modal-body " style="align-content: center;" id="bodyFiltros">

                        <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%; display: inline;" id="rowFiltroRecep" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <div style="display: inline; float: left; margin-left: 2px; width: 22%;">
                                        <h5>RFC Receptor:</h5>
                                        <asp:TextBox ID="tbRFC" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" OnTextChanged="tbRFC_TextChanged" AutoPostBack="true" ValidationGroup="Folio"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRFC" UseContextKey="True" MinimumPrefixLength="1">
                                        </asp:AutoCompleteExtender>
                                    </div>
                                    <div style="display: inline; float: left; margin-left: 2px; width: 77%;">
                                        <h5>Razón Social Receptor:</h5>
                                        <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" Style="text-align: center" placeholder="Nombre *"></asp:TextBox>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <div style="display: inline; float: left; margin-left: 2px; width: 35%;">
                                        <h5>Comprobante:</h5>
                                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="SqlDataSourceTipoDoc" Style="text-align: center" DataTextField="Descripcion" DataValueField="codigo" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoDocumento_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" SelectCommand="SELECT '0' AS codigo, 'Selecciona el Tipo' AS Descripcion UNION SELECT codigo, Descripcion FROM Cat_Catalogo1_C WHERE (tipo = 'Comprobante' AND CHARINDEX(codigo,(SELECT CNSComp FROM Cat_Roles WHERE idRol = @rolUser)) > 0) UNION SELECT '10', 'RECEPCION' FROM Cat_Roles WHERE idRol = @rolUser AND recepcion = 1">
                                            <SelectParameters>
                                                <asp:SessionParameter DefaultValue="" Name="rolUser" SessionField="rolUser" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div style="display: inline; float: left; margin-left: 2px; width: 15%;">
                                <h5>Folio:<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationGroup="Folio" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator></h5>
                                <asp:TextBox ID="tbFolioAnterior" runat="server" CssClass="form-control" ValidationGroup="Folio" placeholder="1,2,3-10 *" Style="text-align: center"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="tbFolioAnterior_Filter" runat="server" FilterMode="ValidChars" TargetControlID="tbFolioAnterior" ValidChars="0123456789,-" />
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

                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <asp:Label ID="lCount" runat="server" Text="Registros" Visible="False"></asp:Label>
                    </div>
                    <div class="col-md-2" align="right">Registros por página:</div>
                    <div class="col-md-1">
                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" CssClass="form-control" Style="text-align: center;">
                            <asp:ListItem Value="10" Text="10" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="25" Text="25"></asp:ListItem>
                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                            <asp:ListItem Value="100" Text="100"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <br />
                <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" OnPageIndexChanged="gvFacturas_PageIndexChanged" OnSelectedIndexChanged="gvFacturas_SelectedIndexChanged" OnPageIndexChanging="gvFacturas_PageIndexChanging" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvFacturas_RowDataBound" Font-Size="Smaller" OnPreRender="gvFacturas_PreRender" GridLines="None" OnSorting="gvFacturas_Sorting" fixed-height>
                    <Columns>
                        <asp:TemplateField HeaderText="RFC EMI" SortExpression="RFCEMI">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("RFCEMI") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RFC REC" SortExpression="RFCREC">
                            <ItemTemplate>
                                <asp:Label ID="Label2Rec" runat="server" Text='<%# Bind("RFCREC") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RAZÓN SOCIAL" SortExpression="NOMREC">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("NOMREC") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TIPO" SortExpression="TIPODOC">
                            <ItemTemplate>
                                <asp:Label ID="Label13" runat="server" Text='<%# Bind("TIPODOC") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="serie" HeaderText="SERIE" InsertVisible="False" ReadOnly="True" SortExpression="serie"></asp:BoundField>
                        <asp:BoundField DataField="FOLFAC" HeaderText="FOLIO" InsertVisible="False" ReadOnly="True" SortExpression="FOLFAC"></asp:BoundField>
                        <asp:TemplateField SortExpression="codigoControl" HeaderText="RESERVA/TICKET" ShowHeader="false">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("codigoControl").ToString() == "" ? "N/A" : Eval("codigoControl").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TOTAL" SortExpression="TOTAL">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# string.Format("{0} {1}", Eval("TOTAL"), Eval("TIPOMON")) %>'></asp:Label>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("FOLFAC") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FECHA" SortExpression="FECHA">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("RFCEMI").ToString().Equals("ACG1208232X5") ? Eval("FECHA").ToString() : (!string.IsNullOrEmpty(Eval("FECHA").ToString()) ? DateTime.Parse(Eval("FECHA").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FECHA").ToString()).ToString("HH:mm:ss") : "") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FACTURAR" ShowHeader="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" runat="server" CssClass="btn btn-primary btn-xs" CommandArgument='<%# Eval("ID") %>' OnClick="lbEditar_Click">EDITAR</asp:LinkButton>
                                <asp:LinkButton ID="bFacturar2" runat="server" CssClass="btn btn-primary btn-xs" CommandArgument='<%# Eval("ID") %>' OnClientClick="return facturar();" OnClick="bFacturar2_Click">FACTURAR</asp:LinkButton>
                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label4" runat="server" Text="SELECCIONAR"></asp:Label>
                                <br />
                                <center>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="SelectAll(this);" data-toggle="tooltip" data-placement="top" title="Seleccionar todos los comprobantes Pendientes" />
                        </center>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="check" runat="server" data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Seleccionar comprobante {0}") %>' />
                                <asp:HiddenField ID="checkHdID" runat="server" Value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                      
                         <asp:TemplateField HeaderText="ELIMINAR" SortExpression="ELIMINAR">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-primary btn-xs" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('¿Desea eliminar el registro de prefactura?');" OnClick="lbDelete_Click">ELIMINAR</asp:LinkButton>
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
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="bBuscar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="modal fade " id="divFacturar" style="align-content: center;">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="modal-dialog modal-lg" style="align-content: center; width: 90% !important">
                        <div class="modal-content " style="align-content: center;">
                            <div class="modal-header " style="align-content: center;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 class="modal-title ">Facturar Comprobante Pendiente</h4>
                            </div>
                            <div class="modal-body " style="align-content: center;" id="bodyFacturar">
                                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="heading2">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse2" aria-expanded="true" aria-controls="collapse2">DATOS DEL COMPROBANTE</a>
                                            </h4>
                                        </div>
                                        <div id="collapse2" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading2">
                                            <div class="panel-body rowsSpaced">
                                                <div class="row">
                                                    <div class="col-md-8">
                                                        <div class="row">
                                                            <div class="col-md-6">CONCEPTOS</div>
                                                            <div class="col-md-6" style="text-align: right !important;">
                                                                <asp:RadioButtonList ID="rblTipoEdicion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblTipoEdicion_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="1" style="margin-right: 30px">&nbsp;Editar Conceptos Individuales</asp:ListItem>
                                                                    <asp:ListItem Value="2">&nbsp;Crear Paquete</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:GridView ID="gvConceptos" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvConceptos_PageIndexChanging" PageSize="5" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None" DataSourceID="SqlDataConcTemp" DataKeyNames="idDetalles" OnRowEditing="gvConceptos_RowEditing" OnRowUpdating="gvConceptos_RowUpdating" OnRowCancelingEdit="gvConceptos_RowCancelingEdit" OnRowDataBound="gvConceptos_RowDataBound">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="cantidad" ReadOnly="true" HeaderText="CANTIDAD" />
                                                                        <asp:TemplateField HeaderText="DESCRIPCIÓN">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcion") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="tbDescripcion" runat="server" CssClass="form-control" Style="text-align: center; width: 100%;" Text='<%# Bind("descripcion") %>'></asp:TextBox>
                                                                                    <div class="input-group-btn">
                                                                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" id="btnDescConc" runat="server"><span class="caret"></span></button>
                                                                                        <ul class="dropdown-menu dropdown-menu-right" role="menu" runat="server" id="ulDescConc" style="height: 200px; overflow: hidden; overflow-y: scroll;">
                                                                                        </ul>
                                                                                    </div>
                                                                                </div>
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="valorUnitario" ReadOnly="true" HeaderText="VAL. UNITARIO" />
                                                                        <asp:BoundField DataField="importe" ReadOnly="true" HeaderText="IMPORTE" />
                                                                        <asp:BoundField DataField="unidad" ReadOnly="true" HeaderText="UNIDAD" />
                                                                        <asp:CommandField ShowEditButton="true" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar" ControlStyle-CssClass="btn btn-primary btn-xs" />
                                                                        <asp:TemplateField HeaderText="IMPUESTOS">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lbVerImpuestosDeConcepto" runat="server" CommandArgument='<%# Eval("idDetalles") %>' CssClass="btn btn-primary btn-xs" OnClick="lbVerImpuestosDeConcepto_Click" CausesValidation="false" ValidateRequestMode="Disabled">Ver</asp:LinkButton>
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
                                                                <asp:SqlDataSource ID="SqlDataConcTemp" runat="server" SelectCommand="SELECT idDetalles, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad FROM Dat_Detalles WHERE (id_Comprobante = @idComprobante)" UpdateCommand="UPDATE Dat_Detalles SET descripcion = @descripcion WHERE idDetalles = @idDetalles">
                                                                    <SelectParameters>
                                                                        <asp:Parameter Name="idComprobante" DefaultValue="" />
                                                                    </SelectParameters>
                                                                    <UpdateParameters>
                                                                        <asp:Parameter Name="idDetalles" DefaultValue="" />
                                                                        <asp:Parameter Name="descripcion" DefaultValue="" />
                                                                    </UpdateParameters>
                                                                </asp:SqlDataSource>
                                                            </div>
                                                        </div>
                                                        <div class="row" id="divPaquete" runat="server" visible="true">
                                                            <div class="col-md-2">
                                                                CONCEPTO:
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNuevoConcepto" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-10">
                                                                <asp:TextBox ID="tbNuevoConcepto" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAQUETE *"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="col-md-4">SUBTOTAL:</div>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="tbSubtotal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-4">IVA 16%:</div>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="tbIva16" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div runat="server" id="trISProp" visible="true">
                                                            <div class="col-md-4">
                                                                ISH
                           
                                        <asp:Label ID="lblISHPrer" runat="server" Text="X"></asp:Label>%:
                                   
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="tbISH" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">TOTAL FAC.:</div>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="tbTotalFac" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div id="rowPropina" runat="server" visible="false">
                                                            <div class="col-md-4">PROPINA:</div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="tbPropina" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div runat="server" id="divDescuentoTot" visible="true">
                                                            <div class="col-md-4">DESCUENTO:</div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="tbDescuento" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0 *" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div runat="server" id="trOtrosCargos" visible="false">
                                                            <div class="col-md-4">OTROS C.:</div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="tbOtrosCargos" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">A PAGAR:</div>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="tbTotal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">DOCUMENTO:</div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbCodDoc" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">SERIE:</div>
                                                    <div class="col-md-3">
                                                        <asp:DropDownList ID="ddlSerie" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged" CssClass="form-control" Style="text-align: center" DataSourceID="SqlDataSourceSeries" DataValueField="idSerie" DataTextField="serie"></asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSourceSeries" runat="server" SelectCommand="SELECT DISTINCT s.idSerie, s.serie FROM Cat_Series s INNER JOIN Cat_ModuloEmpleado m ON m.idSerie = s.idSerie INNER JOIN Cat_Empleados e ON e.idEmpleado = m.idEmpleado WHERE e.idEmpleado = @idUser">
                                                            <SelectParameters>
                                                                <asp:SessionParameter SessionField="idUser" Name="idUser" DefaultValue="1" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    <div class="col-md-1">AMBIENTE:</div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbAmbiente" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        <% if (Session["CfdiVersion"].ToString().Equals("3.3"))
                                                            { %>
                                            M PAGO:
                                            <% }
                                                else
                                                { %>
                                            F PAGO:
                                            <% } %>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbFormaPago" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="form-control" Style="text-align: center" AutoPostBack="true" OnSelectedIndexChanged="ddlFormaPago_SelectedIndexChanged">
                                                            <asp:ListItem Value="" Text="SELECCIONE"></asp:ListItem>
                                                            <asp:ListItem Value="PUE" Text="PAGO EN UNA SOLA EXHIBICIÓN"></asp:ListItem>
                                                            <asp:ListItem Value="PPD" Text="PAGO EN PARCIALIDADES O DIFERIDO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <% if (Session["CfdiVersion"].ToString().Equals("3.3"))
                                                            { %>
                                            F PAGO:
                                            <% }
                                                else
                                                { %>
                                            M PAGO:
                                            <% } %>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbMetodoPago" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlMetodoPago" runat="server" DataSourceID="SqlDataMetodoPago" DataTextField="descripcion" DataValueField="codigo" CssClass="form-control" Style="text-align: center">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataMetodoPago" runat="server" SelectCommand="SELECT codigo, (codigo + ': ' + descripcion) as descripcion FROM Cat_Catalogo1_C WHERE tipo = 'MetodoPagoCfdi33';"></asp:SqlDataSource>
                                                    </div>
                                                    <div class="col-md-1">CTA.:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbNoCta" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2" runat="server" id="divChkCredito" style="display: none;">
                                                        <asp:CheckBox ID="chkCredito" runat="server" Text="CRÉDITO" Checked="false" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2">COND. DE PAGO:</div>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="tbCondPago" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2">CANTIDAD CON LETRA:</div>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="tbCantLetra" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2">EXPEDIDO EN:</div>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="tbLugarExp" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2">OBSERVACIONES:</div>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="tbObservaciones" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="heading1">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse1" aria-expanded="true" aria-controls="collapse1">DATOS DEL RECEPTOR</a>
                                            </h4>
                                        </div>
                                        <div id="collapse1" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading1">
                                            <div class="panel-body rowsSpaced">
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        RFC:<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="tbRfcRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbRfcRec" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRfcRec_TextChanged"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="tbRfcRec_AutoCompleteExtender" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcRec" UseContextKey="True" MinimumPrefixLength="1">
                                                        </asp:AutoCompleteExtender>
                                                    </div>
                                                    <div class="col-md-1">
                                                        RAZÓN SOCIAL:<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tbRazonSocialRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="tbRazonSocialRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row" id="rowDenomSocial" runat="server" visible="true">
                                                    <div class="col-md-3"></div>
                                                    <div class="col-md-1">
                                                        DENOM. SOCIAL:
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="tbDenomSocialRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="DENOMINACIÓN SOCIAL *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2"></div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1"></div>
                                                    <div align="left" class="col-md-11">
                                                        <asp:CheckBox ID="cbDomRec" runat="server" Text="HABILITAR" AutoPostBack="true" OnCheckedChanged="cbDomRec_CheckedChanged" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">SUCURSAL:</div>
                                                    <div class="col-md-11">
                                                        <asp:DropDownList ID="ddlSucRec" runat="server" Style="text-align: center" CssClass="form-control" Enabled="False" AutoPostBack="true" OnSelectedIndexChanged="ddlSucRec_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        CALLE:<asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tbCalleRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbCalleRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">NO. EXT.:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbNoExtRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">NO. INT.:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbNoIntRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">COLONIA:</div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbColoniaRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        MUNICIPIO:
                                                           
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMunicipioRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        ESTADO:<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbEstadoRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        C.P.:<asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="tbCpRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbCpRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" MaxLength="5"></asp:TextBox>
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpRec" Mask="99999" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        PAÍS:<asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="tbPaisRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion" Enabled="False"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddlPais" runat="server" ControlToValidate="ddlPais" ErrorMessage="*" InitialValue="" ValidationGroup="Facturacion" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <div class="input-group" id="groupTbPais" runat="server">
                                                            <asp:TextBox ID="tbPaisRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *"></asp:TextBox>
                                                            <div class="input-group-btn">
                                                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" id="btnPaisRec" runat="server" disabled="disabled"><span class="caret"></span></button>
                                                                <ul class="dropdown-menu dropdown-menu-right" role="menu" runat="server" id="ulPaisRec" style="height: 200px; overflow: hidden; overflow-y: scroll;">
                                                                </ul>
                                                            </div>
                                                        </div>
                                                        <asp:DropDownList ID="ddlPais" runat="server" CssClass="form-control bootstrap-select" Style="text-align: center"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row" id="DivUsoCfdi" runat="server" style="display: none">
                                                    <div class="col-md-1">
                                                        USO CFDI:
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_UsoCfdi" runat="server" ControlToValidate="ddlUsoCfdi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:DropDownList ID="ddlUsoCfdi" runat="server" CssClass="form-control" Style="text-align: center" DataSourceID="SqlDataSourceUsoCfdi" DataTextField="descripcion" DataValueField="clave" Enabled="false"></asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSourceUsoCfdi" runat="server" SelectCommand="SELECT '0' AS clave,'Seleccione'AS descripcion UNION SELECT clave,descripcion FROM Cat_UsoCfdi WHERE tipoPersona LIKE @tipoPersonaUsoCfdi">
                                                            <SelectParameters>
                                                                <asp:Parameter Name="tipoPersonaUsoCfdi" DefaultValue="%FM%" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    <div class="col-md-1">
                                                        E-MAIL:
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbMail" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Facturacion" InitialValue="0" Enabled="false"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbMail" runat="server" CssClass="form-control" Style="text-align: center" placeholder="usuario@dominio.com,usuario@dominio.com"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default" id="panelHospedaje" runat="server" style="display: inline">
                                        <div class="panel-heading" role="tab" id="heading3">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse3" aria-expanded="true" aria-controls="collapse3">DATOS DEL HUÉSPED</a>
                                            </h4>
                                        </div>
                                        <div id="collapse3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading3">
                                            <div class="panel-body rowsSpaced">
                                                <div class="row">
                                                    <div class="col-md-1"># CONFIRM:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbConfirmacion" runat="server" CssClass="form-control" Style="text-align: center" placeholder="9999999999 *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">LLEGADA:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbLlegadaHuesped" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="10" placeholder="MM/dd/yyyy *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">SALIDA:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbSalidaHuesped" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="10" placeholder="MM/dd/yyyy *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">HABITACIÓN:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbHabitacionHuesped" runat="server" CssClass="form-control" Style="text-align: center" placeholder="9999 *"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">NOMBRE:</div>
                                                    <div class="col-md-11">
                                                        <asp:TextBox ID="tbNombrehuesped" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NOMBRE DEL HUÉSPED *"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default" id="panelIne" runat="server" style="display: inline">
                                        <div class="panel-heading" role="tab" id="heading4">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse4" aria-expanded="true" aria-controls="collapse4">COMPLEMENTO INE</a>
                                            </h4>
                                        </div>
                                        <div id="collapse4" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading4">
                                            <div class="panel-body rowsSpaced">
                                                <div align="center">
                                                    <asp:CheckBox ID="chkHabilitarINE" runat="server" Text="HABILITAR" OnCheckedChanged="chkHabilitarINE_CheckedChanged" Checked="false" AutoPostBack="true" />
                                                </div>
                                                <div id="divIne" runat="server" class="rowsSpaced" align="center" visible="false">
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-1">
                                                            Tipo de Proceso:<asp:RequiredFieldValidator ID="DDTipo_Proceso_RequiredFieldValidator" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="DDTipo_Proceso" InitialValue="" ValidationGroup="validationIne"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:DropDownList ID="DDTipo_Proceso" runat="server" Style="text-align: center" CssClass="form-control" OnSelectedIndexChanged="DDTipo_Proceso_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="Seleccione" Value="" />
                                                                <asp:ListItem Value="Ordinario">Ordinario</asp:ListItem>
                                                                <asp:ListItem Value="Precampaña">Precampaña</asp:ListItem>
                                                                <asp:ListItem Value="Campaña">Campaña</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-1" id="divComite" runat="server" visible="false">
                                                            <asp:Label ID="lbTipo_Comite" runat="server" Text="Tipo de Comité: " />
                                                        </div>
                                                        <div class="col-md-2" id="divComite2" runat="server" visible="false">
                                                            <asp:DropDownList ID="DDTipoComite" runat="server" Style="text-align: center" CssClass="form-control" OnSelectedIndexChanged="DDTipoComite_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="Seleccione" Value="" />
                                                                <asp:ListItem Value="Ejecutivo Nacional">Ejecutivo Nacional</asp:ListItem>
                                                                <asp:ListItem Value="Ejecutivo Estatal">Ejecutivo Estatal</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-1" id="divIdContaG" runat="server" visible="false">
                                                            <asp:Label ID="lbIdContabilidad" runat="server" Text="Identificador de contabilidad: " />
                                                        </div>
                                                        <div class="col-md-2" id="divIdContaG2" runat="server" visible="false">
                                                            <asp:TextBox ID="TextboxIdentificador" runat="server" MaxLength="6" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                            <asp:MaskedEditExtender runat="server" ID="TextboxIdentificador_MaskedEditExtender" ClearTextOnInvalid="false" Mask="999999" MaskType="Number" MessageValidatorTip="true" ClearMaskOnLostFocus="true" TargetControlID="TextboxIdentificador" />
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-1" id="divEntidad" runat="server" visible="false">
                                                            <asp:Label ID="lbEstado" runat="server" Text="Entidad: " />
                                                        </div>
                                                        <div class="col-md-2" id="divEntidad2" runat="server" visible="false">
                                                            <asp:DropDownList ID="DDEstado" runat="server" Style="text-align: center" CssClass="form-control">
                                                                <asp:ListItem Text="Seleccione" Value="" />
                                                                <asp:ListItem Value="AGU">AGUASCALIENTES</asp:ListItem>
                                                                <asp:ListItem Value="BCN">BAJA CALIFORNIA NORTE</asp:ListItem>
                                                                <asp:ListItem Value="BCS">BAJA CALIFORNIA SUR</asp:ListItem>
                                                                <asp:ListItem Value="CAM">CAMPECHE</asp:ListItem>
                                                                <asp:ListItem Value="CHP">CHIAPAS</asp:ListItem>
                                                                <asp:ListItem Value="CHH">CHIHUAHUA</asp:ListItem>
                                                                <asp:ListItem Value="COA">COAHUILA</asp:ListItem>
                                                                <asp:ListItem Value="COL">COLIMA</asp:ListItem>
                                                                <asp:ListItem Value="DIF">DISTRITO FEDERAL/CIUDAD DE MÉXICO</asp:ListItem>
                                                                <asp:ListItem Value="DUR">DURANGO</asp:ListItem>
                                                                <asp:ListItem Value="GUA">GUANAJUATO</asp:ListItem>
                                                                <asp:ListItem Value="GRO">GUERRERO</asp:ListItem>
                                                                <asp:ListItem Value="HID">HIDALGO</asp:ListItem>
                                                                <asp:ListItem Value="JAL">JALISCO</asp:ListItem>
                                                                <asp:ListItem Value="MEX">ESTADO DE MÉXICO</asp:ListItem>
                                                                <asp:ListItem Value="MIC">MICHOACÁN</asp:ListItem>
                                                                <asp:ListItem Value="MOR">MORELOS</asp:ListItem>
                                                                <asp:ListItem Value="NAY">NAYARIT</asp:ListItem>
                                                                <asp:ListItem Value="NLE">NUEVO LEÓN</asp:ListItem>
                                                                <asp:ListItem Value="OAX">OAXACA</asp:ListItem>
                                                                <asp:ListItem Value="PUE">PUEBLA</asp:ListItem>
                                                                <asp:ListItem Value="QTO">QUERETÁRO</asp:ListItem>
                                                                <asp:ListItem Value="ROO">QUINTANA ROO</asp:ListItem>
                                                                <asp:ListItem Value="SLP">SAN LUIS POTOSÍ</asp:ListItem>
                                                                <asp:ListItem Value="SIN">SINALOA</asp:ListItem>
                                                                <asp:ListItem Value="SON">SONORA</asp:ListItem>
                                                                <asp:ListItem Value="TAB">TABASCO</asp:ListItem>
                                                                <asp:ListItem Value="TAM">TAMAHULIPAS</asp:ListItem>
                                                                <asp:ListItem Value="TLA">TLAXCALA</asp:ListItem>
                                                                <asp:ListItem Value="VER">VERACRUZ</asp:ListItem>
                                                                <asp:ListItem Value="YUC">YUCATÁN</asp:ListItem>
                                                                <asp:ListItem Value="ZAC">ZACATECAS</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-1" id="divAmbito" runat="server" visible="false">
                                                            <asp:Label ID="lbAmbito" runat="server" Text="Ambito: " />
                                                        </div>
                                                        <div class="col-md-2" id="divAmbito2" runat="server" visible="false">
                                                            <asp:DropDownList ID="DDAmbito" runat="server" Style="text-align: center" CssClass="form-control">
                                                                <asp:ListItem Text="Seleccione" Value="" />
                                                                <asp:ListItem Value="Local">Local</asp:ListItem>
                                                                <asp:ListItem Value="Federal">Federal</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-1" id="divIdConta" runat="server" visible="false">
                                                            <asp:Label ID="lbIdConta" runat="server" Text="Identificador de contabilidad: " />
                                                        </div>
                                                        <div class="col-md-2" id="divIdConta2" runat="server" visible="false">
                                                            <asp:TextBox ID="tbIdConta" runat="server" MaxLength="6" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                            <asp:MaskedEditExtender runat="server" ID="MaskedEditExtender4" ClearTextOnInvalid="false" Mask="999999" MaskType="Number" MessageValidatorTip="true" ClearMaskOnLostFocus="true" TargetControlID="TextboxIdentificador" />
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:LinkButton ID="LnkBAgregarIdentificador" runat="server" CssClass="btn btn-primary btn-sm" Text="Agregar" OnClick="LnkBAgregarIdentificador_Click" ValidationGroup="validationIne" Visible="false"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <asp:GridView ID="gvRegistros" runat="server" OnRowDataBound="gvRegistros_DataBound"
                                                            OnRowCommand="btnEliminar_Click" class=" table table-condensed table-responsive table-hover" BackColor="White" Font-Size="Small" GridLines="None" Visible="false">
                                                            <Columns>
                                                                <asp:TemplateField ShowHeader="true" HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="lnkbtnEliminar" Text="Eliminar" CommandName="SelectRow" CommandArgument='<%#Eval("No")%>' CssClass="btn btn-primary">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ShowHeader="true" HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="lnkbtnAIdConta" Text="+ IdContabilidad" CommandName="AddId" CommandArgument='<%#Eval("No")%>' CssClass="btn btn-primary btn-sm"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                                <asp:Button ID="bFacturar" CssClass="btn btn-primary" runat="server" UseSubmitBehavior="false" OnClientClick="realizarComprobante();" OnClick="bFacturar_Click" Text="Facturar"></asp:Button>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="modal fade " id="divImpuestosDeConcepto" style="align-content: center;">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content " style="align-content: center;">
                            <div class="modal-header " style="align-content: center;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 class="modal-title ">Ver impuestos del concepto</h4>
                            </div>
                            <div class="modal-body " style="align-content: center;">
                                <asp:GridView ID="gvImpuestosConceptos" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" BackColor="White" Font-Size="Smaller" GridLines="None" DataKeyNames="IdImpuesto,IdConcepto" fixed-height OnRowEditing="gvImpuestosConceptos_RowEditing" OnRowUpdating="gvImpuestosConceptos_RowUpdating" OnRowCancelingEdit="gvImpuestosConceptos_RowCancelingEdit" OnRowDataBound="gvImpuestosConceptos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Base">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBase" runat="server" Text='<%# Bind("Base") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbBase" runat="server" CssClass="form-control input-decimal" Style="text-align: center" ToolTip="Base del impuesto (generalmente es el importe del concepto)" Text='<%# Bind("Base") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Impuesto">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImpuesto" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlImpuesto" runat="server" CssClass="form-control" SelectedValue='<%# Eval("Impuesto") %>'>
                                                    <asp:ListItem Text="IVA" Value="002"></asp:ListItem>
                                                    <asp:ListItem Text="IEPS" Value="003"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TipoFactor">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTipoFactor" runat="server" Text='<%# Bind("TipoFactor") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlTipoFactor" runat="server" CssClass="form-control" SelectedValue='<%# Eval("TipoFactor") %>'>
                                                    <asp:ListItem Text="Tasa" Value="Tasa"></asp:ListItem>
                                                    <asp:ListItem Text="Cuota" Value="Cuota"></asp:ListItem>
                                                    <asp:ListItem Text="Exento" Value="Exento"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TasaOCuota">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTasaOCuota" runat="server" Text='<%# Bind("TasaOCuota") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbTasaOCuota" runat="server" CssClass="form-control input-decimal" Style="text-align: center" ToolTip="Tasa o Cuota del impuesto" Text='<%# Bind("TasaOCuota") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImporte" runat="server" Text='<%# Bind("Importe") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbImporte" runat="server" CssClass="form-control input-decimal" Style="text-align: center" ToolTip="Importe del impuesto (Debe ser menor o igual al importe de IVA 16 de la derecha)" Text='<%# Bind("Importe") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:CommandField ShowEditButton="true" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar" ControlStyle-CssClass="btn btn-primary btn-xs" />--%>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="table empty" />
                                    <EmptyDataTemplate>
                                        No existen datos.
                                    </EmptyDataTemplate>
                                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
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
                <asp:Parameter DefaultValue="Dat_General.tipo <> 'C' AND Dat_General.estado = '2'" Name="QUERYSTRING" Type="String" />
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
            </SelectParameters>
        </asp:SqlDataSource>
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
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
