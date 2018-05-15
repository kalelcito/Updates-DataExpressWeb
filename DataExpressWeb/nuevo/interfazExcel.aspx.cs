// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 07-02-2017
//
// Last Modified By : Sergio
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="interfazExcel.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using Config;
using Control;
using DataExpressWeb.wsEmision;
using Datos;
using DocumentFormat.OpenXml.InkML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace DataExpressWeb.nuevo
{
    /// <summary>
    /// Class InterfazExcel.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class InterfazExcel : System.Web.UI.Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log;
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos(Session["IDENTEMI"]?.ToString() ?? "CORE");
                _log = new Log(Session["IDENTEMI"]?.ToString() ?? "CORE");
                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                }
                if (!IsPostBack)
                {
                    Session["_cartera"] = null;
                    Session["_carteraInter"] = null;
                    Session["_facturas"] = null;
                    Session["_dtClientes"] = null;
                    Session["_dtFacturas"] = null;
                    Session["_listaClientes"] = null;
                    Session["_listaFacturas"] = null;
                    GetCarteras();
                }
            }
            else
            {
                Response.Redirect("~/Cerrar.aspx");
            }
        }

        /// <summary>
        /// Gets the carteras.
        /// </summary>
        private void GetCarteras()
        {
            var parametros = new Parametros(Session["IDENTEMI"].ToString());
            //XlsCarteraClientes_SUN
            //XlsCarteraIntercompany_SUN
            lbDownloadCartera.Visible = false;
            lbDownloadIntercompany.Visible = false;
            var carteraClientesGuardada = parametros.GetParametroByNombre("XlsCarteraClientes_SUN");
            var carteraIntercompanyGuardada = parametros.GetParametroByNombre("XlsCarteraIntercompany_SUN");
            if (carteraClientesGuardada != null && !string.IsNullOrEmpty(carteraClientesGuardada.Descripcion))
            {
                byte[] bytes;
                try
                {
                    bytes = ControlUtilities.DecodeBase64ToFile(carteraClientesGuardada.Descripcion);
                    if (bytes != null)
                    {
                        var cartera = new object[] { bytes, "CarteraExistente.xlsx" };
                        Session["_cartera"] = cartera;
                        lbDownloadCartera.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            if (carteraIntercompanyGuardada != null && !string.IsNullOrEmpty(carteraIntercompanyGuardada.Descripcion))
            {
                byte[] bytes;
                try
                {
                    bytes = ControlUtilities.DecodeBase64ToFile(carteraIntercompanyGuardada.Descripcion);
                    if (bytes != null)
                    {
                        var cartera = new object[] { bytes, "CarteraIntercompanyExistente.xlsx" };
                        Session["_carteraInter"] = cartera;
                        lbDownloadIntercompany.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Handles the UploadedComplete event of the fileCartera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AjaxControlToolkit.AsyncFileUploadEventArgs"/> instance containing the event data.</param>
        protected void fileCartera_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Session["_cartera"] = new object[] { PostedFileToBytes(fileCartera.PostedFile), fileCartera.FileName };
        }

        /// <summary>
        /// Handles the UploadedComplete event of the fileIntercomp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AjaxControlToolkit.AsyncFileUploadEventArgs"/> instance containing the event data.</param>
        protected void fileIntercomp_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Session["_carteraInter"] = new object[] { PostedFileToBytes(fileIntercomp.PostedFile), fileIntercomp.FileName };
        }

        /// <summary>
        /// Handles the UploadedComplete event of the fileInterfaz control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AjaxControlToolkit.AsyncFileUploadEventArgs"/> instance containing the event data.</param>
        protected void fileInterfaz_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Session["_facturas"] = new object[] { PostedFileToBytes(fileInterfaz.PostedFile), fileInterfaz.FileName };
        }

        /// <summary>
        /// Handles the Click event of the lbProcesar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbProcesar_Click(object sender, EventArgs e)
        {
            if (Session["_cartera"] == null && Session["_facturas"] == null && Session["_carteraInter"] == null)
            {
                Session["_dtClientes"] = null;
                Session["_dtFacturas"] = null;
                Session["_listaClientes"] = null;
                Session["_listaFacturas"] = null;
                gvClientes.DataSource = null;
                gvClientes.DataBind();
                gvFacturas.DataSource = null;
                gvFacturas.DataBind();
                divClientes.Style["display"] = "none";
                divFacturas.Style["display"] = "none";
                rowFacturar.Style["display"] = "none";
                (Master as SiteMaster).MostrarAlerta(this, "No ha seleccionado ningun archivo", 4);
            }
            else
            {
                MostrarCartera();
                MostrarFacturas();
                GuardarCarteras();
                Session["_cartera"] = null;
                Session["_carteraInter"] = null;
                Session["_facturas"] = null;
            }
        }

        /// <summary>
        /// Posteds the file to bytes.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.Byte[].</returns>
        private static byte[] PostedFileToBytes(HttpPostedFile file)
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
            catch
            {
                // ignored
            }
            return result;
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvClientes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClientes.DataSource = (DataTable)Session["_dtClientes"];
            gvClientes.PageIndex = e.NewPageIndex;
            gvClientes.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the gvClientes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["_dtClientes"] != null)
            {
                var lista = (DataTable)Session["_dtClientes"];
                var dataView = new DataView(lista)
                {
                    Sort = e.SortExpression + " " + (e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC")
                };
                gvClientes.DataSource = dataView;
                gvClientes.DataBind();
            }
        }

        /// <summary>
        /// Converts to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>DataTable.</returns>
        private static DataTable ConvertToDataTable<T>(IEnumerable<T> data)
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
        /// Handles the Click event of the lbFacturar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbFacturar_Click(object sender, EventArgs e)
        {
            string _lugarExpedicion = "";
            string _nombreEmisor = "";
            string _rfcEmi = "";
            string _calleEmisor = "";
            string _noExtEmisor = "";
            string _noIntEmisor = "";
            string _coloniaEmisor = "";
            string _localidadEmisor = "";
            string _refEmisor = "";
            string _munEmisor = "";
            string _estadoEmisor = "";
            string _paisEmisor = "";
            string _cpEmisor = "";
            string _regimen = "";
            string serie = GetSerie();
            bool ambiente = GetAmbiente(serie);

            try
            {
                _db.Conectar();
                _db.CrearComando(@"SELECT
	                                NOMEMI AS nombre
	                                ,RFCEMI AS rfc
	                                ,dirMatriz AS calle
	                                ,noExterior
	                                ,noInterior
	                                ,colonia
	                                ,localidad
	                                ,referencia
	                                ,municipio
	                                ,estado
	                                ,pais
	                                ,codigoPostal
	                                ,regimenFiscal
                                FROM
	                                Cat_Emisor
                                WHERE RFCEMI = @RFCEMI");
                _db.AsignarParametroCadena("@RFCEMI", Session["rfcSucursal"].ToString());
                var dr = _db.EjecutarConsulta();

                while (dr.Read())
                {
                    _nombreEmisor = dr["nombre"].ToString();
                    _rfcEmi = dr["rfc"].ToString();
                    _calleEmisor = dr["calle"].ToString();
                    _noExtEmisor = dr["noExterior"].ToString();
                    _noIntEmisor = dr["noInterior"].ToString();
                    _coloniaEmisor = dr["colonia"].ToString();
                    _localidadEmisor = dr["localidad"].ToString();
                    _refEmisor = dr["referencia"].ToString();
                    _munEmisor = dr["municipio"].ToString();
                    _estadoEmisor = dr["estado"].ToString();
                    _paisEmisor = dr["pais"].ToString();
                    _cpEmisor = dr["codigoPostal"].ToString();
                    _regimen = dr["regimenFiscal"].ToString();
                    _lugarExpedicion = _paisEmisor + ", " + _estadoEmisor;
                }
            }
            catch (Exception ex) { }
            finally { _db.Desconectar(); }
            var listaFacturas = (List<Factura>)Session["_listaFacturas"];
            var listaClientes = (List<Cliente>)Session["_listaClientes"];
            var listaFacturasFallidas = new Dictionary<Factura, string>();
            if (listaFacturas.Count <= 0)
            {

            }
            foreach (var factura in listaFacturas)
            {
                var cliente = listaClientes.First(reg => reg.Codigo.Equals(factura.CuentaContable));
                if (cliente != null)
                {
                    var txt = new SpoolMx();
                    decimal rateUsd = 0;
                    decimal ish = 0;
                    decimal subTotal = 0;
                    decimal iva = 0;
                    decimal.TryParse(factura.Tc, out rateUsd);
                    decimal.TryParse(factura.Ish, out ish);
                    decimal.TryParse(factura.SubTotal, out subTotal);
                    decimal.TryParse(factura.MontoIva, out iva);

                    subTotal = subTotal * rateUsd;
                    ish = ish * rateUsd;
                    iva = iva * rateUsd;
                    subTotal = subTotal - ish;

                    txt.SetComprobanteCfdi(serie, "", Localization.Now.ToString("s"), "Pago en una sola exhibición", "", subTotal.ToString(), "", "", "1.0", "MXN", factura.TotalMxp, "ingreso", factura.MetodoPago, _lugarExpedicion, factura.NumeroCuenta, "", "", "", "", "", "", "", factura.TotalMxp, "");
                    txt.SetEmisorCfdi(_nombreEmisor, _rfcEmi, "", "", "", "1", _regimen);
                    txt.SetEmisorDomCfdi(_calleEmisor, _noExtEmisor, _noIntEmisor, _coloniaEmisor, _localidadEmisor, _refEmisor, _munEmisor, _estadoEmisor, _paisEmisor, _cpEmisor);
                    txt.SetEmisorExpCfdi(_calleEmisor, _noExtEmisor, _noIntEmisor, _coloniaEmisor, _localidadEmisor, _refEmisor, _munEmisor, _estadoEmisor, _paisEmisor, _cpEmisor, GetSucursalSpool(""));
                    txt.SetReceptorCfdi(cliente.RazonSocial, cliente.Rfc, "", cliente.Telefono, cliente.Mail, "", cliente.RazonComercial);
                    txt.SetReceptorDomCfdi(cliente.Calle, "", "", cliente.Colonia, cliente.Localidad, "", cliente.Municipio, "", cliente.Pais, cliente.CodigoPostal);
                    txt.SetCantidadImpuestosCfdi("0.00", iva.ToString(), iva.ToString());
                    txt.AgregaImpuestoTrasladoCfdi("IVA", "16.00", iva.ToString());
                    txt.SetImpuestosLocalesCfdi("0.00", ish.ToString());
                    txt.AgregaTrasladoLocalCfdi("ISH", ISH(), ish.ToString());
                    txt.AgregaConceptoCfdi(factura.NoNoches, "Noches", "", "Renta de Habitación", factura.Tarifa, factura.RentaHabitacion);
                    txt.SetInfoAdicionalRestauranteCfdi("", factura.BookingId);
                    var txtInvoice = txt.ConstruyeTxtCfdi();
                    var coreMx = new WsEmision { Timeout = (1800 * 1000) };
                    var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, "01", false, true, "", "");
                    if (result != null)
                    {
                        var xDoc = new XmlDocument();
                        xDoc.LoadXml(result.OuterXml);
                    }
                    else
                    {
                        listaFacturasFallidas.Add(factura, coreMx.ObtenerMensaje() + "<br/>" + coreMx.ObtenerMensajeTecnico());
                    }
                }
            }
            if (listaFacturasFallidas.Count > 0)
            {
                Session["_dtFacturas"] = null;
                Session["_listaFacturas"] = listaFacturasFallidas.Keys;
                var dt = ConvertToDataTable(listaFacturasFallidas.Keys);
                lCountFacturas.Text = "Se encontraron <span class='badge'>" + dt.Rows.Count + "</span> Registros para facturar";
                Session["_dtFacturas"] = dt;
                gvFacturas.DataSource = (DataTable)Session["_dtFacturas"];
                gvFacturas.DataBind();
                divFacturas.Style["display"] = "inline";
                rowFacturar.Style["display"] = "inline";
                var html = "<ul>";
                foreach (var item in listaFacturasFallidas)
                {
                    html += "<li><b>" + item.Key.BookingId + "</b>: " + item.Value + "</li>";
                }
                html += "</ul>";
                (Master as SiteMaster).MostrarAlerta(this, "Ocurrió uno o más errores al procesar los comprobantes:<br>" + html, 4, null);
            }
            else
            {
                gvFacturas.DataSource = null;
                gvFacturas.DataBind();
                (Master as SiteMaster).MostrarAlerta(this, "Los comprobantes se han generado satisfactoriamente", 2, null, "window.location.href = '" + ResolveClientUrl("~/Documentos.aspx") + "';");
            }
        }

        /// <summary>
        /// Gets the ambiente.
        /// </summary>
        /// <param name="serie">The serie.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception">La serie " + serie + " no está registrada.</exception>
        private bool GetAmbiente(string serie)
        {
            bool ambiente;
            _db.Conectar();
            _db.CrearComando(@"SELECT ambiente FROM Cat_Series Where serie = @serie");
            _db.AsignarParametroCadena("@serie", serie);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                ambiente = dr["ambiente"].ToString().Equals("2");
            }
            else
            {
                throw new Exception("La serie " + serie + " no está registrada.");
            }
            _db.Desconectar();
            return ambiente;
        }

        /// <summary>
        /// Gets the serie.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetSerie()
        {
            var result = "";
            var sql = @"SELECT TOP 1 serie FROM Cat_Series WHERE tipoDoc = '01' AND tipo = 1";
            _db.Conectar();
            _db.CrearComando(sql);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                result = dr["serie"].ToString();
            }
            _db.Desconectar();
            return result;
        }

        /// <summary>
        /// Ishes this instance.
        /// </summary>
        /// <returns>System.String.</returns>
        private string ISH()
        {
            var ish = "";
            try
            {
                _db.Conectar();
                _db.CrearComando(@"SELECT ISNULL(SUM(valor), 00) FROM Cat_CatImpuestos_C WHERE descripcion LIKE '%ISH%'");
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    ish = dr[0].ToString();
                }
                _db.Desconectar();
            }
            catch (Exception)
            {
                _db.Desconectar();
            }
            return ish;
        }

        /// <summary>
        /// Gets the sucursal spool.
        /// </summary>
        /// <param name="_exsucursal">The exsucursal.</param>
        /// <returns>System.String.</returns>
        private string GetSucursalSpool(string _exsucursal)
        {
            var sucursal = "1";
            try
            {
                _db.Conectar();
                _db.CrearComando("SELECT COALESCE((SELECT idSucursal FROM Cat_SucursalesEmisor WHERE clave = @clave), (SELECT MIN(idSucursal) FROM Cat_SucursalesEmisor)) AS idSucursal");
                _db.AsignarParametroCadena("@clave", _exsucursal);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    sucursal = dr[0].ToString();
                }
            }
            catch { sucursal = ""; }
            finally
            {
                _db.Desconectar();
            }
            return sucursal;
        }

        /// <summary>
        /// Clientes the existente.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ClienteExistente(string rfc)
        {
            var existente = false;
            try
            {
                _db.Conectar();
                _db.CrearComando("SELECT idReceptor FROM Cat_Receptor WHERE rfc = @rfc");
                _db.AsignarParametroCadena("@rfc", rfc);
                var dr = _db.EjecutarConsulta();
                existente = dr.Read();
            }
            catch { _db.Desconectar(); }
            return existente;
        }

        /// <summary>
        /// Mostrars the facturas.
        /// </summary>
        private void MostrarFacturas()
        {
            var list = new List<Factura>();
            if (Session["_facturas"] != null)
            {
                var obj = (object[])Session["_facturas"];
                var bytes = (byte[])obj[0];
                var fileName = (string)obj[1];
                Stream stream = new MemoryStream(bytes);
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.First();
                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;
                    const int skipRows = 14;
                    for (var row = (start.Row + skipRows); row <= end.Row; row++)
                    {
                        try
                        {
                            var record = new Factura
                            {
                                CuentaContable = workSheet.Cells[row, 1].Text,
                                NombreHuesped = workSheet.Cells[row, 2].Text,
                                BookingId = workSheet.Cells[row, 3].Text,
                                NroFactura = workSheet.Cells[row, 4].Text,
                                FechaFactura = workSheet.Cells[row, 5].Text,
                                Type = workSheet.Cells[row, 6].Text,//
                                Tarifa = workSheet.Cells[row, 7].Text,
                                Desayuno = workSheet.Cells[row, 8].Text,
                                In = workSheet.Cells[row, 9].Text,
                                Out = workSheet.Cells[row, 10].Text,
                                NoNoches = workSheet.Cells[row, 11].Text,//
                                RentaHabitacion = workSheet.Cells[row, 12].Text,//
                                Ish = workSheet.Cells[row, 13].Text,//
                                ConsumosCIva = workSheet.Cells[row, 18].Text,//
                                ServiciosSIva = workSheet.Cells[row, 19].Text,
                                Misc1CIva = workSheet.Cells[row, 20].Text,
                                Misc2CIva = workSheet.Cells[row, 21].Text,//
                                PeliculasInternet = workSheet.Cells[row, 22].Text,//
                                SubTotal = workSheet.Cells[row, 26].Text,//
                                MetodoPago = workSheet.Cells[row, 27].Text,
                                NumeroCuenta = workSheet.Cells[row, 28].Text,
                                TipoIva = workSheet.Cells[row, 29].Text,
                                MontoIva = workSheet.Cells[row, 30].Text,//
                                OtrosExento = workSheet.Cells[row, 31].Text,//
                                TotalMonedaTrn = workSheet.Cells[row, 32].Text,
                                Moneda = workSheet.Cells[row, 33].Text,
                                Tc = workSheet.Cells[row, 34].Text,//
                                TotalMxp = workSheet.Cells[row, 35].Text,
                                AdvanceSn = workSheet.Cells[row, 36].Text,//
                                Referencia = workSheet.Cells[row, 37].Text,//
                                Ext1 = workSheet.Cells[row, 38].Text,
                                Ext2 = workSheet.Cells[row, 39].Text,
                                Ext3 = workSheet.Cells[row, 40].Text,
                                Ext4 = workSheet.Cells[row, 41].Text,
                                Ext5 = workSheet.Cells[row, 42].Text
                            };
                            if (!record.IsEmpty)
                            {
                                if (Regex.IsMatch(record.NroFactura, @"[A-Za-z]+"))
                                // if (record.NroFactura.ToString().StartsWith("A") || record.NroFactura.ToString().StartsWith("K") || record.NroFactura.ToString().StartsWith("S"))
                                {
                                    try
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"insert into Dat_InterfazSun (cuentaContable,nombreHuesped,bookingID,noFactura,fechaFactura,type,tarifa,desayuno,check_In,
                                        check_Out,noNoches,RentaHabitacion,ISH,Consumos_co_iva,Servicios_sn_iva,misc1_cn_IVA,misc2_sn_IVA,PeliculasInternet,Subtotal,metodoPago,
                                        noCuenta,tipoIVA,MontoIva,OtrosExcento,totalMonedaTRN,moneda,TC,totalMXP,AdvanceSN,Referencia,campo1,campo2,campo3,campo4,campo5,fechaLec,RFCEMI,nombreArchivo)
                                        values
                                        (@cuentaContable,@NombreHuesped,@BookingId,@NroFactura,@Fecha,@type,@Tarifa,@Desayuno,@In,@Out,@noNoches,@RentaHabitacion,@ISH,
                                        @Consumos_co_iva,@ServiciosSIva,@Misc1CIva,@Misc2CIva,@PeliculasInternet,@SubTotal,@MetodoPago,@NumeroCuenta,@TipoIva,@MontoIva,@OtrosExento,
                                        @TotalMonedaTrn,@Moneda,@Tc,@TotalMxp,@AdvanceSn,@Referencia,@Ext1,@Ext2,@Ext3,@Ext4,@Ext5,@fechaFac,@rfcEmi,@nomArchivo) ");
                                        _db.AsignarParametroCadena("@cuentaContable", record.CuentaContable);
                                        _db.AsignarParametroCadena("@NombreHuesped", record.NombreHuesped);
                                        _db.AsignarParametroCadena("@BookingId", record.BookingId);
                                        _db.AsignarParametroCadena("@NroFactura", record.NroFactura);
                                        _db.AsignarParametroCadena("@Fecha", Convert.ToString(record.FechaFactura));
                                        _db.AsignarParametroCadena("@type", record.Type);
                                        _db.AsignarParametroCadena("@Tarifa", record.Tarifa);
                                        _db.AsignarParametroCadena("@Desayuno", record.Desayuno);
                                        _db.AsignarParametroCadena("@In", Convert.ToString(record.In));
                                        _db.AsignarParametroCadena("@Out", Convert.ToString(record.Out));
                                        _db.AsignarParametroCadena("@noNoches", record.NoNoches);
                                        _db.AsignarParametroCadena("@RentaHabitacion", record.RentaHabitacion);
                                        _db.AsignarParametroCadena("@ISH", record.Ish);
                                        _db.AsignarParametroCadena("@Consumos_co_iva", record.ConsumosCIva);
                                        _db.AsignarParametroCadena("@ServiciosSIva", record.ServiciosSIva);
                                        _db.AsignarParametroCadena("@Misc1CIva", record.Misc1CIva);
                                        _db.AsignarParametroCadena("@Misc2CIva", record.Misc2CIva);
                                        _db.AsignarParametroCadena("@PeliculasInternet", record.PeliculasInternet);
                                        _db.AsignarParametroCadena("@SubTotal", record.SubTotal);
                                        _db.AsignarParametroCadena("@MetodoPago", record.MetodoPago);
                                        _db.AsignarParametroCadena("@NumeroCuenta", record.NumeroCuenta);
                                        _db.AsignarParametroCadena("@TipoIva", record.TipoIva);
                                        _db.AsignarParametroCadena("@MontoIva", record.MontoIva);
                                        _db.AsignarParametroCadena("@OtrosExento", record.OtrosExento);
                                        _db.AsignarParametroCadena("@TotalMonedaTrn", record.TotalMonedaTrn);
                                        _db.AsignarParametroCadena("@Moneda", record.Moneda);
                                        _db.AsignarParametroCadena("@Tc", record.Tc);
                                        _db.AsignarParametroCadena("@TotalMxp", record.TotalMxp);
                                        _db.AsignarParametroCadena("@AdvanceSn", record.AdvanceSn);
                                        _db.AsignarParametroCadena("@Referencia", record.Referencia);
                                        _db.AsignarParametroCadena("@Ext1", record.Ext1);
                                        _db.AsignarParametroCadena("@Ext2", record.Ext2);
                                        _db.AsignarParametroCadena("@Ext3", record.Ext3);
                                        _db.AsignarParametroCadena("@Ext4", record.Ext4);
                                        _db.AsignarParametroCadena("@Ext5", record.Ext5);
                                        _db.AsignarParametroCadena("@fechaFac", Localization.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                        _db.AsignarParametroCadena("@rfcEmi", Session["IDENTEMI"].ToString());
                                        _db.AsignarParametroCadena("@nomArchivo", fileName.ToString());
                                        _db.EjecutarConsulta();
                                    }
                                    catch (Exception ex)
                                    { }
                                    finally
                                    {
                                        _db.Desconectar();
                                    }

                                }
                                else
                                {
                                    list.Add(record);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
            Session["_listaFacturas"] = list;
            var dt = ConvertToDataTable(list);
            lCountFacturas.Text = "Se encontraron <span class='badge'>" + dt.Rows.Count + "</span> Registros para facturar";
            Session["_dtFacturas"] = dt;
            gvFacturas.DataSource = (DataTable)Session["_dtFacturas"];
            gvFacturas.DataBind();
            divFacturas.Style["display"] = "inline";
            if (dt.Rows.Count > 0)
            {
                rowFacturar.Style["display"] = "inline";
            }
        }

        /// <summary>
        /// Mostrars the cartera.
        /// </summary>
        private void MostrarCartera()
        {
            var list = new List<Cliente>();
            if (Session["_cartera"] != null)
            {
                var obj = (object[])Session["_cartera"];
                var bytes = (byte[])obj[0];
                var fileName = (string)obj[1];
                Stream stream = new MemoryStream(bytes);
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.First();
                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;
                    const int skipRows = 6;
                    for (var row = (start.Row + skipRows); row <= end.Row; row++)
                    {
                        try
                        {
                            var record = new Cliente
                            {
                                Codigo = workSheet.Cells[row, 1].Text,
                                RazonSocial = workSheet.Cells[row, 2].Text,
                                RazonComercial = workSheet.Cells[row, 3].Text,
                                Address3 = workSheet.Cells[row, 4].Text,
                                Address4 = workSheet.Cells[row, 5].Text,
                                Address5 = workSheet.Cells[row, 6].Text,
                                Address6 = workSheet.Cells[row, 7].Text,
                                Telefono = workSheet.Cells[row, 8].Text,
                                Mail = workSheet.Cells[row, 11].Text,
                                Comentarios = workSheet.Cells[row, 12].Text,
                                Rfc = workSheet.Cells[row, 13].Text,
                                Existente = ClienteExistente(workSheet.Cells[row, 13].Text)
                            };
                            if (!string.IsNullOrEmpty(workSheet.Cells[row, 10].Text))
                            {
                                record.Contactos = new string[] { workSheet.Cells[row, 10].Text };
                            }
                            if (!record.IsEmpty)
                            {
                                list.Add(record);
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
            if (Session["_carteraInter"] != null)
            {
                var obj = (object[])Session["_carteraInter"];
                var bytes = (byte[])obj[0];
                var fileName = (string)obj[1];
                Stream stream = new MemoryStream(bytes);
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.First();
                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;
                    const int skipRows = 2;
                    for (var row = (start.Row + skipRows); row <= end.Row; row++)
                    {
                        try
                        {
                            var contactos = new List<string>();
                            var record = new Cliente
                            {
                                Codigo = workSheet.Cells[row, 1].Text,
                                RazonSocial = workSheet.Cells[row, 2].Text,
                                RazonComercial = workSheet.Cells[row, 3].Text,
                                Rfc = workSheet.Cells[row, 4].Text,
                                Existente = ClienteExistente(workSheet.Cells[row, 4].Text)
                            };
                            if (!string.IsNullOrEmpty(workSheet.Cells[row, 5].Text))
                            {
                                contactos.Add(workSheet.Cells[row, 5].Text);
                            }
                            if (!string.IsNullOrEmpty(workSheet.Cells[row, 6].Text))
                            {
                                contactos.Add(workSheet.Cells[row, 6].Text);
                            }
                            record.Contactos = contactos.ToArray();
                            if (!record.IsEmpty)
                            {
                                list.Add(record);
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
            Session["_listaClientes"] = list;
            var dt = ConvertToDataTable(list);
            lCountClientes.Text = "Se encontraron <span class='badge'>" + dt.Rows.Count + "</span> Clientes";
            Session["_dtClientes"] = dt;
            gvClientes.DataSource = (DataTable)Session["_dtClientes"];
            gvClientes.DataBind();
            divClientes.Style["display"] = "inline";
        }

        /// <summary>
        /// Guardars the carteras.
        /// </summary>
        private void GuardarCarteras()
        {
            var parametros = new Parametros(Session["IDENTEMI"].ToString());
            try
            {
                var arrayCartera = (object[])Session["_cartera"];
                var bytesCartera = (byte[])arrayCartera[0];
                parametros.SetParametro("XlsCarteraClientes_SUN", Convert.ToBase64String(bytesCartera));
            }
            catch (Exception ex)
            {
            }
            try
            {
                var arrayCartera = (object[])Session["_carteraInter"];
                var bytesIntercompany = (byte[])arrayCartera[0];
                parametros.SetParametro("XlsCarteraIntercompany_SUN", Convert.ToBase64String(bytesIntercompany));
            }
            catch (Exception ex)
            {
            }
            GetCarteras();
        }

        /// <summary>
        /// Class Cliente.
        /// </summary>
        private class Cliente
        {
            /// <summary>
            /// The address6
            /// </summary>
            private string _address6 = "";

            /// <summary>
            /// Gets or sets the codigo.
            /// </summary>
            /// <value>The codigo.</value>
            public string Codigo { get; set; } = "";
            /// <summary>
            /// Gets or sets the RFC.
            /// </summary>
            /// <value>The RFC.</value>
            public string Rfc { get; set; } = "";
            /// <summary>
            /// Gets or sets the razon social.
            /// </summary>
            /// <value>The razon social.</value>
            public string RazonSocial { get; set; } = "";
            /// <summary>
            /// Gets or sets the razon comercial.
            /// </summary>
            /// <value>The razon comercial.</value>
            public string RazonComercial { get; set; } = "";
            /// <summary>
            /// Gets or sets the telefono.
            /// </summary>
            /// <value>The telefono.</value>
            public string Telefono { get; set; } = "";
            /// <summary>
            /// Gets or sets the mail.
            /// </summary>
            /// <value>The mail.</value>
            public string Mail { get; set; } = "";
            /// <summary>
            /// Gets or sets the contactos.
            /// </summary>
            /// <value>The contactos.</value>
            public string[] Contactos { get; set; } = new string[] { };
            /// <summary>
            /// Sets the address3.
            /// </summary>
            /// <value>The address3.</value>
            public string Address3 { set { Calle = value; } }
            /// <summary>
            /// Sets the address4.
            /// </summary>
            /// <value>The address4.</value>
            public string Address4 { set { Colonia = value; } }
            /// <summary>
            /// Sets the address5.
            /// </summary>
            /// <value>The address5.</value>
            public string Address5 { set { Localidad = value; } }
            /// <summary>
            /// Sets the address6.
            /// </summary>
            /// <value>The address6.</value>
            public string Address6 { set { _address6 = value; } }
            /// <summary>
            /// Gets or sets the calle.
            /// </summary>
            /// <value>The calle.</value>
            public string Calle { get; private set; } = "";

            /// <summary>
            /// Gets or sets the colonia.
            /// </summary>
            /// <value>The colonia.</value>
            public string Colonia { get; private set; } = "";

            /// <summary>
            /// Gets or sets the localidad.
            /// </summary>
            /// <value>The localidad.</value>
            public string Localidad { get; private set; } = "";

            /// <summary>
            /// Gets the municipio.
            /// </summary>
            /// <value>The municipio.</value>
            public string Municipio
            {
                get
                {
                    var result = "";
                    try
                    {
                        if (_address6.StartsWith("MEXICO", StringComparison.OrdinalIgnoreCase))
                        {
                            result = _address6.Replace(Pais, "").Trim();
                            result = result.Replace(CodigoPostal, "").Trim();
                        }
                    }
                    catch { result = _address6; }
                    return result;
                }
            }
            /// <summary>
            /// Gets the pais.
            /// </summary>
            /// <value>The pais.</value>
            public string Pais
            {
                get
                {
                    var result = "";
                    if (_address6.StartsWith("MEXICO", StringComparison.OrdinalIgnoreCase))
                    {
                        result = "MEXICO";
                    }
                    else if (!string.IsNullOrEmpty(Localidad))
                    {
                        result = Localidad;
                    }
                    else
                    {
                        result = _address6;
                    }
                    return result;
                }
            }
            /// <summary>
            /// Gets the codigo postal.
            /// </summary>
            /// <value>The codigo postal.</value>
            public string CodigoPostal
            {
                get
                {
                    return GetCp(_address6);
                }
            }
            /// <summary>
            /// Gets or sets the comentarios.
            /// </summary>
            /// <value>The comentarios.</value>
            public string Comentarios { get; set; } = "";
            /// <summary>
            /// Gets the cp.
            /// </summary>
            /// <param name="cadena">The cadena.</param>
            /// <returns>System.String.</returns>
            private static string GetCp(string cadena)
            {
                var result = "";
                try
                {
                    var cp = Regex.Match(cadena, @"\d{5}", RegexOptions.IgnoreCase);
                    result = cp.Value;
                }
                catch
                {
                    // ignored
                }
                return result;
            }
            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="Cliente"/> is existente.
            /// </summary>
            /// <value><c>true</c> if existente; otherwise, <c>false</c>.</value>
            public bool Existente { get; set; } = false;
            /// <summary>
            /// Gets a value indicating whether this instance is empty.
            /// </summary>
            /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
            public bool IsEmpty
            {
                get
                {
                    var isEmpty = true;
                    isEmpty &= string.IsNullOrEmpty(RazonSocial.Trim());
                    isEmpty &= string.IsNullOrEmpty(RazonComercial.Trim());
                    isEmpty &= string.IsNullOrEmpty(Calle.Trim());
                    isEmpty &= string.IsNullOrEmpty(CodigoPostal.Trim());
                    isEmpty &= string.IsNullOrEmpty(Colonia.Trim());
                    isEmpty &= string.IsNullOrEmpty(Localidad.Trim());
                    isEmpty &= string.IsNullOrEmpty(Municipio.Trim());
                    isEmpty &= string.IsNullOrEmpty(Rfc.Trim());
                    isEmpty &= !Contactos.Any();
                    return isEmpty;
                }
            }
        }
        /// <summary>
        /// Class Factura.
        /// </summary>
        private class Factura
        {
            /// <summary>
            /// Gets or sets the cuenta contable.
            /// </summary>
            /// <value>The cuenta contable.</value>
            public string CuentaContable { get; set; } = "";
            /// <summary>
            /// Gets or sets the nombre huesped.
            /// </summary>
            /// <value>The nombre huesped.</value>
            public string NombreHuesped { get; set; } = "";
            /// <summary>
            /// Gets or sets the booking identifier.
            /// </summary>
            /// <value>The booking identifier.</value>
            public string BookingId { get; set; } = "";
            /// <summary>
            /// Gets or sets the nro factura.
            /// </summary>
            /// <value>The nro factura.</value>
            public string NroFactura { get; set; } = "";
            /// <summary>
            /// Gets or sets the fecha factura.
            /// </summary>
            /// <value>The fecha factura.</value>
            public string FechaFactura { get; set; } = "";
            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            /// <value>The type.</value>
            public string Type { get; set; } = "";
            /// <summary>
            /// Gets or sets the tarifa.
            /// </summary>
            /// <value>The tarifa.</value>
            public string Tarifa { get; set; } = "";
            /// <summary>
            /// Gets or sets the desayuno.
            /// </summary>
            /// <value>The desayuno.</value>
            public string Desayuno { get; set; } = "";
            /// <summary>
            /// Gets or sets the in.
            /// </summary>
            /// <value>The in.</value>
            public string In { get; set; } = "";
            /// <summary>
            /// Gets or sets the out.
            /// </summary>
            /// <value>The out.</value>
            public string Out { get; set; } = "";
            /// <summary>
            /// Gets or sets the no noches.
            /// </summary>
            /// <value>The no noches.</value>
            public string NoNoches { get; set; } = "";
            /// <summary>
            /// Gets or sets the renta habitacion.
            /// </summary>
            /// <value>The renta habitacion.</value>
            public string RentaHabitacion { get; set; } = "";
            /// <summary>
            /// Gets or sets the ish.
            /// </summary>
            /// <value>The ish.</value>
            public string Ish { get; set; } = "";
            /// <summary>
            /// Gets or sets the consumos c iva.
            /// </summary>
            /// <value>The consumos c iva.</value>
            public string ConsumosCIva { get; set; } = "";
            /// <summary>
            /// Gets or sets the servicios s iva.
            /// </summary>
            /// <value>The servicios s iva.</value>
            public string ServiciosSIva { get; set; } = "";
            /// <summary>
            /// Gets or sets the misc1 c iva.
            /// </summary>
            /// <value>The misc1 c iva.</value>
            public string Misc1CIva { get; set; } = "";
            /// <summary>
            /// Gets or sets the misc2 c iva.
            /// </summary>
            /// <value>The misc2 c iva.</value>
            public string Misc2CIva { get; set; } = "";
            /// <summary>
            /// Gets or sets the peliculas internet.
            /// </summary>
            /// <value>The peliculas internet.</value>
            public string PeliculasInternet { get; set; } = "";
            /// <summary>
            /// Gets or sets the sub total.
            /// </summary>
            /// <value>The sub total.</value>
            public string SubTotal { get; set; } = "";
            /// <summary>
            /// Gets or sets the metodo pago.
            /// </summary>
            /// <value>The metodo pago.</value>
            public string MetodoPago { get; set; } = "";
            /// <summary>
            /// Gets or sets the numero cuenta.
            /// </summary>
            /// <value>The numero cuenta.</value>
            public string NumeroCuenta { get; set; } = "";
            /// <summary>
            /// Gets or sets the tipo iva.
            /// </summary>
            /// <value>The tipo iva.</value>
            public string TipoIva { get; set; } = "";
            /// <summary>
            /// Gets or sets the monto iva.
            /// </summary>
            /// <value>The monto iva.</value>
            public string MontoIva { get; set; } = "";
            /// <summary>
            /// Gets or sets the otros exento.
            /// </summary>
            /// <value>The otros exento.</value>
            public string OtrosExento { get; set; } = "";
            /// <summary>
            /// Gets or sets the total moneda TRN.
            /// </summary>
            /// <value>The total moneda TRN.</value>
            public string TotalMonedaTrn { get; set; } = "";
            /// <summary>
            /// Gets or sets the moneda.
            /// </summary>
            /// <value>The moneda.</value>
            public string Moneda { get; set; } = "";
            /// <summary>
            /// Gets or sets the tc.
            /// </summary>
            /// <value>The tc.</value>
            public string Tc { get; set; } = "";
            /// <summary>
            /// Gets or sets the total MXP.
            /// </summary>
            /// <value>The total MXP.</value>
            public string TotalMxp { get; set; } = "";
            /// <summary>
            /// Gets or sets the advance sn.
            /// </summary>
            /// <value>The advance sn.</value>
            public string AdvanceSn { get; set; } = "";
            /// <summary>
            /// Gets or sets the referencia.
            /// </summary>
            /// <value>The referencia.</value>
            public string Referencia { get; set; } = "";
            /// <summary>
            /// Gets or sets the ext1.
            /// </summary>
            /// <value>The ext1.</value>
            public string Ext1 { get; set; } = "";
            /// <summary>
            /// Gets or sets the ext2.
            /// </summary>
            /// <value>The ext2.</value>
            public string Ext2 { get; set; } = "";
            /// <summary>
            /// Gets or sets the ext3.
            /// </summary>
            /// <value>The ext3.</value>
            public string Ext3 { get; set; } = "";
            /// <summary>
            /// Gets or sets the ext4.
            /// </summary>
            /// <value>The ext4.</value>
            public string Ext4 { get; set; } = "";
            /// <summary>
            /// Gets or sets the ext5.
            /// </summary>
            /// <value>The ext5.</value>
            public string Ext5 { get; set; } = "";
            /// <summary>
            /// Gets a value indicating whether this instance is empty.
            /// </summary>
            /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
            public bool IsEmpty
            {
                get
                {
                    var isEmpty = true;
                    isEmpty &= string.IsNullOrEmpty(CuentaContable.Trim());
                    isEmpty &= string.IsNullOrEmpty(NombreHuesped.Trim());
                    isEmpty &= string.IsNullOrEmpty(BookingId.Trim());
                    isEmpty &= string.IsNullOrEmpty(Tarifa.Trim());
                    isEmpty &= string.IsNullOrEmpty(In.Trim());
                    isEmpty &= string.IsNullOrEmpty(Out.Trim());
                    isEmpty &= string.IsNullOrEmpty(TotalMonedaTrn.Trim());
                    isEmpty &= string.IsNullOrEmpty(TotalMxp.Trim());
                    return isEmpty;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the lbDownloadCartera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbDownloadCartera_Click(object sender, EventArgs e)
        {
            var parametros = new Parametros(Session["IDENTEMI"].ToString());
            try
            {
                var carteraClientesGuardada = parametros.GetParametroByNombre("XlsCarteraClientes_SUN");
                var bytes = ControlUtilities.DecodeBase64ToFile(carteraClientesGuardada.Descripcion);
                var base64 = Convert.ToBase64String(bytes);
                var contentType = "application/octet-stream";
                var fileName = "CarteraExistente.xlsx";
                ScriptManager.RegisterStartupScript(this, GetType(), "_downloadXlsCarteraClientes_SUN", "downloadBytes('" + base64 + "','" + contentType + "','" + fileName + "', false);", true);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Handles the Click event of the lbDownloadIntercompany control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbDownloadIntercompany_Click(object sender, EventArgs e)
        {
            var parametros = new Parametros(Session["IDENTEMI"].ToString());
            try
            {
                var carteraIntercompanyGuardada = parametros.GetParametroByNombre("XlsCarteraIntercompany_SUN");
                var bytes = ControlUtilities.DecodeBase64ToFile(carteraIntercompanyGuardada.Descripcion);
                var base64 = Convert.ToBase64String(bytes);
                var contentType = "application/octet-stream";
                var fileName = "CarteraIntercompanyExistente.xlsx";
                ScriptManager.RegisterStartupScript(this, GetType(), "_downloadXlsCarteraClientes_SUN", "downloadBytes('" + base64 + "','" + contentType + "','" + fileName + "', false);", true);
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// The days
        /// </summary>
        public string[] days = { "A", "R", "C", "D", "K", "S", "U" };
    }
}