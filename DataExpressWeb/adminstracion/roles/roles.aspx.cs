// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="roles.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using DataExpressWeb;
using Datos;

// ReSharper disable once CheckNamespace
namespace Administracion
{
    /// <summary>
    /// Class Roles.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Roles : Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _db = new BasesDatos(Session["IDENTEMI"]?.ToString() ?? "CORE");
                ScriptManager.RegisterStartupScript(this, GetType(), "_showPerfilesActive", "resaltar('liSeguridad');", true);
            }
            SqlDataPerfiles.ConnectionString = _db.CadenaConexion;
            if (Session["descRol"] != null && Session["descRol"].ToString().Equals("delatam", StringComparison.OrdinalIgnoreCase))
            {
                SqlDataPerfiles.SelectCommand = SqlDataPerfiles.SelectCommand.Replace(" AND visible = 'True'", "");
            }
        }

        /// <summary>
        /// Validars the rol.
        /// </summary>
        /// <param name="rol">The rol.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> de lo contrario.</returns>
        private bool ValidarRol(string rol)
        {
            _db.Conectar();
            _db.CrearComando("select idRol from Cat_roles where Descripcion=@Descripcion AND eliminado='False'");
            _db.AsignarParametroCadena("@Descripcion", rol);

            var dr = _db.EjecutarConsulta();

            while (dr.Read())
            {
                _db.Desconectar();
                return true;
            }
            _db.Desconectar();
            return false;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlPerfil control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ddlPerfil.SelectedValue;
            if (!value.Equals("0"))
            {
                BindData();
                bGuardar.Visible = true;
                bEliminar.Visible = !value.Equals("00");
                ScriptManager.RegisterStartupScript(this, GetType(), "_keyddlPerfil", "$('#divPaneles').show();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "_keyddlPerfil", "$('#divPaneles').hide();", true);
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            const string sql = @"SELECT [descripcion]
                              ,[consultar_fac_propias]
                              ,[consultar_todas_fac]
                           ,[VerCanc]
                           ,[DXC]
                              ,CAST(CASE WHEN [CNSComp] LIKE '%01%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CNSComp] LIKE '%04%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CNSComp] LIKE '%06%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CNSComp] LIKE '%07%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CNSComp] LIKE '%08%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CNSComp] LIKE '%09%' THEN 1 ELSE 0 END AS BIT)
                              ,[ReporteGeneral]
                              ,[TOPComp]
                              ,[CancComp]
                              ,CAST(CASE WHEN [CrearNewComp] LIKE '%01%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CrearNewComp] LIKE '%04%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CrearNewComp] LIKE '%06%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CrearNewComp] LIKE '%07%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CrearNewComp] LIKE '%08%' THEN 1 ELSE 0 END AS BIT)
                              ,CAST(CASE WHEN [CrearNewComp] LIKE '%09%' THEN 1 ELSE 0 END AS BIT)
                              ,[EditComp0N]
                              ,[Cliente]
                              ,[Empleado]
                              ,[Roles]
                              ,[EditEmisor]
                              ,[EditEstab]
                              ,[EditPtoEmi]
                              ,[EditInfoGeneral]
                              ,[EditSMTP]
                              ,[EditMensajes]
                              ,[EditUserOpera]
                              ,[LimpiarLogs]
                              ,[EditPerfil]
                              ,[EnvioEmail]
                              ,[eliminado]
                              ,[recepcion]
                              ,[Visible]
                              ,[arch]
                              ,[imp]
                              ,[EditReceptor]
                              ,[conc]
                              ,[validacionRecepcion]
                        FROM [dbo].[Cat_Roles] WHERE idRol = @idRol";
            _db.Conectar();
            _db.CrearComando(sql);
            _db.AsignarParametroCadena("@idRol", ddlPerfil.SelectedValue);
            var dr = _db.EjecutarConsulta();
            if (!dr.Read())
            {
                _db.Desconectar();
                CleanData();
                return;
            }
            tbNombrePerfil.Text = dr[0].ToString();
            consultar_fac_propias.Checked = Convert.ToBoolean(dr[1]);
            consultar_todas_fac.Checked = Convert.ToBoolean(dr[2]);
            VerCanc.Checked = Convert.ToBoolean(dr[3]);
            DXC.Checked = Convert.ToBoolean(dr[4]);
            consultar_fac.Checked = Convert.ToBoolean(dr[5]);
            consultar_nc.Checked = Convert.ToBoolean(dr[6]);
            consultar_cp.Checked = Convert.ToBoolean(dr[7]);
            consultar_ret.Checked = Convert.ToBoolean(dr[8]);
            consultar_nom.Checked = Convert.ToBoolean(dr[9]);
            consultar_cont.Checked = Convert.ToBoolean(dr[10]);
            rep_gral.Checked = Convert.ToBoolean(dr[11]);
            TOPComp.Text = dr[12].ToString();
            CancComp.Checked = Convert.ToBoolean(dr[13]);
            crear_fac.Checked = Convert.ToBoolean(dr[14]);
            crear_nc.Checked = Convert.ToBoolean(dr[15]);
            crear_cp.Checked = Convert.ToBoolean(dr[16]);
            crear_ret.Checked = Convert.ToBoolean(dr[17]);
            crear_nom.Checked = Convert.ToBoolean(dr[18]);
            crear_cont.Checked = Convert.ToBoolean(dr[19]);
            edit_comp.Checked = Convert.ToBoolean(dr[20]);
            Cliente.Checked = Convert.ToBoolean(dr[21]);
            Empleado.Checked = Convert.ToBoolean(dr[22]);
            _Roles.Checked = Convert.ToBoolean(dr[23]);
            EditEmisor.Checked = Convert.ToBoolean(dr[24]);
            EditPtoEmi.Checked = Convert.ToBoolean(dr[26]);
            EditInfoGeneral.Checked = Convert.ToBoolean(dr[27]);
            EditSMTP.Checked = Convert.ToBoolean(dr[28]);
            EditMensajes.Checked = Convert.ToBoolean(dr[29]);
            EditUserOpera.Checked = Convert.ToBoolean(dr[30]);
            EditPerfil.Checked = Convert.ToBoolean(dr[32]);
            EnvioEmail.Checked = Convert.ToBoolean(dr[33]);
            recepcion.Checked = Convert.ToBoolean(dr[35]);
            arch.Checked = Convert.ToBoolean(dr[37]);
            imp.Checked = Convert.ToBoolean(dr[38]);
            EditReceptor.Checked = Convert.ToBoolean(dr[39]);
            conc.Checked = Convert.ToBoolean(dr[40]);
            validacionRecepcion.Checked = Convert.ToBoolean(dr[41]);
            _db.Desconectar();
        }

        /// <summary>
        /// Cleans the data.
        /// </summary>
        private void CleanData()
        {
            tbNombrePerfil.Text = "";
            consultar_fac_propias.Checked = false;
            consultar_todas_fac.Checked = false;
            VerCanc.Checked = false;
            DXC.Checked = false;
            consultar_fac.Checked = false;
            consultar_nc.Checked = false;
            consultar_cp.Checked = false;
            consultar_ret.Checked = false;
            consultar_nom.Checked = false;
            consultar_cont.Checked = false;
            rep_gral.Checked = false;
            TOPComp.Text = "";
            CancComp.Checked = false;
            crear_fac.Checked = false;
            crear_nc.Checked = false;
            crear_cp.Checked = false;
            crear_ret.Checked = false;
            crear_nom.Checked = false;
            crear_cont.Checked = false;
            edit_comp.Checked = false;
            Cliente.Checked = false;
            Empleado.Checked = false;
            _Roles.Checked = false;
            EditEmisor.Checked = false;
            EditPtoEmi.Checked = false;
            EditInfoGeneral.Checked = false;
            EditSMTP.Checked = false;
            EditMensajes.Checked = false;
            EditUserOpera.Checked = false;
            EditPerfil.Checked = false;
            EnvioEmail.Checked = false;
            recepcion.Checked = false;
            arch.Checked = false;
            imp.Checked = false;
            EditReceptor.Checked = false;
            conc.Checked = false;
            validacionRecepcion.Checked = false;
        }

        /// <summary>
        /// Handles the Click event of the bGuardar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">El nombre del perfil no puede estar vacío.</exception>
        /// <exception cref="System.Exception">El nombre del perfil no puede estar vacío.</exception>
        protected void bGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbNombrePerfil.Text))
                {
                    throw new Exception("El nombre del perfil no puede estar vacío.");
                }
                var value = ddlPerfil.SelectedValue;
                var getComps = new string[] {
                    (consultar_fac.Checked ? "01" : ""),
                    (consultar_nc.Checked ? "04" : ""),
                    (consultar_cp.Checked ? "06" : ""),
                    (consultar_ret.Checked ? "07" : ""),
                    (consultar_nom.Checked ? "08" : ""),
                    (consultar_cont.Checked ? "09" : "")
                };
                var newComps = new string[] {
                    (crear_fac.Checked ? "01" : ""),
                    (crear_nc.Checked ? "04" : ""),
                    (crear_cp.Checked ? "06" : ""),
                    (crear_ret.Checked ? "07" : ""),
                    (crear_nom.Checked ? "08" : ""),
                    (crear_cont.Checked ? "09" : "")
                };
                var cnsComp = string.Join(",", (from comp in getComps where !string.IsNullOrEmpty(comp) select comp));
                var crearNewComp = string.Join(",", (from comp in newComps where !string.IsNullOrEmpty(comp) select comp));
                if (!string.IsNullOrEmpty(TOPComp.Text.Trim()))
                {
                    var topeMaximo = 1000;
                    var top = 0;
                    int.TryParse(TOPComp.Text.Trim(), out top);
                    if (top > topeMaximo)
                    {
                        throw new Exception($"El tope de consulta de documentos debe ser menor o igual a {topeMaximo}.");
                    }
                }
                if (value.Equals("00"))
                {
                    if (!ValidarRol(tbNombrePerfil.Text))
                    {
                        _db.Conectar();
                        _db.CrearComandoProcedimiento("PA_insertar_rol");
                        _db.AsignarParametroProcedimiento("@Descripcion", DbType.String, tbNombrePerfil.Text);
                        _db.AsignarParametroProcedimiento("@consultar_fac_propias", DbType.Byte, Convert.ToByte(consultar_fac_propias.Checked));
                        _db.AsignarParametroProcedimiento("@consultar_todas_fac", DbType.Byte, Convert.ToByte(consultar_todas_fac.Checked));
                        _db.AsignarParametroProcedimiento("@CrearNewComp", DbType.String, crearNewComp);
                        _db.AsignarParametroProcedimiento("@EditComp0N", DbType.Byte, Convert.ToByte(edit_comp.Checked));
                        _db.AsignarParametroProcedimiento("@ReporteGeneral", DbType.Byte, Convert.ToByte(rep_gral.Checked));
                        _db.AsignarParametroProcedimiento("@Cliente", DbType.Byte, Convert.ToByte(Cliente.Checked));
                        _db.AsignarParametroProcedimiento("@Empleado", DbType.Byte, Convert.ToByte(Empleado.Checked));
                        _db.AsignarParametroProcedimiento("@Roles", DbType.Byte, Convert.ToByte(_Roles.Checked));
                        _db.AsignarParametroProcedimiento("@EditEmisor", DbType.Byte, Convert.ToByte(EditEmisor.Checked));
                        _db.AsignarParametroProcedimiento("@EditEstab", DbType.Byte, Convert.ToByte(1));
                        _db.AsignarParametroProcedimiento("@EditPtoEmi", DbType.Byte, Convert.ToByte(EditPtoEmi.Checked));
                        _db.AsignarParametroProcedimiento("@EditInfoGeneral", DbType.Byte, Convert.ToByte(EditInfoGeneral.Checked));
                        _db.AsignarParametroProcedimiento("@EditSMTP", DbType.Byte, Convert.ToByte(EditSMTP.Checked));
                        _db.AsignarParametroProcedimiento("@EditMensajes", DbType.String, Convert.ToByte(EditMensajes.Checked));
                        _db.AsignarParametroProcedimiento("@EditUserOpera", DbType.String, Convert.ToByte(EditUserOpera.Checked));
                        _db.AsignarParametroProcedimiento("@LimpiarLogs", DbType.String, Convert.ToByte(1));
                        _db.AsignarParametroProcedimiento("@EditPerfil", DbType.String, Convert.ToByte(EditPerfil.Checked));
                        _db.AsignarParametroProcedimiento("@EnvioEmail", DbType.String, Convert.ToByte(EnvioEmail.Checked));
                        _db.AsignarParametroProcedimiento("@eliminado", DbType.Byte, false);
                        _db.AsignarParametroProcedimiento("@TOPComp", DbType.String, TOPComp.Text);
                        _db.AsignarParametroProcedimiento("@recepcion", DbType.Byte, Convert.ToByte(recepcion.Checked));
                        _db.AsignarParametroProcedimiento("@Visible", DbType.Byte, Convert.ToByte(true));
                        _db.AsignarParametroProcedimiento("@CNSComp", DbType.String, cnsComp);
                        _db.AsignarParametroProcedimiento("@imp", DbType.Byte, Convert.ToByte(imp.Checked));
                        _db.AsignarParametroProcedimiento("@arch", DbType.Byte, Convert.ToByte(arch.Checked));
                        _db.AsignarParametroProcedimiento("@EditReceptor", DbType.Byte, Convert.ToByte(EditReceptor.Checked));
                        _db.AsignarParametroProcedimiento("@CancComp", DbType.Byte, Convert.ToByte(CancComp.Checked));
                        _db.AsignarParametroProcedimiento("@VerCanc", DbType.Byte, Convert.ToByte(VerCanc.Checked));
                        _db.AsignarParametroProcedimiento("@DXC", DbType.Byte, Convert.ToByte(DXC.Checked));
                        _db.AsignarParametroProcedimiento("@conc", DbType.Byte, Convert.ToByte(conc.Checked));
                        _db.AsignarParametroProcedimiento("@validacionRecepcion", DbType.Byte, Convert.ToByte(validacionRecepcion.Checked));
                        _db.EjecutarConsulta1();
                        _db.Desconectar();
                        (Master as SiteMaster).MostrarAlerta(this, "El perfil se creo correctamente.", 2, null);
                    }
                    else
                    {
                        (Master as SiteMaster).MostrarAlerta(this, "El perfil ya existe.", 4, null);
                    }
                }
                else
                {
                    const string sql = @"UPDATE [dbo].[Cat_Roles]
                               SET [descripcion] = @descripcion
                                  ,[consultar_fac_propias] = @consultar_fac_propias
                                  ,[consultar_todas_fac] = @consultar_todas_fac
                                  ,[CrearNewComp] = @CrearNewComp
                                  ,[CNSComp] = @CNSComp
                                  ,[EditComp0N] = @EditComp0N
                                  ,[ReporteGeneral] = @ReporteGeneral
                                  ,[Cliente] = @Cliente
                                  ,[Empleado] = @Empleado
                                  ,[Roles] = @Roles
                                  ,[EditEmisor] = @EditEmisor
                                  ,[EditEstab] = @EditEstab
                                  ,[EditPtoEmi] = @EditPtoEmi
                                  ,[EditInfoGeneral] = @EditInfoGeneral
                                  ,[EditSMTP] = @EditSMTP
                                  ,[EditMensajes] = @EditMensajes
                                  ,[EditUserOpera] = @EditUserOpera
                                  ,[LimpiarLogs] = @LimpiarLogs
                                  ,[EditPerfil] = @EditPerfil
                                  ,[EnvioEmail] = @EnvioEmail
                                  ,[eliminado] = @eliminado
                                  ,[TOPComp] = @TOPComp
                                  ,[recepcion] = @recepcion
                                  ,[Visible] = @Visible
                                  ,[arch] = @arch
                                  ,[imp] = @imp
                                  ,[EditReceptor] = @EditReceptor
                                  ,[CancComp] = @CancComp
                                  ,[VerCanc] = @VerCanc
                                  ,[DXC] = @DXC
                                  ,[conc] = @conc
                                  ,[validacionRecepcion] = @validacionRecepcion
                             WHERE idRol = @idEditar";
                    _db.Conectar();
                    _db.CrearComando(sql);
                    _db.AsignarParametroCadena("@descripcion", tbNombrePerfil.Text);
                    _db.AsignarParametroCadena("@consultar_fac_propias", Convert.ToByte(consultar_fac_propias.Checked).ToString());
                    _db.AsignarParametroCadena("@consultar_todas_fac", Convert.ToByte(consultar_todas_fac.Checked).ToString());
                    _db.AsignarParametroCadena("@CrearNewComp", crearNewComp);
                    _db.AsignarParametroCadena("@CNSComp", cnsComp);
                    _db.AsignarParametroCadena("@EditComp0N", Convert.ToByte(edit_comp.Checked).ToString());
                    _db.AsignarParametroCadena("@ReporteGeneral", Convert.ToByte(rep_gral.Checked).ToString());
                    _db.AsignarParametroCadena("@Cliente", Convert.ToByte(Cliente.Checked).ToString());
                    _db.AsignarParametroCadena("@Empleado", Convert.ToByte(Empleado.Checked).ToString());
                    _db.AsignarParametroCadena("@Roles", Convert.ToByte(_Roles.Checked).ToString());
                    _db.AsignarParametroCadena("@EditEmisor", Convert.ToByte(EditEmisor.Checked).ToString());
                    _db.AsignarParametroCadena("@EditEstab", Convert.ToByte(1).ToString());
                    _db.AsignarParametroCadena("@EditPtoEmi", Convert.ToByte(EditPtoEmi.Checked).ToString());
                    _db.AsignarParametroCadena("@EditInfoGeneral", Convert.ToByte(EditInfoGeneral.Checked).ToString());
                    _db.AsignarParametroCadena("@EditSMTP", Convert.ToByte(EditSMTP.Checked).ToString());
                    _db.AsignarParametroCadena("@EditMensajes", Convert.ToByte(EditMensajes.Checked).ToString());
                    _db.AsignarParametroCadena("@EditUserOpera", Convert.ToByte(EditUserOpera.Checked).ToString());
                    _db.AsignarParametroCadena("@LimpiarLogs", Convert.ToByte(1).ToString());
                    _db.AsignarParametroCadena("@EditPerfil", Convert.ToByte(EditPerfil.Checked).ToString());
                    _db.AsignarParametroCadena("@EnvioEmail", Convert.ToByte(EnvioEmail.Checked).ToString());
                    _db.AsignarParametroCadena("@eliminado", "0");
                    _db.AsignarParametroCadena("@TOPComp", TOPComp.Text);
                    _db.AsignarParametroCadena("@recepcion", Convert.ToByte(recepcion.Checked).ToString());
                    _db.AsignarParametroCadena("@Visible", Convert.ToByte(true).ToString());
                    _db.AsignarParametroCadena("@arch", Convert.ToByte(arch.Checked).ToString());
                    _db.AsignarParametroCadena("@imp", Convert.ToByte(imp.Checked).ToString());
                    _db.AsignarParametroCadena("@EditReceptor", Convert.ToByte(EditReceptor.Checked).ToString());
                    _db.AsignarParametroCadena("@CancComp", Convert.ToByte(CancComp.Checked).ToString());
                    _db.AsignarParametroCadena("@VerCanc", Convert.ToByte(VerCanc.Checked).ToString());
                    _db.AsignarParametroCadena("@DXC", Convert.ToByte(DXC.Checked).ToString());
                    _db.AsignarParametroCadena("@conc", Convert.ToByte(conc.Checked).ToString());
                    _db.AsignarParametroCadena("@validacionRecepcion", Convert.ToByte(validacionRecepcion.Checked).ToString());
                    _db.AsignarParametroCadena("@idEditar", value);
                    _db.EjecutarConsulta1();
                    _db.Desconectar();
                    (Master as SiteMaster).MostrarAlerta(this, "El perfil se actualizó correctamente.", 2, null);
                }
                RebindDdl();
            }
            catch (Exception ex)
            {
                _db.Desconectar();
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo guardar el perfil." + Environment.NewLine + ex.Message, 4, null);
            }

        }

        /// <summary>
        /// Rebinds the DDL.
        /// </summary>
        private void RebindDdl()
        {
            SqlDataPerfiles.DataBind();
            ddlPerfil.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the bEliminar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var value = ddlPerfil.SelectedValue;
                _db.Conectar();
                _db.CrearComando(@"UPDATE Cat_Roles SET eliminado = 'true' WHERE (idRol = @idRol)");
                _db.AsignarParametroCadena("@idRol", value);
                _db.EjecutarConsulta();
                _db.Desconectar();
                RebindDdl();
                (Master as SiteMaster).MostrarAlerta(this, "Perfil eliminado correctamente", 2, null);
            }
            catch (Exception ex)
            {
                _db.Desconectar();
                (Master as SiteMaster).MostrarAlerta(this, "No se pudo eliminar el perfil:" + Environment.NewLine + ex.Message, 4, null);
            }
        }
    }
}