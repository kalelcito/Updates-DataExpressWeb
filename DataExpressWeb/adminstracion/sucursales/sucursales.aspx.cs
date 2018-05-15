// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="sucursales.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataExpressWeb;
using Datos;

namespace Administracion
{
    /// <summary>
    /// Class Sucursales.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Sucursales : Page
    {
        /// <summary>
        /// The _id editar
        /// </summary>
        private static string _idEditar;
        /// <summary>
        /// The _separador
        /// </summary>
        private readonly string _separador = "|";
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
                _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                SqlDataSource1.ConnectionString = _db.CadenaConexion;
                if (!Page.IsPostBack)
                {
                    Buscar();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the bActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bActualizar_Click(object sender, EventArgs e)
        {
            tbRfcRec.Text = "";
            tbClave.Text = "";
            tbDomicilio.Text = "";
            tbSucursal.Text = "";
            Buscar();
        }

        /// <summary>
        /// Handles the Click event of the bBuscarReg control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
            var msjbuscar = "";
            var consulta = "";
            if (tbRfcRec.Text.Length != 0)
            {
                if (consulta.Length != 0)
                {
                    consulta = consulta + "RF" + tbRfcRec.Text + _separador;
                }
                else
                {
                    consulta = "RF" + tbRfcRec.Text + _separador;
                }
            }
            if (tbClave.Text.Length != 0)
            {
                if (consulta.Length != 0)
                {
                    consulta = consulta + "CL" + tbClave.Text + _separador;
                }
                else
                {
                    consulta = "CL" + tbClave.Text + _separador;
                }
            }
            if (tbSucursal.Text.Length != 0)
            {
                if (consulta.Length != 0)
                {
                    consulta = consulta + "SU" + tbSucursal.Text + _separador;
                }
                else
                {
                    consulta = "SU" + tbSucursal.Text + _separador;
                }
            }
            if (tbDomicilio.Text.Length != 0)
            {
                if (consulta.Length != 0)
                {
                    consulta = consulta + "DO" + tbDomicilio.Text + _separador;
                }
                else
                {
                    consulta = "DO" + tbDomicilio.Text + _separador;
                }
            }
            if (consulta.Length > 0)
            {
            }
            else
            {
                consulta = "-";
            }
            consulta = consulta.Trim('|');
            //if (Convert.ToBoolean(Session["asRoles"]))
            //{
            SqlDataSource1.SelectParameters["QUERY"].DefaultValue = consulta;
            //} else
            //{
            //    SqlDataSource1.SelectParameters["QUERY"].DefaultValue = "-";
            //}
            SqlDataSource1.DataBind();
            gvSucursales.DataBind();
            consulta = "";
            lMensaje.Text = msjbuscar;
        }

        /// <summary>
        /// Handles the DataBinding event of the grid_cajaSucursal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void grid_cajaSucursal_DataBinding(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the grid_sucursales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void grid_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Handles the PageIndexChanged event of the grid_sucursales control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void grid_sucursales_PageIndexChanged(object sender, EventArgs e)
        {
            gvSucursales.SelectedIndex = -1;
        }


        /// <summary>
        /// Shows the no result found.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="gv">The gv.</param>
        private void ShowNoResultFound(DataTable source, GridView gv) //funcion de prueba...se utiliza la siguiente
        {
            source.Rows.Add(source.NewRow()); // create a new blank row to the DataTable
            gv.DataSourceID = null; // cambiar el datasource para asignarle uno en blanco
            // Bind the DataTable which contain a blank row to the GridView
            gv.DataSource = source;
            gv.DataBind();
            // Get the total number of columns in the GridView to know what the Column Span should be
            var columnsCount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear(); // clear all the cells in the row
            gv.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
            gv.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

            //You can set the styles here
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            //set No Results found to the new added cell
            gv.Rows[0].Cells[0].Text = "NO RESULT FOUND!";
        }

        /// <summary>
        /// Binds the empty row.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="gv">The gv.</param>
        private void BindEmptyRow(DataTable source, GridView gv)
        {
            gv.Dispose();
            gv.DataSourceID = null; // cambiar el datasource para asignarle uno en blanco
            var t = new DataTable();
            t = source.Clone(); // Clone Source Table

            foreach (DataColumn c in t.Columns)
            {
                c.AllowDBNull = true; // Allow Nulls in all columns
            }

            t.Rows.Add(t.NewRow()); // Add empty row

            gv.DataSource = t; // Set Source to clone table with 1 row
            gv.DataBind(); // bind to empty table
            //gv.Rows[0].Cells[0].Text = gv.EmptyDataText; //If you dont want to show the Message

            gv.Rows[0].Visible = false;
            gv.Rows[0].Controls.Clear();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gv_cajaSucursal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gv_cajaSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            tbClaveNuevo.ReadOnly = !chkEditar.Checked;
            tbTelNuevo.ReadOnly = !chkEditar.Checked;
            tbSucursalNuevo.ReadOnly = !chkEditar.Checked;
            //tbRfcRecNuevo.ReadOnly = !string.IsNullOrEmpty(_idEditar);
            tbCalleRec.ReadOnly = !chkEditar.Checked;
            tbNoExtRec.ReadOnly = !chkEditar.Checked;
            tbNoIntRec.ReadOnly = !chkEditar.Checked;
            tbColoniaRec.ReadOnly = !chkEditar.Checked;
            tbLocRec.ReadOnly = !chkEditar.Checked;
            tbRefRec.ReadOnly = !chkEditar.Checked;
            tbMunicipioRec.ReadOnly = !chkEditar.Checked;
            tbEstadoRec.ReadOnly = !chkEditar.Checked;
            tbPaisRec.ReadOnly = !chkEditar.Checked;
            tbCpRec.ReadOnly = !chkEditar.Checked;
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
            var clave = tbClaveNuevo.Text;
            if (string.IsNullOrEmpty(_idEditar))
            {
                sql = @"SELECT idSucursal FROM Cat_Sucursales WHERE clave = @clave";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@clave", clave);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Ya existe una sucursal con el código " + clave, 4, null);
                    _db.Desconectar();
                    return;
                }
                _db.Desconectar();
                sql = @"INSERT INTO [Cat_Sucursales]
                                   ([clave]
                                   ,[sucursal]
                                   ,[eliminado]
                                   ,[telefono]
                                   ,[RFC]
                                   ,[calle]
                                   ,[noExterior]
                                   ,[noInterior]
                                   ,[colonia]
                                   ,[localidad]
                                   ,[referencia]
                                   ,[municipio]
                                   ,[estado]
                                   ,[pais]
                                   ,[codigoPostal])
                             VALUES
                                   (@clave
                                   ,@sucursal
                                   ,@eliminado
                                   ,@telefono
                                   ,@RFC
                                   ,@calle
                                   ,@noExterior
                                   ,@noInterior
                                   ,@colonia
                                   ,@localidad
                                   ,@referencia
                                   ,@municipio
                                   ,@estado
                                   ,@pais
                                   ,@codigoPostal)";
            }
            else
            {
                sql = @"UPDATE [Cat_Sucursales]
                           SET [clave] = @clave
                              ,[sucursal] = @sucursal
                              ,[eliminado] = @eliminado
                              ,[telefono] = @telefono
                              ,[RFC] = @RFC
                              ,[calle] = @calle
                              ,[noExterior] = @noExterior
                              ,[noInterior] = @noInterior
                              ,[colonia] = @colonia
                              ,[localidad] = @localidad
                              ,[referencia] = @referencia
                              ,[municipio] = @municipio
                              ,[estado] = @estado
                              ,[pais] = @pais
                              ,[codigoPostal] = @codigoPostal
                         WHERE idSucursal = @ID";
            }
            try
            {
                _db.Conectar();
                _db.CrearComando(sql);
                if (!string.IsNullOrEmpty(_idEditar))
                {
                    _db.AsignarParametroCadena("@ID", _idEditar);
                }
                _db.AsignarParametroCadena("@clave", tbClaveNuevo.Text);
                _db.AsignarParametroCadena("@sucursal", tbSucursalNuevo.Text);
                _db.AsignarParametroCadena("@eliminado", "0");
                _db.AsignarParametroCadena("@telefono", tbTelNuevo.Text);
                _db.AsignarParametroCadena("@RFC", tbRfcRecNuevo.Text);
                _db.AsignarParametroCadena("@calle", tbCalleRec.Text);
                _db.AsignarParametroCadena("@noExterior", tbNoExtRec.Text);
                _db.AsignarParametroCadena("@noInterior", tbNoIntRec.Text);
                _db.AsignarParametroCadena("@colonia", tbColoniaRec.Text);
                _db.AsignarParametroCadena("@localidad", tbLocRec.Text);
                _db.AsignarParametroCadena("@referencia", tbRefRec.Text);
                _db.AsignarParametroCadena("@municipio", tbMunicipioRec.Text);
                _db.AsignarParametroCadena("@estado", tbEstadoRec.Text);
                _db.AsignarParametroCadena("@pais", tbPaisRec.Text);
                _db.AsignarParametroCadena("@codigoPostal", tbCpRec.Text);

                dr = _db.EjecutarConsulta();
                if (dr.RecordsAffected > 0)
                {
                    tbRfcRecNuevo.Text = "";
                    tbClaveNuevo.Text = "";
                    tbSucursalNuevo.Text = "";
                    tbTelNuevo.Text = "";
                    tbRfcRec.Text = "";
                    tbCalleRec.Text = "";
                    tbNoExtRec.Text = "";
                    tbNoIntRec.Text = "";
                    tbColoniaRec.Text = "";
                    tbLocRec.Text = "";
                    tbRefRec.Text = "";
                    tbMunicipioRec.Text = "";
                    tbEstadoRec.Text = "";
                    tbPaisRec.Text = "";
                    tbCpRec.Text = "";
                    bActualizar_Click(null, null);
                    (Master as SiteMaster).MostrarAlerta(this, "La sucursal se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');");
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "La sucursal no se pudo agregar/modificar. Intentelo nuevamente", 4, null);
                }
            }
            catch
            {
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
                var sql = @"SELECT * FROM Cat_Sucursales WHERE idSucursal = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", id);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbRfcRecNuevo.Text = dr["RFC"].ToString();
                    tbClaveNuevo.Text = dr["clave"].ToString();
                    tbTelNuevo.Text = dr["telefono"].ToString();
                    tbSucursalNuevo.Text = dr["sucursal"].ToString();
                    tbCalleRec.Text = dr["calle"].ToString();
                    tbNoExtRec.Text = dr["noExterior"].ToString();
                    tbNoIntRec.Text = dr["noInterior"].ToString();
                    tbColoniaRec.Text = dr["colonia"].ToString();
                    tbLocRec.Text = dr["localidad"].ToString();
                    tbRefRec.Text = dr["referencia"].ToString();
                    tbMunicipioRec.Text = dr["municipio"].ToString();
                    tbEstadoRec.Text = dr["estado"].ToString();
                    tbPaisRec.Text = dr["pais"].ToString();
                    tbCpRec.Text = dr["codigoPostal"].ToString();
                }
                _db.Desconectar();
            }
            else
            {
                tbRfcRecNuevo.Text = "";
                tbClaveNuevo.Text = "";
                tbTelNuevo.Text = "";
                tbSucursalNuevo.Text = "";
                tbCalleRec.Text = "";
                tbNoExtRec.Text = "";
                tbNoIntRec.Text = "";
                tbColoniaRec.Text = "";
                tbLocRec.Text = "";
                tbRefRec.Text = "";
                tbMunicipioRec.Text = "";
                tbEstadoRec.Text = "";
                tbPaisRec.Text = "";
                tbCpRec.Text = "";
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
    }
}