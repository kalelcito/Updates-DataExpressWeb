// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 10-17-2016
// ***********************************************************************
// <copyright file="autoRuc.asmx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using Datos;

namespace DataExpressWeb.nuevo
{
    /// <summary>
    /// Descripción breve de autoRuc
    /// </summary>
    /// <seealso cref="System.Web.Services.WebService" />
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class AutoRuc : WebService
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;

        /// <summary>
        /// Gets the ruc.
        /// </summary>
        /// <param name="prefixText">The prefix text.</param>
        /// <returns>System.String[].</returns>
        [WebMethod]
        [ScriptMethod]
        public string[] GetRuc(string prefixText)
        {
            string[] asRfCemi;
            var count = 0;
            var a = new string[1];
            ArrayList arrayRfCemi;
            arrayRfCemi = new ArrayList();
            _db = new BasesDatos("CORE");
            var sql1 = "SELECT TOP 10 RFCREC FROM Cat_Receptor where RFCREC LIKE @rfc";
            var contador = 0;
            try
            {
                _db.Conectar();
                _db.CrearComando("select distinct rfcemi from Cat_Emisor");
                var drrfc = _db.EjecutarConsulta();
                while (drrfc.Read())
                {
                    asRfCemi = new string[3];
                    asRfCemi[0] = drrfc[0].ToString();
                    arrayRfCemi.Add(asRfCemi);
                }
                _db.Desconectar();

                foreach (string[] rfcEmi in arrayRfCemi)
                {
                    _db = new BasesDatos(rfcEmi[0]);
                    _db.Conectar();
                    _db.CrearComando("SELECT TOP 10 COUNT(RFCREC) FROM Cat_Receptor where RFCREC LIKE @rfc ");
                    _db.AsignarParametroCadena("@rfc", prefixText + "%");
                    ;
                    var drTot = _db.EjecutarConsulta();
                    drTot.Read();
                    count = count + Convert.ToInt32(drTot[0].ToString());
                    _db.Desconectar();
                }
                var items = new string[count];
                foreach (string[] rfcEmi in arrayRfCemi)
                {
                    _db = new BasesDatos(rfcEmi[0]);
                    _db.Conectar();
                    _db.CrearComando(sql1);
                    _db.AsignarParametroCadena("@rfc", prefixText + "%");
                    var drSum = _db.EjecutarConsulta();

                    while (drSum.Read())
                    {
                        var grab = true;
                        foreach (var value in items)
                        {
                            try
                            {
                                if (value != null)
                                {
                                    if (value.Equals(drSum[0].ToString()))
                                    {
                                        grab = false;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                grab = false;
                            }
                        }
                        if (grab)
                        {
                            if (!string.IsNullOrEmpty(drSum[0].ToString().Trim()))
                            {
                                items[contador] = drSum[0].ToString();
                                contador++;
                            }
                        }
                    }
                    _db.Desconectar();

                    while (true)
                    {
                        var counttt = 0;
                        foreach (var value in items)
                        {
                            if (value == null)
                            {
                                items = items.Where(w => w != items[counttt]).ToArray();
                                break;
                            }
                            counttt++;
                        }
                        if (items.Count() == counttt)
                        {
                            break;
                        }
                    }
                }
                if (count == 0)
                {
                    a[0] = "No existen registros";
                    return a;
                }

                return items;
            }
            catch (Exception)
            {
                a[0] = "No se pueden visualizar registros";
                return a;
            }
        }

        /// <summary>
        /// Gets the ruc emi.
        /// </summary>
        /// <param name="prefixText">The prefix text.</param>
        /// <returns>System.String[].</returns>
        [WebMethod]
        [ScriptMethod]
        public string[] GetRucEmi(string prefixText)
        {
            string[] asRfCemi;
            var count = 0;
            var a = new string[1];
            ArrayList arrayRfCemi;
            arrayRfCemi = new ArrayList();
            _db = new BasesDatos("CORE");
            var sql1 = "SELECT TOP 10 RFCEMI FROM Cat_Emisor where RFCEMI LIKE @rfc";
            var contador = 0;
            try
            {
                _db.Conectar();
                _db.CrearComando("select distinct rfcemi from Cat_Emisor");
                var drrfc = _db.EjecutarConsulta();
                while (drrfc.Read())
                {
                    asRfCemi = new string[3];
                    asRfCemi[0] = drrfc[0].ToString();
                    arrayRfCemi.Add(asRfCemi);
                }
                _db.Desconectar();

                foreach (string[] rfcEmi in arrayRfCemi)
                {
                    _db = new BasesDatos(rfcEmi[0]);
                    _db.Conectar();
                    _db.CrearComando("SELECT TOP 10 COUNT(RFCEMI) FROM Cat_Emisor where RFCEMI LIKE @rfc ");
                    _db.AsignarParametroCadena("@rfc", prefixText + "%");
                    var drTot = _db.EjecutarConsulta();
                    drTot.Read();
                    count = count + Convert.ToInt32(drTot[0].ToString());
                    _db.Desconectar();
                }
                var items = new string[count];
                foreach (string[] rfcEmi in arrayRfCemi)
                {
                    _db = new BasesDatos(rfcEmi[0]);
                    _db.Conectar();
                    _db.CrearComando(sql1);
                    _db.AsignarParametroCadena("@rfc", prefixText + "%");
                    var drSum = _db.EjecutarConsulta();

                    while (drSum.Read())
                    {
                        var grab = true;
                        foreach (var value in items)
                        {
                            try
                            {
                                if (value != null)
                                {
                                    if (value.Equals(drSum[0].ToString()))
                                    {
                                        grab = false;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                grab = false;
                            }
                        }
                        if (grab)
                        {
                            if (!string.IsNullOrEmpty(drSum[0].ToString().Trim()))
                            {
                                items[contador] = drSum[0].ToString();
                                contador++;
                            }
                        }
                    }
                    _db.Desconectar();

                    while (true)
                    {
                        var counttt = 0;
                        foreach (var value in items)
                        {
                            if (value == null)
                            {
                                items = items.Where(w => w != items[counttt]).ToArray();
                                break;
                            }
                            counttt++;
                        }
                        if (items.Count() == counttt)
                        {
                            break;
                        }
                    }
                }
                if (count == 0)
                {
                    a[0] = "No existen registros";
                    return a;
                }

                return items;
            }
            catch (Exception)
            {
                a[0] = "No se pueden visualizar registros";
                return a;
            }
        }

        /// <summary>
        /// Gets the ruc recepcion.
        /// </summary>
        /// <param name="prefixText">The prefix text.</param>
        /// <returns>System.String[].</returns>
        [WebMethod]
        [ScriptMethod]
        public string[] GetRucRecepcion(string prefixText)
        {
            string[] asRfCemi;
            var count = 0;
            var a = new string[1];
            ArrayList arrayRfCemi;
            arrayRfCemi = new ArrayList();
            _db = new BasesDatos("CORE");
            var sql1 = "SELECT TOP 10 RFCREC FROM Cat_Receptor where RFCREC LIKE @rfc";
            var contador = 0;
            try
            {
                _db.Conectar();
                _db.CrearComando("select distinct rfcemi from Cat_Emisor");
                var drrfc = _db.EjecutarConsulta();
                while (drrfc.Read())
                {
                    asRfCemi = new string[3];
                    asRfCemi[0] = drrfc[0].ToString();
                    arrayRfCemi.Add(asRfCemi);
                }
                _db.Desconectar();

                foreach (string[] rfcEmi in arrayRfCemi)
                {
                    _db = new BasesDatos(rfcEmi[0], "Recepcion");
                    _db.Conectar();
                    _db.CrearComando("SELECT TOP 10 COUNT(RFCREC) FROM Cat_Receptor where RFCREC LIKE @rfc ");
                    _db.AsignarParametroCadena("@rfc", prefixText + "%");
                    ;
                    var drTot = _db.EjecutarConsulta();
                    drTot.Read();
                    count = count + Convert.ToInt32(drTot[0].ToString());
                    _db.Desconectar();
                }
                var items = new string[count];
                foreach (string[] rfcEmi in arrayRfCemi)
                {
                    _db = new BasesDatos(rfcEmi[0], "Recepcion");
                    _db.Conectar();
                    _db.CrearComando(sql1);
                    _db.AsignarParametroCadena("@rfc", prefixText + "%");
                    var drSum = _db.EjecutarConsulta();

                    while (drSum.Read())
                    {
                        var grab = true;
                        foreach (var value in items)
                        {
                            try
                            {
                                if (value != null)
                                {
                                    if (value.Equals(drSum[0].ToString()))
                                    {
                                        grab = false;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                grab = false;
                            }
                        }
                        if (grab)
                        {
                            if (!string.IsNullOrEmpty(drSum[0].ToString().Trim()))
                            {
                                items[contador] = drSum[0].ToString();
                                contador++;
                            }
                        }
                    }
                    _db.Desconectar();

                    while (true)
                    {
                        var counttt = 0;
                        foreach (var value in items)
                        {
                            if (value == null)
                            {
                                items = items.Where(w => w != items[counttt]).ToArray();
                                break;
                            }
                            counttt++;
                        }
                        if (items.Count() == counttt)
                        {
                            break;
                        }
                    }
                }
                if (count == 0)
                {
                    a[0] = "No existen registros";
                    return a;
                }

                return items;
            }
            catch (Exception)
            {
                a[0] = "No se pueden visualizar registros";
                return a;
            }
        }

        /// <summary>
        /// Gets the ruc emi recepcion.
        /// </summary>
        /// <param name="prefixText">The prefix text.</param>
        /// <returns>System.String[].</returns>
        [WebMethod]
        [ScriptMethod]
        public string[] GetRucEmiRecepcion(string prefixText)
        {
            string[] asRfCemi;
            var count = 0;
            var a = new string[1];
            ArrayList arrayRfCemi;
            arrayRfCemi = new ArrayList();
            _db = new BasesDatos("CORE");
            var sql1 = "SELECT TOP 10 RFCEMI FROM Cat_Emisor where RFCEMI LIKE @rfc";
            var contador = 0;
            try
            {
                _db.Conectar();
                _db.CrearComando("select distinct rfcemi from Cat_Emisor");
                var drrfc = _db.EjecutarConsulta();
                while (drrfc.Read())
                {
                    asRfCemi = new string[3];
                    asRfCemi[0] = drrfc[0].ToString();
                    arrayRfCemi.Add(asRfCemi);
                }
                _db.Desconectar();

                foreach (string[] rfcEmi in arrayRfCemi)
                {
                    _db = new BasesDatos(rfcEmi[0], "Recepcion");
                    _db.Conectar();
                    _db.CrearComando("SELECT TOP 10 COUNT(RFCEMI) FROM Cat_Emisor where RFCEMI LIKE @rfc ");
                    _db.AsignarParametroCadena("@rfc", prefixText + "%");
                    var drTot = _db.EjecutarConsulta();
                    drTot.Read();
                    count = count + Convert.ToInt32(drTot[0].ToString());
                    _db.Desconectar();
                }
                var items = new string[count];
                foreach (string[] rfcEmi in arrayRfCemi)
                {
                    _db = new BasesDatos(rfcEmi[0], "Recepcion");
                    _db.Conectar();
                    _db.CrearComando(sql1);
                    _db.AsignarParametroCadena("@rfc", prefixText + "%");
                    var drSum = _db.EjecutarConsulta();

                    while (drSum.Read())
                    {
                        var grab = true;
                        foreach (var value in items)
                        {
                            try
                            {
                                if (value != null)
                                {
                                    if (value.Equals(drSum[0].ToString()))
                                    {
                                        grab = false;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                grab = false;
                            }
                        }
                        if (grab)
                        {
                            if (!string.IsNullOrEmpty(drSum[0].ToString().Trim()))
                            {
                                items[contador] = drSum[0].ToString();
                                contador++;
                            }
                        }
                    }
                    _db.Desconectar();

                    while (true)
                    {
                        var counttt = 0;
                        foreach (var value in items)
                        {
                            if (value == null)
                            {
                                items = items.Where(w => w != items[counttt]).ToArray();
                                break;
                            }
                            counttt++;
                        }
                        if (items.Count() == counttt)
                        {
                            break;
                        }
                    }
                }
                if (count == 0)
                {
                    a[0] = "No existen registros";
                    return a;
                }

                return items;
            }
            catch (Exception)
            {
                a[0] = "No se pueden visualizar registros";
                return a;
            }
        }

    }
}