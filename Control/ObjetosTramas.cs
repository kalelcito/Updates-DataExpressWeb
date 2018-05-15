using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Control
{
    #region Clases Micros

    /// <summary>
    /// Class TramaMicros.
    /// </summary>
    public class TramaMicros
    {

        private string CFDI_VERSION;

        public TramaMicros(string CFDI_VERSION)
        {
            this.CFDI_VERSION = CFDI_VERSION;
        }
        /// <summary>
        /// Gets or sets the resumen.
        /// </summary>
        /// <value>The resumen.</value>
        public ResumenMicros Resumen { get; set; }
        /// <summary>
        /// Gets or sets the resumen33.
        /// </summary>
        /// <value>The resumen33.</value>
        public ResumenMicros33 Resumen33 { get; set; }
        /// <summary>
        /// Gets or sets the pagos.
        /// </summary>
        /// <value>The pagos.</value>
        public List<PagoMicros> Pagos { get; set; }
        /// <summary>
        /// Gets or sets the pagos33.
        /// </summary>
        /// <value>The pagos33.</value>
        public List<PagoMicros33> Pagos33 { get; set; }
        /// <summary>
        /// Gets or sets the detalles.
        /// </summary>
        /// <value>The detalles.</value>
        public List<DetalleMicros> Detalles { get; set; }
        /// <summary>
        /// Gets or sets the detalles33.
        /// </summary>
        /// <value>The detalles33.</value>
        public List<DetalleMicros33> Detalles33 { get; set; }
        /// <summary>
        /// Gets or sets the descuentos.
        /// </summary>
        /// <value>The descuentos.</value>
        public List<DescuentoMicros> Descuentos { get; set; }
        /// <summary>
        /// Gets or sets the descuentos33.
        /// </summary>
        /// <value>The descuentos33.</value>
        public List<DescuentoMicros33> Descuentos33 { get; set; }
        /// <summary>
        /// Gets or sets the propinas.
        /// </summary>
        /// <value>The propinas.</value>
        public PropinaMicros Propinas { get; set; }
        /// <summary>
        /// Gets or sets the propinas33.
        /// </summary>
        /// <value>The propinas33.</value>
        public PropinaMicros33 Propinas33 { get; set; }
        /// <summary>
        /// Gets or sets the impuestos.
        /// </summary>
        /// <value>The impuestos.</value>
        public List<ImpuestoMicros> Impuestos { get; set; }
        /// <summary>
        /// Gets or sets the impuestos33.
        /// </summary>
        /// <value>The impuestos33.</value>
        public List<ImpuestoMicros33> Impuestos33 { get; set; }
        /// <summary>
        /// Gets or sets the dat recep.
        /// </summary>
        /// <value>The dat recep.</value>
        public ReceptorMicros DatRecep { get; set; }
        /// <summary>
        /// Gets or sets the extr.
        /// </summary>
        /// <value>The extr.</value>
        public ExtraMicros Extr { get; set; }
        /// <summary>
        /// Gets or sets the version33.
        /// </summary>
        /// <value>The version33.</value>
        public VersionMicros33 Version33 { get; set; }
        /// <summary>
        /// Loads the specified trama.
        /// </summary>
        /// <param name="trama">The trama.</param>
        public void Load(string trama)
        {
            List<string> lines = new List<string>();
            if (CFDI_VERSION.Equals("3.3"))
            {
                var linesCheck = trama.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim()) && (x.StartsWith("E|") || x.StartsWith("R|"))).ToArray();
                lines.AddRange(linesCheck);
                var finalLines = trama.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim()) && !x.StartsWith("E|") && !x.StartsWith("R|")).ToList();
                foreach (var line in finalLines)
                {
                    var pipes = line.Split(new[] { '|' }).ToList();
                    var data = new List<string>();
                    var restar = 0;
                    string lineData = "";
                    foreach (var pipeLine in pipes)
                    {
                        if (data.Count == (15 - restar))
                        {
                            lineData = string.Join("|", data);
                            var lastChar = lineData.Substring(lineData.Length - 1, 1);
                            if (Regex.IsMatch(lastChar, "P|M|D|S|I|V"))
                            {
                                lineData = lineData.Remove(lineData.Length - 1);
                            }
                            lines.Add(lineData);
                            data = new List<string>();
                            if (Regex.IsMatch(lastChar, "P|M|D|S|I|V"))
                            {
                                data.Add(lastChar);
                            }
                            //restar = 1;
                        }
                        data.Add(pipeLine);
                    }
                    lineData = string.Join("|", data);
                    lines.Add(lineData);
                    data = new List<string>();
                }
            }
            else
            {
                trama = "First" + trama;
                var linesCheck = trama.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim()) && !x.StartsWith("E|") && !x.StartsWith("R|")).ToArray();
                lines = trama.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                if (linesCheck.Count() <= 1)
                {
                    var finalLines = lines.Where(x => !x.StartsWith("FirstT"));
                    trama = Regex.Replace(linesCheck.FirstOrDefault(), @"\bFirstT\|\b", Environment.NewLine + "FirstT|");
                    trama = Regex.Replace(trama, @"\bP\|\b", Environment.NewLine + "P|");
                    trama = Regex.Replace(trama, @"\bM\|\b", Environment.NewLine + "M|");
                    trama = Regex.Replace(trama, @"\bD\|\b", Environment.NewLine + "D|");
                    trama = Regex.Replace(trama, @"\bS\|\b", Environment.NewLine + "S|");
                    trama = Regex.Replace(trama, @"\bI\|\b", Environment.NewLine + "I|");
                    trama = Regex.Replace(trama, @"\bR\|\b", Environment.NewLine + "R|");
                    trama = Regex.Replace(trama, @"\bE\|\b", Environment.NewLine + "E|");
                    trama = Regex.Replace(trama, @"\bV\|\b", Environment.NewLine + "V|");
                    trama += Environment.NewLine + string.Join(Environment.NewLine, finalLines);
                    lines = trama.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                }
            }
            foreach (var line in lines)
            {
                var nodoDatos = line.Split(new[] { "|" }, 2, StringSplitOptions.None);
                var nodo = nodoDatos[0];
                var datos = nodoDatos[1].Split('|').ToList();
                ReadNode(datos, nodo);
            }
        }
        /// <summary>
        /// Reads the node.
        /// </summary>
        /// <param name="datos">The datos.</param>
        /// <param name="nodo">The nodo.</param>
        private void ReadNode(List<string> datos, string nodo)
        {
            if (CFDI_VERSION.Equals("3.3"))
            {
                #region CFDI 3.3
                switch (nodo)
                {
                    #region Linea T
                    case "T":
                        try
                        {
                            Resumen33 = new ResumenMicros33
                            {
                                CurrentReferenceNumber = datos[0],
                                PreviousReferenceNumber = datos[1],
                                CheckNumber = datos[2],
                                PropertyName = datos[3],
                                RevenueCenterNumber = datos[4],
                                RevenueCenterName = datos[5],
                                TransactionDate = datos[6],
                                NumberOfMovements = datos[7],
                                LayoutVersion = datos[8],
                                NetAmountGroupedP = datos[9],
                                TerminalNumberPropertyNumber = datos[10],
                                GrossAmountGroupedP = datos[11],
                                DiscountAmountGroupedM = datos[12],
                                CurrencyPayMethodRoomCharge = datos[13]
                            };
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea P
                    case "P":
                        if (Pagos33 == null)
                        {
                            Pagos33 = new List<PagoMicros33>();
                        }
                        try
                        {
                            var pag = new PagoMicros33
                            {
                                CurrentReferenceNumber = datos[0],
                                ObjectNumber = datos[1],
                                CheckNumber = datos[2],
                                //PropertyName = datos[3],
                                RevenueCenterNumber = datos[4],
                                CardCheckBank = datos[5],
                                //TransactionDate = datos[6],
                                NumberOfMovements = datos[7],
                                //LayoutVersion = datos[8],
                                NetAmount = datos[9],
                                //TerminalNumberPropertyNumber = datos[10],
                                GrossAmount = datos[11],
                                //DiscountAmountGroupedM = datos[12],
                                TenderCodeTenderName = datos[13]
                            };
                            Pagos33.Add(pag);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea M
                    case "M":
                        try
                        {
                            if (Detalles33 == null)
                            {
                                Detalles33 = new List<DetalleMicros33>();
                            }
                            var detalle = new DetalleMicros33
                            {
                                CurrentReferenceNumber = datos[0],
                                ObjectNumber = datos[1],
                                CheckNumber = datos[2],
                                ProductCodeSat = datos[3],
                                RevenueCenterNumber = datos[4],
                                MeasureUnitSat = datos[5],
                                //TransactionDate = datos[6],
                                NumberOfMovements = datos[7],
                                UnitPrice = datos[8],
                                NetAmount = datos[9],
                                TaxBaseFactorAmountCode = datos[10],
                                GrossAmount = datos[11],
                                DiscountAmount = datos[12],
                                ItemName = datos[13]
                            };
                            Detalles33.Add(detalle);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea D
                    case "D":
                        try
                        {
                            if (Descuentos33 == null)
                            {
                                Descuentos33 = new List<DescuentoMicros33>();
                            }
                            var descuento = new DescuentoMicros33
                            {
                                CurrentReferenceNumber = datos[0],
                                ObjectNumber = datos[1],
                                CheckNumber = datos[2],
                                //ProductCodeSat = datos[3],
                                RevenueCenterNumber = datos[4],
                                //MeasureUnitSat = datos[5],
                                //TransactionDate = datos[6],
                                NumberOfMovements = datos[7],
                                UnitPrice = datos[8],
                                NetAmount = datos[9],
                                TaxBaseFactorAmountCode = datos[10],
                                GrossAmount = datos[11],
                                //DiscountAmount = datos[12],
                                DiscountName = datos[13]
                            };
                            Descuentos33.Add(descuento);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea S
                    case "S":
                        try
                        {
                            Propinas33 = new PropinaMicros33
                            {
                                CurrentReferenceNumber = datos[0],
                                ObjectNumber = datos[1],
                                CheckNumber = datos[2],
                                //ProductCodeSat = datos[3],
                                RevenueCenterNumber = datos[4],
                                //MeasureUnitSat = datos[5],
                                //TransactionDate = datos[6],
                                NumberOfMovements = datos[7],
                                UnitPrice = datos[8],
                                NetAmount = datos[9],
                                TaxBaseFactorAmountCode = datos[10],
                                GrossAmount = datos[11],
                                //DiscountAmount = datos[12],
                                ServiceChargeName = datos[13]
                            };
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea I
                    case "I":
                        try
                        {
                            if (Impuestos33 == null)
                            {
                                Impuestos33 = new List<ImpuestoMicros33>();
                            }
                            var impuesto = new ImpuestoMicros33
                            {
                                CurrentReferenceNumber = datos[0],
                                //ObjectNumber = datos[1],
                                CheckNumber = datos[2],
                                //ProductCodeSat = datos[3],
                                RevenueCenterNumber = datos[4],
                                //MeasureUnitSat = datos[5],
                                //TransactionDate = datos[6],
                                NumberOfMovements = datos[7],
                                //UnitPrice = datos[8],
                                NetAmount = datos[9],
                                //TaxBaseFactorAmountCode = datos[10],
                                GrossAmount = datos[11],
                                //DiscountAmount = datos[12],
                                TaxCodeDescription = datos[13]
                            };
                            Impuestos33.Add(impuesto);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea R
                    case "R":
                        try
                        {
                            DatRecep = new ReceptorMicros
                            {
                                Rfcreceptor = datos[0],
                                Nomreceptor = datos[1],
                                Callereceptor = datos[2],
                                Noextreceptor = datos[3],
                                Nointreceptor = datos[4],
                                Colreceptor = datos[5],
                                Locreceptor = datos[6],
                                Munreceptor = datos[7],
                                Edoreceptor = datos[8],
                                Paisreceptor = datos[9],
                                Cpreceptor = datos[10],
                                Mailreceptor = datos[11],
                                UsoCfdiReceptor = datos[12]
                            };
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea E
                    case "E":
                        try
                        {
                            Extr = new ExtraMicros
                            {
                                Metodopago = datos[0],
                                Numctapago = datos[1],
                                Observ = datos[2],
                                Ine = datos[3]
                            };
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
                #endregion
            }
            else
            {
                #region CFDI 3.2
                switch (nodo)
                {
                    #region Linea T
                    case "FirstT":
                        try
                        {
                            Resumen = new ResumenMicros
                            {
                                NoReferenciaAct = datos[0],
                                NoReferenciaAnt = datos[1],
                                NoCheque = datos[2],
                                NombrePropiedad = datos[3],
                                NoCentroConsumo = datos[4],
                                NombreCentroConsumo = datos[5],
                                FechaTrans = datos[6],
                                CantidadMov = datos[7],
                                NoVersion = datos[8],
                                MontoNeto = datos[9],
                                MontoIva = datos[10],
                                MontoBruto = datos[11],
                                MontoDesc = datos[12],
                                Pago = datos[13]
                            };
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea P
                    case "P":
                        if (Pagos == null)
                        {
                            Pagos = new List<PagoMicros>();
                        }
                        try
                        {
                            var pag = new PagoMicros
                            {
                                NoReferenciaActp = datos[0],
                                NoChequep = datos[2],
                                NoCentroConsumop = datos[4],
                                Numtarnumche = datos[5],
                                CantidadMovp = datos[7],
                                NoVersionp = datos[8],
                                MontoNetop = datos[9],
                                MontoIvap = datos[10],
                                MontoBrutop = datos[11],
                                MontoDescp = datos[12],
                                Pagop = datos[13]
                            };
                            Pagos.Add(pag);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea M
                    case "M":
                        try
                        {
                            if (Detalles == null)
                            {
                                Detalles = new List<DetalleMicros>();
                            }
                            var detalle = new DetalleMicros
                            {
                                NoReferenciaActm = datos[0],
                                NoChequem = datos[2],
                                NoCentroConsumom = datos[4],
                                Cantmov = datos[7],
                                Preciounitario = datos[8],
                                MontoNetoAgrupado = datos[9],
                                MontoIvam = datos[10],
                                MontoBrutoagrupadom = datos[11],
                                MontoDescm = datos[12],
                                Detallem = datos[13]
                            };
                            Detalles.Add(detalle);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea D
                    case "D":
                        try
                        {
                            if (Descuentos == null)
                            {
                                Descuentos = new List<DescuentoMicros>();
                            }
                            var descuento = new DescuentoMicros
                            {
                                NoReferenciaActd = datos[0],
                                NoChequed = datos[2],
                                NoCentroConsumod = datos[4],
                                CantidadMovd = datos[7],
                                NoVersiond = datos[8],
                                MontoNetod = datos[9],
                                MontoIvad = datos[10],
                                MontoBrutod = datos[11],
                                MontoDescd = datos[12],
                                Destalledesc = datos[13]
                            };
                            Descuentos.Add(descuento);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea S
                    case "S":
                        try
                        {
                            Propinas = new PropinaMicros
                            {
                                NoReferenciaActs = datos[0],
                                NoCheques = datos[2],
                                NoCentroConsumos = datos[4],
                                CantidadMovs = datos[7],
                                NoVersions = datos[8],
                                MontoNetos = datos[9],
                                MontoIvas = datos[10],
                                MontoBrutos = datos[11],
                                MontoDescs = datos[12],
                                Propinas = datos[13]
                            };

                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea I
                    case "I":
                        try
                        {
                            if (Impuestos == null)
                            {
                                Impuestos = new List<ImpuestoMicros>();
                            }
                            var impuesto = new ImpuestoMicros
                            {
                                NoReferenciaActi = datos[0],
                                NoChequei = datos[2],
                                NoCentroConsumoi = datos[4],
                                CantidadMovi = datos[7],
                                NoVersioni = datos[8],
                                MontoNetoi = datos[9],
                                MontoIvai = datos[10],
                                MontoBrutoi = datos[11],
                                MontoDesci = datos[12],
                                Detallei = datos[13]
                            };
                            Impuestos.Add(impuesto);
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea R
                    case "R":
                        try
                        {
                            DatRecep = new ReceptorMicros
                            {
                                Rfcreceptor = datos[0],
                                Nomreceptor = datos[1],
                                Callereceptor = datos[2],
                                Noextreceptor = datos[3],
                                Nointreceptor = datos[4],
                                Colreceptor = datos[5],
                                Locreceptor = datos[6],
                                Munreceptor = datos[7],
                                Edoreceptor = datos[8],
                                Paisreceptor = datos[9],
                                Cpreceptor = datos[10],
                                Mailreceptor = datos[11]
                            };
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    #region Linea E
                    case "E":
                        try
                        {
                            Extr = new ExtraMicros
                            {
                                Metodopago = datos[0],
                                Numctapago = datos[1],
                                Observ = datos[2],
                                Ine = datos[3]
                            };
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
                #endregion
            }

        }
    }

    /// <summary>
    /// Class ExtraMicros.
    /// </summary>
    public class ExtraMicros
    {
        /// <summary>
        /// Gets or sets the metodopago.
        /// </summary>
        /// <value>The metodopago.</value>
        public string Metodopago { get; set; } = "";
        /// <summary>
        /// Gets or sets the numctapago.
        /// </summary>
        /// <value>The numctapago.</value>
        public string Numctapago { get; set; } = "";
        /// <summary>
        /// Gets or sets the observ.
        /// </summary>
        /// <value>The observ.</value>
        public string Observ { get; set; } = "";
        /// <summary>
        /// Gets or sets the ine.
        /// </summary>
        /// <value>The ine.</value>
        public string Ine { get; set; } = "";
    }

    /// <summary>
    /// Class receptorMicros.
    /// </summary>
    public class ReceptorMicros
    {
        /// <summary>
        /// Gets or sets the rfcreceptor.
        /// </summary>
        /// <value>The rfcreceptor.</value>
        public string Rfcreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the nomreceptor.
        /// </summary>
        /// <value>The nomreceptor.</value>
        public string Nomreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the callereceptor.
        /// </summary>
        /// <value>The callereceptor.</value>
        public string Callereceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the noextreceptor.
        /// </summary>
        /// <value>The noextreceptor.</value>
        public string Noextreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the nointreceptor.
        /// </summary>
        /// <value>The nointreceptor.</value>
        public string Nointreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the colreceptor.
        /// </summary>
        /// <value>The colreceptor.</value>
        public string Colreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the locreceptor.
        /// </summary>
        /// <value>The locreceptor.</value>
        public string Locreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the munreceptor.
        /// </summary>
        /// <value>The munreceptor.</value>
        public string Munreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the edoreceptor.
        /// </summary>
        /// <value>The edoreceptor.</value>
        public string Edoreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the paisreceptor.
        /// </summary>
        /// <value>The paisreceptor.</value>
        public string Paisreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the cpreceptor.
        /// </summary>
        /// <value>The cpreceptor.</value>
        public string Cpreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the mailreceptor.
        /// </summary>
        /// <value>The mailreceptor.</value>
        public string Mailreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the mailreceptor.
        /// </summary>
        /// <value>The mailreceptor.</value>
        public string UsoCfdiReceptor { get; set; } = "";

    }

    #region CFDI 3.2

    /// <summary>
    /// Class ResumenMicros.
    /// </summary>
    public class ResumenMicros
    {
        /// <summary>
        /// Gets or sets the no referencia act.
        /// </summary>
        /// <value>The no referencia act.</value>
        public string NoReferenciaAct { get; set; } = "";
        /// <summary>
        /// Gets or sets the no referencia ant.
        /// </summary>
        /// <value>The no referencia ant.</value>
        public string NoReferenciaAnt { get; set; } = "";
        /// <summary>
        /// Gets or sets the no cheque.
        /// </summary>
        /// <value>The no cheque.</value>
        public string NoCheque { get; set; } = "";
        /// <summary>
        /// Gets or sets the nombre propiedad.
        /// </summary>
        /// <value>The nombre propiedad.</value>
        public string NombrePropiedad { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumo.
        /// </summary>
        /// <value>The no centro consumo.</value>
        public string NoCentroConsumo { get; set; } = "";
        /// <summary>
        /// Gets or sets the nombre centro consumo.
        /// </summary>
        /// <value>The nombre centro consumo.</value>
        public string NombreCentroConsumo { get; set; } = "";
        /// <summary>
        /// Gets or sets the fecha trans.
        /// </summary>
        /// <value>The fecha trans.</value>
        public string FechaTrans { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad mov.
        /// </summary>
        /// <value>The cantidad mov.</value>
        public string CantidadMov { get; set; } = "";
        /// <summary>
        /// Gets or sets the no version.
        /// </summary>
        /// <value>The no version.</value>
        public string NoVersion { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto neto.
        /// </summary>
        /// <value>The monto neto.</value>
        public string MontoNeto { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto iva.
        /// </summary>
        /// <value>The monto iva.</value>
        public string MontoIva { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto bruto.
        /// </summary>
        /// <value>The monto bruto.</value>
        public string MontoBruto { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto desc.
        /// </summary>
        /// <value>The monto desc.</value>
        public string MontoDesc { get; set; } = "";
        /// <summary>
        /// Gets or sets the pago.
        /// </summary>
        /// <value>The pago.</value>
        public string Pago { get; set; } = "";
        /// <summary>
        /// The unidad medida
        /// </summary>
        private string _unidadMedida = "";
        /// <summary>
        /// Gets or sets the unidad medida.
        /// </summary>
        /// <value>The unidad medida.</value>
        public string UnidadMedida { get { return (string.IsNullOrEmpty(_unidadMedida) ? "No Aplica" : _unidadMedida); } set { _unidadMedida = value; } }
    }

    /// <summary>
    /// Class PagoMicros.
    /// </summary>
    public class PagoMicros
    {
        /// <summary>
        /// Gets or sets the no referencia actp.
        /// </summary>
        /// <value>The no referencia actp.</value>
        public string NoReferenciaActp { get; set; } = "";
        /// <summary>
        /// Gets or sets the no referencia antp.
        /// </summary>
        /// <value>The no referencia antp.</value>
        public string NoReferenciaAntp { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequep.
        /// </summary>
        /// <value>The no chequep.</value>
        public string NoChequep { get; set; } = "";
        /// <summary>
        /// Gets or sets the nombre propiedadp.
        /// </summary>
        /// <value>The nombre propiedadp.</value>
        public string NombrePropiedadp { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumop.
        /// </summary>
        /// <value>The no centro consumop.</value>
        public string NoCentroConsumop { get; set; } = "";
        /// <summary>
        /// Gets or sets the numtarnumche.
        /// </summary>
        /// <value>The numtarnumche.</value>
        public string Numtarnumche { get; set; } = "";
        /// <summary>
        /// Gets or sets the fecha transp.
        /// </summary>
        /// <value>The fecha transp.</value>
        public string FechaTransp { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movp.
        /// </summary>
        /// <value>The cantidad movp.</value>
        public string CantidadMovp { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versionp.
        /// </summary>
        /// <value>The no versionp.</value>
        public string NoVersionp { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netop.
        /// </summary>
        /// <value>The monto netop.</value>
        public string MontoNetop { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivap.
        /// </summary>
        /// <value>The monto ivap.</value>
        public string MontoIvap { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutop.
        /// </summary>
        /// <value>The monto brutop.</value>
        public string MontoBrutop { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descp.
        /// </summary>
        /// <value>The monto descp.</value>
        public string MontoDescp { get; set; } = "";
        /// <summary>
        /// Gets or sets the pagop.
        /// </summary>
        /// <value>The pagop.</value>
        public string Pagop { get; set; } = "";
    }

    /// <summary>
    /// Class DetalleMicros.
    /// </summary>
    public class DetalleMicros
    {
        /// <summary>
        /// Gets or sets the no referencia actm.
        /// </summary>
        /// <value>The no referencia actm.</value>
        public string NoReferenciaActm { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequem.
        /// </summary>
        /// <value>The no chequem.</value>
        public string NoChequem { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumom.
        /// </summary>
        /// <value>The no centro consumom.</value>
        public string NoCentroConsumom { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantmov.
        /// </summary>
        /// <value>The cantmov.</value>
        public string Cantmov { get; set; } = "";
        /// <summary>
        /// Gets or sets the preciounitario.
        /// </summary>
        /// <value>The preciounitario.</value>
        public string Preciounitario { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivam.
        /// </summary>
        /// <value>The monto ivam.</value>
        public string MontoIvam { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto neto agrupado.
        /// </summary>
        /// <value>The monto neto agrupado.</value>
        public string MontoNetoAgrupado { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutoagrupadom.
        /// </summary>
        /// <value>The monto brutoagrupadom.</value>
        public string MontoBrutoagrupadom { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descm.
        /// </summary>
        /// <value>The monto descm.</value>
        public string MontoDescm { get; set; } = "";
        /// <summary>
        /// Gets or sets the detallem.
        /// </summary>
        /// <value>The detallem.</value>
        public string Detallem { get; set; } = "";

    }

    /// <summary>
    /// Class DescuentoMicros.
    /// </summary>
    public class DescuentoMicros
    {
        /// <summary>
        /// Gets or sets the no referencia actd.
        /// </summary>
        /// <value>The no referencia actd.</value>
        public string NoReferenciaActd { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequed.
        /// </summary>
        /// <value>The no chequed.</value>
        public string NoChequed { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumod.
        /// </summary>
        /// <value>The no centro consumod.</value>
        public string NoCentroConsumod { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movd.
        /// </summary>
        /// <value>The cantidad movd.</value>
        public string CantidadMovd { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versiond.
        /// </summary>
        /// <value>The no versiond.</value>
        public string NoVersiond { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netod.
        /// </summary>
        /// <value>The monto netod.</value>
        public string MontoNetod { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivad.
        /// </summary>
        /// <value>The monto ivad.</value>
        public string MontoIvad { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutod.
        /// </summary>
        /// <value>The monto brutod.</value>
        public string MontoBrutod { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descd.
        /// </summary>
        /// <value>The monto descd.</value>
        public string MontoDescd { get; set; } = "";
        /// <summary>
        /// Gets or sets the destalledesc.
        /// </summary>
        /// <value>The destalledesc.</value>
        public string Destalledesc { get; set; } = "";
    }

    /// <summary>
    /// Class PropinaMicros.
    /// </summary>
    public class PropinaMicros
    {
        /// <summary>
        /// Gets or sets the no referencia acts.
        /// </summary>
        /// <value>The no referencia acts.</value>
        public string NoReferenciaActs { get; set; } = "";
        /// <summary>
        /// Gets or sets the no cheques.
        /// </summary>
        /// <value>The no cheques.</value>
        public string NoCheques { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumos.
        /// </summary>
        /// <value>The no centro consumos.</value>
        public string NoCentroConsumos { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movs.
        /// </summary>
        /// <value>The cantidad movs.</value>
        public string CantidadMovs { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versions.
        /// </summary>
        /// <value>The no versions.</value>
        public string NoVersions { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netos.
        /// </summary>
        /// <value>The monto netos.</value>
        public string MontoNetos { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivas.
        /// </summary>
        /// <value>The monto ivas.</value>
        public string MontoIvas { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutos.
        /// </summary>
        /// <value>The monto brutos.</value>
        public string MontoBrutos { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descs.
        /// </summary>
        /// <value>The monto descs.</value>
        public string MontoDescs { get; set; } = "";
        /// <summary>
        /// Gets or sets the propinas.
        /// </summary>
        /// <value>The propinas.</value>
        public string Propinas { get; set; } = "";
    }

    /// <summary>
    /// Class ImpuestoMicros.
    /// </summary>
    public class ImpuestoMicros
    {
        /// <summary>
        /// Gets or sets the no referencia acti.
        /// </summary>
        /// <value>The no referencia acti.</value>
        public string NoReferenciaActi { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequei.
        /// </summary>
        /// <value>The no chequei.</value>
        public string NoChequei { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumoi.
        /// </summary>
        /// <value>The no centro consumoi.</value>
        public string NoCentroConsumoi { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movi.
        /// </summary>
        /// <value>The cantidad movi.</value>
        public string CantidadMovi { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versioni.
        /// </summary>
        /// <value>The no versioni.</value>
        public string NoVersioni { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netoi.
        /// </summary>
        /// <value>The monto netoi.</value>
        public string MontoNetoi { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivai.
        /// </summary>
        /// <value>The monto ivai.</value>
        public string MontoIvai { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutoi.
        /// </summary>
        /// <value>The monto brutoi.</value>
        public string MontoBrutoi { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto desci.
        /// </summary>
        /// <value>The monto desci.</value>
        public string MontoDesci { get; set; } = "";
        /// <summary>
        /// Gets or sets the detallei.
        /// </summary>
        /// <value>The detallei.</value>
        public string Detallei { get; set; } = "";
    }

    #endregion
    #region CFDI 3.3

    /// <summary>
    /// Class ResumenMicros33.
    /// </summary>
    public class ResumenMicros33
    {
        /// <summary>
        /// Gets or sets the no referencia act.
        /// </summary>
        /// <value>The no referencia act.</value>
        public string CurrentReferenceNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no referencia ant.
        /// </summary>
        /// <value>The no referencia ant.</value>
        public string PreviousReferenceNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no cheque.
        /// </summary>
        /// <value>The no cheque.</value>
        public string CheckNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the nombre propiedad.
        /// </summary>
        /// <value>The nombre propiedad.</value>
        public string PropertyName { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumo.
        /// </summary>
        /// <value>The no centro consumo.</value>
        public string RevenueCenterNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the nombre centro consumo.
        /// </summary>
        /// <value>The nombre centro consumo.</value>
        public string RevenueCenterName { get; set; } = "";
        /// <summary>
        /// Gets or sets the fecha trans.
        /// </summary>
        /// <value>The fecha trans.</value>
        public string TransactionDate { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad mov.
        /// </summary>
        /// <value>The cantidad mov.</value>
        public string NumberOfMovements { get; set; } = "";
        /// <summary>
        /// Gets or sets the no version.
        /// </summary>
        /// <value>The no version.</value>
        public string LayoutVersion { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto neto.
        /// </summary>
        /// <value>The monto neto.</value>
        public string NetAmountGroupedP { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto iva.
        /// </summary>
        /// <value>The monto iva.</value>
        public string TerminalNumberPropertyNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto bruto.
        /// </summary>
        /// <value>The monto bruto.</value>
        public string GrossAmountGroupedP { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto desc.
        /// </summary>
        /// <value>The monto desc.</value>
        public string DiscountAmountGroupedM { get; set; } = "";
        /// <summary>
        /// Gets or sets the pago.
        /// </summary>
        /// <value>The pago.</value>
        public string CurrencyPayMethodRoomCharge { get; set; } = "";
    }

    /// <summary>
    /// Class PagoMicros.
    /// </summary>
    public class PagoMicros33
    {
        /// <summary>
        /// Gets or sets the no referencia actp.
        /// </summary>
        /// <value>The no referencia actp.</value>
        public string CurrentReferenceNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no referencia antp.
        /// </summary>
        /// <value>The no referencia antp.</value>
        public string ObjectNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequep.
        /// </summary>
        /// <value>The no chequep.</value>
        public string CheckNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumop.
        /// </summary>
        /// <value>The no centro consumop.</value>
        public string RevenueCenterNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the numtarnumche.
        /// </summary>
        /// <value>The numtarnumche.</value>
        public string CardCheckBank { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movp.
        /// </summary>
        /// <value>The cantidad movp.</value>
        public string NumberOfMovements { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versionp.
        /// </summary>
        /// <value>The no versionp.</value>
        public string LayoutNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netop.
        /// </summary>
        /// <value>The monto netop.</value>
        public string NetAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutop.
        /// </summary>
        /// <value>The monto brutop.</value>
        public string GrossAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the pagop.
        /// </summary>
        /// <value>The pagop.</value>
        public string TenderCodeTenderName { get; set; } = "";
    }

    /// <summary>
    /// Class DetalleMicros.
    /// </summary>
    public class DetalleMicros33
    {
        /// <summary>
        /// Gets or sets the no referencia actm.
        /// </summary>
        /// <value>The no referencia actm.</value>
        public string CurrentReferenceNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the object number.
        /// </summary>
        /// <value>The object number.</value>
        public string ObjectNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequem.
        /// </summary>
        /// <value>The no chequem.</value>
        public string CheckNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the product code sat.
        /// </summary>
        /// <value>The product code sat.</value>
        public string ProductCodeSat { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumom.
        /// </summary>
        /// <value>The no centro consumom.</value>
        public string RevenueCenterNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the measure unit sat.
        /// </summary>
        /// <value>The measure unit sat.</value>
        public string MeasureUnitSat { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantmov.
        /// </summary>
        /// <value>The cantmov.</value>
        public string NumberOfMovements { get; set; } = "";
        /// <summary>
        /// Gets or sets the preciounitario.
        /// </summary>
        /// <value>The preciounitario.</value>
        public string UnitPrice { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto neto agrupado.
        /// </summary>
        /// <value>The monto neto agrupado.</value>
        public string NetAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivam.
        /// </summary>
        /// <value>The monto ivam.</value>
        public string TaxBaseFactorAmountCode { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutoagrupadom.
        /// </summary>
        /// <value>The monto brutoagrupadom.</value>
        public string GrossAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descm.
        /// </summary>
        /// <value>The monto descm.</value>
        public string DiscountAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the detallem.
        /// </summary>
        /// <value>The detallem.</value>
        public string ItemName { get; set; } = "";

    }

    /// <summary>
    /// Class DescuentoMicros.
    /// </summary>
    public class DescuentoMicros33
    {
        /// <summary>
        /// Gets or sets the no referencia actd.
        /// </summary>
        /// <value>The no referencia actd.</value>
        public string CurrentReferenceNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the object number.
        /// </summary>
        /// <value>The object number.</value>
        public string ObjectNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequed.
        /// </summary>
        /// <value>The no chequed.</value>
        public string CheckNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumod.
        /// </summary>
        /// <value>The no centro consumod.</value>
        public string RevenueCenterNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movd.
        /// </summary>
        /// <value>The cantidad movd.</value>
        public string NumberOfMovements { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versiond.
        /// </summary>
        /// <value>The no versiond.</value>
        public string UnitPrice { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netod.
        /// </summary>
        /// <value>The monto netod.</value>
        public string NetAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivad.
        /// </summary>
        /// <value>The monto ivad.</value>
        public string TaxBaseFactorAmountCode { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutod.
        /// </summary>
        /// <value>The monto brutod.</value>
        public string GrossAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the destalledesc.
        /// </summary>
        /// <value>The destalledesc.</value>
        public string DiscountName { get; set; } = "";
    }

    /// <summary>
    /// Class PropinaMicros.
    /// </summary>
    public class PropinaMicros33
    {
        /// <summary>
        /// Gets or sets the no referencia acts.
        /// </summary>
        /// <value>The no referencia acts.</value>
        public string CurrentReferenceNumber { get; set; } = "";

        /// <summary>
        /// Gets or sets the object number.
        /// </summary>
        /// <value>The object number.</value>
        public string ObjectNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no cheques.
        /// </summary>
        /// <value>The no cheques.</value>
        public string CheckNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumos.
        /// </summary>
        /// <value>The no centro consumos.</value>
        public string RevenueCenterNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movs.
        /// </summary>
        /// <value>The cantidad movs.</value>
        public string NumberOfMovements { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versions.
        /// </summary>
        /// <value>The no versions.</value>
        public string UnitPrice { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netos.
        /// </summary>
        /// <value>The monto netos.</value>
        public string NetAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivas.
        /// </summary>
        /// <value>The monto ivas.</value>
        public string TaxBaseFactorAmountCode { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutos.
        /// </summary>
        /// <value>The monto brutos.</value>
        public string GrossAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the propinas.
        /// </summary>
        /// <value>The propinas.</value>
        public string ServiceChargeName { get; set; } = "";
    }

    /// <summary>
    /// Class ImpuestoMicros.
    /// </summary>
    public class ImpuestoMicros33
    {
        /// <summary>
        /// Gets or sets the no referencia acti.
        /// </summary>
        /// <value>The no referencia acti.</value>
        public string CurrentReferenceNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequei.
        /// </summary>
        /// <value>The no chequei.</value>
        public string CheckNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumoi.
        /// </summary>
        /// <value>The no centro consumoi.</value>
        public string RevenueCenterNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movi.
        /// </summary>
        /// <value>The cantidad movi.</value>
        public string NumberOfMovements { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versioni.
        /// </summary>
        /// <value>The no versioni.</value>
        public string LayoutVersion { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netoi.
        /// </summary>
        /// <value>The monto netoi.</value>
        public string NetAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutoi.
        /// </summary>
        /// <value>The monto brutoi.</value>
        public string GrossAmount { get; set; } = "";
        /// <summary>
        /// Gets or sets the detallei.
        /// </summary>
        /// <value>The detallei.</value>
        public string TaxCodeDescription { get; set; } = "";
    }

    /// <summary>
    /// Class VersionMicros33.
    /// </summary>
    public class VersionMicros33
    {
        /// <summary>
        /// Gets or sets the current reference number.
        /// </summary>
        /// <value>The current reference number.</value>
        public string CurrentReferenceNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the check number.
        /// </summary>
        /// <value>The check number.</value>
        public string CheckNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; } = "";
        /// <summary>
        /// Gets or sets the revenue center number.
        /// </summary>
        /// <value>The revenue center number.</value>
        public string RevenueCenterNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the name of the revenue center.
        /// </summary>
        /// <value>The name of the revenue center.</value>
        public string RevenueCenterName { get; set; } = "";
        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        /// <value>The transaction date.</value>
        public string TransactionDate { get; set; } = "";
        /// <summary>
        /// Gets or sets the layout version.
        /// </summary>
        /// <value>The layout version.</value>
        public string LayoutVersion { get; set; } = "";
        /// <summary>
        /// Gets or sets the terminal number property number.
        /// </summary>
        /// <value>The terminal number property number.</value>
        public string TerminalNumberPropertyNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the installed version.
        /// </summary>
        /// <value>The installed version.</value>
        public string InstalledVersion { get; set; } = "";
    }

    #endregion

    #endregion
    #region TramaHpresidente
    //inicia tramas hpresidente
    /// <summary>
    /// Class TramaHpresidente.
    /// </summary>
    public class TramaHpresidente
    {
        /// <summary>
        /// Gets or sets the clave.
        /// </summary>
        /// <value>The clave.</value>
        public ClavePresidente Clave { get; set; }
        /// <summary>
        /// Gets or sets the comp.
        /// </summary>
        /// <value>The comp.</value>
        public CompHpresidente Comp { get; set; }
        /// <summary>
        /// Gets or sets the impuesto.
        /// </summary>
        /// <value>The impuesto.</value>
        public ImpuestoHpresidente Impuesto { get; set; }
        /// <summary>
        /// Gets or sets the resumen.
        /// </summary>
        /// <value>The resumen.</value>
        public ResumenHpresidente Resumen { get; set; }
        /// <summary>
        /// Gets or sets the propina.
        /// </summary>
        /// <value>The propina.</value>
        public PropinaHpresidente Propina { get; set; }
        /// <summary>
        /// Gets or sets the claves.
        /// </summary>
        /// <value>The claves.</value>
        public ClavesHpresidente Claves { get; set; }
        /// <summary>
        /// Gets or sets the producto.
        /// </summary>
        /// <value>The producto.</value>
        public List<ProductosHpresidente> Producto { get; set; }
        /// <summary>
        /// Gets or sets the pagos.
        /// </summary>
        /// <value>The pagos.</value>
        public List<PagoHpresidente> Pagos { get; set; }
        /// <summary>
        /// Gets or sets the detalles.
        /// </summary>
        /// <value>The detalles.</value>
        public DetalleHpresidente Detalles { get; set; }
        /// <summary>
        /// Gets or sets the descuentos.
        /// </summary>
        /// <value>The descuentos.</value>
        public List<DescuentoHpresidente> Descuentos { get; set; }
        /// <summary>
        /// Gets or sets the propinas.
        /// </summary>
        /// <value>The propinas.</value>
        public PropinaHpresidente Propinas { get; set; }
        /// <summary>
        /// Gets or sets the impuestos.
        /// </summary>
        /// <value>The impuestos.</value>
        public List<ImpuestoHpresidente> Impuestos { get; set; }
        /// <summary>
        /// Gets or sets the dat recep.
        /// </summary>
        /// <value>The dat recep.</value>
        public ReceptorHpresidente DatRecep { get; set; }
        /// <summary>
        /// Gets or sets the extr.
        /// </summary>
        /// <value>The extr.</value>
        public ExtraHpresidente Extr { get; set; }
        //public string pro1 = "";
        //public string DESCRIPCION = "";
        //public string UNIDAD = "";
        /// <summary>
        /// Loads the specified trama.
        /// </summary>
        /// <param name="trama">The trama.</param>
        /// <returns>System.String.</returns>
        public string Load(string trama)
        {

            var xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml(trama);

                Clave = new ClavePresidente();

                #region datosHPresidente

                Clave.Sisemisor = xDoc.GetElementsByTagName("sistemaEmisor")[0].InnerText;
                Clave.Codhotel = xDoc.GetElementsByTagName("hotelCode")[0].InnerText;
                Clave.Sociedad = xDoc.GetElementsByTagName("sociedad")[0].InnerText;
                Clave.Divicion = xDoc.GetElementsByTagName("division")[0].InnerText;
                Clave.Rvc = xDoc.GetElementsByTagName("rvc")[0].InnerText;
                Clave.Cheque = xDoc.GetElementsByTagName("cheque")[0].InnerText;
                Clave.Fecha = xDoc.GetElementsByTagName("fecha")[0].InnerText;

                #endregion
                //#region Receptor

                //datRecep = new receptorHpresidente();
                //datRecep.RFCRECEPTOR = xDoc.GetElementsByTagName("rfc")[0].InnerText;
                //datRecep.NOMRECEPTOR = xDoc.GetElementsByTagName("nombre")[0].InnerText;
                //datRecep.CALLERECEPTOR = xDoc.GetElementsByTagName("dirLinea1")[0].InnerText;
                //datRecep.COLRECEPTOR = xDoc.GetElementsByTagName("dirLinea2")[0].InnerText;
                //datRecep.EDORECEPTOR = xDoc.GetElementsByTagName("dirLinea3")[0].InnerText;
                //datRecep.MAILRECEPTOR = xDoc.GetElementsByTagName("correoElectronico")[0].InnerText;

                //#endregion
                #region

                Comp = new CompHpresidente();
                Comp.Idioma = xDoc.GetElementsByTagName("idioma")[0].InnerText;
                Comp.Display = xDoc.GetElementsByTagName("display")[0].InnerText;
                Comp.Layout = xDoc.GetElementsByTagName("layout")[0].InnerText;
                Comp.Moneda = xDoc.GetElementsByTagName("moneda")[0].InnerText;
                Comp.Tipcambio = xDoc.GetElementsByTagName("tipoDeCambio")[0].InnerText;
                Comp.Tipcomprobante = xDoc.GetElementsByTagName("tipoDeComprobante")[0].InnerText;

                #endregion

                #region

                Impuesto = new ImpuestoHpresidente();
                Impuesto.MontoIvai = xDoc.GetElementsByTagName("impuesto")[0].InnerText;
                Impuesto.Importe = xDoc.GetElementsByTagName("importe")[0].InnerText;

                #endregion
                #region

                Propina = new PropinaHpresidente();
                Propina.Propina = xDoc.GetElementsByTagName("propina")[0].InnerText;
                Propina.Propinas = xDoc.GetElementsByTagName("nombre")[0].InnerText;
                Propina.Importpropina = xDoc.GetElementsByTagName("importe")[0].InnerText;

                #endregion
                #region CLAVES

                Claves = new ClavesHpresidente();
                Claves.Claveuser = xDoc.GetElementsByTagName("claveUsuarioPresidente")[0].InnerText;
                Claves.Clavefact = xDoc.GetElementsByTagName("claveFacto")[0].InnerText;

                #endregion
                #region producto
                Producto = new List<ProductosHpresidente>();

                var listaNodos = xDoc.GetElementsByTagName("producto");
                foreach (XmlNode nodo in listaNodos)
                {
                    var id = nodo.Attributes["id"].Value;
                    var cantidadFacturada = nodo.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.Equals("cantidadFacturada")).InnerText;
                    var importeNeto = nodo.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.Equals("importeNeto")).InnerText;
                    var pro = new ProductosHpresidente
                    {
                        Prod = id,
                        Cantfact = cantidadFacturada,
                        Importneto = importeNeto
                    };
                    //pro1 = id.ToString();
                    Producto.Add(pro);
                }

                #endregion
                #region detalles

                Detalles = new DetalleHpresidente();
                Detalles.MontoNetoAgrupado = xDoc.GetElementsByTagName("subtotal")[0].InnerText;
                Detalles.MontoBrutoagrupadom = xDoc.GetElementsByTagName("total")[0].InnerText;
                Detalles.FormaPago = xDoc.GetElementsByTagName("formadePago")[0].InnerText;
                //foreach (DetalleHpresidente DetalleHpresidente in new TramaHpresidente())
                //{
                //    var detalle = new DetalleHpresidente
                //    {
                //        NO_REFERENCIA_ACTM = xDoc.GetElementsByTagName("Id")[0].InnerText,
                //        NO_CHEQUEM = xDoc.GetElementsByTagName("imp")[0].InnerText
                //    };
                //    detalles.Add(detalle);
                //}

                #endregion


                try
                {
                    #region ReceptorHpresidente

                    DatRecep = new ReceptorHpresidente();

                    DatRecep.Rfcreceptor = xDoc.GetElementsByTagName("rfcRecep")[0].InnerText;
                    DatRecep.Nomreceptor = xDoc.GetElementsByTagName("razonsoRecep")[0].InnerText;
                    DatRecep.Callereceptor = xDoc.GetElementsByTagName("CalleRecep")[0].InnerText;
                    DatRecep.Noextreceptor = xDoc.GetElementsByTagName("noExteriorRecep")[0].InnerText;
                    DatRecep.Nointreceptor = xDoc.GetElementsByTagName("noInteriorRecep")[0].InnerText;
                    DatRecep.Colreceptor = xDoc.GetElementsByTagName("coloniaRecep")[0].InnerText;
                    //datRecep.LOCRECEPTOR = xDoc.GetElementsByTagName("")[0].InnerText;
                    DatRecep.Munreceptor = xDoc.GetElementsByTagName("minicipioRecep")[0].InnerText;
                    DatRecep.Edoreceptor = xDoc.GetElementsByTagName("estadoRecep")[0].InnerText;
                    DatRecep.Paisreceptor = xDoc.GetElementsByTagName("paisRecep")[0].InnerText;
                    DatRecep.Cpreceptor = xDoc.GetElementsByTagName("cpRecep")[0].InnerText;
                    DatRecep.Mailreceptor = xDoc.GetElementsByTagName("mailRecep")[0].InnerText;
                    #endregion
                    #region ExtraHpresidente
                    Extr = new ExtraHpresidente();
                    Extr.Metodopago = xDoc.GetElementsByTagName("MetodoPago")[0].InnerText;
                    Extr.Numctapago = xDoc.GetElementsByTagName("NumCtaPago")[0].InnerText;
                    Extr.Observ = xDoc.GetElementsByTagName("Observaciones")[0].InnerText;
                    Extr.Ine = xDoc.GetElementsByTagName("GetIne")[0].InnerText;
                    #endregion
                }
                catch (Exception ex)
                {
                    // No existen aun                }
                }
                Resumen = new ResumenHpresidente();
                Resumen.NoReferenciaAct = Clave.Sociedad + Clave.Divicion + Clave.Rvc + Clave.Fecha + Clave.Cheque;
                Resumen.MontoBruto = Convert.ToString(Convert.ToDecimal(Detalles.MontoBrutoagrupadom) + Convert.ToDecimal(Propina.Propina));

                if (Clave.Sociedad == "IH10" && Clave.Divicion == "1110")
                {
                    Resumen.NombrePropiedad = "001";
                    Resumen.FechaTrans = Clave.Fecha;
                    Resumen.NoCheque = Clave.Cheque;
                }
                else
                {
                    Resumen.NombrePropiedad = "003";
                    Resumen.FechaTrans = Clave.Fecha;
                    Resumen.NoCheque = Clave.Cheque;

                }
                //resumen.NOMBRE_PROPIEDAD = neww;
                //if (!string.IsNullOrEmpty(clave.SOCIEDAD) && !string.IsNullOrEmpty(pro1))
                //{
                //    BasesDatos _db = new BasesDatos();
                //    DbDataReader dr = null;
                //    _db.Conectar();
                //    _db.CrearComando(@"select idProducto,descripcion,unidad from Cat_Producto where idProducto = @IDPROD and sociedad = @SOCIEDAD");
                //    _db.AsignarParametroCadena("@IDPROD", clave.SOCIEDAD);
                //    _db.AsignarParametroCadena("@SOCIEDAD", pro1);
                //    dr = _db.EjecutarConsulta();
                //    if (dr.Read())
                //    {
                //        DESCRIPCION = dr[1].ToString() + "," + dr[3].ToString();
                //        UNIDAD = dr[2].ToString();


                //    }
                //    _db.Desconectar();

                //}



            }
            catch (Exception ex)
            { }
            return null;
        }
    }
    /// <summary>
    /// Class ExtraHpresidente.
    /// </summary>
    public class ExtraHpresidente
    {
        /// <summary>
        /// Gets or sets the metodopago.
        /// </summary>
        /// <value>The metodopago.</value>
        public string Metodopago { get; set; } = "";
        /// <summary>
        /// Gets or sets the numctapago.
        /// </summary>
        /// <value>The numctapago.</value>
        public string Numctapago { get; set; } = "";
        /// <summary>
        /// Gets or sets the observ.
        /// </summary>
        /// <value>The observ.</value>
        public string Observ { get; set; } = "";
        /// <summary>
        /// Gets or sets the ine.
        /// </summary>
        /// <value>The ine.</value>
        public string Ine { get; set; } = "";
    }
    /// <summary>
    /// Class ResumenHpresidente.
    /// </summary>
    public class ResumenHpresidente
    {
        /// <summary>
        /// Gets or sets the no referencia act.
        /// </summary>
        /// <value>The no referencia act.</value>
        public string NoReferenciaAct { get; set; } = "";
        /// <summary>
        /// Gets or sets the no referencia ant.
        /// </summary>
        /// <value>The no referencia ant.</value>
        public string NoReferenciaAnt { get; set; } = "";
        /// <summary>
        /// Gets or sets the no cheque.
        /// </summary>
        /// <value>The no cheque.</value>
        public string NoCheque { get; set; } = "";
        /// <summary>
        /// Gets or sets the nombre propiedad.
        /// </summary>
        /// <value>The nombre propiedad.</value>
        public string NombrePropiedad { get; set; }
        /// <summary>
        /// Gets or sets the no centro consumo.
        /// </summary>
        /// <value>The no centro consumo.</value>
        public string NoCentroConsumo { get; set; } = "";
        /// <summary>
        /// Gets or sets the nombre centro consumo.
        /// </summary>
        /// <value>The nombre centro consumo.</value>
        public string NombreCentroConsumo { get; set; } = "";
        /// <summary>
        /// Gets or sets the fecha trans.
        /// </summary>
        /// <value>The fecha trans.</value>
        public string FechaTrans { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad mov.
        /// </summary>
        /// <value>The cantidad mov.</value>
        public string CantidadMov { get; set; } = "";
        /// <summary>
        /// Gets or sets the no version.
        /// </summary>
        /// <value>The no version.</value>
        public string NoVersion { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto neto.
        /// </summary>
        /// <value>The monto neto.</value>
        public string MontoNeto { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto iva.
        /// </summary>
        /// <value>The monto iva.</value>
        public string MontoIva { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto bruto.
        /// </summary>
        /// <value>The monto bruto.</value>
        public string MontoBruto { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto desc.
        /// </summary>
        /// <value>The monto desc.</value>
        public string MontoDesc { get; set; } = "";
        /// <summary>
        /// Gets or sets the pago.
        /// </summary>
        /// <value>The pago.</value>
        public string Pago { get; set; } = "";
        /// <summary>
        /// The unidad medida
        /// </summary>
        private string _unidadMedida = "";
        /// <summary>
        /// Gets or sets the unidad medida.
        /// </summary>
        /// <value>The unidad medida.</value>
        public string UnidadMedida { get { return (string.IsNullOrEmpty(_unidadMedida) ? "No Aplica" : _unidadMedida); } set { _unidadMedida = value; } }
    }

    /// <summary>
    /// Class PagoHpresidente.
    /// </summary>
    public class PagoHpresidente
    {
        /// <summary>
        /// Gets or sets the no referencia actp.
        /// </summary>
        /// <value>The no referencia actp.</value>
        public string NoReferenciaActp { get; set; } = "";
        /// <summary>
        /// Gets or sets the no referencia antp.
        /// </summary>
        /// <value>The no referencia antp.</value>
        public string NoReferenciaAntp { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequep.
        /// </summary>
        /// <value>The no chequep.</value>
        public string NoChequep { get; set; } = "";
        /// <summary>
        /// Gets or sets the nombre propiedadp.
        /// </summary>
        /// <value>The nombre propiedadp.</value>
        public string NombrePropiedadp { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumop.
        /// </summary>
        /// <value>The no centro consumop.</value>
        public string NoCentroConsumop { get; set; } = "";
        /// <summary>
        /// Gets or sets the numtarnumche.
        /// </summary>
        /// <value>The numtarnumche.</value>
        public string Numtarnumche { get; set; } = "";
        /// <summary>
        /// Gets or sets the fecha transp.
        /// </summary>
        /// <value>The fecha transp.</value>
        public string FechaTransp { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movp.
        /// </summary>
        /// <value>The cantidad movp.</value>
        public string CantidadMovp { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versionp.
        /// </summary>
        /// <value>The no versionp.</value>
        public string NoVersionp { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netop.
        /// </summary>
        /// <value>The monto netop.</value>
        public string MontoNetop { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivap.
        /// </summary>
        /// <value>The monto ivap.</value>
        public string MontoIvap { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutop.
        /// </summary>
        /// <value>The monto brutop.</value>
        public string MontoBrutop { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descp.
        /// </summary>
        /// <value>The monto descp.</value>
        public string MontoDescp { get; set; } = "";
        /// <summary>
        /// Gets or sets the pagop.
        /// </summary>
        /// <value>The pagop.</value>
        public string Pagop { get; set; } = "";
    }

    /// <summary>
    /// Class DetalleHpresidente.
    /// </summary>
    public class DetalleHpresidente
    {
        /// <summary>
        /// Gets or sets the no referencia actm.
        /// </summary>
        /// <value>The no referencia actm.</value>
        public string NoReferenciaActm { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequem.
        /// </summary>
        /// <value>The no chequem.</value>
        public string NoChequem { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumom.
        /// </summary>
        /// <value>The no centro consumom.</value>
        public string NoCentroConsumom { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantmov.
        /// </summary>
        /// <value>The cantmov.</value>
        public string Cantmov { get; set; } = "";
        /// <summary>
        /// Gets or sets the preciounitario.
        /// </summary>
        /// <value>The preciounitario.</value>
        public string Preciounitario { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivam.
        /// </summary>
        /// <value>The monto ivam.</value>
        public string MontoIvam { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto neto agrupado.
        /// </summary>
        /// <value>The monto neto agrupado.</value>
        public string MontoNetoAgrupado { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutoagrupadom.
        /// </summary>
        /// <value>The monto brutoagrupadom.</value>
        public string MontoBrutoagrupadom { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descm.
        /// </summary>
        /// <value>The monto descm.</value>
        public string MontoDescm { get; set; } = "";
        /// <summary>
        /// Gets or sets the detallem.
        /// </summary>
        /// <value>The detallem.</value>
        public string Detallem { get; set; } = "";
        /// <summary>
        /// Gets or sets the forma pago.
        /// </summary>
        /// <value>The forma pago.</value>
        public string FormaPago { get; set; } = "";


    }

    /// <summary>
    /// Class DescuentoHpresidente.
    /// </summary>
    public class DescuentoHpresidente//no existe
    {
        /// <summary>
        /// Gets or sets the no referencia actd.
        /// </summary>
        /// <value>The no referencia actd.</value>
        public string NoReferenciaActd { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequed.
        /// </summary>
        /// <value>The no chequed.</value>
        public string NoChequed { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumod.
        /// </summary>
        /// <value>The no centro consumod.</value>
        public string NoCentroConsumod { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movd.
        /// </summary>
        /// <value>The cantidad movd.</value>
        public string CantidadMovd { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versiond.
        /// </summary>
        /// <value>The no versiond.</value>
        public string NoVersiond { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netod.
        /// </summary>
        /// <value>The monto netod.</value>
        public string MontoNetod { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivad.
        /// </summary>
        /// <value>The monto ivad.</value>
        public string MontoIvad { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutod.
        /// </summary>
        /// <value>The monto brutod.</value>
        public string MontoBrutod { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descd.
        /// </summary>
        /// <value>The monto descd.</value>
        public string MontoDescd { get; set; } = "";
        /// <summary>
        /// Gets or sets the destalledesc.
        /// </summary>
        /// <value>The destalledesc.</value>
        public string Destalledesc { get; set; } = "";
    } //termina

    /// <summary>
    /// Class PropinaHpresidente.
    /// </summary>
    public class PropinaHpresidente
    {
        /// <summary>
        /// Gets or sets the no referencia acts.
        /// </summary>
        /// <value>The no referencia acts.</value>
        public string NoReferenciaActs { get; set; } = "";
        /// <summary>
        /// Gets or sets the no cheques.
        /// </summary>
        /// <value>The no cheques.</value>
        public string NoCheques { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumos.
        /// </summary>
        /// <value>The no centro consumos.</value>
        public string NoCentroConsumos { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movs.
        /// </summary>
        /// <value>The cantidad movs.</value>
        public string CantidadMovs { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versions.
        /// </summary>
        /// <value>The no versions.</value>
        public string NoVersions { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netos.
        /// </summary>
        /// <value>The monto netos.</value>
        public string MontoNetos { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivas.
        /// </summary>
        /// <value>The monto ivas.</value>
        public string MontoIvas { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutos.
        /// </summary>
        /// <value>The monto brutos.</value>
        public string MontoBrutos { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto descs.
        /// </summary>
        /// <value>The monto descs.</value>
        public string MontoDescs { get; set; } = "";
        /// <summary>
        /// Gets or sets the propina.
        /// </summary>
        /// <value>The propina.</value>
        public string Propina { get; set; } = "";

        /// <summary>
        /// Gets or sets the propinas.
        /// </summary>
        /// <value>The propinas.</value>
        public string Propinas { get; set; } = "";
        /// <summary>
        /// Gets or sets the importpropina.
        /// </summary>
        /// <value>The importpropina.</value>
        public string Importpropina { get; set; } = "";

    }

    /// <summary>
    /// Class ImpuestoHpresidente.
    /// </summary>
    public class ImpuestoHpresidente
    {
        /// <summary>
        /// Gets or sets the no referencia acti.
        /// </summary>
        /// <value>The no referencia acti.</value>
        public string NoReferenciaActi { get; set; } = "";
        /// <summary>
        /// Gets or sets the no chequei.
        /// </summary>
        /// <value>The no chequei.</value>
        public string NoChequei { get; set; } = "";
        /// <summary>
        /// Gets or sets the no centro consumoi.
        /// </summary>
        /// <value>The no centro consumoi.</value>
        public string NoCentroConsumoi { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantidad movi.
        /// </summary>
        /// <value>The cantidad movi.</value>
        public string CantidadMovi { get; set; } = "";
        /// <summary>
        /// Gets or sets the no versioni.
        /// </summary>
        /// <value>The no versioni.</value>
        public string NoVersioni { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto netoi.
        /// </summary>
        /// <value>The monto netoi.</value>
        public string MontoNetoi { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto ivai.
        /// </summary>
        /// <value>The monto ivai.</value>
        public string MontoIvai { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto brutoi.
        /// </summary>
        /// <value>The monto brutoi.</value>
        public string MontoBrutoi { get; set; } = "";
        /// <summary>
        /// Gets or sets the monto desci.
        /// </summary>
        /// <value>The monto desci.</value>
        public string MontoDesci { get; set; } = "";
        /// <summary>
        /// Gets or sets the detallei.
        /// </summary>
        /// <value>The detallei.</value>
        public string Detallei { get; set; } = "";
        /// <summary>
        /// Gets or sets the importe.
        /// </summary>
        /// <value>The importe.</value>
        public string Importe { get; set; } = "";


    }
    /// <summary>
    /// Class receptorHpresidente.
    /// </summary>
    public class ReceptorHpresidente
    {
        /// <summary>
        /// Gets or sets the rfcreceptor.
        /// </summary>
        /// <value>The rfcreceptor.</value>
        public string Rfcreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the nomreceptor.
        /// </summary>
        /// <value>The nomreceptor.</value>
        public string Nomreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the callereceptor.
        /// </summary>
        /// <value>The callereceptor.</value>
        public string Callereceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the noextreceptor.
        /// </summary>
        /// <value>The noextreceptor.</value>
        public string Noextreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the nointreceptor.
        /// </summary>
        /// <value>The nointreceptor.</value>
        public string Nointreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the colreceptor.
        /// </summary>
        /// <value>The colreceptor.</value>
        public string Colreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the locreceptor.
        /// </summary>
        /// <value>The locreceptor.</value>
        public string Locreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the munreceptor.
        /// </summary>
        /// <value>The munreceptor.</value>
        public string Munreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the edoreceptor.
        /// </summary>
        /// <value>The edoreceptor.</value>
        public string Edoreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the paisreceptor.
        /// </summary>
        /// <value>The paisreceptor.</value>
        public string Paisreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the cpreceptor.
        /// </summary>
        /// <value>The cpreceptor.</value>
        public string Cpreceptor { get; set; } = "";
        /// <summary>
        /// Gets or sets the mailreceptor.
        /// </summary>
        /// <value>The mailreceptor.</value>
        public string Mailreceptor { get; set; } = "";


    }
    /// <summary>
    /// Class clavePresidente.
    /// </summary>
    public class ClavePresidente
    {
        /// <summary>
        /// Gets or sets the sisemisor.
        /// </summary>
        /// <value>The sisemisor.</value>
        public string Sisemisor { get; set; }
        /// <summary>
        /// Gets or sets the codhotel.
        /// </summary>
        /// <value>The codhotel.</value>
        public string Codhotel { get; set; } = "";
        /// <summary>
        /// Gets or sets the sociedad.
        /// </summary>
        /// <value>The sociedad.</value>
        public string Sociedad { get; set; } = "";
        /// <summary>
        /// Gets or sets the divicion.
        /// </summary>
        /// <value>The divicion.</value>
        public string Divicion { get; set; } = "";
        /// <summary>
        /// Gets or sets the RVC.
        /// </summary>
        /// <value>The RVC.</value>
        public string Rvc { get; set; } = "";
        /// <summary>
        /// Gets or sets the cheque.
        /// </summary>
        /// <value>The cheque.</value>
        public string Cheque { get; set; } = "";
        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>
        /// <value>The fecha.</value>
        public string Fecha { get; set; } = "";

    }

    /// <summary>
    /// Class compHpresidente.
    /// </summary>
    public class CompHpresidente
    {
        /// <summary>
        /// Gets or sets the idioma.
        /// </summary>
        /// <value>The idioma.</value>
        public string Idioma { get; set; } = "";
        /// <summary>
        /// Gets or sets the plantilla.
        /// </summary>
        /// <value>The plantilla.</value>
        public string Plantilla { get; set; } = "";
        /// <summary>
        /// Gets or sets the display.
        /// </summary>
        /// <value>The display.</value>
        public string Display { get; set; } = "";
        /// <summary>
        /// Gets or sets the layout.
        /// </summary>
        /// <value>The layout.</value>
        public string Layout { get; set; } = "";
        /// <summary>
        /// Gets or sets the moneda.
        /// </summary>
        /// <value>The moneda.</value>
        public string Moneda { get; set; } = "";
        /// <summary>
        /// Gets or sets the tipcambio.
        /// </summary>
        /// <value>The tipcambio.</value>
        public string Tipcambio { get; set; } = "";
        /// <summary>
        /// Gets or sets the tipcomprobante.
        /// </summary>
        /// <value>The tipcomprobante.</value>
        public string Tipcomprobante { get; set; } = "";
    }
    /// <summary>
    /// Class clavesHpresidente.
    /// </summary>
    public class ClavesHpresidente
    {
        /// <summary>
        /// Gets or sets the claveuser.
        /// </summary>
        /// <value>The claveuser.</value>
        public string Claveuser { get; set; } = "";
        /// <summary>
        /// Gets or sets the clavefact.
        /// </summary>
        /// <value>The clavefact.</value>
        public string Clavefact { get; set; } = "";
    }
    /// <summary>
    /// Class productosHpresidente.
    /// </summary>
    public class ProductosHpresidente
    {
        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>The product.</value>
        public string Prod { get; set; } = "";
        /// <summary>
        /// Gets or sets the cantfact.
        /// </summary>
        /// <value>The cantfact.</value>
        public string Cantfact { get; set; } = "";
        /// <summary>
        /// Gets or sets the importneto.
        /// </summary>
        /// <value>The importneto.</value>
        public string Importneto { get; set; } = "";
        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>The descripcion.</value>
        public string Descripcion { get; set; } = "";
        /// <summary>
        /// Gets or sets the unidad.
        /// </summary>
        /// <value>The unidad.</value>
        public string Unidad { get; set; } = "";

    }

    //termina trama
    #endregion
    #region Trama Aloha

    [XmlRoot(ElementName = "Item")]
    public class Item
    {
        [XmlElement(ElementName = "ItemID")]
        public string ItemID { get; set; }
        [XmlElement(ElementName = "ItemName")]
        public string ItemName { get; set; }
        [XmlElement(ElementName = "ItemBOHName")]
        public string ItemBOHName { get; set; }
        [XmlElement(ElementName = "ItemChitName2")]
        public string ItemChitName2 { get; set; }
        [XmlElement(ElementName = "ItemChitName")]
        public string ItemChitName { get; set; }
        [XmlElement(ElementName = "ItemQty")]
        public string ItemQty { get; set; }
        [XmlElement(ElementName = "ItemPrice")]
        public string ItemPrice { get; set; }
        [XmlElement(ElementName = "ItemGrossTotal")]
        public string ItemGrossTotal { get; set; }
        [XmlElement(ElementName = "ItemNetTotal")]
        public string ItemNetTotal { get; set; }
        [XmlElement(ElementName = "ItemTaxID")]
        public string ItemTaxID { get; set; }
        [XmlElement(ElementName = "ItemTaxRate")]
        public string ItemTaxRate { get; set; }
        [XmlElement(ElementName = "ItemTaxInclusive")]
        public string ItemTaxInclusive { get; set; }
        [XmlElement(ElementName = "ItemDiscountAmt")]
        public string ItemDiscountAmt { get; set; }
        [XmlElement(ElementName = "ItemIsPromo")]
        public string ItemIsPromo { get; set; }
    }

    [XmlRoot(ElementName = "Items")]
    public class Items
    {
        [XmlElement(ElementName = "Item")]
        public List<Item> Item { get; set; }
    }

    [XmlRoot(ElementName = "Tender")]
    public class Tender
    {
        [XmlElement(ElementName = "TenderID")]
        public string TenderID { get; set; }
        [XmlElement(ElementName = "TenderName")]
        public string TenderName { get; set; }
        [XmlElement(ElementName = "TenderAmount")]
        public string TenderAmount { get; set; }
        [XmlElement(ElementName = "TenderTip")]
        public string TenderTip { get; set; }
        [XmlElement(ElementName = "TenderHouseID")]
        public string TenderHouseID { get; set; }
    }

    [XmlRoot(ElementName = "Tenders")]
    public class Tenders
    {
        [XmlElement(ElementName = "Tender")]
        public List<Tender> Tender { get; set; }
    }

    [XmlRoot(ElementName = "Comp")]
    public class Comp
    {
        [XmlElement(ElementName = "CompID")]
        public string CompID { get; set; }
        [XmlElement(ElementName = "CompName")]
        public string CompName { get; set; }
        [XmlElement(ElementName = "CompAmount")]
        public string CompAmount { get; set; }
        [XmlElement(ElementName = "CompUnit")]
        public string CompUnit { get; set; }
        [XmlElement(ElementName = "CompUnitName")]
        public string CompUnitName { get; set; }
        [XmlElement(ElementName = "CompIsPromo")]
        public string CompIsPromo { get; set; }
    }

    [XmlRoot(ElementName = "Comps")]
    public class Comps
    {
        [XmlElement(ElementName = "Comp")]
        public List<Comp> Comp { get; set; }
    }

    [XmlRoot(ElementName = "Transacction")]
    public class Transacction
    {
        [XmlElement(ElementName = "DOB")]
        public string DOB { get; set; }
        [XmlElement(ElementName = "CheckNumber")]
        public string CheckNumber { get; set; }
        [XmlElement(ElementName = "DayPart")]
        public string DayPart { get; set; }
        [XmlElement(ElementName = "PrintTime")]
        public string PrintTime { get; set; }
        [XmlElement(ElementName = "TableName")]
        public string TableName { get; set; }
        [XmlElement(ElementName = "Guests")]
        public string Guests { get; set; }
        [XmlElement(ElementName = "CashierName")]
        public string CashierName { get; set; }
        [XmlElement(ElementName = "RevenueCenter")]
        public string RevenueCenter { get; set; }
        [XmlElement(ElementName = "IsClose")]
        public string IsClose { get; set; }
        [XmlElement(ElementName = "IsRefund")]
        public string IsRefund { get; set; }
        [XmlElement(ElementName = "ReprintCheck")]
        public string ReprintCheck { get; set; }
        [XmlElement(ElementName = "SubTotal")]
        public string SubTotal { get; set; }
        [XmlElement(ElementName = "TaxTotal")]
        public string TaxTotal { get; set; }
        [XmlElement(ElementName = "OrderModeID")]
        public string OrderModeID { get; set; }
        [XmlElement(ElementName = "OrderModeCharge")]
        public string OrderModeCharge { get; set; }
        [XmlElement(ElementName = "Total")]
        public string Total { get; set; }
        [XmlElement(ElementName = "Gratuity")]
        public string Gratuity { get; set; }
        [XmlElement(ElementName = "GrantTotal")]
        public string GrantTotal { get; set; }
        [XmlElement(ElementName = "TotalPayment")]
        public string TotalPayment { get; set; }
        [XmlElement(ElementName = "TotalCash")]
        public string TotalCash { get; set; }
        [XmlElement(ElementName = "TaxInclusive")]
        public string TaxInclusive { get; set; }
        [XmlElement(ElementName = "PaymentTip")]
        public string PaymentTip { get; set; }
        [XmlElement(ElementName = "Promo")]
        public string Promo { get; set; }
        [XmlElement(ElementName = "Comp")]
        public string Comp { get; set; }
        [XmlElement(ElementName = "Change")]
        public string Change { get; set; }
        [XmlElement(ElementName = "MemberID")]
        public string MemberID { get; set; }
        [XmlElement(ElementName = "Items")]
        public Items Items { get; set; }
        [XmlElement(ElementName = "Tenders")]
        public Tenders Tenders { get; set; }
        [XmlElement(ElementName = "Comps")]
        public Comps Comps { get; set; }
        [XmlElement(ElementName = "Promos")]
        public string Promos { get; set; }
        [XmlElement(ElementName = "LineaR")]
        public string LineaR { get; set; }
        [XmlElement(ElementName = "LineaExt")]
        public string LineaExt { get; set; }
    }

    #endregion
}
