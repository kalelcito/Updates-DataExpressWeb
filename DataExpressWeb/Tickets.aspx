<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="DataExpressWeb.Tickets" EnableEventValidation="false" Async="true" AsyncTimeout="6000" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function SelectAll(checkbox) {
            var bool = !checkbox.checked;
            $('#<%= gvTickets.ClientID %> tr').slice(1).each(function (index) {
                var chk = $(this).find("input[type='checkbox']");
                $(chk).prop("checked", bool).change();
            });
        }
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
        function ClientBuscar() {
            var mes = $('#<%= tbMes.ClientID%>').val();
            var serie = $('#<%= ddlSerie.ClientID%> option:selected').val();
            var reserva = $('#<%= tbReserva.ClientID%>').val();
            if (mes == "" && serie == "0" && reserva == "") {
                return confirm('Al no definir ningun filtro, se mostrarán solo los últimos 1000 tickets del mes en curso.\n¿Desea realizar la búsqueda de todas formas?');
            }
            return true;
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
    TICKETS
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div class="rowsSpaced">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="rowsSpaced">
                    <div class="row">
                        <div class="col-md-2">
                            <h5><strong>Mes:</strong></h5>
                            <div class="input-group date-month">
                                <asp:TextBox ID="tbMes" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">
                                    <i class="fa fa-calendar-o"></i>
                                </span>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <h5><strong>Serie:</strong></h5>
                            <asp:DropDownList ID="ddlSerie" runat="server" Style="text-align: center;" CssClass="form-control" DataSourceID="SqlDataSeries" DataTextField="serie" DataValueField="idSerie"></asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <h5><strong>Reservación/Ticket:</strong></h5>
                            <asp:TextBox ID="tbReserva" runat="server" Style="text-align: center;" CssClass="form-control upper" placeholder="REFERENCIA1,REFERENCIA2,..."></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <h5>&nbsp;</h5>
                            <asp:Button ID="bBuscar" runat="server" Text="Buscar/Actualizar" OnClick="bBuscarReg_Click" OnClientClick="return ClientBuscar()" CssClass="btn btn-primary" Style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row form-inline">
                        <fieldset>
                            <div class="col-md-12">
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="bLimpiarFiltros" runat="server" CssClass="btn btn-primary" OnClick="bLimpiarFiltros_Click" Text="Limpiar Filtros"></asp:LinkButton>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="bFacturarTickets" runat="server" CssClass="btn btn-primary" OnClick="bFacturarTickets_Click" Text="Facturar"></asp:LinkButton>
                            </div>
                            <div class="form-group">
                                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#divTicketsManual">Facturar por búsqueda manual</button>
                            </div>
                            <div class="form-group">
                                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#divFactGlobal">Ver proceso de facturas</button>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <strong><asp:Label ID="lCount" runat="server" Text="Registros" Visible="true"></asp:Label></strong>
                    </div>
                </div>
                <br />
                <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" OnPageIndexChanged="gvTickets_PageIndexChanged" OnSelectedIndexChanged="gvTickets_SelectedIndexChanged" OnPageIndexChanging="gvTickets_PageIndexChanging" BackColor="White" AllowPaging="False" AllowSorting="True" DataKeyNames="idTrama" OnRowDataBound="gvTickets_RowDataBound" Font-Size="Smaller" OnPreRender="gvTickets_PreRender" GridLines="None" OnSorting="gvTickets_Sorting" fixed-height>
                    <Columns>
                        <asp:BoundField DataField="serie" HeaderText="SERIE" InsertVisible="False" ReadOnly="True" SortExpression="serie"></asp:BoundField>
                        <asp:TemplateField SortExpression="codigoControl" HeaderText="RESERVA/TICKET" ShowHeader="false">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("codigoControl").ToString() == "" ? "N/A" : Eval("codigoControl").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FECHA" SortExpression="FECHA">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# (!string.IsNullOrEmpty(Eval("FECHA").ToString()) ? DateTime.Parse(Eval("FECHA").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FECHA").ToString()).ToString("HH:mm:ss") : "") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TRAMA" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5">
                            <ItemTemplate>
                                <asp:HyperLink ID="lbPopupDetalles" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="TRAMA" data-content="" data-placement="left" data-trigger="click" min-width="800px" Visible='<%# !string.IsNullOrEmpty(Eval("trama").ToString()) %>'>
                                    <span class="glyphicon glyphicon-file fa-2x" aria-hidden="true"></span></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5">
                            <HeaderTemplate>
                                <asp:Label ID="Label4" runat="server" Text="SELECCIONAR"></asp:Label>
                                <br />
                                <center>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="SelectAll(this);" data-toggle="tooltip" data-placement="top" title="Seleccionar todos los tickets" />
                        </center>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="check" runat="server" data-toggle="tooltip" data-placement="top" title='<%# Eval("codigoControl", "Seleccionar ticket {0}") %>' />
                                <asp:HiddenField ID="checkHdID" runat="server" Value='<%# Eval("idTrama") %>' />
                                <asp:HiddenField ID="checkHdTrama" runat="server" Value='<%# Eval("trama") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle CssClass="table empty" />
                    <EmptyDataTemplate>
                        <strong>No existen datos.</strong>
                    </EmptyDataTemplate>
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="modal fade" id="divFacturar">
            <div class="modal-dialog modal-lg" style="width: 90% !important">
                <div class="modal-content ">
                    <div class="modal-header ">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Generar Comprobante</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
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
                                                        <asp:Label ID="lCountConceptos" runat="server" Text="" Visible="False"></asp:Label>
                                                        <asp:GridView ID="gvConceptos" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvConceptos_PageIndexChanging" PageSize="5" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None">
                                                            <Columns>
                                                                <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD" />
                                                                <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" />
                                                                <asp:BoundField DataField="valorUnitario" HeaderText="VAL. UNITARIO" />
                                                                <asp:BoundField DataField="iva" HeaderText="IVA" />
                                                                <asp:BoundField DataField="importe" HeaderText="IMPORTE" />
                                                                <asp:BoundField DataField="propina" HeaderText="PROPINA" />
                                                                <asp:BoundField DataField="total" HeaderText="TOTAL" />
                                                                <asp:BoundField DataField="unidad" HeaderText="UNIDAD" />
                                                            </Columns>
                                                            <EmptyDataRowStyle CssClass="table empty" />
                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                       
                                                           
                                                            </EmptyDataTemplate>
                                                            <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                                        </asp:GridView>
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
                                                        <div runat="server" id="trISProp" visible="false">
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
                                                        <asp:DropDownList ID="ddlSerieFact" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSeriesFact" DataTextField="serie" DataValueField="idSerie" AutoPostBack="true" OnSelectedIndexChanged="ddlSerieFact_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1">AMBIENTE:</div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbAmbiente" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">F PAGO:</div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbFormaPago" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">M PAGO:</div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlMetodoPago" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataMetodoPago" DataTextField="descripcion" DataValueField="codigo" AutoPostBack="true" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataMetodoPago" runat="server" SelectCommand="SELECT codigo, (codigo + ': ' + descripcion) as descripcion FROM Cat_Catalogo1_C WHERE tipo = 'MetodoPago';"></asp:SqlDataSource>
                                                    </div>
                                                    <div class="col-md-1">CTA.:<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbNumCtaPago" ErrorMessage="El número de cuenta no puede quedar vacío" Style="color: #FF0000" ValidationGroup="validationExtranet" Display="None"></asp:RequiredFieldValidator></div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbNumCtaPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CTA. DE PAGO *"></asp:TextBox>
                                                        <asp:MaskedEditExtender runat="server" ID="tbNumCtaPago_MaskedEditExtender" TargetControlID="tbNumCtaPago" ClearMaskOnLostFocus="false" MaskType="None" Mask="9999" Enabled="false" />
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
                                                        <asp:TextBox ID="tbObservaciones" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="heading3">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse3" aria-expanded="true" aria-controls="collapse3">DATOS DEL RECEPTOR</a>
                                            </h4>
                                        </div>
                                        <div id="collapse3" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading3">
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        RFC:
                                   
                                   

                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="tbRfcRec" ErrorMessage="El RFC no puede quedar vacío" Style="color: #FF0000" ValidationGroup="validationExtranet" Display="None"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-9">
                                                        NOMBRE/RAZÓN SOCIAL:<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tbRazonSocialRec" ErrorMessage="La razón social no puede quedar vacía" Style="color: #FF0000" ValidationGroup="validationExtranet" Display="None"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbRfcRec" runat="server" CssClass="form-control upper" MaxLength="13" Style="text-align: center;" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRfcRec_TextChanged"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <asp:TextBox ID="tbRazonSocialRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="rowsSpaced">
                                                    <div class="row" style="display: none;">
                                                        <div class="col-md-1">SUC.:</div>
                                                        <div class="col-md-11">
                                                            <asp:DropDownList ID="ddlSucRec" runat="server" Style="text-align: center" CssClass="form-control" Enabled="False" AutoPostBack="true" OnSelectedIndexChanged="ddlSucRec_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">
                                                            CALLE:<asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tbCalleRec" ErrorMessage="La calle no puede quedar vacía" Style="color: #FF0000" ValidationGroup="validationExtranet" Enabled="True" Display="None"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbCalleRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *" ReadOnly="False"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">EXT.:</div>
                                                        <div class="col-md-2">
                                                            <asp:TextBox ID="tbNoExtRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *" ReadOnly="False"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">INT.:</div>
                                                        <div class="col-md-2">
                                                            <asp:TextBox ID="tbNoIntRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *" ReadOnly="False"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">COLONIA:</div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbColoniaRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *" ReadOnly="False"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">
                                                            MUNICIPIO:
                                                           
                                                           

                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMunicipioRec" ErrorMessage="El municipio no puede quedar vacío" Style="color: #FF0000" ValidationGroup="validationExtranet" Enabled="True" Display="None"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="False"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">
                                                            ESTADO:

                                   

                                                           



                                                           







                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbEstadoRec" ErrorMessage="El estado no puede quedar vacío" Style="color: #FF0000" ValidationGroup="validationExtranet" Enabled="True" Display="None"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="False"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">
                                                            C.P.:

                                   

                                                           



                                                           







                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="tbCpRec" ErrorMessage="El código postal no puede quedar vacío" Style="color: #FF0000" ValidationGroup="validationExtranet" Enabled="True" Display="None"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbCpRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" ReadOnly="False" MaxLength="5"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpRec" Mask="99999" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">
                                                            PAÍS:<asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="tbPaisRec" ErrorMessage="El país no puede quedar vacío" Display="None" ValidationGroup="validationExtranet" Enabled="True"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="tbPaisRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *" ReadOnly="False" Visible="false"></asp:TextBox>
                                                                <div class="input-group-btn" visible="false">
                                                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" id="btnPaisRec" runat="server" visible="false"><span class="caret" visible="false"></span></button>
                                                                    <ul class="dropdown-menu dropdown-menu-right" role="menu" runat="server" id="ulPaisRec" style="height: 200px; overflow: hidden; overflow-y: scroll;" visible="false">
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                            <asp:DropDownList ID="ddlPais" runat="server" CssClass="form-control bootstrap-select" Style="text-align: center"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-1">
                                                            E-MAIL(S):<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbMailReceptor" ErrorMessage="El E-mail no puede quedar vacío" Display="None" ValidationGroup="validationExtranet" Enabled="True"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="tbMailReceptor" runat="server" CssClass="form-control" Style="text-align: center" placeholder="email@dominio.com,emal@dominio.com *"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1"> Uso CFDI: </div><br /><br /> 
                                                        <div class="col-md-8"> <asp:DropDownList ID="ddlUsoCFDI" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataUsoCfdi" DataTextField="descripcion" DataValueField="clave"> </asp:DropDownList> </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <%-- <asp:LinkButton ID="bFacturar" runat="server" OnClientClick="return realizarComprobante();" OnClick="bFacturar_Click" CssClass="btn btn-primary" ValidationGroup="validationExtranet" Text ="Facturar ">Facturar</asp:LinkButton>--%>
                        <%--
                        <asp:Button runat="server" OnClientClick="return realizarComprobante();" OnClick="bGenerar_Click" CssClass="btn btn-primary" Text="Facturar" ValidationGroup="validationExtranet" />&nbsp;&nbsp;
                        --%>
                        <asp:Button runat="server" OnClientClick="return realizarComprobante();" OnClick="bFacturar_Click" CssClass="btn btn-primary" Text="Facturar" ValidationGroup="validationExtranet" />&nbsp;&nbsp;
                     
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <asp:SqlDataSource ID="SqlDataSeries" runat="server" SelectCommand="(SELECT '0' AS idSerie,'Todas' AS serie) UNION (SELECT DISTINCT s.serie AS idSerie, s.serie FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipo = 4 AND m.idEmpleado = @idUser)">
            <SelectParameters>
                <asp:SessionParameter Name="idUser" SessionField="idUser" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataUsoCfdi" runat="server" SelectCommand="SELECT clave, descripcion FROM Cat_UsoCfdi">
         </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSeriesFact" runat="server" SelectCommand="SELECT s.idSerie, s.serie FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE m.idEmpleado = @idUser">
            <SelectParameters>
                <asp:SessionParameter Name="idUser" SessionField="idUser" />
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
        <div class="modal fade" id="divTicketsManual">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header ">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Facturación por búsqueda manual</h4>
                    </div>
                    <div class="modal-body rowsSpaced">
                        <div class="row">
                            <div class="col-md-12">TICKETS (UNO POR LÍNEA):</div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:TextBox ID="tbTicketsManual" runat="server" CssClass="form-control" TextMode="MultiLine" Style="resize: none; height: 300px;" Font-Size="Small"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="bFacturarManual" runat="server" OnClick="bFacturarManual_Click" CssClass="btn btn-primary">Facturar</asp:LinkButton>
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <%--  MOdal Facura global --%>
        <div class="modal fade" id="divFactGlobal">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header ">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title ">Facturación de Tickets</h4>
                    </div>
                    <div class="modal-body rowsSpaced">
                        <%--  df--%>
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
                                                <asp:Image runat="server" ImageUrl='<%# string.Format("~/Imagenes/{0}.png", Eval("Status")) %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("Status").ToString() == "1" ? "GENERADO" : (Eval("Status").ToString() == "2" ? "En Proceso" : "Desconocido") %>'></asp:Image>
                                            </ItemTemplate>
                                            <ControlStyle Height="18px" Width="18px" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="FECHA FINAL" SortExpression="fechaFin">
                                            <ItemTemplate>
                                                <asp:Label ID="FechaFIn" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("fechaFin").ToString()) ? DateTime.Parse(Eval("fechaFin").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("fechaFin").ToString()).ToString("hh:mm:ss") : "" %>' Visible='<%# Eval("folio").ToString() != null %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="serie" HeaderText="SERIE" SortExpression="serie" />
                                        <asp:BoundField DataField="folio" HeaderText="FOLIO" SortExpression="folio" />
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
                        <asp:SqlDataSource ID="SqlDataSourceFacGlob" runat="server" SelectCommand="select FechaSolicitud,nroTiquets,status,fechaFin,serie,folio from Dat_FacGlobalWeb WHERE status <= 2  order by 1 desc
"></asp:SqlDataSource>
                        <%--  sdfg--%>
                    </div>
                    <div class="modal-footer">
                        <%-- <asp:LinkButton ID="LinkButton1" runat="server" OnClick="bFacturarManual_Click" CssClass="btn btn-primary">Facturar</asp:LinkButton>--%>
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
