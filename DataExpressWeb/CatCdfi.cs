// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernandez
// Created          : 08-08-2017
//
// Last Modified By : Sergio Hernandez
// Last Modified On : 08-09-2017
// ***********************************************************************
// <copyright file="CatCdfi.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataExpressWeb
{
    /// <summary>
    /// Class CatCdfi.
    /// </summary>
    public class CatCdfi
    {
        /// <summary>
        /// The c aduana
        /// </summary>
        public List<KeyValuePair<string, string>> CAduana;
        /// <summary>
        /// The c claveprodserv
        /// </summary>
        public List<KeyValuePair<string, string>> CClaveprodserv;
        /// <summary>
        /// The c claveunidad
        /// </summary>
        public List<KeyValuePair<string, string>> CClaveunidad;
        /// <summary>
        /// The c codigopostal
        /// </summary>
        public List<KeyValuePair<string, string>> CCodigopostal;
        /// <summary>
        /// The c formapago
        /// </summary>
        public List<KeyValuePair<string, string>> CFormapago;
        /// <summary>
        /// The c impuesto
        /// </summary>
        public List<KeyValuePair<string, string>> CImpuesto;
        /// <summary>
        /// The c metodopago
        /// </summary>
        public List<KeyValuePair<string, string>> CMetodopago;
        /// <summary>
        /// The c moneda
        /// </summary>
        public List<KeyValuePair<string, string>> CMoneda;
        /// <summary>
        /// The c numpedimentoaduana
        /// </summary>
        public List<KeyValuePair<string, string>> CNumpedimentoaduana;
        /// <summary>
        /// The c pais
        /// </summary>
        public List<KeyValuePair<string, string>> CPais;
        /// <summary>
        /// The c patenteaduanal
        /// </summary>
        public List<KeyValuePair<string, string>> CPatenteaduanal;
        /// <summary>
        /// The c regimenfiscal
        /// </summary>
        public List<KeyValuePair<string, string>> CRegimenfiscal;
        /// <summary>
        /// The c tasaocuota
        /// </summary>
        public List<KeyValuePair<string, string>> CTasaocuota;
        /// <summary>
        /// The c tipodecomprobante
        /// </summary>
        public List<KeyValuePair<string, string>> CTipodecomprobante;
        /// <summary>
        /// The c tipofactor
        /// </summary>
        public List<KeyValuePair<string, string>> CTipofactor;
        /// <summary>
        /// The c tiporelacion
        /// </summary>
        public List<KeyValuePair<string, string>> CTiporelacion;
        /// <summary>
        /// The c usocfdi
        /// </summary>
        public List<KeyValuePair<string, string>> CUsocfdi;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatCdfi"/> class.
        /// </summary>
        public CatCdfi()
        {
            CAduana = new List<KeyValuePair<string, string>>();
            CClaveprodserv = new List<KeyValuePair<string, string>>();
            CClaveunidad = new List<KeyValuePair<string, string>>();
            CCodigopostal = new List<KeyValuePair<string, string>>();
            CFormapago = new List<KeyValuePair<string, string>>();
            CImpuesto = new List<KeyValuePair<string, string>>();
            CMetodopago = new List<KeyValuePair<string, string>>();
            CNumpedimentoaduana = new List<KeyValuePair<string, string>>();
            CPais = new List<KeyValuePair<string, string>>();
            CPatenteaduanal = new List<KeyValuePair<string, string>>();
            CRegimenfiscal = new List<KeyValuePair<string, string>>();
            CTasaocuota = new List<KeyValuePair<string, string>>();
            CTipodecomprobante = new List<KeyValuePair<string, string>>();
            CTipofactor = new List<KeyValuePair<string, string>>();
            CAduana = new List<KeyValuePair<string, string>>();
            CTiporelacion = new List<KeyValuePair<string, string>>();
            CUsocfdi = new List<KeyValuePair<string, string>>();
            CMoneda = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void LoadFromFile(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            LoadFromBytes(bytes);
        }

        /// <summary>
        /// Loads from bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public void LoadFromBytes(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            LoadFromStream(stream);
        }

        /// <summary>
        /// Loads from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void LoadFromStream(Stream stream)
        {
            using (var package = new ExcelPackage(stream))
            {
                foreach (var workSheet in package.Workbook.Worksheets)
                {
                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;
                    int skipRows;
                    switch (workSheet.Name)
                    {
                        case "c_Aduana":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CAduana.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_ClaveProdServ":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CClaveprodserv.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_ClaveUnidad":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CClaveunidad.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_CodigoPostal_Parte_1":
                        case "c_CodigoPostal_Parte_2":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text + "-" + workSheet.Cells[row, 3].Text + "-" + workSheet.Cells[row, 4].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CCodigopostal.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_FormaPago":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CFormapago.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_Impuesto":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CImpuesto.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_MetodoPago":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CMetodopago.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_Moneda":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CMoneda.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_NumPedimentoAduana":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text + "-" + workSheet.Cells[row, 3].Text + "-" + workSheet.Cells[row, 4].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CNumpedimentoaduana.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_Pais":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CPais.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_PatenteAduanal":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = "";
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CPatenteaduanal.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_RegimenFiscal":
                            skipRows = 6;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CRegimenfiscal.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_TasaOCuota":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 4].Text;
                                    var value = workSheet.Cells[row, 5].Text + "-" + (string.IsNullOrEmpty(workSheet.Cells[row, 2].Text.Trim()) ? "0" : workSheet.Cells[row, 2].Text) + "-" + workSheet.Cells[row, 3].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CTasaocuota.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_TipoDeComprobante":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CTipodecomprobante.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_TipoFactor":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = "";
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CTipofactor.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_TipoRelacion":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = "";
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CTiporelacion.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                        case "c_UsoCFDI":
                            skipRows = 5;
                            for (var row = (start.Row + skipRows); row <= end.Row; row++)
                            {
                                try
                                {
                                    var key = workSheet.Cells[row, 1].Text;
                                    var value = workSheet.Cells[row, 2].Text;
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        CUsocfdi.Add(new KeyValuePair<string, string>(key, value));
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}