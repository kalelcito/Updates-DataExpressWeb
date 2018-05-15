<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="crearFactura.aspx.cs" Inherits="DataExpressWeb.crearFactura" %>

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
        .style3
        {
            text-align: center;
            font-size: large;
        }
        .style4
        {
            font-size: large;
        }
        .style5
        {
            text-align: center;
            font-weight: bold;
            font-size: small;
        }
        .style10
        {
            font-size: small;
        }

        .style11
        {
            font-size: small;
            text-decoration: underline;
        }

        .style13
        {
            width: 100%;
        }
        .style15
        {
            height: 15px;
            width: 182px;
        }
        .style16
        {
            width: 182px;
        }
        .style17
        {
            height: 15px;
            width: 146px;
        }
        .style18
        {
            width: 146px;
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
        .style23
        {
            text-align: left;
        }
        .style24
        {
        }
        .modalPopup
        {
            background-color: #445512;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
        }
        .modalBackground
        {
            background-color: #DEE2EB;

        }

        .style32
        {
            text-align: left;
        }
        .style35
        {
            width: 926px;
        }
        .style36
        {
            width: 195px;
        }
        .style41
        {
            width: 127px;
        }
        .style43
        {
            width: 3px;
        }
        .style44
        {
            width: 126px;
        }
        .style46
        {
            width: 153px;
        }
        .style48
        {
            width: 18px;
        }
        .style49
        {
            width: 148px;
        }
        .style50
        {
            text-align: right;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager2" runat="server" />

    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="1"
        BackColor="#F7F6F3" BorderColor="#CCCCCC"
        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"
        Height="183px" Width="95%" Style="margin-bottom: 0px; font-weight: 700;" OnActiveStepChanged="Wizard1_ActiveStepChanged"
        OnNextButtonClick="StepNextButton_Click"
        onfinishbuttonclick="FinishButton_Click" Font-Bold="False">
        <FinishNavigationTemplate>
            <asp:Button ID="FinishPreviousButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="MovePrevious"
                Font-Names="Verdana" Font-Size="Small" ForeColor="#284775" Text="Anterior" />
            &nbsp;&nbsp;
            <asp:Button ID="FinishButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                BorderStyle="Solid" BorderWidth="1px" CommandName="MoveComplete" Font-Names="Verdana"
                Font-Size="Small" ForeColor="#284775" Text="Crear Documento" OnClick="FinishButton_Click"  />
        </FinishNavigationTemplate>
        <HeaderStyle BackColor="#444345" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
            ForeColor="White" HorizontalAlign="Left" />
        <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
            BorderWidth="1px" Font-Names="Verdana" Font-Size="Small" ForeColor="#284775" />
        <NavigationStyle HorizontalAlign="Center" />
        <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
        <SideBarStyle BackColor="#004B94" BorderWidth="0px" Font-Size="Small" VerticalAlign="Top"
            Width="20%" BorderColor="#3333CC" />
        <StepNavigationTemplate>
            <asp:Button ID="StepPreviousButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="MovePrevious"
                Font-Names="Verdana" Font-Size="Small" ForeColor="#284775" Text="Anterior" />
            <asp:Button ID="StepNextButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                BorderStyle="Solid" BorderWidth="1px" CommandName="MoveNext" Font-Names="Verdana"
                Font-Size="Small" ForeColor="#284775" Text="Siguiente" ValidationGroup="Form" CausesValidation="true" />
        </StepNavigationTemplate>
        <StepStyle BorderWidth="0px" ForeColor="#666666" />
        <WizardSteps>
            <%--<asp:WizardStep runat="server" Title="Información Adicional" ID="infoAdicional">
                <table style="width: 100%;">
                    <tr>
                        <td class="style6" colspan="2">
                            <strong>Agregar Campo Adicional.</strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="style25">
                            NOMBRE:
                        </td>
                        <td>
                            <b>VALOR:</b>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="style8">
                            <asp:TextBox ID="tbNombreCA" runat="server" Height="19px" Width="334px" ValidationGroup="InfoAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ValidationGroup="InfoAdic"
                                ControlToValidate="tbNombreCA" ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbValorCA" runat="server" Height="19px" Width="334px" ValidationGroup="InfoAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="InfoAdic"
                                ControlToValidate="tbValorCA" ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="bAgregarCA" runat="server" Text="Agregar Campo" Width="125px" ValidationGroup="InfoAdic"
                    OnClick="bAgregarCA_Click" />
                <br />
                <br />
                <asp:GridView ID="gvInfoAdic" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataInfoAdic"
                    ForeColor="Black" DataKeyNames="idInfoAdicionalTemp" GridLines="Vertical" Width="90%">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="nombre" HeaderText="NOMBRE" SortExpression="nombre" />
                        <asp:BoundField DataField="valor" HeaderText="VALOR" SortExpression="valor" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#DEE2EB" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2"></SortedAscendingCellStyle>
                    <SortedAscendingHeaderStyle BackColor="#848384"></SortedAscendingHeaderStyle>
                    <SortedDescendingCellStyle BackColor="#EAEAD3"></SortedDescendingCellStyle>
                    <SortedDescendingHeaderStyle BackColor="#575357"></SortedDescendingHeaderStyle>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataInfoAdic" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    DeleteCommand="DELETE FROM InfoAdicionalTemp WHERE (idInfoAdicionalTemp = @idInfoAdicionalTemp)"
                    SelectCommand="SELECT idInfoAdicionalTemp,nombre, valor, id_Empleado FROM InfoAdicionalTemp WHERE (id_Empleado = @id_Empleado)">
                    <DeleteParameters>
                        <asp:Parameter Name="idInfoAdicionalTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="id_Empleado" SessionField="idUser" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:WizardStep>--%>
            <asp:WizardStep runat="server" Title="Tributaria" ID="infoTributaria">
                <div class="style3">
                    Información Tributaria<br />
                </div>
                <br />
                <table class="style13">
                    <tr>
                        <td class="style15">
                            AMBIENTE:<br />
                            <asp:DropDownList ID="ddlAmbiente" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataAmbiente"
                                DataTextField="descripcion" DataValueField="codigo">
                            </asp:DropDownList>
                        </td>
                        <td class="style17">
                            EMISIÓN:<br />
                            <asp:DropDownList ID="ddlEmision" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataEmision"
                                DataTextField="descripcion" DataValueField="codigo">
                            </asp:DropDownList>
                        </td>
                        <td>
                            COMPROBANTE:<br />
                            <asp:DropDownList ID="ddlComprobante" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SqlDataDocumento" DataTextField="descripcion" DataValueField="codigo">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style16">
                            SUCURSAL:<br />
                            <asp:DropDownList ID="ddlSucursal" runat="server" DataSourceID="SqlDataSucursal"
                                DataTextField="sucursal" DataValueField="clave">
                            </asp:DropDownList>
                        </td>
                        <td class="style18">
                            PUNTO DE EMISIÓN:<br />
                            <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataPtoEmision"
                                DataTextField="ptoEmi" DataValueField="ptoEmi">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lSecuencial" runat="server" Text="SECUENCIA:" Visible="False"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbSecuencial" runat="server" Height="19px" Width="93px" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lRuc" runat="server" Text="RUC:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRuc" runat="server" Height="19px" MaxLength="13" Width="163px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRuc"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lRazonSocial" runat="server" Text="RAZÓN SOCIAL:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocial" runat="server" Height="19px" Width="353px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbRazonSocial"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lNombreComercial" runat="server" Text="NOMBRE COMERCIAL"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbNombreComercial" runat="server" Height="19px" Width="293px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbNombreComercial"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lDirMatriz" runat="server" Text="DIRECCIÓN MATRIZ"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbDirMatriz" runat="server" Height="19px" Width="293px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbDirMatriz"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="SqlDataPtoEmision" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT ptoEmi, idEmpleado FROM Empleados WHERE (idEmpleado = @idEmpleado)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idEmpleado" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSucursal" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT clave, sucursal FROM Sucursales WHERE (idSucursal = @idSucursal)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idSucursal" SessionField="sucursalUser" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataDocumento" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WHERE (tipo = 'Comprobante') AND (codigo = '01')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataEmision" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WHERE (tipo = 'Emision')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataAmbiente" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WHERE (tipo = 'Ambiente' and codigo='2')">
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Detalles" ID="infoDetalles" EnableTheming="True">

                <%--<strong><span class="style11">IMPUESTO</span></strong><br class="style12" />--%>
                <%-- <strong><span class="style11">DETALLE ADICIONAL</span></strong><br class="style12" />
                <table class="style9">
                    <tr>
                        <td>
                            <b>NOMBRE</b>
                        </td>
                        <td>
                            <b>VALOR</b>
                        </td>
                        <td rowspan="2">
                            <asp:Button ID="bAgregarDetalle" runat="server" Text="Agregar Det. Adic." Height="25px"
                                OnClick="bAgregarDetalleAdic_Click" Width="119px" ValidationGroup="detallesAdic" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbNombreConcepto" runat="server" Height="34px" Width="254px" MaxLength="300"
                                TextMode="MultiLine" ValidationGroup="detallesAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNombreConcepto"
                                CssClass="style20" ErrorMessage="*" ValidationGroup="detallesAdic"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbValorConcepto" runat="server" Height="34px" Width="254px" MaxLength="300"
                                TextMode="MultiLine" ValidationGroup="detallesAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbValorConcepto"
                                CssClass="style20" ErrorMessage="*" ValidationGroup="detallesAdic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>--%>
                <%-- <asp:Button ID="bAgregarConcepto" runat="server" Text="Agregar Detalle" Width="118px"
                    OnClick="bAgregarConcepto_Click" ValidationGroup="Deta" />--%>
                <br />
                <span class="style11">DETALLES DE FACTURA</span><br />
                <br />

                        <table class="style35">
                            <tr>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    <asp:Label ID="lbl_cantidad" runat="server" CssClass="style24" Text="CANTIDAD:"></asp:Label>
                                </td>
                                <td class="style44">
                                    <asp:Label ID="lbl_descripcion" runat="server" CssClass="style24"
                                        Text="DESCRIPCIÓN:"></asp:Label>
                                </td>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    <asp:Label ID="lbl_precio" runat="server" CssClass="style24" Text="P.U:"></asp:Label>
                                </td>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    <asp:Label ID="lbl_descuento" runat="server" CssClass="style24"
                                        Text="DESCUENTO:"></asp:Label>
                                </td>
                                <td class="style46">
                                    <asp:Label ID="lbl_iva" runat="server" CssClass="style24" Text="IVA:"></asp:Label>
                                </td>
                                <td class="style46">
                                    &nbsp;</td>
                                <td class="style46">
                                    <asp:Label ID="lbl_Subtotal" runat="server" CssClass="style24" Text="IMPORTE:"></asp:Label>
                                </td>
                                <td class="style46" colspan="2">
                                    &nbsp;</td>
                                <td colspan="3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style44" rowspan="2"
                                    style="font-size: 9px; text-align: center; margin-left: 40px;">
                                    <asp:TextBox ID="tbCantidad" runat="server" AutoPostBack="True"
                                        CausesValidation="True" Height="19px" OnTextChanged="tbCantidad_TextChanged"
                                        Style="text-align: right" ValidationGroup="Deta" Width="80px">1</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbCantidad_FilteredTextBoxExtender"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers"
                                        TargetControlID="tbCantidad" ValidChars=",.">
                                    </asp:FilteredTextBoxExtender>
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10"
                                        runat="server" ControlToValidate="tbCantidad" CssClass="style20"
                                        ErrorMessage="*" SetFocusOnError="True"
                                        ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$" ValidationGroup="Deta"></asp:RegularExpressionValidator>
                                </td>
                                <td class="style44" rowspan="3">
                                    <asp:TextBox ID="tbCodigoA0" runat="server" Height="51px"
                                        ValidationGroup="Deta" Width="356px"></asp:TextBox>
                                </td>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    <asp:TextBox ID="tbPU" runat="server" AutoPostBack="True"
                                        CausesValidation="True" Height="19px" OnTextChanged="tbPU_TextChanged"
                                        Style="text-align: right" ValidationGroup="Deta" Width="80px">0</asp:TextBox>
                                </td>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    <asp:TextBox ID="tbDescuento" runat="server" AutoPostBack="True"
                                        CausesValidation="True" Height="19px" OnTextChanged="tbDescuento_TextChanged"
                                        Style="text-align: right; margin-bottom: 0px" ValidationGroup="Deta"
                                        Width="80px">0</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="tbDescuento_FilteredTextBoxExtender"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers"
                                        TargetControlID="tbDescuento" ValidChars=",.">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="style46">
                                    <span class="style11"><strong>
                                    <asp:DropDownList ID="ddlTasaIVA" runat="server" AutoPostBack="True"
                                        Height="25px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                                        Width="60px">
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>No Sujeto</asp:ListItem>
                                    </asp:DropDownList>
                                    </strong></span>
                                </td>
                                <td class="style46">
                                    <span class="style11"><strong>
                                    <asp:TextBox ID="tbValor" runat="server" Height="22px" MaxLength="14"
                                        ReadOnly="True" ValidationGroup="Imp" Width="80px"></asp:TextBox>
                                    </strong></span>
                                </td>
                                <td class="style46">
                                    <asp:TextBox ID="tbImporteConcepto" runat="server" CausesValidation="True"
                                        Height="19px" ReadOnly="True" Style="text-align: right" ValidationGroup="Deta"
                                        Width="80px">0</asp:TextBox>
                                </td>
                                <td class="style46" colspan="2" rowspan="3">
                                    &nbsp;</td>
                                <td colspan="2" rowspan="3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="tbPU" CssClass="style20" ErrorMessage="*"
                                        SetFocusOnError="True" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                                        ValidationGroup="Deta"></asp:RegularExpressionValidator>
                                </td>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                        ControlToValidate="tbDescuento" CssClass="style20" ErrorMessage="*"
                                        SetFocusOnError="True" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                                        ValidationGroup="Deta"></asp:RegularExpressionValidator>
                                </td>
                                <td class="style46" colspan="2">
                                    &nbsp;</td>
                                <td class="style46">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    &nbsp;</td>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    &nbsp;</td>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    &nbsp;</td>
                                <td class="style46" colspan="2">
                                    &nbsp;</td>
                                <td class="style46">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="font-size: 9px; text-align: center;" class="style44">
                                    &nbsp;</td>
                                <td class="style44">
                                    &nbsp;</td>
                                <td class="style44" rowspan="2" style="font-size: 9px; text-align: center;">
                                    <asp:FilteredTextBoxExtender ID="tbPU_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbPU"
                                        ValidChars=",.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="tbPU" Display="None" ErrorMessage="Debe ser mayor a 0."
                                        ForeColor="Red" InitialValue="0" SetFocusOnError="True" ValidationGroup="Deta"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                                        Enabled="True" TargetControlID="RequiredFieldValidator2">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td style="font-size: 9px; text-align: center;" class="style44" rowspan="3">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                        ControlToValidate="tbDescuento" Display="None"
                                        ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True"
                                        ValidationGroup="Deta"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server"
                                        Enabled="True" TargetControlID="RequiredFieldValidator6">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td class="style36" colspan="3" rowspan="2">
                                    <br />
                                </td>
                                <td colspan="2" class="style46" rowspan="2">
                                    <span class="style11"><strong>
                                    <br />
                                    </strong></span></td>
                            </tr>
                            <tr>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server"
                                        ControlToValidate="tbCantidad" Display="None"
                                        ErrorMessage="Debe ser mayor a 0." ForeColor="Red" InitialValue="0"
                                        SetFocusOnError="True" ValidationGroup="Deta"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator23_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator23">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td class="style44" rowspan="2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server"
                                        ControlToValidate="tbConcepto" ErrorMessage="Campo requerido" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                        ControlToValidate="ddlCodigoA0" CssClass="style20" ErrorMessage="*"
                                        ValidationGroup="Deta"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 9px; text-align: center;" class="style44">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="tbCantidad" Display="None"
                                        ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True"
                                        ValidationGroup="Deta"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="CustomValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td class="style44" style="font-size: 9px; text-align: center;">
                                    &nbsp;</td>
                                <td colspan="5" class="style46">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="font-size: 9px; text-align: center;" class="style44">
                                    &nbsp;</td>
                                <td class="style44">
                                    &nbsp;</td>
                                <td style="font-size: 9px; text-align: center;" class="style48" colspan="2"
                                    align="right">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                        ControlToValidate="tbPU" Display="None" ErrorMessage="Debe ingresar valor."
                                        ForeColor="Red" SetFocusOnError="True" ValidationGroup="Deta"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server"
                                        Enabled="True" TargetControlID="RequiredFieldValidator3">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td class="style44" colspan="2">
                                    &nbsp;</td>
                                <td align="right" style="font-size: 9px; text-align: center;" class="style49"
                                    colspan="2">
                                    &nbsp;</td>
                                <td colspan="2">
                                    &nbsp;</td>
                                <td align="right" style="font-size: 9px; text-align: center; " class="style41">
                                    &nbsp;</td>
                                <td valign="top" class="style41">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="12">
                                    <asp:CheckBox ID="chbServicio" runat="server" Text="10%Servicio" />
                                    <br />
                                    <br />
                                    <asp:Button ID="bAgregarImpuesto" runat="server" Height="30px"
                                        OnClick="bAgregarImpuesto_Click" Text="Agregar" ValidationGroup="Deta" CssClass="btGeneralG"
                                        Width="132px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="12">
                                    <asp:Label ID="lMsjImpuestos" runat="server" Style="color: #FF0000"></asp:Label>
                                    <asp:Label ID="lMsjDetalles" runat="server" Style="color: #FF0000"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="12">
                                    <asp:GridView ID="gvDetalles" runat="server" AutoGenerateColumns="False"
                                        BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                                        CellPadding="4" DataKeyNames="codigoPrincipal" DataSourceID="SqlDataDetalles"
                                        ForeColor="Black" GridLines="Vertical" Width="79%">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD"
                                                SortExpression="cantidad" />
                                            <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN"
                                                SortExpression="descripcion" />
                                            <asp:BoundField DataField="precioUnitario" HeaderText="PRECIO UNITARIO"
                                                SortExpression="precioUnitario" />
                                            <asp:BoundField DataField="descuento" HeaderText="DESCUENTO"
                                                SortExpression="descuento" />
                                            <asp:BoundField DataField="precioTotalSinImpuestos" HeaderText="SUBTOTAL"
                                                SortExpression="precioTotalSinImpuestos" />
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False"
                                                        CommandName="Delete"
                                                        OnClientClick="return confirm('¿Desea eliminar el registro?');" Text="Eliminar"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <HeaderStyle BackColor="#004B94" BorderColor="#444345" Font-Bold="True"
                                            ForeColor="White" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <RowStyle BackColor="White" />
                                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                        <SortedAscendingHeaderStyle BackColor="#848384" />
                                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                        <SortedDescendingHeaderStyle BackColor="#575357" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="12">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="12">
                                    <asp:SqlDataSource ID="SqlDataSourceConceptos" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                                        SelectCommand="SELECT [item] FROM [Conceptos_C]"></asp:SqlDataSource>
                                </td>
                                <td align="center" class="style43">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlTipoImpuesto" runat="server" AutoPostBack="True" DataSourceID="SqlDataCatImpuestos"
                                        DataTextField="descripcion" DataValueField="codigo" OnSelectedIndexChanged="ddlTipoImpuesto_SelectedIndexChanged"
                                        AppendDataBoundItems="True" Visible="false">
                                        <asp:ListItem Selected="True" Text="IVA" Value="2">IVA</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="style44">
                                    <asp:TextBox ID="tbCodigoID" runat="server" Height="19px" Width="20px" ValidationGroup="Imp"
                                        MaxLength="1" Visible="False" CssClass="txt_gr2"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[0-9]{1}?$"
                                        ControlToValidate="tbCodigoID" CssClass="style20" ErrorMessage="*"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbCodigoID"
                                        ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp"></asp:RequiredFieldValidator>
                                </td>
                                <td class="style48" colspan="2">
                                    <asp:TextBox ID="tbCodigoIDP" runat="server" Height="19px" Width="60px" ValidationGroup="Imp"
                                        MaxLength="4" Visible="False" CssClass="txt_gr2"></asp:TextBox>
                                    <span class="style11"><strong>
                                    <asp:TextBox ID="tbCodigoP" runat="server" CssClass="txt_gr2" Height="19px"
                                        ValidationGroup="Deta" Visible="False" Width="90px"></asp:TextBox>
                                    <asp:DropDownList ID="ddlCodigoA0" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="True" DataSourceID="SqlDataSourceConceptos" DataTextField="item"
                                        DataValueField="item" OnSelectedIndexChanged="ddlCodigoA0_SelectedIndexChanged"
                                        Visible="False">
                                        <asp:ListItem Value="0">Selecciona un concepto</asp:ListItem>
                                    </asp:DropDownList>
                                    </strong></span>
                                </td>
                                <td class="style44" colspan="2">
                                    <asp:TextBox ID="tbConcepto" runat="server" Height="16px" MaxLength="300"
                                        TextMode="MultiLine" Visible="False" Width="71px"></asp:TextBox>
                                    <asp:TextBox ID="tbTarifa" runat="server" Height="19px" Width="60px" ValidationGroup="Imp"
                                        AutoPostBack="True" OnTextChanged="tbTarifa_TextChanged" MaxLength="4" Visible="False"
                                        CssClass="txt_gr2"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="tbTarifa"
                                        ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp" Visible="False"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbBaseImponible" runat="server" Height="30px" Width="70px" ValidationGroup="Imp"
                                        AutoPostBack="True" ReadOnly="True" MaxLength="14" CssClass="txt_gr2" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbPersonas" runat="server" style="text-align: right"
                                        Text="No. Personas:" Visible="False"></asp:Label>
                                    <asp:Label ID="Label7" runat="server" Text="Servicio 10%:" Visible="False"></asp:Label>
                                    <asp:TextBox ID="tbServicio10" runat="server"
                                        OnTextChanged="tbServicio10_TextChanged" Visible="False">0.00</asp:TextBox>
                                </td>
                                <td class="style44">
                                    <asp:TextBox ID="tbServicio" runat="server" Visible="False" Width="80px">0.00</asp:TextBox>
                                </td>
                                <td class="style48" colspan="2">
                                    &nbsp;</td>
                                <td class="style44" colspan="2">
                                    &nbsp;</td>
                                <td colspan="2">
                                    &nbsp;</td>
                            </tr>
                        </table>

                <br />
                <br />
                <br />
                <asp:TextBox ID="tbnoPersonas" runat="server" Visible="False" Width="43px">1</asp:TextBox>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:GridView ID="gvImpuestosDetalles" runat="server" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                    CellPadding="4" DataSourceID="SqlDataImpuestosConceptos" ForeColor="Black" DataKeyNames="idImpuestosDetallesTemp"
                    GridLines="Vertical" Height="18px" Width="90%" Visible="false">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="CODIGO" SortExpression="codigo">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("codigo") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("codigo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CODIGO %" SortExpression="codigoPorcentaje">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("codigoPorcentaje") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("codigoPorcentaje") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TARIFA" SortExpression="tarifa">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("tarifa") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("tarifa") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BASE IMPONIBLE" SortExpression="baseImponible">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("baseImponible") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("baseImponible") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VALOR" SortExpression="valor">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("valor") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("valor") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar."></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#BACA50" Font-Bold="True" ForeColor="White"
                        BorderColor="#444345" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="White" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2"></SortedAscendingCellStyle>
                    <SortedAscendingHeaderStyle BackColor="#848384"></SortedAscendingHeaderStyle>
                    <SortedDescendingCellStyle BackColor="#EAEAD3"></SortedDescendingCellStyle>
                    <SortedDescendingHeaderStyle BackColor="#575357"></SortedDescendingHeaderStyle>
                </asp:GridView>
                <asp:GridView ID="gvDetAdic" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataDetAdicionalesDetalles"
                    ForeColor="Black" GridLines="Vertical" Width="80%" DataKeyNames="idDetallesAdicionalesTemp">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="NOMBRE" SortExpression="nombre">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("nombre") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VALOR" SortExpression="valor">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("valor") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("valor") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White"
                        BorderColor="Black" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#DFE963" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2"></SortedAscendingCellStyle>
                    <SortedAscendingHeaderStyle BackColor="#848384"></SortedAscendingHeaderStyle>
                    <SortedDescendingCellStyle BackColor="#EAEAD3"></SortedDescendingCellStyle>
                    <SortedDescendingHeaderStyle BackColor="#575357"></SortedDescendingHeaderStyle>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataDetalles" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    DeleteCommand="PA_eliminar_DetallesTemp" SelectCommand="SELECT cantidad, codigoPrincipal, codigoAuxiliar, descripcion, precioUnitario, descuento, precioTotalSinImpuestos, id_Empleado FROM DetallesTemp WHERE (id_Empleado = @idUser)"
                    DeleteCommandType="StoredProcedure">
                    <DeleteParameters>
                        <asp:Parameter Name="codigoPrincipal" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataImpuestosConceptos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT idImpuestosDetallesTemp,codigo, codigoPorcentaje, tarifa, baseImponible, valor, id_Empleado, codigoTemp FROM ImpuestosDetallesTemp WHERE (id_Empleado = @idUser) AND (codigoTemp = @codigoTemp)"
                    DeleteCommand="DELETE FROM ImpuestosDetallesTemp WHERE (idImpuestosDetallesTemp = @idImpuestosDetallesTemp)">
                    <DeleteParameters>
                        <asp:Parameter Name="idImpuestosDetallesTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                        <asp:ControlParameter ControlID="tbCodigoP" Name="codigoTemp" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataDetAdicionalesDetalles" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT idDetallesAdicionalesTemp,nombre, valor, codigoTemp, id_Empleado FROM DetallesAdicionalesTemp WHERE (codigoTemp = @codigoTemp) AND (id_Empleado = @idUser)"
                    DeleteCommand="DELETE FROM DetallesAdicionalesTemp WHERE (idDetallesAdicionalesTemp = @idDetallesAdicionalesTemp)">
                    <DeleteParameters>
                        <asp:Parameter Name="idDetallesAdicionalesTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="tbCodigoP" Name="codigoTemp" PropertyName="Text" />
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataCatImpuestos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT [descripcion], [codigo] FROM [CatImpuestos_C] WHERE ([tipo] = @tipo)">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Impuesto" Name="tipo" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Documento" ID="infoDocumentos">
                <br />
                <div class="style5">
                    <span class="style4">Información de la Factura<br />
                    </span>
                </div>
                <table class="style13">
                    <tr>
                        <td class="style50">
                            &nbsp;</td>
                        <td class="style50">
                            &nbsp;</td>
                        <td class="style50">
                            <asp:Label ID="Label10" runat="server" Text="SECUENCIAL:"></asp:Label>
                            <asp:TextBox ID="tbFolio" runat="server" ReadOnly="True" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                            <asp:Label ID="lFechaEmision" runat="server" Text="FECHA DE EMISIÓN:"></asp:Label>
                            <asp:TextBox ID="tbFechaEmision" runat="server" Height="19px" ReadOnly="True"
                                Width="137px"></asp:TextBox>
                            <br />
                        </td>
                        <td>
                            <br />
                            <br />
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lMetodopago" runat="server" Text="MÉTODO DE PAGO:"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlMetodoPago" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="Selecciona una opción">Selecciona una opción</asp:ListItem>
                                <asp:ListItem Selected="True">Efectivo</asp:ListItem>
                                <asp:ListItem Value="Tarjeta crédito/Débito">Tarjeta crédito/Debito</asp:ListItem>
                                <asp:ListItem>CXC</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Label ID="Label8" runat="server" Text="Monto:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbMonto" runat="server">0.00</asp:TextBox>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                            <asp:Label ID="ldirEstablecimiento" runat="server" Text="DIRECCIÓN SUCURSAL EMISORA:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbDirEstablecimiento" runat="server" Height="47px" Width="293px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <%--<asp:Label ID="lMoneda" runat="server" Text="MONEDA:"></asp:Label>--%>
                            <asp:Label ID="lbl_termino" runat="server" Text="TÉRMINO:" Visible="False"></asp:Label>
                            <asp:TextBox ID="txt_termino" runat="server" Height="19px" MaxLength="15"
                                Width="16px" Visible="False"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Label ID="lbl_proforma" runat="server" Text="PROFORMA:" Visible="False"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txt_proforma" runat="server" Height="19px" MaxLength="15"
                                Width="70px" Visible="False"></asp:TextBox>&nbsp;&nbsp; &nbsp;&nbsp;
                            <asp:TextBox ID="txt_pedido" runat="server" Height="19px" MaxLength="15"
                                Width="53px" Visible="False"></asp:TextBox>
                            <asp:Label ID="lbl_pedido" runat="server" Text="PEDIDO:" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                            <asp:CheckBox ID="cbContribuyenteEspecial" runat="server" Text="CONTRIBUYENTE ESPECIAL"
                                Visible="false" />
                            &nbsp;<asp:TextBox ID="tbContribuyenteEspecial" runat="server" MaxLength="4" Width="51px"
                                Visible="False"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbContribuyenteEspecial_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbContribuyenteEspecial">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <asp:CheckBox ID="cbObligado" runat="server" Text="OBLIGADO A CONTABILIDAD" Checked="True"
                                Visible="false" />
                        </td>
                        <td rowspan="3" class="style23" colspan="2">
                            <asp:Panel ID="Panel1" runat="server" Height="252px">
                                <table style="width:100%;">
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lSubtotal12" runat="server" Text="SUBTOTAL 12%:" Visible="False"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbSubtotal12" runat="server" Height="19px" ReadOnly="True"
                                                Style="text-align: left" Visible="False" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="Label9" runat="server" Text="SUBTOTAL:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbSubT" runat="server" Height="19px" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lSubtotal0" runat="server" Text="SUBTOTAL 0%:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbSubtotal0" runat="server" Height="19px" ReadOnly="True"
                                                Style="text-align: left" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lSubtotalNoSujeto" runat="server"
                                                Text="SUBTOTAL No sujeto de IVA:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbSubtotalNoSujeto" runat="server" Height="19px"
                                                ReadOnly="True" Style="text-align: left" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lTotalDescuento" runat="server" Text="DESCUENTO:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbTotalDescuento" runat="server" Height="19px" ReadOnly="True"
                                                Style="text-align: left" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lTotalSinImpuestos" runat="server"
                                                Text="SUBTOTAL SIN IMPUESTOS:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbTotalSinImpuestos" runat="server" Height="19px"
                                                ReadOnly="True" Style="text-align: left" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lIVA12" runat="server" Text="IVA 12%:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbIVA12" runat="server" Height="19px" ReadOnly="True"
                                                Style="text-align: left" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="LCargoServicio" runat="server" Text="SERVICIO 10%:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbPropinas" runat="server" Height="19px"
                                                style="text-align: left" Width="90px" AutoPostBack="True"
                                                OnTextChanged="tbPropinas_TextChanged">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lTasaHospedaje" runat="server" Text="TASA SERVICIO TURISTICO:"
                                                Visible="False"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbTasaHospedaje" runat="server" Height="19px"
                                                style="text-align: left" Width="90px" Visible="False">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lImporteTotal" runat="server" Text="IMPORTE TOTAL:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbImporteTotal" runat="server" Height="19px" ReadOnly="True"
                                                Style="text-align: left" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style32">
                                            <asp:Label ID="lImporteaPagar" runat="server" Text="IMPORTE A PAGAR:"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:TextBox ID="tbImporteaPagar" runat="server" Height="19px" ReadOnly="True"
                                                Style="text-align: left" Width="90px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lICE" runat="server" Text="ICE:" Visible="False"></asp:Label>
                                            <asp:TextBox ID="tbICE" runat="server" Height="19px" ReadOnly="True"
                                                Style="text-align: right" Visible="False" Width="54px">0</asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator14"
                                                runat="server" ControlToValidate="tbPropinas" CssClass="style20"
                                                ErrorMessage="*" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                                                ValidationGroup="Deta"></asp:RegularExpressionValidator>
                                            <asp:Label ID="lPropinas" runat="server" Text="PROPINAS:" Visible="False"></asp:Label>
                                            <asp:TextBox ID="tbCargoxServicio" runat="server" Height="19px"
                                                Style="text-align: left" Width="90px" Visible="False">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                            <asp:Label ID="lidentificacionComprador" runat="server" Text="INGRESA RUC/CED:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbIdentificacionComprador" runat="server" AutoPostBack="True"
                                Height="19px" MaxLength="13"
                                OnTextChanged="tbIdentificacionComprador_TextChanged"
                                ValidationGroup="Receptor" Width="183px"></asp:TextBox>

                            <asp:AutoCompleteExtender ID="tbIC_AutoCompleteExtender" runat="server"
                                CompletionInterval="10" CompletionListCssClass="CompletionListCssClass"
                                CompletionSetCount="12" DelimiterCharacters="" Enabled="True"
                                ServiceMethod="getRuc" ServicePath="../autoRuc.asmx"
                                ShowOnlyCurrentWordInCompletionListItem="True"
                                TargetControlID="tbIdentificacionComprador" UseContextKey="True">
                            </asp:AutoCompleteExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13"
                                runat="server" ControlToValidate="tbIdentificacionComprador" CssClass="style20"
                                ErrorMessage="RUC Invalido" ValidationExpression="^[0-9a-zA-Z]{0,13}?$"
                                ValidationGroup="Receptor"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                            <asp:Label ID="lTipoIdentificacionComprador" runat="server" CssClass="style5"
                                Text="IDENTIFICACIÓN DEL COMPRADOR"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlTipoIdentificacion" runat="server"
                                AppendDataBoundItems="True" DataSourceID="SqlDataTipoIdentificacion"
                                DataTextField="descripcion" DataValueField="codigo">
                                <asp:ListItem>Selecciona el Tipo</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <br />
                            <br />
                            &nbsp;&nbsp;<br />
                            <br />
                            <asp:Label ID="lRazonSocialComprador" runat="server"
                                Text="RAZÓN SOCIAL DEL COMPRADOR:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocialComprador" runat="server" Height="46px"
                                TextMode="MultiLine" Width="291px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                ControlToValidate="tbRazonSocialComprador" ErrorMessage="*"
                                Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                            <br />
                            <br />
                            <asp:Label ID="lbl_dir_cli" runat="server" Text="DOMICILIO:"></asp:Label>
                            <br />
                            <asp:TextBox ID="txt_dir_cli" runat="server" Height="46px" TextMode="MultiLine"
                                Width="291px"></asp:TextBox>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style13" colspan="3">
                            E-Mail:
                            <asp:TextBox ID="tbEmail" runat="server" Height="19px" Width="413px"></asp:TextBox>
                            &nbsp;&nbsp;
                            <br />
                            <asp:Label ID="lbl_fono" runat="server" Text="TELÉFONO:"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txt_fono" runat="server" Height="19px" MaxLength="15"
                                Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txt_fono_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Custom, Numbers"
                                TargetControlID="txt_fono" ValidChars=",-">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <br />
                            Observaciones:<br />
                            <asp:TextBox ID="tbObservaciones" runat="server" Height="62px"
                                TextMode="MultiLine" Width="399px" MaxLength="300"></asp:TextBox>
                            <br />
                            <span class="style10">Formato:
                            <a href="mailto:email1@email.com,email2@email.com">
                            email1@email.com,email2@email.com</a> </span>
                            <br />
                            <span class="style10">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator15"
                                runat="server" ControlToValidate="tbEmail"
                                ErrorMessage="El formato de E-Mail no es válido" ForeColor="Red"
                                ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$"
                                ValidationGroup="email"></asp:RegularExpressionValidator>
                            </span>
                            <br />
                            <asp:Label ID="lMsjDocumento" runat="server" Style="color: #FF0000"></asp:Label>
                            <asp:TextBox ID="tbMoneda" runat="server" Height="19px" MaxLength="15"
                                Visible="False" Width="70px">DOLAR</asp:TextBox>
                            <asp:TextBox ID="tbFechaV" runat="server" Visible="False"></asp:TextBox>
                            <asp:CalendarExtender ID="tbFechaVencimiento_CalendarExtender" runat="server"
                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="tbFechaV"
                                TodaysDateFormat="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Label ID="Label6" runat="server" Text="FECHA PAGO:" Visible="False"></asp:Label>
                            <asp:Label ID="lGuiaRemision" runat="server" Text="GUÍA DE REMISIÓN:"
                                Visible="False"></asp:Label>
                            <asp:TextBox ID="tbGuiaRemision" runat="server" Height="19px" Visible="False"
                                Width="185px"></asp:TextBox>
                        </td>
                    </tr>
                    </caption>
                    </tr>
                </table>
                <br />
                <asp:SqlDataSource ID="SqlDataTipoIdentificacion" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WHERE (tipo = 'Identificacion')">
                </asp:SqlDataSource>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
