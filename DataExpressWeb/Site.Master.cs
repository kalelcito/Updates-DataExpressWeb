// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="Site.Master.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ************************************************************************
using System;
using System.Web.UI;
using Datos;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Printing;
using System.Threading;
using System.Text;
using System.Security.Cryptography;
using Control;
using System.Net.Mail;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data;

namespace DataExpressWeb
{
    /// <summary>
    /// Class SiteMaster.
    /// </summary>
    /// <seealso cref="System.Web.UI.MasterPage" />
    public partial class SiteMaster : MasterPage
    {

        /// <summary>
        /// The notificaciones maximas
        /// </summary>
        private const int NotificacionesMaximas = 7;
        /// <summary>
        /// The _hilo timbres
        /// </summary>
        private static Thread _hiloTimbres;
        /// <summary>
        /// The _hilo timbres
        /// </summary>
        private static Thread _hiloMensual;
        /// <summary>
        /// The folios restantes
        /// </summary>
        protected static string FoliosRestantes = "Desconocido";
        /// <summary>
        /// The _em
        /// </summary>
        private SendMail _em;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //var mailer = new MailBee.SmtpMail.Smtp("MN110-1C20DF387EA9952F661A118648FA-468F");
            var modal = Request.QueryString.Get("insidemodal");
            bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");
            ModalContent(isModal);
            if (Session["rfcUser"] != null)
            {
                var rfc = Session["IDENTEMI"]?.ToString() ?? (Session["IDENTEMIEXT"]?.ToString() ?? "CORE");
                var db = new BasesDatos(rfc);

                Perfil.Text = Session["nombreEmpleado"].ToString();
                bool comp = false, inform = false, segurid = false, empresa = false, ajustes = false;
                SetTimeOut();

                #region Permisos

                #region Comprobantes
                if (Session["CRnewComp"] == null || string.IsNullOrEmpty(Session["CRnewComp"].ToString()))
                {
                    liCrear.Visible = false;
                }
                if (Session["CRnewComp"].ToString().Contains("01"))
                {
                    var asistente = Session["asistente"] != null && bool.Parse(Session["asistente"].ToString());
                    liCrear.Visible = true;
                    hlFacturaRestaurante.Visible = asistente;
                    hlFactura.Visible = !asistente;
                    comp = true;
                    hlReformaXls.Visible = (Session["IDENTEMI"].ToString().Equals("LAN7008173R5") || Session["IDENTEMI"].ToString().Equals("OHC080924AV5"));
                    hlGlobalOnQ.Visible = (Session["IDENTEMI"].ToString().Equals("LAN7008173R5") || Session["IDENTEMI"].ToString().Equals("OHC080924AV5"));
                    hlGlobalMicros.Visible = (Session["IDENTEMI"].ToString().Equals("LAN7008173R5") || Session["IDGIRO"].ToString().Equals("2")|| Session["IDENTEMI"].ToString().Equals("SANG761202BN6"));
                    hlFacturaOperaHp.Visible = (Session["IDENTEMI"].ToString().Equals("LAN7008173R5") || Session["IDENTEMI"].ToString().Equals("IET661110DF2") || Session["IDENTEMI"].ToString().Equals("GIO100406FS6"));
                }
                else
                {
                    hlFactura.Visible = false;
                    hlFacturaRestaurante.Visible = false;
                    hlReformaXls.Visible = false;
                    hlGlobalOnQ.Visible = false;
                    hlFacturaOperaHp.Visible = false;
                    hlGlobalMicros.Visible = false;
                    if (Request.Path.IndexOf(hlFactura.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlFacturaRestaurante.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlReformaXls.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlGlobalOnQ.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlGlobalMicros.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Session["CRnewComp"].ToString().Contains("04"))
                {
                    liCrear.Visible = true;
                    liNotaCredito.Visible = true;
                    comp = true;
                }
                else
                {
                    liNotaCredito.Visible = false;
                    if (Request.Path.IndexOf(hlNotaCreditoParcial.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlNotaCreditoAnulacion.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Session["CRnewComp"].ToString().Contains("08") && !Session["IDGIRO"].ToString().Equals("1") && !Session["IDGIRO"].ToString().Equals("2"))
                {
                    liCrear.Visible = true;
                    hlNomina.Visible = true;
                    comp = true;
                }
                else
                {
                    hlNomina.Visible = false;
                    if (Request.Path.IndexOf(hlNomina.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Session["CRnewComp"].ToString().Contains("06") && !Session["IDGIRO"].ToString().Equals("1") && !Session["IDGIRO"].ToString().Equals("2"))
                {
                    liCrear.Visible = true;
                    hlCartaPorte.Visible = true;
                    comp = true;
                }
                else
                {
                    hlCartaPorte.Visible = false;
                    if (Request.Path.IndexOf(hlCartaPorte.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Session["CRnewComp"].ToString().Contains("07") && /*!Session["IDGIRO"].ToString().Equals("1") &&*/ !Session["IDGIRO"].ToString().Equals("2"))
                {
                    liCrear.Visible = true;
                    hlRetenciones.Visible = true;
                    comp = true;
                }
                else
                {
                    hlRetenciones.Visible = false;
                    if (Request.Path.IndexOf(hlRetenciones.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["coFactPropias"]) || Convert.ToBoolean(Session["coFactTodas"]))
                {
                    hlDocumentos.Visible = true;
                    comp = true;
                }
                else
                {
                    hlDocumentos.Visible = false;
                    if (Request.Path.IndexOf(hlDocumentos.NavigateUrl.Replace("~", "")) != -1 && !Request.Path.Contains("recepcion"))
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["DXC"]))
                {
                    hlDXC.Visible = true;
                    comp = true;
                }
                else
                {
                    hlDXC.Visible = false;
                    if (Request.Path.IndexOf(hlDXC.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Session["CfdiVersion"].ToString().Equals("3.3") && Session["IDGIRO"].ToString().Equals("1") && hlDocumentos.Visible)
                {
                    hlDocsPendientes.Visible = true;
                    comp = true;
                }
                else
                {
                    hlDocsPendientes.Visible = false;
                    if (Request.Path.IndexOf(hlDocsPendientes.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Session["IsProveedor"] == null && Session["IsCliente"] == null && (Session["IDGIRO"].ToString().Equals("2") || Session["IDGIRO"].ToString().Equals("1")))
                {
                    hlTickets.Visible = true;
                    comp = true;
                }
                else
                {
                    hlTickets.Visible = false;
                    if (Request.Path.IndexOf(hlTickets.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["VerCanc"]))
                {
                    hlCancelaciones.Visible = true;
                    comp = true;
                }
                else
                {
                    hlCancelaciones.Visible = false;
                    if (Request.Path.IndexOf(hlCancelaciones.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                comp &= (Session["IsProveedor"] == null || !Convert.ToBoolean(Session["IsProveedor"]));
                if (comp)
                {
                    hlEmision.Visible = true;
                    if (Session["IsCliente"] != null && Convert.ToBoolean(Session["IsCliente"]))
                    {
                        hlEmision.Text = "Mis Documentos <span class=\"caret\"></span>";
                        //K
                        hlDocsPendientes.Visible = false;
                        if (Request.Path.IndexOf(hlDocsPendientes.NavigateUrl.Replace("~", "")) != -1 )
                        {
                            Response.Redirect("~/seguridad.aspx");
                        }
                        hlCancelaciones.Visible = false;
                        if (Request.Path.IndexOf(hlCancelaciones.NavigateUrl.Replace("~", "")) != -1)
                        {
                            Response.Redirect("~/seguridad.aspx");
                        }
                        hlDescargarZip.Visible = false;
                        if (Request.Path.IndexOf(hlDescargarZip.NavigateUrl.Replace("~", "")) != -1)
                        {
                            Response.Redirect("~/seguridad.aspx");
                        }
                    }
                }
                else
                {
                    hlEmision.Visible = false;
                }
                hlEditarComp.Visible = comp && Session["EDcompr0N"] != null && Convert.ToBoolean(Session["EDcompr0N"]);

                #endregion

                #region Informes

                if (Convert.ToBoolean(Session["RepGeneral"]))
                {
                    if (rfc.Equals("YAN810728RW7D") || rfc.Equals("LAN7008173R5")){ HyperLink10.Visible = true; }//RFC YAMBAL
                    HyperLink3.Visible = true;
                    inform = true;                  
                }
                else
                {
                    HyperLink3.Visible = false;
                    HyperLink10.Visible = false;                   
                    if (Request.Path.IndexOf(HyperLink3.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (inform)
                {
                    HyperLink3.Visible = true;
                    //HyperLink10.Visible = true; 
                }
                else
                {
                    HyperLink3.Visible = false;
                    //HyperLink10.Visible = false;
                }

                InicioID.Visible = true;
                PerfilID.Visible = true;

                #endregion          

                #region Seguridad

                if (Convert.ToBoolean(Session["PEmple"]))
                {
                    HyperLink18.Visible = true;
                    segurid = true;
                }
                else
                {
                    HyperLink18.Visible = false;
                    if (Request.Path.IndexOf(HyperLink18.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["PClient"]))
                {
                    HyperLink26.Visible = true;
                    hlProveedores.Visible = true;
                    segurid = true;
                }
                else
                {
                    HyperLink26.Visible = false;
                    hlProveedores.Visible = false;
                    if (Request.Path.IndexOf(HyperLink26.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlProveedores.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["PRol"]))
                {
                    HyperLink19.Visible = true;
                    segurid = true;
                }
                else
                {
                    HyperLink19.Visible = false;
                    if (Request.Path.IndexOf(HyperLink19.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (segurid)
                {
                    HyperLink15.Visible = true;
                }
                else
                {
                    HyperLink15.Visible = false;
                }

                #endregion

                #region Empresa

                if (Convert.ToBoolean(Session["EditEmi"]))
                {
                    HyperLink22.Visible = true;
                    empresa = true;
                }
                else
                {
                    HyperLink22.Visible = false;
                    if (Request.Path.IndexOf(HyperLink22.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["EditRecep"]))
                {
                    HyperLink1.Visible = true;
                    empresa = true;
                    if (Session["IDENTEMI"].ToString().Equals("OHC070227M80") || Session["IDENTEMI"].ToString().Equals("OPL000131DL3"))
                    {
                        HyperLink1.Text = "Clientes";
                    }
                }
                else
                {
                    HyperLink1.Visible = false;
                    if (Request.Path.IndexOf(HyperLink11.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["EditEmi"]))
                {
                    HyperLink13.Visible = true;
                    empresa = true;
                }
                else
                {
                    HyperLink13.Visible = false;
                    if (Request.Path.IndexOf(HyperLink13.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["EditPtoEmi"]))
                {
                    HyperLink14.Visible = true;
                    hlCertificados.Visible = true;
                    empresa = true;
                }
                else
                {
                    HyperLink14.Visible = false;
                    hlCertificados.Visible = false;
                    if (Request.Path.IndexOf(HyperLink14.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlCertificados.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if (Convert.ToBoolean(Session["EditInfoGral"]))
                {
                    HyperLink21.Visible = true;
                    empresa = true;
                }
                else
                {
                    HyperLink21.Visible = false;
                    if (Request.Path.IndexOf(HyperLink21.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }
                if ((Session["IDENTEMI"].ToString().Equals("LAN7008173R5") || Session["IDENTEMI"].ToString().Equals("OHC080924AV5")) && hlDocumentos.Visible)
                {
                    HyperLinkFacturaGlobal.Visible = true;
                    empresa = true;
                }
                else
                {
                    HyperLinkFacturaGlobal.Visible = false;
                    if (Request.Path.IndexOf(HyperLinkFacturaGlobal.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (Session["IsCliente"] != null || Session["IsProveedor"] != null)
                {
                    empresa = false;
                }

                if (empresa)
                {
                    HyperLink12.Visible = true;
                }
                else
                {
                    HyperLink12.Visible = false;
                }

                #endregion

                #region Ajustes

                if (Convert.ToBoolean(Session["EditSMTP"]))
                {
                    HyperLink17.Visible = true;
                    ajustes = true;
                }
                else
                {
                    HyperLink17.Visible = false;
                    if (Request.Path.IndexOf(HyperLink17.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }


                if (Convert.ToBoolean(Session["EditMensajes"]))
                {
                    HyperLink24.Visible = true;
                    ajustes = true;
                }
                else
                {
                    HyperLink24.Visible = false;
                    if (Request.Path.IndexOf(HyperLink24.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (Convert.ToBoolean(Session["EnvioEmail"]))
                {
                    HyperLink27.Visible = true;
                    ajustes = true;
                }
                else
                {
                    HyperLink27.Visible = false;
                    if (Request.Path.IndexOf(HyperLink27.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (Convert.ToBoolean(Session["imp"]))
                {
                    HyperLink28.Visible = true;
                    ajustes = true;
                }
                else
                {
                    HyperLink28.Visible = false;
                    if (Request.Path.IndexOf(HyperLink28.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (Convert.ToBoolean(Session["conc"]))
                {
                    bool bOnQ = false;
                    var OnQ = ConfigurationManager.AppSettings.Get("CONCEPTOSONQ");
                    bool.TryParse(OnQ, out bOnQ);
                    hlConceptosOnQ.Visible = Session["IDGIRO"].ToString().Equals("1") && bOnQ;
                    hlConceptos.Visible = true;
                    ajustes = true;
                }
                else
                {
                    hlConceptos.Visible = false;
                    hlConceptosOnQ.Visible = false;
                    if (Request.Path.IndexOf(hlConceptos.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlConceptosOnQ.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (Convert.ToBoolean(Session["arch"]))
                {
                    HyperLink36.Visible = true;
                    ajustes = true;
                }
                else
                {
                    HyperLink36.Visible = false;
                    if (Request.Path.IndexOf(HyperLink36.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (Session["CfdiVersion"] != null && Session["CfdiVersion"].ToString().Equals("3.3"))
                {
                    hlBancos.Visible = true;
                    ajustes = true;
                }
                else
                {
                    hlBancos.Visible = false;
                    if (Request.Path.IndexOf(hlBancos.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (Session["IsProveedor"] != null && !string.IsNullOrEmpty(Session["IsProveedor"].ToString()))
                {
                    ajustes = false;
                }

                if (ajustes)
                {
                    HyperLink16.Visible = true;
                }
                else
                {
                    HyperLink16.Visible = false;
                }

                if (Session["IsCliente"] == null && Session["IsProveedor"] == null)
                {
                    liHelpDesk.Visible = true;
                }
                else
                {
                    liHelpDesk.Visible = false;
                    if (Request.Path.IndexOf(HyperLink34.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                    //K
                    HyperLink16.Visible = false;
                    if (Request.Path.IndexOf(hlBancos.NavigateUrl.Replace("~", "")) != -1 || Request.Path.IndexOf(hlVisorEventos.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                #endregion

                #region Perfil

                if (Convert.ToBoolean(Session["EditPerfil"]))
                {
                    HyperLink4.Visible = true;
                }
                else
                {
                    HyperLink4.Visible = false;
                    if (Request.Path.IndexOf(HyperLink4.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                HyperLink34.Visible = true;

                #endregion

                #region Recepcion

                if (Convert.ToBoolean(Session["Recepcion"]))
                {
                    HyperLink29.Visible = true;
                    HyperLink7.Visible = !(rfc.Equals("HIM890120VEA") || rfc.Equals("SIH071204N90") || rfc.Equals("LAN7008173R5"));
                    HyperLink9.Visible = !HyperLink7.Visible;
                    if (rfc.Equals("HAP9504215L5") || rfc.Equals("OAP981214DP3") || rfc.Equals("HSC0010193M7") || rfc.Equals("OHF921110BF2") || rfc.Equals("OHS0312152Z4")
                        || rfc.Equals("CCO8405097T7") || rfc.Equals("GFE920615EC8") || rfc.Equals("HSO981214MD9") || rfc.Equals("HVA000306NT1") || rfc.Equals("IAE940527BEA") || rfc.Equals("IIR9908194XA")
                        || rfc.Equals("IOP000921LH0") || rfc.Equals("OHM981214KX6") || rfc.Equals("PHJ061113D27") || rfc.Equals("PHO030626236") || rfc.Equals("SES0404281R0") || rfc.Equals("CAS0008318A8") || rfc.Equals("OHE070511872") || rfc.Equals("EOP000815CM5") || rfc.Equals("OHC070227M80") || rfc.Equals("OPL000131DL3")
                        )
                    {
                        HyperLink29.Text = "Proveedores<span class='caret'><span>";
                    }
                }
                else
                {
                    HyperLink29.Visible = false;
                    HyperLink7.Visible = false;
                    HyperLink9.Visible = false;
                    if (Request.Path.IndexOf(ldDocumentosRecepcion.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                if (Session["IDENTEMI"] != null && Session["IDENTEMI"].ToString().Equals("LAN7008173R5"))
                {
                    LiRecepcionVip.Visible = true;
                }

                if (Session["IsProveedor"] != null)
                {
                    HyperLink8.Visible = false;
                    if (Request.Path.IndexOf(HyperLink8.NavigateUrl.Replace("~", "")) != -1)
                    {
                        Response.Redirect("~/seguridad.aspx");
                    }
                }

                #endregion

                #region Ayuda

                hlAyuda.Visible = true;
                var descRol = Session["descRol"] != null ? Session["descRol"].ToString().ToLower() : "";
                var prefix = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "";

                //Manual de Administrador visible solo si el usuario es empleado y administrador
                liManualAdmin.Visible = prefix.Contains("EMPLE") && ((descRol.Contains("admin") || descRol.Contains("delatam") || descRol.Contains("sistema") || descRol.Contains("super")));

                //Manual de Usuario visible solo si el usuario es empleado
                liManualEmpleado.Visible = prefix.Contains("EMPLE");

                //Manual de Usuario visible solo si el usuario es cliente
                liManualCliente.Visible = prefix.Contains("CLIEN");

                //Manual de Proveedor visible solo si el usuario es proveedor
                liManualProveedor.Visible = prefix.Contains("PROVE");

                liManualExtranet.Visible = false; // Falta definir en qué momento se muestra.

                var onqKey = ConfigurationManager.AppSettings.Get("CONCEPTOSONQ");
                var onqVisible = false;
                //bool.TryParse(onqKey, out onqVisible);
                liManualOnQ.Visible = onqVisible;

                #endregion

                hlWebSettings.Visible = liManualAdmin.Visible;
                divConfigGlobal.Style["display"] = (liManualAdmin.Visible && Session["IDENTEMI"] != null && descRol.Contains("delatam") && prefix.Contains("EMPLE")) ? "inline" : "none";

                #endregion

                #region Notificaciones
                //K
                if (Session["IsCliente"] != null)
                {
                    btnNotificaciones.Visible = false;
                }
                else
                {
                    btnNotificaciones.Visible = true;
                }

                if (IsPostBack)
                {
                    if (Page.Request.Params["__EVENTTARGET"].Equals("removeNotif"))
                    {
                        int index;
                        if (int.TryParse(Page.Request.Params["__EVENTARGUMENT"].ToString(), out index))
                        {
                            RemoveNotification(index);
                        }
                    }
                }
                CargaNotificaciones();

                if (Session["IDENTEMI"] == null || (_hiloTimbres != null && _hiloTimbres.IsAlive) || Session["IsCliente"] != null)
                { }
                else
                {
                    _hiloTimbres = new Thread(CheckTimbres);
                    _hiloTimbres.Start();
                }

                if (Session["IDENTEMI"] == null || (_hiloMensual != null && _hiloMensual.IsAlive) || Session["IsCliente"] != null)
                { }
                else
                {
                    _hiloMensual = new Thread(FolioMensual);
                    _hiloMensual.Start();
                }

                #endregion

                if (!IsPostBack)
                {
                    //K revisar accesibility y ubicacion de esta llamada postback
                    //CheckTimbres();
                    
                    SqlDataSourceTipoEmpresa.ConnectionString = db.CadenaConexion;
                    ddlTipoEmpresa.DataSourceID = "SqlDataSourceTipoEmpresa";
                    ddlTipoEmpresa.DataBind();
                    lblTimbresRestantes.DataBind();
                    var sucursal = Session["claveSucursalUser"];
                    if (sucursal != null)
                    {
                        var sSucursal = sucursal.ToString();
                        if (!string.IsNullOrEmpty(sSucursal) && !sSucursal.Equals("S--X"))
                        {
                            lblSucursal.Text = "Sucursal: " + sSucursal + "<br/>";
                            spanSucursal.Style["display"] = "inline-block !important";
                        }
                    }
                    lblUsuario.Text = "Usuario: " + Perfil.Text;
                    divSucursal.Style["display"] = "block";
                }
            }
            else
            {
                if (Request.Path.IndexOf("/cuenta/olvidos.aspx") != -1 || Request.Path.IndexOf("/consultarCodigo.aspx") != -1 || Request.Path.IndexOf("/cuenta/Registro.aspx") != -1 || Request.Path.IndexOf("/download.aspx") != -1 || Request.Path.IndexOf("/Notificacion.aspx") != -1)
                {
                    //InicioID.Visible = true;
                }
                else
                {
                    InicioID.Visible = false;
                    if (Request.Path.IndexOf("/cuenta/Login.aspx") == -1) { Response.Redirect("~/Cerrar.aspx"); }
                }
            }
        }

        /// <summary>
        /// Handles the Selecting event of the source control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlDataSourceCommandEventArgs"/> instance containing the event data.</param>
        protected void source_Selecting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.CommandTimeout = 18000;
        }

        /// <summary>
        /// Sets the time out.
        /// </summary>
        private void SetTimeOut()
        {
            var sources = this.FindDescendants<SqlDataSource>();
            foreach (var source in sources)
            {
                source.Selecting += new SqlDataSourceSelectingEventHandler(source_Selecting);
            }
        }

        #region Control de Notificaciones

        /// <summary>
        /// Removes the notification.
        /// </summary>
        /// <param name="index">The index.</param>
        private void RemoveNotification(int index)
        {
            var notifArray = (Session["Notificaciones"] == null) ? new List<string[]>() : (List<string[]>)Session["Notificaciones"];
            notifArray.RemoveAt(index);
            Session["Notificaciones"] = notifArray;
        }

        /// <summary>
        /// Cargas the notificaciones.
        /// </summary>
        private void CargaNotificaciones()
        {
            var notifArray = (Session["Notificaciones"] == null) ? new List<string[]>() : (List<string[]>)Session["Notificaciones"];
            countNotifications.InnerText = notifArray.Count.ToString();
            var markup = "";
            for (var i = 0; i < notifArray.Count; i++)
            {
                var notificacion = notifArray[i];
                markup += "<div class='alert alert-" + notificacion[0] + " alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close' onclick = '__doPostBack(\"removeNotif\", " + i + ");'>x</button><a  href='" + notificacion[2] + "' class='alert-link' onclick = '__doPostBack(\"removeNotif\", " + i + ");'>" + notificacion[1] + "</a></div>";
            }
            btnNotificaciones.Attributes["data-content"] = markup;
        }

        /// <summary>
        /// Agregars the alerta session.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="href">The href.</param>
        /// <param name="level">The level.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool AgregarAlertaSession(string text, string href = "#", string level = "info")
        {
            try
            {
                var notifArray = (Session["Notificaciones"] == null) ? new List<string[]>() : (List<string[]>)Session["Notificaciones"];
                var notif = new string[] { level, text, href };
                var contains = ArrayInListArray(notifArray, notif);
                if (contains)
                {
                    return false;
                }
                notifArray.Add(notif);
                if (notifArray.Count > NotificacionesMaximas)
                {
                    notifArray = notifArray.Skip(Math.Max(0, notifArray.Count() - NotificacionesMaximas)).ToList();
                }
                Session["Notificaciones"] = notifArray;
                CargaNotificaciones();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Arrays the in list array.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="arr">The arr.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private static bool ArrayInListArray(IEnumerable<string[]> list, IList<string> arr)
        {
            foreach (var item in list)
            {
                var control = true;
                for (var i = 0; i < item.Length; i++) { control &= item[i].Equals(arr[i]); }
                if (control) { return true; }
            }
            return false;
        }

        #endregion

        #region Control de Alertas

        /// <summary>
        /// MD5s the alert.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string Md5Alert()
        {
            var sb = new StringBuilder();
            using (var sha1 = MD5.Create())
            {
                var fecha = Localization.Now.ToString("s");
                var inputBytes = Encoding.ASCII.GetBytes(fecha);
                var hashBytes = sha1.ComputeHash(inputBytes);
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Handles the Click event of the btnSaveWebSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSaveWebSettings_Click(object sender, EventArgs e)
        {
            if (sender != null && e != null)
            {
                GuardarSettings();
            }
        }

        /// <summary>
        /// Mostrars the alerta bootstrap.
        /// </summary>
        /// <param name="tipoAlerta">The tipo alerta.</param>
        /// <param name="mensajeHtml">The mensaje HTML.</param>
        /// <param name="href">The href.</param>
        public void MostrarAlertaBootstrap(string tipoAlerta, string mensajeHtml, string href = "#")
        {
            if (!AgregarAlertaSession(mensajeHtml, href, tipoAlerta))
            {
                return;
            }
            var js = "alertBootstrap('" + tipoAlerta + "','" + mensajeHtml + "');";
            ScriptManager.RegisterStartupScript(this, GetType(), "_alertBootstrap", js, true);
        }

        #endregion

        /// <summary>
        /// Listas the impresoras.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String[]&gt;.</returns>
        public static IEnumerable<string[]> ListaImpresoras()
        {
            var limpresoras = new List<string[]>();
            try
            {
                for (var a = 0; a < PrinterSettings.InstalledPrinters.Count; a++)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(PrinterSettings.InstalledPrinters[a]))
                        {
                            limpresoras.Add(new string[] { a.ToString(), PrinterSettings.InstalledPrinters[a] });
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch
            {
                // ignored
            }
            return limpresoras;
        }

        /// <summary>
        /// Checks the timbres.
        /// </summary>
        public void CheckTimbres()
        {
            try
            {
                var rfcEmisor = Session["IDENTEMI"].ToString();
                var wsTimbres = new wsTimbres.wsCertTimbres { Timeout = (1800 * 1000) };
                var respuestaFolios = wsTimbres.ValidaFolios(rfcEmisor);
                var respuestaCertificado = wsTimbres.ValidaCertificado(rfcEmisor);
                var hoy = Localization.Now;
                var foliosUsados = respuestaFolios[0];
                var foliosRestantes = respuestaFolios[1];
                var foliosFin = respuestaFolios[2];
                var cernum = respuestaCertificado[0];
                var fechaFin = Localization.Parse(respuestaCertificado[1]);
                var tiempoRestante = respuestaCertificado[2];
                if (foliosFin > 0)
                {
                    FoliosRestantes = foliosRestantes + " de " + foliosFin;
                    if (foliosUsados >= foliosFin)
                    {
                        Session["errorTimbres"] = true;
                        var mensaje = "No le quedan timbres: " + foliosRestantes + "/" + foliosFin;
                        SendMail("Notificación de Timbres", mensaje);
                        MostrarAlertaBootstrap("danger", "<strong>Timbres: </strong>" + mensaje, ResolveUrl("~/configuracion/Herramientas/helpdesk.aspx"));
                    }
                    else if (foliosRestantes < 0)
                    {
                        Session["errorTimbres"] = true;
                        var mensaje = "Se ha excedido en timbres: " + (-1 * foliosRestantes) + "/" + foliosFin;
                        SendMail("Notificación de Timbres", mensaje);
                        MostrarAlertaBootstrap("danger", "<strong>Timbres: </strong>" + mensaje, ResolveUrl("~/configuracion/Herramientas/helpdesk.aspx"));
                    }
                    else if (foliosRestantes <= 200)
                    {
                        var mensaje = "Le quedan pocos timbres: " + foliosRestantes + "/" + foliosFin;
                        SendMail("Notificación de Timbres", mensaje);
                        MostrarAlertaBootstrap("danger", "<strong>Timbres: </strong>" + mensaje, ResolveUrl("~/configuracion/Herramientas/helpdesk.aspx"));
                    }
                }
                else
                {
                    FoliosRestantes = "-";
                }
                var dt20 = fechaFin.AddDays(-20);
                if (hoy >= dt20 && hoy < fechaFin)
                {
                    var mensaje = "Su certificado \"" + cernum + "\" caducará en: " + tiempoRestante;
                    SendMail("Notificación de Certificado", mensaje);
                    MostrarAlertaBootstrap("warning", "<strong>Certificado: </strong>" + mensaje);
                }
                else if (fechaFin.ToString("M/d/yyyy h").Equals(hoy.ToString("M/d/yyyy h")))
                {
                    Session["errorCertificado"] = true;
                    var mensaje = "Su certificado \"" + cernum + "\" ha caducado";
                    SendMail("Notificación de Certificado", mensaje);
                    MostrarAlertaBootstrap("warning", "<strong>Certificado: </strong>" + mensaje);
                }
            }
            catch (Exception ex)
            {
                MostrarAlertaBootstrap("warning", "<strong>Timbres/Certificado: </strong>" + ex.Message);
            }
        }

        /// <summary>
        /// Enviars the mail.
        /// </summary>
        /// <param name="asunto">The asunto.</param>
        /// <param name="mensaje">The mensaje.</param>
        private void SendMail(string asunto, string mensaje)
        {
            string _servidor = "";
            int _puerto = 0;
            bool _ssl = false;
            string _emailCredencial = "";
            string _passCredencial = "";
            string _rutaDoc = "";
            string _emailEnviar = "";
            string emails = "";
            if (Session["idUser"] == null)
            {
                return;
            }
            try
            {
                _em = new SendMail();
                var _db = new BasesDatos(Session["IDENTEMI"]?.ToString() ?? "CORE");
                _db.Conectar();
                _db.CrearComando(@"SELECT servidorsmtp, 
       puertosmtp, 
       sslsmtp, 
       usersmtp, 
       passsmtp, 
       dirdocs, 
       emailenvio, 
       ( emailnotificacion + ',' + emailbcc + ',' 
         + emailopera ) AS emailNotificacion 
FROM   par_parametrossistema");
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
                    emails += dr1[7].ToString();
                }
                _db.Desconectar();
                //emails = "sehernandez@dataexpressintmx.com"; // <------ Prueba
                var emailsArray = emails.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (emailsArray.Count() > 0)
                {
                    emails = string.Join(",", emailsArray);
                    try
                    {
                        _em.LlenarEmail(_emailEnviar, emails, "", "", asunto, mensaje);
                        _em.EnviarEmail();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// FolioMensual.
        /// </summary>
        private void FolioMensual()
        {
            var hoy = Localization.Now;
            var enviadoAntes = ReporteMensual.enviado;
            if (hoy.Day == 1 && !enviadoAntes)
            {
                var dt = new DataTable();
                dt.Columns.Add("Serie");
                dt.Columns.Add("Folio Inicial");
                dt.Columns.Add("Folio Final");
                dt.Columns.Add("Folios Totales");
                var mes = hoy.AddMonths(-1).ToString("MMMM");
                var fechaInicio = hoy.AddMonths(-1).ToString("yyyyMMdd");
                var fechaFin = hoy.AddDays(-1).ToString("yyyyMMdd");
                string _emailfm = "";
                string _nombreEmisor = "";
                var rfc = Session["IDENTEMI"]?.ToString() ?? (Session["IDENTEMIEXT"]?.ToString() ?? "CORE");
                var _db = new BasesDatos(rfc);
                _db.Conectar();
                _db.CrearComando("select serie, MIN(folio) as folioInicial, MAX(folio) as folioFinal, COUNT(*) as totalFolios from dat_general where fecha >= @fechaInicio and fecha <= @fechaFin and (convert(NVARCHAR, estado) + tipo) IN ('1E', '0C', '2C', '4E') group by serie");
                try
                {
                    _db.AsignarParametroCadena("@fechaInicio", fechaInicio);
                }
                catch { }
                try
                {
                    _db.AsignarParametroCadena("@fechaFin", fechaFin);
                }
                catch { }
                var dr = _db.EjecutarConsulta();

                while (dr.Read())
                {
                    var _serie = dr[0].ToString();
                    var _folioInicial = Convert.ToInt32(dr[1].ToString());
                    var _folioFinal = Convert.ToInt32(dr[2].ToString());
                    var _TotalF = Convert.ToInt32(dr[3].ToString());
                    dt.Rows.Add(_serie, _folioInicial, _folioFinal, _TotalF);
                }
                _db.Desconectar();

                _db.Conectar();
                _db.CrearComando("SELECT DISTINCT TOP 1 p.emailFmensual, c.NOMEMI FROM Par_ParametrosSistema p, Cat_Emisor c WHERE c.RFCEMI = @rfc");
                _db.AsignarParametroCadena("@rfc", rfc);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    _emailfm = dr[0].ToString();
                    _nombreEmisor = dr[1].ToString();
                }
                _db.Desconectar();

                if (_emailfm.Length > 15)
                {
                    #region Enviar Mail

                    string asunto = "Reporte de Folios de " + _nombreEmisor + " [" + Session["IDENTEMI"].ToString() + "]";
                    var mensaje = @"A continuación se detallan los timbres consumidos por " + _nombreEmisor + " con RFC " + Session["IDENTEMI"].ToString() + " durante el mes de " + mes + " del presente año." +
                        "<br/><br/>" + ControlUtilities.ConvertDataTableToHTML(dt) + "<br/><br/>Saludos cordiales.";
                    string _servidor = "";
                    int _puerto = 0;
                    bool _ssl = false;
                    string _emailCredencial = "";
                    string _passCredencial = "";
                    string _rutaDoc = "";
                    string _emailEnviar = "";
                    if (Session["idUser"] == null)
                    {
                        return;
                    }
                    try
                    {
                        _em = new SendMail();
                        _db.Conectar();
                        _db.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio from Par_ParametrosSistema");
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
                        }
                        _db.Desconectar();
                        try
                        {
                            _em.ServidorSmtp(_servidor, _puerto, _ssl, _emailCredencial, _passCredencial);
                            _em.LlenarEmail(_emailEnviar, _emailfm.Trim(','), "", "", asunto, mensaje);
                            _em.EnviarEmail();
                            ReporteMensual.enviado = true;
                        }
                        catch (SmtpException)
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    #endregion
                }

            }
        }

        /// <summary>
        /// Modals the content.
        /// </summary>
        /// <param name="isModal">if set to <c>true</c> [is modal].</param>
        private void ModalContent(bool isModal = false)
        {
            var url = Request.AppRelativeCurrentExecutionFilePath.ToLower();
            navBarMenu.Visible = !isModal;
            footer.Visible = !isModal;
            divHeader.Visible = !isModal;
            divSucursal.Visible = !isModal;
            var colorConfig = ConfigurationManager.AppSettings.Get("ColorDefault");
            var colorSession = Session["colorFondo"] != null ? Session["colorFondo"].ToString() : (!string.IsNullOrEmpty(colorConfig) ? colorConfig : "90,175,212");
            var transparency = "0.75";
            var color = "linear-gradient(rgba(" + colorSession + "," + transparency + "),rgba(" + colorSession + "," + transparency + "))";
            var background = isModal ? "white" : (color + ",url('" + ResolveClientUrl(hfBackground.Value)) + "') no-repeat center center fixed";
            bodyMaster.Style["background"] = background + " !important";
            bodyMaster.Style["background-size"] = "cover !important";
            if (isModal)
            {
                bodyMaster.Style["padding-top"] = "0";
                bodyMaster.Style["margin"] = "0";
            }
        }

        /// <summary>
        /// Mostrars the URL modal.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="url">The URL.</param>
        /// <param name="titulo">The titulo.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="customHeightPx">The custom height px.</param>
        /// <param name="customWidth">Width of the custom.</param>
        public void MostrarUrlModal(Page page, string url, string titulo, string args = "", string callback = "", int customHeightPx = 350, string customWidth = "90%")
        {
            var js = $"loadUrlModal('{url}', '{args}', '{titulo}', '{callback}'" + (customHeightPx > 0 ? $", '{customHeightPx}'" : "") + (!string.IsNullOrEmpty(customWidth) ? $", '{customWidth}'" : "") + ");";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "_key" + Md5Alert(), js, true);
        }

        /// <summary>
        /// Muestra mensaje de alerta (modal) con el estilo de Twitter Bootstrap
        /// </summary>
        /// <param name="page">Página desde donde se mostrará el mensaje</param>
        /// <param name="mensaje">Cuerpo del mensaje (puede llevar HTML)</param>
        /// <param name="messageLevel">Nivel del mensaje
        /// <para>0 (default) = null | 1 = success | 2 = info | 3 = question | 4 = warning | 5 = error</para></param>
        /// <param name="size">Tamaño del mensaje<para>null(default) = mediano | small = pequeño | large = grande</para></param>
        /// <param name="jsCallBack">Script (JS) que se ejecutará al cerrar el mensaje</param>
        /// <param name="jsAlong">Script (JS) que se ejecutará al mismo tiempo que se muestra el mensaje</param>
        public void MostrarAlerta(Page page, string mensaje, int messageLevel = 0, string size = null, string jsCallBack = null, string jsAlong = null)
        {
            var js = !string.IsNullOrEmpty(jsAlong) ? jsAlong : "";
            js += "alertBootBox('" + mensaje.Replace(Environment.NewLine, "<br />").Replace("'", "\"") + "'";
            js += "," + messageLevel;
            js += ",'" + (!string.IsNullOrEmpty(jsCallBack) ? jsCallBack.Replace("'", "\"") : "") + "'";
            js += ",'" + (!string.IsNullOrEmpty(size) ? size : "") + "'";
            js += ");";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "_key" + Md5Alert(), js, true);
        }

        /// <summary>
        /// Muestra mensaje de alerta (modal) con el estilo de Twitter Bootstrap
        /// </summary>
        /// <param name="page">Página desde donde se mostrará el mensaje</param>
        /// <param name="mensaje">Cuerpo del mensaje (puede llevar HTML)</param>
        /// <param name="titulo">Titulo del mensaje</param>
        /// <param name="size">Tamaño del mensaje<para>null(default) = mediano | small = pequeño | large = grande</para></param>
        /// <param name="jsCallBack">Script (JS) que se ejecutará al cerrar el mensaje</param>
        /// <param name="jsAlong">Script (JS) que se ejecutará al mismo tiempo que se muestra el mensaje</param>
        public void MostrarAlerta(Page page, string mensaje, string titulo = null, string size = null, string jsCallBack = null, string jsAlong = null)
        {
            var js = !string.IsNullOrEmpty(jsAlong) ? jsAlong : "";
            js += "alertBootBoxTitle('" + mensaje.Replace(Environment.NewLine, "<br />").Replace("'", "\"") + "'";
            js += ",'" + (!string.IsNullOrEmpty(titulo) ? titulo : "").Replace(Environment.NewLine, "<br />").Replace("'", "\"") + "'";
            js += ",'" + (!string.IsNullOrEmpty(jsCallBack) ? jsCallBack.Replace("'", "\"") : "") + "'";
            js += ",'" + (!string.IsNullOrEmpty(size) ? size : "") + "'";
            js += ");";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "_key" + Md5Alert(), js, true);
        }

        /// <summary>
        /// Handles the Click event of the hlWebSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void hlWebSettings_Click(object sender, EventArgs e)
        {
            var colorSession = Session["colorFondo"] != null ? Session["colorFondo"].ToString() : "90,175,212";
            tbColorFondo.Text = "rgb(" + colorSession + ")";
            try
            {
                ddlTipoEmpresa.SelectedValue = Session["IDGIRO"].ToString();
            }
            catch { }
            btnSaveWebSettings.Enabled = true;
            var version = Session["CfdiVersion"] != null ? Session["CfdiVersion"].ToString() : "3.2";
            ddlVersionCfdi.SelectedValue = version;
            string[] conectores = null;
            if (Session["Conectores"] != null)
            {
                conectores = (string[])Session["Conectores"];
            }
            if (conectores != null)
            {
                var otros = new List<string>();
                var itemsPms = chkPms.Items.Cast<ListItem>().ToList();
                var itemsPos = chkPos.Items.Cast<ListItem>().ToList();
                var itemsErp = chkErp.Items.Cast<ListItem>().ToList();
                foreach (var conector in conectores)
                {
                    var itemPms = itemsPms.FirstOrDefault(item => item.Value.Equals(conector, StringComparison.OrdinalIgnoreCase));
                    var itemPos = itemsPos.FirstOrDefault(item => item.Value.Equals(conector, StringComparison.OrdinalIgnoreCase));
                    var itemErp = itemsErp.FirstOrDefault(item => item.Value.Equals(conector, StringComparison.OrdinalIgnoreCase));
                    if (itemPms != null) { itemPms.Selected = true; }
                    else if (itemPos != null) { itemPos.Selected = true; }
                    else if (itemErp != null) { itemErp.Selected = true; }
                    else { otros.Add(conector); }
                }
                if (otros.Count > 0)
                {
                    var otrosConectores = string.Join(",", otros);
                    tbOtrosErp.Text = otrosConectores;
                }
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "_key" + Md5Alert(), "$('#webSettings').modal('show');", true);
        }

        /// <summary>
        /// Guardars the settings.
        /// </summary>
        private void GuardarSettings()
        {
            var mensajes = new List<string>();
            if (Session["IDENTEMI"] != null)
            {
                var sColor = tbColorFondo.Text.Replace("rgb", "").Replace("(", "").Replace(")", "");
                var _db = new BasesDatos(Session["IDENTEMI"].ToString());
                try
                {
                    _db.Conectar();
                    _db.CrearComando("UPDATE Par_ParametrosSistema SET colorFondo = @color OUTPUT inserted.colorFondo");
                    _db.AsignarParametroCadena("@color", sColor);
                    var dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        Session["colorFondo"] = dr[0].ToString();
                    }
                }
                catch (Exception ex) { mensajes.Add("El color de fondo no se pudo actualizar: " + ex.Message); }
                finally { _db.Desconectar(); }
                if (!divConfigGlobal.Style["display"].Equals("none"))
                {
                    var _dbCore = new BasesDatos("CORE");
                    try
                    {
                        _db.Conectar();
                        _db.CrearComando("UPDATE Cat_Emisor SET EmpresaTipo = @empresaTipo");
                        _db.AsignarParametroCadena("@empresaTipo", ddlTipoEmpresa.SelectedValue);
                        _db.EjecutarConsulta1();
                    }
                    catch (Exception ex) { mensajes.Add("El tipo de empresa (Emision) no se pudo actualizar: " + ex.Message); }
                    finally { _db.Desconectar(); }
                    try
                    {
                        _dbCore.Conectar();
                        _dbCore.CrearComando("UPDATE Cat_Emisor SET EmpresaTipo = @empresaTipo");
                        _dbCore.AsignarParametroCadena("@empresaTipo", ddlTipoEmpresa.SelectedValue);
                        _dbCore.EjecutarConsulta1();
                    }
                    catch (Exception ex) { mensajes.Add("El tipo de empresa (Core) no se pudo actualizar: " + ex.Message); }
                    finally { _dbCore.Desconectar(); }
                    try
                    {
                        _db.Conectar();
                        _db.CrearComando("UPDATE Cat_Configuracion SET Status = @version WHERE nomConfiguracion = 'VersionCfdi'");
                        _db.AsignarParametroCadena("@version", ddlVersionCfdi.SelectedValue);
                        _db.EjecutarConsulta1();
                    }
                    catch (Exception ex) { mensajes.Add("La version del CFDI no se pudo actualizar: " + ex.Message); }
                    finally { _db.Desconectar(); }
                    try
                    {
                        var listaConectores = new List<string>();
                        var erpSeleccionados = chkErp.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();
                        var posSeleccionados = chkPos.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();
                        var pmsSeleccionados = chkPms.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();
                        var otros = tbOtrosErp.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        listaConectores.AddRange(erpSeleccionados);
                        listaConectores.AddRange(posSeleccionados);
                        listaConectores.AddRange(pmsSeleccionados);
                        listaConectores.AddRange(otros);
                        var conectores = string.Join("|", listaConectores);
                        _db.Conectar();
                        _db.CrearComando("UPDATE Cat_Configuracion SET Status = @conectores WHERE nomConfiguracion = 'TiposConecores'");
                        _db.AsignarParametroCadena("@conectores", conectores);
                        _db.EjecutarConsulta1();
                    }
                    catch (Exception ex) { mensajes.Add("La lista de conectores no se pudo actualizar: " + ex.Message); }
                    finally { _db.Desconectar(); }
                }
                if (mensajes.Count <= 0)
                {
                    MostrarAlerta(Page, "La configuración se guardó correctamente, es necesario volver a iniciar sesión.<br/>Al cerrar este dialogo se le redireccionara a la página de Login", 2, null, "window.location.href = '" + ResolveClientUrl("~/Cerrar.aspx") + "';");
                }
                else
                {
                    MostrarAlerta(Page, "Algunas configuraciones pudieron no guardarse correctamente:<br/><br/>" + string.Join("<br/>", mensajes) + "<br/><br/>Es necesario volver a iniciar sesión.<br/>Al cerrar este dialogo se le redireccionara a la página de Login", 4, null, "window.location.href = '" + ResolveClientUrl("~/Cerrar.aspx") + "';");
                }
                var modal = Request.QueryString.Get("insidemodal");
                bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");
                ModalContent(isModal);
            }
            else
            {
                Response.Redirect("~/Cerrar.aspx");
            }
            btnSaveWebSettings.Enabled = false;
        }

        /// <summary>
        /// Handles the Click event of the bColorDefault control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bColorDefault_Click(object sender, EventArgs e)
        {
            tbColorFondo.Text = "rgb(90,175,212)";
            //GuardarSettings();
        }
    }
}
