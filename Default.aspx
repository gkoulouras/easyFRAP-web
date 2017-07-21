<%@ Page Title="easyFRAP-web | Main Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta name="description" content="Welcome to easyFRAP-web. EasyFRAP-web is a web-based application which facilitates qualitative and quantitative analysis of Fluorecence Recovery After Photobleaching (FRAP) data." />
 
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



<script>

    function ShowHideLabelsRawData() {
        var checkedstatus = document.getElementById("chcboxshowhidelabels").checked;

        if (checkedstatus === true) {
            var span = document.getElementById("firstgroupofgraphs");
            var txt = document.createTextNode("Labeling is enabled");
            span.innerText = txt.textContent;
            span.className = "label label-primary";
            
            g1.updateOptions({
                legend: 'onmouseover'
            });
            g2.updateOptions({
                legend: 'onmouseover'
            });
            g3.updateOptions({
                legend: 'onmouseover'
            });
        }
        else {
            var span = document.getElementById("firstgroupofgraphs");
            var txt = document.createTextNode("Labeling is disabled");
            span.innerText = txt.textContent;
            span.className = "label label-danger";
            g1.updateOptions({
                legend: 'never'
            });
            g2.updateOptions({
                legend: 'never'
            });
            g3.updateOptions({
                legend: 'never'
            });
        }
        return false;
    }

    function ShowHideLabelsNormData() {
        var checkedstatus = document.getElementById("chcboxshowhidelabelsnormdata").checked;

        if (checkedstatus === true) {
            var span = document.getElementById("secondgroupofgraphs");
            var txt = document.createTextNode("Labeling is enabled");
            span.innerText = txt.textContent;
            span.className = "label label-primary";
            g4.updateOptions({
                legend: 'onmouseover'
            });
            g5.updateOptions({
                legend: 'onmouseover'
            });
        }
        else {
            var span = document.getElementById("secondgroupofgraphs");
            var txt = document.createTextNode("Labeling is disabled");
            span.innerText = txt.textContent;
            span.className = "label label-danger";
            g4.updateOptions({
                legend: 'never'
            });
            g5.updateOptions({
                legend: 'never'
            });
        }
        return false;
    }

    function ShowHideLabelsCurveFitData() {
        var checkedstatus = document.getElementById("chcboxshowhidelabelscurvefitdata").checked;

        if (checkedstatus === true) {
            var span = document.getElementById("thirdgroupofgraphs");
            var txt = document.createTextNode("Labeling is enabled");
            span.innerText = txt.textContent;
            span.className = "label label-primary";
            g6.updateOptions({
                legend: 'onmouseover'
            });
            g7.updateOptions({
                legend: 'onmouseover'
            });
        }
        else {
            var span = document.getElementById("thirdgroupofgraphs");
            var txt = document.createTextNode("Labeling is disabled");
            span.innerText = txt.textContent;
            span.className = "label label-danger";
            g6.updateOptions({
                legend: 'never'
            });
            g7.updateOptions({
                legend: 'never'
            });
        }
        return false;
    }
</script>

<script type="text/javascript">
    var g1 = null; //ROI1 graph
    var g2 = null; //ROI2 graph
    var g3 = null; //ROI3 graph
    var g4 = null; //Normalized graph
    var g5 = null; //Mean-STD graph
    var g6 = null; //Fit Curve graph
    var g7 = null; //Residuals graph

var prm = Sys.WebForms.PageRequestManager.getInstance();

//Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
prm.add_beginRequest(BeginRequestHandler);

// Raised after an asynchronous postback is finished and control has been returned to the browser.
prm.add_endRequest(EndRequestHandler);

function BeginRequestHandler(sender, args) {
//Shows the modal popup - the update progress
var popup = $find('<%= modalPopup.ClientID %>');
if (popup != null) {
popup.show();
}
}

function EndRequestHandler(sender, args) {
//Hide the modal popup - the update progress
var popup = $find('<%= modalPopup.ClientID %>');
if (popup != null) {
    popup.hide();
}
}

    function popupMessage() {
        var popup = document.getElementById("myPopup");
        popup.classList.toggle("show");
}
</script>
 

<div class="container">


<asp:UpdateProgress ID="UpdateProgress" runat="server">
<ProgressTemplate>
<asp:Image ID="Image1" ImageUrl="img/icon-load.gif" AlternateText="Processing" runat="server" />
</ProgressTemplate>
</asp:UpdateProgress>
<ajaxToolkit:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />


<%--<ul class="breadcrumb">--%>
  <%--<li class="active">EasyFRAP Web Edition</li>--%>
<%--</ul> --%>  

    
    <%--Todelete    --%>
<%--    <div class="form-horizontal" >
        <div class="col-md-12 ">
            <span class="help-block" style="color: red;"><%= sCurrentSession %></span>
        </div>
    </div>--%>
                   

<asp:UpdatePanel ID="updpanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

    <section id="datasetselection">
        <!-- anchor -->
        <a id="dataset_selection" name="dataset_selection" href="#dataset_selection"></a>
     <fieldset>
     <legend>Dataset Selection</legend>
 
    <div class="form-horizontal">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ExpName" CssClass="col-lg-3 control-label">Experiment Name</asp:Label>
                <div class="col-lg-9">
                    <asp:TextBox runat="server" ID="ExpName" placeholder="Set the experiment name (optionally)" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="FileExtensionList1" CssClass="col-lg-3 control-label">File Type*</asp:Label>
                <div class="col-lg-9">
                    <asp:DropDownList ID="FileExtensionList1" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="0" Text="Select File Type"></asp:ListItem>
                        <asp:ListItem Value=".txt" Text="Text Files (.txt)" ></asp:ListItem>
                        <asp:ListItem Value=".csv" Text="Comma Separated Values (.csv)" ></asp:ListItem>
                        <asp:ListItem Value=".xls" Text="Excel Files - 2003 (.xls)" ></asp:ListItem>
                        <asp:ListItem Value=".xlsx" Text="Excel Files - 2007 (.xlsx)" ></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="FileUpload1" CssClass="col-lg-3 control-label">Browse Files*</asp:Label>
                <div class="col-lg-8">
                        <asp:FileUpload ID="FileUpload1" runat="server" Multiple="Multiple" CssClass="form-control" data-buttonName="btn-primary" data-buttonText="Choose files" data-buttonBefore="true"></asp:FileUpload>
                    <span class="help-block">Tip: You can upload multiple files by holding down the "Ctrl" button pressed. Sample data can be found <a href="<%= Page.ResolveUrl("~")%>sample_files.zip">here</a>.</span>    
                </div>
            </div>
            <div class="form-group">
                <div class="col-lg-9 col-lg-offset-3">
                    <asp:Button runat="server" ID="ResetBtn" Text="Reset All" CssClass="btn btn-danger" OnClientClick="javascript:return DoActions('Reset');"></asp:Button> 
                    <asp:Button runat="server" ID="UploadBtn" Text="Upload" CssClass="btn btn-info" OnClientClick="javascript:return DoActions('UploadFiles');"></asp:Button>
                </div>
            </div>
        </div>

        <div id="FileNametbl" Class="col-md-6 text-center" <%=sFileNametbl%>>                
            <asp:Panel ID="pnlList" runat="server" ScrollBars="Vertical" Height="300px">
                  <asp:GridView ID="grdFiles" runat="server" OnRowDataBound="grdFiles_RowDataBound" AutoGenerateColumns="False" GridLines="None"  
                      CssClass="mGrid"  SelectedRowStyle-BackColor="#E5E5E5" DataKeyNames="RF_ID"> 
                      <Columns>
                          <asp:CommandField ShowSelectButton="True" ItemStyle-Width="80px" />                              
                          <asp:BoundField DataField="RF_ID" HeaderText="ID" ItemStyle-Width="50px" Visible="false" /> 
                          <asp:BoundField DataField="RF_FileName" HeaderText="Filename" ItemStyle-Width="180px" ItemStyle-Wrap="true"  /> 
                          <asp:BoundField DataField="RD_RowCounter" HeaderText="Number of Rows" ItemStyle-Width="120px"  />
                          <asp:BoundField DataField="RF_IsEnabled" HeaderText="Status" ItemStyle-Width="100px" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Small" />
                      </Columns>
                      <SelectedRowStyle BackColor="#E5E5E5" CssClass="RowSelected" />
                      <AlternatingRowStyle CssClass="alt" />
                  </asp:GridView>  
                 
            </asp:Panel>
            <asp:Button runat="server" id="RemoveAllbtn" Text="Remove All" CssClass="btn btn-default btn-xs waves-effect waves-light" OnClick="RemoveAllbtn_Click" />
            <asp:Button runat="server" id="RestoreAllbtn" Text="Restore All" CssClass="btn btn-default btn-xs waves-effect waves-light" OnClick="RestoreAllbtn_Click" />
            <span class="help-block">Tip: You can easily handle your uploaded files. Just click on Remove/Restore buttons to disable or re-enable any datafile from the current dataset. All the following graphs will be dynamically redesigned.</span>
        </div>
    </div>

    </fieldset>
    </section>


    <section id="RawDataVisualization"  <%=sRawDataVisualization%>>
    <fieldset>
    <legend>Raw Data Visualization</legend>
    <div class="form-horizontal">
    <div class="col-md-12">
    <span class="help-block">Tip: You can zoom-in either vertically or horizontally in any graph by clicking & dragging your mouse at the same time. Also, you can move (pan) the graph by pressing the 'Shift' button of your keyboard and then by clicking and dragging your mouse. To restore the graph in its initial state just double click wherever on it. Furthermore, you can click on any datapoint to get useful information. Finally, you are able to disable labeling by switching the following control-element to the left.</span>
    </div>
    </div>
   
<div class="form-horizontal">
<div class="col-md-12">
                <ul class="list-group">
                    <li class="list-group-item">
                        <span class="label label-primary" id="firstgroupofgraphs">Labeling is enabled</span>
                        <div class="material-switch pull-right">
                            <input id="chcboxshowhidelabels" name="chcboxshowhidelabels" type="checkbox" onclick="ShowHideLabelsRawData();" checked="checked"/>
                            <label for="chcboxshowhidelabels" class="label-primary"></label>
                        </div>
                    </li>
                </ul>
</div></div>

    <div class="form-horizontal">
    <%--First Line Chart--%>
    <div class="col-md-10">
    <div class="panel panel-default">
    <div class="panel-body">
    <div class="graphdiv" id="graphdiv" style="width:100%; margin: 0 auto;" ></div>
    <div class="col-md-10 col-md-offset-2">
    <div id="hiddenroi1" style="display: none;"><%= sDataForROI1Graph %></div>
    <div id="hiddenlabels1" style="display: none;"><%= sLabelsForROI1Graph %></div>
    <image id="imgid1" style="display: none;" />
    </div>
    </div>
    </div>
    </div>
    <div class="col-md-2"><div class="panel panel-default" style="max-height: 353px; overflow:hidden;"><div class="panel-body"><legend style="width: 100%; text-align: center;">Labels</legend><div class="labelsdiv" id="labelsdiv1"></div></div></div></div>
    </div>
    <%--End of First Line Chart--%>

    <%--Second Line Chart--%>
    <div class="form-horizontal">
    <div class="col-md-10">
    <div class="panel panel-default">
    <div class="panel-body">
    <div class="graphdiv2" id="graphdiv2" style="width:100%; margin: 0 auto;"></div>
    <div class="col-md-10 col-md-offset-2">
    <div id="hiddenroi2" style="display: none;"><%= sDataForROI2Graph %></div>
    <div id="hiddenlabels2" style="display: none;"><%= sLabelsForROI2Graph %></div>
    <image id="imgid2" style="display: none;" />
    </div>
    </div>
    </div>
    </div>
    <div class="col-md-2"><div class="panel panel-default" style="max-height: 353px; overflow:hidden;"><div class="panel-body"><legend style="width: 100%; text-align: center;">Labels</legend><div class="labelsdiv" id="labelsdiv2"></div></div></div></div>
    <%--End of Second Line Chart--%>
    </div>

    <%--Third Line Chart--%>
    <div class="form-horizontal">
    <%--<div class="col-md-6 col-md-offset-3">--%>
    <div class="col-md-10">
    <div class="panel panel-default">
    <div class="panel-body">
    <div class="graphdiv3" id="graphdiv3" style="width:100%; margin: 0 auto;"></div>
    <div class="col-md-10 col-md-offset-2">
    <div id="hiddenroi3" style="display: none;"><%= sDataForROI3Graph %></div>
    <div id="hiddenlabels3" style="display: none;"><%= sLabelsForROI3Graph %></div>
    <image id="imgid3" style="display: none;" />
    </div>
    </div>
    </div>
    </div>
    <div class="col-md-2">
    <div class="panel panel-default" style="max-height: 353px; overflow:hidden;">
    <div class="panel-body">
    <legend style="width: 100%; text-align: center;">Labels</legend>
    <div class="labelsdiv" id="labelsdiv3">
    </div>
    </div>
    </div>
    </div>
    </div>
    <%--End of Third Line Chart--%>

    <div class="form-horizontal">
    <div class="col-md-11 col-md-offset-1">
        <asp:Button runat="server" id="printDygraphROIbtn" Text="Save Graphs" CssClass="btn btn-info waves-effect waves-light" OnClientClick="javascript:return printDygraph('ROI');" />
        <asp:Button runat="server" id="ExportRoiDatabtn" Text="Export ROI Data" CssClass="btn btn-success waves-effect waves-light" OnClientClick="javascript:return DoActions('ExportRoiDatabtn_Click');" OnClick="ExportRoiDatabtn_Click" />
    </div></div>


   </fieldset> 
   </section> 

    <script>
                
        function designRawGraphs()
        {
           
            var hiddendataforroi1 = document.getElementById("hiddenroi1").innerHTML;
            var hiddenlabelsforroi1 = document.getElementById("hiddenlabels1").innerHTML;
            var hiddendataforroi2 = document.getElementById("hiddenroi2").innerHTML;
            var hiddenlabelsforroi2 = document.getElementById("hiddenlabels2").innerHTML;
            var hiddendataforroi3 = document.getElementById("hiddenroi3").innerHTML;
            var hiddenlabelsforroi3 = document.getElementById("hiddenlabels3").innerHTML;

            //console.log(hiddendataforroi1.length);

            if (hiddendataforroi1.length>5) {
                var objforroi1 = JSON.parse('' + hiddendataforroi1 + '');
                var objlabelsforroi1 = JSON.parse('' + hiddenlabelsforroi1 + '');
                var objforroi2 = JSON.parse('' + hiddendataforroi2 + '');
                var objlabelsforroi2 = JSON.parse('' + hiddenlabelsforroi2 + '');
                var objforroi3 = JSON.parse('' + hiddendataforroi3 + '');
                var objlabelsforroi3 = JSON.parse('' + hiddenlabelsforroi3 + '');
            }
            else {
                var objforroi1 = [[0]];
                var objlabelsforroi1 = [''];
                var objforroi2 = [[0]];
                var objlabelsforroi2 = [''];
                var objforroi3 = [[0]];
                var objlabelsforroi3 = [''];
            }

           

           g1 = new Dygraph(
                     document.getElementById("graphdiv"),
                       objforroi1, 
                { 
                    labels: objlabelsforroi1,
                    legend: 'onmouseover',
                    labelsDiv: 'labelsdiv1',
                    labelsSeparateLines: true,
                    //showLabelsOnHighlight: true,
                    connectSeparatedPoints: true,
                    animatedZooms: true,
                    highlightCircleSize: 3,
                    highlightSeriesOpts: {
                        highlightCircleSize: 6
                    },
                    pointClickCallback: function(e, pt) {
                        ShowInfoMessage(' File Name: ' + JSON.stringify(pt.name) + '</br>' + ' Time: ' + JSON.stringify(pt.xval.toFixed(2)) + '</br>' + ' Fluorescence Intensity: ' + JSON.stringify(pt.yval.toFixed(2)));
                    },
                    drawPoints: true,
                    title: 'ROI 1 - Region of Interest',
                    ylabel: 'Raw Fluorescence Intensity',
                    xlabel: 'Time (seconds)' 
                }
                            );

                //disable right click for g1
                $('.graphdiv').bind('contextmenu', function(e) {
                return false;
                });


                g2 = new Dygraph(
                document.getElementById("graphdiv2"),
                        objforroi2 ,
                    { 
                        labels: objlabelsforroi2,
                        legend: 'onmouseover',
                        labelsDiv: 'labelsdiv2',
                        labelsSeparateLines: true,
                        connectSeparatedPoints: true,
                        animatedZooms: true,
                        highlightCircleSize: 3,
                        highlightSeriesOpts: {
                            highlightCircleSize: 6
                        },
                        pointClickCallback: function (e, pt) {
                            ShowInfoMessage(' File Name: ' + JSON.stringify(pt.name) + '</br>' + ' Time: ' + JSON.stringify(pt.xval.toFixed(2)) + '</br>' + ' Fluorescence Intensity: ' + JSON.stringify(pt.yval.toFixed(2)));
                        },
                        drawPoints: true,
                        title: 'ROI 2 - Whole Cell Area',
                        ylabel: 'Raw Fluorescence Intensity',
                        xlabel: 'Time (seconds)' 
                    }
                );
        
            //disable right click for g2
            $('.graphdiv2').bind('contextmenu', function(e) {
                return false;
            }); 


                g3 = new Dygraph(
                document.getElementById("graphdiv3"),
                    objforroi3 ,
                {

                    labels: objlabelsforroi3,
                    legend: 'onmouseover',
                    labelsDiv: 'labelsdiv3',
                    labelsSeparateLines: true,
                    connectSeparatedPoints: true,
                    animatedZooms: true,
                    highlightCircleSize: 3,
                    highlightSeriesOpts: {
                        highlightCircleSize: 6
                    },
                    pointClickCallback: function (e, pt) {
                        ShowInfoMessage(' File Name: ' + JSON.stringify(pt.name) + '</br>' + ' Time: ' + JSON.stringify(pt.xval.toFixed(2)) + '</br>' + ' Fluorescence Intensity: ' + JSON.stringify(pt.yval.toFixed(2)));
                    },
                    drawPoints: true,
                    title: 'ROI 3 - Background',
                    ylabel: 'Raw Fluorescence Intensity',
                    xlabel: 'Time (seconds)' 
                }
            );
        
            //disable right click for g3
            $('.graphdiv3').bind('contextmenu', function(e) {
                return false;
            });

        
            //Synchronizer of g1 and g2 and g3
            var sync = Dygraph.synchronize( [g1, g2, g3], {
                selection: true,
                zoom: false
            });
        
        }


        
    </script>



    <section id="BleachingParameters"  <%=sBleachingParameters%>>
     <!-- anchor -->
     <a id="bd_gr_position" name="bd_gr_position" href="#bd_gr_position"></a>
    <fieldset>
    <legend>Bleaching Depth - Gap Ratio</legend>
    <div class="form-horizontal">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PreBleachValuestxtbox" CssClass="col-lg-3 control-label">Pre-Bleach Values*</asp:Label>
                <div class="col-lg-9">
                    <asp:TextBox runat="server" ID="PreBleachValuestxtbox" placeholder="Set the number of frames before bleach" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Label runat="server" AssociatedControlID="BleachValuestxtbox" CssClass="col-lg-3 control-label">Bleach Values*</asp:Label>
                <div class="col-lg-9">
                    <asp:TextBox runat="server" ID="BleachValuestxtbox" placeholder="Set the number of frames during bleach" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Label runat="server" AssociatedControlID="PostBleachValuestxtbox" CssClass="col-lg-3 control-label">Post-Bleach Values*</asp:Label>
                <div class="col-lg-9">
                    <asp:TextBox runat="server" ID="PostBleachValuestxtbox" placeholder="Set the number of frames after bleach" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Label runat="server" AssociatedControlID="DiscardValuestxtbox" CssClass="col-lg-3 control-label">Initial Values to Discard</asp:Label>
                <div class="col-lg-9">
                    <asp:TextBox runat="server" ID="DiscardValuestxtbox" placeholder="Proposed value -10- (optionally)" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">

            <asp:Table ID="BD_GP_Table" runat="server" CssClass="mGrid">
                <asp:TableHeaderRow ID="BD_GP_TableHeader" runat="server">
                        <asp:TableHeaderCell CssClass="text-center">Bleaching Depth</asp:TableHeaderCell>
                        <asp:TableHeaderCell CssClass="text-center">Gap Ratio</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow ID="BD_GP_TableRow" runat="server">
                        <asp:TableCell CssClass="text-center" ID="sBleachingDepthId" Text="N/A" Font-Bold="true" ForeColor="Red" Font-Size="Medium"><%=sBleachingDepthScore%></asp:TableCell>
                        <asp:TableCell CssClass="text-center" ID="sGapRatioId" Text="N/A" Font-Bold="true" ForeColor="Red" Font-Size="Medium"><%=sGapRatioScore%></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div class="form-group">
                <div class="col-lg-12">
                    <span class="help-block">Tip: You can remove/restore files of your dataset at any time. If you have estimated both Bleaching Depth and Gap Ratio, the specific values will be automatically recalculated.</span>
                </div>
            </div>
            </div>
            <div class="form-group">
                <div class="col-lg-9 col-lg-offset-3">
                    <asp:Button runat="server" ID="BleachingParametersReset" Text="Reset" CssClass="btn btn-danger" OnClientClick="javascript:return DoActions('BleachingParametersReset');"></asp:Button> 
                    <asp:Button runat="server" ID="BleachingParametersCompute" Text="Compute" CssClass="btn btn-info" OnClick="BleachingParametersCompute_Click" OnClientClick="javascript:return DoActions('BleachingParametersCompute');"></asp:Button>
                </div>
            </div>
        </div>
    </div>
    </fieldset>
    </section>


    <section id="Normalization"  <%=sNormalization%>>
         <!-- anchor -->
        <a id="normalization" name="normalization" href="#normalization"></a>
    <fieldset>
    <legend>Normalization</legend>
    <div class="form-horizontal">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="NormalizationRadioButtonList" CssClass="col-lg-3 control-label">Select Method*</asp:Label>
                <div class="col-lg-9">
                    <asp:RadioButtonList runat="server" ID="NormalizationRadioButtonList" CssClass="radio" RepeatDirection="Vertical" RepeatLayout="Table">
                        <asp:ListItem Text="Double" Value="Double"></asp:ListItem>
                        <asp:ListItem Text="Full Scale" Value="FullScale"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <div class="col-lg-9 col-lg-offset-3">
                    <asp:Button runat="server" ID="ClearNormalizebtn" Text="Clear" CssClass="btn btn-danger" OnClientClick="javascript:return DoActions('ClearNormalize');"></asp:Button> 
                    <asp:Button runat="server" ID="Normalizebtn" Text="Normalize" CssClass="btn btn-info" OnClick="Normalizebtn_Click" OnClientClick="javascript:return DoActions('Normalize');"></asp:Button> 
                </div>
            </div>
        </div>
    </div>

<div class="form-horizontal">
<div class="col-md-12">
                <ul class="list-group">
                    <li class="list-group-item">
                        <span class="label label-primary" id="secondgroupofgraphs">Labeling is enabled</span>
                        <div class="material-switch pull-right">
                            <input id="chcboxshowhidelabelsnormdata" name="chcboxshowhidelabelsnormdata" type="checkbox" onclick="ShowHideLabelsNormData();" checked="checked"/>
                            <label for="chcboxshowhidelabelsnormdata" class="label-primary"></label>
                        </div>
                    </li>
                </ul>
</div></div>

    <%--Fourth Line Chart--%>
    <div class="form-horizontal">
    <div class="col-md-10">
    <div class="panel panel-default">
    <div class="panel-body">
    <div class="graphdiv4" id="graphdiv4" style="width:100%; margin: 0 auto;"></div>
    <image id="imgid4" style="display: none;" />
    <div class="col-md-10 col-md-offset-2">
        <div id="hiddennorm" style="display: none;"><%= sDataForNormalizationGraph %></div>
        <div id="hiddennormlabels" style="display: none;"><%= sLabelsForNormalizationGraph %></div>
    </div>
    </div>
    </div>
    
   
    </div>
    <div class="col-md-2"><div class="panel panel-default" style="max-height: 353px; overflow:hidden;"><div class="panel-body"><legend style="width: 100%; text-align: center;">Labels</legend><div class="labelsdiv" id="labelsnormdiv"></div></div></div></div>
    </div>
    <%--End of Fourth Line Chart--%>

    <%--Fifth Line Chart--%>
    <div class="form-horizontal">
    <div class="col-md-10">
    <div class="panel panel-default">
    <div class="panel-body">
    <div class="graphdiv5" id="graphdiv5" style="width:100%; margin: 0 auto;"></div>
    <image id="imgid5" style="display: none;" />
    <div class="col-md-10 col-md-offset-2">
        <div id="hiddenstd" style="display: none;"><%= sDataForStandardDevNormalizationGraph %></div>
    </div>
    </div>
    </div>
    
    </div>
   </div>
    <%--End of Fifth Line Chart--%>

    <div class="form-horizontal">
    <div class="col-md-11 col-md-offset-1">
        <asp:Button runat="server" id="printDygraphbtn4" Text="Save Graphs" CssClass="btn btn-info waves-effect waves-light" OnClientClick="javascript:return printDygraph('Normalization');" />
        <asp:Button runat="server" id="ExportNormDatabtn" Text="Export Normalized Data" CssClass="btn btn-success waves-effect waves-light" OnClientClick="javascript:return DoActions('ExportNormData');" OnClick="ExportNormDatabtn_Click" />
    </div></div>

    </fieldset>
    </section>

        <script type="text/javascript">

            function designNormGraphs() {

                var hiddendatafornorm = document.getElementById("hiddennorm").innerHTML;
                var hiddenlabelsfornorm = document.getElementById("hiddennormlabels").innerHTML;
                var hiddendataforstd = document.getElementById("hiddenstd").innerHTML;

                if (hiddendatafornorm.length > 5) {
                    var objfornorm = JSON.parse('' + hiddendatafornorm + '');
                    var objlabelsfornorm = JSON.parse('' + hiddenlabelsfornorm + '');
                    var objforstd = JSON.parse('' + hiddendataforstd + '');
                    var objlabelsforstd = ['', 'Mean Normalized Value'];
                }
                else {
                    var objfornorm = [[0]];
                    var objlabelsfornorm = [''];
                    var objforstd = [[0]];
                    var objlabelsforstd = [''];
                }


                g4 = new Dygraph(
                document.getElementById("graphdiv4"),
                    objfornorm,
            { 
                labels: objlabelsfornorm,
                valueRange: [0, 1.2],
                legend: 'onmouseover',
                labelsDiv: 'labelsnormdiv',
                labelsSeparateLines: true,
                connectSeparatedPoints: true,
                animatedZooms: true,
                highlightCircleSize: 3,
                highlightSeriesOpts: {
                    highlightCircleSize: 6
                },
                pointClickCallback: function (e, pt) {
                    ShowInfoMessage(' File Name: ' + JSON.stringify(pt.name) + '</br>' + ' Time: ' + JSON.stringify(pt.xval.toFixed(2)) + '</br>' + ' Fluorescence Intensity: ' + JSON.stringify(pt.yval.toFixed(2)));
                },
                drawPoints: true,
                title: 'Normalized Data',
                ylabel: 'Normalized Fluorescence Intensity',
                xlabel: 'Time (seconds)' 
            }
        );

                //disable right click for g4
                $('.graphdiv4').bind('contextmenu', function (e) {
                    return false;
                });


                g5 = new Dygraph(
                document.getElementById("graphdiv5"),
                    objforstd,
            { 
                
                //labels: [ "" , "Mean Normalized Value"],
                labels: objlabelsforstd,
                valueRange: [0, 1.2],
                legend: 'onmouseover',
                customBars: true,
                connectSeparatedPoints: true,
                animatedZooms: true,
                highlightCircleSize: 3,
                customBars: true,
                highlightSeriesOpts: {
                    highlightCircleSize: 6
                },
                pointClickCallback: function (e, pt) {
                    ShowInfoMessage(' Time: ' + JSON.stringify(pt.xval.toFixed(2)) + '</br>' + ' Fluorescence Intensity: ' + JSON.stringify(pt.yval.toFixed(2)) + '</br>' + ' Fluorescence Intensity (max): ' + JSON.stringify(pt.yval_plus.toFixed(2)) + '</br>' + ' Fluorescence Intensity (min) : ' + JSON.stringify(pt.yval_minus.toFixed(2)));
                },
                drawPoints: true,
                title: 'Mean Normalized +/- Standard Deviation',
                ylabel: 'Normalized Fluorescence Intensity',
                xlabel: 'Time (seconds)' 
            }
        );

                //disable right click for g5
                $('.graphdiv5').bind('contextmenu', function (e) {
                    return false;
                });


                //Synchronizer of g4 and g5
                var sync = Dygraph.synchronize([g4, g5], {
                    selection: true,
                    zoom: true
                });

            }
        </script>

    <section id="CurveFitting"  <%=sCurveFitting%>>
         <!-- anchor -->
        <a id="curvefitting" name="curvefitting" href="#curvefitting"></a>
    <fieldset>
    <legend>Curve Fitting</legend>
    <div class="form-horizontal">
        <div class="col-md-12"><span class="help-block">Tip: Initially, declare the exponential equation from the left panel. After that, select a sample of interest and perform curve fitting in order to extract quantitative information from the curves. By pressing the "Fit Mean Data" button the same quantitative information will be retrieved for the mean of the active curves.</span></div>
        <div class="col-md-12">
        <div class="col-md-6">
            <div class="form-group">
            <div class="panel panel-default">
                <div class="panel-body" style="height: 350px;">
                 
                 <div class="col-md-12">   
                <div class="panel panel-default">
                <div class="panel-body">
                    <asp:Label runat="server" AssociatedControlID="CurveFittingRadioButtonList" CssClass="col-md-2 control-label">Select Equation*</asp:Label>
                <div class="col-md-4">
                    <asp:RadioButtonList runat="server" ID="CurveFittingRadioButtonList" CssClass="radio" RepeatDirection="Vertical" RepeatLayout="Table">
                        <asp:ListItem Text="Single" Value="Single"></asp:ListItem>
                        <asp:ListItem Text="Double" Value="Double"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-6 text-center">
                    <asp:Button runat="server" ID="ClearCurvebtn" Text="Clear" CssClass="btn btn-danger" OnClientClick="javascript:return DoActions('ClearCurve');"></asp:Button> 
                </div>
                </div>
                </div>
                </div>

                    <div class="col-md-12">   
            <div class="panel panel-default">
            <div class="panel-body">                 
            <asp:Table ID="Table1" runat="server" CssClass="mGrid">
                <asp:TableHeaderRow ID="TableHeaderRow1" runat="server">
                        <asp:TableHeaderCell CssClass="col-md-4 text-center">Mobile Fraction</asp:TableHeaderCell>
                        <asp:TableHeaderCell CssClass="col-md-4 text-center">T-half</asp:TableHeaderCell>
                        <asp:TableHeaderCell CssClass="col-md-4 text-center">R Square</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow ID="TableRow1" runat="server">
                        <asp:TableCell CssClass="text-center" ID="sMobFractionId" Text="N/A" Font-Bold="true" ForeColor="Red" Font-Size="Medium"><%=sMobileFractionScore%></asp:TableCell>
                        <asp:TableCell CssClass="text-center" ID="sThalfId" Text="N/A" Font-Bold="true" ForeColor="Red" Font-Size="Medium"><%=sThalfScore%></asp:TableCell>
                        <asp:TableCell CssClass="text-center" ID="sRsquareId" Text="N/A" Font-Bold="true" ForeColor="Red" Font-Size="Medium"><%=sRsquareScore%></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            </div>
           </div>
                    </div>


                <div class="col-md-12"> 
                <div class="panel panel-default">
                <div class="panel-body">
                <div class="col-md-6  text-center">
                    <asp:Button runat="server" ID="FitAllbtn" Text="Fit Mean Data" CssClass="btn btn-info" OnClientClick="javascript:return DoActions('FitCurve');" OnClick="FitAllCurve_Click"></asp:Button>
                </div>
                <div class="col-md-6 text-center">
                    <asp:Button runat="server" id="SaveResultsbtn" Text="Save Results" CssClass="btn btn-success waves-effect waves-light" OnClientClick="javascript:return DoActions('ExportFinalData');" />
                </div>
               </div>
               </div>
               </div>              

             </div>
            </div>
          </div>
        </div>


        <div class="col-md-6">
        <div class="form-group">
        <div id="CurveFittingtbl" Class="col-md-12 text-center">                
            <asp:Panel ID="pnlList2" runat="server" ScrollBars="Vertical" Height="350px">
                  <asp:GridView ID="grdFitCurve" runat="server" OnRowDataBound="grdFitCurve_RowDataBound" AutoGenerateColumns="False" GridLines="None" CssClass="mGrid"  SelectedRowStyle-BackColor="#E5E5E5" DataKeyNames="RF_ID"> 
                      <Columns>
                          <asp:CommandField ShowSelectButton="True" ItemStyle-Width="35%" ItemStyle-Wrap="false" SelectText="Perform Curve Fitting" />                              
                          <asp:BoundField DataField="RF_ID" HeaderText="ID"  Visible="false" /> 
                          <asp:BoundField DataField="RF_FileName" HeaderText="Filename" ItemStyle-Wrap="true"  /> 
                      </Columns>
                      <SelectedRowStyle BackColor="#E5E5E5" CssClass="RowSelected" />
                      <AlternatingRowStyle CssClass="alt" />
                  </asp:GridView>  
                 
            </asp:Panel>
        </div>
        </div>
        </div>
      </div>
    </div>

<div class="form-horizontal">
<div class="col-md-12">
      <ul class="list-group">
          <li class="list-group-item">
              <span class="label label-primary" id="thirdgroupofgraphs">Labeling is enabled</span>
                    <div class="material-switch pull-right">
                    <input id="chcboxshowhidelabelscurvefitdata" name="chcboxshowhidelabelscurvefitdata" type="checkbox" onclick="ShowHideLabelsCurveFitData();" checked="checked"/>
                    <label for="chcboxshowhidelabelscurvefitdata" class="label-primary"></label>
              </div>
           </li>
      </ul>
</div></div>

    <%--Sixth Line Chart--%>
    <div class="form-horizontal">
    <div class="col-md-10">
    <div class="panel panel-default">
    <div class="panel-body">
    <div class="graphdiv6" id="graphdiv6" style="width:100%; margin: 0 auto;"></div>
    <image id="imgid6" style="display: none;" />
    <div class="col-md-10 col-md-offset-2">
        <div id="hiddencurvedots" style="display: none;"><%= sDataForCurveFittingDots %></div>
        <div id="hiddenlabelsforcurvefittingdots" style="display: none;"><%= sLabelsForCurveFittingDots %></div>
    </div>
    </div>

    



    </div></div>
    <div class="col-md-2"><div class="panel panel-default" style="max-height: 353px; overflow:hidden;"><div class="panel-body"><legend style="width: 100%; text-align: center;">Labels</legend><div class="labelsdiv" id="labelscurvediv"></div></div></div></div>
    </div>
    <%--End of Sixth Line Chart--%>

    <%--Seventh Line Chart--%>
    <div class="form-horizontal">
        <div class="col-md-10">
        <div class="panel panel-default">
        <div class="panel-body">
        <div class="graphdiv7" id="graphdiv7" style="width:100%; margin: 0 auto;"></div>
        <image id="imgid7" style="display: none;" />
        <div class="col-md-10 col-md-offset-2">
            <div id="hiddenresiduals" style="display: none;"><%= sDataForResiduals %></div>
            <div id="hiddenlabelsforresiduals" style="display: none;"><%= sLabelsForResiduals %></div>
        </div>
        </div>
        </div>
        </div>
    </div>

    <div class="form-horizontal">
    <div class="col-md-11 col-md-offset-1">
        <asp:Button runat="server" id="printDygraphbtn7" Text="Save Graphs" CssClass="btn btn-info waves-effect waves-light" OnClientClick="javascript:return printDygraph('Curve_Fitting');" />
    </div></div>

    <%--End of Seventh Line Chart--%>

       <script type="text/javascript">

       function designCurveFittingGraphs() {

           var hiddendataforcurve = document.getElementById("hiddencurvedots").innerHTML;
           var hiddenlabelsforcurve = document.getElementById("hiddenlabelsforcurvefittingdots").innerHTML;

           var hiddendataforresiduals = document.getElementById("hiddenresiduals").innerHTML;
           var hiddenlabelsforresiduals = document.getElementById("hiddenlabelsforresiduals").innerHTML;


                if (hiddendataforcurve.length > 5) {
                    var objforcurve = JSON.parse('' + hiddendataforcurve + '');
                    var objforlabelscurve = JSON.parse('' + hiddenlabelsforcurve + '');

                    var objforresiduals = JSON.parse('' + hiddendataforresiduals + '');
                    var objforlabelsresiduals = JSON.parse('' + hiddenlabelsforresiduals + '');
                }
                else {
                    var objforcurve = [[0]];
                    var objforlabelscurve = [''];

                    var objforresiduals = [[0]];
                    var objforlabelsresiduals = [''];
                }


               
                g6 = new Dygraph(
                document.getElementById("graphdiv6"),
                    objforcurve,
            { 
                labels: objforlabelscurve,
                valueRange: [0, 1.2],
                legend: 'onmouseover',
                labelsDiv: 'labelscurvediv',
                labelsSeparateLines: true,
                drawPoints: true,
                //strokeWidth: 0,
                connectSeparatedPoints: true,
                animatedZooms: true,
                highlightCircleSize: 3,
                highlightSeriesOpts: {
                    highlightCircleSize: 6
                },
                pointClickCallback: function (e, pt) {
                    ShowInfoMessage(' Filename: ' + JSON.stringify(pt.name) + '</br>' + ' Time: ' + JSON.stringify(pt.xval.toFixed(2)) + '</br>' + ' Fluorescence Intensity: ' + JSON.stringify(pt.yval.toFixed(2)));
                },
                title: 'Curve Fit Results',
                ylabel: 'Relative Fluorence Intensity',
                xlabel: 'Time (seconds)',
            }
            );

                //disable right click for g6
                $('.graphdiv6').bind('contextmenu', function (e) {
                    return false;
                });

            g7 = new Dygraph(
                document.getElementById("graphdiv7"),
                objforresiduals,
            {
                labels: objforlabelsresiduals,
                legend: 'onmouseover',
                drawPoints: true,
                //strokeWidth: 0,
                connectSeparatedPoints: true,
                // valueRange: [-1, 1],
                animatedZooms: true,
                highlightCircleSize: 3,
                highlightSeriesOpts: {
                    highlightCircleSize: 6
                },
                pointClickCallback: function (e, pt) {
                    ShowInfoMessage(' Filename: ' + JSON.stringify(pt.name) + '</br>' + ' Time: ' + JSON.stringify(pt.xval.toFixed(2)) + '</br>' + ' Deviation: ' + JSON.stringify(pt.yval.toFixed(2)));
                },
                title: 'Residuals',
                ylabel: 'Deviation',
                xlabel: 'Time (seconds)'
                }
          );

                $('.graphdiv7').bind('contextmenu', function (e) {
                    return false;
                });

                //Synchronizer of g6 and g7
                var sync = Dygraph.synchronize([g6, g7], {
                    selection: true,
                    zoom: false
                });

       }
    </script>


    </fieldset>
    </section>


    <section id="DeleteSession"  <%=sDeleteSessionButton%>> 
    <fieldset>
    <legend>Remove Data</legend>
    <div class="form-horizontal">
    <div class="col-md-12">
    <div class="panel panel-default">
    <div class="panel-body">
        <div class="col-md-12 text-center">
              <asp:Button runat="server" ID="DeleteSessionbtn" Text="Delete the entire Dataset" title="" CssClass="btn btn-danger" OnClientClick="javascript:return DoActions('DeleteSession');"></asp:Button> 
        </div>
    </div>
    </div>
    </div>
    </div>
    </fieldset>
    </section>

        <script type="text/javascript">

        function DoActions(s) {

            if (s === 'Reset') {
                swal({ title: "Do you want to proceed?", text: "This action will erase all your saved data! You will not be able to undo this action! Are you sure?", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes", cancelButtonText: "No", closeOnConfirm: true, closeOnCancel: true }, function () { ShowLoadingModal(); __doPostBack(document.forms[0].name, s); });
                return false;
            }
            else if (s === 'UploadFiles') {
                var mySession = '<%= Session("Guid") %>';
                if (mySession === '')
                {
                    var filextension = document.getElementById('<%=FileExtensionList1.ClientID%>').value;
                    var fileupload = document.getElementById('<%=FileUpload1.ClientID%>').value;
                    if (filextension == '0'){
                        ShowWarningMessage('Please declare the type of your files.');
                        return false;
                    }
                    else if (fileupload == ''){
                        ShowWarningMessage('Please upload at least one file.');
                        return false;
                    }
                    else{
                        ShowFileInputInformationScreen(s);
                        return false; 
                    }
                }
                else
                {
                    swal({   title: "Do you want to proceed?",   text: "By uploading new files, the current dataset will be deleted!",   type: "warning",   showCancelButton: true,  confirmButtonColor: "#DD6B55",   confirmButtonText: "Yes",   cancelButtonText: "No",   closeOnConfirm: true,   closeOnCancel: true }, function(){    ShowFileInputInformationScreen(s); return false;  });
                    return false;       
                }   
            }
            else if (s === 'BleachingParametersReset'){
                swal({ title: "Do you want to proceed?", text: "This action will reset the Bleaching Depth and the Gap Ratio values!", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes", cancelButtonText: "No", closeOnConfirm: true, closeOnCancel: true }, function () { ShowLoadingModal(); __doPostBack(document.forms[0].name, s); });
                return false;
            }
            else if (s === 'Invoke_Click'){
                return true;
            }
            else if (s === 'BleachingParametersCompute'){
                var prebleachvaluestxtbox = document.getElementById('<%=PreBleachValuestxtbox.ClientID%>').value;
                var bleachvaluestxtbox = document.getElementById('<%=BleachValuestxtbox.ClientID%>').value;
                var postbleachvaluestxtbox = document.getElementById('<%=PostBleachValuestxtbox.ClientID%>').value;
                var initialvaluestxtbox = document.getElementById('<%=DiscardValuestxtbox.ClientID%>').value;
                var data1; var data2; var data3;

                if ((prebleachvaluestxtbox == '') || (bleachvaluestxtbox == '') || (postbleachvaluestxtbox == '')){
                    ShowWarningMessage('Please enter all the mandatory parameters. Pre-Bleach, Bleach and Post-Bleach values should not be empty in order to compute the Bleaching Depth and Gap Ratio.');
                    return false;
                }
                else if (isNaN(prebleachvaluestxtbox) || isNaN(bleachvaluestxtbox) || isNaN(postbleachvaluestxtbox)) {
                    ShowWarningMessage('Only numerical values are accepted. Please correct the values.');
                    return false;
                }
                else if (prebleachvaluestxtbox % 1 !== 0) {
                    ShowWarningMessage('Only integer values are accepted. Prebleach value seems to be incorrect. Please correct your entries.');
                    return false;
                }
                else if (bleachvaluestxtbox % 1 !== 0) {
                    ShowWarningMessage('Only integer values are accepted. Bleach value seems to be incorrect. Please correct your entries.');
                    return false;
                }
                else if (postbleachvaluestxtbox % 1 !== 0) {
                    ShowWarningMessage('Only integer values are accepted. Postbleach value seems to be incorrect. Please correct your entries.');
                    return false;
                }
                else if (initialvaluestxtbox % 1 !== 0) {
                    ShowWarningMessage('Only integer values are accepted. Initial value to discard seems to be incorrect. Please correct your entries.');
                    return false;
                }
                else if ((parseInt(initialvaluestxtbox,10)) >= (parseInt(prebleachvaluestxtbox,10))) {
                    ShowWarningMessage('The number of Pre-bleach values should be greater than the number of initial values to discard. Please correct your entries.');
                    return false;
                }
                else{
                    return true;
                }   
            }
            else if (s === 'ClearNormalize'){
                swal({ title: "Do you want to proceed?", text: "This action will reset everything in the Normalization section!", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes", cancelButtonText: "No", closeOnConfirm: true, closeOnCancel: true }, function () { ShowLoadingModal(); __doPostBack(document.forms[0].name, s); });
                return false;
            }
            else if (s === 'DeleteSession') {
                swal({ title: "Do you want to proceed?", text: "This action will delete the entire dataset! You will not be able to undo this action! Are you sure?", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes", cancelButtonText: "No", closeOnConfirm: true, closeOnCancel: true }, function () { ShowLoadingModal(); __doPostBack(document.forms[0].name, s); });
                return false;
            }
            else if (s === 'ClearCurve'){
                swal({ title: "Do you want to proceed?", text: "This action will reset everything in the Curve Fitting section!", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes", cancelButtonText: "No", closeOnConfirm: true, closeOnCancel: true }, function () { ShowLoadingModal(); __doPostBack(document.forms[0].name, s); });
                return false;
            }
            else if (s === 'FitCurve') {
                
                var normalizationradiobuttonlist = document.getElementById('<%=NormalizationRadioButtonList.ClientID%>');
                var listItems = normalizationradiobuttonlist.getElementsByTagName("input");
                var curveradiobuttonlist = document.getElementById('<%=CurveFittingRadioButtonList.ClientID%>');
                var curvelistItems = curveradiobuttonlist.getElementsByTagName("input");
                var bleachingdepthvalue = document.getElementById('<%=BD_GP_Table.ClientID %>').rows[1].cells[0].innerHTML;
                var gapratiovalue = document.getElementById('<%=BD_GP_Table.ClientID %>').rows[1].cells[1].innerHTML;

                if ((gapratiovalue == 'N/A') || (bleachingdepthvalue == 'N/A')) {
                    ShowWarningMessage('Bleaching Depth and the Gap Ratio values have to be estimated in order to continue with curve fitting process. Please complete the 3rd step.');
                    return false;
                }
                else if ((listItems[0].checked == false) && (listItems[1].checked == false)) {
                    ShowWarningMessage('Please select a normalization method.');
                    return false;
                }
                else if ((curvelistItems[0].checked == false) && (curvelistItems[1].checked == false)) {
                    ShowWarningMessage('Please select a curve fitting equation to continue.');
                    return false;
                }
                else  {
                    return true;
                } 
            }
            else if (s === 'Normalize'){
                var normalizationradiobuttonlist = document.getElementById('<%=NormalizationRadioButtonList.ClientID%>');
                var listItems = normalizationradiobuttonlist.getElementsByTagName("input");
                var bleachingdepthvalue = document.getElementById('<%=BD_GP_Table.ClientID %>').rows[1].cells[0].innerHTML;
                var gapratiovalue = document.getElementById('<%=BD_GP_Table.ClientID %>').rows[1].cells[1].innerHTML;


                if ((gapratiovalue == 'N/A') || (bleachingdepthvalue == 'N/A')) {
                    ShowWarningMessage('Bleaching Depth and the Gap Ratio values have to be estimated in order to normalize the dataset. Please complete the 3rd step.');
                    return false;
                }
                else if ((listItems[0].checked == false) && (listItems[1].checked == false)) {
                    ShowWarningMessage('Please select a normalization method.');
                    return false;
                }
                else{
                    return true;
                } 
            }
            else if (s === 'ExportFinalData') {
                var normalizationradiobuttonlist = document.getElementById('<%=NormalizationRadioButtonList.ClientID%>');
                var listItems = normalizationradiobuttonlist.getElementsByTagName("input");
                var curveradiobuttonlist = document.getElementById('<%=CurveFittingRadioButtonList.ClientID%>');
                var curvelistItems = curveradiobuttonlist.getElementsByTagName("input");
                var bleachingdepthvalue = document.getElementById('<%=BD_GP_Table.ClientID %>').rows[1].cells[0].innerHTML;
                var gapratiovalue = document.getElementById('<%=BD_GP_Table.ClientID %>').rows[1].cells[1].innerHTML;
                var GridVwHeaderChckbox = document.getElementById("<%=grdExportFinalData.ClientID %>");

                for (i = 0; i < GridVwHeaderChckbox.rows.length; i++) {
                    GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = false;
                }

                if ((gapratiovalue == 'N/A') || (bleachingdepthvalue == 'N/A')) {
                    ShowWarningMessage('Bleaching Depth and the Gap Ratio values have to be estimated in order to export the normalized dataset. Please complete the 3rd step.');
                    return false;
                }
                else if ((listItems[0].checked == false) && (listItems[1].checked == false)) {
                    ShowWarningMessage('Please select a normalization method.');
                    return false;
                }
                else if ((curvelistItems[0].checked == false) && (curvelistItems[1].checked == false)) {
                    ShowWarningMessage('Please select a curve fitting equation to continue.');
                    return false;
                }
                else {
                    swal({ title: "Important Note", text: "You are about to start a time-consuming operation. The execution time depends on the workload of the system. Also note that this task may take up to 10 minutes for a big dataset. Please do not close this window while the process is in progress. ", html: true, type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Continue", cancelButtonText: "Return", closeOnConfirm: true, closeOnCancel: true }, function () { document.getElementById('modalbtn').click(); });
                    return false;
                }
                }


            else if (s === 'ExportFinalParameters') {
                document.getElementById("closemodalbtn").click();
                var checkedBoxesCount = $('#<%= grdExportFinalData.ClientID%> input[type="checkbox"]:checked').length;
                if (checkedBoxesCount == 0) {
                    ShowWarningMessage('Please select at least one file from the list in order to run this step.');
                    return false;
                }
                else {
                    ShowLoadingModal();
                    setTimeout(function () { __doPostBack(document.forms[0].name, s); }, 1);
                }

            }
            else if (s === 'ExportNormData'){
                var normalizationradiobuttonlist = document.getElementById('<%= NormalizationRadioButtonList.ClientID %>');
                var listItems = normalizationradiobuttonlist.getElementsByTagName("input");
                var bleachingdepthvalue = document.getElementById('<%=BD_GP_Table.ClientID %>').rows[1].cells[0].innerHTML;
                var gapratiovalue = document.getElementById('<%=BD_GP_Table.ClientID %>').rows[1].cells[1].innerHTML;

                if ((gapratiovalue == 'N/A') || (bleachingdepthvalue == 'N/A')) {
                    ShowWarningMessage('Bleaching Depth and the Gap Ratio values have to be estimated in order to export the normalized dataset. Please complete the 3rd step.');
                    return false;
                }
                else if ((listItems[0].checked == false) && (listItems[1].checked == false)) {
                    ShowWarningMessage('Please select a normalization method.');
                    return false;
                }
                else{
                    return true;
                }
            }

        }

        function ShowFileInputInformationScreen(s){
            //START OF BOOTBOX
            bootbox.dialog({
                size: 'medium',
                message:
                    '<form class="form-horizontal"> ' +
                    '<p align="center" style="margin-left: 40px; margin-right: 20px; color: #fff; background-color:rgba(0, 41, 94, 0.8)"><strong>Specify the order of the columns for the selected files.</strong></p>  ' +
                    '<div class="form-group"> ' +
                    '<label class="col-md-6 col-md-offset-1 control-label" for="column1">Specify the column according to the time</label> ' +
                    '<div class="col-md-4 col-md-offset-1"> ' +
                    '<input id="column1" name="column1" type="text" placeholder="Suggested value is 1" value="1" class="form-control input-md"> ' +
                    '</div> ' +
                    '</div> ' +
                    '<div class="form-group"> ' +
                    '<label class="col-md-6 col-md-offset-1 control-label" for="column2">Specify the column corresponding to ROI1 (Region of Interest)</label> ' +
                    '<div class="col-md-4 col-md-offset-1"> ' +
                    '<input id="column2" name="column2" type="text" placeholder="Suggested value is 2" value="2" class="form-control input-md"> ' +
                    '</div> ' +
                    '</div> ' +
                    '<div class="form-group"> ' +
                    '<label class="col-md-6 col-md-offset-1 control-label" for="column3">Specify the column corresponding to ROI2 (Whole Cell Area)</label> ' +
                    '<div class="col-md-4 col-md-offset-1"> ' +
                    '<input id="column3" name="column3" type="text" placeholder="Suggested value is 3" value="3" class="form-control input-md"> ' +
                    '</div> ' +
                    '</div> ' +
                    '<div class="form-group"> ' +
                    '<label class="col-md-6 col-md-offset-1 control-label" for="column4">Specify the column corresponding to ROI3 (Background)</label> ' +
                    '<div class="col-md-4 col-md-offset-1"> ' +
                    '<input id="column4" name="column4" type="text" placeholder="Suggested value is 4" value="4" class="form-control input-md"> ' +
                    '</div> ' +
                    '</div>' +
                    '</form>'
                ,buttons: {
                    confirm: {
                        label: "Proceed",
                        className: "btn-info",
                        callback: function () {
                            ShowLoadingModal();
                            var column1value = $('#column1').val();
                            var column2value = $('#column2').val();
                            var column3value = $('#column3').val();
                            var column4value = $('#column4').val();
                            if (column1value != "1" && column1value != "2" && column1value != "3" && column1value != "4"){
                                HideLoadingModal();
                                ShowErrorMessage('Please check the values that you specified. Only numerical values are accepted between 1 to 4.');
                                return false;
                            }
                            else if (column2value != "1" && column2value != "2" && column2value != "3" && column2value != "4"){
                                HideLoadingModal();
                                ShowErrorMessage('Please check the values that you specified. Only numerical values are accepted between 1 to 4.');
                                return false;
                            }
                            else if (column3value != "1" && column3value != "2" && column3value != "3" && column3value != "4"){
                                HideLoadingModal();
                                ShowErrorMessage('Please check the values that you specified. Only numerical values are accepted between 1 to 4.');
                                return false;
                            }
                            else if (column4value != "1" && column4value != "2" && column4value != "3" && column4value != "4"){
                                HideLoadingModal();
                                ShowErrorMessage('Please check the values that you specified. Only numerical values are accepted between 1 to 4.');
                                return false;
                            }
                            else if ((column1value == column2value) || (column1value == column3value) || (column1value == column4value) || (column2value == column3value) || (column2value == column4value) || (column3value == column4value)){
                                HideLoadingModal();
                                ShowErrorMessage('Please check the values that you specified. Only numerical values are accepted between 1 to 4.');
                                return false;
                            }
                            else if ((parseInt(column1value) + parseInt(column2value) + parseInt(column3value) + parseInt(column4value)) != 10){
                                HideLoadingModal();
                                ShowErrorMessage('Please check the values that you specified. Only numerical values are accepted between 1 to 4.');
                                return false;
                            }
                            s = s + "_" + column1value + column2value + column3value + column4value;
                            setTimeout(function() { __doPostBack(document.forms[0].name, s);} ,1);
                        }
                    }
                },
                closeButton: true
            });
            return false;
            //END OF BOOTBOX 
        }

        function checkAll(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdExportFinalData.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }


        function ShowLoadingModal() {
            //bootbox.dialog({ message: '<div class="text-center"><i class="fa fa-spin fa-spinner"></i> Loading...</div>' });
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
            popup.show();
            }
        }

        function HideLoadingModal() {
            //bootbox.dialog({ message: '<div class="text-center"><i class="fa fa-spin fa-spinner"></i> Loading...</div>' });
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
            popup.hide();
            }
        }

        function ShowWarningMessage(message){
            toastr["warning"](message)

            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": true,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "onclick": null,
                "timeOut": "9000",
                "extendedTimeOut": "7000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
                function ShowErrorMessage(message){
                    toastr["error"](message)

                    toastr.options = {
                        "closeButton": true,
                        "debug": false,
                        "newestOnTop": true,
                        "progressBar": true,
                        "positionClass": "toast-top-right",
                        "preventDuplicates": false,
                        "onclick": null,
                        "timeOut": "12000",
                        "extendedTimeOut": "8000",
                        "showEasing": "swing",
                        "hideEasing": "linear",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut"
                    }
                }
         
                function ShowSuccessMessage(message){
                    toastr["success"](message)

                    toastr.options = {
                        "closeButton": true,
                        "debug": false,
                        "newestOnTop": true,
                        "progressBar": true,
                        "positionClass": "toast-top-right",
                        "preventDuplicates": false,
                        "onclick": null,
                        "timeOut": "4000",
                        "extendedTimeOut": "3000",
                        "showEasing": "swing",
                        "hideEasing": "linear",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut"
                    }
                }

                  function ShowInfoMessage(message){
                      toastr["info"](message)

                      toastr.options = {
                          "closeButton": true,
                          "debug": false,
                          "newestOnTop": true,
                          "progressBar": true,
                          "positionClass": "toast-top-right",
                          "preventDuplicates": false,
                          "onclick": null,
                          "timeOut": "8000",
                          "extendedTimeOut": "6000",
                          "showEasing": "swing",
                          "hideEasing": "linear",
                          "showMethod": "fadeIn",
                          "hideMethod": "fadeOut"
                      }
                  }


</script>

<!--DO NOT DELETE THIS BUTTON -->
<button id="modalbtn" type="button" class="btn btn-info" data-toggle="modal" data-target="#myModal" style="display: none;">Open Modal</button>
<asp:Button runat="server" ID="SaveFinalDataToExcelbtn" OnClick="SaveFinalDataToExcelFilebtn_Click"  />
<!-- Modal -->
<div id="myModal" class="modal fade" role="dialog" data-keyboard="false" data-backdrop="static">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
<%--          <div class="modal-header">
                <p style="margin-left: 40px; text-align:center; margin-right: 20px; color: #fff; background-color:rgba(0, 41, 94, 0.8)"><strong>Select the files for which you want to export the curve fitting parameters.</strong></p>  
          </div>--%>
            <div class="modal-body">
                <div class="form-group">
                    <p style="margin-left: 0px; text-align:center; margin-right: 0px; color: #fff; background-color:rgba(0, 41, 94, 0.8)"><strong>Select the files for which you want to export the curve fitting parameters.</strong></p>
                    <asp:Panel ID="pnlExportFinalData" runat="server" ScrollBars="Vertical" Height="300px">
                    <asp:GridView ID="grdExportFinalData" runat="server" AutoGenerateColumns="False" GridLines="None" CssClass="mGrid"  SelectedRowStyle-BackColor="#E5E5E5" DataKeyNames="RF_ID">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="20%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkHeader" ToolTip="Click here to select/deselect all rows" runat="server" OnClick="checkAll(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRow" runat="server"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("RF_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filename" ItemStyle-Width="80%">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%#Eval("RF_Filename") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                        <SelectedRowStyle BackColor="#E5E5E5" CssClass="RowSelected" />
                        <AlternatingRowStyle CssClass="alt" />
                </asp:GridView>
                </asp:Panel>
                </div>
            </div>
            <div class="modal-footer">
                
                
            
                <div class="col-md-12"> 
                <div class="col-md-6  text-center">
                    <button id="closemodalbtn" type="button" class="btn btn-danger" data-dismiss="modal">CANCEL</button>
                </div>
                <div class="col-md-6 text-center">
                    <asp:LinkButton ID="ExportFinalDatabtn" runat="server" Text="Proceed" CssClass="btn btn-info" OnClientClick="javascript:return DoActions('ExportFinalParameters');"  />
                </div>
               </div>  
            
            </div>
        </div>
      

    </div>
</div>
                    
</ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="ExportRoiDatabtn" />
        <asp:PostBackTrigger ControlID="ExportNormDatabtn"  />
        <asp:PostBackTrigger ControlID="SaveFinalDataToExcelbtn"  />
    </Triggers>
</asp:UpdatePanel>

</div>

    <script>
        function printDygraph(dygraphnumber) {


            var ms_ie = false;
            var ua = window.navigator.userAgent;
            var old_ie = ua.indexOf('MSIE ');
            var new_ie = ua.indexOf('Trident/');
            var newer_ie = ua.indexOf('Edge/');

            //if ((old_ie > -1) || (new_ie > -1) || (newer_ie > -1)) {
            //    ms_ie = true;
            //}

            if (ms_ie) {
                //alert('IE detected');
                ShowWarningMessage('The specific functionality is not available on Internet Explorer and on Microsoft Edge. Please try again using an alternative web browser.');
                return false;
            }
            else {
                if (dygraphnumber == 'ROI') {
                    var img1 = document.getElementById('imgid1');
                    var img2 = document.getElementById('imgid2');
                    var img3 = document.getElementById('imgid3');
                    Dygraph.Export.asPNG(g1, img1);
                    Dygraph.Export.asPNG(g2, img2);
                    Dygraph.Export.asPNG(g3, img3);
                    download(img1.src, "ROI1_Graph.png", "image/png");
                    download(img2.src, "ROI2_Graph.png", "image/png");
                    download(img3.src, "ROI3_Graph.png", "image/png");
                    return false;
                }
                else if (dygraphnumber == 'Normalization') {
                    var img4 = document.getElementById('imgid4');
                    var img5 = document.getElementById('imgid5');
                    Dygraph.Export.asPNG(g4, img4);
                    Dygraph.Export.asPNG(g5, img5);
                    download(img4.src, "Normalization_Graph.png", "image/png");
                    download(img5.src, "Mean&Standard_Deviation_Graph.png", "image/png");
                    return false;
                }
                else if (dygraphnumber == 'Curve_Fitting') {
                    var img6 = document.getElementById('imgid6');
                    var img7 = document.getElementById('imgid7');
                    Dygraph.Export.asPNG(g6, img6);
                    Dygraph.Export.asPNG(g7, img7);
                    download(img6.src, "Curve_Fitting_Graph.png", "image/png");
                    download(img7.src, "Residuals_Graph.png", "image/png");
                    return false;
                }
            }
        }
    </script>
    <script>
        function ShowPopUpForDownload() {
            swal({
                title: "<legend>Work is done!</legend>",
                text: "<label class='col-md-12 control-label'>The estimation was completed. The file is ready to download.</label>",
                type: "info",
                showCancelButton: false,
                //cancelButtonText: "DOWNLOAD",
                showConfirmButton: true,
                confirmButtonText: "CONTINUE",
                closeOnConfirm: true,
                showLoaderOnConfirm: true,
                html: true,
            },
        function () {
            $('#<%= SaveFinalDataToExcelbtn.ClientID %>').click();
        });

        }
    </script>



</asp:Content>
