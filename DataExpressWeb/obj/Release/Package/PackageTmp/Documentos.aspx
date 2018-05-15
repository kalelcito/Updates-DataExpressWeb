<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="DataExpressWeb.Documentos" EnableEventValidation="false" Async="true" AsyncTimeout="6000" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function SelectAll(checkbox) {
            var bool = !checkbox.checked;
            $('#<%= gvFacturas.ClientID %> tr').slice(1).each(function (index) {
                var chk = $(this).find("input[type='checkbox']");
                var edo = $(this).find("img:first").attr('src');
                if ((edo != undefined) && (edo == "Imagenes/1.png" || edo == "Imagenes/4.png") || !bool) {
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
        function verImpresion(edofac, idImpresion) {
            if (AlertEdoFac(edofac)) {
                $('#<%= hfIdImpresion.ClientID %>').val(idImpresion);
                $('#ModuloImpr').modal('toggle');
            }
            return false;
        }
        function setNombreImpresora() {
            var impresora = $('#<%= ddlImpresoras.ClientID %> option:selected').text();
            $('#<%= hfNombreImpresora.ClientID %>').val(impresora);
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
    function showRegistroBuzonSat() {
        loadPdfModal("<%= ResolveClientUrl("~/manual/RegistroBuzonSat.pdf") %>");
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

        .dropdown-content {
            position: absolute;
            top: 100%;
            left: 0;
            z-index: 1000;
            display: none;
            float: left;
            min-width: 160px;
            padding: 0px 0;
            margin: 0px 0 0;
            font-size: 14px;
            text-align: left;
            list-style: none;
            background-color: #fff;
            -webkit-background-clip: padding-box;
            background-clip: padding-box;
            border: 1px solid #ccc;
            border: 1px solid rgba(0,0,0,.15);
            border-radius: 4px;
            -webkit-box-shadow: 0 6px 12px rgba(0,0,0,.175);
            box-shadow: 0 6px 12px rgba(0,0,0,.175);
        }

        .dropdown-content a {
            color: black;
            padding: 3px 20px;
            text-decoration: none;
            display: block;
            white-space: nowrap;
        }

        .dropdown-content a:hover {
            background-color: #337ab7;
        }

        .dropdown:hover .dropdown-content {
            display: block;
        }

        .dropdown:hover .btn btn-primary {
            background-color: #2e6da4;
        }

        .dropdown button{
            margin: 0px 0px 2px 0px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CONSULTAR DOCUMENTOS
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server" Visible="false">
    
    <asp:HiddenField ID="hfIdImpresion" runat="server" Value="" />
    <asp:HiddenField ID="hfIdAcciones" runat="server" Value="" />
    <asp:HiddenField ID="hfNombreImpresora" runat="server" Value="" />
    <div class="rowsSpaced">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-2">
                        <asp:CheckBox ID="checkPDF" runat="server" Checked="True" Text="PDF" />
                    </div>
                    <div class="col-md-2">
                        <asp:CheckBox ID="checkXML" runat="server" Checked="True" Text="XML" />
                    </div>
                    <div class="col-md-2">
                        <asp:CheckBox ID="chkReglas" runat="server" Text="Reglas" />
                    </div>
                    <div class="col-md-3"></div>
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
                        <%-- <div class="form-group">
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary" OnClick="bBuscar_Click" Text="Actualizar"></asp:LinkButton>
                        </div> --%>
                        <div class="form-group">
                            <div class="dropdown">
                                <button class="btn btn-primary" onclick="return false;">Actualizar</button>
                                <div class="dropdown-content">
                                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="Button1_Click1">Limpiar Filtros</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton4" runat="server" OnClick="bBuscar_Click">Ultimo(s) parametro(s) de busqueda</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <%-- <div class="form-group">
                            <asp:LinkButton ID="bActualizar" runat="server" CssClass="btn btn-primary" OnClick="Button1_Click1" Text="Limpiar Filtros">Limpiar Filtros</asp:LinkButton>
                        </div> --%>
                        <div class="form-group">
                            <asp:LinkButton ID="btnZip" runat="server" CssClass="btn btn-primary" OnClick="btnZip_Click" Text="Descargar ZIP">Descargar ZIP</asp:LinkButton>
                        </div>
                        <div class="form-group">
                            <asp:LinkButton ID="bMail" runat="server" CssClass="btn btn-primary" OnClick="Button1_Click2" Text="Enviar E-Mail" ValidationGroup="email">Enviar E-Mail</asp:LinkButton>
                        </div>
                        <div class="form-group">
                            <div class="dropdown">
                                <button class="btn btn-primary" onclick="return false;">Generar</button>
                                <div class="dropdown-content">
                                    <asp:LinkButton ID="hlCrearNotaC" runat="server" OnClick="hlCrearNotaC_Click">Nota de Crédito (Anulación)</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <%-- <div class="form-group">
                            <div class="dropdown">
                                <button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenu1" runat="server" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                    Generar
                            <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                                    <li runat="server" id="liCrearNotaC">
                                        <asp:LinkButton ID="hlCrearNotaC" runat="server" OnClick="hlCrearNotaC_Click">Nota de Crédito (Anulación)</asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                        </div> --%>
                        <div class="form-group">
                            <asp:LinkButton ID="bEventos" runat="server" CssClass="btn btn-primary" Text="Visor de Eventos" OnClick="bEventos_Click"></asp:LinkButton>
                        </div>
                         <div class="form-group">
                            <asp:LinkButton ID="bExcel" runat="server" CssClass="btn btn-primary" Text="Subir Excel" OnClick="bExcel_Click" Visible="false"></asp:LinkButton>
                        </div>
                    </fieldset>
                </div>
                <div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="El formato de E-Mail no es válido" ForeColor="Red" ControlToValidate="tbEmail" ValidationExpression="^[_a-z-A-Z0-9-]+(\.[_a-z-A-Z0-9-]+)*@[a-z-A-Z0-9-]+((\.[a-z-A-Z0-9-]+)|(\.[a-z-A-Z0-9-]+)(\.[a-z-A-Z]{2,3}))([,][_a-z-A-Z0-9-]+(\.[_a-z0-9-]+)*@[a-z-A-Z0-9-]+((\.[a-z-A-Z0-9-]+)|(\.[a-z-A-Z0-9-]+)(\.[a-z-A-Z]{2,3})))*$" ValidationGroup="email"></asp:RegularExpressionValidator>
                    <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbMsgZip" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

         <div class="modal fade " id="FiltroExcel" style="align-content: center;">
            <div class="modal-dialog modal-lg" style="align-content: center;">
                <div class="modal-content " style="align-content: center;">
                    <div class="modal-header " style="align-content: center;">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Subir Excel a Timbrar</h4>
                    </div>
                    <div class="modal-body " style="align-content: center;" id="bodyFiltros1">
                        
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <center>
                                    <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
                                    <div style="display: inline; float: left; margin-left: 25%; width: 50%;">

                                         <input type="file" id="File1" name="File1" runat="server" cssclass="form-control"  style="text-align: center"  autopostback="true" /> 

                                    </div>
                                    </div>
                                    </center>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <%--<div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">                                
                            <div style="display: inline; float: left; margin-left: 2px; width: 20.5%;"> 
                                <div class="input-group date">
                                     <input type="file" id="File1" name="File1" runat="server" />                                
                                     <input type="submit" id="Submit1" value="Upload" runat="server" />      
                                </div>
                            </div>
                        </div>  --%>                      
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        <asp:LinkButton ID="LinkButton3" CssClass="btn btn-primary" runat="server" OnClick="bSubirExcel" Text="Buscar" ValidationGroup="Folio">Subir Documento</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

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
                                    <div style="display: none; float: left; margin-left: 2px; width: 22%;" id="divRFCemi" runat="server">
                                        <h5>RFC Emisor:</h5>
                                        <asp:TextBox ID="tbRFCemi" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *"></asp:TextBox>
                                    </div>
                                    <div style="display: inline; float: left; margin-left: 2px; width: 22%;">
                                        <h5>RFC Receptor:</h5>
                                        <asp:TextBox ID="tbRFC" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *"></asp:TextBox>
                                        <!--K -->
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="False" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRFC" UseContextKey="True" MinimumPrefixLength="1">
                                        </asp:AutoCompleteExtender>
                                    </div>
                                    <div style="display: inline; float: left; margin-left: 2px; width: 55%;" id="divtbNombre" runat="server">
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
                                <h5>Folio:
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator>
                                </h5>
                                <asp:TextBox ID="tbFolioAnterior" runat="server" CssClass="form-control" placeholder="1,2,3-10 *" Style="text-align: center"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="tbFolioAnterior_Filter" runat="server" FilterMode="ValidChars" TargetControlID="tbFolioAnterior" ValidChars="0123456789,-" />
                            </div>
                            <div style="display: inline; float: left; margin-left: 2px; width: 29%;">
                                <h5>Estado:</h5>
                                <asp:DropDownList ID="ddlEstado" runat="server" AppendDataBoundItems="True" Style="text-align: center" CssClass="form-control" placeholder="D:\ *">
                                    <asp:ListItem Value="0">Selecciona el Estado</asp:ListItem>
                                    <%--<asp:ListItem Value="E0">Cancelado</asp:ListItem>--%>
                                    <asp:ListItem Value="E1">Autorizado</asp:ListItem>
                                    <%--<asp:ListItem Value="E2">Pendiente</asp:ListItem>--%>
                                    <asp:ListItem Value="E4">Cancelado por Nota de Crédito</asp:ListItem>
                                    <asp:ListItem Value="N0">No Autorizado</asp:ListItem>
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
                                <h5>
                                    <asp:Label ID="lblTicketFiltro" runat="server" Text="Reservación/Ticket:"></asp:Label></h5>
                                <asp:TextBox ID="tbControl" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        <asp:LinkButton ID="bBuscar" CssClass="btn btn-primary" runat="server" OnClick="bBuscar_Click" Text="Buscar">Buscar</asp:LinkButton>
                    </div>

                </div>
            </div>
        </div>

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

        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-2">
                        <asp:CheckBox ID="chkHistorico" runat="server" Checked="False" Text="Ver Historico" Visible="false" OnCheckedChanged="chkHistorial_CheckedChanged" AutoPostBack="true" />
                    </div>
                    <div class="col-md-1"></div>
                    <div class="col-md-6">
                        <asp:Label ID="lCount" runat="server" Text="Registros" Visible="False"></asp:Label>
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
                <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" OnPageIndexChanged="gvFacturas_PageIndexChanged" OnSelectedIndexChanged="gvFacturas_SelectedIndexChanged" OnPageIndexChanging="gvFacturas_PageIndexChanging" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvFacturas_RowDataBound" Font-Size="Smaller" OnPreRender="gvFacturas_PreRender" GridLines="None" OnSorting="gvFacturas_Sorting" fixed-height>
                    <Columns>
                        <asp:TemplateField HeaderText="ESTADO" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image runat="server" ImageUrl='<%# string.Format("~/Imagenes/{0}.png", Eval("EDOFAC")) %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("EDOFAC").ToString() == "0" ? "No Autorizado" : (Eval("EDOFAC").ToString() == "1" ? "Autorizado" : Eval("EDOFAC").ToString() == "2" ? "En Proceso" : Eval("EDOFAC").ToString() == "4" ? "Cancelado por nota de Crédito" : "Desconocido") %>'></asp:Image>
                            </ItemTemplate>
                            <ControlStyle Height="18px" Width="18px" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IDENTIFICADOR EMISOR" SortExpression="RFCEMI">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("RFCEMI") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="RAZÓN SOCIAL" SortExpression="NOMEMI" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("NOMEMI") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="IDENTIFICADOR RECEPTOR" SortExpression="RFCREC">
                            <ItemTemplate>
                                <asp:Label ID="Label2Rec" runat="server" Text='<%# Bind("RFCREC") %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("NOMREC", "Razón Social: {0}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RAZÓN SOCIAL" SortExpression="NOMREC">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("NOMREC") %>'></asp:Label>
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
                        <asp:TemplateField SortExpression="codigoControl" HeaderText="RESERVA/TICKET" ShowHeader="false">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("codigoControl").ToString() == "N/A / N/A" || Eval("codigoControl").ToString() == "NA / NA" || Eval("codigoControl").ToString() == "NA / N/A" || Eval("codigoControl").ToString() == "N/A / NA" ? "N/A" : (Eval("codigoControl").ToString().Replace("NA / ", "").Replace(" / NA", "")) %>'></asp:Label>
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
                        <asp:TemplateField HeaderText="ACCIONES" ShowHeader="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button runat="server" Text="VER" OnClientClick='<%# string.Format("javascript:verDocumentos(\"" + "{0}" + "\",\"" + "{1}" + "\"); return false;", Eval("ID"), Eval("FOLFAC")) %>' CssClass="btn btn-primary btn-xs" UseSubmitBehavior="false" data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Muestra acciones para el comprobante {0}") %>' />
                                <div id='<%# Eval("ID", "divDocumentos{0}") %>' class="modal-div">
                                    <table class="table table-condensed table-responsive">
                                        <thead>
                                            <tr>
                                                <th style="text-align: center">XML</th>
                                                <th style="text-align: center">PDF</th>
                                                <%--<th style="text-align: center">Imprimir</th>--%>
                                                <th style="text-align: center" runat="server" visible='<%# Eval("EDOFAC").ToString() == "0" %>'>Mensaje SAT</th>
                                                <% if (Convert.ToBoolean(Session["CancComp"].ToString()))
                                                    { %>
                                                <th runat="server" visible='<%# Eval("EDOFAC").ToString() == "1" || Eval("EDOFAC").ToString() == "4" %>' style="text-align: center">Cancelar</th>
                                                <% } %>
                                                <% if (Convert.ToBoolean(Session["EDcompr0N"].ToString()))
                                                    { %>
                                                <th runat="server" visible='<%# Eval("EDOFAC").ToString() == "0" %>' style="text-align: center">Editar</th>
                                                <% } %>
                                            </tr>
                                        </thead>
                                        <tr>
                                            <td>
                                                <%--<asp:HyperLink ID="hlXml" runat="server" NavigateUrl='<%# Eval("XMLARC", "download.aspx?file={0}&ext=.xml") %>' onclick='<%# string.Format("return AlertEdoFac(\"" + "{0}" + "\");", Eval("EDOFAC") + Eval("TIPO").ToString()) %>' Style="cursor: pointer">
                                                    <asp:Image ID="imgXML" runat="server" ImageUrl="~/imagenes/xml.png" />
                                                </asp:HyperLink>--%>
                                                <asp:HyperLink ID="hlXml" runat="server" NavigateUrl='<%# Eval("XMLARC", "download.aspx?file={0}&ext=.xml") %>' Style="cursor: pointer">
                                                    <asp:Image ID="imgXML" runat="server" ImageUrl="~/imagenes/xml.png" />
                                                </asp:HyperLink>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnDPDF" runat="server" CommandArgument='<%# Eval("PDFARC") + ";" + Eval("FOLFAC") + ";" + Eval("EDOFAC") + Eval("TIPO") + ";" + Eval("ID") + ";" + Eval("UUID") %>' Height="19" BorderStyle="None" BorderWidth="0" OnClientClick='<%# string.Format("return AlertEdoFac(\"" + "{0}" + "\");", Eval("EDOFAC") + Eval("TIPO").ToString()) %>' OnClick="btnDPDF_Click" Visible='<%# !Eval("TIPODOC").ToString().Equals("CONTABILIDAD", StringComparison.OrdinalIgnoreCase) %>'>
                                                    <asp:Image ID="imgPDF" runat="server" ImageUrl="~/imagenes/pdf.png" />
                                                </asp:LinkButton>
                                            </td>
                                            <%--<td>
                                                <asp:LinkButton ID="bImprimirComprobante" Height="19" runat="server" BorderWidth="0px" BorderStyle="None" Visible="true" OnClick="bImprimirComprobante_Click" OnClientClick='<%# string.Format("return verImpresion(\"" + "{0}" + "\",\"" + "{1}" + "\");", Eval("EDOFAC") + Eval("TIPO").ToString(), Eval("ID")) %>'>
                                                    <asp:Image ID="imgImp" runat="server" ImageUrl="~/imagenes/print.png" />
                                                </asp:LinkButton>
                                            </td>--%>
                                            <td runat="server" visible='<%# Eval("EDOFAC").ToString() == "0" %>'>
                                                <asp:LinkButton ID="bSAT" Height="19" runat="server" BorderWidth="0px" BorderStyle="None" OnClientClick='<%# string.Format("javascript:alertBootBoxTitle(\"" + "{0}" + "\",\"Mensaje del SAT del comprobante " + "{1}" + "\"); return false;", string.IsNullOrEmpty(Eval("mensajeSAT").ToString()) ? "Error interno, para más información consulte el Visor de Eventos en la sección de Ajustes" : Eval("mensajeSAT").ToString().Replace("\"", ""), Eval("FOLFAC")) %>'>
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/imagenes/sat.png" />
                                                </asp:LinkButton>
                                            </td>
                                            <% if (Convert.ToBoolean(Session["CancComp"].ToString()))
                                                { %>
                                            <td runat="server" visible='<%# Eval("EDOFAC").ToString() == "1" || Eval("EDOFAC").ToString() == "4" %>'>

                                                <asp:LinkButton ID="bCancelarComprobante" Height="19" runat="server" BorderWidth="0px" BorderStyle="None" OnClick="bCancelarComprobante_Click" OnClientClick="return confirm('¿Seguro de querer cancelar el comprobante?');" CommandArgument='<%# Eval("ID") %>'>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/imagenes/cancelar.png" />
                                                </asp:LinkButton>
                                            </td>
                                            <% } %>
                                            <% if (Convert.ToBoolean(Session["EDcompr0N"].ToString()))
                                                { %>
                                            <td runat="server" visible='<%# Eval("EDOFAC").ToString() == "0" %>'>
                                                <asp:HyperLink ID="HyperLink1" runat="server" Height="19" BorderWidth="0px" BorderStyle="None" NavigateUrl='<%# string.Format("~/nuevo/editarComprobante.aspx?id={0}", Eval("ID"))  %>'>
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/imagenes/editar.png" />
                                                </asp:HyperLink>
                                            </td>
                                            <% } %>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OBSERV." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:HyperLink ID="lbPopupObs" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="OBSERVACIONES / NOTAS" data-content="" data-placement="left" min-width="800px" Visible='<%# (!string.IsNullOrEmpty(Eval("observaciones").ToString()) && !Eval("observaciones").ToString().Equals("<br/>")) || Eval("EDOFAC").ToString() == "0" %>'>
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
                                <asp:HiddenField ID="checkHdID" runat="server" Value='<%# Eval("ID") %>' />
                                <asp:HiddenField ID="checkHdEstado" runat="server" Value='<%# Eval("EDOFAC") %>' />
                                <asp:HiddenField ID="checkSerFol" runat="server" Value='<%# string.Format("{0}{1}", Eval("serie"), Eval("FOLFAC")) %>' />
                                <asp:HiddenField ID="checkTipo" runat="server" Value='<%# Eval("codDoc") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle CssClass="table empty" />
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
                <asp:Parameter DefaultValue="Dat_General.tipo <> 'C' AND Dat_General.estado <> '2'" Name="QUERYSTRING" Type="String" />
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
        <!-- Modal Escenario-->
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
        <div class="modal fade" id="ModuloRemote" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="z-index: 1500">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Visor de Eventos</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <asp:GridView ID="gvEventos" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White"
                                    AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" DataSourceID="SqlDataSource3" OnRowDataBound="gvEventos_RowDataBound" OnSorting="gvEventos_Sorting" OnPageIndexChanging="gvEventos_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="codigo" HeaderText="CODIGO" SortExpression="codigo" />
                                        <asp:BoundField DataField="origen" HeaderText="ORIGEN" SortExpression="origen" />
                                        <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" SortExpression="descripcion" />
                                        <asp:TemplateField HeaderText="FECHA" SortExpression="fecha">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("fecha").ToString()) ? DateTime.Parse(Eval("fecha").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("fecha").ToString()).ToString("hh:mm:ss") : "" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DETALLES" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lbPopupDetalles" runat="server" NavigateUrl="javascript:;" data-toggle="popover" title="DETALLES TÉCNICOS" data-content="" data-placement="left" min-width="800px" Visible='<%# !string.IsNullOrEmpty(Eval("detallesTecnicos").ToString()) %>'>
                                <span class="glyphicon glyphicon-comment" aria-hidden="true"></span></asp:HyperLink>
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
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" SelectCommand="SELECT TOP 50 codigo, origen1 + '.' + origen2 AS origen, descripcion, detallesTecnicos, fecha FROM Logs WHERE codigo IN ('2','3','4','5','7','8','9','10') ORDER BY fecha DESC"></asp:SqlDataSource>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade " id="ModuloNota">
            <div class="modal-dialog">
                <div class="modal-content ">
                    <div class="modal-header ">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Anular Comprobantes</h4>
                    </div>
                    <div class="modal-body ">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-2">AMBIENTE:</div>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="hfambiente" runat="server" />
                                        <asp:TextBox ID="tbAmbiente" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">SERIE:</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSerie" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSeriesNotaC" DataTextField="serie" DataValueField="idSerie" AutoPostBack="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSeriesNotaC" runat="server" SelectCommand="SELECT s.* FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipoDoc = '04' AND s.tipo = 2 AND m.idEmpleado = @idUser">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">MOTIVO:</div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="tbMotivoAnulacion" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MOTIVO DE ANULACIÓN *"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-5"></div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbAnularFactura" runat="server" CssClass="btn btn-primary" OnClientClick="return realizarComprobante('¿Seguro/a que desea anular los comprobantes seleccionados?\nLa anulación no cancela el timbre.');" OnClick="lbAnularFactura_Click">Anular</asp:LinkButton>
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
        <div class="modal fade " id="ModuloImpr" style="z-index: 1500">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header ">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Imprimir Comprobante</h4>
                    </div>
                    <div class="modal-body ">
                        <asp:Label ID="Label14" runat="server" Text="Impresora:"></asp:Label>
                        <asp:DropDownList ID="ddlImpresoras" runat="server" CssClass="form-control">
                        </asp:DropDownList>

                        <br />

                        <asp:LinkButton ID="bImprimirComprobante" runat="server" CssClass="btn btn-primary" OnClientClick="javascript:setNombreImpresora();" OnClick="bImprimirComprobante_Click">Imprimir</asp:LinkButton>
                        <br />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
