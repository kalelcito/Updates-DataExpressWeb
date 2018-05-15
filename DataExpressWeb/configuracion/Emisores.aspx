<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Emisores.aspx.cs" Inherits="DataExpressWeb.Emisores" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        
    </style>
    <script type="text/javascript">
        function FileUploadStarted(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            if (ext != 'jpg' && ext != 'JPG' && ext != 'png' && ext != 'PNG') {
                var err = new Error();
                err.message = 'Solo se aceptan archivos con extensiones ".jpg" y ".png"';
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
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="row">
                <div align="center">
                    <asp:Button ID="bBus" runat="server" OnClientClick="javascript:$('#divBuscar').modal('toggle'); return false;" CssClass="btn btn-primary" Text="Filtros de Búsqueda" UseSubmitBehavior="False" />
                    <asp:Button ID="bLimpiarBus" runat="server" Text="Limpiar Filtros" CssClass="btn btn-primary" UseSubmitBehavior="False" OnClick="bLimpiarBus_Click" />
                    <asp:Button ID="bNuevo" runat="server" OnClick="bNuevo_Click" CssClass="btn btn-primary" Text="Nuevo Emisor" />
                    <br />
                </div>
            </div>
            <asp:Label ID="lCount" runat="server" Text="Registros" Visible="False"></asp:Label>
            <br />
            <div align="center">
                <asp:GridView ID="gvEmisor" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Smaller" GridLines="None" DataSourceID="SqlDataEmisores" DataKeyNames="IDEEMI">
                    <Columns>
                        <asp:BoundField DataField="RFCEMI" HeaderText="RFC" SortExpression="RFCEMI" />
                        <asp:BoundField DataField="NOMEMI" HeaderText="NOMBRE" SortExpression="NOMEMI" />
                        <asp:BoundField DataField="curp" HeaderText="CURP" SortExpression="curp" Visible="false" />
                        <asp:BoundField DataField="regimenFiscal" HeaderText="REG. FISCAL" SortExpression="regimenFiscal" />
                        <asp:BoundField DataField="pais" HeaderText="PAIS" SortExpression="pais" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%#Bind("IDEEMI") %>' OnClick="bDetalles_Click"></asp:LinkButton>
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
                <asp:SqlDataSource ID="SqlDataEmisores" runat="server" SelectCommand="SELECT IDEEMI,RFCEMI,NOMEMI,curp,regimenFiscal,pais FROM Cat_Emisor" DeleteCommand="DELETE FROM Cat_Emisor WHERE IDEEMI = @IDEEMI; delete from dat_general where id_emisor = @IDEEMI">
                    <DeleteParameters>
                        <asp:Parameter Name="IDEEMI" />
                    </DeleteParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="divBuscar" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Buscar Emisor</h4>
                </div>
                <div class="modal-body rowsSpaced">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div align="center">
                                <div class="row">
                                    <div class="col-md-4">
                                        RFC:<asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                            runat="server"
                                            ControlToValidate="tbRfcBus"
                                            ErrorMessage="*"
                                            ForeColor="Red"
                                            SetFocusOnError="True"
                                            ValidationGroup="ValidationRfc"
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
                                <div class="row">
                                    <div class="col-md-4">DIRECCIÓN: </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="tbDireccBus" runat="server" CssClass="form-control" placeholder="DIRECCIÓN *" Style="text-align: center;"></asp:TextBox>
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
                                        <asp:Button ID="bBuscar" runat="server" OnClick="bBuscar_Click" CssClass="btn btn-primary" Text="Buscar" ValidationGroup="ValidationRfc" />
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
                    <h4 class="modal-title ">Agregar/Editar Emisor</h4>
                </div>
                <div class="modal-body ">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <table class=" table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">

                                <tr align="center">
                                    <td align="center" colspan="2"></td>
                                    <td align="right" colspan="2">
                                        <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" Visible="false" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">RFC:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRfcEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>

                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                            runat="server"
                                            ControlToValidate="tbRfcEmi"
                                            ErrorMessage="*"
                                            ForeColor="Red"
                                            SetFocusOnError="True"
                                            ValidationGroup="validationEmisor"
                                            ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbRfcEmi" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">RAZÓN SOCIAL:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbNomEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbNomEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">CURP:</td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbCURPE" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CURP *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">E-MAIL:</td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbMailEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="E-MAIL *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">TELÉFONO:</td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbTelEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="TELÉFONO"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">RÉGIMEN FISCAL:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbRegimenFiscal" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisor"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbRegimenFiscal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RÉGIMEN FISCAL *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" id="trConta" runat="server" visible="false">
                                    <td align="center" class="col-md-3">OBLIGADO A CONTABILIDAD</td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:DropDownList ID="ddlObligado" runat="server" Style="text-align: center" CssClass="form-control">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="SI">SI</asp:ListItem>
                                            <asp:ListItem Value="NO">NO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%-- <tr align="center" id="trGiro" runat="server">
                                    <td align="center" class="col-md-3">GIRO EMPRESARIAL:</td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:DropDownList ID="ddlTEmp" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSource2" DataTextField="Descripcion" DataValueField="codigo" AutoPostBack="True" AppendDataBoundItems="True">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr align="center">
                                    <td align="center" class="col-md-3">LOGO (.JPG):</td>
                                    <td align="left" class="col-md-9" colspan="3">
                                        <asp:Image ID="Image1" runat="server" Height="43px" Width="110px" />
                                        &nbsp;<span class="style13">110px/43px</span>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3"></td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="FileUploadStarted" runat="server" ID="fileLogo" OnUploadedComplete="fileLogo_UploadedComplete" accept="image/jpeg, image/jpg, image/png" />
                                    </td>
                                </tr>
                                <tr align="center" id="trColorPdf01" runat="server" visible="true">
                                    <td align="center" class="col-md-3">COLOR PDF FACTURA:</td>
                                    <td align="left" class="col-md-9" colspan="3">
                                        <div class="input-group colorpicker colorpicker-component">
                                            <asp:TextBox ID="tbColorPdf01" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            <span class="input-group-addon"><i></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center" id="trColorPdf04" runat="server" visible="true">
                                    <td align="center" class="col-md-3">COLOR PDF NOTA CRÉDITO:</td>
                                    <td align="left" class="col-md-9" colspan="3">
                                        <div class="input-group colorpicker colorpicker-component">
                                            <asp:TextBox ID="tbColorPdf04" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            <span class="input-group-addon"><i></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center" id="trColorPdf06" runat="server" visible="true">
                                    <td align="center" class="col-md-3">COLOR PDF CARTA PORTE:</td>
                                    <td align="left" class="col-md-9" colspan="3">
                                        <div class="input-group colorpicker colorpicker-component">
                                            <asp:TextBox ID="tbColorPdf06" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            <span class="input-group-addon"><i></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center" id="trColorPdf07" runat="server" visible="true">
                                    <td align="center" class="col-md-3">COLOR PDF RETENCIONES:</td>
                                    <td align="left" class="col-md-9" colspan="3">
                                        <div class="input-group colorpicker colorpicker-component">
                                            <asp:TextBox ID="tbColorPdf07" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            <span class="input-group-addon"><i></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center" id="trColorPdf08" runat="server" visible="true">
                                    <td align="center" class="col-md-3">COLOR PDF NÓMINA:</td>
                                    <td align="left" class="col-md-9" colspan="3">
                                        <div class="input-group colorpicker colorpicker-component">
                                            <asp:TextBox ID="tbColorPdf08" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            <span class="input-group-addon"><i></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center" id="trColorPdf09" runat="server" visible="true">
                                    <td align="center" class="col-md-3">COLOR PDF CONTABILIDAD:</td>
                                    <td align="left" class="col-md-9" colspan="3">
                                        <div class="input-group colorpicker colorpicker-component">
                                            <asp:TextBox ID="tbColorPdf09" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                            <span class="input-group-addon"><i></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" colspan="4">
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
                                                        <table class=" table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">CALLE:
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbCalleEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbCalleEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">NO. EXTERIOR:</td>
                                                                <td align="center" class="col-md-3">
                                                                    <asp:TextBox ID="tbNoExtEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td align="center" class="col-md-3">NO. INTERIOR:</td>
                                                                <td align="center" class="col-md-3">
                                                                    <asp:TextBox ID="tbNoIntEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">COLONIA:</td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbColoniaEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">LOCALIDAD:</td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbLocEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LOCALIDAD *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">REFERENCIA:</td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbRefEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="REFERENCIA *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">MUNICIPIO:
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="tbMunicipioEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbMunicipioEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">ESTADO:
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="tbEstadoEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="center" class="col-md-3">
                                                                    <asp:TextBox ID="tbEstadoEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td align="center" class="col-md-3">PAÍS:
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="tbPaisEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="center" class="col-md-3">
                                                                    <asp:TextBox ID="tbPaisEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="col-md-3">CODIGO POSTAL:
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="tbCpEmi" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationEmisorDom" Enabled="False"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="center" class="col-md-9" colspan="3">
                                                                    <asp:TextBox ID="tbCpEmi" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" ReadOnly="True" MaxLength="5"></asp:TextBox>
                                                                    <asp:MaskedEditExtender ID="tbCpEmi_MaskedEditExtender" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpEmi" Mask="99999" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" colspan="4">
                                        <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" ValidationGroup="validationEmisor"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" SelectCommand="SELECT DISTINCT Descripcion, codigo FROM Cat_Catalogo1_C WHERE (tipo = 'EmpresaTipo') AND (codigo = no)"></asp:SqlDataSource>
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

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cpTitulo">
    EMISORES
</asp:Content>
