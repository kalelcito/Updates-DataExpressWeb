using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataExpressWeb.adminstracion.sucursales
{
    public partial class pruebaGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
if (!IsPostBack){var modal = Request.QueryString.Get("modal");bool isModal = !string.IsNullOrEmpty(modal) && modal.Equals("true");(Master as SiteMaster).BackgroundContent(isModal);}

        }

        protected void gvSuppliers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                // Create header row
                GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

                TableCell cell = new TableCell();
                cell.Text = "Action";
                gvr.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Company Name";
                gvr.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Phone";
                gvr.Cells.Add(cell);

                // Add row
                gvSuppliers.Controls[0].Controls.AddAt(0, gvr);
            }
        }

        protected void gvSuppliers_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            if ((e.Exception == null) && e.AffectedRows.Equals(1))
            {
                lblResults.Text = "Supplier successfully updated.";
            }
            else
            {
                lblResults.Text = "Unable to successfully update supplier.";
                e.ExceptionHandled = true;
            }
        }

        protected void gvSuppliers_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if ((e.Exception == null) && e.AffectedRows.Equals(1))
            {
                lblResults.Text = "Supplier successfully deleted.";
            }
            else
            {
                lblResults.Text = "Unable to successfully delete supplier.";
                e.ExceptionHandled = true;
            }
        }

        protected void gvSuppliers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EmptyDataTemplateInsert"))
            {
                // Retrieve row
                GridViewRow gvr = gvSuppliers.Controls[0].Controls[1] as GridViewRow;

                if (gvr == null) { return; }

                // Retrieve controls
                TextBox txtCompanyName = gvr.FindControl("txtCompanyName") as TextBox;
                TextBox txtPhone = gvr.FindControl("txtPhone") as TextBox;

                if (txtCompanyName == null) { return; }
                if (txtPhone == null) { return; }

                // Set parameters
                sdsSuppliers.InsertParameters["CompanyName"].DefaultValue = txtCompanyName.Text;
                sdsSuppliers.InsertParameters["Phone"].DefaultValue = txtPhone.Text;

                // Perform insert
                sdsSuppliers.Insert();
            }
            else if (e.CommandName.Equals("FooterInsert"))
            {
                // Retrieve controls
                TextBox txtCompanyName = gvSuppliers.FooterRow.FindControl("txtCompanyName") as TextBox;
                TextBox txtPhone = gvSuppliers.FooterRow.FindControl("txtPhone") as TextBox;

                if (txtCompanyName == null) { return; }
                if (txtPhone == null) { return; }

                // Set parameters
                sdsSuppliers.InsertParameters["CompanyName"].DefaultValue = txtCompanyName.Text;
                sdsSuppliers.InsertParameters["Phone"].DefaultValue = txtPhone.Text;

                // Perform insert
                sdsSuppliers.Insert();
            }
        }

        protected void sdsSuppliers_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if ((e.Exception == null) && e.AffectedRows.Equals(1))
            {
                lblResults.Text = "Supplier successfully added.";
            }
            else
            {
                lblResults.Text = "Unable to successfully add supplier.";
                e.ExceptionHandled = true;
            }
        }
    }
}