<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Conceptos.aspx.cs" Inherits="Configuracion.Conceptos" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        
    </style>
    <script type="text/javascript">
        function cerrarCategoria() {
            $('#<%= ddlCategorias.ClientID %>').val("");
            $('#<%= ddlCategorias.ClientID %>').trigger('change');
        }
        function changeText(html, id) {
            $(id).val(html).change();
            return false;
        }
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
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CATÁLOGO DE CONCEPTOS
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div align="center">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-5">
                        <asp:DropDownList ID="ddlCategorias" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategorias_SelectedIndexChanged" DataSourceID="SqlDataCategorias" DataValueField="idCatalogo1_C" DataTextField="descripcion" CssClass="form-control" Style="text-align: center">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataCategorias" runat="server" SelectCommand="SELECT idCatalogo1_C, descripcion FROM Cat_Catalogo1_C WHERE tipo = 'CategoriaConceptos'"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="bUnidades" runat="server" OnClick="bUnidades_Click" CssClass="btn btn-primary" Text="Unidades de Medida" />
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="bLoadXlsx" runat="server" OnClick="bLoadXlsx_Click" CssClass="btn btn-primary" Text="Cargar Excel" />
                    </div>
                    <div class="col-md-2"></div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-3">
                        <asp:Button ID="bNuevo" runat="server" OnClick="bNuevo_Click" CssClass="btn btn-primary" Text="Nuevo Concepto" Visible="false" />
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="bDelCat" runat="server" OnClick="bDelCat_Click" CssClass="btn btn-primary" OnClientClick="return confirm('¿Desea eliminar la categoría?\nLos conceptos dentro de la categoría también se eliminarán');" Text="Eliminar categoría" Visible="false" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None" DataKeyNames="idConcepto" DataSourceID="SqlDataSource3">
                    <Columns>
                        <asp:BoundField DataField="codigo" HeaderText="Codigo" SortExpression="codigo" />
                        <asp:BoundField DataField="descripcion" HeaderText="Descripción" SortExpression="Descripcion" ItemStyle-Width="40%" />
                        <asp:BoundField DataField="valorUnitario" HeaderText="Valor Unitario" SortExpression="valor" ItemStyle-Width="20%" />
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%#Bind("idConcepto") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('¿Deseas eliminar el concepto?');" Text="Eliminar" CssClass="btn btn-primary btn-sm">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle />
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                    <PagerSettings />
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
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" DeleteCommand="DELETE FROM Cat_CatConceptos_C WHERE (idConcepto=@idConcepto)">
        <DeleteParameters>
            <asp:Parameter Name="idConcepto" />
        </DeleteParameters>
    </asp:SqlDataSource>

    <div id="divNuevaCategoria" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" onclick="cerrarCategoria();">×</button>
                    <h4 class="modal-title ">Agregar Categoría</h4>
                </div>
                <div class="modal-body ">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-2">NOMBRE:</div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbNombreCat" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NOMBRE *"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" Style="color: #FF0000" ControlToValidate="tbNombreCat" ValidationGroup="validationCat"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:LinkButton ID="bAgregarCat" runat="server" CssClass="btn btn-primary" OnClick="bAgregarCat_Click" Text="Agregar" ValidationGroup="validationCat"></asp:LinkButton>
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="cerrarCategoria();">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div id="divNuevo" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Agregar/Editar Concepto</h4>
                </div>
                <div class="modal-body ">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <table class=" table-responsive table-condensed" style="-ms-align-content: center; -webkit-align-content: center; align-content: center; width: 100%">
                                <tr align="center">
                                    <td align="center" class="col-md-3">CÓDIGO:</td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbCodigo" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CÓDIGO *" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" runat="server" visible='<%# Session["CfdiVersion"].ToString().Equals("3.3") %>'>
                                    <td align="center" class="col-md-3">CLAVE PRODUCTO/SERVICIO:
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_tbCveProdServ" runat="server" ControlToValidate="tbCveProdServ" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationConcepto"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbCveProdServ" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CLAVE PROD/SERV *" ReadOnly="true"></asp:TextBox>
                                        <asp:DropDownList ID="ddlCveProdServ" runat="server" CssClass="select-large-4" Style="text-align: center" Visible="false" OnSelectedIndexChanged="ddlCveProdServ_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">DESCRIPCIÓN:
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbDescripcion" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationConcepto"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbDescripcion" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true" placeholder="DESCRIPCIÓN *"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">VALOR UNITARIO:
                                        <asp:FilteredTextBoxExtender ID="tbVU_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbVU" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <asp:TextBox ID="tbVU" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="col-md-3">UNIDAD DE MEDIDA:
                                    </td>
                                    <td align="center" class="col-md-9" colspan="3">
                                        <div class="input-group">
                                            <asp:TextBox ID="tbUMedida" runat="server" CssClass="form-control" Style="text-align: center" placeholder="UNIDAD DE MEDIDA *" ReadOnly="true"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
                                                <asp:ListView ID="lvUnidades" runat="server" DataSourceID="SqlDataSourceUnidades" DataKeyNames="id">
                                                    <LayoutTemplate>
                                                        <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                        </ul>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <li>
                                                            <a href="#" onclick="return changeText($(this).html().split(':')[0], '#<%= tbUMedida.ClientID %>');"><%# Eval("descripcion") %></a>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" colspan="4">
                                        <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" Visible="false" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" colspan="4">
                                        <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" ValidationGroup="validationConcepto"></asp:LinkButton>
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
                                    <asp:AsyncFileUpload OnClientUploadError="UploadError" OnClientUploadStarted="fileXlsx_FileUploadStarted" runat="server" ID="fileXlsx" OnUploadedComplete="fileXlsx_UploadedComplete" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="bs-callout bs-callout-info">
                                        <h4>Nota:</h4>
                                        <p>El catálogo debe ser un archivo de Excel en formato 2007 o posterior (con extensión <code>.xlsx</code>), respetando la siguiente estructura:</p>
                                        <asp:Image ID="imgXlsx" runat="server" AlternateText="Estructura XLSX" ImageUrl="~/imagenes/estructura-catalogo-conceptos-33.jpg" CssClass="img-responsive" />
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

    <div id="divUnidades" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Catálogo Unidades de Medida</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-3">
                                    CLAVE SAT
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbClaveSat" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationUnidadMedida"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    UNIDAD DE MEDIDA
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbUnidadMedida" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationUnidadMedida"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1"></div>
                                <div class="col-md-1"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="tbClaveSat" runat="server" CssClass="form-control upper" Style="text-align: center" MaxLength="3"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender_tbClaveSat" runat="server" TargetControlID="tbClaveSat" FilterType="LowercaseLetters,UppercaseLetters,Numbers" FilterMode="ValidChars" />
                                    <asp:DropDownList ID="ddlClaveSat" runat="server" CssClass="select-large-1" Style="text-align: center" Visible="false" OnSelectedIndexChanged="ddlClaveSat_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="tbUnidadMedida" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:LinkButton ID="bAgregarUnidad" runat="server" CssClass="btn btn-primary" OnClick="bAgregarUnidad_Click" ValidationGroup="validationUnidadMedida">
                                        <i class="fa fa-plus" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="bAgregarUnidadExistente" runat="server" OnClick="bAgregarUnidad_Click" Style="display: none;"></asp:LinkButton>
                                </div>
                                <div class="col-md-1"></div>
                            </div>
                            <div class="row">&nbsp;</div>
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-10">
                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" BackColor="White" Font-Size="Small" GridLines="None" DataKeyNames="id" DataSourceID="SqlDataSourceUnidades">
                                        <Columns>
                                            <asp:BoundField DataField="claveSat" HeaderText="Clave SAT" SortExpression="claveSat" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('¿Deseas eliminar el registro?');" CssClass="btn btn-primary btn-sm">
                                                    <i class="fa fa-times" aria-hidden="true"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </div>
                                <div class="col-md-1"></div>
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
    <asp:SqlDataSource ID="SqlDataSourceUnidades" runat="server" SelectCommand="SELECT id,claveSat,descripcion FROM Cat_CveUnidad" DeleteCommand="DELETE FROM Cat_CveUnidad WHERE (id=@id)">
        <DeleteParameters>
            <asp:Parameter Name="id" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>

<asp:Content ID="Content32" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
