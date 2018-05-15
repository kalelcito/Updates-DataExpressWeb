// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 04-09-2017
// ***********************************************************************
// <copyright file="Documentos.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using Control;
using Ionic.Zip;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Threading;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text.RegularExpressions;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Documentos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class DocumentosPendientes : System.Web.UI.Page
    {
        /// <summary>
        /// The _chkbox select all
        /// </summary>
        private static CheckBox _chkboxSelectAll;
        /// <summary>
        /// The _consulta
        /// </summary>
        private string _consulta;
        /// <summary>
        /// The _aux
        /// </summary>
        private string _aux;
        /// <summary>
        /// The _separador
        /// </summary>
        private string _separador = "|";
        /// <summary>
        /// The _DT
        /// </summary>
        private readonly DataTable _dt = new DataTable();
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db = new BasesDatos();
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log;
        /// <summary>
        /// The _em
        /// </summary>
        private SendMail _em;
        /// <summary>
        /// The _seleccionados
        /// </summary>
        private string[] _seleccionados;
        /// <summary>
        /// The _cantidad
        /// </summary>
        private int _cantidad;
        /// <summary>
        /// The _servidor
        /// </summary>
        private string _servidor = "";
        /// <summary>
        /// The _puerto
        /// </summary>
        private int _puerto = 25;
        /// <summary>
        /// The _SSL
        /// </summary>
        private bool _ssl = false;
        /// <summary>
        /// The _email credencial
        /// </summary>
        private string _emailCredencial = "";
        /// <summary>
        /// The _pass credencial
        /// </summary>
        private string _passCredencial = "";
        /// <summary>
        /// The _email enviar
        /// </summary>
        private string _emailEnviar = "";
        /// <summary>
        /// The _ruta document
        /// </summary>
        private string _rutaDoc = "";
        /// <summary>
        /// The _DT actual
        /// </summary>
        private DataTable _dtActual = new DataTable();
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";
        /// <summary>
        /// The _id user
        /// </summary>
        private static string _idComprobante = "";
        /// <summary>
        /// The _uuid highlight
        /// </summary>
        private static string _uuidHighlight = "";
        /// <summary>
        /// The identificadores table
        /// </summary>
        DataTable IdentificadoresTable = new DataTable("TablaIdentificadores");
        /// <summary>
        /// The registro
        /// </summary>
        string[] registro;
        /// <summary>
        /// The array_registros
        /// </summary>
        static ArrayList Array_registros = new ArrayList();
        /// <summary>
        /// The id_ renglon
        /// </summary>
        static int id_Renglon = 0;
        private readonly string comandoConcTemp = "SELECT idDetalles, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad FROM Dat_Detalles WHERE (id_Comprobante = @idComprobante)";
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                if (Session["errorTimbres"] != null && Convert.ToBoolean(Session["errorTimbres"]))
                {
                    Response.Redirect("~/CantidadTimbres.aspx", true);
                    return;
                }
                else if (Session["errorCertificado"] != null && Convert.ToBoolean(Session["errorCertificado"]))
                {
                    Response.Redirect("~/LicExpirada.aspx", true);
                    return;
                }
                _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                _log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                SqlDataSource1.ConnectionString = _db.CadenaConexion;
                SqlDataSource2.ConnectionString = _db.CadenaConexion;
                SqlDataSourcePtoEmi.ConnectionString = _db.CadenaConexion;
                SqlDataSourcePtoEmiCliente.ConnectionString = _db.CadenaConexion;
                SqlDataSourceTipoDoc.ConnectionString = _db.CadenaConexion;
                SqlDataConcTemp.ConnectionString = _db.CadenaConexion;
                SqlDataSourceSeries.ConnectionString = _db.CadenaConexion;
                SqlDataSourceUsoCfdi.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.ConnectionString = _db.CadenaConexion;
                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                }
                if (!Page.IsPostBack)
                {
                    #region Giro Empresarial

                    if (Session["IDGIRO"] != null)
                    {
                        if (Session["IDGIRO"].ToString().Contains("1"))
                        {
                            #region Hotel

                            trOtrosCargos.Visible = true;
                            rowPropina.Visible = true;
                            divDescuentoTot.Visible = false;
                            rowDenomSocial.Visible = false;
                            #endregion
                        }
                        else if (Session["IDGIRO"].ToString().Contains("2"))
                        {
                            #region Restaurante

                            trOtrosCargos.Visible = true;
                            rowPropina.Visible = true;
                            rowDenomSocial.Visible = false;
                            #endregion
                        }
                        else if (Session["IDGIRO"].ToString().Contains("3"))
                        {
                            #region Empresa



                            #endregion
                        }
                    }

                    #endregion
                    Array_registros.Clear();
                    id_Renglon = 0;
                }
                else
                {

                }
            }

            if (!IsPostBack)
            {
                Session["IdReceptor"] = "";
                Session["gvFacturas"] = null;
                _dtActual = new DataTable();

                if (Convert.ToBoolean(Session["coFactPropias"]) || Convert.ToBoolean(Session["coFactTodas"]))
                {
                    Buscar();
                    if (string.IsNullOrEmpty((string)Session["rfcCliente"]))
                    {
                        _consulta = "";
                    }
                    if (Session["rolUser"].ToString() == "3")
                    {
                        _consulta = "";
                    }
                }
                this.LlenarGrid();
                _dtActual = Session["gvComprobantes"] as DataTable;
                gvFacturas.DataSource = _dtActual;
                gvFacturas.DataBind();
                if (_dtActual != null)
                {
                    lCount.Text = "Se encontraron <span class='badge'>" + _dtActual.Rows.Count + "</span> Registros ";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "_showDocumentosActive", "resaltar('liEmision');", true);
            }
            if (!IsPostBack)
            {
                ViewState["_PageID"] = (new Random()).Next().ToString();
                try
                {
                    var cfdiVersion = Session["CfdiVersion"].ToString();
                    var columnaFacturar = gvFacturas.Columns.Cast<DataControlField>().FirstOrDefault(c => c.HeaderText.Equals("FACTURAR", StringComparison.OrdinalIgnoreCase));
                    if (columnaFacturar == null)
                    {
                        columnaFacturar = gvFacturas.Columns[9];
                    }
                    if (cfdiVersion.Equals("3.3") && columnaFacturar != null)
                    {
                        columnaFacturar.Visible = false;
                    }
                    groupTbPais.Visible = !cfdiVersion.Equals("3.3");
                    ddlPais.Visible = cfdiVersion.Equals("3.3");
                    RequiredFieldValidator_ddlPais.Enabled = cfdiVersion.Equals("3.3");
                    FillCatalogPaises();
                    DivUsoCfdi.Style["display"] = cfdiVersion.Equals("3.3") ? "inline" : "none";
                    RequiredFieldValidator_UsoCfdi.Enabled = cfdiVersion.Equals("3.3");
                    tbFormaPago.Visible = !cfdiVersion.Equals("3.3");
                    ddlFormaPago.Visible = cfdiVersion.Equals("3.3");
                    tbMetodoPago.Visible = !cfdiVersion.Equals("3.3");
                    ddlMetodoPago.Visible = cfdiVersion.Equals("3.3");
                    divChkCredito.Style["display"] = Regex.IsMatch(Session["IDENTEMI"].ToString(), @"SET920324FC3|OPL000131DL3|OHR980618BLA|OHC070227M80|LAN7008173R5|SGO050614JG0") ? "inline" : "none";
                    if (Session["IDENTEMI"].ToString().Equals("OHR980618BLA"))
                    {
                        RequiredFieldValidator14.Enabled = false;
                        RequiredFieldValidator15.Enabled = false;
                        RequiredFieldValidator16.Enabled = false;
                    }

                }
                catch (Exception ex)
                {

                }
            }
            if (Session["IsCliente"] != null)
            {
                rowFiltroRecep.Style["display"] = "none";
            }
            if (Session["IDENTEMI"].ToString().Equals("TCA130827M58"))
            {
                gvFacturas.Columns[11].Visible = true;
            }
            else
            {
                gvFacturas.Columns[11].Visible = false;
            }


        }

        private void FillCatalogPaises()
        {
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                var catalogos = (CatCdfi)Session["CatalogosCfdi33"];
                ddlPais.Items.Add(new ListItem("Seleccione", ""));
                foreach (var item in catalogos.CPais)
                {
                    ddlPais.Items.Add(new ListItem(item.Key + ": " + item.Value, item.Key));
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void LlenarGrids()
        {
            if (RowsChecked().Count > 0 && string.IsNullOrEmpty(_idComprobante))
            {
                var IDs = RowsChecked().Select(row => ((HiddenField)row.FindControl("checkHdID")).Value).ToList();
                LlenarGridView(IDs);


            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSucursal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPtoEmi.Items.Clear();
            ddlPtoEmi.Items.Add(new ListItem("Todos", "0"));
            ddlPtoEmi.DataBind();
            ddlPtoEmi.SelectedValue = "0";
        }

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
            if (ddlTipoDocumento.SelectedValue.Equals("10"))
            {
                Response.Redirect("~/recepcion/Documentos.aspx", true);
            }
            var fecha = 0;
            _consulta = "";
            _dt.Clear();
            if (tbFolioAnterior.Text.Length != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "FA" + tbFolioAnterior.Text + _separador; }
                else { _consulta = "FA" + tbFolioAnterior.Text + _separador; }
            }
            if (tbNombre.Text.Length != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "RS" + tbNombre.Text + _separador; }
                else { _consulta = "RS" + tbNombre.Text + _separador; }
            }
            if (tbControl.Text.Length != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "CF" + tbControl.Text + _separador; }
                else { _consulta = "CF" + tbControl.Text + _separador; }
            }

            if (tbRFC.Text.Length != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "RF" + tbRFC.Text + _separador; }
                else { _consulta = "RF" + tbRFC.Text + _separador; }
            }

            if (ddlTipoDocumento.SelectedIndex != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "TD" + ddlTipoDocumento.SelectedValue + _separador; }
                else { _consulta = "TD" + ddlTipoDocumento.SelectedValue + _separador; }
            }
            if (!ddlPtoEmi.SelectedValue.Equals("0"))
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "PO" + ddlPtoEmi.SelectedItem.Text + _separador; }
                else { _consulta = "PO" + ddlPtoEmi.SelectedItem.Text + _separador; }
            }
            if (tbFechaInicial.Text.Length > 5 && tbFechaFinal.Text.Length > 5)
            {
                if (tbFechaInicial.Text.Length > 5)
                {
                    if (_consulta.Length != 0) { _consulta = _consulta + "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + _separador; }
                    else { _consulta = "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + _separador; }
                    fecha = 1;
                }

                if (tbFechaFinal.Text.Length > 5)
                {
                    if (_consulta.Length != 0) { _consulta = _consulta + "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + _separador; }
                    else { _consulta = "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + _separador; }
                    fecha = fecha + 1;
                }
            }
            if (_consulta.Length > 0)
            {
                if (fecha > 1)
                {
                    _consulta = _consulta.Substring(0, _consulta.Length - 1);
                }
                if (fecha == 1)
                {
                }
            }
            else
            {
                _consulta = "-";
            }
            _consulta = _consulta.Trim('|');

            #region Procedimiento Almacenado Nuevo
            if (IsPostBack)
            {
                this.LlenarGrid();
                _dtActual = Session["gvComprobantes"] as DataTable;
                gvFacturas.DataSource = _dtActual;
                gvFacturas.DataBind();
            }
            #endregion

            lCount.Visible = true;
            //lCount.Text = "Registros encontrados: " + gvFacturas.Rows.Count;
            _consulta = "";
        }

        /// <summary>
        /// Handles the Click1 event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click1(object sender, EventArgs e)
        {
            tbFolioAnterior.Attributes["Text"] = "";
            tbFolioAnterior.Text = "";
            ddlTipoDocumento.SelectedValue = "0";
            //ddlSucursal.Text = "Selecciona el Establecimiento";
            tbFechaInicial.Text = "";
            tbFechaFinal.Text = "";
            tbRFC.Text = "";
            tbNombre.Text = "";

            Buscar();
            //Response.Redirect("Documentos.aspx", false);
            Response.Redirect("~/DocumentosPendientes.aspx", false);
        }

        /// <summary>
        /// Limpiars the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Limpiar(object sender, EventArgs e)
        {
            //tbFolioAnterior.Value = "";
            //var txtFiltros = bodyFiltros.FindDescendants<TextBox>();
            //foreach (var txt in txtFiltros)
            //{
            //    txt.Text = "";
            //}
            ScriptManager.RegisterStartupScript(this, GetType(), "_limpiarFiltros_key", "LimpiarFiltros('bodyFiltros', 'FiltrosC2');", true);
        }

        /// <summary>
        /// Regs the log.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <param name="metodoActual">The metodo actual.</param>
        /// <param name="mensajeTecnico">The mensaje tecnico.</param>
        private void RegLog(string mensaje, string metodoActual, string mensajeTecnico = "")
        {
            _log.Registrar(mensaje, GetType().Name, metodoActual, null, mensajeTecnico, null, null, _idUser, Session["IDENTEMI"].ToString());
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.LoadComplete" /> event at the end of the page load stage.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //DataFilter11.BeginFilter();
                //DataFilter11.AddNewFilter("SECUENCIA", "LIKE", "060%");
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_PageIndexChanged(object sender, EventArgs e)
        {
            //DataFilter1_OnFilterAdded();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataFilter1_OnFilterAdded();
            //var row = gvFacturas.SelectedRow;
            //var img = (ImageButton)row.Cells[0].Controls[0];
            //if (img != null)
            //{
            //    img.ImageUrl = "~/imagenes/loading.gif";
            //}
        }

        /// <summary>
        /// Handles the CheckedChanged event of the check control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void check_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Selected event of the SqlDataSource1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlDataSourceStatusEventArgs" /> instance containing the event data.</param>
        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            lCount.Visible = true;
            lCount.Text = "Se encontraron <span class='badge'>" + e.AffectedRows + "</span> registros ";
        }

        /// <summary>
        /// Handles the Click event of the bBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Handles the Click3 event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click3(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the Form control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Form_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Llenars the grid.
        /// </summary>
        private void LlenarGrid()
        {
            var aux2 = ""; var tipoComp = "";
            var topCopm = "''";
            if (Session["TOPComp"] != null) { topCopm = Session["TOPComp"].ToString(); }
            if (Convert.ToBoolean(Session["coFactTodas"])) { _aux = "true"; aux2 = "false"; } else { _aux = "false"; aux2 = "true"; }

            if (Convert.ToBoolean(Session["coFactPropias"]))
            {
                aux2 = "true";
            }
            else { aux2 = "false"; }
            if (Session["CNSComp"] != null)
            {
                if (!string.IsNullOrEmpty(Session["CNSComp"].ToString()))
                {
                    if (Session["CNSComp"].ToString().Contains("01")) { tipoComp = tipoComp.Trim(',') + "'01'"; }
                    if (Session["CNSComp"].ToString().Contains("04")) { tipoComp = tipoComp.Trim(',') + ",'04'"; }
                    if (Session["CNSComp"].ToString().Contains("05")) { tipoComp = tipoComp.Trim(',') + ",'05'"; }
                    if (Session["CNSComp"].ToString().Contains("06")) { tipoComp = tipoComp.Trim(',') + ",'06'"; }
                    if (Session["CNSComp"].ToString().Contains("07")) { tipoComp = tipoComp.Trim(',') + ",'07'"; }
                    if (Session["CNSComp"].ToString().Contains("08")) { tipoComp = tipoComp.Trim(',') + ",'08'"; }
                    if (Session["CNSComp"].ToString().Contains("09")) { tipoComp = tipoComp.Trim(',') + ",'09'"; }

                    if (!string.IsNullOrEmpty(tipoComp))
                    {
                        tipoComp = "Dat_General.codDoc in (" + tipoComp.Trim(',') + ") ";
                    }
                }
            }

            _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            _db.Conectar();
            _db.CrearComandoProcedimiento("PA_facturas_basico");
            _db.AsignarParametroProcedimiento("@QUERY", System.Data.DbType.String, _consulta);
            _db.AsignarParametroProcedimiento("@RFC", System.Data.DbType.String, Session["rfcCliente"].ToString());
            _db.AsignarParametroProcedimiento("@ROL", System.Data.DbType.Boolean, _aux);
            _db.AsignarParametroProcedimiento("@empleado", System.Data.DbType.String, Session["USERNAME"].ToString());
            _db.AsignarParametroProcedimiento("@ROLSUCURSAL", System.Data.DbType.String, aux2);
            _db.AsignarParametroProcedimiento("@TOP", System.Data.DbType.String, topCopm);
            _db.AsignarParametroProcedimiento("@PTOEMII", System.Data.DbType.String, "1");
            _db.AsignarParametroProcedimiento("@CNSComp", System.Data.DbType.String, tipoComp);
            _db.AsignarParametroProcedimiento("@QUERYSTRING", DbType.String, "Dat_General.tipo <> 'C' AND Dat_General.estado = 2");
            _dtActual.Load(_db.EjecutarConsulta());

            if (Session["IsCliente"] != null && !IsPostBack)
            {
                ddlTipoDocumento.Items.Clear();
                ddlTipoDocumento.DataSource = null;
                ddlTipoDocumento.DataSourceID = null;
                ddlTipoDocumento.DataTextField = null;
                ddlTipoDocumento.DataValueField = null;
                ddlTipoDocumento.DataBind();
                ddlTipoDocumento.Items.Add(new ListItem("Todos", "0"));
                foreach (DataRow row in _dtActual.Rows)
                {
                    var codDoc = row["codDoc"].ToString();
                    var tipoDoc = row["TIPODOC"].ToString();
                    if (ddlTipoDocumento.Items.FindByValue(codDoc) == null)
                    {
                        ddlTipoDocumento.Items.Add(new ListItem(tipoDoc, codDoc));
                    }
                }
                ddlTipoDocumento.SelectedValue = "0";
            }
            _db.Desconectar();
            Session["gvComprobantes"] = _dtActual;
            lCount.Text = "Se encontraron <span class='badge'>" + _dtActual.Rows.Count + "</span> Registros ";
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;
            gvFacturas.PageIndex = e.NewPageIndex;

            //gvFacturas.PageIndex = gvFacturas.PageCount;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the First event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_First(object sender, EventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;
            gvFacturas.PageIndex = 0;

            //gvFacturas.PageIndex = gvFacturas.PageCount;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Prev event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Prev(object sender, EventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;

            if (gvFacturas.PageIndex > 0)
            {
                gvFacturas.PageIndex = gvFacturas.PageIndex - 1;
            }
            //gvFacturas.PageIndex = gvFacturas.PageCount;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Next event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Next(object sender, EventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;


            if (gvFacturas.PageIndex < gvFacturas.PageCount - 1)
            {
                gvFacturas.PageIndex = gvFacturas.PageIndex + 1;
            }
            //gvFacturas.PageIndex = gvFacturas.PageCount;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Last event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Last(object sender, EventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;

            //gvFacturas.SetPageIndex(gvFacturas.PageCount);

            //gvFacturas.PageIndex = gvFacturas.PageIndex + 1;

            gvFacturas.PageIndex = gvFacturas.PageCount - 1;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Sorted event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Sorted(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Sorting event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            if (_dtActual != null)
            {
                var dataView = new DataView(_dtActual);
                if (Session["ordenar"] != null)
                {
                    if (Session["ordenar"].ToString() == "ASC")
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    else
                    {
                        e.SortDirection = SortDirection.Ascending;
                    }
                }
                dataView.Sort = e.SortExpression + " " + ConvertSortDirection(e.SortDirection);

                gvFacturas.DataSource = dataView;
                gvFacturas.DataBind();
            }
        }

        /// <summary>
        /// Converts the sort direction.
        /// </summary>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>System.String.</returns>
        private string ConvertSortDirection(SortDirection sortDirection)
        {
            var newSortDirection = string.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    Session["ordenar"] = newSortDirection;
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    Session["ordenar"] = newSortDirection;
                    break;
            }

            return newSortDirection;
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvFacturas.HeaderRow != null)
            {
                _chkboxSelectAll = (CheckBox)gvFacturas.HeaderRow.FindControl("chkboxSelectAll");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var uuid = row.Field<string>("UUID");
                    if (!string.IsNullOrEmpty(uuid) && !string.IsNullOrEmpty(_uuidHighlight) && uuid.Equals(_uuidHighlight))
                    {
                        e.Row.CssClass = "bgRow";
                    }
                }
                catch { }
                try
                {
                    var lb = (HyperLink)(e.Row.FindControl("lbPopupObs"));
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var name = row.Field<string>("observaciones").Trim("<br/>".ToCharArray());
                    if (!string.IsNullOrEmpty(name))
                    {
                        var nameScaped = name.Replace("'", "\"");
                        lb.Attributes["data-content"] = name + "<br/><div align='right'><button type='button' id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
                    }
                }
                catch { }

            }
        }

        /// <summary>
        /// Handles the PreRender event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvFacturas.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTipoDocumento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPtoEmi.Items.Clear();
            var nullItem = new ListItem("Todas", "0");
            nullItem.Selected = true;
            if (Session["IsCliente"] != null)
            {
                ddlPtoEmi.DataSourceID = "SqlDataSourcePtoEmiCliente";
                SqlDataSourcePtoEmiCliente.SelectParameters["tipo"].DefaultValue = ddlTipoDocumento.SelectedValue;
                SqlDataSourcePtoEmiCliente.DataBind();
            }
            else
            {
                SqlDataSourcePtoEmi.SelectParameters["tipo"].DefaultValue = ddlTipoDocumento.SelectedValue;
                SqlDataSourcePtoEmi.DataBind();
            }
            ddlPtoEmi.DataBind();
            ddlPtoEmi.Items.Add(nullItem);
        }

        /// <summary>
        /// Ceroses the null.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>System.String.</returns>
        private string CerosNull(string a)
        {
            decimal b;
            var cifra = (!string.IsNullOrEmpty(a) ? a : "").Replace(",", "").Trim();
            var result = string.Format("{0:0.00}", Convert.ToDecimal(string.IsNullOrEmpty(cifra) || !decimal.TryParse(cifra, out b) || b < 0 ? "0.00" : cifra));
            return result;
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRFC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRFC_TextChanged(object sender, EventArgs e)
        {
            _db.Conectar();
            _db.CrearComando(@"SELECT NOMREC FROM Cat_Receptor WHERE RFCREC = @RFCREC");
            _db.AsignarParametroCadena("@RFCREC", tbRFC.Text);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                tbNombre.Text = dr[0].ToString();
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlPageSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pageSize = 10;
            int.TryParse(ddlPageSize.SelectedValue, out pageSize);
            gvFacturas.PageSize = pageSize;
            Buscar();
        }

        /// <summary>
        /// Handles the Click event of the lbEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="Exception"></exception>
        protected void lbEditar_Click(object sender, EventArgs e)
        {
            try
            {
                panelHospedaje.Style["display"] = "block";
                panelIne.Style["display"] = "block";
                var lb = (LinkButton)sender;
                _idComprobante = lb.CommandArgument;
                var tryLoad = LoadData(_idComprobante);
                if (!tryLoad.Key)
                {
                    throw new Exception(tryLoad.Value);
                }
                rblTipoEdicion.SelectedValue = "2";
                rblTipoEdicion_SelectedIndexChanged(null, null);
                rblTipoEdicion.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "lbEditar_Click" + _idComprobante, "$('#divFacturar').modal('show');", true);
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, ex.Message, 4);
            }
        }

        /// <summary>
        /// Handles the Click event of the bFacturar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="Exception">
        /// </exception>
        protected void bFacturar_Click(object sender, EventArgs e)
        {
            Page.Validate("Facturacion");
            if (Page.IsValid || Session["IDENTEMI"].ToString().Equals("OHR980618BLA"))
            {
                //var lb = (LinkButton)sender;
                //_idComprobante = lb.CommandArgument;
                //    lbEditar_Click(null, null);
                var rows = RowsChecked();
                if (!string.IsNullOrEmpty(_idComprobante))
                {
                    try
                    {
                        #region Factura Individual

                        var sql = "";
                        decimal iva16 = 0;
                        decimal propina = 0;
                        DbDataReader dr;
                        var sAmbiente = "1";
                        var conceptos = new List<object[]>();
                        var txt = new SpoolMx();
                        var codDoc = "";
                        var manual = true;
                        var folioReservacion = "";
                        var subtotal = "";
                        sql = @"SELECT
                            p.formapago, p.condicionesDePago, g.subTotal, g.totalDescuento, g.motivoDescuento, g.tipoCambio, g.moneda, g.total, g.tipoDeComprobante as tipoDoc, g.metodoPago, g.lugarExpedicion, p.numCtaPago, g.numeroAutorizacion, g.serie, g.fecha, g.total, g.codDoc, e.NOMEMI, e.RFCEMI, e.curp as curpE, e.telefono as telE, e.email as mailE, e.EmpresaTipo as etipoE, e.regimenFiscal as regimenE, e.obligadoContabilidad as contE, e.dirMatriz AS calleE, e.noExterior as extE, e.noInterior as intE, e.colonia as colE, e.localidad as locE, e.referencia as refE, e.municipio as munE, e.estado as edoE, e.pais as paisE, e.codigoPostal as cpE, d.dirEstablecimientos as calleExp, d.noExterior as extExp, d.noInterior as intExp, d.colonia as colExp, d.localidad as locExp, d.referencia as refExp, d.municipio as munExp, d.estado as esoExp, d.pais as paisExp, d.codigoPostal as cpExp, r.NOMREC, r.RFCREC, r.curp as curpR, r.telefono as telR, r.email as mailR, r.telefono2 as tel2R, r.denominacionSocial as denR, r.obligadoContabilidad as contR, r.domicilio as calleR, r.noExterior as extR, r.noInterior as intR, r.colonia as colR, r.localidad as locR, r.referencia as refR, r.municipio as munR, r.estado as edoR, r.pais as paisR, r.codigoPostal as cpR, IVA12 as iva16, g.propina, g.ambiente, g.idComprobante, g.importeAPagar, g.cargoxservicio as otrosCargos, g.observaciones, he.tipo AS tipoHE, g.folioReservacion, he.noHabitacion, he.fechaLlegada, he.fechaSalida, he.huesped, g.folio, t.tipo AS tipoEmision, g.estab AS claveSucursal
                        FROM
                            Dat_General g INNER JOIN
                            Cat_Emisor e ON g.id_Emisor = e.IDEEMI INNER JOIN
                            Dat_DOMEMIEXP d ON g.id_EmisorExp = d.IDEDOMEMIEXP INNER JOIN
                            Cat_Receptor r ON g.id_Receptor = r.IDEREC INNER JOIN
							Log_Trama t ON g.idTrama = t.idTrama INNER JOIN
                            Dat_Pagos p ON g.idComprobante = p.id_Comprobante LEFT OUTER JOIN
							Dat_HabitacionEvento he ON he.id_Comprobante = g.idComprobante
                        WHERE g.idComprobante = @id";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            subtotal = dr["subTotal"].ToString();
                            codDoc = dr["codDoc"].ToString();
                            manual = dr["tipoEmision"].ToString().Equals("2");
                            txt.SetComprobanteCfdi(dr["serie"].ToString(), dr["folio"].ToString(), Localization.Now.ToString("s"), dr["formapago"].ToString(), dr["condicionesDePago"].ToString(), dr["subTotal"].ToString(), dr["totalDescuento"].ToString(), dr["motivoDescuento"].ToString(), dr["tipoCambio"].ToString(), dr["moneda"].ToString(), dr["total"].ToString(), dr["tipoDoc"].ToString(), dr["metodoPago"].ToString(), dr["lugarExpedicion"].ToString(), dr["numCtaPago"].ToString(), dr["numeroAutorizacion"].ToString(), dr["serie"].ToString(), Localization.Parse(dr["fecha"].ToString()).ToString("s"), dr["total"].ToString(), dr["codDoc"].ToString(), "", dr["otrosCargos"].ToString(), dr["importeAPagar"].ToString(), tbObservaciones.Text);
                            txt.SetEmisorCfdi(dr["NOMEMI"].ToString(), dr["RFCEMI"].ToString(), dr["curpE"].ToString(), dr["telE"].ToString(), dr["mailE"].ToString(), dr["etipoE"].ToString(), dr["regimenE"].ToString(), dr["contE"].ToString());
                            txt.SetEmisorDomCfdi(dr["calleE"].ToString(), dr["extE"].ToString(), dr["intE"].ToString(), dr["colE"].ToString(), dr["locE"].ToString(), dr["refE"].ToString(), dr["munE"].ToString(), dr["edoE"].ToString(), dr["paisE"].ToString(), dr["cpE"].ToString());
                            txt.SetEmisorExpCfdi(dr["calleExp"].ToString(), dr["extExp"].ToString(), dr["intExp"].ToString(), dr["colExp"].ToString(), dr["locExp"].ToString(), dr["refExp"].ToString(), dr["munExp"].ToString(), dr["esoExp"].ToString(), dr["paisExp"].ToString(), dr["cpExp"].ToString(), dr["claveSucursal"].ToString());
                            txt.SetReceptorCfdi(tbRazonSocialRec.Text, tbRfcRec.Text, dr["curpR"].ToString(), dr["telR"].ToString(), dr["mailR"].ToString(), dr["tel2R"].ToString(), tbDenomSocialRec.Text, dr["contR"].ToString());
                            if (cbDomRec.Checked)
                            {
                                var isSucursal = !ddlSucRec.SelectedValue.Equals("0");
                                txt.SetReceptorDomCfdi(tbCalleRec.Text, tbNoExtRec.Text, tbNoIntRec.Text, tbColoniaRec.Text, dr["locR"].ToString(), dr["refR"].ToString(), tbMunicipioRec.Text, tbEstadoRec.Text, tbPaisRec.Text, tbCpRec.Text, isSucursal);
                            }
                            folioReservacion = dr["folioReservacion"].ToString();
                            decimal.TryParse(CerosNull(dr["iva16"].ToString()), out iva16);
                            decimal.TryParse(CerosNull(dr["propina"].ToString()), out propina);
                            sAmbiente = dr["ambiente"].ToString();
                            if (!(dr["tipoHE"] is DBNull) && dr["tipoHE"] != null && !string.IsNullOrEmpty(dr["tipoHE"].ToString()))
                            {
                                switch (dr["tipoHE"].ToString())
                                {
                                    //case "1":
                                    //    txt.SetInfoAdicionalHabitacionCfdi(dr["huesped"].ToString(), folioReservacion, dr["noHabitacion"].ToString(), dr["fechaLlegada"].ToString(), dr["fechaSalida"].ToString());
                                    //    break;
                                    //case "2":
                                    //    txt.SetInfoAdicionalEventoCfdi(dr["huesped"].ToString(), dr["folioReservacion"].ToString(), dr["fechaLlegada"].ToString());
                                    //    break;
                                    case "1":
                                        txt.SetInfoAdicionalHabitacionCfdi(tbNombrehuesped.Text, folioReservacion, tbHabitacionHuesped.Text, tbLlegadaHuesped.Text, tbSalidaHuesped.Text);
                                        break;
                                    case "2":
                                        txt.SetInfoAdicionalEventoCfdi(tbNombrehuesped.Text, folioReservacion, tbLlegadaHuesped.Text);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        _db.Desconectar();
                        sql = @"SELECT
	                         ISNULL(SUM(it.valor), 0.00) as impTras, ISNULL(SUM(ir.valorRetenido), 0.00) as impRets
                        FROM
	                        Dat_General g INNER JOIN
							Dat_TotalConImpuestos it ON it.id_Comprobante = g.idComprobante INNER JOIN
							Dat_ImpuestosRetenciones ir ON ir.numDocSustento = g.idComprobante
						WHERE
							g.idComprobante = @id";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            txt.SetCantidadImpuestosCfdi(dr["impRets"].ToString(), dr["impTras"].ToString(), CerosNull(iva16.ToString()));
                        }
                        _db.Desconectar();
                        sql = @"SELECT
	                        ir.tipo as impuesto, ir.valorRetenido as importe
                        FROM
	                        Dat_ImpuestosRetenciones ir INNER JOIN
	                        Dat_General g on ir.numDocSustento = g.idComprobante
						WHERE
							g.idComprobante = @id";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            txt.AgregaImpuestoRetencionCfdi(dr["impuesto"].ToString(), dr["importe"].ToString());
                        }
                        _db.Desconectar();
                        sql = @"SELECT
	                        CASE
		                        it.codigo
		                        WHEN '1' THEN 'IVA'
		                        WHEN '2' THEN 'IEPS'
	                        END AS impuesto, it.tarifa as tasa, it.valor as importe
                        FROM
	                        Dat_TotalConImpuestos it INNER JOIN
	                        Dat_General g on it.id_Comprobante = g.idComprobante
						WHERE
							g.idComprobante = @id";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            txt.AgregaImpuestoTrasladoCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                        }
                        _db.Desconectar();
                        if (rblTipoEdicion.SelectedValue.Equals("1"))
                        {
                            sql = "SELECT '', codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial FROM Dat_Detalles INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @id";
                            _db.Conectar();
                            _db.CrearComando(sql);
                            _db.AsignarParametroCadena("@id", _idComprobante);
                            dr = _db.EjecutarConsulta();
                            while (dr.Read())
                            {
                                var sqlResults = new object[dr.FieldCount];
                                dr.GetValues(sqlResults);
                                conceptos.Add(sqlResults);
                            }
                            _db.Desconectar();
                        }
                        else
                        {
                            conceptos = new List<object[]>();
                            conceptos.Add(new string[] { "", "", tbNuevoConcepto.Text, subtotal, "1.0", subtotal, "NO APLICA", "" });
                        }
                        var conceptosGroup = new List<object[]>();
                        foreach (var concepto in conceptos)
                        {
                            var conceptoGrouped = conceptosGroup.Find(x => x[2].ToString().Equals(concepto[2].ToString(), StringComparison.OrdinalIgnoreCase));
                            if (conceptoGrouped != null)
                            {
                                decimal unitarioConcepto = 0;
                                decimal unitarioGrouped = 0;
                                decimal importeConcepto = 0;
                                decimal importeGrouped = 0;
                                decimal.TryParse(concepto[3].ToString(), out unitarioConcepto);
                                decimal.TryParse(conceptoGrouped[3].ToString(), out unitarioGrouped);
                                decimal.TryParse(concepto[5].ToString(), out importeConcepto);
                                decimal.TryParse(conceptoGrouped[5].ToString(), out importeGrouped);
                                unitarioGrouped += unitarioConcepto;
                                importeGrouped += importeConcepto;
                                conceptoGrouped[3] = unitarioGrouped.ToString();
                                conceptoGrouped[5] = importeGrouped.ToString();
                            }
                            else
                            {
                                conceptosGroup.Add(concepto);
                            }
                        }
                        foreach (var concepto in conceptosGroup)
                        {
                            txt.AgregaConceptoCfdi(concepto[4].ToString(), concepto[6].ToString(), concepto[1].ToString(), concepto[2].ToString(), concepto[3].ToString(), concepto[5].ToString());
                        }
                        if (propina > 0)
                        {
                            txt.SetInfoAdicionalRestauranteCfdi(CerosNull(propina.ToString()), folioReservacion);
                        }
                        sql = "SELECT idImpLocal FROM Dat_MX_ImpLocales INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @id";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        dr = _db.EjecutarConsulta();
                        var tieneLocales = dr.HasRows;
                        _db.Desconectar();
                        if (tieneLocales)
                        {
                            sql = @"SELECT totalRetImpLocales, totalTraImpLocales FROM Dat_General WHERE idComprobante = @id";
                            _db.Conectar();
                            _db.CrearComando(sql);
                            _db.AsignarParametroCadena("@id", _idComprobante);
                            dr = _db.EjecutarConsulta();
                            if (dr.Read())
                            {
                                txt.SetImpuestosLocalesCfdi(dr["totalRetImpLocales"].ToString(), dr["totalTraImpLocales"].ToString());
                            }
                            _db.Desconectar();
                            sql = "SELECT nombre as impuesto, tasadeRetencion as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeRetencion IS NOT NULL AND tasadeTraslado IS NULL AND idComprobante = @id";
                            _db.Conectar();
                            _db.CrearComando(sql);
                            _db.AsignarParametroCadena("@id", _idComprobante);
                            dr = _db.EjecutarConsulta();
                            while (dr.Read())
                            {
                                txt.AgregaRetencionLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                            }
                            _db.Desconectar();
                            sql = "SELECT nombre as impuesto, tasadeTraslado as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeTraslado IS NOT NULL AND tasadeRetencion IS NULL AND idComprobante = @id";
                            _db.Conectar();
                            _db.CrearComando(sql);
                            _db.AsignarParametroCadena("@id", _idComprobante);
                            dr = _db.EjecutarConsulta();
                            while (dr.Read())
                            {
                                txt.AgregaTrasladoLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                            }
                            _db.Desconectar();
                        }
                        #region NODO INE
                        if (chkHabilitarINE.Checked)
                        {
                            txt.SetComplementoIneCfdi(DDTipo_Proceso.SelectedValue, DDTipoComite.SelectedValue, TextboxIdentificador.Text);
                            string[] arrayEntidades = new string[64];
                            int conta = 0;
                            foreach (string[] registro in Array_registros)
                            {
                                if (!arrayEntidades.Contains(registro[3] + registro[4]))
                                {
                                    //Por clave
                                    txt.AgregarEntidadIne(registro[3], registro[4]);
                                    //Por Entidad
                                    foreach (string[] registroId in Array_registros)
                                    {
                                        if (registroId[3] == registro[3])
                                        {
                                            if (!string.IsNullOrEmpty(registro[7]))
                                            {
                                                string[] GrupoIdContabilidad = registro[7].Split(',');
                                                foreach (string idContabilidad in GrupoIdContabilidad)
                                                {
                                                    txt.AgregarContabilidadIne(idContabilidad);
                                                }
                                            }
                                        }
                                    }
                                    arrayEntidades[conta] = registro[3] + registro[4];
                                    conta++;
                                }
                            }
                        }
                        #endregion
                        var ambiente = false;
                        switch (sAmbiente)
                        {
                            case "1":
                                ambiente = false;
                                break;
                            case "2":
                                ambiente = true;
                                break;
                            default:
                                break;
                        }
                        var txtInvoice = "";
                        switch (codDoc)
                        {
                            case "01":
                            case "04":
                            case "06":
                            case "08":
                            case "09":
                                txtInvoice = txt.ConstruyeTxtCfdi();
                                break;
                            case "07":
                                txtInvoice = txt.ConstruyeTxtRetenciones();
                                break;
                        }
                        var coreMx = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                        var result = coreMx.ComprobantePaquete(_idComprobante, txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, codDoc, !manual, manual);
                        if (result != null)
                        {
                            var xDoc = new XmlDocument();
                            xDoc.LoadXml(result.OuterXml);
                            Session["uuidCreado"] = GetAtributte(xDoc, "UUID", "tfd:TimbreFiscalDigital");
                            (Master as SiteMaster).MostrarAlerta(this, "El comprobante se creó correctamente.", 2, null, "bootbox.hideAll();$('#divFacturar').modal('hide'); window.location.href = '" + ResolveClientUrl("~/Documentos.aspx") + "';", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }
                        else
                        {
                            throw new Exception(coreMx.ObtenerMensaje(), new Exception(coreMx.ObtenerMensajeTecnico()));
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El comprobante no procesó correctamente.<br/><br/>" + ex.Message, 2, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
                else if (rows.Count > 0)
                {
                    try
                    {
                        #region Factura en Conjunto

                        decimal totalCalculado = 0;
                        decimal dSubTotal = 0;
                        decimal dImpuestrosT = 0;
                        decimal dImpuestrosR = 0;
                        decimal dImpuestrosTl = 0;
                        decimal dImpuestrosRl = 0;
                        decimal dDescuentos = 0;
                        decimal totalOriginal = 0;
                        var txt = new SpoolMx();
                        DbDataReader dr;
                        var rowsChecked = RowsChecked();
                        var idComprobantes = rowsChecked.Select(row => ((HiddenField)row.FindControl("checkHdID")).Value).ToList();
                        var comprobantes = string.Join(",", idComprobantes);

                        #region Datos Emisor

                        _db.Conectar();
                        _db.CrearComando(@"SELECT TOP 1
       RFCEMI AS rfc
      ,NOMEMI AS nombre
      ,dirMatriz AS calle
      ,EmpresaTipo AS empresaTipo
      ,noExterior
      ,noInterior
      ,colonia
      ,localidad
      ,referencia
      ,municipio
      ,estado
      ,pais
      ,codigoPostal
      ,regimenFiscal
  FROM Cat_Emisor
  WHERE RFCEMI = @rfc
  ORDER BY IDEEMI ASC");
                        _db.AsignarParametroCadena("@rfc", Session["IDENTEMI"].ToString());
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            txt.SetEmisorCfdi(dr["nombre"].ToString(), dr["rfc"].ToString(), "", "", "", dr["empresaTipo"].ToString(), dr["regimenFiscal"].ToString());
                            txt.SetEmisorDomCfdi(dr["calle"].ToString(), dr["noExterior"].ToString(), dr["noInterior"].ToString(), dr["colonia"].ToString(), dr["localidad"].ToString(), dr["referencia"].ToString(), dr["municipio"].ToString(), dr["estado"].ToString(), dr["pais"].ToString(), dr["codigoPostal"].ToString());
                        }
                        _db.Desconectar();

                        #endregion
                        #region Datos Sucursal/Expedicion

                        var sucursalSession = Session["idSucursal"];

                        if (sucursalSession != null)
                        {
                            var sucursal = sucursalSession.ToString();
                            if (!string.IsNullOrEmpty(sucursal))
                            {
                                _db.Conectar();
                                _db.CrearComando(@"SELECT
       RFC AS rfc
      ,clave
      ,sucursal AS nombre
      ,calle
      ,noExterior
      ,noInterior
      ,colonia
      ,localidad
      ,referencia
      ,municipio
      ,estado
      ,pais
      ,codigoPostal
      ,Cat_SucursalesEmisor.telefono
  FROM Cat_SucursalesEmisor
  WHERE idSucursal = @id");
                                _db.AsignarParametroCadena("@id", sucursal);
                                dr = _db.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    txt.SetEmisorExpCfdi(dr["calle"].ToString(), dr["noExterior"].ToString(), dr["noInterior"].ToString(), dr["colonia"].ToString(), dr["localidad"].ToString(), dr["referencia"].ToString(), dr["municipio"].ToString(), dr["estado"].ToString(), dr["pais"].ToString(), dr["codigoPostal"].ToString(), sucursal);
                                }
                                _db.Desconectar();
                            }
                        }

                        #endregion
                        #region Datos Receptor

                        _db.Conectar();
                        _db.CrearComando(@"SELECT TOP 1 curp AS curpR, telefono AS telR, email AS mailR, telefono2 AS tel2R, obligadoContabilidad AS contR, localidad as locR, referencia as refR, numRegIdTrib, usoCfdi FROM Cat_Receptor WHERE RFCREC = @rfc ORDER BY IDEREC");
                        _db.AsignarParametroCadena("@rfc", tbRfcRec.Text);
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            txt.SetReceptorCfdi(tbRazonSocialRec.Text, tbRfcRec.Text, dr["curpR"].ToString(), dr["telR"].ToString(), tbMail.Text.Trim(), dr["tel2R"].ToString(), tbDenomSocialRec.Text, dr["contR"].ToString(), dr["numRegIdTrib"].ToString(), ddlUsoCfdi.SelectedValue);
                            if (cbDomRec.Checked)
                            {
                                var isSucursal = !ddlSucRec.SelectedValue.Equals("0");
                                txt.SetReceptorDomCfdi(tbCalleRec.Text, tbNoExtRec.Text, tbNoIntRec.Text, tbColoniaRec.Text, dr["locR"].ToString(), dr["refR"].ToString(), tbMunicipioRec.Text, tbEstadoRec.Text, tbPaisRec.Text, tbCpRec.Text, isSucursal);
                            }
                        }
                        else
                        {
                            txt.SetReceptorCfdi(tbRazonSocialRec.Text, tbRfcRec.Text, "", "", tbMail.Text.Trim(), "", tbDenomSocialRec.Text, "", "", ddlUsoCfdi.SelectedValue);
                            if (cbDomRec.Checked)
                            {
                                var isSucursal = !ddlSucRec.SelectedValue.Equals("0");
                                txt.SetReceptorDomCfdi(tbCalleRec.Text, tbNoExtRec.Text, tbNoIntRec.Text, tbColoniaRec.Text, "", "", tbMunicipioRec.Text, tbEstadoRec.Text, tbPaisRec.Text, tbCpRec.Text, isSucursal);
                            }
                        }
                        _db.Desconectar();

                        #endregion
                        #region Datos Conceptos

                        var conceptos = new List<object[]>();
                        _db.Conectar();
                        _db.CrearComando(@"SELECT idDetalles, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial, descuento, claveProdServ, claveUnidad FROM Dat_Detalles INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante IN (@id)");
                        _db.AsignarParametroFlotante("@id", comprobantes);
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            var sqlResults = new object[dr.FieldCount];
                            dr.GetValues(sqlResults);
                            conceptos.Add(sqlResults);
                        }
                        _db.Desconectar();

                        //var conceptosGroup = new List<object[]>();
                        //foreach (var concepto in conceptos)
                        //{
                        //    var conceptoGrouped = conceptosGroup.Find(x => x[2].ToString().Equals(concepto[2].ToString(), StringComparison.OrdinalIgnoreCase));
                        //    if (conceptoGrouped != null)
                        //    {
                        //        decimal unitarioConcepto = 0;
                        //        decimal unitarioGrouped = 0;
                        //        decimal importeConcepto = 0;
                        //        decimal importeGrouped = 0;
                        //        decimal.TryParse(concepto[3].ToString(), out unitarioConcepto);
                        //        decimal.TryParse(conceptoGrouped[3].ToString(), out unitarioGrouped);
                        //        decimal.TryParse(concepto[5].ToString(), out importeConcepto);
                        //        decimal.TryParse(conceptoGrouped[5].ToString(), out importeGrouped);
                        //        unitarioGrouped += unitarioConcepto;
                        //        importeGrouped += importeConcepto;
                        //        conceptoGrouped[3] = unitarioGrouped.ToString();
                        //        conceptoGrouped[5] = importeGrouped.ToString();
                        //    }
                        //    else
                        //    {
                        //        conceptosGroup.Add(concepto);
                        //    }
                        //}
                        //foreach (var concepto in conceptosGroup)
                        foreach (var concepto in conceptos)
                        {
                            var idInterno = concepto[0].ToString();
                            txt.AgregaConceptoCfdi(concepto[4].ToString(), concepto[6].ToString(), concepto[1].ToString(), concepto[2].ToString(), concepto[3].ToString(), concepto[5].ToString(), concepto[8].ToString(), concepto[9].ToString(), concepto[10].ToString(), idInterno);
                            var listaImpuestosSession = Session["_listaImpuestos"];
                            if (listaImpuestosSession != null)
                            {
                                var listaImpuestos = (List<ImpuestosConceptoTemp>)listaImpuestosSession;
                                var impuestosConcepto = listaImpuestos.Where(x => x.IdConcepto.Equals(idInterno)).ToList();
                                foreach (var impuesto in impuestosConcepto)
                                {
                                    txt.AgregaConceptoImpuestoCfdi(impuesto.IsRetencion, ControlUtilities.CerosNull(impuesto.Base), impuesto.Impuesto, impuesto.TipoFactor, ControlUtilities.CerosNull(impuesto.TasaOCuota), ControlUtilities.CerosNull(impuesto.Importe), idInterno);
                                }
                            }
                            else
                            {
                                _db.Conectar();
                                _db.CrearComando(@"SELECT tipo, baseImpuesto, claveImpuesto, descripcion, tipoFactor, tasaOCuota, importe FROM Dat_ImpuestosDetallesCfdi33 WHERE idDetalle = @idDetalle");
                                _db.AsignarParametroFlotante("@idDetalle", idInterno);
                                dr = _db.EjecutarConsulta();
                                while (dr.Read())
                                {
                                    txt.AgregaConceptoImpuestoCfdi(dr["tipo"].ToString().Equals("R"), dr["baseImpuesto"].ToString(), dr["claveImpuesto"].ToString(), dr["tipoFactor"].ToString(), dr["tasaOCuota"].ToString(), dr["importe"].ToString(), idInterno);
                                }
                            }
                            _db.Desconectar();
                        }


                        #endregion
                        #region Datos Impuestos

                        _db.Conectar();
                        _db.CrearComando(@"SELECT
	                         ISNULL(SUM(it.valor), 0.00) as impTras, ISNULL(SUM(ir.valorRetenido), 0.00) as impRets, ISNULL(SUM(g.IVA12), 0.00) AS iva16
                        FROM
	                        Dat_General g INNER JOIN
							Dat_TotalConImpuestos it ON it.id_Comprobante = g.idComprobante LEFT OUTER JOIN
							Dat_ImpuestosRetenciones ir ON ir.numDocSustento = g.idComprobante
						WHERE
							g.idComprobante IN (@id)");
                        _db.AsignarParametroFlotante("@id", comprobantes);
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            txt.SetCantidadImpuestosCfdi(dr["impRets"].ToString(), dr["impTras"].ToString(), dr["iva16"].ToString());
                            decimal.TryParse(dr["impTras"].ToString(), out dImpuestrosT);
                            decimal.TryParse(dr["impRets"].ToString(), out dImpuestrosR);
                        }
                        _db.Desconectar();
                        _db.Conectar();
                        _db.CrearComando(@"SELECT
	                        ir.tipo as impuesto, ir.valorRetenido as importe
                        FROM
	                        Dat_ImpuestosRetenciones ir INNER JOIN
	                        Dat_General g on ir.numDocSustento = g.idComprobante
						WHERE
							g.idComprobante IN (@id)");
                        _db.AsignarParametroFlotante("@id", comprobantes);
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            txt.AgregaImpuestoRetencionCfdi(dr["impuesto"].ToString(), dr["importe"].ToString());
                        }
                        _db.Desconectar();
                        _db.Conectar();
                        _db.CrearComando(@"SELECT
	                        CASE
		                        it.codigo
		                        WHEN '1' THEN 'IVA'
		                        WHEN '2' THEN 'IEPS'
	                        END AS impuesto, it.tarifa as tasa, it.valor as importe
                        FROM
	                        Dat_TotalConImpuestos it INNER JOIN
	                        Dat_General g on it.id_Comprobante = g.idComprobante
						WHERE
							g.idComprobante IN (@id)");
                        _db.AsignarParametroFlotante("@id", comprobantes);
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            txt.AgregaImpuestoTrasladoCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                        }
                        _db.Desconectar();

                        #endregion
                        #region Datos Impuestos Locales

                        _db.Conectar();
                        _db.CrearComando(@"SELECT SUM(idImpLocal) AS idImpLocal FROM Dat_MX_ImpLocales INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante IN (@id)");
                        _db.AsignarParametroFlotante("@id", comprobantes);
                        dr = _db.EjecutarConsulta();
                        var tieneLocales = dr.HasRows;
                        _db.Desconectar();
                        if (tieneLocales)
                        {
                            _db.Conectar();
                            _db.CrearComando(@"SELECT SUM(totalRetImpLocales) AS totalRetImpLocales, SUM(totalTraImpLocales) AS totalTraImpLocales FROM Dat_General WHERE idComprobante IN (@id)");
                            _db.AsignarParametroFlotante("@id", comprobantes);
                            dr = _db.EjecutarConsulta();
                            if (dr.Read())
                            {
                                txt.SetImpuestosLocalesCfdi(dr["totalRetImpLocales"].ToString(), dr["totalTraImpLocales"].ToString());
                                decimal.TryParse(dr["totalTraImpLocales"].ToString(), out dImpuestrosTl);
                                decimal.TryParse(dr["totalRetImpLocales"].ToString(), out dImpuestrosRl);
                            }
                            _db.Desconectar();
                            _db.Conectar();
                            _db.CrearComando(@"SELECT nombre as impuesto, tasadeRetencion as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeRetencion IS NOT NULL AND tasadeTraslado IS NULL AND idComprobante IN (@id)");
                            _db.AsignarParametroFlotante("@id", comprobantes);
                            dr = _db.EjecutarConsulta();
                            while (dr.Read())
                            {
                                txt.AgregaRetencionLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                            }
                            _db.Desconectar();
                            _db.Conectar();
                            _db.CrearComando(@"SELECT nombre as impuesto, tasadeTraslado as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeTraslado IS NOT NULL AND tasadeRetencion IS NULL AND idComprobante IN (@id)");
                            _db.AsignarParametroFlotante("@id", comprobantes);
                            dr = _db.EjecutarConsulta();
                            while (dr.Read())
                            {
                                txt.AgregaTrasladoLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                            }
                            _db.Desconectar();
                        }

                        #endregion
                        #region Datos Comprobante

                        var serie = ddlSerie.SelectedItem.Text;
                        var ambiente = tbAmbiente.Text.Equals("PRODUCCIÓN", StringComparison.OrdinalIgnoreCase);
                        var folio = "";
                        var fecha = Localization.Now.ToString("s");
                        var formapago = tbFormaPago.Text;
                        var condicionesDePago = tbCondPago.Text.Trim();
                        var subTotal = tbSubtotal.Text;
                        var totalDescuento = tbDescuento.Text;
                        var motivoDescuento = "";
                        var tipoCambio = "1.0";
                        var moneda = "MXN";
                        var total = tbTotalFac.Text;
                        var tipoDoc = "ingreso";
                        var codDoc = "01";
                        var metodoPago = Session["CfdiVersion"].ToString().Equals("3.3") ? ddlMetodoPago.SelectedValue : tbMetodoPago.Text;
                        var lugarExpedicion = tbLugarExp.Text;
                        var numCtaPago = tbNoCta.Text;
                        var otrosCargos = tbOtrosCargos.Text;
                        var importeAPagar = tbTotal.Text;
                        var observaciones = tbObservaciones.Text;

                        if (idComprobantes.Count == 1)
                        {
                            _db.Conectar();
                            _db.CrearComando("SELECT folio FROM Dat_General WHERE idComprobante = @id");
                            _db.AsignarParametroCadena("@id", idComprobantes.FirstOrDefault());
                            dr = _db.EjecutarConsulta();
                            if (dr.Read())
                            {
                                folio = dr["folio"].ToString();
                            }
                            _db.Desconectar();
                        }

                        decimal.TryParse(total, out totalOriginal);
                        decimal.TryParse(subTotal, out dSubTotal);
                        decimal.TryParse(totalDescuento, out dDescuentos);
                        totalCalculado = dSubTotal + dImpuestrosT - dImpuestrosR - dDescuentos + dImpuestrosTl - dImpuestrosRl;
                        var diferencia = Math.Abs(totalCalculado - totalOriginal);
                        var sTotalCalculado = ControlUtilities.CerosNull(totalCalculado.ToString());
                        if (diferencia <= (decimal)0.1)
                        {
                            total = sTotalCalculado;
                        }
                        txt.SetComprobanteCfdi(serie, folio, fecha, formapago, condicionesDePago, subTotal, totalDescuento, motivoDescuento, tipoCambio, moneda, total, tipoDoc, metodoPago, lugarExpedicion, numCtaPago, "", "", "", "", "", "", otrosCargos, importeAPagar, observaciones, "", "", "", "", chkCredito.Checked);

                        if (panelHospedaje.Style["display"].Equals("block", StringComparison.OrdinalIgnoreCase))
                        {
                            var propina = "0.00";
                            try
                            {
                                _db.Conectar();
                                _db.CrearComando("select propina from Dat_General where idComprobante = @id");
                                _db.AsignarParametroCadena("@id", idComprobantes.FirstOrDefault());
                                dr = _db.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    propina = dr[0].ToString();
                                }
                                _db.Desconectar();
                            }
                            catch (Exception ex)
                            {
                                // ignored
                            }
                            DateTime fechaLlegada;
                            DateTime fechaSalida;
                            if (DateTime.TryParse(tbLlegadaHuesped.Text, out fechaLlegada) && DateTime.TryParse(tbSalidaHuesped.Text, out fechaSalida))
                            {
                                txt.SetInfoAdicionalHabitacionCfdi(tbNombrehuesped.Text, tbConfirmacion.Text, tbHabitacionHuesped.Text, fechaLlegada.ToString("dd/MM/yyyy"), fechaSalida.ToString("dd/MM/yyyy"), "", "", propina);
                            }
                            else
                            {
                                txt.SetInfoAdicionalHabitacionCfdi(tbNombrehuesped.Text, tbConfirmacion.Text, tbHabitacionHuesped.Text, tbLlegadaHuesped.Text, tbSalidaHuesped.Text, "", "", propina);
                            }
                        }

                        #endregion

                        var parametros = new Config.Parametros(Session["IDENTEMI"].ToString());
                        var omitirSleepKey = "0";
                        var omitirSleep = false;
                        try
                        {
                            omitirSleepKey = parametros.GetParametroByNombre("OmitirSleep").Status;
                            omitirSleep = omitirSleepKey.Equals("1");
                        }
                        catch { }

                        if (!omitirSleep)
                        {
                            var randomMs = new Random().Next(1000, 5000);
                            System.Threading.Thread.Sleep(randomMs);
                        }

                        var txtInvoice = txt.ConstruyeTxtCfdi();
                        var coreMx = new wsEmision.WsEmision { Timeout = Timeout.Infinite };
                        var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, codDoc, false, true, "semi-automatico", folio);
                        if (result != null)
                        {
                            var xDoc = new XmlDocument();
                            xDoc.LoadXml(result.OuterXml);
                            if (xDoc.DocumentElement.Name.Equals("cfdi:Comprobante", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Eliminar Pendientes

                                _db.Conectar();
                                _db.CrearComando("DELETE FROM Dat_HabitacionEvento WHERE id_Comprobante IN (@id)");
                                _db.AsignarParametroFlotante("@id", comprobantes);
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                                _db.Conectar();
                                _db.CrearComando(@"DELETE FROM Dat_General WHERE idComprobante IN (@id)");
                                _db.AsignarParametroFlotante("@id", comprobantes);
                                _db.EjecutarConsulta();
                                _db.Desconectar();

                                #endregion
                                Session["uuidCreado"] = GetAtributte(xDoc, "UUID", "tfd:TimbreFiscalDigital");
                                (Master as SiteMaster).MostrarAlerta(this, "El comprobante se creó correctamente.", 2, null, "bootbox.hideAll();$('#divFacturar').modal('hide'); window.location.href = '" + ResolveClientUrl("~/Documentos.aspx") + "';", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                            }
                        }
                        else
                        {
                            throw new Exception(coreMx.ObtenerMensaje(), new Exception(coreMx.ObtenerMensajeTecnico()));
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se procesó correctamente.<br/><br/>" + ex.Message, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante no existe.", 4, null, "bootbox.hideAll();$('#divFacturar').modal('hide');", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                }
            }
            else
            {
                var errored = this.Validators.Cast<IValidator>().Where(v => !v.IsValid).ToList();
                (Master as SiteMaster).MostrarAlerta(this, "Verifique que todos los datos de la factura sean correctos.", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            }
        }
        /// <summary>
        /// Handles the Click event of the bFacturar2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="Exception">
        /// </exception>
        protected void bFacturar2_Click(object sender, EventArgs e)
        {
            var lb = (LinkButton)sender;
            _idComprobante = lb.CommandArgument;
            if (!string.IsNullOrEmpty(_idComprobante))
            {
                try
                {
                    var sql = "";
                    decimal iva16 = 0;
                    decimal propina = 0;
                    DbDataReader dr;
                    var sAmbiente = "1";
                    var conceptos = new List<object[]>();
                    var txt = new SpoolMx();
                    var codDoc = "";
                    var manual = true;
                    var folioReservacion = "";
                    var subtotal = "";
                    sql = @"SELECT
                            p.formapago, p.condicionesDePago, g.subTotal, g.totalDescuento, g.motivoDescuento, g.tipoCambio, g.moneda, g.total, g.tipoDeComprobante as tipoDoc, g.metodoPago, g.lugarExpedicion, p.numCtaPago, g.numeroAutorizacion, g.serie, g.fecha, g.total, g.codDoc, e.NOMEMI, e.RFCEMI, e.curp as curpE, e.telefono as telE, e.email as mailE, e.EmpresaTipo as etipoE, e.regimenFiscal as regimenE, e.obligadoContabilidad as contE, e.dirMatriz AS calleE, e.noExterior as extE, e.noInterior as intE, e.colonia as colE, e.localidad as locE, e.referencia as refE, e.municipio as munE, e.estado as edoE, e.pais as paisE, e.codigoPostal as cpE, d.dirEstablecimientos as calleExp, d.noExterior as extExp, d.noInterior as intExp, d.colonia as colExp, d.localidad as locExp, d.referencia as refExp, d.municipio as munExp, d.estado as esoExp, d.pais as paisExp, d.codigoPostal as cpExp, r.NOMREC, r.RFCREC, r.curp as curpR, r.telefono as telR, r.email as mailR, r.telefono2 as tel2R, r.denominacionSocial as denR, r.obligadoContabilidad as contR, r.domicilio as calleR, r.noExterior as extR, r.noInterior as intR, r.colonia as colR, r.localidad as locR, r.referencia as refR, r.municipio as munR, r.estado as edoR, r.pais as paisR, r.codigoPostal as cpR, IVA12 as iva16, g.propina, g.ambiente, g.idComprobante, g.importeAPagar, g.cargoxservicio as otrosCargos, g.observaciones, he.tipo AS tipoHE, g.folioReservacion, he.noHabitacion, he.fechaLlegada, he.fechaSalida, he.huesped, g.folio, t.tipo AS tipoEmision, g.estab AS claveSucursal
                        FROM
                            Dat_General g INNER JOIN
                            Cat_Emisor e ON g.id_Emisor = e.IDEEMI INNER JOIN
                            Dat_DOMEMIEXP d ON g.id_EmisorExp = d.IDEDOMEMIEXP INNER JOIN
                            Cat_Receptor r ON g.id_Receptor = r.IDEREC INNER JOIN
							Log_Trama t ON g.idTrama = t.idTrama INNER JOIN
                            Dat_Pagos p ON g.idComprobante = p.id_Comprobante LEFT OUTER JOIN
							Dat_HabitacionEvento he ON he.id_Comprobante = g.idComprobante
                        WHERE g.idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        subtotal = dr["subTotal"].ToString();
                        codDoc = dr["codDoc"].ToString();
                        manual = dr["tipoEmision"].ToString().Equals("2");
                        txt.SetComprobanteCfdi(dr["serie"].ToString(), dr["folio"].ToString(), Localization.Now.ToString("s"), dr["formapago"].ToString(), dr["condicionesDePago"].ToString(), dr["subTotal"].ToString(), dr["totalDescuento"].ToString(), dr["motivoDescuento"].ToString(), dr["tipoCambio"].ToString(), dr["moneda"].ToString(), dr["total"].ToString(), dr["tipoDoc"].ToString(), dr["metodoPago"].ToString(), dr["lugarExpedicion"].ToString(), dr["numCtaPago"].ToString(), dr["numeroAutorizacion"].ToString(), dr["serie"].ToString(), Localization.Parse(dr["fecha"].ToString()).ToString("s"), dr["total"].ToString(), dr["codDoc"].ToString(), "", dr["otrosCargos"].ToString(), dr["importeAPagar"].ToString(), tbObservaciones.Text);
                        txt.SetEmisorCfdi(dr["NOMEMI"].ToString(), dr["RFCEMI"].ToString(), dr["curpE"].ToString(), dr["telE"].ToString(), dr["mailE"].ToString(), dr["etipoE"].ToString(), dr["regimenE"].ToString(), dr["contE"].ToString());
                        txt.SetEmisorDomCfdi(dr["calleE"].ToString(), dr["extE"].ToString(), dr["intE"].ToString(), dr["colE"].ToString(), dr["locE"].ToString(), dr["refE"].ToString(), dr["munE"].ToString(), dr["edoE"].ToString(), dr["paisE"].ToString(), dr["cpE"].ToString());
                        txt.SetEmisorExpCfdi(dr["calleExp"].ToString(), dr["extExp"].ToString(), dr["intExp"].ToString(), dr["colExp"].ToString(), dr["locExp"].ToString(), dr["refExp"].ToString(), dr["munExp"].ToString(), dr["esoExp"].ToString(), dr["paisExp"].ToString(), dr["cpExp"].ToString(), dr["claveSucursal"].ToString());
                        txt.SetReceptorCfdi(dr["NOMREC"].ToString(), dr["RFCREC"].ToString(), dr["curpR"].ToString(), dr["telR"].ToString(), dr["mailR"].ToString(), dr["tel2R"].ToString(), tbDenomSocialRec.Text, dr["contR"].ToString());
                        if (cbDomRec.Checked)
                        {
                            var isSucursal = !ddlSucRec.SelectedValue.Equals("0");
                            txt.SetReceptorDomCfdi(tbCalleRec.Text, tbNoExtRec.Text, tbNoIntRec.Text, tbColoniaRec.Text, dr["locR"].ToString(), dr["refR"].ToString(), tbMunicipioRec.Text, tbEstadoRec.Text, tbPaisRec.Text, tbCpRec.Text, isSucursal);
                        }
                        folioReservacion = dr["folioReservacion"].ToString();
                        decimal.TryParse(CerosNull(dr["iva16"].ToString()), out iva16);
                        decimal.TryParse(CerosNull(dr["propina"].ToString()), out propina);
                        sAmbiente = dr["ambiente"].ToString();
                        if (!(dr["tipoHE"] is DBNull) && dr["tipoHE"] != null && !string.IsNullOrEmpty(dr["tipoHE"].ToString()))
                        {
                            switch (dr["tipoHE"].ToString())
                            {
                                case "1":
                                    txt.SetInfoAdicionalHabitacionCfdi(dr["huesped"].ToString(), folioReservacion, dr["noHabitacion"].ToString(), dr["fechaLlegada"].ToString(), dr["fechaSalida"].ToString());
                                    break;
                                case "2":
                                    txt.SetInfoAdicionalEventoCfdi(dr["huesped"].ToString(), dr["folioReservacion"].ToString(), dr["fechaLlegada"].ToString());
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    _db.Desconectar();
                    sql = @"SELECT
	                         ISNULL(SUM(it.valor), 0.00) as impTras, ISNULL(SUM(ir.valorRetenido), 0.00) as impRets
                        FROM
	                        Dat_General g INNER JOIN
							Dat_TotalConImpuestos it ON it.id_Comprobante = g.idComprobante INNER JOIN
							Dat_ImpuestosRetenciones ir ON ir.numDocSustento = g.idComprobante
						WHERE
							g.idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        txt.SetCantidadImpuestosCfdi(dr["impRets"].ToString(), dr["impTras"].ToString(), CerosNull(iva16.ToString()));
                    }
                    _db.Desconectar();
                    sql = @"SELECT
	                        ir.tipo as impuesto, ir.valorRetenido as importe
                        FROM
	                        Dat_ImpuestosRetenciones ir INNER JOIN
	                        Dat_General g on ir.numDocSustento = g.idComprobante
						WHERE
							g.idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        txt.AgregaImpuestoRetencionCfdi(dr["impuesto"].ToString(), dr["importe"].ToString());
                    }
                    _db.Desconectar();
                    sql = @"SELECT
	                        CASE
		                        it.codigo
		                        WHEN '1' THEN 'IVA'
		                        WHEN '2' THEN 'IEPS'
	                        END AS impuesto, it.tarifa as tasa, it.valor as importe
                        FROM
	                        Dat_TotalConImpuestos it INNER JOIN
	                        Dat_General g on it.id_Comprobante = g.idComprobante
						WHERE
							g.idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        txt.AgregaImpuestoTrasladoCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                    }
                    _db.Desconectar();
                    sql = "SELECT idDetalles, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial FROM Dat_Detalles INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        var sqlResults = new object[dr.FieldCount];
                        dr.GetValues(sqlResults);
                        conceptos.Add(sqlResults);
                    }
                    _db.Desconectar();
                    foreach (var concepto in conceptos)
                    {
                        txt.AgregaConceptoCfdi(concepto[4].ToString(), concepto[6].ToString(), concepto[1].ToString(), concepto[2].ToString(), concepto[3].ToString(), concepto[5].ToString());
                    }
                    if (propina > 0)
                    {
                        txt.SetInfoAdicionalRestauranteCfdi(CerosNull(propina.ToString()), folioReservacion);
                    }
                    sql = "SELECT idImpLocal FROM Dat_MX_ImpLocales INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
                    dr = _db.EjecutarConsulta();
                    var tieneLocales = dr.HasRows;
                    _db.Desconectar();
                    if (tieneLocales)
                    {
                        sql = @"SELECT totalRetImpLocales, totalTraImpLocales FROM Dat_General WHERE idComprobante = @id";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            txt.SetImpuestosLocalesCfdi(dr["totalRetImpLocales"].ToString(), dr["totalTraImpLocales"].ToString());
                        }
                        _db.Desconectar();
                        sql = "SELECT nombre as impuesto, tasadeRetencion as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeRetencion IS NOT NULL AND tasadeTraslado IS NULL AND idComprobante = @id";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            txt.AgregaRetencionLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                        }
                        _db.Desconectar();
                        sql = "SELECT nombre as impuesto, tasadeTraslado as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeTraslado IS NOT NULL AND tasadeRetencion IS NULL AND idComprobante = @id";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            txt.AgregaTrasladoLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                        }
                        _db.Desconectar();
                    }
                    var ambiente = false;
                    switch (sAmbiente)
                    {
                        case "1":
                            ambiente = false;
                            break;
                        case "2":
                            ambiente = true;
                            break;
                        default:
                            break;
                    }
                    var txtInvoice = "";
                    switch (codDoc)
                    {
                        case "01":
                        case "04":
                        case "06":
                        case "08":
                        case "09":
                            txtInvoice = txt.ConstruyeTxtCfdi();
                            break;
                        case "07":
                            txtInvoice = txt.ConstruyeTxtRetenciones();
                            break;
                    }
                    var coreMx = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                    var result = coreMx.ComprobantePaquete(_idComprobante, txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, codDoc, !manual, manual);
                    if (result != null)
                    {
                        var xDoc = new XmlDocument();
                        xDoc.LoadXml(result.OuterXml);
                        Session["uuidCreado"] = GetAtributte(xDoc, "UUID", "tfd:TimbreFiscalDigital");
                        (Master as SiteMaster).MostrarAlerta(this, "El comprobante se procesó correctamente.", 2, null, "bootbox.hideAll();$('#divFacturar').modal('hide'); window.location.href = '" + ResolveClientUrl("~/Documentos.aspx") + "';", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                    else
                    {
                        throw new Exception(coreMx.ObtenerMensaje(), new Exception(coreMx.ObtenerMensajeTecnico()));
                    }
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante no procesó correctamente.<br/><br/>" + ex.Message, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no existe.", 4, null, "bootbox.hideAll();$('#divFacturar').modal('hide');", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            }
        }

        /// <summary>
        /// Gets the atributte.
        /// </summary>
        /// <param name="XmlTimbrado">The XML timbrado.</param>
        /// <param name="name">The name.</param>
        /// <param name="node">The node.</param>
        /// <returns>System.String.</returns>
        private string GetAtributte(XmlDocument XmlTimbrado, string name, string node)
        {
            string result = null;
            if (XmlTimbrado != null)
            {
                XmlNodeList listaNodos = XmlTimbrado.GetElementsByTagName(node);
                foreach (XmlElement nodo in listaNodos)
                {
                    if (nodo.HasAttributes && nodo.HasAttribute(name))
                    {
                        result = nodo.GetAttribute(name);
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LlenarGrids();
            gvConceptos.PageIndex = e.NewPageIndex;
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRfcRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tbRfcRec_TextChanged(object sender, EventArgs e)
        {
            Llenarlista(tbRfcRec.Text, true);
        }

        /// <summary>
        /// Llenarlistas the specified identifier record.
        /// </summary>
        /// <param name="idRec">The identifier record.</param>
        /// <param name="isRfc">if set to <c>true</c> [is RFC].</param>
        private void Llenarlista(string idRec, bool isRfc = false)
        {
            var sql = @"SELECT [RFCREC]
                              ,[NOMREC]
                              ,[telefono]
                              ,[contribuyenteEspecial]
                              ,[obligadoContabilidad]
                              ,[tipoIdentificacionComprador]
                              ,[email]
                              ,[curp]
                              ,(CASE ISNULL(CONVERT(VARCHAR, metodoPago), '') WHEN '' THEN '99' ELSE ISNULL(cc.codigo, '99') END) AS metodoPago
                              ,[numCtaPago]
                              ,[denominacionSocial]
                              ,[telefono2]
                          FROM [Cat_Receptor] LEFT OUTER JOIN Cat_Catalogo1_C cc ON CONVERT(VARCHAR, metodoPago) = cc.codigo AND cc.tipo = 'MetodoPago'
                          WHERE " + (!isRfc ? "(IDEREC=@id)" : "(RFCREC=@rfc)");
            _db.Conectar();
            _db.CrearComando(sql);
            if (isRfc)
            {
                _db.AsignarParametroCadena("@rfc", idRec);
            }
            else
            {
                _db.AsignarParametroCadena("@id", idRec);
            }
            var dr = _db.EjecutarConsulta();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tbRfcRec.Text = dr["RFCREC"].ToString();
                    tbRazonSocialRec.Text = dr["NOMREC"].ToString();
                    tbDenomSocialRec.Text = dr["denominacionSocial"].ToString();
                    tbMail.Text = dr["email"].ToString();
                }
            }
            else
            {
                tbRazonSocialRec.Text = "";
                tbDenomSocialRec.Text = "";
                tbMail.Text = "";
            }
            _db.Desconectar();
            //Llenarlistadom(tbRfcRec.Text);
            LlenarlistadomId(idRec);
            if (tbRfcRec.Text.Length >= 12)
            {
                var tipoPersona = "%" + (tbRfcRec.Text.Length == 12 ? "F" : (tbRfcRec.Text.Length == 12 ? "M" : "FM")) + "%";
                SqlDataSourceUsoCfdi.SelectParameters["tipoPersonaUsoCfdi"].DefaultValue = tipoPersona;
                ddlUsoCfdi.DataBind();
                ddlUsoCfdi.Enabled = true;

            }
            else
            {
                ddlUsoCfdi.Enabled = false;
            }
            if (ddlUsoCfdi.Enabled && Session["IDENTEMI"].ToString().Equals("TCA130827M58"))
            {
                ddlUsoCfdi.SelectedValue = "P01";
            }
            else
            {
                ddlUsoCfdi.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Llenarlistadoms the specified RFC record.
        /// </summary>
        /// <param name="rfcRec">The RFC record.</param>
        /// <param name="chkDom">if set to <c>true</c> [CHK DOM].</param>
        private void Llenarlistadom(string rfcRec, bool chkDom = true)
        {
            var sql = @"SELECT [domicilio]
                              ,[noExterior]
                              ,[noInterior]
                              ,[colonia]
                              ,[localidad]
                              ,[referencia]
                              ,[municipio]
                              ,[estado]
                              ,[pais]
                              ,[codigoPostal]
                          FROM [Cat_Receptor]
                          WHERE RFCREC=@rfc";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@rfc", rfcRec);
            var dr = _db.EjecutarConsulta();
            var control = dr.HasRows || !cbDomRec.Checked;
            tbCalleRec.ReadOnly = !cbDomRec.Checked;
            tbNoExtRec.ReadOnly = !cbDomRec.Checked;
            tbNoIntRec.ReadOnly = !cbDomRec.Checked;
            tbColoniaRec.ReadOnly = !cbDomRec.Checked;
            //tbLocRec.ReadOnly = !cbDomRec.Checked;
            //tbRefRec.ReadOnly = !cbDomRec.Checked;
            tbMunicipioRec.ReadOnly = !cbDomRec.Checked;
            tbEstadoRec.ReadOnly = !cbDomRec.Checked;
            tbPaisRec.ReadOnly = !cbDomRec.Checked;
            tbCpRec.ReadOnly = !cbDomRec.Checked;
            if (!cbDomRec.Checked)
            {
                btnPaisRec.Attributes["disabled"] = "disabled";
            }
            else
            {
                btnPaisRec.Attributes.Remove("disabled");
            }
            if (control && dr.Read())
            {
                tbCalleRec.Text = dr["domicilio"].ToString();
                tbNoExtRec.Text = dr["noExterior"].ToString();
                tbNoIntRec.Text = dr["noInterior"].ToString();
                tbColoniaRec.Text = dr["colonia"].ToString();
                //tbLocRec.Text = dr["localidad"].ToString();
                //tbRefRec.Text = dr["referencia"].ToString();
                tbMunicipioRec.Text = dr["municipio"].ToString();
                tbEstadoRec.Text = dr["estado"].ToString();
                tbPaisRec.Text = dr["pais"].ToString();
                try
                {
                    if (Regex.IsMatch(tbPaisRec.Text, @"M.XICO|MX|M.X", RegexOptions.IgnoreCase))
                    {
                        ddlPais.SelectedValue = "MEX";
                    }
                    else
                    {
                        var firstLetters = tbPaisRec.Text.Substring(0, 3);
                        ddlPais.SelectedValue = firstLetters.ToUpper();
                    }
                }
                catch (Exception ex) { }
                tbCpRec.Text = dr["codigoPostal"].ToString();
                _db.Desconectar();
                _db.Conectar();
                _db.CrearComando(@"SELECT idSucursal, sucursal FROM Cat_Sucursales WHERE RFC = @RFC");
                _db.AsignarParametroCadena("@RFC", rfcRec);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    ddlSucRec.Items.Add(new ListItem(dr["sucursal"].ToString(), dr["idSucursal"].ToString()));
                }
                _db.Desconectar();
                if (chkDom)
                {
                    cbDomRec.Checked = true;
                    cbDomRec_CheckedChanged(null, null);
                }
            }
            else
            {
                _db.Desconectar();
                if (chkDom)
                {
                    cbDomRec.Checked = false;
                    cbDomRec_CheckedChanged(null, null);
                }
                tbCalleRec.Text = "";
                tbNoExtRec.Text = "";
                tbNoIntRec.Text = "";
                tbColoniaRec.Text = "";
                //tbLocRec.Text = "";
                //tbRefRec.Text = "";
                tbMunicipioRec.Text = "";
                tbEstadoRec.Text = "";
                tbPaisRec.Text = "";
                tbCpRec.Text = "";
            }
        }

        private void LlenarlistadomId(string idRec, bool chkDom = true)
        {
            var sql = @"SELECT [domicilio]
                              ,[noExterior]
                              ,[noInterior]
                              ,[colonia]
                              ,[localidad]
                              ,[referencia]
                              ,[municipio]
                              ,[estado]
                              ,[pais]
                              ,[codigoPostal]
                              ,[RFCREC]
                          FROM [Cat_Receptor]
                          WHERE IDEREC=@idRec";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idRec", idRec);
            var dr = _db.EjecutarConsulta();
            var control = dr.HasRows || !cbDomRec.Checked;
            tbCalleRec.ReadOnly = !cbDomRec.Checked;
            tbNoExtRec.ReadOnly = !cbDomRec.Checked;
            tbNoIntRec.ReadOnly = !cbDomRec.Checked;
            tbColoniaRec.ReadOnly = !cbDomRec.Checked;
            //tbLocRec.ReadOnly = !cbDomRec.Checked;
            //tbRefRec.ReadOnly = !cbDomRec.Checked;
            tbMunicipioRec.ReadOnly = !cbDomRec.Checked;
            tbEstadoRec.ReadOnly = !cbDomRec.Checked;
            tbPaisRec.ReadOnly = !cbDomRec.Checked;
            tbCpRec.ReadOnly = !cbDomRec.Checked;
            if (!cbDomRec.Checked)
            {
                btnPaisRec.Attributes["disabled"] = "disabled";
            }
            else
            {
                btnPaisRec.Attributes.Remove("disabled");
            }
            if (control && dr.Read())
            {
                var rfcRec = dr["RFCREC"].ToString();
                tbCalleRec.Text = dr["domicilio"].ToString();
                tbNoExtRec.Text = dr["noExterior"].ToString();
                tbNoIntRec.Text = dr["noInterior"].ToString();
                tbColoniaRec.Text = dr["colonia"].ToString();
                //tbLocRec.Text = dr["localidad"].ToString();
                //tbRefRec.Text = dr["referencia"].ToString();
                tbMunicipioRec.Text = dr["municipio"].ToString();
                tbEstadoRec.Text = dr["estado"].ToString();
                tbPaisRec.Text = dr["pais"].ToString();
                try
                {
                    if (Regex.IsMatch(tbPaisRec.Text, @"M.XICO|MX|M.X", RegexOptions.IgnoreCase))
                    {
                        ddlPais.SelectedValue = "MEX";
                    }
                    else
                    {
                        var firstLetters = tbPaisRec.Text.Substring(0, 3);
                        ddlPais.SelectedValue = firstLetters.ToUpper();
                    }
                }
                catch (Exception ex) { }
                tbCpRec.Text = dr["codigoPostal"].ToString();
                _db.Desconectar();
                _db.Conectar();
                _db.CrearComando(@"SELECT idSucursal, sucursal FROM Cat_Sucursales WHERE RFC = @RFC");
                _db.AsignarParametroCadena("@RFC", rfcRec);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    ddlSucRec.Items.Add(new ListItem(dr["sucursal"].ToString(), dr["idSucursal"].ToString()));
                }
                _db.Desconectar();
                if (chkDom)
                {
                    cbDomRec.Checked = true;
                    cbDomRec_CheckedChanged(null, null);
                }
            }
            else
            {
                _db.Desconectar();
                if (chkDom)
                {
                    cbDomRec.Checked = false;
                    cbDomRec_CheckedChanged(null, null);
                }
                tbCalleRec.Text = "";
                tbNoExtRec.Text = "";
                tbNoIntRec.Text = "";
                tbColoniaRec.Text = "";
                //tbLocRec.Text = "";
                //tbRefRec.Text = "";
                tbMunicipioRec.Text = "";
                tbEstadoRec.Text = "";
                tbPaisRec.Text = "";
                tbCpRec.Text = "";
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbDomRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void cbDomRec_CheckedChanged(object sender, EventArgs e)
        {
            RequiredFieldValidator14.Enabled = cbDomRec.Checked;
            RequiredFieldValidator15.Enabled = cbDomRec.Checked;
            RequiredFieldValidator16.Enabled = cbDomRec.Checked;
            RequiredFieldValidator23.Enabled = cbDomRec.Checked;
            RequiredFieldValidator27.Enabled = cbDomRec.Checked;
            ddlSucRec.Items.Clear();
            var itemNull = new ListItem("SELECCIONE", "0");
            itemNull.Selected = true;
            ddlSucRec.Items.Add(itemNull);
            LlenarlistadomId(Session["IdReceptor"].ToString(), false);
            ddlSucRec.Enabled = ddlSucRec.Items.Count > 1;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSucRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlSucRec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlSucRec.SelectedValue.Equals("0"))
            {
                var id = ddlSucRec.SelectedValue;
                _db.Conectar();
                _db.CrearComando(@"SELECT * FROM Cat_Sucursales WHERE idSucursal = @ID");
                _db.AsignarParametroCadena("@ID", id);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbCalleRec.Text = dr["calle"].ToString();
                    tbNoExtRec.Text = dr["noExterior"].ToString();
                    tbNoIntRec.Text = dr["noInterior"].ToString();
                    tbColoniaRec.Text = dr["colonia"].ToString();
                    //tbLocRec.Text = dr["localidad"].ToString();
                    //tbRefRec.Text = dr["referencia"].ToString();
                    tbMunicipioRec.Text = dr["municipio"].ToString();
                    tbEstadoRec.Text = dr["estado"].ToString();
                    tbPaisRec.Text = dr["pais"].ToString();
                    tbCpRec.Text = dr["codigoPostal"].ToString();
                }
                _db.Desconectar();
            }
            else
            {
                LlenarlistadomId(Session["IdReceptor"].ToString());
            }
        }

        private KeyValuePair<bool, string> LoadData(List<string> idComprobantes)
        {
            ClearData();
            ddlSerie.Enabled = true;
            DbDataReader dr = null;
            var comprobantes = string.Join(",", idComprobantes);
            var sqlReceptor = @"SELECT 
                            SUM(g.subTotal) AS subTotal, SUM(g.totalDescuento) AS totalDescuento, SUM(g.total) AS total, SUM(g.propina) AS propina, SUM (g.cargoxservicio) AS otrosCargos, SUM(g.importeAPagar) AS importeAPagar, SUM (g.IVA12) AS iva, SUM(g.impuestoHopedaje) AS ish, ISNULL((SELECT DISTINCT TOP 1 valor FROM Cat_CatImpuestos_C WHERE descripcion LIKE '%ISH%'), 0) AS valor_ish, (SELECT TOP 1 s.estado + ', ' + s.pais FROM Cat_SucursalesEmisor s INNER JOIN Cat_Empleados e On e.id_Sucursal = s.idSucursal WHERE e.idEmpleado = @idEmpleado) AS lugarExpedicion, (SELECT TOP 1 IDEREC FROM Cat_Receptor WHERE RFCREC = 'XAXX010101000' ORDER BY IDEREC) AS IDEREC
                        FROM
                            Dat_General g
                        WHERE g.idComprobante IN (@id)";
            _db.Conectar();
            _db.CrearComando(sqlReceptor);
            _db.AsignarParametroCadena("@idEmpleado", _idUser);
            _db.AsignarParametroFlotante("@id", comprobantes);
            dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                var idRec = dr["IDEREC"].ToString();
                tbSubtotal.Text = CerosNull(dr["subTotal"].ToString());
                tbTotalFac.Text = CerosNull(dr["total"].ToString());
                tbPropina.Text = CerosNull(dr["propina"].ToString());
                tbDescuento.Text = CerosNull(dr["totalDescuento"].ToString());
                tbOtrosCargos.Text = CerosNull(dr["otrosCargos"].ToString());
                tbTotal.Text = CerosNull(dr["importeAPagar"].ToString());
                tbIva16.Text = CerosNull(dr["iva"].ToString());
                lblISHPrer.Text = CerosNull(dr["valor_ish"].ToString());
                tbISH.Text = CerosNull(dr["ish"].ToString());
                tbCodDoc.Text = "01: FACTURA";
                tbAmbiente.Text = "PRODUCCIÓN"; // HUEHUEHUE
                tbFormaPago.Text = "UNA SOLA EXHIBICIÓN";
                tbMetodoPago.Text = "99";
                ddlFormaPago.SelectedValue = "PUE";
                ddlMetodoPago.SelectedValue = "99";
                tbNoCta.Text = "NO IDENTIFICADO";
                var toText = new NumerosALetras();
                tbCantLetra.Text = toText.ConvertirALetras(tbTotal.Text, "MXN");
                tbLugarExp.Text = dr["lugarExpedicion"].ToString();
                _db.Desconectar();
                if (idComprobantes.Count == 1)
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT g.metodoPago, p.formapago, g.id_Receptor, g.observaciones, g.folioReservacion, h.tipo, h.huesped, h.fechaLlegada, h.fechaSalida, h.noHabitacion, h.tipohabitacion, g.serie from Dat_HabitacionEvento h RIGHT OUTER JOIN Dat_General g ON g.idComprobante = h.id_Comprobante INNER JOIN Dat_pagos p ON g.idComprobante = p.id_Comprobante WHERE g.idComprobante = @id");
                    _db.AsignarParametroCadena("@id", idComprobantes.FirstOrDefault());
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        tbFormaPago.Text = dr["formapago"].ToString();
                        tbMetodoPago.Text = dr["metodoPago"].ToString();
                        try
                        {
                            if (Regex.IsMatch(tbFormaPago.Text, @"sola exhibici.n|PUE", RegexOptions.IgnoreCase))
                            {
                                ddlFormaPago.SelectedValue = "PUE";
                            }
                            else if (Regex.IsMatch(tbFormaPago.Text, @"parcialidad|PPD", RegexOptions.IgnoreCase))
                            {
                                ddlFormaPago.SelectedValue = "PPD";
                            }
                            ddlFormaPago_SelectedIndexChanged(null, null);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        try
                        {
                            var metodoFirst = tbMetodoPago.Text.Split(',').FirstOrDefault().Trim();
                            ddlMetodoPago.SelectedValue = metodoFirst;
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        try
                        {
                            var finded = ddlSerie.Items.FindByText(dr["serie"].ToString());
                            ddlSerie.SelectedValue = finded.Value;
                            ddlSerie.Enabled = false;
                            if (Session["IDENTEMI"].ToString().Equals("OHC080924AV5"))
                            {
                                ddlSerie.Enabled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        idRec = dr["id_Receptor"].ToString();
                        tbObservaciones.Text = dr["observaciones"].ToString();
                        tbConfirmacion.Text = dr["folioReservacion"].ToString();
                        tbNombrehuesped.Text = dr["huesped"].ToString();
                        tbLlegadaHuesped.Text = dr["fechaLlegada"].ToString();
                        tbSalidaHuesped.Text = dr["fechaSalida"].ToString();
                        tbHabitacionHuesped.Text = dr["noHabitacion"].ToString();
                    }
                    _db.Desconectar();
                    if (!Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HAP9504215L5|HCC891030SX7|HRL141027IP2", RegexOptions.IgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(tbConfirmacion.Text) && IsConfirmacionFacturada(tbConfirmacion.Text))
                        {
                            return new KeyValuePair<bool, string>(false, $"La confirmación/reserva {tbConfirmacion.Text} ya se encuentra facturada.");
                        }
                    }
                }
                else
                {
                    tbObservaciones.Text = "";
                    tbConfirmacion.Text = "";
                    tbNombrehuesped.Text = "";
                    tbLlegadaHuesped.Text = "";
                    tbSalidaHuesped.Text = "";
                    tbHabitacionHuesped.Text = "";
                }
                Session["IdReceptor"] = idRec;
                Llenarlista(idRec);
                LlenarGridView(idComprobantes);
                tbConfirmacion.ReadOnly = !string.IsNullOrEmpty(tbConfirmacion.Text);
                ddlSerie_SelectedIndexChanged(null, null);
                if (Session["IDENTEMI"].ToString().Equals("OHC070227M80") || Session["IDENTEMI"].ToString().Equals("OPL000131DL3"))
                {
                    ddlFormaPago.SelectedValue = "PUE";
                    ddlFormaPago_SelectedIndexChanged(null, null);
                    ddlFormaPago.Enabled = false;
                }
                return new KeyValuePair<bool, string>(true, "");
            }
            else
            {
                _db.Desconectar();
                return new KeyValuePair<bool, string>(false, "No existe un comprobante no autorizado con los datos especificados.");
            }
        }

        private bool IsConfirmacionFacturada(string confirmacion)
        {
            var isFacturada = false;
            _db.Conectar();
            try
            {
                _db.CrearComando("SELECT idComprobante FROM Dat_General WHERE folioReservacion = @folio AND codDoc = '01' AND estado = 1 AND tipo = 'E'");
                _db.AsignarParametroCadena("@folio", confirmacion);
                var dr = _db.EjecutarConsulta();
                isFacturada = dr.Read();
                _db.Desconectar();
            }
            catch (Exception ex)
            {
                // ignored
            }
            finally
            {
                _db.Desconectar();
            }
            return isFacturada;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="idComprobante">The identifier comprobante.</param>
        /// <returns>KeyValuePair&lt;System.Boolean, System.String&gt;.</returns>
        private KeyValuePair<bool, string> LoadData(string idComprobante = "")
        {
            ClearData();
            var sqlReceptor = @"SELECT TOP 1
                            p.formapago, g.subTotal, g.totalDescuento, g.cantletras, g.total, (CONVERT(VARCHAR(MAX), g.metodoPago) + ': ' + ccMetodoPago.descripcion) AS metodoPago, g.lugarExpedicion, p.numCtaPago, g.numeroAutorizacion, g.serie, g.codDoc, ccCodDoc.descripcion AS descDoc, g.idComprobante, e.IDEEMI, r.IDEREC, CONVERT(VARCHAR,CAST((CASE WHEN g.IVA12 IS NULL OR g.IVA12 = 0.00 THEN (SELECT ISNULL(SUM(dt.valor), 0.00) FROM Dat_TotalConImpuestos dt INNER JOIN Cat_Catalogo1_C ci ON ci.codigo = dt.codigo AND ci.tipo = 'Impuesto Trasladado' AND ci.descripcion = 'IVA' AND dt.id_Comprobante = g.idComprobante) ELSE g.IVA12 END) AS money), 1) AS iva16, CONVERT(VARCHAR,CAST((SELECT ISNULL(SUM(dt2.importe), 0.00) FROM Dat_MX_ImpLocales dt2 WHERE dt2.nombre LIKE '%ISH%' AND dt2.id_Comprobante = g.idComprobante) AS money), 1) AS valor_ish, g.propina, g.ambiente, ccAmbiente.descripcion AS descAmbiente, g.idComprobante, g.importeAPagar, g.cargoxservicio as otrosCargos, g.observaciones, he.tipo AS tipoHE, g.folioReservacion, he.noHabitacion, he.fechaLlegada, he.fechaSalida, he.huesped, ISNULL(g.mensajeSAT, log.descripcion) AS mensajeSAT
                        FROM
                            Dat_General g INNER JOIN
							Cat_Catalogo1_C ccCodDoc ON g.codDoc = ccCodDoc.codigo AND ccCodDoc.tipo = 'Comprobante' INNER JOIN
							Cat_Catalogo1_C ccAmbiente ON g.ambiente = ccAmbiente.codigo AND ccAmbiente.tipo = 'Ambiente' INNER JOIN
							Cat_Catalogo1_C ccMetodoPago ON CONVERT(VARCHAR(MAX), g.metodoPago) = ccMetodoPago.codigo AND ccMetodoPago.tipo = 'MetodoPago' INNER JOIN
                            Cat_Emisor e ON g.id_Emisor = e.IDEEMI INNER JOIN
                            Dat_DOMEMIEXP d ON g.id_EmisorExp = d.IDEDOMEMIEXP INNER JOIN
                            Cat_Receptor r ON g.id_Receptor = r.IDEREC LEFT OUTER JOIN
                            Dat_Pagos p ON g.idComprobante = p.id_Comprobante LEFT OUTER JOIN
							Dat_HabitacionEvento he ON he.id_Comprobante = g.idComprobante LEFT OUTER JOIN
							Logs log ON (log.comprobanteAsociado = g.idComprobante OR log.tramaAsociada = g.idTrama)
                        WHERE (g.idComprobante = @id AND g.estado = '2' AND g.tipo = 'E')";
            _db.Conectar();
            _db.CrearComando(sqlReceptor);
            _db.AsignarParametroCadena("@id", idComprobante);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                var idRec = dr["IDEREC"].ToString();
                tbSubtotal.Text = CerosNull(dr["subTotal"].ToString());
                tbIva16.Text = CerosNull(dr["iva16"].ToString());
                lblISHPrer.Text = CerosNull(dr["valor_ish"].ToString());
                tbISH.Text = CerosNull(dr["valor_ish"].ToString());
                tbTotalFac.Text = CerosNull(dr["total"].ToString());
                tbPropina.Text = CerosNull(dr["propina"].ToString());
                tbDescuento.Text = CerosNull(dr["totalDescuento"].ToString());
                tbOtrosCargos.Text = CerosNull(dr["otrosCargos"].ToString());
                tbTotal.Text = CerosNull(dr["importeAPagar"].ToString());
                tbCodDoc.Text = dr["descDoc"].ToString();
                tbAmbiente.Text = dr["descAmbiente"].ToString();
                tbFormaPago.Text = dr["formapago"].ToString();
                tbMetodoPago.Text = dr["metodoPago"].ToString();
                tbNoCta.Text = dr["numCtaPago"].ToString();
                tbCantLetra.Text = dr["cantletras"].ToString();
                tbLugarExp.Text = dr["lugarExpedicion"].ToString();
                tbObservaciones.Text = dr["observaciones"].ToString();
                tbConfirmacion.Text = dr["folioReservacion"].ToString();
                tbNombrehuesped.Text = dr["huesped"].ToString();
                try { tbLlegadaHuesped.Text = DateTime.Parse(dr["fechaLlegada"].ToString()).ToString("dd/MM/yyyy"); } catch { }
                try { tbSalidaHuesped.Text = DateTime.Parse(dr["fechaSalida"].ToString()).ToString("dd/MM/yyyy"); } catch { }
                tbHabitacionHuesped.Text = dr["noHabitacion"].ToString();
                try
                {
                    ddlSerie.ClearSelection();
                    var index = ddlSerie.Items.FindByText(dr["serie"].ToString());
                    index.Selected = true;
                    ddlSerie.Enabled = false;
                }
                catch (Exception ex) { }
                _db.Desconectar();
                Session["IdReceptor"] = idRec;
                Llenarlista(idRec);
                LlenarGridView(idComprobante);
                _db.Desconectar();
                tbConfirmacion.ReadOnly = !string.IsNullOrEmpty(tbConfirmacion.Text);
                return new KeyValuePair<bool, string>(true, "");
            }
            else
            {
                _db.Desconectar();
                return new KeyValuePair<bool, string>(false, "No existe un comprobante no autorizado con los datos especificados.");
            }
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        private void ClearData()
        {
            Session["IdReceptor"] = "";
            Llenarlista("");
            LlenarGridView("");
            tbSubtotal.Text = "";
            tbIva16.Text = "";
            lblISHPrer.Text = "";
            tbISH.Text = "";
            tbTotalFac.Text = "";
            tbPropina.Text = "";
            tbDescuento.Text = "";
            tbOtrosCargos.Text = "";
            tbTotal.Text = "";
            tbCodDoc.Text = "";
            tbAmbiente.Text = "";
            tbFormaPago.Text = "";
            tbMetodoPago.Text = "";
            tbNoCta.Text = "";
            tbCantLetra.Text = "";
            tbLugarExp.Text = "";
            tbObservaciones.Text = "";
            tbCondPago.Text = "";
            tbMail.Text = "";
        }

        /// <summary>
        /// Llenars the grid view.
        /// </summary>
        /// <param name="idComprobante">The identifier comprobante.</param>
        private void LlenarGridView(string idComprobante)
        {
            if (SqlDataConcTemp.SelectParameters.Count > 0)
            {
                SqlDataConcTemp.SelectParameters["idComprobante"].DefaultValue = idComprobante;
            }
            else
            {
                SqlDataConcTemp.SelectParameters.Add("idComprobante", idComprobante);
            }
            SqlDataConcTemp.SelectCommand = comandoConcTemp;
            SqlDataConcTemp.DataBind();
            gvConceptos.DataBind();
            if (!string.IsNullOrEmpty(idComprobante))
            {
                var lista = GetImpuestosConceptoDataBase(new string[] { idComprobante });
                Session["_listaImpuestos"] = lista;
            }
            try
            {
                gvConceptos.Columns.Cast<DataControlField>().FirstOrDefault(c => c.HeaderText.Equals("IMPUESTOS", StringComparison.OrdinalIgnoreCase)).Visible = Session["CfdiVersion"].ToString().Equals("3.3");
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private void LlenarGridView(List<string> idComprobantes)
        {
            SqlDataConcTemp.SelectParameters.Clear();
            var ids = string.Join(",", idComprobantes);
            var comando = comandoConcTemp.Replace("(id_Comprobante = @idComprobante)", "id_Comprobante IN (" + ids + ")");
            SqlDataConcTemp.SelectCommand = comando;
            SqlDataConcTemp.DataBind();
            gvConceptos.DataBind();
            var lista = GetImpuestosConceptoDataBase(idComprobantes.ToArray());
            Session["_listaImpuestos"] = lista;
            try
            {
                gvConceptos.Columns.Cast<DataControlField>().FirstOrDefault(c => c.HeaderText.Equals("IMPUESTOS", StringComparison.OrdinalIgnoreCase)).Visible = Session["CfdiVersion"].ToString().Equals("3.3");
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the rblTipoEdicion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void rblTipoEdicion_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNuevoConcepto.Text = "";
            switch (rblTipoEdicion.SelectedValue)
            {
                case "1":

                    tbNuevoConcepto.ReadOnly = true;
                    if (!string.IsNullOrEmpty(_idComprobante))
                    {
                        gvConceptos.Columns[5].Visible = true;
                    }
                    else
                    {
                        gvConceptos.Columns[5].Visible = false;
                    }
                    divPaquete.Visible = false;
                    break;
                case "2":
                    tbNuevoConcepto.ReadOnly = false;
                    gvConceptos.Columns[5].Visible = false;
                    divPaquete.Visible = true;
                    break;
                default:
                    break;
            }
            LlenarGrids();
        }

        /// <summary>
        /// Handles the RowEditing event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewEditEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvConceptos.EditIndex = e.NewEditIndex;
            if (RowsChecked().Count > 0 && string.IsNullOrEmpty(_idComprobante))
            {
                LlenarGrids();
            }
            else
            {
                LlenarGridView(_idComprobante);
            }
        }

        /// <summary>
        /// Handles the RowUpdating event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewUpdateEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var idDetalles = gvConceptos.DataKeys[e.RowIndex].Value.ToString();
                var row = gvConceptos.Rows[e.RowIndex];
                var descripcion = (TextBox)row.FindControl("tbDescripcion");
                SqlDataConcTemp.UpdateParameters["idDetalles"].DefaultValue = idDetalles;
                SqlDataConcTemp.UpdateParameters["descripcion"].DefaultValue = descripcion.Text;
                SqlDataConcTemp.Update();
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El concepto no se pudo editar, inténtelo nuevamente.<br/><br/>" + ex.Message, 4);
            }
            finally
            {
                gvConceptos.EditIndex = -1;
                if (RowsChecked().Count > 0 && string.IsNullOrEmpty(_idComprobante))
                {
                    LlenarGrids();
                }
                else
                {
                    LlenarGridView(_idComprobante);
                }
                gvConceptos.EditIndex = -1;
            }
        }

        /// <summary>
        /// Handles the RowCancelingEdit event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvConceptos.EditIndex = -1;
            if (RowsChecked().Count > 0 && string.IsNullOrEmpty(_idComprobante))
            {
                LlenarGrids();
            }
            else
            {
                LlenarGridView(_idComprobante);
            }
        }

        /// <summary>
        /// Binds the conceptos.
        /// </summary>
        /// <param name="row">The row.</param>
        private void bindConceptos(GridViewRow row)
        {
            _db.Conectar();
            _db.CrearComando("SELECT DISTINCT CLASIFICACION FROM CatalogoConceptos");
            var dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                var li = new HtmlGenericControl("li");
                var ulDescConc = (HtmlGenericControl)row.FindControl("ulDescConc");
                var tbDescConc = (TextBox)row.FindControl("tbDescripcion");
                ulDescConc.Controls.Add(li);
                var anchor = new HtmlGenericControl("a");
                anchor.Attributes.Add("href", "#");
                anchor.InnerText = dr[0].ToString();
                anchor.Attributes.Add("onclick", "return changeText($(this).html(), '#" + tbDescConc.ClientID + "');");
                li.Controls.Add(anchor);
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    try
                    {
                        bindConceptos(e.Row);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the lbDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            var lb = (LinkButton)sender;
            _idComprobante = lb.CommandArgument;
            try
            {

                _db.Conectar();
                _db.CrearComando("DELETE FROM Dat_General WHERE idComprobante = @ID");
                _db.AsignarParametroCadena("@ID", _idComprobante);
                _db.EjecutarConsulta1();
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Ocurrió un error al eliminar el registro. Inténtelo de nuevo.<br/><br/>" + ex.Message, 4);
            }
            finally
            {
                _db.Desconectar();
                _idComprobante = "";
                Button1_Click1(null, null);
            }
        }

        #region CODIGO INE
        /// <summary>
        /// Handles the Click event of the LnkBAgregarIdentificador control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void LnkBAgregarIdentificador_Click(object sender, EventArgs e)
        {
            if (DDAmbito.SelectedValue.Equals("") && DDAmbito.Visible)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe definir un Ámbito para la Entidad", 4);
                return;
            }
            else if (DDTipoComite.SelectedValue.Equals("") && DDTipoComite.Visible)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe definir un Tipo de Comité", 4);
                return;
            }
            else if (DDTipo_Proceso.SelectedValue.Equals("") && DDTipo_Proceso.Visible)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe definir un Tipo de Proceso", 4);
                return;
            }

            ////////////////////// VERIFICAR SI EL IDCONTABILIDAD YA EISTE ////////////////////////
            //bool idContaExist = false;
            //if (divIdConta.Visible && TextboxIdentificador.Text != "")
            //{
            //    if (Array_registros.Count > 0)
            //    {
            //        foreach (string[] registro in Array_registros)
            //        {
            //            if (registro[1] == TextboxIdentificador.Text &&
            //               registro[4] == DDAmbito.SelectedItem.Value)
            //                idContaExist = true;
            //        }
            //    }
            //}
            ////////////////////// FIN VERIFICAR SI EL IDCONTABILIDAD YA EISTE /////////////////////

            ////////////////////// VERIFICAR SI LA ENTIDAD YA EISTE ////////////////////////
            bool EntidadExist = false;
            if (divIdConta.Visible)
            {
                if (Array_registros.Count > 0)
                {
                    foreach (string[] registro in Array_registros)
                    {
                        if (registro[2] == DDEstado.SelectedValue)
                            EntidadExist = true;
                    }
                }
            }
            ////////////////////// FIN VERIFICAR SI LA ENTIDAD YA EISTE /////////////////////

            //PRUEBAS
            //idContaExist = false;
            //PRUEBAS
            if (!EntidadExist)
            {
                registro = new string[8];
                registro[0] = id_Renglon.ToString();
                id_Renglon++;
                registro[1] = divIdContaG.Visible == true ? TextboxIdentificador.Text : "";
                registro[2] = divEntidad.Visible == true ? DDEstado.SelectedItem.Text : "";
                registro[3] = divEntidad.Visible == true == true ? DDEstado.SelectedItem.Value : "";
                /*Agregar Columnas Tipo Proceso, Tipo Comité, Ambito al gridview*/
                registro[4] = divAmbito.Visible == true ? DDAmbito.SelectedItem.Value : "";
                registro[5] = divComite.Visible == true ? DDTipoComite.SelectedItem.Value : "";
                registro[6] = DDTipo_Proceso.SelectedItem.Value != "" ? DDTipo_Proceso.SelectedItem.Value : "";
                registro[7] = divIdConta.Visible == true ? tbIdConta.Text : "";
                Array_registros.Add(registro);
                crear_tabla_INE();
                if (Array_registros.Count > 0)
                {
                    foreach (string[] registro in Array_registros)
                    {
                        DataRow row = IdentificadoresTable.NewRow();
                        row["No"] = registro[0];
                        row["Identificador"] = registro[1];
                        row["Estado"] = registro[2];
                        row["Prefijo"] = registro[3];
                        /*Agregar Columnas Tipo Proceso, Tipo Comité, Ambito al gridview*/
                        row["Ámbito"] = registro[4];
                        row["Tipo Comité"] = registro[5];
                        row["Tipo Proceso"] = registro[6];
                        row["IdContabilidad"] = registro[7];
                        IdentificadoresTable.Rows.Add(row);
                        gvRegistros.DataSource = IdentificadoresTable;
                        gvRegistros.DataBind();

                    }
                }
                //TextboxIdentificador.Text = "";
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "La entidad ya se ha agregado", 4);
            }
        }
        /// <summary>
        /// Handles the DataBound event of the gvRegistros control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gvRegistros_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                //Ocultar celdas de No de renglon e identificador de estado
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[5].Visible = false;

                //No permitir modificar datos
                DDTipoComite.Enabled = false;
                DDTipo_Proceso.Enabled = false;
                TextboxIdentificador.ReadOnly = true;
            }
            else
            {
                DDTipoComite.Enabled = true;
                DDTipo_Proceso.Enabled = true;
                TextboxIdentificador.ReadOnly = false;
            }
        }
        /// <summary>
        /// Handles the Click event of the btnEliminar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs" /> instance containing the event data.</param>
        protected void btnEliminar_Click(object sender, GridViewCommandEventArgs e)
        {
            ArrayList Array_registros_aux = (ArrayList)Array_registros.Clone();
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "SelectRow")
            {
                Array_registros_aux = (ArrayList)Array_registros.Clone();
                foreach (string[] registro in Array_registros)
                {
                    if (registro[0].ToString() == index.ToString())
                    {
                        Array_registros_aux.Remove(registro);
                    }
                }
            }
            if (e.CommandName == "AddId")
            {
                int contador = 0;
                Array_registros_aux = new ArrayList();
                foreach (string[] registro in Array_registros)
                {
                    contador++;
                    if (registro[0].ToString() == index.ToString())
                    {
                        //AGREGAR EL PRIMER IDENTIFICADOR
                        registro[7] = string.IsNullOrEmpty(registro[7]) ? tbIdConta.Text : registro[7] + "," + tbIdConta.Text;
                        Array_registros_aux.Add(registro);
                    }
                    else
                    {
                        Array_registros_aux.Add(registro);
                    }
                }
                tbIdConta.Text = "";
            }
            crear_tabla_INE();
            Array_registros = Array_registros_aux;
            if (Array_registros.Count > 0)
            {
                foreach (string[] registro in Array_registros)
                {
                    DataRow row = IdentificadoresTable.NewRow();
                    row["No"] = registro[0];
                    row["Identificador"] = registro[1];
                    row["Estado"] = registro[2];
                    row["Prefijo"] = registro[3];
                    /*Agregar Columnas Tipo Proceso, Tipo Comité, Ambito al gridview*/
                    row["Ámbito"] = registro[4];
                    row["Tipo Comité"] = registro[5];
                    row["Tipo Proceso"] = registro[6];
                    row["IdContabilidad"] = registro[7];

                    IdentificadoresTable.Rows.Add(row);
                    gvRegistros.DataSource = IdentificadoresTable;
                    gvRegistros.DataBind();

                }
            }
            else
                gvRegistros.DataBind();
            CheckRegistros();
        }

        /// <summary>
        /// Checks the registros.
        /// </summary>
        private void CheckRegistros()
        {
            if (Array_registros.Count >= 1)
            {
                //No permitir modificar datos
                DDTipoComite.Enabled = false;
                DDTipo_Proceso.Enabled = false;
                TextboxIdentificador.ReadOnly = true;
            }
            else
            {
                DDTipoComite.Enabled = true;
                DDTipo_Proceso.Enabled = true;
                TextboxIdentificador.ReadOnly = false;
            }
        }

        /// <summary>
        /// Crear_tabla_s the ine.
        /// </summary>
        private void crear_tabla_INE()
        {
            if (IdentificadoresTable.Columns.Count == 0)
            {
                IdentificadoresTable.Columns.Add(new DataColumn("No", typeof(string)));
                IdentificadoresTable.Columns.Add(new DataColumn("Identificador", typeof(string)));
                IdentificadoresTable.Columns.Add(new DataColumn("Estado", typeof(string)));
                IdentificadoresTable.Columns.Add(new DataColumn("Prefijo", typeof(string)));
                /*Agregar Columnas Tipo Proceso, Tipo Comité, Ambito al gridview*/
                IdentificadoresTable.Columns.Add(new DataColumn("Tipo Proceso", typeof(string)));
                IdentificadoresTable.Columns.Add(new DataColumn("Tipo Comité", typeof(string)));
                IdentificadoresTable.Columns.Add(new DataColumn("Ámbito", typeof(string)));
                IdentificadoresTable.Columns.Add(new DataColumn("IdContabilidad", typeof(string)));
            }
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the DDTipo_Proceso control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void DDTipo_Proceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextboxIdentificador.Text = "";
            DDTipoComite.SelectedValue = "";
            DDAmbito.SelectedValue = "";
            DDEstado.SelectedValue = "";
            if (DDTipo_Proceso.SelectedValue == "Ordinario")
            {
                //COMITE
                divComite.Visible = true;
                divComite2.Visible = true;
                //AMBITO
                divAmbito.Visible = false;
                divAmbito2.Visible = false;
                //ENTIDAD
                divEntidad.Visible = false;
                divEntidad2.Visible = false;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = false;
                //BOTON
                LnkBAgregarIdentificador.Visible = false;
                //IDCONTABILIDAD
                divIdConta.Visible = false;
                divIdConta2.Visible = false;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = false;
                divIdContaG2.Visible = false;
            }
            else if (DDTipo_Proceso.SelectedValue == "Campaña")
            {
                //COMITE
                divComite.Visible = false;
                divComite2.Visible = false;
                //AMBITO
                divAmbito.Visible = true;
                divAmbito2.Visible = true;
                //ENTIDAD
                divEntidad.Visible = true;
                divEntidad2.Visible = true;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = true;
                //BOTON
                LnkBAgregarIdentificador.Visible = true;
                //IDCONTABILIDAD
                divIdConta.Visible = true;
                divIdConta2.Visible = true;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = false;
                divIdContaG2.Visible = false;
            }
            else if (DDTipo_Proceso.SelectedValue == "Precampaña")
            {
                //COMITE
                divComite.Visible = false;
                divComite2.Visible = false;
                //AMBITO
                divAmbito.Visible = true;
                divAmbito2.Visible = true;
                //ENTIDAD
                divEntidad.Visible = true;
                divEntidad2.Visible = true;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = true;
                //BOTON
                LnkBAgregarIdentificador.Visible = true;
                //IDCONTABILIDAD
                divIdConta.Visible = true;
                divIdConta2.Visible = true;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = false;
                divIdContaG2.Visible = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkHabilitarINE control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkHabilitarINE_CheckedChanged(object sender, EventArgs e)
        {
            divIne.Visible = chkHabilitarINE.Checked;
            DDTipo_Proceso_RequiredFieldValidator.Enabled = chkHabilitarINE.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the DDTipoComite control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void DDTipoComite_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextboxIdentificador.Text = "";
            DDAmbito.SelectedValue = "";
            DDEstado.SelectedValue = "";
            if (DDTipoComite.SelectedValue == "Ejecutivo Nacional" && DDTipo_Proceso.SelectedValue == "Ordinario")
            {
                //AMBITO
                divAmbito.Visible = false;
                divAmbito2.Visible = false;
                //ENTIDAD
                divEntidad.Visible = false;
                divEntidad2.Visible = false;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = false;
                //BOTON
                LnkBAgregarIdentificador.Visible = false;
                //IDCONTABILIDAD
                divIdConta.Visible = false;
                divIdConta2.Visible = false;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = true;
                divIdContaG2.Visible = true;
            }
            else if (DDTipoComite.SelectedValue == "Ejecutivo Estatal" && DDTipo_Proceso.SelectedValue == "Ordinario")
            {
                //AMBITO
                divAmbito.Visible = false;
                divAmbito2.Visible = false;
                //ENTIDAD
                divEntidad.Visible = true;
                divEntidad2.Visible = true;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = true;
                //BOTON
                LnkBAgregarIdentificador.Visible = true;
                //IDCONTABILIDAD
                divIdConta.Visible = true;
                divIdConta2.Visible = true;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = false;
                divIdContaG2.Visible = false;
            }
        }

        #endregion

        protected void lbFacturarTodos_Click(object sender, EventArgs e)
        {
            _idComprobante = "";
            int totales = 0;
            int procesadas = 0;
            string mensajes = "";
            var rowsChecked = RowsChecked();
            if (rowsChecked.Count > 0 && string.IsNullOrEmpty(_idComprobante))
            {
                panelHospedaje.Style["display"] = "block";//"none";
                panelIne.Style["display"] = "block";//"none";
                var IDs = rowsChecked.Select(row => ((HiddenField)row.FindControl("checkHdID")).Value).ToList();
                var tryLoad = LoadData(IDs);
                if (!tryLoad.Key)
                {
                    //throw new Exception(tryLoad.Value);
                    (Master as SiteMaster).MostrarAlerta(this, tryLoad.Value, 4, null);
                }
                else
                {
                    rblTipoEdicion.SelectedValue = "1";
                    rblTipoEdicion_SelectedIndexChanged(null, null);
                    rblTipoEdicion.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "lbFacturarTodos_ServerClick", "$('#divFacturar').modal('show');", true);
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debes seleccionar al menos un comprobante pendiente", 4, null);
            }
        }

        private List<GridViewRow> RowsChecked() => gvFacturas.Rows.Cast<GridViewRow>().Where(row => ((CheckBox)row.FindControl("check")).Checked).ToList();

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RowsChecked().Count > 0 && string.IsNullOrEmpty(_idComprobante))
            {
                tbAmbiente.Text = "PRODUCCIÓN";
                var idSerie = ddlSerie.SelectedValue;
                try
                {
                    _db.Conectar();
                    _db.CrearComando(@"SELECT serie, ambiente, (CASE ambiente WHEN 2 THEN 'PRODUCCIÓN' ELSE 'PRUEBAS' END) AS textoAmbiente from Cat_Series WHERE idSerie = @idSerie");
                    _db.AsignarParametroCadena("@idSerie", idSerie);
                    var dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        tbAmbiente.Text = dr["textoAmbiente"].ToString();
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    _db.Desconectar();
                }
            }
        }

        private void BindImpuestosConcepto(string idDetalle)
        {
            var listaImpuestosSession = Session["_listaImpuestos"];
            if (listaImpuestosSession != null)
            {
                var listaImpuestos = (List<ImpuestosConceptoTemp>)listaImpuestosSession;
                var impuestosConcepto = listaImpuestos.Where(x => x.IdConcepto.Equals(idDetalle)).ToList();
                gvImpuestosConceptos.DataSource = impuestosConcepto;
                gvImpuestosConceptos.DataBind();
            }
        }

        private List<ImpuestosConceptoTemp> GetImpuestosConceptoDataBase(string[] idComprobantes)
        {
            var ids = string.Join(",", idComprobantes);
            var sqlImpuestos = "SELECT g.idComprobante, id.idDetalle, id.idImpuesto, id.tipo, id.baseImpuesto, id.claveImpuesto, id.descripcion, id.tipoFactor, id.tasaOCuota, id.importe FROM Dat_ImpuestosDetallesCfdi33 id INNER JOIN Dat_Detalles dd ON id.idDetalle = dd.idDetalles INNER JOIN Dat_General g ON g.idComprobante = dd.id_Comprobante WHERE g.idComprobante IN (" + ids + ")";
            var impuestosConceptos = new List<ImpuestosConceptoTemp>();
            _db.Conectar();
            _db.CrearComando(sqlImpuestos);
            var dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                var impuesto = new ImpuestosConceptoTemp();
                impuesto.IdComprobante = dr["idComprobante"].ToString();
                impuesto.IdConcepto = dr["idDetalle"].ToString();
                impuesto.IdImpuesto = dr["idImpuesto"].ToString();
                impuesto.IsRetencion = dr["tipo"].ToString().Equals("R");
                impuesto.Base = dr["baseImpuesto"].ToString();
                impuesto.Impuesto = dr["claveImpuesto"].ToString();
                impuesto.Descripcion = dr["descripcion"].ToString();
                impuesto.TipoFactor = dr["tipoFactor"].ToString();
                impuesto.TasaOCuota = dr["tasaOCuota"].ToString();
                impuesto.Importe = dr["importe"].ToString();
                impuestosConceptos.Add(impuesto);
            }
            _db.Desconectar();
            return impuestosConceptos;
        }

        protected void lbVerImpuestosDeConcepto_Click(object sender, EventArgs e)
        {
            var lb = (LinkButton)sender;
            var idConcepto = lb.CommandArgument;
            BindImpuestosConcepto(idConcepto);
            ScriptManager.RegisterStartupScript(this, GetType(), "lbFacturarTodos_ServerClick", "$('#divImpuestosDeConcepto').modal('show');", true);
        }

        protected void gvImpuestosConceptos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvImpuestosConceptos.EditIndex = e.NewEditIndex;
        }

        protected void gvImpuestosConceptos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var idImpuesto = e.Keys[0].ToString();
            var idDetalle = e.Keys[0].ToString();
            try
            {
                var listaImpuestosSession = Session["_listaImpuestos"];
                if (listaImpuestosSession != null)
                {
                    var listaImpuestos = (List<ImpuestosConceptoTemp>)listaImpuestosSession;
                    var row = gvConceptos.Rows[e.RowIndex];
                    var Base = ((TextBox)row.FindControl("tbBase")).Text;
                    var Impuesto = ((DropDownList)row.FindControl("ddlImpuesto")).SelectedValue;
                    var Descripcion = ((DropDownList)row.FindControl("ddlImpuesto")).SelectedItem.Text;
                    var TipoFactor = ((DropDownList)row.FindControl("ddlTipoFactor")).SelectedValue;
                    var TasaOCuota = ((TextBox)row.FindControl("tbTasaOCuota")).Text;
                    var Importe = ((TextBox)row.FindControl("tbImporte")).Text;
                    var impuestoItem = listaImpuestos.FirstOrDefault(i => i.IdConcepto == idDetalle && i.IdImpuesto == idImpuesto);
                    if (impuestoItem != null)
                    {
                        impuestoItem.Base = Base;
                        impuestoItem.Impuesto = Impuesto;
                        impuestoItem.Descripcion = Descripcion;
                        impuestoItem.TipoFactor = TipoFactor;
                        impuestoItem.TasaOCuota = TasaOCuota;
                        impuestoItem.Importe = Importe;
                    }
                    Session["_listaImpuestos"] = listaImpuestos;
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El impuesto no se pudo editar, inténtelo nuevamente.<br/><br/>" + ex.Message, 4);
            }
            finally
            {
                BindImpuestosConcepto(idDetalle);
                gvImpuestosConceptos.EditIndex = -1;
            }
        }

        protected void gvImpuestosConceptos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvImpuestosConceptos.EditIndex = -1;
        }

        protected void gvImpuestosConceptos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFormaPago.Text = ddlFormaPago.SelectedItem.Text;
            if (ddlFormaPago.SelectedValue.Equals("PPD"))
            {
                ddlMetodoPago.SelectedValue = "99";
                ddlMetodoPago.Enabled = false;
                if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HSC0010193M7|OHS0312152Z4|OHF921110BF2|LAN7008173R5"))
                {
                    ddlMetodoPago.Enabled = true;
                }
            }
            else
            {
                ddlMetodoPago.Enabled = true;
            }
        }
    }
}