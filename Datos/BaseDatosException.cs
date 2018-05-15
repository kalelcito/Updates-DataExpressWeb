// ***********************************************************************
// Assembly         : Datos
// Author           : Sergio Hernández
// Created          : 07-18-2016
//
// Last Modified By : Sergio Hernández
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="BaseDatosException.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Datos
{
    /// <summary>
    /// Representa un error de acceso a la base de datos.
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    public class BaseDatosException : ApplicationException
    {
        /// <summary>
        /// Construye una instancia en base a un mensaje de error y la una excepción original.
        /// </summary>
        /// <param name="mensaje">El mensaje de error.</param>
        /// <param name="original">La excepción original.</param>
        public BaseDatosException(string mensaje, Exception original) : base(mensaje, original)
        {
        }

        /// <summary>
        /// Construye una instancia en base a un mensaje de error.
        /// </summary>
        /// <param name="mensaje">El mensaje de error.</param>
        public BaseDatosException(string mensaje) : base(mensaje)
        {
        }
    }
}