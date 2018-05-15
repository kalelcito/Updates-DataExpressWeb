<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarCaja.aspx.cs" Inherits="DataExpressWeb.AgregarCaja" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .style1
    {
        width: 473px;
    }
    .style2
    {
        width: 152px;
    }
    .style3
    {
        width: 152px;
        text-align: right;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SupContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuIzqTitulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuIzqContent" runat="server">
    <asp:HyperLink ID="HyperLink8" runat="server" CssClass="hipermenu" 
    Font-Bold="True" Font-Size="Small" ForeColor="Black" > Agregar punto Emision</asp:HyperLink>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
    <tr>
        <td class="style3">
            <asp:Label ID="Label1" runat="server" Text="Documento"></asp:Label>
        </td>
        <td class="style1">
            <asp:DropDownList ID="dllTipoDoc" runat="server" Width="91px">
                <asp:ListItem Value="0">------</asp:ListItem>
                <asp:ListItem Value="01">Factura</asp:ListItem>
                <asp:ListItem Value="04">Nota de crédito</asp:ListItem>
                <asp:ListItem Value="05">Nota de débito</asp:ListItem>
                <asp:ListItem Value="06">Guias de remisión</asp:ListItem>
                <asp:ListItem Value="07">Retención</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="dllTipoDoc" ErrorMessage="Campo requerido" 
                Font-Size="XX-Small" ForeColor="Red" InitialValue="0" 
                ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label2" runat="server" Text="Establecimiento"></asp:Label>
        </td>
        <td class="style1">
            <asp:DropDownList ID="dllEstab" runat="server" AppendDataBoundItems="True" 
                DataSourceID="SqlDataSourceSucursal" DataTextField="clave" 
                DataValueField="idSucursal">
                <asp:ListItem Value="0">------</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="dllEstab" ErrorMessage="Campo requerido" 
                Font-Size="XX-Small" ForeColor="Red" InitialValue="0" 
                ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label3" runat="server" Text="Pto. Emisión"></asp:Label>
        </td>
        <td class="style1">
            <asp:TextBox ID="tbPtomi" runat="server" MaxLength="3" Width="91px"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbPtomi"
FilterType="Numbers">
            </asp:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="tbPtomi" ErrorMessage="Campo requerido" Font-Size="XX-Small" 
                ForeColor="Red" ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label4" runat="server" Text="Ambiente"></asp:Label>
        </td>
        <td class="style1">
            <asp:DropDownList ID="ddlAmbiente" runat="server" Width="91px">
                <asp:ListItem Value="0">------</asp:ListItem>
                <asp:ListItem Value="1">Pruebas</asp:ListItem>
                <asp:ListItem Value="2">Producción</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="ddlAmbiente" ErrorMessage="Campo requerido" 
                Font-Size="XX-Small" ForeColor="Red" InitialValue="0" 
                ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label5" runat="server" Text="Nombre"></asp:Label>
        </td>
        <td class="style1">
            <asp:TextBox ID="tbNombre" runat="server" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ControlToValidate="tbNombre" ErrorMessage="Campo requerido" 
                Font-Size="XX-Small" ForeColor="Red" ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label6" runat="server" Text="Impresora"></asp:Label>
        </td>
        <td class="style1">
            <asp:DropDownList ID="ddlImpresora" runat="server">
                <asp:ListItem>Ninguna</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnConsultar" runat="server" CssClass="btGeneral" 
                onclick="btnConsultar_Click" Text="Consultar" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style1">
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;</td>
        <td class="style1">
            <asp:Button ID="btAgregar" runat="server" CssClass="btGeneral" 
                onclick="btAgregar_Click" Text="Agregar" ValidationGroup="agregar" />
            <asp:Button ID="btnCancelar" runat="server" CssClass="btGeneral" 
                onclick="btnCancelar_Click" Text="Cancelar" />
            <asp:TextBox ID="tbImpresora" runat="server" Width="105px" Visible="False"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;</td>
        <td class="style1">
            <asp:SqlDataSource ID="SqlDataSourceSucursal" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT [clave], [idSucursal] FROM [Sucursales]">
            </asp:SqlDataSource>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;</td>
        <td class="style1">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </td>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
