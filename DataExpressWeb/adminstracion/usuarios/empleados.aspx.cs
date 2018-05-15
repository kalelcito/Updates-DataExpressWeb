// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 07-02-2017
//
// Last Modified By : Sergio
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="empleados.aspx.cs" company="DataExpress Latinoamérica">
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
using System.Linq;
using Control;
using System.Data.Common;

namespace Administracion
{
    /// <summary>
    /// Class Empleados.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Empleados : Page
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
        /// The DB2
        /// </summary>
        private BasesDatos _db2;
        /// <summary>
        /// The DBR
        /// </summary>
        private BasesDatos _dbr;

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
                _db2 = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                _dbr = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE", "Recepcion");

                SqlDataSourceEmpleados.ConnectionString = _db.CadenaConexion;
                SqlDataSourceRol.ConnectionString = _db.CadenaConexion;
                //SqlDataSourceSesion.ConnectionString = _db.CadenaConexion;
                SqlDataSourceSucursal.ConnectionString = _db.CadenaConexion;
                SqlDataSourceModulo.ConnectionString = _db.CadenaConexion;
                SqlDataSourceGrupo.ConnectionString = _dbr.CadenaConexion;

                if (!IsPostBack)
                {
                    _idEditar = "";
                }
                BindGrupos();
            }
        }

        /// <summary>
        /// Handles the Click event of the bActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bActualizar_Click(object sender, EventArgs e)
        {
            tbNombre.Text = "";
            tbUsuario.Text = "";
            Buscar();
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
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
            var consulta = "";

            if (tbNombre.Text.Length != 0)
            {
                consulta = " and nombreEmpleado like '%" + tbNombre.Text + "%' ";
            }
            if (tbUsuario.Text.Length != 0)
            {
                consulta = " and userEmpleado = '" + tbUsuario.Text + "' " + consulta;
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
        /// Handles the CheckedChanged event of the chkEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            tbUsername.ReadOnly = true;
            ddlStatus.Enabled = chkEditar.Checked;
            tbNombre1.ReadOnly = !chkEditar.Checked;
            ddlRol.Enabled = chkEditar.Checked;
            ddlSucursal.Enabled = chkEditar.Checked;
            tbEmail.ReadOnly = !chkEditar.Checked;
            tbContrasena.ReadOnly = !chkEditar.Checked || !string.IsNullOrEmpty(_idEditar);
            tbRepetir.ReadOnly = !chkEditar.Checked || !string.IsNullOrEmpty(_idEditar);
            bAgregar.Visible = chkEditar.Checked;
            cbAsistente.Enabled = chkEditar.Checked;
            cbModificarContrasena.Enabled = chkEditar.Checked;
            //tbRFCEmi.ReadOnly = !chkEditar.Checked;
            ddlModulo.Enabled = chkEditar.Checked;
            ddlGrupo.Enabled = chkEditar.Checked;
            Llenarlista(_idEditar);
            ScriptManager.RegisterStartupScript(this, GetType(), "_chkEditar", "$('#divModPwd')." + (!string.IsNullOrEmpty(_idEditar) ? "show" : "hide") + "();", true);
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
        /// Handles the Click event of the bDetalles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bDetalles_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            chkEditar.Visible = true;
            chkEditar.Checked = false;
            chkEditar_CheckedChanged(null, null);
            bAgregar.Text = "Modificar";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divNuevo').modal('toggle'); $('#divModPwd').show();", true);
        }

        /// <summary>
        /// Llenarlistas the specified identifier user.
        /// </summary>
        /// <param name="idUser">The identifier user.</param>
        private void Llenarlista(string idUser)
        {
            var sql = @"SELECT userEmpleado, status, nombreEmpleado, id_Rol, ISNULL(id_Sucursal, 0) AS id_Sucursal, claveEmpleado, RFCEMI, asistenteSimplificado, Cat_Empleados.email, Cat_Empleados.idGrupo FROM Cat_Empleados LEFT OUTER JOIN Cat_Emisor ON id_Emisor = IDEEMI WHERE idEmpleado=@id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", idUser);
            var dr = _db.EjecutarConsulta();
            if (dr.HasRows)
            {
                ddlRol.DataBind();
                ddlSucursal.DataBind();
                ddlGrupo.DataBind();
                while (dr.Read())
                {
                    tbUsername.Text = dr[0].ToString();
                    ddlStatus.SelectedValue = dr[1].ToString();
                    tbNombre1.Text = dr[2].ToString();
                    ddlRol.SelectedValue = dr[3].ToString();
                    try { ddlSucursal.SelectedValue = dr[4].ToString(); }
                    catch (Exception ex) { ddlSucursal.SelectedValue = "0"; }
                    SetPwds(dr[5].ToString());
                    tbRFCEmi.Text = dr[6].ToString();
                    cbAsistente.Checked = bool.Parse(dr[7].ToString());
                    tbEmail.Text = dr[8].ToString();
                    try { ddlGrupo.SelectedValue = dr[9].ToString(); }
                    catch (Exception ex) { ddlGrupo.SelectedValue = "0"; }
                    rowRfc.Visible = true;
                }
            }
            else
            {
                tbUsername.Text = "";
                ddlStatus.SelectedValue = "0";
                tbNombre1.Text = "";
                ddlRol.SelectedValue = "0";
                ddlSucursal.SelectedValue = "0";
                cbAsistente.Checked = false;
                SetPwds("");
                tbRFCEmi.Text = Session["IDENTEMI"].ToString();
                tbEmail.Text = "";
                rowRfc.Visible = false;
            }
            cbModificarContrasena.Checked = false;
            _db.Desconectar();
            BindModulos();
        }

        /// <summary>
        /// Handles the Click event of the bNuevo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bNuevo_Click(object sender, EventArgs e)
        {
            _idEditar = "";
            chkEditar.Visible = false;
            chkEditar.Checked = true;
            chkEditar_CheckedChanged(null, null);
            User1();
            bAgregar.Text = "Agregar";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('toggle'); resetPass(); $('#divModPwd').hide();", true);
        }

        /// <summary>
        /// Handles the Click event of the bAgregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bAgregar_Click(object sender, EventArgs e)
        {
            var rfc = "";
            var pass1 = hfPass1.Value;
            var pass2 = hfPass2.Value;
            _db.Conectar();
            _db.CrearComando(@"SELECT RFCEMI from Cat_Emisor where RFCEMI=@RFC");
            _db.AsignarParametroCadena("@RFC", tbRFCEmi.Text);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                rfc = dr[0].ToString();
            }
            _db.Desconectar();
            if (string.IsNullOrEmpty(_idEditar))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_hidePwdKey", "$('#divModPwd').hide();", true);
            }
            if (ddlRol.SelectedValue != "0" && !string.IsNullOrEmpty(rfc))
            {
                if (string.IsNullOrEmpty(tbNombre1.Text))
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El campo Nombre esta vacio.", 4, null);
                }
                else
                {
                    if (tbUsername.Text.Length > 8)
                    {
                        if (string.IsNullOrEmpty(pass1) || string.IsNullOrEmpty(pass2))
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "Campos de contraseña incompletos.", 4, null);
                        }
                        else
                        {
                            if (!pass1.Equals(pass2))
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Los campos contraseña no coinciden.", 4, null);
                            }
                            else
                            {
                                //if (ddlSucursal.SelectedValue.Equals("0"))
                                //{
                                //    (Master as SiteMaster).MostrarAlerta(this, "Debe escoger una Sucursal.", 4, null);
                                //}
                                //else
                                //{
                                if (!ddlSucursal.SelectedValue.Equals("0"))
                                {
                                    _db.Conectar();
                                    _db.CrearComando(@"SELECT RFC from Cat_SucursalesEmisor where idSucursal=@idSucursal");
                                    _db.AsignarParametroCadena("@idSucursal", ddlSucursal.SelectedValue);
                                    dr = _db.EjecutarConsulta();
                                    if (dr.Read())
                                    {
                                        rfc = dr[0].ToString();
                                    }
                                    _db.Desconectar();
                                }
                                if (string.IsNullOrEmpty(_idEditar))
                                {
                                    _db.Conectar();
                                    _db.CrearComando("SELECT idEmpleado FROM Cat_Empleados WHERE userEmpleado = @user");
                                    _db.AsignarParametroCadena("@user", tbUsername.Text);
                                    dr = _db.EjecutarConsulta();
                                    if (dr.Read())
                                    {
                                        _db.Desconectar();
                                        (Master as SiteMaster).MostrarAlerta(this, "El nombre de usuario ya esta registrado.", 4, null);
                                        return;
                                    }
                                    _db.Desconectar();
                                    _db.Conectar();
                                    _db.CrearComandoProcedimiento("PA_insertar_empleados");
                                    _db.AsignarParametroProcedimiento("@nombreEmpleado", DbType.String, tbNombre1.Text);
                                    _db.AsignarParametroProcedimiento("@userEmpleado", DbType.String, tbUsername.Text);
                                    _db.AsignarParametroProcedimiento("@claveEmpleado", DbType.String, pass1);
                                    _db.AsignarParametroProcedimiento("@status", DbType.String, ddlStatus.SelectedValue);
                                    _db.AsignarParametroProcedimiento("@RFC", DbType.String, Session["IDENTEMI"].ToString());
                                    _db.AsignarParametroProcedimiento("@id_Rol", DbType.Int16, ddlRol.SelectedValue);
                                    _db.AsignarParametroProcedimiento("@id_Sesion", DbType.Int16, 1);
                                    _db.AsignarParametroProcedimiento("@id_Sucursal", DbType.Int16, ddlSucursal.SelectedValue);
                                    _db.AsignarParametroProcedimiento("@ptoEmi", DbType.String, "001");
                                    _db.AsignarParametroProcedimiento("@asistenteSimplificado", DbType.Boolean, cbAsistente.Checked);
                                    _db.AsignarParametroProcedimiento("@idGrupo", DbType.Int16, DivGrupos.Visible ? ddlGrupo.SelectedValue : "0");
                                    _db.AsignarParametroProcedimiento("@email", DbType.String, tbEmail.Text);
                                }
                                else
                                {
                                    _db.Conectar();
                                    _db.CrearComando("SELECT idEmpleado FROM Cat_Empleados WHERE idEmpleado <> @id AND userEmpleado = @user");
                                    _db.AsignarParametroCadena("@id", _idEditar);
                                    _db.AsignarParametroCadena("@user", tbUsername.Text);
                                    dr = _db.EjecutarConsulta();
                                    if (dr.Read())
                                    {
                                        _db.Desconectar();
                                        (Master as SiteMaster).MostrarAlerta(this, "El nombre de usuario ya está registrado.", 4, null);
                                        return;
                                    }
                                    _db.Desconectar();
                                    _db.Conectar();
                                    _db.CrearComandoProcedimiento("PA_modificar_empleado");
                                    _db.AsignarParametroProcedimiento("@idEmpleado", DbType.String, _idEditar);
                                    _db.AsignarParametroProcedimiento("@nombreEmpleado", DbType.String, tbNombre1.Text);
                                    _db.AsignarParametroProcedimiento("@userEmpleado", DbType.String, tbUsername.Text);
                                    _db.AsignarParametroProcedimiento("@claveEmpleado", DbType.String, pass1);
                                    _db.AsignarParametroProcedimiento("@id_Rol", DbType.Int16, ddlRol.SelectedValue);
                                    _db.AsignarParametroProcedimiento("@status", DbType.String, ddlStatus.SelectedValue);
                                    _db.AsignarParametroProcedimiento("@id_Sesion", DbType.Int16, 1);
                                    _db.AsignarParametroProcedimiento("@id_Sucursal", DbType.Int16, ddlSucursal.SelectedValue);
                                    _db.AsignarParametroProcedimiento("@ptoEmi", DbType.Int16, "001");
                                    _db.AsignarParametroProcedimiento("@asistenteSimplificado", DbType.Boolean, cbAsistente.Checked);
                                    _db.AsignarParametroProcedimiento("@idGrupo", DbType.Int16, DivGrupos.Visible ? ddlGrupo.SelectedValue : "0");
                                    _db.AsignarParametroProcedimiento("@email", DbType.String, tbEmail.Text);
                                    //_db.AsignarParametroProcedimiento("@RFC", DbType.String, rfc);
                                }
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                                _db.Conectar();
                                _db.CrearComando(@"DELETE m FROM Cat_ModuloEmpleado m INNER JOIN Cat_Empleados e ON e.idEmpleado = m.idEmpleado WHERE e.userEmpleado = @USER");
                                _db.AsignarParametroCadena("@USER", tbUsername.Text);
                                _db.EjecutarConsulta1();
                                _db.Desconectar();
                                var selected = (from ListItem item in ddlModulo.Items.OfType<ListItem>() where item.Selected select item.Value);
                                foreach (var serie in selected)
                                {
                                    try
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"INSERT Cat_ModuloEmpleado(idEmpleado, idSerie) VALUES ((SELECT idEmpleado FROM Cat_Empleados WHERE userEmpleado = @USER),@IdSerie)");
                                        _db.AsignarParametroCadena("@USER", tbUsername.Text);
                                        _db.AsignarParametroCadena("@IdSerie", serie);
                                        _db.EjecutarConsulta1();
                                        _db.Desconectar();
                                    }
                                    catch (Exception ex)
                                    {
                                        var idUser = "";
                                        _db.Conectar();
                                        _db.CrearComando(@"SELECT idEmpleado FROM Cat_Empleados WHERE userEmpleado = @USER");
                                        _db.AsignarParametroCadena("@USER", tbUsername.Text);
                                        dr = _db.EjecutarConsulta();
                                        if (dr.Read())
                                        {
                                            idUser = dr[0].ToString();
                                        }
                                        _db.Desconectar();
                                        if (!string.IsNullOrEmpty(idUser))
                                        {
                                            _db.Conectar();
                                            _db.CrearComando(@"INSERT Cat_ModuloEmpleado(idEmpleado, idSerie) VALUES (@IdUser,@IdSerie)");
                                            _db.AsignarParametroCadena("@IdUser", idUser);
                                            _db.AsignarParametroCadena("@IdSerie", serie);
                                            _db.EjecutarConsulta1();
                                            _db.Desconectar();
                                        }
                                    }
                                }
                                tbUsername.Text = "";
                                ddlStatus.SelectedValue = "0";
                                tbNombre1.Text = "";
                                tbEmail.Text = "";
                                ddlRol.SelectedValue = "0";
                                ddlSucursal.SelectedValue = "0";
                                cbAsistente.Checked = false;
                                cbModificarContrasena.Checked = false;
                                tbRFCEmi.Text = "";
                                BindModulos();
                                SqlDataSourceEmpleados.DataBind();
                                gvEmpleados.DataBind();
                                (Master as SiteMaster).MostrarAlerta(this, "El empleado se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');" + SetPwds("", false));
                                _idEditar = "";
                                //}
                            }
                        }
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El nombre de usuario es muy corto, la longitud minima es de 8 caracteres.", 4, null);
                    }
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "Selecciona un tipo de Empleado o no existe el RFC.", 4, null);
            }
        }

        /// <summary>
        /// User1s this instance.
        /// </summary>
        private void User1()
        {
            var maxemp = "";
            var aux = 0;
            ddlRol.Visible = true;
            if (!IsPostBack)
            {
                SqlDataSourceRol.DataBind();
                ddlRol.DataBind();
            }
            _db.Conectar();
            _db.CrearComando("select SUBSTRING(userEmpleado,LEN(userEmpleado)-3,4) from  Cat_EMPLEADOS WHERE idEmpleado= (SELECT MAX(idEmpleado) FROM Cat_EMPLEADOS)");
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
            ddlRol.Visible = true;
            ddlSucursal.Visible = true;
            tbUsername.Text = "EMPLE" + Localization.Now.ToString("yy") + maxemp;
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
        /// Handles the SelectedIndexChanged event of the ddlRol control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindModulos();
            ScriptManager.RegisterStartupScript(this, GetType(), "_chkEditar", "$('#divModPwd')." + (!string.IsNullOrEmpty(_idEditar) ? "show" : "hide") + "();", true);
            BindGrupos();

        }

        /// <summary>
        /// Binds the modulos.
        /// </summary>
        private void BindModulos()
        {
            SqlDataSourceModulo.DataBind();
            ddlModulo.DataBind();
            if (!string.IsNullOrEmpty(_idEditar))
            {
                var sql = @"SELECT idSerie FROM Cat_ModuloEmpleado WHERE idEmpleado = @ID";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@ID", _idEditar);
                var dr = _db.EjecutarConsulta();
                while (dr.Read())
                {
                    try
                    {
                        ddlModulo.Items.FindByValue(dr["idSerie"].ToString()).Selected = true;
                    }
                    catch (Exception)
                    {

                    }
                }
                _db.Desconectar();
            }
            else
            {
                foreach (ListItem item in ddlModulo.Items)
                {
                    item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Binds the grupos.
        /// </summary>
        private void BindGrupos()
        {
            string validacion = "False";
            var sql = "SELECT ISNULL(validacionRecepcion,'False') FROM Cat_Roles WHERE idRol=@idRol";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idRol", ddlRol.SelectedValue == "" ? "0" : ddlRol.SelectedValue);
            var DR = _db.EjecutarConsulta();
            while (DR.Read())
            {
                validacion = DR[0].ToString();
            }
            _db.Desconectar();

            DivGrupos.Visible = Convert.ToBoolean(validacion);

        }

    }
}