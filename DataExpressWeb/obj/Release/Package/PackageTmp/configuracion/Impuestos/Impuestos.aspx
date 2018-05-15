<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Impuestos.aspx.cs" Inherits="Configuracion.Impuestos" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CATÁLOGO DE IMPUESTOS
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:Button ID="bNuevo" runat="server" OnClick="bNuevo_Click" CssClass="btn btn-primary" Text="Nuevo Impuesto" />
            <br />
            <br />
            <div align="center">
                <asp:GridView ID="GridView1" runat="server" DataKeyNames="id" DataSourceID="SqlDataSource3" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="codigo" HeaderText="CODIGO" SortExpression="codigo" />
                        <asp:BoundField DataField="tipo" HeaderText="TIPO" SortExpression="tipo" />
                        <asp:BoundField DataField="Descripcion" HeaderText="IMPUESTO" SortExpression="Descripcion" />
                        <asp:BoundField DataField="valor" HeaderText="VALOR" SortExpression="valor" />
                        <asp:BoundField DataField="comentarios" HeaderText="DESCRIPCIÓN" SortExpression="comentarios" />
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%#Bind("id") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('¿Deseas eliminar el impuesto?');" Text="Eliminar" CssClass="btn btn-primary btn-sm">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" DeleteCommand="DELETE FROM Cat_CatImpuestos_C WHERE (id=@id)" SelectCommand="SELECT ci.id, ci.Descripcion, ci.valor, ci.codigo, ci.comentarios, cc.descripcion AS tipo FROM Cat_CatImpuestos_C ci INNER JOIN Cat_Catalogo1_C cc ON ci.tipo = cc.codigo AND cc.tipo = 'ImpuestoMX'">
        <DeleteParameters>
            <asp:Parameter Name="id" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <div id="divNuevo" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Agregar/Editar Impuesto</h4>
                </div>
                <div class="modal-body rowsSpaced">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-2">
                                    TIPO:<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validationImpuesto" ControlToValidate="ddlTipo" InitialValue="0"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlTipo" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataTipo" DataValueField="codigo" DataTextField="descripcion" CssClass="form-control" Style="text-align: center" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" Enabled="false">
                                        <asp:ListItem Value="0" Text="SELECCIONE" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    IMPUESTO:<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validationImpuesto" ControlToValidate="tbImpuesto"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlImpuesto" runat="server" Visible="false" CssClass="form-control" Style="text-align: center" DataSourceID="SqlDataImpuesto" DataValueField="codigo" DataTextField="descripcion" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlImpuesto_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="tbImpuesto" runat="server" Visible="true" CssClass="form-control" Style="text-align: center; text-transform: uppercase;" placeholder="NOMBRE DEL IMPUESTO *" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    FACTOR:<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validationImpuesto" ControlToValidate="ddlTipoFactor" InitialValue="0"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlTipoFactor" runat="server" Visible="true" CssClass="form-control" Style="text-align: center">
                                        <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Tasa" Value="Tasa"></asp:ListItem>
                                        <asp:ListItem Text="Cuota" Value="Cuota"></asp:ListItem>
                                        <asp:ListItem Text="Exento" Value="Exento"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    TASA/CUOTA:<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validationImpuesto" ControlToValidate="tbValor"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbValor" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbValor" ValidChars=",."></asp:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-2">CÓDIGO INT.:</div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="tbCodigo" runat="server" CssClass="form-control" Style="text-align: center; text-transform: uppercase;" placeholder="000" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div id="baseContainer" runat="server" style="display: none">
                                    <div class="col-md-2">BASE (%):</div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="tbBase" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00" ReadOnly="true" ToolTip="Porcentaje del impuesto (opcional)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbBase" ValidChars=",."></asp:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    COMENTARIOS:<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="validationImpuesto" ControlToValidate="tbDescripcion"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="tbDescripcion" runat="server" CssClass="form-control" Style="text-align: center" placeholder="DESCRIPCIÓN O COMENTARIOS *" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" Visible="false" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" ValidationGroup="validationImpuesto"></asp:LinkButton>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:SqlDataSource ID="SqlDataImpuesto" runat="server" SelectCommand="SELECT codigo, descripcion FROM Cat_Catalogo1_C WHERE tipo=@tipo">
                        <SelectParameters>
                            <asp:Parameter Name="tipo" Type="String" DefaultValue="" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataTipo" runat="server" SelectCommand="SELECT codigo, descripcion FROM Cat_Catalogo1_C WHERE tipo = 'ImpuestoMX'"></asp:SqlDataSource>
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
