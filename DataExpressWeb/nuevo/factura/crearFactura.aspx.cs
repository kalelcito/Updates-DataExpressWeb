// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="crearFactura.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ************************************************************************
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Control;
using DataExpressWeb.wsEmision;
using Datos;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.Configuration;
using Config;
using System.Text;
using DataExpressWeb.nuevo;
using System.Web.Services;

namespace DataExpressWeb
{
    /// <summary>
    /// Class CrearFacturaMx.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CrearFacturaMx : Page
    {
        /// <summary>
        /// The _ish p
        /// </summary>
        private static decimal _ishP;
        private static decimal _iuiP;
        /// <summary>
        /// The _ish p
        /// </summary>
        private static decimal _ishBase;
        private static decimal _iuiBase;
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The _id receptor
        /// </summary>
        private string _idReceptor = "";
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log;
        /// <summary>
        /// The number letra
        /// </summary>
        private static NumerosALetras numLetra;
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
        /// <summary>
        /// The Sucursal Emisor
        /// </summary>
        private static string _sucursalEmisor = "";
        /// <summary>
        /// The tc peso
        /// </summary>
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

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
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
                _db = new BasesDatos(Session["IDENTEMI"].ToString());
                _log = new Log(Session["IDENTEMI"].ToString());
                DbDataReader dr;
                SqlDataPtoEmision.ConnectionString = _db.CadenaConexion;
                SqlDataSucursal.ConnectionString = _db.CadenaConexion;
                SqlDataSeries.ConnectionString = _db.CadenaConexion;
                SqlDataCat1.ConnectionString = _db.CadenaConexion;
                SqlDataCatImpuestos.ConnectionString = _db.CadenaConexion;
                SqlDataImpTemp.ConnectionString = _db.CadenaConexion;
                SqlDataConcTemp.ConnectionString = _db.CadenaConexion;
                SqlDataAduanerasConcTemp.ConnectionString = _db.CadenaConexion;
                SqlDataPartes.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.ConnectionString = _db.CadenaConexion;
                SqlDataReceptores.ConnectionString = _db.CadenaConexion;
                SqlDataAduanerasParteTemp.ConnectionString = _db.CadenaConexion;
                SqlDataImpLocTemp.ConnectionString = _db.CadenaConexion;
                SqlDataContactos.ConnectionString = _db.CadenaConexion;
                SqlDataSource2.ConnectionString = _db.CadenaConexion;
                SqlDataCatConc.ConnectionString = _db.CadenaConexion;
                SqlDataDescConc.ConnectionString = _db.CadenaConexion;
                SqlDataSourceUsoCfdi.ConnectionString = _db.CadenaConexion;
                SqlDataSourceUnidades.ConnectionString = _db.CadenaConexion;
                SqlDataSourceSucEmi.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.SelectCommand = SqlDataMetodoPago.SelectCommand.Replace("@tipoCatalogo", Session["CfdiVersion"].ToString().Equals("3.3") ? "MetodoPagoCfdi33" : "MetodoPago");

                #region Asignar Sesion en DataSources Temporales

                SqlDataAduanerasConcTemp.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataAduanerasParteTemp.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataContactos.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataConcTemp.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataImpTemp.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataImpLocTemp.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataPartes.SelectParameters["SessionId"].DefaultValue = Session.SessionID;

                #endregion

                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                    if (!Page.IsPostBack)
                    {
                        _sucursalEmisor = "";
                        numLetra = new NumerosALetras();
                        _db.Conectar();
                        _db.CrearComando(@"delete from Dat_ImpuestosDetallesTemp where id_Empleado = @ID AND SessionId = @SessionId;
                                           delete from Dat_DetallesTemp where id_Empleado = @ID AND SessionId = @SessionId;
                                           delete from Dat_MX_DetallesAduanaTemp where id_Empleado = @ID AND SessionId = @SessionId;
                                           delete from Dat_MX_DetallesParteTemp where id_Empleado = @ID AND SessionId = @SessionId;
                                           delete from Dat_MX_ImpLocalesTemp where id_Empleado = @ID AND SessionId = @SessionId;
                                           delete from Cat_Mx_Contactos_Temp where id_Empleado = @ID AND SessionId = @SessionId;
                                           delete from Dat_DetallesNominaTemp where idUser = @ID AND SessionId = @SessionId;");
                        var deleted = false;
                        var commas = _db.Comando.CommandText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Count();
                        for (int i = 0; i < commas; i++)
                        {
                            _db.AsignarParametroCadena("@ID", _idUser);
                            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                        }
                        do
                        {
                            try
                            {
                                _db.EjecutarConsulta1();
                                deleted = true;
                            }
                            catch
                            {
                            }
                        } while (!deleted);
                        _db.Desconectar();
                        ddlTipoImpuesto_SelectedIndexChanged(null, null);
                        Llenarlista(Session["rfcSucursal"].ToString(), "emi");
                        ddlCatConc.DataBind();
                        ddlSerie.DataBind();
                        ddlSerie.SelectedIndex = 0;
                        ddlSerie_SelectedIndexChanged(null, null);

                        #region Giro Empresarial

                        if (Session["IDGIRO"] != null)
                        {
                            if (Session["IDGIRO"].ToString().Contains("1"))
                            {
                                #region Hotel

                                bPartes.Visible = false;
                                bAduanas.Visible = false;
                                divPredial.Visible = false;
                                rowHabEv.Visible = (Session["IDENTEMI"].ToString().Equals("DEC9606203K9") || Session["IDENTEMI"].ToString().Equals("SER960911DE7")) ? false : true;
                                trOtrosCargos.Visible = true;
                                rowPropOtros.Visible = true;
                                rowPropina.Visible = true;
                                tbUnidadConc.ReadOnly = true;
                                //var toRemove = from i in ddlCatConc.Items.Cast<ListItem>() where i.Text.IndexOf("HOTEL", StringComparison.OrdinalIgnoreCase) < 0 select i;
                                //var lista = toRemove.ToList();
                                //foreach (var item in lista)
                                //{
                                //    ddlCatConc.Items.Remove(item);
                                //}
                                if (ddlCatConc.Items.Count <= 1)
                                {
                                    ddlCatConc.Items.Add(new ListItem("OTRO", "0"));
                                }
                                ddlCatConc.Enabled = ddlCatConc.Items.Count >= 2;
                                SelectConceptosFromCat();
                                //tbPropina.ReadOnly = false;
                                tbPropinaConc.ReadOnly = false;
                                _db.Conectar();
                                _db.CrearComando(@"SELECT ISNULL(SUM(valor), 0.00) AS ISH, ISNULL(SUM(porcentajeBase), 0.00) AS Base FROM Cat_CatImpuestos_C WHERE tipo = '03' AND descripcion LIKE '%ISH%'");
                                dr = _db.EjecutarConsulta();
                                var sish = "";
                                var sishbase = "";
                                if (dr.Read())
                                {
                                    sish = CerosNull(dr["ISH"].ToString());
                                    sishbase = CerosNull(dr["Base"].ToString());
                                }
                                decimal.TryParse(sish, out _ishP);
                                decimal.TryParse(sishbase, out _ishBase);
                                _db.Desconectar();
                                ///IUI
                                var textIMP = "";
                                if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HCQ100621378|LAN7008173R5"))
                                {
                                    _db.Conectar();
                                    _db.CrearComando(@"SELECT ISNULL(valor, 0.00) AS IUI, ISNULL(porcentajeBase, 0.00) AS Base, ISNULL(comentarios, 'OTRO') AS NOM FROM Cat_CatImpuestos_C WHERE tipo = '03' AND descripcion LIKE '%IUI%'");
                                    dr = _db.EjecutarConsulta();
                                    var siui = "";
                                    var siuibase = "";
                                    if (dr.Read())
                                    {
                                        siui = CerosNull(dr["IUI"].ToString());
                                        siuibase = CerosNull(dr["Base"].ToString());
                                        textIMP = dr["NOM"].ToString();
                                    }
                                    decimal.TryParse(siui, out _iuiP);
                                    decimal.TryParse(siuibase, out _iuiBase);
                                    _db.Desconectar();


                                }
                                ///

                                chkHabilitarIUI.Text = textIMP;
                                var hasIsh = _ishP > 0;
                                var hasiui = _iuiP > 0;
                                trISProp.Visible = hasIsh;
                                if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HCQ100621378|LAN7008173R5"))
                                {
                                    trIUIProp.Visible = hasiui;
                                }
                                lblISHPrer.Text = CerosNull(_ishP.ToString());
                                lblIUIPrer.Text = CerosNull(_iuiP.ToString());
                                tbFormaPago.Text = "PAGO EN UNA SOLA EXHIBICIÓN";
                                tbFormaPago.ReadOnly = true;
                                btnFormaPago.Disabled = true;
                                rowExpedidoEn.Visible = false;
                                rowDescuento.Visible = false;
                                rowCondiciones.Visible = Regex.IsMatch(Session["IDENTEMI"].ToString(), @"SGO050614JG0", RegexOptions.IgnoreCase);
                                divDescuentoTot.Visible = false;
                                //divTipoCambio.Visible = true;
                                divIdentificacion.Visible = false;
                                divUnidad.Visible = false;
                                //rowCategoria.Visible = false;
                                rowDenomSocial.Visible = false;
                                chkHabilitarIsh.Attributes.Add("checkStrings", lblISHPrer.Text + "%|0%");
                                if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HCQ100621378|LAN7008173R5", RegexOptions.IgnoreCase))
                                {
                                    chkHabilitarIUI.Attributes.Add("checkStrings", lblIUIPrer.Text + "%|0%");
                                }
                                rowISHEdit.Visible = false;
                                rowIUIEdit.Visible = false;
                                tbISHEdit.Text = "0.00";
                                tbReferenciaRestaurante.Text = "";
                                divReferenciaRestaurante.Style["display"] = "inline";

                                if (Session["IDENTEMI"].ToString().Equals("OHC080924AV5") || Session["IDENTEMI"].ToString().Equals("DEC9606203K9") || Session["IDENTEMI"].ToString().Equals("SER960911DE7"))
                                {
                                    divAdendaPropina.Style["display"] = "inline";
                                }
                                if (Session["IDENTEMI"].ToString().Equals("GIO100406FS6") || Session["IDENTEMI"].ToString().Equals("IET661110DF2") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                                {
                                    rowTipoHabitacion.Style["display"] = "inline";
                                    tbTipoHabitacion.Visible = true;
                                }

                                #endregion
                            }
                            else
                            {
                                //var toRemove = from i in ddlCatConc.Items.Cast<ListItem>() where i.Text.IndexOf("HOTEL", StringComparison.OrdinalIgnoreCase) >= 0 select i;
                                //var lista = toRemove.ToList();
                                //foreach (var item in lista)
                                //{
                                //    ddlCatConc.Items.Remove(item);
                                //}
                                if (ddlCatConc.Items.Count <= 1)
                                {
                                    ddlCatConc.Items.Add(new ListItem("OTRO", "0"));
                                }
                                ddlCatConc.Enabled = true;
                                SelectConceptosFromCat();
                                if (Session["IDGIRO"].ToString().Contains("2"))
                                {
                                    #region Restaurante

                                    bPartes.Visible = false;
                                    bAduanas.Visible = false;
                                    divPredial.Visible = false;
                                    rowHabEv.Visible = false;
                                    trOtrosCargos.Visible = true;
                                    rowPropOtros.Visible = true;
                                    rowPropina.Visible = true;
                                    tbPropinaConc.ReadOnly = false;
                                    tbFormaPago.Text = "PAGO EN UNA SOLA EXHIBICIÓN";
                                    tbFormaPago.ReadOnly = true;
                                    btnFormaPago.Disabled = true;
                                    rowExpedidoEn.Visible = false;
                                    rowDescuento.Visible = false;
                                    rowCondiciones.Visible = false;
                                    //divTipoCambio.Visible = false;
                                    divIdentificacion.Visible = false;
                                    divUnidad.Visible = false;
                                    //rowCategoria.Visible = false;
                                    rowDenomSocial.Visible = false;
                                    tbReferenciaRestaurante.Text = "";
                                    divReferenciaRestaurante.Style["display"] = "inline";
                                    #endregion
                                }
                                else if (Session["IDGIRO"].ToString().Contains("3"))
                                {
                                    #region Empresa



                                    #endregion
                                }
                            }
                        }

                        #endregion

                        Array_registros.Clear();
                        id_Renglon = 0;
                        //chkMetodoPago_SelectedIndexChanged(null, null);
                        chkHabilitarLeyendasFiscales.Visible = true; // Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5");
                        chkHabilitarComercioExterior.Visible = Session["IDENTEMI"].ToString().Equals("LAN7008173R5");
                        chkHabilitarTerceros.Visible = true; // Session["IDENTEMI"].ToString().Equals("LAN7008173R5");
                        LoadTiposCambio();
                        lbPrevisualizar.Style["display"] = Session["IDGIRO"].ToString().Contains("1") && (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|SET920324FC3")) ? "inline" : "none";
                        CustomValidator1.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                        //var browser = Request.Browser;
                        //if (!string.IsNullOrEmpty(browser.Browser))
                        //{
                        //    if (browser.Browser.Contains("Chrome") || browser.Browser.Contains("Safari"))
                        //    {
                        //        var controls = Controls.Cast<System.Web.UI.Control>().Where(c => c is AjaxControlToolkit.AutoCompleteExtender);
                        //        var autos = controls.Where(c => ((AjaxControlToolkit.AutoCompleteExtender)c).TargetControlID.Equals(tbRFCClienteBusqueda.ID));
                        //        foreach (AjaxControlToolkit.AutoCompleteExtender auto in autos)
                        //        {
                        //            auto.Enabled = false;
                        //        }
                        //    }
                        //}
                        #region Build ImportesTercerosTemp DataSet

                        Session["_dtImportesTercerosTemp"] = null;
                        var dtImportesTercerosTemp = new DataTable();
                        var columns = new[]{ new DataColumn("id", typeof(int)),
                            new DataColumn("tipo", typeof(string)),
                            new DataColumn("impuesto", typeof(string)),
                            new DataColumn("tasa", typeof(string)),
                            new DataColumn("importe",typeof(string)) };
                        dtImportesTercerosTemp.Columns.AddRange(columns);
                        Session["_dtImportesTercerosTemp"] = dtImportesTercerosTemp;
                        gvImpuestosTercero.DataSource = dtImportesTercerosTemp;
                        gvImpuestosTercero.DataBind();

                        #endregion
                        BindData();
                        ddlMoneda.Items.Add(new ListItem("Pesos Mexicanos (MXN)", "MXN"));
                        if (!tbRfcEmi.Equals("OHC070227M80") && !tbRfcEmi.Text.Equals("OPL000131DL3"))
                        {
                            ddlMoneda.Items.Add(new ListItem("Dólares Americanos (USD)", "USD"));
                            if (!tbRfcEmi.Equals("OHC080924AV5"))
                            {
                                #region Moneda
                                ddlMoneda.Items.Add(new ListItem("Euros (EUR)", "EUR"));
                                ddlMoneda.Items.Add(new ListItem("Sol Peruano (PEN)", "PEN"));
                                ddlMoneda.Items.Add(new ListItem("Balboa Panameño (PAB)", "PAB"));
                                ddlMoneda.Items.Add(new ListItem("Peso Colombiano (COP)", "COP"));
                                ddlMoneda.Items.Add(new ListItem("Colón Costarricense (CRC)", "CRC"));
                                ddlMoneda.Items.Add(new ListItem("Colónde El Salvador (SVC)", "SVC"));
                                ddlMoneda.Items.Add(new ListItem("Gourde (HTG)", "HTG"));
                                ddlMoneda.Items.Add(new ListItem("Boliviano (BOB)", "BOB"));
                                ddlMoneda.Items.Add(new ListItem("Quetzal (GTQ)", "GTQ"));
                                ddlMoneda.Items.Add(new ListItem("Real brasileño (BRL)", "BRL"));
                                ddlMoneda.Items.Add(new ListItem("Libra Esterlina (GBP)", "GBP"));
                                #endregion
                            }
                        }

                        DivUsoCfdi.Style["display"] = Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                        ddlUsoCfdi.Enabled = Session["CfdiVersion"].ToString().Equals("3.3");
                        RequiredFieldValidator_UsoCfdi.Enabled = Session["CfdiVersion"].ToString().Equals("3.3");
                        rowRelacionados.Style["display"] = Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                        CustomValidator1.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                        Session["_CfdiRelacionados"] = Session["CfdiVersion"].ToString().Equals("3.3") ? new List<CfdiRelacionadoTemp>() : null;
                        divTbDescConc.Style["display"] = !Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                        btnDdlUnidadConc.Style["display"] = !Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                        divTbFormaPago.Style["display"] = !Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                        ddlFormaPago.Visible = Session["CfdiVersion"].ToString().Equals("3.3");
                        ddlCatConc.DataBind();
                        //if (ddlCatConc.Items.Count > 1)
                        //{
                        //    ddlCatConc.SelectedIndex = 1;
                        //}
                        SelectConceptosFromCat();
                        lbCatalogoConceptos.Visible = Session["CfdiVersion"].ToString().Equals("3.3");
                        lbRecargar.Visible = Session["CfdiVersion"].ToString().Equals("3.3");
                        rowDescuentos33.Style["display"] = Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                        rowDescuento.Visible = !Session["CfdiVersion"].ToString().Equals("3.3");
                        divDescuentoTot.Visible = divDescuentoTot.Visible || Session["CfdiVersion"].ToString().Equals("3.3");
                        RequiredFieldValidator53.Enabled = Session["IDENTEMI"].ToString().Equals("OHC070227M80") || Session["IDENTEMI"].ToString().Equals("OPL000131DL3");
                        divChkCredito.Style["display"] = Regex.IsMatch(Session["IDENTEMI"].ToString(), @"SET920324FC3|OPL000131DL3|OHR980618BLA|OHC070227M80|LAN7008173R5|SGO050614JG0") ? "inline" : "none";
                        rowReciboRetenciones.Style["display"] = Regex.IsMatch(Session["IDENTEMI"].ToString(), @"AACS451119EF6|AEBC640406NS3|AECE870621VB3|AECE890206JS4|MOVH710826IH4|LAN7008173R5|YAN810728RW7") && Session["IDGIRO"].ToString().Contains("3") ? "block" : "none";
                        chkReciboRetenciones.Checked = false;
                        ddlTipoReciboRetencion.SelectedValue = "";
                        rowFechaEmision.Visible = Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|SGO050614JG0");
                        if (!Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|ZFM111018JQ4|YAN810728RW7"))
                        {
                            ddlSucEmi.SelectedValue = "0";
                            ddlSucEmi.Enabled = false;
                        }
                        if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"TCA130827M58"))
                        {
                            int idReceptor = 0;
                            _db.Conectar();
                            _db.CrearComando("SELECT TOP 1 IDEREC FROM Cat_Receptor WHERE RFCREC = 'XEXX010101000' ORDER BY IDEREC ASC");
                            dr = _db.EjecutarConsulta();
                            if (dr.Read())
                            {
                                int.TryParse(dr["IDEREC"].ToString(), out idReceptor);
                            }
                            _db.Desconectar();
                            Llenarlista(idReceptor, "rec");
                            LlenaContactosInicio();
                        }
                    }
                    chkHabilitarIva16.Attributes.Add("checkStrings", "16.00%|0%");
                    chkHabilitarIvaRet4.Attributes.Add("checkStrings", "4.00%|0%");
                    //  chkHabilitarIvaIUI.Attributes.Add("checkStrings", "1.00%|0%");
                    divIvaRet4.Style["display"] = Session["IDGIRO"].ToString().Contains("3") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"MOOD6707064D5|LAN7008173R5") ? "block" : "none";
                    _db.Conectar();
                    _db.CrearComando("SELECT descripcion from Cat_Mx_Paises");
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        var li = new HtmlGenericControl("li");
                        ulPaisRec.Controls.Add(li);
                        var anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("href", "#");
                        anchor.InnerText = dr[0].ToString();
                        anchor.Attributes.Add("onclick", "return changeText($(this).html(), '#" + tbPaisRec.ClientID + "');");
                        li.Controls.Add(anchor);
                    }
                    _db.Desconectar();
                }
            }
            else
            {
                Response.Redirect("~/Cerrar.aspx");
            }
        }


        /// <summary>
        /// Binds the conceptos.
        /// </summary>
        private void bindConceptos(bool mantener = false)
        {
            ulDescConc.Controls.Clear();
            _db.Conectar();
            _db.CrearComando("SELECT idConcepto, descripcion FROM Cat_CatConceptos_C WHERE idCategoria=@idCat");
            _db.AsignarParametroCadena("@idCat", ddlCatConc.SelectedValue);
            var dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                var li = new HtmlGenericControl("li");
                ulDescConc.Controls.Add(li);
                var anchor = new HtmlGenericControl("a");
                anchor.Attributes.Add("href", "#");
                anchor.InnerText = dr[1].ToString();
                anchor.Attributes.Add("onclick", "return changeText($(this).html(), '#" + tbDescConc.ClientID + "');");
                li.Controls.Add(anchor);
            }
            _db.Desconectar();
            if (!mantener)
            {
                SqlDataAduanerasConcTemp.DataBind();
                gvAduanasConcepto.DataBind();
                SqlDataPartes.DataBind();
                gvPartes.DataBind();
                SqlDataAduanerasParteTemp.DataBind();
                gvAduanaPartesTemp.DataBind();
                tbCantidadConc.Text = "";
                tbUnidadConc.Text = "";
                tbIdentConc.Text = "";
                tbDescConc.Text = "";
                tbVUConc.Text = "";
                tbImpConc.Text = "";
                tbCantidadParteConc.Text = "";
                tbUnidadParteConc.Text = "";
                tbIdentParteConc.Text = "";
                tbDescParteConc.Text = "";
                tbVUParteConc.Text = "";
                tbImpParteConc.Text = "";
                tbNumAduanaConc.Text = "";
                tbFechaAduanaConc.Text = "";
                tbAduanaConc.Text = "";
                tbNumAduanaParte.Text = "";
                tbFechaAduanaParte.Text = "";
                tbAduanaParte.Text = "";
                tbNumeroPredial.Text = "";
                chkHabilitarIva16.Checked = true;
                /*if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HCQ100621378|LAN7008173R5")) { chkHabilitarIva16.Checked = true; } else { chkHabilitarIva16.Checked = false; }*/
                chkHabilitarIsh.Checked = false;
                chkHabilitarIsh.Visible = false;
                chkHabilitarIsh.Enabled = false;

                chkHabilitarIUI.Checked = false;
                chkHabilitarIUI.Visible = false;
                chkHabilitarIUI.Enabled = false;

                chkHabilitarIvaRet4.Checked = false;
                rowISHEdit.Visible = false;
                tbISHEdit.Text = "";
            }
        }

        /// <summary>
        /// Llenas the contactos inicio.
        /// </summary>
        private void LlenaContactosInicio()
        {
            if (!string.IsNullOrEmpty(_idReceptor))
            {
                _db.Conectar();
                _db.CrearComando(@"delete from Cat_Mx_Contactos_Temp where id_Empleado = @ID AND SessionId = @SessionId;");
                var deleted = false;
                var commas = _db.Comando.CommandText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Count();
                for (int i = 0; i < commas; i++)
                {
                    _db.AsignarParametroCadena("@ID", _idUser);
                    _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                }
                do
                {
                    try
                    {
                        _db.EjecutarConsulta1();
                        deleted = true;
                    }
                    catch
                    {
                    }
                } while (!deleted);
                _db.Desconectar();
                _db.Conectar();
                _db.CrearComando(@"INSERT INTO Cat_Mx_Contactos_Temp (id_Empleado, nombre, puesto, telefono1, telefono2, correo, SessionId) (SELECT @idUser, nombre, puesto, telefono1, telefono2, correo, @SessionId FROM Cat_Mx_Contactos WHERE Cat_Mx_Contactos.idReceptor = @idReceptor)");
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                _db.AsignarParametroCadena("@idReceptor", _idReceptor);
                _db.EjecutarConsulta();
                _db.Desconectar();
                SqlDataContactos.DataBind();
                gvContactos.DataBind();
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            //var query = "SELECT IDEREC ,RFCREC ,NOMREC ,(domicilio + ' Ext.' + noExterior + ' Int.' + noInterior + '. Col. ' + colonia + '. ' + localidad + ' Mun. ' + municipio + ', ' + estado + ', ' + pais + '. C.P.: ' + codigoPostal) AS domicilio FROM Cat_Receptor";
            var sql = "";
            var where = "";
            var notEmptytbRfc = !string.IsNullOrEmpty(tbRFCClienteBusqueda.Text);
            var notEmptyRazon = !string.IsNullOrEmpty(tbRazonClienteBusqueda.Text);
            if (notEmptytbRfc)
            {
                where += "RFCREC LIKE '" + tbRFCClienteBusqueda.Text + "%'";
            }
            if (notEmptyRazon)
            {
                where += (notEmptytbRfc ? " OR " : "") + "NOMREC LIKE '" + tbRazonClienteBusqueda.Text + "%'";
            }
            if (chkRepetidos.Checked)
            {
                sql = @"SELECT IDEREC ,RFCREC ,NOMREC ,(domicilio + ' Ext.' + noExterior + ' Int.' + noInterior + '. Col. ' + colonia + '. ' + localidad + ' Mun. ' + municipio + ', ' + estado + ', ' + pais + '. C.P.: ' + codigoPostal) AS domicilio FROM Cat_Receptor";
                sql += !string.IsNullOrEmpty(where) ? " WHERE (" + where + ")" : "";
            }
            else
            {
                sql = @"WITH Receptores AS
                             (
                                  SELECT r.IDEREC ,r.RFCREC ,r.NOMREC ,(r.domicilio + ' Ext.' + r.noExterior + ' Int.' + r.noInterior + '. Col. ' + r.colonia + '. ' + r.localidad + ' Mun. ' + r.municipio + ', ' + r.estado + ', ' + r.pais + '. C.P.: ' + r.codigoPostal) AS domicilio,
                                        ROW_NUMBER() OVER(
                                            PARTITION BY r.RFCREC ORDER BY r.RFCREC ASC, r.IDEREC ASC
                                        ) counter
                                 FROM   Cat_Receptor r LEFT OUTER JOIN Cat_Mx_Contactos c ON r.IDEREC = c.idReceptor
                                 " + (!string.IsNullOrEmpty(where) ? " WHERE (" + where + ")" : "") + @"
                             )
                        SELECT *
                        FROM   Receptores
                        WHERE  counter = 1";
            }
            if (Session["IDENTEMI"].ToString().Equals("HAP9504215L5") || Session["IDENTEMI"].ToString().Equals("HSC0010193M7") || Session["IDENTEMI"].ToString().Equals("OAP981214DP3") || Session["IDENTEMI"].ToString().Equals("OHF921110BF2") || Session["IDENTEMI"].ToString().Equals("OHS0312152Z4"))
            {
                sql += " ORDER BY RFCREC ASC, IDEREC ASC";
            }
            else
            {
                sql += " ORDER BY NOMREC";
            }
            SqlDataReceptores.SelectCommand = sql;
            gvRec.DataBind();
        }

        /// <summary>
        /// Texts the boxes vacios.
        /// </summary>
        /// <param name="lista">The lista.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool TextBoxesVacios(IEnumerable<TextBox> lista)
        {
            foreach (var txt in lista)
            {
                if (!string.IsNullOrEmpty(txt.Text))
                {
                    if (txt.Visible) { return false; }
                }
            }
            return true;
        }

        /// <summary>
        /// Hays the text vacios.
        /// </summary>
        /// <param name="lista">The lista.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool HayTxtVacios(IEnumerable<TextBox> lista)
        {
            //if (tbRfcEmi.Text.Equals("OHC080924AV5"))                  
            //{
            //    foreach (var tex in lista)
            //    {
            //        if (tex.ID.Equals("tbReservacionHabitacion") || tex.Text.Equals("tbHabitacion") || tex.Text.Equals("tbTipoHabitacion") && tex.Text.Equals(""))
            //        {
            //            if (tex.Visible) { return true; }
            //        }

            //    }
            //}
            foreach (var txt in lista)
            {
                if (tbRfcEmi.Text.Equals("OHC080924AV5"))
                {
                    if (txt.ID.Equals("tbHuespedHabitacion") || txt.Text.Equals("tbFechaHabitacion2") || txt.Text.Equals("tbFechaHabitacion1") && txt.Text.Equals(""))
                    {
                        if (string.IsNullOrEmpty(txt.Text))
                        {
                            if (txt.Visible) { return true; }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txt.Text))
                    {
                        if (txt.Visible) { return true; }
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Validars the habitacion evento.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool ValidarHabitacionEvento(out string mensaje)
        {
            var control = true;
            mensaje = "";
            if (Session["IDENTEMI"].ToString().Equals("OPL000131DL3") || Session["IDENTEMI"].ToString().Equals("OHC070227M80") || Session["IDENTEMI"].ToString().Equals("DEC9606203K9") || Session["IDENTEMI"].ToString().Equals("SER960911DE7"))
            {
                return control;
            }
            var txtHabitacion = divH.FindDescendants<TextBox>();
            var txtEventos = divE.FindDescendants<TextBox>();
            if (!TextBoxesVacios(txtHabitacion))
            {
                if (HayTxtVacios(txtHabitacion))
                {
                    mensaje = "Debe llenar todos los campos de la Habitación";
                    control = false;
                }
            }
            else if (!TextBoxesVacios(txtEventos))
            {
                if (HayTxtVacios(txtEventos))
                {
                    mensaje = "Debe llenar todos los campos del Evento";
                    control = false;
                }
            }
            else
            {
                mensaje = "Para empresas con giro de Hotelería es necesario registrar una Habitación o un Evento";
                control = false;
            }
            return control;
        }

        /// <summary>
        /// Handles the Click event of the FinishButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void FinishButton_Click(object sender, EventArgs e)
        {
            try
            {
                var ambiente = false;
                switch (hfambiente.Value)
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
                var txtInvoice = BuildTxt();
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
                var coreMx = new WsEmision { Timeout = System.Threading.Timeout.Infinite };
                var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, "01", false, true, "", "");
                if (result != null)
                {
                    var xDoc = new XmlDocument();
                    xDoc.LoadXml(result.OuterXml);
                    Session["uuidCreado"] = GetAtributte(xDoc, "UUID", "tfd:TimbreFiscalDigital");
                    Response.Redirect("~/Documentos.aspx", false);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "[1] El comprobante no se creó correctamente<br/>" + coreMx.ObtenerMensaje(), 4, null);
                }
                //}
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "[0] El comprobante no se creó correctamente<br/>" + ex.Message, 4, null);
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
        /// Handles the SelectedIndexChanged event of the ddlTipoImpuesto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbTasa.Text = "";
            tbImporte.Text = "";
            switch (ddlTipoImpuesto.SelectedValue)
            {
                case "01":
                case "":
                    SqlDataCatImpuestos.SelectParameters["tipo"].DefaultValue = "Impuesto Retenido";
                    tbTasa.ReadOnly = true;
                    tbImporte.ReadOnly = false;
                    RequiredFieldValidator2.Enabled = true;
                    RequiredFieldValidator3.Enabled = false;
                    break;
                case "02":
                    SqlDataCatImpuestos.SelectParameters["tipo"].DefaultValue = "Impuesto Trasladado";
                    tbTasa.ReadOnly = false;
                    tbImporte.ReadOnly = true;
                    RequiredFieldValidator2.Enabled = false;
                    RequiredFieldValidator3.Enabled = true;
                    break;
                default:
                    SqlDataCatImpuestos.SelectParameters["tipo"].DefaultValue = "";
                    break;
            }
            SqlDataCatImpuestos.DataBind();
            ddlImpuesto.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlImpuesto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the TextChanged event of the tbDescuentoP control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbDescuentoP_TextChanged(object sender, EventArgs e)
        {
            CalcularTotales();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTasaIVA control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTasaIVA_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Click event of the bBuscarCliente control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscarCliente_Click(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// Handles the Click event of the bLimpiarBusquedaCliente control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bLimpiarBusquedaCliente_Click(object sender, EventArgs e)
        {
            LimpiarBusquedaCliente();
        }

        /// <summary>
        /// Limpiars the busqueda cliente.
        /// </summary>
        private void LimpiarBusquedaCliente()
        {
            tbRFCClienteBusqueda.Text = "";
            tbRazonClienteBusqueda.Text = "";
            BindData();
        }

        /// <summary>
        /// Handles the Click event of the bUsarRecep control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bUsarRecep_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var idReceptor = 0;
            int.TryParse(btn.CommandArgument, out idReceptor);
            Llenarlista(idReceptor, "rec");
            LimpiarBusquedaCliente();
        }

        /// <summary>
        /// Handles the Click event of the bAgregarImpuesto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarImpuesto_Click(object sender, EventArgs e)
        {
            _db.Conectar();
            _db.CrearComando(@"insert into Dat_ImpuestosDetallesTemp
                           (codigo" + (!string.IsNullOrEmpty(tbTasa.Text) ? ",tarifa" : "") + @"" + (!string.IsNullOrEmpty(tbImporte.Text) ? ",valor" : "") + @",codigoTemp,id_Empleado,SessionId)
                           values
                           (@codigo" + (!string.IsNullOrEmpty(tbTasa.Text) ? ",@tarifa" : "") + @"" + (!string.IsNullOrEmpty(tbImporte.Text) ? ",@valor" : "") + @",@codigoTemp,@id_Empleado,@SessionId)");
            _db.AsignarParametroCadena("@codigo", ddlImpuesto.SelectedValue);
            if (!string.IsNullOrEmpty(tbTasa.Text))
            {
                _db.AsignarParametroCadena("@tarifa", CerosNull(tbTasa.Text));
            }
            if (!string.IsNullOrEmpty(tbImporte.Text))
            {
                _db.AsignarParametroCadena("@valor", CerosNull(tbImporte.Text));
            }
            _db.AsignarParametroCadena("@codigoTemp", ddlTipoImpuesto.SelectedValue);
            _db.AsignarParametroCadena("@id_Empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.EjecutarConsulta1();
            _db.Desconectar();
            tbTasa.Text = "";
            tbImporte.Text = "";
            CalculaImpuestos();
        }

        /// <summary>
        /// Calculas the impuestos.
        /// </summary>
        private void CalculaImpuestos()
        {
            SqlDataImpTemp.DataBind();
            gvImpuestos.DataBind();
            CalcularTotales();
        }

        /// <summary>
        /// Calculas the impuestos locales.
        /// </summary>
        private void CalculaImpuestosLocales()
        {
            SqlDataImpLocTemp.DataBind();
            gvImpuestosLocales.DataBind();
            CalcularTotales();
        }

        /// <summary>
        /// Calculars the totales.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void CalcularTotales(object sender = null, EventArgs e = null)
        {
            decimal subTotal = 0;
            decimal impuestosTra = 0;
            decimal impuestosTraLoc = 0;
            decimal impuestosRet = 0;
            decimal impuestosRetLoc = 0;
            decimal descuento = 0;
            decimal total = 0;
            decimal iva16 = 0;
            decimal propina = 0;
            decimal otrosCargos = 0;
            decimal ish = 0;
            decimal iui = 0;
            var sql = @"SELECT ISNULL(SUM(precioTotalSinImpuestos), 0.00) AS subtotal, ISNULL(SUM(descuento), 0.00) as iva16, ISNULL(SUM(ish), 0.00) AS ish, ISNULL(SUM(CONVERT(FLOAT, descuentoCfdi33)), 0.00) as descuento, ISNULL(SUM(otherIMP), 0.00) AS iui FROM Dat_DetallesTemp WHERE (id_Empleado = @idUser AND SessionId = @SessionId)";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idUser", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                decimal.TryParse(CerosNull(dr["subtotal"].ToString()), out subTotal);
                decimal.TryParse(CerosNull(dr["iva16"].ToString()), out iva16);
                decimal.TryParse((dr["ish"].ToString()), out ish);
                decimal.TryParse((dr["descuento"].ToString()), out descuento);
                decimal.TryParse((dr["iui"].ToString()), out iui);
            }
            _db.Desconectar();

            sql = @"SELECT tarifa FROM Dat_ImpuestosDetallesTemp WHERE codigoTemp = @codigo AND id_Empleado = @empleado AND SessionId = @SessionId";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.AsignarParametroCadena("@codigo", "02");
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                decimal tasa = 0;
                decimal.TryParse(CerosNull(dr["tarifa"].ToString(), true), out tasa);
                impuestosTra += subTotal * tasa / 100;
            }
            _db.Desconectar();

            sql = @"SELECT ISNULL(SUM(valor), 0.00) AS total FROM Dat_ImpuestosDetallesTemp WHERE codigoTemp = @codigo AND id_Empleado = @empleado AND SessionId = @SessionId";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.AsignarParametroCadena("@codigo", "01");
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                decimal val = 0;
                decimal.TryParse(CerosNull(dr["total"].ToString(), true), out val);
                impuestosRet += val;
            }
            _db.Desconectar();

            sql = @"SELECT tasa FROM Dat_MX_ImpLocalesTemp WHERE tipo = @tipo AND id_Empleado = @empleado AND SessionId = @SessionId";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.AsignarParametroCadena("@tipo", "1");
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                decimal tasa = 0;
                decimal.TryParse(CerosNull(dr["tasa"].ToString()), out tasa);
                impuestosTraLoc += subTotal * tasa / 100;
            }
            _db.Desconectar();
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@empleado", _idUser);
            _db.AsignarParametroCadena("@tipo", "2");
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                decimal tasa = 0;
                decimal.TryParse(CerosNull(dr["tasa"].ToString()), out tasa);
                impuestosRetLoc += subTotal * tasa / 100;
            }
            _db.Desconectar();

            //decimal.TryParse(CerosNull(tbDescuentoP.Text), out descuento);
            decimal.TryParse(CerosNull(tbPropinaConc.Text), out propina);
            decimal.TryParse(CerosNull(tbOtrosCargosConc.Text), out otrosCargos);
            //descuento = subTotal * descuento / 100;
            total = subTotal - descuento + impuestosTra - impuestosRet + impuestosTraLoc - impuestosRetLoc + iva16 + ish + iui;
            decimal totalPago = total + propina + otrosCargos;
            tbIva16.Text = CerosNull(iva16.ToString());
            tbDescuento.Text = CerosNull(descuento.ToString());
            tbSubtotal.Text = CerosNull(subTotal.ToString());
            tbTotalImpTras.Text = CerosNull(impuestosTra.ToString());
            tbTotalImpRet.Text = CerosNull(impuestosRet.ToString());
            tbTotalTrasLoc.Text = CerosNull(impuestosTraLoc.ToString());
            tbTotalRetLoc.Text = CerosNull(impuestosRetLoc.ToString());
            tbTotal.Text = CerosNull(totalPago.ToString());
            tbTotalFac.Text = CerosNull(total.ToString());
            tbISH.Text = CerosNull(ish.ToString());
            tbIUI.Text = CerosNull(iui.ToString());
            tbPropina.Text = CerosNull(propina.ToString());
            tbOtrosCargos.Text = CerosNull(otrosCargos.ToString());
            tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, ddlMoneda.SelectedValue);
        }

        /// <summary>
        /// Ceroses the null.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>System.String.</returns>
        private string CerosNull(string a, bool d4 = false)
        {
            decimal b;
            var cifra = (!string.IsNullOrEmpty(a) ? a : "").Replace(",", "").Trim();
            var result = string.Format("{0:0.00}", Convert.ToDecimal(string.IsNullOrEmpty(cifra) || !decimal.TryParse(cifra, out b) || b < 0 ? "0.00" : cifra));
            if (d4)
            {
                result = string.Format("{0:0.000000}", Convert.ToDecimal(string.IsNullOrEmpty(cifra) || !decimal.TryParse(cifra, out b) || b < 0 ? "0.000000" : cifra));
            }
            return result;
        }

        /// <summary>
        /// Handles the Click event of the bAgregarConcepto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarConcepto_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbDescConc.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe especificar un concepto", 4, null);
                return;
            }
            if (string.IsNullOrEmpty(tbCantidadConc.Text) || string.IsNullOrEmpty(tbVUConc.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe especificar las cantidades del concepto", 4, null);
                return;
            }
            CalculaImporteConcepto(null, null);
            DbDataReader dr;
            var idConcepto = "";
            var sql = @"INSERT INTO Dat_DetallesTemp
                                    (" + (!string.IsNullOrEmpty(tbIdentConc.Text) ? "codigoPrincipal," : "") + @"descripcion,cantidad,precioUnitario
                                    ,id_Empleado,SessionId,PrecioTotalSinImpuestos,unidad,ish" + (!string.IsNullOrEmpty(tbNumeroPredial.Text) ? ",ctaPredial" : "") + (chkHabilitarIva16.Checked ? ",descuento" : "") + @",claveProdServ,claveUnidad,descuentoCfdi33,otherIMP) OUTPUT inserted.idDetallesTemp
                                    VALUES
                                    (" + (!string.IsNullOrEmpty(tbIdentConc.Text) ? "@codigoPrincipal," : "") + @"@descripcion,@cantidad,@precioUnitario
                                   ,@id_Empleado,@SessionId,@PrecioTotalSinImpuestos,@unidad,@ish" + (!string.IsNullOrEmpty(tbNumeroPredial.Text) ? ",@ctaPredial" : "") + (chkHabilitarIva16.Checked ? ",@iva16" : "") + @",@claveProdServ,@claveUnidad,@descuentoCfdi33,@otherIMP)";
            var claveProdServ = "";
            var claveUnidad = "";
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                _db.Conectar();
                _db.CrearComando("SELECT TOP 1 claveProdServ FROM Cat_CatConceptos_C WHERE descripcion = @descripcion AND claveProdServ IS NOT NULL");
                _db.AsignarParametroCadena("@descripcion", tbDescConc.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read()) { claveProdServ = dr[0].ToString(); }
                _db.Desconectar();
                if (string.IsNullOrEmpty(claveProdServ))
                {
                    //claveProdServ = "01010101";
                    (Master as SiteMaster).MostrarAlerta(this, "El concepto no tiene asignada ninguna clave del catalogo c_ClaveProdServ del SAT", 4, null);
                    return;
                }
                foreach (var busqueda in tbUnidadConc.Text.Trim().Split(':'))
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT TOP 1 claveSat FROM Cat_CveUnidad WHERE claveSat = @busq OR descripcion = @busq AND claveSat IS NOT NULL");
                    _db.AsignarParametroCadena("@busq", busqueda);
                    _db.AsignarParametroCadena("@busq", busqueda);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read()) { claveUnidad = dr[0].ToString(); }
                    _db.Desconectar();
                }
                if (string.IsNullOrEmpty(claveUnidad))
                {
                    //claveUnidad = "ACT";
                    (Master as SiteMaster).MostrarAlerta(this, "El concepto no tiene asignada ninguna clave del catalogo c_ClaveUnidad del SAT", 4, null);
                    return;
                }
            }
            _db.Desconectar();
            _db.Conectar();
            _db.CrearComando(sql);
            if (!string.IsNullOrEmpty(tbIdentConc.Text))
            {
                _db.AsignarParametroCadena("@codigoPrincipal", tbIdentConc.Text);
            }
            decimal importe = 0;
            decimal cantidad = 0;
            decimal descuento = 0;
            decimal baseImpuesto = 0;
            decimal.TryParse(CerosNull(tbImpConc.Text), out importe);
            decimal.TryParse(CerosNull(tbCantidadConc.Text), out cantidad);
            decimal.TryParse(CerosNull(tbDescuentoConc.Text), out descuento);
            baseImpuesto = importe - descuento;
            var iva16 = chkHabilitarIva16.Checked ? (baseImpuesto * 16 / 100) : 0;
            decimal ivaRet = 0;

            // DEC9606203K9
            var rfcBase = Session["IDENTEMI"].ToString();

            var ish = (chkHabilitarIsh.Checked ? (((rfcBase.Equals("DEC9606203K9") || rfcBase.Equals("SER960911DE7")) && _ishBase > 0 ? (baseImpuesto * (_ishBase / 100)) : baseImpuesto) * _ishP / 100) : 0);
            var iui = (chkHabilitarIUI.Checked ? (((rfcBase.Equals("HCQ100621378|LAN7008173R5")) && 1 > 0 ? (baseImpuesto * (1 / 100)) : baseImpuesto) * 1 / 100) : 0);
            if (chkDesgloce.Checked)
            {
                var impuestos = (((chkHabilitarIva16.Checked ? Convert.ToDecimal(16) : 0) + (chkHabilitarIsh.Checked ? ((rfcBase.Equals("DEC9606203K9") || rfcBase.Equals("SER960911DE7")) && _ishBase > 0 ? ((_ishP * _ishBase) / 100) : _ishP) : 0)) / 100) + 1;
                var importeP = baseImpuesto / impuestos;
                iva16 = chkHabilitarIva16.Checked ? (importeP * Convert.ToDecimal(0.16)) : 0;
                ish = chkHabilitarIsh.Checked ? importeP * (_ishP / 100) : 0;
                tbVUConc.Text = (Math.Round(importeP / cantidad, 4).ToString());
                tbImpConc.Text = (Math.Round(importeP, 4).ToString());
            }
            if (chkHabilitarIsh.Checked && rowISHEdit.Visible)
            {
                decimal.TryParse(tbISHEdit.Text, out ish);
            }
            if (chkHabilitarIUI.Checked && rowIUIEdit.Visible)
            {
                decimal.TryParse(tbIUIEdit.Text, out iui);
            }
            var descripcion = tbDescConc.Text;
            if (Session["CfdiVersion"].ToString().Equals("3.3") && chkEditarItem33.Checked && !string.IsNullOrEmpty(tbCutomItem.Text.Trim()))
            {
                descripcion = tbCutomItem.Text.Trim();
            }
            _db.AsignarParametroCadena("@descripcion", descripcion);
            _db.AsignarParametroCadena("@cantidad", tbCantidadConc.Text);
            _db.AsignarParametroCadena("@precioUnitario", tbVUConc.Text);
            _db.AsignarParametroCadena("@id_Empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.AsignarParametroCadena("@PrecioTotalSinImpuestos", tbImpConc.Text);
            _db.AsignarParametroCadena("@unidad", tbUnidadConc.Text);
            _db.AsignarParametroCadena("@ish", (ish.ToString()));
            _db.AsignarParametroCadena("@otherIMP", (iui.ToString()));
            if (!string.IsNullOrEmpty(tbNumeroPredial.Text))
            {
                _db.AsignarParametroCadena("@ctaPredial", tbNumeroPredial.Text);
            }
            if (chkHabilitarIva16.Checked)
            {
                _db.AsignarParametroCadena("@iva16", CerosNull(iva16.ToString()));
            }
            _db.AsignarParametroCadena("@claveProdServ", claveProdServ);
            _db.AsignarParametroCadena("@claveUnidad", claveUnidad);
            _db.AsignarParametroCadena("@descuentoCfdi33", CerosNull(tbDescuentoConc.Text));
            dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                idConcepto = dr[0].ToString();
            }
            _db.Desconectar();
            if (!string.IsNullOrEmpty(idConcepto))
            {
                sql = @"UPDATE Dat_MX_DetallesAduanaTemp SET id_DetallesTemp = @ID WHERE ISNULL(id_DetallesTemp, '') = '' AND tipo = 1 AND id_Empleado = @idUser AND SessionId = @SessionId";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", idConcepto);
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                _db.EjecutarConsulta1();
                _db.Desconectar();
                sql = @"UPDATE Dat_MX_DetallesParteTemp SET id_DetallesTemp = @ID WHERE ISNULL(id_DetallesTemp, '') = '' AND id_Empleado = @idUser AND SessionId = @SessionId";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", idConcepto);
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                _db.EjecutarConsulta1();
                _db.Desconectar();
                if (chkHabilitarIvaRet4.Checked)
                {
                    decimal tasaIvaRet = (decimal)0.04;
                    ivaRet = baseImpuesto * tasaIvaRet;
                    _db.Conectar();
                    _db.CrearComando(@"insert into Dat_ImpuestosDetallesTemp
                           (codigo,tarifa,valor,codigoTemp,id_Empleado,SessionId,tipoFactor,id_DetallesTemp)
                           values
                           (@codigo,@tarifa,@valor,'01',@id_Empleado,@SessionId,'Tasa',@id_DetallesTemp)");
                    _db.AsignarParametroCadena("@codigo", "1"); // IVA
                    _db.AsignarParametroCadena("@tarifa", CerosNull(tasaIvaRet.ToString(), true));
                    _db.AsignarParametroCadena("@valor", CerosNull(ivaRet.ToString()));
                    _db.AsignarParametroCadena("@id_Empleado", _idUser);
                    _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                    _db.AsignarParametroCadena("@id_DetallesTemp", idConcepto);
                    _db.EjecutarConsulta1();
                    _db.Desconectar();
                }
            }
            SqlDataConcTemp.DataBind();
            gvConceptos.DataBind();
            SqlDataAduanerasConcTemp.DataBind();
            gvAduanasConcepto.DataBind();
            SqlDataPartes.DataBind();
            gvPartes.DataBind();
            SqlDataAduanerasParteTemp.DataBind();
            gvAduanaPartesTemp.DataBind();
            tbCantidadConc.Text = "";
            tbUnidadConc.Text = "";
            tbIdentConc.Text = "";
            tbDescConc.Text = "";
            tbVUConc.Text = "";
            tbImpConc.Text = "";
            tbCantidadParteConc.Text = "";
            tbUnidadParteConc.Text = "";
            tbIdentParteConc.Text = "";
            tbDescParteConc.Text = "";
            tbVUParteConc.Text = "";
            tbImpParteConc.Text = "";
            tbNumAduanaConc.Text = "";
            tbFechaAduanaConc.Text = "";
            tbAduanaConc.Text = "";
            tbNumAduanaParte.Text = "";
            tbFechaAduanaParte.Text = "";
            tbAduanaParte.Text = "";
            tbNumeroPredial.Text = "";
            chkHabilitarIva16.Checked = true;
            chkHabilitarIsh.Checked = false;
            chkHabilitarIsh.Visible = false;
            chkHabilitarIsh.Enabled = false;

            chkHabilitarIUI.Checked = false;
            chkHabilitarIUI.Visible = false;
            chkHabilitarIUI.Enabled = false;

            rowISHEdit.Visible = false;
            tbISHEdit.Text = "";
            tbCutomItem.Text = "";
            chkEditarItem33.Visible = false;
            tbDescuentoConc.Text = "";
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                try
                {
                    ddlCatConc.SelectedValue = "0";
                }
                catch { }
                try
                {
                    ddlDesCon.SelectedValue = "0";
                }
                catch { }
            }
            else
            {
                try
                {
                    //ddlCatConc.SelectedIndex = 1;
                    ddlCatConc_SelectedIndexChanged(null, null);
                }
                catch { }
            }
            //ddlCatConc.SelectedValue = "0";
            //ddlCatConc_SelectedIndexChanged(null, null);
            CalcularTotales();
            if (chkHabilitarIvaRet4.Checked)
            {
                decimal retIvaTotales = 0;
                decimal.TryParse(tbRetIva.Text, out retIvaTotales);
                retIvaTotales += ivaRet;
                tbRetIva.Text = CerosNull(retIvaTotales.ToString());
                rowTotalesArrendamiento.Visible = true;
            }
            chkHabilitarIvaRet4.Checked = false;
            rowVerDatosSat.Style["display"] = "none";
        }

        /// <summary>
        /// Calculas the importe concepto.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void CalculaImporteConcepto(object sender, EventArgs e)
        {
            tbCantidadConc.Text = tbCantidadConc.Text.Replace(".00", "");
            tbVUConc.Text = tbVUConc.Text.Replace(".00", "");
            if (!string.IsNullOrEmpty(tbCantidadConc.Text) && !string.IsNullOrEmpty(tbVUConc.Text))
            {
                decimal cantidad;
                decimal vu;
                if (decimal.TryParse(tbCantidadConc.Text, out cantidad) && decimal.TryParse(tbVUConc.Text, out vu))
                {
                    tbImpConc.Text = (cantidad * vu).ToString();
                }
                else
                {
                    tbImpConc.Text = "";
                }
            }
            else
            {
                tbImpConc.Text = "";
            }
        }

        /// <summary>
        /// Calculas the importe concepto parte.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void CalculaImporteConceptoParte(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbCantidadParteConc.Text) && !string.IsNullOrEmpty(tbVUParteConc.Text))
            {
                decimal cantidad;
                decimal vu;
                if (decimal.TryParse(tbCantidadParteConc.Text, out cantidad) && decimal.TryParse(tbVUParteConc.Text, out vu))
                {
                    tbImpParteConc.Text = (cantidad * vu).ToString();
                }
                else
                {
                    tbImpParteConc.Text = "";
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the bAgregarAduanaC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarAduanaC_Click(object sender, EventArgs e)
        {
            var sql = @"INSERT INTO Dat_MX_DetallesAduanaTemp
                                    (" + (!string.IsNullOrEmpty(tbAduanaConc.Text) ? "aduana," : "") + @"numero,fecha,id_Empleado,tipo,SessionId)
                                    VALUES
                                    (" + (!string.IsNullOrEmpty(tbAduanaConc.Text) ? "@aduana," : "") + @"@numero,@fecha,@id_Empleado,@tipo,@SessionId)";
            _db.Conectar();
            _db.CrearComando(sql);
            if (!string.IsNullOrEmpty(tbAduanaConc.Text))
            {
                _db.AsignarParametroCadena("@aduana", tbAduanaConc.Text);
            }
            _db.AsignarParametroCadena("@numero", tbNumAduanaConc.Text);
            _db.AsignarParametroCadena("@fecha", tbFechaAduanaConc.Text);
            _db.AsignarParametroCadena("@id_Empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.AsignarParametroCadena("@tipo", "1");
            _db.EjecutarConsulta1();
            _db.Desconectar();
            SqlDataAduanerasConcTemp.DataBind();
            gvAduanasConcepto.DataBind();
            tbNumAduanaConc.Text = "";
            tbFechaAduanaConc.Text = "";
            tbAduanaConc.Text = "";
            tbNumeroPredial.Text = gvPartes.Rows.Count + gvAduanasConcepto.Rows.Count > 0 ? "" : tbNumeroPredial.Text;
            tbNumeroPredial.ReadOnly = gvPartes.Rows.Count + gvAduanasConcepto.Rows.Count > 0;
        }

        /// <summary>
        /// Handles the Click event of the bAgregarParteConcepto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarParteConcepto_Click(object sender, EventArgs e)
        {
            var sql = @"INSERT INTO dbo.Dat_MX_DetallesParteTemp
                               (cantidad
                               ,unidad
                               ,noIdentificacion
                               ,descripcion
                                " + (!string.IsNullOrEmpty(tbVUParteConc.Text) ? ",valorUnitario" : "") + @"
                                " + (!string.IsNullOrEmpty(tbImpParteConc.Text) ? ",importe" : "") + @"
                               ,id_Empleado,SessionId)
                        OUTPUT inserted.idDetallesParteTemp
                         VALUES
                               (@cantidad
                               ,@unidad
                               ,@noIdentificacion
                               ,@descripcion
                               " + (!string.IsNullOrEmpty(tbVUParteConc.Text) ? ",@valorUnitario" : "") + @"
                                " + (!string.IsNullOrEmpty(tbImpParteConc.Text) ? ",@importe" : "") + @"
                               ,@id_Empleado,@SessionId)";
            _db.Conectar();
            _db.CrearComando(sql);
            if (!string.IsNullOrEmpty(tbVUParteConc.Text))
            {
                _db.AsignarParametroCadena("@valorUnitario", tbVUParteConc.Text);
            }
            if (!string.IsNullOrEmpty(tbImpParteConc.Text))
            {
                _db.AsignarParametroCadena("@importe", tbImpParteConc.Text);
            }
            _db.AsignarParametroCadena("@cantidad", tbCantidadParteConc.Text);
            _db.AsignarParametroCadena("@unidad", tbUnidadParteConc.Text);
            _db.AsignarParametroCadena("@noIdentificacion", tbIdentParteConc.Text);
            _db.AsignarParametroCadena("@descripcion", tbDescParteConc.Text);
            _db.AsignarParametroCadena("@id_Empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                var id = dr[0].ToString();
                _db.Desconectar();
                sql = @"UPDATE Dat_MX_DetallesAduanaTemp SET id_DetallesParteTemp = @ID WHERE ISNULL(id_DetallesParteTemp, '') = '' AND tipo = 2 AND id_Empleado = @id_Empleado AND SessionId = @SessionId";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", id);
                _db.AsignarParametroCadena("@id_Empleado", _idUser);
                _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                _db.EjecutarConsulta1();
                _db.Desconectar();
                SqlDataPartes.DataBind();
                gvPartes.DataBind();
                SqlDataAduanerasParteTemp.DataBind();
                gvAduanaPartesTemp.DataBind();
                tbCantidadParteConc.Text = "";
                tbUnidadParteConc.Text = "";
                tbIdentParteConc.Text = "";
                tbDescParteConc.Text = "";
                tbVUParteConc.Text = "";
                tbImpParteConc.Text = "";
            }
            else
            {
                _db.Desconectar();
            }
            tbNumeroPredial.Text = gvPartes.Rows.Count + gvAduanasConcepto.Rows.Count > 0 ? "" : tbNumeroPredial.Text;
            tbNumeroPredial.ReadOnly = gvPartes.Rows.Count + gvAduanasConcepto.Rows.Count > 0;
        }

        /// <summary>
        /// Handles the Click event of the bAgregarAduanaParte control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarAduanaParte_Click(object sender, EventArgs e)
        {
            var sql = @"INSERT INTO Dat_MX_DetallesAduanaTemp
                                    (" + (!string.IsNullOrEmpty(tbAduanaParte.Text) ? "aduana," : "") + @"numero,fecha,id_Empleado,tipo,SessionId)
                                    VALUES
                                    (" + (!string.IsNullOrEmpty(tbAduanaParte.Text) ? "@aduana," : "") + @"@numero,@fecha,@id_Empleado,@tipo,@SessionId)";
            _db.Conectar();
            _db.CrearComando(sql);
            if (!string.IsNullOrEmpty(tbAduanaParte.Text))
            {
                _db.AsignarParametroCadena("@aduana", tbAduanaParte.Text);
            }
            _db.AsignarParametroCadena("@numero", tbNumAduanaParte.Text);
            _db.AsignarParametroCadena("@fecha", tbFechaAduanaParte.Text);
            _db.AsignarParametroCadena("@id_Empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.AsignarParametroCadena("@tipo", "2");
            _db.EjecutarConsulta1();
            _db.Desconectar();
            SqlDataAduanerasParteTemp.DataBind();
            gvAduanaPartesTemp.DataBind();
            tbNumAduanaParte.Text = "";
            tbFechaAduanaParte.Text = "";
            tbAduanaParte.Text = "";
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkDomEmi control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkDomEmi_CheckedChanged(object sender, EventArgs e)
        {
            cbUsarDirecEmisor.Checked = false;
            cbUsarDirecEmisor_CheckedChanged(null, null);
            RequiredFieldValidator10.Enabled = chkDomEmi.Checked;
            RequiredFieldValidator17.Enabled = chkDomEmi.Checked;
            RequiredFieldValidator18.Enabled = chkDomEmi.Checked;
            RequiredFieldValidator19.Enabled = chkDomEmi.Checked;
            RequiredFieldValidator20.Enabled = chkDomEmi.Checked;
            if (chkDomEmi.Checked)
            {
                validationEmisorDom.Attributes.Add("habilitado", "");
            }
            else
            {
                validationEmisorDom.Attributes.Remove("habilitado");
            }
            if (sender != null || e != null)
            {
                Llenarlistadom(chkDomEmi.Checked ? tbRfcEmi.Text : "", "emi", false);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbUsarDirecEmisor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void cbUsarDirecEmisor_CheckedChanged(object sender, EventArgs e)
        {
            tbCalleExp.Text = cbUsarDirecEmisor.Checked ? tbCalleEmi.Text : tbCalleExp.Text;
            tbNoExtExp.Text = cbUsarDirecEmisor.Checked ? tbNoExtEmi.Text : tbNoExtExp.Text;
            tbNoIntExp.Text = cbUsarDirecEmisor.Checked ? tbNoIntEmi.Text : tbNoIntExp.Text;
            tbColoniaExp.Text = cbUsarDirecEmisor.Checked ? tbColoniaEmi.Text : tbColoniaExp.Text;
            //tbLocExp.Text = cbUsarDirecEmisor.Checked ? tbLocEmi.Text : tbLocExp.Text;
            //tbRefExp.Text = cbUsarDirecEmisor.Checked ? tbRefEmi.Text : tbRefExp.Text;
            tbMunicipioExp.Text = cbUsarDirecEmisor.Checked ? tbMunicipioEmi.Text : tbMunicipioExp.Text;
            tbEstadoExp.Text = cbUsarDirecEmisor.Checked ? tbEstadoEmi.Text : tbEstadoExp.Text;
            tbPaisExp.Text = cbUsarDirecEmisor.Checked ? tbPaisEmi.Text : tbPaisExp.Text;
            tbCpExp.Text = cbUsarDirecEmisor.Checked ? tbCpEmi.Text : tbCpExp.Text;
            tbCalleExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbNoExtExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbNoIntExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbColoniaExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            //tbLocExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            //tbRefExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbMunicipioExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbEstadoExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbPaisExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbCpExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbEmiExp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void cbEmiExp_CheckedChanged(object sender, EventArgs e)
        {
            cbUsarDirecEmisor.Checked = false;
            cbUsarDirecEmisor_CheckedChanged(null, null);
            tbCalleExp.Text = "";
            tbNoExtExp.Text = "";
            tbNoIntExp.Text = "";
            tbColoniaExp.Text = "";
            //tbLocExp.Text = "";
            //tbRefExp.Text = "";
            tbMunicipioExp.Text = "";
            tbEstadoExp.Text = "";
            tbPaisExp.Text = "";
            tbCpExp.Text = "";
            cbUsarDirecEmisor.Enabled = cbEmiExp.Checked;
            RequiredFieldValidator21.Enabled = cbEmiExp.Checked;
            RequiredFieldValidator31.Enabled = cbEmiExp.Checked;
            RequiredFieldValidator32.Enabled = cbEmiExp.Checked;
            RequiredFieldValidator33.Enabled = cbEmiExp.Checked;
            RequiredFieldValidator34.Enabled = cbEmiExp.Checked;
            if (cbEmiExp.Checked)
            {
                validationEmisorDomExp.Attributes.Add("habilitado", "");
            }
            else
            {
                validationEmisorDomExp.Attributes.Remove("habilitado");
            }
        }

        /// <summary>
        /// Llenarlistadoms the specified RFC.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <param name="tipo">The tipo.</param>
        /// <param name="chkDom">if set to <c>true</c> [CHK DOM].</param>
        private void Llenarlistadom(string rfc, string tipo, bool chkDom = true)
        {
            var sql = "";
            if (tipo == "emi")
            {
                sql = @"SELECT [dirMatriz]
                              ,[noExterior]
                              ,[noInterior]
                              ,[colonia]
                              ,[localidad]
                              ,[referencia]
                              ,[municipio]
                              ,[estado]
                              ,[pais]
                              ,[codigoPostal]
                          FROM [Cat_Emisor]
                          WHERE RFCEMI=@ruc";
            }
            if (tipo == "rec")
            {
                sql = @"SELECT [domicilio]
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
                          WHERE RFCREC=@ruc";
            }
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@ruc", rfc);
            var dr = _db.EjecutarConsulta();
            var control = (dr.HasRows && !rfc.Equals("XAXX010101000", StringComparison.OrdinalIgnoreCase) && !rfc.Equals("XEXX010101000", StringComparison.OrdinalIgnoreCase)) || !cbDomRec.Checked;
            if (tipo == "emi")
            {
                tbCalleEmi.ReadOnly = control;
                tbNoExtEmi.ReadOnly = control;
                tbNoIntEmi.ReadOnly = control;
                tbColoniaEmi.ReadOnly = control;
                tbMunicipioEmi.ReadOnly = control;
                tbEstadoEmi.ReadOnly = control;
                tbPaisEmi.ReadOnly = control;
                tbCpEmi.ReadOnly = control;
                tbLugarExp.ReadOnly = control;
            }
            else if (tipo == "rec")
            {
                var controlTemp = control;
                if (Session["IDENTEMI"].ToString().Equals("OHR980618BLA"))
                {
                    controlTemp = false;
                }
                tbCalleRec.ReadOnly = controlTemp;
                tbNoExtRec.ReadOnly = controlTemp;
                tbNoIntRec.ReadOnly = controlTemp;
                tbColoniaRec.ReadOnly = controlTemp;
                tbMunicipioRec.ReadOnly = controlTemp;
                tbEstadoRec.ReadOnly = controlTemp;
                tbPaisRec.ReadOnly = controlTemp;
                if (controlTemp)
                {
                    btnPaisRec.Attributes["disabled"] = "disabled";
                }
                else
                {
                    btnPaisRec.Attributes.Remove("disabled");
                }
                tbCpRec.ReadOnly = controlTemp;
            }
            if (dr.Read())
            {
                if (tipo == "emi")
                {
                    tbCalleEmi.Text = dr["dirMatriz"].ToString();
                    tbNoExtEmi.Text = dr["noExterior"].ToString();
                    tbNoIntEmi.Text = dr["noInterior"].ToString();
                    tbColoniaEmi.Text = dr["colonia"].ToString();
                    tbMunicipioEmi.Text = dr["municipio"].ToString();
                    tbEstadoEmi.Text = dr["estado"].ToString();
                    tbPaisEmi.Text = dr["pais"].ToString();
                    tbCpEmi.Text = dr["codigoPostal"].ToString();
                    tbLugarExp.Text = dr["pais"] + ", " + dr["estado"];
                    _db.Desconectar();
                    if (chkDom)
                    {
                        chkDomEmi.Checked = true;
                        chkDomEmi_CheckedChanged(null, null);
                        if (!string.IsNullOrEmpty(ddlSucEmi.SelectedValue) && !ddlSucEmi.SelectedValue.Equals("0"))
                        {
                            _db.Conectar();
                            _db.CrearComando(@"SELECT
	                                        suc.*
                                        FROM
	                                        Cat_SucursalesEmisor suc
                                         WHERE suc.idSucursal = @idSucursal");
                            _db.AsignarParametroCadena("@idSucursal", ddlSucEmi.SelectedValue);
                        }
                        else
                        {
                            _db.Conectar();
                            _db.CrearComando(@"SELECT
	                                        suc.*
                                        FROM
	                                        Cat_SucursalesEmisor suc INNER JOIN
	                                        Cat_Empleados emp ON suc.idSucursal = emp.id_Sucursal
                                         WHERE suc.RFC = @RFC AND emp.idEmpleado = @idUser");
                            _db.AsignarParametroCadena("@RFC", rfc);
                            _db.AsignarParametroCadena("@idUser", _idUser);
                        }
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            _sucursalEmisor = dr["idSucursal"].ToString();
                            cbEmiExp.Checked = true;
                            cbEmiExp_CheckedChanged(null, null);
                            cbUsarDirecEmisor.Checked = false;
                            cbUsarDirecEmisor_CheckedChanged(null, null);
                            tbCalleExp.Text = dr["calle"].ToString();
                            tbNoExtExp.Text = dr["noExterior"].ToString();
                            tbNoIntExp.Text = dr["noInterior"].ToString();
                            tbColoniaExp.Text = dr["colonia"].ToString();
                            tbMunicipioExp.Text = dr["municipio"].ToString();
                            tbEstadoExp.Text = dr["estado"].ToString();
                            tbPaisExp.Text = dr["pais"].ToString();
                            tbCpExp.Text = dr["codigoPostal"].ToString();
                            tbLugarExp.Text = dr["pais"] + ", " + dr["estado"];
                        }
                        _db.Desconectar();
                    }
                }
                if (tipo == "rec")
                {
                    tbCalleRec.Text = dr["domicilio"].ToString();
                    tbNoExtRec.Text = dr["noExterior"].ToString();
                    tbNoIntRec.Text = dr["noInterior"].ToString();
                    tbColoniaRec.Text = dr["colonia"].ToString();
                    tbMunicipioRec.Text = dr["municipio"].ToString();
                    tbEstadoRec.Text = dr["estado"].ToString();
                    tbPaisRec.Text = dr["pais"].ToString();
                    if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OAL111108K5A"))
                    {
                        tbPaisRec.Text = "MEX";
                    }
                    tbCpRec.Text = dr["codigoPostal"].ToString();
                    _db.Desconectar();
                    _db.Conectar();
                    _db.CrearComando(@"SELECT idSucursal, sucursal FROM Cat_Sucursales WHERE RFC = @RFC");
                    _db.AsignarParametroCadena("@RFC", rfc);
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
            }
            else
            {
                if (tipo == "emi")
                {
                    _db.Desconectar();
                    if (chkDom)
                    {
                        chkDomEmi.Checked = false;
                        chkDomEmi_CheckedChanged(null, null);
                    }
                    tbCalleEmi.Text = "";
                    tbNoExtEmi.Text = "";
                    tbNoIntEmi.Text = "";
                    tbColoniaEmi.Text = "";
                    tbMunicipioEmi.Text = "";
                    tbEstadoEmi.Text = "";
                    tbPaisEmi.Text = "";
                    tbCpEmi.Text = "";
                    tbLugarExp.Text = "";
                }
                if (tipo == "rec")
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
                    tbMunicipioRec.Text = "";
                    tbEstadoRec.Text = "";
                    tbPaisRec.Text = "";
                    tbCpRec.Text = "";
                }
            }
        }

        /// <summary>
        /// Llenarlistadoms the specified identifier receptor.
        /// </summary>
        /// <param name="idReceptor">The identifier receptor.</param>
        /// <param name="rfc">The RFC.</param>
        /// <param name="tipo">The tipo.</param>
        /// <param name="chkDom">if set to <c>true</c> [CHK DOM].</param>
        private void Llenarlistadom(int idReceptor, string rfc, string tipo, bool chkDom = true)
        {
            var sql = "";
            if (tipo == "emi")
            {
                sql = @"SELECT [dirMatriz]
                              ,[noExterior]
                              ,[noInterior]
                              ,[colonia]
                              ,[localidad]
                              ,[referencia]
                              ,[municipio]
                              ,[estado]
                              ,[pais]
                              ,[codigoPostal]
                          FROM [Cat_Emisor]
                          WHERE IDEEMI=@id";
            }
            if (tipo == "rec")
            {
                sql = @"SELECT [domicilio]
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
                          WHERE IDEREC=@id";
            }
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroEntero("@id", idReceptor);
            var dr = _db.EjecutarConsulta();
            var control = (dr.HasRows && !rfc.Equals("XAXX010101000", StringComparison.OrdinalIgnoreCase) && !rfc.Equals("XEXX010101000", StringComparison.OrdinalIgnoreCase)) || !cbDomRec.Checked;
            if (tipo == "emi")
            {
                tbCalleEmi.ReadOnly = control;
                tbNoExtEmi.ReadOnly = control;
                tbNoIntEmi.ReadOnly = control;
                tbColoniaEmi.ReadOnly = control;
                tbMunicipioEmi.ReadOnly = control;
                tbEstadoEmi.ReadOnly = control;
                tbPaisEmi.ReadOnly = control;
                tbCpEmi.ReadOnly = control;
                tbLugarExp.ReadOnly = control;
            }
            else if (tipo == "rec")
            {
                var controlTemp = control;
                if (Session["IDENTEMI"].ToString().Equals("OHR980618BLA"))
                {
                    controlTemp = false;
                }
                tbCalleRec.ReadOnly = controlTemp;
                tbNoExtRec.ReadOnly = controlTemp;
                tbNoIntRec.ReadOnly = controlTemp;
                tbColoniaRec.ReadOnly = controlTemp;
                tbMunicipioRec.ReadOnly = controlTemp;
                tbEstadoRec.ReadOnly = controlTemp;
                tbPaisRec.ReadOnly = controlTemp;
                if (controlTemp)
                {
                    btnPaisRec.Attributes["disabled"] = "disabled";
                }
                else
                {
                    btnPaisRec.Attributes.Remove("disabled");
                }
                tbCpRec.ReadOnly = controlTemp;
            }
            if (dr.Read())
            {
                if (tipo == "emi")
                {
                    tbCalleEmi.Text = dr["dirMatriz"].ToString();
                    tbNoExtEmi.Text = dr["noExterior"].ToString();
                    tbNoIntEmi.Text = dr["noInterior"].ToString();
                    tbColoniaEmi.Text = dr["colonia"].ToString();
                    tbMunicipioEmi.Text = dr["municipio"].ToString();
                    tbEstadoEmi.Text = dr["estado"].ToString();
                    tbPaisEmi.Text = dr["pais"].ToString();
                    tbCpEmi.Text = dr["codigoPostal"].ToString();
                    tbLugarExp.Text = dr["pais"] + ", " + dr["estado"];
                    _db.Desconectar();
                    if (chkDom)
                    {
                        chkDomEmi.Checked = true;
                        chkDomEmi_CheckedChanged(null, null);
                        _db.Conectar();
                        _db.CrearComando(@"SELECT
	                                        suc.*
                                        FROM
	                                        Cat_SucursalesEmisor suc INNER JOIN
	                                        Cat_Empleados emp ON suc.idSucursal = emp.id_Sucursal
                                         WHERE suc.RFC = @RFC AND emp.idEmpleado = @idUser");
                        _db.AsignarParametroCadena("@RFC", rfc);
                        _db.AsignarParametroCadena("@idUser", _idUser);
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            _sucursalEmisor = dr["idSucursal"].ToString();
                            cbEmiExp.Checked = true;
                            cbEmiExp_CheckedChanged(null, null);
                            cbUsarDirecEmisor.Checked = false;
                            cbUsarDirecEmisor_CheckedChanged(null, null);
                            tbCalleExp.Text = dr["calle"].ToString();
                            tbNoExtExp.Text = dr["noExterior"].ToString();
                            tbNoIntExp.Text = dr["noInterior"].ToString();
                            tbColoniaExp.Text = dr["colonia"].ToString();
                            tbMunicipioExp.Text = dr["municipio"].ToString();
                            tbEstadoExp.Text = dr["estado"].ToString();
                            tbPaisExp.Text = dr["pais"].ToString();
                            tbCpExp.Text = dr["codigoPostal"].ToString();
                            tbLugarExp.Text = dr["pais"] + ", " + dr["estado"];
                        }
                        _db.Desconectar();
                    }
                }
                if (tipo == "rec")
                {
                    tbCalleRec.Text = dr["domicilio"].ToString();
                    tbNoExtRec.Text = dr["noExterior"].ToString();
                    tbNoIntRec.Text = dr["noInterior"].ToString();
                    tbColoniaRec.Text = dr["colonia"].ToString();
                    tbMunicipioRec.Text = dr["municipio"].ToString();
                    tbEstadoRec.Text = dr["estado"].ToString();
                    tbPaisRec.Text = dr["pais"].ToString();
                    if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OAL111108K5A"))
                    {
                        tbPaisRec.Text = "MEX";
                    }
                    tbCpRec.Text = dr["codigoPostal"].ToString();
                    _db.Desconectar();
                    _db.Conectar();
                    _db.CrearComando(@"SELECT idSucursal, sucursal FROM Cat_Sucursales WHERE RFC = @RFC");
                    _db.AsignarParametroCadena("@RFC", rfc);
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
            }
            else
            {
                if (tipo == "emi")
                {
                    _db.Desconectar();
                    if (chkDom)
                    {
                        chkDomEmi.Checked = false;
                        chkDomEmi_CheckedChanged(null, null);
                    }
                    tbCalleEmi.Text = "";
                    tbNoExtEmi.Text = "";
                    tbNoIntEmi.Text = "";
                    tbColoniaEmi.Text = "";
                    tbMunicipioEmi.Text = "";
                    tbEstadoEmi.Text = "";
                    tbPaisEmi.Text = "";
                    tbCpEmi.Text = "";
                    tbLugarExp.Text = "";
                }
                if (tipo == "rec")
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
                    tbMunicipioRec.Text = "";
                    tbEstadoRec.Text = "";
                    tbPaisRec.Text = "";
                    tbCpRec.Text = "";
                }
            }
        }

        /// <summary>
        /// Llenarlistas the specified RFC.
        /// </summary>
        /// <param name="idReceptor">The Receptor Id.</param>
        /// <param name="tipo">The tipo.</param>
        private void Llenarlista(int idReceptor, string tipo)
        {
            var sql = "";
            var rfc = "";
            if (tipo == "emi")
            {
                sql = @"SELECT [NOMEMI]
                              ,[RFCEMI] AS rfc
                              ,[obligadoContabilidad]
                              ,[curp]
                              ,[telefono]
                              ,[email]
                              ,[regimenFiscal]
                              ,[EmpresaTipo]
                          FROM [Cat_Emisor]
                          WHERE IDEEMI=@ruc";
            }
            if (tipo == "rec")
            {
                sql = @"SELECT IDEREC
                              ,[RFCREC] AS rfc
                              ,[NOMREC]
                              ,[telefono]
                              ,[contribuyenteEspecial]
                              ,[obligadoContabilidad]
                              ,[tipoIdentificacionComprador]
                              ,[email]
                              ,[curp]
                              ,ISNULL(CONVERT(VARCHAR, metodoPago), '') AS metodoPago
                              ,[numCtaPago]
                              ,[denominacionSocial]
                              ,[telefono2]
                          FROM [Cat_Receptor] LEFT OUTER JOIN Cat_Catalogo1_C cc ON CONVERT(VARCHAR, metodoPago) = cc.codigo AND cc.tipo = 'MetodoPago'
                          WHERE IDEREC=@ruc";
            }
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroEntero("@ruc", idReceptor);
            var dr = _db.EjecutarConsulta();
            if (tipo == "emi")
            {
                tbRfcEmi.ReadOnly = dr.HasRows;
                tbNomEmi.ReadOnly = dr.HasRows;
                tbCURPE.ReadOnly = dr.HasRows;
                tbRegimenFiscal.ReadOnly = dr.HasRows;
                //ddlTEmp.Enabled = !dr.HasRows;
            }
            else if (tipo == "rec")
            {
                var control = dr.HasRows;
                tbRazonSocialRec.ReadOnly = control;
                tbCURPR.ReadOnly = control;
                //chkMetodoPago.Enabled = !dr.HasRows;
                tbDenomSocialRec.ReadOnly = control;
            }
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    rfc = dr["rfc"].ToString();
                    if (tipo == "emi")
                    {
                        tbRfcEmi.Text = rfc;
                        tbNomEmi.Text = dr["NOMEMI"].ToString();
                        tbCURPE.Text = dr["curp"].ToString();
                        tbMailEmi.Text = dr["email"].ToString();
                        tbTelEmi.Text = dr["telefono"].ToString();
                        tbRegimenFiscal.Text = dr["regimenFiscal"].ToString();
                        //ddlTEmp.SelectedValue = dr["EmpresaTipo"].ToString();
                    }
                    if (tipo == "rec")
                    {
                        var control = !rfc.Equals("XAXX010101000", StringComparison.OrdinalIgnoreCase) && !rfc.Equals("XEXX010101000", StringComparison.OrdinalIgnoreCase);
                        tbRazonSocialRec.ReadOnly = control;
                        tbCURPR.ReadOnly = control;
                        //chkMetodoPago.Enabled = !dr.HasRows;
                        tbDenomSocialRec.ReadOnly = control;
                        _idReceptor = dr["IDEREC"].ToString();
                        tbRfcRec.Text = rfc;
                        tbRazonSocialRec.Text = dr["NOMREC"].ToString();
                        tbCURPR.Text = dr["curp"].ToString();
                        tbMailRec.Text = dr["email"].ToString();
                        tbTelRec.Text = dr["telefono"].ToString();
                        var metodosPago = dr["metodoPago"].ToString().Split(',');
                        chkMetodoPago.ClearSelection();
                        ddlMetodoPago.ClearSelection();
                        foreach (var pago in metodosPago)
                        {
                            if (Session["CfdiVersion"].ToString().Equals("3.3"))
                            {
                                try { ddlMetodoPago.Items.FindByValue(pago).Selected = true; } catch { }
                            }
                            else
                            {
                                try { chkMetodoPago.Items.FindByValue(pago).Selected = true; } catch { }
                            }
                        }
                        tbNumCtaPago.Text = string.IsNullOrEmpty(dr["numCtaPago"].ToString()) ? "NO IDENTIFICADO" : dr["numCtaPago"].ToString();
                        tbDenomSocialRec.Text = dr["denominacionSocial"].ToString();
                        tbTelRec2.Text = dr["telefono2"].ToString();
                    }
                }
            }
            else
            {
                if (tipo == "emi")
                {
                    tbNomEmi.Text = "";
                    tbCURPE.Text = "";
                    tbMailEmi.Text = "";
                    tbTelEmi.Text = "";
                    tbRegimenFiscal.Text = "";
                    //ddlTEmp.Text = "";
                }
                if (tipo == "rec")
                {
                    _idReceptor = "";
                    tbRazonSocialRec.Text = "";
                    tbCURPR.Text = "";
                    tbMailRec.Text = "";
                    tbTelRec.Text = "";
                    chkMetodoPago.ClearSelection();
                    ddlMetodoPago.ClearSelection();
                    tbNumCtaPago.Text = "NO IDENTIFICADO";
                    tbDenomSocialRec.Text = "";
                    tbTelRec2.Text = "";
                }
            }
            _db.Desconectar();
            Llenarlistadom(idReceptor, rfc, tipo);
            if (tipo.Equals("rec"))
            {
                var tipoPersona = "%" + (rfc.Length == 12 ? "F" : (rfc.Length == 12 ? "M" : "FM")) + "%";
                SqlDataSourceUsoCfdi.SelectParameters["tipoPersonaUsoCfdi"].DefaultValue = tipoPersona;
                ddlUsoCfdi.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "_hideUsarRfc", "$('#ModuloBC').modal('hide');", true);
        }

        /// <summary>
        /// Llenarlistas the specified RFC.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <param name="tipo">The tipo.</param>
        private void Llenarlista(string rfc, string tipo)
        {
            var sql = "";
            if (tipo == "emi")
            {
                sql = @"SELECT [NOMEMI]
                              ,[obligadoContabilidad]
                              ,[curp]
                              ,[telefono]
                              ,[email]
                              ,[regimenFiscal]
                              ,[EmpresaTipo]
                          FROM [Cat_Emisor]
                          WHERE RFCEMI=@ruc";
            }
            if (tipo == "rec")
            {
                sql = @"SELECT IDEREC
                              ,[NOMREC]
                              ,[telefono]
                              ,[contribuyenteEspecial]
                              ,[obligadoContabilidad]
                              ,[tipoIdentificacionComprador]
                              ,[email]
                              ,[curp]
                              ,ISNULL(CONVERT(VARCHAR, metodoPago), '') AS metodoPago
                              ,[numCtaPago]
                              ,[denominacionSocial]
                              ,[telefono2]
                          FROM [Cat_Receptor] LEFT OUTER JOIN Cat_Catalogo1_C cc ON CONVERT(VARCHAR, metodoPago) = cc.codigo AND cc.tipo = 'MetodoPago'
                          WHERE RFCREC=@ruc";
            }
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@ruc", rfc);
            var dr = _db.EjecutarConsulta();
            if (tipo == "emi")
            {
                tbRfcEmi.ReadOnly = dr.HasRows;
                tbNomEmi.ReadOnly = dr.HasRows;
                tbCURPE.ReadOnly = dr.HasRows;
                tbRegimenFiscal.ReadOnly = dr.HasRows;
                //ddlTEmp.Enabled = !dr.HasRows;
            }
            else if (tipo == "rec")
            {
                var control = dr.HasRows && !rfc.Equals("XAXX010101000", StringComparison.OrdinalIgnoreCase) && !rfc.Equals("XEXX010101000", StringComparison.OrdinalIgnoreCase);
                tbRazonSocialRec.ReadOnly = control;
                tbCURPR.ReadOnly = control;
                //chkMetodoPago.Enabled = !dr.HasRows;
                tbDenomSocialRec.ReadOnly = control;
            }
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (tipo == "emi")
                    {
                        tbRfcEmi.Text = rfc;
                        tbNomEmi.Text = dr["NOMEMI"].ToString();
                        tbCURPE.Text = dr["curp"].ToString();
                        tbMailEmi.Text = dr["email"].ToString();
                        tbTelEmi.Text = dr["telefono"].ToString();
                        tbRegimenFiscal.Text = dr["regimenFiscal"].ToString();
                        //ddlTEmp.SelectedValue = dr["EmpresaTipo"].ToString();
                    }
                    if (tipo == "rec")
                    {
                        _idReceptor = dr["IDEREC"].ToString();
                        tbRfcRec.Text = rfc;
                        tbRazonSocialRec.Text = dr["NOMREC"].ToString();
                        tbCURPR.Text = dr["curp"].ToString();
                        tbMailRec.Text = dr["email"].ToString();
                        tbTelRec.Text = dr["telefono"].ToString();
                        var metodosPago = dr["metodoPago"].ToString().Split(',');
                        chkMetodoPago.ClearSelection();
                        ddlMetodoPago.ClearSelection();
                        foreach (var pago in metodosPago)
                        {
                            if (Session["CfdiVersion"].ToString().Equals("3.3"))
                            {
                                try { ddlMetodoPago.Items.FindByValue(pago).Selected = true; } catch { }
                            }
                            else
                            {
                                try { chkMetodoPago.Items.FindByValue(pago).Selected = true; } catch { }
                            }
                        }
                        tbNumCtaPago.Text = string.IsNullOrEmpty(dr["numCtaPago"].ToString()) ? "NO IDENTIFICADO" : dr["numCtaPago"].ToString();
                        tbDenomSocialRec.Text = dr["denominacionSocial"].ToString();
                        tbTelRec2.Text = dr["telefono2"].ToString();
                    }
                }
            }
            else
            {
                if (tipo == "emi")
                {
                    tbNomEmi.Text = "";
                    tbCURPE.Text = "";
                    tbMailEmi.Text = "";
                    tbTelEmi.Text = "";
                    tbRegimenFiscal.Text = "";
                    //ddlTEmp.Text = "";
                }
                if (tipo == "rec")
                {
                    _idReceptor = "";
                    tbRazonSocialRec.Text = "";
                    tbCURPR.Text = "";
                    tbMailRec.Text = "";
                    tbTelRec.Text = "";
                    chkMetodoPago.ClearSelection();
                    ddlMetodoPago.ClearSelection();
                    tbNumCtaPago.Text = "NO IDENTIFICADO";
                    tbDenomSocialRec.Text = "";
                    tbTelRec2.Text = "";
                }
            }
            _db.Desconectar();
            Llenarlistadom(rfc, tipo);
            if (tipo.Equals("rec"))
            {
                var tipoPersona = "%" + (rfc.Length == 12 ? "F" : (rfc.Length == 12 ? "M" : "FM")) + "%";
                SqlDataSourceUsoCfdi.SelectParameters["tipoPersonaUsoCfdi"].DefaultValue = tipoPersona;
                ddlUsoCfdi.DataBind();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbDomRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void cbDomRec_CheckedChanged(object sender, EventArgs e)
        {
            //RequiredFieldValidator14.Enabled = cbDomRec.Checked;
            //RequiredFieldValidator15.Enabled = cbDomRec.Checked;
            //RequiredFieldValidator16.Enabled = cbDomRec.Checked;
            RequiredFieldValidator23.Enabled = cbDomRec.Checked;
            //RequiredFieldValidator27.Enabled = cbDomRec.Checked;
            ddlSucRec.Items.Clear();
            var itemNull = new ListItem("SELECCIONE", "0");
            itemNull.Selected = true;
            ddlSucRec.Items.Add(itemNull);
            //Llenarlistadom(cbDomRec.Checked ? tbRfcRec.Text : "", "rec", false);
            var id = 0;
            int.TryParse(_idReceptor, out id);
            Llenarlistadom(cbDomRec.Checked ? id : 0, cbDomRec.Checked ? tbRfcRec.Text : "", "rec", false);
            ddlSucRec.Enabled = ddlSucRec.Items.Count > 1;
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRfcRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRfcRec_TextChanged(object sender, EventArgs e)
        {
            Llenarlista(tbRfcRec.Text, "rec");
            LlenaContactosInicio();
            if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"SET920324FC3|LAN7008173R5|YAM8302185A8"))
            {
                divAddendaCts.Style["display"] = Regex.IsMatch(tbRfcRec.Text, @"CTS021101KU8|CTS960918BJ4|CTS160922QLA|XAXX010101000") ? "inline" : "none";
            }
            else if (Session["IDENTEMI"].ToString().Equals("SGO050614JG0", StringComparison.OrdinalIgnoreCase))
            {
                divAddendaCts.Style["display"] = "inline";
            }
            else
            {
                divAddendaCts.Style["display"] = "none";
            }
        }

        /// <summary>
        /// Handles the Click event of the bAgregarImpuestoLocal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarImpuestoLocal_Click(object sender, EventArgs e)
        {
            _db.Conectar();
            _db.CrearComando(@"insert into Dat_MX_ImpLocalesTemp
                           (tipo,nombre,tasa,id_Empleado,SessionId)
                           values
                           (@tipo,@nombre,@tasa,@id_Empleado,@SessionId)");
            var tipo = "";
            switch (ddlTipoImpuestoLocal.SelectedValue)
            {
                case "01":
                    tipo = "2";
                    break;
                case "02":
                    tipo = "1";
                    break;
                default:
                    break;
            }
            _db.AsignarParametroCadena("@tipo", tipo);
            _db.AsignarParametroCadena("@nombre", tbImpLocal.Text);
            _db.AsignarParametroCadena("@tasa", tbTasaLocal.Text);
            _db.AsignarParametroCadena("@id_Empleado", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.EjecutarConsulta1();
            _db.Desconectar();
            tbImpLocal.Text = "";
            tbTasaLocal.Text = "";
            CalculaImpuestosLocales();
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRuc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRuc_TextChanged(object sender, EventArgs e)
        {
            Llenarlista(tbRfcEmi.Text, "emi");
        }

        /// <summary>
        /// Handles the Click event of the bAgregarContacto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarContacto_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(tbNomContRec.Text) && !string.IsNullOrEmpty(tbMailRec.Text))
            {
                _db.Conectar();
                _db.CrearComando(@"INSERT INTO Cat_Mx_Contactos_Temp (id_Empleado, nombre, puesto, telefono1, telefono2, correo, SessionId) VALUES (@idUser, @nombre, @puesto, @tel1, @tel2, @correo, @SessionId)");
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@nombre", tbNomContRec.Text);
                _db.AsignarParametroCadena("@puesto", tbPuestoContRec.Text);
                _db.AsignarParametroCadena("@tel1", tbTelContRec1.Text);
                _db.AsignarParametroCadena("@tel2", tbTelContRec2.Text);
                _db.AsignarParametroCadena("@correo", tbMailContRec.Text);
                _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                _db.EjecutarConsulta1();
                _db.Desconectar();
                SqlDataContactos.DataBind();
                gvContactos.DataBind();
                tbNomContRec.Text = "";
                tbPuestoContRec.Text = "";
                tbTelContRec1.Text = "";
                tbTelContRec2.Text = "";
                tbMailContRec.Text = "";
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El campo " + (string.IsNullOrEmpty(tbNomContRec.Text) ? "nombre" : (string.IsNullOrEmpty(tbMailRec.Text) ? "correo" : "")) + " no puede quedar vacio", 4, null);
            }
        }

        //protected void ddlDescConc_SelectedIndexChanged(object sender, EventArgs e)
        /// <summary>
        /// Handles the TextChanged event of the tbDescConc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbDescConc_TextChanged(object sender, EventArgs e)
        {
            var isHotel = false;
            if (Session["IDGIRO"].ToString().Contains("1"))
            {
                isHotel = true;
                //isHotel = Regex.IsMatch(ddlCatConc.SelectedItem.Text, @".*HOTEL.*", RegexOptions.IgnoreCase);
            }
            var nombre = tbDescConc.Text;
            _db.Conectar();
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                _db.CrearComando(@"SELECT valorUnitario as vu, codigo as noId, unidadMedida FROM Cat_CatConceptos_C WHERE descripcion = @nombre AND idCategoria = @idCategoria AND idConcepto = @idConcepto");
                _db.AsignarParametroCadena("@idConcepto", ddlDesCon.SelectedValue);
            }
            else
            {
                _db.CrearComando(@"SELECT valorUnitario as vu, codigo as noId, unidadMedida FROM Cat_CatConceptos_C WHERE descripcion = @nombre AND idCategoria = @idCategoria");
            }
            _db.AsignarParametroCadena("@nombre", nombre);
            _db.AsignarParametroCadena("@idCategoria", ddlCatConc.SelectedValue);
            tbVUConc.Text = "";
            tbIdentConc.Text = "";
            tbUnidadConc.Text = "";
            tbCantidadConc.Text = "";
            tbImpConc.Text = "";
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                if (!string.IsNullOrEmpty(dr["vu"].ToString()))
                {
                    decimal vu = 0;
                    decimal.TryParse(dr["vu"].ToString(), out vu);
                    tbVUConc.Text = CerosNull(vu.ToString());
                }
                tbIdentConc.Text = dr["noId"].ToString();
                if (string.IsNullOrEmpty(tbIdentConc.Text)) { tbIdentConc.Text = "NO APLICA"; }
                tbUnidadConc.Text = dr["unidadMedida"].ToString();
                if (string.IsNullOrEmpty(tbUnidadConc.Text) && !Session["CfdiVersion"].ToString().Equals("3.3")) { tbUnidadConc.Text = "NO APLICA"; }
                tbIdentConc.ReadOnly = false;
                tbUnidadConc.ReadOnly = Session["CfdiVersion"].ToString().Equals("3.3");
            }
            else
            {
                tbIdentConc.Text = "";
                tbUnidadConc.Text = "";
                tbIdentConc.ReadOnly = false;
                tbUnidadConc.ReadOnly = false;
            }
            _db.Desconectar();
            chkHabilitarIsh.Visible = isHotel;
            chkHabilitarIsh.Enabled = isHotel/* && (Regex.IsMatch(tbDescConc.Text, @".*(paquete)|(otro.).*", RegexOptions.IgnoreCase))*/;
            chkHabilitarIsh.Checked = isHotel && (Regex.IsMatch(tbDescConc.Text, @".*(habitaci.n)|(hosped).*", RegexOptions.IgnoreCase));

            if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HCQ100621378|LAN7008173R5"))
            {
                chkHabilitarIUI.Visible = isHotel;
                chkHabilitarIUI.Enabled = isHotel/* && (Regex.IsMatch(tbDescConc.Text, @".*(paquete)|(otro.).*", RegexOptions.IgnoreCase))*/;
                chkHabilitarIUI.Checked = isHotel && (Regex.IsMatch(tbDescConc.Text, @".*(habitaci.n)|(hosped).*", RegexOptions.IgnoreCase));
            }

            if (isHotel && chkHabilitarIsh.Enabled && (Session["IDENTEMI"].ToString().Equals("OHR980618BLA", StringComparison.OrdinalIgnoreCase)))
            {
                rowISHEdit.Visible = true;
            }
            else
            {
                rowISHEdit.Visible = false;
            }
            rowVerDatosSat.Style["display"] = Session["CfdiVersion"].ToString().Equals("3.3") ? "block" : "none";
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                _db.Conectar();
                _db.CrearComando("SELECT TOP 1 claveProdServ FROM Cat_CatConceptos_C WHERE descripcion = @descripcion AND claveProdServ IS NOT NULL AND idConcepto = @idConcepto");
                _db.AsignarParametroCadena("@idConcepto", ddlDesCon.SelectedValue);
                _db.AsignarParametroCadena("@descripcion", tbDescConc.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    lblCveProdServ.Text = $"Clave Producto/Servicio: {dr[0].ToString()}";
                }
                else
                {
                    lblCveProdServ.Text = $"Clave Producto/Servicio: N/A";
                }
                _db.Desconectar();
                _db.Conectar();
                _db.CrearComando("SELECT TOP 1 u.claveSat FROM Cat_CatConceptos_C c INNER JOIN Cat_CveUnidad u ON c.unidadMedida COLLATE DATABASE_DEFAULT = u.descripcion WHERE c.descripcion = @descripcion AND idConcepto = @idConcepto");
                _db.AsignarParametroCadena("@idConcepto", ddlDesCon.SelectedValue);
                _db.AsignarParametroCadena("@descripcion", tbDescConc.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    lblCveUnidad.Text = $"Clave Unidad: {dr[0].ToString()}";
                }
                else
                {
                    lblCveUnidad.Text = $"Clave Unidad: N/A";
                }
                _db.Desconectar();
            }
            CalculaImporteConcepto(null, null);
            if (!Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                SelectConceptosFromCat(true);
            }
        }

        private void SelectConceptosFromCat(bool mantener = false)
        {
            if (!mantener)
            {
                tbDescConc.Text = "";
                tbVUConc.Text = "";
                tbUnidadConc.Text = "";
                tbIdentConc.Text = "";
                tbDescConc.ReadOnly = false;
                tbVUConc.ReadOnly = false;
                tbUnidadConc.ReadOnly = false;
                tbIdentConc.ReadOnly = false;
                ddlDesCon.Enabled = true;
            }
            SqlDataDescConc.SelectParameters["idCat"].DefaultValue = ddlCatConc.SelectedValue;
            RequiredFieldValidator22.Enabled = ddlCatConc.SelectedValue.Equals("0");
            if (!ddlCatConc.SelectedValue.Equals("0"))
            {
                bindConceptos(mantener);
            }
            else
            {
                tbDescConc.ReadOnly = true;
                tbVUConc.ReadOnly = true;
                tbUnidadConc.ReadOnly = true;
                tbIdentConc.ReadOnly = true;
                ddlDesCon.Enabled = false;
                CalculaImporteConcepto(null, null);
            }
            chkEditarItem33.Visible = false;
            rowVerDatosSat.Style["display"] = "none";
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlCatConc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlCatConc_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectConceptosFromCat();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSucRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
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
                    if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OAL111108K5A"))
                    {
                        tbPaisRec.Text = "MEX";
                    }
                    tbCpRec.Text = dr["codigoPostal"].ToString();
                }
                _db.Desconectar();
            }
            else
            {
                Llenarlistadom(tbRfcRec.Text, "rec");
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSerie control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sql = "SELECT c.codigo, c.descripcion AS ambiente FROM Cat_Catalogo1_C c INNER JOIN Cat_Series s ON s.ambiente = c.codigo WHERE c.tipo = 'Ambiente' AND s.idSerie = @idSerie";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idSerie", ddlSerie.SelectedValue);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                hfambiente.Value = dr["codigo"].ToString();
                tbAmbiente.Text = dr["ambiente"].ToString();
            }
            _db.Desconectar();
        }

        #region GridViews

        /// <summary>
        /// Handles the PageIndexChanging event of the gvRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRec.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvImpuestos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvImpuestos_PageIndexChanged(object sender, EventArgs e)
        {
            gvImpuestos.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvImpuestos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvImpuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvImpuestos.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvImpuestos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvImpuestos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvImpuestos.PageIndex = e.NewPageIndex;
            gvImpuestos.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvImpuestos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvImpuestos_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvImpuestos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the RowDeleted event of the gvImpuestos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeletedEventArgs" /> instance containing the event data.</param>
        protected void gvImpuestos_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            CalculaImpuestos();
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvConceptos_PageIndexChanged(object sender, EventArgs e)
        {
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvConceptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvConceptos.PageIndex = e.NewPageIndex;
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvConceptos_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvConceptos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvPartes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvPartes_PageIndexChanged(object sender, EventArgs e)
        {
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvPartes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvPartes_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvPartes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvPartes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPartes.PageIndex = e.NewPageIndex;
            gvPartes.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvPartes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvPartes_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvPartes.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvAduanaPartesTemp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvAduanaPartesTemp_PageIndexChanged(object sender, EventArgs e)
        {
            gvAduanaPartesTemp.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvAduanaPartesTemp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvAduanaPartesTemp_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvAduanaPartesTemp.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvAduanaPartesTemp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvAduanaPartesTemp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAduanaPartesTemp.PageIndex = e.NewPageIndex;
            gvAduanaPartesTemp.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvAduanaPartesTemp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvAduanaPartesTemp_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvAduanaPartesTemp.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvAduanasConcepto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvAduanasConcepto_PageIndexChanged(object sender, EventArgs e)
        {
            gvAduanasConcepto.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvAduanasConcepto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvAduanasConcepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvAduanasConcepto.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvAduanasConcepto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvAduanasConcepto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAduanasConcepto.PageIndex = e.NewPageIndex;
            gvAduanasConcepto.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvAduanasConcepto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvAduanasConcepto_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvAduanasConcepto.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the RowDeleted event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeletedEventArgs" /> instance containing the event data.</param>
        protected void gvConceptos_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            CalcularTotales();
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvImpuestosLocales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvImpuestosLocales_PageIndexChanged(object sender, EventArgs e)
        {
            gvImpuestosLocales.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvImpuestosLocales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvImpuestosLocales_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvImpuestosLocales.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvImpuestosLocales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvImpuestosLocales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvImpuestosLocales.PageIndex = e.NewPageIndex;
            gvImpuestosLocales.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvImpuestosLocales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvImpuestosLocales_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvImpuestosLocales.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the RowDeleted event of the gvImpuestosLocales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeletedEventArgs" /> instance containing the event data.</param>
        protected void gvImpuestosLocales_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            CalcularTotales();
        }

        /// <summary>
        /// Handles the RowDeleted event of the gvAduanasConcepto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeletedEventArgs" /> instance containing the event data.</param>
        protected void gvAduanasConcepto_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            tbNumeroPredial.Text = "";
            tbNumeroPredial.ReadOnly = gvPartes.Rows.Count + gvAduanasConcepto.Rows.Count > 1;
        }

        /// <summary>
        /// Handles the RowDeleted event of the gvPartes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeletedEventArgs" /> instance containing the event data.</param>
        protected void gvPartes_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            tbNumeroPredial.Text = "";
            tbNumeroPredial.ReadOnly = gvPartes.Rows.Count + gvAduanasConcepto.Rows.Count > 1;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvContactos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvContactos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvContactos.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvContactos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvContactos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvContactos.PageIndex = e.NewPageIndex;
            gvContactos.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvContactos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvContactos_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvContactos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvContactos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvContactos_PageIndexChanged(object sender, EventArgs e)
        {
            gvContactos.DataBind();
        }

        #endregion

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlMetodoPago control.
        /// </summary>
        //protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    tbNumCtaPago_MaskedEditExtender.Enabled = ddlMetodoPago.SelectedValue.Equals("03") || ddlMetodoPago.SelectedValue.Equals("04") || ddlMetodoPago.SelectedValue.Equals("28");
        //    tbNumCtaPago.ReadOnly = ddlMetodoPago.SelectedValue.Equals("99") || ddlMetodoPago.SelectedValue.Equals("01") || ddlMetodoPago.SelectedValue.Equals("NA");
        //    tbNumCtaPago.Text = tbNumCtaPago_MaskedEditExtender.Enabled ? "" : "NO IDENTIFICADO";
        //}

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
        /// Handles the SelectedIndexChanged event of the ddlMoneda control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, ddlMoneda.SelectedValue);
            switch (ddlMoneda.SelectedValue)
            {
                case "MXN": tbTipoCambio.Text = TC_peso; break;
                case "USD": tbTipoCambio.Text = TC_dolarUsLiquidacion; break;
                case "EUR": tbTipoCambio.Text = TC_euro; break;
                case "LSD": tbTipoCambio.Text = TC_libra; break;
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
            if (chkHabilitarINE.Checked)
            {
                divValidationIne.Attributes.Add("habilitado", "");
            }
            else
            {
                divValidationIne.Attributes.Remove("habilitado");
            }
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
            else if (DDTipoComite.SelectedValue == "Directivo Estatal" && DDTipo_Proceso.SelectedValue == "Ordinario")
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

        /// <summary>
        /// Builds the text.
        /// </summary>
        /// <returns>System.String.</returns>
        private string BuildTxt()
        {
            if (rowFechaEmision.Visible && !string.IsNullOrEmpty(tbFechaEmision.Text))
            {
                var ahora = Localization.Now;
                var fechaExt = ahora;
                Localization.TryParse(tbFechaEmision.Text, out fechaExt);
                var diferenciaHoras = (fechaExt - ahora).TotalHours;
                var diferenciaMinutos = (fechaExt - ahora).TotalMinutes;
                if (diferenciaMinutos > 5)
                {
                    throw new Exception($"La fecha y hora de expedición puede estar hasta 5 minutos en el futuro al momento de su envío. La diferencia con la fecha definida es de {diferenciaMinutos} minutos");
                }
                else if (diferenciaHoras < -72)
                {
                    throw new Exception($"La fecha y hora de expedición solo puede estar hasta 72 horas en el pasado. La diferencia con la fecha definida es de {diferenciaHoras} horas");
                }
            }
            var selected = new List<string>();
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                selected.Add(ddlMetodoPago.SelectedValue);
            }
            else
            {
                selected = chkMetodoPago.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();
            }
            var mensajeHabitacionEvento = "";
            if (Session["IDGIRO"].ToString().Equals("1") && !ValidarHabitacionEvento(out mensajeHabitacionEvento))
            {
                throw new Exception(mensajeHabitacionEvento);
            }
            else if (selected.Count <= 0)
            {
                throw new Exception("Debe escoger al menos un método de pago");
                //(Master as SiteMaster).MostrarAlerta(this, "Debe escoger al menos un método de pago", 4, null);
                //return null;
            }
            else if (string.IsNullOrEmpty(tbNumCtaPago.Text))
            {
                throw new Exception("Debe definir al menos un número de cuenta separados por coma (,). Si el método de pago es deferente a tarjeta, éste deberá ser NO IDENTIFICADO");
                //(Master as SiteMaster).MostrarAlerta(this, "Debe definir al menos un número de cuenta separados por coma (,). Si el método de pago es deferente a tarjeta, éste deberá ser NO IDENTIFICADO", 4, null);
                //return null;
            }
            else if (ExisteTicket())
            {
                throw new Exception("El ticket " + tbReferenciaRestaurante.Text + " ya se encuentra facturado");
                //(Master as SiteMaster).MostrarAlerta(this, "El ticket " + tbReferenciaRestaurante.Text + " ya se encuentra facturado", 4, null);
                //return null;
            }
            else if (ExisteReservacion())
            {
                var folio = !string.IsNullOrEmpty(tbReservacionEvento.Text) ? tbReservacionEvento.Text : tbReservacionHabitacion.Text;
                throw new Exception("El no. de reserva " + folio + " ya se encuentra facturado");
                //(Master as SiteMaster).MostrarAlerta(this, "El no. de reserva " + folio + " ya se encuentra facturado", 4, null);
                //return null;
            }
            DbDataReader dr;
            var conceptos = new List<object[]>();
            var txt = new SpoolMx();
            decimal iva16 = 0;
            decimal ish = 0;
            decimal iui = 0;
            txt.SetEmisorCfdi(tbNomEmi.Text, tbRfcEmi.Text, tbCURPE.Text, tbTelEmi.Text, tbMailEmi.Text, Session["IDGIRO"].ToString(), tbRegimenFiscal.Text);
            if (chkDomEmi.Checked)
            {
                txt.SetEmisorDomCfdi(tbCalleEmi.Text, tbNoExtEmi.Text, tbNoIntEmi.Text, tbColoniaEmi.Text, "", "", tbMunicipioEmi.Text, tbEstadoEmi.Text, tbPaisEmi.Text, tbCpEmi.Text);
            }
            if (cbEmiExp.Checked)
            {
                var idSucursal = Session["idSucursal"].ToString();
                if (!string.IsNullOrEmpty(ddlSucEmi.SelectedValue) && !ddlSucEmi.SelectedValue.Equals("0"))
                {
                    idSucursal = ddlSucEmi.SelectedValue;
                }
                txt.SetEmisorExpCfdi(tbCalleExp.Text, tbNoExtExp.Text, tbNoIntExp.Text, tbColoniaExp.Text, "", "", tbMunicipioExp.Text, tbEstadoExp.Text, tbPaisExp.Text, tbCpExp.Text, idSucursal);
            }
            txt.SetReceptorCfdi(tbRazonSocialRec.Text, tbRfcRec.Text, tbCURPR.Text, tbTelRec.Text, tbMailRec.Text, tbTelRec2.Text, tbDenomSocialRec.Text, "", "", ddlUsoCfdi.SelectedValue);
            if (cbDomRec.Checked)
            {
                var isSucursal = !ddlSucRec.SelectedValue.Equals("0");
                txt.SetReceptorDomCfdi(tbCalleRec.Text, tbNoExtRec.Text, tbNoIntRec.Text, tbColoniaRec.Text, "", "", tbMunicipioRec.Text, tbEstadoRec.Text, tbPaisRec.Text, tbCpRec.Text, isSucursal);
            }
            var sql = "SELECT nombre, puesto, telefono1, telefono2, correo FROM Cat_Mx_Contactos_Temp WHERE (id_Empleado = @idUser AND SessionId = @SessionId)";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idUser", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                txt.AgregaContatoReceptorCfdi(dr["nombre"].ToString(), dr["puesto"].ToString(), dr["telefono1"].ToString(), dr["telefono2"].ToString(), dr["correo"].ToString());
            }
            _db.Desconectar();
            sql = "SELECT idDetallesTemp, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial, descuento as iva16, ish, claveProdServ, claveUnidad, descuentoCfdi33, precioRet, otherIMP AS iui FROM Dat_DetallesTemp WHERE (id_Empleado = @idUser AND SessionId = @SessionId)";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idUser", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                var sqlResults = new object[dr.FieldCount];
                dr.GetValues(sqlResults);
                conceptos.Add(sqlResults);
            }
            _db.Desconectar();
            var dtImportesTercerosTemp = (DataTable)Session["_dtImportesTercerosTemp"];
            foreach (var concepto in conceptos)
            {
                txt.AgregaConceptoCfdi(concepto[4].ToString(), concepto[6].ToString(), concepto[1].ToString(), concepto[2].ToString(), concepto[3].ToString(), concepto[5].ToString(), concepto[12].ToString(), concepto[10].ToString(), concepto[11].ToString(), concepto[0].ToString());
                #region Nodo Pagos A Terceros

                if (chkHabilitarTerceros.Checked)
                {
                    txt.SetTercerosConceptoCfdi(tbRazonTercero.Text, tbRfcTercero.Text);
                    txt.AgregaFiscalTercerosConceptoCfdi(tbCalleTercero.Text, tbNoExtTercero.Text, tbNoIntTercero.Text, tbColoniaTercero.Text, tbLocTercero.Text, tbRefTercero.Text, tbMunicipioTercero.Text, tbEstadoTercero.Text, tbPaisTercero.Text, tbCpTercero.Text);
                    if (dtImportesTercerosTemp.Rows.Count > 0)
                    {
                        var trasladosTerceros = dtImportesTercerosTemp.Rows.Cast<DataRow>().Where(row => row.Field<string>("tipo").Equals("02")).ToList();
                        var retencionesTerceros = dtImportesTercerosTemp.Rows.Cast<DataRow>().Where(row => row.Field<string>("tipo").Equals("01")).ToList();
                        trasladosTerceros.ForEach(traslado => txt.AgregaImpuestoTercerosCfdi(false, traslado.Field<string>("impuesto"), traslado.Field<string>("importe"), traslado.Field<string>("tasa")));
                        retencionesTerceros.ForEach(retencion => txt.AgregaImpuestoTercerosCfdi(true, retencion.Field<string>("impuesto"), retencion.Field<string>("importe")));
                    }
                }

                #endregion
                decimal importe = 0;
                decimal importeish = 0;
                decimal importeiui = 0;
                decimal importeConcepto = 0;
                decimal descuentoConcepto = 0;
                decimal.TryParse(concepto[5].ToString(), out importeConcepto);
                decimal.TryParse(concepto[12].ToString(), out descuentoConcepto);
                var baseImpuesto = ControlUtilities.CerosNull((importeConcepto - descuentoConcepto).ToString());
                if (!string.IsNullOrEmpty(concepto[8].ToString()) && decimal.TryParse(concepto[8].ToString(), out importe) && importe > 0)
                {
                    iva16 += importe;
                }
                if (!string.IsNullOrEmpty(concepto[9].ToString()) && decimal.TryParse(concepto[9].ToString(), out importeish) && importeish > 0)
                {
                    ish += importeish;
                }
                if (!string.IsNullOrEmpty(concepto[14].ToString()) && decimal.TryParse(concepto[14].ToString(), out importeiui) && importeiui > 0)//otro impuesto
                {
                    iui += importeiui;
                }
                if (importe > 0)
                {
                    txt.AgregaConceptoImpuestoCfdi(false, baseImpuesto, "IVA", "Tasa", "0.16", importe.ToString(), concepto[0].ToString());
                }else if (importe == 0 && Session["IDENTEMI"].ToString().Equals("ZFM111018JQ4"))
                {
                    bool indemnizacion = false;
                    if (concepto[2].Equals("INDEMNIZACIÓN POR DAÑO"))
                    {
                        indemnizacion = true;
                    }
                    //
                    if (!indemnizacion)
                    {
                        txt.AgregaConceptoImpuestoCfdi(false, baseImpuesto, "IVA", "Tasa", "0.000000", importe.ToString(), concepto[0].ToString());
                    }
                }
                else if (importe == 0 && Session["IDENTEMI"].ToString().Equals("ZFM111018JQ4"))
                {
                    bool indemnizacion = false;
                    if (concepto[2].Equals("INDEMNIZACIÓN POR DAÑO"))
                    {
                        indemnizacion = true;
                    }
                    //
                    if (!indemnizacion)
                    {
                        txt.AgregaConceptoImpuestoCfdi(false, baseImpuesto, "IVA", "Tasa", "0.000000", importe.ToString(), concepto[0].ToString());
                    }
                }
                
                if (!string.IsNullOrEmpty(concepto[7].ToString()))
                {
                    txt.SetPredialConceptoCfdi(concepto[7].ToString());
                }
                sql = "SELECT idDetallesAduanaTemp, numero, fecha, aduana FROM Dat_MX_DetallesAduanaTemp WHERE (id_Empleado = @idUser AND id_DetallesTemp = @idDet AND tipo = 1 AND SessionId = @SessionId)";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                _db.AsignarParametroCadena("@idDet", concepto[0].ToString());
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    txt.AgregaAduaneraConceptoCfdi(dr["numero"].ToString(), dr["fecha"].ToString(), dr["aduana"].ToString());
                }
                _db.Desconectar();
                var partes = new List<object[]>();
                sql = "SELECT idDetallesParteTemp, cantidad, unidad, noIdentificacion, descripcion, valorUnitario, importe FROM Dat_MX_DetallesParteTemp WHERE (id_Empleado = @idUser AND id_DetallesTemp = @idDet AND SessionId = @SessionId)";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                _db.AsignarParametroCadena("@idDet", concepto[0].ToString());
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    var sqlResults = new object[dr.FieldCount];
                    dr.GetValues(sqlResults);
                    partes.Add(sqlResults);
                }
                _db.Desconectar();
                foreach (var parte in partes)
                {
                    txt.AgregaParteConceptoCfdi(parte[1].ToString(), parte[2].ToString(), parte[3].ToString(), parte[4].ToString(), parte[5].ToString(), parte[6].ToString());
                    sql = "SELECT idDetallesAduanaTemp, numero, fecha, aduana FROM Dat_MX_DetallesAduanaTemp WHERE (id_Empleado = @idUser AND id_DetallesParteTemp = @idPart AND tipo = 2 AND SessionId = @SessionId)";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@idUser", _idUser);
                    _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                    _db.AsignarParametroCadena("@idPart", parte[0].ToString());
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        txt.AgregaParteAduaneraConceptoCfdi(dr["numero"].ToString(), dr["fecha"].ToString(), dr["aduana"].ToString());
                    }
                    _db.Desconectar();
                }

                sql = "SELECT cc2.descripcion as impuesto, id.tarifa AS tasa, id.valor as importe, tipoFactor FROM Dat_ImpuestosDetallesTemp id INNER JOIN Cat_Catalogo1_C cc1 ON (cc1.codigo = id.codigoTemp AND cc1.tipo = 'ImpuestoMX') INNER JOIN Cat_Catalogo1_C cc2 ON (cc2.codigo = CONVERT(VARCHAR(MAX), id.codigo) AND cc2.tipo = (CASE cc1.codigo WHEN '01' THEN 'Impuesto Retenido' ELSE 'Impuesto Trasladado' END)) WHERE (cc1.codigo = @codigo AND id.id_DetallesTemp = @idDetalle)";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@idDetalle", concepto[0].ToString());
                _db.AsignarParametroCadena("@codigo", "01");
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    // RETENCIONES
                    txt.AgregaConceptoImpuestoCfdi(true, baseImpuesto, dr["impuesto"].ToString(), dr["tipoFactor"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString(), concepto[0].ToString());
                }
                _db.Desconectar();
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@idDetalle", concepto[0].ToString());
                _db.AsignarParametroCadena("@codigo", "02");
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    // TRASLADOS
                    txt.AgregaConceptoImpuestoCfdi(false, baseImpuesto, dr["impuesto"].ToString(), dr["tipoFactor"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString(), concepto[0].ToString());
                }
                _db.Desconectar();

            }
            if (iva16 > 0)
            {
                var tarifa = "16.00";
                if (Session["CfdiVersion"] != null && Session["CfdiVersion"].ToString().Equals("3.3"))
                { tarifa = "0.16"; }
                txt.AgregaImpuestoTrasladoCfdi("IVA", tarifa, CerosNull(iva16.ToString()));
            }
            else if (iva16 == 0 && Session["IDENTEMI"].ToString().Equals("ZFM111018JQ4"))
            {
                var tarifa = "0.00";
                if (Session["CfdiVersion"] != null && Session["CfdiVersion"].ToString().Equals("3.3"))
                { tarifa = "0.00"; }
                txt.AgregaImpuestoTrasladoCfdi("IVA", tarifa, CerosNull(iva16.ToString()));
            }
            if (ish > 0)
            {
                txt.AgregaTrasladoLocalCfdi("ISH", CerosNull(_ishP.ToString()), CerosNull(ish.ToString()));
            }
            if (iui > 0)//IUI
            {
                txt.AgregaTrasladoLocalCfdi("IUI", CerosNull(_iuiP.ToString()), CerosNull(iui.ToString()));
            }
            decimal subTotal = 0;
            decimal.TryParse(CerosNull(tbSubtotal.Text), out subTotal);
            sql = "SELECT cc2.descripcion as impuesto, id.tarifa AS tasa, id.valor as importe, tipoFactor FROM Dat_ImpuestosDetallesTemp id INNER JOIN Cat_Catalogo1_C cc1 ON (cc1.codigo = id.codigoTemp AND cc1.tipo = 'ImpuestoMX') INNER JOIN Cat_Catalogo1_C cc2 ON (cc2.codigo = CONVERT(VARCHAR(MAX), id.codigo) AND cc2.tipo = (CASE cc1.codigo WHEN '01' THEN 'Impuesto Retenido' ELSE 'Impuesto Trasladado' END)) WHERE (cc1.codigo = @codigo AND id_Empleado = @idUser AND SessionId = @SessionId)";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idUser", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            _db.AsignarParametroCadena("@codigo", "01");
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                txt.AgregaImpuestoRetencionCfdi(dr["impuesto"].ToString(), dr["importe"].ToString());
            }
            _db.Desconectar();
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idUser", _idUser);
            _db.AsignarParametroCadena("@codigo", "02");
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                decimal tasa = 0;
                decimal.TryParse(dr["tasa"].ToString(), out tasa);
                var importe = subTotal * tasa / 100;
                var sImporte = CerosNull(iva16.ToString());
                txt.AgregaImpuestoTrasladoCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), sImporte);
            }
            _db.Desconectar();
            decimal trasl = 0;
            decimal.TryParse(CerosNull(tbTotalImpTras.Text), out trasl);
            trasl += iva16;
            decimal impLocal = ish + iui;
            txt.SetCantidadImpuestosCfdi(tbTotalImpRet.Text, CerosNull(trasl.ToString()), CerosNull(iva16.ToString()));
            txt.SetImpuestosLocalesCfdi(tbTotalRetLoc.Text, CerosNull(impLocal.ToString()));//ish.ToString()));
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
            #region LeyendasFiscales

            if (chkHabilitarLeyendasFiscales.Checked)
            {
                txt.AgregarLeyendaFiscalCfdi(tbLeyendas_Disposicion.Text, tbLeyendas_Norma.Text, tbLeyendas_Texto.Text);
            }

            #endregion
            #region ComercioExterior

            if (chkHabilitarComercioExterior.Checked)
            {
                // ToDo
            }

            #endregion
            var tipocomp = "ingreso";
            string folioFiscalOriginal = null;
            if (Session["_CfdiRelacionados"] != null)
            {
                var lista = (List<CfdiRelacionadoTemp>)Session["_CfdiRelacionados"];
                if (lista.Count > 0)
                {
                    folioFiscalOriginal = string.Join(",", lista.Select(x => x.Uuid + ":" + x.TipoRelacion));
                }
            }
            string observacion_cupon = tbCupon.Text != "" ? ". Cupón: " + tbCupon.Text + " " : "";
            string observacion_mesEmision = ddlMesEmision.Visible && ddlMesEmision.SelectedValue != "" ? ". INGRESO DEL MES: " + ddlMesEmision.SelectedValue + " " : "";

            string observaciones = (tbObservaciones.Text + observacion_cupon + observacion_mesEmision).Trim(new[] { '.', ' ' });
            string condicionesPago = (tbCondPago.Text + observacion_mesEmision).Trim(new[] { '.', ' ' });

            var fecha = Localization.Now.ToString("s");
            if (rowFechaEmision.Visible && !string.IsNullOrEmpty(tbFechaEmision.Text))
            {
                fecha = tbFechaEmision.Text;
            }

            string SQL_ConsultarFolioConsecuente = "SElECT TOP 1 (g.folio + 1) AS folio FROM Dat_General g INNER JOIN Cat_Emisor e ON g.id_Emisor = e.IDEEMI WHERE g.serie = @Serie ORDER BY folio DESC";
            var folioGenerado = "";
            _db.Conectar();
            _db.CrearComando(SQL_ConsultarFolioConsecuente);
            _db.AsignarParametroCadena("@Serie", ddlSerie.SelectedItem.Text.ToString());
            dr = _db.EjecutarConsulta();
            while (dr.Read())
            {
                folioGenerado = dr["folio"].ToString();
            }
            _db.Desconectar();


            txt.SetComprobanteCfdi(ddlSerie.SelectedItem.Text, folioGenerado, fecha, tbFormaPago.Text, condicionesPago, tbSubtotal.Text, tbDescuento.Text, tbMotivoDescuento.Text, tbTipoCambio.Text, ddlMoneda.SelectedValue, tbTotalFac.Text, tipocomp, string.Join(",", selected), tbLugarExp.Text, tbNumCtaPago.Text, folioFiscalOriginal, null, null, null, null, null, tbOtrosCargos.Text, tbTotal.Text, observaciones, CheckAdendaPropina.Checked == true ? "1" : "", "", "", "", chkCredito.Checked);
            //txt.SetComprobanteCfdi(ddlSerie.SelectedItem.Text, "", fecha, tbFormaPago.Text, condicionesPago, tbSubtotal.Text, tbDescuento.Text, tbMotivoDescuento.Text, tbTipoCambio.Text, ddlMoneda.SelectedValue, tbTotalFac.Text, tipocomp, string.Join(",", selected), tbLugarExp.Text, tbNumCtaPago.Text, folioFiscalOriginal, null, null, null, null, null, tbOtrosCargos.Text, tbTotal.Text, observaciones, CheckAdendaPropina.Checked == true ? "1" : "", "", "", "", chkCredito.Checked);
            if (Session["IDGIRO"].ToString().Equals("1"))
            {
                if (!Session["IDENTEMI"].ToString().Equals("DEC9606203K9") && !Session["IDENTEMI"].ToString().Equals("SER960911DE7"))
                {
                    txt.SetInfoAdicionalHabitacionCfdi(tbHuespedHabitacion.Text, tbReservacionHabitacion.Text, tbHabitacion.Text, tbFechaHabitacion1.Text, tbFechaHabitacion2.Text, tbCupon.Text, tbTipoHabitacion.Text);
                    txt.SetInfoAdicionalEventoCfdi(tbHusespedEvento.Text, tbReservacionEvento.Text, tbFechaEvento.Text);
                }
                txt.SetInfoAdicionalRestauranteCfdi(tbPropina.Text, tbReferenciaRestaurante.Text);
            }
            else if (Session["IDGIRO"].ToString().Contains("2"))
            {
                txt.SetInfoAdicionalRestauranteCfdi(tbPropina.Text, tbReferenciaRestaurante.Text);
            }
            if (divAddendasTlalnepantla.Visible)
            {
                txt.AgregarAddendaTlalnepantlaVECICfdi(vci_ServiceType.Text, vci_idProvider.Text, vci_invoiceNumber.Text, vci_issueDateInvoice.Text, vci_namePassanger.Text, vci_recordLoc.Text, vci_startDate.Text, vci_endtDate.Text, vci_bonus.Text, vci_currency.Text, vci_taxXHostingServices.Text, vci_taxable.Text, vci_unTaxable.Text, vci_vatRate.Text, vci_vatAmount.Text, vci_grossAmount.Text, vci_retencionPercibida.Text, vci_importeRetenido.Text);
            }
            if (divAddenasHmpEUM.Visible)
            {
                txt.AgregarAddendaHamptonEUMCfdi(EUM_Transaction.Text, EUM_Idpedido.Text, EUM_Alvaran.Text, EUM_typecurrency.Text, EUM_tipocambio.Text, EUM_impuesto.Text);
            }
            if (divAddenasHmpANF.Visible)
            {
                txt.AgregarAddendaHamptonANFfdi(ANF_ORDENCOMP.Text, ANF_CODIGO.Text);
            }
            if (divAddenasHmpZF.Visible)
            {
                txt.AgregarAddendaHamptonZFfdi(ZF_ORDEN.Text, ZF_CANTIDAD.Text, ZF_VALORUNI.Text, ZF_IMPORTE.Text, ZF_TIPOMONEDA.Text);
            }
            if (divAddenasHmpSANMINA.Visible)
            {
                txt.AgregarAddendaHamptonSAMINAfdi(SAMINA_ORDENCOMPRA.Text, SAMINA_CORREOPROV.Text, SAMINA_NUMEROPROV.Text, SAMINA_MONEDA.Text, SAMINA_CAMBIO.Text);
            }
            if (divAddenasGENEZF.Visible)
            {
                txt.AgregarAddendaHamptonAGENEfdi(AGENE_MONEDA.Text, AGENE_CAMBIO.Text);
            }
            if (divAddendasGrupoReforma.Visible && !string.IsNullOrEmpty(ddlMesEmision.SelectedValue))
            {
                txt.AgregarAddendaReformaMesCfdi(ddlMesEmision.SelectedValue);
            }
            var txtInvoice = txt.ConstruyeTxtCfdi();
            return txtInvoice;
        }

        /// <summary>
        /// Handles the Click event of the lbPrevisualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbPrevisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                var txtInvoice = BuildTxt();
                if (!string.IsNullOrEmpty(txtInvoice))
                {
                    Session["PREVIEW_PDF_INVOICE"] = txtInvoice;
                    var pageurl = ResolveUrl("~/previsualizarPDF.aspx?invoice=0");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "loadPdfModal('" + pageurl + "&mode=view','" + pageurl + "&mode=download');", true);
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se creó correctamente<br/>" + ex.Message, 4, null);
            }
        }

        /// <summary>
        /// Existes the ticket.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ExisteTicket()
        {
            bool existe = false;
            var folio = tbReferenciaRestaurante.Text.Trim();
            if (!string.IsNullOrEmpty(folio))
            {
                _db.Conectar();
                _db.CrearComando("SELECT idComprobante FROM Dat_General WHERE (folioReservacion = @folio OR noTicket = @folio) AND codDoc = '01' AND estado = 1 AND tipo = 'E'");
                _db.AsignarParametroCadena("@folio", folio);
                _db.AsignarParametroCadena("@folio", folio);
                var dr = _db.EjecutarConsulta();
                existe = dr.Read();
                _db.Desconectar();
            }
            return existe;
        }

        /// <summary>
        /// Existes the reservacion.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ExisteReservacion()
        {
            bool existe = false;
            if (Session["IDENTEMI"].ToString().Equals("IMO700623PJ4") || Session["IDENTEMI"].ToString().Equals("HAP9504215L5") || Session["IDENTEMI"].ToString().Equals("HSC0010193M7") || Session["IDENTEMI"].ToString().Equals("OAP981214DP3") || Session["IDENTEMI"].ToString().Equals("OHF921110BF2") || Session["IDENTEMI"].ToString().Equals("OHS0312152Z4") || Session["IDENTEMI"].ToString().Equals("EOP000815CM5") || Session["IDENTEMI"].ToString().Equals("OHE070511872") || Session["IDENTEMI"].ToString().Equals("IPS0806248X5"))
            {
                var folio = !string.IsNullOrEmpty(tbReservacionEvento.Text) ? tbReservacionEvento.Text : tbReservacionHabitacion.Text;
                if (!string.IsNullOrEmpty(folio))
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT idComprobante FROM Dat_General WHERE folioReservacion = @folio AND codDoc = '01' AND estado = 1 AND tipo = 'E'");
                    _db.AsignarParametroCadena("@folio", folio);
                    var dr = _db.EjecutarConsulta();
                    existe = dr.Read();
                    _db.Desconectar();
                }
            }
            return existe;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkHabilitarLeyendasFiscales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void chkHabilitarLeyendasFiscales_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHabilitarLeyendasFiscales.Checked)
            {
                divValidationLeyendasFiscales.Attributes.Add("habilitado", "");
            }
            else
            {
                divValidationLeyendasFiscales.Attributes.Remove("habilitado");
            }
            divLeyendasFiscales.Visible = chkHabilitarLeyendasFiscales.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkHabilitarComercioExterior control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void chkHabilitarComercioExterior_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHabilitarComercioExterior.Checked)
            {
                divValidationComercioExterior.Attributes.Add("habilitado", "");
            }
            else
            {
                divValidationComercioExterior.Attributes.Remove("habilitado");
            }
            divComercioExterior.Visible = chkHabilitarComercioExterior.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkHabilitarTerceros control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void chkHabilitarTerceros_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHabilitarTerceros.Checked)
            {
                divValidationTerceros.Attributes.Add("habilitado", "");
            }
            else
            {
                divValidationTerceros.Attributes.Remove("habilitado");
            }
            divTerceros.Visible = chkHabilitarTerceros.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTipoImpuestoTercero control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlTipoImpuestoTercero_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbTasaTercero.Text = "";
            tbImporteTercero.Text = "";
            ddlImpuestoTercero.Enabled = false;
            ddlImpuestoTercero.SelectedValue = "00";
            switch (ddlTipoImpuestoTercero.SelectedValue)
            {
                case "01":
                    tbTasaTercero.ReadOnly = true;
                    tbImporteTercero.ReadOnly = false;
                    tbTasaTercero_RequiredFieldValidator.Enabled = false;
                    ddlImpuestoTercero.Items.FindByValue("IEPS").Enabled = false;
                    ddlImpuestoTercero.Items.FindByValue("ISR").Enabled = true;
                    ddlImpuestoTercero.Enabled = true;
                    break;
                case "02":
                    tbTasaTercero.ReadOnly = false;
                    tbImporteTercero.ReadOnly = false;
                    tbTasaTercero_RequiredFieldValidator.Enabled = true;
                    ddlImpuestoTercero.Items.FindByValue("IEPS").Enabled = true;
                    ddlImpuestoTercero.Items.FindByValue("ISR").Enabled = false;
                    ddlImpuestoTercero.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the lbAgregarImpuestoTercero control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAgregarImpuestoTercero_Click(object sender, EventArgs e)
        {
            var dtImportesTercerosTemp = (DataTable)Session["_dtImportesTercerosTemp"];
            var lastId = 0;
            try
            {
                lastId = dtImportesTercerosTemp.Rows.Cast<DataRow>().LastOrDefault().Field<int>("id");
            }
            catch { }
            var nextId = lastId + 1;
            dtImportesTercerosTemp.Rows.Add(nextId, ddlTipoImpuestoTercero.SelectedValue, ddlImpuestoTercero.SelectedValue, tbTasaTercero.Text, tbImporteTercero.Text);
            Session["_dtImportesTercerosTemp"] = dtImportesTercerosTemp;
            gvImpuestosTercero.DataSource = dtImportesTercerosTemp;
            gvImpuestosTercero.DataBind();
            ddlTipoImpuestoTercero.SelectedValue = "00";
            ddlTipoImpuestoTercero_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Handles the RowDeleting event of the gvImpuestosTercero control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeleteEventArgs"/> instance containing the event data.</param>
        protected void gvImpuestosTercero_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var dtImportesTercerosTemp = (DataTable)Session["_dtImportesTercerosTemp"];
            var selectedId = 0;
            try
            {
                selectedId = Convert.ToInt32(gvImpuestosTercero.DataKeys[e.RowIndex].Values["id"].ToString());
            }
            catch { }
            if (selectedId > 0)
            {
                var rowToRemove = dtImportesTercerosTemp.Rows.Cast<DataRow>().SingleOrDefault(x => x.Field<int>("id") == selectedId);
                if (rowToRemove != null)
                {
                    dtImportesTercerosTemp.Rows.Remove(rowToRemove);
                    Session["_dtImportesTercerosTemp"] = dtImportesTercerosTemp;
                    gvImpuestosTercero.DataSource = dtImportesTercerosTemp;
                    gvImpuestosTercero.DataBind();
                }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvImpuestosTercero control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvImpuestosTercero_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    var label = (Label)(e.Row.FindControl("lblTipoImpuesto"));
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var codigoTipoImpuesto = row.Field<string>("tipo");
                    if (!string.IsNullOrEmpty(codigoTipoImpuesto))
                    {
                        label.Text = ddlTipoImpuestoTercero.Items.FindByValue(codigoTipoImpuesto).Text;
                    }
                }
                catch { }
                try
                {
                    var label = (Label)(e.Row.FindControl("lblTasa"));
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var tasa = row.Field<string>("tasa");
                    if (!string.IsNullOrEmpty(tasa))
                    {
                        label.Text = tasa + " %";
                    }
                    else
                    {
                        label.Text = "N/A";
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Handles the Click event of the lbAddendas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAddendas_Click(object sender, EventArgs e)
        {
            //divSinAddendas.Visible = false;
            var giro = Session["IDGIRO"].ToString();
            var rfcemi = Session["IDENTEMI"].ToString();

            if (Session["IDGIRO"].ToString().Contains("1") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|SET920324FC3|OHF921110BF2") && Regex.IsMatch(tbRfcRec.Text, @"VCI0004041R0|CTS160922QLA"))
            {
                divAddendasTlalnepantla.Visible = Session["IDGIRO"].ToString().Contains("1") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|SET920324FC3|OHF921110BF2") && Regex.IsMatch(tbRfcRec.Text, @"VCI0004041R0|CTS160922QLA");
                divSinAddendas.Visible = !divAddendasTlalnepantla.Visible;
            }
            if (Session["IDGIRO"].ToString().Contains("1") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5"))
            {
                divAddendasGrupoReforma.Visible = true;
                divSinAddendas.Visible = !divAddendasGrupoReforma.Visible;
            }
            if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OHE070511872"))
            {
                if (divAddenasHmpEUM.Visible = Session["IDGIRO"].ToString().Contains("1") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OHE070511872") && Regex.IsMatch(tbRfcRec.Text, @"EUM000707DQ2|CGS120904DG8|CIN981030FF3|PEJ931018IIA|OHE070511872"))
                { divSinAddendas.Visible = !divAddenasHmpEUM.Visible; }

                if (divAddenasHmpANF.Visible = Session["IDGIRO"].ToString().Contains("1") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OHE070511872") && Regex.IsMatch(tbRfcRec.Text, @"ANA080808IC1"))
                { divSinAddendas.Visible = !divAddenasHmpANF.Visible; }

                if (divAddenasHmpZF.Visible = Session["IDGIRO"].ToString().Contains("1") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OHE070511872") && Regex.IsMatch(tbRfcRec.Text, @"ZSS8107103P6"))
                { divSinAddendas.Visible = !divAddenasHmpZF.Visible; }

                if (divAddenasHmpSANMINA.Visible = Session["IDGIRO"].ToString().Contains("1") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OHE070511872") && Regex.IsMatch(tbRfcRec.Text, @"SRM9812177W9|SSM950412Q42"))
                { divSinAddendas.Visible = !divAddenasHmpSANMINA.Visible; }

                if (divAddenasGENEZF.Visible = Session["IDGIRO"].ToString().Contains("1") && Regex.IsMatch(Session["IDENTEMI"].ToString(), @"LAN7008173R5|OHE070511872") && Regex.IsMatch(tbRfcRec.Text, @"EMG8107176LA"))
                { divSinAddendas.Visible = !divAddenasGENEZF.Visible; }

            }


            ScriptManager.RegisterStartupScript(this, GetType(), "_showModalAddendas", "$('#divAddendas').modal('show');", true);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkRepetidos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void chkRepetidos_CheckedChanged(object sender, EventArgs e)
        {
            BindData();
        }

        [WebMethod]
        public static bool VerificarExistenciaUuid(string rfcDb, string Uuid)
        {
            var existe = false;
            BasesDatos dbTemp = null;
            try
            {
                dbTemp = new BasesDatos(rfcDb);
                dbTemp.Conectar();
                dbTemp.CrearComando("SELECT idComprobante FROM Dat_General WHERE numeroAutorizacion = @uuid");
                dbTemp.AsignarParametroCadena("@uuid", Uuid);
                var dr = dbTemp.EjecutarConsulta();
                existe = dr.Read();
            }
            catch (Exception ex)
            {
                existe = false;
            }
            finally
            {
                if (dbTemp != null) { dbTemp.Desconectar(); }
            }
            return existe;
        }

        [WebMethod]
        public static bool VerificarExistenciaSerieFolio(string rfcDb, string Serie, string Folio)
        {
            var existe = false;
            BasesDatos dbTemp = null;
            try
            {
                dbTemp = new BasesDatos(rfcDb);
                dbTemp.Conectar();
                dbTemp.CrearComando("SELECT idComprobante FROM Dat_General WHERE serie = @serie AND folio = @folio AND ISNULL(numeroAutorizacion, '') <> ''");
                dbTemp.AsignarParametroCadena("@serie", Serie.Trim());
                dbTemp.AsignarParametroCadena("@folio", Folio.Trim());
                var dr = dbTemp.EjecutarConsulta();
                existe = dr.Read();
            }
            catch (Exception ex)
            {
                existe = false;
            }
            finally
            {
                if (dbTemp != null) { dbTemp.Desconectar(); }
            }
            return existe;
        }

        protected void bAgregarRelacionado_Click(object sender, EventArgs e)
        {
            DbDataReader dr;
            var uuid = "";
            if (ddlBusq.SelectedValue.Equals("1"))
            {
                // Busqueda por UUID
                uuid = tbUUID.Text;
            }
            else if (ddlBusq.SelectedValue.Equals("2"))
            {
                // Busqueda por Serie y Folio
                _db.Conectar();
                try
                {
                    _db.CrearComando("SELECT TOP 1 numeroAutorizacion FROM Dat_General WHERE serie = @serie AND folio = @folio ORDER BY idComprobante DESC");
                    _db.AsignarParametroCadena("@serie", tbSerie.Text.Trim());
                    _db.AsignarParametroCadena("@folio", tbFolio.Text.Trim());
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        uuid = dr["numeroAutorizacion"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
                finally
                {
                    _db.Desconectar();
                }
            }
            if (string.IsNullOrEmpty(uuid))
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se encontró el comprobante con los parámetros introducidos", 4);
                return;
            }
            if (Session["_CfdiRelacionados"] != null)
            {
                uuid = uuid.Trim();
                var lista = (List<CfdiRelacionadoTemp>)Session["_CfdiRelacionados"];
                if (!lista.Any(x => x.Uuid.Equals(uuid)))
                {
                    var lastId = 0;
                    if (lista.Count > 0) { lastId = lista.Max(x => x.Id); }
                    var nextId = lastId + 1;
                    lista.Add(new CfdiRelacionadoTemp
                    {
                        Id = nextId,
                        Uuid = uuid,
                        TipoRelacion = ddlTipoRelacion.SelectedValue,
                        Observaciones = ""
                    });
                    ddlTipoRelacion.Enabled = false;
                    Session["_CfdiRelacionados"] = lista;
                    tbUUID.Text = "";
                    tbSerie.Text = "";
                    tbFolio.Text = "";
                    if (ddlBusq.SelectedValue.Equals("1"))
                    {
                    } else if (ddlBusq.SelectedValue.Equals("2"))
                    {
                        ddlBusq.SelectedValue = "1";
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El UUID ya se encuentra en la lista", 4);
                    return;
                }
                LlenaGvRelacionados();
            }
        }

        private void LlenaGvRelacionados()
        {
            var lista = (List<CfdiRelacionadoTemp>)Session["_CfdiRelacionados"];
            gvCfdiRelacionados.DataSource = lista;
            gvCfdiRelacionados.DataBind();
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            if (Session["_CfdiRelacionados"] != null)
            {
                var btn = (LinkButton)sender;
                var id = 0;
                int.TryParse(btn.CommandArgument, out id);
                var lista = (List<CfdiRelacionadoTemp>)Session["_CfdiRelacionados"];
                if (lista.Any(x => x.Id == id))
                {
                    lista.RemoveAll(x => x.Id.Equals(id));
                    Session["_CfdiRelacionados"] = lista;
                }
                ddlTipoRelacion.Enabled = lista.Count <= 0;
                LlenaGvRelacionados();
            }
        }

        protected void ddlDesCon_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbDescConc.Text = ddlDesCon.SelectedItem.Text;
            tbDescConc_TextChanged(sender, e);
            chkEditarItem33.Visible = true;
            tbCutomItem.Text = "";
        }

        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFormaPago.Text = ddlFormaPago.SelectedItem.Text;
            if (ddlFormaPago.SelectedValue.Equals("PPD"))
            {
                ddlMetodoPago.SelectedValue = "99";
                ddlMetodoPago.Enabled = false;
                if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
                {
                    ddlMetodoPago.Enabled = true;
                }
                divChkCredito.Style["display"] = "none";
            }
            else
            {
                ddlMetodoPago.Enabled = true;
                divChkCredito.Style["display"] = Regex.IsMatch(Session["IDENTEMI"].ToString(), @"SET920324FC3|OPL000131DL3|OHR980618BLA|OHC070227M80|HOS140313LH7|LAN7008173R5") ? "inline" : "none";
            }
        }

        protected void lbCatalogoConceptos_Click(object sender, EventArgs e)
        {
            var urlAsp = "~/configuracion/Conceptos.aspx";
            var urlRelative = ResolveClientUrl(urlAsp);
            var callBack = $"SimulateClick(\"{lbRecargar.ClientID}\");";
            (Master as SiteMaster).MostrarUrlModal(this, urlRelative, "Catálogo de Conceptos", "", callBack, 500);
        }

        protected void lbRecargar_Click(object sender, EventArgs e)
        {
            ddlCatConc.DataBind();
            ddlCatConc.SelectedValue = "0";
            ddlCatConc_SelectedIndexChanged(null, null);
        }

        protected void chkReciboRetenciones_CheckedChanged(object sender, EventArgs e)
        {
            //_db.Conectar();
            //_db.CrearComando(@"delete from Dat_ImpuestosDetallesTemp where id_Empleado = @ID AND SessionId = @SessionId;");
            //_db.AsignarParametroCadena("@ID", _idUser);
            //_db.AsignarParametroCadena("@SessionId", Session.SessionID);
            //_db.EjecutarConsulta1();
            //_db.Desconectar();
            tbRetIva.Text = "";
            tbRetIsr.Text = "";
            decimal dRetIva = 0;
            decimal dRetIsr = 0;
            var aplicaIsrRet = !Regex.IsMatch(Session["IDENTEMI"].ToString(), @"AEBC640406NS3|AECE870621VB3|AECE890206JS4");
            if (chkReciboRetenciones.Checked)
            {
                var conceptos = new List<object[]>();
                var sql = "SELECT idDetallesTemp, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial, descuento as iva16, ish, claveProdServ, claveUnidad, descuentoCfdi33, precioRet FROM Dat_DetallesTemp WHERE (id_Empleado = @idUser AND SessionId = @SessionId)";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                var dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    var sqlResults = new object[dr.FieldCount];
                    dr.GetValues(sqlResults);
                    conceptos.Add(sqlResults);
                }
                _db.Desconectar();
                foreach (var concepto in conceptos)
                {
                    decimal descuento = 0;
                    decimal importe = 0;
                    decimal.TryParse(concepto[5].ToString(), out importe);
                    decimal.TryParse(concepto[12].ToString(), out descuento);
                    decimal baseImpuesto = importe - descuento;
                    decimal tasaIvaRet = 0;
                    decimal tasaIsrRet = 0;
                    decimal tasaIvaTras = 0;
                    decimal ivaTras = 0;
                    if (!string.IsNullOrEmpty(concepto[8].ToString()))
                    {
                        decimal.TryParse(concepto[8].ToString(), out ivaTras);
                    }
                    tasaIvaTras = decimal.Parse(CerosNull((ivaTras / baseImpuesto).ToString(), true));
                    if (tasaIvaTras > 1)
                    {
                        tasaIvaTras = tasaIvaTras / 100;
                    }
                    decimal ivaRet = 0;
                    decimal isrRet = 0;
                    if (ddlTipoReciboRetencion.SelectedValue.Equals("1"))
                    {
                        // HONORARIOS
                        ivaRet = decimal.Parse(CerosNull((((baseImpuesto * tasaIvaTras) / 3) * 2).ToString()));
                        tasaIvaRet = decimal.Parse(CerosNull((ivaRet / baseImpuesto).ToString(), true));
                        tasaIsrRet = (decimal)0.10;
                    }
                    else if (ddlTipoReciboRetencion.SelectedValue.Equals("2"))
                    {
                        // ARRENDAMIENTO
                        ivaRet = decimal.Parse(CerosNull((((baseImpuesto * tasaIvaTras) / 3) * 2).ToString()));
                        tasaIvaRet = decimal.Parse(CerosNull((ivaRet / baseImpuesto).ToString(), true));
                        tasaIsrRet = (decimal)0.10;
                    }
                    var minimoIvaRet = (baseImpuesto - ((decimal)0.01 / 2)) * (decimal.Parse(CerosNull(tasaIvaRet.ToString(), true)));
                    var maximoIvaRet = (baseImpuesto + ((decimal)0.01 / 2) - (10 ^ -12)) * (decimal.Parse(CerosNull(tasaIvaRet.ToString(), true)));
                    if (ivaRet < minimoIvaRet)
                    {
                        ivaRet = minimoIvaRet;
                    }
                    else if (ivaRet > maximoIvaRet)
                    {
                        ivaRet = maximoIvaRet;
                    }
                    isrRet = baseImpuesto * tasaIsrRet;
                    dRetIva += ivaRet;
                    dRetIsr += isrRet;

                    _db.Conectar();
                    _db.CrearComando(@"insert into Dat_ImpuestosDetallesTemp
                           (codigo,tarifa,valor,codigoTemp,id_Empleado,SessionId,tipoFactor,id_DetallesTemp)
                           values
                           (@codigo,@tarifa,@valor,'01',@id_Empleado,@SessionId,'Tasa',@id_DetallesTemp)");
                    _db.AsignarParametroCadena("@codigo", "1"); // IVA
                    _db.AsignarParametroCadena("@tarifa", CerosNull(tasaIvaRet.ToString(), true));
                    _db.AsignarParametroCadena("@valor", CerosNull(ivaRet.ToString()));
                    _db.AsignarParametroCadena("@id_Empleado", _idUser);
                    _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                    _db.AsignarParametroCadena("@id_DetallesTemp", concepto[0].ToString());
                    _db.EjecutarConsulta1();
                    _db.Desconectar();

                    if (aplicaIsrRet)
                    {
                        _db.Conectar();
                        _db.CrearComando(@"insert into Dat_ImpuestosDetallesTemp
                           (codigo,tarifa,valor,codigoTemp,id_Empleado,SessionId,tipoFactor,id_DetallesTemp)
                           values
                           (@codigo,@tarifa,@valor,'01',@id_Empleado,@SessionId,'Tasa',@id_DetallesTemp)");
                        _db.AsignarParametroCadena("@codigo", "2"); // ISR
                        _db.AsignarParametroCadena("@tarifa", CerosNull(tasaIsrRet.ToString(), true));
                        _db.AsignarParametroCadena("@valor", CerosNull(isrRet.ToString()));
                        _db.AsignarParametroCadena("@id_Empleado", _idUser);
                        _db.AsignarParametroCadena("@SessionId", Session.SessionID);
                        _db.AsignarParametroCadena("@id_DetallesTemp", concepto[0].ToString());
                        _db.EjecutarConsulta1();
                        _db.Desconectar();
                    }
                }

            }
            if (!aplicaIsrRet)
            {
                dRetIsr = 0;
            }
            CalcularTotales();
            if (chkReciboRetenciones.Checked)
            {
                tbRetIva.Text = CerosNull(dRetIva.ToString());
                tbRetIsr.Text = CerosNull(dRetIsr.ToString());
            }
            rowTotalesArrendamiento.Visible = chkReciboRetenciones.Checked;
        }

        protected void ddlTipoReciboRetencion_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkReciboRetenciones.Checked = false;
            chkReciboRetenciones_CheckedChanged(null, null);
            chkReciboRetenciones.Visible = !ddlTipoReciboRetencion.SelectedValue.Equals("");
        }

        protected void ddlSucEmi_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idSucursal = ddlSucEmi.SelectedValue;
            var rfcEmisor = Session["rfcSucursal"].ToString();
            if (!string.IsNullOrEmpty(idSucursal) && !idSucursal.Equals("0"))
            {
                try
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT s.RFC FROM Cat_SucursalesEmisor s WHERE s.idSucursal = @idSucursal");
                    _db.AsignarParametroCadena("@idSucursal", idSucursal);
                    var dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        rfcEmisor = dr["RFC"].ToString();
                    }
                    _db.Desconectar();
                    if (!string.IsNullOrEmpty(rfcEmisor))
                    {
                        Llenarlista(rfcEmisor, "emi");
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }
    }
}