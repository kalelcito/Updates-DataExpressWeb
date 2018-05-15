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
    public partial class AgregarCaja : System.Web.UI.Page
    {
        private BasesDatos DB = new BasesDatos();
        ArrayList Limpresoras;
        string[] arrayImpresoras;
        int posIpresora;
        Boolean banImpresora = false;

        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}

            Limpresoras = new ArrayList();
            if (!IsPostBack)
            {




            }
        }

        protected void btAgregar_Click(object sender, EventArgs e)
        {

            DB.Conectar();
            DB.CrearComando(@"INSERT INTO CajaSucursal
               (idSucursal,NumeroRentas,estab,ptoEmi,estadoPro,nombrePtoEmi,impresora)
                VALUES
                (@idSucursal,@NumeroRentas,@estab,@ptoEmi,@estadoPro,@nombre,@impresora)");
            DB.AsignarParametroCadena("@idSucursal", dllEstab.SelectedValue);
            DB.AsignarParametroCadena("@NumeroRentas", dllTipoDoc.SelectedValue);
            DB.AsignarParametroCadena("@estab", Convert.ToString(dllEstab.SelectedItem));
            DB.AsignarParametroCadena("@ptoEmi", tbPtomi.Text);
            DB.AsignarParametroCadena("@estadoPro", ddlAmbiente.SelectedValue);
            DB.AsignarParametroCadena("@nombre", tbNombre.Text);
            DB.AsignarParametroCadena("@impresora", Convert.ToString(ddlImpresora.SelectedItem));
            DB.EjecutarConsulta1();
            DB.Desconectar();




            Response.Redirect("~/adminstracion/sucursales/CajaSucursal/CajaSucursal.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/adminstracion/sucursales/CajaSucursal/CajaSucursal.aspx");
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

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            impresoras();
            foreach (String[] obj in Limpresoras)
            {
                ddlImpresora.Items.Add(obj[1]);
            }

        }
    }
}