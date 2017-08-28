<%@ Page Title="easyFRAP-web | Getting Started" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="GettingStarted.aspx.vb" Inherits="GettingStarted" %>

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

<script>
    $().ready(function() {
        var $scrollingDiv = $("#scrollingDiv");

        $(window).scroll(function(){            
            $scrollingDiv
                .stop()
                .animate({"marginTop": ($(window).scrollTop() )}, "slow" );         
        });
    });
</script>
<script>
    /* Set the width of the side navigation to 250px */
    function openNav() {
        document.getElementById("mySidenav").style.width = "100%"; /*"250px";*/
    }

    /* Set the width of the side navigation to 0 */
    function closeNav() {
        document.getElementById("mySidenav").style.width = "0";
    }
</script>
<div id="hp-ctn-howItWorks" class="scrollingDiv" style="position:fixed; top: 30%;">
        <p><span onclick="openNav()">Sections</span></p>
</div>  
<div class="container">

<ul class="breadcrumb">
  <li class="active">User Documentation for EasyFRAP-web</li>
</ul>


  <div class="form-horizontal">
    
    <div id="mySidenav" class="sidenav">
        <a href="javascript:void(0)" class="closebtn" onclick="closeNav()">&times;</a>
        <a href="#getstarted" onclick="closeNav()">Getting Started</a>
        <a href="#dataupload" onclick="closeNav()">Data Upload Panel</a>
        <a href="#rawdatavisualization" onclick="closeNav()">Raw Data Visualization</a>
        <a href="#bleachingdepthgapratio" onclick="closeNav()">Bleaching Depth - Gap Ratio Panel</a>
        <a href="#normalizationpanel" onclick="closeNav()">Normalization Panel</a>
        <a href="#curvefitting" onclick="closeNav()">Curve Fitting Panel</a>
        <a href="#deletedataset" onclick="closeNav()">Remove Data Panel</a>
    </div>
    
    
    <div class="col-md-12">
        <div class="form-group">
            <span class="header-block"><a id="getstarted" name="getstarted" href="#getstarted"></a>Getting Started</span>
            <p><i>EasyFRAP-web</i> is a web-based application that enables the qualitative and quantitative analysis of Fluorescence Recovery After Photobleaching (FRAP) experimental data. </p>     
        </div>
    </div>
    <div class="col-md-9">
        <div class="form-group">
            <p><i>EasyFRAP-web</i> is compatible with all major FRAP data formats (.csv, .txt, .xlsx). It is designed to use measurements from the region of interest (ROI1), the area of total fluorescence (ROI2) and a non-fluorescent, background region (ROI3) in each cell analyzed, over time (Fig. 1). In a typical FRAP experiment, 20-50 single cells are analyzed per experimental point. For more information on how to perform FRAP analysis see <a href="https://www.ncbi.nlm.nih.gov/pubmed/28324613" target="_blank">Giakoumakis et al, 2017</a>. </p>
               <p><i>EasyFRAP-web</i> is designed as a single screen application which consists of six sections progressively activated, in order to simplify the steps of the analysis. The main functionalities of <i>EasyFRAP-web</i> include data processing, computation of parameters that allow experimental assessment, normalization through different methods, estimation of quantitative parameters via curve fitting (fraction of mobile molecules and the time of half-maximal recovery), as well as data and figure export for further analysis.</p>
            <p>To use <i>EasyFRAP-web</i>, the following elements are needed: </p>
                <ul class="b">
                    <li><p>A modern web browser. <i>EasyFRAP-web</i> is built using web standards that allow the application to run on Windows, Mac, Linux, iOS, Android or any other operating system with a modern web browser. It is highly recommended to update to the latest version of the preferred browser. The current version of the application was fully tested both on Google Chrome (v.56) and Mozilla Firefox (v.51), which are recommended.</p></li>
                    <li><p>An Internet connection. A broadband Internet connection improves performance and is highly recommended, especially in case of large datasets.</p></li>
                </ul>
            
        </div>
    </div>
    <div class="col-md-3 text-center">
        <div class="form-group">
            <asp:Image ID="ROIimage" runat="server" ImageUrl="img/ROIimage.png" Height="80%" Width="80%" AlternateText="Regions of Interest"/>
            <br />
            <span class="help-block">Figure 1: Regions of Interest in a cell</span>
        </div>
    </div>

    
    <div class="col-md-12">
        <div class="form-group">
            <br />
            <span class="header-block"><a id="dataupload" name="dataupload" href="#dataupload"></a>Data Upload Panel</span>
            <p>The first step of the analysis pipeline regards data uploading. The user is asked to choose the corresponding file format and subsequently to indicate the appropriate files which contain the experimental data. Multiple individual files can be uploaded by holding down the <i>Ctrl</i> key. Naming the experiment is optional, but advisable. Every field marked with an asterisk is required. Sample data for experimenting with easyFRAP-web can be found <a href="<%= Page.ResolveUrl("~")%>sample_files.zip">here</a>.</p>     
            <asp:Image ID="DatasetSelection" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Dataset_Selection.png" Height="60%" Width="85%" AlternateText="Dataset Selection"/>
            <p>The <i>RESET ALL</i> button deletes all previously uploaded files. By clicking the <i>UPLOAD</i> button, a modal window appears which allows the order of the columns in the selected files to be specified.</p>
            <asp:Image ID="ColumnOrder" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Column_Order.png" Height="40%" Width="40%" AlternateText="Column Order"/>
            <p>The next screen after a successful upload depicts the <i>Dataset Selection</i> section. A graphical table displays information of the uploaded files. Any file(s) can be excluded or restored at any time from the table. </p>
            <asp:Image ID="DatasetSelection2" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Dataset_Selection2.png" Height="60%" Width="85%" AlternateText="Dataset Selection 2"/>

        </div>
    </div>
    
      <div class="col-md-12">
        <div class="form-group">
            <br />
            <span class="header-block"><a id="rawdatavisualization" name="rawdatavisualization" href="#rawdatavisualization"></a>Raw Data Visualization</span>
            <p>At the same time the raw recovery curves in ROI1, ROI2 and ROI3 are automatically visualized. The graphs of ROI1, 2 and 3 are interactive in several ways. Zoom in/out, drag on selection, save graphs as images at a specific focus, show coordinates on click, synchronization between graphs while zooming as well as panning are some of the key features that are introduced in this tool. When files are excluded or restored from the Dataset Selection table, all graphs are dynamically redesigned.</p>
        <div class="row">
        <div class="col-md-6">
            <asp:Image ID="ROI1" runat="server" CssClass="img-responsive center-block" ImageUrl="img/ROI1.png" Height="100%" Width="100%" AlternateText="Region of Interest 1"/>
        </div>
        <div class="col-md-6">
            <asp:Image ID="ROI2" runat="server" CssClass="img-responsive center-block" ImageUrl="img/ROI2.png" Height="100%" Width="100%" AlternateText="Region of Interest 2"/>
        </div>
        </div>
        <div class="row">
        <div class="col-md-6 col-md-offset-3">
            <asp:Image ID="ROI3" runat="server" CssClass="img-responsive center-block" ImageUrl="img/ROI3.png" Height="100%" Width="100%" AlternateText="Region of Interest 3"/>
        </div>
        </div>
        
            <p>A panel depicting which curves are displayed can optionally be visualized. Labels include information with regards to the fluorescence intensity of each designated curve when the mouse is moved over a specific time-point. By default, labeling is enabled in every group of graphs. This option can simply be disabled by switching the corresponding element to the left. The deactivation of this functionality may be useful in case of very large data-sets.</p>
        <div class="row">
        <div class="col-md-8 col-md-offset-2">
            <asp:Image ID="enablelabeling" runat="server" CssClass="img-responsive center-block" ImageUrl="img/labeling.png" Height="100%" Width="100%" AlternateText="Enable/Disable labeling"/>
        </div>
        </div>
            
        <div class="row">
        <div class="col-md-4 col-md-offset-4">
            <asp:Image ID="save_export1" runat="server" CssClass="img-responsive center-block" ImageUrl="img/save_export1.png" Height="100%" Width="100%" AlternateText="Region of Interest 3"/>
        </div>
        </div>
            <p>The graphs can be exported as image files at any desirable zoom level for better examination by pressing the <i>SAVE GRAPHS</i> button. Raw data points can also be exported in a separate .xlsx file in a such way that enables detailed comparisons between the time points.</p>
       </div>
     </div>
    
      <div class="col-md-12">
      <div class="form-group">
            <br />
            <span class="header-block"><a id="bleachingdepthgapratio" name="bleachingdepthgapratio" href="#bleachingdepthgapratio"></a>Bleaching Depth - Gap Ratio Panel</span>
            <p>The next step concerns the estimation of the <i>Bleaching Depth</i> and the <i>Gap Ratio</i>. The user is asked to insert the number of pre-bleach, bleach and post-bleach images in order to calculate these metrics. If the values are incorrect or if they are not compatible with the data (the program checks if their sum is equal to the number of lines in the files), different error messages are returned. Optionally, a number of initial pre-bleach values can be deleted, as they exhibit loss of fluorescence due to non intentional bleaching (the proposed value for initial values to be discarded is 10).</p>
            <asp:Image ID="BleachingDepthGapRatio" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Bleaching_Depth_Gap_Ratio.png" Height="60%" Width="85%" AlternateText="Bleaching Depth - Gap Ratio"/>
            <p>The <i>Bleaching Depth</i> parameter gives an estimation of the degree of fluorescence loss in the bleaching region during the bleach. A <i>Bleaching Depth</i> of 1 corresponds to 100% loss of fluorescence in the bleaching region while a <i>Bleaching Depth</i> of 0 to no loss in fluorescence in the bleaching region. The <i>Gap Ratio</i> is a way to evaluate the amount of total fluorescence remaining in the cell following the bleaching step. A <i>Gap Ratio</i> of 1 corresponds to 100% of total fluorescence remaining while a <i>Gap Ratio</i> of 0 corresponds to total loss of fluorescence in the cell following the bleaching step. Bleaching depth could be evaluated as sufficient when approximately 80% of the fluorescence in ROI1 is bleached. A small value of bleaching depth (smaller than 0.6) indicates insufficient bleaching of the region of interest. Similarly, gap ratio could be evaluated as acceptable when its value is around 80%. A small value of gap ratio (smaller than 0.6) indicates excessive bleaching. For exact definitions, see the <a href="<%= Page.ResolveUrl("~")%>easyfrap_web_manual_appendix.pdf" target="_blank">easyfrap manual appendix</a>. The <i>RESET</i> button clears the estimated values of this panel.</p>
      </div>
      </div>      
      
      <div class="col-md-12">
      <div class="form-group">    
            <br />
            <span class="header-block"><a id="normalizationpanel" name="normalizationpanel" href="#normalizationpanel"></a>Normalization Panel</span>
            <p>Preprocessing of FRAP data involves the removal of noise, systematic bias and artifacts to produce comparable data. Usually preprocessing involves the following steps:</p>
            <ul class="b">
                <li><p>Subtract background values at each time point to correct for noise and autofluorescence</p></li>
                <li><p>Divide by the total cell intensity at each time point to correct for laser fluctuations, acquisition photobleaching and fluorescence loss during photobleaching</p></li>
                <li><p>Divide by the average pre-bleach intensity to normalize across experiments</p></li>
            </ul>
            <p>EasyFRAP computes the normalized recovery curves according to the two most common formulas used in literature: <i>Double</i> and <i>Full Scale</i>. For exact formulas used see the <a href="<%= Page.ResolveUrl("~")%>easyfrap_web_manual_appendix.pdf" target="_blank">easyfrap manual appendix</a>. By pressing the <i>NORMALIZE</i> button, all samples are normalized according to the selected method and the plots of all normalized samples as well as their mean (± standard deviation) are provided.</p>
        <div class="row">
        <div class="col-md-6">
            <asp:Image ID="DoubleNorm" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Double_Norm.png" Height="100%" Width="100%" AlternateText="Double Normalization"/>
        </div>
        <div class="col-md-6">
            <asp:Image ID="FullScaleNorm" runat="server" CssClass="img-responsive center-block" ImageUrl="img/FullScale_Norm.png" Height="100%" Width="100%" AlternateText="Full Scale Normalization"/>
        </div>
        </div>
        <div class="row">
        <div class="col-md-4 col-md-offset-4">
            <asp:Image ID="save_export2" runat="server" CssClass="img-responsive center-block" ImageUrl="img/save_export2.png" Height="100%" Width="100%" AlternateText="Region of Interest 3"/>
        </div>
        </div>
          <p>At the end of this step both individual normalized curves, mean normalized curves and standard deviation can be exported in a separate .xlsx file. Furthermore, the corresponding graphs can be saved as images.</p>
        </div>  
        </div>
        
    
      <div class="col-md-12">
      <div class="form-group">
        <br />
        <span class="header-block"><a id="curvefitting" name="curvefitting" href="#curvefitting"></a>Curve Fitting Panel</span> 
        <p>The user can select a sample of interest and perform curve fitting using a single or double term exponential equation. The <i>T-half</i> (half maximal recovery time) and <i>Mobile Fraction</i> values (individual and mean values) are computed. The data, fitted curve and the residuals are visualized in order to evaluate the fit. <i>Goodness-of-fit</i> statistics (R<sup>2</sup>) are also provided. More specifically, the program returns the value of R-square, which is the square of the correlation between the response values and the predicted response values. For a detailed presentation of the fitting process, see the <a href="<%= Page.ResolveUrl("~")%>easyfrap_web_manual_appendix.pdf" target="_blank">easyfrap manual appendix</a>.</p>
        <p>To perform curve fitting, the user must select the appropriate equation and press the <i>Perform Curve Fitting</i> button of the corresponding file. To perform curve fitting for the average values of the selected files, the user must press the <i>FIT MEAN DATA</i> button. Please note that fitting a mean curve is not the same as fitting individual curves and obtaining a mean of the computed parameters. The Fit Mean functionality should only be used as a rough estimate of protein kinetics, as it cannot depict cell-to-cell heterogeneity, which is important for interpreting biological behavior.</p>
        <div class="col-md-12">
            <asp:Image ID="CurveFitting1" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Curve_Fitting1.png" Height="60%" Width="85%" AlternateText="Curve Fitting"/>
        </div>  
        <div class="col-md-6">
            <asp:Image ID="CurveFitting2" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Curve_Fitting3.png" Height="100%" Width="100%" AlternateText="Curve Fitting"/>
        </div>
        <div class="col-md-6">
            <asp:Image ID="CurveFitting3" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Curve_Fitting4.png" Height="100%" Width="100%" AlternateText="Curve Fitting"/>
        </div>
        <p>By pressing the <i>SAVE RESULTS</i> button, the user can save fitting results (R-square, T-half, Mobile Fraction for every individual curve and their mean values/standard deviation) in a separate .xlsx file for further use.</p>
    
        <br />
        <span class="header-block"><a id="deletedataset" name="deletedataset" href="#deletedataset"></a>Remove Data Panel</span> 
        <p>It should be noted that the uploaded datasets will not be stored on the server after the end of the analysis. Periodically, all the old records are automatically deleted in order for the system to be maintained at a high performance level. Additionally, the user is able to manually delete the uploaded data after finishing the analysis by pressing the <i>DELETE THE ENTIRE DATASET</i> button. </p>
        <div class="col-md-12">
            <asp:Image ID="RemoveData" runat="server" CssClass="img-responsive center-block" ImageUrl="img/Remove_Data.png" Height="60%" Width="85%" AlternateText="Remove Data"/>
        </div>  
        <p>Finally, data removal is also triggered when the <i>RESET ALL</i> button of the <i>Dataset Selection</i> panel is pressed.</p>
    
    </div>
  </div>
</div>



</asp:Content>
