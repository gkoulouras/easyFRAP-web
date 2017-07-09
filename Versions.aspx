<%@ Page Title="easyFRAP-web | Release History" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Versions.aspx.vb" Inherits="Versions" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta name="description" content="Welcome to easyFRAP-web. EasyFRAP-web is a web-based application which facilitates qualitative and quantitative analysis of Fluorecence Recovery After Photobleaching (FRAP) data." />
        
<%--<script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-96619605-1', 'auto');
  ga('send', 'pageview');

</script>--%>

<div class="container">
<ul class="breadcrumb">
  <li class="active">Release History</li>
</ul>

    <p>The following table summarizes the release history for the EasyFRAP-web application.</p>

<table class="table table-hover table-bordered">
  <thead>
    <tr>
      <th class="text-center">Version</th>
      <th class="text-center">Release Date</th>
      <th class="text-center">Significant Changes</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <th scope="row" class="text-center">1.1</th>
      <td class="text-center">4 Jun 2017</td>
      <td>&#9900; Added functionality for the uploading of Excel 97-2003 files(.xls)</td>
    </tr>
  </tbody>
  <tbody>
    <tr>
      <th scope="row" class="text-center">1.0</th>
      <td class="text-center">15 Mar 2017</td>
      <td>&#9900; First Release</td>
    </tr>
  </tbody>
</table>

</div>
</asp:Content>
