// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 02-19-2017
//
// Last Modified By : Sergio
// Last Modified On : 02-19-2017
// ***********************************************************************
// <copyright file="FacturaGlobal.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using AjaxControlToolkit;
using Control;
using DataExpressWeb.wsRecepcion;
using Datos;
using System.Web.UI.WebControls;

namespace DataExpressWeb.adminstracion
{
    /// <summary>
    /// Class FacturaGlobal.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class FacturaGlobal : System.Web.UI.Page
    {
        /// <summary>
        /// The _files
        /// </summary>
        private static Dictionary<int, HttpPostedFile[]> _files;
        /// <summary>
        /// The _log
        /// </summary>
        private Log _log;
        /// <summary>
        /// The _dbe
        /// </summary>
        private BasesDatos _dbe;

        /// <summary>
        /// The _DBR
        /// </summary>
        private BasesDatos _dbr;
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _dbe = new BasesDatos(Session["IDENTEMI"].ToString());
            SqlDataRFC.ConnectionString = _dbe.CadenaConexion;
            SqlDataSourceRfc.ConnectionString = _dbe.CadenaConexion;

            if (Session["IDENTEMI"] != null)
            {
                if (Session["errorTimbres"] != null && Convert.ToBoolean(Session["errorTimbres"]))
                {
                    Response.Redirect("~/CantidadTimbres.aspx", true);
                    return;
                }
                else if (Session["errorCertificado"] != null && Convert.ToBoolean(Session["errorCertificado"]))
                {
                    Response.Redirect("~/LicExpirada.aspx", true);
                    return;
                }
                _dbe = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                _dbr = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE", "Recepcion");
                _log = new Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");

                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                }
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvEmpleados control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvEmpleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRFC.PageIndex = e.NewPageIndex;
            gvRFC.DataBind();
        }
        /// <summary>
        /// Handles the Click event of the bAgregarRFC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bAgregarRFC_Click(object sender, EventArgs e)
        {
            var existe = false;
            var rfc = "";
            _dbe.Conectar();
            var sql = "";
            sql = @"select id FROM Cat_Aerolinea where rfc = @rfc";
            _dbe.Conectar();
            _dbe.CrearComando(sql);
            _dbe.AsignarParametroCadena("@rfc", DropDownListRFC.SelectedItem.Text);
            var dr = _dbe.EjecutarConsulta();
            if (dr.Read())
            {
                existe = true;
            }
            _dbe.Desconectar();
            if (!existe)
            {
                _dbe.Conectar();
                _dbe.CrearComando(@"INSERT INTO Cat_Aerolinea VALUES ( @rfc, @tiempo)");
                _dbe.AsignarParametroCadena("@rfc", DropDownListRFC.SelectedValue);
                _dbe.AsignarParametroCadena("@tiempo", DropDownListPeriodo.SelectedValue);
                _dbe.EjecutarConsulta1();
                _dbe.Desconectar();
                SqlDataSourceRfc.DataBind();
                gvRFC.DataBind();
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El RFC ya se encuentra registrado", 4);
            }




        }

        /// <summary>
        /// Handles the RowEditing event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewEditEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRFC.EditIndex = e.NewEditIndex;
        }

        /// <summary>
        /// Handles the RowUpdating event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewUpdateEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var tiempo = gvRFC.DataKeys[e.RowIndex].Value.ToString();
                var row = gvRFC.Rows[e.RowIndex];
                var tiempos = (DropDownList)row.FindControl("DropDownListPeriodo2");
                SqlDataSourceRfc.UpdateParameters["tiempo"].DefaultValue = tiempos.SelectedValue.ToString();
                SqlDataSourceRfc.UpdateParameters["id"].DefaultValue = tiempo;
                SqlDataSourceRfc.Update();
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El concepto no se pudo editar, inténtelo nuevamente.<br/><br/>" + ex.Message, 4);
            }
            finally
            {
                gvRFC.EditIndex = -1;
            }
        }

        /// <summary>
        /// Handles the RowCancelingEdit event of the gvConceptos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        protected void gvConceptos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRFC.EditIndex = -1;

        }


    }
}