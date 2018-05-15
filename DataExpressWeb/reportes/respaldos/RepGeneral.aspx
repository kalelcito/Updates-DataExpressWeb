<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RepGeneral.aspx.cs" Inherits="DataExpressWeb.RepGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style5
        {
            width: 274px;
        }
        .style6
        {
            width: 274px;
            font-weight: bold;
            text-align: center;
        }
        .style7
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
                  <center>  <asp:DropDownList ID="ddlReporte" runat="server"
                          style="text-align: center" AutoPostBack="True"
                          OnSelectedIndexChanged="ddlReporte_SelectedIndexChanged">
                        <asp:ListItem Value="General">General</asp:ListItem>
                        <asp:ListItem Value="Email">E-Mail</asp:ListItem>
                        <asp:ListItem Value="NA">No Autorizadas</asp:ListItem>
                        <asp:ListItem Value="Retenciones">Comprobante de Retención</asp:ListItem>
                        <asp:ListItem Value="0" Selected="True">Seleccionar Reporte</asp:ListItem>
                    </asp:DropDownList></center>
            <br />
    <table align="center">
        <tr>
            <td class="style6">
                <b style="text-align: center">Fecha Inicial:</b></td>
            <td class="style7">
                <b style="text-align: center">Fecha Final:</b></td>
        </tr>
        <tr>
            <td class="style5">
                <asp:Calendar ID="cFechaInicial" runat="server" BackColor="White"
                    BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt"
                    ForeColor="Black" Height="180px" Width="200px"
                    style="text-align: left" CellPadding="4" DayNameFormat="Shortest">
                    <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="White" />
                    <NextPrevStyle
                        VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#004B94" BorderColor="Black"
                        Font-Bold="True" ForeColor="White" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="White" />
                </asp:Calendar>
            </td>
            <td>
                <asp:Calendar ID="cFechaFinal" runat="server" BackColor="White"
                    BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt"
                    ForeColor="Black" Height="180px" Width="200px" CellPadding="4"
                    DayNameFormat="Shortest" style="margin-bottom: 1px">
                    <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="White" />
                    <NextPrevStyle
                        VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#004B94" BorderColor="Black"
                        Font-Bold="True" ForeColor="White" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="White" />
                </asp:Calendar>
            </td>
        </tr>
        <tr align="center">
            <td style="text-align: right">
                    &nbsp;</td>
            <td style="text-align: left">
                    &nbsp;</td>
        </tr>
        <tr align="center">
            <td style="text-align: right">
                    <asp:Label ID="Label3" runat="server" Text="Sucursal:"></asp:Label>
            </td>
            <td style="text-align: left">
                    <asp:DropDownList ID="ddlSucursal" runat="server"
                        DataSourceID="SqlDataSourceEstab" DataTextField="Sucursal"
                        DataValueField="clave" AppendDataBoundItems="True" AutoPostBack="True"
                        onselectedindexchanged="ddlSucursal_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                    </asp:DropDownList>
            </td>
        </tr>
        <tr align="center">
            <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Pto. Emisión:"></asp:Label>
            </td>
            <td style="text-align: left">
                    <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataSourcePtoEmi"
                     DataTextField="ptoEmi" DataValueField="ptoEmi" AppendDataBoundItems="True">
                        <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                    </asp:DropDownList>
            </td>
        </tr>
        <tr align="center">
            <td style="text-align: right">
                    Estado:</td>
            <td style="text-align: left">
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Value="E1">Autorizados</asp:ListItem>
                        <asp:ListItem Value="N0">No autorizados</asp:ListItem>
                        <asp:ListItem Value="P0">Pendiente</asp:ListItem>
                        <asp:ListItem Value="0" Selected="True">Todos</asp:ListItem>
                    </asp:DropDownList>
            </td>
        </tr>
        <tr align="center">
            <td style="text-align: right">
                    <asp:Label ID="Label4" runat="server" Text="Tipo Documento:"></asp:Label>
            </td>
            <td style="text-align: left">
                    <asp:DropDownList ID="ddlTipDoc" runat="server" DataSourceID="SqlDataSourceTipDoc"
                     DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems=true>
                        <asp:ListItem Value="0">Todos</asp:ListItem>
                    </asp:DropDownList>
            </td>
        </tr>
        <tr align="center">
            <td style="text-align: right">
                    <asp:Button ID="bGenerar" CssClass="btGeneralG" runat="server" onclick="bGenerar_Click"
                    Text="Generar" />
            </td>
            <td style="text-align: left">
                    <asp:Button ID="Button1" runat="server" CssClass="btGeneralG"
                        onclick="Button1_Click" Text="Limpiar" />
                </td>
        </tr>
        <tr align="center">
            <td colspan="2">
                    <br />
                <br />
                <asp:Label ID="Label2" runat="server" ForeColor="Red"
                    style="text-align: center"></asp:Label>
                <br />
    <asp:SqlDataSource ID="SqlDataSourceEstab" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT clave, clave + ': ' + sucursal AS Sucursal FROM Sucursales">
    </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourcePtoEmi" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT ptoEmi, estab FROM CajaSucursal GROUP BY ptoEmi, estab HAVING (estab = @estab)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlSucursal" DefaultValue="0" Name="estab" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceTipDoc" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT codigo, descripcion FROM Catalogo1_C WHERE (tipo = 'Comprobante')"></asp:SqlDataSource>
                <br />
                </p>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="MenuIzqTitulo">
    Reportes
</asp:Content>

