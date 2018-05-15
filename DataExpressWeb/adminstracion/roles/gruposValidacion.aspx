<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="gruposValidacion.aspx.cs" Inherits="DataExpressWeb.adminstracion.roles.gruposValidacion" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function SelectAll(checkbox, idgrupo) {
            var bool = !checkbox.checked;
            var idGlobal = checkbox.id;
            $('#<%= gvGroupValidation.ClientID %> tr').slice(1).each(function (index) {
                var chk = $(this).find("input[type='checkbox']");
                var id = chk.attr('id');
                if (bool) {
                    if (id != idGlobal) {
                        $(chk).prop('disabled', true);
                        $('#<%= idcheck.ClientID %>').val(idgrupo);
                        SimulateClick('<%= Distribuidor_btn.ClientID %>');
                    }
                } else {

                    $(chk).removeAttr('disabled');
                }
                //var edo = $(this).find("idGrupo")
                //var edo = $(this).find("check");
                //if ((edo != undefined) && (edo != "") || !bool) {
                //    $(chk).prop("disabled", bool).change();
                //}           

            });
            //return bool;

        }
    </script>
    <style type="text/css">
        .panel {
            background: rgba(0,0,0,0) !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    ADMINISTRAR GRUPOS DE VALIDACIÓN
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="updTab7" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField ID="idcheck" runat="server" Value="" />
            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingOne">
                        <h4 class="panel-title">
                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">GRUPOS VALIDADORES</a>
                        </h4>
                    </div>
                    <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-2">
                                    <strong>Nombre:</strong>
                                    <asp:RequiredFieldValidator ID="tbNuevoGrupo_RequiredFieldValidator" runat="server" ErrorMessage="*" ValidationGroup="nuevoGrupo" ControlToValidate="tbNuevoGrupo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox ID="tbNuevoGrupo" runat="server" AutoPostBack="false" CssClass="form-control" Style="text-align: center;" placeholder="NOMBRE DEL GRUPO A AGREGAR *"></asp:TextBox>

                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="lbNuevoGrupo" runat="server" CssClass="btn btn-primary" ValidationGroup="nuevoGrupo" OnClick="lbAddGroupValidation_Click">
                                                <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>&nbsp;Agregar
                                        </asp:LinkButton>
                                    </span>
                                    <div>
                                        <asp:LinkButton ID="bUpdateG" runat="server" CssClass="btn btn-primary btn-sm" Text="Actualizar" CommandArgument='<%#Bind("idGrupo") %>' OnClick="bUpdateG_Click"></asp:LinkButton>
                                        <asp:GridView ID="gvGroupValidation" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover"
                                            PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Small" GridLines="None"
                                            DataSourceID="SqlDataSourceGValidation" DataKeyNames="idGrupo" EnableModelValidation="False" OnRowDataBound="gvGroupValidation_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="descripcion" HeaderText="Nombre" SortExpression="descripcion" />
                                                <asp:TemplateField ShowHeader="False" HeaderText="ACCIONES" ItemStyle-Width="50%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="check" runat="server" CssClass="btn-xs" onclick='<%# string.Format("return SelectAll(this,{0});", Eval("idGrupo").ToString()) %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("idGrupo", "Seleccionar grupo {0}") %>' AutoPostBack="true" idGrupo='<%# Eval("idGrupo").ToString() %>' />
                                                        <asp:LinkButton ID="bDeleteG" runat="server" OnClientClick="return confirm('¿Deseas eliminar este grupo validador?');" CommandArgument='<%#Bind("idGrupo")%>' OnClick="bDeleteG_Click" Text="Eliminar" CssClass="btn btn-primary btn-sm">
                                                        </asp:LinkButton>
                                                        <asp:HiddenField ID="checkidgrupo" runat="server" Value='<%# Eval("idGrupo").ToString() %>' />
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
                                        <asp:LinkButton runat="server" Style="display: none;" CommandArgument="" ID="Distribuidor_btn" OnClick="Distribuidor_Clic"></asp:LinkButton>
                                        <asp:SqlDataSource ID="SqlDataSourceGValidation" runat="server"
                                            SelectCommand="SELECT idGrupo, descripcion, ISNULL(statusDistribuidor,0) AS statusDistribuidor FROM cat_grupos_validadores ORDER BY descripcion DESC"></asp:SqlDataSource>
                                    </div>



                                    <div class="modal fade" id="divUpdateTipoProveedor">
                                        <div class="modal-dialog modal-md">
                                            <div class="modal-content ">
                                                <div class="modal-header ">
                                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                    <h4 class="modal-title ">ACTUALIZAR TIPO DE PROVEEDOR</h4>
                                                </div>
                                                <div class="modal-body rowsSpaced">
                                                    <div class="col-md-2">
                                                        Nombre:
                                    <asp:RequiredFieldValidator ID="updateProv" runat="server" ErrorMessage="*" ValidationGroup="tbUpdatePro" ControlToValidate="tbUpdatePro" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="tbUpdatePro" runat="server" Style="text-align: center" CssClass="form-control" placeholder="ACTUALIZE TIPO DE PROVEEDOR *"></asp:TextBox>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" ValidationGroup="updateProv" OnClick="lbAddUpdateProveedor_Click">Actualizar</asp:LinkButton>
                                                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-2"></div>
                            </div>
                            <div class="row">&nbsp;</div>
                            <div class="row">
                            </div>
                        </div>
                    </div>
                </div>


                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingTwo">
                        <h4 class="panel-title">
                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">ESTRUCTURA VALIDACIONES</a>
                        </h4>
                    </div>
                    <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-2"><strong>Tipos de Proveedor:</strong></div>
                                <div class="col-md-7">
                                    <asp:DropDownList ID="ddlTiposProveedor" runat="server" DataSourceID="SqlDataTiposProveedor" AutoPostBack="true" OnSelectedIndexChanged="ddlTiposProveedor_SelectedIndexChanged" DataTextField="NOMBRE" DataValueField="ID" CssClass="form-control" Style="text-align: center">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataTiposProveedor" runat="server" SelectCommand="(SELECT '0' AS ID, 'Selecciona Proveedor' AS NOMBRE) UNION (SELECT '00', 'Nuevo Proveedor') UNION (SELECT CONVERT(VARCHAR(MAX), idTipo) AS ID, nombre AS NOMBRE FROM Cat_TiposProveedor WHERE eliminado = 'False' AND visible = 'True')"></asp:SqlDataSource>
                                    <div class="modal fade" id="divNuevoTipoProveedor">
                                        <div class="modal-dialog modal-md">
                                            <div class="modal-content ">
                                                <div class="modal-header ">
                                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                    <h4 class="modal-title ">AGREGAR TIPO DE PROVEEDOR</h4>
                                                </div>
                                                <div class="modal-body rowsSpaced">
                                                    <div class="col-md-2">
                                                        Nombre:
                                    <asp:RequiredFieldValidator ID="tbNuevoTipoProveedor_RequiredFieldValidator" runat="server" ErrorMessage="*" ValidationGroup="nuevoTipoProveedor" ControlToValidate="tbNuevoTipoProveedor" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="tbNuevoTipoProveedor" runat="server" Style="text-align: center" CssClass="form-control" placeholder="TIPO DE PROVEEDOR *"></asp:TextBox>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:LinkButton ID="lbAddTipoProveedor" runat="server" CssClass="btn btn-primary" ValidationGroup="nuevoTipoProveedor" OnClick="lbAddTipoProveedor_Click">Agregar</asp:LinkButton>
                                                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="modal fade" id="divEstructureProveedor" style="align-content: center;">
                                        <div class="modal-dialog" style="align-content: center;">
                                            <div class="modal-content " style="align-content: center;">
                                                <div class="modal-header " style="align-content: center;">
                                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                    <h4 class="modal-title ">CARGAR ESTRUCTURAS VALIDACIONES</h4>
                                                </div>
                                                <div class="modal-body rowsSpaced" style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            </div>
                                                <div class="modal-body rowsSpaced">
                                                    <div class="col-md-2">
                                                        Proveedor:
                                                    </div>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="tbProveedor" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <br />
                                                    <br />
                                                    <div class="col-md-2">
                                                        Grupo Validador:
                                                    </div>
                                                    <div class="col-md-10">
                                                        <%--      <asp:DropDownList ID="ddlGroupValidate" runat="server" DataSourceID="SqlDataSourceGroupValidate" Style="text-align: center" CssClass="form-control" DataTextField="nombre" DataValueField="idGrupo" AutoPostBack="true"">
                                                        </asp:DropDownList>--%>

                                                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSourceGroupValidate" DataTextField="descripcion" DataValueField="idGrupo" CssClass="form-control" Style="text-align: center">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <br />
                                                    <br />
                                                    <div class="col-md-2">
                                                        Orden Validador:
                                                    </div>
                                                    <div class="col-md-10 input-number">
                                                        <asp:TextBox ID="tbOrden" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <br />
                                                    <br />



                                                    <div>
                                                        <asp:GridView ID="gvEstructuras" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover"
                                                            PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Small" GridLines="None"
                                                            DataKeyNames="idGrupo" EnableModelValidation="False">
                                                            <Columns>
                                                                <asp:BoundField DataField="descripcion" HeaderText="Proveedor" SortExpression="descripcion" />
                                                                <asp:BoundField DataField="orden" HeaderText="Orden" SortExpression="Orden" />
                                                                <asp:TemplateField ShowHeader="False" HeaderText="ACCIONES" ItemStyle-Width="30%">
                                                                    <ItemTemplate>

                                                                        <asp:LinkButton ID="bUpdate" runat="server" CssClass="btn btn-primary btn-sm" Text="Cambiar" CommandArgument='<%#Bind("idGrupo") %>'
                                                                            OnClick="bUpdateFI_Click"></asp:LinkButton>
                                                                        <asp:LinkButton ID="bDelete" runat="server" OnClientClick="return confirm('¿Deseas eliminar este grupo validador?');"
                                                                            CommandArgument='<%#Bind("idGrupo") %>' OnClick="bDeleteFI_Click" Text="Eliminar" CssClass="btn btn-primary btn-sm"> 
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
                                                        <%--<asp:SqlDataSource ID="SqlDataSourceEstructura" runat="server" SelectCommand="SELECT * FROM Cat_EstructuraValidacionTemp ORDER BY idEstructura ASC"></asp:SqlDataSource>--%>
                                                        <asp:SqlDataSource ID="SqlDataSourceEstructura" runat="server" SelectCommand="SELECT g.idGrupo, g.descripcion, V.orden FROM Cat_EstructuraValidacionTemp V
inner join Cat_Grupos_validadores g ON V.ordenIdGruposValidacion = g.idGrupo ORDER BY idEstructura ASC"></asp:SqlDataSource>
                                                    </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:LinkButton ID="EAgregar" runat="server" CssClass="btn btn-primary" OnClick="lbAddEstructure_Click">Agregar</asp:LinkButton>
                                                    <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="$('body').removeClass('modal-open');$('.modal-backdrop').remove();">Cerrar</button>
                                                    <asp:LinkButton ID="bSave" runat="server" CssClass="btn btn-primary" OnClick="lbAddAllOrders_Click">Guardar</asp:LinkButton>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <asp:GridView ID="gvTiposProveedor" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover"
                                            PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Small" GridLines="None"
                                            DataKeyNames="idTipo" EnableModelValidation="False" DataSourceID="SqlDataSourceTProveedor">
                                            <Columns>
                                                <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre" />
                                                <asp:BoundField DataField="visible" HeaderText="Visible" SortExpression="visible" />
                                                <asp:TemplateField ShowHeader="False" HeaderText="ACCIONES" ItemStyle-Width="30%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="bUpdate" runat="server" CssClass="btn btn-primary btn-sm" Text="Actualizar" CommandArgument='<%#Bind("idTipo") %>' OnClick="bUpdate_Click"></asp:LinkButton>
                                                        <asp:LinkButton ID="bDelete" runat="server" OnClientClick="return confirm('¿Deseas eliminar este grupo validador?');" CommandArgument='<%#Bind("idTipo") %>' OnClick="bDeleteE_Click" Text="Eliminar" CssClass="btn btn-primary btn-sm" aria-hidden="true">
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
                                        <asp:SqlDataSource ID="SqlDataSourceTProveedor" runat="server"
                                            SelectCommand="SELECT idTipo, nombre, visible, eliminado FROM Cat_TiposProveedor where eliminado='False' ORDER BY nombre DESC"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:SqlDataSource ID="SqlDataSourceGroupValidate" runat="server" SelectCommand="SELECT idGrupo, descripcion FROM cat_grupos_validadores ORDER BY descripcion DESC"></asp:SqlDataSource>

    <div class="modal fade" id="divUpdateP" style="align-content: center;">
        <div class="modal-dialog" style="align-content: center;">
            <div class="modal-content " style="align-content: center;">
                <div class="modal-header " style="align-content: center;">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">x</button>
                    <h4 class="modal-title ">Editar Tipo de Proveedor</h4>
                </div>
                <div class="modal-body rowsSpaced" style="-ms-align-content: center; -webkit-align-content: center; align-content: center;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Nombre:
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="tbNname" runat="server" MaxLength="100" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 tamaño">
                                    Visible:
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
                                    <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bUpdateP_Click" Text="Agregar"></asp:LinkButton>
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
