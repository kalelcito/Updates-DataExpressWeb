using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using System.Xml;
namespace DataExpressWeb
{
    public partial class Trama : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}

            string file = Request.QueryString.Get("Trama");
            BasesDatos DB = new BasesDatos("");
            DbDataReader DR;
            string trama = "";
            DB = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            DB.Conectar();
            DB.CrearComando(@"select trama from Log_Archivos where id_Archivo = @id_Archivo");
            DB.AsignarParametroCadena("@id_Archivo", file);
            DR = DB.EjecutarConsulta();
            while (DR.Read())
            {
                trama = DR[0].ToString().Trim();
            }
            DB.Desconectar();

            trama = trama.Replace("/r", "\r");
            trama = trama.Replace("/n", "\n");

            tbTrama.Text = trama;
        }
    }
}