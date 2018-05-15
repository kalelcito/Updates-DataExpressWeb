<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="reglas.aspx.cs" Inherits="DataExpressWeb.Distribucion" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <script type="text/javascript">
        function ReglaValidation() {
            if (!Page_ClientValidate()) {
                showDivOnModalLevel('modalValidation', 4);
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div align="center">
                <asp:Button ID="bNuevo" runat="server" Text="Agregar" CssClass="btn btn-primary" CausesValidation="false" OnClick="bNuevo_Click" />
            </div>
            <br />
            <asp:GridView ID="gvReglas" runat="server" AutoGenerateColumns="False" DataKeyNames="idEmailRegla" DataSourceID="reglasDataSource" CssClass="table table-responsive table-hover table-condensed" CellPadding="4" GridLines="None" BackColor="White" PageSize="15" AllowPaging="True" Font-Size="Small">
                <Columns>
                    <asp:BoundField DataField="nombreRegla" HeaderText="NOMBRE" SortExpression="nombreRegla" />
                    <asp:BoundField DataField="NOMREC" HeaderText="CLIENTE" SortExpression="NOMREC" />
                    <asp:CheckBoxField DataField="estadoRegla" HeaderText="ACTIVO" SortExpression="estadoRegla"></asp:CheckBoxField>
                    <asp:TemplateField HeaderText="ACCIÓN" ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="bDetalles" runat="server" CssClass="btn btn-primary" CommandArgument='<%# Bind("idEmailRegla") %>' CausesValidation="false" OnClick="bDetalles_Click">Detalles</asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false" CssClass="btn btn-primary" CommandName="Delete" Text="Eliminar" OnClientClick="return confirm('¿Desea eliminar el registro?');"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
            </asp:GridView>
            <asp:SqlDataSource ID="reglasDataSource" runat="server" SelectCommand="SELECT  er.idEmailRegla,er.nombreRegla,r.NOMREC,er.estadoRegla FROM Cat_EmailsReglas er INNER JOIN Cat_Receptor r ON er.Receptor=r.RFCREC and er.eliminado='False'" DeleteCommand="DELETE FROM Cat_EmailsReglas WHERE idEmailRegla= @idEmailRegla">
                <DeleteParameters>
                    <asp:Parameter Name="idEmailRegla" />
                </DeleteParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="modalValidation" class="modal-div">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <asp:Label ID="ValidationList" runat="server"></asp:Label>
    </div>
    <div id="divNuevo" class="modal fade" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content ">
                <div class="modal-header ">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title ">Agregar/Editar Regla</h4>
                </div>
                <div class="modal-body rowsSpaced">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-9"></div>
                                <div class="col-md-3">
                                    <asp:CheckBox ID="chkEditar" runat="server" Text="Editar" OnCheckedChanged="chkEditar_CheckedChanged" Visible="false" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-2">RFC:</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="tbRFC" runat="server" Style="text-align: center" placeholder="AAA[A]999999XXX *" MaxLength="13" CssClass="form-control"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters="" Enabled="True" ServiceMethod="getRuc" ServicePath="~/nuevo/autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbRFC" UseContextKey="True" MinimumPrefixLength="1">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-md-1">Estado:</div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlEstado" runat="server" Style="text-align: center" CssClass="form-control">
                                        <asp:ListItem Value="0">Inactiva</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">Activa</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-2">Nombre:</div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tbNombre" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-1"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-2">E-Mail:</div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tbEmail" runat="server" Style="text-align: center" placeholder="email@dominio.com, email@domain.net" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-md-1"></div>
                            </div>
                            <div class="row">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:LinkButton ID="bAgregar" runat="server" CssClass="btn btn-primary" OnClick="bAgregar_Click" Text="Agregar" OnClientClick="return ReglaValidation();"></asp:LinkButton>
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                            <div id="divValidators">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbRFC" ErrorMessage="El RFC es requerido" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbNombre" ErrorMessage="El nombre es requerido" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEmail" ErrorMessage="Los E-Mail son requeridos" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ErrorMessage="El formato de los E-Mail no es válido" ForeColor="Red"
                                    ControlToValidate="tbEmail"
                                    ValidationExpression="^[_a-z-A-Z0-9-]+(\.[_a-z-A-Z0-9-]+)*@[a-z-A-Z0-9-]+((\.[a-z-A-Z0-9-]+)|(\.[a-z-A-Z0-9-]+)(\.[a-z-A-Z]{2,3}))([,][_a-z-A-Z0-9-]+(\.[_a-z0-9-]+)*@[a-z-A-Z0-9-]+((\.[a-z-A-Z0-9-]+)|(\.[a-z-A-Z0-9-]+)(\.[a-z-A-Z]{2,3})))*$" Display="None"></asp:RegularExpressionValidator>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="cpCuerpoInf">
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cpTitulo">
    REGLAS DE E-MAIL
</asp:Content>
