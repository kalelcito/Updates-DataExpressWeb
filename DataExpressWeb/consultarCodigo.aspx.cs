// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 08-09-2017
// ***********************************************************************
// <copyright file="consultarCodigo.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Datos;
using System.Web.UI;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.IO;
using Control;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using ColorThiefDotNet;
using System.Web.Services;
using System.Xml;
using Config;
using System.Globalization;

namespace DataExpressWeb
{
    /// <summary>
    /// Class ConsultarCodigo.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class ConsultarCodigo : System.Web.UI.Page
    {

        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The log
        /// </summary>
        private Log _log;

        /// <summary>
        /// The detalles
        /// </summary>
        private List<Dictionary<string, string>> _detalles;
        /// <summary>
        /// The impuestos
        /// </summary>
        private List<Dictionary<string, string>> _impuestos;
        /// <summary>
        /// The retenciones
        /// </summary>
        private List<Dictionary<string, string>> _retenciones;
        /// <summary>
        /// The identificadores table
        /// </summary>
        DataTable _identificadoresTable = new DataTable("TablaIdentificadores");
        /// <summary>
        /// The registro
        /// </summary>
        string[] _registro;
        /// <summary>
        /// The array_registros
        /// </summary>
        static ArrayList _arrayRegistros = new ArrayList();
        /// <summary>
        /// The id_ renglon
        /// </summary>
        static int _idRenglon = 0;
        /// <summary>
        ///  The catalogos
        /// </summary>
        //private CatCdfi catalogos = null;


        #region Variables Opera                   
        #region strings Opera


        /// <summary>
        /// The empresa tipo
        /// </summary>
        private static string _empresaTipo = "";

        /// <summary>
        /// The trama
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> _trama;

        /// <summary>
        /// The detalles
        /// </summary>
        private static List<Dictionary<string, string>> _detallesOp;

        /// <summary>
        /// The log cadena
        /// </summary>
        private readonly string _logCadena = "";

        /// <summary>
        /// The clase document
        /// </summary>
        private string _claseDoc = "";

        /// <summary>
        /// The numaprob
        /// </summary>
        private string _numaprob = "";

        /// <summary>
        /// The anoaprob
        /// </summary>
        private string _anoaprob = "";

        /// <summary>
        /// The serie
        /// </summary>
        private string _serie = "";

        /// <summary>
        /// The folio
        /// </summary>
        private string _folio = "";

        /// <summary>
        /// The folioint
        /// </summary>
        private string _folioint = "";

        /// <summary>
        /// The fechaemision
        /// </summary>
        private string _fechaemision = "";

        /// <summary>
        /// The formapago
        /// </summary>
        private string _formapago = "";

        /// <summary>
        /// The condpago
        /// </summary>
        private string _condpago = "";

        /// <summary>
        /// The ordencompra
        /// </summary>
        private string _ordencompra = "";

        /// <summary>
        /// The fechaoc
        /// </summary>
        private string _fechaoc = "";

        /// <summary>
        /// The contrarecibo
        /// </summary>
        private string _contrarecibo = "";

        /// <summary>
        /// The fechacontra
        /// </summary>
        private string _fechacontra = "";

        /// <summary>
        /// The termpagodias
        /// </summary>
        private string _termpagodias = "";

        /// <summary>
        /// The fechavenc
        /// </summary>
        private string _fechavenc = "";

        /// <summary>
        /// The RFC
        /// </summary>
        private string _rfc = "";

        /// <summary>
        /// The nombre
        /// </summary>
        private string _nombre = "";

        /// <summary>
        /// The numeroprov
        /// </summary>
        private string _numeroprov = "";

        /// <summary>
        /// The GLN
        /// </summary>
        private string _gln = "";

        /// <summary>
        /// The calle
        /// </summary>
        private string _calle = "";

        /// <summary>
        /// The noexterior
        /// </summary>
        private string _noexterior = "";

        /// <summary>
        /// The nointerior
        /// </summary>
        private string _nointerior = "";

        /// <summary>
        /// The colonia
        /// </summary>
        private string _colonia = "";

        /// <summary>
        /// The localidad
        /// </summary>
        private string _localidad = "";

        /// <summary>
        /// The municipio
        /// </summary>
        private string _municipio = "";

        /// <summary>
        /// The estado
        /// </summary>
        private string _estado = "";

        /// <summary>
        /// The pais
        /// </summary>
        private string _pais = "";

        /// <summary>
        /// The codigopostal
        /// </summary>
        private string _codigopostal = "";

        /// <summary>
        /// The sucursalex
        /// </summary>
        private string _sucursalex = "";

        /// <summary>
        /// The glnex
        /// </summary>
        private string _glnex = "";

        /// <summary>
        /// The calleex
        /// </summary>
        private string _calleex = "";

        /// <summary>
        /// The noexteriorex
        /// </summary>
        private string _noexteriorex = "";

        /// <summary>
        /// The nointeriorex
        /// </summary>
        private string _nointeriorex = "";

        /// <summary>
        /// The coloniaex
        /// </summary>
        private string _coloniaex = "";

        /// <summary>
        /// The localidadex
        /// </summary>
        private string _localidadex = "";

        /// <summary>
        /// The municipioex
        /// </summary>
        private string _municipioex = "";

        /// <summary>
        /// The estadoex
        /// </summary>
        private string _estadoex = "";

        /// <summary>
        /// The paisex
        /// </summary>
        private string _paisex = "";

        /// <summary>
        /// The codigopostalex
        /// </summary>
        private string _codigopostalex = "";

        /// <summary>
        /// The lugarexpedicion
        /// </summary>
        private string _lugarexpedicion = "";

        /// <summary>
        /// The rfcre
        /// </summary>
        private string _rfcre = "";

        /// <summary>
        /// The nombrere
        /// </summary>
        private string _nombrere = "";

        /// <summary>
        /// The numerocliente
        /// </summary>
        private string _numerocliente = "";

        /// <summary>
        /// The habitacion
        /// </summary>
        private string _habitacion = "";

        /// <summary>
        /// The checkin
        /// </summary>
        private string _checkin = "";

        /// <summary>
        /// The checkout
        /// </summary>
        private string _checkout = "";

        /// <summary>
        /// The cajero
        /// </summary>
        private string _cajero = "";

        /// <summary>
        /// The cuenta ar
        /// </summary>
        private string _cuentaAr = "";

        /// <summary>
        /// The email
        /// </summary>
        private string _email = "";

        /// <summary>
        /// The impresion
        /// </summary>
        private string _impresion = "";

        /// <summary>
        /// The RDFGLN
        /// </summary>
        private string _rdfgln = "";

        /// <summary>
        /// The rdfcalle
        /// </summary>
        private string _rdfcalle = "";

        /// <summary>
        /// The rdfnoexterior
        /// </summary>
        private string _rdfnoexterior = "";

        /// <summary>
        /// The rdfnointerior
        /// </summary>
        private string _rdfnointerior = "";

        /// <summary>
        /// The rdfcolonia
        /// </summary>
        private string _rdfcolonia = "";

        /// <summary>
        /// The rdflocalidad
        /// </summary>
        private string _rdflocalidad = "";

        /// <summary>
        /// The rdfreferencia
        /// </summary>
        private string _rdfreferencia = "";

        /// <summary>
        /// The rdfmunicipio
        /// </summary>
        private string _rdfmunicipio = "";

        /// <summary>
        /// The rdfestado
        /// </summary>
        private string _rdfestado = "";

        /// <summary>
        /// The rdfpais
        /// </summary>
        private string _rdfpais = "";

        /// <summary>
        /// The rdfcodigopostal
        /// </summary>
        private string _rdfcodigopostal = "";


        /// <summary>
        /// The sucursalre
        /// </summary>
        private string _sucursalre = "";

        /// <summary>
        /// The cdgsucursal
        /// </summary>
        private string _cdgsucursal = "";

        /// <summary>
        /// The regln
        /// </summary>
        private string _regln = "";

        /// <summary>
        /// The callere
        /// </summary>
        private string _callere = "";

        /// <summary>
        /// The noexteriorre
        /// </summary>
        private string _noexteriorre = "";

        /// <summary>
        /// The nointeriorre
        /// </summary>
        private string _nointeriorre = "";

        /// <summary>
        /// The coloniare
        /// </summary>
        private string _coloniare = "";

        /// <summary>
        /// The localidadre
        /// </summary>
        private string _localidadre = "";

        /// <summary>
        /// The referencia
        /// </summary>
        private string _referencia = "";

        /// <summary>
        /// The municipiore
        /// </summary>
        private string _municipiore = "";

        /// <summary>
        /// The estadore
        /// </summary>
        private string _estadore = "";

        /// <summary>
        /// The paisre
        /// </summary>
        private string _paisre = "";

        /// <summary>
        /// The codigopostalre
        /// </summary>
        private string _codigopostalre = "";


        /// <summary>
        /// The arrayconceptos
        /// </summary>
        private string[] _arrayconceptos;

        /// <summary>
        /// The array list d
        /// </summary>
        private ArrayList _arrayListD;

        /// <summary>
        /// The moneda
        /// </summary>
        private string _moneda = "";

        /// <summary>
        /// The tipocambio
        /// </summary>
        private string _tipocambio = "";

        /// <summary>
        /// The subtotal
        /// </summary>
        private string _subtotal = "";

        /// <summary>
        /// The tasadesc
        /// </summary>
        private string _tasadesc = "";

        /// <summary>
        /// The totaldesc
        /// </summary>
        private string _totaldesc = "";

        /// <summary>
        /// The montobase
        /// </summary>
        private string _montobase = "";

        /// <summary>
        /// The tipodeimp
        /// </summary>
        private string _tipodeimp = "";

        /// <summary>
        /// The tasaiva
        /// </summary>
        private string _tasaiva = "";

        /// <summary>
        /// The montoiva
        /// </summary>
        private string _montoiva = "";

        /// <summary>
        /// The tipoimpuesto1
        /// </summary>
        private string _tipoimpuesto1 = "";

        /// <summary>
        /// The tasaimpuesto1
        /// </summary>
        private string _tasaimpuesto1 = "";

        /// <summary>
        /// The montoimpuesto1
        /// </summary>
        private string _montoimpuesto1 = "";

        /// <summary>
        /// The tipoimpuesto2
        /// </summary>
        private string _tipoimpuesto2 = "";

        /// <summary>
        /// The tasaimpuesto2
        /// </summary>
        private string _tasaimpuesto2 = "";

        /// <summary>
        /// The montoimpuesto2
        /// </summary>
        private string _montoimpuesto2 = "";

        /// <summary>
        /// The tipoimpuesto3
        /// </summary>
        private string _tipoimpuesto3 = "";

        /// <summary>
        /// The tasaimpuesto3
        /// </summary>
        private string _tasaimpuesto3 = "";

        /// <summary>
        /// The montoimpuesto3
        /// </summary>
        private string _montoimpuesto3 = "";

        /// <summary>
        /// The montoimpuestos
        /// </summary>
        private string _montoimpuestos = "";

        /// <summary>
        /// The tiporet
        /// </summary>
        private string _tiporet = "";

        /// <summary>
        /// The tasaret
        /// </summary>
        private string _tasaret = "";

        /// <summary>
        /// The montoret
        /// </summary>
        private string _montoret = "";

        /// <summary>
        /// The tiporetd
        /// </summary>
        private string _tiporetd = "";

        /// <summary>
        /// The tasaretd
        /// </summary>
        private string _tasaretd = "";

        /// <summary>
        /// The montoretd
        /// </summary>
        private string _montoretd = "";

        /// <summary>
        /// The totalret
        /// </summary>
        private string _totalret = "";

        /// <summary>
        /// The total reference
        /// </summary>
        private string _totalRef = "";

        /// <summary>
        /// The totalletra reference
        /// </summary>
        private string _totalletraRef = "";

        /// <summary>
        /// The VLR pagar
        /// </summary>
        private string _vlrPagar = "";

        /// <summary>
        /// The totalletra
        /// </summary>
        private string _totalletra = "";

        /// <summary>
        /// The propinas
        /// </summary>
        private string _propinas = "";

        /// <summary>
        /// The paidouts
        /// </summary>
        private string _paidouts = "";

        /// <summary>
        /// The otros
        /// </summary>
        private string _otros = "";

        /// <summary>
        /// The tipopago1
        /// </summary>
        private string _tipopago1 = "";

        /// <summary>
        /// The pago1
        /// </summary>
        private string _pago1 = "";

        /// <summary>
        /// The tipopago2
        /// </summary>
        private string _tipopago2 = "";

        /// <summary>
        /// The pago2
        /// </summary>
        private string _pago2 = "";

        /// <summary>
        /// The tipopago3
        /// </summary>
        private string _tipopago3 = "";

        /// <summary>
        /// The pago3
        /// </summary>
        private string _pago3 = "";

        /// <summary>
        /// The tipopago4
        /// </summary>
        private string _tipopago4 = "";

        /// <summary>
        /// The pago4
        /// </summary>
        private string _pago4 = "";

        /// <summary>
        /// The metodopago
        /// </summary>
        private string _metodopago = "";

        /// <summary>
        /// The numerocuentapago
        /// </summary>
        private string _numerocuentapago = "";

        /// <summary>
        /// The regimenfiscal
        /// </summary>
        private string _regimenfiscal = "";

        /// <summary>
        /// The nom huesped
        /// </summary>
        private string _nomHuesped = "";

        /// <summary>
        /// The pasaporte
        /// </summary>
        private string _pasaporte = "";

        /// <summary>
        /// The bandera
        /// </summary>
        private bool _bandera;

        private string _mmsg;

        #endregion

        #region email

        /// <summary>
        /// The servidor
        /// </summary>
        private string _servidor = "";

        /// <summary>
        /// The puerto
        /// </summary>
        private int _puerto;

        /// <summary>
        /// The SSL
        /// </summary>
        private bool _ssl;

        /// <summary>
        /// The email credencial
        /// </summary>
        private string _emailCredencial = "";

        /// <summary>
        /// The pass credencial
        /// </summary>
        private string _passCredencial = "";

        /// <summary>
        /// The mensaje
        /// </summary>
        private string _mensaje;

        /// <summary>
        /// The asunto
        /// </summary>
        private string _asunto;

        /// <summary>
        /// The em
        /// </summary>
        private SendMail _em = new SendMail();

        /// <summary>
        /// The email enviar
        /// </summary>
        private string _emailEnviar = "";

        /// <summary>
        /// The email notificacion
        /// </summary>
        private string _emailNotificacion = "";

        /// <summary>
        /// The email opera
        /// </summary>
        private string _emailOpera = "";

        private string _numeroreserva = "";
        private string _numerovoucher = "";
        private string _noadultos = "";
        private string _noninos = "";
        private string _subtotalsindescuento = "";
        private string _anticipos = "";
        private string _subtotal1 = "";
        private string _propinas2 = "";
        private string _tipoimpuesto4 = "";
        private string _tasaimpuesto4 = "";
        private string _montoimpuesto4 = "";
        private string _tipoimpuesto5 = "";
        private string _tasaimpuesto5 = "";
        private string _montoimpuesto5 = "";
        private string _tipoDocto = "";
        private string _division = "";
        private string _noProveedor = "";
        private string _correoSolicitante = "";
        private string _nombreSolicitante = "";

        #endregion

        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var sessionRfc = Session["IDENTEMIEXT"];
            var rfc = (sessionRfc == null ? GetFirstRfcFromCore() : sessionRfc.ToString());
            Session["IDENTEMIEXT"] = rfc;
            _db = new BasesDatos(rfc);
            _log = new Log(rfc);
            if (!IsPostBack)
            {
                // Generate a new PageiD //
                ViewState["_PageID"] = (new Random()).Next().ToString();
                if (rfc.Equals("OAP981214DP3") || rfc.Equals("HSC0010193M7") || rfc.Equals("OHF921110BF2") || rfc.Equals("OHS0312152Z4"))
                {
                    tbTicket.Attributes["title"] = "<div align='left'>Información aceptada:<br/><ul><li>{NoHabitacion+Entrada+Salida} (sin espacios ni guiones)</li></ul></div>";
                }
                else if (Regex.IsMatch(Session["IDENTEMIEXT"].ToString(), @"RMM120815858|MAN100706R93|MAB081110C73|KAI0305307N8|MDU100621V6A|MAN081113157|GAR020131PP9|MMO040331J1A|SSU041119694|OPT970210N72"))
                {
                    tbTicket.Attributes.Remove("data-toggle");
                    tbTicket.Attributes.Remove("data-placement");
                    tbTicket.Attributes.Remove("title");
                }
                else
                {
                    tbTicket.Attributes["title"] = "<div align='left'>Información aceptada:<br/><ul><li>{Reserva}</li><li>{Ticket}</li><li>{Cheque}</li><li>{Serie+Folio}</li><li>{NoHabitacion+Entrada+Salida} (sin espacios ni guiones)</li></ul></div>";
                }
                Session[ViewState["_PageID"].ToString() + "_trama"] = "";
                Session[ViewState["_PageID"].ToString() + "_arrayTrama"] = null;
                Session[ViewState["_PageID"].ToString() + "_idTrama"] = "";
                //tbNumCtaPago.ReadOnly = true;
                tbNumCtaPago.Text = "NO IDENTIFICADO";
                LoadLogo(rfc);
                var empresaTipo = Empresatipo(rfc);
                if (empresaTipo.Equals("1") && rfc.Equals("OHR980618BLA")) { empresaTipo = "2"; }
                switch (empresaTipo)
                {
                    case "1":
                        // HOTELES
                        bGenerar1.Visible = true;
                        bConsultar.Visible = true;
                        bGenerarConsultar.Visible = false;
                        break;
                    case "2":
                        // RESTAURANTES
                        bGenerar1.Visible = false;
                        bConsultar.Visible = false;
                        bGenerarConsultar.Visible = true;
                        break;
                    case "3":
                        // EMPRESAS
                        break;
                    case "4":
                        // PAQUETERIA
                        break;
                    default:
                        break;
                }
                var parametros = new Parametros(Session["IDENTEMIEXT"].ToString());
                try
                {
                    Session["CfdiVersion"] = parametros.GetParametroByNombre("VersionCfdi").Status;
                }
                catch { Session["CfdiVersion"] = "3.2"; }
                try
                {
                    var conectoresBase = parametros.GetParametroByNombre("TiposConecores").Status;
                    var arrayConcetores = conectoresBase.Split('|');
                    Session["Conectores"] = arrayConcetores;
                }
                catch (Exception ex) { }
                /*if (Session["CfdiVersion"].ToString().Equals("3.3") && Session["CatalogosCfdi33"] == null)
                {
                    CatCdfi catalogos = null;
                    try
                    {
                        catalogos = new CatCdfi();
                        var bytes = Properties.Resources.catCFDI;
                        catalogos.LoadFromBytes(bytes);
                        //ThreadStart pre = new ThreadStart(CrearCatalogo);
                        //Thread c = new Thread(pre);
                        //c.IsBackground = true;
                        //c.Start();
                    }
                    catch (Exception ex)
                    {
                        // ignored
                        catalogos = null;
                    }
                    finally
                    {
                        Session["CatalogosCfdi33"] = catalogos;
                    }
                }*/
                DivUsoCfdi.Style["display"] = Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                tbPaisRec.Visible = !Session["CfdiVersion"].ToString().Equals("3.3");
                ddlPais.Visible = Session["CfdiVersion"].ToString().Equals("3.3");
                RequiredFieldValidator_ddlPais.Enabled = Session["CfdiVersion"].ToString().Equals("3.3");
                RequiredFieldValidator_UsoCfdi.Enabled = Session["CfdiVersion"].ToString().Equals("3.3");
                FillCatalogPaises();
                var conectores = new List<string>();
                if (Session["Conectores"] != null)
                {
                    conectores = ((string[])Session["Conectores"]).ToList();
                }

                if (conectores.Any(c => Regex.IsMatch(c, "maitred", RegexOptions.IgnoreCase)))
                {
                    imgPaso1.ImageUrl = "~/imagenes/tutorial-tickets/maitred/paso1.jpg";
                    imgPaso2.ImageUrl = "~/imagenes/tutorial-tickets/maitred/paso2.jpg";
                    imgPaso3.ImageUrl = "~/imagenes/tutorial-tickets/maitred/paso3.jpg";
                    imgPaso4.ImageUrl = "~/imagenes/tutorial-tickets/maitred/paso4.jpg";
                }
                else if (conectores.Any(c => Regex.IsMatch(c, "tca|in+s+is+t", RegexOptions.IgnoreCase)))
                {
                    imgPaso1.ImageUrl = "~/imagenes/tutorial-tickets/tca/paso1.jpg";
                    imgPaso2.ImageUrl = "~/imagenes/tutorial-tickets/tca/paso2.jpg";
                    imgPaso3.ImageUrl = "~/imagenes/tutorial-tickets/tca/paso3.jpg";
                    imgPaso4.ImageUrl = "~/imagenes/tutorial-tickets/tca/paso4.jpg";
                }
                else if (conectores.Any(c => Regex.IsMatch(c, "smh", RegexOptions.IgnoreCase)))
                {
                    imgPaso1.ImageUrl = "~/imagenes/tutorial-tickets/smh/paso1.jpg";
                    imgPaso2.ImageUrl = "~/imagenes/tutorial-tickets/smh/paso2.jpg";
                    imgPaso3.ImageUrl = "~/imagenes/tutorial-tickets/smh/paso3.jpg";
                    imgPaso4.ImageUrl = "~/imagenes/tutorial-tickets/smh/paso4.jpg";
                }
                else
                {
                    imgPaso1.ImageUrl = "~/imagenes/tutorial-tickets/micros/paso1.jpg";
                    imgPaso2.ImageUrl = "~/imagenes/tutorial-tickets/micros/paso2.jpg";
                    imgPaso3.ImageUrl = "~/imagenes/tutorial-tickets/micros/paso3.jpg";
                    imgPaso4.ImageUrl = "~/imagenes/tutorial-tickets/micros/paso4.jpg";
                }
            }
            else
            {
                if (Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] != null)
                {
                    gvConceptos.DataSource = (DataTable)Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"];
                }
            }
            ddlMetodoPago.CssClass = "form-control" + (Session["CfdiVersion"].ToString().Equals("3.3") ? "" : " bootstrap-select-multiple");
            //CssClass="form-control bootstrap-select-multiple"
            SqlDataMetodoPago.ConnectionString = _db.CadenaConexion;
            SqlDataSourceSucursalesEmisor.ConnectionString = _db.CadenaConexion;
            SqlDataSourceUsoCfdi.ConnectionString = _db.CadenaConexion;
            SqlDataMetodoPago.SelectCommand = SqlDataMetodoPago.SelectCommand.Replace("@tipoCatalogo", Session["CfdiVersion"].ToString().Equals("3.3") ? "MetodoPagoCfdi33" : "MetodoPago");
            if (!IsPostBack)
            {
                ddlSucursalEmisor.DataBind();
            }
            divSucursal.Style["display"] = (ddlSucursalEmisor.Items.Count > 1) ? "inline" : "none";
            if (Regex.IsMatch(Session["IDENTEMIEXT"].ToString(), @"RMM120815858|MAN100706R93|MAB081110C73|KAI0305307N8|MDU100621V6A|MAN081113157|GAR020131PP9|MMO040331J1A|SSU041119694|OPT970210N72"))
            {
                #region CSS Moshi Moshi
                var pngMoshi = "data:image/png;base64,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";
                var pngMoshiNew = "data:image/png;base64,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";
                Page.Header.Controls.Add(
    new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Open+Sans\" />"));
                Page.Header.Controls.Add(
    new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Raleway\" />"));
                Page.Header.Controls.Add(
    new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" href=\"" + ResolveUrl("~/css/essentials.css") + "\" />"));
                Page.Header.Controls.Add(
    new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" href=\"" + ResolveUrl("~/css/yellow.css") + "\" />"));
                ScriptManager.RegisterStartupScript(this, GetType(), "cssMoshiMoshi", @"
$('.btn-primary').addClass('btn-warning');
$('.btn-primary').removeClass('btn-primary');
$('#bodyContainer').addClass('well');
$('#bodyContainer').attr('style', 'background: rgba(255,255,255,.7) !important');
$('#spanTitulo').html('<img alt=\""\"" src=\""" + pngMoshiNew + @"\""></img><br/>Facturación en línea');
$('#spanTitulo').css('font-size', '25px');
$('#cpCuerpoSup_Label1').css('display', 'none');
$('#divSucursalText').html('Selecciona la ubicación de tu consumo');
$('#divReferenciaText').html('Ingresa el número de referencia de tu ticket');
/*$('#cpCuerpoSup_hlRegresar').html('moshi.mx');*/
/*$('#cpCuerpoSup_hlRegresar').attr('href', 'http://www.moshimoshi.com.mx');*/
$('.form-control').addClass('moshi');
$('.btn').addClass('moshi');
$('#navBarMenu').css('background-color', '#000');
$('#navBarMenu').css('background-image', 'linear-gradient(to bottom, #000 0%, #000 100%)');
$('#imgLogo').removeClass('img-rounded');
$('#imgLogo').attr('src', '" + pngMoshi + @"');
$('#imgLogo').removeAttr('style');
$('#footer-extended').attr('style', 'display: table; height: 50px; background: black; width: 100%; position: fixed; bottom: 0; left: 0');
$('#footer-extended').html('<div class=\""row\"" style=\""display: table-cell; vertical-align: middle;\""><div class=\""col-md-3\""><img alt=\""\"" src=\""" + pngMoshi + @"\""></img></div><div class=\""col-md-6\""><a href=\""https://www.moshimoshi.mx/Contacto/\"">Contacto</a> | <a href=\""https://www.moshimoshi.mx/Trabaja\"">Talento</a> | <a href=\""#\"" data-toggle=\""modal\"" id=\""terminos\"" data-target=\""#mdtyc\"">Términos y condiciones</a> | <a href=\""#\"" data-toggle=\""modal\"" id=\""aviso\"" data-target=\""#mdfa\"">Aviso de privacidad</a></div><div class=\""col-md-3\""><a href=\""https://www.facebook.com/moshimoshimx\""><i class=\""fa fa-facebook-official fa-2x\"" aria-hidden=\""true\""></i></a>&nbsp;&nbsp;<a href=\""https://twitter.com/moshimoshimx\""><i class=\""fa fa-twitter fa-2x\"" aria-hidden=\""true\""></i></a>&nbsp;&nbsp;<a href=\""https://www.instagram.com/moshimoshimx/\""><i class=\""fa fa-instagram fa-2x\"" aria-hidden=\""true\""></i></a></div></div>');
$('#footer-extended a').attr('style', 'color: gray');
$('.gvHeader').css('background-color', 'rgb(255,175,69)');
$('#bodyMaster').attr('style','');
$('body').css('font-family','Open Sans,Arial,Helvetica,sans-serif');
$('#footer').css('display','none');
$('#UpdatePanelMenuMaster').prepend('<span style= \""display: inline-block; float: right; color: white; vertical-align: middle; \""></span>');
", true);
                #endregion
            }
            var sucursalEx = Request.QueryString.Get("sucursalEx");
            if (!string.IsNullOrEmpty(sucursalEx))
            {
                var value = "";
                ListItem dest;
                try
                {
                    dest = ddlSucursalEmisor.Items.FindByValue(sucursalEx);
                    if (dest != null) { value = dest.Value; }
                }
                catch { }
                if (string.IsNullOrEmpty(value))
                {
                    try
                    {
                        dest = ddlSucursalEmisor.Items.FindByText(sucursalEx);
                        if (dest != null) { value = dest.Value; }
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(value)) { ddlSucursalEmisor.SelectedValue = value; }
            }
            /*void CrearCatalogo()
            {
                catalogos = new CatCdfi();
                var bytes = Properties.Resources.catCFDI;
                catalogos.LoadFromBytes(bytes);
                Session["CatalogosCfdi33"] = catalogos;
                //FillCatalogPaises();
            }*/
        }

        /// <summary>
        /// Gets the first RFC from core.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetFirstRfcFromCore()
        {
            var rfc = "LAN7008173R5";
            try
            {
                var db = new BasesDatos("CORE");
                db.Conectar();
                db.CrearComando("SELECT TOP 1 e.RFCEMI FROM Cat_Emisor e INNER JOIN Par_ParametrosEmisor p ON p.idEmisor = e.IDEEMI ORDER BY e.IDEEMI");
                var dr = db.EjecutarConsulta();
                if (dr.Read())
                {
                    rfc = dr["RFCEMI"].ToString();
                }
                db.Desconectar();
            }
            catch { }
            return rfc;
        }

        /// <summary>
        /// Loads the logo.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        private void LoadLogo(string rfc)
        {
            try
            {
                _db.Conectar();
                _db.CrearComando(@"SELECT logo FROM Cat_Emisor WHERE RFCEMI = @RFC");
                _db.AsignarParametroCadena("@RFC", rfc);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    var bytes = (byte[])dr[0];
                    var base64 = "data:image/png;base64," + Convert.ToBase64String(bytes, Base64FormattingOptions.None);
                    var image = ((Image)((Master as SiteMaster).FindControl("imgLogo")));
                    image.Attributes["src"] = base64;

                    var colorThief = new ColorThief();
                    var color = colorThief.GetColor(ControlUtilities.GetBitmapFromByteArray(bytes)).Color;
                    Session["colorFondo"] = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                }
                _db.Desconectar();
            }
            catch { }
        }

        /// <summary>
        /// bs the moddd.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void BModdd(object sender, EventArgs e)
        {
            Session[ViewState["_PageID"].ToString() + "_trama"] = "";
            Session[ViewState["_PageID"].ToString() + "_arrayTrama"] = null;
            Session[ViewState["_PageID"].ToString() + "_idTrama"] = "";
            if (!string.IsNullOrEmpty(tbTicket.Text))
            {
                if (!Session["IDENTEMIEXT"].ToString().Equals("LAN7008173R5"))
                {
                    bConsultar_Click(null, null);
                }
                else
                {
                    var sucEmi = ddlSucursalEmisor.SelectedValue;
                    var idFact = ConsultarIdefac(tbTicket.Text, sucEmi);
                    if (!string.IsNullOrEmpty(idFact))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "_btnPopUp", "$('#PanelModal').modal();", true);
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Comprobante inexistente", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El campo no puede estar vacio", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            }
        }

        /// <summary>
        /// Handles the Click event of the bConsultar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bConsultar_Click(object sender, EventArgs e)
        {
            if (sender == null && e == null)
            {
                var sucEmi = ddlSucursalEmisor.SelectedValue;
                var idFact = ConsultarIdefac(tbTicket.Text, sucEmi);
                if (!string.IsNullOrEmpty(idFact))
                {
                    ConsultarArchivos(idFact);
                    ScriptManager.RegisterStartupScript(this, GetType(), "_keyFacturaConsultar", "$('#ModuloFactura').modal('show');", true);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "No se encontró el comprobante, favor de verificar la generación del mismo vía correo electrónico", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(tmonto.Text) || string.IsNullOrEmpty(tfecha.Text) || string.IsNullOrEmpty(trfc.Text))
                    (Master as SiteMaster).MostrarAlerta(this, "Los campos no pueden estar vacios", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                else
                {
                    var sucEmi = ddlSucursalEmisor.SelectedValue;
                    var monto = tmonto.Text;
                    var fechaE = tfecha.Text;
                    var rfcR = trfc.Text;
                    var idFact = ConsultarIdefac(tbTicket.Text, sucEmi, fechaE, rfcR, Convert.ToDouble(monto));
                    if (!string.IsNullOrEmpty(idFact))
                    {
                        ConsultarArchivos(idFact);
                        ScriptManager.RegisterStartupScript(this, GetType(), "_keyFacturaConsultar", "$('#ModuloFactura').modal('show');", true);
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Comprobante inexistente, por favor verifique la información ingresada.", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
            }
            trfc.Text = ""; tmonto.Text = ""; tfecha.Text = ""; tbTicket.Text = "";
        }


        /// <summary>
        /// Consultars the archivos.
        /// </summary>
        /// <param name="idefact">The idefact.</param>
        private void ConsultarArchivos(string idefact)
        {
            _db.Conectar();
            _db.CrearComando(@"SELECT g.numeroAutorizacion AS UUID, a.XMLARC, a.PDFARC, r.email, g.fecha, r.RFCREC, g.serie, g.folio FROM Dat_General g LEFT OUTER JOIN Dat_Archivos a ON g.idComprobante = a.IDEFAC INNER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor where g.idComprobante=@idefac");
            _db.AsignarParametroCadena("@idefac", idefact);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                var xml = dr["XMLARC"].ToString();
                var pdf = dr["PDFARC"].ToString();
                var uuid = dr["UUID"].ToString();
                var fecha = dr["fecha"].ToString();
                var RFCREC = dr["RFCREC"].ToString();
                var serie = dr["serie"].ToString();
                var folio = dr["folio"].ToString();
                var fechaFile = DateTime.Parse(fecha).ToString(@"yyyy\\MM\\dd");
                if (string.IsNullOrEmpty(xml))
                {
                    xml = $@"docus\\{fechaFile}\XML\{RFCREC}_{folio}_{serie}_{uuid}.xml";
                }
                if (string.IsNullOrEmpty(pdf))
                {
                    pdf = $@"docus\\{fechaFile}\XML\{RFCREC}_{folio}_{serie}_{uuid}.pdf";
                }
                hlXml.NavigateUrl = "download.aspx?file=" + xml;
                btnDPDF.CommandArgument = idefact + ";" + uuid + ";" + pdf;
                tbEmail.Text = dr["email"].ToString();
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Empresatipoes the specified RFC.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <returns>System.String.</returns>
        private string Empresatipo(string rfc)
        {
            var tipo = "";
            try
            {
                _db.Conectar();
                _db.CrearComando(@"SELECT EmpresaTipo FROM Cat_Emisor WHERE RFCEMI = @RFC");
                _db.AsignarParametroCadena("@RFC", rfc);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tipo = dr["EmpresaTipo"].ToString();
                }
                _db.Desconectar();
            }
            catch { }
            return tipo;
        }

        /// <summary>
        /// Handles the Click event of the bLimpiar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bLimpiar_Click(object sender, EventArgs e)
        {
            tbTicket.Text = "";
            tfecha.Text = "";
            tmonto.Text = "";
            trfc.Text = "";
            hlXml.NavigateUrl = "#";
            btnDPDF.CommandArgument = "";
        }

        /// <summary>
        /// Consultars the idefac.
        /// </summary>
        /// <param name="codDontrol">The cod dontrol.</param>
        /// <param name="claveSucursal">The clave sucursal.</param>
        /// <param name="fechE">The fech e.</param>
        /// <param name="rfc">The RFC.</param>
        /// <param name="total">The total.</param>
        /// <returns>System.String.</returns>
        private string ConsultarIdefac(string codDontrol, string claveSucursal, string fechE = "", string rfc = "", double total = 0)
        {
            string idFact = null;
            DbDataReader dr;
            _db.Conectar();
            if (ddlSucursalEmisor.Items.Count > 1)
            {
                _db.CrearComando(@"select TOP 1 g.idComprobante from Dat_General g LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor
                                where   g.folioReservacion=@codDontrol and g.tipo+g.estado='E1' and g.estab=@claveSucursal " + ((!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0) ? (@"and CAST(g.fecha AS Date)=@fechE
                                and g.total = @total
                                and g.id_Receptor = r.IDEREC and r.RFCREC = @rfc") : "") + @" ORDER BY g.idComprobante DESC");
                _db.AsignarParametroCadena("@claveSucursal", claveSucursal);
            }
            else
            {
                _db.CrearComando(@"select TOP 1 g.idComprobante from Dat_General g LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor
                            where  g.folioReservacion=@codDontrol and g.tipo+g.estado='E1' " + ((!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0) ? (@"and CAST(g.fecha AS Date)=@fechE
                                and g.total = @total
                                and g.id_Receptor = r.IDEREC and r.RFCREC = @rfc") : "") + @" ORDER BY g.idComprobante DESC");
            }
            if (!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0)
            {
                _db.AsignarParametroCadena("@fechE", fechE);
                _db.AsignarParametroCadena("@rfc", rfc);
                _db.AsignarParametroCadena("@total", total.ToString());
            }
            _db.AsignarParametroCadena("@codDontrol", codDontrol);

            dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                idFact = dr[0].ToString();
            }
            _db.Desconectar();
            if (string.IsNullOrEmpty(idFact))
            {
                _db.Conectar();
                if (ddlSucursalEmisor.Items.Count > 1)
                {
                    _db.CrearComando(@"select TOP 1 g.idComprobante from Dat_General g LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor
                                where   g.serie+CAST(g.folio AS varchar)=@codDontrol and g.tipo+g.estado='E1' and g.estab=@claveSucursal " + ((!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0) ? (@"and CAST(g.fecha AS Date)=@fechE
                                and g.total = @total
                                and g.id_Receptor = r.IDEREC and r.RFCREC = @rfc") : "") + @" ORDER BY g.idComprobante DESC");
                    _db.AsignarParametroCadena("@claveSucursal", claveSucursal);
                }
                else
                {
                    _db.CrearComando(@"select TOP 1 g.idComprobante from Dat_General g LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor
                            where   g.serie+CAST(g.folio AS varchar)=@codDontrol and g.tipo+g.estado='E1' " + ((!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0) ? (@"and CAST(g.fecha AS Date)=@fechE
                                and g.total = @total
                                and g.id_Receptor = r.IDEREC and r.RFCREC = @rfc") : "") + @" ORDER BY g.idComprobante DESC");
                }
                if (!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0)
                {
                    _db.AsignarParametroCadena("@fechE", fechE);
                    _db.AsignarParametroCadena("@rfc", rfc);
                    _db.AsignarParametroCadena("@total", total.ToString());
                }
                _db.AsignarParametroCadena("@codDontrol", codDontrol);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    idFact = dr[0].ToString();
                }
                _db.Desconectar();
            }
            if (string.IsNullOrEmpty(idFact))
            {
                _db.Conectar();
                if (ddlSucursalEmisor.Items.Count > 1)
                {
                    _db.CrearComando(@"select TOP 1 g.idComprobante from Dat_General g LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor
                                where   g.referencia=@codDontrol and g.tipo+g.estado='E1' and g.estab=@claveSucursal " + ((!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0) ? (@"and CAST(g.fecha AS Date)=@fechE
                                and g.total = @total
                                and g.id_Receptor = r.IDEREC and r.RFCREC = @rfc") : "") + @" ORDER BY g.idComprobante DESC");
                    _db.AsignarParametroCadena("@claveSucursal", claveSucursal);
                }
                else
                {
                    _db.CrearComando(@"select TOP 1 g.idComprobante from Dat_General g LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor
                            where   g.referencia=@codDontrol and g.tipo+g.estado='E1' " + ((!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0) ? (@"and CAST(g.fecha AS Date)=@fechE
                                and g.total = @total
                                and g.id_Receptor = r.IDEREC and r.RFCREC = @rfc") : "") + @" ORDER BY g.idComprobante DESC");
                }
                if (!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0)
                {
                    _db.AsignarParametroCadena("@fechE", fechE);
                    _db.AsignarParametroCadena("@rfc", rfc);
                    _db.AsignarParametroCadena("@total", total.ToString());
                }
                _db.AsignarParametroCadena("@codDontrol", codDontrol);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    idFact = dr[0].ToString();
                }
                _db.Desconectar();
            }
            if (string.IsNullOrEmpty(idFact))
            {
                _db.Conectar();
                if (ddlSucursalEmisor.Items.Count > 1)
                {
                    _db.CrearComando(@"select TOP 1 g.idComprobante from Dat_General g LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor
                                where   g.noTicket=@codDontrol and g.tipo+g.estado='E1' and g.estab=@claveSucursal " + ((!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0) ? (@"and CAST(g.fecha AS Date)=@fechE
                                and g.total = @total
                                and g.id_Receptor = r.IDEREC and r.RFCREC = @rfc") : "") + @" ORDER BY g.idComprobante DESC");
                    _db.AsignarParametroCadena("@claveSucursal", claveSucursal);
                }
                else
                {
                    _db.CrearComando(@"select TOP 1 g.idComprobante from Dat_General g LEFT OUTER JOIN Cat_Receptor r ON r.IDEREC = g.id_Receptor
                            where   g.noTicket=@codDontrol and g.tipo+g.estado='E1' " + ((!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0) ? (@"and CAST(g.fecha AS Date)=@fechE
                                and g.total = @total
                                and g.id_Receptor = r.IDEREC and r.RFCREC = @rfc") : "") + @" ORDER BY g.idComprobante DESC");
                }
                if (!string.IsNullOrEmpty(fechE) && !string.IsNullOrEmpty(rfc) && total > 0)
                {
                    _db.AsignarParametroCadena("@fechE", fechE);
                    _db.AsignarParametroCadena("@rfc", rfc);
                    _db.AsignarParametroCadena("@total", total.ToString());
                }
                _db.AsignarParametroCadena("@codDontrol", codDontrol);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    idFact = dr[0].ToString();
                }
                _db.Desconectar();
            }
            return idFact;
        }

        /// <summary>
        /// Consultars the idefac.
        /// </summary>
        /// <param name="codDontrol">The cod dontrol.</param>
        /// <returns>System.String.</returns>
        private string[] ConsultarTrama(string codDontrol)
        {
            string trama = "";
            DbDataReader dr;
            _db.Conectar();
            _db.CrearComando(@"select TOP 1 idTrama, Trama, Fecha from Log_Trama
                            where   serie+CAST(folio AS varchar)=@codDontrol and tipo = '4' and observaciones = 'ExtranetOK' ORDER BY idTrama DESC");
            _db.AsignarParametroCadena("@codDontrol", codDontrol);
            dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                Session[ViewState["_PageID"].ToString() + "_idTrama"] = dr[0].ToString();
                trama = dr[1].ToString();
            }
            _db.Desconectar();
            if (string.IsNullOrEmpty(Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString()))
            {
                _db.Conectar();
                _db.CrearComando(@"select TOP 1 idTrama, Trama, Fecha from Log_Trama
                            where   noReserva=@codDontrol and tipo = '4' and observaciones = 'ExtranetOK' ORDER BY idTrama DESC");
                _db.AsignarParametroCadena("@codDontrol", codDontrol);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    Session[ViewState["_PageID"].ToString() + "_idTrama"] = dr[0].ToString();
                    trama = dr[1].ToString();
                }
                _db.Desconectar();
            }
            if (string.IsNullOrEmpty(Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString()))
            {
                _db.Conectar();
                _db.CrearComando(@"select TOP 1 idTrama, Trama, Fecha from Log_Trama
                            where   Secuencial=@codDontrol and tipo = '4' and observaciones = 'ExtranetOK' ORDER BY idTrama DESC");
                _db.AsignarParametroCadena("@codDontrol", codDontrol);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    Session[ViewState["_PageID"].ToString() + "_idTrama"] = dr[0].ToString();
                    trama = dr[1].ToString();
                }
                _db.Desconectar();
            }
            if (string.IsNullOrEmpty(Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString()))
            {
                _db.Conectar();
                _db.CrearComando(@"select TOP 1 idTrama, Trama, Fecha from Log_Trama
                            where   noTicket=@codDontrol and tipo = '4' and observaciones = 'ExtranetOK' ORDER BY idTrama DESC");
                _db.AsignarParametroCadena("@codDontrol", codDontrol);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    Session[ViewState["_PageID"].ToString() + "_idTrama"] = dr[0].ToString();
                    trama = dr[1].ToString();
                }
                _db.Desconectar();
            }
            return new string[] { Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString(), trama };
        }

        /// <summary>
        /// Handles the Click event of the btnDPDF control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnDPDF_Click(object sender, EventArgs e)
        {
            // Ponerle Commands
            var btn = (LinkButton)sender;
            var pdf = "";
            var rutaCodigoControl = "";
            var idComprobante = "";
            var rutaDocus = "";
            var rutaPdf = "";
            var uuid = "";
            _db.Conectar();
            _db.CrearComando(@"select dirdocs from Par_ParametrosSistema");

            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                rutaDocus = dr[0].ToString().Trim();
            }
            _db.Desconectar();

            rutaCodigoControl = btn.CommandArgument;
            var parametros = rutaCodigoControl.Split(';');
            idComprobante = parametros[0];
            uuid = parametros[1];
            pdf = parametros[2];
            pdf = pdf.Replace(@"docus\", "").Replace("docus//", "").Replace("docus/", "");
            rutaPdf = rutaDocus + pdf;
            rutaPdf = rutaPdf.Replace("/", @"\").Replace(@"\\", @"\").Replace("//", @"\");
            if (pdf != null && (File.Exists(rutaPdf)))
            {
                var urlRedirect = "download.aspx?file=" + parametros[2];
                Response.Redirect(urlRedirect);
            }
            else
            {
                var pageurl = ResolveUrl("~/descargarPDF.aspx?idFactura=" + idComprobante + "&uuid=" + uuid);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "loadPdfModal('" + pageurl + "&mode=view','" + pageurl + "&mode=download');", true);
            }
        }

        /// <summary>
        /// Handles the Click event of the bMail control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bMail_Click(object sender, EventArgs e)
        {
            string servidor = "";
            int puerto = 25;
            bool ssl = false;
            string emailCredencial = "";
            string passCredencial = "";
            string emailEnviar = "";
            string rutaDoc = "";
            SendMail em;
            DbDataReader dr;
            var parametros = btnDPDF.CommandArgument.Split(';');
            var idComprobante = parametros[0];

            if (!string.IsNullOrEmpty(idComprobante))
            {
                string uuid = "";
                string xml = "";
                string pdf = "";
                string folio = "";
                string fecha = "";
                string fechaAutorizacion = "";
                string serie = "";
                string codDoc = "";
                _db.Conectar();
                _db.CrearComando(@"SELECT numeroAutorizacion AS UUID, XMLARC, PDFARC, folio, fecha, fechaAutorizacion, serie, codDoc FROM Dat_General INNER JOIN Dat_Archivos ON idComprobante = IDEFAC where  idComprobante=@idefac");
                _db.AsignarParametroCadena("@idefac", idComprobante);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    uuid = dr["UUID"].ToString();
                    xml = dr["XMLARC"].ToString();
                    pdf = dr["PDFARC"].ToString();
                    folio = dr["folio"].ToString();
                    fecha = dr["fecha"].ToString();
                    serie = dr["serie"].ToString();
                    codDoc = dr["codDoc"].ToString();
                    fechaAutorizacion = dr["fechaAutorizacion"].ToString();
                }
                _db.Desconectar();
                _db.Conectar();
                _db.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio from Par_ParametrosSistema");
                var dr1 = _db.EjecutarConsulta();

                while (dr1.Read())
                {
                    servidor = dr1[0].ToString();
                    puerto = Convert.ToInt32(dr1[1].ToString());
                    ssl = Convert.ToBoolean(dr1[2].ToString());
                    emailCredencial = dr1[3].ToString();
                    passCredencial = dr1[4].ToString();
                    rutaDoc = dr1[5].ToString();
                    emailEnviar = dr1[6].ToString();
                }
                _db.Desconectar();

                var mensaje = "";

                var emails = "";
                emails = tbEmail.Text + "," + emails;
                emails = emails.Trim();
                emails = emails.Trim(',');

                if (emails.Length > 5)
                {
                    em = new SendMail();
                    em.ServidorSmtp(servidor, puerto, ssl, emailCredencial, passCredencial);
                    if (checkPDF.Checked)
                    {
                        var fileName = rutaDoc + pdf.Replace("docus/" + (Session["IDENTEMIEXT"] != null ? Session["IDENTEMIEXT"].ToString() : "") + "/", "").Replace("docus/", "");
                        if (File.Exists(fileName))
                        {
                            em.Adjuntar(fileName);
                        }
                        else
                        {
                            var ws = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
                            var respuesta = ws.GenerarPdf(Session["IDENTEMIEXT"].ToString(), idComprobante, "");
                            if (respuesta != null)
                            {
                                em.Adjuntar(respuesta, uuid + ".pdf");
                            }
                        }
                    }
                    if (checkXML.Checked)
                    {
                        var fileName = rutaDoc + hlXml.NavigateUrl.Split('=').Last().Replace(@"docus\" + (Session["IDENTEMIEXT"] != null ? Session["IDENTEMIEXT"].ToString() : "") + "/", "").Replace(@"docus\", "");
                        if (File.Exists(fileName))
                        {
                            em.Adjuntar(fileName);
                        }
                    }
                    var emir = "";
                    _db.Conectar();
                    _db.CrearComando(@"SELECT nombre,mensaje FROM Cat_Mensajes where nombre='MensajePortalWebEmision' ");
                    var drSum = _db.EjecutarConsulta();
                    if (drSum.Read())
                    {
                        mensaje = drSum["mensaje"].ToString();
                    }
                    _db.Desconectar();

                    _db.Conectar();
                    _db.CrearComando(@"SELECT NOMEMI FROM Cat_Emisor WHERE RFCEMI = @RFC");
                    _db.AsignarParametroCadena("@RFC", Session["IDENTEMIEXT"].ToString());
                    var dRemi = _db.EjecutarConsulta();
                    if (dRemi.Read())
                    {
                        emir = dRemi[0].ToString();
                    }
                    _db.Desconectar();

                    var asunto = "Documento electrónico No: " + folio + " de " + emir + "";

                    var tipoDoc = "Desconocido";
                    switch (codDoc)
                    {
                        case "01": tipoDoc = "Factura"; break;
                        case "04": tipoDoc = "Nota de Crédito"; break;
                        case "06": tipoDoc = "Carta Porte"; break;
                        case "07": tipoDoc = "Retención"; break;
                        case "08": tipoDoc = "Nómina"; break;
                        case "09": tipoDoc = "Contabilidad"; break;
                        default: break;
                    }
                    em.LlenarEmail(emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                    try
                    {
                        em.ReemplazarVariable("@TipoDocumento", tipoDoc);
                        em.ReemplazarVariable("@Serie", serie);
                        em.ReemplazarVariable("@Folio", folio);
                        em.ReemplazarVariable("@NumeroAutorizacion", uuid);
                        em.ReemplazarVariable("@FechaEmision", fecha);
                        em.ReemplazarVariable("@FechaAutorizacion", fechaAutorizacion);
                        em.EnviarEmail();
                        var sql = @"INSERT INTO Cat_emailEnvio
                                           (emailEnviado
                                           ,fechaEnvio
                                           ,id_General)
                                     VALUES
                                           (@emailEnviado
                                           ,@fechaEnvio
                                           ,@id_General)";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@emailEnviado", emails.Trim(','));
                        _db.AsignarParametroCadena("@fechaEnvio", Localization.Now.ToString("s"));
                        _db.AsignarParametroCadena("@id_General", idComprobante);
                        _db.EjecutarConsulta1();
                        _db.Desconectar();
                        (Master as SiteMaster).MostrarAlerta(this, "E-Mail enviado correctamente", 2, null);
                    }
                    catch (Exception ex)
                    {
                        var metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        (Master as SiteMaster).MostrarAlerta(this, "No se pudo enviar el E-mail. Razón: " + ex.Message, 4, null);
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Tienes seleccionar algún E-mail", 4, null);
                }
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRfcRec control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRfcRec_TextChanged(object sender, EventArgs e)
        {
            var rfcEmisor = Session["IDENTEMIEXT"].ToString();

            switch (rfcEmisor)
            {
                case "OAL111108K5A":
                case "OAB030130MA1":
                case "OCC1301221FA":
                case "LAN7008173R5":
                    Llenarlista(tbRfcRec.Text);
                    break;
                default: //llenar datos para todos
                    Llenarlista(tbRfcRec.Text);
                    break;
            }
            if (tbRfcRec.Text.Length >= 12)
            {
                var tipoPersona = "%" + (tbRfcRec.Text.Length == 12 ? "F" : (tbRfcRec.Text.Length == 12 ? "M" : "FM")) + "%";
                SqlDataSourceUsoCfdi.SelectParameters["tipoPersonaUsoCfdi"].DefaultValue = tipoPersona;
                ddlUsoCfdi.DataBind();
                ddlUsoCfdi.Enabled = true;
                ddlUsoCfdi.SelectedIndex = 0;
                try
                {
                    var usoMaitreD = GetValue("IdDoc", "UsoCfdi");
                    if (!string.IsNullOrEmpty(usoMaitreD))
                    {
                        ddlUsoCfdi.SelectedValue = usoMaitreD;
                        if (Regex.IsMatch(rfcEmisor, @"OHR980618BLA"))
                        {
                            ddlUsoCfdi.Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            else
            {
                //ddlUsoCfdi.Enabled = false;
                ddlUsoCfdi.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbDomRec control.
        /// </summary>
        protected void HabilitarDomicilioReceptor()
        {
            RequiredFieldValidator14.Enabled = true;
            RequiredFieldValidator15.Enabled = true;
            RequiredFieldValidator16.Enabled = true;
            RequiredFieldValidator_tbPaisRec.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
            RequiredFieldValidator_ddlPais.Enabled = Session["CfdiVersion"].ToString().Equals("3.3");
            RequiredFieldValidator27.Enabled = true;
            ddlSucRec.Items.Clear();
            var itemNull = new ListItem("SELECCIONE", "0");
            itemNull.Selected = true;
            ddlSucRec.Items.Add(itemNull);
            Llenarlistadom(tbRfcRec.Text, false);
            ddlSucRec.Enabled = ddlSucRec.Items.Count > 1;
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
        /// Llenarlistadoms the specified RFC.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        /// <param name="chkDom">if set to <c>true</c> [CHK DOM].</param>
        private void Llenarlistadom(string rfc, bool chkDom = true)
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
                          WHERE RFCREC=@ruc";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@ruc", rfc);
            var dr = _db.EjecutarConsulta();
            var control = (dr.HasRows && !rfc.Equals("XAXX010101000", StringComparison.OrdinalIgnoreCase) && !rfc.Equals("XEXX010101000", StringComparison.OrdinalIgnoreCase));
            tbCalleRec.ReadOnly = control;
            tbNoExtRec.ReadOnly = control;
            tbNoIntRec.ReadOnly = control;
            tbColoniaRec.ReadOnly = control;
            tbMunicipioRec.ReadOnly = control;
            tbEstadoRec.ReadOnly = control;
            tbPaisRec.ReadOnly = control;
            ddlPais.Enabled = !control;
            tbCpRec.ReadOnly = control;
            if (dr.Read())
            {
                tbCalleRec.Text = dr["domicilio"].ToString();
                tbNoExtRec.Text = dr["noExterior"].ToString();
                tbNoIntRec.Text = dr["noInterior"].ToString();
                tbColoniaRec.Text = dr["colonia"].ToString();
                tbMunicipioRec.Text = dr["municipio"].ToString();
                tbEstadoRec.Text = dr["estado"].ToString();
                tbPaisRec.Text = dr["pais"].ToString();
                try
                {
                    var item = ddlPais.Items.Cast<ListItem>().FirstOrDefault(x => Regex.IsMatch(x.Value, dr["pais"].ToString(), RegexOptions.IgnoreCase) || Regex.IsMatch(x.Text, dr["pais"].ToString(), RegexOptions.IgnoreCase));
                    if (item == null)
                    {
                        var tres = new string(dr["pais"].ToString().Take(3).ToArray());
                        item = ddlPais.Items.Cast<ListItem>().FirstOrDefault(x => Regex.IsMatch(x.Value, tres, RegexOptions.IgnoreCase) || Regex.IsMatch(x.Text, tres, RegexOptions.IgnoreCase));
                    }
                    if (item != null)
                    {
                        ddlPais.SelectedValue = item.Value;
                    }
                    if (Regex.IsMatch(Session["IDENTEMIEXT"].ToString(), @"LAN7008173R5|OAL111108K5A"))
                    {
                        try
                        {
                            ddlPais.SelectedValue = "MEX";
                            tbPaisRec.Text = "MEX";
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
                tbCpRec.Text = dr["codigoPostal"].ToString();
                _db.Desconectar();
                _db.Conectar();
                _db.CrearComando(@"SELECT idSucursal, sucursal FROM Cat_Sucursales WHERE RFC = @RFC");
                _db.AsignarParametroCadena("@RFC", rfc);
                dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    ddlSucRec.Items.Add(new ListItem(dr["sucursal"].ToString(), dr["idSucursal"].ToString()));
                }
                _db.Desconectar();
                if (chkDom)
                {
                    HabilitarDomicilioReceptor();
                }
            }
            else
            {
                _db.Desconectar();
                if (chkDom)
                {
                    HabilitarDomicilioReceptor();
                }
                tbRazonSocialRec.Text = "";
                tbCalleRec.Text = "";
                tbNoExtRec.Text = "";
                tbNoIntRec.Text = "";
                tbColoniaRec.Text = "";
                tbMunicipioRec.Text = "";
                tbEstadoRec.Text = "";
                //tbPaisRec.Text = "";
                tbCpRec.Text = "";
                //ddlPais.SelectedValue = "";
                ddlPais.SelectedValue = "MEX";
                tbPaisRec.Text = "MEX";
                ddlUsoCfdi.Enabled = true;
            }
        }

        /// <summary>
        /// Reads the trama micros.
        /// </summary>
        /// <param name="txtInvoice">The text invoice.</param>
        private void ReadTramaMicros(string txtInvoice)
        {
            var version = Session["CfdiVersion"].ToString();
            // Aqui van los que no quieren pagar Micros para colocar un solo concepto para la 3.3
            Session[ViewState["_PageID"].ToString() + "_isOneItem"] = Regex.IsMatch(Session["IDENTEMIEXT"].ToString(), @"RMM120815858|MAN100706R93|MAB081110C73|KAI0305307N8|MDU100621V6A|MAN081113157|GAR020131PP9|MMO040331J1A|SSU041119694|OCC1301221FA|ZFM111018JQ4|OPT970210N72|CFL040216TJ9");
            var _isOneItem = (bool)(Session[ViewState["_PageID"].ToString() + "_isOneItem"]);
            var trama = new TramaMicros(_isOneItem ? "3.2" : version);
            trama.Load(txtInvoice);
            Session[ViewState["_PageID"].ToString() + "_tramaMicros"] = trama;
        }

        private void ReadTramaAloha(string txtInvoice)
        {
            var trama = new Transacction();
            DeserializeAloha(txtInvoice, out trama);
            Session[ViewState["_PageID"].ToString() + "_tramaAloha"] = trama;
        }

        #region Deserialize Document Aloha
        public virtual System.Threading.Tasks.Task<string> SerializeAloha(Transacction trans, System.Text.Encoding encoding)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                System.IO.StreamReader streamReader = null;
                System.IO.MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new System.IO.MemoryStream();
                    System.Xml.XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings();
                    xmlWriterSettings.Encoding = encoding;
                    System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(memoryStream, xmlWriterSettings);
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Transacction));
                    serializer.Serialize(xmlWriter, trans);
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    streamReader = new System.IO.StreamReader(memoryStream);
                    return streamReader.ReadToEnd();
                }
                finally
                {
                    if ((streamReader != null))
                    {
                        streamReader.Dispose();
                    }
                    if ((memoryStream != null))
                    {
                        memoryStream.Dispose();
                    }
                }
            });
        }
        public virtual System.Threading.Tasks.Task<string> SerializeAloha(Transacction trans)
        {
            return SerializeAloha(trans, System.Text.Encoding.UTF8);
        }
        private bool DeserializeAloha(string xml, out Transacction obj, out System.Exception exception)
        {
            exception = null;
            obj = default(Transacction);
            try
            {
                obj = DeserializeAloha(xml);
                return true;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                return false;
            }
        }
        private bool DeserializeAloha(string xml, out Transacction obj)
        {
            System.Exception exception = null;
            return DeserializeAloha(xml, out obj, out exception);
        }
        private Transacction DeserializeAloha(string xml)
        {
            System.IO.StringReader stringReader = null;
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Transacction));
                stringReader = new System.IO.StringReader(xml);
                return ((Transacction)(serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
            }
            finally
            {
                if ((stringReader != null))
                {
                    stringReader.Dispose();
                }
            }
        }

        #endregion

        /// <summary>
        /// Reads the trama micros presidente.
        /// </summary>
        /// <param name="txtInvoice">The text invoice.</param>
        private void ReadTramaMicrosPresidente(string txtInvoice)
        {
            var trama = new TramaHpresidente();
            trama.Load(txtInvoice);
            Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"] = trama;
        }

        private void FillCatalogPaises()
        {
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                var catalogos = (CatCdfi)Session["CatalogosCfdi33"];
                ddlPais.Items.Add(new ListItem("Seleccione", ""));
                foreach (var item in catalogos.CPais)
                {
                    ddlPais.Items.Add(new ListItem(item.Key + ": " + item.Value, item.Key));
                }
                if (Regex.IsMatch(Session["IDENTEMIEXT"].ToString(), @"LAN7008173R5|OAL111108K5A"))
                {
                    try
                    {
                        ddlPais.SelectedValue = "MEX";
                        tbPaisRec.Text = "MEX";
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
            }
        }

        /// <summary>
        /// Lee la trama y la almacena en arreglos
        /// </summary>
        /// <param name="txtInvoice">Trama recibida</param>
        private void ReadTramaMaitred(string txtInvoice)
        {
            Session[ViewState["_PageID"].ToString() + "_arrayTrama"] = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            var sections = Regex.Split(txtInvoice, @"(\r\n|\n)(=+|-+)").Where(x => !string.IsNullOrEmpty(x.Trim()) && !Regex.IsMatch(x.Trim(), @"^( *)(=+|-+)( *)$")).ToList();
            sections.Remove(sections.FirstOrDefault());
            foreach (var section in sections)
            {
                var contents = section.Split(new[] { "\n", "\r\n" }, 2, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
                var title = contents.First().Trim();
                var content = contents.Last();
                var variables = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                var contentSplit = content.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
                if (title.StartsWith("Detalle", StringComparison.OrdinalIgnoreCase))
                {
                    _detalles = new List<Dictionary<string, string>>();
                    if (GetValue("ExEmisor", "RFCEmisor").Equals("ESC920529NP4"))
                    {
                        // ORACLE
                        var detalles = content.Split(new[] { "\nXXXFINDETA", "\r\nXXXFINDETA" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                        foreach (var detalle in detalles)
                        {
                            var contentSplitDetalle = detalle.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
                            var dataDetalles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                            foreach (var variable in contentSplitDetalle)
                            {
                                var key = GetVariableData(variable, 30);
                                if (!dataDetalles.ContainsKey(key.Key))
                                {
                                    dataDetalles.Add(key.Key, key.Value);
                                }
                            }
                            _detalles.Add(dataDetalles);
                        }
                    }
                    else
                    {
                        // MAITRE'D
                        var titleDetalles = contentSplit.First();
                        var titleDetallesSplit = Regex.Split(contentSplit.First(), @" +").ToList().Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                        var lista = contentSplit.ToList();
                        lista.RemoveAt(0);
                        var contentFixed = string.Join(Environment.NewLine, lista);
                        var detalles = contentFixed.Split(new[] { "\nXXXFINDETA", "\r\nXXXFINDETA", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                        foreach (var detalle in detalles)
                        {
                            var dataDetalles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                            int[] spacesApply = new int[0];
                            switch (GetValue("ExEmisor", "RFCEmisor"))
                            {
                                case "OHR980618BLA":
                                case "OTS1001259H0":
                                case "LAN7008173R5":
                                case "SET920324FC3":
                                case "OPL000131DL3":
                                    spacesApply = new int[] { 150, 16, 16, 16, 16, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25 };
                                    break;
                                case "OAL111108K5A":
                                    spacesApply = new int[] { 5, 17, 15, 17, 15, 17, 21, 3, 7, 3, 151, 17, 33, 17, 5, 17, 17, 17, 7, 17, 5, 7, 17, 5, 7, 17, 5, 7, 17, 17, 17, 17, 11, 21, 41, 17, 12 };
                                    break;
                            }
                            var index = 0;
                            for (int i = 0; i < titleDetallesSplit.Count; i++)
                            {
                                var titulo = titleDetallesSplit[i].Trim();
                                var valor = "";
                                if (detalle.Length >= (index + spacesApply[i]))
                                {
                                    valor = detalle.Substring(index, spacesApply[i]).Trim();
                                }
                                if (!dataDetalles.ContainsKey(titulo))
                                {
                                    dataDetalles.Add(titulo, valor);
                                }
                                index += spacesApply[i];
                            }
                            _detalles.Add(dataDetalles);
                        }
                    }
                }
                else if (title.StartsWith("ExImpuestos", StringComparison.OrdinalIgnoreCase))
                {
                    if (GetValue("ExEmisor", "RFCEmisor").Equals("ESC920529NP4"))
                    {
                        // IMPUESTOS DHL
                        _impuestos = new List<Dictionary<string, string>>();
                        var titleImpuestos = contentSplit.First();
                        var titleImpuestosSplit = Regex.Split(contentSplit.First(), @" +").ToList();
                        var lista = contentSplit.ToList();
                        lista.RemoveAt(0);
                        foreach (var lineImpuestos in lista)
                        {
                            var contentImpuestosSplit = Regex.Split(lineImpuestos, @" +").Select(s => s.Trim()).ToList();
                            if (titleImpuestosSplit.Count != contentImpuestosSplit.Count)
                            {
                                continue;
                            }
                            var dataImpuestos = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                            for (var detI = 0; detI < titleImpuestosSplit.Count; detI++)
                            {
                                dataImpuestos.Add(titleImpuestosSplit[detI], contentImpuestosSplit[detI]);
                            }
                            _impuestos.Add(dataImpuestos);
                        }
                    }
                    else
                    {
                        foreach (var variable in contentSplit)
                        {
                            if (!variable.StartsWith("XXX"))
                            {
                                var key = GetVariableData(variable, 20, !title.StartsWith("Personalizados", StringComparison.OrdinalIgnoreCase));
                                if (!variables.ContainsKey(key.Key))
                                {
                                    variables.Add(key.Key, key.Value);
                                }
                            }
                        }
                        ((Dictionary<string, Dictionary<string, string>>)Session[ViewState["_PageID"].ToString() + "_arrayTrama"]).Add(title, variables);
                    }
                }
                else if (title.StartsWith("ExRetenciones", StringComparison.OrdinalIgnoreCase))
                {
                    _retenciones = new List<Dictionary<string, string>>();
                    var titleRetenciones = contentSplit.First();
                    var titleRetencionesSplit = Regex.Split(contentSplit.First(), @" +").ToList();
                    var lista = contentSplit.ToList();
                    lista.RemoveAt(0);
                    foreach (var lineRetenciones in lista)
                    {
                        var contentRetencionesSplit = Regex.Split(lineRetenciones, @" +").Select(s => s.Trim()).ToList();
                        if (titleRetencionesSplit.Count != contentRetencionesSplit.Count)
                        {
                            continue;
                        }
                        var dataRetenciones = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                        for (var detI = 0; detI < titleRetencionesSplit.Count; detI++)
                        {
                            dataRetenciones.Add(titleRetencionesSplit[detI], contentRetencionesSplit[detI]);
                        }
                        _retenciones.Add(dataRetenciones);
                    }
                }
                else if (title.StartsWith("Addendas", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var variable in contentSplit)
                    {
                        var key = GetVariableData(variable, 35);
                        if (!variables.ContainsKey(key.Key))
                        {
                            variables.Add(key.Key, key.Value);
                        }
                    }
                }
                else
                {
                    foreach (var variable in contentSplit)
                    {
                        if (!variable.StartsWith("XXX"))
                        {
                            var key = GetVariableData(variable, 20, !title.StartsWith("Personalizados", StringComparison.OrdinalIgnoreCase));
                            if (!variables.ContainsKey(key.Key))
                            {
                                variables.Add(key.Key, key.Value);
                            }
                        }
                    }
                    ((Dictionary<string, Dictionary<string, string>>)Session[ViewState["_PageID"].ToString() + "_arrayTrama"]).Add(title, variables);
                }
            }
        }

        /// <summary>
        /// Obtiene el valor de la trama
        /// </summary>
        /// <param name="bloque">Bloque de la trama</param>
        /// <param name="variable">Variable en el bloque</param>
        /// <returns>El valor de la variable, si no se encuentra, se retorna una cadena vacia para evitar errores</returns>
        private string GetValue(string bloque, string variable)
        {
            string value = "";
            try
            {
                value = ((Dictionary<string, Dictionary<string, string>>)Session[ViewState["_PageID"].ToString() + "_arrayTrama"])[bloque][variable] ?? "";
            }
            catch
            {
                value = "";
            }
            return value;
        }

        /// <summary>
        /// Establece el valor de una variable de la trama
        /// </summary>
        /// <param name="bloque">Bloque de la trama</param>
        /// <param name="variable">Variable en el bloque</param>
        /// <param name="valor">Valor de la variable a asignar</param>
        /// <param name="appendNewLine">if set to <c>true</c> [append new line].</param>
        /// <returns>True si se cambio el valor, false de lo contrario</returns>
        private bool SetValue(string bloque, string variable, string valor, bool appendNewLine = false)
        {
            bool value = false;
            try
            {
                ((Dictionary<string, Dictionary<string, string>>)Session[ViewState["_PageID"].ToString() + "_arrayTrama"])[bloque][variable] = valor;
                var splitted = Regex.Split(Session[ViewState["_PageID"].ToString() + "_trama"].ToString(), @"(-+|=+) +" + bloque + (appendNewLine ? "\n" : ""), RegexOptions.IgnoreCase & RegexOptions.Singleline);
                //var splitted = trama.Split(("== " + bloque).ToCharArray(), 2);
                var separator = "================ " + bloque + (appendNewLine ? "\n" : "");
                var before = splitted.First();
                var after = splitted.Last();
                splitted = after.Split("==".ToCharArray(), 2);
                var middle = splitted.First();
                after = splitted.Last();
                middle = Regex.Replace(middle, variable + ".*", variable + "        " + valor);
                Session[ViewState["_PageID"].ToString() + "_trama"] = before + separator + middle + after;
                value = (GetValue(bloque, variable).Equals(valor));
            }
            catch
            {
                value = false;
            }
            return value;
        }

        /// <summary>
        /// Obtiene el nombre y el valor de cada variable
        /// </summary>
        /// <param name="variableLine">La linea que contiene la variable a leer</param>
        /// <param name="splitChars">La cantidad de caracteres que separan el nombre del valor</param>
        /// <param name="processExtraBlanks">True si se requiere procesar los espacios en blanco del valor de la variable, false de lo contrario</param>
        /// <returns>El conjunto que contiene el nombre de la variable y su valor</returns>
        private KeyValuePair<string, string> GetVariableData(string variableLine, int splitChars, bool processExtraBlanks = true)
        {
            Match variableContent = null;
            GroupCollection variableGroups = null;
            var variableName = "";
            var variableValue = "";
            var variableTemp = variableLine;
            var loop = false;
            do
            {
                loop = false;
                variableContent = Regex.Match(variableTemp, @"(.*)( {2,})(.*)$", RegexOptions.IgnoreCase);
                variableGroups = variableContent.Groups;
                variableName = variableGroups[1].Value.Trim();
                variableValue = (variableName.Contains("  ") ? variableGroups[2].Value : "") + variableGroups[3].Value.Trim() + variableValue;
                if (processExtraBlanks && variableName.Contains("  "))
                {
                    variableTemp = variableName;
                    loop = true;
                }
                if (string.IsNullOrEmpty(variableName) && string.IsNullOrEmpty(variableValue))
                {
                    variableName = new string(variableLine.Take(splitChars).ToArray());
                    variableValue = variableLine.Replace(variableName, "");
                    variableName = variableName.Trim();
                }
            } while (loop);
            variableValue = variableValue.TrimStart(':').Trim();
            return new KeyValuePair<string, string>(variableName, variableValue);
        }

        /// <summary>
        /// Llenarlistas the specified RFC.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        private void Llenarlista(string rfc)
        {
            var sql = @"SELECT TOP 1 IDEREC
                              ,[NOMREC]
                              ,[telefono]
                              ,[contribuyenteEspecial]
                              ,[obligadoContabilidad]
                              ,[tipoIdentificacionComprador]
                              ,[email]
                              ,[curp]
                              ,(CASE ISNULL(CONVERT(VARCHAR, metodoPago), '') WHEN '' THEN '99' ELSE ISNULL(cc.codigo, '99') END) AS metodoPago
                              ,[numCtaPago]
                              ,[telefono2]
                          FROM [Cat_Receptor] LEFT OUTER JOIN Cat_Catalogo1_C cc ON CONVERT(VARCHAR, metodoPago) = cc.codigo AND cc.tipo = 'MetodoPago'
                          WHERE RFCREC=@ruc ORDER BY IDEREC ASC";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@ruc", rfc);
            var dr = _db.EjecutarConsulta();
            tbRazonSocialRec.ReadOnly = dr.HasRows;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tbRfcRec.Text = rfc;
                    tbRazonSocialRec.Text = dr["NOMREC"].ToString();
                    tbMailReceptor.Text = dr["email"].ToString();
                }
            }
            _db.Desconectar();
            Llenarlistadom(rfc);
        }

        /// <summary>
        /// Validas the fecha consumo.
        /// </summary>
        /// <param name="fechaTrama">The fecha trama.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidaFechaConsumo(string fechaTrama)
        {
            var status = true;
            try
            {
                var diasConsumo = 0;
                _db.Conectar();
                _db.CrearComando(@"SELECT
	TOP 1 ISNULL(diasFacturacionExtranet, 0) AS diasFacturacionExtranet
FROM
	Par_ParametrosSistema");
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    int.TryParse(dr[0].ToString(), out diasConsumo);
                }
                _db.Desconectar();
                if (diasConsumo < 0)
                {
                    var date1 = Localization.Now;
                    var date2 = DateTime.Parse(fechaTrama);
                    var diffMonths = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
                    status = (diffMonths <= 0);
                }
                else
                {
                    var controlDate = Localization.Now.AddDays(diasConsumo * -1);
                    var fechaConsumo = DateTime.Parse(fechaTrama);
                    status = (fechaConsumo >= controlDate);
                }
                var rfc = Session["IDENTEMIEXT"].ToString();
                if (status && (rfc.Equals("OAL111108K5A") || rfc.Equals("HSL130116EN5")))
                {
                    var hoy = Localization.Now;
                    var fechaConsumo = DateTime.Parse(fechaTrama);
                    status = (hoy.Month == fechaConsumo.Month);
                }
            }
            catch (Exception ex)
            {
                status = true;
            }
            return status;
        }

        /// <summary>
        /// Gets the ine.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetIne()
        {
            #region NODO INE
            var txt = new SpoolMx();
            var linea = "";
            if (chkHabilitarINE.Checked)
            {
                txt.SetComplementoIneCfdi(DDTipo_Proceso.SelectedValue, DDTipoComite.SelectedValue, TextboxIdentificador.Text);
                string[] arrayEntidades = new string[64];
                int conta = 0;
                foreach (string[] registro in _arrayRegistros)
                {
                    if (!arrayEntidades.Contains(registro[3] + registro[4]))
                    {
                        //Por clave
                        txt.AgregarEntidadIne(registro[3], registro[4]);
                        //Por Entidad
                        foreach (string[] registroId in _arrayRegistros)
                        {
                            if (registroId[3] == registro[3])
                            {
                                if (!string.IsNullOrEmpty(registro[7]))
                                {
                                    string[] grupoIdContabilidad = registro[7].Split(',');
                                    foreach (string idContabilidad in grupoIdContabilidad)
                                    {
                                        txt.AgregarContabilidadIne(idContabilidad);
                                    }
                                }
                            }
                        }
                        arrayEntidades[conta] = registro[3] + registro[4];
                        conta++;
                    }
                }
            }
            if (txt.CfdiComplementoIne != null)
            {
                var arrayIne = (string[])txt.CfdiComplementoIne[0];
                var arrayEntidades = (List<object[]>)txt.CfdiComplementoIne[1];
                if (arrayIne != null)
                {
                    linea += ConcatArray(arrayIne);
                    if (arrayEntidades != null)
                    {
                        foreach (var entidad in arrayEntidades)
                        {
                            var datosEntidad = (string[])entidad[0];
                            var arrayContabilidades = (List<string[]>)entidad[1];
                            if (datosEntidad != null)
                            {
                                linea += ConcatArray(datosEntidad);
                                if (arrayContabilidades != null)
                                {
                                    foreach (var contabilidad in arrayContabilidades)
                                    {
                                        linea += ConcatArray(contabilidad);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(linea));
            #endregion
        }

        /// <summary>
        /// Concats the array.
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

        /// <summary>
        /// Validates the mail.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>System.Int32.</returns>
        [WebMethod]
        public static int ValidateMail(string email)
        {
            var valid = ControlUtilities.ValidateMail(email);
            var iStatus = (int)valid;
            return iStatus;
        }

        /// <summary>
        /// Handles the Click event of the bGenerar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bGenerar_Click(object sender, EventArgs e)
        {
            var metodoPago = ddlMetodoPago.SelectedItem.Text;
            var listKeys = ddlMetodoPago.Attributes["jsFunction"];
            if (!string.IsNullOrEmpty(listKeys) && ddlMetodoPago.CssClass.Contains("bootstrap-select-multiple") && !string.IsNullOrEmpty(hfMetodoPago.Value))
            {
                metodoPago = hfMetodoPago.Value;
            }
            if (string.IsNullOrEmpty(metodoPago))
            {
                (Master as SiteMaster).MostrarAlerta(this, "El código del pago no puede quedar vacío", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
            }
            if (Session[ViewState["_PageID"].ToString() + "_trama"].ToString().StartsWith("XXX", StringComparison.OrdinalIgnoreCase))
            {
                // Trama Maitre'D
                try
                {
                    SetValue("ExReceptor", "RFCRecep", tbRfcRec.Text, true);
                    if (Regex.IsMatch(GetValue("ExEmisor", "RFCEmisor"), @"OHR980618BLA|OTS1001259H0|LAN7008173R5|SET920324FC3|OPL000131DL3"))
                    {
                        SetValue("ExReceptor", "NmbRecep", tbRazonSocialRec.Text + "|" + tbMailReceptor.Text, true);
                        SetValue("ExReceptorDomFiscal", "Calle", tbCalleRec.Text);
                        SetValue("ExReceptorDomFiscal", "NroExterior", tbNoExtRec.Text);
                        SetValue("ExReceptorDomFiscal", "NroInterior", tbNoIntRec.Text);
                        SetValue("ExReceptorDomFiscal", "Colonia", tbColoniaRec.Text);
                        SetValue("ExReceptorDomFiscal", "Municipio", tbMunicipioRec.Text);
                        SetValue("ExReceptorDomFiscal", "Estado", tbEstadoRec.Text);
                        SetValue("ExReceptorDomFiscal", "Pais", tbPaisRec.Text);
                        if (Session["CfdiVersion"].ToString().Equals("3.3"))
                        {
                            SetValue("ExReceptorDomFiscal", "Pais", ddlPais.SelectedValue);
                        }
                        SetValue("ExReceptorDomFiscal", "CP", tbCpRec.Text);
                        SetValue("ExReceptorDomFiscal", "Localidad", "");
                        SetValue("ExReceptorDomFiscal", "Referencia", "");
                        SetValue("IdDoc", "MetodoPago", metodoPago);
                        SetValue("IdDoc", "NumCuentan", tbNumCtaPago.Text);
                        SetValue("IdDoc", "Estado", tbObservaciones.Text);
                        SetValue("IdDoc", "UsoCfdi", ddlUsoCfdi.SelectedValue);
                        var ineSection = "XXXINE" + Environment.NewLine + "Linea = " + GetIne();
                        Session[ViewState["_PageID"].ToString() + "_trama"] = Session[ViewState["_PageID"].ToString() + "_trama"].ToString().Replace("XXXFINDOC", ineSection + Environment.NewLine + "XXXFINDOC");
                    }
                    else if (GetValue("ExEmisor", "RFCEmisor").Equals("OAL111108K5A", StringComparison.OrdinalIgnoreCase))
                    {
                        SetValue("ExReceptor", "NmbRecep", tbRazonSocialRec.Text, true);
                        SetValue("ExReceptor", "Contacto", tbMailReceptor.Text, true);
                        SetValue("ExReceptorDomFiscal", "Calle", tbCalleRec.Text, false);
                        SetValue("ExReceptorDomFiscal", "NroExterior", tbNoExtRec.Text, false);
                        SetValue("ExReceptorDomFiscal", "NroInterior", tbNoIntRec.Text, false);
                        SetValue("ExReceptorDomFiscal", "Colonia", tbColoniaRec.Text, false);
                        SetValue("ExReceptorDomFiscal", "Municipio", tbMunicipioRec.Text, false);
                        SetValue("ExReceptorDomFiscal", "Estado", tbEstadoRec.Text, false);
                        SetValue("ExReceptorDomFiscal", "Pais", tbPaisRec.Text, false);
                        SetValue("ExReceptorDomFiscal", "CodigoPostal", tbCpRec.Text, false);
                        SetValue("ExReceptorDomFiscal", "Localidad", "", false);
                        SetValue("ExReceptorDomFiscal", "Referencia", "", false);
                        SetValue("Personalizados", "MedioPago", metodoPago, false);
                        SetValue("Personalizados", "CtaPagoCli", tbNumCtaPago.Text, false);
                        SetValue("IdDoc", "Estado", tbObservaciones.Text, false);
                        SetValue("IdDoc", "UsoCfdi", ddlUsoCfdi.SelectedValue);
                        string seriefolioM = GetValue("IdDoc", "Serie");
                        string folioserM= GetValue("IdDoc", "Folio");
                        _mmsg = Convert.ToString(seriefolioM) + Convert.ToString(folioserM);
                        var ineSection = "---------------- Ine" + Environment.NewLine + "Linea             " + GetIne();
                        Session[ViewState["_PageID"].ToString() + "_trama"] = Session[ViewState["_PageID"].ToString() + "_trama"].ToString().Replace("XXXFINDO", ineSection + Environment.NewLine + "XXXFINDO");

                        
                    }
                    object[] result = null;
                    //var count = 0;
                    //do
                    //{
                    //    count++;
                    var randomMs = new Random().Next(1000, 5000);
                    System.Threading.Thread.Sleep(randomMs);
                    var ws = new wsRestaurantes.wsRestaurantes { Timeout = (1800 * 1000) };
                    result = ws.RecibeInfoTxt(Session[ViewState["_PageID"].ToString() + "_trama"].ToString(), "", true);
                    //} while ((result == null || !((bool)result[0])) && count < 3);
                    if (result != null)
                    {
                        var status = (bool)result[0];
                        var msg = (string)result[1];
                        if (status)
                        {
                            try
                            {
                                _db.Conectar();
                                _db.CrearComando(@"UPDATE Log_Trama SET
                                observaciones = NULL
                                WHERE idTrama = @idTrama");
                                _db.AsignarParametroCadena("@idTrama", Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString());
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                            }
                            catch { }
                            if (Session["IDENTEMIEXT"].ToString().Equals("OAL111108K5A"))
                            {
                                tbTicket.Text = _mmsg;
                                msg = _mmsg;

                            }
                            bConsultar_Click(null, null);
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + msg, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + "El servidor regresó una respuesta vacía", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + ex.Message, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                }
            }
            else if (Session[ViewState["_PageID"].ToString() + "_trama"].ToString().StartsWith("OPXXX", StringComparison.OrdinalIgnoreCase))
            {
                //trama opera 
                #region Trama Opera
                try
                {
                    string result = null;
                    var randomMs = new Random().Next(1000, 5000);
                    System.Threading.Thread.Sleep(randomMs);
                    var ws = new wsOpera.WsOpera { Timeout = (1800 * 1000) };
                    result = ws.ConvertDocument("", "", "AUTO", Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                    //} while ((result == null || !((bool)result[0])) && count < 3);
                    if (result != null)
                    {
                        var msg = result;
                        if (msg.Equals("OK"))
                        {
                            try
                            {
                                _db.Conectar();
                                _db.CrearComando(@"UPDATE Log_Trama SET
                                observaciones = NULL
                                WHERE idTrama = @idTrama");
                                _db.AsignarParametroCadena("@idTrama", Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString());
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                            }
                            catch { }
                            if (Session["IDENTEMIEXT"].ToString().Equals("OAL111108K5A"))
                            {
                                //  tbTicket.Text = msg;
                            }
                            bConsultar_Click(null, null);
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + msg, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + "El servidor regresó una respuesta vacía", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + ex.Message, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                }
                #endregion

            }
            else if (Session[ViewState["_PageID"].ToString() + "_trama"].ToString().StartsWith("T|", StringComparison.OrdinalIgnoreCase))
            {
                // Trama MICROS
                try
                {
                    var lineaR = "R|" + tbRfcRec.Text + "|" + tbRazonSocialRec.Text + "|" + tbCalleRec.Text +
                                  "|" + tbNoExtRec.Text + "|" + tbNoIntRec.Text + "|" + tbColoniaRec.Text + "|" + "" +
                                  "|" + tbMunicipioRec.Text + "|" + tbEstadoRec.Text + "|" + (Session["CfdiVersion"].ToString().Equals("3.3") ? ddlPais.SelectedValue : tbPaisRec.Text) +
                                  "|" + tbCpRec.Text + "|" + tbMailReceptor.Text + "|" + ddlUsoCfdi.SelectedValue;

                    var lineaExt = "E|" + metodoPago + "|" + tbNumCtaPago.Text + "|" + tbObservaciones.Text + "|" + GetIne();

                    Session[ViewState["_PageID"].ToString() + "_trama"] += Environment.NewLine + lineaR + Environment.NewLine + lineaExt;
                    object[] result = null;
                    //var count = 0;
                    //do
                    //{
                    //    count++;
                    var randomMs = new Random().Next(1000, 5000);
                    System.Threading.Thread.Sleep(randomMs);
                    var ws = new wsMicros.wsMicros { Timeout = (1800 * 1000) };
                    result = ws.RecibeInfoTxt(Session[ViewState["_PageID"].ToString() + "_trama"].ToString(), Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString(), Session["IDENTEMIEXT"].ToString(), null, true);
                    //} while ((result == null || !((bool)result[0])) && count < 3);
                    if (result != null)
                    {
                        var status = (bool)result[0];
                        var msg = (string)result[1];
                        if (status)
                        {
                            try
                            {
                                _db.Conectar();
                                _db.CrearComando(@"UPDATE Log_Trama SET
                                observaciones = NULL
                                WHERE idTrama = @idTrama");
                                _db.AsignarParametroCadena("@idTrama", Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString());
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                            }
                            catch { }
                            if (Session["IDENTEMIEXT"].ToString().Equals("OAL111108K5A"))
                            {
                                
                                tbTicket.Text = msg;  
                            }
                            bConsultar_Click(null, null);
                        }
                        else
                        {
                            var mensaje = msg.Replace(@"\", "");
                            if (Regex.IsMatch(mensaje, @"El ticket ya se encuentra facturado en la factura", RegexOptions.IgnoreCase))
                            {
                                bConsultar_Click(null, null);
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + msg, 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                            }
                        }
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + "El servidor regreso una respuesta vacia", 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + ex.Message, 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                }
            }
            else
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                var nodos = xDoc.ChildNodes.Cast<XmlNode>().ToList();
                if (nodos.Any(n => n.Name.Equals("documentoFiscalmicros", StringComparison.OrdinalIgnoreCase)))
                {
                    try
                    {
                        var result3 = xDoc.DocumentElement.ChildNodes.Cast<XmlNode>().ToList()
                            .Where(x => x.Name.Equals("receptor")).FirstOrDefault();
                        if (result3 != null)
                        {
                            XmlElement rfcrec = xDoc.CreateElement("rfcRecep");
                            rfcrec.InnerText = tbRfcRec.Text;
                            XmlElement razonsorec = xDoc.CreateElement("razonsoRecep");
                            razonsorec.InnerText = tbRazonSocialRec.Text;
                            XmlElement callerec = xDoc.CreateElement("CalleRecep");
                            callerec.InnerText = tbCalleRec.Text;
                            XmlElement noextrec = xDoc.CreateElement("noExteriorRecep");
                            noextrec.InnerText = tbNoExtRec.Text;
                            XmlElement nointrec = xDoc.CreateElement("noInteriorRecep");
                            nointrec.InnerText = tbNoIntRec.Text;
                            XmlElement coloniarec = xDoc.CreateElement("coloniaRecep");
                            coloniarec.InnerText = tbColoniaRec.Text;
                            XmlElement municipiorec = xDoc.CreateElement("minicipioRecep");
                            municipiorec.InnerText = tbMunicipioRec.Text;
                            XmlElement estadorec = xDoc.CreateElement("estadoRecep");
                            estadorec.InnerText = tbEstadoRec.Text;
                            XmlElement paisrec = xDoc.CreateElement("paisRecep");
                            paisrec.InnerText = tbPaisRec.Text;
                            XmlElement cprec = xDoc.CreateElement("cpRecep");
                            cprec.InnerText = tbCpRec.Text;
                            XmlElement mailrec = xDoc.CreateElement("mailRecep");
                            mailrec.InnerText = tbMailReceptor.Text;
                            XmlElement metodopagorec = xDoc.CreateElement("MetodoPago");
                            metodopagorec.InnerText = metodoPago;
                            XmlElement numctapagorec = xDoc.CreateElement("NumCtaPago");
                            numctapagorec.InnerText = tbNumCtaPago.Text;
                            XmlElement observacionesrec = xDoc.CreateElement("Observaciones");
                            observacionesrec.InnerText = tbObservaciones.Text;
                            XmlElement ine = xDoc.CreateElement("GetIne");
                            ine.InnerText = GetIne();

                            result3.AppendChild(rfcrec);
                            result3.AppendChild(razonsorec);
                            result3.AppendChild(callerec);
                            result3.AppendChild(noextrec);
                            result3.AppendChild(nointrec);
                            result3.AppendChild(coloniarec);
                            result3.AppendChild(municipiorec);
                            result3.AppendChild(estadorec);
                            result3.AppendChild(paisrec);
                            result3.AppendChild(cprec);
                            result3.AppendChild(mailrec);
                            result3.AppendChild(metodopagorec);
                            result3.AppendChild(numctapagorec);
                            result3.AppendChild(observacionesrec);
                            result3.AppendChild(ine);

                            Session[ViewState["_PageID"].ToString() + "_trama"] = xDoc.OuterXml;

                        }
                        object[] result = null;
                        //var count = 0;
                        //do
                        //{
                        //    count++;
                        var randomMs = new Random().Next(1000, 5000);
                        System.Threading.Thread.Sleep(randomMs);
                        var ws = new wsHpresidente.wsHpresidente { Timeout = (1800 * 1000) };
                        result = ws.RecibeInfoTxt(Session[ViewState["_PageID"].ToString() + "_trama"].ToString(), Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString(), Session["IDENTEMIEXT"].ToString(), null, true);
                        //} while ((result == null || !((bool)result[0])) && count < 3);
                        if (result != null)
                        {
                            var status = (bool)result[0];
                            var msg = (string)result[1];
                            if (status)
                            {
                                try
                                {
                                    _db.Conectar();
                                    _db.CrearComando(@"UPDATE Log_Trama SET
                                observaciones = NULL
                                WHERE idTrama = @idTrama");
                                    _db.AsignarParametroCadena("@idTrama", Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString());
                                    _db.EjecutarConsulta1();
                                    _db.Desconectar();
                                }
                                catch { }
                                if (Session["IDENTEMIEXT"].ToString().Equals("OAL111108K5A"))
                                {
                                    
                                    tbTicket.Text = msg;
                                   
                                }

                                
                                bConsultar_Click(null, null);
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + msg, 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                            }
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + "El servidor regreso una respuesta vacia", 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + ex.Message, 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
                else if (nodos.Any(n => n.Name.Equals("Transacction", StringComparison.OrdinalIgnoreCase)))
                {
                    #region Trama Aloha

                    try
                    {
                        ReadTramaAloha(Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                        var trama = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]);
                        var lineaR = "R|" + tbRfcRec.Text + "|" + tbRazonSocialRec.Text + "|" + tbCalleRec.Text +
                                     "|" + tbNoExtRec.Text + "|" + tbNoIntRec.Text + "|" + tbColoniaRec.Text + "|" + "" +
                                     "|" + tbMunicipioRec.Text + "|" + tbEstadoRec.Text + "|" + (Session["CfdiVersion"].ToString().Equals("3.3") ? ddlPais.SelectedValue : tbPaisRec.Text) +
                                     "|" + tbCpRec.Text + "|" + tbMailReceptor.Text + "|" + ddlUsoCfdi.SelectedValue;
                        var lineaExt = "E|" + metodoPago + "|" + tbNumCtaPago.Text + "|" + tbObservaciones.Text + "|" + GetIne();
                        trama.LineaR = lineaR;
                        trama.LineaExt = lineaExt;
                        var serialized = SerializeAloha(trama).Result;
                        Session[ViewState["_PageID"].ToString() + "_trama"] = serialized;
                        object[] result = null;
                        var serie = "";
                        _db.Conectar();
                        _db.CrearComando("SELECT serie FROM Log_Trama WHERE idTrama = @id");
                        _db.AsignarParametroCadena("@id", Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString());
                        var dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            serie = dr[0].ToString();
                        }
                        _db.Desconectar();
                        //var count = 0;
                        //do
                        //{
                        //    count++;
                        var randomMs = new Random().Next(1000, 5000);
                        System.Threading.Thread.Sleep(randomMs);
                        var ws = new wsAloha.wsAloha { Timeout = (1800 * 1000) };

                        result = ws.RecibeInfoTxt(Session[ViewState["_PageID"].ToString() + "_trama"].ToString(), Session["IDENTEMIEXT"].ToString(), "", serie, true);
                        //} while ((result == null || !((bool)result[0])) && count < 3);
                        if (result != null)
                        {
                            var status = (bool)result[0];
                            var msg = (string)result[1];
                            if (status)
                            {
                                try
                                {
                                    _db.Conectar();
                                    _db.CrearComando(@"UPDATE Log_Trama SET
                                observaciones = NULL
                                WHERE idTrama = @idTrama");
                                    _db.AsignarParametroCadena("@idTrama", Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString());
                                    _db.EjecutarConsulta1();
                                    _db.Desconectar();
                                }
                                catch { }
                                bConsultar_Click(null, null);
                            }
                            else
                            {
                                var mensaje = msg.Replace(@"\", "");
                                if (Regex.IsMatch(mensaje, @"El ticket ya se encuentra facturado en la factura", RegexOptions.IgnoreCase))
                                {
                                    bConsultar_Click(null, null);
                                }
                                else
                                {
                                    (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + msg, 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                                }
                            }
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + "El servidor regreso una respuesta vacia", 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Error al generar el comprobante" + Environment.NewLine + Environment.NewLine + ex.Message, 4, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }

                    #endregion
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Trama no reconocida", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    return;
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlMetodoPago control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNumCtaPago.Text = "";
            if (Session["IDENTEMIEXT"].ToString().Equals("ASA150811K53"))
            {
                if (ddlMetodoPago.SelectedValue.Equals("99"))
                {
                    tbFormaPago.Text = "PPD";
                }
                else
                {
                    tbFormaPago.Text = "PUE";
                }
            }
        }

        /// <summary>
        /// Ceroses the null.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>System.String.</returns>
        private string CerosNull(string a)
        {
            var result = a;
            var cifra = (!string.IsNullOrEmpty(result) ? result : "").Replace(",", "").Trim();
            if (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(cifra))
            {
                decimal b = 0;
                if (decimal.TryParse(cifra, out b))
                {
                    result = string.Format("{0:0.00}", b);
                }
            }
            return result;
        }

        #region CODIGO INE
        /// <summary>
        /// Handles the Click event of the LnkBAgregarIdentificador control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void LnkBAgregarIdentificador_Click(object sender, EventArgs e)
        {
            if (DDAmbito.SelectedValue.Equals("") && DDAmbito.Visible)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe definir un Ámbito para la Entidad", 4);
                return;
            }
            else if (DDTipoComite.SelectedValue.Equals("") && DDTipoComite.Visible)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe definir un Tipo de Comité", 4);
                return;
            }
            else if (DDTipo_Proceso.SelectedValue.Equals("") && DDTipo_Proceso.Visible)
            {
                (Master as SiteMaster).MostrarAlerta(this, "Debe definir un Tipo de Proceso", 4);
                return;
            }

            ////////////////////// VERIFICAR SI EL IDCONTABILIDAD YA EISTE ////////////////////////
            //bool idContaExist = false;
            //if (divIdConta.Visible && TextboxIdentificador.Text != "")
            //{
            //    if (Array_registros.Count > 0)
            //    {
            //        foreach (string[] registro in Array_registros)
            //        {
            //            if (registro[1] == TextboxIdentificador.Text &&
            //               registro[4] == DDAmbito.SelectedItem.Value)
            //                idContaExist = true;
            //        }
            //    }
            //}
            ////////////////////// FIN VERIFICAR SI EL IDCONTABILIDAD YA EISTE /////////////////////

            ////////////////////// VERIFICAR SI LA ENTIDAD YA EISTE ////////////////////////
            bool entidadExist = false;
            if (divIdConta.Visible)
            {
                if (_arrayRegistros.Count > 0)
                {
                    foreach (string[] registro in _arrayRegistros)
                    {
                        if (registro[2] == DDEstado.SelectedValue)
                            entidadExist = true;
                    }
                }
            }
            ////////////////////// FIN VERIFICAR SI LA ENTIDAD YA EISTE /////////////////////

            //PRUEBAS
            //idContaExist = false;
            //PRUEBAS
            if (!entidadExist)
            {
                _registro = new string[8];
                _registro[0] = _idRenglon.ToString();
                _idRenglon++;
                _registro[1] = divIdContaG.Visible == true ? TextboxIdentificador.Text : "";
                _registro[2] = divEntidad.Visible == true ? DDEstado.SelectedItem.Text : "";
                _registro[3] = divEntidad.Visible == true == true ? DDEstado.SelectedItem.Value : "";
                /*Agregar Columnas Tipo Proceso, Tipo Comité, Ambito al gridview*/
                _registro[4] = divAmbito.Visible == true ? DDAmbito.SelectedItem.Value : "";
                _registro[5] = divComite.Visible == true ? DDTipoComite.SelectedItem.Value : "";
                _registro[6] = DDTipo_Proceso.SelectedItem.Value != "" ? DDTipo_Proceso.SelectedItem.Value : "";
                _registro[7] = divIdConta.Visible == true ? tbIdConta.Text : "";
                _arrayRegistros.Add(_registro);
                crear_tabla_INE();
                if (_arrayRegistros.Count > 0)
                {
                    foreach (string[] registro in _arrayRegistros)
                    {
                        DataRow row = _identificadoresTable.NewRow();
                        row["No"] = registro[0];
                        row["Identificador"] = registro[1];
                        row["Estado"] = registro[2];
                        row["Prefijo"] = registro[3];
                        /*Agregar Columnas Tipo Proceso, Tipo Comité, Ambito al gridview*/
                        row["Ámbito"] = registro[4];
                        row["Tipo Comité"] = registro[5];
                        row["Tipo Proceso"] = registro[6];
                        row["IdContabilidad"] = registro[7];
                        _identificadoresTable.Rows.Add(row);
                        gvRegistros.DataSource = _identificadoresTable;
                        gvRegistros.DataBind();

                    }
                }
                //TextboxIdentificador.Text = "";
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "La entidad ya se ha agregado", 4);
            }
        }
        /// <summary>
        /// Handles the DataBound event of the gvRegistros control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gvRegistros_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                //Ocultar celdas de No de renglon e identificador de estado
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[5].Visible = false;

                //No permitir modificar datos
                DDTipoComite.Enabled = false;
                DDTipo_Proceso.Enabled = false;
                TextboxIdentificador.ReadOnly = true;
            }
            else
            {
                DDTipoComite.Enabled = true;
                DDTipo_Proceso.Enabled = true;
                TextboxIdentificador.ReadOnly = false;
            }
        }
        /// <summary>
        /// Handles the Click event of the btnEliminar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs" /> instance containing the event data.</param>
        protected void btnEliminar_Click(object sender, GridViewCommandEventArgs e)
        {
            ArrayList arrayRegistrosAux = (ArrayList)_arrayRegistros.Clone();
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "SelectRow")
            {
                arrayRegistrosAux = (ArrayList)_arrayRegistros.Clone();
                foreach (string[] registro in _arrayRegistros)
                {
                    if (registro[0].ToString() == index.ToString())
                    {
                        arrayRegistrosAux.Remove(registro);
                    }
                }
            }
            if (e.CommandName == "AddId")
            {
                int contador = 0;
                arrayRegistrosAux = new ArrayList();
                foreach (string[] registro in _arrayRegistros)
                {
                    contador++;
                    if (registro[0].ToString() == index.ToString())
                    {
                        //AGREGAR EL PRIMER IDENTIFICADOR
                        registro[7] = string.IsNullOrEmpty(registro[7]) ? tbIdConta.Text : registro[7] + "," + tbIdConta.Text;
                        arrayRegistrosAux.Add(registro);
                    }
                    else
                    {
                        arrayRegistrosAux.Add(registro);
                    }
                }
                tbIdConta.Text = "";
            }
            crear_tabla_INE();
            _arrayRegistros = arrayRegistrosAux;
            if (_arrayRegistros.Count > 0)
            {
                foreach (string[] registro in _arrayRegistros)
                {
                    DataRow row = _identificadoresTable.NewRow();
                    row["No"] = registro[0];
                    row["Identificador"] = registro[1];
                    row["Estado"] = registro[2];
                    row["Prefijo"] = registro[3];
                    /*Agregar Columnas Tipo Proceso, Tipo Comité, Ambito al gridview*/
                    row["Ámbito"] = registro[4];
                    row["Tipo Comité"] = registro[5];
                    row["Tipo Proceso"] = registro[6];
                    row["IdContabilidad"] = registro[7];

                    _identificadoresTable.Rows.Add(row);
                    gvRegistros.DataSource = _identificadoresTable;
                    gvRegistros.DataBind();

                }
            }
            else
                gvRegistros.DataBind();
            CheckRegistros();
        }

        /// <summary>
        /// Checks the registros.
        /// </summary>
        private void CheckRegistros()
        {
            if (_arrayRegistros.Count >= 1)
            {
                //No permitir modificar datos
                DDTipoComite.Enabled = false;
                DDTipo_Proceso.Enabled = false;
                TextboxIdentificador.ReadOnly = true;
            }
            else
            {
                DDTipoComite.Enabled = true;
                DDTipo_Proceso.Enabled = true;
                TextboxIdentificador.ReadOnly = false;
            }
        }

        /// <summary>
        /// Crear_tabla_s the ine.
        /// </summary>
        private void crear_tabla_INE()
        {
            if (_identificadoresTable.Columns.Count == 0)
            {
                _identificadoresTable.Columns.Add(new DataColumn("No", typeof(string)));
                _identificadoresTable.Columns.Add(new DataColumn("Identificador", typeof(string)));
                _identificadoresTable.Columns.Add(new DataColumn("Estado", typeof(string)));
                _identificadoresTable.Columns.Add(new DataColumn("Prefijo", typeof(string)));
                /*Agregar Columnas Tipo Proceso, Tipo Comité, Ambito al gridview*/
                _identificadoresTable.Columns.Add(new DataColumn("Tipo Proceso", typeof(string)));
                _identificadoresTable.Columns.Add(new DataColumn("Tipo Comité", typeof(string)));
                _identificadoresTable.Columns.Add(new DataColumn("Ámbito", typeof(string)));
                _identificadoresTable.Columns.Add(new DataColumn("IdContabilidad", typeof(string)));
            }
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the DDTipo_Proceso control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void DDTipo_Proceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextboxIdentificador.Text = "";
            DDTipoComite.SelectedValue = "";
            DDAmbito.SelectedValue = "";
            DDEstado.SelectedValue = "";
            if (DDTipo_Proceso.SelectedValue == "Ordinario")
            {
                //COMITE
                divComite.Visible = true;
                divComite2.Visible = true;
                //AMBITO
                divAmbito.Visible = false;
                divAmbito2.Visible = false;
                //ENTIDAD
                divEntidad.Visible = false;
                divEntidad2.Visible = false;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = false;
                //BOTON
                LnkBAgregarIdentificador.Visible = false;
                //IDCONTABILIDAD
                divIdConta.Visible = false;
                divIdConta2.Visible = false;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = false;
                divIdContaG2.Visible = false;
            }
            else if (DDTipo_Proceso.SelectedValue == "Campaña")
            {
                //COMITE
                divComite.Visible = false;
                divComite2.Visible = false;
                //AMBITO
                divAmbito.Visible = true;
                divAmbito2.Visible = true;
                //ENTIDAD
                divEntidad.Visible = true;
                divEntidad2.Visible = true;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = true;
                //BOTON
                LnkBAgregarIdentificador.Visible = true;
                //IDCONTABILIDAD
                divIdConta.Visible = true;
                divIdConta2.Visible = true;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = false;
                divIdContaG2.Visible = false;
            }
            else if (DDTipo_Proceso.SelectedValue == "Precampaña")
            {
                //COMITE
                divComite.Visible = false;
                divComite2.Visible = false;
                //AMBITO
                divAmbito.Visible = true;
                divAmbito2.Visible = true;
                //ENTIDAD
                divEntidad.Visible = true;
                divEntidad2.Visible = true;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = true;
                //BOTON
                LnkBAgregarIdentificador.Visible = true;
                //IDCONTABILIDAD
                divIdConta.Visible = true;
                divIdConta2.Visible = true;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = false;
                divIdContaG2.Visible = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkHabilitarINE control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkHabilitarINE_CheckedChanged(object sender, EventArgs e)
        {
            divIne.Visible = chkHabilitarINE.Checked;
            DDTipo_Proceso_RequiredFieldValidator.Enabled = chkHabilitarINE.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the DDTipoComite control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void DDTipoComite_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextboxIdentificador.Text = "";
            DDAmbito.SelectedValue = "";
            DDEstado.SelectedValue = "";
            if (DDTipoComite.SelectedValue == "Ejecutivo Nacional" && DDTipo_Proceso.SelectedValue == "Ordinario")
            {
                //AMBITO
                divAmbito.Visible = false;
                divAmbito2.Visible = false;
                //ENTIDAD
                divEntidad.Visible = false;
                divEntidad2.Visible = false;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = false;
                //BOTON
                LnkBAgregarIdentificador.Visible = false;
                //IDCONTABILIDAD
                divIdConta.Visible = false;
                divIdConta2.Visible = false;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = true;
                divIdContaG2.Visible = true;
            }
            else if (DDTipoComite.SelectedValue == "Ejecutivo Estatal" && DDTipo_Proceso.SelectedValue == "Ordinario")
            {
                //AMBITO
                divAmbito.Visible = false;
                divAmbito2.Visible = false;
                //ENTIDAD
                divEntidad.Visible = true;
                divEntidad2.Visible = true;
                //GRIVIEW ENTIDADES
                gvRegistros.Visible = true;
                //BOTON
                LnkBAgregarIdentificador.Visible = true;
                //IDCONTABILIDAD
                divIdConta.Visible = true;
                divIdConta2.Visible = true;
                //IDCONTABILIDAD GENERAL
                divIdContaG.Visible = false;
                divIdContaG2.Visible = false;
            }
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the bGenerar1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bGenerar1_Click(object sender, EventArgs e)
        {
            #region Limpiar Extranet

            foreach (System.Web.UI.Control ctrl in UpdatePanel3.Controls[0].Controls)
            {
                try
                {

                    if (ctrl is TextBox)
                    {
                        ((TextBox)ctrl).Text = string.Empty;
                    }
                    else if (ctrl is DropDownList)
                    {
                        ((DropDownList)ctrl).SelectedValue = null;
                    }
                    else if (ctrl is GridView)
                    {
                        ((GridView)ctrl).DataSource = null;
                        ((GridView)ctrl).DataBind();
                    }
                }
                catch { }
            }

            #endregion
            if (string.IsNullOrEmpty(tbTicket.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, "El campo no puede estar vacio", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                return;
            }
            FillCatalogPaises();
            Session[ViewState["_PageID"].ToString() + "_trama"] = "";
            Session[ViewState["_PageID"].ToString() + "_arrayTrama"] = null;
            Session[ViewState["_PageID"].ToString() + "_idTrama"] = "";
            var regTrama = ConsultarTrama(tbTicket.Text);
            Session[ViewState["_PageID"].ToString() + "_idTrama"] = regTrama[0];
            var idTrama = Session[ViewState["_PageID"].ToString() + "_idTrama"].ToString();
            Session[ViewState["_PageID"].ToString() + "_trama"] = regTrama[1];
            if (!string.IsNullOrEmpty(Session[ViewState["_PageID"].ToString() + "_trama"].ToString()))
            {
                Session[ViewState["_PageID"].ToString() + "_trama"] = Session[ViewState["_PageID"].ToString() + "_trama"].ToString().Replace("====== Detalle", "================ Detalle");
            }
            if (!string.IsNullOrEmpty(idTrama) && !string.IsNullOrEmpty(Session[ViewState["_PageID"].ToString() + "_trama"].ToString()))
            {
                var numLetra = new NumerosALetras();
                if (Session[ViewState["_PageID"].ToString() + "_trama"].ToString().StartsWith("XXX", StringComparison.OrdinalIgnoreCase))
                {
                    #region MAITRE'D
                    // Trama de Maitre'D
                    ReadTramaMaitred(Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                    var fechaConsumo = GetValue("IdDoc", "FechaEmis");
                    if (ValidaFechaConsumo(fechaConsumo))
                    {
                        if (Regex.IsMatch(GetValue("ExEmisor", "RFCEmisor"), @"OHR980618BLA|OTS1001259H0|LAN7008173R5|SET920324FC3|OPL000131DL3"))
                        {
                            tbTotal.Text = GetValue("Totales", "TotalPagado");
                            tbTotalFac.Text = GetValue("Totales", "VlrPagar");
                            tbAmbiente.Text = "PRODUCCIÓN";
                            tbCodDoc.Text = "FACTURA";
                            tbISH.Text = "0.00";
                            tbIva16.Text = "0.00";
                            tbFormaPago.Text = GetValue("IdDoc", "FormaPago");
                            tbSubtotal.Text = GetValue("Totales", "Subtotal");
                            tbPropina.Text = GetValue("Totales", "Propina");
                            tbOtrosCargos.Text = "";
                            tbLugarExp.Text = GetValue("ExEmisorDomFiscal", "Pais") + ", " + GetValue("ExEmisorDomFiscal", "Estado");
                            tbCantLetra.Text = numLetra.ConvertirALetras(GetValue("Totales", "TotalPagado"), GetValue("Totales", "Moneda"));
                            rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                            trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());
                            try { ddlUsoCfdi.SelectedValue = GetValue("IdDoc", "UsoCfdi"); } catch { }
                            for (int i = 1; i <= 5; i++)
                            {
                                var tipoImpuesto = GetValue("ExImpuestos", "TiposImp" + i);
                                var tasaImpuesto = GetValue("ExImpuestos", "TasaImp" + i);
                                var montoImpuesto = GetValue("ExImpuestos", "MontoImp" + i);
                                if (!string.IsNullOrEmpty(tipoImpuesto))
                                {
                                    switch (tipoImpuesto)
                                    {
                                        case "IVA":
                                            tbIva16.Text = montoImpuesto;
                                            break;
                                        case "ISH":
                                            lblISHPrer.Text = tasaImpuesto;
                                            tbISH.Text = montoImpuesto;
                                            trISProp.Visible = true;
                                            break;
                                    }
                                }
                            }

                            try
                            {
                                var dataTableConceptos = new DataTable();
                                if (dataTableConceptos.Columns.Count == 0)
                                {
                                    dataTableConceptos.Columns.Add("cantidad", typeof(string));
                                    dataTableConceptos.Columns.Add("descripcion", typeof(string));
                                    dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                                    dataTableConceptos.Columns.Add("importe", typeof(string));
                                    dataTableConceptos.Columns.Add("unidad", typeof(string));
                                }
                                foreach (var concepto in _detalles)
                                {
                                    var newRow = dataTableConceptos.NewRow();
                                    newRow[0] = concepto["QtyItem"]; // CANTIDAD
                                    newRow[1] = concepto["DscItem"]; // DESCRIPCION
                                    newRow[2] = concepto["PrcNetoItem"]; // VAL. UNITARIO
                                    if (Session["CfdiVersion"].ToString().Equals("3.3"))
                                    {
                                        decimal cantidad = 0;
                                        decimal precioNeto = 0;
                                        decimal.TryParse(concepto["QtyItem"], out cantidad);
                                        decimal.TryParse(concepto["PrcNetoItem"], out precioNeto);
                                        decimal valorUnitario = precioNeto / cantidad;
                                        newRow[2] = valorUnitario.ToString(); // VAL. UNITARIO
                                    }
                                    newRow[3] = concepto["MontoNetoItem"]; // IMPORTE
                                    newRow[4] = concepto["UnMdItem"]; // UNIDAD
                                    dataTableConceptos.Rows.Add(newRow);
                                }
                                Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] = dataTableConceptos;
                                gvConceptos.DataSource = dataTableConceptos;
                                gvConceptos.DataBind();
                            }
                            catch { }
                        }
                        else if (GetValue("ExEmisor", "RFCEmisor").Equals("OAL111108K5A", StringComparison.OrdinalIgnoreCase))
                        {
                            var desgloce = true;
                            if (desgloce)
                            {
                                var granTotal = Convert.ToDecimal(GetValue("Personalizados", "MontoMasPropina"));
                                var propina = Convert.ToDecimal(GetValue("Personalizados", "MontoPropina"));
                                var impuestos = Convert.ToDecimal(GetValue("Totales", "MntImp"));
                                var totalFactura = granTotal - propina;
                                var subtotal = totalFactura - impuestos;
                                SetValue("Totales", "VlrPagar", CerosNull(totalFactura.ToString()), true);
                                SetValue("Totales", "SubTotal", CerosNull(subtotal.ToString()));
                            }
                            tbTotalFac.Text = GetValue("Totales", "VlrPagar");
                            tbTotal.Text = GetValue("Personalizados", "MontoMasPropina");
                            tbAmbiente.Text = "PRODUCCIÓN";
                            tbCodDoc.Text = "FACTURA";
                            tbISH.Text = "0.00";
                            tbIva16.Text = "0.00";
                            tbFormaPago.Text = GetValue("IdDoc", "FormaPago");
                            string seriefolioM= GetValue("IdDoc", "Serie+Folio");
                           
                            tbSubtotal.Text = GetValue("Totales", "SubTotal");
                            tbPropina.Text = GetValue("Personalizados", "MontoPropina");
                            tbOtrosCargos.Text = "";
                            tbLugarExp.Text = GetValue("ExEmisorDomFiscal", "Pais") + ", " + GetValue("ExEmisorDomFiscal", "Estado");
                            tbCantLetra.Text = numLetra.ConvertirALetras(GetValue("Personalizados", "MontoMasPropina"), GetValue("Totales", "Moneda"));
                            rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                            trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());
                            tbNumCtaPago.Text = GetValue("Personalizados", "CtaPagoCli");
                            try { ddlUsoCfdi.SelectedValue = GetValue("IdDoc", "UsoCfdi"); } catch { }
                            try
                            {
                                var primerMetodo = GetValue("Personalizados", "MedioPago").Split(',').FirstOrDefault();
                                ddlMetodoPago.SelectedValue = primerMetodo;
                                ddlMetodoPago.Enabled = false;
                            }
                            catch { }
                            var tipoImpuesto = GetValue("ExImpuestos", "TipoImp");
                            var tasaImpuesto = GetValue("ExImpuestos", "PctImp");
                            var montoImpuesto = GetValue("ExImpuestos", "MntImp");
                            switch (tipoImpuesto)
                            {
                                case "IVA":
                                    tbIva16.Text = montoImpuesto;
                                    break;
                                case "ISH":
                                    lblISHPrer.Text = tasaImpuesto;
                                    tbISH.Text = montoImpuesto;
                                    trISProp.Visible = true;
                                    break;
                            }

                            try
                            {
                                var dataTableConceptos = new DataTable();
                                if (dataTableConceptos.Columns.Count == 0)
                                {
                                    dataTableConceptos.Columns.Add("cantidad", typeof(string));
                                    dataTableConceptos.Columns.Add("descripcion", typeof(string));
                                    dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                                    dataTableConceptos.Columns.Add("importe", typeof(string));
                                    dataTableConceptos.Columns.Add("unidad", typeof(string));
                                }
                                foreach (var concepto in _detalles)
                                {
                                    decimal monto = 0;
                                    decimal iva = 0;
                                    decimal.TryParse(concepto["MontoTotalItem"], out monto);
                                    decimal.TryParse(concepto["MontoImp1"], out iva);
                                    decimal montoTotal = monto - iva;
                                    decimal subTotal = 0;
                                    decimal.TryParse(GetValue("Totales", "SubTotal"), out subTotal);
                                    if (montoTotal != subTotal && _detalles.Count == 1)
                                    {
                                        montoTotal = subTotal;
                                    }
                                    var newRow = dataTableConceptos.NewRow();
                                    newRow[0] = concepto["QtyItem"]; // CANTIDAD
                                    newRow[1] = concepto["DscItem"]; // DESCRIPCION
                                    newRow[2] = CerosNull(montoTotal.ToString()); // VAL. UNITARIO
                                    newRow[3] = CerosNull(montoTotal.ToString()); // IMPORTE
                                    newRow[4] = concepto["Unmd"]; // UNIDAD
                                    dataTableConceptos.Rows.Add(newRow);
                                }
                                Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] = dataTableConceptos;
                                gvConceptos.DataSource = dataTableConceptos;
                                gvConceptos.DataBind();
                            }
                            catch { }
                        }
                        ddlMetodoPago_SelectedIndexChanged(null, null);
                        if (GetValue("ExEmisor", "RFCEmisor").Equals("OAL111108K5A", StringComparison.OrdinalIgnoreCase))
                        { tbNumCtaPago.ReadOnly = !string.IsNullOrEmpty(tbNumCtaPago.Text.Replace("_", "")); }
                        ScriptManager.RegisterStartupScript(this, GetType(), "_keyExtranet", "$('#" + progressCrear.ClientID + "').css('display', 'none'); $('#ModuloExtranet').modal('show');", true);
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El periodo de generación de su factura ha caducado", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                    #endregion
                }
                else if (Session[ViewState["_PageID"].ToString() + "_trama"].ToString().StartsWith("T|", StringComparison.OrdinalIgnoreCase))
                {
                    #region MICROS
                    // Trama de Micros
                    ReadTramaMicros(Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                    var trama = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]);
                    var _isOneItem = (bool)(Session[ViewState["_PageID"].ToString() + "_isOneItem"]);

                    #region Condicion de restriccion de tickets

                    var restriccion = false;
                    if (Session["IDENTEMIEXT"].ToString().Equals("HSL130116EN5"))
                    {
                        try
                        {
                            if (Session["CfdiVersion"].ToString().Equals("3.3") && !_isOneItem)
                            {
                                restriccion = trama.Resumen33 != null && !string.IsNullOrEmpty(trama.Resumen33.CurrencyPayMethodRoomCharge) && trama.Resumen33.CurrencyPayMethodRoomCharge.Split(';')[3].Equals("RoomChg", StringComparison.OrdinalIgnoreCase);
                            }
                            else
                            {
                                restriccion = ((trama.Resumen != null && !string.IsNullOrEmpty(trama.Resumen.Pago) && trama.Resumen.Pago.Equals("RoomChg", StringComparison.OrdinalIgnoreCase)) || (trama.Pagos != null && trama.Pagos.Any() && trama.Pagos.Any(x => x.Pagop.Equals("Room Charge", StringComparison.OrdinalIgnoreCase))));
                            }
                        }
                        catch { }
                    }
                    if (restriccion)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "[Room Charge] El ticket no puede ser facturado.", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        return;
                    }

                    #endregion

                    if (Session["CfdiVersion"].ToString().Equals("3.3") && !_isOneItem)
                    {
                        #region CFDI 3.3

                        decimal _subTotal = 0;
                        decimal _iva16 = 0;
                        decimal _totalFac = 0;
                        decimal _propina = 0;
                        decimal _otrosCargos = 0;
                        decimal _aPagar = 0;
                        decimal _descuentos = 0;

                        var fechaConsumo = trama.Resumen33.TransactionDate;
                        var sucursal = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Resumen33.PropertyName;
                        var sucEmi = ddlSucursalEmisor.SelectedValue;
                        var claveSucursal = ddlSucursalEmisor.SelectedItem.Text.Split(':').Last().Trim();
                        if (sucursal.Equals(claveSucursal, StringComparison.OrdinalIgnoreCase))
                        {
                            if (ValidaFechaConsumo(fechaConsumo))
                            {
                                _db = new BasesDatos(Session["IDENTEMIEXT"].ToString());
                                _db.Conectar();
                                _db.CrearComando(@"select codigoPostal FROM Cat_SucursalesEmisor where clave=@sucursal");

                                _db.AsignarParametroCadena("@sucursal", sucursal);
                                var dr = _db.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    tbLugarExp.Text = dr[0].ToString();
                                }
                                _db.Desconectar();

                                var descripcion = "Ticket " + ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Resumen33.CurrentReferenceNumber;
                                tbTotalFac.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.GrossAmount)).ToString();
                                tbTotal.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Resumen33.GrossAmountGroupedP;
                                tbAmbiente.Text = "PRODUCCIÓN";
                                tbCodDoc.Text = "FACTURA";
                                tbISH.Text = "0.00";
                                tbIva16.Text = "0.00";
                                tbFormaPago.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Resumen33.CurrencyPayMethodRoomCharge.Split(';')[1];
                                var subTotal = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString();
                                var descuento = "0.00";
                                if (((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Descuentos33 != null)
                                {
                                    descuento = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Descuentos33.Sum(x => Convert.ToDecimal(x.NetAmount.Replace("-", ""))).ToString();
                                }
                                if (((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Propinas33 == null)
                                {
                                    tbPropina.Text = "0.0";
                                }
                                else
                                {
                                    tbPropina.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Propinas33.NetAmount;
                                }

                                tbOtrosCargos.Text = "";

                                tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");
                                rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                                trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());

                                try
                                {
                                    tbIva16.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Impuestos33.Sum(x => Convert.ToDecimal(x.GrossAmount.Replace("-", ""))).ToString();
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
                                decimal.TryParse(tbIva16.Text, out dImpuestos);
                                decimal.TryParse(tbPropina.Text, out dPropinas);
                                tbSubtotal.Text = dSubTotal.ToString();
                                dTotalFactura = dSubTotal - dDescuento + dImpuestos;
                                dTotalPagar = dTotalFactura + dPropinas;
                                tbTotalFac.Text = dTotalFactura.ToString();
                                tbTotal.Text = dTotalPagar.ToString();
                                tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");

                                try
                                {
                                    var dataTableConceptos = new DataTable();
                                    if (dataTableConceptos.Columns.Count == 0)
                                    {
                                        dataTableConceptos.Columns.Add("cantidad", typeof(string));
                                        dataTableConceptos.Columns.Add("cveProdServ", typeof(string));
                                        dataTableConceptos.Columns.Add("descripcion", typeof(string));
                                        dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                                        dataTableConceptos.Columns.Add("importe", typeof(string));
                                        dataTableConceptos.Columns.Add("unidad", typeof(string));
                                    }
                                    /*foreach (var concepto in ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33)
                                    {*/
                                    var newRow = dataTableConceptos.NewRow();
                                    newRow[0] = "1"; // CANTIDAD
                                    newRow[1] = "NO APLICA"; // CLAVE PRODUCTO/SERVICIO
                                    newRow[2] = descripcion.ToString(); // DESCRIPCION
                                    newRow[3] = dSubTotal.ToString(); // VAL. UNITARIO
                                    newRow[4] = dSubTotal.ToString(); // IMPORTE
                                    newRow[5] = "NO APLICA"; // UNIDAD
                                    dataTableConceptos.Rows.Add(newRow);
                                    /*}*/
                                    //var das = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33;
                                    //foreach (var tramaMicros in ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33)
                                    //{
                                    //    string descripcion = "";
                                    //    decimal esubTotal = 0;
                                    //    decimal iva = 0;
                                    //    decimal totalFac = 0;
                                    //    decimal propina = 0;
                                    //    decimal otrosCargos = 0;
                                    //    decimal aPagar = 0;
                                    //    decimal descuentos = 0;

                                    //    try { descripcion = "Ticket " + tramaMicros.CurrentReferenceNumber; } catch { }
                                    //    try { esubTotal = Convert.ToDecimal(((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString()); } catch { }
                                    //    //  try { subTotal = tramaMicros.Detalles.Sum(x => Convert.ToDecimal(x.MontoNetoAgrupado)); } catch { }
                                    //    try { iva = Convert.ToDecimal(((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString()); } catch { }
                                    //    try { totalFac = Convert.ToDecimal(((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString()); } catch { }
                                    //    // try { propina = Convert.ToDecimal(tramaMicros.Propinas != null ? tramaMicros.Propinas.MontoNetos : "0.00"); } catch { }
                                    //    try { propina = Convert.ToDecimal(((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount != null ? tramaMicros.GrossAmount : "0.00")).ToString()); } catch { }
                                    //    //try { otrosCargos = Convert.ToDecimal("0.00"); } catch { }
                                    //    try { otrosCargos = Convert.ToDecimal("0.00"); } catch { }
                                    //    //try { aPagar = Convert.ToDecimal(tramaMicros.Resumen.MontoBruto); } catch { }
                                    //    try { aPagar = Convert.ToDecimal(((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString()); } catch { }
                                    //    //{ try { descuentos = tramaMicros.Descuentos.Sum(x => Convert.ToDecimal(x.MontoNetod, new CultureInfo("en-US"))); } catch { } }
                                    //    { try { descuentos = Convert.ToDecimal(((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount, new CultureInfo("en-US"))).ToString()); } catch { } }
                                    //    //try { MontoNeto = Convert.ToDecimal(tramaMicros.Resumen.MontoNeto); } catch { }

                                    //    //   try { MontoNeto = Convert.ToDecimal(((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles33.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString()); } catch { }

                                    //    _subTotal += esubTotal;
                                    //    _iva16 += iva;
                                    //    //_totalFac += totalFac;
                                    //    //  _totalFac += MontoNeto;
                                    //    _propina += propina;
                                    //    _otrosCargos += otrosCargos;
                                    //    _aPagar += aPagar;
                                    //    _descuentos += descuentos;

                                    //    var NewRow = dataTableConceptos.NewRow();
                                    //    NewRow[0] = "1"; // CANTIDAD
                                    //    NewRow[1] = descripcion; // DESCRIPCION
                                    //    NewRow[2] = subTotal.ToString(); // VAL. UNITARIO
                                    //    NewRow[3] = iva.ToString(); // IVA
                                    //                                //   NewRow[4] = MontoNeto.ToString(); // IMPORTE
                                    //    NewRow[5] = propina.ToString(); // PROPINA
                                    //    NewRow[6] = aPagar.ToString(); // TOTAL
                                    //    NewRow[7] = "NO APLICA"; // UNIDAD
                                    //    NewRow[8] = descuentos; // DESCUENTOS
                                    //    dataTableConceptos.Rows.Add(NewRow);
                                    //}

                                    Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] = dataTableConceptos;
                                    gvConceptos.DataSource = dataTableConceptos;
                                    gvConceptos.DataBind();
                                }
                                catch { }
                                ddlMetodoPago_SelectedIndexChanged(null, null);
                                ScriptManager.RegisterStartupScript(this, GetType(), "_keyExtranet", "$('#" + progressCrear.ClientID + "').css('display', 'none'); $('#ModuloExtranet').modal('show');", true);
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "El periodo de generación de su factura ha caducado", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                            }
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "El ticket no pertenece a la sucursal " + ddlSucursalEmisor.SelectedItem.Text, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }

                        #endregion
                    }
                    else
                    {
                        #region CFDI 3.2

                        var fechaConsumo = trama.Resumen.FechaTrans;
                        var sucursal = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Resumen.NombrePropiedad;
                        var sucEmi = ddlSucursalEmisor.SelectedValue;
                        var claveSucursal = ddlSucursalEmisor.SelectedItem.Text.Split(':').First().Trim();
                        if (sucursal.Equals(claveSucursal, StringComparison.OrdinalIgnoreCase))
                        {
                            if (ValidaFechaConsumo(fechaConsumo))
                            {
                                _db = new BasesDatos(Session["IDENTEMIEXT"].ToString());
                                _db.Conectar();
                                _db.CrearComando(@"select RFCEMI,NOMEMI,dirMatriz,noExterior,noInterior,colonia,referencia,municipio,
                                 estado,pais,codigoPostal,regimenFiscal FROM Cat_Emisor where RFCEMI=@RFCEMI");

                                _db.AsignarParametroCadena("@RFCEMI", Session["IDENTEMIEXT"].ToString());
                                var dr = _db.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    tbLugarExp.Text = dr[8].ToString() + "," + dr[9].ToString();
                                }
                                _db.Desconectar();

                                tbTotalFac.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles.Sum(x => Convert.ToDecimal(x.MontoBrutoagrupadom)).ToString();
                                tbTotal.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Resumen.MontoBruto;
                                tbAmbiente.Text = "PRODUCCIÓN";
                                tbCodDoc.Text = "FACTURA";
                                tbISH.Text = "0.00";
                                tbIva16.Text = "0.00";
                                tbFormaPago.Text = "PAGO EN UNA SOLA EXIBICION";

                                var subTotal = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles.Sum(x => Convert.ToDecimal(x.MontoNetoAgrupado)).ToString();
                                var descuento = "0.00";
                                if (((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Descuentos != null)
                                {
                                    descuento = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Descuentos.Sum(x => Convert.ToDecimal(x.MontoNetod.Replace("-", ""))).ToString();
                                }
                                if (((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Propinas == null)
                                {
                                    tbPropina.Text = "0.0";
                                }
                                else
                                {
                                    tbPropina.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Propinas.MontoNetos;
                                }

                                tbOtrosCargos.Text = "";

                                tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");
                                rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                                trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());

                                try
                                {
                                    tbIva16.Text = ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Impuestos.Sum(x => Convert.ToDecimal(x.MontoBrutoi.Replace("-", ""))).ToString();
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
                                decimal.TryParse(tbIva16.Text, out dImpuestos);
                                decimal.TryParse(tbPropina.Text, out dPropinas);
                                tbSubtotal.Text = dSubTotal.ToString();
                                dTotalFactura = dSubTotal - dDescuento + dImpuestos;
                                dTotalPagar = dTotalFactura + dPropinas;
                                tbTotalFac.Text = dTotalFactura.ToString();
                                tbTotal.Text = dTotalPagar.ToString();
                                tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");

                                try
                                {
                                    var dataTableConceptos = new DataTable();
                                    if (dataTableConceptos.Columns.Count == 0)
                                    {
                                        dataTableConceptos.Columns.Add("cantidad", typeof(string));
                                        dataTableConceptos.Columns.Add("descripcion", typeof(string));
                                        dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                                        dataTableConceptos.Columns.Add("importe", typeof(string));
                                        dataTableConceptos.Columns.Add("unidad", typeof(string));
                                    }
                                    foreach (var concepto in ((TramaMicros)Session[ViewState["_PageID"].ToString() + "_tramaMicros"]).Detalles)
                                    {
                                        var newRow = dataTableConceptos.NewRow();
                                        newRow[0] = concepto.Cantmov; // CANTIDAD
                                        newRow[1] = concepto.Detallem; // DESCRIPCION
                                        newRow[2] = concepto.Preciounitario; // VAL. UNITARIO
                                        newRow[3] = concepto.MontoNetoAgrupado; // IMPORTE
                                        newRow[4] = "NO APLICA"; // UNIDAD
                                        dataTableConceptos.Rows.Add(newRow);
                                    }
                                    Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] = dataTableConceptos;
                                    gvConceptos.DataSource = dataTableConceptos;
                                    gvConceptos.DataBind();
                                }
                                catch { }
                                ddlMetodoPago_SelectedIndexChanged(null, null);
                                ScriptManager.RegisterStartupScript(this, GetType(), "_keyExtranet", "$('#" + progressCrear.ClientID + "').css('display', 'none'); $('#ModuloExtranet').modal('show');", true);
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "El periodo de generación de su factura ha caducado", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                            }
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "El ticket no pertenece a la sucursal " + ddlSucursalEmisor.SelectedItem.Text, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }

                        #endregion
                    }
                    #endregion
                }
                else if (Session[ViewState["_PageID"].ToString() + "_trama"].ToString().StartsWith("OPXXX", StringComparison.OrdinalIgnoreCase))
                {
                    #region OPERA
                    ReadTramaOpera(Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                    var fechaConsumoOp = _fechaemision;

                    if (ValidaFechaConsumo(fechaConsumoOp))
                    {
                        if (Regex.IsMatch(_rfc, @"OHR980618BLA|OTS1001259H0|LAN7008173R5|SET920324FC3|OPL000131DL3"))
                        {
                            tbTotal.Text = _totalRef;
                            tbTotalFac.Text = _vlrPagar;
                            tbAmbiente.Text = "PRODUCCIÓN";
                            tbCodDoc.Text = "FACTURA";
                            tbISH.Text = "0.00";
                            tbIva16.Text = "0.00";
                            tbFormaPago.Text = _formapago;
                            tbSubtotal.Text = _subtotal;
                            tbPropina.Text = _propinas;
                            tbOtrosCargos.Text = "";
                            tbLugarExp.Text = _pais + ", " + _estado;
                            tbCantLetra.Text = numLetra.ConvertirALetras(_vlrPagar, _moneda);
                            rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                            trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());
                            if (string.IsNullOrEmpty(_numerocuentapago.Trim()))
                            {
                                tbNumCtaPago.Text = "NO IDENTIFICADO";
                            }
                            else
                            {
                                tbNumCtaPago.Text = _numerocuentapago;
                            }

                            if (_tipoimpuesto1.Equals("ISH", StringComparison.OrdinalIgnoreCase))
                            {
                                lblISHPrer.Text = _tasaimpuesto1;
                                tbISH.Text = _montoimpuesto1;
                            }
                            else if (_tipoimpuesto2.Equals("ISH", StringComparison.OrdinalIgnoreCase))
                            {
                                lblISHPrer.Text = _tasaimpuesto2;
                                tbISH.Text = _montoimpuesto2;
                            }
                            else if (_tipoimpuesto3.Equals("ISH", StringComparison.OrdinalIgnoreCase))
                            {
                                lblISHPrer.Text = _tasaimpuesto3;
                                tbISH.Text = _montoimpuesto3;
                            }
                            else
                            {
                                lblISHPrer.Text = _tasaimpuesto1;
                                tbISH.Text = _montoimpuesto1;
                            }

                            tbIva16.Text = _montoiva;
                            trISProp.Visible = true;

                            try
                            {
                                var dataTableConceptos = new DataTable();
                                if (dataTableConceptos.Columns.Count == 0)
                                {
                                    dataTableConceptos.Columns.Add("cantidad", typeof(string));
                                    dataTableConceptos.Columns.Add("descripcion", typeof(string));
                                    dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                                    dataTableConceptos.Columns.Add("importe", typeof(string));
                                    dataTableConceptos.Columns.Add("unidad", typeof(string));
                                }

                                foreach (var concepto in _detalles)
                                {
                                    var descripcion = concepto["Descripcion"];
                                    if (_rfc.Equals("OAL111108K5A")) { descripcion = Regex.Replace(descripcion.Trim(), @" {2,}\(\s*\d+\)", "", RegexOptions.IgnoreCase); }
                                    var newRow = dataTableConceptos.NewRow();
                                    newRow[0] = concepto["Cantidad"]; // CANTIDAD
                                    newRow[1] = descripcion; // DESCRIPCION
                                    newRow[2] = concepto["Precio Neto"]; // VAL. UNITARIO
                                    newRow[3] = concepto["Monto Neto"]; // IMPORTE
                                    newRow[4] = concepto["Unidad de Medida"]; // UNIDAD
                                    dataTableConceptos.Rows.Add(newRow);
                                }

                                Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] = dataTableConceptos;
                                gvConceptos.DataSource = dataTableConceptos;
                                gvConceptos.DataBind();
                            }
                            catch (Exception ex) { }

                            //region receptor
                            tbRfcRec.Text = _rfcre;
                            tbRazonSocialRec.Text = _nombrere;
                            tbCalleRec.Text = _rdfcalle;
                            tbNoExtRec.Text = _rdfnoexterior;
                            tbNoIntRec.Text = _rdfnointerior;
                            tbColoniaRec.Text = _rdfcolonia;
                            tbMunicipioRec.Text = _rdfmunicipio;
                            tbEstadoRec.Text = _rdfestado;
                            tbCpRec.Text = _rdfcodigopostal;
                            tbPaisRec.Text = _rdfpais;
                            tbMailReceptor.Text = _email;

                            tbRfcRec.ReadOnly = true;
                            tbRazonSocialRec.ReadOnly = true;
                            tbCalleRec.ReadOnly = true;
                            tbNoExtRec.ReadOnly = true;
                            tbNoIntRec.ReadOnly = true;
                            tbColoniaRec.ReadOnly = true;
                            tbMunicipioRec.ReadOnly = true;
                            tbEstadoRec.ReadOnly = true;
                            tbCpRec.ReadOnly = true;
                            tbPaisRec.ReadOnly = true;
                            ddlUsoCfdi.Enabled = true;
                            // tbEmail.ReadOnly = true;

                        }
                        ddlMetodoPago_SelectedIndexChanged(null, null);
                        //if (GetValue("ExEmisor", "RFCEmisor").Equals("OAL111108K5A", StringComparison.OrdinalIgnoreCase))
                        //{ tbNumCtaPago.ReadOnly = !string.IsNullOrEmpty(tbNumCtaPago.Text.Replace("_", "")); }
                        ScriptManager.RegisterStartupScript(this, GetType(), "_keyExtranet", "$('#" + progressCrear.ClientID + "').css('display', 'none'); $('#ModuloExtranet').modal('show');", true);
                    }
                    #endregion
                }
                else
                {
                    try
                    {
                        var xDoc = new XmlDocument();
                        xDoc.LoadXml(Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                        var nodos = xDoc.ChildNodes.Cast<XmlNode>().ToList();
                        if (nodos.Any(n => n.Name.Equals("documentoFiscalmicros", StringComparison.OrdinalIgnoreCase)))
                        {
                            #region tramaHpresidente
                            // Trama Micros 2
                            ReadTramaMicrosPresidente(Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                            var trama = ((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]);
                            var fechaConsumo = trama.Resumen.FechaTrans;
                            var sucursal = ((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]).Resumen.NombrePropiedad;
                            var sucEmi = ddlSucursalEmisor.SelectedValue;
                            var claveSucursal = ddlSucursalEmisor.SelectedItem.Text.Split(':').First().Trim();
                            //termina Trama Micros 2
                            //if
                            if (sucursal.Equals(claveSucursal, StringComparison.OrdinalIgnoreCase))
                            {
                                if (ValidaFechaConsumo(fechaConsumo))
                                {
                                    _db = new BasesDatos(Session["IDENTEMIEXT"].ToString());
                                    _db.Conectar();
                                    _db.CrearComando(@"select RFCEMI,NOMEMI,dirMatriz,noExterior,noInterior,colonia,referencia,municipio,
                                 estado,pais,codigoPostal,regimenFiscal FROM Cat_Emisor where RFCEMI=@RFCEMI");

                                    _db.AsignarParametroCadena("@RFCEMI", Session["IDENTEMIEXT"].ToString());
                                    var dr = _db.EjecutarConsulta();
                                    if (dr.Read())
                                    {
                                        tbLugarExp.Text = dr[8].ToString() + "," + dr[9].ToString();
                                    }
                                    _db.Desconectar();

                                    tbTotalFac.Text = ((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]).Detalles.MontoBrutoagrupadom.ToString();
                                    tbTotal.Text = ((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]).Resumen.MontoBruto;
                                    tbAmbiente.Text = "PRODUCCIÓN";
                                    tbCodDoc.Text = "FACTURA";
                                    tbISH.Text = "0.00";
                                    tbIva16.Text = trama.Impuesto.Importe;
                                    tbFormaPago.Text = "PAGO EN UNA SOLA EXIBICION";
                                    tbSubtotal.Text = ((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]).Detalles.MontoNetoAgrupado.ToString();
                                    if (((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]).Propina == null)
                                    {
                                        tbPropina.Text = "0.0";
                                    }
                                    else
                                    {
                                        tbPropina.Text = ((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]).Propina.Propina;
                                    }

                                    tbOtrosCargos.Text = "";

                                    tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");
                                    rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                                    trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());

                                    try
                                    {
                                        tbIva16.Text = string.Join(",", ((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]).Impuestos.Select(x => x.MontoNetoi));
                                    }
                                    catch { }

                                    try
                                    {
                                        var dataTableConceptos = new DataTable();
                                        if (dataTableConceptos.Columns.Count == 0)
                                        {
                                            dataTableConceptos.Columns.Add("cantidad", typeof(string));
                                            dataTableConceptos.Columns.Add("descripcion", typeof(string));
                                            dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                                            dataTableConceptos.Columns.Add("importe", typeof(string));
                                            dataTableConceptos.Columns.Add("unidad", typeof(string));
                                        }
                                        foreach (var concepto in ((TramaHpresidente)Session[ViewState["_PageID"].ToString() + "_tramaMicrosPresidente"]).Producto)
                                        {

                                            _db.Conectar();
                                            _db.CrearComando(@"select idProducto,descripcion,unidad from Cat_Producto where idProducto = @IDPROD and sociedad = @SOCIEDAD");
                                            _db.AsignarParametroCadena("@IDPROD", concepto.Prod);
                                            _db.AsignarParametroCadena("@SOCIEDAD", trama.Clave.Sociedad);
                                            dr = _db.EjecutarConsulta();
                                            if (dr.Read())
                                            {
                                                concepto.Descripcion = dr[1].ToString();
                                                concepto.Unidad = dr[2].ToString();


                                            }
                                            _db.Desconectar();

                                            var newRow = dataTableConceptos.NewRow();
                                            newRow[0] = concepto.Cantfact; // CANTIDAD
                                            newRow[1] = concepto.Descripcion; // DESCRIPCION
                                            newRow[2] = trama.Detalles.MontoNetoAgrupado; // VAL. UNITARIO  preguntar al cliente
                                            newRow[3] = concepto.Importneto; // IMPORTE
                                            newRow[4] = concepto.Unidad; // UNIDAD
                                            dataTableConceptos.Rows.Add(newRow);
                                        }
                                        Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] = dataTableConceptos;
                                        gvConceptos.DataSource = dataTableConceptos;
                                        gvConceptos.DataBind();
                                    }
                                    catch { }
                                    ddlMetodoPago_SelectedIndexChanged(null, null);
                                    ScriptManager.RegisterStartupScript(this, GetType(), "_keyExtranet", "$('#" + progressCrear.ClientID + "').css('display', 'none'); $('#ModuloExtranet').modal('show');", true);
                                }
                                else
                                {
                                    (Master as SiteMaster).MostrarAlerta(this, "El periodo de generación de su factura ha caducado", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                                }
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "El ticket no pertenece a la sucursal " + ddlSucursalEmisor.SelectedItem.Text, 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                            }
                            //
                            #endregion tramaHpresidente
                        }
                        else if (nodos.Any(n => n.Name.Equals("Transacction", StringComparison.OrdinalIgnoreCase)))
                        {
                            #region Trama Aloha

                            ReadTramaAloha(Session[ViewState["_PageID"].ToString() + "_trama"].ToString());
                            var trama = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]);

                            #region CFDI 3.3

                            var fechaConsumo = trama.DOB;
                            var sucursal = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).RevenueCenter;
                            var sucEmi = ddlSucursalEmisor.SelectedValue;
                            var claveSucursal = ddlSucursalEmisor.SelectedItem.Text.Split(':').First().Trim();
                            if (ValidaFechaConsumo(fechaConsumo))
                            {
                                _db = new BasesDatos(Session["IDENTEMIEXT"].ToString());
                                _db.Conectar();
                                _db.CrearComando(@"select codigoPostal FROM Cat_SucursalesEmisor where clave=@sucursal");

                                _db.AsignarParametroCadena("@sucursal", sucursal);
                                var dr = _db.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    tbLugarExp.Text = dr[0].ToString();
                                }
                                _db.Desconectar();

                                tbTotalFac.Text = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).Total;
                                tbTotal.Text = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).GrantTotal;
                                tbAmbiente.Text = "PRODUCCIÓN";
                                tbCodDoc.Text = "FACTURA";
                                tbISH.Text = "0.00";
                                tbIva16.Text = "0.00";
                                tbFormaPago.Text = "PAGO EN UNA SOLA EXHIBICION";
                                var subTotal = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).SubTotal;
                                var descuento = "0.00";
                                if (((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).Comp != null)
                                {
                                    descuento = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).Comp.Replace("-", "");
                                }
                                if (((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).PaymentTip == null)
                                {
                                    tbPropina.Text = "0.0";
                                }
                                else
                                {
                                    tbPropina.Text = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).PaymentTip;
                                }

                                tbOtrosCargos.Text = "";

                                tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");
                                rowPropina.Visible = !string.IsNullOrEmpty(tbPropina.Text.Trim());
                                trOtrosCargos.Visible = !string.IsNullOrEmpty(tbOtrosCargos.Text.Trim());
                                rowDescuentos.Visible = true;

                                try
                                {
                                    tbIva16.Text = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).TaxTotal;
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
                                decimal.TryParse(tbIva16.Text, out dImpuestos);
                                decimal.TryParse(tbPropina.Text, out dPropinas);
                                tbSubtotal.Text = dSubTotal.ToString();
                                dTotalFactura = dSubTotal - dDescuento + dImpuestos;
                                dTotalPagar = dTotalFactura + dPropinas;
                                tbTotalFac.Text = dTotalFactura.ToString();
                                tbTotal.Text = dTotalPagar.ToString();
                                tbCantLetra.Text = numLetra.ConvertirALetras(tbTotal.Text, "MXN");
                                tbDescuento.Text = dDescuento.ToString();

                                try
                                {
                                    var dataTableConceptos = new DataTable();
                                    if (dataTableConceptos.Columns.Count == 0)
                                    {
                                        dataTableConceptos.Columns.Add("cantidad", typeof(string));
                                        dataTableConceptos.Columns.Add("cveProdServ", typeof(string));
                                        dataTableConceptos.Columns.Add("descripcion", typeof(string));
                                        dataTableConceptos.Columns.Add("valorUnitario", typeof(string));
                                        dataTableConceptos.Columns.Add("importe", typeof(string));
                                        dataTableConceptos.Columns.Add("unidad", typeof(string));
                                    }
                                    foreach (var concepto in ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).Items.Item)
                                    {
                                        decimal qty = 0;
                                        decimal price = 0;
                                        decimal taxRate = 0;
                                        decimal grossAmount = 0;
                                        decimal descuentoItem = 0;
                                        decimal unitPrice = 0;
                                        decimal netAmount = 0;
                                        decimal.TryParse(concepto.ItemQty, out qty);
                                        decimal.TryParse(concepto.ItemPrice, out price);
                                        decimal.TryParse(concepto.ItemTaxRate, out taxRate);
                                        decimal.TryParse(concepto.ItemGrossTotal, out grossAmount);
                                        decimal.TryParse(concepto.ItemDiscountAmt, out descuentoItem);
                                        decimal.TryParse(concepto.ItemNetTotal, out netAmount);
                                        unitPrice = decimal.Parse(CerosNull((grossAmount / (decimal)1.16).ToString()));
                                        var unitPriceConc = unitPrice + descuentoItem;
                                        decimal importe = decimal.Parse(CerosNull((qty * unitPrice).ToString()));
                                        var importeConc = decimal.Parse(CerosNull((qty * unitPriceConc).ToString()));
                                        var iva = decimal.Parse(CerosNull((importe * taxRate).ToString()));
                                        var baseImpuesto = decimal.Parse(CerosNull((netAmount).ToString()));
                                        var newRow = dataTableConceptos.NewRow();
                                        newRow[0] = qty.ToString(); // CANTIDAD
                                        newRow[1] = concepto.ItemBOHName; // CLAVE PRODUCTO/SERVICIO
                                        newRow[2] = concepto.ItemName; // DESCRIPCION
                                        newRow[3] = (unitPriceConc).ToString(); // VAL. UNITARIO
                                        newRow[4] = (importeConc).ToString(); // IMPORTE
                                        newRow[5] = concepto.ItemChitName2; // UNIDAD
                                        dataTableConceptos.Rows.Add(newRow);
                                    }
                                    Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] = dataTableConceptos;
                                    gvConceptos.DataSource = dataTableConceptos;
                                    gvConceptos.DataBind();
                                }
                                catch { }
                                try
                                {
                                    var metodoPagoTrama = ((Transacction)Session[ViewState["_PageID"].ToString() + "_tramaAloha"]).Tenders.Tender.FirstOrDefault().TenderName;
                                    var item = ddlMetodoPago.Items.Cast<ListItem>().FirstOrDefault(i => Regex.IsMatch(i.Text, @".*" + metodoPagoTrama + ".*", RegexOptions.IgnoreCase));
                                    ddlMetodoPago.SelectedValue = item.Value;
                                    //ddlMetodoPago.Enabled = false;
                                }
                                catch (Exception ex)
                                {
                                    // ignored
                                }
                                ddlMetodoPago_SelectedIndexChanged(null, null);
                                ScriptManager.RegisterStartupScript(this, GetType(), "_keyExtranet", "$('#" + progressCrear.ClientID + "').css('display', 'none'); $('#ModuloExtranet').modal('show');", true);
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "El periodo de generación de su factura ha caducado", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                            }

                            #endregion

                            #endregion
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "Trama no reconocida", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                        }
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "Trama no reconocida", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
            }
            else
            {
                var button = (Button)sender;
                if (button.ID.Equals(bGenerarConsultar.ID))
                {
                    Session[ViewState["_PageID"].ToString() + "_trama"] = "";
                    Session[ViewState["_PageID"].ToString() + "_arrayTrama"] = null;
                    Session[ViewState["_PageID"].ToString() + "_idTrama"] = "";
                    var sucEmi = ddlSucursalEmisor.SelectedValue;
                    var idFact = ConsultarIdefac(tbTicket.Text, sucEmi, "", "", 0);
                    if (!string.IsNullOrEmpty(idFact))
                    {
                        ConsultarArchivos(idFact);
                        ScriptManager.RegisterStartupScript(this, GetType(), "_keyFacturaConsultar", "$('#ModuloFactura').modal('show');", true);
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El ticket no se encuentra registrado. Favor de verificarlo e intentarlo nuevamente en unos momentos", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El ticket no se encuentra registrado o ya fue facturado. Favor de verificarlo e intentarlo nuevamente en unos momentos", 4, null, null, "$('#" + progressCrear.ClientID + "').css('display', 'none');");
                }
            }
        }

        /// <summary>
        /// Reads the trama opera.
        /// </summary>
        /// <param name="txtTrama">The text trama.</param>
        //private void ReadTramaOpera(string txtTrama)
        //{

        //    // var tra = txtTrama.Replace("OP", "").ToString();
        //    ReadTramaop(txtTrama);
        //    #region Lectura

        //    #region INFORMACION DOCUMENTO

        //    var bloque = "INFORMACION DOCUMENTO";
        //    _claseDoc = GetValueOp(bloque, "clasedoc");
        //    _numaprob = GetValueOp(bloque, "numaprob");
        //    _anoaprob = GetValueOp(bloque, "anoaprob");
        //    _serie = GetValueOp(bloque, "serie");
        //    _folio = GetValueOp(bloque, "folio");
        //    _folioint = GetValueOp(bloque, "folioint");
        //    _fechaemision = GetValueOp(bloque, "fechaemision");
        //    _formapago = GetValueOp(bloque, "formapago");
        //    _condpago = GetValueOp(bloque, "condpago");
        //    _ordencompra = GetValueOp(bloque, "ordencompra");
        //    _fechaoc = GetValueOp(bloque, "fechaoc");
        //    _contrarecibo = GetValueOp(bloque, "contrarecibo");
        //    _fechacontra = GetValueOp(bloque, "fechacontra");
        //    _termpagodias = GetValueOp(bloque, "termpagodias");
        //    _fechavenc = GetValueOp(bloque, "fecha venc");

        //    #endregion

        //    #region EMISOR

        //    bloque = "EMISOR";
        //    _rfc = GetValueOp(bloque, "rfc");
        //    _nombre = GetValueOp(bloque, "nombre");
        //    _numeroprov = GetValueOp(bloque, "numeroprov");

        //    #endregion

        //    #region EMISOR DOMICILIO FISCAL

        //    bloque = "EMISOR DOMICILIO FISCAL";

        //    _gln = GetValueOp(bloque, "gln");
        //    _calle = GetValueOp(bloque, "calle");
        //    _noexterior = GetValueOp(bloque, "noexterior");
        //    _nointerior = GetValueOp(bloque, "nointerior");
        //    _colonia = GetValueOp(bloque, "colonia");
        //    _localidad = GetValueOp(bloque, "localidad");
        //    _municipio = GetValueOp(bloque, "municipio");
        //    _estado = GetValueOp(bloque, "estado");
        //    _pais = GetValueOp(bloque, "pais");
        //    _codigopostal = GetValueOp(bloque, "codigopostal");
        //    if (_codigopostal.Length > 5)
        //    {
        //        _codigopostal = _codigopostal.TrimStart('0');
        //    }
        //    if (_estado.Trim().Equals("DF") || _estado.Trim().Equals("CDM") || _estado.Trim().Equals("CIU") || _estado.Trim().Equals("Distrito Federal"))
        //    {
        //        if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
        //        {
        //            _estado = "CDMX";
        //        }
        //        else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
        //        {
        //            _estado = "Ciudad de México";
        //        }
        //    }

        //    #endregion

        //    #region EMISOR DOMICILIO EXPEDICION

        //    bloque = "EMISOR DOMICILIO EXPEDICION";
        //    _sucursalex = GetValueOp(bloque, "sucursal");
        //    _glnex = GetValueOp(bloque, "gln");
        //    _calleex = GetValueOp(bloque, "calle");
        //    _noexteriorex = GetValueOp(bloque, "noexterior");
        //    _nointeriorex = GetValueOp(bloque, "nointerior");
        //    _coloniaex = GetValueOp(bloque, "colonia");
        //    _localidadex = GetValueOp(bloque, "localidad");
        //    _municipioex = GetValueOp(bloque, "municipio");
        //    _estadoex = GetValueOp(bloque, "estado");
        //    _paisex = GetValueOp(bloque, "pais");
        //    _codigopostalex = GetValueOp(bloque, "codigopostal");
        //    if (_codigopostalex.Length > 5)
        //    {
        //        _codigopostalex = _codigopostalex.TrimStart('0');
        //    }
        //    _lugarexpedicion = GetValueOp(bloque, "lugarexpedicion");
        //    if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA|HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
        //    {
        //        if (_lugarexpedicion.Trim().Equals("Distrito Federal") || _lugarexpedicion.Trim().Equals("DF", StringComparison.OrdinalIgnoreCase) || _lugarexpedicion.Trim().Equals("CDM", StringComparison.OrdinalIgnoreCase) || _lugarexpedicion.Trim().Equals("CIU", StringComparison.OrdinalIgnoreCase)) { _lugarexpedicion = "Ciudad de Mexico"; }
        //    }

        //    if (_estadoex.Trim().Equals("DF") || _estadoex.Trim().Equals("CDM") || _estadoex.Trim().Equals("CIU") || _estadoex.Trim().Equals("Distrito Federal"))
        //    {
        //        if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
        //        {
        //            _estadoex = "CDMX";
        //        }
        //        else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
        //        {
        //            _estadoex = "Ciudad de México";
        //        }
        //    }


        //    #endregion

        //    #region RECEPTOR

        //    bloque = "RECEPTOR";
        //    _rfcre = GetValueOp(bloque, "rfc");
        //    _nombrere = GetValueOp(bloque, "nombre");
        //    _numerocliente = GetValueOp(bloque, "numerocliente");
        //    _habitacion = GetValueOp(bloque, "habitacion");
        //    _checkin = GetValueOp(bloque, "checkin");
        //    _checkout = GetValueOp(bloque, "checkout");
        //    _cajero = GetValueOp(bloque, "cajero");
        //    _cuentaAr = GetValueOp(bloque, "cuentaAR");
        //    _email = GetValueOp(bloque, "email");
        //    _impresion = GetValueOp(bloque, "impresion");

        //    #endregion

        //    #region RECEPTOR DOMICILIO FISCAL

        //    bloque = "RECEPTOR DOMICILIO FISCAL";
        //    _rdfgln = GetValueOp(bloque, "gln");
        //    _rdfcalle = GetValueOp(bloque, "calle");
        //    _rdfnoexterior = GetValueOp(bloque, "noexterior");
        //    _rdfnointerior = GetValueOp(bloque, "nointerior");
        //    _rdfcolonia = GetValueOp(bloque, "colonia");
        //    _rdflocalidad = GetValueOp(bloque, "localidad");
        //    _rdfreferencia = GetValueOp(bloque, "referencia");
        //    _rdfmunicipio = GetValueOp(bloque, "municipio");
        //    _rdfestado = GetValueOp(bloque, "estado");
        //    _rdfpais = GetValueOp(bloque, "pais");
        //    _rdfcodigopostal = GetValueOp(bloque, "codigopostal");

        //    if (_rdfestado.Trim().Equals("DF") || _rdfestado.Trim().Equals("CDM") || _rdfestado.Trim().Equals("CIU") || _rdfestado.Trim().Equals("Distrito Federal"))
        //    {
        //        if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
        //        {
        //            _rdfestado = "CDMX";
        //        }
        //        else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
        //        {
        //            _rdfestado = "Ciudad de México";
        //        }
        //    }



        //    #endregion

        //    #region RECEPTOR DOMICILIO RECEPCION

        //    bloque = "RECEPTOR DOMICILIO RECEPCION";
        //    _sucursalre = GetValueOp(bloque, "sucursal");
        //    _cdgsucursal = GetValueOp(bloque, "cdgsucursal");
        //    _regln = GetValueOp(bloque, "gln");
        //    _callere = GetValueOp(bloque, "calle");
        //    _noexteriorre = GetValueOp(bloque, "noexterior");
        //    _nointeriorre = GetValueOp(bloque, "nointerior");
        //    _coloniare = GetValueOp(bloque, "colonia");
        //    _localidadre = GetValueOp(bloque, "localidad");
        //    _referencia = GetValueOp(bloque, "referencia");
        //    _municipiore = GetValueOp(bloque, "municipio");
        //    _estadore = GetValueOp(bloque, "estado");
        //    _paisre = GetValueOp(bloque, "pais");
        //    _codigopostalre = GetValueOp(bloque, "codigopostal");

        //    if (_estadore.Trim().Equals("DF") || _estadore.Trim().Equals("CDM") || _estadore.Trim().Equals("CIU") || _estadore.Trim().Equals("Distrito Federal"))
        //    {
        //        if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
        //        {
        //            _estadore = "CDMX";
        //        }
        //        else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
        //        {
        //            _estadore = "Ciudad de México";
        //        }
        //    }

        //    #endregion

        //    #region DETALLES

        //    bloque = "DETALLES";
        //    _arrayListD = new ArrayList();
        //    _arrayconceptos = new string[14];
        //    foreach (var concepto in _detalles)
        //    {
        //        _arrayconceptos[0] = concepto["Cantidad"];
        //        _arrayconceptos[1] = concepto["Descripcion"];
        //        if (_rfc.Equals("OAL111108K5A")) { _arrayconceptos[1] = Regex.Replace(_arrayconceptos[1].Trim(), @" {2,}\(\s*\d+\)", "", RegexOptions.IgnoreCase); }
        //        _arrayconceptos[2] = concepto["Codigo Producto"];
        //        _arrayconceptos[3] = concepto["Orden"];
        //        _arrayconceptos[4] = concepto["Precio Bruto"];
        //        _arrayconceptos[5] = concepto["Monto Bruto"];
        //        _arrayconceptos[6] = concepto["Tasa IVA"];
        //        _arrayconceptos[7] = concepto["Tipo IVA"];
        //        _arrayconceptos[8] = concepto["Monto IVA"];
        //        _arrayconceptos[9] = concepto["Precio Neto"];
        //        _arrayconceptos[10] = concepto["Monto Neto"];
        //        _arrayconceptos[11] = concepto["Monto Total Item"];
        //        _arrayconceptos[12] = concepto["Numero de Material"];
        //        _arrayconceptos[13] = concepto["Unidad de Medida"];
        //        _arrayListD.Add(_arrayconceptos);
        //    }

        //    #endregion

        //    #region TOTALES

        //    bloque = "TOTALES";
        //    _moneda = GetValueOp(bloque, "moneda");
        //    _tipocambio = GetValueOp(bloque, "tipocambio");
        //    _subtotal = GetValueOp(bloque, "subtotal");
        //    _tasadesc = GetValueOp(bloque, "tasadesc");
        //    _totaldesc = GetValueOp(bloque, "totaldesc");
        //    _montobase = GetValueOp(bloque, "montobase");
        //    _tipodeimp = GetValueOp(bloque, "tipodeimp");
        //    _tasaiva = GetValueOp(bloque, "tasaiva");
        //    _montoiva = GetValueOp(bloque, "montoiva");
        //    _tipoimpuesto1 = GetValueOp(bloque, "tipoimpuesto1");
        //    _tasaimpuesto1 = GetValueOp(bloque, "tasaimpuesto1");
        //    _montoimpuesto1 = GetValueOp(bloque, "montoimpuesto1");
        //    _tipoimpuesto2 = GetValueOp(bloque, "tipoimpuesto2");
        //    _tasaimpuesto2 = GetValueOp(bloque, "tasaimpuesto2");
        //    _montoimpuesto2 = GetValueOp(bloque, "montoimpuesto2");
        //    _tipoimpuesto3 = GetValueOp(bloque, "tipoimpuesto3");
        //    _tasaimpuesto3 = GetValueOp(bloque, "tasaimpuesto3");
        //    _montoimpuesto3 = GetValueOp(bloque, "montoimpuesto3");
        //    _montoimpuestos = GetValueOp(bloque, "montoimpuestos");
        //    _tiporet = GetValueOp(bloque, "tiporet");
        //    _tasaret = GetValueOp(bloque, "tasaret");
        //    _montoret = GetValueOp(bloque, "montoret");
        //    _tiporetd = GetValueOp(bloque, "tiporetd");
        //    _tasaretd = GetValueOp(bloque, "tasadesc");
        //    _montoretd = GetValueOp(bloque, "montoretd");
        //    _totalret = GetValueOp(bloque, "totalret");
        //    _totalRef = GetValueOp(bloque, "totalRef");
        //    _totalletraRef = GetValueOp(bloque, "totalletraRef");
        //    _vlrPagar = GetValueOp(bloque, "VlrPagar");
        //    _totalletra = GetValueOp(bloque, "totalletra");
        //    _propinas = GetValueOp(bloque, "propinas");
        //    _paidouts = GetValueOp(bloque, "paidouts");
        //    _otros = GetValueOp(bloque, "otros");
        //    _tipopago1 = GetValueOp(bloque, "tipopago1");
        //    _pago1 = GetValueOp(bloque, "pago1");
        //    _tipopago2 = GetValueOp(bloque, "tipopago2");
        //    _pago2 = GetValueOp(bloque, "pago2");
        //    _tipopago3 = GetValueOp(bloque, "tipopago3");
        //    _pago3 = GetValueOp(bloque, "pago3");
        //    _tipopago4 = GetValueOp(bloque, "tipopago4");
        //    _pago4 = GetValueOp(bloque, "pago4");
        //    _metodopago = GetValueOp(bloque, "metodopago");
        //    _numerocuentapago = GetValueOp(bloque, "numerocuentapago");
        //    _regimenfiscal = GetValueOp(bloque, "regimenfiscal");
        //    _nomHuesped = GetValueOp(bloque, "NomHuesped");
        //    _pasaporte = GetValueOp(bloque, "pasaporte");

        //    var tiposPago = !string.IsNullOrEmpty(_tipopago1) ? _tipopago1.Trim() + " " + _pago1.Trim() : "";
        //    tiposPago += !string.IsNullOrEmpty(_tipopago2) ? (!string.IsNullOrEmpty(tiposPago) ? ",,,,," : "") + _tipopago2.Trim() + " " + _pago2.Trim() : "";
        //    tiposPago += !string.IsNullOrEmpty(_tipopago3) ? (!string.IsNullOrEmpty(tiposPago) ? ",,,,," : "") + _tipopago3.Trim() + " " + _pago3.Trim() : "";
        //    tiposPago += !string.IsNullOrEmpty(_tipopago4) ? (!string.IsNullOrEmpty(tiposPago) ? ",,,,," : "") + _tipopago4.Trim() + " " + _pago4.Trim() : "";

        //    var cuentasPago = new List<string>();
        //    cuentasPago.Add(GetNumeroCuenta(_tipopago1));
        //    cuentasPago.Add(GetNumeroCuenta(_tipopago2));
        //    cuentasPago.Add(GetNumeroCuenta(_tipopago3));
        //    cuentasPago.Add(GetNumeroCuenta(_tipopago4));
        //    cuentasPago = cuentasPago.Where(cuenta => !string.IsNullOrEmpty(cuenta.Trim())).ToList();
        //    if (!string.IsNullOrEmpty(string.Join(",", cuentasPago)))
        //    {
        //        _numerocuentapago = string.Join(",", cuentasPago);
        //    }
        //    if (!string.IsNullOrEmpty(tiposPago))
        //    {
        //        _metodopago = tiposPago;
        //    }
        //    if (_rfc.Equals("IPY031013AD4"))
        //    {
        //        if (_rfcre.Equals("KOM660329K18"))
        //        {
        //            _metodopago = "NA";
        //        }
        //        else if (_rfcre.Equals("AMP990730M15"))
        //        {
        //            _metodopago = "02";
        //        }
        //    }
        //    else if (_rfc.Equals("OHS0312152Z4"))
        //    {
        //        if (Regex.IsMatch(_rfcre, "KOM660329K18|CIN981030FF3|CGS120904DG8|SCA721126MF6|AST1209042L1|EUM000707DQ2"))
        //        {
        //            _metodopago = "NA";
        //        }
        //    }
        //    else if (_rfc.Equals("OHF921110BF2"))
        //    {
        //        if (Regex.IsMatch(_rfcre, "URE780612C48|OUR990222MWA|SAU131126K91|DEJ961202UM2"))
        //        {
        //            _metodopago = "NA";
        //        }
        //    }
        //    if (_rfc.Equals("OHR980618BLA"))
        //    {
        //        decimal dSubTotal = 0;
        //        decimal dIva = 0;
        //        decimal dIsh = 0;
        //        decimal dPropina = 0;
        //        decimal dOtros = 0;
        //        decimal.TryParse(_subtotal, out dSubTotal);
        //        decimal.TryParse(_montoiva, out dIva);
        //        decimal.TryParse(_montoimpuesto1, out dIsh);
        //        decimal.TryParse(_propinas, out dPropina);
        //        decimal.TryParse(_otros, out dOtros);
        //        var vlrPagar = dSubTotal + dIva + dIsh;
        //        _vlrPagar = vlrPagar.ToString();
        //        var dTotal = vlrPagar + dPropina + dOtros;
        //        _totalRef = dTotal.ToString();
        //        _totalletra = new NumerosALetras().ConvertirALetras(_totalRef, _moneda);
        //    }


        //    #endregion

        //    #region REFERENCIADOS

        //    bloque = "REFERENCIADOS";


        //    #endregion

        //    #endregion
        //}

        private void ReadTramaOpera(string txtTrama)
        {

            // var tra = txtTrama.Replace("OP", "").ToString();
            ReadTramaop(txtTrama);
            #region 3.3

            #region Lectura

            #region INFORMACION DOCUMENTO

            var bloque = "INFORMACION DOCUMENTO";
            _claseDoc = GetValueOp(bloque, "clasedoc");
            _numaprob = GetValueOp(bloque, "numaprob");
            _anoaprob = GetValueOp(bloque, "anoaprob");
            _serie = GetValueOp(bloque, "serie");
            _folio = GetValueOp(bloque, "folio");
            _folioint = GetValueOp(bloque, "folioint");
            _fechaemision = GetValueOp(bloque, "fechaemision");
            _formapago = GetValueOp(bloque, "formapago");
            _condpago = GetValueOp(bloque, "condpago");
            _ordencompra = GetValueOp(bloque, "ordencompra");
            _fechaoc = GetValueOp(bloque, "fechaoc");
            _contrarecibo = GetValueOp(bloque, "contrarecibo");
            _fechacontra = GetValueOp(bloque, "fechacontra");
            _termpagodias = GetValueOp(bloque, "termpagodias");
            _fechavenc = GetValueOp(bloque, "fecha venc");


            #endregion

            #region EMISOR

            bloque = "EMISOR";
            _rfc = GetValueOp(bloque, "rfc");
            _nombre = GetValueOp(bloque, "nombre");
            _numeroprov = GetValueOp(bloque, "numeroprov");


            #endregion

            #region EMISOR DOMICILIO FISCAL

            bloque = "EMISOR DOMICILIO FISCAL";

            _gln = GetValueOp(bloque, "gln");
            _calle = GetValueOp(bloque, "calle");
            _noexterior = GetValueOp(bloque, "noexterior");
            _nointerior = GetValueOp(bloque, "nointerior");
            _colonia = GetValueOp(bloque, "colonia");
            _localidad = GetValueOp(bloque, "localidad");
            _municipio = GetValueOp(bloque, "municipio");
            _estado = GetValueOp(bloque, "estado");
            _pais = GetValueOp(bloque, "pais");
            _codigopostal = GetValueOp(bloque, "codigopostal");
            if (_codigopostal.Length > 5)
            {
                _codigopostal = _codigopostal.TrimStart('0');
            }
            if (_estado.Trim().Equals("DF") || _estado.Trim().Equals("CDM") || _estado.Trim().Equals("CIU") || _estado.Trim().Equals("Distrito Federal"))
            {
                if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
                {
                    _estado = "CDMX";
                }
                else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
                {
                    _estado = "Ciudad de México";
                }
            }

            #endregion

            #region EMISOR DOMICILIO EXPEDICION

            bloque = "EMISOR DOMICILIO EXPEDICION";
            _sucursalex = GetValueOp(bloque, "sucursal");
            _glnex = GetValueOp(bloque, "gln");
            _calleex = GetValueOp(bloque, "calle");
            _noexteriorex = GetValueOp(bloque, "noexterior");
            _nointeriorex = GetValueOp(bloque, "nointerior");
            _coloniaex = GetValueOp(bloque, "colonia");
            _localidadex = GetValueOp(bloque, "localidad");
            _municipioex = GetValueOp(bloque, "municipio");
            _estadoex = GetValueOp(bloque, "estado");
            _paisex = GetValueOp(bloque, "pais");
            _codigopostalex = GetValueOp(bloque, "codigopostal");
            if (_codigopostalex.Length > 5)
            {
                _codigopostalex = _codigopostalex.TrimStart('0');
            }
            _lugarexpedicion = GetValue(bloque, "lugarexpedicion");
            if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA|HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
            {
                if (_lugarexpedicion.Trim().Equals("Distrito Federal") || _lugarexpedicion.Trim().Equals("DF", StringComparison.OrdinalIgnoreCase) || _lugarexpedicion.Trim().Equals("CDM", StringComparison.OrdinalIgnoreCase) || _lugarexpedicion.Trim().Equals("CIU", StringComparison.OrdinalIgnoreCase)) { _lugarexpedicion = "Ciudad de Mexico"; }
            }

            if (_estadoex.Trim().Equals("DF") || _estadoex.Trim().Equals("CDM") || _estadoex.Trim().Equals("CIU") || _estadoex.Trim().Equals("Distrito Federal"))
            {
                if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
                {
                    _estadoex = "CDMX";
                }
                else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
                {
                    _estadoex = "Ciudad de México";
                }
            }


            #endregion

            #region RECEPTOR

            bloque = "RECEPTOR";
            _rfcre = GetValueOp(bloque, "rfc");
            _nombrere = GetValueOp(bloque, "nombre");
            _numerocliente = GetValueOp(bloque, "numerocliente");
            _habitacion = GetValueOp(bloque, "habitacion");
            _numeroreserva = GetValueOp(bloque, "numeroreserva");
            _numerovoucher = GetValueOp(bloque, "numerovoucher");
            _noadultos = GetValueOp(bloque, "noadultos");
            _noninos = GetValueOp(bloque, "noninos");
            _checkin = GetValueOp(bloque, "checkin");
            _checkout = GetValueOp(bloque, "checkout");
            _cajero = GetValueOp(bloque, "cajero");
            _cuentaAr = GetValueOp(bloque, "cuentaAR");
            _email = GetValueOp(bloque, "email");
            _impresion = GetValueOp(bloque, "impresion");


            #endregion

            #region RECEPTOR DOMICILIO FISCAL

            bloque = "RECEPTOR DOMICILIO FISCAL";
            _rdfgln = GetValueOp(bloque, "gln");
            _rdfcalle = GetValueOp(bloque, "calle");
            _rdfnoexterior = GetValueOp(bloque, "noexterior");
            _rdfnointerior = GetValueOp(bloque, "nointerior");
            _rdfcolonia = GetValueOp(bloque, "colonia");
            _rdflocalidad = GetValueOp(bloque, "localidad");
            _rdfreferencia = GetValueOp(bloque, "referencia");
            _rdfmunicipio = GetValueOp(bloque, "municipio");
            _rdfestado = GetValueOp(bloque, "estado");
            _rdfpais = GetValueOp(bloque, "pais");
            _rdfcodigopostal = GetValueOp(bloque, "codigopostal");

            if (_rdfestado.Trim().Equals("DF") || _rdfestado.Trim().Equals("CDM") || _rdfestado.Trim().Equals("CIU") || _rdfestado.Trim().Equals("Distrito Federal"))
            {
                if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
                {
                    _rdfestado = "CDMX";
                }
                else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
                {
                    _rdfestado = "Ciudad de México";
                }
            }

            #endregion

            #region RECEPTOR DOMICILIO RECEPCION

            bloque = "RECEPTOR DOMICILIO RECEPCION";
            _sucursalre = GetValueOp(bloque, "sucursal");
            _cdgsucursal = GetValueOp(bloque, "cdgsucursal");
            _regln = GetValueOp(bloque, "gln");
            _callere = GetValueOp(bloque, "calle");
            _noexteriorre = GetValueOp(bloque, "noexterior");
            _nointeriorre = GetValueOp(bloque, "nointerior");
            _coloniare = GetValueOp(bloque, "colonia");
            _localidadre = GetValueOp(bloque, "localidad");
            _referencia = GetValueOp(bloque, "referencia");
            _municipiore = GetValueOp(bloque, "municipio");
            _estadore = GetValueOp(bloque, "estado");
            _paisre = GetValueOp(bloque, "pais");
            _codigopostalre = GetValueOp(bloque, "codigopostal");

            if (_estadore.Trim().Equals("DF") || _estadore.Trim().Equals("CDM") || _estadore.Trim().Equals("CIU") || _estadore.Trim().Equals("Distrito Federal"))
            {
                if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
                {
                    _estadore = "CDMX";
                }
                else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
                {
                    _estadore = "Ciudad de México";
                }
            }

            #endregion

            #region DETALLES

            bloque = "DETALLES";
            _arrayListD = new ArrayList();
            _arrayconceptos = new string[31];
            foreach (var concepto in _detalles)
            {
                _arrayconceptos[0] = concepto["Cantidad"];
                _arrayconceptos[1] = concepto["Descripcion"];
                if (_rfc.Equals("OAL111108K5A")) { _arrayconceptos[1] = Regex.Replace(_arrayconceptos[1].Trim(), @" {1,}\(\s*\d+\)", "", RegexOptions.IgnoreCase); }
                _arrayconceptos[2] = concepto["Codigo Producto"];
                _arrayconceptos[3] = concepto["Orden"];
                _arrayconceptos[4] = concepto["Precio Bruto"];
                _arrayconceptos[5] = concepto["Monto Bruto"];
                try { _arrayconceptos[6] = concepto["Tasa IVA"]; } catch { _arrayconceptos[6] = ""; };
                try { _arrayconceptos[7] = concepto["Tipo IVA"]; } catch { _arrayconceptos[7] = ""; };
                try { _arrayconceptos[8] = concepto["Monto IVA"]; } catch { _arrayconceptos[8] = ""; };
                _arrayconceptos[9] = concepto["Precio Neto"];
                _arrayconceptos[10] = concepto["Monto Neto"];
                _arrayconceptos[11] = concepto["Monto Total Item"];
                _arrayconceptos[12] = concepto["Numero de Material"];
                _arrayconceptos[13] = concepto["Unidad de Medida"];
                try { _arrayconceptos[14] = concepto["CodigoImpuesto1"]; } catch { _arrayconceptos[14] = ""; }
                try { _arrayconceptos[15] = concepto["Tasa1"]; } catch { _arrayconceptos[15] = ""; }
                try { _arrayconceptos[16] = concepto["Montoimpuesto1"]; } catch { _arrayconceptos[16] = ""; }
                try { _arrayconceptos[17] = concepto["CodigoImpuesto2"]; } catch { _arrayconceptos[17] = ""; }
                try { _arrayconceptos[18] = concepto["Tasa2"]; } catch { _arrayconceptos[18] = ""; }
                try { _arrayconceptos[19] = concepto["Montoimpuesto2"]; } catch { _arrayconceptos[19] = ""; }
                try { _arrayconceptos[20] = concepto["CodigoImpuesto3"]; } catch { _arrayconceptos[20] = ""; }
                try { _arrayconceptos[21] = concepto["Tasa3"]; } catch { _arrayconceptos[21] = ""; }
                try { _arrayconceptos[22] = concepto["Montoimpuesto3"]; } catch { _arrayconceptos[22] = ""; }
                try { _arrayconceptos[23] = concepto["CodigoImpuesto4"]; } catch { _arrayconceptos[23] = ""; }
                try { _arrayconceptos[24] = concepto["Tasa4"]; } catch { _arrayconceptos[24] = ""; }
                try { _arrayconceptos[25] = concepto["Montoimpuesto4"]; } catch { _arrayconceptos[25] = ""; }
                try { _arrayconceptos[26] = concepto["CodigoImpuesto5"]; } catch { _arrayconceptos[26] = ""; }
                try { _arrayconceptos[27] = concepto["Tasa5"]; } catch { _arrayconceptos[27] = ""; }
                try { _arrayconceptos[28] = concepto["Montoimpuesto5"]; } catch { _arrayconceptos[28] = ""; }
                try { _arrayconceptos[29] = concepto["MontoDescuento"]; } catch { _arrayconceptos[29] = ""; }
                try { _arrayconceptos[30] = concepto["NaturalezaDescuento"]; } catch { _arrayconceptos[30] = ""; }
                _arrayListD.Add(_arrayconceptos);
            }

            #endregion

            #region TOTALES

            bloque = "TOTALES";
            try { _moneda = GetValueOp(bloque, "moneda"); } catch { _moneda = ""; }
            try { _tipocambio = GetValueOp(bloque, "tipocambio"); } catch { _tipocambio = ""; }
            try { _subtotal = GetValueOp(bloque, "subtotal"); } catch { _subtotal = ""; }
            try { _subtotalsindescuento = GetValueOp(bloque, "subtotalsindescuento"); } catch { _subtotalsindescuento = ""; }
            try { _anticipos = GetValueOp(bloque, "anticipos"); } catch { _anticipos = ""; }
            try { _subtotal1 = GetValueOp(bloque, "subtotal1"); } catch { _subtotal1 = ""; }
            try { _propinas2 = GetValueOp(bloque, "propinas2"); } catch { _propinas2 = ""; }
            try { _tasadesc = GetValueOp(bloque, "tasadesc"); } catch { _tasadesc = ""; }
            try { _totaldesc = GetValueOp(bloque, "totaldesc"); } catch { _totaldesc = ""; }
            try { _montobase = GetValueOp(bloque, "montobase"); } catch { _montobase = ""; }
            try { _tipodeimp = GetValueOp(bloque, "tipodeimp"); } catch { _tipodeimp = ""; }
            try { _tasaiva = GetValueOp(bloque, "tasaiva"); } catch { _tasaiva = ""; }
            try { _montoiva = GetValueOp(bloque, "montoiva"); } catch { _montoiva = ""; }
            try { _tipoimpuesto1 = GetValueOp(bloque, "tipoimpuesto1"); } catch { _tipoimpuesto1 = ""; }
            try { _tasaimpuesto1 = GetValueOp(bloque, "tasaimpuesto1"); } catch { _tasaimpuesto1 = ""; }
            try { _montoimpuesto1 = GetValueOp(bloque, "montoimpuesto1"); } catch { _montoimpuesto1 = ""; }
            try { _tipoimpuesto2 = GetValueOp(bloque, "tipoimpuesto2"); } catch { _tipoimpuesto2 = ""; }
            try { _tasaimpuesto2 = GetValueOp(bloque, "tasaimpuesto2"); } catch { _tasaimpuesto2 = ""; }
            try { _montoimpuesto2 = GetValueOp(bloque, "montoimpuesto2"); } catch { _montoimpuesto2 = ""; }
            try { _tipoimpuesto3 = GetValueOp(bloque, "tipoimpuesto3"); } catch { _tipoimpuesto3 = ""; }
            try { _tasaimpuesto3 = GetValueOp(bloque, "tasaimpuesto3"); } catch { _tasaimpuesto3 = ""; }
            try { _montoimpuesto3 = GetValueOp(bloque, "montoimpuesto3"); } catch { _montoimpuesto3 = ""; }
            try { _tipoimpuesto4 = GetValueOp(bloque, "tipoimpuesto4"); } catch { _tipoimpuesto4 = ""; }
            try { _tasaimpuesto4 = GetValueOp(bloque, "tasaimpuesto4"); } catch { _tasaimpuesto4 = ""; }
            try { _montoimpuesto4 = GetValueOp(bloque, "montoimpuesto4"); } catch { _montoimpuesto4 = ""; }
            try { _tipoimpuesto5 = GetValueOp(bloque, "tipoimpuesto5"); } catch { _tipoimpuesto5 = ""; }
            try { _tasaimpuesto5 = GetValueOp(bloque, "tasaimpuesto5"); } catch { _tasaimpuesto5 = ""; }
            try { _montoimpuesto5 = GetValueOp(bloque, "montoimpuesto5"); } catch { _montoimpuesto5 = ""; }
            try { _montoimpuestos = GetValueOp(bloque, "montoimpuestos"); } catch { _montoimpuestos = ""; }
            try { _tiporet = GetValueOp(bloque, "tiporet"); } catch { _tiporet = ""; }
            try { _tasaret = GetValueOp(bloque, "tasaret"); } catch { _tasaret = ""; }
            try { _montoret = GetValueOp(bloque, "montoret"); } catch { _montoret = ""; }
            try { _tiporetd = GetValueOp(bloque, "tiporetd"); } catch { _tiporetd = ""; }
            try { _tasaretd = GetValueOp(bloque, "tasadesc"); } catch { _tasaretd = ""; }
            try { _montoretd = GetValueOp(bloque, "montoretd"); } catch { _montoretd = ""; }
            try { _totalret = GetValueOp(bloque, "totalret"); } catch { _totalret = ""; }
            try { _totalRef = GetValueOp(bloque, "totalRef"); } catch { _totalRef = ""; }
            try { _totalletraRef = GetValueOp(bloque, "totalletraRef"); } catch { _totalletraRef = ""; }
            try { _vlrPagar = GetValueOp(bloque, "VlrPagar"); } catch { _vlrPagar = ""; }
            try { _totalletra = GetValueOp(bloque, "totalletra"); } catch { _totalletra = ""; }
            try { _propinas = GetValueOp(bloque, "propinas"); } catch { _propinas = ""; }
            try { _paidouts = GetValueOp(bloque, "paidouts"); } catch { _paidouts = ""; }
            try { _otros = GetValueOp(bloque, "otros"); } catch { _otros = ""; }
            try { _tipopago1 = GetValueOp(bloque, "tipopago1"); } catch { _tipopago1 = ""; }
            try { _pago1 = GetValueOp(bloque, "pago1"); } catch { _pago1 = ""; }
            try { _tipopago2 = GetValueOp(bloque, "tipopago2"); } catch { _tipopago2 = ""; }
            try { _pago2 = GetValueOp(bloque, "pago2"); } catch { _pago2 = ""; }
            try { _tipopago3 = GetValueOp(bloque, "tipopago3"); } catch { _tipopago3 = ""; }
            try { _pago3 = GetValueOp(bloque, "pago3"); } catch { _pago3 = ""; }
            try { _tipopago4 = GetValueOp(bloque, "tipopago4"); } catch { _tipopago4 = ""; }
            try { _pago4 = GetValueOp(bloque, "pago4"); } catch { _pago4 = ""; }
            try { _metodopago = GetValueOp(bloque, "metodopago"); } catch { _metodopago = ""; }
            try { _numerocuentapago = GetValueOp(bloque, "numerocuentapago"); } catch { _numerocuentapago = ""; }
            try { _regimenfiscal = GetValueOp(bloque, "regimenfiscal"); } catch { _regimenfiscal = ""; }
            try { _nomHuesped = GetValueOp(bloque, "NomHuesped"); } catch { _nomHuesped = ""; }
            try { _pasaporte = GetValueOp(bloque, "pasaporte"); } catch { _pasaporte = ""; }
            try { _tipoDocto = GetValueOp(bloque, "TipoDocto"); } catch { _tipoDocto = ""; }
            try { _division = GetValueOp(bloque, "Division"); } catch { _division = ""; }
            try { _noProveedor = GetValueOp(bloque, "No. De Proveedor"); } catch { _noProveedor = ""; }
            try { _correoSolicitante = GetValueOp(bloque, "Correo Solicitante"); } catch { _correoSolicitante = ""; }
            try { _nombreSolicitante = GetValueOp(bloque, "Nombre Solicitante"); } catch { _nombreSolicitante = ""; }

            var _tiposPago = !string.IsNullOrEmpty(_tipopago1) ? _tipopago1.Trim() + " " + _pago1.Trim() : "";
            _tiposPago += !string.IsNullOrEmpty(_tipopago2) ? (!string.IsNullOrEmpty(_tiposPago) ? ",,,,," : "") + _tipopago2.Trim() + " " + _pago2.Trim() : "";
            _tiposPago += !string.IsNullOrEmpty(_tipopago3) ? (!string.IsNullOrEmpty(_tiposPago) ? ",,,,," : "") + _tipopago3.Trim() + " " + _pago3.Trim() : "";
            _tiposPago += !string.IsNullOrEmpty(_tipopago4) ? (!string.IsNullOrEmpty(_tiposPago) ? ",,,,," : "") + _tipopago4.Trim() + " " + _pago4.Trim() : "";

            var cuentasPago = new List<string>();
            cuentasPago.Add(GetNumeroCuenta(_tipopago1));
            cuentasPago.Add(GetNumeroCuenta(_tipopago2));
            cuentasPago.Add(GetNumeroCuenta(_tipopago3));
            cuentasPago.Add(GetNumeroCuenta(_tipopago4));
            cuentasPago = cuentasPago.Where(cuenta => !string.IsNullOrEmpty(cuenta.Trim())).ToList();
            if (!string.IsNullOrEmpty(string.Join(",", cuentasPago)))
            {
                _numerocuentapago = string.Join(",", cuentasPago);
            }
            if (_rfc.Equals("SGO050614JG0"))
            {
                // Respetar metodo de pago
            }
            else if (!string.IsNullOrEmpty(_tiposPago))
            {
                _metodopago = _tiposPago;
            }
            if (_rfc.Equals("IPY031013AD4"))
            {
                if (_rfcre.Equals("KOM660329K18"))
                {
                    _metodopago = "NA";
                }
                else if (_rfcre.Equals("AMP990730M15"))
                {
                    _metodopago = "02";
                }
            }
            else if (_rfc.Equals("OHS0312152Z4"))
            {
                if (Regex.IsMatch(_rfcre, "KOM660329K18|CIN981030FF3|CGS120904DG8|SCA721126MF6|AST1209042L1|EUM000707DQ2"))
                {
                    _metodopago = "NA";
                }
            }
            else if (_rfc.Equals("OHF921110BF2"))
            {
                if (Regex.IsMatch(_rfcre, "URE780612C48|OUR990222MWA|SAU131126K91|DEJ961202UM2"))
                {
                    _metodopago = "NA";
                }
            }

            //if (_metodopago.Equals("no identificado", StringComparison.OrdinalIgnoreCase) || _rfc.Trim().Equals("HRP880129QX5", StringComparison.OrdinalIgnoreCase) || _rfc.Trim().Equals("OAL111108K5A", StringComparison.OrdinalIgnoreCase))
            //{
            //    _metodopago = _tiposPago;
            //}
            //if (string.IsNullOrEmpty(_metodopago))
            //{
            //    _metodopago = _tiposPago;
            //}
            //if (_rfc.Trim().Equals("HRP880129QX5", StringComparison.OrdinalIgnoreCase) || _rfc.Trim().Equals("OAL111108K5A", StringComparison.OrdinalIgnoreCase))
            //{
            //    _numerocuentapago = _tiposPago;
            //}

            #endregion

            #region REFERENCIADOS

            bloque = "REFERENCIADOS";

            #endregion

            _bandera = false;

            #endregion

            #endregion
            #region 3.2
            //#region Lectura

            //#region INFORMACION DOCUMENTO

            //var bloque = "INFORMACION DOCUMENTO";
            //_claseDoc = GetValueOp(bloque, "clasedoc");
            //_numaprob = GetValueOp(bloque, "numaprob");
            //_anoaprob = GetValueOp(bloque, "anoaprob");
            //_serie = GetValueOp(bloque, "serie");
            //_folio = GetValueOp(bloque, "folio");
            //_folioint = GetValueOp(bloque, "folioint");
            //_fechaemision = GetValueOp(bloque, "fechaemision");
            //_formapago = GetValueOp(bloque, "formapago");
            //_condpago = GetValueOp(bloque, "condpago");
            //_ordencompra = GetValueOp(bloque, "ordencompra");
            //_fechaoc = GetValueOp(bloque, "fechaoc");
            //_contrarecibo = GetValueOp(bloque, "contrarecibo");
            //_fechacontra = GetValueOp(bloque, "fechacontra");
            //_termpagodias = GetValueOp(bloque, "termpagodias");
            //_fechavenc = GetValueOp(bloque, "fecha venc");

            //#endregion

            //#region EMISOR

            //bloque = "EMISOR";
            //_rfc = GetValueOp(bloque, "rfc");
            //_nombre = GetValueOp(bloque, "nombre");
            //_numeroprov = GetValueOp(bloque, "numeroprov");

            //#endregion

            //#region EMISOR DOMICILIO FISCAL

            //bloque = "EMISOR DOMICILIO FISCAL";

            //_gln = GetValueOp(bloque, "gln");
            //_calle = GetValueOp(bloque, "calle");
            //_noexterior = GetValueOp(bloque, "noexterior");
            //_nointerior = GetValueOp(bloque, "nointerior");
            //_colonia = GetValueOp(bloque, "colonia");
            //_localidad = GetValueOp(bloque, "localidad");
            //_municipio = GetValueOp(bloque, "municipio");
            //_estado = GetValueOp(bloque, "estado");
            //_pais = GetValueOp(bloque, "pais");
            //_codigopostal = GetValueOp(bloque, "codigopostal");
            //if (_codigopostal.Length > 5)
            //{
            //    _codigopostal = _codigopostal.TrimStart('0');
            //}
            //if (_estado.Trim().Equals("DF") || _estado.Trim().Equals("CDM") || _estado.Trim().Equals("CIU") || _estado.Trim().Equals("Distrito Federal"))
            //{
            //    if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
            //    {
            //        _estado = "CDMX";
            //    }
            //    else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
            //    {
            //        _estado = "Ciudad de México";
            //    }
            //}

            //#endregion

            //#region EMISOR DOMICILIO EXPEDICION

            //bloque = "EMISOR DOMICILIO EXPEDICION";
            //_sucursalex = GetValueOp(bloque, "sucursal");
            //_glnex = GetValueOp(bloque, "gln");
            //_calleex = GetValueOp(bloque, "calle");
            //_noexteriorex = GetValueOp(bloque, "noexterior");
            //_nointeriorex = GetValueOp(bloque, "nointerior");
            //_coloniaex = GetValueOp(bloque, "colonia");
            //_localidadex = GetValueOp(bloque, "localidad");
            //_municipioex = GetValueOp(bloque, "municipio");
            //_estadoex = GetValueOp(bloque, "estado");
            //_paisex = GetValueOp(bloque, "pais");
            //_codigopostalex = GetValueOp(bloque, "codigopostal");
            //if (_codigopostalex.Length > 5)
            //{
            //    _codigopostalex = _codigopostalex.TrimStart('0');
            //}
            //_lugarexpedicion = GetValueOp(bloque, "lugarexpedicion");
            //if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA|HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
            //{
            //    if (_lugarexpedicion.Trim().Equals("Distrito Federal") || _lugarexpedicion.Trim().Equals("DF", StringComparison.OrdinalIgnoreCase) || _lugarexpedicion.Trim().Equals("CDM", StringComparison.OrdinalIgnoreCase) || _lugarexpedicion.Trim().Equals("CIU", StringComparison.OrdinalIgnoreCase)) { _lugarexpedicion = "Ciudad de Mexico"; }
            //}

            //if (_estadoex.Trim().Equals("DF") || _estadoex.Trim().Equals("CDM") || _estadoex.Trim().Equals("CIU") || _estadoex.Trim().Equals("Distrito Federal"))
            //{
            //    if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
            //    {
            //        _estadoex = "CDMX";
            //    }
            //    else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
            //    {
            //        _estadoex = "Ciudad de México";
            //    }
            //}


            //#endregion

            //#region RECEPTOR

            //bloque = "RECEPTOR";
            //_rfcre = GetValueOp(bloque, "rfc");
            //_nombrere = GetValueOp(bloque, "nombre");
            //_numerocliente = GetValueOp(bloque, "numerocliente");
            //_habitacion = GetValueOp(bloque, "habitacion");
            //_checkin = GetValueOp(bloque, "checkin");
            //_checkout = GetValueOp(bloque, "checkout");
            //_cajero = GetValueOp(bloque, "cajero");
            //_cuentaAr = GetValueOp(bloque, "cuentaAR");
            //_email = GetValueOp(bloque, "email");
            //_impresion = GetValueOp(bloque, "impresion");

            //#endregion

            //#region RECEPTOR DOMICILIO FISCAL

            //bloque = "RECEPTOR DOMICILIO FISCAL";
            //_rdfgln = GetValueOp(bloque, "gln");
            //_rdfcalle = GetValueOp(bloque, "calle");
            //_rdfnoexterior = GetValueOp(bloque, "noexterior");
            //_rdfnointerior = GetValueOp(bloque, "nointerior");
            //_rdfcolonia = GetValueOp(bloque, "colonia");
            //_rdflocalidad = GetValueOp(bloque, "localidad");
            //_rdfreferencia = GetValueOp(bloque, "referencia");
            //_rdfmunicipio = GetValueOp(bloque, "municipio");
            //_rdfestado = GetValueOp(bloque, "estado");
            //_rdfpais = GetValueOp(bloque, "pais");
            //_rdfcodigopostal = GetValueOp(bloque, "codigopostal");

            //if (_rdfestado.Trim().Equals("DF") || _rdfestado.Trim().Equals("CDM") || _rdfestado.Trim().Equals("CIU") || _rdfestado.Trim().Equals("Distrito Federal"))
            //{
            //    if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
            //    {
            //        _rdfestado = "CDMX";
            //    }
            //    else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
            //    {
            //        _rdfestado = "Ciudad de México";
            //    }
            //}



            //#endregion

            //#region RECEPTOR DOMICILIO RECEPCION

            //bloque = "RECEPTOR DOMICILIO RECEPCION";
            //_sucursalre = GetValueOp(bloque, "sucursal");
            //_cdgsucursal = GetValueOp(bloque, "cdgsucursal");
            //_regln = GetValueOp(bloque, "gln");
            //_callere = GetValueOp(bloque, "calle");
            //_noexteriorre = GetValueOp(bloque, "noexterior");
            //_nointeriorre = GetValueOp(bloque, "nointerior");
            //_coloniare = GetValueOp(bloque, "colonia");
            //_localidadre = GetValueOp(bloque, "localidad");
            //_referencia = GetValueOp(bloque, "referencia");
            //_municipiore = GetValueOp(bloque, "municipio");
            //_estadore = GetValueOp(bloque, "estado");
            //_paisre = GetValueOp(bloque, "pais");
            //_codigopostalre = GetValueOp(bloque, "codigopostal");

            //if (_estadore.Trim().Equals("DF") || _estadore.Trim().Equals("CDM") || _estadore.Trim().Equals("CIU") || _estadore.Trim().Equals("Distrito Federal"))
            //{
            //    if (Regex.IsMatch(_rfc, @"ODO120810J51|HRP880129QX5|OHR980618BLA"))
            //    {
            //        _estadore = "CDMX";
            //    }
            //    else if (Regex.IsMatch(_rfc, @"HSC0010193M7|OAP981214DP3|OHF921110BF2|OHS0312152Z4"))
            //    {
            //        _estadore = "Ciudad de México";
            //    }
            //}

            //#endregion

            //#region DETALLES

            //bloque = "DETALLES";
            //_arrayListD = new ArrayList();
            //_arrayconceptos = new string[14];
            //foreach (var concepto in _detalles)
            //{
            //    _arrayconceptos[0] = concepto["Cantidad"];
            //    _arrayconceptos[1] = concepto["Descripcion"];
            //    if (_rfc.Equals("OAL111108K5A")) { _arrayconceptos[1] = Regex.Replace(_arrayconceptos[1].Trim(), @" {2,}\(\s*\d+\)", "", RegexOptions.IgnoreCase); }
            //    _arrayconceptos[2] = concepto["Codigo Producto"];
            //    _arrayconceptos[3] = concepto["Orden"];
            //    _arrayconceptos[4] = concepto["Precio Bruto"];
            //    _arrayconceptos[5] = concepto["Monto Bruto"];
            //    _arrayconceptos[6] = concepto["Tasa IVA"];
            //    _arrayconceptos[7] = concepto["Tipo IVA"];
            //    _arrayconceptos[8] = concepto["Monto IVA"];
            //    _arrayconceptos[9] = concepto["Precio Neto"];
            //    _arrayconceptos[10] = concepto["Monto Neto"];
            //    _arrayconceptos[11] = concepto["Monto Total Item"];
            //    _arrayconceptos[12] = concepto["Numero de Material"];
            //    _arrayconceptos[13] = concepto["Unidad de Medida"];
            //    _arrayListD.Add(_arrayconceptos);
            //}

            //#endregion

            //#region TOTALES

            //bloque = "TOTALES";
            //_moneda = GetValueOp(bloque, "moneda");
            //_tipocambio = GetValueOp(bloque, "tipocambio");
            //_subtotal = GetValueOp(bloque, "subtotal");
            //_tasadesc = GetValueOp(bloque, "tasadesc");
            //_totaldesc = GetValueOp(bloque, "totaldesc");
            //_montobase = GetValueOp(bloque, "montobase");
            //_tipodeimp = GetValueOp(bloque, "tipodeimp");
            //_tasaiva = GetValueOp(bloque, "tasaiva");
            //_montoiva = GetValueOp(bloque, "montoiva");
            //_tipoimpuesto1 = GetValueOp(bloque, "tipoimpuesto1");
            //_tasaimpuesto1 = GetValueOp(bloque, "tasaimpuesto1");
            //_montoimpuesto1 = GetValueOp(bloque, "montoimpuesto1");
            //_tipoimpuesto2 = GetValueOp(bloque, "tipoimpuesto2");
            //_tasaimpuesto2 = GetValueOp(bloque, "tasaimpuesto2");
            //_montoimpuesto2 = GetValueOp(bloque, "montoimpuesto2");
            //_tipoimpuesto3 = GetValueOp(bloque, "tipoimpuesto3");
            //_tasaimpuesto3 = GetValueOp(bloque, "tasaimpuesto3");
            //_montoimpuesto3 = GetValueOp(bloque, "montoimpuesto3");
            //_montoimpuestos = GetValueOp(bloque, "montoimpuestos");
            //_tiporet = GetValueOp(bloque, "tiporet");
            //_tasaret = GetValueOp(bloque, "tasaret");
            //_montoret = GetValueOp(bloque, "montoret");
            //_tiporetd = GetValueOp(bloque, "tiporetd");
            //_tasaretd = GetValueOp(bloque, "tasadesc");
            //_montoretd = GetValueOp(bloque, "montoretd");
            //_totalret = GetValueOp(bloque, "totalret");
            //_totalRef = GetValueOp(bloque, "totalRef");
            //_totalletraRef = GetValueOp(bloque, "totalletraRef");
            //_vlrPagar = GetValueOp(bloque, "VlrPagar");
            //_totalletra = GetValueOp(bloque, "totalletra");
            //_propinas = GetValueOp(bloque, "propinas");
            //_paidouts = GetValueOp(bloque, "paidouts");
            //_otros = GetValueOp(bloque, "otros");
            //_tipopago1 = GetValueOp(bloque, "tipopago1");
            //_pago1 = GetValueOp(bloque, "pago1");
            //_tipopago2 = GetValueOp(bloque, "tipopago2");
            //_pago2 = GetValueOp(bloque, "pago2");
            //_tipopago3 = GetValueOp(bloque, "tipopago3");
            //_pago3 = GetValueOp(bloque, "pago3");
            //_tipopago4 = GetValueOp(bloque, "tipopago4");
            //_pago4 = GetValueOp(bloque, "pago4");
            //_metodopago = GetValueOp(bloque, "metodopago");
            //_numerocuentapago = GetValueOp(bloque, "numerocuentapago");
            //_regimenfiscal = GetValueOp(bloque, "regimenfiscal");
            //_nomHuesped = GetValueOp(bloque, "NomHuesped");
            //_pasaporte = GetValueOp(bloque, "pasaporte");

            //var tiposPago = !string.IsNullOrEmpty(_tipopago1) ? _tipopago1.Trim() + " " + _pago1.Trim() : "";
            //tiposPago += !string.IsNullOrEmpty(_tipopago2) ? (!string.IsNullOrEmpty(tiposPago) ? ",,,,," : "") + _tipopago2.Trim() + " " + _pago2.Trim() : "";
            //tiposPago += !string.IsNullOrEmpty(_tipopago3) ? (!string.IsNullOrEmpty(tiposPago) ? ",,,,," : "") + _tipopago3.Trim() + " " + _pago3.Trim() : "";
            //tiposPago += !string.IsNullOrEmpty(_tipopago4) ? (!string.IsNullOrEmpty(tiposPago) ? ",,,,," : "") + _tipopago4.Trim() + " " + _pago4.Trim() : "";

            //var cuentasPago = new List<string>();
            //cuentasPago.Add(GetNumeroCuenta(_tipopago1));
            //cuentasPago.Add(GetNumeroCuenta(_tipopago2));
            //cuentasPago.Add(GetNumeroCuenta(_tipopago3));
            //cuentasPago.Add(GetNumeroCuenta(_tipopago4));
            //cuentasPago = cuentasPago.Where(cuenta => !string.IsNullOrEmpty(cuenta.Trim())).ToList();
            //if (!string.IsNullOrEmpty(string.Join(",", cuentasPago)))
            //{
            //    _numerocuentapago = string.Join(",", cuentasPago);
            //}
            //if (!string.IsNullOrEmpty(tiposPago))
            //{
            //    _metodopago = tiposPago;
            //}
            //if (_rfc.Equals("IPY031013AD4"))
            //{
            //    if (_rfcre.Equals("KOM660329K18"))
            //    {
            //        _metodopago = "NA";
            //    }
            //    else if (_rfcre.Equals("AMP990730M15"))
            //    {
            //        _metodopago = "02";
            //    }
            //}
            //else if (_rfc.Equals("OHS0312152Z4"))
            //{
            //    if (Regex.IsMatch(_rfcre, "KOM660329K18|CIN981030FF3|CGS120904DG8|SCA721126MF6|AST1209042L1|EUM000707DQ2"))
            //    {
            //        _metodopago = "NA";
            //    }
            //}
            //else if (_rfc.Equals("OHF921110BF2"))
            //{
            //    if (Regex.IsMatch(_rfcre, "URE780612C48|OUR990222MWA|SAU131126K91|DEJ961202UM2"))
            //    {
            //        _metodopago = "NA";
            //    }
            //}
            //if (_rfc.Equals("OHR980618BLA"))
            //{
            //    decimal dSubTotal = 0;
            //    decimal dIva = 0;
            //    decimal dIsh = 0;
            //    decimal dPropina = 0;
            //    decimal dOtros = 0;
            //    decimal.TryParse(_subtotal, out dSubTotal);
            //    decimal.TryParse(_montoiva, out dIva);
            //    decimal.TryParse(_montoimpuesto1, out dIsh);
            //    decimal.TryParse(_propinas, out dPropina);
            //    decimal.TryParse(_otros, out dOtros);
            //    var vlrPagar = dSubTotal + dIva + dIsh;
            //    _vlrPagar = vlrPagar.ToString();
            //    var dTotal = vlrPagar + dPropina + dOtros;
            //    _totalRef = dTotal.ToString();
            //    _totalletra = new NumerosALetras().ConvertirALetras(_totalRef, _moneda);
            //}


            //#endregion

            //#region REFERENCIADOS

            //bloque = "REFERENCIADOS";


            //#endregion

            //#endregion
            #endregion
        }

        /// <summary>
        /// Reads the trama.
        /// </summary>
        /// <param name="documentContent">Content of the document.</param>
        //private void ReadTrama(string documentContent)
        //{
        //    _trama = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        //    //var sectionFirst = documentContent.Split(new[] { "XXX" }, StringSplitOptions.RemoveEmptyEntries, 2);
        //    var sections = documentContent.Split(new[]
        //    {
        //        "\nXXX",
        //        "\r\nXXX"
        //    }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
        //    //sections.Remove(sections.FirstOrDefault());
        //    foreach (var section in sections)
        //    {
        //        var contents = section.Split(new[]
        //        {
        //            "\n",
        //            "\r\n"
        //        }, 2, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
        //        var title = contents.First();
        //        var content = contents.Last();
        //        var variables = new Dictionary<string, string>();
        //        var contentSplit = content.Split(new[]
        //        {
        //            "\n",
        //            "\r\n"
        //        }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
        //        if (title.Equals("DETALLES", StringComparison.OrdinalIgnoreCase))
        //        {
        //            var titleDetalles = contentSplit.First();
        //            var titleDetallesSplit = titleDetalles.Split('|').Select(s => s.Trim()).ToList();
        //            _detalles = new List<Dictionary<string, string>>();
        //            var lista = contentSplit.ToList();
        //            lista.RemoveAt(0);
        //            foreach (var lineDetalles in lista)
        //            {
        //                var contentDetallesSplit = lineDetalles.Split('|').Select(s => s.Trim()).ToList();
        //                if (titleDetallesSplit.Count != contentDetallesSplit.Count)
        //                {
        //                    continue;
        //                }
        //                var dataDetalles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        //                for (var detI = 0; detI < titleDetallesSplit.Count; detI++)
        //                {
        //                    dataDetalles.Add(titleDetallesSplit[detI], contentDetallesSplit[detI]);
        //                }
        //                _detalles.Add(dataDetalles);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var variable in contentSplit)
        //            {
        //                var variableContent = variable.Split(new[]
        //                {
        //                    '='
        //                }, 2).Select(s => s.Trim()).ToList();
        //                var variableName = variableContent.First();
        //                var variableValue = variableContent.LastOrDefault();
        //                if (!variables.ContainsKey(variableName))
        //                {
        //                    variables.Add(variableName, variableValue);
        //                }
        //                else
        //                {
        //                    // ==== Special exclusions
        //                    switch (variableName)
        //                    {
        //                        case "tiporet":
        //                            variables.Add("tiporetd", variableValue);
        //                            break;
        //                        case "montoret":
        //                            variables.Add("montoretd", variableValue);
        //                            break;
        //                        default:
        //                            variables[variableName] = variableValue;
        //                            break;
        //                    }
        //                }
        //            }
        //            _trama.Add(title, variables);
        //        }
        //    }
        //}

        private void ReadTrama(string documentContent)
        {
            _trama = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            //var sectionFirst = documentContent.Split(new[] { "XXX" }, StringSplitOptions.RemoveEmptyEntries, 2);
            var sections = documentContent.Split(new[]
            {
                "\nXXX",
                "\r\nXXX"
            }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
            //sections.Remove(sections.FirstOrDefault());
            foreach (var section in sections)
            {
                var contents = section.Split(new[]
                {
                    "\n",
                    "\r\n"
                }, 2, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
                var title = contents.First();
                var content = contents.Last();
                var variables = new Dictionary<string, string>();
                var contentSplit = content.Split(new[]
                {
                    "\n",
                    "\r\n"
                }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
                if (title.Equals("DETALLES", StringComparison.OrdinalIgnoreCase))
                {
                    var titleDetalles = contentSplit.First();
                    var titleDetallesSplit = titleDetalles.Split('|').Select(s => s.Trim()).ToList();
                    _detalles = new List<Dictionary<string, string>>();
                    var lista = contentSplit.ToList();
                    lista.RemoveAt(0);
                    foreach (var lineDetalles in lista)
                    {
                        var contentDetallesSplit = lineDetalles.Split('|').Select(s => s.Trim()).ToList();
                        if (titleDetallesSplit.Count != contentDetallesSplit.Count)
                        {
                            continue;
                        }
                        var dataDetalles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                        for (var detI = 0; detI < titleDetallesSplit.Count; detI++)
                        {
                            dataDetalles.Add(titleDetallesSplit[detI], contentDetallesSplit[detI]);
                        }
                        _detalles.Add(dataDetalles);
                    }
                }
                else
                {
                    foreach (var variable in contentSplit)
                    {
                        var variableContent = variable.Split(new[]
                        {
                            '='
                        }, 2).Select(s => s.Trim()).ToList();
                        var variableName = variableContent.First();
                        var variableValue = variableContent.LastOrDefault();
                        if (!variables.ContainsKey(variableName))
                        {
                            variables.Add(variableName, variableValue);
                        }
                        else
                        {
                            // ==== Special exclusions
                            switch (variableName)
                            {
                                case "tiporet":
                                    variables.Add("tiporetd", variableValue);
                                    break;
                                case "montoret":
                                    variables.Add("montoretd", variableValue);
                                    break;
                                default:
                                    variables[variableName] = variableValue;
                                    break;
                            }
                        }
                    }
                    _trama.Add(title, variables);
                }
            }
        }


        /// <summary>
        /// Gets the numero cuenta.
        /// </summary>
        /// <param name="tipoPago">The tipo pago.</param>
        /// <returns>System.String.</returns>
        private string GetNumeroCuenta(string tipoPago)
        {
            var cuentas = new List<string>();
            try
            {
                var accounts = Regex.Matches(tipoPago, @"(X+\d{4})|(\d{4}.{0,2}(MXN|USD))", RegexOptions.IgnoreCase);
                foreach (Match account in accounts)
                {
                    var numbers = Regex.Matches(account.Value, @"\d{4}");
                    foreach (Match number in numbers)
                    {
                        cuentas.Add(number.Value);
                    }
                }
            }
            catch { cuentas = new List<string>(); }
            return string.Join(",", cuentas);
        }

        /// <summary>
        /// Reads the tramaop.
        /// </summary>
        /// <param name="documentContent">Content of the document.</param>
        //private void ReadTramaop(string documentContent)
        //{
        //    _trama = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        //    //var sectionFirst = documentContent.Split(new[] { "XXX" }, StringSplitOptions.RemoveEmptyEntries, 2);
        //    var sections = documentContent.Split(new[]
        //    {
        //        "\nXXX",
        //        "\r\nXXX"
        //    }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
        //    //sections.Remove(sections.FirstOrDefault());
        //    foreach (var section in sections)
        //    {
        //        var contents = section.Split(new[]
        //        {
        //            "\n",
        //            "\r\n"
        //        }, 2, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
        //        var title = contents.First();
        //        var content = contents.Last();
        //        var variables = new Dictionary<string, string>();
        //        var contentSplit = content.Split(new[]
        //        {
        //            "\n",
        //            "\r\n"
        //        }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
        //        if (title.Equals("DETALLES", StringComparison.OrdinalIgnoreCase))
        //        {
        //            var titleDetalles = contentSplit.First();
        //            var titleDetallesSplit = titleDetalles.Split('|').Select(s => s.Trim()).ToList();
        //            _detalles = new List<Dictionary<string, string>>();
        //            var lista = contentSplit.ToList();
        //            lista.RemoveAt(0);
        //            foreach (var lineDetalles in lista)
        //            {
        //                var contentDetallesSplit = lineDetalles.Split('|').Select(s => s.Trim()).ToList();
        //                if (titleDetallesSplit.Count != contentDetallesSplit.Count)
        //                {
        //                    continue;
        //                }
        //                var dataDetalles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        //                for (var detI = 0; detI < titleDetallesSplit.Count; detI++)
        //                {
        //                    dataDetalles.Add(titleDetallesSplit[detI], contentDetallesSplit[detI]);
        //                }
        //                _detalles.Add(dataDetalles);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var variable in contentSplit)
        //            {
        //                var variableContent = variable.Split(new[]
        //                {
        //                    '='
        //                }, 2).Select(s => s.Trim()).ToList();
        //                var variableName = variableContent.First();
        //                var variableValue = variableContent.LastOrDefault();
        //                if (!variables.ContainsKey(variableName))
        //                {
        //                    variables.Add(variableName, variableValue);
        //                }
        //                else
        //                {
        //                    // ==== Special exclusions
        //                    switch (variableName)
        //                    {
        //                        case "tiporet":
        //                            variables.Add("tiporetd", variableValue);
        //                            break;
        //                        case "montoret":
        //                            variables.Add("montoretd", variableValue);
        //                            break;
        //                        default:
        //                            variables[variableName] = variableValue;
        //                            break;
        //                    }
        //                }
        //            }
        //            _trama.Add(title, variables);
        //        }
        //    }
        //}

        private void ReadTramaop(string documentContent)
        {
            #region 3.3
            _trama = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            //var sectionFirst = documentContent.Split(new[] { "XXX" }, StringSplitOptions.RemoveEmptyEntries, 2);
            var sections = documentContent.Split(new[]
            {
                "\nXXX",
                "\r\nXXX"
            }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
            //sections.Remove(sections.FirstOrDefault());
            foreach (var section in sections)
            {
                var contents = section.Split(new[]
                {
                    "\n",
                    "\r\n"
                }, 2, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
                var title = contents.First();
                var content = contents.Last();
                var variables = new Dictionary<string, string>();
                var contentSplit = content.Split(new[]
                {
                    "\n",
                    "\r\n"
                }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
                if (title.Equals("DETALLES", StringComparison.OrdinalIgnoreCase))
                {
                    var titleDetalles = contentSplit.First();
                    var titleDetallesSplit = titleDetalles.Split('|').Select(s => s.Trim()).ToList();
                    _detalles = new List<Dictionary<string, string>>();
                    var lista = contentSplit.ToList();
                    lista.RemoveAt(0);
                    foreach (var lineDetalles in lista)
                    {
                        var contentDetallesSplitPrb = lineDetalles.Split('|').Select(s => s.Trim()).ToList();
                        var contentDetallesSplit = Regex.Split(lineDetalles, @" \|").Select(s => s.Trim()).ToList();
                        if (titleDetallesSplit.Count != contentDetallesSplit.Count)
                        {
                            continue;
                        }
                        var dataDetalles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                        for (var detI = 0; detI < titleDetallesSplit.Count; detI++)
                        {
                            dataDetalles.Add(titleDetallesSplit[detI], contentDetallesSplit[detI]);
                        }
                        _detalles.Add(dataDetalles);
                    }
                }
                else
                {
                    foreach (var variable in contentSplit)
                    {
                        var variableContent = variable.Split(new[]
                        {
                            '='
                        }, 2).Select(s => s.Trim()).ToList();
                        var variableName = variableContent.First();
                        var variableValue = variableContent.LastOrDefault();
                        if (!variables.ContainsKey(variableName))
                        {
                            variables.Add(variableName, variableValue);
                        }
                        else
                        {
                            // ==== Special exclusions
                            switch (variableName)
                            {
                                case "tiporet":
                                    variables.Add("tiporetd", variableValue);
                                    break;
                                case "montoret":
                                    variables.Add("montoretd", variableValue);
                                    break;
                                default:
                                    variables[variableName] = variableValue;
                                    break;
                            }
                        }
                    }
                    _trama.Add(title, variables);
                }
            }
            #endregion
            #region 3.2
            //_trama = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            ////var sectionFirst = documentContent.Split(new[] { "XXX" }, StringSplitOptions.RemoveEmptyEntries, 2);
            //var sections = documentContent.Split(new[]
            //{
            //    "\nXXX",
            //    "\r\nXXX"
            //}, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
            ////sections.Remove(sections.FirstOrDefault());
            //foreach (var section in sections)
            //{
            //    var contents = section.Split(new[]
            //    {
            //        "\n",
            //        "\r\n"
            //    }, 2, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
            //    var title = contents.First();
            //    var content = contents.Last();
            //    var variables = new Dictionary<string, string>();
            //    var contentSplit = content.Split(new[]
            //    {
            //        "\n",
            //        "\r\n"
            //    }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
            //    if (title.Equals("DETALLES", StringComparison.OrdinalIgnoreCase))
            //    {
            //        var titleDetalles = contentSplit.First();
            //        var titleDetallesSplit = titleDetalles.Split('|').Select(s => s.Trim()).ToList();
            //        _detalles = new List<Dictionary<string, string>>();
            //        var lista = contentSplit.ToList();
            //        lista.RemoveAt(0);
            //        foreach (var lineDetalles in lista)
            //        {
            //            var contentDetallesSplit = lineDetalles.Split('|').Select(s => s.Trim()).ToList();
            //            if (titleDetallesSplit.Count != contentDetallesSplit.Count)
            //            {
            //                continue;
            //            }
            //            var dataDetalles = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            //            for (var detI = 0; detI < titleDetallesSplit.Count; detI++)
            //            {
            //                dataDetalles.Add(titleDetallesSplit[detI], contentDetallesSplit[detI]);
            //            }
            //            _detalles.Add(dataDetalles);
            //        }
            //    }
            //    else
            //    {
            //        foreach (var variable in contentSplit)
            //        {
            //            var variableContent = variable.Split(new[]
            //            {
            //                '='
            //            }, 2).Select(s => s.Trim()).ToList();
            //            var variableName = variableContent.First();
            //            var variableValue = variableContent.LastOrDefault();
            //            if (!variables.ContainsKey(variableName))
            //            {
            //                variables.Add(variableName, variableValue);
            //            }
            //            else
            //            {
            //                // ==== Special exclusions
            //                switch (variableName)
            //                {
            //                    case "tiporet":
            //                        variables.Add("tiporetd", variableValue);
            //                        break;
            //                    case "montoret":
            //                        variables.Add("montoretd", variableValue);
            //                        break;
            //                    default:
            //                        variables[variableName] = variableValue;
            //                        break;
            //                }
            //            }
            //        }
            //        _trama.Add(title, variables);
            //    }
            //}
            #endregion|
        }


        /// <summary>
        /// Gets the value op.
        /// </summary>
        /// <param name="bloque">The bloque.</param>
        /// <param name="variable">The variable.</param>
        /// <returns>System.String.</returns>
        private string GetValueOp(string bloque, string variable)
        {
            var value = "";
            try
            {
                value = _trama[bloque][variable] ?? "";
            }
            catch
            {
                value = "";
            }
            return value;
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"] != null)
            {
                gvConceptos.PageIndex = e.NewPageIndex;
                gvConceptos.DataSource = (DataTable)Session[ViewState["_PageID"].ToString() + "_dataTableConceptos"];
                gvConceptos.DataBind();
            }

        }
    }
}