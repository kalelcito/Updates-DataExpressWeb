// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 12-10-2016
// ***********************************************************************
// <copyright file="Emisores.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Datos;
using System.Linq;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Emisores.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Emisores : Page
    {
        /// <summary>
        /// The _id editar
        /// </summary>
        private static string _idEditar;
        /// <summary>
        /// The _file bytes
        /// </summary>
        private static byte[] _fileBytes;


        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            if (Session["idUser"] != null)
            {
                _idUser = Session["idUser"].ToString();
                SqlDataEmisores.ConnectionString = _db.CadenaConexion;
                SqlDataSource2.ConnectionString = _db.CadenaConexion;
            }
            if (!Page.IsPostBack)
            {
                Buscar();
                RequiredFieldValidator10.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                RequiredFieldValidator17.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                RequiredFieldValidator18.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                RequiredFieldValidator19.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
            }
        }

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
            var sql = @"SELECT IDEEMI,RFCEMI,NOMEMI,curp,regimenFiscal,pais FROM Cat_Emisor";
            var where = "";
            var whereTemp = "";
            if (!string.IsNullOrEmpty(tbRfcBus.Text))
            {
                whereTemp = "RFCEMI LIKE '%" + tbRfcBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (!string.IsNullOrEmpty(tbNomBus.Text))
            {
                whereTemp = "NOMEMI LIKE '%" + tbNomBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (!string.IsNullOrEmpty(tbDireccBus.Text))
            {
                whereTemp = "dirMatriz LIKE '%" + tbDireccBus.Text + "%' OR colonia LIKE '%" + tbDireccBus.Text + "%' OR localidad LIKE '%" + tbDireccBus.Text + "%' OR referencia LIKE '%" + tbDireccBus.Text + "%' OR municipio LIKE '%" + tbDireccBus.Text + "%' OR estado LIKE '%" + tbDireccBus.Text + "%' OR pais LIKE '%" + tbDireccBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (!string.IsNullOrEmpty(tbMailBus.Text))
            {
                whereTemp = "correo LIKE '%" + tbMailBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            sql += !string.IsNullOrEmpty(where) ? " WHERE (" + where + ")" : "";
            SqlDataEmisores.SelectCommand = sql;
            SqlDataEmisores.DataBind();
            gvEmisor.DataBind();
            lCount.Visible = true;
            lCount.Text = "<strong>Registros encontrados: " + gvEmisor.Rows.Count + "</strong>";
        }

        /// <summary>
        /// Handles the Click event of the bDetalles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bDetalles_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            Llenarlista(id);
            chkEditar.Visible = true;
            chkEditar.Checked = false;
            chkEditar_CheckedChanged(null, null);
            bAgregar.Text = "Modificar";
            //trGiro.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divNuevo').modal('toggle');", true);
        }

        /// <summary>
        /// Handles the Click event of the bLimpiarBus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bLimpiarBus_Click(object sender, EventArgs e)
        {
            tbRfcBus.Text = "";
            tbNomBus.Text = "";
            tbDireccBus.Text = "";
            tbMailBus.Text = "";
            Buscar();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            tbNomEmi.ReadOnly = !chkEditar.Checked;
            tbCURPE.ReadOnly = !chkEditar.Checked;
            tbTelEmi.ReadOnly = !chkEditar.Checked;
            tbMailEmi.ReadOnly = !chkEditar.Checked;
            tbTelEmi.ReadOnly = !chkEditar.Checked;
            tbCalleEmi.ReadOnly = !chkEditar.Checked;
            tbNoExtEmi.ReadOnly = !chkEditar.Checked;
            tbNoIntEmi.ReadOnly = !chkEditar.Checked;
            tbColoniaEmi.ReadOnly = !chkEditar.Checked;
            tbLocEmi.ReadOnly = !chkEditar.Checked;
            tbRefEmi.ReadOnly = !chkEditar.Checked;
            tbMunicipioEmi.ReadOnly = !chkEditar.Checked;
            tbEstadoEmi.ReadOnly = !chkEditar.Checked;
            tbPaisEmi.ReadOnly = !chkEditar.Checked;
            tbCpEmi.ReadOnly = !chkEditar.Checked;
            tbRegimenFiscal.ReadOnly = !chkEditar.Checked;
            //ddlTEmp.Enabled = chkEditar.Checked;
            //ddlObligado.Enabled = chkEditar.Checked;
            fileLogo.Visible = chkEditar.Checked;
            var giro = Session["IDGIRO"].ToString();
            if (giro.Equals("1"))
            {
                trColorPdf01.Visible = chkEditar.Checked;
                trColorPdf04.Visible = chkEditar.Checked;
                trColorPdf06.Visible = false;
                trColorPdf07.Visible = false;
                trColorPdf08.Visible = false;
                trColorPdf09.Visible = false;
            }
            else if (giro.Equals("2"))
            {
                trColorPdf01.Visible = chkEditar.Checked;
                trColorPdf04.Visible = chkEditar.Checked;
                trColorPdf06.Visible = false;
                trColorPdf07.Visible = false;
                trColorPdf08.Visible = false;
                trColorPdf09.Visible = false;
            }
            else
            {
                trColorPdf01.Visible = chkEditar.Checked;
                trColorPdf04.Visible = chkEditar.Checked;
                trColorPdf06.Visible = chkEditar.Checked;
                trColorPdf07.Visible = chkEditar.Checked;
                trColorPdf08.Visible = chkEditar.Checked;
                trColorPdf09.Visible = chkEditar.Checked;
            }
            bAgregar.Visible = chkEditar.Checked;
            tbRfcEmi.ReadOnly = !string.IsNullOrEmpty(_idEditar);
        }

        /// <summary>
        /// Handles the Click event of the bNuevo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bNuevo_Click(object sender, EventArgs e)
        {
            _idEditar = null;
            Llenarlista("");
            chkEditar.Visible = false;
            chkEditar.Checked = true;
            chkEditar_CheckedChanged(null, null);
            bAgregar.Text = "Agregar";
            //trGiro.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('toggle');", true);
        }

        /// <summary>
        /// Llenarlistas the specified ideemi.
        /// </summary>
        /// <param name="ideemi">The ideemi.</param>
        private void Llenarlista(string ideemi)
        {
            var sql = @"SELECT [RFCEMI]
                              ,[NOMEMI]
                              ,[curp]
                              ,[telefono]
                              ,[email]
                              ,[regimenFiscal]
                              ,[dirMatriz]
                              ,[EmpresaTipo]
                              ,[noExterior]
                              ,[noInterior]
                              ,[colonia]
                              ,[localidad]
                              ,[referencia]
                              ,[municipio]
                              ,[estado]
                              ,[pais]
                              ,[codigoPostal]
                              ,[obligadoContabilidad]
                              ,[logo]
                              ,'rgb(' + (CASE WHEN ISNULL([pdfColor01], '') = '' THEN '0,0,0' ELSE [pdfColor01] END) + ')' AS [pdfColor01]
                              ,'rgb(' + (CASE WHEN ISNULL([pdfColor04], '') = '' THEN '0,0,0' ELSE [pdfColor04] END) + ')' AS [pdfColor04]
                              ,'rgb(' + (CASE WHEN ISNULL([pdfColor06], '') = '' THEN '0,0,0' ELSE [pdfColor06] END) + ')' AS [pdfColor06]
                              ,'rgb(' + (CASE WHEN ISNULL([pdfColor07], '') = '' THEN '0,0,0' ELSE [pdfColor07] END) + ')' AS [pdfColor07]
                              ,'rgb(' + (CASE WHEN ISNULL([pdfColor08], '') = '' THEN '0,0,0' ELSE [pdfColor08] END) + ')' AS [pdfColor08]
                              ,'rgb(' + (CASE WHEN ISNULL([pdfColor09], '') = '' THEN '0,0,0' ELSE [pdfColor09] END) + ')' AS [pdfColor09]
                          FROM [Cat_Emisor]
                          WHERE IDEEMI=@id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", ideemi);
            var dr = _db.EjecutarConsulta();
            var emisores = new List<object[]>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var emisor = new object[dr.FieldCount];
                    dr.GetValues(emisor);
                    emisores.Add(emisor);
                }
                _db.Desconectar();
                foreach (var emisor in emisores)
                {
                    tbRfcEmi.Text = emisor[0].ToString();
                    tbNomEmi.Text = emisor[1].ToString();
                    tbCURPE.Text = emisor[2].ToString();
                    tbTelEmi.Text = emisor[3].ToString();
                    tbMailEmi.Text = emisor[4].ToString();
                    tbRegimenFiscal.Text = emisor[5].ToString();
                    tbCalleEmi.Text = emisor[6].ToString();
                    tbNoExtEmi.Text = emisor[8].ToString();
                    tbNoIntEmi.Text = emisor[9].ToString();
                    tbColoniaEmi.Text = emisor[10].ToString();
                    tbLocEmi.Text = emisor[11].ToString();
                    tbRefEmi.Text = emisor[12].ToString();
                    tbMunicipioEmi.Text = emisor[13].ToString();
                    tbEstadoEmi.Text = emisor[14].ToString();
                    tbPaisEmi.Text = emisor[15].ToString();
                    tbCpEmi.Text = emisor[16].ToString();
                    tbColorPdf01.Text = emisor[19].ToString();
                    tbColorPdf04.Text = emisor[20].ToString();
                    tbColorPdf06.Text = emisor[21].ToString();
                    tbColorPdf07.Text = emisor[22].ToString();
                    tbColorPdf08.Text = emisor[23].ToString();
                    tbColorPdf09.Text = emisor[24].ToString();
                    //ddlTEmp.SelectedValue = emisor[7].ToString();
                    //ddlObligado.SelectedValue = emisor[17].ToString();
                    var bytesDb = emisor[18];
                    var bytes = bytesDb is DBNull ? new byte[0] : (byte[])emisor[18];
                    var base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    Image1.ImageUrl = "data:image/png;base64," + base64String;
                }
            }
            else
            {
                tbRfcEmi.Text = "";
                tbNomEmi.Text = "";
                tbCURPE.Text = "";
                tbMailEmi.Text = "";
                tbTelEmi.Text = "";
                tbRegimenFiscal.Text = "";
                tbCalleEmi.Text = "";
                tbNoExtEmi.Text = "";
                tbNoIntEmi.Text = "";
                tbColoniaEmi.Text = "";
                tbLocEmi.Text = "";
                tbRefEmi.Text = "";
                tbMunicipioEmi.Text = "";
                tbEstadoEmi.Text = "";
                tbPaisEmi.Text = "";
                tbCpEmi.Text = "";
                //ddlTEmp.Text = "";
                //ddlObligado.Text = "";
                tbColorPdf01.Text = "rgb(0,0,0)";
                tbColorPdf04.Text = "rgb(0,0,0)";
                tbColorPdf06.Text = "rgb(0,0,0)";
                tbColorPdf07.Text = "rgb(0,0,0)";
                tbColorPdf08.Text = "rgb(0,0,0)";
                tbColorPdf09.Text = "rgb(0,0,0)";
                Image1.ImageUrl = "data:image/png;base64,";
            }
        }

        /// <summary>
        /// Handles the Click event of the bBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
            ScriptManager.RegisterStartupScript(this, GetType(), "_bBuscar_Click_CerrarFiltros", "$('#divBuscar').modal('hide');", true);
        }

        private bool ValidarCodigoPostal(string clave)
        {
            try
            {
                if (!Session["CfdiVersion"].ToString().Equals("3.3"))
                {
                    return true;
                }
                var catalogos = (CatCdfi)Session["CatalogosCfdi33"];
                var existe = catalogos.CCodigopostal.Any(c => c.Key.Equals(clave, StringComparison.OrdinalIgnoreCase));
                return existe;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        /// <summary>
        /// Handles the Click event of the bAgregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregar_Click(object sender, EventArgs e)
        {
            if (Session["CfdiVersion"].ToString().Equals("3.3") && !ValidarCodigoPostal(tbCpEmi.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, $"El codigo postal { tbCpEmi.Text } no se encuentra en el catálogo del SAT", 4, null);
            }
            var sql = "";
            if (string.IsNullOrEmpty(_idEditar))
            {
                sql = @"INSERT INTO Cat_Emisor
                            (RFCEMI, NOMEMI, dirMatriz, EmpresaTipo, noExterior, noInterior, colonia, localidad, referencia, municipio, estado, pais, codigoPostal, curp, telefono, email, regimenFiscal, pdfColor01, pdfColor04, pdfColor06, pdfColor07, pdfColor08, pdfColor09" + (_fileBytes != null ? ", logo" : "") + @")
                            VALUES
                           (@RFCEMI,@NOMEMI,@CALLE,@EmpresaTipo,@NOEXT,@NOINT,@COL,@LOC,@REF,
                            @MUN,@EST,@PAIS,@CP,@CURP,@TEL,@EMAIL,@REGIMEN, @pdfColor01, @pdfColor04, @pdfColor06, @pdfColor07, @pdfColor08, @pdfColor09" + (_fileBytes != null ? ",@LOGO" : "") + @")";
            }
            else
            {
                sql = @"UPDATE Cat_Emisor SET RFCEMI = @RFCEMI, NOMEMI = @NOMEMI, curp=@CURP, telefono=@TEL, email=@EMAIL, regimenFiscal=@REGIMEN, dirMatriz=@CALLE, EmpresaTipo=@EmpresaTipo, noExterior=@NOEXT, noInterior=@NOINT, colonia=@COL, localidad=@LOC, referencia=@REF, municipio=@MUN, estado=@EST, pais=@PAIS, codigoPostal=@CP, pdfColor01=@pdfColor01, pdfColor04=@pdfColor04, pdfColor06=@pdfColor06, pdfColor07=@pdfColor07, pdfColor08=@pdfColor08, pdfColor09=@pdfColor09" + (_fileBytes != null ? ", logo = @LOGO" : "") + @" WHERE IDEEMI=@IDEEMI";
            }
            try
            {
                _db.Conectar();
                _db.CrearComando(sql);
                if (!string.IsNullOrEmpty(_idEditar))
                {
                    _db.AsignarParametroCadena("@IDEEMI", _idEditar);
                }
                _db.AsignarParametroCadena("@RFCEMI", tbRfcEmi.Text);
                _db.AsignarParametroCadena("@NOMEMI", tbNomEmi.Text);
                _db.AsignarParametroCadena("@CALLE", tbCalleEmi.Text);
                _db.AsignarParametroCadena("@NOEXT", tbNoExtEmi.Text);
                _db.AsignarParametroCadena("@NOINT", tbNoIntEmi.Text);
                _db.AsignarParametroCadena("@COL", tbColoniaEmi.Text);
                _db.AsignarParametroCadena("@LOC", tbLocEmi.Text);
                _db.AsignarParametroCadena("@REF", tbRfcEmi.Text);
                _db.AsignarParametroCadena("@MUN", tbMunicipioEmi.Text);
                _db.AsignarParametroCadena("@EST", tbEstadoEmi.Text);
                _db.AsignarParametroCadena("@PAIS", tbPaisEmi.Text);
                _db.AsignarParametroCadena("@CP", tbCpEmi.Text);
                _db.AsignarParametroCadena("@EmpresaTipo", Session["IDGIRO"].ToString());
                _db.AsignarParametroCadena("@CURP", tbCURPE.Text);
                _db.AsignarParametroCadena("@TEL", tbTelEmi.Text);
                _db.AsignarParametroCadena("@EMAIL", tbMailEmi.Text);
                _db.AsignarParametroCadena("@REGIMEN", tbRegimenFiscal.Text);
                _db.AsignarParametroCadena("@pdfColor01", tbColorPdf01.Text.Replace("rgb(", "").Replace(")", "").Replace(" ", ""));
                _db.AsignarParametroCadena("@pdfColor04", tbColorPdf04.Text.Replace("rgb(", "").Replace(")", "").Replace(" ", ""));
                _db.AsignarParametroCadena("@pdfColor06", tbColorPdf06.Text.Replace("rgb(", "").Replace(")", "").Replace(" ", ""));
                _db.AsignarParametroCadena("@pdfColor07", tbColorPdf07.Text.Replace("rgb(", "").Replace(")", "").Replace(" ", ""));
                _db.AsignarParametroCadena("@pdfColor08", tbColorPdf08.Text.Replace("rgb(", "").Replace(")", "").Replace(" ", ""));
                _db.AsignarParametroCadena("@pdfColor09", tbColorPdf09.Text.Replace("rgb(", "").Replace(")", "").Replace(" ", ""));
                //_db.AsignarParametroCadena("@OBLIG", ddlObligado.SelectedItem.Text);
                if (_fileBytes != null)
                {
                    _db.AsignarParametroByteArray("@LOGO", _fileBytes);
                }
                _db.EjecutarConsulta1();
                tbRfcEmi.Text = "";
                tbNomEmi.Text = "";
                tbCURPE.Text = "";
                tbMailEmi.Text = "";
                tbTelEmi.Text = "";
                tbRegimenFiscal.Text = "";
                tbCalleEmi.Text = "";
                tbNoExtEmi.Text = "";
                tbNoIntEmi.Text = "";
                tbColoniaEmi.Text = "";
                tbLocEmi.Text = "";
                tbRefEmi.Text = "";
                tbMunicipioEmi.Text = "";
                tbEstadoEmi.Text = "";
                tbPaisEmi.Text = "";
                tbCpEmi.Text = "";
                //ddlTEmp.Text = "";
                //ddlObligado.Text = "";
                tbColorPdf01.Text = "";
                tbColorPdf04.Text = "";
                tbColorPdf06.Text = "";
                tbColorPdf07.Text = "";
                tbColorPdf08.Text = "";
                tbColorPdf09.Text = "";
                Image1.ImageUrl = "data:image/jpg;base64,";
                bLimpiarBus_Click(null, null);
                var updatePanelMenuMaster = ((Master as SiteMaster).FindControl("UpdatePanelMenuMaster") as UpdatePanel);
                updatePanelMenuMaster.Update();
                (Master as SiteMaster).MostrarAlerta(this, "El emisor se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');");
                _idEditar = "";
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El emisor no se pudo agregar/modificar. Intentelo nuevamente.<br>" + ex.Message, 4, null);
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the UploadedComplete event of the fileLogo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AsyncFileUploadEventArgs" /> instance containing the event data.</param>
        protected void fileLogo_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            _fileBytes = fileLogo.FileBytes;
            //_fileBytes = ControlUtilities.CompressImage(_fileBytes, 70);
            var base64String = Convert.ToBase64String(_fileBytes, 0, _fileBytes.Length);
            Image1.ImageUrl = "data:image/jpg;base64," + base64String;
        }
    }
}