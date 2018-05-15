// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="autoComprobantes.asmx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using Datos;

namespace DataExpressWeb.nuevo
{
    /// <summary>
    /// Descripción breve de autoComprobantes
    /// </summary>
    /// <seealso cref="System.Web.Services.WebService" />
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [ScriptService]
    public class AutoComprobantes : WebService
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;

        /// <summary>
        /// UUIDs the specified UUID prefix.
        /// </summary>
        /// <param name="uuidPrefix">The UUID prefix.</param>
        /// <param name="tipo">The tipo.</param>
        /// <returns>System.String[].</returns>
        [WebMethod]
        [ScriptMethod]
        public string[] Uuid(string uuidPrefix = null, string tipo = null)
        {
            var asRfCemi = new List<string>();
            var uuids = new List<string>();
            try
            {
                _db = new BasesDatos("CORE");
                _db.Conectar();
                _db.CrearComando("select distinct rfcemi from Cat_Emisor");
                var dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    asRfCemi.Add(dr[0].ToString());
                }
                _db.Desconectar();
                foreach (var rfcEmi in asRfCemi)
                {
                    _db = new BasesDatos(rfcEmi);
                    _db.Conectar();
                    var myWhere = "";
                    if (!string.IsNullOrEmpty(uuidPrefix))
                    {
                        myWhere += (!string.IsNullOrEmpty(myWhere) ? " AND " : "") + "numeroAutorizacion LIKE '" + uuidPrefix + "%'";
                    }
                    if (!string.IsNullOrEmpty(tipo))
                    {
                        myWhere += (!string.IsNullOrEmpty(myWhere) ? " AND " : "") + "tipoDeComprobante = '" + tipo + "'";
                    }
                    myWhere = (!string.IsNullOrEmpty(myWhere) ? " WHERE " : "") + myWhere;
                    _db.CrearComando("SELECT TOP 10 numeroAutorizacion FROM Dat_General" + myWhere);
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        uuids.Add(dr[0].ToString());
                    }
                    _db.Desconectar();
                }
            }
            catch
            {
            }
            return uuids.ToArray();
        }
    }
}