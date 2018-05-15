<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="crearNotaCreditoNuevo.aspx.cs" Inherits="DataExpressWeb.CrearNotaCreditoNuevo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        #Background {
            background-color: #F0F0F0;
            bottom: 0px;
            filter: alpha(opacity=70);
            left: 0px;
            margin: 0;
            overflow: hidden;
            padding: 0;
            position: fixed;
            right: 0px;
            top: 0px;
            z-index: 100000;
        }

        #Progress {
            background-color: #FFFFFF;
            background-position: center;
            background-repeat: no-repeat;
            border: 1px solid Gray;
            filter: alpha(opacity=70);
            height: 20%;
            left: 40%;
            position: fixed;
            top: 40%;
            width: 20%;
            z-index: 100001;
        }
    </style>
    <script type="text/javascript">
        function ddlClick() {
            var ddlBusq = $('#<%= ddlBusq.ClientID %>');
            var selectedValue = ddlBusq.val();
            $('#<%= tbUUID.ClientID %>').val('');
            $('#<%= tbFolio.ClientID %>').val('');
            $('#<%= tbSerie.ClientID %>').val('');
            if (selectedValue == "1") {
                $('#divUUID').css('display', 'inline');
                $('#divSerFol').css('display', 'none');
            } else if (selectedValue == "2") {
                $('#divUUID').css('display', 'none');
                $('#divSerFol').css('display', 'inline');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="rowsSpaced">
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-2">BUSCAR POR:</div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlBusq" runat="server" Style="text-align: center" CssClass="form-control" onChange="javascript:ddlClick()">
                            <asp:ListItem Value="1" Text="UUID" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="2" Text="SERIE/FOLIO"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="divUUID" style="display: inline">
                        <div class="col-md-6">
                            <asp:TextBox ID="tbUUID" runat="server" CssClass="form-control" Style="text-align: center" placeholder="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"></asp:TextBox>
                            <asp:MaskedEditExtender runat="server" ID="tbUUID_MaskedEditExtender" TargetControlID="tbUUID" ClearMaskOnLostFocus="false" MaskType="None" Mask="CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC" Filtered="ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" />
                        </div>
                    </div>
                    <div id="divSerFol" style="display: none">
                        <div class="col-md-1">SERIE:</div>
                        <div class="col-md-2">
                            <asp:TextBox ID="tbSerie" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                        </div>
                        <div class="col-md-1">FOLIO:</div>
                        <div class="col-md-2">
                            <asp:TextBox ID="tbFolio" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-2">EMISOR:</div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlRFC" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataRFC" DataTextField="RFCEMI" DataValueField="IDEEMI">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataRFC" runat="server" SelectCommand="SELECT IDEEMI,RFCEMI FROM Cat_Emisor"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-1">AMBIENTE:</div>
                    <div class="col-md-2">
                        <asp:HiddenField ID="hfambiente" runat="server" />
                        <asp:TextBox ID="tbAmbiente" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col-md-1">SERIE:</div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlSerie" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataSeries" DataTextField="serie" DataValueField="idSerie" AutoPostBack="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSeries" runat="server" SelectCommand="SELECT s.* FROM Cat_ModuloEmpleado m INNER JOIN Cat_Series s ON s.idSerie = m.idSerie WHERE s.tipoDoc = '04' AND s.tipo = 2 AND m.idEmpleado = @idUser">
                            <SelectParameters>
                                <asp:SessionParameter Name="idUser" SessionField="idUser" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-2">MOTIVO:</div>
                    <div class="col-md-8">
                        <asp:TextBox ID="tbMotivo" runat="server" Style="text-align: center" CssClass="form-control" placeholder="MOTIVO DE ANULACIÓN *"></asp:TextBox>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row"> 
                    <div class="col-md-1"></div>
                    <div class="col-md-2">Uso CFDI:</div>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlUsoCFDI" runat="server" Style="text-align: center" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="bFinishButton" runat="server" OnClick="FinishButton_Click" CssClass="btn btn-primary">Generar</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-5"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel3">
        <ProgressTemplate>
            <div id="Background"></div>
            <div id="Progress">
                <h6>
                    <p style="text-align: center">
                        <b>Generando Comprobante Electronico, Espere un momento... </b>
                    </p>
                </h6>
                <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/imagenes/loading.gif" />--%>
                <div class="progress progress-striped active">
                    <div class="progress-bar" style="width: 100%"></div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="cpTitulo">
    NOTA DE CRÉDITO DE ANULACIÓN
</asp:Content>
