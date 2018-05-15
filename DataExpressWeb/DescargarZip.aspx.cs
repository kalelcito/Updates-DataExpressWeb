using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using Control;
using Datos;

namespace DataExpressWeb
{
    public partial class DescargarZip : System.Web.UI.Page
    {
        private BasesDatos _db;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDENTEMI"] != null)
            {
                _db = new BasesDatos((Session["IDENTEMI"] != null ? Session["IDENTEMI"].ToString() : "CORE"));
                SqlDataSourcezIP.ConnectionString = _db.CadenaConexion;
            }
        }           
     
    }
}