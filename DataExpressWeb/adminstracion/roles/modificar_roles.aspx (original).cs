using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
namespace Administracion
{
    public partial class modificar_roles : System.Web.UI.Page
    {
        string idRol;

        private BasesDatos DB = new BasesDatos("");
        //private Boolean bcrearCliente;

        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            if (Session["IDENTEMI"] != null)
            {
                DB = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            }
            //log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            if (!Page.IsPostBack)
            {
                if (Session["idUser"]!= null)
                {
                    idRol = Request.QueryString.Get("id");
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_consulta_rol");
                    DB.AsignarParametroProcedimiento("@idRol", System.Data.DbType.String, idRol);
                    DbDataReader DR = DB.EjecutarConsulta();
                    if (DR.Read())
                    {
                        tbRol.Text = DR[1].ToString();
                        cbConsultarPropia.Checked = Convert.ToBoolean(DR[2].ToString());
                        cbConsultarTodas.Checked = Convert.ToBoolean(DR[3].ToString());
                        tbComprobantes.Text = Convert.ToString(DR[4].ToString());
                        tbComprobantesCNS.Text = Convert.ToString(DR[5].ToString());
                        cbConsultarPropiaPtoEmi.Checked = Convert.ToBoolean(DR[6].ToString());
                        cbEditComp.Checked = Convert.ToBoolean(DR[9].ToString());
                        cbReportesGeneral.Checked = Convert.ToBoolean(DR[10].ToString());
                        cbClientes.Checked = Convert.ToBoolean(DR[11].ToString());
                        cbEmpleado.Checked = Convert.ToBoolean(DR[12].ToString());
                        cbRol.Checked = Convert.ToBoolean(DR[13].ToString());
                        cbEditEmi.Checked = Convert.ToBoolean(DR[14].ToString());
                        cbEditEstab.Checked = Convert.ToBoolean(DR[15].ToString());
                        cbEditPtoEmi.Checked = Convert.ToBoolean(DR[16].ToString());
                        cbEditInfoGeneral.Checked = Convert.ToBoolean(DR[17].ToString());
                        cbEditSmtp.Checked = Convert.ToBoolean(DR[18].ToString());
                        cbEditMensaje.Checked = Convert.ToBoolean(DR[19].ToString());
                        cbEditUserOpera.Checked = Convert.ToBoolean(DR[20].ToString());
                        cbLimpiarLogs.Checked = Convert.ToBoolean(DR[21].ToString());
                        cbEditPerfil.Checked = Convert.ToBoolean(DR[22].ToString());
                        cbEnvioEmail.Checked = Convert.ToBoolean(DR[23].ToString());
                        tbTOPComp.Text = Convert.ToString(DR[25].ToString());
                        cbRecepcion.Checked = Convert.ToBoolean(DR[26].ToString());
                        cbVisible.Checked = Convert.ToBoolean(DR[27].ToString());
                        //ddlFacturas.SelectedValue = DR[16].ToString();
                        //cbNC.Checked = Convert.ToBoolean(DR[17].ToString());
                    }
                    DB.Desconectar();
                }

            }
        }

       protected void bModificar_Click(object sender, EventArgs e)
        {
            idRol = Request.QueryString.Get("id");
            if (!consultaRol(tbRol.Text, idRol))
            {

                if (cbConsultarPropia.Checked || cbConsultarTodas.Checked || cbEditComp.Checked || cbReportesGeneral.Checked ||
                    cbClientes.Checked || cbEmpleado.Checked || cbRol.Checked ||
                    cbEditEmi.Checked || cbEditEstab.Checked ||
                    cbEditPtoEmi.Checked || cbEditInfoGeneral.Checked || cbEditSmtp.Checked)
                {
            DB.Conectar();
            DB.CrearComandoProcedimiento("PA_modificar_rol");
            DB.AsignarParametroProcedimiento("@idRol", System.Data.DbType.String, idRol);
            DB.AsignarParametroProcedimiento("@descripcion", System.Data.DbType.String, tbRol.Text);
            DB.AsignarParametroProcedimiento("@consultar_fac_propias", System.Data.DbType.Byte, Convert.ToByte(cbConsultarPropia.Checked));
            DB.AsignarParametroProcedimiento("@consultar_todas_fac", System.Data.DbType.Byte, Convert.ToByte(cbConsultarTodas.Checked));
            DB.AsignarParametroProcedimiento("@CrearNewComp", System.Data.DbType.String, Convert.ToString(tbComprobantes.Text));
            DB.AsignarParametroProcedimiento("@EditComp0N", System.Data.DbType.Byte, Convert.ToByte(cbEditComp.Checked));
            DB.AsignarParametroProcedimiento("@ReporteGeneral", System.Data.DbType.Byte, Convert.ToByte(cbReportesGeneral.Checked));
            DB.AsignarParametroProcedimiento("@Cliente", System.Data.DbType.Byte, Convert.ToByte(cbClientes.Checked));
            DB.AsignarParametroProcedimiento("@Empleado", System.Data.DbType.Byte, Convert.ToByte(cbEmpleado.Checked));
            DB.AsignarParametroProcedimiento("@Roles", System.Data.DbType.Byte, Convert.ToByte(cbRol.Checked));
            DB.AsignarParametroProcedimiento("@EditEmisor", System.Data.DbType.Byte, Convert.ToByte(cbEditEmi.Checked));
            DB.AsignarParametroProcedimiento("@EditEstab", System.Data.DbType.Byte, Convert.ToByte(cbEditEstab.Checked));
            DB.AsignarParametroProcedimiento("@EditPtoEmi", System.Data.DbType.Byte, Convert.ToByte(cbEditPtoEmi.Checked));
            DB.AsignarParametroProcedimiento("@EditInfoGeneral", System.Data.DbType.Byte, Convert.ToByte(cbEditInfoGeneral.Checked));
            DB.AsignarParametroProcedimiento("@EditSMTP", System.Data.DbType.Byte, Convert.ToByte(cbEditSmtp.Checked));
            DB.AsignarParametroProcedimiento("@EditMensajes", System.Data.DbType.Byte, Convert.ToByte(cbEditMensaje.Checked));
            DB.AsignarParametroProcedimiento("@EditUserOpera", System.Data.DbType.Byte, Convert.ToByte(cbEditUserOpera.Checked));
            DB.AsignarParametroProcedimiento("@LimpiarLogs", System.Data.DbType.Byte, Convert.ToByte(cbLimpiarLogs.Checked));
            DB.AsignarParametroProcedimiento("@EditPerfil", System.Data.DbType.Byte, Convert.ToByte(cbEditPerfil.Checked));
            DB.AsignarParametroProcedimiento("@EnvioEmail", System.Data.DbType.Byte, Convert.ToByte(cbEnvioEmail.Checked));
            DB.AsignarParametroProcedimiento("@eliminado", System.Data.DbType.Byte, false);
            DB.AsignarParametroProcedimiento("@TOPComp", System.Data.DbType.String, Convert.ToString(tbTOPComp.Text));
            DB.AsignarParametroProcedimiento("@recepcion", System.Data.DbType.Byte, Convert.ToByte(cbRecepcion.Checked));
            DB.AsignarParametroProcedimiento("@Visible", System.Data.DbType.Byte, Convert.ToByte(cbVisible.Checked));
            DB.AsignarParametroProcedimiento("@CNSComp", System.Data.DbType.String, Convert.ToString(tbComprobantesCNS.Text));
            DB.AsignarParametroProcedimiento("@consultar_fac_propias_ptoemi", System.Data.DbType.Byte, Convert.ToByte(cbConsultarPropiaPtoEmi.Checked));
            //DB.AsignarParametroProcedimiento("@nc", System.Data.DbType.Byte, Convert.ToByte(cbNC.Checked));
            DB.EjecutarConsulta1();
            DB.Desconectar();
            Response.Redirect("roles.aspx");
                }
                else
                {
                    lMsj.Text = "Seleccionar una opción";
                }
            }
            else
            {
                lMsj.Text = "El Rol ya Existe.";
            }
        }
       public Boolean consultaRol(string descripcion, string idRol)
       {
           DB.Conectar();
           DB.CrearComando("select descripcion from Cat_Roles where descripcion=@descripcion  and idRol<>@idRol");
           DB.AsignarParametroCadena("@descripcion", descripcion);
           DB.AsignarParametroCadena("@idRol", idRol);
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
           Response.Redirect("roles.aspx");
       }

       protected void btnCancelar_Click(object sender, EventArgs e)
       {
           Response.Redirect("roles.aspx");
       }


    }
}