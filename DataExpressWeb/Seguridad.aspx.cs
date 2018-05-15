// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="Seguridad.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;

/// <summary>
/// The DataExpressWeb namespace.
/// </summary>
namespace DataExpressWeb
{
    /// <summary>
    /// Class Seguridad.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Seguridad : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["rfcUser"] != null)
            {
                // lSesion.Text = (string)(Session["rfcUser"]);
            }
        }
    }
}