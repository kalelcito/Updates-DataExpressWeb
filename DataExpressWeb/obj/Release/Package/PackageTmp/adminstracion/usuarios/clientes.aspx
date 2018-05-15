<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="clientes.aspx.cs" Inherits="Administracion.Clientes" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .tamaño {
            font-size: 17.1px;
            text-align: right;
        }
    </style>
    <script type="text/javascript">
        function resetPass(resetUser) {
            var isReset = resetUser == undefined ? true : Boolean(resetUser);
            var selector = isReset ? ".pwd-control" : ".pwd-control:not([id='<%=tbNombre.ClientID%>'])";
            window.setTimeout("$('" + selector + "').val('');setPwdVal(1, '');setPwdVal(2, '');", 200);
        }
        function setPwdVal(index, value) {
            var id = "";
            switch (index) {
                case 1:
                    id = '<%= hfPass1.ClientID %>';
                    break;
                case 2:
                    id = '<%= hfPass2.ClientID %>';
                    break;
                default:
                    break;
            }
            $('#' + id).val(value);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    CLIENTES
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField ID="hfPass1" runat="server" Value="" />
            <asp:HiddenField ID="hfPass2" runat="server" Value="" />
            <div align="center" class="rowsSpaced">
                <div class="row">
                    <div class="col-md-4 tamaño">
                        <strong>Nombre:</strong>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="tbNomClient" runat="server" CssClass="form-control" placeholder="Nombre del Cliente" Style="text-align: center;"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4 tamaño">
                        <strong>RFC:</strong><asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                            runat="server"
                            ControlToValidate="tbIdentificacion"
                            ErrorMessage="*"
                            ForeColor="Red"
                            SetFocusOnError="True"
                            ValidationGroup="validationRfc"
                            ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                        </asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="tbIdentificacion" runat="server" CssClass="form-control" placeholder="RFC del Cliente" Style="text-align: center;"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                    </div>
                </div>
                <div class="row">
                    <div align="center">
                        <asp:Button ID="bBuscar" runat="server" OnClick="bBuscarReg_Click" CssClass="btn btn-primary" Text="Buscar" ValidationGroup="validationRfc" />
                        <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" CssClass="btn btn-primary" Text="Nueva Busqueda" />
                        <asp:Button ID="bNuevo" runat="server" OnClick="bNuevo_Click" CssClass="btn btn-primary" Text="Nuevo Cliente" CausesValidation="false" />
                        <br />
                    </div>
                </div>
            </div>
            <br />
            <div align="center">
                <asp:GridView ID="gvEmpleados" runat="server" DataSourceID="SqlDataSourceEmpleados" DataKeyNames="idCliente" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Small" GridLines="None" OnSorting="gvEmpleados_Sorting" OnPageIndexChanging="gvEmpleados_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="NOMBRE" SortExpression="NOMBRE_COMPLETO" />
                        <asp:BoundField DataField="USERNAME" HeaderText="USUARIO" SortExpression="USERNAME" />
                        <asp:BoundField DataField="RFCREC" HeaderText="RFC" SortExpression="RFCREC" />
                        <asp:BoundField DataField="Estado" HeaderText="ESTADO" SortExpression="Estado" ReadOnly="True" />
                        <asp:TemplateField ShowHeader="False" HeaderText="ACCIONES" ItemStyle-Width="14%">
                            <ItemTemplate>
                                <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%#Bind("idCliente") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Deseas eliminar el Cliente?');" CausesValidation="False" CommandName="Delete" Text="Eliminar" CssClass="btn btn-primary btn-sm">
                                </asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle />
                            <ItemStyle HorizontalAlign="Center" />
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
                <asp:SqlDataSource ID="SqlDataSourceEmpleados" runat="server" SelectCommand="SELECT Cat_Clientes.idCliente, Cat_Clientes.nombreCliente AS NOMBRE_COMPLETO, Cat_Clientes.userCliente AS USERNAME, CASE Cat_Clientes.Status WHEN 0 THEN 'Inactivo' ELSE 'Activo' END AS Estado, Cat_Receptor.RFCREC FROM Cat_Clientes INNER JOIN Cat_Receptor ON Cat_Clientes.id_Receptor = Cat_Receptor.IDEREC WHERE Cat_Clientes.eliminado = 0" DeleteCommand="UPDATE Cat_CLIENTES SET eliminado = 1 where idCliente = @idCliente">
                    <DeleteParameters>
                        <asp:Parameter Name="idCliente" />
                    </DeleteParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="divNuevo" style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
        <div class="modal-dialog" style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
            <div class="modal-content " style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
                <div class="modal-header " style="align-content: center;">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">x</button>
                    <h4 class="modal-title ">Agregar/Modificar Cliente</h4>
                </div>
                <div class="modal-body rowsSpaced" style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row" id="rowEditar" runat="server" style="display: none;">
                                <div class="col-md-9"></div>
                                <div class="col-md-3">
                                    <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    RFC:
                                </div>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlRFC" CssClass="form-control" runat="server" Style="text-align: center" AppendDataBoundItems="true" DataSourceID="SqlDataReceptores" DataTextField="RFCREC" DataValueField="IDEREC" AutoPostBack="true" OnSelectedIndexChanged="ddlRFC_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="SELECCIONE RFC" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataReceptores" runat="server" SelectCommand="SELECT DISTINCT IDEREC, RFCREC FROM Cat_Receptor"></asp:SqlDataSource>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Razón Social:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbRazonSocial" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Cliente:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbUsername" runat="server" MaxLength="15" Style="text-align: center" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Status:
                                </div>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Style="text-align: center" CssClass="form-control">
                                        <asp:ListItem Value="1">Activo</asp:ListItem>
                                        <asp:ListItem Value="0">Inactivo</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Nombre:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbNombre" Style="text-align: center" MaxLength="35" CssClass="form-control pwd-control" runat="server" pattern="[a-z A-Z ñ Ñ 0-9 _ - / \ * ]*"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" id="divModPwd">
                                <div class="col-md-12">
                                    <asp:CheckBox ID="cbModificarContrasena" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="cbModificarContrasena_CheckedChanged" />
                                    Modificar contraseña
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Contraseña:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbContrasena" runat="server" MaxLength="15" TextMode="Password" Style="text-align: center" CssClass="form-control pwd-control" onkeyup="setPwdVal(1, $(this).val());"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Repetir Contraseña:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbRepetir" runat="server" MaxLength="15" TextMode="Password" Style="text-align: center" CssClass="form-control pwd-control" onkeyup="setPwdVal(2, $(this).val());"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    E-Mail:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbEmail" runat="server" Style="text-align: center" placeholder="ej. tu@dominio.com, pagina@dataexpress.com" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Teléfono:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbTel" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar"></asp:LinkButton>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
