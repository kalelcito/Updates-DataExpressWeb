<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="consultarCodigo.aspx.cs" Inherits="DataExpressWeb.ConsultarCodigo" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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

        .tooltip-inner {
            white-space: pre;
            max-width: none;
        }

        hr {
            border: 0;
            box-shadow: 0 0 10px 1px black;
            height: 0; /* Firefox... */
        }

            hr:after { /* Not really supposed to work, but does */
                content: "\00a0"; /* Prevent margin collapse */
            }

        .btn, .form-control:focus {
            border-color: transparent;
        }

        .form-control.moshi:focus {
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgba(255,175,69,.6) !important;
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgba(255,175,69,.6) !important;
        }

        .btn.moshi, .form-control.moshi {
            /*border-color: rgb(255,175,69) !important;*/
            border-radius: 0;
        }
    </style>
    <script type="text/javascript">
        function generarComprobante2() {
            var RfcRec = $('#<%=tbRfcRec.ClientID%>').val();
            var RazonSocialRec = $('#<%=tbRazonSocialRec.ClientID%>').val();
            var CpRec = $('#<%=tbCpRec.ClientID%>').val();
            <%--var UsoCfdi = $('#<%=ddlUsoCfdi.ClientID%>').val();--%>
            var UsoCfdi2 = $('#<%=ddlUsoCfdi.ClientID%>').find('option:selected').text();
            /*
            if (UsoCfdi2 == "Seleccione" || UsoCfdi == "Seleccione") {
                showDivOnModalLevel('modalValidation2', 4);
                return false;
            }
            else
            {
                return true;
            }*/

            if (RfcRec.length >= 12 && RfcRec != null && RfcRec != "" && RazonSocialRec != null && RazonSocialRec != "" && CpRec != null && CpRec != "" && UsoCfdi2 != "Seleccione" )
            {
                return true;
            }
            else
            {
                showDivOnModalLevel('modalValidation2', 4);
                $('#<%=tbRfcRec.ClientID%>').focus();
                $('#<%=tbRazonSocialRec.ClientID%>').focus();
                $('#<%=tbCpRec.ClientID%>').focus();
                <%--$('#<%=ddlUsoCfdi.ClientID%>').focus();--%>
                return false;
            }
        }

        function generarComprobante() {
            var returned = false;
            if (!Page_ClientValidate('validationExtranet')) {
                showDivOnModalLevel('modalValidation', 4);
            } else {
                var mail = $('#<%=tbMailReceptor.ClientID%>').val();
                var question = confirm('¿Seguro/a de que ' + mail + ' es tu correo electrónico?');
                if (question == true) {
                    $('#<%= progressCrear.ClientID %>').css('display', 'inline');
                    returned = true;
                } else {
                    returned = false;
                }
                <%--$("div#divLoading").addClass('show');
                ValidateMail(mail).done(function (data) {
                    $("div#divLoading").removeClass('show');
                    var valid = data.d;
                    if (valid == 0) {
                        alertBootBox('El E-mail ' + mail + ' no es válido', 4);
                        returned = false;
                    } else {
                        var question = true;
                        if (valid == 2) {
                            question = confirm('¿Seguro/a de que ' + mail + ' es tu correo electrónico?');
                        }
                        if (question == true) {
                            $('#<%= progressCrear.ClientID %>').css('display', 'inline');
                            returned = true;
                        } else {
                            returned = false;
                        }
                    }
                });--%>
            }
            return returned;
        }

        function componentToHex(c) {
            var hex = c.toString(16);
            return hex.length == 1 ? "0" + hex : hex;
        }

        function rgbToHex(r, g, b) {
            return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
        }

        function ValidateMail(command) {

            return $.ajax({
                type: "POST",
                url: "./consultarCodigo.aspx/ValidateMail",
                data: '{email: "' + command + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    console.log('ValidateMail: ' + data.d);
                },
                failure: function (data) {
                    console.log('ValidateMail: FAIL');
                }
            });
        }
        function ddlMetodoPago_Changed(command) {
            $('#<%= hfMetodoPago.ClientID %>').val(command);
        }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CONSULTAR TICKET
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div id="modalValidation2" class="modal-div">
        <asp:Label ID="Label2" runat="server" Text="Los campos RFC:, NOMBRE/RAZÓN SOCIAL:, C.P.: y USO CFDI: deben ir llenos"></asp:Label>
    </div>
    <div id="modalValidation" class="modal-div">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="validationExtranet" />
        <asp:Label ID="ValidationList" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfColor" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <asp:Label ID="Label1" runat="server"
                        Style="font-family: Arial; font-weight: 700" Text="Buscador de Documentos Electronicos"></asp:Label>
                </div>
                <div class="col-md-4"></div>
            </div>
            <div id="divSucursal" runat="server" style="display: none">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6" id="divSucursalText">Sucursal:</div>
                    <div class="col-md-3"></div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <asp:DropDownList ID="ddlSucursalEmisor" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSourceSucursalesEmisor" DataValueField="idSucursal" DataTextField="sucursal"></asp:DropDownList>
                        <%--<asp:SqlDataSource ID="SqlDataSourceSucursalesEmisor" runat="server" SelectCommand="SELECT idSucursal, (clave + ': ' + sucursal) as sucursal from Cat_SucursalesEmisor WHERE RFC = @RFC AND eliminado = 0">
                            <SelectParameters>
                                <asp:SessionParameter Name="RFC" SessionField="IDENTEMIEXT" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>
                        <asp:SqlDataSource ID="SqlDataSourceSucursalesEmisor" runat="server" SelectCommand="SELECT idSucursal, (clave + ': ' + sucursal) as sucursal from Cat_SucursalesEmisor WHERE eliminado = 0"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-3"></div>
                </div>
                <br />
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6" id="divReferenciaText">
                    Número de Referencia:
                </div>
                <div class="col-md-3"></div>
            </div>

            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-5">
                    <asp:TextBox ID="tbTicket" runat="server" Style="text-align: center; width: 113% !important;" CssClass="form-control" placeholder="REFERENCIA *" data-toggle="tooltip" data-placement="right" title=""></asp:TextBox><br />
                </div>
                <div class="col-md-1" style="text-align: right">
                    <button type="button" class="btn btn-default" data-toggle="tooltip" data-placement="right" title="Encuentra tu número de referencia" onclick="$('#TutorialTicket').modal('show');">
                        <i class="fa fa-question" aria-hidden="true"></i>
                    </button>
                </div>
                <div class="col-md-3"></div>
            </div>

            <br />
            <div class="row">
                <div class="col-md-2"></div>
                <div class="col-md-8">
                    <asp:Button ID="bGenerar1" runat="server" CssClass="btn btn-primary" OnClick="bGenerar1_Click" Text="Generar" />
                    <asp:Button ID="bConsultar" runat="server" CssClass="btn btn-primary" OnClick="BModdd" Text="Buscar" />
                    <asp:Button ID="bGenerarConsultar" runat="server" CssClass="btn btn-primary" OnClick="bGenerar1_Click" Text="Buscar" Visible="false" />
                    <asp:Button ID="bLimpiar" runat="server" CssClass="btn btn-primary" OnClick="bLimpiar_Click" Text="Limpiar" />
                    <asp:HyperLink ID="hlRegresar" runat="server" CssClass="btn btn-primary" Text="Regresar" NavigateUrl="~/Default.aspx" />
                </div>
                <div class="col-md-2"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade " id="PanelModal">
        <div class="modal-dialog">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Descargar Comprobante</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-6">
                                    Fecha de Emisión:
                                    <asp:RequiredFieldValidator ID="tfecha_RequiredFieldValidator" runat="server" ErrorMessage="*" ForeColor="Red" SetFocusOnError="true" ControlToValidate="tfecha" ValidationGroup="DatosExtras"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-6">
                                    <div class="input-group date">
                                        <asp:TextBox ID="tfecha" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="11" placeholder="FECHA *"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                                    </div>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-6">
                                    Total del Ticket
                                    <asp:RequiredFieldValidator ID="tmonto_RequiredFieldValidator" runat="server" ErrorMessage="*" ForeColor="Red" SetFocusOnError="true" ControlToValidate="tmonto" ValidationGroup="DatosExtras"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="tmonto" runat="server" CssClass="form-control input-decimal" placeholder="00.00 *" Style="text-align: center;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tmonto_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tmonto" ValidChars=",."></asp:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-6">
                                    RFC del Receptor
                                    <asp:RequiredFieldValidator ID="trfc_RequiredFieldValidator" runat="server" ErrorMessage="*" ForeColor="Red" SetFocusOnError="true" ControlToValidate="trfc" ValidationGroup="DatosExtras"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="trfc" runat="server" Style="text-align: center" CssClass="form-control upper" MaxLength="13" placeholder="RFC *"></asp:TextBox>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="SendDatos" runat="server" CssClass="btn btn-primary" OnClick="bConsultar_Click" Text="Buscar" ValidationGroup="DatosExtras" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModuloExtranet">
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
                                                    <asp:GridView ID="gvConceptos" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvConceptos_PageIndexChanging" PageSize="5" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD" />
                                                            <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" />
                                                            <asp:BoundField DataField="valorUnitario" HeaderText="VAL. UNITARIO" />
                                                            <asp:BoundField DataField="importe" HeaderText="IMPORTE" />
                                                            <asp:BoundField DataField="unidad" HeaderText="UNIDAD" />
                                                        </Columns>
                                                        <EmptyDataRowStyle CssClass="table empty" />
                                                        <EmptyDataTemplate>
                                                            No existen datos.
                                                        </EmptyDataTemplate>
                                                        <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" CssClass="gvHeader" />
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
                                                    <div id="rowDescuentos" runat="server" visible="false">
                                                        <div class="col-md-4">DESCUENTO:</div>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="tbDescuento" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
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
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="tbCodDoc" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1">AMBIENTE:</div>
                                                <div class="col-md-4">
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
                                                <div class="col-md-4">
                                                    <asp:HiddenField ID="hfMetodoPago" runat="server" Value="" />
                                                    <asp:DropDownList ID="ddlMetodoPago" runat="server" Style="text-align: center" DataSourceID="SqlDataMetodoPago" DataTextField="descripcion" DataValueField="codigo" SetControlID='<%# hfMetodoPago.ClientID %>' AutoPostBack="true" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="ddlMetodoPago_RequiredFieldValidator" runat="server" ControlToValidate="ddlMetodoPago" ErrorMessage="El código del pago no puede quedar vacío" Style="color: #FF0000" ValidationGroup="validationExtranet" Display="None" InitialValue=""></asp:RequiredFieldValidator>
                                                    <asp:SqlDataSource ID="SqlDataMetodoPago" runat="server" SelectCommand="SELECT codigo, (codigo + ': ' + descripcion) as descripcion FROM Cat_Catalogo1_C WHERE tipo = '@tipoCatalogo';"></asp:SqlDataSource>
                                                </div>
                                                <div class="col-md-1">CTA.:</div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="tbNumCtaPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CTA. DE PAGO *"></asp:TextBox>
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
                                    <div class="panel-heading" role="tab" id="heading22">
                                        <h4 class="panel-title">
                                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse22" aria-expanded="true" aria-controls="collapse22">COMPLEMENTO INE</a>
                                        </h4>
                                    </div>
                                    <div id="collapse22" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading22">
                                        <div class="panel-body">
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
                                                        PAÍS:<asp:RequiredFieldValidator ID="RequiredFieldValidator_tbPaisRec" runat="server" ControlToValidate="tbPaisRec" ErrorMessage="El país no puede quedar vacío" Display="None" ValidationGroup="validationExtranet" Enabled="False"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddlPais" runat="server" ControlToValidate="ddlPais" ErrorMessage="El país no puede quedar vacío" Display="None" InitialValue="" ValidationGroup="validationExtranet" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="tbPaisRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *" ReadOnly="False"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlPais" runat="server" CssClass="form-control bootstrap-select" Style="text-align: center"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1">
                                                        E-MAIL(S):<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbMailReceptor" ErrorMessage="El E-mail no puede quedar vacío" Display="None" ValidationGroup="validationExtranet" Enabled="True"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="tbMailReceptor" runat="server" CssClass="form-control" Style="text-align: center" placeholder="email@dominio.com,emal@dominio.com *"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row" id="DivUsoCfdi" runat="server" style="display: none">
                                                    <div class="col-md-2">
                                                        USO CFDI:
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_UsoCfdi" runat="server" ControlToValidate="ddlUsoCfdi" ErrorMessage="El uso del CFDI no puede quedar vacío" Style="color: #FF0000" ValidationGroup="validationExtranet" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <asp:DropDownList ID="ddlUsoCfdi" runat="server" CssClass="form-control" Style="text-align: center" DataSourceID="SqlDataSourceUsoCfdi" DataTextField="descripcion" DataValueField="clave" Enabled="false"></asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSourceUsoCfdi" runat="server" SelectCommand="SELECT '0' AS clave,'Seleccione'AS descripcion UNION SELECT clave,descripcion FROM Cat_UsoCfdi WHERE tipoPersona LIKE @tipoPersonaUsoCfdi">
                                                            <SelectParameters>
                                                                <asp:Parameter Name="tipoPersonaUsoCfdi" DefaultValue="%FM%" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
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
                    <asp:LinkButton ID="bGenerar" runat="server" OnClientClick="return generarComprobante2();" OnClick="bGenerar_Click" CssClass="btn btn-primary">Generar Comprobante</asp:LinkButton>&nbsp;&nbsp;
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
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

    <div class="modal fade" id="TutorialTicket">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Facturación en Cuatro Pasos</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanelTutorialTicket" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div id="carouselTutorialTicket" class="carousel slide carousel-fixed" data-ride="carousel">
                                <!-- Indicators -->
                                <ol class="carousel-indicators">
                                    <li data-target="#carouselTutorialTicket" data-slide-to="0" class="active"></li>
                                    <li data-target="#carouselTutorialTicket" data-slide-to="1"></li>
                                    <li data-target="#carouselTutorialTicket" data-slide-to="2"></li>
                                    <li data-target="#carouselTutorialTicket" data-slide-to="3"></li>
                                </ol>
                                <!-- Wrapper for slides -->
                                <div class="carousel-inner" role="listbox">
                                    <div class="item active">
                                        <asp:Image ID="imgPaso1" runat="server" ImageUrl="" AlternateText="Paso 1" />
                                    </div>
                                    <div class="item">
                                        <asp:Image ID="imgPaso2" runat="server" ImageUrl="" AlternateText="Paso 2" />
                                    </div>
                                    <div class="item">
                                        <asp:Image ID="imgPaso3" runat="server" ImageUrl="" AlternateText="Paso 3" />
                                    </div>
                                    <div class="item">
                                        <asp:Image ID="imgPaso4" runat="server" ImageUrl="" AlternateText="Paso 4" />
                                    </div>
                                </div>

                                <!-- Controls -->
                                <a class="left carousel-control" href="#carouselTutorialTicket" role="button" data-slide="prev">
                                    <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                                    <span class="sr-only">Previous</span>
                                </a>
                                <a class="right carousel-control" href="#carouselTutorialTicket" role="button" data-slide="next">
                                    <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                                    <span class="sr-only">Next</span>
                                </a>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade " id="ModuloFactura">
        <div class="modal-dialog">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Descargar Comprobante</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div style="width: 50%;">
                                <table class="table table-condensed table-responsive table-hover">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center">XML</th>
                                            <th style="text-align: center">PDF</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:HyperLink ID="hlXml" runat="server" NavigateUrl="#">
                                                    <asp:Image ID="imgXML" runat="server" ImageUrl="~/imagenes/xml.png" />
                                                </asp:HyperLink>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:LinkButton ID="btnDPDF" runat="server" Height="19" BorderStyle="None" BorderWidth="0" OnClick="btnDPDF_Click">
                                                    <asp:Image ID="imgPDF" runat="server" ImageUrl="~/imagenes/pdf.png" />
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <hr />
                            <table class="table table-condensed table-responsive table-hover">
                                <thead>
                                    <tr>
                                        <th style="text-align: center">Enviar Comprobante por E-Mail</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td style="text-align: center">
                                            <div class="col-md-8">
                                                <asp:TextBox ID="tbEmail" runat="server" CssClass="form-control" Style="text-align: center" placeholder="email@dominio.com,emal@dominio.com *"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:CheckBox ID="checkXML" runat="server" Checked="True" Text="XML" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:CheckBox ID="checkPDF" runat="server" Checked="True" Text="PDF" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="bMail" runat="server" CssClass="btn btn-primary btn-sm" CausesValidation="false" OnClick="bMail_Click" data-toggle="tooltip" data-placement="top" title="Enviar">
                    <span class="
glyphicon glyphicon-send" aria-hidden="true"></span></asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <div id="mdfa" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>

                    <h4 class="modal-title" style="text-align: center;">
                        <span style="color: #fab702; font-size: larger; font-weight: bold; text-align: center;">AVISO DE PRIVACIDAD
                        </span>
                    </h4>
                </div>
                <div class="modal-body" id="contenidoAviso" style="text-align: left;">
                    <p>
                        Los datos proporcionados por nuestros clientes son tratados por
    RESTAURANTES MOSHI MOSHI S.A.P.I. DE C.V. que tiene domicilio en: LAGO
    TEXCOCO 112 PB, DELEGACION ANAHUAC Ia SECCION, DELEGACION MIGUEL HIDALGO,
    CP 11320 MEXICO DF.
    <br>
                        <br>
                        Los datos personales que recabamos por teléfono, internet o vía correo
    electrónico, tienen como finalidad incluirle en un programa de recompensas
    por consumo que realice, evaluar la calidad de nuestros servicios y
    productos, mantener la información en una base de datos para agilizar los
    pedidos de servicio a domicilio, así como generar facturación por consumos
    y para ello requerimos lo siguiente: Nombre, dirección, teléfono, RFC,
    correo electrónico, fecha de nacimiento.
    <br>
                        <br>
                        En todo momento usted podrá revocar o modificar el consentimiento que ha
    otorgado para el tratamiento de sus datos personales, con el fin de dejar
    de hacer uso de los mismos. Para ello, es necesario que presente su
    petición por escrito en la dirección de correo electrónico
    <a href="mailto:datospersonales@moshimoshi.mx" target="_blank">datospersonales@moshimoshi.mx
    </a>
                        donde le responderemos en un plazo no mayor a 7 (siete) días hábiles.
    <br>
                        <br>
                        En caso de existir alguna modificación al presente Aviso de Privacidad se
    hará de su conocimiento en este mismo sitio de Internet.
    <br>
                        <br>
                        Le informamos que para cumplir con las finalidades previstas en este aviso,
    nos comprometemos a que los datos serán tratados bajo medidas de seguridad,
    siempre garantizando su confidencialidad.
                    </p>
                </div>

            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <div id="mdtyc" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" style="text-align: center;">
                        <span style="color: #fab702; font-size: larger; font-weight: bold; text-align: center;">TÉRMINOS Y CONDICIONES 
                        </span>
                    </h4>
                </div>
                <div class="modal-body" id="contenidoTerminos" style="text-align: left;">
                    <p>
                        <strong>Términos y Condiciones del Programa de Lealtad </strong>
                    </p>
                    <p>
                        El Programa de Lealtad “Puntos Moshi“ es un programa mediante el cual los
    participantes, a través de sus consumos en los restaurantes participantes
    podrán acumular puntos que pueden ser utilizados para el pago de consumos
    de alimentos y bebidas en una visita posterior.
                    </p>
                    <p>
                        El Programa de Lealtad “Puntos Moshi“ estará sujeto a los siguientes
    términos y condiciones:
                    </p>
                    <p>
                        <strong>1. GENERALES</strong>
                    </p>
                    <p>
                        1.1. El presente documento sienta las bases de la operación respecto del
    Programa de Lealtad “Puntos Moshi“ que, de manera enunciativa mas no
    limitativa, se basa en: (i) la acumulación de Puntos en la cuenta personal
    de los Participantes; (ii) el uso de los Puntos como medio de pago en
    restaurantes, y (iii) las promociones y beneficios.
                    </p>
                    <p>
                        <strong>2. PROGRAMA DE LEALTAD “</strong>
                        Puntos Moshi<strong>“</strong>
                    </p>
                    <p>
                        2.1. El Programa de Lealtad es operado y administrado por RESTAURANTES
    MOSHI MOSHI, S.A.P.I.. DE C.V., en adelante (CLIENTE). El Programa de
    Lealtad es aplicable para consumos en los restaurantes que operan bajo la
    marca “Moshi Moshi®“ que forman parte de la cadena del CLIENTE en el
    territorio mexicano (excepto aquéllos que operan dentro el modelo de
    negocio conocido como “Cocina Abierta”) y está dirigido a los comensales
    que de estos restaurantes elijan participar en el mismo (los
    “Participantes”). Los Participantes podrían consultar sus cuentas y saldos,
    respecto del presente Programa de Lealtad, así como la información general,
    términos y condiciones y otras promociones relacionadas con el mismo, en la
página de Internet:    <a href="http://www.moshimoshi.mx/">www.moshimoshi.mx</a>.
                    </p>
                    <p>
                        El registro al Programa de Lealtad “Puntos Moshi“ es sin costo alguno para
    los Participantes.
                    </p>
                    <p>
                        2.2. Todos los Participantes que se hayan registrado en el Programa de
    Lealtad “Puntos Moshi“ tendrán como beneficio durante su participación, la
    bonificación en puntos, equivalente al 10% del monto total de su cuenta
    (incluyendo impuestos pero sin considerar cualquier monto por concepto de
    propina) en cada visita que hagan a los restaurantes que operan bajo la
    marca “Moshi Moshi®”, salvo casos puntuales de activaciones, marketing o
    promociones especiales.
                    </p>
                    <p>
                        2.3 Los puntos serán acumulados por los Participantes al haber pagado una
    cuenta de consumo de alimentos o bebidas en los restaurantes participantes.
    Los Puntos podrán utilizarse como una forma de pago para liquidar la cuenta
    del consumo dentro de los restaurantes participantes en una visita
    posterior. Los puntos no tienen valor monetario y no pueden canjearse por
    dinero en efectivo, vales de despensa o cederse para el pago de deudas de
    ningún tipo, ni podrán utilizarse para pago de otras promociones a menos
    que exista alguna en particular que así lo señale y que se haya publicado
    en la página web antes señalada.
                    </p>
                    <p>
                        <strong>3. PARTICIPANTES</strong>
                    </p>
                    <p>
                        3.1. Puede participar en el Programa de Lealtad “Puntos Moshi“ cualquier
    persona física que sea mayor de 12 años de edad, que resida en el
    territorio Mexicano y que haya registrado en el Programa de Lealtad.
                    </p>
                    <p>
                        3.2. El Participante al registrarse en el Programa de Lealtad “Puntos Moshi“, 
estará aceptando los presentes Términos y Condiciones.
                    </p>
                    <p>
                        Los presentes Términos y Condiciones podrán consultarse en la página de
    Internet: <a href="http://www.moshimoshi..mx./">www.moshimoshi.mx</a>.
                    </p>
                    <p>
                        3.3. Los Puntos adquiridos por el Participante se pueden acumular y/o
    canjear una vez que el Participante se haya registrado electrónicamente con
    sus datos personales en el Programa “Puntos Moshi“; el registro puede
realizarse en la página    <a href="http://www.moshimoshi..mx./">www.moshimoshi.mx</a>
                    </p>
                    <p>
                        3.4. El CLIENTE no tiene obligación alguna de informarle al Participante
    sobre cualquier error en su solicitud inicial.
                    </p>
                    <p>
                        3.5. No se aceptarán registros múltiples al Programa de Lealtad realizados
    por el mismo Participante o por otras personas con los datos de un mismo
    Participante; por tal razón, si se detecta la duplicidad de datos o
    registros múltiples con el mismo nombre, éstos se darán de baja de
    inmediato.
                    </p>
                    <p>
                        3.6. Es responsabilidad del Participante y debe asegurarse que sus datos
    (registrados en el proceso de registro) son precisos y actuales. En caso de
    error, rectificación o cambio de los datos personales el participante
deberá comunicarse a través del correo electrónico    <a href="mailto:sistemas@moshimoshi.mx ">sistemas@moshimoshi.mx </a>El dato
    identificador del Participante en el programa será su correo electrónico el
    cual ha sido proporcionado por el Participante al momento de afiliarse al
    Programa de Lealtad, mismo que no podrá ser modificado. Si el Participante
    no brinda correctamente su correo electrónico no podrá confirmar su
    registro, el CLIENTE no será responsable de las consecuencias de la falta
    de veracidad por parte del Participante al proporcionar sus datos
    personales.
                    </p>
                    <p>
                        3.7. Los participantes podrían consultar el Aviso de Privacidad, en la
    página <a href="http://www.moshimoshi..mx./">www.moshimoshi.mx</a> o a
    través de la aplicación móvil (App) “Moshi Moshi App“, sin necesidad de
    contar con un usuario o contraseña.
                    </p>
                    <p>
                        3.8. CLIENTE podrá rechazar el registro de un Participante, así como
    bloquear o cancelar una cuenta ya sea temporalmente o definitivamente por
    presentar mal manejo de sus derechos. Los Participantes podrán hacer
    aclaraciones al correo electrónico antes señalado o mediante la página de
    Internet en la sección identificada como “Mi Cuenta”.
                    </p>
                    <p>
                        3.9. La clave de usuario (correo electrónico) y contraseña serán creadas
    por los Participantes; son personales y su manejo es responsabilidad de
    cada Participante
                    </p>
                    <p>
                        3.10. Los formatos de registro al Programa de Lealtad “Puntos Moshi”
    (digitales) estarán a disposición de los Participantes a través de la
página de Internet del Programa de Lealtad    <a href="http://www.moshimoshi..mx./">www.moshimoshi.mx</a>. El registro
    del Participante podrá realizarlo cada uno por su cuenta.
                    </p>
                    <p>
                        <strong>4. VALOR DE LOS PUNTOS “</strong>
                        Puntos Moshi<strong>“</strong>
                    </p>
                    <p>
                        4.1. Cada punto equivale a un peso, moneda nacional.
                    </p>
                    <p>
                        4.2. La información sobre el número de puntos acumulados estará a
    disposición del Participante en la página de Internet del Programa de
    Lealtad“ <a href="http://www.moshimoshi.mx/">www.moshimoshi.mx</a> Estos
    puntos “Puntos Moshi“, se podrán usar únicamente en los restaurantes
    participantes dentro del territorio mexicano.
                    </p>
                    <p>
                        4.3. Para acumular puntos a través de un consumo en el restaurante o en el
    servicio a domicilio, el Participante debe ingresar a su cuenta través de
    la página de Internet, ingresar a la sección de “acumulación”, registrar el
    número de ticket de consumo y esperar 24 horas para que se reflejen los
    puntos abonados en su cuenta de usuario.
                    </p>
                    <p>
                        Para acumular lo puntos correspondientes a un pedido en línea, una vez que
    el CLIENTE ha dado autorización de registro en el sistema “Puntos Moshi”,
    el Participante debe ingresar a su cuenta y realizar el pedido. Los puntos
    se reflejarán en automático, ya que el sistema obtendrá la información del
    ticket posterior al pago de la orden correspondiente.
                    </p>
                    <p>
                        4.4. No se podrán acumular juntos los tickets de cuentas divididas, en caso
    de que haya cuentas divididas, solo se podrá realizar la acumulación por un
    solo ticket.
                    </p>
                    <p>
                        *No se acumularán puntos de tickets anteriores a la fecha del registro en
    el programa.
                    </p>
                    <p>
                        4.5. Si el Participante decide realizar la acumulación de puntos por su
propia cuenta lo podrá realizar por medio de la página de Internet “    <a href="http://www.moshimoshi..mx./">www.moshimoshi.mx</a> en la sección
    de “Acumulación”, deberá contar forzosamente con un registro previo en el
    Programa “Puntos Moshi“ y contar físicamente con el ticket de consumo, ya
    que le serán solicitados datos contenidos en el mismo. Los datos requeridos
    para la acumulación fuera de restaurante, son: total de ticket, número de
    ticket, fecha de ticket y sucursal. Una vez registrado el ticket no podrá
    utilizarse nuevamente ya que el sistema bloquea la información. Los tickets
    de consumos anteriores a la fecha de registro del Participante en el
    programa no podrán ser acumulados como puntos.
                    </p>
                    <p>
                        4.6. El plazo máximo para acumular los puntos es máximo de 30 días
    naturales siguientes a la fecha del consumo y, por consiguiente, de
    generación del ticket.
                    </p>
                    <p>
                        4.7. Para acumular puntos sólo participan los tickets pagados en efectivo o
    con tarjeta de crédito o débito bancarias.
                    </p>
                    <p>
                        4.8. Durante la vigencia del Programa de Lealtad “Puntos Moshi“ se
    publicarán diversas promociones con las que contará el mismo programa.
Dichas promociones se comunicarán a través de la página de Internet    <a href="http://www.moshimoshi.mx./">www.moshimoshi.mx</a> y a través de
    otros medios de comunicación según lo decida CLIENTE.
                    </p>
                    <p>
                        Habrá promociones permanentes y promociones temporales para los
    participantes del Programa de Lealtad “Puntos Moshi“ y podrán hacerlas
    efectivas cumpliendo con las condiciones de cada una de las mismas.
                    </p>
                    <p>
                        La promoción permanente del Programa “Puntos Moshi“ es:
                    </p>
                    <p>
                        • Mecánica base por ser parte del programa, el participante acumulará en
    puntos, un monto equivalente al 10% del total de su consumo (incluyendo
    impuestos pero sin considerar cualquier monto por concepto de propina).
                    </p>
                    <p>
                        4.9. La administración y control del Programa de Lealtad “Puntos Moshi“
    será a través de un sistema informático conectado a las terminales punto de
    venta de los restaurantes que operan bajo la marca “Moshi Moshi®”; por tal
    motivo, este sistema puede presentar fallas ocasionalmente impidiendo que
    al momento de pago de la cuenta no se puedan acumular los puntos que se
    hayan generado en ese momento, por consiguiente el Participante tendrá 30
    días naturales contados a partir de la generación del ticket para
acumularlos personalmente desde la página de internet    <a href="http://www.moshimoshi.mx./">www.moshimoshi.mx.</a> En caso de que
    los problemas con la red impidan que la transmisión de datos se dé en un
    lapso mayor a 30 días posteriores a la fecha de emisión del ticket de
    consumo, el Participante recibirá un correo con informando que el sistema
    se haya reestablecido y podrá solicitar su acumulación de puntos vía el
    correo electrónico antes mencionado, proporcionando los datos del ticket
    para hacerlos válidos. En caso de que el restaurante de que se trate no
    cuente con sistema al momento que el Participante desee hacer el pago de su
    ticket con puntos, este pago no podrá efectuarse y no habrá algún tipo de
    condonación para el Participante por dicha eventualidad, debiendo el
    participante liquidar la cuenta mediante cualquier forma de pago legal en
    la República Mexicana.
                    </p>
                    <p>
                        <strong>5. ESTADO DE MOVIMIENTOS “Puntos Moshi“</strong>
                    </p>
                    <p>
                        5.1. Los puntos que haya acumulado el Participante se registrarán en la
    cuenta creada por él mismo al momento de su registro, podrá verificar el
    saldo de su cuenta, a través de la página de Internet del Programa “Puntos Moshi“ 
    en <a href="http://www.moshimoshi.mx/">www.moshimoshi.mx.</a>
                        en la sección de “Saldos y Movimientos” o solicitarlo al correo
electrónico:    <a href="mailto:sistemas@moshimoshi.mx ">sistemas@moshimoshi.mx</a> .
                    </p>
                    <p>
                        5.2. Cualquier reclamación con respecto al saldo de la cuenta se debe
presentar por escrito al correo electrónico de    <a href="mailto:sistemas@moshimoshi.mx">sistemas@moshimoshi.mx</a> donde se
    le asignará un código de seguimiento. La solicitud deberá ser dentro de un
    periodo máximo de 30 días naturales a partir de haberse generado el recibo
    de la reclamación. La reclamación debe estar respaldada por los recibos de
    consumo emitidos por la sucursal objeto de la reclamación. El saldo de la
    cuenta se considerará como confirmada si no se presenta reclamación alguna
    dentro del plazo antes señalado.
                    </p>
                    <p>
                        5.3. Todas las consultas de movimientos se podrán realizar dentro del sitio
    web <a href="http://www.moshimoshi.mx/">www.moshimoshi.mx</a> al día
    siguiente de haberse efectuado, en la sección de saldos y movimientos con
    la actualización a mes vencido.
                    </p>
                    <p>
                        <strong>6. PAGO CON PUNTOS </strong>
                    </p>
                    <p>
                        6.1. Los puntos serán válidos para pago de consumos de alimentos y bebidas
    en los restaurantes participantes. El Participante deberá avisar al mesero
    al momento de solicitar la cuenta que desea pagar con puntos. El pago de la
    cuenta con puntos podrá realizarse en el mismo restaurante desde cualquier
    dispositivo móvil, ingresando con correo y contraseña previamente
registrados en la página de Internet    <a href="http://www.moshimoshi.mx/">www.moshimoshi.mx</a> en la sección
    “Pago con Puntos” e ingresando los datos solicitados contenidos en el
    ticket (número de sucursal, ticket y fecha de emisión del ticket).
                    </p>
                    <p>
                        6.2. Los puntos no aplican para pago de propinas ni estacionamiento, ni con
otras promociones que no sean señaladas en la página de Internet    <a href="http://www.moshimoshi.mx/">www.moshimoshi.mx</a>
                    </p>
                    <p>
                        6.3. Los puntos no aplican para efectuar donaciones de ningún tipo.
                    </p>
                    <p>
                        6.4 Los puntos que se emiten a favor del Participante y estarán a su
    disposición para pago de consumos al momento que el cliente lo disponga,
    siempre y cuando estén vigentes según la política de vigencias estipulada
    en el apartado 9 de este documento.
                    </p>
                    <p>
                        6.5. En los restaurantes participantes se verificará el saldo de puntos
    antes del pago de consumo con puntos. El pago de las cuentas podrá
    realizarse en su totalidad con los puntos, en caso que éstos no sean
    suficientes el Participante podrá pagar el remanente en efectivo u otro
    medio de pago legal como tarjetas de crédito o débito.
                    </p>
                    <p>
                        <strong>7. VENCIMIENTO DE PUNTOS “Puntos Moshi“</strong>
                    </p>
                    <p>
                        7.1. Los puntos acumulados en el Programa “Puntos Moshi“ tienen una fecha
    de vencimiento de 12 meses a partir de la fecha de su acumulación, pasado
    este tiempo tus puntos vencerán y no podrán ser reembolsados ni reclamados
    de manera alguna.
                    </p>
                    <p>
                        7.2. En caso que se dé por terminado el Programa de Lealtad “Puntos Moshi“, 
    se publicará tal situación por medio de la página de Internet   
    <a href="http://www.moshimoshi.mx/">www.moshimoshi.mx</a> y los Puntos
    adquiridos vencerán a los 30 (treinta) días naturales siguientes de dicha
    publicación. En virtud de lo anterior, el Participante no podrá hacer
    reclamo alguno con motivo de dicho supuesto.
                    </p>
                    <p>
                        <strong>8. PROMOCIONES DE MERCADOTECNIA</strong>
                    </p>
                    <p>
                        8.1. Al aceptar estos Términos y Condiciones, y proporcionar sus datos
    personales, el Participante otorga su consentimiento a CLIENTE y a
    afiliados para que realice actividades promocionales respecto del presente
    Programa de Lealtad, así como enviar información comercial a través de
    medios de comunicación electrónicos. Los datos también serán utilizados con
    fines de estudios estadísticos y de análisis del mercado.
                    </p>
                    <p>
                        <strong>9. TERMINACIÓN DE LA PARTICIPACIÓN EN EL PROGRAMA “Puntos Moshi“
                        </strong>
                    </p>
                    <p>
                        9.1. El Participante puede cancelar su participación al Programa de Lealtad
    “Puntos Moshi“ en cualquier momento y para ello deberá solicitar el
bloqueo de su cuenta escribiendo a    <a href="sistemas@moshimoshi.mx">sistemas@moshimoshi.mx</a>. El
    Participante recibirá un correo electrónico confirmando la baja de su
    cuenta y participación en el programa.
                    </p>
                    <p>
                        9.2. Una vez que el Participante haya renunciado al Programa de Lealtad,
    los puntos acumulados por el Participante que se encuentren acumulados
    dentro de su cuenta “Puntos Moshi“ se cancelarán, sin posibilidad de ser
    nuevamente cargados.
                    </p>
                    <p>
                        9.3. CLIENTE puede cancelar el registro o la cuenta de un Participante con
    efecto inmediato, en caso de que el Participante proporcione a un tercero
    acceso a su cuenta o de cualquier otro modo, se violen estos Términos y
    Condiciones. CLIENTE notificará el aviso de cancelación por escrito (en
    forma digital) a través de un correo electrónico explicando la razón para
    cancelar la participación en el Programa “Puntos Moshi“. El Participante
    podrá solicitar aclaración respecto de la cancelación dentro de un periodo
    de 10 (diez) días naturales después de recibir el aviso de cancelación,
comunicándose al correo    <a href="mailto:sistemas@moshimoshi.mx">sistemas@moshimoshi.mx</a> ; en caso
    que el participante no solicite aclaración alguna dentro de dicho plazo, se
    tendrá por consentida la cancelación en el Programa de Lealtad “Puntos Moshi“.
                    </p>
                    <p>
                        9.4. Cualquier cancelación de participación en el Programa de Lealtad
    resultará la pérdida y cancelación de los puntos acumulados, así como la
    cancelación de la cuenta misma. El Participante podrá solicitar una nueva
    cuenta o solicitar la reactivación de una anterior pero esto no resultará
    en la reactivación de los puntos.
                    </p>
                    <p>
                        9.5. Después de la terminación o cancelación de la participación en el
    Programa de Lealtad, la cuenta del Participante quedará inactiva
    definitivamente en el sistema.
                    </p>
                    <p>
                        <strong>10. CANCELACIÓN, SUSPENSIÓN O MODIFICACIÓN DEL PROGRAMA “Puntos Moshi“
                        </strong>
                    </p>
                    <p>
                        10.1. CLIENTE puede decidir a su completa discreción suspender o terminar
    el Programa de Lealtad “Puntos Moshi“. También podrá modificar o completar
    en cualquier momento Términos y Condiciones, así como premios, beneficios,
    reglas y contenidos publicados para asegurar la operación del Programa
    “Puntos Moshi“. Esa decisión se le comunicará a los Participantes por
medio de la página de Internet    <a href="http://www.moshimoshi.mx./">www.moshimoshi.mx</a> o mediante envío
    de un correo electrónico a la cuenta registrada por el Participante al
    momento del registro, por lo menos con 30 (treinta) días naturales antes de
    la suspensión o cancelación del Programa “Puntos Moshi“.
                    </p>
                    <p>
                        10.2. Una vez que se haya notificado la cancelación del Programa de Lealtad
    “Puntos Moshi“ por parte del CLIENTE, el Participante podrá hacer uso de
    los puntos acumulados dentro del plazo de los 30 (treinta) días siguientes
    a dicha cancelación.
                    </p>
                    <p>
                        <strong>11. DISPOSICIONES ESPECIALES</strong>
                    </p>
                    <p>
                        11.1. Los puntos “Puntos Moshi“ que sean acumulados por los Participantes,
    podrán ser usados en cualquier sucursal participante, lo anterior estará
    sujeto a los presentes términos y condiciones.
                    </p>
                    <p>
                        11.2. El pago de propinas y estacionamiento no genera Puntos “Puntos Moshi“.
                    </p>
                    <p>
                        <strong>12. DISPOSICIONES GENERALES</strong>
                    </p>
                    <p>
                        12.1. Todos los derechos y obligaciones que resulten de estos Términos y
    Condiciones se regirán por las leyes de los Estados Unidos Mexicanos.
                    </p>
                    <p>
                        12.2. CLIENTE no tendrá responsabilidad en caso de que no pueda cumplir con
    estos Términos y Condiciones debido a casos de fuerza mayor o caso
    fortuito, u otro cambio en la ley o por cualesquiera otras razones fuera de
    su control.
                    </p>
                    <p>
                        12.3. CLIENTE podrá en cualquier momento, por así convenir a sus intereses,
    suspender, modificar o cancelar el Programa de Lealtad, premios,
    beneficios, reglas y contenidos, en tal razón se dará aviso a través de la
página del sitio del programa    <a href="http://www.moshimoshi.mx/">www.moshimoshi.mx</a>
                    </p>
                    <p>
                        12.4. CLIENTE, se reserva el derecho de revisar los saldos del puntaje
    acumulado por los Participantes, así como suspender cualquier participación
    hasta resolver satisfactoriamente cualquier discrepancia o anomalía
    observada referente a la acumulación ilícita de puntos dentro del programa.
                    </p>
                    <p>
                        12.5. El presente programa y las promociones del mismo no aplican con otras
    promociones ni descuentos.
                    </p>
                    <p>
                        12.6. El tipo de pago para acumular puntos aplica sólo en dinero en
    efectivo, tarjetas de débito y/o crédito.
                    </p>
                    <p>
                        12.7. Todos los puntos generados podrán ser utilizados cuando el
    Participante lo decida (mientras la cuenta se encuentre activa de acuerdo a
    lo considerado por el programa como periodo de actividad del programa
    “Puntos Moshi“), directamente en el restaurante participante desde el
portal de Internet    <a href="http://www.moshimoshi.mx./">www.moshimoshi.mx</a> ingresando
    correo y contraseña previamente registrados y entrando en la sección de
    “Pago con Puntos” (ingresando número de sucursal, ticket, fecha de ticket y
    monto del ticket).
                    </p>
                    <p>
                        <strong>13. Niveles dentro del programa</strong>
                    </p>
                    <p>
                        A. Nivel 1
                    </p>
                    <p>
                        1. Todas las personas inscritas al programa “Puntos Moshi” se convierten
    en clientes Rewards.
                    </p>
                    <p>
                        2. Acumularán el 10% de su cuenta en puntos, ya sea en consumo en
    restaurante, take away o servicio a domicilio.
                    </p>
                    <p>
                        3. No acumularán puntos las propinas, ni estacionamiento.
                    </p>
                    <p>
                        4. A fin de que acredite puntos, es necesario que realice el proceso
    mencionado en el numeral 6.3 de este documento.
                    </p>
                    <p>
                        5. Los puntos tienen una vigencia de un año a partir de la fecha exacta de
    su acreditación. Si no son utilizados, serán dados de baja automáticamente.
                    </p>
                    <p>
                        B. Nivel 2
                    </p>
                    <p>
                        1. Los clientes que durante un año acumulen al menos 52 visitas en su
    tarjeta Rewards, recibirán el status de Moshi Fan, el cual conservarán
    indefinidamente.
                    </p>
                    <p>
                        2. Acumularán el 20% de su cuenta en puntos, ya sea en consumo en
    restaurante, take away o servicio a domicilio.
                    </p>
                    <p>
                        3. No acumularán puntos las propinas, ni estacionamiento.
                    </p>
                    <p>
                        4. A fin de que acredite puntos, es necesario que realice el proceso
    mencionado en el numeral 6.3 de este documento.
                    </p>
                    <p>
                        5. Los puntos tienen una vigencia de un año a partir de la fecha exacta de
    su acreditación. Si no son utilizados, serán dados de baja automáticamente.
                    </p>
                    <p>
                        <strong>14. Exclusiones</strong>
                    </p>
                    <p>
                        Quedarán excluidos de la participación en el programa “Puntos Moshi”,
    personal que sea asignado y/o colabore de alguna manera en los restaurantes
    participantes.
                    </p>
                    <p>
                        <strong>LIMITACIONES DE RESPONSABILIDAD</strong>
                    </p>
                    <p>
                        El CLIENTE no será responsable por aquellas fallas que pudieran ocurrir en
    los sistemas o problemas ocasionados por el servidor, equipos de terminales
    de venta, suministro de energía, telecomunicaciones, incluidas Internet,
    líneas telefónicas, desperfectos técnicos, errores humanos, problemas de
    cualquier naturaleza, ya sea mecánica, humana o electrónica o acciones de
    terceros, que puedan perturbar el desarrollo normal de la promoción.
                    </p>
                </div>

            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</asp:Content>
<asp:Content ID="Content25" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
