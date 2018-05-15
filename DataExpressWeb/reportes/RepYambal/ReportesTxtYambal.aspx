<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesTxtYambal.aspx.cs" Inherits="DataExpressWeb.reportes.RepYambal.ReportesTxtYambal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="server">
    REPORTES
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <div class="rowsSpaced">
        <div class="row" align="center">
            <div class="col-md-4"></div>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlReporte" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="true">
                    <asp:ListItem Value="0" Selected="True">Seleccionar Reporte</asp:ListItem>
                    <asp:ListItem Value="REPFACTURA">REPORTE FACTURAS</asp:ListItem>
                    <asp:ListItem Value="REPCOBRANZA">REPORTE COBRANZA</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-4"></div>
        </div>
    </div>
    <div class="rowsSpaced">
        <div class="row" align="center">
            <div class="col-md-2"></div>
            <div class="col-md-1">
                Mes:
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlMes" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="false">
                    <asp:ListItem Value="0" Selected="True">Seleccionar</asp:ListItem>
                    <asp:ListItem Value="01">Enero</asp:ListItem>
                    <asp:ListItem Value="02">Febrero</asp:ListItem>
                    <asp:ListItem Value="03">Marzo</asp:ListItem>
                    <asp:ListItem Value="04">Abril</asp:ListItem>
                    <asp:ListItem Value="05">Mayo</asp:ListItem>
                    <asp:ListItem Value="06">Junio</asp:ListItem>
                    <asp:ListItem Value="07">Julio</asp:ListItem>
                    <asp:ListItem Value="08">Agosto</asp:ListItem>
                    <asp:ListItem Value="09">Septiembre</asp:ListItem>
                    <asp:ListItem Value="10">Octubre</asp:ListItem>
                    <asp:ListItem Value="11">Noviembre</asp:ListItem>
                    <asp:ListItem Value="12">Diciembre</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-1">
                Año:
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlAño" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="false">
                    <asp:ListItem Value="0" Selected="True">Seleccionar</asp:ListItem>
                    <asp:ListItem Value="2017">2017</asp:ListItem>
                    <asp:ListItem Value="2018">2018</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3"></div>
        </div>
    </div>
    <div class="rowsSpaced">
        <div class="row" align="center">
            <div class="col-md-5"></div>
            <div class="col-md-2">
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="bGenerar" CssClass="btn btn-primary" runat="server" OnClick="bGenerar_Click" Text="Generar">Generar</asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-md-5"></div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoInf" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpPie" runat="server">
</asp:Content>
