// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="crearNotaCreditoNuevo.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using Control;
using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace DataExpressWeb
{
    /// <summary>
    /// Class CrearNotaCreditoNuevo.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CrearNotaCreditoNuevo : Page
    {
        /// <summary>
        /// The _cod impuesto ret
        /// </summary>
        private readonly Dictionary<string, string> _codImpuestoRet = new Dictionary<string, string> { { "1", "IVA" }, { "2", "ISR" } };

        /// <summary>
        /// The _cod impuesto tras
        /// </summary>
        private readonly Dictionary<string, string> _codImpuestoTras = new Dictionary<string, string> { { "1", "IVA" }, { "2", "IEPS" } };

        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db = new BasesDatos("");
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log = new Log("");
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
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
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            _log = new Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            SqlDataRFC.ConnectionString = _db.CadenaConexion;
            SqlDataSeries.ConnectionString = _db.CadenaConexion;
            
            if (Session["idUser"] != null)
            {
                _idUser = Session["idUser"].ToString();
                if (!Page.IsPostBack)
                {
                    SqlDataRFC.DataBind();
                    ddlRFC.DataBind();
                    foreach (ListItem item in ddlRFC.Items)
                    {
                        if (item.Text.Equals(Session["rfcSucursal"].ToString()))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                    ddlSerie.DataBind();
                    ddlSerie.SelectedIndex = 0;
                    ddlSerie_SelectedIndexChanged(null, null);

                    SqlConnection con = new SqlConnection(_db.CadenaConexion);
                    SqlCommand cmd = new SqlCommand("SELECT clave, descripcion FROM Cat_UsoCfdi", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlUsoCFDI.DataTextField = "descripcion";
                    ddlUsoCFDI.DataValueField = "clave";
                    ddlUsoCFDI.DataSource = dt;
                    ddlUsoCFDI.DataBind();
                    ddlUsoCFDI.SelectedValue = "G02";
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the FinishButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">El comprobante \"" + tbUUID.Text + "\" es " + tipoDoc + ", solo se pueden anular facturas.</exception>
        protected void FinishButton_Click(object sender, EventArgs e)
        {
            try
            {
                var sql = "";
                decimal iva16 = 0;
                decimal propina = 0;
                DbDataReader dr;
                var sAmbiente = "1";
                var conceptos = new List<object[]>();
                var txt = new SpoolMx();
                var versionXmlOriginal = "";
                var folioReservacion = "";
                var idFactura = "";
                var codDoc = "";
                var tipoDoc = "";
                var motivo = "";
                if (string.IsNullOrEmpty(tbUUID.Text))
                {
                    sql = @"SELECT TOP 1 numeroAutorizacion FROM Dat_General INNER JOIN Cat_Emisor ON id_Emisor = IDEEMI WHERE serie = @serie AND folio = @folio AND RFCEMI = @RFC";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@serie", tbSerie.Text);
                    _db.AsignarParametroCadena("@folio", tbFolio.Text);
                    _db.AsignarParametroCadena("@RFC", ddlRFC.SelectedItem.Text);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        tbUUID.Text = dr[0].ToString();
                    }
                    _db.Desconectar();
                }
                if (!string.IsNullOrEmpty(tbUUID.Text))
                {
                    sql = @"SELECT TOP 1 idcomprobante FROM Dat_General WHERE numeroAutorizacion = @UUID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@UUID", tbUUID.Text);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        idFactura = dr[0].ToString();
                    }
                    _db.Desconectar();
                }
                else
                {
                    throw new Exception("No existe ningun comprobante con los parametros establecidos");
                }
                sql = "SELECT g.codDoc, cc.descripcion FROM Dat_General g INNER JOIN Cat_Catalogo1_C cc ON cc.codigo = g.codDoc AND cc.tipo = 'Comprobante' WHERE g.idComprobante = @id";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@id", idFactura);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    codDoc = dr[0].ToString();
                    tipoDoc = dr[1].ToString();
                }
                _db.Desconectar();
                if (!codDoc.Equals("01"))
                {
                    throw new Exception("El comprobante \"" + tbUUID.Text + "\" es " + tipoDoc + ", solo se pueden anular facturas.");
                }
                sql = @"SELECT TOP 1
                            g.referencia,
                            p.formapago, p.condicionesDePago, g.subTotal, g.totalDescuento, g.motivoDescuento, g.tipoCambio, g.moneda, g.total, 'egreso' as tipoDoc, g.metodoPago, g.lugarExpedicion, p.numCtaPago, g.numeroAutorizacion, g.serie, g.fecha, g.total, g.codDoc, e.NOMEMI, e.RFCEMI, e.curp as curpE, e.telefono as telE, e.email as mailE, e.EmpresaTipo as etipoE, e.regimenFiscal as regimenE, e.obligadoContabilidad as contE, e.dirMatriz AS calleE, e.noExterior as extE, e.noInterior as intE, e.colonia as colE, e.localidad as locE, e.referencia as refE, e.municipio as munE, e.estado as edoE, e.pais as paisE, e.codigoPostal as cpE, d.dirEstablecimientos as calleExp, d.noExterior as extExp, d.noInterior as intExp, d.colonia as colExp, d.localidad as locExp, d.referencia as refExp, d.municipio as munExp, d.estado as esoExp, d.pais as paisExp, d.codigoPostal as cpExp, r.NOMREC, r.RFCREC, r.curp as curpR, r.telefono as telR, r.email as mailR, r.telefono2 as tel2R, r.denominacionSocial as denR, r.obligadoContabilidad as contR, r.domicilio as calleR, r.noExterior as extR, r.noInterior as intR, r.colonia as colR, r.localidad as locR, r.referencia as refR, r.municipio as munR, r.estado as edoR, r.pais as paisR, r.codigoPostal as cpR, IVA12 as iva16, g.propina, g.ambiente, g.cargoxservicio as otrosCargos, g.importeAPagar as totalAPagar, g.observaciones, he.tipo AS tipoHE, g.folioReservacion, he.noHabitacion, he.fechaLlegada, he.fechaSalida, he.huesped,he.tipohabitacion, g.estab as claveSucursal, g.noTicket, g.usoCfdi, r.numRegIdTrib, g.version, g.folio
                        FROM
                            Dat_General g INNER JOIN
                            Cat_Emisor e ON g.id_Emisor = e.IDEEMI INNER JOIN
                            Dat_DOMEMIEXP d ON g.id_EmisorExp = d.IDEDOMEMIEXP INNER JOIN
                            Cat_Receptor r ON g.id_Receptor = r.IDEREC INNER JOIN
                            Dat_Pagos p ON g.idComprobante = p.id_Comprobante LEFT OUTER JOIN
                            Dat_HabitacionEvento he ON he.id_Comprobante = g.idComprobante
                        WHERE g.idComprobante = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", idFactura);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    var formaPago = Session["CfdiVersion"].ToString().Equals("3.3") ? "PUE" : dr["formapago"].ToString();
                    versionXmlOriginal = dr["version"].ToString();
                    folioReservacion = dr["folioReservacion"].ToString();
                    motivo = "ESTA NOTA DE CREDITO AFECTA A LA FACTURA " + dr["serie"].ToString() + dr["folio"] + ". " + tbMotivo.Text;
                    if (string.IsNullOrEmpty(folioReservacion)) { folioReservacion = dr["noTicket"].ToString(); }
                    var moneda = dr["moneda"].ToString();
                    var tipoCambio = dr["tipoCambio"].ToString();
                    if (string.IsNullOrEmpty(moneda))
                    {
                        moneda = "MXN";
                    }
                    txt.SetComprobanteCfdi(ddlSerie.SelectedItem.Text, "", Localization.Now.ToString("s"), formaPago, dr["condicionesDePago"].ToString(), dr["subTotal"].ToString(), dr["totalDescuento"].ToString(), dr["motivoDescuento"].ToString(), tipoCambio, moneda, dr["total"].ToString(), dr["tipoDoc"].ToString(), dr["metodoPago"].ToString(), dr["lugarExpedicion"].ToString(), dr["numCtaPago"].ToString(), dr["numeroAutorizacion"].ToString() + ":01", dr["serie"].ToString(), Localization.Parse(dr["fecha"].ToString()).ToString("s"), dr["total"].ToString(), dr["codDoc"].ToString(), motivo, dr["otrosCargos"].ToString(), dr["totalAPagar"].ToString(), dr["observaciones"].ToString());
                    txt.SetEmisorCfdi(dr["NOMEMI"].ToString(), dr["RFCEMI"].ToString(), dr["curpE"].ToString(), dr["telE"].ToString(), dr["mailE"].ToString(), dr["etipoE"].ToString(), dr["regimenE"].ToString(), dr["contE"].ToString());
                    txt.SetEmisorDomCfdi(dr["calleE"].ToString(), dr["extE"].ToString(), dr["intE"].ToString(), dr["colE"].ToString(), dr["locE"].ToString(), dr["refE"].ToString(), dr["munE"].ToString(), dr["edoE"].ToString(), dr["paisE"].ToString(), dr["cpE"].ToString());
                    txt.SetEmisorExpCfdi(dr["calleExp"].ToString(), dr["extExp"].ToString(), dr["intExp"].ToString(), dr["colExp"].ToString(), dr["locExp"].ToString(), dr["refExp"].ToString(), dr["munExp"].ToString(), dr["esoExp"].ToString(), dr["paisExp"].ToString(), dr["cpExp"].ToString(), dr["claveSucursal"].ToString());
                    var UsoCfdi = ""; //dr["usoCfdi"].ToString();
                    if (string.IsNullOrEmpty(UsoCfdi)) { UsoCfdi = ddlUsoCFDI.SelectedValue.ToString();} // UsoCfdi = "P01";
                    txt.SetReceptorCfdi(dr["NOMREC"].ToString(), dr["RFCREC"].ToString(), dr["curpR"].ToString(), dr["telR"].ToString(), dr["mailR"].ToString(), dr["tel2R"].ToString(), dr["denR"].ToString(), dr["contR"].ToString(), dr["numRegIdTrib"].ToString(), UsoCfdi);
                    txt.SetReceptorDomCfdi(dr["calleR"].ToString(), dr["extR"].ToString(), dr["intR"].ToString(), dr["colR"].ToString(), dr["locR"].ToString(), dr["refR"].ToString(), dr["munR"].ToString(), dr["edoR"].ToString(), dr["paisR"].ToString(), dr["cpR"].ToString());
                    decimal.TryParse(CerosNull(dr["iva16"].ToString()), out iva16);
                    decimal.TryParse(CerosNull(dr["propina"].ToString()), out propina);
                    sAmbiente = dr["ambiente"].ToString();
                    if (!(dr["tipoHE"] is DBNull) && dr["tipoHE"] != null && !string.IsNullOrEmpty(dr["tipoHE"].ToString()))
                    {
                        switch (dr["tipoHE"].ToString())
                        {
                            case "1":
                                txt.SetInfoAdicionalHabitacionCfdi(dr["huesped"].ToString(), folioReservacion, dr["noHabitacion"].ToString(), dr["fechaLlegada"].ToString(), dr["fechaSalida"].ToString(), dr["referencia"].ToString(), dr["tipohabitacion"].ToString(), dr["propina"].ToString());
                                break;
                            case "2":
                                txt.SetInfoAdicionalEventoCfdi(dr["huesped"].ToString(), folioReservacion, dr["fechaLlegada"].ToString(), dr["propina"].ToString());
                                break;
                            default:
                                break;
                        }
                    }
                    else if (!string.IsNullOrEmpty(folioReservacion))
                    {
                        txt.SetInfoAdicionalRestauranteCfdi(CerosNull(propina.ToString()), folioReservacion);
                    }
                }
                _db.Desconectar();
                sql = @"SELECT
                             ISNULL(SUM(it.valor), 0.00) as impTras, ISNULL(SUM(ir.valorRetenido), 0.00) as impRets
                        FROM
                            Dat_General g LEFT OUTER JOIN
                            Dat_TotalConImpuestos it ON it.id_Comprobante = g.idComprobante LEFT OUTER JOIN
                            Dat_ImpuestosRetenciones ir ON ir.numDocSustento = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", idFactura);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    txt.SetCantidadImpuestosCfdi(dr["impRets"].ToString(), dr["impTras"].ToString(), CerosNull(iva16.ToString()));
                }
                _db.Desconectar();
                sql = @"SELECT
                            ir.tipo as impuesto, ir.valorRetenido as importe, ir.tipoFactor
                        FROM
                            Dat_ImpuestosRetenciones ir INNER JOIN
                            Dat_General g on ir.numDocSustento = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", idFactura);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    txt.AgregaImpuestoRetencionCfdi(dr["impuesto"].ToString(), dr["importe"].ToString(), dr["tipoFactor"].ToString());
                }
                _db.Desconectar();
                sql = @"SELECT
                            CASE
                                it.codigo
                                WHEN '1' THEN 'IVA'
                                WHEN '2' THEN 'IEPS'
                            END AS impuesto, it.tarifa as tasa, it.valor as importe, it.tipoFactor
                        FROM
                            Dat_TotalConImpuestos it INNER JOIN
                            Dat_General g on it.id_Comprobante = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", idFactura);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    txt.AgregaImpuestoTrasladoCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString(), dr["tipoFactor"].ToString());
                }
                _db.Desconectar();
                #region Conceptos
                if (Session["CfdiVersion"].ToString().Equals("3.3") && versionXmlOriginal.Equals("3.3"))
                {
                    sql = "SELECT p.dirdocs, a.XMLARC FROM Dat_General g INNER JOIN Dat_Archivos a ON g.idComprobante = a.IDEFAC, Par_ParametrosSistema p WHERE g.idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        var rutaXml = dr["dirdocs"].ToString().Replace("docus", "") + @"\" + dr["XMLARC"].ToString();
                        var fileXml = new FileInfo(rutaXml);
                        if (fileXml.Exists)
                        {
                            var xDoc = new XmlDocument();
                            xDoc.Load(fileXml.FullName);
                            var nodosConceptos = xDoc.GetElementsByTagName("cfdi:Concepto").Cast<XmlNode>().ToList();
                            for (int i = 0; i < nodosConceptos.Count; i++)
                            {
                                var concepto = nodosConceptos[i];
                                var cantidad = "";
                                var unidad = "";
                                var numeroIdentificacion = "";
                                var descripcion = "";
                                var valorUnitario = "";
                                var importe = "";
                                var descuento = "";
                                var claveProdServ = "";
                                var claveUnidad = "";
                                try { cantidad = concepto.Attributes["Cantidad"].Value; } catch (Exception ex) { }
                                try { unidad = concepto.Attributes["Unidad"].Value; } catch (Exception ex) { }
                                try { numeroIdentificacion = concepto.Attributes["NoIdentificacion"].Value; } catch (Exception ex) { }
                                try { descripcion = concepto.Attributes["Descripcion"].Value; } catch (Exception ex) { }
                                try { valorUnitario = concepto.Attributes["ValorUnitario"].Value; } catch (Exception ex) { }
                                try { importe = concepto.Attributes["Importe"].Value; } catch (Exception ex) { }
                                try { descuento = concepto.Attributes["Descuento"].Value; } catch (Exception ex) { }
                                //try { claveProdServ = concepto.Attributes["ClaveProdServ"].Value; } catch (Exception ex) { }
                                //try { claveUnidad = concepto.Attributes["ClaveUnidad"].Value; } catch (Exception ex) { }
                                claveProdServ = "84111506";
                                claveUnidad = "ACT";
                                txt.AgregaConceptoCfdi(cantidad, unidad, numeroIdentificacion, descripcion, valorUnitario, importe, descuento, claveProdServ, claveUnidad, i.ToString());
                                if (concepto.HasChildNodes)
                                {
                                    var nsmgr = new XmlNamespaceManager(xDoc.NameTable);
                                    nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                    var trasladosConcepto = concepto.SelectNodes("cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado", nsmgr).Cast<XmlNode>().ToList();
                                    var retencionesConcepto = concepto.SelectNodes("cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion", nsmgr).Cast<XmlNode>().ToList();
                                    if (trasladosConcepto != null && trasladosConcepto.Count > 0)
                                    {
                                        foreach (var impuesto in trasladosConcepto)
                                        {
                                            var baseC = "";
                                            var impuestoC = "";
                                            var tipoFactorC = "";
                                            var tasaOCuotaC = "";
                                            var importeC = "";
                                            try { baseC = impuesto.Attributes["Base"].Value; } catch (Exception ex) { }
                                            try { impuestoC = impuesto.Attributes["Impuesto"].Value; } catch (Exception ex) { }
                                            try { tipoFactorC = impuesto.Attributes["TipoFactor"].Value; } catch (Exception ex) { }
                                            try { tasaOCuotaC = impuesto.Attributes["TasaOCuota"].Value; } catch (Exception ex) { }
                                            try { importeC = impuesto.Attributes["Importe"].Value; } catch (Exception ex) { }
                                            txt.AgregaConceptoImpuestoCfdi(false, baseC, impuestoC, tipoFactorC, tasaOCuotaC, importeC, i.ToString());
                                        }
                                    }
                                    if (retencionesConcepto != null && retencionesConcepto.Count > 0)
                                    {
                                        foreach (var impuesto in retencionesConcepto)
                                        {
                                            var baseC = "";
                                            var impuestoC = "";
                                            var tipoFactorC = "";
                                            var tasaOCuotaC = "";
                                            var importeC = "";
                                            try { baseC = impuesto.Attributes["Base"].Value; } catch (Exception ex) { }
                                            try { impuestoC = impuesto.Attributes["Impuesto"].Value; } catch (Exception ex) { }
                                            try { tipoFactorC = impuesto.Attributes["TipoFactor"].Value; } catch (Exception ex) { }
                                            try { tasaOCuotaC = impuesto.Attributes["TasaOCuota"].Value; } catch (Exception ex) { }
                                            try { importeC = impuesto.Attributes["Importe"].Value; } catch (Exception ex) { }
                                            txt.AgregaConceptoImpuestoCfdi(true, baseC, impuestoC, tipoFactorC, tasaOCuotaC, importeC, i.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    _db.Desconectar();
                }
                else if (Session["CfdiVersion"].ToString().Equals("3.3") && versionXmlOriginal.Equals("3.2"))
                {
                    var importeConcepto = "";
                    var descuentoConcepto = "";
                    var baseImpuesto = "";
                    sql = "SELECT SUM(precioTotalSinImpuestos) AS importe, SUM(descuento) AS descuento FROM Dat_Detalles INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        importeConcepto = ControlUtilities.CerosNull(dr["importe"].ToString());
                        descuentoConcepto = ControlUtilities.CerosNull(dr["descuento"].ToString());
                        baseImpuesto = (decimal.Parse(importeConcepto) - decimal.Parse(descuentoConcepto)).ToString();
                    }
                    _db.Desconectar();
                    txt.AgregaConceptoCfdi("1", "Actividad", "", motivo, importeConcepto, importeConcepto, descuentoConcepto, "84111506", "ACT", "1");
                    sql = @"SELECT
                            ir.tipo as impuesto, ir.valorRetenido as importe, ir.tipoFactor
                        FROM
                            Dat_ImpuestosRetenciones ir INNER JOIN
                            Dat_General g on ir.numDocSustento = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        txt.AgregaConceptoImpuestoCfdi(true, baseImpuesto, dr["impuesto"].ToString(), dr["tipoFactor"].ToString(), "", dr["importe"].ToString(), "1");
                    }
                    _db.Desconectar();
                    sql = @"SELECT
                            CASE
                                it.codigo
                                WHEN '1' THEN 'IVA'
                                WHEN '2' THEN 'IEPS'
                            END AS impuesto, it.tarifa as tasa, it.valor as importe, it.tipoFactor
                        FROM
                            Dat_TotalConImpuestos it INNER JOIN
                            Dat_General g on it.id_Comprobante = g.idComprobante
                        WHERE
                            g.idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        txt.AgregaConceptoImpuestoCfdi(false, baseImpuesto, dr["impuesto"].ToString(), dr["tipoFactor"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString(), "1");
                    }
                    _db.Desconectar();
                }
                else
                {
                    sql = "SELECT idDetalles, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial, claveProdServ, claveUnidad, descuento FROM Dat_Detalles INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        var sqlResults = new object[dr.FieldCount];
                        dr.GetValues(sqlResults);
                        conceptos.Add(sqlResults);
                    }
                    _db.Desconectar();
                    foreach (var concepto in conceptos)
                    {
                        txt.AgregaConceptoCfdi(concepto[4].ToString(), concepto[6].ToString(), concepto[1].ToString(), concepto[2].ToString(), concepto[3].ToString(), concepto[5].ToString(), concepto[10].ToString(), concepto[8].ToString(), concepto[9].ToString());
                        if (!string.IsNullOrEmpty(concepto[7].ToString())) { txt.SetPredialConceptoCfdi(concepto[7].ToString()); }
                        sql = "SELECT idDetallesAduana, numero, fecha, aduana FROM Dat_DetallesAduana WHERE id_Detalles = @idDet";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@idDet", concepto[0].ToString());
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            txt.AgregaAduaneraConceptoCfdi(dr["numero"].ToString(), dr["fecha"].ToString(), dr["aduana"].ToString());
                        }
                        _db.Desconectar();
                        var partes = new List<object[]>();
                        sql = "SELECT idDetallesParte, cantidad, unidad, noIdentificacion, descripcion, valorUnitario, importe FROM Dat_DetallesParte WHERE id_Detalles = @idDet";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@idDet", concepto[0].ToString());
                        dr = _db.EjecutarConsulta();
                        while (dr.Read())
                        {
                            var sqlResults = new object[dr.FieldCount];
                            dr.GetValues(sqlResults);
                            partes.Add(sqlResults);
                        }
                        _db.Desconectar();
                        foreach (var parte in partes)
                        {
                            txt.AgregaParteConceptoCfdi(parte[1].ToString(), parte[2].ToString(), parte[3].ToString(), parte[4].ToString(), parte[5].ToString(), parte[6].ToString());
                            sql = "SELECT idDetallesAduana, numero, fecha, aduana FROM Dat_DetallesAduana WHERE id_DetallesParte = @idDet";
                            _db.Conectar();
                            _db.CrearComando(sql);
                            _db.AsignarParametroCadena("@idPart", parte[0].ToString());
                            dr = _db.EjecutarConsulta();
                            while (dr.Read())
                            {
                                txt.AgregaParteAduaneraConceptoCfdi(dr["numero"].ToString(), dr["fecha"].ToString(), dr["aduana"].ToString());
                            }
                            _db.Desconectar();
                        }
                    }
                }
                #endregion
                sql = "SELECT idImpLocal FROM Dat_MX_ImpLocales INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", idFactura);
                dr = _db.EjecutarConsulta();
                var tieneLocales = dr.HasRows;
                _db.Desconectar();
                if (tieneLocales)
                {
                    sql = @"SELECT totalRetImpLocales, totalTraImpLocales FROM Dat_General WHERE idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        txt.SetImpuestosLocalesCfdi(dr["totalRetImpLocales"].ToString(), dr["totalTraImpLocales"].ToString());
                    }
                    _db.Desconectar();
                    sql = "SELECT nombre as impuesto, tasadeRetencion as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeRetencion IS NOT NULL AND tasadeTraslado IS NULL AND idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        txt.AgregaRetencionLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                    }
                    _db.Desconectar();
                    sql = "SELECT nombre as impuesto, tasadeTraslado as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeTraslado IS NOT NULL AND tasadeRetencion IS NULL AND idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        txt.AgregaTrasladoLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                    }
                    _db.Desconectar();
                }
                var ambiente = false;
                switch (sAmbiente)
                {
                    case "1":
                        ambiente = false;
                        break;
                    case "2":
                        ambiente = true;
                        break;
                    default:
                        break;
                }
                var txtInvoice = txt.ConstruyeTxtCfdi();
                var randomMs = new Random().Next(1000, 5000);
                System.Threading.Thread.Sleep(randomMs);
                var coreMx = new wsEmision.WsEmision { Timeout = (1800 * 5000) };
                var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, "04", false, true, "", "");
                if (result != null)
                {
                    //sql = @"UPDATE Dat_General SET estado = 4, saldo = total, pagoAplicado = total, saldoPendiente = 0.00, estadoPago = 1, id_Empleado_Canc = @idCanc WHERE idComprobante = @ID";
                    //_db.Conectar();
                    //_db.CrearComando(sql);
                    //_db.AsignarParametroCadena("@ID", idComprobanteOriginal);
                    //_db.AsignarParametroCadena("@idCanc", _idUser);
                    //_db.EjecutarConsulta1();
                    //_db.Desconectar();
                    sql = @"UPDATE Dat_General SET estado = 4, id_Empleado_Canc = @idCanc WHERE idComprobante = @ID";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@ID", idFactura);
                    _db.AsignarParametroCadena("@idCanc", _idUser);
                    _db.EjecutarConsulta1();
                    _db.Desconectar();
                    try
                    {
                        var idTramas = new List<string>();
                        _db.Conectar();
                        _db.CrearComando("select idTrama from log_trama where tipo = 4 and (noReserva = @folioReservacion or noTicket = @folioReservacion)");
                        _db.AsignarParametroCadena("@folioReservacion", folioReservacion);
                        _db.AsignarParametroCadena("@folioReservacion", folioReservacion);
                        dr = _db.EjecutarConsulta();
                        while (dr.Read()) { idTramas.Add(dr["idTrama"].ToString()); }
                        _db.Desconectar();
                        if (idTramas.Count > 1)
                        {
                            var idOriginal = idTramas.OrderBy(t => t).First();
                            if (!string.IsNullOrEmpty(idOriginal))
                            {
                                idTramas.Remove(idOriginal);
                                var tramasBorrar = string.Join(",", idTramas);
                                _db.Conectar();
                                _db.CrearComando("DELETE FROM Log_Trama WHERE idTrama IN (" + tramasBorrar + ")");
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                                _db.Conectar();
                                _db.CrearComando("UPDATE Log_Trama SET observaciones = 'ExtranetOk', folio = 0 WHERE idTrama = @idTrama");
                                _db.AsignarParametroCadena("@idTrama", idOriginal);
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                            }
                        }
                    }
                    catch { }
                    var xDoc = new XmlDocument();
                    xDoc.LoadXml(result.OuterXml);
                    Session["uuidCreado"] = GetAtributte(xDoc, "UUID", "tfd:TimbreFiscalDigital");
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante se ha generado satisfactoriamente", 2, null);
                    Response.Redirect("~/Documentos.aspx", false);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se creó correctamente<br/>" + coreMx.ObtenerMensaje(), 4, null);
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se creó correctamente<br/>" + ex.Message, 4, null);
                if (ddlBusq.SelectedValue.Equals("1"))
                {
                }
                else if (ddlBusq.SelectedValue.Equals("2"))
                {
                    ddlBusq.SelectedValue = "1";
                }
            }
        }

        /// <summary>
        /// Gets the atributte.
        /// </summary>
        /// <param name="XmlTimbrado">The XML timbrado.</param>
        /// <param name="name">The name.</param>
        /// <param name="node">The node.</param>
        /// <returns>System.String.</returns>
        private string GetAtributte(XmlDocument XmlTimbrado, string name, string node)
        {
            string result = null;
            if (XmlTimbrado != null)
            {
                XmlNodeList listaNodos = XmlTimbrado.GetElementsByTagName(node);
                foreach (XmlElement nodo in listaNodos)
                {
                    if (nodo.HasAttributes && nodo.HasAttribute(name))
                    {
                        result = nodo.GetAttribute(name);
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Ceroses the null.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>System.String.</returns>
        private string CerosNull(string a)
        {
            decimal b;
            var cifra = (!string.IsNullOrEmpty(a) ? a : "").Replace(",", "").Trim();
            var result = string.Format("{0:0.00}", Convert.ToDecimal(string.IsNullOrEmpty(cifra) || !decimal.TryParse(cifra, out b) || b < 0 ? "0.00" : cifra));
            return result;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSerie control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sql = "SELECT c.codigo, c.descripcion AS ambiente FROM Cat_Catalogo1_C c INNER JOIN Cat_Series s ON s.ambiente = c.codigo WHERE c.tipo = 'Ambiente' AND s.idSerie = @idSerie";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idSerie", ddlSerie.SelectedValue);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                hfambiente.Value = dr["codigo"].ToString();
                tbAmbiente.Text = dr["ambiente"].ToString();
            }
            _db.Desconectar();
        }
    }
}
