// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="ActivarCuenta.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using Control;
using Datos;
using System;
using System.Configuration;

namespace DataExpressWeb.cuenta
{
    /// <summary>
    /// Class ActivarCuenta.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class ActivarCuenta : System.Web.UI.Page
    {

        /// <summary>
        /// The database
        /// </summary>
        private static BasesDatos db;
        /// <summary>
        /// The _mail
        /// </summary>
        private readonly SendMail _mail = new SendMail();
        /// <summary>
        /// The tipo cuenta
        /// </summary>
        private static string tipoCuenta = "";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Request.QueryString["usuario"];
                var token = Request.QueryString["token"];
                var rfcEmisor = Request.QueryString["rfcEmisor"];
                var rfc = Request.QueryString["rfc"];
                rfcEmisor = ControlUtilities.DecodeStringFromBase64(rfcEmisor);
                rfc = ControlUtilities.DecodeStringFromBase64(rfc);
                if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(rfcEmisor) && !string.IsNullOrEmpty(rfc))
                {
                    db = new BasesDatos(rfcEmisor);
                    if (usuario.ToUpper().StartsWith("CLIEN"))
                    {
                        tipoCuenta = "1";
                    }
                    else if (usuario.ToUpper().StartsWith("PROVE"))
                    {
                        tipoCuenta = "2";
                    }
                    var id = GetIdCliente(usuario, rfc);
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!CheckStatus(id))
                        {
                            db.Conectar();
                            db.CrearComando("UPDATE " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + " SET status = 1, ActivationToken=NULL WHERE " + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + " = @id");
                            db.AsignarParametroCadena("@id", id);
                            var dr = db.EjecutarConsulta();
                            var updated = dr.RecordsAffected > 0;
                            db.Desconectar();
                            if (updated && CheckStatus(id))
                            {
                                string servidor = "", emailCredencial = "", passCredencial = "", emailEnviar = "";
                                var ssl = true;
                                var puerto = 0;
                                db.Conectar();
                                db.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,emailEnvio from Par_ParametrosSistema");

                                var dr1 = db.EjecutarConsulta();

                                if (dr1.Read())
                                {
                                    servidor = dr1[0].ToString();
                                    puerto = Convert.ToInt32(dr1[1]);
                                    ssl = Convert.ToBoolean(dr1[2]);
                                    emailCredencial = dr1[3].ToString();
                                    passCredencial = dr1[4].ToString();
                                    emailEnviar = dr1[5].ToString();
                                }
                                db.Desconectar();

                                db.Conectar();
                                db.CrearComando("SELECT " + (tipoCuenta.Equals("1") ? "nombreCliente" : "nombreProveedor") + ", " + (tipoCuenta.Equals("1") ? "claveCliente" : "claveProveedor") + ", email FROM " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + " WHERE " + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + " = @ID");
                                db.AsignarParametroCadena("@ID", id);
                                dr1 = db.EjecutarConsulta();
                                dr1.Read();
                                var nombrecliente = dr1[0].ToString();
                                var pass = dr1[1].ToString();
                                var emailCliente = dr1[2].ToString();
                                db.Desconectar();

                                _mail.ServidorSmtp(servidor, puerto, ssl, emailCredencial, passCredencial);

                                //var mensaje = "Estimado cliente: " + nombrecliente + "<br>";
                                //mensaje += "<br>Su cuenta ha sido activada correctamente, estos son los siguientes:<br>";
                                //mensaje += "<br>Contraseña: " + pass + "<br>";
                                //mensaje += "<br>Asignada al Usuario: " + usuario + "<br>";
                                //mensaje += "<br>Cualquier consulta por favor contáctenos ";
                                //mensaje += "<br><br>Gracias por preferirnos. ";
                                //mensaje += "<br><br> Atentamente ““ <br><br>";

                                var mensaje = "";
                                db.Conectar();
                                db.CrearComando(@"SELECT nombre,mensaje FROM Cat_Mensajes where nombre='MensajePortalWebActivada' ");
                                var drSum = db.EjecutarConsulta();
                                if (drSum.Read())
                                {
                                    mensaje = drSum[1].ToString();
                                }
                                db.Desconectar();

                                #region Copia de Alta

                                var bcc = "";
                                db.Conectar();
                                db.CrearComando(@"SELECT emailAltaUsers FROM Par_ParametrosSistema");
                                drSum = db.EjecutarConsulta();
                                if (drSum.Read())
                                {
                                    bcc = drSum[0].ToString();
                                }
                                db.Desconectar();

                                #endregion

                                _mail.LlenarEmail(emailEnviar, emailCliente, bcc, "", "Activación de cuenta", mensaje);

                                try
                                {
                                    _mail.ReemplazarVariable("@NombreEmpleado", nombrecliente);
                                    _mail.ReemplazarVariable("@Contrasena", pass);
                                    _mail.ReemplazarVariable("@Username", usuario);
                                    _mail.EnviarEmail();

                                    Session["mensaje"] = "Su cuenta ha sido activada, se ha enviado un correo con los detalles de su cuenta para que pueda ingresar al sistema.";
                                    Response.Redirect("~/cuenta/Login.aspx", false);
                                }
                                catch (Exception)
                                {
                                    db.Conectar();
                                    db.CrearComando("UPDATE " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + " SET status = 0, ActivationToken=@token WHERE " + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + " = @id");
                                    db.AsignarParametroCadena("@id", id);
                                    db.AsignarParametroCadena("@token", token);
                                    db.EjecutarConsulta1();
                                    Session["mensaje"] = "No se pudo activar la cuenta, inténtelo de nuevo. Si el problema persiste, pongase en contacto con DataExpress.";
                                }
                            }
                            else
                            {
                                Session["mensaje"] = "No se pudo activar la cuenta, inténtelo de nuevo. Si el problema persiste, pongase en contacto con DataExpress.";
                            }

                        }
                        else
                        {
                            Session["mensaje"] = "La cuenta ya ha sido activada previamente.";
                        }
                    }
                    else
                    {
                        Session["mensaje"] = "No existe ningun usuario con los parametros especificados.";
                    }
                    Response.Redirect("~/cuenta/Login.aspx", true);
                }
                else
                {
                    Response.Redirect("~/Seguridad.aspx", true);
                }

            }
        }

        /// <summary>
        /// Checks the status.
        /// </summary>
        /// <param name="idCliente">The identifier cliente.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool CheckStatus(string idCliente)
        {
            var status = false;
            db.Conectar();
            db.CrearComando("SELECT ActivationToken FROM " + (tipoCuenta.Equals("1") ? "Cat_Clientes" : "Cat_Proveedores") + " WHERE " + (tipoCuenta.Equals("1") ? "idCliente" : "idProveedor") + "=@id");
            db.AsignarParametroCadena("@id", idCliente);
            var dr = db.EjecutarConsulta();
            if (dr.Read())
            {
                status = dr[0] is DBNull || string.IsNullOrEmpty(dr[0].ToString());
            }
            db.Desconectar();
            return status;
        }

        /// <summary>
        /// Gets the identifier cliente.
        /// </summary>
        /// <param name="userCliente">The user cliente.</param>
        /// <param name="rfc">The RFC.</param>
        /// <returns>System.String.</returns>
        private string GetIdCliente(string userCliente, string rfc)
        {
            string id = null;
            db.Conectar();
            if (tipoCuenta.Equals("1"))
            {
                db.CrearComando("SELECT c.idCliente FROM Cat_Clientes c INNER JOIN Cat_Receptor r ON r.IDEREC = c.id_Receptor WHERE userCliente=@user AND r.RFCREC=@rfc");
            }
            else
            {
                db.CrearComando("SELECT c.idProveedor FROM Cat_Proveedores c WHERE userProveedor=@user AND c.id_Receptor=@rfc");
            }
            db.AsignarParametroCadena("@user", userCliente);
            db.AsignarParametroCadena("@rfc", rfc);
            var dr = db.EjecutarConsulta();
            if (dr.Read())
            {
                id = dr[0].ToString();
            }
            db.Desconectar();
            return id;
        }
    }
}