using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using System.IO;
namespace Administracion
{
    public partial class agregarSucursal : System.Web.UI.Page
    {
        private BasesDatos DB = new BasesDatos();
        HttpPostedFile file;

        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}




        }

        protected void bGuardar_Click(object sender, EventArgs e)
        {
           // if (!ValidarSucursal(tbClave.Text))
          //  {
            byte[] datos = null;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg"))
                datos = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg");
            if (datos != null)
            {
                string iddd = "";
                DB.Conectar();
                DB.CrearComando(@"INSERT INTO Cat_Sucursales
                (clave,sucursal,clavePtoEmision,domicilio,zona,tipo,telefono,division,eliminado)
                VALUES
                (@clave,@sucursal,@clavePtoEmision,@domicilio,@zona,@tipo,@telefono,@division,@eliminado)  SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];");
                DB.AsignarParametroCadena("@clave", tbClave.Text);
                DB.AsignarParametroCadena("@sucursal", tbSucursal.Text);
                DB.AsignarParametroCadena("@clavePtoEmision", "001");
                DB.AsignarParametroCadena("@domicilio", tbDireccion.Text);
                DB.AsignarParametroCadena("@zona", ddlZona.SelectedValue);
                DB.AsignarParametroCadena("@tipo", ddlTipo.SelectedValue);
                DB.AsignarParametroCadena("@telefono", tbTelefono.Text);
                DB.AsignarParametroCadena("@eliminado", "False");
                DB.AsignarParametroCadena("@division", "");
                //DB.AsignarParametroCadena("@logo", datos);
                //DB.EjecutarConsulta1();
                DbDataReader da = DB.EjecutarConsulta();
                if (da.Read())
                {
                    iddd = da[0].ToString();
                }
                DB.Desconectar();

                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_modificar_LogoSuc");
                DB.AsignarParametroProcedimiento("@idSucursal", System.Data.DbType.String, iddd);
                DB.AsignarParametroProcedimiento("@logo", System.Data.DbType.Binary, datos);
                DB.EjecutarConsulta1();
                DB.Desconectar();

                Response.Redirect("sucursales.aspx");
                /* }
                 else
                 {
                     lMsj.Text = "La sucursal ya existe.";
                 }*/
            }
            else
            {
                DB.Conectar();
                DB.CrearComando(@"INSERT INTO Cat_Sucursales
                (clave,sucursal,clavePtoEmision,domicilio,zona,tipo,telefono,division,eliminado)
                VALUES
                (@clave,@sucursal,@clavePtoEmision,@domicilio,@zona,@tipo,@telefono,@division,@eliminado)");
                DB.AsignarParametroCadena("@clave", tbClave.Text);
                DB.AsignarParametroCadena("@sucursal", tbSucursal.Text);
                DB.AsignarParametroCadena("@clavePtoEmision", "001");
                DB.AsignarParametroCadena("@domicilio", tbDireccion.Text);
                DB.AsignarParametroCadena("@zona", ddlZona.SelectedValue);
                DB.AsignarParametroCadena("@tipo", ddlTipo.SelectedValue);
                DB.AsignarParametroCadena("@telefono", tbTelefono.Text);
                DB.AsignarParametroCadena("@eliminado", "False");
                DB.AsignarParametroCadena("@division", "");
                DB.EjecutarConsulta1();
                DB.Desconectar();
                Response.Redirect("sucursales.aspx");
            }
        }

        private Boolean ValidarSucursal(string claveSucursal)
        {
            DB.Conectar();
            DB.CrearComando(@"select * from Cat_sucursales
            where clave=@clave and clavePtoEmision=@clavePtoEmision
                    and eliminado='False'");
            DB.AsignarParametroCadena("@clave", claveSucursal);

            DbDataReader DR = DB.EjecutarConsulta();

            while (DR.Read())
            {
                DB.Desconectar();
                return true;
            }
            DB.Desconectar();
            return false;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + @"imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg");
                Image3.ImageUrl = @"~/imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg";
                file = FileUpload1.PostedFile;
                bGuardar0.Text = file.ContentType.ToString();
            }
            else
            {
                lMsj.Text = "Tienes que seleccionar el archivo";
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("sucursales.aspx");
        }

    }
}