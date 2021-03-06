﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace DataExpressWeb.wsRecepcion {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WsRecepcionSoap", Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
    public partial class WsRecepcion : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ObtenerMensajeOperationCompleted;
        
        private System.Threading.SendOrPostCallback ObtenerMensajeTecnicoOperationCompleted;
        
        private System.Threading.SendOrPostCallback RecibeComprobanteEmisionOperationCompleted;
        
        private System.Threading.SendOrPostCallback RecibeComprobanteOperationCompleted;
        
        private System.Threading.SendOrPostCallback RecibeComplementoPagoOperationCompleted;
        
        private System.Threading.SendOrPostCallback CancelarComprobanteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WsRecepcion() {
            this.Url = global::DataExpressWeb.Properties.Settings.Default.DataExpressWeb_wsRecepcion_WsRecepcion;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ObtenerMensajeCompletedEventHandler ObtenerMensajeCompleted;
        
        /// <remarks/>
        public event ObtenerMensajeTecnicoCompletedEventHandler ObtenerMensajeTecnicoCompleted;
        
        /// <remarks/>
        public event RecibeComprobanteEmisionCompletedEventHandler RecibeComprobanteEmisionCompleted;
        
        /// <remarks/>
        public event RecibeComprobanteCompletedEventHandler RecibeComprobanteCompleted;
        
        /// <remarks/>
        public event RecibeComplementoPagoCompletedEventHandler RecibeComplementoPagoCompleted;
        
        /// <remarks/>
        public event CancelarComprobanteCompletedEventHandler CancelarComprobanteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerMensaje", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ObtenerMensaje() {
            object[] results = this.Invoke("ObtenerMensaje", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ObtenerMensajeAsync() {
            this.ObtenerMensajeAsync(null);
        }
        
        /// <remarks/>
        public void ObtenerMensajeAsync(object userState) {
            if ((this.ObtenerMensajeOperationCompleted == null)) {
                this.ObtenerMensajeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerMensajeOperationCompleted);
            }
            this.InvokeAsync("ObtenerMensaje", new object[0], this.ObtenerMensajeOperationCompleted, userState);
        }
        
        private void OnObtenerMensajeOperationCompleted(object arg) {
            if ((this.ObtenerMensajeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ObtenerMensajeCompleted(this, new ObtenerMensajeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerMensajeTecnico", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ObtenerMensajeTecnico() {
            object[] results = this.Invoke("ObtenerMensajeTecnico", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ObtenerMensajeTecnicoAsync() {
            this.ObtenerMensajeTecnicoAsync(null);
        }
        
        /// <remarks/>
        public void ObtenerMensajeTecnicoAsync(object userState) {
            if ((this.ObtenerMensajeTecnicoOperationCompleted == null)) {
                this.ObtenerMensajeTecnicoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerMensajeTecnicoOperationCompleted);
            }
            this.InvokeAsync("ObtenerMensajeTecnico", new object[0], this.ObtenerMensajeTecnicoOperationCompleted, userState);
        }
        
        private void OnObtenerMensajeTecnicoOperationCompleted(object arg) {
            if ((this.ObtenerMensajeTecnicoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ObtenerMensajeTecnicoCompleted(this, new ObtenerMensajeTecnicoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RecibeComprobanteEmision", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object[] RecibeComprobanteEmision(string xmlInvoice, string emisorDb) {
            object[] results = this.Invoke("RecibeComprobanteEmision", new object[] {
                        xmlInvoice,
                        emisorDb});
            return ((object[])(results[0]));
        }
        
        /// <remarks/>
        public void RecibeComprobanteEmisionAsync(string xmlInvoice, string emisorDb) {
            this.RecibeComprobanteEmisionAsync(xmlInvoice, emisorDb, null);
        }
        
        /// <remarks/>
        public void RecibeComprobanteEmisionAsync(string xmlInvoice, string emisorDb, object userState) {
            if ((this.RecibeComprobanteEmisionOperationCompleted == null)) {
                this.RecibeComprobanteEmisionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecibeComprobanteEmisionOperationCompleted);
            }
            this.InvokeAsync("RecibeComprobanteEmision", new object[] {
                        xmlInvoice,
                        emisorDb}, this.RecibeComprobanteEmisionOperationCompleted, userState);
        }
        
        private void OnRecibeComprobanteEmisionOperationCompleted(object arg) {
            if ((this.RecibeComprobanteEmisionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RecibeComprobanteEmisionCompleted(this, new RecibeComprobanteEmisionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RecibeComprobante", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object[] RecibeComprobante(string xmlInvoice, string idEmpleado, string emisorDb, bool manual, bool pdf, string ordenFilename) {
            object[] results = this.Invoke("RecibeComprobante", new object[] {
                        xmlInvoice,
                        idEmpleado,
                        emisorDb,
                        manual,
                        pdf,
                        ordenFilename});
            return ((object[])(results[0]));
        }
        
        /// <remarks/>
        public void RecibeComprobanteAsync(string xmlInvoice, string idEmpleado, string emisorDb, bool manual, bool pdf, string ordenFilename) {
            this.RecibeComprobanteAsync(xmlInvoice, idEmpleado, emisorDb, manual, pdf, ordenFilename, null);
        }
        
        /// <remarks/>
        public void RecibeComprobanteAsync(string xmlInvoice, string idEmpleado, string emisorDb, bool manual, bool pdf, string ordenFilename, object userState) {
            if ((this.RecibeComprobanteOperationCompleted == null)) {
                this.RecibeComprobanteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecibeComprobanteOperationCompleted);
            }
            this.InvokeAsync("RecibeComprobante", new object[] {
                        xmlInvoice,
                        idEmpleado,
                        emisorDb,
                        manual,
                        pdf,
                        ordenFilename}, this.RecibeComprobanteOperationCompleted, userState);
        }
        
        private void OnRecibeComprobanteOperationCompleted(object arg) {
            if ((this.RecibeComprobanteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RecibeComprobanteCompleted(this, new RecibeComprobanteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RecibeComplementoPago", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object[] RecibeComplementoPago(string idComprobante, string jsonPagos, string[] xmlPagos, string idEmpleado, string emisorDb) {
            object[] results = this.Invoke("RecibeComplementoPago", new object[] {
                        idComprobante,
                        jsonPagos,
                        xmlPagos,
                        idEmpleado,
                        emisorDb});
            return ((object[])(results[0]));
        }
        
        /// <remarks/>
        public void RecibeComplementoPagoAsync(string idComprobante, string jsonPagos, string[] xmlPagos, string idEmpleado, string emisorDb) {
            this.RecibeComplementoPagoAsync(idComprobante, jsonPagos, xmlPagos, idEmpleado, emisorDb, null);
        }
        
        /// <remarks/>
        public void RecibeComplementoPagoAsync(string idComprobante, string jsonPagos, string[] xmlPagos, string idEmpleado, string emisorDb, object userState) {
            if ((this.RecibeComplementoPagoOperationCompleted == null)) {
                this.RecibeComplementoPagoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecibeComplementoPagoOperationCompleted);
            }
            this.InvokeAsync("RecibeComplementoPago", new object[] {
                        idComprobante,
                        jsonPagos,
                        xmlPagos,
                        idEmpleado,
                        emisorDb}, this.RecibeComplementoPagoOperationCompleted, userState);
        }
        
        private void OnRecibeComplementoPagoOperationCompleted(object arg) {
            if ((this.RecibeComplementoPagoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RecibeComplementoPagoCompleted(this, new RecibeComplementoPagoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CancelarComprobante", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool CancelarComprobante(string idComprobante, string idEmpleado, string emisorDb) {
            object[] results = this.Invoke("CancelarComprobante", new object[] {
                        idComprobante,
                        idEmpleado,
                        emisorDb});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void CancelarComprobanteAsync(string idComprobante, string idEmpleado, string emisorDb) {
            this.CancelarComprobanteAsync(idComprobante, idEmpleado, emisorDb, null);
        }
        
        /// <remarks/>
        public void CancelarComprobanteAsync(string idComprobante, string idEmpleado, string emisorDb, object userState) {
            if ((this.CancelarComprobanteOperationCompleted == null)) {
                this.CancelarComprobanteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCancelarComprobanteOperationCompleted);
            }
            this.InvokeAsync("CancelarComprobante", new object[] {
                        idComprobante,
                        idEmpleado,
                        emisorDb}, this.CancelarComprobanteOperationCompleted, userState);
        }
        
        private void OnCancelarComprobanteOperationCompleted(object arg) {
            if ((this.CancelarComprobanteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CancelarComprobanteCompleted(this, new CancelarComprobanteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void ObtenerMensajeCompletedEventHandler(object sender, ObtenerMensajeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ObtenerMensajeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ObtenerMensajeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void ObtenerMensajeTecnicoCompletedEventHandler(object sender, ObtenerMensajeTecnicoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ObtenerMensajeTecnicoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ObtenerMensajeTecnicoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void RecibeComprobanteEmisionCompletedEventHandler(object sender, RecibeComprobanteEmisionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecibeComprobanteEmisionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RecibeComprobanteEmisionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void RecibeComprobanteCompletedEventHandler(object sender, RecibeComprobanteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecibeComprobanteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RecibeComprobanteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void RecibeComplementoPagoCompletedEventHandler(object sender, RecibeComplementoPagoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecibeComplementoPagoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RecibeComplementoPagoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void CancelarComprobanteCompletedEventHandler(object sender, CancelarComprobanteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CancelarComprobanteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CancelarComprobanteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591