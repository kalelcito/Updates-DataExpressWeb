// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 12-10-2016
// ***********************************************************************
// <copyright file="visorEventos.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using Datos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DataExpressWeb
{
    /// <summary>
    /// Class VisorEventos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class VisorEventos : Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                SqlDataSource1.ConnectionString = _db.CadenaConexion;
            }
            if (!IsPostBack)
            {
                DataBindEventos();
                gvEventos.Sort("fecha", SortDirection.Descending);
            }
        }

        /// <summary>
        /// Datas the bind eventos.
        /// </summary>
        /// <param name="estado">The estado.</param>
        /// <param name="fecha">The fecha.</param>
        /// <param name="descripcion">The descripcion.</param>
        private void DataBindEventos(string estado = "", string fecha = "", string descripcion = "")
        {
            var where = "";
            var sql = "SELECT TOP 100 codigo, origen1 + '.' + origen2 AS origen, descripcion, detallesTecnicos, fecha FROM Logs";
            if (!string.IsNullOrEmpty(estado))
            {
                where += (!string.IsNullOrEmpty(where) ? " AND " : " WHERE ") + "codigo = '" + estado + "'";
            }
            if (!string.IsNullOrEmpty(fecha))
            {
                where += (!string.IsNullOrEmpty(where) ? " AND " : " WHERE ") + "'" + fecha + "' = FORMAT(fecha, 'yyyy/MM/dd')";
            }
            if (!string.IsNullOrEmpty(descripcion))
            {
                where += (!string.IsNullOrEmpty(where) ? " AND " : " WHERE ") + "descripcion LIKE '%" + descripcion + "%'";
            }
            sql += where + " ORDER BY fecha DESC";
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.DataBind();
            gvEventos.DataBind();
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvEventos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gvEventos_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    var lb = (HyperLink)(e.Row.FindControl("lbPopupDetalles"));
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var name = row.Field<string>("detallesTecnicos");
                    if (!string.IsNullOrEmpty(name))
                    {
                        var nameScaped = name.Replace("'", "\"");
                        lb.Attributes["data-content"] = name + "<br/><div align='right'><button type='button' id='btnPopover_" + lb.ClientID + "' class='btn btn-primary btn-sm btn-clip' data-clipboard-text='" + nameScaped + "' data-clipboard-action='copy'><i class='fa fa-clipboard'></i> Copiar</button>&nbsp;<button type='button' id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvEventos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvEventos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEventos.PageIndex = e.NewPageIndex;
            DataBindEventos(ddlEstado.SelectedValue, tbFecha.Text, tbDescripción.Text);
        }

        /// <summary>
        /// Handles the Click event of the lbBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            DataBindEventos(ddlEstado.SelectedValue, tbFecha.Text, tbDescripción.Text);
        }

        /// <summary>
        /// Handles the Click event of the lbLimpiar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbLimpiar_Click(object sender, EventArgs e)
        {
            ddlEstado.SelectedValue = "";
            tbFecha.Text = "";
            tbDescripción.Text = "";
            DataBindEventos(ddlEstado.SelectedValue, tbFecha.Text, tbDescripción.Text);
        }

        /// <summary>
        /// Handles the Sorting event of the gvEventos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvEventos_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataBindEventos(ddlEstado.SelectedValue, tbFecha.Text, tbDescripción.Text);
        }
    }
}