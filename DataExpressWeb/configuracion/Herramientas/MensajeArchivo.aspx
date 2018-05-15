<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MensajeArchivo.aspx.cs" Inherits="DataExpressWeb.MensajeArchivo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/css/bootstrap.min.css" rel="stylesheet">
    <link href="~/css/bootstrap-datepicker.css" rel="stylesheet">
    <link href="~/css/bootstrap-theme.min.css" rel="stylesheet">
    <link href="~/css/theme.css" rel="stylesheet">

    <title></title>
    <style type="text/css">
        .style1 {
            width: 85%;
        }
        .styleSize {
            resize: vertical;
        }
        .style2 {
            width: 193px;
            font-weight: bold;
            text-align: right;
        }

        .style3 {
            font-size: large;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div cssclass="form-control">


            <div>
               <h3> Mensajes</h3>
           <br />

<%--                <div class="col-md-12"><strong>Clave Acceso:</strong><asp:TextBox ID="tbClaveAcceso" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6"><strong>Numero de Autorización:</strong><asp:TextBox ID="tbNoAutorizacion" runat="server" CssClass="form-control"></asp:TextBox></div>

                <div class="col-md-6"><strong>Fecha de Autorización:</strong><asp:TextBox ID="tbFechaAutorizacion" runat="server" CssClass="form-control"></asp:TextBox></div>

                <div class="col-md-4"><strong>Estado:</strong><asp:TextBox ID="tbEstado" runat="server" CssClass="form-control"></asp:TextBox></div>

                <div class="col-md-4"><strong>Ambiente:</strong><asp:TextBox ID="tbAmbiente" runat="server" CssClass="form-control"></asp:TextBox></div>

                <div class="col-md-4"><strong>Emisión:</strong><asp:TextBox ID="tbEmision" runat="server" CssClass="form-control"></asp:TextBox></div>

                <div class="col-md-4"><strong>Comprobante:</strong><asp:TextBox ID="tbComprobante" runat="server" CssClass="form-control"></asp:TextBox></div>

                <div class="col-md-4"><strong>Número:</strong><asp:TextBox ID="tbNumero" runat="server" CssClass="form-control"></asp:TextBox></div>

                <div class="col-md-4"><strong>Código:</strong><asp:TextBox ID="tbCodigo" runat="server" CssClass="form-control"></asp:TextBox></div>--%>

                <div class="col-md-4">
                    <strong>Mensaje:</strong><asp:TextBox ID="tbMensaje" runat="server" CssClass="form-control styleSize"
                        TextMode="MultiLine" Height="55px" Width="640px"></asp:TextBox>
                </div>

                <div class="col-md-8">
                    Mensaje Tecnico<strong>:</strong><asp:TextBox ID="tbMensajeTecnico" runat="server" CssClass="form-control styleSize"
                        TextMode="MultiLine" Height="55px" Width="640px"></asp:TextBox>
                </div>
               <%-- <div class="col-md-8">
                    <textarea ID="tbIA" class="form-control" runat="server" cols="30" rows="5"></textarea></div>--%>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
            </div>

        </div>
    </form>

</body>
</html>
