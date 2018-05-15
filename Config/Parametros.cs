// ***********************************************************************
// Assembly         : Config
// Author           : Sergio
// Created          : 09-07-2016
//
// Last Modified By : Sergio
// Last Modified On : 04-09-2017
// ***********************************************************************
// <copyright file="Parametros.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Datos;

namespace Config
{
    /// <summary>
    /// Modo de uso:
    /// {...}
    /// var params = new Parametros(Rfc);
    /// {...}
    /// var valorParametro = params.GetParametroByNombre("EC000001").Status;
    /// </summary>
    public class Parametros
    {
        /// <summary>
        /// The emisor
        /// </summary>
        private readonly string Emisor = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="Parametros" /> class.
        /// </summary>
        /// <param name="Emisor">The emisor.</param>
        public Parametros(string Emisor)
        {
            this.Emisor = Emisor;
            ListaParametros = new List<Parametro>();
            CargaParametros();
        }

        /// <summary>
        /// Gets or sets the lista parametros.
        /// </summary>
        /// <value>The lista parametros.</value>
        public List<Parametro> ListaParametros { get; set; }

        #region sets

        /// <summary>
        /// Sets the parametro.
        /// </summary>
        /// <param name="nomConfiguracion">The nom configuracion.</param>
        /// <param name="descripcion">The descripcion.</param>
        /// <param name="status">The status.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SetParametro(string nomConfiguracion, string descripcion = "", string status = "")
        {
            return ListaParametros.Exists(p => p.NomConfiguracion.Equals(nomConfiguracion, StringComparison.OrdinalIgnoreCase)) ? ModificarParametro(nomConfiguracion, descripcion, status) : AgregarParametro(nomConfiguracion, descripcion, status);
        }

        /// <summary>
        /// Modificars the parametro.
        /// </summary>
        /// <param name="nomConfiguracion">The nom configuracion.</param>
        /// <param name="descripcion">The descripcion.</param>
        /// <param name="status">The status.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ModificarParametro(string nomConfiguracion, string descripcion, string status)
        {
            BasesDatos db = null;
            var estado = false;
            try
            {
                db = new BasesDatos(Emisor);
                db.Conectar();
                db.CrearComando(@"UPDATE Cat_Configuracion SET Descripcion = @Descripcion, Status = @Status WHERE NomConfiguracion = @nombre");
                db.AsignarParametroCadena("@nombre", nomConfiguracion);
                db.AsignarParametroCadena("@Descripcion", descripcion);
                db.AsignarParametroCadena("@Status", status);
                var dr = db.EjecutarConsulta();
                if (dr.RecordsAffected > 0)
                {
                    var parametro = GetParametroByNombre(nomConfiguracion);
                    if (parametro != null)
                    {
                        var indexOf = ListaParametros.IndexOf(parametro);
                        if (indexOf >= 0)
                        {
                            parametro.Descripcion = descripcion;
                            parametro.Status = status;
                            ListaParametros.Insert(indexOf, parametro);
                            estado = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                estado = false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(db.CadenaConexion))
                {
                    db.Desconectar();
                }
            }
            return estado;
        }

        /// <summary>
        /// Agregars the parametro.
        /// </summary>
        /// <param name="nomConfiguracion">The nom configuracion.</param>
        /// <param name="descripcion">The descripcion.</param>
        /// <param name="status">The status.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool AgregarParametro(string nomConfiguracion, string descripcion, string status)
        {
            BasesDatos db = null;
            var estado = false;
            try
            {
                db = new BasesDatos(Emisor);
                db.Conectar();
                db.CrearComando(@"INSERT INTO Cat_Configuracion (NomConfiguracion, Descripcion, Status) VALUES (@nombre, @Descripcion, @Status); SELECT SCOPE_IDENTITY() AS IdCong;");
                db.AsignarParametroCadena("@nombre", nomConfiguracion);
                db.AsignarParametroCadena("@Descripcion", descripcion);
                db.AsignarParametroCadena("@Status", status);
                var dr = db.EjecutarConsulta();
                if (dr.HasRows && dr.Read())
                {
                    var parametro = new Parametro
                    {
                        IdCong = dr["IdCong"].ToString(),
                        NomConfiguracion = nomConfiguracion,
                        Descripcion = descripcion,
                        Status = status
                    };
                    ListaParametros.Add(parametro);
                    estado = true;
                }
            }
            catch (Exception)
            {
                estado = false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(db.CadenaConexion))
                {
                    db.Desconectar();
                }
            }
            return estado;
        }

        #endregion

        #region gets

        /// <summary>
        /// Gets the parametro by nombre.
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <returns>Parametro.</returns>
        public Parametro GetParametroByNombre(string nombre)
        {
            var parametro = GetParametrosByNombre(nombre).FirstOrDefault();
            return parametro;
        }

        /// <summary>
        /// Gets the parametro by valor.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns>Parametro.</returns>
        public Parametro GetParametroByValor(string valor)
        {
            var parametro = GetParametrosByValor(valor).FirstOrDefault();
            return parametro;
        }

        /// <summary>
        /// Gets the parametro by descripcion.
        /// </summary>
        /// <param name="descripcion">The descripcion.</param>
        /// <returns>Parametro.</returns>
        public Parametro GetParametroByDescripcion(string descripcion)
        {
            var parametro = GetParametrosByDescripcion(descripcion).FirstOrDefault();
            return parametro;
        }

        /// <summary>
        /// Gets the parametros by nombre.
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <returns>List&lt;Parametro&gt;.</returns>
        public List<Parametro> GetParametrosByNombre(string nombre)
        {
            var parametros = ListaParametros.Where(p => p.NomConfiguracion.Equals(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
            return parametros;
        }

        /// <summary>
        /// Gets the parametros by valor.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns>List&lt;Parametro&gt;.</returns>
        public List<Parametro> GetParametrosByValor(string valor)
        {
            var parametros = ListaParametros.Where(p => p.Status.Equals(valor, StringComparison.OrdinalIgnoreCase)).ToList();
            return parametros;
        }

        /// <summary>
        /// Gets the parametros by descripcion.
        /// </summary>
        /// <param name="descripcion">The descripcion.</param>
        /// <returns>List&lt;Parametro&gt;.</returns>
        public List<Parametro> GetParametrosByDescripcion(string descripcion)
        {
            var parametros = ListaParametros.Where(p => p.Descripcion.Equals(descripcion, StringComparison.OrdinalIgnoreCase)).ToList();
            return parametros;
        }

        /// <summary>
        /// Cargas the parametros.
        /// </summary>
        private void CargaParametros()
        {
            BasesDatos db = null;
            try
            {
                db = new BasesDatos(Emisor);
                db.Conectar();
                db.CrearComando("SELECT * FROM Cat_Configuracion");
                var dr = db.EjecutarConsulta();
                while (dr.Read())
                {
                    try
                    {
                        var parametro = new Parametro
                        {
                            IdCong = dr["IdCong"].ToString(),
                            NomConfiguracion = dr["NomConfiguracion"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            Status = dr["Status"].ToString()
                        };
                        ListaParametros.Add(parametro);
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
            finally
            {
                if (!string.IsNullOrEmpty(db.CadenaConexion))
                {
                    db.Desconectar();
                }
            }
        }

        #endregion
    }
}

/// <summary>
/// Class Parametro.
/// </summary>
public class Parametro
{
    /// <summary>
    /// Gets or sets the identifier cong.
    /// </summary>
    /// <value>The identifier cong.</value>
    public string IdCong { get; set; } = "";

    /// <summary>
    /// Gets or sets the nom configuracion.
    /// </summary>
    /// <value>The nom configuracion.</value>
    public string NomConfiguracion { get; set; } = "";

    /// <summary>
    /// Gets or sets the descripcion.
    /// </summary>
    /// <value>The descripcion.</value>
    public string Descripcion { get; set; } = "";

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    public string Status { get; set; } = "";
}