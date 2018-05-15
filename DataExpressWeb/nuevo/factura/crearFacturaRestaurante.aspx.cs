// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="crearFacturaRestaurante.aspx.cs" company="DataExpress Latinoamérica">
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
using System.Xml;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections;
using System.Data;

namespace DataExpressWeb
{
    /// <summary>
    /// Class CrearFacturaRestauranteMx.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CrearFacturaRestauranteMx : Page
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
                SqlDataPtoEmision.ConnectionString = _db.CadenaConexion;
                SqlDataSucursal.ConnectionString = _db.CadenaConexion;
                SqlDataSeries.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.ConnectionString = _db.CadenaConexion;
                SqlDataReceptores.ConnectionString = _db.CadenaConexion;
                SqlDataContactos.ConnectionString = _db.CadenaConexion;
                SqlDataSource2.ConnectionString = _db.CadenaConexion;
                SqlDataSourceUsoCfdi.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.SelectCommand = SqlDataMetodoPago.SelectCommand.Replace("@tipoCatalogo", Session["CfdiVersion"].ToString().Equals("3.3") ? "MetodoPagoCfdi33" : "MetodoPago");

                #region Asignar Sesion en DataSources Temporales

                SqlDataContactos.SelectParameters["SessionId"].DefaultValue = Session.SessionID;

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
                        Llenarlista(Session["rfcSucursal"].ToString(), "emi");
                        ddlSerie.DataBind();
                        ddlSerie.SelectedIndex = 0;
                        ddlSerie_SelectedIndexChanged(null, null);

                        #region Giro Empresarial

                        if (Session["IDGIRO"] != null)
                        {
                            if (Session["IDGIRO"].ToString().Contains("1") || Session["IDGIRO"].ToString().Contains("2"))
                            {
                                tbPropina.ReadOnly = false;
                                tbFormaPago.Text = "PAGO EN UNA SOLA EXHIBICIÓN";
                                tbFormaPago.ReadOnly = true;
                                btnFormaPago.Disabled = true;
                                rowDenomSocial.Visible = false;
                            }
                            else if (Session["IDGIRO"].ToString().Contains("3"))
                            {
                                Response.Redirect("~/nuevo/factura/crearFactura.aspx");
                            }
                        }

                        #endregion
                        CustomValidator1.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                        divPrevisualizar.Style["display"] = Session["IDGIRO"].ToString().Contains("1") && (Session["IDENTEMI"].ToString().Equals("LAN7008173R5") || Session["IDENTEMI"].ToString().Equals("SET920324FC3")) ? "inline" : "none";
                        divTbFormaPago.Style["display"] = !Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                        ddlFormaPago.Visible = Session["CfdiVersion"].ToString().Equals("3.3");
                        DivUsoCfdi.Style["display"] = Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                        ddlUsoCfdi.Enabled = Session["CfdiVersion"].ToString().Equals("3.3");
                        RequiredFieldValidator_UsoCfdi.Enabled = Session["CfdiVersion"].ToString().Equals("3.3");
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
                        //ddlMetodoPago_SelectedIndexChanged(null, null);
                    }
                }
            }
            else
            {
                Response.Redirect("~/Cerrar.aspx");
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

        protected void chkRepetidos_CheckedChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFormaPago.Text = ddlFormaPago.SelectedItem.Text;
            if (ddlFormaPago.SelectedValue.Equals("PPD"))
            {
                ddlMetodoPago.SelectedValue = "99";
                ddlMetodoPago.Enabled = false;
                if (Regex.IsMatch(Session["IDENTEMI"].ToString(), @"HSC0010193M7"))
                {
                    ddlMetodoPago.Enabled = true;
                }
            }
            else
            {
                ddlMetodoPago.Enabled = true;
            }
        }

        /// <summary>
        /// Builds the text.
        /// </summary>
        /// <returns>System.String.</returns>
        private string BuildTxt()
        {
            var selected = new List<string>();
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                selected.Add(ddlMetodoPago.SelectedValue);
            }
            else
            {
                selected = ddlMetodoPago.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();
            }
            if (selected.Count <= 0)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe escoger al menos un método de pago", 4, null);
                return null;
            }
            DbDataReader dr;
            var txt = new SpoolMx();
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
            txt.SetCantidadImpuestosCfdi("", CerosNull(tbIva16.Text), CerosNull(tbIva16.Text));

            var tarifa = "16.00";
            if (Session["CfdiVersion"] != null && Session["CfdiVersion"].ToString().Equals("3.3"))
            { tarifa = "0.16"; }
            txt.AgregaImpuestoTrasladoCfdi("IVA", tarifa, CerosNull(tbIva16.Text));

            txt.AgregaConceptoCfdi("1", "Unidad de servicio", "", "Establecimientos para comer y beber", CerosNull(tbSubtotal.Text), CerosNull(tbSubtotal.Text), CerosNull(tbDescuento.Text), "90101500", "E48", "1");
            txt.AgregaConceptoImpuestoCfdi(false, CerosNull(tbSubtotal.Text), "IVA", "Tasa", tarifa, CerosNull(tbIva16.Text), "1");
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
            var tipocomp = "ingreso";
            //tbFormaPago.Text = trParcialidad.Visible ? tbFormaPago.Text : ddlFormaPago.SelectedItem.Text;
            txt.SetComprobanteCfdi(ddlSerie.SelectedItem.Text, "", Localization.Now.ToString("s"), tbFormaPago.Text, tbCondPago.Text, tbSubtotal.Text, tbDescuento.Text, tbMotivoDescuento.Text, tbTipoCambio.Text, ddlMoneda.SelectedValue, tbTotal.Text, tipocomp, string.Join(",", selected), tbLugarExp.Text, tbNumCtaPago.Text, "", "", "", "", "", "", CerosNull(tbOtrosCargos.Text), CerosNull(tbGranTotal.Text), tbObservaciones.Text, "", "", "", "", false);
            txt.SetInfoAdicionalRestauranteCfdi(CerosNull(tbPropina.Text));
            var txtInvoice = txt.ConstruyeTxtCfdi();
            return txtInvoice;
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
                var coreMx = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, "01", false, true, "", "");
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
                        ddlMetodoPago.ClearSelection();
                        foreach (var pago in metodosPago)
                        {
                            try { ddlMetodoPago.Items.FindByValue(pago).Selected = true; } catch { }
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
            decimal propina = 0;
            decimal otrosCargos = 0;
            decimal.TryParse(CerosNull(tbSubtotal.Text), out subTotal);
            decimal.TryParse(CerosNull(tbDescuentoP.Text), out descuento);
            decimal.TryParse(CerosNull(tbPropina.Text), out propina);
            decimal.TryParse(CerosNull(tbOtrosCargos.Text), out otrosCargos);
            iva16 = (16 * subTotal) / 100;
            /* DESGLOCE DE IVA */
            bool bDesgloce = false;
            var desgloce = ConfigurationManager.AppSettings.Get("DESGALOSARIVA");
            bool.TryParse(desgloce, out bDesgloce);
            if (bDesgloce)
            {
                subTotal = subTotal - iva16;
            }
            /* DESGLOCE DE IVA */
            //descuento = (subTotal * descuento) / 100;
            total = subTotal - descuento + iva16/* + propina*/;
            decimal totalPago = total + propina + otrosCargos;
            tbIva16.Text = CerosNull(iva16.ToString());
            tbDescuento.Text = CerosNull(descuento.ToString());
            tbSubtotal.Text = CerosNull(subTotal.ToString());
            tbTotal.Text = CerosNull(total.ToString());
            tbGranTotal.Text = CerosNull(totalPago.ToString());
            tbCantLetra.Text = numLetra.ConvertirALetras(tbGranTotal.Text, ddlMoneda.SelectedValue);
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
            tbMunicipioExp.Text = cbUsarDirecEmisor.Checked ? tbMunicipioEmi.Text : tbMunicipioExp.Text;
            tbEstadoExp.Text = cbUsarDirecEmisor.Checked ? tbEstadoEmi.Text : tbEstadoExp.Text;
            tbPaisExp.Text = cbUsarDirecEmisor.Checked ? tbPaisEmi.Text : tbPaisExp.Text;
            tbCpExp.Text = cbUsarDirecEmisor.Checked ? tbCpEmi.Text : tbCpExp.Text;
            tbCalleExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbNoExtExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbNoIntExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
            tbColoniaExp.ReadOnly = (!cbEmiExp.Checked && !cbUsarDirecEmisor.Checked) || (cbEmiExp.Checked && cbUsarDirecEmisor.Checked);
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
                tbCalleRec.ReadOnly = control;
                tbNoExtRec.ReadOnly = control;
                tbNoIntRec.ReadOnly = control;
                tbColoniaRec.ReadOnly = control;
                tbMunicipioRec.ReadOnly = control;
                tbEstadoRec.ReadOnly = control;
                tbPaisRec.ReadOnly = control;
                if (control)
                {
                    btnPaisRec.Attributes["disabled"] = "disabled";
                }
                else
                {
                    btnPaisRec.Attributes.Remove("disabled");
                }
                tbCpRec.ReadOnly = control;
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
                              ,(CASE ISNULL(CONVERT(VARCHAR, metodoPago), '') WHEN '' THEN '99' ELSE ISNULL(cc.codigo, '99') END) AS metodoPago
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
            }
            else if (tipo == "rec")
            {
                var control = dr.HasRows && !rfc.Equals("XAXX010101000", StringComparison.OrdinalIgnoreCase) && !rfc.Equals("XEXX010101000", StringComparison.OrdinalIgnoreCase);
                tbRazonSocialRec.ReadOnly = control;
                tbCURPR.ReadOnly = control;
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
                        ddlMetodoPago.ClearSelection();
                        foreach (var pago in metodosPago)
                        {
                            //try { ddlMetodoPago.Items.FindByValue(pago).Selected = true; } catch { }
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
                }
                if (tipo == "rec")
                {
                    _idReceptor = "";
                    tbRazonSocialRec.Text = "";
                    tbCURPR.Text = "";
                    tbMailRec.Text = "";
                    tbTelRec.Text = "";
                    ddlMetodoPago.ClearSelection();
                    tbNumCtaPago.Text = "NO IDENTIFICADO";
                    tbDenomSocialRec.Text = "";
                    tbTelRec2.Text = "";
                }
            }
            _db.Desconectar();
            Llenarlistadom(rfc, tipo);
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
            RequiredFieldValidator27.Enabled = cbDomRec.Checked;
            ddlSucRec.Items.Clear();
            var itemNull = new ListItem("SELECCIONE", "0");
            itemNull.Selected = true;
            ddlSucRec.Items.Add(itemNull);
            Llenarlistadom(cbDomRec.Checked ? tbRfcRec.Text : "", "rec", false);
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

        /// <summary>
        /// Handles the PageIndexChanging event of the gvRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRec.PageIndex = e.NewPageIndex;
            gvRec.DataBind();
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

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlMoneda control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, ddlMoneda.SelectedValue);
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
    }
}