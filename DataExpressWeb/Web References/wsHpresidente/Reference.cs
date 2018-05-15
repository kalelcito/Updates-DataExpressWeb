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

namespace DataExpressWeb.wsHpresidente {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="wsHpresidenteSoap", Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
    public partial class wsHpresidente : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback RecibeInfoTxtOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public wsHpresidente() {
            this.Url = global::DataExpressWeb.Properties.Settings.Default.DataExpressWeb_wsHpresidente_wsHpresidente;
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
        public event RecibeInfoTxtCompletedEventHandler RecibeInfoTxtCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RecibeInfoTxt", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object[] RecibeInfoTxt(string txtInvoice, string idTrama, string rfcBase, [System.Xml.Serialization.XmlArrayItemAttribute("ArrayOfString")] [System.Xml.Serialization.XmlArrayItemAttribute(NestingLevel=1)] string[][] series, bool facturar) {
            object[] results = this.Invoke("RecibeInfoTxt", new object[] {
                        txtInvoice,
                        idTrama,
                        rfcBase,
                        series,
                        facturar});
            return ((object[])(results[0]));
        }
        
        /// <remarks/>
        public void RecibeInfoTxtAsync(string txtInvoice, string idTrama, string rfcBase, string[][] series, bool facturar) {
            this.RecibeInfoTxtAsync(txtInvoice, idTrama, rfcBase, series, facturar, null);
        }
        
        /// <remarks/>
        public void RecibeInfoTxtAsync(string txtInvoice, string idTrama, string rfcBase, string[][] series, bool facturar, object userState) {
            if ((this.RecibeInfoTxtOperationCompleted == null)) {
                this.RecibeInfoTxtOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecibeInfoTxtOperationCompleted);
            }
            this.InvokeAsync("RecibeInfoTxt", new object[] {
                        txtInvoice,
                        idTrama,
                        rfcBase,
                        series,
                        facturar}, this.RecibeInfoTxtOperationCompleted, userState);
        }
        
        private void OnRecibeInfoTxtOperationCompleted(object arg) {
            if ((this.RecibeInfoTxtCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RecibeInfoTxtCompleted(this, new RecibeInfoTxtCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void RecibeInfoTxtCompletedEventHandler(object sender, RecibeInfoTxtCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecibeInfoTxtCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RecibeInfoTxtCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
}

#pragma warning restore 1591