<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bancos.aspx.cs" Inherits="DataExpressWeb.configuracion.Bancos" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .panel {
            background: rgba(0,0,0,0) !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    CATÁLOGO DE BANCOS
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-1"></div>
                <div class="col-md-7">
                    <asp:TextBox ID="tbBusqueda" runat="server" CssClass="form-control" placeholder="// Escriba parámetros de búsqueda (banco, cuenta, rfc...)"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <asp:LinkButton ID="lbBusqueda" runat="server" OnClick="lbBusqueda_Click" CssClass="btn btn-primary">
                <i class="fa fa-search" aria-hidden="true"></i>&nbsp;Buscar
                    </asp:LinkButton>
                </div>
                <div class="col-md-1">
                    <asp:LinkButton ID="lbLimpiar" runat="server" OnClick="lbLimpiar_Click" CssClass="btn btn-primary">
                <i class="fa fa-refresh" aria-hidden="true"></i>&nbsp;Limpiar
                    </asp:LinkButton>
                </div>
                <div class="col-md-1">
                    <asp:LinkButton ID="lbAgregarNuevo" runat="server" OnClick="lbAgregarNuevo_Click" CssClass="btn btn-primary">
                <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Nuevo
                    </asp:LinkButton>
                </div>
                <div class="col-md-1"></div>
            </div>
            <div class="row">
                <div class="col-md-12">&nbsp;</div>
            </div>
            <div class="row">
                <asp:GridView ID="gvBancos" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" BackColor="White" Font-Size="Small" GridLines="None" DataKeyNames="Id" DataSourceID="SqlDataSourceBancos">
                    <Columns>
                        <asp:BoundField DataField="NombreCuenta" HeaderText="Nombre de Cuenta" />
                        <asp:BoundField DataField="RfcBanco" HeaderText="Rfc de Banco" />
                        <asp:BoundField DataField="NombreBanco" HeaderText="Nombre de Banco" />
                        <asp:BoundField DataField="ClaveBanco" HeaderText="Clave de Banco" />
                        <asp:BoundField DataField="NumeroCuenta" HeaderText="Número de Cuenta" />
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Editar" CommandArgument='<%#Bind("Id") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lbEliminar" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('¿Deseas eliminar el registro?');" Text="Eliminar" CssClass="btn btn-primary btn-sm">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSourceBancos" runat="server" SelectCommand="SELECT * FROM Cat_BancosComplemento" DeleteCommand="DELETE FROM Cat_BancosComplemento WHERE Id = @Id">
                    <DeleteParameters>
                        <asp:Parameter Name="Id" />
                    </DeleteParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divNuevo" class="modal fade" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header ">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Agregar/Editar Registro</h4>
                        </div>
                        <div class="modal-body" style="text-align: left;">
                            <div class="row">
                                <div class="col-md-12">NOMBRE DE CUENTA:<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbNombreCuenta" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationNuevo"></asp:RequiredFieldValidator></div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="tbNombreCuenta" runat="server" CssClass="form-control" placeholder="// Obligatorio"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    RFC DE BANCO:<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbRfcBanco" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationNuevo"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                        runat="server"
                                        ControlToValidate="tbRfcBanco"
                                        ErrorMessage="*"
                                        ForeColor="Red"
                                        SetFocusOnError="True"
                                        ValidationGroup="validationNuevo"
                                        ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="tbRfcBanco" runat="server" CssClass="form-control upper" placeholder="// Obligatorio" MaxLength="13"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">NOMBRE DE BANCO:<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNombreBanco" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationNuevo"></asp:RequiredFieldValidator></div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="tbNombreBanco" runat="server" CssClass="form-control" placeholder="// Obligatorio" MaxLength="300"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">CLAVE DE BANCO:</div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="tbClaveBanco" runat="server" CssClass="form-control" placeholder="// Opcional"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    NÚMERO DE CUENTA:<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbNumeroCuenta" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationNuevo"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                        runat="server"
                                        ControlToValidate="tbNumeroCuenta"
                                        ErrorMessage="*"
                                        ForeColor="Red"
                                        SetFocusOnError="True"
                                        ValidationGroup="validationNuevo"
                                        ValidationExpression="[A-Z0-9_]{10,50}">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="tbNumeroCuenta" runat="server" CssClass="form-control upper" placeholder="// Obligatorio" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbGuardar" runat="server" CssClass="btn btn-primary" OnClick="lbGuardar_Click" ValidationGroup="validationNuevo">Guardar</asp:LinkButton>
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
