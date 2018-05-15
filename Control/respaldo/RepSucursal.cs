using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Datos;
using System.Configuration;
using Reportes;
namespace Control
{
    public class RepSucursal
    {
        String error = "";
        public RepSucursal(String rutadoc, String FechaMinima, string Fechamaxima, string CodigoDocumento, Boolean reporteindividual, Boolean reportesupervisor, String empleado, String sucursal)
        {
            //String FechaMinima = Convert.ToString(DateTime.Now.Day) + "/" + Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Year);
            SqlConnection sqlConn;

            SqlDataAdapter sqlDaDetalle;
            //.Reportes dsPc = new Reportes.Reportes();
            Reportes.DataSet1 dsPc = new Reportes.DataSet1();

            //FechaMinima = "26/09/2011";
            String Fecha;
            Fecha = FechaMinima.Replace("/", "");
            String strConn;
            string StrDetalle;
            strConn = ConfigurationManager.ConnectionStrings["dataexpressConnectionString"].ConnectionString;

            if ((CodigoDocumento == "01" || CodigoDocumento == "04") && reporteindividual == true && reportesupervisor == true)
            {
                StrDetalle = @"set dateformat ymd SELECT        IDcOMPROBANTE,Cat_Receptor.RFCREC, CAST(Cat_Receptor.NOMREC AS VARCHAR(30)) AS NOMREC, CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) AS TIPODOC, 
                         Dat_General.secuencial AS FOLFAC, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalDescuento * (- 1) 
                         ELSE Dat_General.totalDescuento END AS totalDescuento, Dat_General.fecha AS FECHA, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN Dat_General.subtotal12 * (- 1) ELSE Dat_General.subtotal12 END AS totalAntDesc, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.IVA12 * (- 1) ELSE Dat_General.IVA12 END AS IVA12, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalSinImpuestos * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS totalSinImpuestos, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) * (- 1) ELSE CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) 
                         END AS TOTAL, Dat_General.numeroAutorizacion, Dat_General.fechaAutorizacion, Dat_General.numDocModificado, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.totalSinImpuestos AS NUMERIC(18, 2)) * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS baseImponible, 0.00 AS Tarifa,IVA12 AS totalImpuestos
FROM            Dat_General INNER JOIN
                         Cat_Emisor ON Dat_General.id_Emisor = Cat_Emisor.IDEEMI INNER JOIN
                         Cat_Receptor ON Dat_General.id_Receptor = Cat_Receptor.IDEREC LEFT OUTER JOIN
                         Cat_Catalogo1_C ON Dat_General.codDoc = Cat_Catalogo1_C.codigo AND Cat_Catalogo1_C.tipo = 'Comprobante'
                WHERE 
                      ((Dat_General.estado =1 AND Dat_General.tipo ='E') or (Dat_General.estado =0 AND Dat_General.tipo ='L')) AND 
                      Dat_General.fecha>='" + FechaMinima + "' AND Dat_General.fecha<='" + Fechamaxima + @" 23:59:59.997' AND 
                      Dat_General.codDoc =" + CodigoDocumento;
            }
            else if ((CodigoDocumento == "01" || CodigoDocumento == "04") && reporteindividual == true && reportesupervisor == false)
            {
                StrDetalle = @"SELECT        IDcOMPROBANTE,Cat_Receptor.RFCREC, CAST(Cat_Receptor.NOMREC AS VARCHAR(30)) AS NOMREC, CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) AS TIPODOC, 
                         Dat_General.secuencial AS FOLFAC, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalDescuento * (- 1) 
                         ELSE Dat_General.totalDescuento END AS totalDescuento, Dat_General.fecha AS FECHA, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN Dat_General.subtotal12 * (- 1) ELSE Dat_General.subtotal12 END AS totalAntDesc, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.IVA12 * (- 1) ELSE Dat_General.IVA12 END AS IVA12, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalSinImpuestos * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS totalSinImpuestos, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) * (- 1) ELSE CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) 
                         END AS TOTAL, Dat_General.numeroAutorizacion, Dat_General.fechaAutorizacion, Dat_General.numDocModificado, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.totalSinImpuestos AS NUMERIC(18, 2)) * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS baseImponible, 0.00 AS Tarifa,IVA12 AS totalImpuestos
FROM            Dat_General INNER JOIN
                         Cat_Emisor ON Dat_General.id_Emisor = Cat_Emisor.IDEEMI INNER JOIN
                         Cat_Receptor ON Dat_General.id_Receptor = Cat_Receptor.IDEREC LEFT OUTER JOIN
                         Cat_Catalogo1_C ON Dat_General.codDoc = Cat_Catalogo1_C.codigo AND Cat_Catalogo1_C.tipo = 'Comprobante'
                WHERE 
                      ((Dat_General.estado =1 AND Dat_General.tipo ='E') or (Dat_General.estado =0 AND Dat_General.tipo ='L')) AND 
                      Dat_General.fecha>='" + FechaMinima + "' AND Dat_General.fecha<='" + Fechamaxima + @" 23:59:59.997' AND 
                      Dat_General.codDoc =" + CodigoDocumento + " ";
            }
            else if ((CodigoDocumento == "01" || CodigoDocumento == "04") && reporteindividual == false && reportesupervisor == true)
            {
                StrDetalle = @"SELECT        IDcOMPROBANTE,Cat_Receptor.RFCREC, CAST(Cat_Receptor.NOMREC AS VARCHAR(30)) AS NOMREC, CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) AS TIPODOC, 
                         Dat_General.secuencial AS FOLFAC, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalDescuento * (- 1) 
                         ELSE Dat_General.totalDescuento END AS totalDescuento, Dat_General.fecha AS FECHA, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN Dat_General.subtotal12 * (- 1) ELSE Dat_General.subtotal12 END AS totalAntDesc, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.IVA12 * (- 1) ELSE Dat_General.IVA12 END AS IVA12, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalSinImpuestos * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS totalSinImpuestos, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) * (- 1) ELSE CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) 
                         END AS TOTAL, Dat_General.numeroAutorizacion, Dat_General.fechaAutorizacion, Dat_General.numDocModificado, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.totalSinImpuestos AS NUMERIC(18, 2)) * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS baseImponible, 0.00 AS Tarifa,IVA12 AS totalImpuestos
FROM            Dat_General INNER JOIN
                         Cat_Emisor ON Dat_General.id_Emisor = Cat_Emisor.IDEEMI INNER JOIN
                         Cat_Receptor ON Dat_General.id_Receptor = Cat_Receptor.IDEREC LEFT OUTER JOIN
                         Cat_Catalogo1_C ON Dat_General.codDoc = Cat_Catalogo1_C.codigo AND Cat_Catalogo1_C.tipo = 'Comprobante'
                WHERE 
                      ((Dat_General.estado =1 AND Dat_General.tipo ='E') or (Dat_General.estado =0 AND Dat_General.tipo ='L')) AND 
                      Dat_General.fecha>='" + FechaMinima + "' AND Dat_General.fecha<='" + Fechamaxima + @" 23:59:59.997' AND 
                       Dat_General.codDoc =" + CodigoDocumento + "AND Dat_General.estab ='" + sucursal + "'";
            }
            else if (CodigoDocumento == "all" && reporteindividual == true && reportesupervisor == true)
            {
                StrDetalle = @"SELECT        IDcOMPROBANTE,Cat_Receptor.RFCREC, CAST(Cat_Receptor.NOMREC AS VARCHAR(30)) AS NOMREC, CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) AS TIPODOC, 
                         Dat_General.secuencial AS FOLFAC, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalDescuento * (- 1) 
                         ELSE Dat_General.totalDescuento END AS totalDescuento, Dat_General.fecha AS FECHA, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN Dat_General.subtotal12 * (- 1) ELSE Dat_General.subtotal12 END AS totalAntDesc, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.IVA12 * (- 1) ELSE Dat_General.IVA12 END AS IVA12, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalSinImpuestos * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS totalSinImpuestos, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) * (- 1) ELSE CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) 
                         END AS TOTAL, Dat_General.numeroAutorizacion, Dat_General.fechaAutorizacion, Dat_General.numDocModificado, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.totalSinImpuestos AS NUMERIC(18, 2)) * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS baseImponible, 0.00 AS Tarifa,IVA12 AS totalImpuestos
FROM            Dat_General INNER JOIN
                         Cat_Emisor ON Dat_General.id_Emisor = Cat_Emisor.IDEEMI INNER JOIN
                         Cat_Receptor ON Dat_General.id_Receptor = Cat_Receptor.IDEREC LEFT OUTER JOIN
                         Cat_Catalogo1_C ON Dat_General.codDoc = Cat_Catalogo1_C.codigo AND Cat_Catalogo1_C.tipo = 'Comprobante'
                WHERE 
                       (Dat_General.codDoc='01' OR  Dat_General.codDoc='04') AND ((Dat_General.estado =1 AND Dat_General.tipo ='E') or 
                       (Dat_General.estado =0 AND Dat_General.tipo ='L')) AND 
                      Dat_General.fecha>='" + FechaMinima + "' AND Dat_General.fecha<='" + Fechamaxima + @" 23:59:59.997'";
            }
            else if (CodigoDocumento == "all" && reporteindividual == true && reportesupervisor == false)
            {
                StrDetalle = @"SELECT        IDcOMPROBANTE,Cat_Receptor.RFCREC, CAST(Cat_Receptor.NOMREC AS VARCHAR(30)) AS NOMREC, CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) AS TIPODOC, 
                         Dat_General.secuencial AS FOLFAC, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalDescuento * (- 1) 
                         ELSE Dat_General.totalDescuento END AS totalDescuento, Dat_General.fecha AS FECHA, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN Dat_General.subtotal12 * (- 1) ELSE Dat_General.subtotal12 END AS totalAntDesc, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.IVA12 * (- 1) ELSE Dat_General.IVA12 END AS IVA12, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalSinImpuestos * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS totalSinImpuestos, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) * (- 1) ELSE CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) 
                         END AS TOTAL, Dat_General.numeroAutorizacion, Dat_General.fechaAutorizacion, Dat_General.numDocModificado, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.totalSinImpuestos AS NUMERIC(18, 2)) * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS baseImponible, 0.00 AS Tarifa,IVA12 AS totalImpuestos
FROM            Dat_General INNER JOIN
                         Cat_Emisor ON Dat_General.id_Emisor = Cat_Emisor.IDEEMI INNER JOIN
                         Cat_Receptor ON Dat_General.id_Receptor = Cat_Receptor.IDEREC LEFT OUTER JOIN
                         Cat_Catalogo1_C ON Dat_General.codDoc = Cat_Catalogo1_C.codigo AND Cat_Catalogo1_C.tipo = 'Comprobante'
                WHERE 
                       (Dat_General.codDoc='01' OR  Dat_General.codDoc='04') AND ((Dat_General.estado =1 AND Dat_General.tipo ='E') or (Dat_General.estado =0 AND Dat_General.tipo ='L')) AND 
                      Dat_General.fecha>='" + FechaMinima + "' AND Dat_General.fecha<='" + Fechamaxima + @" 23:59:59.997' ";
            }
            else if (CodigoDocumento == "all" && reporteindividual == false && reportesupervisor == true)
            {
                StrDetalle = @"SELECT        IDcOMPROBANTE,Cat_Receptor.RFCREC, CAST(Cat_Receptor.NOMREC AS VARCHAR(30)) AS NOMREC, CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) AS TIPODOC, 
                         Dat_General.secuencial AS FOLFAC, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalDescuento * (- 1) 
                         ELSE Dat_General.totalDescuento END AS totalDescuento, Dat_General.fecha AS FECHA, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN Dat_General.subtotal12 * (- 1) ELSE Dat_General.subtotal12 END AS totalAntDesc, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.IVA12 * (- 1) ELSE Dat_General.IVA12 END AS IVA12, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN Dat_General.totalSinImpuestos * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS totalSinImpuestos, CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) 
                         WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) * (- 1) ELSE CAST(Dat_General.importeAPagar AS NUMERIC(18, 2)) 
                         END AS TOTAL, Dat_General.numeroAutorizacion, Dat_General.fechaAutorizacion, Dat_General.numDocModificado, 
                         CASE CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) WHEN 'NOTA DE CREDITO' THEN CAST(Dat_General.totalSinImpuestos AS NUMERIC(18, 2)) * (- 1) 
                         ELSE Dat_General.totalSinImpuestos END AS baseImponible, 0.00 AS Tarifa,IVA12 AS totalImpuestos
FROM            Dat_General INNER JOIN
                         Cat_Emisor ON Dat_General.id_Emisor = Cat_Emisor.IDEEMI INNER JOIN
                         Cat_Receptor ON Dat_General.id_Receptor = Cat_Receptor.IDEREC LEFT OUTER JOIN
                         Cat_Catalogo1_C ON Dat_General.codDoc = Cat_Catalogo1_C.codigo AND Cat_Catalogo1_C.tipo = 'Comprobante'
                WHERE 
                       (Dat_General.codDoc='01' OR  Dat_General.codDoc='04') AND ((Dat_General.estado =1 AND Dat_General.tipo ='E') or (Dat_General.estado =0 AND Dat_General.tipo ='L')) AND 
                      Dat_General.fecha>='" + FechaMinima + "' AND Dat_General.fecha<='" + Fechamaxima + @" 23:59:59.997' AND"
                      + " Dat_General.estab ='" + sucursal + "'";

            }
            else if (CodigoDocumento == "07")
            {
                StrDetalle = @" SELECT Cat_Receptor.RFCREC, CAST(Cat_Receptor.NOMREC AS VARCHAR(30)) AS NOMREC, CAST(Cat_Catalogo1_C.descripcion AS VARCHAR(15)) AS TIPODOC, 
                         Dat_TotalConImpuestos.codigo, Dat_TotalConImpuestos.codigoPorcentaje, Dat_TotalConImpuestos.baseImponible, Dat_TotalConImpuestos.tarifa, Dat_TotalConImpuestos.valor, 
                         Dat_TotalConImpuestos.porcentajeRetener, Cat_Receptor.RFCREC AS RFCREC, Dat_General.secuencial AS FOLFAC, Dat_General.numeroAutorizacion, 
                         Dat_General.fechaAutorizacion,Dat_General.numDocModificado
                        FROM    Dat_Archivos INNER JOIN
                         Dat_General ON Dat_Archivos.IDEFAC = Dat_General.idComprobante INNER JOIN
                         Cat_Emisor ON Dat_General.id_Emisor = Cat_Emisor.IDEEMI INNER JOIN
                         Cat_Receptor ON Dat_General.id_Receptor = Cat_Receptor.IDEREC INNER JOIN
                         Dat_TotalConImpuestos ON Dat_General.idComprobante = Dat_TotalConImpuestos.id_Comprobante INNER JOIN
                         Cat_Catalogo1_C ON Dat_General.codDoc = Cat_Catalogo1_C.codigo AND Cat_Catalogo1_C.tipo = 'Comprobante'
                        WHERE
                      ((Dat_General.estado =1 AND Dat_General.tipo ='E') or (Dat_General.estado =0 AND Dat_General.tipo ='L')) and
                      Dat_General.fecha>='" + FechaMinima + "' AND Dat_General.fecha<='" + Fechamaxima + " 23:59:59.997' AND Dat_General.codDoc =" + CodigoDocumento;
            }
            else
            {
                StrDetalle = "";
            }

            try
            {
                //Crear los DataAdapters
                sqlConn = new SqlConnection(strConn);

                sqlDaDetalle = new SqlDataAdapter(StrDetalle, sqlConn);
                //Poblar las tablas del dataset desde los dataAdaperts
                dsPc.EnforceConstraints = false;

                if (CodigoDocumento == "01" || CodigoDocumento == "04")
                {
                    Reportes.dtsReporteIndividual dtsIndividual = new Reportes.dtsReporteIndividual();
                    sqlDaDetalle = new SqlDataAdapter(StrDetalle, sqlConn);
                    dsPc.EnforceConstraints = false;
                    sqlDaDetalle.Fill(dtsIndividual, "RepDat_General");
                    ReporteFactNC rpt = new ReporteFactNC();
                    rpt.SetDataSource(dtsIndividual);
                    // crystalReportViewer1.ReportSource = rpt; 
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutadoc + ".xls");
                    rpt.Close();
                }
                if (CodigoDocumento == "07")
                {
                    Reportes.dtsReporteRetenciones dtsRetenciones = new Reportes.dtsReporteRetenciones();
                    sqlDaDetalle = new SqlDataAdapter(StrDetalle, sqlConn);
                    dsPc.EnforceConstraints = false;
                    sqlDaDetalle.Fill(dtsRetenciones, "RepRetenciones");
                    RepRetenciones rpt = new RepRetenciones();
                    rpt.SetDataSource(dtsRetenciones);
                    // crystalReportViewer1.ReportSource = rpt; 
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutadoc + ".xls");
                    rpt.Close();
                }

                if (CodigoDocumento == "all")
                {
                    Reportes.dtsReporteGeneral dtsDat_General = new Reportes.dtsReporteGeneral();
                    sqlDaDetalle = new SqlDataAdapter(StrDetalle, sqlConn);
                    //Poblar las tablas del dataset desde los dataAdaperts
                    dsPc.EnforceConstraints = false;
                    sqlDaDetalle.Fill(dtsDat_General, "ReporteDat_General");
                    ReporteGeneral rpt = new ReporteGeneral();
                    rpt.SetDataSource(dtsDat_General);
                    // crystalReportViewer1.ReportSource = rpt; 
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutadoc + ".xls");
                    rpt.Close();
                }

                if (CodigoDocumento == "NA")
                {
                    Reportes.dtsReporteNA dtsNA = new Reportes.dtsReporteNA();
                    sqlDaDetalle = new SqlDataAdapter(StrDetalle, sqlConn);
                    dsPc.EnforceConstraints = false;
                    sqlDaDetalle.Fill(dtsNA, "ReporteNA");
                    rptReporteNA rpt = new rptReporteNA();
                    rpt.SetDataSource(dtsNA);
                    // crystalReportViewer1.ReportSource = rpt; 
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutadoc + ".xls");
                    rpt.Close();
                }
                if (CodigoDocumento == "NE")
                {
                    Reportes.dtsReporteNE dtsNE = new Reportes.dtsReporteNE();
                    sqlDaDetalle = new SqlDataAdapter(StrDetalle, sqlConn);
                    dsPc.EnforceConstraints = false;
                    sqlDaDetalle.Fill(dtsNE, "validarxmlecu");
                    rptReporteNE rpt = new rptReporteNE();
                    rpt.SetDataSource(dtsNE);
                    // crystalReportViewer1.ReportSource = rpt; 
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutadoc + ".xls");
                    rpt.Close();
                }
            }
            catch (Exception ex)
            {

                String archivo = System.AppDomain.CurrentDomain.BaseDirectory + @"reportes\docs\" + rutadoc + ".txt";
                using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(rutadoc + ".txt"))
                {
                    escritor.WriteLine(ex.ToString());
                }
            }
        }
        public String getError()
        {
            return error;
        }
    }
}