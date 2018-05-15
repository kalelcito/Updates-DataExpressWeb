<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Trama.aspx.cs" Inherits="DataExpressWeb.Trama" %>

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
<%--               <h3> Trama </h3>
           <br />
                <div class="col-md-4">
                    <strong>Mensaje:</strong>--%>
                    <asp:TextBox ID="tbTrama" runat="server" CssClass="form-control styleSize"
                        TextMode="MultiLine" Height="404px" Width="1005px"></asp:TextBox>
<%--                </div>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
        </div>--%>
    </form>

</body>
</html>
