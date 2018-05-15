// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="series.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataExpressWeb;
using Datos;

namespace Administracion
{
    /// <summary>
    /// Class Series.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Series : Page
    {
        /// <summary>
        /// The _id editar
        /// </summary>
        private static string _idEditar;
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;
        /// <summary>
        /// The _SQL series
        /// </summary>
        private const string _sqlSeries = "SELECT s.idSerie, s.serie, s.descripcion, tipoRecep.descripcion AS tipoRecep, ctipo.descripcion AS tipoDoc, cambi.descripcion AS ambiente FROM Cat_Series s INNER JOIN Cat_Catalogo1_C ctipo ON (s.tipoDoc = ctipo.codigo AND ctipo.tipo = 'Comprobante') INNER JOIN Cat_Catalogo1_C cambi ON(s.ambiente = cambi.codigo AND cambi.tipo = 'Ambiente') INNER JOIN Cat_Catalogo1_C tipoRecep ON(s.tipo = tipoRecep.codigo AND tipoRecep.tipo = 'TipoTrama')";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                SqlDataSource1.ConnectionString = _db.CadenaConexion;
                SqlDataAmbiente.ConnectionString = _db.CadenaConexion;
                SqlDataComprobante.ConnectionString = _db.CadenaConexion;
                SqlDataTipoRecep.ConnectionString = _db.CadenaConexion;
                if (!Page.IsPostBack)
                {
                    LimpiarBusqueda();
                }
            }
        }

        /// <summary>
        /// Buscars the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        private void Buscar(string sql)
        {
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.DataBind();
            gvSeries.DataBind();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            ddlAmbiente.Enabled = chkEditar.Checked;
            ddlTipoRecep.Enabled = chkEditar.Checked;
            ddlComprobante.Enabled = chkEditar.Checked;
            tbSerie.ReadOnly = !string.IsNullOrEmpty(_idEditar);
            tbDesc.ReadOnly = !chkEditar.Checked;
            bAgregar.Visible = chkEditar.Checked;
        }

        /// <summary>
        /// Handles the Click event of the bAgregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bAgregar_Click(object sender, EventArgs e)
        {
            var sql = "";
            DbDataReader dr;
            if (string.IsNullOrEmpty(_idEditar))
            {
                sql = @"INSERT INTO [Cat_Series]
                                   ([serie]
                                   ,[tipoDoc]
                                   ,[ambiente]
                                   ,[descripcion]
                                   ,[tipo])
                             VALUES
                                   (@serie
                                   ,@tipoDoc
                                   ,@ambiente
                                   ,@descripcion
                                   ,@tipo)";
            }
            else
            {
                sql = @"UPDATE [Cat_Series]
                           SET [serie] = @serie
                              ,[tipoDoc] = @tipoDoc
                              ,[ambiente] = @ambiente
                              ,[descripcion] = @descripcion
                              ,[tipo] = @tipo
                         WHERE idSerie = @ID";
            }
            try
            {
                _db.Conectar();
                _db.CrearComando(sql);
                if (!string.IsNullOrEmpty(_idEditar))
                {
                    _db.AsignarParametroCadena("@ID", _idEditar);
                }
                _db.AsignarParametroCadena("@serie", tbSerie.Text);
                _db.AsignarParametroCadena("@tipoDoc", ddlComprobante.SelectedValue);
                _db.AsignarParametroCadena("@ambiente", ddlAmbiente.SelectedValue);
                _db.AsignarParametroCadena("@descripcion", tbDesc.Text);
                _db.AsignarParametroCadena("@tipo", ddlTipoRecep.SelectedValue);
                dr = _db.EjecutarConsulta();
                if (dr.RecordsAffected > 0)
                {
                    tbSerie.Text = "";
                    tbDesc.Text = "";
                    ddlComprobante.SelectedValue = "0";
                    ddlAmbiente.SelectedValue = "0";
                    ddlTipoRecep.SelectedValue = "0";
                    LimpiarBusqueda();
                    (Master as SiteMaster).MostrarAlerta(this, "La serie se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 4, null, "$('#divNuevo').modal('hide');");
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "La serie no se pudo agregar/modificar. Intentelo nuevamente", 4, null);
                }
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "La serie no se pudo agregar/modificar. Intentelo nuevamente:<br/>" + ex.Message, 4, null);
            }
        }

        /// <summary>
        /// Handles the Click event of the bNuevo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bNuevo_Click(object sender, EventArgs e)
        {
            _idEditar = null;
            Llenarlista();
            chkEditar.Visible = false;
            chkEditar.Checked = true;
            chkEditar_CheckedChanged(null, null);
            bAgregar.Text = "Agregar";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('show');", true);
        }

        /// <summary>
        /// Llenarlistas the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void Llenarlista(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var sql = @"SELECT serie, tipoDoc, ambiente, descripcion, tipo FROM Cat_Series WHERE idSerie = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", id);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbSerie.Text = dr["serie"].ToString();
                    ddlAmbiente.SelectedValue = dr["ambiente"].ToString();
                    ddlTipoRecep.SelectedValue = dr["tipo"].ToString();
                    ddlComprobante.SelectedValue = dr["tipoDoc"].ToString();
                    tbDesc.Text = dr["descripcion"].ToString();
                }
                _db.Desconectar();
            }
            else
            {
                tbSerie.Text = "";
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
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divNuevo').modal('show');", true);
        }

        /// <summary>
        /// Handles the Click event of the bBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscar_Click(object sender, EventArgs e)
        {
            var where = "";
            where += !string.IsNullOrEmpty(tbBuscaDescripcion.Text) ? (string.IsNullOrEmpty(where) ? " WHERE " : " AND ") + "ctipo.descripcion LIKE '%" + tbBuscaDescripcion.Text + "%'" : "";
            where += !ddlBuscaAmbiente.SelectedValue.Equals("0") ? (string.IsNullOrEmpty(where) ? " WHERE " : " AND ") + "cambi.codigo = '" + ddlBuscaAmbiente.SelectedValue + "'" : "";
            where += !ddlBuscaComprobante.SelectedValue.Equals("0") ? (string.IsNullOrEmpty(where) ? " WHERE " : " AND ") + "ctipo.codigo = '" + ddlBuscaComprobante.SelectedValue + "'" : "";
            where += !ddlBuscaTipo.SelectedValue.Equals("0") ? (string.IsNullOrEmpty(where) ? " WHERE " : " AND ") + "tipoRecep.codigo = '" + ddlBuscaTipo.SelectedValue + "'" : "";
            Buscar(_sqlSeries + where);
        }

        /// <summary>
        /// Limpiars the busqueda.
        /// </summary>
        private void LimpiarBusqueda()
        {
            tbBuscaDescripcion.Text = "";
            ddlBuscaAmbiente.SelectedValue = "0";
            ddlBuscaComprobante.SelectedValue = "0";
            ddlBuscaTipo.SelectedValue = "0";
            Buscar(_sqlSeries);
        }

        /// <summary>
        /// Handles the Click event of the bActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bActualizar_Click(object sender, EventArgs e)
        {
            LimpiarBusqueda();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvSeries control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvSeries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSeries.PageIndex = e.NewPageIndex;
            Buscar(_sqlSeries);
        }

        /// <summary>
        /// Handles the RowDeleted event of the gvSeries control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeletedEventArgs" /> instance containing the event data.</param>
        protected void gvSeries_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            LimpiarBusqueda();
        }
    }
}