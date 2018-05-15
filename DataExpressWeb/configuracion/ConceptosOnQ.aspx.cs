// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="ConceptosOnQ.aspx.cs" company="DataExpress Latinoamérica">
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

namespace Configuracion
{
    /// <summary>
    /// Class ConceptosOnQ.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class ConceptosOnQ : Page
    {
        /// <summary>
        /// The _id editar
        /// </summary>
        private static string _idEditar;
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
                SqlDataSource3.ConnectionString = _db.CadenaConexion;
                SqlDataCategorias.ConnectionString = _db.CadenaConexion;
            }
            if (!IsPostBack)
            {
                BindConceptos();
                BindCategorias();
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
            if (string.IsNullOrEmpty(_idEditar))
            {
                _db.Conectar();
                _db.CrearComando("SELECT IDECONCEPTO FROM CatalogoConceptos WHERE DESCRIPCION = @descripcion AND CLASIFICACION = @clasificacion");
                _db.AsignarParametroCadena("@descripcion", tbDescripcion.Text);
                _db.AsignarParametroCadena("@clasificacion", ddlCategorias.SelectedValue);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    _db.Desconectar();
                    (Master as SiteMaster).MostrarAlerta(this, "El concepto ya esta registrado.", 4, null);
                    return;
                }
                _db.Desconectar();
                sql = @"INSERT INTO CatalogoConceptos
                            (DESCRIPCION, CLASIFICACION)
                            VALUES
                           (@descripcion, @clasificacion)";
            }
            else
            {
                _db.Conectar();
                _db.CrearComando("SELECT IDECONCEPTO FROM CatalogoConceptos WHERE IDECONCEPTO <> @id DESCRIPCION = @descripcion AND CLASIFICACION = @clasificacion");
                _db.AsignarParametroCadena("@id", _idEditar);
                _db.AsignarParametroCadena("@descripcion", tbDescripcion.Text);
                _db.AsignarParametroCadena("@clasificacion", ddlCategorias.SelectedValue);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    _db.Desconectar();
                    (Master as SiteMaster).MostrarAlerta(this, "El concepto ya esta registrado.", 4, null);
                    return;
                }
                _db.Desconectar();
                sql = @"UPDATE CatalogoConceptos SET DESCRIPCION = @descripcion, CLASIFICACION = @clasificacion WHERE IDECONCEPTO=@idConcepto";
            }
            try
            {
                _db.Conectar();
                _db.CrearComando(sql);
                if (!string.IsNullOrEmpty(_idEditar))
                {
                    _db.AsignarParametroCadena("@idConcepto", _idEditar);
                }
                _db.AsignarParametroCadena("@clasificacion", ddlCategorias.SelectedValue);
                _db.AsignarParametroCadena("@descripcion", tbDescripcion.Text);
                _db.EjecutarConsulta1();
                tbDescripcion.Text = "";
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
            tbDescripcion.ReadOnly = !chkEditar.Checked;
            bAgregar.Visible = chkEditar.Checked;
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
        /// Llenarlistas the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void Llenarlista(string id)
        {
            tbDescripcion.Text = "";
            var sql = @"SELECT [DESCRIPCION]
                              ,[CLASIFICACION]
                          FROM [CatalogoConceptos]
                          WHERE IDECONCEPTO=@id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", id);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                tbDescripcion.Text = dr["DESCRIPCION"].ToString();
            }
            _db.Desconectar();
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
            var sql = @"DELETE FROM CatalogoConceptos WHERE CLASIFICACION = @clasificacion";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@clasificacion", ddlCategorias.SelectedValue);
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
            ddlCategorias.Items.Add(new ListItem("Nueva Categoría", "0"));
        }

        /// <summary>
        /// Binds the conceptos.
        /// </summary>
        protected void BindConceptos()
        {
            SqlDataSource3.SelectCommand = "SELECT IDECONCEPTO, DESCRIPCION FROM CatalogoConceptos WHERE CLASIFICACION = '" + ddlCategorias.SelectedValue + "'";
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
            BindConceptos();
            if (!string.IsNullOrEmpty(ddlCategorias.SelectedValue) && ddlCategorias.SelectedValue.Equals("0"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_ddlCategoriasKey", "$('#divNuevaCategoria').modal('show');", true);
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindConceptos();
        }

        /// <summary>
        /// Handles the RowDeleted event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeletedEventArgs" /> instance containing the event data.</param>
        protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            BindConceptos();
        }

        /// <summary>
        /// Handles the Click event of the bAgregarCat control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregarCat_Click(object sender, EventArgs e)
        {
            var sql = @"INSERT INTO CatalogoConceptos (DESCRIPCION, CLASIFICACION) VALUES (@desc, @clas)";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@desc", tbConceptoInicial.Text);
            _db.AsignarParametroCadena("@clas", tbNombreCat.Text);
            var dr = _db.EjecutarConsulta();
            if (dr.RecordsAffected > 0)
            {
                BindCategorias();
                tbNombreCat.Text = "";
                tbConceptoInicial.Text = "";
                (Master as SiteMaster).MostrarAlerta(this, "Categoría creada.", 2, null, "$('#divNuevaCategoria').modal('hide');");
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "La categoría no se pudo crear. Intentelo nuevamente.", 4, null);
            }
            _db.Desconectar();
        }
    }
}