// ***********************************************************************
// Assembly         : Control
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 03-08-2017
// ***********************************************************************
// <copyright file="NumerosALetras.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;

namespace Control
{
    /// <summary>
    /// Class NumerosALetras.
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class NumerosALetras
    {
        /// <summary>
        /// Convierte una cadena numérica a cadena de texto.
        /// </summary>
        /// <param name="num">El número a convertir.</param>
        /// <param name="moneda">El tipo de moneda a utilizar en la conversión.</param>
        /// <param name="translateIfRequired">if set to <c>true</c> [translate if required].</param>
        /// <returns>El número convertido a texto</returns>
        public string ConvertirALetras(string num, string moneda, bool translateIfRequired = false)
        {
            var res = "";
            double nro;
            try
            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "Formato no Valido";
            }
            var entero = Convert.ToInt64(Math.Truncate(nro));
            var decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            var decString = decimales.ToString();
            if (decString.Length <= 0)
            {
                decString = "00";
            }
            else if (decString.Length < 2)
            {
                decString = "0" + decString;
            }
            var dec = decString + "/100";
            if (entero < 0)
            {
                res = "MENOS ";
                entero = entero * -1;
            }
            res += ToText(Convert.ToDouble(entero)) + " " + ToTipo(moneda) + " " + dec + " " + MonedaTipo(moneda);
            if (translateIfRequired) { res = Translate(res, moneda); }
            return res;
        }

        /// <summary>
        /// To the text.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        private static string ToText(double value)
        {
            string num2Text;
            value = Math.Truncate(value);
            if (value == 0)
            {
                num2Text = "CERO";
            }
            else if (value == 1)
            {
                num2Text = "UN";
            }
            else if (value == 2)
            {
                num2Text = "DOS";
            }
            else if (value == 3)
            {
                num2Text = "TRES";
            }
            else if (value == 4)
            {
                num2Text = "CUATRO";
            }
            else if (value == 5)
            {
                num2Text = "CINCO";
            }
            else if (value == 6)
            {
                num2Text = "SEIS";
            }
            else if (value == 7)
            {
                num2Text = "SIETE";
            }
            else if (value == 8)
            {
                num2Text = "OCHO";
            }
            else if (value == 9)
            {
                num2Text = "NUEVE";
            }
            else if (value == 10)
            {
                num2Text = "DIEZ";
            }
            else if (value == 11)
            {
                num2Text = "ONCE";
            }
            else if (value == 12)
            {
                num2Text = "DOCE";
            }
            else if (value == 13)
            {
                num2Text = "TRECE";
            }
            else if (value == 14)
            {
                num2Text = "CATORCE";
            }
            else if (value == 15)
            {
                num2Text = "QUINCE";
            }
            else if (value < 20)
            {
                num2Text = "DIECI" + ToText(value - 10);
            }
            else if (value == 20)
            {
                num2Text = "VEINTE";
            }
            else if (value < 30)
            {
                num2Text = "VEINTI" + ToText(value - 20);
            }
            else if (value == 30)
            {
                num2Text = "TREINTA";
            }
            else if (value == 40)
            {
                num2Text = "CUARENTA";
            }
            else if (value == 50)
            {
                num2Text = "CINCUENTA";
            }
            else if (value == 60)
            {
                num2Text = "SESENTA";
            }
            else if (value == 70)
            {
                num2Text = "SETENTA";
            }
            else if (value == 80)
            {
                num2Text = "OCHENTA";
            }
            else if (value == 90)
            {
                num2Text = "NOVENTA";
            }
            else if (value < 100)
            {
                num2Text = ToText(Math.Truncate(value / 10) * 10) + " Y " + ToText(value % 10);
            }
            else if (value == 100)
            {
                num2Text = "CIEN";
            }
            else if (value < 200)
            {
                num2Text = "CIENTO " + ToText(value - 100);
            }
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
            {
                num2Text = ToText(Math.Truncate(value / 100)) + "CIENTOS";
            }
            else if (value == 500)
            {
                num2Text = "QUINIENTOS";
            }
            else if (value == 700)
            {
                num2Text = "SETECIENTOS";
            }
            else if (value == 900)
            {
                num2Text = "NOVECIENTOS";
            }
            else if (value < 1000)
            {
                num2Text = ToText(Math.Truncate(value / 100) * 100) + " " + ToText(value % 100);
            }
            else if (value == 1000)
            {
                num2Text = "MIL";
            }
            else if (value < 2000)
            {
                num2Text = "MIL " + ToText(value % 1000);
            }
            else if (value < 1000000)
            {
                num2Text = ToText(Math.Truncate(value / 1000)) + " MIL";
                if (value % 1000 > 0)
                {
                    num2Text = num2Text + " " + ToText(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "UN MILLON";
            }
            else if (value < 2000000)
            {
                num2Text = "UN MILLON " + ToText(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = ToText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if (value - Math.Truncate(value / 1000000) * 1000000 > 0)
                {
                    num2Text = num2Text + " " + ToText(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000)
            {
                num2Text = "UN BILLON";
            }
            else if (value < 2000000000000)
            {
                num2Text = "UN BILLON " + ToText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            else
            {
                num2Text = ToText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if (value - Math.Truncate(value / 1000000000000) * 1000000000000 > 0)
                {
                    num2Text = num2Text + " " + ToText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }
            return num2Text;
        }

        /// <summary>
        /// To the tipo.
        /// </summary>
        /// <param name="moneda">The moneda.</param>
        /// <returns>System.String.</returns>
        private static string ToTipo(string moneda)
        {
            const bool bandera = false;
            string valMoneda;
            switch (moneda)
            {
                case "MXN":
                    valMoneda = bandera ? "PESO" : "PESOS";
                    break;
                case "USD":
                    valMoneda = bandera ? "DOLAR" : "DOLARES";
                    break;
                case "EUR":
                    valMoneda = bandera ? "EURO" : "EUROS";
                    break;
                case "GBP":
                    valMoneda = bandera ? "LIBRA" : "LIBRAS";
                    break;
                default:
                    valMoneda = bandera ? "UNIDAD" : "UNIDADES";
                    break;
            }
            return valMoneda;
        }

        /// <summary>
        /// Monedas the tipo.
        /// </summary>
        /// <param name="moneda">The moneda.</param>
        /// <returns>System.String.</returns>
        private static string MonedaTipo(string moneda)
        {
            string tipoMoneda = "";
            switch (moneda)
            {
                case "MXN":
                    tipoMoneda = "M.N.";
                    break;
                case "USD":
                    tipoMoneda = "US Dollars";
                    break;
            }
            return tipoMoneda;
        }

        /// <summary>
        /// Translates the specified texto.
        /// </summary>
        /// <param name="texto">The texto.</param>
        /// <param name="moneda">The moneda.</param>
        /// <returns>System.String.</returns>
        private static string Translate(string texto, string moneda)
        {
            string translated = texto;
            switch (moneda)
            {
                case "USD":
                    translated = GoogleTranslate.Translate(texto, "es", "en");
                    break;
            }
            return translated;
        }
    }
}