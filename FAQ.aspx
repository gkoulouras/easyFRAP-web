<%@ Page Title="easyFRAP-web | Frequently Asked Questions" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="FAQ.aspx.vb" Inherits="FAQ" %>

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
  <li class="active">Frequently Asked Questions</li>
</ul>

    <div class="bs-example">
    <span class="help-block"><strong>Tip:</strong> Click on each linked heading question to expand or collapse the corresponding answer.</span>
    <div class="panel-group" id="accordion">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="form-group">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne">What is easyFRAP-web?</a>
                </h6>
            </div>
            <div id="collapseOne" class="panel-collapse collapse">
                <div class="panel-body">
                    <div class="col-md-12">
                        <div class="form-group">
                            <p><i>EasyFRAP-web</i> is a web-based application which facilitates qualitative and quantitative analysis of Fluorecence Recovery After Photobleaching (FRAP) data. It is an easy-to-use software tool that facilitates the fast and interactive analysis of FRAP curves. EasyFRAP-web allows large data-sets to be rapidly evaluated and normalized fluorescence recovery curves to be generated and exported. It also performs curve fitting and provides quantitative parameters (mobile fraction and recovery half time) necessary for comparison across data-sets.</p>
                        </div>
                    </div>                 
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="form-group">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">What file formats can be uploaded to easyFRAP-web?</a>
                </h6>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse">
                <div class="panel-body">
                    <div class="col-md-12">
                        <div class="form-group">
                            <p>The following four file types are compatible with the <i>easyFRAP-web</i> tool:</p>
                            <ul class="b">
                                <li><p>Plain Text (*.txt), tab-delimited files</p></li>
                                <li><p>Comma Separated Values(*.csv), comma-delimited files</p></li>
                                <li><p>Excel 2003(*.xls), spreadsheet files</p></li>
                                <li><p>Excel 2007(*.xlsx), spreadsheet files</p></li>
                            </ul>
                            <p>Each input data file must contain four columns. Three columns which correspond to the raw fluorescence intensity measurements from three Regions of Interest (ROI1, ROI2 and ROI3) and an additional column for the corresponding time points. The order of the columns can be arbitrary. Users will be asked to declare the right order of the columns during the upload step. Sample files can be downloaded from the "Downloads" menu.</p>
                        </div>
                    </div>                
                 </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="form-group">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree">How many files can be simultaneously uploaded to easyFRAP-web?</a>
                </h6>
            </div>
            <div id="collapseThree" class="panel-collapse collapse">
                <div class="panel-body">
                    <div class="col-md-12">
                        <div class="form-group">
                            <p>For security purposes and for service availability, the maximum number of the files that can be uploaded at the same time has been set to 100. Batch mode analysis is supported in the <a href="http://ccl.med.upatras.gr/index.php?id=easyfrap" target="_blank">Standalone Version</a> of the application.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="form-group">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseFour">Who can use easyFRAP-web?</a>
                </h6>
            </div>
            <div id="collapseFour" class="panel-collapse collapse">
                <div class="panel-body">
                    <div class="col-md-12">
                        <div class="form-group">
                            <p>Use of easyFRAP-web tool is free to all users.</p><p>Please cite 'Koulouras G, Rapsomaniki MA, Giakoumakis NN, Panagopoulos A, Taraviras S and Lygerou Z (2017). <b><i>EasyFRAP-web: a web-based tool for the analysis of Fluorescence Recovery After Photo-bleaching (FRAP) data.</i></b>' when using easyFRAP-web.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
	
</div>


</div>
</asp:Content>
