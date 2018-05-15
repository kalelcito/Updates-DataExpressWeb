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
using System;
using System.Text;
using System.Web.UI;

namespace DataExpressWeb
{
    /// <summary>
    /// Class VisualizarDocumento.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class VisualizarDocumento : System.Web.UI.Page
    {

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var IDENTEMI = Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : (Session["IDENTEMIEXT"] != null ? Session["IDENTEMIEXT"].ToString() : "");
            if (!string.IsNullOrEmpty(IDENTEMI))
            {
                var idFactura = Request.QueryString.Get("idFactura");
                var pago = Request.QueryString.Get("pago");
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
                if (!string.IsNullOrEmpty(idFactura))
                {
                    var uuid = Request.QueryString.Get("uuid");
                    var fileName = (!string.IsNullOrEmpty(uuid) ? uuid : "factura") + ".pdf";
                    var tipo = Request.QueryString.Get("tipo");
                    if (string.IsNullOrEmpty(tipo))
                    {
                        tipo = "E";
                    }
                    var ws = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                    byte[] respuesta;
                    if (tipo.Equals("C"))
                    {
                        respuesta = ws.GenerarPdfCancelacion(IDENTEMI, idFactura);
                    }
                    else
                    {
                        respuesta = ws.GenerarPdf(IDENTEMI, idFactura, "");
                    }
                    if (respuesta != null)
                    {
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", header + ";filename=" + fileName);
                        Response.BufferOutput = true;
                        Response.AddHeader("Content-Length", respuesta.Length.ToString());
                        Response.BinaryWrite(respuesta);
                        Response.End();
                    }
                    else
                    {
                        var mensaje = ws.ObtenerMensaje();
                        (Master as SiteMaster).MostrarAlerta(this, mensaje + "." + "<br/><br/>Al presionar el botón de 'Cerrar' se le regresará a la página anterior", 4, null, "history.go(-1);");
                    }
                }
                else if (!string.IsNullOrEmpty(pago))
                {
                    var dataPago = Encoding.Default.GetString(Convert.FromBase64String(pago)).Split(';');
                    var idPago = dataPago[0];
                    var idComprobante = dataPago[1];
                    var uuid = dataPago[2];
                    var fileName = (!string.IsNullOrEmpty(uuid) ? uuid : "pago") + ".pdf";
                    var ws = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                    byte[] respuesta;
                    respuesta = ws.GenerarPdfPagos(IDENTEMI, idPago, idComprobante);
                    if (respuesta != null)
                    {
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", header + ";filename=" + fileName);
                        Response.BufferOutput = true;
                        Response.AddHeader("Content-Length", respuesta.Length.ToString());
                        Response.BinaryWrite(respuesta);
                        Response.End();
                    }
                    else
                    {
                        var mensaje = ws.ObtenerMensaje();
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