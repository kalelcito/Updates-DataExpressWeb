<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConceptosOnQ.aspx.cs" Inherits="Configuracion.ConceptosOnQ" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CATÁLOGO DE CONCEPTOS DE ONQ
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div align="center">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCategorias" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategorias_SelectedIndexChanged" DataSourceID="SqlDataCategorias" DataValueField="descripcion" DataTextField="descripcion" CssClass="form-control" Style="text-align: center">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataCategorias" runat="server" SelectCommand="SELECT DISTINCT CLASIFICACION AS descripcion FROM CatalogoConceptos"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-1"></div>
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
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="5" BackColor="White" AllowPaging="True" Font-Size="Smaller" GridLines="None" DataKeyNames="IDECONCEPTO" DataSourceID="SqlDataSource3" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDeleted="GridView1_RowDeleted">
                    <Columns>
                        <asp:BoundField DataField="descripcion" HeaderText="Descripción" SortExpression="descripcion" ItemStyle-Width="40%" />
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('¿Deseas eliminar el concepto?');" Text="Eliminar" CssClass="btn btn-primary btn-sm">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle />
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" DeleteCommand="DELETE FROM CatalogoConceptos WHERE (IDECONCEPTO=@IDECONCEPTO)">
        <DeleteParameters>
            <asp:Parameter Name="IDECONCEPTO" />
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
                                <div class="col-md-2">CONCEPTO:</div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbConceptoInicial" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CONCEPTO INICIAL *"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" Style="color: #FF0000" ControlToValidate="tbConceptoInicial" ValidationGroup="validationCat"></asp:RequiredFieldValidator>
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
                            <div class="rowsSpaced">
                                <div class="row">
                                    <div class="col-md-10"></div>
                                    <div class="col-md-2" align="right">
                                        <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" Visible="false" AutoPostBack="true" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        DESCRIPCIÓN:
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbDescripcion" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationConcepto"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:TextBox ID="tbDescripcion" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true" placeholder="DESCRIPCIÓN *"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" ValidationGroup="validationConcepto"></asp:LinkButton>
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

</asp:Content>

<asp:Content ID="Content32" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
