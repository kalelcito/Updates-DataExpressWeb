<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarCaja.aspx.cs" Inherits="DataExpressWeb.AgregarCaja" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
      <style type="text/css">
        .CompletionListCssClass
{
    font-size: 16px;
    color: #000;
    padding: 0px 0px;
    border: 1px solid #999;
    background: #fff;
    width: 300px;
    float: left;
    z-index: 1;
    position:absolute;
    margin-left:0px;
    margin-top:-3px;
}
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

<asp:Content ID="Content3" ContentPlaceHolderID="cpTitulo" runat="server">

     <asp:HyperLink ID="HyperLink8" runat="server" CssClass="hipermenu" 
    Font-Bold="True" Font-Size="Small" ForeColor="Black" > AGREGAR PUNTO DE EMISIÓN</asp:HyperLink>
     
    </asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpCuerpoSup" runat="server">
     <table style="width:100%;">
    <tr>
        <td class="style3">
            <asp:Label ID="Label1" runat="server" Text="Documento"></asp:Label>
        </td>
        <td class="style1">
            <asp:DropDownList ID="dllTipoDoc" runat="server" CssClass="form-control">
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
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label2" runat="server" Text="Establecimiento"></asp:Label>
        </td>
        <td class="style1">
            <asp:DropDownList ID="dllEstab" runat="server" AppendDataBoundItems="True" 
                DataSourceID="SqlDataSourceSucursal" DataTextField="clave" 
                DataValueField="idSucursal" CssClass="form-control">
                <asp:ListItem Value="0">------</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="dllEstab" ErrorMessage="Campo requerido" 
                Font-Size="XX-Small" ForeColor="Red" InitialValue="0" 
                ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label3" runat="server" Text="Punto de Emisión"></asp:Label>
        </td>
        <td class="style1">
            <asp:TextBox ID="tbPtomi" runat="server" MaxLength="3" CssClass="form-control"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbPtomi"
FilterType="Numbers">
            </asp:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="tbPtomi" ErrorMessage="Campo requerido" Font-Size="XX-Small" 
                ForeColor="Red" ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label4" runat="server" Text="Ambiente"></asp:Label>
        </td>
        <td class="style1">
            <asp:DropDownList ID="ddlAmbiente" runat="server" CssClass="form-control">
                <asp:ListItem Value="0">------</asp:ListItem>
                <asp:ListItem Value="1">Pruebas</asp:ListItem>
                <asp:ListItem Value="2">Producción</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="ddlAmbiente" ErrorMessage="Campo requerido" 
                Font-Size="XX-Small" ForeColor="Red" InitialValue="0" 
                ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label5" runat="server" Text="Nombre"></asp:Label>
        </td>
        <td class="style1">
            <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ControlToValidate="tbNombre" ErrorMessage="Campo requerido" 
                Font-Size="XX-Small" ForeColor="Red" ValidationGroup="agregar"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="Label6" runat="server" Text="Impresora"></asp:Label>
        </td>
        <td class="style1">
            <asp:DropDownList ID="ddlImpresora" runat="server" CssClass="form-control">
                <asp:ListItem>Ninguna</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnConsultar" runat="server" CssClass="btn btn-primary"
                onclick="btnConsultar_Click" Text="Consultar" />
        </td>
    </tr>
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style1">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;</td>
        <td class="style1">
            <asp:Button ID="btAgregar" runat="server" CssClass="btn btn-primary"
                onclick="btAgregar_Click" Text="Agregar" ValidationGroup="agregar" />
            <asp:LinkButton ID="btnCancelar"  CssClass="btn btn-primary" runat="server" Text="Regresar" CausesValidation="false" OnClick="btnCancelar_Click1">
</asp:LinkButton>
            <asp:TextBox ID="tbImpresora" runat="server" CssClass="form-control" Visible="False"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;</td>
        <td class="style1">
            <asp:SqlDataSource ID="SqlDataSourceSucursal" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT [clave], [idSucursal] FROM [Cat_Sucursales]  where eliminado='false'">
            </asp:SqlDataSource>
        </td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;</td>
        <td class="style1">           
        </td>
    </tr>
</table>


</asp:Content>
