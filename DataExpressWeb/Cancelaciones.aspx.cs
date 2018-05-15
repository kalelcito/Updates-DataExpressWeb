// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 01-30-2017
// ***********************************************************************
// <copyright file="Cancelaciones.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using Control;
using System.IO;

namespace DataExpressWeb
{
    /// <summary>
    /// Class Cancelaciones.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Cancelaciones : System.Web.UI.Page
    {
        /// <summary>
        /// The _chkbox select all
        /// </summary>
        private static CheckBox _chkboxSelectAll;
        /// <summary>
        /// The _consulta
        /// </summary>
        private string _consulta;
        /// <summary>
        /// The _aux
        /// </summary>
        private string _aux;
        /// <summary>
        /// The _separador
        /// </summary>
        private string _separador = "|";
        /// <summary>
        /// The _DT
        /// </summary>
        private DataTable _dt = new DataTable();
        /// <summary>
        /// The _DB
        /// </summary>
        private static BasesDatos _db = new BasesDatos();
        /// <summary>
        /// The _id user
        /// </summary>
        private string _idUser = "";
        /// <summary>
        /// The _DT actual
        /// </summary>
        private DataTable _dtActual = new DataTable();


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            //tbRFC.Attributes.Add("onkeyup", "changeToUpperCase(this.id)");
            //tbRFC.Attributes.Add("onchange", "changeToUpperCase(this.id)");
            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                SqlDataSource1.ConnectionString = _db.CadenaConexion;
                SqlDataSource2.ConnectionString = _db.CadenaConexion;
                SqlDataSourcePtoEmi.ConnectionString = _db.CadenaConexion;
                SqlDataSourcePtoEmiCliente.ConnectionString = _db.CadenaConexion;
                //SqlDataSourceSucursales.ConnectionString = DB.cadenaConexion;
                SqlDataSourceTipoDoc.ConnectionString = _db.CadenaConexion;
            }
            //log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));

            if (!IsPostBack)
            {
                if (Session["idUser"] != null)
                {
                    _idUser = Session["idUser"].ToString();
                    Session["gvFacturas"] = null;
                    _dtActual = new DataTable();
                    //    if (Convert.ToBoolean(Session["facturasManuales"])) { HyperLink8.Visible = true; } else { HyperLink8.Visible = false; }
                    //    if (Convert.ToBoolean(Session["facturasManuales"])) { HyperLink1.Visible = true; } else { HyperLink1.Visible = false; }
                    //    if (Convert.ToBoolean(Session["facturasManuales"])) { HyperLink3.Visible = true; } else { HyperLink3.Visible = false; }
                    //    if (Convert.ToBoolean(Session["retencionesManuales"])) { HyperLink4.Visible = true; } else { HyperLink4.Visible = false; }
                    //    if (Convert.ToBoolean(Session["reporteIndividual"])) { HyperLink8.Visible = true; } else { HyperLink8.Visible = false; }
                    //    if (Convert.ToBoolean(Session["asRoles"])) { hlNA.Visible = true; } else { hlNA.Visible = false; }


                    if (Convert.ToBoolean(Session["coFactPropias"]) || Convert.ToBoolean(Session["coFactTodas"]))
                    {
                        Buscar();
                        if (string.IsNullOrEmpty((string)Session["rfcCliente"]))
                        {
                            //ddlSucursal.Visible = false;
                            //    lSucursal.Visible = false;
                            //bMail.Visible = false;
                            // lSeleccionDocus.Visible = false;
                            _consulta = "";
                        }
                        if (Session["rolUser"].ToString() == "3")
                        {
                            _consulta = "";
                        }
                        var idComprobante = Request.QueryString.Get("comprobante");
                        if (!string.IsNullOrEmpty(idComprobante))
                        {

                        }
                    }
                    this.LlenarGrid();
                    _dtActual = Session["gvComprobantes"] as DataTable;
                    gvFacturas.DataSource = _dtActual;
                    gvFacturas.DataBind();
                    lCount.Text = "<strong>Se encontraron <span class='badge'>" + _dtActual.Rows.Count + "</span> Registros</strong>";
                }
            }
            if (Session["IsCliente"] != null)
            {
                rowFiltroRecep.Style["display"] = "none";
            }
        }

        /// <summary>
        /// Handles the Click event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlSucursal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPtoEmi.Items.Clear();
            ddlPtoEmi.Items.Add(new ListItem("Todos", "0"));
            ddlPtoEmi.DataBind();
            ddlPtoEmi.SelectedValue = "0";
        }

        /// <summary>
        /// Buscars this instance.
        /// </summary>
        private void Buscar()
        {
            if (ddlTipoDocumento.SelectedValue.Equals("10"))
            {
                Response.Redirect("~/recepcion/Documentos.aspx", true);
            }
            var fecha = 0;
            _consulta = "";
            _dt.Clear();
            if (tbFolioAnterior.Text.Length != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "FA" + tbFolioAnterior.Text + _separador; }
                else { _consulta = "FA" + tbFolioAnterior.Text + _separador; }
            }
            if (tbNombre.Text.Length != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "RS" + tbNombre.Text + _separador; }
                else { _consulta = "RS" + tbNombre.Text + _separador; }
            }
            if (tbControl.Text.Length != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "CF" + tbControl.Text + _separador; }
                else { _consulta = "CF" + tbControl.Text + _separador; }
            }

            if (tbRFC.Text.Length != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "RF" + tbRFC.Text + _separador; }
                else { _consulta = "RF" + tbRFC.Text + _separador; }
            }

            if (ddlTipoDocumento.SelectedIndex != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "TD" + ddlTipoDocumento.SelectedValue + _separador; }
                else { _consulta = "TD" + ddlTipoDocumento.SelectedValue + _separador; }
            }
            if (ddlEstado.SelectedIndex != 0)
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "ED" + ddlEstado.SelectedValue + _separador; }
                else { _consulta = "ED" + ddlEstado.SelectedValue + _separador; }
            }
            if (!ddlPtoEmi.SelectedValue.Equals("0"))
            {
                if (_consulta.Length != 0) { _consulta = _consulta + "PO" + ddlPtoEmi.SelectedItem.Text + _separador; }
                else { _consulta = "PO" + ddlPtoEmi.SelectedItem.Text + _separador; }
            }
            if (tbFechaInicial.Text.Length > 5 && tbFechaFinal.Text.Length > 5)
            {
                if (tbFechaInicial.Text.Length > 5)
                {
                    if (_consulta.Length != 0) { _consulta = _consulta + "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + _separador; }
                    else { _consulta = "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + _separador; }
                    fecha = 1;
                }

                if (tbFechaFinal.Text.Length > 5)
                {
                    if (_consulta.Length != 0) { _consulta = _consulta + "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + _separador; }
                    else { _consulta = "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + _separador; }
                    fecha = fecha + 1;
                }
            }
            if (_consulta.Length > 0)
            {
                if (fecha > 1)
                {
                    _consulta = _consulta.Substring(0, _consulta.Length - 1);
                }
                if (fecha == 1)
                {
                }
            }
            else
            {
                _consulta = "-";
            }
            _consulta = _consulta.Trim('|');

            #region Procedimiento Almacenado Nuevo
            if (IsPostBack)
            {
                this.LlenarGrid();
                _dtActual = Session["gvComprobantes"] as DataTable;
                gvFacturas.DataSource = _dtActual;
                gvFacturas.DataBind();
            }
            #endregion

            lCount.Visible = true;
            //lCount.Text = "Registros encontrados: " + gvFacturas.Rows.Count;
            _consulta = "";
        }

        /// <summary>
        /// Handles the Click1 event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click1(object sender, EventArgs e)
        {
            tbFolioAnterior.Attributes["Text"] = "";
            tbFolioAnterior.Text = "";
            ddlTipoDocumento.SelectedValue = "0";
            tbFechaInicial.Text = "";
            tbFechaFinal.Text = "";
            ddlEstado.SelectedIndex = 0;
            tbRFC.Text = "";
            tbNombre.Text = "";
            Buscar();
        }

        /// <summary>
        /// Limpiars the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Limpiar(object sender, EventArgs e)
        {
            tbFolioAnterior.Text = "";
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.LoadComplete" /> event at the end of the page load stage.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //DataFilter11.BeginFilter();
                //DataFilter11.AddNewFilter("SECUENCIA", "LIKE", "060%");
            }
        }

        //void DataFilter1_OnFilterAdded()
        //{
        //    try
        //    {
        //        DataFilter11.FilterSessionID = "Cancelaciones.aspx";
        //        DataFilter11.FilterDataSource();
        //        gvFacturas.DataBind();

        //    }
        //    catch (Exception e)
        //    {
        //    }
        //}

        /// <summary>
        /// Handles the PageIndexChanged event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_PageIndexChanged(object sender, EventArgs e)
        {
            //DataFilter1_OnFilterAdded();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataFilter1_OnFilterAdded();
            //var row = gvFacturas.SelectedRow;
            //var img = (ImageButton)row.Cells[0].Controls[0];
            //if (img != null)
            //{
            //    img.ImageUrl = "~/imagenes/loading.gif";
            //}
        }

        /// <summary>
        /// Handles the CheckedChanged event of the check control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void check_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTipoDocumento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Selected event of the SqlDataSource1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlDataSourceStatusEventArgs" /> instance containing the event data.</param>
        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            lCount.Visible = true;
            lCount.Text = "Se encontraron <span class='badge'>" + e.AffectedRows + "</span> Registros ";
        }

        /// <summary>
        /// Handles the Click event of the bBuscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        /// <summary>
        /// Handles the Click3 event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Button1_Click3(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the Form control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Form_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Llenars the grid.
        /// </summary>
        private void LlenarGrid()
        {
            var aux2 = ""; /*var aux3 = "";*/ var tipoComp = "";
            var topCopm = "''";
            if (Session["TOPComp"] != null) { topCopm = Session["TOPComp"].ToString(); }
            if (Convert.ToBoolean(Session["coFactTodas"])) { _aux = "true"; aux2 = "false"; /*aux3 = "false";*/ } else { _aux = "false"; aux2 = "true"; }

            if (Convert.ToBoolean(Session["coFactPropias"]))
            {
                aux2 = "true";
                //if (Convert.ToBoolean(Session["coFactPropiasPtoEmi"])) { aux3 = "true"; } else { aux3 = "false"; }
            }
            else { aux2 = "false";/* aux3 = "false";*/ }
            //Session["CNSComp"]
            if (Session["CNSComp"] != null)
            {
                if (!string.IsNullOrEmpty(Session["CNSComp"].ToString()))
                {
                    if (Session["CNSComp"].ToString().Contains("01")) { tipoComp = tipoComp.Trim(',') + "'01'"; }
                    if (Session["CNSComp"].ToString().Contains("04")) { tipoComp = tipoComp.Trim(',') + ",'04'"; }
                    if (Session["CNSComp"].ToString().Contains("05")) { tipoComp = tipoComp.Trim(',') + ",'05'"; }
                    if (Session["CNSComp"].ToString().Contains("06")) { tipoComp = tipoComp.Trim(',') + ",'06'"; }
                    if (Session["CNSComp"].ToString().Contains("07")) { tipoComp = tipoComp.Trim(',') + ",'07'"; }
                    if (Session["CNSComp"].ToString().Contains("08")) { tipoComp = tipoComp.Trim(',') + ",'08'"; }
                    if (Session["CNSComp"].ToString().Contains("09")) { tipoComp = tipoComp.Trim(',') + ",'09'"; }

                    if (!string.IsNullOrEmpty(tipoComp))
                    {
                        tipoComp = "Dat_General.codDoc in (" + tipoComp.Trim(',') + ") ";
                    }
                }
            }

            _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            //log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            _db.Conectar();
            _db.CrearComandoProcedimiento("PA_facturas_basico");
            _db.AsignarParametroProcedimiento("@QUERY", System.Data.DbType.String, _consulta);
            //_db.AsignarParametroProcedimiento("@SUCURSAL", System.Data.DbType.String, Session["claveSucursalUser"].ToString());
            _db.AsignarParametroProcedimiento("@RFC", System.Data.DbType.String, Session["rfcCliente"].ToString());
            _db.AsignarParametroProcedimiento("@ROL", System.Data.DbType.Boolean, _aux);
            _db.AsignarParametroProcedimiento("@empleado", System.Data.DbType.String, Session["USERNAME"].ToString());
            _db.AsignarParametroProcedimiento("@ROLSUCURSAL", System.Data.DbType.String, aux2);
            _db.AsignarParametroProcedimiento("@TOP", System.Data.DbType.String, topCopm);
            //_db.AsignarParametroProcedimiento("@CNSptoemi", System.Data.DbType.String, aux3);
            _db.AsignarParametroProcedimiento("@PTOEMII", System.Data.DbType.String, "1");
            _db.AsignarParametroProcedimiento("@CNSComp", System.Data.DbType.String, tipoComp);
            _db.AsignarParametroProcedimiento("@QUERYSTRING", DbType.String, "Dat_General.tipo = 'C'");
            //DB.AsignarParametroProcedimiento("@PTOEMI", System.Data.DbType.String, completaceros(ptoEMi));
            _dtActual.Load(_db.EjecutarConsulta());

            if (Session["IsCliente"] != null && !IsPostBack)
            {
                ddlTipoDocumento.Items.Clear();
                ddlTipoDocumento.DataSource = null;
                ddlTipoDocumento.DataSourceID = null;
                ddlTipoDocumento.DataTextField = null;
                ddlTipoDocumento.DataValueField = null;
                ddlTipoDocumento.DataBind();
                ddlTipoDocumento.Items.Add(new ListItem("Todos", "0"));
                foreach (DataRow row in _dtActual.Rows)
                {
                    var codDoc = row["codDoc"].ToString();
                    var tipoDoc = row["TIPODOC"].ToString();
                    if (ddlTipoDocumento.Items.FindByValue(codDoc) == null)
                    {
                        ddlTipoDocumento.Items.Add(new ListItem(tipoDoc, codDoc));
                    }
                }
                ddlTipoDocumento.SelectedValue = "0";
            }

            //dtActual.Merge(dtActualContado);
            //                lMensaje.Text += "Entro CADENA_CONEXION3" + dtActual.Rows.Count;
            _db.Desconectar();


            //  lMensaje.Text += cont;
            Session["gvComprobantes"] = _dtActual;
            lCount.Text = "Se encontraron <span class='badge'>" + _dtActual.Rows.Count + "</span> Registros ";
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;
            gvFacturas.PageIndex = e.NewPageIndex;

            //gvFacturas.PageIndex = gvFacturas.PageCount;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the First event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_First(object sender, EventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;
            gvFacturas.PageIndex = 0;

            //gvFacturas.PageIndex = gvFacturas.PageCount;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Prev event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Prev(object sender, EventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;

            if (gvFacturas.PageIndex > 0)
            {
                gvFacturas.PageIndex = gvFacturas.PageIndex - 1;
            }
            //gvFacturas.PageIndex = gvFacturas.PageCount;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Next event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Next(object sender, EventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;


            if (gvFacturas.PageIndex < gvFacturas.PageCount - 1)
            {
                gvFacturas.PageIndex = gvFacturas.PageIndex + 1;
            }
            //gvFacturas.PageIndex = gvFacturas.PageCount;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Last event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Last(object sender, EventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            gvFacturas.DataSource = _dtActual;

            //gvFacturas.SetPageIndex(gvFacturas.PageCount);

            //gvFacturas.PageIndex = gvFacturas.PageIndex + 1;

            gvFacturas.PageIndex = gvFacturas.PageCount - 1;

            gvFacturas.DataBind();
        }

        /// <summary>
        /// Handles the Sorted event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_Sorted(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_SelectedIndexChanged(object sender, GridViewSortEventArgs e)
        {
            _dtActual = Session["gvComprobantes"] as DataTable;
            if (_dtActual != null)
            {
                var dataView = new DataView(_dtActual);
                if (Session["ordenar"].ToString() == "ASC")
                {
                    e.SortDirection = SortDirection.Descending;
                }
                else
                {
                    e.SortDirection = SortDirection.Ascending;
                }

                dataView.Sort = e.SortExpression + " " + ConvertSortDirection(e.SortDirection);

                gvFacturas.DataSource = dataView;
                gvFacturas.DataBind();
            }
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
        /// Handles the RowDataBound event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvFacturas.HeaderRow != null)
            {
                _chkboxSelectAll = (CheckBox)gvFacturas.HeaderRow.FindControl("chkboxSelectAll");
            }
        }

        /// <summary>
        /// Handles the PreRender event of the gvFacturas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gvFacturas_PreRender(object sender, EventArgs e)
        {
            try
            {
                gvFacturas.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Handles the Click event of the bCancelarComprobante control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bCancelarComprobante_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            var id = btn.CommandArgument.ToString();
            var ws = new wsEmision.WsEmision { Timeout = (1800 * 1000) };
            var respuesta = ws.CancelarComprobante(id, _idUser, Session["IDENTEMI"].ToString(), false);
            var mensaje = ws.ObtenerMensaje();
            if (respuesta)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobate ahora está en proceso de cancelación", 2, null);
            }
            else
            {
                (Master as SiteMaster).MostrarAlerta(this, "El comprobante no se pudo cancelar<br/><br/>" + mensaje, 4, null);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged1 event of the ddlTipoDocumento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlTipoDocumento_SelectedIndexChanged1(object sender, EventArgs e)
        {
            ddlPtoEmi.Items.Clear();
            var nullItem = new ListItem("Todas", "0");
            nullItem.Selected = true;
            if (Session["IsCliente"] != null)
            {
                ddlPtoEmi.DataSourceID = "SqlDataSourcePtoEmiCliente";
                SqlDataSourcePtoEmiCliente.SelectParameters["tipo"].DefaultValue = ddlTipoDocumento.SelectedValue;
                SqlDataSourcePtoEmiCliente.DataBind();
            }
            else
            {
                SqlDataSourcePtoEmi.SelectParameters["tipo"].DefaultValue = ddlTipoDocumento.SelectedValue;
                SqlDataSourcePtoEmi.DataBind();
            }
            ddlPtoEmi.DataBind();
            ddlPtoEmi.Items.Add(nullItem);
        }

        /// <summary>
        /// Handles the TextChanged event of the tbRFC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbRFC_TextChanged(object sender, EventArgs e)
        {
            _db.Conectar();
            _db.CrearComando(@"SELECT NOMREC FROM Cat_Receptor WHERE RFCREC = @RFCREC");
            _db.AsignarParametroCadena("@RFCREC", tbRFC.Text);
            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                tbNombre.Text = dr[0].ToString();
            }
            _db.Desconectar();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlPageSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pageSize = 10;
            int.TryParse(ddlPageSize.SelectedValue, out pageSize);
            gvFacturas.PageSize = pageSize;
            Buscar();
        }

        /// <summary>
        /// BTNs the DPD f_ click.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>System.String.</returns>
        [System.Web.Services.WebMethod]
        public static string btnDPDF_Click(string param)
        {
            var pdf = "";
            var rutaCodigoControl = "";
            var idComprobante = "";
            var rutaDocus = "";
            var rutaPdf = "";
            var uuid = "";
            var js = "";
            _db.Conectar();
            _db.CrearComando(@"select dirdocs from Par_ParametrosSistema");

            var dr = _db.EjecutarConsulta();
            if (dr.Read())
            {
                rutaDocus = dr[0].ToString().Trim();
            }
            _db.Desconectar();

            rutaCodigoControl = param;
            var parametros = rutaCodigoControl.Split('|');
            pdf = parametros[0];
            var pdfVirtual = pdf.Replace("/", @"\");
            idComprobante = parametros[3];
            uuid = parametros[4];
            pdf = pdf.Replace(@"docus\", "").Replace("docus//", "").Replace("docus/", "");
            rutaPdf = rutaDocus + pdf;
            rutaPdf = rutaPdf.Replace("/", @"\").Replace(@"\\", @"\").Replace("//", @"\");
            if (pdf != null && (File.Exists(rutaPdf)))
            {
                var pageurl = "~/download.aspx?file=" + parametros[0];
                js += "var url = ResolveUrl('" + pageurl + "');";
                js += "$( location ).attr('href', url);";
            }
            else
            {
                var pageurl = "~/descargarPDF.aspx?idFactura=" + idComprobante + "&uuid=" + uuid;
                js += "var url = ResolveUrl('" + pageurl + "');";
                js += "loadPdfModal(url + '&mode=view', url + '&mode=download');";
            }
            return js;
        }

        /// <summary>
        /// BTNs the DPD F2_ click.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>System.String.</returns>
        [System.Web.Services.WebMethod]
        public static string btnDPDF2_Click(string param)
        {
            var idComprobante = "";
            var uuid = "";
            var js = "";
            var rutaCodigoControl = param;
            var parametros = rutaCodigoControl.Split('|');
            idComprobante = parametros[0];
            uuid = parametros[1];
            var pageurl = "~/descargarPDF.aspx?idFactura=" + idComprobante + "&uuid=" + uuid + "&tipo=C";
            js += "var url = ResolveUrl('" + pageurl + "');";
            js += "loadPdfModal(url + '&mode=view', url + '&mode=download');";
            return js;
        }
    }
}