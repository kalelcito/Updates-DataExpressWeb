<%@ Page Title="Mostrar" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Mostrar.aspx.cs" Inherits="ups.Mostrar" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="cpCabecera">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="cpCuerpoSup">
    <div class="rowsSpaced">
        <div class="row">
            <div class="col-md-3">
                <strong>Directorio de Documentos:</strong>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDirdocs">
            </asp:RequiredFieldValidator>
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="tbDirdocs" runat="server" ReadOnly="True" CssClass="form-control" placeholder="D:\ *">
                </asp:TextBox>
            </div>
        </div>
        <div class="row" style="display: none;">
            <div class="col-md-3">
                <strong>Directorio de Txt:</strong>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDirtxt">
            </asp:RequiredFieldValidator>
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="tbDirtxt" runat="server" ReadOnly="True" CssClass="form-control" placeholder="D:\ *">
                </asp:TextBox>
            </div>
        </div>
        <div class="row" style="display: none;">
            <div class="col-md-3">
                <strong>Directorio de Xml Base:</strong>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDirxmlbase">
            </asp:RequiredFieldValidator>
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="tbDirxmlbase" runat="server" ReadOnly="True" CssClass="form-control" placeholder="D:\ *">
                </asp:TextBox>
            </div>
        </div>
        <div class="row" style="display: none;">
            <div class="col-md-3">
                <strong>Directorio de Respaldo:</strong>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDirrespaldo">
            </asp:RequiredFieldValidator>
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="tbDirrespaldo" runat="server" ReadOnly="True" CssClass="form-control" placeholder="D:\ *">
                </asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <strong>Directorio de Certificados:</strong>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDircerti" Visible="False">
            </asp:RequiredFieldValidator>
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="tbDircerti" runat="server" ReadOnly="True" CssClass="form-control" placeholder="D:\ *">
                </asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <strong>Directorio de Llaves:</strong>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDirllaves" Visible="False">
            </asp:RequiredFieldValidator>
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="tbDirllaves" runat="server" ReadOnly="True" CssClass="form-control" placeholder="D:\ *">
                </asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4"></div>
            <div class="col-md-4">
                <asp:Button ID="bModificar" runat="server" Text="Modificar" OnClick="bModificar_Click" CssClass="btn btn-primary" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

            <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" Text="Actualizar" Visible="False" CssClass="btn btn-primary" />
            </div>
            <div class="col-md-4"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cpTitulo">
    ADMINISTRAR CARPETAS
</asp:Content>
