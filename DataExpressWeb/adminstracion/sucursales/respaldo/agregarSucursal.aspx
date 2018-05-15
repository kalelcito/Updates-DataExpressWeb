<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="agregarSucursal.aspx.cs" Inherits="Administracion.agregarSucursal" %>
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
        width: 111px;
    }
    .style2
    {
        width: 423px;
    }
        .style3
        {
            width: 111px;
        }
        .style5
        {
            color: #000000;
        }
        .style7
        {
            text-align: center;
            font-size: large;
        }
        .style8
        {
            text-align: right;
            font-size: small;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <table style="width:59%;">
    <tr>
        <td class="style8">
            Clave:</td>
        <td class="style2">
            <asp:TextBox ID="tbClave" runat="server"  MaxLength="3" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tbClave" ErrorMessage="Requiere clave" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style8">
            Establecimiento:</td>
        <td class="style2">
            <asp:TextBox ID="tbSucursal" runat="server" CssClass="form-control" Width="382px" placeholder="Nombre del Establecimiento *" MaxLength="100"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="tbSucursal" ErrorMessage="Requiere sucursal" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style8">
            Dirección:</td>
        <td class="style2">
            <asp:TextBox ID="tbDireccion" runat="server"  TextMode="MultiLine" 
                CssClass="form-control" MaxLength="150"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="tbDireccion" ErrorMessage="Requiere dirección" 
                ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style8" colspan="2">
            <asp:DropDownList ID="ddlZona" runat="server" CssClass="form-control" Visible="False">
                <asp:ListItem Selected="True">NORTE</asp:ListItem>
                <asp:ListItem>SUR</asp:ListItem>
                <asp:ListItem>CENTRO</asp:ListItem>
                <asp:ListItem>ESTE</asp:ListItem>
                <asp:ListItem>OESTE</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control" Visible="False">
                <asp:ListItem Selected="True">DIRECTO</asp:ListItem>
                <asp:ListItem>CONSESION</asp:ListItem>
                <asp:ListItem>CRESS</asp:ListItem>
                <asp:ListItem>NINGUNO</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="style8">
            Telefono: </td>
        <td class="style2">
            <asp:TextBox ID="tbTelefono" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="tbTelefono" ErrorMessage="Requiere Telefono" 
                ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style8">
            Logo<span class="style13">(.JPG)</span>:</td>
        <td class="style2">
          <asp:Image ID="Image3" runat="server" Height="47px" 
              ImageUrl="" Width="145px" />
            <span class="style13">195px/50px<br />
            </span><strong>Buscar:</strong><asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Solo son permitidos archivos .jpg!" style="color: #FF0000" ValidationExpression="^.+(.jpg|.JGP)$"> </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style2">
               
                 <asp:LinkButton ID="bGuardar0"  CssClass="btn btn-primary" runat="server" onclick="Button2_Click"  
                    Text="Subir imagen" >Subir imagen</asp:LinkButton>

            <asp:Button ID="bGuardar" runat="server" Text="Agregar" 
                onclick="bGuardar_Click" CssClass="btn btn-primary" />
        <asp:LinkButton ID="btnCancelar"  CssClass="btn btn-primary" runat="server" Text="Regresar" CausesValidation="false" OnClick="btnCancelar_Click">
</asp:LinkButton></td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;</td>
        <td class="style2">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
            <asp:Label ID="lMsj" runat="server" ForeColor="Red"></asp:Label>
            <br />
        </td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;</td>
        <td class="style2">
            &nbsp;</td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="cpTitulo">
    CREAR ESTABLECIMIENTO 
</asp:Content>

