// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 07-02-2017
//
// Last Modified By : Sergio
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="FacturaGlobalOnQ.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using ClosedXML.Excel;
using Control;
using Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace DataExpressWeb.nuevo.factura
{
    /// <summary>
    /// Class FacturaGlobalOnQ.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class FacturaGlobalOnQ : System.Web.UI.Page
    {
        /// <summary>
        /// The database
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The log
        /// </summary>
        private Log _log;
        /// <summary>
        /// The identifier user
        /// </summary>
        private string _idUser;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                if (Session["errorTimbres"] != null && Convert.ToBoolean(Session["errorTimbres"]))
                {
                    Response.Redirect("~/CantidadTimbres.aspx", true);
                    return;
                }
                else if (Session["errorCertificado"] != null && Convert.ToBoolean(Session["errorCertificado"]))
                {
                    Response.Redirect("~/LicExpirada.aspx", true);
                    return;
                }
                _db = new BasesDatos(Session["IDENTEMI"].ToString());
                _log = new Log(Session["IDENTEMI"].ToString());

                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                    if (!Page.IsPostBack)
                    {
                        Session["_bytesCargados"] = null;
                        Session["_dtFacturas"] = null;
                    }
                }
            }
            else
            {
                Response.Redirect("~/Cerrar.aspx");
            }
        }

        /// <summary>
        /// Handles the UploadedComplete event of the fileOnq control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AjaxControlToolkit.AsyncFileUploadEventArgs"/> instance containing the event data.</param>
        protected void fileOnq_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Session["_bytesCargados"] = PostedFileToBytes(fileOnq.PostedFile);
            Session["_dtFacturas"] = null;
        }

        /// <summary>
        /// Gets the XML from session.
        /// </summary>
        /// <returns>XmlDocument.</returns>
        private XmlDocument GetXmlFromSession()
        {
            var bytesCargados = (byte[])Session["_bytesCargados"];
            var contentFile = Encoding.UTF8.GetString(bytesCargados);
            var xDoc = new XmlDocument();
            xDoc.LoadXml(contentFile);
            return xDoc;
        }

        /// <summary>
        /// Handles the Click event of the bCargarFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bCargarFile_Click(object sender, EventArgs e)
        {
            if (Session["_bytesCargados"] == null)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe escoger un archivo", 4);
                return;
            }
            try
            {
                var xDoc = GetXmlFromSession();
                var listaFolios = new List<RegistroXmlOnQMensual>();
                var nodos = xDoc.GetElementsByTagName("FormattedArea").Cast<XmlNode>().Where(x => x.Attributes["Type"] != null && x.Attributes["Type"].Value.Equals("Details")).ToList();
                for (int i = 0; i < nodos.Count; i++)
                {
                    var nodo = nodos[i];
                    var xNodo = new XmlDocument();
                    xNodo.LoadXml(nodo.OuterXml);
                    var listaFormattedReportObject = xNodo.GetElementsByTagName("FormattedReportObject").Cast<XmlNode>().Where(x => x.Attributes["FieldName"] != null && x.Attributes["FieldName"].Value.Equals("{cp_rpt_DPAUDDAA_1.conf_num}")).ToList();
                    var listaFormattedReportObjectFinal = listaFormattedReportObject.Select(x => x.ChildNodes.Cast<XmlNode>().Where(y => y.Name.Equals("FormattedValue")).FirstOrDefault()).ToList();
                    foreach (var child in listaFormattedReportObjectFinal)
                    {
                        if (string.IsNullOrEmpty(child.InnerText.Trim()))
                        {
                            var nodoHabitacion = xNodo.GetElementsByTagName("FormattedReportObject").Cast<XmlNode>().Where(x => x.Attributes["FieldName"] != null && x.Attributes["FieldName"].Value.Equals("{@roomhouse}")).ToList();
                            var habitacion = nodoHabitacion.Select(x => x.ChildNodes.Cast<XmlNode>().Where(y => y.Name.Equals("Value")).FirstOrDefault()).FirstOrDefault().InnerText;
                            listaFolios.Add(new RegistroXmlOnQMensual
                            {
                                Id = i,
                                NumeroConfirmacion = child.InnerText,
                                Habitacion = habitacion
                                //,Nodo = nodo
                            });
                        }
                        else if (!listaFolios.Any(x => x.NumeroConfirmacion.Equals(child.InnerText)))
                        {
                            var nodoHabitacion = xNodo.GetElementsByTagName("FormattedReportObject").Cast<XmlNode>().Where(x => x.Attributes["FieldName"] != null && x.Attributes["FieldName"].Value.Equals("{@roomhouse}")).ToList();
                            var habitacion = nodoHabitacion.Select(x => x.ChildNodes.Cast<XmlNode>().Where(y => y.Name.Equals("Value")).FirstOrDefault()).FirstOrDefault().InnerText;
                            listaFolios.Add(new RegistroXmlOnQMensual
                            {
                                Id = i,
                                NumeroConfirmacion = child.InnerText,
                                Habitacion = habitacion
                                //,Nodo = nodo
                            });
                        }
                    }
                }
                ConsultarBd(listaFolios);
                LoadFacturadas();
                divPanelMaster.Style["display"] = "inline";
            }
            catch (Exception ex)
            {
                _log.RegistrarTxt(ex.Message, "FacturaGlobalOnQ", "bCargarFile_Click", ex.ToString());
                divPanelMaster.Style["display"] = "none";
                (Master as SiteMaster).MostrarAlerta(this, "Intente subir nuevamente el archivo<br/><br/>" + ex.Message, 4);
            }
        }

        /// <summary>
        /// Loads the facturadas.
        /// </summary>
        private void LoadFacturadas()
        {
            var mixed = (List<RegistroXmlOnQMensual>)Session["_listaFacturados_FacturaGlobalOnQ"];
            mixed.AddRange((List<RegistroXmlOnQMensual>)Session["_listaNoFacturados_FacturaGlobalOnQ"]);
            var dt = ConvertToDataTable(mixed);
            lCountFacturas.Text = "Se encontraron <span class='badge'>" + dt.Rows.Count + "</span> registros";
            Session["_dtFacturas"] = dt;
            gvFacturas.DataSource = (DataTable)Session["_dtFacturas"];
            gvFacturas.DataBind();
        }

        /// <summary>
        /// Consultars the bd.
        /// </summary>
        /// <param name="listaFolios">The lista folios.</param>
        private void ConsultarBd(List<RegistroXmlOnQMensual> listaFolios)
        {
            var listaFacturados = new List<RegistroXmlOnQMensual>();
            var listaNoFacturados = new List<RegistroXmlOnQMensual>();
            try
            {
                listaNoFacturados.AddRange(listaFolios);
                for (int i = 0; i < listaFolios.Count; i++)
                {
                    var folioXml = listaFolios[i];
                    if (string.IsNullOrEmpty(folioXml.NumeroConfirmacion))
                    {
                        decimal totalXml = 0;
                        var xDoc = GetXmlFromSession();
                        var nodos = xDoc.GetElementsByTagName("FormattedArea").Cast<XmlNode>().Where(x => x.Attributes["Type"] != null && x.Attributes["Type"].Value.Equals("Details")).ToList();
                        for (int j = 0; j < nodos.Count; j++)
                        {
                            var nodo = nodos[j];
                            var xNodo = new XmlDocument();
                            xNodo.LoadXml(nodo.OuterXml);
                            var roomhouse = xNodo.GetElementsByTagName("FormattedReportObject").Cast<XmlNode>().Where(x => x.Attributes["FieldName"] != null && x.Attributes["FieldName"].Value.Equals("{@roomhouse}")).Select(x => x.ChildNodes.Cast<XmlNode>().Where(y => y.Name.Equals("Value")).FirstOrDefault()).FirstOrDefault().InnerText;
                            if (roomhouse.Equals(folioXml.Habitacion))
                            {
                                var listaPosted = xNodo.GetElementsByTagName("FormattedReportObject").Cast<XmlNode>().Where(x => x.Attributes["FieldName"] != null && x.Attributes["FieldName"].Value.Equals("{@posted}")).ToList();
                                var listaValues = listaPosted.Select(x => x.ChildNodes.Cast<XmlNode>().Where(y => y.Name.Equals("Value")).FirstOrDefault()).ToList();
                                foreach (var child in listaValues)
                                {
                                    if (!string.IsNullOrEmpty(child.InnerText.Trim()))
                                    {
                                        decimal childValue = 0;
                                        decimal.TryParse(child.InnerText.Replace("(", "").Trim(), out childValue);
                                        totalXml += childValue;
                                    }
                                }
                            }
                        }
                        folioXml.TotalConfirmacion = totalXml.ToString();
                        listaNoFacturados[i] = folioXml;
                        continue;
                    }
                    _db.Conectar();
                    _db.CrearComando("SELECT idComprobante, folioReservacion, numeroAutorizacion AS UUID, subTotal, total, IVA12 AS iva, impuestoHopedaje as ish, fecha, fechaAutorizacion, CONVERT(NVARCHAR, estado)+tipo AS estado, userEmpleado, propina, cargoxservicio AS otros, serie+CONVERT(NVARCHAR, folio) AS folioFactura FROM Dat_General INNER JOIN Cat_Empleados ON Dat_General.id_Empleado = Cat_Empleados.idEmpleado WHERE codDoc IN ('01','04') AND CONVERT(NVARCHAR, estado)+tipo IN ('1E','4E','2C','0C','2E') AND ISNULL(numeroAutorizacion, '') <> '' AND folioReservacion = @folioReservacion");
                    _db.AsignarParametroCadena("@folioReservacion", folioXml.NumeroConfirmacion);
                    var dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        var idComprobante = dr["idComprobante"].ToString();
                        var folio = dr["folioReservacion"].ToString();
                        var uuid = dr["UUID"].ToString();
                        var estado = dr["estado"].ToString();
                        var subTotal = dr["subTotal"].ToString();
                        var total = dr["total"].ToString();
                        var iva = dr["iva"].ToString();
                        var ish = dr["ish"].ToString();
                        var propina = dr["propina"].ToString();
                        var otros = dr["otros"].ToString();
                        var fecha = dr["fecha"].ToString();
                        var fechaAutorizacion = dr["fechaAutorizacion"].ToString();
                        var userEmpleado = dr["userEmpleado"].ToString();
                        var folioFactura = dr["folioFactura"].ToString();
                        folioXml.IdComprobante = idComprobante;
                        folioXml.Uuid = uuid;
                        folioXml.Estado = estado;
                        folioXml.SubTotal = subTotal;
                        folioXml.Total = total;
                        folioXml.FechaEmision = fecha;
                        folioXml.Fechatimbrado = fechaAutorizacion;
                        folioXml.Usuario = userEmpleado;
                        folioXml.Iva = iva;
                        folioXml.Ish = ish;
                        folioXml.Propinas = propina;
                        folioXml.Otros = otros;
                        folioXml.FolioFactura = folioFactura;
                        try
                        {
                            decimal totalXml = 0;
                            var xDoc = GetXmlFromSession();
                            var nodos = xDoc.GetElementsByTagName("FormattedArea").Cast<XmlNode>().Where(x => x.Attributes["Type"] != null && x.Attributes["Type"].Value.Equals("Details")).ToList();
                            for (int j = 0; j < nodos.Count; j++)
                            {
                                var nodo = nodos[j];
                                var xNodo = new XmlDocument();
                                xNodo.LoadXml(nodo.OuterXml);
                                var confNum = xNodo.GetElementsByTagName("FormattedReportObject").Cast<XmlNode>().Where(x => x.Attributes["FieldName"] != null && x.Attributes["FieldName"].Value.Equals("{cp_rpt_DPAUDDAA_1.conf_num}")).Select(x => x.ChildNodes.Cast<XmlNode>().Where(y => y.Name.Equals("Value")).FirstOrDefault()).FirstOrDefault().InnerText;
                                if (confNum.Equals(folio))
                                {
                                    var listaPosted = xNodo.GetElementsByTagName("FormattedReportObject").Cast<XmlNode>().Where(x => x.Attributes["FieldName"] != null && x.Attributes["FieldName"].Value.Equals("{@posted}")).ToList();
                                    var listaValues = listaPosted.Select(x => x.ChildNodes.Cast<XmlNode>().Where(y => y.Name.Equals("Value")).FirstOrDefault()).ToList();
                                    foreach (var child in listaValues)
                                    {
                                        if (!string.IsNullOrEmpty(child.InnerText.Trim()))
                                        {
                                            decimal childValue = 0;
                                            decimal.TryParse(child.InnerText.Replace("(", "").Trim(), out childValue);
                                            totalXml += childValue;
                                        }
                                    }
                                }
                            }
                            folioXml.TotalConfirmacion = totalXml.ToString();
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }

                        //if (estado.Equals("2E"))
                        //{
                        //    // 2E = Pendientes por facturar (Prefacturador)
                        //    listaNoFacturados.Add(folioXml);
                        //}
                        //else
                        //{
                        listaFacturados.Add(folioXml);
                        //}
                        //listaNoFacturados.Remove(folioXml);
                    }
                }
                listaFacturados.ForEach(x => listaNoFacturados.Remove(x));
                Session["_listaFacturados_FacturaGlobalOnQ"] = listaFacturados;
                Session["_listaNoFacturados_FacturaGlobalOnQ"] = listaNoFacturados;
                _db.Desconectar();
            }
            catch (Exception ex)
            {
                _log.RegistrarTxt(ex.Message, "FacturaGlobalOnQ", "ConsultarBd", ex.ToString());
                Session["_listaFacturados_FacturaGlobalOnQ"] = null;
                Session["_listaNoFacturados_FacturaGlobalOnQ"] = null;
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
        /// Handles the PageIndexChanging event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFacturas.DataSource = (DataTable)Session["_dtFacturas"];
            gvFacturas.PageIndex = e.NewPageIndex;
            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["_dtFacturas"] != null)
            {
                var lista = (DataTable)Session["_dtFacturas"];
                var dataView = new DataView(lista)
                {
                    Sort = e.SortExpression + " " + (e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC")
                };
                gvFacturas.DataSource = dataView;
                gvFacturas.DataBind();
            }
        }

        /// <summary>
        /// Converts to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>DataTable.</returns>
        private DataTable ConvertToDataTable<T>(IEnumerable<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// Handles the Click event of the bGenerarExcel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bGenerarExcel_Click(object sender, EventArgs e)
        {
            var initialColumnNumber = 8;
            var finalColumnNumber = 16;
            try
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Match");
                var dt = (DataTable)Session["_dtFacturas"];
                ws.Cell(1, 1).Value = "Reporte de Match OnQ vs Data Express Mes " + Localization.Now.ToString("MM/yyyy");
                ws.Range(1, 1, 1, dt.Columns.Count).Merge().AddToNamed("Titles1");
                ws.Cell(2, 1).Value = "";
                ws.Range(2, 1, 2, dt.Columns.Count).Merge().AddToNamed("Titles2");
                ws.Cell(3, 1).Value = "Fecha de Generación: " + Localization.Now.ToString("s");
                ws.Range(3, 1, 3, dt.Columns.Count).Merge().AddToNamed("Titles3");
                var tableWithData = ws.Cell(4, 1).InsertTable(dt.AsEnumerable());
                var titlesStyle = wb.Style;
                titlesStyle.Font.Bold = true;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                titlesStyle.Font.FontSize = 20;
                wb.NamedRanges.NamedRange("Titles1").Ranges.Style = titlesStyle;
                titlesStyle.Font.Bold = false;
                titlesStyle.Font.FontSize = 12;
                wb.NamedRanges.NamedRange("Titles2").Ranges.Style = titlesStyle;
                titlesStyle.Font.FontSize = 11;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                wb.NamedRanges.NamedRange("Titles3").Ranges.Style = titlesStyle;
                try
                {
                    ws.Range(5, initialColumnNumber, dt.Rows.Count + 4, finalColumnNumber).Style.NumberFormat.Format = "#,##0.00";
                    ws.Range(5, initialColumnNumber, dt.Rows.Count + 4, finalColumnNumber).DataType = XLCellValues.Number;
                }
                catch { }
                for (int i = initialColumnNumber; i <= finalColumnNumber; i++)
                {
                    var initialRow = 5;
                    var finalRow = (dt.Rows.Count + 4);
                    var letraColumna = GetExcelColumnName(i);
                    string formula = "=SUM(" + letraColumna + initialRow + ":" + letraColumna + finalRow + ")";
                    ws.Cell(finalRow + 1, i).FormulaA1 = formula;
                    try
                    {
                        ws.Cell(finalRow + 1, i).Style.NumberFormat.Format = "#,##0.00";
                        ws.Cell(finalRow + 1, i).DataType = XLCellValues.Number;
                        ws.Cell(finalRow + 1, i).Style.Font.Bold = true;
                    }
                    catch { }
                }
                ws.Columns().AdjustToContents();
                ws.Tables.First().ShowAutoFilter = false;
                wb.ShowZeros = true;
                var stream = new MemoryStream();
                wb.SaveAs(stream);
                var bytes = stream.ToArray();
                var base64 = Convert.ToBase64String(bytes);
                var contentType = "application/octet-stream";
                var fileName = "ReporteMatch.xlsx";
                ScriptManager.RegisterStartupScript(this, GetType(), "_downloadXlsReporteMatch_OnQ", "downloadBytes('" + base64 + "','" + contentType + "','" + fileName + "', false);", true);
            }
            catch (Exception ex)
            {
                _log.RegistrarTxt(ex.Message, "FacturaGlobalOnQ", "bGenerarExcel_Click", ex.ToString());
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo generar el reporte, intentelo nuevamente.<br/><br/>" + ex.Message, 4);
            }
        }

        /// <summary>
        /// Gets the name of the excel column.
        /// </summary>
        /// <param name="columnNumber">The column number.</param>
        /// <returns>System.String.</returns>
        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }

    /// <summary>
    /// Class RegistroXmlOnQMensual.
    /// </summary>
    public class RegistroXmlOnQMensual
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; } = 0;
        /// <summary>
        /// Gets or sets the numero confirmacion.
        /// </summary>
        /// <value>The numero confirmacion.</value>
        public string NumeroConfirmacion { get; set; } = "";
        /// <summary>
        /// Gets or sets the habitacion.
        /// </summary>
        /// <value>The habitacion.</value>
        public string Habitacion { get; set; } = "";
        /// <summary>
        /// Gets or sets the identifier comprobante.
        /// </summary>
        /// <value>The identifier comprobante.</value>
        public string IdComprobante { get; set; } = "";
        /// <summary>
        /// Gets or sets the UUID.
        /// </summary>
        /// <value>The UUID.</value>
        public string Uuid { get; set; } = "";
        /// <summary>
        /// Gets or sets the folio factura.
        /// </summary>
        /// <value>The folio factura.</value>
        public string FolioFactura { get; set; } = "";
        /// <summary>
        /// The estado
        /// </summary>
        private string _Estado = "";
        /// <summary>
        /// Gets or sets the estado.
        /// </summary>
        /// <value>The estado.</value>
        public string Estado
        {
            get
            {
                switch (_Estado)
                {
                    case "1E": return "Timbrado";
                    case "4E": return "Anulado por NC";
                    case "2E": return "Pendiente por timbrar";
                    case "2C": return "Pendiente por cancelar";
                    case "0C": return "Cancelado";
                    default: return "N/A";
                }
            }
            set { _Estado = value; }
        }
        /// <summary>
        /// Gets or sets the sub total.
        /// </summary>
        /// <value>The sub total.</value>
        public string SubTotal { get; set; } = "";
        /// <summary>
        /// Gets or sets the iva.
        /// </summary>
        /// <value>The iva.</value>
        public string Iva { get; set; } = "";
        /// <summary>
        /// Gets or sets the ish.
        /// </summary>
        /// <value>The ish.</value>
        public string Ish { get; set; } = "";
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public string Total { get; set; } = "";
        /// <summary>
        /// Gets or sets the propinas.
        /// </summary>
        /// <value>The propinas.</value>
        public string Propinas { get; set; } = "";
        /// <summary>
        /// Gets or sets the otros.
        /// </summary>
        /// <value>The otros.</value>
        public string Otros { get; set; } = "";
        /// <summary>
        /// Gets the total pagar.
        /// </summary>
        /// <value>The total pagar.</value>
        public string TotalPagar
        {
            get
            {
                decimal mensual = 0;
                decimal totalPagar = 0;
                decimal total = 0;
                decimal propinas = 0;
                decimal otros = 0;
                decimal diferencia = 0;
                decimal.TryParse(TotalConfirmacion, out mensual);
                decimal.TryParse(Total, out total);
                decimal.TryParse(Propinas, out propinas);
                decimal.TryParse(Otros, out otros);
                totalPagar = total + propinas + otros;
                return totalPagar.ToString();
            }
        }
        /// <summary>
        /// Gets or sets the total confirmacion.
        /// </summary>
        /// <value>The total confirmacion.</value>
        public string TotalConfirmacion { get; set; } = "";
        /// <summary>
        /// Gets the diferencia montos.
        /// </summary>
        /// <value>The diferencia montos.</value>
        public string DiferenciaMontos
        {
            get
            {
                decimal mensual = 0;
                decimal totalPagar = 0;
                decimal diferencia = 0;
                decimal.TryParse(TotalConfirmacion, out mensual);
                decimal.TryParse(TotalPagar, out totalPagar);
                diferencia = Math.Abs(totalPagar - mensual);
                return diferencia.ToString();
            }
        }
        /// <summary>
        /// Gets or sets the fecha emision.
        /// </summary>
        /// <value>The fecha emision.</value>
        public string FechaEmision { get; set; } = "";
        /// <summary>
        /// Gets or sets the fechatimbrado.
        /// </summary>
        /// <value>The fechatimbrado.</value>
        public string Fechatimbrado { get; set; } = "";
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>The usuario.</value>
        public string Usuario { get; set; } = "";
    }

}