// ***********************************************************************
// Assembly         : Control
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="Informes.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ClosedXML.Excel;
using Datos;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Control
{
    /// <summary>
    /// Class Informes.
    /// </summary>
    public class Informes
    {
        /// <summary>
        /// The _connection string
        /// </summary>
        private readonly string _connectionString = "";
        /// <summary>
        /// The _periodo
        /// </summary>
        private readonly string _periodo = "";

        /// <summary>
        /// The _ruta document
        /// </summary>
        private readonly string _rutaDoc = "";
        /// <summary>
        /// The ex
        /// </summary>
        public Exception Ex;
        /// <summary>
        /// The is recepcion
        /// </summary>
        private readonly bool _isRecepcion = false;
        /// <summary>
        /// The empresa tipo
        /// </summary>
        private readonly string _empresaTipo = "";
        /// <summary>
        /// The database name emision
        /// </summary>
        private readonly string _dbNameEmision = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="Informes" /> class.
        /// </summary>
        /// <param name="rutaDoc">Ruta del documento a generar.</param>
        /// <param name="connectionString">Cadena de conexión de la base de datos a utilizar para las consultas.</param>
        /// <param name="periodo">El periodo que contempla el reporte.</param>
        /// <param name="empresaTipo">The empresa tipo.</param>
        /// <param name="isRecepcion">if set to <c>true</c> [is recepcion].</param>
        /// <param name="dbNameEmision">The database name emision.</param>
        public Informes(string rutaDoc, string connectionString, string periodo, string empresaTipo, bool isRecepcion = false, string dbNameEmision = "")
        {
            _connectionString = connectionString;
            _rutaDoc = rutaDoc;
            _periodo = periodo;
            _isRecepcion = isRecepcion;
            _empresaTipo = empresaTipo;
            _dbNameEmision = dbNameEmision;
        }

        /// <summary>
        /// Genera el reporte general con la condición sql especificada.
        /// </summary>
        /// <param name="sqlWhere">Condición SQL.</param>
        /// <returns><c>true</c> si se genera el reporte correctamente, <c>false</c> de lo contrario.</returns>
        public bool General(string sqlWhere, string sessionIDENTEMI="")
        {
            Ex = null;
            var result = false;
            var sqlQuery = "";
            var initialColumnNumber = 0;
            var finalColumnNumber = 0;
            var rfcEmiCore = "";

            var db = new BasesDatos("CORE");
            db.Conectar();
            db.CrearComando("SELECT RFCEMI FROM Cat_Emisor WHERE RFCEMI = @RFCEMI");
            db.AsignarParametroCadena("@RFCEMI", sessionIDENTEMI);
            var dr = db.EjecutarConsulta();
            if (dr.Read())
            {
                rfcEmiCore = dr["RFCEMI"].ToString();
            }

            db.Desconectar();

            if (!_isRecepcion)
            {
                if (_rutaDoc.Contains("TCA130827M58"))
                {
                    sqlQuery = @"SELECT 
    cem.RFCEMI AS 'RFC EMISOR', cem.NOMEMI AS 'RAZÓN SOCIAL EMISOR', Cat_Catalogo1_C.descripcion AS COMPROBANTE, g.tipoDeComprobante AS 'TIPO', g.folio AS 'FOLIO', g.serie AS 'SERIE', g2.serie AS 'SERIE MODIFICADA', g2.folio AS 'FOLIO MODIFICADO', g.fecha AS 'FECHA EMISIÓN', cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR', g.folioReservacion AS 'CONFIRMACIÓN', dhe.noHabitacion AS 'HABITACIÓN', CAST(g.subTotal AS money) AS SUBTOTAL, CAST(g.IVA12 AS money) AS IVA, CAST(g.ImpuestoHopedaje AS money) AS ISH, CAST(g.total AS money) AS TOTAL, CAST(g.propina AS money) AS 'PROPINAS',CAST(g.cargoxservicio AS money) AS 'OTROS CARGOS', CAST(g.importeAPagar AS money) AS 'A PAGAR', g.moneda AS MONEDA, g.tipoCambio AS 'TIPO DE CAMBIO', (g.tipo + g.estado) + (CASE (g.tipo + g.estado) WHEN 'E1' THEN ': Timbrado/Vigente' WHEN 'E4' THEN ': Timbrado/Vigente y Anulado por Nota de Crédito'  WHEN 'N0' THEN ': No Timbrado' WHEN 'C0' THEN ': Cancelado ante SAT' WHEN 'C2' THEN ': En proceso de Cancelación ante SAT' ELSE '' END) AS 'ESTADO', ce.userEmpleado + ': ' + ce.nombreEmpleado AS 'REALIZADO POR', ce2.userEmpleado + ': ' + ce2.nombreEmpleado AS 'CANCELADO POR', (CASE ISNULL(CONVERT(VARCHAR, g.metodoPago), '') WHEN '' THEN '98' ELSE ISNULL(ccMp.descripcion, g.MetodoPago) END) AS 'METODO PAGO', dp.numCtaPago AS 'NUM. CTA.', g.numeroAutorizacion AS UUID, g.numDocModificado AS 'UUID MODIFICADOS', g.version AS VERSION, g.usoCfdi AS 'USO DE CFDI',
(Select TOP(1) SUM(d.preciototalsinimpuestos)  AS 'AYB' from dat_detalles d INNER JOIN cat_catconceptos_c c on 
d.descripcion = c.descripcion inner join cat_catalogo1_c cc on c.idcategoria = cc.idcatalogo1_c left outer join cat_cveunidad u 
on u.descripcion = c.unidadMedida where d.id_comprobante = G.idComprobante AND cc.descripcion = 'HABITACIONES') as 'HABITACION',
(Select TOP(1) SUM(d.preciototalsinimpuestos)  AS 'AYB' from dat_detalles d INNER JOIN cat_catconceptos_c c on 
d.descripcion = c.descripcion inner join cat_catalogo1_c cc on c.idcategoria = cc.idcatalogo1_c left outer join cat_cveunidad u 
on u.descripcion = c.unidadMedida where d.id_comprobante = G.idComprobante AND cc.descripcion = 'Miscelaneos') as 'MISELANEOS',
(Select TOP(1) SUM(d.preciototalsinimpuestos)  AS 'AYB' from dat_detalles d INNER JOIN cat_catconceptos_c c on 
d.descripcion = c.descripcion inner join cat_catalogo1_c cc on c.idcategoria = cc.idcatalogo1_c left outer join cat_cveunidad u 
on u.descripcion = c.unidadMedida where d.id_comprobante = G.idComprobante AND cc.descripcion = 'AYB') as 'AYB',
(Select TOP(1) SUM(d.preciototalsinimpuestos)  AS 'AYB' from dat_detalles d INNER JOIN cat_catconceptos_c c on 
d.descripcion = c.descripcion inner join cat_catalogo1_c cc on c.idcategoria = cc.idcatalogo1_c left outer join cat_cveunidad u 
on u.descripcion = c.unidadMedida where d.id_comprobante = G.idComprobante AND cc.descripcion = 'Anticipos') as 'ANTICIPOS',
CAST(g.subTotal AS money) AS SUBTOTAL12,g.totaldescuento AS DESCUENTOS, CAST(g.IVA12 AS money) AS IVA2,CONVERT(VARCHAR,CAST(g.propina AS money), 1) AS '10% SERVICIO',
CAST(g.ImpuestoHopedaje AS money) AS '3% HOSPEDAJE', 
CAST(g.total AS money) AS TOTAL
FROM
    Dat_General g LEFT OUTER JOIN
    Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
    Cat_Receptor cr ON g.id_Receptor = cr.IDEREC LEFT OUTER JOIN
    Dat_pagos dp ON g.idComprobante = dp.id_Comprobante LEFT OUTER JOIN
    Dat_HabitacionEvento dhe ON g.idComprobante = dhe.id_Comprobante INNER JOIN
    Cat_Empleados ce ON g.id_Empleado = ce.idEmpleado LEFT OUTER JOIN
    Cat_Empleados ce2 ON g.id_Empleado_Canc = ce2.idEmpleado INNER JOIN
    Cat_Catalogo1_C ON Cat_Catalogo1_C.codigo = g.codDoc AND Cat_Catalogo1_C.tipo = 'Comprobante' LEFT OUTER JOIN
    Cat_Catalogo1_C ccMp ON CONVERT(VARCHAR, g.metodoPago) = ccMp.codigo AND ccMp.tipo = 'MetodoPago' LEFT OUTER JOIN
    Dat_General g2 ON g.numDocModificado = g2.numeroAutorizacion AND ISNULL(g.numDocModificado, '') <> ''";
                    initialColumnNumber = 14;
                    finalColumnNumber = 20;
                }
                else
                {

                    switch (_empresaTipo)
                    {
                    case "1":
                            if (rfcEmiCore.Equals("YAN810728RW7") || sessionIDENTEMI.Equals("YAN810728RW7"))
                            {
                                sqlQuery = @"SELECT
    cem.RFCEMI AS 'RFC EMISOR', cem.NOMEMI AS 'RAZÓN SOCIAL EMISOR', Cat_Catalogo1_C.descripcion AS COMPROBANTE, g.tipoDeComprobante AS 'TIPO', g.folio AS 'FOLIO', g.serie AS 'SERIE', g2.serie AS 'SERIE MODIFICADA', g2.folio AS 'FOLIO MODIFICADO', g.fecha AS 'FECHA EMISIÓN', cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR', g.folioReservacion AS 'CONFIRMACIÓN', dhe.noHabitacion AS 'HABITACIÓN', CAST(g.subTotal AS money) AS SUBTOTAL, CAST(g.IVA12 AS money) AS IVA, CAST(g.ImpuestoHopedaje AS money) AS ISH, CAST(g.total AS money) AS TOTAL, CAST(g.propina AS money) AS 'PROPINAS',CAST(g.cargoxservicio AS money) AS 'OTROS CARGOS', CAST(g.importeAPagar AS money) AS 'A PAGAR', g.moneda AS MONEDA, g.tipoCambio AS 'TIPO DE CAMBIO', (g.tipo + g.estado) + (CASE (g.tipo + g.estado) WHEN 'E1' THEN ': Timbrado/Vigente' WHEN 'E4' THEN ': Timbrado/Vigente y Anulado por Nota de Crédito'  WHEN 'N0' THEN ': No Timbrado' WHEN 'C0' THEN ': Cancelado ante SAT' WHEN 'C2' THEN ': En proceso de Cancelación ante SAT' ELSE '' END) AS 'ESTADO', ce.userEmpleado + ': ' + ce.nombreEmpleado AS 'REALIZADO POR', ce2.userEmpleado + ': ' + ce2.nombreEmpleado AS 'CANCELADO POR', (CASE ISNULL(CONVERT(VARCHAR, g.metodoPago), '') WHEN '' THEN '98' ELSE ISNULL(ccMp.descripcion, g.MetodoPago) END) AS 'METODO PAGO', dp.numCtaPago AS 'NUM. CTA.', g.numeroAutorizacion AS UUID, g.numDocModificado AS 'UUID MODIFICADOS', g.version AS VERSION, g.usoCfdi AS 'USO DE CFDI'
FROM
    Dat_General g LEFT OUTER JOIN
    Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
    Cat_Receptor cr ON g.id_Receptor = cr.IDEREC LEFT OUTER JOIN
    Dat_pagos dp ON g.idComprobante = dp.id_Comprobante LEFT OUTER JOIN
    Dat_HabitacionEvento dhe ON g.idComprobante = dhe.id_Comprobante INNER JOIN
    Cat_Empleados ce ON g.id_Empleado = ce.idEmpleado LEFT OUTER JOIN
    Cat_Empleados ce2 ON g.id_Empleado_Canc = ce2.idEmpleado INNER JOIN
    Cat_Catalogo1_C ON Cat_Catalogo1_C.codigo = g.codDoc AND Cat_Catalogo1_C.tipo = 'Comprobante' LEFT OUTER JOIN
    Cat_Catalogo1_C ccMp ON CONVERT(VARCHAR, g.metodoPago) = ccMp.codigo AND ccMp.tipo = 'MetodoPago' LEFT OUTER JOIN
    Dat_General g2 ON g.numDocModificado = g2.numeroAutorizacion AND ISNULL(g.numDocModificado, '') <> ''";
                                initialColumnNumber = 14;
                                finalColumnNumber = 20;
                                
                            }
                            else
                            {
                                sqlQuery = @"SELECT
    Cat_Catalogo1_C.descripcion AS COMPROBANTE, g.tipoDeComprobante AS 'TIPO', g.folio AS 'FOLIO', g.serie AS 'SERIE', g2.serie AS 'SERIE MODIFICADA', g2.folio AS 'FOLIO MODIFICADO', g.fecha AS 'FECHA EMISIÓN', cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR', g.folioReservacion AS 'CONFIRMACIÓN', dhe.noHabitacion AS 'HABITACIÓN', CAST(g.subTotal AS money) AS SUBTOTAL, CAST(g.IVA12 AS money) AS IVA, CAST(g.ImpuestoHopedaje AS money) AS ISH, CAST(g.total AS money) AS TOTAL, CAST(g.propina AS money) AS 'PROPINAS',CAST(g.cargoxservicio AS money) AS 'OTROS CARGOS', CAST(g.importeAPagar AS money) AS 'A PAGAR', g.moneda AS MONEDA, g.tipoCambio AS 'TIPO DE CAMBIO', (g.tipo + g.estado) + (CASE (g.tipo + g.estado) WHEN 'E1' THEN ': Timbrado/Vigente' WHEN 'E4' THEN ': Timbrado/Vigente y Anulado por Nota de Crédito'  WHEN 'N0' THEN ': No Timbrado' WHEN 'C0' THEN ': Cancelado ante SAT' WHEN 'C2' THEN ': En proceso de Cancelación ante SAT' ELSE '' END) AS 'ESTADO', ce.userEmpleado + ': ' + ce.nombreEmpleado AS 'REALIZADO POR', ce2.userEmpleado + ': ' + ce2.nombreEmpleado AS 'CANCELADO POR', (CASE ISNULL(CONVERT(VARCHAR, g.metodoPago), '') WHEN '' THEN '98' ELSE ISNULL(ccMp.descripcion, g.MetodoPago) END) AS 'METODO PAGO', dp.numCtaPago AS 'NUM. CTA.', g.numeroAutorizacion AS UUID, g.numDocModificado AS 'UUID MODIFICADOS', g.version AS VERSION, g.usoCfdi AS 'USO DE CFDI'
FROM
    Dat_General g LEFT OUTER JOIN
    Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
    Cat_Receptor cr ON g.id_Receptor = cr.IDEREC LEFT OUTER JOIN
    Dat_pagos dp ON g.idComprobante = dp.id_Comprobante LEFT OUTER JOIN
    Dat_HabitacionEvento dhe ON g.idComprobante = dhe.id_Comprobante INNER JOIN
    Cat_Empleados ce ON g.id_Empleado = ce.idEmpleado LEFT OUTER JOIN
    Cat_Empleados ce2 ON g.id_Empleado_Canc = ce2.idEmpleado INNER JOIN
    Cat_Catalogo1_C ON Cat_Catalogo1_C.codigo = g.codDoc AND Cat_Catalogo1_C.tipo = 'Comprobante' LEFT OUTER JOIN
    Cat_Catalogo1_C ccMp ON CONVERT(VARCHAR, g.metodoPago) = ccMp.codigo AND ccMp.tipo = 'MetodoPago' LEFT OUTER JOIN
    Dat_General g2 ON g.numDocModificado = g2.numeroAutorizacion AND ISNULL(g.numDocModificado, '') <> ''";
                                initialColumnNumber = 12;
                                finalColumnNumber = 18;
                            }
                            break;

                        case "2":
                            if (rfcEmiCore.Equals("YAN810728RW7") || sessionIDENTEMI.Equals("YAN810728RW7"))
                            {
                                sqlQuery = @"SELECT
    cem.RFCEMI AS 'RFC EMISOR', cem.NOMEMI AS 'RAZÓN SOCIAL EMISOR', Cat_Catalogo1_C.descripcion AS COMPROBANTE, g.tipoDeComprobante AS 'TIPO', g.folio AS 'FOLIO', g.serie AS 'SERIE', g2.serie AS 'SERIE MODIFICADA', g2.folio AS 'FOLIO MODIFICADO', g.fecha AS 'FECHA EMISIÓN', cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR', (CASE ISNULL(g.noTicket, '') WHEN '' THEN g.folioReservacion ELSE g.noTicket END) AS 'TICKET', CAST(g.subTotal AS money) AS SUBTOTAL, CAST(g.IVA12 AS money) AS IVA, CAST(g.total AS money) AS TOTAL, CAST(g.propina AS money) AS 'PROPINAS',CAST(g.cargoxservicio AS money) AS 'OTROS CARGOS', CAST(g.importeAPagar AS money) AS 'A PAGAR', g.moneda AS MONEDA, g.tipoCambio AS 'TIPO DE CAMBIO', (g.tipo + g.estado) + (CASE (g.tipo + g.estado) WHEN 'E1' THEN ': Timbrado/Vigente' WHEN 'E4' THEN ': Timbrado/Vigente y Anulado por Nota de Crédito'  WHEN 'N0' THEN ': No Timbrado' WHEN 'C0' THEN ': Cancelado ante SAT' WHEN 'C2' THEN ': En proceso de Cancelación ante SAT' ELSE '' END) AS 'ESTADO', ce.userEmpleado + ': ' + ce.nombreEmpleado AS 'REALIZADO POR', ce2.userEmpleado + ': ' + ce2.nombreEmpleado AS 'CANCELADO POR', (CASE ISNULL(CONVERT(VARCHAR, g.metodoPago), '') WHEN '' THEN '98' ELSE ISNULL(ccMp.descripcion, g.MetodoPago) END) AS 'METODO PAGO', dp.numCtaPago AS 'NUM. CTA.', g.numeroAutorizacion AS UUID, g.numDocModificado AS 'UUID MODIFICADOS', g.version AS VERSION, g.usoCfdi AS 'USO DE CFDI'
FROM    
    Dat_General g LEFT OUTER JOIN
    Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
    Cat_Receptor cr ON g.id_Receptor = cr.IDEREC LEFT OUTER JOIN
    Dat_pagos dp ON g.idComprobante = dp.id_Comprobante LEFT OUTER JOIN
    Dat_HabitacionEvento dhe ON g.idComprobante = dhe.id_Comprobante INNER JOIN
    Cat_Empleados ce ON g.id_Empleado = ce.idEmpleado LEFT OUTER JOIN
    Cat_Empleados ce2 ON g.id_Empleado_Canc = ce2.idEmpleado INNER JOIN
    Cat_Catalogo1_C ON Cat_Catalogo1_C.codigo = g.codDoc AND Cat_Catalogo1_C.tipo = 'Comprobante' LEFT OUTER JOIN
    Cat_Catalogo1_C ccMp ON CONVERT(VARCHAR, g.metodoPago) = ccMp.codigo AND ccMp.tipo = 'MetodoPago' LEFT OUTER JOIN
    Dat_General g2 ON g.numDocModificado = g2.numeroAutorizacion AND ISNULL(g.numDocModificado, '') <> ''";
                                initialColumnNumber = 13;
                                finalColumnNumber = 18;
                            }
                            else
                            {
                                sqlQuery = @"SELECT
    Cat_Catalogo1_C.descripcion AS COMPROBANTE, g.tipoDeComprobante AS 'TIPO', g.folio AS 'FOLIO', g.serie AS 'SERIE', g2.serie AS 'SERIE MODIFICADA', g2.folio AS 'FOLIO MODIFICADO', g.fecha AS 'FECHA EMISIÓN', cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR', (CASE ISNULL(g.noTicket, '') WHEN '' THEN g.folioReservacion ELSE g.noTicket END) AS 'TICKET', CAST(g.subTotal AS money) AS SUBTOTAL, CAST(g.IVA12 AS money) AS IVA, CAST(g.total AS money) AS TOTAL, CAST(g.propina AS money) AS 'PROPINAS',CAST(g.cargoxservicio AS money) AS 'OTROS CARGOS', CAST(g.importeAPagar AS money) AS 'A PAGAR', g.moneda AS MONEDA, g.tipoCambio AS 'TIPO DE CAMBIO', (g.tipo + g.estado) + (CASE (g.tipo + g.estado) WHEN 'E1' THEN ': Timbrado/Vigente' WHEN 'E4' THEN ': Timbrado/Vigente y Anulado por Nota de Crédito'  WHEN 'N0' THEN ': No Timbrado' WHEN 'C0' THEN ': Cancelado ante SAT' WHEN 'C2' THEN ': En proceso de Cancelación ante SAT' ELSE '' END) AS 'ESTADO', ce.userEmpleado + ': ' + ce.nombreEmpleado AS 'REALIZADO POR', ce2.userEmpleado + ': ' + ce2.nombreEmpleado AS 'CANCELADO POR', (CASE ISNULL(CONVERT(VARCHAR, g.metodoPago), '') WHEN '' THEN '98' ELSE ISNULL(ccMp.descripcion, g.MetodoPago) END) AS 'METODO PAGO', dp.numCtaPago AS 'NUM. CTA.', g.numeroAutorizacion AS UUID, g.numDocModificado AS 'UUID MODIFICADOS', g.version AS VERSION, g.usoCfdi AS 'USO DE CFDI'
FROM    
    Dat_General g LEFT OUTER JOIN
    Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
    Cat_Receptor cr ON g.id_Receptor = cr.IDEREC LEFT OUTER JOIN
    Dat_pagos dp ON g.idComprobante = dp.id_Comprobante LEFT OUTER JOIN
    Dat_HabitacionEvento dhe ON g.idComprobante = dhe.id_Comprobante INNER JOIN
    Cat_Empleados ce ON g.id_Empleado = ce.idEmpleado LEFT OUTER JOIN
    Cat_Empleados ce2 ON g.id_Empleado_Canc = ce2.idEmpleado INNER JOIN
    Cat_Catalogo1_C ON Cat_Catalogo1_C.codigo = g.codDoc AND Cat_Catalogo1_C.tipo = 'Comprobante' LEFT OUTER JOIN
    Cat_Catalogo1_C ccMp ON CONVERT(VARCHAR, g.metodoPago) = ccMp.codigo AND ccMp.tipo = 'MetodoPago' LEFT OUTER JOIN
    Dat_General g2 ON g.numDocModificado = g2.numeroAutorizacion AND ISNULL(g.numDocModificado, '') <> ''";
                                initialColumnNumber = 11;
                                finalColumnNumber = 16;
                            }
                            break;

                        case "3":
                            if (rfcEmiCore.Equals("YAN810728RW7") || sessionIDENTEMI.Equals("YAN810728RW7"))
                            {
                                sqlQuery = @"SELECT
    cem.RFCEMI AS 'RFC EMISOR', cem.NOMEMI AS 'RAZÓN SOCIAL EMISOR', Cat_Catalogo1_C.descripcion AS COMPROBANTE, g.tipoDeComprobante AS 'TIPO', g.folio AS 'FOLIO', g.serie AS 'SERIE', g2.serie AS 'SERIE MODIFICADA', g2.folio AS 'FOLIO MODIFICADO', g.fecha AS 'FECHA EMISIÓN', cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR', CAST(g.subTotal AS money) AS SUBTOTAL, CAST(g.IVA12 AS money) AS IVA, CAST(g.total AS money) AS TOTAL, CAST(g.importeAPagar AS money) AS 'A PAGAR', g.moneda AS MONEDA, g.tipoCambio AS 'TIPO DE CAMBIO', (g.tipo + g.estado) + (CASE (g.tipo + g.estado) WHEN 'E1' THEN ': Timbrado/Vigente' WHEN 'E4' THEN ': Timbrado/Vigente y Anulado por Nota de Crédito'  WHEN 'N0' THEN ': No Timbrado' WHEN 'C0' THEN ': Cancelado ante SAT' WHEN 'C2' THEN ': En proceso de Cancelación ante SAT' ELSE '' END) AS 'ESTADO', ce.userEmpleado + ': ' + ce.nombreEmpleado AS 'REALIZADO POR', ce2.userEmpleado + ': ' + ce2.nombreEmpleado AS 'CANCELADO POR', (CASE ISNULL(CONVERT(VARCHAR, g.metodoPago), '') WHEN '' THEN '98' ELSE ISNULL(ccMp.descripcion, g.MetodoPago) END) AS 'METODO PAGO', dp.numCtaPago AS 'NUM. CTA.', g.numeroAutorizacion AS UUID, g.numDocModificado AS 'UUID MODIFICADOS', g.version AS VERSION, G.usoCfdi AS 'USO DE CFDI'
FROM
    Dat_General g LEFT OUTER JOIN
    Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
    Cat_Receptor cr ON g.id_Receptor = cr.IDEREC LEFT OUTER JOIN
    Dat_pagos dp ON g.idComprobante = dp.id_Comprobante LEFT OUTER JOIN
    Dat_HabitacionEvento dhe ON g.idComprobante = dhe.id_Comprobante INNER JOIN
    Cat_Empleados ce ON g.id_Empleado = ce.idEmpleado LEFT OUTER JOIN
    Cat_Empleados ce2 ON g.id_Empleado_Canc = ce2.idEmpleado INNER JOIN
    Cat_Catalogo1_C ON Cat_Catalogo1_C.codigo = g.codDoc AND Cat_Catalogo1_C.tipo = 'Comprobante' LEFT OUTER JOIN
    Cat_Catalogo1_C ccMp ON CONVERT(VARCHAR, g.metodoPago) = ccMp.codigo AND ccMp.tipo = 'MetodoPago' LEFT OUTER JOIN
    Dat_General g2 ON g.numDocModificado = g2.numeroAutorizacion AND ISNULL(g.numDocModificado, '') <> ''";
                                initialColumnNumber = 12;
                                finalColumnNumber = 15;
                            }
                            else
                            {
                                sqlQuery = @"SELECT
    Cat_Catalogo1_C.descripcion AS COMPROBANTE, g.tipoDeComprobante AS 'TIPO', g.folio AS 'FOLIO', g.serie AS 'SERIE', g2.serie AS 'SERIE MODIFICADA', g2.folio AS 'FOLIO MODIFICADO', g.fecha AS 'FECHA EMISIÓN', cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR', CAST(g.subTotal AS money) AS SUBTOTAL, CAST(g.IVA12 AS money) AS IVA, CAST(g.total AS money) AS TOTAL, CAST(g.importeAPagar AS money) AS 'A PAGAR', g.moneda AS MONEDA, g.tipoCambio AS 'TIPO DE CAMBIO', (g.tipo + g.estado) + (CASE (g.tipo + g.estado) WHEN 'E1' THEN ': Timbrado/Vigente' WHEN 'E4' THEN ': Timbrado/Vigente y Anulado por Nota de Crédito'  WHEN 'N0' THEN ': No Timbrado' WHEN 'C0' THEN ': Cancelado ante SAT' WHEN 'C2' THEN ': En proceso de Cancelación ante SAT' ELSE '' END) AS 'ESTADO', ce.userEmpleado + ': ' + ce.nombreEmpleado AS 'REALIZADO POR', ce2.userEmpleado + ': ' + ce2.nombreEmpleado AS 'CANCELADO POR', (CASE ISNULL(CONVERT(VARCHAR, g.metodoPago), '') WHEN '' THEN '98' ELSE ISNULL(ccMp.descripcion, g.MetodoPago) END) AS 'METODO PAGO', dp.numCtaPago AS 'NUM. CTA.', g.numeroAutorizacion AS UUID, g.numDocModificado AS 'UUID MODIFICADOS', g.version AS VERSION, G.usoCfdi AS 'USO DE CFDI'
FROM
    Dat_General g LEFT OUTER JOIN
    Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
    Cat_Receptor cr ON g.id_Receptor = cr.IDEREC LEFT OUTER JOIN
    Dat_pagos dp ON g.idComprobante = dp.id_Comprobante LEFT OUTER JOIN
    Dat_HabitacionEvento dhe ON g.idComprobante = dhe.id_Comprobante INNER JOIN
    Cat_Empleados ce ON g.id_Empleado = ce.idEmpleado LEFT OUTER JOIN
    Cat_Empleados ce2 ON g.id_Empleado_Canc = ce2.idEmpleado INNER JOIN
    Cat_Catalogo1_C ON Cat_Catalogo1_C.codigo = g.codDoc AND Cat_Catalogo1_C.tipo = 'Comprobante' LEFT OUTER JOIN
    Cat_Catalogo1_C ccMp ON CONVERT(VARCHAR, g.metodoPago) = ccMp.codigo AND ccMp.tipo = 'MetodoPago' LEFT OUTER JOIN
    Dat_General g2 ON g.numDocModificado = g2.numeroAutorizacion AND ISNULL(g.numDocModificado, '') <> ''";
                                initialColumnNumber = 10;
                                finalColumnNumber = 13;
                            }
                        break;
                    }
                }
            }
            else
            {
                sqlQuery = @"SELECT
    Cat_Catalogo1_C.descripcion AS COMPROBANTE, g.tipoDeComprobante AS 'TIPO', g.folio AS FOLFAC, g.serie AS SERFAC, g2.serie AS 'SERIE MODIFICADA', g2.folio AS 'FOLIO MODIFICADO', g.fecha AS FECHEMI, g.fechaRecepcion AS FECHREC, cr.RFCREC, cr.NOMREC,cem.RFCEMI,cem.NOMEMI, CAST(g.subTotal AS money) AS SUBTOTAL, CAST(g.IVA12 AS money) AS IVA, CAST(g.total AS money) AS TOTAL, CAST(g.importeAPagar AS money) AS 'A PAGAR', g.moneda AS MONEDA, (g.tipo + g.estado) + (CASE (g.tipo + g.estado) WHEN 'E1' THEN ': Timbrado/Vigente' WHEN 'E4' THEN ': Timbrado/Vigente y Anulado por Nota de Crédito'  WHEN 'N0' THEN ': No Timbrado' WHEN 'C0' THEN ': Cancelado ante SAT' WHEN 'C2' THEN ': En proceso de Cancelación ante SAT' ELSE '' END) AS EDO, ce.userEmpleado + ': ' + ce.nombreEmpleado AS REALIZO, ce2.userEmpleado + ': ' + ce2.nombreEmpleado AS CANCELO, (CASE ISNULL(CONVERT(VARCHAR, g.metodoPago), '') WHEN '' THEN '98' ELSE ISNULL(ccMp.descripcion, g.MetodoPago) END) AS 'METODO PAGO', dp.numCtaPago AS 'NUM. CTA.', g.numeroAutorizacion AS UUID, g.numDocModificado AS 'UUID MODIFICADO', CONVERT(NVARCHAR, g.estadoValidacion) + (CASE CONVERT(NVARCHAR, g.estadoValidacion) WHEN '0' THEN ': Rechazado' WHEN '1' THEN ': Aprobado'  WHEN '2' THEN ': Pendiente de Aprobación' ELSE '' END) AS 'ESTADO VALIDACIÓN', g.version AS VERSION
FROM
    Dat_General g LEFT OUTER JOIN
    Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
    Cat_Receptor cr ON g.id_Receptor = cr.IDEREC LEFT OUTER JOIN
    Dat_pagos dp ON g.idComprobante = dp.id_Comprobante LEFT OUTER JOIN
    Dat_HabitacionEvento dhe ON g.idComprobante = dhe.id_Comprobante LEFT OUTER JOIN
    " + _dbNameEmision + @".dbo.Cat_Empleados ce ON g.id_Empleado = ce.idEmpleado LEFT OUTER JOIN
    " + _dbNameEmision + @".dbo.Cat_Empleados ce2 ON g.id_Empleado_Canc = ce2.idEmpleado LEFT OUTER JOIN
    Cat_Catalogo1_C ON Cat_Catalogo1_C.codigo = g.codDoc AND Cat_Catalogo1_C.tipo = 'Comprobante' LEFT OUTER JOIN
    Cat_Catalogo1_C ccMp ON CONVERT(VARCHAR, g.metodoPago) = ccMp.codigo AND ccMp.tipo = 'MetodoPago' LEFT OUTER JOIN
    Dat_General g2 ON g.numDocModificado = g2.numeroAutorizacion AND ISNULL(g.numDocModificado, '') <> ''";
                initialColumnNumber = 13;
                finalColumnNumber = 16;
            }
            try
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Reporte General");
                var con = new SqlConnection(_connectionString);
                con.Open();
                var cmd = new SqlCommand(sqlQuery + (!string.IsNullOrEmpty(sqlWhere) ? " WHERE " + sqlWhere : ""), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 18000; // Timeout de 20 minutos
                var dt = new DataTable();
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                ws.Cell(1, 1).Value = "Reporte General de Comprobantes Electronicos " + _periodo;
                ws.Range(1, 1, 1, dt.Columns.Count).Merge().AddToNamed("Titles1");
                ws.Cell(2, 1).Value = "E1 = Timbrado/Vigente || E4 = Timbrado/Vigente y Anulado por Nota de Crédito || N0 = No Timbrado || C0 = Cancelado ante SAT || C2 = En proceso de Cancelación ante SAT";
                ws.Range(2, 1, 2, dt.Columns.Count).Merge().AddToNamed("Titles2");
                ws.Cell(3, 1).Value = "Fecha de Generación: " + Localization.Now.ToString("s");
                ws.Range(3, 1, 3, dt.Columns.Count).Merge().AddToNamed("Titles3");
                var tableWithData = ws.Cell(4, 1).InsertTable(dt.AsEnumerable());
                con.Close();
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
                wb.SaveAs(_rutaDoc);
                result = true;
            }
            catch (Exception ex)
            {
                Ex = ex;
            }
            return result;
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

        /// <summary>
        /// Genera el reporte de correos con la condición sql especificada.
        /// </summary>
        /// <param name="sqlWhere">Condición SQL.</param>
        /// <returns><c>true</c> si se genera el reporte correctamente, <c>false</c> de lo contrario.</returns>
        public bool Mail(string sqlWhere, string sessionIDENTEMI="")
        {
            Ex = null;
            var result = false;
            var sqlQuery = "";
            var rfcEmiCore = "";

            var db = new BasesDatos("CORE");
            db.Conectar();
            db.CrearComando("SELECT RFCEMI FROM Cat_Emisor WHERE RFCEMI = @RFCEMI");
            db.AsignarParametroCadena("@RFCEMI", sessionIDENTEMI);
            var dr = db.EjecutarConsulta();
            if (dr.Read())
            {
                rfcEmiCore = dr["RFCEMI"].ToString();
            }
            db.Desconectar();

            if (rfcEmiCore.Equals("YAN810728RW7") || sessionIDENTEMI.Equals("YAN810728RW7"))
            {
                sqlQuery = @"SELECT
	cem.RFCEMI AS 'RFC EMISOR', cem.NOMEMI AS 'RAZÓN SOCIAL EMISOR', mail.emailEnviado AS 'CORREO(S)'
	, g.fecha AS 'FECHA EMISIÓN'
	, CONVERT(VARCHAR, g.folio) + ' - ' + CONVERT(VARCHAR, g.serie) AS 'FOLIO-SERIE'
	, cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR'
	, mail.fechaEnvio AS 'FECHA ENVIO'
FROM Cat_emailEnvio mail
	LEFT OUTER JOIN Dat_general g on g.idComprobante = mail.id_general
	LEFT OUTER JOIN Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI
	LEFT OUTER JOIN Cat_RECEPTOR cr ON cr.IDEREC = g.id_Receptor";
            }
            else
            {
                sqlQuery = @"SELECT
	mail.emailEnviado AS 'CORREO(S)'
	, g.fecha AS 'FECHA EMISIÓN'
	, CONVERT(VARCHAR, g.folio) + ' - ' + CONVERT(VARCHAR, g.serie) AS 'FOLIO-SERIE'
	, cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR'
	, mail.fechaEnvio AS 'FECHA ENVIO'
FROM Cat_emailEnvio mail
	LEFT OUTER JOIN Dat_general g on g.idComprobante = mail.id_general
	LEFT OUTER JOIN Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI
	LEFT OUTER JOIN Cat_RECEPTOR cr ON cr.IDEREC = g.id_Receptor";
            }

            try
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Reporte de E-Mails");
                var con = new SqlConnection(_connectionString);
                con.Open();
                var cmd = new SqlCommand(sqlQuery + (!string.IsNullOrEmpty(sqlWhere) ? " WHERE " + sqlWhere : ""), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1800; // Timeout de 20 minutos
                var dt = new DataTable();
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                ws.Cell(1, 1).Value = "Reporte de E-Mails Enviados " + _periodo;
                ws.Range(1, 1, 1, dt.Columns.Count).Merge().AddToNamed("Titles1");
                ws.Cell(2, 1).Value = "Fecha de Generación: " + Localization.Now.ToString("s");
                ws.Range(2, 1, 2, dt.Columns.Count).Merge().AddToNamed("Titles2");
                var tableWithData = ws.Cell(3, 1).InsertTable(dt.AsEnumerable());
                var titlesStyle = wb.Style;
                titlesStyle.Font.Bold = true;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                titlesStyle.Font.FontSize = 20;
                wb.NamedRanges.NamedRange("Titles1").Ranges.Style = titlesStyle;
                titlesStyle.Font.FontSize = 11;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                wb.NamedRanges.NamedRange("Titles2").Ranges.Style = titlesStyle;
                ws.Columns().AdjustToContents();
                ws.Tables.First().ShowAutoFilter = false;
                wb.ShowZeros = true;
                wb.SaveAs(_rutaDoc);
                result = true;
            }
            catch (Exception ex)
            {
                Ex = ex;
            }
            return result;
        }

        private bool isFacturado(string noTicket)
        {
            var status = false;
            var db = new BasesDatos(_connectionString, 0);
            db.Conectar();
            db.CrearComando("SELECT TOP 1 (tipo + CONVERT(VARCHAR(MAX), estado)) AS estado FROM Dat_General WHERE noTicket = @noTicket OR folioReservacion = @noTicket ORDER BY idComprobante DESC");
            db.AsignarParametroCadena("@noTicket", noTicket);
            db.AsignarParametroCadena("@noTicket", noTicket);
            var dr = db.EjecutarConsulta();
            if (dr.Read())
            {
                var estado = dr["estado"].ToString();
                status = estado.Equals("E1");
            }
            else
            {
                status = false;
            }
            db.Desconectar();
            return status;
        }

        private bool isFacturado(string[] ticket)
        {
            var noTicket = ticket[1];
            var status = false;
            var db = new BasesDatos(_connectionString, 0);
            db.Conectar();
            db.CrearComando("SELECT ISNULL(observaciones, '') AS observaciones FROM Log_Trama WHERE noTicket = @noTicket");
            db.AsignarParametroCadena("@noTicket", noTicket);
            var dr = db.EjecutarConsulta();
            if (dr.Read())
            {
                var observaciones = dr["observaciones"].ToString();
                status = !string.IsNullOrEmpty(noTicket);
            }
            else
            {
                status = false;
            }
            db.Desconectar();
            return status;
        }

        private void GetInfoFacturado(string noTicket, out string idComprobante, out string fechaFacturacion, out string uuid, out string folio)
        {
            idComprobante = "";
            fechaFacturacion = "";
            uuid = "";
            folio = "";
            var db = new BasesDatos(_connectionString, 0);
            db.Conectar();
            db.CrearComando("SELECT TOP 1 (tipo + CONVERT(VARCHAR(MAX), estado)) AS estado, idComprobante, fecha AS fechaFacturacion, numeroAutorizacion AS uuid, folio FROM Dat_General WHERE noTicket = @noTicket OR folioReservacion = @noTicket ORDER BY idComprobante DESC");
            db.AsignarParametroCadena("@noTicket", noTicket);
            db.AsignarParametroCadena("@noTicket", noTicket);
            var dr = db.EjecutarConsulta();
            if (dr.Read())
            {
                var estado = dr["estado"].ToString();
                if (estado.Equals("E1"))
                {
                    idComprobante = dr["idComprobante"].ToString();
                    fechaFacturacion = dr["fechaFacturacion"].ToString();
                    uuid = dr["uuid"].ToString();
                    folio = dr["folio"].ToString();
                }
            }
            db.Desconectar();
        }

        /// <summary>
        /// Genera el reporte de tickets con la condición sql especificada.
        /// </summary>
        /// <param name="sqlWhere">Condición SQL.</param>
        /// <returns><c>true</c> si se genera el reporte correctamente, <c>false</c> de lo contrario.</returns>
        public bool Tickets(string sqlWhere, string CfdiVersion, string mes, string anio, string serie, string rfcEmisor, out int count)
        {
            Ex = null;
            var result = false;
            try
            {
                var _counter = 0;
                var todosTickets = new List<string[]>();
                var ticketsFacturados = new List<string[]>();
                var ticketsNoFacturados = new List<string[]>();
                var db = new BasesDatos(_connectionString, 0);
                db.Conectar();
                db.CrearComando(@";WITH partitioned AS (
    SELECT idTrama
		,(CASE ISNULL(noTicket, '') WHEN '' THEN noReserva ELSE noTicket END) AS noTicket
        ,Trama
        ,ROW_NUMBER() OVER(PARTITION BY noTicket, CONVERT(VARCHAR(MAX), Trama)
                            ORDER BY fecha) AS seq
    FROM Log_Trama WHERE " + (!string.IsNullOrEmpty(serie) ? "serie = '" + serie + "' AND " : "") + (!string.IsNullOrEmpty(rfcEmisor) ? " RFCEMI = '" + rfcEmisor + "' AND " : "") + " CONVERT(VARCHAR(MAX), Trama) LIKE 'T|%|" + anio + "-" + mes + @"%'
)
SELECT *
FROM partitioned WHERE seq = 1");
                var dr = db.EjecutarConsulta();
                while (dr.Read())
                {
                    var values = new object[dr.FieldCount];
                    dr.GetValues(values);
                    var sValues = values.Select(v => v.ToString()).ToArray();
                    todosTickets.Add(sValues);
                    _counter++;
                }
                db.Desconectar();
                count = _counter;
                ticketsFacturados = todosTickets.Where(t => isFacturado(t)).ToList();
                ticketsNoFacturados = todosTickets.Where(t => !ticketsFacturados.Any(t1 => t1[1].Equals(t[1]))).ToList();
                DataTable dt = new DataTable("Tickets");
                dt.Columns.Add("Ticket");
                dt.Columns.Add("FechaTicket");
                dt.Columns.Add("Serie");
                dt.Columns.Add("Factura");
                dt.Columns.Add("FechaFacturacion");
                dt.Columns.Add("SubTotal");
                dt.Columns.Add("Descuento");
                dt.Columns.Add("IVA");
                dt.Columns.Add("Total");
                dt.Columns.Add("Propinas");
                dt.Columns.Add("TotalAPagar");
                dt.Columns.Add("Estado");
                foreach (var ticket in todosTickets)
                {
                    var tbTotalFac = "";
                    var tbTotal = "";
                    var tbIva16 = "";
                    var tbPropina = "";
                    var tbSubtotal = "";
                    var tbDescuento = "";
                    var tbFecha = "";
                    var row = dt.NewRow();
                    #region Trama Micros

                    var trama = new TramaMicros(CfdiVersion);
                    trama.Load(ticket[2]);

                    if (CfdiVersion.Equals("3.3"))
                    {
                        #region CFDI 3.3

                        tbTotalFac = trama.Detalles33.Sum(x => Convert.ToDecimal(x.GrossAmount)).ToString();
                        tbFecha = trama.Resumen33.TransactionDate;
                        tbTotal = trama.Resumen33.GrossAmountGroupedP;
                        tbIva16 = "0.00";
                        var subTotal = trama.Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString();
                        var descuento = "0.00";
                        if (trama.Descuentos33 != null)
                        {
                            descuento = trama.Descuentos33.Sum(x => Convert.ToDecimal(x.NetAmount.Replace("-", ""))).ToString();
                        }
                        if (trama.Propinas33 == null)
                        {
                            tbPropina = "0.0";
                        }
                        else
                        {
                            tbPropina = trama.Propinas33.NetAmount;
                        }

                        try
                        {
                            tbIva16 = trama.Impuestos33.Sum(x => Convert.ToDecimal(x.GrossAmount.Replace("-", ""))).ToString();
                        }
                        catch { }

                        decimal dSubTotal = 0;
                        decimal dDescuento = 0;
                        decimal dImpuestos = 0;
                        decimal dPropinas = 0;
                        decimal dTotalFactura = 0;
                        decimal dTotalPagar = 0;
                        decimal.TryParse(subTotal, out dSubTotal);
                        decimal.TryParse(descuento, out dDescuento);
                        decimal.TryParse(tbIva16, out dImpuestos);
                        decimal.TryParse(tbPropina, out dPropinas);
                        tbSubtotal = dSubTotal.ToString();
                        dTotalFactura = dSubTotal - dDescuento + dImpuestos;
                        dTotalPagar = dTotalFactura + dPropinas;
                        tbTotalFac = dTotalFactura.ToString();
                        tbTotal = dTotalPagar.ToString();
                        tbDescuento = descuento;

                        #endregion
                    }
                    else
                    {
                        #region CFDI 3.2

                        tbTotalFac = trama.Detalles.Sum(x => Convert.ToDecimal(x.MontoBrutoagrupadom)).ToString();
                        tbFecha = trama.Resumen.FechaTrans;
                        tbTotal = trama.Resumen.MontoBruto;
                        tbIva16 = "0.00";

                        var subTotal = trama.Detalles.Sum(x => Convert.ToDecimal(x.MontoNetoAgrupado)).ToString();
                        var descuento = "0.00";
                        if (trama.Descuentos != null)
                        {
                            descuento = trama.Descuentos.Sum(x => Convert.ToDecimal(x.MontoNetod.Replace("-", ""))).ToString();
                        }
                        if (trama.Propinas == null)
                        {
                            tbPropina = "0.0";
                        }
                        else
                        {
                            tbPropina = trama.Propinas.MontoNetos;
                        }

                        try
                        {
                            tbIva16 = trama.Impuestos.Sum(x => Convert.ToDecimal(x.MontoBrutoi.Replace("-", ""))).ToString();
                        }
                        catch { }

                        decimal dSubTotal = 0;
                        decimal dDescuento = 0;
                        decimal dImpuestos = 0;
                        decimal dPropinas = 0;
                        decimal dTotalFactura = 0;
                        decimal dTotalPagar = 0;
                        decimal.TryParse(subTotal, out dSubTotal);
                        decimal.TryParse(descuento, out dDescuento);
                        decimal.TryParse(tbIva16, out dImpuestos);
                        decimal.TryParse(tbPropina, out dPropinas);
                        tbSubtotal = dSubTotal.ToString();
                        dTotalFactura = dSubTotal - dDescuento + dImpuestos;
                        dTotalPagar = dTotalFactura + dPropinas;
                        tbTotalFac = dTotalFactura.ToString();
                        tbTotal = dTotalPagar.ToString();
                        tbDescuento = descuento;

                        #endregion
                    }


                    #endregion

                    row["Ticket"] = ticket[1];
                    row["FechaTicket"] = tbFecha;
                    row["Serie"] = serie;
                    row["Total"] = tbTotalFac;
                    row["TotalAPagar"] = tbTotal;
                    row["IVA"] = tbIva16;
                    row["Propinas"] = tbPropina;
                    row["SubTotal"] = tbSubtotal;
                    row["Descuento"] = tbDescuento;
                    var facturado = ticketsFacturados.Any(t => t[1].Equals(ticket[1]));
                    row["Estado"] = facturado ? "FACTURADO" : "NO FACTURADO";
                    if (facturado)
                    {
                        var idComprobante = "";
                        var fechaFacturacion = "";
                        var uuid = "";
                        var folio = "";
                        GetInfoFacturado(ticket[1], out idComprobante, out fechaFacturacion, out uuid, out folio);
                        row["Factura"] = folio;
                        row["FechaFacturacion"] = fechaFacturacion;
                    }
                    dt.Rows.Add(row);
                }
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Reporte de Tickets");
                ws.Cell(1, 1).Value = "Reporte de Tickets " + _periodo;
                ws.Range(1, 1, 1, dt.Columns.Count).Merge().AddToNamed("Titles1");
                ws.Cell(2, 1).Value = "Fecha de Generación: " + Localization.Now.ToString("s");
                ws.Range(2, 1, 2, dt.Columns.Count).Merge().AddToNamed("Titles2");
                var tableWithData = ws.Cell(3, 1).InsertTable(dt.AsEnumerable());
                var titlesStyle = wb.Style;
                titlesStyle.Font.Bold = true;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                titlesStyle.Font.FontSize = 20;
                wb.NamedRanges.NamedRange("Titles1").Ranges.Style = titlesStyle;
                titlesStyle.Font.FontSize = 11;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                wb.NamedRanges.NamedRange("Titles2").Ranges.Style = titlesStyle;
                ws.Columns().AdjustToContents();
                ws.Tables.First().ShowAutoFilter = false;
                wb.ShowZeros = true;
                wb.SaveAs(_rutaDoc);
                result = true;
            }
            catch (Exception ex)
            {
                Ex = ex;
                count = -1;
            }
            return result;
        }

        /// <summary>
        /// Genera el reporte de conceptos con la condición sql especificada.
        /// </summary>
        /// <param name="sqlWhere">Condición SQL.</param>
        /// <returns><c>true</c> si se genera el reporte correctamente, <c>false</c> de lo contrario.</returns>
        public bool Conceptos(string sqlWhere, string sessionIDENTEMI = "")
        {
            Ex = null;
            var result = false;
            var sqlQuery = "";
            var val = "";
            var rfcEmiCore = "";

            var db = new BasesDatos("CORE");
            db.Conectar();
            db.CrearComando("SELECT RFCEMI FROM Cat_Emisor WHERE RFCEMI = @RFCEMI");
            db.AsignarParametroCadena("@RFCEMI", sessionIDENTEMI);
            var dr = db.EjecutarConsulta();
            if (dr.Read())
            {
                rfcEmiCore = dr["RFCEMI"].ToString();
            }
            db.Desconectar();

            if (rfcEmiCore.Equals("YAN810728RW7") || sessionIDENTEMI.Equals("YAN810728RW7"))
            {
                sqlQuery = @"SELECT
	CAST(ISNULL(SUM(dd.cantidad), 0) AS money) AS 'CANTIDAD'
	, ISNULL(ISNULL((SELECT TOP 1 cat1.descripcion 
	FROM Cat_CatConceptos_C conc INNER JOIN Cat_Catalogo1_C cat1 ON cat1.idCatalogo1_C = conc.idCategoria 
	WHERE conc.descripcion = dd.descripcion ORDER BY cat1.idCatalogo1_C DESC), (SELECT TOP 1 onq.clasificacion 
	FROM CatalogoConceptos onq WHERE DESCRIPCION = dd.descripcion ORDER BY onq.IDECONCEPTO DESC)), 'SIN CATEGORIA') AS 'CATEGORÍA'
	, dd.descripcion AS 'DESCRIPCIÓN CONCEPTO'
	, CAST(ISNULL(dd.precioUnitario, 0.00) AS money) AS 'PRECIO UNITARIO'
	, CAST(ISNULL(SUM(dd.precioTotalSinImpuestos), 0.00) AS money) AS 'TOTAL'
	, CAST(ISNULL(SUM(dd.precioTotalSinImpuestos * CONVERT(FLOAT, g.tipoCambio)), 0.00) AS money) AS 'TOTAL PESOS'
	, cem.RFCEMI AS 'RFC EMISOR', cem.NOMEMI AS 'RAZÓN SOCIAL EMISOR', cr.RFCREC AS 'RFC RECEPTOR', cr.NOMREC AS 'RAZÓN SOCIAL RECEPTOR'
FROM
	Dat_Detalles dd  LEFT OUTER JOIN
    Dat_General g ON g.idComprobante = dd.id_Comprobante LEFT OUTER JOIN
	Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
	Cat_Receptor cr ON g.id_Receptor = cr.IDEREC";
                val = "  GROUP BY dd.descripcion, dd.precioUnitario, cem.RFCEMI, cem.NOMEMI, cr.RFCREC, cr.NOMREC";
            }
            else
            {
                sqlQuery = @"SELECT
	CAST(ISNULL(SUM(dd.cantidad), 0) AS money) AS 'CANTIDAD'
	, ISNULL(ISNULL((SELECT TOP 1 cat1.descripcion 
	FROM Cat_CatConceptos_C conc INNER JOIN Cat_Catalogo1_C cat1 ON cat1.idCatalogo1_C = conc.idCategoria 
	WHERE conc.descripcion = dd.descripcion ORDER BY cat1.idCatalogo1_C DESC), (SELECT TOP 1 onq.clasificacion 
	FROM CatalogoConceptos onq WHERE DESCRIPCION = dd.descripcion ORDER BY onq.IDECONCEPTO DESC)), 'SIN CATEGORIA') AS 'CATEGORÍA'
	, dd.descripcion AS 'DESCRIPCIÓN CONCEPTO'
	, CAST(ISNULL(dd.precioUnitario, 0.00) AS money) AS 'PRECIO UNITARIO'
	, CAST(ISNULL(SUM(dd.precioTotalSinImpuestos), 0.00) AS money) AS 'TOTAL'
	, CAST(ISNULL(SUM(dd.precioTotalSinImpuestos * CONVERT(FLOAT, g.tipoCambio)), 0.00) AS money) AS 'TOTAL PESOS'
FROM
	Dat_Detalles dd  LEFT OUTER JOIN
    Dat_General g ON g.idComprobante = dd.id_Comprobante LEFT OUTER JOIN
	Cat_Emisor cem ON g.id_Emisor = cem.IDEEMI LEFT OUTER JOIN
	Cat_Receptor cr ON g.id_Receptor = cr.IDEREC";
                val = "  GROUP BY dd.descripcion, dd.precioUnitario";
            }

                try
                {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Reporte de Conceptos");
                var con = new SqlConnection(_connectionString);
                con.Open();
                var cmd = new SqlCommand(sqlQuery + (!string.IsNullOrEmpty(sqlWhere) ? " WHERE " + sqlWhere : "") + val, con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1800; // Timeout de 20 minutos
                var dt = new DataTable();
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                ws.Cell(1, 1).Value = "Reporte de Conceptos registrados " + _periodo;
                ws.Range(1, 1, 1, dt.Columns.Count).Merge().AddToNamed("Titles1");
                ws.Cell(2, 1).Value = "Fecha de Generación: " + Localization.Now.ToString("s");
                ws.Range(2, 1, 2, dt.Columns.Count).Merge().AddToNamed("Titles2");
                var tableWithData = ws.Cell(3, 1).InsertTable(dt.AsEnumerable());
                var titlesStyle = wb.Style;
                titlesStyle.Font.Bold = true;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                titlesStyle.Font.FontSize = 20;
                wb.NamedRanges.NamedRange("Titles1").Ranges.Style = titlesStyle;
                titlesStyle.Font.FontSize = 11;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                wb.NamedRanges.NamedRange("Titles2").Ranges.Style = titlesStyle;
                try
                {
                    ws.Range(4, 1, dt.Rows.Count + 3, 1).Style.NumberFormat.Format = "#,##0.00";
                    ws.Range(4, 1, dt.Rows.Count + 3, 1).DataType = XLCellValues.Number;
                    ws.Range(4, 3, dt.Rows.Count + 3, 4).Style.NumberFormat.Format = "#,##0.00";
                    ws.Range(4, 3, dt.Rows.Count + 3, 4).DataType = XLCellValues.Number;
                }
                catch { }
                for (int i = 1; i <= 1; i++)
                {
                    var initialRow = 4;
                    var finalRow = (dt.Rows.Count + 3);
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
                for (int i = 3; i <= 4; i++)
                {
                    var initialRow = 4;
                    var finalRow = (dt.Rows.Count + 3);
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
                wb.SaveAs(_rutaDoc);
                result = true;
            }
            catch (Exception ex)
            {
                Ex = ex;
            }
            return result;
        }

        /// <summary>
        /// Genera el reporte de contabilidad (INSSIST) con la condición sql especificada.
        /// </summary>
        /// <param name="sqlWhere">Condición SQL.</param>
        /// <returns><c>true</c> si se genera el reporte correctamente, <c>false</c> de lo contrario.</returns>
        public bool Contabilidad(string sqlWhere, string rfcBase = "", string sessionIDENTEMI = "")
        {
            Ex = null;
            var result = false;

            var rfcEmiCore = "";

            var dbb = new BasesDatos("CORE");
            dbb.Conectar();
            dbb.CrearComando("SELECT RFCEMI FROM Cat_Emisor WHERE RFCEMI = @RFCEMI");
            dbb.AsignarParametroCadena("@RFCEMI", sessionIDENTEMI);
            var drr = dbb.EjecutarConsulta();
            if (drr.Read())
            {
                rfcEmiCore = drr["RFCEMI"].ToString();
            }
            dbb.Desconectar();

                try
                {
                var db = new BasesDatos();
                db.CadenaConexion = _connectionString;
                db.Conectar();
                db.CrearComandoProcedimiento("PA_reporte_INNSIST");
                db.AsignarParametroProcedimiento("where", DbType.String, (!string.IsNullOrEmpty(sqlWhere) ? " AND " + sqlWhere : ""));
                var dr = db.EjecutarConsulta();
                var texto = "";
                var tasaA = "0.16";
                var impuestoB = "0.00";
                var tasaB = "0.00";
                var impuestoC = "0.00";
                var tasaC = "0.00";
                var cliente = "";
                bool valor = dr.Read();
                while (valor)
                {
                    result = true;
                    var codDoc = dr[0].ToString();
                    var fecha = FormatDate(dr[1].ToString());
                    var fechaTimbrado = FormatDate(dr[2].ToString());
                    var metodoPago = dr[3].ToString();
                    var folioReservacion = dr[4].ToString();
                    var subtotal = dr[5].ToString();
                    var impuestoA = dr[6].ToString();
                    var impuestoHospedaje = dr[7].ToString();
                    var tasaHospedaje = dr[8].ToString();
                    var propinas = dr[9].ToString();
                    var total = dr[10].ToString();
                    var nomrec = dr[11].ToString();
                    var folio = dr[12].ToString();
                    var numeroAutorizacion = dr[13].ToString();
                    var rfcrec = dr[14].ToString();
                    var serie = dr[15].ToString();
                    var observaciones = dr[16].ToString();
                    var rfcEmisor = dr[17].ToString();


                    switch (codDoc)
                    {
                        case "00": codDoc = "CAFA"; break;
                        case "01": codDoc = "FACT"; break;
                        case "04": codDoc = "NOTA"; break;
                    }

                    metodoPago = "P";

                    if (Regex.IsMatch(observaciones, @"DIRECT.BILL", RegexOptions.IgnoreCase))
                    {
                        metodoPago = "C";
                    }

                    if (rfcEmisor.Trim().Equals("ODO120810J51", StringComparison.OrdinalIgnoreCase))
                    {
                        propinas = "0.00";
                    }

                    if (rfcEmiCore.Equals("YAN810728RW7") || sessionIDENTEMI.Equals("YAN810728RW7"))
                    {
                        texto += (codDoc + ComplementarCadena(cliente, 15, 0) + ComplementarCadena(folioReservacion, 9, 0)
                        + fecha + fechaTimbrado + metodoPago + ComplementarCadena(folio, 9, 0) + ComplementarCadena(subtotal, 15, 1) + ComplementarCadena(impuestoA, 15, 1) + ComplementarCadena(tasaA, 15, 1)
                        + ComplementarCadena(impuestoB, 15, 1) + ComplementarCadena(tasaB, 15, 1) + ComplementarCadena(impuestoC, 15, 1)
                        + ComplementarCadena(tasaC, 15, 1) + ComplementarCadena(impuestoHospedaje, 15, 1) + ComplementarCadena(tasaHospedaje, 15, 1)
                        + ComplementarCadena(propinas, 15, 1) + ComplementarCadena(total, 15, 1) + ComplementarCadena(nomrec, 150, 0) +
                        ComplementarCadena(serie + folio, 15, 0)
                        + ComplementarCadena(numeroAutorizacion, 36, 0) + ComplementarCadena(rfcrec, 14, 0) + " " + ComplementarCadena(rfcEmisor, 14, 0)).Truncate(454);
                    }
                    else
                    {
                        texto += (codDoc + ComplementarCadena(cliente, 15, 0) + ComplementarCadena(folioReservacion, 9, 0)
                        + fecha + fechaTimbrado + metodoPago + ComplementarCadena(folio, 9, 0) + ComplementarCadena(subtotal, 15, 1) + ComplementarCadena(impuestoA, 15, 1) + ComplementarCadena(tasaA, 15, 1)
                        + ComplementarCadena(impuestoB, 15, 1) + ComplementarCadena(tasaB, 15, 1) + ComplementarCadena(impuestoC, 15, 1)
                        + ComplementarCadena(tasaC, 15, 1) + ComplementarCadena(impuestoHospedaje, 15, 1) + ComplementarCadena(tasaHospedaje, 15, 1)
                        + ComplementarCadena(propinas, 15, 1) + ComplementarCadena(total, 15, 1) + ComplementarCadena(nomrec, 150, 0) +
                        ComplementarCadena(serie + folio, 15, 0)
                        + ComplementarCadena(numeroAutorizacion, 36, 0) + ComplementarCadena(rfcrec, 14, 0)).Truncate(454);
                    }
                    
                    valor = dr.Read();
                    if (valor)
                    {
                        texto += Environment.NewLine;
                    }

                }
                db.Desconectar();
                File.WriteAllText(_rutaDoc, StripNonAscii(texto), System.Text.Encoding.ASCII);
                return File.Exists(_rutaDoc);
            }
            catch (Exception ex)
            {
                Ex = ex;
                result = false;
            }
            return result;
        }

        private string StripNonAscii(string inputString)
        {
            string response = inputString.Trim(new char[] { '\uFEFF' });
            return response;
        }

        /// <summary>
        /// Contabilidads the c.
        /// </summary>
        /// <param name="sqlWhere">The SQL where.</param>
        /// <param name="sesion">The sesion.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ContabilidadC(string sqlWhere, string sesion)
        {
            Ex = null;
            var result = false;
            try
            {

                var db = new BasesDatos();
                db.CadenaConexion = _connectionString;
                db.Conectar();
                db.CrearComandoProcedimiento("PA_reporte_INNSIST");
                db.AsignarParametroProcedimiento("where", DbType.String, (!string.IsNullOrEmpty(sqlWhere) ? " AND " + sqlWhere : ""));
                var dr = db.EjecutarConsulta();
                var texto = "";
                var tasaA = "0.16";
                var impuestoB = "0.00";
                var tasaB = "0.00";
                var impuestoC = "0.00";
                var tasaC = "0.00";
                var cliente = "";
                bool valor = dr.Read();
                var fecha = "";
                while (valor)
                {
                    result = true;
                    var codDoc = dr[0].ToString();
                    fecha = FormatDate(dr[1].ToString());
                    var fechaTimbrado = FormatDate(dr[2].ToString());
                    var metodoPago = dr[3].ToString();
                    var folioReservacion = dr[4].ToString();
                    var subtotal = dr[5].ToString();
                    var impuestoA = dr[6].ToString();
                    var impuestoHospedaje = dr[7].ToString();
                    var tasaHospedaje = dr[8].ToString();
                    var propinas = dr[9].ToString();
                    var total = dr[10].ToString();
                    var nomrec = dr[11].ToString();
                    var folio = dr[12].ToString();
                    var numeroAutorizacion = dr[13].ToString();
                    var rfcrec = dr[14].ToString();
                    var serie = dr[15].ToString();
                    var observaciones = dr[16].ToString();
                    var rfcEmisor = dr[17].ToString();

                    var claveCliente = "123456789123456";
                    //   var FolioF = dr[18].ToString();

                    switch (codDoc)
                    {
                        case "00": codDoc = "CAFA"; break;
                        case "01": codDoc = "FACT"; break;
                        case "04": codDoc = "NOTA"; break;
                    }

                    metodoPago = "P";

                    if (Regex.IsMatch(observaciones, @"DIRECT.BILL", RegexOptions.IgnoreCase))
                    {
                        metodoPago = "C";
                    }

                    if (rfcEmisor.Trim().Equals("ODO120810J51", StringComparison.OrdinalIgnoreCase))
                    {
                        propinas = "0.00";
                    }

                    texto += (codDoc + ComplementarCadena(claveCliente/*cliente*/, 15, 0) + ComplementarCadena(folioReservacion, 9, 0)
                    + fecha + fechaTimbrado + metodoPago + ComplementarCadena(folio, 9, 0) + ComplementarCadena(subtotal, 15, 1) + ComplementarCadena(impuestoA, 15, 1) + ComplementarCadena(tasaA, 15, 1)
                    + ComplementarCadena(impuestoB, 15, 1) + ComplementarCadena(tasaB, 15, 1) + ComplementarCadena(impuestoC, 15, 1)
                    + ComplementarCadena(tasaC, 15, 1) + ComplementarCadena(impuestoHospedaje, 15, 1) + ComplementarCadena(tasaHospedaje, 15, 1)
                    + ComplementarCadena(propinas, 15, 1) + ComplementarCadena(total, 15, 1) + ComplementarCadena(nomrec, 150, 0) +
                    ComplementarCadena(serie + folio, 15, 0)
                    + ComplementarCadena(numeroAutorizacion, 36, 0) + ComplementarCadena(rfcrec, 14, 0) + " " + ComplementarCadena(rfcEmisor, 14, 0)).Truncate(434 - 1) ;

                    valor = dr.Read();
                    if (valor)
                    {
                        texto += "\r\n";
                    }
                }
                db.Desconectar();
                //                File.WriteAllText(_rutaDoc, texto);
                //                return File.Exists(_rutaDoc);

                if (!Directory.Exists(_rutaDoc))
                {
                    Directory.CreateDirectory(_rutaDoc);
                }
                String file = _rutaDoc + "U" + sesion + fecha + ".txt";
                using (StreamWriter w = File.AppendText(file))
                {
                    w.WriteLine(texto);
                    w.Flush();
                    w.Close();
                }

            }
            catch (Exception ex)
            {
                Ex = ex;
                result = false;
            }
            return result;
        }


        //public static void anade_linea_archivo(string archivo, string linea)
        //{
        //    if (!Directory.Exists(_rutaDoc))
        //    {
        //        Directory.CreateDirectory(_rutaDoc);
        //    }
        //    using (StreamWriter w = File.AppendText(archivo))
        //    {
        //        w.WriteLine(linea.Replace(Environment.NewLine, ""));
        //        w.Flush();
        //        w.Close();
        //    }
        //}
        /// <summary>
        /// Adjunta ceros o espacios a la derecha de la cadena para completar un tamaño determinado.
        /// </summary>
        /// <param name="cadena">La cadena a completar.</param>
        /// <param name="numero">El tamaño de destino.</param>
        /// <param name="validar">1 para completar cadenas numéricas, cualquier otro valor para cadenas de texto.</param>
        /// <returns>System.String.</returns>
        private string ComplementarCadena(string cadena, int numero, int validar)
        {
            numero = numero - cadena.Length;
            for (int i = 0; i < numero; i++)
            {
                if (validar == 1)
                {
                    cadena = "0" + cadena;
                }
                else
                {
                    cadena += " ";
                }
            }
            return cadena;
        }

        /// <summary>
        /// Convierte una fecha a formato yyyyMMdd.
        /// </summary>
        /// <param name="dRvalue">Fecha a formatear.</param>
        /// <returns>System.String.</returns>
        private string FormatDate(string dRvalue)
        {
            string resultado = "";
            try
            {
                resultado = Convert.ToDateTime(dRvalue).ToString("yyyyMMdd");

            }
            catch { }
            if (string.IsNullOrEmpty(resultado))
            {
                resultado = "        ";
            }
            return resultado;
        }
    }

    /// <summary>
    /// Class StringExt.
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// Corta la cadena especificada para concordar con un tamaño específico.
        /// </summary>
        /// <param name="value">La cadena a cortar.</param>
        /// <param name="maxLength">El tamaño máximo que tendrá la cadena.</param>
        /// <returns>La cadena cortada.</returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}