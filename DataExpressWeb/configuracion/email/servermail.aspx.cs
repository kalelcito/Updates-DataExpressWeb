// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="servermail.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using Datos;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace DataExpressWeb.configuracion.email
{
    /// <summary>
    /// Class Servermail.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Servermail : Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] == null)
            {
                Response.Redirect("~/Cerrar.aspx", true);
                return;
            }
            if (!Page.IsPostBack)
            {
                _db = new BasesDatos(Session["IDENTEMI"].ToString());
                _db.Conectar();
                _db.CrearComando("SELECT DISTINCT TOP 1 servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,emailEnvio,emailNotificacion,emailOpera,emailRecepcion,emailFmensual,emailBcc,emailAltaUsers,emailRecepBcc FROM Par_ParametrosSistema");
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbServidor.Text = dr["servidorSMTP"].ToString();
                    tbPuerto.Text = dr["puertoSMTP"].ToString();
                    cbSSL.Checked = Convert.ToBoolean(dr["sslSMTP"]);
                    tbUsuario.Text = dr["userSMTP"].ToString();
                    tbPassword.Text = dr["passSMTP"].ToString();
                    tbEmailEnvio.Text = dr["emailEnvio"].ToString();
                    tbEmailNotificacion.Text = dr["emailNotificacion"].ToString();
                    tbEmailOpera.Text = dr["emailOpera"].ToString();
                    tbEmailRecepcion.Text = dr["emailRecepcion"].ToString();
                    tbEmailFolios.Text = dr["emailFmensual"].ToString();
                    tbBcc.Text = dr["emailBcc"].ToString();
                    tbEmailUsers.Text = dr["emailAltaUsers"].ToString();
                    tbEmailRecepBcc.Text = dr["emailRecepBcc"].ToString();
                }
                _db.Desconectar();
            }
        }

        /// <summary>
        /// Handles the Click event of the bModificar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bModificar_Click(object sender, EventArgs e)
        {
            tbServidor.ReadOnly = false;
            tbPuerto.ReadOnly = false;
            tbUsuario.ReadOnly = false;
            tbPassword.ReadOnly = false;
            tbEmailEnvio.ReadOnly = false;
            tbEmailNotificacion.ReadOnly = false;
            tbEmailOpera.ReadOnly = false;
            tbEmailRecepcion.ReadOnly = false;
            tbEmailFolios.ReadOnly = false;
            tbBcc.ReadOnly = false;
            tbEmailUsers.ReadOnly = false;
            cbSSL.Enabled = true;
            bModificar.Visible = false;
            bActualizar.Visible = true;
            tbEmailRecepBcc.ReadOnly = false;
            //Response.Redirect("Modificar.aspx");
        }

        /// <summary>
        /// Handles the Click event of the bActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bActualizar_Click(object sender, EventArgs e)
        {
            var regexMail = @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$";
            if (!string.IsNullOrEmpty(tbEmailNotificacion.Text))
            {
                var mails = tbEmailNotificacion.Text.Split(',');
                foreach (var mail in mails)
                {
                    if (!Regex.IsMatch(mail, regexMail))
                    {
                        (Master as SiteMaster).MostrarAlerta(this, mail + " no es un correo válido", 4);
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(tbEmailOpera.Text))
            {
                var mails = tbEmailOpera.Text.Split(',');
                foreach (var mail in mails)
                {
                    if (!Regex.IsMatch(mail, regexMail))
                    {
                        (Master as SiteMaster).MostrarAlerta(this, mail + " no es un correo válido", 4);
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(tbBcc.Text))
            {
                var mails = tbBcc.Text.Split(',');
                foreach (var mail in mails)
                {
                    if (!Regex.IsMatch(mail, regexMail))
                    {
                        (Master as SiteMaster).MostrarAlerta(this, mail + " no es un correo válido", 4);
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(tbEmailRecepcion.Text))
            {
                var mails = tbEmailRecepcion.Text.Split(',');
                foreach (var mail in mails)
                {
                    if (!Regex.IsMatch(mail, regexMail))
                    {
                        (Master as SiteMaster).MostrarAlerta(this, mail + " no es un correo válido", 4);
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(tbEmailFolios.Text))
            {
                var mails = tbEmailFolios.Text.Split(',');
                foreach (var mail in mails)
                {
                    if (!Regex.IsMatch(mail, regexMail))
                    {
                        (Master as SiteMaster).MostrarAlerta(this, mail + " no es un correo válido", 4);
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(tbEmailRecepBcc.Text))
            {
                var mails = tbEmailRecepBcc.Text.Split(',');
                foreach (var mail in mails)
                {
                    if (!Regex.IsMatch(mail, regexMail))
                    {
                        (Master as SiteMaster).MostrarAlerta(this, mail + " no es un correo válido", 4);
                        return;
                    }
                }
            }
            _db.Conectar();
            _db.CrearComando("UPDATE Par_ParametrosSistema SET servidorSMTP=@servidorSMTP, puertoSMTP=@puertoSMTP, sslSMTP=@sslSMTP, userSMTP=@userSMTP, passSMTP=@passSMTP, emailEnvio=@emailEnvio, emailNotificacion=@emailNotificacion, emailOpera=@emailOpera, emailRecepcion=@emailRecepcion, emailFmensual=@emailFmensual, emailBcc=@emailBcc, emailAltaUsers=@emailAltaUsers, emailRecepBcc=@emailRecepBcc");
            _db.AsignarParametroCadena("@servidorSMTP", tbServidor.Text);
            _db.AsignarParametroCadena("@puertoSMTP", tbPuerto.Text);
            _db.AsignarParametroCadena("@sslSMTP", cbSSL.Checked ? "1" : "0");
            _db.AsignarParametroCadena("@userSMTP", tbUsuario.Text);
            _db.AsignarParametroCadena("@passSMTP", tbPassword.Text);
            _db.AsignarParametroCadena("@emailEnvio", tbEmailEnvio.Text);
            _db.AsignarParametroCadena("@emailNotificacion", tbEmailNotificacion.Text);
            _db.AsignarParametroCadena("@emailOpera", tbEmailOpera.Text);
            _db.AsignarParametroCadena("@emailRecepcion", tbEmailRecepcion.Text);
            _db.AsignarParametroCadena("@emailFmensual", tbEmailFolios.Text);
            _db.AsignarParametroCadena("@emailBcc", tbBcc.Text);
            _db.AsignarParametroCadena("@emailAltaUsers", tbEmailUsers.Text);
            _db.AsignarParametroCadena("@emailRecepBcc", tbEmailRecepBcc.Text);
            _db.EjecutarConsulta();
            _db.Desconectar();
            tbServidor.ReadOnly = true;
            tbPuerto.ReadOnly = true;
            tbUsuario.ReadOnly = true;
            tbPassword.ReadOnly = true;
            tbEmailEnvio.ReadOnly = true;
            tbEmailNotificacion.ReadOnly = true;
            tbEmailOpera.ReadOnly = true;
            tbEmailRecepcion.ReadOnly = true;
            tbEmailFolios.ReadOnly = true;
            tbBcc.ReadOnly = true;
            tbEmailUsers.ReadOnly = true;
            tbEmailRecepBcc.ReadOnly = true;
            cbSSL.Enabled = false;
            bModificar.Visible = true;
            bActualizar.Visible = false;
        }
    }
}
