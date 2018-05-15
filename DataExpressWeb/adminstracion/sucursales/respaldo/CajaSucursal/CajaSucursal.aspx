<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CajaSucursal.aspx.cs" Inherits="DataExpressWeb.CajaSucursal" %>
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
            font-size: medium;
        }
        .style2
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <p class="style1">
        <asp:HyperLink ID="HyperLink8" runat="server"  CssClass="btn btn-primary" 
            NavigateUrl="~/adminstracion/sucursales/CajaSucursal/AgregarCaja.aspx" 
             >Agregar punto</asp:HyperLink>
    </p>
    <p class="style1">
        <asp:HyperLink ID="HyperLink19" runat="server" Font-Size="Small"   CssClass="btn btn-primary"
             NavigateUrl="~/adminstracion/sucursales/sucursales.aspx" CausesValidation="false" Visible="False">Regresar</asp:HyperLink>
    </p>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpTitulo" runat="server">
                <asp:Label ID="Label1" runat="server" Font-Size="Large" 
                    Text="PUNTOS DE EMISION"></asp:Label>
            </asp:Content>
<asp:Content ID="Contentt" ContentPlaceHolderID="cpCuerpoInf" runat="server">

    <div align="center">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" DataKeyNames="idCaja" DataSourceID="SqlDataSource1" 
                    ForeColor="#333333" GridLines="None" AllowPaging="True" CssClass="table table-hover">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="idCaja" HeaderText="idCaja" InsertVisible="False" 
                            ReadOnly="True" SortExpression="idCaja" Visible="False" />
                <asp:BoundField DataField="idSucursal" HeaderText="idSucursal" 
                            SortExpression="idSucursal" Visible="False" />
                <asp:BoundField DataField="nombrePtoEmi" HeaderText="NOMBRE" 
                            SortExpression="nombre" />
                <asp:BoundField DataField="NumeroRentas" HeaderText="TIPO DE DOCUMENTO" 
                            SortExpression="NumeroRentas" />
                <asp:BoundField DataField="estab" HeaderText="ESTABLECIMIENTO" 
                            SortExpression="estab" />
                <asp:BoundField DataField="ptoEmi" HeaderText="PUNTO DE EMISIÓN" 
                            SortExpression="ptoEmi" />
                <asp:BoundField DataField="estadoPro" HeaderText="AMBIENTE" 
                            SortExpression="estadoPro" />
                <asp:BoundField DataField="impresora" HeaderText="IMPRESORA" 
                            SortExpression="impresora" />
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                                    CommandName="Update" Text="Actualizar"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                                    CommandName="Cancel" Text="Cancelar"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <a href='modificarCaja.aspx?idmrdxbdi=<%# Eval("idCaja") %>'>Editar</a>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                        CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
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
        </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                    DeleteCommand="DELETE FROM [Cat_CajasSucursal] WHERE [idCaja] = @idCaja" 
                    InsertCommand="INSERT INTO [Cat_CajasSucursal] ([idSucursal], [NumeroSisposnet], [NumeroRentas], [estab], [ptoEmi], [secuencial], [fecha], [estado], [estadoFE], [estadoPro], [nombre], [impresora]) VALUES (@idSucursal, @NumeroSisposnet, @NumeroRentas, @estab, @ptoEmi, @secuencial, @fecha, @estado, @estadoFE, @estadoPro, @nombre, @impresora)" 
                    SelectCommand="SELECT idCaja, idSucursal, NumeroSisposnet, NumeroRentas, estab, ptoEmi, secuencial, fecha, estado, estadoFE, estadoPro, nombrePtoEmi, impresora FROM Cat_CajasSucursal" 
                    UpdateCommand="UPDATE [Cat_CajasSucursal] SET [idSucursal] = @idSucursal, [NumeroSisposnet] = @NumeroSisposnet, [NumeroRentas] = @NumeroRentas, [estab] = @estab, [ptoEmi] = @ptoEmi, [secuencial] = @secuencial, [fecha] = @fecha, [estado] = @estado, [estadoFE] = @estadoFE, [estadoPro] = @estadoPro, [nombre] = @nombre, [impresora] = @impresora WHERE [idCaja] = @idCaja">
                    <DeleteParameters>
                        <asp:Parameter Name="idCaja" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="idSucursal" Type="Int32" />
                        <asp:Parameter Name="NumeroSisposnet" Type="Int32" />
                        <asp:Parameter Name="NumeroRentas" Type="String" />
                        <asp:Parameter Name="estab" Type="String" />
                        <asp:Parameter Name="ptoEmi" Type="String" />
                        <asp:Parameter Name="secuencial" Type="String" />
                        <asp:Parameter Name="fecha" Type="DateTime" />
                        <asp:Parameter Name="estado" Type="String" />
                        <asp:Parameter Name="estadoFE" Type="String" />
                        <asp:Parameter Name="estadoPro" Type="String" />
                        <asp:Parameter Name="nombre" Type="String" />
                        <asp:Parameter Name="impresora" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="idSucursal" Type="Int32" />
                        <asp:Parameter Name="NumeroSisposnet" Type="Int32" />
                        <asp:Parameter Name="NumeroRentas" Type="String" />
                        <asp:Parameter Name="estab" Type="String" />
                        <asp:Parameter Name="ptoEmi" Type="String" />
                        <asp:Parameter Name="secuencial" Type="String" />
                        <asp:Parameter Name="fecha" Type="DateTime" />
                        <asp:Parameter Name="estado" Type="String" />
                        <asp:Parameter Name="estadoFE" Type="String" />
                        <asp:Parameter Name="estadoPro" Type="String" />
                        <asp:Parameter Name="nombre" Type="String" />
                        <asp:Parameter Name="impresora" Type="String" />
                        <asp:Parameter Name="idCaja" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </div>

</asp:Content>
