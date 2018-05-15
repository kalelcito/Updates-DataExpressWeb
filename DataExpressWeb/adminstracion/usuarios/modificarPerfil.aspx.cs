// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 12-10-2016
//
// Last Modified By : Sergio
// Last Modified On : 12-10-2016
// ***********************************************************************
// <copyright file="modificarPerfil.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Web.UI;
using Datos;
using DataExpressWeb;

namespace Administracion
{
    /// <summary>
    /// Class ModificarPerfil.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class ModificarPerfil : Page
    {
        /// <summary>
        /// The database
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The identifier user
        /// </summary>
        private static string _idUser;
        /// <summary>
        /// The clave user
        /// </summary>
        private static string _claveUser;
        /// <summary>
        /// The is cliente
        /// </summary>
        private static bool _isCliente;
        /// <summary>
        /// The is proveedor
        /// </summary>
        private static bool _isProveedor;
        /// <summary>
        /// The is empleado
        /// </summary>
        private static bool _isEmpleado;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] == null)
            {
                Response.Redirect("~/Cerrar.aspx", true);
            }
            _db = new BasesDatos(Session["IDENTEMI"].ToString());
            if (!Page.IsPostBack)
            {
                _idUser = Session["idUser"].ToString();
                _isCliente = Session["IsCliente"] != null && Convert.ToBoolean(Session["IsCliente"]);
                _isProveedor = Session["IsProveedor"] != null && Convert.ToBoolean(Session["IsProveedor"]);
                _isEmpleado = !_isCliente && !_isProveedor;
                Clean();
            }
        }

        /// <summary>
        /// Handles the Click event of the bModificar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bModificar_Click(object sender, EventArgs e)
        {
            if (cbCambiarPass.Checked && !(_claveUser.Equals(CurrentPassword.Text)))
            {
                (Master as SiteMaster).MostrarAlerta(this, "La contraseña actual es incorrecta", 4, null, "resetPass();");
            }
            else
            {
                var sql = "";
                try
                {
                    if (_isEmpleado)
                    {
                        sql = "UPDATE Cat_Empleados SET nombreEmpleado=@nombre" + (cbCambiarPass.Checked ? ", claveEmpleado=@claveUser" : "") + " WHERE idEmpleado = @idUser";
                    }
                    else if (_isCliente)
                    {
                        sql = "UPDATE Cat_Clientes SET nombreCliente=@nombre" + (cbCambiarPass.Checked ? ", claveCliente=@claveUser" : "") + " WHERE idCliente = @idUser";
                    }
                    else if (_isProveedor)
                    {
                        sql = "UPDATE Cat_Proveedores SET nombreProveedor=@nombre" + (cbCambiarPass.Checked ? ", claveProveedor=@claveUser" : "") + " WHERE idProveedor = @idUser";
                    }
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@idUser", _idUser);
                    _db.AsignarParametroCadena("@nombre", tbNombre.Text);
                    if (cbCambiarPass.Checked)
                    {
                        _db.AsignarParametroCadena("@claveUser", NewPassword.Text);
                    }
                    _db.EjecutarConsulta();
                    Session["nombreEmpleado"] = tbNombre.Text;
                    (Master as SiteMaster).MostrarAlerta(this, "La cuenta se modificó correctamente", 2, null, "resetPass();");
                }
                catch (Exception ex)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "No se pudo modificar la cuenta, inténtelo nuevamente.<br/><br/>" + ex.Message, 4, null, "resetPass();");
                }
                finally
                {
                    Clean();
                }
            }
        }

        /// <summary>
        /// Llenars the lista.
        /// </summary>
        private void LlenarLista()
        {
            var sql = "";
            if (_isEmpleado)
            {
                sql = "SELECT e.nombreEmpleado AS nombre, e.userEmpleado AS codigoUser, r.descripcion AS rol, '' AS email, (CASE e.status WHEN 1 THEN 'ACTIVO' WHEN 0 THEN 'INACTIVO' END) AS status, s.sucursal, e.asistenteSimplificado, e.claveEmpleado AS claveUser FROM Cat_Empleados e INNER JOIN Cat_Roles r ON e.id_Rol = r.idRol LEFT OUTER JOIN Cat_SucursalesEmisor s ON e.id_Sucursal = s.idSucursal WHERE e.idEmpleado = @idUser";
            }
            else if (_isCliente)
            {
                sql = "SELECT c.nombreCliente AS nombre, c.userCliente AS codigoUser, 'CLIENTES' AS rol, c.email AS email, (CASE c.status WHEN 1 THEN 'ACTIVO' WHEN 0 THEN 'INACTIVO' END) AS status, '' AS sucursal, 'false' AS asistenteSimplificado, c.claveCliente AS claveUser FROM Cat_Clientes c WHERE c.idCliente = @idUser";
            }
            else if (_isProveedor)
            {
                sql = "SELECT p.nombreProveedor AS nombre, p.userProveedor AS codigoUser, 'PROVEEDORES' AS rol, p.email AS email, (CASE p.status WHEN 1 THEN 'ACTIVO' WHEN 0 THEN 'INACTIVO' END) AS status, '' AS sucursal, 'false' AS asistenteSimplificado, p.claveProveedor AS claveUser FROM Cat_Proveedores p WHERE p.idProveedor = @idUser";
            }
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idUser", _idUser);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                tbNombre.Text = dr["nombre"].ToString();
                tbUsername.Text = dr["codigoUser"].ToString();
                tbRol.Text = dr["rol"].ToString();
                tbEmail.Text = dr["email"].ToString();
                tbStatus.Text = dr["status"].ToString();
                tbSucursal.Text = dr["sucursal"].ToString();
                //cbFacturaRestaurantes.Checked = Convert.ToBoolean(dr["asistenteSimplificado"]);
                _claveUser = dr["claveUser"].ToString();
            }
            _db.Desconectar();
            lRol.Visible = _isEmpleado;
            lMail.Visible = !_isEmpleado;
            tbRol.Visible = _isEmpleado;
            tbEmail.Visible = !_isEmpleado;
            lSucursal.Visible = _isEmpleado;
            tbSucursal.Visible = _isEmpleado;
            //cbFacturaRestaurantes.Visible = _isEmpleado;
            //lFacturaRestaurantes.Visible = _isEmpleado;
        }

        /// <summary>
        /// Cleans this instance.
        /// </summary>
        private void Clean()
        {
            LlenarLista();
            cbCambiarPass.Checked = false;
            cbCambiarPass_CheckedChanged(null, null);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the cbCambiarPass control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void cbCambiarPass_CheckedChanged(object sender, EventArgs e)
        {
            CurrentPassword.Text = "";
            NewPassword.Text = "";
            ConfirmNewPassword.Text = "";
            CurrentPassword_RequiredFieldValidator.Enabled = cbCambiarPass.Checked;
            NewPassword_RequiredFieldValidator.Enabled = cbCambiarPass.Checked;
            ConfirmNewPassword_RequiredFieldValidator.Enabled = cbCambiarPass.Checked;
            ConfirmNewPassword_CompareValidator.Enabled = cbCambiarPass.Checked;
            divCambiarPass.Style["display"] = cbCambiarPass.Checked ? "inline" : "none";
        }
    }
}