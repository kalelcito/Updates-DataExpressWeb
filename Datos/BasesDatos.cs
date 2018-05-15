// ***********************************************************************
// Assembly         : Datos
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 11-30-2016
// ***********************************************************************
// <copyright file="BasesDatos.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using Mirabeau.MsSql.Library;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Datos
{
    /// <summary>
    /// Class BasesDatos.
    /// </summary>
    public class BasesDatos
    {
        /// <summary>
        /// The cadena conexion
        /// </summary>
        public string CadenaConexion;
        /// <summary>
        /// The database name
        /// </summary>
        public string DatabaseSchema;
        /// <summary>
        /// The comando
        /// </summary>
        public DbCommand Comando;
        /// <summary>
        /// The _conexion
        /// </summary>
        private DbConnection _conexion;
        /// <summary>
        /// The _factory
        /// </summary>
        private DbProviderFactory _factory;
        /// <summary>
        /// The _transaccion
        /// </summary>
        private readonly DbTransaction _transaccion = null;
        /// <summary>
        /// The _empresa tipo
        /// </summary>
        private string _empresaTipo = null;

        private void LoadAssembly()
        {
            try
            {
                SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            }
            catch (Exception ex)
            {
                try
                {
                    SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory + @"\bin");
                }
                catch (Exception ex1)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasesDatos" /> class.
        /// </summary>
        /// <param name="emisor">The emisor.</param>
        /// <param name="db">The database.</param>
        public BasesDatos(string emisor, string db = "Emision", string baseCore = "")
        {
            _empresaTipo = null;
            Configurar(emisor, db, baseCore);
            LoadAssembly();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasesDatos" /> class.
        /// </summary>
        public BasesDatos()
        {
            _empresaTipo = null;
            LoadAssembly();
        }

        public BasesDatos(string connectionString, int dummy)
        {
            _empresaTipo = null;
            CadenaConexion = connectionString;
            LoadAssembly();
        }

        /// <summary>
        /// Configura el acceso a la base de datos para su utilización.
        /// </summary>
        /// <param name="emisor">The emisor.</param>
        /// <param name="db">The database.</param>
        /// <exception cref="Datos.BaseDatosException">Error al cargar la configuración del acceso a datos.</exception>
        /// <exception cref="BaseDatosException">Si existe un error al cargar la configuración.</exception>
        private void Configurar(string emisor, string db, string baseCore = "")
        {
            try
            {
                var con = ObtenerCadena(emisor, db, baseCore);
                CadenaConexion = con[0];
                DatabaseSchema = con[1];
            }
            catch (ConfigurationException ex)
            {
                throw new BaseDatosException("Error al cargar la configuración del acceso a datos.", ex);
            }
        }

        /// <summary>
        /// Gets the empresa tipo.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetEmpresaTipo()
        {
            return _empresaTipo;
        }

        /// <summary>
        /// Obteners the cadena.
        /// </summary>
        /// <param name="rfcEmisor">The RFC emisor.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Datos.BaseDatosException">Error al cargar la configuración del acceso a datos.</exception>
        private string[] ObtenerCadena(string rfcEmisor, string dbName, string baseCore = "")
        {
            var dbLocal = new BasesDatos();
            var cadena = "";
            var dbSchema = "";
            try
            {
                var proveedor = ConfigurationManager.AppSettings.Get("PROVEEDOR_ADONET");
                dbLocal.CadenaConexion = ConfigurationManager.AppSettings.Get("CORE");
                dbLocal._factory = DbProviderFactories.GetFactory(proveedor);
                dbLocal.Conectar();
                dbLocal.CrearComando(@"SELECT
                                        p.CadenaConexion, e.EmpresaTipo
                                       FROM
	                                    Par_ParametrosEmisor p
	                                   INNER JOIN
		                                Cat_Emisor e ON p.idEmisor = e.IDEEMI
                                       WHERE	
	                                    e.RFCEMI = @rfc AND p.DB = @dbName");
                dbLocal.AsignarParametroCadena("@rfc", rfcEmisor);
                dbLocal.AsignarParametroCadena("@dbName", dbName);
                var dr = dbLocal.EjecutarConsulta();
                if (dr.Read())
                {
                    cadena = dr["CadenaConexion"].ToString();
                    _empresaTipo = dr["EmpresaTipo"].ToString();
                    try
                    {
                        dbLocal.Desconectar();
                        dbLocal = new BasesDatos();
                        dbLocal.CadenaConexion = cadena;
                        dbLocal._factory = DbProviderFactories.GetFactory(proveedor);
                        dbLocal.Conectar();
                        dbLocal.CrearComando("SELECT 'DUMMY'");
                        dbSchema = dbLocal.Comando.Connection.Database;
                    }
                    catch { }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseCore))
                    {
                        string val = baseCore;
                        cadena = val;
                        dbSchema = "IpsofactuMxEmision_YanbalDirectoras";
                    }
                    else
                    {
                        _empresaTipo = "3";
                        cadena = dbLocal.CadenaConexion;
                        dbSchema = dbLocal.Comando.Connection.Database;
                    }
                }
            }
            catch (ConfigurationException ex)
            {
                throw new BaseDatosException("Error al cargar la configuración del acceso a datos.", ex);
            }
            finally
            {
                if (dbLocal != null && dbLocal._conexion != null)
                {
                    dbLocal.Desconectar();
                }
                dbLocal = null;
            }
            return new string[] { cadena, dbSchema };
        }

        /// <summary>
        /// Permite desconectarse de la base de datos.
        /// </summary>
        public void Desconectar()
        {
            if (_conexion.State.Equals(ConnectionState.Open))
            {
                _conexion.Close();
            }
        }

        /// <summary>
        /// Se concecta con la base de datos.
        /// </summary>
        /// <exception cref="Datos.BaseDatosException">Error al conectarse a la base de datos.</exception>
        /// <exception cref="BaseDatosException">Error al conectarse a la base de datos.</exception>
        public void Conectar()
        {
            if (_conexion != null && !_conexion.State.Equals(ConnectionState.Closed))
            {
                //throw new BaseDatosException("La conexión ya se encuentra abierta.");
                Desconectar();
            }
            try
            {
                if (_conexion == null)
                {
                    var proveedor = ConfigurationManager.AppSettings.Get("PROVEEDOR_ADONET");
                    _factory = DbProviderFactories.GetFactory(proveedor);
                    _conexion = _factory.CreateConnection();
                    var cs = new SqlConnectionStringBuilder(CadenaConexion);
                    cs.ConnectTimeout = 1800;
                    _conexion.ConnectionString = cs.ConnectionString;
                }
                _conexion.Open();
            }
            catch (DataException ex)
            {
                throw new BaseDatosException("Error al conectarse a la base de datos.", ex);
            }
        }

        /// <summary>
        /// Crea un comando en base a una sentencia SQL.
        /// Ejemplo:
        /// <code>SELECT * FROM Tabla WHERE campo1=@campo1, campo2=@campo2</code>
        /// Guarda el comando para el seteo de parámetros y la posterior ejecución.
        /// </summary>
        /// <param name="sentenciaSql">La sentencia SQL con el formato: SENTENCIA [param = @param,]</param>
        public void CrearComando(string sentenciaSql)
        {
            Comando = _factory.CreateCommand();
            Comando.Connection = _conexion;
            Comando.CommandType = CommandType.Text;
            Comando.CommandText = sentenciaSql;
            Comando.CommandTimeout = 1800; // 20 minutos de timeout
            if (_transaccion != null)
            {
                Comando.Transaction = _transaccion;
            }
        }

        /// <summary>
        /// Crears the comando procedimiento.
        /// </summary>
        /// <param name="sentenciaSql">The sentencia SQL.</param>
        public void CrearComandoProcedimiento(string sentenciaSql)
        {
            Comando = _factory.CreateCommand();
            Comando.Connection = _conexion;
            Comando.CommandType = CommandType.StoredProcedure;
            Comando.CommandText = sentenciaSql;
            Comando.CommandTimeout = 1800; // 10 minutos de timeout
            if (_transaccion != null)
            {
                Comando.Transaction = _transaccion;
            }
        }


        /// <summary>
        /// Asignars the parametro procedimiento.
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <param name="tipo">The tipo.</param>
        /// <param name="valor">The valor.</param>
        public void AsignarParametroProcedimiento(string nombre, DbType tipo, object valor)
        {
            var param = Comando.CreateParameter();
            param.ParameterName = nombre;
            param.DbType = tipo;
            //if (valor.ToString().Contains("'"))
            //{
            //    valor = valor.ToString().Replace("'", "''");
            //}
            param.Value = valor;
            Comando.Parameters.Add(param);
        }

        /// <summary>
        /// Asignars the parametro flotante.
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <param name="valor">The valor.</param>
        public void AsignarParametroFlotante(string nombre, string valor)
        {
            if (valor == " ")
            {
                valor = "0";
            }
            AsignarParametro(nombre, "", valor);
        }

        /// <summary>
        /// Asigna un parámetro de tipo entero al comando creado.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro.</param>
        /// <param name="valor">El valor del parámetro.</param>
        public void AsignarParametroEntero(string nombre, int valor)
        {
            AsignarParametro(nombre, "", valor.ToString());
        }

        /// <summary>
        /// Asigna un parámetro de tipo cadena al comando creado.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro.</param>
        /// <param name="valor">El valor del parámetro.</param>
        public void AsignarParametroCadena(string nombre, string valor)
        {
            AsignarParametro(nombre, "'", valor);
        }

        /// <summary>
        /// Asignars the parametro byte array.
        /// </summary>
        /// <param name="nombre">The nombre.</param>
        /// <param name="valor">The valor.</param>
        public void AsignarParametroByteArray(string nombre, byte[] valor)
        {
            var param = "0x" + System.BitConverter.ToString(valor).Replace("-", "");
            AsignarParametro(nombre, "", param);
        }

        /// <summary>
        /// Asigna un parámetro de tipo fecha al comando creado.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro.</param>
        /// <param name="valor">El valor del parámetro.</param>
        public void AsignarParametroFecha(string nombre, string valor)
        {
            AsignarParametro(nombre, "'", valor);
        }

        /// <summary>
        /// Asigna un parámetro al comando creado.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro.</param>
        /// <param name="separador">El separador que será agregado al valor del parámetro.</param>
        /// <param name="valor">El valor del parámetro.</param>
        private void AsignarParametro(string nombre, string separador, string valor)
        {
            var indice = Comando.CommandText.IndexOf(nombre);
            var prefijo = Comando.CommandText.Substring(0, indice);
            var sufijo = Comando.CommandText.Substring(indice + nombre.Length);
            Comando.CommandText = prefijo + separador + (!string.IsNullOrEmpty(valor) ? valor.Replace("'", "''") : "") + separador + sufijo;
        }

        /// <summary>
        /// Ejecuta el comando creado y retorna el resultado de la consulta.
        /// </summary>
        /// <returns>El resultado de la consulta.</returns>
        /// <exception cref="BaseDatosException">Si ocurre un error al ejecutar el comando.</exception>
        public DbDataReader EjecutarConsulta()
        {
            try
            {
                return Comando.ExecuteReader();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    return RetryOnDeadlock();
                }
            }
            return null;
        }

        /// <summary>
        /// Ejecutars the consulta2.
        /// </summary>
        /// <param name="men">The men.</param>
        public void EjecutarConsulta2(ref string men)
        {
            try
            {
                try
                {
                    Comando.ExecuteReader();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 1205)
                    {
                        RetryOnDeadlock();
                    }
                }
                //this.comando.ExecuteNonQuery();
                men = "";
            }
            //catch (Exception)
            catch (SqlException ex)
            {
                var err = ex.Errors[0];
                var mensaje = string.Empty;
                mensaje = err.ToString();
                men = mensaje;
            }
        }

        /// <summary>
        /// Ejecutars the consulta3.
        /// </summary>
        /// <param name="men">The men.</param>
        /// <returns>DbDataReader.</returns>
        public DbDataReader EjecutarConsulta3(ref string men)
        {
            try
            {
                try
                {
                    return Comando.ExecuteReader();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 1205)
                    {
                        return RetryOnDeadlock();
                    }
                }
            }
            //catch (Exception)
            catch (SqlException ex)
            {
                var err = ex.Errors[0];
                var mensaje = string.Empty;
                mensaje = err.ToString();
                men = mensaje;

            }
            return null;
        }

        /// <summary>
        /// Ejecuta el comando creado .
        /// </summary>
        /// <exception cref="BaseDatosException">Si ocurre un error al ejecutar el comando.</exception>
        public void EjecutarConsulta1()
        {
            try
            {
                Comando.ExecuteReader();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    RetryOnDeadlock();
                }
            }
        }

        private DbDataReader RetryOnDeadlock()
        {
            var query = Comando.CommandText;
            SqlDataReader reader = null;
            try
            {
                var pars = Comando.Parameters.Cast<SqlParameter>().ToList();
                var rs = SqlDebugHelper.CreateExecutableSqlStatement(Comando.CommandText, Comando.CommandType, pars).Replace("@@", "@");
                query = rs;
                bool retry = false;
                do
                {
                    try
                    {
                        var sqlConnection = new SqlConnection(Comando.Connection.ConnectionString);
                        var sqlCommand = new SqlCommand(query, sqlConnection);
                        reader = sqlCommand.ExecuteReader();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 1205)
                        {
                            retry = true;
                        }
                    }
                } while (retry);
            }
            catch (Exception ex)
            {
                // ignored
            }
            return reader;
        }
    }
}