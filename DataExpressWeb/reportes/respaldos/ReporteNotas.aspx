﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReporteNotas.aspx.cs" Inherits="DataExpressWeb.RepNotas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpCabecera" runat="server">
    <style type="text/css">
        .style4
        {
            font-size: large;
        }
        .style5
        {
            width: 274px;
        }
        .style6
        {
            width: 274px;
            font-weight: bold;
            text-align: center;
        }
        .style7
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpCuerpoSup" runat="server">
  <div class="row">
    <div style="width: 15%; display: inline;  float: left; margin-left:2px ">
        <h5> Desde:</h5>
               <div class="input-group date">
                    <asp:TextBox ID="tbFechaInicial" runat="server" CssClass="form-control"  ></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                </div>
    </div >
    <div style="width: 15%; display: inline; float: left; margin-left:2px ">
        <h5> Hasta:</h5>
          <div class="input-group date">
                    <asp:TextBox ID="tbFechaFinal" runat="server" CssClass="form-control"></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                </div>
    </div>
  </div>
    <div class="row">
      <asp:Button ID="bGenerar" CssClass="btn btn-info"  runat="server" onclick="bGenerar_Click" Text="Generar" />
  </div>
    <div class="row">
                <asp:Label ID="Label2" runat="server" ></asp:Label>
  </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="cpTitulo">
    Reporte de Notas de Crédito
</asp:Content>
