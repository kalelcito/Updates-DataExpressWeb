<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" EnableEventValidation="false" Inherits="DataExpressWeb.Login" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function LoginVaidation() {
            if (!Page_ClientValidate()) {
                showDivOnModalLevel('modalValidation', 4);
                return false;
            }
            return true;
        }
        function resetPass(resetUser) {
            var isReset = resetUser == undefined ? true : Boolean(resetUser);
            var selector = isReset ? ".pwd-control" : ".pwd-control:not([id='<%=tbNombre.ClientID%>'])";
            window.setTimeout("$('" + selector + "').val('');setPwdVal(1, '');setPwdVal(2, '');", 200);
        }
        function setPwdVal(index, value) {
            var id = "";
            switch (index) {
                case 1:
                    id = '<%= hfPass1.ClientID %>';
                    break;
                case 2:
                    id = '<%= hfPass2.ClientID %>';
                    break;
                default:
                    break;
            }
            $('#' + id).val(value);
        }

    </script>
    <style type="text/css">
        .tamaño {
            font-size: 17.1px;
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    Datos de Ingreso
</asp:Content>
<asp:Content ID="Content25" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div class="col-centered" style="width: 30%;">
        <div id="divEmpresa" runat="server" style="display: none">
        <%--    Empresa:<br />--%>
            <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-control" DataSourceID="SqlDataSource3" DataTextField="nombreComercial" DataValueField="RFCEMI">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource3" runat="server" SelectCommand="SELECT [nombreComercial], [RFCEMI] FROM [Cat_Emisor]"></asp:SqlDataSource>
            <br />
        </div>
        <strong>Usuario:</strong><br />
        <asp:TextBox ID="tbRfcuser" runat="server" CssClass="form-control upper" placeholder="USUARIO000000 *" Style="text-align: center" ValidationGroup="login" MaxLength="20">
        </asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbRfcuser" ErrorMessage="Requiere Usuario**" ForeColor="Red" ValidationGroup="login" Display="None">
        </asp:RequiredFieldValidator>
        <asp:Panel ID="Panel1" runat="server" DefaultButton="bSesion">
            <strong>Contraseña:</strong>
            <asp:TextBox ID="tbPass" runat="server" TextMode="Password" CssClass="form-control" placeholder="Contraseña *" Style="text-align: center" ValidationGroup="login">
            </asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPass" ErrorMessage="Requiere Contraseña" ForeColor="Red" ValidationGroup="login" Display="None">
            </asp:RequiredFieldValidator>
            <br />
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:LinkButton ID="bSesion" runat="server" CssClass="btn btn-primary btn-lg" Font-Bold="true" OnClick="bSesion_Click" Text="Ingresar" OnClientClick="return LoginVaidation();" ValidationGroup="login"></asp:LinkButton>
                    <br />
                    <br />
                    <asp:HyperLink ID="HyperLink1" CssClass="style99" runat="server" data-toggle="modal" data-target="#FiltrosC2" Style="cursor: pointer;">
                        ¿Ha olvidado la contraseña?
                        <br />
                        <asp:LinkButton ID="bNuevo" runat="server" OnClick="bNuevo_Click" Text="Nuevo Cliente/Proveedor" CausesValidation="false"></asp:LinkButton>
                    </asp:HyperLink>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <br />
    <asp:LinkButton ID="bCodigo" CssClass="btn btn-default btn-lg" runat="server" OnClick="bCodigo_Click" CausesValidation="false">
        <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>&nbsp;
        <asp:Label ID="lblCodigo" runat="server" Text="Buscar por Ticket" Font-Bold="true"></asp:Label>
    </asp:LinkButton>
    <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT [nombreComercial], [RFCEMI] FROM [Cat_Emisor]"></asp:SqlDataSource>--%>

    <div id="modalValidation" class="modal-div">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <asp:Label ID="ValidationList" runat="server"></asp:Label>
    </div>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">

    <div class="modal fade" id="FiltrosC1">
        <div class="modal-dialog" style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
            <div class="modal-content " style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
                <div class="modal-header " style="align-content: center;">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">x</button>
                    <h4 class="modal-title ">Registro de Nuevo Cliente/Proveedor</h4>
                </div>
                <div class="modal-body rowsSpaced" style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:HiddenField ID="hfPass1" runat="server" Value="" />
                            <asp:HiddenField ID="hfPass2" runat="server" Value="" />
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Tipo de Cuenta:
                                </div>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlTipoCuenta" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCuenta_SelectedIndexChanged">
                                        <asp:ListItem Value="1" Text="Cliente" />
                                        <asp:ListItem Value="2" Text="Proveedor" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    RFC:
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                        runat="server"
                                        ControlToValidate="tbRFCNuevo"
                                        ErrorMessage="*"
                                        ForeColor="Red"
                                        SetFocusOnError="True"
                                        ValidationGroup="validationRfc"
                                        ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbRFCNuevo" CssClass="form-control upper" runat="server" Style="text-align: center" AutoPostBack="true" OnTextChanged="tbRFCNuevo_TextChanged" MaxLength="13"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Usuario:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbUsername" runat="server" MaxLength="15" Style="text-align: center" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Razón Social:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbNombre" Style="text-align: center" CssClass="form-control pwd-control" runat="server" pattern="[a-z A-Z ñ Ñ 0-9 _ - / \ * ]*"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" id="rowDireccionFiscal" runat="server" style="display: none;">
                                <div class="panel-group" id="accordion1" role="tablist" aria-multiselectable="true">
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="heading1One">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse1One" aria-expanded="true" aria-controls="collapse1One">DIRECCIÓN FISCAL
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapse1One" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading1One">
                                            <div class="panel-body">
                                                <table class=" table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">
                                                    <tr align="center">
                                                        <td align="center" class="col-md-3">CALLE:</td>
                                                        <td align="center" class="col-md-9" colspan="3">
                                                            <asp:TextBox ID="tbCalleRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="center" class="col-md-3">NO. EXTERIOR:</td>
                                                        <td align="center" class="col-md-3">
                                                            <asp:TextBox ID="tbNoExtRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *"></asp:TextBox>
                                                        </td>
                                                        <td align="center" class="col-md-3">NO. INTERIOR:</td>
                                                        <td align="center" class="col-md-3">
                                                            <asp:TextBox ID="tbNoIntRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="center" class="col-md-3">COLONIA:</td>
                                                        <td align="center" class="col-md-9" colspan="3">
                                                            <asp:TextBox ID="tbColoniaRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="center" style="display: none">
                                                        <td align="center" class="col-md-3">LOCALIDAD:</td>
                                                        <td align="center" class="col-md-9" colspan="3">
                                                            <asp:TextBox ID="tbLocRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LOCALIDAD *"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="center" style="display: none">
                                                        <td align="center" class="col-md-3">REFERENCIA:</td>
                                                        <td align="center" class="col-md-9" colspan="3">
                                                            <asp:TextBox ID="tbRefRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="REFERENCIA *"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="center" class="col-md-3">MUNICIPIO:</td>
                                                        <td align="center" class="col-md-9" colspan="3">
                                                            <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="center" class="col-md-3">ESTADO:</td>
                                                        <td align="center" class="col-md-3">
                                                            <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *"></asp:TextBox>
                                                        </td>
                                                        <td align="center" class="col-md-3">PAÍS:</td>
                                                        <td align="center" class="col-md-3">
                                                            <asp:TextBox ID="tbPaisRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="center" class="col-md-3">CODIGO POSTAL:</td>
                                                        <td align="center" class="col-md-9" colspan="3">
                                                            <asp:TextBox ID="tbCpRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" MaxLength="5"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="tbCpRec_MaskedEditExtender" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpRec" Mask="99999" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Contraseña:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbContrasena" runat="server" MaxLength="15" TextMode="Password" Style="text-align: center" CssClass="form-control pwd-control" onkeyup="setPwdVal(1, $(this).val());"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Repetir Contraseña:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbRepetir" runat="server" MaxLength="15" TextMode="Password" Style="text-align: center" CssClass="form-control pwd-control" onkeyup="setPwdVal(2, $(this).val());"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    E-Mail:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbEmail" runat="server" Style="text-align: center" placeholder="ej. tu@dominio.com, pagina@dataexpress.com" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Teléfono:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbTel" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbTel" FilterMode="ValidChars" FilterType="Numbers,Custom" ValidChars="-() ." />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Registrar" ValidationGroup="validationRfc"></asp:LinkButton>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="FiltrosC2">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Reestablecer Contraseña</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <%--<div id="divEmpresa" runat="server" style="display: none">
                                <div class="row">
                                    <div class="col-md-12">Empresa:</div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:DropDownList ID="ddlEmpresaOlvido" runat="server" CssClass="form-control" DataSourceID="SqlDataSource3" DataTextField="nombreComercial" DataValueField="RFCEMI">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" SelectCommand="SELECT [nombreComercial], [RFCEMI] FROM [Cat_Emisor]"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <br />
                            </div>--%>
                            <div class="row">
                                <div class="col-md-12">
                                    Tipo de Cuenta:
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:DropDownList ID="ddlTipoCuentaOlvido" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="1" Text="Empleado" />
                                        <asp:ListItem Value="2" Text="Cliente" />
                                        <asp:ListItem Value="3" Text="Proveedor" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    RFC:<asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                        runat="server"
                                        ControlToValidate="tbRFCOlvido"
                                        ErrorMessage="*"
                                        ForeColor="Red"
                                        SetFocusOnError="True"
                                        ValidationGroup="validationRfc2"
                                        ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="tbRFCOlvido" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">Usuario:</div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="tbUserOlvido" runat="server" CssClass="form-control" placeholder="USUARIO000000 *" Style="text-align: center"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            La Contraseña se enviara a su E-Mail.
                    <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="bEnviarOlvido" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="bEnviarOlvido_Click" ValidationGroup="validationRfc2" />
                                </div>
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
</asp:Content>
