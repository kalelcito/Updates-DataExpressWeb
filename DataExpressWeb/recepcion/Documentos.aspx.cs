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
using DataExpressWeb.wsRecepcion;
using Datos;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace DataExpressWeb.recepcion
{
    /// <summary>
    /// Class Documentos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Documentos : Page
    {
        /// <summary>
        /// The _chkbox select all
        /// </summary>
        private static CheckBox _chkboxSelectAll;
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log;
        /// <summary>
        /// The _aux
        /// </summary>
        private string _aux;
        /// <summary>
        /// The _cantidad
        /// </summary>
        private int _cantidad;
        /// <summary>
        /// The _consulta
        /// </summary>
        private string _consulta = "";
        /// <summary>
        /// The _dbe
        /// </summary>
        private BasesDatos _dbe;
        /// <summary>
        /// The _DBR
        /// </summary>
        private BasesDatos _dbr;
        /// <summary>
        /// The _DT
        /// </summary>
        private readonly DataTable _dt = new DataTable();
        /// <summary>
        /// The _DT actual
        /// </summary>
        private DataTable _dtActual = new DataTable();
        /// <summary>
        /// The _em
        /// </summary>
        private SendMail _em;
        /// <summary>
        /// The _email credencial
        /// </summary>
        private string _emailCredencial = "";
        /// <summary>
        /// The _email enviar
        /// </summary>
        private string _emailEnviar = "";
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";
        /// <summary>
        /// The _pass credencial
        /// </summary>
        private string _passCredencial = "";
        /// <summary>
        /// The _puerto
        /// </summary>
        private int _puerto = 25;
        /// <summary>
        /// The _ruta document
        /// </summary>
        private string _rutaDoc = "";
        /// <summary>
        /// The _seleccionados
        /// </summary>
        private string[] _seleccionados;
        /// <summary>
        /// The _separador
        /// </summary>
        private readonly string _separador = "|";
        /// <summary>
        /// The _servidor
        /// </summary>
        private string _servidor = "";
        /// <summary>
        /// The _SSL
        /// </summary>
        private bool _ssl;
        /// <summary>
        /// The hidden rows
        /// </summary>
        private int _hiddenRows = 0;

        private string nameEstruc = "";

        private string idPDFupdate = "";


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["statusDistribuidor"] = null;
            _hiddenRows = 0;
            if (Session["IDENTEMI"] != null)
            {
                _dbe = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                _dbr = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE", "Recepcion");
                _log = new Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                SqlDataSource1.ConnectionString = _dbr.CadenaConexion;
                SqlDataSource2.ConnectionString = _dbr.CadenaConexion;
                SqlDataSourcePtoEmi.ConnectionString = _dbr.CadenaConexion;
                SqlDataSourceTipoDoc.ConnectionString = _dbr.CadenaConexion;
                SqlDataSeriesRet.ConnectionString = _dbe.CadenaConexion;
                SqlDataSourcePagos.ConnectionString = _dbr.CadenaConexion;
                SqlDataSourceModTipoProv.ConnectionString = _dbr.CadenaConexion;
                SqlDataSourceGeneral.ConnectionString = _dbr.CadenaConexion;
                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                }
                Session["_isWorkflow"] = IsWorkflow(); //Usage: Convert.ToBoolean(Session["_isWorkflow"])
                bool validador = false;
                bool.TryParse(Session["validacionRecepcion"].ToString(), out validador);
                divValidador.Visible = Convert.ToBoolean(Session["_isWorkflow"]) && validador;

                //obtener status distribuidor
                _dbe.Conectar();
                _dbe.CrearComando(@"select g.statusDistribuidor from Cat_Empleados e 
            inner join Cat_Roles r on e.id_Rol = r.idRol and r.validacionRecepcion = 1
            left outer join " + _dbr.DatabaseSchema + @".dbo.Cat_Grupos_validadores g on g.idGrupo = e.idGrupo
            where e.idEmpleado =@iduser");
                _dbe.AsignarParametroCadena("@iduser", Session["idUser"].ToString());
                var dr = _dbe.EjecutarConsulta();
                Session["statusDistribuidor"] = dr.Read() && dr[0].ToString().Equals("1");
                _dbe.Desconectar();
                SqlDataSourceDocsAdi.ConnectionString = _dbr.CadenaConexion;
            }

            if (!IsPostBack)
            {
                Session["gvFacturas"] = null;
                _dtActual = new DataTable();

                if (Convert.ToBoolean(Session["coFactPropias"]) || Convert.ToBoolean(Session["coFactTodas"]))
                {
                    if (Convert.ToBoolean(Session["EnvioEmail"]))
                    {
                        tbEmail.Visible = true;
                        bMail.Visible = true;
                    }
                    else
                    {
                        tbEmail.Visible = false;
                        bMail.Visible = false;
                    }
                    if (string.IsNullOrEmpty((string)Session["rfcCliente"]))
                    {
                        _consulta = "";
                    }
                    if (Session["rolUser"].ToString() == "3")
                    {
                        _consulta = "";
                    }
                }
                ddlTipoDocumento.DataBind();
                ddlTipoDocumento_SelectedIndexChanged(null, null);
                ddlSerie1.DataBind();
                ddlSerie1.SelectedIndex = 0;
                ddlSerie1_SelectedIndexChanged(null, null);
                Buscar();
            }
        }

        private bool IsWorkflow()
        {
            var result = true;
            try
            {
                _dbr.Conectar();
                _dbr.CrearComando("SELECT idGrupo FROM Cat_Grupos_validadores");
                var dr = _dbr.EjecutarConsulta();
                result = dr.Read();
            }
            catch (Exception ex)
            {
                // ignored
            }
            finally
            {
                _dbr.Desconectar();
            }
            return result;
        }

        /// <summary>
        /// Handles the Click event of the hlCrearRetencion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void hlCrearRetencion_Click(object sender, EventArgs e)
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
            if (rowsChecked(new string[] { "01", "04", "08", "06" }, new string[] { "1", "4" }) < 1)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debes seleccionar al menos un comprobante autorizado<br/>Ten en cuenta que solo se pueden generar retenciones para facturas, notas de crédito, nóminas y cartas porte", 4, null);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_keyhlCrearRetencionModal", "$('#ModuloRetencion').modal('show');", true);
            }
        }

        /// <summary>
        /// Rowses the checked.
        /// </summary>
        /// <param name="tiposValidos">The tipos validos.</param>
        /// <param name="estadosValidos">The estados validos.</param>
        /// <returns>System.Int32.</returns>
        private int rowsChecked(string[] tiposValidos = null, string[] estadosValidos = null)
        {
            int counter = 0;
            foreach (GridViewRow row in gvFacturas.Rows)
            {
                CheckBox chk_Seleccionar = (CheckBox)row.FindControl("check");
                var hdEstado = (HiddenField)row.FindControl("checkHdEstado");
                var hdTipo = (HiddenField)row.FindControl("checkTipo");
                if (chk_Seleccionar.Checked)
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
        /// Handles the SelectedIndexChanged event of the ddlSerie1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlSerie1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sql = "SELECT c.codigo, c.descripcion AS ambiente FROM Cat_Catalogo1_C c INNER JOIN Cat_Series s ON s.ambiente = c.codigo WHERE c.tipo = 'Ambiente' AND s.idSerie = @idSerie";
            _dbe.Conectar();
            _dbe.CrearComando(sql);
            _dbe.AsignarParametroCadena("@idSerie", ddlSerie1.SelectedValue);
            var dr = _dbe.EjecutarConsulta();
            if (dr.Read())
            {
                hfAmbiente1.Value = dr["codigo"].ToString();
                tbAmbiente1.Text = dr["ambiente"].ToString();
            }
            _dbe.Desconectar();
        }

        /// <summary>
        /// Handles the Click event of the lbCrearRet control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbCrearRet_Click(object sender, EventArgs e)
        {
            int totales = 0;
            int procesadas = 0;
            string mensajes = "";
            string sql = "";
            DbDataReader dr;
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
                    if (hdTipo.Value.Equals("01") || hdTipo.Value.Equals("04") || hdTipo.Value.Equals("08") || hdTipo.Value.Equals("06"))
                    {
                        if (hdEstado.Value.Equals("1") || hdEstado.Value.Equals("1"))
                        {
                            try
                            {
                                var idFactura = hdId.Value;
                                sql = @"SELECT idComprobante FROM Dat_General WHERE codDoc = '07' AND numDocModificado = @ID";
                                _dbe.Conectar();
                                _dbe.CrearComando(sql);
                                _dbe.AsignarParametroCadena("@ID", idFactura);
                                dr = _dbe.EjecutarConsulta();
                                var retencionAsignada = dr.HasRows && dr.Read();
                                _dbe.Desconectar();
                                if (!retencionAsignada)
                                {
                                    sql = @"SELECT ISNULL(SUM(valorRetenido), 0.00) AS valorRetenido FROM Dat_ImpuestosRetenciones WHERE numDocSustento = @ID";
                                    _dbe.Conectar();
                                    _dbe.CrearComando(sql);
                                    _dbe.AsignarParametroCadena("@ID", idFactura);
                                    dr = _dbe.EjecutarConsulta();
                                    decimal valorRetenido = 0;
                                    if (dr.Read())
                                    {
                                        decimal.TryParse(dr["valorRetenido"].ToString(), out valorRetenido);
                                    }
                                    _dbe.Desconectar();
                                    if (valorRetenido > 0)
                                    {
                                        var fecha = Localization.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                                        var cve = "04";
                                        var desc = "Servicios prestados por comisionistas";
                                        Dictionary<string, string> catImpuestos = new Dictionary<string, string>
                                        {
                                            { "ISR", "01" },
                                            { "IVA", "02" },
                                            { "IEPS", "03" },
                                            { "", "" }
                                        };
                                        sql = @"SELECT
                            r.RFCREC AS RFCEMI, r.NOMREC AS NOMEMI, r.curp AS CURPE, CASE
                                    WHEN LOWER(e.pais) LIKE 'méx%'
                                        THEN 'Nacional'
                                    WHEN LOWER(e.pais) LIKE 'mex%'
                                        THEN 'Nacional'
                                    WHEN LOWER(e.pais) = 'mx'
                                        THEN 'Nacional'
                                    ELSE
                                        'Extranjero'
                                END AS 'NacEmi', e.RFCEMI AS RFCREC, e.NOMEMI AS NOMREC, e.curp
                                 AS CURPR, RIGHT(RTRIM(MONTH(g.fecha)), 2) AS MesIni, RIGHT(RTRIM(MONTH(g.fecha)), 2) AS MesFin, YEAR(g.fecha) AS Ejerc, g.importeAPagar AS montoTotOperacion, g.total AS montoTotGrav, g.subTotal AS montoTotExent, ISNULL((SELECT SUM(ir.valorRetenido) FROM Dat_ImpuestosRetenciones ir WHERE ir.numDocSustento = g.idComprobante), 0.00) AS montoTotRet, g.codDoc, g.fecha
                        FROM
                            Dat_General g INNER JOIN
                            Cat_Emisor e ON g.id_Emisor = e.IDEEMI INNER JOIN
                            Cat_Receptor r ON g.id_Receptor = r.IDEREC
                        WHERE g.idComprobante = @ID";
                                        var txt = new SpoolMx();
                                        _dbe.Conectar();
                                        _dbe.CrearComando(sql);
                                        _dbe.AsignarParametroCadena("@ID", idFactura);
                                        dr = _dbe.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            txt.SetEmisorRet(dr["RFCEMI"].ToString(), dr["NOMEMI"].ToString(), dr["CURPE"].ToString());
                                            if (string.IsNullOrEmpty(dr["NacEmi"].ToString()) || dr["NacEmi"].ToString().Equals("Nacional", StringComparison.OrdinalIgnoreCase))
                                            {
                                                txt.SetReceptorNacRet(dr["RFCREC"].ToString(), dr["NOMREC"].ToString(), dr["CURPR"].ToString());
                                            }
                                            else
                                            {
                                                txt.SetReceptorExtRet(dr["RFCREC"].ToString(), dr["NOMREC"].ToString());
                                            }
                                            txt.SetPeriodoRet(dr["MesIni"].ToString(), dr["MesFin"].ToString(), dr["Ejerc"].ToString());
                                            txt.SetTotalesRet(dr["montoTotOperacion"].ToString(), dr["montoTotGrav"].ToString(), dr["montoTotExent"].ToString(), dr["montoTotRet"].ToString());
                                            var fechaDocModificado = Localization.Parse(dr["fecha"].ToString()).ToString("s");
                                            txt.SetComprobanteRet("", fecha, cve, desc, ddlSerie1.SelectedItem.Text, dr["codDoc"].ToString(), idFactura, fechaDocModificado);
                                        }
                                        _dbe.Desconectar();
                                        sql = "SELECT tipo as impuesto, baseImponible as base, valorRetenido as importe FROM Dat_ImpuestosRetenciones WHERE numDocSustento = @ID";
                                        _dbe.Conectar();
                                        _dbe.CrearComando(sql);
                                        _dbe.AsignarParametroCadena("@ID", idFactura);
                                        dr = _dbe.EjecutarConsulta();
                                        while (dr.Read())
                                        {
                                            txt.AgregaImpuestoRet(dr["base"].ToString(), catImpuestos[dr["impuesto"].ToString()], dr["importe"].ToString(), "Pago definitivo");
                                        }
                                        _dbe.Desconectar();
                                        var ambiente = false;
                                        switch (hfAmbiente1.Value)
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
                                        var coreMx = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                                        var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, "07", false, true, "", "");
                                        if (result != null)
                                        {
                                            procesadas++;
                                        }
                                        else
                                        {
                                            mensajes += "La retencipon del comprobante \"" + hdSerFol.Value + "\" no se pudo generar: " + coreMx.ObtenerMensaje() + "." + Environment.NewLine;
                                        }
                                    }
                                    else
                                    {
                                        mensajes += "El comprobante \"" + hdSerFol.Value + "\" no tiene retenciones." + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    mensajes += "El comprobante \"" + hdSerFol.Value + "\" ya tiene retención vinculada." + Environment.NewLine;
                                }
                            }
                            catch (Exception ex)
                            {
                                mensajes += "La retencipon del comprobante \"" + hdSerFol.Value + "\" no se pudo generar: " + ex.Message + "." + Environment.NewLine;
                            }
                        }
                        else
                        {
                            mensajes += "El comprobante \"" + hdSerFol.Value + "\" no está autorizado." + Environment.NewLine;
                        }
                    }
                    else
                    {
                        mensajes += "El comprobante \"" + hdSerFol.Value + "\" es " + hdTipo.Value + ", solo se pueden crear retenciones a facturas, notas de crédito, nóminas y cartas porte." + Environment.NewLine;
                    }
                }
            }
            Buscar();
            if (!string.IsNullOrEmpty(mensajes))
            {
                var msg = ("<ul style='padding-left:10px;'><li>" + mensajes.Replace(Environment.NewLine, "</li><li>") + "</li></ul>").Replace("<li></li>", "");
                (Master as SiteMaster).MostrarAlerta(this, "Se procesaron " + procesadas + " de " + totales + " comprobantes seleccionados." + Environment.NewLine + msg, 2, null, "bootbox.hideAll();$('#ModuloRetencion').modal('hide');", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "Se procesaron todos los comprobantes seleccionados", 2, null, "bootbox.hideAll();$('#ModuloRetencion').modal('hide');", "$('#" + progressCrear.ClientID + "').css('display', 'none');");
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
                Response.Redirect("~/DocumentosRecepcion.aspx", true);
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
            //if (ddlEstado.SelectedIndex != 0)
            //{
            //    if (_consulta.Length != 0)
            //    {
            //        _consulta = _consulta + "ED" + ddlEstado.SelectedValue + _separador;
            //    }
            //    else
            //    {
            //        _consulta = "ED" + ddlEstado.SelectedValue + _separador;
            //    }
            //}
            if (!ddlPtoEmi.SelectedValue.Equals("0"))
            {
                if (_consulta.Length != 0)
                {
                    _consulta = _consulta + "PO" + ddlPtoEmi.SelectedValue + _separador;
                }
                else
                {
                    _consulta = "PO" + ddlPtoEmi.SelectedValue + _separador;
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
            if (tbFechaInicialR.Text.Length > 5 && tbFechaFinalR.Text.Length > 5)
            {
                if (tbFechaInicialR.Text.Length > 5)
                {
                    if (_consulta.Length != 0)
                    {
                        _consulta = _consulta + "UA" + Convert.ToDateTime(tbFechaInicialR.Text).ToString("yyyyMMdd") + _separador;
                    }
                    else
                    {
                        _consulta = "UA" + Convert.ToDateTime(tbFechaInicialR.Text).ToString("yyyyMMdd") + _separador;
                    }
                    fecha = 1;
                }

                if (tbFechaFinalR.Text.Length > 5)
                {
                    if (_consulta.Length != 0)
                    {
                        _consulta = _consulta + "UF" + Convert.ToDateTime(tbFechaFinalR.Text).ToString("yyyyMMdd") + _separador;
                    }
                    else
                    {
                        _consulta = "UF" + Convert.ToDateTime(tbFechaFinalR.Text).ToString("yyyyMMdd") + _separador;
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
            LlenarGrid();
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
            tbFechaInicial.Text = "";
            tbFechaFinal.Text = "";
            tbFechaInicialR.Text = "";
            tbFechaFinalR.Text = "";
            ddlEstado.SelectedValue = "00";
            tbRFC.Text = "";
            tbNombre.Text = "";

            Buscar();
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
        /// Handles the Click2 event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click2(object sender, EventArgs e)
        {
            _dbe.Conectar();
            _dbe.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio from Par_ParametrosSistema");
            var dr1 = _dbe.EjecutarConsulta();

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
            _dbe.Desconectar();

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
                var hdSeleccionarOrden = (HiddenField)row.FindControl("checkHdORDEN");
                var rutaOrden = hdSeleccionarOrden.Value;
                //var lLabel2 = (Label)row.FindControl("Label2"); //RFCREC
                //var lLabel5 = (Label)row.FindControl("Label5"); //FOLFAC
                //var lLabel1 = (Label)row.FindControl("Label1"); //FECHA
                var RFCREC = "";
                var FOLFAC = "";
                var FECHA = "";
                var uuid = "";
                var serie = "";
                var codDoc = "";
                var fechaAutorizacion = "";
                var hdId = (HiddenField)row.FindControl("checkHdID"); //ID
                if (!chkSeleccionar.Checked)
                {
                    continue;
                }
                _dbr.Conectar();
                _dbr.CrearComando(@"SELECT numeroAutorizacion AS UUID, folio, fecha, fechaAutorizacion, serie, codDoc, RFCREC FROM Dat_General INNER JOIN Cat_Receptor ON id_Receptor = IDEREC where  idComprobante=@idefac");
                _dbr.AsignarParametroCadena("@idefac", hdId.Value);
                var dr = _dbr.EjecutarConsulta();
                if (dr.Read())
                {
                    uuid = dr["UUID"].ToString();
                    FOLFAC = dr["folio"].ToString();
                    FECHA = dr["fecha"].ToString();
                    serie = dr["serie"].ToString();
                    codDoc = dr["codDoc"].ToString();
                    RFCREC = dr["RFCREC"].ToString();
                    fechaAutorizacion = dr["fechaAutorizacion"].ToString();
                }
                _dbr.Desconectar();
                bChk = true;
                var emails = "";
                if (chkReglas.Checked)
                {
                    _dbe.Conectar();
                    _dbe.CrearComando("select emailsRegla from Cat_EmailsReglas  where Receptor=@rfcrec and estadoRegla=1");
                    _dbe.AsignarParametroCadena("@rfcrec", RFCREC);
                    var dr3 = _dbe.EjecutarConsulta();
                    if (dr3.Read())
                    {
                        emails = dr3[0].ToString();
                    }
                    _dbe.Desconectar();
                }
                emails = tbEmail.Text + "," + emails;
                emails = emails.Trim();
                emails = emails.Trim(',');

                if (emails.Length > 15)
                {
                    _em = new SendMail();
                    _em.ServidorSmtp(_servidor, _puerto, _ssl, _emailCredencial, _passCredencial);
                    if (checkPDF.Checked)
                    {
                        var fileName = _rutaDoc + hdSeleccionaPdf.Value.ToString().Replace(@"docus\" + (Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "") + "/", "").Replace(@"docus\", "");
                        if (File.Exists(fileName))
                        {
                            _em.Adjuntar(fileName);
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
                    if (checkORDEN.Checked)
                    {
                        var fileName = _rutaDoc + rutaOrden.Replace(@"docus\" + (Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "") + "/", "").Replace(@"docus\", "");
                        if (File.Exists(fileName))
                        {
                            _em.Adjuntar(fileName);
                        }
                    }
                    if (checkADICIONALES.Checked)
                    {
                        _dbr.Conectar();
                        _dbr.CrearComando("SELECT path FROM Dat_ArchivosAdicionales WHERE idComprobante = @id");
                        _dbr.AsignarParametroCadena("@id", hdId.Value);
                        dr = _dbr.EjecutarConsulta();
                        while (dr.Read())
                        {
                            var path = dr["path"].ToString();
                            var fileName = Path.GetFileName(path);
                            if (File.Exists(fileName))
                            {
                                _em.Adjuntar(fileName);
                            }
                        }
                    }
                    var emir = "";
                    _dbe.Conectar();
                    _dbe.CrearComando(@"SELECT nombre,mensaje FROM Cat_Mensajes where nombre='MensajePortalWebRecepcion' ");
                    var drSum = _dbe.EjecutarConsulta();
                    if (drSum.Read())
                    {
                        mensaje = drSum["mensaje"].ToString();
                    }
                    _dbe.Desconectar();

                    _dbr.Conectar();
                    _dbr.CrearComando(@"SELECT NOMEMI FROM Cat_Emisor WHERE RFCEMI = @RFC");
                    _dbr.AsignarParametroCadena("@RFC", Session["IDENTEMI"].ToString());
                    var dRemi = _dbr.EjecutarConsulta();
                    if (dRemi.Read())
                    {
                        emir = dRemi[0].ToString();
                    }
                    _dbr.Desconectar();

                    #region Remplazar info
                    ////mensaje = mensaje.Replace("@FechaRecepcion", fecharec);
                    //mensaje = mensaje.Replace("@NumDocumento", lLabel5.Text);
                    //mensaje = mensaje.Replace("@FechaEmision", lLabel1.Text);
                    ////mensaje = mensaje.Replace("@FechaAutorizacion", fechaAutorizacion);
                    ////mensaje = mensaje.Replace("@NumeroAutorizacion", numeroAutorizacion);


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



                    #endregion

                    var asunto = "Documento electrónico No: " + FOLFAC + " de " + emir + "";
                    //                        mensaje = @"Estimado(a) cliente;  <br>
                    //                          Acaba de recibir su documento electrónico generado el " + l_Label1.Text + @"<br>
                    //                          con folio No: " + l_Label5.Text + ".";
                    //                        mensaje += "<br><br>Saludos cordiales, ";
                    //                        mensaje += "<br>Hotel DeCameron Ecuador, ";
                    //                        mensaje += "<br><br>Servicio proporcionado por DataExpress Latinoamerica";

                    _em.LlenarEmail(_emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                    try
                    {
                        _em.ReemplazarVariable("@TipoDocumento", tipoDoc);
                        _em.ReemplazarVariable("@Serie", serie);
                        _em.ReemplazarVariable("@Folio", FOLFAC);
                        _em.ReemplazarVariable("@NumeroAutorizacion", uuid);
                        _em.ReemplazarVariable("@FechaEmision", FECHA);
                        _em.ReemplazarVariable("@FechaAutorizacion", fechaAutorizacion);
                        _em.EnviarEmail();
                        //lbMsgZip.Text = "E-Mail enviado";
                        var sql = @"INSERT INTO Cat_emailEnvio
                                           (emailEnviado
                                           ,fechaEnvio
                                           ,id_General)
                                     VALUES
                                           (@emailEnviado
                                           ,@fechaEnvio
                                           ,@id_General)";
                        _dbe.Conectar();
                        _dbe.CrearComando(sql);
                        _dbe.AsignarParametroCadena("@emailEnviado", emails.Trim(','));
                        _dbe.AsignarParametroCadena("@fechaEnvio", Localization.Now.ToString("s"));
                        _dbe.AsignarParametroCadena("@id_General", hdId.Value);
                        _dbe.EjecutarConsulta1();
                        _dbe.Desconectar();
                        estadoEnvio &= true;
                    }
                    catch (Exception ex)
                    {
                        var metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        RegLog(ex.Message, metodo, ex.ToString());
                        //lbMsgZip.Text = ex.Message;
                        estadoEnvio &= false;
                        mailsFallidos += "<li>" + FOLFAC + ": " + ex.Message + "</li>";
                        //mostrarAlerta(ex.Message, "Error");
                    }
                }
                else
                {
                    //lbMsgZip.Text = "Tienes seleccionar algún E-Mail";
                    (Master as SiteMaster).MostrarAlerta(this, "Tienes que seleccionar algún E-Mail", 4, null);
                    break;
                }
            }
            mailsFallidos += "</ul>";
            if (bChk)
            {
                (Master as SiteMaster).MostrarAlerta(this, estadoEnvio ? "Se enviaron todos los Emails" : "No se pudieron enviar los siguientes comprobantes:" + Environment.NewLine + Environment.NewLine + mailsFallidos, estadoEnvio ? 2 : 4, null);
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
            _cantidad = gvFacturas.Rows.Count;
            _seleccionados = new string[_cantidad];
            var bChk = false;
            using (ZipFile zip = new ZipFile())
            {
                foreach (GridViewRow row in gvFacturas.Rows)
                {
                    var chkSeleccionar = (CheckBox)row.FindControl("check");
                    var hdSeleccionaPdf = (HiddenField)row.FindControl("checkHdPDF");
                    var hdSeleccionarXml = (HiddenField)row.FindControl("checkHdXML");
                    var hdSeleccionarOrden = (HiddenField)row.FindControl("checkHdORDEN");
                    var hdId = (HiddenField)row.FindControl("checkHdID");
                    if (chkSeleccionar.Checked)
                    {
                        bChk = true;
                        var rutaPdf = System.AppDomain.CurrentDomain.BaseDirectory + hdSeleccionaPdf.Value;
                        var rutaXml = System.AppDomain.CurrentDomain.BaseDirectory + hdSeleccionarXml.Value;
                        var rutaOrden = System.AppDomain.CurrentDomain.BaseDirectory + hdSeleccionarOrden.Value;
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
                        if (checkORDEN.Checked && File.Exists(rutaOrden))
                        {
                            try
                            {
                                zip.AddItem(System.AppDomain.CurrentDomain.BaseDirectory + hdSeleccionarOrden.Value, "");
                            }
                            catch { }
                        }
                        if (checkADICIONALES.Checked)
                        {
                            _dbr.Conectar();
                            _dbr.CrearComando("SELECT path FROM Dat_ArchivosAdicionales WHERE idComprobante = @id");
                            _dbr.AsignarParametroCadena("@id", hdId.Value);
                            var dr = _dbr.EjecutarConsulta();
                            while (dr.Read())
                            {
                                var path = dr["path"].ToString();
                                var fileName = Path.GetFileName(path);
                                try
                                {
                                    zip.AddItem(System.AppDomain.CurrentDomain.BaseDirectory + path, "");
                                }
                                catch { }
                            }
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
                    var fileName = "FacturasRecepcion_" + Localization.Now.ToString("ddMMyyyy") + ".zip";
                    ScriptManager.RegisterStartupScript(this, GetType(), "_btnZipkey", "downloadBytes('" + base64 + "','" + contentType + "','" + fileName + "', false);", true);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Debes seleccionar una factura", 4, null);
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
            DbDataReader dr;
            var estado = "";
            var codigoControl = "";
            var pdf = "";
            var rutaCodigoControl = "";
            var idComprobante = "";
            var rutaDocus = "";
            var rutaPdf = "";
            var uuid = "";
            _dbe.Conectar();
            _dbe.CrearComando(@"select dirdocs from Par_ParametrosSistema");

            dr = _dbe.EjecutarConsulta();
            if (dr.Read())
            {
                rutaDocus = dr[0].ToString().Trim() + @"\recepcion\";
            }
            _dbe.Desconectar();

            rutaCodigoControl = btn.CommandArgument;
            var parametros = rutaCodigoControl.Split(';');
            pdf = parametros[0];
            codigoControl = parametros[1];
            estado = parametros[2];
            idComprobante = parametros[3];
            uuid = parametros[4];
            pdf = pdf.Replace(@"docus\recepcion\", "").Replace("docus//recepcion//", "").Replace("docus/recepcion/", "");
            rutaPdf = rutaDocus + pdf;
            rutaPdf = rutaPdf.Replace("/", @"\").Replace(@"\\", @"\").Replace("//", @"\");
            if (pdf != null && File.Exists(rutaPdf))
            {
                var urlRedirect = "~/download.aspx?file=" + parametros[0];
                Response.Redirect(urlRedirect);
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El PDF no existe", 4, null);
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
        /// Handles the Selected event of the SqlDataSource1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlDataSourceStatusEventArgs" /> instance containing the event data.</param>
        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            lCount.Visible = true;
            lCount.Text = "Se encontraron <span class='badge'>" + (e.AffectedRows - _hiddenRows) + "</span> Registros ";
        }

        /// <summary>
        /// Handles the Click event of the bBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscar_Click(object sender, EventArgs e)
        {
            var visible = gvFacturas.Columns[12].Visible;
            if (visible)
            {
                gvFacturas.DataSource = null;
                gvFacturas.DataBind();
                _dtActual = new DataTable();
                Session["gvComprobantes"] = _dtActual;
                bValidar_Click(null, null);
            }
            else
            {
                Buscar();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "_bBuscar_Click_CerrarFiltros", "$('#FiltrosC2').modal('hide');", true); ;
        }
        /// <summary>
        /// Handles the Click event of the bBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void PanelGrupo_Clic(object sender, EventArgs e)
        {
            var val = hfIdUpdateTipoProveedor.Value;
            var splitted = val.Split(',');
            var idComprobante = splitted[0];
            var idgrupo = splitted[1];
            ModTipoProv.SelectedValue = idgrupo;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "$('#EditarTipoProveedor').modal('show'); ", true);
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
        /// <exception cref="Exception">EXEC    [dbo].[PA_facturas_basico]
        ///         @QUERY = N'" + _consulta.Replace("'", "''") + @"',
        ///         @RFC = N'" + Session["rfcCliente"].ToString().Replace("'", "''") + @"',
        ///         @ROL = " + (bool.Parse(_aux) ? "1" : "0") + @",
        ///         @empleado = N'" + Session["USERNAME"].ToString().Replace("'", "''") + @"',
        ///         @TOP = N'" + topCopm.Replace("'", "''") + @"',
        ///         @CNSptoemi = " + aux3.Replace("'", "''") + @",
        ///         @PTOEMII = N'1',
        ///         @CNSComp = N'" + tipoComp.Replace("'", "''") + @"',
        ///         @QUERYSTRING = N'',
        ///         @DBEMISION = N'" + _dbe.DatabaseSchema.Replace("'", "''") + @"'</exception>
        private void LlenarGrid()
        {
            gvFacturas.DataSource = null;
            gvFacturas.DataBind();
            _dtActual = new DataTable();
            lMensaje.Text = "";
            var queryString = "";

            var aux3 = "";
            var tipoComp = "";
            var topCopm = "''";
            if (Session["TOPComp"] != null)
            {
                topCopm = Session["TOPComp"].ToString();
            }
            if (Convert.ToBoolean(Session["coFactTodas"]))
            {
                _aux = "true";
                aux3 = "false";
            }
            else
            {
                _aux = "false";
            }

            if (Convert.ToBoolean(Session["coFactPropias"]))
            {
                if (Convert.ToBoolean(Session["coFactPropiasPtoEmi"]))
                {
                    aux3 = "true";
                }
                else
                {
                    aux3 = "false";
                }
            }
            else
            {
                aux3 = "false";
            }

            if (Session["CNSComp"] != null)
            {
                if (!string.IsNullOrEmpty(Session["CNSComp"].ToString()))
                {
                    if (Session["CNSComp"].ToString().Contains("01"))
                    {
                        tipoComp = tipoComp.Trim(',') + "'01'";
                    }
                    if (Session["CNSComp"].ToString().Contains("04"))
                    {
                        tipoComp = tipoComp.Trim(',') + ",'04'";
                    }
                    if (Session["CNSComp"].ToString().Contains("05"))
                    {
                        tipoComp = tipoComp.Trim(',') + ",'05'";
                    }
                    if (Session["CNSComp"].ToString().Contains("06"))
                    {
                        tipoComp = tipoComp.Trim(',') + ",'06'";
                    }
                    if (Session["CNSComp"].ToString().Contains("07"))
                    {
                        tipoComp = tipoComp.Trim(',') + ",'07'";
                    }
                    tipoComp = tipoComp.Trim(',') + ",'08'";
                    tipoComp = tipoComp.Trim(',') + ",'09'";

                    if (!string.IsNullOrEmpty(tipoComp))
                    {
                        tipoComp = "Dat_General.codDoc in (" + tipoComp.Trim(',') + ") ";
                    }
                }
            }
            if (ddlEstado.SelectedIndex != 0)
            {
                queryString = "Dat_General.estadoValidacion = '" + ddlEstado.SelectedValue + "'";
            }
            try
            {
                _dbr.Conectar();
                _dbr.CrearComandoProcedimiento("PA_facturas_basico");
                _dbr.AsignarParametroProcedimiento("@QUERY", DbType.String, _consulta);
                _dbr.AsignarParametroProcedimiento("@RFC", DbType.String, Session["rfcCliente"].ToString());
                _dbr.AsignarParametroProcedimiento("@ROL", DbType.Boolean, _aux);
                _dbr.AsignarParametroProcedimiento("@empleado", DbType.String, Session["USERNAME"].ToString());
                _dbr.AsignarParametroProcedimiento("@TOP", DbType.String, topCopm);
                _dbr.AsignarParametroProcedimiento("@CNSptoemi", DbType.String, aux3);
                _dbr.AsignarParametroProcedimiento("@PTOEMII", DbType.String, "1");
                _dbr.AsignarParametroProcedimiento("@CNSComp", DbType.String, tipoComp);
                _dbr.AsignarParametroProcedimiento("@QUERYSTRING", DbType.String, queryString);
                _dbr.AsignarParametroProcedimiento("@DBEMISION", DbType.String, _dbe.DatabaseSchema);
                //DB.AsignarParametroProcedimiento("@PTOEMI", System.Data.DbType.String, completaceros(ptoEMi));
                var sql = @"EXEC    [dbo].[PA_facturas_basico]
        @QUERY = N'" + _consulta.Replace("'", "''") + @"',
        @RFC = N'" + Session["rfcCliente"].ToString().Replace("'", "''") + @"',
        @ROL = " + (bool.Parse(_aux) ? "1" : "0") + @",
        @empleado = N'" + Session["USERNAME"].ToString().Replace("'", "''") + @"',
        @TOP = N'" + topCopm.Replace("'", "''") + @"',
        @CNSptoemi = " + aux3.Replace("'", "''") + @",
        @PTOEMII = N'1',
        @CNSComp = N'" + tipoComp.Replace("'", "''") + @"',
        @QUERYSTRING = N'" + queryString + @"',
        @DBEMISION = N'" + _dbe.DatabaseSchema.Replace("'", "''") + @"'";
                _dtActual.Load(_dbr.EjecutarConsulta());
                _dbr.Desconectar();
                Session["gvComprobantes"] = _dtActual;
                gvFacturas.DataSource = _dtActual;
                gvFacturas.DataBind();
                lCount.Text = "Se encontraron <span class='badge'>" + (_dtActual.Rows.Count - _hiddenRows) + "</span> Registros ";
                gvFacturas.Columns[10].Visible = Convert.ToBoolean(Session["_isWorkflow"]);
                gvFacturas.Columns[11].Visible = Convert.ToBoolean(Session["_isWorkflow"]);
                gvFacturas.Columns[12].Visible = false;
                gvFacturas.Columns[14].Visible = (bool)Session["statusDistribuidor"];
                var columnaEstructura = gvFacturas.Columns.Cast<DataControlField>().FirstOrDefault(c => c.HeaderText.Equals("ESTRUCTURA VALIDACIÓN", StringComparison.OrdinalIgnoreCase));
                if (columnaEstructura != null)
                {
                    columnaEstructura.Visible = Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@"EXEC  [dbo].[PA_facturas_basico]
        @QUERY = N'" + _consulta.Replace("'", "''") + @"',
        @RFC = N'" + Session["rfcCliente"].ToString().Replace("'", "''") + @"',
        @ROL = " + (bool.Parse(_aux) ? "1" : "0") + @",
        @empleado = N'" + Session["USERNAME"].ToString().Replace("'", "''") + @"',
        @TOP = N'" + topCopm.Replace("'", "''") + @"',
        @CNSptoemi = " + aux3.Replace("'", "''") + @",
        @PTOEMII = N'1',
        @CNSComp = N'" + tipoComp.Replace("'", "''") + @"',
        @QUERYSTRING = N'',
        @DBEMISION = N'" + _dbe.DatabaseSchema.Replace("'", "''") + @"'", ex);
            }
        }

        /// <summary>
        /// Nivels the validacion.
        /// </summary>
        /// <returns>System.String.</returns>
        private string NivelValidacion()
        {
            var nivel = "0";
            if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
            {
                _dbe.Conectar();
                _dbr.Conectar();
                _dbe.CrearComando("SELECT g.idGrupo FROM Cat_Empleados e INNER JOIN " + _dbr.DatabaseSchema + ".dbo.Cat_Grupos_Validadores g ON g.idGrupo = e.idGrupo WHERE e.idEmpleado = @id");
                _dbe.AsignarParametroCadena("@id", Session["idUser"].ToString());
                var dr = _dbe.EjecutarConsulta();
                if (dr.Read())
                {
                    nivel = dr["idGrupo"].ToString();
                }
            }
            else
            {
                _dbe.Conectar();
                _dbe.CrearComando("SELECT g.orden FROM Cat_Empleados e INNER JOIN " + _dbr.DatabaseSchema + ".dbo.Cat_Grupos_Validadores g ON g.idGrupo = e.idGrupo WHERE e.idEmpleado = @id");
                _dbe.AsignarParametroCadena("@id", Session["idUser"].ToString());
                var dr = _dbe.EjecutarConsulta();
                if (dr.Read())
                {
                    nivel = dr["orden"].ToString();
                }
            }
            return nivel;
        }

        private string IdToOrder(string idGroup)
        {
            var order = idGroup;
            try
            {
                _dbr.Conectar();
                _dbr.CrearComando("SELECT DISTINCT TOP 1 orden FROM Cat_Grupos_validadores WHERE idGrupo = @idGroup");
                _dbr.AsignarParametroCadena("@idGroup", idGroup);
                var dr = _dbr.EjecutarConsulta();
                while (dr.Read())
                {
                    order = dr["orden"].ToString();
                }
                _dbr.Desconectar();
            }
            catch (Exception ex)
            {
                // ignored
            }
            return order;
        }

        private string OrderToId(string order)
        {
            var id = order;
            try
            {
                _dbr.Conectar();
                _dbr.CrearComando("SELECT DISTINCT TOP 1 idGrupo FROM Cat_Grupos_validadores WHERE orden = @orden");
                _dbr.AsignarParametroCadena("@orden", order);
                var dr = _dbr.EjecutarConsulta();
                while (dr.Read())
                {
                    id = dr["idGrupo"].ToString();
                }
                _dbr.Desconectar();
            }
            catch (Exception ex)
            {
                // ignored
            }
            return id;
        }

        /// <summary>
        /// Builds the table.
        /// </summary>
        /// <param name="validadoPor">The validado por.</param>
        /// <returns>System.String.</returns>
        private string BuildTable(string validadoPor)
        {
            var validado = validadoPor.Trim("<br/>".ToCharArray()).Trim(',', ' ');
            var status = "<table class='table table-condensed table-responsive'><thead><tr style='color:White;background-color:#4580A8;font-weight:bold;'><th scope='col' style='text-align: center;'>Estado</th><th scope='col' style='text-align: center;'>Usuario</th><th scope='col' style='text-align: center;'>Fecha Validación</th><th scope='col' style='text-align: center;'>Observaciones</th><th scope='col' style='text-align: center;'>Nivel Grupo</th></tr></thead><tbody>";
            validado = validado.Replace("'", "\"");
            var validaciones = validado.Split(new string[] { ",,,,," }, StringSplitOptions.None);
            foreach (var validacion in validaciones)
            {
                var props = validacion.Split(new string[] { ", | ," }, 5, StringSplitOptions.None);
                var usuario = props[0].ToString();
                var estado = props[1].ToString();
                var observaciones = props[2].ToString();
                var fecha = props[3].ToString();
                var nivelGrupo = props[4].ToString();
                fecha = Localization.Parse(fecha).ToString("dd/MM/yyyy<br/>HH:mm:ss");
                status += "<tr><td scope='col' style='text-align: center;'>" + (estado.Equals("1") ? "Aprobado" : (estado.Equals("0") ? "No Aprobado/Rechazado" : "Desconocido")) + "</td><td scope='col' style='text-align: center;'>" + usuario + "</td><td scope='col' style='text-align: center;'>" + fecha + "</td><td scope='col' style='text-align: center;'>" + observaciones + "</td><td scope='col' style='text-align: center;'>" + nivelGrupo + "</td></tr>";
            }
            status += "</tbody></table>";
            return status;
        }

        /// <summary>
        /// Encodes the file to base64.
        /// </summary>
        /// <param name="fileToEncode">The file to encode.</param>
        /// <returns>System.String.</returns>
        private string EncodeFileToBase64(string fileToEncode)
        {
            var bytes = File.ReadAllBytes(fileToEncode);
            return Convert.ToBase64String(bytes);
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

                #region Observaciones

                try
                {
                    var lb = (HyperLink)(e.Row.FindControl("lbPopupObs"));
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var observaciones = row.Field<string>("observaciones");
                    try { observaciones = observaciones.Trim("<br/>".ToCharArray()); } catch { }
                    if (!string.IsNullOrEmpty(observaciones))
                    {
                        var nameScaped = observaciones.Replace("'", "\"");
                        lb.Attributes["data-content"] = (!string.IsNullOrEmpty(observaciones) ? (observaciones + "<br/>") : "") + "<div align='right'><button type='button' id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
                    }
                }
                catch { }

                #endregion
                #region Validaciones WorkFlow

                try
                {
                    var radios = (RadioButtonList)(e.Row.FindControl("valid"));
                    var tbObs = (TextBox)(e.Row.FindControl("tbValidObs"));
                    var lb = (HyperLink)(e.Row.FindControl("lbPopupVal"));
                    var lb2 = (HyperLink)(e.Row.FindControl("lbPopupVa2"));
                    var IdTipDis = (Button)(e.Row.FindControl("BtnDistr"));
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var id = ((HiddenField)(e.Row.FindControl("checkHdID"))).Value;
                    var validado = row.Field<string>("validadoPor");
                    var status = "";
                    var EstrucValid = "";
                    var idTipo = "";
                    var actualGroup = int.Parse(NivelValidacion());
                    var minimo = 0;
                    List<int> arrayOrden = null;
                    DbDataReader dr = null;
                    if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                    {
                        var ordenIdGruposValidacion = "";
                        var idTipoproveedor = "";
                        _dbr.Conectar();
                        _dbr.CrearComando("SELECT e.ordenIdGruposValidacion,P.nombre,P.IdTipo,g.id_Tipoproveedor FROM Cat_EstructuraValidacion e INNER JOIN Cat_TiposProveedor tp ON e.id_TipoProveedor = tp.IdTipo INNER JOIN Dat_General g ON g.id_TipoProveedor = tp.IdTipo INNER JOIN Cat_TiposProveedor P ON g.id_TipoProveedor = p.IdTipo WHERE g.idComprobante = @id");
                        _dbr.AsignarParametroCadena("@id", id);
                        dr = _dbr.EjecutarConsulta();
                        if (dr.Read())
                        {
                            ordenIdGruposValidacion = dr[0].ToString();
                            nameEstruc = dr[1].ToString();
                            idTipo = dr[2].ToString();
                            idTipoproveedor = dr[3].ToString();
                        }
                        _dbr.Desconectar();
                        if (Session["USERNAME"].ToString().StartsWith("EMPLE"))
                        {
                            if (string.IsNullOrEmpty(idTipo) && !(bool)Session["statusDistribuidor"])
                            {
                                e.Row.Visible = false;
                                return;
                            }
                            else if ((bool)Session["statusDistribuidor"] && string.IsNullOrEmpty(validado))
                            {
                                e.Row.Visible = true;
                                return;
                            }
                        }
                        if (string.IsNullOrEmpty(idTipoproveedor.ToString()))
                        {
                            IdTipDis.Visible = true;
                        }
                        else
                        {
                            IdTipDis.Visible = false;
                            //return;
                        }
                        var arrayOrdenString = ordenIdGruposValidacion.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        arrayOrden = arrayOrdenString.Select(int.Parse).ToList();
                        minimo = arrayOrden.First();
                        if (ordenIdGruposValidacion != "")
                        {
                            foreach (var grupo in arrayOrdenString)
                            {
                                _dbr.Conectar();
                                _dbr.CrearComando(@"select descripcion from Cat_Grupos_validadores where idGrupo in (@orden)");
                                _dbr.AsignarParametroCadena("@orden", grupo);
                                dr = _dbr.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    EstrucValid += dr[0].ToString() + ",";
                                }
                                _dbr.Desconectar();
                            }
                        }
                    }
                    else
                    {
                        _dbr.Conectar();
                        _dbr.CrearComando("select MIN(orden) from Cat_Grupos_validadores");
                        dr = _dbr.EjecutarConsulta();
                        if (dr.Read())
                        {
                            int.TryParse(dr[0].ToString(), out minimo);
                        }
                        _dbr.Desconectar();
                    }

                    if (!string.IsNullOrEmpty(validado))
                    {
                        validado = validado.Trim("<br/>".ToCharArray()).Trim(',', ' ');
                        status = "<table class='table table-condensed table-responsive'><thead><tr style='color:White;background-color:#4580A8;font-weight:bold;'><th scope='col' style='text-align: center;'>Estado</th><th scope='col' style='text-align: center;'>Usuario</th><th scope='col' style='text-align: center;'>Fecha Validación</th><th scope='col' style='text-align: center;'>Observaciones</th>" + (!Session["USERNAME"].ToString().StartsWith("PROVE", StringComparison.OrdinalIgnoreCase) ? "<th scope='col' style='text-align: center;'>Grupo</th>" : "") + "</tr></thead><tbody>";
                        validado = validado.Replace("'", "\"");
                        bool validador = false;
                        bool.TryParse(Session["validacionRecepcion"].ToString(), out validador);
                        validador &= Convert.ToBoolean(Session["_isWorkflow"]);
                        bool alreadyValidated = false;
                        var validaciones = validado.Split(new string[] { ",,,,," }, StringSplitOptions.None);
                        if (!Session["USERNAME"].ToString().StartsWith("PROVE", StringComparison.OrdinalIgnoreCase))
                        {
                            var idGroup = actualGroup;
                            if (!Session["IDENTEMI"].ToString().Equals("HIM890120VEA") && !Session["IDENTEMI"].ToString().Equals("SIH071204N90") && !Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                            {
                                int.TryParse(OrderToId(actualGroup.ToString()), out idGroup);
                            }
                            var alreadyValidatedGroup = !string.IsNullOrEmpty(validaciones.FirstOrDefault(x => x.Split(new string[] { ", | ," }, 5, StringSplitOptions.None)[4].ToString().Contains(idGroup.ToString())));
                            var alreadyValidatedUser = !string.IsNullOrEmpty(validaciones.FirstOrDefault(x => x.Split(new string[] { ", | ," }, 5, StringSplitOptions.None)[0].ToString().Contains(Session["USERNAME"].ToString())));
                            if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                            {
                                // HERBALIFE Y DEMO
                                alreadyValidated = alreadyValidatedUser;
                            }
                            else
                            {
                                // TODOS LOS DEMAS CLIENTES
                                alreadyValidated = alreadyValidatedGroup;
                            }
                        }
                        if (!alreadyValidated)
                        {
                            radios.SelectedValue = "2";
                            tbObs.ReadOnly = false;
                        }
                        foreach (var validacion in validaciones)
                        {
                            var props = validacion.Split(new string[] { ", | ," }, 5, StringSplitOptions.None);
                            var usuario = props[0].ToString();
                            var estado = props[1].ToString();
                            var observaciones = props[2].ToString();
                            var fecha = props[3].ToString();
                            var idGrupo = "";
                            var nombreGrupo = "";
                            if (!Session["USERNAME"].ToString().StartsWith("PROVE", StringComparison.OrdinalIgnoreCase))
                            {
                                idGrupo = props[4].ToString();
                                _dbr.Conectar();
                                _dbr.CrearComando("SELECT DISTINCT descripcion FROM Cat_Grupos_validadores WHERE idGrupo = @idGrupo");
                                _dbr.AsignarParametroCadena("@idGrupo", idGrupo);
                                dr = _dbr.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    nombreGrupo = dr["descripcion"].ToString();
                                }
                                else
                                {
                                    nombreGrupo = idGrupo;
                                }
                                _dbr.Desconectar();
                            }
                            fecha = Localization.Parse(fecha).ToString("dd/MM/yyyy<br/>HH:mm:ss");
                            var img = "<img width='18px' height='18px' src='" + ResolveClientUrl(string.Format("~/Imagenes/{0}.png", estado)) + "' alt='[" + estado + "]' data-toggle='tooltip' data-placement='top' title='" + (estado.Equals("1") ? "Aprobado" : (estado.Equals("0") ? "No Aprobado/Rechazado" : "Desconocido")) + "' />";
                            status += "<tr><td scope='col' style='text-align: center;'>" + img + "</td><td scope='col' style='text-align: center;'>" + usuario + "</td><td scope='col' style='text-align: center;'>" + fecha + "</td><td scope='col' style='text-align: center;'>" + observaciones + "</td>" + (!Session["USERNAME"].ToString().StartsWith("PROVE", StringComparison.OrdinalIgnoreCase) ? ("<td scope='col' style='text-align: center;'>" + nombreGrupo + "</td>") : "") + "</tr>";
                            if ((validador && alreadyValidated) || estado.Equals("0"))
                            {
                                tbObs.Text = observaciones;
                                radios.SelectedValue = estado;
                                radios.Enabled = false;
                                tbObs.ReadOnly = true;
                            }
                        }
                        status += "</tbody></table>";
                        if (!Session["USERNAME"].ToString().StartsWith("PROVE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                            {
                                #region /* =================================== VALIDACION INDIVIDUAL */
                                var maxGroup = validaciones.Max(x => int.Parse(x.Split(new string[] { ", | ," }, 5, StringSplitOptions.None)[4].ToString()));
                                var validationsOfMaxGroup = validaciones.Count(x => int.Parse(x.Split(new string[] { ", | ," }, 5, StringSplitOptions.None)[4].ToString()) == maxGroup);
                                if (actualGroup > 0)
                                {
                                    //if (actualGroup > (maxGroup + 1))
                                    //{
                                    //    e.Row.Visible = false;
                                    //    _hiddenRows++;
                                    //}
                                    var siguiente = arrayOrden.IndexOf(actualGroup) + 1;
                                    if (siguiente >= arrayOrden.Count)
                                    {
                                        siguiente = (arrayOrden.Count - 1);
                                    }
                                    var containsBetween = (arrayOrden.IndexOf(actualGroup) - arrayOrden.IndexOf(maxGroup) > 1);
                                    var validate = false;
                                    var existeEnEstructura = false;
                                    try
                                    {
                                        //validate = actualGroup > arrayOrden[siguiente];
                                        validate = (arrayOrden.IndexOf(actualGroup) > siguiente);
                                    }
                                    catch (Exception ex)
                                    {
                                        // ignored
                                    }
                                    try
                                    {
                                        existeEnEstructura = arrayOrden.Contains(actualGroup);
                                    }
                                    catch (Exception ex)
                                    {
                                        // ignored
                                    }
                                    if (containsBetween || validate || !existeEnEstructura)
                                    {
                                        e.Row.Visible = false;
                                        _hiddenRows++;
                                    }
                                    else if (actualGroup != maxGroup)
                                    {
                                        var usersInPreviousLevel = 0;
                                        _dbe.Conectar();
                                        _dbe.CrearComando("select count(*) FROM Cat_Empleados e INNER JOIN " + _dbr.DatabaseSchema + ".dbo.Cat_Grupos_validadores g ON e.idGrupo = g.idGrupo where g.idGrupo = @orden");
                                        _dbe.AsignarParametroCadena("@orden", maxGroup.ToString());
                                        dr = _dbe.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            usersInPreviousLevel = int.Parse(dr[0].ToString());
                                        }
                                        _dbe.Desconectar();
                                        if (usersInPreviousLevel > 0 && validationsOfMaxGroup < usersInPreviousLevel && arrayOrden.IndexOf(maxGroup) < arrayOrden.IndexOf(actualGroup))
                                        {
                                            e.Row.Visible = false;
                                            _hiddenRows++;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region /* =================================== VALIDACION POR GRUPOS  */
                                var maxGroup = validaciones.Max(x => int.Parse(x.Split(new string[] { ", | ," }, 5, StringSplitOptions.None)[4].ToString()));
                                if (!Session["IDENTEMI"].ToString().Equals("HIM890120VEA") && !Session["IDENTEMI"].ToString().Equals("SIH071204N90") && !Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                                {
                                    int.TryParse(IdToOrder(maxGroup.ToString()), out maxGroup);
                                }
                                if (actualGroup > (maxGroup + 1) && actualGroup > 0)
                                {
                                    e.Row.Visible = false;
                                    _hiddenRows++;
                                }
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        radios.SelectedValue = "2";
                        tbObs.ReadOnly = false;
                        status = "El comprobante no ha sido validado aún por nadie";
                        if (minimo != actualGroup && actualGroup > 0)
                        {
                            e.Row.Visible = false;
                            _hiddenRows++;
                        }
                    }
                    lb.Attributes["data-content"] = status + "<div align='right'><button type='button' id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
                    var header = "<div class='stepwizard'><div class='stepwizard-row'>";
                    var footer = "</div></div>";
                    var body = "";
                    var btnEdtEstruc = "<div align = 'right'><button type='button' id='btnGrupo' class='btn btn-primary btn-sm' onclick='MostrarGrupo(" + id + "," + idTipo + ");'>Editar</button></div>";
                    var arrayEstructura = EstrucValid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < arrayEstructura.Length; i++)
                    {
                        body += "<div class='stepwizard-step'><button type = 'button' class='btn btn-defaulti btn-circle'>" + (i + 1) + "</button><p>" + arrayEstructura[i] + "</p></div>";
                    }
                    lb2.Attributes["data-content"] = header + body + footer + (radios.SelectedValue.Equals("2") && (bool)Session["statusDistribuidor"] && string.IsNullOrEmpty(validado) ? btnEdtEstruc : "");
                    lb2.Attributes["title"] = "ESTRUCTURA DE VALIDACIÓN<br><span class='label label-primary' style='text-align: center;'>Tipo Estructura: " + nameEstruc + "</span>";
                }
                catch (Exception ex) { }

                #endregion
                #region Boton Pagos

                try
                {
                    var id = ((HiddenField)(e.Row.FindControl("checkHdID"))).Value;
                    var btnPagos = (LinkButton)(e.Row.FindControl("btnPagos"));
                    if (!string.IsNullOrEmpty(id) && btnPagos != null)
                    {
                        _dbr.Conectar();
                        _dbr.CrearComando("SELECT p.formapago, g.metodoPago FROM Dat_General g LEFT OUTER JOIN Dat_Pagos p ON g.idComprobante = p.id_Comprobante WHERE g.idComprobante = @id");
                        _dbr.AsignarParametroCadena("@id", id);
                        var dr = _dbr.EjecutarConsulta();
                        if (dr.Read())
                        {
                            var forma = dr["formapago"].ToString();
                            var metodo = dr["metodoPago"].ToString();
                            var isComplementoPago = System.Text.RegularExpressions.Regex.IsMatch(forma, @"^(PPD|.*parcial.*|.*difer.*)$") && System.Text.RegularExpressions.Regex.IsMatch(metodo, @"^99$");
                            btnPagos.Visible = isComplementoPago;
                        }
                        _dbr.Desconectar();
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }

                #endregion
            }
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
            try
            {
                var btn = (LinkButton)sender;
                var id = btn.CommandArgument;
                var ws = new WsRecepcion();
                var respuesta = ws.CancelarComprobante(id, _idUser, Session["IDENTEMI"].ToString());
                var mensaje = ws.ObtenerMensaje();
                if (respuesta)
                {
                    Button1_Click1(null, null);
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobate ahora está en proceso de cancelación", 2, null);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se pudo cancelar<br/><br/>" + mensaje, 4, null);
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se pudo cancelar<br/><br/>" + ex, 4, null);
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
            SqlDataSourcePtoEmi.SelectParameters["tipo"].DefaultValue = ddlTipoDocumento.SelectedValue;
            SqlDataSourcePtoEmi.DataBind();
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
            var row = gvFacturas.SelectedRow;
            var img = (ImageButton)row.Cells[0].Controls[0];
            if (img != null)
            {
                img.ImageUrl = "~/imagenes/loading.gif";
            }
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

        ///// <summary>
        ///// Handles the PreRender event of the gvFacturas control.
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected void gvFacturas_PreRender(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gvFacturas.HeaderRow.TableSection = TableRowSection.TableHeader;
        //    }
        //    catch (Exception) { }
        //}

        /// <summary>
        /// Handles the Sorting event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
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
            catch (Exception ex) { }
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
            var visible = gvFacturas.Columns[12].Visible;
            if (visible)
            {
                gvFacturas.DataSource = null;
                gvFacturas.DataBind();
                _dtActual = new DataTable();
                Session["gvComprobantes"] = _dtActual;
                bValidar_Click(null, null);
            }
            else
            {
                Buscar();
            }
        }

        /// <summary>
        /// Handles the Click event of the bGuardarValidaciones control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bGuardarValidaciones_Click(object sender, EventArgs e)
        {
            GuardarValidaciones();
            bCancelarValidaciones.Visible = false;
            bGuardarValidaciones.Visible = false;
            bValidar.Visible = true;
            btnZip.Visible = true;
            bMail.Visible = true;
            LlenarGrid();
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;
            gvFacturas.DataBind();
            gvFacturas.Columns[12].Visible = false;
        }

        /// <summary>
        /// Guardars the validaciones.
        /// </summary>
        private void GuardarValidaciones()
        {
            try
            {
                string empleado = "";
                string grupo = "";
                string nivel = "";
                var idUser = Session["idUser"].ToString();
                var sql = "SELECT nombreEmpleado,descripcion,orden FROM " + _dbe.DatabaseSchema + ".dbo.Cat_Empleados emp INNER JOIN " +
                    _dbr.DatabaseSchema + ".dbo.Cat_Grupos_validadores rol ON emp.idGrupo = rol.idGrupo WHERE idEmpleado=@idEmpleado";
                _dbe.Conectar();
                _dbe.CrearComando(sql);
                _dbe.AsignarParametroCadena("@idEmpleado", idUser);
                var DR = _dbe.EjecutarConsulta();
                if (DR.Read())
                {
                    empleado = DR[0].ToString();
                    grupo = DR[1].ToString();
                    nivel = DR[2].ToString();
                }
                _dbe.Desconectar();
                var gruposList = new List<string>();
                foreach (GridViewRow row in gvFacturas.Rows)
                {
                    var radios = (RadioButtonList)(row.FindControl("valid"));
                    var valid = radios.SelectedValue;
                    var id = ((HiddenField)(row.FindControl("checkHdID"))).Value;
                    var tb = (TextBox)(row.FindControl("tbValidObs"));
                    var actualStructureState = ((HiddenField)(row.FindControl("estadoEstructura"))).Value;
                    var actualValidationState = ((HiddenField)(row.FindControl("estadoValidacion"))).Value;
                    DbDataReader dr;
                    if (row.Visible && radios.Enabled)
                    {
                        var validaciones = 0;
                        var validadores = 0;
                        var status = "";
                        if (!valid.Equals("2"))
                        {
                            if (!ExisteValidacion(id))
                            {
                                _dbr.Conectar();
                                _dbr.CrearComando("INSERT INTO Dat_Validaciones (id_Comprobante, idEmpleadoEmision, estadoValidacion, observaciones) VALUES (@id, @user, @estado, @obs)");
                                _dbr.AsignarParametroCadena("@id", id);
                                _dbr.AsignarParametroCadena("@user", idUser);
                                _dbr.AsignarParametroCadena("@estado", valid);
                                _dbr.AsignarParametroCadena("@obs", tb.Text);
                                _dbr.EjecutarConsulta1();
                                _dbr.Desconectar();
                            }
                            string fechavalidacion = DateTime.Now.ToString();
                            _dbr.Conectar();
                            _dbr.CrearComando("INSERT INTO Dat_Relacion_Validaciones (idComprobante, Empleado, Grupo, estatus,nivel,fecha_validacion) VALUES (@idComprobante, @Empleado, @Grupo, @estatus, @nivel, GETDATE())");
                            _dbr.AsignarParametroCadena("@idComprobante", id);
                            _dbr.AsignarParametroCadena("@Empleado", empleado);
                            _dbr.AsignarParametroCadena("@Grupo", grupo);
                            _dbr.AsignarParametroCadena("@estatus", valid.ToString()); // "1"
                            _dbr.AsignarParametroCadena("@nivel", nivel);
                            _dbr.EjecutarConsulta1();
                            _dbr.Desconectar();
                        }
                        try
                        {
                            _dbr.Conectar();
                            if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                            {
                                #region/* ============================= VALIDACION INDIVIDUAL */
                                //_dbr.CrearComando("SELECT (SELECT COUNT(DISTINCT idEmpleadoEmision) FROM Dat_Validaciones) AS validaciones, (SELECT COUNT(DISTINCT idEmpleado) FROM " + _dbe.DatabaseSchema + ".dbo.Cat_Empleados emp INNER JOIN " + _dbe.DatabaseSchema + ".dbo.Cat_Roles rol ON emp.id_Rol = rol.idRol WHERE rol.validacionRecepcion = '1' AND idEmpleado <> 999999999) AS validadores");
                                var grupos = "";
                                _dbr.CrearComando("SELECT e.ordenIdGruposValidacion FROM Cat_EstructuraValidacion e INNER JOIN Cat_TiposProveedor tp ON e.id_TipoProveedor = tp.IdTipo INNER JOIN Dat_General g ON g.id_TipoProveedor = tp.IdTipo  WHERE g.idComprobante = @id");
                                _dbr.AsignarParametroCadena("@id", id);
                                dr = _dbr.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    grupos = dr["ordenIdGruposValidacion"].ToString();
                                }
                                _dbr.Desconectar();
                                gruposList = grupos.Split(',').ToList();
                                grupos = "'" + grupos.Replace(",", "','") + "'";
                                _dbr.Conectar();
                                _dbr.CrearComando("SELECT (SELECT COUNT(DISTINCT idEmpleadoEmision) FROM Dat_Validaciones WHERE id_Comprobante = @id) AS validaciones, (SELECT COUNT(DISTINCT idEmpleado) FROM " + _dbe.DatabaseSchema + ".dbo.Cat_Empleados emp INNER JOIN " + _dbe.DatabaseSchema + ".dbo.Cat_Roles rol ON emp.id_Rol = rol.idRol WHERE rol.validacionRecepcion = '1' AND idEmpleado <> 999999999 AND idEmpleado <> 1 AND emp.idGrupo IN (" + grupos + ")) AS validadores");
                                _dbr.AsignarParametroCadena("@id", id);
                                #endregion
                            }
                            else
                            {
                                #region/* ============================= VALIDACION POR GRUPO */
                                _dbr.CrearComando("SELECT idGrupo FROM Cat_Grupos_Validadores ORDER BY orden");
                                dr = _dbr.EjecutarConsulta();
                                while (dr.Read())
                                {
                                    gruposList.Add(dr[0].ToString());
                                }
                                _dbr.Desconectar();
                                _dbr.Conectar();
                                _dbr.CrearComando("SELECT (SELECT ISNULL(MAX(nivel), 0) FROM Dat_Relacion_Validaciones WHERE idComprobante = @id) AS validados, (SELECT MAX(orden) FROM Cat_Grupos_validadores) AS porValidar");
                                _dbr.AsignarParametroCadena("@id", id);
                                #endregion
                            }

                            dr = _dbr.EjecutarConsulta();
                            if (dr.Read())
                            {
                                int.TryParse(dr[0].ToString(), out validaciones);
                                int.TryParse(dr[1].ToString(), out validadores);
                            }
                        }
                        catch (Exception ex) { }
                        finally
                        {
                            _dbr.Desconectar();
                        }
                        if (validadores <= 0)
                        {
                            status = "2";
                        }
                        else if (validaciones == validadores)
                        {
                            status = "1";
                        }
                        else if (validaciones < validadores)
                        {
                            status = "2";
                        }
                        if (valid.ToString().Equals("0"))
                        {
                            status = "0";
                        }
                        try
                        {
                            if (!string.IsNullOrEmpty(status))
                            {
                                _dbr.Conectar();
                                _dbr.CrearComando("UPDATE Dat_General SET estadoValidacion = @status WHERE idComprobante = @id");
                                _dbr.AsignarParametroCadena("@id", id);
                                _dbr.AsignarParametroCadena("@status", status);
                                _dbr.EjecutarConsulta1();
                            }
                        }
                        catch (Exception ex) { }
                        EnviarCorreoValidacion(valid, id, grupo, gruposList);
                    }
                }
                (Master as SiteMaster).MostrarAlerta(this, "Se actualizaron los estados de validacion de los comprobantes seleccionados", 2);
            }
            catch (Exception ex)
            {
                var metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                RegLog("No se pudieron validar algunos comprobantes, inténtelo de nuevo: " + ex.Message, metodo, ex.ToString());
                (Master as SiteMaster).MostrarAlerta(this, "No se pudieron validar algunos comprobantes, inténtelo de nuevo.<br/><br/>" + ex.Message, 4);
            }
        }

        /// <summary>
        /// Enviars the correo validacion.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="grupo">The grupo.</param>
        /// <param name="gruposList">The grupos list.</param>
        private void EnviarCorreoValidacion(string status, string id, string grupo, List<string> gruposList)
        {
            if (status.Equals("2"))
            {
                return;
            }
            var _em = new SendMail();
            var mails = "";
            var mensaje = "";
            var asunto = "";
            var actualGroup = int.Parse(NivelValidacion());
            var ordenPlus = actualGroup + 1;
            if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
            {
                var actualIndex = gruposList.IndexOf(actualGroup.ToString());
                var nextIndex = actualIndex + 1;
                try
                {
                    ordenPlus = int.Parse(gruposList[nextIndex]);
                }
                catch { }
            }
            var maximo = int.Parse(gruposList.Last());
            _dbe.Conectar();
            _dbe.CrearComando(@"SELECT mensaje FROM Cat_Mensajes where nombre='MensajePortalWebRecepcionWorkflow' ");
            var dr = _dbe.EjecutarConsulta();
            if (dr.Read())
            {
                mensaje = dr["mensaje"].ToString();
            }
            _dbe.Desconectar();
            _dbr.Conectar();
            _dbr.CrearComando(@"SELECT g.codDoc, g.numeroAutorizacion, g.fecha, g.folio, g.serie, g.fechaAutorizacion, e.NOMEMI, r.RFCREC, r.NOMREC, (SELECT DISTINCT SUBSTRING((SELECT
                ',,,,,' + emp.userEmpleado + ': ' + emp.nombreEmpleado + ', | ,' + CONVERT(VARCHAR(MAX), val.estadoValidacion) + ', | ,' + val.observaciones + ', | ,' + CONVERT(NVARCHAR, val.fechaValidacion, 126) + ', | ,' + CONVERT(VARCHAR(MAX), gr.orden) AS [text()]
                FROM " + _dbe.DatabaseSchema + @".dbo.Cat_Empleados emp
                INNER JOIN Dat_Validaciones val
                ON val.idEmpleadoEmision = emp.idEmpleado
                INNER JOIN Cat_Grupos_validadores gr ON emp.idGrupo = gr.idGrupo
                WHERE val.id_Comprobante = g.idComprobante
                ORDER BY emp.idEmpleado
                FOR xml PATH (''))
                , 6, 1000) [Cat_Empleados]
              FROM Dat_Validaciones val) AS validadoPor FROM Dat_General g LEFT OUTER JOIN Cat_Emisor e ON e.IDEEMI = g.id_Emisor LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor where g.idComprobante=@id");
            _dbr.AsignarParametroCadena("@id", id);
            dr = _dbr.EjecutarConsulta();
            if (dr.Read())
            {
                try
                {
                    var tipoDoc = "Desconocido";
                    switch (dr["codDoc"].ToString())
                    {
                        case "01":
                            tipoDoc = "Factura";
                            break;
                        case "04":
                            tipoDoc = "Nota de Crédito";
                            break;
                        case "06":
                            tipoDoc = "Carta Porte";
                            break;
                        case "07":
                            tipoDoc = "Retención";
                            break;
                        case "08":
                            tipoDoc = "Nómina";
                            break;
                        case "09":
                            tipoDoc = "Contabilidad";
                            break;
                        default:
                            break;
                    }
                    var tabla = BuildTable(dr["validadoPor"].ToString());
                    mensaje = mensaje.Replace("@NombreReceptor", dr["NOMREC"].ToString());
                    mensaje = mensaje.Replace("@RFCReceptor", dr["RFCREC"].ToString());
                    mensaje = mensaje.Replace("@Validaciones", tabla);
                    mensaje = mensaje.Replace("@TipoDocumento", tipoDoc);
                    mensaje = mensaje.Replace("@NumeroAutorizacion", dr["numeroAutorizacion"].ToString());
                    mensaje = mensaje.Replace("@FechaEmision", dr["fecha"].ToString());
                    mensaje = mensaje.Replace("@Folio", dr["folio"].ToString());
                    mensaje = mensaje.Replace("@Serie", dr["serie"].ToString());
                    mensaje = mensaje.Replace("@FechaAutorizacion", dr["fechaAutorizacion"].ToString());
                    asunto = "Aprobación del documento electrónico No: " + dr["folio"].ToString() + " de " + dr["NOMEMI"].ToString() + "";
                }
                catch (Exception ex)
                {

                }
            }
            _dbr.Desconectar();
            if (actualGroup == maximo)
            {
                mensaje = mensaje.Replace("@Status", "APROBADO PARA PAGO");
                _dbe.Conectar();
                _dbe.CrearComando("select distinct e.email from Cat_Empleados e INNER JOIN Cat_Roles r ON e.id_Rol = r.idRol WHERE r.validacionRecepcion = 1");
                dr = _dbe.EjecutarConsulta();
                while (dr.Read())
                {
                    mails += dr[0].ToString() + ",";
                }
                _dbe.Desconectar();
            }
            else if (status.Equals("0"))
            {
                mensaje = mensaje.Replace("@Status", "RECHAZADO POR " + Session["USERNAME"].ToString() + " (ORDEN " + NivelValidacion() + ")");
                _dbe.Conectar();
                _dbe.CrearComando("select distinct e.email from Cat_Empleados e INNER JOIN Cat_Roles r ON e.id_Rol = r.idRol WHERE r.validacionRecepcion = 1");
                dr = _dbe.EjecutarConsulta();
                while (dr.Read())
                {
                    mails += dr[0].ToString() + ",";
                }
                _dbe.Desconectar();
            }
            else if (status.Equals("1"))
            {
                mensaje = mensaje.Replace("@Status", "APROBADO POR " + Session["USERNAME"].ToString() + " (ORDEN " + NivelValidacion() + ")");
                _dbe.Conectar();
                if (Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                {
                    _dbe.CrearComando("select distinct e.email from Cat_Empleados e INNER JOIN Cat_Roles r ON e.id_Rol = r.idRol INNER JOIN " + _dbr.DatabaseSchema + ".dbo.Cat_Grupos_validadores v ON e.idGrupo = v.idGrupo WHERE r.validacionRecepcion = 1 AND v.idGrupo = @ordenplus");
                }
                else
                {
                    _dbe.CrearComando("select distinct e.email from Cat_Empleados e INNER JOIN Cat_Roles r ON e.id_Rol = r.idRol INNER JOIN " + _dbr.DatabaseSchema + ".dbo.Cat_Grupos_validadores v ON e.idGrupo = v.idGrupo WHERE r.validacionRecepcion = 1 AND v.orden = @ordenplus");
                }
                _dbe.AsignarParametroEntero("@ordenplus", ordenPlus);
                dr = _dbe.EjecutarConsulta();
                while (dr.Read())
                {
                    mails += dr[0].ToString() + ",";
                }
                _dbe.Desconectar();
            }
            _dbr.Conectar();
            _dbr.CrearComando("select distinct p.email from " + _dbe.DatabaseSchema + ".dbo.Cat_Proveedores p INNER JOIN Cat_Emisor e ON p.id_Receptor = e.RFCEMI INNER JOIN Dat_General g ON g.id_Emisor = e.IDEEMI WHERE g.idComprobante = @id");
            _dbr.AsignarParametroCadena("@id", id);
            dr = _dbr.EjecutarConsulta();
            while (dr.Read())
            {
                mails += dr[0].ToString() + ",";
            }
            _dbr.Desconectar();
            try
            {
                _dbe.Conectar();
                _dbe.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio, a.XMLARC, a.PDFARC, a.ORDENARC, emailRecepBcc from Par_ParametrosSistema, " + _dbr.DatabaseSchema + ".dbo.Dat_Archivos a WHERE a.IDEFAC = @id");
                _dbe.AsignarParametroCadena("@id", id);
                dr = _dbe.EjecutarConsulta();
                if (dr.Read())
                {
                    _em.ServidorSmtp(dr["servidorSMTP"].ToString(), int.Parse(dr["puertoSMTP"].ToString()), bool.Parse(dr["sslSMTP"].ToString()), dr["userSMTP"].ToString(), dr["passSMTP"].ToString());
                    _em.LlenarEmail(dr["emailEnvio"].ToString(), mails.Trim(' ', ','), "", dr["emailRecepBcc"].ToString(), asunto, mensaje);
                    _em.Adjuntar(Server.MapPath("~/" + dr["XMLARC"].ToString()));
                    _em.Adjuntar(Server.MapPath("~/" + dr["PDFARC"].ToString()));
                    _em.Adjuntar(Server.MapPath("~/" + dr["ORDENARC"].ToString()));
                }
                _dbe.Desconectar();
                _em.EnviarEmail();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Existes the validacion.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ExisteValidacion(string id)
        {
            var existe = false;
            try
            {
                var idUser = Session["idUser"].ToString();
                _dbr.Conectar();
                _dbr.CrearComando("SELECT COUNT(*) FROM Dat_Validaciones WHERE id_Comprobante = @id AND idEmpleadoEmision = @user");
                _dbr.AsignarParametroCadena("@id", id);
                _dbr.AsignarParametroCadena("@user", idUser);
                var dr = _dbr.EjecutarConsulta();
                if (dr.Read())
                {
                    int count = 0;
                    int.TryParse(dr[0].ToString(), out count);
                    existe = count > 0;
                }
                _dbr.Desconectar();
            }
            catch { }
            return existe;
        }

        /// <summary>
        /// Handles the Click event of the bValidar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bValidar_Click(object sender, EventArgs e)
        {
            bCancelarValidaciones.Visible = true;
            bGuardarValidaciones.Visible = true;
            btnZip.Visible = false;
            bMail.Visible = false;
            bValidar.Visible = false;
            Buscar();
            gvFacturas.Columns[12].Visible = true;


        }

        /// <summary>
        /// Handles the Click event of the bCancelarValidaciones control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bCancelarValidaciones_Click(object sender, EventArgs e)
        {
            bCancelarValidaciones.Visible = false;
            bGuardarValidaciones.Visible = false;
            bValidar.Visible = true;
            btnZip.Visible = true;
            bMail.Visible = true;
            LlenarGrid();
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;
            gvFacturas.DataBind();
            gvFacturas.Columns[12].Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the lbPopupAdicionales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Th
        /// e <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbPopupAdicionales_Click1(object sender, EventArgs e)
        {
            var lb = (LinkButton)sender;
            var args = lb.CommandArgument.Split(',');
            var id = args[0];
            var estadoValidacion = args[1];
            #region Archivos Adicionales

            _dbr.Conectar();
            _dbr.CrearComando("SELECT path, idArchivo FROM Dat_ArchivosAdicionales WHERE idComprobante = @id");
            _dbr.AsignarParametroCadena("@id", id);
            var dr = _dbr.EjecutarConsulta();
            var hasFiles = false;
            var statusAdicional = "";
            var filesArray = "";
            while (dr.Read())
            {
                hasFiles = true;
                var path = dr[0].ToString();
                var fileName = Path.GetFileName(path);
                var encodedPath = ControlUtilities.EncodeStringToBase64(path);
                // var btnEliminar = "<input type='button' value='Eliminar' onclick='Mostrar(\"" + dr[1].ToString() + "\");' />";
                var canUpdate = (Session["USERNAME"].ToString().StartsWith("PROVE") ? (estadoValidacion == "0" || estadoValidacion == "2") : (true));
                var btnEliminar = canUpdate ? "<input type='button' value='Eliminar' onclick='Mostrar();' />" : "";
                var btnUpdate = canUpdate ? "<input type='button' value='Actualizar' onclick='Mostrar1();' />" : "";
                var img = "<a href='" + ResolveClientUrl("~/download.aspx") + "?fileEncoded=" + encodedPath + "'><img width='20px' height='25px' src='" + ResolveClientUrl("~/Imagenes/orden.png") + "' alt='[File]' data-toggle='tooltip' data-placement='top' title='Descargar' /></a>";
                filesArray += "<tr><td scope='col' style='text-align: center;'>" + img + "</td><td scope='col' style='text-align: center;'>" + fileName + "</td><td scope='col' style='text-align: center;'>" + btnEliminar + "</td><td scope='col' title='Eliminar' style='text-align: center;'>" + btnUpdate + "</td></tr>";
            }
            _dbr.Desconectar();
            if (hasFiles)
            {
                statusAdicional = "<table class='table table-condensed table-responsive table-hover'><thead><tr style='color:White;background-color:#4580A8;font-weight:bold;'><th scope='col' style='text-align: center;'></th><th scope='col' style='text-align: center;'>Nombre</th></tr><th scope='col' style='text-align: center;'>Eliminar</th><th scope='col' style='text-align: center;'>Actualizar</th></thead><tbody>" + filesArray + "</tbody></table>";
            }
            else
            {
                statusAdicional = "El comprobante no tiene archivos adicionales";
            }
            (Master as SiteMaster).MostrarAlerta(this, statusAdicional, 2);
            #endregion
        }

        /// <summary>
        /// Handles the UploadedComplete event of the filePago control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AjaxControlToolkit.AsyncFileUploadEventArgs"/> instance containing the event data.</param>
        protected void filePago_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            try
            {
                var posted = filePago.PostedFile;
                var bytes = PostedFileToBytes(posted);
                var xDoc = GetEntryXmlDoc(bytes);
                var tipoComprobante = "";
                var xmlNode = xDoc.DocumentElement;
                if (xmlNode?.Attributes != null)
                {
                    try
                    {
                        tipoComprobante = xmlNode.Attributes["tipoDeComprobante"].Value;
                    }
                    catch { }
                    if (string.IsNullOrEmpty(tipoComprobante))
                    {
                        try
                        {
                            tipoComprobante = xmlNode.Attributes["TipoDeComprobante"].Value;
                        }
                        catch { }
                    }
                }
                if (!tipoComprobante.Equals("P", StringComparison.OrdinalIgnoreCase) && !tipoComprobante.Equals("Pago", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("El comprobante NO es de pago");
                }
                var listaXml = Session["_listaXmlPagos"] != null ? (List<XmlDocument>)Session["_listaXmlPagos"] : new List<XmlDocument>();
                Session["_xmlPago"] = xDoc;
                LlenaInfoCfdiPagos();
                listaXml.Add(xDoc);
                Session["_listaXmlPagos"] = listaXml;
                Session["_messageUpload"] = null;
            }
            catch (Exception ex)
            {
                Session["_xmlPago"] = null;
                Session["_messageUpload"] = "El XML no se pudo procesar<br/><br/>" + ex.Message;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPagos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPagos_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            try
            {
                _dbr.Conectar();
                _dbr.CrearComando("SELECT g.total AS 'APagar', (g.total - (SELECT ISNULL(SUM(cp.MontoPago), 0.00) FROM Dat_ComplementosPago cp WHERE cp.id_Comprobante = g.idComprobante)) AS 'PendientePago', (SELECT ISNULL(SUM(cp.MontoPago), 0.00) FROM Dat_ComplementosPago cp WHERE cp.id_Comprobante = g.idComprobante) AS 'Pagado' FROM Dat_General g WHERE g.idComprobante = @id");
                _dbr.AsignarParametroCadena("@id", id);
                var dr = _dbr.EjecutarConsulta();
                if (dr.Read())
                {
                    decimal apagar = 0;
                    decimal pendientepago = 0;
                    decimal pagado = 0;
                    decimal.TryParse(dr["APagar"].ToString(), out apagar);
                    decimal.TryParse(dr["PendientePago"].ToString(), out pendientepago);
                    decimal.TryParse(dr["Pagado"].ToString(), out pagado);
                    lblPagos_APagar.Text = ControlUtilities.CerosNull(dr["APagar"].ToString(), true, true, false, true);
                    lblPagos_PendientePago.Text = ControlUtilities.CerosNull(dr["PendientePago"].ToString(), true, true, false, true);
                    lblPagos_Pagado.Text = ControlUtilities.CerosNull(dr["Pagado"].ToString(), true, true, false, true);
                    rowUploadPayments.Style["display"] = pendientepago > 0 ? "inline" : "none";
                }
                _dbr.Desconectar();
            }
            catch (Exception ex)
            {
                // ignored
                lblPagos_APagar.Text = ControlUtilities.CerosNull("0", true, true, false, true);
                lblPagos_PendientePago.Text = ControlUtilities.CerosNull("0", true, true, false, true);
                lblPagos_Pagado.Text = ControlUtilities.CerosNull("0", true, true, false, true);
                rowUploadPayments.Style["display"] = "none";
            }
            hfIdComprobantePago.Value = id;
            SqlDataSourcePagos.SelectParameters["idComprobante"].DefaultValue = id;
            gvPagos.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "btnPagos_ServerClick", "$('#ModuloPagos').modal('show');", true);
        }

        protected void bGuardarTipoProveedor_Click(object sender, EventArgs e)
        {
            var val = hfIdUpdateTipoProveedor.Value;
            var splitted = val.Split(',');
            var idComprobante = splitted[0];
            var idgrupo = ModTipoProv.SelectedValue;
            _dbr.Conectar();
            _dbr.CrearComando("UPDATE Dat_General SET id_TipoProveedor = @idTipo  OUTPUT inserted.idComprobante WHERE idComprobante = @id");
            _dbr.AsignarParametroCadena("@idTipo", idgrupo);
            _dbr.AsignarParametroCadena("@id", idComprobante);
            var dr = _dbr.EjecutarConsulta();
            var inserted = (dr.Read());
            _dbr.Desconectar();
            if (inserted)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Registro actualizado correctamente", 2, null, "$('#EditarTipoProveedor').modal('hide');");
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El registro no se pudo actualizar. Intentelo nuevamente", 4);
            }
            bBuscar_Click(null, null);
        }

        protected void gvPagoCfdi_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        private string GetUuidOriginal(string idComprobante)
        {
            var uuid = "";
            try
            {
                _dbr.Conectar();
                _dbr.CrearComando(@"SELECT numeroAutorizacion AS uuid FROM Dat_General WHERE idcomprobante = @id");
                _dbr.AsignarParametroCadena("@id", idComprobante);
                var dr = _dbr.EjecutarConsulta();
                if (dr.Read())
                {
                    uuid = dr["uuid"].ToString();
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
            finally { _dbr.Desconectar(); }
            return uuid;
        }

        private void LlenaInfoCfdiPagos()
        {
            var xDoc = (XmlDocument)Session["_xmlPago"];
            var uuid = "";
            var uuidOriginal = GetUuidOriginal(hfIdComprobantePago.Value);
            try
            {
                uuid = xDoc.GetElementsByTagName("tfd:TimbreFiscalDigital").Cast<XmlNode>().FirstOrDefault().Attributes["UUID"].Value;
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El XML no está timbrado, falta el nodo tfd:TimbreFiscalDigital", 4);
                return;
            }

            var lista = Session["_PagoCfdi"] != null ? (List<PagoCfdiRecepcionTemp>)Session["_PagoCfdi"] : new List<PagoCfdiRecepcionTemp>();

            var listaPagos = xDoc.GetElementsByTagName("pago10:Pago").Cast<XmlNode>().ToList();

            if (listaPagos.Count <= 0)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El XML no contiene nodos pago10:Pago", 4);
                return;
            }

            //var listaRelacionados;
            var listaUuidsrelacionados = xDoc.GetElementsByTagName("pago10:DoctoRelacionado").Cast<XmlNode>().ToList().Select(x => x.Attributes["IdDocumento"].Value).ToList();

            var corresponde = listaUuidsrelacionados.Any(x => x.ToUpper().Equals(uuidOriginal.ToUpper()));

            if (!corresponde)
            {
                throw new Exception("El complemento de pago no está relacionado con el UUID de la factura original");
            }

            var yaRegistrado = false;
            _dbr.Conectar();
            _dbr.CrearComando("SELECT idPago FROM Dat_ComplementosPago WHERE UPPER(UUID) = @uuid");
            _dbr.AsignarParametroCadena("@uuid", uuid.ToUpper());
            var dr = _dbr.EjecutarConsulta();
            yaRegistrado = dr.Read();
            _dbr.Desconectar();
            if (yaRegistrado)
            {
                throw new Exception("El complemento de pago ya se encuentra registrado para la factura original");
            }

            foreach (var pago in listaPagos)
            {
                var listaRelacionados = pago.ChildNodes.Cast<XmlNode>().Where(x => x.Name.Equals("pago10:DoctoRelacionado")).ToList();
                if (listaRelacionados.Count > 0)
                {
                    foreach (var relacionado in listaRelacionados)
                    {
                        var temp = new PagoCfdiRecepcionTemp();
                        try { temp.FechaPago = pago.Attributes["FechaPago"].Value; } catch { }
                        try { temp.FormaPago = pago.Attributes["FormaDePagoP"].Value; } catch { }
                        try { temp.MontoPago = pago.Attributes["Monto"].Value; } catch { }
                        try { temp.NumOperacion = pago.Attributes["NumOperacion"].Value; } catch { }
                        try { temp.Parcialidad = relacionado.Attributes["NumParcialidad"].Value; } catch { }
                        try { temp.SaldoAnterior = relacionado.Attributes["ImpSaldoAnt"].Value; } catch { }
                        try { temp.SaldoInsoluto = relacionado.Attributes["ImpSaldoInsoluto"].Value; } catch { }
                        try { temp.SaldoPagado = relacionado.Attributes["ImpPagado"].Value; } catch { }
                        try { temp.SerieFolio = relacionado.Attributes["Serie"].Value; } catch { }
                        try { temp.SerieFolio += relacionado.Attributes["Folio"].Value; } catch { }
                        try { temp.UuidRelacionado = relacionado.Attributes["IdDocumento"].Value; } catch { }
                        temp.UuidPago = uuid;
                        lista.Add(temp);
                    }
                }
                else
                {
                    var temp = new PagoCfdiRecepcionTemp();
                    try { temp.FechaPago = pago.Attributes["FechaPago"].Value; } catch { }
                    try { temp.FormaPago = pago.Attributes["FormaDePagoP"].Value; } catch { }
                    try { temp.MontoPago = pago.Attributes["Monto"].Value; } catch { }
                    try { temp.NumOperacion = pago.Attributes["NumOperacion"].Value; } catch { }
                    temp.UuidPago = uuid;
                    lista.Add(temp);
                }
            }
            Session["_PagoCfdi"] = lista;
        }

        /// <summary>
        /// Posteds the file to bytes.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.Byte[].</returns>
        private byte[] PostedFileToBytes(HttpPostedFile file)
        {
            byte[] result = null;
            try
            {
                var stream = file.InputStream;
                var output = new MemoryStream();
                stream.Position = 0;
                stream.CopyTo(output);
                result = output.ToArray();
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Gets the entry XML document.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>XmlDocument.</returns>
        private XmlDocument GetEntryXmlDoc(byte[] bytes)
        {
            var xmlDoc = new XmlDocument();
            using (var ms = new MemoryStream(bytes))
            {
                xmlDoc.Load(ms);
            }
            return xmlDoc;
        }

        protected void lbUploadPago_Click(object sender, EventArgs e)
        {
            if (Session["_messageUpload"] != null && !string.IsNullOrEmpty(Session["_messageUpload"].ToString()))
            {
                (Master as SiteMaster).MostrarAlerta(this, Session["_messageUpload"].ToString(), 4);
                return;
            }
            if (Session["_PagoCfdi"] != null)
            {
                var lista = (List<PagoCfdiRecepcionTemp>)Session["_PagoCfdi"];
                gvPagoCfdi.DataSource = lista;
                gvPagoCfdi.DataBind();
            }
            Session["_xmlPago"] = null;
        }

        protected void lbCleanPago_Click(object sender, EventArgs e)
        {
            CleanPagos();
        }

        private void CleanPagos()
        {
            Session["_xmlPago"] = null;
            Session["_PagoCfdi"] = null;
            Session["_listaXmlPagos"] = null;
            gvPagoCfdi.DataSource = null;
            gvPagoCfdi.DataBind();
        }

        protected void lbGuardarPagosComprobante_Click(object sender, EventArgs e)
        {
            var listaPagos = Session["_PagoCfdi"] != null ? (List<PagoCfdiRecepcionTemp>)Session["_PagoCfdi"] : new List<PagoCfdiRecepcionTemp>();
            var listaXml = Session["_listaXmlPagos"] != null ? ((List<XmlDocument>)Session["_listaXmlPagos"]).Select(x => ControlUtilities.EncodeXmlToBase64(x)).ToList() : new List<string>();
            if (listaXml.Count <= 0)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No ha subido ningún XML", 4, null);
                return;
            }
            else if (listaPagos.Count <= 0)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Ningun XML subido tiene pagos relacionados", 4, null);
                return;
            }
            var id_Comprobante = hfIdComprobantePago.Value;
            var listaPagosSerialized = JsonConvert.SerializeObject(listaPagos);
            var wsRecepcion = new WsRecepcion() { Timeout = (1800 * 1000) };
            var idUserWs = _idUser;
            if (Session["USERNAME"].ToString().StartsWith("PROVE", StringComparison.OrdinalIgnoreCase))
            {
                idUserWs = "999999999";
            }
            object[] response = null;
            try
            {
                response = wsRecepcion.RecibeComplementoPago(id_Comprobante, listaPagosSerialized, listaXml.ToArray(), idUserWs, Session["IDENTEMI"].ToString());
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Han ocurrido algunos errores, intente subir los pagos nuevamente<br/>" + ex.Message, 4, null);
            }
            if (response != null)
            {
                CleanPagos();
                var flagStatus = (bool)response[0];
                var messageList = JsonConvert.DeserializeObject<List<string>>((string)response[1]);
                if (flagStatus)
                {
                    // TODO OK
                    (Master as SiteMaster).MostrarAlerta(this, "Todos los pagos se registraron correctamente", 2, null, "$('#ModuloPagos').modal('hide');");
                    bBuscar_Click(null, null);
                }
                else
                {
                    var liFailed = "<li><ul>" + string.Join("</ul><ul>", messageList) + "</ul></li>";
                    (Master as SiteMaster).MostrarAlerta(this, "Han ocurrido algunos errores, intente subir los pagos nuevamente<br/>" + liFailed, 4);
                }
            }
        }


        protected void bUpdatePDF_Click(object sender, EventArgs e)
        {
            try
            {

                var btn = (LinkButton)sender;
                idPDFupdate = btn.CommandArgument;
                Session["_files"] = idPDFupdate;
                ScriptManager.RegisterStartupScript(this, GetType(), "hjvkjvkj", "$('#EditarPDF').modal('show'); ", true);
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se pudo cancelar<br/><br/>" + ex, 4, null);
            }
        }

        protected void bUpdateOC_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (LinkButton)sender;
                idPDFupdate = btn.CommandArgument;
                Session["_files"] = idPDFupdate;
                ScriptManager.RegisterStartupScript(this, GetType(), "hjvkjvkj", "$('#EditarOC').modal('show'); ", true);
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
            }
        }

        protected void btUpdateDocAdi_Click(object sender, EventArgs e)
        {
            try
            {
                var _idEditar = ((LinkButton)sender).CommandArgument;
                Session["_files"] = _idEditar;
                ScriptManager.RegisterStartupScript(this, GetType(), "hjvkjvkj", "$('#EditarArcAdi').modal('show'); ", true);
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
            }



            //_dbr.Conectar();
            //_dbr.CrearComando("update Dat_ArchivosAdicionales set path=@path where idArchivo=@id");
            //_dbr.AsignarParametroCadena("@id", _idEditar);
            //_dbr.EjecutarConsulta();
        }


        protected void bSavePDF_Click(object sender, EventArgs e)
        {
            try
            {
                var XMLrute = "";
                var idP = Session["_files"];
                _dbr.Conectar();
                _dbr.CrearComando("SELECT xmlarc FROM Dat_Archivos WHERE IDEFAC  = @id");
                _dbr.AsignarParametroCadena("@id", idP.ToString());
                var dr = _dbr.EjecutarConsulta();
                if (dr.Read())
                {
                    XMLrute = dr[0].ToString();
                }
                _dbr.Desconectar();

                string[] rute = XMLrute.ToString().Split(@"\".ToCharArray());//xml
                var ruta = rute[0] + @"\" + rute[1] + @"\" + rute[2] + @"\" + rute[3] + @"\" + rute[4] + @"\" + rute[5] + @"\" + "PDF" + @"\";
                var namePDF = Session["_xmlPago"];

                var dirdocs = "";
                _dbe.Conectar();
                _dbe.CrearComando("SELECT top 1 dirdocs FROM Par_ParametrosSistema");
                var dre = _dbe.EjecutarConsulta();
                if (dre.Read())
                {
                    dirdocs = dre[0].ToString();
                }
                _dbe.Desconectar();


                var ruta1 = dirdocs + @"\" + rute[1] + @"\" + rute[2] + @"\" + rute[3] + @"\" + rute[4] + @"\" + rute[5] + @"\" + "PDF" + @"\";

                var datosPDF = (byte[])Session["_PagoCfdi"];

                File.WriteAllBytes(ruta1 + namePDF, datosPDF);


                _dbr.Conectar();
                _dbr.CrearComando("UPDATE Dat_Archivos SET PDFARC=@namePDF WHERE IDEFAC = @id AND XMLARC=@xml");
                _dbr.AsignarParametroCadena("@namePDF", ruta + namePDF);
                _dbr.AsignarParametroCadena("@id", idP.ToString());
                _dbr.AsignarParametroCadena("@xml", XMLrute);
                var dr1 = _dbr.EjecutarConsulta();
                _dbr.Desconectar();

                _dbr.Conectar();
                _dbr.CrearComando("UPDATE Dat_general SET estadoValidacion=@edo WHERE idComprobante = @id ");
                _dbr.AsignarParametroCadena("@edo", "2");
                _dbr.AsignarParametroCadena("@id", idP.ToString());
                _dbr.EjecutarConsulta();
                _dbr.Desconectar();


                //System.IO.File.Create(ruta + namePDF);

                ScriptManager.RegisterStartupScript(this, GetType(), "hjvkjvkj", "$('#EditarPDF').modal('hide'); ", true);
                (Master as SiteMaster).MostrarAlerta(this, "PDF actualizado correctamente", 2, null, "$('#EditarPDF').modal('hide');");
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
            }
        }

        protected void bSaveOC_Click(object sender, EventArgs e)
        {
            try
            {
                var XMLrute = "";
                var idP = Session["_files"];
                _dbr.Conectar();
                _dbr.CrearComando("SELECT xmlarc FROM Dat_Archivos WHERE IDEFAC  = @id");
                _dbr.AsignarParametroCadena("@id", idP.ToString());
                var dr = _dbr.EjecutarConsulta();
                if (dr.Read())
                {
                    XMLrute = dr[0].ToString();
                }
                _dbr.Desconectar();

                string[] rute = XMLrute.ToString().Split(@"\".ToCharArray());//xml
                var ruta = rute[0] + @"\" + rute[1] + @"\" + rute[2] + @"\" + rute[3] + @"\" + rute[4] + @"\" + rute[5] + @"\" + "ORDEN" + @"\";
                var nameOC = Session["_xmlPago"];

                var dirdocs = "";
                _dbe.Conectar();
                _dbe.CrearComando("SELECT top 1 dirdocs FROM Par_ParametrosSistema");
                var dre = _dbe.EjecutarConsulta();
                if (dre.Read())
                {
                    dirdocs = dre[0].ToString();
                }
                _dbe.Desconectar();


                var ruta1 = dirdocs + @"\" + rute[1] + @"\" + rute[2] + @"\" + rute[3] + @"\" + rute[4] + @"\" + rute[5] + @"\" + "PDF" + @"\";

                var datosOC = (byte[])Session["_PagoCfdi"];

                File.WriteAllBytes(ruta1 + nameOC, datosOC);

                _dbr.Conectar();
                _dbr.CrearComando("UPDATE Dat_Archivos SET ORDENARC=@nameOC WHERE IDEFAC = @id AND XMLARC=@xml");
                _dbr.AsignarParametroCadena("@nameOC", ruta + nameOC);
                _dbr.AsignarParametroCadena("@id", idP.ToString());
                _dbr.AsignarParametroCadena("@xml", XMLrute);
                var dr1 = _dbr.EjecutarConsulta();
                _dbr.Desconectar();

                _dbr.Conectar();
                _dbr.CrearComando("UPDATE Dat_general SET estadoValidacion=@edo WHERE idComprobante = @id ");
                _dbr.AsignarParametroCadena("@edo", "2");
                _dbr.AsignarParametroCadena("@id", idP.ToString());
                _dbr.EjecutarConsulta();
                _dbr.Desconectar();

                ScriptManager.RegisterStartupScript(this, GetType(), "hjvkjvkj", "$('#EditarOC').modal('hide'); ", true);
                (Master as SiteMaster).MostrarAlerta(this, "Orden de compra actualizada correctamente", 2, null, "$('#EditarOC').modal('hide');");
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
            }
        }

        protected void bSaveAA_Click(object sender, EventArgs e)
        {
            try
            {
                var _idEditar = ((LinkButton)sender).CommandArgument;
                var XMLrute = "";
                var idCom = "";
                var idgeneral = Session["_files"];
                _dbr.Conectar();
                _dbr.CrearComando("SELECT path, idComprobante FROM Dat_ArchivosAdicionales WHERE idArchivo  = @id");
                _dbr.AsignarParametroCadena("@id", idgeneral.ToString());
                var dr = _dbr.EjecutarConsulta();
                if (dr.Read())
                {
                    XMLrute = dr[0].ToString();
                    idCom = dr[1].ToString();
                }
                _dbr.Desconectar();

                string[] rute = XMLrute.ToString().Split(@"\".ToCharArray());//xml
                var ruta = rute[0] + @"\" + rute[1] + @"\" + rute[3] + @"\" + rute[4] + @"\" + rute[5] + @"\" + "ADICIONALES" + @"\" + rute[7] + @"\" + rute[8];

                //rute[0] + @"\" + rute[1] + @"\" + rute[2] + @"\" + rute[3] + @"\" + rute[4] + @"\" + rute[5] + @"\" + "ORDEN" + @"\";
                var nameAA = Session["_xmlPago"];

                var dirdocs = "";
                _dbe.Conectar();
                _dbe.CrearComando("SELECT top 1 dirdocs FROM Par_ParametrosSistema");
                var dre = _dbe.EjecutarConsulta();
                if (dre.Read())
                {
                    dirdocs = dre[0].ToString();
                }
                _dbe.Desconectar();


                var ruta1 = dirdocs + rute[1] + @"\" + rute[3] + @"\" + rute[4] + @"\" + rute[5] + @"\" + "ADICIONALES" + @"\" + rute[7] + @"\" + rute[8];

                var datosOC = (byte[])Session["_PagoCfdi"];

                File.WriteAllBytes(ruta1 + nameAA, datosOC);

                _dbr.Conectar();
                _dbr.CrearComando("UPDATE Dat_ArchivosAdicionales SET path=@nameAA WHERE idArchivo = @id");
                _dbr.AsignarParametroCadena("@nameAA", ruta + nameAA);
                _dbr.AsignarParametroCadena("@id", idgeneral.ToString());
                var dr1 = _dbr.EjecutarConsulta();
                _dbr.Desconectar();

                _dbr.Conectar();
                _dbr.CrearComando("UPDATE Dat_general SET estadoValidacion=@edo WHERE idComprobante = @id ");
                _dbr.AsignarParametroCadena("@edo", "2");
                _dbr.AsignarParametroCadena("@id", rute[7].ToString());
                _dbr.EjecutarConsulta();
                _dbr.Desconectar();

                this.SqlDataSourceDocsAdi.DataBind();
                gvDocsAdi.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "hjvkjvkj", "$('#EditarArcAdi').modal('hide'); ", true);
                //        (Master as SiteMaster).MostrarAlerta(this, "Orden de compra actualizada correctamente", 2, null, "$('#EditarArcAdi').modal('hide');");
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
            }
        }

        protected void AsyncFUpdf_UploadedCompletePDF(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            try
            {
                var posted = AsyncFUpdf.PostedFile;
                var bytes = PostedFileToBytes(posted);
                var name = posted.FileName;
                var listaXml = Session["_listaXmlPagos"] != null ? (List<XmlDocument>)Session["_listaXmlPagos"] : new List<XmlDocument>();
                Session["_xmlPago"] = name;
                Session["_PagoCfdi"] = bytes;
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
            }
        }

        protected void AsyncFUpdf_UploadedCompleteOC(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            try
            {
                var posted = AsyncFUOC.PostedFile;
                var bytes = PostedFileToBytes(posted);
                var name = posted.FileName;
                var listaXml = Session["_listaXmlPagos"] != null ? (List<XmlDocument>)Session["_listaXmlPagos"] : new List<XmlDocument>();
                Session["_xmlPago"] = name;
                Session["_PagoCfdi"] = bytes;
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
            }
        }

        protected void AsyncFUpdf_UploadedCompleteAA(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            try
            {
                var posted = AsyncFUAA.PostedFile;
                var bytes = PostedFileToBytes(posted);
                var name = posted.FileName;
                var listaXml = Session["_listaXmlPagos"] != null ? (List<XmlDocument>)Session["_listaXmlPagos"] : new List<XmlDocument>();
                Session["_xmlPago"] = name;
                Session["_PagoCfdi"] = bytes;
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
            }
        }

        protected void btDeleteDocAdi_Click(object sender, EventArgs e)
        {
            var _idEditar = ((LinkButton)sender).CommandArgument;
            _dbr.Conectar();
            _dbr.CrearComando("DELETE Dat_ArchivosAdicionales where idArchivo=@id");
            _dbr.AsignarParametroCadena("@id", _idEditar);
            _dbr.EjecutarConsulta();

            this.SqlDataSourceDocsAdi.DataBind();
            gvDocsAdi.DataBind();
        }

        //protected void lbAddArcAdi_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var varidPDFupdate = ((LinkButton)sender).CommandArgument;
        //        Session["_files"] = idPDFupdate;
        //        ScriptManager.RegisterStartupScript(this, GetType(), "hjvkjvkj", "$('#EditarOC').modal('show'); ", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        (Master as SiteMaster).MostrarAlerta(this, "No se pudo cargar el archivo<br/><br/>" + ex, 4, null);
        //    }
        //}

        protected void lbPopupAdicionales_Click(object sender, EventArgs e)
        {
            try
            {

                var btn = (LinkButton)sender;

                var args = btn.CommandArgument.Split(',');
                idPDFupdate = args[0];

                Session["_files"] = idPDFupdate;

                this.SqlDataSourceDocsAdi.SelectParameters["QUERY"].DefaultValue = "-";
                this.SqlDataSourceDocsAdi.SelectParameters["SiO"].DefaultValue = this.idPDFupdate;
                this.SqlDataSourceDocsAdi.DataBind();


                gvDocsAdi.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "hjvkjvkj", "$('#infoDocsAdi').modal('show'); ", true);
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se pudo cancelar<br/><br/>" + ex, 4, null);
            }
        }
        protected void Distribuidor_Click(object sender, EventArgs e)
        {
            var id = ((Button)sender).CommandArgument;
            var idTipoProveedor = "";
            _dbr.Conectar();
            _dbr.CrearComando("SELECT (CASE ISNULL(id_TipoProveedor, '') WHEN '' THEN 0 ELSE id_TipoProveedor END) AS id_TipoProveedor FROM Dat_General WHERE idComprobante = @id");
            _dbr.AsignarParametroCadena("@id", id);
            var dr = _dbr.EjecutarConsulta();
            if (dr.Read())
            {
                idTipoProveedor = dr["id_TipoProveedor"].ToString();
            }
            _dbr.Desconectar();
            DropDownList1.SelectedValue = idTipoProveedor;
            Session["IdDistribuidor"] = id;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "$('#Distribuidor').modal('show'); ", true);
        }
        protected void bSaveProveedor_Click(object sender, EventArgs e)
        {
            var id = Session["IdDistribuidor"].ToString();
            try
            {
                _dbr.Conectar();
                _dbr.CrearComando(@"update dat_general set id_TipoProveedor =@idtipoProv  where idComprobante =@idcomprobante ");
                _dbr.AsignarParametroCadena("@idtipoProv", DropDownList1.SelectedValue.ToString());
                _dbr.AsignarParametroCadena("@idcomprobante", id);
                _dbr.EjecutarConsulta1();
                (Master as SiteMaster).MostrarAlerta(this, "Cambios guardados correctamente", 1);
                Session["IdDistribuidor"] = null;
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Los cambios no se pudieron efectuar, inténtelo de nuevo por favor.<br/><br/>" + ex.Message, 4);
            }
            finally { _dbr.Desconectar(); }
        }
    }
}
