<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="/recepcion/RepGeneral.aspx.cs" Inherits="DataExpressWeb.recepcion.RepGeneral" Async="true" %>

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

        .style5 {
        }

        .style6 {
            font-weight: bold;
            text-align: center;
        }

        .style7 {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function validateRepGeneral() {
            var valid = Boolean(Page_ClientValidate());
            if (!valid) {
                alertBootBox('Verifique que los datos sean correctos', 4);
            }
            return valid;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div align="center">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlReporte" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlReporte_SelectedIndexChanged">
                        <asp:ListItem Value="0" Selected="True">Seleccionar Reporte</asp:ListItem>
                        <asp:ListItem Value="General">General</asp:ListItem>
                        <asp:ListItem Value="Conceptos">Conceptos</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />

                <div class="col-md-4"></div>
                <div class="col-md-1">
                    <strong>Emisor:</strong>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlEmisor" runat="server" DataSourceID="SqlDataSourceEmisor" Style="text-align: center" CssClass="form-control" DataTextField="RFCEMI" DataValueField="IDEEMI">
                    </asp:DropDownList>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />

                <div class="col-md-4"></div>
                <div class="col-md-1">
                    <strong>Receptor:</strong>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlReceptor" runat="server" DataSourceID="SqlDataSourceReceptor" Style="text-align: center" CssClass="form-control" DataTextField="RFCREC" DataValueField="IDEREC">
                    </asp:DropDownList>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />

                <div class="col-md-4"></div>
                <div class="col-md-1">
                    <strong>Tipo Fecha:</strong>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlTipoFecha" runat="server" Style="text-align: center" CssClass="form-control">
                        <asp:ListItem Text="Fecha de Recepción" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Fecha de Emisión" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />

                <div class="col-md-4"></div>
                <div class="col-md-1">
                    <strong>Desde:</strong>
                </div>
                <div class="col-md-3">
                    <div class="input-group date">
                        <asp:TextBox ID="tbFechaInicial" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="true" OnTextChanged="tbFechaInicial_TextChanged"></asp:TextBox>
                        <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                    </div>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />

                <div class="col-md-4"></div>
                <div class="col-md-1">
                    <strong>Hasta:</strong>
                </div>
                <div class="col-md-3">
                    <div class="input-group date">
                        <asp:TextBox ID="tbFechaFinal" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                        <span class="input-group-addon"><i class="fa fa-calendar-o"></i></span>
                    </div>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />

                <div class="col-md-5"></div>
                <div class="col-md-2">
                    <strong><asp:Label ID="Label4" runat="server" Text="Tipo de Documento:"></asp:Label></strong>
                </div>
                <div class="col-md-5"></div>
                <br />
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlTipDoc" runat="server" DataSourceID="SqlDataSourceTipDoc" Style="text-align: center" CssClass="form-control" DataTextField="Descripcion" DataValueField="codigo">
                    </asp:DropDownList>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />

                <div class="col-md-4"></div>
                <div class="col-md-2">
                    <strong>Estado SAT:</strong>
                </div>
                <div class="col-md-2">
                    <strong>Estado Validación:</strong>
                </div>
                <div class="col-md-4"></div>
                <br />

                <div class="col-md-4"></div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlEstado" runat="server" Style="text-align: center" CssClass="form-control">
                        <asp:ListItem Value="0" Selected="True">Todos</asp:ListItem>
                        <asp:ListItem Value="E1">Autorizados</asp:ListItem>
                        <%--<asp:ListItem Value="E2">Pendiente</asp:ListItem>--%>
                        <asp:ListItem Value="E4">Anulados por Nota de Crédito</asp:ListItem>
                        <asp:ListItem Value="N0">No Autorizados</asp:ListItem>
                        <asp:ListItem Value="C0">Cancelados</asp:ListItem>
                        <asp:ListItem Value="C2">En proceso de Cancelación</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlEstadoValidacion" runat="server" Style="text-align: center" CssClass="form-control">
                        <asp:ListItem Value="0" Selected="True">Todos</asp:ListItem>
                        <asp:ListItem Value="1">Aprobados</asp:ListItem>
                        <asp:ListItem Value="0">Rechazados</asp:ListItem>
                        <asp:ListItem Value="2">Pendientes</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />
                <br />

                <div class="col-md-4"></div>
                <div class="col-md-2">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="bGenerar" CssClass="btn btn-primary" runat="server" OnClick="bGenerar_Click" Text="Generar">Generar</asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="col-md-2">
                    <asp:LinkButton ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="Button1_Click" Text="Limpiar">Limpiar</asp:LinkButton>
                </div>
                <div class="col-md-4"></div>
                <br />
                <br />

                <asp:UpdateProgress ID="UpdateProgress9" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel9">
                    <ProgressTemplate>
                        <div id="Background"></div>
                        <div id="Progress">
                            <h6>
                                <p style="text-align: center">
                                    <b>Generando Reporte, Espere un momento... </b>
                                </p>
                            </h6>
                            <div class="progress progress-striped active">
                                <div class="progress-bar" style="width: 100%"></div>
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div class="col-md-4"></div>
                <div class="col-md-4">

                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Style="text-align: center">
                    </asp:Label>

                </div>
                <div class="col-md-4"></div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataSourceTipDoc" runat="server" SelectCommand="(SELECT '0' AS codigo, 'Todos' AS Descripcion) UNION (SELECT codigo, Descripcion FROM Cat_Catalogo1_C WHERE (tipo = 'Comprobante'))"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceEmisor" runat="server" SelectCommand="(SELECT '0' AS IDEEMI, 'Todos' AS RFCEMI) UNION (SELECT DISTINCT RFCEMI AS IDEEMI, RFCEMI FROM Cat_Emisor) ORDER BY IDEEMI"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceReceptor" runat="server" SelectCommand="(SELECT '0' AS IDEREC, 'Todos' AS RFCREC) UNION (SELECT DISTINCT RFCREC AS IDEREC, RFCREC FROM Cat_Receptor) ORDER BY IDEREC"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    INFORMES
</asp:Content>
