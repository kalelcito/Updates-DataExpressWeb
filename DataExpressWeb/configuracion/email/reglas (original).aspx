<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="reglas.aspx.cs" Inherits="DataExpressWeb.distribucion" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div align="center">  
        <asp:HyperLink ID="HyperLink8" runat="server" CssClass="btn btn-primary" NavigateUrl="~/configuracion/email/addReglas.aspx">Agregar</asp:HyperLink>
        
        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-primary" NavigateUrl="~/configuracion/email/reglas.aspx" Visible="False">Consultar</asp:HyperLink>
    </div>
    <br />


</asp:Content>

<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="cpCuerpoInf">
      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="idEmailRegla" DataSourceID="reglasDataSource"  CssClass="table table-hover"
        CellPadding="4" GridLines="Horizontal"  BackColor="White"  Width="100%"
         BorderStyle="Solid" BorderWidth="1px" 
        Font-Size="XX-Small" style="margin-bottom: 0px" PageSize="15">
        <Columns>
            <asp:BoundField DataField="nombreRegla" HeaderText="NOMBRE" 
                SortExpression="nombreRegla" />
            <asp:BoundField DataField="NOMREC" HeaderText="CLIENTE" 
                SortExpression="NOMREC" />
            <asp:CheckBoxField DataField="estadoRegla" HeaderText="ACTIVO" 
                SortExpression="estadoRegla" >
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:TemplateField HeaderText="ACCIÓN" ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="Actualizar"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Cancel" Text="Cancelar"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                                <a href='modReglas.aspx?regladi=<%# Eval("idEmailRegla") %>'" >Editar</a>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle Width="100px" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No existen registros disponibles.
        </EmptyDataTemplate>
        <HeaderStyle  CssClass="info"/>
        <FooterStyle />
        <PagerStyle  />
        <RowStyle  />
        <SelectedRowStyle  />
        <SortedAscendingCellStyle  />
        <SortedAscendingHeaderStyle  />
        <SortedDescendingCellStyle  />
        <SortedDescendingHeaderStyle  />
    </asp:GridView>
    <asp:SqlDataSource ID="reglasDataSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="SELECT top 100 idEmailRegla,nombreRegla,NOMREC,estadoRegla FROM Cat_EmailsReglas,Cat_Receptor WHERE Receptor=RFCREC and eliminado='False'" 
        DeleteCommand="UPDATE Cat_EmailsReglas set eliminado='True' WHERE idEmailRegla= @idEmailRegla">
        <DeleteParameters>
            <asp:Parameter Name="idEmailRegla" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="cpTitulo">
REGLAS DE E-MAIL
</asp:Content>