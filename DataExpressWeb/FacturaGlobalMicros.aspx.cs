using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Datos;
using System.Collections;
using System.Data.SqlClient;
using Control;
using System.Data;
using ClosedXML.Excel;
using System.Web.UI.WebControls;
using System.Globalization;

namespace DataExpressWeb
{
    public partial class FacturaGlobalMicros : System.Web.UI.Page
    {
        private string _fechacreacion;
        private string _virtualDir = "";
        public Exception Ex;
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        private BasesDatos _db = new BasesDatos();
        /// <summary>
        /// 
        /// </summary>
        private static DataTable dataTableConceptos;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string trama = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string noticket = "";
        /// <summary>
        /// /
        /// </summary>
        public string serie = "";
        /// <summary>
        /// 
        /// </summary>
        private string _idUser = "";
        private Log _log;

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
                SqlDataSourceFacGlob.ConnectionString = _db.CadenaConexion;
                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                }
                if (!Page.IsPostBack)
                {

                }
            }

        }
        /// <summary>
        /// Tramases the checked.
        /// </summary>
        /// <param name="errores">The errores.</param>
        /// <param name="tickets">The tickets.</param>
        /// <returns>List&lt;TramaMicros&gt;.</returns>
        private List<TramaMicros> tramasChecked(out List<KeyValuePair<string, string>> errores, List<string[]> tickets)
        {
            errores = new List<KeyValuePair<string, string>>();
            var tramas = new List<TramaMicros>();
            if (tickets != null)
            {
                foreach (var ticket in tickets)
                {
                    var trama = ticket[0];
                    if (trama.StartsWith("T"))
                    {
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
                }
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
            var tramas = tramasSeleccionadas.Where(trama => isFacturada(Session["CfdiVersion"].ToString().Equals("3.3") ? trama.Resumen33.CurrentReferenceNumber : trama.Resumen.NoReferenciaAct)).ToList();
            return tramas;
        }

        private bool isFacturada(string referencia)
        {
            bool result = false;
            try
            {
                _db.Conectar();
                _db.CrearComando("SELECT idTrama FROM Log_Trama WHERE observaciones = 'ExtranetOk' AND noReserva = @referencia");
                _db.AsignarParametroCadena("@referencia", referencia);
                var dr = _db.EjecutarConsulta();
                result = !dr.Read();
            }
            catch { }
            finally
            {
                _db.Desconectar();
            }
            return result;
        }
        protected void GenerarTickets_Click(object sender, EventArgs e)
        {
            _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            if (ddlSerie.SelectedItem.Text.Equals("Todas") || string.IsNullOrEmpty(tbsucursal.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, ddlSerie.SelectedItem.Text.Equals("Todas") ? "Seleccione la serie" : "Inserte la sucursal", 4, null);
                return;
            }
            if (!string.IsNullOrEmpty(tbFecha.Text) || !string.IsNullOrEmpty(FechaFin.Text) || !string.IsNullOrEmpty(tbsucursal.Text))
            {
                var where = "";
                var sql = @"SELECT DISTINCT CONVERT(VARCHAR(MAX), Trama) AS Trama,serie,noTicket from log_trama";
                var fechain = tbFecha.Text + "-" + FechaFin.Text;
                if (!string.IsNullOrEmpty(tbsucursal.Text))
                {
                    where += (!string.IsNullOrEmpty(where) ? " AND " : " WHERE ") + "serie = " + "'" + tbsucursal.Text + "'";
                }
                where += (!string.IsNullOrEmpty(where) ? " AND " : " WHERE ") + " Trama like '%" + fechain.ToString() + "%'" + " AND " + " observaciones = " + "'ExtranetOK'" + " AND " + "tipo= " + "4";
                _db.Conectar();
                _db.CrearComando(sql + where);
                var dr = _db.EjecutarConsulta();
                var val = new List<string[]>();
                while (dr.Read())
                {
                    string[] valor = new string[3];
                    valor[0] = dr[0].ToString();
                    valor[1] = dr[1].ToString();
                    valor[2] = dr[2].ToString();
                    val.Add(valor);
                }
                _db.Desconectar();
                List<KeyValuePair<string, string>> errores;
                var tbLugarExp = "";
                var tbTotalFac = "";
                var tbTotal = "";
                var tbCodDoc = "";
                var tbISH = "";
                var tbFormaPago = "";
                var tbSubtotal = "";
                var tbPropina = "";
                var tbOtrosCargos = "";
                var tbCantLetra = "";
                var tbIva16 = "";
                var ddlMetodoPago = "";
                var serie = "";
                var tbNumCtaPago = "";
                var tbObservaciones = "";
                var facturas = new List<object[]>();
                var tramas = tramasChecked(out errores, val);
                var tramasFacturadas = GetFacturadas(tramas);
                if (tramas.Count < 1)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "No se encontraron registros", 4, null);
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
                            //requerido por parroquia
                            decimal MontoNeto = 0;

                            if (Session["CfdiVersion"].ToString().Equals("3.3"))
                            {
                                if (tramaMicros.Resumen33.CurrentReferenceNumber.Equals("21913756180557"))
                                {
                                    // Do Nothing
                                }

                                try { descripcion = tramaMicros.Resumen33.CurrentReferenceNumber; } catch { }
                                try { subTotal = tramaMicros.Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount)); } catch { }
                                //try { iva = tramaMicros.Impuestos33.Sum(x => Convert.ToDecimal(x.GrossAmount.Replace("-", ""))); } catch { }
                                #region Impuestos por Concepto

                                iva = 0;
                                foreach (var concepto in tramaMicros.Detalles33)
                                {
                                    var taxDetails = concepto.TaxBaseFactorAmountCode.Split(';');
                                    foreach (var taxDetail in taxDetails)
                                    {
                                        if (!string.IsNullOrEmpty(taxDetail))
                                        {
                                            decimal dtaxAmount = 0;
                                            var taxAmount = taxDetail.Split(' ')[2];
                                            decimal.TryParse(taxAmount, out dtaxAmount);
                                            iva += dtaxAmount;
                                        }
                                    }
                                }

                                #endregion
                                try { totalFac = tramaMicros.Detalles33.Sum(x => Convert.ToDecimal(x.GrossAmount)); } catch { }
                                try { propina = Convert.ToDecimal(tramaMicros.Propinas33 != null ? tramaMicros.Propinas33.NetAmount : "0.00"); } catch { }
                                try { otrosCargos = Convert.ToDecimal("0.00"); } catch { }
                                try { aPagar = Convert.ToDecimal(tramaMicros.Resumen33.GrossAmountGroupedP); } catch { }
                                descuentos = tramaMicros.Descuentos33 == null ? 0 : tramaMicros.Descuentos33.Sum(x => Convert.ToDecimal(x.NetAmount.Replace("-", "")));
                                totalFac = subTotal - descuentos + iva;
                                aPagar = totalFac + propina;
                            }
                            else
                            {
                                if (tramaMicros.Resumen.NoReferenciaAct.Equals("21913756180557"))
                                {
                                    // Do Nothing
                                }

                                try { descripcion = tramaMicros.Resumen.NoReferenciaAct; } catch { }
                                try { subTotal = tramaMicros.Detalles.Sum(x => Convert.ToDecimal(x.MontoNetoAgrupado)); } catch { }
                                try { iva = tramaMicros.Impuestos.Sum(x => Convert.ToDecimal(x.MontoNetoi)); } catch { }
                                try { totalFac = tramaMicros.Detalles.Sum(x => Convert.ToDecimal(x.MontoBrutoagrupadom)); } catch { }
                                try { propina = Convert.ToDecimal(tramaMicros.Propinas != null ? tramaMicros.Propinas.MontoNetos : "0.00"); } catch { }
                                try { otrosCargos = Convert.ToDecimal("0.00"); } catch { }
                                try { aPagar = Convert.ToDecimal(tramaMicros.Resumen.MontoBruto); } catch { }
                                if (tramaMicros.Descuentos != null)
                                { try { descuentos = tramaMicros.Descuentos.Sum(x => Math.Abs(Convert.ToDecimal(x.MontoNetod, new CultureInfo("en-US")))); } catch { } }
                                MontoNeto = Convert.ToDecimal(tramaMicros.Resumen.MontoNeto);//
                                totalFac = MontoNeto - propina;
                                aPagar = MontoNeto;
                            }

                            _subTotal += subTotal;
                            _iva16 += iva;
                            _totalFac += totalFac;
                            //_totalFac += MontoNeto;
                            _propina += propina;
                            _otrosCargos += otrosCargos;
                            _aPagar += aPagar;
                            _descuentos += descuentos;

                            var NewRow = dataTableConceptos.NewRow();
                            NewRow[0] = "1"; // CANTIDAD
                            NewRow[1] = descripcion; // DESCRIPCION
                            NewRow[2] = subTotal.ToString(); // VAL. UNITARIO
                            NewRow[3] = iva.ToString(); // IVA
                            NewRow[4] = subTotal.ToString(); // IMPORTE
                            NewRow[5] = propina.ToString(); // PROPINA
                            NewRow[6] = aPagar.ToString(); // TOTAL
                            NewRow[7] = "NO APLICA"; // UNIDAD
                            NewRow[8] = descuentos; // DESCUENTOS
                            dataTableConceptos.Rows.Add(NewRow);
                            facturas.Add(NewRow.ItemArray);

                        }
                        if (Session["CfdiVersion"].ToString().Equals("3.3"))
                        {
                            var claveSucursalTramas = "";
                            var tramasGrouped = tramas.GroupBy(trama => Session["CfdiVersion"].ToString().Equals("3.3") ? (trama.Resumen33.PropertyName) : (trama.Resumen.NombrePropiedad));
                            if (tramasGrouped.Count() == 1)
                            {
                                claveSucursalTramas = tramasGrouped.FirstOrDefault().Key;
                            }
                            if (!string.IsNullOrEmpty(claveSucursalTramas))
                            {
                                _db.Conectar();
                                _db.CrearComando(@"select codigoPostal from cat_sucursalesemisor where clave = @claveSucursal AND RFC = @RFCEMI");
                                _db.AsignarParametroCadena("@RFCEMI", Session["IDENTEMI"].ToString());
                                _db.AsignarParametroCadena("@claveSucursal", claveSucursalTramas);
                                dr = _db.EjecutarConsulta();
                                if (dr.Read()) { tbLugarExp = dr[0].ToString(); }
                                _db.Desconectar();
                            }
                            else
                            {
                                _db.Conectar();
                                _db.CrearComando(@"select codigoPostal FROM Cat_Emisor where RFCEMI=@RFCEMI");
                                _db.AsignarParametroCadena("@RFCEMI", Session["IDENTEMI"].ToString());
                                dr = _db.EjecutarConsulta();
                                while (dr.Read()) { tbLugarExp = dr[0].ToString(); }
                                _db.Desconectar();
                            }
                        }
                        else
                        {
                            _db.Conectar();
                            _db.CrearComando(@"select RFCEMI,NOMEMI,dirMatriz,noExterior,noInterior,colonia,referencia,municipio,
                                 estado,pais,codigoPostal,regimenFiscal FROM Cat_Emisor where RFCEMI=@RFCEMI");
                            _db.AsignarParametroCadena("@RFCEMI", Session["IDENTEMI"].ToString());
                            dr = _db.EjecutarConsulta();
                            if (dr.Read()) { tbLugarExp = dr[9].ToString() + ", " + dr[8].ToString(); }
                            _db.Desconectar();
                        }
                        tbTotalFac = _totalFac.ToString();
                        tbTotal = _aPagar.ToString();
                        //tbAmbiente.Text = "PRODUCCIÓN";
                        tbCodDoc = "FACTURA";
                        tbISH = "0.00";
                        tbFormaPago = "PAGO EN UNA SOLA EXIBICION";
                        tbSubtotal = _subTotal.ToString();
                        tbPropina = _propina.ToString();
                        tbOtrosCargos = _otrosCargos.ToString();
                        tbCantLetra = numLetra.ConvertirALetras(tbTotal, "MXN");
                        //rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Trim());
                        //trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Trim());
                        tbIva16 = _iva16.ToString();
                        ddlMetodoPago = "NA";
                        serie = (ddlSerie.SelectedItem.Text); ;
                        tbObservaciones = $"Factura global {FechaFin.SelectedItem.Text} {tbFecha.Text}";
                    }
                    catch { }
                }
                var comprobante = serie + '|' + tbFormaPago + '|' + tbSubtotal + '|' + tbTotalFac + '|' + ddlMetodoPago + '|' + tbLugarExp + '|' + tbNumCtaPago + '|' + tbOtrosCargos + '|' + tbTotal + '|' + tbObservaciones + '|' + tbPropina; ;
                var daterecep = ("PUBLICO GENERAL" + "|" + "XAXX010101000" + "|" + "" + "|" + "-" + "|" + "" + "|" + "" + "|" + "-" + "|" + "" + "|" + "" + "|" + "MEX" + "|" + "00000" + "|" + tbIva16);

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(facturas);
                _idUser = Session["idUser"].ToString();
                var FGWeb = new FacturaGlobalWeb.FacturaGlobalWeb();
                new System.Threading.Thread(() =>
                {
                    FGWeb.FacuraGlobalAsync(Session["IDENTEMI"].ToString(), Session["IDGIRO"].ToString(), json, _idUser, serie, daterecep, comprobante);
                }).Start();
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante se esta generando", 2, null);
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe sellecionar ambas fechas", 2, null);
                return;
            }
        }
        protected void Actualizar_Click(object sender, EventArgs e)
        {
            gvFacGlobWeb.DataBind();
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GenerarReporte_Click(object sender, EventArgs e)
        {
            var Reporte = new FacturaGlobalWeb.FacturaGlobalWeb();
            _fechacreacion = Localization.Now.ToString("yyyyMMddHHmmss");
            Ex = null;
            _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            var _connectionString = _db.CadenaConexion.ToString();
            if (string.IsNullOrEmpty(tbsucursal.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, "Inserte la serie de la sucursal", 4, null);
                return;
            }

            if (!string.IsNullOrEmpty(tbFecha.Text) || !string.IsNullOrEmpty(FechaFin.Text) || !string.IsNullOrEmpty(tbsucursal.Text))
            {
                var where = "";
                var sql = @"SELECT Trama from log_trama";
                var fechain = tbFecha.Text + "-" + FechaFin.Text;
                if (!string.IsNullOrEmpty(tbsucursal.Text))
                {
                    where += (!string.IsNullOrEmpty(where) ? " AND " : " WHERE ") + "serie = " + "'" + tbsucursal.Text + "'";
                }
                where += (!string.IsNullOrEmpty(where) ? " AND " : " WHERE ") + " Trama like '%" + fechain.ToString() + "%'" + " AND " + " observaciones = " + "'ExtranetOK'" + " AND " + "tipo= " + "4";
                _db.Conectar();
                _db.CrearComando(sql + where);
                var dr = _db.EjecutarConsulta();
                var val = new List<string[]>();
                while (dr.Read())
                {
                    string[] valor = new string[3];
                    valor[0] = dr[0].ToString();
                    val.Add(valor);
                }
                _db.Desconectar();
                var directorio = AppDomain.CurrentDomain.BaseDirectory;
                var rfc = Session["IDENTEMI"].ToString();
                var cfdi = Session["CfdiVersion"].ToString();
                if (val.Count > 0)
                {
                    new System.Threading.Thread(() =>
                    {
                        Reporte.GenerarReporteAsync(tbFecha.Text, FechaFin.Text, tbsucursal.Text, rfc.ToString(), cfdi, directorio);
                    }).Start();
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "No se encontraron registros", 4, null);
                    return;
                }
                gvFacGlobWeb.DataBind();
                (Master as SiteMaster).MostrarAlerta(this, "El reporte se está generando, favor de actualizar los registros nuevamente en unos minutos", 4);
            }
        }
    }
}