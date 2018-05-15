using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using Control;
using System.Data.SqlClient;

namespace DataExpressWeb
{
    public partial class VerRegistros : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            otro();
        }

        public void otro() {
            SqlDataSource2.SelectCommand = buscar();
            SqlDataSource2.DataBind();
            gv.DataBind();
            SqlDataSource2.Dispose();
            gv.Dispose();
        }

        protected void gv_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        public string buscar()
        {
            string consulta = "";
            string auxcons = "";
            if (TextBoxNombre.Text.Length != 0)
            {
                auxcons += "nombre like '%" + TextBoxNombre.Text + "%'";
            }
            if (TextBoxTipo.Text.Length != 0)
            {
                if (auxcons.Length != 0)
                {
                    auxcons += " and tipo like '%" + TextBoxTipo.Text + "%'";
                }
                else
                {
                    auxcons += "tipo like '%" + TextBoxTipo.Text + "%'";
                }
            }
            if (TextBoxNoOrden.Text.Length != 0)
            {
                if (auxcons.Length != 0)
                {
                    auxcons += " and noorden like '%" + TextBoxNoOrden.Text + "%'";
                }
                else
                {
                    auxcons += "noorden like '%" + TextBoxNoOrden.Text + "%'";
                }
            }
            if (TextBoxFecha.Text.Length != 0)
            {
                if (auxcons.Length != 0)
                {
                    auxcons += " and fecha like '%" + TextBoxFecha.Text + "%'";
                }
                else
                {
                    auxcons += "fecha like '%" + TextBoxFecha.Text + "%'";
                }
            }
            if (auxcons.Length != 0)
            {
                auxcons = " where " + auxcons;
            }
            consulta = "SELECT nombre, tipo, noorden, fecha FROM RegistroArchivos" + auxcons + " order by idArchivo desc";
            return consulta;
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            buscar();
        }

        protected void gv_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }
    }
}