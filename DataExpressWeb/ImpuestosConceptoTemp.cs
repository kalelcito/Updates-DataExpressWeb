// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernandez
// Created          : 10-20-2017
//
// Last Modified By : Sergio Hernandez
// Last Modified On : 10-20-2017
// ***********************************************************************
// <copyright file="ImpuestosConceptoTemp.cs" company="DataExpress Latinoamérica">
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
    /// Class ImpuestosConceptoTemp.
    /// </summary>
    public class ImpuestosConceptoTemp
    {
        /// <summary>
        /// Gets or sets the identifier comprobante.
        /// </summary>
        /// <value>The identifier comprobante.</value>
        public string IdComprobante { get; set; } = "";
        /// <summary>
        /// Gets or sets the identifier concepto.
        /// </summary>
        /// <value>The identifier concepto.</value>
        public string IdConcepto { get; set; } = "";
        /// <summary>
        /// Gets or sets the identifier impuesto.
        /// </summary>
        /// <value>The identifier impuesto.</value>
        public string IdImpuesto { get; set; } = "";
        /// <summary>
        /// Gets or sets a value indicating whether this instance is retencion.
        /// </summary>
        /// <value><c>true</c> if this instance is retencion; otherwise, <c>false</c>.</value>
        public bool IsRetencion { get; set; } = false;
        /// <summary>
        /// Gets or sets the base.
        /// </summary>
        /// <value>The base.</value>
        public string Base { get; set; } = "";
        /// <summary>
        /// Gets or sets the impuesto.
        /// </summary>
        /// <value>The impuesto.</value>
        public string Impuesto { get; set; } = "";
        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>The descripcion.</value>
        public string Descripcion { get; set; } = "";
        /// <summary>
        /// Gets or sets the tipo factor.
        /// </summary>
        /// <value>The tipo factor.</value>
        public string TipoFactor { get; set; } = "";
        /// <summary>
        /// Gets or sets the tasa o cuota.
        /// </summary>
        /// <value>The tasa o cuota.</value>
        public string TasaOCuota { get; set; } = "";
        /// <summary>
        /// Gets or sets the importe.
        /// </summary>
        /// <value>The importe.</value>
        public string Importe { get; set; } = "";
    }
}