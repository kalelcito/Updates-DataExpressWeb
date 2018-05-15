<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="empleados.aspx.cs" Inherits="Administracion.Empleados" Async="true" %>

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
            var selector = isReset ? ".pwd-control" : ".pwd-control:not([id='<%=tbRFCEmi.ClientID%>'])";
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
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
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
                        <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" placeholder="Nombre del Empleado" Style="text-align: center;"></asp:TextBox>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-2"></div>
                </div>
                <div class="row">
                    <div class="col-md-4 tamaño">
                        <strong>Usuario:</strong>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="tbUsuario" runat="server" CssClass="form-control" placeholder="Usuario del Empleado" Style="text-align: center;"></asp:TextBox>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-2"></div>
                </div>
                <div class="row">
                    <div align="center">
                        <asp:Button ID="bBuscarReg" runat="server" OnClick="bBuscarReg_Click" CssClass="btn btn-primary" Text="Buscar" />
                        <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" CssClass="btn btn-primary" Text="Nueva Busqueda" />
                        <asp:Button ID="bNuevo" runat="server" OnClick="bNuevo_Click" CssClass="btn btn-primary" Text="Nuevo Empleado" CausesValidation="false" />
                    </div>
                </div>
            </div>
            <br />
            <div align="center">
                <asp:GridView ID="gvEmpleados" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Small" GridLines="None" DataSourceID="SqlDataSourceEmpleados" DataKeyNames="idEmpleado" EnableModelValidation="False">
                    <Columns>
                        <asp:BoundField DataField="nombreEmpleado" HeaderText="NOMBRE" SortExpression="nombreEmpleado" />
                        <asp:BoundField DataField="userEmpleado" HeaderText="USUARIO" SortExpression="userEmpleado" />
                        <asp:BoundField DataField="status" HeaderText="ACTIVO" SortExpression="status" />
                        <asp:BoundField DataField="rol" HeaderText="PERMISOS" SortExpression="rol" />
                        <asp:BoundField DataField="claveSucursal" HeaderText="SUCURSAL" SortExpression="claveSucursal" />
                        <asp:TemplateField HeaderText="FACT. RESTAURANTE">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbFactRest" runat="server" Checked='<%# Eval("asistenteSimplificado") %>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" HeaderText="ACCIONES" ItemStyle-Width="14%">
                            <ItemTemplate>
                                <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary btn-sm" Text="Detalles" CommandArgument='<%#Bind("idEmpleado") %>' OnClick="bDetalles_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Deseas eliminar el Empleado?');" CausesValidation="False" CommandName="Delete" Text="Eliminar" CssClass="btn btn-primary btn-sm">
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
                <asp:SqlDataSource ID="SqlDataSourceEmpleados" runat="server" SelectCommand="SELECT Cat_Empleados.idEmpleado, Cat_Emisor.RFCEMI, Cat_Empleados.nombreEmpleado, Cat_Empleados.userEmpleado, CASE WHEN Cat_Empleados.status = 0 THEN 'Inactivo' WHEN Cat_Empleados.Status = 1 THEN 'Activo' END AS status, Cat_Roles.Descripcion AS rol, ISNULL(Cat_SucursalesEmisor.idSucursal, 0) AS idSucursal, Cat_SucursalesEmisor.clave AS claveSucursal, Cat_Empleados.asistenteSimplificado FROM Cat_Empleados INNER JOIN Cat_Roles ON Cat_Empleados.id_Rol = Cat_Roles.idRol INNER JOIN Cat_Emisor ON id_Emisor = IDEEMI LEFT OUTER JOIN Cat_SucursalesEmisor ON Cat_Empleados.id_Sucursal = Cat_SucursalesEmisor.idSucursal  WHERE Cat_Empleados.idEmpleado <> (CASE @currentUser WHEN 1 THEN 0 ELSE 999999999 END) AND Cat_Empleados.idEmpleado <> (CASE @currentUser WHEN 1 THEN 0 ELSE 1 END) AND Cat_Empleados.eliminado <> 'True' AND Cat_Empleados.idEmpleado <> (CASE @currentUser WHEN 1 THEN 0 ELSE @currentUser END)" DeleteCommand="DELETE FROM Cat_Empleados WHERE idEmpleado=@idEmpleado">
                    <SelectParameters>
                        <asp:SessionParameter Name="currentUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <!--DeleteCommand="UPDATE Cat_Empleados set eliminado='True' WHERE idEmpleado=@idEmpleado"-->
                <!-- MODIFICAR ARRIBA 'AND idEmpleado <> 1' PARA EVITAR QUE EL EMPLEADO SE PUEDA MODIFICAR A SI MISMO -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="divNuevo" style="align-content: center;">
        <div class="modal-dialog" style="align-content: center;">
            <div class="modal-content " style="align-content: center;">
                <div class="modal-header " style="align-content: center;">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">x</button>
                    <h4 class="modal-title ">Agregar/Modificar Empleado</h4>
                </div>
                <div class="modal-body rowsSpaced" style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-9"></div>
                                <div class="col-md-3">
                                    <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" Visible="false" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Usuario:
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
                                <div class="col-md-12">
                                    <asp:CheckBox ID="cbAsistente" runat="server" Checked="false" />
                                    Asistente de facturación simplificado
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Nombre:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbNombre1" Style="text-align: center" MaxLength="35" CssClass="form-control" runat="server" pattern="[a-z A-Z ñ Ñ 0-9 _ - / \ * ]*"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" runat="server" id="rowRfc" visible="true">
                                <div class="col-md-3 tamaño">
                                    RFC:<asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                        runat="server"
                                        ControlToValidate="tbRFCEmi"
                                        ErrorMessage="*"
                                        ForeColor="Red"
                                        SetFocusOnError="True"
                                        ValidationGroup="validationRfc"
                                        ValidationExpression="^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbRFCEmi" CssClass="form-control pwd-control" runat="server" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Permisos:
                                </div>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlRol" runat="server" DataSourceID="SqlDataSourceRol" Style="text-align: center" CssClass="form-control" DataTextField="Descripcion" DataValueField="idRol" AutoPostBack="true" OnSelectedIndexChanged="ddlRol_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" id="DivGrupos" runat="server" visible="false">
                                <div class="col-md-3 tamaño">
                                    Grupo validador:
                                </div>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlGrupo" runat="server" DataSourceID="SqlDataSourceGrupo" Style="text-align: center" CssClass="form-control" DataTextField="descripcion" DataValueField="idGrupo">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño"></div>
                                <div class="col-md-9">
                                    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse">MÓDULO</a>
                                                </h4>
                                            </div>
                                            <div id="collapse" class="panel-collapse collapse" role="tabpanel">

                                                <div class="panel-body">
                                                    <div style="max-height: 200px; overflow: auto;">
                                                        <asp:CheckBoxList ID="ddlModulo" runat="server" DataSourceID="SqlDataSourceModulo" DataTextField="serie" DataValueField="idSerie" Enabled="false">
                                                        </asp:CheckBoxList>
                                                        <asp:SqlDataSource ID="SqlDataSourceModulo" runat="server" SelectCommand="SELECT DISTINCT s.idSerie, s.serie + ': ' + s.descripcion AS serie FROM Cat_Series s INNER JOIN Cat_Roles r ON r.CrearNewComp LIKE '%' + s.tipoDoc + '%' OR r.CNSComp LIKE '%' + s.tipoDoc + '%' WHERE r.idRol = @idRol">
                                                            <SelectParameters>
                                                                <asp:ControlParameter Name="idRol" ControlID="ddlRol" PropertyName="SelectedValue" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Sucursal:
                               
                                </div>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlSucursal" runat="server" DataSourceID="SqlDataSourceSucursal" DataTextField="Sucursal" Style="text-align: center" CssClass="form-control" DataValueField="idSucursal">
                                    </asp:DropDownList>
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
                                <div class="col-md-12">
                                    <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" ValidationGroup="validationRfc"></asp:LinkButton>
                                </div>
                            </div>
                            <%--<div class="escondido">
                                <asp:Label ID="lSesion" runat="server" Text="Sesion" CssClass="style14" Visible="False"></asp:Label>
                                <asp:DropDownList ID="ddlSesion" runat="server" DataSourceID="SqlDataSourceSesion" DataTextField="Descripcion" DataValueField="idSesion" Visible="False" Height="16px" Width="87px"></asp:DropDownList>
                            </div>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:SqlDataSource ID="SqlDataSourceRol" runat="server" SelectCommand="SELECT '0' as idRol, 'Selecciona el Rol' as Descripcion UNION SELECT idRol, Descripcion FROM Cat_Roles WHERE idRol <> 1 AND eliminado = 0"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceSucursal" runat="server" SelectCommand="SELECT '0' AS idSucursal, 'Selecciona Sucursal' AS Sucursal UNION SELECT idSucursal, sucursal + ':' + clave AS Sucursal FROM Cat_SucursalesEmisor WHERE eliminado=0"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceGrupo" runat="server" SelectCommand="SELECT '0' AS idGrupo, 'Selecciona Grupo' AS descripcion UNION SELECT idGrupo,descripcion FROM Cat_Grupos_validadores"></asp:SqlDataSource>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    EMPLEADOS
</asp:Content>
