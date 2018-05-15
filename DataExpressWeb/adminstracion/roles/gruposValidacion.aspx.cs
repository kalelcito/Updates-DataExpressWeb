// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 07-02-2017
//
// Last Modified By : Sergio
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="gruposValidacion.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;
using Datos;
using System.Web.UI.WebControls;
using System.Data;

namespace DataExpressWeb.adminstracion.roles
{
    /// <summary>
    /// Class gruposValidacion.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class gruposValidacion : Page
    {
        /// <summary>
        /// The identifier editar
        /// </summary>
        private static string _idEditar;
        /// <summary>
        /// The identifier editar u
        /// </summary>
        private static string _idEditarU;

        /// <summary>
        /// The log
        /// </summary>
        private Control.Log _log;
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
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
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
                _log = new Control.Log(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");

                if (Session["idUser"] != null)
                {
                    SqlDataTiposProveedor.ConnectionString = _dbr.CadenaConexion;
                    SqlDataSourceTProveedor.ConnectionString = _dbr.CadenaConexion;
                    SqlDataSourceGValidation.ConnectionString = _dbr.CadenaConexion;
                    SqlDataSourceGroupValidate.ConnectionString = _dbr.CadenaConexion;
                    SqlDataSourceEstructura.ConnectionString = _dbr.CadenaConexion;
                    _idUser = Session["idUser"].ToString();
                }
            }
        }

        /// <summary>
        /// Regs the log.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <param name="metodoActual">The metodo actual.</param>
        /// <param name="mensajeTecnico">The mensaje tecnico.</param>
        private void RegLog(string mensaje, string metodoActual, string mensajeTecnico = "")
        {
            _log.Registrar(mensaje, GetType().Name, metodoActual, null, mensajeTecnico, null, null, _idUser, Session["IDENTEMI"].ToString());
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTiposProveedor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlTiposProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNuevoTipoProveedor.Text = "";
            if (ddlTiposProveedor.SelectedValue.Equals("00"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_ddlTiposProveedor", "$('#divNuevoTipoProveedor').modal('show');", true);
            }
            if (!ddlTiposProveedor.SelectedValue.Equals("00") && !ddlTiposProveedor.SelectedValue.Equals("0"))
            {
                tbProveedor.Text = ddlTiposProveedor.SelectedValue;
                ScriptManager.RegisterStartupScript(this, GetType(), "_ddlTiposProveedor", "$('#divEstructureProveedor').modal('show');", true);
            }
            llenarProveedor(ddlTiposProveedor.SelectedValue.ToString());
            // DropDownList1.DataBind();
            tbOrden.Text = "";
        }

        /// <summary>
        /// Handles the Click event of the lbAddTipoProveedor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAddTipoProveedor_Click(object sender, EventArgs e)
        {
            (Master as SiteMaster).MostrarAlerta(this, "El tipo de proveedor \"" + tbNuevoTipoProveedor.Text + "\" se ha agregado correctamente", 2, null, "$('#divNuevoTipoProveedor').hide();$('body').removeClass('modal-open');$('.modal-backdrop').remove();", null);
            _dbr.Conectar();
            _dbr.CrearComando(@"INSERT Cat_TiposProveedor (nombre, visible, eliminado) VALUES (@nombre, @visible, @eliminado)");
            _dbr.AsignarParametroCadena("@nombre", tbNuevoTipoProveedor.Text);
            _dbr.AsignarParametroCadena("@visible", "1");
            _dbr.AsignarParametroCadena("@eliminado", "0");
            _dbr.EjecutarConsulta1();
            _dbr.Desconectar();
            gvTiposProveedor.DataBind();
            ddlTiposProveedor.DataBind();
            ddlTiposProveedor.SelectedValue = "0";
            ddlTiposProveedor_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Handles the Click event of the lbAddEstructure control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAddEstructure_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var Gvalid = DropDownList1.SelectedValue;
            //_dbr.Conectar();
            //_dbr.CrearComando("SELECT nombre FROM Cat_TiposProveedor WHERE idTipo  = @id");
            //_dbr.AsignarParametroCadena("@id", _idEditar);
            //var dr = _dbr.EjecutarConsulta();
            //if (dr.Read())
            //{
            //    tbNname.Text = dr[0].ToString();
            //}
            //_dbr.Desconectar();   
            if (!string.IsNullOrEmpty(tbOrden.Text))
            {
                var idEstructura = "";
                try
                {
                    _dbr.Conectar();
                    _dbr.CrearComando(@"SELECT IdEstructura from Cat_EstructuraValidacionTemp WHERE id_TipoProveedor = @idTipoProveedor AND ordenIdGruposValidacion = @grupo OR orden = @orden AND id_TipoProveedor = @idTipoProveedor");
                    _dbr.AsignarParametroCadena("@idTipoProveedor", tbProveedor.Text);
                    _dbr.AsignarParametroCadena("@idTipoProveedor", tbProveedor.Text);
                    _dbr.AsignarParametroCadena("@grupo", Gvalid);
                    // _dbr.AsignarParametroCadena("@ordenIdGruposValidacion", DropDownList1.SelectedValue);
                    _dbr.AsignarParametroCadena("@orden", tbOrden.Text);
                    var dr = _dbr.EjecutarConsulta();
                    if (dr.Read())
                    {
                        idEstructura = dr[0].ToString();
                    }
                    _dbr.Desconectar();
                }
                catch (Exception ex) { }
                if (string.IsNullOrEmpty(idEstructura))
                {
                    _dbr.Conectar();
                    _dbr.CrearComando(@"INSERT Cat_EstructuraValidacionTemp (id_TipoProveedor, ordenIdGruposValidacion, orden) VALUES (@idTipoProveedor, @ordenIdGruposValidacion, @orden)");
                    _dbr.AsignarParametroCadena("@idTipoProveedor", tbProveedor.Text);
                    _dbr.AsignarParametroCadena("@ordenIdGruposValidacion", DropDownList1.SelectedValue);
                    _dbr.AsignarParametroCadena("@orden", tbOrden.Text);
                    _dbr.EjecutarConsulta1();
                    _dbr.Desconectar();

                    //        divEstructureProveedor.show();  
                    llenarProveedor(ddlTiposProveedor.SelectedValue);
                    ScriptManager.RegisterStartupScript(this, GetType(), "_ddlTiposProveedor", "$('body').removeClass('modal-open');$('.modal-backdrop').remove(); $('#divEstructureProveedor').modal('show');", true);

                    gvEstructuras.DataBind();
                    // DropDownList1.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "_ddlTiposProveedor", "$('body').removeClass('modal-open');$('.modal-backdrop').remove(); $('#divEstructureProveedor').modal('show');", true);
                    llenarProveedor(ddlTiposProveedor.SelectedValue);
                    gvEstructuras.DataBind();
                    // DropDownList1.DataBind();                   
                    (Master as SiteMaster).MostrarAlerta(this, "Grupo validador registrado", 2, null);
                }
                //ScriptManager.RegisterStartupScript(this, GetType(), "_ddlTiposProveedor", "$('body').removeClass('modal-open');$('.modal-backdrop').remove(); $('#divEstructureProveedor').modal('show');", true);
                //gvEstructuras.DataBind();
                //DropDownList1.DataBind();
                //tbOrden.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_ddlTiposProveedor", "$('body').removeClass('modal-open');$('.modal-backdrop').remove(); $('#divEstructureProveedor').modal('show');", true);
                llenarProveedor(ddlTiposProveedor.SelectedValue);
                gvEstructuras.DataBind();
                (Master as SiteMaster).MostrarAlerta(this, "Orden validador no puede quedar vacio", 2, null);
            }
            tbOrden.Text = "";

        }

        /// <summary>
        /// Handles the Click event of the bUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bUpdate_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            bAgregar.Text = "Modificar";
            _dbr.Conectar();
            _dbr.CrearComando("SELECT nombre FROM Cat_TiposProveedor WHERE idTipo  = @id");
            _dbr.AsignarParametroCadena("@id", _idEditar);
            var dr = _dbr.EjecutarConsulta();
            if (dr.Read())
            {
                tbNname.Text = dr[0].ToString();
            }
            _dbr.Desconectar();
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divUpdateP').modal('toggle').show();", true);
        }

        /// <summary>
        /// Handles the Click event of the bUpdateP control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bUpdateP_Click(object sender, EventArgs e)
        {
            _dbr.Conectar();
            _dbr.CrearComando("update Cat_TiposProveedor set nombre=@nombre, visible=@visible where idTipo=@id");
            _dbr.AsignarParametroCadena("@nombre", tbNname.Text);
            _dbr.AsignarParametroCadena("@visible", ddlStatus.SelectedValue);
            _dbr.AsignarParametroCadena("@id", _idEditar);
            _dbr.EjecutarConsulta();

            _dbr.Desconectar();
            (Master as SiteMaster).MostrarAlerta(this, "El Tipo de Proveedor " + (!string.IsNullOrEmpty(tbNname.Text) ? " se modificó" : "") + " correctamente.", 2, null, "$('#divUpdateP').modal('hide');");
            _idEditar = "";
            gvTiposProveedor.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the lbAddGroupValidation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAddGroupValidation_Click(object sender, EventArgs e)
        {
            (Master as SiteMaster).MostrarAlerta(this, "El Grupo de validacion \"" + tbNuevoGrupo.Text + "\" se ha agregado correctamente", 2, null, "$('#divNuevoTipoProveedor').hide();$('body').removeClass('modal-open');$('.modal-backdrop').remove();", null);

            _dbr.Conectar();
            _dbr.CrearComando(@"INSERT cat_grupos_validadores (descripcion, orden) VALUES (@descripcion, @orden)");
            _dbr.AsignarParametroCadena("@descripcion", tbNuevoGrupo.Text);
            _dbr.AsignarParametroCadena("@orden", "1");     //quitar de BD
            _dbr.EjecutarConsulta1();
            _dbr.Desconectar();
            gvGroupValidation.DataBind();
            ddlTiposProveedor.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the bDeleteG control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bDeleteG_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            _dbr.Conectar();
            _dbr.CrearComando("DELETE cat_grupos_validadores where idGrupo=@id");
            _dbr.AsignarParametroCadena("@id", _idEditar);
            _dbr.EjecutarConsulta();
            _dbr.Desconectar();
            (Master as SiteMaster).MostrarAlerta(this, "El Grupo se elimino correctamente " + (!string.IsNullOrEmpty(tbNname.Text) ? " " : "") + ".", 2, null, "$('#divUpdateP').modal('hide');");
            _idEditar = "";
            gvGroupValidation.DataBind();
            ddlTiposProveedor.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the bUpdateG control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bUpdateG_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            _idEditarU = _idEditar;
            bAgregar.Text = "Modificar";
            _dbr.Conectar();
            _dbr.CrearComando("SELECT descripcion FROM cat_grupos_validadores WHERE idGrupo = @id");
            _dbr.AsignarParametroCadena("@id", _idEditar);
            var dr = _dbr.EjecutarConsulta();
            if (dr.Read())
            {
                tbUpdatePro.Text = dr[0].ToString();
            }
            _dbr.Desconectar();
            ScriptManager.RegisterStartupScript(this, GetType(), "", "$('#divUpdateTipoProveedor').modal('show');", true);
        }


        /// <summary>
        /// Handles the Click event of the bDeleteE control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bDeleteE_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            _dbr.Conectar();
            _dbr.CrearComando("update Cat_TiposProveedor set visible=@visible, eliminado=@eliminado where idTipo=@id");
            _dbr.AsignarParametroCadena("@visible", "0");
            _dbr.AsignarParametroCadena("@eliminado", "1");
            _dbr.AsignarParametroCadena("@id", _idEditar);
            _dbr.EjecutarConsulta();
            gvTiposProveedor.DataBind();
            ddlTiposProveedor.DataBind();
        }




        /// <summary>
        /// Handles the Click event of the bUpdateFI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bUpdateFI_Click(object sender, EventArgs e)
        {

            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            _dbr.Conectar();
            _dbr.CrearComando("update Cat_EstructuraValidacionTemp set ordenIdGruposValidacion=@OG, orden =@orden where idTipo=@id");
            _dbr.AsignarParametroCadena("@OG", tbProveedor.Text);
            _dbr.AsignarParametroCadena("@orden", tbOrden.Text);
            _dbr.AsignarParametroCadena("@id", _idEditar);
            _dbr.EjecutarConsulta();
        }

        /// <summary>
        /// Handles the Click event of the bDeleteFI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bDeleteFI_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            _dbr.Conectar();
            _dbr.CrearComando("DELETE Cat_EstructuraValidacionTemp where idEstructura=@id");
            _dbr.AsignarParametroCadena("@id", _idEditar);
            _dbr.EjecutarConsulta();

            ScriptManager.RegisterStartupScript(this, GetType(), "_ddlTiposProveedor", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#divEstructureProveedor').modal('show');", true);
            gvEstructuras.DataBind();
            DropDownList1.DataBind();
            //  _idEditar = "";
        }

        /// <summary>
        /// Handles the Click event of the lbAddUpdateProveedor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAddUpdateProveedor_Click(object sender, EventArgs e)
        {


            var btn = (LinkButton)sender;
            var id = btn.CommandArgument;
            _idEditar = id;
            _dbr.Conectar();
            _dbr.CrearComando("update cat_grupos_validadores set descripcion=@desc where idGrupo=@id");
            _dbr.AsignarParametroCadena("@desc", tbUpdatePro.Text);
            _dbr.AsignarParametroCadena("@id", _idEditarU);
            _dbr.EjecutarConsulta();
            gvGroupValidation.DataBind();
            (Master as SiteMaster).MostrarAlerta(this, "El Grupo \"" + tbUpdatePro.Text + "\" se actualizo correctamente ", 2, null, "$('#divUpdateTipoProveedor').hide();$('body').removeClass('modal-open');$('.modal-backdrop').remove();", null);
        }

        /// <summary>
        /// Handles the Click event of the lbAddAllOrders control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAddAllOrders_Click(object sender, EventArgs e)
        {
            var inserted = false;
            var idProveedor = tbProveedor.Text;
            try
            {
                var estructura = "";
                _dbr.Conectar();
                _dbr.CrearComando("SELECT ordenIdGruposValidacion FROM Cat_EstructuraValidacionTemp WHERE id_TipoProveedor = @idProveedor order BY id_TipoProveedor, orden");
                _dbr.AsignarParametroCadena("@idProveedor", idProveedor);
                var dr = _dbr.EjecutarConsulta();
                while (dr.Read())
                {
                    estructura += dr["ordenIdGruposValidacion"].ToString() + ",";
                }
                _dbr.Desconectar();
                estructura = estructura.Trim(',').Trim();
                var existe = false;
                _dbr.Conectar();
                _dbr.CrearComando("SELECT IdEstructura FROM Cat_EstructuraValidacion WHERE id_TipoProveedor = @id");
                _dbr.AsignarParametroCadena("@id", idProveedor);
                dr = _dbr.EjecutarConsulta();
                existe = dr.Read();
                _dbr.Desconectar();
                if (!existe)
                {
                    _dbr.Conectar();
                    _dbr.CrearComando("INSERT INTO Cat_EstructuraValidacion (id_TipoProveedor, ordenIdGruposValidacion) OUTPUT inserted.IdEstructura VALUES (@id, @estructura)");
                    _dbr.AsignarParametroCadena("@id", idProveedor);
                    _dbr.AsignarParametroCadena("@estructura", estructura);
                    dr = _dbr.EjecutarConsulta();
                    if (dr.Read())
                    {
                        inserted = true;
                        var data = dr["IdEstructura"].ToString();
                    }
                    _dbr.Desconectar();
                }
                else
                {
                    _dbr.Conectar();
                    _dbr.CrearComando("UPDATE Cat_EstructuraValidacion SET ordenIdGruposValidacion = @estructura OUTPUT inserted.IdEstructura WHERE id_TipoProveedor = @id");
                    _dbr.AsignarParametroCadena("@id", idProveedor);
                    _dbr.AsignarParametroCadena("@estructura", estructura);
                    dr = _dbr.EjecutarConsulta();
                    if (dr.Read())
                    {
                        inserted = true;
                        var data = dr["IdEstructura"].ToString();
                    }
                }

                if (!inserted)
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El registro no se pudo insertar. Intentelo nuevamente", 4);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El registro se inserto correctamente", 2, null, "$('#divEstructureProveedor').hide();$('body').removeClass('modal-open');$('.modal-backdrop').remove();", null);
                }
            }
            catch (Exception ex)
            {
                inserted = false;
                (Master as SiteMaster).MostrarAlerta(this, "El registro no se pudo insertar: " + ex.Message + ". Intentelo nuevamente", 4);
            }
            finally
            {
                try
                {
                    _dbr.Conectar();
                    _dbr.CrearComando("DELETE FROM Cat_EstructuraValidacionTemp WHERE id_TipoProveedor = @id");
                    _dbr.AsignarParametroCadena("@id", idProveedor);
                    _dbr.EjecutarConsulta1();
                    _dbr.Desconectar();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    gvTiposProveedor.DataBind();
                    ddlTiposProveedor.DataBind();
                    gvEstructuras.DataBind();
                    DropDownList1.DataBind();
                    tbOrden.Text = "";
                }
            }
        }

        protected void llenarProveedor(string idProveedor)
        {
            var where = "";
            try
            {
                var existe = false;
                _dbr.Conectar();
                _dbr.CrearComando(@" SELECT g.idGrupo, g.descripcion, s.i AS orden FROM FN_Split((SELECT ordenIdGruposValidacion 
                FROM Cat_EstructuraValidacion WHERE id_TipoProveedor = @idtipoProv), ',') s INNER JOIN Cat_Grupos_validadores g ON g.idGrupo = s.item");
                _dbr.AsignarParametroCadena("@idtipoProv", idProveedor);
                var dt = new DataTable();
                var dr = _dbr.EjecutarConsulta();
                if (dr.HasRows)
                {
                    dt.Load(dr);
                    gvEstructuras.DataSourceID = null;
                    gvEstructuras.DataSource = dt;
                    _dbr.Desconectar();
                    gvEstructuras.DataBind();
                }
                else
                {
                    if (!string.IsNullOrEmpty(idProveedor))
                    {
                        where = "WHERE " + " id_TipoProveedor= " + idProveedor;
                    }
                    var sql = @"SELECT g.idGrupo, g.descripcion, V.orden FROM Cat_EstructuraValidacionTemp V
inner join Cat_Grupos_validadores g ON V.ordenIdGruposValidacion = g.idGrupo ";
                    sql += where + " ORDER BY idEstructura ASC";
                    SqlDataSourceEstructura.SelectCommand = sql;
                    gvEstructuras.DataSource = null;
                    gvEstructuras.DataSourceID = SqlDataSourceEstructura.ID;
                    gvEstructuras.DataBind();
                }
                DropDownList1.DataBind();
                ddlTiposProveedor.DataBind();

            }
            catch (Exception ex) { }
            //finally { _dbr.Desconectar(); }

        }

        protected void Distribuidor_Clic(object sender, EventArgs e)
        {
            var ab = idcheck.Value;
            if (!string.IsNullOrEmpty(ab.ToString()))
            {

                try
                {
                    _dbr.Conectar();
                    _dbr.CrearComando(@"UPDATE Cat_Grupos_validadores SET statusDistribuidor = 1 where idGrupo = @idgrupo");
                    _dbr.AsignarParametroCadena("@idgrupo", ab);
                    _dbr.EjecutarConsulta();
                    _dbr.Desconectar();
                }
                catch (Exception ex) { }


                _dbr.Conectar();
                _dbr.CrearComando(@"UPDATE Cat_Grupos_validadores SET statusDistribuidor = 0 where idGrupo <> @idgrupo");
                _dbr.AsignarParametroCadena("@idgrupo", ab);
                _dbr.EjecutarConsulta();
                _dbr.Desconectar();

            }
            else
            {

            }
            gvGroupValidation.DataBind();
        }

        protected void gvGroupValidation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataRow = ((DataRowView)e.Row.DataItem).Row;
                var idGrupo = dataRow.Field<int>("idGrupo");
                var check = (CheckBox)(e.Row.FindControl("check"));
                var statusDistribuidor = dataRow.Field<int>("statusDistribuidor");
                check.Checked = statusDistribuidor.Equals(1);
            }
        }
    }
}