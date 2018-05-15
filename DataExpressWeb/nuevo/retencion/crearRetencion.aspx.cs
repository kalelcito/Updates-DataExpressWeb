// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="crearRetencion.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Control;
using Datos;
using System.Xml;

namespace DataExpressWeb
{
    /// <summary>
    /// Class CrearRetencion.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CrearRetencion : Page
    {

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
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
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            _log = new Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            SqlDataImpTemp.ConnectionString = _db.CadenaConexion;
            SqlDataSeries.ConnectionString = _db.CadenaConexion;
            if (Session["idUser"] != null)
            {
                _idUser = Session["idUser"].ToString();
                if (!Page.IsPostBack)
                {
                    _db.Conectar();
                    _db.CrearComando(@"delete from Dat_ImpuestosDetallesTemp where id_Empleado = @ID; delete from Dat_DetallesTemp where id_Empleado = @ID; delete from Dat_MX_DetallesAduanaTemp where id_Empleado = @ID; delete from Dat_MX_DetallesParteTemp where id_Empleado = @ID; delete from Dat_MX_ImpLocalesTemp where id_Empleado = @ID; delete from Cat_Mx_Contactos_Temp where id_Empleado = @ID; delete from Dat_DetallesNominaTemp where idUser = @ID; delete from DAT_MX_ImpuestosRetencionesTemp where id_Empleado = @ID;");
                    var deleted = false;
                    var commas = _db.Comando.CommandText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    for (int i = 0; i < commas; i++)
                    {
                        _db.AsignarParametroCadena("@ID", _idUser);
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
                    //_db.Conectar();
                    //_db.CrearComando(@"SELECT rfcemisor from Par_ParametrosSistema");
                    //var drSum = _db.EjecutarConsulta();
                    //while (drSum.Read())
                    //{
                    //    _rucEmisor = drSum[0].ToString();
                    //}
                    //_db.Desconectar();
                    _rucEmisor = Session["rfcSucursal"].ToString();
                    Llenarlista(_rucEmisor, "emi");
                    ddlPaisExt.DataTextField = "Text";
                    ddlPaisExt.DataValueField = "Value";
                    ddlPaisExt.DataSource = _listaPaises;
                    ddlPaisExt.DataBind();
                    ddlSerie.DataBind();
                    ddlSerie.SelectedIndex = 0;
                    ddlSerie_SelectedIndexChanged(null, null);
                }
                _idUser = Session["idUser"].ToString();
            }
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
                              ,[curp]
                          FROM [Cat_Emisor]
                          WHERE RFCEMI=@ruc";
            }
            if (tipo == "rec")
            {
                sql = @"SELECT [NOMREC]
                              ,CASE
									WHEN LOWER([pais]) LIKE 'méx%'
	                                    THEN 'Nacional'
                                    WHEN LOWER([pais]) LIKE 'mex%'
	                                    THEN 'Nacional'
									WHEN LOWER([pais]) = 'mx'
	                                    THEN 'Nacional'
                                    ELSE
	                                    'Extranjero'
                                END AS 'Nacionalidad'
                              ,[curp]
                          FROM [Cat_Receptor]
                          WHERE RFCREC=@ruc";
            }
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@ruc", rfc);
            var dr = _db.EjecutarConsulta();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (tipo == "emi")
                    {
                        tbRfcEmi.Text = rfc;
                        tbNomEmi.Text = dr["NOMEMI"].ToString();
                        tbCURPE.Text = dr["curp"].ToString();
                    }
                    if (tipo == "rec")
                    {
                        chkRecNacional.Checked = dr["Nacionalidad"].ToString().Equals("Nacional");
                        chkRecNacional_CheckedChanged(null, null);
                        tbRfcRec.Text = rfc;
                        tbNomRec.Text = dr["NOMREC"].ToString();
                        tbCURPR.Text = dr["curp"].ToString();
                    }
                }
            }
            else
            {
                if (tipo == "emi")
                {
                    tbNomEmi.Text = "";
                    tbCURPE.Text = "";
                }
                if (tipo == "rec")
                {
                    tbNomRec.Text = "";
                    tbCURPR.Text = "";
                }
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkRecNacional control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkRecNacional_CheckedChanged(object sender, EventArgs e)
        {
            tbCURPR.Text = "";
            tbNomRec.Text = "";
            tbRfcRec.Text = "";//chkRecNacional.Checked ? "" : "XEXX010101000";
            tbCURPR.ReadOnly = !chkRecNacional.Checked;
            lblRFC.Text = chkRecNacional.Checked ? "RFC:" : "ID. FISCAL";
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRfcRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRfcRec_TextChanged(object sender, EventArgs e)
        {
            Llenarlista(tbRfcRec.Text, "rec");
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
                var txt = new SpoolMx();
                txt.SetEmisorRet(tbRfcEmi.Text, tbNomEmi.Text, tbCURPE.Text);
                if (chkRecNacional.Checked)
                {
                    txt.SetReceptorNacRet(tbRfcRec.Text, tbNomRec.Text, tbCURPR.Text);
                }
                else
                {
                    txt.SetReceptorExtRet(tbRfcRec.Text, tbNomRec.Text);
                }
                txt.SetPeriodoRet(tbMesIni.Text, tbMesFin.Text, tbEjerc.Text);
                txt.SetTotalesRet(tbTotOperacion.Text, tbTotGrav.Text, tbTotExent.Text, tbTotRet.Text);
                var sql = "SELECT impuesto, base, importe, tipoPago FROM DAT_MX_ImpuestosRetencionesTemp WHERE (id_Empleado = @idUser)";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@idUser", _idUser);
                var dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    txt.AgregaImpuestoRet(dr["base"].ToString(), dr["impuesto"].ToString(), dr["importe"].ToString(), dr["tipoPago"].ToString());
                }
                _db.Desconectar();
                var fecha = Localization.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                var cve = ddlCveRet.SelectedValue;
                var desc = ddlCveRet.SelectedItem.Text;
                if (chkPagos.Checked)
                {
                    txt.SetPagosExtranjeros(chkBeneficiario.Checked ? "SI" : "NO");
                    if (chkBeneficiario.Checked)
                    {
                        txt.SetPagosExtranjerosBeneficiario(tbRFCB.Text, tbCURPB.Text, tbRazonB.Text, ddlConceptoPagoB.SelectedValue, tbDescConceptoB.Text);
                    }
                    if (chkNoBeneficiario.Checked)
                    {
                        txt.SetPagosExtranjerosNoBeneficiario(ddlPaisExt.SelectedValue, ddlConceptoPagoNB.SelectedValue, tbDescConceptoNB.Text);
                    }
                    cve = "18";
                    desc = "Pagos realizados a favor de residentes en el extranjero.";
                }
                else if (chkDividOutil.Checked)
                {
                    txt.SetDividendosRetencion(ddlTipoDivOUtil.SelectedValue, tbMontISRAcredRetMexico.Text, tbMontISRAcredRetExtranjero.Text, tbMontRetExtDivExt.Text, ddlTipoSocDistrDiv.SelectedItem.Text, tbMontISRAcredNal.Text, tbMontDivAcumNal.Text, tbMontDivAcumExt.Text, tbProporcionRem.Text);
                    cve = "14";
                    desc = "Dividendos o utilidades distribuidas.";
                }

                var folioR = 0;
                var sqlR = "select top 1 folio from Dat_General where serie=@serie and codDoc='07' ORDER BY folio DESC";
                _db.Conectar();
                _db.CrearComando(sqlR);
                _db.AsignarParametroCadena("@serie", ddlSerie.SelectedItem.Text);
                var drR = _db.EjecutarConsulta();
                if (drR.Read())
                {
                    folioR = Convert.ToInt32(drR["folio"].ToString()) + 1;
                }
                _db.Desconectar();


                txt.SetComprobanteRet(folioR.ToString(), fecha, cve, desc, ddlSerie.SelectedItem.Text);



                // txt.SetComprobanteRet("", fecha, cve, desc, ddlSerie.SelectedItem.Text);



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
                var txtInvoice = txt.ConstruyeTxtRetenciones();
                var randomMs = new Random().Next(1000, 5000);
                System.Threading.Thread.Sleep(randomMs);
                var coreMx = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, "07", false, true, "", "");
                if (result != null)
                {
                    var xDoc = new XmlDocument();
                    xDoc.LoadXml(result.OuterXml);
                    Session["uuidCreado"] = GetAtributte(xDoc, "UUID", "tfd:TimbreFiscalDigital");
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante se ha generado satisfactoriamente", 2, null);
                    Response.Redirect("~/Documentos.aspx", false);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se creó correctamente<br/>" + coreMx.ObtenerMensaje(), 4, null);
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se creó correctamente<br/>" + ex.Message, 4, null);
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
        /// Handles the TextChanged event of the tbRfcEmi control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRfcEmi_TextChanged(object sender, EventArgs e)
        {
            Llenarlista(tbRfcEmi.Text, "emi");
        }

        /// <summary>
        /// Handles the Click event of the bAgregarImpuesto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarImpuesto_Click(object sender, EventArgs e)
        {
            _db.Conectar();
            _db.CrearComando(@"insert into DAT_MX_ImpuestosRetencionesTemp
                           (impuesto,impuestoNombre" + (!string.IsNullOrEmpty(tbBase.Text) ? ",base" : "") + @",importe,tipoPago,id_Empleado)
                           values
                           (@impuesto,@impuestoNombre" + (!string.IsNullOrEmpty(tbBase.Text) ? ",@base" : "") + @",@importe,@tipoPago,@id_Empleado)");
            _db.AsignarParametroCadena("@impuesto", ddlImpuesto.SelectedValue);
            _db.AsignarParametroCadena("@impuestoNombre", ddlImpuesto.SelectedItem.Text);
            if (!string.IsNullOrEmpty(tbBase.Text))
            {
                _db.AsignarParametroCadena("@base", CerosNull(tbBase.Text));
            }
            _db.AsignarParametroCadena("@importe", CerosNull(tbImporte.Text));
            _db.AsignarParametroCadena("@tipoPago", ddlTipoPago.SelectedItem.Text);
            _db.AsignarParametroCadena("@id_Empleado", _idUser);
            _db.EjecutarConsulta1();
            _db.Desconectar();
            ddlImpuesto.SelectedIndex = 0;
            tbImporte.Text = "";
            tbBase.Text = "";
            SqlDataImpTemp.DataBind();
            gvImpuestos.DataBind();
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
        /// Handles the CheckedChanged event of the chkPagos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkPagos_CheckedChanged(object sender, EventArgs e)
        {
            chkBeneficiario.Checked = false;
            chkNoBeneficiario.Checked = false;
            chkBeneficiario.Enabled = chkPagos.Checked;
            chkNoBeneficiario.Enabled = chkPagos.Checked;
            chkNoBeneficiario_CheckedChanged(null, null);
            chkBeneficiario_CheckedChanged(null, null);
            if (chkPagos.Checked && chkDividOutil.Checked)
            {
                chkDividOutil.Checked = false;
                chkDividOutil_CheckedChanged(null, null);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkBeneficiario control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkBeneficiario_CheckedChanged(object sender, EventArgs e)
        {
            ddlConceptoPagoB.Enabled = chkBeneficiario.Checked;
            tbRFCB.ReadOnly = !chkBeneficiario.Checked;
            tbRazonB.ReadOnly = !chkBeneficiario.Checked;
            tbCURPB.ReadOnly = !chkBeneficiario.Checked;
            tbDescConceptoB.ReadOnly = !chkBeneficiario.Checked;
            tbRFCB.Text = "";
            tbRazonB.Text = "";
            tbCURPB.Text = "";
            tbDescConceptoB.Text = "";
            RequiredFieldValidator11.Enabled = chkBeneficiario.Checked;
            RequiredFieldValidator13.Enabled = chkBeneficiario.Checked;
            RequiredFieldValidator12.Enabled = chkBeneficiario.Checked;
            RequiredFieldValidator10.Enabled = chkBeneficiario.Checked;
            if (chkBeneficiario.Checked)
            {
                validationB.Attributes.Add("habilitado", "");
            }
            else
            {
                validationB.Attributes.Remove("habilitado");
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkNoBeneficiario control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkNoBeneficiario_CheckedChanged(object sender, EventArgs e)
        {
            ddlPaisExt.Enabled = chkNoBeneficiario.Checked;
            ddlConceptoPagoNB.Enabled = chkNoBeneficiario.Checked;
            tbDescConceptoNB.ReadOnly = !chkNoBeneficiario.Checked;
            tbDescConceptoNB.Text = "";
            RequiredFieldValidator14.Enabled = chkNoBeneficiario.Checked;
            if (chkNoBeneficiario.Checked)
            {
                validationNB.Attributes.Add("habilitado", "");
            }
            else
            {
                validationNB.Attributes.Remove("habilitado");
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkDividOutil control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkDividOutil_CheckedChanged(object sender, EventArgs e)
        {
            ddlTipoDivOUtil.Enabled = chkDividOutil.Checked;
            tbMontISRAcredRetMexico.ReadOnly = !chkDividOutil.Checked;
            tbMontISRAcredRetExtranjero.ReadOnly = !chkDividOutil.Checked;
            tbMontRetExtDivExt.ReadOnly = !chkDividOutil.Checked;
            ddlTipoSocDistrDiv.Enabled = chkDividOutil.Checked;
            tbMontISRAcredNal.ReadOnly = !chkDividOutil.Checked;
            tbMontDivAcumNal.ReadOnly = !chkDividOutil.Checked;
            tbMontDivAcumExt.ReadOnly = !chkDividOutil.Checked;
            tbProporcionRem.ReadOnly = !chkDividOutil.Checked;
            tbMontISRAcredRetMexico.Text = "";
            tbMontISRAcredRetExtranjero.Text = "";
            tbMontRetExtDivExt.Text = "";
            tbMontISRAcredNal.Text = "";
            tbMontDivAcumNal.Text = "";
            tbMontDivAcumExt.Text = "";
            tbProporcionRem.Text = "";
            RequiredFieldValidator15.Enabled = chkDividOutil.Checked;
            RequiredFieldValidator16.Enabled = chkDividOutil.Checked;
            if (chkDividOutil.Checked)
            {
                validationDivid.Attributes.Add("habilitado", "");
            }
            else
            {
                validationDivid.Attributes.Remove("habilitado");
            }
            if (chkPagos.Checked && chkDividOutil.Checked)
            {
                chkPagos.Checked = false;
                chkPagos_CheckedChanged(null, null);
            }
        }

        #region Variables

        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db = new BasesDatos("");
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log = new Log("");
        /// <summary>
        /// The _ruc emisor
        /// </summary>
        private string _rucEmisor = "";

        /// <summary>
        /// The _lista paises
        /// </summary>
        private readonly List<ListItem> _listaPaises = new List<ListItem> { new ListItem("AFGANISTAN (EMIRATO ISLAMICO DE)", "AF"), new ListItem("ALBANIA (REPUBLICA DE)", "AL"), new ListItem("ALEMANIA (REPUBLICA FEDERAL DE)", "DE"), new ListItem("ANDORRA (PRINCIPADO DE)", "AD"), new ListItem("ANGOLA (REPUBLICA DE)", "AO"), new ListItem("ANGUILA", "AI"), new ListItem("ANTARTIDA", "AQ"), new ListItem("ANTIGUA Y BARBUDA (COMUNIDAD BRITANICA DE NACIONES)", "AG"), new ListItem("ANTILLAS NEERLANDESAS (TERRITORIO HOLANDES DE ULTRAMAR)", "AN"), new ListItem("ARABIA SAUDITA (REINO DE)", "SA"), new ListItem("ARGELIA (REPUBLICA DEMOCRATICA Y POPULAR DE)", "DZ"), new ListItem("ARGENTINA (REPUBLICA)", "AR"), new ListItem("ARMENIA (REPUBLICA DE)", "AM"), new ListItem("ARUBA (TERRITORIO HOLANDES DE ULTRAMAR)", "AW"), new ListItem("AUSTRALIA (COMUNIDAD DE)", "AU"), new ListItem("AUSTRIA (REPUBLICA DE)", "AT"), new ListItem("AZERBAIJAN (REPUBLICA AZERBAIJANI)", "AZ"), new ListItem("BAHAMAS (COMUNIDAD DE LAS)", "BS"), new ListItem("BAHREIN (ESTADO DE)", "BH"), new ListItem("BANGLADESH (REPUBLICA POPULAR DE)", "BD"), new ListItem("BARBADOS (COMUNIDAD BRITANICA DE NACIONES)", "BB"), new ListItem("BELGICA (REINO DE)", "BE"), new ListItem("BELICE", "BZ"), new ListItem("BENIN (REPUBLICA DE)", "BJ"), new ListItem("BERMUDAS", "BM"), new ListItem("BIELORRUSIA (REPUBLICA DE)", "BY"), new ListItem("BOLIVIA (REPUBLICA DE)", "BO"), new ListItem("BOSNIA Y HERZEGOVINA", "BA"), new ListItem("BOTSWANA (REPUBLICA DE)", "BW"), new ListItem("BRASIL (REPUBLICA FEDERATIVA DE)", "BR"), new ListItem("BRUNEI (ESTADO DE) (RESIDENCIA DE PAZ)", "BN"), new ListItem("BULGARIA (REPUBLICA DE)", "BG"), new ListItem("BURKINA FASO", "BF"), new ListItem("BURUNDI (REPUBLICA DE)", "BI"), new ListItem("BUTAN (REINO DE)", "BT"), new ListItem("CABO VERDE (REPUBLICA DE)", "CV"), new ListItem("CHAD (REPUBLICA DEL)", "TD"), new ListItem("CAIMAN (ISLAS)", "KY"), new ListItem("CAMBOYA (REINO DE)", "KH"), new ListItem("CAMERUN (REPUBLICA DEL)", "CM"), new ListItem("CANADA", "CA"), new ListItem("CHILE (REPUBLICA DE)", "CL"), new ListItem("CHINA (REPUBLICA POPULAR)", "CN"), new ListItem("CHIPRE (REPUBLICA DE)", "CY"), new ListItem("CIUDAD DEL VATICANO (ESTADO DE LA)", "VA"), new ListItem("COCOS (KEELING, ISLAS AUSTRALIANAS)", "CC"), new ListItem("COLOMBIA (REPUBLICA DE)", "CO"), new ListItem("COMORAS (ISLAS)", "KM"), new ListItem("CONGO (REPUBLICA DEL)", "CG"), new ListItem("COOK (ISLAS)", "CK"), new ListItem("COREA (REPUBLICA POPULAR DEMOCRATICA DE) (COREA DEL NORTE)", "KP"), new ListItem("COREA (REPUBLICA DE) (COREA DEL SUR)", "KR"), new ListItem("COSTA DE MARFIL (REPUBLICA DE LA)", "CI"), new ListItem("COSTA RICA (REPUBLICA DE)", "CR"), new ListItem("CROACIA (REPUBLICA DE)", "HR"), new ListItem("CUBA (REPUBLICA DE)", "CU"), new ListItem("DINAMARCA (REINO DE)", "DK"), new ListItem("DJIBOUTI (REPUBLICA DE)", "DJ"), new ListItem("DOMINICA (COMUNIDAD DE)", "DM"), new ListItem("ECUADOR (REPUBLICA DEL)", "EC"), new ListItem("EGIPTO (REPUBLICA ARABE DE)", "EG"), new ListItem("EL SALVADOR (REPUBLICA DE)", "SV"), new ListItem("EMIRATOS ARABES UNIDOS", "AE"), new ListItem("ERITREA (ESTADO DE)", "ER"), new ListItem("ESLOVENIA(REPUBLICA DE)", "SI"), new ListItem("ESPAÑA (REINO DE)", "ES"), new ListItem("ESTADO FEDERADO DE MICRONESIA", "FM"), new ListItem("ESTADOS UNIDOS DE AMERICA", "US"), new ListItem("ESTONIA (REPUBLICA DE)", "EE"), new ListItem("ETIOPIA (REPUBLICA DEMOCRATICA FEDERAL)", "ET"), new ListItem("FIDJI (REPUBLICA DE)", "FJ"), new ListItem("FILIPINAS (REPUBLICA DE LAS)", "PH"), new ListItem("FINLANDIA (REPUBLICA DE)", "FI"), new ListItem("FRANCIA (REPUBLICA FRANCESA)", "FR"), new ListItem("GABONESA (REPUBLICA)", "GA"), new ListItem("GAMBIA (REPUBLICA DE LA)", "GM"), new ListItem("GEORGIA (REPUBLICA DE)", "GE"), new ListItem("GHANA (REPUBLICA DE)", "GH"), new ListItem("GIBRALTAR (R.U.)", "GI"), new ListItem("GRANADA", "GD"), new ListItem("GRECIA (REPUBLICA HELENICA)", "GR"), new ListItem("GROENLANDIA (DINAMARCA)", "GL"), new ListItem("GUADALUPE (DEPARTAMENTO DE)", "GP"), new ListItem("GUAM (E.U.A.)", "GU"), new ListItem("GUATEMALA (REPUBLICA DE)", "GT"), new ListItem("GUERNSEY", "GG"), new ListItem("GUINEA-BISSAU (REPUBLICA DE)", "GW"), new ListItem("GUINEA ECUATORIAL (REPUBLICA DE)", "GQ"), new ListItem("GUINEA (REPUBLICA DE)", "GN"), new ListItem("GUYANA FRANCESA", "GF"), new ListItem("GUYANA (REPUBLICA COOPERATIVA DE)", "GY"), new ListItem("HAITI (REPUBLICA DE)", "HT"), new ListItem("HONDURAS (REPUBLICA DE)", "HN"), new ListItem("HONG KONG (REGION ADMINISTRATIVA ESPECIAL DE LA REPUBLICA)", "HK"), new ListItem("HUNGRIA (REPUBLICA DE)", "HU"), new ListItem("INDIA (REPUBLICA DE)", "IN"), new ListItem("INDONESIA (REPUBLICA DE)", "ID"), new ListItem("IRAK (REPUBLICA DE)", "IQ"), new ListItem("IRAN (REPUBLICA ISLAMICA DEL)", "IR"), new ListItem("IRLANDA (REPUBLICA DE)", "IE"), new ListItem("ISLANDIA (REPUBLICA DE)", "IS"), new ListItem("ISLA BOUVET", "BV"), new ListItem("ISLA DE MAN", "IM"), new ListItem("ISLAS ALAND", "AX"), new ListItem("ISLAS FEROE", "FO"), new ListItem("ISLAS GEORGIA Y SANDWICH DEL SUR", "GS"), new ListItem("ISLAS HEARD Y MCDONALD", "HM"), new ListItem("ISLAS MALVINAS (R.U.)", "FK"), new ListItem("ISLAS MARIANAS SEPTENTRIONALES", "MP"), new ListItem("ISLAS MARSHALL", "MH"), new ListItem("ISLAS MENORES DE ULTRAMAR DE ESTADOS UNIDOS DE AMERICA", "UM"), new ListItem("ISLAS SALOMON (COMUNIDAD BRITANICA DE NACIONES)", "SB"), new ListItem("ISLAS SVALBARD Y JAN MAYEN (NORUEGA)", "SJ"), new ListItem("ISLAS TOKELAU", "TK"), new ListItem("ISLAS WALLIS Y FUTUNA", "WF"), new ListItem("ISRAEL (ESTADO DE)", "IL"), new ListItem("ITALIA (REPUBLICA ITALIANA)", "IT"), new ListItem("JAMAICA", "JM"), new ListItem("JAPON", "JP"), new ListItem("JERSEY", "JE"), new ListItem("JORDANIA (REINO HACHEMITA DE)", "JO"), new ListItem("KAZAKHSTAN (REPUBLICA DE) ", "KZ"), new ListItem("KENYA (REPUBLICA DE)", "KE"), new ListItem("KIRIBATI (REPUBLICA DE)", "KI"), new ListItem("KUWAIT (ESTADO DE)", "KW"), new ListItem("KYRGYZSTAN (REPUBLICA KIRGYZIA)", "KG"), new ListItem("LESOTHO (REINO DE)", "LS"), new ListItem("LETONIA (REPUBLICA DE)", "LV"), new ListItem("LIBANO (REPUBLICA DE)", "LB"), new ListItem("LIBERIA (REPUBLICA DE)", "LR"), new ListItem("LIBIA (JAMAHIRIYA LIBIA ARABE POPULAR SOCIALISTA)", "LY"), new ListItem("LIECHTENSTEIN (PRINCIPADO DE)", "LI"), new ListItem("LITUANIA (REPUBLICA DE)", "LT"), new ListItem("LUXEMBURGO (GRAN DUCADO DE)", "LU"), new ListItem("MACAO", "MO"), new ListItem("MACEDONIA (ANTIGUA REPUBLICA YUGOSLAVA DE)", "MK"), new ListItem("MADAGASCAR (REPUBLICA DE)", "MG"), new ListItem("MALASIA", "MY"), new ListItem("MALAWI (REPUBLICA DE)", "MW"), new ListItem("MALDIVAS (REPUBLICA DE)", "MV"), new ListItem("MALI (REPUBLICA DE)", "ML"), new ListItem("MALTA (REPUBLICA DE)", "MT"), new ListItem("MARRUECOS (REINO DE)", "MA"), new ListItem("MARTINICA (DEPARTAMENTO DE) (FRANCIA)", "MQ"), new ListItem("MAURICIO (REPUBLICA DE)", "MU"), new ListItem("MAURITANIA (REPUBLICA ISLAMICA DE)", "MR"), new ListItem("MAYOTTE", "YT"), new ListItem("MEXICO (ESTADOS UNIDOS MEXICANOS)", "MX"), new ListItem("MOLDAVIA (REPUBLICA DE)", "MD"), new ListItem("MONACO (PRINCIPADO DE)", "MC"), new ListItem("MONGOLIA", "MN"), new ListItem("MONSERRAT (ISLA)", "MS"), new ListItem("MONTENEGRO", "ME"), new ListItem("MOZAMBIQUE (REPUBLICA DE)", "MZ"), new ListItem("MYANMAR (UNION DE)", "MM"), new ListItem("NAMIBIA (REPUBLICA DE)", "NA"), new ListItem("NAURU", "NR"), new ListItem("NAVIDAD (CHRISTMAS) (ISLAS)", "CX"), new ListItem("NEPAL (REINO DE)", "NP"), new ListItem("NICARAGUA (REPUBLICA DE)", "NI"), new ListItem("NIGER (REPUBLICA DE)", "NE"), new ListItem("NIGERIA (REPUBLICA FEDERAL DE)", "NG"), new ListItem("NIVE (ISLA)", "NU"), new ListItem("NORFOLK (ISLA)", "NF"), new ListItem("NORUEGA (REINO DE)", "NO"), new ListItem("NUEVA CALEDONIA (TERRITORIO FRANCES DE ULTRAMAR)", "NC"), new ListItem("NUEVA ZELANDIA", "NZ"), new ListItem("OMAN (SULTANATO DE)", "OM"), new ListItem("PACIFICO, ISLAS DEL (ADMON. E.U.A.)", "PIK"), new ListItem("PAISES BAJOS (REINO DE LOS) (HOLANDA)", "NL"), new ListItem("PAKISTAN (REPUBLICA ISLAMICA DE)", "PK"), new ListItem("PALAU (REPUBLICA DE)", "PW"), new ListItem("PALESTINA", "PS"), new ListItem("PANAMA (REPUBLICA DE)", "PA"), new ListItem("PAPUA NUEVA GUINEA (ESTADO INDEPENDIENTE DE)", "PG"), new ListItem("PARAGUAY (REPUBLICA DEL)", "PY"), new ListItem("PERU (REPUBLICA DEL)", "PE"), new ListItem("PITCAIRNS (ISLAS DEPENDENCIA BRITANICA)", "PN"), new ListItem("POLINESIA FRANCESA", "PF"), new ListItem("POLONIA (REPUBLICA DE)", "PL"), new ListItem("PORTUGAL (REPUBLICA PORTUGUESA)", "PT"), new ListItem("PUERTO RICO (ESTADO LIBRE ASOCIADO DE LA COMUNIDAD DE)", "PR"), new ListItem("QATAR (ESTADO DE)", "QA"), new ListItem("REINO UNIDO DE LA GRAN BRETAÑA E IRLANDA DEL NORTE", "GB"), new ListItem("REPUBLICA CHECA", "CZ"), new ListItem("REPUBLICA CENTROAFRICANA", "CF"), new ListItem("REPUBLICA DEMOCRATICA POPULAR LAOS", "LA"), new ListItem("REPUBLICA DE SERBIA", "RS"), new ListItem("REPUBLICA DOMINICANA", "DO"), new ListItem("REPUBLICA ESLOVACA", "SK"), new ListItem("REPUBLICA POPULAR DEL CONGO", "CD"), new ListItem("REPUBLICA RUANDESA", "RW"), new ListItem("REUNION (DEPARTAMENTO DE LA) (FRANCIA)", "RE"), new ListItem("RUMANIA", "RO"), new ListItem("RUSIA (FEDERACION RUSA)", "RU"), new ListItem("SAHARA OCCIDENTAL (REPUBLICA ARABE SAHARAVI DEMOCRATICA)", "EH"), new ListItem("SAMOA (ESTADO INDEPENDIENTE DE)", "WS"), new ListItem("SAMOA AMERICANA", "AS"), new ListItem("SAN BARTOLOME", "BL"), new ListItem("SAN CRISTOBAL Y NIEVES (FEDERACION DE) (SAN KITTS - NEVIS)", "KN"), new ListItem("SAN MARINO (SERENISIMA REPUBLICA DE)", "SM"), new ListItem("SAN MARTIN", "MF"), new ListItem("SAN PEDRO Y MIQUELON", "PM"), new ListItem("SAN VICENTE Y LAS GRANADINAS", "VC"), new ListItem("SANTA ELENA", "SH"), new ListItem("SANTA LUCIA", "LC"), new ListItem("SANTO TOME Y PRINCIPE (REPUBLICA DEMOCRATICA DE)", "ST"), new ListItem("SENEGAL (REPUBLICA DEL)", "SN"), new ListItem("SEYCHELLES (REPUBLICA DE LAS)", "SC"), new ListItem("SIERRA LEONA (REPUBLICA DE)", "SL"), new ListItem("SINGAPUR (REPUBLICA DE)", "SG"), new ListItem("SIRIA (REPUBLICA ARABE)", "SY"), new ListItem("SOMALIA", "SO"), new ListItem("SRI LANKA (REPUBLICA DEMOCRATICA SOCIALISTA DE)", "LK"), new ListItem("SUDAFRICA (REPUBLICA DE)", "ZA"), new ListItem("SUDAN (REPUBLICA DEL)", "SD"), new ListItem("SUECIA (REINO DE)", "SE"), new ListItem("SUIZA (CONFEDERACION)", "CH"), new ListItem("SURINAME (REPUBLICA DE)", "SR"), new ListItem("SWAZILANDIA (REINO DE)", "SZ"), new ListItem("TADJIKISTAN (REPUBLICA DE)", "TJ"), new ListItem("TAILANDIA (REINO DE)", "TH"), new ListItem("TAIWAN (REPUBLICA DE CHINA)", "TW"), new ListItem("TANZANIA (REPUBLICA UNIDA DE)", "TZ"), new ListItem("TERRITORIOS BRITANICOS DEL OCEANO INDICO", "IO"), new ListItem("TERRITORIOS FRANCESES, AUSTRALES Y ANTARTICOS", "TF"), new ListItem("TIMOR ORIENTAL", "TL"), new ListItem("TOGO (REPUBLICA TOGOLESA)", "TG"), new ListItem("TONGA (REINO DE)", "TO"), new ListItem("TRINIDAD Y TOBAGO (REPUBLICA DE)", "TT"), new ListItem("TUNEZ (REPUBLICA DE)", "TN"), new ListItem("TURCAS Y CAICOS(ISLAS)", "TC"), new ListItem("TURKMENISTAN (REPUBLICA DE)", "TM"), new ListItem("TURQUIA (REPUBLICA DE)", "TR"), new ListItem("TUVALU (COMUNIDAD BRITANICA DE NACIONES)", "TV"), new ListItem("UCRANIA", "UA"), new ListItem("UGANDA (REPUBLICA DE)", "UG"), new ListItem("URUGUAY (REPUBLICA ORIENTAL DEL)", "UY"), new ListItem("UZBEJISTAN (REPUBLICA DE)", "UZ"), new ListItem("VANUATU", "VU"), new ListItem("VENEZUELA (REPUBLICA DE)", "VE"), new ListItem("VIETNAM (REPUBLICA SOCIALISTA DE)", "VN"), new ListItem("VIRGENES. ISLAS (BRITANICAS)", "VG"), new ListItem("VIRGENES. ISLAS (NORTEAMERICANAS)", "VI"), new ListItem("YEMEN (REPUBLICA DE)", "YE"), new ListItem("ZAMBIA (REPUBLICA DE)", "ZM") };

        #endregion

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSerie control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfambiente.Value = ddlSerie.SelectedValue;
            var sql = "SELECT c.codigo, c.descripcion AS ambiente FROM Cat_Catalogo1_C c INNER JOIN Cat_Series s ON s.ambiente = c.codigo WHERE c.tipo = 'Ambiente' AND s.idSerie = @idSerie";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idSerie", ddlSerie.SelectedValue);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                tbAmbiente.Text = dr["ambiente"].ToString();
                hfambiente.Value = dr["codigo"].ToString();
            }
            _db.Desconectar();
        }
    }
}