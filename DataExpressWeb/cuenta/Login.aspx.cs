// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="Login.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Web.UI;
using Datos;
using Control;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;
using Config;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Threading;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Login.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Login : Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;
        /// <summary>
        /// The _DB Recepcion
        /// </summary>
        private static BasesDatos _dbRecepcion;
        /// <summary>
        /// The _mail
        /// </summary>
        private readonly SendMail _mail = new SendMail();
        /// <summary>
        /// The _id usuario
        /// </summary>
        private string _idUsuario, _nombreEmpleado, _rfcCliente, _sucursal;
        /// <summary>
        /// The _MSJ
        /// </summary>
        private string _msj;
        /// <summary>
        /// The _ideemi
        /// </summary>
        private string _ideemi, _idgiro;
        /// <summary>
        /// The _rol
        /// </summary>
        private int _rol;
        /// <summary>
        /// The _asistente simplificado
        /// </summary>
        private bool _asistenteSimplificado;
        //private static string rfc;
        //K
        /// <summary>
        ///  The catalogos
        /// </summary>
        private CatCdfi catalogos = null;

        /// <summary>
        /// Handles the Click event of the bAgregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">Fallo al inserción en Base de Datos: {" + _sql + "}</exception>
        protected void bAgregar_Click(object sender, EventArgs e)
        {
            _db = new BasesDatos(ddlEmpresa.SelectedValue);

            _dbRecepcion = new BasesDatos(ddlEmpresa.SelectedValue, "Recepcion");
            DbDataReader dr;
            var pass1 = hfPass1.Value;
            var pass2 = hfPass2.Value;
            var ruc = "";
            var nomcli = "";
            var tipoCuenta = ddlTipoCuenta.SelectedValue;

            var _Registrado = "";
            if (tipoCuenta.Equals("2"))
            {
                _db.Conectar();
                _db.CrearComando(@"SELECT DISTINCT idProveedor FROM Cat_Proveedores where userProveedor=@userProveedor AND eliminado = '0'");
                _db.AsignarParametroCadena("@userProveedor", tbUsername.Text);
                var dr3 = _db.EjecutarConsulta();

                if (dr3.Read())
                {
                    _Registrado = dr3[0].ToString();
                }
                _db.Desconectar();
            }

            if (string.IsNullOrEmpty(_Registrado))
            {
                var idRecep = idReceptor(tbRFCNuevo.Text, !tipoCuenta.Equals("1"));
                if (string.IsNullOrEmpty(idRecep))
                {
                    try
                    {
                        if (tipoCuenta.Equals("1"))
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "El RFC no está registrado. Por favor revisar su información", 4, null);
                            return;
                        }
                        else
                        {
                            var sql = @"INSERT INTO Cat_Emisor
                            (RFCEMI, NOMEMI, dirMatriz, noExterior, noInterior, colonia, localidad, referencia, municipio, estado, pais, codigoPostal, empresaTipo)
                            OUTPUT Inserted.RFCEMI
                            VALUES (@RFCEMI,@NOMEMI,@CALLE,@NOEXT,@NOINT,@COL,@LOC,@REF,@MUN,@EST,@PAIS,@CP,@empresaTipo)";
                            _dbRecepcion.Conectar();
                            _dbRecepcion.CrearComando(sql);
                            _dbRecepcion.AsignarParametroCadena("@RFCEMI", tbRFCNuevo.Text);
                            _dbRecepcion.AsignarParametroCadena("@NOMEMI", tbNombre.Text);
                            _dbRecepcion.AsignarParametroCadena("@CALLE", tbCalleRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@NOEXT", tbNoExtRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@NOINT", tbNoIntRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@COL", tbColoniaRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@LOC", tbLocRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@REF", tbRefRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@MUN", tbMunicipioRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@EST", tbEstadoRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@PAIS", tbPaisRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@CP", tbCpRec.Text);
                            _dbRecepcion.AsignarParametroCadena("@empresaTipo", "0");
                            dr = _dbRecepcion.EjecutarConsulta();
                            if (dr.Read())
                            {
                                idRecep = dr[0].ToString();
                            }
                            _dbRecepcion.Desconectar();
                        }
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Ocurrió un error. Inténte el registro nuevamente.", 4, null, null, "resetPass();");
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(idRecep))
                {
                    ruc = tbRFCNuevo.Text;
                    nomcli = tbNombre.Text;
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Ocurrió un error. Inténte el registro nuevamente.", 4, null, null, "resetPass();");
                    return;
                }
                if (tbUsername.Text.Length == 11)
                {
                    var exist = false;
                    _db.Conectar();
                    _db.CrearComando(@"SELECT " + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + " FROM " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + " where " + (tipoCuenta.Equals("1") ? "userCliente" : "userProveedor") + "=@userCliente");
                    _db.AsignarParametroCadena("@userCliente", tbUsername.Text);
                    var dr2 = _db.EjecutarConsulta();
                    exist = dr2.Read();
                    _db.Desconectar();
                    if (!string.IsNullOrEmpty(ruc))
                    {
                        if (string.IsNullOrEmpty(tbNombre.Text))
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "El campo Nombre esta vacio.", 4, null, null, "resetPass(false);");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(pass1) || string.IsNullOrEmpty(pass2))
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Campos de contraseña incompletos.", 4, null, null, "resetPass(false);");
                            }
                            else
                            {
                                if (!pass1.Equals(pass2))
                                {
                                    (Master as SiteMaster).MostrarAlerta(this, "Los campos contraseña no coinciden.", 4, null, null, "resetPass(false);");
                                }
                                else
                                {
                                    var _sql = "";
                                    var idClienteNuevo = "";
                                    var ActivationToken = "";
                                    var read = false;
                                    try
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"INSERT " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + " (id_Receptor, " + (tipoCuenta.Equals("1") ? "userCliente" : "userProveedor") + ", " + (tipoCuenta.Equals("1") ? "claveCliente" : "claveProveedor") + ", " + (tipoCuenta.Equals("1") ? "nombreCliente" : "nombreProveedor") + ", email, " + (tipoCuenta.Equals("1") ? "telCliente" : "telProveedor") + ", status, eliminado) OUTPUT inserted." + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + ", inserted.ActivationToken VALUES (@id_Receptor, @userCliente, @claveCliente, @nombreCliente, @email, @telCliente, 0, 0)");
                                        _db.AsignarParametroCadena("@id_Receptor", idRecep);
                                        _db.AsignarParametroCadena("@userCliente", tbUsername.Text);
                                        _db.AsignarParametroCadena("@claveCliente", pass1);
                                        _db.AsignarParametroCadena("@nombreCliente", tbNombre.Text);
                                        _db.AsignarParametroCadena("@email", tbEmail.Text);
                                        _db.AsignarParametroCadena("@telCliente", tbTel.Text);
                                        _sql = _db.Comando.CommandText;
                                        dr = _db.EjecutarConsulta();
                                        read = dr.Read();
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Fallo al inserción en Base de Datos: {" + _sql + "}", ex);
                                    }
                                    if (read)
                                    {
                                        idClienteNuevo = dr[0].ToString();
                                        ActivationToken = dr[1].ToString();
                                        _db.Desconectar();
                                        string servidor = "", emailCredencial = "", passCredencial = "", emailEnviar = "";
                                        var ssl = true;
                                        var puerto = 0;
                                        _db.Conectar();
                                        _db.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,emailEnvio from Par_ParametrosSistema");

                                        var dr1 = _db.EjecutarConsulta();

                                        if (dr1.Read())
                                        {
                                            servidor = dr1[0].ToString();
                                            puerto = Convert.ToInt32(dr1[1]);
                                            ssl = Convert.ToBoolean(dr1[2]);
                                            emailCredencial = dr1[3].ToString();
                                            passCredencial = dr1[4].ToString();
                                            emailEnviar = dr1[5].ToString();
                                        }
                                        _db.Desconectar();
                                        _mail.ServidorSmtp(servidor, puerto, ssl, emailCredencial, passCredencial);

                                        //var linkActivacion = Absolute("~/cuenta/ActivarCuenta.aspx?usuario=" + tbUsername.Text + "&token=" + ActivationToken + "&rfcEmisor=" + ddlEmpresa.SelectedValue + "&rfc=" + ruc);
                                        var linkActivacion = Absolute("~/cuenta/ActivarCuenta.aspx?usuario=" + tbUsername.Text + "&token=" + ActivationToken + "&rfcEmisor=" + ControlUtilities.EncodeStringToBase64(ddlEmpresa.SelectedValue) + "&rfc=" + ControlUtilities.EncodeStringToBase64(ruc));
                                        var mensaje = "";
                                        _db.Conectar();
                                        _db.CrearComando(@"SELECT nombre,mensaje FROM Cat_Mensajes where nombre='MensajePortalWebVerificacion' ");
                                        var drSum = _db.EjecutarConsulta();
                                        if (drSum.Read())
                                        {
                                            mensaje = drSum[1].ToString();
                                        }
                                        _db.Desconectar();

                                        #region Copia de Alta

                                        var bcc = "";
                                        _db.Conectar();
                                        _db.CrearComando(@"SELECT emailAltaUsers FROM Par_ParametrosSistema");
                                        drSum = _db.EjecutarConsulta();
                                        if (drSum.Read())
                                        {
                                            bcc = drSum[0].ToString();
                                        }
                                        _db.Desconectar();

                                        #endregion

                                        _mail.LlenarEmail(emailEnviar, tbEmail.Text, bcc, "", "Activación de cuenta", mensaje);

                                        try
                                        {
                                            _mail.ReemplazarVariable("@NombreEmpleado", tbNombre.Text);
                                            _mail.ReemplazarVariable("@Username", tbUsername.Text);
                                            _mail.ReemplazarVariable("@LinkActivacion", "<a href=\"" + linkActivacion + "\">" + linkActivacion + "</a>");
                                            _mail.ReemplazarVariable("@RfcReceptor", ruc);
                                            _mail.EnviarEmail();

                                            Session["mensaje"] = "Le hemos enviado un correo de verifcación de su E-Mail.<br/>Una vez verificado el E-Mail, podrá iniciar sesión en el portal de consulta de documentos.";
                                            Response.Redirect("~/cuenta/Login.aspx", false);
                                        }
                                        catch (Exception ex)
                                        {
                                            _db.Conectar();
                                            _db.CrearComando("DELETE FROM " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + " WHERE " + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + " = @ID");
                                            _db.AsignarParametroCadena("@ID", idClienteNuevo);
                                            _db.EjecutarConsulta1();
                                            _db.Desconectar();
                                            (Master as SiteMaster).MostrarAlerta(this, "Ocurrió un error. Inténte el registro nuevamente.<br/><br/>" + ex.Message, 4, null, null, "resetPass();");
                                        }
                                    }
                                    else
                                    {
                                        _db.Desconectar();
                                        (Master as SiteMaster).MostrarAlerta(this, "Ocurrió un error. Inténte el registro nuevamente.", 4, null, null, "resetPass();");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El RFC no ha sido registrado.", 4, null, null, "resetPass();");
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El nombre de usuario es muy corto, la longitud minima es de 8 caracteres.", 4, null, null, "resetPass(false);");
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El proveedor con nombre de usuario " + tbUsername.Text + " ya se encuentra registrado.", 2, null, "$('#divNuevo').modal('hide');" + SetPwds("", false));
            }
        }

        /// <summary>
        /// Handles the Click event of the bNuevo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bNuevo_Click(object sender, EventArgs e)
        {
            var listaTextbox = UpdatePanel2.FindDescendants<System.Web.UI.WebControls.TextBox>().ToList();
            if (listaTextbox != null && listaTextbox.Count > 0)
            {
                listaTextbox.ForEach(txt => txt.Text = "");
            }
            rowDireccionFiscal.Style["display"] = "none";
            ddlTipoCuenta.SelectedValue = "1";
            ddlTipoCuenta_SelectedIndexChanged(null, null);
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#FiltrosC1').modal('toggle'); resetPass();", true);
        }

        /// <summary>
        /// Sets the PWDS.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="exec">if set to <c>true</c> [execute].</param>
        /// <returns>System.String.</returns>
        private string SetPwds(string val, bool exec = true)
        {
            tbContrasena.Text = val;
            tbRepetir.Text = val;
            var js = "setPwdVal(1, \"" + val + "\"); setPwdVal(2, \"" + val + "\");";
            if (exec)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_setPwds", js, true);
            }
            return js;
        }

        /// <summary>
        /// User1s this instance.
        /// </summary>
        private void User1()
        {
            _db = new BasesDatos(ddlEmpresa.SelectedValue);
            _dbRecepcion = new BasesDatos(ddlEmpresa.SelectedValue, "Recepcion");
            var tipoCuenta = ddlTipoCuenta.SelectedValue;
            var maxemp = "";
            var aux = 0;
            _db.Conectar();
            _db.CrearComando("select SUBSTRING(" + (tipoCuenta.Equals("1") ? "userCliente" : "userProveedor") + ",LEN(" + (tipoCuenta.Equals("1") ? "userCliente" : "userProveedor") + ")-3,4) from " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + " WHERE " + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + "= (SELECT MAX(" + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + ") FROM " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + ")");
            var dr1 = _db.EjecutarConsulta();
            if (dr1.Read())
            {
                if (int.TryParse(dr1[0].ToString(), out aux))
                {
                    aux = Convert.ToInt32(dr1[0]) + 1;
                }
            }
            _db.Desconectar();
            if (aux.ToString().Length == 1)
            {
                maxemp = "000" + aux;
            }
            if (aux.ToString().Length == 2)
            {
                maxemp = "00" + aux;
            }
            if (aux.ToString().Length == 3)
            {
                maxemp = "0" + aux;
            }
            if (aux.ToString().Length == 4)
            {
                maxemp = aux.ToString();
            }
            tbUsername.Text = (tipoCuenta.Equals("1") ? "CLIEN" : "PROVE") + Localization.Now.ToString("yy") + maxemp;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTipoCuenta control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTipoCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            User1();
            //tbNombre.Text = "";
            //tbRFCNuevo.Text = "";
            //tbNombre.ReadOnly = false;
            //rowDireccionFiscal.Style["display"] = "none";
            ColocarDatosRfcNuevo();
        }

        /// <summary>
        /// Handles the Click event of the bCodigo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bCodigo_Click(object sender, EventArgs e)
        {
            Session["IDENTEMIEXT"] = ddlEmpresa.SelectedValue;
            Response.Redirect("~/consultarCodigo.aspx");
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //K
            if (Session["CatalogosCfdi33"]==null)
            {
                ThreadStart pre = new ThreadStart(CrearCatalogo);
                Thread c = new Thread(pre);
                c.IsBackground = true;
                c.Start();
            }
            var auth = Request.QueryString.Get("auth");
            var redirect = Request.QueryString.Get("redirect");
            if (!string.IsNullOrEmpty(auth))
            {
                try
                {
                    // Esta accediendo desde URL remota
                    var aes = new Security();
                    auth = aes.Decryption(auth.Trim(), "1p50f4c7u");
                    var authSplit = auth.Split(new string[] { "&&&" }, 3, StringSplitOptions.None);
                    var userGlobal = authSplit[0];
                    var pwdGlobal = authSplit[1];
                    var urlGlobal = authSplit[2];
                    if (!string.IsNullOrEmpty(urlGlobal)) { Session["ExternalAuthUrl"] = urlGlobal; }
                    tbRfcuser.Text = userGlobal;
                    tbPass.Text = pwdGlobal;
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "No se pudo iniciar sesión remotamente. Inténtelo nuevamente<br/><br/>" + ex.Message, 4, null, "window.history.go(-1)");
                }
            }
            else
            {
                Session["ExternalAuthUrl"] = null;
                if ((Session["IDENTEMI"] != null && !string.IsNullOrEmpty(Session["IDENTEMI"].ToString())))
                {
                    if (string.IsNullOrEmpty(redirect))
                    {
                        Response.Redirect("~/Default.aspx", true);
                    }
                }
            }
            Session["IDENTEMIEXT"] = null;
            _db = new BasesDatos("CORE");
            SqlDataSource3.ConnectionString = _db.CadenaConexion;

            if (!IsPostBack)
            {
                ddlEmpresa.DataBind();
                _msj = "";
                _rol = 0;

                //rfc = ConfigurationManager.AppSettings.Get("RFC");
                //_db = new BasesDatos(rfc);
            }
            if (Session["Mensaje"] != null)
            {
                (Master as SiteMaster).MostrarAlerta(this, Session["Mensaje"].ToString(), 2, null);
                Session["mensaje"] = null;
            }
            divEmpresa.Style["display"] = (ddlEmpresa.Items.Count > 1) ? "inline" : "none";
            //if (ddlEmpresa.Items.FindByValue("LAN7008173R5") != null)
            //{
            //    ddlEmpresa.SelectedIndex = -1;
            //    ddlEmpresa.Items.FindByValue("LAN7008173R5").Selected = true;
            //    divEmpresa.Style["display"] = "none";
            //}
            if (ddlEmpresa.Items.FindByValue("GCP831026IGA") != null)
            {
                lblCodigo.Text = "Facturar o Buscar Ticket";
            }
            if (!string.IsNullOrEmpty(auth) && !string.IsNullOrEmpty(tbRfcuser.Text) && !string.IsNullOrEmpty(tbPass.Text))
            {
                if (ddlEmpresa.Items.Count <= 1)
                {
                    bSesion_Click(null, null);
                }
            }
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(redirect))
                {
                    switch (redirect)
                    {
                        case "ticket":
                            bCodigo_Click(null, null);
                            break;
                        default:
                            break;
                    }
                }
            }
            ddlEmpresa.Visible = false;
        }

        /// <summary>
        /// Colocars the datos RFC nuevo.
        /// </summary>
        private void ColocarDatosRfcNuevo()
        {
            var id = idReceptor(tbRFCNuevo.Text, true);
            var exists = !string.IsNullOrEmpty(id);
            rowDireccionFiscal.Style["display"] = exists ? "none" : "inline";
            tbNombre.Text = "";
            if (exists)
            {
                _dbRecepcion.Conectar();
                try
                {
                    _dbRecepcion.CrearComando("SELECT DISTINCT NOMEMI FROM Cat_Emisor WHERE RFCEMI = @id");
                    _dbRecepcion.AsignarParametroCadena("@id", id);
                    var dr = _dbRecepcion.EjecutarConsulta();
                    if (dr.Read())
                    {
                        tbNombre.Text = dr["NOMEMI"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        var _log = new Log(ddlEmpresa.SelectedValue);
                        _log.RegistrarTxt("Error al obtener nombre del Proveedor: " + ex.Message, "Login.aspx", "tbRFCNuevo_TextChanged", ex.ToString());
                    }
                    catch (Exception inner) { }
                    finally
                    {
                        //tbNombre.ReadOnly = false;
                    }
                }
                finally
                {
                    _dbRecepcion.Desconectar();
                    if (string.IsNullOrEmpty(tbNombre.Text))
                    {
                        //tbNombre.ReadOnly = false;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRFCNuevo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tbRFCNuevo_TextChanged(object sender, EventArgs e)
        {
            ColocarDatosRfcNuevo();
        }

        /// <summary>
        /// Carga catalogo CFDI33.
        /// </summary>
        public void CrearCatalogo()
        {
            catalogos = new CatCdfi();
            var bytes = Properties.Resources.catCFDI;
            catalogos.LoadFromBytes(bytes);
            Session["CatalogosCfdi33"] = catalogos;
        }

        /// <summary>
        /// Identifiers the receptor.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <param name="isRecepcion">if set to <c>true</c> [is recepcion].</param>
        /// <returns>System.String.</returns>
        private string idReceptor(string rfc, bool isRecepcion)
        {
            string id = null;
            try
            {
                if (isRecepcion)
                {
                    _dbRecepcion.Conectar();
                    _dbRecepcion.CrearComando("SELECT DISTINCT RFCEMI FROM Cat_Emisor WHERE RFCEMI = @rfc");
                    _dbRecepcion.AsignarParametroCadena("@rfc", rfc);
                    var dr = _dbRecepcion.EjecutarConsulta();
                    if (dr.Read())
                    {
                        id = dr[0].ToString();
                    }
                    _dbRecepcion.Desconectar();
                }
                else
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT IDEREC FROM Cat_Receptor WHERE RFCREC = @rfc");
                    _db.AsignarParametroCadena("@rfc", rfc);
                    var dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        id = dr[0].ToString();
                    }
                    _db.Desconectar();
                }
            }
            catch { id = null; }
            return id;
        }

        /// <summary>
        /// Absolutes the specified relative URL.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <returns>System.String.</returns>
        private string Absolute(string relativeUrl)
        {
            //return Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(relativeUrl);
            return relativeUrl.ToAbsoluteUrl();
        }

        /// <summary>
        /// Handles the Click event of the bEnviarOlvido control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bEnviarOlvido_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbRFCOlvido.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, "El RFC no puede estar vacío", 4, null);
                return;
            }
            else if (string.IsNullOrEmpty(tbUserOlvido.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, "El nombre de usuario no puede estar vacío", 4, null);
                return;
            }
            _db = new BasesDatos(ddlEmpresa.SelectedValue);
            _dbRecepcion = new BasesDatos(ddlEmpresa.SelectedValue, "Recepcion");
            //_db = new BasesDatos(rfc);
            string servidor = "", emailCredencial = "", passCredencial = "", emailEnviar = "";
            string passAct = "", corre = "", nombreCliente = "";
            var ssl = true;
            bool dat;
            var puerto = 0;
            _db.Conectar();
            _db.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,emailEnvio from Par_ParametrosSistema");

            var dr1 = _db.EjecutarConsulta();

            if (dr1.Read())
            {
                servidor = dr1[0].ToString();
                puerto = Convert.ToInt32(dr1[1]);
                ssl = Convert.ToBoolean(dr1[2]);
                emailCredencial = dr1[3].ToString();
                passCredencial = dr1[4].ToString();
                emailEnviar = dr1[5].ToString();
            }
            _db.Desconectar();
            var id = "";
            var sql = "";

            if (ddlTipoCuentaOlvido.SelectedValue.Equals("1"))
            {
                id = "1";
                sql = "SELECT Cat_Empleados.nombreEmpleado, Cat_Empleados.claveEmpleado, Cat_Empleados.email FROM Cat_Empleados INNER JOIN  Cat_Emisor ON Cat_Empleados.id_Emisor = Cat_Emisor.IDEEMI AND Cat_Emisor.RFCEMI = @rfc AND Cat_Empleados.userEmpleado = @user";
            }
            else if (ddlTipoCuentaOlvido.SelectedValue.Equals("2"))
            {
                id = idReceptor(tbRFCOlvido.Text, false);
                sql = "SELECT Cat_Clientes.nombreCliente, Cat_Clientes.claveCliente, Cat_Clientes.email FROM  Cat_Clientes INNER JOIN  Cat_RECEPTOR ON Cat_Clientes.id_Receptor = Cat_Receptor.IDEREC AND Cat_Receptor.RFCREC = @rfc AND Cat_Clientes.userCliente = @user";
            }
            else if (ddlTipoCuentaOlvido.SelectedValue.Equals("3"))
            {
                id = idReceptor(tbRFCOlvido.Text, true);
                sql = "SELECT Cat_Proveedores.nombreProveedor, Cat_Proveedores.claveProveedor, Cat_Proveedores.email FROM Cat_Proveedores WHERE Cat_Proveedores.id_Receptor = @rfc AND Cat_Proveedores.userProveedor = @user";
            }
            if (!string.IsNullOrEmpty(id))
            {
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@rfc", tbRFCOlvido.Text);
                _db.AsignarParametroCadena("@user", tbUserOlvido.Text);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    nombreCliente = dr[0].ToString();
                    passAct = dr[1].ToString();
                    corre = dr[2].ToString();
                    dat = true;
                }
                else
                {
                    dat = false;
                }
                _db.Desconectar();
                if (dat)
                {
                    _mail.ServidorSmtp(servidor, puerto, ssl, emailCredencial, passCredencial);

                    var mensaje = "";
                    _db.Conectar();
                    _db.CrearComando(@"SELECT nombre,mensaje FROM Cat_Mensajes where nombre='MensajePortalWebOlvido' ");
                    var drSum = _db.EjecutarConsulta();
                    if (drSum.Read())
                    {
                        mensaje = drSum[1].ToString();
                    }
                    _db.Desconectar();

                    _mail.LlenarEmail(emailEnviar, corre, "", "", "Recuperación de contraseña", mensaje);

                    try
                    {
                        _mail.ReemplazarVariable("@NombreEmpleado", nombreCliente);
                        _mail.ReemplazarVariable("@Username", tbUserOlvido.Text);
                        _mail.ReemplazarVariable("@Contrasena", passAct);
                        _mail.EnviarEmail();

                        Session["mensaje"] = "En breve recibira su contraseña en su correo";
                        Response.Redirect("~/cuenta/Login.aspx", false);
                    }
                    catch (Exception ex)
                    {
                        Session["mensaje"] = "Error al enviar el E-Mail " + ex.Message;
                        Response.Redirect("~/cuenta/Login.aspx", false);
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El usuario '" + tbUserOlvido.Text + "' no está ligado con el RFC '" + tbRFCOlvido.Text + "'. Por favor revisar su información", 4, null);
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El RFC no está registrado. Por favor revisar su información", 4, null);
            }
        }

        /// <summary>
        /// Handles the Click event of the bSesion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bSesion_Click(object sender, EventArgs e)
        {
            var timeout = 100;
            var conexionessimultaneas = "";
            var identificacionemisor = "";
            _db = new BasesDatos(ddlEmpresa.SelectedValue);
            _dbRecepcion = new BasesDatos(ddlEmpresa.SelectedValue, "Recepcion");

            _db.Conectar();
            _db.CrearComandoProcedimiento("PA_Login");
            _db.AsignarParametroProcedimiento("@USER", DbType.String, tbRfcuser.Text);
            _db.AsignarParametroProcedimiento("@PASSWORD", DbType.String, tbPass.Text);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                _msj = dr[0].ToString();
                _rol = Convert.ToInt16(dr[1].ToString()); //ROL
                _idUsuario = dr[2].ToString(); //idUsuario
                _nombreEmpleado = dr[3].ToString(); //nombre
                _rfcCliente = dr[4].ToString(); //IDENTIFICACION RECEPTOR
                _ideemi = dr[5].ToString(); //IDEEMISOR
                                            //_idgiro = dr[10].ToString(); //GIRO EMPRESARIAL
                _idgiro = _db.GetEmpresaTipo(); //GIRO EMPRESARIAL
                conexionessimultaneas = dr[7].ToString(); //CONEXIONES SIMULTANEAS
                bool.TryParse(dr[10].ToString(), out _asistenteSimplificado);
                try
                {
                    timeout = Convert.ToInt16(dr[8].ToString()); //SESSION TIME
                }
                catch (Exception)
                {
                    timeout = 1440;
                }
                timeout = 1440;

                identificacionemisor = Convert.ToString(dr[9].ToString());
                _sucursal = dr[11].ToString();

                if (string.IsNullOrEmpty(_rfcCliente))
                {
                    _rfcCliente = "R---";
                }
            }
            _db.Desconectar();
            // Codigo de Errores.

            #region Giro Empresarial

            Session["IDEMI"] = _ideemi;
            Session["IDGIRO"] = _idgiro;

            #endregion

            #region Identificacion del Emisor

            Session["IDENTEMI"] = identificacionemisor;

            #endregion

            if (!_msj.Equals("00000"))
            {
                _db.Conectar();
                _db.CrearComandoProcedimiento("PA_Errores");
                _db.AsignarParametroProcedimiento("@CODIGO", DbType.String, _msj);
                var dre = _db.EjecutarConsulta();
                if (dre.Read())
                {
                    string js = null;
                    var tokenGlobal = Request.QueryString.Get("tokenGlobal");
                    if (!string.IsNullOrEmpty(tokenGlobal))
                    {
                        js = "history.go(-1);";
                    }
                    (Master as SiteMaster).MostrarAlerta(this, dre[0].ToString(), 4, null, js);
                    //lMensaje.Text = DRE[0].ToString();
                }
                _db.Desconectar();
            }
            else
            {
                Session.Timeout = timeout;
                Session["CONEXIONESSIMULTANEAS"] = conexionessimultaneas;
                Session["rfcUser"] = tbRfcuser.Text;
                Session["rolUser"] = _rol;
                Session["idUser"] = _idUsuario;
                Session["nombreEmpleado"] = _nombreEmpleado;
                Session["rfcCliente"] = _rfcCliente;
                if (!string.IsNullOrEmpty(_rfcCliente) && !_rfcCliente.Equals("R---"))
                {
                    Session["claveSucursalUser"] = "S--X";
                }
                else
                {
                    Session["claveSucursalUser"] = _sucursal;
                }
                Session["USERNAME"] = tbRfcuser.Text.ToUpper();
                Session["asistente"] = _asistenteSimplificado;
                if (_rol > 0)
                {
                    _db.Conectar();
                    _db.CrearComando("SELECT s.idSucursal, s.RFC FROM Cat_Empleados e INNER JOIN Cat_SucursalesEmisor s ON e.id_Sucursal = s.idSucursal WHERE e.idEmpleado = @idUser");
                    _db.AsignarParametroCadena("@idUser", _idUsuario);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        Session["idSucursal"] = dr["idSucursal"].ToString();
                        Session["rfcSucursal"] = dr["RFC"].ToString();
                    }
                    _db.Desconectar();
                    _db.Conectar();
                    _db.CrearComandoProcedimiento("PA_consulta_rol");
                    _db.AsignarParametroProcedimiento("@idRol", DbType.Int16, _rol);
                    var drr = _db.EjecutarConsulta();
                    if (drr.Read())
                    {
                        Session["descRol"] = drr[1].ToString();
                        Session["coFactPropias"] = drr[2].ToString();
                        Session["coFactTodas"] = drr[3].ToString();
                        Session["CRnewComp"] = drr[4].ToString();
                        Session["CNSComp"] = drr[5].ToString();
                        Session["null3"] = drr[6].ToString();
                        Session["null4"] = drr[7].ToString();
                        Session["EDcompr0N"] = drr[8].ToString();
                        Session["RepGeneral"] = drr[9].ToString();
                        Session["PClient"] = drr[10].ToString();
                        Session["PEmple"] = drr[11].ToString();
                        Session["PRol"] = drr[12].ToString();
                        Session["EditEmi"] = drr[13].ToString();
                        Session["EditEstab"] = drr[14].ToString();
                        Session["EditPtoEmi"] = drr[15].ToString();
                        Session["EditInfoGral"] = drr[16].ToString();
                        Session["EditSMTP"] = drr[17].ToString();
                        Session["EditMensajes"] = drr[18].ToString();
                        Session["EditUserOpera"] = drr[19].ToString();
                        Session["LimpLogs"] = drr[20].ToString();
                        Session["EditPerfil"] = drr[21].ToString();
                        Session["EnvioEmail"] = drr[22].ToString();
                        Session["Eliminado"] = drr[23].ToString();
                        Session["TOPComp"] = drr[24].ToString();
                        Session["Recepcion"] = drr[25].ToString();
                        Session["arch"] = drr[27].ToString();
                        Session["imp"] = drr[28].ToString();
                        Session["EditRecep"] = drr[29].ToString();
                        Session["CancComp"] = drr[30].ToString();
                        Session["VerCanc"] = drr[31].ToString();
                        Session["DXC"] = drr[32].ToString();
                        Session["conc"] = drr[33].ToString();
                        Session["validacionRecepcion"] = drr[34].ToString();
                        _db.Desconectar();

                        _db.Conectar();
                        _db.CrearComando(@"INSERT INTO Log_Conexion
                                        (idUser,idSession,fechaInicio,fechaFin,estado)
                                            VALUES
                                        (@idUser,@idSession,@fechaInicio,@fechaFin,@estado) SELECT @@IDENTITY AS 'Identity';");
                        _db.AsignarParametroCadena("@idUser", _idUsuario);
                        _db.AsignarParametroCadena("@idSession", Session.SessionID);
                        _db.AsignarParametroCadena("@fechaInicio", Localization.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                        _db.AsignarParametroCadena("@fechaFin", Localization.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                        _db.AsignarParametroCadena("@estado", "1");
                        var dr1 = _db.EjecutarConsulta();
                        if (dr1.Read())
                        {
                            Session["idCONEX"] = dr1[0].ToString();
                        }
                    }
                    _db.Desconectar();
                }
                else
                {
                    //CLIENTES (ROL = 0)
                    if (tbRfcuser.Text.ToUpper().StartsWith("CLIEN"))
                    {
                        Session["IDENTEMI"] = ddlEmpresa.SelectedValue; //_rfc;
                        Session["IsCliente"] = true;
                        Session["coFactPropias"] = true;
                        Session["coFactTodas"] = true;
                        Session["CRnewComp"] = "";
                        if (_idgiro.Equals("1"))
                        {
                            Session["CNSComp"] = "01,04";
                        }
                        else if (_idgiro.Equals("2"))
                        {
                            Session["CNSComp"] = "01,04";
                        }
                        else if (_idgiro.Equals("3"))
                        {
                            Session["CNSComp"] = "01,04,06,07,08,09";
                        }
                        else if (_idgiro.Equals("4"))
                        {
                            Session["CNSComp"] = "01,04,06,07,08,09";
                        }
                        //Session["coFactPropiasPtoEmi"] = true;
                        Session["null3"] = false;
                        Session["null4"] = false;
                        Session["EDcompr0N"] = false;
                        Session["RepGeneral"] = false;
                        Session["PClient"] = false;
                        Session["PEmple"] = false;
                        Session["PRol"] = false;
                        Session["EditEmi"] = false;
                        Session["EditEstab"] = false;
                        Session["EditPtoEmi"] = false;
                        Session["EditInfoGral"] = false;
                        Session["EditSMTP"] = false;
                        Session["EditMensajes"] = false;
                        Session["EditUserOpera"] = false;
                        Session["LimpLogs"] = false;
                        Session["EditPerfil"] = true;
                        Session["EnvioEmail"] = false;
                        Session["Eliminado"] = false;
                        Session["TOPComp"] = "";
                        Session["Recepcion"] = false;
                        Session["arch"] = false;
                        Session["imp"] = false;
                        Session["EditRecep"] = false;
                        Session["CancComp"] = false;
                        Session["VerCanc"] = true;
                        Session["DXC"] = false;
                        Session["conc"] = false;
                        Session["validacionRecepcion"] = false;
                    }
                    else if (tbRfcuser.Text.ToUpper().StartsWith("PROVE"))
                    {
                        Session["IDENTEMI"] = ddlEmpresa.SelectedValue; //_rfc;
                        Session["IsProveedor"] = true;
                        Session["coFactPropias"] = true;
                        Session["coFactTodas"] = false;
                        Session["CRnewComp"] = "07";
                        if (_idgiro.Equals("1"))
                        {
                            Session["CNSComp"] = "01,04";
                        }
                        else if (_idgiro.Equals("2"))
                        {
                            Session["CNSComp"] = "01,04";
                        }
                        else if (_idgiro.Equals("3"))
                        {
                            Session["CNSComp"] = "01,04,06,07,08,09";
                        }
                        else if (_idgiro.Equals("4"))
                        {
                            Session["CNSComp"] = "01,04,06,07,08,09";
                        }
                        //Session["coFactPropiasPtoEmi"] = true;
                        Session["null3"] = false;
                        Session["null4"] = false;
                        Session["EDcompr0N"] = false;
                        Session["RepGeneral"] = false;
                        Session["PClient"] = false;
                        Session["PEmple"] = false;
                        Session["PRol"] = false;
                        Session["EditEmi"] = false;
                        Session["EditEstab"] = false;
                        Session["EditPtoEmi"] = false;
                        Session["EditInfoGral"] = false;
                        Session["EditSMTP"] = false;
                        Session["EditMensajes"] = false;
                        Session["EditUserOpera"] = false;
                        Session["LimpLogs"] = false;
                        Session["EditPerfil"] = true;
                        Session["EnvioEmail"] = true;
                        Session["Eliminado"] = false;
                        Session["TOPComp"] = "500";
                        Session["Recepcion"] = true;
                        Session["arch"] = false;
                        Session["imp"] = false;
                        Session["EditRecep"] = false;
                        Session["CancComp"] = true;
                        Session["VerCanc"] = true;
                        Session["DXC"] = false;
                        Session["conc"] = false;
                        Session["validacionRecepcion"] = false;
                    }
                }
                Session["colorFondo"] = "90,175,212";
                _db.Conectar();
                try
                {
                    _db.CrearComando("SELECT (CASE ISNULL(colorFondo, '') WHEN '' THEN '90,175,212' ELSE colorFondo END) AS colorFondo FROM Par_ParametrosSistema");
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        Session["colorFondo"] = dr["colorFondo"].ToString();
                    }
                }
                catch { }
                finally { _db.Desconectar(); }
                var parametros = new Parametros(Session["IDENTEMI"].ToString());
                try
                {
                    Session["CfdiVersion"] = parametros.GetParametroByNombre("VersionCfdi").Status;
                }
                catch (Exception ex) { Session["CfdiVersion"] = "3.2"; }
                try
                {
                    var conectores = parametros.GetParametroByNombre("TiposConecores").Status;
                    var arrayConcetores = conectores.Split('|');
                    Session["Conectores"] = arrayConcetores;
                }
                catch (Exception ex) { }
                /*if (Session["CfdiVersion"].ToString().Equals("3.3") && Session["CatalogosCfdi33"] == null)
                {
                    //CatCdfi catalogos = null;
                    try
                    {
                        //CATALOGOS

                        //catalogos = new CatCdfi();
                        var bytes = Properties.Resources.catCFDI;
                        catalogos.LoadFromBytes(bytes);
                        ThreadStart pre = new ThreadStart(CrearCatalogo);
                        Thread c = new Thread(pre);
                        c.IsBackground = true;
                        c.Start();
                    }
                    catch (Exception ex)
                    {
                        // ignored
                        catalogos = null;
                    }
                    finally
                    {
                        Session["CatalogosCfdi33"] = catalogos;
                    }
                }*/
                Response.Redirect("~/Default.aspx");
            }
        }
        /*void CrearCatalogo()
        {
            catalogos = new CatCdfi();
            var bytes = Properties.Resources.catCFDI;
            catalogos.LoadFromBytes(bytes);
            Session["CatalogosCfdi33"] = catalogos;
        }*/
    }

    /// <summary>
    /// Class Extensiones.
    /// </summary>
    static class Extensiones
    {
        /// <summary>
        /// To the absolute URL.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <returns>System.String.</returns>
        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : string.Empty;

            return string.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }
}