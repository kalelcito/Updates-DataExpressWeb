// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 04-09-2017
// ***********************************************************************
// <copyright file="helpdesk.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using Control;
using Datos;


namespace DataExpressWeb.configuracion.Herramientas
{
    /// <summary>
    /// Class Helpdesk.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Helpdesk : Page
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
        /// The _ruc emi
        /// </summary>
        private string _rucEmi = "";
        /// <summary>
        /// The _razon emi
        /// </summary>
        private string _razonEmi = "";
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
        /// The _posted attachment
        /// </summary>
        private static HttpPostedFile _postedAttachment;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            }
            //log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            //if (!IsPostBack)
            //{
            if (Session["IDEMI"] != null)
            {
                if (Session["nombreEmpleado"] != null)
                {
                    if (Session["USERNAME"] != null)
                    {
                        _db.Conectar();
                        _db.CrearComando(@"select NOMEMI,RFCEMI,nombreComercial from Cat_Emisor where IDEEMI=@IDEEMI");
                        _db.AsignarParametroCadena("@IDEEMI", Session["IDEMI"].ToString());
                        var dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            _rucEmi = dr[1].ToString();
                            lbNombreEmp.Text = !string.IsNullOrEmpty(dr[2].ToString()) ? dr[2].ToString() : dr[0].ToString();
                            _razonEmi = dr[0].ToString();
                        }
                        _db.Desconectar();
                        lbNbC.Text = Session["nombreEmpleado"] + " : " + Session["USERNAME"];
                        lbFchC.Text = Localization.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (!IsPostBack)
                    {

                    }
                    else
                    {

                    }
                }
            }
            //}
        }

        /// <summary>
        /// Consultas the database.
        /// </summary>
        private void ConsultaDb()
        {
            try
            {
                //DB.Conectar();
                //DB.CrearComando(@"SELECT dirP12, passP12,FechaCADP12 FROM Par_ParametrosSistema where idparametro='" + ddlRucEmi .SelectedValue.ToString() + "'");
                //DR = DB.EjecutarConsulta();
                //while (DR.Read())
                //{

                //    lbDirectorio.Text = DR[0].ToString().Trim();
                //    tbPass.Text = DR[1].ToString().Trim();
                ////    fechaLav = DR[2].ToString();
                //    tbFechaCaducidad.Text = DR[2].ToString();

                //}
                //DB.Desconectar();
                ////MessageBox.Show("Conexión Realizada con Exito.");
                //textBoxDirP12.Text = RutaP12;
                //textBoxKeyP12.Text = PassP12;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Guardars the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void GuardarClick(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlRucEmi control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlRucEmi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConsultaDb();
        }

        /// <summary>
        /// Handles the Click event of the btNIncidencia control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btNIncidencia_Click(object sender, EventArgs e)
        {
            divNueva.Visible = true;
            tbEmail.Text = "";
            Asunto0.Text = "";
            tbCodTick.Text = "MX" + Session["IDEMI"] + Session["IDGIRO"] + Localization.Now.ToString("yyyyMMddHHmmss");
            ScriptManager.RegisterStartupScript(this, GetType(), "_setMsgJS", "setSn('', '#" + txtEditor.ClientID + "');", true);
        }

        /// <summary>
        /// Handles the Click event of the btEnvio control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btEnvio_Click(object sender, EventArgs e)
        {
            var html = txtEditor.Value;
            var emails = "";

            try
            {
                #region Enviar E-Mail

                _db = new BasesDatos("");
                if (Session["IDENTEMI"] != null)
                {
                    _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                }
                //log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
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
            }
            catch (Exception)
            {
            }
            emails = tbEmail.Text.Trim(',');
            var asunto = "";
            var mensaje = "";

            _em = new SendMail();

            _em.ServidorSmtp(_servidor, _puerto, _ssl, _emailCredencial, _passCredencial);
            try
            {
                if (emails.Length > 0)
                {
                    //string emir = "";
                    _db.Conectar();
                    _db.CrearComando(@"SELECT nombre,mensaje FROM Cat_Mensajes where nombre='MensajePortalWebTicket' ");
                    var drSum = _db.EjecutarConsulta();
                    if (drSum.Read())
                    {
                        mensaje = drSum[1].ToString();
                    }
                    _db.Desconectar();

                    asunto = "Solicitud de Soporte con el ticket: " + tbCodTick.Text + " Empresa: " + lbNombreEmp.Text + "";

                    _em.LlenarEmail(_emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                    try
                    {
                        _em.ReemplazarVariable("@RfcEmisor", _rucEmi);
                        _em.ReemplazarVariable("@Emisor", _razonEmi);
                        _em.ReemplazarVariable("@Username", lbNbC.Text);
                        _em.ReemplazarVariable("@FechaTicket", lbFchC.Text);
                        _em.ReemplazarVariable("@CodigoTicket", tbCodTick.Text);
                        _em.EnviarEmail();

                        _em = new SendMail();
                        _em.ServidorSmtp(_servidor, _puerto, _ssl, _emailCredencial, _passCredencial);
                        asunto = "Solicitud de Soporte con el ticket: " + tbCodTick.Text + " Empresa: " + lbNombreEmp.Text + "";
                        mensaje = html + "<br/><br/> Correo del cliente: " + emails.Trim(',');
                        var datos = _postedAttachment != null ? PostedFileToBytes(_postedAttachment) : null;
                        if (datos != null)
                        {
                            _em.Adjuntar(datos, _postedAttachment.FileName);
                        }
                        _em.LlenarEmail(_emailEnviar, "helpdesk.dataexpress@gmail.com", "", "", asunto, mensaje);
                        _em.EnviarEmail();

                        _db.Conectar();
                        _db.CrearComandoProcedimiento("PA_insertar_ticket");
                        _db.AsignarParametroProcedimiento("@CodigoTicket", DbType.String, tbCodTick.Text);
                        _db.AsignarParametroProcedimiento("@Fecha", DbType.DateTime, Convert.ToDateTime(lbFchC.Text));
                        _db.AsignarParametroProcedimiento("@NombreEmpleado", DbType.String, lbNbC.Text);
                        _db.AsignarParametroProcedimiento("@Email", DbType.String, tbEmail.Text);
                        _db.AsignarParametroProcedimiento("@Descripcion", DbType.String, Asunto0.Text);
                        _db.AsignarParametroProcedimiento("@Mensaje", DbType.String, html);
                        _db.AsignarParametroProcedimiento("@Archivo", DbType.String, "");
                        _db.EjecutarConsulta1();
                        _db.Desconectar();

                        (Master as SiteMaster).MostrarAlerta(this, "Se ha enviado notificación al HelpDesk con número de ticket \"" + tbCodTick.Text + "\".<br/>Por favor esté al pendiente de su correo electrónico.", 2, null, "window.location.href = '" + ResolveClientUrl("~/configuracion/Herramientas/helpdesk.aspx") + "';");
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "No se Envio correctamente el Ticket" + "<br /><br />" + ex.Message, 4, null);
                        return;
                    }

                    #endregion
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Una direccion de correo para levantar el ticket", 4, null);
                    return;
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se Envio correctamente el Ticket" + "<br /><br />" + ex.Message, 4, null);
                return;
            }
        }

        /// <summary>
        /// Handles the Click event of the btConsultarTck control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btConsultarTck_Click(object sender, EventArgs e)
        {
            var mensaje = "";
            _db.Conectar();
            _db.CrearComando(@"SELECT idTicket,CodigoTicket,Fecha,NombreEmpleado,Email,Descripcion,Mensaje,Archivo  FROM Cat_Ticket where CodigoTicket=@CodigoTicket");
            _db.AsignarParametroCadena("@CodigoTicket", tbCodTick.Text);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                divNueva.Visible = true;
                tbCodTick.Enabled = false;
                btConsultarTck.Visible = false;
                lbEmail.Visible = true;
                tbEmail.Text = "";
                tbEmail.Enabled = false;
                Asunto0.Text = "";
                Asunto0.Enabled = false;
                btEnvio.Visible = false;
                _rucEmi = dr[0].ToString();
                lbNombreEmp.Text = dr[1].ToString();
                lbFchC.Text = dr[2].ToString();
                _razonEmi = dr[3].ToString();
                tbEmail.Text = dr[4].ToString();
                Asunto0.Text = dr[5].ToString();
                mensaje = dr[6].ToString();
                _razonEmi = dr[7].ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "_setMsgJS", "setSn('" + mensaje + "', '#" + txtEditor.ClientID + "');", true);
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "No hay ningun ticket asociado al código " + tbCodTick.Text, 4, null);
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the Click event of the btClean control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btClean_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/configuracion/Herramientas/helpdesk.aspx");
        }

        /// <summary>
        /// Handles the UploadedComplete event of the FileUpload1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AjaxControlToolkit.AsyncFileUploadEventArgs" /> instance containing the event data.</param>
        protected void FileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            _postedAttachment = FileUpload1.PostedFile;
        }

        /// <summary>
        /// Posteds the file to bytes.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.Byte[].</returns>
        private byte[] PostedFileToBytes(HttpPostedFile file)
        {
            var output = new MemoryStream();
            var stream = file.InputStream;
            using (output)
            {
                stream.Position = 0;
                stream.CopyTo(output);
            }
            var result = output.ToArray();
            return result;
        }
    }
}