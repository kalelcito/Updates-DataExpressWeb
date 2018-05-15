// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
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

namespace DataExpressWeb.recepcion
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
        /// The _DBr
        /// </summary>
        private BasesDatos _dbr;
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
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            _dbr = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE", "Recepcion");
            SqlDataSourceTipDoc.ConnectionString = _dbr.CadenaConexion;
            SqlDataSourceEmisor.ConnectionString = _dbr.CadenaConexion;
            SqlDataSourceReceptor.ConnectionString = _dbr.CadenaConexion;
            Session["_isWorkflow"] = IsWorkflow(); //Usage: Convert.ToBoolean(Session["_isWorkflow"])
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
                    if (Session["USERNAME"].ToString().StartsWith("PROVE"))
                    {
                        ddlEmisor.DataBind();
                        try
                        {
                            var rfc = Session["rfcCliente"].ToString();
                            var item = ddlEmisor.Items.FindByText(rfc);
                            ddlEmisor.SelectedValue = item.Value;
                            ddlEmisor.Enabled = false;
                        }
                        catch { }
                        //ddlReceptor.DataBind();
                        //try
                        //{
                        //    var rfc = Session["IDENTEMI"].ToString();
                        //    var item = ddlReceptor.Items.FindByText(rfc);
                        //    ddlReceptor.SelectedValue = item.Value;
                        //    ddlReceptor.Enabled = false;
                        //}
                        //catch { }
                    }
                }
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
                                    if (ddlTipoFecha.SelectedValue.Equals("1"))
                                    {
                                        // Fecha de Recepción
                                        where += " CONVERT(VARCHAR(MAX),g.fechaRecepcion,112) >= " + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + " AND CONVERT(VARCHAR(MAX),g.fechaRecepcion,112) <=" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + " AND ";
                                    }
                                    else
                                    {
                                        // Fecha de Emisión
                                        where += " CONVERT(VARCHAR(MAX),g.FECHA,112) >= " + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + " AND CONVERT(VARCHAR(MAX),g.FECHA,112) <=" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + " AND ";
                                    }
                                    if (ddlEstado.SelectedValue != "0")
                                    {
                                        where += " (g.tipo +g.estado) =  '" + ddlEstado.SelectedValue + "' AND ";
                                    }
                                    if (ddlEstadoValidacion.SelectedValue != "0")
                                    {
                                        where += " g.estadoValidacion = '" + ddlEstadoValidacion.SelectedValue + "' AND ";
                                    }
                                    if (ddlTipDoc.SelectedValue != "0")
                                    {
                                        where += " g.codDoc='" + ddlTipDoc.SelectedValue + "' AND ";
                                    }
                                    if (ddlEmisor.SelectedValue != "0")
                                    {
                                        where += " cem.RFCEMI='" + ddlEmisor.SelectedItem.Text + "' AND ";
                                    }
                                    if (ddlReceptor.SelectedValue != "0")
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
                                        _virtualDir = @"docs\" + "U" + new string((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE").Take(3).ToArray()) + _fecha + ".txt";
                                    }
                                    _dir = AppDomain.CurrentDomain.BaseDirectory + @"recepcion\" + _virtualDir;
                                    Directory.CreateDirectory(Path.GetDirectoryName(_dir));
                                    var periodo = tbFechaInicial.Text + " - " + tbFechaFinal.Text;
                                    var informes = new Informes(_dir, _dbr.CadenaConexion, periodo, Session["IDGIRO"].ToString(), true);
                                    if (ddlReporte.SelectedValue.Equals("General"))
                                    {
                                        _dbr.Conectar();
                                        _dbr.CrearComando(@"SELECT  count(g.idComprobante)
                                            FROM            Dat_GENERAL g INNER JOIN
                                                Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
                                                Cat_RECEPTOR cr ON g.id_Receptor = cr.IDEREC
                                            WHERE " + where);

                                        var dr = _dbr.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            _count = Convert.ToInt32(dr[0]);
                                        }
                                        _dbr.Desconectar();
                                        if (_count > 0)
                                        {
                                            if (informes.General(where))
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
                                        _dbr.Conectar();
                                        _dbr.CrearComando(@"SELECT  COUNT(g.idComprobante)
                                              FROM   Dat_Detalles dd inner join
			                                       Dat_general g on g.idComprobante = dd.id_Comprobante INNER JOIN
                                                   Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
			                                       Cat_RECEPTOR cr ON cr.IDEREC = g.id_Receptor
                                             WHERE " + where);
                                        var dr = _dbr.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            _count = Convert.ToInt32(dr[0]);
                                        }
                                        _dbr.Desconectar();
                                        if (_count > 0)
                                        {
                                            if (informes.Conceptos(where))
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
                    tbFechaFinal.Text = tbFechaInicial.Text;
                    tbFechaFinal.ReadOnly = true;
                }
                else
                {
                    tbFechaFinal.ReadOnly = false;
                }
                ddlTipDoc.SelectedValue = "0";
                ddlEstado.SelectedValue = "0";
                ddlTipDoc.Enabled = false;
                ddlEstado.Enabled = false;
            }
            else
            {
                ddlTipDoc.Enabled = true;
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
                tbFechaFinal.Text = tbFechaInicial.Text;
            }
        }
    }
}