<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="crearNomina.aspx.cs" Inherits="DataExpressWeb.CrearNomina" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
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
            switch (currentIndex) {
                case 3:
                    var countConc = $("[id*=gvPercepciones] td").closest("tr").length;
                    control = Boolean(countConc > 0);
                    if (!control) {
                        alertBootBox("Debe agregar al menos una Percepción", 4);
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
        function changeText(html, id) {
            $(id).val(html);
            return false;
        }
        function ddlBancos_Changed() {
            var selectedValue = $('#<% =ddlBancos.ClientID %>').val();
            if (selectedValue == "") {
                $('#<% =tbClabe.ClientID %>').val('');
            }
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
                            <a href="#tab3" data-toggle="tab" id="linkTab3" runat="server">Empleado</a>
                        </li>
                        <li>
                            <a href="#tab4" data-toggle="tab" id="linkTab4" runat="server">Percepciones/Deducciones</a>
                        </li>
                        <li>
                            <a href="#tab5" data-toggle="tab" id="linkTab5" runat="server">Incapacidades/Horas Extras</a>
                        </li>
                        <li>
                            <a href="#tab7" data-toggle="tab" id="linkTab7" runat="server">Documento</a>
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
                                                    <%--<div class="col-md-1">LOCALIDAD:</div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbLocEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LOCALIDAD *" ReadOnly="True"></asp:TextBox>
                                                    </div>--%>
                                                    <div class="col-md-1">
                                                        MUNICIPIO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="tbMunicipioEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbMunicipioEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <%--<div class="col-md-1">REFERENCIA:</div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="tbRefEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="REFERENCIA *" ReadOnly="True"></asp:TextBox>
                                                    </div>--%>
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
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpEmi" Mask="99999" />
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
                                                    <%--<div class="col-md-1">
                                                        ESTADO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="tbEstadoEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbEstadoEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="True"></asp:TextBox>
                                                    </div>--%>
                                                    <%--<div class="col-md-1">
                                                        MUNICIPIO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="tbMunicipioEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbMunicipioEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                    </div>--%>
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
                                                    <%--<div class="col-md-1">LOCALIDAD:</div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbLocExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LOCALIDAD *" ReadOnly="True"></asp:TextBox>
                                                    </div>--%>
                                                    <div class="col-md-1">
                                                        MUNICIPIO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="tbMunicipioExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="tbMunicipioExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <%--<div class="col-md-1">REFERENCIA:</div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="tbRefExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="REFERENCIA *" ReadOnly="True"></asp:TextBox>
                                                    </div>--%>
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
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender4" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpExp" Mask="99999" />
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
                                                    <%--<div class="col-md-1">
                                                        ESTADO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="tbEstadoExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbEstadoExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="True"></asp:TextBox>
                                                    </div>--%>
                                                    <%--<div class="col-md-1">
                                                        MUNICIPIO:
                                   
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="tbMunicipioExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbMunicipioExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                    </div>--%>
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
                                                <%--<div class="row">
                                                    <div class="col-md-1">GIRO:</div>
                                                    <div class="col-md-3">
                                                        <asp:DropDownList ID="ddlTEmp" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSource2" DataTextField="Descripcion" DataValueField="codigo" AutoPostBack="True" AppendDataBoundItems="True">
                                                            <asp:ListItem Text="" Value="" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>--%>
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
                        <%--<asp:SqlDataSource ID="SqlDataDocumento" runat="server"
                            SelectCommand="SELECT Descripcion, codigo, tipo FROM Cat_Catalogo1_C WHERE (tipo = 'Comprobante') AND (codigo IN ('01', '04'))"></asp:SqlDataSource>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:SqlDataSource ID="SqlDataSeries" runat="server" SelectCommand="SELECT s.* FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipoDoc = '08' AND s.tipo = 2 AND m.idEmpleado = @idUser">
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
                                                        <%--<div class="col-md-1">LOCALIDAD:</div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbLocRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LOCALIDAD *" ReadOnly="True"></asp:TextBox>
                                                        </div>--%>
                                                        <div class="col-md-1">
                                                            MUNICIPIO:
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMunicipioRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <%--<div class="col-md-1">REFERENCIA:</div>
                                                        <div class="col-md-7">
                                                            <asp:TextBox ID="tbRefRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="REFERENCIA *" ReadOnly="True"></asp:TextBox>
                                                        </div>--%>
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
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender5" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpRec" Mask="99999" />
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
                                                        <%--<div class="col-md-1">
                                                            ESTADO:

                                   

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbEstadoRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="True"></asp:TextBox>
                                                        </div>--%>
                                                        <%--<div class="col-md-1">
                                                            MUNICIPIO:

                                   

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMunicipioRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                        </div>--%>
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
            <div class="tab-pane" id="tab7">
                <asp:UpdatePanel ID="updTab6" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationDocumento" mensaje="Verifique que los datos del documento sean correctos" habilitado=""></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Información del Documento</span></strong>
                        </div>
                        <br />
                        <div class="rowsSpaced" align="center">
                            <div class="row">
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-2">SERIE:</div>
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ddlSerie" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSeries" DataTextField="serie" DataValueField="idSerie" AutoPostBack="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">AMBIENTE:</div>
                                        <div class="col-md-5">
                                            <asp:HiddenField ID="hfambiente" runat="server" />
                                            <asp:TextBox ID="tbAmbiente" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                            FORMA DE PAGO:
                       
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="tbFormaPago" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-10">
                                            <div class="input-group">
                                                <asp:TextBox ID="tbFormaPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="FORMA DE PAGO *"></asp:TextBox>
                                                <div class="input-group-btn">
                                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
                                                    <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                                        <li>
                                                            <a href="#" onclick="return changeText($(this).html(), '#<%= tbFormaPago.ClientID %>');">PAGO EN UNA SOLA EXHIBICIÓN</a>
                                                            <a href="#" onclick="return changeText($(this).html(), '#<%= tbFormaPago.ClientID %>');">PARCIALIDAD X DE X</a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">CONDICIONES DE PAGO:</div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="tbCondPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CONDICIONES DE PAGO *"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">DESCUENTO (%):</div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="tbDescuentoP" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0 *" AutoPostBack="true" OnTextChanged="tbDescuentoP_TextChanged" TextMode="Number" min="0" step="0.01"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbDescuentoP" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        </div>
                                        <div class="col-md-1">MOTIVO:</div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="tbMotivoDescuento" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MOTIVO DESCUENTO *"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">TIPO DE CAMBIO:</div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="tbTipoCambio" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TIPO DE CAMBIO *"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">MONEDA:</div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="form-control" Style="text-align: center" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged">
                                                <asp:ListItem Value="MXN" Text="Pesos Mexicanos (MXN)"></asp:ListItem>
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
                                                <asp:ListItem Value="GBP" Text="Libra Esterlina (GBP)"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <h3 class="panel-title">MÉTODO(S) DE PAGO:
                                                        <asp:CustomValidator ID="CustomValidator1" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento" ClientValidationFunction="ValidateCheckBoxList" runat="server" />
                                                    </h3>
                                                </div>
                                                <div class="panel-body">
                                                    <div style="max-height: 100px; overflow: auto;">
                                                        <asp:CheckBoxList ID="ddlMetodoPago" runat="server" DataSourceID="SqlDataMetodoPago" DataTextField="descripcion" DataValueField="codigo">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                    <asp:SqlDataSource ID="SqlDataMetodoPago" runat="server" SelectCommand="SELECT codigo, (codigo + ': ' + descripcion) as descripcion FROM Cat_Catalogo1_C WHERE tipo = 'MetodoPago';"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-7">
                                            <div class="row"></div>
                                            <div class="row"></div>
                                            <div class="row"></div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    NÚMERO(S) DE CUENTA:
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="tbNumCtaPago" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:TextBox ID="tbNumCtaPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CTA. DE PAGO *" Text="NO IDENTIFICADO"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                            EXPEDIDO EN:
                       
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="tbLugarExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="tbLugarExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LUGAR DE EXPEDICION *"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="col-md-4">SUBTOTAL:</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbSubtotal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="tbSubtotal_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbSubtotal" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="tbSubtotal" Display="None" ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-4">ISR RET.:</div>
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
                                    <%--<div class="col-md-4">PROPINA:</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbPropina" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true" TextMode="Number" min="0" step="0.01" AutoPostBack="true" OnTextChanged="CalcularTotales"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbPropina" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    </div>--%>
                                    <div class="col-md-4">DESCUENTO:</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbDescuento" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0 *" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <%--<div runat="server" id="trOtrosCargos" visible="false">
                                        <div class="col-md-4">OTROS C.:</div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="tbOtrosCargos" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" OnTextChanged="CalcularTotales" AutoPostBack="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbOtrosCargos" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>--%>
                                    <div class="col-md-4">TOTAL FAC.:</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbTotalFac" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotalFac" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbTotalFac" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-4">A PAGAR:</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbTotal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotal" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="tbTotal" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab3">
                <asp:UpdatePanel ID="updTab7" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationEmpleadoGeneral" mensaje="Verifique que los datos generales del empleado sean correctos" habilitado=""></div>
                        <div class="validationGroup" value="validationEmpleadoPagos" mensaje="Verifique que los datos de pagos del empleado sean correctos" habilitado=""></div>
                        <div align="center">
                            <strong>
                                <span class="style3">Información de Empleado</span></strong>
                        </div>
                        <br />
                        <div class="panel-group rowsSpaced" id="accordion2" role="tablist" aria-multiselectable="true">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" data-parent="#accordion2" href="#collapse1">DATOS GENERALES</a>
                                    </h4>
                                </div>
                                <div id="collapse1" class="panel-collapse collapse in" role="tabpanel">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">
                                                IDENTIFICACIÓN:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbIdEmpleado" ValidationGroup="validationEmpleadoGeneral" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-4">
                                                CURP:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbCurpEmpleado" ValidationGroup="validationEmpleadoGeneral" SetFocusOnError="true"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ValidationExpression="[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]" ControlToValidate="tbCurpEmpleado" ValidationGroup="validationEmpleadoGeneral" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                            </div>
                                            <div class="col-md-4">
                                                NO. SEG. SOCIAL:
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbIdEmpleado" runat="server" CssClass="form-control" Style="text-align: center;" placeholder="NO. O ID DE EMPLEADO *" MaxLength="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbCurpEmpleado" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="18"></asp:TextBox>
                                                <asp:MaskedEditExtender runat="server" ID="MaskedEditExtender1" TargetControlID="tbCurpEmpleado" ClearMaskOnLostFocus="false" MaskType="None" Mask="CCCCCCCCCCCCCCCCCC" Filtered="ABCDEFGHIJKLMNÑOPQRSTUVWXYZ1234567890" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbSegSocialEmpleado" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="15" placeholder="NO. DE SEGURIDAD SOCIAL"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">REGISTRO PATRONAL:</div>
                                            <div class="col-md-8">DEPARTAMENTO:</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbRegPatronalEmpleado" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="20"></asp:TextBox>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="tbDeptoEmpleado" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="100" placeholder="DEPARTAMENTO DEL EMPLEADO"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                RÉGIMEN DE CONTRATACIÓN:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="ddlRegimenEmpleado" ValidationGroup="validationEmpleadoGeneral" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <asp:DropDownList ID="ddlRegimenEmpleado" runat="server" CssClass="form-control" Style="text-align: center;">
                                                <asp:ListItem Value="0" Text="SELECCIONE" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Sueldos y salarios"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Jubilados"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Pensionados"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Asimilados a salarios, Miembros de las Sociedades Cooperativas de Producción."></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Asimilados a salarios, Integrantes de Sociedades y Asociaciones Civiles"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Asimilados a salarios, Miembros de consejos directivos, de vigilancia, consultivos, honorarios a administradores, comisarios y gerentes generales."></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Asimilados a salarios, Actividad empresarial (comisionistas)"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Asimilados a salarios, Honorarios asimilados a salarios"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="Asimilados a salarios, Ingresos acciones o títulos valor"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" data-parent="#accordion2" href="#collapse2">DATOS DE EMPRESA</a>
                                    </h4>
                                </div>
                                <div id="collapse2" class="panel-collapse collapse" role="tabpanel">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-3">INICIO REL. LABORAL:</div>
                                            <div class="col-md-3">ANTIGÜEDAD (SEMANAS):</div>
                                            <div class="col-md-3">CONTRATO:</div>
                                            <div class="col-md-3">JORNADA:</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="input-group date">
                                                    <asp:TextBox ID="tbFechaInicioRel" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="fa fa-calendar-o"></i>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbAntiguedadEmpleado" runat="server" Style="text-align: center" CssClass="form-control input-number" min="0"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="tbContratoEmpleado" runat="server" CssClass="form-control" Style="text-align: center;" placeholder="TIPO DE CONTRATO"></asp:TextBox>
                                                    <div class="input-group-btn">
                                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
                                                        <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                                            <li>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbContratoEmpleado.ClientID %>');">Base</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbContratoEmpleado.ClientID %>')">Eventual</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbContratoEmpleado.ClientID %>')">Confianza</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbContratoEmpleado.ClientID %>')">Sindicalizado</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbContratoEmpleado.ClientID %>')">A Prueba</a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="tbJornadaEmpleado" runat="server" CssClass="form-control" Style="text-align: center;" placeholder="TIPO DE JORNADA"></asp:TextBox>
                                                    <div class="input-group-btn">
                                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
                                                        <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                                            <li>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbJornadaEmpleado.ClientID %>');">Diurna</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbJornadaEmpleado.ClientID %>');">Nocturna</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbJornadaEmpleado.ClientID %>');">Mixta</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbJornadaEmpleado.ClientID %>');">Por hora</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbJornadaEmpleado.ClientID %>');">Reducida</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbJornadaEmpleado.ClientID %>');">Continuada</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbJornadaEmpleado.ClientID %>');">Partida</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbJornadaEmpleado.ClientID %>');">Por Turnos</a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-9">PUESTO:</div>
                                            <div class="col-md-3">RIESGO PUESTO:</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-9">
                                                <asp:TextBox ID="tbPuestoEmpleado" runat="server" Style="text-align: center" CssClass="form-control" placeholder="PUESTO DEL EMPLEADO"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlRiesgoPuesto" runat="server" CssClass="form-control" Style="text-align: center;">
                                                    <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Clase I"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Clase II"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Clase III"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Clase IV"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="Clase V"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" data-parent="#accordion2" href="#collapse3">DATOS DE PAGOS</a>
                                    </h4>
                                </div>
                                <div id="collapse3" class="panel-collapse collapse" role="tabpanel">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-3">
                                                FECHA DE PAGO:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbFechaPagoEmpleado" ValidationGroup="validationEmpleadoPagos" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3">
                                                FECHA INICIAL PAGO:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbFechaInicialPagoEmpleado" ValidationGroup="validationEmpleadoPagos" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3">
                                                FECHA FINAL PAGO:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbFechaFinalPagoEmpleado" ValidationGroup="validationEmpleadoPagos" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3">
                                                DÍAS PAGADOS:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDiasPagadosEmpleado" ValidationGroup="validationEmpleadoPagos" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="input-group date">
                                                    <asp:TextBox ID="tbFechaPagoEmpleado" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="fa fa-calendar-o"></i>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group date">
                                                    <asp:TextBox ID="tbFechaInicialPagoEmpleado" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="fa fa-calendar-o"></i>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group date">
                                                    <asp:TextBox ID="tbFechaFinalPagoEmpleado" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="fa fa-calendar-o"></i>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbDiasPagadosEmpleado" runat="server" Style="text-align: center" CssClass="form-control input-number" min="0"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                PERIODICIDAD DE PAGO:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator49" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbPeriodicidadEmpleado" ValidationGroup="validationEmpleadoPagos" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3">SALARIO BASE DE COTIZACIÓN:</div>
                                            <div class="col-md-3">SALARIO DIARIO INTEGRADO:</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:TextBox ID="tbPeriodicidadEmpleado" runat="server" CssClass="form-control" Style="text-align: center;" placeholder="TIPO DE JORNADA"></asp:TextBox>
                                                    <div class="input-group-btn">
                                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
                                                        <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                                            <li>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbPeriodicidadEmpleado.ClientID %>');">Diario</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbPeriodicidadEmpleado.ClientID %>');">Semanal</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbPeriodicidadEmpleado.ClientID %>');">Quincenal</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbPeriodicidadEmpleado.ClientID %>');">Catorcenal Mensual</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbPeriodicidadEmpleado.ClientID %>');">Bimestral</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbPeriodicidadEmpleado.ClientID %>');">Unidad de Obra</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbPeriodicidadEmpleado.ClientID %>');">Comisión</a>
                                                                <a href="#" onclick="return changeText($(this).html(), '#<%= tbPeriodicidadEmpleado.ClientID %>');">Precio Alzado</a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbSalarioBaseEmpleado" runat="server" Style="text-align: center" CssClass="form-control input-money" min="0" Text="0.00"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbSalarioDiarioEmpleado" runat="server" Style="text-align: center" CssClass="form-control input-money" min="0" Text="0.00"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" data-parent="#accordion2" href="#collapse4">DATOS BANCARIOS</a>
                                    </h4>
                                </div>
                                <div id="collapse4" class="panel-collapse collapse" role="tabpanel">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-2"></div>
                                            <div class="col-md-5">BANCO:</div>
                                            <div class="col-md-3">
                                                CLABE:
                                            </div>
                                            <div class="col-md-2"></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2"></div>
                                            <div class="col-md-5">
                                                <asp:DropDownList ID="ddlBancos" runat="server" Style="text-align: center" CssClass="form-control" onchange="">
                                                    <asp:ListItem Value="" Selected="True" Text="SELECCIONE"></asp:ListItem>
                                                    <asp:ListItem Value="002" Text="BANAMEX"></asp:ListItem>
                                                    <asp:ListItem Value="006" Text="BANCOMEXT"></asp:ListItem>
                                                    <asp:ListItem Value="009" Text="BANOBRAS"></asp:ListItem>
                                                    <asp:ListItem Value="012" Text="BBVA BANCOMER"></asp:ListItem>
                                                    <asp:ListItem Value="014" Text="SANTANDER"></asp:ListItem>
                                                    <asp:ListItem Value="019" Text="BANJERCITO"></asp:ListItem>
                                                    <asp:ListItem Value="021" Text="HSBC"></asp:ListItem>
                                                    <asp:ListItem Value="030" Text="BAJIO"></asp:ListItem>
                                                    <asp:ListItem Value="032" Text="IXE"></asp:ListItem>
                                                    <asp:ListItem Value="036" Text="INBURSA"></asp:ListItem>
                                                    <asp:ListItem Value="037" Text="INTERACCIONES"></asp:ListItem>
                                                    <asp:ListItem Value="042" Text="MIFEL"></asp:ListItem>
                                                    <asp:ListItem Value="044" Text="SCOTIABANK"></asp:ListItem>
                                                    <asp:ListItem Value="058" Text="BANREGIO"></asp:ListItem>
                                                    <asp:ListItem Value="059" Text="INVEX"></asp:ListItem>
                                                    <asp:ListItem Value="060" Text="BANSI"></asp:ListItem>
                                                    <asp:ListItem Value="062" Text="AFIRME"></asp:ListItem>
                                                    <asp:ListItem Value="072" Text="BANORTE"></asp:ListItem>
                                                    <asp:ListItem Value="102" Text="THE ROYAL BANK"></asp:ListItem>
                                                    <asp:ListItem Value="103" Text="AMERICAN EXPRESS"></asp:ListItem>
                                                    <asp:ListItem Value="106" Text="BAMSA"></asp:ListItem>
                                                    <asp:ListItem Value="108" Text="TOKYO"></asp:ListItem>
                                                    <asp:ListItem Value="110" Text="JP MORGAN"></asp:ListItem>
                                                    <asp:ListItem Value="112" Text="BMONEX"></asp:ListItem>
                                                    <asp:ListItem Value="113" Text="VE POR MAS"></asp:ListItem>
                                                    <asp:ListItem Value="116" Text="ING"></asp:ListItem>
                                                    <asp:ListItem Value="124" Text="DEUTSCHE"></asp:ListItem>
                                                    <asp:ListItem Value="126" Text="CREDIT SUISSE"></asp:ListItem>
                                                    <asp:ListItem Value="127" Text="AZTECA"></asp:ListItem>
                                                    <asp:ListItem Value="128" Text="AUTOFIN"></asp:ListItem>
                                                    <asp:ListItem Value="129" Text="BARCLAYS"></asp:ListItem>
                                                    <asp:ListItem Value="130" Text="COMPARTAMOS"></asp:ListItem>
                                                    <asp:ListItem Value="131" Text="BANCO FAMSA"></asp:ListItem>
                                                    <asp:ListItem Value="132" Text="BMULTIVA"></asp:ListItem>
                                                    <asp:ListItem Value="133" Text="ACTINVER"></asp:ListItem>
                                                    <asp:ListItem Value="134" Text="WAL-MART"></asp:ListItem>
                                                    <asp:ListItem Value="135" Text="NAFIN"></asp:ListItem>
                                                    <asp:ListItem Value="136" Text="INTERBANCO"></asp:ListItem>
                                                    <asp:ListItem Value="137" Text="BANCOPPEL"></asp:ListItem>
                                                    <asp:ListItem Value="138" Text="ABC CAPITAL"></asp:ListItem>
                                                    <asp:ListItem Value="139" Text="UBS BANK"></asp:ListItem>
                                                    <asp:ListItem Value="140" Text="CONSUBANCO"></asp:ListItem>
                                                    <asp:ListItem Value="141" Text="VOLKSWAGEN"></asp:ListItem>
                                                    <asp:ListItem Value="143" Text="CIBANCO"></asp:ListItem>
                                                    <asp:ListItem Value="145" Text="BBASE"></asp:ListItem>
                                                    <asp:ListItem Value="166" Text="BANSEFI"></asp:ListItem>
                                                    <asp:ListItem Value="168" Text="HIPOTECARIA FEDERAL"></asp:ListItem>
                                                    <asp:ListItem Value="600" Text="MONEXCB"></asp:ListItem>
                                                    <asp:ListItem Value="601" Text="GBM"></asp:ListItem>
                                                    <asp:ListItem Value="602" Text="MASARI"></asp:ListItem>
                                                    <asp:ListItem Value="605" Text="VALUE"></asp:ListItem>
                                                    <asp:ListItem Value="606" Text="ESTRUCTURADORES"></asp:ListItem>
                                                    <asp:ListItem Value="607" Text="TIBER"></asp:ListItem>
                                                    <asp:ListItem Value="608" Text="VECTOR"></asp:ListItem>
                                                    <asp:ListItem Value="610" Text="B&B"></asp:ListItem>
                                                    <asp:ListItem Value="614" Text="ACCIVAL"></asp:ListItem>
                                                    <asp:ListItem Value="615" Text="MERRILL LYNCH"></asp:ListItem>
                                                    <asp:ListItem Value="616" Text="FINAMEX"></asp:ListItem>
                                                    <asp:ListItem Value="617" Text="VALMEX"></asp:ListItem>
                                                    <asp:ListItem Value="618" Text="UNICA"></asp:ListItem>
                                                    <asp:ListItem Value="619" Text="MAPFRE"></asp:ListItem>
                                                    <asp:ListItem Value="620" Text="PROFUTURO"></asp:ListItem>
                                                    <asp:ListItem Value="621" Text="CB ACTINVER"></asp:ListItem>
                                                    <asp:ListItem Value="622" Text="OACTIN"></asp:ListItem>
                                                    <asp:ListItem Value="623" Text="SKANDIA"></asp:ListItem>
                                                    <asp:ListItem Value="626" Text="CBDEUTSCHE"></asp:ListItem>
                                                    <asp:ListItem Value="627" Text="ZURICH"></asp:ListItem>
                                                    <asp:ListItem Value="628" Text="ZURICHVI"></asp:ListItem>
                                                    <asp:ListItem Value="629" Text="SU CASITA"></asp:ListItem>
                                                    <asp:ListItem Value="630" Text="CB INTERCAM"></asp:ListItem>
                                                    <asp:ListItem Value="631" Text="CI BOLSA"></asp:ListItem>
                                                    <asp:ListItem Value="632" Text="BULLTICK CB"></asp:ListItem>
                                                    <asp:ListItem Value="633" Text="STERLING"></asp:ListItem>
                                                    <asp:ListItem Value="634" Text="FINCOMUN"></asp:ListItem>
                                                    <asp:ListItem Value="636" Text="HDI SEGUROS"></asp:ListItem>
                                                    <asp:ListItem Value="637" Text="ORDER"></asp:ListItem>
                                                    <asp:ListItem Value="638" Text="AKALA"></asp:ListItem>
                                                    <asp:ListItem Value="640" Text="CB JPMORGAN"></asp:ListItem>
                                                    <asp:ListItem Value="642" Text="REFORMA"></asp:ListItem>
                                                    <asp:ListItem Value="646" Text="STP"></asp:ListItem>
                                                    <asp:ListItem Value="647" Text="TELECOMM"></asp:ListItem>
                                                    <asp:ListItem Value="648" Text="EVERCORE"></asp:ListItem>
                                                    <asp:ListItem Value="649" Text="SKANDIA"></asp:ListItem>
                                                    <asp:ListItem Value="651" Text="SEGMTY"></asp:ListItem>
                                                    <asp:ListItem Value="652" Text="ASEA"></asp:ListItem>
                                                    <asp:ListItem Value="653" Text="KUSPIT"></asp:ListItem>
                                                    <asp:ListItem Value="655" Text="SOFIEXPRESS"></asp:ListItem>
                                                    <asp:ListItem Value="656" Text="UNAGRA"></asp:ListItem>
                                                    <asp:ListItem Value="659" Text="OPCIONES EMPRESARIALES DEL NOROESTE"></asp:ListItem>
                                                    <asp:ListItem Value="901" Text="CLS"></asp:ListItem>
                                                    <asp:ListItem Value="902" Text="INDEVAL"></asp:ListItem>
                                                    <asp:ListItem Value="670" Text="LIBERTAD"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbClabe" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="18"></asp:TextBox>
                                                <asp:MaskedEditExtender PromptCharacter="" runat="server" ID="MaskedEditExtender2" TargetControlID="tbClabe" ClearMaskOnLostFocus="false" MaskType="None" Mask="999999999999999999" />
                                            </div>
                                            <div class="col-md-2"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab4">
                <asp:UpdatePanel ID="updTab8" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <!-- No es necesario ningun div de validation -->
                        <div align="center">
                            <strong>
                                <span class="style3">Información de Deducciones y Percepciones</span></strong>
                        </div>
                        <br />
                        <div class="panel-group rowsSpaced" id="accordion3" role="tablist" aria-multiselectable="true">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" data-parent="#accordion3" href="#collapse11">PERCEPCIONES</a>
                                    </h4>
                                </div>
                                <div id="collapse11" class="panel-collapse collapse in" role="tabpanel">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-8">TIPO PERCEPCIÓN:</div>
                                            <div class="col-md-4">
                                                CLAVE:
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="^[\s\S]{3,15}$" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbClavePercepcion" ValidationGroup="validationPercepcion" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlTipoPercepcion" runat="server" CssClass="form-control" Style="text-align: center;">
                                                    <asp:ListItem Value="001" Text="Sueldos, Salarios Rayas y Jornales"></asp:ListItem>
                                                    <asp:ListItem Value="002" Text="Gratificación Anual (Aguinaldo)"></asp:ListItem>
                                                    <asp:ListItem Value="003" Text="Participación de los Trabajadores en las Utilidades PTU"></asp:ListItem>
                                                    <asp:ListItem Value="004" Text="Reembolso de Gastos Médicos Dentales y Hospitalarios"></asp:ListItem>
                                                    <asp:ListItem Value="005" Text="Fondo de Ahorro"></asp:ListItem>
                                                    <asp:ListItem Value="006" Text="Caja de ahorro"></asp:ListItem>
                                                    <asp:ListItem Value="009" Text="Contribuciones a Cargo del Trabajador Pagadas por el Patrón"></asp:ListItem>
                                                    <asp:ListItem Value="010" Text="Premios por puntualidad"></asp:ListItem>
                                                    <asp:ListItem Value="011" Text="Prima de Seguro de vida"></asp:ListItem>
                                                    <asp:ListItem Value="012" Text="Seguro de Gastos Médicos Mayores"></asp:ListItem>
                                                    <asp:ListItem Value="013" Text="Cuotas Sindicales Pagadas por el Patrón"></asp:ListItem>
                                                    <asp:ListItem Value="014" Text="Subsidios por incapacidad"></asp:ListItem>
                                                    <asp:ListItem Value="015" Text="Becas para trabajadores y/o hijos"></asp:ListItem>
                                                    <asp:ListItem Value="016" Text="Otros"></asp:ListItem>
                                                    <asp:ListItem Value="017" Text="Subsidio para el empleo"></asp:ListItem>
                                                    <asp:ListItem Value="019" Text="Horas extra"></asp:ListItem>
                                                    <asp:ListItem Value="020" Text="Prima dominical"></asp:ListItem>
                                                    <asp:ListItem Value="021" Text="Prima vacacional"></asp:ListItem>
                                                    <asp:ListItem Value="022" Text="Prima por antigüedad"></asp:ListItem>
                                                    <asp:ListItem Value="023" Text="Pagos por separación"></asp:ListItem>
                                                    <asp:ListItem Value="024" Text="Seguro de retiro"></asp:ListItem>
                                                    <asp:ListItem Value="025" Text="Indemnizaciones"></asp:ListItem>
                                                    <asp:ListItem Value="026" Text="Reembolso por funeral"></asp:ListItem>
                                                    <asp:ListItem Value="027" Text="Cuotas de seguridad social pagadas por el patrón"></asp:ListItem>
                                                    <asp:ListItem Value="028" Text="Comisiones"></asp:ListItem>
                                                    <asp:ListItem Value="029" Text="Vales de despensa"></asp:ListItem>
                                                    <asp:ListItem Value="030" Text="Vales de restaurante"></asp:ListItem>
                                                    <asp:ListItem Value="031" Text="Vales de gasolina"></asp:ListItem>
                                                    <asp:ListItem Value="032" Text="Vales de ropa"></asp:ListItem>
                                                    <asp:ListItem Value="033" Text="Ayuda para renta"></asp:ListItem>
                                                    <asp:ListItem Value="034" Text="Ayuda para artículos escolares"></asp:ListItem>
                                                    <asp:ListItem Value="035" Text="Ayuda para anteojos"></asp:ListItem>
                                                    <asp:ListItem Value="036" Text="Ayuda para transporte"></asp:ListItem>
                                                    <asp:ListItem Value="037" Text="Ayuda para gastos de funeral"></asp:ListItem>
                                                    <asp:ListItem Value="038" Text="Otros ingresos por salarios"></asp:ListItem>
                                                    <asp:ListItem Value="039" Text="Jubilaciones, pensiones o haberes de retiro"></asp:ListItem>
                                                    <asp:ListItem Value="040" Text="Ingreso pagado por Entidades federativas ... con ingresos propios"></asp:ListItem>
                                                    <asp:ListItem Value="041" Text="Ingreso por Entidades federativas ... con ingresos federales"></asp:ListItem>
                                                    <asp:ListItem Value="042" Text="Ingreso pagado por Entidades federativas ... con ingresos propios y federales"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbClavePercepcion" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="15"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                CONCEPTO:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbConceptoPercepcion" ValidationGroup="validationPercepcion" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <asp:TextBox ID="tbConceptoPercepcion" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2"></div>
                                            <div class="col-md-4">
                                                IMPORTE GRAVADO:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbIGravadoPercepcion" ValidationGroup="validationPercepcion" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-4">
                                                IMPORTE EXENTO:
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbIExentoPercepcion" ValidationGroup="validationPercepcion" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-2"></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2"></div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbIGravadoPercepcion" runat="server" Style="text-align: center" CssClass="form-control input-money" min="0" Text="0.00"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbIExentoPercepcion" runat="server" Style="text-align: center" CssClass="form-control input-money" min="0" Text="0.00"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2"></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:LinkButton ID="bAgregarPercepcion" runat="server" CssClass="btn btn-primary" OnClick="bAgregarPercepcion_Click" Text="Agregar Percepción" ValidationGroup="validationPercepcion"></asp:LinkButton>
                                            </div>
                                        </div>
                                        <hr />
                                        <asp:GridView ID="gvPercepciones" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None" DataSourceID="SqlDataPercTemp" DataKeyNames="idDetallesNominaTemp" OnRowDeleted="gvPercepciones_RowDeleted">
                                            <Columns>
                                                <asp:BoundField DataField="Tipo" HeaderText="TIPO"></asp:BoundField>
                                                <asp:BoundField DataField="Clave" HeaderText="CLAVE"></asp:BoundField>
                                                <asp:BoundField DataField="Concepto" HeaderText="CONCEPTO"></asp:BoundField>
                                                <asp:BoundField DataField="ImporteGravado" HeaderText="GRAVADO"></asp:BoundField>
                                                <asp:BoundField DataField="ImporteExento" HeaderText="EXENTO"></asp:BoundField>
                                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="18%">
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
                                            <div class="col-md-3"></div>
                                            <div class="col-md-3">TOTAL GRAVADO:</div>
                                            <div class="col-md-3">TOTAL EXENTO:</div>
                                            <div class="col-md-3"></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3"></div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbTotalGravadoPercepciones" runat="server" Style="text-align: center" CssClass="form-control" Text="0.00" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbTotalExentoPercepciones" runat="server" Style="text-align: center" CssClass="form-control" Text="0.00" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3"></div>
                                        </div>
                                        <asp:SqlDataSource ID="SqlDataPercTemp" runat="server" SelectCommand="SELECT idDetallesNominaTemp, Tipo, Clave, Concepto, ImporteGravado, ImporteExento FROM Dat_DetallesNominaTemp WHERE (idUser = @idUser AND codigo = 1 AND SessionId=@SessionId)" DeleteCommand="DELETE FROM Dat_DetallesNominaTemp WHERE (idDetallesNominaTemp = @idDetallesNominaTemp)">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                                <asp:Parameter Name="SessionId" DefaultValue="" />
                                            </SelectParameters>
                                            <DeleteParameters>
                                                <asp:Parameter Name="idDetallesNominaTemp" />
                                            </DeleteParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" data-parent="#accordion3" href="#collapse22">DEDUCCIONES</a>
                                    </h4>
                                </div>
                                <div id="collapse22" class="panel-collapse collapse" role="tabpanel">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-8">TIPO DEDUCCIÓN:</div>
                                            <div class="col-md-4">
                                                CLAVE:
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ValidationExpression="^[\s\S]{3,15}$" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbClaveDeduccion" ValidationGroup="validationDeduccion" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlTipoDeduccion" runat="server" CssClass="form-control" Style="text-align: center;" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoDeduccion_SelectedIndexChanged">
                                                    <asp:ListItem Value="001" Text="Seguridad social"></asp:ListItem>
                                                    <asp:ListItem Value="002" Text="ISR"></asp:ListItem>
                                                    <asp:ListItem Value="003" Text="Aportaciones a retiro, cesantía en edad avanzada y vejez."></asp:ListItem>
                                                    <asp:ListItem Value="004" Text="Otros"></asp:ListItem>
                                                    <asp:ListItem Value="005" Text="Aportaciones a Fondo de vivienda"></asp:ListItem>
                                                    <asp:ListItem Value="006" Text="Descuento por incapacidad"></asp:ListItem>
                                                    <asp:ListItem Value="007" Text="Pensión alimenticia"></asp:ListItem>
                                                    <asp:ListItem Value="008" Text="Renta"></asp:ListItem>
                                                    <asp:ListItem Value="009" Text="Préstamos provenientes del INFONAVIT"></asp:ListItem>
                                                    <asp:ListItem Value="010" Text="Pago por crédito de vivienda"></asp:ListItem>
                                                    <asp:ListItem Value="011" Text="Pago de abonos INFONACOT"></asp:ListItem>
                                                    <asp:ListItem Value="012" Text="Anticipo de salarios"></asp:ListItem>
                                                    <asp:ListItem Value="013" Text="Pagos hechos con exceso al trabajador"></asp:ListItem>
                                                    <asp:ListItem Value="014" Text="Errores"></asp:ListItem>
                                                    <asp:ListItem Value="015" Text="Pérdidas"></asp:ListItem>
                                                    <asp:ListItem Value="016" Text="Averías"></asp:ListItem>
                                                    <asp:ListItem Value="017" Text="Adquisición de artículos producidos por la empresa o establecimiento"></asp:ListItem>
                                                    <asp:ListItem Value="018" Text="Cuotas para la constitución y fomento de sociedades cooperativas y de cajas de ahorro"></asp:ListItem>
                                                    <asp:ListItem Value="019" Text="Cuotas sindicales"></asp:ListItem>
                                                    <asp:ListItem Value="020" Text="Ausencia (Ausentismo)"></asp:ListItem>
                                                    <asp:ListItem Value="021" Text="Cuotas obrero patronales"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbClaveDeduccion" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="15"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                CONCEPTO:

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbConceptoDeduccion" ValidationGroup="validationDeduccion" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <asp:TextBox ID="tbConceptoDeduccion" runat="server" CssClass="form-control" Style="text-align: center;" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2"></div>
                                            <div class="col-md-4">
                                                IMPORTE GRAVADO:

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbIGravadoDeduccion" ValidationGroup="validationDeduccion" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-4">
                                                IMPORTE EXENTO:

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbIExentoDeduccion" ValidationGroup="validationDeduccion" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-2"></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2"></div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbIGravadoDeduccion" runat="server" Style="text-align: center" CssClass="form-control input-money" min="0" Text="0.00"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbIExentoDeduccion" runat="server" Style="text-align: center" CssClass="form-control input-money" min="0" Text="0.00"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2"></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:LinkButton ID="bAgregarDeduccion" runat="server" CssClass="btn btn-primary" OnClick="bAgregarDeduccion_Click" Text="Agregar Deducción" ValidationGroup="validationDeduccion"></asp:LinkButton>
                                            </div>
                                        </div>
                                        <hr />
                                        <asp:GridView ID="gvDeducciones" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None" DataSourceID="SqlDataDedcTemp" DataKeyNames="idDetallesNominaTemp" OnRowDeleted="gvDeducciones_RowDeleted">
                                            <Columns>
                                                <asp:BoundField DataField="Tipo" HeaderText="TIPO"></asp:BoundField>
                                                <asp:BoundField DataField="Clave" HeaderText="CLAVE"></asp:BoundField>
                                                <asp:BoundField DataField="Concepto" HeaderText="CONCEPTO"></asp:BoundField>
                                                <asp:BoundField DataField="ImporteGravado" HeaderText="GRAVADO"></asp:BoundField>
                                                <asp:BoundField DataField="ImporteExento" HeaderText="EXENTO"></asp:BoundField>
                                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="18%">
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
                                            <div class="col-md-3"></div>
                                            <div class="col-md-3">TOTAL GRAVADO:</div>
                                            <div class="col-md-3">TOTAL EXENTO:</div>
                                            <div class="col-md-3"></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3"></div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbTotalGravadoDeducciones" runat="server" Style="text-align: center" CssClass="form-control" Text="0.00" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="tbTotalExentoDeducciones" runat="server" Style="text-align: center" CssClass="form-control" Text="0.00" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3"></div>
                                        </div>
                                        <asp:SqlDataSource ID="SqlDataDedcTemp" runat="server" SelectCommand="SELECT idDetallesNominaTemp, Tipo, Clave, Concepto, ImporteGravado, ImporteExento FROM Dat_DetallesNominaTemp WHERE (idUser = @idUser AND codigo = 2 AND SessionId=@SessionId)" DeleteCommand="DELETE FROM Dat_DetallesNominaTemp WHERE (idDetallesNominaTemp = @idDetallesNominaTemp)">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                                <asp:Parameter Name="SessionId" DefaultValue="" />
                                            </SelectParameters>
                                            <DeleteParameters>
                                                <asp:Parameter Name="idDetallesNominaTemp" />
                                            </DeleteParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab5">
                <asp:UpdatePanel ID="updTab5" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <!-- DIVS DE VALIDACION AQUI -->
                        <div align="center">
                            <strong>
                                <span class="style3">Información de Incapacidades/Horas Extras</span></strong>
                        </div>
                        <br />
                        <div class="rowsSpaced">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">INCAPACIDADES</h4>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            DÍAS:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDiasIncap" ValidationGroup="validationIncapacidad" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6">
                                            TIPO:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="ddlTipoIncap" InitialValue="0" ValidationGroup="validationIncapacidad" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3">
                                            DESCUENTO:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDescuentoIncap" ValidationGroup="validationIncapacidad" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <asp:TextBox ID="tbDiasIncap" runat="server" CssClass="form-control input-number" placeholder="0"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlTipoIncap" runat="server" CssClass="form-control" Style="text-align: center;">
                                                <asp:ListItem Value="0" Text="SELECCIONE"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Riesgo de trabajo"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Enfermedad en general"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Maternidad"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="tbDescuentoIncap" runat="server" CssClass="form-control input-money" placeholder="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:LinkButton ID="bAgregarIncap" runat="server" CssClass="btn btn-primary" ValidationGroup="validationIncapacidad" OnClick="bAgregarIncap_Click">Agregar</asp:LinkButton>
                                        </div>
                                    </div>
                                    <br />
                                    <asp:GridView ID="gvIncap" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="5" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None" OnRowDeleting="gvIncap_RowDeleting" OnPageIndexChanging="gvIncap_PageIndexChanging" DataKeyNames="index">
                                        <Columns>
                                            <asp:BoundField DataField="dias" HeaderText="DÍAS" />
                                            <asp:BoundField DataField="tipoString" HeaderText="TIPO" />
                                            <asp:BoundField DataField="descuento" HeaderText="DESCUENTO" />
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
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">HORAS EXTRAS</h4>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            DÍAS:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDiasExtras" ValidationGroup="validationExtras" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3">
                                            TIPO:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="ddlTipoExtras" InitialValue="0" ValidationGroup="validationExtras" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3">
                                            HORAS:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbHorasExtras" ValidationGroup="validationExtras" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3">
                                            IMPORTE:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbImporteExtras" ValidationGroup="validationExtras" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <asp:TextBox ID="tbDiasExtras" runat="server" CssClass="form-control input-number" placeholder="0"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ddlTipoExtras" runat="server" CssClass="form-control" Style="text-align: center;">
                                                <asp:ListItem Value="0" Text="SELECCIONE"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Dobles"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Triples"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="tbHorasExtras" runat="server" CssClass="form-control input-number" placeholder="0"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="tbImporteExtras" runat="server" CssClass="form-control input-money" placeholder="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:LinkButton ID="bAgregarExtras" runat="server" CssClass="btn btn-primary" ValidationGroup="validationExtras" OnClick="bAgregarExtras_Click">Agregar</asp:LinkButton>
                                        </div>
                                    </div>
                                    <br />
                                    <asp:GridView ID="gvExtras" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="5" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None" OnRowDeleting="gvExtras_RowDeleting" OnPageIndexChanging="gvExtras_PageIndexChanging" DataKeyNames="index">
                                        <Columns>
                                            <asp:BoundField DataField="dias" HeaderText="DÍAS" />
                                            <asp:BoundField DataField="tipo" HeaderText="TIPO" />
                                            <asp:BoundField DataField="horas" HeaderText="HORAS" />
                                            <asp:BoundField DataField="importe" HeaderText="IMPORTE PAGADO" />
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
        <div class="modal-dialog" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Buscar Clientes</h4>
                </div>
                <div class="modal-body ">
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
                                </fieldset>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="gvRec" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" Font-Size="X-Small" GridLines="None" DataKeyNames="IDEREC" DataSourceID="SqlDataReceptores" EmptyDataText="Sin Datos." AllowPaging="true" OnPageIndexChanging="gvRec_PageIndexChanging" RowStyle-Height="5px">
                                        <Columns>
                                            <asp:BoundField DataField="RFCREC" HeaderText="RFC" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:BoundField DataField="NOMREC" HeaderText="NOMBRE" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
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
                                    <asp:SqlDataSource ID="SqlDataReceptores" runat="server" SelectCommand="SELECT IDEREC ,RFCREC ,NOMREC FROM Cat_Receptor ORDER BY NOMREC"></asp:SqlDataSource>
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
    NÓMINA
</asp:Content>
