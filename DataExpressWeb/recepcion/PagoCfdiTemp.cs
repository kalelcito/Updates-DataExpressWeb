using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataExpressWeb.recepcion
{
    public class PagoCfdiRecepcionTemp
    {
        public string UuidRelacionado { get; set; } = "";
        public string SerieFolio { get; set; } = "";
        public string Parcialidad { get; set; } = "";
        public string SaldoAnterior { get; set; } = "";
        public string SaldoPagado { get; set; } = "";
        public string SaldoInsoluto { get; set; } = "";
        public string FechaPago { get; set; } = "";
        public string FormaPago { get; set; } = "";
        public string MontoPago { get; set; } = "";
        public string NumOperacion { get; set; } = "";
        public string UuidPago { get; set; } = "";
    }
}