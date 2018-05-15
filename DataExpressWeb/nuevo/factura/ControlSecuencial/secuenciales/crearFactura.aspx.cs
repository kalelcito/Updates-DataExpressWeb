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
using System.Web.UI;
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
        //ArrayList arraylCargosxServico;
        string nombrecajero = "";
       decimal  totCargoXservicio ;
        decimal cargoxservicio = 0;

        string numFacturas = "0";
        string numFacturascos = "0";
        string folioFinal = "";
        string folioFinalcos = "";
        int totalTimbres = 0;
        int totalTimbrescos = 0;
        decimal sumpropinas ;
        String[] propinastk;
        List<string> propinaslist;

        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            //totalDocus();
            //consultaFolio();
            //totalTimbres = Convert.ToInt32(folioFinal) - Convert.ToInt32(numFacturas);
            propinaslist = new List<string>();

            //if (Convert.ToInt16(numFacturas) < Convert.ToInt16(folioFinal))
            //{

                // totalTimbres = Convert.ToInt32(folioFinal) - Convert.ToInt32(numFacturas);
                // tbTimbrestot.Text = Convert.ToString(totalTimbres);

                lMsjImpuestos.Text = "";
                if (Session["idUser"] != null)
                {
                    idUser = Session["idUser"].ToString();
                    sucursalUser = Session["sucursalUser"].ToString();
                    if (!Page.IsPostBack)
                    {


                        DB.Conectar();
                        DB.CrearComando(@"SELECT rfcemisor from ParametrosSistema");
                        DbDataReader DRSum = DB.EjecutarConsulta();
                        if (DRSum.Read())
                        {
                            rucEmisor = DRSum[0].ToString();
                        }
                        DB.Desconectar();

                        DB.Conectar();
                        DB.CrearComando(@"SELECT domicilio from Sucursales where idSucursal = @idSucursal ");
                        DB.AsignarParametroCadena("@idSucursal", sucursalUser);
                        DbDataReader DRSuc = DB.EjecutarConsulta();
                        if (DRSuc.Read())
                        {
                            tbDirEstablecimiento.Text = DRSuc[0].ToString();
                        }
                        DB.Desconectar();

                        tbFechaEmision.Text = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                        // tbCodigoP.Text = "00000001";

                    }
                    llenarTotales();
                    llenarlista(rucEmisor, "emi");

                }
                tbCodigoP.Text = codigoPrincipal();
            //}
            //else { Wizard1.Enabled = false; }

                tbFolio.Text = consultarSecuencial(ddlComprobante.SelectedValue, ddlSucursal.SelectedValue, ddlPtoEmi.SelectedValue);
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

            Wizard1.Enabled = false;
                //if (ddlMetodoPago.SelectedValue !="Selecciona una opción")
                //{
                //    if (tbFechaV.Text.Length > 0)

                //    {

                        codigoControl = "";
                        lMsjDocumento.Text = "";
                        string obligadocontabilidad = "";
                        string guiaRemision;
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
                        }

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
                            obligadocontabilidad = "SI";
                            string auxtipovemta = "";
                            //if (tbIdentificacionComprador.Text.Length == 13)
                            //{auxtipovemta = "04";  }

                            spoolComprobante = new Spool();
                            spoolComprobante.xmlComprobante();
                            spoolComprobante.InformacionTributaria(ddlAmbiente.SelectedValue, ddlEmision.SelectedValue, tbRazonSocial.Text, tbNombreComercial.Text,
                             tbRuc.Text, "", ddlComprobante.SelectedValue, "001","001", tbFolio.Text, tbDirMatriz.Text, tbEmail.Text);
                            spoolComprobante.infromacionDocumento(tbFechaEmision.Text, tbDirEstablecimiento.Text, "", obligadocontabilidad,
                                ddlTipoIdentificacion.SelectedValue, "", tbRazonSocialComprador.Text, tbIdentificacionComprador.Text, tbMoneda.Text,
                                "", "", "", "", "", "", "", "", "", "", "", formatCero, "");
                            spoolComprobante.cantidades(tbSubtotal12.Text, tbSubtotal0.Text, tbSubtotalNoSujeto.Text, tbTotalSinImpuestos.Text,
                                tbTotalDescuento.Text, tbICE.Text, tbIVA12.Text, tbImporteTotal.Text, tbPropinas.Text, tbImporteaPagar.Text);
                            spoolComprobante.totalImpuestos(idUser);
                            spoolComprobante.detalles(idUser);
                            spoolComprobante.impuestos(idUser);
                            spoolComprobante.detallesAdicionales(idUser);
                            spoolComprobante.informacionAdicional(idUser);


                            consultaEmp(Session["idUser"].ToString());
                            //INFO ADICONAL
                            spoolComprobante.infoSatcom(txt_dir_cli.Text, txt_fono.Text, tbObservaciones.Text, ddlMetodoPago.SelectedValue, tbMonto.Text, tbEmail.Text);




                            codigoControl = spoolComprobante.generarDocumento();




                            if (!String.IsNullOrEmpty(codigoControl))
                            {
                                registroSecuencial(ddlComprobante.SelectedValue, ddlSucursal.SelectedValue, ddlPtoEmi.SelectedValue, tbFolio.Text);


                                Session["codigoControl"] = codigoControl;
                                //Response.Redirect("~/Procesando.aspx");
                                Response.Redirect("~/Procesando.aspx", false);




                            }
                            else
                            {
                                lMsjDocumento.Text = "No se pudo crear el Comprobante.";
                            }
                        }
                        catch (Exception ex)
                        {
                            msj = log.PA_mensajes("EM011")[0];
                            lMsjDocumento.Text = msj;
                            log.mensajesLog("EM011", "", ex.Message, "Crear Factura", "");
                        }
            //        }
            //        else { lMsjDocumento.Text = "Debes seleccionar fecha de pago"; }

            //}
            //else { lMsjDocumento.Text = "Debes seleccionar método de pago"; }

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
            DB.CrearComando(@"SELECT count(id_Empleado) FROM DetallesTemp WHERE id_Empleado=@id_Empleado");
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

        public string  obtenerSecuencial()
        {
            string ultimoFolio = "";
            string code = "";

            DB.Conectar();
            DB.CrearComando("select ISNULL(MAX(CONVERT(int,secuencial)),0)+ 1 from General where estab= @estab and ptoEmi=@ptoEmi and codDoc = @codDoc");
            DB.AsignarParametroCadena("@estab", "001");
            DB.AsignarParametroCadena("@ptoEmi", "002");
            DB.AsignarParametroCadena("@codDoc", "01");
            DbDataReader DR = DB.EjecutarConsulta();

            if (DR.Read())
            {
                ultimoFolio = DR[0].ToString().Trim();
            }
            DB.Desconectar();
            return ultimoFolio;
        }

        public string consultarSecuencial(string codoc, string estab, string ptoemi)
        {
            string ultimoFolio = "";
            string code = "";

            DB.Conectar();
            DB.CrearComando("select ISNULL(MAX(CONVERT(int,secuencial)),0)+ 1 from controlSecuencial where estab= @estab and ptoEmi=@ptoEmi and codDoc = @codDoc");
            DB.AsignarParametroCadena("@estab", "001");
            DB.AsignarParametroCadena("@ptoEmi", "001");
            DB.AsignarParametroCadena("@codDoc", "01");
            DbDataReader DR = DB.EjecutarConsulta();

            if (DR.Read())
            {
                ultimoFolio = DR[0].ToString().Trim();
            }
            DB.Desconectar();
            return ultimoFolio;
        }




        public void registroSecuencial(string codDoc, string estab,string ptoemi, string secuencial)
        {
            DB.Conectar();
            DB.CrearComando(@"insert into controlSecuencial
                           (codDoc,estab,ptoEmi,secuencial,procesado,fecha,usuario )
                           values
                           (@codDoc,@estab,@ptoEmi,@secuencial,@procesado,@fecha,@usuario)");
            DB.AsignarParametroCadena("@codDoc","01");
            DB.AsignarParametroCadena("@estab", "001");
            DB.AsignarParametroCadena("@ptoEmi", "001");
            DB.AsignarParametroCadena("@secuencial", secuencial);
            DB.AsignarParametroCadena("@procesado", "true");
            DB.AsignarParametroCadena("@fecha", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            DB.AsignarParametroCadena("@usuario", idUser);
            DB.EjecutarConsulta1();
            DB.Desconectar();

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
                    DB.CrearComando(@"insert into ImpuestosDetallesTemp
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
                    DB.CrearComando(@"insert into ImpuestosDetallesTemp
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
           // arraylCargosxServico = new ArrayList();

            string auxservicio = "";

            int countImp = 0;
            Page.Validate("Deta");
            if (Page.IsValid)
            {
                DB.Conectar();
                DB.CrearComando(@"SELECT count(*) FROM ImpuestosDetallesTemp WHERE id_Empleado=@empleado and codigoTemp=@codigoPrincipal");
                DB.AsignarParametroCadena("@empleado", idUser);

                DB.AsignarParametroCadena("@codigoPrincipal", tbCodigoP.Text);
                DbDataReader DRCont = DB.EjecutarConsulta();
                if (DRCont.Read())
                {
                    countImp = Convert.ToInt32(DRCont[0]);
                }
                DB.Desconectar();

                string auxdescripcion = "";


                if (countImp > 0)
                {
                    DB.Conectar();
                    DB.CrearComando(@"insert into DetallesTemp
                           (codigoPrincipal,codigoAuxiliar,descripcion,cantidad,precioUnitario,
                            descuento,precioTotalSinImpuestos,id_Empleado,servicio)
                           values
                           (@codigoPrincipal,@codigoAuxiliar,@descripcion,@cantidad,@precioUnitario,
                            @descuento,@precioTotalSinImpuestos,@id_Empleado,@servicio)");
                    DB.AsignarParametroCadena("@codigoPrincipal", tbCodigoP.Text);
                    DB.AsignarParametroCadena("@codigoAuxiliar", "");//tbCodigoA.Text);
                    DB.AsignarParametroCadena("@descripcion", tbCodigoA0.Text.Replace(Environment.NewLine, ""));
                   // DB.AsignarParametroCadena("@descripcion", ddlCodigoA0.SelectedValue);
                    //DB.AsignarParametroCadena("@descripcion", tbConcepto.Text.Replace(Environment.NewLine, "").Replace("â€“",""));
                    DB.AsignarParametroCadena("@cantidad", tbCantidad.Text);
                    DB.AsignarParametroCadena("@precioUnitario", tbPU.Text);
                    DB.AsignarParametroCadena("@descuento", tbDescuento.Text);
                    DB.AsignarParametroCadena("@precioTotalSinImpuestos", tbImporteConcepto.Text);
                    DB.AsignarParametroCadena("@id_Empleado", idUser);

                    if (chbServicio.Checked)
                    {
                        auxservicio = String.Format("{0:f}", (Convert.ToDecimal(cc(tbImporteConcepto.Text)) * Convert.ToDecimal(cc("0.10"))));
                        DB.AsignarParametroCadena("@servicio", auxservicio);
                    } else  {DB.AsignarParametroCadena("@servicio", "0.00");  }


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
                    ddlCodigoA0.SelectedValue = "0";
                    tbPU.Text = "0";
                    tbConcepto.Text = "";


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



        public void seguroHotelero(int npersonas,double totseguroh)
        {

                    DB.Conectar();
                    DB.CrearComando(@"insert into DetallesTemp
                           (codigoPrincipal,codigoAuxiliar,descripcion,cantidad,precioUnitario,
                            descuento,precioTotalSinImpuestos,id_Empleado)
                           values
                           (@codigoPrincipal,@codigoAuxiliar,@descripcion,@cantidad,@precioUnitario,
                            @descuento,@precioTotalSinImpuestos,@id_Empleado)");
                    DB.AsignarParametroCadena("@codigoPrincipal", "71708");
                    DB.AsignarParametroCadena("@codigoAuxiliar", "");//tbCodigoA.Text);
                    DB.AsignarParametroCadena("@descripcion", "Seguro Hotelero");
                    DB.AsignarParametroCadena("@cantidad",Convert.ToString(npersonas));
                    DB.AsignarParametroCadena("@precioUnitario",Convert.ToString(totseguroh) );
                    DB.AsignarParametroCadena("@descuento", tbDescuento.Text);
                    DB.AsignarParametroCadena("@precioTotalSinImpuestos", Convert.ToString(totseguroh));
                    DB.AsignarParametroCadena("@id_Empleado", idUser);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();

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
            if (tipo == "emi") { sql = @"SELECT * FROM EMISOR WHERE RFCEMI=@ruc"; }
            if (tipo == "rec") { sql = @"SELECT * FROM RECEPTOR WHERE RFCREC=@ruc"; }

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
                    txt_fono.Text = DRSum[8].ToString();

                }
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
            tbSubT.Text = formatCero;
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
            DB.CrearComando(@"select ISNULL(sum(precioTotalSinImpuestos),0),ISNULL(sum(descuento),0) from DetallesTemp WHERE id_Empleado=@empleado");
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
            DB.CrearComando(@"SELECT ISNULL(sum(ImpuestosDetallesTemp.valor),0),ISNULL(sum(DetallesTemp.precioTotalSinImpuestos),0)
                              FROM  ImpuestosDetallesTemp INNER JOIN
                                    DetallesTemp ON ImpuestosDetallesTemp.codigoTemp = DetallesTemp.codigoPrincipal
                              WHERE   CAST(tarifa AS INT)='12' AND ImpuestosDetallesTemp.tipo='IVA' AND
ImpuestosDetallesTemp.id_Empleado=@empleado  and DetallesTemp.id_Empleado=@empleadod  ");

            DB.AsignarParametroCadena("@empleado", idUser);
            DB.AsignarParametroCadena("@empleadod", idUser);
            DbDataReader DRSum1 = DB.EjecutarConsulta();
            if (DRSum1.Read())
            {
                tbIVA12.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum1[0].ToString()));
                tbSubtotal12.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum1[1].ToString()));
            }
            DB.Desconectar();
            //Impuesto del 0
            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(DetallesTemp.precioTotalSinImpuestos),0)
                              FROM  ImpuestosDetallesTemp INNER JOIN
                                    DetallesTemp ON ImpuestosDetallesTemp.codigoTemp = DetallesTemp.codigoPrincipal
                              WHERE   CAST(tarifa AS INT)='0' AND ImpuestosDetallesTemp.tipo='IVA' AND ImpuestosDetallesTemp.id_Empleado=@empleado and DetallesTemp.id_Empleado=@empleadod  ");

            DB.AsignarParametroCadena("@empleado", idUser);
            DB.AsignarParametroCadena("@empleadod", idUser);
            DbDataReader DRSum2 = DB.EjecutarConsulta();
            if (DRSum2.Read())
            {
                tbSubtotal0.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum2[0].ToString()));
            }
            DB.Desconectar();
            //No sujeto de IVA
            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(DetallesTemp.precioTotalSinImpuestos),0)
                              FROM  ImpuestosDetallesTemp INNER JOIN
                                    DetallesTemp ON ImpuestosDetallesTemp.codigoTemp = DetallesTemp.codigoPrincipal
                              WHERE   tarifa IS NULL AND ImpuestosDetallesTemp.tipo='IVA' AND  ImpuestosDetallesTemp.id_Empleado=@empleado and DetallesTemp.id_Empleado=@empleadod  ");

            DB.AsignarParametroCadena("@empleado", idUser);
            DB.AsignarParametroCadena("@empleadod", idUser);
            DbDataReader DRSum3 = DB.EjecutarConsulta();
            if (DRSum3.Read())
            {
                tbSubtotalNoSujeto.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum3[0].ToString()));
            }
            DB.Desconectar();
            //ICE
            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(ImpuestosDetallesTemp.valor),0)
                              FROM  ImpuestosDetallesTemp INNER JOIN
                                    DetallesTemp ON ImpuestosDetallesTemp.codigoTemp = DetallesTemp.codigoPrincipal
                              WHERE   ImpuestosDetallesTemp.tipo='ICE' AND ImpuestosDetallesTemp.id_Empleado=@empleado");
            DB.AsignarParametroCadena("@empleado", idUser);
            DbDataReader DRSum4 = DB.EjecutarConsulta();
            if (DRSum4.Read())
            {
                tbICE.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum4[0].ToString()));
            }
            DB.Desconectar();

            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(servicio),0)
                              FROM  DetallesTemp
                              WHERE   id_Empleado=@empleado");
            DB.AsignarParametroCadena("@empleado", idUser);
            DbDataReader DRSum5 = DB.EjecutarConsulta();
            if (DRSum5.Read())
            {

                tbPropinas.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum5[0].ToString()));
            }
            DB.Desconectar();

            tbSubT.Text = String.Format("{0:f}", Convert.ToDecimal(tbSubtotal12.Text) + Convert.ToDecimal(tbTotalDescuento.Text));
            tbImporteTotal.Text = String.Format("{0:f}", Convert.ToDecimal(tbTotalSinImpuestos.Text) - Convert.ToDecimal(tbDescuento.Text) +
                                   Convert.ToDecimal(tbICE.Text) + Convert.ToDecimal(tbIVA12.Text) + Convert.ToDecimal(tbTasaHospedaje.Text) + Convert.ToDecimal(tbPropinas.Text));

            tbImporteaPagar.Text = String.Format("{0:f}", Convert.ToDecimal(tbImporteTotal.Text)) ;

            tbMonto.Text = tbImporteaPagar.Text;
        }

        protected void ddlTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoImpuesto.SelectedValue == "3")
            {
                ddlTasaIVA.Visible = false;
                tbTarifa.Visible = true;
                RegularExpressionValidator2.Visible = true;
                RequiredFieldValidator16.Visible = true;
                tbCodigoIDP.Text = "";
                tbTarifa.Text = formatCero;
            }
            else
            {
                ddlTasaIVA.Visible = true;
                tbTarifa.Visible = false;
                RegularExpressionValidator2.Visible = false;
                RequiredFieldValidator16.Visible = false;
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

        public void calculaSerivicio()
        {
            DB.Conectar();
            DB.CrearComando(@"SELECT ISNULL(sum(DetallesTemp.servicio),0)
                              FROM  detalles
                              WHERE   id_Empleado=@empleado");
            DB.AsignarParametroCadena("@empleado", idUser);
            DbDataReader DRSum3 = DB.EjecutarConsulta();
            if (DRSum3.Read())
            {

               tbPropinas.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum3[0].ToString()));
            }
            DB.Desconectar();



        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        private void importeConcepto()
        {
            lMsjImpuestos.Text = "";
            tbImporteConcepto.Text = String.Format("{0:f}", (Convert.ToDecimal(cc(tbPU.Text)) * Convert.ToDecimal(cc(tbCantidad.Text))) - Convert.ToDecimal(cc(tbDescuento.Text)));

         //   sumpropinas += Convert.ToDecimal(cc(tbPU.Text)) * Convert.ToDecimal(cc("0.10"));







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
                    DB.CrearComando(@"SELECT count(*) FROM DetallesTemp WHERE id_Empleado=@empleado");
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



        protected void ddlCodigoA0_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB.Conectar();
            DB.CrearComando(@"SELECT codigo FROM Conceptos_C where item=@item");
            DB.AsignarParametroCadena("@item", ddlCodigoA0.SelectedValue );

            DbDataReader DRitem = DB.EjecutarConsulta();
            if (DRitem.Read())
            {
                tbCodigoP.Text = DRitem[0].ToString();
            }
            DB.Desconectar();
        }

        private void detallesadd(string nombre, string valorconcepto, string codigoP, string user,string personas)
        {

            decimal  cxservicio;
            decimal totcxpservicio;
            //cxservicio = Convert.ToInt32(valorconcepto);
            //totcxpservicio = cxservicio / 10;


              if (nombre == "Tasa Servicio Turistico")
            {
            DB.Conectar();
            DB.CrearComando(@"insert into DetallesAdicionalesTemp
                                       (nombre,valor,id_DetallesTemp,codigoTemp,id_Empleado)
                                       values
                                       (@nombre,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado)");

            DB.AsignarParametroCadena("@nombre", nombre);
            DB.AsignarParametroCadena("@valor", valorconcepto);



            DB.AsignarParametroCadena("@id_DetallesTemp", "");
            DB.AsignarParametroCadena("@codigoTemp", codigoP);
            DB.AsignarParametroCadena("@id_Empleado", idUser);
            DB.EjecutarConsulta1();
            DB.Desconectar();
            }
          }

        public Boolean  consultarTipoItem(string item)
        {
            string tipo;
            DB.Conectar();
            DB.CrearComando("select tipo from Conceptos_C where item=@item");
            DB.AsignarParametroCadena("@item", item);

            DbDataReader DR = DB.EjecutarConsulta();

            if (DR.Read())
            {
                tipo = DR[0].ToString();
                DB.Desconectar();
                if (tipo == "True")
                {
                 return true;

                }

            }
            DB.Desconectar();
            return false;
        }
        public void impuestos(double  valor,string codigoP)
        {
            double impvalor;
            impvalor=(valor) *(0.12);

            DB.Conectar();
            DB.CrearComando(@"insert into ImpuestosDetallesTemp
                           (codigo,codigoPorcentaje,baseImponible,tarifa,valor,id_DetallesTemp,codigoTemp,id_Empleado,tipo)
                           values
                           (@codigo,@codigoPorcentaje,@baseImponible,@tarifa,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado,@tipo)");
            DB.AsignarParametroCadena("@codigo", "2");
            DB.AsignarParametroCadena("@codigoPorcentaje", "2");
            DB.AsignarParametroCadena("@baseImponible", Convert.ToString(valor));
            DB.AsignarParametroCadena("@tarifa", "12.00");
            DB.AsignarParametroCadena("@valor", Convert.ToString(impvalor));
            DB.AsignarParametroCadena("@id_DetallesTemp", "");
            DB.AsignarParametroCadena("@codigoTemp", "71708");
            DB.AsignarParametroCadena("@id_Empleado", idUser);
            DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
            DB.EjecutarConsulta1();
            DB.Desconectar();

        }
        protected void tbSubtotalNoSujeto_TextChanged(object sender, EventArgs e)
        {

        }

        private void  consultaEmp(string idEmpleado)
        {
            DB.Conectar();
            DB.CrearComando("select nombreEmpleado from Empleados where idEmpleado=@idEmpleado");
            DB.AsignarParametroCadena("@idEmpleado", idEmpleado);

            DbDataReader DR = DB.EjecutarConsulta();

            if (DR.Read())
            {
                nombrecajero = DR[0].ToString();
                DB.Desconectar();
            }
            DB.Desconectar();
        }

        public int totalDocus()
        {

            DB.Conectar();
            DB.CrearComando("select count(SECUENCIAL) from General where tipo='E'");
            DbDataReader DR = DB.EjecutarConsulta();

            while (DR.Read())
            {
                numFacturas = DR[0].ToString();
            }
            DB.Desconectar();
            //ULTIMOFOLIO =Convert.ToString(Convert.ToInt32(ULTIMOFOLIO) + 1);
            return Convert.ToInt32(numFacturas);
        }

        public int consultaFolio()
        {
            DB.Conectar();
            DB.CrearComando("select  folioFinal from ParametrosSistema ");
            DbDataReader DRF = DB.EjecutarConsulta();
            if (DRF.Read())
            {
                //folioInicial = Convert.ToInt32(DRF[0].ToString());
                folioFinal = DRF[0].ToString();

            }
            DB.Desconectar();
            return Convert.ToInt32(folioFinal);
        }

        protected void tbServicio10_TextChanged(object sender, EventArgs e)
        {
            llenarTotales();
        }

      }
}