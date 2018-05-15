<%@ Page Title="Ver Registros" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerRegistros.aspx.cs" Inherits="DataExpressWeb.VerRegistros" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Table ID="TableRegArch" runat="server" Height="80px" Width="140px" CellPadding="4">
    <asp:TableRow>
        <asp:TableCell><strong>Nombre:</strong></asp:TableCell>
        <asp:TableCell><asp:TextBox ID="TextBoxNombre" runat="server"></asp:TextBox></asp:TableCell>
        <asp:TableCell><strong>Tipo:</strong></asp:TableCell>
        <asp:TableCell><asp:TextBox ID="TextBoxTipo" runat="server"></asp:TextBox></asp:TableCell>
        <asp:TableCell><asp:Button ID="ButtonSearch" runat="server" Text="Buscar" CssClass="btGeneralG" onclick="ButtonSearch_Click"/></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell><strong>NoOrden:</strong></asp:TableCell>
        <asp:TableCell><asp:TextBox ID="TextBoxNoOrden" runat="server"></asp:TextBox></asp:TableCell>
        <asp:TableCell><strong>Fecha:</strong></asp:TableCell>
        <asp:TableCell><asp:TextBox ID="TextBoxFecha" runat="server"></asp:TextBox></asp:TableCell>
    </asp:TableRow>
    </asp:Table><br />

<asp:GridView ID="gv" runat="server" DataSourceID="SqlDataSource2" 
        AutoGenerateColumns="False" Height="16px" CellPadding="4" AllowPaging="True"
        ForeColor="#333333" Width="900px" Font-Bold="False" PersistedSelection="True"
        onselectedindexchanged="gv_SelectedIndexChanged1">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre" 
            HeaderStyle-Width="200px">
<HeaderStyle Width="200px"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="tipo" HeaderText="Tipo" SortExpression="tipo"  
            HeaderStyle-Width="200px">
<HeaderStyle Width="200px"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="noorden" HeaderText="NoOrden" 
            SortExpression="noorden" HeaderStyle-Width="200px">
<HeaderStyle Width="200px"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" 
            HeaderStyle-Width="200px">
<HeaderStyle Width="200px"></HeaderStyle>
        </asp:BoundField>
    </Columns>
    <EditRowStyle BackColor="#2461BF"/>
    <EmptyDataTemplate>
        No Existen Registros.
    </EmptyDataTemplate>
    <FooterStyle BackColor="#69C100" Font-Bold="False" ForeColor="White"/>
    <HeaderStyle BackColor="#69C100" Font-Bold="False" ForeColor="White"/>
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="False" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#F5F7FB" />
    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
    <SortedDescendingCellStyle BackColor="#E9EBEF" />
    <SortedDescendingHeaderStyle BackColor="#4870BE" />
</asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>">
        </asp:SqlDataSource>
    </asp:Content>