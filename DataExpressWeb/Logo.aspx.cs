// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="Logo.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using Datos;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Logo.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Logo : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var db = new BasesDatos("");

            //log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            try
            {
                byte[] img = new byte[0];

                //Recuperar el identificador
                var id = Request.QueryString["id"];

                if (Session["IDEMI"] != null && !string.IsNullOrEmpty(Session["IDEMI"].ToString()))
                {
                    db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                    db.Conectar();
                    db.CrearComando(@"SELECT logo FROM Cat_Emisor where IDEEMI='" + Session["IDEMI"] + "'");
                    var dr = db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        img = (byte[])dr["logo"];
                    }
                    db.Desconectar();
                    var str = new MemoryStream();
                    str.Write(img, 0, img.Length);
                    var bit = new Bitmap(str);
                    var imageFormat = bit.RawFormat;
                    Response.ContentType = imageFormat.GetMimeType(); //Responder Img JPG
                    bit.Save(Response.OutputStream, imageFormat);
                }
                //else if (Session["IDENTEMIEXT"] != null && !string.IsNullOrEmpty(Session["IDENTEMIEXT"].ToString()))
                //{
                //    db = new BasesDatos(Session["IDENTEMIEXT"] != null ? Session["IDENTEMIEXT"].ToString() : "CORE");
                //    db.Conectar();
                //    db.CrearComando(@"SELECT logo FROM Cat_Emisor where RFCEMI='" + Session["IDENTEMIEXT"] + "'");
                //    img = (byte[])db.EjecutarConsultaScar();
                //    db.Desconectar();
                //    var str = new MemoryStream();
                //    str.Write(img, 0, img.Length);
                //    var bit = new Bitmap(str);
                //    var imageFormat = bit.RawFormat;
                //    Response.ContentType = imageFormat.GetMimeType(); //Responder Img JPG
                //    bit.Save(Response.OutputStream, imageFormat);
                //}
                else
                {
                    byte[] datos = null;
                    var logoPath = AppDomain.CurrentDomain.BaseDirectory + @"imagenes/logo.";
                    if (File.Exists(logoPath + "png"))
                    {
                        datos = File.ReadAllBytes(logoPath + "png");
                    }
                    else if (File.Exists(logoPath + "jpg"))
                    {
                        datos = File.ReadAllBytes(logoPath + "jpg");
                    }
                    if (datos != null)
                    {
                        var str = new MemoryStream();
                        str.Write(datos, 0, datos.Length);
                        var bit = new Bitmap(str);
                        var imageFormat = bit.RawFormat;
                        Response.ContentType = imageFormat.GetMimeType(); //Responder Img JPG
                        bit.Save(Response.OutputStream, imageFormat);
                    }
                }
            }
            catch (Exception)
            {
                db.Desconectar();
            }
        }
    }
}