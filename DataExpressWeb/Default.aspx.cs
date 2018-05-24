// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="Default.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Default.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Default : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            //Control.Parametros param = new Control.Parametros();
            //string esIgual;
            //if (param.GetParametroByNombre("EC00001").Status.Equals("5.0"))
            //{
            //    esIgual = param.GetParametroByNombre("EC00001").Status;
            //}
            //else
            //{
            //    esIgual = param.GetParametroByNombre("EC00001").Status;
            //}
            if (Session["rfcUser"] != null)
            {
                // lSesion.Text = (string)(Session["rfcUser"]);
            }

            if (Session["Mensaje"] != null)
            {
                (Master as SiteMaster).MostrarAlerta(this, Session["Mensaje"].ToString(), 2, null);
                Session["mensaje"] = null;
            }
            (Master as SiteMaster).CheckTimbres();
            if (Session["IsCliente"] != null || Session["IsProveedor"] != null)
            {

            }
            else
            {
                if (IsPostBack)
                {

                }
                else
                {
                    var notifArray = (Session["Notificaciones"] == null) ? new List<string[]>() : (List<string[]>)Session["Notificaciones"];
                    for (var i = 0; i < notifArray.Count; i++)
                    {
                        var notificacion = notifArray[i];
                        var tipo = "";
                        if (notificacion[0] == "danger")
                        {
                            tipo = "error";
                        }
                        else if (notificacion[0] == "info")
                        {
                            tipo = "notice";
                        }
                        else
                        {
                            tipo = notificacion[0];
                        }
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "test" + i, "messages('" + notificacion[1] + "','" + tipo + "','" + i + "');", true);
                    }
                }
            }
        }

        private void llenarNotify()
        {
            if (Session["Notificaciones"] != null && !IsPostBack)
            {
                var notifArray = (Session["Notificaciones"] == null) ? new List<string[]>() : (List<string[]>)Session["Notificaciones"];
                var markup = "";
                for (var i = 0; i < notifArray.Count; i++)
                {
                    markup = "";
                    var notificacion = notifArray[i];
                    markup += "<div class='alert alert-" + notificacion[0] + " alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'>x</button><a  href='" + notificacion[2] + "' class='alert-link'>" + notificacion[1] + "</a></div>";
                    LiteralControl lc = new LiteralControl(markup);
                    pNotify.Controls.Add(lc);
                }
            }
        }
    }
}