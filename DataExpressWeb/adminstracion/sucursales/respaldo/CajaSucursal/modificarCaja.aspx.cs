using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using System.Collections;
namespace DataExpressWeb
{
    public partial class modificarCaja : System.Web.UI.Page
    {
        string idCaja="";
        private BasesDatos DB = new BasesDatos();
        ArrayList Limpresoras;
        string[] arrayImpresoras;
        int posIpresora;
        Boolean banImpresora = false;
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            Limpresoras = new ArrayList();
            idCaja = Request.QueryString.Get("idmrdxbdi");
            if (!Page.IsPostBack)
            {
                idCaja = Request.QueryString.Get("idmrdxbdi");

                if (!String.IsNullOrEmpty(idCaja))
                {
                    try
                    {
                        SqlDataSourceSucursal.SelectCommand = "SELECT [clave], [idSucursal] FROM [Cat_Sucursales]   where eliminado='false'";
                        SqlDataSourceSucursal.DataBind();

                        DB.Conectar();
                        DB.CrearComandoProcedimiento("PA_consulta_cajaSucursalUpadate");
                        DB.AsignarParametroProcedimiento("@idCaja", System.Data.DbType.String, idCaja);
                        DbDataReader DR = DB.EjecutarConsulta();
                        DR.Read();
                        dllTipoDoc.SelectedValue = DR[0].ToString();
                        dllEstab.SelectedValue = DR[1].ToString();
                        tbPtomi.Text = DR[2].ToString();
                        ddlAmbiente.SelectedValue = DR[3].ToString();
                        tbNombre.Text = DR[4].ToString();
                        //ddlImpresora.SelectedValue = DR[5].ToString();
                        tbImpresora.Text = DR[5].ToString();

                        DB.Desconectar();

                    }
                    catch (Exception exce)
                        {
                            Label7.Text = "Error en consulta";
                        }
                }


            }
        }



        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/adminstracion/sucursales/CajaSucursal/CajaSucursal.aspx");
        }

        protected void btAgregar_Click(object sender, EventArgs e)
        {



            if (!String.IsNullOrEmpty(idCaja))
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_modificarCaja");
                DB.AsignarParametroProcedimiento("@idCaja", System.Data.DbType.String, idCaja);
                DB.AsignarParametroProcedimiento("@NumeroRentas", System.Data.DbType.String, dllTipoDoc.SelectedValue);
                DB.AsignarParametroProcedimiento("@estab", System.Data.DbType.String,Convert.ToString(dllEstab.SelectedValue));
                DB.AsignarParametroProcedimiento("@ptoEmi", System.Data.DbType.String, tbPtomi.Text);
                DB.AsignarParametroProcedimiento("@estadoPro", System.Data.DbType.String, ddlAmbiente.SelectedValue);
                DB.AsignarParametroProcedimiento("@nombre", System.Data.DbType.String, tbNombre.Text);
                DB.AsignarParametroProcedimiento("@impresora ", System.Data.DbType.String,tbImpresora.Text);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                Response.Redirect("CajaSucursal.aspx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            impresoras();
            foreach (String[] obj in Limpresoras)
            {
                ddlImpresora.Items.Add(obj[1]);
            }
        }
        public void impresoras()
        {

            for (int a = 0; a < System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count; a++)
            {
                if (!string.IsNullOrEmpty(System.Drawing.Printing.PrinterSettings.InstalledPrinters[a]))
                {
                    arrayImpresoras = new string[2];
                    arrayImpresoras[0] = a.ToString();
                    arrayImpresoras[1] = System.Drawing.Printing.PrinterSettings.InstalledPrinters[a];
                    Limpresoras.Add(arrayImpresoras);
                }
                //lMsj.Text = lMsj.Text + "impresora [" + a + "] =" + System.Drawing.Printing.PrinterSettings.InstalledPrinters[a] + Environment.NewLine;
            }
        }

        protected void ddlImpresora_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbImpresora.Text = "";

            tbImpresora.Text = Convert.ToString(ddlImpresora.SelectedItem);
        }

        protected void btnCancelar_Click1(object sender, EventArgs e)
        {
            Response.Redirect("CajaSucursal.aspx");
        }
    }
}