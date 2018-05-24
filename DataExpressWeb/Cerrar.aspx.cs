// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 10-17-2016
// ***********************************************************************
// <copyright file="Cerrar.aspx.cs" company="DataExpress Latinoamérica">
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
    /// Class Cerrar.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Cerrar : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var urlRedirect = (Session["ExternalAuthUrl"] != null && !string.IsNullOrEmpty(Session["ExternalAuthUrl"].ToString())) ? Session["ExternalAuthUrl"].ToString() : "~/cuenta/Login.aspx";
            //Session.Clear();
            //Session.Abandon();
            var sessionsToRemove = new List<string>();
            foreach (string key in Session.Keys)
            {
                if (key != "CatalogosCfdi33")
                    sessionsToRemove.Add(key);
            }

            foreach (var key in sessionsToRemove)
            {
                Session.Remove(key);
            }
            Response.Redirect(urlRedirect);
        }
    }
}