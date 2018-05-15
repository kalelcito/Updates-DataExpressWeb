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
    public partial class MensajeArchivo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            string idArc = Request.QueryString.Get("Mensaje");

            BasesDatos DB = new BasesDatos("");
            DbDataReader DR;
            string mensaje = "";
            string mensajeTecnico = "";
            DB = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            DB.Conectar();
            DB.CrearComando(@"select mensaje, MensajeTecnico from Log_Archivos where id_Archivo = @id_Archivo");
            DB.AsignarParametroCadena("@id_Archivo", idArc);
            DR = DB.EjecutarConsulta();
            while (DR.Read())
            {
                mensaje = DR[0].ToString().Trim();
                mensajeTecnico = DR[1].ToString().Trim();
            }
            DB.Desconectar();

            tbMensaje.Text = mensaje;
            tbMensajeTecnico.Text = mensajeTecnico;
        }
    }
}