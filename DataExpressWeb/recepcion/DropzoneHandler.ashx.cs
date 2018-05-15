// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 04-19-2017
//
// Last Modified By : Sergio
// Last Modified On : 04-19-2017
// ***********************************************************************
// <copyright file="DropzoneHandler.ashx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DataExpressWeb.recepcion
{
    /// <summary>
    /// Summary description for DropzoneHandler
    /// </summary>
    /// <seealso cref="System.Web.IHttpHandler" />
    public class DropzoneHandler : IHttpHandler
    {
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            var status = true;
            context.Response.ContentType = "text/plain";
            if (context.Request.Files.Count > 0)
            {
                try
                {

                    foreach (string s in context.Request.Files)
                    {
                        HttpPostedFile file = context.Request.Files[s];
                        var bytes = PostedFileToBytes(file);
                        int fileSizeInBytes = file.ContentLength;
                        string fileName = file.FileName;
                        string fileExtension = file.ContentType;
                        status &= !string.IsNullOrEmpty(fileName);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Status = "1 Error en solicitud";
                    context.Response.StatusCode = 1;
                    context.Response.StatusDescription = "No se pudo procesar uno o más archivos: " + ex.Message;
                    context.Response.End();
                }
                context.Response.Write(status.ToString().ToUpper());
            }
            else
            {
                context.Response.Status = "2 Error en solicitud";
                context.Response.StatusCode = 2;
                context.Response.StatusDescription = "No hay archivos a procesar";
                context.Response.End();
            }
        }

        /// <summary>
        /// Posteds the file to bytes.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.Byte[].</returns>
        private byte[] PostedFileToBytes(HttpPostedFile file)
        {
            byte[] result = null;
            try
            {
                var stream = file.InputStream;
                var output = new MemoryStream();
                stream.Position = 0;
                stream.CopyTo(output);
                result = output.ToArray();
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}