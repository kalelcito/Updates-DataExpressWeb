<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Impuestos.aspx.cs" Inherits="Configuracion.Impuestos" %>
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
    CATÁLOGO DE IMPUESTOS  
</asp:Content>

  <%--  <p class="green_right">
        <asp:HyperLink ID="HyperLink8" runat="server" CssClass="hipermenu" NavigateUrl="~/adminstracion/roles/crear_rol.aspx" >Agregar</asp:HyperLink>
    </p>--%>


<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:HyperLink ID="HyperLink8" runat="server" CssClass="btn btn-primary" Font-Bold="True" Font-Size="Small" NavigateUrl="~/configuracion/impuestos/crear_impuestos.aspx">Agregar</asp:HyperLink>
</asp:Content>
<asp:Content ID="Content32" ContentPlaceHolderID="cpCuerpoInf" runat="server">

   <div align="center"> <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="id" DataSourceID="SqlDataSource3"  CssClass="table table-hover" style="font-size: xx-small" >
        <Columns>
<%--            <asp:BoundField DataField="facturas" HeaderText="Documentos" 
                SortExpression="facturas" />--%>
 <%--           <asp:CheckBoxField DataField="nc" HeaderText="Crear NC" 
                SortExpression="nc">
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>--%>
<asp:BoundField DataField="id" HeaderText="id" SortExpression="id" ReadOnly="True" Visible="False" InsertVisible="False"></asp:BoundField>
            <asp:BoundField DataField="descripcion" HeaderText="Descripción" 
                SortExpression="descripcion" />
            <asp:BoundField DataField="valor" HeaderText="Porcentaje" SortExpression="valor" />
            <asp:BoundField DataField="codigo" HeaderText="Codigo" SortExpression="codigo" />
            <asp:BoundField DataField="comentarios" HeaderText="Comentarios" SortExpression="comentarios" />
            <asp:BoundField DataField="tipo" HeaderText="tipo" SortExpression="tipo" Visible="False" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <a href='modificar_impuestos.aspx?id=<%# Eval("id") %>'>Editar</a>
                    <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" 
                        CommandName="Delete" 
                        OnClientClick="return confirm('¿Deseas eliminar el impuesto?');" 
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
    <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
    <br />
    </span>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        DeleteCommand="DELETE FROM Cat_CatImpuestos_C WHERE (id=@id)" 
        SelectCommand="SELECT id, descripcion, valor, codigo, comentarios, tipo FROM Cat_CatImpuestos_C WHERE (tipo = 'IVARetencion') OR (tipo = 'ISDRetencion') OR (tipo = 'AIRRetencion')">
        <DeleteParameters>
            <asp:Parameter Name="idRol" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <br class="auto-style2" />
    </asp:Content>
