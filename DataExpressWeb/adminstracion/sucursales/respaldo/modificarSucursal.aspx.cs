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
    public partial class modificarSucursal : System.Web.UI.Page
    {
        string idSucursal;

        private BasesDatos DB = new BasesDatos();
        HttpPostedFile file;


        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            if (Session["idUser"] != null)
            {
                if (!Page.IsPostBack)
                {
                    idSucursal = Request.QueryString.Get("id");
                    DB.Conectar();
                    DB.CrearComando(@"SELECT clave,sucursal,domicilio,
                                      zona,tipo,telefono  FROM Cat_Sucursales
                                    WHERE idSucursal = @idSucursal");
                    DB.AsignarParametroCadena("@idSucursal",idSucursal);
                    DbDataReader DR = DB.EjecutarConsulta();
                    if (DR.Read())
                    {
                        tbClave.Text = DR[0].ToString();
                        tbSucursal.Text = DR[1].ToString();
                        tbDireccion.Text = DR[2].ToString();
                        ddlZona.SelectedValue = DR[3].ToString();
                        ddlTipo.SelectedValue = DR[4].ToString();
                        tbTelefono.Text = DR[5].ToString();
                    }
                    DB.Desconectar();

                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg"))
                    {
                        Image3.ImageUrl = @"~/imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg";
                    }
                }
            }
        }

        protected void bModificar_Click(object sender, EventArgs e)
        {

           idSucursal = Request.QueryString.Get("id");
          // if (!ValidarSucursal(tbClave.Text,idSucursal))
           //{
           byte[] datos = null;
           if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg"))
               datos = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg");
               DB.Conectar();
               DB.CrearComando(@"UPDATE Cat_Sucursales
               SET
                clave = @clave,sucursal = @sucursal,clavePtoEmision = @clavePtoEmision,
                domicilio = @domicilio,zona = @zona,tipo = @tipo,telefono = @telefono,
                division = @division, logo = @logo
               WHERE idSucursal=@idSucursal");
               DB.AsignarParametroCadena("@clave", tbClave.Text);
               DB.AsignarParametroCadena("@sucursal", tbSucursal.Text);
               DB.AsignarParametroCadena("@clavePtoEmision", "001");
               DB.AsignarParametroCadena("@domicilio", tbDireccion.Text);
               DB.AsignarParametroCadena("@zona", ddlZona.SelectedValue);
               DB.AsignarParametroCadena("@tipo", ddlTipo.SelectedValue);
               DB.AsignarParametroCadena("@telefono", tbTelefono.Text);
               DB.AsignarParametroCadena("@division", "centrosolucion");
               DB.AsignarParametroCadena("@idSurcursal", idSucursal);
               //DB.AsignarParametroCadena("@logo", datos);
               DB.EjecutarConsulta1();
               DB.Desconectar();

               DB.Conectar();
               DB.CrearComandoProcedimiento("PA_modificar_LogoSuc");
               DB.AsignarParametroProcedimiento("@idSucursal", System.Data.DbType.String, idSucursal);
               DB.AsignarParametroProcedimiento("@logo", System.Data.DbType.Binary, datos);
               DB.EjecutarConsulta1();
               DB.Desconectar();
          /* }
           else {
               lMsj.Text = "La clave de sucursal ya existe.";
           }*/
        }
        private Boolean ValidarSucursal(string claveSucursal,string idSuc)
        {
            DB.Conectar();
            DB.CrearComando("select * from Cat_Sucursales where clave=@clave and idSucursal<>@idSuc and eliminado='False' ");
            DB.AsignarParametroCadena("@clave", claveSucursal);
            DB.AsignarParametroCadena("@idSuc", idSuc);
            DbDataReader DR = DB.EjecutarConsulta();

            while (DR.Read())
            {
                DB.Desconectar();
                return true;
            }
            DB.Desconectar();
            return false;
        }

        protected void bCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("sucursales.aspx");
        }

        protected void bGuardar_Click(object sender, EventArgs e)
        {
            idSucursal = Request.QueryString.Get("id");
            // if (!ValidarSucursal(tbClave.Text,idSucursal))
            //{
            byte[] datos = null;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg"))
                datos = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"imagenes/" + tbClave.Text + Session["IDEMI"].ToString() + ".jpg");
            DB.Conectar();
            DB.CrearComando(@"UPDATE Cat_Sucursales
               SET
                clave = @clave,sucursal = @sucursal,clavePtoEmision = @clavePtoEmision,
                domicilio = @domicilio,zona = @zona,tipo = @tipo,telefono = @telefono,
                division = @division
               WHERE idSucursal=@idSucursal");
            DB.AsignarParametroCadena("@clave", tbClave.Text);
            DB.AsignarParametroCadena("@sucursal", tbSucursal.Text);
            DB.AsignarParametroCadena("@clavePtoEmision", "001");
            DB.AsignarParametroCadena("@domicilio", tbDireccion.Text);
            DB.AsignarParametroCadena("@zona", ddlZona.SelectedValue);
            DB.AsignarParametroCadena("@tipo", ddlTipo.SelectedValue);
            DB.AsignarParametroCadena("@telefono", tbTelefono.Text);
            DB.AsignarParametroCadena("@division", "centrosolucion");
            DB.AsignarParametroCadena("@idSucursal", idSucursal);
            //DB.AsignarParametroCadena("@logo", datos);
            DB.EjecutarConsulta1();
            DB.Desconectar();

            DB.Conectar();
            DB.CrearComandoProcedimiento("PA_modificar_LogoSuc");
            DB.AsignarParametroProcedimiento("@idSucursal", System.Data.DbType.String, idSucursal);
            DB.AsignarParametroProcedimiento("@logo", System.Data.DbType.Binary, datos);
            DB.EjecutarConsulta1();
            DB.Desconectar();

            lMsj.Text = "Cambios guardados correctamente.";
            /* }
             else {
                 lMsj.Text = "La clave de sucursal ya existe.";
             }*/

            Response.Redirect("sucursales.aspx");
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