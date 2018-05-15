<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Cambiar.aspx.cs" Inherits="DataExpressWeb.CambiarDatos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #Text1
        {
            width: 189px;
            margin-top: 0px;
        }
        #Text2
        {
            width: 474px;
        }
        #Text3
        {
            width: 164px;
        }
        #Text5
        {
            width: 139px;
        }
        #Text6
        {
            width: 138px;
        }
        .CompletionListCssClass
        {
            font-size: 16px;
            color: #000;
            padding: 0px 0px;
            border: 1px solid #999;
            background: #fff;
            width: 300px;
            float: left;
            z-index: 1;
            position: absolute;
            margin-left: 0px;
            margin-top: -3px;
        }
        .style1
        {
            height: 6px;
        }
        .style2
        {
            height: 34px;
        }
        .style4
        {
            height: 26px;
        }
        .style5
        {
            font-size: small;
        }
        .style6
        {
            height: 253px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" BackColor="#F7F6F3" BorderColor="#CCCCCC"
        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"
        Height="331px" Width="97%" Style="margin-bottom: 0px"
         StartNextButtonText=" "
        FinishCompleteButtonText=" " FinishPreviousButtonText=" "
        StepNextButtonText=" " StepPreviousButtonText=" "
        >
        <HeaderTemplate>

        </HeaderTemplate>
        <NavigationButtonStyle BackColor="#F7F6F3" BorderColor="#F7F6F3" BorderStyle="Solid"
            BorderWidth="1px" Font-Names="Verdana" Font-Size="Small" ForeColor="#173E57" />
        <NavigationStyle HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
            ForeColor="White" HorizontalAlign="Left" />

        <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
        <SideBarStyle BackColor="#6DBE1E" BorderWidth="0px" Font-Size="Small" VerticalAlign="Top"
            Width="20%" BorderColor="#173E57" />
        <WizardSteps>
            <asp:WizardStep runat="server" Title="Cambiar información" ID="cargaNum">

                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
                <asp:Label ID="lError" runat="server" Text=" "></asp:Label>
                <br />
                <br />
            <table class="style15">
        <tr style="background-color:#53ab46;">
            <td colspan="2" style="text-align: center" class="style1">
                <span class="style19">Consultar</span></td>
        </tr>
        <tr valign="top">
            <td>
                <strong style="font-size: small">Introduzca No Orden:</strong>
                <asp:TextBox ID="tbnoOrden" runat="server" MaxLength="13" Width="160px"></asp:TextBox>
                <asp:Button ID="consultar" runat="server" CssClass="CambiarDatos"
                    OnClick="consultar_Click" Style="height: 26px" Text="Consultar Orden"
                    ValidationGroup="email" Width="106px" BackColor="#009933"
                    ForeColor="White" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr valign ="top">
            <td colspan="2">
                <strong style="font-size: small">Nombre:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tbNombre" runat="server" Width="593px" CssClass="style9"
                    ReadOnly="True"></asp:TextBox>
                </td>
        </tr>
        <tr valign="top">
            <td colspan="2" class="style4">

            </td>
            <td class="style4">
                    </td>
        </tr>
                <tr valign="top">
                    <td class="style21" colspan="2">
                        <asp:Label ID="Label17" runat="server" Text="RUC:"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="tbRUC" runat="server" Height="20px" Width="230px"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                        <asp:Label ID="ECID" runat="server" Text="ECID:"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="tbID" runat="server" Width="221px"></asp:TextBox>
                    </td>
                    <td class="style21">
                    </td>
                </tr>
        <tr valign="top">
            <td colspan="2" class="style2">
                <asp:Label ID="ldireccion" runat="server" Text="Dirección"></asp:Label>
                <asp:TextBox ID="tbdireccion" runat="server" Width="592px"></asp:TextBox>
            </td>
        </tr>
                <tr valign="top">
                    <td colspan="2" class="style6">
                        <strong><span class="style5">E-mails:&nbsp; </span></strong>
                        <asp:TextBox ID="tbEmail" runat="server"
                            ValidationGroup="email" Width="594px"></asp:TextBox>
                        <br><span class="style10">Formato: <span class="style17"><em>
                        email_oracle@dataexpressintmx.com,prueba@herbalife.com.ec</em></span><span class="style16">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </span>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="tbEmail" ErrorMessage="Coloca un E-Mail" ForeColor="Red"
                            ValidationGroup="email"></asp:RequiredFieldValidator>--%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                            ControlToValidate="tbEmail" ErrorMessage="El formato de E-Mail no es válido"
                            ForeColor="Red"
                            ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$"
                            ValidationGroup="email"></asp:RegularExpressionValidator>
                        </span>
                        <br>
                        <br>
                        <br>
                        <br>
                        <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="bMail" runat="server" BackColor="#009933"
                            CssClass="CambiarDatos" ForeColor="White" OnClick="Actualizar_Click"
                            Style="height: 26px" Text="Cambiar datos" ValidationGroup="email"
                            Width="106px" />
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        <br></br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                        </br>
                    </td>
                </tr>
        <tr>
            <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
    </table>
            </asp:WizardStep>

        </WizardSteps>
    </asp:Wizard>
    <br />
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="MenuIzqTitulo">
    Cambiar info. de orden
</asp:Content>