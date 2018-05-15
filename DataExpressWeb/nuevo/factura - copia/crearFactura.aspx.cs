using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using Control;
using Ionic.Zip;
using System.Xml;

namespace DataExpressWeb
{
    public partial class crearFactura : System.Web.UI.Page
    {
        private BasesDatos DB = new BasesDatos();
        private BasesDatos DBTimer = new BasesDatos();
        private Spool spoolComprobante;
        private string idUser = "";
        private string rucEmisor="";
        private string sucursalUser="";
        private int count = 0;
        private string msj="";
        string codigoControl = "";
        private string formatCero = "0.00";
        private Log log = new Log();
        private int countTimer=0;
        string txt = "";
        byte[] encData_byte;

                string ClaveAcceso="";
                string NoAutorizacion="";
                string FechaAutorizacion="";
                string Estado="";
                string Ambiente="";
                string Emision="";

                string Numero="";

                string Codigo="";
                string Mensaje="";
                string IA="";

        string MensajeT="";

        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            lMsjImpuestos.Text = "";
            if (Session["idUser"] != null)
            {
                idUser = Session["idUser"].ToString();
                sucursalUser = Session["sucursalUser"].ToString();
                if (!Page.IsPostBack)
                {

                    DB.Conectar();
                    DB.CrearComando(@"SELECT rfcemisor from Par_ParametrosSistema");
                    DbDataReader DRSum = DB.EjecutarConsulta();
                    if (DRSum.Read())
                    {
                        rucEmisor = DRSum[0].ToString();
                    }
                    DB.Desconectar();

                    DB.Conectar();
                    DB.CrearComando(@"SELECT domicilio from Cat_Sucursales where idSucursal = @idSucursal ");
                    DB.AsignarParametroCadena("@idSucursal", sucursalUser);
                    DbDataReader DRSuc = DB.EjecutarConsulta();
                    if (DRSuc.Read())
                    {
                        tbDirEstablecimiento.Text = DRSuc[0].ToString();
                    }
                    DB.Desconectar();

                    tbFechaEmision.Text = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                llenarTotales();
                llenarlista(rucEmisor, "emi");
            }
            tbCodigoP.Text = codigoPrincipal();
        }

        //protected void FinishButton_Click_Client(object sender, WizardNavigationEventArgs e)
        //{
        //    switch (Wizard1.ActiveStepIndex)
        //    {
        //        case 2:
        //            Page.Validate("Receptor");
        //            if (!Page.IsValid)
        //            {
        //                e.Cancel = true;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}

        protected void FinishButton_Click(object sender, EventArgs e)
        {
            codigoControl = "";
            lMsjDocumento.Text = "";
            string obligadocontabilidad = "";
            string guiaRemision;
            string datos = "";
            string respuesta = "";
            string ptoEmi = "";
            string claveSucursal = "";
            ClaveAcceso = "";
            NoAutorizacion = "";
            FechaAutorizacion = "";
            Estado = "";
            Ambiente = "";
            Emision = "";
            MensajeT = "";
            Numero = "";
            Codigo = "";
            Mensaje = "";
            IA = "";


            XmlDocument xmlRespuesta = new XmlDocument();
            codigoControl = "";
            if (String.IsNullOrEmpty(ddlSucursal.SelectedValue))
            {
                ddlSucursal.Items.Clear();
                ddlSucursal.DataBind();

            }
            if (String.IsNullOrEmpty(ddlEmision.SelectedValue))
            {
                ddlEmision.Items.Clear();
                ddlEmision.DataBind();
            }
            if (String.IsNullOrEmpty(ddlAmbiente.SelectedValue))
            {
                ddlAmbiente.Items.Clear();
                ddlAmbiente.DataBind();
            }
            if (String.IsNullOrEmpty(ddlComprobante.SelectedValue))
            {
                ddlComprobante.Items.Clear();
                ddlComprobante.DataBind();
            }
            if (String.IsNullOrEmpty(ddlPtoEmi.SelectedValue))
            {
                ddlPtoEmi.Items.Clear();
                ddlPtoEmi.DataBind();
                ptoEmi = ddlPtoEmi.SelectedValue;
            }
            if (Session["estab"] != null)
                claveSucursal = Session["estab"].ToString();
            if (Session["ptoemi"] != null)
                ptoEmi = Session["ptoemi"].ToString();

            if (cbObligado.Checked)
            {
                obligadocontabilidad = "SI";
            }
            else
            {
                obligadocontabilidad = "NO";
            }
            try
            {
                tbSecuencial.Text = obtenerSecuencial(claveSucursal, ptoEmi, ddlComprobante.SelectedValue);
                spoolComprobante = new Spool();
                spoolComprobante.xmlComprobante();
                spoolComprobante.InformacionTributaria(ddlAmbiente.SelectedValue, ddlEmision.SelectedValue, tbRazonSocial.Text, tbNombreComercial.Text,
                 tbRuc.Text, "", ddlComprobante.SelectedValue, claveSucursal, ptoEmi, tbSecuencial.Text, tbDirMatriz.Text, tbEmail.Text);

                spoolComprobante.infromacionDocumento(tbFechaEmision.Text, tbDirEstablecimiento.Text, tbContribuyenteEspecial.Text, obligadocontabilidad,
                    ddlTipoIdentificacion.SelectedValue, "", tbRazonSocialComprador.Text, tbIdentificacionComprador.Text, tbMoneda.Text,
                    "", "", "", "", "", "", "", "", "", "", "", formatCero, "");

                spoolComprobante.cantidades(tbSubtotal12.Text, tbSubtotal0.Text, tbSubtotalNoSujeto.Text, tbTotalSinImpuestos.Text,
                    tbTotalDescuento.Text, tbICE.Text, tbIVA12.Text, tbImporteTotal.Text, tbPropinas.Text, tbImporteaPagar.Text);

                spoolComprobante.datosHerbalife(System.DateTime.Now.ToString("yyyyMMddHHmmss"), "1900-01-01T00:00:00", "0.0", "", "", "",
                           "", "0.0", "", "", "", "", "", "", "", "E3", "", "", "", "");

                spoolComprobante.totalImpuestos(idUser);
                spoolComprobante.detalles(idUser);
                spoolComprobante.impuestos(idUser);
                spoolComprobante.detallesAdicionales(idUser);
                spoolComprobante.informacionAdicional(idUser);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                datos = spoolComprobante.generarDocumento();
                codigoControl = spoolComprobante.codigoControl;

              // wsGenerarDoc.wsGenerarDocSoapClient crearDocManual = new wsGenerarDoc.wsGenerarDocSoapClient();
                wsGenerarDocSSL.wsGenerarDocSoapClient crearDocManual = new wsGenerarDocSSL.wsGenerarDocSoapClient();

                txt = spoolComprobante.generarDocumento();
                codigoControl = spoolComprobante.codigoControl;
                DB.Conectar();
                DB.CrearComando(@"insert into Log_Trama
                                (Trama,fecha)
                                values
                                (@Trama,@fecha)");
                DB.AsignarParametroCadena("@Trama","FM ---"+ txt.Replace("'", ""));
                DB.AsignarParametroCadena("@fecha", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                DB.EjecutarConsulta1();
                DB.Desconectar();

                //crearDocManual = new wsGenerarDoc.wsGenerarDocSoapClient();
                crearDocManual = new wsGenerarDocSSL.wsGenerarDocSoapClient();
                encData_byte = new byte[txt.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(txt);

                xmlRespuesta.InnerXml = Base64String_String(crearDocManual.procesarDocumento(Convert.ToBase64String(encData_byte), "", "", "1"));
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                ClaveAcceso = xmlRespuesta.GetElementsByTagName("claveAcceso")[0].InnerText;
                NoAutorizacion = xmlRespuesta.GetElementsByTagName("numeroAutorizacion")[0].InnerText;
                FechaAutorizacion = xmlRespuesta.GetElementsByTagName("fechaAutorizacion")[0].InnerText;
                Estado = xmlRespuesta.GetElementsByTagName("estado")[0].InnerText;
                Mensaje = xmlRespuesta.GetElementsByTagName("msj")[0].InnerText;
                MensajeT = xmlRespuesta.GetElementsByTagName("msjT")[0].InnerText;

                Codigo = xmlRespuesta.GetElementsByTagName("codigo")[0].InnerText;

                if (!Estado.Equals("N0")&& !String.IsNullOrEmpty(Estado))
                {

                    DB.Conectar();
                    DB.CrearComando(@"delete from Dat_detallesTemp where id_Empleado=@id_Empleado");
                    DB.AsignarParametroCadena("@id_Empleado", idUser);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    Response.Redirect("~/Documentos.aspx",false);
                    //Response.Redirect("~/Procesando.aspx", false);
                }
                else
                {
                    lMsjDocumento.Text = "No se pudo crear el Comprobante." + Environment.NewLine;
                    lMsjDocumento.Text += "ClaveAcceso:" + ClaveAcceso + Environment.NewLine;
                    lMsjDocumento.Text += "Número: " + Numero + Environment.NewLine;
                    lMsjDocumento.Text += "Mensaje: " + Mensaje + Environment.NewLine;
                    lMsjDocumento.Text += "IA: " + MensajeT + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                msj = log.PA_mensajes("EM011")[0];

                lMsjDocumento.Text = msj;
                lMsjDocumento.Text = ex.ToString();
                log.mensajesLog("EM011", "", ex.Message, "0", "");
            }
        }

        protected void tbPU_TextChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        protected void tbDescuento_TextChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        private string codigoPrincipal()
        {
            int aux = 0;
            string code = "";
            DB.Conectar();
            DB.CrearComando(@"SELECT count(id_Empleado) from Dat_detallesTemp WHERE id_Empleado=@id_Empleado");
            DB.AsignarParametroCadena("@id_Empleado", idUser);
            DbDataReader DRDT = DB.EjecutarConsulta();
            if (DRDT.Read())
            {
                aux = Convert.ToInt32(DRDT[0]) + 1;
            }
            DB.Desconectar();

            if (aux.ToString().Length == 1) { code = "00000000" + aux.ToString(); } if (aux.ToString().Length == 2) { code = "0000000" + aux.ToString(); }
            if (aux.ToString().Length == 3) { code = "000000" + aux.ToString(); } if (aux.ToString().Length == 4) { code = "00000" + aux.ToString(); }
            if (aux.ToString().Length == 5) { code = "0000" + aux.ToString(); } if (aux.ToString().Length == 6) { code = "000" + aux.ToString(); }
            if (aux.ToString().Length == 7) { code = "00" + aux.ToString(); } if (aux.ToString().Length == 8) { code = "0" + aux.ToString(); }
            if (aux.ToString().Length == 9) { code = aux.ToString(); }
            return code;
        }

        protected void bAgregarImpuesto_Click(object sender, EventArgs e)
        {
            string valImpuesto = "";
            if (Convert.ToDecimal(tbImporteConcepto.Text) > 0)
            {

            if (tbTarifa.Text != "No Sujeto")
            {
                valImpuesto = tbTarifa.Text;
                if (Convert.ToInt32(tbValor.Text.Length) > 0)
                {
                    DB.Conectar();
                    DB.CrearComando(@"insert into Dat_ImpuestosDetallesTemp
                           (codigo,codigoPorcentaje,baseImponible,tarifa,valor,id_DetallesTemp,codigoTemp,id_Empleado,tipo)
                           values
                           (@codigo,@codigoPorcentaje,@baseImponible,@tarifa,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado,@tipo)");
                    DB.AsignarParametroCadena("@codigo", "2");
                    DB.AsignarParametroCadena("@codigoPorcentaje", tbCodigoIDP.Text);
                    DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                    DB.AsignarParametroCadena("@tarifa", valImpuesto);
                    DB.AsignarParametroCadena("@valor", tbValor.Text);
                    DB.AsignarParametroCadena("@id_DetallesTemp", "");
                    DB.AsignarParametroCadena("@codigoTemp", tbCodigoP.Text);
                    DB.AsignarParametroCadena("@id_Empleado", idUser);
                    DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
                    DB.EjecutarConsulta1();
                    DB.Desconectar();

                    SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                    SqlDataImpuestosConceptos.SelectParameters[1].DefaultValue = tbCodigoP.Text;
                    SqlDataImpuestosConceptos.DataBind();
                    gvImpuestosDetalles.DataBind();
                }
            }
            else
            {
                valImpuesto = "0";
                if (tbValor.Text.Length > 0)
                {
                    DB.Conectar();
                    DB.CrearComando(@"insert into Dat_ImpuestosDetallesTemp
                           (codigo,codigoPorcentaje,baseImponible,valor,id_DetallesTemp,codigoTemp,id_Empleado,tipo)
                           values
                           (@codigo,@codigoPorcentaje,@baseImponible,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado,@tipo)");
                    DB.AsignarParametroCadena("@codigo", tbCodigoID.Text);
                    DB.AsignarParametroCadena("@codigoPorcentaje", tbCodigoIDP.Text);
                    DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                    DB.AsignarParametroCadena("@valor", valImpuesto);
                    DB.AsignarParametroCadena("@id_DetallesTemp", "");
                    DB.AsignarParametroCadena("@codigoTemp", tbCodigoP.Text);
                    DB.AsignarParametroCadena("@id_Empleado", idUser);
                    DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
                    DB.EjecutarConsulta1();
                    DB.Desconectar();

                    SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                    SqlDataImpuestosConceptos.SelectParameters[1].DefaultValue = tbCodigoP.Text;
                    SqlDataImpuestosConceptos.DataBind();
                    gvImpuestosDetalles.DataBind();
                }
            }
            tbCodigoID.Text = "";
            tbCodigoIDP.Text = "";
            tbTarifa.Text = formatCero;
            tbValor.Text = formatCero;

            //Agregado cg1

            bAgregarConcepto_Click();
            }
             else
             {
                 lMsjImpuestos.Text = "Debe corregir Subtotal";
             }
        }

        protected void bAgregarC()
        {
            string valImpuesto = "";
            if (Convert.ToDecimal(tbImporteConcepto.Text) > 0)
            {

                if (tbTarifa.Text != "No Sujeto")
                {
                    valImpuesto = tbTarifa.Text;
                    if (Convert.ToInt32(tbValor.Text.Length) > 0)
                    {
                        DB.Conectar();
                        DB.CrearComando(@"insert into Dat_ImpuestosDetallesTemp
                           (codigo,codigoPorcentaje,baseImponible,tarifa,valor,id_DetallesTemp,codigoTemp,id_Empleado,tipo)
                           values
                           (@codigo,@codigoPorcentaje,@baseImponible,@tarifa,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado,@tipo)");
                        DB.AsignarParametroCadena("@codigo", "2");
                        DB.AsignarParametroCadena("@codigoPorcentaje", tbCodigoIDP.Text);
                        DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                        DB.AsignarParametroCadena("@tarifa", valImpuesto);
                        DB.AsignarParametroCadena("@valor", tbValor.Text);
                        DB.AsignarParametroCadena("@id_DetallesTemp", "");
                        DB.AsignarParametroCadena("@codigoTemp", tbCodigoP.Text);
                        DB.AsignarParametroCadena("@id_Empleado", idUser);
                        DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
                        DB.EjecutarConsulta1();
                        DB.Desconectar();

                        SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                        SqlDataImpuestosConceptos.SelectParameters[1].DefaultValue = tbCodigoP.Text;
                        SqlDataImpuestosConceptos.DataBind();
                        gvImpuestosDetalles.DataBind();
                    }
                }
                else
                {
                    valImpuesto = "0";
                    if (tbValor.Text.Length > 0)
                    {
                        DB.Conectar();
                        DB.CrearComando(@"insert into Dat_ImpuestosDetallesTemp
                           (codigo,codigoPorcentaje,baseImponible,valor,id_DetallesTemp,codigoTemp,id_Empleado,tipo)
                           values
                           (@codigo,@codigoPorcentaje,@baseImponible,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado,@tipo)");
                        DB.AsignarParametroCadena("@codigo", tbCodigoID.Text);
                        DB.AsignarParametroCadena("@codigoPorcentaje", tbCodigoIDP.Text);
                        DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                        DB.AsignarParametroCadena("@valor", valImpuesto);
                        DB.AsignarParametroCadena("@id_DetallesTemp", "");
                        DB.AsignarParametroCadena("@codigoTemp", tbCodigoP.Text);
                        DB.AsignarParametroCadena("@id_Empleado", idUser);
                        DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
                        DB.EjecutarConsulta1();
                        DB.Desconectar();

                        SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                        SqlDataImpuestosConceptos.SelectParameters[1].DefaultValue = tbCodigoP.Text;
                        SqlDataImpuestosConceptos.DataBind();
                        gvImpuestosDetalles.DataBind();
                    }
                }
                tbCodigoID.Text = "";
                tbCodigoIDP.Text = "";
                tbTarifa.Text = formatCero;
                tbValor.Text = formatCero;

                //Agregado cg1

                bAgregarConcepto_Click();
            }
            else
            {
                lMsjImpuestos.Text = "Debe corregir Subtotal";
            }
        }
//        protected void bAgregarDetalleAdic_Click(object sender, EventArgs e)
//        {
//            if (Convert.ToDouble(tbImporteConcepto.Text) > 0.01)
//            {
//                DB.Conectar();
//                DB.CrearComando(@"insert into DetallesAdicionalesTemp
//                           (nombre,valor,id_DetallesTemp,codigoTemp,id_Empleado)
//                           values
//                           (@nombre,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado)");
//                DB.AsignarParametroCadena("@nombre", tbNombreConcepto.Text);
//                DB.AsignarParametroCadena("@valor", tbValorConcepto.Text);
//                DB.AsignarParametroCadena("@id_DetallesTemp", "");
//                DB.AsignarParametroCadena("@codigoTemp", tbCodigoP.Text);
//                DB.AsignarParametroCadena("@id_Empleado", idUser);
//                DB.EjecutarConsulta1();
//                DB.Desconectar();
//                SqlDataDetAdicionalesDetalles.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
//                SqlDataDetAdicionalesDetalles.SelectParameters[1].DefaultValue = tbCodigoP.Text;
//                SqlDataDetAdicionalesDetalles.DataBind();
//                gvDetAdic.DataBind();
//            }
//            tbValorConcepto.Text = "";
//            tbNombreConcepto.Text = "";
//        }

        protected void bAgregarConcepto_Click()
        {
            int countImp = 0;
            Page.Validate("Deta");
            if (Page.IsValid)
            {
                DB.Conectar();
                DB.CrearComando(@"SELECT count(*) FROM Dat_ImpuestosDetallesTemp WHERE id_Empleado=@empleado and codigoTemp=@codigoPrincipal");
                DB.AsignarParametroCadena("@empleado", idUser);
                DB.AsignarParametroCadena("@codigoPrincipal", tbCodigoP.Text);
                DbDataReader DRCont = DB.EjecutarConsulta();
                if (DRCont.Read())
                {
                    countImp = Convert.ToInt32(DRCont[0]);
                }
                DB.Desconectar();
                if (countImp > 0)
                {
                    DB.Conectar();
                    DB.CrearComando(@"insert into Dat_DetallesTemp
                           (codigoPrincipal,codigoAuxiliar,descripcion,cantidad,precioUnitario,
                            descuento,precioTotalSinImpuestos,id_Empleado)
                           values
                           (@codigoPrincipal,@codigoAuxiliar,@descripcion,@cantidad,@precioUnitario,
                            @descuento,@precioTotalSinImpuestos,@id_Empleado)");
                    DB.AsignarParametroCadena("@codigoPrincipal", tbCodigoP.Text);
                    DB.AsignarParametroCadena("@codigoAuxiliar", "");//tbCodigoA.Text);
                    DB.AsignarParametroCadena("@descripcion", tbCodigoA0.Text);
                    DB.AsignarParametroCadena("@cantidad", tbCantidad.Text);
                    DB.AsignarParametroCadena("@precioUnitario", tbPU.Text);
                    DB.AsignarParametroCadena("@descuento", tbDescuento.Text);
                    DB.AsignarParametroCadena("@precioTotalSinImpuestos", tbImporteConcepto.Text);
                    DB.AsignarParametroCadena("@id_Empleado", idUser);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    SqlDataDetalles.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                    SqlDataDetalles.DataBind();
                    gvDetalles.DataBind();

                    SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                    SqlDataImpuestosConceptos.SelectParameters[1].DefaultValue = Session["idUser"].ToString();
                    SqlDataImpuestosConceptos.DataBind();
                    gvImpuestosDetalles.DataBind();
                    tbCantidad.Text = String.Format("{0:f}",1);
                    //tbCodigoA.Text = "";
                    tbCodigoP.Text = codigoPrincipal();
                    tbCodigoA0.Text = "";
                    tbPU.Text = "0";
                    tbDescuento.Text = "0";
                    tbImporteConcepto.Text = "0";
                    tbBaseImponible.Text = "0";
                }
                else
                {
                    lMsjImpuestos.Text = "Necesitas Agregar al menos un Impuesto.";
                }
            }
                codigoPrincipal();
        }

        protected void bAgregarCA_Click(object sender, EventArgs e)
        {

        }
        protected void tbTarifa_TextChanged(object sender, EventArgs e)
        {
            calcularImpuesto();
        }

        private void llenarlista(String ruc, String tipo)
        {
            String sql = "";
            if (tipo == "emi") { sql = @"SELECT * FROM Cat_Emisor WHERE RFCEMI=@ruc"; }
            if (tipo == "rec") { sql = @"SELECT * FROM Cat_Receptor WHERE RFCREC=@ruc"; }

            DB.Conectar();
            DB.CrearComando(sql);
            DB.AsignarParametroCadena("@ruc", ruc);
            DbDataReader DRSum = DB.EjecutarConsulta();
            while (DRSum.Read())
            {
                if (tipo == "emi")
                {
                    tbRuc.Text = DRSum[1].ToString();
                    tbRazonSocial.Text = DRSum[2].ToString();
                    tbNombreComercial.Text = DRSum[3].ToString();
                    tbDirMatriz.Text = DRSum[4].ToString();
                }
                if (tipo == "rec")
                {
                    tbIdentificacionComprador.Text = DRSum[1].ToString();
                    tbRazonSocialComprador.Text = DRSum[2].ToString();
                    ddlTipoIdentificacion.SelectedValue = DRSum[5].ToString();
                    tbEmail.Text = DRSum[6].ToString();
                    txt_dir_cli.Text = DRSum[7].ToString();
                  //  txt_fono.Text = DRSum[8].ToString();

                }
            }
            DB.Desconectar();
        }

        private void llenarlista()
        {
            String sql = "";
            sql = @"SELECT * FROM Cat_Emisor";
            DB.Conectar();
            DB.CrearComando(sql);
            DbDataReader DRSum = DB.EjecutarConsulta();
            if (DRSum.Read())
            {
                tbRuc.Text = DRSum[1].ToString();
                tbRazonSocial.Text = DRSum[2].ToString();
                tbNombreComercial.Text = DRSum[3].ToString();
                tbDirMatriz.Text = DRSum[4].ToString();
            }
            DB.Desconectar();
        }

        protected void tbIdentificacionComprador_TextChanged(object sender, EventArgs e)
        {
            if (tbIdentificacionComprador.Text.Length > 0)
            {
                llenarlista(tbIdentificacionComprador.Text, "rec");
            }
        }

        public void llenarTotales()
        {
            tbSubtotal0.Text = formatCero;
            tbSubtotal12.Text = formatCero;
            tbSubtotalNoSujeto.Text = formatCero;
            tbTotalSinImpuestos.Text = formatCero;
            tbTotalDescuento.Text = formatCero;
            tbICE.Text = formatCero;
            tbIVA12.Text = formatCero;
            tbImporteTotal.Text = formatCero;
            if (Convert.ToDecimal(tbPropinas.Text)==0)
            {
                tbPropinas.Text = formatCero;
            }
            tbImporteaPagar.Text = formatCero;

            //Total sin impuestos, descuentos
            DB.Conectar();
            DB.CrearComando(@"select ISNULL(sum(precioTotalSinImpuestos),0),ISNULL(sum(descuento),0) from Dat_detallesTemp WHERE id_Empleado=@empleado");
            DB.AsignarParametroCadena("@empleado", idUser);
            DbDataReader DRSum = DB.EjecutarConsulta();
            if (DRSum.Read())
            {
                tbTotalSinImpuestos.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum[0].ToString()));
                tbTotalDescuento.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum[1].ToString()));
            }
            DB.Desconectar();

            //Impuesto del 12
            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(Dat_ImpuestosDetallesTemp.valor),0),ISNULL(sum(Dat_DetallesTemp.precioTotalSinImpuestos),0)
                              FROM  Dat_ImpuestosDetallesTemp INNER JOIN
                                    Dat_DetallesTemp  ON Dat_ImpuestosDetallesTemp.codigoTemp = Dat_DetallesTemp .codigoPrincipal
                              WHERE   CAST(tarifa AS INT)='12' AND Dat_ImpuestosDetallesTemp.tipo='IVA' AND Dat_ImpuestosDetallesTemp.id_Empleado=@empleado");
            DB.AsignarParametroCadena("@empleado", idUser);
            DbDataReader DRSum1 = DB.EjecutarConsulta();
            if (DRSum1.Read())
            {
                tbIVA12.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum1[0].ToString()));
                tbSubtotal12.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum1[1].ToString()));
            }
            DB.Desconectar();
            //Impuesto del 0
            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(Dat_DetallesTemp.precioTotalSinImpuestos),0)
                              FROM  Dat_ImpuestosDetallesTemp INNER JOIN
                                    Dat_DetallesTemp  ON Dat_ImpuestosDetallesTemp.codigoTemp = Dat_DetallesTemp .codigoPrincipal
                              WHERE   CAST(tarifa AS INT)='0' AND Dat_ImpuestosDetallesTemp.tipo='IVA' AND Dat_ImpuestosDetallesTemp.id_Empleado=@empleado");
            DB.AsignarParametroCadena("@empleado", idUser);
            DbDataReader DRSum2 = DB.EjecutarConsulta();
            if (DRSum2.Read())
            {
                tbSubtotal0.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum2[0].ToString()));
            }
            DB.Desconectar();
            //No sujeto de IVA
            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(Dat_DetallesTemp.precioTotalSinImpuestos),0)
                              FROM  Dat_ImpuestosDetallesTemp INNER JOIN
                                    Dat_DetallesTemp  ON Dat_ImpuestosDetallesTemp.codigoTemp = Dat_DetallesTemp .codigoPrincipal
                              WHERE   tarifa IS NULL AND Dat_ImpuestosDetallesTemp.tipo='IVA' AND  Dat_ImpuestosDetallesTemp.id_Empleado=@empleado");
            DB.AsignarParametroCadena("@empleado", idUser);
            DbDataReader DRSum3 = DB.EjecutarConsulta();
            if (DRSum3.Read())
            {
                tbSubtotalNoSujeto.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum3[0].ToString()));
            }
            DB.Desconectar();
            //ICE
            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(Dat_ImpuestosDetallesTemp.valor),0)
                              FROM  Dat_ImpuestosDetallesTemp INNER JOIN
                                    Dat_DetallesTemp  ON Dat_ImpuestosDetallesTemp.codigoTemp = Dat_DetallesTemp .codigoPrincipal
                              WHERE   Dat_ImpuestosDetallesTemp.tipo='ICE' AND Dat_ImpuestosDetallesTemp.id_Empleado=@empleado");
            DB.AsignarParametroCadena("@empleado", idUser);
            DbDataReader DRSum4 = DB.EjecutarConsulta();
            if (DRSum4.Read())
            {
                tbICE.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum4[0].ToString()));
            }
            DB.Desconectar();
            tbImporteTotal.Text = String.Format("{0:f}", Convert.ToDecimal(tbTotalSinImpuestos.Text) - Convert.ToDecimal(tbDescuento.Text) +
                                   Convert.ToDecimal(tbICE.Text) + Convert.ToDecimal(tbIVA12.Text));
            tbImporteaPagar.Text = String.Format("{0:f}", Convert.ToDecimal(tbImporteTotal.Text) + Convert.ToDecimal(tbPropinas.Text));
        }

        protected void ddlTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoImpuesto.SelectedValue == "3")
            {
                ddlTasaIVA.Visible = false;
                tbTarifa.Visible = true;
                //RegularExpressionValidator2.Visible = true;
                //RequiredFieldValidator16.Visible = true;
                tbCodigoIDP.Text = "";
                tbTarifa.Text = formatCero;
            }
            else
            {
                ddlTasaIVA.Visible = true;
                tbTarifa.Visible = false;
                //RegularExpressionValidator2.Visible = false;
              //  RequiredFieldValidator16.Visible = false;
            }
            importeConcepto();
        }

        private void calcularImpuesto()
        {
            decimal tarifa = 0;
            if (tbTarifa.Text != "No Sujeto")
            {
                try
                {
                    tarifa = Convert.ToDecimal(cc(tbTarifa.Text));
                }
                catch (Exception ex)
                {
                    lMsjImpuestos.Text = "No es un numero Válido.";
                }
                tbValor.Text = String.Format("{0:f}", (tarifa * Convert.ToDecimal(tbBaseImponible.Text)) / 100);
            }
            else
            {
                tbValor.Text = "0";
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        private void importeConcepto()
        {
            lMsjImpuestos.Text = "";
            tbImporteConcepto.Text = String.Format("{0:f}", (Convert.ToDecimal(cc(tbPU.Text)) * Convert.ToDecimal(cc(tbCantidad.Text))) - Convert.ToDecimal(cc(tbDescuento.Text)));
            tbBaseImponible.Text = tbImporteConcepto.Text;

            tbCodigoID.Text = ddlTipoImpuesto.SelectedValue;
            tbTarifa.Text = ddlTasaIVA.SelectedValue;
            tbTarifa.ReadOnly = false;
            if (tbTarifa.Text == "12" || tbTarifa.Text == "12,00" || tbTarifa.Text == "12.00")
            {
                tbCodigoIDP.Text = "2";
            }
            if (tbTarifa.Text == "0" || tbTarifa.Text == "0.00" || tbTarifa.Text == "0,00")
            {
                tbCodigoIDP.Text = "0";
            }
            if (tbTarifa.Text == "No Sujeto")
            {
                tbCodigoIDP.Text = "6";
            }
            if (ddlTipoImpuesto.SelectedValue == "3")
            {
                tbCodigoIDP.Text = "";
                tbTarifa.Text = formatCero;
                tbValor.Text = formatCero;
            }
            else
            {
                calcularImpuesto();
            }

        }

        protected void StepNextButton_Click(object sender, WizardNavigationEventArgs e)
        {
            switch (Wizard1.ActiveStepIndex)
            {
                case 1:
                    DB.Conectar();
                    DB.CrearComando(@"SELECT count(*) from Dat_detallesTemp WHERE id_Empleado=@empleado");
                    DB.AsignarParametroCadena("@empleado", idUser);
                    DbDataReader DRCont = DB.EjecutarConsulta();
                    if (DRCont.Read())
                    {
                        count = Convert.ToInt32(DRCont[0]);
                    }
                    DB.Desconectar();
                    if (count < 1) { lMsjDetalles.Text = "Necesitas Agregar al menos un Detalle."; e.Cancel = true; } else { lMsjDetalles.Text = ""; }
                    break;
                case 2:
                    Page.Validate("Receptor");
                    if (!Page.IsValid)
                    {
                        e.Cancel = true;
                    }
                    break;
                default:
                    break;
            }
        }

        private string cc(string numero) {
            if (String.IsNullOrEmpty(numero))
            {
                numero = formatCero;
            }

            return numero;
        }
        protected void Wizard1_ActiveStepChanged(object sender, EventArgs e)
        {

        }

        protected void tbPropinas_TextChanged(object sender, EventArgs e)
        {
            llenarTotales();
        }

        protected void tbCantidad_TextChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        protected void FinishButton_Click(object sender, WizardNavigationEventArgs e)//(object sender, EventArgs e)
        {
            Page.Validate("Receptor");
            if (!Page.IsValid)
            {
                e.Cancel = true;
            }
            else
            {
                codigoControl = "";
            }
	}
        public static string Base64String_String(string b64)
        {
            try
            {
                return ByteArray_String(Base64String_ByteArray(b64));
            }
            catch
            {
                return b64;
            }

        }//Base64String_String

        public string obtenerSecuencial(string estab, string ptoEmi,string codDoc )
        {
            string ultimoFolio = "";
            string code = "";

                DB.Conectar();
                DB.CrearComando("select ISNULL(MAX(CONVERT(int,secuencial)),0)+ 1 from Dat_General where estab= @estab and ptoEmi=@ptoEmi and codDoc = @codDoc");
                DB.AsignarParametroCadena("@estab", estab);
                DB.AsignarParametroCadena("@ptoEmi", ptoEmi);
                DB.AsignarParametroCadena("@codDoc", codDoc);
                DbDataReader DR = DB.EjecutarConsulta();

                if (DR.Read())
                {
                    ultimoFolio = DR[0].ToString().Trim();
                }
                DB.Desconectar();
            code = ultimoFolio;

            switch (ultimoFolio.ToString().Trim().Length)
            {
                case 1:
                    code = "00000000" + ultimoFolio.ToString().Trim();
                    break;
                case 2:
                    code = "0000000" + ultimoFolio.ToString().Trim();
                    break;
                case 3:
                    code = "000000" + ultimoFolio.ToString().Trim();
                    break;
                case 4:
                    code = "00000" + ultimoFolio.ToString().Trim();
                    break;
                case 5:
                    code = "0000" + ultimoFolio.ToString().Trim();
                    break;
                case 6:
                    code = "000" + ultimoFolio.ToString().Trim();
                    break;
                case 7:
                    code = "00" + ultimoFolio.ToString().Trim();
                    break;
                case 8:
                    code = "0" + ultimoFolio.ToString().Trim();
                    break;
                case 9:
                    code = ultimoFolio.ToString().Trim();
                    break;
            }

            return code;
        }

        private static string ByteArray_String(byte[] b)
        {
            return new string(System.Text.Encoding.UTF8.GetChars(b));
        }//ByteArray_String
        private static byte[] Base64String_ByteArray(string s)
        {
            return Convert.FromBase64String(s);
        }//Base64String_ByteArray
    }
}