// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="Impuestos.aspx.cs" company="DataExpress Latinoamérica">
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
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

namespace Configuracion
{
    /// <summary>
    /// Class Impuestos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Impuestos : Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;
        /// <summary>
        /// The _id editar
        /// </summary>
        private static string _idEditar;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            SqlDataSource3.ConnectionString = _db.CadenaConexion;
            SqlDataTipo.ConnectionString = _db.CadenaConexion;
            SqlDataImpuesto.ConnectionString = _db.CadenaConexion;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('toggle');", true);
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
            tbImpuesto.ReadOnly = !chkEditar.Checked;
            tbValor.ReadOnly = !chkEditar.Checked;
            tbBase.ReadOnly = !chkEditar.Checked;
            ddlImpuesto.Enabled = chkEditar.Checked;
            ddlTipo.Enabled = chkEditar.Checked;
            ddlTipoFactor.Enabled = chkEditar.Checked;
            bAgregar.Visible = chkEditar.Checked;
            var css = "form-control" + (chkEditar.Checked ? " input-decimal" : "");
            tbValor.CssClass = css;
            RequiredFieldValidator5.Enabled = Session["CfdiVersion"].ToString().Equals("3.3") && Regex.IsMatch(ddlTipo.SelectedValue, @"^(01|02)$");
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
            if (string.IsNullOrEmpty(_idEditar))
            {
                _db.Conectar();
                _db.CrearComando("SELECT id FROM Cat_CatImpuestos_C WHERE codigo = @codigo");
                _db.AsignarParametroCadena("@codigo", tbCodigo.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    _db.Desconectar();
                    (Master as SiteMaster).MostrarAlerta(this, "El impuesto con el código " + tbCodigo.Text + " ya esta registrado.", 4, null);
                    return;
                }
                _db.Desconectar();
                sql = @"INSERT INTO Cat_CatImpuestos_C
                            (descripcion, valor, codigo, comentarios, tipo, tipoFactor, porcentajeBase)
                            VALUES
                           (@descripcion, @valor, @codigo, @comentarios, @tipo, @tipoFactor, @porcentajeBase)";
            }
            else
            {
                _db.Conectar();
                _db.CrearComando("SELECT id FROM Cat_CatImpuestos_C WHERE id <> @id AND codigo = @codigo");
                _db.AsignarParametroCadena("@id", _idEditar);
                _db.AsignarParametroCadena("@codigo", tbCodigo.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    _db.Desconectar();
                    (Master as SiteMaster).MostrarAlerta(this, "El impuesto con el código " + tbCodigo.Text + " ya esta registrado.", 4, null);
                    return;
                }
                _db.Desconectar();
                sql = @"UPDATE Cat_CatImpuestos_C SET descripcion = @descripcion, valor = @valor, codigo = @codigo, comentarios = @comentarios, tipo = @tipo, tipoFactor = @tipoFactor, porcentajeBase = @porcentajeBase WHERE id=@id";
            }
            try
            {
                if (Session["CfdiVersion"].ToString().Equals("3.3") && ddlImpuesto.Visible)
                {
                    var catalogos = (CatCdfi)Session["CatalogosCfdi33"];
                    var existeImpuesto = catalogos.CImpuesto.Any(c => c.Value.Equals(tbImpuesto.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                    var existeTasaCuota = catalogos.CTasaocuota.Any(c => c.Key.Equals(tbImpuesto.Text.Trim()) && c.Value.Split('-')[0].Equals(ddlTipoFactor.SelectedValue, StringComparison.OrdinalIgnoreCase) && decimal.Parse(CerosNull(tbValor.Text)) >= decimal.Parse(c.Value.Split('-')[1]) && decimal.Parse(CerosNull(tbValor.Text)) <= decimal.Parse(c.Value.Split('-')[2]));
                    // Validations
                    if (!existeImpuesto)
                    {
                        throw new Exception($"El impuesto {tbImpuesto.Text} no existe en el catálogo del SAT");
                    }
                    else if (!existeTasaCuota)
                    {
                        throw new Exception($"La tasa del impuesto {tbImpuesto.Text} no está en el rango del catálogo del SAT");
                    }
                }
                _db.Conectar();
                _db.CrearComando(sql);
                if (!string.IsNullOrEmpty(_idEditar))
                {
                    _db.AsignarParametroCadena("@id", _idEditar);
                }
                _db.AsignarParametroCadena("@descripcion", tbImpuesto.Text);
                _db.AsignarParametroCadena("@valor", CerosNull(tbValor.Text));
                _db.AsignarParametroCadena("@codigo", tbCodigo.Text);
                _db.AsignarParametroCadena("@comentarios", tbDescripcion.Text);
                _db.AsignarParametroCadena("@tipo", ddlTipo.SelectedValue);
                _db.AsignarParametroCadena("@tipoFactor", ddlTipoFactor.SelectedValue);
                _db.AsignarParametroCadena("@porcentajeBase", tbBase.Text);
                _db.EjecutarConsulta1();
                tbCodigo.Text = "";
                tbImpuesto.Text = "";
                tbValor.Text = "";
                tbDescripcion.Text = "";
                ddlTipo.SelectedValue = "0";
                ddlTipoFactor.SelectedValue = "0";
                (Master as SiteMaster).MostrarAlerta(this, "El impuesto se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');");
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El impuesto no se pudo agregar/modificar. Intentelo nuevamente.<br>" + ex.Message, 4, null);
            }
            _db.Desconectar();
            SqlDataSource3.DataBind();
            GridView1.DataBind();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divNuevo').modal('toggle');", true);
        }

        /// <summary>
        /// Llenarlistas the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void Llenarlista(string id)
        {
            var sql = @"SELECT [codigo]
                              ,[descripcion]
                              ,[valor]
                              ,[comentarios]
                              ,[tipo]
                              ,[tipoFactor]
                              ,[porcentajeBase]
                          FROM [Cat_CatImpuestos_C]
                          WHERE id=@id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", id);
            var dr = _db.EjecutarConsulta();
            var impuestos = new List<object[]>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var impuesto = new object[dr.FieldCount];
                    dr.GetValues(impuesto);
                    impuestos.Add(impuesto);
                }
                _db.Desconectar();
                foreach (var impuesto in impuestos)
                {
                    ddlTipo.SelectedValue = impuesto[4].ToString();
                    ddlTipo_SelectedIndexChanged(null, null);
                    tbCodigo.Text = impuesto[0].ToString();
                    tbImpuesto.Text = impuesto[1].ToString();
                    var selected = ddlImpuesto.Items.FindByText(tbImpuesto.Text);
                    if (selected != null)
                    {
                        ddlImpuesto.SelectedValue = selected.Value;
                    }
                    tbValor.Text = impuesto[2].ToString();
                    tbDescripcion.Text = impuesto[3].ToString();
                    try
                    {
                        ddlTipoFactor.SelectedValue = impuesto[5].ToString();
                    }
                    catch (Exception ex)
                    {
                        ddlTipoFactor.SelectedValue = "0";
                    }
                    try
                    {
                        tbBase.Text = impuesto[6].ToString();
                    }
                    catch (Exception ex)
                    {
                        tbBase.Text = "0";
                    }
                }
            }
            else
            {
                ddlImpuesto.Visible = false;
                tbImpuesto.Visible = true;
                tbCodigo.Text = "";
                tbImpuesto.Text = "";
                tbValor.Text = "";
                tbDescripcion.Text = "";
                tbBase.Text = "0";
                ddlTipo.SelectedValue = "0";
                ddlTipoFactor.SelectedValue = "0";
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
        /// Handles the SelectedIndexChanged event of the ddlTipo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbImpuesto.Text = "";
            switch (ddlTipo.SelectedValue)
            {
                case "01":
                    //RETENCION
                    SqlDataImpuesto.SelectParameters["tipo"].DefaultValue = "Impuesto Retenido";
                    ddlImpuesto.Visible = true;
                    tbImpuesto.Visible = false;
                    ddlImpuesto.DataBind();
                    tbImpuesto.Text = ddlImpuesto.SelectedItem.Text;
                    baseContainer.Style["display"] = "none";
                    break;
                case "02":
                    //TRASLADO
                    SqlDataImpuesto.SelectParameters["tipo"].DefaultValue = "Impuesto Trasladado";
                    ddlImpuesto.Visible = true;
                    tbImpuesto.Visible = false;
                    ddlImpuesto.DataBind();
                    tbImpuesto.Text = ddlImpuesto.SelectedItem.Text;
                    baseContainer.Style["display"] = "none";
                    break;
                default:
                    //LOCAL O OTRO
                    SqlDataImpuesto.SelectParameters["tipo"].DefaultValue = "";
                    ddlImpuesto.Visible = false;
                    tbImpuesto.Visible = true;
                    ddlImpuesto.DataBind();
                    baseContainer.Style["display"] = Session["CfdiVersion"].ToString().Equals("3.3") ? "inline" : "none";
                    break;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlImpuesto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbImpuesto.Text = ddlImpuesto.SelectedItem.Text;
        }
    }
}