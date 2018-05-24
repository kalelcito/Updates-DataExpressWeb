<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="~/recepcion/Documentos.aspx.cs" Inherits="DataExpressWeb.recepcion.Documentos" EnableEventValidation="false" Async="true" AsyncTimeout="6000" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function SelectAll(checkbox) {
            var bool = !checkbox.checked;
            $('#<%= gvFacturas.ClientID %> tr').slice(1).each(function (index) {
                var chk = $(this).find("input[type='checkbox']");
                var edo = $(this).find("input:hidden[id*='chkEstado']").val();
                if ((edo != undefined) && (startsWith(edo, '1') && !endsWith(edo, '0')) || !bool) {
                    $(chk).prop("checked", bool).change();
                }
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
        function MostrarGrupo(idComprobante, idTipo) {
            var ids = idComprobante + "," + idTipo;
            $('#<%= hfIdUpdateTipoProveedor.ClientID %>').val(ids);
            SimulateClick('<%= PanelGrupo_btn.ClientID %>');
        }


        function Mostrar(id) {
            alert('attr');
            SimulateClick('<%= btDeleteDocAdi.ClientID %>');
        }
        function Mostrar1() {
            SimulateClick('<%= btUpdateDocAdi.ClientID %>');
        }


        function verDocumentos(id, folio) {
            $('#<%= hfIdAcciones.ClientID %>').val(id);
            var htmlCode = $('#divDocumentos' + id).html();
            alertBootBoxTitle(htmlCode, 'Acciones del comprobante con folio ' + folio);
        }
        function realizarComprobante(msg) {
            var result = confirm(msg);
            if (result) {
                $('#<%= progressCrear.ClientID %>').css('display', 'inline');
        }
        return result;
    }
    function FileUploadStarted(sender, args) {
        var filename = args.get_fileName();
        var ext = filename.substring(filename.lastIndexOf(".") + 1).toUpperCase();
        if (ext != 'xml' && ext != 'XML') {
            var err = new Error();
            err.message = 'Solo se aceptan archivos con extensiones ".xml"';
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


    function OrdenUpload(sender, args) {
        var filename = args.get_fileName();
        var ext = filename.substring(filename.lastIndexOf(".") + 1).toUpperCase();
        //if (ext != 'pdf' && ext != 'PDF') {
        //    var err = new Error();
        //    err.message = 'Solo se aceptan archivos con extensión ".pdf"';
        //    throw (err);
        //    return false;
        //}
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
    function triggerValidation(control) {
        var id = control.attr('id');
        var rbvalue = $('#' + id + ' input:checked').val()
        if (rbvalue == '0') {
            if (confirm('¿Desea rechazar el documento?')) {
                return true;
            } else {
                $('#' + id).find("input[value='2']").attr("checked", "checked");
                return false;
            }
        } else {
            return true;
        }
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
    </style>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CONSULTAR DOCUMENTOS
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server" Visible="false">
    <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField ID="hfIdImpresion" runat="server" Value="" />
            <asp:HiddenField ID="hfIdAcciones" runat="server" Value="" />
            <asp:HiddenField ID="hfNombreImpresora" runat="server" Value="" />
            <asp:HiddenField ID="hfMotivoAnulacion" runat="server" Value="" />
            <asp:HiddenField ID="hfIdUpdateTipoProveedor" runat="server" Value="" />
            <div class="rowsSpaced">
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-2">
                        <asp:CheckBox ID="checkPDF" runat="server" Checked="True" Text="PDF" />
                    </div>
                    <div class="col-md-2">
                        <asp:CheckBox ID="checkXML" runat="server" Checked="True" Text="XML" />
                    </div>
                    <div class="col-md-2">
                        <asp:CheckBox ID="checkORDEN" runat="server" Checked="True" Text="ORDEN DE COMPRA" />
                    </div>
                    <div class="col-md-2">
                        <asp:CheckBox ID="checkADICIONALES" runat="server" Checked="True" Text="ARCHIVOS ADICIONALES" />
                    </div>
                    <div class="col-md-2">
                        <asp:CheckBox ID="chkReglas" runat="server" Text="Reglas" />
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row">
                    <div style="align-content: center" class="col-xs-offset-3 col-xs-6">
                        <asp:TextBox ID="tbEmail" runat="server" ValidationGroup="email" CssClass="form-control" Style="text-align: center" placeholder="email@dominio.com, prueba@dataexpressintmx.com *"></asp:TextBox>
                    </div>
                </div>
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
                            <asp:LinkButton ID="btnZip" runat="server" CssClass="btn btn-primary" OnClick="btnZip_Click" Text="Descargar ZIP">Descargar ZIP</asp:LinkButton>
                        </div>
                        <div class="form-group">
                            <asp:LinkButton ID="bMail" runat="server" CssClass="btn btn-primary" OnClick="Button1_Click2" Text="Enviar E-Mail" ValidationGroup="email">Enviar E-Mail</asp:LinkButton>
                        </div>
                        <div class="form-group" id="divValidador" runat="server" visible="false">
                            <asp:LinkButton ID="bValidar" runat="server" CssClass="btn btn-primary" OnClick="bValidar_Click" Text="Validar" ValidationGroup="email">Validar</asp:LinkButton>
                            <asp:LinkButton ID="bGuardarValidaciones" runat="server" CssClass="btn btn-primary" OnClick="bGuardarValidaciones_Click" Text="Guardar Validaciones" ValidationGroup="email" Visible="false">Guardar Validaciones</asp:LinkButton>
                            <asp:LinkButton ID="bCancelarValidaciones" runat="server" CssClass="btn btn-primary" OnClick="bCancelarValidaciones_Click" Text="Cancelar Validaciones" ValidationGroup="email" Visible="false">Cancelar Validaciones</asp:LinkButton>
                        </div>
                        <div class="form-group" style="display: none">
                            <div class="dropdown">
                                <button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenu1" runat="server" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" visible='<%# Session["CRnewComp"].ToString().Contains("07") %>'>
                                    Generar<span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                                    <li>
                                        <asp:LinkButton ID="hlCrearRetencion" runat="server" OnClick="hlCrearRetencion_Click" Visible='<%# Session["CRnewComp"].ToString().Contains("07") && Session["IDGIRO"].ToString() != "1" && Session["IDGIRO"].ToString() != "2" %>'>Retención</asp:LinkButton>

                                        <asp:LinkButton ID="btDeleteDocAdi" runat="server" Visible="true" Style="display: none" CssClass="btn btn-primary" OnClick="btDeleteDocAdi_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="btUpdateDocAdi" runat="server" Visible="true" Style="display: none" CssClass="btn btn-primary" OnClick="btUpdateDocAdi_Click"></asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="El formato de Email no es válido" ForeColor="Red" ControlToValidate="tbEmail" ValidationExpression="^[_a-z-A-Z0-9-]+(\.[_a-z-A-Z0-9-]+)*@[a-z-A-Z0-9-]+((\.[a-z-A-Z0-9-]+)|(\.[a-z-A-Z0-9-]+)(\.[a-z-A-Z]{2,3}))([,][_a-z-A-Z0-9-]+(\.[_a-z0-9-]+)*@[a-z-A-Z0-9-]+((\.[a-z-A-Z0-9-]+)|(\.[a-z-A-Z0-9-]+)(\.[a-z-A-Z]{2,3})))*$" ValidationGroup="email"></asp:RegularExpressionValidator>
                    <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbMsgZip" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div class="modal fade " id="FiltrosC2" style="align-content: center;">
        <div class="modal-dialog modal-lg" style="align-content: center;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="modal-content " style="align-content: center;">
                        <div class="modal-header " style="align-content: center;">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Filtros</h4>
                        </div>
                        <div class="modal-body " style="align-content: center;" id="bodyFiltros">
                            <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
                                <div style="display: inline; float: left; margin-left: 2px; width: 22%;">
                                    <h5>RFC Emisor:</h5>
                                    <asp:TextBox ID="tbRFC" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" ValidationGroup="Folio"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRFC" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 77%;">
                                    <h5>Razón Social Emisor:</h5>
                                    <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" Style="text-align: center" placeholder="Nombre *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
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
                                    <asp:SqlDataSource ID="SqlDataSourcePtoEmi" runat="server" SelectCommand="SELECT idSerie, serie FROM Cat_Series WHERE tipoDoc = @tipo">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="0" Name="tipo" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 15%;">
                                    <h5>Folio:</h5>
                                    <asp:TextBox ID="tbFolioAnterior" runat="server" CssClass="form-control" ValidationGroup="Folio" placeholder="1,A2,3 *" Style="text-align: center"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbFolioAnterior_Filter" runat="server" FilterType="UppercaseLetters,Numbers,Custom" TargetControlID="tbFolioAnterior" ValidChars="," />
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 29%;">
                                    <h5>Estado:</h5>
                                    <asp:DropDownList ID="ddlEstado" runat="server" AppendDataBoundItems="True" Style="text-align: center" CssClass="form-control" placeholder="D:\ *">
                                        <asp:ListItem Value="00">Selecciona el Estado</asp:ListItem>
                                        <asp:ListItem Value="0">Rechazado</asp:ListItem>
                                        <asp:ListItem Value="1">Pagado</asp:ListItem>
                                        <asp:ListItem Value="2">En proceso</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
                                <div style="display: inline; float: left; margin-left: 2px; width: 24.5%;">
                                    <h5>Emitido Desde:</h5>
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFechaInicial" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar-o"></i>
                                        </span>
                                    </div>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 24.5%;">
                                    <h5>Emitido Hasta:</h5>
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFechaFinal" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar-o"></i>
                                        </span>
                                    </div>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 0.75%;">
                                    &nbsp;
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 24.5%;">
                                    <h5>Recibido Desde:</h5>
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFechaInicialR" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar-o"></i>
                                        </span>
                                    </div>
                                </div>
                                <div style="display: inline; float: left; margin-left: 2px; width: 24.5%;">
                                    <h5>Recibido Hasta:</h5>
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFechaFinalR" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar-o"></i>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
                                <div style="display: inline; float: left; margin-left: 2px; width: 99%;">
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <asp:Label ID="lCount" runat="server" Text="" Visible="False"></asp:Label>
                </div>
                <div class="col-md-2" align="right"><strong>Registros por página:</strong></div>
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
            <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" OnPageIndexChanged="gvFacturas_PageIndexChanged" OnSelectedIndexChanged="gvFacturas_SelectedIndexChanged" OnPageIndexChanging="gvFacturas_PageIndexChanging" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvFacturas_RowDataBound" Font-Size="Smaller" OnSorting="gvFacturas_Sorting" GridLines="None" fixed-height>
                <Columns>
                    <asp:TemplateField HeaderText="ESTADO">
                        <ItemTemplate>
                            <asp:Image runat="server" ImageUrl='<%# string.Format("~/Imagenes/{0}.png", Eval("EDOFAC")) %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("EDOFAC").ToString() == "0" ? "No Autorizado/Cancelado" : (Eval("EDOFAC").ToString() == "1" ? "Autorizado" : Eval("EDOFAC").ToString() == "2" ? "En Proceso" : Eval("EDOFAC").ToString() == "4" ? "Cancelado por nota de Crédito" : "Desconocido") %>'></asp:Image>
                            <asp:Image runat="server" ImageUrl='<%# string.Format("~/Imagenes/{0}.png", Eval("estadoValidacion")) %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("estadoValidacion").ToString() == "0" ? "Rechazado" : (Eval("estadoValidacion").ToString() == "1" ? "Pagado" : Eval("estadoValidacion").ToString() == "2" ? "En Proceso" : "Desconocido") %>' Visible='<%# Session["_isWorkflow"] != null && Session["_isWorkflow"] is bool && Convert.ToBoolean(Session["_isWorkflow"]) %>'></asp:Image>
                            <asp:HiddenField ID="estadoEstructura" runat="server" Value='<%# Eval("EDOFAC") %>' />
                            <asp:HiddenField ID="estadoValidacion" runat="server" Value='<%# Eval("estadoValidacion") %>' />
                        </ItemTemplate>
                        <ControlStyle Height="18px" Width="18px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RFC" SortExpression="RFCEMI">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("RFCEMI") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RAZÓN SOCIAL" SortExpression="NOMREC">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("NOMREC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EMPLEADO" SortExpression="empleado">
                        <ItemTemplate>
                            <asp:Label ID="Label113" runat="server" Text='<%# Bind("empleado") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TIPO" SortExpression="TIPODOC">
                        <ItemTemplate>
                            <asp:Label ID="Label13" runat="server" Text='<%# Bind("TIPODOC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="serie" HeaderText="SERIE" InsertVisible="False" ReadOnly="True" SortExpression="serie"></asp:BoundField>
                    <asp:BoundField DataField="FOLFAC" HeaderText="FOLIO" InsertVisible="False" ReadOnly="True" SortExpression="FOLFAC"></asp:BoundField>
                    <asp:TemplateField HeaderText="TOTAL" SortExpression="TOTAL">
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("FOLFAC") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FECHA" SortExpression="FECHA">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("FECHA").ToString()) ? DateTime.Parse(Eval("FECHA").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FECHA").ToString()).ToString("HH:mm:ss") : "" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FECHA RECEPCIÓN" SortExpression="fechaRecepcion">
                        <ItemTemplate>
                            <asp:Label ID="lblFechaRecepcion" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("fechaRecepcion").ToString()) ? DateTime.Parse(Eval("fechaRecepcion").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("fechaRecepcion").ToString()).ToString("HH:mm:ss") : "" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ESTRUCTURA VALIDACIÓN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="lbPopupVa2" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="" data-content="" data-placement="left" min-width="800px">
                                    <i class="fa fa-users fa-2x" aria-hidden="true"></i></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="VALIDADO POR" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="lbPopupVal" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="USUARIOS QUE VALIDARON" data-content="" data-placement="left" min-width="800px">
                                    <i class="fa fa-users fa-2x" aria-hidden="true"></i></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="VALIDAR" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:RadioButtonList ID="valid" data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Validar comprobante {0}") %>' runat="server" Style="width: 100%;" RepeatDirection="Horizontal" onclick="return triggerValidation($(this));">
                                <asp:ListItem Value="1">Validar</asp:ListItem>
                                <asp:ListItem Value="0">Rechazar</asp:ListItem>
                                <asp:ListItem Value="2">Pendiente</asp:ListItem>
                                <%--<asp:ListItem Value="1" title="Aprobar y validar comprobante">Validar</asp:ListItem>
                                <asp:ListItem Value="0" title="Rechazar el comprobante">Rechazar</asp:ListItem>
                                <asp:ListItem Value="2" title="No realizar ninguna acción">Pendiente</asp:ListItem>
                                <asp:ListItem Value="-1" title="Regresar comprobante a un nivel de validación anterior">Regresar</asp:ListItem>--%>
                            </asp:RadioButtonList>
                            <asp:TextBox ID="tbValidObs" runat="server" Text="" CssClass="form-control" placeholder="Observaciones" Style="text-align: center; width: 100%;"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ACCIONES" ShowHeader="false" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button runat="server" Text="VER" OnClientClick='<%# string.Format("javascript:verDocumentos(\"" + "{0}" + "\",\"" + "{1}" + "\"); return false;", Eval("ID"), Eval("FOLFAC")) %>' CssClass="btn btn-primary btn-xs" UseSubmitBehavior="false" data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Muestra acciones para el comprobante {0}") %>' />
                            <div id='<%# Eval("ID", "divDocumentos{0}") %>' class="modal-div">
                                <table class="table table-condensed table-responsive">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center">XML</th>
                                            <th style="text-align: center">PDF</th>
                                            <th style="text-align: center" runat="server" visible='<%# !string.IsNullOrEmpty(Eval("ORDENARC").ToString()) %>'>ORDEN</th>
                                            <th style='text-align: center' runat="server" visible='<%# int.Parse(Eval("archivosAdicionales").ToString()) > 0 %>'>ADICIONALES</th>
                                            <th style="text-align: center" runat="server" visible='<%# Eval("EDOFAC").ToString() == "0" %>'>Mensaje SAT</th>
                                            <th style="text-align: center">Cancelar</th>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hlXml" runat="server" NavigateUrl='<%# Eval("XMLARC", "~/download.aspx?file={0}") %>' onclick='<%# string.Format("return AlertEdoFac(\"" + "{0}" + "\");", Eval("EDOFAC") + Eval("TIPO").ToString()) %>' Style="cursor: pointer">
                                                <asp:Image ID="imgXML" runat="server" ImageUrl="~/imagenes/xml.png" />
                                            </asp:HyperLink>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnDPDF" runat="server" CommandArgument='<%# Eval("PDFARC") + ";" + Eval("FOLFAC") + ";" + Eval("EDOFAC") + Eval("TIPO") + ";" + Eval("ID") + ";" + Eval("UUID") %>' Height="19" BorderStyle="None" BorderWidth="0" OnClientClick='<%# string.Format("return AlertEdoFac(\"" + "{0}" + "\");", Eval("EDOFAC") + Eval("TIPO").ToString()) %>' OnClick="btnDPDF_Click" Visible='<%# !Eval("TIPODOC").ToString().Equals("CONTABILIDAD", StringComparison.OrdinalIgnoreCase) %>'>
                                                <asp:Image ID="imgPDF" runat="server" ImageUrl="~/imagenes/pdf.png" />
                                            </asp:LinkButton>
                                        </td>
                                        <td id="tdOrdenCompra" runat="server" visible='<%# !string.IsNullOrEmpty(Eval("ORDENARC").ToString()) %>'>
                                            <asp:HyperLink ID="hlOrden" runat="server" NavigateUrl='<%# Eval("ORDENARC", "~/download.aspx?file={0}") %>' onclick='<%# string.Format("return AlertEdoFac(\"" + "{0}" + "\");", Eval("EDOFAC") + Eval("TIPO").ToString()) %>' Style="cursor: pointer">
                                                <asp:Image ID="Image20" runat="server" ImageUrl="~/imagenes/orden.png" />
                                            </asp:HyperLink>
                                        </td>
                                        <td id="tdAdicionales" runat="server" visible='<%# int.Parse(Eval("archivosAdicionales").ToString()) > 0 %>'>
                                            <asp:LinkButton ID="lbPopupAdicionales" runat="server" OnClick="lbPopupAdicionales_Click" CommandArgument='<%# Eval("ID").ToString() + "," + Eval("estadoValidacion").ToString() %>'>
                                                <asp:Image ID="Image3" runat="server" ImageUrl="~/imagenes/adicionales.png" />
                                            </asp:LinkButton>
                                        </td>
                                        <td runat="server" visible='<%# Eval("EDOFAC").ToString() == "0" %>'>
                                            <asp:LinkButton ID="bSAT" Height="19" runat="server" BorderWidth="0px" BorderStyle="None" OnClientClick='<%# string.Format("javascript:alertBootBoxTitle(\"" + "{0}" + "\",\"Mensaje del SAT del comprobante " + "{1}" + "\"); return false;", Eval("mensajeSAT"), Eval("FOLFAC")) %>'>
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/imagenes/sat.png" />
                                            </asp:LinkButton>
                                        </td>
                                        <td>
                                            <% if (Convert.ToBoolean(Session["CancComp"].ToString()))
                                                { %>
                                            <asp:LinkButton ID="bCancelarComprobante" Height="19" runat="server" BorderWidth="0px" BorderStyle="None" OnClick="bCancelarComprobante_Click" OnClientClick="return confirm('¿Seguro de querar cancelar el comprobante?');" CommandArgument='<%# Eval("ID") %>'>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/imagenes/cancelar.png" />
                                            </asp:LinkButton>
                                            <% } %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td id="tdPDFU" runat="server" visible='<%# Session["USERNAME"].ToString().StartsWith("PROVE") ? (Eval("estadoValidacion").ToString() == "0" || Eval("estadoValidacion").ToString() == "2") : (false) %>'>
                                            <asp:LinkButton ID="LinkButton6" CssClass="btn btn-primary" Text="Actualizar" runat="server" BorderWidth="0px" BorderStyle="None" OnClick="bUpdatePDF_Click" OnClientClick="return confirm('¿Seguro que desea actualizar el PDF?');" CommandArgument='<%# Eval("ID") %>'>
                                            </asp:LinkButton>
                                        </td>
                                        <td id="tdOrdenCompraU" runat="server" visible='<%# !string.IsNullOrEmpty(Eval("ORDENARC").ToString()) && (Session["USERNAME"].ToString().StartsWith("PROVE") ? (Eval("estadoValidacion").ToString() == "0" || Eval("estadoValidacion").ToString() == "2") : (true)) %>'>
                                            <asp:LinkButton ID="LinkButton2" CssClass="btn btn-primary" Text="Actualizar" runat="server" BorderWidth="0px" BorderStyle="None" OnClick="bUpdateOC_Click" OnClientClick="return confirm('¿Seguro que desea actualizar la orden de compra?');" CommandArgument='<%# Eval("ID") %>'>
                                            </asp:LinkButton>
                                        </td>
                                        <td></td>
                                        <td></td>

                                        <td></td>
                                    </tr>

                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DISTRIBUCIÓN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="BtnDistr" runat="server" Text="Editar" OnClick="Distribuidor_Click" CssClass="btn btn-primary btn-xs" UseSubmitBehavior="false" data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Edita la distribución para el comprobante {0}") %>' CommandArgument='<%# Eval("ID").ToString() %>' />
                            <%--<asp:HyperLink ID="lbDistribucion" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="" data-content="" data-placement="left" min-width="800px">
                                    <i class="fa fa-users fa-2x" aria-hidden="true"></i></asp:HyperLink>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PAGOS" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnPagos" CssClass="btn btn-primary btn-xs" UseSubmitBehavior="false" data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Muestra los pagos del comprobante {0}") %>' OnClick="btnPagos_Click" CommandArgument='<%# Eval("ID") %>' Visible='<%# Eval("estadoValidacion").ToString() == "1" || Session["_isWorkflow"] == null || !(Session["_isWorkflow"] is bool) || !(Convert.ToBoolean(Session["_isWorkflow"])) %>'><i class="fa fa-credit-card" aria-hidden="true"></i>&nbsp;VER</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="OBSERV." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="lbPopupObs" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="OBSERVACIONES / NOTAS" data-content="" data-placement="left" min-width="800px" Visible='<%# (!string.IsNullOrEmpty(Eval("observaciones").ToString()) && !Eval("observaciones").ToString().Equals("<br/>")) %>'>
                                <%--<span class="glyphicon glyphicon-comment" aria-hidden="true"></span></asp:HyperLink>--%>
                                    <i class="fa fa-comments fa-2x" aria-hidden="true"></i></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="Label4" runat="server" Text="SELECCIONAR"></asp:Label>
                            <br />
                            <center>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="SelectAll(this);" data-toggle="tooltip" data-placement="top" title="Seleccionar todos los comprobantes Autorizados" />
                        </center>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="check" runat="server" data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Seleccionar comprobante {0}") %>' />
                            <asp:HiddenField ID="checkHdPDF" runat="server" Value='<%# Eval("PDFARC") %>' />
                            <asp:HiddenField ID="checkHdXML" runat="server" Value='<%# Eval("XMLARC") %>' />
                            <asp:HiddenField ID="checkHdORDEN" runat="server" Value='<%# Eval("ORDENARC") %>' />
                            <asp:HiddenField ID="checkHdID" runat="server" Value='<%# Eval("ID") %>' />
                            <asp:HiddenField ID="chkEstado" runat="server" Value='<%# string.Format("{0}{1}", Eval("EDOFAC"), Eval("estadoValidacion")) %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle CssClass="table" />
                <EmptyDataTemplate>No existen datos.</EmptyDataTemplate>
                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
            </asp:GridView>
            <asp:LinkButton runat="server" Style="display: none;" CommandArgument="" ID="PanelGrupo_btn" OnClick="PanelGrupo_Clic"></asp:LinkButton>
        </ContentTemplate>
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
            <asp:Parameter DefaultValue="" Name="QUERYSTRING" Type="String" />
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
                    <b>Generando Comprobante Electronico, Espere un momento... </b>
                </p>
            </h6>
            <div class="progress progress-striped active">
                <div class="progress-bar" style="width: 100%"></div>
            </div>
        </div>
    </div>
    <!-- Modal Escenario-->
    <div class="rowsSpaced">
        <div class=" modal fade  modal-lg" id="FiltrosC" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div>
                <div class="modal-content modal-lg">
                    <div class="modal-header modal-lg">
                    </div>
                    <div id="Filtros" class="modal-body modal-lg">
                        <form role="form">
                            <div class="form-group">
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="ValidarArc" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="z-index: 1500">
            <div class="modal-dialog modal-lg">

                <div class="modal-content">
                    <div class="modal-header">
                    </div>
                    <div id="Validacion" class="modal-body">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="ModuloRemote" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="z-index: 1500">
            <div class="modal-dialog modal-lg">

                <div class="modal-content">
                    <div class="modal-header">
                    </div>
                    <div class="modal-body">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade " id="ModuloRetencion">
            <div class="modal-dialog">
                <div class="modal-content ">
                    <div class="modal-header ">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Crear Retenciones</h4>
                    </div>
                    <div class="modal-body ">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-2">AMBIENTE:</div>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="hfAmbiente1" runat="server" />
                                        <asp:TextBox ID="tbAmbiente1" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">SERIE:</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSerie1" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSeriesRet" DataTextField="serie" DataValueField="idSerie" AutoPostBack="true" OnSelectedIndexChanged="ddlSerie1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSeriesRet" runat="server" SelectCommand="SELECT * FROM Cat_Series WHERE tipoDoc = '07' AND tipo = 2"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-5"></div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbCrearRet" runat="server" CssClass="btn btn-primary" OnClientClick="return realizarComprobante('¿Seguro/a que desea crear retenciones de los comprobantes seleccionados?.');" OnClick="lbCrearRet_Click">Crear Retenciones</asp:LinkButton>
                                    </div>
                                    <div class="col-md-5"></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade " id="ModuloPagos">
            <div class="modal-dialog modal-lg">
                <div class="modal-content ">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="modal-header ">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 class="modal-title ">PAGOS</h4>
                            </div>
                            <div class="modal-body ">
                                <asp:HiddenField ID="hfIdComprobantePago" runat="server" Value="" />
                                <asp:GridView ID="gvPagos" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" DataSourceID="SqlDataSourcePagos" DataKeyNames="idPago" fixed-height>
                                    <Columns>
                                        <asp:BoundField DataField="UUID" HeaderText="UUID Complemento" />
                                        <asp:BoundField DataField="MontoPago" HeaderText="Pagado" />
                                        <asp:BoundField DataField="FormaPago" HeaderText="Forma de Pago" />
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                        <asp:TemplateField HeaderText="XML">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlXml" runat="server" NavigateUrl='<%# Eval("XML", "~/download.aspx?file={0}") %>' Style="cursor: pointer">
                                                    <asp:Image ID="imgXML" runat="server" ImageUrl="~/imagenes/xml.png" Height="20" />
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="table" />
                                    <EmptyDataTemplate>
                                        No existen pagos registrados.
                                    </EmptyDataTemplate>
                                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSourcePagos" runat="server" SelectCommand="SELECT * FROM Dat_ComplementosPago p INNER JOIN Dat_General g ON g.idComprobante = p.id_Comprobante WHERE g.idComprobante = @idComprobante">
                                    <SelectParameters>
                                        <asp:Parameter Name="idComprobante" DefaultValue="0" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <div class="row">
                                    <div class="col-md-4">
                                        A Pagar:<span class="badge">
                                            <asp:Label ID="lblPagos_APagar" runat="server" Text=""></asp:Label></span>
                                    </div>
                                    <div class="col-md-4">
                                        Pendiente de Pago:<span class="badge">
                                            <asp:Label ID="lblPagos_PendientePago" runat="server" Text=""></asp:Label></span>
                                    </div>
                                    <div class="col-md-4">
                                        Pagado:<span class="badge">
                                            <asp:Label ID="lblPagos_Pagado" runat="server" Text=""></asp:Label></span>
                                    </div>
                                </div>
                                <div class="row" id="rowUploadPayments" runat="server" style="display: none;">
                                    <div class="col-md-12">
                                        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <h4 class="panel-title">
                                                        <a data-toggle="collapse" data-parent="#accordion" href="#collapse">SUBIR PAGO</a>
                                                    </h4>
                                                </div>
                                                <div id="collapse" class="panel-collapse collapse" role="tabpanel">
                                                    <div class="panel-body">
                                                        <div class="row">
                                                            <div class="col-md-10">
                                                                <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="FileUploadStarted" runat="server" ID="filePago" OnUploadedComplete="filePago_UploadedComplete" accept="application/xml" />
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:LinkButton ID="lbUploadPago" runat="server" CssClass="btn btn-primary" OnClick="lbUploadPago_Click" data-toggle="tooltip" data-placement="top" title="Agregar Pago">
                                                                    <i class="fa fa-plus" aria-hidden="true"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="lbCleanPago" runat="server" CssClass="btn btn-primary" OnClick="lbCleanPago_Click" data-toggle="tooltip" data-placement="top" title="Limpiar Pagos">
                                                                    <i class="fa fa-refresh" aria-hidden="true"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:GridView ID="gvPagoCfdi" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvPagoCfdi_PageIndexChanging" BackColor="White" AllowPaging="True" Font-Size="XX-Small" GridLines="None">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="UuidPago" HeaderText="Uuid" />
                                                                        <asp:BoundField DataField="UuidRelacionado" HeaderText="Uuid Relacionado" />
                                                                        <asp:BoundField DataField="SerieFolio" HeaderText="Serie/Folio" />
                                                                        <asp:BoundField DataField="Parcialidad" HeaderText="# Parcialidad" />
                                                                        <asp:BoundField DataField="SaldoAnterior" HeaderText="Anterior" />
                                                                        <asp:BoundField DataField="SaldoPagado" HeaderText="Pagado" />
                                                                        <asp:BoundField DataField="SaldoInsoluto" HeaderText="Insoluto" />
                                                                        <asp:BoundField DataField="FechaPago" HeaderText="Fecha" />
                                                                        <asp:BoundField DataField="FormaPago" HeaderText="Forma de Pago" />
                                                                        <asp:BoundField DataField="MontoPago" HeaderText="Monto" />
                                                                        <asp:BoundField DataField="NumOperacion" HeaderText="# Operacion" />
                                                                    </Columns>
                                                                    <EmptyDataRowStyle CssClass="table" />
                                                                    <EmptyDataTemplate>
                                                                        No existen datos.
                                                                    </EmptyDataTemplate>
                                                                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" CssClass="gvHeader" />
                                                                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                                <asp:LinkButton ID="lbGuardarPagosComprobante" CssClass="btn btn-primary" runat="server" OnClick="lbGuardarPagosComprobante_Click" Text="Buscar">Guardar Pagos</asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!--inicia panel editar-->
    <div class="modal fade " id="EditarTipoProveedor">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <asp:UpdatePanel runat="server" ID="updPanel1" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Editar Tipo Proveedor</h4>
                        </div>
                        <div class="modal-body ">
                            <h5>Tipo de Proveedor: </h5>
                            <asp:DropDownList ID="ModTipoProv" runat="server" DataSourceID="SqlDataSourceModTipoProv" DataValueField="IdTipo" DataTextField="nombre" Style="text-align: center" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSourceModTipoProv" runat="server" SelectCommand="SELECT IdTipo, nombre FROM Cat_TiposProveedor WHERE visible = 'true' AND eliminado = 'false'"></asp:SqlDataSource>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="bGuardarTipoProveedor" runat="server" OnClick="bGuardarTipoProveedor_Click" OnClientClick="return confirm('¿Desea actualizar el registro?');" CssClass="btn btn-primary">Guardar</asp:LinkButton>
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!--inicia panel editar-->


    <!--inicia panel editarPDF-->
    <div class="modal fade " id="EditarPDF" style="z-index: 9999">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <asp:UpdatePanel runat="server" ID="UpdatePanel4" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Cargar PDF para actualizar</h4>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HFidArchivo" runat="server" Value="" />
                            <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="pdfUpload" runat="server" ID="AsyncFUpdf" OnUploadedComplete="AsyncFUpdf_UploadedCompletePDF" accept="application/xml" />
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="bSavePDF" runat="server" OnClick="bSavePDF_Click" OnClientClick="return confirm('¿Desea actializar este PDF?');" CssClass="btn btn-primary">Actualizar</asp:LinkButton>
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!--inicia panel editar-->


    <!--inicia panel editarOC-->
    <div class="modal fade " id="EditarOC" style="z-index: 9999">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <asp:UpdatePanel runat="server" ID="UpdatePanel6" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Cargar oden de compra para actualizar</h4>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                            <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="OrdenUpload" runat="server" ID="AsyncFUOC" OnUploadedComplete="AsyncFUpdf_UploadedCompleteOC" accept="application/xml" />
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="bSaveOC" runat="server" OnClick="bSaveOC_Click" OnClientClick="return confirm('¿Desea actualizar la orden de compra?');" CssClass="btn btn-primary">Actualizar</asp:LinkButton>
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!--inicia panel editar-->


    <!--inicia panel editar archivo adi-->
    <div class="modal fade " id="EditarArcAdi" style="z-index: 99999">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <asp:UpdatePanel runat="server" ID="UpdatePanel9" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Cargar archivo adicional para actualizar</h4>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="HiddenField2" runat="server" Value="" />
                            <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="OrdenUpload" runat="server" ID="AsyncFUAA" OnUploadedComplete="AsyncFUpdf_UploadedCompleteAA" accept="application/xml" />
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="bSaveAA" runat="server" OnClick="bSaveAA_Click" OnClientClick="return confirm('¿Desea actualizar este archivo adicional?');" CssClass="btn btn-primary">Actualizar</asp:LinkButton>
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!--inicia panel editar-->


    <div class="modal fade " id="infoDocsAdi" style="z-index: 9999">
        <div class="modal-dialog modal-lg">
            <div class="modal-content ">
                <asp:UpdatePanel ID="UpdatePanel8" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Información</h4>
                        </div>
                        <div class="modal-body rowsSpaced">
                            <asp:GridView ID="gvDocsAdi" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover"
                                PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Small" GridLines="None" DataSourceID="SqlDataSourceDocsAdi"
                                DataKeyNames="idArchivo" EnableModelValidation="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="XML">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlXml" runat="server" NavigateUrl='<%# Eval("path", "~/download.aspx?file={0}") %>' Style="cursor: pointer">
                                                <asp:Image ID="imgXML" runat="server" ImageUrl="~/imagenes/orden.png" Height="20" />
                                                <asp:HiddenField ID="hfidArchivo" runat="server" Value='<%#Bind("idArchivo")%>' />
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Archivo">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelMto" runat="server" Text='<%# Bind("path") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" HeaderText="ACCIONES" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="bUpdate" runat="server" CssClass="btn btn-primary btn-sm" Text="Cambiar" CommandArgument='<%#Bind("idArchivo") %>'
                                                OnClick="btUpdateDocAdi_Click"></asp:LinkButton>
                                            <asp:LinkButton ID="bDelete" runat="server" OnClientClick="return confirm('¿Deseas eliminar este documento adicional?');" CommandArgument='<%#Bind("idArchivo") %>' OnClick="btDeleteDocAdi_Click" Text="Eliminar" CssClass="btn btn-primary btn-sm">
                                                     </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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
                            <asp:SqlDataSource ID="SqlDataSourceDocsAdi" runat="server" SelectCommand="PA_recepcion_ArchivosAdi" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
                                    <asp:Parameter DefaultValue=" " Name="SiO" Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>

                    <%--                    <asp:LinkButton ID="lbAddArcAdi" CssClass="btn btn-primary" runat="server" CommandArgument='<%#Bind("idArchivo")%>' OnClick="lbAddArcAdi_Click" Text="Agregar"></asp:LinkButton>--%>
                </div>
                <br />
            </div>
        </div>
    </div>
    <!--inicia panle distribucion-->
    <div class="modal fade " id="Distribuidor">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <asp:UpdatePanel runat="server" ID="UpdatePanel10" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Editar Distribución</h4>
                        </div>
                        <div class="modal-body">
                            <%--<div style="display: inline; float: left; margin-left: 2px; width: 35%;">--%>
                            <h5>Estructura de Validación / Tipo de Proveedor:</h5>
                            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSourceGeneral" Style="text-align: center" DataTextField="nombre" DataValueField="idTipo" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSourceGeneral" runat="server" SelectCommand="(select '0' AS idTipo, 'Seleccione...' AS nombre) union (select idTipo, nombre from Cat_TiposProveedor where visible = 1)"></asp:SqlDataSource>
                            <%--</div>--%>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="LbsaveProv" runat="server" OnClick="bSaveProveedor_Click" OnClientClick="return confirm('¿Desea guargar cambios?');" CssClass="btn btn-primary" CommandArgument="">Guardar</asp:LinkButton>
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!--finaliza panel-->



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
