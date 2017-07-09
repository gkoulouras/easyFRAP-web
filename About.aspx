<%@ Page Title="easyFRAP-web | About us" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.vb" Inherits="About" %>

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
  <li class="active">About us</li>
</ul>


  <div class="form-horizontal">

    <div class="col-md-12">
        <div class="form-group">
            <span class="header-block"><a id="aboutus" name="aboutus" href="#aboutus"></a>About easyFRAP-web</span>
            <p>EasyFRAP-web is a web-based application which facilitates qualitative and quantitative analysis of Fluorecence Recovery After Photobleaching (FRAP) data.</p>
            <p>It is developed and maintained by the <a href="http://ccl.med.upatras.gr/" target="_blank">Cell Cycle Laboratory</a>, University of Patras, Greece. The <a href="GettingStarted.aspx">How to use</a> and the <a href="FAQ.aspx">FAQ</a> categories contain useful information about how to use easyFRAP-web. Under Downloads, <a href="<%= Page.ResolveUrl("~")%>sample_files.zip">sample data</a> for experimenting with easyFRAP-web, the easyFRAP <a href="<%= Page.ResolveUrl("~")%>easyfrap_web_manual_appendix.pdf" target="_blank">manual appendix</a> which details definitions and formulas used for analysis, and a <a href="http://ccl.med.upatras.gr/index.php?id=easyfrap" target="_blank">link</a> to the standalone version of easyFRAP can be found.</p>     
        </div>
    </div>

    <div class="col-md-12">
        <div class="form-group">
            <span class="header-block"><a id="contact" name="contact" href="#contact"></a>Contact</span>
            <p>For questions, suggestions, bug-reports or feedback, please email us:</p>
                <ul class="b">
                    <li><address><a href="mailto:easyfrap@upatras.gr">easyfrap@upatras.gr</a></address></li>
                </ul>
        </div>
    </div>

    <div class="col-md-12">
        <div class="form-group">
            <span class="header-block"><a id="publications" name="publications" href="#publications"></a>Publications</span>
            <p>For more information consult these publications:</p>
                <ul class="b">
                    <li><p>Koulouras G, Rapsomaniki MA, Giakoumakis NN, Panagopoulos A, Taraviras S and Lygerou Z (2017). <b><i>EasyFRAP-web: a web-based tool for the analysis of Fluorescence Recovery After Photo-bleaching (FRAP) data.</i></b> (Manuscript submitted for publication) </p></li>
                    <li><p>Rapsomaniki MA, Kotsantis P, Symeonidou IE, Giakoumakis NN, Taraviras S and Lygerou Z (2012). <b><i>easyFRAP: an interactive, easy-to-use tool for qualitative and quantitative analysis of Fluorescence Recovery After Photobleaching data.</i></b> Bioinformatics. 28(13):1800-1801</p></li>
                    <li><p>Giakoumakis NN, Rapsomaniki MA and Lygerou Z (2017). <b><i>Analysis of Protein Kinetics Using Fluorescence Recovery After Photobleaching (FRAP).</i></b> Methods Mol. Biol, 1563: 243-267</p></li>
                    <li><p>Rapsomaniki MA, Cinquemani E, Giakoumakis NN, Kotsantis P, Lygeros J, Lygerou Z (2015). <b><i>Inference of protein kinetics by stochastic modeling and simulation of fluorescence recovery after photobleaching experiments.</i></b> Bioinformatics 31:355-362.</p></li>
                </ul>
        </div>
    </div>

    <div class="col-md-12">
        <div class="form-group">
            <span class="header-block"><a id="aboutthealb" name="aboutthealb" href="#aboutthelab"></a>About the Cell Cycle Laboratory</span>
            <p>The <a href="http://ccl.med.upatras.gr/" target="_blank">Cell Cycle Laboratory</a>, headed by Prof. Zoi Lygerou, is studying cell cycle control and the maintenance of genome integrity. We combine functional live-cell imaging approaches to modeling and in silico analysis to study protein complexes maintaining genome stability across evolution.</p>
        </div>
    </div>

      <div>

      </div>
  </div>
</div>



</asp:Content>
