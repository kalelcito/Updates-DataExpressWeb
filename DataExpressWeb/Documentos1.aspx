<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="DataExpressWeb.Documentos" EnableEventValidation = "false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    </asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpTitulo" runat="server">
    EMISIÓN

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpCuerpoSup" runat="server" Visible="false">

    <%--<button class="btn btn-default" data-toggle="modal" data-target="#FiltrosC2">Filtros</button>--%>
    <asp:LinkButton ID="LinkButton1" runat="server" data-toggle="modal" data-target="#FiltrosC2" CssClass="btn btn-info" onclick="Limpiar" Text="Filtros" >Filtros</asp:LinkButton>

    <asp:LinkButton ID="btnZip" runat="server" CssClass="btn btn-info" onclick="btnZip_Click" Text="Descargar ZIP">Descargar ZIP</asp:LinkButton>
     <div class="col-md-3"><asp:CheckBox ID="checkPDF" runat="server" Checked="True" Text="PDF" CssClass="form-control"  /></div>
               <div class="col-md-3" > <asp:CheckBox ID="checkXML" runat="server" Checked="True" Text="XML" CssClass="form-control"  /></div>
                <div class="col-md-3" ><asp:CheckBox ID="chkReglas" runat="server" Text="Reglas" CssClass="form-control"  /></div>
 <br /><br />
     <div class="col-md-9">  <asp:TextBox ID="tbEmail" runat="server"
                 ValidationGroup="email" CssClass="form-control" placeholder="email@dominio.com,prueba@dataexpressintmx.com *" ></asp:TextBox>
  </div>         <div class="col-md-2" align="center">     <asp:LinkButton ID="bMail" runat="server" CssClass="btn btn-info" onclick="Button1_Click2" Text="Enviar E-Mail" ValidationGroup="email">Enviar E-Mail</asp:LinkButton>

       </div>
    <br /><br />
    <div>
     <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="El formato de E-Mail no es válido"
                        ForeColor="Red" ControlToValidate="tbEmail" ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$"
                        ValidationGroup="email"></asp:RegularExpressionValidator>
    <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
     <asp:Label ID="lbMsgZip" runat="server" ForeColor="Red"></asp:Label>
   </div>
      <div class="modal fade " id="FiltrosC2">
  <div class="modal-dialog modal-lg">
    <div class="modal-content ">
      <div class="modal-header ">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
        <h4 class="modal-title ">Filtros</h4>
      </div>
      <div class="modal-body ">
       &nbsp;<div class="row">


    <div style="width: 12%; display: inline;  float: left; margin-left:2px " >
        <h5>Secuencial:</h5>
         <input type="text" class="form-control" id="tbFolioAnterior" runat="server" ValidationGroup="Folio" placeholder="000000001 *"  required>
     <%--<asp:TextBox ID="tbFolioAnterior2" runat="server"
     CssClass="form-control" placeholder="000000001 *" ></asp:TextBox>--%>
     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*"
     ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationGroup="Folio" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator>
        </div>
    <div  style="width: 12%; display: inline;  float: left; margin-left:2px ">
                 <h5> Comprobante:</h5><asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="SqlDataSourceTipoDoc"
                    DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True"
                     CssClass="form-control">
                    <asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
                </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT codigo, descripcion, tipo FROM Cat_Catalogo1_C WHERE (tipo = 'Comprobante')">
    </asp:SqlDataSource>
    </div>
    <div style="width: 12%; display: inline;  float: left; margin-left:2px ">
          <h5><asp:Label ID="lSucursal" runat="server" Text="Establecimiento:"></asp:Label></h5>
                <asp:DropDownList ID="ddlSucursal" runat="server"
                    DataSourceID="SqlDataSourceSucursales" DataTextField="Cat_Sucursal"
                    DataValueField="clave" AppendDataBoundItems="True" AutoPostBack="True"  CssClass="form-control" onselectedindexchanged="ddlSucursal_SelectedIndexChanged">
                    <asp:ListItem Selected="True">Selecciona el Establecimiento</asp:ListItem>
                </asp:DropDownList>


         <asp:SqlDataSource ID="SqlDataSourceSucursales" runat="server"
                    ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT clave, sucursal + ':' + clave AS Cat_Sucursal FROM Cat_Sucursales where eliminado='False' ORDER BY sucursal ASC">
                </asp:SqlDataSource>
    </div>
           <div style="width: 15%; display: inline;  float: left; margin-left:2px ">


               <asp:Label ID="lPtoEmi" runat="server" Text="Punto de Emisión:"></asp:Label>
                    <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataSourcePtoEmi"  CssClass="form-control"
                     DataTextField="ptoEmi" DataValueField="ptoEmi" AppendDataBoundItems="True">
                        <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                    </asp:DropDownList>

       <asp:SqlDataSource ID="SqlDataSourcePtoEmi" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT ptoEmi, estab FROM Cat_CajasSucursal GROUP BY ptoEmi, estab HAVING (estab = @estab)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlSucursal" DefaultValue="0" Name="estab" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
    </div>
    <div style="width: 15%; display: inline;  float: left; margin-left:2px ">
        <h5> Desde:</h5>
               <div class="input-group date">
                    <asp:TextBox ID="tbFechaInicial" runat="server" CssClass="form-control"  ></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                </div>
    </div >
    <div style="width: 15%; display: inline; float: left; margin-left:2px ">
        <h5> Hasta:</h5>
          <div class="input-group date">
                    <asp:TextBox ID="tbFechaFinal" runat="server" CssClass="form-control"></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
          </div>
        </div>
    <div style="width: 12%; display: inline;  float: left; margin-left:2px ">
 <h5>Estado:</h5>
                <asp:DropDownList ID="ddlEstado" runat="server" AppendDataBoundItems="True"  CssClass="form-control" placeholder="D:\ *">
                    <asp:ListItem Value="0">Selecciona el Estado</asp:ListItem>
                    <asp:ListItem Value="E1">Autorizado</asp:ListItem>
                    <asp:ListItem Value="E2">Pendientes</asp:ListItem>
                    <asp:ListItem Value="N0">No Autorizados</asp:ListItem>
                    <asp:ListItem Value="E0">Anuladas</asp:ListItem>
                    <asp:ListItem Value="P0">Pendientes de Autorizar</asp:ListItem>
                </asp:DropDownList>


<br />
    </div>
</div>
<div class="row">

        <div  style="width: 12%; display: inline;  float: left; margin-left:2px ">
            <h5>Identificación:</h5>
            <asp:TextBox ID="tbRFC" runat="server" CssClass="form-control" MaxLength="13" placeholder="9999999999999 *"></asp:TextBox>
        </div>
        <div  style="width: 40%; display: inline;  float: left; margin-left:2px ">
            <h5>Razón Social:</h5>
            <asp:TextBox ID="tbNombre" runat="server"
                    CssClass="form-control" placeholder="Nombre *" ></asp:TextBox>


        </div>

     <div  style="width: 40%; display: inline;  float: left; margin-left:2px ">
            <h5>Reserva/Tiket:</h5>
            <asp:TextBox ID="tbControl" runat="server" CssClass="form-control" MaxLength="100" placeholder="01001001000000001 *"></asp:TextBox>



        </div>
       <%-- <div style="width: 24%; display: inline;  float: left; margin:16px 2px 0px 10px ">
            <br /> <br />



        </div>--%>
     <br />


   </div>


        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
          <asp:LinkButton ID="bBuscar" CssClass="btn btn-info" runat="server" onclick="bBuscar_Click"
                    Text="Buscar" >Buscar</asp:LinkButton>


              <asp:LinkButton ID="bActualizar"  CssClass="btn btn-info" runat="server" onclick="Button1_Click1"
                    Text="Nueva Busqueda" >Nueva Busqueda</asp:LinkButton>

      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->



    <div class=" modal fade  modal-lg" id="FiltrosC"  ="False" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" >
  <div >

    <div class="modal-content modal-lg">
      <div class="modal-header modal-lg">

      </div>
      <div id="Filtros" class="modal-body modal-lg">
            <form role="form">
              <div class="form-group">




 </div>
           </form>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
        <%--<button type="button" class="btn btn-success" id="botonAventura" onClick="alert('Botón crear')">Crear</button>--%>
      </div>
    </div>
  </div>
</div>


    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoInf" runat="server">




            <asp:Label ID="lCount" runat="server" Text="Registros" Visible="False"></asp:Label>
            <br />
            <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" CellPadding="4"
                CssClass="table table-hover" DataSourceID="SqlDataSource1" AllowPaging="True" AllowSorting="True" >
                <Columns>
                    <asp:ImageField DataImageUrlField="EDOFAC" HeaderText="ESTADO"
                        DataImageUrlFormatString="~/Imagenes/{0}.png">
                        <ControlStyle Height="18px" Width="18px" />
                        <HeaderStyle Width="1%" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:ImageField>
                    <asp:TemplateField HeaderText="RUC" SortExpression="RFCREC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("RFCREC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("RFCREC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RAZÓN SOCIAL" SortExpression="NOMREC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("NOMREC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("NOMREC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TIPO" SortExpression="TIPODOC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox13" runat="server" Text='<%# Bind("TIPODOC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label13" runat="server" Text='<%# Bind("TIPODOC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FOLFAC" HeaderText="NUM DOC"
                        InsertVisible="False" ReadOnly="True"
                        SortExpression="FOLFAC" HeaderStyle-Width="10%">
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="RESERVA/TICKET" SortExpression="codigoControl">
                       <ItemTemplate>
                           <asp:Label ID="lbcodigoControl" runat="server" Text='<%# Bind("codigoControl") %>'></asp:Label>

                        </ItemTemplate>

                    </asp:TemplateField>





                    <asp:TemplateField HeaderText="TOTAL" SortExpression="TOTAL" ItemStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("FOLFAC") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FECHA" SortExpression="FECHA" ItemStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("FECHA") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("FECHA") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="XML" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <a href='download.aspx?file=<%# Eval("XMLARC") %>'>

                                <img src="imagenes/xml.png" alt="xml" border="0"></a>
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PDF" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" height="19"  runat="server"  ImageUrl="~/imagenes/pdf.png"  BorderWidth="0px" BorderStyle="None"
                             Visible='<%# Eval("creado").ToString() == ("1") %>'
                             NavigateUrl='<%# Eval("PDFARC","~/download.aspx?file={0}") %>' > </asp:HyperLink>
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SRI">
                        <ItemTemplate>
                      <a href="#">
                        <asp:HyperLink ID="HyperLink2" height="15" width="15" runat="server" data-toggle="modal" data-target="#ValidarArc" ImageUrl="~/imagenes/modify.png" BorderWidth="0px" BorderStyle="None"
                                Visible='<%# Eval("EDOFAC").ToString() != ("1") %>'
                              NavigateUrl='<%# Eval("ID","~/ResultadoSRI.aspx?comprobante={0}") %>' ></asp:HyperLink></a>



                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MARCAR">
                        <ItemTemplate>
                            <asp:CheckBox ID="check" runat="server"
                                oncheckedchanged="check_CheckedChanged" />
                            <asp:HiddenField ID="checkHdPDF" runat="server" Value='<%# Eval("PDFARC") %>' />
                            <asp:HiddenField ID="checkHdXML" runat="server" Value='<%# Eval("XMLARC") %>' />
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                </Columns>
                <EmptyDataRowStyle CssClass="table" />
                <EmptyDataTemplate>
                    No existen datos.
                </EmptyDataTemplate>
        <FooterStyle   />
        <HeaderStyle CssClass="info"  />
        <PagerStyle  />
                <PagerSettings />
                <RowStyle  />
        <SelectedRowStyle  />
        <SortedAscendingCellStyle  />
        <SortedAscendingHeaderStyle />
        <SortedDescendingCellStyle />
        <SortedDescendingHeaderStyle />
                <PagerTemplate>
                    <div class="text-center">
                        <asp:LinkButton ID="linkPrimero" runat="server" CommandArgument="First" CommandName="Page" Text="&amp;lt;&amp;lt; Primera" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkAnterior" runat="server" CommandArgument="Prev" CommandName="Page" Text="&amp;lt; Anterior" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkSiguiente" runat="server" CommandArgument="Next" CommandName="Page" Text="Siguiente &amp;gt;" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkUltimo" runat="server" CommandArgument="Last" CommandName="Page" Text="&amp;Uacute;ltima &amp;gt;&amp;gt;" />
                    </div>
                </PagerTemplate>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="PA_facturas_basico" SelectCommandType="StoredProcedure"
                onselected="SqlDataSource1_Selected">
        <SelectParameters>
            <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
            <asp:SessionParameter DefaultValue="S--X" Name="SUCURSAL" SessionField="claveSucursalUser"
                Type="String" />
            <asp:SessionParameter DefaultValue="R---" Name="RFC" SessionField="rfcCliente" Type="String" />
            <asp:SessionParameter DefaultValue="false" Name="ROL" SessionField="coFactTodas"
                Type="Boolean" />
            <asp:SessionParameter DefaultValue="" Name="empleado" SessionField="USERNAME"
                Type="String" />
            <asp:SessionParameter DefaultValue="False" Name="ROLSUCURSAL"
                SessionField="coFactPropias" Type="Boolean" />
            <asp:Parameter DefaultValue="" Name="TOP" Type="String" />
        </SelectParameters>

    </asp:SqlDataSource>

<%--<div id="example" class="modal hide fade in" style="display:compact; ">
    <div class="modal-header">
        <a data-dismiss="modal" class="close">×</a>
        <h3>Cabecera de la ventana</h3>
     </div>
     <div class="modal-body">
    <div>


    </div>
  </div>
    <div class="modal-footer">
        <a href="index.html" class="btn btn-success">Guardar</a>
        <a href="#" data-dismiss="modal" class="btn">Cerrar</a>
    </div>
</div>--%>





  <!-- Modal Escenario-->
<div class="modal fade" id="ValidarArc"  ="False" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" >
  <div class="modal-dialog modal-lg">

    <div class="modal-content">
      <div class="modal-header">


      </div>
      <div id="Validacion" class="modal-body">

      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
        <%--<button type="button" class="btn btn-success" id="botonAventura" onClick="alert('Botón crear')">Crear</button>--%>
      </div>
    </div>
  </div>
</div>



</asp:Content>

