using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Administracion
{
    public partial class sucursales : System.Web.UI.Page
    {
        private String separador = "|";
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}

        }

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/adminstracion/sucursales/sucursales.aspx", false);
        }

        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            buscar();
        }

        private void buscar()
        {
            int fecha = 0;
            string msjbuscar = "";
            string consulta = "";

            if (tbClave.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "CL" + tbClave.Text + separador; }
                else { consulta = "CL" + tbClave.Text + separador; }
            }
            if (tbSucursal.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "SU" + tbSucursal.Text + separador; }
                else { consulta = "SU" + tbSucursal.Text + separador; }
            }
            if (tbDomicilio.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "DO" + tbDomicilio.Text + separador; }
                else { consulta = "DO" + tbDomicilio.Text + separador; }
            }
            if (consulta.Length > 0)
            {

            }
            else
            {
                consulta = "-";
            }
            if (Convert.ToBoolean(Session["asRoles"]))
            {
                SqlDataSource1.SelectParameters["QUERY"].DefaultValue = consulta;
            }
            else {
                SqlDataSource1.SelectParameters["QUERY"].DefaultValue = "-";
            }
            SqlDataSource1.DataBind();
            gvSucursales.DataBind();
            consulta = "";
            lMensaje.Text = msjbuscar;
        }
        protected void grid_cajaSucursal_DataBinding(object sender, EventArgs e)
        {
            if (gvSucursales.SelectedIndex.Equals(-1))
            {
                gv_cajaSucursal.EmptyDataText = "";

            }
            else
            {
                gv_cajaSucursal.EmptyDataText = "No hay cajas registradas en esta sucursal";
            }

            if (!gvSucursales.SelectedIndex.Equals(-1))
            {

                //Panel_cajaSucursal.Visible = true;
                this.Panel_cajaSucursal.Visible = true;


            }
            else
            {
                this.Panel_cajaSucursal.Visible = false;
                //Panel_cajaSucursal.Visible = false;
                // ((Button)gv_cajaSucursal.Controls[0].Controls[0].FindControl("btnCajas")).Visible = false;
            }

        }

        protected void grid_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {

            gv_cajaSucursal.SelectedIndex = -1;

            try
            {
                gv_cajaSucursal.DataBind();
                Show(false);
            }
            catch (Exception ex)
            {
            }

        }


        protected void grid_sucursales_PageIndexChanged(object sender, EventArgs e)
        {
            gvSucursales.SelectedIndex = -1;


        }

        protected void btnCajas_Click(object sender, EventArgs e)
        {
            //gv_cajaSucursal.ShowFooter = true;
            DataTable dt = new DataTable();//(DataTable)gv_cajaSucursal.DataSource;
            dt.Columns.Add("idCaja");
            dt.Columns.Add("idSucursal");
            dt.Columns.Add("NumeroSisposnet");
            dt.Columns.Add("NumeroRentas");
            dt.Columns.Add("estab");
            dt.Columns.Add("ptoEmi");
            dt.Columns.Add("secuencial");
            dt.Columns.Add("estado");
            dt.Columns.Add("estadoFE");
            dt.Columns.Add("estadoPro");
            //dt = (DataTable)gv_cajaSucursal.DataSource;

            BindEmptyRow(dt, gv_cajaSucursal);
        }

        protected void lbnuevo_Click(object sender, EventArgs e)
        {
            Show(true);
        }

        protected void lbcancel_Click(object sender, EventArgs e)
        {
            Show(false);
        }

        protected void lbinsert_Click(object sender, EventArgs e)
        {
            // As i said, you can use your own method to perfom the insertion of the
            // data, using your Data Acces Layer, or direct SqlCommand on this function,
            // to do that just replace the code below with your own, dont forget to re-bind
            // your grid to show the latest data.

            // We find the controls to extract its data
            string Column1 = ((TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_numsispo")).Text;
            string Column2 = ((DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_nrentas")).SelectedValue.ToString();
            string Column3 = ((TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_estab")).Text;
            string Column4 = ((TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_ptoemi")).Text;
            string Column5 = ((TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_secuencial")).Text;
            string Column6 = ((DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estado")).SelectedValue.ToString();
            string Column7 = ((DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estadofe")).SelectedValue.ToString();
            string Column8 = ((DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estadopro")).SelectedValue.ToString();

            // We set the parameters for the datasource, on this case for the stored procedures, but
            // also can be parameters of a raw query.
            // NOTICE, here the parameters doesnt go with a "@"
            ds_CajaSucursal.InsertParameters.Add(new Parameter("NumeroSisposnet", TypeCode.String, Column1));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("NumeroRentas", TypeCode.String, Column2));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("estab", TypeCode.String, Column3));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("ptoEmi", TypeCode.String, Column4));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("secuencial", TypeCode.String, Column5));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("estado", TypeCode.String, Column6));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("estadoFE", TypeCode.String, Column7));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("estadoPro", TypeCode.String, Column8));



            // Call the insert method of the DataSource
            ds_CajaSucursal.Insert();

            // Re bind the Grid to show the changes
            gv_cajaSucursal.DataSourceID = "ds_CajaSucursal"; // volver a asignar el datasource original
            gv_cajaSucursal.DataBind();

            // Hide the controls
            Show(false);
        }

        protected void Show(bool visible)
        {
            //find the insert button on the GridView (gv)
            try
            {
                LinkButton Insert = (LinkButton)gv_cajaSucursal.FooterRow.FindControl("lbinsert");

                //find the Cancel button on the GridView (gv)
                LinkButton Cancel = (LinkButton)gv_cajaSucursal.FooterRow.FindControl("lbcancel");

                LinkButton New = (LinkButton)gv_cajaSucursal.FooterRow.FindControl("lbnuevo");

                //find the TextBoxes on the GV
                TextBox Column1 = (TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_numsispo");
                DropDownList Column2 = (DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_nrentas");
                TextBox Column3 = (TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_estab");
                TextBox Column4 = (TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_ptoemi");
                TextBox Column5 = (TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_secuencial");
                DropDownList Column6 = (DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estado");
                DropDownList Column7 = (DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estadofe");
                DropDownList Column8 = (DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estadopro");

                //Now set their propierties, based on the parameter "visible"
                New.Visible = !visible;
                Insert.Visible = visible;
                Cancel.Visible = visible;

                Column1.Text = string.Empty;
                //Column2.Text = string.Empty;
                Column3.Text = string.Empty;
                Column4.Text = string.Empty;
                Column5.Text = string.Empty;


                Column1.Visible = visible;
                Column2.Visible = visible;
                Column3.Visible = visible;
                Column4.Visible = visible;
                Column5.Visible = visible;
                Column6.Visible = visible;
                Column7.Visible = visible;
                Column8.Visible = visible;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }

        }


        private void ShowNoResultFound(DataTable source, GridView gv) //funcion de prueba...se utiliza la siguiente
        {
            source.Rows.Add(source.NewRow()); // create a new blank row to the DataTable
            gv.DataSourceID = null; // cambiar el datasource para asignarle uno en blanco
            // Bind the DataTable which contain a blank row to the GridView
            gv.DataSource = source;
            gv.DataBind();
            // Get the total number of columns in the GridView to know what the Column Span should be
            int columnsCount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();// clear all the cells in the row
            gv.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
            gv.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

            //You can set the styles here
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            //set No Results found to the new added cell
            gv.Rows[0].Cells[0].Text = "NO RESULT FOUND!";
        }

        private void BindEmptyRow(DataTable source, GridView gv)
        {
            gv.Dispose();
            gv.DataSourceID = null; // cambiar el datasource para asignarle uno en blanco
            DataTable t = new DataTable();
            t = source.Clone(); // Clone Source Table

            foreach (DataColumn c in t.Columns)
                c.AllowDBNull = true; // Allow Nulls in all columns

            t.Rows.Add(t.NewRow()); // Add empty row

            gv.DataSource = t; // Set Source to clone table with 1 row
            gv.DataBind(); // bind to empty table
            //gv.Rows[0].Cells[0].Text = gv.EmptyDataText; //If you dont want to show the Message

            gv.Rows[0].Visible = false;
            gv.Rows[0].Controls.Clear();

        }

        protected void gv_cajaSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




    }
}