// ***********************************************************************
// Assembly         : DataExpressWeb
// Author           : Sergio
// Created          : 09-07-2016
//
// Last Modified By : Sergio
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="RawPrinterHelper.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DataExpressWeb
{
    /// <summary>
    /// Class RawPrinterHelper.
    /// </summary>
    public class RawPrinterHelper
    {
        /// <summary>
        /// Opens the printer.
        /// </summary>
        /// <param name="szPrinter">The sz printer.</param>
        /// <param name="hPrinter">The h printer.</param>
        /// <param name="pd">The pd.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        /// <summary>
        /// Sends the file to printer.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="NotImplementedException"></exception>
        internal static void SendFileToPrinter(string path, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Closes the printer.
        /// </summary>
        /// <param name="hPrinter">The h printer.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        /// <summary>
        /// Starts the document printer.
        /// </summary>
        /// <param name="hPrinter">The h printer.</param>
        /// <param name="level">The level.</param>
        /// <param name="di">The di.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] Docinfoa di);

        /// <summary>
        /// Ends the document printer.
        /// </summary>
        /// <param name="hPrinter">The h printer.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        /// <summary>
        /// Starts the page printer.
        /// </summary>
        /// <param name="hPrinter">The h printer.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        /// <summary>
        /// Ends the page printer.
        /// </summary>
        /// <param name="hPrinter">The h printer.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        /// <summary>
        /// Writes the printer.
        /// </summary>
        /// <param name="hPrinter">The h printer.</param>
        /// <param name="pBytes">The p bytes.</param>
        /// <param name="dwCount">The dw count.</param>
        /// <param name="dwWritten">The dw written.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        /// <summary>
        /// Sends the bytes to printer.
        /// </summary>
        /// <param name="szPrinterName">Name of the sz printer.</param>
        /// <param name="pBytes">The p bytes.</param>
        /// <param name="dwCount">The dw count.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, int dwCount)
        {
            int dwError = 0, dwWritten = 0;
            var hPrinter = new IntPtr(0);
            var di = new Docinfoa();
            var bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        /// <summary>
        /// Sends the bytes to printer.
        /// </summary>
        /// <param name="szPrinterName">Name of the sz printer.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SendBytesToPrinter(string szPrinterName, byte[] data)
        {
            var retval = false;
            var pUnmanagedBytes = Marshal.AllocCoTaskMem(data.Length); //Allocate unmanaged memory
            Marshal.Copy(data, 0, pUnmanagedBytes, data.Length); //Copy bytes into unmanaged memory
            retval = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, data.Length); //Send bytes to printer
            Marshal.FreeCoTaskMem(pUnmanagedBytes); // Free the allocated unmanaged memory
            return retval;
        }

        /// <summary>
        /// Sends the file to printer.
        /// </summary>
        /// <param name="szPrinterName">Name of the sz printer.</param>
        /// <param name="szFileName">Name of the sz file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            var fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            var br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            var bytes = new byte[fs.Length];
            var bSuccess = false;
            // Your unmanaged pointer.
            var pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }

        /// <summary>
        /// Sends the string to printer.
        /// </summary>
        /// <param name="szPrinterName">Name of the sz printer.</param>
        /// <param name="szString">The sz string.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            int dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        // Structure and API declarions:
        /// <summary>
        /// Class Docinfoa.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class Docinfoa
        {
            /// <summary>
            /// The p data type
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;

            /// <summary>
            /// The p document name
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;

            /// <summary>
            /// The p output file
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
        }
    }
}