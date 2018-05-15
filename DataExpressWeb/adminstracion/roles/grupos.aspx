<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Grupos.aspx.cs" Inherits="Administracion.Grupos" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function xmlUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            if (ext != 'xml' && ext != 'XML') {
                var err = new Error();
                err.message = 'Solo se aceptan archivos con extensión ".xml"';
                throw (err);
                return false;
            }
            return true;
        }
        function pdfUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            if (ext != 'pdf' && ext != 'PDF') {
                var err = new Error();
                err.message = 'Solo se aceptan archivos con extensión ".pdf"';
                throw (err);
                return false;
            }
            return true;
        }
        function ordenUpload(sender, args) {
            var filename = args.get_fileName();
            var ext = filename.substring(filename.lastIndexOf(".") + 1);
            return true;
        }
        function UploadError(sender, args) {
            var ctrlText = sender.get_element().getElementsByTagName("input");
            var ctrlSpan = sender.get_element().getElementsByTagName("span");
            for (var i = 0; i < ctrlText.length; i++) {
                if (ctrlText[i].type == "text" || ctrlText[i].type == "hidden") {
                    ctrlText[i].value = "";
                }
            }
            alertBootBox(args.get_errorMessage(), 4);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    CREAR GRUPOS
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">

    <asp:UpdatePanel ID="updTab7" runat="server">
        <ContentTemplate>
            <br />
            <div id="divGrupos" runat="server" class="rowsSpaced" align="center" visible="true">
                <br />
                <div class="row">
                    <div class="col-md-1" id="divComite" runat="server" visible="true">
                        <asp:Label ID="lbGrupo" runat="server" Text="GRUPO: " />
                    </div>
                    <div class="col-md-2" id="divIdContaG2" runat="server" visible="true">
                        <asp:TextBox ID="TextboxGrupo" runat="server" MaxLength="25" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <asp:LinkButton ID="LnkBAgregarGrupo" runat="server" CssClass="btn btn-primary btn-sm" Text="Agregar" OnClick="LnkBAgregarGrupo_Click" ValidationGroup="validationIne" Visible="true"></asp:LinkButton>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="tbMensaje" Text="" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div style="width: 20%"></div>
                    <div style="width: 60%">

                        <%------- GRIDVIE AGREGAR REGISTROS -------%>
                        <asp:GridView ID="gvRegistros" runat="server" OnRowDataBound="gvRegistros_DataBound"
                            OnRowCommand="btnEliminar_Click" class=" table table-condensed table-responsive table-hover" BackColor="White" Font-Size="Small" GridLines="None" Visible="false">
                            <Columns>
                                <asp:TemplateField ShowHeader="true" HeaderText="Orden" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="tbOrden" CssClass="form-control"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredtbGrupos" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbOrden"></asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="true" HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnEliminar" Text="Eliminar" CommandName="SelectRow" CommandArgument='<%#Eval("No")%>' CssClass="btn btn-primary">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                        </asp:GridView>
                        <%------- GRIDVIE AGREGAR REGISTROS -------%>

                        <%------- GRIDVIE MOSTRAR REGISTROS -------%>
                        <asp:GridView ID="gvResistrosDB" runat="server" AutoGenerateColumns="true"
                            class=" table table-condensed table-responsive table-hover" BackColor="White" Font-Size="Small" GridLines="None" Visible="false">
                            <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                        </asp:GridView>
                        <%------- GRIDVIE MOSTRAR REGISTROS -------%>
                    </div>
                    <div style="width: 20%"></div>
                </div>
                <div class="row">
                    <asp:LinkButton ID="ButtonGenerarGrupos" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="ButtonGenerarGrupos_Click" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="ButtonEditarGrupos" runat="server" CssClass="btn btn-primary btn-sm" Text="Editar" OnClick="ButtonEditarGrupos_Click" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="ButtonCancelarGrupos" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="ButtonCancelarGrupos_Click" Visible="false"></asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
