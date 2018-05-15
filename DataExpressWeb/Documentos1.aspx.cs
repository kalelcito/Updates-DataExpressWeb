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
	public partial class Documentos : System.Web.UI.Page
	{
		private String consulta;
		private String aux;
		private String separador = "|";
		private DataTable DT = new DataTable();
		private BasesDatos DB = new BasesDatos();
		private EnviarMail EM;
		String[] seleccionados;
		int cantidad;
		string servidor = "";
		int puerto = 25;
		Boolean ssl = false;
		string emailCredencial = "";
		string passCredencial = "";
		string emailEnviar = "";
		string RutaDOC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}
            //tbRFC.Attributes.Add("onkeyup", "changeToUpperCase(this.id)");
            //tbRFC.Attributes.Add("onchange", "changeToUpperCase(this.id)");
            if (!IsPostBack)
            {
            //    if (Convert.ToBoolean(Session["facturasManuales"])) { HyperLink8.Visible = true; } else { HyperLink8.Visible = false; }
            //    if (Convert.ToBoolean(Session["facturasManuales"])) { HyperLink1.Visible = true; } else { HyperLink1.Visible = false; }
            //    if (Convert.ToBoolean(Session["facturasManuales"])) { HyperLink3.Visible = true; } else { HyperLink3.Visible = false; }
            //    if (Convert.ToBoolean(Session["retencionesManuales"])) { HyperLink4.Visible = true; } else { HyperLink4.Visible = false; }
            //    if (Convert.ToBoolean(Session["reporteIndividual"])) { HyperLink8.Visible = true; } else { HyperLink8.Visible = false; }
            //    if (Convert.ToBoolean(Session["asRoles"])) { hlNA.Visible = true; } else { hlNA.Visible = false; }
                if (Session["sucursalUser"] != null)
                {
                    if (Convert.ToBoolean(Session["coFactPropias"]) || Convert.ToBoolean(Session["coFactTodas"]))
                    {
                        if (Convert.ToBoolean(Session["EnvioEmail"])) { tbEmail.Visible = true; bMail.Visible = true; } else { tbEmail.Visible = false; bMail.Visible = false; }
                        buscar();
                        if (String.IsNullOrEmpty((String)Session["rfcCliente"]))
                        {
                            ddlSucursal.Visible = false;
                            //    lSucursal.Visible = false;
                            //bMail.Visible = false;
                            // lSeleccionDocus.Visible = false;
                            consulta = "";
                        }
                        if (Session["rolUser"].ToString() == "3")
                        {
                            //if (Convert.ToBoolean(Session["coFactTodas"])) { aux = "1"; } else { aux = "0"; }
                            //SqlDataSource1.SelectParameters["QUERY"].DefaultValue = consulta;
                            //SqlDataSource1.SelectParameters["SUCURSAL"].DefaultValue = Session["sucursalUser"].ToString();
                            //SqlDataSource1.SelectParameters["RFC"].DefaultValue = Session["rfcCliente"].ToString();
                            //SqlDataSource1.SelectParameters["ROL"].DefaultValue = aux;
                            //SqlDataSource1.SelectParameters["empleado"].DefaultValue = "";
                            //SqlDataSource1.DataBind();
                            //gvFacturas.DataBind();
                            consulta = "";
                        }
                        string idComprobante = Request.QueryString.Get("comprobante");
                        if (!String.IsNullOrEmpty(idComprobante))
                        {

                        }
                    }
                }
                else
                {
                    if (Request.Path.IndexOf("/cuenta/Login.aspx") == -1)
                    {
                        Response.Redirect("~/cuenta/Login.aspx");
                    }
                }
            }


        }

			//Filtros grid view
			//gvFacturas.DataBind();


            //DataFilter11.DataSource = SqlDataSource1;
            //DataFilter11.DataColumns = gvFacturas.Columns;
            //DataFilter11.FilterSessionID = "Documentos.aspx";
            //DataFilter11.OnFilterAdded += new DataFilter1.RefreshDataGridView(DataFilter1_OnFilterAdded);


		protected void Button1_Click(object sender, EventArgs e)
		{
			buscar();
		}



        private void buscar()
        {
            int fecha = 0;
            string msjbuscar = "";
            consulta = "";
            DT.Clear();
            if (tbFolioAnterior.Value.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "FA" + tbFolioAnterior.Value + separador; }
                else { consulta = "FA" + tbFolioAnterior.Value + separador; }
            }
            if (tbNombre.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "RS" + tbNombre.Text + separador; }
                else { consulta = "RS" + tbNombre.Text + separador; }
            }
            //if (tbControl.Text.Length != 0)
            //{
            //    if (consulta.Length != 0) { consulta = consulta + "CF" + tbControl.Text + separador; }
            //    else { consulta = "CF" + tbControl.Text + separador; }
            //}
            if (tbRFC.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "RF" + tbRFC.Text + separador; }
                else { consulta = "RF" + tbRFC.Text + separador; }
            }

            if (ddlTipoDocumento.SelectedIndex != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "TD" + ddlTipoDocumento.SelectedValue + separador; }
                else { consulta = "TD" + ddlTipoDocumento.SelectedValue + separador; }
            }
            if (ddlEstado.SelectedIndex != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "ED" + ddlEstado.SelectedValue + separador; }
                else { consulta = "ED" + ddlEstado.SelectedValue + separador; }
            }
            if (ddlSucursal.SelectedIndex != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "SU" + ddlSucursal.SelectedValue + separador; }
                else { consulta = "SU" + ddlSucursal.SelectedValue + separador; }
            }
            //if (ddlPtoEmi.SelectedIndex != 0)
            //{
            //    if (consulta.Length != 0) { consulta = consulta + "PO" + ddlPtoEmi.SelectedValue + separador; }
            //    else { consulta = "PO" + ddlPtoEmi.SelectedValue + separador; }
            //}
            if (tbFechaInicial.Text.Length > 5 && tbFechaFinal.Text.Length > 5)
            {
                if (tbFechaInicial.Text.Length > 5)
                {
                    if (consulta.Length != 0) { consulta = consulta + "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + separador; }
                    else { consulta = "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + separador; }
                    fecha = 1;
                }

                if (tbFechaFinal.Text.Length > 5)
                {
                    if (consulta.Length != 0) { consulta = consulta + "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + separador; }
                    else { consulta = "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + separador; }
                    fecha = fecha + 1;
                }
            }
            //if (((String)Session["coFactTodas"])=="") { miSucursal = "S---"; } else { miSucursal = (String)Session["sucursalUser"]; }
            // miSucursal = "S---";
            if (consulta.Length > 0)
            {
                if (fecha > 1)
                {
                    consulta = consulta.Substring(0, consulta.Length - 1);
                }
                if (fecha == 1)
                {
                    msjbuscar = "Es necesario seleccionar ambas fechas.";
                }
            }
            else
            {
                consulta = "-";
            }
            //lMensaje.Text += consulta;
            //lMensaje.Text += Session["coFactTodas"];
            //lMensaje.Text += Session["sucursalUser"];
            //lMensaje.Text += Session["rfcCliente"];
            //lMensaje.Text += Session["USERNAME"].ToString();
            //lMensaje.Text += Session["clavesucursalUser"].ToString();
            string TOPCopm = "''";
            if (Session["TOPComp"] != null) { TOPCopm = Session["TOPComp"].ToString(); }
            if (Convert.ToBoolean(Session["coFactTodas"])) { aux = "1"; } else { aux = "0"; }
            SqlDataSource1.SelectParameters["QUERY"].DefaultValue = consulta;
            SqlDataSource1.SelectParameters["SUCURSAL"].DefaultValue = Session["claveSucursalUser"].ToString();
            SqlDataSource1.SelectParameters["RFC"].DefaultValue = Session["rfcCliente"].ToString();
            SqlDataSource1.SelectParameters["ROL"].DefaultValue = Session["coFactTodas"].ToString();
            SqlDataSource1.SelectParameters["empleado"].DefaultValue = Session["USERNAME"].ToString();
            SqlDataSource1.SelectParameters["ROLSUCURSAL"].DefaultValue = Session["coFactPropias"].ToString();
            SqlDataSource1.SelectParameters["TOP"].DefaultValue = TOPCopm;
            //SqlDataSource1.SelectParameters["nombreSucursalUser"].DefaultValue = Session["nombreSucursalUser"].ToString();
            SqlDataSource1.DataBind();
            gvFacturas.DataBind();
            //lCount.Visible = true;
            //lCount.Text = "Registros encontrados: "+gvFacturas.Rows.Count;

            consulta = "";
        }

		protected void Button1_Click1(object sender, EventArgs e)
		{
			Response.Redirect("Documentos.aspx", false);
		}

        protected void Limpiar(object sender, EventArgs e)
        {
            tbFolioAnterior.Value = "";

        }

		protected void Button1_Click2(object sender, EventArgs e)
		{
			DB.Conectar();
            DB.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio from Par_ParametrosSistema");
			DbDataReader DR1 = DB.EjecutarConsulta();

			while (DR1.Read())
			{
				servidor = DR1[0].ToString();
				puerto = Convert.ToInt32(DR1[1].ToString());
				ssl = Convert.ToBoolean(DR1[2].ToString());
				emailCredencial = DR1[3].ToString();
				passCredencial = DR1[4].ToString();
				RutaDOC = DR1[5].ToString();
				emailEnviar = DR1[6].ToString();
			}
			DB.Desconectar();

			string emails = "";
			string asunto = "";
			string mensaje = "";
			//string temp = "";
			Boolean bCHK = false;
			DB.Desconectar();
			consulta = "";
			cantidad = gvFacturas.Rows.Count;
			foreach (GridViewRow row in gvFacturas.Rows)
			{
				CheckBox chk_Seleccionar = (CheckBox)row.FindControl("check");
				HiddenField hd_SeleccionaPDF = (HiddenField)row.FindControl("checkHdPDF");
				HiddenField hd_SeleccionarXML = (HiddenField)row.FindControl("checkHdXML");
				Label l_Label2 = (Label)row.FindControl("Label2");
				// Label l_Label4 = (Label)row.FindControl("Label4");
				Label l_Label5 = (Label)row.FindControl("Label5");
				Label l_Label1 = (Label)row.FindControl("Label1");
				EM = new EnviarMail();
				EM.servidorSTMP(servidor, puerto, ssl, emailCredencial, passCredencial);
				if (chk_Seleccionar.Checked)
				{
					bCHK = true;
					emails = "";
					if (chkReglas.Checked)
					{
						if (!l_Label2.Text.Trim().Equals("9999999999999"))
						{
							DB.Conectar();
							DB.CrearComando("select emailsRegla from Cat_EmailsReglas  where Receptor=@rfcrec and estadoRegla=1");
							DB.AsignarParametroCadena("@rfcrec", l_Label2.Text.Trim());
							DbDataReader DR3 = DB.EjecutarConsulta();
							if (DR3.Read())
							{
								emails = DR3[0].ToString();
							}
						}
					}
					emails = tbEmail.Text + "," + emails;
					emails = emails.Trim();
					emails = emails.Trim(',');

					if (checkPDF.Checked)
					{
						EM.adjuntar(RutaDOC + hd_SeleccionaPDF.Value.ToString().Replace("docus/", ""));
					}
					if (checkXML.Checked)
					{
						EM.adjuntar(RutaDOC + hd_SeleccionarXML.Value.ToString().Replace("docus/", ""));
					}
					if (emails.Length > 15)
					{
                        asunto = "Documento electrónico No: " + l_Label5.Text + " de DeCameron Ecuador";
                        mensaje = @"Estimado(a) cliente;  <br>
							Acaba de recibir su documento electrónico generado el " + l_Label1.Text + @"<br>
							con folio No: " + l_Label5.Text + ".";
                        mensaje += "<br><br>Saludos cordiales, ";
                        mensaje += "<br>Hotel DeCameron Ecuador, ";
                        mensaje += "<br><br>Servicio proporcionado por DataExpress Latinoamerica";

						EM.llenarEmail(emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
						try
						{
							EM.enviarEmail();
							lbMsgZip.Text = "E-Mail enviado";

						}
						catch (System.Net.Mail.SmtpException ex)
						{
							DB.Desconectar();
							DB.Conectar();
							DB.CrearComando(@"insert into Log_ErrorFacturas
								(detalle,fecha,archivo,linea,numeroDocumento,tipo)
								values
								(@detalle,@fecha,@archivo,@linea,@numeroDocumento,@tipo)");
							DB.AsignarParametroCadena("@detalle", ex.Message.Replace("'", "''"));
							DB.AsignarParametroCadena("@fecha", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
							DB.AsignarParametroCadena("@archivo", "");
							DB.AsignarParametroCadena("@linea", "");
							DB.AsignarParametroCadena("@tipo", "E-Mail");
							DB.AsignarParametroCadena("@numeroDocumento", l_Label5.Text + " ");
							DB.EjecutarConsulta1();
							DB.Desconectar();
							lbMsgZip.Text = ex.Message;
						}
					}
					else
					{
						lbMsgZip.Text = "Tienes seleccionar algún E-Mail";
					}
				}
			}
			if (!bCHK)
			{
				lbMsgZip.Text = "Debes seleccionar una factura";
			}
		}

		protected void btnZip_Click(object sender, EventArgs e)
		{
			lbMsgZip.Text = "";

			cantidad = gvFacturas.Rows.Count;
			seleccionados = new String[cantidad];
			//String mensaje = "", val_rb = "";
            Boolean bCHK = false;//, bRB = false, bSelect = false;
			//int a = 0;
			Response.Clear();
			ZipFile zip = new ZipFile();
			using (zip);


			foreach (GridViewRow row in gvFacturas.Rows)
			{
				CheckBox chk_Seleccionar = (CheckBox)row.FindControl("check");
				HiddenField hd_SeleccionaPDF = (HiddenField)row.FindControl("checkHdPDF");
				HiddenField hd_SeleccionarXML = (HiddenField)row.FindControl("checkHdXML");

				if (chk_Seleccionar.Checked)
				{
					bCHK = true;
					if (checkPDF.Checked)
					{
						zip.AddItem(System.AppDomain.CurrentDomain.BaseDirectory + hd_SeleccionaPDF.Value, "Archivos");
					}
					if (checkXML.Checked)
					{
						zip.AddItem(System.AppDomain.CurrentDomain.BaseDirectory + hd_SeleccionarXML.Value, "Archivos");
					}
				}
			}
			if (bCHK)
			{
				zip.Save(Response.OutputStream);
				Response.AddHeader("Content-Disposition", "attachment; filename=Facturas.zip");
				Response.ContentType = "application/octet-stream";
				Response.End();
			}
			else
			{
				lbMsgZip.Text = "Debes seleccionar una factura";
			}
		}

		protected override void OnLoadComplete(EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				//DataFilter11.BeginFilter();
				//DataFilter11.AddNewFilter("SECUENCIA", "LIKE", "060%");
			}
		}

        //void DataFilter1_OnFilterAdded()
        //{
        //    try
        //    {
        //        DataFilter11.FilterSessionID = "Documentos.aspx";
        //        DataFilter11.FilterDataSource();
        //        gvFacturas.DataBind();

        //    }
        //    catch (Exception e)
        //    {
        //    }
        //}

        //protected void gvFacturas_PageIndexChanged(object sender, EventArgs e)
        //{
        //    DataFilter1_OnFilterAdded();
        //}

        //protected void gvFacturas_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataFilter1_OnFilterAdded();
        //}

        protected void check_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            lCount.Visible = true;
            lCount.Text = "Se encontraron " + e.AffectedRows + " registros ";
        }

        protected void bBuscar_Click(object sender, EventArgs e)
        {
            buscar();
        }

        protected void Button1_Click3(object sender, EventArgs e)
        {

        }

        protected void Form_Click(object sender, EventArgs e)
        {

        }




	}
}