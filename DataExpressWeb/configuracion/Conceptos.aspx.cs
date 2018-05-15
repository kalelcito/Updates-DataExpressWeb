// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="Conceptos.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataExpressWeb;
using Datos;
using System.Linq;
using System.Data.Common;
using Control;
using System.IO;
using OfficeOpenXml;

namespace Configuracion
{
    /// <summary>
    /// Class Conceptos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Conceptos : Page
    {
        /// <summary>
        /// The _id editar
        /// </summary>
        private static string _idEditar;
        /// <summary>
        /// The _hotel
        /// </summary>
        private static bool _hotel;

        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";
        private Log _log;
        public const bool showCatalog33 = false;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            _log = new Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            if (Session["idUser"] != null)
            {
                _idUser = Session["idUser"].ToString();
                SqlDataSource3.ConnectionString = _db.CadenaConexion;
                SqlDataCategorias.ConnectionString = _db.CadenaConexion;
                SqlDataSourceUnidades.ConnectionString = _db.CadenaConexion;
            }
            if (!IsPostBack)
            {
                _hotel = Session["IDGIRO"].ToString().Equals("1");
                BindConceptos();
                BindCategorias();
                RequiredFieldValidator_tbCveProdServ.Enabled = Session["CfdiVersion"].ToString().Equals("3.3");
                ddlCveProdServ.Visible = Session["CfdiVersion"].ToString().Equals("3.3") && showCatalog33;
                ddlClaveSat.Visible = Session["CfdiVersion"].ToString().Equals("3.3") && showCatalog33;
                tbCveProdServ.Visible = Session["CfdiVersion"].ToString().Equals("3.3") && !showCatalog33;
                tbClaveSat.Visible = Session["CfdiVersion"].ToString().Equals("3.3") && !showCatalog33;
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
            var id = btn.CommandArgument;
            _idEditar = id;
            FillCatalog();
            Llenarlista(id);
            chkEditar.Visible = true;
            chkEditar.Checked = false;
            chkEditar_CheckedChanged(null, null);
            bAgregar.Text = "Modificar";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divNuevo').modal('toggle');", true);
        }

        /// <summary>
        /// Handles the Click event of the bAgregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregar_Click(object sender, EventArgs e)
        {
            var sql = "";
            DbDataReader dr = null;
            if (Session["CfdiVersion"].ToString().Equals("3.3") && !showCatalog33)
            {
                if (string.IsNullOrEmpty(tbCveProdServ.Text) || !ValidarClaveProdServ(tbCveProdServ.Text))
                {
                    (Master as SiteMaster).MostrarAlerta(this, "La clave de producto/servicio especificada no existe en los catálogos del SAT", 4, null);
                    BindConceptos();
                    return;
                }
            }
            if (string.IsNullOrEmpty(_idEditar))
            {
                _db.Conectar();
                _db.CrearComando("SELECT idConcepto FROM Cat_CatConceptos_C WHERE codigo = @codigo");
                _db.AsignarParametroCadena("@codigo", tbCodigo.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    _db.Desconectar();
                    (Master as SiteMaster).MostrarAlerta(this, "El concepto con el código " + tbCodigo.Text + " ya esta registrado.", 4, null);
                    return;
                }
                _db.Desconectar();
                sql = @"INSERT INTO Cat_CatConceptos_C
                            (idCategoria, codigo, descripcion, valorUnitario, unidadMedida, claveProdServ)
                            VALUES
                           (@idCategoria, @codigo, @descripcion, @valorUnitario, @unidadMedida, @claveProdServ)";
            }
            else
            {
                _db.Conectar();
                _db.CrearComando("SELECT idConcepto FROM Cat_CatConceptos_C WHERE idConcepto <> @id AND codigo = @codigo");
                _db.AsignarParametroCadena("@id", _idEditar);
                _db.AsignarParametroCadena("@codigo", tbCodigo.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    _db.Desconectar();
                    (Master as SiteMaster).MostrarAlerta(this, "El concepto con el código " + tbCodigo.Text + " ya esta registrado.", 4, null);
                    return;
                }
                _db.Desconectar();
                sql = @"UPDATE Cat_CatConceptos_C SET idCategoria = @idCategoria, codigo = @codigo, descripcion = @descripcion, valorUnitario = @valorUnitario, unidadMedida = @unidadMedida, claveProdServ = @claveProdServ WHERE idConcepto=@idConcepto";
            }
            try
            {
                _db.Conectar();
                _db.CrearComando(sql);
                if (!string.IsNullOrEmpty(_idEditar))
                {
                    _db.AsignarParametroCadena("@idConcepto", _idEditar);
                }
                _db.AsignarParametroCadena("@idCategoria", ddlCategorias.SelectedValue);
                _db.AsignarParametroCadena("@codigo", tbCodigo.Text);
                _db.AsignarParametroCadena("@descripcion", tbDescripcion.Text);
                _db.AsignarParametroCadena("@valorUnitario", CerosNull(tbVU.Text));
                _db.AsignarParametroCadena("@unidadMedida", tbUMedida.Text);
                _db.AsignarParametroCadena("@claveProdServ", tbCveProdServ.Text);
                _db.EjecutarConsulta1();
                tbCodigo.Text = "";
                tbDescripcion.Text = "";
                tbVU.Text = "";
                tbUMedida.Text = "";
                ddlCveProdServ.SelectedValue = "";
                ddlCveProdServ_SelectedIndexChanged(null, null);
                (Master as SiteMaster).MostrarAlerta(this, "El concepto se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');");
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El concepto no se pudo agregar/modificar. Intentelo nuevamente.<br>" + ex.Message, 4, null);
            }
            _db.Desconectar();
            BindConceptos();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            tbCodigo.ReadOnly = !chkEditar.Checked;
            tbDescripcion.ReadOnly = !chkEditar.Checked;
            tbVU.ReadOnly = !chkEditar.Checked;
            tbUMedida.ReadOnly = !chkEditar.Checked || !Session["CfdiVersion"].ToString().Equals("3.3");
            tbCveProdServ.ReadOnly = !chkEditar.Checked;
            bAgregar.Visible = chkEditar.Checked;
            ddlCveProdServ.Enabled = chkEditar.Checked;
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
            FillCatalog();
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('toggle');", true);
        }

        private void FillCatalog()
        {
            if (Session["CfdiVersion"].ToString().Equals("3.3") && showCatalog33)
            {
                var catalogos = (CatCdfi)Session["CatalogosCfdi33"];
                ddlCveProdServ.Visible = true;
                ddlClaveSat.Visible = true;
                ddlCveProdServ.Items.Add(new ListItem("Seleccione", ""));
                ddlClaveSat.Items.Add(new ListItem("Seleccione", ""));
                foreach (var item in catalogos.CClaveprodserv)
                {
                    var listItem = new ListItem(item.Key, item.Key);
                    listItem.Attributes.Add("data-subtext", item.Value);
                    listItem.Attributes.Add("data-tokens", item.Value + " " + item.Key);
                    ddlCveProdServ.Items.Add(listItem);
                }
                foreach (var item in catalogos.CClaveunidad)
                {
                    var listItem = new ListItem(item.Key, item.Key);
                    listItem.Attributes.Add("data-subtext", item.Value);
                    listItem.Attributes.Add("data-tokens", item.Value + " " + item.Key);
                    ddlClaveSat.Items.Add(listItem);
                }
            }
        }

        /// <summary>
        /// Llenarlistas the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void Llenarlista(string id)
        {
            var sql = @"SELECT [codigo]
                              ,[descripcion]
                              ,[valorUnitario]
                              ,[unidadMedida]
                              ,[claveProdServ]
                          FROM [Cat_CatConceptos_C]
                          WHERE idConcepto=@id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", id);
            var dr = _db.EjecutarConsulta();
            var conceptos = new List<object[]>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var concepto = new object[dr.FieldCount];
                    dr.GetValues(concepto);
                    conceptos.Add(concepto);
                }
                _db.Desconectar();
                foreach (var concepto in conceptos)
                {
                    tbCodigo.Text = concepto[0].ToString();
                    tbDescripcion.Text = concepto[1].ToString();
                    tbVU.Text = concepto[2].ToString();
                    tbUMedida.Text = concepto[3].ToString();
                    tbCveProdServ.Text = concepto[4].ToString();
                    try
                    {
                        ddlCveProdServ.SelectedValue = concepto[4].ToString();
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
            }
            else
            {
                tbCodigo.Text = "";
                tbDescripcion.Text = "";
                tbVU.Text = "";
                tbUMedida.Text = "";
                tbCveProdServ.Text = "";
                try
                {
                    ddlCveProdServ.SelectedValue = "";
                }
                catch (Exception ex)
                {
                    // ignored
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
            decimal b;
            var cifra = (!string.IsNullOrEmpty(a) ? a : "").Replace(",", "").Trim();
            var result = string.Format("{0:0.00}", Convert.ToDecimal(string.IsNullOrEmpty(cifra) || !decimal.TryParse(cifra, out b) || b < 0 ? "0.00" : cifra));
            return result;
        }

        /// <summary>
        /// Handles the Click event of the bDelCat control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bDelCat_Click(object sender, EventArgs e)
        {
            var sql = @"DELETE FROM Cat_Catalogo1_C WHERE idCatalogo1_C = @id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", ddlCategorias.SelectedValue);
            var dr = _db.EjecutarConsulta();
            if (dr.RecordsAffected > 0)
            {
                BindCategorias();
                BindConceptos();
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "La categoría no se pudo eliminar. Intentelo nuevamente.", 4, null);
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Binds the categorias.
        /// </summary>
        private void BindCategorias()
        {
            SqlDataCategorias.DataBind();
            ddlCategorias.DataBind();
            var empty = new ListItem("Seleccione Categoría", "");
            empty.Selected = true;
            ddlCategorias.Items.Add(empty);
            //if (!_hotel)
            //{
            ddlCategorias.Items.Add(new ListItem("Nueva Categoría", "0"));
            //}
            //if (_hotel)
            //{
            //    var toRemove = from i in ddlCategorias.Items.Cast<ListItem>() where !i.Value.Equals("") && i.Text.IndexOf("HOTEL", StringComparison.OrdinalIgnoreCase) < 0 select i;
            //    var lista = toRemove.ToList();
            //    foreach (var item in lista)
            //    {
            //        ddlCategorias.Items.Remove(item);
            //    }
            //    bAgregarCat.Visible = false;
            //}
        }

        /// <summary>
        /// Binds the conceptos.
        /// </summary>
        protected void BindConceptos()
        {
            SqlDataSource3.SelectCommand = "SELECT idConcepto, codigo, descripcion, valorUnitario, claveProdServ FROM Cat_CatConceptos_C WHERE idCategoria = '" + ddlCategorias.SelectedValue + "'";
            SqlDataSource3.DataBind();
            GridView1.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlCategorias control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            bDelCat.Visible = !string.IsNullOrEmpty(ddlCategorias.SelectedValue) && !ddlCategorias.SelectedValue.Equals("0");
            bNuevo.Visible = !string.IsNullOrEmpty(ddlCategorias.SelectedValue) && !ddlCategorias.SelectedValue.Equals("0");
            //if (_hotel)
            //{
            //    bDelCat.Visible = false;
            //}
            BindConceptos();
            if (!string.IsNullOrEmpty(ddlCategorias.SelectedValue) && ddlCategorias.SelectedValue.Equals("0"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_ddlCategoriasKey", "$('#divNuevaCategoria').modal('show');", true);
            }
        }

        /// <summary>
        /// Handles the Click event of the bAgregarCat control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarCat_Click(object sender, EventArgs e)
        {
            var count = 1;
            var sql = @"SELECT (ISNULL(MAX(no), 0) + 1) AS no FROM Cat_Catalogo1_C WHERE tipo = @tipo";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@tipo", "CategoriaConceptos");
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                int.TryParse(dr["no"].ToString(), out count);
            }
            _db.Desconectar();
            sql = @"INSERT INTO Cat_Catalogo1_C (no, descripcion, codigo, tipo) VALUES (@iCount, @desc, @sCount, @tipo)";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@tipo", "CategoriaConceptos");
            _db.AsignarParametroEntero("@iCount", count);
            _db.AsignarParametroCadena("@desc", tbNombreCat.Text);
            _db.AsignarParametroCadena("@sCount", count.ToString());
            dr = _db.EjecutarConsulta();
            if (dr.RecordsAffected > 0)
            {
                BindCategorias();
                tbNombreCat.Text = "";
                (Master as SiteMaster).MostrarAlerta(this, "Categoría creada.", 2, null, "$('#divNuevaCategoria').modal('hide');");
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "La categoría no se pudo crear. Intentelo nuevamente.", 4, null);
            }
            _db.Desconectar();
        }

        protected void bUnidades_Click(object sender, EventArgs e)
        {
            tbClaveSat.Text = "";
            tbUnidadMedida.Text = "";
            GridView2.DataBind();
            FillCatalog();
            ScriptManager.RegisterStartupScript(this, GetType(), "_bUnidades", "$('#divUnidades').modal('toggle');", true);
        }

        protected void bAgregarUnidad_Click(object sender, EventArgs e)
        {
            if (Session["CfdiVersion"].ToString().Equals("3.3"))
            {
                if (string.IsNullOrEmpty(tbClaveSat.Text) || !ValidarClaveUnidad(tbClaveSat.Text))
                {
                    (Master as SiteMaster).MostrarAlerta(this, "La clave de unidad especificada no existe en los catálogos del SAT", 4, null);
                    GridView2.DataBind();
                    return;
                }
            }
            var idInsertado = "";
            var descripcionExistente = "";
            DbDataReader dr;
            var btn = (LinkButton)sender;
            if (btn.ID.Equals(bAgregarUnidad.ID))
            {
                _db.Conectar();
                _db.CrearComando("SELECT id,descripcion FROM Cat_CveUnidad WHERE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(descripcion, ' ', ''), '.', ''), '/', ''), '\', ''), '-', ''), '_', '') = @descripcion AND claveSat = @claveSat");
                _db.AsignarParametroCadena("@descripcion", tbUnidadMedida.Text.Trim().Replace(" ", "").Replace(".", "").Replace("/", "").Replace(@"\", "").Replace("-", "").Replace("_", ""));
                _db.AsignarParametroCadena("@claveSat", tbClaveSat.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    idInsertado = dr[0].ToString();
                    descripcionExistente = dr[1].ToString();
                }
                _db.Desconectar();
            }
            if (!string.IsNullOrEmpty(idInsertado))
            {
                //(Master as SiteMaster).MostrarAlerta(this, "La clave \"" + tbClaveSat.Text + "\" ya tiene asignado el registro \"" + descripcionExistente + "\"", 4);
                var msjJs = "La clave \"" + tbClaveSat.Text + "\" ya tiene asignado el registro \"" + descripcionExistente + "\"<br/>¿Desea registrar la clave de todas formas?";
                var js = "confirmBootBox('" + msjJs + "','SimulateClick(\"" + bAgregarUnidadExistente.ClientID + "\")','','')";
                ScriptManager.RegisterStartupScript(this, GetType(), "_bAgregarUnidadExistente", js, true);
            }
            else
            {
                var mensaje = "";
                var mensajeError = "";
                try
                {
                    _db.Conectar();
                    _db.CrearComando("INSERT INTO Cat_CveUnidad (claveSat, descripcion) OUTPUT Inserted.id VALUES (@claveSat, @descripcion)");
                    _db.AsignarParametroCadena("@claveSat", tbClaveSat.Text);
                    _db.AsignarParametroCadena("@descripcion", tbUnidadMedida.Text);
                    dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        tbClaveSat.Text = "";
                        tbUnidadMedida.Text = "";
                        idInsertado = dr[0].ToString();
                    }
                    _db.Desconectar();
                    ddlClaveSat.SelectedValue = "";
                    ddlClaveSat_SelectedIndexChanged(null, null);
                }
                catch (Exception ex)
                {
                    mensaje = "El registro de Clave Unidad [" + tbClaveSat.Text + " -> " + tbUnidadMedida.Text + "] no se pudo registrar";
                    mensajeError = ex.ToString() + "<br/>" + _db.Comando.CommandText;
                    _log.Registrar(mensaje, "Conceptos.aspx.cs", "bAgregarUnidad_Click", "catch");
                    _db.Desconectar();
                    (Master as SiteMaster).MostrarAlerta(this, "El registro no se ha podido insertar, favor de intentarlo nuevamente", 4);
                }
            }
            GridView2.DataBind();
            lvUnidades.DataBind();
        }

        protected void ddlClaveSat_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbClaveSat.Text = ddlClaveSat.SelectedValue;
        }

        protected void ddlCveProdServ_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCveProdServ.Text = ddlCveProdServ.SelectedValue;
        }

        protected void bLoadXlsx_Click(object sender, EventArgs e)
        {
            Session["_XlsxCatalogoConceptos"] = null;
            ScriptManager.RegisterStartupScript(this, GetType(), "_bLoadXlsx_Click", "$('#divLoadXlsx').modal('show');", true);
        }

        protected void fileXlsx_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            var bytes = fileXlsx.FileBytes;
            Session["_XlsxCatalogoConceptos"] = bytes;
        }

        protected void lbCargarXlsx_Click(object sender, EventArgs e)
        {
            var sessionKey = Session["_XlsxCatalogoConceptos"];
            if (sessionKey != null)
            {
                var bytes = (byte[])sessionKey;
                if (bytes != null && bytes.Length > 30)
                {
                    try
                    {
                        var listaConceptos = GetConceptosFromXlsx(bytes);
                        if (listaConceptos.Count > 0)
                        {
                            var guardado = GuardaConceptos(listaConceptos);
                            if (guardado)
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Los conceptos se han cargado correctamente", 2, null, "window.location.href = '" + ResolveClientUrl("~/configuracion/Conceptos.aspx") + "';");
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Los conceptos no pudieron ser guardados en su totalidad, favor de intentarlo nuevamente", 4);
                            }
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "El archivo no contiene ningun registro válido", 4);
                        }
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El archivo está corrupto o no tiene la estructura correcta", 4);
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Archivo vacío o corrupto", 4);
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se ha cargado ningun archivo", 4);
            }
        }

        private bool ValidarClaveProdServ(string clave)
        {
            var catalogos = (CatCdfi)Session["CatalogosCfdi33"];
            var existe = catalogos.CClaveprodserv.Any(c => c.Key.Equals(clave, StringComparison.OrdinalIgnoreCase));
            return existe;
        }

        private bool ValidarClaveUnidad(string clave)
        {
            var catalogos = (CatCdfi)Session["CatalogosCfdi33"];
            var existe = catalogos.CClaveunidad.Any(c => c.Key.Equals(clave, StringComparison.OrdinalIgnoreCase));
            return existe;
        }

        private List<string[]> GetConceptosFromXlsx(byte[] xlsx)
        {
            var result = new List<string[]>();
            Stream stream = new MemoryStream(xlsx);
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.First();
                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;
                const int skipRows = 1;
                for (var row = (start.Row + skipRows); row <= end.Row; row++)
                {
                    var record = new string[5];
                    record[0] = workSheet.Cells[row, 1].Text;
                    record[1] = workSheet.Cells[row, 2].Text;
                    record[2] = workSheet.Cells[row, 3].Text;
                    record[3] = workSheet.Cells[row, 4].Text;
                    record[4] = workSheet.Cells[row, 5].Text;
                    var isEmptyRow = !record.Any(cell => !string.IsNullOrEmpty(cell));
                    if (!isEmptyRow) { result.Add(record); }
                }
            }
            return result;
        }

        private bool GuardaConceptos(List<string[]> listaConceptos)
        {
            var result = false;
            int inserted = 0;
            foreach (var record in listaConceptos)
            {
                try
                {
                    DbDataReader dr;
                    var categoria = record[0].Trim();
                    var textoConcepto = record[1].Trim();
                    var claveProdServ = record[2].Trim();
                    var textoUM = record[3].Trim();
                    var claveUM = record[4].Trim();
                    var idCategoria = "";
                    var idUnidadMedida = "";
                    var idConcepto = "";
                    //if (Session["CfdiVersion"].ToString().Equals("3.3"))
                    //{
                    if (string.IsNullOrEmpty(claveProdServ) || !ValidarClaveProdServ(claveProdServ) || string.IsNullOrEmpty(claveUM) || !ValidarClaveUnidad(claveUM))
                    {
                        continue;
                    }
                    //}
                    if (!string.IsNullOrEmpty(categoria))
                    {
                        _db.Conectar();
                        _db.CrearComando("SELECT idCatalogo1_C FROM Cat_Catalogo1_C WHERE descripcion = @desc");
                        _db.AsignarParametroCadena("@desc", categoria);
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            idCategoria = dr[0].ToString();
                        }
                        _db.Desconectar();
                    }
                    if (string.IsNullOrEmpty(idCategoria))
                    {
                        var count = 1;
                        var sql = @"SELECT (ISNULL(MAX(no), 0) + 1) AS no FROM Cat_Catalogo1_C WHERE tipo = @tipo";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@tipo", "CategoriaConceptos");
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            int.TryParse(dr["no"].ToString(), out count);
                        }
                        _db.Desconectar();
                        sql = @"INSERT INTO Cat_Catalogo1_C (no, descripcion, codigo, tipo) OUTPUT inserted.idCatalogo1_C VALUES (@iCount, @desc, @sCount, @tipo)";
                        _db.Conectar();
                        _db.CrearComando(sql);
                        _db.AsignarParametroCadena("@tipo", "CategoriaConceptos");
                        _db.AsignarParametroEntero("@iCount", count);
                        _db.AsignarParametroCadena("@desc", !string.IsNullOrEmpty(categoria) ? categoria : "IMPORTADOS");
                        _db.AsignarParametroCadena("@sCount", count.ToString());
                        dr = _db.EjecutarConsulta();
                        if (dr.Read())
                        {
                            idCategoria = dr[0].ToString();
                        }
                        _db.Desconectar();
                    }
                    if (!string.IsNullOrEmpty(idCategoria))
                    {
                        if (!string.IsNullOrEmpty(textoUM) && !string.IsNullOrEmpty(claveUM))
                        {
                            _db.Conectar();
                            _db.CrearComando("SELECT id FROM Cat_CveUnidad WHERE descripcion = @descripcion AND claveSat = @claveSat");
                            _db.AsignarParametroCadena("@descripcion", textoUM);
                            _db.AsignarParametroCadena("@claveSat", claveUM);
                            dr = _db.EjecutarConsulta();
                            if (dr.Read())
                            {
                                idUnidadMedida = dr[0].ToString();
                            }
                            _db.Desconectar();
                            if (string.IsNullOrEmpty(idUnidadMedida))
                            {
                                _db.Conectar();
                                _db.CrearComando("INSERT INTO Cat_CveUnidad (claveSat, descripcion) OUTPUT Inserted.id VALUES (@claveSat, @descripcion)");
                                _db.AsignarParametroCadena("@claveSat", claveUM);
                                _db.AsignarParametroCadena("@descripcion", textoUM);
                                dr = _db.EjecutarConsulta();
                                if (dr.Read())
                                {
                                    idUnidadMedida = dr[0].ToString();
                                }
                                _db.Desconectar();
                            }
                        }
                        if (!string.IsNullOrEmpty(idUnidadMedida))
                        {
                            _db.Conectar();
                            _db.CrearComando("SELECT idConcepto FROM Cat_CatConceptos_C WHERE idCategoria = @idCategoria AND descripcion = @descripcion AND unidadMedida = @textoUnidad");
                            _db.AsignarParametroCadena("@idCategoria", idCategoria);
                            _db.AsignarParametroCadena("@descripcion", textoConcepto);
                            _db.AsignarParametroCadena("@textoUnidad", textoUM);
                            dr = _db.EjecutarConsulta();
                            if (dr.Read())
                            {
                                idConcepto = dr[0].ToString();
                            }
                            _db.Desconectar();
                            if (string.IsNullOrEmpty(idConcepto))
                            {
                                _db.Conectar();
                                _db.CrearComando(@"INSERT INTO Cat_CatConceptos_C
                                    (idCategoria, descripcion, unidadMedida, claveProdServ)
                                    VALUES
                                   (@idCategoria, @descripcion, @unidadMedida, @claveProdServ)");
                                _db.AsignarParametroCadena("@idCategoria", idCategoria);
                                _db.AsignarParametroCadena("@descripcion", textoConcepto);
                                _db.AsignarParametroCadena("@unidadMedida", textoUM);
                                _db.AsignarParametroCadena("@claveProdServ", claveProdServ);
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                            }
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            result = inserted.Equals(listaConceptos.Count);
            return result;
        }
    }
}