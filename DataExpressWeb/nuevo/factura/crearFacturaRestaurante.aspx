<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="crearFacturaRestaurante.aspx.cs" Inherits="DataExpressWeb.CrearFacturaRestauranteMx" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/imagenes/loading.gif" />--%>
    <style type="text/css">
        .auto-style1 {
            font-size: large;
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
        function changeText(html, id) {
            $(id).val(html);
            return false;
        }
        function wizardPrev() {
        }
        function wizardFirst() {
        }
        function wizardNext() {
            var control = true;
            var currentIndex = $('#rootwizard').bootstrapWizard('currentIndex');
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
        function ValidateCheckBoxList(sender, args) {
            var checkBoxList = document.getElementById("<%=ddlMetodoPago.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
            }
            args.IsValid = isValid;
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
                            <a href="#tab1" data-toggle="tab" id="linkTab1" runat="server">Emisor</a>
                        </li>
                        <li>
                            <a href="#tab2" data-toggle="tab" id="linkTab2" runat="server">Receptor</a>
                        </li>
                        <li>
                            <a href="#tab3" data-toggle="tab" id="linkTab3" runat="server">INE</a>
                        </li>
                        <li>
                            <a href="#tab4" data-toggle="tab" id="linkTab4" runat="server">Documento</a>
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
                <asp:UpdatePanel ID="updTab1" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationEmisor" mensaje="Verifique que los datos del emisor sean correctos" habilitado=""></div>
                        <div class="validationGroup" value="validationEmisorDom" mensaje="Verifique que los datos del domicilio fiscal sean correctos" id="validationEmisorDom" runat="server"></div>
                        <div class="validationGroup" value="validationEmisorDomExp" mensaje="Verifique que los datos del domicilio de expedición sean correctos" id="validationEmisorDomExp" runat="server"></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Información del Emisor</span></strong>
                        </div>
                        <br />
                        <div class="rowsSpaced">
                            <div class="row">
                                <div class="col-md-1">
                                    RFC:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRfcEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbRfcEmi" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRuc_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRucEmi" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcEmi" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-md-2">
                                    RAZÓN SOCIAL:
       
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbNomEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="tbNomEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="headingOne">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">DIRECCIÓN FISCAL
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-1"></div>
                                                    <div align="left" class="col-md-11">
                                                        <asp:CheckBox ID="chkDomEmi" runat="server" Text="HABILITAR" AutoPostBack="true" OnCheckedChanged="chkDomEmi_CheckedChanged" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        CALLE:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbCalleEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbCalleEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">NO. EXT.:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbNoExtEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">NO. INT.:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbNoIntEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">COLONIA:</div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbColoniaEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        MUNICIPIO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="tbMunicipioEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbMunicipioEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        ESTADO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="tbEstadoEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="tbEstadoEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">
                                                        C.P.:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="tbCpEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbCpEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" ReadOnly="True" MaxLength="5"></asp:TextBox>
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpEmi" Mask="99999" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        PAÍS:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="tbPaisEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="tbPaisEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="headingTwo">
                                            <h4 class="panel-title">
                                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">DIRECCIÓN DE EXPEDICIÓN
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-1"></div>
                                                    <div align="left" class="col-md-11">
                                                        <asp:CheckBox ID="cbEmiExp" runat="server" Text="HABILITAR" AutoPostBack="true" OnCheckedChanged="cbEmiExp_CheckedChanged" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1"></div>
                                                    <div align="left" class="col-md-11">
                                                        <asp:CheckBox ID="cbUsarDirecEmisor" runat="server" Text="USAR DIRECCIÓN DEL EMISOR" AutoPostBack="true" OnCheckedChanged="cbUsarDirecEmisor_CheckedChanged" Enabled="False" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        CALLE:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="tbCalleExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbCalleExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">NO. EXT.:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbNoExtExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">NO. INT.:</div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="tbNoIntExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">COLONIA:</div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbColoniaExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        MUNICIPIO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="tbMunicipioExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbMunicipioExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        ESTADO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="tbEstadoExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="tbEstadoExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        C.P.:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="tbCpExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbCpExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" ReadOnly="True" MaxLength="5"></asp:TextBox>
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpExp" Mask="99999" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        PAÍS:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="tbPaisExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="tbPaisExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="headingThree">
                                            <h4 class="panel-title">
                                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">DATOS ADICIONALES
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-1">CURP:</div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbCURPE" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CURP *"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">E-MAIL:</div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="tbMailEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="E-MAIL *"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1">TELÉFONO:</div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbTelEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">
                                                        RÉGIMEN FISCAL:
       
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbRegimenFiscal" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="tbRegimenFiscal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RÉGIMEN FISCAL *"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" SelectCommand="SELECT DISTINCT Descripcion, codigo FROM Cat_Catalogo1_C WHERE (tipo = 'EmpresaTipo') AND (codigo = no)"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataPtoEmision" runat="server" SelectCommand="SELECT noEmpleado, idEmpleado FROM Cat_Empleados WHERE (idEmpleado = @idEmpleado)">
                            <SelectParameters>
                                <asp:SessionParameter Name="idEmpleado" SessionField="idUser" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSucursal" runat="server" SelectCommand="SELECT clave, sucursal FROM Cat_Sucursales WHERE (idSucursal = @idSucursal)">
                            <SelectParameters>
                                <asp:SessionParameter Name="idSucursal" SessionField="sucursalUser" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:SqlDataSource ID="SqlDataSeries" runat="server" SelectCommand="SELECT s.* FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipoDoc = '01' AND s.tipo = 2 AND m.idEmpleado = @idUser">
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div class="tab-pane" id="tab2">
                <asp:UpdatePanel ID="updTab2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationReceptor" mensaje="Verifique que los datos del receptor sean correctos" habilitado=""></div>
                        <div class="validationGroup" value="validationReceptorDom" mensaje="Verifique que los datos del domicilio fiscal sean correctos" id="validationReceptorDom" runat="server"></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Información del Receptor</span></strong>
                        </div>
                        <br />
                        <div class="rowsSpaced" align="center">
                            <div class="row">
                                <div class="col-md-1">
                                    RFC:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="tbRfcRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbRfcRec" runat="server" CssClass="form-control upper" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRfcRec_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="tbRfcRec_AutoCompleteExtender" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcRec" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-md-1">
                                    RAZÓN SOCIAL:

       

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tbRazonSocialRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="tbRazonSocialRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:LinkButton ID="bBuscaReceptor" runat="server" CssClass="btn btn-primary btn-sm" data-target="#ModuloBC" data-toggle="modal" Text="Buscar Receptor"></asp:LinkButton>
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
                                <div align="center">
                                    <div class="panel-group" id="accordion1" role="tablist" aria-multiselectable="true">
                                        <div class="panel panel-default">
                                            <div class="panel-heading" role="tab" id="heading1One">
                                                <h4 class="panel-title">
                                                    <a role="button" data-toggle="collapse" data-parent="#accordion1" href="#collapse1One" aria-expanded="true" aria-controls="collapse1One">DOMICILIO
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapse1One" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading1One">
                                                <div class="panel-body">
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
                                                            CALLE:

                                   

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tbCalleRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbCalleRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">NO. EXT.:</div>
                                                        <div class="col-md-2">
                                                            <asp:TextBox ID="tbNoExtRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">NO. INT.:</div>
                                                        <div class="col-md-2">
                                                            <asp:TextBox ID="tbNoIntRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">COLONIA:</div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbColoniaRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">
                                                            MUNICIPIO:
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMunicipioRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">
                                                            ESTADO:

                                   

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbEstadoRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-7">
                                                            <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">
                                                            C.P.:

                                   

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="tbCpRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbCpRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" ReadOnly="True" MaxLength="5"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpRec" Mask="99999" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">
                                                            PAÍS:

                                   

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="tbPaisRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-7">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="tbPaisRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *" ReadOnly="True"></asp:TextBox>
                                                                <div class="input-group-btn">
                                                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" id="btnPaisRec" runat="server" disabled="disabled"><span class="caret"></span></button>
                                                                    <ul class="dropdown-menu dropdown-menu-right" role="menu" runat="server" id="ulPaisRec" style="height: 200px; overflow: hidden; overflow-y: scroll;">
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading" role="tab" id="heading1Three">
                                                <h4 class="panel-title">
                                                    <a role="button" data-toggle="collapse" data-parent="#accordion1" href="#collapse1Three" aria-expanded="true" aria-controls="collapse1Three">DATOS ADICIONALES
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapse1Three" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading1Three">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-2"></div>
                                                        <div class="col-md-1">CURP:</div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbCURPR" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CURP *"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">E-MAIL:</div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbMailRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="E-MAIL *"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-2"></div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-2"></div>
                                                        <div class="col-md-1">TEL. 1:</div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbTelRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">TEL. 2:</div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbTelRec2" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-2"></div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="panel panel-default" id="divContactos" runat="server">
                                            <div class="panel-heading" role="tab" id="heading1Two">
                                                <h4 class="panel-title">
                                                    <a role="button" data-toggle="collapse" data-parent="#accordion1" href="#collapse1Two" aria-expanded="true" aria-controls="collapse1Two">CONTACTOS
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapse1Two" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading1Two">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-2"></div>
                                                        <div class="col-md-1">NOMBRE:</div>
                                                        <div class="col-md-7">
                                                            <asp:TextBox ID="tbNomContRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NOMBRE *"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-2"></div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-2"></div>
                                                        <div class="col-md-1">TEL. 1:</div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbTelContRec1" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">TEL. 2:</div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbTelContRec2" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-2"></div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">PUESTO:</div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbPuestoContRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PUESTO *"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">E-MAIL:</div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbMailContRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="E-MAIL *"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:LinkButton ID="bAgregarContacto" runat="server" CssClass="btn btn-primary" OnClick="bAgregarContacto_Click" Text="Agregar Contacto"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <asp:GridView ID="gvContactos" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvContactos_PageIndexChanged" OnSelectedIndexChanged="gvContactos_SelectedIndexChanged" OnPageIndexChanging="gvContactos_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvContactos_PreRender" GridLines="None" DataSourceID="SqlDataContactos" DataKeyNames="idContactoTemp">
                                                        <Columns>
                                                            <asp:BoundField DataField="nombre" HeaderText="NOMBRE">
                                                                <ItemStyle Width="30%"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="puesto" HeaderText="PUESTO">
                                                                <ItemStyle Width="30%"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="telefono1" HeaderText="TELEFONO1">
                                                                <ItemStyle Width="10%"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="telefono2" HeaderText="TELEFONO2">
                                                                <ItemStyle Width="10%"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="correo" HeaderText="CORREO">
                                                                <ItemStyle Width="20%"></ItemStyle>
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
                                                    <asp:SqlDataSource ID="SqlDataContactos" runat="server" SelectCommand="SELECT idContactoTemp, nombre, puesto, telefono1, telefono2, correo FROM Cat_Mx_Contactos_Temp WHERE (id_Empleado = @idUser AND SessionId=@SessionId)" DeleteCommand="DELETE FROM Cat_Mx_Contactos_Temp WHERE (idContactoTemp = @idContactoTemp)">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                                            <asp:Parameter Name="SessionId" DefaultValue="" />
                                                        </SelectParameters>
                                                        <DeleteParameters>
                                                            <asp:Parameter Name="idContactoTemp" />
                                                        </DeleteParameters>
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
            <%-----------------------------Panel INE-------------------------------------------------%>
            <div class="tab-pane" id="tab3">
                <asp:UpdatePanel ID="updTab3" runat="server">
                    <ContentTemplate>
                        <div align="center">
                            <strong><span class="style3">Complemento INE</span></strong>
                        </div>
                        <br />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%-----------------------------Fin Panel INE-------------------------------------------------%>
            <div class="tab-pane" id="tab4" style="max-height: 450px; overflow-y: scroll; overflow-x: hidden;">
                <asp:UpdatePanel ID="updTab4" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationDocumento" mensaje="Verifique que los datos del documento sean correctos" habilitado=""></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Información del Documento</span></strong>
                        </div>
                        <br />
                        <div class="rowsSpaced" align="center">
                            <div class="row">
                                <div class="col-md-2">AMBIENTE:</div>
                                <div class="col-md-4">
                                    <asp:HiddenField ID="hfambiente" runat="server" />
                                    <asp:TextBox ID="tbAmbiente" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-1">SERIE:</div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlSerie" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSeries" DataTextField="serie" DataValueField="idSerie" AutoPostBack="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    <% if (Session["CfdiVersion"].ToString().Equals("3.3"))
                                        { %>
                                            MÉTODO DE PAGO:
                                            <% }
                                                else
                                                { %>
                                            FORMA DE PAGO:
                                            <% } %>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="tbFormaPago" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento" Enabled='<%# !Session["CfdiVersion"].ToString().Equals("3.3") %>'></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator52" runat="server" ControlToValidate="ddlFormaPago" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento" Enabled='<%# Session["CfdiVersion"].ToString().Equals("3.3") %>' InitialValue=""></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <div runat="server" id="divTbFormaPago" style="display: none;">
                                        <div class="input-group">
                                            <asp:TextBox ID="tbFormaPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder=""></asp:TextBox>
                                            <div class="input-group-btn">
                                                <button type="button" id="btnFormaPago" runat="server" class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
                                                <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                                    <li>
                                                        <a href="#" onclick="return changeText($(this).html(), '#<%= tbFormaPago.ClientID %>');">PAGO EN UNA SOLA EXHIBICIÓN</a>
                                                        <a href="#" onclick="return changeText($(this).html(), '#<%= tbFormaPago.ClientID %>');">PARCIALIDAD X DE X</a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="form-control" Style="text-align: center" AutoPostBack="true" OnSelectedIndexChanged="ddlFormaPago_SelectedIndexChanged">
                                        <asp:ListItem Value="" Text="SELECCIONE"></asp:ListItem>
                                        <asp:ListItem Value="PUE" Text="PAGO EN UNA SOLA EXHIBICIÓN"></asp:ListItem>
                                        <asp:ListItem Value="PPD" Text="PAGO EN PARCIALIDADES O DIFERIDO"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1">MONEDA:</div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="form-control" Style="text-align: center" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged">
                                        <%--<asp:ListItem Value="MXN" Text="Pesos Mexicanos (MXN)"></asp:ListItem>
                                                <asp:ListItem Value="USD" Text="Dólares Americanos (USD)"></asp:ListItem>
                                                <asp:ListItem Value="EUR" Text="Euros (EUR)"></asp:ListItem>
                                                <asp:ListItem Value="PEN" Text="Sol Peruano (PEN)"></asp:ListItem>
                                                <asp:ListItem Value="PAB" Text="Balboa Panameño (PAB)"></asp:ListItem>
                                                <asp:ListItem Value="COP" Text="Peso Colombiano (COP)"></asp:ListItem>
                                                <asp:ListItem Value="CRC" Text="Colón Costarricense (CRC)"></asp:ListItem>
                                                <asp:ListItem Value="SVC" Text="Colónde El Salvador (SVC)"></asp:ListItem>
                                                <asp:ListItem Value="HTG" Text="Gourde (HTG)"></asp:ListItem>
                                                <asp:ListItem Value="BOB" Text="Boliviano (BOB)"></asp:ListItem>
                                                <asp:ListItem Value="GTQ" Text="Quetzal (GTQ)"></asp:ListItem>
                                                <asp:ListItem Value="BRL" Text="Real brasileño (BRL)"></asp:ListItem>
                                                <asp:ListItem Value="GBP" Text="Libra Esterlina (GBP)"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1"></div>
                            </div>
                            <div class="row" id="rowCondPago" runat="server" visible="false">
                                <div class="col-md-2">COND. DE PAGO:</div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tbCondPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CONDICIONES DE PAGO *"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">SUBTOTAL:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbSubtotal" runat="server" CssClass="form-control input-money" Style="text-align: center" placeholder="0.00 *"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbSubtotal_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbSubtotal" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="tbSubtotal" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">IVA 16 %:</div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbIva16" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">DESCUENTO:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbDescuentoP" runat="server" CssClass="form-control input-money" Style="text-align: center" placeholder="0 *"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbDescuentoP" ValidChars=",."></asp:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-1">DESCUENTO:</div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbDescuento" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0 *" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">PROPINA:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbPropina" runat="server" CssClass="form-control input-money" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbPropina" ValidChars=",."></asp:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-1">TOTAL FACT.:</div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbTotal" runat="server" CssClass="form-control input-money" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotal" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="tbTotal" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">OTROS C.:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbOtrosCargos" runat="server" CssClass="form-control input-money" Style="text-align: center" placeholder="0.00 *"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbOtrosCargos" ValidChars=",."></asp:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-1">GRAN TOTAL:</div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbGranTotal" runat="server" CssClass="form-control input-money" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbGranTotal" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbGranTotal" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnCalcularTotales" runat="server" Text="Calcular Totales" OnClick="CalcularTotales" CssClass="btn btn-primary btn-sm" Style="width: 100%" />
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">MOTIVO DESCUENTO:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbMotivoDescuento" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MOTIVO DESCUENTO *"></asp:TextBox>
                                </div>
                                <div class="col-md-1">REFER.:</div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbReferencia" runat="server" CssClass="form-control" Style="text-align: center" placeholder="REFERENCIA *"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row" id="rowTipoCambio" runat="server" visible="false">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">TIPO DE CAMBIO:</div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTipoCambio" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TIPO DE CAMBIO *" Text="1"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>

                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-4">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <h3 class="panel-title">
                                                <% if (Session["CfdiVersion"].ToString().Equals("3.3"))
                                                    { %>
                                            FORMA DE PAGO:
                                            <% }
                                                else
                                                { %>
                                            MÉTODOS DE PAGO:
                                            <% } %>
                                                <asp:CustomValidator ID="CustomValidator1" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento" ClientValidationFunction="ValidateCheckBoxList" runat="server" />
                                            </h3>
                                        </div>
                                        <div class="panel-body">
                                            <div style="max-height: 100px; overflow: auto;">
                                                <asp:DropDownList ID="ddlMetodoPago" runat="server" DataSourceID="SqlDataMetodoPago" DataTextField="descripcion" DataValueField="codigo" CssClass="form-control" Style="text-align: center" Visible='<%# Session["CfdiVersion"].ToString().Equals("3.3") %>'>
                                                </asp:DropDownList>
                                                <asp:CheckBoxList ID="chkMetodoPago" runat="server" DataSourceID="SqlDataMetodoPago" DataTextField="descripcion" DataValueField="codigo" Visible='<%# !Session["CfdiVersion"].ToString().Equals("3.3") %>'>
                                                </asp:CheckBoxList>
                                            </div>
                                            <asp:SqlDataSource ID="SqlDataMetodoPago" runat="server" SelectCommand="SELECT codigo, (codigo + ': ' + descripcion) as descripcion FROM Cat_Catalogo1_C WHERE tipo = '@tipoCatalogo';"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-12">
                                            NÚMERO(S) DE CUENTA:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbNumCtaPago" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:TextBox ID="tbNumCtaPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CTA. DE PAGO *" Text="NO IDENTIFICADO"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">CANTIDAD CON LETRA:</div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:TextBox ID="tbCantLetra" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12"></div>
                            </div>
                            <div id="DivUsoCfdi" runat="server" style="display: none;" class="row">
                                <div class="col-md-2">
                                    USO CFDI:<asp:RequiredFieldValidator ID="RequiredFieldValidator_UsoCfdi" runat="server" ControlToValidate="ddlUsoCfdi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento" InitialValue="0"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlUsoCfdi" runat="server" CssClass="form-control" Style="text-align: center" DataSourceID="SqlDataSourceUsoCfdi" DataTextField="descripcion" DataValueField="clave"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSourceUsoCfdi" runat="server" SelectCommand="SELECT '0' AS clave,'Seleccione'AS descripcion UNION SELECT clave,descripcion FROM Cat_UsoCfdi WHERE tipoPersona LIKE @tipoPersonaUsoCfdi">
                                        <SelectParameters>
                                            <asp:Parameter Name="tipoPersonaUsoCfdi" DefaultValue="%FM%" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    LUGAR DE EXPEDICIÓN:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="tbLugarExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tbLugarExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LUGAR DE EXPEDICION *"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    OBSERVACIONES:
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tbObservaciones" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <div class="row" id="divPrevisualizar" runat="server" style="display: none;">
                                <div class="col-md-12" style="text-align: left;">
                                    <asp:LinkButton ID="lbPrevisualizar" runat="server" CssClass="btn btn-default" OnClick="lbPrevisualizar_Click">Previsualizar Factura</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
                <div class="progress progress-striped active">
                    <div class="progress-bar" style="width: 100%"></div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <div class="modal fade" role="dialog" aria-labelledby="myModalLabel" id="ModuloBC">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Buscar Clientes</h4>
                </div>
                <div class="modal-body rowsSpaced ">
                    <asp:UpdatePanel ID="updClientes" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="Label12" runat="server" Text="Buscar por RFC:"></asp:Label>
                                    <asp:TextBox ID="tbRFCClienteBusqueda" runat="server" Style="text-align: center" CssClass="form-control upper"></asp:TextBox>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="Label13" runat="server" Text="Buscar por Razón Social:"></asp:Label>
                                    <asp:TextBox ID="tbRazonClienteBusqueda" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-inline">
                                <fieldset>
                                    <div class="form-group">
                                        <asp:LinkButton ID="bBuscarCliente" OnClick="bBuscarCliente_Click" runat="server" CssClass="btn btn-primary" ValidationGroup="validationRfc">Buscar</asp:LinkButton>
                                    </div>
                                    <div class="form-group">
                                        <asp:LinkButton ID="bLimpiarBusquedaCliente" OnClick="bLimpiarBusquedaCliente_Click" runat="server" CssClass="btn btn-primary">Limpia Búsqueda</asp:LinkButton>
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="chkRepetidos" runat="server" Text="Mostrar repetidos" OnCheckedChanged="chkRepetidos_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                </fieldset>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="gvRec" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" Font-Size="X-Small" GridLines="None" DataKeyNames="IDEREC" DataSourceID="SqlDataReceptores" EmptyDataText="Sin Datos." AllowPaging="true" OnPageIndexChanging="gvRec_PageIndexChanging" RowStyle-Height="5px">
                                        <Columns>
                                            <asp:BoundField DataField="RFCREC" HeaderText="RFC" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:BoundField DataField="NOMREC" HeaderText="NOMBRE" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="bUsarRecep" Text="Usar" CssClass="btn btn-primary btn-xs" runat="server" OnClick="bUsarRecep_Click" CommandArgument='<%#Eval("IDEREC") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataReceptores" runat="server" SelectCommand="SELECT IDEREC ,RFCREC ,NOMREC ,(domicilio + ' Ext.' + noExterior + ' Int.' + noInterior + '. Col. ' + colonia + '. ' + localidad + ' Mun. ' + municipio + ', ' + estado + ', ' + pais + '. C.P.: ' + codigoPostal) AS domicilio FROM Cat_Receptor ORDER BY RFCREC"></asp:SqlDataSource>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    FACTURA - CENTRO DE CONSUMO
</asp:Content>
