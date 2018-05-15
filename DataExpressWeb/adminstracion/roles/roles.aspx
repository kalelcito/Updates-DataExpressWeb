<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="roles.aspx.cs" Inherits="Administracion.Roles" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .panel-trans {
            background-color: #f5f5f5;
            background-color: rgba(245, 245, 245, 0);
            margin-bottom: 5px !important;
        }

        .panel {
            margin-bottom: 0 !important;
        }

        .panel-heading {
            padding: 5px 15px;
        }

        .panel-body {
            padding-bottom: 0 !important;
            padding-top: 0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CATÁLOGO DE PERFILES
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div align="center">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-4"></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPerfil" runat="server" DataSourceID="SqlDataPerfiles" AutoPostBack="true" OnSelectedIndexChanged="ddlPerfil_SelectedIndexChanged" DataTextField="descripcion" DataValueField="idRol" CssClass="form-control" Style="text-align: center">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataPerfiles" runat="server" SelectCommand="(SELECT '0' AS idRol, 'Selecciona Perfil' AS descripcion) UNION (SELECT '00', 'Nuevo Perfil') UNION (SELECT CONVERT(VARCHAR(MAX), idRol), descripcion FROM Cat_Roles WHERE eliminado = 'False' AND visible = 'True')"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div id="divPaneles" style="display: none;" class="rowsSpaced">
                    <div class="row">
                        <div class="col-md-3"></div>
                        <div class="col-md-1"><span style="text-align: right"><strong>Nombre:</strong></span></div>
                        <div class="col-md-4">
                            <asp:TextBox ID="tbNombrePerfil" runat="server" CssClass="form-control" Style="text-align: center;"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="bGuardar" runat="server" Text="Guardar" OnClick="bGuardar_Click" CssClass="btn btn-primary" Visible="false" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="bEliminar" runat="server" Text="Eliminar" OnClick="bEliminar_Click" CssClass="btn btn-primary" Visible="false" OnClientClick="return confirm('¿Desea eliminar el perfil seleccionado?');" />
                        </div>
                        <div class="col-md-2"></div>
                    </div>
                    <div class="row">
                        <div class="panel panel-default panel-trans">
                            <div class="panel panel-heading">
                                <h3 class="panel-title">PERMISOS DE CONSULTA</h3>
                            </div>
                            <div class="panel-body">
                                <table class="table table-condensed table-responsive table-hover panel-trans" style="font-size: Small; border-collapse: collapse;" cellspacing="0">
                                    <tbody>
                                        <tr style="color: White; background-color: #4580A8; font-weight: bold;">
                                            <th scope="col">Facturas Propias</th>
                                            <th scope="col">Todas las Facturas</th>
                                            <th scope="col">Cancelaciones</th>
                                            <th scope="col">Docs. por Cobrar</th>
                                            <th scope="col">Facturas (01)</th>
                                            <th scope="col">Notas de Crédito (04)</th>
                                            <th scope="col">Cartas Porte (06)</th>
                                            <th scope="col">Retenciones (07)</th>
                                            <th scope="col">Nóminas (08)</th>
                                            <th scope="col">Contabilidad (09)</th>
                                            <th scope="col">Rep. General</th>
                                            <th scope="col">Comprobantes a Mostrar</th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="consultar_fac_propias" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="consultar_todas_fac" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="VerCanc" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="DXC" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="consultar_fac" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="consultar_nc" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="consultar_cp" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="consultar_ret" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="consultar_nom" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="consultar_cont" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="rep_gral" runat="server" /></td>
                                            <td>
                                                <asp:TextBox ID="TOPComp" runat="server" CssClass="form-control input-number" Style="text-align: center" placeholder="0"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="TOPComp_FilteredTextBoxExtender" runat="server" FilterType="Numbers" TargetControlID="TOPComp"></asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panel panel-default panel-trans">
                            <div class="panel panel-heading">
                                <h3 class="panel-title">PERMISOS DE CREACIÓN</h3>
                            </div>
                            <div class="panel-body">
                                <table class="table table-condensed table-responsive table-hover panel-trans" style="font-size: Small; border-collapse: collapse;" cellspacing="0">
                                    <tbody>
                                        <tr style="color: White; background-color: #4580A8; font-weight: bold;">
                                            <th scope="col">Cancelaciones</th>
                                            <th scope="col">Facturas (01)</th>
                                            <th scope="col">Notas de Crédito (04)</th>
                                            <th scope="col">Cartas Porte (06)</th>
                                            <th scope="col">Retenciones (07)</th>
                                            <th scope="col">Nóminas (08)</th>
                                            <th scope="col">Contabilidad (09)</th>
                                            <th scope="col">Editar Compr.</th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CancComp" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="crear_fac" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="crear_nc" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="crear_cp" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="crear_ret" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="crear_nom" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="crear_cont" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="edit_comp" runat="server" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panel panel-default panel-trans">
                            <div class="panel panel-heading">
                                <h3 class="panel-title">PERMISOS ADMINISTRATIVOS</h3>
                            </div>
                            <div class="panel-body">
                                <table class="table table-condensed table-responsive table-hover panel-trans" style="font-size: Small; border-collapse: collapse;" cellspacing="0">
                                    <tbody>
                                        <tr style="color: White; background-color: #4580A8; font-weight: bold;">
                                            <th scope="col">Directorios</th>
                                            <th scope="col">Clientes</th>
                                            <th scope="col">Empleados</th>
                                            <th scope="col">Emisores</th>
                                            <th scope="col">Receptores</th>
                                            <th scope="col">Perfiles</th>
                                            <th scope="col">Perfil Propio</th>
                                            <th scope="col">Series</th>
                                            <th scope="col">SMTP</th>
                                            <th scope="col">Mensajes</th>
                                            <th scope="col">Opera/OnQ</th>
                                            <th scope="col">Enviar E-Mail</th>
                                            <th scope="col">Impuestos</th>
                                            <th scope="col">Conceptos</th>
                                            <th scope="col">Tramas</th>
                                            <th scope="col">Recepción</th>
                                            <th scope="col">Validación Recepción</th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="EditInfoGeneral" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="Cliente" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="Empleado" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="EditEmisor" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="EditReceptor" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="_Roles" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="EditPerfil" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="EditPtoEmi" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="EditSMTP" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="EditMensajes" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="EditUserOpera" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="EnvioEmail" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="imp" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="conc" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="arch" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="recepcion" runat="server" /></td>
                                            <td>
                                                <asp:CheckBox ID="validacionRecepcion" runat="server" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
