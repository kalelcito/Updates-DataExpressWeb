<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="DataExpressWeb.Default" %>

<asp:Content ID="cpCabecera" runat="server" ContentPlaceHolderID="cpCabecera">
    <style type="text/css">
        .style1 {
            font-size: small;
        }
    </style>
</asp:Content>
<asp:Content ID="cpCuerpoSup" runat="server" ContentPlaceHolderID="cpCuerpoSup">
    <p>
        <span class="welcome_text"><strong>Bienvenido</strong></span> <span class="web_text"><strong>DataExpress Latinoamérica (México)
        </strong>
        </span>
        <br />
        <br />
        <span class="style1">Para obtener más información acerca de DataExpress Latinoamérica (México), visite</span>
        <a href="http://www.dataexpressintmx.com/" target="_blank" title="Sitio web de DataExpress Latinoamérica (México)">http://www.dataexpressintmx.com/</a>
    </p>
    <asp:Panel ID="pNotify" runat="server">
        
    </asp:Panel>
        <script type="text/javascript">
            function messages(texto,tipo)
            {
               $().toastmessage('showToast', {
                   text     : texto,
                   sticky   : true,
                   position : 'top-left',
                   type     : tipo
                });
            }
        </script>
</asp:Content>
