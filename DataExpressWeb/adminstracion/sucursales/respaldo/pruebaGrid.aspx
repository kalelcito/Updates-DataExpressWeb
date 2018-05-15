<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pruebaGrid.aspx.cs" Inherits="DataExpressWeb.adminstracion.sucursales.pruebaGrid"
    MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="MainContent">
    <asp:Label ID="lblResults" runat="server" />
    <asp:GridView ID="gvSuppliers" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="ShipperID" DataSourceID="sdsSuppliers" EmptyDataText="There are no data records to display."
        OnRowCommand="gvSuppliers_RowCommand" OnRowCreated="gvSuppliers_RowCreated" OnRowDeleted="gvSuppliers_RowDeleted"
        OnRowUpdated="gvSuppliers_RowUpdated" ShowFooter="true" Style="margin-top: 12px;">
        <EmptyDataTemplate>
            <div style="float: left; width: 100px;">
                <asp:LinkButton ID="btnInsert" runat="server" CommandName="EmptyDataTemplateInsert"
                    Text="Insert" />
            </div>
            <div style="float: left; width: 175px;">
                <asp:TextBox ID="txtCompanyName" runat="server" />
            </div>
            <div style="float: right;">
                <asp:TextBox ID="txtPhone" runat="server" />
            </div>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit"
                        Text="Edit" />
                    <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete"
                        Text="Delete" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />
                    <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel"
                        Text="Cancel" />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="btnInsert" runat="server" CommandName="FooterInsert" Text="Insert" />
                </FooterTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ShipperID" HeaderText="ShipperID" ReadOnly="True" SortExpression="ShipperID" />
            <asp:TemplateField HeaderText="Company Name">
                <ItemTemplate>
                    <%# Eval("CompanyName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtCompanyName" runat="server" Text='<%# Bind("CompanyName") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtCompanyName" runat="server" />
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
                    <%# Eval("Phone") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPhone" runat="server" Text='<%# Bind("Phone") %>' />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtPhone" runat="server" />
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsSuppliers" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>"
        DeleteCommand="DELETE FROM [Shippers] WHERE [ShipperID] = @ShipperID" InsertCommand="INSERT INTO [Shippers] ([CompanyName], [Phone]) VALUES (@CompanyName, @Phone)"
        OnInserted="sdsSuppliers_Inserted" SelectCommand="SELECT * FROM [Shippers]" UpdateCommand="UPDATE [Shippers] SET [CompanyName] = @CompanyName, [Phone] = @Phone WHERE [ShipperID] = @ShipperID">
        <InsertParameters>
            <asp:Parameter Name="CompanyName" Type="String" />
            <asp:Parameter Name="Phone" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="CompanyName" Type="String" />
            <asp:Parameter Name="Phone" Type="String" />
            <asp:Parameter Name="ShipperID" Type="Int32" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="ShipperID" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
