// ***********************************************************************
// Assembly         : Control
// Author           : Sergio Hernández
// Created          : 01-29-2017
//
// Last Modified By : Sergio Hernández
// Last Modified On : 01-30-2017
// ***********************************************************************
// <copyright file="GoogleTranslate.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Control
{
    /// <summary>
    /// Class GoogleTranslate.
    /// </summary>
    public static class GoogleTranslate
    {
        /// <summary>
        /// Taken this function reference from http://weblog.west-wind.com/posts/2011/Aug/06/Translating-with-Google-Translate-without-API-and-C-Code
        /// </summary>
        /// <param name="text">Enter the input text to translate</param>
        /// <param name="fromLang">Enter the from culture eg. en or hi</param>
        /// <param name="toLang">Enter the to culture eg. en or hi</param>
        /// <returns>returns translated utf 8 output</returns>
        public static string Translate(string text, string fromLang, string toLang)
        {
            fromLang = fromLang.ToLower();
            toLang = toLang.ToLower();

            var tokens = fromLang.Split('-');
            if (tokens.Length > 1)
            {
                fromLang = tokens[0];
            }

            tokens = toLang.Split('-');
            if (tokens.Length > 1)
            {
                toLang = tokens[0];
            }

            var url = string.Format(@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}", HttpUtility.UrlEncode(text), fromLang, toLang);
            string html = null;
            try
            {
                var web = new WebClient();

                web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
                web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");

                web.Encoding = Encoding.UTF8;
                html = web.DownloadString(url);
            }
            catch (Exception ex)
            {
                return null;
            }
            var result = html.Replace("\"", "");
            return result;
        }
    }
}
