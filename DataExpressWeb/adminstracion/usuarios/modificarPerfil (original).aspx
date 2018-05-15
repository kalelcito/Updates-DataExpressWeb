<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="modificarPerfil.aspx.cs" Inherits="Administracion.modificarPerfil" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 513px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div class="row">
     <div class="col-md-6">
       <div class="form-group">
            Nombre completo:
            <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" placeholder="DATAEXPRESS LATINOAMÉRICA *" ></asp:TextBox>
       </div>
       <div class="form-group">
            Usuario:
            <asp:TextBox ID="tbUsername" runat="server" MaxLength="15" ReadOnly="True" CssClass="form-control" placeholder="EMPLE110001 *" ></asp:TextBox>
       </div>
       <div class="form-group">







                   <table cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
                       <tr>
                           <td>
                               <table cellpadding="0" >
                                   <tr>
                                       <td colspan="2" class="text-center">
                                           <asp:CheckBox ID="CheckBox1" runat="server" CssClass="form-control" Text="Cambiar Contrseña" AutoPostBack="True" />
                                       </td>
                                   </tr>
                                   <tr>
                                       <td class="auto-style1">
                                           <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword" Visible="False">Contraseña Actual:</asp:Label>
                                           <asp:TextBox ID="CurrentPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15" Visible="False"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" ErrorMessage="La contraseña es obligatoria." ToolTip="La contraseña es obligatoria." ValidationGroup="ChangePassword1">* Campo obligatorio para actualizar</asp:RequiredFieldValidator>
                                           <br />
            <asp:Label ID="mensajeeee" runat="server" class="auto-style2" style="font-size: xx-small" Visible="False">*Si esta vacio no se va a modificar</asp:Label></td>
                                       <td>
                                           &nbsp;</td>
                                   </tr>
                                   <tr>
                                       <td class="auto-style1">
                                           <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword" Visible="False">Nueva contraseña:</asp:Label>
                                           <asp:TextBox ID="NewPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15" Visible="False"></asp:TextBox>
            <asp:Label ID="mensajeeee2" class="auto-style2" runat="server" style="font-size: xx-small" Visible="False">*15 Caracteres Maximo</asp:Label><asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword" ErrorMessage="La nueva contraseña es obligatoria." ToolTip="La nueva contraseña es obligatoria." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                       </td>
                                       <td>
                                           &nbsp;</td>
                                   </tr>
                                   <tr>
                                       <td class="auto-style1">
                                           <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword" Visible="False">Confirmar la nueva contraseña:</asp:Label>
                                           <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="15" Visible="False"></asp:TextBox>
                                           <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="Confirmar la nueva contraseña debe coincidir con la entrada Nueva contraseña." ValidationGroup="ChangePassword1" style="color: #FF0000"></asp:CompareValidator>
                                           <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" ErrorMessage="Confirmar la nueva contraseña es obligatorio." ToolTip="Confirmar la nueva contraseña es obligatorio." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                           <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                       </td>
                                       <td>
                                           &nbsp;</td>
                                   </tr>
                                   </table>
                           </td>
                       </tr>
                   </table>







       </div>
       <div class="form-group">
            Estado:
            <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True"
                CssClass="form-control" Enabled="False" >
                <asp:ListItem Value="1">Activo</asp:ListItem>
                <asp:ListItem Value="0">Inactivo</asp:ListItem>
            </asp:DropDownList>
       </div>
         <div class="form-group">
             <asp:Label ID="lbEmail" runat="server" Text="E-Mail:"></asp:Label>
&nbsp;<asp:TextBox ID="tbEmail" runat="server" CssClass="form-control" placeholder="DATAEXPRESS LATINOAMÉRICA *" ></asp:TextBox>
        </div>



         <div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                ControlToValidate="tbNombre" ErrorMessage="Requiere el Nombre" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                ControlToValidate="tbUsername" ErrorMessage="Requiere el Usuario" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                ControlToValidate="CurrentPassword" ErrorMessage="Requiere la contraseña. " ForeColor="Red"></asp:RequiredFieldValidator>
         </div>
     </div>
     <div class="col-md-6">
       <div class="form-group">
            <asp:Label ID="lRol" runat="server" Text="Permisos:" Visible="False"  ></asp:Label>
            <asp:DropDownList ID="ddlRol" runat="server" DataSourceID="SqlDataSource2" CssClass="form-control"      DataTextField="descripcion" DataValueField="idRol" Enabled="False">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                SelectCommand="SELECT * FROM [Cat_Roles]"></asp:SqlDataSource>
       </div>
       <div class="form-group">
            <asp:Label ID="lSucursal" runat="server" Text="Establecimiento:" Visible="False" ></asp:Label>
            <asp:DropDownList ID="ddlSucursal" runat="server" DataSourceID="SqlDataSource3"
                DataTextField="Sucursal" DataValueField="idSucursal" CssClass="form-control" Enabled="False"  >
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                SelectCommand="SELECT idSucursal, sucursal + ':' + clave AS Sucursal FROM Cat_Sucursales"></asp:SqlDataSource>
       </div>
       <div class="form-group">
            <asp:Label ID="lPtoEmi" runat="server" Text="Punto de Emisión:"></asp:Label>
            <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataSourcePtoEmi" DataTextField="ptoemi" CssClass="form-control" DataValueField="ptoemi" Enabled="False">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSourcePtoEmi" runat="server"
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"

                SelectCommand="SELECT TOP 1000 ptoemi FROM [Cat_CajasSucursal] group by ptoemi">
            </asp:SqlDataSource>
       </div>
       <div class="form-group">
            <asp:Button ID="bModificar" runat="server" Text="Actualizar"
                onclick="bModificar_Click1" CssClass="btn btn-primary" />
            &nbsp;
            <asp:LinkButton ID="btnCancelar"  CssClass="btn btn-primary" runat="server" Text="Regresar" CausesValidation="false" OnClick="btnCancelar_Click">
</asp:LinkButton>
            <br />
            <br />
     <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
       </div>
       <div class="form-group">
            <asp:DropDownList ID="ddlSesion" runat="server" DataSourceID="SqlDataSource1"
                DataTextField="descripcion" DataValueField="idSesion" CssClass="form-control" Visible="False">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                SelectCommand="SELECT * FROM [Cat_Sesiones]"></asp:SqlDataSource>
            <asp:TextBox ID="TextPass" runat="server" AutoPostBack="True" ReadOnly="True" Visible="False"></asp:TextBox>
       </div>
     </div>
   </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="cpTitulo">
    INFORMACIÓN DE LA CUENTA
</asp:Content>

