<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="modificar_impuestos.aspx.cs" Inherits="Configuracion.modificar_impuestos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">



        .style2
    {
        font-size: x-small;
    }



        </style>
    </asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    MODIFICAR IMPUESTOS 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
    
            <table class="table nav-justified">
                <tr class="info">
                    <td class="auto-style6" style="color: #FFFFFF; text-align: right""><strong>Nombre</strong></td>
                    <td class="text-right" class="auto-style4"></td>
                    <td class="auto-style6" style="color: #FFFFFF; text-align: left"><strong>Activar/Desactivar</strong></td>
                </tr>
                <tr>
                    <td class="text-right">Descripción:</td>
                    <td class="text-right">&nbsp;</td>
                    <td class="text-left">
                        <asp:TextBox ID="tbDescrip" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbDescrip" ErrorMessage="Requiere nombre del Impuesto" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="text-right">Valor:</td>
                    <td class="text-right">&nbsp;</td>
                    <td class="text-left">
                        <asp:TextBox ID="tbValor" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="text-right">Codigo:</td>
                    <td class="text-right">&nbsp;</td>
                    <td class="text-left">
                        <asp:TextBox ID="tbCodigo" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="text-right">Comentarios:</td>
                    <td class="text-right">&nbsp;</td>
                    <td class="text-left">
                        <asp:TextBox ID="tbComentarios" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="text-right">Tipo:</td>
                    <td class="text-right">&nbsp;</td>
                    <td class="text-left">
                        <asp:DropDownList ID="tbTipo" runat="server" CssClass="form-control">
                            <asp:ListItem Value="IVARetencion">IVA</asp:ListItem>
                            <asp:ListItem Value="AIRretencion">Rentas</asp:ListItem>
                            <asp:ListItem Value="ISDRetencion">ISD</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
          
            <asp:Label ID="lMsj" runat="server" ForeColor="#F93200"></asp:Label>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tbDescrip" 
                ErrorMessage="Tienes que escribir la Descripción del Impuesto" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="bModificar" runat="server" CssClass="btn btn-primary" onclick="bModificar_Click" 
                Text="Actualizar" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCancelar" CssClass="btn btn-primary" runat="server" Text="Regresar" CausesValidation="false" OnClick="btnCancelar_Click">
</asp:Button>
&nbsp;&nbsp;
            <br />
</asp:Content>
