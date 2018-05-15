<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FacturaGlobal.aspx.cs" Inherits="DataExpressWeb.adminstracion.FacturaGlobal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .tamaño {
            font-size: 17.1px;
            text-align: right;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    FACTURAS GLOBALES
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div align="center" class="rowsSpaced">
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-2">
                        <strong>RFC:</strong><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DropDownListRFC" InitialValue="" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="datosBusqueda"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="DropDownListRFC" CssClass="form-control" runat="server"
                            DataSourceID="SqlDataRFC" DataTextField="RFCREC" DataValueField="RFCREC" AppendDataBoundItems="true">
                            <asp:ListItem Value="" Text="Seleccione RFC"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <strong>Periodo:</strong>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownListPeriodo" InitialValue="" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="datosBusqueda"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="DropDownListPeriodo" runat="server" CssClass="form-control" Style="text-align: center;" ValidationGroup="datosBusqueda">
                            <asp:ListItem Value="" Text="Periodo"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Diario"></asp:ListItem>
                            <asp:ListItem Value="15" Text="15 Días"></asp:ListItem>
                            <asp:ListItem Value="30" Text="Fin de mes"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary" Visible="TRUE" Style="width: 100%;" OnClick="bAgregarRFC_Click" ValidationGroup="datosBusqueda">Guardar</asp:LinkButton>
                    </div>
                </div>
            </div>
            <asp:SqlDataSource ID="SqlDataRFC" runat="server" SelectCommand="SELECT DISTINCT RFCREC FROM Cat_Receptor"></asp:SqlDataSource>
            <br></br>

            <asp:GridView ID="gvRFC" runat="server" DataSourceID="SqlDataSourceRfc" DataKeyNames="id" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" AllowPaging="True" AllowSorting="True" Font-Size="Small" GridLines="None" OnRowEditing="gvConceptos_RowEditing" OnRowUpdating="gvConceptos_RowUpdating " OnRowCancelingEdit="gvConceptos_RowCancelingEdit" OnPageIndexChanging="gvEmpleados_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id" ReadOnly="true" />
                    <asp:BoundField DataField="rfc" HeaderText="RFC" SortExpression="rfc" ReadOnly="true" />

                    <asp:TemplateField HeaderText="PERIODO">
                        <ItemTemplate>
                            <asp:Label ID="lbTiempo" runat="server" Text='<%# Bind("tiempo") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div class="input-group">
                                <asp:DropDownList ID="DropDownListPeriodo2" runat="server" CssClass="form-control" Style="text-align: center;">
                                    <asp:ListItem Value="1" Text="Diario"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15 Días"></asp:ListItem>
                                    <asp:ListItem Value="30" Text="Fin de mes"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar" ControlStyle-CssClass="btn btn-primary btn-xs" />
                    <asp:TemplateField ShowHeader="False" HeaderText="ACCIONES" ItemStyle-Width="14%">
                        <ItemTemplate>
                            <asp:LinkButton ID="bEliminar" runat="server" OnClientClick="return confirm('¿Deseas eliminar el RFC?');" CausesValidation="False" CommandName="Delete" Text="Eliminar" CssClass="btn btn-primary btn-xs">
                            </asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <strong>No hay Datos</strong>
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
            <asp:SqlDataSource ID="SqlDataSourceRfc" runat="server" SelectCommand="SELECT id,rfc,(CASE tiempo WHEN 30 THEN 'FIN DE MES' WHEN 15 THEN '15 DÍAS' WHEN 1 THEN 'DIARIO' ELSE convert(varchar(max), tiempo) end) as tiempo FROM Cat_Aerolinea" DeleteCommand="DELETE FROM Cat_Aerolinea where id = @id" UpdateCommand="UPDATE Cat_Aerolinea SET tiempo=@tiempo where id=@id">
                <DeleteParameters>
                    <asp:Parameter Name="id" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="id" />
                    <asp:Parameter Name="tiempo" />
                </UpdateParameters>
            </asp:SqlDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpPie" runat="server">
</asp:Content>
