// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 05-22-2017
// ***********************************************************************
// <copyright file="editarComprobante.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using Control;
using Datos;
using System.Xml;

namespace DataExpressWeb
{
    /// <summary>
    /// Class EditarComprobante.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class EditarComprobante : Page
    {

        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";
        /// <summary>
        /// The _log
        /// </summary>
        private static Log _log;
        /// <summary>
        /// The _id comprobante
        /// </summary>
        private static string _idComprobante = "";
        /// <summary>
        /// The _ish p
        /// </summary>
        private static decimal _ishP;

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
            if (Session["idUser"] != null)
            {
                _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                _log = new Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                SqlDataRFC.ConnectionString = _db.CadenaConexion;
                SqlDataConcTemp.ConnectionString = _db.CadenaConexion;
                SqlDataReceptores.ConnectionString = _db.CadenaConexion;
                _idUser = Session["idUser"].ToString();
                if (!Page.IsPostBack)
                {
                    #region Giro Empresarial

                    if (Session["IDGIRO"] != null)
                    {
                        if (Session["IDGIRO"].ToString().Contains("1"))
                        {
                            #region Hotel

                            trOtrosCargos.Visible = true;
                            rowPropina.Visible = true;
                            _db.Conectar();
                            _db.CrearComando(@"SELECT ISNULL(SUM(valor), 0.00) AS ISH FROM Cat_CatImpuestos_C WHERE tipo = '03' AND descripcion LIKE '%ISH%'");
                            var dr = _db.EjecutarConsulta();
                            var sish = "";
                            if (dr.Read())
                            {
                                sish = CerosNull(dr["ISH"].ToString());
                            }
                            decimal.TryParse(sish, out _ishP);
                            _db.Desconectar();
                            var hasIsh = _ishP > 0;
                            trISProp.Visible = hasIsh;
                            lblISHPrer.Text = CerosNull(_ishP.ToString());
                            divDescuentoTot.Visible = false;
                            rowDenomSocial.Visible = false;
                            #endregion
                        }
                        else if (Session["IDGIRO"].ToString().Contains("2"))
                        {
                            #region Restaurante

                            trOtrosCargos.Visible = true;
                            rowPropina.Visible = true;
                            rowDenomSocial.Visible = false;
                            #endregion
                        }
                        else if (Session["IDGIRO"].ToString().Contains("3"))
                        {
                            #region Empresa



                            #endregion
                        }
                    }

                    #endregion
                    var id = Request.QueryString["id"];
                    if (!string.IsNullOrEmpty(id))
                    {
                        divBusqueda.Visible = false;
                        var tryLoad = LoadData(id);
                        if (!tryLoad.Key)
                        {
                            (Master as SiteMaster).MostrarAlerta(this, tryLoad.Value, 4, null, "history.go(-1);");
                        }
                    }
                    else
                    {
                        divBusqueda.Visible = true;
                    }
                }

            }
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        private void ClearData()
        {
            _idComprobante = "";
            Llenarlista("");
            LlenarGridView();
            tbSubtotal.Text = "";
            tbIva16.Text = "";
            lblISHPrer.Text = "";
            tbISH.Text = "";
            tbTotalFac.Text = "";
            tbPropina.Text = "";
            tbDescuento.Text = "";
            tbOtrosCargos.Text = "";
            tbTotal.Text = "";
            tbCodDoc.Text = "";
            tbAmbiente.Text = "";
            tbFormaPago.Text = "";
            tbMetodoPago.Text = "";
            tbNoCta.Text = "";
            tbCantLetra.Text = "";
            tbLugarExp.Text = "";
            tbObservaciones.Text = "";
            divDatos.Visible = false;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="idComprobante">The identifier comprobante.</param>
        /// <param name="serie">The serie.</param>
        /// <param name="folio">The folio.</param>
        /// <param name="idEmisor">The identifier emisor.</param>
        /// <returns>KeyValuePair&lt;System.Boolean, System.String&gt;.</returns>
        private KeyValuePair<bool, string> LoadData(string idComprobante = "", string serie = "", string folio = "", string idEmisor = "")
        {
            ClearData();
            var sqlReceptor = @"SELECT TOP 1
                            p.formapago, g.subTotal, g.totalDescuento, g.cantletras, g.total, g.metodoPago, g.lugarExpedicion, p.numCtaPago, g.numeroAutorizacion, g.serie, g.codDoc, ccCodDoc.descripcion AS descDoc, g.idComprobante, e.IDEEMI, r.IDEREC, CONVERT(VARCHAR,CAST((CASE WHEN g.IVA12 IS NULL OR g.IVA12 = 0.00 THEN (SELECT ISNULL(SUM(dt.valor), 0.00) FROM Dat_TotalConImpuestos dt INNER JOIN Cat_Catalogo1_C ci ON ci.codigo = dt.codigo AND ci.tipo = 'Impuesto Trasladado' AND ci.descripcion = 'IVA' AND dt.id_Comprobante = g.idComprobante) ELSE g.IVA12 END) AS money), 1) AS iva16, CONVERT(VARCHAR,CAST((SELECT ISNULL(SUM(dt2.importe), 0.00) FROM Dat_MX_ImpLocales dt2 WHERE dt2.nombre LIKE '%ISH%' AND dt2.id_Comprobante = g.idComprobante) AS money), 1) AS valor_ish, g.propina, g.ambiente, ccAmbiente.descripcion AS descAmbiente, g.idComprobante, g.importeAPagar, g.cargoxservicio as otrosCargos, g.observaciones, he.tipo AS tipoHE, g.folioReservacion, he.noHabitacion, he.fechaLlegada, he.fechaSalida, he.huesped, ISNULL(g.mensajeSAT, log.descripcion) AS mensajeSAT
                        FROM
                            Dat_General g INNER JOIN
							Cat_Catalogo1_C ccCodDoc ON g.codDoc = ccCodDoc.codigo AND ccCodDoc.tipo = 'Comprobante' INNER JOIN
							Cat_Catalogo1_C ccAmbiente ON g.ambiente = ccAmbiente.codigo AND ccAmbiente.tipo = 'Ambiente' INNER JOIN
                            Cat_Emisor e ON g.id_Emisor = e.IDEEMI INNER JOIN
                            Dat_DOMEMIEXP d ON g.id_EmisorExp = d.IDEDOMEMIEXP INNER JOIN
                            Cat_Receptor r ON g.id_Receptor = r.IDEREC LEFT OUTER JOIN
                            Dat_Pagos p ON g.idComprobante = p.id_Comprobante LEFT OUTER JOIN
							Dat_HabitacionEvento he ON he.id_Comprobante = g.idComprobante LEFT OUTER JOIN
							Logs log ON (log.comprobanteAsociado = g.idComprobante OR log.tramaAsociada = g.idTrama)
                        WHERE (g.idComprobante = @id OR (g.serie = @serie AND g.folio = @folio)) AND g.estado = '0' AND (g.tipo = 'E' OR g.tipo = 'N')" + (!string.IsNullOrEmpty(idEmisor) ? " AND e.IDEEMI = @idEmi" : "");
            _db.Conectar();
            _db.CrearComando(sqlReceptor);
            _db.AsignarParametroCadena("@id", idComprobante);
            _db.AsignarParametroCadena("@serie", serie);
            _db.AsignarParametroCadena("@folio", folio);
            if (!string.IsNullOrEmpty(idEmisor))
            {
                _db.AsignarParametroCadena("@idEmi", idEmisor);
            }
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                _idComprobante = dr["idComprobante"].ToString();
                var idRec = dr["IDEREC"].ToString();
                tbSubtotal.Text = CerosNull(dr["subTotal"].ToString());
                tbIva16.Text = CerosNull(dr["iva16"].ToString());
                lblISHPrer.Text = CerosNull(_ishP.ToString());
                tbISH.Text = CerosNull(dr["valor_ish"].ToString());
                tbTotalFac.Text = CerosNull(dr["total"].ToString());
                tbPropina.Text = CerosNull(dr["propina"].ToString());
                tbDescuento.Text = CerosNull(dr["totalDescuento"].ToString());
                tbOtrosCargos.Text = CerosNull(dr["otrosCargos"].ToString());
                tbTotal.Text = CerosNull(dr["importeAPagar"].ToString());
                tbCodDoc.Text = dr["descDoc"].ToString();
                tbAmbiente.Text = dr["descAmbiente"].ToString();
                tbFormaPago.Text = dr["formapago"].ToString();
                tbMetodoPago.Text = dr["metodoPago"].ToString();
                tbNoCta.Text = dr["numCtaPago"].ToString();
                tbCantLetra.Text = dr["cantletras"].ToString();
                tbLugarExp.Text = dr["lugarExpedicion"].ToString();
                tbObservaciones.Text = dr["observaciones"].ToString();
                lblMensajeSAT.Text = dr["mensajeSAT"].ToString();
                alertSat.Visible = !string.IsNullOrEmpty(lblMensajeSAT.Text);
                _db.Desconectar();
                divDatos.Visible = true;
                Llenarlista(idRec);
                LlenarGridView();
                _db.Desconectar();
                return new KeyValuePair<bool, string>(true, "");
            }
            else
            {
                _db.Desconectar();
                return new KeyValuePair<bool, string>(false, "No existe un comprobante no autorizado con los datos especificados.");
            }
        }

        /// <summary>
        /// Llenars the grid view.
        /// </summary>
        private void LlenarGridView()
        {
            SqlDataConcTemp.SelectParameters["idComprobante"].DefaultValue = _idComprobante;
            SqlDataConcTemp.DataBind();
            gvConceptos.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the FinishButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
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
                var codDoc = "";
                var manual = true;
                sql = @"SELECT
                            p.formapago, p.condicionesDePago, g.subTotal, g.totalDescuento, g.motivoDescuento, g.tipoCambio, g.moneda, g.total, g.tipoDeComprobante as tipoDoc, g.metodoPago, g.lugarExpedicion, p.numCtaPago, g.numeroAutorizacion, g.serie, g.fecha, g.total, g.codDoc, e.NOMEMI, e.RFCEMI, e.curp as curpE, e.telefono as telE, e.email as mailE, e.EmpresaTipo as etipoE, e.regimenFiscal as regimenE, e.obligadoContabilidad as contE, e.dirMatriz AS calleE, e.noExterior as extE, e.noInterior as intE, e.colonia as colE, e.localidad as locE, e.referencia as refE, e.municipio as munE, e.estado as edoE, e.pais as paisE, e.codigoPostal as cpE, d.dirEstablecimientos as calleExp, d.noExterior as extExp, d.noInterior as intExp, d.colonia as colExp, d.localidad as locExp, d.referencia as refExp, d.municipio as munExp, d.estado as esoExp, d.pais as paisExp, d.codigoPostal as cpExp, r.NOMREC, r.RFCREC, r.curp as curpR, r.telefono as telR, r.email as mailR, r.telefono2 as tel2R, r.denominacionSocial as denR, r.obligadoContabilidad as contR, r.domicilio as calleR, r.noExterior as extR, r.noInterior as intR, r.colonia as colR, r.localidad as locR, r.referencia as refR, r.municipio as munR, r.estado as edoR, r.pais as paisR, r.codigoPostal as cpR, IVA12 as iva16, g.propina, g.ambiente, g.idComprobante, g.importeAPagar, g.cargoxservicio as otrosCargos, g.observaciones, he.tipo AS tipoHE, g.folioReservacion, he.noHabitacion, he.fechaLlegada, he.fechaSalida, he.huesped, g.folio, t.tipo AS tipoEmision, g.estab AS claveSucursal
                        FROM
                            Dat_General g INNER JOIN
                            Cat_Emisor e ON g.id_Emisor = e.IDEEMI INNER JOIN
                            Dat_DOMEMIEXP d ON g.id_EmisorExp = d.IDEDOMEMIEXP INNER JOIN
                            Cat_Receptor r ON g.id_Receptor = r.IDEREC INNER JOIN
							Log_Trama t ON g.idTrama = t.idTrama INNER JOIN
                            Dat_Pagos p ON g.idComprobante = p.id_Comprobante LEFT OUTER JOIN
							Dat_HabitacionEvento he ON he.id_Comprobante = g.idComprobante
                        WHERE g.idComprobante = @id";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@id", _idComprobante);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    codDoc = dr["codDoc"].ToString();
                    manual = dr["tipoEmision"].ToString().Equals("2");
                    txt.SetComprobanteCfdi(dr["serie"].ToString(), dr["folio"].ToString(), Localization.Now.ToString("s"), dr["formapago"].ToString(), dr["condicionesDePago"].ToString(), dr["subTotal"].ToString(), dr["totalDescuento"].ToString(), dr["motivoDescuento"].ToString(), dr["tipoCambio"].ToString(), dr["moneda"].ToString(), dr["total"].ToString(), dr["tipoDoc"].ToString(), dr["metodoPago"].ToString(), dr["lugarExpedicion"].ToString(), dr["numCtaPago"].ToString(), dr["numeroAutorizacion"].ToString(), dr["serie"].ToString(), Localization.Parse(dr["fecha"].ToString()).ToString("s"), dr["total"].ToString(), dr["codDoc"].ToString(), "", dr["otrosCargos"].ToString(), dr["importeAPagar"].ToString(), dr["observaciones"].ToString());
                    txt.SetEmisorCfdi(dr["NOMEMI"].ToString(), dr["RFCEMI"].ToString(), dr["curpE"].ToString(), dr["telE"].ToString(), dr["mailE"].ToString(), dr["etipoE"].ToString(), dr["regimenE"].ToString(), dr["contE"].ToString());
                    txt.SetEmisorDomCfdi(dr["calleE"].ToString(), dr["extE"].ToString(), dr["intE"].ToString(), dr["colE"].ToString(), dr["locE"].ToString(), dr["refE"].ToString(), dr["munE"].ToString(), dr["edoE"].ToString(), dr["paisE"].ToString(), dr["cpE"].ToString());
                    txt.SetEmisorExpCfdi(dr["calleExp"].ToString(), dr["extExp"].ToString(), dr["intExp"].ToString(), dr["colExp"].ToString(), dr["locExp"].ToString(), dr["refExp"].ToString(), dr["munExp"].ToString(), dr["esoExp"].ToString(), dr["paisExp"].ToString(), dr["cpExp"].ToString(), dr["claveSucursal"].ToString());
                    txt.SetReceptorCfdi(tbRazonSocialRec.Text, tbRfcRec.Text, dr["curpR"].ToString(), dr["telR"].ToString(), dr["mailR"].ToString(), dr["tel2R"].ToString(), tbDenomSocialRec.Text, dr["contR"].ToString());
                    if (cbDomRec.Checked)
                    {
                        var isSucursal = !ddlSucRec.SelectedValue.Equals("0");
                        txt.SetReceptorDomCfdi(tbCalleRec.Text, tbNoExtRec.Text, tbNoIntRec.Text, tbColoniaRec.Text, dr["locR"].ToString(), dr["refR"].ToString(), tbMunicipioRec.Text, tbEstadoRec.Text, tbPaisRec.Text, tbCpRec.Text, isSucursal);
                    }
                    decimal.TryParse(CerosNull(dr["iva16"].ToString()), out iva16);
                    decimal.TryParse(CerosNull(dr["propina"].ToString()), out propina);
                    sAmbiente = dr["ambiente"].ToString();
                    if (!(dr["tipoHE"] is DBNull) && dr["tipoHE"] != null && !string.IsNullOrEmpty(dr["tipoHE"].ToString()))
                    {
                        switch (dr["tipoHE"].ToString())
                        {
                            case "1":
                                txt.SetInfoAdicionalHabitacionCfdi(dr["huesped"].ToString(), dr["folioReservacion"].ToString(), dr["noHabitacion"].ToString(), dr["fechaLlegada"].ToString(), dr["fechaSalida"].ToString());
                                break;
                            case "2":
                                txt.SetInfoAdicionalEventoCfdi(dr["huesped"].ToString(), dr["folioReservacion"].ToString(), dr["fechaLlegada"].ToString());
                                break;
                            default:
                                break;
                        }
                    }
                }
                _db.Desconectar();
                sql = @"SELECT
	                         ISNULL(SUM(it.valor), 0.00) as impTras, ISNULL(SUM(ir.valorRetenido), 0.00) as impRets
                        FROM
	                        Dat_General g INNER JOIN
							Dat_TotalConImpuestos it ON it.id_Comprobante = g.idComprobante INNER JOIN
							Dat_ImpuestosRetenciones ir ON ir.numDocSustento = g.idComprobante
						WHERE
							g.idComprobante = @id";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@id", _idComprobante);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    txt.SetCantidadImpuestosCfdi(dr["impRets"].ToString(), dr["impTras"].ToString(), CerosNull(iva16.ToString()));
                }
                _db.Desconectar();
                sql = @"SELECT
	                        ir.tipo as impuesto, ir.valorRetenido as importe
                        FROM
	                        Dat_ImpuestosRetenciones ir INNER JOIN
	                        Dat_General g on ir.numDocSustento = g.idComprobante
						WHERE
							g.idComprobante = @id";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@id", _idComprobante);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    txt.AgregaImpuestoRetencionCfdi(dr["impuesto"].ToString(), dr["importe"].ToString());
                }
                _db.Desconectar();
                sql = @"SELECT
	                        CASE
		                        it.codigo
		                        WHEN '1' THEN 'IVA'
		                        WHEN '2' THEN 'IEPS'
	                        END AS impuesto, it.tarifa as tasa, it.valor as importe
                        FROM
	                        Dat_TotalConImpuestos it INNER JOIN
	                        Dat_General g on it.id_Comprobante = g.idComprobante
						WHERE
							g.idComprobante = @id";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@id", _idComprobante);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    txt.AgregaImpuestoTrasladoCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                }
                _db.Desconectar();
                sql = "SELECT idDetalles, codigoPrincipal AS noId, descripcion, precioUnitario AS valorUnitario, cantidad, precioTotalSinImpuestos AS importe, unidad, ctaPredial FROM Dat_Detalles INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @id";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@id", _idComprobante);
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
                    txt.AgregaConceptoCfdi(concepto[4].ToString(), concepto[6].ToString(), concepto[1].ToString(), concepto[2].ToString(), concepto[3].ToString(), concepto[5].ToString());
                    if (!string.IsNullOrEmpty(concepto[7].ToString()))
                    {
                        txt.SetPredialConceptoCfdi(concepto[7].ToString());
                    }
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
                if (propina > 0)
                {
                    txt.SetInfoAdicionalRestauranteCfdi(CerosNull(propina.ToString()));
                }
                sql = "SELECT idImpLocal FROM Dat_MX_ImpLocales INNER JOIN Dat_General on id_Comprobante = idComprobante WHERE idComprobante = @id";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@id", _idComprobante);
                dr = _db.EjecutarConsulta();
                var tieneLocales = dr.HasRows;
                _db.Desconectar();
                if (tieneLocales)
                {
                    sql = @"SELECT totalRetImpLocales, totalTraImpLocales FROM Dat_General WHERE idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        txt.SetImpuestosLocalesCfdi(dr["totalRetImpLocales"].ToString(), dr["totalTraImpLocales"].ToString());
                    }
                    _db.Desconectar();
                    sql = "SELECT nombre as impuesto, tasadeRetencion as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeRetencion IS NOT NULL AND tasadeTraslado IS NULL AND idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        txt.AgregaRetencionLocalCfdi(dr["impuesto"].ToString(), dr["tasa"].ToString(), dr["importe"].ToString());
                    }
                    _db.Desconectar();
                    sql = "SELECT nombre as impuesto, tasadeTraslado as tasa, importe FROM Dat_MX_ImpLocales INNER JOIN Dat_General ON id_Comprobante = idComprobante WHERE tasadeTraslado IS NOT NULL AND tasadeRetencion IS NULL AND idComprobante = @id";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@id", _idComprobante);
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
                var coreMx = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                var result = coreMx.RecibeInfoTxt(txtInvoice, _idUser, Session["IDENTEMI"].ToString(), ambiente, codDoc, !manual, manual, "", "");
                if (result != null)
                {
                    try
                    {
                        _db.Conectar();
                        _db.CrearComando(@"DELETE FROM Dat_HabitacionEvento WHERE id_Comprobante = @id;DELETE FROM Dat_General WHERE idComprobante = @id");
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        _db.AsignarParametroCadena("@id", _idComprobante);
                        _db.EjecutarConsulta1();
                    }
                    catch { }
                    finally
                    {
                        _db.Desconectar();
                    }
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
            }
            _idComprobante = "";
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
        /// Handles the TextChanged event of the tbRfcRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRfcRec_TextChanged(object sender, EventArgs e)
        {
            Llenarlista(tbRfcRec.Text);
        }

        /// <summary>
        /// Llenarlistadoms the specified RFC record.
        /// </summary>
        /// <param name="rfcRec">The RFC record.</param>
        /// <param name="chkDom">if set to <c>true</c> [CHK DOM].</param>
        private void Llenarlistadom(string rfcRec, bool chkDom = true)
        {
            var sql = @"SELECT [domicilio]
                              ,[noExterior]
                              ,[noInterior]
                              ,[colonia]
                              ,[localidad]
                              ,[referencia]
                              ,[municipio]
                              ,[estado]
                              ,[pais]
                              ,[codigoPostal]
                          FROM [Cat_Receptor]
                          WHERE RFCREC=@rfc";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@rfc", rfcRec);
            var dr = _db.EjecutarConsulta();
            var control = dr.HasRows || !cbDomRec.Checked;
            tbCalleRec.ReadOnly = !cbDomRec.Checked;
            tbNoExtRec.ReadOnly = !cbDomRec.Checked;
            tbNoIntRec.ReadOnly = !cbDomRec.Checked;
            tbColoniaRec.ReadOnly = !cbDomRec.Checked;
            //tbLocRec.ReadOnly = !cbDomRec.Checked;
            //tbRefRec.ReadOnly = !cbDomRec.Checked;
            tbMunicipioRec.ReadOnly = !cbDomRec.Checked;
            tbEstadoRec.ReadOnly = !cbDomRec.Checked;
            tbPaisRec.ReadOnly = !cbDomRec.Checked;
            tbCpRec.ReadOnly = !cbDomRec.Checked;
            if (!cbDomRec.Checked)
            {
                btnPaisRec.Attributes["disabled"] = "disabled";
            }
            else
            {
                btnPaisRec.Attributes.Remove("disabled");
            }
            if (control && dr.Read())
            {
                tbCalleRec.Text = dr["domicilio"].ToString();
                tbNoExtRec.Text = dr["noExterior"].ToString();
                tbNoIntRec.Text = dr["noInterior"].ToString();
                tbColoniaRec.Text = dr["colonia"].ToString();
                //tbLocRec.Text = dr["localidad"].ToString();
                //tbRefRec.Text = dr["referencia"].ToString();
                tbMunicipioRec.Text = dr["municipio"].ToString();
                tbEstadoRec.Text = dr["estado"].ToString();
                tbPaisRec.Text = dr["pais"].ToString();
                tbCpRec.Text = dr["codigoPostal"].ToString();
                _db.Desconectar();
                _db.Conectar();
                _db.CrearComando(@"SELECT idSucursal, sucursal FROM Cat_Sucursales WHERE RFC = @RFC");
                _db.AsignarParametroCadena("@RFC", rfcRec);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    ddlSucRec.Items.Add(new ListItem(dr["sucursal"].ToString(), dr["idSucursal"].ToString()));
                }
                _db.Desconectar();
                if (chkDom)
                {
                    cbDomRec.Checked = true;
                    cbDomRec_CheckedChanged(null, null);
                }
            }
            else
            {
                _db.Desconectar();
                if (chkDom)
                {
                    cbDomRec.Checked = false;
                    cbDomRec_CheckedChanged(null, null);
                }
                tbCalleRec.Text = "";
                tbNoExtRec.Text = "";
                tbNoIntRec.Text = "";
                tbColoniaRec.Text = "";
                //tbLocRec.Text = "";
                //tbRefRec.Text = "";
                tbMunicipioRec.Text = "";
                tbEstadoRec.Text = "";
                tbPaisRec.Text = "";
                tbCpRec.Text = "";
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbDomRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void cbDomRec_CheckedChanged(object sender, EventArgs e)
        {
            RequiredFieldValidator14.Enabled = cbDomRec.Checked;
            RequiredFieldValidator15.Enabled = cbDomRec.Checked;
            RequiredFieldValidator16.Enabled = cbDomRec.Checked;
            RequiredFieldValidator23.Enabled = cbDomRec.Checked;
            RequiredFieldValidator27.Enabled = cbDomRec.Checked;
            ddlSucRec.Items.Clear();
            var itemNull = new ListItem("SELECCIONE", "0");
            itemNull.Selected = true;
            ddlSucRec.Items.Add(itemNull);
            Llenarlistadom(tbRfcRec.Text, false);
            ddlSucRec.Enabled = ddlSucRec.Items.Count > 1;
        }

        /// <summary>
        /// Llenarlistas the specified identifier record.
        /// </summary>
        /// <param name="idRec">The identifier record.</param>
        private void Llenarlista(string idRec)
        {
            var sql = @"SELECT [RFCREC]
                              ,[NOMREC]
                              ,[telefono]
                              ,[contribuyenteEspecial]
                              ,[obligadoContabilidad]
                              ,[tipoIdentificacionComprador]
                              ,[email]
                              ,[curp]
                              ,(CASE ISNULL(CONVERT(VARCHAR, metodoPago), '') WHEN '' THEN '99' ELSE ISNULL(cc.codigo, '99') END) AS metodoPago
                              ,[numCtaPago]
                              ,[denominacionSocial]
                              ,[telefono2]
                          FROM [Cat_Receptor] LEFT OUTER JOIN Cat_Catalogo1_C cc ON CONVERT(VARCHAR, metodoPago) = cc.codigo AND cc.tipo = 'MetodoPago'
                          WHERE IDEREC=@id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", idRec);
            var dr = _db.EjecutarConsulta();
            //tbRazonSocialRec.ReadOnly = dr.HasRows;
            //ddlMetodoPago.Enabled = !dr.HasRows;
            //tbDenomSocialRec.ReadOnly = dr.HasRows;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tbRfcRec.Text = dr["RFCREC"].ToString();
                    tbRazonSocialRec.Text = dr["NOMREC"].ToString();
                    tbDenomSocialRec.Text = dr["denominacionSocial"].ToString();
                }
            }
            else
            {
                tbRazonSocialRec.Text = "";
                tbDenomSocialRec.Text = "";
            }
            _db.Desconectar();
            Llenarlistadom(tbRfcRec.Text);
        }

        /// <summary>
        /// Handles the Click event of the lbBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">
        /// Debe introducir una serie y un folio
        /// or
        /// </exception>
        /// <exception cref="System.Exception">Debe introducir una serie y un folio
        /// or</exception>
        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbSerie.Text) || string.IsNullOrEmpty(tbFolio.Text))
                {
                    throw new Exception("Debe introducir una serie y un folio");
                }
                var tryLoad = LoadData("", tbSerie.Text, tbFolio.Text, ddlRFC.SelectedValue);
                if (!tryLoad.Key)
                {
                    throw new Exception(tryLoad.Value);
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, ex.Message, 4);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSucRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlSucRec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlSucRec.SelectedValue.Equals("0"))
            {
                var id = ddlSucRec.SelectedValue;
                _db.Conectar();
                _db.CrearComando(@"SELECT * FROM Cat_Sucursales WHERE idSucursal = @ID");
                _db.AsignarParametroCadena("@ID", id);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbCalleRec.Text = dr["calle"].ToString();
                    tbNoExtRec.Text = dr["noExterior"].ToString();
                    tbNoIntRec.Text = dr["noInterior"].ToString();
                    tbColoniaRec.Text = dr["colonia"].ToString();
                    //tbLocRec.Text = dr["localidad"].ToString();
                    //tbRefRec.Text = dr["referencia"].ToString();
                    tbMunicipioRec.Text = dr["municipio"].ToString();
                    tbEstadoRec.Text = dr["estado"].ToString();
                    tbPaisRec.Text = dr["pais"].ToString();
                    tbCpRec.Text = dr["codigoPostal"].ToString();
                }
                _db.Desconectar();
            }
            else
            {
                Llenarlistadom(tbRfcRec.Text);
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvConceptos.PageIndex = e.NewPageIndex;
            LlenarGridView();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRec.PageIndex = e.NewPageIndex;
            gvRec.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the bLimpiarBusquedaCliente control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bLimpiarBusquedaCliente_Click(object sender, EventArgs e)
        {
            LimpiarBusquedaCliente();
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            var query = "SELECT IDEREC ,RFCREC ,NOMREC ,(domicilio + ' Ext.' + noExterior + ' Int.' + noInterior + '. Col. ' + colonia + '. ' + localidad + ' Mun. ' + municipio + ', ' + estado + ', ' + pais + '. C.P.: ' + codigoPostal) AS domicilio FROM Cat_Receptor";
            var whereBody = "";
            var notEmptytbRfc = !string.IsNullOrEmpty(tbRFCClienteBusqueda.Text);
            var notEmptyRazon = !string.IsNullOrEmpty(tbRazonClienteBusqueda.Text);
            if (notEmptytbRfc)
            {
                whereBody += "RFCREC LIKE '" + tbRFCClienteBusqueda.Text + "%'";
            }
            if (notEmptyRazon)
            {
                whereBody += (notEmptytbRfc ? " OR " : "") + "NOMREC LIKE '" + tbRazonClienteBusqueda.Text + "%'";
            }
            if (!string.IsNullOrEmpty(whereBody))
            {
                query += " WHERE (" + whereBody + ")";
            }
            query += " ORDER BY NOMREC";
            SqlDataReceptores.SelectCommand = query;
            gvRec.DataBind();
        }

        /// <summary>
        /// Limpiars the busqueda cliente.
        /// </summary>
        private void LimpiarBusquedaCliente()
        {
            tbRFCClienteBusqueda.Text = "";
            tbRazonClienteBusqueda.Text = "";
            BindData();
        }

        /// <summary>
        /// Handles the Click event of the bBuscarCliente control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscarCliente_Click(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// Handles the Click event of the bUsarRecep control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bUsarRecep_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            Llenarlista(id);
            LimpiarBusquedaCliente();
        }
    }
}