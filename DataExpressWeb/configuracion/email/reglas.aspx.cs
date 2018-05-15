// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="reglas.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;
using Datos;
using System.Web.UI.WebControls;
using System.Data.Common;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Distribucion.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Distribucion : Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;
        /// <summary>
        /// The _id editar
        /// </summary>
        private static string _idEditar = null;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IDENTEMI"] != null)
            {
                if (!IsPostBack)
                {
                    _db = new BasesDatos(Session["IDENTEMI"].ToString());
                }
                reglasDataSource.ConnectionString = _db.CadenaConexion;
            }
            else
            {
                Response.Redirect("~/Seguridad.aspx");
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
        /// Handles the CheckedChanged event of the chkEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            tbRFC.ReadOnly = !chkEditar.Checked;
            ddlEstado.Enabled = chkEditar.Checked;
            tbNombre.ReadOnly = !chkEditar.Checked;
            tbEmail.ReadOnly = !chkEditar.Checked;
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
                sql = @"INSERT INTO [Cat_EmailsReglas]
                                   ([nombreRegla]
                                   ,[estadoRegla]
                                   ,[eliminado]
                                   ,[Receptor]
                                   ,[emailsRegla])
                             VALUES
                                   (@nombreRegla
                                   ,@estadoRegla
                                   ,'0'
                                   ,@Receptor
                                   ,@emailsRegla)";
            }
            else
            {
                sql = @"UPDATE [Cat_EmailsReglas]
                           SET [nombreRegla] = @nombreRegla
                              ,[estadoRegla] = @estadoRegla
                              ,[Receptor] = @Receptor
                              ,[emailsRegla] = @emailsRegla
                         WHERE idEmailRegla = @ID";
            }
            try
            {
                _db.Conectar();
                _db.CrearComando(sql);
                if (!string.IsNullOrEmpty(_idEditar))
                {
                    _db.AsignarParametroCadena("@ID", _idEditar);
                }
                _db.AsignarParametroCadena("@nombreRegla", tbNombre.Text);
                _db.AsignarParametroCadena("@estadoRegla", ddlEstado.SelectedValue);
                _db.AsignarParametroCadena("@Receptor", tbRFC.Text);
                _db.AsignarParametroCadena("@emailsRegla", tbEmail.Text);

                dr = _db.EjecutarConsulta();
                var records = (dr.RecordsAffected > 0);
                _db.Desconectar();
                if (records)
                {
                    tbRFC.Text = "";
                    ddlEstado.SelectedValue = "1";
                    tbNombre.Text = "";
                    tbEmail.Text = "";
                    reglasDataSource.DataBind();
                    gvReglas.DataBind();
                    (Master as SiteMaster).MostrarAlerta(this, "La regla se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');");
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "La regla no se pudo agregar/modificar. Intentelo nuevamente", 4, null);
                }
            }
            catch (Exception)
            {
                (Master as SiteMaster).MostrarAlerta(this, "La regla no se pudo agregar/modificar. Intentelo nuevamente", 4, null);
            }
        }

        /// <summary>
        /// Llenarlistas the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void Llenarlista(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var sql = @"SELECT nombreRegla, CONVERT(CHAR(1),ISNULL(estadoRegla, '0')) AS estadoRegla, Receptor, emailsRegla FROM Cat_EmailsReglas WHERE idEmailRegla = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", id);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbRFC.Text = dr["Receptor"].ToString();
                    ddlEstado.SelectedValue = dr["estadoRegla"].ToString();
                    tbNombre.Text = dr["nombreRegla"].ToString();
                    tbEmail.Text = dr["emailsRegla"].ToString();
                }
                _db.Desconectar();
            }
            else
            {
                tbRFC.Text = "";
                ddlEstado.SelectedValue = "1";
                tbNombre.Text = "";
                tbEmail.Text = "";
            }
        }
    }
}