<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="editarComprobante.aspx.cs" Inherits="DataExpressWeb.EditarComprobante" %>

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

        .panel {
            background: rgba(0,0,0,0) !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div id="divBusqueda" class="rowsSpaced" runat="server" visible="true">
                <div class="row">
                    <div class="col-md-1">EMISOR:</div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlRFC" runat="server" Style="text-align: center" CssClass="form-control" DataSourceID="SqlDataRFC" DataTextField="RFCEMI" DataValueField="IDEEMI">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataRFC" runat="server" SelectCommand="SELECT IDEEMI,RFCEMI FROM Cat_Emisor"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-1">SERIE:</div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbSerie" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                    </div>
                    <div class="col-md-1">FOLIO:</div>
                    <div class="col-md-3">
                        <asp:TextBox ID="tbFolio" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="lbBuscar" runat="server" OnClick="lbBuscar_Click" CssClass="btn btn-primary" CausesValidation="false">Buscar</asp:LinkButton>
                    </div>
                    <div class="col-md-5"></div>
                </div>
            </div>
            <div id="divDatos" runat="server" visible="false">
                <br />
                <div class="alert alert-danger" runat="server" id="alertSat">
                    <asp:Label ID="lblMensajeSAT" runat="server" Text=""></asp:Label>
                </div>
                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-default">
                        <div class="panel-heading" role="tab" id="heading2">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse2" aria-expanded="true" aria-controls="collapse2">DATOS DEL COMPROBANTE</a>
                            </h4>
                        </div>
                        <div id="collapse2" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading2">
                            <div class="panel-body rowsSpaced">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:GridView ID="gvConceptos" runat="server" AutoGenerateColumns="False" class="table table-condensed table-responsive table-hover" OnPageIndexChanging="gvConceptos_PageIndexChanging" PageSize="5" BackColor="White" AllowPaging="True" Font-Size="Small" GridLines="None" DataSourceID="SqlDataConcTemp" DataKeyNames="idDetalles">
                                            <Columns>
                                                <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD" />
                                                <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" />
                                                <asp:BoundField DataField="valorUnitario" HeaderText="VAL. UNITARIO" />
                                                <asp:BoundField DataField="importe" HeaderText="IMPORTE" />
                                                <asp:BoundField DataField="unidad" HeaderText="UNIDAD" />
                                            </Columns>
                                            <EmptyDataRowStyle CssClass="table empty" />
                                            <EmptyDataTemplate>
                                                No existen datos.
                                            </EmptyDataTemplate>
                                            <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataConcTemp" runat="server" SelectCommand="SELECT idDetalles, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad FROM Dat_Detalles WHERE (id_Comprobante = @idComprobante)">
                                            <SelectParameters>
                                                <asp:Parameter Name="idComprobante" DefaultValue="" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="col-md-4">SUBTOTAL:</div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="tbSubtotal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">IVA 16%:</div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="tbIva16" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div runat="server" id="trISProp" visible="false">
                                            <div class="col-md-4">
                                                ISH
                           
                                        <asp:Label ID="lblISHPrer" runat="server" Text="X"></asp:Label>%:
                                   
                                            </div>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="tbISH" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">TOTAL FAC.:</div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="tbTotalFac" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div id="rowPropina" runat="server" visible="false">
                                            <div class="col-md-4">PROPINA:</div>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="tbPropina" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div runat="server" id="divDescuentoTot" visible="true">
                                            <div class="col-md-4">DESCUENTO:</div>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="tbDescuento" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0 *" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div runat="server" id="trOtrosCargos" visible="false">
                                            <div class="col-md-4">OTROS C.:</div>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="tbOtrosCargos" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">A PAGAR:</div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="tbTotal" runat="server" CssClass="form-control" Style="text-align: center" placeholder="0.00 *" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1">DOCUMENTO:</div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="tbCodDoc" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">AMBIENTE:</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="tbAmbiente" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1">F PAGO:</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="tbFormaPago" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">M PAGO:</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="tbMetodoPago" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">CTA.:</div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="tbNoCta" runat="server" Style="text-align: center" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">CANTIDAD CON LETRA:</div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="tbCantLetra" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">EXPEDIDO EN:</div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="tbLugarExp" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">OBSERVACIONES:</div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="tbObservaciones" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading" role="tab" id="heading1">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse1" aria-expanded="true" aria-controls="collapse1">DATOS DEL RECEPTOR</a>
                            </h4>
                        </div>
                        <div id="collapse1" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading1">
                            <div class="panel-body rowsSpaced">
                                <div class="row">
                                    <div class="col-md-1">
                                        RFC:<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="tbRfcRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="tbRfcRec" runat="server" CssClass="form-control upper" MaxLength="13" Style="text-align: center" placeholder="AAA[A]999999XXX *" AutoPostBack="true" OnTextChanged="tbRfcRec_TextChanged"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="tbRfcRec_AutoCompleteExtender" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRfcRec" UseContextKey="True" MinimumPrefixLength="1">
                                        </asp:AutoCompleteExtender>
                                    </div>
                                    <div class="col-md-1">
                                        RAZÓN SOCIAL:<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tbRazonSocialRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptor"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="tbRazonSocialRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="RAZÓN SOCIAL *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="bBuscaReceptor" runat="server" CssClass="btn btn-primary btn-sm" data-target="#ModuloBC" data-toggle="modal" Text="Buscar Receptor"></asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row" id="rowDenomSocial" runat="server" visible="true">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-1">
                                        DENOM. SOCIAL:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="tbDenomSocialRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="DENOMINACIÓN SOCIAL *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1"></div>
                                    <div align="left" class="col-md-11">
                                        <asp:CheckBox ID="cbDomRec" runat="server" Text="HABILITAR" AutoPostBack="true" OnCheckedChanged="cbDomRec_CheckedChanged" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1">SUCURSAL:</div>
                                    <div class="col-md-11">
                                        <asp:DropDownList ID="ddlSucRec" runat="server" Style="text-align: center" CssClass="form-control" Enabled="False" AutoPostBack="true" OnSelectedIndexChanged="ddlSucRec_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1">
                                        CALLE:<asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tbCalleRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="tbCalleRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CALLE *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">NO. EXT.:</div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="tbNoExtRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. EXTERIOR *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">NO. INT.:</div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="tbNoIntRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="NO. INTERIOR *"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1">COLONIA:</div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="tbColoniaRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="COLONIA *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        MUNICIPIO:
                                                           
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tbMunicipioRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="tbMunicipioRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="MUNICIPIO *"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1">
                                        ESTADO:<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbEstadoRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-7">
                                        <asp:TextBox ID="tbEstadoRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="ESTADO *"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        C.P.:<asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="tbCpRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="tbCpRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="CODIGO POSTAL *" MaxLength="5"></asp:TextBox>
                                        <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" MaskType="Number" ClearMaskOnLostFocus="true" ClearTextOnInvalid="true" TargetControlID="tbCpRec" Mask="99999" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1">
                                        PAÍS:<asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="tbPaisRec" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="validationReceptorDom" Enabled="False"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-7">
                                        <div class="input-group">
                                            <asp:TextBox ID="tbPaisRec" runat="server" CssClass="form-control" Style="text-align: center" placeholder="PAÍS *"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" id="btnPaisRec" runat="server" disabled="disabled"><span class="caret"></span></button>
                                                <ul class="dropdown-menu dropdown-menu-right" role="menu" runat="server" id="ulPaisRec" style="height: 200px; overflow: hidden; overflow-y: scroll;">
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="bFinishButton" runat="server" OnClick="FinishButton_Click" ValidationGroup="validationReceptor" CssClass="btn btn-primary">Generar</asp:LinkButton>
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
    <div class="modal fade" role="dialog" aria-labelledby="myModalLabel" id="ModuloBC">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Buscar Clientes</h4>
                </div>
                <div class="modal-body rowsSpaced">
                    <asp:UpdatePanel ID="updClientes" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="Label12" runat="server" Text="Buscar por RFC:"></asp:Label>
                                    <asp:TextBox ID="tbRFCClienteBusqueda" runat="server" Style="text-align: center" CssClass="form-control upper"></asp:TextBox>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="Label13" runat="server" Text="Buscar por Razón Social:"></asp:Label>
                                    <asp:TextBox ID="tbRazonClienteBusqueda" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-inline">
                                <fieldset>
                                    <div class="form-group">
                                        <asp:LinkButton ID="bBuscarCliente" OnClick="bBuscarCliente_Click" runat="server" CssClass="btn btn-primary" ValidationGroup="validationRfc">Buscar</asp:LinkButton>
                                    </div>
                                    <div class="form-group">
                                        <asp:LinkButton ID="bLimpiarBusquedaCliente" OnClick="bLimpiarBusquedaCliente_Click" runat="server" CssClass="btn btn-primary">Limpia Búsqueda</asp:LinkButton>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="gvRec" runat="server" AutoGenerateColumns="False" CssClass=" table table-condensed table-responsive table-hover" PageSize="10" BackColor="White" Font-Size="X-Small" GridLines="None" DataKeyNames="IDEREC" DataSourceID="SqlDataReceptores" EmptyDataText="Sin Datos." AllowPaging="true" OnPageIndexChanging="gvRec_PageIndexChanging" RowStyle-Height="5px">
                                        <Columns>
                                            <asp:BoundField DataField="RFCREC" HeaderText="RFC" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:BoundField DataField="NOMREC" HeaderText="NOMBRE" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="bUsarRecep" Text="Usar" CssClass="btn btn-primary btn-xs" runat="server" OnClick="bUsarRecep_Click" CommandArgument='<%#Eval("IDEREC") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataReceptores" runat="server" SelectCommand="SELECT IDEREC ,RFCREC ,NOMREC ,(domicilio + ' Ext.' + noExterior + ' Int.' + noInterior + '. Col. ' + colonia + '. ' + localidad + ' Mun. ' + municipio + ', ' + estado + ', ' + pais + '. C.P.: ' + codigoPostal) AS domicilio FROM Cat_Receptor ORDER BY RFCREC"></asp:SqlDataSource>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="cpTitulo">
    EDITAR INFORMACIÓN DE COMPROBANTE NO AUTORIZADO
</asp:Content>
