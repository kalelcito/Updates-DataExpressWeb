// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 01-30-2017
// ***********************************************************************
// <copyright file="roles.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Datos;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using Datos;
using System.Data.SqlClient;

// ReSharper disable once CheckNamespace
namespace Administracion
{
    /// <summary>
    /// Class Roles.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />

    public partial class Grupos : Page
    {
        /// <summary>
        /// The files
        /// </summary>
        private static Dictionary<int, HttpPostedFile[]> _files;
        /// <summary>
        /// The _log
        /// </summary>
        private Control.Log _log;//
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
        /// The tabla grupos
        /// </summary>
        DataTable TablaGrupos = new DataTable("TablaGrupos");
        /// <summary>
        /// The registro
        /// </summary>
        string[] registro;
        /// <summary>
        /// The array_registros
        /// </summary>
        static ArrayList Array_registros = new ArrayList();
        /// <summary>
        /// The Array_Grupo_Eliminar
        /// </summary>
        static ArrayList Array_Grupo_Eliminar = new ArrayList();
        /// <summary>
        /// The id_ renglon
        /// </summary>
        static int id_Renglon = 0;
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
                    _idUser = Session["idUser"].ToString();
                }

                llenarGridViewBD();

                if (!IsPostBack)
                {
                    _files = new Dictionary<int, HttpPostedFile[]>();
                    Array_registros.Clear();
                    Session["GROUPEDIT"] = "";
                    id_Renglon = 0;
                }
                else
                {
                    CreateControls();
                }
            }
        }

        /// <summary>
        /// Creates the controls.
        /// </summary>
        private void CreateControls()
        {
            crear_tabla_Grupos();
        }

        /// <summary>
        /// Crears the tabla grupos.
        /// </summary>
        private void crear_tabla_Grupos()
        {
            if (TablaGrupos.Columns.Count == 0)
            {
                TablaGrupos.Columns.Add(new DataColumn("No", typeof(string)));
                TablaGrupos.Columns.Add(new DataColumn("Descripcion", typeof(string)));
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
        /// Handles the DataBound event of the gvRegistros control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvRegistros_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 0)
            {
                //Ocultar celdas de No de renglon
                e.Row.Cells[2].Visible = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEliminar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void btnEliminar_Click(object sender, GridViewCommandEventArgs e)
        {
            ArrayList Array_registros_aux = (ArrayList)Array_registros.Clone();
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "SelectRow")
            {
                Array_registros_aux = (ArrayList)Array_registros.Clone();
                foreach (string[] registro in Array_registros)
                {
                    if (registro[0].ToString() == index.ToString())
                    {
                        Array_Grupo_Eliminar.Add(registro);
                        Array_registros_aux.Remove(registro);
                    }
                }
            }
            crear_tabla_Grupos();
            Array_registros = Array_registros_aux;
            if (Array_registros.Count > 0)
            {
                foreach (string[] registro in Array_registros)
                {
                    DataRow row = TablaGrupos.NewRow();
                    row["No"] = registro[0];
                    row["Descripcion"] = registro[1];

                    TablaGrupos.Rows.Add(row);
                    gvRegistros.DataSource = TablaGrupos;
                    gvRegistros.DataBind();

                    Controls_setVisible(true, true, true, true, true, false, false, true, false);

                }
            }
            else
            {
                gvRegistros.DataBind();
                ButtonGenerarGrupos.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the LnkBAgregarGrupo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void LnkBAgregarGrupo_Click(object sender, EventArgs e)
        {
            if (!TextboxGrupo.Text.Equals(""))
            {
                registro = new string[2];
                registro[0] = id_Renglon.ToString();
                id_Renglon++;
                registro[1] = TextboxGrupo.Text;
                Array_registros.Add(registro);
                Agregar_registro_gridView(Array_registros);
            }

            TextboxGrupo.Text = "";
            Controls_setVisible(true, true, true, true, true, false, false, (string)Session["GROUPEDIT"] == "" ? false : true, false);
        }

        /// <summary>
        /// Agregars the registro grid view.
        /// </summary>
        /// <param name="Array_registros">The array registros.</param>
        public void Agregar_registro_gridView(ArrayList Array_registros)
        {
            crear_tabla_Grupos();
            if (Array_registros.Count > 0)
            {
                foreach (string[] registro in Array_registros)
                {
                    DataRow row = TablaGrupos.NewRow();
                    row["No"] = registro[0];
                    row["Descripcion"] = registro[1];

                    TablaGrupos.Rows.Add(row);
                    gvRegistros.DataSource = TablaGrupos;
                    gvRegistros.DataBind();
                }
            }
        }


        /// <summary>
        /// Handles the Click event of the ButtonGenerarGrupos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ButtonGenerarGrupos_Click(object sender, EventArgs e)
        {
            int[] numeros = new int[gvRegistros.Rows.Count];
            int cont = 0;
            foreach (GridViewRow row in gvRegistros.Rows)
            {
                var orden = (System.Web.UI.WebControls.TextBox)row.Cells[0].FindControl("tbOrden");
                string valor4 = row.Cells[3].Text;
                numeros[cont] = orden.Text == "" ? 0 : Convert.ToInt32(orden.Text);
                cont++;
            }

            Array.Sort(numeros);

            bool ord = numeros[0] == 1 ? true : false;

            if (numeros.Length == 1)
            {
                if (numeros[0] != 1)
                    ord = false;
            }
            else
            {
                if (numeros[0] == 1)
                {
                    for (int a = 0; a < numeros.Length; a++)
                    {
                        if (numeros[a] > 0)
                        {
                            if (a < numeros.Length - 1)
                            {
                                if (!(numeros[a] + 1 == numeros[a + 1]))
                                {
                                    ord = false;
                                }
                            }
                        }
                        else
                        {
                            ord = false;
                        }
                    }
                }
            }

            if (ord)
            {
                if (string.IsNullOrEmpty(((string)Session["GROUPEDIT"])))
                {
                    foreach (GridViewRow row in gvRegistros.Rows)
                    {
                        _dbr.Conectar();
                        var orden = (System.Web.UI.WebControls.TextBox)row.Cells[0].FindControl("tbOrden");
                        string descripcion = row.Cells[3].Text;
                        string ordn = orden.Text;
                        _dbr.CrearComando(@"INSERT INTO Cat_Grupos_validadores (descripcion,orden) values (@descripcion,@orden)");
                        _dbr.AsignarParametroCadena("@descripcion", descripcion);
                        _dbr.AsignarParametroCadena("@orden", ordn);
                        _dbr.EjecutarConsulta();
                        _dbr.Desconectar();
                    }
                }
                else
                {
                    foreach (GridViewRow row in gvRegistros.Rows)
                    {
                        string id_inserted = "";
                        _dbr.Conectar();
                        var orden = (System.Web.UI.WebControls.TextBox)row.Cells[0].FindControl("tbOrden");
                        string descripcion = row.Cells[3].Text;
                        string ordn = orden.Text;
                        _dbr.CrearComando(@"UPDATE Cat_Grupos_validadores SET orden=@orden OUTPUT inserted.idGrupo WHERE descripcion=@descripcion");
                        _dbr.AsignarParametroCadena("@descripcion", descripcion);
                        _dbr.AsignarParametroCadena("@orden", ordn);
                        var dr = _dbr.EjecutarConsulta();
                        if (dr.Read())
                        {
                            id_inserted = dr[0].ToString();
                        }
                        _dbr.Desconectar();

                        if (id_inserted == "")
                        {
                            _dbr.Conectar();
                            _dbr.CrearComando(@"INSERT INTO Cat_Grupos_validadores (descripcion,orden) values (@descripcion,@orden)");
                            _dbr.AsignarParametroCadena("@descripcion", descripcion);
                            _dbr.AsignarParametroCadena("@orden", ordn);
                            _dbr.EjecutarConsulta();
                            _dbr.Desconectar();
                        }
                    }
                }
                Eliminar_registro_Grupo(Array_Grupo_Eliminar);
                Session["GROUPEDIT"] = "";

                llenarGridViewBD();

                Controls_setVisible(false, false, false, false, false, true, true, false, false);
            }
            else
            {
                tbMensaje.Text = "Los grupos deben ser numerados de 1 en adelante de acuerdo a la prioridad";
                Controls_setVisible(true, true, true, true, true, false, false, true, true);
            }
        }

        /// <summary>
        /// Llenars the grid view bd.
        /// </summary>
        public void llenarGridViewBD()
        {
            _dbr.Conectar();
            _dbr.CrearComando(@"SELECT descripcion as Descripción,orden as Orden
  FROM 
 Cat_Grupos_validadores
  order by orden asc");
            var dataReader = _dbr.EjecutarConsulta();
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            gvResistrosDB.DataSource = dataTable;
            gvResistrosDB.DataBind();

            _dbr.Desconectar();
            if (gvResistrosDB.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty((string)Session["GROUPEDIT"]))
                {
                    Controls_setVisible(false, false, false, false, false, true, true, false, false);
                }
                else
                {
                    Controls_setVisible(true, true, true, true, true, true, true, true, false);
                }
            }
        }
        /// <summary>
        /// Handles the Click event of the ButtonEditarGrupos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ButtonEditarGrupos_Click(object sender, EventArgs e)
        {
            Session["GROUPEDIT"] = "true";
            Array_registros.Clear();
            _dbr.Conectar();
            _dbr.CrearComando(@"SELECT descripcion as DESCRIPCION,orden as ORDEN
  FROM 
 Cat_Grupos_validadores
  order by orden asc");
            var dataReader = _dbr.EjecutarConsulta();
            while (dataReader.Read())
            {
                registro = new string[3];
                registro[0] = id_Renglon.ToString();
                id_Renglon++;
                registro[1] = dataReader[0].ToString();
                registro[2] = dataReader[1].ToString();
                Array_registros.Add(registro);
            }
            _dbr.Desconectar();

            Agregar_registro_gridView(Array_registros);


            // var rows = gvRegistros.Rows;
            // foreach (GridViewRow row in rows)
            // {
            //     TextBox tb = new TextBox();
            //     tb.Text = "1";
            //     string a = row.Cells[3].Text;
            //}

            Controls_setVisible(true, true, true, true, true, false, false, true);

        }
        /// <summary>
        /// Handles the Click event of the ButtonCancelarGrupos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ButtonCancelarGrupos_Click(object sender, EventArgs e)
        {
            Controls_setVisible(false, false, false, false, false, true, true, false);

        }

        /// <summary>
        /// Controlses the set visible.
        /// </summary>
        /// <param name="gvRegitros">if set to <c>true</c> [gv regitros].</param>
        /// <param name="btnGenerarGrupo">if set to <c>true</c> [BTN generar grupo].</param>
        /// <param name="TextboxGrupoV">if set to <c>true</c> [textbox grupo v].</param>
        /// <param name="labelGrrupo">if set to <c>true</c> [label grrupo].</param>
        /// <param name="linkBagregarReg">if set to <c>true</c> [link bagregar reg].</param>
        /// <param name="GVRegistrosBD">if set to <c>true</c> [gv registros bd].</param>
        /// <param name="botonEditar">if set to <c>true</c> [boton editar].</param>
        /// <param name="BotonCancelar">if set to <c>true</c> [boton cancelar].</param>
        /// <param name="textboxMensaje">if set to <c>true</c> [textbox mensaje].</param>
        public void Controls_setVisible(bool gvRegitros = false, bool btnGenerarGrupo = false, bool TextboxGrupoV = false, bool
            labelGrrupo = false, bool linkBagregarReg = false, bool GVRegistrosBD = false, bool botonEditar = false, bool BotonCancelar = false, bool textboxMensaje = false)
        {
            gvRegistros.Visible = gvRegitros;
            ButtonGenerarGrupos.Visible = btnGenerarGrupo;
            TextboxGrupo.Visible = TextboxGrupoV;
            lbGrupo.Visible = labelGrrupo;
            LnkBAgregarGrupo.Visible = linkBagregarReg;
            gvResistrosDB.Visible = GVRegistrosBD;
            ButtonEditarGrupos.Visible = botonEditar;
            ButtonCancelarGrupos.Visible = BotonCancelar;
            tbMensaje.Visible = textboxMensaje;
        }

        /// <summary>
        /// Eliminars the registro grupo.
        /// </summary>
        /// <param name="Grupos">The grupos.</param>
        public void Eliminar_registro_Grupo(ArrayList Grupos)
        {
            if (Grupos.Count > 0)
            {
                try
                {
                    foreach (string[] grupo in Grupos)
                    {
                        _dbr.Conectar();
                        _dbr.CrearComando(@"DELETE Cat_Grupos_validadores WHERE descripcion=@descripcion AND orden=@orden");
                        _dbr.AsignarParametroCadena("@descripcion", grupo[1].ToString());
                        _dbr.AsignarParametroCadena("@orden", grupo[2].ToString());
                        _dbr.EjecutarConsulta();
                        _dbr.Desconectar();
                    }
                }
                catch (Exception) { }
            }
            Array_Grupo_Eliminar.Clear();
        }
    }
}