// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 05-22-2017
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
using DataExpressWeb.wsEmision;
using System.Text.RegularExpressions;
using System.Collections;
using System.Globalization;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Documentos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Tickets : System.Web.UI.Page
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
        /// The command tramas
        /// </summary>
        private static readonly string CommandTramas = "SELECT TOP 1000  l.idTrama, l.Trama, l.FECHA, l.serie, (CASE ISNULL(l.noTicket,'') WHEN '' THEN l.noReserva ELSE l.noTicket END) AS codigoControl FROM Log_Trama l WHERE l.tipo = 4 AND Observaciones = 'ExtranetOk'";
        /// <summary>
        /// The data table conceptos
        /// </summary>
        private static DataTable dataTableConceptos;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                if (Session["errorCertificado"] != null && Convert.ToBoolean(Session["errorCertificado"]))
                {
                    Response.Redirect("~/LicExpirada.aspx", true);
                    return;
                }
                _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                _log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                SqlDataSeries.ConnectionString = _db.CadenaConexion;
                SqlDataSeriesFact.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.ConnectionString = _db.CadenaConexion;
                SqlDataSourceFacGlob.ConnectionString = _db.CadenaConexion;
                SqlDataUsoCfdi.ConnectionString = _db.CadenaConexion;
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
                            #endregion
                        }
                        else if (Session["IDGIRO"].ToString().Contains("2"))
                        {
                            #region Restaurante

                            trOtrosCargos.Visible = true;
                            rowPropina.Visible = true;
                            #endregion
                        }
                        else if (Session["IDGIRO"].ToString().Contains("3"))
                        {
                            #region Empresa



                            #endregion
                        }
                    }

                    #endregion
                    tbNumCtaPago.ReadOnly = true;
                    tbNumCtaPago.Text = "NO IDENTIFICADO";
                }
            }

            if (!IsPostBack)
            {
                Session["gvTickets"] = null;
                _dtActual = new DataTable();

                if (Convert.ToBoolean(Session["coFactPropias"]) || Convert.ToBoolean(Session["coFactTodas"]))
                {
                    if (string.IsNullOrEmpty((string)Session["rfcCliente"]))
                    {
                        _consulta = "";
                    }
                    if (Session["rolUser"].ToString() == "3")
                    {
                        _consulta = "";
                    }
                }
                //this.LlenarGrid();
                _dtActual = Session["gvTickets"] as DataTable;
                gvTickets.DataSource = _dtActual;
                gvTickets.DataBind();
                if (_dtActual != null)
                {
                    lCount.Text = "Se encontraron <span class='badge'>" + _dtActual.Rows.Count + "</span> Registros ";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "_showDocumentosActive", "resaltar('liEmision');", true);
                FillCatalogPaises();
            }
            else
            {
                if (dataTableConceptos != null)
                {
                    gvConceptos.DataSource = dataTableConceptos;
                }
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
                /*if (Regex.IsMatch(Session["IDENTEMIEXT"].ToString(), @"LAN7008173R5|OAL111108K5A"))
                {
                    try
                    {
                        ddlPais.SelectedValue = "MEX";
                        tbPaisRec.Text = "MEX";
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }*/
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

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar(bool consultarBase = true)
        {
            if (IsPostBack)
            {
                try
                {
                    if (consultarBase)
                    {
                        this.LlenarGrid();
                    }
                    else
                    {
                        Session["gvTickets"] = null;
                    }
                    _dtActual = Session["gvTickets"] as DataTable;
                    gvTickets.DataSource = _dtActual;
                    gvTickets.DataBind();
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Error al cargar registros: " + ex.Message, 4);
                }
            }
            lCount.Visible = true;
            _consulta = "";
        }

        /// <summary>
        /// Handles the Click1 event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bLimpiarFiltros_Click(object sender, EventArgs e)
        {
            tbMes.Text = "";
            ddlSerie.SelectedValue = "0";
            tbReserva.Text = "";
            Buscar(false);
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
        /// Handles the PageIndexChanged event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvTickets_PageIndexChanged(object sender, EventArgs e)
        {
            //DataFilter1_OnFilterAdded();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataFilter1_OnFilterAdded();
            //var row = gvTickets.SelectedRow;
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
        /// <exception cref="Exception">Para filtrar por fecha final, debe especificar una fecha inicial.</exception>
        private void LlenarGrid()
        {
            var paramEstablished = true;
            var myWhere = "";
            if (!string.IsNullOrEmpty(tbMes.Text))
            {
                var currentDate = DateTime.Parse(tbMes.Text).ToString("yyyy-MM");
                myWhere = " AND l.Trama like '%" + currentDate + "%'";
            }
            if (!string.IsNullOrEmpty(ddlSerie.SelectedValue) && !ddlSerie.SelectedValue.Equals("0"))
            {
                myWhere += " AND l.serie = '" + ddlSerie.SelectedValue + "'";
            }
            if (!string.IsNullOrEmpty(tbReserva.Text))
            {
                var reservas = tbReserva.Text.Split(',').ToList().Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
                myWhere += " AND (";
                for (int i = 0; i < reservas.Count; i++)
                {
                    var reserva = reservas[i];
                    myWhere += "l.noReserva LIKE '%" + reserva + "%' OR l.noTicket LIKE '%" + reserva + "%'";
                    if (i < reservas.Count - 1)
                    {
                        myWhere += " OR ";
                    }
                }
                myWhere += ")";
            }
            if (string.IsNullOrEmpty(myWhere))
            {
                paramEstablished = false;
                var currentDate = DateTime.Now.ToString("yyyy-MM");
                myWhere = " AND l.Trama like '%" + currentDate + "%'";
            }
            _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            _db.Conectar();
            var comando = CommandTramas + myWhere + " ORDER BY l.Fecha DESC";
            if (IsPostBack && paramEstablished)
            {
                comando = Regex.Replace(comando, @"TOP \d+", "");
            }
            _db.CrearComando(comando);
            _dtActual.Load(_db.EjecutarConsulta());
            _db.Desconectar();
            Session["gvTickets"] = _dtActual;
            lCount.Text = "Se encontraron <span class='badge'>" + _dtActual.Rows.Count + "</span> Registros ";
            lCount.Visible = true;
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _dtActual = Session["gvTickets"] as DataTable;
            gvTickets.DataSource = _dtActual;
            gvTickets.PageIndex = e.NewPageIndex;

            //gvTickets.PageIndex = gvTickets.PageCount;

            gvTickets.DataBind();
        }

        /// <summary>
        /// Handles the First event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvTickets_First(object sender, EventArgs e)
        {
            _dtActual = Session["gvTickets"] as DataTable;
            gvTickets.DataSource = _dtActual;
            gvTickets.PageIndex = 0;

            //gvTickets.PageIndex = gvTickets.PageCount;

            gvTickets.DataBind();
        }

        /// <summary>
        /// Handles the Prev event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvTickets_Prev(object sender, EventArgs e)
        {
            _dtActual = Session["gvTickets"] as DataTable;
            gvTickets.DataSource = _dtActual;

            if (gvTickets.PageIndex > 0)
            {
                gvTickets.PageIndex = gvTickets.PageIndex - 1;
            }
            //gvTickets.PageIndex = gvTickets.PageCount;

            gvTickets.DataBind();
        }

        /// <summary>
        /// Handles the Next event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvTickets_Next(object sender, EventArgs e)
        {
            _dtActual = Session["gvTickets"] as DataTable;
            gvTickets.DataSource = _dtActual;


            if (gvTickets.PageIndex < gvTickets.PageCount - 1)
            {
                gvTickets.PageIndex = gvTickets.PageIndex + 1;
            }
            //gvTickets.PageIndex = gvTickets.PageCount;

            gvTickets.DataBind();
        }

        /// <summary>
        /// Handles the Last event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvTickets_Last(object sender, EventArgs e)
        {
            _dtActual = Session["gvTickets"] as DataTable;
            gvTickets.DataSource = _dtActual;

            //gvTickets.SetPageIndex(gvTickets.PageCount);

            //gvTickets.PageIndex = gvTickets.PageIndex + 1;

            gvTickets.PageIndex = gvTickets.PageCount - 1;

            gvTickets.DataBind();
        }

        /// <summary>
        /// Handles the Sorted event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvTickets_Sorted(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Sorting event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs" /> instance containing the event data.</param>
        protected void gvTickets_Sorting(object sender, GridViewSortEventArgs e)
        {
            _dtActual = Session["gvTickets"] as DataTable;
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

                gvTickets.DataSource = dataView;
                gvTickets.DataBind();
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
        /// Handles the RowDataBound event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gvTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvTickets.HeaderRow != null)
            {
                _chkboxSelectAll = (CheckBox)gvTickets.HeaderRow.FindControl("chkboxSelectAll");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    var lb = (HyperLink)(e.Row.FindControl("lbPopupDetalles"));
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var name = row.Field<string>("trama").Trim("<br/>".ToCharArray());
                    var chkSeleccionar = (CheckBox)(e.Row.FindControl("check"));
                    if (!string.IsNullOrEmpty(name))
                    {
                        var nameScaped = name.Replace("'", "\"");
                        lb.Attributes["data-content"] = "<pre style='max-height: 500px'>" + name + "</pre><br/><div align='right'><button type='button' id='btnPopover_" + lb.ClientID + "' class='btn btn-primary btn-sm btn-clip' data-clipboard-text='" + nameScaped + "' data-clipboard-action='copy'><i class='fa fa-clipboard'></i> Copiar</button>&nbsp;<button type='button' id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Handles the Click event of the bBuscarReg control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Handles the PreRender event of the gvTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvTickets_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvTickets.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception) { }
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
        /// Gets the sucursal spool.
        /// </summary>
        /// <param name="_exsucursal">The exsucursal.</param>
        /// <returns>System.String.</returns>
        private string GetSucursalSpool(string _exsucursal)
        {
            var sucursal = "1";
            try
            {
                _db.Conectar();
                _db.CrearComando("SELECT COALESCE((SELECT idSucursal FROM Cat_SucursalesEmisor WHERE clave = @clave), (SELECT MIN(idSucursal) FROM Cat_SucursalesEmisor)) AS idSucursal");
                _db.AsignarParametroCadena("@clave", _exsucursal);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    sucursal = dr[0].ToString();
                }
            }
            catch { sucursal = ""; }
            finally
            {
                _db.Desconectar();
            }
            return sucursal;
        }

        /// <summary>
        /// Handles the Click event of the bFacturar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bFacturar_Click(object sender, EventArgs e)
        {
            //Page.Validate("validationExtranet");
            //if (Page.IsValid)
            if (tbRfcRec.Text.Length >= 12 && !String.IsNullOrEmpty(tbRazonSocialRec.Text) && !String.IsNullOrEmpty(tbCpRec.Text) && !String.IsNullOrEmpty(ddlPais.SelectedItem.Text) && ddlPais.SelectedItem.Text != "Seleccione")
            {
                try
                {
                    var txt = new SpoolMx();
                    var dtRows = dataTableConceptos.Rows.Cast<DataRow>().ToList();
                    if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"RMM120815858|CFL040216TJ9"))
                    {
                        var metodoPago = ddlMetodoPago.SelectedItem.Text;
                        var listKeys = ddlMetodoPago.Attributes["jsFunction"];
                        //if (!string.IsNullOrEmpty(listKeys) && ddlMetodoPago.CssClass.Contains("bootstrap-select-multiple") && !string.IsNullOrEmpty(hfMetodoPago.Value))
                        //{
                        //    metodoPago = hfMetodoPago.Value;
                        //}

                        var regTrama = ConsultarTrama(tbReserva.Text);
                        Session["_idTrama"] = regTrama[0];
                        var idTrama = Session["_idTrama"].ToString();
                        Session["_trama"] = regTrama[1];

                        if (string.IsNullOrEmpty(metodoPago))
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "El código del pago no puede quedar vacío", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }

                        #region Trama MICROS
                        try
                        {
                            var lineaR = "R|" + tbRfcRec.Text + "|" + tbRazonSocialRec.Text + "|" + tbCalleRec.Text +
                                          "|" + tbNoExtRec.Text + "|" + tbNoIntRec.Text + "|" + tbColoniaRec.Text + "|" + "" +
                                          "|" + tbMunicipioRec.Text + "|" + tbEstadoRec.Text + "|" + (Session["CfdiVersion"].ToString().Equals("3.3") ? ddlPais.SelectedValue : tbPaisRec.Text) +
                                          "|" + tbCpRec.Text + "|" + tbMailReceptor.Text + "|";

                            var lineaExt = "E|" + metodoPago + "|" + tbNumCtaPago.Text + "|" + tbObservaciones.Text + "|";

                            Session["_trama"] += Environment.NewLine + lineaR + Environment.NewLine + lineaExt;
                            object[] result = null;

                            var tramatxt = Session["_trama"];

                            var randomMs = new Random().Next(1000, 5000);
                            System.Threading.Thread.Sleep(randomMs);
                            var ws = new wsMicros.wsMicros { Timeout = (1800 * 1000) };
                            result = ws.RecibeInfoTxt(Session["_trama"].ToString(), Session["_idTrama"].ToString(), Session["IDENTEMI"].ToString(), null, true);
                            //} while ((result == null || !((bool)result[0])) && count < 3);
                            if (result != null)
                            {
                                var status = (bool)result[0];
                                var msg = (string)result[1];
                                if (status)
                                {
                                    try
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"UPDATE Log_Trama SET
                                observaciones = NULL
                                WHERE idTrama = @idTrama");
                                        _db.AsignarParametroCadena("@idTrama", Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString());
                                        _db.EjecutarConsulta1();
                                        _db.Desconectar();
                                    }
                                    catch { }
                                    //if (Session["IDENTEMIEXT"].ToString().Equals("OAL111108K5A"))
                                    //{
                                    //    tbTicket.Text = msg;
                                    //}
                                    //bConsultar_Click(null, null);
                                }
                                else
                                {
                                    var mensaje = msg.Replace(@"\", "");
                                    if (Regex.IsMatch(mensaje, @"El ticket ya se encuentra facturado en la factura", RegexOptions.IgnoreCase))
                                    {
                                        // bConsultar_Click(null, null);
                                    }
                                    else
                                    {
                                        (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + msg, 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                                    }
                                }
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + "El servidor regreso una respuesta vacia", 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                            }
                        }
                        catch (Exception ex)
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + ex.Message, 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }
                        #endregion

                    }
                    else if (dtRows.Count <= 500)
                    {
                        #region MENOR A 500 TICKEST
                        _db.Conectar();
                        _db.CrearComando(@"select RFCEMI,NOMEMI,curp,telefono,email,empresaTipo,dirMatriz AS calle,noExterior,noInterior,colonia,localidad,referencia,municipio,estado,pais,codigoPostal,regimenFiscal FROM Cat_Emisor where RFCEMI=@RFCEMI");
                        _db.AsignarParametroCadena("@RFCEMI", Session["IDENTEMI"].ToString());
                        var dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            txt.SetEmisorCfdi(dr["NOMEMI"].ToString(), dr["RFCEMI"].ToString(), dr["curp"].ToString(), dr["telefono"].ToString(), dr["email"].ToString(), Session["IDGIRO"].ToString(), dr["regimenFiscal"].ToString());
                            txt.SetEmisorDomCfdi(dr["calle"].ToString(), dr["noExterior"].ToString(), dr["noInterior"].ToString(), dr["colonia"].ToString(), dr["localidad"].ToString(), dr["referencia"].ToString(), dr["municipio"].ToString(), dr["estado"].ToString(), dr["pais"].ToString(), dr["codigoPostal"].ToString());
                            txt.SetEmisorExpCfdi(dr["calle"].ToString(), dr["noExterior"].ToString(), dr["noInterior"].ToString(), dr["colonia"].ToString(), dr["localidad"].ToString(), dr["referencia"].ToString(), dr["municipio"].ToString(), dr["estado"].ToString(), dr["pais"].ToString(), dr["codigoPostal"].ToString(), GetSucursalSpool(""));
                        }
                        _db.Desconectar();
                       // txt.SetReceptorCfdi(tbRazonSocialRec.Text, tbRfcRec.Text, "", "", tbMailReceptor.Text);
                        txt.SetReceptorCfdi(tbRazonSocialRec.Text, tbRfcRec.Text, null, null, tbMailReceptor.Text, null, null, "", "", ddlUsoCFDI.SelectedValue);
                        txt.SetReceptorDomCfdi(tbCalleRec.Text, tbNoExtRec.Text, tbNoIntRec.Text, tbColoniaRec.Text, "", "", tbMunicipioRec.Text, tbEstadoRec.Text, tbPaisRec.Text, tbCpRec.Text);
                        txt.SetCantidadImpuestosCfdi("", tbIva16.Text, tbIva16.Text);
                        for (int i = 0, count = dtRows.Count; i < count; i++)
                        {
                            var row = dtRows[i];
                            var cantidad = row[0].ToString();
                            var descripcion = row[1].ToString();
                            var valorUnitario = row[2].ToString();
                            var iva = row[3].ToString();
                            var importe = row[2].ToString();
                            var unidad = row[7].ToString();
                            txt.AgregaConceptoCfdi(cantidad, unidad, "", descripcion, valorUnitario, importe);
                            txt.AgregaImpuestoTrasladoCfdi("IVA", "16.00", iva);
                        }
                        var tickets = string.Join(",", dtRows.Select(row => row[1].ToString().Replace("Ticket ", "")));
                        txt.SetComprobanteCfdi(ddlSerieFact.SelectedItem.Text, "", Localization.Now.ToString("s"), tbFormaPago.Text, "", tbSubtotal.Text, "", "", "1.0", "MXN", tbTotalFac.Text, "ingreso", ddlMetodoPago.SelectedValue, tbLugarExp.Text, tbNumCtaPago.Text, null, null, null, null, null, null, tbOtrosCargos.Text, tbTotal.Text, (dtRows.Count > 1 ? "Factura de Tickets " + tickets + ". " : "") + tbObservaciones.Text);
                        if (Session["IDGIRO"].ToString().Contains("2") || Session["IDGIRO"].ToString().Contains("1"))
                        {
                            txt.SetInfoAdicionalRestauranteCfdi(tbPropina.Text, dtRows.Count == 1 ? tickets : "");
                        }
                        var txtInvoice = txt.ConstruyeTxtCfdi();
                        var coreMx = new WsEmision { Timeout = (1800 * 1000) };
                        var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), GetAmbiente(ddlSerieFact.SelectedItem.Text), "01", false, true, "", "");
                        var outter = result != null ? result.OuterXml : ("NULL: " + coreMx.ObtenerMensaje());
                        if (result != null)
                        {
                            var xDoc = new XmlDocument();
                            xDoc.LoadXml(result.OuterXml);
                            Session["uuidCreado"] = GetAtributte(xDoc, "UUID", "tfd:TimbreFiscalDigital");
                            (Master as SiteMaster).MostrarAlerta(this, "El comprobante se ha generado satisfactoriamente", 2, null);
                            try
                            {
                                var referencias = "'" + string.Join("','", dtRows.Select(row => row[1].ToString().Replace("Ticket ", ""))) + "'";
                                var sql = "UPDATE Log_Trama SET observaciones = NULL, folio = NULL WHERE noReserva IN (" + referencias + ")";
                                _db.Conectar();
                                _db.CrearComando(sql);
                                _db.EjecutarConsulta1();
                            }
                            catch { }
                            finally
                            {
                                _db.Desconectar();
                            }
                            Response.Redirect("~/Documentos.aspx", false);
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se creó correctamente<br/>" + coreMx.ObtenerMensaje(), 4, null);
                        }
                        #endregion
                    }
                    else
                    {

                        #region nueva
                        // se envia wsfactura global

                        var FGWeb = new FacturaGlobalWeb.FacturaGlobalWeb();
                        var datosReceptor = "";
                        var comprobante = "";
                        //datos receptor
                        // if (string.IsNullOrEmpty(tbMailReceptor.Text)) { tbMailReceptor.Text = "-"; }
                        datosReceptor = tbRazonSocialRec.Text + '|' + tbRfcRec.Text + '|' + tbMailReceptor.Text + '|' + tbCalleRec.Text + '|' + tbNoExtRec.Text + '|' + tbNoIntRec.Text + '|' + tbColoniaRec.Text + '|' + tbMunicipioRec.Text + '|' + tbEstadoRec.Text + '|' + ddlPais.SelectedValue.ToString() + '|' + tbCpRec.Text + '|' + tbIva16.Text;
                        //datos comprobante
                        comprobante = ddlSerieFact.SelectedItem.Text + '|' + tbFormaPago.Text + '|' + tbSubtotal.Text + '|' + tbTotalFac.Text + '|' + ddlMetodoPago.SelectedValue + '|' + tbLugarExp.Text + '|' + tbNumCtaPago.Text + '|' + tbOtrosCargos.Text + '|' + tbTotal.Text + '|' + tbObservaciones.Text + '|' + tbPropina.Text;

                        var ambiente = (ddlSerieFact.SelectedItem.Text);
                        //se recorre la lista para sacar los valores                    
                        var val = new List<object[]>();
                        string[] valores = new string[6];
                        for (int i = 0, count = dtRows.Count; i < count; i++)
                        {
                            var row = dtRows[i];
                            val.Add(row.ItemArray);
                        }
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(val);
                        //se envia a wsFactura Global
                        FGWeb.FacuraGlobalAsync(Session["IDENTEMI"].ToString(), Session["IDGIRO"].ToString(), json, _idUser, ambiente, datosReceptor, comprobante);
                        (Master as SiteMaster).MostrarAlerta(this, "El comprobante se esta generando", 2, null);
                        // Delay de 10 s
                        // Actualizar el gridview
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se creó correctamente<br/>" + ex.Message, 4, null);
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "Verifique que todos los datos de la factura sean correctos.", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            }
        }


        private string[] ConsultarTrama(string codDontrol)
        {
            string trama = "";
            DbDataReader dr;
            _db.Conectar();
            _db.CrearComando(@"select TOP 1 idTrama, Trama, Fecha from Log_Trama
                            where   noReserva=@codDontrol and tipo = '4' and observaciones = 'ExtranetOK' ORDER BY idTrama DESC");
            _db.AsignarParametroCadena("@codDontrol", codDontrol);
            dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                Session["_idTrama"] = dr[0].ToString();
                trama = dr[1].ToString();
            }
            _db.Desconectar();
            var txt = Session["_idTrama"].ToString();
            // }
            if (string.IsNullOrEmpty(Session["_idTrama"].ToString()))
            {
                _db.Conectar();
                _db.CrearComando(@"select TOP 1 idTrama, Trama, Fecha from Log_Trama
                            where   Secuencial=@codDontrol and tipo = '4' and observaciones = 'ExtranetOK' ORDER BY idTrama DESC");
                _db.AsignarParametroCadena("@codDontrol", codDontrol);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    Session["_idTrama"] = dr[0].ToString();
                    trama = dr[1].ToString();
                }
                _db.Desconectar();
            }
            if (string.IsNullOrEmpty(Session["_idTrama"].ToString()))
            {
                _db.Conectar();
                _db.CrearComando(@"select TOP 1 idTrama, Trama, Fecha from Log_Trama
                            where   noTicket=@codDontrol and tipo = '4' and observaciones = 'ExtranetOK' ORDER BY idTrama DESC");
                _db.AsignarParametroCadena("@codDontrol", codDontrol);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    Session["_idTrama"] = dr[0].ToString();
                    trama = dr[1].ToString();
                }
                _db.Desconectar();
            }
            return new string[] { Session["_idTrama"].ToString(), trama };
        }



        /// <summary>
        /// Obtiene el ambiente dependiendo de la serie con la que salga
        /// </summary>
        /// <param name="serie">Serie de la factura</param>
        /// <returns>El ambiente</returns>
        /// <exception cref="Exception">La serie " + serie + " no está registrada.</exception>
        private bool GetAmbiente(string serie)
        {
            bool ambiente;
            _db.Conectar();
            _db.CrearComando(@"SELECT ambiente FROM Cat_Series Where serie = @serie");
            _db.AsignarParametroCadena("@serie", serie);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                ambiente = dr["ambiente"].ToString().Equals("2");
            }
            else
            {
                throw new Exception("La serie " + serie + " no está registrada.");
            }
            _db.Desconectar();
            return ambiente;
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
            gvConceptos.PageIndex = e.NewPageIndex;
            gvConceptos.DataSource = dataTableConceptos;
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlMetodoPago control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNumCtaPago_MaskedEditExtender.Enabled = ddlMetodoPago.SelectedValue.Equals("03") || ddlMetodoPago.SelectedValue.Equals("04") || ddlMetodoPago.SelectedValue.Equals("28");
            tbNumCtaPago.ReadOnly = ddlMetodoPago.SelectedValue.Equals("99") || ddlMetodoPago.SelectedValue.Equals("01");
            tbNumCtaPago.Text = (!string.IsNullOrEmpty(tbNumCtaPago.Text) && !tbNumCtaPago.Text.Equals("____")) ? tbNumCtaPago.Text : (tbNumCtaPago_MaskedEditExtender.Enabled ? "" : "NO IDENTIFICADO");
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRfcRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tbRfcRec_TextChanged(object sender, EventArgs e)
        {
            ddlUsoCFDI.SelectedValue = "P01";
            Llenarlista(tbRfcRec.Text);
        }

        /// <summary>
        /// Llenarlistas the specified RFC.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        private void Llenarlista(string rfc)
        {
            var sql = @"SELECT IDEREC
                              ,[NOMREC]
                              ,[telefono]
                              ,[contribuyenteEspecial]
                              ,[obligadoContabilidad]
                              ,[tipoIdentificacionComprador]
                              ,[email]
                              ,[curp]
                              ,(CASE ISNULL(CONVERT(VARCHAR, metodoPago), '') WHEN '' THEN '99' ELSE ISNULL(cc.codigo, '99') END) AS metodoPago
                              ,[numCtaPago]
                              ,[telefono2]
                          FROM [Cat_Receptor] LEFT OUTER JOIN Cat_Catalogo1_C cc ON CONVERT(VARCHAR, metodoPago) = cc.codigo AND cc.tipo = 'MetodoPago'
                          WHERE RFCREC=@ruc";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@ruc", rfc);
            var dr = _db.EjecutarConsulta();
            tbRazonSocialRec.ReadOnly = dr.HasRows && !rfc.Equals("XAXX010101000") && !rfc.Equals("XEXX010101000");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tbRfcRec.Text = rfc;
                    tbRazonSocialRec.Text = dr["NOMREC"].ToString();
                    tbMailReceptor.Text = dr["email"].ToString();
                }
            }
            _db.Desconectar();
            Llenarlistadom(rfc);
        }

        /// <summary>
        /// Llenarlistadoms the specified RFC record.
        /// </summary>
        /// <param name="rfcRec">The RFC record.</param>
        private void Llenarlistadom(string rfcRec)
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
            var control = dr.HasRows;
            //tbCalleRec.ReadOnly = !control;
            //tbNoExtRec.ReadOnly = !control;
            //tbNoIntRec.ReadOnly = !control;
            //tbColoniaRec.ReadOnly = !control;
            //tbLocRec.ReadOnly = !control;
            //tbRefRec.ReadOnly = !control;
            //tbMunicipioRec.ReadOnly = !control;
            //tbEstadoRec.ReadOnly = !control;
            //tbPaisRec.ReadOnly = !control;
            //tbCpRec.ReadOnly = !control;
            btnPaisRec.Attributes.Remove("disabled");
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
                if (dr["pais"].ToString().Length > 3)
                {
                    string resultado = dr["pais"].ToString().Substring(0, 3);
                    tbPaisRec.Text = resultado;
                    ddlPais.SelectedValue = resultado;
                }
                else
                {
                    tbPaisRec.Text = dr["pais"].ToString();
                    ddlPais.SelectedValue = dr["pais"].ToString();
                }
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
            }
            else
            {
                _db.Desconectar();
                tbRazonSocialRec.Text = "";
                tbCalleRec.Text = "";
                tbNoExtRec.Text = "";
                tbNoIntRec.Text = "";
                tbColoniaRec.Text = "";
                //tbLocRec.Text = "";
                //tbRefRec.Text = "";
                tbMunicipioRec.Text = "";
                tbEstadoRec.Text = "";
                //tbPaisRec.Text = "";
                ddlPais.SelectedValue = "MEX";
                tbCpRec.Text = "";
            }
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
                    if (dr["pais"].ToString().Length > 3)
                    {
                        string resultado = dr["pais"].ToString().Substring(0, 3);
                        tbPaisRec.Text = resultado;
                        ddlPais.SelectedValue = resultado;
                    }
                    else
                    {
                        tbPaisRec.Text = dr["pais"].ToString();
                        ddlPais.SelectedValue = dr["pais"].ToString();
                    }
                    tbCpRec.Text = dr["codigoPostal"].ToString();
                }
                _db.Desconectar();
            }
            else
            {
                Llenarlistadom(tbRfcRec.Text);
            }
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        private void ClearData()
        {
            Llenarlista("");
            LlenarGridView("");
            tbSubtotal.Text = "";
            tbIva16.Text = "";
            lblISHPrer.Text = "";
            tbISH.Text = "";
            tbTotalFac.Text = "";
            tbPropina.Text = "";
            tbOtrosCargos.Text = "";
            tbTotal.Text = "";
            tbCodDoc.Text = "";
            tbAmbiente.Text = "";
            tbFormaPago.Text = "";
            tbCantLetra.Text = "";
            tbLugarExp.Text = "";
            tbObservaciones.Text = "";
        }

        /// <summary>
        /// Llenars the grid view.
        /// </summary>
        /// <param name="idComprobante">The identifier comprobante.</param>
        private void LlenarGridView(string idComprobante)
        {
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the RowEditing event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewEditEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvConceptos.EditIndex = e.NewEditIndex;
            LlenarGridView(_idComprobante);
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
        /// Handles the Click event of the bFacturarTickets control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bFacturarTickets_Click(object sender, EventArgs e)
        {
            ddlUsoCFDI.SelectedValue = "P01";
            if (Session["errorTimbres"] != null && Convert.ToBoolean(Session["errorTimbres"]))
            {
                (Master as SiteMaster).MostrarAlerta(this, "Excedió la cantidad de timbres", 4, null);
                return;
            }
            else if (Session["errorCertificado"] != null && Convert.ToBoolean(Session["errorCertificado"]))
            {
                (Master as SiteMaster).MostrarAlerta(this, "Certificado expirado", 4, null);
                return;
            }
            List<KeyValuePair<string, string>> errores;
            var tramas = tramasChecked(out errores);
            var tramasFacturadas = GetFacturadas(tramas);
            if (tramas.Count < 1)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debes seleccionar al menos un ticket de Micros", 4, null);
                return;
            }
            else if (tramasFacturadas.Count > 0)
            {
                var html = "<ul><li>" + string.Join("</li></li>", tramasFacturadas.Select(x => Session["CfdiVersion"].ToString().Equals("3.3") ? x.Resumen33.CurrentReferenceNumber : x.Resumen.NoReferenciaAct)) + "</li></ul>";
                (Master as SiteMaster).MostrarAlerta(this, "Los siguientes tickets ya fueron facturados, actualice la busqueda:<br/>" + html, 4, null);
                return;
            }
            else
            {
                try
                {
                    var numLetra = new NumerosALetras();
                    decimal _subTotal = 0;
                    decimal _iva16 = 0;
                    decimal _totalFac = 0;
                    decimal _propina = 0;
                    decimal _otrosCargos = 0;
                    decimal _aPagar = 0;
                    decimal _descuentos = 0;
                    decimal MontoNeto = 0;
                    dataTableConceptos = new DataTable();
                    if (dataTableConceptos.Columns.Count == 0)
                    {
                        dataTableConceptos.Columns.Add("cantidad", typeof(string));
                        dataTableConceptos.Columns.Add("descripcion", typeof(string));
                        dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                        dataTableConceptos.Columns.Add("iva", typeof(string));
                        dataTableConceptos.Columns.Add("importe", typeof(string));
                        dataTableConceptos.Columns.Add("propina", typeof(string));
                        dataTableConceptos.Columns.Add("total", typeof(string));
                        dataTableConceptos.Columns.Add("unidad", typeof(string));
                        dataTableConceptos.Columns.Add("descuento", typeof(string));
                    }
                    foreach (var tramaMicros in tramas)
                    {
                        string descripcion = "";
                        decimal subTotal = 0;
                        decimal iva = 0;
                        decimal totalFac = 0;
                        decimal propina = 0;
                        decimal otrosCargos = 0;
                        decimal aPagar = 0;
                        decimal descuentos = 0;

                        try { descripcion = "Ticket " + tramaMicros.Resumen.NoReferenciaAct; } catch { }
                        try { subTotal = tramaMicros.Detalles.Sum(x => Convert.ToDecimal(x.MontoNetoAgrupado)); } catch { }
                        try { iva = tramaMicros.Impuestos.Sum(x => Convert.ToDecimal(x.MontoNetoi)); } catch { }
                        try { totalFac = tramaMicros.Detalles.Sum(x => Convert.ToDecimal(x.MontoBrutoagrupadom)); } catch { }
                        try { propina = Convert.ToDecimal(tramaMicros.Propinas != null ? tramaMicros.Propinas.MontoNetos : "0.00"); } catch { }
                        try { otrosCargos = Convert.ToDecimal("0.00"); } catch { }
                        try { aPagar = Convert.ToDecimal(tramaMicros.Resumen.MontoBruto); } catch { }
                        { try { descuentos = tramaMicros.Descuentos.Sum(x => Convert.ToDecimal(x.MontoNetod, new CultureInfo("en-US"))); } catch { } }
                        try { MontoNeto = Convert.ToDecimal(tramaMicros.Resumen.MontoNeto); } catch { }

                        _subTotal += subTotal;
                        _iva16 += iva;
                        //_totalFac += totalFac;
                        _totalFac += MontoNeto;
                        _propina += propina;
                        _otrosCargos += otrosCargos;
                        _aPagar += aPagar;
                        _descuentos += descuentos;

                        var NewRow = dataTableConceptos.NewRow();
                        NewRow[0] = "1"; // CANTIDAD
                        NewRow[1] = descripcion; // DESCRIPCION
                        NewRow[2] = subTotal.ToString(); // VAL. UNITARIO
                        NewRow[3] = iva.ToString(); // IVA
                        NewRow[4] = MontoNeto.ToString(); // IMPORTE
                        NewRow[5] = propina.ToString(); // PROPINA
                        NewRow[6] = aPagar.ToString(); // TOTAL
                        NewRow[7] = "NO APLICA"; // UNIDAD
                        NewRow[8] = descuentos; // DESCUENTOS
                        dataTableConceptos.Rows.Add(NewRow);
                    }
                    _db.Conectar();
                    _db.CrearComando(@"select RFCEMI,NOMEMI,dirMatriz,noExterior,noInterior,colonia,referencia,municipio,
                                 estado,pais,codigoPostal,regimenFiscal FROM Cat_Emisor where RFCEMI=@RFCEMI");
                    _db.AsignarParametroCadena("@RFCEMI", Session["IDENTEMI"].ToString());
                    var dr = _db.EjecutarConsulta();
                    if (dr.Read()) { tbLugarExp.Text = dr[9].ToString() + ", " + dr[8].ToString(); }
                    _db.Desconectar();
                    tbTotalFac.Text = _totalFac.ToString();
                    tbTotal.Text = _aPagar.ToString();
                    //tbAmbiente.Text = "PRODUCCIÓN";
                    tbCodDoc.Text = "FACTURA";
                    tbISH.Text = "0.00";
                    tbFormaPago.Text = "PAGO EN UNA SOLA EXIBICION";
                    tbSubtotal.Text = _subTotal.ToString();
                    tbPropina.Text = _propina.ToString();
                    tbOtrosCargos.Text = _otrosCargos.ToString();
                    tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");
                    rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                    trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());
                    tbIva16.Text = _iva16.ToString();
                    gvConceptos.DataSource = dataTableConceptos;
                    gvConceptos.DataBind();
                    ddlMetodoPago_SelectedIndexChanged(null, null);
                    ddlSerieFact_SelectedIndexChanged(null, null);

                    lCountConceptos.Text = "Se encontraron <span class='badge'>" + tramas.Count + "</span> Registros ";
                    lCountConceptos.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "_keybFacturarTickets", "$('#divFacturar').modal('show');", true);
                }
                catch { }
            }
        }
        /// <summary>
        /// Tramases the checked.
        /// </summary>
        /// <param name="errores">The errores.</param>
        /// <param name="tickets">The tickets.</param>
        /// <returns>List&lt;TramaMicros&gt;.</returns>
        private List<TramaMicros> tramasChecked(out List<KeyValuePair<string, string>> errores, string[] tickets = null)
        {
            errores = new List<KeyValuePair<string, string>>();
            var tramas = new List<TramaMicros>();
            if (tickets == null)
            {
                var rows = gvTickets.Rows.OfType<GridViewRow>().Where(r => ((CheckBox)r.FindControl("check")).Checked);
                foreach (var row in rows)
                {
                    var trama = ((HiddenField)row.FindControl("checkHdTrama")).Value;
                    try
                    {
                        Session["CfdiVersion"] = "3.2"; // cambio a 3.2
                        var tramaMicros = new TramaMicros(Session["CfdiVersion"].ToString());
                        tramaMicros.Load(trama);
                        tramas.Add(tramaMicros);
                        Session["CfdiVersion"] = "3.3"; // cambio a 3.3
                    }
                    catch (Exception ex)
                    {
                        errores.Add(new KeyValuePair<string, string>(trama, ex.ToString()));
                    }
                }
            }
            else
            {
                var busqueda = "'" + string.Join("','", tickets) + "'";
                _db.Conectar();
                _db.CrearComando("SELECT DISTINCT CONVERT(VARCHAR(MAX), Trama) AS Trama, noTicket, noReserva FROM Log_Trama WHERE noReserva IN (" + busqueda + ") OR noTicket IN (" + busqueda + ") AND observaciones = 'ExtranetOk' AND tipo = 4");
                var dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    var trama = dr["Trama"].ToString();
                    try
                    {
                        var tramaMicros = new TramaMicros(Session["CfdiVersion"].ToString());
                        tramaMicros.Load(trama);
                        tramas.Add(tramaMicros);
                    }
                    catch (Exception ex)
                    {
                        errores.Add(new KeyValuePair<string, string>(trama, ex.ToString()));
                    }
                }
                _db.Desconectar();
            }
            return tramas;
        }

        /// <summary>
        /// Gets the facturadas.
        /// </summary>
        /// <param name="tramasSeleccionadas">The tramas seleccionadas.</param>
        /// <returns>List&lt;TramaMicros&gt;.</returns>
        private List<TramaMicros> GetFacturadas(List<TramaMicros> tramasSeleccionadas)
        {
            var tramas = new List<TramaMicros>();
            foreach (var trama in tramasSeleccionadas)
            {
                try
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT idTrama FROM Log_Trama WHERE observaciones = 'ExtranetOk' AND noReserva = @referencia");
                    _db.AsignarParametroCadena("@referencia", trama.Resumen.NoReferenciaAct);
                    var dr = _db.EjecutarConsulta();
                    if (!dr.Read())
                    {
                        tramas.Add(trama);
                    }
                }
                catch { }
                finally
                {
                    _db.Desconectar();
                }
            }
            return tramas;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSerieFact control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlSerieFact_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sql = "SELECT c.codigo, c.descripcion AS ambiente FROM Cat_Catalogo1_C c INNER JOIN Cat_Series s ON s.ambiente = c.codigo WHERE c.tipo = 'Ambiente' AND s.idSerie = @idSerie";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idSerie", ddlSerieFact.SelectedValue);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                tbAmbiente.Text = dr["ambiente"].ToString();
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the Click event of the bFacturarManual control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bFacturarManual_Click(object sender, EventArgs e)
        {
            try
            {
                var lineas = tbTicketsManual.Text.Split(new[]
                {
                    "\n",
                    "\r\n"
                }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyValuePair<string, string>> errores;
                var tramas = tramasChecked(out errores, lineas);
                var tramasFacturadas = GetFacturadas(tramas);
                if (tramas.Count < 1)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Debes seleccionar al menos un ticket de Micros", 4, null);
                    return;
                }
                else if (tramasFacturadas.Count > 0)
                {
                    var html = "<ul><li>" + string.Join("</li></li>", tramasFacturadas.Select(x => x.Resumen.NoReferenciaAct)) + "</li></ul>";
                    (Master as SiteMaster).MostrarAlerta(this, "Los siguientes tickets ya fueron facturados, actualice la busqueda:<br/>" + html, 4, null);
                    return;
                }
                var numLetra = new NumerosALetras();
                decimal _subTotal = 0;
                decimal _iva16 = 0;
                decimal _totalFac = 0;
                decimal _propina = 0;
                decimal _otrosCargos = 0;
                decimal _aPagar = 0;
                dataTableConceptos = new DataTable();
                if (dataTableConceptos.Columns.Count == 0)
                {
                    dataTableConceptos.Columns.Add("cantidad", typeof(string));
                    dataTableConceptos.Columns.Add("descripcion", typeof(string));
                    dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                    dataTableConceptos.Columns.Add("iva", typeof(string));
                    dataTableConceptos.Columns.Add("importe", typeof(string));
                    dataTableConceptos.Columns.Add("propina", typeof(string));
                    dataTableConceptos.Columns.Add("total", typeof(string));
                    dataTableConceptos.Columns.Add("unidad", typeof(string));
                }
                foreach (var tramaMicros in tramas)
                {
                    string descripcion = "";
                    decimal subTotal = 0;
                    decimal iva = 0;
                    decimal totalFac = 0;
                    decimal propina = 0;
                    decimal otrosCargos = 0;
                    decimal aPagar = 0;

                    try { descripcion = "Ticket " + tramaMicros.Resumen.NoReferenciaAct; } catch { }
                    try { subTotal = tramaMicros.Detalles.Sum(x => Convert.ToDecimal(x.MontoNetoAgrupado)); } catch { }
                    try { iva = tramaMicros.Impuestos.Sum(x => Convert.ToDecimal(x.MontoNetoi)); } catch { }
                    try { totalFac = tramaMicros.Detalles.Sum(x => Convert.ToDecimal(x.MontoBrutoagrupadom)); } catch { }
                    try { propina = Convert.ToDecimal(tramaMicros.Propinas != null ? tramaMicros.Propinas.MontoNetos : "0.00"); } catch { }
                    try { otrosCargos = Convert.ToDecimal("0.00"); } catch { }
                    try { aPagar = Convert.ToDecimal(tramaMicros.Resumen.MontoBruto); } catch { }

                    _subTotal += subTotal;
                    _iva16 += iva;
                    _totalFac += totalFac;
                    _propina += propina;
                    _otrosCargos += otrosCargos;
                    _aPagar += aPagar;

                    var NewRow = dataTableConceptos.NewRow();
                    NewRow[0] = "1"; // CANTIDAD
                    NewRow[1] = descripcion; // DESCRIPCION
                    NewRow[2] = subTotal.ToString(); // VAL. UNITARIO
                    NewRow[3] = iva.ToString(); // IVA
                    NewRow[4] = totalFac.ToString(); // IMPORTE
                    NewRow[5] = propina.ToString(); // PROPINA
                    NewRow[6] = aPagar.ToString(); // TOTAL
                    NewRow[7] = "NO APLICA"; // UNIDAD
                    dataTableConceptos.Rows.Add(NewRow);
                }
                _db.Conectar();
                _db.CrearComando(@"select RFCEMI,NOMEMI,dirMatriz,noExterior,noInterior,colonia,referencia,municipio,
                                 estado,pais,codigoPostal,regimenFiscal FROM Cat_Emisor where RFCEMI=@RFCEMI");
                _db.AsignarParametroCadena("@RFCEMI", Session["IDENTEMI"].ToString());
                var dr = _db.EjecutarConsulta();
                if (dr.Read()) { tbLugarExp.Text = dr[9].ToString() + ", " + dr[8].ToString(); }
                _db.Desconectar();
                tbTotalFac.Text = _totalFac.ToString();
                tbTotal.Text = _aPagar.ToString();
                //tbAmbiente.Text = "PRODUCCIÓN";
                tbCodDoc.Text = "FACTURA";
                tbISH.Text = "0.00";
                tbFormaPago.Text = "PAGO EN UNA SOLA EXIBICION";
                tbSubtotal.Text = _subTotal.ToString();
                tbPropina.Text = _propina.ToString();
                tbOtrosCargos.Text = _otrosCargos.ToString();
                tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");
                rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());
                tbIva16.Text = _iva16.ToString();
                gvConceptos.DataSource = dataTableConceptos;
                gvConceptos.DataBind();
                ddlMetodoPago_SelectedIndexChanged(null, null);
                ddlSerieFact_SelectedIndexChanged(null, null);

                lCountConceptos.Text = "Se encontraron <span class='badge'>" + tramas.Count + "</span> Registros ";
                lCountConceptos.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "_keybFacturarTickets", "$('#divFacturar').modal('show');", true);
            }
            catch (Exception ex)
            {
            }
        }
    }
}