<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="servermail.aspx.cs" Inherits="DataExpressWeb.configuracion.email.Servermail" Async="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="cpCabecera">
    <style type="text/css">
        .panel {
            background: rgba(0,0,0,0) !important;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="cpCuerpoSup">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="rowsSpaced">
                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-default">
                        <div class="panel-heading" role="tab" id="headingOne">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">DATOS SMTP</a>

                            </h4>
                        </div>
                        <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-3">
                                        <strong>Servidor SMTP:</strong>
                   
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="tbServidor" runat="server" Width="349px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="mail.dominio.com *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbServidor" ErrorMessage="*" ForeColor="Red" ValidationGroup="validateMail"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-3">
                                        <strong>Puerto:</strong>                   
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="tbPuerto" runat="server" Width="349px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="2525 *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPuerto" ErrorMessage="*" ForeColor="Red" ValidationGroup="validateMail"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-3">
                                        <strong>Usuario:</strong>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="tbUsuario" runat="server" Width="347px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="username@dominio.com *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        &nbsp;
                       
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbUsuario" ErrorMessage="*" ForeColor="Red" ValidationGroup="validateMail"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-3">
                                        <strong>Contraseña:</strong>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="tbPassword" runat="server" Width="346px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="Contraeña *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        &nbsp;
                       
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbPassword" ErrorMessage="*" ForeColor="Red" ValidationGroup="validateMail"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-3">
                                        <strong>SSL:</strong>
                                    </div>
                                    <div class="col-md-3" align="left">
                                        <asp:CheckBox ID="cbSSL" runat="server" Enabled="False" />
                                    </div>
                                    <div class="col-md-3"></div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-3">
                                        <strong>E-Mail de envio:</strong>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="tbEmailEnvio" runat="server" Width="348px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="email@dominio.com *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbEmailEnvio" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEmailEnvio" ErrorMessage="E-Mail Invalido" ForeColor="Red" ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$" ValidationGroup="validateMail"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="rowsSpaced">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-3">
                        <strong>E-Mail de comprobantes (BCC):</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbBcc" runat="server" Width="348px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="email@dominio.com *"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        &nbsp;
                   
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-3">
                        <strong>E-Mail de notificaciones (Web):</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbEmailNotificacion" runat="server" Width="348px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="email@dominio.com *"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbEmailNotificacion" ErrorMessage="*" ForeColor="Red" ValidationGroup="validateMail"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-3">
                        <strong>E-Mail de notificaciones (PMS/POS):</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbEmailOpera" runat="server" Width="348px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="email@dominio.com *"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbEmailOpera" ErrorMessage="*" ForeColor="Red" ValidationGroup="validateMail"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-3">
                        <strong>E-Mail de recepción:</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbEmailRecepcion" runat="server" Width="348px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="email@dominio.com *"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        &nbsp;
                   
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-3">
                        <strong>E-Mail de alta de Clientes/Proveedores:</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbEmailUsers" runat="server" Width="348px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="email@dominio.com *"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        &nbsp;
                   
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-3">
                        <strong>E-Mail de Folios mensuales:</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbEmailFolios" runat="server" Width="348px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="email@dominio.com *"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        &nbsp;
                   
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-3">
                        <strong>E-Mail notificacion recepcion:</strong>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbEmailRecepBcc" runat="server" Width="348px" ReadOnly="True" Style="text-align: center" CssClass="form-control" placeholder="email@dominio.com *"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        &nbsp;
                   
                    </div>
                </div>
            </div>
            <br />
            <asp:Button ID="bModificar" runat="server" Style="text-align: center" Text="Modificar" OnClick="bModificar_Click" CssClass="btn btn-primary" />

            <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" Text="Actualizar" Visible="False" CssClass="btn btn-primary" ValidationGroup="validateMail" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cpTitulo">
    CONFIGURACIÓN DE E-MAILS
</asp:Content>
