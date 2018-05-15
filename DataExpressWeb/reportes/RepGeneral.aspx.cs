// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 04-09-2017
// ***********************************************************************
// <copyright file="RepGeneral.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Control;
using Datos;
using System.Linq;
using System.IO;
using Ionic.Zip;

namespace DataExpressWeb
{
    /// <summary>
    /// Class RepGeneral.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class RepGeneral : Page
    {
        /// <summary>
        /// The _count
        /// </summary>
        private int _count;
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The _dir
        /// </summary>
        private string _dir;
        /// <summary>
        /// The _fecha
        /// </summary>
        private string _fecha;
        /// <summary>
        /// The _fechacreacion
        /// </summary>
        private string _fechacreacion;
        /// <summary>
        /// The _fechanom
        /// </summary>
        private string _fechanom;
        /// <summary>
        /// The _virtual dir
        /// </summary>
        private string _virtualDir;
        /// <summary>
        /// The virtual dir conta
        /// </summary>
        private string _virtualDirConta;
        /// <summary>
        /// The sesion
        /// </summary>
        private string _sesion;
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            SqlDataSourcePtoEmi.ConnectionString = _db.CadenaConexion;
            SqlDataSourceTipDoc.ConnectionString = _db.CadenaConexion;
            SqlDataSourceEmisor.ConnectionString = _db.CadenaConexion;
            SqlDataSourceReceptor.ConnectionString = _db.CadenaConexion;
            SqlDataSourceRepTickets.ConnectionString = _db.CadenaConexion;
            if (Session["idUser"] != null)
            {
                if (!IsPostBack)
                {
                    ddlTipDoc.DataBind();
                    if (Session["IDGIRO"] != null)
                    {
                        if (Session["IDGIRO"].ToString().Contains("1"))
                        {
                            #region Hotel
                            var toRemove = from i in ddlTipDoc.Items.Cast<ListItem>() where (!i.Value.Equals("01") && !i.Value.Equals("04") && !i.Value.Equals("0")) select i;
                            var lista = toRemove.ToList();
                            foreach (var item in lista)
                            {
                                ddlTipDoc.Items.Remove(item);
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the bGenerar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bGenerar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                var where = "";
                try
                {
                    _count = 0;
                    if (!ddlReporte.SelectedValue.Equals("0"))
                    {
                        if (!string.IsNullOrEmpty(tbFechaInicial.Text) && !string.IsNullOrEmpty(tbFechaFinal.Text))
                        {
                            if (!Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd").Equals("00010101") && !Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd").Equals("00010101"))
                            {
                                if (Convert.ToDateTime(tbFechaInicial.Text) <= Convert.ToDateTime(tbFechaFinal.Text))
                                {
                                    _fecha = Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd");
                                    _fechanom = Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd");
                                    _fechacreacion = Localization.Now.ToString("yyyyMMddHHmmss");
                                    where += " CONVERT(VARCHAR(MAX),g.FECHA,112) >= " + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + " AND CONVERT(VARCHAR(MAX),g.FECHA,112) <=" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + " AND ";

                                    if (ddlPtoEmi.SelectedValue != "0")
                                    {
                                        where += " g.serie='" + ddlPtoEmi.SelectedItem.Text + "' AND ";
                                    }
                                    if (ddlEstado.SelectedValue != "0")
                                    {
                                        where += " (g.tipo +g.estado) =  '" + ddlEstado.SelectedValue + "' AND ";
                                    }
                                    if (ddlTipDoc.SelectedValue != "0")
                                    {
                                        where += " g.codDoc='" + ddlTipDoc.SelectedValue + "' AND ";
                                    }
                                    if (ddlEmisor.SelectedValue != "0" && ddlEmisor.SelectedValue != "Todos")
                                    {
                                        where += " cem.RFCEMI='" + ddlEmisor.SelectedItem.Text + "' AND ";
                                    }
                                    if (ddlReceptor.SelectedValue != "0" && ddlReceptor.SelectedValue != "Todos")
                                    {
                                        where += " cr.RFCREC='" + ddlReceptor.SelectedItem.Text + "' AND ";
                                    }
                                    where = where.Substring(0, where.Length - 5);
                                    if (!ddlReporte.SelectedValue.Equals("Contabilidad"))
                                    {
                                        _virtualDir = @"docs\" + (Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE") + _fechacreacion + "_" + ddlReporte.SelectedValue + ".xlsx";
                                    }
                                    else
                                    {
                                        if (Session["IDENTEMI"].ToString() == "HRP880129QX5")
                                        {
                                            _virtualDir = @"docs\Conta\";
                                            //   _virtualDirConta = @"docs\" + "U" + new string((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE").Take(3).ToArray()) + _fechacreacion + "_" + ddlReporte.SelectedValue + ".zip";
                                            _sesion = new string((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE").Take(3).ToArray());
                                        }
                                        else
                                        {
                                            _virtualDir = @"docs\Conta\" + "U" + new string((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE").Take(3).ToArray()) + _fecha + ".txt";
                                        }
                                    }
                                    _dir = AppDomain.CurrentDomain.BaseDirectory + @"reportes\" + _virtualDir;
                                    Directory.CreateDirectory(Path.GetDirectoryName(_dir));
                                    var periodo = tbFechaInicial.Text + " - " + tbFechaFinal.Text;
                                    var informes = new Informes(_dir, _db.CadenaConexion, periodo, Session["IDGIRO"].ToString());
                                    if (ddlReporte.SelectedValue.Equals("General"))
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"SELECT  count(g.idComprobante)
                                            FROM            Dat_GENERAL g INNER JOIN
                                                Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
                                                Cat_RECEPTOR cr ON g.id_Receptor = cr.IDEREC
                                            WHERE " + where);

                                        var dr = _db.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            _count = Convert.ToInt32(dr[0]);
                                        }
                                        _db.Desconectar();
                                        if (_count > 0)
                                        {
                                            if (informes.General(where, Session["IDENTEMI"].ToString()))
                                            {
                                                var js = "downloadFile('" + _virtualDir.Replace(@"\", "/") + "');";
                                                ScriptManager.RegisterStartupScript(this, GetType(), "downloadViaJS", js, true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('" + informes.Ex.Message + "', 4);", true);
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('No hay registros de acuerdo a los criterios de busqueda', 4);", true);
                                        }
                                    }
                                    else if (ddlReporte.SelectedValue.Equals("Email"))
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"SELECT  COUNT(g.idComprobante)
                                              FROM   Cat_emailEnvio mail inner join
			                                       Dat_general g on g.idComprobante = mail.id_general INNER JOIN
                                                   Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
			                                       Cat_RECEPTOR cr ON cr.IDEREC = g.id_Receptor
                                             WHERE " + where);
                                        var dr = _db.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            _count = Convert.ToInt32(dr[0]);
                                        }
                                        _db.Desconectar();
                                        if (_count > 0)
                                        {
                                            if (informes.Mail(where, Session["IDENTEMI"].ToString()))
                                            {
                                                var js = "downloadFile('" + _virtualDir.Replace(@"\", "/") + "');";
                                                ScriptManager.RegisterStartupScript(this, GetType(), "downloadViaJS", js, true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('" + informes.Ex.Message + "', 4);", true);
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('No hay registros de acuerdo a los criterios de busqueda', 4);", true);
                                        }
                                    }
                                    else if (ddlReporte.SelectedValue.Equals("Conceptos"))
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"SELECT  COUNT(g.idComprobante)
                                              FROM   Dat_Detalles dd inner join
			                                       Dat_general g on g.idComprobante = dd.id_Comprobante INNER JOIN
                                                   Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
			                                       Cat_RECEPTOR cr ON cr.IDEREC = g.id_Receptor
                                             WHERE " + where);
                                        var dr = _db.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            _count = Convert.ToInt32(dr[0]);
                                        }
                                        _db.Desconectar();
                                        if (_count > 0)
                                        {
                                            if (informes.Conceptos(where, Session["IDENTEMI"].ToString()))
                                            {
                                                var js = "downloadFile('" + _virtualDir.Replace(@"\", "/") + "');";
                                                ScriptManager.RegisterStartupScript(this, GetType(), "downloadViaJS", js, true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('" + informes.Ex.Message + "', 4);", true);
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('No hay registros de acuerdo a los criterios de busqueda', 4);", true);
                                        }
                                    }
                                    else if (ddlReporte.SelectedValue.Equals("Contabilidad"))
                                    {
                                        if (Session["IDENTEMI"].ToString() == "HRP880129QX5")
                                        {
                                            //obtener rango

                                            var whereC = "";
                                            if (ddlPtoEmi.SelectedValue != "0")
                                            {
                                                whereC += " g.serie='" + ddlPtoEmi.SelectedItem.Text + "' AND ";
                                            }
                                            if (ddlEstado.SelectedValue != "0")
                                            {
                                                whereC += " (g.tipo +g.estado) =  '" + ddlEstado.SelectedValue + "' AND ";
                                            }
                                            if (ddlTipDoc.SelectedValue != "0")
                                            {
                                                whereC += " g.codDoc='" + ddlTipDoc.SelectedValue + "' AND ";
                                            }
                                            if (ddlEmisor.SelectedValue != "0")
                                            {
                                                whereC += " cem.RFCEMI='" + ddlEmisor.SelectedItem.Text + "' AND ";
                                            }
                                            if (ddlReceptor.SelectedValue != "0")
                                            {
                                                whereC += " cr.RFCREC='" + ddlReceptor.SelectedItem.Text + "' AND ";
                                            }

                                            var yearS = Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyy");
                                            var yearE = Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyy");
                                            var monthS = Convert.ToDateTime(tbFechaInicial.Text).ToString("MM");
                                            var monthE = Convert.ToDateTime(tbFechaInicial.Text).ToString("MM");
                                            int dayStart = Convert.ToInt32(Convert.ToDateTime(tbFechaInicial.Text).ToString("dd"));
                                            int dayEnd = Convert.ToInt32(Convert.ToDateTime(tbFechaFinal.Text).ToString("dd"));
                                            var j = dayStart - 1;
                                            for (var i = dayStart; i <= dayEnd; i++)
                                            {
                                                where = "";
                                                j += 1;
                                                var dateS = yearS + "/" + monthS + "/" + j;
                                                var dateE = yearE + "/" + monthE + "/" + i;
                                                where = " CONVERT(VARCHAR(MAX),g.FECHA,112) >= " + Convert.ToDateTime(dateS).ToString("yyyyMMdd") + " AND CONVERT(VARCHAR(MAX),g.FECHA,112) <=" + Convert.ToDateTime(dateE).ToString("yyyyMMdd") + " AND " + whereC;
                                                where = where.Substring(0, where.Length - 5);
                                                //fin repCon
                                                _db.Conectar();
                                                _db.CrearComando(@"SELECT  COUNT(g.idComprobante)
                                              FROM   Dat_Detalles inner join
			                                       Dat_general g on g.idComprobante = Dat_Detalles.id_Comprobante INNER JOIN
                                                   Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
			                                       Cat_RECEPTOR cr ON cr.IDEREC = g.id_Receptor
                                             WHERE " + where);
                                                var dr = _db.EjecutarConsulta();
                                                if (dr.Read())
                                                {
                                                    _count = Convert.ToInt32(dr[0]);
                                                }
                                                _db.Desconectar();


                                                if (_count > 0)
                                                {
                                                    if (informes.ContabilidadC(where, _sesion))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('" + informes.Ex.Message + "', 4);", true);
                                                    }
                                                }
                                                else
                                                {
                                                }
                                            }//for reporte por dias
                                            Zip_Download(_dir);
                                        }
                                        else
                                        {
                                            _db.Conectar();
                                            _db.CrearComando(@"SELECT  COUNT(g.idComprobante)
                                              FROM   Dat_Detalles inner join
			                                       Dat_general g on g.idComprobante = Dat_Detalles.id_Comprobante INNER JOIN
                                                   Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
			                                       Cat_RECEPTOR cr ON cr.IDEREC = g.id_Receptor
                                             WHERE " + where);
                                            var dr = _db.EjecutarConsulta();
                                            if (dr.Read())
                                            {
                                                _count = Convert.ToInt32(dr[0]);
                                            }
                                            _db.Desconectar();


                                            if (_count > 0)
                                            {
                                                if (informes.Contabilidad(where, "", Session["IDENTEMI"].ToString()))
                                                {
                                                    var js = "downloadFile('" + _virtualDir.Replace(@"\", "/") + "');";
                                                    ScriptManager.RegisterStartupScript(this, GetType(), "downloadViaJS", js, true);
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('" + informes.Ex.Message + "', 4);", true);
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('No hay registros de acuerdo a los criterios de busqueda', 4);", true);
                                            }
                                        }
                                    }
                                    else if (ddlReporte.SelectedValue.Equals("Tickets"))
                                    {
                                        var date = Convert.ToDateTime(tbFechaInicial.Text);
                                        var year = date.ToString("yyyy");
                                        var month = date.ToString("MM");
                                        var serie = "";
                                        var rfcEmisor = "";
                                        var count = -1;
                                        if (!ddlPtoEmi.SelectedValue.Equals("0")) { serie = ddlPtoEmi.SelectedItem.Text; }
                                        if (!ddlEmisor.SelectedValue.Equals("0")) { rfcEmisor = ddlEmisor.SelectedItem.Text; }
                                        var Reptikets = new FacturaGlobalWeb.FacturaGlobalWeb();
                                        var RutBase = AppDomain.CurrentDomain.BaseDirectory;
                                        Reptikets.TicketsAsync(where, Session["CfdiVersion"].ToString(), month, year, serie, rfcEmisor, _dir, RutBase, Session["IDENTEMI"].ToString());
                                        (Master as SiteMaster).MostrarAlerta(this, "El reporte se esta generando", 4);
                                        gvRepTickets.DataBind();
                                        //if (informes.Tickets(where, Session["CfdiVersion"].ToString(), month, year, serie, rfcEmisor, out count))
                                        //{
                                        //    if (count <= 0)
                                        //    {
                                        //        ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('No hay registros de acuerdo a los criterios de busqueda', 4);", true);
                                        //        return;
                                        //    }
                                        //    var js = "downloadFile('" + _virtualDir.Replace(@"\", "/") + "');";
                                        //    ScriptManager.RegisterStartupScript(this, GetType(), "downloadViaJS", js, true);
                                        //    Response.Redirect("~/download.aspx?file=" + "reportes/" + _virtualDir.Replace(@"\", "/"));
                                        //}
                                        //else
                                        //{
                                        //    ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('" + informes.Ex.Message + "', 4);", true);
                                        //}
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('Selecciona fecha inicial y fecha final', 4);", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('Tienes que seleccionar ambas fechas', 4);", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('Tienes que seleccionar ambas fechas', 4);", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('Tienes que seleccionar un tipo de Reporte', 4);", true);
                    }
                }
                catch (Exception ea)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('" + ea.Message + "', 4);", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertBootBox", "alertBootBox('Verifique que los datos sean correctos', 4);", true);
            }
        }

        /// <summary>
        /// Handles the Click event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("RepGeneral.aspx", false);
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
        /// Handles the SelectedIndexChanged event of the ddlTipDoc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTipDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPtoEmi.Items.Clear();
            var nullItem = new ListItem("Todas", "0");
            nullItem.Selected = true;
            SqlDataSourcePtoEmi.SelectParameters["tipo"].DefaultValue = ddlTipDoc.SelectedValue;
            SqlDataSourcePtoEmi.DataBind();
            ddlPtoEmi.DataBind();
            ddlPtoEmi.Items.Add(nullItem);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlReporte control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlReporte.SelectedValue.Equals("General", StringComparison.OrdinalIgnoreCase))
            {
                if (ddlReporte.SelectedValue.Equals("Contabilidad", StringComparison.OrdinalIgnoreCase))
                {
                    if (Session["IDENTEMI"].ToString() != "HRP880129QX5")
                    {
                        //tbFechaFinal.Text = tbFechaInicial.Text;
                        tbFechaFinal.ReadOnly = false;
                    }
                    ddlTipDoc.SelectedValue = "0";
                    ddlTipDoc_SelectedIndexChanged(null, null);
                    ddlEstado.SelectedValue = "0";
                    ddlTipDoc.Enabled = false;
                    ddlPtoEmi.Enabled = false;
                    ddlEstado.Enabled = false;
                }
                else if (ddlReporte.SelectedValue.Equals("Tickets", StringComparison.OrdinalIgnoreCase))
                {
                    ddlReceptor.Enabled = false;
                    ddlTipDoc.SelectedValue = "01";
                    ddlTipDoc_SelectedIndexChanged(null, null);
                    ddlEstado.SelectedValue = "0";
                    ddlTipDoc.Enabled = false;
                    ddlPtoEmi.Enabled = true;
                    ddlEstado.Enabled = false;
                    tbFechaFinal.ReadOnly = false;
                }
                else
                {
                    tbFechaFinal.ReadOnly = false;
                    ddlTipDoc.SelectedValue = "0";
                    ddlTipDoc_SelectedIndexChanged(null, null);
                    ddlEstado.SelectedValue = "0";
                    ddlTipDoc.Enabled = false;
                    ddlPtoEmi.Enabled = false;
                    ddlEstado.Enabled = false;
                }
            }
            else
            {
                ddlTipDoc.Enabled = true;
                ddlPtoEmi.Enabled = true;
                ddlEstado.Enabled = true;
                tbFechaFinal.ReadOnly = false;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the tbFechaInicial control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbFechaInicial_TextChanged(object sender, EventArgs e)
        {
            if (ddlReporte.SelectedValue.Equals("Contabilidad", StringComparison.OrdinalIgnoreCase))
            {
                //tbFechaFinal.Text = tbFechaInicial.Text;
            }
            else if (ddlReporte.SelectedValue.Equals("Tickets", StringComparison.OrdinalIgnoreCase))
            {
                var date = Convert.ToDateTime(tbFechaInicial.Text);
                var year = date.ToString("yyyy");
                var month = date.ToString("MM");
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                tbFechaInicial.Text = firstDayOfMonth.ToString("yyyy/MM/dd");
                tbFechaFinal.Text = lastDayOfMonth.ToString("yyyy/MM/dd");
            }
        }

        /// <summary>
        /// Zips the download.
        /// </summary>
        /// <param name="ruta">The ruta.</param>
        protected void Zip_Download(string ruta)
        {
            var dir = new DirectoryInfo(ruta);
            dir.Create();
            var archivos = dir.GetFiles("*.txt");
            if (archivos.Length > 0)
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (var archivo in archivos)
                    {
                        zip.AddItem(archivo.FullName, "");
                    }
                    var ms = new MemoryStream();
                    zip.Save(ms);
                    var bytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(bytes);
                    var contentType = "application/x-zip-compressed";
                    var fileName = "FacturasContabilidad_" + Localization.Now.ToString("ddMMyyyy") + ".zip";
                    ScriptManager.RegisterStartupScript(this, GetType(), "_btnZipkeyCont", "downloadBytes('" + base64 + "','" + contentType + "','" + fileName + "', false);", true);
                }
                foreach (var archivo in archivos)
                {
                    archivo.Delete();
                }
            }

        }
        protected void bActRepTik_Click(object sender, EventArgs e)
        {
            gvRepTickets.DataBind();
        }
    }
}