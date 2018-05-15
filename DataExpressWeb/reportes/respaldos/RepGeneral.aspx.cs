using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Control;
using System.Data.Common;

namespace DataExpressWeb
{
    public partial class RepGeneral : System.Web.UI.Page
    {
        String fecha, fechanom, fechacreacion;
        String dir;
        int count;
        string ptoEMi = "";
        private BasesDatos DB = new BasesDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            if (Session["idUser"] != null)
            {
                if (!Convert.ToBoolean(Session["coFactTodas"]))
                {
                    ddlPtoEmi.SelectedValue = Session["ptoEmi"].ToString();
                    ddlSucursal.SelectedValue = Session["claveSucursal"].ToString();
                    ddlPtoEmi.Enabled = false;
                    ddlSucursal.Enabled = false;
                }
            }
        }

        protected void bGenerar_Click(object sender, EventArgs e)
        {
            string estab = "";
            string ptoEmision = "";
            string where = "";
            try
            {
                count = 0;
                if (!ddlReporte.SelectedValue.Equals("0"))
                {

                    if (!cFechaInicial.SelectedDate.ToString("yyyyMMdd").Equals("00010101") &&
                    !cFechaFinal.SelectedDate.ToString("yyyyMMdd").Equals("00010101")
                    )
                    {
                        if (cFechaInicial.SelectedDate <= cFechaFinal.SelectedDate)
                        {
                            fecha = cFechaInicial.SelectedDate.ToString("yyyyMMdd");
                            //sucursal = ddlSucursal.SelectedValue;
                            fechanom = cFechaFinal.SelectedDate.ToString("yyyyMMdd");
                            fechacreacion = System.DateTime.Now.ToString("yyyyMMddHHmm");


                            dir = System.AppDomain.CurrentDomain.BaseDirectory + @"reportes\docs\" + fechacreacion;
                            // fecha = Convert.ToString(DateTime.Now.Day) + "/" + Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Year);
                            //fecha = calentario.SelectedDate.ToShortDateString();
                            // sucursal = ddlSucursal.SelectedValue;
                            where += " CONVERT(VARCHAR,GENERAL.FECHA,112) >= " + cFechaInicial.SelectedDate.ToString("yyyyMMdd") +
                                     " AND CONVERT(VARCHAR,GENERAL.FECHA,112) <=" + cFechaFinal.SelectedDate.ToString("yyyyMMdd") + " AND ";

                            if (ddlSucursal.SelectedValue != "0")
                            {
                                where += " GENERAL.estab='" + ddlSucursal.SelectedValue + "' AND ";

                            }
                             if (ddlPtoEmi.SelectedValue != "0")
                            {
                                where += " GENERAL.ptoemi='" + ddlPtoEmi.SelectedValue + "' AND ";

                            }
                             if (ddlEstado.SelectedValue != "0")
                            {
                                where += " (GENERAL.tipo +GENERAL.estado) =  '" + ddlEstado.SelectedValue + "' AND ";

                            }
                             if (ddlTipDoc.SelectedValue != "0")
                            {
                                where += " GENERAL.codDoc='" + ddlTipDoc.SelectedValue + "' AND ";
                            }

                            where = where.Substring(0, where.Length - 5);

                            if (ddlReporte.SelectedValue.Equals("General"))
                            {
                                DB.Conectar();
                                DB.CrearComando(@"SELECT  count(General.idComprobante)
                                            FROM            GENERAL INNER JOIN
                                                RECEPTOR ON GENERAL.id_Receptor = RECEPTOR.IDEREC
                                            WHERE " + where);

                                DbDataReader DR = DB.EjecutarConsulta();
                                if (DR.Read())
                                {
                                    count = Convert.ToInt32(DR[0]);
                                }
                                DB.Desconectar();
                                if (count > 0)
                                {
                                    RepSucursal reporteSuc = new RepSucursal(dir, fechanom, where, ddlReporte.SelectedValue);

                                    if (reporteSuc.getError() != null)
                                    {
                                        Response.Redirect(@"docs\" + fechacreacion + ddlReporte.SelectedValue + ".xls", false);
                                    }
                                    else
                                    {
                                        Label2.Text = reporteSuc.getError();
                                    }
                                }
                                else
                                {
                                    Label2.Text = "No hay registros de acuerdo a los criterios de busqueda";
                                }
                            }
                            else if (ddlReporte.SelectedValue.Equals("Retenciones"))
                            {
                                DB.Conectar();
                                DB.CrearComando(@"SELECT  count(General.idComprobante)
                                              FROM            RECEPTOR INNER JOIN
                                                GENERAL ON RECEPTOR.IDEREC = GENERAL.id_Receptor INNER JOIN
                                                TotalConImpuestos ON GENERAL.idComprobante = TotalConImpuestos.id_Comprobante
                                            WHERE " + where);

                                DbDataReader DR = DB.EjecutarConsulta();
                                if (DR.Read())
                                {
                                    count = Convert.ToInt32(DR[0]);
                                }
                                DB.Desconectar();
                                if (count > 0)
                                {
                                    RepSucursal reporteSuc = new RepSucursal(dir, fechanom, where, ddlReporte.SelectedValue);
                                    if (reporteSuc.getError() != null)
                                    {
                                        Response.Redirect(@"docs\" + fechacreacion + ddlReporte.SelectedValue + ".xls", false);
                                    }
                                    else
                                    {
                                        Label2.Text = reporteSuc.getError();
                                    }
                                }
                                else
                                {
                                    Label2.Text = "No hay registros de acuerdo a los criterios de busqueda";
                                }
                            }
                            else if (ddlReporte.SelectedValue.Equals("NA"))
                            {
                                DB.Conectar();
                                DB.CrearComando(@"SELECT  COUNT(General.idComprobante)
                                      FROM
                                        GENERAL
                                     WHERE " + where);

                                DbDataReader DR = DB.EjecutarConsulta();
                                if (DR.Read())
                                {
                                    count = Convert.ToInt32(DR[0]);
                                }
                                DB.Desconectar();
                                if (count > 0)
                                {
                                    RepSucursal reporteSuc = new RepSucursal(dir, fechanom, where, ddlReporte.SelectedValue);
                                    if (reporteSuc.getError() != null)
                                    {
                                        Response.Redirect(@"docs\" + fechacreacion + ddlReporte.SelectedValue + ".xls", false);
                                    }
                                    else
                                    {
                                        Label2.Text = reporteSuc.getError();
                                    }
                                }
                                else
                                {
                                    Label2.Text = "No hay registros de acuerdo a los criterios de busqueda";
                                }
                            }
                            else if (ddlReporte.SelectedValue.Equals("Email"))
                            {
                                DB.Conectar();
                                DB.CrearComando(@"SELECT  COUNT(GENERAL.idComprobante)
                                              FROM   emailEnvio inner join
			                                       general on GENERAL.idComprobante = emailEnvio.id_general INNER JOIN
			                                       RECEPTOR ON RECEPTOR.IDEREC = GENERAL.id_Receptor
                                             WHERE " + where);
                                DbDataReader DR = DB.EjecutarConsulta();
                                if (DR.Read())
                                {
                                    count = Convert.ToInt32(DR[0]);
                                }
                                DB.Desconectar();
                                if (count > 0)
                                {
                                    RepSucursal reporteSuc = new RepSucursal(dir, fechanom, where, ddlReporte.SelectedValue);
                                    if (reporteSuc.getError() != null)
                                    {
                                        Response.Redirect(@"docs\" + fechacreacion + ddlReporte.SelectedValue + ".xls",false);
                                    }
                                    else
                                    {
                                        Label2.Text = reporteSuc.getError();
                                    }
                                }
                                else
                                {
                                    Label2.Text = "No hay registros de acuerdo a los criterios de busqueda";
                                }
                            }
                        }
                        else
                        {
                            Label2.Text = "Selecciona fecha inical y fecha final";
                        }

                    }
                    else
                    {
                        Label2.Text = "Tienes que seleccionar ambas fechas.";
                    }
                }
                else {
                    Label2.Text = "Tienes que seleccionar un tipo de Reporte";
                }
            }
            catch (Exception ea) { Label2.Text = ea.Message; }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("RepGeneral.aspx", false);
        }

        protected void ddlReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTipDoc.SelectedValue = "0";
            ddlEstado.SelectedValue = "0";
            ddlPtoEmi.Items.Clear();
            ddlPtoEmi.Items.Add(new ListItem("Todos", "0"));
            ddlPtoEmi.DataBind();
            ddlPtoEmi.SelectedValue = "0";

            ddlTipDoc.Items.Clear();
            ddlTipDoc.Items.Add(new ListItem("Todos", "0"));
            ddlTipDoc.DataBind();
            ddlTipDoc.Items.Remove(new ListItem("Comprobante de Retenciones", "07"));
            ddlTipDoc.SelectedValue = "0";
            ddlTipDoc.Enabled = true;

            ddlEstado.Enabled = true;

            if (ddlReporte.SelectedValue.Equals("General"))
            {


            }
            else
                if (ddlReporte.SelectedValue.Equals("Retenciones"))
            {
                ddlTipDoc.Items.Clear();
                ddlTipDoc.Items.Add(new ListItem("Todos", "0"));
                ddlTipDoc.DataBind();
                ddlTipDoc.SelectedValue = "07";
                ddlTipDoc.Enabled = false;
            }
            //else if (ddlReporte.SelectedValue.Equals("Email"))
            //{

            //}
            else if (ddlReporte.SelectedValue.Equals("NA"))
            {
                ddlEstado.SelectedValue = "N0";
                ddlEstado.Enabled = false;
            }
        }

        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPtoEmi.Items.Clear();
            ddlPtoEmi.Items.Add(new ListItem("Todos", "0"));
            ddlPtoEmi.DataBind();
            ddlPtoEmi.SelectedValue = "0";
        }
    }
}