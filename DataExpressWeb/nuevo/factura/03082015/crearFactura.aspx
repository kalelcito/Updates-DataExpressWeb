<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="crearFactura.aspx.cs" Inherits="DataExpressWeb.crearFactura" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
<%--   <script language="javascript">
       function TextBox_LostFocus() {
           var Cant;
           var Impu;
           var tbPU;
           var tbDescuento;

           Impu = document.getElementById("tbTarifa").value;
           Cant = document.getElementById("tbCantidad").value;
           tbPU = document.getElementById("tbPU").value;
           tbDescuento = document.getElementById("tbDescuento").value;

           x = x * 1234;
           lblResultado.innerText = "Valor:" + x;
       }
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
              <div class="row">



    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="1" OnActiveStepChanged="Wizard1_ActiveStepChanged"
        OnNextButtonClick="StepNextButton_Click">
        <FinishNavigationTemplate>
             <div align="center"><asp:LinkButton ID="FinishPreviousButton"  CssClass="btn btn-info" runat="server" CausesValidation="False" CommandName="MovePrevious"
                    Text="Anterior" >Anterior</asp:LinkButton>

            &nbsp;&nbsp;
            <asp:LinkButton ID="FinishButton"  CssClass="btn btn-info" runat="server" OnClick="FinishButton_Click"  CommandName="MoveComplete"
                    Text="Crear Documento" >Crear Documento</asp:LinkButton>

            <asp:ConfirmButtonExtender ID="FinishButton_ConfirmButtonExtender" runat="server" BehaviorID="FinishButton_ConfirmButtonExtender" ConfirmText="Confirmar que los datos Ingresados son correctos." TargetControlID="FinishButton">
            </asp:ConfirmButtonExtender>
                 </div>
        </FinishNavigationTemplate>
        <HeaderStyle />
        <NavigationButtonStyle  />
        <NavigationStyle />
        <SideBarButtonStyle />
        <SideBarStyle  />
        <SideBarTemplate>
            <asp:DataList ID="SideBarList" runat="server">
                <ItemTemplate>
                    <asp:LinkButton ID="SideBarButton" runat="server" ></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server"> </asp:LinkButton>
                    <asp:LinkButton ID="LinkButton3" runat="server"> </asp:LinkButton>

                </ItemTemplate>
                <SelectedItemStyle Font-Bold="True" />
            </asp:DataList>
        </SideBarTemplate>
        <StartNavigationTemplate>
            <div align="center">
             <asp:LinkButton ID="StartNextButton"  CssClass="btn btn-info" runat="server" CommandName="MoveNext"
                    Text="Siguiente" >Siguiente</asp:LinkButton>
                </div>
        </StartNavigationTemplate>
        <StepNavigationTemplate>

             <div align="center"> <asp:LinkButton ID="StepPreviousButton"  CssClass="btn btn-info" CausesValidation="False" runat="server" CommandName="MovePrevious"
                    Text="Anterior" >Anterior</asp:LinkButton>

              <asp:LinkButton ID="StepNextButton"  ValidationGroup="Form" CssClass="btn btn-info" runat="server" CommandName="MoveNext"
                    Text="Siguiente" >Siguiente</asp:LinkButton>
            </div>
        </StepNavigationTemplate>
        <StepStyle/>
        <WizardSteps>
            <asp:WizardStep runat="server" Title="Tributaria" ID="infoTributaria"  >
                    <h4>Información Tributaria</h4>
                 <div class="row">
                     <div class="col-md-4">
                            AMBIENTE:<br />
                            <asp:DropDownList ID="ddlAmbiente" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataAmbiente"
                                DataTextField="descripcion" DataValueField="codigo"  CssClass="form-control">
                            </asp:DropDownList>
                      </div>
                     <div class="col-md-4">
                            EMISIÓN:<br />
                            <asp:DropDownList ID="ddlEmision" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataEmision"
                                DataTextField="descripcion" DataValueField="codigo" CssClass="form-control" >
                            </asp:DropDownList>
                      </div>
                      <div class="col-md-4">
                            COMPROBANTE:<br />
                            <asp:DropDownList ID="ddlComprobante" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SqlDataDocumento" DataTextField="descripcion" DataValueField="codigo" CssClass="form-control" >
                            </asp:DropDownList>
                     </div>
                    </div>
                    <div class="row">
                      <div class="col-md-4">
                            SUCURSAL:<br />
                            <asp:DropDownList ID="ddlSucursal" runat="server" DataSourceID="SqlDataSucursal"
                                DataTextField="sucursal" DataValueField="clave" CssClass="form-control" >
                            </asp:DropDownList>
                      </div>
                      <div class="col-md-4">
                            PUNTO DE EMISIÓN:<br />
                            <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataPtoEmision"
                                DataTextField="noEmpleado" DataValueField="noEmpleado" CssClass="form-control" >
                            </asp:DropDownList>
                      </div>
                      <div class="col-md-4">
                            SECUENCIAL
                            <br />
                            <asp:TextBox ID="tbSecuencial" runat="server"  Visible="True"  CssClass="form-control" placeholder="000000001 *"></asp:TextBox>
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-md-4">
                            <asp:Label ID="lRuc" runat="server" Text="RUC:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRuc" runat="server" MaxLength="13" CssClass="form-control" placeholder="0000000000001 *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRuc"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                      </div>
                      <div class="col-md-8">
                            <asp:Label ID="lRazonSocial" runat="server" Text="RAZÓN SOCIAL:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocial" runat="server"  CssClass="form-control" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbRazonSocial"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                     </div>
                    </div>
                    <div class="row">
                      <div class="col-md-6">
                       <asp:Label ID="lNombreComercial" runat="server" Text="NOMBRE COMERCIAL" ></asp:Label>
                            <br />
                            <asp:TextBox ID="tbNombreComercial" runat="server" CssClass="form-control" placeholder="0000000000001 *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbNombreComercial"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                      </div>
                      <div class="col-md-3">
                          CONTRIBUYENTE ESPECIAL:
                            <asp:CheckBox ID="cbContribuyenteEspecial" runat="server" Visible="True" />
                      </div>
                      <div class="col-md-2">
                            <asp:TextBox ID="tbContribuyenteEspecial" runat="server" MaxLength="4"
                                Visible="True" Width="51px" CssClass="form-control" placeholder="#000 *"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbContribuyenteEspecial_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Custom, Numbers"
                                TargetControlID="tbContribuyenteEspecial">
                            </asp:FilteredTextBoxExtender>
                      </div>
                      <div class="col-md-3">
                            <asp:CheckBox ID="cbObligado" runat="server" Checked="True"
                                Text="OBLIGADO A CONTABILIDAD" Visible="False" />
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-md-6">
                            MATRIZ:
                            <br />
                            <asp:TextBox ID="tbDirMatriz" runat="server" CssClass="form-control" placeholder="DIRECCIÓN *" TextMode="MultiLine"
                                 Height="70px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbDirMatriz"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                     </div>
                     <div class="col-md-6">
                            SUCURSAL:
                            <asp:TextBox ID="tbDirEstablecimiento" runat="server" Height="70px"
                                TextMode="MultiLine"  CssClass="form-control" placeholder="DIRECCIÓN *"></asp:TextBox>
                      </div>
                    </div>

                <asp:SqlDataSource ID="SqlDataPtoEmision" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT noEmpleado, idEmpleado FROM Cat_Empleados WHERE (idEmpleado = @idEmpleado)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idEmpleado" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSucursal" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT clave, sucursal FROM Cat_Sucursales WHERE (idSucursal = @idSucursal)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idSucursal" SessionField="sucursalUser" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataDocumento" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Cat_Catalogo1_C WHERE (tipo = 'Comprobante') AND (codigo = '01')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataEmision" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Cat_Catalogo1_C WHERE (tipo = 'Emision')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataAmbiente" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT codigo, descripcion, tipo FROM Cat_Catalogo1_C WHERE (tipo = 'Ambiente')">
                </asp:SqlDataSource>
            </asp:WizardStep>

            <asp:WizardStep runat="server" Title="Detalles" ID="infoDetalles" EnableTheming="True">
                <div Width="100%"><h4>Detalles</h4></div>



        <div align="center">
                <br />
               <asp:LinkButton ID="bAgregarImpuesto"  data-toggle="modal" data-target="#AgregarC" CssClass="btn btn-info" runat="server"
                    Text="Agregar" >Agregar</asp:LinkButton>






<%--------------Modal Inicio--%>
            <div class="modal fade " id="AgregarC">
  <div class="modal-dialog modal-lg">
    <div class="modal-content ">
      <div class="modal-header ">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
        <h4 class="modal-title ">Detalles</h4>
      </div>
      <div class="modal-body ">
       &nbsp;<div class="row">
<script >
    $(document.body).on('hidden.bs.modal', function () {
        $('#FiltrosC2').removeData('bs.modal')
    });

</script>

    <%--<div style="width: 12%; display: inline;  float: left; margin-left:2px " >--%>
     <%--------------Inicio Contenido Modal--%>

          <%-- <h5>Detalles:</h5>--%>
         <%--<input type="text" class="form-control" id="tbFolioAnterior" runat="server" ValidationGroup="Folio" placeholder="000000001 *"  required>--%>


            <div >
<asp:Label ID="lbl_cantidad" runat="server" Text="CANTIDAD:" ></asp:Label>
                     <asp:TextBox ID="tbCantidad" runat="server" AutoPostBack="True"
                         CausesValidation="True"  OnTextChanged="tbCantidad_TextChanged"
                          ValidationGroup="Deta" CssClass="form-control" placeholder="00.00 *" ToolTip="Cantidad">1</asp:TextBox>
                     <asp:FilteredTextBoxExtender ID="tbCantidad_FilteredTextBoxExtender"
                         runat="server" FilterType="Custom, Numbers"
                         TargetControlID="tbCantidad" ValidChars=",." BehaviorID="Wizard1_tbCantidad_FilteredTextBoxExtender">
                     </asp:FilteredTextBoxExtender>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator16"
                         runat="server" ControlToValidate="tbCantidad" CssClass="style20"
                         ErrorMessage="*" SetFocusOnError="True"
                         ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                         ValidationGroup="Deta" style="color: #FF3300"></asp:RegularExpressionValidator>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server"
                         ControlToValidate="tbCantidad" Display="None"
                         ErrorMessage="Debe ingresar valor." ForeColor="Red" SetFocusOnError="True"
                         ValidationGroup="Deta"></asp:RequiredFieldValidator>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                         ControlToValidate="tbCantidad" Display="None"
                         ErrorMessage="Debe ser mayor a 0." ForeColor="Red" InitialValue="0"
                         SetFocusOnError="True" ValidationGroup="Deta"></asp:RequiredFieldValidator>

  <asp:TextBox ID="tbCodigoA0" runat="server" Height="80px" TextMode="MultiLine"
                         ValidationGroup="Deta" CssClass="form-control" placeholder="DESCRIPCIÓN DEL PRODUCTO *" ToolTip="Descripción del Producto"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server"
                         ControlToValidate="tbCodigoA0" CssClass="style20" ErrorMessage="*"
                         ValidationGroup="Deta" style="color: #FF3300"></asp:RequiredFieldValidator>


  <asp:Label ID="lbl_precio" runat="server"  Text="PRECIO:"></asp:Label>
                     <asp:TextBox ID="tbPU" runat="server"
                         CausesValidation="True"
                         Style="text-align: right" ValidationGroup="Deta"  CssClass="form-control" placeholder="00.00 *" ToolTip="Descripción">0</asp:TextBox>
                     <asp:FilteredTextBoxExtender ID="tbPU_FilteredTextBoxExtender" runat="server" FilterType="Custom, Numbers" TargetControlID="tbPU"
                         ValidChars=",." BehaviorID="Wizard1_tbPU_FilteredTextBoxExtender">
                     </asp:FilteredTextBoxExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                         ControlToValidate="tbPU" CssClass="style20" ErrorMessage="*"
                         SetFocusOnError="True" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                         ValidationGroup="Deta" style="color: #FF3300"></asp:RegularExpressionValidator>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                         ControlToValidate="tbPU" Display="None" ErrorMessage="Debe ser mayor a 0."
                         ForeColor="Red" InitialValue="0" SetFocusOnError="True" ValidationGroup="Deta"></asp:RequiredFieldValidator>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                         ControlToValidate="tbPU" Display="None" ErrorMessage="Debe ingresar valor."
                         ForeColor="Red" SetFocusOnError="True" ValidationGroup="Deta"></asp:RequiredFieldValidator>

 <asp:Label ID="lbl_descuento" runat="server" Text="DESCUENTO" ></asp:Label>
                     <asp:TextBox ID="tbDescuento" runat="server" AutoPostBack="True"
                         CausesValidation="True" OnTextChanged="tbDescuento_TextChanged"
                         Style="text-align: right;" ValidationGroup="Deta" CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                     <asp:FilteredTextBoxExtender ID="tbDescuento_FilteredTextBoxExtender"
                         runat="server" FilterType="Custom, Numbers"
                         TargetControlID="tbDescuento" ValidChars=",." BehaviorID="Wizard1_tbDescuento_FilteredTextBoxExtender">
                     </asp:FilteredTextBoxExtender>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                         ControlToValidate="tbDescuento" CssClass="style20" ErrorMessage="*"
                         SetFocusOnError="True" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                         ValidationGroup="Deta" style="color: #FF3300"></asp:RegularExpressionValidator>

                         <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbDescuento"
                         ErrorMessage="Debe ingresar valor." ForeColor="Red" ValidationGroup="Deta"
                         InitialValue="" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>

   <asp:Label ID="lbl_iva" runat="server"  Text="IVA:"></asp:Label>
                     <asp:DropDownList ID="ddlTasaIVA" runat="server" AutoPostBack="True"
                         OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                          CssClass="form-control">
                         <asp:ListItem>12</asp:ListItem>
                         <asp:ListItem>0</asp:ListItem>
                         <asp:ListItem>6</asp:ListItem>
                     </asp:DropDownList>

<asp:Label ID="lbl_iva0" runat="server"  Text="IMPORTE:" Visible="False"></asp:Label>
                     <asp:TextBox ID="tbImporteConcepto" runat="server" CausesValidation="True"
                         ReadOnly="True" Style="text-align: right" ValidationGroup="Deta"
                         CssClass="form-control" placeholder="00.00 *" Visible="False">0</asp:TextBox>


</div>

     <%--------------Fin Contenido Modal--%>
    <%--</div>--%>

        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                         <asp:LinkButton ID="LinkButton4"  data-toggle="modal" data-target="#AgregarC" CssClass="btn btn-info" runat="server" onclick="bAgregarImpuesto_Click" ValidationGroup="Deta"
                    Text="Guardar" >Guardar</asp:LinkButton>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->










                <asp:Label ID="lMsjImpuestos" runat="server" Style="color: #FF0000"></asp:Label>
                <br />
                <asp:Label ID="lMsjDetalles" runat="server" Style="color: #FF0000"></asp:Label>
                <br />
            </div>
                <asp:GridView ID="gvDetalles" runat="server" AutoGenerateColumns="False"  CssClass="table table-hover"
                    DataSourceID="SqlDataDetalles" DataKeyNames="codigoPrincipal" GridLines="Horizontal" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="cantidad" HeaderText="CANT." SortExpression="cantidad" ItemStyle-Width="10%" >
<ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" SortExpression="descripcion" ItemStyle-Width="40%" >
<ItemStyle Width="40%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="precioUnitario" HeaderText="PRECIO" SortExpression="precioUnitario" ItemStyle-Width="20%" >
<ItemStyle Width="20%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="descuento" HeaderText="DESC." SortExpression="descuento" ItemStyle-Width="10%" >
<ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="precioTotalSinImpuestos" HeaderText="TOTAL" SortExpression="precioTotalSinImpuestos" ItemStyle-Width="20%" >
<ItemStyle Width="20%"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                                   <EmptyDataRowStyle CssClass="table" />
                <EmptyDataTemplate>
                    No existen datos.
                </EmptyDataTemplate>
                <HeaderStyle CssClass="info"  />
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
                <asp:GridView ID="gvImpuestosDetalles" runat="server" AutoGenerateColumns="False"
                   DataSourceID="SqlDataImpuestosConceptos" DataKeyNames="idImpuestosDetallesTemp"
                  GridLines="Horizontal"   CssClass="table table-hover" Visible ="false" >
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
                                   <EmptyDataRowStyle CssClass="table" />
                <EmptyDataTemplate>
                    No existen datos.
                </EmptyDataTemplate>
                <FooterStyle   />
                <HeaderStyle CssClass="btn-info"  />
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
                <asp:GridView ID="gvDetAdic" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataDetAdicionalesDetalles"
                    GridLines="Horizontal"   CssClass="table table-hover" DataKeyNames="idDetallesAdicionalesTemp" Visible="false">
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
                                   <EmptyDataRowStyle CssClass="table" />
                <EmptyDataTemplate>
                    No existen datos.
                </EmptyDataTemplate>
                <FooterStyle   />
                <HeaderStyle CssClass="success"  />
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
                <asp:SqlDataSource ID="SqlDataDetalles" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    DeleteCommand="PA_eliminar_DetallesTemp" SelectCommand="SELECT cantidad, codigoPrincipal, codigoAuxiliar, descripcion, precioUnitario, descuento, precioTotalSinImpuestos, id_Empleado from Dat_detallesTemp WHERE (id_Empleado = @idUser)"
                    DeleteCommandType="StoredProcedure">
                    <DeleteParameters>
                        <asp:Parameter Name="codigoPrincipal" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataImpuestosConceptos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT idImpuestosDetallesTemp,codigo, codigoPorcentaje, tarifa, baseImponible, valor, id_Empleado, codigoTemp FROM Dat_ImpuestosDetallesTemp WHERE (id_Empleado = @idUser) AND (codigoTemp = @codigoTemp)"
                    DeleteCommand="DELETE FROM Dat_ImpuestosDetallesTemp WHERE (idDat_ImpuestosDetallesTemp = @idDat_ImpuestosDetallesTemp)">
                    <DeleteParameters>
                        <asp:Parameter Name="idDat_ImpuestosDetallesTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                        <asp:ControlParameter ControlID="tbCodigoP" Name="codigoTemp" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataDetAdicionalesDetalles" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT idDetallesAdicionalesTemp,nombre, valor, codigoTemp, id_Empleado FROM Dat_DetallesAdicionalesTemp WHERE (codigoTemp = @codigoTemp) AND (id_Empleado = @idUser)"
                    DeleteCommand="DELETE FROM Dat_DetallesAdicionalesTemp WHERE (idDetallesAdicionalesTemp = @idDetallesAdicionalesTemp)">
                    <DeleteParameters>
                        <asp:Parameter Name="idDetallesAdicionalesTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="tbCodigoP" Name="codigoTemp" PropertyName="Text" />
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Documento" ID="infoDocumentos">
                    <h4>Información Comprobante</h4>
          <div class="row">
            <div class="col-md-6">
                            <asp:Label ID="lFechaEmision" runat="server" Text="FECHA DE EMISIÓN:" ></asp:Label>
                            <asp:TextBox ID="tbFechaEmision" runat="server" ReadOnly="True"
                                CssClass="form-control" placeholder="yyyy-MM-ddTHH:mm:ss *"></asp:TextBox>
                            <asp:Label ID="lidentificacionComprador" runat="server"
                                Text="IDENTIFICACIÓN DEL COMPRADOR:" CssClass="style29"></asp:Label>
                            <asp:TextBox  ID="tbIdentificacionComprador" runat="server" AutoPostBack="True"
                                 MaxLength="13" OnTextChanged="tbIdentificacionComprador_TextChanged"
                                ValidationGroup="Receptor"  CssClass="form-control" placeholder="00000000000001 *"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbIdentificacionComprador_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Custom, Numbers"
                                TargetControlID="tbIdentificacionComprador">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="tbIC_AutoCompleteExtender" runat="server"
                                CompletionInterval="10" CompletionListCssClass="CompletionListCssClass"
                                CompletionSetCount="12" DelimiterCharacters="" Enabled="True"
                                ServiceMethod="getRuc" ServicePath="../autoRuc.asmx"
                                ShowOnlyCurrentWordInCompletionListItem="True"
                                TargetControlID="tbIdentificacionComprador" UseContextKey="True">
                            </asp:AutoCompleteExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                ControlToValidate="tbIdentificacionComprador" CssClass="style20"
                                ErrorMessage="Ingresa el RUC" ValidationGroup="Receptor"
                                style="color: #FF3300; font-size: x-small;"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13"
                                runat="server" ControlToValidate="tbIdentificacionComprador" CssClass="style20"
                                ErrorMessage="RUC Invalido" ValidationExpression="^[0-9a-zA-Z]{0,13}?$"
                                ValidationGroup="Receptor" style="color: #FF3300; font-size: x-small;"></asp:RegularExpressionValidator>
                            <asp:DropDownList ID="ddlTipoIdentificacion" runat="server"
                                AppendDataBoundItems="True" DataSourceID="SqlDataTipoIdentificacion"
                                DataTextField="descripcion" DataValueField="codigo" CssClass="form-control">
                                <asp:ListItem>Selecciona el Tipo</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lRazonSocialComprador" runat="server"
                                Text="RAZÓN SOCIAL DEL COMPRADOR:" CssClass="style29"></asp:Label>
                            <asp:TextBox ID="tbRazonSocialComprador" runat="server"
                                TextMode="MultiLine" CssClass="form-control" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                ControlToValidate="tbRazonSocialComprador" ErrorMessage="*"
                                Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                            <asp:Label ID="lbl_dir_cli" runat="server" Text="DOMICILIO:" CssClass="style29"
                                Visible="False"></asp:Label>
                            <asp:TextBox ID="txt_dir_cli" runat="server" TextMode="MultiLine"
                                Width="291px" Visible="False" CssClass="form-control" placeholder="00.00 *"></asp:TextBox>

                                <span>E-Mail:</span>
                                <asp:TextBox ID="tbEmail" runat="server"  CssClass="form-control" placeholder="email1@email.com,email2@email.com *"></asp:TextBox>
                                <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server"
                                        ControlToValidate="tbEmail" ErrorMessage="El formato de E-Mail no es válido" ForeColor="Red"
                                        ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$"
                                        ValidationGroup="email"></asp:RegularExpressionValidator>
            </div>
            <div class="col-md-3">
                            <asp:Label ID="lSubtotal12" runat="server" Text="SUBTOTAL 12%:" ></asp:Label>
                            <asp:TextBox ID="tbSubtotal12" runat="server" ReadOnly="True"
                                Style="text-align: right" CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                            <asp:Label ID="lSubtotal0" runat="server" Text="SUBTOTAL 0%:"></asp:Label>
                            <asp:TextBox ID="tbSubtotal0" runat="server" ReadOnly="True"
                                Style="text-align: right" CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                            <asp:Label ID="lSubtotalNoSujeto" runat="server"
                                Text="SUBTOTAL No sujeto de IVA:"
                                ></asp:Label>
                            <asp:TextBox ID="tbSubtotalNoSujeto" runat="server"
                                ReadOnly="True" Style="text-align: right"  CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                            <asp:Label ID="lTotalSinImpuestos" runat="server"
                                Text="SUBTOTAL SIN IMPUESTOS:"
                                ></asp:Label>
                            <asp:TextBox ID="tbTotalSinImpuestos" runat="server"
                                ReadOnly="True" Style="text-align: right"  CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                            <asp:Label ID="lTotalDescuento" runat="server" Text="DESCUENTO:"
                                 ></asp:Label>
                            <asp:TextBox ID="tbTotalDescuento" runat="server" ReadOnly="True"
                                Style="text-align: right"  CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                            <asp:Label ID="lIVA12" runat="server" Text="IVA 12%:" CssClass="style25"
                                ></asp:Label>
                            <asp:TextBox ID="tbIVA12" runat="server" ReadOnly="True"
                                Style="text-align: right"  CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                            <asp:Label ID="lImporteTotal" runat="server" Text="IMPORTE TOTAL:"
                                ></asp:Label>
                            <asp:TextBox ID="tbImporteTotal" runat="server" ReadOnly="True"
                                Style="text-align: right"  CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                            <asp:Label ID="lPropinas" runat="server" Text="PROPINAS:" Visible="False"
                                ></asp:Label>
                            <asp:TextBox ID="tbPropinas" runat="server" AutoPostBack="True"
                                OnTextChanged="tbPropinas_TextChanged" Style="text-align: right"
                                Visible="False"  CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
                            <asp:Label ID="lImporteaPagar" runat="server" Text="IMPORTE A PAGAR:"
                                ></asp:Label>
                            <asp:TextBox ID="tbImporteaPagar" runat="server" ReadOnly="True"
                                Style="text-align: right"  CssClass="form-control" placeholder="00.00 *">0</asp:TextBox>
            </div>
          </div>
                <asp:SqlDataSource ID="SqlDataTipoIdentificacion" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Cat_Catalogo1_C WHERE (tipo = 'Identificacion')">
                </asp:SqlDataSource>
                <asp:Label ID="lMsjDocumento" runat="server" Style="color: #FF0000"></asp:Label>
            </asp:WizardStep>
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
        </WizardSteps>
    </asp:Wizard>
    <%--  Conceptos --%>
</div>
                                    <asp:TextBox runat="server"
        ValidationGroup="Deta" Height="19px"
        ID="tbCodigoP" Visible="False"></asp:TextBox>


         <asp:TextBox runat="server" MaxLength="5"
        ValidationGroup="Imp"  Height="19px" Width="60px"
        ID="tbCodigoIDP" Visible="False"></asp:TextBox>


                                    <asp:TextBox runat="server" AutoPostBack="True"
        MaxLength="4" ValidationGroup="Imp" CssClass="txt_gr2" Height="19px"
        Width="60px" ID="tbTarifa" Visible="False" OnTextChanged="tbTarifa_TextChanged"></asp:TextBox>

                                    <asp:TextBox runat="server" AutoPostBack="True"
        MaxLength="14" ReadOnly="True" ValidationGroup="Imp" CssClass="txt_gr2"
        Height="30px" Width="70px" ID="tbBaseImponible" Visible="False"></asp:TextBox>


                                    <asp:TextBox runat="server" MaxLength="1"
        ValidationGroup="Imp" CssClass="txt_gr2" Height="19px" Width="20px"
        ID="tbCodigoID" Visible="False"></asp:TextBox>

                                    <asp:DropDownList runat="server"
        AppendDataBoundItems="True" AutoPostBack="True" DataTextField="descripcion"
        DataValueField="codigo" DataSourceID="SqlDataCatImpuestos" ID="ddlTipoImpuesto"
        Visible="False" OnSelectedIndexChanged="ddlTipoImpuesto_SelectedIndexChanged"><asp:ListItem Text="IVA"
                                            Selected="True" Value="2"></asp:ListItem>
</asp:DropDownList>

                                    <asp:TextBox runat="server" MaxLength="14"
        ReadOnly="True" ValidationGroup="Imp" Height="19px" Width="80px" ID="tbValor"
        Visible="False"></asp:TextBox>
<%--  Conceptos --%>
                                <br />
    <asp:Label runat="server" Text="GU&#205;A DE REMISI&#211;N:" ID="lGuiaRemision"
        Visible="False"></asp:Label>
    <asp:TextBox runat="server" Height="19px" Width="185px" ID="tbGuiaRemision"
        Visible="False"></asp:TextBox>
    <asp:Label runat="server" Text="IDENTIFICACI&#211;N DEL COMPRADOR"
        CssClass="style5" ID="lTipoIdentificacionComprador" Visible="False"></asp:Label>
    <asp:Label runat="server" Text="ICE:" ID="lICE" Visible="False"></asp:Label>
    <asp:TextBox runat="server" ReadOnly="True" Height="19px" Width="150px"
        ID="tbICE" Visible="False" Style="text-align: right">0</asp:TextBox>
    <asp:TextBox runat="server" MaxLength="15" Height="19px" Width="70px"
        ID="tbMoneda" Visible="False">DOLAR</asp:TextBox>
                                <asp:SqlDataSource runat="server"
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT [descripcion], [codigo] FROM [Cat_CatImpuestos_C] WHERE ([tipo] = @tipo)"
        ID="SqlDataCatImpuestos">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="Impuesto" Name="tipo" Type="String">
                                        </asp:Parameter>
                                    </SelectParameters>
    </asp:SqlDataSource>
                                <br />


                                </asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="cpTitulo">
    Factura
</asp:Content>

