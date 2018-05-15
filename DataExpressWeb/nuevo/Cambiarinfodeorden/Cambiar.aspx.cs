using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Control;
using System.Data.Common;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace DataExpressWeb
{
    public partial class CambiarDatos : System.Web.UI.Page
    {
        String fechanom, fechacreacion, busqueda;
        string[] nombrearchivo;
        string[] datos;
        String dirorigen;
        string doc, dirtxt;
        int count;
        Lectura lectura;
        private BasesDatos DB = new BasesDatos();

        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            tbnoOrden.Attributes.Add("onkeyup", "changeToUpperCase(this.id)");
            tbnoOrden.Attributes.Add("onchange", "changeToUpperCase(this.id)");
            lectura = new Lectura(dirorigen, dirtxt, doc);
        }

        protected void consultar_Click(object sender, EventArgs e)
        {
            try
            {
                lbMsg.Text = "";
                if (tbnoOrden.Text.Length != 0)
                {
                    DB.Conectar();
                    DB.CrearComando("select top 1 dirrespaldo, dirorigen from ParametrosSistema");
                    DbDataReader DR1 = DB.EjecutarConsulta();
                    if (DR1.Read())
                    {
                        dirtxt = DR1[0].ToString();
                        dirorigen = DR1[1].ToString();
                    }
                    DB.Desconectar();

                    nombrearchivo = System.IO.Directory.GetFiles(dirtxt, "*" + tbnoOrden.Text + "*.xml", SearchOption.AllDirectories);
                    datos = lectura.leer(nombrearchivo[0]);
                    tbRUC.Text = datos[0];
                    tbID.Text = datos[1];
                    tbdireccion.Text = datos[4];
                    tbNombre.Text = datos[5];
                    tbEmail.Text = datos[6];
                }
                else
                {
                    lbMsg.Text = "Necesita colocar un número de orden.";
                }
            }
            catch(Exception ex)
            {
                lbMsg.Text = "No se encontró un archivo con este número de orden.";
                lbMsg.Text += ex.ToString();
            }
        }


        protected void Actualizar_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                DB.Conectar();
                DB.CrearComando("select top 1 dirrespaldo, dirorigen from ParametrosSistema");
                DbDataReader DR1 = DB.EjecutarConsulta();
                if (DR1.Read())
                {
                    dirtxt = DR1[0].ToString();
                    dirorigen = DR1[1].ToString();
                }
                DB.Desconectar();

                lbMsg.Text = "";
                nombrearchivo = System.IO.Directory.GetFiles(dirtxt, "*" + tbnoOrden.Text + "*.xml", SearchOption.AllDirectories);
                FileInfo file = new FileInfo(nombrearchivo[0]);


                System.IO.File.Copy(nombrearchivo[0], nombrearchivo[0].Replace( file.Name, "Resp_" + file.Name));

                xDoc.Load(nombrearchivo[0]);

                XmlElement OrderHeader = (XmlElement)xDoc.GetElementsByTagName("OrderHeader")[0];
                XmlElement General = (XmlElement)OrderHeader.GetElementsByTagName("General")[0];
                XmlElement VAT_Number = (XmlElement)OrderHeader.GetElementsByTagName("VAT_Number")[0];
                if (tbID.Text!=null)
                    VAT_Number.InnerText = tbID.Text.Trim();

                XmlElement DistributorDetails = (XmlElement)OrderHeader.GetElementsByTagName("DistributorDetails")[0];
                XmlElement BillTo = (XmlElement)DistributorDetails.GetElementsByTagName("BillTo")[0];
                XmlElement billName = (XmlElement)OrderHeader.GetElementsByTagName("Name")[0];
                if (tbNombre.Text!=null)
                    billName.InnerText = tbNombre.Text.Trim();

                XmlElement BillToAddress = (XmlElement)OrderHeader.GetElementsByTagName("BillToAddress")[0];
                if (tbdireccion.Text!=null)
                    BillToAddress.InnerText = tbdireccion.Text.Trim();

                XmlElement NationalIDs = (XmlElement)DistributorDetails.GetElementsByTagName("NationalIDs")[0];
                XmlElement NationalID1 = (XmlElement)NationalIDs.GetElementsByTagName("NationalID1")[0];
                if (tbRUC.Text != null)
                    NationalID1.InnerText = tbRUC.Text.Trim();

                XmlElement Primary_EmailAddress = (XmlElement)DistributorDetails.GetElementsByTagName("Primary_EmailAddress")[0];
                if (tbEmail.Text != null)
                    Primary_EmailAddress.InnerText = tbEmail.Text.Trim();

                xDoc.Save(dirorigen + "Modificado_" + tbnoOrden.Text.Trim() + ".xml");
                //xDoc.Save(@"F:\DataExpress\InvoiceMx\Web\ModificarOrdenes\" + "Modificado_" + tbnoOrden.Text + ".xml");
                //

                lbMsg.Text = "Se ha guardado Correctamente la Orden.";
            }
            catch (Exception ex)
            {

                lbMsg.Text = "Ocurrio un error al guardar la Orden." + ex.ToString();
            }
        }
    }
}
