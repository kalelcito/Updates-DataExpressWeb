// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 01-30-2017
// ***********************************************************************
// <copyright file="DocumentosPorCobrar.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace DataExpressWeb
{
    /// <summary>
    /// Class DocumentosPorCobrar.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class DocumentosPorCobrar : System.Web.UI.Page
    {
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
        private DataTable _dt = new DataTable();
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;
        /// <summary>
        /// The _DT actual
        /// </summary>
        private DataTable _dtActual = new DataTable();
        /// <summary>
        /// The URL descarga
        /// </summary>
        public static string UrlDescarga;
        /// <summary>
        /// The URL PDF
        /// </summary>
        public static string UrlPdf;
        private static string TC_peso = "1.0";
        /// <summary>
        /// The tc dolar us liquidacion
        /// </summary>
        private static string TC_dolarUsLiquidacion = "1.0";
        /// <summary>
        /// The tc dolar us determinacion fix
        /// </summary>
        private static string TC_dolarUsDeterminacionFix = "1.0";
        /// <summary>
        /// The tc euro
        /// </summary>
        private static string TC_euro = "1.0";
        /// <summary>
        /// The tc dolar canadiense
        /// </summary>
        private static string TC_dolarCanadiense = "1.0";
        /// <summary>
        /// The tc yen
        /// </summary>
        private static string TC_yen = "1.0";
        /// <summary>
        /// The tc libra
        /// </summary>
        private static string TC_libra = "1.0";
        private string _idUser = "";
        private static CheckBox _chkboxSelectAll;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                _idUser = Session["idUser"].ToString();
                _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                SqlDataSource1.ConnectionString = _db.CadenaConexion;
                SqlDataSource2.ConnectionString = _db.CadenaConexion;
                SqlDataSourcePtoEmi.ConnectionString = _db.CadenaConexion;
                SqlDataSourcePagos.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.ConnectionString = _db.CadenaConexion;
                SqlDataSourceCtasBancos.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.SelectCommand = SqlDataMetodoPago.SelectCommand.Replace("@tipoCatalogo", Session["CfdiVersion"].ToString().Equals("3.3") ? "MetodoPagoCfdi33" : "MetodoPago");
            }
            if (!IsPostBack)
            {
                Session["gvFacturas"] = null;
                _dtActual = new DataTable();
                if (Convert.ToBoolean(Session["coFactPropias"]) || Convert.ToBoolean(Session["coFactTodas"]))
                {
                    Buscar();
                    var idComprobante = Request.QueryString.Get("comprobante");
                    if (!string.IsNullOrEmpty(idComprobante))
                    {

                    }
                }
                this.LlenarGrid();
                _dtActual = Session["gvComprobantes"] as DataTable;
                gvFacturas.DataSource = _dtActual;
                gvFacturas.DataBind();
                lCount.Text = "<strong>Se encontraron <span class='badge'>" + _dtActual.Rows.Count + "</span> Registros</strong>";
                if (!Session["IDENTEMI"].ToString().Equals("OHC080924AV5"))
                {
                    #region Moneda
                    ddlMoneda_Pago.Items.Add(new ListItem("Euros (EUR)", "EUR"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Sol Peruano (PEN)", "PEN"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Balboa Panameño (PAB)", "PAB"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Peso Colombiano (COP)", "COP"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Colón Costarricense (CRC)", "CRC"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Colónde El Salvador (SVC)", "SVC"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Gourde (HTG)", "HTG"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Boliviano (BOB)", "BOB"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Quetzal (GTQ)", "GTQ"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Real brasileño (BRL)", "BRL"));
                    ddlMoneda_Pago.Items.Add(new ListItem("Libra Esterlina (GBP)", "GBP"));
                    #endregion
                }
                LoadTiposCambio();
                bGenerarComplemento.Visible = Session["CfdiVersion"].ToString().Equals("3.3");
                gvFacturas.Columns.Cast<DataControlField>().FirstOrDefault(x => x.HeaderText.Equals("PAGOS", StringComparison.OrdinalIgnoreCase)).Visible = Session["CfdiVersion"].ToString().Equals("3.3");
            }
        }

        private void LoadTiposCambio()
        {
            try
            {
                var tipoCambio = new XmlDocument();
                var wsBan = new wsBanxico.DgieWS { Timeout = (1800 * 1000) };
                var result = wsBan.tiposDeCambioBanxico();
                if (!string.IsNullOrEmpty(result))
                {
                    tipoCambio.LoadXml(result);
                    var series = tipoCambio.GetElementsByTagName("bm:Series").Cast<XmlNode>();
                    TC_peso = "1.0";
                    TC_dolarUsLiquidacion = series.FirstOrDefault(x => Regex.IsMatch(x.Attributes["TITULO"].Value, @"d.lar E\.U\.A.*liquidaci.n", RegexOptions.IgnoreCase)).ChildNodes.Cast<XmlNode>().First(x => x.Name.Equals("bm:Obs")).Attributes["OBS_VALUE"].Value;
                    TC_dolarUsDeterminacionFix = series.FirstOrDefault(x => Regex.IsMatch(x.Attributes["TITULO"].Value, @"d.lar E\.U\.A.*(determinaci.n|FIX)", RegexOptions.IgnoreCase)).ChildNodes.Cast<XmlNode>().First(x => x.Name.Equals("bm:Obs")).Attributes["OBS_VALUE"].Value;
                    TC_euro = series.FirstOrDefault(x => Regex.IsMatch(x.Attributes["TITULO"].Value, @"Euro", RegexOptions.IgnoreCase)).ChildNodes.Cast<XmlNode>().First(x => x.Name.Equals("bm:Obs")).Attributes["OBS_VALUE"].Value;
                    TC_dolarCanadiense = series.FirstOrDefault(x => Regex.IsMatch(x.Attributes["TITULO"].Value, @"D.lar Canad.*", RegexOptions.IgnoreCase)).ChildNodes.Cast<XmlNode>().First(x => x.Name.Equals("bm:Obs")).Attributes["OBS_VALUE"].Value;
                    TC_yen = series.FirstOrDefault(x => Regex.IsMatch(x.Attributes["TITULO"].Value, @"Yen japon", RegexOptions.IgnoreCase)).ChildNodes.Cast<XmlNode>().First(x => x.Name.Equals("bm:Obs")).Attributes["OBS_VALUE"].Value;
                    TC_libra = series.FirstOrDefault(x => Regex.IsMatch(x.Attributes["TITULO"].Value, @"Libra esterlina", RegexOptions.IgnoreCase)).ChildNodes.Cast<XmlNode>().First(x => x.Name.Equals("bm:Obs")).Attributes["OBS_VALUE"].Value;
                }
            }
            catch { }
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

        //protected void ddlTipoDocumento_SelectedIndexChanged1(object sender, EventArgs e)
        //{
        //    ddlPtoEmi.Items.Clear();
        //    var nullItem = new ListItem("Todas", "0");
        //    nullItem.Selected = true;
        //    SqlDataSourcePtoEmi.SelectParameters["tipo"].DefaultValue = ddlTipoDocumento.SelectedValue;
        //    SqlDataSourcePtoEmi.DataBind();
        //    ddlPtoEmi.DataBind();
        //    ddlPtoEmi.Items.Add(nullItem);
        //}

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
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
            //if (ddlSucursal.SelectedIndex != 0)
            //{
            //    if (consulta.Length != 0) { consulta = consulta + "SU" + ddlSucursal.SelectedValue + separador; }
            //    else { consulta = "SU" + ddlSucursal.SelectedValue + separador; }
            //}
            if (ddlPtoEmi.SelectedIndex != 0)
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
            lCount.Text = "<strong>Registros encontrados: " + gvFacturas.Rows.Count + "</strong>";
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
            tbFechaInicial.Text = "";
            tbFechaFinal.Text = "";
            tbRFC.Text = "";
            tbNombre.Text = "";

            Buscar();
            Response.Redirect("~/DocumentosPorCobrar.aspx", false);
        }

        /// <summary>
        /// Limpiars the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Limpiar(object sender, EventArgs e)
        {
            tbFolioAnterior.Text = "";
        }

        /// <summary>
        /// BTNs the DPD f_ click.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>System.String.</returns>
        [WebMethod]
        public static string btnDPDF_Click(string param)
        {
            DbDataReader dr;
            var estado = "";
            var codigoControl = "";
            var pdf = "";
            var rutaCodigoControl = "";
            var idComprobante = "";
            var rutaDocus = "";
            var rutaPdf = "";
            var urlRedirect = "";
            _db.Conectar();
            _db.CrearComando(@"select dirdocs from Par_ParametrosSistema");

            dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                rutaDocus = dr[0].ToString().Trim();
            }
            _db.Desconectar();

            rutaCodigoControl = param;
            var parametros = rutaCodigoControl.Split(';');
            pdf = parametros[0];
            codigoControl = parametros[1];
            estado = parametros[2];
            idComprobante = parametros[3];
            pdf = pdf.Replace(@"docus\", "").Replace("docus//", "").Replace("docus/", "");
            rutaPdf = rutaDocus + pdf;
            rutaPdf = rutaPdf.Replace("/", @"\").Replace(@"\\", @"\").Replace("//", @"\");
            if (pdf != null && (File.Exists(rutaPdf)))
            {
                urlRedirect = "download.aspx?file=" + parametros[0];
            }
            else
            {
                urlRedirect = "descargarPDF.aspx?idFactura=" + idComprobante;
            }
            return urlRedirect;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.LoadComplete" /> event at the end of the page load stage.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_PageIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvFacturas.SelectedRow;
            var img = (ImageButton)row.Cells[0].Controls[0];
            if (img != null)
            {
                img.ImageUrl = "~/imagenes/loading.gif";
            }
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
        /// Handles the SelectedIndexChanged event of the ddlTipoDocumento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
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
            lCount.Text = "Se encontraron <span class='badge'>" + e.AffectedRows + "</span> Registros ";
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
            lMensaje.Text = "";

            var aux2 = ""; /*var aux3 = "";*/ var tipoComp = "";
            var topCopm = "''";
            if (Session["TOPComp"] != null) { topCopm = Session["TOPComp"].ToString(); }
            if (Convert.ToBoolean(Session["coFactTodas"])) { _aux = "true"; aux2 = "false"; /*aux3 = "false";*/ } else { _aux = "false"; aux2 = "true"; }

            if (Convert.ToBoolean(Session["coFactPropias"]))
            {
                aux2 = "true";
                //if (Convert.ToBoolean(Session["coFactPropiasPtoEmi"])) { aux3 = "true"; } else { aux3 = "false"; }
            }
            else { aux2 = "false";/* aux3 = "false";*/ }
            //Session["CNSComp"]
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
            //log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            _db.Conectar();
            _db.CrearComandoProcedimiento("PA_facturas_basico");
            _db.AsignarParametroProcedimiento("@QUERY", System.Data.DbType.String, _consulta);
            //_db.AsignarParametroProcedimiento("@SUCURSAL", System.Data.DbType.String, Session["claveSucursalUser"].ToString());
            _db.AsignarParametroProcedimiento("@RFC", System.Data.DbType.String, Session["rfcCliente"].ToString());
            _db.AsignarParametroProcedimiento("@ROL", System.Data.DbType.Boolean, _aux);
            _db.AsignarParametroProcedimiento("@empleado", System.Data.DbType.String, Session["USERNAME"].ToString());
            _db.AsignarParametroProcedimiento("@ROLSUCURSAL", System.Data.DbType.String, aux2);
            _db.AsignarParametroProcedimiento("@TOP", System.Data.DbType.String, topCopm);
            //_db.AsignarParametroProcedimiento("@CNSptoemi", System.Data.DbType.String, aux3);
            _db.AsignarParametroProcedimiento("@PTOEMII", System.Data.DbType.String, "1");
            _db.AsignarParametroProcedimiento("@CNSComp", System.Data.DbType.String, tipoComp);
            _db.AsignarParametroProcedimiento("@QUERYSTRING", DbType.String, "Dat_General.tipo <> 'C' AND Dat_General.estado IN ('1', '4') AND Dat_General.codDoc = '01'" + (Session["CfdiVersion"].ToString().Equals("3.3") ? " AND Dat_General.version = '3.3' AND ((SELECT DISTINCT TOP 1 p1.formapago FROM Dat_pagos p1 WHERE p1.id_Comprobante=Dat_General.idcomprobante AND p1.total = (SELECT MAX(p2.total) FROM Dat_pagos p2 WHERE p2.id_Comprobante=p1.id_Comprobante GROUP BY p2.idPagos)) NOT LIKE '%SOLA%EXHIBIC%' OR Dat_General.isCredito = '1')" : ""));
            _dtActual.Load(_db.EjecutarConsulta());
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
        /// Handles the SelectedIndexChanged event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_SelectedIndexChanged(object sender, GridViewSortEventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            if (_dtActual != null)
            {
                var dataView = new DataView(_dtActual);
                if (Session["ordenar"].ToString() == "ASC")
                {
                    e.SortDirection = SortDirection.Descending;
                }
                else
                {
                    e.SortDirection = SortDirection.Ascending;
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
        /// Handles the Click event of the bImprimirComprobante control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bImprimirComprobante_Click(object sender, EventArgs e)
        {
            var urlRedirect = "print.aspx?idFactura=" + hfIdImpresion.Value + "&printer=" + hfNombreImpresora.Value;
            Response.Redirect(urlRedirect);
        }

        /// <summary>
        /// Handles the Click event of the lbActualizarPago control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbActualizarPago_Click(object sender, EventArgs e)
        {
            var id = hfIdPagos.Value;
            decimal saldo = 0;
            decimal aplicado = 0;
            decimal pendiente = 0;
            var edo = 0;
            decimal.TryParse(CerosNull(tbSaldo.Text), out saldo);
            decimal.TryParse(CerosNull(tbPagoAplicado.Text), out aplicado);
            decimal.TryParse(CerosNull(tbPagoPendiente.Text), out pendiente);
            if (pendiente > 0 && pendiente < saldo)
            {
                edo = 2;
            }
            else if (pendiente <= 0)
            {
                edo = 1;
            }
            else if (pendiente >= saldo)
            {
                edo = 0;
            }
            _db.Conectar();
            _db.CrearComando(@"UPDATE Dat_General SET pagoAplicado=(pagoAplicado+@aplicado), saldoPendiente=@pendiente, estadoPago=@estado WHERE idComprobante = @ID");
            _db.AsignarParametroCadena("@ID", id);
            _db.AsignarParametroFlotante("@aplicado", CerosNull(tbPagoAplicado.Text));
            _db.AsignarParametroCadena("@pendiente", CerosNull(tbPagoPendiente.Text));
            _db.AsignarParametroCadena("@estado", edo.ToString());
            _db.EjecutarConsulta1();
            _db.Desconectar();
            Buscar();
            tbPagoPendiente.Text = CerosNull(pendiente.ToString());
            hfPagoPendiente.Value = CerosNull(pendiente.ToString());
            tbPagoAplicado.Text = "0.00";
            hfPagoAplicado.Value = "0.00";
            tbSaldo.Text = CerosNull(saldo.ToString());
            hfSaldo.Value = CerosNull(saldo.ToString());
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
        /// Calculars the pago.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void CalcularPago(object sender, EventArgs e)
        {
            decimal saldo = 0;
            decimal aplicado = 0;
            decimal pendiente = 0;
            decimal.TryParse(CerosNull(tbPagoAplicado.Text), out aplicado);
            decimal.TryParse(CerosNull(!string.IsNullOrEmpty(tbPagoPendiente.Text) ? tbPagoPendiente.Text : hfPagoPendiente.Value), out pendiente);
            decimal.TryParse(CerosNull(hfSaldo.Value), out saldo);
            tbSaldo.Text = CerosNull(saldo.ToString());
            hfSaldo.Value = CerosNull(saldo.ToString());
            hfPagoPendiente.Value = CerosNull(pendiente.ToString());
            tbPagoPendiente.Text = CerosNull(pendiente.ToString());
            pendiente -= aplicado;
            if (pendiente >= 0)
            {
                hfPagoAplicado.Value = CerosNull(aplicado.ToString());
                hfPagoPendiente.Value = CerosNull(pendiente.ToString());
                tbPagoPendiente.Text = CerosNull(pendiente.ToString());
            }
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

        protected void lbGuardarPagosComprobante_Click(object sender, EventArgs e)
        {
            var jsonPagoManual = "";
            var tramaOriginal = "";
            var ambiente = false;
            var list = (List<DoctosRelacionadosTemp>)Session["_doctosRelacionados"];
            try
            {
                if (string.IsNullOrEmpty(tbFecha_Pago.Text))
                {
                    throw new Exception("La fecha del pago es obligatoria");
                }
                else if (tbRfcEmisorCtaOrd_Pago.Text.Equals("XEXX010101000") && string.IsNullOrEmpty(tbBancoOrd_Pago.Text))
                {
                    throw new Exception("Cuando se defina el RFC genérico extranjero como emisor de la cuenta ordenante, se debe escribir el nombre del banco en cuestión");
                }
                else if (!string.IsNullOrEmpty(tbCtaOrd_Pago.Text) && (tbCtaOrd_Pago.Text.Length < 10 || tbCtaOrd_Pago.Text.Length > 50))
                {
                    throw new Exception("El número de la cuenta ordenante debe tener entre 10 y 50 caractéres");
                }
                else if (!string.IsNullOrEmpty(tbCtaBen_Pago.Text) && (tbCtaBen_Pago.Text.Length < 10 || tbCtaBen_Pago.Text.Length > 50))
                {
                    throw new Exception("El número de la cuenta beneficiaria debe tener entre 10 y 50 caractéres");
                }
                else if (!ddlTipoCad_Pago.SelectedValue.Equals("") && (string.IsNullOrEmpty(tbCert_Pago.Text.Trim()) || string.IsNullOrEmpty(tbCad_Pago.Text.Trim()) || string.IsNullOrEmpty(tbSello_Pago.Text.Trim())))
                {
                    throw new Exception("El certificado, cadena y sello del pago sob obligatorios cuando el tipo de cadena de pago contenga algún valor");
                }
                var propertys = new Dictionary<string, object>();
                propertys.Add("FechaPago", tbFecha_Pago.Text);
                propertys.Add("FormaPago", ddlForma_Pago.SelectedValue);
                propertys.Add("MonedaPago", ddlMoneda_Pago.SelectedValue);
                propertys.Add("MontoPago", tbMonto_Pago.Text);
                propertys.Add("NumOperacion", tbNumOperacion_Pago.Text);
                propertys.Add("TipoCambioPago", tbTipoCambio_Pago.Text);
                var rfcReceptor = "";
                foreach (var doctoRelacionado in list)
                {
                    var id = doctoRelacionado.IdComprobante;
                    var uuid = doctoRelacionado.Uuid;
                    DbDataReader dr;
                    if (!string.IsNullOrEmpty(id))
                    {
                        try
                        {
                            _db.Conectar();
                            _db.CrearComando("SELECT TOP 1 t.Trama, g.ambiente, g.numeroAutorizacion AS uuid, r.RFCREC AS rfcReceptor FROM Log_Trama t INNER JOIN Dat_General g ON g.idTrama = t.idTrama INNER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor WHERE g.idComprobante = @id ORDER BY t.idTrama DESC");
                            _db.AsignarParametroCadena("@id", id);
                            dr = _db.EjecutarConsulta();
                            if (dr.Read())
                            {
                                tramaOriginal = dr["Trama"].ToString();
                                ambiente = dr["ambiente"].ToString().Equals("2");
                                uuid = dr["uuid"].ToString();
                                rfcReceptor = dr["rfcReceptor"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            _db.Desconectar();
                        }
                        if (!string.IsNullOrEmpty(tramaOriginal) && !string.IsNullOrEmpty(uuid))
                        {
                            doctoRelacionado.Trama = tramaOriginal;
                        }
                        //decimal saldoInsoluto = 0;
                        //decimal importePagado = 0;
                        //decimal.TryParse(doctoRelacionado.SaldoInsoluto, out saldoInsoluto);
                        //decimal.TryParse(doctoRelacionado.ImportePagado, out importePagado);
                        //doctoRelacionado.SaldoInsoluto = (saldoInsoluto + importePagado).ToString();
                    }
                }
                propertys.Add("RfcReceptor", rfcReceptor);
                propertys.Add("DocumentosRelacionados", list);
                propertys.Add("RfcEmisorCtaOrd", tbRfcEmisorCtaOrd_Pago.Text);
                propertys.Add("BancoOrd", tbBancoOrd_Pago.Text);
                propertys.Add("CtaOrd", tbCtaOrd_Pago.Text);
                propertys.Add("RfcEmisorCtaBen", tbRfcEmisorCtaBen_Pago.Text);
                propertys.Add("CtaBen", tbCtaBen_Pago.Text);
                propertys.Add("TipoCad", ddlTipoCad_Pago.SelectedValue);
                propertys.Add("Cert", tbCert_Pago.Text);
                propertys.Add("Cad", tbCad_Pago.Text);
                propertys.Add("Sello", tbSello_Pago.Text);
                jsonPagoManual = JsonConvert.SerializeObject(propertys);
                var ws = new wsEmision.WsEmision { Timeout = (1800 * 5000) };
                var response = ws.RecibeInfoPagos(jsonPagoManual, _idUser, Session["IDENTEMI"].ToString(), ambiente, "02", false, true);
                if (response != null)
                {
                    var xDoc = new XmlDocument();
                    xDoc.LoadXml(response.OuterXml);
                    var uuidGenerado = GetAtributte(xDoc, "UUID", "tfd:TimbreFiscalDigital");
                    // TODO OK
                    (Master as SiteMaster).MostrarAlerta(this, "El pago se aplicó correctamente", 2, null, null, "$('#divPagos33').modal('hide');");
                    bBuscar_Click(null, null);
                }
                else
                {
                    var mensaje = ws.ObtenerMensaje();
                    (Master as SiteMaster).MostrarAlerta(this, "[1] El pago no se pudo aplicar. Intentelo de nuevo por favor<br/>" + mensaje, 4, null, null, "$('#divPagos33').modal('hide');");
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "[0] El pago no se pudo aplicar. Intentelo de nuevo por favor<br/>" + ex.Message, 4, null, null, "$('#divPagos33').modal('hide');");
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

        protected void bActualizarPago_Click(object sender, EventArgs e)
        {
            //var btn = (LinkButton)sender;
            //var id = btn.CommandArgument;
            //if (Session["CfdiVersion"].ToString().Equals("3.3"))
            //{
            //    try
            //    {
            //        _db.Conectar();
            //        _db.CrearComando("SELECT TOP 1 g.total AS 'APagar', (g.total - (SELECT ISNULL(SUM(cp.MontoPago), 0.00) FROM Dat_ComplementosPago cp WHERE cp.id_Comprobante = g.idComprobante)) AS 'PendientePago', (SELECT ISNULL(SUM(cp.MontoPago), 0.00) FROM Dat_ComplementosPago cp WHERE cp.id_Comprobante = g.idComprobante) AS 'Pagado', p.formapago FROM Dat_General g LEFT OUTER JOIN Dat_pagos p ON p.id_Comprobante = g.idComprobante WHERE g.idComprobante = @id ORDER BY p.total DESC");
            //        _db.AsignarParametroCadena("@id", id);
            //        var dr = _db.EjecutarConsulta();
            //        if (dr.Read())
            //        {
            //            decimal apagar = 0;
            //            decimal pendientepago = 0;
            //            decimal pagado = 0;
            //            decimal.TryParse(dr["APagar"].ToString(), out apagar);
            //            decimal.TryParse(dr["PendientePago"].ToString(), out pendientepago);
            //            decimal.TryParse(dr["Pagado"].ToString(), out pagado);
            //            var formaPago = dr["formapago"].ToString();
            //            var isParcialidad = Regex.IsMatch(formaPago, @"parcialidad", RegexOptions.IgnoreCase);
            //            lblPagos_APagar.Text = ControlUtilities.CerosNull(dr["APagar"].ToString(), true, true, false, true);
            //            lblPagos_PendientePago.Text = ControlUtilities.CerosNull(dr["PendientePago"].ToString(), true, true, false, true);
            //            lblPagos_Pagado.Text = ControlUtilities.CerosNull(dr["Pagado"].ToString(), true, true, false, true);
            //            rowUploadPayments.Style["display"] = pendientepago > 0 && isParcialidad ? "inline" : "none";
            //        }
            //        _db.Desconectar();
            //    }
            //    catch (Exception ex)
            //    {
            //        // ignored
            //        lblPagos_APagar.Text = ControlUtilities.CerosNull("0", true, true, false, true);
            //        lblPagos_PendientePago.Text = ControlUtilities.CerosNull("0", true, true, false, true);
            //        lblPagos_Pagado.Text = ControlUtilities.CerosNull("0", true, true, false, true);
            //        rowUploadPayments.Style["display"] = "none";
            //    }
            //    hfIdComprobantePago.Value = id;
            //    SqlDataSourcePagos.SelectParameters["idComprobante"].DefaultValue = id;
            //    gvPagos.DataBind();
            //    LimpiarPago33();
            //    ScriptManager.RegisterStartupScript(this, GetType(), "btnPagos_ServerClick", "$('#divPagos33').modal('show');", true);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "btnPagos_ServerClick", "$('#divPagos32').modal('show');", true);
            //}
        }

        private void LimpiarPago33()
        {
            var txtPagos = divPagos33Content.FindDescendants<TextBox>().ToList();
            txtPagos.ForEach(txt => txt.Text = "");
            ddlForma_Pago.SelectedIndex = 0;
            ddlTipoCad_Pago.SelectedIndex = 0;
            ddlMoneda_Pago.SelectedIndex = 0;
            ddlMoneda_Pago_SelectedIndexChanged(null, null);
        }


        private string GetUuidOriginal(string idComprobante)
        {
            var uuid = "";
            try
            {
                _db.Conectar();
                _db.CrearComando(@"SELECT numeroAutorizacion AS uuid FROM Dat_General WHERE idcomprobante = @id");
                _db.AsignarParametroCadena("@id", idComprobante);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    uuid = dr["uuid"].ToString();
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
            finally { _db.Desconectar(); }
            return uuid;
        }

        private void BindDoctosRelacionadosPrimero()
        {
            var list = new List<DoctosRelacionadosTemp>();
            var rowsChecked = gvFacturas.Rows.Cast<GridViewRow>().Where(row => ((CheckBox)row.FindControl("check")).Checked).ToList();
            decimal total = 0;
            foreach (GridViewRow row in rowsChecked)
            {
                var id = ((HiddenField)row.FindControl("checkHdID")).Value;
                try
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT TOP 1 g.numeroAutorizacion AS Uuid, g.serie AS Serie, g.folio AS Folio, g.moneda AS Moneda, g.tipoCambio AS TipoCambio, g.total AS 'APagar', g.saldoPendiente AS 'PendientePago', g.pagoAplicado AS 'Pagado', p.formapago AS FormaPago FROM Dat_General g LEFT OUTER JOIN Dat_pagos p ON p.id_Comprobante = g.idComprobante WHERE g.idComprobante = @id ORDER BY p.total DESC");
                    _db.AsignarParametroCadena("@id", id);
                    var dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        decimal pendientepago = 0;
                        decimal.TryParse(dr["PendientePago"].ToString(), out pendientepago);
                        total += pendientepago;
                    }
                    _db.Desconectar();
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            decimal monto = 0;
            decimal.TryParse(tbMonto_Pago.Text, out monto);
            if (monto > total)
            {
                if (rowsChecked.Count > 1)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El monto no puede exceder a la suma de los adeudos.", 4);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El monto no puede exceder al adeudo.", 4);
                }
            }
            else
            {
                foreach (GridViewRow row in rowsChecked)
                {
                    var id = ((HiddenField)row.FindControl("checkHdID")).Value;
                    try
                    {
                        _db.Conectar();
                        _db.CrearComando("SELECT TOP 1 g.numeroAutorizacion AS Uuid, g.serie AS Serie, g.folio AS Folio, g.moneda AS Moneda, g.tipoCambio AS TipoCambio, g.total AS 'APagar', g.saldoPendiente AS 'PendientePago', g.pagoAplicado AS 'Pagado', p.formapago AS FormaPago FROM Dat_General g LEFT OUTER JOIN Dat_pagos p ON p.id_Comprobante = g.idComprobante WHERE g.idComprobante = @id ORDER BY p.total DESC");
                        _db.AsignarParametroCadena("@id", id);
                        var dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            decimal apagar = 0;
                            decimal pendientepago = 0;
                            decimal pagado = 0;
                            decimal t = 0;
                            //fdecimal.TryParse(tbImportePagado, out t);
                            decimal.TryParse(dr["APagar"].ToString(), out apagar);
                            decimal.TryParse(dr["PendientePago"].ToString(), out pendientepago);
                            decimal.TryParse(dr["Pagado"].ToString(), out pagado);
                            var formaPago = dr["FormaPago"].ToString();
                            var isParcialidad = Regex.IsMatch(formaPago, @"parcialidad", RegexOptions.IgnoreCase);
                            var uuid = dr["Uuid"].ToString();
                            var serie = dr["Serie"].ToString();
                            var folio = dr["Folio"].ToString();
                            var moneda = dr["Moneda"].ToString();
                            var tipoCambio = dr["TipoCambio"].ToString();
                            var relacionado = new DoctosRelacionadosTemp
                            {
                                IdComprobante = id,
                                Serie = serie,
                                Folio = folio,
                                Parcialidad = "",
                                MetodoPago = formaPago,
                                Moneda = moneda,
                                SaldoAnterior = pendientepago.ToString(),
                                ImportePagado = "",
                                SaldoInsoluto = pagado.ToString(),
                                TipoCambio = tipoCambio,
                                Uuid = uuid,
                                Trama = ""
                            };
                            list.Add(relacionado);
                        }
                        _db.Desconectar();
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
                Session["_doctosRelacionados"] = list;
                BindDoctosRelacionadosExistentes();
            }
        }

        protected void bGenerarComplemento_Click(object sender, EventArgs e)
        {
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                var rowsChecked = gvFacturas.Rows.Cast<GridViewRow>().Where(row => ((CheckBox)row.FindControl("check")).Checked).ToList();
                if (rowsChecked.Count <= 0)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Debe seleccionar al menos un comprobante", 4);
                    return;
                }
                var sonIguales = rowsChecked.Select(row => ((Label)(row.FindControl("lblRfcRec"))).Text).Distinct().Count() == 1;
                if (sonIguales)
                {
                    LimpiarPago33();
                    BindDoctosRelacionadosPrimero();
                    rowGridViewDoctosRelacionados.Style["display"] = "none";
                    ScriptManager.RegisterStartupScript(this, GetType(), "bGenerarComplemento_ServerClick", "$('#divPagos33').modal('show');", true);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Los comprobantes deben pertenecer al mismo RFC receptor", 4);
                }
            }
        }

        private void BindDoctosRelacionadosExistentes()
        {
            var list = (List<DoctosRelacionadosTemp>)Session["_doctosRelacionados"];
            gvDoctosRelacionados.DataSource = list;
            gvDoctosRelacionados.DataBind();
            decimal montoPago = 0;
            decimal.TryParse(tbMonto_Pago.Text, out montoPago);
            rowGridViewDoctosRelacionados.Style["display"] = montoPago <= 0 ? "none" : "inline";
            
        }

        protected void ddlMoneda_Pago_SelectedIndexChanged(object sender, EventArgs e)
        {
            //tbCantLetra.Text = numLetra.ConvertirALetras(tbMonto_Pago.Text, ddlMoneda_Pago.SelectedValue);
            switch (ddlMoneda_Pago.SelectedValue)
            {
                case "MXN": tbTipoCambio_Pago.Text = TC_peso; break;
                case "USD": tbTipoCambio_Pago.Text = TC_dolarUsLiquidacion; break;
                case "EUR": tbTipoCambio_Pago.Text = TC_euro; break;
                case "LSD": tbTipoCambio_Pago.Text = TC_libra; break;
            }
        }

        //protected void tbMonto_Pago_TextChanged(object sender, EventArgs e)
        //{
        //    BindDoctosRelacionadosPrimero();
        //}

        protected void lbSiguiente_CLick(object sender, EventArgs e)
        {
            BindDoctosRelacionadosPrimero();
        }

        protected void tbImportePagado_TextChanged(object sender, EventArgs e)
        {
            decimal importePagado = 0;
            decimal montoPago = 0;
            decimal saldoAnterior = 0;
            decimal.TryParse(tbMonto_Pago.Text, out montoPago);
            var textValue = ((TextBox)sender).Text;
            decimal.TryParse(textValue, out importePagado);
            var list = (List<DoctosRelacionadosTemp>)Session["_doctosRelacionados"];
            var uuid = ((TextBox)sender).Attributes["UUID"];
            var sumatoriaList = list.Where(docto => !docto.Uuid.Equals(uuid)).Sum(docto => decimal.Parse(ControlUtilities.CerosNull(docto.ImportePagado)));
            if (importePagado > montoPago)
            {
                ((TextBox)sender).Text = "";
                (Master as SiteMaster).MostrarAlerta(this, "El importe para el UUID actual es mayor al monto total del pago", 4);
                return;
            }
            else if (sumatoriaList + importePagado > montoPago)
            {
                ((TextBox)sender).Text = "";
                (Master as SiteMaster).MostrarAlerta(this, "La sumatoria de los importes de los documentos es mayor al monto total del pago", 4);
                return;
            }
            decimal.TryParse(list.FirstOrDefault(docto => docto.Uuid.Equals(uuid)).SaldoAnterior, out saldoAnterior);
            var insoluto = (saldoAnterior - importePagado).ToString();
            if(saldoAnterior - importePagado <0)
            {
                ((TextBox)sender).Text = "";
                (Master as SiteMaster).MostrarAlerta(this, "El importe a pagar excede el adeudo.", 4);
                return;
            }
            list.FirstOrDefault(docto => docto.Uuid.Equals(uuid)).ImportePagado = importePagado.ToString();
            list.FirstOrDefault(docto => docto.Uuid.Equals(uuid)).SaldoInsoluto = insoluto;
            Session["_doctosRelacionados"] = list;
        }

        protected void tbParcialidad_TextChanged(object sender, EventArgs e)
        {
            var textValue = ((TextBox)sender).Text;
            var list = (List<DoctosRelacionadosTemp>)Session["_doctosRelacionados"];
            var uuid = ((TextBox)sender).Attributes["UUID"];
            list.FirstOrDefault(docto => docto.Uuid.Equals(uuid)).Parcialidad = textValue;
            Session["_doctosRelacionados"] = list;
        }

        protected void lbHistorialPagos_Click(object sender, EventArgs e)
        {
            var style = "";
            var btn = ((LinkButton)sender);
            switch (btn.ID)
            {
                case "lbHistorialPagos":
                    style = "z-index: 9999";
                    break;
                case "lbHistorialPagosFact":
                    style = "";
                    break;
            }
            var uuid = btn.CommandArgument;
            Session["_uuidDoctoRelacionadoHistorial"] = uuid;
            SqlDataSourcePagos.SelectParameters["uuid"].DefaultValue = uuid;
            gvHistorialPagos.DataBind();
            var js = "$('#divHistorialPagos33').attr('style','" + style + "');$('#divHistorialPagos33').modal('show');";
            ScriptManager.RegisterStartupScript(this, GetType(), "lbHistorialPagos_ServerClick", js, true);
        }

        protected void gvHistorialPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvFacturas.HeaderRow != null)
            {
                _chkboxSelectAll = (CheckBox)gvFacturas.HeaderRow.FindControl("chkboxSelectAll");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lblImpPagado = (Label)(e.Row.FindControl("lblImpPagado"));
                var row = ((DataRowView)e.Row.DataItem).Row;
                var uuid = Session["_uuidDoctoRelacionadoHistorial"].ToString();
                var pagado = "";
                try
                {
                    var xmlPath = row.Field<string>("XML");
                    var rutaXml = Server.MapPath("" + xmlPath);
                    //var rutaXml = rutaDocus() + xmlPath;
                    var xDoc = new XmlDocument();
                    xDoc.Load(rutaXml);
                    var doctosRelacionados = xDoc.DocumentElement.GetElementsByTagName("pago10:DoctoRelacionado").Cast<XmlNode>();
                    var doctoUuid = doctosRelacionados.FirstOrDefault(x => x.Attributes["IdDocumento"].Value.Equals(uuid, StringComparison.OrdinalIgnoreCase));
                    pagado = doctoUuid.Attributes["ImpPagado"].Value;
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "error xml<br/>" + ex.Message, 4);
                }
                finally
                {
                    lblImpPagado.Text = pagado;
                }
            }
        }

        protected void btnPdfComplemento_Click(object sender, EventArgs e)
        {
            var command = ((LinkButton)sender).CommandArgument;
            var base64 = Convert.ToBase64String(Encoding.Default.GetBytes(command));
            var pageurl = ResolveUrl("~/descargarPDF.aspx?pago=" + base64);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "loadPdfModal('" + pageurl + "&mode=view','" + pageurl + "&mode=download');", true);
        }

        protected void ddlCtaBen_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCtaBen_Pago.Text = "";
            tbRfcEmisorCtaBen_Pago.Text = "";
            if (!ddlCtaBen.SelectedValue.Equals(""))
            {
                _db.Conectar();
                _db.CrearComando("SELECT NumeroCuenta, RfcBanco FROM Cat_BancosComplemento WHERE Id = @Id");
                _db.AsignarParametroCadena("@Id", ddlCtaBen.SelectedValue);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbCtaBen_Pago.Text = dr["NumeroCuenta"].ToString();
                    tbRfcEmisorCtaBen_Pago.Text = dr["RfcBanco"].ToString();
                }
                _db.Desconectar();
            }
        }

        protected void ddlCtaOrd_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCtaOrd_Pago.Text = "";
            tbRfcEmisorCtaOrd_Pago.Text = "";
            tbBancoOrd_Pago.Text = "";
            if (!ddlCtaOrd.SelectedValue.Equals(""))
            {
                _db.Conectar();
                _db.CrearComando("SELECT NumeroCuenta, RfcBanco, NombreBanco FROM Cat_BancosComplemento WHERE Id = @Id");
                _db.AsignarParametroCadena("@Id", ddlCtaOrd.SelectedValue);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbCtaOrd_Pago.Text = dr["NumeroCuenta"].ToString();
                    tbRfcEmisorCtaOrd_Pago.Text = dr["RfcBanco"].ToString();
                    tbBancoOrd_Pago.Text = dr["NombreBanco"].ToString();
                }
                _db.Desconectar();
            }
        }

        //directorios fuera de DataExpressWeb
        public string rutaDocus()
        {
            var _DirDocs = "";
            _db.Conectar();
            _db.CrearComando(@"select dirdocs from Par_ParametrosSistema");
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                _DirDocs = dr[0].ToString().Trim();
            }
            _db.Desconectar();

            return _DirDocs.Replace("docus\\","");
        }
    }
}