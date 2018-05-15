<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="roles.aspx.cs" Inherits="Administracion.roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .style2
        {
            font-size: large;
        }
        .auto-style2 {
            font-size: xx-small;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    CATÁLOGO DE PERMISOS  
</asp:Content>

  <%--  <p class="green_right">
        <asp:HyperLink ID="HyperLink8" runat="server" CssClass="hipermenu" NavigateUrl="~/adminstracion/roles/crear_rol.aspx" >Agregar</asp:HyperLink>
    </p>--%>


<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:HyperLink ID="HyperLink8" runat="server" CssClass="btn btn-primary" Font-Bold="True" Font-Size="Small" NavigateUrl="~/adminstracion/roles/crear_rol.aspx">Agregar</asp:HyperLink>
</asp:Content>
<asp:Content ID="Content32" ContentPlaceHolderID="cpCuerpoInf" runat="server">

    <div align="center"> <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="idRol" DataSourceID="SqlDataSource3"  CssClass="table table-hover" style="font-size: xx-small" >
        <Columns>
<%--            <asp:BoundField DataField="facturas" HeaderText="Documentos" 
                SortExpression="facturas" />--%>
 <%--           <asp:CheckBoxField DataField="nc" HeaderText="Crear NC" 
                SortExpression="nc">
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>--%>
<asp:BoundField DataField="idRol" HeaderText="idRol" SortExpression="idRol" ReadOnly="True" Visible="False"></asp:BoundField>
            <asp:BoundField DataField="descripcion" HeaderText="Nombre" 
                SortExpression="descripcion" />
                        <asp:CheckBoxField DataField="consultar_fac_propias" HeaderText="Consultar Propias" 
                SortExpression="consultar_fac_propias" Visible="False" />
            <asp:CheckBoxField DataField="consultar_todas_fac" HeaderText="Consultar Todas" 
                SortExpression="consultar_todas_fac" Visible="False" />
            <asp:BoundField DataField="CrearNewComp" HeaderText="Crear Comprobantes" SortExpression="CrearNewComp" />
            <asp:CheckBoxField DataField="null1" HeaderText="null1" 
                SortExpression="null1" Visible="False" />
            <asp:CheckBoxField DataField="null2" HeaderText="null2" 
                SortExpression="null2" Visible="False">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="null3" 
                HeaderText="null3" SortExpression="null3" Visible="False">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="null4" 
                HeaderText="null4" SortExpression="null4" Visible="False">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="EditComp0N" 
                HeaderText="Editar Comprobantes" SortExpression="EditComp0N" Visible="False">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="ReporteGeneral" HeaderText="Reporte General" 
                SortExpression="ReporteGeneral">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="Cliente" HeaderText="Cliente" 
                SortExpression="Cliente">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="Empleado" 
                HeaderText="Empleado" SortExpression="Empleado">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="Roles" HeaderText="Permisos" 
                SortExpression="Roles">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="EditEmisor" HeaderText="Editar Emisor" 
                SortExpression="EditEmisor">
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="EditEstab" HeaderText="Editar Establecimiento" SortExpression="EditEstab" />
            <asp:CheckBoxField DataField="EditPtoEmi" HeaderText="Editar Punto de Emisión" SortExpression="EditPtoEmi" />
            <asp:CheckBoxField DataField="EditInfoGeneral" HeaderText="EditInfoGeneral" SortExpression="EditInfoGeneral" Visible="False" />
            <asp:CheckBoxField DataField="EditSMTP" HeaderText="EditSMTP" SortExpression="EditSMTP" Visible="False" />
            <asp:CheckBoxField DataField="EditMensajes" HeaderText="EditMensajes" SortExpression="Editar Mensajes" />
            <asp:CheckBoxField DataField="EditUserOpera" HeaderText="EditUserOpera" SortExpression="EditUserOpera" Visible="False" />
            <asp:CheckBoxField DataField="LimpiarLogs" HeaderText="LimpiarLogs" SortExpression="LimpiarLogs" Visible="False" />
            <asp:CheckBoxField DataField="EditPerfil" HeaderText="EditPerfil" SortExpression="EditPerfil" Visible="False" />
            <asp:CheckBoxField DataField="EnvioEmail" HeaderText="EnvioEmail" SortExpression="EnvioEmail" Visible="False" />
            <asp:CheckBoxField DataField="eliminado" HeaderText="eliminado" SortExpression="eliminado" Visible="False" />
          
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <a href='modificar_roles.aspx?id=<%# Eval("idRol") %>'>Editar</a>
                    <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" 
                        CommandName="Delete" 
                        OnClientClick="return confirm('Si eliminas el Rol, se eliminaran los Empleados asociado a el. ¿Deseas eliminar el rol?');" 
                        Text="Eliminar"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" Width="80px" />
            </asp:TemplateField>            
        </Columns>
        <FooterStyle   />
        <HeaderStyle CssClass="info"  />
        <PagerStyle  />
                <PagerSettings />
                <RowStyle  />
        <SelectedRowStyle  />
        <SortedAscendingCellStyle  />
        <SortedAscendingHeaderStyle />
        <SortedDescendingCellStyle />
        <SortedDescendingHeaderStyle />
                <PagerTemplate>
                    <div class="text-center">
                        <asp:LinkButton ID="linkPrimero" runat="server" CommandArgument="First" CommandName="Page" Text="&amp;lt;&amp;lt; Primera" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkAnterior" runat="server" CommandArgument="Prev" CommandName="Page" Text="&amp;lt; Anterior" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkSiguiente" runat="server" CommandArgument="Next" CommandName="Page" Text="Siguiente &amp;gt;" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkUltimo" runat="server" CommandArgument="Last" CommandName="Page" Text="&amp;Uacute;ltima &amp;gt;&amp;gt;" />
                    </div>
                </PagerTemplate>  
    </asp:GridView></div>
    <span class="auto-style2">
    <br />
    <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
    <br />
    </span>
    <asp:DropDownList ID="ddlRol" runat="server" AutoPostBack="True" 
        DataSourceID="SqlDataSource2" DataTextField="descripcion" 
        DataValueField="idRol" 
        onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
        AppendDataBoundItems="True" Visible="False" CssClass="auto-style2">
        <asp:ListItem Value="0">Selecciona un Rol</asp:ListItem>
    </asp:DropDownList>
    <span class="auto-style2">
    <br />
    <br />
    </span>
    <asp:DetailsView ID="dvRoles" runat="server" AutoGenerateRows="False" 
        BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
        CellPadding="4" DataKeyNames="idRol" DataSourceID="SqlDataSource1" 
        ForeColor="Black" GridLines="Vertical" Height="50px" Width="414px" 
        style="text-align: center; color: #FFFFFF;" 
        onitemdeleted="dvRoles_ItemDeleted" oniteminserted="dvRoles_ItemInserted" 
        Visible="False" CssClass="auto-style2"  >
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#ECF2B2" Font-Bold="True" ForeColor="Black" />
        <EmptyDataTemplate>
            No existen Roles.
        </EmptyDataTemplate>
        <Fields>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="Actualizar" ForeColor="White"></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Cancel" Text="Cancelar" ForeColor="White"></asp:LinkButton>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                        CommandName="Insert" Text="Insertar" ForeColor="White"></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Cancel" Text="Cancelar" ForeColor="White"></asp:LinkButton>
                </InsertItemTemplate>
                <ItemTemplate>
                    &nbsp; &nbsp;<asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" 
                        CommandName="New" ForeColor="White" Text="Nuevo"></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                        CommandName="Edit" Text="Editar" ForeColor="White"></asp:LinkButton>
                    &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Eliminar" ForeColor="White"></asp:LinkButton>
                </ItemTemplate>
                <FooterStyle ForeColor="White" />
                <HeaderStyle BackColor="#307C34" />
                <ItemStyle BackColor="#307C34" Font-Bold="True" ForeColor="White" />
            </asp:TemplateField>
            <asp:BoundField DataField="descripcion" HeaderText="Descripción" 
                SortExpression="descripcion" />
            <asp:CheckBoxField DataField="crear_cliente" HeaderText="Crear Cliente" 
                SortExpression="crear_cliente" />
            <asp:CheckBoxField DataField="crear_admin_sucursal" 
                HeaderText="Crear Administrador de Sucursal" 
                SortExpression="crear_admin_sucursal" />
            <asp:CheckBoxField DataField="consultar_facturas_propias" 
                HeaderText="Facturas Propias" 
                SortExpression="consultar_facturas_propias" />
            <asp:CheckBoxField DataField="consultar_todas_facturas" 
                HeaderText="Todas las Facturas" 
                SortExpression="consultar_todas_facturas" />
            <asp:CheckBoxField DataField="reportesCat_Sucursales" 
                HeaderText="Reportes de Cat_Sucursales" SortExpression="reportesCat_Sucursales" />
            <asp:CheckBoxField DataField="reportesGlobales" HeaderText="Reportes Globales" 
                SortExpression="reportesGlobales" />
            <asp:CheckBoxField DataField="modificarEmpleado" HeaderText="Modificar Empleado" 
                SortExpression="modificarEmpleado" />
            <asp:CheckBoxField DataField="asignacion_roles" HeaderText="Roles" 
                SortExpression="asignacion_roles" />
            <asp:CheckBoxField DataField="envio_facturas_email" 
                HeaderText="Enviar Emails" SortExpression="envio_facturas_email" />
            <asp:CheckBoxField DataField="agregar_documento" HeaderText="Crear Documentos" 
                SortExpression="agregar_documento" />
            <asp:CheckBoxField DataField="validarFactura" HeaderText="Validar Facturas" 
                SortExpression="validarFactura" />
            <asp:CheckBoxField DataField="aceptarFactura" HeaderText="Aceptar Facturas" 
                SortExpression="aceptarFactura" />
            <asp:CheckBoxField DataField="Recepcion" HeaderText="Ver Facturas Recibidas" 
                SortExpression="Recepcion" />
        </Fields>
        <FooterStyle BackColor="#990000" BorderColor="Black" BorderStyle="Solid" 
                    Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#53ab46" Font-Bold="True" ForeColor="White" BorderColor="#53ab46" 
                    BorderStyle="None" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" 
                    BorderColor="#009900" BorderStyle="Solid" />
        <RowStyle BackColor="#ECF2B2" ForeColor="Black" />
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="SELECT * FROM [Cat_Roles] WHERE idRol =@idRol " 
        InsertCommand="INSERT INTO Cat_Roles(descripcion, crear_cliente, crear_admin_sucursal, consultar_facturas_propias, consultar_todas_facturas, reportesCat_Sucursales, reportesGlobales, modificarEmpleado, asignacion_roles, envio_facturas_email, agregar_documento, eliminado, validarFactura, aceptarFactura, Recepcion) VALUES (@descripcion, @crear_cliente, @crear_admin_sucursal, @consultar_facturas_propias, @consultar_todas_facturas, @reportesCat_Sucursales, @reportesGlobales, @modificarEmpleado, @asignacion_roles, @envio_facturas_email, @agregar_documento, 0, @validarFactura, @aceptarFactura, @Recepcion)" 
        DeleteCommand="UPDATE Cat_Roles SET eliminado = 'true' WHERE (idRol = @idRol)" 
        UpdateCommand="UPDATE Cat_Roles SET descripcion = @descripcion, crear_cliente = @crear_cliente, crear_admin_sucursal = @crear_admin_sucursal, consultar_facturas_propias = @consultar_facturas_propias, consultar_todas_facturas = @consultar_todas_facturas, reportesCat_Sucursales = @reportesCat_Sucursales, reportesGlobales = @reportesGlobales, modificarEmpleado = @modificarEmpleado, asignacion_roles = @asignacion_roles, envio_facturas_email = @envio_facturas_email, agregar_documento = @agregar_documento, validarFactura = @validarFactura, aceptarFactura = @aceptarFactura, Recepcion = @Recepcion WHERE idRol = @idRol">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlRol" Name="idRol" PropertyName="SelectedValue" Type="Byte" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="idRol" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="descripcion" />
            <asp:Parameter Name="crear_cliente" />
            <asp:Parameter Name="crear_admin_sucursal" />
            <asp:Parameter Name="consultar_facturas_propias" />
            <asp:Parameter Name="consultar_todas_facturas" />
            <asp:Parameter Name="reportesCat_Sucursales" />
            <asp:Parameter Name="reportesGlobales" />
            <asp:Parameter Name="modificarEmpleado" />
            <asp:Parameter Name="asignacion_roles" />
            <asp:Parameter Name="envio_facturas_email" />
            <asp:Parameter Name="agregar_documento" />
            <asp:Parameter Name="validarFactura" />
            <asp:Parameter Name="aceptarFactura" />
            <asp:Parameter Name="Recepcion" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="descripcion" />
            <asp:Parameter Name="crear_cliente" />
            <asp:Parameter Name="crear_admin_sucursal" />
            <asp:Parameter Name="consultar_facturas_propias" />
            <asp:Parameter Name="consultar_todas_facturas" />
            <asp:Parameter Name="reportesCat_Sucursales" />
            <asp:Parameter Name="reportesGlobales" />
            <asp:Parameter Name="modificarEmpleado" />
            <asp:Parameter Name="asignacion_roles" />
            <asp:Parameter Name="envio_facturas_email" />
            <asp:Parameter Name="agregar_documento" />
            <asp:Parameter Name="validarFactura" />
            <asp:Parameter Name="aceptarFactura" />
            <asp:Parameter Name="Recepcion" />
            <asp:Parameter Name="idRol" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="SELECT * FROM [Cat_Roles] WHERE eliminado = 'False'" 
        InsertCommand="INSERT INTO Cat_Roles(descripcion, crear_cliente, crear_admin_sucursal, consultar_facturas_propias, consultar_todas_facturas, reportesCat_Sucursales, reportesGlobales, modificarEmpleado, asignacion_roles, envio_facturas_email, agregar_documento, eliminado, validarFactura, aceptarFactura, Recepcion) VALUES (@descripcion, @crear_cliente, @crear_admin_sucursal, @consultar_facturas_propias, @consultar_todas_facturas, @reportesCat_Sucursales, @reportesGlobales, @modificarEmpleado, @asignacion_roles, @envio_facturas_email, @agregar_documento, 0, @validarFactura, @aceptarFactura, @Recepcion)">
        <InsertParameters>
            <asp:Parameter Name="descripcion" />
            <asp:Parameter Name="crear_cliente" />
            <asp:Parameter Name="crear_admin_sucursal" />
            <asp:Parameter Name="consultar_facturas_propias" />
            <asp:Parameter Name="consultar_todas_facturas" />
            <asp:Parameter Name="reportesCat_Sucursales" />
            <asp:Parameter Name="reportesGlobales" />
            <asp:Parameter Name="modificarEmpleado" />
            <asp:Parameter Name="asignacion_roles" />
            <asp:Parameter Name="envio_facturas_email" />
            <asp:Parameter Name="agregar_documento" />
            <asp:Parameter Name="validarFactura" />
            <asp:Parameter Name="aceptarFactura" />
            <asp:Parameter Name="Recepcion" />
        </InsertParameters>
                
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        DeleteCommand="UPDATE Cat_Roles SET eliminado='true' WHERE (idRol = @idRol) UPDATE Cat_Empleados SET eliminado='true' WHERE (id_Rol=@idRol)" 
        SelectCommand="SELECT * FROM Cat_roles WHERE eliminado='False'">
    </asp:SqlDataSource>
    <br class="auto-style2" />
    </asp:Content>
