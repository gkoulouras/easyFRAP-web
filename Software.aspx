<%@ Page Title="easyFRAP-web | Software & Libraries" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Software.aspx.vb" Inherits="Software" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta name="description" content="Welcome to easyFRAP-web. EasyFRAP-web is a web-based application which facilitates qualitative and quantitative analysis of Fluorecence Recovery After Photobleaching (FRAP) data." />
    <script type="text/javascript" src="Scripts/jquery-3.1.1.js"></script>
    <script type="text/javascript" src="Scripts/dygraph-combined.js"> </script>
    <script type="text/javascript" src="Scripts/dygraph-extra.js"></script>
    <script type="text/javascript" src="Scripts/synchronizer.js"></script>
    <script type="text/javascript" src="Scripts/toastr.js"></script>
    <script type="text/javascript" src="Scripts/sweetalert.min.js"></script>
    <script type="text/javascript" src="Scripts/spin.js"></script>
    <script type="text/javascript" src="Scripts/bootbox.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-3.1.1.js"></script>
    <script type="text/javascript" src="Scripts/download.js"></script>
    <script src="//d3js.org/d3.v3.min.js"></script> 

<%--    <script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-96619605-1', 'auto');
  ga('send', 'pageview');

   </script>--%>

<div class="container">
<ul class="breadcrumb">
  <li class="active">Software & Libraries</li>
</ul>

    
<div class="col-md-12">
    <div class="form-group">
            <span class="header-block"><a id="software" name="software" href="#"></a>Software</span>
            <p><i>EasyFRAP-web</i> is built using the following core software technologies: </p>   
            <ul class="b">
                    <li><p><a href="https://msdn.microsoft.com/en-us/library/w0x726c2(v=vs.110).aspx" target="_blank">Micrsoft ASP.NET Framework (v4.5)</a></p></li>
                    <li><p><a href="https://www.microsoft.com/en-us/sql-server/sql-server-2016" target="_blank">Microsoft SQL Server 2016</a></p></li>
                    <li><p><a href="https://www.microsoft.com/en-us/sql-server/sql-server-r-services" target="_blank">SQL Server R Services</a></p></li>
           </ul> 
    </div>
    <br />
    <div class="form-group">
            <span class="header-block"><a id="libraries" name="libraries" href="#"></a>Third-party Libraries</span>
            <p>Extensive object-oriented programming techniques in conjuction with the embodiment of the following open source Javascript Libraries are involved in the development of the <i>easyFRAP-web</i> to ensure the quality and the flexibility of the work.</p>   
            <ul class="b">
                    <li><p><a href="https://mdbootstrap.com/" target="_blank">Material Design for Bootstrap Framework</a></p></li>
                    <li><p><a href="http://dygraphs.com/" target="_blank">Dygraphs - charting library</a></p></li>
                    <li><p><a href="http://t4t5.github.io/sweetalert/" target="_blank">Sweet Alert - custom pop up boxes</a></p></li>
                    <li><p><a href="https://codeseven.github.io/toastr/" target="_blank">Toastr - toast notifications</a></p></li>
           </ul> 
    </div>
</div>


</div>
</asp:Content>
