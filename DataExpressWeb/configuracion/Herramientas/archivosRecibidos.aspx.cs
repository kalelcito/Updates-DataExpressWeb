// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 02-19-2017
// ***********************************************************************
// <copyright file="archivosRecibidos.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using DataExpressWeb;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Text;

namespace Administracion
{
    /// <summary>
    /// Class ArchivosRecibidos.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class ArchivosRecibidos : Page
    {
        /// <summary>
        /// The command tramas
        /// </summary>
        private static readonly string CommandTramas = "SELECT TOP 500 t.idTrama, t.Trama, t.Fecha, e.userEmpleado, t.rfcemi, t.serie, t.folio, t.observaciones, t.noReserva, c.descripcion FROM Log_Trama t INNER JOIN Cat_Empleados e ON t.idUser = e.idEmpleado INNER JOIN Cat_Catalogo1_C c ON t.tipo = c.codigo AND c.tipo = 'TipoTrama'";
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
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            SqlDataTramas.ConnectionString = _db.CadenaConexion;
            SqlDataTipoTramas.ConnectionString = _db.CadenaConexion;
            if (!IsPostBack)
            {
                Buscar();
            }
        }

        /// <summary>
        /// Handles the Click event of the bBuscarReg control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            Buscar();
            ScriptManager.RegisterStartupScript(this, GetType(), "Clean_bBuscarReg_Click", "$('#FiltrosC2').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();", true);
        }

        /// <summary>
        /// Handles the Click event of the bActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bActualizar_Click(object sender, EventArgs e)
        {
            tbID.Text = "";
            tbFecha.Text = "";
            tbRFCEMI.Text = "";
            tbSerie.Text = "";
            tbFolio.Text = "";
            ddlTipo.SelectedValue = "0";
            Buscar();
        }

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
            var myWhere = "";
            if (!string.IsNullOrEmpty(tbID.Text))
            {
                myWhere += (string.IsNullOrEmpty(myWhere) ? " WHERE " : " AND ") + "t.idTrama = " + tbID.Text;
            }
            if (!string.IsNullOrEmpty(tbFecha.Text))
            {
                myWhere += (string.IsNullOrEmpty(myWhere) ? " WHERE " : " AND ") + "CAST(t.Fecha AS DATE) = '" + tbFecha.Text + "'";
            }
            if (!string.IsNullOrEmpty(tbRFCEMI.Text))
            {
                myWhere += (string.IsNullOrEmpty(myWhere) ? " WHERE " : " AND ") + "t.RFCEMI LIKE '%" + tbRFCEMI.Text + "%'";
            }
            if (!ddlTipo.SelectedValue.Equals("0"))
            {
                myWhere += (string.IsNullOrEmpty(myWhere) ? " WHERE " : " AND ") + "t.tipo = '" + ddlTipo.SelectedValue + "'";
            }
            if (!string.IsNullOrEmpty(tbSerie.Text))
            {
                myWhere += (string.IsNullOrEmpty(myWhere) ? " WHERE " : " AND ") + "t.serie = '" + tbSerie.Text.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(tbFolio.Text))
            {
                myWhere += (string.IsNullOrEmpty(myWhere) ? " WHERE " : " AND ") + "t.folio = '" + tbFolio.Text.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(tbObservaciones.Text))
            {
                myWhere += (string.IsNullOrEmpty(myWhere) ? " WHERE " : " AND ") + "t.observaciones LIKE '%" + tbObservaciones.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(tbReserva.Text))
            {
                myWhere += (string.IsNullOrEmpty(myWhere) ? " WHERE " : " AND ") + "t.noReserva = '" + tbReserva.Text.Trim() + "' OR t.noTicket = '" + tbReserva.Text.Trim() + "'";
            }
            SqlDataTramas.SelectCommand = CommandTramas + myWhere + " ORDER BY t.Fecha DESC";
        }

        /// <summary>
        /// Handles the Click event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/reportes/RepNoProcesados.aspx");
        }

        /// <summary>
        /// Tries the parse XML.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        /// <param name="res">The resource.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TryParseXml(string xmlString, out XmlDocument res)
        {
            try
            {
                res = new XmlDocument();
                res.LoadXml(xmlString);
                return true;
            }
            catch
            {
                res = null;
                return false;
            }
        }

        /// <summary>
        /// To the indented string.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>System.String.</returns>
        private string ToIndentedString(XmlDocument doc)
        {
            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            doc.Save(xmlTextWriter);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvTramas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gvTramas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    var lb = (HyperLink)e.Row.FindControl("lbPopupDetalles");
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var name = row.Field<string>("Trama");
                    if (!string.IsNullOrEmpty(name))
                    {
                        name = name.Trim();
                        XmlDocument xmlTrama;
                        var isXml = TryParseXml(name, out xmlTrama);
                        var nameScaped = name.Replace("'", "\"");
                        name = isXml ? ToIndentedString(xmlTrama) : name;
                        lb.Attributes["data-content"] = "<pre style='max-height: 500px'>" + (isXml ? "<xmp>" : "") + name + (isXml ? "</xmp>" : "") + "</pre><br/><div align='right'><button type='button' id='btnPopover_" + lb.ClientID + "' class='btn btn-primary btn-sm btn-clip' data-clipboard-text='" + nameScaped + "' data-clipboard-action='copy'><i class='fa fa-clipboard'></i> Copiar</button>&nbsp;<button type='button' id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
                    }
                }
                catch
                {
                }
                try
                {
                    var lb = (HyperLink)e.Row.FindControl("lbPopupObs");
                    var row = ((DataRowView)e.Row.DataItem).Row;
                    var name = row.Field<string>("observaciones");
                    if (!string.IsNullOrEmpty(name))
                    {
                        var nameScaped = name.Replace("'", "\"");
                        lb.Attributes["data-content"] = name + "<br/><div align='right'><button type='button' id='btnPopover_" + lb.ClientID + "' class='btn btn-primary btn-sm btn-clip' data-clipboard-text='" + nameScaped + "' data-clipboard-action='copy'><i class='fa fa-clipboard'></i> Copiar</button>&nbsp;<button type='button' id='close-popover' data-toggle='clickover' class='btn btn-primary btn-sm' onclick='$(\"#" + lb.ClientID + "\").popover(\"hide\");'>Cerrar</button></div>";
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvTramas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvTramas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Buscar();
            gvTramas.PageIndex = e.NewPageIndex;
            gvTramas.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the gvTramas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs" /> instance containing the event data.</param>
        protected void gvTramas_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["ordenar"].ToString() == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
            }
            gvTramas.Sort(e.SortExpression, e.SortDirection);
            Buscar();
            gvTramas.DataBind();
        }

        /// <summary>
        /// Converts the sort direction.
        /// </summary>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>System.String.</returns>
        private string ConvertSortDirection(SortDirection sortDirection)
        {
            var newSortDirection = string.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    Session["ordenar"] = newSortDirection;
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    Session["ordenar"] = newSortDirection;
                    break;
            }

            return newSortDirection;
        }
    }
}