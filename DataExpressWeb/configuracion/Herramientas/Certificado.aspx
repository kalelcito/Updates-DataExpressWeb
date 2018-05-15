<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Certificado.aspx.cs" Inherits="DataExpressWeb.configuracion.Herramientas.Certificado" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function limpiarFiles() {
            clearContent("<%= fileCER.ClientID %>");
            clearContent("<%= fileKey.ClientID %>");
        }
        function clearContent(id) {
            var asyncFileUpload = $get(id);
            var txts = asyncFileUpload.getElementsByTagName("input");
            for (var i = 0; i < txts.length; i++) {
                if (txts[i].type == "text") {
                    txts[i].value = "";
                    txts[i].style.backgroundColor = "white";
                }
            }
        }
        function cerUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            if (ext != 'cer' && ext != 'CER') {
                alertBootBox('Solo se aceptan archivos con extensión ".cer"<br/> Intentelo de nuevo.', 4);
                return false;
            }
            return true;
        }
        function keyUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            if (ext != 'key' && ext != 'KEY') {
                limpiarFiles();
                alertBootBox('Solo se aceptan archivos con extensión ".key"<br/> Intentelo de nuevo.', 4);
                return false;
            }
            return true;
        }
        function uploadError(sender) {
            limpiarFiles();
            alertBootBox('El archivo no se pudo cargar en el servidor. Intentelo de nuevo.', 4);
        }
    </script>

</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="cpTitulo" runat="server">
    CATALOGO DE CERTIFICADOS
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <asp:LinkButton ID="lbNuevo" runat="server" CssClass="btn btn-primary" OnClick="lbNuevo_Click">Nuevo</asp:LinkButton>
                </div>
            </div>
            <br />
            <asp:GridView ID="gvCertificados" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Small" GridLines="None" DataSourceID="SqlDataCertificados" DataKeyNames="IDECNF">
                <Columns>
                    <asp:BoundField DataField="RFCEMI" HeaderText="RFC" SortExpression="RFCEMI"></asp:BoundField>
                    <asp:BoundField DataField="CERNUM" HeaderText="CERTIFICADO" SortExpression="CERNUM"></asp:BoundField>
                    <asp:BoundField DataField="FOLIOS" HeaderText="FOLIOS INICIO-FIN" SortExpression="FOLIOS"></asp:BoundField>
                    <asp:BoundField DataField="FECHAINICIO" HeaderText="FECHA INICIO" SortExpression="FECHAINICIO"></asp:BoundField>
                    <asp:BoundField DataField="FECHAFIN" HeaderText="FECHA FIN" SortExpression="FECHAFIN"></asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%# Eval("IDECNF") + "|" + Eval("RFCEMI") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                            <asp:LinkButton ID="bEliminar" runat="server" CssClass="btn btn-primary btn-sm" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle />
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
    </asp:UpdatePanel>
    <div class="modal fade " id="divEditar">
        <div class="modal-dialog">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Agregar/Editar Certificado</h4>
                </div>
                <div class="modal-body rowsSpaced">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-2">
                                    RFC EMISOR:<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRfcEmisor" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationRfc"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                        runat="server"
                                        ControlToValidate="tbRfcEmisor"
                                        ErrorMessage="*"
                                        ForeColor="Red"
                                        SetFocusOnError="True"
                                        ValidationGroup="validationRfc"
                                        ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbRfcEmisor" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" placeholder="AAA[A]999999XXX *"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">FOLIO INICIO:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbFolioInicio" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="tbFolioInicio"></asp:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-2">FOLIO FIN:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbFolioFin" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" TargetControlID="tbFolioFin"></asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="row" style="display: none;">
                                <div class="col-md-2">NUM. APROB.:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbNumAprob" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" TargetControlID="tbNumAprob"></asp:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-2">AÑO APROB.:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbAnioAprob" runat="server" CssClass="form-control input-number" Style="text-align: center"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers" TargetControlID="tbAnioAprob"></asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">NO. CERT.:</div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbNoCert" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">FECHA INICIO:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbFechaInicio" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                </div>
                                <div class="col-md-2">FECHA FIN:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbFechaFin" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">CERTIFICADO (.cer):</div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbCer" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-10">
                                    <asp:AsyncFileUpload OnClientUploadError="uploadError" runat="server" ID="fileCER" OnClientUploadStarted="cerUpload" OnUploadedComplete="fileCER_UploadedComplete" Style="text-align: center" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">LLAVE (.key):</div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbKey" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-10">
                                    <asp:AsyncFileUpload OnClientUploadError="uploadError" runat="server" ID="fileKey" OnClientUploadStarted="keyUpload" OnUploadedComplete="fileKey_UploadedComplete" Style="text-align: center" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">CLAVE:</div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbClave1" runat="server" CssClass="form-control" TextMode="Password" Style="text-align: center;"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">VERIFIQUE CLAVE:</div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbClave2" runat="server" CssClass="form-control" TextMode="Password" Style="text-align: center;"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-5"></div>
                                <div class="col-md-2">
                                    <asp:LinkButton ID="bActualizar" CssClass="btn btn-primary" runat="server" OnClick="bActualizar_Click" ValidationGroup="validationRfc">Actualizar</asp:LinkButton>
                                </div>
                                <div class="col-md-5"></div>
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
    <asp:SqlDataSource ID="SqlDataCertificados" runat="server" DeleteCommand="DELETE FROM Cat_Certificados WHERE IDECNF = @IDECNF">
        <DeleteParameters>
            <asp:Parameter Name="IDECNF" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
