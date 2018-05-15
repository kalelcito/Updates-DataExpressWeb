// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="SubirArchivos.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using AjaxControlToolkit;
using Control;
using DataExpressWeb.wsRecepcion;
using Datos;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace DataExpressWeb.recepcion
{
    /// <summary>
    /// Class SubirArchivos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class SubirArchivos : Page
    {
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log;
        /// <summary>
        /// The _dbe
        /// </summary>
        private BasesDatos _dbe;

        /// <summary>
        /// The _DBR
        /// </summary>
        private BasesDatos _dbr;
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                _dbe = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                _dbr = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE", "Recepcion");
                _log = new Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                }
                Session["_isWorkflow"] = IsWorkflow(); //Usage: Convert.ToBoolean(Session["_isWorkflow"])
                if (!IsPostBack)
                {
                    Session["_files"] = new Dictionary<int, object[]>();

                }
                else
                {
                    CreateControls();
                }
            }
        }

        private bool IsWorkflow()
        {
            var result = true;
            try
            {
                _dbr.Conectar();
                _dbr.CrearComando("SELECT idGrupo FROM Cat_Grupos_validadores");
                var dr = _dbr.EjecutarConsulta();
                result = dr.Read();
            }
            catch (Exception ex)
            {
                // ignored
            }
            finally
            {
                _dbr.Desconectar();
            }
            return result;
        }

        /// <summary>
        /// Creates the controls.
        /// </summary>
        private void CreateControls()
        {
            var items = 0;
            int.TryParse(tbComprobantes.Text, out items);

            panelComprobantes.Controls.Clear();


            for (var i = 1; i <= items; i++)
            {
                if (!((Dictionary<int, object[]>)Session["_files"]).ContainsKey(i))
                {
                    ((Dictionary<int, object[]>)Session["_files"]).Add(i, new object[] { null, null, null, null });
                }

                #region Definicion de Panel

                var content = new HtmlGenericControl("div");
                content.ID = "panelComprobante" + i;
                content.Attributes["class"] = "panel panel-primary";
                content.Style["background-color"] = "rgba(245, 245, 245, 0)";

                var heading = new HtmlGenericControl("div");
                SqlDataSourceTipoProveedor.ConnectionString = _dbr.CadenaConexion;
                var ddlTipoProveedor = new System.Web.UI.WebControls.DropDownList
                {
                    CssClass = "form-control",
                    DataSourceID = "SqlDataSourceTipoProveedor",
                    DataValueField = "IdTipo",
                    DataTextField = "nombre",
                    ID = "ddlTipoProveedor" + i
                };
                ddlTipoProveedor.Attributes.Add("style", "text-align:center;display:" + (Convert.ToBoolean(Session["_isWorkflow"]) ? "inline" : "none"));
                ddlTipoProveedor.ClientIDMode = ClientIDMode.Static;
                ddlTipoProveedor.Visible = false;
                //ddlTipoProveedor.Visible = Convert.ToBoolean(Session["_isWorkflow"]);

                var tbObservaciones = new System.Web.UI.WebControls.TextBox
                {
                    CssClass = "form-control",
                    ID = "tbObservaciones" + i
                };
                tbObservaciones.Attributes.Add("style", "text-align:center;");
                tbObservaciones.Attributes.Add("placeholder", "OBSERVACIONES OPCIONALES PARA EL COMPROBANTE " + i);
                tbObservaciones.ClientIDMode = ClientIDMode.Static;

                heading.Attributes["class"] = "panel panel-heading";
                var innerRow1 = new HtmlGenericControl("div");
                innerRow1.Attributes["class"] = "row panel-title";
                var innerCol1 = new HtmlGenericControl("div");
                innerCol1.Attributes["class"] = "col-md-6";
                innerCol1.Attributes["style"] = "text-align: left;";
                var innerCol2 = new HtmlGenericControl("div");
                innerCol2.Attributes["class"] = "col-md-2";
                var innerCol3 = new HtmlGenericControl("div");
                innerCol3.Attributes["class"] = "col-md-4";
                var h3 = new System.Web.UI.HtmlControls.HtmlGenericControl("span");
                h3.InnerText = "Comprobante " + i + "";
                var span3 = new System.Web.UI.HtmlControls.HtmlGenericControl("span");
                span3.InnerText = Convert.ToBoolean(Session["_isWorkflow"]) ? "Tipo de Proveedor: " : "";
                span3.Visible = false;
                var innerRow2 = new HtmlGenericControl("div");
                innerRow2.Attributes["class"] = "row";
                var innerCol1_2 = new HtmlGenericControl("div");
                innerCol1_2.Attributes["class"] = "col-md-12";
                innerCol1_2.Attributes["style"] = "text-align: center;";
                innerCol1_2.Controls.Add(tbObservaciones);
                innerCol1.Controls.Add(h3);
                innerCol2.Controls.Add(span3);
                innerCol2.Visible = Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5");
                innerCol3.Controls.Add(ddlTipoProveedor);
                innerCol3.Visible = Session["IDENTEMI"].ToString().Equals("HIM890120VEA") || Session["IDENTEMI"].ToString().Equals("SIH071204N90") || Session["IDENTEMI"].ToString().Equals("LAN7008173R5");
                innerRow1.Controls.Add(innerCol1);
                innerRow1.Controls.Add(innerCol2);
                innerRow1.Controls.Add(innerCol3);
                innerRow2.Controls.Add(innerCol1_2);
                heading.Controls.Add(innerRow1);
                heading.Controls.Add(innerRow2);

                var body = new HtmlGenericControl("div");
                body.Attributes["class"] = "panel-body";

                #endregion
                #region Division de rows

                var rowFilesFixed = new HtmlGenericControl("div");
                rowFilesFixed.ID = "rowFixed" + i;
                rowFilesFixed.Attributes["class"] = "row";

                var emptyRow = new HtmlGenericControl("div");
                emptyRow.Attributes["class"] = "row";
                emptyRow.InnerHtml = "&nbsp;";

                var rowDropZone = new HtmlGenericControl("div");
                rowDropZone.ID = "rowDropZone" + i;
                rowDropZone.Attributes["class"] = "row";

                var colFilesFixed = new HtmlGenericControl("div");
                colFilesFixed.ID = "colFixed" + i;
                colFilesFixed.Attributes["class"] = "col-md-12";

                var colDropzone = new HtmlGenericControl("div");
                colDropzone.ID = "colDropzone" + i;
                colDropzone.Attributes["class"] = "col-md-12";

                var collapseDropZone = new HtmlGenericControl("div");
                collapseDropZone.ID = "collapseDropZone" + i;
                collapseDropZone.Attributes["class"] = "collapse";
                collapseDropZone.ClientIDMode = ClientIDMode.Static;

                var rowBtnCollapse = new HtmlGenericControl("div");
                rowBtnCollapse.ID = "rowBtnCollapse" + i;
                rowBtnCollapse.InnerHtml = "<button id='bCollapseAdicionales' runat='server' class='btn btn-primary btn-xs' type='button' data-toggle='collapse' data-target='#" + collapseDropZone.ClientID + "' aria-expanded='false' aria-controls='" + collapseDropZone.ClientID + "' visible='false'><span class='glyphicon glyphicon-plus-sign' aria-hidden='true'></span>&nbsp;Archivos Adicionales</button>";

                #endregion
                #region Titulos

                var rowTitulos = new HtmlGenericControl("div");
                rowTitulos.ID = "rowTitulo" + i;
                rowTitulos.Attributes["class"] = "row";

                var colTitulo1 = new HtmlGenericControl("div");
                colTitulo1.ID = "colTitulo" + i + "_1";
                colTitulo1.Attributes["class"] = "col-md-4";
                colTitulo1.InnerHtml = "ARCHIVO XML *:";

                var colTitulo2 = new HtmlGenericControl("div");
                colTitulo2.ID = "colTitulo" + i + "_2";
                colTitulo2.Attributes["class"] = "col-md-4";
                colTitulo2.InnerHtml = "ARCHIVO PDF *:";

                var colTitulo3 = new HtmlGenericControl("div");
                colTitulo3.ID = "colTitulo" + i + "_3";
                colTitulo3.Attributes["class"] = "col-md-4";
                colTitulo3.InnerHtml = "ORDEN DE COMPRA (OPCIONAL):";

                #endregion
                #region Columnas

                var rowFields = new HtmlGenericControl("div");
                rowFields.ID = "rowComprobante" + i;
                rowFields.Attributes["class"] = "row";

                var colXml = new HtmlGenericControl("div");
                colXml.ID = "colXML" + i;
                colXml.Attributes["class"] = "col-md-4";

                var colPdf = new HtmlGenericControl("div");
                colPdf.ID = "colPDF" + i;
                colPdf.Attributes["class"] = "col-md-4";

                var colOrden = new HtmlGenericControl("div");
                colOrden.ID = "colOrden" + i;
                colOrden.Attributes["class"] = "col-md-4";

                #endregion
                #region Componentes

                var xml = new AsyncFileUpload();
                xml.ID = "xml_" + i;
                xml.Style["text-align"] = "center";
                xml.UploadedComplete += AsyncFileUpload_UploadedComplete;
                xml.OnClientUploadStarted = "xmlUpload";
                xml.OnClientUploadError = "UploadError";

                var pdf = new AsyncFileUpload();
                pdf.ID = "pdf_" + i;
                pdf.Style["text-align"] = "center";
                pdf.UploadedComplete += AsyncFileUpload_UploadedComplete;
                pdf.OnClientUploadStarted = "pdfUpload";
                pdf.OnClientUploadError = "UploadError";

                var orden = new AsyncFileUpload();
                orden.ID = "orden_" + i;
                orden.Style["text-align"] = "center";
                orden.UploadedComplete += AsyncFileUpload_UploadedComplete;
                orden.OnClientUploadStarted = "ordenUpload";
                orden.OnClientUploadError = "UploadError";

                var adicionales = new HtmlGenericControl("div");
                adicionales.ID = "adicionales_" + i;
                adicionales.Attributes["class"] = "dropzone";
                adicionales.Attributes["maxFiles"] = "10";
                adicionales.Attributes["maxFileSizeMb"] = "10";
                adicionales.Attributes["extraParam"] = i.ToString();
                adicionales.Attributes["idBlockedButton"] = lbSubir.ClientID;
                adicionales.Attributes["message"] = "<button type='button' class='btn btn-primary' data-toggle='tooltip' data-placement='bottom' title='Haga click o arrastre/suelte aquí para agregar archivos adicionales'><i class='fa fa-plus-square-o' aria-hidden='true'></i> Agregar</button>";
                var url = ResolveClientUrl("~/recepcion/DropzoneHandler.ashx");
                var urlAjax = ResolveClientUrl("~/recepcion/SubirArchivos.aspx") + "/DropZone_UploadedComplete";
                adicionales.Attributes["url"] = url;
                adicionales.Attributes["urlAjax"] = urlAjax;

                #endregion
                #region Creacion de layout

                rowTitulos.Controls.Add(colTitulo1);
                rowTitulos.Controls.Add(colTitulo2);
                rowTitulos.Controls.Add(colTitulo3);

                colXml.Controls.Add(xml);
                colPdf.Controls.Add(pdf);
                colOrden.Controls.Add(orden);

                rowFields.Controls.Add(colXml);
                rowFields.Controls.Add(colPdf);
                rowFields.Controls.Add(colOrden);

                colFilesFixed.Controls.Add(rowTitulos);
                colFilesFixed.Controls.Add(rowFields);

                collapseDropZone.Controls.Add(adicionales);
                colDropzone.Controls.Add(rowBtnCollapse);
                colDropzone.Controls.Add(collapseDropZone);

                rowFilesFixed.Controls.Add(colFilesFixed);
                rowDropZone.Controls.Add(colDropzone);

                body.Controls.Add(rowFilesFixed);
                body.Controls.Add(emptyRow);
                body.Controls.Add(rowDropZone);

                content.Controls.Add(heading);
                content.Controls.Add(body);
                panelComprobantes.Controls.Add(content);

                #endregion
            }
            lbSubir.Visible = (items > 0);
        }

        /// <summary>
        /// Handles the TextChanged event of the tbComprobantes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbComprobantes_TextChanged(object sender, EventArgs e)
        {
            ((Dictionary<int, object[]>)Session["_files"]).Clear();
        }

        /// <summary>
        /// Drops the zone uploaded complete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        [System.Web.Services.WebMethod]
        public static string DropZone_UploadedComplete(string id, string data)
        {
            var jsReturned = "";
            try
            {
                var intId = 0;
                int.TryParse(id, out intId);
                var fileArray = ((Dictionary<int, object[]>)HttpContext.Current.Session["_files"])[intId];
                if (intId > 0)
                {
                    var array = new List<object[]>();
                    var fileData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string[]>>(data);
                    for (int i = 0; i < fileData.Count; i++)
                    {
                        var file = fileData[i];
                        var fileName = file[0];
                        var fileSplit = Regex.Split(file[1], @";base64,");
                        var dataType = "";
                        try { dataType = fileSplit[0].Split(':')[1]; } catch { }
                        var fileBase64 = fileSplit[1];
                        var fileBytes = ControlUtilities.DecodeBase64ToFile(fileBase64);
                        array.Add(new object[] { fileBytes, dataType, fileName });
                    }
                    fileArray[3] = array;
                }
            }
            catch (Exception ex)
            {
                jsReturned = "alertBootBox('No se pudieron cargar los archivos, intente quitarlos y volverlos a cargar, de lo contrario no serán registrados<br/><br/>" + ex.Message + "', 4);";
            }
            return jsReturned;
        }

        /// <summary>
        /// Handles the UploadedComplete event of the AsyncFileUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AsyncFileUploadEventArgs" /> instance containing the event data.</param>
        protected void AsyncFileUpload_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            var control = (AsyncFileUpload)sender;
            var id = control.ID;
            var idSplitted = id.Split(new[] { '_' }, 2);
            var type = idSplitted.First();
            var count = idSplitted.Last();
            var i = 0;
            int.TryParse(count, out i);
            if (i > 0 && ((Dictionary<int, object[]>)Session["_files"]).ContainsKey(i))
            {
                if (type.Equals("xml", StringComparison.OrdinalIgnoreCase))
                {
                    ((Dictionary<int, object[]>)Session["_files"])[i][0] = control.PostedFile;
                }
                else if (type.Equals("pdf", StringComparison.OrdinalIgnoreCase))
                {
                    ((Dictionary<int, object[]>)Session["_files"])[i][1] = control.PostedFile;
                }
                else if (type.Equals("orden", StringComparison.OrdinalIgnoreCase))
                {
                    ((Dictionary<int, object[]>)Session["_files"])[i][2] = control.PostedFile;
                }
            }
        }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="pClientID">The p client identifier.</param>
        /// <param name="regex">if set to <c>true</c> [regex].</param>
        /// <returns>System.Web.UI.Control.</returns>
        private System.Web.UI.Control getControl(System.Web.UI.Control root, string pClientID, bool regex = false)
        {
            var expression = false;
            var rootId = root.ClientID;
            if (regex)
            {
                expression = (Regex.IsMatch(rootId, ".*" + pClientID + ".*", RegexOptions.IgnoreCase));
            }
            else
            {
                expression = (rootId == pClientID);
            }
            if (expression)
                return root;
            foreach (System.Web.UI.Control c in root.Controls)
                using (System.Web.UI.Control subc = getControl(c, pClientID, regex))
                    if (subc != null)
                        return subc;
            return null;
        }

        /// <summary>
        /// Handles the Click event of the lbSubir control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">
        /// El comprobante no pudo generarse: El RFC receptor del comprobante (" + rfcReceptorXml + ") no se encuentra registrado en el portal.
        /// or
        /// El comprobante no pudo generarse: El RFC emisor del comprobante (" + rfcEmisorXml + ") es distinto al RFC del proveedor actual (" + rfcWeb + ")
        /// or
        /// No se pudo guardar el PDF en la ruta especificada. Causa: " + ex.Message
        /// or
        /// No existe ruta de PDF asignada para el comprobante.
        /// or
        /// El comprobante no pudo generarse: " + ws.ObtenerMensaje() + ". Para más información consulte la trama con ID " + id
        /// or
        /// El comprobante no pudo generarse: " + ws.ObtenerMensaje()
        /// </exception>
        /// <exception cref="System.Exception">No se pudo guardar el PDF en la ruta especificada. Causa:  + ex.Message
        /// or
        /// No existe ruta de PDF asignada para el comprobante.
        /// or
        /// El comprobante no pudo generarse:  + ws.ObtenerMensaje() + . Para más información consulte la trama con ID  + id
        /// or
        /// El comprobante no pudo generarse:  + ws.ObtenerMensaje()</exception>
        protected void lbSubir_Click(object sender, EventArgs e)
        {
            //[0] = XML
            //[1] = PDF
            //[2] = ORDEN_COMPRA
            //[3] = ADICIONALES
            var _recepBcc = "";
            if (Session["_files"] == null || ((Dictionary<int, object[]>)Session["_files"]).Count <= 0)
            {
                var msg = "No se han cargado comprobantes";
                var metodo = MethodBase.GetCurrentMethod().Name;
                RegLog(msg, metodo);
                (Master as SiteMaster).MostrarAlerta(this, msg, 4, null);
            }
            else
            {
                var comprobantesFallidos = ((Dictionary<int, object[]>)Session["_files"]).Where(x => x.Value[0] == null || x.Value[1] == null).Select(x => x.Key).ToList();
                var msg = "";
                if (comprobantesFallidos.Count == 1)
                {
                    msg = "El comprobante " + comprobantesFallidos.FirstOrDefault() + " no tiene alguno de los archivos requeridos para ser procesado.";
                    var metodo = MethodBase.GetCurrentMethod().Name;
                    RegLog(msg, metodo);
                    (Master as SiteMaster).MostrarAlerta(this, msg, 4, null);
                }
                else if (comprobantesFallidos.Count > 1)
                {
                    var compFallidos = string.Join(", ", comprobantesFallidos.ToArray());
                    msg = "Los comprobantes " + compFallidos + " no tienen alguno de los archivos requeridos para ser procesados.";
                    var metodo = MethodBase.GetCurrentMethod().Name;
                    RegLog(msg, metodo);
                    (Master as SiteMaster).MostrarAlerta(this, msg, 4, null);
                }
                else
                {
                    var mensajeFallidos = new List<KeyValuePair<int, string>>();
                    foreach (var comprobante in ((Dictionary<int, object[]>)Session["_files"]))
                    {
                        try
                        {
                            var xml = PostedFileToBytes((HttpPostedFile)comprobante.Value[0]);
                            var pdf = (HttpPostedFile)comprobante.Value[1];
                            var orden = (HttpPostedFile)comprobante.Value[2];
                            var bytesPdf = PostedFileToBytes(pdf);
                            var bytesOrden = PostedFileToBytes(orden);
                            var xDoc = GetEntryXmlDoc(xml);
                            var archivosAdicionales = comprobante.Value[3];
                            var ws = new WsRecepcion { Timeout = (1800 * 1000) };
                            //var ddl = FindControl("ddlTipoProveedor" + comprobante.Key);
                            System.Web.UI.WebControls.DropDownList ddlTipoProveedor = null;
                            var idTipoProveedor = "";
                            var observaciones = "";
                            try
                            {
                                var ddl = (System.Web.UI.WebControls.DropDownList)getControl(Page, "ddlTipoProveedor" + comprobante.Key);
                                idTipoProveedor = ddl.SelectedValue;
                            }
                            catch (Exception ex) { }
                            try
                            {
                                var tb = (System.Web.UI.WebControls.TextBox)getControl(Page, "tbObservaciones" + comprobante.Key);
                                observaciones = tb.Text;
                            }
                            catch (Exception ex) { }

                            DbDataReader dr;

                            var idUserWs = _idUser;
                            XmlNode xmlNode;
                            if (Session["USERNAME"].ToString().StartsWith("PROVE", StringComparison.OrdinalIgnoreCase))
                            {
                                idUserWs = "999999999";
                                #region Validacion RFC Receptor

                                var rfcReceptorXml = "";
                                xmlNode = xDoc.GetElementsByTagName("cfdi:Receptor").Item(0);
                                if (xmlNode?.Attributes != null)
                                {
                                    try
                                    {
                                        rfcReceptorXml = xmlNode.Attributes["rfc"].Value;
                                    }
                                    catch { }
                                    if (string.IsNullOrEmpty(rfcReceptorXml))
                                    {
                                        try
                                        {
                                            rfcReceptorXml = xmlNode.Attributes["Rfc"].Value;
                                        }
                                        catch { }
                                    }
                                }

                                _dbe.Conectar();
                                _dbe.CrearComando("SELECT IDEEMI FROM Cat_Emisor WHERE RFCEMI = @rfc");
                                _dbe.AsignarParametroCadena("@rfc", rfcReceptorXml);
                                dr = _dbe.EjecutarConsulta();
                                if (!dr.Read())
                                {
                                    _dbe.Desconectar();
                                    throw new Exception("El comprobante no pudo generarse: El RFC receptor del comprobante (" + rfcReceptorXml + ") no se encuentra registrado en el portal.");
                                }
                                _dbe.Desconectar();

                                #endregion
                                #region Validacion RFC Proveedor

                                var rfcWeb = "";
                                _dbe.Conectar();
                                _dbe.CrearComando("SELECT id_Receptor FROM Cat_Proveedores WHERE userProveedor = @user");
                                _dbe.AsignarParametroCadena("@user", Session["USERNAME"].ToString());
                                dr = _dbe.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    rfcWeb = dr["id_Receptor"].ToString();
                                    _dbe.Desconectar();
                                    var rfcEmisorXml = "";
                                    xmlNode = xDoc.GetElementsByTagName("cfdi:Emisor").Item(0);
                                    if (xmlNode?.Attributes != null)
                                    {
                                        try
                                        {
                                            rfcEmisorXml = xmlNode.Attributes["rfc"].Value;
                                        }
                                        catch { }
                                        if (string.IsNullOrEmpty(rfcEmisorXml))
                                        {
                                            try
                                            {
                                                rfcEmisorXml = xmlNode.Attributes["Rfc"].Value;
                                            }
                                            catch { }
                                        }
                                    }
                                    if (!rfcEmisorXml.Equals(rfcWeb, StringComparison.OrdinalIgnoreCase))
                                    {
                                        throw new Exception("El comprobante no pudo generarse: El RFC emisor del comprobante (" + rfcEmisorXml + ") es distinto al RFC del proveedor actual (" + rfcWeb + ")");
                                    }
                                }
                                _dbe.Desconectar();

                                #endregion
                            }
                            #region Validacion TipoDeComprobante != Pago

                            var tipoComprobante = "";
                            xmlNode = xDoc.DocumentElement;
                            if (xmlNode?.Attributes != null)
                            {
                                try
                                {
                                    tipoComprobante = xmlNode.Attributes["tipoDeComprobante"].Value;
                                }
                                catch { }
                                if (string.IsNullOrEmpty(tipoComprobante))
                                {
                                    try
                                    {
                                        tipoComprobante = xmlNode.Attributes["TipoDeComprobante"].Value;
                                    }
                                    catch { }
                                }
                            }
                            if (tipoComprobante.Equals("P", StringComparison.OrdinalIgnoreCase) || tipoComprobante.Equals("Pago", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new Exception("El comprobante es de pago, para subir complementos utilice la página de Consulta de Documentos");
                            }

                            #endregion
                            var inner = xDoc.OuterXml;
                            var base64 = ControlUtilities.EncodeStringToBase64(inner);
                            var response = ws.RecibeComprobante(base64, idUserWs, Session["IDENTEMI"].ToString(), true, pdf != null, bytesOrden != null ? Path.GetFileName(orden.FileName) : "");
                            if (response != null && response[0] != null && response[1] != null)
                            {
                                var id = response[0].ToString();
                                var estado = false;
                                bool.TryParse(response[1].ToString(), out estado);
                                if (estado)
                                {
                                    var serverUrlPdf = "";
                                    var serverUrlOrden = "";
                                    if (pdf != null || orden != null)
                                    {
                                        var sql = "";


                                        sql = "SELECT TOP 1 dirdocs, emailRecepBcc FROM Par_ParametrosSistema ORDER BY idparametro DESC";
                                        _dbe.Conectar();
                                        _dbe.CrearComando(sql);
                                        dr = _dbe.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            var rootDir = dr["dirdocs"].ToString();
                                            _recepBcc = dr["emailRecepBcc"].ToString().Trim();
                                            rootDir = rootDir.Replace("docus", "");
                                            _dbe.Desconectar();
                                            sql = @"SELECT PDFARC, ORDENARC FROM Dat_Archivos WHERE IDEFAC = @ID";
                                            _dbr.Conectar();
                                            _dbr.CrearComando(sql);
                                            _dbr.AsignarParametroCadena("@ID", id);
                                            dr = _dbr.EjecutarConsulta();
                                            if (dr.Read())
                                            {
                                                try
                                                {
                                                    serverUrlPdf = rootDir + @"\" + (dr["PDFARC"].ToString());//Server.MapPath("~/" + dr["PDFARC"]);
                                                    serverUrlOrden = rootDir + @"\" + (dr["ORDENARC"].ToString());//Server.MapPath("~/" + dr["ORDENARC"]);
                                                    serverUrlPdf = Path.GetFullPath(serverUrlPdf);
                                                    serverUrlOrden = Path.GetFullPath(serverUrlOrden);
                                                    try
                                                    {
                                                        File.WriteAllBytes(serverUrlPdf, bytesPdf);
                                                    }
                                                    catch
                                                    {
                                                        if (pdf != null) { pdf.SaveAs(serverUrlPdf); }
                                                    }
                                                    try
                                                    {
                                                        File.WriteAllBytes(serverUrlOrden, bytesOrden);
                                                    }
                                                    catch
                                                    {
                                                        if (orden != null) { orden.SaveAs(serverUrlOrden); }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw new Exception("No se pudo guardar el PDF en la ruta especificada. Causa: " + ex.Message);
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("No existe ruta de PDF asignada para el comprobante.");
                                            }
                                            _dbr.Desconectar();
                                        }
                                    }
                                    if (archivosAdicionales != null)
                                    {
                                        var listAdicionales = (List<object[]>)archivosAdicionales;
                                        _dbe.Conectar();
                                        _dbe.CrearComando(@"SELECT [dirdocs] FROM [Par_ParametrosSistema]");
                                        var _varSql = _dbe.Comando.CommandText;
                                        dr = _dbe.EjecutarConsulta();
                                        var _rutaDoc = "";

                                        while (dr.Read())
                                        {
                                            _rutaDoc = dr["dirdocs"].ToString().Trim();
                                        }
                                        _dbe.Desconectar();
                                        var rutaRuc = "recepcion/" + (_rutaDoc.Contains(Session["IDENTEMI"].ToString()) ? Session["IDENTEMI"].ToString() : "") + "/";
                                        var subcarpeta = Control.Localization.Now.ToString("yyyy/MM/dd");
                                        subcarpeta = (_rutaDoc + rutaRuc + subcarpeta + "/ADICIONALES/" + id + "/").Replace("/", @"\");
                                        Directory.CreateDirectory(subcarpeta);
                                        foreach (var item in listAdicionales)
                                        {
                                            var bytes = (byte[])item[0];
                                            var dataType = (string)item[1];
                                            var fileName = (string)item[2];
                                            var filePath = subcarpeta + @"\" + fileName;
                                            var virtualPath = @"docus\" + filePath.Replace(_rutaDoc, "");
                                            File.WriteAllBytes(filePath, bytes);
                                            if (File.Exists(filePath))
                                            {
                                                _dbr.Conectar();
                                                _dbr.CrearComando("INSERT INTO Dat_ArchivosAdicionales (idComprobante,path,dataType) OUTPUT inserted.idArchivo VALUES (@idComprobante,@path,@dataType)");
                                                _dbr.AsignarParametroCadena("@idComprobante", id);
                                                _dbr.AsignarParametroCadena("@path", virtualPath);
                                                _dbr.AsignarParametroCadena("@dataType", dataType);
                                                dr = _dbr.EjecutarConsulta();
                                                if (dr.Read())
                                                {
                                                    // OK
                                                }
                                                _dbr.Desconectar();
                                            }
                                        }
                                    }
                                    if (response[2] != null)
                                    {
                                        try
                                        {
                                            var arrayMail = (object[])response[2];

                                            var _servidor = (string)arrayMail[0];
                                            var _puerto = (int)arrayMail[1];
                                            var _ssl = (bool)arrayMail[2];
                                            var _emailCredencial = (string)arrayMail[3];
                                            var _passCredencial = (string)arrayMail[4];
                                            var _emailEnviar = (string)arrayMail[5];
                                            var emails = (string)arrayMail[6];
                                            var _bcc = (string)arrayMail[7];
                                            var _cc = (string)arrayMail[8]; // _recepBcc;
                                            if (string.IsNullOrEmpty(_cc)) { _cc = _recepBcc; }
                                            else { _cc += "," + _recepBcc; }
                                            var asunto = (string)arrayMail[9];
                                            var mensaje = (string)arrayMail[10];

                                            var _em = new SendMail();
                                            _em.ServidorSmtp(_servidor, _puerto, _ssl, _emailCredencial, _passCredencial);
                                            _em.LlenarEmail(_emailEnviar, emails, _bcc, _cc, asunto, mensaje);
                                            _em.Adjuntar(xml, ((HttpPostedFile)comprobante.Value[0]).FileName);
                                            _em.Adjuntar(bytesPdf, Path.GetFileName(serverUrlPdf));
                                            _em.Adjuntar(bytesOrden, Path.GetFileName(serverUrlOrden));
                                            if (archivosAdicionales != null)
                                            {
                                                var listAdicionales = (List<object[]>)archivosAdicionales;
                                                listAdicionales.ForEach(x => _em.Adjuntar((byte[])x[0], (string)x[2]));
                                            }
                                            _em.EnviarEmail();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    if (!string.IsNullOrEmpty(idTipoProveedor))
                                    {
                                        try
                                        {
                                            _dbr.Conectar();
                                            _dbr.CrearComando("UPDATE Dat_General SET id_TipoProveedor = @idProv WHERE idComprobante = @id");
                                            _dbr.AsignarParametroCadena("@idProv", idTipoProveedor);
                                            _dbr.AsignarParametroCadena("@id", id);
                                            _dbr.EjecutarConsulta1();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        finally
                                        {
                                            _dbr.Desconectar();
                                        }
                                    }
                                    try
                                    {
                                        _dbr.Conectar();
                                        _dbr.CrearComando("UPDATE Dat_General SET observaciones = @observaciones WHERE idComprobante = @id");
                                        _dbr.AsignarParametroCadena("@observaciones", observaciones);
                                        _dbr.AsignarParametroCadena("@id", id);
                                        _dbr.EjecutarConsulta1();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    finally
                                    {
                                        _dbr.Desconectar();
                                    }
                                }
                                else
                                {
                                    throw new Exception("El comprobante no pudo generarse: " + ws.ObtenerMensaje() + ". Para más información consulte la trama con ID " + id);
                                }
                            }
                            else
                            {
                                throw new Exception("El comprobante no pudo generarse: " + ws.ObtenerMensaje());
                            }
                        }
                        catch (Exception ex)
                        {
                            mensajeFallidos.Add(new KeyValuePair<int, string>(comprobante.Key, ex.Message));
                        }
                    }
                    if (mensajeFallidos.Count > 0)
                    {
                        var html = "<ul>";
                        foreach (var item in mensajeFallidos)
                        {
                            html += "<li><b>" + item.Key + "</b>: " + item.Value + "</li>";
                        }
                        html += "</ul>";
                        (Master as SiteMaster).MostrarAlerta(this, "Ocurrió uno o más errores al subir los comprobantes:<br>" + html, 4, null);
                    }
                    else
                    {
                        var urlDocus = "~/recepcion/Documentos.aspx";
                        (Master as SiteMaster).MostrarAlerta(this, "Todos los comprobantes se generaron con éxito", 2, null, "window.location.href = '" + ResolveClientUrl(urlDocus) + "';");
                    }
                    Session["_files"] = new Dictionary<int, object[]>();
                    tbComprobantes.Text = "0";
                    CreateControls();
                }
            }
        }

        /// <summary>
        /// Regs the log.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <param name="metodoActual">The metodo actual.</param>
        /// <param name="mensajeTecnico">The mensaje tecnico.</param>
        private void RegLog(string mensaje, string metodoActual, string mensajeTecnico = "")
        {
            _log.Registrar(mensaje, GetType().Name, metodoActual, null, mensajeTecnico, null, null, _idUser, Session["IDENTEMI"].ToString());
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
        /// Gets the entry XML document.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>XmlDocument.</returns>
        private XmlDocument GetEntryXmlDoc(byte[] bytes)
        {
            var xmlDoc = new XmlDocument();
            using (var ms = new MemoryStream(bytes))
            {
                xmlDoc.Load(ms);
            }
            return xmlDoc;
        }
    }
}
