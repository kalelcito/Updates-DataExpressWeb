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
    public partial class RepRetenciones : System.Web.UI.Page
    {
        String fecha, fechanom, fechacreacion;
        string fechaInicial = "", fechaFinal = "";
        String dir;
        int count;
        private BasesDatos DB = new BasesDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}

        }

        protected void bGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                count = 0;
                fechaInicial = tbFechaInicial.Text;
                fechaFinal = tbFechaFinal.Text;
                if (!String.IsNullOrEmpty(fechaInicial) && !String.IsNullOrEmpty(fechaFinal))
                {
                    if ((Convert.ToInt32(fechaInicial.Replace(@"/", "")) < Convert.ToInt32(fechaFinal.Replace(@"/", ""))) || (fechaInicial == fechaFinal))
                    {
                        //fecha = calentario.SelectedDate.ToShortDateString();
                        fecha = fechaInicial;
                        //sucursal = ddlSucursal.SelectedValue;
                        //fechanom = calendario2.SelectedDate.ToShortDateString();
                        fechanom = fechaFinal;
                        fechacreacion = System.DateTime.Now.ToString("yyyyMMddHHmm");


                        dir = System.AppDomain.CurrentDomain.BaseDirectory + @"reportes\docs\" + fechacreacion;
                        // fecha = Convert.ToString(DateTime.Now.Day) + "/" + Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Year);
                        // fecha = calentario.SelectedDate.ToShortDateString();
                        fecha = fechaInicial.Replace(@"/", "");
                        // sucursal = ddlSucursal.SelectedValue;
                        DB.Conectar();
                        DB.CrearComando(@"set dateformat ymd SELECT  count(Dat_General.idComprobante)
                                      FROM
                                        Dat_Archivos INNER JOIN
                                        Dat_General ON Dat_Archivos.IDEFAC = Dat_General.idComprobante INNER JOIN
                                        Cat_Emisor ON Dat_General.id_Emisor = Cat_Emisor.IDEEMI INNER JOIN
                                        Cat_Receptor ON Dat_General.id_Receptor = Cat_Receptor.IDEREC INNER JOIN
					                    Cat_Catalogo1_C ON Dat_General.codDoc = Cat_Catalogo1_C.codigo AND Cat_Catalogo1_C.tipo = 'Comprobante'
                                     WHERE
                                        Dat_General.fecha>=@fechaIni AND Dat_General.fecha<=@fechaFin and Dat_General.codDoc = '07'");
                        DB.AsignarParametroCadena("@fechaIni", fecha);
                        DB.AsignarParametroCadena("@fechaFin", fechanom + " 23:59:58");
                        DbDataReader DR = DB.EjecutarConsulta();
                        if (DR.Read())
                        {
                            count = Convert.ToInt32(DR[0]);
                        }
                        DB.Desconectar();
                        if (count > 0)
                        {
                            RepSucursal reporteSuc = new RepSucursal(dir, fecha, fechanom, "07",false,false,"","");
                            if (reporteSuc.getError() != null)
                            {
                                Response.Redirect(@"docs\" + fechacreacion + ".xls");
                            }
                            else
                            {
                                Label2.Text = reporteSuc.getError();
                            }
                        }
                        else
                        {
                            Label2.Text = "No se han elaborado retenciones durante estas fechas";
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
            catch (Exception ea) { Label2.Text = ea.Message; }
        }
    }
}