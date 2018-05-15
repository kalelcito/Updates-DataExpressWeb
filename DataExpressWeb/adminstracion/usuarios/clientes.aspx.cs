// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 01-30-2017
//
// Last Modified By : Sergio
// Last Modified On : 01-30-2017
// ***********************************************************************
// <copyright file="clientes.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataExpressWeb;
using Datos;
using Control;

namespace Administracion
{
    /// <summary>
    /// Class Clientes.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Clientes : Page
    {
        /// <summary>
        /// The identifier editar
        /// </summary>
        private static string _idEditar;
        /// <summary>
        /// The database
        /// </summary>
        private BasesDatos _db;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                SqlDataSourceEmpleados.ConnectionString = _db.CadenaConexion;
                SqlDataReceptores.ConnectionString = _db.CadenaConexion;
                if (!IsPostBack)
                {
                    _idEditar = "";
                }
            }
        }

        /// <summary>
        /// User1s this instance.
        /// </summary>
        private void User1()
        {
            var maxemp = "";
            var aux = 0;
            _db.Conectar();
            _db.CrearComando("select SUBSTRING(userCliente,LEN(userCliente)-3,4) from  Cat_Clientes WHERE idCliente= (SELECT MAX(idCliente) FROM Cat_Clientes)");
            var dr1 = _db.EjecutarConsulta();
            if (dr1.Read())
            {
                if (int.TryParse(dr1[0].ToString(), out aux))
                {
                    aux = Convert.ToInt32(dr1[0]) + 1;
                }
            }
            _db.Desconectar();
            if (aux.ToString().Length == 1)
            {
                maxemp = "000" + aux;
            }
            if (aux.ToString().Length == 2)
            {
                maxemp = "00" + aux;
            }
            if (aux.ToString().Length == 3)
            {
                maxemp = "0" + aux;
            }
            if (aux.ToString().Length == 4)
            {
                maxemp = aux.ToString();
            }
            tbUsername.Text = "CLIEN" + Localization.Now.ToString("yy") + maxemp;
        }

        /// <summary>
        /// Handles the Click event of the bBuscarReg control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Handles the Click event of the bActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bActualizar_Click(object sender, EventArgs e)
        {
            tbNomClient.Text = "";
            tbIdentificacion.Text = "";
            Buscar();
        }

        /// <summary>
        /// Handles the Click event of the bAgregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bAgregar_Click(object sender, EventArgs e)
        {
            var pass1 = hfPass1.Value;
            var pass2 = hfPass2.Value;
            var ruc = "";
            var nomcli = "";
            _db.Conectar();
            _db.CrearComando(@"SELECT RFCREC, NOMREC FROM Cat_Receptor where IDEREC=@IDEREC");
            _db.AsignarParametroCadena("@IDEREC", ddlRFC.SelectedValue);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                ruc = dr[0].ToString();
                nomcli = dr[1].ToString();
            }
            _db.Desconectar();
            if (tbUsername.Text.Length > 8)
            {
                var exist = false;
                _db.Conectar();
                _db.CrearComando(@"SELECT idCliente FROM Cat_Clientes where userCliente=@userCliente");
                _db.AsignarParametroCadena("@userCliente", tbUsername.Text);
                var dr2 = _db.EjecutarConsulta();
                exist = dr2.Read();
                _db.Desconectar();
                if (!string.IsNullOrEmpty(ruc))
                {
                    if (string.IsNullOrEmpty(tbNombre.Text))
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El campo Nombre esta vacio.", 4, null, null, "resetPass(false);");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(pass1) || string.IsNullOrEmpty(pass2))
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "Campos de contraseña incompletos.", 4, null, null, "resetPass(false);");
                        }
                        else
                        {
                            if (!pass1.Equals(pass2))
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Los campos contraseña no coinciden.", 4, null, null, "resetPass(false);");
                            }
                            else
                            {
                                if (exist)
                                {
                                    if (!string.IsNullOrEmpty(_idEditar))
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"UPDATE Cat_Clientes SET id_Receptor = @id_Receptor, userCliente = @userCliente, claveCliente = @claveCliente, nombreCliente = @nombreCliente, email = @email, telCliente = @telCliente, status = @status WHERE idCliente = @idCliente");
                                        _db.AsignarParametroCadena("@idCliente", _idEditar);
                                        _db.AsignarParametroCadena("@id_Receptor", ddlRFC.SelectedValue);
                                        _db.AsignarParametroCadena("@userCliente", tbUsername.Text);
                                        _db.AsignarParametroCadena("@claveCliente", pass1);
                                        _db.AsignarParametroCadena("@nombreCliente", tbNombre.Text);
                                        _db.AsignarParametroCadena("@email", tbEmail.Text);
                                        _db.AsignarParametroCadena("@telCliente", tbTel.Text);
                                        _db.AsignarParametroCadena("@status", ddlStatus.SelectedValue);
                                        _db.EjecutarConsulta1();
                                        _db.Desconectar();
                                        SqlDataSourceEmpleados.DataBind();
                                        gvEmpleados.DataBind();
                                        tbUsername.Text = "";
                                        ddlStatus.SelectedValue = "0";
                                        tbNombre.Text = "";
                                        tbEmail.Text = "";
                                        SetPwds("");
                                        ddlRFC.SelectedValue = "0";
                                        tbTel.Text = "";
                                        (Master as SiteMaster).MostrarAlerta(this, "El cliente se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');" + SetPwds("", false));
                                    }
                                    else
                                    {
                                        (Master as SiteMaster).MostrarAlerta(this, "Ya existe un usuario registrado con esta identificación.", 4, null, null, "resetPass(false);");
                                    }
                                }
                                else
                                {
                                    _db.Conectar();
                                    _db.CrearComando(@"INSERT Cat_Clientes (id_Receptor, userCliente, claveCliente, nombreCliente, email, telCliente, status, eliminado, ActivationToken) VALUES (@id_Receptor, @userCliente, @claveCliente, @nombreCliente, @email, @telCliente, @status, 0, NULL)");
                                    _db.AsignarParametroCadena("@id_Receptor", ddlRFC.SelectedValue);
                                    _db.AsignarParametroCadena("@userCliente", tbUsername.Text);
                                    _db.AsignarParametroCadena("@claveCliente", pass1);
                                    _db.AsignarParametroCadena("@nombreCliente", tbNombre.Text);
                                    _db.AsignarParametroCadena("@email", tbEmail.Text);
                                    _db.AsignarParametroCadena("@telCliente", tbTel.Text);
                                    _db.AsignarParametroCadena("@status", ddlStatus.SelectedValue);
                                    _db.EjecutarConsulta1();
                                    _db.Desconectar();
                                    SqlDataSourceEmpleados.DataBind();
                                    gvEmpleados.DataBind();
                                    tbUsername.Text = "";
                                    ddlStatus.SelectedValue = "0";
                                    tbNombre.Text = "";
                                    tbEmail.Text = "";
                                    SetPwds("");
                                    ddlRFC.SelectedValue = "0";
                                    tbTel.Text = "";
                                    (Master as SiteMaster).MostrarAlerta(this, "El cliente se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');" + SetPwds("", false));
                                }
                            }
                        }
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El RFC no ha sido registrado.", 4, null, null, "resetPass();");
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El nombre de usuario es muy corto, la longitud minima es de 8 caracteres.", 4, null, null, "resetPass(false);");
            }
        }

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
            var consulta = "";

            if (tbNomClient.Text.Length != 0)
            {
                consulta = " and nombreCliente like '%" + tbNomClient.Text + "%' ";
            }
            if (tbIdentificacion.Text.Length != 0)
            {
                consulta = " and id_Receptor = (select TOP 1 IDEREC from Cat_RECEPTOR where RFCREC='" + tbIdentificacion.Text + "') " + consulta;
            }
            if (consulta.Length > 0)
            {
                SqlDataSourceEmpleados.SelectCommand = SqlDataSourceEmpleados.SelectCommand + consulta;
            }
            else
            {
                consulta = "";
            }

            SqlDataSourceEmpleados.DataBind();
            gvEmpleados.DataBind();
            consulta = "";
        }

        /// <summary>
        /// Sets the PWDS.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="exec">if set to <c>true</c> [execute].</param>
        /// <returns>System.String.</returns>
        private string SetPwds(string val, bool exec = true)
        {
            tbContrasena.Text = val;
            tbRepetir.Text = val;
            var js = "setPwdVal(1, \"" + val + "\"); setPwdVal(2, \"" + val + "\");";
            if (exec)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_setPwds", js, true);
            }
            return js;
        }

        /// <summary>
        /// Llenarlistas the specified identifier user.
        /// </summary>
        /// <param name="idUser">The identifier user.</param>
        private void Llenarlista(string idUser)
        {
            var sql = @"SELECT userCliente, status, nombreCliente, claveCliente, id_Receptor, NOMREC, telCliente, Cat_Clientes.email FROM Cat_Clientes INNER JOIN Cat_Receptor ON IDEREC = id_Receptor WHERE idCliente=@id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", idUser);
            var dr = _db.EjecutarConsulta();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tbUsername.Text = dr[0].ToString();
                    ddlStatus.SelectedValue = Convert.ToInt32(bool.Parse(dr[1].ToString())).ToString();
                    tbNombre.Text = dr[2].ToString();
                    SetPwds(dr[3].ToString());
                    ddlRFC.SelectedValue = dr[4].ToString();
                    tbRazonSocial.Text = dr[5].ToString();
                    tbTel.Text = dr[6].ToString();
                    tbEmail.Text = dr[7].ToString();
                }
            }
            else
            {
                tbUsername.Text = "";
                ddlStatus.SelectedValue = "0";
                tbNombre.Text = "";
                SetPwds("");
                ddlRFC.SelectedValue = "0";
                tbRazonSocial.Text = "";
                tbEmail.Text = "";
                tbTel.Text = "";
            }
            cbModificarContrasena.Checked = false;
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the Click event of the bDetalles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bDetalles_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            rowEditar.Visible = true;
            chkEditar.Checked = false;
            chkEditar_CheckedChanged(null, null);
            bAgregar.Text = "Modificar";
            var activado = true;
            try
            {
                _db.Conectar();
                _db.CrearComando("SELECT ActivationToken FROM Cat_Clientes WHERE idCliente = @id");
                _db.AsignarParametroCadena("@id", _idEditar);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    activado = dr[0] is DBNull || string.IsNullOrEmpty(dr[0].ToString());
                }
            }
            catch { }
            finally
            {
                _db.Desconectar();
            }
            rowEditar.Style["display"] = activado ? "inline" : "none";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divNuevo').modal('toggle'); $('#divModPwd').show();" + (activado ? "" : "alertBootBox('El cliente no ha verificado aún su cuenta de correo, por lo que no se puede editar', 4);"), true);
        }

        /// <summary>
        /// Handles the Click event of the bNuevo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bNuevo_Click(object sender, EventArgs e)
        {
            _idEditar = "";
            rowEditar.Visible = false;
            chkEditar.Checked = true;
            chkEditar_CheckedChanged(null, null);
            User1();
            bAgregar.Text = "Agregar";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('toggle'); resetPass(); $('#divModPwd').hide();", true);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            tbUsername.ReadOnly = true;
            ddlStatus.Enabled = chkEditar.Checked;
            tbNombre.ReadOnly = !chkEditar.Checked;
            tbContrasena.ReadOnly = !chkEditar.Checked || !string.IsNullOrEmpty(_idEditar);
            tbRepetir.ReadOnly = !chkEditar.Checked || !string.IsNullOrEmpty(_idEditar);
            bAgregar.Visible = chkEditar.Checked;
            tbEmail.ReadOnly = !chkEditar.Checked;
            cbModificarContrasena.Enabled = chkEditar.Checked;
            ddlRFC.Enabled = chkEditar.Checked && string.IsNullOrEmpty(_idEditar);
            tbTel.ReadOnly = !chkEditar.Checked;
            Llenarlista(_idEditar);
            ScriptManager.RegisterStartupScript(this, GetType(), "_chkEditar", "$('#divModPwd')." + (!string.IsNullOrEmpty(_idEditar) ? "show" : "hide") + "();", true);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbModificarContrasena control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void cbModificarContrasena_CheckedChanged(object sender, EventArgs e)
        {
            tbContrasena.ReadOnly = !cbModificarContrasena.Checked;
            tbRepetir.ReadOnly = !cbModificarContrasena.Checked;
            if (!cbModificarContrasena.Checked)
            {
                var sql = @"SELECT claveEmpleado FROM Cat_Empleados WHERE idEmpleado=@id";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@id", _idEditar);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    SetPwds(dr[0].ToString());
                }
                else
                {
                    SetPwds("");
                }
                _db.Desconectar();
            }
            else
            {
                SetPwds("");
            }
        }

        /// <summary>
        /// Handles the Sorting event of the gvEmpleados control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvEmpleados_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["ordenar"].ToString() == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
            }
            gvEmpleados.Sort(e.SortExpression, e.SortDirection);
            Buscar();
            gvEmpleados.DataBind();
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

        /// <summary>
        /// Handles the PageIndexChanging event of the gvEmpleados control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvEmpleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Buscar();
            gvEmpleados.PageIndex = e.NewPageIndex;
            gvEmpleados.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlRFC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlRFC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlRFC.SelectedValue.Equals("0"))
            {
                _db.Conectar();
                _db.CrearComando("SELECT NOMREC FROM Cat_Receptor WHERE IDEREC=@ID");
                _db.AsignarParametroCadena("@ID", ddlRFC.SelectedValue);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    tbRazonSocial.Text = dr[0].ToString();
                }
                _db.Desconectar();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "_ddlRFCKey", "$('#divModPwd')." + (!string.IsNullOrEmpty(_idEditar) ? "show" : "hide") + "(); resetPass();", true);
        }
    }
}