<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="crearFactura.aspx.cs" Inherits="DataExpressWeb.CrearFacturaMx" Async="true" %>

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
            var rfcEmisor = '<%= Session["IDENTEMI"] %>';
            if (rfcEmisor != "ZFM111018JQ4" && rfcEmisor != "YAN810728RW7") {
                $('#rootwizard').bootstrapWizard('show', 1);
            }
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
                    //var chk = Boolean($("[id*=chkHabilitarINE]").is(':checked'));
                    //if (chk) {
                    //    var countConc = $("[id*=gvRegistros] td").closest("tr").not('.empty').length;
                    //    control = Boolean(countConc > 0);
                    //    if (!control) {                                   
                    //        alertBootBox("Debe agregar al menos una Entidad", 4);
                    //    }
                    //}
                    break;
                case 3:
                    var countConc = $("[id*=gvConceptos] td").closest("tr").not('.empty').length;
                    control = Boolean(countConc > 0);
                    if (!control) {
                        alertBootBox("Debe agregar al menos un Concepto", 4);
                    }
                    break;
                default:
                    break;
            }
            if (!control) {
                //alert(currentIndex - 1);
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
        function verAduanerasConcepto() {
            var countPartes = $("[id*=gvPartes] td").closest("tr").length;
            var pred = $('#<%= tbNumeroPredial.ClientID %>').val() || "";
            if (countPartes > 0) {
                alertBootBox("Ya se han agregado Partes al concepto, no puede mezlar Informacion Aduanera y Partes", 4);
            } else if (pred != "") {
                alertBootBox("Ya se ha definido una Cuenta Predial, no puede mezlar Informacion Aduanera y Cuenta Predial", 4);
            } else {
                $('#divAduanasConcepto').modal('toggle');
            }
        }
        function verAduanerasParte() {
            $('#divAduanasParte').modal('toggle');
        }
        function verHabitacion() {
            var evt1 = $('#<%= tbHusespedEvento.ClientID %>').val();
            var evt2 = $('#<%= tbReservacionEvento.ClientID %>').val();
            var evt3 = $('#<%= tbFechaEvento.ClientID %>').val();
            var isEvento = Boolean(evt1 != "" || evt2 != "" || evt3 != "");
            if (isEvento) {
                alertBootBox("Ya se ha definido información para un evento, no puede mezlar evento y habitación", 4);
            } else {
                $('#divHabitacion').modal('toggle');
            }
        }
        function verEvento() {
            var hab1 = $('#<%= tbHuespedHabitacion.ClientID %>').val();
            var hab2 = $('#<%= tbReservacionHabitacion.ClientID %>').val();
            var hab3 = $('#<%= tbHabitacion.ClientID %>').val();
            var hab4 = $('#<%= tbFechaHabitacion1.ClientID %>').val();
            var hab5 = $('#<%= tbFechaHabitacion2.ClientID %>').val();
            var hab6 = $('#<%= tbTipoHabitacion.ClientID %>').val();
            var isHab = Boolean(hab1 != "" || hab2 != "" || hab3 != "" || hab4 != "" || hab5 != "" || (hab6 != undefined && hab6 != ""));
            if (isHab) {
                alertBootBox("Ya se ha definido información para una habitación, no puede mezlar evento y habitación", 4);
            } else {
                $('#divEvento').modal('toggle');
            }
        }
        function verPartesConcepto() {
            var countAd = $("[id*=gvAduanasConcepto] td").closest("tr").length;
            var pred = $('#<%= tbNumeroPredial.ClientID %>').val() || "";
            if (countAd > 0) {
                alertBootBox("Ya se ha agregado Información Aduanera al concepto, no puede mezlar Partes e Informacion Aduanera", 4);
            } else if (pred != "") {
                alertBootBox("Ya se ha definido una Cuenta Predial, no puede mezlar Partes y Cuenta Predial", 4);
            } else {
                $('#divPartesConcepto').modal('toggle');
            }
        }
        function changeText(html, id) {
            $(id).val(html).change();
            return false;
        }
        function ValidateCheckBoxList(sender, args) {
            var checkBoxList = document.getElementById("<%=chkMetodoPago.ClientID %>");
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
        function gvImpuestosTerceroValidation(source, args) {
            var Grid = document.getElementById("<%=gvImpuestosTercero.ClientID%>");
            if (Grid == null) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function ddlClick() {
            var ddlBusq = $('#<%= ddlBusq.ClientID %>');
            var selectedValue = ddlBusq.val();
            $('#<%= tbUUID.ClientID %>').val('');
            $('#<%= tbFolio.ClientID %>').val('');
            $('#<%= tbSerie.ClientID %>').val('');
            if (selectedValue == "1") {
                $('#divUUID').css('display', 'inline');
                $('#divSerFol').css('display', 'none');
            } else if (selectedValue == "2") {
                $('#divUUID').css('display', 'none');
                $('#divSerFol').css('display', 'inline');
            }
        }
        function AjaxValidateUuid(urlAjax, dataAjax) {
            return $.ajax({
                type: "POST",
                url: urlAjax,
                data: dataAjax,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    console.log('AjaxValidateUuid: ' + data.d);
                    //return false;
                },
                failure: function (data) {
                    console.log('AjaxValidateUuid: FAIL');
                    //alertBootBox('No se pudo verificar Uuid<br/>Razón:' + response.d, 4);
                    //return confirm('No se pudo verificar Uuid. ¿Desea agregarlo de todas formas?');
                }
            });
        }
        function ValidarUuid() {
            var ddlBusq = $('#<%= ddlBusq.ClientID %>');
            var selectedValue = ddlBusq.val();
            var rfcDb = '<%=Session["IDENTEMI"].ToString() %>';
            var urlAjax = "";
            var dataAjax = '';
            var returned = false;
            if (selectedValue == "1") {
                var uuid = $('#<%= tbUUID.ClientID %>').val();
                if (uuid == undefined || uuid == "") {
                    alertBootBox('El UUID no puede estar vacío', 4);
                    return false;
                } else {
                    urlAjax = "./crearFactura.aspx/VerificarExistenciaUuid";
                    dataAjax = '{rfcDb: "' + rfcDb + '", Uuid: "' + uuid + '" }';
                }
            } else if (selectedValue == "2") {
                var serie = $('#<%= tbSerie.ClientID %>').val();
                var folio = $('#<%= tbFolio.ClientID %>').val();
                if (serie == undefined || serie == "") {
                    alertBootBox('La serie no puede estar vacía', 4);
                    return false;
                } else if (folio == undefined || folio == "") {
                    alertBootBox('El folio no puede estar vacío', 4);
                    return false;
                } else {
                    urlAjax = "./crearFactura.aspx/VerificarExistenciaSerieFolio";
                    dataAjax = '{rfcDb: "' + rfcDb + '", Serie: "' + serie + '", Folio: "' + folio + '" }';
                }
            }
        AjaxValidateUuid(urlAjax, dataAjax).done(function (data) {
            $("div#divLoading").removeClass('show');
            var valid = data.d;
            if (valid != undefined && (valid == "true" || valid == "1" || valid == "True" || valid == true)) {
                returned = true;
            } else if (selectedValue == "1") {
                returned = confirm('El UUID no existe en el sistema, ¿Desea agregarlo de cualquier forma?');
            } else if (selectedValue == "2") {
                returned = false;
                alertBootBox('No existe comprobante válido en sistema con la serie y el folio especificados', 4);
            }
        });
        return returned;
    }
    function EditarItem33(checked) {
        var divCustomItem = $('#rowCustomItem');
        $('#<%= tbCutomItem.ClientID %>').val('');
        if (checked) {
            divCustomItem.css('display', 'inline');
        } else {
            divCustomItem.css('display', 'none');
        }
    }
    function CheckPostalCode(sender, args) {
        var compare = RegExp("\\d{5}");
        args.IsValid = compare.test(args.Value);
        return;
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
                            <a href="#tab3" data-toggle="tab" id="linkTab3" runat="server">Complementos</a>
                        </li>
                        <li>
                            <a href="#tab4" data-toggle="tab" id="linkTab4" runat="server">Conceptos</a>
                        </li>
                        <%--<li>
                            <a href="#tab4" data-toggle="tab" id="linkTab4" runat="server">Impuestos</a>
                        </li>
                        <li>
                            <a href="#tab5" data-toggle="tab" id="linkTab5" runat="server">Impuestos Locales</a>
                        </li>--%>
                        <li>
                            <a href="#tab5" data-toggle="tab" id="linkTab5" runat="server">Documento</a>
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
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlSucEmi" runat="server" CssClass="form-control bootstrap-select" Style="text-align: center" DataSourceID="SqlDataSourceSucEmi" DataTextField="sucursal" DataValueField="idSucursal" AutoPostBack="true" OnSelectedIndexChanged="ddlSucEmi_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSourceSucEmi" runat="server" SelectCommand="SELECT '0' AS idSucursal, 'Seleccione...' AS sucursal UNION SELECT idSucursal, (clave + ': ' + sucursal) AS sucursal FROM Cat_SucursalesEmisor WHERE eliminado = 0"></asp:SqlDataSource>
                                </div>
                                <div class="col-md-1">
                                    RFC:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRfcEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbRfcEmi" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRuc_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRucEmi" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcEmi" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-md-1">
                                    RAZÓN SOCIAL:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbNomEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-5">
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
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbCpEmi" ValidationExpression="\d{5}" SetFocusOnError="true" Style="color: #FF0000" ValidationGroup="validationEmisorDom"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbCpEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" ReadOnly="True" MaxLength="5"></asp:TextBox>
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
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="*" ControlToValidate="tbCpExp" ValidationExpression="\d{5}" SetFocusOnError="true" Style="color: #FF0000" ValidationGroup="validationEmisorDomExp"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="tbCpExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" ReadOnly="True" MaxLength="5"></asp:TextBox>
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
                        <div class="validationGroup" value="validationReceptorDom" mensaje="Verifique que los datos del domicilio fiscal sean correctos" habilitado=""></div>
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
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                        runat="server"
                                        ControlToValidate="tbRfcRec"
                                        ErrorMessage="*"
                                        ForeColor="Red"
                                        SetFocusOnError="True"
                                        ValidationGroup="validationReceptor"
                                        ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                    </asp:RegularExpressionValidator>
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

                                   

                                                           



                                                           







                                                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tbCalleRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>--%>
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
                                                           
                                                           

                                                        <%--    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMunicipioRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-1">
                                                            ESTADO:

                                   

                                                           



                                                           







                                                   <%--         <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbEstadoRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                        <div class="col-md-7">
                                                            <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1">
                                                            C.P.:

                                   

                                                           



                                                           






                                                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="*"
                                                                ControlToValidate="tbCpRec" ValidateEmptyText="true"
                                                                ClientValidationFunction="CheckPostalCode" ValidationGroup="validationReceptorDom" Style="color: #FF0000"></asp:CustomValidator>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbCpRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" ReadOnly="True" MaxLength="5"></asp:TextBox>
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
                                                        <div class="col-md-1">E-MAIL:<asp:RequiredFieldValidator ID="RequiredFieldValidator53" runat="server" ControlToValidate="tbMailRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator></div>
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
            <div class="tab-pane" id="tab3">
                <asp:UpdatePanel ID="updTab7" runat="server">
                    <ContentTemplate>
                        <div class="validationGroup" value="validationIne" mensaje="Verifique que los datos del complemento INE" id="divValidationIne" runat="server"></div>
                        <div class="validationGroup" value="validationLeyendas" mensaje="Verifique que los datos del complemento de Leyendas Fiscales sean correctos" id="divValidationLeyendasFiscales" runat="server"></div>
                        <div class="validationGroup" value="validationComercioExterior" mensaje="Verifique que los datos del complemento de Comercio Exterior sean correctos" id="divValidationComercioExterior" runat="server"></div>
                        <div class="validationGroup" value="validationTerceros" mensaje="Verifique que los datos del complemento de Pagos a Terceros sean correctos" id="divValidationTerceros" runat="server"></div>
                        <div align="center">
                            <strong><span class="style3">Complementos</span></strong>
                        </div>
                        <br />
                        <div align="center">
                            <asp:CheckBox ID="chkHabilitarINE" runat="server" Text="INE" OnCheckedChanged="chkHabilitarINE_CheckedChanged" Checked="false" AutoPostBack="true" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="chkHabilitarLeyendasFiscales" runat="server" Text="Leyendas Fiscales" OnCheckedChanged="chkHabilitarLeyendasFiscales_CheckedChanged" Checked="false" AutoPostBack="true" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="chkHabilitarComercioExterior" runat="server" Text="Comercio Exterior" OnCheckedChanged="chkHabilitarComercioExterior_CheckedChanged" Checked="false" AutoPostBack="true" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="chkHabilitarTerceros" runat="server" Text="Pagos a Terceros" OnCheckedChanged="chkHabilitarTerceros_CheckedChanged" Checked="false" AutoPostBack="true" />
                        </div>
                        <br />
                        <div id="divIne" runat="server" class="rowsSpaced" align="center" visible="false">
                            <div class="panel panel-primary">
                                <div class="panel panel-heading" style="background: #337ab7 !important">INE</div>
                                <div class="panel-body">
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
                                                <asp:ListItem Value="Directivo Estatal">Directivo Estatal</asp:ListItem>
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
                                                <asp:ListItem Value="DIF">CIUDAD DE MÉXICO</asp:ListItem>
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
                        <div id="divLeyendasFiscales" runat="server" class="rowsSpaced" align="center" visible="false">
                            <div class="panel panel-primary">
                                <div class="panel panel-heading" style="background: #337ab7 !important">Leyendas Fiscales</div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-2">DISPOSICIÓN FISCAL:</div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="tbLeyendas_Disposicion" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">NORMA:</div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="tbLeyendas_Norma" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">LEYENDA:<asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbLeyendas_Texto" ValidationGroup="validationLeyendas"></asp:RequiredFieldValidator></div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="tbLeyendas_Texto" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divComercioExterior" runat="server" class="rowsSpaced" align="center" visible="false">
                            <div class="panel panel-primary">
                                <div class="panel panel-heading" style="background: #337ab7 !important">Comercio Exterior</div>
                                <div class="panel-body">
                                    <div class="alert alert-info">En construcción...</div>
                                    <div class="row">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divTerceros" runat="server" class="rowsSpaced" align="center" visible="false">
                            <div class="panel panel-primary">
                                <div class="panel panel-heading" style="background: #337ab7 !important">Pagos a Terceros</div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-1">RFC:<asp:RequiredFieldValidator ID="RequiredFieldValidator49" runat="server" ControlToValidate="tbRfcTercero" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationTerceros"></asp:RequiredFieldValidator></div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="tbRfcTercero" runat="server" CssClass="form-control upper" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRfcRec_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">RAZÓN SOCIAL:<asp:RequiredFieldValidator ID="RequiredFieldValidator50" runat="server" ControlToValidate="tbRazonTercero" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationTerceros"></asp:RequiredFieldValidator></div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="tbRazonTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                            CALLE:
                                               
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="tbCalleTercero" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationTerceros"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="tbCalleTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">NO. EXT.:</div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="tbNoExtTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">NO. INT.:</div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="tbNoIntTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">COLONIA:</div>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="tbColoniaTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">LOCALIDAD:</div>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="tbLocTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LOCALIDAD *"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                            MUNICIPIO:
                                               
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ControlToValidate="tbMunicipioTercero" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationTerceros"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="tbMunicipioTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">REFERENCIA:</div>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="tbRefTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="REFERENCIA *"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                            ESTADO:
                                               
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" ControlToValidate="tbEstadoTercero" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationTerceros"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="tbEstadoTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            C.P.:
                                               
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server" ControlToValidate="tbCpTercero" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationTerceros"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="tbCpTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" MaxLength="5"></asp:TextBox>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender5" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpTercero" Mask="99999" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                            PAÍS:
                                               
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ControlToValidate="tbPaisTercero" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationTerceros"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="tbPaisTercero" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="panel panel-default">
                                                <div class="panel panel-heading" style="background: #f5f5f5 !important">Impuestos</div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="ddlTipoImpuestoTercero_RequiredFieldValidator" runat="server" ErrorMessage="* " ForeColor="Red" ControlToValidate="ddlTipoImpuestoTercero" ValidationGroup="validationImpuestosTerceros" InitialValue="00"></asp:RequiredFieldValidator>TIPO:
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="ddlImpuestoTercero_RequiredFieldValidator" runat="server" ErrorMessage="* " ForeColor="Red" ControlToValidate="ddlImpuestoTercero" ValidationGroup="validationImpuestosTerceros" InitialValue="00"></asp:RequiredFieldValidator>IMPUESTO:
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="tbTasaTercero_RequiredFieldValidator" runat="server" ErrorMessage="* " ForeColor="Red" ControlToValidate="tbTasaTercero" ValidationGroup="validationImpuestosTerceros"></asp:RequiredFieldValidator>TASA (%):
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="tbImporteTercero_RequiredFieldValidator" runat="server" ErrorMessage="* " ForeColor="Red" ControlToValidate="tbImporteTercero" ValidationGroup="validationImpuestosTerceros"></asp:RequiredFieldValidator>IMPORTE:
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <asp:DropDownList ID="ddlTipoImpuestoTercero" runat="server" Style="text-align: center" CssClass="form-control" OnSelectedIndexChanged="ddlTipoImpuestoTercero_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="00" Text="SELECCIONE"></asp:ListItem>
                                                                <asp:ListItem Value="02" Text="TRASLADO"></asp:ListItem>
                                                                <asp:ListItem Value="01" Text="RETENCIÓN"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:DropDownList ID="ddlImpuestoTercero" runat="server" Style="text-align: center" CssClass="form-control" Enabled="false">
                                                                <asp:ListItem Value="00" Text="SELECCIONE"></asp:ListItem>
                                                                <asp:ListItem Value="IVA"></asp:ListItem>
                                                                <asp:ListItem Value="ISR"></asp:ListItem>
                                                                <asp:ListItem Value="IEPS"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbTasaTercero" runat="server" CssClass="form-control input-decimal" placeholder="00.00 *" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTasaTercero" ValidChars=",."></asp:FilteredTextBoxExtender>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="tbImporteTercero" runat="server" CausesValidation="True" CssClass="form-control input-decimal" placeholder="00.00 *" Style="text-align: center;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbImporteTercero" ValidChars=",."></asp:FilteredTextBoxExtender>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:LinkButton ID="lbAgregarImpuestoTercero" runat="server" CssClass="btn btn-primary" OnClick="lbAgregarImpuestoTercero_Click" Text="Agregar" ValidationGroup="validationImpuestosTerceros"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="*" Display="None" ClientValidationFunction="gvImpuestosTerceroValidation" ValidationGroup="validationTerceros"></asp:CustomValidator>
                                                            <asp:GridView ID="gvImpuestosTercero" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None" DataKeyNames="id" OnRowDeleting="gvImpuestosTercero_RowDeleting" OnRowDataBound="gvImpuestosTercero_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="TIPO" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTipoImpuesto" runat="server" Text=""></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="30%"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="impuesto" HeaderText="IMPUESTO" ItemStyle-Width="40%">
                                                                        <ItemStyle Width="30%"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="TASA" ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTasa" runat="server" Text=""></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="importe" HeaderText="IMPORTE" ItemStyle-Width="10%">
                                                                        <ItemStyle Width="10%"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LinkButton3" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CssClass="btn btn-primary btn-xs" CausesValidation="False" CommandName="Delete" Text="Eliminar">
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
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab4">
                <asp:UpdatePanel ID="updTab3" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div align="center">
                            <strong>
                                <span class="auto-style1">Conceptos</span></strong>
                        </div>
                        <br />
                        <div class="rowsSpaced" align="center">
                            <div class="row" id="rowCategoria" runat="server" visible="true">
                                <div class="col-md-3"></div>
                                <div class="col-md-2">
                                    CATEGORÍA:

                               
                               
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCatConc" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataCatConc" DataTextField="descripcion" DataValueField="idCatalogo1_C" OnSelectedIndexChanged="ddlCatConc_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataCatConc" runat="server" SelectCommand="SELECT '0' AS idCatalogo1_C, 'Seleccione' AS descripcion UNION SELECT idCatalogo1_C, descripcion FROM Cat_Catalogo1_C WHERE tipo = 'CategoriaConceptos'"></asp:SqlDataSource>
                                </div>
                                <div class="col-md-1">
                                    <asp:LinkButton ID="lbCatalogoConceptos" runat="server" CssClass="btn btn-primary" OnClick="lbCatalogoConceptos_Click">
                                        <i class="fa fa-arrow-circle-up" aria-hidden="true"></i>&nbsp;Catálogo
                                    </asp:LinkButton>
                                </div>
                                <div class="col-md-1">
                                    <asp:LinkButton ID="lbRecargar" runat="server" CssClass="btn btn-primary" data-toggle="tooltip" data-placement="top" title="Recargar conceptos" OnClick="lbRecargar_Click">
                                        <i class="fa fa-refresh" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-md-1"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-2">
                                    DESCRIPCIÓN:

                               
                               
                                </div>
                                <div class="col-md-4">
                                    <%--<asp:HiddenField ID="hfIdCat" runat="server" Value="0" />--%>
                                    <div runat="server" id="divTbDescConc" style="display: none;">
                                        <div class="input-group">
                                            <asp:TextBox ID="tbDescConc" runat="server" CssClass="form-control" Style="text-align: center" placeholder="DESCRIPCIÓN *" AutoPostBack="true" OnTextChanged="tbDescConc_TextChanged"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" id="btnDescConc" runat="server"><span class="caret"></span></button>
                                                <ul class="dropdown-menu dropdown-menu-right" role="menu" runat="server" id="ulDescConc" style="height: 200px; overflow: hidden; overflow-y: scroll;">
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:DropDownList ID="ddlDesCon" runat="server" CssClass="form-control bootstrap-select" Style="text-align: center" DataSourceID="SqlDataDescConc" DataTextField="descripcion" DataValueField="idConcepto" Visible='<%# Session["CfdiVersion"].ToString().Equals("3.3") %>' AutoPostBack="true" OnSelectedIndexChanged="ddlDesCon_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="tbDescConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationConc" Enabled='<%# !Session["CfdiVersion"].ToString().Equals("3.3") %>'></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator51" runat="server" ControlToValidate="ddlDesCon" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationConc" Enabled='<%# Session["CfdiVersion"].ToString().Equals("3.3") %>' InitialValue=""></asp:RequiredFieldValidator>
                                    <asp:SqlDataSource ID="SqlDataDescConc" runat="server" SelectCommand="(SELECT '' AS idConcepto, 'Seleccione' AS descripcion) UNION (SELECT idConcepto, descripcion FROM Cat_CatConceptos_C WHERE idCategoria=@idCat)">
                                        <SelectParameters>
                                            <%--<asp:ControlParameter ControlID="hfIdCat" Name="idCat" PropertyName="Value" />--%>
                                            <asp:Parameter Name="idCat" DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="col-md-2" style="text-align: left !important;">
                                    <asp:CheckBox ID="chkEditarItem33" runat="server" Text="Editar" OnClick="EditarItem33(!this.checked);" Visible="false" />
                                </div>
                                <div class="col-md-1"></div>
                            </div>
                            <div class="row" id="rowCustomItem" style="display: none">
                                <div class="col-md-3"></div>
                                <div class="col-md-2">
                                    EDITAR DESCRIPCIÓN:
                                </div>
                                <div class="col-md-4">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="sizing-addon2"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></span>
                                        <asp:TextBox ID="tbCutomItem" runat="server" CssClass="form-control" Style="text-align: center" placeholder="DESCRIPCIÓN *" aria-describedby="sizing-addon2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row" id="rowVerDatosSat" runat="server" style="display: none">
                                <div class="col-md-5"></div>
                                <div class="col-md-3">
                                    <span class="label label-primary" style="display: inline-block !important;">
                                        <h5>
                                            <asp:Label ID="lblCveProdServ" runat="server" Text=""></asp:Label>
                                            &nbsp;&nbsp;&nbsp<asp:Label ID="lblCveUnidad" runat="server" Text=""></asp:Label>
                                        </h5>
                                    </span>
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-2">
                                            CANTIDAD:

                               
                                       
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="tbCantidadConc" runat="server" CausesValidation="True" CssClass="form-control input-decimal" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="tbCantidadConc_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbCantidadConc" ValidChars=",."></asp:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbCantidadConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationConc"></asp:RequiredFieldValidator>
                                        </div>
                                        <div id="divUnidad" runat="server" visible="true">
                                            <div class="col-md-2">
                                                UNIDAD:

                                   
                                           
                                            </div>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <asp:TextBox ID="tbUnidadConc" runat="server" CssClass="form-control" Style="text-align: center" placeholder="UNIDAD *" Text=""></asp:TextBox>
                                                    <div class="input-group-btn" runat="server">
                                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" runat="server" id="btnDdlUnidadConc" style="display: none;"><span class="caret"></span></button>
                                                        <asp:ListView ID="lvUnidades" runat="server" DataSourceID="SqlDataSourceUnidades" DataKeyNames="id">
                                                            <LayoutTemplate>
                                                                <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                </ul>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <li>
                                                                    <a href="#" onclick="return changeText($(this).html().split(':')[0], '#<%= tbUnidadConc.ClientID %>');"><%# Session["CfdiVersion"].ToString().Equals("3.3") ? string.Format("{0}: {1}", Eval("claveSat"), Eval("descripcion")) : Eval("descripcion") %></a>
                                                                </li>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </div>
                                                </div>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbUnidadConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationConc"></asp:RequiredFieldValidator>
                                                <asp:SqlDataSource ID="SqlDataSourceUnidades" runat="server" SelectCommand="SELECT id, claveSat, descripcion FROM Cat_CveUnidad"></asp:SqlDataSource>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                            PRECIO:

                               
                                       
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="tbVUConc" runat="server" CausesValidation="True" CssClass="form-control input-decimal" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="tbVUConc_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbVUConc" ValidChars=",."></asp:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="tbVUConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationConc"></asp:RequiredFieldValidator>
                                        </div>
                                        <div id="divIdentificacion" runat="server" visible="true">
                                            <div class="col-md-2" align="justified">
                                                <asp:Label ID="Label2" runat="server" Text="IDENTIFICACION:"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="tbIdentConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="IDENTIFICACIÓN *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="rowDescuentos33" runat="server" style="display: none">
                                        <div class="col-md-2">DESCUENTO:</div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="tbDescuentoConc" runat="server" CausesValidation="True" CssClass="form-control input-decimal" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbDescuentoConc" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <asp:TextBox ID="tbImpConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center; display: none;" ToolTip="Descripción" ReadOnly="true"></asp:TextBox>
                                        <div class="col-md-12" align="left">
                                            <div class="row" align="left">
                                                <div class="col-md-12" align="left">
                                                    <asp:CheckBox ID="chkDesgloce" runat="server" Checked="false" size="normal" Enabled="true" Visible="true" Text="La cantidad ya incluye impuestos" />
                                                </div>
                                            </div>
                                            <div class="row" align="left">
                                                <div class="col-md-6" align="left">
                                                    <asp:CheckBox ID="chkHabilitarIva16" runat="server" Checked="true" size="normal" Enabled="true" Text="I.V.A." />
                                                </div>
                                                <div class="col-md-6" align="left">
                                                    <asp:CheckBox ID="chkHabilitarIsh" runat="server" Checked="false" size="normal" Enabled="true" Visible="false" Text="I.S.H." />
                                                </div>
                                                <div class="col-md-6" align="left">
                                                    <asp:CheckBox ID="chkHabilitarIUI" runat="server" Checked="false" size="normal" Enabled="true" Visible="false" Text="OTRO" />
                                                </div>
                                            </div>
                                            <div class="row" id="divIvaRet4" runat="server" style="display: none;">
                                                <div class="col-md-6" align="left">
                                                    <asp:CheckBox ID="chkHabilitarIvaRet4" runat="server" Checked="false" size="normal" Enabled="true" Text="I.V.A. Ret" />
                                                </div>
                                                <div class="col-md-6" align="left">
                                                </div>
                                            </div>



                                            <div class="row" align="left" id="divAdendaPropina" runat="server" style="display: none;">
                                                <asp:CheckBox ID="CheckAdendaPropina" runat="server" Checked="false" size="normal" Enabled="true" Visible="true" Text="Adenda Hoteles" />
                                            </div>
                                            <div id="rowISHEdit" runat="server" class="row" visible="false">
                                                <div class="col-md-6" align="left"></div>
                                                <div class="col-md-6" align="left">
                                                    <asp:TextBox ID="tbISHEdit" runat="server" CausesValidation="True" CssClass="form-control input-decimal" placeholder="00.00 *" Style="text-align: center;" ToolTip="ISH ($)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbISHEdit" ValidChars=",."></asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                            <div id="rowIUIEdit" runat="server" class="row" visible="false">
                                                <div class="col-md-6" align="left"></div>
                                                <div class="col-md-6" align="left">
                                                    <asp:TextBox ID="tbIUIEdit" runat="server" CausesValidation="True" CssClass="form-control input-decimal" placeholder="00.00 *" Style="text-align: center;" ToolTip="ISH ($)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbIUIEdit" ValidChars=",."></asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="divPredial" visible="false">
                                <div class="row">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-2">CUENTA PREDIAL:</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="tbNumeroPredial" runat="server" CausesValidation="True" CssClass="form-control" placeholder="NÚMERO *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="btn-group dropup" runat="server">
                                        <asp:LinkButton ID="bAgregarConcepto" runat="server" CssClass="btn btn-primary" OnClick="bAgregarConcepto_Click" Text="Agregar Concepto" ValidationGroup="validationConc"></asp:LinkButton>
                                        <% if (Session["IDGIRO"] != null && !Session["IDGIRO"].ToString().Contains("1") && !Session["IDGIRO"].ToString().Contains("2"))
                                            { %>
                                        <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <span class="caret"></span>
                                            <span class="sr-only">Toggle Dropdown</span>
                                        </button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="bPartes" runat="server" Text="Partes" OnClientClick="javascript:verPartesConcepto(); return false;" />
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="bAduanas" runat="server" Text="Información Aduanera" OnClientClick="javascript:verAduanerasConcepto(); return false;" />
                                            </li>
                                        </ul>
                                        <% } %>
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="rowHabEv" runat="server" visible="false">
                                <div class="col-md-5"></div>
                                <div class="col-md-2">
                                    <div class="dropup">
                                        <button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            Habitación/Evento
   
                                           

                                           



                                            <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" aria-labelledby="dropdownMenu2">
                                            <li runat="server">
                                                <asp:LinkButton ID="hlHabitacion" runat="server" OnClientClick="verHabitacion();">Habitación</asp:LinkButton>
                                            </li>
                                            <li runat="server">
                                                <asp:LinkButton ID="hlEvento" runat="server" OnClientClick="verEvento();">Evento</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="col-md-5"></div>
                            </div>
                            <div class="row" id="rowPropOtros" runat="server" visible="false">
                                <div class="col-md-2"></div>
                                <div class="col-md-1">
                                    <% if (Session != null && Session["IDENTEMI"] != null && Session["IDENTEMI"].ToString().Equals("TCA130827M58"))
                                        { %>
                                    CARGO X S.:
                                    <% }
                                        else
                                        { %>
                                    PROPINA:
                                    <% } %>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbPropinaConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción" ReadOnly="true" AutoPostBack="true" OnTextChanged="CalcularTotales"></asp:TextBox>
                                </div>
                                <div class="col-md-2">OTROS CARGOS:</div>
                                <div class="col-md-3" align="left">
                                    <asp:TextBox ID="tbOtrosCargosConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción" AutoPostBack="true" OnTextChanged="CalcularTotales"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <hr />
                            <asp:GridView ID="gvConceptos" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvConceptos_PageIndexChanged" OnSelectedIndexChanged="gvConceptos_SelectedIndexChanged" OnPageIndexChanging="gvConceptos_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvConceptos_PreRender" GridLines="None" DataSourceID="SqlDataConcTemp" DataKeyNames="idDetallesTemp" OnRowDeleted="gvConceptos_RowDeleted">
                                <Columns>
                                    <%--<asp:BoundField DataField="noId" HeaderText="IDENTIFICACIÓN">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN">
                                        <ItemStyle Width="35%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="valorUnitario" HeaderText="VAL. UNITARIO">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="importe" HeaderText="IMPORTE">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="unidad" HeaderText="UNIDAD">
                                        <ItemStyle Width="15%"></ItemStyle>
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="ctaPredial" HeaderText="CTA. PREDIAL">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>--%>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar" CssClass="btn btn-primary">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <%--<EmptyDataRowStyle CssClass="table empty" />
                                <EmptyDataTemplate>
                                    No existen datos.
                                </EmptyDataTemplate>--%>
                                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                        <asp:SqlDataSource ID="SqlDataConcTemp" runat="server" SelectCommand="SELECT idDetallesTemp, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial FROM Dat_DetallesTemp WHERE (id_Empleado = @idUser AND SessionId=@SessionId)" DeleteCommand="DELETE FROM Dat_DetallesTemp WHERE (idDetallesTemp = @idDetallesTemp)">
                            <SelectParameters>
                                <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                <asp:Parameter Name="SessionId" DefaultValue="" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="idDetallesTemp" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab51">
                <asp:UpdatePanel ID="updTab4" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div align="center">
                            <strong>
                                <span class="auto-style1">Impuestos</span></strong>
                        </div>
                        <br />
                        <div align="center">
                            <div class="row">
                                <div class="col-md-8"></div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-1">
                                    TIPO:
                               
                               
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlTipoImpuesto" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataCat1" DataTextField="Descripcion" DataValueField="codigo" OnSelectedIndexChanged="ddlTipoImpuesto_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1" align="justified">
                                    <asp:Label ID="lbl_cantidad" runat="server" Text="IMPUESTO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlImpuesto" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataCatImpuestos" DataTextField="Descripcion" DataValueField="codigo" OnSelectedIndexChanged="ddlImpuesto_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1" align="justified">
                                    <asp:Label ID="lbl_precio" runat="server" Text="TASA (%):"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTasa" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción" TextMode="Number" min="0" step="0.01">0</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbTasa_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTasa" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbTasa" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationImp"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1" align="justified">
                                    <asp:Label ID="Label1" runat="server" Text="IMPORTE:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbImporte" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción" TextMode="Number" min="0" step="0.01"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbImporte" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbImporte" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationImp"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <asp:LinkButton ID="bAgregarImpuesto" runat="server" CssClass="btn btn-primary" OnClick="bAgregarImpuesto_Click" Text="Agregar" ValidationGroup="validationImp"></asp:LinkButton>
                            <br />
                            <br />
                            <asp:GridView ID="gvImpuestos" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvImpuestos_PageIndexChanged" OnSelectedIndexChanged="gvImpuestos_SelectedIndexChanged" OnPageIndexChanging="gvImpuestos_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvImpuestos_PreRender" GridLines="None" DataSourceID="SqlDataImpTemp" DataKeyNames="idImpuestosDetallesTemp" OnRowDeleted="gvImpuestos_RowDeleted">
                                <Columns>
                                    <asp:BoundField DataField="tipo" HeaderText="TIPO" ItemStyle-Width="10%">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="impuesto" HeaderText="IMPUESTO" ItemStyle-Width="40%">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="tasa" HeaderText="TASA (%)" ItemStyle-Width="20%">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="importe" HeaderText="IMPORTE" ItemStyle-Width="10%">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar">
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
                                    <asp:Label ID="lbl_descuento" runat="server" Text="TOTAL RETENIDO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTotalImpRet" runat="server" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2" align="justified">
                                    <asp:Label ID="lbl_iva" runat="server" Text="TOTAL TRASLADADO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTotalImpTras" runat="server" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                        </div>
                        <asp:SqlDataSource ID="SqlDataCat1" runat="server" SelectCommand="SELECT [codigo], [descripcion] FROM Cat_Catalogo1_C WHERE (tipo = @tipo)">
                            <SelectParameters>
                                <asp:Parameter Name="tipo" DefaultValue="ImpuestoMX" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataCatImpuestos" runat="server" SelectCommand="SELECT [codigo], [descripcion] FROM Cat_Catalogo1_C WHERE (tipo = @tipo)">
                            <SelectParameters>
                                <asp:Parameter Name="tipo" DefaultValue="" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataImpTemp" runat="server" SelectCommand="SELECT id.idImpuestosDetallesTemp, cc.descripcion as tipo, cc2.descripcion as impuesto, id.tarifa AS tasa, id.valor as importe,  id.id_Empleado FROM Dat_ImpuestosDetallesTemp id INNER JOIN Cat_Catalogo1_C cc ON (cc.codigo = id.codigoTemp AND cc.tipo = 'ImpuestoMX') INNER JOIN Cat_Catalogo1_C cc2 ON (cc2.codigo = id.codigo AND cc2.tipo = (CASE cc2.codigo WHEN '01' THEN 'Impuesto Retenido' ELSE 'Impuesto Trasladado' END)) WHERE id_Empleado = @idUser AND SessionId=@SessionId" DeleteCommand="DELETE FROM Dat_ImpuestosDetallesTemp WHERE (idImpuestosDetallesTemp = @idImpuestosDetallesTemp)">
                            <SelectParameters>
                                <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                <asp:Parameter Name="SessionId" DefaultValue="" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="idImpuestosDetallesTemp" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab61">
                <asp:UpdatePanel ID="updTab5" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div align="center">
                            <strong>
                                <span class="auto-style1">Impuestos Locales</span></strong>
                        </div>
                        <br />
                        <div align="center">
                            <div class="row">
                                <div class="col-md-8"></div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-1">
                                    TIPO:
                               
                               
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlTipoImpuestoLocal" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataCat1" DataTextField="Descripcion" DataValueField="codigo">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1" align="justified">
                                    <asp:Label ID="Label4" runat="server" Text="IMPUESTO:"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbImpLocal" runat="server" CausesValidation="True" CssClass="form-control" placeholder="NOMBRE *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ControlToValidate="tbImpLocal" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationImpLoc"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1" align="justified">
                                    <asp:Label ID="Label5" runat="server" Text="TASA (%):"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbTasaLocal" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción" TextMode="Number" min="0" step="0.01"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTasaLocal" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ControlToValidate="tbTasaLocal" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationImpLoc"></asp:RequiredFieldValidator>
                                </div>
                                <%--<div class="col-md-1" align="justified">
                            <asp:Label ID="Label6" runat="server" Text="IMPORTE:"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="tbImporteLoc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbImporteLoc" ValidChars=",."></asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="tbImporteLoc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationImpLoc"></asp:RequiredFieldValidator>
                        </div>--%>
                            </div>
                            <asp:LinkButton ID="bAgregarImpuestoLocal" runat="server" CssClass="btn btn-primary" OnClick="bAgregarImpuestoLocal_Click" Text="Agregar" ValidationGroup="validationImpLoc"></asp:LinkButton>
                            <br />
                            <br />
                            <asp:GridView ID="gvImpuestosLocales" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvImpuestosLocales_PageIndexChanged" OnSelectedIndexChanged="gvImpuestosLocales_SelectedIndexChanged" OnPageIndexChanging="gvImpuestosLocales_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvImpuestosLocales_PreRender" GridLines="None" DataSourceID="SqlDataImpLocTemp" DataKeyNames="idImpLocalTemp" OnRowDeleted="gvImpuestosLocales_RowDeleted">
                                <Columns>
                                    <asp:BoundField DataField="tipo" HeaderText="TIPO" ItemStyle-Width="10%">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="impuesto" HeaderText="IMPUESTO" ItemStyle-Width="40%">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="tasa" HeaderText="TASA (%)" ItemStyle-Width="20%">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="importe" HeaderText="IMPORTE" ItemStyle-Width="10%">
                                <ItemStyle Width="10%"></ItemStyle>
                            </asp:BoundField>--%>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton4" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar">
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
                                    <asp:Label ID="Label7" runat="server" Text="TOTAL RETENIDO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTotalRetLoc" runat="server" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2" align="justified">
                                    <asp:Label ID="Label10" runat="server" Text="TOTAL TRASLADADO:"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbTotalTrasLoc" runat="server" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <%--<asp:SqlDataSource ID="SqlDataImpLocTemp" runat="server"
                        SelectCommand="SELECT id.idImpLocalTemp, cc.descripcion as tipo, id.nombre as impuesto, id.tasa, id.importe FROM Dat_MX_ImpLocalesTemp id INNER JOIN Cat_Catalogo1_C cc ON (cc.tipo = 'ImpuestoMX' AND cc.codigo = (CASE id.tipo WHEN '1' THEN '02' ELSE '01' END)) WHERE (id_Empleado = @idUser)" DeleteCommand="DELETE FROM Dat_MX_ImpLocalesTemp WHERE (idImpLocalTemp = @idImpLocalTemp)">--%>
                            <asp:SqlDataSource ID="SqlDataImpLocTemp" runat="server" SelectCommand="SELECT id.idImpLocalTemp, cc.descripcion as tipo, id.nombre as impuesto, id.tasa FROM Dat_MX_ImpLocalesTemp id INNER JOIN Cat_Catalogo1_C cc ON (cc.tipo = 'ImpuestoMX' AND cc.codigo = (CASE id.tipo WHEN '1' THEN '02' ELSE '01' END)) WHERE (id_Empleado = @idUser AND SessionId=@SessionId)" DeleteCommand="DELETE FROM Dat_MX_ImpLocalesTemp WHERE (idImpLocalTemp = @idImpLocalTemp)">
                                <SelectParameters>
                                    <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                    <asp:Parameter Name="SessionId" DefaultValue="" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="idImpLocalTemp" />
                                </DeleteParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="tab5">
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
                                    <div class="row" id="rowReciboRetenciones" runat="server" style="display: none;">
                                        <div class="col-md-2">RECIBO:</div>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="ddlTipoReciboRetencion" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoReciboRetencion_SelectedIndexChanged">
                                                <asp:ListItem Text="Seleccione" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Honorarios" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Arrendamiento" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:CheckBox ID="chkReciboRetenciones" runat="server" OnCheckedChanged="chkReciboRetenciones_CheckedChanged" AutoPostBack="true" Text="Retenciones" Visible="false" />
                                        </div>
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
                                        <div class="col-md-2" runat="server" id="divChkCredito" style="display: none;">
                                            <asp:CheckBox ID="chkCredito" runat="server" Text="CRÉDITO" Checked="false" />
                                        </div>
                                        <div class="col-md-2">MONEDA:</div>
                                        <div class="col-md-4">
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
                                    </div>
                                    <div class="row" id="rowCondiciones" runat="server" visible="true">
                                        <div class="col-md-2">CONDICIONES DE PAGO:</div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="tbCondPago" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CONDICIONES DE PAGO *"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="row" id="rowDescuento" runat="server" visible="true">
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
                                        <div class="col-md-1">TIPO CAMBIO:</div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="tbTipoCambio" runat="server" CssClass="form-control" Style="text-align: center" Text="1.0" placeholder="TIPO DE CAMBIO *"></asp:TextBox>
                                        </div>
                                        <div id="DivUsoCfdi" runat="server" style="display: none;">
                                            <div class="col-md-1">
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
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-7">
                                            <div class="row">
                                                <div class="col-md-12">
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
                                            </div>
                                            <div class="row" id="rowFechaEmision" runat="server" visible="false">
                                                <div class="col-md-2">FECHA:</div>
                                                <div class="col-md-10">
                                                    <div class="input-group datetime">
                                                        <asp:TextBox ID="tbFechaEmision" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <i class="fa fa-calendar-o"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
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
                                            <div id="divReferenciaRestaurante" runat="server" style="display: none;">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        REF. RESTAURANTE:                    
                                                   
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:TextBox ID="tbReferenciaRestaurante" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="divAddendaCts" runat="server" style="display: none;">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        CUPÓN:                    
                                                   
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3"></div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="tbCupon" runat="server" CssClass="form-control" Style="text-align: center" placeholder="cupo1,cupon2,..." CausesValidation="true"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredtbCupon" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbCupon" ValidChars=","></asp:FilteredTextBoxExtender>
                                                    </div>
                                                    <div class="col-md-3"></div>
                                                </div>
                                            </div>


                                            <!-------------------ADENDA CTS------------------------>

                                            <!-------------------FIN ADENDA CTS---------------------->

                                        </div>
                                    </div>
                                    <div class="row" id="rowExpedidoEn" runat="server" visible="false">
                                        <div class="col-md-2">
                                            EXPEDIDO EN:
                       
                                   

                                           



                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="tbLugarExp" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="tbLugarExp" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LUGAR DE EXPEDICION *"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">CANTIDAD CON LETRA:</div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="tbCantLetra" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">OBSERVACIONES:</div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="tbObservaciones" runat="server" CssClass="form-control" Style="text-align: center" placeholder="OBSERVACIONES"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" id="rowRelacionados" runat="server" style="display: none;">
                                        <div class="panel-group" id="accordionRelacionados" role="tablist" aria-multiselectable="true">
                                            <div class="panel panel-default">
                                                <div class="panel-heading" role="tab" id="headingRelacionados">
                                                    <h4 class="panel-title">
                                                        <a role="button" data-toggle="collapse" data-parent="#accordionRelacionados" href="#collapseRelacionados" aria-expanded="true" aria-controls="collapseRelacionados">DOCUMENTOS RELACIONADOS:</a>
                                                    </h4>
                                                </div>
                                                <div id="collapseRelacionados" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingRelacionados">
                                                    <div class="panel-body">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:DropDownList ID="ddlBusq" runat="server" Style="text-align: center" CssClass="form-control" onChange="javascript:ddlClick()">
                                                                    <asp:ListItem Value="1" Text="UUID" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="SERIE/FOLIO"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-5">TIPO RELACIÓN</div>
                                                            <div class="col-md-1"></div>
                                                        </div>
                                                        <div class="row">
                                                            <div id="divUUID" style="display: inline">
                                                                <div class="col-md-6">
                                                                    <asp:TextBox ID="tbUUID" runat="server" CssClass="form-control" Style="text-align: center" placeholder="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"></asp:TextBox>
                                                                    <asp:MaskedEditExtender runat="server" ID="tbUUID_MaskedEditExtender" TargetControlID="tbUUID" ClearMaskOnLostFocus="false" MaskType="None" Mask="CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC" Filtered="ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" />
                                                                </div>
                                                            </div>
                                                            <div id="divSerFol" style="display: none">
                                                                <div class="col-md-1">SERIE:</div>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox ID="tbSerie" runat="server" CssClass="form-control upper" Style="text-align: center"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-1">FOLIO:</div>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox ID="tbFolio" runat="server" CssClass="form-control" Style="text-align: center" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-5">
                                                                <asp:DropDownList ID="ddlTipoRelacion" runat="server" CssClass="form-control" Style="text-align: center;">
                                                                    <asp:ListItem Value="03" Text="Devolución de mercancía sobre facturas o traslados previos"></asp:ListItem>
                                                                    <asp:ListItem Value="04" Text="Sustitución de los CFDI previos"></asp:ListItem>
                                                                    <asp:ListItem Value="05" Text="Traslados de mercancias facturados previamente"></asp:ListItem>
                                                                    <asp:ListItem Value="06" Text="Factura generada por los traslados previos"></asp:ListItem>
                                                                    <asp:ListItem Value="07" Text="CFDI por aplicación de anticipo"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:LinkButton ID="bAgregarRelacionado" runat="server" CssClass="btn btn-primary btn-sm" OnClick="bAgregarRelacionado_Click" OnClientClick="return ValidarUuid();">
                     <i class="fa fa-plus" aria-hidden="true"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:GridView ID="gvCfdiRelacionados" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvConceptos_PageIndexChanging" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Uuid" HeaderText="UUID" />
                                                                        <asp:BoundField DataField="TipoRelacion" HeaderText="TIPO DE RELACIÓN" />
                                                                        <asp:TemplateField HeaderText="Eliminar" ShowHeader="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-primary btn-xs" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('¿Desea eliminar el registro?');" OnClick="lbDelete_Click">ELIMINAR</asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataRowStyle CssClass="table empty" />
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
                                    <div class="row" id="divAdicionales">
                                        <div class="col-md-12" style="text-align: left;">
                                            <asp:LinkButton ID="lbPrevisualizar" runat="server" CssClass="btn btn-default" OnClick="lbPrevisualizar_Click" Style="display: none;">Previsualizar Factura</asp:LinkButton>
                                            <asp:LinkButton ID="lbAddendas" runat="server" CssClass="btn btn-default" OnClick="lbAddendas_Click">Addendas</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="col-md-4">SUBTOTAL:</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbSubtotal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="tbSubtotal_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbSubtotal" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="tbSubtotal" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
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
                                    <div runat="server" id="trIUIProp" visible="false">
                                        <div class="col-md-4">
                                            IUI    
                                            <asp:Label ID="lblIUIPrer" runat="server" Text="X"></asp:Label>%:
                                        </div>
                                        <div class="col-md-8 ">
                                            <asp:TextBox ID="tbIUI" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div runat="server" id="rowTotalesArrendamiento" visible="false">
                                        <div class="col-md-4">RET IVA:</div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="tbRetIva" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">RET ISR:</div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="tbRetIsr" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div runat="server" id="divDescuentoTot" visible="true">
                                        <div class="col-md-4">DESCUENTO:</div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="tbDescuento" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0 *" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">TOTAL FAC.:</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbTotalFac" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotalFac" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbTotalFac" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                    </div>
                                    <div id="rowPropina" runat="server" visible="false">
                                        <div class="col-md-4">
                                            <% if (Session != null && Session["IDENTEMI"] != null && Session["IDENTEMI"].ToString().Equals("TCA130827M58"))
                                                { %>
                                    CARGO X S.:
                                    <% }
                                        else
                                        { %>
                                    PROPINA:
                                    <% } %>
                                        </div>
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
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbTotal" ValidChars=",."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="tbTotal" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationDocumento"></asp:RequiredFieldValidator>
                                    </div>
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

    <div class="modal fade " id="divHabitacion">
        <div class="modal-dialog">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Habitación</h4>
                </div>
                <div class="modal-body  rowsSpaced">
                    <div id="divH" runat="server">
                        <div class="row">
                            <div class="col-md-2">HUÉSPED:</div>
                            <div class="col-md-10">
                                <asp:TextBox ID="tbHuespedHabitacion" runat="server" Style="text-align: center" CssClass="form-control" placeholder="NOMBRE DEL HUÉSPED *"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">CHEQUE/RESERVACIÓN:</div>
                            <div class="col-md-8">
                                <asp:TextBox ID="tbReservacionHabitacion" runat="server" Style="text-align: center" CssClass="form-control" placeholder="# DE RESERVACIÓN *"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">HABITACIÓN:</div>
                            <div class="col-md-8">
                                <asp:TextBox ID="tbHabitacion" runat="server" Style="text-align: center" CssClass="form-control" placeholder="# DE HABITACIÓN *"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="tbHabitacion_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="tbHabitacion" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="row" id="rowTipoHabitacion" runat="server" style="display: none;">
                            <div class="col-md-4">TIPO HABITACIÓN:</div>
                            <div class="col-md-8">
                                <asp:TextBox ID="tbTipoHabitacion" runat="server" Style="text-align: center" CssClass="form-control" placeholder="TIPO DE HABITACIÓN *" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">LLEGADA:</div>
                            <div class="col-md-4">
                                <div class="input-group date">
                                    <asp:TextBox ID="tbFechaHabitacion1" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="11"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                                </div>
                            </div>
                            <div class="col-md-2">SALIDA:</div>
                            <div class="col-md-4">
                                <div class="input-group date">
                                    <asp:TextBox ID="tbFechaHabitacion2" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="11"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade " id="divEvento">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Evento</h4>
                </div>
                <div class="modal-body  rowsSpaced">
                    <div id="divE" runat="server">
                        <asp:Label ID="lbevent" runat="server" Text="EVENTO:"></asp:Label>
                        <asp:TextBox ID="tbHusespedEvento" runat="server" Style="text-align: center" CssClass="form-control" placeholder="NOMBRE DEL HUÉSPED *"></asp:TextBox>

                        <asp:Label runat="server" Text="CHEQUE/RESERVACIÓN:"></asp:Label>
                        <asp:TextBox ID="tbReservacionEvento" runat="server" Style="text-align: center" CssClass="form-control" placeholder="# DE RESERVACIÓN *"></asp:TextBox>

                        <asp:Label runat="server">FECHA:</asp:Label>
                        <div class="input-group date">
                            <asp:TextBox ID="tbFechaEvento" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="11"></asp:TextBox>
                            <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                        </div>
                        <br />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="divAddendas">
        <div class="modal-dialog modal-lg">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Addendas</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div id="divSinAddendas" runat="server" visible="true">
                                <div class="alert alert-info">No existen addendas para el contribuyente emisor</div>
                            </div>
                            <div id="divAddendasTlalnepantla" runat="server" visible="false">
                                <div class="panel panel-primary">
                                    <div class="panel panel-heading" style="background: #337ab7 !important">VECI</div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">TIPO SERVICIO</div>
                                            <div class="col-md-4">PROVEEDOR</div>
                                            <div class="col-md-4"># FACTURA</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="vci_ServiceType" runat="server" CssClass="form-control" Style="text-align: center">
                                                    <asp:ListItem Value="A" Text="AÉREO"></asp:ListItem>
                                                    <asp:ListItem Value="C" Text="CRUCERO"></asp:ListItem>
                                                    <asp:ListItem Value="G" Text="GASTOS DE GESTIÓN"></asp:ListItem>
                                                    <asp:ListItem Value="H" Text="HOTEL"></asp:ListItem>
                                                    <asp:ListItem Value="O" Text="OTROS"></asp:ListItem>
                                                    <asp:ListItem Value="P" Text="PAQUETE"></asp:ListItem>
                                                    <asp:ListItem Value="R" Text="FERROCARRIL"></asp:ListItem>
                                                    <asp:ListItem Value="S" Text="SEGURO"></asp:ListItem>
                                                    <asp:ListItem Value="T" Text="TRASLADO/EXCURSION"></asp:ListItem>
                                                    <asp:ListItem Value="V" Text="ALQUILER VEHÍCULO"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_idProvider" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_invoiceNumber" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">&nbsp;</div>
                                        <div class="row">
                                            <div class="col-md-4">FECHA EMISION</div>
                                            <div class="col-md-4">PASAJERO</div>
                                            <div class="col-md-4">CONFIRMACIÓN</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="input-group date">
                                                    <asp:TextBox ID="vci_issueDateInvoice" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="10"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_namePassanger" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="40"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_recordLoc" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="15"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">&nbsp;</div>
                                        <div class="row">
                                            <div class="col-md-4">FECHA INICIO</div>
                                            <div class="col-md-4">FECHA FIN</div>
                                            <div class="col-md-4">BONO</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="input-group date">
                                                    <asp:TextBox ID="vci_startDate" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="10"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="input-group date">
                                                    <asp:TextBox ID="vci_endtDate" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="10"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_bonus" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="16"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">&nbsp;</div>
                                        <div class="row">
                                            <div class="col-md-4">MONEDA</div>
                                            <div class="col-md-4">ISH</div>
                                            <div class="col-md-4">IMP. BASE</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_currency" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="5"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_taxXHostingServices" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_taxable" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">&nbsp;</div>
                                        <div class="row">
                                            <div class="col-md-4">IMP. EXENTO</div>
                                            <div class="col-md-4">% IVA</div>
                                            <div class="col-md-4">IMP. IVA</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_unTaxable" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_vatRate" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_vatAmount" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">&nbsp;</div>
                                        <div class="row">
                                            <div class="col-md-4">TOTAL</div>
                                            <div class="col-md-4">% RETENCIÓN</div>
                                            <div class="col-md-4">IMP. RETENCION</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_grossAmount" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_retencionPercibida" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="vci_importeRetenido" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divAddenasHmpEUM" runat="server" visible="false">
                                <div class="panel panel-primary">
                                    <div class="panel panel-heading" style="background: #337ab7 !important">EUM</div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">No. TRANSACCIÓN</div>
                                            <div class="col-md-4">ALVARÁN</div>
                                            <div class="col-md-4">ID PEDIDO</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="EUM_Transaction" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="EUM_Alvaran" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="EUM_Idpedido" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">&nbsp;</div>
                                        <div class="row">
                                            <div class="col-md-4">TIPO DE MONEDA</div>
                                            <div class="col-md-4">TIPO DE CAMBIO</div>
                                            <div class="col-md-4">IMPUESTO R.</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="EUM_typecurrency" runat="server" CssClass="form-control" Style="text-align: center">
                                                    <asp:ListItem Value="MXN" Text="MXN"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="EUM_tipocambio" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="5"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="EUM_impuesto" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="15"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divAddenasHmpSANMINA" runat="server" visible="false">
                                <div class="panel panel-primary">
                                    <div class="panel panel-heading" style="background: #337ab7 !important">SANMINA</div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">ORDEN DE COMPRA SAMINA</div>
                                            <div class="col-md-4">CORREO ELECTRONICO PROVEEDOR</div>
                                            <div class="col-md-4">NUMERO DE PROVEEDOR</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="SAMINA_ORDENCOMPRA" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="SAMINA_CORREOPROV" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="SAMINA_NUMEROPROV" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">&nbsp;</div>
                                        <div class="row">
                                            <div class="col-md-4">TIPO DE MONEDA</div>
                                            <div class="col-md-4">TIPO DE CAMBIO</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="SAMINA_MONEDA" runat="server" CssClass="form-control" Style="text-align: center">
                                                    <asp:ListItem Value="MXN" Text="MXN"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="SAMINA_CAMBIO" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="5"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divAddenasHmpANF" runat="server" visible="false">
                                <div class="panel panel-primary">
                                    <div class="panel panel-heading" style="background: #337ab7 !important">ANFITRIONES</div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">ORDEN DE COMPRA</div>
                                            <div class="col-md-4">CODIGO</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="ANF_CODIGO" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="ANF_ORDENCOMP" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divAddenasHmpZF" runat="server" visible="false">
                                <div class="panel panel-primary">
                                    <div class="panel panel-heading" style="background: #337ab7 !important">ZF</div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">No. ORDEN</div>
                                            <div class="col-md-4">CANTIDAD</div>
                                            <div class="col-md-4">VALOR UNITARIO</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="ZF_ORDEN" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="ZF_CANTIDAD" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>

                                            <div class="col-md-4">
                                                <asp:TextBox ID="ZF_VALORUNI" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">IMPORTE</div>
                                            <div class="col-md-4">TIPODE MONEDA</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="ZF_IMPORTE" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ZF_TIPOMONEDA" runat="server" CssClass="form-control" Style="text-align: center">
                                                    <asp:ListItem Value="MXN" Text="MXN"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divAddenasGENEZF" runat="server" visible="false">
                                <div class="panel panel-primary">
                                    <div class="panel panel-heading" style="background: #337ab7 !important">GENERAL</div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">TIPO DE MONEDA</div>
                                            <div class="col-md-4">TIPO CAMBIO</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="AGENE_MONEDA" runat="server" CssClass="form-control" Style="text-align: center">
                                                    <asp:ListItem Value="Peso Mexicano" Text="Peso Mexicano"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="AGENE_CAMBIO" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divAddendasGrupoReforma" runat="server" visible="false">
                                <div class="panel panel-primary">
                                    <div class="panel panel-heading" style="background: #337ab7 !important">GRUPO REFORMA</div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">INGRESO DEL MES:</div>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlMesEmision" runat="server" CssClass="form-control" Style="text-align: center">
                                                    <asp:ListItem Selected="True" Text="Seleccione..." Value=""></asp:ListItem>
                                                    <asp:ListItem Text="01: Enero" Value="01"></asp:ListItem>
                                                    <asp:ListItem Text="02: Febrero" Value="02"></asp:ListItem>
                                                    <asp:ListItem Text="03: Marzo" Value="03"></asp:ListItem>
                                                    <asp:ListItem Text="04: Abril" Value="04"></asp:ListItem>
                                                    <asp:ListItem Text="05: Mayo" Value="05"></asp:ListItem>
                                                    <asp:ListItem Text="06: Junio" Value="06"></asp:ListItem>
                                                    <asp:ListItem Text="07: Julio" Value="07"></asp:ListItem>
                                                    <asp:ListItem Text="08: Agosto" Value="08"></asp:ListItem>
                                                    <asp:ListItem Text="09: Septiembre" Value="09"></asp:ListItem>
                                                    <asp:ListItem Text="10: Octubre" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="11: Noviembre" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="12: Diciembre" Value="12"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
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

    <div id="divAduanasConcepto" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Información Aduanera</h4>
                </div>
                <div class="modal-body  rowsSpaced">
                    <asp:UpdatePanel ID="updAduanasConcepto" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-4">
                                    NÚMERO:
                               
                               
                                </div>
                                <div class="col-md-4">
                                    FECHA:
                               
                               
                                </div>
                                <div class="col-md-4">
                                    ADUANA:
                               
                               
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbNumAduanaConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="NÚMERO *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="tbNumAduanaConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationAdConc"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFechaAduanaConc" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="11"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="tbFechaAduanaConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationAdConc"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbAduanaConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="ADUANA *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:LinkButton ID="bAgregarAduanaC" runat="server" CssClass="btn btn-primary" OnClick="bAgregarAduanaC_Click" Text="Agregar Aduana" ValidationGroup="validationAdConc"></asp:LinkButton>
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                            <br />
                            <div>
                                <asp:GridView ID="gvAduanasConcepto" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvAduanasConcepto_PageIndexChanged" OnSelectedIndexChanged="gvAduanasConcepto_SelectedIndexChanged" OnPageIndexChanging="gvAduanasConcepto_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvAduanasConcepto_PreRender" GridLines="None" DataSourceID="SqlDataAduanerasConcTemp" DataKeyNames="idDetallesAduanaTemp" OnRowDeleted="gvAduanasConcepto_RowDeleted">
                                    <Columns>
                                        <asp:BoundField DataField="numero" HeaderText="NÚMERO">
                                            <ItemStyle Width="20%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fecha" HeaderText="FECHA">
                                            <ItemStyle Width="35%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="aduana" HeaderText="ADUANA">
                                            <ItemStyle Width="35%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton5" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataAduanerasConcTemp" runat="server" SelectCommand="SELECT idDetallesAduanaTemp, numero, fecha, aduana FROM Dat_MX_DetallesAduanaTemp WHERE (id_Empleado = @idUser AND (IsNull(id_DetallesTemp, '') = '') AND (IsNull(id_DetallesParteTemp, '') = '') AND tipo = 1 AND SessionId=@SessionId)" DeleteCommand="DELETE FROM Dat_MX_DetallesAduanaTemp WHERE (idDetallesAduanaTemp = @idDetallesAduanaTemp)">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                        <asp:Parameter Name="SessionId" DefaultValue="" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="idDetallesAduanaTemp" />
                                    </DeleteParameters>
                                </asp:SqlDataSource>
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

    <div id="divPartesConcepto" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Partes</h4>
                </div>
                <div class="modal-body rowsSpaced">
                    <asp:UpdatePanel ID="updPartesConcepto" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-1">
                                    CANTIDAD:


                               
                               
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbCantidadParteConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción" AutoPostBack="true" OnTextChanged="CalculaImporteConceptoParte"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbCantidadParteConc_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbCantidadParteConc" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="tbCantidadParteConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationParteConc"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    UNIDAD:


                               
                               
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbUnidadParteConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="UNIDAD *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="tbUnidadParteConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationParteConc"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2" align="justified">
                                    <asp:Label ID="Label3" runat="server" Text="IDENTIFICACION:"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbIdentParteConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="IDENTIFICACIÓN *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1">
                                    DESCRIPCIÓN:


                               
                               
                                </div>
                                <div class="col-md-11">
                                    <asp:TextBox ID="tbDescParteConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="DESCRIPCIÓN *" Style="text-align: center;" ToolTip="Descripción" Width="95%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="tbDescParteConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationParteConc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">
                                    VAL. UNITARIO:


                               
                               
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbVUParteConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción" AutoPostBack="true" OnTextChanged="CalculaImporteConceptoParte"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbVUParteConc_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbVUParteConc" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="tbVUParteConc" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationParteConc"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    IMPORTE:


                               
                               
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbImpParteConc" runat="server" CausesValidation="True" CssClass="form-control" placeholder="00.00 *" Style="text-align: center;" ToolTip="Descripción" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-4">
                                    <asp:Button runat="server" Text="Información Aduanera" OnClientClick="javascript:verAduanerasParte(); return false;" CssClass="btn btn-primary" UseSubmitBehavior="false" />
                                </div>
                                <div class="col-md-4">
                                    <asp:LinkButton ID="bAgregarParteConcepto" runat="server" CssClass="btn btn-primary" OnClick="bAgregarParteConcepto_Click" Text="Agregar Parte" ValidationGroup="validationParteConc"></asp:LinkButton>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                            <hr />
                            <asp:GridView ID="gvPartes" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvPartes_PageIndexChanged" OnSelectedIndexChanged="gvPartes_SelectedIndexChanged" OnPageIndexChanging="gvPartes_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvPartes_PreRender" GridLines="None" DataSourceID="SqlDataPartes" DataKeyNames="idDetallesParteTemp" OnRowDeleted="gvPartes_RowDeleted">
                                <Columns>
                                    <asp:BoundField DataField="noIdentificacion" HeaderText="IDENTIFICACIÓN">
                                        <ItemStyle Width="20%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN">
                                        <ItemStyle Width="35%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="valorUnitario" HeaderText="VAL. UNITARIO">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="importe" HeaderText="IMPORTE">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="unidad" HeaderText="UNIDAD">
                                        <ItemStyle Width="15%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton6" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataPartes" runat="server" SelectCommand="SELECT idDetallesParteTemp, cantidad, unidad, noIdentificacion, descripcion, valorUnitario, importe FROM Dat_MX_DetallesParteTemp WHERE (id_Empleado = @idUser AND SessionId=@SessionId AND SessionId=@SessionId AND (IsNull(id_DetallesTemp, '') = ''))" DeleteCommand="DELETE FROM Dat_MX_DetallesParteTemp WHERE (idDetallesParteTemp = @idDetallesParteTemp)">
                                <SelectParameters>
                                    <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                    <asp:Parameter Name="SessionId" DefaultValue="" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="idDetallesParteTemp" />
                                </DeleteParameters>
                            </asp:SqlDataSource>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div id="divAduanasParte" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Información Aduanera</h4>
                </div>
                <div class="modal-body  rowsSpaced">
                    <asp:UpdatePanel ID="updAduanasParte" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-4">
                                    NÚMERO:

                               
                               
                                </div>
                                <div class="col-md-4">
                                    FECHA:

                               
                               
                                </div>
                                <div class="col-md-4">
                                    ADUANA:

                               
                               
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbNumAduanaParte" runat="server" CausesValidation="True" CssClass="form-control" placeholder="NÚMERO *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ControlToValidate="tbNumAduanaParte" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationAdParteConc"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFechaAduanaParte" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="11"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ControlToValidate="tbFechaAduanaParte" Display="None" ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="validationAdParteConc"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbAduanaParte" runat="server" CausesValidation="True" CssClass="form-control" placeholder="ADUANA *" Style="text-align: center;" ToolTip="Descripción"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:LinkButton ID="bAgregarAduanaParte" runat="server" CssClass="btn btn-primary" OnClick="bAgregarAduanaParte_Click" Text="Agregar Aduana" ValidationGroup="validationAdParteConc"></asp:LinkButton>
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                            <br />
                            <div>
                                <asp:GridView ID="gvAduanaPartesTemp" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" OnPageIndexChanged="gvAduanaPartesTemp_PageIndexChanged" OnSelectedIndexChanged="gvAduanaPartesTemp_SelectedIndexChanged" OnPageIndexChanging="gvAduanaPartesTemp_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvAduanaPartesTemp_PreRender" GridLines="None" DataSourceID="SqlDataAduanerasParteTemp" DataKeyNames="idDetallesAduanaTemp">
                                    <Columns>
                                        <asp:BoundField DataField="numero" HeaderText="NÚMERO">
                                            <ItemStyle Width="20%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fecha" HeaderText="FECHA">
                                            <ItemStyle Width="35%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="aduana" HeaderText="ADUANA">
                                            <ItemStyle Width="35%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton7" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataAduanerasParteTemp" runat="server" SelectCommand="SELECT idDetallesAduanaTemp, numero, fecha, aduana FROM Dat_MX_DetallesAduanaTemp WHERE (id_Empleado = @idUser AND SessionId=@SessionId AND (IsNull(id_DetallesTemp, '') = '') AND (IsNull(id_DetallesParteTemp, '') = '') AND tipo = 2)" DeleteCommand="DELETE FROM Dat_MX_DetallesAduanaTemp WHERE (idDetallesAduanaTemp = @idDetallesAduanaTemp)">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                        <asp:Parameter Name="SessionId" DefaultValue="" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="idDetallesAduanaTemp" />
                                    </DeleteParameters>
                                </asp:SqlDataSource>
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
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    FACTURA
</asp:Content>
