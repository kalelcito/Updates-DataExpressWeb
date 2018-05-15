<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="editemail.aspx.cs" Inherits="DataExpressWeb.configuracion.email.Editemail" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="cpCabecera">
    <script type="text/javascript">
        function validarMensajes() {
            var ddl = $('#<%= ddlMensaje.ClientID %>');
            if (ddl.val() == '0') {
                alertBootBox("Tiene que escoger un mensaje a editar", 4);
                return false;
            } else if ($('#summernoteDiv').summernote('isEmpty')) {
                alertBootBox("El mensaje no puede quedar vacío", 4);
                return false;
            }
            save();
            return true;
        }
        function actualizaPagina() {
            window.location.href = './editemail.aspx';
        }
        function save() {
            setSn(ReformatTextEditor(), '#<%= txtEditor.ClientID %>');
        }
        function recargarEditor() {
            var val = $('#<%= txtEditor.ClientID %>').val();
            setSn(val, '#<%= txtEditor.ClientID %>');
        }
    </script>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="cpCuerpoSup">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="txtEditor" Value=""></asp:HiddenField>
            <div align="center">
                <div class="col-md-3" style="align-content: center">
                </div>
                <div class="col-md-6">
                    <asp:DropDownList ID="ddlMensaje" runat="server" Style="text-align: center;" DataSourceID="SqlDataSource1" DataTextField="descripcion" DataValueField="idMensaje" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AppendDataBoundItems="true">
                        <asp:ListItem Text="Seleccionar Mensaje" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="ddlMensaje" SetFocusOnError="true" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-3" style="align-content: center">
                </div>
            </div>
            <br />
            <div align="center">
                <asp:Label ID="Nombremsj" runat="server" Style="font-size: large; font-weight: 700"></asp:Label>
            </div>
            <br />
            <div align="center" id="divMensajes" runat="server" visible="false">
                <div id="summernoteDiv"></div>
            </div>
            <asp:Button ID="Limpiar" runat="server" CssClass="btn btn-primary" OnClientClick="actualizaPagina()" UseSubmitBehavior="false" Text="Limpiar"></asp:Button>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="Guardar" runat="server" CssClass="btn btn-primary" OnClientClick="return validarMensajes();" OnClick="Submit" Text="Guardar"></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT * FROM [Cat_Mensajes] order by idMensaje asc"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cpTitulo">
    MODIFICAR MENSAJES
</asp:Content>
