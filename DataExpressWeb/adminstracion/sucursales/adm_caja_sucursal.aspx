<%@ Page Language="VB" StylesheetTheme="White2" MasterPageFile="~/MasterCrmExperiencia.master"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">
    
    
    
    
    Public conexion As conexiones = New conexiones()
    
    
    Public funcion_inicio As cs_Funciones_Aritmeticas = New cs_Funciones_Aritmeticas()
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dsTipo.InsertParameters.Item("nombre").DefaultValue = tipo_nuevo_text.Text
        dsTipo.InsertParameters.Item("usuario").DefaultValue = User.Identity.Name
        dsTipo.Insert()
        tipo_nuevo_text.Text = ""
    End Sub
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dsMotivo.InsertParameters.Item("nombre").DefaultValue = tipo_motivo_text.Text
        dsMotivo.InsertParameters.Item("usuario").DefaultValue = User.Identity.Name
        dsMotivo.Insert()
        tipo_motivo_text.Text = ""
    End Sub
    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dsCausal.InsertParameters.Item("nombre").DefaultValue = tipo_causal_text.Text
        dsCausal.InsertParameters.Item("usuario").DefaultValue = User.Identity.Name
        dsCausal.Insert()
        tipo_causal_text.Text = ""
        
    End Sub
    Protected Sub tipo_cliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        grid_tipo.SelectedIndex = -1
        grid_motivo.SelectedIndex = -1
    End Sub

    

    'INICIO 8363 ----------
      Protected Sub chkEnvioCRM_OnCheckedChanged(ByVal sender As  object ,ByVal e As System.EventArgs)
                Dim chkEnvio as CheckBox = sender
                Dim row as GridViewRow = chkEnvio.NamingContainer
                Dim cod_causal as Integer
                Dim cidx as Integer = row.RowIndex
                cod_causal = grid_causal.DataKeys(cidx).Value

                If chkEnvio.Checked = True Then
                    Set_EnvioCRMS(1,cod_causal)
                Else
                    Set_EnvioCRMS(0,cod_causal)
                End If
      End Sub

      Protected Function get_estado(ByVal cau as Integer) as Boolean
                Return True
      End Function
      
      Protected Function get_EnvioCRMS() as Boolean
                Dim row As GridViewRow
                Try
                    For each row in  grid_causal.Rows
                       Dim cb As CheckBox = CType(row.FindControl("chkEnvioCRM"), CheckBox)
                       cb.Checked = True
                    Next  
                Catch ex as Exception
                
                End Try
      End Function
      Protected Sub Set_EnvioCRMS(ByVal opc as Integer,ByVal id_cau as Integer)
             Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DB_CausalesConnectionString").ConnectionString)
             Dim cmd As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand() 
             Dim consulta as String
             Try
                 conn.Open()
                 cmd.CommandType = CommandType.Text
                 cmd.Connection = conn
                 If opc = 1 Then
                    consulta = " INSERT INTO tbl_info SELECT 'A','Enviar CRM Soluciones'," & id_cau & ",isnull((SELECT max(orden)+1 from tbl_info where causal = " & id_cau & " and tipo = 'A'),1)"     
                 ElseIf opc = 0 Then
                    consulta = "DELETE FROM tbl_info WHERE causal = " & id_cau & " AND nombre = 'Enviar CRM Soluciones'"
                 End If
                 cmd.CommandText = consulta
                 cmd.ExecuteNonQuery()
                 conn.Close()
             Catch ex as Exception
             End Try
      End Sub
             
    'FIN 8363 -------------

   
    '------------- 8363
    Protected Sub tipo_problema_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        grid_tipo.SelectedIndex = -1
        grid_motivo.SelectedIndex = -1
    End Sub
    
    '-------------
    
    Protected Sub grid_tipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        grid_motivo.SelectedIndex = -1
        grid_causal.SelectedIndex = -1
        HiddenField2.Value = grid_tipo.SelectedValue
    End Sub
    
    Protected Sub grid_motivo_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        If grid_tipo.SelectedIndex = -1 Then
            grid_motivo.EmptyDataText = ""
        Else
            grid_motivo.EmptyDataText = "No hay motivos ingresados"
        End If
        If grid_tipo.SelectedIndex <> -1 Then
            Panel_motivo.Visible = True
        Else
            Panel_motivo.Visible = False
        End If
    End Sub
    Protected Sub grid_motivo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        grid_causal.SelectedIndex = -1
        HiddenField1.Value = grid_motivo.SelectedValue
    End Sub
    
    Protected Sub grid_causal_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If (e.Row.RowType = DataControlRowType.DataRow) Then
        '    If ((e.Row.RowState And DataControlRowState.Edit) > 0) Then
        '    Else
        '        If CType(e.Row.FindControl("Label2"), Label).Text = "A" And CType(e.Row.FindControl("Label3"), Label).Text = "Trx Axis" Then
        '            e.Row.FindControl("ImageButton1").Visible = True
        '        Else
        '            e.Row.FindControl("ImageButton1").Visible = False
        '        End If
        '    End If
        'End If
        
        
        'If (e.Row.RowType = DataControlRowType.DataRow) Then
        '    If ((e.Row.RowState And DataControlRowState.Edit) > 0) Then
        '    Else
        '        If CType(e.Row.FindControl("Label2"), Label).Text = "A" And CType(e.Row.FindControl("Label3"), Label).Text = "Trx Axis" Then
        '            e.Row.FindControl("ImageButton1").Visible = True
        '        Else
        '            e.Row.FindControl("ImageButton1").Visible = False
        '        End If
        '    End If
        'End If
    End Sub
    
    Protected Sub grid_causal_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        If grid_motivo.SelectedIndex = -1 Then
            grid_causal.EmptyDataText = ""
        Else
            grid_causal.EmptyDataText = "No hay causales ingresadas"
        End If
        If grid_motivo.SelectedIndex <> -1 Then
            Panel_causal.Visible = True
          '  get_EnvioCRMS()
        Else
            Panel_causal.Visible = False
        End If
    End Sub
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        If User.Identity.IsAuthenticated Then
            If User.IsInRole("Administrador") Or User.IsInRole("APL causales") Then
            Else
                Response.Redirect("/portalclaro/login.aspx?ReturnUrl=/portalclaro/webpages/administrador/contactos/crm_principal.aspx")
            End If
            Dim lab1, lab2, lab3 As New Label
            lab1.Text = Profile.Nombre + " " + Profile.Apellido + " [" + User.Identity.Name + "] "
            lab3.Text = Format(Date.Now, "dd/MM/yyyy HH:mm:ss")
            
        Else
            Response.Redirect("/portalclaro/login.aspx?ReturnUrl=/portalclaro/webpages/administrador/contactos/crm_principal.aspx")
        End If
        Dim archivo As String = ""
        Select Case gestion.SelectedValue
            Case "P" : archivo = "lista_causales.js"
            Case "C" : archivo = "lista_causales_cac.js"
        End Select
        Dim fileData As FileInfo = My.Computer.FileSystem.GetFileInfo(Server.MapPath("/portalclaro/webpages/crm_contactos/" & archivo))
        Label3.Text = fileData.LastWriteTime
        usuario.Value = Page.User.Identity.Name
        ip.Value = Request.UserHostAddress
        
    End Sub

    


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("DB_CausalesConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("select id=id_tipo,nombre,tipo_cliente from tbl_tipo where estado='A' and gestion='" & gestion.SelectedValue & "' order by nombre  ", conn)
        Dim reader As SqlDataReader
        Dim texto As StringBuilder = New StringBuilder("var arr_tipo=new Array();" & vbCrLf)
        texto.Append("var arr_motivo=new Array();" & vbCrLf)
        Dim index As Integer = 0
        Dim previousConnectionState As ConnectionState = conn.State
        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            reader = cmd.ExecuteReader()
            Using reader
                While reader.Read
                    texto.Append("arr_tipo[" & index & "]='" & reader.GetInt32(0) & "|" & reader.GetString(1) & "|" & reader.GetString(2) & "';" & vbCrLf)
                    index = index + 1
                End While
            End Using
        Finally
            If previousConnectionState = ConnectionState.Closed Then
                conn.Close()
            End If
            
        End Try
        
        cmd.CommandText = "select a.id_motivo,a.nombre,a.padre from tbl_motivo A inner join tbl_tipo B on a.padre=b.id_tipo  where a.estado='A' and b.gestion='" & gestion.SelectedValue & "'  order by a.padre,a.nombre"
        index = 0
        Dim reader1 As SqlDataReader
        Dim previousConnectionState1 As ConnectionState = conn.State
        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            reader1 = cmd.ExecuteReader()
            Using reader1
                While reader1.Read
                    texto.Append("arr_motivo[" & index & "]='" & reader1.GetInt32(0) & "|" & reader1.GetString(1) & "|" & reader1.GetInt32(2) & "';" & vbCrLf)
                    index = index + 1
                End While
            End Using
        Finally
            If previousConnectionState1 = ConnectionState.Closed Then
                conn.Close()
            End If
        End Try
        Dim archivo As String = ""
        Select Case gestion.SelectedValue
            Case "P" : archivo = "lista_causales.js"
            Case "C" : archivo = "lista_causales_cac.js"
        End Select
        My.Computer.FileSystem.DeleteFile(Server.MapPath("/portalclaro/webpages/crm_contactos/" & archivo), FileIO.UIOption.AllDialogs, FileIO.RecycleOption.DeletePermanently)
        My.Computer.FileSystem.WriteAllText(Server.MapPath("/portalclaro/webpages/crm_contactos/" & archivo), texto.ToString, True)
        'Response.Redirect("crm_principal.aspx", True)
        Dim trama As String = "crm_principal.aspx"
        
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "nombre", "location.href='" + trama + "';", True)
               
        
    End Sub
   
    
    
    Protected Sub grid_tipo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If e.CommandName = "Borrar" Then
            
            Dim But As ImageButton = CType(e.CommandSource, ImageButton)
            Dim gvRow As GridViewRow = But.BindingContainer
            Dim strKey As String = grid_tipo.DataKeys(gvRow.RowIndex)(0).ToString()
            dsTipo.DeleteParameters.Item("id_tipo").DefaultValue = strKey
            dsTipo.DeleteParameters.Item("usuario").DefaultValue = usuario.Value
            dsTipo.DeleteParameters.Item("ip").DefaultValue = ip.Value
            dsTipo.Delete()
            
        End If
    End Sub

    Protected Sub grid_motivo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If e.CommandName = "Borrar" Then
                       
            Dim But As ImageButton = CType(e.CommandSource, ImageButton)
            Dim gvRow As GridViewRow = But.BindingContainer
            Dim strKey As String = grid_motivo.DataKeys(gvRow.RowIndex)(0).ToString()
            
            dsMotivo.DeleteParameters.Item("id_motivo").DefaultValue = strKey
            dsMotivo.DeleteParameters.Item("usuario").DefaultValue = usuario.Value
            dsMotivo.DeleteParameters.Item("ip").DefaultValue = ip.Value
            dsMotivo.Delete()
            
        End If
        
    End Sub

    Protected Sub grid_causal_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If e.CommandName = "Borrar" Then
            Dim But As ImageButton = CType(e.CommandSource, ImageButton)
                       
            Dim gvRow As GridViewRow = But.BindingContainer
            Dim strKey As String = grid_causal.DataKeys(gvRow.RowIndex)(0).ToString()
            
            dsCausal.DeleteParameters.Item("id_causal").DefaultValue = strKey
            dsCausal.DeleteParameters.Item("usuario").DefaultValue = usuario.Value
            dsCausal.DeleteParameters.Item("ip").DefaultValue = ip.Value
            dsCausal.Delete()
            
        End If
        
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim trama As String = ""
        trama = "crm_principal_reporte.aspx?gestion=" + gestion.SelectedValue + "&cliente=" + tipo_cliente.SelectedValue
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "nombre", "location.href='" + trama + "';", True)
        
    End Sub

    Protected Sub gestion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="cabecera" runat="Server">

    <script language="javascript" type="text/javascript" src="/portalclaro/include/js/tiny_mce/tiny_mce.js"></script>

    <script language="javascript" type="text/javascript">

        tinyMCE.init({
            mode: "textareas",
            theme: "advanced",
            language: "en",
            theme_advanced_buttons1: "bold,italic,underline,image,forecolor,backcolor,cut,copy,paste,tablecontrols,separator,strikethrough,justifyleft,justifycenter,justifyright, justifyfull,bullist,numlist,undo,redo,link,unlink, code",
            theme_advanced_buttons2: "",
            theme_advanced_buttons3: "",
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_path_location: "bottom",
            extended_valid_elements: "a[name|href|target|title|onclick],img[class|src|border=0|alt|title|hspace|vspace|width|height|align|onmouseover|onmouseout|name],hr[class|width|size|noshade],font[face|size|color|style],span[class|align|style]"
        });

   


    </script>

    <script language="javascript">
   //<![CDATA[
        function Abrir(causal, motivo) {
            window.open('crm_principal_detalle.aspx?id=' + causal + '&motivo=' + motivo, 'admi', 'scrollbars=yes, width=600,height=550, left=' + ((screen.availWidth - 500) / 2) + ',top=' + ((screen.availHeight - 400) / 2) + '')
        }
        function CopiarCausal(causal) {
            window.open('crm_copiar_causal.aspx?id_causal=' + causal, '', 'scrollbars=no, width=410,height=320, left=' + ((screen.availWidth - 300) / 2) + ',top=' + ((screen.availHeight - 250) / 2) + '')
        }
        function CopiarMotivo(motivo) {
            window.open('crm_copiar_motivo.aspx?id_motivo=' + motivo, '', 'scrollbars=no, width=410,height=300, left=' + ((screen.availWidth - 300) / 2) + ',top=' + ((screen.availHeight - 250) / 2) + '')
        }
        function CopiarTipo(tipo) {
            window.open('crm_copiar_tipo.aspx?id_tipo=' + tipo, '', 'scrollbars=no, width=300,height=250, left=' + ((screen.availWidth - 300) / 2) + ',top=' + ((screen.availHeight - 250) / 2) + '')
        }

        function administrador() {
            var mot = "";
            mot = document.getElementById("ctl00_ctl00_ContentPlaceHolder1_cabecera_HiddenField1").value;
            /*alert(mot)*/
            window.open('crm_adm_verificacion.aspx?motivo=' + mot, 'adm', 'scrollbars=yes,width=500px, height=500, left=320, top=70')

        }

        //   function administrador_trx(causal)
        //   {
        //   var mot,tipo = "";
        //   mot = document.getElementById("ctl00_ctl00_ContentPlaceHolder1_cabecera_HiddenField1").value;
        //   tipo = document.getElementById("ctl00_ctl00_ContentPlaceHolder1_cabecera_HiddenField2").value;
        //   
        //   window.open('crm_adm_causal_axis.aspx?causal='+causal ,'adm','scrollbars=yes,width=400px, height=300, left=320, top=70')
        //   
        //   }


        function administrador_feature(causal) {
            var mot, tipo = "";
            mot = document.getElementById("ctl00_ctl00_ContentPlaceHolder1_cabecera_HiddenField1").value;
            tipo = document.getElementById("ctl00_ctl00_ContentPlaceHolder1_cabecera_HiddenField2").value;

            window.open('crm_adm_causal_feature.aspx?causal=' + causal, 'adm', 'scrollbars=yes,width=400px, height=300, left=320, top=70')

        }
  



     //]]>
    </script>

    <div style="font-size: 0.7em; text-align: center">
        <br />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <strong>GESTIÓN:&nbsp;</strong>
                    <asp:DropDownList ID="gestion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="gestion_SelectedIndexChanged">
                        <asp:ListItem Value="P">Contact Center</asp:ListItem>
                        <asp:ListItem Value="C">CAC</asp:ListItem>
                    </asp:DropDownList></td>
                <td align="right" style="width: 50%">
                    Portal SCO:
                    <asp:Button ID="Button5" runat="server" OnClick="LinkButton1_Click" Text="Actualizar" />
                    (Ult actualización:
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Label"></asp:Label>)</td>
            </tr>
            <tr>
                <td style="width: 50%">
                    <strong>CLIENTE:</strong>
                    <asp:DropDownList ID="tipo_cliente" runat="server" AutoPostBack="True" OnSelectedIndexChanged="tipo_cliente_SelectedIndexChanged" DataSourceID="SqlDataSource1" DataTextField="descripcion" DataValueField="valor">
                       <%-- <asp:ListItem Selected="True" Value="P">Prepago</asp:ListItem>
                        <asp:ListItem Value="T">Postpago</asp:ListItem>
                        <asp:ListItem Value="Y">Pymes</asp:ListItem>
                        <asp:ListItem Value="N">Portanet</asp:ListItem>
                        <asp:ListItem Value="V">Vip</asp:ListItem>
                        <asp:ListItem Value="O">Tlmk</asp:ListItem>
                        <asp:ListItem Value="D">Datos</asp:ListItem>
                        <asp:ListItem Value="F">Retencion</asp:ListItem>
                        <asp:ListItem Value="C">Ases</asp:ListItem>
                        <asp:ListItem Value="E">Regularizacion</asp:ListItem>
                        <asp:ListItem Value="S">CVS</asp:ListItem>--%>
                        
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CausalesConnectionString %>" 
                    SelectCommand="Select * from Tbl_gestion_clientes where estado = 'A' and gestion = @gestion" SelectCommandType="Text">
                    <SelectParameters>
                    <asp:ControlParameter ControlID="gestion" Name="gestion" PropertyName="Selectedvalue" />
                    </SelectParameters>
                    </asp:SqlDataSource>
                    
                    
                    </td>
            </tr>
                 <tr>
                <td style="width: 50%">
                    <strong>TIPO DE PROBLEMA: </strong>
                    <asp:DropDownList ID="tipologia" Heigth="10px" runat="server" AutoPostBack="True" DataSourceID="ds_tipologia" OnSelectedIndexChanged="tipo_problema_SelectedIndexChanged"
                     DataTextField="descrip" DataValueField="id_apl" AppendDataBoundItems="true" >                                             
                    </asp:DropDownList></td>
                <td align="right" style="width: 50%">
                </td>
            </tr>
        </table>
        <br />
        <span class="red">Tipo: </span>
        <asp:TextBox ID="tipo_nuevo_text" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tipo_nuevo_text"
            ErrorMessage="*" SetFocusOnError="True" ValidationGroup="tipo"></asp:RequiredFieldValidator>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Agregar" ValidationGroup="tipo" />
        <img src="/portalclaro/images/adm/excel.jpg" width="14" height="14" />
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">Reporte 
                       Excel</asp:LinkButton>
        <br />
        <br />
        <asp:GridView ID="grid_tipo" runat="server" AutoGenerateColumns="False" DataKeyNames="id_tipo"
            DataSourceID="dsTipo" OnSelectedIndexChanged="grid_tipo_SelectedIndexChanged"
            SkinID="porta" OnRowCommand="grid_tipo_RowCommand">
            <Columns>
                <asp:CommandField ShowEditButton="True" ShowSelectButton="True" ButtonType="Image"
                    CancelImageUrl="~/images/adm/icon-cancel.gif" EditImageUrl="~/images/adm/icon-edit.gif"
                    SelectImageUrl="~/images/adm/icono_adelante.gif" UpdateImageUrl="~/images/adm/icon-save.gif"
                    ValidationGroup="v_tipo">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:CommandField>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/iconos/icon-delete.gif"
                            CommandArgument="Borrar" CommandName="Borrar" />
                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="¿Esta seguro de eliminar el tipo?"
                            TargetControlID="ImageButton2">
                        </cc1:ConfirmButtonExtender>
                    </ItemTemplate>
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo" SortExpression="nombre">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" MaxLength="100" Text='<%# Bind("nombre") %>'
                            Width="350px"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TextBox1"
                            Display="Dynamic" ErrorMessage="Ingrese la descripción" ForeColor="White" SetFocusOnError="True"
                            ValidationGroup="v_tipo"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemStyle Width="500px" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="p_gridview_h" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
            
                
                <asp:TemplateField HeaderText="Estado" SortExpression="estado">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("estado") %>'>
                            <asp:ListItem Value="A">Activo</asp:ListItem>
                            <asp:ListItem Value="I">Inactivo</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("estado") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:TemplateField>
                
                <asp:BoundField DataField="user_creado" HeaderText="Usuario" ReadOnly="True">
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Fecha" SortExpression="fecha">
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%#eval("fecha")%>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:;" onclick="CopiarTipo('<%#eval("id_tipo")%>')">
                            <img border="0" alt="Copiar este tipo a otro cliente" src="/portalclaro/images/atv/icon_copiar2.gif" /></a>
                    </ItemTemplate>
                    <HeaderStyle CssClass="p_gridview_h" />
                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No hay datos
            </EmptyDataTemplate>
        </asp:GridView>
        <asp:HiddenField ID="usuario" runat="server" />
        <asp:HiddenField ID="ip" runat="server" />
        <asp:HiddenField ID="tipo" runat="server" />
        
        <asp:HiddenField ID="HiddenField2" runat="server" />
        
        <asp:SqlDataSource ID="ds_Tipologia" runat="server" ConnectionString="<%$ ConnectionStrings:CRMConnectionString %>" 
         SelectCommand="select id_apl, descrip  from tbl_tipo where estado = 'A'">         
        </asp:SqlDataSource> 
        
        <asp:SqlDataSource ID="dsTipo" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CausalesConnectionString %>"
            DeleteCommand="sp_delete_tipo" DeleteCommandType="StoredProcedure" InsertCommand="INSERT INTO [tbl_tipo] ([nombre], [estado],[tipo_cliente],gestion, user_creado) VALUES (@nombre, 'I',@tipo, @gestion,@usuario)"
            SelectCommand="SELECT [id_tipo],[nombre], ce.estado, [user_creado],convert(varchar,fecha,103)+' '+ convert(char(5),fecha,108)as fecha FROM [tbl_tipo] ce WHERE ([tipo_cliente] = @tipo_cliente) and not ce.estado='E'  and gestion=@gestion order by nombre"
            UpdateCommand="UPDATE [tbl_tipo] SET [nombre] = @nombre,[estado] = @estado, fecha = getdate() WHERE [id_tipo] = @id_tipo">
            <DeleteParameters>
                <asp:Parameter Name="id_tipo" Type="Int32" />
                <asp:Parameter Name="usuario" Type="string" />
                <asp:Parameter Name="ip" Type="string" />
                
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="nombre" Type="String" />                
                <asp:Parameter Name="estado" Type="String" />
                <asp:Parameter Name="id_tipo" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="tipo_cliente" Name="tipo_cliente" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="gestion" Name="gestion" PropertyName="SelectedValue" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="nombre" Type="String" />
                <asp:ControlParameter ControlID="tipo_cliente" Name="tipo" PropertyName="SelectedValue" />
                <asp:Parameter Name="usuario" />
                <asp:ControlParameter ControlID="gestion" Name="gestion" PropertyName="SelectedValue" />                            
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:Panel ID="Panel_motivo" Style="padding-top: 10px" runat="server" Width="100%">
            <hr color="red" style="border-style: dotted" />
            <span class="red">Motivo: </span>
            <asp:TextBox ID="tipo_motivo_text" runat="server" MaxLength="100" Width="200px"></asp:TextBox>&nbsp;
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Agregar" ValidationGroup="brmotivo" /><br />
        </asp:Panel>
        <br />
        <asp:GridView ID="grid_motivo" runat="server" AutoGenerateColumns="False" DataKeyNames="id_motivo"
            DataSourceID="dsMotivo" OnDataBinding="grid_motivo_DataBinding" OnSelectedIndexChanged="grid_motivo_SelectedIndexChanged"
            SkinID="porta" OnRowCommand="grid_motivo_RowCommand">
            <Columns>
                <asp:CommandField ShowEditButton="True" ShowSelectButton="True" ButtonType="Image"
                    CancelImageUrl="~/images/adm/icon-cancel.gif" EditImageUrl="~/images/adm/icon-edit.gif"
                    SelectImageUrl="~/images/adm/icono_adelante.gif" UpdateImageUrl="~/images/adm/icon-save.gif"
                    ValidationGroup="v_motivo">
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:CommandField>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/iconos/icon-delete.gif"
                            CommandArgument="Borrar" CommandName="Borrar" />
                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="¿Esta seguro de eliminar el motivo?"
                            TargetControlID="ImageButton2">
                        </cc1:ConfirmButtonExtender>
                    </ItemTemplate>
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Motivo" SortExpression="nombre">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" MaxLength="100" Text='<%# Bind("nombre") %>'
                            Width="350px"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TextBox1"
                            Display="Dynamic" ErrorMessage="Ingrese el motivo" ForeColor="White" ValidationGroup="v_motivo"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemStyle Width="500px" />
                    <HeaderStyle HorizontalAlign="Left" CssClass="p_gridview_h" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado" SortExpression="estado">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server" SelectedValue='<%# Bind("estado") %>'>
                            <asp:ListItem Value="A">Activo</asp:ListItem>
                            <asp:ListItem Value="I">Inactivo</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("estado") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="Usuario" DataField="user_creado" ReadOnly="True">
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Fecha" SortExpression="fecha">
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%#eval("fecha")%>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:;" onclick="CopiarMotivo('<%#eval("id_motivo")%>')">
                            <img border="0" alt="Copiar este motivo a otro tipo" src="/portalclaro/images/atv/icon_copiar2.gif" /></a>
                    </ItemTemplate>
                    <HeaderStyle CssClass="p_gridview_h" />
                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <%--</ContentTemplate></atlas:UpdatePanel> SelectCommand="SELECT [id_motivo], [nombre], [estado], [padre], [user_Creado] FROM [tbl_motivo] WHERE ([padre] = @padre) and not estado='E' ORDER BY nombre"--%>
        <br />
        <asp:SqlDataSource ID="dsMotivo" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CausalesConnectionString %>"
            DeleteCommand="sp_delete_motivo" DeleteCommandType="StoredProcedure" InsertCommand="INSERT INTO [tbl_motivo] ([nombre], [estado], [padre], [user_creado]) VALUES (@nombre, 'I', @padre,@usuario)"
            SelectCommand="SELECT [id_motivo], [nombre], [estado], [padre], [user_Creado],convert(varchar,fecha,103)+' '+ convert(char(5),fecha,108)as fecha FROM [tbl_motivo] WHERE ([padre] = @padre) and not estado='E' ORDER BY nombre"
            UpdateCommand="UPDATE [tbl_motivo] SET [nombre] = @nombre, [estado] = @estado, fecha = getdate() WHERE [id_motivo] = @id_motivo">
            <DeleteParameters>
                <asp:Parameter Name="id_motivo" Type="Int32" />
                <asp:Parameter Name="usuario" Type="string" />
                <asp:Parameter Name="ip" Type="string" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="nombre" />
                <asp:Parameter Name="estado" />
                <asp:Parameter Name="id_motivo" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="grid_tipo" Name="padre" PropertyName="SelectedValue"
                    Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="nombre" Type="String" />
                <asp:ControlParameter ControlID="grid_tipo" Name="padre" PropertyName="SelectedValue"
                    Type="Int32" />
                <asp:Parameter Name="usuario" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:Panel ID="Panel_causal" Style="padding-top: 10px" runat="server" Width="100%">
            <hr color="red" style="border-style: dotted" />
            <table border="0" cellpadding="0" cellspacing="5" style="width: 100%">
                <tr>
                    <td style="width: 50%" valign="top">
                        <span class="red">Causal:</span>
                        <asp:TextBox ID="tipo_causal_text" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Agregar" ValidationGroup="ag_Ca" /></td>
                    <td style="width: 50%" valign="top">
                        <span class="red">Verificación:</span> <a href="javascript:;" onclick="administrador()">
                            <asp:Label ID="Label4" runat="server" Text="Administrador"></asp:Label>
                        </a>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%--</ContentTemplate></atlas:UpdatePanel   --%>
        <table border="0" cellpadding="0" cellspacing="5" style="width: 100%">
            <tr>
                <td style="width: 50%" valign="top">
                    <asp:GridView ID="grid_causal" runat="server" AutoGenerateColumns="False" DataKeyNames="id_causal"
                        DataSourceID="dsCausal" SkinID="porta" OnRowDataBound="grid_causal_RowDataBound"
                        OnDataBinding="grid_causal_DataBinding" OnRowCommand="grid_causal_RowCommand">
                        <Columns>
                            <asp:CommandField ShowEditButton="True" EditImageUrl="~/images/adm/icon-edit.gif"
                                UpdateImageUrl="~/images/adm/icon-save.gif" ButtonType="Image" CancelImageUrl="~/images/adm/icon-cancel.gif">
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:CommandField>
                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/iconos/icon-delete.gif"
                                        CommandArgument="Borrar" CommandName="Borrar" />
                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="¿Esta seguro de eliminar el causal?"
                                        TargetControlID="ImageButton2">
                                    </cc1:ConfirmButtonExtender>
                                </ItemTemplate>
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a href="javascript:;" onclick="Abrir('<%#eval("id_causal")%>','<%# eval("motivo") %>')">
                                        <img alt="Editar Alternativas-Información" border="0" src="/portalclaro/images/atv/icon_gen14.gif" /></a>
                                    <a href="javascript:;" onclick="CopiarCausal('<%#eval("id_causal")%>')">
                                        <img border="0" alt="Copiar esta causa a otro motivo" src="/portalclaro/images/atv/icon_copiar2.gif" /></a>
                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:TemplateField>
                            
                             <asp:TemplateField>
                                <ItemTemplate>
                                    <a id="imgadm" href="javascript:;" onclick="administrador_feature('<%#eval("id_causal")%>')"  >
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="/portalclaro/images/atv/icon_gen8.gif" ToolTip="Asignar Feature"
                                            />
                                    </a>
                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:TemplateField>
                           <%-- <asp:TemplateField>
                                <ItemTemplate>
                                    <a id="imgadm" href="javascript:;" onclick="administrador_trx('<%#eval("id_causal")%>')">
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/portalclaro/images/atv/icon_gen11.gif" ToolTip="Asignar Trx Axis"
                                            Visible="false" />
                                    </a>
                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Causal" SortExpression="nombre">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="100" Text='<%# Bind("nombre") %>'
                                        ValidationGroup="v_causal" Width="150px"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TextBox1"
                                        Display="Dynamic" ErrorMessage="Ingrese la causal" ForeColor="White" ValidationGroup="v_causal"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemStyle Width="200px" />
                                <HeaderStyle HorizontalAlign="Left" CssClass="p_gridview_h" />
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="estado">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList3" runat="server" SelectedValue='<%# Bind("estado") %>'>
                                        <asp:ListItem Value="A">Activo</asp:ListItem>
                                        <asp:ListItem Value="I">Inactivo</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("estado") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="user_Creado" HeaderText="Usuario" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Fecha" SortExpression="fecha">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%#eval("fecha")%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Categor&#237;a" SortExpression="cat_nombre">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList4" runat="server" AppendDataBoundItems="True" DataSourceID="dsCategorias"
                                        DataTextField="cat_nombre" DataValueField="id_categoria" SelectedValue='<%# bind("categoria") %>'>
                                        <%--<asp:ListItem Value="">--</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <%--<asp:HiddenField ID="HiddenField2" runat="server" Value= '<%# bind("categoria") %>'/>--%>
                                    <asp:SqlDataSource ID="dsCategorias" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CausalesConnectionString %>"
                                        SelectCommand="SELECT [id_categoria], [cat_nombre] FROM [tbl_causal_categorias]">
                                    </asp:SqlDataSource>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("cat_nombre") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:TemplateField>


                             <asp:TemplateField HeaderText="Tipo Problema" SortExpression="descrip">
                    <EditItemTemplate>
                        <asp:DropDownList ID="cmbTipoProblema" runat="server" DataSourceID="ds_tipologia" DataTextField="descrip" DataValueField="id_apl" SelectedValue='<%# Bind("tipo_problema") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblTipoProblema" Width="200px" runat="server" Text='<%# Bind("descrip") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                    <HeaderStyle CssClass="p_gridview_h" />
                </asp:TemplateField>

                             <asp:TemplateField HeaderText="Enviar a CRM Soluciones">
                                 <ItemTemplate>
                                 <asp:CheckBox ID="chkEnvioCRM" runat="server" 
                            AutoPostBack="true" OnCheckedChanged="chkEnvioCRM_OnCheckedChanged" Checked='<%# get_estado(Eval("id_causal")) %>'/>
                            </ItemTemplate> 
                              <ItemStyle HorizontalAlign="Center" Width="100px" />
                             <HeaderStyle CssClass="p_gridview_h" />                   
                            </asp:TemplateField>

                            
                        </Columns>
                    </asp:GridView>
                    <%-- </contenttemplate></atlas:UpdatePanel> SelectCommand="SELECT [id_causal], [nombre],A.estado, [motivo], [user_creado], cat_nombre,categoria FROM [tbl_causal]  A &#13;&#10;left join tbl_causal_categorias on categoria=id_categoria WHERE ([motivo] = @motivo) and not A.estado='E' order by nombre"--%>
                    <asp:SqlDataSource ID="dsCausal" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CausalesConnectionString %>"
                        DeleteCommand="sp_delete_causal" DeleteCommandType="StoredProcedure" InsertCommand="INSERT INTO [tbl_causal] ([nombre], [estado], [motivo],[user_Creado],categoria,tipo_problema) VALUES (@nombre, 'I', @motivo,@usuario,15,@tipo_problema)"
                        SelectCommand="SELECT [id_causal], [nombre],A.estado, [motivo], [user_creado],convert(varchar,fecha,103)+' '+ convert(char(5),fecha,108)as fecha, cat_nombre,categoria, cr.descrip , A.tipo_problema FROM [tbl_causal]  A left join tbl_causal_categorias on categoria=id_categoria join crm.dbo.tbl_tipo cr on A.tipo_problema = cr.id_apl WHERE ([motivo] = @motivo) and not A.estado='E' order by nombre"
                        UpdateCommand="UPDATE [tbl_causal] SET [nombre] = @nombre, [estado] = @estado,fecha = getdate(), categoria=@categoria&#13;&#10; where id_causal=@id_Causal">
                        <DeleteParameters>
                            <asp:Parameter Name="id_causal" Type="Int32" />
                            <asp:Parameter Name="usuario" Type="string" />
                            <asp:Parameter Name="ip" Type="string" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="nombre" Type="String" />
                            <asp:Parameter Name="estado" Type="String" />
                            <asp:Parameter Name="motivo" Type="Int32" />
                            <asp:Parameter Name="id_causal" Type="Int32" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="grid_motivo" Name="motivo" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:Parameter Name="nombre" Type="String" />
                            <asp:ControlParameter ControlID="grid_motivo" Name="motivo" PropertyName="SelectedValue"
                                Type="Int32" />
                            <asp:Parameter Name="usuario" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                </td>
                <td style="width: 50%" valign="top">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <%--   <asp:GridView ID="grid_verifica" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="id_verificacion" DataSourceID="dsVerifica" BorderWidth="0" >
                        <Columns>--%>
                    <%-- SkinID="porta" <asp:CommandField ButtonType="Image" CancelImageUrl="~/images/adm/icon-cancel.gif"
                                EditImageUrl="~/images/adm/icon-edit.gif" ShowEditButton="True" UpdateImageUrl="~/images/adm/icon-save.gif" DeleteImageUrl="~/images/adm/icon-delete.gif" ShowDeleteButton="True" ValidationGroup="v_verifica" >
                                <HeaderStyle CssClass="p_gridview_h" />
                            </asp:CommandField>--%>
                    <%-- <asp:TemplateField HeaderText="Verificaci&#243;n" SortExpression="nombre">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="100" Text='<%# Bind("nombre") %>'
                                        Width="150px"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TextBox1"
                                        ErrorMessage="Ingrese la verificación" ForeColor="White" SetFocusOnError="True"
                                        ValidationGroup="v_verifica" Display="Dynamic"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemStyle Width="200px" />
                                <HeaderStyle HorizontalAlign="Left" CssClass="p_gridview_h" />
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                    <%--   <asp:TemplateField>--%>
                    <%--<EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" MaxLength="2" Text='<%# bind("orden") %>'
                                        Width="20px"></asp:TextBox>
                                </EditItemTemplate>--%>
                    <%--   <ItemTemplate>
                                    
                                    
                                   <a  href="javascript:;" onclick="administrador()"  >
                                         <asp:Label ID="Label4" runat="server" Text="Administrador"></asp:Label>
                                   </a>
                                   
                                </ItemTemplate>--%>
                    <%-- <HeaderStyle CssClass="p_gridview_h" />--%>
                    <%--        </asp:TemplateField>
                        </Columns>
                    </asp:GridView>--%>
                    <%--  <asp:SqlDataSource ID="dsVerifica" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CausalesConnectionString %>"
                        DeleteCommand="DELETE FROM [tbl_verificaciones] WHERE [id_verificacion] = @id_verificacion"
                        InsertCommand="INSERT INTO [tbl_verificaciones] ([motivo], [nombre], [estado]) VALUES (@motivo, @nombre, 'A')"
                        SelectCommand="SELECT [id_verificacion], [motivo], [nombre], [estado], orden FROM [tbl_verificaciones] WHERE ([motivo] = @motivo) order by orden"
                        UpdateCommand="UPDATE [tbl_verificaciones] SET  [nombre] = @nombre, orden=@orden WHERE [id_verificacion] = @id_verificacion">
                        <DeleteParameters>
                            <asp:Parameter Name="id_verificacion" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="nombre" />
                            <asp:Parameter Name="estado" />
                            <asp:Parameter Name="id_verificacion" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="grid_motivo" Name="motivo" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:ControlParameter ControlID="grid_motivo" Name="motivo" PropertyName="SelectedValue"
                                Type="Int32" />
                            <asp:Parameter Name="nombre" Type="String" />
                        </InsertParameters>
                    </asp:SqlDataSource>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<%--</form>
</body>
</html>--%>
