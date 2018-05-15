// ***********************************************************************
// Assembly         : Control
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="SendMail.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Control.WsEmail;
using MailBee.Mime;
using MailBee.SmtpMail;
using Attachment = System.Net.Mail.Attachment;
using MailMessage = System.Net.Mail.MailMessage;
using MailPriority = System.Net.Mail.MailPriority;

namespace Control
{
    /// <summary>
    /// Class SendMail.
    /// </summary>
    public class EnviarMail
    {
        /// <summary>
        /// The credenciales
        /// </summary>
        private KeyValuePair<string, string> _credenciales;

        /// <summary>
        /// The _m mail message
        /// </summary>
        private MailMessage _mMailMessage;

        /// <summary>
        /// The _m SMTP client
        /// </summary>
        private SmtpClient _mSmtpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendMail" /> class.
        /// </summary>
        public EnviarMail()
        {
            _mMailMessage = new MailMessage();
            _mSmtpClient = new SmtpClient();
        }

        /// <summary>
        /// Inicializa los valores del servidor SMTP configurado
        /// </summary>
        /// <param name="servidor">Dirección del servidor de mail saliente.</param>
        /// <param name="puerto">El puerto de SMTP.</param>
        /// <param name="ssl">Usar o no SSL para autenticación.</param>
        /// <param name="emailCredencial">Usuario para autenticación.</param>
        /// <param name="passCredencial">Contraseña para autenticación.</param>
        public void ServidorSmtp(string servidor, int puerto, bool ssl, string emailCredencial, string passCredencial)
        {
            _mSmtpClient.Host = servidor;
            _mSmtpClient.Port = puerto;
            _mSmtpClient.EnableSsl = ssl;
            _credenciales = new KeyValuePair<string, string>(emailCredencial, passCredencial);
            _mSmtpClient.Credentials = new NetworkCredential(emailCredencial, passCredencial);
        }

        /// <summary>
        /// Adjunta un archivo al mensaje.
        /// </summary>
        /// <param name="ruta">Ruta absoluta del archivo a adjuntar.</param>
        /// <returns><c>true</c> si el archivo se adjunto correctamente, <c>false</c> de lo contrario.</returns>
        public bool Adjuntar(string ruta)
        {
            var control = false;
            if (!File.Exists(ruta))
                return false;
            try
            {
                _mMailMessage.Attachments.Add(new Attachment(ruta));
                control = true;
            }
            catch (Exception)
            {
                // ignored
            }
            return control;
        }

        /// <summary>
        /// Adjunta un archivo en binario al mensaje.
        /// </summary>
        /// <param name="datos">Arreglo binario que contiene el archivo a adjuntar.</param>
        /// <param name="nombre">El nombre que tendrá el archivo adjunto.</param>
        /// <returns><c>true</c> si el archivo se adjunto correctamente, <c>false</c> de lo contrario.</returns>
        public bool Adjuntar(byte[] datos, string nombre)
        {
            var control = false;
            try
            {
                Stream stream = new MemoryStream(datos);
                _mMailMessage.Attachments.Add(new Attachment(stream, nombre));
                control = true;
            }
            catch (Exception)
            {
                // ignored
            }
            return control;
        }

        /// <summary>
        /// Inicializa los valores requeridos para el envío del mail.
        /// </summary>
        /// <param name="from">Dirección de correo emisora.</param>
        /// <param name="to">Direcciones de correo receptoras separadas por coma.</param>
        /// <param name="bcc">Direcciones de correo receptoras con copia oculta.</param>
        /// <param name="cc">Direcciones de correo receptoras con copia.</param>
        /// <param name="subject">Asunto del mensaje.</param>
        /// <param name="body">Cuerpo del mensaje (permite código HTML).</param>
        public void LlenarEmail(string from, string to, string bcc, string cc, string subject, string body)
        {
            _mMailMessage.From = new MailAddress(from);
            var destinatarios = to.Split(',');
            foreach (var email in destinatarios)
                _mMailMessage.To.Add(new MailAddress(email));
            if (!string.IsNullOrEmpty(bcc))
                _mMailMessage.Bcc.Add(new MailAddress(bcc));
            if (!string.IsNullOrEmpty(cc))
                _mMailMessage.CC.Add(new MailAddress(cc));

            _mMailMessage.Subject = subject;
            _mMailMessage.Body = body;
            _mMailMessage.IsBodyHtml = true;
            _mMailMessage.Priority = MailPriority.Normal;
        }

        /// <summary>
        /// Reemplazar variables del cuerpo del mail.
        /// </summary>
        /// <param name="variable">La variable a reemplazar.</param>
        /// <param name="valor">The valor a reemplazar.</param>
        public void ReemplazarVariable(string variable, string valor)
        {
            var body = _mMailMessage.Body;
            body = new Regex(Regex.Escape(variable)).Replace(body, valor, 1);
            _mMailMessage.Body = body;
        }

        /// <summary>
        /// Enviar mail mediante WebService remoto.
        /// </summary>
        private void WebService()
        {
            var attachments = new Attachment[_mMailMessage.Attachments.Count];
            _mMailMessage.Attachments.CopyTo(attachments, 0);
            var pdf = attachments.FirstOrDefault(x =>
            {
                var extension = Path.GetExtension(x.Name);
                return extension != null && extension.EndsWith("PDF", StringComparison.OrdinalIgnoreCase);
            });
            var xml = attachments.FirstOrDefault(x =>
            {
                var extension = Path.GetExtension(x.Name);
                return extension != null && extension.EndsWith("XML", StringComparison.OrdinalIgnoreCase);
            });
            byte[] pdfBytes;
            byte[] xmlBytes;
            using (var streamReader = new MemoryStream())
            {
                if (pdf != null)
                    pdf.ContentStream.CopyTo(streamReader);
                pdfBytes = streamReader.ToArray();
            }
            using (var streamReader = new MemoryStream())
            {
                if (xml != null)
                    xml.ContentStream.CopyTo(streamReader);
                xmlBytes = streamReader.ToArray();
            }
            var ws = new ServiceEmail();
            if (pdf != null && xml != null)
                ws.enviar(_mSmtpClient.Host, _mSmtpClient.Port, _mSmtpClient.EnableSsl, _credenciales.Key, _credenciales.Value, pdfBytes, pdf.Name, xmlBytes, xml.Name, _mMailMessage.From.Address, string.Join(",", _mMailMessage.To.Select(x => x.Address).ToList()), string.Join(",", _mMailMessage.Bcc.Select(x => x.Address).ToList()), string.Join(",", _mMailMessage.CC.Select(x => x.Address).ToList()), _mMailMessage.Subject, _mMailMessage.Body);
        }

        /// <summary>
        /// Enviar mail.
        /// </summary>
        public void EnviarEmail()
        {
            //WebService();
            _mSmtpClient.Send(_mMailMessage);
            _mMailMessage = new MailMessage();
            _mSmtpClient = new SmtpClient();
        }
    }

    /// <summary>
    /// Class SendMail.
    /// </summary>
    public class SendMail
    {
        /// <summary>
        /// The license key
        /// </summary>
        private const string LicenseKey = "MN110-1C20DF387EA9952F661A118648FA-468F";

        /// <summary>
        /// The m mail message
        /// </summary>
        private readonly MailBee.Mime.MailMessage _mMailMessage;

        /// <summary>
        /// The m SMTP client
        /// </summary>
        private readonly Smtp _mSmtpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendMail" /> class.
        /// </summary>
        public SendMail()
        {
            _mSmtpClient = new Smtp(LicenseKey);
            _mMailMessage = new MailBee.Mime.MailMessage();
        }

        /// <summary>
        /// Adjuntars the specified ruta.
        /// </summary>
        /// <param name="ruta">The ruta.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Adjuntar(string ruta)
        {
            var control = false;
            if (!File.Exists(ruta))
                return false;
            try
            {
                _mMailMessage.Attachments.Add(ruta);
                control = true;
            }
            catch (Exception)
            {
                // ignored
            }
            return control;
        }

        /// <summary>
        /// Adjuntars the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="nombre">The nombre.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Adjuntar(byte[] bytes, string nombre)
        {
            var control = false;
            try
            {
                _mMailMessage.Attachments.Add(bytes, nombre, "", null, null, NewAttachmentOptions.None, MailTransferEncoding.Base64);
                control = true;
            }
            catch (Exception)
            {
                // ignored
            }
            return control;
        }

        /// <summary>
        /// Inicializa los valores requeridos para el envío del mail.
        /// </summary>
        /// <param name="from">Dirección de correo emisora.</param>
        /// <param name="to">Direcciones de correo receptoras separadas por coma.</param>
        /// <param name="bcc">Direcciones de correo receptoras con copia oculta.</param>
        /// <param name="cc">Direcciones de correo receptoras con copia.</param>
        /// <param name="subject">Asunto del mensaje.</param>
        /// <param name="body">Cuerpo del mensaje (permite código HTML).</param>
        public void LlenarEmail(string from, string to, string bcc, string cc, string subject, string body)
        {
            var emisores = from.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
            var destinatarios = to.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
            var copia = cc.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
            var copiaOculta = bcc.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
            foreach (var email in emisores)
                _mMailMessage.From = new EmailAddress(email);
            foreach (var email in destinatarios)
                _mMailMessage.To.Add(new EmailAddress(email));
            foreach (var email in copia)
                _mMailMessage.Cc.Add(new EmailAddress(email));
            foreach (var email in copiaOculta)
                _mMailMessage.Bcc.Add(new EmailAddress(email));
            _mMailMessage.Subject = subject;
            _mMailMessage.Priority = MailBee.Mime.MailPriority.Normal;
            _mMailMessage.BodyHtmlText = body;
        }

        /// <summary>
        /// Reemplazars the variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="valor">The valor.</param>
        public void ReemplazarVariable(string variable, string valor)
        {
            var text = new Regex(Regex.Escape(variable)).Replace(_mMailMessage.BodyHtmlText, valor, 1);
            _mMailMessage.BodyHtmlText = text;
        }

        /// <summary>
        /// Servidors the STMP.
        /// </summary>
        /// <param name="servidor">The servidor.</param>
        /// <param name="puerto">The puerto.</param>
        /// <param name="ssl">if set to <c>true</c> [SSL].</param>
        /// <param name="emailCredencial">The email credencial.</param>
        /// <param name="passCredencial">The pass credencial.</param>
        public void ServidorSmtp(string servidor, int puerto, bool ssl, string emailCredencial, string passCredencial)
        {
            SmtpServer server;
            if (string.IsNullOrEmpty(emailCredencial) && string.IsNullOrEmpty(passCredencial))
            {
                server = new SmtpServer(servidor) { Port = puerto };
            }
            else
            {
                server = new SmtpServer(servidor, emailCredencial, passCredencial) { Port = puerto };
            }
            _mSmtpClient.SmtpServers.Add(server);
            try { var connected = _mSmtpClient.Connect(); } catch (Exception ex) { }
            try { var reached = _mSmtpClient.Hello(); } catch (Exception ex) { }
            if (!string.IsNullOrEmpty(emailCredencial) && !string.IsNullOrEmpty(passCredencial))
            {
                try { var logged = _mSmtpClient.Login(); } catch (Exception ex) { }
            }
        }

        /// <summary>
        /// Enviars the email.
        /// </summary>
        public void EnviarEmail()
        {
            _mSmtpClient.Message = _mMailMessage;
            var sended = false;
            var maximosIntentos = 5;
            var contadorIntentos = 0;
            Exception ex = null;
            do
            {
                contadorIntentos++;
                try
                {
                    sended = _mSmtpClient.Send();
                }
                catch (Exception _ex)
                {
                    sended = false;
                    ex = _ex;
                }
            } while (!sended && contadorIntentos <= maximosIntentos);
            if (!sended)
            {
                if (ex != null) { throw ex; }
            }
        }
    }
}