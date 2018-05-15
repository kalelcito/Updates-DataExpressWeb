// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 07-02-2017
//
// Last Modified By : Sergio
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="proveedores.aspx.cs" company="DataExpress Latinoamérica">
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
    /// Class Proveedores.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Proveedores : Page
    {
        /// <summary>
        /// The registrado
        /// </summary>
        private static string _Registrado;
        /// <summary>
        /// The identifier editar
        /// </summary>
        private static string _idEditar;
        /// <summary>
        /// The database
        /// </summary>
        private BasesDatos _db;
        /// <summary>
        /// The database r
        /// </summary>
        private BasesDatos _dbR;
        /// <summary>
        /// The schema e
        /// </summary>
        private string schemaE = "";
        /// <summary>
        /// The schema r
        /// </summary>
        private string schemaR = "";

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
                _dbR = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE", "Recepcion");
                schemaE = _db.DatabaseSchema + ".dbo";
                schemaR = _dbR.DatabaseSchema + ".dbo";
                SqlDataSourceEmpleados.ConnectionString = _db.CadenaConexion;
                //SqlDataReceptores.ConnectionString = _dbR.CadenaConexion;
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
            _db.CrearComando("select SUBSTRING(userProveedor,LEN(userProveedor)-3,4) from  Cat_Proveedores WHERE idProveedor= (SELECT MAX(idProveedor) FROM Cat_Proveedores)");
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
            tbUsername.Text = "PROVE" + Localization.Now.ToString("yy") + maxemp;
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
            _dbR.Conectar();
            _dbR.CrearComando(@"SELECT DISTINCT TOP 1 RFCEMI, NOMEMI FROM Cat_Emisor where RFCEMI=@RFCEMI ORDER BY 1 DESC");
            _dbR.AsignarParametroCadena("@RFCEMI", tbRFC.Text);
            var dr = _dbR.EjecutarConsulta();
            var hasRows = false;
            if (dr.Read())
            {
                hasRows = true;
                ruc = dr[0].ToString();
                nomcli = dr[1].ToString();
            }
            _dbR.Desconectar();

            _Registrado = "";

            if (bAgregar.Text != "Modificar")
            {
                _db.Conectar();
                _db.CrearComando(@"SELECT DISTINCT idProveedor FROM Cat_Proveedores where id_Receptor=@RFCProveedor AND eliminado = '0'");
                _db.AsignarParametroCadena("@RFCProveedor", tbRFC.Text);
                var dr3 = _db.EjecutarConsulta();
                if (dr3.Read())
                {
                    _Registrado = dr3[0].ToString();
                }
                _db.Desconectar();
            }
            if (string.IsNullOrEmpty(_Registrado))
            {
                if (!hasRows)
                {
                    try
                    {
                        #region Cat_Emisor
                        var sql = @"INSERT INTO Cat_Emisor
                            (RFCEMI, NOMEMI, dirMatriz, noExterior, noInterior, colonia, localidad, referencia, municipio, estado, pais, codigoPostal)
                            OUTPUT Inserted.IDEEMI
                            VALUES (@RFCEMI,@NOMEMI,@CALLE,@NOEXT,@NOINT,@COL,@LOC,@REF,@MUN,@EST,@PAIS,@CP)";
                        _dbR.Conectar();
                        _dbR.CrearComando(sql);
                        _dbR.AsignarParametroCadena("@RFCEMI", tbRFC.Text);
                        _dbR.AsignarParametroCadena("@NOMEMI", tbNomRec.Text);
                        _dbR.AsignarParametroCadena("@CALLE", tbCalleRec.Text);
                        _dbR.AsignarParametroCadena("@NOEXT", tbNoExtRec.Text);
                        _dbR.AsignarParametroCadena("@NOINT", tbNoIntRec.Text);
                        _dbR.AsignarParametroCadena("@COL", tbColoniaRec.Text);
                        _dbR.AsignarParametroCadena("@LOC", tbLocRec.Text);
                        _dbR.AsignarParametroCadena("@REF", tbRefRec.Text);
                        _dbR.AsignarParametroCadena("@MUN", tbMunicipioRec.Text);
                        _dbR.AsignarParametroCadena("@EST", tbEstadoRec.Text);
                        _dbR.AsignarParametroCadena("@PAIS", tbPaisRec.Text);
                        _dbR.AsignarParametroCadena("@CP", tbCpRec.Text);
                        _dbR.EjecutarConsulta1();
                        ruc = tbRFC.Text;

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _dbR.Desconectar();
                        (Master as SiteMaster).MostrarAlerta(this, "El RFC no ha sido registrado.<br/><br/>" + ex.Message, 4, null, null, "resetPass();");
                        return;
                    }
                    finally
                    {
                        _dbR.Desconectar();
                    }
                }
                else
                {
                    try
                    {
                        #region Cat_Emisor
                        var sql = @"UPDATE Cat_Emisor SET
                            RFCEMI = @RFCEMI, NOMEMI = @NOMEMI, dirMatriz = @CALLE, noExterior = @NOEXT, noInterior = @NOINT, colonia = @COL, localidad = @LOC, referencia = @REF, municipio = @MUN, estado = @EST, pais = @PAIS, codigoPostal = @CP
                            OUTPUT Inserted.IDEEMI WHERE RFCEMI = @RFCCUENTA";
                        _dbR.Conectar();
                        _dbR.CrearComando(sql);
                        _dbR.AsignarParametroCadena("@RFCEMI", tbRFC.Text);
                        _dbR.AsignarParametroCadena("@NOMEMI", tbNomRec.Text);
                        _dbR.AsignarParametroCadena("@CALLE", tbCalleRec.Text);
                        _dbR.AsignarParametroCadena("@NOEXT", tbNoExtRec.Text);
                        _dbR.AsignarParametroCadena("@NOINT", tbNoIntRec.Text);
                        _dbR.AsignarParametroCadena("@COL", tbColoniaRec.Text);
                        _dbR.AsignarParametroCadena("@LOC", tbLocRec.Text);
                        _dbR.AsignarParametroCadena("@REF", tbRefRec.Text);
                        _dbR.AsignarParametroCadena("@MUN", tbMunicipioRec.Text);
                        _dbR.AsignarParametroCadena("@EST", tbEstadoRec.Text);
                        _dbR.AsignarParametroCadena("@PAIS", tbPaisRec.Text);
                        _dbR.AsignarParametroCadena("@CP", tbCpRec.Text);
                        _dbR.AsignarParametroCadena("@RFCCUENTA", ruc);
                        _dbR.EjecutarConsulta1();

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _dbR.Desconectar();
                        (Master as SiteMaster).MostrarAlerta(this, "El RFC no ha sido actualizado.<br/><br/>" + ex.Message, 4, null, null, "resetPass();");
                        return;
                    }
                    finally
                    {
                        _dbR.Desconectar();
                    }
                }
                if (tbUsername.Text.Length > 8)
                {
                    var exist = false;
                    _db.Conectar();
                    _db.CrearComando(@"SELECT DISTINCT idProveedor FROM Cat_Proveedores where userProveedor=@userProveedor AND eliminado = '0'");
                    _db.AsignarParametroCadena("@userProveedor", tbUsername.Text);
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
                                            _db.CrearComando(@"UPDATE Cat_Proveedores SET id_Receptor = @id_Receptor, userProveedor = @userProveedor, claveProveedor = @claveProveedor, nombreProveedor = @nombreProveedor, email = @email, telProveedor = @telProveedor, status = @status WHERE idProveedor = @idProveedor");
                                            _db.AsignarParametroCadena("@idProveedor", _idEditar);
                                            //_db.AsignarParametroCadena("@id_Receptor", ddlRFC.SelectedValue);
                                            _db.AsignarParametroCadena("@id_Receptor", tbRFC.Text);
                                            _db.AsignarParametroCadena("@userProveedor", tbUsername.Text);
                                            _db.AsignarParametroCadena("@claveProveedor", pass1);
                                            _db.AsignarParametroCadena("@nombreProveedor", tbNombre.Text);
                                            _db.AsignarParametroCadena("@email", tbEmail.Text);
                                            _db.AsignarParametroCadena("@telProveedor", tbTel.Text);
                                            _db.AsignarParametroCadena("@status", ddlStatus.SelectedValue);
                                            _db.EjecutarConsulta1();
                                            _db.Desconectar();
                                            if (ddlStatus.SelectedValue.Equals("1"))
                                            {
                                                _db.Conectar();
                                                _db.CrearComando(@"UPDATE Cat_Proveedores SET ActivationToken = NULL WHERE idProveedor = @idProveedor");
                                                _db.AsignarParametroCadena("@idProveedor", _idEditar);
                                                _db.EjecutarConsulta1();
                                                _db.Desconectar();
                                            }
                                            SqlDataSourceEmpleados.DataBind();
                                            gvEmpleados.DataBind();
                                            tbUsername.Text = "";
                                            ddlStatus.SelectedValue = "0";
                                            tbNombre.Text = "";
                                            tbEmail.Text = "";
                                            SetPwds("");
                                            tbRFC.Text = "";
                                            tbTel.Text = "";
                                            (Master as SiteMaster).MostrarAlerta(this, "El Proveedor se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');" + SetPwds("", false));
                                        }
                                        else
                                        {
                                            (Master as SiteMaster).MostrarAlerta(this, "Ya existe un usuario registrado con esta identificación.", 4, null, null, "resetPass(false);");
                                        }
                                    }
                                    else
                                    {
                                        _db.Conectar();
                                        _db.CrearComando(@"INSERT Cat_Proveedores (id_Receptor, userProveedor, claveProveedor, nombreProveedor, email, telProveedor, status, eliminado, ActivationToken) VALUES (@id_Receptor, @userProveedor, @claveProveedor, @nombreProveedor, @email, @telProveedor, @status, 0, NULL)");
                                        //_db.AsignarParametroCadena("@id_Receptor", ddlRFC.SelectedValue);
                                        _db.AsignarParametroCadena("@id_Receptor", tbRFC.Text);
                                        _db.AsignarParametroCadena("@userProveedor", tbUsername.Text);
                                        _db.AsignarParametroCadena("@claveProveedor", pass1);
                                        _db.AsignarParametroCadena("@nombreProveedor", tbNombre.Text);
                                        _db.AsignarParametroCadena("@email", tbEmail.Text);
                                        _db.AsignarParametroCadena("@telProveedor", tbTel.Text);
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
                                        //ddlRFC.SelectedValue = "0";
                                        tbRFC.Text = "";
                                        tbTel.Text = "";
                                        tbNomRec.Text = "";
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
                                        (Master as SiteMaster).MostrarAlerta(this, "El Proveedor se " + (!string.IsNullOrEmpty(_idEditar) ? "modificó" : "agregó") + " correctamente.", 2, null, "$('#divNuevo').modal('hide');" + SetPwds("", false));
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
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El proveedor con RFC " + tbRFC.Text + " ya se encuentra registrado." + (!string.IsNullOrEmpty(_idEditar) ? "" : "") + "", 2, null, "$('#divNuevo').modal('hide');" + SetPwds("", false));
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
                consulta = " and p.nombreProveedor like '%" + tbNomClient.Text + "%' ";
            }
            if (tbIdentificacion.Text.Length != 0)
            {
                consulta = " and p.id_Receptor = '" + tbIdentificacion.Text + "' " + consulta;
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
            var sql = @"SELECT DISTINCT p.userProveedor, p.status, p.nombreProveedor, p.claveProveedor, p.id_Receptor, p.telProveedor, p.email FROM Cat_Proveedores p WHERE p.idProveedor=@id";
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
                    //ddlRFC.SelectedValue = dr[4].ToString();
                    tbRFC.Text = dr[4].ToString();
                    tbTel.Text = dr[5].ToString();
                    tbEmail.Text = dr[6].ToString();
                }
            }
            else
            {
                tbUsername.Text = "";
                ddlStatus.SelectedValue = "0";
                tbNombre.Text = "";
                SetPwds("");
                //ddlRFC.SelectedValue = "0";
                tbRFC.Text = "";
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
            //try
            //{
            //    _db.Conectar();
            //    _db.CrearComando("SELECT ActivationToken FROM Cat_Proveedores WHERE idProveedor = @id");
            //    _db.AsignarParametroCadena("@id", _idEditar);
            //    var dr = _db.EjecutarConsulta();
            //    if (dr.Read())
            //    {
            //        activado = dr[0] is DBNull || string.IsNullOrEmpty(dr[0].ToString());
            //    }
            //}
            //catch { }
            //finally
            //{
            //    _db.Desconectar();
            //}
            rowEditar.Style["display"] = activado ? "inline" : "none";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divNuevo').modal('toggle'); $('#divModPwd').show();" + (activado ? "" : "alertBootBox('El Proveedor no ha verificado aún su cuenta de correo, por lo que no se puede editar', 4);"), true);
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
            //ddlRFC.Enabled = chkEditar.Checked && string.IsNullOrEmpty(_idEditar);
            tbRFC.Enabled = chkEditar.Checked && string.IsNullOrEmpty(_idEditar);
            tbTel.ReadOnly = !chkEditar.Checked;
            Llenarlista(_idEditar);
            LlenarlistaRfc(tbRFC.Text);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "_ddlRFCKey", "$('#divModPwd')." + (!string.IsNullOrEmpty(_idEditar) ? "show" : "hide") + "(); resetPass();", true);
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRFC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tbRFC_TextChanged(object sender, EventArgs e)
        {
            LlenarlistaRfc(tbRFC.Text);
        }

        /// <summary>
        /// Llenarlistas the RFC.
        /// </summary>
        /// <param name="rfc">The RFC.</param>
        private void LlenarlistaRfc(string rfc)
        {
            var sql = "";
            sql = @"SELECT TOP 1 [NOMEMI]
                              ,[dirMatriz]
                              ,[noExterior]
                              ,[noInterior]
                              ,[colonia]
                              ,[localidad]
                              ,[referencia]
                              ,[municipio]
                              ,[estado]
                              ,[pais]
                              ,[codigoPostal]
                          FROM [Cat_Emisor]
                          WHERE RFCEMI=@ruc ORDER BY 1 DESC";
            _dbR.Conectar();
            _dbR.CrearComando(sql);
            _dbR.AsignarParametroCadena("@ruc", rfc);
            var dr = _dbR.EjecutarConsulta();
            var control = !chkEditar.Checked;
            tbNomRec.ReadOnly = control;
            tbCalleRec.ReadOnly = control;
            tbNoExtRec.ReadOnly = control;
            tbNoIntRec.ReadOnly = control;
            tbColoniaRec.ReadOnly = control;
            tbLocRec.ReadOnly = control;
            tbRefRec.ReadOnly = control;
            tbMunicipioRec.ReadOnly = control;
            tbEstadoRec.ReadOnly = control;
            tbPaisRec.ReadOnly = control;
            tbCpRec.ReadOnly = control;
            if (dr.HasRows && dr.Read())
            {
                tbNomRec.Text = dr["NOMEMI"].ToString();
                tbCalleRec.Text = dr["dirMatriz"].ToString();
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
            else
            {
                tbNomRec.Text = "";
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
            _dbR.Desconectar();
        }
    }
}