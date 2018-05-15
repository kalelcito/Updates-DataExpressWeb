<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="modificarPerfil.aspx.cs" Inherits="Administracion.ModificarPerfil" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function ProfileValidation() {
            if (!Page_ClientValidate()) {
                showDivOnModalLevel('modalValidation', 4);
                return false;
            }
            return true;
        }
        function resetPass() {
            $('pwd-control').val('');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div id="modalValidation" class="modal-div">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <asp:Label ID="ValidationList" runat="server"></asp:Label>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div align="center">
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-1" align="justified">
                        <strong>NOMBRE:</strong>
                    </div>
                    <div class="col-md-5">
                        <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" Style="text-align: center" placeholder="DATAEXPRESS LATINOAMÉRICA *"></asp:TextBox>
                    </div>
                    <div class="col-md-1" align="justified">
                        <strong>USUARIO:</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbUsername" runat="server" MaxLength="15" Style="text-align: center" ReadOnly="True" CssClass="form-control" placeholder="EMPLE110001 *"></asp:TextBox>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-1" align="justified">
                        <asp:Label ID="lRol" runat="server" Text="PERFIL:" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lMail" runat="server" Text="E-MAIL:"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <asp:TextBox ID="tbRol" runat="server" Style="text-align: center" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        <asp:TextBox ID="tbEmail" runat="server" CssClass="form-control" placeholder="email@dominio.com"></asp:TextBox>
                    </div>
                    <div class="col-md-1" align="justified">
                        <strong>ESTADO:</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbStatus" runat="server" Style="text-align: center" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-1" align="justified">
                        <asp:Label ID="lSucursal" runat="server" Text="SUCURSAL:" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <asp:TextBox ID="tbSucursal" runat="server" Style="text-align: center" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                    <%--<div class="col-md-4" align="left">
                        <asp:CheckBox ID="cbFacturaRestaurantes" runat="server" Checked="false" Enabled="false" />
                        <asp:Label ID="lFacturaRestaurantes" runat="server" Text="ASISTENTE DE FACT. SIMPLIFICADO"></asp:Label>
                    </div>--%>
                    <div class="col-md-1"></div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12" align="justified">
                        <asp:CheckBox ID="cbCambiarPass" runat="server" AutoPostBack="True" CausesValidation="false" OnCheckedChanged="cbCambiarPass_CheckedChanged" onchange="resetPass();" />
                        <strong>CAMBIAR CONTRASEÑA</strong>
                    </div>
                </div>
                <div id="divCambiarPass" runat="server" style="display: none;">
                    <br />
                    <div class="row">
                        <div class="col-md-4">CONTRASEÑA ANTERIOR</div>
                        <div class="col-md-4">NUEVA CONTRASEÑA</div>
                        <div class="col-md-4">CONFIRMAR NUEVA CONTRASEÑA</div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:TextBox ID="CurrentPassword" runat="server" CssClass="form-control pwd-control" TextMode="Password" MaxLength="15" Style="text-align: center"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="CurrentPassword_RequiredFieldValidator" runat="server" ControlToValidate="CurrentPassword" ErrorMessage="La contraseña es obligatoria." ValidationGroup="Profile" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="NewPassword" runat="server" CssClass="form-control pwd-control" TextMode="Password" MaxLength="15" Style="text-align: center"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NewPassword_RequiredFieldValidator" runat="server" ControlToValidate="NewPassword" ErrorMessage="La nueva contraseña es obligatoria." ValidationGroup="Profile" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="form-control pwd-control" TextMode="Password" MaxLength="15" Style="text-align: center"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ConfirmNewPassword_RequiredFieldValidator" runat="server" ControlToValidate="ConfirmNewPassword" ErrorMessage="Confirmar la nueva contraseña es obligatorio." ValidationGroup="Profile" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="ConfirmNewPassword_CompareValidator" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" ErrorMessage="Confirmar la nueva contraseña debe coincidir con la entrada Nueva contraseña." ValidationGroup="Profile" Display="None" Enabled="false"></asp:CompareValidator>
                        </div>
                    </div>
                </div>
                <br />
                <asp:Button ID="bModificar" runat="server" Text="Actualizar" OnClick="bModificar_Click" CssClass="btn btn-primary" OnClientClick="return ProfileValidation();" ValidationGroup="Profile" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    INFORMACIÓN DE LA CUENTA
</asp:Content>
