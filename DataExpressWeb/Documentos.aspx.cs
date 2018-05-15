// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="Documentos.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using Control;
using Datos;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Documentos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Documentos : System.Web.UI.Page
    {
        /// <summary>
        /// The _chkbox select all
        /// </summary>
        private string[] Nrofacuras;
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
        private string _todasFacturas;
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
        /// The _uuid highlight
        /// </summary>
        private static string _uuidHighlight = "";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos(Session["IDENTEMI"]?.ToString() ?? "CORE");
                _log = new Log(Session["IDENTEMI"]?.ToString() ?? "CORE");
                SqlDataSource1.ConnectionString = _db.CadenaConexion;
                SqlDataSource2.ConnectionString = _db.CadenaConexion;
                SqlDataSourcePtoEmi.ConnectionString = _db.CadenaConexion;
                SqlDataSourcePtoEmiCliente.ConnectionString = _db.CadenaConexion;
                SqlDataSourceTipoDoc.ConnectionString = _db.CadenaConexion;
                SqlDataSource3.ConnectionString = _db.CadenaConexion;
                //SqlDataSeries.ConnectionString = _db.CadenaConexion;
                SqlDataSeriesNotaC.ConnectionString = _db.CadenaConexion;
                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                }
                //K
                if (Session["IDGIRO"].ToString().Equals("3") || Session["IDGIRO"].ToString().Equals("4"))
                {
                    lblTicketFiltro.Visible = false;
                    tbControl.Visible = false;
                }

                _db.Conectar();
                _db.CrearComando(@"SELECT RFCREC FROM Cat_Receptor WHERE RFCREC = @RFCREC");
                _db.AsignarParametroCadena("@RFCREC", "YAN810728RW7");
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    divRFCemi.Style["display"] = "inline";
                    ((DataControlField)gvFacturas.Columns.Cast<DataControlField>().Where(fld => fld.SortExpression == "NOMEMI").SingleOrDefault()).Visible = true;
                    ((DataControlField)gvFacturas.Columns.Cast<DataControlField>().Where(fld => fld.SortExpression == "NOMREC").SingleOrDefault()).Visible = false;
                    bExcel.Visible = true;
                }
                else
                {
                    divtbNombre.Style["width"] = "77%";
                }
                _db.Desconectar();
            }

            var lista = SiteMaster.ListaImpresoras();
            ddlImpresoras.Items.Clear();
            foreach (var printerArray in lista)
            {
                ddlImpresoras.Items.Add(printerArray[1]);
            }

            if (!IsPostBack)
            {
                try
                {
                    _uuidHighlight = Session["uuidCreado"].ToString();
                }
                catch { }
                try
                {
                    (Master as SiteMaster).FindControl("lblTimbresRestantes").Visible = Session["IsCliente"] == null;
                }
                catch { }
                finally { Session["uuidCreado"] = null; }
                Session["gvFacturas"] = null;
                _dtActual = new DataTable();

                if (Convert.ToBoolean(Session["coFactPropias"]) || Convert.ToBoolean(Session["coFactTodas"]))
                {
                    if (Convert.ToBoolean(Session["EnvioEmail"]) || Session["IsCliente"] != null) { tbEmail.Visible = true; bMail.Visible = true; } else { tbEmail.Visible = false; bMail.Visible = false; }
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
                    lCount.Text = "<strong>Se encontraron</strong> <span class='badge'>" + _dtActual.Rows.Count + "</span> <strong>Registros </strong>";
                }
                //SqlDataSeries.DataBind();
                //ddlSerie_SelectedIndexChanged(null, null);
                ddlSerie.DataBind();
                ddlSerie.SelectedIndex = 0;
                ddlSerie_SelectedIndexChanged(null, null);

                ScriptManager.RegisterStartupScript(this, GetType(), "_showDocumentosActive", "resaltar('liEmision');", true);
                if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90"))
                {
                    lblTicketFiltro.Text = "DSID/OrderNumber:";
                    chkHistorico.Visible = true;
                }
            }

            //dropdownMenu1.Visible = Session["IsCliente"] == null;
            bEventos.Visible = Session["IsCliente"] == null;
            if (Session["IsCliente"] != null)
            {
                rowFiltroRecep.Style["display"] = "none";
            }
            //dropdownMenu1.Visible = Session["CRnewComp"].ToString().Contains("04");
            //liCrearNotaC.Visible = Session["CRnewComp"].ToString().Contains("04");

            ClientScript.RegisterStartupScript(GetType(), "BG", "setBG('" + gvFacturas.ClientID + "')", true);
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
                if (_consulta.Length != 0)
                {
                    _consulta = _consulta + "FA" + tbFolioAnterior.Text + _separador;
                }
                else
                {
                    _consulta = "FA" + tbFolioAnterior.Text + _separador;
                }
            }
            if (tbNombre.Text.Length != 0)
            {
                if (_consulta.Length != 0)
                {
                    _consulta = _consulta + "RS" + tbNombre.Text + _separador;
                }
                else
                {
                    _consulta = "RS" + tbNombre.Text + _separador;
                }
            }
            if (tbControl.Text.Length != 0)
            {
                if (_consulta.Length != 0)
                {
                    _consulta = _consulta + "CF" + tbControl.Text + _separador;
                }
                else
                {
                    _consulta = "CF" + tbControl.Text + _separador;
                }
            }

            if (tbRFC.Text.Length != 0)
            {
                if (_consulta.Length != 0)
                {
                    _consulta = _consulta + "RF" + tbRFC.Text + _separador;
                }
                else
                {
                    _consulta = "RF" + tbRFC.Text + _separador;
                }
            }

            if (ddlTipoDocumento.SelectedIndex != 0)
            {
                if (_consulta.Length != 0)
                {
                    _consulta = _consulta + "TD" + ddlTipoDocumento.SelectedValue + _separador;
                }
                else
                {
                    _consulta = "TD" + ddlTipoDocumento.SelectedValue + _separador;
                }
            }
            if (ddlEstado.SelectedIndex != 0)
            {
                if (_consulta.Length != 0)
                {
                    _consulta = _consulta + "ED" + ddlEstado.SelectedValue + _separador;
                }
                else
                {
                    _consulta = "ED" + ddlEstado.SelectedValue + _separador;
                }
            }
            if (!ddlPtoEmi.SelectedValue.Equals("0"))
            {
                if (_consulta.Length != 0)
                {
                    _consulta = _consulta + "PO" + ddlPtoEmi.SelectedItem.Text + _separador;
                }
                else
                {
                    _consulta = "PO" + ddlPtoEmi.SelectedItem.Text + _separador;
                }
            }
            if (tbFechaInicial.Text.Length > 5 && tbFechaFinal.Text.Length > 5)
            {
                if (tbFechaInicial.Text.Length > 5)
                {
                    if (_consulta.Length != 0)
                    {
                        _consulta = _consulta + "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + _separador;
                    }
                    else
                    {
                        _consulta = "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + _separador;
                    }
                    fecha = 1;
                }

                if (tbFechaFinal.Text.Length > 5)
                {
                    if (_consulta.Length != 0)
                    {
                        _consulta = _consulta + "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + _separador;
                    }
                    else
                    {
                        _consulta = "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + _separador;
                    }
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
            tbEmail.Text = "";
            tbFolioAnterior.Attributes["Text"] = "";
            tbFolioAnterior.Text = "";
            ddlTipoDocumento.SelectedValue = "0";
            //ddlSucursal.Text = "Selecciona el Establecimiento";
            tbFechaInicial.Text = "";
            tbFechaFinal.Text = "";
            ddlEstado.SelectedValue = "0";
            tbRFC.Text = "";
            tbNombre.Text = "";

            Buscar();
            //Response.Redirect("Documentos.aspx", false);
            Response.Redirect("~/Documentos.aspx", false);
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
        /// Handles the Click2 event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click2(object sender, EventArgs e)
        {
            _db.Conectar();
            _db.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio from Par_ParametrosSistema");
            var dr1 = _db.EjecutarConsulta();

            while (dr1.Read())
            {
                _servidor = dr1[0].ToString();
                _puerto = Convert.ToInt32(dr1[1].ToString());
                _ssl = Convert.ToBoolean(dr1[2].ToString());
                _emailCredencial = dr1[3].ToString();
                _passCredencial = dr1[4].ToString();
                _rutaDoc = dr1[5].ToString();
                _emailEnviar = dr1[6].ToString();
            }
            _db.Desconectar();

            var mensaje = "";
            //string temp = "";
            var bChk = false;

            //List<string>
            _consulta = "";
            _cantidad = gvFacturas.Rows.Count;
            var estadoEnvio = true;
            var mailsFallidos = "<ul>";
            foreach (GridViewRow row in gvFacturas.Rows)
            {
                var chkSeleccionar = (CheckBox)row.FindControl("check");
                var hdSeleccionaPdf = (HiddenField)row.FindControl("checkHdPDF");
                var hdSeleccionarXml = (HiddenField)row.FindControl("checkHdXML");
                var hdTipo = (HiddenField)row.FindControl("checkTipo");
                //var lLabel2 = (Label)row.FindControl("Label2"); //RFCREC
                //var lLabel5 = (Label)row.FindControl("Label5"); //FOLFAC
                //var lLabel1 = (Label)row.FindControl("Label1"); //FECHA
                var rfcrec = "";
                var folfac = "";
                var fecha = "";
                var uuid = "";
                var serie = "";
                var codDoc = "";
                var fechaAutorizacion = "";
                var hdId = (HiddenField)row.FindControl("checkHdID"); //ID
                if (!chkSeleccionar.Checked)
                {
                    continue;
                }
                _db.Conectar();
                _db.CrearComando(@"SELECT numeroAutorizacion AS UUID, folio, fecha, fechaAutorizacion, serie, codDoc, RFCREC FROM Dat_General INNER JOIN Cat_Receptor ON id_Receptor = IDEREC where  idComprobante=@idefac");
                _db.AsignarParametroCadena("@idefac", hdId.Value);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    uuid = dr["UUID"].ToString();
                    folfac = dr["folio"].ToString();
                    fecha = dr["fecha"].ToString();
                    serie = dr["serie"].ToString();
                    codDoc = dr["codDoc"].ToString();
                    rfcrec = dr["RFCREC"].ToString();
                    fechaAutorizacion = dr["fechaAutorizacion"].ToString();
                }
                _db.Desconectar();
                bChk = true;
                var emails = "";
                if (chkReglas.Checked)
                {
                    _db.Conectar();
                    _db.CrearComando("select emailsRegla from Cat_EmailsReglas  where Receptor=@rfcrec and estadoRegla=1");
                    _db.AsignarParametroCadena("@rfcrec", rfcrec);
                    var dr3 = _db.EjecutarConsulta();
                    if (dr3.Read())
                    {
                        emails = dr3[0].ToString();
                    }
                    _db.Desconectar();
                }
                emails = tbEmail.Text + "," + emails;
                emails = emails.Trim();
                emails = emails.Trim(',');

                if (!string.IsNullOrEmpty(emails))
                {
                    _em = new SendMail();
                    _em.ServidorSmtp(_servidor, _puerto, _ssl, _emailCredencial, _passCredencial);
                    if (checkPDF.Checked)
                    {
                        var fileName = _rutaDoc + hdSeleccionaPdf.Value.ToString().Replace("docus/" + (Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "") + "/", "").Replace("docus/", "");
                        if (File.Exists(fileName))
                        {
                            _em.Adjuntar(fileName);
                        }
                        else
                        {
                            var ws = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                            var respuesta = ws.GenerarPdf(Session["IDENTEMI"].ToString(), hdId.Value, "");
                            if (respuesta != null)
                            {
                                fileName = Path.GetFileNameWithoutExtension(hdSeleccionarXml.Value.ToString().Replace("docus/" + (Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "") + "/", "").Replace("docus/", "")) + ".pdf";
                                _em.Adjuntar(respuesta, fileName);
                            }
                        }
                    }
                    if (checkXML.Checked)
                    {
                        var fileName = _rutaDoc + hdSeleccionarXml.Value.ToString().Replace(@"docus\" + (Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "") + "/", "").Replace(@"docus\", "");
                        if (File.Exists(fileName))
                        {
                            _em.Adjuntar(fileName);
                        }
                    }
                    var emir = "";
                    _db.Conectar();
                    _db.CrearComando(@"SELECT nombre,mensaje FROM Cat_Mensajes where nombre='MensajePortalWebEmision' ");
                    var drSum = _db.EjecutarConsulta();
                    if (drSum.Read())
                    {
                        mensaje = drSum["mensaje"].ToString();
                    }
                    _db.Desconectar();
                    _db.Conectar();
                    _db.CrearComando(@"SELECT NOMEMI FROM Cat_Emisor WHERE RFCEMI = @RFC");
                    _db.AsignarParametroCadena("@RFC", Session["IDENTEMI"].ToString());
                    var dRemi = _db.EjecutarConsulta();
                    if (dRemi.Read())
                    {
                        emir = dRemi[0].ToString();
                    }
                    _db.Desconectar();

                    var tipoDoc = "Desconocido";
                    switch (codDoc)
                    {
                        case "01": tipoDoc = "Factura"; break;
                        case "04": tipoDoc = "Nota de Crédito"; break;
                        case "06": tipoDoc = "Carta Porte"; break;
                        case "07": tipoDoc = "Retención"; break;
                        case "08": tipoDoc = "Nómina"; break;
                        case "09": tipoDoc = "Contabilidad"; break;
                        default: break;
                    }

                    var asunto = "Documento electrónico No: " + folfac + " de " + emir + "";
                    //Session["IDENTEMI"].ToString()
                    if (Session["IDENTEMI"].Equals("OHR980618BLA"))
                    {
                        asunto = "Documento electrónico No:" + folfac + " de " + "HOLIDAY INN CIUDAD DE MEXICO TRADE CENTER" + "";
                    }

                    _em.LlenarEmail(_emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                    try
                    {
                        _em.ReemplazarVariable("@TipoDocumento", tipoDoc);
                        _em.ReemplazarVariable("@Serie", serie);
                        _em.ReemplazarVariable("@Folio", folfac);
                        _em.ReemplazarVariable("@NumeroAutorizacion", uuid);
                        _em.ReemplazarVariable("@FechaEmision", fecha);
                        _em.ReemplazarVariable("@FechaAutorizacion", fechaAutorizacion);
                        _em.EnviarEmail();
                        var sql = @"INSERT INTO Cat_emailEnvio
                                           (emailEnviado
                                           ,fechaEnvio
                                           ,id_General)
                                     VALUES
                                           (@emailEnviado
                                           ,@fechaEnvio
                                           ,@id_General)";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@emailEnviado", emails.Trim(','));
                        _db.AsignarParametroCadena("@fechaEnvio", Localization.Now.ToString("s"));
                        _db.AsignarParametroCadena("@id_General", hdId.Value);
                        _db.EjecutarConsulta1();
                        _db.Desconectar();
                        estadoEnvio &= true;
                    }
                    catch (Exception ex)
                    {
                        var metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        RegLog(ex.Message, metodo, ex.StackTrace);
                        //lbMsgZip.Text = ex.Message;
                        estadoEnvio &= false;
                        mailsFallidos += "<li>" + folfac + ": " + ex.Message + "</li>";
                        //mostrarAlerta(ex.Message, "Error");
                    }
                }
                else
                {
                    //lbMsgZip.Text = "Tienes seleccionar algún E-Mail";
                    (Master as SiteMaster).MostrarAlerta(this, "Tienes que especificar algún E-Mail", 4, null);
                    break;
                }
            }
            mailsFallidos += "</ul>";
            if (bChk)
            {
                (Master as SiteMaster).MostrarAlerta(this, (estadoEnvio ? "Se enviaron todos los Emails" : "No se pudieron enviar los siguientes comprobantes:" + Environment.NewLine + Environment.NewLine + mailsFallidos), estadoEnvio ? 2 : 4, null);
            }
            else
            {
                //lbMsgZip.Text = "Debes seleccionar una factura";
                (Master as SiteMaster).MostrarAlerta(this, "Debes seleccionar una factura", 4, null);
            }
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
        /// Handles the Click event of the btnZip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnZip_Click(object sender, EventArgs e)
        {
            _cantidad = RowsChecked();
            _seleccionados = new string[_cantidad];
            var bChk = false;
            if (_cantidad <= 40)
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (GridViewRow row in gvFacturas.Rows)
                    {
                        var chkSeleccionar = (CheckBox)row.FindControl("check");
                        var hdSeleccionaPdf = (HiddenField)row.FindControl("checkHdPDF");
                        var hdSeleccionarXml = (HiddenField)row.FindControl("checkHdXML");
                        var hdId = (HiddenField)row.FindControl("checkHdID");
                        var hdTipo = (HiddenField)row.FindControl("checkTipo");
                        if (chkSeleccionar.Checked)
                        {
                            bChk = true;
                            var rutaPdf = System.AppDomain.CurrentDomain.BaseDirectory + hdSeleccionaPdf.Value;
                            var rutaXml = System.AppDomain.CurrentDomain.BaseDirectory + hdSeleccionarXml.Value;
                            if (checkPDF.Checked)
                            {
                                if (File.Exists(rutaPdf))
                                {
                                    try
                                    {
                                        zip.AddItem(System.AppDomain.CurrentDomain.BaseDirectory + hdSeleccionaPdf.Value, "");
                                    }
                                    catch { }
                                }
                                else
                                {
                                    var ws = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                                    var respuesta = ws.GenerarPdf(Session["IDENTEMI"].ToString(), hdId.Value, "");
                                    if (respuesta != null)
                                    {
                                        var fileName = Path.GetFileNameWithoutExtension(rutaXml) + ".pdf";
                                        try
                                        {
                                            zip.AddEntry(fileName, respuesta);
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        var msg = ws.ObtenerMensaje();
                                        var msgt = ws.ObtenerMensajeTecnico();
                                    }
                                }
                            }
                            if (checkXML.Checked && File.Exists(rutaXml))
                            {
                                try
                                {
                                    zip.AddItem(System.AppDomain.CurrentDomain.BaseDirectory + hdSeleccionarXml.Value, "");
                                }
                                catch { }
                            }
                        }
                    }
                    if (bChk)
                    {
                        //Response.Clear();
                        //Response.ContentType = "application/zip";
                        //Response.AddHeader("content-disposition", "filename=" + "Facturas.zip");
                        //zip.Save(Response.OutputStream);
                        var ms = new MemoryStream();
                        zip.Save(ms);
                        var bytes = ms.ToArray();
                        var base64 = Convert.ToBase64String(bytes);
                        var contentType = "application/x-zip-compressed";
                        var fileName = "FacturasEmision_" + Localization.Now.ToString("ddMMyyyy") + ".zip";
                        ScriptManager.RegisterStartupScript(this, GetType(), "_btnZipkey", "downloadBytes('" + base64 + "','" + contentType + "','" + fileName + "', false);", true);
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Debes seleccionar una factura", 4, null);
                    }
                }
            }
            else
            {
                var descarga = new wsDescargarZip.wsDescargarZip();

                var Rango = tbFechaInicial.Text + "," + tbFechaFinal.Text;
                var RFCEMI = Session["IDENTEMI"].ToString();
                string fecFin = "";
                string rutaZip = "";
                DateTime fecSolic = Localization.Now;
                var NroFAc = "";

                foreach (GridViewRow rowsid in gvFacturas.Rows)
                {
                    var chkSeleccionar = (CheckBox)rowsid.FindControl("check");
                    var hdId = (HiddenField)rowsid.FindControl("checkHdID");
                    if (chkSeleccionar.Checked)
                    {
                        NroFAc += hdId.Value + ",";
                    }
                }

                Nrofacuras = NroFAc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (Nrofacuras.Count() > 0)
                {
                    try
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Su descarga fue enviada a descargas ZIP", 4, null);
                        descarga.DescargarZipAsync(_idUser, Rango, Nrofacuras, fecSolic, fecFin, rutaZip, RFCEMI);
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Debe seleccionar los comprobantes", 4, null);
                }

            }
        }

        /// <summary>
        /// Handles the Click event of the btnDPDF control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnDPDF_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var pdf = "";
            var rutaCodigoControl = "";
            var idComprobante = "";
            var rutaDocus = "";
            var rutaPdf = "";
            var uuid = "";
            _db.Conectar();
            _db.CrearComando(@"select dirdocs from Par_ParametrosSistema");

            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                rutaDocus = dr[0].ToString().Trim();
            }
            _db.Desconectar();

            rutaCodigoControl = btn.CommandArgument.ToString();
            var parametros = rutaCodigoControl.Split(';');
            pdf = parametros[0];
            idComprobante = parametros[3];
            uuid = parametros[4];
            pdf = pdf.Replace(@"docus\", "").Replace("docus//", "").Replace("docus/", "");
            rutaPdf = rutaDocus + pdf;
            rutaPdf = rutaPdf.Replace("/", @"\").Replace(@"\\", @"\").Replace("//", @"\");
            if (pdf != null && (File.Exists(rutaPdf)))
            {
                var urlRedirect = "download.aspx?file=" + parametros[0] + "&ext=.pdf";
                Response.Redirect(urlRedirect);
            }
            else
            {
                var pageurl = ResolveUrl("~/descargarPDF.aspx?idFactura=" + idComprobante + "&uuid=" + uuid);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "loadPdfModal('" + pageurl + "&mode=view','" + pageurl + "&mode=download');", true);
            }
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
            lCount.Text = "<strong>Se encontraron <span class='badge'>" + e.AffectedRows + "</span> registros</strong>";
        }

        /// <summary>
        /// Handles the Click event of the bBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
            ScriptManager.RegisterStartupScript(this, GetType(), "_bBuscar_Click_CerrarFiltros", "$('#FiltrosC2').modal('hide');", true);
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

            var facturasPropias = "";
            var tipoComp = "";
            var tipoCompHb = "";
            var topCopm = "''";
            if (Session["TOPComp"] != null) { topCopm = Session["TOPComp"].ToString(); }
            if (Convert.ToBoolean(Session["coFactTodas"]))
            {
                _todasFacturas = "true";
                facturasPropias = "true";
            }
            else
            {
                _todasFacturas = "false";
                facturasPropias = Convert.ToBoolean(Session["coFactPropias"]) ? "true" : "false";
            }
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
                        tipoCompHb = "ISNULL((select CASE TiposDocumentos.descripcionDocumento WHEN 'Factura' THEN '01' WHEN 'Nota de Credito' THEN '04' WHEN 'Retencion' THEN '07' END), '01') in (" + tipoComp.Trim(',') + ") ";
                        tipoComp = "Dat_General.codDoc in (" + tipoComp.Trim(',') + ") ";
                    }
                }
            }
            var queryString = "Dat_General.tipo <> 'C' AND Dat_General.estado <> 2";
            if (ddlEstado.SelectedIndex == 0 && Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90"))
            {
                queryString += " AND Dat_General.estado <> 0";
            }
            _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            _db.Conectar();
            _db.CrearComandoProcedimiento("PA_facturas_basico");
            _db.AsignarParametroProcedimiento("@QUERY", System.Data.DbType.String, _consulta);
            _db.AsignarParametroProcedimiento("@RFC", System.Data.DbType.String, Session["rfcCliente"].ToString());
            _db.AsignarParametroProcedimiento("@ROL", System.Data.DbType.Boolean, _todasFacturas);
            _db.AsignarParametroProcedimiento("@empleado", System.Data.DbType.String, Session["USERNAME"].ToString());
            _db.AsignarParametroProcedimiento("@ROLSUCURSAL", System.Data.DbType.String, facturasPropias);
            _db.AsignarParametroProcedimiento("@TOP", System.Data.DbType.String, topCopm);
            _db.AsignarParametroProcedimiento("@PTOEMII", System.Data.DbType.String, "1");
            _db.AsignarParametroProcedimiento("@CNSComp", System.Data.DbType.String, tipoComp);
            _db.AsignarParametroProcedimiento("@QUERYSTRING", DbType.String, queryString);
            var sql = @"EXEC    [dbo].[PA_facturas_basico]
        @QUERY = N'" + _consulta.Replace("'", "''") + @"',
        @RFC = N'" + Session["rfcCliente"].ToString().Replace("'", "''") + @"',
        @ROL = " + (bool.Parse(_todasFacturas) ? "1" : "0") + @",
        @empleado = N'" + Session["USERNAME"].ToString().Replace("'", "''") + @"',
        @ROLSUCURSAL = " + facturasPropias.Replace("'", "''") + @",
        @TOP = N'" + topCopm.Replace("'", "''") + @"',
        @PTOEMII = N'1',
        @CNSComp = N'" + tipoComp.Replace("'", "''") + @"',
        @QUERYSTRING = N'" + queryString.Replace("'", "''") + @"'";
            var dtIpso = new DataTable();
            try
            {
                if (!chkHistorico.Checked)
                {
                    var drIpso = _db.EjecutarConsulta();
                    dtIpso.Load(drIpso);
                }
            }
            catch (Exception ex)
            {
                var message = "SQL: " + sql;
                _db.Desconectar();
                throw new Exception(message, ex);
            }
            finally
            {
                _db.Desconectar();
            }
            if (chkHistorico.Checked)
            {
                if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90"))
                {
                    /* AQUI VA LA CONEXION A BASE DE DATOS DE HB Y LA EJECUCION DEL STORED PROCEDURE */
                    var dbHb = new BasesDatos(Session["IDENTEMI"].ToString(), "Historial");
                    dbHb.Conectar();
                    dbHb.CrearComandoProcedimiento("PA_facturas_basico_Ipsofactu");
                    dbHb.AsignarParametroProcedimiento("@QUERY", System.Data.DbType.String, _consulta);
                    dbHb.AsignarParametroProcedimiento("@RFC", System.Data.DbType.String, Session["rfcCliente"].ToString());
                    dbHb.AsignarParametroProcedimiento("@ROL", System.Data.DbType.Boolean, _todasFacturas);
                    dbHb.AsignarParametroProcedimiento("@empleado", System.Data.DbType.String, Session["USERNAME"].ToString());
                    dbHb.AsignarParametroProcedimiento("@ROLSUCURSAL", System.Data.DbType.String, facturasPropias);
                    dbHb.AsignarParametroProcedimiento("@TOP", System.Data.DbType.String, topCopm);
                    dbHb.AsignarParametroProcedimiento("@PTOEMII", System.Data.DbType.String, "1");
                    dbHb.AsignarParametroProcedimiento("@CNSComp", System.Data.DbType.String, tipoCompHb);
                    dbHb.AsignarParametroProcedimiento("@QUERYSTRING", DbType.String, "");
                    var drIpso = dbHb.EjecutarConsulta();
                    dtIpso.Load(drIpso);
                    dbHb.Desconectar();
                }
            }
            try
            {
                var rows2 = dtIpso.Rows.Cast<DataRow>().Where(row => row.Field<string>("EDOFAC") == "2").ToList();
                if (rows2.Count > 0)
                {
                    rows2.ForEach(row => dtIpso.Rows.Remove(row));
                }

                _db.Conectar();
                _db.CrearComando(@"SELECT NOMEMI FROM Cat_Emisor WHERE RFCEMI = @RFCEMI");
                _db.AsignarParametroCadena("@RFCEMI", tbRFCemi.Text);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    var rows3 = dtIpso.Rows.Cast<DataRow>().Where(row => row.Field<string>("RFCEMI") != tbRFCemi.Text.ToString()).ToList();
                    if (rows3.Count > 0)
                    {
                        rows3.ForEach(row => dtIpso.Rows.Remove(row));
                    }
                }
                _db.Desconectar();
            }
            catch (Exception ex)
            {
                // ignored
            }
            _dtActual = dtIpso;
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

            Session["gvComprobantes"] = _dtActual;
            lCount.Text = "<strong>Se encontraron</strong> <span class='badge'>" + _dtActual.Rows.Count + "</span> <strong>Registros</strong> ";
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
                    var observaciones = row.Field<string>("observaciones");
                    var estado = row.Field<string>("EDOFAC");
                    var errorSat = row.Field<string>("mensajeSAT");
                    try { observaciones = observaciones.Trim("<br/>".ToCharArray()); } catch { }
                    try { estado = estado.Trim("<br/>".ToCharArray()); } catch { }
                    try { errorSat = errorSat.Trim("<br/>".ToCharArray()); } catch { }
                    if (!string.IsNullOrEmpty(observaciones) || !string.IsNullOrEmpty(errorSat))
                    {
                        var nameScaped = observaciones.Replace("'", "\"");
                        lb.Attributes["data-content"] = (!string.IsNullOrEmpty(observaciones) ? (observaciones + "<br/>") : "") + (estado.Equals("0") ? "<font color='red'>Error" + (!string.IsNullOrEmpty(errorSat) ? (": " + errorSat) : "") + "</font>" : "") + "<div align='right'><button type='button' id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
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
        /// Handles the Click event of the bCancelarComprobante control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bCancelarComprobante_Click(object sender, EventArgs e)
        {
            var ambiente = false;
            try
            {
                var btn = (LinkButton)sender;
                var id = btn.CommandArgument.ToString();
                try
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT ambiente FROM Dat_General WHERE idComprobante = @id");
                    _db.AsignarParametroCadena("@id", id);
                    var dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        var str = dr["ambiente"].ToString();
                        ambiente = str.Equals("2");
                    }
                    _db.Desconectar();
                }
                catch (Exception ex)
                {

                }
                var ws = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                var respuesta = ws.CancelarComprobante(id, _idUser, Session["IDENTEMI"].ToString(), ambiente);
                var mensaje = ws.ObtenerMensaje();
                if (respuesta)
                {
                    Thread.Sleep(5000);
                    Buscar();
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobate ahora está en proceso de cancelación", 2, null, "bootbox.hideAll();");
                }
                else
                {
                    if (mensaje.Contains("Resolución Miscelánea Fiscal 2017 para cancelaciones"))
                    {
                        var listMsg = new List<string>();
                        var prefix = "<i class='fa fa-check' aria-hidden='true'></i>&nbsp;";
                        listMsg.Add(prefix + "Si el CFDI a cancelar es por concepto de nómina.");
                        listMsg.Add(prefix + "Si el importe del ingreso facturado no rebasa los 5,000 pesos.");
                        listMsg.Add(prefix + "Si se trata de un CFDI de egresos o traslado.");
                        listMsg.Add(prefix + "Si se le facturó a un contribuyente del RIF.");
                        listMsg.Add(prefix + "Si realizaron la factura por operaciones con Público en General.");
                        listMsg.Add(prefix + "Facturas a extranjeros");
                        listMsg.Add(prefix + "Cuando la cancelación del CFDI se de dentro de las primeras 72 horas después de su emisión.");
                        var msg = string.Join("<br/>", listMsg);
                        mensaje += ":<br/><span class='label label-warning' style='display:inline-block !important; text-align: left !important;'>" + msg + "</span><br/>Para cancelarlo se debe crear una Nota de Crédito de Anulación o realizar la solicitud de cancelación mediante el buzón tributario del SAT.<br/><br/><a class='btn btn-primary btn-sm' href='https://www.siat.sat.gob.mx/PTSC/' Target='_blank'><i class='fa fa-external-link' aria-hidden='true'></i>&nbsp;Buzón Tributario</a>&nbsp;&nbsp;<a class='btn btn-primary btn-sm' href='javascript:showRegistroBuzonSat();'><i class='fa fa-question-circle' aria-hidden='true'></i>&nbsp;Guía de registro</a>";
                    }
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se pudo cancelar<br/><br/>" + mensaje, 4, null, "bootbox.hideAll();");
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se pudo cancelar<br/><br/>" + ex.ToString(), 4, null, "bootbox.hideAll();");
            }
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
        /// Handles the Click event of the lbAnularFactura control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbAnularFactura_Click(object sender, EventArgs e)
        {
            int totales = 0;
            int procesadas = 0;
            string mensajes = "";
            foreach (GridViewRow row in gvFacturas.Rows)
            {
                var chkSeleccionar = (CheckBox)row.FindControl("check");
                var hdId = (HiddenField)row.FindControl("checkHdID");
                var hdSerFol = (HiddenField)row.FindControl("checkSerFol");
                var hdEstado = (HiddenField)row.FindControl("checkHdEstado");
                var hdTipo = (HiddenField)row.FindControl("checkTipo");
                if (chkSeleccionar.Checked)
                {
                    totales++;
                    if (hdTipo.Value.Equals("01"))
                    {
                        if (hdEstado.Value.Equals("1"))
                        {
                            try
                            {
                                var idFactura = hdId.Value;
                                var motivo = "ESTA NOTA DE CREDITO AFECTA A LA FACTURA " + hdSerFol.Value + ". " + tbMotivoAnulacion.Text;
                                var sql = "";
                                decimal iva16 = 0;
                                decimal propina = 0;
                                DbDataReader dr;
                                var sAmbiente = "1";
                                var conceptos = new List<object[]>();
                                var txt = new SpoolMx();
                                var versionXmlOriginal = "";
                                var folioReservacion = "";
                                sql = @"SELECT TOP 1
                            g.referencia,
                            p.formapago, p.condicionesDePago, g.subTotal, g.totalDescuento, g.motivoDescuento, g.tipoCambio, g.moneda, g.total, 'egreso' as tipoDoc, g.metodoPago, g.lugarExpedicion, p.numCtaPago, g.numeroAutorizacion, g.serie, g.fecha, g.total, g.codDoc, e.NOMEMI, e.RFCEMI, e.curp as curpE, e.telefono as telE, e.email as mailE, e.EmpresaTipo as etipoE, e.regimenFiscal as regimenE, e.obligadoContabilidad as contE, e.dirMatriz AS calleE, e.noExterior as extE, e.noInterior as intE, e.colonia as colE, e.localidad as locE, e.referencia as refE, e.municipio as munE, e.estado as edoE, e.pais as paisE, e.codigoPostal as cpE, d.dirEstablecimientos as calleExp, d.noExterior as extExp, d.noInterior as intExp, d.colonia as colExp, d.localidad as locExp, d.referencia as refExp, d.municipio as munExp, d.estado as esoExp, d.pais as paisExp, d.codigoPostal as cpExp, r.NOMREC, r.RFCREC, r.curp as curpR, r.telefono as telR, r.email as mailR, r.telefono2 as tel2R, r.denominacionSocial as denR, r.obligadoContabilidad as contR, r.domicilio as calleR, r.noExterior as extR, r.noInterior as intR, r.colonia as colR, r.localidad as locR, r.referencia as refR, r.municipio as munR, r.estado as edoR, r.pais as paisR, r.codigoPostal as cpR, IVA12 as iva16, g.propina, g.ambiente, g.cargoxservicio as otrosCargos, g.importeAPagar as totalAPagar, g.observaciones, he.tipo AS tipoHE, g.folioReservacion, he.noHabitacion, he.fechaLlegada, he.fechaSalida, he.huesped,he.tipohabitacion, g.estab as claveSucursal, g.noTicket, g.usoCfdi, r.numRegIdTrib, g.version
                        FROM
                            Dat_General g INNER JOIN
                            Cat_Emisor e ON g.id_Emisor = e.IDEEMI INNER JOIN
                            Dat_DOMEMIEXP d ON g.id_EmisorExp = d.IDEDOMEMIEXP INNER JOIN
                            Cat_Receptor r ON g.id_Receptor = r.IDEREC INNER JOIN
                            Dat_Pagos p ON g.idComprobante = p.id_Comprobante LEFT OUTER JOIN
                            Dat_HabitacionEvento he ON he.id_Comprobante = g.idComprobante
                        WHERE g.idComprobante = @ID";
                                _db.Conectar();
                                _db.CrearComando(sql);
                                _db.AsignarParametroCadena("@ID", idFactura);
                                dr = _db.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    var formaPago = Session["CfdiVersion"].ToString().Equals("3.3") ? "PUE" : dr["formapago"].ToString();
                                    versionXmlOriginal = dr["version"].ToString();
                                    folioReservacion = dr["folioReservacion"].ToString();
                                    var moneda = dr["moneda"].ToString();
                                    var tipoCambio = dr["tipoCambio"].ToString();
                                    if (string.IsNullOrEmpty(moneda))
                                    {
                                        moneda = "MXN";
                                    }
                                    if (string.IsNullOrEmpty(folioReservacion)) { folioReservacion = dr["noTicket"].ToString(); }
                                    txt.SetComprobanteCfdi(ddlSerie.SelectedItem.Text, "", Localization.Now.ToString("s"), formaPago, dr["condicionesDePago"].ToString(), dr["subTotal"].ToString(), dr["totalDescuento"].ToString(), dr["motivoDescuento"].ToString(), tipoCambio, moneda, dr["total"].ToString(), dr["tipoDoc"].ToString(), dr["metodoPago"].ToString(), dr["lugarExpedicion"].ToString(), dr["numCtaPago"].ToString(), dr["numeroAutorizacion"].ToString() + ":01", dr["serie"].ToString(), Localization.Parse(dr["fecha"].ToString()).ToString("s"), dr["total"].ToString(), dr["codDoc"].ToString(), motivo, dr["otrosCargos"].ToString(), dr["totalAPagar"].ToString(), dr["observaciones"].ToString());
                                    txt.SetEmisorCfdi(dr["NOMEMI"].ToString(), dr["RFCEMI"].ToString(), dr["curpE"].ToString(), dr["telE"].ToString(), dr["mailE"].ToString(), dr["etipoE"].ToString(), dr["regimenE"].ToString(), dr["contE"].ToString());
                                    txt.SetEmisorDomCfdi(dr["calleE"].ToString(), dr["extE"].ToString(), dr["intE"].ToString(), dr["colE"].ToString(), dr["locE"].ToString(), dr["refE"].ToString(), dr["munE"].ToString(), dr["edoE"].ToString(), dr["paisE"].ToString(), dr["cpE"].ToString());
                                    txt.SetEmisorExpCfdi(dr["calleExp"].ToString(), dr["extExp"].ToString(), dr["intExp"].ToString(), dr["colExp"].ToString(), dr["locExp"].ToString(), dr["refExp"].ToString(), dr["munExp"].ToString(), dr["esoExp"].ToString(), dr["paisExp"].ToString(), dr["cpExp"].ToString(), dr["claveSucursal"].ToString());
                                    var UsoCfdi = dr["usoCfdi"].ToString();
                                    if (string.IsNullOrEmpty(UsoCfdi)) { UsoCfdi = "P01"; }
                                    txt.SetReceptorCfdi(dr["NOMREC"].ToString(), dr["RFCREC"].ToString(), dr["curpR"].ToString(), dr["telR"].ToString(), dr["mailR"].ToString(), dr["tel2R"].ToString(), dr["denR"].ToString(), dr["contR"].ToString(), dr["numRegIdTrib"].ToString(), UsoCfdi);
                                    txt.SetReceptorDomCfdi(dr["calleR"].ToString(), dr["extR"].ToString(), dr["intR"].ToString(), dr["colR"].ToString(), dr["locR"].ToString(), dr["refR"].ToString(), dr["munR"].ToString(), dr["edoR"].ToString(), dr["paisR"].ToString(), dr["cpR"].ToString());
                                    decimal.TryParse(CerosNull(dr["iva16"].ToString()), out iva16);
                                    decimal.TryParse(CerosNull(dr["propina"].ToString()), out propina);
                                    sAmbiente = dr["ambiente"].ToString();
                                    if (!(dr["tipoHE"] is DBNull) && dr["tipoHE"] != null && !string.IsNullOrEmpty(dr["tipoHE"].ToString()))
                                    {
                                        switch (dr["tipoHE"].ToString())
                                        {
                                            case "1":
                                                txt.SetInfoAdicionalHabitacionCfdi(dr["huesped"].ToString(), folioReservacion, dr["noHabitacion"].ToString(), dr["fechaLlegada"].ToString(), dr["fechaSalida"].ToString(), dr["referencia"].ToString(), dr["tipohabitacion"].ToString(), dr["propina"].ToString());
                                                break;
                                            case "2":
                                                txt.SetInfoAdicionalEventoCfdi(dr["huesped"].ToString(), folioReservacion, dr["fechaLlegada"].ToString(), dr["propina"].ToString());
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(folioReservacion))
                                    {
                                        txt.SetInfoAdicionalRestauranteCfdi(CerosNull(propina.ToString()), folioReservacion);
                                    }
                                }
                                _db.Desconectar();
                                sql = @"SELECT
                             ISNULL(SUM(it.valor), 0.00) as impTras, ISNULL(SUM(ir.valorRetenido), 0.00) as impRets
                        FROM
                            Dat_General g LEFT OUTER JOIN
                            Dat_TotalConImpuestos it ON it.id_Comprobante = g.idComprobante LEFT OUTER JOIN
                            Dat_ImpuestosRetenciones ir ON ir.numDocSustento = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                                _db.Conectar();
                                _db.CrearComando(sql);
                                _db.AsignarParametroCadena("@ID", idFactura);
                                dr = _db.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    txt.SetCantidadImpuestosCfdi(dr["impRets"].ToString(), dr["impTras"].ToString(), CerosNull(iva16.ToString()));
                                }
                                _db.Desconectar();
                                sql = @"SELECT
                            ir.tipo as impuesto, ir.valorRetenido as importe, ir.tipoFactor
                        FROM
                            Dat_ImpuestosRetenciones ir INNER JOIN
                            Dat_General g on ir.numDocSustento = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                                _db.Conectar();
                                _db.CrearComando(sql);
                                _db.AsignarParametroCadena("@ID", idFactura);
                                dr = _db.EjecutarConsulta();
                                while (dr.Read())
                                {
                                    txt.AgregaImpuestoRetencionCfdi(dr["impuesto"].ToString(), dr["importe"].ToString(), dr["tipoFactor"].ToString());
                                }
                                _db.Desconectar();
                                sql = @"SELECT
                            CASE
                                it.codigo
                                WHEN '1' THEN 'IVA'
                                WHEN '2' THEN 'IEPS'
                            END AS impuesto, it.tarifa as tasa, it.valor as importe, it.tipoFactor
                        FROM
                            Dat_TotalConImpuestos it INNER JOIN
                            Dat_General g on it.id_Comprobante = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                                _db.Conectar();
                                _db.CrearComando(sql);
                                _db.AsignarParametroCadena("@ID", idFactura);
                                dr = _db.EjecutarConsulta();
                                while (dr.Read())
                                {
                                    txt.AgregaImpuestoTrasladoCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString(), dr["tipoFactor"].ToString());
                                }
                                _db.Desconectar();
                                #region Conceptos
                                if (Session["CfdiVersion"].ToString().Equals("3.3") && versionXmlOriginal.Equals("3.3"))
                                {
                                    sql = "SELECT p.dirdocs, a.XMLARC FROM Dat_General g INNER JOIN Dat_Archivos a ON g.idComprobante = a.IDEFAC, Par_ParametrosSistema p WHERE g.idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
                                    dr = _db.EjecutarConsulta();
                                    if (dr.Read())
                                    {
                                        var rutaXml = dr["dirdocs"].ToString().Replace("docus", "") + @"\" + dr["XMLARC"].ToString();
                                        var fileXml = new FileInfo(rutaXml);
                                        if (fileXml.Exists)
                                        {
                                            var xDoc = new XmlDocument();
                                            xDoc.Load(fileXml.FullName);
                                            var nodosConceptos = xDoc.GetElementsByTagName("cfdi:Concepto").Cast<XmlNode>().ToList();
                                            for (int i = 0; i < nodosConceptos.Count; i++)
                                            {
                                                var concepto = nodosConceptos[i];
                                                var cantidad = "";
                                                var unidad = "";
                                                var numeroIdentificacion = "";
                                                var descripcion = "";
                                                var valorUnitario = "";
                                                var importe = "";
                                                var descuento = "";
                                                var claveProdServ = "";
                                                var claveUnidad = "";
                                                try { cantidad = concepto.Attributes["Cantidad"].Value; } catch (Exception ex) { }
                                                try { unidad = concepto.Attributes["Unidad"].Value; } catch (Exception ex) { }
                                                try { numeroIdentificacion = concepto.Attributes["NoIdentificacion"].Value; } catch (Exception ex) { }
                                                try { descripcion = concepto.Attributes["Descripcion"].Value; } catch (Exception ex) { }
                                                try { valorUnitario = concepto.Attributes["ValorUnitario"].Value; } catch (Exception ex) { }
                                                try { importe = concepto.Attributes["Importe"].Value; } catch (Exception ex) { }
                                                try { descuento = concepto.Attributes["Descuento"].Value; } catch (Exception ex) { }
                                                //try { claveProdServ = concepto.Attributes["ClaveProdServ"].Value; } catch (Exception ex) { }
                                                //try { claveUnidad = concepto.Attributes["ClaveUnidad"].Value; } catch (Exception ex) { }
                                                claveProdServ = "84111506";
                                                claveUnidad = "ACT";
                                                txt.AgregaConceptoCfdi(cantidad, unidad, numeroIdentificacion, descripcion, valorUnitario, importe, descuento, claveProdServ, claveUnidad, i.ToString());
                                                if (concepto.HasChildNodes)
                                                {
                                                    var nsmgr = new XmlNamespaceManager(xDoc.NameTable);
                                                    nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                                    var trasladosConcepto = concepto.SelectNodes("cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado", nsmgr).Cast<XmlNode>().ToList();
                                                    var retencionesConcepto = concepto.SelectNodes("cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion", nsmgr).Cast<XmlNode>().ToList();
                                                    if (trasladosConcepto != null && trasladosConcepto.Count > 0)
                                                    {
                                                        foreach (var impuesto in trasladosConcepto)
                                                        {
                                                            var baseC = "";
                                                            var impuestoC = "";
                                                            var tipoFactorC = "";
                                                            var tasaOCuotaC = "";
                                                            var importeC = "";
                                                            try { baseC = impuesto.Attributes["Base"].Value; } catch (Exception ex) { }
                                                            try { impuestoC = impuesto.Attributes["Impuesto"].Value; } catch (Exception ex) { }
                                                            try { tipoFactorC = impuesto.Attributes["TipoFactor"].Value; } catch (Exception ex) { }
                                                            try { tasaOCuotaC = impuesto.Attributes["TasaOCuota"].Value; } catch (Exception ex) { }
                                                            try { importeC = impuesto.Attributes["Importe"].Value; } catch (Exception ex) { }
                                                            txt.AgregaConceptoImpuestoCfdi(false, baseC, impuestoC, tipoFactorC, tasaOCuotaC, importeC, i.ToString());
                                                        }
                                                    }
                                                    if (retencionesConcepto != null && retencionesConcepto.Count > 0)
                                                    {
                                                        foreach (var impuesto in retencionesConcepto)
                                                        {
                                                            var baseC = "";
                                                            var impuestoC = "";
                                                            var tipoFactorC = "";
                                                            var tasaOCuotaC = "";
                                                            var importeC = "";
                                                            try { baseC = impuesto.Attributes["Base"].Value; } catch (Exception ex) { }
                                                            try { impuestoC = impuesto.Attributes["Impuesto"].Value; } catch (Exception ex) { }
                                                            try { tipoFactorC = impuesto.Attributes["TipoFactor"].Value; } catch (Exception ex) { }
                                                            try { tasaOCuotaC = impuesto.Attributes["TasaOCuota"].Value; } catch (Exception ex) { }
                                                            try { importeC = impuesto.Attributes["Importe"].Value; } catch (Exception ex) { }
                                                            txt.AgregaConceptoImpuestoCfdi(true, baseC, impuestoC, tipoFactorC, tasaOCuotaC, importeC, i.ToString());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    _db.Desconectar();
                                }
                                else if (Session["CfdiVersion"].ToString().Equals("3.3") && versionXmlOriginal.Equals("3.2"))
                                {
                                    var importeConcepto = "";
                                    var descuentoConcepto = "";
                                    var baseImpuesto = "";
                                    sql = "SELECT SUM(precioTotalSinImpuestos) AS importe, SUM(descuento) AS descuento FROM Dat_Detalles INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
                                    dr = _db.EjecutarConsulta();
                                    if (dr.Read())
                                    {
                                        importeConcepto = ControlUtilities.CerosNull(dr["importe"].ToString());
                                        descuentoConcepto = ControlUtilities.CerosNull(dr["descuento"].ToString());
                                        baseImpuesto = (decimal.Parse(importeConcepto) - decimal.Parse(descuentoConcepto)).ToString();
                                    }
                                    _db.Desconectar();
                                    txt.AgregaConceptoCfdi("1", "Actividad", "", motivo, importeConcepto, importeConcepto, descuentoConcepto, "84111506", "ACT", "1");
                                    sql = @"SELECT
                            ir.tipo as impuesto, ir.valorRetenido as importe, ir.tipoFactor
                        FROM
                            Dat_ImpuestosRetenciones ir INNER JOIN
                            Dat_General g on ir.numDocSustento = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
                                    dr = _db.EjecutarConsulta();
                                    while (dr.Read())
                                    {
                                        txt.AgregaConceptoImpuestoCfdi(true, baseImpuesto, dr["impuesto"].ToString(), dr["tipoFactor"].ToString(), "", dr["importe"].ToString(), "1");
                                    }
                                    _db.Desconectar();
                                    sql = @"SELECT
                            CASE
                                it.codigo
                                WHEN '1' THEN 'IVA'
                                WHEN '2' THEN 'IEPS'
                            END AS impuesto, it.tarifa as tasa, it.valor as importe, it.tipoFactor
                        FROM
                            Dat_TotalConImpuestos it INNER JOIN
                            Dat_General g on it.id_Comprobante = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
                                    dr = _db.EjecutarConsulta();
                                    while (dr.Read())
                                    {
                                        var tipoFactor = dr["tipoFactor"].ToString();
                                        if (string.IsNullOrEmpty(tipoFactor))
                                        {
                                            tipoFactor = "Tasa";
                                        }
                                        txt.AgregaConceptoImpuestoCfdi(false, baseImpuesto, dr["impuesto"].ToString(), tipoFactor, dr["tasa"].ToString(), dr["importe"].ToString(), "1");
                                    }
                                    _db.Desconectar();
                                }
                                else
                                {
                                    sql = "SELECT idDetalles, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial, claveProdServ, claveUnidad, descuento FROM Dat_Detalles INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
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
                                        txt.AgregaConceptoCfdi(concepto[4].ToString(), concepto[6].ToString(), concepto[1].ToString(), concepto[2].ToString(), concepto[3].ToString(), concepto[5].ToString(), concepto[10].ToString(), concepto[8].ToString(), concepto[9].ToString());
                                        if (!string.IsNullOrEmpty(concepto[7].ToString())) { txt.SetPredialConceptoCfdi(concepto[7].ToString()); }
                                        sql = "SELECT idDetallesAduana, numero, fecha, aduana FROM Dat_DetallesAduana WHERE id_Detalles = @idDet";
                                        _db.Conectar();
                                        _db.CrearComando(sql);
                                        _db.AsignarParametroCadena("@idDet", concepto[0].ToString());
                                        dr = _db.EjecutarConsulta();
                                        while (dr.Read())
                                        {
                                            txt.AgregaAduaneraConceptoCfdi(dr["numero"].ToString(), dr["fecha"].ToString(), dr["aduana"].ToString());
                                        }
                                        _db.Desconectar();
                                        var partes = new List<object[]>();
                                        sql = "SELECT idDetallesParte, cantidad, unidad, noIdentificacion, descripcion, valorUnitario, importe FROM Dat_DetallesParte WHERE id_Detalles = @idDet";
                                        _db.Conectar();
                                        _db.CrearComando(sql);
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
                                            sql = "SELECT idDetallesAduana, numero, fecha, aduana FROM Dat_DetallesAduana WHERE id_DetallesParte = @idDet";
                                            _db.Conectar();
                                            _db.CrearComando(sql);
                                            _db.AsignarParametroCadena("@idPart", parte[0].ToString());
                                            dr = _db.EjecutarConsulta();
                                            while (dr.Read())
                                            {
                                                txt.AgregaParteAduaneraConceptoCfdi(dr["numero"].ToString(), dr["fecha"].ToString(), dr["aduana"].ToString());
                                            }
                                            _db.Desconectar();
                                        }
                                    }
                                }
                                #endregion
                                sql = "SELECT idImpLocal FROM Dat_MX_ImpLocales INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @ID";
                                _db.Conectar();
                                _db.CrearComando(sql);
                                _db.AsignarParametroCadena("@ID", idFactura);
                                dr = _db.EjecutarConsulta();
                                var tieneLocales = dr.HasRows;
                                _db.Desconectar();
                                if (tieneLocales)
                                {
                                    sql = @"SELECT totalRetImpLocales, totalTraImpLocales FROM Dat_General WHERE idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
                                    dr = _db.EjecutarConsulta();
                                    if (dr.Read())
                                    {
                                        txt.SetImpuestosLocalesCfdi(dr["totalRetImpLocales"].ToString(), dr["totalTraImpLocales"].ToString());
                                    }
                                    _db.Desconectar();
                                    sql = "SELECT nombre as impuesto, tasadeRetencion as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeRetencion IS NOT NULL AND tasadeTraslado IS NULL AND idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
                                    dr = _db.EjecutarConsulta();
                                    while (dr.Read())
                                    {
                                        txt.AgregaRetencionLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                                    }
                                    _db.Desconectar();
                                    sql = "SELECT nombre as impuesto, tasadeTraslado as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeTraslado IS NOT NULL AND tasadeRetencion IS NULL AND idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
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
                                var txtInvoice = txt.ConstruyeTxtCfdi();
                                var coreMx = new wsEmision.WsEmision { Timeout = (1800 * 5000) };
                                var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, "04", false, true, "", "");
                                if (result != null)
                                {
                                    //sql = @"UPDATE Dat_General SET estado = 4, saldo = total, pagoAplicado = total, saldoPendiente = 0.00, estadoPago = 1, id_Empleado_Canc = @idCanc WHERE idComprobante = @ID";
                                    //_db.Conectar();
                                    //_db.CrearComando(sql);
                                    //_db.AsignarParametroCadena("@ID", idFactura);
                                    //_db.AsignarParametroCadena("@idCanc", _idUser);
                                    //_db.EjecutarConsulta1();
                                    //_db.Desconectar();
                                    sql = @"UPDATE Dat_General SET estado = 4, id_Empleado_Canc = @idCanc WHERE idComprobante = @ID";
                                    _db.Conectar();
                                    _db.CrearComando(sql);
                                    _db.AsignarParametroCadena("@ID", idFactura);
                                    _db.AsignarParametroCadena("@idCanc", _idUser);
                                    _db.EjecutarConsulta1();
                                    _db.Desconectar();
                                    try
                                    {
                                        var idTramas = new List<string>();
                                        _db.Conectar();
                                        _db.CrearComando("select idTrama from log_trama where tipo = 4 and (noReserva = @folioReservacion or noTicket = @folioReservacion)");
                                        _db.AsignarParametroCadena("@folioReservacion", folioReservacion);
                                        _db.AsignarParametroCadena("@folioReservacion", folioReservacion);
                                        dr = _db.EjecutarConsulta();
                                        while (dr.Read()) { idTramas.Add(dr["idTrama"].ToString()); }
                                        _db.Desconectar();
                                        if (idTramas.Count > 1)
                                        {
                                            var idOriginal = idTramas.OrderBy(t => t).First();
                                            if (!string.IsNullOrEmpty(idOriginal))
                                            {
                                                idTramas.Remove(idOriginal);
                                                var tramasBorrar = string.Join(",", idTramas);
                                                _db.Conectar();
                                                _db.CrearComando("DELETE FROM Log_Trama WHERE idTrama IN (" + tramasBorrar + ")");
                                                _db.EjecutarConsulta1();
                                                _db.Desconectar();
                                                _db.Conectar();
                                                _db.CrearComando("UPDATE Log_Trama SET observaciones = 'ExtranetOk', folio = 0 WHERE idTrama = @idTrama");
                                                _db.AsignarParametroCadena("@idTrama", idOriginal);
                                                _db.EjecutarConsulta1();
                                                _db.Desconectar();
                                            }
                                        }
                                    }
                                    catch { }
                                    procesadas++;
                                    //Buscar();
                                    //(Master as SiteMaster).MostrarAlerta(this, "El comprobante se ha generado satisfactoriamente", "Correcto", "bootbox.hideAll();", null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                                }
                                else
                                {
                                    mensajes += "El comprobante \"" + hdSerFol.Value + "\" no se pudo anular: " + coreMx.ObtenerMensaje() + "." + Environment.NewLine;
                                }
                            }
                            catch (Exception ex)
                            {
                                mensajes += "El comprobante \"" + hdSerFol.Value + "\" no se pudo anular: " + ex.Message + "." + Environment.NewLine;
                            }
                        }
                        else
                        {
                            mensajes += "El comprobante \"" + hdSerFol.Value + "\" no está autorizado." + Environment.NewLine;
                        }
                    }
                    else
                    {
                        mensajes += "El comprobante \"" + hdSerFol.Value + "\" es " + hdTipo.Value + ", solo se pueden anular facturas." + Environment.NewLine;
                    }
                }
            }
            Buscar();
            if (!string.IsNullOrEmpty(mensajes))
            {
                var msg = ("<ul style='padding-left:10px;'><li>" + mensajes.Replace(Environment.NewLine, "</li><li>") + "</li></ul>").Replace("<li></li>", "");
                (Master as SiteMaster).MostrarAlerta(this, "Se procesaron " + procesadas + " de " + totales + " comprobantes seleccionados." + Environment.NewLine + msg, 2, null, "bootbox.hideAll();$('#ModuloNota').modal('hide');", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "Se procesaron todos los comprobantes seleccionados", 2, null, "bootbox.hideAll();$('#ModuloNota').modal('hide');", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            }
            //}
            //catch (Exception ex)
            //{
            //    (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se creó correctamente<br/>" + ex.Message, "Error", null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            //}
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
        /// Handles the RowDataBound event of the gvEventos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>

        protected void gvEventos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    var lb = (HyperLink)(e.Row.FindControl("lbPopupDetalles"));
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var name = row.Field<string>("detallesTecnicos");
                    if (!string.IsNullOrEmpty(name))
                    {
                        var nameScaped = name.Replace("'", "\"");
                        lb.Attributes["data-content"] = name + "<br/><div align='right'><button type='button' id='btnPopover_" + lb.ClientID + "' class='btn btn-primary btn-sm btn-clip' data-clipboard-text='" + nameScaped + "' data-clipboard-action='copy'><i class='fa fa-clipboard'></i> Copiar</button>&nbsp;<button id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRFC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRFC_TextChanged(object sender, EventArgs e)
        {
            var nombres = new object[1];
            _db.Conectar();
            _db.CrearComando(@"SELECT NOMREC FROM Cat_Receptor WHERE RFCREC = @RFCREC");
            _db.AsignarParametroCadena("@RFCREC", tbRFC.Text);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                dr.GetValues(nombres);
            }
            _db.Desconectar();
            if (nombres.Length == 1)
            {
                //tbNombre.Text = nombres.FirstOrDefault().ToString();
            }
        }

        /// <summary>
        ///  
        /// </summary>
        //protected void TbRFCemi_TextChanged(object sender, EventArgs e)
        //{
        //    _db.Conectar();
        //    _db.CrearComando(@"SELECT RFCEMI FROM Cat_Emisor WHERE RFCEMI = @RFCEMI");
        //    _db.AsignarParametroCadena("@RFCEMI", tbRFCemi.Text);
        //    var dr = _db.EjecutarConsulta();
        //    if (dr.Read())
        //    {
        //        //ScriptManager.RegisterStartupScript(this, GetType(), "_bBuscar_Click_CerrarFiltros", "$('#FiltrosC2').modal('hide');", true);
        //        Buscar();
        //    }
        //    _db.Desconectar();
        //}

        /// <summary>
        /// Handles the Click event of the bEventos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bEventos_Click(object sender, EventArgs e)
        {
            gvEventos.Sort("fecha", SortDirection.Descending);
            ScriptManager.RegisterStartupScript(this, GetType(), "_keyEventos", "$('#ModuloRemote').modal('show');", true);
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

        private int RowsChecked()
        {
            int counter = 0;
            foreach (GridViewRow row in gvFacturas.Rows)
            {
                CheckBox chkSeleccionar = (CheckBox)row.FindControl("check");
                var hdEstado = (HiddenField)row.FindControl("checkHdEstado");
                var hdTipo = (HiddenField)row.FindControl("checkTipo");
                if (chkSeleccionar.Checked)
                {
                    counter++;
                }
            }
            return counter;
        }

        /// <summary>
        /// Rowses the checked.
        /// </summary>
        /// <param name="tiposValidos">The tipos validos.</param>
        /// <param name="estadosValidos">The estados validos.</param>
        /// <returns>System.Int32.</returns>
        private int RowsChecked(string[] tiposValidos = null, string[] estadosValidos = null)
        {
            int counter = 0;
            foreach (GridViewRow row in gvFacturas.Rows)
            {
                CheckBox chkSeleccionar = (CheckBox)row.FindControl("check");
                var hdEstado = (HiddenField)row.FindControl("checkHdEstado");
                var hdTipo = (HiddenField)row.FindControl("checkTipo");
                if (chkSeleccionar.Checked)
                {
                    if (estadosValidos != null)
                    {
                        if (estadosValidos.Contains(hdEstado.Value))
                        {
                            if (tiposValidos != null)
                            {
                                if (tiposValidos.Contains(hdTipo.Value))
                                {
                                    counter++;
                                }
                            }
                            else
                            {
                                counter++;
                            }
                        }
                    }
                    else
                    {
                        if (tiposValidos != null)
                        {
                            if (tiposValidos.Contains(hdTipo.Value))
                            {
                                counter++;
                            }
                        }
                        else
                        {
                            counter++;
                        }
                    }
                }
            }
            return counter;
        }

        /// <summary>
        /// Handles the Click event of the hlCrearNotaC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void hlCrearNotaC_Click(object sender, EventArgs e)
        {
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
            if (RowsChecked(new string[] { "01" }, new string[] { "1" }) < 1)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debes seleccionar al menos un comprobante autorizado<br/>Ten en cuenta que solo se pueden anular facturas", 4, null);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_keyhlCrearNotaCModal", "$('#ModuloNota').modal('show');", true);
            }
        }

        /// <summary>
        /// Handles the Sorting event of the gvEventos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvEventos_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvEventos.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvEventos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvEventos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEventos.PageIndex = e.NewPageIndex;
            gvEventos.DataBind();
        }

        protected void chkHistorial_CheckedChanged(object sender, EventArgs e)
        {
            tbEmail.Text = "";
            tbFolioAnterior.Attributes["Text"] = "";
            tbFolioAnterior.Text = "";
            ddlTipoDocumento.SelectedValue = "0";
            //ddlSucursal.Text = "Selecciona el Establecimiento";
            tbFechaInicial.Text = "";
            tbFechaFinal.Text = "";
            ddlEstado.SelectedValue = "0";
            tbRFC.Text = "";
            tbNombre.Text = "";

            Buscar();
        }

        protected void bExcel_Click(object sender, EventArgs e)
        {

            //Response.Redirect("Documentos.aspx", false);
            // Response.Redirect("~/Documentos.aspx", false);
            ScriptManager.RegisterStartupScript(this, GetType(), "_FiltoExcel_key", "LimpiarFiltros('bodyFiltros1', 'FiltroExcel'); ", true);
        }

        /// <summary>
        /// Handles the Click1 event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bSubirExcel(object sender, EventArgs e)
        {
            //Response.Redirect("Documentos.aspx", false);
            // Response.Redirect("~/Documentos.aspx", false);

            string strFileName;
            string strFilePath;
            string strFolder;

            strFolder = "C:/Users/haza/Documents/DataExpress/YanServDirectoras/txt/";

            // ambiente de pruebas
             strFolder = "C:/DataExpress/IpsofactuMx/TramasPrueba/xlsx_directorasPruebas/"; 

            // ambiente de produccion
            // strFolder = "C:/DataExpress/IpsofactuMx/TramasPrueba/xlsx_directoras\";

            strFileName = File1.PostedFile.FileName;
            strFileName = Path.GetFileName(strFileName);
            string extension = Path.GetExtension(strFileName);

            if (File1.Value != "")
            {
                if (!extension.ToString().Equals(".xlsx"))
                {
                    string script1 = @"<script type='text/javascript'>
                                alert('No se ha cargado ningun un archivo con extencion .xlsx');
                            </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script1, false);
                }
                else
                {
                    // Create the folder if it does not exist.
                    if (!Directory.Exists(strFolder))
                    {
                        Directory.CreateDirectory(strFolder);
                    }
                    // Save the uploaded file to the server.
                    strFilePath = strFolder + strFileName;
                    if (File.Exists(strFilePath))
                    {
                        string script2 = @"<script type='text/javascript'>
                                alert('Ya existe un documento con el mismo nombre.');
                            </script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
                    }
                    else
                    {
                        string script3 = @"<script type='text/javascript'>
                                alert('Archivo listo para ser procesado.');
                            </script>";
                        File1.PostedFile.SaveAs(strFilePath);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script3, false);
                    }

                }
            }
            else
            {
                string script4 = @"<script type='text/javascript'>
                                alert('No se ha seleccionado ningun archivo.');
                            </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script4, false);
            }
            // Display the result of the upload.
            // frmConfirmation.Visible = true;

        }

    }
}
