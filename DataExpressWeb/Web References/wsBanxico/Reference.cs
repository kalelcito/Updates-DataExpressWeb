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

namespace DataExpressWeb.wsBanxico {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="DgieWSPortSoapBinding", Namespace="http://ws.dgie.banxico.org.mx")]
    public partial class DgieWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback reservasInternacionalesBanxicoOperationCompleted;
        
        private System.Threading.SendOrPostCallback tasasDeInteresBanxicoOperationCompleted;
        
        private System.Threading.SendOrPostCallback tiposDeCambioBanxicoOperationCompleted;
        
        private System.Threading.SendOrPostCallback udisBanxicoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public DgieWS() {
            this.Url = global::DataExpressWeb.Properties.Settings.Default.DataExpressWeb_wsBanxico_DgieWS;
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
        public event reservasInternacionalesBanxicoCompletedEventHandler reservasInternacionalesBanxicoCompleted;
        
        /// <remarks/>
        public event tasasDeInteresBanxicoCompletedEventHandler tasasDeInteresBanxicoCompleted;
        
        /// <remarks/>
        public event tiposDeCambioBanxicoCompletedEventHandler tiposDeCambioBanxicoCompleted;
        
        /// <remarks/>
        public event udisBanxicoCompletedEventHandler udisBanxicoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.dgie.banxico.org.mx", ResponseNamespace="http://ws.dgie.banxico.org.mx")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public string reservasInternacionalesBanxico() {
            object[] results = this.Invoke("reservasInternacionalesBanxico", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void reservasInternacionalesBanxicoAsync() {
            this.reservasInternacionalesBanxicoAsync(null);
        }
        
        /// <remarks/>
        public void reservasInternacionalesBanxicoAsync(object userState) {
            if ((this.reservasInternacionalesBanxicoOperationCompleted == null)) {
                this.reservasInternacionalesBanxicoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnreservasInternacionalesBanxicoOperationCompleted);
            }
            this.InvokeAsync("reservasInternacionalesBanxico", new object[0], this.reservasInternacionalesBanxicoOperationCompleted, userState);
        }
        
        private void OnreservasInternacionalesBanxicoOperationCompleted(object arg) {
            if ((this.reservasInternacionalesBanxicoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.reservasInternacionalesBanxicoCompleted(this, new reservasInternacionalesBanxicoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.dgie.banxico.org.mx", ResponseNamespace="http://ws.dgie.banxico.org.mx")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public string tasasDeInteresBanxico() {
            object[] results = this.Invoke("tasasDeInteresBanxico", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void tasasDeInteresBanxicoAsync() {
            this.tasasDeInteresBanxicoAsync(null);
        }
        
        /// <remarks/>
        public void tasasDeInteresBanxicoAsync(object userState) {
            if ((this.tasasDeInteresBanxicoOperationCompleted == null)) {
                this.tasasDeInteresBanxicoOperationCompleted = new System.Threading.SendOrPostCallback(this.OntasasDeInteresBanxicoOperationCompleted);
            }
            this.InvokeAsync("tasasDeInteresBanxico", new object[0], this.tasasDeInteresBanxicoOperationCompleted, userState);
        }
        
        private void OntasasDeInteresBanxicoOperationCompleted(object arg) {
            if ((this.tasasDeInteresBanxicoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.tasasDeInteresBanxicoCompleted(this, new tasasDeInteresBanxicoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.dgie.banxico.org.mx", ResponseNamespace="http://ws.dgie.banxico.org.mx")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public string tiposDeCambioBanxico() {
            object[] results = this.Invoke("tiposDeCambioBanxico", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void tiposDeCambioBanxicoAsync() {
            this.tiposDeCambioBanxicoAsync(null);
        }
        
        /// <remarks/>
        public void tiposDeCambioBanxicoAsync(object userState) {
            if ((this.tiposDeCambioBanxicoOperationCompleted == null)) {
                this.tiposDeCambioBanxicoOperationCompleted = new System.Threading.SendOrPostCallback(this.OntiposDeCambioBanxicoOperationCompleted);
            }
            this.InvokeAsync("tiposDeCambioBanxico", new object[0], this.tiposDeCambioBanxicoOperationCompleted, userState);
        }
        
        private void OntiposDeCambioBanxicoOperationCompleted(object arg) {
            if ((this.tiposDeCambioBanxicoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.tiposDeCambioBanxicoCompleted(this, new tiposDeCambioBanxicoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.dgie.banxico.org.mx", ResponseNamespace="http://ws.dgie.banxico.org.mx")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public string udisBanxico() {
            object[] results = this.Invoke("udisBanxico", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void udisBanxicoAsync() {
            this.udisBanxicoAsync(null);
        }
        
        /// <remarks/>
        public void udisBanxicoAsync(object userState) {
            if ((this.udisBanxicoOperationCompleted == null)) {
                this.udisBanxicoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnudisBanxicoOperationCompleted);
            }
            this.InvokeAsync("udisBanxico", new object[0], this.udisBanxicoOperationCompleted, userState);
        }
        
        private void OnudisBanxicoOperationCompleted(object arg) {
            if ((this.udisBanxicoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.udisBanxicoCompleted(this, new udisBanxicoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void reservasInternacionalesBanxicoCompletedEventHandler(object sender, reservasInternacionalesBanxicoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class reservasInternacionalesBanxicoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal reservasInternacionalesBanxicoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void tasasDeInteresBanxicoCompletedEventHandler(object sender, tasasDeInteresBanxicoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tasasDeInteresBanxicoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal tasasDeInteresBanxicoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void tiposDeCambioBanxicoCompletedEventHandler(object sender, tiposDeCambioBanxicoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tiposDeCambioBanxicoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal tiposDeCambioBanxicoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void udisBanxicoCompletedEventHandler(object sender, udisBanxicoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class udisBanxicoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal udisBanxicoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
}

#pragma warning restore 1591