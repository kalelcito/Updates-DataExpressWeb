<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DescargarZip.aspx.cs" Inherits="DataExpressWeb.DescargarZip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    CONSULTAR DESCARGAS ZIP
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:GridView ID="gvDescargasZip" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" DataSourceID="SqlDataSourcezIP">
                <Columns>
                    <asp:TemplateField HeaderText="FECHA DE SOLICITUD" SortExpression="FechaSoltd">
                        <ItemTemplate>
                            <asp:Label ID="FechaSoltd" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("FechaSoltd").ToString()) ? DateTime.Parse(Eval("FechaSoltd").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FechaSoltd").ToString()).ToString("hh:mm:ss") : "" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Rango" HeaderText="RANGO" SortExpression="Rango" />
                    <asp:BoundField DataField="NroDocus" HeaderText="N° DE DOCUMENTOS" SortExpression="NroDocus" />
                    <%-- <asp:BoundField DataField="Status" HeaderText="STATUS" SortExpression="Status" />--%>

                    <asp:TemplateField HeaderText="STATUS" SortExpression="Status" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image runat="server" ImageUrl='<%# string.Format("~/Imagenes/{0}.png", Eval("Status")) %>' data-toggle="tooltip" data-placement="top" title='<%# Eval("Status").ToString() == "1" ? "GENERADO" : (Eval("Status").ToString() == "2" ? "En Proceso" : "Desconocido") %>'></asp:Image>
                        </ItemTemplate>
                        <ControlStyle Height="18px" Width="18px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="FECHA FINAL" SortExpression="FechaFIn">
                        <ItemTemplate>
                            <asp:Label ID="FechaFIn" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("FechaFIn").ToString()) ? DateTime.Parse(Eval("FechaFIn").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("FechaFIn").ToString()).ToString("hh:mm:ss") : "" %>' Visible='<%# Eval("ZipDir").ToString() != "" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Descargar" SortExpression="ZipDir">
                        <ItemTemplate>
                            <asp:HyperLink ID="lbDescZip" runat="server" CssClass="btn btn-primary" NavigateUrl='<%# string.Format("~/download.aspx.?file={0}",Eval("ZipDir")) %>' EnableEventValidation="false" Visible='<%# Eval("ZipDir").ToString() != "" %>'>Descargar</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle CssClass="table" />
                <EmptyDataTemplate>
                    <strong>No existen datos.</strong>
                </EmptyDataTemplate>
                <FooterStyle />
                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                <PagerSettings />
                <RowStyle />
                <SelectedRowStyle />
                <SortedAscendingCellStyle />
                <SortedAscendingHeaderStyle />
                <SortedDescendingCellStyle />
                <SortedDescendingHeaderStyle />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataSourcezIP" runat="server" SelectCommand="select FechaSoltd,Rango,NroDocus,Status,FechaFIn,ZipDir from Dat_Descargas_Zip order by 1 desc"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpPie" runat="server">
</asp:Content>
