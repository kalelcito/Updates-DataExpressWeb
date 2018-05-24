<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentosPorCobrar.aspx.cs" Inherits="DataExpressWeb.DocumentosPorCobrar" EnableEventValidation="false" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function SelectAll(checkbox) {
            var bool = !checkbox.checked;
            $('#<%= gvFacturas.ClientID %> tr').slice(1).each(function (index) {
                var chk = $(this).find("input[type='checkbox']");
                var edo = $(this).find("img:first").attr('src');
                if ((edo != undefined) && (edo == "Imagenes/pago0.png" || edo == "Imagenes/pago2.png") || !bool) {
                    $(chk).prop("checked", bool).change();
                }
            });
        }
        function verDocumentos(id, folio) {
            var htmlCode = $('#divDocumentos' + id).html();
            alertBootBoxTitle(htmlCode, 'Acciones del comprobante ' + folio);
        }
        function CollapsePanels33() {
            $('#collapse').addClass('in');
            $('#collapseInner1').removeClass('in');
        }
        function verPagos(idPagos, pendiente, saldo) {
            $('#<%= tbPagoPendiente.ClientID %>').val(pendiente);
            $('#<%= tbSaldo.ClientID %>').val(saldo);
            $('#<%= tbPagoAplicado.ClientID %>').val('0.00');
            $('#<%= hfPagoPendiente.ClientID %>').val(pendiente);
            $('#<%= hfSaldo.ClientID %>').val(saldo);
            $('#<%= hfPagoAplicado.ClientID %>').val('0.00');
            $('#<%= hfIdPagos.ClientID %>').val(idPagos);
            $('#divPagos32').modal('show');
        }
        function crearPDF(params) {
            $.ajax({
                type: "POST",
                url: "/DocumentosPorCobrar.aspx/btnDPDF_Click",
                data: '{param: "' + params + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    window.location.href = response.d;
                },
                failure: function (response) {
                    alertBootBox('No se pudo generar el PDF<br/>Razón:' + response.d, 4);
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                     console.log(errorThrown);
                  }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CONSULTAR DOCUMENTOS POR COBRAR
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server" Visible="false">
    <asp:HiddenField ID="hfIdImpresion" runat="server" Value="" />
    <asp:HiddenField ID="hfIdPagos" runat="server" Value="" />
    <asp:HiddenField ID="hfNombreImpresora" runat="server" Value="" />
    <asp:HiddenField ID="hfPagoPendiente" runat="server" Value="" />
    <asp:HiddenField ID="hfSaldo" runat="server" Value="" />
    <asp:HiddenField ID="hfPagoAplicado" runat="server" Value="" />

    <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div align="center">
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" OnClientClick="return LimpiarFiltros('bodyFiltros', 'FiltrosC2');" Text="Filtros">Filtros</asp:LinkButton>
                <asp:LinkButton ID="bActualizar" runat="server" CssClass="btn btn-primary" OnClick="Button1_Click1" Text="Limpiar Filtros">Limpiar Filtros</asp:LinkButton>
                <asp:Button ID="bGenerarComplemento" runat="server" Text="Generar Complemento Pagos" CssClass="btn btn-primary" OnClick="bGenerarComplemento_Click" CausesValidation="False" OnClientClick="CollapsePanels33();" />
            </div>
            <div>
                <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
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
                <div class="modal-body" style="align-content: center;" id="bodyFiltros">

                    <div class="row" align="center" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
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
                        <div style="display: inline; float: left; margin-left: 2px; width: 20%;">
                            &nbsp;
                        </div>
                        <div style="display: inline; float: left; margin-left: 2px; width: 15%;">
                            <h5>
                                <asp:Label ID="lPtoEmi" runat="server" Text="Serie:"></asp:Label>
                            </h5>
                            <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataSourcePtoEmi" Style="text-align: center" CssClass="form-control" DataTextField="serie" DataValueField="idSerie" AppendDataBoundItems="true">
                                <asp:ListItem Selected="True" Value="0" Text="Todas"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSourcePtoEmi" runat="server" SelectCommand="SELECT s.* FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipoDoc = '01' AND m.idEmpleado = @idUser">
                                <SelectParameters>
                                    <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div style="display: inline; float: left; margin-left: 2px; width: 15%;">
                            <h5>Folio:<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationGroup="Folio" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator></h5>
                            <asp:TextBox ID="tbFolioAnterior" runat="server" CssClass="form-control" ValidationGroup="Folio" placeholder="1,2,3-10 *" Style="text-align: center"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbFolioAnterior_Filter" runat="server" FilterMode="ValidChars" TargetControlID="tbFolioAnterior" ValidChars="0123456789,-" />
                        </div>
                        <div style="display: inline; float: left; margin-left: 2px; width: 29%;">
                            <h5>Estado:</h5>
                            <asp:DropDownList ID="ddlEstado" runat="server" AppendDataBoundItems="True" Style="text-align: center" CssClass="form-control" placeholder="D:\ *">
                                <asp:ListItem Value="0">Selecciona el Estado</asp:ListItem>
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
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->

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
                            <asp:ListItem Value="100" Text="100"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <br />
            <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvFacturas_PageIndexChanged" OnPageIndexChanging="gvFacturas_PageIndexChanging" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvFacturas_RowDataBound" Font-Size="Smaller" GridLines="None" OnSorting="gvFacturas_SelectedIndexChanged" fixed-height DataKeyNames="ID">
                <Columns>
                    <asp:TemplateField HeaderText="ESTADO">
                        <ItemTemplate>
                            <asp:Image runat="server" ImageUrl='<%# string.Format("~/Imagenes/pago{0}.png", decimal.Parse(Eval("saldoPendiente").ToString()) <= 0 ? "1" : (decimal.Parse(Eval("pagoAplicado").ToString()) < decimal.Parse(Eval("saldo").ToString()) && decimal.Parse(Eval("pagoAplicado").ToString()) > 0 ? "2" : "0")) %>' data-toggle="tooltip" data-placement="top" title='<%# string.Format("{0}", decimal.Parse(Eval("saldoPendiente").ToString()) <= 0 ? "Pagado" : (decimal.Parse(Eval("pagoAplicado").ToString()) < decimal.Parse(Eval("saldo").ToString()) && decimal.Parse(Eval("pagoAplicado").ToString()) > 0 ? "En proceso de Pago" : "Pendiente de Pago")) %>'></asp:Image>
                        </ItemTemplate>
                        <ControlStyle Height="18px" Width="18px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RFC" SortExpression="RFCREC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("RFCREC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblRfcRec" runat="server" Text='<%# Bind("RFCREC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EMPLEADO" SortExpression="empleado">
                        <ItemTemplate>
                            <asp:Label ID="Label113" runat="server" Text='<%# Bind("empleado") %>'></asp:Label>
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
                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("codigoControl").ToString() == "N/A / N/A" || Eval("codigoControl").ToString() == "NA / NA" || Eval("codigoControl").ToString() == "NA / N/A" || Eval("codigoControl").ToString() == "N/A / NA" ? "N/A" : (Eval("codigoControl").ToString().Replace("NA / ", "").Replace(" / NA", "")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TOTAL" SortExpression="TOTAL" ItemStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("FOLFAC") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FECHA" SortExpression="FECHA">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("FECHA").ToString()) ? DateTime.Parse(Eval("FECHA").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FECHA").ToString()).ToString("HH:mm:ss") : "" %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SALDO" SortExpression="saldo">
                        <ItemTemplate>
                            <asp:Label ID="Label111" runat="server" Text='<%# Bind("saldo") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PAGADO" SortExpression="pagoAplicado">
                        <ItemTemplate>
                            <asp:Label ID="Label121" runat="server" Text='<%# Bind("pagoAplicado") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PENDIENTE" SortExpression="saldoPendiente">
                        <ItemTemplate>
                            <asp:Label ID="Label131" runat="server" Text='<%# Bind("saldoPendiente") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ACCIONES" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button runat="server" Text="VER" OnClientClick='<%# string.Format("javascript:verDocumentos(\"" + "{0}" + "\",\"" + "{1}" + "\"); return false;", Eval("ID"), Eval("FOLFAC")) %>' CssClass="btn btn-primary btn-xs" UseSubmitBehavior="false" />
                            <div id='<%# Eval("ID", "divDocumentos{0}") %>' class="modal-div">
                                <table class="table table-condensed table-responsive table-hover">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center">Descargar XML</th>
                                            <th style="text-align: center">Descargar PDF</th>
                                            <th id="thActualizarPago" runat="server" visible='<%# !Session["CfdiVersion"].ToString().Equals("3.3") %>' style="text-align: center">Actualizar Pago</th>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td>
                                            <a href='download.aspx?file=<%# Eval("XMLARC") %>'>
                                                <asp:Image ID="imgXML" runat="server" ImageUrl="~/imagenes/xml.png" />
                                            </a>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnDPDF" runat="server" Height="19" BorderStyle="None" BorderWidth="0" OnClientClick='<%#string.Format("crearPDF(\"" + "{0};{1};{2};{3}" + "\");", Eval("PDFARC"), Eval("FOLFAC"), Eval("EDOFAC") + Eval("TIPO").ToString(), Eval("ID")) %>'>
                                                <asp:Image ID="imgPDF" runat="server" ImageUrl="~/imagenes/pdf.png" />
                                            </asp:LinkButton>
                                        </td>
                                        <td id="tdActualizarPago" runat="server" visible='<%# !Session["CfdiVersion"].ToString().Equals("3.3") %>'>
                                            <asp:LinkButton ID="bActualizarPago" runat="server" CausesValidation="false" CommandArgument='<%# Eval("ID") %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Muestra los pagos del comprobante {0}") %>' OnClientClick='<%# string.Format("verPagos(\"" + "{0}" + "\",\"" + "{1}" + "\",\"" + "{2}" + "\");", Eval("ID"), Eval("saldoPendiente"), Eval("saldo")) %>'>
                                                <asp:Image ID="impPago" runat="server" ImageUrl="~/imagenes/credit_card.png" />
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PAGOS">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbHistorialPagosFact" runat="server" CssClass="btn btn-primary btn-xs" CommandArgument='<%# Eval("UUID").ToString() %>' CausesValidation="false" OnClick="lbHistorialPagos_Click">VER</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="Label4" runat="server" Text="SELECCIONAR"></asp:Label>
                            <%--<br />
                            <center>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="SelectAll(this);" data-toggle="tooltip" data-placement="top" title="Seleccionar todos los comprobantes por pagar" />--%>
                        </center>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="check" runat="server" data-toggle="tooltip" data-placement="top" title='<%# Eval("ID", "Seleccionar comprobante {0}") %>' Visible='<%# decimal.Parse(Eval("saldoPendiente").ToString()) > 0 %>' />
                            <asp:HiddenField ID="checkHdID" runat="server" Value='<%# Eval("ID") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
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
            <asp:Parameter DefaultValue="Dat_General.tipo <> 'C' AND Dat_General.estado = '1' AND Dat_General.codDoc = '01'" Name="QUERYSTRING" Type="String" />
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
    <div class="modal fade " id="divPagos32">
        <div class="modal-dialog">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Modificar Pagos</h4>
                </div>
                <div class="modal-body ">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-3">SALDO:</div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbSaldo" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true" placeholder="0.00 *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">PAGO APLICADO:</div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbPagoAplicado" runat="server" Style="text-align: center" CssClass="form-control" placeholder="0.00 *" AutoPostBack="true" OnTextChanged="CalcularPago">0.00</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbPagoAplicado" ValidChars=",."></asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">PAGO PENDIENTE:</div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbPagoPendiente" runat="server" Style="text-align: center" CssClass="form-control" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:LinkButton ID="lbActualizarPago" runat="server" CssClass="btn btn-primary" OnClick="lbActualizarPago_Click">Actualizar</asp:LinkButton>
                                </div>
                                <div class="col-md-4"></div>
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
    <div class="modal fade" id="divHistorialPagos33">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">HISTORIAL DE COMPLEMENTOS DE PAGO</h4>
                        </div>
                        <div class="modal-body ">
                            <asp:GridView ID="gvHistorialPagos" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" DataSourceID="SqlDataSourcePagos" DataKeyNames="idPago" fixed-height OnRowDataBound="gvHistorialPagos_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="UUID" HeaderText="UUID Complemento" />
                                    <asp:BoundField DataField="MontoPago" HeaderText="Monto total de Pago" />
                                    <asp:TemplateField HeaderText="Importe pagado">
                                        <ItemTemplate>
                                            <asp:Label ID="lblImpPagado" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FormaPago" HeaderText="Forma de Pago" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                    <asp:TemplateField HeaderText="XML">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlXml" runat="server" NavigateUrl='<%# Eval("XML", "~/download.aspx?file={0}") %>' Style="cursor: pointer">
                                                <asp:Image ID="imgXML" runat="server" ImageUrl="~/imagenes/xml.png" Height="20" />
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PDF">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnPdfComplemento" runat="server" CommandArgument='<%# Eval("idPago") + ";" + Eval("id_Comprobante") + ";" + Eval("UUID") %>' BorderStyle="None" BorderWidth="0" OnClick="btnPdfComplemento_Click" CausesValidation="false" ValidateRequestMode="Disabled">
                                                <asp:Image ID="imgPDF" runat="server" ImageUrl="~/imagenes/pdf.png" Height="20" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="table" />
                                <EmptyDataTemplate>No existen pagos registrados.</EmptyDataTemplate>
                                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSourcePagos" runat="server" SelectCommand="SELECT * FROM Dat_ComplementosPago p INNER JOIN Dat_General g ON g.idComprobante = p.id_Comprobante WHERE g.numeroAutorizacion = @uuid">
                                <SelectParameters>
                                    <asp:Parameter Name="uuid" DefaultValue="-" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="modal fade " id="divPagos33">
        <div class="modal-dialog modal-lg">
            <div class="modal-content ">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">GENERAR COMPLEMENTO DE PAGOS</h4>
                        </div>
                        <div id="divPagos33Content" class="modal-body" runat="server">
                            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse">INFORMACIÓN DEL PAGO</a>
                                        </h4>
                                    </div>
                                    <div id="collapse" class="panel-collapse collapse" role="tabpanel">
                                        <div class="panel-body">
                                            <!-- [Inicio] Primera seccion -->
                                            <div class="row">
                                                <div class="col-md-4">
                                                    Fecha Pago:
                                                       
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbFecha_Pago" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-4">Forma de Pago:</div>
                                                <div class="col-md-4">Moneda:</div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="input-group date">
                                                        <asp:TextBox ID="tbFecha_Pago" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <i class="fa fa-calendar-o"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlForma_Pago" runat="server" DataSourceID="SqlDataMetodoPago" DataTextField="descripcion" DataValueField="codigo" CssClass="form-control" Style="text-align: center"></asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataMetodoPago" runat="server" SelectCommand="SELECT codigo, (codigo + ': ' + descripcion) as descripcion FROM Cat_Catalogo1_C WHERE tipo = '@tipoCatalogo';"></asp:SqlDataSource>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlMoneda_Pago" runat="server" CssClass="form-control" Style="text-align: center" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_Pago_SelectedIndexChanged">
                                                        <asp:ListItem Value="MXN" Text="Pesos Mexicanos (MXN)"></asp:ListItem>
                                                        <asp:ListItem Value="USD" Text="Dólares Americanos (USD)"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <!-- [Fin] Primera seccion -->
                                            <div class="row">&nbsp;</div>
                                            <!-- [Inicio] Segunda seccion -->
                                            <div class="row">
                                                <div class="col-md-4">
                                                    Monto:
                                                       
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbMonto_Pago" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-4"># Operación:</div>
                                                <div class="col-md-4">Tipo Cambio:</div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <%--                            <asp:TextBox ID="tbMonto_Pago" runat="server" CssClass="form-control input-decimal" Style="text-align: center" placeholder="0.00 *" OnTextChanged="tbMonto_Pago_TextChanged" AutoPostBack="true"></asp:TextBox>--%>
                                                    <asp:TextBox ID="tbMonto_Pago" runat="server" CssClass="form-control input-decimal" Style="text-align: center" placeholder="0.00 *"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMonto_Pago" ValidChars=",."></asp:FilteredTextBoxExtender>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="tbNumOperacion_Pago" runat="server" CssClass="form-control upper" Style="text-align: center" placeholder=""></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="tbTipoCambio_Pago" runat="server" CssClass="form-control input-decimal" Style="text-align: center" Text="1.0" placeholder="TIPO DE CAMBIO *"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTipoCambio_Pago" ValidChars=",."></asp:FilteredTextBoxExtender>
                                                </div>

                                                </br>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:LinkButton ID="lbsig" CssClass="btn btn-primary" runat="server" OnClick="lbSiguiente_CLick" Text="Siguiente" ValidationGroup="validationPagos"><i  aria-hidden="true"></i>&nbsp;Siguiente</asp:LinkButton>
                                                </div>

                                            </div>
                                            </div>

                                            <!-- [Fin] Segunda seccion -->

                                            <div class="row">&nbsp;</div>
                                            <!-- [Inicio] Cuarta seccion -->
                                            <div class="row" id="rowGridViewDoctosRelacionados" runat="server" style="display: none;">
                                                <div class="col-md-12">
                                                    <asp:GridView ID="gvDoctosRelacionados" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-responsive table-hover" BackColor="White" AllowPaging="false" AllowSorting="True" Font-Size="Smaller" GridLines="None" DataKeyNames="uuid" fixed-height>
                                                        <Columns>
                                                            <asp:BoundField DataField="Uuid" HeaderText="UUID" />
                                                            <asp:BoundField DataField="Serie" HeaderText="Serie" />
                                                            <asp:BoundField DataField="Folio" HeaderText="Folio" />
                                                            <asp:TemplateField HeaderText="Parcialidad">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="tbParcialidad" runat="server" CssClass="form-control input-sm" Style="text-align: center;" placeholder="1,2,3..." OnTextChanged="tbParcialidad_TextChanged" AutoPostBack="true" CausesValidation="true" UUID='<%# Eval("Uuid").ToString() %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SaldoAnterior" HeaderText="Anterior" />
                                                            <asp:BoundField DataField="SaldoInsoluto" HeaderText="Insoluto" />
                                                            <asp:TemplateField HeaderText="Pagado">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="tbImportePagado" runat="server" CssClass="form-control input-decimal input-sm" Style="text-align: center;" placeholder="0.00 *" OnTextChanged="tbImportePagado_TextChanged" AutoPostBack="true" UUID='<%# Eval("Uuid").ToString() %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Historial">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbHistorialPagos" runat="server" CssClass="btn btn-primary btn-sm" CommandArgument='<%# Eval("Uuid").ToString() %>' OnClick="lbHistorialPagos_Click">Ver</asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataRowStyle CssClass="table" />
                                                        <EmptyDataTemplate>No existen pagos registrados.</EmptyDataTemplate>
                                                        <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <!-- [Fin] Cuarta seccion -->
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-group" id="accordionInner1" role="tablist" aria-multiselectable="true">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" data-parent="#accordionInner1" href="#collapseInner1">INFORMACIÓN BANCARIA</a>
                                        </h4>
                                    </div>
                                    <div id="collapseInner1" class="panel-collapse collapse" role="tabpanel">
                                        <div class="panel-body">
                                            <!-- [Inicio] Primera linea -->
                                            <div class="row">
                                                <div class="col-md-2">Cta. Ordenante</div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlCtaOrd" runat="server" CssClass="form-control bootstrap-select" Style="text-align: center" DataSourceID="SqlDataSourceCtasBancos" DataTextField="NombreCuenta" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlCtaOrd_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-2">Cta. Beneficiario</div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlCtaBen" runat="server" CssClass="form-control bootstrap-select" Style="text-align: center" DataSourceID="SqlDataSourceCtasBancos" DataTextField="NombreCuenta" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlCtaBen_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <asp:SqlDataSource ID="SqlDataSourceCtasBancos" runat="server" SelectCommand="SELECT '' AS Id, 'Seleccione' AS NombreCuenta UNION SELECT Id, NombreCuenta FROM Cat_BancosComplemento"></asp:SqlDataSource>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <hr />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    RFC Emisor Cta. Ord.:<asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                                        runat="server"
                                                        ControlToValidate="tbRfcEmisorCtaOrd_Pago"
                                                        ErrorMessage="*"
                                                        ForeColor="Red"
                                                        SetFocusOnError="True"
                                                        ValidationGroup="validationPagos"
                                                        ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                                    </asp:RegularExpressionValidator>
                                                </div>
                                                <div class="col-md-4">Banco Ordenante:</div>
                                                <div class="col-md-4">Cuenta Ordenante:</div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="tbRfcEmisorCtaOrd_Pago" runat="server" CssClass="form-control upper" Style="text-align: center" placeholder="AAA[A]999999XXX" MaxLength="13"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="tbBancoOrd_Pago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="Banco extranjero" MaxLength="300"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="tbCtaOrd_Pago" runat="server" CssClass="form-control upper" Style="text-align: center" placeholder="" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <!-- [Fin] Primera linea -->
                                            <div class="row">&nbsp;</div>
                                            <!-- [Inicio] Segunda linea -->
                                            <div class="row">
                                                <div class="col-md-6">
                                                    RFC Emisor Cta. Ben.:<asp:RegularExpressionValidator ID="RegularExpressionValidator3"
                                                        runat="server"
                                                        ControlToValidate="tbRfcEmisorCtaBen_Pago"
                                                        ErrorMessage="*"
                                                        ForeColor="Red"
                                                        SetFocusOnError="True"
                                                        ValidationGroup="validationPagos"
                                                        ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                                    </asp:RegularExpressionValidator>
                                                </div>
                                                <div class="col-md-6">Cuenta Beneficiario:</div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="tbRfcEmisorCtaBen_Pago" runat="server" CssClass="form-control upper" Style="text-align: center" placeholder="AAA[A]999999XXX" MaxLength="13"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="tbCtaBen_Pago" runat="server" CssClass="form-control upper" Style="text-align: center" placeholder=""></asp:TextBox>
                                                </div>
                                            </div>
                                            <!-- [Fin] Segunda linea -->
                                            <div class="row">&nbsp;</div>
                                            <!-- [Inicio] Tercera seccion -->
                                            <div class="row">
                                                <div class="col-md-3">Tipo Cadena de Pago:</div>
                                                <div class="col-md-9">
                                                    <asp:DropDownList ID="ddlTipoCad_Pago" runat="server" CssClass="form-control" Style="text-align: center">
                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                        <asp:ListItem Value="01" Text="SPEI"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">Certificado (Base64):</div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="tbCert_Pago" runat="server" CssClass="form-control" Style="text-align: center" placeholder=""></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">Cad. Orig. (Base64):</div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="tbCad_Pago" runat="server" CssClass="form-control" Style="text-align: center" placeholder=""></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">Sello (Base64):</div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="tbSello_Pago" runat="server" CssClass="form-control" Style="text-align: center" placeholder=""></asp:TextBox>
                                                </div>
                                            </div>
                                            <!-- [Fin] Tercera seccion -->
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">&nbsp;</div>
                            <div class="row">
                                <div class="col-md-10"></div>
                                <div class="col-md-2">
                                    <asp:LinkButton ID="lbGuardarPagosComprobante" CssClass="btn btn-primary" runat="server" OnClick="lbGuardarPagosComprobante_Click" Text="Buscar" ValidationGroup="validationPagos"><i class="fa fa-usd" aria-hidden="true"></i>&nbsp;Emitir Pago</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
