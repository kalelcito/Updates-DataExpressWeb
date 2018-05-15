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
    public partial class ProcesandoRec : System.Web.UI.Page
    {
        BasesDatos DB = new BasesDatos();

        Log log = new Log();
        String msj = "";
        string idUser;

        string detalle;
        string fecha;
        string numeroDocumento;
        string detalleTecnico;
        string claveAcceso = "";
        int countTimer = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            if (Session["idUser"] != null)
            {
                idUser = Session["idUser"].ToString();
                claveAcceso = Session["claveAcceso"].ToString();
                if (!Page.IsPostBack)
                {
                    hdCount.Value = countTimer.ToString();
                }
                // lSesion.Text = (string)(Session["rfcUser"]);
            }
        }


        protected void timer1_tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            hdCount.Value = (Convert.ToInt32(hdCount.Value) + 1).ToString();
            this.countTimer = this.countTimer + 1;
            DB.Conectar();
            DB.CrearComando(@"SELECT claveAcceso FROM General WHERE claveAcceso=@claveAcceso and creado='1'");
            DB.AsignarParametroCadena("@claveAcceso", claveAcceso);
            DbDataReader DRDT = DB.EjecutarConsulta();
            if (DRDT.Read())
            {
                Timer1.Enabled = false;
                Response.Redirect("~/Validar.aspx");
            }
            DB.Desconectar();

            if (hdCount.Value.Equals("18"))
            {
                Timer1.Enabled = false;
                Response.Redirect("~/Validar.aspx");
            }
            Timer1.Enabled = true;
        }


        private void HistoryBack()
        {
            string key = "historyback";
            string javascript = @"<SCRIPT Language=JavaScript>
                            window.onload = function() {
                              history.back()
                            };
                        </SCRIPT>";

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), key, javascript, true);
            }
        }
    }
}
