<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="sucursalesEmisor.aspx.cs" Inherits="Administracion.SucursalesEmisor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
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
    <div align="center">
        <div class="row">
            <div class="col-md-1"></div>
            <div class="col-md-2">
                <strong>RFC:</strong>
            </div>
            <div class="col-md-2"><strong>CLAVE:</strong></div>
            <div class="col-md-6"><strong>SUCURSAL:</strong></div>
            <div class="col-md-1"></div>
        </div>
        <div class="row">
            <div class="col-md-1"></div>
            <div class="col-md-2">
                <asp:TextBox ID="tbRfcRec" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="0000000000001 *"></asp:TextBox>
                <asp:AutoCompleteExtender ID="tbRfcRec_AutoCompleteExtender" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcRec" UseContextKey="True" MinimumPrefixLength="1">
                </asp:AutoCompleteExtender>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="tbClave" MaxLength="3" runat="server" Style="text-align: center" CssClass="form-control" placeholder="001 *"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbClave" FilterType="Numbers"></asp:FilteredTextBoxExtender>
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="tbSucursal" MaxLength="100" runat="server" Style="text-align: center" CssClass="form-control" placeholder="Establecimiento *"></asp:TextBox>
            </div>
            <div class="col-md-1"></div>
        </div>
        <div class="row">
            <div class="col-md-5"></div>
            <div class="col-md-2"><strong>DIRECCIÓN:</strong></div>
            <div class="col-md-5"></div>
        </div>
        <div class="row">
            <div class="col-md-1"></div>
            <div class="col-md-10">
                <asp:TextBox ID="tbDomicilio" MaxLength="150" runat="server" Style="text-align: center" CssClass="form-control" placeholder="Domicilio *"></asp:TextBox>
            </div>
            <div class="col-md-1"></div>
        </div>
    </div>
    <div align="center">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <br />
                <asp:Button ID="bBuscarReg" runat="server" OnClick="bBuscarReg_Click" CssClass="btn btn-primary" Text="Buscar" ValidationGroup="valRfc" />
                <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" CssClass="btn btn-primary" Text="Nueva Busqueda" />
                <asp:Button ID="bNuevo" runat="server" CssClass="btn btn-primary" OnClick="bNuevo_Click" Text="Agregar"></asp:Button>
                <asp:Label ID="lMensaje" runat="server" Style="color: #FF0000"></asp:Label>
                <br />
                <br />
                <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover" BackColor="White" DataKeyNames="idSucursal" DataSourceID="SqlDataSource1" GridLines="None" AllowPaging="True" OnSelectedIndexChanged="grid_sucursales_SelectedIndexChanged" OnPageIndexChanged="grid_sucursales_PageIndexChanged" Font-Size="Small">
                    <Columns>
                        <%-- <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/imagenes/icono_adelante.gif">
            </asp:CommandField>--%>
                        <asp:BoundField DataField="RFC" HeaderText="RFC" ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField DataField="clave" HeaderText="CLAVE" ItemStyle-Width="5%"></asp:BoundField>
                        <asp:BoundField DataField="sucursal" HeaderText="SUCURSAL" SortExpression="sucursal" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" SortExpression="domicilio" ItemStyle-Width="55%" />
                        <asp:TemplateField ShowHeader="False" HeaderText="ACCIÓN">
                            <ItemTemplate>
                                <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%#Bind("idSucursal") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Si eliminas el Establecimiento se borraran todos los datos que contengan. ¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar" CssClass="btn btn-primary btn-sm"></asp:LinkButton>
                            </ItemTemplate>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="PA_Busqueda_SucursalesEmisor" DeleteCommand="UPDATE Cat_SucursalesEmisor SET eliminado='True' WHERE (idSucursal = @idSucursal) UPDATE Cat_Empleados SET eliminado='true' WHERE (id_Sucursal=@idSucursal)" SelectCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="idSucursal" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <div id="divNuevo" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Agregar/Editar Sucursal</h4>
                </div>
                <div class="modal-body rowsSpaced">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-9"></div>
                                <div class="col-md-3">
                                    <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" Visible="false" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-1">
                                    RFC:
                                </div>
                                <div class="col-md-5">
                                    <asp:TextBox ID="tbRfcRecNuevo" runat="server" CssClass="form-control" MaxLength="13" Style="text-align: center" placeholder="0000000000001 *"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcRecNuevo" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">CLAVE:</div>
                                <div class="col-md-5">NOMBRE SUC.:</div>
                                <div class="col-md-4">TELÉFONO:</div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbClaveNuevo" runat="server" placeholder="001 *" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbClaveNuevo" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationSucursal"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-5">
                                    <asp:TextBox ID="tbSucursalNuevo" runat="server" Style="text-align: center" CssClass="form-control" placeholder="Nombre del Establecimiento *" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbSucursalNuevo" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationSucursal"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbTelNuevo" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    LOGO:
                                </div>
                                <div class="col-md-9">
                                    <asp:Image ID="Image1" runat="server" Height="47px" Width="145px" />
                                    &nbsp;<span class="style13">195px/50px</span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-9">
                                    <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="FileUploadStarted" runat="server" ID="fileLogo" OnUploadedComplete="fileLogo_UploadedComplete" accept="image/jpeg, image/jpg, image/png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    CALLE:
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tbCalleRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationSucursal" Enabled="False"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbCalleRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">NO. EXTERIOR:</div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbNoExtRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *"></asp:TextBox>
                                </div>
                                <div class="col-md-3">NO. INTERIOR:</div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbNoIntRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">COLONIA:</div>
                                <div class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbColoniaRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">LOCALIDAD:</div>
                                <div class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbLocRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="LOCALIDAD *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">REFERENCIA:</div>
                                <div class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbRefRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="REFERENCIA *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    MUNICIPIO:
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMunicipioRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationSucursal"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    ESTADO:
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbEstadoRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationSucursal"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    PAÍS:
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="tbPaisRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationSucursal"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbPaisRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    CODIGO POSTAL:
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="tbCpRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationSucursal"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-9" colspan="3">
                                    <asp:TextBox ID="tbCpRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" MaxLength="5"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="tbCpRec_MaskedEditExtender" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpRec" Mask="99999" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" ValidationGroup="validationSucursal"></asp:LinkButton>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>

<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="cpTitulo">
    SUCURSALES
</asp:Content>
