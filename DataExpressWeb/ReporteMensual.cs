// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 11-04-2016
//
// Last Modified By : Sergio
// Last Modified On : 11-04-2016
// ***********************************************************************
// <copyright file="ReporteMensual.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataExpressWeb
{
    /// <summary>
    /// Class ReporteMensual.
    /// </summary>
    public static class ReporteMensual
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ReporteMensual"/> is enviado.
        /// </summary>
        /// <value><c>true</c> if enviado; otherwise, <c>false</c>.</value>
        public static bool enviado { get; set; } = false;
    }
}