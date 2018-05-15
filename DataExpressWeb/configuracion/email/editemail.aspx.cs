// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="editemail.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;
using Datos;

namespace DataExpressWeb.configuracion.email
{
    /// <summary>
    /// Class Editemail.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Editemail : Page
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

            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
                SqlDataSource1.ConnectionString = _db.CadenaConexion;
            }
            if (!Page.IsPostBack)
            {
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the DropDownList1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlMensaje.SelectedValue.Equals("0"))
            {
                var mensaje = "";
                _db.Conectar();
                _db.CrearComando(@"SELECT mensaje,descripcion FROM [Cat_Mensajes] where [idMensaje]=" + ddlMensaje.SelectedValue + "");
                var drSum = _db.EjecutarConsulta();
                if (drSum.Read())
                {
                    Nombremsj.Text = drSum[1].ToString();
                    mensaje = drSum[0].ToString();
                }
                _db.Desconectar();
                ddlMensaje.Visible = false;
                divMensajes.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "_setMsgJS", "setSn('" + mensaje + "', '#" + txtEditor.ClientID + "');", true);
            }
        }

        /// <summary>
        /// Submits the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Submit(object sender, EventArgs e)
        {
            try
            {
                _db.Conectar();
                _db.CrearComando(@"update  Cat_Mensajes set
                                mensaje=@mensaje where idMensaje='" + ddlMensaje.SelectedValue + "'");
                _db.AsignarParametroCadena("@mensaje", txtEditor.Value);
                _db.EjecutarConsulta1();
                _db.Desconectar();
                (Master as SiteMaster).MostrarAlerta(this, "El Mensaje fue guardado Correctamente", 2, null, "actualizaPagina();");
            }
            catch (Exception ex)
            {
                (Master as SiteMaster).MostrarAlerta(this, "El Mensaje no se pudo guardar" + Environment.NewLine + Environment.NewLine + ex.Message, 4, null, "recargarEditor();");
            }
        }
    }
}