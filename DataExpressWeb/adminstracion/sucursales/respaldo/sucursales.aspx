<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="sucursales.aspx.cs" Inherits="Administracion.sucursales" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <script  type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
	  </script>

    <div class="col-md-4">
        <h5>Clave:</h5>
            <asp:TextBox ID="tbClave" runat="server"  CssClass="form-control" placeholder="001 *" ></asp:TextBox>
      </div>  <div class="col-md-4"> <h5>Establecimiento:</h5>
            <asp:TextBox ID="tbSucursal" runat="server"   CssClass="form-control" placeholder="001 *" ></asp:TextBox>
      </div> <div class="col-md-4"> <h5>Domicilio:</h5>
            <asp:TextBox ID="tbDomicilio" runat="server"  CssClass="form-control" placeholder="Domicilio *" ></asp:TextBox>
        </div>
        <div >
            <br /><br /><br /><br />
            <asp:Button ID="bBuscarReg" runat="server" onclick="bBuscarReg_Click" CssClass="btn btn-primary" Text="Buscar" />
            <asp:Button ID="bActualizar" runat="server" onclick="bActualizar_Click" CssClass="btn btn-primary" Text="Nueva Busqueda" />
         <asp:HyperLink ID="HyperLink8" runat="server" CssClass="btn btn-primary"  NavigateUrl="~/adminstracion/sucursales/agregarSucursal.aspx">Agregar</asp:HyperLink>
        </div>    
        <asp:Label ID="lMensaje" runat="server" style="color: #FF0000"></asp:Label>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpCuerpoInf" runat="server">
    <div align="center">

    <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False"
        DataKeyNames="idSucursal" DataSourceID="SqlDataSource1" GridLines="Horizontal"
        AllowPaging="True" OnSelectedIndexChanged="grid_sucursales_SelectedIndexChanged"
        OnPageIndexChanged="grid_sucursales_PageIndexChanged" CssClass="table table-hover">
        <Columns>
           <%-- <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/imagenes/icono_adelante.gif">
            </asp:CommandField>--%>
            <asp:BoundField DataField="clave" HeaderText="CLAVE">
            </asp:BoundField>
            <asp:BoundField DataField="sucursal" HeaderText="ESTABLECIMIENTO" SortExpression="sucursal" />
            <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" SortExpression="domicilio" />
            <asp:TemplateField ShowHeader="False" HeaderText="ACCIÓN">
                <ItemTemplate>
                    <a href='modificarSucursal.aspx?id=<%# Eval("idSucursal") %>'>Editar</a>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Si eliminas el Establecimiento se borraran todos los datos que contengan. ¿Desea eliminar el registro?');"
                        CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle  CssClass="info"/>
        <FooterStyle />
        <PagerStyle  />
        <RowStyle  />
        <SelectedRowStyle  />
        <SortedAscendingCellStyle  />
        <SortedAscendingHeaderStyle  />
        <SortedDescendingCellStyle  />
        <SortedDescendingHeaderStyle  />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="PA_Busqueda_Sucursales" DeleteCommand="UPDATE Cat_Sucursales SET eliminado='True' WHERE (idSucursal = @idSucursal) UPDATE Cat_Empleados SET eliminado='true' WHERE (id_Sucursal=@idSucursal)"
        SelectCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="idSucursal" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
    <br />
    <asp:Panel ID="Panel_cajaSucursal" Style="padding-top: 10px" runat="server" Width="100%">
        <asp:Label ID="Label1" runat="server" Text="Administrador de Cajas por Sucursal"
            ForeColor="#008834" CssClass="titulo_pagina"></asp:Label>
        
        <asp:GridView ID="gv_cajaSucursal" runat="server" AutoGenerateColumns="False" DataKeyNames="idCaja"
            DataSourceID="ds_CajaSucursal" OnDataBinding="grid_cajaSucursal_DataBinding"
            BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px"
            CellPadding="4" GridLines="Horizontal" Width="70%" ShowFooter="True" 
            ShowHeaderWhenEmpty="True" 
            onselectedindexchanged="gv_cajaSucursal_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField HeaderText="Caja Sisposnet" SortExpression="NumeroSisposnet" Visible=false>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_i_numsispo" runat="server" ValidationGroup="gvinsert" Width="50px" onkeypress="return isNumberKey(event)"
                            MaxLength="2" Visible="False" CssClass="cajaEditGrid" Height="17px" CausesValidation="True"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="i_RequiredFieldValidator1" runat="server" ControlToValidate="txt_i_numsispo" ValidationGroup="gvinsert" Text="*"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("NumeroSisposnet") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtnumsis" runat="server" Text='<%# Bind("NumeroSisposnet") %>' 
                            Width="50px" Height="17px" CssClass="cajaEditGrid" onkeypress="return isNumberKey(event)" MaxLength="2" ValidationGroup="gvupdate" CausesValidation="True"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="u_RequiredFieldValidator1" runat="server" ControlToValidate="txtnumsis" ValidationGroup="gvupdate" Text="*"></asp:RequiredFieldValidator>
                       
                    </EditItemTemplate>
                    <HeaderStyle CssClass="p_gridview_h" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo de Documento" SortExpression="NumeroRentas">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("NumeroRentas") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                       <asp:DropDownList ID="ddlNumeroRentas" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("NumeroRentas") %>'
                            Height="17px">
                            <asp:ListItem Selected="True" Value="01">Factura</asp:ListItem>
                            <asp:ListItem Value="04">Nota de Crédito</asp:ListItem>
                            <asp:ListItem Value="05">Nota de Débito</asp:ListItem>
                            <asp:ListItem Value="06">Guía de Remisión</asp:ListItem>
                            <asp:ListItem Value="07">Comprobante de Retención</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                    <asp:DropDownList ID="ddl_i_nrentas" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("NumeroRentas") %>'
                            Height="17px" Visible="False">
                            <asp:ListItem Selected="True" Value="01">Factura</asp:ListItem>
                            <asp:ListItem Value="04">Nota de Crédito</asp:ListItem>
                            <asp:ListItem Value="05">Nota de Débito</asp:ListItem>
                            <asp:ListItem Value="06">Guía de Remisión</asp:ListItem>
                            <asp:ListItem Value="07">Comprobante de Retención</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Establecimiento" SortExpression="estab">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("estab") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtestab" runat="server" Text='<%# Bind("estab") %>' CssClass="cajaEditGrid" onkeypress="return isNumberKey(event)"
                            Width="50px" Height="17px" ValidationGroup="gvupdate" MaxLength="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="u_RequiredFieldValidator3" runat="server" ControlToValidate="txtestab" ValidationGroup="gvupdate" Text="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_i_estab" runat="server" MaxLength="3" ValidationGroup="gvinsert" onkeypress="return isNumberKey(event)"
                            Width="50px" Height="17px" Visible="False" CssClass="cajaEditGrid" CausesValidation="True"></asp:TextBox>
                       <asp:RequiredFieldValidator ID="i_RequiredFieldValidator3" runat="server" ControlToValidate="txt_i_estab" ValidationGroup="gvinsert" Text="*"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Punto Emisión" SortExpression="ptoEmi">
                    <FooterTemplate>
                        <asp:TextBox ID="txt_i_ptoemi" runat="server" MaxLength="3" Width="50px" Height="17px" onkeypress="return isNumberKey(event)"
                            Visible="False" CssClass="cajaEditGrid"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="i_RequiredFieldValidator4" runat="server" ControlToValidate="txt_i_ptoemi" ValidationGroup="gvinsert" Text="*"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ptoEmi") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtptoEmi" runat="server" Text='<%# Bind("ptoEmi") %>' CssClass="cajaEditGrid" onkeypress="return isNumberKey(event)"
                            Width="50px" Height="17px" CausesValidation="True" ValidationGroup="gvupdate" MaxLength="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="u_RequiredFieldValidator4" runat="server" ControlToValidate="txtptoEmi" ValidationGroup="gvupdate" Text="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Secuencial" SortExpression="secuencial" Visible="false">
                    <FooterTemplate>
                        <asp:TextBox ID="txt_i_secuencial" runat="server" MaxLength="9" Width="80px" Height="17px" onkeypress="return isNumberKey(event)"
                            Visible="False" CssClass="cajaEditGrid" CausesValidation="True"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="i_RequiredFieldValidator5" runat="server" ControlToValidate="txt_i_secuencial" ValidationGroup="gvinsert" Text="*"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("secuencial") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtsecuencial" runat="server" Text='<%# Bind("secuencial") %>' CssClass="cajaEditGrid" onkeypress="return isNumberKey(event)"
                            Width="80px" Height="17px" AutoPostBack="True" ValidationGroup="gvupdate" MaxLength="9"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="u_RequiredFieldValidator5" runat="server" ControlToValidate="txtsecuencial" ValidationGroup="gvupdate" Text="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado" SortExpression="estado" Visible="false">
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_i_estado" runat="server" Visible="False" Height="17px"
                            CssClass="cajaEditGrid">
                            <asp:ListItem Selected="True" Value="A">Activo</asp:ListItem>
                            <asp:ListItem Value="I">Inactivo</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("estado") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlestado" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("estado") %>'
                            Height="17px">
                            <asp:ListItem Selected="True" Value="A">Activo</asp:ListItem>
                            <asp:ListItem Value="I">Inactivo</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Factura Electrónica" SortExpression="estadoFE" Visible="false">
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_i_estadofe" runat="server" ValidationGroup="insert" Visible="False"
                            Height="17px" CssClass="cajaEditGrid">
                            <asp:ListItem Value="A">Activo</asp:ListItem>
                            <asp:ListItem Selected="True" Value="I">Inactivo</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("estadoFE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlestadofe" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("estadoFE") %>'
                            Height="17px">
                            <asp:ListItem Value="A">Activo</asp:ListItem>
                            <asp:ListItem Selected="True" Value="I">Inactivo</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Emisión" SortExpression="estadoPro">
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("estadoPro") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlestadopro" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("estadoPro") %>'
                            Height="17px">
                            <asp:ListItem Selected="True" Value="1">Pruebas</asp:ListItem>
                            <asp:ListItem Value="2">Producción</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_i_estadopro" runat="server" ValidationGroup="gvinsert"
                            Visible="False" Height="17px" CssClass="cajaEditGrid">
                            <asp:ListItem Selected="True" Value="1">Pruebas</asp:ListItem>
                            <asp:ListItem Value="2">Producción</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <FooterTemplate>
                        <asp:LinkButton ID="lbnuevo" runat="server" CausesValidation="False" OnClick="lbnuevo_Click">Nuevo</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lbcancel" runat="server" CausesValidation="False" OnClick="lbcancel_Click"
                            Visible="False">Cancelar</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lbinsert" runat="server" OnClick="lbinsert_Click" ValidationGroup="gvinsert"
                            Visible="False">Insertar</asp:LinkButton>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                            Text="Editar"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lbactualizar" runat="server" CausesValidation="True" CommandName="Update" ValidationGroup="gvupdate"
                            Text="Actualizar"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                            Text="Cancelar"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle CssClass="cajaEditGrid" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                &nbsp;<asp:Button ID="btnCajas" runat="server" Text="Agregar Cajas" OnClick="btnCajas_Click" />
            </EmptyDataTemplate>
            <FooterStyle CssClass="cajaEditGrid" HorizontalAlign="Center" />
            <HeaderStyle CssClass="p_gridview_h" />
            <RowStyle HorizontalAlign="Center" />
        </asp:GridView>
        <asp:SqlDataSource ID="ds_CajaSucursal" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
            DeleteCommand="UPDATE CajaSucursal SET estado = 'I', estadoFE = 'I', estadoPro = '1' WHERE (idCaja = @idCaja)"
            InsertCommand="INSERT INTO CajaSucursal(idSucursal, NumeroSisposnet, NumeroRentas, estab, ptoEmi, secuencial, estado, estadoFE, estadoPro) VALUES (@idSucursal, @NumeroSisposnet, @NumeroRentas, @estab, @ptoEmi, @secuencial, @estado, @estadoFE, @estadoPro)"
            SelectCommand="SELECT idCaja, idSucursal, NumeroSisposnet, NumeroRentas, estab, ptoEmi, secuencial, estado, estadoFE, estadoPro FROM CajaSucursal WHERE (idSucursal = @idSucursal)"
            UpdateCommand="UPDATE CajaSucursal SET NumeroSisposnet = @NumeroSisposnet, NumeroRentas = @NumeroRentas, estab = @estab, ptoEmi = @ptoEmi, secuencial = @secuencial, estado = @estado, estadoFE = @estadoFE, estadoPro = @estadoPro WHERE (idCaja = @idCaja)">
            <DeleteParameters>
                <asp:Parameter Name="idCaja" />
            </DeleteParameters>
            <InsertParameters>
                <asp:ControlParameter ControlID="gvSucursales" Name="idSucursal" PropertyName="SelectedValue" />
            </InsertParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="gvSucursales" Name="idSucursal" PropertyName="SelectedValue" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="NumeroSisposnet" />
                <asp:Parameter Name="NumeroRentas" />
                <asp:Parameter Name="estab" />
                <asp:Parameter Name="ptoEmi" />
                <asp:Parameter Name="secuencial" />
                <asp:Parameter Name="estado" />
                <asp:Parameter Name="estadoFE" />
                <asp:Parameter Name="estadoPro" />
                <asp:Parameter Name="idCaja" />
            </UpdateParameters>
        </asp:SqlDataSource>

    </asp:Panel>
        </div>
</asp:Content>
<asp:Content ID="Content4" runat="server" contentplaceholderid="cpTitulo">
    ESTABLECIMIENTOS 
</asp:Content>