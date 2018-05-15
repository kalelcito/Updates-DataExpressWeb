// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 11-30-2016
// ***********************************************************************
// <copyright file="print.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using Datos;
using System;
using System.IO;
using System.Web.UI;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Print.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Print : System.Web.UI.Page
    {
        //String sRuta = "";
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db = new BasesDatos();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            var idFactura = Request.QueryString.Get("idFactura");
            var printername = Request.QueryString.Get("printer");
            try
            {
                if (!string.IsNullOrEmpty(idFactura) && !string.IsNullOrEmpty(printername))
                {
                    if (Session["IDENTEMI"] != null)
                    {
                        var rutaDocus = "";
                        var pdf = "";
                        var rutaPdf = "";
                        _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                        _db.Conectar();
                        _db.CrearComando(@"select dirdocs from Par_ParametrosSistema");
                        var dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            rutaDocus = dr[0].ToString().Trim();
                        }
                        _db.Desconectar();
                        _db.Conectar();
                        _db.CrearComando(@"SELECT
	                                        PDFARC
                                        FROM
	                                        Dat_Archivos
                                        WHERE IDEFAC = @ID");
                        _db.AsignarParametroCadena("@ID", idFactura);
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            pdf = dr["PDFARC"].ToString();
                        }
                        _db.Desconectar();
                        pdf = pdf.Replace(@"docus\", "").Replace("docus//", "").Replace("docus/", "");
                        rutaPdf = rutaDocus + pdf;
                        rutaPdf = rutaPdf.Replace("/", @"\").Replace(@"\\", @"\").Replace("//", @"\");
                        if (pdf != null && (File.Exists(rutaPdf)))
                        {
                            Response.Clear();
                            RawPrinterHelper.SendFileToPrinter(printername, rutaPdf);
                            ScriptManager.RegisterStartupScript(this, GetType(), "_goBack", "history.go(-1);", true);
                        }
                        else
                        {
                            var ws = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                            var respuesta = ws.GenerarPdf(Session["IDENTEMI"].ToString(), idFactura, "");
                            if (respuesta != null)
                            {
                                Response.Clear();
                                RawPrinterHelper.SendBytesToPrinter(printername, respuesta);
                                ScriptManager.RegisterStartupScript(this, GetType(), "_goBack", "history.go(-1);", true);
                            }
                            else
                            {
                                var mensaje = ws.ObtenerMensaje();
                                (Master as SiteMaster).MostrarAlerta(this, mensaje + "." + "<br/><br/>Al presionar el botón de 'Cerrar' se le regresará a la página anterior", 4, null, "history.go(-1);");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, ex.Message + "<br/><br/>Al presionar el botón de 'Cerrar' se le regresará a la página anterior", 4, null, "history.go(-1);");
            }
        }
    }
}