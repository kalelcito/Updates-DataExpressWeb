// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="Mostrar.aspx.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Web.UI;
using Datos;
using DataExpressWeb;

namespace ups
{
    /// <summary>
    /// Class Mostrar.
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Mostrar : Page
    {
        /// <summary>
        /// The _DB
        /// </summary>
        private BasesDatos _db = new BasesDatos("");

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _db = new BasesDatos(Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE");
            //Log log = new Log((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
            if (!Page.IsPostBack)
            {
                _db.Conectar();
                _db.CrearComandoProcedimiento("PA_consultarParametros");
                _db.AsignarParametroProcedimiento("@idparametro", DbType.String, 3);
                var dr = _db.EjecutarConsulta();

                while (dr.Read())
                {
                    tbDirdocs.Text = dr["dirdocs"].ToString();
                    tbDirtxt.Text = dr["dirtxt"].ToString();
                    tbDirrespaldo.Text = dr["dirrespaldo"].ToString();
                    tbDircerti.Text = dr["dircertificados"].ToString();
                    tbDirllaves.Text = dr["dirllaves"].ToString();
                    tbDirxmlbase.Text = dr["dirXMLbase"].ToString();
                }
                _db.Desconectar();
            }
        }

        /// <summary>
        /// Handles the Click event of the bModificar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bModificar_Click(object sender, EventArgs e)
        {
            tbDirtxt.ReadOnly = false;
            tbDirdocs.ReadOnly = false;
            tbDirtxt.ReadOnly = false;
            tbDirrespaldo.ReadOnly = false;
            tbDircerti.ReadOnly = false;
            tbDirllaves.ReadOnly = false;
            bModificar.Visible = false;
            tbDirxmlbase.ReadOnly = false;
            bActualizar.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the bActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void bActualizar_Click(object sender, EventArgs e)
        {
            _db.Conectar();
            _db.CrearComandoProcedimiento("PA_modificarParametros");
            _db.AsignarParametroProcedimiento("@idparametro", DbType.Int16, 0);
            _db.AsignarParametroProcedimiento("@dirdocs", DbType.String, tbDirdocs.Text);
            _db.AsignarParametroProcedimiento("@dirtxt", DbType.String, tbDirtxt.Text);
            _db.AsignarParametroProcedimiento("@dirrespaldo", DbType.String, tbDirrespaldo.Text);
            _db.AsignarParametroProcedimiento("@dircertificados", DbType.String, tbDircerti.Text);
            _db.AsignarParametroProcedimiento("@dirllaves", DbType.String, tbDirllaves.Text);
            _db.AsignarParametroProcedimiento("@dirxmlbase", DbType.String, tbDirxmlbase.Text);
            //DbDataReader DR = DB.EjecutarConsulta1();
            _db.EjecutarConsulta();
            _db.Desconectar();
            tbDirtxt.ReadOnly = true;
            tbDirdocs.ReadOnly = true;
            tbDirtxt.ReadOnly = true;
            tbDirrespaldo.ReadOnly = true;
            tbDircerti.ReadOnly = true;
            tbDirllaves.ReadOnly = true;
            tbDirxmlbase.ReadOnly = true;
            bActualizar.Visible = false;
            bModificar.Visible = true;
        }

        /// <summary>
        /// Handles the TextChanged event of the tbDirtxt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void tbDirtxt_TextChanged(object sender, EventArgs e)
        {
        }
    }
}