// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="Certificado.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Datos;
using Control;

namespace DataExpressWeb.configuracion.Herramientas
{
    /// <summary>
    /// Class Certificado.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Certificado : Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;
        /// <summary>
        /// The log
        /// </summary>
        private static Log _log;
        /// <summary>
        /// The _id editar
        /// </summary>
        private static string _idEditar;
        /// <summary>
        /// The identifier user
        /// </summary>
        private static string _idUser;
        /// <summary>
        /// The _ruta certificados
        /// </summary>
        private static string _rutaCertificados;
        /// <summary>
        /// The _ruta llaves
        /// </summary>
        private static string _rutaLlaves;
        //private static byte[] cerBytes;
        //private static byte[] keyBytes;
        /// <summary>
        /// The _posted cer
        /// </summary>
        private static HttpPostedFile _postedCer;
        /// <summary>
        /// The _posted key
        /// </summary>
        private static HttpPostedFile _postedKey;

        /// <summary>
        /// The query certificados
        /// </summary>
        private static readonly string queryCertificados = "SELECT c.IDECNF, c.CERNUM, (CONVERT(VARCHAR, c.FOLIOINICIO) + ' - ' + CONVERT(VARCHAR, c.FOLIOFIN)) AS FOLIOS, c.FECHAINICIO, c.FECHAFIN, c.RFCEMISOR AS RFCEMI FROM Cat_Certificados c";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos(Session["IDENTEMI"].ToString());
                _log = new Log(Session["IDENTEMI"].ToString());
                SqlDataCertificados.ConnectionString = _db.CadenaConexion;
                SqlDataCertificados.SelectCommand = queryCertificados + (Session["idUser"].ToString().Equals("1") ? "" : " WHERE c.invisible = 'False'");
                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                }
            }
            if (!IsPostBack)
            {
                _postedCer = null;
                _postedCer = null;
                _idEditar = null;
                var sql = @"SELECT dircertificados, dirllaves FROM Par_ParametrosSistema";
                _db.Conectar();
                _db.CrearComando(sql);
                var dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    _rutaCertificados = dr["dircertificados"].ToString();
                    _rutaLlaves = dr["dirllaves"].ToString();
                }
                _db.Desconectar();
            }
        }

        /// <summary>
        /// Handles the Click event of the bDetalles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bDetalles_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            _idEditar = btn.CommandArgument.Split('|')[0];
            var rfc = btn.CommandArgument.Split('|')[1];
            tbRfcEmisor.Text = rfc;
            var sql = "";
            _db.Conectar();
            var css = "form-control" + (string.IsNullOrEmpty(_idEditar) ? " input-number" : "");
            tbFolioInicio.CssClass = css;
            tbFolioInicio.ReadOnly = !string.IsNullOrEmpty(_idEditar);
            if (!string.IsNullOrEmpty(_idEditar))
            {
                sql = "SELECT * FROM Cat_Certificados WHERE IDECNF = @ID";
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", _idEditar);
                bActualizar.Text = "Actualizar";
            }
            //else
            //{
            //    sql = "SELECT * FROM Cat_Certificados WHERE RFCEMISOR = @RFC AND ISNULL(IDECNF, '') = ''";
            //    _db.CrearComando(sql);
            //    _db.AsignarParametroCadena("@RFC", rfc);
            //    bActualizar.Text = "Agregar";
            //}
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                tbFolioInicio.Text = dr["FOLIOINICIO"].ToString();
                tbFolioFin.Text = dr["FOLIOFIN"].ToString();
                tbNumAprob.Text = dr["NUMAPROB"].ToString();
                tbAnioAprob.Text = dr["ANIOAPROB"].ToString();
                tbFechaInicio.Text = Control.Localization.Parse(dr["FECHAINICIO"].ToString()).ToString("s");
                tbFechaFin.Text = Control.Localization.Parse(dr["FECHAFIN"].ToString()).ToString("s");
                tbNoCert.Text = dr["CERNUM"].ToString();
                var cerFileName = dr["CERRUT"].ToString();
                var keyFileName = dr["PRVRUT"].ToString();
                var rutaCer = _rutaCertificados + @"\" + cerFileName;
                var rutaKey = _rutaLlaves + @"\" + keyFileName;
                tbCer.Text = File.Exists(rutaCer) ? cerFileName : "";
                tbKey.Text = File.Exists(rutaKey) ? keyFileName : "";
                tbClave1.Attributes["value"] = dr["CLAVE"].ToString();
                tbClave2.Attributes["value"] = dr["CLAVE"].ToString();
            }
            else
            {
                tbFolioInicio.Text = "";
                tbFolioFin.Text = "";
                tbNumAprob.Text = "0";
                tbAnioAprob.Text = "";
                tbFechaInicio.Text = "";
                tbFechaFin.Text = "";
                tbNoCert.Text = "";
                tbCer.Text = "";
                tbKey.Text = "";
                tbClave1.Text = "";
                tbClave2.Text = "";
            }
            tbRfcEmisor.ReadOnly = true;
            _db.Desconectar();
            _postedCer = null;
            _postedCer = null;
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "limpiarFiles(); $('#divEditar').modal('show');", true);
        }

        /// <summary>
        /// Handles the UploadedComplete event of the fileCER control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AsyncFileUploadEventArgs" /> instance containing the event data.</param>
        protected void fileCER_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            //cerBytes = fileCER.FileBytes;
            _postedCer = fileCER.PostedFile;
        }

        /// <summary>
        /// Handles the UploadedComplete event of the fileKey control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AsyncFileUploadEventArgs" /> instance containing the event data.</param>
        protected void fileKey_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            //keyBytes = fileKey.FileBytes;
            _postedKey = fileKey.PostedFile;
        }

        /// <summary>
        /// Handles the Click event of the bActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">
        /// El certificado para el emisor " + tbRfcEmisor.Text + " ya existe
        /// or
        /// No se pudo guardar el certificado en " + _rutaCertificados
        /// or
        /// No se pudo obtener la información del certificado.<br/>Razón: " + ex.Message
        /// or
        /// or
        /// No se pudo guardar la llave en " + _rutaLlaves
        /// or
        /// No se pudo validar la llave con la clave introducida. Inténtelo de nuevo.<br/>" + ex.Message
        /// or
        /// No se pudo actualizar el registro de certificado. Inténtelo de nuevo.<br/>Razón: " + ex.Message
        /// </exception>
        /// <exception cref="System.Exception">El certificado para el emisor  + tbRfcEmisor.Text +  ya existe
        /// or
        /// No se pudo guardar el certificado en  + _rutaCertificados
        /// or
        /// No se pudo obtener la información del certificado.<br />Razón:  + ex.Message
        /// or
        /// or
        /// No se pudo guardar la llave en  + _rutaLlaves
        /// or
        /// No se pudo validar la llave con la clave introducida. Inténtelo de nuevo.<br /> + ex.Message
        /// or
        /// No se pudo actualizar el registro de certificado. Inténtelo de nuevo.<br />Razón:  + ex.Message</exception>
        protected void bActualizar_Click(object sender, EventArgs e)
        {
            if (tbClave1.Text.Equals(tbClave2.Text))
            {
                try
                {
                    if (string.IsNullOrEmpty(_idEditar) && certificadoExistente(tbRfcEmisor.Text))
                    {
                        throw new Exception("El certificado para el emisor " + tbRfcEmisor.Text + " ya existe");
                    }
                    var certUtils = new Certificados();
                    var rutaCer = "";
                    var rutaKey = "";
                    try
                    {
                        var cerFileName = tbCer.Text;
                        rutaCer = _rutaCertificados + @"\" + cerFileName;
                        var bytesCer = _postedCer != null ? PostedFileToBytes(_postedCer) : File.ReadAllBytes(rutaCer);
                        string numCert;
                        string fechaInicio;
                        string fechaFin;
                        certUtils.CertificateData(bytesCer, out numCert, out fechaInicio, out fechaFin);
                        if (_postedCer != null)
                        {
                            rutaCer = _rutaCertificados + @"\" + _postedCer.FileName;
                            _postedCer.SaveAs(rutaCer);
                            if (!File.Exists(rutaCer))
                            {
                                throw new Exception("No se pudo guardar el certificado en " + _rutaCertificados);
                            }
                        }
                        tbNoCert.Text = numCert;
                        tbFechaInicio.Text = Control.Localization.Parse(fechaInicio).ToString("s");
                        tbFechaFin.Text = Control.Localization.Parse(fechaFin).ToString("s");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo obtener la información del certificado.<br/>Razón: " + ex.Message, ex);
                    }
                    try
                    {
                        var keyFileName = tbKey.Text;
                        rutaKey = _rutaLlaves + @"\" + keyFileName;
                        var bytesKey = _postedKey != null ? PostedFileToBytes(_postedKey) : File.ReadAllBytes(rutaKey);
                        var rsa = certUtils.OpenKeyFile(bytesKey, tbClave1.Text);
                        if (rsa == null)
                        {
                            throw new Exception("");
                        }
                        if (_postedKey != null)
                        {
                            rutaKey = _rutaLlaves + @"\" + _postedKey.FileName;
                            _postedKey.SaveAs(rutaKey);
                            if (!File.Exists(rutaKey))
                            {
                                throw new Exception("No se pudo guardar la llave en " + _rutaLlaves);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo validar la llave con la clave introducida. Inténtelo de nuevo.<br/>" + ex.Message, ex);
                    }
                    try
                    {
                        var sql = "";
                        if (string.IsNullOrEmpty(_idEditar))
                        {
                            sql = @"INSERT INTO Cat_Certificados
                                               (CERRUT
                                               ,PRVRUT
                                               ,CLAVE
                                               ,CERNUM
                                               ,EDOCER
                                               ,NUMAPROB
                                               ,ANIOAPROB
                                               ,FOLIOINICIO
                                               ,FOLIOFIN
                                               ,FECHAINICIO
                                               ,FECHAFIN
                                               ,RFCEMISOR)
                                         VALUES
                                               (@CERRUT
                                               ,@PRVRUT
                                               ,@CLAVE
                                               ,@CERNUM
                                               ,@EDOCER
                                               ,@NUMAPROB
                                               ,@ANIOAPROB
                                               ,@FOLIOINICIO
                                               ,@FOLIOFIN
                                               ,@FECHAINICIO
                                               ,@FECHAFIN
                                               ,@RFCEMISOR)";
                        }
                        else
                        {
                            sql = @"UPDATE Cat_Certificados
                                       SET CERRUT = @CERRUT
                                          ,PRVRUT = @PRVRUT
                                          ,CLAVE = @CLAVE
                                          ,CERNUM = @CERNUM
                                          ,EDOCER = @EDOCER
                                          ,NUMAPROB = @NUMAPROB
                                          ,ANIOAPROB = @ANIOAPROB
                                          ,FOLIOINICIO = @FOLIOINICIO
                                          ,FOLIOFIN = @FOLIOFIN
                                          ,FECHAINICIO = @FECHAINICIO
                                          ,FECHAFIN = @FECHAFIN
                                          ,RFCEMISOR = @RFCEMISOR
                                     WHERE IDECNF = @ID";
                        }
                        _db.Conectar();
                        _db.CrearComando(sql);
                        if (!string.IsNullOrEmpty(_idEditar))
                        {
                            _db.AsignarParametroCadena("@ID", _idEditar);
                        }
                        _db.AsignarParametroCadena("@CERRUT", Path.GetFileName(rutaCer));
                        _db.AsignarParametroCadena("@PRVRUT", Path.GetFileName(rutaKey));
                        _db.AsignarParametroCadena("@CLAVE", tbClave1.Text);
                        _db.AsignarParametroCadena("@CERNUM", tbNoCert.Text);
                        _db.AsignarParametroCadena("@EDOCER", "x");
                        _db.AsignarParametroCadena("@NUMAPROB", tbNumAprob.Text);
                        _db.AsignarParametroCadena("@ANIOAPROB", tbAnioAprob.Text);
                        _db.AsignarParametroCadena("@FOLIOINICIO", tbFolioInicio.Text);
                        _db.AsignarParametroCadena("@FOLIOFIN", tbFolioFin.Text);
                        _db.AsignarParametroCadena("@FECHAINICIO", tbFechaInicio.Text);
                        _db.AsignarParametroCadena("@FECHAFIN", tbFechaFin.Text);
                        _db.AsignarParametroCadena("@RFCEMISOR", tbRfcEmisor.Text);
                        _db.EjecutarConsulta1();
                        tbFolioInicio.Text = "";
                        tbFolioFin.Text = "";
                        tbNumAprob.Text = "";
                        tbAnioAprob.Text = "";
                        tbNoCert.Text = "";
                        tbRfcEmisor.Text = "";
                        tbCer.Text = "";
                        tbKey.Text = "";
                        tbClave1.Text = "";
                        tbClave2.Text = "";
                        SqlDataCertificados.DataBind();
                        gvCertificados.DataBind();
                        (Master as SiteMaster).MostrarAlerta(this, "El certificado se ha actualizado correctamente", 2, null, "$('#divEditar').modal('hide');");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo actualizar el registro de certificado. Inténtelo de nuevo.<br/>Razón: " + ex.Message, ex);
                    }
                    finally
                    {
                        _db.Desconectar();
                    }
                }
                catch (Exception ex)
                {
                    var metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    RegLog(ex.Message, metodo, ex.ToString());
                    (Master as SiteMaster).MostrarAlerta(this, ex.Message, 4, null);
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "Ambas claves deben coincidir. Inténtelo de nuevo.", 4, null);
            }
        }

        /// <summary>
        /// Regs the log.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <param name="metodoActual">The metodo actual.</param>
        /// <param name="mensajeTecnico">The mensaje tecnico.</param>
        private void RegLog(string mensaje, string metodoActual, string mensajeTecnico = "")
        {
            _log.Registrar(mensaje, GetType().Name, metodoActual, null, mensajeTecnico, null, null, _idUser, Session["IDENTEMI"].ToString());
        }

        /// <summary>
        /// Certificadoes the existente.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool certificadoExistente(string rfc)
        {
            var existe = false;
            try
            {
                _db.Conectar();
                _db.CrearComando("SELECT IDECNF FROM Cat_Certificados WHERE RFCEMISOR = @rfc");
                _db.AsignarParametroCadena("@rfc", rfc);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    existe = true;
                }
                _db.Desconectar();
            }
            catch { }
            return existe;
        }

        /// <summary>
        /// Posteds the file to bytes.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.Byte[].</returns>
        private byte[] PostedFileToBytes(HttpPostedFile file)
        {
            var output = new MemoryStream();
            var stream = file.InputStream;
            using (output)
            {
                stream.Position = 0;
                stream.CopyTo(output);
            }
            var result = output.ToArray();
            return result;
        }

        /// <summary>
        /// Handles the Click event of the lbNuevo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbNuevo_Click(object sender, EventArgs e)
        {
            tbRfcEmisor.Text = "";
            tbRfcEmisor.ReadOnly = false;
            tbFolioInicio.Text = "";
            tbFolioFin.Text = "";
            tbNumAprob.Text = "";
            tbAnioAprob.Text = "";
            tbFechaInicio.Text = "";
            tbFechaFin.Text = "";
            tbNoCert.Text = "";
            tbCer.Text = "";
            tbKey.Text = "";
            tbClave1.Text = "";
            tbClave2.Text = "";
            bActualizar.Text = "Agregar";
            _postedCer = null;
            _postedCer = null;
            _idEditar = null;
            var css = "form-control" + (string.IsNullOrEmpty(_idEditar) ? " input-number" : "");
            tbFolioInicio.CssClass = css;
            tbFolioInicio.ReadOnly = !string.IsNullOrEmpty(_idEditar);
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "limpiarFiles(); $('#divEditar').modal('show');", true);
        }
    }

    /// <summary>
    /// Class Certificados.
    /// </summary>
    public class Certificados
    {
        /// <summary>
        /// Decodes the private key information.
        /// </summary>
        /// <param name="encpkcs8">The encpkcs8.</param>
        /// <param name="pPassword">The p password.</param>
        /// <returns>RSACryptoServiceProvider.</returns>
        public RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] encpkcs8, string pPassword)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            // this byte[] includes the sequence byte and terminal encoded null
            byte[] oiDpkcs5Pbes2 = { 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x05, 0x0D };
            byte[] oiDpkcs5Pbkdf2 = { 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x05, 0x0C };
            byte[] oiDdesEde3Cbc = { 0x06, 0x08, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x03, 0x07 };

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            var mem = new MemoryStream(encpkcs8);
            var binr = new BinaryReader(mem); //wrap Memory Stream with BinaryReader for easy reading

            try
            {
                var twobytes = binr.ReadUInt16();
                switch (twobytes)
                {
                    case 0x8130:
                        //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte(); //advance 1 byte
                        break;
                    case 0x8230:
                        binr.ReadInt16(); //advance 2 bytes
                        break;
                    default:
                        return null;
                        /*
                                                        break;
                                */
                }

                twobytes = binr.ReadUInt16(); //inner sequence
                switch (twobytes)
                {
                    case 0x8130:
                        binr.ReadByte();
                        break;
                    case 0x8230:
                        binr.ReadInt16();
                        break;
                }

                var seq = binr.ReadBytes(11);
                if (!CompareBytearrays(seq, oiDpkcs5Pbes2)) //is it a OIDpkcs5PBES2 ?
                {
                    return null;
                }

                twobytes = binr.ReadUInt16(); //inner sequence for pswd salt
                switch (twobytes)
                {
                    case 0x8130:
                        binr.ReadByte();
                        break;
                    case 0x8230:
                        binr.ReadInt16();
                        break;
                }

                twobytes = binr.ReadUInt16(); //inner sequence for pswd salt
                switch (twobytes)
                {
                    case 0x8130:
                        binr.ReadByte();
                        break;
                    case 0x8230:
                        binr.ReadInt16();
                        break;
                }

                seq = binr.ReadBytes(11); //read the Sequence OID
                if (!CompareBytearrays(seq, oiDpkcs5Pbkdf2)) //is it a OIDpkcs5PBKDF2 ?
                {
                    return null;
                }

                twobytes = binr.ReadUInt16();
                switch (twobytes)
                {
                    case 0x8130:
                        binr.ReadByte();
                        break;
                    case 0x8230:
                        binr.ReadInt16();
                        break;
                }

                var bt = binr.ReadByte();
                if (bt != 0x04) //expect octet string for salt
                {
                    return null;
                }
                int saltsize = binr.ReadByte();
                var salt = binr.ReadBytes(saltsize);

                bt = binr.ReadByte();
                if (bt != 0x02) //expect an integer for PBKF2 interation count
                {
                    return null;
                }

                int itbytes = binr.ReadByte(); //PBKD2 iterations should fit in 2 bytes.
                int iterations;
                if (itbytes == 1)
                {
                    iterations = binr.ReadByte();
                }
                else if (itbytes == 2)
                {
                    iterations = 256 * binr.ReadByte() + binr.ReadByte();
                }
                else
                {
                    return null;
                }

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                {
                    binr.ReadByte();
                }
                else if (twobytes == 0x8230)
                {
                    binr.ReadInt16();
                }

                var seqdes = binr.ReadBytes(10);
                if (!CompareBytearrays(seqdes, oiDdesEde3Cbc)) //is it a OIDdes-EDE3-CBC ?
                {
                    return null;
                }

                bt = binr.ReadByte();
                if (bt != 0x04) //expect octet string for IV
                {
                    return null;
                }
                int ivsize = binr.ReadByte();
                var iv = binr.ReadBytes(ivsize);

                bt = binr.ReadByte();
                if (bt != 0x04) // expect octet string for encrypted PKCS8 data
                {
                    return null;
                }

                bt = binr.ReadByte();

                int encblobsize;
                if (bt == 0x81)
                {
                    encblobsize = binr.ReadByte(); // data size in next byte
                }
                else if (bt == 0x82)
                {
                    encblobsize = 256 * binr.ReadByte() + binr.ReadByte();
                }
                else
                {
                    encblobsize = bt; // we already have the data size
                }

                var encryptedpkcs8 = binr.ReadBytes(encblobsize);
                var secpswd = new SecureString();
                foreach (var c in pPassword)
                {
                    secpswd.AppendChar(c);
                }

                var pkcs8 = DecryptPbdk2(encryptedpkcs8, salt, iv, secpswd, iterations);
                if (pkcs8 == null) // probably a bad pswd entered.
                {
                    return null;
                }

                var rsa = DecodePrivateKeyInfo(pkcs8);
                return rsa;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        /// <summary>
        /// Decodes the private key information.
        /// </summary>
        /// <param name="pkcs8">The PKCS8.</param>
        /// <returns>RSACryptoServiceProvider.</returns>
        public RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            // this byte[] includes the sequence byte and terminal encoded null
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            var mem = new MemoryStream(pkcs8);
            var lenstream = (int)mem.Length;
            var binr = new BinaryReader(mem); //wrap Memory Stream with BinaryReader for easy reading

            try
            {
                var twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                {
                    binr.ReadByte(); //advance 1 byte
                }
                else if (twobytes == 0x8230)
                {
                    binr.ReadInt16(); //advance 2 bytes
                }
                else
                {
                    return null;
                }

                var bt = binr.ReadByte();
                if (bt != 0x02)
                {
                    return null;
                }

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                {
                    return null;
                }

                var seq = binr.ReadBytes(15);
                if (!CompareBytearrays(seq, seqOid)) //make sure Sequence for OID is correct
                {
                    return null;
                }

                bt = binr.ReadByte();
                if (bt != 0x04) //expect an Octet string
                {
                    return null;
                }

                bt = binr.ReadByte(); //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                if (bt == 0x81)
                {
                    binr.ReadByte();
                }
                else if (bt == 0x82)
                {
                    binr.ReadUInt16();
                }
                //------ at this stage, the remaining sequence should be the RSA private key

                var rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                var rsacsp = DecodeRsaPrivateKey(rsaprivkey);
                return rsacsp;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        /// <summary>
        /// Decodes the RSA private key.
        /// </summary>
        /// <param name="privkey">The privkey.</param>
        /// <returns>RSACryptoServiceProvider.</returns>
        public RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privkey)
        {
            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            var mem = new MemoryStream(privkey);
            var binr = new BinaryReader(mem); //wrap Memory Stream with BinaryReader for easy reading
            try
            {
                var twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                {
                    binr.ReadByte(); //advance 1 byte
                }
                else if (twobytes == 0x8230)
                {
                    binr.ReadInt16(); //advance 2 bytes
                }
                else
                {
                    return null;
                }

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                {
                    return null;
                }
                var bt = binr.ReadByte();
                if (bt != 0x00)
                {
                    return null;
                }

                //------  all private key components are Integer sequences ----
                var elems = GetIntegerSize(binr);
                var modulus = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var e = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var d = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var p = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var dp = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var dq = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var iq = binr.ReadBytes(elems);

                Console.WriteLine("showing components ..");

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                var rsa = new RSACryptoServiceProvider();
                var rsAparams = new RSAParameters { Modulus = modulus, Exponent = e, D = d, P = p, Q = q, DP = dp, DQ = dq, InverseQ = iq };
                rsa.ImportParameters(rsAparams);
                return rsa;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        /// <summary>
        /// Decrypts the PBDK2.
        /// </summary>
        /// <param name="edata">The edata.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iv">The iv.</param>
        /// <param name="secpswd">The secpswd.</param>
        /// <param name="iterations">The iterations.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] DecryptPbdk2(byte[] edata, byte[] salt, byte[] iv, SecureString secpswd, int iterations)
        {
            var psbytes = new byte[secpswd.Length];
            var unmanagedPswd = Marshal.SecureStringToGlobalAllocAnsi(secpswd);
            Marshal.Copy(unmanagedPswd, psbytes, 0, psbytes.Length);
            Marshal.ZeroFreeGlobalAllocAnsi(unmanagedPswd);

            try
            {
                var kd = new Rfc2898DeriveBytes(psbytes, salt, iterations);
                var decAlg = TripleDES.Create();
                decAlg.Key = kd.GetBytes(24);
                decAlg.IV = iv;
                var memstr = new MemoryStream();
                var decrypt = new CryptoStream(memstr, decAlg.CreateDecryptor(), CryptoStreamMode.Write);
                decrypt.Write(edata, 0, edata.Length);
                decrypt.Flush();
                decrypt.Close(); // this is REQUIRED.
                var cleartext = memstr.ToArray();
                return cleartext;
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem decrypting: {0}", e.Message);
                return null;
            }
        }

        /// <summary>
        /// Certificates the data.
        /// </summary>
        /// <param name="bCerFile">The b cer file.</param>
        /// <param name="certificateNumber">The certificate number.</param>
        /// <param name="effectiveDate">The effective date.</param>
        /// <param name="expirationDate">The expiration date.</param>
        public void CertificateData(byte[] bCerFile, out string certificateNumber, out string effectiveDate, out string expirationDate)
        {
            var cert = new X509Certificate(bCerFile);
            var strcert = cert.GetRawCertData();
            strcert = cert.GetSerialNumber();
            certificateNumber = Reverse(Encoding.UTF8.GetString(strcert));
            effectiveDate = cert.GetEffectiveDateString();
            expirationDate = cert.GetExpirationDateString();
        }

        /// <summary>
        /// Opens the key file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="pPassword">The p password.</param>
        /// <returns>RSACryptoServiceProvider.</returns>
        public RSACryptoServiceProvider OpenKeyFile(string filename, string pPassword)
        {
            var keyblob = GetFileBytes(filename);
            if (keyblob == null)
            {
                return null;
            }
            var rsa = DecodePrivateKeyInfo(keyblob, pPassword);
            if (rsa != null)
            {
                return rsa;
            }
            return null;
        }

        /// <summary>
        /// Opens the key file.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="pPassword">The p password.</param>
        /// <returns>RSACryptoServiceProvider.</returns>
        public RSACryptoServiceProvider OpenKeyFile(byte[] bytes, string pPassword)
        {
            var keyblob = bytes;
            if (keyblob == null)
            {
                return null;
            }
            var rsa = DecodePrivateKeyInfo(keyblob, pPassword);
            if (rsa != null)
            {
                return rsa;
            }
            return null;
        }

        /// <summary>
        /// Reverses the specified original.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns>System.String.</returns>
        public string Reverse(string original)
        {
            var reverse = "";
            for (var i = original.Length - 1; i >= 0; i--)
            {
                reverse += original.Substring(i, 1);
            }
            return reverse;
        }

        /// <summary>
        /// Signs the string.
        /// </summary>
        /// <param name="pKeyFile">The p key file.</param>
        /// <param name="pPassword">The p password.</param>
        /// <param name="originalString">The original string.</param>
        /// <returns>System.String.</returns>
        public string SignString(string pKeyFile, string pPassword, string originalString)
        {
            var signedString = "";
            var filename = pKeyFile;
            if (!File.Exists(filename))
            {
                return ".key file does not exist " + pKeyFile;
            }

            var rsa = OpenKeyFile(filename, pPassword);
            if (rsa != null)
            {
                var co = Encoding.UTF8.GetBytes(originalString);
                var signedBytes = rsa.SignData(co, new SHA1CryptoServiceProvider());
                signedString = Convert.ToBase64String(signedBytes);
            }
            return signedString;
        }

        /// <summary>
        /// Compares the bytearrays.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            var i = 0;
            foreach (var c in a)
            {
                if (c != b[i])
                {
                    return false;
                }
                i++;
            }
            return true;
        }

        /// <summary>
        /// Gets the file bytes.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>System.Byte[].</returns>
        private byte[] GetFileBytes(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }
            Stream stream = new FileStream(filename, FileMode.Open);
            var datalen = (int)stream.Length;
            var filebytes = new byte[datalen];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(filebytes, 0, datalen);
            stream.Close();
            return filebytes;
        }

        /// <summary>
        /// Gets the size of the integer.
        /// </summary>
        /// <param name="binr">The binr.</param>
        /// <returns>System.Int32.</returns>
        private int GetIntegerSize(BinaryReader binr)
        {
            int count;
            var bt = binr.ReadByte();
            if (bt != 0x02) //expect integer
            {
                return 0;
            }
            bt = binr.ReadByte();

            if (bt == 0x81)
            {
                count = binr.ReadByte(); // data size in next byte
            }
            else if (bt == 0x82)
            {
                var highbyte = binr.ReadByte();
                var lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt; // we already have the data size
            }
            while (binr.ReadByte() == 0x00)
            {
                //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }
}