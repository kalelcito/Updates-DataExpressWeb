<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="modificar_roles.aspx.cs" Inherits="Administracion.modificar_roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">

        .auto-style4 {
            text-align: right;
            background-color: #3399FF;
        }

        .auto-style5 {
            text-align: right;
            background-color: #3399FF;
            color: #FFFFFF;
        }
        .auto-style6 {
            text-align: left;
            background-color: #3399FF;
            color: #FFFFFF;
        }

    </style>
    </asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    MODIFICAR PERMISOS
</asp:Content>


<asp:Content ID="Content6" runat="server" contentplaceholderid="cpCuerpoSup">

    <div class="col-md-3"></div>
    <div class="col-md-1">
        Descripción:<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbRol" ErrorMessage="Requiere nombre del rol" ForeColor="Red">*</asp:RequiredFieldValidator>
    </div>
    <div class="col-md-5">
        <asp:TextBox ID="tbRol" runat="server" style="text-align:center" CssClass="form-control" placeholder="Descripción del Permiso" required></asp:TextBox>
    </div>
    <div class="col-md-3"></div>
    <br /><br /><br />

    <asp:LinkButton ID="LinkButton1" runat="server" data-toggle="modal" data-target="#Consulta" CssClass="btn btn-primary">Editar Permisos</asp:LinkButton>
    <br /><br />

    <div class="modal fade" id="Consulta" style="align-content:center;">
        <div class="modal-dialog modal-md" style="align-content:center;">
            <div class="modal-content " style="align-content:center;">
                <div class="modal-header " style="align-content:center;">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">X</button>
                    <h4 class="modal-title ">Paso 1. Permisos de Consulta</h4>
                </div>
                <div class="modal-body " style="align-content:center;">
                    <div class="table-responsive">
                        <table>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="cbConsultarPropia" runat="server" Text="Consultar Propias Facturas" EnableViewState="true" Checked='<%# Convert.ToBoolean(Eval("crear_admin_sucursal")) %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <ContentTemplate>

                    </ContentTemplate>
                </div>
            </div>
        </div>
    </div>














    <table cellpadding="0" cellspacing="0" width="100%" style="border: 1px None #DEDFDE; color:Black; background-color:White; border-collapse:collapse;font-size: small">
		<tr>
        <td>
    <table class="table nav-justified">
        <tr class="info">
            <td class="auto-style5"><strong>Nombre</strong></td>
            <td class="auto-style4" class="text-right"></td>
            <td class="auto-style6"><strong>Activar/Desactivar</strong></td>
           <td class="auto-style5"><strong>Nombre</strong></td>
            <td></td>
            <td class="auto-style6"><strong>Activar/Desactivar</strong></td>
        </tr>
        <tr>
            <td class="text-right">&nbsp;</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
                &nbsp;</td>
            <td>Editar Punto de Emisión:</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbEditPtoEmi" runat="server" style="font-size: x-small" />

            </td>
        </tr>
        <tr>
            <td class="text-right">&nbsp;</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
                &nbsp;</td>
            <td>Editar informacion General:</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbEditInfoGeneral" runat="server" style="font-size: x-small" />
            </td>
        </tr>
        <tr>
            <td class="text-right">Consulta por Punto de Emisión:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbConsultarPropiaPtoEmi" runat="server" EnableViewState="true"
                Checked='<%# Convert.ToBoolean(Eval("crear_admin_sucursal")) %>' ToolTip="*Para utilizar esta funcion debe estar activo Consultar Propias" />
            </td>
            <td>Consultar por Tipo de Comprobante:</td>
            <td>&nbsp;</td>
            <td>
            <asp:TextBox ID="tbComprobantesCNS" runat="server" Width="80px" ToolTip="01,04,05,06,07 (Escribir los numeros de los comprobantes que puede generar)">1,4,5,6,7</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="text-right">Consultar Todas:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbConsultarTodas" runat="server" />
            </td>
            <td>Editar SMTP:</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbEditSmtp" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="text-right">Crear Comprobante:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:TextBox ID="tbComprobantes" runat="server" Width="80px" ToolTip="01,04,05,06,07 (Escribir los numeros de los comprobantes que puede generar)">1,4,5,6,7</asp:TextBox>
            </td>
            <td>Editar Mensajes:</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbEditMensaje" runat="server" EnableViewState="true"
                Checked='<%# Convert.ToBoolean(Eval("crear_admin_sucursal")) %>' />
            </td>
        </tr>
        <tr>
            <td class="text-right">Editar Comprobante:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbEditComp" runat="server"  />
            </td>
            <td>Editar Usuarios Opera:</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbEditUserOpera" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="text-right">Reporte General:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbReportesGeneral" runat="server" />
            </td>
            <td>Limpiar Registros:</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbLimpiarLogs" runat="server" style="font-size: x-small" />
            </td>
        </tr>
        <tr>
            <td class="text-right">Cliente:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbClientes" runat="server" style="font-size: x-small" />

            </td>
            <td>Editar Perfil:</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbEditPerfil" runat="server"  />
            </td>
        </tr>
        <tr>
            <td class="text-right">Empleado:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbEmpleado" runat="server" style="font-size: x-small" />
            </td>
            <td>Envio E-Mail:</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbEnvioEmail" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="text-right">Permisos:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbRol" runat="server" />
            </td>
            <td>Top Comprobantes:</td>
            <td></td>
            <td>
            <asp:TextBox ID="tbTOPComp" runat="server" Width="80px" ToolTip="01,04,05,06,07 (Escribir los numeros de los comprobantes que puede generar)">100</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="text-right">Editar Emisor:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbEditEmi" runat="server"  />
            </td>
            <td>&nbsp;</td>
            <td></td>
            <td>
            <asp:CheckBox ID="cbRecepcion" runat="server" Visible="False" />
            </td>
        </tr>
        <tr>
            <td class="text-right">Editar Establecimiento:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbEditEstab" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="text-right">Visible:</td>
            <td class="text-right">&nbsp;</td>
            <td class="text-left">
            <asp:CheckBox ID="cbVisible" runat="server" />
            </td>
        </tr>
        </table>
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbRol" ErrorMessage="Tienes que escribir la Descripción del Rol" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lMsj" runat="server" ForeColor="#F93200"></asp:Label>
        </td>
        </tr>
</table>

    <asp:Button ID="bModificar" runat="server" CssClass="btn btn-primary" onclick="bModificar_Click" Text="Actualizar" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="btnCancelar"  CssClass="btn btn-primary" runat="server" Text="Regresar" CausesValidation="false" OnClick="btnCancelar_Click"></asp:LinkButton>
&nbsp;&nbsp;

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>