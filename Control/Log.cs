// ***********************************************************************
// Assembly         : Control
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="Log.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Data.Common;
using Datos;
using System.Linq;
using System.IO;

namespace Control
{
    /// <summary>
    /// Class Log.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private readonly BasesDatos _db;
        /// <summary>
        /// The emisor
        /// </summary>
        private readonly string _emisor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log" /> class.
        /// </summary>
        /// <param name="emisor">The emisor.</param>
        /// <param name="dbName">Name of the database.</param>
        public Log(string emisor, string dbName = "Emision")
        {
            _emisor = emisor;
            _db = new BasesDatos(emisor, dbName);
        }


        /// <summary>
        /// Registra un evento en la base de datos.
        /// </summary>
        /// <param name="mensaje">El evento a registrar.</param>
        /// <param name="clase">La clase en la cual se genera el evento.</param>
        /// <param name="metodo">El metodo en el cual se genera el evento.</param>
        /// <param name="origen3">Tercer origen para identificar mejor el evento (opcional).</param>
        /// <param name="mensajeTecnico">Detalles tectnicos del evento (opcional).</param>
        /// <param name="idTrama">El ID de la trama asociada al evento (opcional).</param>
        /// <param name="idComprobante">El ID del comprobante asociado al evento (opcional).</param>
        /// <param name="idEmpleado">El ID del empleado asociado al evento (opcional).</param>
        /// <param name="rfcEmisor">El RFC del emisor asociado al evento (opcional).</param>
        /// <param name="registrarTxt">if set to <c>true</c> [registrar text].</param>
        /// <returns><c>true</c> si el evento se registra correctamente en la base de datos, <c>false</c> de lo contrario.</returns>
        public bool Registrar(string mensaje, string clase, string metodo, string origen3 = "", string mensajeTecnico = "", string idTrama = "", string idComprobante = "", string idEmpleado = "", string rfcEmisor = "", bool registrarTxt = false)
        {
            if (registrarTxt) { RegistrarTxt(mensaje, clase, metodo, mensajeTecnico); }
            DbDataReader dr;
            var insertado = false;
            _db.Conectar();
            try
            {
                _db.CrearComando(@"INSERT INTO Logs (
                                        codigo
                                        ,origen1
                                        ,origen2
                                        ,origen3
                                        ,descripcion
                                        ,detallesTecnicos
                                        " + (!string.IsNullOrEmpty(idTrama) ? ",tramaAsociada" : "") + @"
                                        " + (!string.IsNullOrEmpty(idComprobante) ? ",comprobanteAsociado" : "") + @"
                                        " + (!string.IsNullOrEmpty(idEmpleado) ? ",idEmpleado" : "") + @"
                                        ,RFCEMI) 
                                      OUTPUT Inserted.idLog
                                      VALUES (
                                        @codigo
                                        ,@origen1
                                        ,@origen2
                                        ,@origen3
                                        ,@descripcion
                                        ,@detallesTecnicos
                                        " + (!string.IsNullOrEmpty(idTrama) ? ",@tramaAsociada" : "") + @"
                                        " + (!string.IsNullOrEmpty(idComprobante) ? ",@comprobanteAsociado" : "") + @"
                                        " + (!string.IsNullOrEmpty(idEmpleado) ? ",@idEmpleado" : "") + @"
                                        ,@RFCEMI)");
                _db.AsignarParametroEntero("@codigo", 100);
                _db.AsignarParametroCadena("@origen1", clase);
                _db.AsignarParametroCadena("@origen2", metodo);
                _db.AsignarParametroCadena("@origen3", origen3);
                _db.AsignarParametroCadena("@descripcion", mensaje);
                _db.AsignarParametroCadena("@detallesTecnicos", mensajeTecnico);
                if (!string.IsNullOrEmpty(idTrama))
                {
                    _db.AsignarParametroCadena("@tramaAsociada", idTrama);
                }
                if (!string.IsNullOrEmpty(idComprobante))
                {
                    _db.AsignarParametroCadena("@comprobanteAsociado", idComprobante);
                }
                if (!string.IsNullOrEmpty(idEmpleado))
                {
                    _db.AsignarParametroCadena("@idEmpleado", idEmpleado);
                }
                _db.AsignarParametroCadena("@RFCEMI", rfcEmisor);
                dr = _db.EjecutarConsulta();
                insertado = dr.Read();
            }
            catch (Exception)
            {
            }
            finally
            {
                _db.Desconectar();
            }
            return insertado;
        }

        /// <summary>
        /// Registrars the text.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <param name="clase">The clase.</param>
        /// <param name="metodo">The metodo.</param>
        /// <param name="mensajeTecnico">The mensaje tecnico.</param>
        public void RegistrarTxt(string mensaje, string clase, string metodo, string mensajeTecnico = "")
        {
            try
            {
                var rutaLog = AppDomain.CurrentDomain.BaseDirectory + @"log\";
                var DIR = new DirectoryInfo(rutaLog);
                if (!DIR.Exists) { DIR.Create(); }
                var dateHoy = Localization.Now;
                var _rutaArchivo = rutaLog + "Log_" + (!string.IsNullOrEmpty(_emisor) ? _emisor + "_" : "") + dateHoy.ToString("ddMMyyyy") + ".log";
                using (var w = File.AppendText(_rutaArchivo))
                {
                    w.Write("\r\nEvento de Log : ");
                    w.WriteLine("  Hora: {0}", dateHoy.ToString("hh:mm:ss"));
                    w.WriteLine("  Codigo: {0}", "100");
                    w.WriteLine("  Origen: {0}.{1}", clase, metodo);
                    w.WriteLine("  Mensaje: {0}", mensaje);
                    w.WriteLine("  Tecnico: {0}", mensajeTecnico);
                    w.WriteLine("  RFC Emisor: {0}", _emisor);
                    w.WriteLine("--------------------------------------------------------------------------------------");
                }
            }
            catch
            {
            }
        }
    }
}