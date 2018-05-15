// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 07-02-2017
// ***********************************************************************
// <copyright file="ControlUtilities.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using MailBee.AddressCheck;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DataExpressWeb
{
    /// <summary>
    /// Provide utilities methods related to <see cref="Control" /> objects
    /// </summary>
    public static class ControlUtilities
    {

        /// <summary>
        /// Enum MailAdressStatus
        /// </summary>
        public enum MailAdressStatus
        {
            /// <summary>
            /// The bad
            /// </summary>
            Bad = 0,
            /// <summary>
            /// The good
            /// </summary>
            Good = 1,
            /// <summary>
            /// The probably good
            /// </summary>
            ProbablyGood = 2
        }

        /// <summary>
        /// Find the first ancestor of the selected control in the control tree
        /// </summary>
        /// <typeparam name="TControl">Type of the ancestor to look for</typeparam>
        /// <param name="control">The control to look for its ancestors</param>
        /// <returns>The first ancestor of the specified type, or null if no ancestor is found.</returns>
        /// <exception cref="ArgumentNullException">control</exception>
        /// <exception cref="System.ArgumentNullException">control</exception>
        public static TControl FindAncestor<TControl>(this System.Web.UI.Control control) where TControl : System.Web.UI.Control
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            var parent = control;
            do
            {
                parent = parent.Parent;
                var candidate = parent as TControl;
                if (candidate != null)
                {
                    return candidate;
                }
            } while (parent != null);
            return null;
        }

        /// <summary>
        /// Finds all descendants of a certain type of the specified control.
        /// </summary>
        /// <typeparam name="TControl">The type of descendant controls to look for.</typeparam>
        /// <param name="parent">The parent control where to look into.</param>
        /// <returns>All corresponding descendants</returns>
        /// <exception cref="ArgumentNullException">control</exception>
        /// <exception cref="System.ArgumentNullException">control</exception>
        public static IEnumerable<TControl> FindDescendants<TControl>(this System.Web.UI.Control parent) where TControl : System.Web.UI.Control
        {
            if (parent == null)
            {
                throw new ArgumentNullException("control");
            }

            if (parent.HasControls())
            {
                foreach (System.Web.UI.Control childControl in parent.Controls)
                {
                    var candidate = childControl as TControl;
                    if (candidate != null)
                    {
                        yield return candidate;
                    }

                    foreach (var nextLevel in FindDescendants<TControl>(childControl))
                    {
                        yield return nextLevel;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the type of the MIME.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>System.String.</returns>
        public static string GetMimeType(this Image image)
        {
            return image.RawFormat.GetMimeType();
        }

        /// <summary>
        /// Gets the type of the MIME.
        /// </summary>
        /// <param name="imageFormat">The image format.</param>
        /// <returns>System.String.</returns>
        public static string GetMimeType(this ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.First(codec => codec.FormatID == imageFormat.Guid).MimeType;
        }

        /// <summary>
        /// Gets the image from byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>System.Drawing.Image.</returns>
        public static Image GetImageFromByteArray(byte[] byteArray)
        {
            var _imageConverter = new ImageConverter();
            var bm = (Bitmap)_imageConverter.ConvertFrom(byteArray);
            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution || bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f), (int)(bm.VerticalResolution + 0.5f));
            }
            return bm;
        }

        /// <summary>
        /// Gets the bitmap from byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap GetBitmapFromByteArray(byte[] byteArray)
        {
            var _imageConverter = new ImageConverter();
            var bm = (Bitmap)_imageConverter.ConvertFrom(byteArray);
            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution || bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f), (int)(bm.VerticalResolution + 0.5f));
            }
            return bm;
        }

        /// <summary>
        /// RGB2s the hexadecimal.
        /// </summary>
        /// <param name="rgbString">The RGB string.</param>
        /// <returns>System.String.</returns>
        public static string Rgb2Hex(string rgbString)
        {
            var values = rgbString.Split(',');
            int r = 0;
            int g = 0;
            int b = 0;
            int.TryParse(values[0], out r);
            int.TryParse(values[1], out g);
            int.TryParse(values[2], out b);
            var color = Color.FromArgb(r, g, b);
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            return hex;
        }

        /// <summary>
        /// Compresses the image.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="imageQuality">The image quality.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] CompressImage(byte[] src, int imageQuality)
        {
            try
            {
                var sourceImage = GetImageFromByteArray(src);
                ImageCodecInfo jpegCodec = null;
                var imageQualitysParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, imageQuality);
                var alleCodecs = ImageCodecInfo.GetImageEncoders();
                var codecParameter = new EncoderParameters(1);
                codecParameter.Param[0] = imageQualitysParameter;
                for (int i = 0; i < alleCodecs.Length; i++)
                {
                    if (alleCodecs[i].MimeType == "image/jpeg")
                    {
                        jpegCodec = alleCodecs[i];
                        break;
                    }
                }
                var outputStream = new MemoryStream();
                sourceImage.Save(outputStream, jpegCodec, codecParameter);
                return outputStream.ToArray();
            }
            catch (Exception e)
            {

            }
            return null;
        }

        /// <summary>
        /// Converts the data table to HTML.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>System.String.</returns>
        public static string ConvertDataTableToHTML(System.Data.DataTable dt)
        {
            string html = "<table class='table table-condensed table-responsive'>";
            //add header row
            html += "<thead><tr style='color:White;background-color:#4580A8;font-weight:bold;'>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<th scope='col' style='text-align: center;'>" + dt.Columns[i].ColumnName + "</th>";
            html += "</tr></thead><tbody>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td scope='col' style='text-align: center;'>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</tbody></table>";
            return html;
        }

        /// <summary>
        /// Codifica una cadenad etexto plano en Base64/UTF-8
        /// </summary>
        /// <param name="stringToEncode">El texto plao a codificar</param>
        /// <returns>Cadena de texto en formato Base64/UTF-8</returns>
        public static string EncodeStringToBase64(string stringToEncode)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(stringToEncode));
        }

        /// <summary>
        /// Codifica un archivo en Base64/UTF-8
        /// </summary>
        /// <param name="fileToEncode">La ruta del archivo a codificar</param>
        /// <returns>Cadena de texto en formato Base64/UTF-8</returns>
        public static string EncodeFileToBase64(string fileToEncode)
        {
            var bytes = File.ReadAllBytes(fileToEncode);
            return Convert.ToBase64String(bytes);
        }

        public static string EncodeXmlToBase64(XmlDocument xDoc)
        {
            var stringToEncode = xDoc.OuterXml;
            return EncodeStringToBase64(stringToEncode);
        }

        public static XmlDocument DecodeBase64ToXml(string stringToDecode)
        {
            var xmlString = DecodeStringFromBase64(stringToDecode);
            var xDoc = new XmlDocument();
            xDoc.LoadXml(xmlString);
            return xDoc;
        }

        /// <summary>
        /// Decodifica una cadena en Base64 archivo a un archivo
        /// </summary>
        /// <param name="stringToDecode">La ruta del archivo a codificar</param>
        /// <returns>Cadena de texto en formato Base64/UTF-8</returns>
        public static byte[] DecodeBase64ToFile(string stringToDecode)
        {
            var bytes = Convert.FromBase64String(stringToDecode);
            return bytes;
        }

        /// <summary>
        /// Decodifica una cadena en texto Base64/UTF-8 a texto plano
        /// </summary>
        /// <param name="stringToDecode">La cadena en formato Base64/UTF-8 a decodificar</param>
        /// <returns>El texto plano decodificado</returns>
        public static string DecodeStringFromBase64(string stringToDecode)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(stringToDecode));
        }

        /// <summary>
        /// Validates the mail.
        /// </summary>
        /// <param name="mail">The mail.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static MailAdressStatus ValidateMail(string mail)
        {
            var valid = MailAdressStatus.ProbablyGood;
            try
            {
                var validator = new EmailAddressValidator("MN110-1C20DF387EA9952F661A118648FA-468F");

                // To perform DNS MX lookup queries, we need some DNS servers for that.
                validator.DnsServers.Autodetect();

                var result = validator.Verify(mail);

                switch (result)
                {
                    case AddressValidationLevel.OK:
                        valid = MailAdressStatus.Good; break;
                    case AddressValidationLevel.RegexCheck:
                        valid = MailAdressStatus.Bad; break;
                    case AddressValidationLevel.DnsQuery:
                    case AddressValidationLevel.SendAttempt:
                    case AddressValidationLevel.SmtpConnection:
                        valid = MailAdressStatus.ProbablyGood; break;
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
            return valid;
        }

        //http://stackoverflow.com/questions/3885964/regex-to-replace-invalid-characters
        /// <summary>
        /// Removes the non word chars.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string RemoveNonWordChars(string source)
        {
            return RemoveNonWordChars(source, "");
        }


        //http://stackoverflow.com/questions/3885964/regex-to-replace-invalid-characters
        /// <summary>
        /// Removes the non word chars.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns>System.String.</returns>
        private static string RemoveNonWordChars(string source, string replacement)
        {
            //\W is any non-word character (not [^a-zA-Z0-9_]).
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[^a-zA-Z0-9-]+");
            return regex.Replace(source, replacement);
        }


        /// <summary>
        /// Cleans the name of the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>System.String.</returns>
        public static string CleanFileName(string filename)
        {
            string fileEnding = null;
            int index = filename.LastIndexOf(".");

            //removes the file ending.
            if (index != -1)
            {
                fileEnding = filename.Substring(index + 1);
                filename = filename.Substring(0, index);

                //remove based on the CharacterReplacements list
                for (int i = 0; i < CharacterReplacements.GetLength(0); i++)
                {
                    fileEnding = fileEnding.Replace(CharacterReplacements[i, 0], CharacterReplacements[i, 1]);
                }

                //remove everything that is left
                fileEnding = "." + RemoveNonWordChars(fileEnding);
            }

            //remove based on the CharacterReplacements list
            for (int i = 0; i < CharacterReplacements.GetLength(0); i++)
            {
                filename = filename.Replace(CharacterReplacements[i, 0], CharacterReplacements[i, 1]);
            }

            //remove everything that is left
            filename = RemoveNonWordChars(filename);

            return filename + fileEnding;
        }

        /// <summary>
        /// Ceroses the null.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="requerido">if set to <c>true</c> [requerido].</param>
        /// <param name="zeros">if set to <c>true</c> [zeros].</param>
        /// <param name="comas">if set to <c>true</c> [comas].</param>
        /// <param name="pesosSign">if set to <c>true</c> [pesos sign].</param>
        /// <returns>System.String.</returns>
        public static string CerosNull(string a, bool requerido = true, bool zeros = true, bool comas = false, bool pesosSign = false)
        {
            var result = a;
            var format = zeros ? (!comas ? "{0:0.00}" : "{0:#,##0.00}") : (!comas ? "{0:0.##}" : "{0:#,##.##}");
            var cifra = (!string.IsNullOrEmpty(result) ? result : "").Replace(",", "").Trim();
            if (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(cifra))
            {
                decimal b = 0;
                if (decimal.TryParse(cifra, out b))
                    result = string.Format(format, b);
            }
            else
            {
                result = requerido ? "0.00" : a;
            }
            if (!string.IsNullOrEmpty(result) && !result.Equals("N/A") && pesosSign)
                result = "$ " + result;
            return result;
        }

        /// <summary>
        /// To the x element.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>XElement.</returns>
        public static XElement ToXElement(this XmlNode node)
        {
            var xDoc = new XDocument();
            using (var xmlWriter = xDoc.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }
            return xDoc.Root;
        }

        /// <summary>
        /// To the XML node.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>XmlNode.</returns>
        public static XmlNode ToXmlNode(this XElement element)
        {
            using (var xmlReader = element.CreateReader())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        //http://www.pjb.com.au/comp/diacritics.html
        /// <summary>
        /// The character replacements
        /// </summary>
        private static string[,] CharacterReplacements = {
    { " ", "-"},
    { "&", "-"},
    { "?", "-"},
    { "!", "-"},
    { "%", "-"},
    { "+", "-"},
    { "#", "-"},
    { ":", "-"},
    { ";", "-"},
    { ".", "-"},

    { "¢", "c" },   //cent
    { "£", "P" },   //Pound
    { "€", "E" },   //Euro
    { "¥", "Y" },   //Yen
    { "°", "d" },   //degree
    { "¼", "1-4" }, //fraction one-quarter
    { "½", "1-2" }, //fraction half    
    { "¾", "1-3" }, //fraction three-quarters}
    { "@", "AT)"}, //at                                                  
    { "Œ", "OE" },  //OE ligature, French (in ISO-8859-15)        
    { "œ", "oe" },  //OE ligature, French (in ISO-8859-15)        
 
    {"Å","A" },  //ring
    {"Æ","AE"},  //diphthong
    {"Ç","C" },  //cedilla
    {"È","E" },  //grave accent
    {"É","E" },  //acute accent
    {"Ê","E" },  //circumflex accent
    {"Ë","E" },  //umlaut mark
    {"Ì","I" },  //grave accent
    {"Í","I" },  //acute accent
    {"Î","I" },  //circumflex accent
    {"Ï","I" },  //umlaut mark
    {"Ð","Eth"}, //Icelandic
    {"Ñ","N" },  //tilde
    {"Ò","O" },  //grave accent
    {"Ó","O" },  //acute accent
    {"Ô","O" },  //circumflex accent
    {"Õ","O" },  //tilde
    {"Ö","O" },  //umlaut mark
    {"Ø","O" },  //slash
    {"Ù","U" },  //grave accent
    {"Ú","U" },  //acute accent
    {"Û","U" },  //circumflex accent
    {"Ü","U" },  //umlaut mark
    {"Ý","Y" },  //acute accent
    {"Þ","eth"}, //Icelandic - http://en.wikipedia.org/wiki/Thorn_(letter)
    {"ß","ss"},  //German
 
    {"à","a" },  //grave accent
    {"á","a" },  //acute accent
    {"â","a" },  //circumflex accent
    {"ã","a" },  //tilde
    {"ä","ae"},  //umlaut mark
    {"å","a" },  //ring
    {"æ","ae"},  //diphthong
    {"ç","c" },  //cedilla
    {"è","e" },  //grave accent
    {"é","e" },  //acute accent
    {"ê","e" },  //circumflex accent
    {"ë","e" },  //umlaut mark
    {"ì","i" },  //grave accent
    {"í","i" },  //acute accent
    {"î","i" },  //circumflex accent
    {"ï","i" },  //umlaut mark
    {"ð","eth"}, //Icelandic
    {"ñ","n" },  //tilde
    {"ò","o" },  //grave accent
    {"ó","o" },  //acute accent
    {"ô","o" },  //circumflex accent
    {"õ","o" },  //tilde
    {"ö","oe"},  //umlaut mark
    {"ø","o" },  //slash
    {"ù","u" },  //grave accent
    {"ú","u" },  //acute accent
    {"û","u" },  //circumflex accent
    {"ü","ue"},  //umlaut mark
    {"ý","y" },  //acute accent
    {"þ","eth"}, //Icelandic - http://en.wikipedia.org/wiki/Thorn_(letter)
    {"ÿ","y" },  //umlaut mark
    };

    }
}