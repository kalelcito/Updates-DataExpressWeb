// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="Global.asax.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net.Mail;
using System.Web;
using Control;
using Datos;


namespace DataExpressWeb
{
    /// <summary>
    /// Class Global.
    /// </summary>
    /// <seealso cref="System.Web.HttpApplication" />
    public class Global : HttpApplication
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The _em
        /// </summary>
        private SendMail _em;
        /// <summary>
        /// The _email credencial
        /// </summary>
        private string _emailCredencial = "";
        /// <summary>
        /// The _email enviar
        /// </summary>
        private string _emailEnviar = "";
        /// <summary>
        /// The _pass credencial
        /// </summary>
        private string _passCredencial = "";
        /// <summary>
        /// The _puerto
        /// </summary>
        private int _puerto = 25;
        /// <summary>
        /// The _ruta document
        /// </summary>
        private string _rutaDoc = "";
        /// <summary>
        /// The _servidor
        /// </summary>
        private string _servidor = "";
        /// <summary>
        /// The _SSL
        /// </summary>
        private bool _ssl;
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log;
        /// <summary>
        /// The _identemi
        /// </summary>
        private string _identemi = "";
        /// <summary>
        /// The _iduser
        /// </summary>
        private string _iduser = "";

        /// <summary>
        /// Handles the Start event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciarse la aplicación
        }

        /// <summary>
        /// Handles the End event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Application_End(object sender, EventArgs e)
        {
            //  Código que se ejecuta cuando se cierra la aplicación
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            var errorMessage = error.Message;
            var errorString = error.ToString();
            var errorStack = error.StackTrace;
            while (error.InnerException != null)
            {
                error = error.InnerException;
                errorMessage += "<br/>-> " + error.Message;
                errorString += "<br/>-> " + error.ToString();
                errorStack += "<br/>-> " + error.StackTrace;
            }
            var urlSource = Request.Url.LocalPath;
            var detallesError = "<p><strong>Error (String Format):</strong></p><blockquote>" + errorString + "</blockquote><p><strong>Error (Stack Trace):</strong></p><blockquote>" + errorStack + "</blockquote><br/>";
            try
            {
                var application = (HttpApplication)sender;
                var context = application.Context;
                var session = context.Session;
                if (session["IDENTEMI"] == null && session["IDENTEMIEXT"] == null)
                {
                    if (!Request.Path.Contains("/consultarCodigo.aspx") && !Request.Path.Contains("/download.aspx") && !Request.Path.Contains("/descargarPDF.aspx") && !Request.Path.Contains("/cuenta/Login.aspx") && !Request.Path.Contains("/previsualizarPDF.aspx"))
                    {
                        Response.Redirect("~/Cerrar.aspx", true);
                        return;
                    }
                }
                Session["errorWeb"] = error;
                var emails = "";
                _identemi = Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE";
                _iduser = Session["idUser"] != null ? Session["idUser"].ToString() : "";
                _db = new BasesDatos(_identemi);
                _log = new Log(_identemi);

                RegLog("Error en la Web", urlSource, "Mensaje:" + Environment.NewLine + errorMessage + Environment.NewLine + Environment.NewLine + "Error:" + Environment.NewLine + errorString + Environment.NewLine + Environment.NewLine + "StackTrace" + Environment.NewLine + errorStack);

                #region Enviar E-Mail

                _db.Conectar();
                _db.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio,emailNotificacion from Par_ParametrosSistema");
                var dr1 = _db.EjecutarConsulta();

                while (dr1.Read())
                {
                    _servidor = dr1[0].ToString();
                    _puerto = Convert.ToInt32(dr1[1].ToString());
                    _ssl = Convert.ToBoolean(dr1[2].ToString());
                    _emailCredencial = dr1[3].ToString();
                    _passCredencial = dr1[4].ToString();
                    _rutaDoc = dr1[5].ToString();
                    _emailEnviar = dr1[6].ToString();
                    emails = dr1[7].ToString();
                }
                _db.Desconectar();

                emails += ",sehernandez@dataexpressintmx.com"; // <------ Prueba

                var asunto = "";
                var mensaje = "";

                _em = new SendMail();

                _em.ServidorSmtp(_servidor, _puerto, _ssl, _emailCredencial, _passCredencial);

                if (!string.IsNullOrEmpty(emails))
                {
                    var emir = "";
                    _db.Conectar();
                    _db.CrearComando(@"SELECT nombre,mensaje FROM Cat_Mensajes where nombre='MensajePortalWebError' ");
                    var drSum = _db.EjecutarConsulta();
                    if (drSum.Read())
                    {
                        mensaje = drSum[1].ToString();
                    }
                    _db.Desconectar();

                    _db.Conectar();
                    _db.CrearComando(@"SELECT NOMEMI FROM Cat_Emisor where RFCEMI = @rfc");
                    _db.AsignarParametroCadena("@rfc", _identemi);
                    var dRemi = _db.EjecutarConsulta();
                    if (dRemi.Read())
                    {
                        emir = dRemi[0].ToString();
                    }
                    _db.Desconectar();

                    asunto = "Error en el Portal: " + emir + "";

                    try
                    {
                        _em.LlenarEmail(_emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                        _em.ReemplazarVariable("@FechaError", Localization.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                        _em.ReemplazarVariable("@MensajeError", errorMessage);
                        _em.ReemplazarVariable("@DetallesError", detallesError);
                        _em.ReemplazarVariable("@Emisor", emir);
                        _em.ReemplazarVariable("@URLError", urlSource);
                        _em.EnviarEmail();
                    }
                    catch (SmtpException)
                    {
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Cerrar.aspx", true);
                return;
            }
        }

        /// <summary>
        /// Regs the log.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <param name="metodoActual">The metodo actual.</param>
        /// <param name="mensajeTecnico">The mensaje tecnico.</param>
        private void RegLog(string mensaje, string metodoActual, string mensajeTecnico = "")
        {
            _log.Registrar(mensaje, GetType().Name, metodoActual, null, mensajeTecnico, null, null, _iduser, Session["IDENTEMI"].ToString(), true);
        }

        /// <summary>
        /// Handles the Start event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Session_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se inicia una nueva sesión
            //Session.Timeout = 1;
        }

        /// <summary>
        /// Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Session_End(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando finaliza una sesión.
            // Nota: el evento Session_End se desencadena sólo cuando el modo sessionstate
            // se establece como InProc en el archivo Web.config. Si el modo de sesión se establece como StateServer
            // o SQLServer, el evento no se genera.
        }
    }
}