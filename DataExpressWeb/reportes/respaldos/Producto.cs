using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataExpressWeb.reportes
{
    public class Producto
    {
        public string noCuenta { get; set; }
        public string cdOrigen { get; set; }
        public string cpOrigen { get; set; }
        public string envios { get; set; }
        public string piezas { get; set; }
        public string paisDestino { get; set; }
        public string cpDestino { get; set; }
        public string pesoTotal { get; set; }
        public string importe { get; set; }
        public string descuento { get; set; }
        public string neto { get; set; }
        public string servicio { get; set; }
        public string producto { get; set; }
        public string noGuia { get; set; }
        public string folio { get; set; }
        public string serie { get; set; }
        public string estado { get; set; }

        public Producto(string noCuenta, string cdOrigen, string cpOrigen, string envios, string piezas, string paisDestino, string cpDestino, 
                                string pesoTotal, string importe, string descuento, string neto, string servicio, string producto, string noGuia,string folio,string serie, string estado)
        {
            this.noCuenta = noCuenta;
            this.cdOrigen = cdOrigen;
            this.cpOrigen = cpOrigen;
            this.envios = envios;
            this.piezas = piezas;
            this.paisDestino = paisDestino;
            this.cpDestino = cpDestino;
            this.pesoTotal = pesoTotal;
            this.importe = importe;
            this.descuento = descuento;
            this.neto = neto;
            this.servicio = servicio;
            this.producto = producto;
            this.noGuia = noGuia;
            this.folio = folio;
            this.serie = serie;
            this.estado = estado;
        }
    }
}