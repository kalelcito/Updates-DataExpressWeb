// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 05-22-2017
// ***********************************************************************
// <copyright file="crearCartaPorte.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Control;
using Datos;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text.RegularExpressions;
using DataExpressWeb.nuevo;
using System.Web.Services;
using DataExpressWeb.wsEmision;

namespace DataExpressWeb
{
    /// <summary>
    /// Class CrearCartaPorte.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CrearCartaPorte : Page
    {
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
        /// The sucursal emisor
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
        private static NumerosALetras numLetra;

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
                SqlDataConcTemp.ConnectionString = _db.CadenaConexion;
                SqlDataAduanerasConcTemp.ConnectionString = _db.CadenaConexion;
                SqlDataPartes.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.ConnectionString = _db.CadenaConexion;
                SqlDataReceptores.ConnectionString = _db.CadenaConexion;
                SqlDataAduanerasParteTemp.ConnectionString = _db.CadenaConexion;
                SqlDataContactos.ConnectionString = _db.CadenaConexion;
                SqlDataSource2.ConnectionString = _db.CadenaConexion;
                SqlDataCatConc.ConnectionString = _db.CadenaConexion;
                SqlDataDescConc.ConnectionString = _db.CadenaConexion;
                SqlDataSourceUsoCfdi.ConnectionString = _db.CadenaConexion;
                SqlDataSourceUnidades.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.SelectCommand = SqlDataMetodoPago.SelectCommand.Replace("@tipoCatalogo", Session["CfdiVersion"].ToString().Equals("3.3") ? "MetodoPagoCfdi33" : "MetodoPago");

                #region Asignar Sesion en DataSources Temporales

                SqlDataAduanerasConcTemp.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataAduanerasParteTemp.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataContactos.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataConcTemp.SelectParameters["SessionId"].DefaultValue = Session.SessionID;
                SqlDataPartes.SelectParameters["SessionId"].DefaultValue = Session.SessionID;

                #endregion

                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                    if (!Page.IsPostBack)
                    {
                        numLetra = new NumerosALetras();
                        _sucursalEmisor = "";
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
                        Llenarlista(Session["IDENTEMI"].ToString(), "emi");
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
                                rowHabEv.Visible = true;
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
                                tbFormaPago.Text = "PAGO EN UNA SOLA EXHIBICIÓN";
                                tbFormaPago.ReadOnly = true;
                                btnFormaPago.Disabled = true;
                                rowExpedidoEn.Visible = false;
                                rowDescuento.Visible = false;
                                rowCondiciones.Visible = false;
                                divDescuentoTot.Visible = false;
                                //divTipoCambio.Visible = true;
                                divIdentificacion.Visible = false;
                                divUnidad.Visible = false;
                                //rowCategoria.Visible = false;
                                rowDenomSocial.Visible = false;
                                rowISHEdit.Visible = false;
                                tbISHEdit.Text = "0.00";
                                tbReferenciaRestaurante.Text = "";
                                divReferenciaRestaurante.Style["display"] = "inline";

                                if (Session["IDENTEMI"].ToString().Equals("OHC080924AV5") || Session["IDENTEMI"].ToString().Equals("DEC9606203K9") || Session["IDENTEMI"].ToString().Equals("SER960911DE7"))
                                {
                                    divAdendaPropina.Style["display"] = "inline";
                                }
                                if (Session["IDENTEMI"].ToString().Equals("GIO100406FS6") || Session["IDENTEMI"].ToString().Equals("IET661110DF2") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                                {
                                    //rowTipoHabitacion.Style["display"] = "inline";
                                    //tbTipoHabitacion.Visible = true;
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

                        LoadTiposCambio();
                        CustomValidator1.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                        //ddlMetodoPago_SelectedIndexChanged(null, null);
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
                    }
                    chkHabilitarIva16.Attributes.Add("checkStrings", "16%|0%");
                    chkHabilitarIva4.Attributes.Add("checkStrings", "4%|0%");
                }
            }
            else
            {
                Response.Redirect("~/Cerrar.aspx");
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
                chkHabilitarIva4.Checked = false;
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
                    return false;
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
            foreach (var txt in lista)
            {
                if (string.IsNullOrEmpty(txt.Text))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Validars the habitacion evento.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool ValidarHabitacionEvento()
        {
            var control = true;
            var txtHabitacion = divH.FindDescendants<TextBox>();
            var txtEventos = divE.FindDescendants<TextBox>();
            if (!TextBoxesVacios(txtHabitacion))
            {
                if (HayTxtVacios(txtHabitacion))
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Debe llenar todos los campos de la Habitación", 4, null);
                    control = false;
                }
            }
            else if (!TextBoxesVacios(txtEventos))
            {
                if (HayTxtVacios(txtEventos))
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Debe llenar todos los campos del Evento", 4, null);
                    control = false;
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "Para empresas con giro de Hotelería es necesario registrar una Habitación o un Evento", 4, null);
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
                var randomMs = new Random().Next(1000, 5000);
                System.Threading.Thread.Sleep(randomMs);
                var coreMx = new WsEmision { Timeout = (1800 * 5000) };
                var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, "06", false, true, "", "");
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

        private string BuildTxt()
        {
            var selected = new List<string>();
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                selected.Add(ddlMetodoPago.SelectedValue);
            }
            else
            {
                selected = chkMetodoPago.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();
            }
            if (Session["IDGIRO"].ToString().Equals("1") && !ValidarHabitacionEvento())
            {
                return null;
            }
            else if (selected.Count <= 0)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe escoger al menos un método de pago", 4, null);
                return null;
            }
            else if (string.IsNullOrEmpty(tbNumCtaPago.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe definir al menos un número de cuenta separados por coma (,). Si el método de pago es deferente a tarjeta, éste deberá ser NO IDENTIFICADO", 4, null);
                return null;
            }
            DbDataReader dr;
            var conceptos = new List<object[]>();
            var txt = new SpoolMx();
            decimal iva16 = 0;
            decimal ish = 0;
            decimal ivaret = 0;
            txt.SetEmisorCfdi(tbNomEmi.Text, tbRfcEmi.Text, tbCURPE.Text, tbTelEmi.Text, tbMailEmi.Text, Session["IDGIRO"].ToString(), tbRegimenFiscal.Text);
            if (chkDomEmi.Checked)
            {
                txt.SetEmisorDomCfdi(tbCalleEmi.Text, tbNoExtEmi.Text, tbNoIntEmi.Text, tbColoniaEmi.Text, "", "", tbMunicipioEmi.Text, tbEstadoEmi.Text, tbPaisEmi.Text, tbCpEmi.Text);
            }
            if (cbEmiExp.Checked)
            {
                txt.SetEmisorExpCfdi(tbCalleExp.Text, tbNoExtExp.Text, tbNoIntExp.Text, tbColoniaExp.Text, "", "", tbMunicipioExp.Text, tbEstadoExp.Text, tbPaisExp.Text, tbCpExp.Text, _sucursalEmisor);
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
            sql = "SELECT idDetallesTemp, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial, descuento as iva16, ish, claveProdServ, claveUnidad, descuentoCfdi33,precioRet FROM Dat_DetallesTemp WHERE (id_Empleado = @idUser AND SessionId = @SessionId)";
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
            foreach (var concepto in conceptos)
            {
                txt.AgregaConceptoCfdi(concepto[4].ToString(), concepto[6].ToString(), concepto[1].ToString(), concepto[2].ToString(), concepto[3].ToString(), concepto[5].ToString(), concepto[12].ToString(), concepto[10].ToString(), concepto[11].ToString(), concepto[0].ToString());
                decimal importe = 0;
                decimal importeish = 0;
                decimal importeret = 0;
                if (!string.IsNullOrEmpty(concepto[8].ToString()) && decimal.TryParse(concepto[8].ToString(), out importe) && importe > 0)
                {
                    iva16 += importe;
                }
                if (!string.IsNullOrEmpty(concepto[13].ToString()) && decimal.TryParse(concepto[13].ToString(), out importeret) && importeret > 0)
                {
                    ivaret += importeret;
                }
                if (!string.IsNullOrEmpty(concepto[9].ToString()) && decimal.TryParse(concepto[9].ToString(), out importeish) && importeish > 0)
                {
                    ish += importeish;
                }
                if (importe > 0)
                {
                    txt.AgregaConceptoImpuestoCfdi(false, concepto[5].ToString(), "IVA", "Tasa", "0.16", importe.ToString(), concepto[0].ToString());
                }
                if (importeret > 0)
                {
                    decimal montoConcepto = 0;
                    decimal.TryParse(concepto[5].ToString(), out montoConcepto);
                    var tasaRet = ((importeret * 100) / montoConcepto) / 100;
                    txt.AgregaConceptoImpuestoCfdi(true, concepto[5].ToString(), "IVA", "Tasa", CerosNull(tasaRet.ToString()), CerosNull(importeret.ToString()), concepto[0].ToString());
                    txt.AgregaImpuestoRetencionCfdi("IVA", CerosNull(importeret.ToString()), "Tasa");
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
            }
            if (iva16 > 0)
            {
                var tarifa = "16.00";
                if (Session["CfdiVersion"] != null && Session["CfdiVersion"].ToString().Equals("3.3"))
                { tarifa = "0.16"; }
                txt.AgregaImpuestoTrasladoCfdi("IVA", tarifa, CerosNull(iva16.ToString()), "Tasa");
            }
            decimal subTotal = 0;
            decimal.TryParse(CerosNull(tbSubtotal.Text), out subTotal);
            sql = "SELECT cc2.descripcion as impuesto, id.tarifa AS tasa, id.valor as importe FROM Dat_ImpuestosDetallesTemp id INNER JOIN Cat_Catalogo1_C cc1 ON (cc1.codigo = id.codigoTemp AND cc1.tipo = 'ImpuestoMX') INNER JOIN Cat_Catalogo1_C cc2 ON (cc2.codigo = id.codigo AND cc2.tipo = (CASE cc2.codigo WHEN '01' THEN 'Impuesto Retenido' ELSE 'Impuesto Trasladado' END)) WHERE (cc1.codigo = @codigo AND id_Empleado = @idUser AND SessionId = @SessionId)";
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
            decimal retn = 0;
            trasl += iva16;
            retn += ivaret;
            txt.SetCantidadImpuestosCfdi(CerosNull(retn.ToString()), CerosNull(trasl.ToString()), CerosNull(iva16.ToString()));
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
            string observacion_cupon = tbCupon.Text != "" ? ". Cupón: " + tbCupon.Text : "";
            txt.SetComprobanteCfdi(ddlSerie.SelectedItem.Text, "", Localization.Now.ToString("s"), tbFormaPago.Text, tbCondPago.Text, tbSubtotal.Text, tbDescuento.Text, tbMotivoDescuento.Text, tbTipoCambio.Text, ddlMoneda.SelectedValue, tbTotalFac.Text, tipocomp, string.Join(",", selected), tbLugarExp.Text, tbNumCtaPago.Text, folioFiscalOriginal, null, null, null, null, null, tbOtrosCargos.Text, tbTotal.Text, tbObservaciones.Text + observacion_cupon, CheckAdendaPropina.Checked == true ? "1" : "");
            txt.SetInfoAdicionalMercanciaCfdi(tbOrigenMercancia.Text, tbDestinoMercancia.Text, tbConductorMercancia.Text, tbVehiculoMercancia.Text, tbPlacasMercancia.Text, tbKmMercancia.Text);
            var txtInvoice = txt.ConstruyeTxtCfdi();
            return txtInvoice;
        }

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
        /// Calculars the totales.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void CalcularTotales(object sender = null, EventArgs e = null)
        {
            decimal subTotal = 0;
            decimal descuento = 0;
            decimal total = 0;
            decimal iva16 = 0;
            decimal iva4 = 0;
            var sql = @"SELECT ISNULL(SUM(precioTotalSinImpuestos), 0.00) AS subtotal, ISNULL(SUM(descuento), 0.00) as iva16, ISNULL(SUM(precioRet), 0.00) AS iva14, ISNULL(SUM(CONVERT(FLOAT, descuentoCfdi33)), 0.00) as descuento FROM Dat_DetallesTemp WHERE (id_Empleado = @idUser AND SessionId = @SessionId)";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idUser", _idUser);
            _db.AsignarParametroCadena("@SessionId", Session.SessionID);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                decimal.TryParse(CerosNull(dr["subtotal"].ToString()), out subTotal);
                decimal.TryParse(CerosNull(dr["iva16"].ToString()), out iva16);
                decimal.TryParse(CerosNull(dr["iva14"].ToString()), out iva4);
                decimal.TryParse((dr["descuento"].ToString()), out descuento);
            }
            _db.Desconectar();

            total = subTotal - descuento + iva16 - iva4;
            tbIva16.Text = CerosNull(iva16.ToString());
            tbIvaRet.Text = CerosNull(iva4.ToString());
            tbDescuento.Text = CerosNull(descuento.ToString());
            tbSubtotal.Text = CerosNull(subTotal.ToString());
            tbTotal.Text = CerosNull(total.ToString());
            tbTotalFac.Text = CerosNull(total.ToString());
            tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, ddlMoneda.SelectedValue);
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
                                    ,id_Empleado,SessionId,PrecioTotalSinImpuestos,unidad" + (!string.IsNullOrEmpty(tbNumeroPredial.Text) ? ",ctaPredial" : "") + (chkHabilitarIva16.Checked ? ",descuento" : "") + (chkHabilitarIva4.Checked ? ",precioRet" : "") + @",claveProdServ,claveUnidad,descuentoCfdi33) OUTPUT inserted.idDetallesTemp
                                    VALUES
                                    (" + (!string.IsNullOrEmpty(tbIdentConc.Text) ? "@codigoPrincipal," : "") + @"@descripcion,@cantidad,@precioUnitario
                                   ,@id_Empleado,@SessionId,@PrecioTotalSinImpuestos,@unidad" + (!string.IsNullOrEmpty(tbNumeroPredial.Text) ? ",@ctaPredial" : "") + (chkHabilitarIva16.Checked ? ",@iva16" : "") + (chkHabilitarIva4.Checked ? ",@iva4" : "") + @",@claveProdServ,@claveUnidad,@descuentoCfdi33)";
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
            decimal.TryParse(CerosNull(tbImpConc.Text), out importe);
            decimal.TryParse(CerosNull(tbCantidadConc.Text), out cantidad);
            var iva16 = importe * 16 / 100;
            var iva4 = (chkHabilitarIva4.Checked ? (importe * (decimal)0.04) : 0);
            if (chkDesgloce.Checked)
            {
                var impuestos = (Convert.ToDecimal(16) + (chkHabilitarIva4.Checked ? (decimal)0.04 : (decimal)0.00)) + 1;
                var importeP = importe / impuestos;
                iva16 = importeP * Convert.ToDecimal(0.16);
                iva4 = chkHabilitarIva4.Checked ? importeP * (decimal)0.04 : 0;
                tbVUConc.Text = (Math.Round(importeP / cantidad, 4).ToString());
                tbImpConc.Text = (Math.Round(importeP, 4).ToString());
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
            if (!string.IsNullOrEmpty(tbNumeroPredial.Text))
            {
                _db.AsignarParametroCadena("@ctaPredial", tbNumeroPredial.Text);
            }
            if (chkHabilitarIva16.Checked)
            {
                _db.AsignarParametroCadena("@iva16", CerosNull(iva16.ToString()));
            }
            if (chkHabilitarIva4.Checked)
            {
                _db.AsignarParametroCadena("@iva4", CerosNull(iva4.ToString()));
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
            chkHabilitarIva4.Checked = false;
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
            RequiredFieldValidator14.Enabled = cbDomRec.Checked;
            RequiredFieldValidator15.Enabled = cbDomRec.Checked;
            RequiredFieldValidator16.Enabled = cbDomRec.Checked;
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

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlDescConc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbDescConc_TextChanged(object sender, EventArgs e)
        {
            var isHotel = false;
            if (Session["IDGIRO"].ToString().Contains("1"))
            {
                isHotel = Regex.IsMatch(ddlCatConc.SelectedItem.Text, @".*HOTEL.*", RegexOptions.IgnoreCase);
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
            rowISHEdit.Visible = false;
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
            }
            else
            {
                ddlMetodoPago.Enabled = true;
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
    }
}