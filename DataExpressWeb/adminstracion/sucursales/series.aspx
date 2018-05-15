<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="series.aspx.cs" Inherits="Administracion.Series" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div align="center">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div>
                    <div class="row">
                        <div class="col-md-1"></div>
                        <div class="col-md-6"><strong>DESCRIPCIÓN:</strong></div>
                        <div class="col-md-4"><strong>COMPROBANTE:</strong></div>
                        <div class="col-md-1"></div>
                    </div>
                    <div class="row">
                        <div class="col-md-1"></div>
                        <div class="col-md-6">
                            <asp:TextBox ID="tbBuscaDescripcion" runat="server" CssClass="form-control" Style="text-align: center;" placeholder="DESCRIPCIÓN DE LA SERIE"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlBuscaComprobante" runat="server" CssClass="form-control" DataSourceID="SqlDataComprobante" DataTextField="descripcion" DataValueField="codigo"></asp:DropDownList>
                        </div>
                        <div class="col-md-1"></div>
                    </div>
                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4"><strong>TIPO:</strong></div>
                        <div class="col-md-4"><strong>AMBIENTE:</strong></div>
                        <div class="col-md-2"></div>
                    </div>
                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlBuscaTipo" runat="server" CssClass="form-control" DataSourceID="SqlDataTipoRecep" DataTextField="descripcion" DataValueField="codigo"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlBuscaAmbiente" runat="server" CssClass="form-control" DataSourceID="SqlDataAmbiente" DataTextField="descripcion" DataValueField="codigo"></asp:DropDownList>
                        </div>
                        <div class="col-md-2"></div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="bBuscar" runat="server" CssClass="btn btn-primary" OnClick="bBuscar_Click" Text="Buscar"></asp:Button>
                            <asp:Button ID="bActualizar" runat="server" CssClass="btn btn-primary" OnClick="bActualizar_Click" Text="Limpiar Búsqueda"></asp:Button>
                            <asp:Button ID="bNuevo" runat="server" CssClass="btn btn-primary" OnClick="bNuevo_Click" Text="Nuevo"></asp:Button>
                        </div>
                    </div>
                </div>
                <br />
                <asp:GridView ID="gvSeries" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover" BackColor="White" DataKeyNames="idSerie" DataSourceID="SqlDataSource1" GridLines="None" AllowPaging="True" Font-Size="Smaller" OnPageIndexChanging="gvSeries_PageIndexChanging" OnRowDeleted="gvSeries_RowDeleted">
                    <Columns>
                        <asp:BoundField DataField="serie" HeaderText="SERIE"></asp:BoundField>
                        <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" />
                        <asp:BoundField DataField="tipoRecep" HeaderText="TIPO" />
                        <asp:BoundField DataField="tipoDoc" HeaderText="COMPROBANTE"></asp:BoundField>
                        <asp:BoundField DataField="ambiente" HeaderText="AMBIENTE" />
                        <asp:TemplateField ShowHeader="False" HeaderText="ACCIÓN">
                            <ItemTemplate>
                                <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%#Bind("idSerie") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" CommandName="Delete" Text="Eliminar" CssClass="btn btn-primary btn-sm"></asp:LinkButton>
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
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" DeleteCommand="DELETE FROM Cat_Series WHERE idSerie = @idSerie">
        <DeleteParameters>
            <asp:Parameter Name="idSerie" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <div id="divNuevo" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Agregar/Editar Serie</h4>
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
                                    <div class="col-md-2">
                                        SERIE:
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbSerie" FilterType="UppercaseLetters, Numbers"></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ValidationGroup="validationSerie" Style="color: #FF0000" ControlToValidate="tbSerie"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="tbSerie" runat="server" CssClass="form-control" MaxLength="10" Style="text-align: center" placeholder="X *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        DESCRIPCIÓN:
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ValidationGroup="validationSerie" Style="color: #FF0000" ControlToValidate="tbDesc"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="tbDesc" runat="server" CssClass="form-control" Style="text-align: center" placeholder="DESCRIPCIÓN *"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">COMPROBANTE:<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="validationSerie" ControlToValidate="ddlComprobante"></asp:RequiredFieldValidator></div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlComprobante" runat="server" CssClass="form-control" DataSourceID="SqlDataComprobante" DataTextField="descripcion" DataValueField="codigo"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">AMBIENTE:<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="validationSerie" ControlToValidate="ddlAmbiente"></asp:RequiredFieldValidator></div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlAmbiente" runat="server" CssClass="form-control" DataSourceID="SqlDataAmbiente" DataTextField="descripcion" DataValueField="codigo"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">TIPO:<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="validationSerie" ControlToValidate="ddlTipoRecep"></asp:RequiredFieldValidator></div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlTipoRecep" runat="server" CssClass="form-control" DataSourceID="SqlDataTipoRecep" DataTextField="descripcion" DataValueField="codigo"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4">
                                        <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" ValidationGroup="validationSerie" Visible="false"></asp:LinkButton>
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
    <asp:SqlDataSource ID="SqlDataComprobante" runat="server" SelectCommand="(SELECT '0' AS codigo, 'Todos' AS descripcion) UNION (SELECT codigo, descripcion FROM Cat_Catalogo1_C WHERE tipo = 'Comprobante')"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataTipoRecep" runat="server" SelectCommand="(SELECT '0' AS codigo, 'Todos' AS descripcion) UNION (SELECT codigo, descripcion FROM Cat_Catalogo1_C WHERE tipo = 'TipoTrama')"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataAmbiente" runat="server" SelectCommand="(SELECT '0' AS codigo, 'Todos' AS descripcion) UNION (SELECT codigo, descripcion FROM Cat_Catalogo1_C WHERE tipo = 'Ambiente')"></asp:SqlDataSource>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>

<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="cpTitulo">
    SERIES
</asp:Content>
