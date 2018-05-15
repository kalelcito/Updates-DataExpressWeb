<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="archivosRecibidos.aspx.cs" Inherits="Administracion.ArchivosRecibidos" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    TRAMAS RECIBIDAS
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="modal fade " id="FiltrosC2" style="align-content: center;">
                <div class="modal-dialog modal-md" style="align-content: center;">
                    <div class="modal-content " style="align-content: center;">
                        <div class="modal-header " style="align-content: center;">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title ">Filtros</h4>
                        </div>
                        <div class="modal-body " style="align-content: center;" id="bodyFiltros">
                            <div class="row">
                                <div class="col-md-4">
                                    <h5>ID:</h5>
                                    <asp:TextBox ID="tbID" runat="server" Style="text-align: center" CssClass="form-control input-number"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbID_FilteredTextBoxExtender" runat="server" TargetControlID="tbID" FilterType="Numbers" />
                                </div>
                                <div class="col-md-4">
                                    <h5>Fecha:</h5>
                                    <div class="input-group date">
                                        <asp:TextBox ID="tbFecha" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar-o"></i>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <h5>RFC Emisor:</h5>
                                    <asp:TextBox ID="tbRFCEMI" runat="server" Style="text-align: center; text-transform: uppercase;" CssClass="form-control"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRucEmi" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcEmi" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <h5>Tipo:</h5>
                                    <asp:DropDownList ID="ddlTipo" runat="server" Style="text-align: center" CssClass="form-control" AppendDataBoundItems="true" DataSourceID="SqlDataTipoTramas" DataValueField="codigo" DataTextField="descripcion">
                                        <asp:ListItem Selected="True" Text="SELECCIONE" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <h5>Serie:</h5>
                                    <asp:TextBox ID="tbSerie" runat="server" Style="text-align: center; text-transform: uppercase;" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <h5>Folio:</h5>
                                    <asp:TextBox ID="tbFolio" runat="server" Style="text-align: center" CssClass="form-control input-number"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbFolio_FilteredTextBoxExtender" runat="server" TargetControlID="tbFolio" FilterType="Numbers" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Observaciones:</h5>
                                    <asp:TextBox ID="tbObservaciones" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <h5>Reservación/Ticket:</h5>
                                    <asp:TextBox ID="tbReserva" runat="server" Style="text-align: center;" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="bBuscar" runat="server" OnClick="bBuscarReg_Click" CssClass="btn btn-primary" Text="Buscar" ValidationGroup="validationRfc" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row form-inline">
                <fieldset>
                    <div class="col-md-12">
                    </div>
                    <div class="form-group">
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" OnClientClick="return LimpiarFiltros('bodyFiltros', 'FiltrosC2');" Text="Filtros"></asp:LinkButton>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" CssClass="btn btn-primary" Text="Limpiar Filtros" />
                    </div>
                </fieldset>
            </div>
            <br />
            <div align="center">
                <asp:GridView ID="gvTramas" runat="server" DataSourceID="SqlDataTramas" DataKeyNames="idTrama" AutoGenerateColumns="False" class=" table table-condensed table-responsive table-hover" PageSize="20" BackColor="White" AllowPaging="True" Font-Size="Smaller" AllowSorting="True" GridLines="None" OnRowDataBound="gvTramas_RowDataBound" OnPageIndexChanging="gvTramas_PageIndexChanging" OnSorting="gvTramas_Sorting" fixed-height>
                    <Columns>
                        <asp:BoundField DataField="idTrama" HeaderText="ID" SortExpression="idTrama" />
                        <asp:TemplateField HeaderText="FECHA" SortExpression="fecha">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("fecha").ToString()) ? DateTime.Parse(Eval("fecha").ToString()).ToString("dd/MM/yyyy") + "<br/>" + DateTime.Parse(Eval("fecha").ToString()).ToString("HH:mm:ss") : "" %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="userEmpleado" HeaderText="EMPLEADO" SortExpression="userEmpleado" />
                        <asp:BoundField DataField="rfcemi" HeaderText="EMISOR" SortExpression="rfcemi" />
                        <asp:BoundField DataField="serie" HeaderText="SERIE" SortExpression="serie" HeaderStyle-Width="10"></asp:BoundField>
                        <asp:BoundField DataField="folio" HeaderText="FOLIO" SortExpression="folio" HeaderStyle-Width="10"></asp:BoundField>
                        <asp:TemplateField SortExpression="noReserva" HeaderText="RESERVA/TICKET" ShowHeader="false">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("noReserva").ToString() == "" ? "N/A" : Eval("noReserva").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="descripcion" HeaderText="TIPO" SortExpression="descripcion" />
                        <asp:TemplateField HeaderText="OBSERV." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5">
                            <ItemTemplate>
                                <asp:HyperLink ID="lbPopupObs" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="OBSERVACIONES / NOTAS" data-content="" data-placement="left" min-width="800px" Visible='<%# !string.IsNullOrEmpty(Eval("observaciones").ToString()) %>'>
                                    <i class="fa fa-comments fa-2x" aria-hidden="true"></i></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TRAMA" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5">
                            <ItemTemplate>
                                <asp:HyperLink ID="lbPopupDetalles" runat="server" NavigateUrl="javascript:;" data-toggle="popover" notHtml="" title="TRAMA" data-content="" data-placement="left" data-trigger="click" min-width="800px" Visible='<%# !string.IsNullOrEmpty(Eval("Trama").ToString()) %>'>
                                    <span class="glyphicon glyphicon-file fa-2x" aria-hidden="true"></span></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <FooterStyle />
                    <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                    <RowStyle />
                    <SelectedRowStyle />
                    <SortedAscendingCellStyle />
                    <SortedAscendingHeaderStyle />
                    <SortedDescendingCellStyle />
                    <SortedDescendingHeaderStyle />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataTramas" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataTipoTramas" runat="server" SelectCommand="SELECT codigo, descripcion FROM Cat_Catalogo1_C WHERE tipo = 'TipoTrama'"></asp:SqlDataSource>
</asp:Content>
