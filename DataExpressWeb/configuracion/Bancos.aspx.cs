using Control;
using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataExpressWeb.configuracion
{
    public partial class Bancos : System.Web.UI.Page
    {
        private const string BANK_QUERY = "SELECT * FROM Cat_BancosComplemento";
        private static string _idEditar;
        private BasesDatos _db;
        private string _idUser = "";
        private Log _log;

        protected void Page_Load(object sender, EventArgs e)
        {
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            _log = new Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            if (Session["idUser"] != null)
            {
                _idUser = Session["idUser"].ToString();
                SqlDataSourceBancos.ConnectionString = _db.CadenaConexion;
            }
            else
            {
                Response.Redirect("~/Cerrar.aspx", false);
            }
            if (!IsPostBack)
            {

            }
        }

        protected void lbBusqueda_Click(object sender, EventArgs e)
        {
            var busqueda = tbBusqueda.Text;
            var sql = BANK_QUERY + $" WHERE NombreCuenta LIKE '%{busqueda}%' OR RfcBanco LIKE '%{busqueda}%' OR NombreBanco LIKE '%{busqueda}%' OR NumeroCuenta LIKE '%{busqueda}%' OR ClaveBanco LIKE '%{busqueda}%'";
            SqlDataSourceBancos.SelectCommand = sql;
            gvBancos.DataBind();
        }

        protected void lbAgregarNuevo_Click(object sender, EventArgs e)
        {
            tbClaveBanco.Text = "";
            tbNombreBanco.Text = "";
            tbNombreCuenta.Text = "";
            tbNumeroCuenta.Text = "";
            tbRfcBanco.Text = "";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('toggle');", true);
        }

        protected void bDetalles_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;
            Session["_idEditar"] = id;
            tbClaveBanco.Text = "";
            tbNombreBanco.Text = "";
            tbNombreCuenta.Text = "";
            tbNumeroCuenta.Text = "";
            tbRfcBanco.Text = "";
            try
            {
                _db.Conectar();
                _db.CrearComando(@"SELECT * FROM Cat_BancosComplemento WHERE Id = @Id");
                _db.AsignarParametroCadena("@Id", id);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbClaveBanco.Text = dr["ClaveBanco"].ToString();
                    tbNombreBanco.Text = dr["NombreBanco"].ToString();
                    tbNombreCuenta.Text = dr["NombreCuenta"].ToString();
                    tbNumeroCuenta.Text = dr["NumeroCuenta"].ToString();
                    tbRfcBanco.Text = dr["RfcBanco"].ToString();
                }
                _db.Desconectar();
                ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('toggle');", true);
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, $"El registro no se pudo visualizar, por favor inténtelo nuevamente.<br/><br/>{ex.Message}", 4);
            }
        }

        protected void lbLimpiar_Click(object sender, EventArgs e)
        {
            tbBusqueda.Text = "";
            SqlDataSourceBancos.SelectCommand = BANK_QUERY;
            gvBancos.DataBind();
        }

        protected void lbGuardar_Click(object sender, EventArgs e)
        {
            var idSession = Session["_idEditar"];
            try
            {
                if (idSession != null)
                {
                    // Editar
                    _db.Conectar();
                    _db.CrearComando(@"UPDATE Cat_BancosComplemento SET NombreCuenta = @NombreCuenta, RfcBanco = @RfcBanco, NombreBanco = @NombreBanco, NumeroCuenta = @NumeroCuenta, ClaveBanco = @ClaveBanco WHERE Id = @Id");
                    _db.AsignarParametroCadena("@NombreCuenta", tbNombreCuenta.Text.Trim());
                    _db.AsignarParametroCadena("@RfcBanco", tbRfcBanco.Text.Trim());
                    _db.AsignarParametroCadena("@NombreBanco", tbNombreBanco.Text.Trim());
                    _db.AsignarParametroCadena("@NumeroCuenta", tbNumeroCuenta.Text.Trim());
                    _db.AsignarParametroCadena("@ClaveBanco", tbClaveBanco.Text.Trim());
                    _db.AsignarParametroCadena("@Id", idSession.ToString());
                    _db.EjecutarConsulta1();
                    _db.Desconectar();
                }
                else
                {
                    // Nuevo
                    _db.Conectar();
                    _db.CrearComando(@"INSERT INTO [Cat_BancosComplemento]
           ([NombreCuenta]
           ,[RfcBanco]
           ,[NombreBanco]
           ,[NumeroCuenta]
           ,[ClaveBanco])
     VALUES
           (@NombreCuenta
           ,@RfcBanco
           ,@NombreBanco
           ,@NumeroCuenta
           ,@ClaveBanco)");
                    _db.AsignarParametroCadena("@NombreCuenta", tbNombreCuenta.Text.Trim());
                    _db.AsignarParametroCadena("@RfcBanco", tbRfcBanco.Text.Trim());
                    _db.AsignarParametroCadena("@NombreBanco", tbNombreBanco.Text.Trim());
                    _db.AsignarParametroCadena("@NumeroCuenta", tbNumeroCuenta.Text.Trim());
                    _db.AsignarParametroCadena("@ClaveBanco", tbClaveBanco.Text.Trim());
                    _db.EjecutarConsulta1();
                    _db.Desconectar();
                }
                (Master as SiteMaster).MostrarAlerta(this, $"El registro se {(idSession == null ? "agregó" : "actualizó")} correctamente", 2, null, "$('#divNuevo').modal('hide');");
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, $"El registro no se pudo {(idSession == null ? "agregar" : "actualizar")}, por favor inténtelo nuevamente.<br/><br/>{ex.Message}", 4);
            }
            Session["_idEditar"] = null;
            lbLimpiar_Click(null, null);
        }
    }
}