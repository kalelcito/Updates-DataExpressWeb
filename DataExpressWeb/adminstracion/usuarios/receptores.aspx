<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="receptores.aspx.cs" Inherits="Administracion.Receptores" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        hr {
            border: 0;
            box-shadow: 0 0 10px 1px black;
            height: 0; /* Firefox... */
        }

            hr:after { /* Not really supposed to work, but does */
                content: "\00a0"; /* Prevent margin collapse */
            }
    </style>
    <script type="text/javascript">
        function fileXlsx_FileUploadStarted(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            if (ext != 'xlsx' && ext != 'XLSX') {
                var err = new Error();
                err.message = 'Solo se aceptan archivos con extensiones ".xlsx"';
                throw (err);
                return false;
            }
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    <asp:Label ID="lblTitulo" runat="server" Text="RECEPTORES"></asp:Label>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="rowsSpaced">
                <div class="row">
                    <div class="col-md-12" align="center">
                        <asp:Button ID="bBus" runat="server" OnClientClick="javascript:$('#divBuscar').modal('toggle'); return false;" CssClass="btn btn-primary" Text="Filtros de Búsqueda" UseSubmitBehavior="False" />
                        <asp:Button ID="bLimpiarBus" runat="server" Text="Limpiar Filtros" CssClass="btn btn-primary" UseSubmitBehavior="False" OnClick="bLimpiarBus_Click" />
                        <asp:Button ID="bNuevo" runat="server" OnClick="bNuevo_Click" CssClass="btn btn-primary" Text="Nuevo Receptor" />
                        <asp:Button ID="bLoadXlsx" runat="server" OnClick="bLoadXlsx_Click" CssClass="btn btn-primary" Text="Cargar Excel" Visible="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8">
                        <asp:Label ID="lCount" runat="server" Text="Registros" Visible="False"></asp:Label>
                    </div>
                    <div class="col-md-2" align="right">
                        <asp:CheckBox ID="chkRepetidos" runat="server" Text="Mostrar repetidos" OnCheckedChanged="chkRepetidos_CheckedChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="row" align="center">
                    <div class="col-md-12">
                        <asp:GridView ID="gvReceptor" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" OnPageIndexChanged="gvReceptor_PageIndexChanged" OnSelectedIndexChanged="gvReceptor_SelectedIndexChanged" OnPageIndexChanging="gvReceptor_PageIndexChanging" PageSize="10" BackColor="White" AllowPaging="True" Font-Size="Smaller" OnPreRender="gvReceptor_PreRender" GridLines="None" DataSourceID="SqlDataReceptores" DataKeyNames="IDEREC" OnRowDeleted="gvReceptor_RowDeleted" OnRowDeleting="gvReceptor_RowDeleting" OnDataBound="gvReceptor_DataBound" AllowSorting="true">
                            <Columns>
                                <asp:BoundField DataField="RFCREC" HeaderText="RFC" SortExpression="RFCREC" />
                                <asp:BoundField DataField="NOMREC" HeaderText="NOMBRE" SortExpression="NOMREC" />
                                <asp:BoundField DataField="denominacionSocial" HeaderText="DEN. SOCIAL" SortExpression="denominacionSocial" />
                                <asp:BoundField DataField="curp" HeaderText="CURP" SortExpression="curp" ReadOnly="True" />
                                <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" SortExpression="domicilio" />
                                <%--<asp:BoundField DataField="noExterior" HeaderText="NO. EXT" SortExpression="noExterior" />
                        <asp:BoundField DataField="noInterior" HeaderText="NO. INT" SortExpression="noInterior" />
                        <asp:BoundField DataField="colonia" HeaderText="COLONIA" SortExpression="colonia" />
                        <asp:BoundField DataField="municipio" HeaderText="MUNICIPIO" SortExpression="municipio" />--%>
                                <asp:BoundField DataField="estado" HeaderText="ESTADO" SortExpression="estado" />
                                <asp:BoundField DataField="pais" HeaderText="PAIS" SortExpression="pais" />
                                <asp:BoundField DataField="codigoPostal" HeaderText="C.P." SortExpression="codigoPostal" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%#Bind("IDEREC") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="bEliminar" runat="server" CssClass="btn btn-primary btn-sm" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                            <FooterStyle />
                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                            <RowStyle />
                            <SelectedRowStyle />
                            <SortedAscendingCellStyle />
                            <SortedAscendingHeaderStyle />
                            <SortedDescendingCellStyle />
                            <SortedDescendingHeaderStyle />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataReceptores" runat="server" SelectCommand="SELECT 
                   [IDEREC]
                  ,[RFCREC]
                  ,[NOMREC]
                  ,[domicilio]
                  ,[noExterior]
                  ,[noInterior]
                  ,[colonia]
                  ,[municipio]
                  ,[estado]
                  ,[pais]
                  ,[codigoPostal]
                  ,[curp]
                  ,[denominacionSocial]
              FROM [Cat_Receptor]"
                            DeleteCommand="DELETE FROM Cat_Receptor WHERE IDEREC = @IDEREC" OnSelected="SqlDataReceptores_Selected">
                            <DeleteParameters>
                                <asp:Parameter Name="IDEREC" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="divBuscar" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">
                        <asp:Label ID="lblBuscarReceptor" runat="server" Text="Buscar Receptor"></asp:Label></h4>
                </div>
                <div class="modal-body rowsSpaced">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div align="center">
                                <div class="row">
                                    <div class="col-md-4">
                                        RFC:<asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                            runat="server"
                                            ControlToValidate="tbRfcBus"
                                            ErrorMessage="*"
                                            ForeColor="Red"
                                            SetFocusOnError="True"
                                            ValidationGroup="validationRfc"
                                            ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbRfcBus" runat="server" CssClass="form-control" placeholder="RFC *" Style="text-align: center;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">RAZÓN SOCIAL: </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbNomBus" runat="server" CssClass="form-control" placeholder="RAZÓN SOCIAL *" Style="text-align: center;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row" id="rowDenomBus" runat="server" visible="true">
                                    <div class="col-md-4">DENOMINACIÓN SOCIAL: </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbDenBus" runat="server" CssClass="form-control" placeholder="DENOMINACIÓN SOCIAL *" Style="text-align: center;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">DIRECCIÓN: </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbDireccBus" runat="server" CssClass="form-control" placeholder="DIRECCIÓN *" Style="text-align: center;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">CONTACTO: </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbContactBus" runat="server" CssClass="form-control" placeholder="CONTACTO *" Style="text-align: center;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">CORREO: </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbMailBus" runat="server" CssClass="form-control" placeholder="CORREO *" Style="text-align: center;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4">
                                        <asp:Button ID="bBuscar" runat="server" OnClick="bBuscarReg_Click" CssClass="btn btn-primary" Text="Buscar" ValidationGroup="validationRfc" />
                                    </div>
                                    <div class="col-md-4"></div>
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

    <div id="divNuevo" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">
                        <asp:Label ID="lblAgregarReceptor" runat="server" Text="Agregar/Editar Receptor"></asp:Label></h4>
                </div>
                <div class="modal-body ">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <table class="table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">
                                <tr align="center">
                                    <td align="center" colspan="3"></td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" Visible="false" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">RFC:

        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="tbRfcRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                            runat="server"
                                            ControlToValidate="tbRfcRec"
                                            ErrorMessage="*"
                                            ForeColor="Red"
                                            SetFocusOnError="True"
                                            ValidationGroup="validationReceptor"
                                            ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbRfcRec" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="0000000000001 *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">RAZÓN SOCIAL:

        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tbRazonSocialRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbRazonSocialRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" runat="server" id="trDenom" visible="true">
                                    <td align="center" class="col-md-3">DENOMINACIÓN SOCIAL:
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbDenomSocialRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" runat="server" id="trCurp" visible="true">
                                    <td align="center" class="col-md-3">CURP:</td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbCURPR" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CURP *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">E-MAIL:</td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbMailRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="E-MAIL *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">TELÉFONO 1:</td>
                                    <td align="center" class="col-md-3">
                                        <asp:TextBox ID="tbTelRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                    </td>
                                    <td align="center" class="col-md-3">TELÉFONO 2:</td>
                                    <td align="center" class="col-md-3">
                                        <asp:TextBox ID="tbTelRec2" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3"><% if (Session["CfdiVersion"].ToString().Equals("3.3"))
                                                                            { %>
                                            FORMA DE PAGO:
                                            <% }
                                                else
                                                { %>
                                            MÉTODO DE PAGO:
                                            <% } %>
                                    </td>
                                    <td align="center" class="col-md-3">
                                        <asp:DropDownList ID="ddlMetodoPago" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataMetodoPago" DataTextField="descripcion" DataValueField="codigo" AutoPostBack="true" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataMetodoPago" runat="server" SelectCommand="SELECT '' AS codigo, 'Seleccione' AS descripcion UNION SELECT codigo, descripcion FROM Cat_Catalogo1_C WHERE tipo = '@tipoCatalogo';"></asp:SqlDataSource>
                                    </td>
                                    <td align="center" class="col-md-3">NUM. CUENTA:</td>
                                    <td align="center" class="col-md-3">
                                        <asp:TextBox ID="tbNumCta" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NUM. CUENTA *"></asp:TextBox>
                                        <asp:MaskedEditExtender runat="server" ID="tbNumCta_MaskedEditExtender" TargetControlID="tbNumCta" ClearMaskOnLostFocus="false" MaskType="None" Mask="9999" Enabled="false" />
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" colspan="4">
                                        <div class="panel-group" id="accordion1" role="tablist" aria-multiselectable="true">
                                            <div class="panel panel-default">
                                                <div class="panel-heading" role="tab" id="heading1One">
                                                    <h4 class="panel-title">
                                                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse1One" aria-expanded="true" aria-controls="collapse1One">DIRECCIÓN FISCAL
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div id="collapse1One" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading1One">
                                                    <div class="panel-body">
                                                        <table class=" table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">CALLE:

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tbCalleRec" ErrorMessage="*" Style="color: #FF0000" Enabled="False"></asp:RequiredFieldValidator>
                                                                </td>
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
                                                                <td align="center" class="col-md-3">ESTADO:

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbEstadoRec" ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="center" class="col-md-3">
                                                                    <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *"></asp:TextBox>
                                                                </td>
                                                                <td align="center" class="col-md-3">PAÍS:

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="tbPaisRec" ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="center" class="col-md-3">
                                                                    <asp:TextBox ID="tbPaisRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">CODIGO POSTAL:

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="tbCpRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbCpRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" MaxLength="5"></asp:TextBox>
                                                                    <asp:MaskedEditExtender ID="tbCpRec_MaskedEditExtender" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpRec" Mask="99999" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="panel panel-default">
                                                <div class="panel-heading" role="tab" id="heading1Two">
                                                    <h4 class="panel-title">
                                                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse1Two" aria-expanded="true" aria-controls="collapse1Two">CONTACTOS
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div id="collapse1Two" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading1One">
                                                    <div class="panel-body">
                                                        <table class=" table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">NOMBRE:</td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbNomContRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NOMBRE *"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">TELÉFONO 1:</td>
                                                                <td align="center" class="col-md-3">
                                                                    <asp:TextBox ID="tbTelContRec1" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                                                </td>
                                                                <td align="center" class="col-md-3">TELÉFONO 2:</td>
                                                                <td align="center" class="col-md-3">
                                                                    <asp:TextBox ID="tbTelContRec2" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">PUESTO:</td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbPuestoContRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PUESTO *"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">E-MAIL:</td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbMailContRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="E-MAIL *"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" colspan="4" class="col-md-12">
                                                                    <asp:LinkButton ID="bAgregarContacto" runat="server" CssClass="btn btn-primary" OnClick="bAgregarContacto_Click" Text="Agregar Contacto"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <hr />
                                                        <asp:GridView ID="gvContactos" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" OnPageIndexChanged="gvContactos_PageIndexChanged" OnSelectedIndexChanged="gvContactos_SelectedIndexChanged" OnPageIndexChanging="gvContactos_PageIndexChanging" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" OnPreRender="gvContactos_PreRender" GridLines="None" DataSourceID="SqlDataContactos" DataKeyNames="idContactoTemp">
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
                                                                        <asp:LinkButton ID="bEliminarContacto" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                        <asp:SqlDataSource ID="SqlDataContactos" runat="server" SelectCommand="SELECT idContactoTemp, nombre, puesto, telefono1, telefono2, correo FROM Cat_Mx_Contactos_Temp WHERE (id_Empleado = @idUser)" DeleteCommand="DELETE FROM Cat_Mx_Contactos_Temp WHERE (idContactoTemp = @idContactoTemp)">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="idUser" SessionField="idUser" />
                                                            </SelectParameters>
                                                            <DeleteParameters>
                                                                <asp:Parameter Name="idContactoTemp" />
                                                            </DeleteParameters>
                                                        </asp:SqlDataSource>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" colspan="4">
                                        <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" ValidationGroup="validationReceptor"></asp:LinkButton>
                                    </td>
                                </tr>
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
    <div id="divLoadXlsx" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Cargar archivo de catálogo</h4>
                        </div>
                        <div class="modal-body" style="text-align: left;">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="fileXlsx_FileUploadStarted" runat="server" ID="fileXlsx" OnUploadedComplete="fileXlsx_UploadedComplete" accept="image/jpeg, image/jpg, image/png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="bs-callout bs-callout-info">
                                        <h4>Nota:</h4>
                                        <p>El catálogo debe ser un archivo de Excel en formato 2007 o posterior (con extensión <code>.xlsx</code>), conteniendo las siguientes columnas:</p>
                                        <ul>
                                            <li>RFC</li>
                                            <li>RAZON SOCIAL</li>
                                            <li>E-MAIL</li>
                                            <li>CALLE</li>
                                            <li>TELEFONO</li>
                                            <li>NO EXTERIOR</li>
                                            <li>NO INTERIOR</li>
                                            <li>COLONIA</li>
                                            <li>LOCALIDAD</li>
                                            <li>REFERENCIA</li>
                                            <li>MUNICIPIO</li>
                                            <li>ESTADO</li>
                                            <li>PAIS</li>
                                            <li>CODIGO POSTAL</li>
                                            <li>CURP</li>
                                            <li>TELEFONO</li>
                                            <li>METODO PAGO</li>
                                            <li>NUMERO CUENTA</li>
                                            <li>NUMREGIDTRIB</li>
                                        </ul>
                                        <asp:LinkButton ID="lbDownloadExample" runat="server" CausesValidation="false" ValidateRequestMode="Disabled" OnClick="lbDownloadExample_Click">
                                            <span class="glyphicon glyphicon-save" aria-hidden="true"></span>&nbsp;Puede descargar un ejemplo del catálogo haciendo click aquí</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbCargarXlsx" runat="server" CssClass="btn btn-primary" OnClick="lbCargarXlsx_Click">Cargar</asp:LinkButton>
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>
