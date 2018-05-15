// ***********************************************************************
// Assembly         : Control
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="Spool.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 414

namespace Control
{
    /// <summary>
    /// Class SpoolMx.
    /// </summary>
    public class SpoolMx
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpoolMx" /> class.
        /// </summary>
        public SpoolMx()
        {
            #region CFDI

            _cfdiArrayComprobante = null;
            _cfdiArrayEmisor = null;
            _cfdiArrayEmisorDom = null;
            _cfdiArrayEmisorExp = null;
            _cfdiarrayEmisorRegimen = null;
            _cfdiArrayReceptor = null;
            _cfdiArrayReceptorDom = null;
            _cfdiArrayReceptorDomRecepcion = null;
            _cfdiArrayReceptorCont = new List<string[]>();
            _cfdiArrayConceptos = new List<object[]>();
            _cfdiArrayCantidadImpuestos = null;
            _cfdiArrayImpuestos = new List<string[]>();
            _cfdiComplementoNomina = null;
            CfdiComplementoIne = null;
            _cfdiComplementoComercioExterior = null;
            _cfdiArrayImpuestosLocales = null;
            _cfdiArrayTrasladosLocales = new List<string[]>();
            _cfdiArrayRetencionesLocales = new List<string[]>();
            _cfdiArrayIa = new List<string[]>();
            _cfdiArrayLeyendasFiscales = new List<string[]>();
            _cfdiArrayAddendas = new List<string[]>();

            #endregion

            #region Retencion

            _retArrayComprobante = null;
            _retArrayPeriodo = null;
            _retArrayEmisor = null;
            _retArrayReceptor = null;
            _retArrayReceptorNac = null;
            _retArrayReceptorExt = null;
            _retArrayImpuestos = new List<string[]>();
            _retArrayTotales = null;
            _retComplementoPagosExt = null;
            _retComplementoPagosExtB = null;
            _retComplementoPagosExtNb = null;
            _retComplementoDividendos = null;
            _retComplementoDividendosRemanente = null;

            #endregion
        }

        /// <summary>
        /// Construye el texto plano de un CFDI.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ConstruyeTxtCfdi()
        {
            var txt = "";
            txt += ConcatArray(_cfdiArrayComprobante);
            txt += ConcatArray(_cfdiArrayEmisor);
            if (_cfdiArrayEmisorDom != null)
            {
                txt += ConcatArray(_cfdiArrayEmisorDom);
            }

            if (_cfdiArrayEmisorExp != null)
            {
                txt += ConcatArray(_cfdiArrayEmisorExp);
            }

            txt += ConcatArray(_cfdiarrayEmisorRegimen);
            txt += ConcatArray(_cfdiArrayReceptor);
            if (_cfdiArrayReceptorDom != null)
            {
                txt += ConcatArray(_cfdiArrayReceptorDom);
            }

            if (_cfdiArrayReceptorDomRecepcion != null)
            {
                txt += ConcatArray(_cfdiArrayReceptorDomRecepcion);
            }

            foreach (var contacto in _cfdiArrayReceptorCont)
            {
                txt += ConcatArray(contacto);
            }

            txt += ConcatArray(_cfdiArrayCantidadImpuestos);
            foreach (var impuesto in _cfdiArrayImpuestos)
            {
                txt += ConcatArray(impuesto);
            }

            foreach (var concepto in _cfdiArrayConceptos)
            {
                var conc = (string[])concepto[0];
                var terceros = (List<object>)concepto[1];
                var aduaneras = (List<string[]>)concepto[2];
                var predial = (string[])concepto[3];
                var partes = (List<object[]>)concepto[4];
                var impuestos = (List<string[]>)concepto[5];
                txt += ConcatArray(conc);
                if (terceros != null && terceros.Count > 0)
                {
                    var infoTercero = (string[])terceros[0];
                    var fiscales = (List<string[]>)terceros[1];
                    var retenciones = (List<string[]>)terceros[2];
                    var traslados = (List<string[]>)terceros[3];
                    txt += ConcatArray(infoTercero);
                    if (fiscales != null && fiscales.Count > 0)
                    {
                        foreach (var fiscal in fiscales)
                        {
                            txt += ConcatArray(fiscal);
                        }
                    }

                    if (retenciones != null && retenciones.Count > 0)
                    {
                        foreach (var retencion in retenciones)
                        {
                            txt += ConcatArray(retencion);
                        }
                    }

                    if (traslados != null && traslados.Count > 0)
                    {
                        foreach (var traslado in traslados)
                        {
                            txt += ConcatArray(traslado);
                        }
                    }
                }
                foreach (var aduanera in aduaneras)
                {
                    txt += ConcatArray(aduanera);
                }

                if (predial != null)
                {
                    txt += ConcatArray(predial);
                }

                foreach (var parte in partes)
                {
                    var part = (string[])parte[0];
                    var aduanerasParte = (List<string[]>)parte[1];
                    txt += ConcatArray(part);
                    foreach (var aduanera in aduanerasParte)
                    {
                        txt += ConcatArray(aduanera);
                    }
                }
                foreach (var impuesto in impuestos)
                {
                    txt += ConcatArray(impuesto);
                }
            }
            if (_cfdiComplementoNomina != null)
            {
                var arrayNomina = (string[])_cfdiComplementoNomina[0];
                var arrayPercepciones = (object[])_cfdiComplementoNomina[1];
                var arrayDeducciones = (object[])_cfdiComplementoNomina[2];
                var arrayIncapacidades = (List<string[]>)_cfdiComplementoNomina[3];
                var arrayHorasExtras = (List<string[]>)_cfdiComplementoNomina[4];
                if (arrayNomina != null)
                {
                    txt += ConcatArray(arrayNomina);
                    if (arrayPercepciones != null)
                    {
                        var nodo = (string[])arrayPercepciones[0];
                        var lista = (List<string[]>)arrayPercepciones[1];
                        txt += ConcatArray(nodo);
                        foreach (var percepcion in lista)
                        {
                            txt += ConcatArray(percepcion);
                        }
                    }
                    if (arrayDeducciones != null)
                    {
                        var nodo = (string[])arrayDeducciones[0];
                        var lista = (List<string[]>)arrayDeducciones[1];
                        txt += ConcatArray(nodo);
                        foreach (var deduccion in lista)
                        {
                            txt += ConcatArray(deduccion);
                        }
                    }
                    if (arrayIncapacidades != null)
                    {
                        foreach (var incap in arrayIncapacidades)
                        {
                            txt += ConcatArray(incap);
                        }
                    }

                    if (arrayHorasExtras != null)
                    {
                        foreach (var horas in arrayHorasExtras)
                        {
                            txt += ConcatArray(horas);
                        }
                    }
                }
            }
            if (CfdiComplementoIne != null)
            {
                var arrayIne = (string[])CfdiComplementoIne[0];
                var arrayEntidades = (List<object[]>)CfdiComplementoIne[1];
                if (arrayIne != null)
                {
                    txt += ConcatArray(arrayIne);
                    if (arrayEntidades != null)
                    {
                        foreach (var entidad in arrayEntidades)
                        {
                            var datosEntidad = (string[])entidad[0];
                            var arrayContabilidades = (List<string[]>)entidad[1];
                            if (datosEntidad != null)
                            {
                                txt += ConcatArray(datosEntidad);
                                if (arrayContabilidades != null)
                                {
                                    foreach (var contabilidad in arrayContabilidades)
                                    {
                                        txt += ConcatArray(contabilidad);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (_cfdiArrayLeyendasFiscales != null)
            {
                foreach (var leyenda in _cfdiArrayLeyendasFiscales)
                {
                    txt += ConcatArray(leyenda);
                }
            }

            if (_cfdiComplementoComercioExterior != null)
            {
                string[] comercio = null;
                string[] emisor = null;
                List<string[]> propietarios = null;
                string[] receptor = null;
                List<string[]> destinatarios = null;
                List<object[]> mercancias = null;
                try
                {
                    comercio = (string[])_cfdiComplementoComercioExterior[0];
                }
                catch
                {
                }
                try
                {
                    emisor = (string[])_cfdiComplementoComercioExterior[1];
                }
                catch
                {
                }
                try
                {
                    propietarios = (List<string[]>)_cfdiComplementoComercioExterior[2];
                }
                catch
                {
                }
                try
                {
                    receptor = (string[])_cfdiComplementoComercioExterior[3];
                }
                catch
                {
                }
                try
                {
                    destinatarios = (List<string[]>)_cfdiComplementoComercioExterior[4];
                }
                catch
                {
                }
                try
                {
                    mercancias = (List<object[]>)_cfdiComplementoComercioExterior[5];
                }
                catch
                {
                }
                if (comercio != null)
                {
                    txt += ConcatArray(comercio);
                    if (emisor != null)
                    {
                        txt += ConcatArray(emisor);
                    }

                    if (propietarios != null)
                    {
                        foreach (var propietario in propietarios)
                        {
                            txt += ConcatArray(propietario);
                        }
                    }

                    if (receptor != null)
                    {
                        txt += ConcatArray(receptor);
                    }

                    if (destinatarios != null)
                    {
                        foreach (var propietario in propietarios)
                        {
                            txt += ConcatArray(propietario);
                        }
                    }

                    if (mercancias != null)
                    {
                        foreach (var mercancia in mercancias)
                        {
                            string[] infoMercancia = null;
                            List<string[]> especificaciones = null;
                            try
                            {
                                infoMercancia = (string[])mercancia[0];
                            }
                            catch
                            {
                            }
                            try
                            {
                                especificaciones = (List<string[]>)mercancia[1];
                            }
                            catch
                            {
                            }
                            if (infoMercancia != null)
                            {
                                txt += ConcatArray(infoMercancia);
                                if (especificaciones != null)
                                {
                                    foreach (var especificacion in especificaciones)
                                    {
                                        txt += ConcatArray(especificacion);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (_cfdiArrayImpuestosLocales != null)
            {
                txt += ConcatArray(_cfdiArrayImpuestosLocales);
                foreach (var retLocal in _cfdiArrayRetencionesLocales)
                {
                    txt += ConcatArray(retLocal);
                }

                foreach (var traLocal in _cfdiArrayTrasladosLocales)
                {
                    txt += ConcatArray(traLocal);
                }
            }
            foreach (var ia in _cfdiArrayIa)
            {
                txt += ConcatArray(ia);
            }

            foreach (var addenda in _cfdiArrayAddendas)
            {
                txt += ConcatArray(addenda);
            }

            return txt;
        }

        /// <summary>
        /// Construye el texto plano de un comprobante de Retenciones.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ConstruyeTxtRetenciones()
        {
            var txt = "";
            txt += ConcatArray(_retArrayComprobante);
            txt += ConcatArray(_retArrayEmisor);
            txt += ConcatArray(_retArrayReceptor);
            if (_retArrayReceptorNac != null)
            {
                txt += ConcatArray(_retArrayReceptorNac);
            }
            else if (_retArrayReceptorExt != null)
            {
                txt += ConcatArray(_retArrayReceptorExt);
            }

            txt += ConcatArray(_retArrayPeriodo);
            txt += ConcatArray(_retArrayTotales);
            foreach (var imp in _retArrayImpuestos)
            {
                txt += ConcatArray(imp);
            }

            if (_retComplementoPagosExt != null)
            {
                txt += ConcatArray(_retComplementoPagosExt);
                if (_retComplementoPagosExtB != null)
                {
                    txt += ConcatArray(_retComplementoPagosExtB);
                }

                if (_retComplementoPagosExtNb != null)
                {
                    txt += ConcatArray(_retComplementoPagosExtNb);
                }
            }
            if (_retComplementoDividendos != null)
            {
                txt += ConcatArray(_retComplementoDividendos);
                if (_retComplementoDividendosRemanente != null)
                {
                    txt += ConcatArray(_retComplementoDividendosRemanente);
                }
            }
            return txt;
        }

        /// <summary>
        /// Concatena los elementos de un array en una cadena separada por |.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>System.String.</returns>
        private string ConcatArray(string[] array)
        {
            var txt = "";
            var tmp = array.ToList();
            tmp.RemoveAll(str => string.IsNullOrEmpty(str));
            // >1 por el nodo de cada linea
            if (array != null && tmp != null && tmp.Count > 1)
            {
                txt = string.Join("|", array) + "|" + Environment.NewLine;
            }

            return txt;
        }

        #region Retencion

        /// <summary>
        /// Asigna los valores requeridos para el nodo "retenciones:Retenciones" de un comprobante de retenciones.
        /// </summary>
        /// <param name="folioInt">The folio int.</param>
        /// <param name="fechaExp">The fecha exp.</param>
        /// <param name="cveRetenc">The cve retenc.</param>
        /// <param name="descRetenc">The desc retenc.</param>
        /// <param name="serie">The serie.</param>
        /// <param name="codDocModificado">The cod document modificado.</param>
        /// <param name="numDocModificado">The number document modificado.</param>
        /// <param name="fechaDocModificado">The fecha document modificado.</param>
        public void SetComprobanteRet(string folioInt = "", string fechaExp = "", string cveRetenc = "", string descRetenc = "", string serie = "", string codDocModificado = "", string numDocModificado = "", string fechaDocModificado = "")
        {
            _retArrayComprobante = new[]
            {
                "Retenciones|",
                folioInt,
                fechaExp,
                cveRetenc,
                descRetenc,
                serie,
                codDocModificado,
                numDocModificado,
                fechaDocModificado
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "retenciones:Emisor" de un comprobante de retenciones.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <param name="nombre">The nombre.</param>
        /// <param name="curp">The curp.</param>
        public void SetEmisorRet(string rfc = "", string nombre = "", string curp = "")
        {
            _retArrayEmisor = new[]
            {
                "Emisor|",
                rfc,
                nombre,
                curp
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "retenciones:Nacional" de un comprobante de retenciones.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <param name="nombre">The nombre.</param>
        /// <param name="curp">The curp.</param>
        public void SetReceptorNacRet(string rfc = "", string nombre = "", string curp = "")
        {
            _retArrayReceptor = new[]
            {
                "Receptor|",
                "Nacional"
            };
            _retArrayReceptorNac = new[]
            {
                "Receptor|RetencionesReceptorNacional|",
                rfc,
                nombre,
                curp
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "retenciones:Extranjero" de un comprobante de retenciones.
        /// </summary>
        /// <param name="numeroRegistroFiscal">The numero registro fiscal.</param>
        /// <param name="nombre">The nombre.</param>
        public void SetReceptorExtRet(string numeroRegistroFiscal = "", string nombre = "")
        {
            _retArrayReceptor = new[]
            {
                "Receptor|",
                "Extranjero"
            };
            _retArrayReceptorExt = new[]
            {
                "Receptor|RetencionesReceptorExtranjero|",
                numeroRegistroFiscal,
                nombre
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "retenciones:Periodo" de un comprobante de retenciones.
        /// </summary>
        /// <param name="mesIni">The mes ini.</param>
        /// <param name="mesFin">The mes fin.</param>
        /// <param name="ejerc">The ejerc.</param>
        public void SetPeriodoRet(string mesIni = "", string mesFin = "", string ejerc = "")
        {
            _retArrayPeriodo = new[]
            {
                "Periodo|",
                mesIni,
                mesFin,
                ejerc
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "retenciones:Totales" de un comprobante de retenciones.
        /// </summary>
        /// <param name="montoTotOperacion">The monto tot operacion.</param>
        /// <param name="montoTotGrav">The monto tot grav.</param>
        /// <param name="montoTotExent">The monto tot exent.</param>
        /// <param name="montoTotRet">The monto tot ret.</param>
        public void SetTotalesRet(string montoTotOperacion = "", string montoTotGrav = "", string montoTotExent = "", string montoTotRet = "")
        {
            _retArrayTotales = new[]
            {
                "Totales|",
                montoTotOperacion,
                montoTotGrav,
                montoTotExent,
                montoTotRet
            };
        }

        /// <summary>
        /// Agrega un registro del nodo "retenciones:ImpRetenidos" de un comprobante de retenciones.
        /// </summary>
        /// <param name="baseRet">The base ret.</param>
        /// <param name="impuesto">The impuesto.</param>
        /// <param name="montoRet">The monto ret.</param>
        /// <param name="tipoPagoRet">The tipo pago ret.</param>
        public void AgregaImpuestoRet(string baseRet = "", string impuesto = "", string montoRet = "", string tipoPagoRet = "")
        {
            if (_retArrayImpuestos == null)
            {
                _retArrayImpuestos = new List<string[]>();
            }

            var imp = new[]
            {
                "Totales|impRetenidos|",
                baseRet,
                impuesto,
                montoRet,
                tipoPagoRet
            };
            _retArrayImpuestos.Add(imp);
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "pagosaextranjeros:Pagosaextranjeros" de un comprobante de retenciones.
        /// </summary>
        /// <param name="esBenefEfectDelCobro">The es benef efect delete cobro.</param>
        public void SetPagosExtranjeros(string esBenefEfectDelCobro = "")
        {
            _retComplementoPagosExt = new[]
            {
                "Complemento|Pagosaextranjeros|",
                esBenefEfectDelCobro
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "pagosaextranjeros:Beneficiario" de un comprobante de retenciones.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <param name="curp">The curp.</param>
        /// <param name="nombre">The nombre.</param>
        /// <param name="conceptoPago">The concepto pago.</param>
        /// <param name="descripcionConcepto">The descripcion concepto.</param>
        public void SetPagosExtranjerosBeneficiario(string rfc = "", string curp = "", string nombre = "", string conceptoPago = "", string descripcionConcepto = "")
        {
            _retComplementoPagosExtB = new[]
            {
                "Complemento|Pagosaextranjeros|beneficiario|",
                rfc,
                curp,
                nombre,
                conceptoPago,
                descripcionConcepto
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "pagosaextranjeros:NoBeneficiario" de un comprobante de retenciones.
        /// </summary>
        /// <param name="paisDeResidParaEfecFisc">The pais de resid para efec fisc.</param>
        /// <param name="conceptoPago">The concepto pago.</param>
        /// <param name="descripcionConcepto">The descripcion concepto.</param>
        public void SetPagosExtranjerosNoBeneficiario(string paisDeResidParaEfecFisc = "", string conceptoPago = "", string descripcionConcepto = "")
        {
            _retComplementoPagosExtNb = new[]
            {
                "Complemento|Pagosaextranjeros|noBeneficiario|",
                paisDeResidParaEfecFisc,
                conceptoPago,
                descripcionConcepto
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "dividendos:Dividendos" de un comprobante de retenciones.
        /// </summary>
        /// <param name="cveTipDivOUtil">The cve tip div o utility.</param>
        /// <param name="montIsrAcredRetMexico">The mont isr acred ret mexico.</param>
        /// <param name="montIsrAcredRetExtranjero">The mont isr acred ret extranjero.</param>
        /// <param name="montRetExtDivExt">The mont ret ext div ext.</param>
        /// <param name="tipoSocDistrDiv">The tipo soc distr div.</param>
        /// <param name="montIsrAcredNal">The mont isr acred nal.</param>
        /// <param name="montDivAcumNal">The mont div acum nal.</param>
        /// <param name="montDivAcumExt">The mont div acum ext.</param>
        /// <param name="proporcionRem">The proporcion rem.</param>
        public void SetDividendosRetencion(string cveTipDivOUtil = "", string montIsrAcredRetMexico = "", string montIsrAcredRetExtranjero = "", string montRetExtDivExt = "", string tipoSocDistrDiv = "", string montIsrAcredNal = "", string montDivAcumNal = "", string montDivAcumExt = "", string proporcionRem = "")
        {
            _retComplementoDividendos = new[]
            {
                "Complemento|Dividendos|dividOUtil|",
                cveTipDivOUtil,
                montIsrAcredRetMexico,
                montIsrAcredRetExtranjero,
                montRetExtDivExt,
                tipoSocDistrDiv,
                montIsrAcredNal,
                montDivAcumNal,
                montDivAcumExt
            };
            if (!string.IsNullOrEmpty(proporcionRem))
            {
                _retComplementoDividendosRemanente = new[]
                {
                    "Complemento|Dividendos|remanente|",
                    proporcionRem
                };
            }
        }

        #endregion

        #region CFDI

        /// <summary>
        /// Asigna los valores requeridos para el nodo "cfdi:Comprobante" de un comprobante CFDI.
        /// </summary>
        /// <param name="serie">The serie.</param>
        /// <param name="folio">The folio.</param>
        /// <param name="fecha">The fecha.</param>
        /// <param name="formaDePago">The forma de pago.</param>
        /// <param name="condicionesDePago">The condiciones de pago.</param>
        /// <param name="subTotal">The sub total.</param>
        /// <param name="descuento">The descuento.</param>
        /// <param name="motivoDescuento">The motivo descuento.</param>
        /// <param name="tipoCambio">The tipo cambio.</param>
        /// <param name="moneda">The moneda.</param>
        /// <param name="totalFactura">The total factura.</param>
        /// <param name="tipoDeComprobante">The tipo de comprobante.</param>
        /// <param name="metodoDePago">The metodo de pago.</param>
        /// <param name="lugarExpedicion">The lugar expedicion.</param>
        /// <param name="numCtaPago">The number cta pago.</param>
        /// <param name="folioFiscalOrig">The folio fiscal original.</param>
        /// <param name="serieFolioFiscalOrig">The serie folio fiscal original.</param>
        /// <param name="fechaFolioFiscalOrig">The fecha folio fiscal original.</param>
        /// <param name="montoFolioFiscalOrig">The monto folio fiscal original.</param>
        /// <param name="codFolioFiscalOrig">The cod folio fiscal original.</param>
        /// <param name="motivoAnulacion">The motivo anulacion.</param>
        /// <param name="otrosCargos">The otros cargos.</param>
        /// <param name="totalPagar">The total pagar.</param>
        /// <param name="observaciones">The observaciones.</param>
        /// <param name="adendaHoteles">The adenda hoteles.</param>
        /// <param name="fechaCierre">The fecha cierre.</param>
        /// <param name="Cheque">The cheque.</param>
        /// <param name="confirmacionPAC">The confirmacion pac.</param>
        public void SetComprobanteCfdi(string serie = "", string folio = "", string fecha = "", string formaDePago = "", string condicionesDePago = "", string subTotal = "", string descuento = "", string motivoDescuento = "", string tipoCambio = "", string moneda = "", string totalFactura = "", string tipoDeComprobante = "", string metodoDePago = "", string lugarExpedicion = "", string numCtaPago = "", string folioFiscalOrig = "", string serieFolioFiscalOrig = "", string fechaFolioFiscalOrig = "", string montoFolioFiscalOrig = "", string codFolioFiscalOrig = "", string motivoAnulacion = "", string otrosCargos = "", string totalPagar = "", string observaciones = "", string adendaHoteles = "", string fechaCierre = "", string Cheque = "", string confirmacionPAC = "", bool isCredito = false)
        {
            _cfdiArrayComprobante = new[]
            {
                "Comprobante|",
                serie,
                folio,
                fecha,
                formaDePago,
                condicionesDePago,
                subTotal,
                descuento,
                motivoDescuento,
                tipoCambio,
                moneda,
                totalFactura,
                tipoDeComprobante,
                metodoDePago,
                lugarExpedicion,
                numCtaPago,
                folioFiscalOrig,
                serieFolioFiscalOrig,
                fechaFolioFiscalOrig,
                montoFolioFiscalOrig,
                codFolioFiscalOrig,
                motivoAnulacion,
                otrosCargos,
                totalPagar,
                observaciones,
                "",
                "",
                adendaHoteles,
                fechaCierre,
                Cheque,
                confirmacionPAC,
                isCredito ? "1" : "0"
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "cfdi:Emisor" de un comprobante CFDI.
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <param name="rfc">The RFC.</param>
        /// <param name="curp">The curp.</param>
        /// <param name="telefono">The telefono.</param>
        /// <param name="email">The email.</param>
        /// <param name="empresaTipo">The empresa tipo.</param>
        /// <param name="regimen">The regimen.</param>
        /// <param name="obligadoContabilidad">The obligado contabilidad.</param>
        public void SetEmisorCfdi(string nombre = "", string rfc = "", string curp = "", string telefono = "", string email = "", string empresaTipo = "", string regimen = "", string obligadoContabilidad = "")
        {
            _cfdiArrayEmisor = new[]
            {
                "Emisor|",
                nombre,
                rfc,
                curp,
                telefono,
                email,
                empresaTipo,
                obligadoContabilidad
            };
            _cfdiarrayEmisorRegimen = new[]
            {
                "Emisor|regimenFiscal|",
                regimen
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "cfdi:DomicilioFiscal" de un comprobante CFDI.
        /// </summary>
        /// <param name="calle">The calle.</param>
        /// <param name="noExterior">The no exterior.</param>
        /// <param name="noInterior">The no interior.</param>
        /// <param name="colonia">The colonia.</param>
        /// <param name="localidad">The localidad.</param>
        /// <param name="referencia">The referencia.</param>
        /// <param name="municipio">The municipio.</param>
        /// <param name="estado">The estado.</param>
        /// <param name="pais">The pais.</param>
        /// <param name="codigoPostal">The codigo postal.</param>
        public void SetEmisorDomCfdi(string calle = "", string noExterior = "", string noInterior = "", string colonia = "", string localidad = "", string referencia = "", string municipio = "", string estado = "", string pais = "", string codigoPostal = "")
        {
            _cfdiArrayEmisorDom = new[]
            {
                "Emisor|domicilioFiscal|",
                calle,
                noExterior,
                noInterior,
                colonia,
                localidad,
                referencia,
                municipio,
                estado,
                pais,
                codigoPostal
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "cfdi:DomicilioExpedicion" de un comprobante CFDI.
        /// </summary>
        /// <param name="calleExp">The calle exp.</param>
        /// <param name="noExteriorExp">The no exterior exp.</param>
        /// <param name="noInteriorExp">The no interior exp.</param>
        /// <param name="coloniaExp">The colonia exp.</param>
        /// <param name="localidadExp">The localidad exp.</param>
        /// <param name="referenciaExp">The referencia exp.</param>
        /// <param name="municipioExp">The municipio exp.</param>
        /// <param name="estadoExp">The estado exp.</param>
        /// <param name="paisExp">The pais exp.</param>
        /// <param name="codigoPostalExp">The codigo postal exp.</param>
        /// <param name="claveSucursal">The clave sucursal.</param>
        public void SetEmisorExpCfdi(string calleExp = "", string noExteriorExp = "", string noInteriorExp = "", string coloniaExp = "", string localidadExp = "", string referenciaExp = "", string municipioExp = "", string estadoExp = "", string paisExp = "", string codigoPostalExp = "", string claveSucursal = "")
        {
            _cfdiArrayEmisorExp = new[]
            {
                "Emisor|expedidoEn|",
                calleExp,
                noExteriorExp,
                noInteriorExp,
                coloniaExp,
                localidadExp,
                referenciaExp,
                municipioExp,
                estadoExp,
                paisExp,
                codigoPostalExp,
                claveSucursal
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "cfdi:Receptor" de un comprobante CFDI.
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <param name="rfc">The RFC.</param>
        /// <param name="curp">The curp.</param>
        /// <param name="telefono">The telefono.</param>
        /// <param name="email">The email.</param>
        /// <param name="telefono2">The telefono2.</param>
        /// <param name="denominacionSocial">The denominacion social.</param>
        /// <param name="obligadoContabilidad">The obligado contabilidad.</param>
        /// <param name="numRegIdTrib">The number reg identifier trib.</param>
        /// <param name="usoCfdi">The uso cfdi.</param>
        public void SetReceptorCfdi(string nombre = "", string rfc = "", string curp = "", string telefono = "", string email = "", string telefono2 = "", string denominacionSocial = "", string obligadoContabilidad = "", string numRegIdTrib = "", string usoCfdi = "", string numCliente = "")
        {
            _cfdiArrayReceptor = new[]
            {
                "Receptor|",
                nombre,
                rfc,
                curp,
                telefono,
                email,
                telefono2,
                denominacionSocial,
                obligadoContabilidad,
                numRegIdTrib,
                usoCfdi,
                numCliente
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "cfdi:Domicilio" de un comprobante CFDI.
        /// </summary>
        /// <param name="calle">The calle.</param>
        /// <param name="noExterior">The no exterior.</param>
        /// <param name="noInterior">The no interior.</param>
        /// <param name="colonia">The colonia.</param>
        /// <param name="localidad">The localidad.</param>
        /// <param name="referencia">The referencia.</param>
        /// <param name="municipio">The municipio.</param>
        /// <param name="estado">The estado.</param>
        /// <param name="pais">The pais.</param>
        /// <param name="codigoPostal">The codigo postal.</param>
        /// <param name="sucursal">if set to <c>true</c> [sucursal].</param>
        public void SetReceptorDomCfdi(string calle = "", string noExterior = "", string noInterior = "", string colonia = "", string localidad = "", string referencia = "", string municipio = "", string estado = "", string pais = "", string codigoPostal = "", bool sucursal = false)
        {
            _cfdiArrayReceptorDom = new[]
            {
                "Receptor|domicilio|",
                calle,
                noExterior,
                noInterior,
                colonia,
                localidad,
                referencia,
                municipio,
                estado,
                pais,
                codigoPostal,
                sucursal.ToString()
            };
        }

        /// <summary>
        /// Sets the receptor DOM recepcion cfdi.
        /// </summary>
        /// <param name="calle">The calle.</param>
        /// <param name="noExterior">The no exterior.</param>
        /// <param name="noInterior">The no interior.</param>
        /// <param name="colonia">The colonia.</param>
        /// <param name="localidad">The localidad.</param>
        /// <param name="referencia">The referencia.</param>
        /// <param name="municipio">The municipio.</param>
        /// <param name="estado">The estado.</param>
        /// <param name="pais">The pais.</param>
        /// <param name="codigoPostal">The codigo postal.</param>
        public void SetReceptorDomRecepcionCfdi(string calle = "", string noExterior = "", string noInterior = "", string colonia = "", string localidad = "", string referencia = "", string municipio = "", string estado = "", string pais = "", string codigoPostal = "")
        {
            _cfdiArrayReceptorDomRecepcion = new[]
            {
                "Receptor|domicilioRecepcion|",
                calle,
                noExterior,
                noInterior,
                colonia,
                localidad,
                referencia,
                municipio,
                estado,
                pais,
                codigoPostal
            };
        }

        /// <summary>
        /// Asigna los valores opcionales para los contactos de un Receptor (no se reflejan en el CFDI).
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <param name="puesto">The puesto.</param>
        /// <param name="telefono1">The telefono1.</param>
        /// <param name="telefono2">The telefono2.</param>
        /// <param name="correo">The correo.</param>
        public void AgregaContatoReceptorCfdi(string nombre = "", string puesto = "", string telefono1 = "", string telefono2 = "", string correo = "")
        {
            if (_cfdiArrayReceptorCont == null)
            {
                _cfdiArrayReceptorCont = new List<string[]>();
            }

            var contacto = new[]
            {
                "Receptor|contacto|",
                nombre,
                puesto,
                telefono1,
                telefono2,
                correo
            };
            _cfdiArrayReceptorCont.Add(contacto);
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "cfdi:Impuestos" de un comprobante CFDI.
        /// </summary>
        /// <param name="totalImpuestosRetenidos">The total impuestos retenidos.</param>
        /// <param name="totalImpuestosTrasladados">The total impuestos trasladados.</param>
        /// <param name="iva16">The iva16.</param>
        public void SetCantidadImpuestosCfdi(string totalImpuestosRetenidos = "", string totalImpuestosTrasladados = "", string iva16 = "")
        {
            _cfdiArrayCantidadImpuestos = new[]
            {
                "Impuestos|",
                totalImpuestosRetenidos,
                totalImpuestosTrasladados,
                iva16
            };
        }

        /// <summary>
        /// Agrega un registro del nodo "cfdi:Retencion" de un comprobante CFDI.
        /// </summary>
        /// <param name="impuesto">The impuesto.</param>
        /// <param name="importe">The importe.</param>
        /// <param name="tipoFactor">The tipo factor.</param>
        public void AgregaImpuestoRetencionCfdi(string impuesto = "", string importe = "", string tipoFactor = "")
        {
            if (_cfdiArrayImpuestos == null)
            {
                _cfdiArrayImpuestos = new List<string[]>();
            }

            var imp = new[]
            {
                "Impuestos|retencion|",
                impuesto,
                importe,
                tipoFactor
            };
            _cfdiArrayImpuestos.Add(imp);
        }

        /// <summary>
        /// Agrega un registro del nodo "cfdi:Traslado" de un comprobante CFDI.
        /// </summary>
        /// <param name="impuesto">The impuesto.</param>
        /// <param name="tasa">The tasa.</param>
        /// <param name="importe">The importe.</param>
        /// <param name="tipoFactor">The tipo factor.</param>
        public void AgregaImpuestoTrasladoCfdi(string impuesto = "", string tasa = "", string importe = "", string tipoFactor = "")
        {
            if (_cfdiArrayImpuestos == null)
            {
                _cfdiArrayImpuestos = new List<string[]>();
            }

            var imp = new[]
            {
                "Impuestos|traslado|",
                impuesto,
                tasa,
                importe,
                tipoFactor
            };
            _cfdiArrayImpuestos.Add(imp);
        }

        /// <summary>
        /// Agregas the concepto cfdi.
        /// </summary>
        /// <param name="cantidad">The cantidad.</param>
        /// <param name="unidad">The unidad.</param>
        /// <param name="noIdentificacion">The no identificacion.</param>
        /// <param name="descripcion">The descripcion.</param>
        /// <param name="valorUnitario">The valor unitario.</param>
        /// <param name="importe">The importe.</param>
        /// <param name="descuento">The descuento.</param>
        /// <param name="claveProdServ">The clave product serv.</param>
        /// <param name="claveUnidad">The clave unidad.</param>
        /// <param name="idInterno">The identifier interno.</param>
        public void AgregaConceptoCfdi(string cantidad = "", string unidad = "", string noIdentificacion = "", string descripcion = "", string valorUnitario = "", string importe = "", string descuento = "", string claveProdServ = "", string claveUnidad = "", string idInterno = "")
        {
            if (_cfdiArrayConceptos == null)
            {
                _cfdiArrayConceptos = new List<object[]>();
            }

            var concepto = new[]
            {
                "Concepto|",
                cantidad,
                unidad,
                noIdentificacion,
                descripcion,
                valorUnitario,
                importe,
                descuento,
                claveProdServ,
                claveUnidad,
                idInterno
            };
            var aduaneras = new List<string[]>();
            var partes = new List<object[]>();
            var terceros = new List<object>();
            var impuestos = new List<string[]>();
            string[] predial = null;
            var regConcepto = new object[]
            {
                concepto,
                terceros,
                aduaneras,
                predial,
                partes,
                impuestos
            };
            _cfdiArrayConceptos.Add(regConcepto);
        }

        /// <summary>
        /// Agregas the concepto impuesto cfdi.
        /// </summary>
        /// <param name="isRetencion">if set to <c>true</c> [is retencion].</param>
        /// <param name="Base">The base.</param>
        /// <param name="Impuesto">The impuesto.</param>
        /// <param name="TipoFactor">The tipo factor.</param>
        /// <param name="TasaOCuota">The tasa o cuota.</param>
        /// <param name="Importe">The importe.</param>
        /// <param name="idInterno">The identifier interno.</param>
        public void AgregaConceptoImpuestoCfdi(bool isRetencion, string Base = "", string Impuesto = "", string TipoFactor = "", string TasaOCuota = "", string Importe = "", string idInterno = "")
        {
            var ultimoConcepto = _cfdiArrayConceptos.Last();
            var impuestos = (List<string[]>)ultimoConcepto[5];
            var imp = new[]
            {
                "Concepto|" + (!isRetencion ? "traslado" : "retencion") + "|",
                Base,
                Impuesto,
                TipoFactor,
                TasaOCuota,
                Importe,
                idInterno
            };
            impuestos.Add(imp);
            ultimoConcepto[5] = impuestos;
        }

        /// <summary>
        /// Sets the terceros concepto cfdi.
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <param name="rfc">The RFC.</param>
        public void SetTercerosConceptoCfdi(string nombre = "", string rfc = "")
        {
            var ultimoConcepto = _cfdiArrayConceptos.Last();
            var terceros = new[]
            {
                "Concepto|complemento|terceros|",
                nombre,
                rfc
            };
            var fiscales = new List<string[]>();
            var retenciones = new List<string[]>();
            var traslados = new List<string[]>();
            ultimoConcepto[1] = new List<object>
            {
                terceros,
                fiscales,
                retenciones,
                traslados
            };
        }

        /// <summary>
        /// Agregas the fiscal terceros concepto cfdi.
        /// </summary>
        /// <param name="calle">The calle.</param>
        /// <param name="noExterior">The no exterior.</param>
        /// <param name="noInterior">The no interior.</param>
        /// <param name="colonia">The colonia.</param>
        /// <param name="localidad">The localidad.</param>
        /// <param name="referencia">The referencia.</param>
        /// <param name="municipio">The municipio.</param>
        /// <param name="estado">The estado.</param>
        /// <param name="pais">The pais.</param>
        /// <param name="codigoPostal">The codigo postal.</param>
        public void AgregaFiscalTercerosConceptoCfdi(string calle = "", string noExterior = "", string noInterior = "", string colonia = "", string localidad = "", string referencia = "", string municipio = "", string estado = "", string pais = "", string codigoPostal = "")
        {
            var ultimoConcepto = _cfdiArrayConceptos.Last();
            var info = (List<object>)ultimoConcepto[1];
            var terceros = (string[])info[0];
            var fiscales = (List<string[]>)info[1];
            var retenciones = (List<string[]>)info[2];
            var traslados = (List<string[]>)info[3];
            var fiscal = new[]
            {
                "Concepto|complemento|terceros|fiscal|",
                calle,
                noExterior,
                noInterior,
                colonia,
                localidad,
                referencia,
                municipio,
                estado,
                pais,
                codigoPostal
            };
            fiscales.Add(fiscal);
            ultimoConcepto[1] = new List<object>
            {
                terceros,
                fiscales,
                retenciones,
                traslados
            };
        }

        /// <summary>
        /// Agregas the impuesto terceros cfdi.
        /// </summary>
        /// <param name="isRetencion">if set to <c>true</c> [is retencion].</param>
        /// <param name="impuesto">The impuesto.</param>
        /// <param name="importe">The importe.</param>
        /// <param name="tasa">The tasa.</param>
        public void AgregaImpuestoTercerosCfdi(bool isRetencion, string impuesto = "", string importe = "", string tasa = "")
        {
            var ultimoConcepto = _cfdiArrayConceptos.Last();
            var info = (List<object>)ultimoConcepto[1];
            var terceros = (string[])info[0];
            var fiscales = (List<string[]>)info[1];
            var retenciones = (List<string[]>)info[2];
            var traslados = (List<string[]>)info[3];
            string[] tax = null;
            if (isRetencion)
            {
                tax = new[]
                {
                    "Concepto|complemento|terceros|impuestos|retencion|",
                    impuesto,
                    importe
                };
                retenciones.Add(tax);
            }
            else
            {
                tax = new[]
                {
                    "Concepto|complemento|terceros|impuestos|traslado|",
                    impuesto,
                    tasa,
                    importe
                };
                traslados.Add(tax);
            }
            ultimoConcepto[1] = new List<object>
            {
                terceros,
                fiscales,
                retenciones,
                traslados
            };
        }

        /// <summary>
        /// Agregas the aduanera concepto cfdi.
        /// </summary>
        /// <param name="numero">The numero.</param>
        /// <param name="fecha">The fecha.</param>
        /// <param name="aduana">The aduana.</param>
        public void AgregaAduaneraConceptoCfdi(string numero = "", string fecha = "", string aduana = "")
        {
            var ultimoConcepto = _cfdiArrayConceptos.Last();
            var infoTerceros = (List<object>)ultimoConcepto[1];
            var aduanas = (List<string[]>)ultimoConcepto[2];
            var aduanera = new[]
            {
                infoTerceros == null || infoTerceros.Count <= 0 ? "Concepto|aduanera|" : "Concepto|complemento|terceros|aduanera|",
                numero,
                fecha,
                aduana
            };
            aduanas.Add(aduanera);
            ultimoConcepto[2] = aduanas;
        }

        /// <summary>
        /// Sets the predial concepto cfdi.
        /// </summary>
        /// <param name="numero">The numero.</param>
        public void SetPredialConceptoCfdi(string numero = "")
        {
            var ultimoConcepto = _cfdiArrayConceptos.Last();
            var infoTerceros = (List<object>)ultimoConcepto[1];
            ultimoConcepto[3] = new[]
            {
                infoTerceros == null || infoTerceros.Count <= 0 ? "Concepto|predial|" : "Concepto|complemento|terceros|predial|",
                numero
            };
        }

        /// <summary>
        /// Agregas the parte concepto cfdi.
        /// </summary>
        /// <param name="cantidad">The cantidad.</param>
        /// <param name="unidad">The unidad.</param>
        /// <param name="noIdentificacion">The no identificacion.</param>
        /// <param name="descripcion">The descripcion.</param>
        /// <param name="valorUnitario">The valor unitario.</param>
        /// <param name="importe">The importe.</param>
        public void AgregaParteConceptoCfdi(string cantidad = "", string unidad = "", string noIdentificacion = "", string descripcion = "", string valorUnitario = "", string importe = "")
        {
            var ultimoConcepto = _cfdiArrayConceptos.Last();
            var infoTerceros = (List<object>)ultimoConcepto[1];
            var partes = (List<object[]>)ultimoConcepto[4];
            var parte = new object[]
            {
                new[]
                {
                    infoTerceros == null || infoTerceros.Count <= 0 ? "Concepto|parte|" : "Concepto|complemento|terceros|parte|",
                    cantidad,
                    unidad,
                    noIdentificacion,
                    descripcion,
                    valorUnitario,
                    importe
                },
                new List<string[]>()
            };
            partes.Add(parte);
            ultimoConcepto[4] = partes;
        }

        /// <summary>
        /// Agregas the parte aduanera concepto cfdi.
        /// </summary>
        /// <param name="numero">The numero.</param>
        /// <param name="fecha">The fecha.</param>
        /// <param name="aduana">The aduana.</param>
        public void AgregaParteAduaneraConceptoCfdi(string numero = "", string fecha = "", string aduana = "")
        {
            var ultimoConcepto = _cfdiArrayConceptos.Last();
            var infoTerceros = (List<object>)ultimoConcepto[1];
            var partes = (List<object[]>)ultimoConcepto[4];
            var ultimaParte = partes.Last();
            var aduaneras = (List<string[]>)ultimaParte[1];
            var aduanera = new[]
            {
                infoTerceros == null || infoTerceros.Count <= 0 ? "Concepto|parte|aduanera|" : "Concepto|complemento|terceros|parte|aduanera|",
                numero,
                fecha,
                aduana
            };
            aduaneras.Add(aduanera);
            ultimaParte[1] = aduaneras;
            ultimoConcepto[4] = partes;
        }

        /// <summary>
        /// Asigna los valores opcionales para restaurantes (no se reflejan en el CFDI).
        /// </summary>
        /// <param name="propina">The propina.</param>
        /// <param name="ticket">The ticket.</param>
        public void SetInfoAdicionalRestauranteCfdi(string propina = "", string ticket = "")
        {
            _cfdiArrayIa = _cfdiArrayIa != null ? _cfdiArrayIa : new List<string[]>();
            var restaurante = new[]
            {
                "InformacionAdicional|restaurante|",
                propina,
                ticket
            };
            _cfdiArrayIa.Add(restaurante);
        }

        /// <summary>
        /// Asigna los valores opcionales para una habitación en un hotel (no se reflejan en el CFDI).
        /// </summary>
        /// <param name="huesped">The huesped.</param>
        /// <param name="reservacion">The reservacion.</param>
        /// <param name="habitacion">The habitacion.</param>
        /// <param name="llegada">The llegada.</param>
        /// <param name="salida">The salida.</param>
        /// <param name="cupon">The cupon.</param>
        /// <param name="tipoHabitacion">The tipo habitacion.</param>
        public void SetInfoAdicionalHabitacionCfdi(string huesped = "", string reservacion = "", string habitacion = "", string llegada = "", string salida = "", string cupon = "", string tipoHabitacion = "", string propina = "")
        {
            _cfdiArrayIa = _cfdiArrayIa != null ? _cfdiArrayIa : new List<string[]>();
            var hotel = new[]
            {
                "InformacionAdicional|hotel|habitacion|",
                huesped,
                reservacion,
                habitacion,
                llegada,
                salida,
                cupon,
                tipoHabitacion,
                propina
            };
            _cfdiArrayIa.Add(hotel);
        }

        /// <summary>
        /// Asigna los valores opcionales para un evento en un hotel (no se reflejan en el CFDI).
        /// </summary>
        /// <param name="huesped">The huesped.</param>
        /// <param name="reservacion">The reservacion.</param>
        /// <param name="llegada">The llegada.</param>
        public void SetInfoAdicionalEventoCfdi(string huesped = "", string reservacion = "", string llegada = "", string propina = "")
        {
            _cfdiArrayIa = _cfdiArrayIa != null ? _cfdiArrayIa : new List<string[]>();
            var hotel = new[]
            {
                "InformacionAdicional|hotel|evento|",
                huesped,
                reservacion,
                llegada,
                propina
            };
            _cfdiArrayIa.Add(hotel);
        }

        /// <summary>
        /// Asigna los valores opcionales para el traslado de mercancía para una Carta Porte (no se reflejan en el CFDI).
        /// </summary>
        /// <param name="origen">The origen.</param>
        /// <param name="destino">The destino.</param>
        /// <param name="conductor">The conductor.</param>
        /// <param name="vehiculo">The vehiculo.</param>
        /// <param name="placas">The placas.</param>
        /// <param name="kilometros">The kilometros.</param>
        public void SetInfoAdicionalMercanciaCfdi(string origen = "", string destino = "", string conductor = "", string vehiculo = "", string placas = "", string kilometros = "")
        {
            _cfdiArrayIa = _cfdiArrayIa != null ? _cfdiArrayIa : new List<string[]>();
            var mercancia = new[]
            {
                "InformacionAdicional|mercancia|",
                origen,
                destino,
                conductor,
                vehiculo,
                placas,
                kilometros
            };
            _cfdiArrayIa.Add(mercancia);
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "implocal:ImpuestosLocales" de un comprobante CFDI.
        /// </summary>
        /// <param name="totaldeRetenciones">The totalde retenciones.</param>
        /// <param name="totaldeTraslados">The totalde traslados.</param>
        public void SetImpuestosLocalesCfdi(string totaldeRetenciones = "", string totaldeTraslados = "")
        {
            _cfdiArrayImpuestosLocales = new[]
            {
                "Complemento|impuestosLocales|",
                totaldeRetenciones,
                totaldeTraslados
            };
        }

        /// <summary>
        /// Agrega un registro del nodo "implocal:TrasladosLocales" de un comprobante CFDI.
        /// </summary>
        /// <param name="impLocTrasladado">The imp loc trasladado.</param>
        /// <param name="tasadeTraslado">The tasade traslado.</param>
        /// <param name="importe">The importe.</param>
        public void AgregaTrasladoLocalCfdi(string impLocTrasladado = "", string tasadeTraslado = "", string importe = "")
        {
            if (_cfdiArrayTrasladosLocales == null)
            {
                _cfdiArrayTrasladosLocales = new List<string[]>();
            }

            var array = new[]
            {
                "Complemento|impuestosLocales|trasladoLocal|",
                impLocTrasladado,
                tasadeTraslado,
                importe
            };
            _cfdiArrayTrasladosLocales.Add(array);
        }

        /// <summary>
        /// Agrega un registro del nodo "implocal:RetencionesLocales" de un comprobante CFDI.
        /// </summary>
        /// <param name="impLocRetenido">The imp loc retenido.</param>
        /// <param name="tasadeRetencion">The tasade retencion.</param>
        /// <param name="importe">The importe.</param>
        public void AgregaRetencionLocalCfdi(string impLocRetenido = "", string tasadeRetencion = "", string importe = "")
        {
            if (_cfdiArrayRetencionesLocales == null)
            {
                _cfdiArrayRetencionesLocales = new List<string[]>();
            }

            var array = new[]
            {
                "Complemento|impuestosLocales|retencionLocal|",
                impLocRetenido,
                tasadeRetencion,
                importe
            };
            _cfdiArrayRetencionesLocales.Add(array);
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "ine:INE" de un comprobante CFDI.
        /// </summary>
        /// <param name="tipoProceso">The tipo proceso.</param>
        /// <param name="tipoComite">The tipo comite.</param>
        /// <param name="idContabilidad">The identifier contabilidad.</param>
        public void SetComplementoIneCfdi(string tipoProceso = "", string tipoComite = "", string idContabilidad = "")
        {
            var ine = new[]
            {
                "Complemento|INE|",
                tipoProceso,
                tipoComite,
                idContabilidad
            };
            var entidades = new List<object[]>();
            CfdiComplementoIne = new object[]
            {
                ine,
                entidades
            };
        }

        /// <summary>
        /// Agrega un registro del nodo "ine:Entidad" de un comprobante CFDI.
        /// </summary>
        /// <param name="claveEntidad">The clave entidad.</param>
        /// <param name="ambito">The ambito.</param>
        public void AgregarEntidadIne(string claveEntidad = "", string ambito = "")
        {
            if (CfdiComplementoIne != null)
            {
                var entidades = CfdiComplementoIne[1] == null ? new List<object[]>() : (List<object[]>)CfdiComplementoIne[1];
                var entidad = new[]
                {
                    "Complemento|INE|Entidad|",
                    claveEntidad,
                    ambito
                };
                var contabilidades = new List<string[]>();
                entidades.Add(new object[]
                {
                    entidad,
                    contabilidades
                });
                CfdiComplementoIne[1] = entidades;
            }
        }

        /// <summary>
        /// Agrega un registro del nodo "ine:Contabilidad" de un comprobante CFDI.
        /// </summary>
        /// <param name="idContabilidad">The identifier contabilidad.</param>
        public void AgregarContabilidadIne(string idContabilidad = "")
        {
            if (CfdiComplementoIne != null && CfdiComplementoIne[1] != null)
            {
                var entidades = (List<object[]>)CfdiComplementoIne[1];
                if (entidades.Count > 0)
                {
                    var ultimaEntidad = entidades.Last();
                    var contabilidades = (List<string[]>)ultimaEntidad[1];
                    contabilidades.Add(new[]
                    {
                        "Complemento|INE|Entidad|Contabilidad|",
                        idContabilidad
                    });
                    ultimaEntidad[1] = contabilidades;
                    CfdiComplementoIne[1] = entidades;
                }
            }
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "nomina:Nomina" de un comprobante CFDI.
        /// </summary>
        /// <param name="registroPatronal">The registro patronal.</param>
        /// <param name="numEmpleado">The number empleado.</param>
        /// <param name="curp">The curp.</param>
        /// <param name="tipoRegimen">The tipo regimen.</param>
        /// <param name="numSeguridadSocial">The number seguridad social.</param>
        /// <param name="fechaPago">The fecha pago.</param>
        /// <param name="fechaInicialPago">The fecha inicial pago.</param>
        /// <param name="fechaFinalPago">The fecha final pago.</param>
        /// <param name="numDiasPagados">The number dias pagados.</param>
        /// <param name="departamento">The departamento.</param>
        /// <param name="clabe">The clabe.</param>
        /// <param name="banco">The banco.</param>
        /// <param name="fechaInicioRelLaboral">The fecha inicio relative laboral.</param>
        /// <param name="antiguedad">The antiguedad.</param>
        /// <param name="puesto">The puesto.</param>
        /// <param name="tipoContrato">The tipo contrato.</param>
        /// <param name="tipoJornada">The tipo jornada.</param>
        /// <param name="periodicidadPago">The periodicidad pago.</param>
        /// <param name="salarioBaseCotApor">The salario base cot apor.</param>
        /// <param name="riesgoPuesto">The riesgo puesto.</param>
        /// <param name="salarioDiarioIntegrado">The salario diario integrado.</param>
        public void SetComplemntoNominaCfdi(string registroPatronal = "", string numEmpleado = "", string curp = "", string tipoRegimen = "", string numSeguridadSocial = "", string fechaPago = "", string fechaInicialPago = "", string fechaFinalPago = "", string numDiasPagados = "", string departamento = "", string clabe = "", string banco = "", string fechaInicioRelLaboral = "", string antiguedad = "", string puesto = "", string tipoContrato = "", string tipoJornada = "", string periodicidadPago = "", string salarioBaseCotApor = "", string riesgoPuesto = "", string salarioDiarioIntegrado = "")
        {
            var nomina = new[]
            {
                "Complemento|nomina|",
                registroPatronal,
                numEmpleado,
                curp,
                tipoRegimen,
                numSeguridadSocial,
                fechaPago,
                fechaInicialPago,
                fechaFinalPago,
                numDiasPagados,
                departamento,
                clabe,
                banco,
                fechaInicioRelLaboral,
                antiguedad,
                puesto,
                tipoContrato,
                tipoJornada,
                periodicidadPago,
                salarioBaseCotApor,
                riesgoPuesto,
                salarioDiarioIntegrado
            };
            var percepciones = new object[0];
            var deducciones = new object[0];
            var incapacidades = new List<string[]>();
            var horasExtras = new List<string[]>();
            _cfdiComplementoNomina = new object[]
            {
                nomina,
                percepciones,
                deducciones,
                incapacidades,
                horasExtras
            };
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "nomina:Percepciones" de un comprobante CFDI.
        /// </summary>
        /// <param name="totalGravado">The total gravado.</param>
        /// <param name="totalExento">The total exento.</param>
        public void SetNominaPercepciones(string totalGravado = "", string totalExento = "")
        {
            if (_cfdiComplementoNomina != null)
            {
                _cfdiComplementoNomina[1] = new object[]
                {
                    new[]
                    {
                        "Complemento|nomina|percepciones|",
                        totalGravado,
                        totalExento
                    },
                    new List<string[]>()
                };
            }
        }

        /// <summary>
        /// Agrega un registro del nodo "nomina:Percepcion" de un comprobante CFDI.
        /// </summary>
        /// <param name="tipoPercepcion">The tipo percepcion.</param>
        /// <param name="clave">The clave.</param>
        /// <param name="concepto">The concepto.</param>
        /// <param name="importeGravado">The importe gravado.</param>
        /// <param name="importeExento">The importe exento.</param>
        public void AgregarPercepcionNomina(string tipoPercepcion = "", string clave = "", string concepto = "", string importeGravado = "", string importeExento = "")
        {
            if (_cfdiComplementoNomina != null)
            {
                var percepciones = (List<string[]>)((object[])_cfdiComplementoNomina[1])[1];
                percepciones.Add(new[]
                {
                    "Complemento|nomina|percepcion|",
                    tipoPercepcion,
                    clave,
                    concepto,
                    importeGravado,
                    importeExento
                });
                ((object[])_cfdiComplementoNomina[1])[1] = percepciones;
            }
        }

        /// <summary>
        /// Asigna los valores requeridos para el nodo "nomina:Deducciones" de un comprobante CFDI.
        /// </summary>
        /// <param name="totalGravado">The total gravado.</param>
        /// <param name="totalExento">The total exento.</param>
        public void SetNominaDeducciones(string totalGravado = "", string totalExento = "")
        {
            if (_cfdiComplementoNomina != null)
            {
                _cfdiComplementoNomina[2] = new object[]
                {
                    new[]
                    {
                        "Complemento|nomina|deducciones|",
                        totalGravado,
                        totalExento
                    },
                    new List<string[]>()
                };
            }
        }

        /// <summary>
        /// Agrega un registro del nodo "nomina:Deduccion" de un comprobante CFDI.
        /// </summary>
        /// <param name="tipoDeduccion">The tipo deduccion.</param>
        /// <param name="clave">The clave.</param>
        /// <param name="concepto">The concepto.</param>
        /// <param name="importeGravado">The importe gravado.</param>
        /// <param name="importeExento">The importe exento.</param>
        public void AgregarDeduccionNomina(string tipoDeduccion = "", string clave = "", string concepto = "", string importeGravado = "", string importeExento = "")
        {
            if (_cfdiComplementoNomina != null)
            {
                var deducciones = (List<string[]>)((object[])_cfdiComplementoNomina[2])[1];
                deducciones.Add(new[]
                {
                    "Complemento|nomina|deduccion|",
                    tipoDeduccion,
                    clave,
                    concepto,
                    importeGravado,
                    importeExento
                });
                ((object[])_cfdiComplementoNomina[2])[1] = deducciones;
            }
        }

        /// <summary>
        /// Agrega un registro del nodo "nomina:Incapacidad" de un comprobante CFDI.
        /// </summary>
        /// <param name="diasIncapacidad">The dias incapacidad.</param>
        /// <param name="tipoIncapacidad">The tipo incapacidad.</param>
        /// <param name="descuento">The descuento.</param>
        public void AgregarIncapacidadNomina(string diasIncapacidad = "", string tipoIncapacidad = "", string descuento = "")
        {
            if (_cfdiComplementoNomina != null)
            {
                var deducciones = (List<string[]>)_cfdiComplementoNomina[3];
                deducciones.Add(new[]
                {
                    "Complemento|nomina|incapacidad|",
                    diasIncapacidad,
                    tipoIncapacidad,
                    descuento
                });
                _cfdiComplementoNomina[3] = deducciones;
            }
        }

        /// <summary>
        /// Agrega un registro del nodo "nomina:HorasExtra" de un comprobante CFDI.
        /// </summary>
        /// <param name="dias">The dias.</param>
        /// <param name="tipoHoras">The tipo horas.</param>
        /// <param name="horasExtra">The horas extra.</param>
        /// <param name="importePagado">The importe pagado.</param>
        public void AgregarHorasExtraNomina(string dias = "", string tipoHoras = "", string horasExtra = "", string importePagado = "")
        {
            if (_cfdiComplementoNomina != null)
            {
                var deducciones = (List<string[]>)_cfdiComplementoNomina[4];
                deducciones.Add(new[]
                {
                    "Complemento|nomina|horasExtras|",
                    dias,
                    tipoHoras,
                    horasExtra,
                    importePagado
                });
                _cfdiComplementoNomina[4] = deducciones;
            }
        }

        /// <summary>
        /// Agregars the leyenda fiscal cfdi.
        /// </summary>
        /// <param name="disposicionFiscal">The disposicion fiscal.</param>
        /// <param name="norma">The norma.</param>
        /// <param name="textoLeyenda">The texto leyenda.</param>
        public void AgregarLeyendaFiscalCfdi(string disposicionFiscal = "", string norma = "", string textoLeyenda = "")
        {
            if (_cfdiArrayLeyendasFiscales == null)
            {
                _cfdiArrayLeyendasFiscales = new List<string[]>();
            }

            var leyenda = new[]
            {
                "Complemento|leyendaFiscal|",
                disposicionFiscal,
                norma,
                textoLeyenda
            };
            _cfdiArrayLeyendasFiscales.Add(leyenda);
        }

        /// <summary>
        /// Sets the comercio exterior cfdi.
        /// </summary>
        /// <param name="motivoTraslado">The motivo traslado.</param>
        /// <param name="tipoOperacion">The tipo operacion.</param>
        /// <param name="claveDePedimento">The clave de pedimento.</param>
        /// <param name="certificadoOrigen">The certificado origen.</param>
        /// <param name="numCertificadoOrigen">The number certificado origen.</param>
        /// <param name="numeroExportadorConfiable">The numero exportador confiable.</param>
        /// <param name="incoterm">The incoterm.</param>
        /// <param name="subdivision">The subdivision.</param>
        /// <param name="observaciones">The observaciones.</param>
        /// <param name="tipoCambioUSD">The tipo cambio usd.</param>
        /// <param name="totalUSD">The total usd.</param>
        public void SetComercioExteriorCfdi(string motivoTraslado = "", string tipoOperacion = "", string claveDePedimento = "", string certificadoOrigen = "", string numCertificadoOrigen = "", string numeroExportadorConfiable = "", string incoterm = "", string subdivision = "", string observaciones = "", string tipoCambioUSD = "", string totalUSD = "")
        {
            var comercio = new[]
            {
                "Complemento|ComercioExterior|",
                motivoTraslado,
                tipoOperacion,
                claveDePedimento,
                certificadoOrigen,
                numCertificadoOrigen,
                numeroExportadorConfiable,
                incoterm,
                subdivision,
                observaciones,
                tipoCambioUSD,
                totalUSD
            };
            _cfdiComplementoComercioExterior = new object[]
            {
                comercio,
                null, //_cfdiEmisorComercioExterior, //[1]
                null, //_cfdiDomicilioEmisorComercioExterior, //[2]
                null, //_cfdiPropietarioComercioExterior, //[3]
                null, //_cfdiReceptorComercioExterior, //[4]
                null, //_cfdiDomicilioReceptorComercioExterior, //[5]
                null, //_cfdiDestinatarioComercioExterior, //[6]
                null //_cfdiMercanciasComercioExterior //[7]
            };
        }

        /// <summary>
        /// Sets the emisor comercio exterior cfdi.
        /// </summary>
        /// <param name="calle">The calle.</param>
        /// <param name="numeroExterior">The numero exterior.</param>
        /// <param name="numeroInterior">The numero interior.</param>
        /// <param name="colonia">The colonia.</param>
        /// <param name="localidad">The localidad.</param>
        /// <param name="referencia">The referencia.</param>
        /// <param name="municipio">The municipio.</param>
        /// <param name="estado">The estado.</param>
        /// <param name="pais">The pais.</param>
        /// <param name="codigoPostal">The codigo postal.</param>
        /// <param name="curp">The curp.</param>
        public void SetEmisorComercioExteriorCfdi(string calle = "", string numeroExterior = "", string numeroInterior = "", string colonia = "", string localidad = "", string referencia = "", string municipio = "", string estado = "", string pais = "", string codigoPostal = "", string curp = "")
        {
            if (_cfdiComplementoComercioExterior != null)
            {
                var emisor = new[]
                {
                    "Complemento|ComercioExterior|Emisor|",
                    curp
                };
                var domicilio = new[]
                {
                    "Complemento|ComercioExterior|Emisor|Domicilio|",
                    calle,
                    numeroExterior,
                    numeroInterior,
                    colonia,
                    localidad,
                    referencia,
                    municipio,
                    estado,
                    pais,
                    codigoPostal
                };
                _cfdiComplementoComercioExterior[1] = emisor;
                _cfdiComplementoComercioExterior[2] = domicilio;
            }
        }

        /// <summary>
        /// Agregars the propietario comercio exterior cfdi.
        /// </summary>
        /// <param name="numRegIdTrib">The number reg identifier trib.</param>
        /// <param name="residenciaFiscal">The residencia fiscal.</param>
        public void AgregarPropietarioComercioExteriorCfdi(string numRegIdTrib = "", string residenciaFiscal = "")
        {
            if (_cfdiComplementoComercioExterior != null)
            {
                var listaPropietarios = new List<string[]>();
                try
                {
                    listaPropietarios = (List<string[]>)_cfdiComplementoComercioExterior[3];
                }
                catch
                {
                }
                listaPropietarios.Add(new[]
                {
                    "Complemento|ComercioExterior|Propietario|",
                    numRegIdTrib,
                    residenciaFiscal
                });
                _cfdiComplementoComercioExterior[3] = listaPropietarios;
            }
        }

        /// <summary>
        /// Sets the receptor comercio exterior cfdi.
        /// </summary>
        /// <param name="calle">The calle.</param>
        /// <param name="numeroExterior">The numero exterior.</param>
        /// <param name="numeroInterior">The numero interior.</param>
        /// <param name="colonia">The colonia.</param>
        /// <param name="localidad">The localidad.</param>
        /// <param name="referencia">The referencia.</param>
        /// <param name="municipio">The municipio.</param>
        /// <param name="estado">The estado.</param>
        /// <param name="pais">The pais.</param>
        /// <param name="codigoPostal">The codigo postal.</param>
        /// <param name="numRegIdTrib">The number reg identifier trib.</param>
        public void SetReceptorComercioExteriorCfdi(string calle = "", string numeroExterior = "", string numeroInterior = "", string colonia = "", string localidad = "", string referencia = "", string municipio = "", string estado = "", string pais = "", string codigoPostal = "", string numRegIdTrib = "")
        {
            if (_cfdiComplementoComercioExterior != null)
            {
                var receptor = new[]
                {
                    "Complemento|ComercioExterior|Receptor|",
                    numRegIdTrib
                };
                var domicilio = new[]
                {
                    "Complemento|ComercioExterior|Receptor|Domicilio|",
                    calle,
                    numeroExterior,
                    numeroInterior,
                    colonia,
                    localidad,
                    referencia,
                    municipio,
                    estado,
                    pais,
                    codigoPostal
                };
                _cfdiComplementoComercioExterior[4] = receptor;
                _cfdiComplementoComercioExterior[5] = domicilio;
            }
        }

        /// <summary>
        /// Agregars the destinatario comercio exterior cfdi.
        /// </summary>
        /// <param name="calle">The calle.</param>
        /// <param name="numeroExterior">The numero exterior.</param>
        /// <param name="numeroInterior">The numero interior.</param>
        /// <param name="colonia">The colonia.</param>
        /// <param name="localidad">The localidad.</param>
        /// <param name="referencia">The referencia.</param>
        /// <param name="municipio">The municipio.</param>
        /// <param name="estado">The estado.</param>
        /// <param name="pais">The pais.</param>
        /// <param name="codigoPostal">The codigo postal.</param>
        /// <param name="numRegIdTrib">The number reg identifier trib.</param>
        /// <param name="nombre">The nombre.</param>
        public void AgregarDestinatarioComercioExteriorCfdi(string calle = "", string numeroExterior = "", string numeroInterior = "", string colonia = "", string localidad = "", string referencia = "", string municipio = "", string estado = "", string pais = "", string codigoPostal = "", string numRegIdTrib = "", string nombre = "")
        {
            if (_cfdiComplementoComercioExterior != null)
            {
                var destinatario = new[]
                {
                    "Complemento|ComercioExterior|Destinatario|",
                    numRegIdTrib,
                    nombre
                };
                var domicilio = new[]
                {
                    "Complemento|ComercioExterior|Destinatario|Domicilio|",
                    calle,
                    numeroExterior,
                    numeroInterior,
                    colonia,
                    localidad,
                    referencia,
                    municipio,
                    estado,
                    pais,
                    codigoPostal
                };
                var listaPropietarios = new List<List<string[]>>();
                try
                {
                    listaPropietarios = (List<List<string[]>>)_cfdiComplementoComercioExterior[6];
                }
                catch
                {
                }
                listaPropietarios.Add(new List<string[]>
                {
                    destinatario,
                    domicilio
                });
                _cfdiComplementoComercioExterior[6] = listaPropietarios;
            }
        }

        /// <summary>
        /// Agregars the mercancia comercio exterior cfdi.
        /// </summary>
        /// <param name="noIdentificacion">The no identificacion.</param>
        /// <param name="fraccionArancelaria">The fraccion arancelaria.</param>
        /// <param name="cantidadAduana">The cantidad aduana.</param>
        /// <param name="unidadAduana">The unidad aduana.</param>
        /// <param name="valorUnitarioAduana">The valor unitario aduana.</param>
        /// <param name="valorDolares">The valor dolares.</param>
        public void AgregarMercanciaComercioExteriorCfdi(string noIdentificacion = "", string fraccionArancelaria = "", string cantidadAduana = "", string unidadAduana = "", string valorUnitarioAduana = "", string valorDolares = "")
        {
            if (_cfdiComplementoComercioExterior != null)
            {
                var mercancia = new[]
                {
                    "Complemento|ComercioExterior|Mercancia|",
                    noIdentificacion,
                    fraccionArancelaria,
                    cantidadAduana,
                    unidadAduana,
                    valorUnitarioAduana,
                    valorDolares
                };
                var listaMercancias = new List<object[]>();
                try
                {
                    listaMercancias = (List<object[]>)_cfdiComplementoComercioExterior[7];
                }
                catch
                {
                }
                listaMercancias.Add(new object[]
                {
                    mercancia,
                    null
                });
                _cfdiComplementoComercioExterior[7] = listaMercancias;
            }
        }

        /// <summary>
        /// Agregars the especificacion mercancia comercio exterior cfdi.
        /// </summary>
        /// <param name="marca">The marca.</param>
        /// <param name="modelo">The modelo.</param>
        /// <param name="subModelo">The sub modelo.</param>
        /// <param name="numeroSerie">The numero serie.</param>
        public void AgregarEspecificacionMercanciaComercioExteriorCfdi(string marca = "", string modelo = "", string subModelo = "", string numeroSerie = "")
        {
            if (_cfdiComplementoComercioExterior != null)
            {
                var lastIndex = -1;
                var listaMercancias = new List<object[]>();
                try
                {
                    listaMercancias = (List<object[]>)_cfdiComplementoComercioExterior[7];
                }
                catch
                {
                }
                var mercanciaItem = listaMercancias.LastOrDefault();
                if (mercanciaItem != null)
                {
                    lastIndex = listaMercancias.IndexOf(mercanciaItem);
                    var mercancia = mercanciaItem[0];
                    var especificaciones = new List<string[]>();
                    try
                    {
                        especificaciones = (List<string[]>)mercanciaItem[1];
                    }
                    catch
                    {
                    }
                    var especificacion = new[]
                    {
                        "Complemento|ComercioExterior|Mercancia|Especificacion|",
                        marca,
                        modelo,
                        subModelo,
                        numeroSerie
                    };
                    especificaciones.Add(especificacion);
                    mercanciaItem[1] = especificaciones;
                }
                if (lastIndex >= 0)
                {
                    listaMercancias[lastIndex] = mercanciaItem;
                }

                _cfdiComplementoComercioExterior[7] = listaMercancias;
            }
        }

        /// <summary>
        /// Agregars the addenda tlalnepantla veci cfdi.
        /// </summary>
        /// <param name="ServiceType">Type of the service.</param>
        /// <param name="idProvider">The identifier provider.</param>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <param name="issueDateInvoice">The issue date invoice.</param>
        /// <param name="namePassanger">The name passanger.</param>
        /// <param name="recordLoc">The record loc.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endtDate">The endt date.</param>
        /// <param name="bonus">The bonus.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="taxXHostingServices">The tax x hosting services.</param>
        /// <param name="taxable">The taxable.</param>
        /// <param name="unTaxable">The un taxable.</param>
        /// <param name="vatRate">The vat rate.</param>
        /// <param name="vatAmount">The vat amount.</param>
        /// <param name="grossAmount">The gross amount.</param>
        /// <param name="retencionPercibida">The retencion percibida.</param>
        /// <param name="importeRetenido">The importe retenido.</param>
        public void AgregarAddendaTlalnepantlaVECICfdi(string ServiceType = "", string idProvider = "", string invoiceNumber = "", string issueDateInvoice = "", string namePassanger = "", string recordLoc = "", string startDate = "", string endtDate = "", string bonus = "", string currency = "", string taxXHostingServices = "", string taxable = "", string unTaxable = "", string vatRate = "", string vatAmount = "", string grossAmount = "", string retencionPercibida = "", string importeRetenido = "")
        {
            var arrayData = string.Join("|", ServiceType, idProvider, invoiceNumber, issueDateInvoice, namePassanger, recordLoc, startDate, endtDate, bonus, currency, taxXHostingServices, taxable, unTaxable, vatRate, vatAmount, grossAmount, retencionPercibida, importeRetenido);
            var dataBase64 = Convert.ToBase64String(Encoding.Default.GetBytes(arrayData));
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|TplVECI|",
                dataBase64
            });
        }

        public void AgregarAddendaBdi(string name, string jsonData)
        {
            var dataBase64 = Convert.ToBase64String(Encoding.Default.GetBytes(jsonData));
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|Bdi|",
                name,
                dataBase64
            });
        }

        public void AgregarAddendaHerbalife(string OrderNumber, string DSID)
        {
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|Herbalife|",
                OrderNumber,
                DSID
            });
        }

        /// <summary>
        /// Agregars the addenda hampton eum cfdi.
        /// </summary>
        /// <param name="transnnumber">The transnnumber.</param>
        /// <param name="numberorder">The numberorder.</param>
        /// <param name="albaran">The albaran.</param>
        /// <param name="numsec">The numsec.</param>
        /// <param name="typecurrency">The typecurrency.</param>
        /// <param name="typechange">The typechange.</param>
        /// <param name="tax">The tax.</param>
        /// <param name="Subtotales">The subtotales.</param>
        /// <param name="totalM">The total m.</param>
        /// <param name="impuestoM">The impuesto m.</param>
        public void AgregarAddendaHamptonEUMCfdi(string transnnumber = "", string numberorder = "", string albaran = "", string numsec = "1", string typecurrency = "", string typechange = "", string tax = "", string Subtotales = "", string totalM = "", string impuestoM = "")


        {
            var arrayData = string.Join("|", transnnumber, numberorder, albaran, numsec, typecurrency, typechange, tax, Subtotales, totalM, impuestoM);
            var dataBAse64 = Convert.ToBase64String(Encoding.Default.GetBytes(arrayData));
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|HmpEUM|",
                dataBAse64
            });
        }

        /// <summary>
        /// Agregars the addenda hampton an ffdi.
        /// </summary>
        /// <param name="folioOrdenCompra">The folio orden compra.</param>
        /// <param name="codigoAplicacion">The codigo aplicacion.</param>
        public void AgregarAddendaHamptonANFfdi(string folioOrdenCompra = "", string codigoAplicacion = "")
        {
            var arrayData = string.Join("|", folioOrdenCompra, codigoAplicacion);
            var dataBAse64 = Convert.ToBase64String(Encoding.Default.GetBytes(arrayData));
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|HmpANF|",
                dataBAse64
            });
        }

        /// <summary>
        /// Agregars the addenda hampton z ffdi.
        /// </summary>
        /// <param name="orden">The orden.</param>
        /// <param name="cantidad">The cantidad.</param>
        /// <param name="valoruni">The valoruni.</param>
        /// <param name="numerosec">The numerosec.</param>
        /// <param name="importe">The importe.</param>
        /// <param name="tipomoneda">The tipomoneda.</param>
        public void AgregarAddendaHamptonZFfdi(string orden = "", string cantidad = "", string valoruni = "", string numerosec = "1", string importe = "", string tipomoneda = "")
        {
            var arrayData = string.Join("|", orden, cantidad, valoruni, numerosec, importe, tipomoneda);
            var dataBAse64 = Convert.ToBase64String(Encoding.Default.GetBytes(arrayData));
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|HmpZF|",
                dataBAse64
            });
        }

        /// <summary>
        /// Agregars the addenda hampton samin afdi.
        /// </summary>
        /// <param name="Orden">The orden.</param>
        /// <param name="Correo">The correo.</param>
        /// <param name="Razon">The razon.</param>
        /// <param name="numerosec">The numerosec.</param>
        /// <param name="nProveedor">The n proveedor.</param>
        /// <param name="tMoneda">The t moneda.</param>
        /// <param name="tCambio">The t cambio.</param>
        /// <param name="nFactura">The n factura.</param>
        public void AgregarAddendaHamptonSAMINAfdi(string Orden = "", string Correo = "", string Razon = "", string numerosec = "1", string nProveedor = "", string tMoneda = "", string tCambio = "", string nFactura = "")
        {
            var arrayData = string.Join("|", Orden, Correo, Razon, numerosec, nProveedor, tMoneda, tCambio, nFactura);
            var dataBAse64 = Convert.ToBase64String(Encoding.Default.GetBytes(arrayData));
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|HmpSAMINA|",
                dataBAse64
            });
        }

        /// <summary>
        /// Agregars the addenda hampton agen efdi.
        /// </summary>
        /// <param name="MONEDA">The moneda.</param>
        /// <param name="CAMBIO">The cambio.</param>
        public void AgregarAddendaHamptonAGENEfdi(string MONEDA = "", string CAMBIO = "")
        {
            var arrayData = string.Join("|", MONEDA, CAMBIO);
            var dataBAse64 = Convert.ToBase64String(Encoding.Default.GetBytes(arrayData));
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|HmpGENE|",
                dataBAse64
            });
        }

        public void AgregarAddendaReformaMesCfdi(string mes = "")
        {
            _cfdiArrayAddendas.Add(new[]
            {
                "Addenda|GrupoReforma|MesEmision|",
                mes
            });
        }

        #endregion

        #region Variables

        /// <summary>
        /// The _cfdi array comprobante
        /// </summary>
        private string[] _cfdiArrayComprobante;

        /// <summary>
        /// The _cfdi array emisor
        /// </summary>
        private string[] _cfdiArrayEmisor;

        /// <summary>
        /// The _cfdi array emisor DOM
        /// </summary>
        private string[] _cfdiArrayEmisorDom;

        /// <summary>
        /// The _cfdi array emisor exp
        /// </summary>
        private string[] _cfdiArrayEmisorExp;

        /// <summary>
        /// The _cfdiarray emisor regimen
        /// </summary>
        private string[] _cfdiarrayEmisorRegimen;

        /// <summary>
        /// The _cfdi array receptor
        /// </summary>
        private string[] _cfdiArrayReceptor;

        /// <summary>
        /// The _cfdi array receptor DOM
        /// </summary>
        private string[] _cfdiArrayReceptorDom;

        /// <summary>
        /// The cfdi array receptor DOM recepcion
        /// </summary>
        private string[] _cfdiArrayReceptorDomRecepcion;

        /// <summary>
        /// The _cfdi array receptor cont
        /// </summary>
        private List<string[]> _cfdiArrayReceptorCont;

        /// <summary>
        /// The _cfdi complemento nomina
        /// </summary>
        private object[] _cfdiComplementoNomina;

        /// <summary>
        /// The _cfdi complemento ine
        /// </summary>
        public object[] CfdiComplementoIne;

        //private object[] _cfdiComplementoDonatarias;

        /// <summary>
        /// The cfdi complemento comercio exterior
        /// </summary>
        private object[] _cfdiComplementoComercioExterior;

        /// <summary>
        /// The _cfdi array conceptos
        /// </summary>
        private List<object[]> _cfdiArrayConceptos;

        /// <summary>
        /// The cfdi array leyendas fiscales
        /// </summary>
        private List<string[]> _cfdiArrayLeyendasFiscales;

        /// <summary>
        /// The _cfdi array cantidad impuestos
        /// </summary>
        private string[] _cfdiArrayCantidadImpuestos;

        /// <summary>
        /// The _cfdi array impuestos
        /// </summary>
        private List<string[]> _cfdiArrayImpuestos;

        /// <summary>
        /// The _cfdi array impuestos locales
        /// </summary>
        private string[] _cfdiArrayImpuestosLocales;

        /// <summary>
        /// The _cfdi array traslados locales
        /// </summary>
        private List<string[]> _cfdiArrayTrasladosLocales;

        /// <summary>
        /// The _cfdi array retenciones locales
        /// </summary>
        private List<string[]> _cfdiArrayRetencionesLocales;

        /// <summary>
        /// The _cfdi array ia
        /// </summary>
        private List<string[]> _cfdiArrayIa;

        /// <summary>
        /// The cfdi array addendas
        /// </summary>
        private readonly List<string[]> _cfdiArrayAddendas;

        /// <summary>
        /// The _ret array comprobante
        /// </summary>
        private string[] _retArrayComprobante;

        /// <summary>
        /// The _ret array periodo
        /// </summary>
        private string[] _retArrayPeriodo;

        /// <summary>
        /// The _ret array emisor
        /// </summary>
        private string[] _retArrayEmisor;

        /// <summary>
        /// The _ret array receptor
        /// </summary>
        private string[] _retArrayReceptor;

        /// <summary>
        /// The _ret array receptor nac
        /// </summary>
        private string[] _retArrayReceptorNac;

        /// <summary>
        /// The _ret array receptor ext
        /// </summary>
        private string[] _retArrayReceptorExt;

        /// <summary>
        /// The _ret array impuestos
        /// </summary>
        private List<string[]> _retArrayImpuestos;

        /// <summary>
        /// The _ret array totales
        /// </summary>
        private string[] _retArrayTotales;

        /// <summary>
        /// The _ret complemento pagos ext
        /// </summary>
        private string[] _retComplementoPagosExt;

        /// <summary>
        /// The _ret complemento pagos ext b
        /// </summary>
        private string[] _retComplementoPagosExtB;

        /// <summary>
        /// The _ret complemento pagos ext nb
        /// </summary>
        private string[] _retComplementoPagosExtNb;

        /// <summary>
        /// The _ret complemento dividendos
        /// </summary>
        private string[] _retComplementoDividendos;

        /// <summary>
        /// The _ret complemento dividendos remanente
        /// </summary>
        private string[] _retComplementoDividendosRemanente;

        #endregion
    }
}
