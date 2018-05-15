// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 02-19-2017
// ***********************************************************************
// <copyright file="Notificacion.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Notificacion.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Notificacion : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var error = (Exception)Session["errorWeb"];
                var errorMessage = error.Message;
                var errorString = error.ToString();
                var errorStack = error.StackTrace;
                var innerError = error.InnerException;
                if (innerError != null)
                {
                    errorMessage = innerError.Message;
                    errorString = innerError.ToString();
                    errorStack = innerError.StackTrace;
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    lblMsg.Text = errorMessage + "<br/><br/>" + errorString;
                    bDetallesError.Visible = true;
                }
                else
                {
                    lblMsg.Text = "";
                    bDetallesError.Visible = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// BTNs the error.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void BtnError(object sender, EventArgs e)
        {
            if (Session == null || Session["IDENTEMI"] == null)
            {
                Response.Redirect("~/Cerrar.aspx");
            }
            else
            {
                Response.Redirect(Request.UrlReferrer.ToString(), true);
            }
        }
    }
}