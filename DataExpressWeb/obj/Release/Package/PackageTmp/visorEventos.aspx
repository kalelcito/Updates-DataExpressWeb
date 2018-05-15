<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="visorEventos.aspx.cs" Inherits="DataExpressWeb.VisorEventos" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="cpTitulo" runat="server">
    <div runat="server" id="divTitulo" visible="true">
        VISOR DE EVENTOS (LOGS)
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="rowsSpaced">
                <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-1"><strong>FECHA:</strong></div>
                    <div class="col-md-3">
                        <div class="input-group date">
                            <asp:TextBox ID="tbFecha" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon">
                                <i class="fa fa-calendar-o"></i>
                            </span>
                        </div>
                    </div>
                    <div class="col-md-1"><strong>CÓDIGO:</strong></div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlEstado" runat="server" Style="text-align: center" CssClass="form-control">
                            <asp:ListItem Value="">Todos</asp:ListItem>
                            <asp:ListItem Value="0">Desconocido</asp:ListItem>
                            <asp:ListItem Value="1">Base de Datos</asp:ListItem>
                            <asp:ListItem Value="2">Comprobantes</asp:ListItem>
                            <asp:ListItem Value="3">Timbrado</asp:ListItem>
                            <asp:ListItem Value="4">Documentos</asp:ListItem>
                            <asp:ListItem Value="5">Cancelacion</asp:ListItem>
                            <asp:ListItem Value="6">Base de Datos Recepcion</asp:ListItem>
                            <asp:ListItem Value="7">Comprobantes Recepcion</asp:ListItem>
                            <asp:ListItem Value="8">Documentos Recepcion</asp:ListItem>
                            <asp:ListItem Value="9">Cancelacion Recepcion</asp:ListItem>
                            <asp:ListItem Value="10">Proceso Automático</asp:ListItem>
                            <asp:ListItem Value="100">Portal Web</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2"></div>
                </div>
                <div class="row">
                    <div class="col-md-2"><strong>DESCRIPCIÓN:</strong></div>
                    <div class="col-md-10">
                        <asp:TextBox ID="tbDescripción" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                    </div>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="lbBuscar" runat="server" CssClass="btn btn-primary" OnClick="lbBuscar_Click">Buscar</asp:LinkButton>
            <asp:LinkButton ID="lbLimpiar" runat="server" CssClass="btn btn-primary" OnClick="lbLimpiar_Click">Limpiar Búsqueda</asp:LinkButton>
            </br>
            <br />
            <asp:GridView ID="gvEventos" runat="server" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Smaller" GridLines="None" DataSourceID="SqlDataSource1" OnRowDataBound="gvEventos_RowDataBound" OnPageIndexChanging="gvEventos_PageIndexChanging" OnSorting="gvEventos_Sorting">
                <Columns>
                    <asp:BoundField DataField="codigo" HeaderText="CODIGO" SortExpression="codigo" />
                    <asp:BoundField DataField="origen" HeaderText="ORIGEN" SortExpression="origen" />
                    <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" SortExpression="descripcion" />
                    <asp:TemplateField HeaderText="FECHA" SortExpression="fecha">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("fecha").ToString()) ? DateTime.Parse(Eval("fecha").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("fecha").ToString()).ToString("hh:mm:ss") : "" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DETALLES" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="lbPopupDetalles" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="DETALLES TÉCNICOS" data-content="" data-placement="left" min-width="800px" Visible='<%# !string.IsNullOrEmpty(Eval("detallesTecnicos").ToString()) %>'>
                                <span class="glyphicon glyphicon-comment" aria-hidden="true"></span></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle CssClass="table" />
                <EmptyDataTemplate>
                    No existen datos.
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
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
</asp:Content>
