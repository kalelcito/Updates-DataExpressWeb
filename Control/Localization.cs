// ***********************************************************************
// Assembly         : Control
// Author           : Sergio
// Created          : 01-30-2017
//
// Last Modified By : Sergio
// Last Modified On : 01-30-2017
// ***********************************************************************
// <copyright file="Localization.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Control
{
    /// <summary>
    /// Class Localization.
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// The time zone
        /// </summary>
        private static TimeZoneInfo _timeZone = null;
        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public static TimeZoneInfo TimeZone { get { return (_timeZone == null) ? TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(t => t.Id.ToUpper().Contains("MEXICO") && t.Id.ToUpper().Contains("CENTRAL")) : _timeZone; } set { _timeZone = value; } }
        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value>The now.</value>
        public static DateTime Now { get { return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZone); } }
        /// <summary>
        /// Parses the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>DateTime.</returns>
        public static DateTime Parse(string s)
        {
            var parsed = DateTime.Parse(s);
            return TimeZoneInfo.ConvertTime(parsed, TimeZone);
        }
        /// <summary>
        /// Parses the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>DateTime.</returns>
        public static DateTime Parse(string s, IFormatProvider provider)
        {
            var parsed = DateTime.Parse(s, provider);
            return TimeZoneInfo.ConvertTime(parsed, TimeZone);
        }
        /// <summary>
        /// Parses the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="styles">The styles.</param>
        /// <returns>DateTime.</returns>
        public static DateTime Parse(string s, IFormatProvider provider, System.Globalization.DateTimeStyles styles)
        {
            var parsed = DateTime.Parse(s, provider, styles);
            return TimeZoneInfo.ConvertTime(parsed, TimeZone);
        }
        /// <summary>
        /// Parses the exact.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ParseExact(string s, string format, IFormatProvider provider)
        {
            var parsed = DateTime.ParseExact(s, format, provider);
            return TimeZoneInfo.ConvertTime(parsed, TimeZone);
        }
        /// <summary>
        /// Parses the exact.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="style">The style.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ParseExact(string s, string format, IFormatProvider provider, System.Globalization.DateTimeStyles style)
        {
            var parsed = DateTime.ParseExact(s, format, provider, style);
            return TimeZoneInfo.ConvertTime(parsed, TimeZone);
        }
        /// <summary>
        /// Parses the exact.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="style">The style.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ParseExact(string s, string[] format, IFormatProvider provider, System.Globalization.DateTimeStyles style)
        {
            var parsed = DateTime.ParseExact(s, format, provider, style);
            return TimeZoneInfo.ConvertTime(parsed, TimeZone);
        }
        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryParse(string s, out DateTime result)
        {
            var parsed = new DateTime();
            bool status = false;
            try
            {
                parsed = DateTime.Parse(s);
                status = true;
            }
            catch
            {
                status = false;
            }
            result = parsed;
            return status;
        }
        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryParse(string s, IFormatProvider provider, out DateTime result)
        {
            var parsed = new DateTime();
            bool status = false;
            try
            {
                parsed = DateTime.Parse(s, provider);
                status = true;
            }
            catch
            {
                status = false;
            }
            result = parsed;
            return status;
        }
        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="styles">The styles.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryParse(string s, IFormatProvider provider, System.Globalization.DateTimeStyles styles, out DateTime result)
        {
            var parsed = new DateTime();
            bool status = false;
            try
            {
                parsed = DateTime.Parse(s, provider, styles);
                status = true;
            }
            catch
            {
                status = false;
            }
            result = parsed;
            return status;
        }
    }
}
