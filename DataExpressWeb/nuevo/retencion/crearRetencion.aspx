<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="crearRetencion.aspx.cs" Inherits="DataExpressWeb.CrearRetencion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .CompletionListCssClass {
            background: #fff;
            border: 1px solid #999;
            color: #000;
            float: left;
            font-size: 16px;
            margin-left: 0px;
            margin-top: -3px;
            padding: 0px 0px;
            position: absolute;
            width: 300px;
            z-index: 1;
        }

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

        .style3 {
            font-size: large;
            text-align: center;
        }

        .panel {
            background: rgba(0,0,0,0) !important;
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            $('#rootwizard').bootstrapWizard('show', 1);
        }
        function wizardPrev() {
        }
        function wizardFirst() {
        }
        function wizardNext() {
            var control = true;
            var currentIndex = $('#rootwizard').bootstrapWizard('currentIndex');
            switch (currentIndex) {
                case 2:
                    var countConc = $("[id*=gvImpuestos] td").closest("tr").length;
                    control = Boolean(countConc > 0);
                    if (!control) {
                        alertBootBox("Debe agregar al menos un Impuesto", 4);
                    }
                    break;
                default:
                    break;
            }
            if (!control) {
                $('#rootwizard').bootstrapWizard('show', currentIndex - 1);
                //$('#rootwizard').bootstrapWizard('back');
            }
            return control;
        }
        function wizardLast() {
        }
        function wizardFinish() {
            return validateFinish('<%= bFinishButton.ClientID %>');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div id="rootwizard">
        <div class="navbar">
            <div class="navbar-inner">
                <div class="container">
                    <ul>
                        <li>
                            <a href="#tab1" data-toggle="tab">Emisor</a>
                        </li>
                        <li>
                            <a href="#tab2" data-toggle="tab">Receptor</a>
                        </li>
                        <li>
                            <a href="#tab3" data-toggle="tab">Impuestos y Totales</a>
                        </li>
                        <li>
                            <a href="#tab4" data-toggle="tab">Pagos a Extranjeros</a>
                        </li>
                        <li>
                            <a href="#tab5" data-toggle="tab">Dividendos</a>
                        </li>
                        <li>
                            <a href="#tab6" data-toggle="tab">Documento</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="bar" class="progress progress-striped active">
            <div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>
        </div>
        <div class="tab-content">
            <div class="tab-pane" id="tab1">
                <asp:UpdatePanel ID="updtab1" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationEmisor" mensaje="Verifique que los datos del emisor sean correctos" habilitado=""></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Información del Emisor</span></strong>
                        </div>
                        <br />
                        <table class=" table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">
                            <tr align="center">
                                <td align="center" class="col-md-3">RFC:

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRfcEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                </td>
                                <td align="center" class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbRfcEmi" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRfcEmi_TextChanged" ReadOnly="true"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRucEmi" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcEmi" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" class="col-md-3">RAZÓN SOCIAL:

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbNomEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                </td>
                                <td align="center" class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbNomEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" class="col-md-3">CURP:</td>
                                <td align="center" class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbCURPE" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CURP *"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab2">
                <asp:UpdatePanel ID="updtab2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationReceptor" mensaje="Verifique que los datos del receptor sean correctos" habilitado=""></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Información del Receptor</span></strong>
                        </div>
                        <br />
                        <table class=" table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">
                            <tr align="center">
                                <td align="center" class="col-md-3"></td>
                                <td align="left" class="col-md-9" colspan="2">
                                    <asp:CheckBox ID="chkRecNacional" runat="server" Text="RECEPTOR NACIONAL" Checked="true" AutoPostBack="true" OnCheckedChanged="chkRecNacional_CheckedChanged" />
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" class="col-md-3">
                                    <asp:Label ID="lblRFC" runat="server" Text="RFC:"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbRfcRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                </td>
                                <td align="center" class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbRfcRec" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRfcRec_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcRec" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" class="col-md-3">RAZÓN SOCIAL:

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNomRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                </td>
                                <td align="center" class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbNomRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" class="col-md-3">CURP:</td>
                                <td align="center" class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbCURPR" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CURP *"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab3">
                <asp:UpdatePanel ID="updtab3" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationTotales" mensaje="Verifique que los totales sean correctos" habilitado=""></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Impuestos Retenidos y Totales</span></strong>
                        </div>
                        <br />
                        <div align="center">
                            <div class="row">
                                <div class="col-md-8"></div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-1">
                                    IMPUESTO:
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlImpuesto" runat="server" Style="text-align: center" CssClass="form-control">
                                        <asp:ListItem Selected="True" Value=""></asp:ListItem>
                                        <asp:ListItem Value="01">ISR</asp:ListItem>
                                        <asp:ListItem Value="02">IVA</asp:ListItem>
                                        <asp:ListItem Value="03">IEPS</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1" align="justified">
                                    <asp:Label ID="lbl_cantidad" runat="server" Text="BASE:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbBase" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbBase" ValidChars=",."></asp:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-1" align="justified">
                                    <asp:Label ID="Label1" runat="server" Text="IMPORTE:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbImporte" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbImporte" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbImporte" Display="None" ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationImp"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1" align="justified">
                                    <asp:Label ID="lbl_precio" runat="server" Text="PAGO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlTipoPago" runat="server" Style="text-align: center" CssClass="form-control">
                                        <asp:ListItem Value="1" Selected="True">Pago definitivo</asp:ListItem>
                                        <asp:ListItem Value="2">Pago provisional</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <asp:LinkButton ID="bAgregarImpuesto" runat="server" CssClass="btn btn-primary" OnClick="bAgregarImpuesto_Click" Text="Agregar" ValidationGroup="validationImp"></asp:LinkButton>
                            <br />
                            <br />
                            <asp:GridView ID="gvImpuestos" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvImpuestos_PageIndexChanged" OnSelectedIndexChanged="gvImpuestos_SelectedIndexChanged" OnPageIndexChanging="gvImpuestos_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvImpuestos_PreRender" GridLines="None" DataSourceID="SqlDataImpTemp" DataKeyNames="idImpTemp">
                                <Columns>
                                    <asp:BoundField DataField="impuesto" HeaderText="IMPUESTO" ItemStyle-Width="40%">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="base" HeaderText="BASE" ItemStyle-Width="20%">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="importe" HeaderText="IMPORTE" ItemStyle-Width="10%">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pago" HeaderText="PAGO" ItemStyle-Width="20%">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                            </asp:GridView>
                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-2" align="justified">
                                    <asp:Label ID="Label2" runat="server" Text="TOTAL OPERACIÓN:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTotOperacion" runat="server" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotOperacion" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbTotOperacion" Display="None" ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationTotales"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2" align="justified">
                                    <asp:Label ID="lbl_descuento" runat="server" Text="TOTAL GRAVADO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTotGrav" runat="server" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotGrav" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbTotGrav" Display="None" ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationTotales"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-2" align="justified">
                                    <asp:Label ID="lbl_iva" runat="server" Text="TOTAL EXENTO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTotExent" runat="server" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotExent" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbTotExent" Display="None" ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationTotales"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2" align="justified">
                                    <asp:Label ID="Label3" runat="server" Text="TOTAL RETENIDO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTotRet" runat="server" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotRet" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbTotRet" Display="None" ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationTotales"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                        </div>
                        <asp:SqlDataSource ID="SqlDataImpTemp" runat="server" SelectCommand="SELECT idImpTemp, impuestoNombre AS impuesto, base, importe, tipoPago AS pago FROM DAT_MX_ImpuestosRetencionesTemp WHERE (id_Empleado = @idUser)" DeleteCommand="DELETE FROM DAT_MX_ImpuestosRetencionesTemp WHERE (idImpTemp = @idImpTemp)">
                            <SelectParameters>
                                <asp:SessionParameter Name="idUser" SessionField="idUser" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="idImpTemp" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab4">
                <asp:UpdatePanel ID="updtab4" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationB" mensaje="Verifique que los datos del beneficiario sean correctos" id="validationB" runat="server"></div>
                        <div class="validationGroup" value="validationNB" mensaje="Verifique que los datos del no beneficiario sean correctos" id="validationNB" runat="server"></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Complemento de Pagos a Extranjeros</span></strong>
                        </div>
                        <br />
                        <div align="center">
                            <div class="row">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:CheckBox ID="chkPagos" runat="server" Text="HABILITAR COMPLEMENTO" AutoPostBack="true" OnCheckedChanged="chkPagos_CheckedChanged" Checked="false" />
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                            <br />
                            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                <div class="panel panel-default">
                                    <div class="panel-heading" role="tab" id="headingOne">
                                        <h4 class="panel-title">
                                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">BENEFICIARIO
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                        <div class="panel-body">
                                            <div align="center">
                                                <div class="row">
                                                    <div class="col-md-3"></div>
                                                    <div class="col-md-3" align="left">
                                                        <asp:CheckBox ID="chkBeneficiario" runat="server" OnCheckedChanged="chkBeneficiario_CheckedChanged" AutoPostBack="True" Text="BENEFICIARIO" Checked="false" Enabled="false" />
                                                    </div>
                                                    <div class="col-md-6"></div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3">RFC:</div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbRFCB" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" MaxLength="20"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="tbRFCB" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationB" Enabled="false"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">CURP:</div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbCURPB" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" MaxLength="20"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tbCURPB" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationB" Enabled="false"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3">RAZÓN SOCIAL:</div>
                                                    <div class="col-md-9">
                                                        <asp:TextBox ID="tbRazonB" runat="server" Style="text-align: center;" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="tbRazonB" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationB" Enabled="false"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3">CONCEPTO DE PAGO:</div>
                                                    <div class="col-md-9">
                                                        <asp:DropDownList ID="ddlConceptoPagoB" runat="server" Style="text-align: center" CssClass="form-control" Enabled="false">
                                                            <asp:ListItem Value="1" Text="Artistas, deportistas y espectáculos públicos"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Otras personas físicas" Selected="true"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="Persona moral"></asp:ListItem>
                                                            <asp:ListItem Value="4" Text="Fideicomiso"></asp:ListItem>
                                                            <asp:ListItem Value="5" Text="Asociación en participación"></asp:ListItem>
                                                            <asp:ListItem Value="6" Text="Organizaciones Internacionales o de gobierno"></asp:ListItem>
                                                            <asp:ListItem Value="7" Text="Organizaciones exentas"></asp:ListItem>
                                                            <asp:ListItem Value="8" Text="Agentes pagadores"></asp:ListItem>
                                                            <asp:ListItem Value="9" Text="Otros"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">DESCRIPCIÓN DE CONCEPTO:</div>
                                                    <div class="col-md-9">
                                                        <asp:TextBox ID="tbDescConceptoB" runat="server" Style="text-align: center;" CssClass="form-control" ReadOnly="True" placeholder="DESCRIPCIÓN *"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbDescConceptoB" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationB"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading" role="tab" id="headingTwo">
                                        <h4 class="panel-title">
                                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">NO BENEFICIARIO
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                        <div class="panel-body">
                                            <div align="center">
                                                <div class="row">
                                                    <div class="col-md-3"></div>
                                                    <div class="col-md-3" align="left">
                                                        <asp:CheckBox ID="chkNoBeneficiario" runat="server" AutoPostBack="True" OnCheckedChanged="chkNoBeneficiario_CheckedChanged" Text="NO BENEFICIARIO" Checked="false" Enabled="false" />
                                                    </div>
                                                    <div class="col-md-6"></div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3">PAÍS DE RESID. PARA EFECTOS FISCALES:</div>
                                                    <div class="col-md-9">
                                                        <asp:DropDownList ID="ddlPaisExt" runat="server" Style="text-align: center" CssClass="form-control" Enabled="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3">CONCEPTO DE PAGO:</div>
                                                    <div class="col-md-9">
                                                        <asp:DropDownList ID="ddlConceptoPagoNB" runat="server" Style="text-align: center" CssClass="form-control" Enabled="false">
                                                            <asp:ListItem Value="1" Text="Artistas, deportistas y espectáculos públicos"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Otras personas físicas" Selected="true"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="Persona moral"></asp:ListItem>
                                                            <asp:ListItem Value="4" Text="Fideicomiso"></asp:ListItem>
                                                            <asp:ListItem Value="5" Text="Asociación en participación"></asp:ListItem>
                                                            <asp:ListItem Value="6" Text="Organizaciones Internacionales o de gobierno"></asp:ListItem>
                                                            <asp:ListItem Value="7" Text="Organizaciones exentas"></asp:ListItem>
                                                            <asp:ListItem Value="8" Text="Agentes pagadores"></asp:ListItem>
                                                            <asp:ListItem Value="9" Text="Otros"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">DESCRIPCIÓN DE CONCEPTO:</div>
                                                    <div class="col-md-9">
                                                        <asp:TextBox ID="tbDescConceptoNB" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" placeholder="DESCRIPCIÓN *"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tbDescConceptoNB" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationNB"></asp:RequiredFieldValidator>
                                                    </div>
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
            <div class="tab-pane" id="tab5">
                <asp:UpdatePanel ID="updtab5" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationDivid" mensaje="Verifique que los datos de los dividendos sean correctos" id="validationDivid" runat="server"></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Complemento de Dividendos</span></strong>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4"></div>
                            <div class="col-md-4">
                                <asp:CheckBox ID="chkDividOutil" runat="server" Text="HABILITAR COMPLEMENTO" AutoPostBack="true" OnCheckedChanged="chkDividOutil_CheckedChanged" Checked="false" />
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-3">TIPO DE DIVIDENDO:</div>
                            <div class="col-md-9">
                                <asp:DropDownList ID="ddlTipoDivOUtil" runat="server" Style="text-align: center" CssClass="form-control" Enabled="false">
                                    <asp:ListItem Value="01" Text="Proviene de CUFIN"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="No proviene de CUFIN"></asp:ListItem>
                                    <asp:ListItem Value="03" Text="Reembolso o reducción de capital"></asp:ListItem>
                                    <asp:ListItem Value="04" Text="Liquidación de la persona moral"></asp:ListItem>
                                    <asp:ListItem Value="05" Text="CUFINRE"></asp:ListItem>
                                    <asp:ListItem Value="06" Text="Proviene de CUFIN al 31 
de diciembre 2013">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-3">IMPORTE EN TERRITORIO NAL.:</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="tbMontISRAcredRetMexico" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" placeholder="0.00 *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMontISRAcredRetMexico" CssClass="style44" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDivid" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMontISRAcredRetMexico" ValidChars=",."></asp:FilteredTextBoxExtender>
                            </div>
                            <div class="col-md-3">IMPORTE EN TERRITORIO EXT.:</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="tbMontISRAcredRetExtranjero" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" placeholder="0.00 *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbMontISRAcredRetExtranjero" CssClass="style44" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDivid" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMontISRAcredRetExtranjero" ValidChars=",."></asp:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">MONTO EN TERRITORIO EXT.:</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="tbMontRetExtDivExt" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" placeholder="0.00 *"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMontRetExtDivExt" ValidChars=",."></asp:FilteredTextBoxExtender>
                            </div>
                            <div class="col-md-3">TIPO DE SOCIEDAD</div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlTipoSocDistrDiv" runat="server" Style="text-align: center" CssClass="form-control" Enabled="false">
                                    <asp:ListItem Value="Sociedad Nacional" Text="Sociedad Nacional"></asp:ListItem>
                                    <asp:ListItem Value="Sociedad Extranjera" Text="Sociedad Extranjera"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">MONTO DEL ISR ACREDITABLE NAL.:</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="tbMontISRAcredNal" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" placeholder="0.00 *"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMontISRAcredNal" ValidChars=",."></asp:FilteredTextBoxExtender>
                            </div>
                            <div class="col-md-3">MONTO DEL DIV. ACUMULABLE NAL.:</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="tbMontDivAcumNal" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" placeholder="0.00 *"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMontDivAcumNal" ValidChars=",."></asp:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">MONTO DEL DIV. ACUMULABLE EXT.:</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="tbMontDivAcumExt" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" placeholder="0.00 *"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMontDivAcumExt" ValidChars=",."></asp:FilteredTextBoxExtender>
                            </div>
                            <div class="col-md-3">REMANENTE:</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="tbProporcionRem" runat="server" CssClass="form-control" Style="text-align: center;" ReadOnly="True" placeholder="0.00 *"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbProporcionRem" ValidChars=",."></asp:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab6">
                <asp:UpdatePanel ID="updtab6" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationDivid" mensaje="Verifique que los datos de los dividendos sean correctos" id="Div1" runat="server"></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Información del Documento</span></strong>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">AMBIENTE:</div>
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfambiente" runat="server" />
                                <asp:TextBox ID="tbAmbiente" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2">SERIE:</div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlSerie" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSeries" DataTextField="serie" DataValueField="idSerie" AutoPostBack="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">FOLIO:</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="tbFolio" runat="server" Style="text-align: center" CssClass="form-control" placeholder="000000001" MaxLength="9"></asp:TextBox>
                            </div>
                            <div class="col-md-2"></div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">TIPO RETENCIÓN:</div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCveRet" runat="server" Style="text-align: center" CssClass="form-control">
                                    <asp:ListItem Value="01" Text="Servicios profesionales"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="Regalías por derechos de autor"></asp:ListItem>
                                    <asp:ListItem Value="03" Text="Autotransporte terrestre de carga"></asp:ListItem>
                                    <asp:ListItem Value="04" Text="Servicios prestados por comisionistas" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="05" Text="Arrendamiento"></asp:ListItem>
                                    <asp:ListItem Value="06" Text="Enajenación de acciones."></asp:ListItem>
                                    <asp:ListItem Value="07" Text="Enajenación de bienes objeto de la LIEPS, a través de mediadores, agentes, representantes, corredores, consignatarios o distribuidores"></asp:ListItem>
                                    <asp:ListItem Value="08" Text="Enajenación de bienes inmuebles consignada en escritura pública"></asp:ListItem>
                                    <asp:ListItem Value="09" Text="Enajenación de otros bienes, no consignada en escritura pública"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Adquisición de desperdicios industriales"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Adquisición de bienes consignada en escritura pública"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Adquisición de otros bienes, no consignada en escritura pública"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="Otros retiros de AFORE."></asp:ListItem>
                                    <asp:ListItem Value="14" Text="Dividendos o utilidades distribuidas."></asp:ListItem>
                                    <asp:ListItem Value="15" Text="Remanente distribuible."></asp:ListItem>
                                    <asp:ListItem Value="16" Text="Intereses."></asp:ListItem>
                                    <asp:ListItem Value="17" Text="Arrendamiento en fideicomiso."></asp:ListItem>
                                    <asp:ListItem Value="18" Text="Pagos realizados a favor de residentes en el extranjero."></asp:ListItem>
                                    <asp:ListItem Value="19" Text="Enajenación de acciones u operaciones en bolsa de valores."></asp:ListItem>
                                    <asp:ListItem Value="20" Text="Obtención de premios."></asp:ListItem>
                                    <asp:ListItem Value="21" Text="Fideicomisos que no realizan actividades empresariales."></asp:ListItem>
                                    <asp:ListItem Value="22" Text="Planes personales de retiro."></asp:ListItem>
                                    <asp:ListItem Value="23" Text="Intereses reales deducibles por créditos hipotecarios."></asp:ListItem>
                                    <asp:ListItem Value="24" Text="Operaciones Financieras Derivadas de Capital"></asp:ListItem>
                                    <asp:ListItem Value="25" Text="Otro tipo de retenciones"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">MES INICIAL</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="tbMesIni" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00 *" Style="text-align: center;" ToolTip="Descripción" MaxLength="2"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMesIni"></asp:FilteredTextBoxExtender>
                            </div>
                            <div class="col-md-2">MES FINAL</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="tbMesFin" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00 *" Style="text-align: center;" ToolTip="Descripción" MaxLength="2"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbMesFin"></asp:FilteredTextBoxExtender>
                            </div>
                            <div class="col-md-2">AÑO</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="tbEjerc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="0000 *" Style="text-align: center;" ToolTip="Descripción" MaxLength="4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbEjerc"></asp:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:SqlDataSource ID="SqlDataSeries" runat="server" SelectCommand="SELECT s.* FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipoDoc = '07' AND s.tipo = 2 AND m.idEmpleado = @idUser">
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <ul class="pager wizard">
                <li class="previous first" style="display: none;">
                    <a href="javascript:;" onclick="return wizardFirst();">Primero</a>
                </li>
                <li class="previous">
                    <a href="javascript:;" onclick="return wizardPrev();">Anterior</a>
                </li>
                <li class="next last" style="display: none;">
                    <a href="javascript:;" onclick="return wizardLast();">Último</a>
                </li>
                <li class="next">
                    <a href="javascript:;" onclick="return wizardNext();">Siguiente</a>
                </li>
                <li class="finish">
                    <a href="javascript:;" onclick="return wizardFinish();">Crear Documento</a>
                </li>
            </ul>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="bFinishButton" runat="server" OnClick="FinishButton_Click" CssClass="btn btn-primary" Style="display: none">Finish</asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel2">
        <ProgressTemplate>
            <div id="Background"></div>
            <div id="Progress">
                <h6>
                    <p style="text-align: center">
                        <b>Generando Comprobante Electronico, Espere un momento... </b>
                    </p>
                </h6>
                <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/imagenes/loading.gif" />--%>
                <div class="progress progress-striped active">
                    <div class="progress-bar" style="width: 100%"></div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    Retenciones
</asp:Content>
