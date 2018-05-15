// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="download.aspx.cs" company="DataExpress Latinoamérica">
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
    /// Class FormularioWeb11.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class FormularioWeb11 : System.Web.UI.Page
    {
        //String sRuta = "";
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db = new BasesDatos();
        /// <summary>
        /// The reference URL
        /// </summary>
        private static string refUrl;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">
        /// El comprobante no existe o no se genero correctamente
        /// or
        /// El comprobante no existe o no se genero correctamente
        /// </exception>
        /// <exception cref="System.Exception">El comprobante no existe o no se genero correctamente
        /// or
        /// El comprobante no existe o no se genero correctamente</exception>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var urlRef = Request.UrlReferrer;
                if (urlRef == null)
                {
                    Response.Redirect("~/Seguridad.aspx", true);
                }
                var dlDir = "";//@"docus/";
                var filenameEncoded = Request.QueryString.Get("fileEncoded");
                var filename = Request.QueryString.Get("file");
                if (string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(filenameEncoded))
                {
                    filename = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(filenameEncoded));
                }
                var ext = Request.QueryString.Get("ext");
                divLost.Style["display"] = "none";
                refUrl = urlRef.ToString();
                string path = "";
                try
                {
                    if (!string.IsNullOrEmpty(filename))
                    {
                        var IDENTEMI = Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : (Session["IDENTEMIEXT"] != null ? Session["IDENTEMIEXT"].ToString() : "");
                        if (!string.IsNullOrEmpty(IDENTEMI))
                        {
                            string rutaDocus;
                            _db = new BasesDatos(IDENTEMI);
                            _db.Conectar();
                            _db.CrearComando(@"select dirdocs from Par_ParametrosSistema");
                            var dr = _db.EjecutarConsulta();
                            if (dr.Read())
                            {
                                rutaDocus = dr["dirdocs"].ToString().Trim();
                                path = rutaDocus + filename.Replace("docus/", "");
                                path = path.Replace("/", @"\").Replace(@"\\", @"\").Replace("//", @"\").Replace("docus/", "").Replace(@"\docus\", @"\");
                                if (rutaDocus.StartsWith(@"\")) { path = @"\" + path; }
                                if (!new FileInfo(path).Exists)
                                {
                                    path = Server.MapPath(dlDir + filename);
                                }
                            }
                            else
                            {
                                path = Server.MapPath(dlDir + filename);
                            }
                            _db.Desconectar();
                        }
                        else
                        {
                            path = Server.MapPath(dlDir + filename);
                        }
                        //Label1.Text = path;
                        var toDownload = new FileInfo(path);
                        var cleanName = ControlUtilities.CleanFileName(toDownload.Name);
                        if (toDownload.Exists)
                        {
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + cleanName); // + ext
                            Response.AddHeader("Content-Length", toDownload.Length.ToString());
                            Response.ContentType = "application/octet-stream";
                            Response.WriteFile(path);//dlDir + filename);
                            Response.End();
                        }
                        else
                        {
                            throw new Exception("El comprobante no existe o no se genero correctamente");
                        }
                    }
                    else
                    {
                        throw new Exception("El comprobante no existe o no se genero correctamente");
                    }
                }
                catch (Exception ex)
                {
                    //(Master as SiteMaster).MostrarAlerta(this, ex.Message + "<br/><br/>Al presionar el botón de 'Cerrar' se le regresará a la página anterior", 4, null, "history.go(-1);");
                    lblPath.Text = path;
                    divLost.Style["display"] = "inline-block";
                }
            }
        }



        /// <summary>
        /// BTNs the error.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void BtnError(object sender, EventArgs e)
        {
            if (Session == null || Session["IDENTEMI"] == null)
            {
                Response.Redirect("~/Cerrar.aspx");
            }
            else
            {
                Response.Redirect(refUrl.ToString(), true);
            }
        }
    }
}