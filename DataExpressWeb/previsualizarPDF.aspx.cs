// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 02-19-2017
// ***********************************************************************
// <copyright file="descargarPDF.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using DataExpressWeb.wsEmision;
using System;
using System.Text;
using System.Web.UI;

namespace DataExpressWeb
{
    /// <summary>
    /// Class VisualizarDocumento.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class PrevisualizarDocumento : System.Web.UI.Page
    {

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var IDENTEMI = Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "";
            var txtInvoice = Session["PREVIEW_PDF_INVOICE"] != null ? Session["PREVIEW_PDF_INVOICE"].ToString() : "";
            if (!string.IsNullOrEmpty(IDENTEMI))
            {
                if (!string.IsNullOrEmpty(txtInvoice))
                {
                    var mode = Request.QueryString.Get("mode");
                    var header = "";
                    switch (mode)
                    {
                        case "download":
                            header = "attachment";
                            break;
                        default:
                            header = "inline";
                            break;
                    }
                    var coreMx = new WsEmision { Timeout = (1800 * 1000) };
                    var respuesta = coreMx.PreviewPdf(Session["IDENTEMI"].ToString(), txtInvoice, "01");
                    if (respuesta != null)
                    {
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", header + ";filename=preview.pdf");
                        Response.BufferOutput = true;
                        Response.AddHeader("Content-Length", respuesta.Length.ToString());
                        Response.BinaryWrite(respuesta);
                        Response.End();
                    }
                    else
                    {
                        var mensaje = coreMx.ObtenerMensaje();
                        (Master as SiteMaster).MostrarAlerta(this, mensaje + "." + "<br/><br/>Al presionar el botón de 'Cerrar' se le regresará a la página anterior", 4, null, "history.go(-1);");
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Se debe indicar un ID de factura para generar el PDF en tiempo real." + "<br/><br/>Al presionar el botón de 'Cerrar' se le regresará a la página anterior", 4, null, "history.go(-1);");
                }
            }
        }
    }
}