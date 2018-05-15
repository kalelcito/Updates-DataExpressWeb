// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 07-02-2017
//
// Last Modified By : Sergio
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="receptores.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataExpressWeb;
using Datos;
using System.Linq;
using System.IO;
using OfficeOpenXml;

namespace Administracion
{
    /// <summary>
    /// Class Receptores.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Receptores : Page
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
        /// The identifier user
        /// </summary>
        private string _idUser = "";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            if (Session["idUser"] != null)
            {
                _idUser = Session["idUser"].ToString();
                SqlDataReceptores.ConnectionString = _db.CadenaConexion;
                SqlDataContactos.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.ConnectionString = _db.CadenaConexion;
                SqlDataMetodoPago.SelectCommand = SqlDataMetodoPago.SelectCommand.Replace("@tipoCatalogo", Session["CfdiVersion"].ToString().Equals("3.3") ? "MetodoPagoCfdi33" : "MetodoPago");
                if (!Page.IsPostBack)
                {
                    #region Giro Empresarial

                    if (Session["IDGIRO"] != null)
                    {
                        if (Session["IDGIRO"].ToString().Contains("1") || Session["IDGIRO"].ToString().Contains("2"))
                        {
                            trDenom.Visible = false;
                            trCurp.Visible = false;
                            rowDenomBus.Visible = false;
                        }
                        else
                        {
                            trDenom.Visible = true;
                            trCurp.Visible = true;
                            rowDenomBus.Visible = true;
                        }
                    }

                    #endregion
                    _db.Conectar();
                    _db.CrearComando("DELETE FROM Cat_Mx_Contactos_Temp");
                    _db.EjecutarConsulta1();
                    _db.Desconectar();
                    Buscar();
                    RequiredFieldValidator14.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                    RequiredFieldValidator16.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                    RequiredFieldValidator23.Enabled = !Session["CfdiVersion"].ToString().Equals("3.3");
                    if (Session["IDENTEMI"].ToString().Equals("OHC070227M80") || Session["IDENTEMI"].ToString().Equals("OPL000131DL3"))
                    {
                        lblTitulo.Text = "CLIENTES";
                        bNuevo.Text = "Nuevo Cliente";
                        lblBuscarReceptor.Text = "Buscar Cliente";
                        lblAgregarReceptor.Text = "Agregar/Editar Cliente";
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the bBuscarReg control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            Buscar();
            ScriptManager.RegisterStartupScript(this, GetType(), "_bBuscar_Click_CerrarFiltros", "$('#divBuscar').modal('hide');", true);
        }

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
            var where = "";
            var whereTemp = "";
            var sql = "";
            if (!string.IsNullOrEmpty(tbRfcBus.Text))
            {
                whereTemp = "r.RFCREC LIKE '%" + tbRfcBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (!string.IsNullOrEmpty(tbNomBus.Text))
            {
                whereTemp = "r.NOMREC LIKE '%" + tbNomBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (!string.IsNullOrEmpty(tbDenBus.Text))
            {
                whereTemp = "r.denominacionSocial LIKE '%" + tbDenBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (!string.IsNullOrEmpty(tbDireccBus.Text))
            {
                whereTemp = "r.domicilio LIKE '%" + tbDireccBus.Text + "%' OR r.colonia LIKE '%" + tbDireccBus.Text + "%' OR r.localidad LIKE '%" + tbDireccBus.Text + "%' OR r.referencia LIKE '%" + tbDireccBus.Text + "%' OR r.municipio LIKE '%" + tbDireccBus.Text + "%' OR r.estado LIKE '%" + tbDireccBus.Text + "%' OR r.pais LIKE '%" + tbDireccBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (!string.IsNullOrEmpty(tbContactBus.Text))
            {
                whereTemp = "c.nombre LIKE '%" + tbContactBus.Text + "%' OR c.correo LIKE '%" + tbContactBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (!string.IsNullOrEmpty(tbMailBus.Text))
            {
                whereTemp = "r.email LIKE '%" + tbMailBus.Text + "%'";
                where += !string.IsNullOrEmpty(where) ? " AND " + whereTemp : whereTemp;
            }
            if (chkRepetidos.Checked)
            {
                sql = @"SELECT DISTINCT r.IDEREC, r.RFCREC, r.NOMREC, r.domicilio, r.noExterior, r.noInterior, r.colonia, r.municipio, r.estado, r.pais, r.codigoPostal, r.curp, r.denominacionSocial FROM Cat_Receptor r LEFT OUTER JOIN Cat_Mx_Contactos c ON r.IDEREC = c.idReceptor";
                sql += !string.IsNullOrEmpty(where) ? " WHERE (" + where + ")" : "";
            }
            else
            {
                sql = @"WITH Receptores AS
                             (
                                 SELECT r.IDEREC, r.RFCREC, r.NOMREC, r.domicilio, r.noExterior, r.noInterior, r.colonia, r.municipio, r.estado, r.pais, r.codigoPostal, r.curp, r.denominacionSocial,
                                        ROW_NUMBER() OVER(
                                            PARTITION BY r.RFCREC ORDER BY r.RFCREC ASC, r.IDEREC ASC
                                        ) counter
                                 FROM   Cat_Receptor r LEFT OUTER JOIN Cat_Mx_Contactos c ON r.IDEREC = c.idReceptor
                                 " + (!string.IsNullOrEmpty(where) ? " WHERE (" + where + ")" : "") + @"
                             )
                        SELECT *
                        FROM   Receptores
                        WHERE  counter = 1";
            }
            SqlDataReceptores.SelectCommand = sql;
            SqlDataReceptores.DataBind();
            gvReceptor.DataBind();
            SqlDataContactos.DataBind();
            gvContactos.DataBind();
            lCount.Visible = true;

        }

        private bool ValidarCodigoPostal(string clave)
        {
            try
            {
                if (!Session["CfdiVersion"].ToString().Equals("3.3"))
                {
                    return true;
                }
                var catalogos = (CatCdfi)Session["CatalogosCfdi33"];
                var existe = catalogos.CCodigopostal.Any(c => c.Key.Equals(clave, StringComparison.OrdinalIgnoreCase));
                return existe;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        /// <summary>
        /// Handles the Click event of the bAgregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bAgregar_Click(object sender, EventArgs e)
        {
            if (Session["CfdiVersion"].ToString().Equals("3.3") && !ValidarCodigoPostal(tbCpRec.Text))
            {
                (Master as SiteMaster).MostrarAlerta(this, $"El codigo postal { tbCpRec.Text } no se encuentra en el catálogo del SAT", 4, null);
            }
            var idCatReceptor = "";
            var sql = "";
            DbDataReader dr;
            if (string.IsNullOrEmpty(_idEditar))
            {
                sql = @"INSERT INTO Cat_Receptor (RFCREC, NOMREC, curp, telefono, email, telefono2, denominacionSocial, metodoPago, numCtaPago, domicilio, noExterior, noInterior, colonia, localidad, referencia, municipio, estado, pais, codigoPostal) OUTPUT inserted.IDEREC VALUES (@RFCREC,@NOMREC,@CURP,@TEL,@MAIL,@TEL2,@DENOM,@MET,@CTA,@CALLE,@NOEXT,@NOINT,@COL,@LOC,@REF,@MUN,@EST,@PAIS,@CP)";
            }
            else
            {
                sql = @"UPDATE Cat_Receptor SET RFCREC = @RFCREC, NOMREC = @NOMREC, curp = @CURP, telefono = @TEL, email = @MAIL, telefono2 = @TEL2, denominacionSocial = @DENOM, metodoPago = @MET, numCtaPago = @CTA, domicilio=@CALLE, noExterior=@NOEXT, noInterior=@NOINT, colonia=@COL, localidad=@LOC, referencia=@REF, municipio=@MUN, estado=@EST, pais=@PAIS, codigoPostal=@CP OUTPUT inserted.IDEREC WHERE IDEREC=@IDEREC";
            }
            try
            {
                _db.Conectar();
                _db.CrearComando(sql);
                if (!string.IsNullOrEmpty(_idEditar))
                {
                    _db.AsignarParametroCadena("@IDEREC", _idEditar);
                }
                _db.AsignarParametroCadena("@RFCREC", tbRfcRec.Text);
                _db.AsignarParametroCadena("@NOMREC", tbRazonSocialRec.Text);
                _db.AsignarParametroCadena("@CURP", tbCURPR.Text);
                _db.AsignarParametroCadena("@TEL", tbTelRec.Text);
                _db.AsignarParametroCadena("@MAIL", tbMailRec.Text);
                _db.AsignarParametroCadena("@TEL2", tbTelRec2.Text);
                _db.AsignarParametroCadena("@DENOM", tbDenomSocialRec.Text);
                _db.AsignarParametroCadena("@MET", ddlMetodoPago.SelectedValue);
                _db.AsignarParametroCadena("@CTA", tbNumCta.Text);
                _db.AsignarParametroCadena("@CALLE", tbCalleRec.Text);
                _db.AsignarParametroCadena("@NOEXT", tbNoExtRec.Text);
                _db.AsignarParametroCadena("@NOINT", tbNoIntRec.Text);
                _db.AsignarParametroCadena("@COL", tbColoniaRec.Text);
                _db.AsignarParametroCadena("@LOC", tbLocRec.Text);
                _db.AsignarParametroCadena("@REF", tbRefRec.Text);
                _db.AsignarParametroCadena("@MUN", tbMunicipioRec.Text);
                _db.AsignarParametroCadena("@EST", tbEstadoRec.Text);
                _db.AsignarParametroCadena("@PAIS", string.IsNullOrEmpty(tbPaisRec.Text) ? "MEX" : tbPaisRec.Text);
                _db.AsignarParametroCadena("@CP", tbCpRec.Text);
                dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    idCatReceptor = dr[0].ToString();
                    _db.Desconectar();
                    sql = "SELECT nombre, puesto, telefono1, telefono2, correo FROM Cat_Mx_Contactos_Temp WHERE (id_Empleado = @idUser)";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@idUser", _idUser);
                    var contactosTemp = new List<object[]>();
                    dr = _db.EjecutarConsulta();
                    while (dr.Read())
                    {
                        var contactoTemp = new object[dr.FieldCount];
                        dr.GetValues(contactoTemp);
                        contactosTemp.Add(contactoTemp);
                    }
                    _db.Desconectar();
                    var todosContactos = true;
                    foreach (var contactoTemp in contactosTemp)
                    {
                        sql = @"INSERT INTO Cat_Mx_Contactos (idReceptor, nombre, puesto, telefono1, telefono2, correo) VALUES (@idReceptor,@nombre,@puesto,@telefono1,@telefono2,@correo)";
                        try
                        {
                            _db.Conectar();
                            _db.CrearComando(sql);
                            _db.AsignarParametroCadena("@idReceptor", idCatReceptor);
                            _db.AsignarParametroCadena("@nombre", contactoTemp[0].ToString());
                            _db.AsignarParametroCadena("@puesto", contactoTemp[1].ToString());
                            _db.AsignarParametroCadena("@telefono1", contactoTemp[2].ToString());
                            _db.AsignarParametroCadena("@telefono2", contactoTemp[3].ToString());
                            _db.AsignarParametroCadena("@correo", contactoTemp[4].ToString());
                            _db.EjecutarConsulta1();
                        }
                        catch (Exception)
                        {
                            todosContactos = false;
                        }
                        finally
                        {
                            _db.Desconectar();
                        }
                    }
                    if (!todosContactos)
                    {
                        if (Session["IDENTEMI"].ToString().Equals("OHC070227M80") || Session["IDENTEMI"].ToString().Equals("OPL000131DL3"))
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "No todos los contactos del cliente se pudieron agregar correctamente. Intente agregarlos de nuevo.", 4, null);
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "No todos los contactos del receptor se pudieron agregar correctamente. Intente agregarlos de nuevo.", 4, null);
                        }
                    }
                    else
                    {

                        tbNomContRec.Text = "";
                        tbPuestoContRec.Text = "";
                        tbTelContRec1.Text = "";
                        tbTelContRec2.Text = "";
                        tbMailContRec.Text = "";
                        tbRfcBus.Text = "";
                        tbNomBus.Text = "";
                        tbDenBus.Text = "";
                        tbDireccBus.Text = "";
                        tbContactBus.Text = "";
                        tbMailBus.Text = "";
                        tbRfcRec.Text = "";
                        tbRazonSocialRec.Text = "";
                        tbCURPR.Text = "";
                        tbTelRec.Text = "";
                        tbMailRec.Text = "";
                        tbTelRec2.Text = "";
                        tbDenomSocialRec.Text = "";
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
                        ddlMetodoPago.SelectedValue = "";
                        tbNumCta.Text = "";
                        var key = "_key" + SiteMaster.Md5Alert();
                        ScriptManager.RegisterStartupScript(this, GetType(), key, "$('.modal').modal('hide');", true);
                    }
                }
                else
                {
                    if (Session["IDENTEMI"].ToString().Equals("OHC070227M80") || Session["IDENTEMI"].ToString().Equals("OPL000131DL3"))
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El cliente no se pudo agregar/modificar. Intentelo nuevamente", 4, null);
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El receptor no se pudo agregar/modificar. Intentelo nuevamente", 4, null);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session["IDENTEMI"].ToString().Equals("OHC070227M80") || Session["IDENTEMI"].ToString().Equals("OPL000131DL3"))
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El cliente no se pudo agregar/modificar. Intentelo nuevamente.<br>" + ex.Message, 4, null);
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "El receptor no se pudo agregar/modificar. Intentelo nuevamente.<br>" + ex.Message, 4, null);
                }
            }
            finally
            {
                _db.Desconectar();
                Buscar();
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvReceptor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvReceptor_PageIndexChanged(object sender, EventArgs e)
        {
            gvReceptor.DataBind();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvReceptor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvReceptor_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvReceptor.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvReceptor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvReceptor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReceptor.PageIndex = e.NewPageIndex;
            gvReceptor.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvReceptor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvReceptor_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvReceptor.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the RowDeleted event of the gvReceptor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeletedEventArgs"/> instance containing the event data.</param>
        protected void gvReceptor_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvContactos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvContactos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvContactos.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvContactos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvContactos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvContactos.PageIndex = e.NewPageIndex;
            gvContactos.DataBind();
        }

        /// <summary>
        /// Handles the PreRender event of the gvContactos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvContactos_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvContactos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gvContactos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvContactos_PageIndexChanged(object sender, EventArgs e)
        {
            gvContactos.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the bAgregarContacto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bAgregarContacto_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbNomContRec.Text) && !string.IsNullOrEmpty(tbMailContRec.Text))
            {
                _db.Conectar();
                _db.CrearComando(@"INSERT INTO Cat_Mx_Contactos_Temp (id_Empleado, nombre, puesto, telefono1, telefono2, correo) VALUES (@idUser, @nombre, @puesto, @tel1, @tel2, @correo)");
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@nombre", tbNomContRec.Text);
                _db.AsignarParametroCadena("@puesto", tbPuestoContRec.Text);
                _db.AsignarParametroCadena("@tel1", tbTelContRec1.Text);
                _db.AsignarParametroCadena("@tel2", tbTelContRec2.Text);
                _db.AsignarParametroCadena("@correo", tbMailContRec.Text);
                _db.EjecutarConsulta1();
                _db.Desconectar();
                SqlDataContactos.DataBind();
                gvContactos.DataBind();
                tbNomContRec.Text = "";
                tbPuestoContRec.Text = "";
                tbTelContRec1.Text = "";
                tbTelContRec2.Text = "";
                tbMailContRec.Text = "";
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El campo " + (string.IsNullOrEmpty(tbNomContRec.Text) ? "nombre" : (string.IsNullOrEmpty(tbMailContRec.Text) ? "correo" : "")) + " no puede quedar vacio", 4, null);
            }

        }

        /// <summary>
        /// Handles the Click event of the bLimpiarBus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bLimpiarBus_Click(object sender, EventArgs e)
        {
            tbRfcBus.Text = "";
            tbNomBus.Text = "";
            tbDenBus.Text = "";
            tbDireccBus.Text = "";
            tbContactBus.Text = "";
            tbMailBus.Text = "";
            Buscar();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEditar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            tbRfcRec.ReadOnly = !chkEditar.Checked;
            if (chkEditar.Visible)
            {
                if (Session["IDENTEMI"].ToString().Equals(""))
                {
                    // Los que si quieren editar el RFC
                }
                else
                {
                    tbRfcRec.ReadOnly = true;
                }
            }
            tbRazonSocialRec.ReadOnly = !chkEditar.Checked;
            tbCURPR.ReadOnly = !chkEditar.Checked;
            tbTelRec.ReadOnly = !chkEditar.Checked;
            tbMailRec.ReadOnly = !chkEditar.Checked;
            tbTelRec2.ReadOnly = !chkEditar.Checked;
            tbDenomSocialRec.ReadOnly = !chkEditar.Checked;
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
            tbNomContRec.ReadOnly = !chkEditar.Checked;
            tbPuestoContRec.ReadOnly = !chkEditar.Checked;
            tbTelContRec1.ReadOnly = !chkEditar.Checked;
            tbTelContRec2.ReadOnly = !chkEditar.Checked;
            tbMailContRec.ReadOnly = !chkEditar.Checked;
            bAgregarContacto.Visible = chkEditar.Checked;
            ddlMetodoPago.Enabled = chkEditar.Checked;
            tbNumCta.ReadOnly = !chkEditar.Checked;
            foreach (GridViewRow row in gvContactos.Rows)
            {
                var bEliminarRow = (LinkButton)row.FindControl("bEliminarContacto");
                bEliminarRow.Visible = chkEditar.Checked;
            }
            bAgregar.Visible = chkEditar.Checked;
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
            Llenarlista(id);
            LlenaContactosInicio(id);
            chkEditar.Visible = true;
            chkEditar.Checked = false;
            chkEditar_CheckedChanged(null, null);
            // tbRfcRec.ReadOnly = false;
            bAgregar.Text = "Modificar";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bDetallesKey", "$('#divNuevo').modal('toggle');", true);
        }

        /// <summary>
        /// Llenas the contactos inicio.
        /// </summary>
        /// <param name="iderec">The iderec.</param>
        private void LlenaContactosInicio(string iderec)
        {
            _db.Conectar();
            _db.CrearComando("DELETE FROM Cat_Mx_Contactos_Temp");
            _db.EjecutarConsulta1();
            _db.Desconectar();
            if (!string.IsNullOrEmpty(iderec))
            {
                _db.Conectar();
                _db.CrearComando(@"INSERT INTO Cat_Mx_Contactos_Temp (id_Empleado, nombre, puesto, telefono1, telefono2, correo) (SELECT @idUser, nombre, puesto, telefono1, telefono2, correo FROM Cat_Mx_Contactos WHERE Cat_Mx_Contactos.idReceptor = @idReceptor)");
                _db.AsignarParametroCadena("@idUser", _idUser);
                _db.AsignarParametroCadena("@idReceptor", iderec);
                _db.EjecutarConsulta();
                _db.Desconectar();
                SqlDataContactos.DataBind();
                gvContactos.DataBind();
            }
        }

        /// <summary>
        /// Llenarlistas the specified iderec.
        /// </summary>
        /// <param name="iderec">The iderec.</param>
        private void Llenarlista(string iderec)
        {
            var sql = @"SELECT [RFCREC]
                              ,[NOMREC]
                              ,[DenominacionSocial]
                              ,[contribuyenteEspecial]
                              ,[tipoIdentificacionComprador]
                              ,[email]
                              ,[domicilio]
                              ,[telefono]
                              ,[noExterior]
                              ,[noInterior]
                              ,[colonia]
                              ,[localidad]
                              ,[referencia]
                              ,[municipio]
                              ,[estado]
                              ,[pais]
                              ,[codigoPostal]
                              ,[curp]
                              ,[metodoPago]
                              ,[numCtaPago]
                          FROM [Cat_Receptor]
                          WHERE IDEREC=@id";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@id", iderec);
            var dr = _db.EjecutarConsulta();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tbRfcRec.Text = dr["RFCREC"].ToString();
                    tbRazonSocialRec.Text = dr["NOMREC"].ToString();
                    tbCURPR.Text = dr["curp"].ToString();
                    tbMailRec.Text = dr["email"].ToString();
                    tbTelRec.Text = dr["telefono"].ToString();
                    tbCalleRec.Text = dr["domicilio"].ToString();
                    tbNoExtRec.Text = dr["noExterior"].ToString();
                    tbNoIntRec.Text = dr["noInterior"].ToString();
                    tbColoniaRec.Text = dr["colonia"].ToString();
                    tbLocRec.Text = dr["localidad"].ToString();
                    tbRefRec.Text = dr["referencia"].ToString();
                    tbMunicipioRec.Text = dr["municipio"].ToString();
                    tbEstadoRec.Text = dr["estado"].ToString();
                    tbPaisRec.Text = dr["pais"].ToString();
                    tbCpRec.Text = dr["codigoPostal"].ToString();
                    try { ddlMetodoPago.SelectedValue = dr["metodoPago"].ToString(); } catch { ddlMetodoPago.SelectedValue = ""; }
                    tbNumCta.Text = dr["numCtaPago"].ToString();
                    tbDenomSocialRec.Text = dr["DenominacionSocial"].ToString();
                }
            }
            else
            {
                tbRfcRec.Text = "";
                tbRazonSocialRec.Text = "";
                tbCURPR.Text = "";
                tbMailRec.Text = "";
                tbTelRec.Text = "";
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
                ddlMetodoPago.SelectedValue = "";
                tbNumCta.Text = "";
                tbDenomSocialRec.Text = "";
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the Click event of the bNuevo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bNuevo_Click(object sender, EventArgs e)
        {

            try
            {
                _db.Conectar();
                _db.CrearComando("DELETE FROM Cat_Mx_Contactos_Temp");
                _db.EjecutarConsulta1();
            }
            catch (Exception ex) { }
            finally { _db.Desconectar(); }

            _idEditar = null;
            Llenarlista("");
            chkEditar.Visible = false;
            chkEditar.Checked = true;
            tbRfcRec.ReadOnly = false;
            chkEditar_CheckedChanged(null, null);
            gvContactos.DataBind(); //recargar gv
            bAgregar.Text = "Agregar";
            ScriptManager.RegisterStartupScript(this, GetType(), "_bNuevoKey", "$('#divNuevo').modal('toggle');", true);
        }

        /// <summary>
        /// Handles the DataBound event of the gvReceptor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvReceptor_DataBound(object sender, EventArgs e)
        {
            #region Giro Empresarial

            var visible = true;

            if (Session["IDGIRO"] != null)
            {
                if (Session["IDGIRO"].ToString().Contains("1") || Session["IDGIRO"].ToString().Contains("2"))
                {
                    visible = false;
                }
            }

            #endregion
            gvReceptor.Columns[2].Visible = visible;
            gvReceptor.Columns[3].Visible = visible;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlMetodoPago control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNumCta_MaskedEditExtender.Enabled = ddlMetodoPago.SelectedValue.Equals("03") || ddlMetodoPago.SelectedValue.Equals("04") || ddlMetodoPago.SelectedValue.Equals("28");
            tbNumCta.ReadOnly = ddlMetodoPago.SelectedValue.Equals("99") || ddlMetodoPago.SelectedValue.Equals("01") || ddlMetodoPago.SelectedValue.Equals("NA") || ddlMetodoPago.SelectedValue.Equals("");
            tbNumCta.Text = tbNumCta_MaskedEditExtender.Enabled ? "" : "NO IDENTIFICADO";
        }

        /// <summary>
        /// Handles the RowDeleting event of the gvReceptor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeleteEventArgs"/> instance containing the event data.</param>
        protected void gvReceptor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int count = 0;
                var cell = gvReceptor.Rows[e.RowIndex].Cells[0];
                var rfc = cell.Text;
                var sql = @"SELECT COUNT(*) FROM Dat_General g
INNER JOIN Cat_Receptor r ON g.id_Receptor = r.IDEREC
WHERE r.RFCREC = @RFC";
                _db.Conectar();
                _db.CrearComando(sql);
                _db.AsignarParametroCadena("@RFC", rfc);
                var dr = _db.EjecutarConsulta();
                if (dr.Read())
                {
                    int.TryParse(dr[0].ToString(), out count);
                }
                _db.Desconectar();
                if (count > 0)
                {
                    e.Cancel = true;
                    (Master as SiteMaster).MostrarAlerta(this, "El cliente no se puede eliminar dado que tiene facturas asociadas.", 4);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkRepetidos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void chkRepetidos_CheckedChanged(object sender, EventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Handles the Selected event of the SqlDataReceptores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlDataSourceStatusEventArgs"/> instance containing the event data.</param>
        protected void SqlDataReceptores_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            var rowCount = e.AffectedRows;
            lCount.Text = "<strong>Se encontraron <span class='badge'>" + rowCount + "</span> Registros<strong>";
        }

        protected void bLoadXlsx_Click(object sender, EventArgs e)
        {
            Session["_XlsxCatalogoReceptores"] = null;
            ScriptManager.RegisterStartupScript(this, GetType(), "_bLoadXlsx_Click", "$('#divLoadXlsx').modal('show');", true);
        }

        protected void fileXlsx_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            var bytes = fileXlsx.FileBytes;
            Session["_XlsxCatalogoReceptores"] = bytes;
        }

        protected void lbCargarXlsx_Click(object sender, EventArgs e)
        {
            var sessionKey = Session["_XlsxCatalogoReceptores"];
            if (sessionKey != null)
            {
                var bytes = (byte[])sessionKey;
                if (bytes != null && bytes.Length > 30)
                {
                    try
                    {
                        var listaReceptores = GetReceptoresFromXlsx(bytes);
                        if (listaReceptores.Count > 0)
                        {
                            var guardado = GuardaReceptores(listaReceptores);
                            if (guardado)
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Los registros se han cargado correctamente", 2, null, "window.location.href = '" + ResolveClientUrl("~/adminstracion/usuarios/receptores.aspx") + "';");
                            }
                            else
                            {
                                (Master as SiteMaster).MostrarAlerta(this, "Los registros no pudieron ser guardados en su totalidad, favor de intentarlo nuevamente", 4);
                            }
                        }
                        else
                        {
                            (Master as SiteMaster).MostrarAlerta(this, "El archivo no contiene ningun registro válido", 4);
                        }
                    }
                    catch (Exception ex)
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El archivo está corrupto o no tiene la estructura correcta", 4);
                    }
                }
                else
                {
                    (Master as SiteMaster).MostrarAlerta(this, "Archivo vacío o corrupto", 4);
                }
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "No se ha cargado ningun archivo", 4);
            }
        }

        private List<Dictionary<string, string>> GetReceptoresFromXlsx(byte[] xlsx)
        {
            var result = new List<Dictionary<string, string>>();
            Stream stream = new MemoryStream(xlsx);
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.First();
                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;
                const int skipRows = 1;
                for (var row = (start.Row + skipRows); row <= end.Row; row++)
                {
                    var record = new Dictionary<string, string>();
                    record.Add("RFC", workSheet.Cells[row, 1].Text);
                    record.Add("RAZON SOCIAL", workSheet.Cells[row, 2].Text);
                    record.Add("E-MAIL", workSheet.Cells[row, 3].Text);
                    record.Add("CALLE", workSheet.Cells[row, 4].Text);
                    record.Add("TELEFONO", workSheet.Cells[row, 5].Text);
                    record.Add("NO EXTERIOR", workSheet.Cells[row, 6].Text);
                    record.Add("NO INTERIOR", workSheet.Cells[row, 7].Text);
                    record.Add("COLONIA", workSheet.Cells[row, 8].Text);
                    record.Add("LOCALIDAD", workSheet.Cells[row, 9].Text);
                    record.Add("REFERENCIA", workSheet.Cells[row, 10].Text);
                    record.Add("MUNICIPIO", workSheet.Cells[row, 11].Text);
                    record.Add("ESTADO", workSheet.Cells[row, 12].Text);
                    record.Add("PAIS", workSheet.Cells[row, 13].Text);
                    record.Add("CODIGO POSTAL", workSheet.Cells[row, 14].Text);
                    record.Add("CURP", workSheet.Cells[row, 15].Text);
                    record.Add("TELEFONO2", workSheet.Cells[row, 16].Text);
                    record.Add("METODO PAGO", workSheet.Cells[row, 17].Text);
                    record.Add("NUMERO CUENTA", workSheet.Cells[row, 18].Text);
                    record.Add("NUMREGIDTRIB", workSheet.Cells[row, 19].Text);
                    var isEmptyRow = !record.Any(cell => !string.IsNullOrEmpty(cell.Value));
                    if (!isEmptyRow) { result.Add(record); }
                }
            }
            return result;
        }

        private bool GuardaReceptores(List<Dictionary<string, string>> listaConceptos)
        {
            var result = false;
            int inserted = 0;
            foreach (var record in listaConceptos)
            {
                try
                {
                    var RFC = record["RFC"].Trim();
                    var RAZONSOCIAL = record["RAZON SOCIAL"].Trim();
                    var EMAIL = record["E-MAIL"].Trim();
                    var CALLE = record["CALLE"].Trim();
                    var TELEFONO = record["TELEFONO"].Trim();
                    var NOEXTERIOR = record["NO EXTERIOR"].Trim();
                    var NOINTERIOR = record["NO INTERIOR"].Trim();
                    var COLONIA = record["COLONIA"].Trim();
                    var LOCALIDAD = record["LOCALIDAD"].Trim();
                    var REFERENCIA = record["REFERENCIA"].Trim();
                    var MUNICIPIO = record["MUNICIPIO"].Trim();
                    var ESTADO = record["ESTADO"].Trim();
                    var PAIS = record["PAIS"].Trim();
                    var CODIGOPOSTAL = record["CODIGO POSTAL"].Trim();
                    var CURP = record["CURP"].Trim();
                    var TELEFONO2 = record["TELEFONO2"].Trim();
                    var METODOPAGO = record["METODO PAGO"].Trim();
                    var NUMEROCUENTA = record["NUMERO CUENTA"].Trim();
                    var NUMREGIDTRIB = record["NUMREGIDTRIB"].Trim();
                    var idReceptor = "";
                    if (string.IsNullOrEmpty(RFC) || string.IsNullOrEmpty(RAZONSOCIAL))
                    {
                        continue;
                    }
                    var sql = @"INSERT INTO Cat_Receptor (RFCREC, NOMREC, curp, telefono, email, telefono2, denominacionSocial, metodoPago, numCtaPago, domicilio, noExterior, noInterior, colonia, localidad, referencia, municipio, estado, pais, codigoPostal) OUTPUT inserted.IDEREC VALUES (@RFCREC,@NOMREC,@CURP,@TEL,@MAIL,@TEL2,@DENOM,@MET,@CTA,@CALLE,@NOEXT,@NOINT,@COL,@LOC,@REF,@MUN,@EST,@PAIS,@CP)";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@RFCREC", RFC);
                    _db.AsignarParametroCadena("@NOMREC", RAZONSOCIAL);
                    _db.AsignarParametroCadena("@CURP", CURP);
                    _db.AsignarParametroCadena("@TEL", TELEFONO);
                    _db.AsignarParametroCadena("@MAIL", EMAIL);
                    _db.AsignarParametroCadena("@TEL2", TELEFONO2);
                    _db.AsignarParametroCadena("@DENOM", "");
                    _db.AsignarParametroCadena("@MET", METODOPAGO);
                    _db.AsignarParametroCadena("@CTA", NUMEROCUENTA);
                    _db.AsignarParametroCadena("@CALLE", CALLE);
                    _db.AsignarParametroCadena("@NOEXT", NOEXTERIOR);
                    _db.AsignarParametroCadena("@NOINT", NOINTERIOR);
                    _db.AsignarParametroCadena("@COL", COLONIA);
                    _db.AsignarParametroCadena("@LOC", LOCALIDAD);
                    _db.AsignarParametroCadena("@REF", REFERENCIA);
                    _db.AsignarParametroCadena("@MUN", MUNICIPIO);
                    _db.AsignarParametroCadena("@EST", ESTADO);
                    _db.AsignarParametroCadena("@PAIS", PAIS);
                    _db.AsignarParametroCadena("@CP", CODIGOPOSTAL);
                    var dr = _db.EjecutarConsulta();
                    if (dr.Read())
                    {
                        idReceptor = dr[0].ToString();
                        inserted++;
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            result = inserted.Equals(listaConceptos.Count);
            return result;
        }

        protected void lbDownloadExample_Click(object sender, EventArgs e)
        {
            var bytes = DataExpressWeb.Properties.Resources.Layout_Receptores;
            var base64 = Convert.ToBase64String(bytes);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Layout.Receptores.Ejemplo.xlsx";
            ScriptManager.RegisterStartupScript(this, GetType(), "_lbDownloadExamplekey", "downloadBytes('" + base64 + "','" + contentType + "','" + fileName + "', false);", true);
        }
    }
}