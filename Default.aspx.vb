Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Drawing
Imports OfficeOpenXml
Imports System.Web.Script.Serialization

Public Class _Default
    Inherits System.Web.UI.Page

#Region " Class Variables "

    Public sAction, sPostBackArgument, sPostBackArgumentEnding, sInfoDiv1, sFileNametbl, sRawDataVisualization, sBleachingParameters, sNormalization, sCurveFitting, sCurveFittingtbl, sDataForCurveGraph, Guid, sNormType, sDataForCurveFittingDots, sLabelsForCurveFittingDots, sDataForResiduals, sDataForKernelDensityThalf, sLabelsForResiduals, sDeleteSessionButton, sSaveResultsSession As String
    Public sCurrentSession As String = ""
    Public sBleachingDepthScore As String = "N/A"
    Public sGapRatioScore As String = "N/A"
    Public sMobileFractionScore As String = "N/A"
    Public sThalfScore As String = "N/A"
    Public sRsquareScore As String = "N/A"
    Public sDataForROI1Graph As String = "[ , ]"
    Public sLabelsForROI1Graph As String = "[ , ]"
    Public sDataForROI2Graph As String = "[ , ]"
    Public sLabelsForROI2Graph As String = "[ , ]"
    Public sDataForROI3Graph As String = "[ , ]"
    Public sLabelsForROI3Graph As String = "[ , ]"
    Public sDataForNormalizationGraph As String = "[ , ]"
    Public sLabelsForNormalizationGraph As String = ""
    Public sLabelsForStandardDevNormalizationGraph As String = "[ , ]"
    Public sDataForStandardDevNormalizationGraph As String = "[ , ]"
    Public sBooleanChecker As Boolean = False
    Public ROI1DataTable, ROI2DataTable, ROI3DataTable, DoubleNormDataTable, FullScaleDataTable, StandardDevDoubleNormDataTable, StandardDevFullScaleNormDataTable, CurveFitDataTable As DataTable


#End Region

#Region " Page Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SaveFinalDataToExcelbtn.Style.Add("display", "none")
        If Not Page.IsPostBack Then
            Initialization()
            If Not IsNothing(Session("GUID")) Then
                Session.Remove("GUID")
            End If
        Else
            sPostBackArgument = Request.Item("__EVENTARGUMENT")
            If sPostBackArgument.Contains("_") Then
                Dim splitter As String() = sPostBackArgument.Split("_")
                sPostBackArgument = splitter(0)
                sPostBackArgumentEnding = splitter(1)
            End If
            Select Case sPostBackArgument
                Case "DeleteSession"
                    DeleteSession()
                    Initialization()
                    If Not IsNothing(Session("GUID")) Then
                        Session.Remove("GUID")
                    End If
                Case "Reset"
                    DeleteSession()
                    Initialization()
                    If Not IsNothing(Session("GUID")) Then
                        Session.Remove("GUID")
                    End If
                Case "UploadFiles"
                    HideFileNameTable()
                    HideRawDataVisualization()
                    HideBleachingParameters()
                    HideNormalization()
                    HideCurveFitting()
                    HideDeleteSessionButton()
                    UploadButtonClicked(sPostBackArgumentEnding)
                    ClearBleachingFieldSetValues()
                    ClearNormalizationFieldset()
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
                Case "ExportFinalParameters"
                    GoToCurveFittingAnchor()
                    DrawRawDataGraphs()
                    EstimateBleachingDepthAndGapRatio()
                    DrawNormalizationGraphs()
                    ExportFinalData()
                    ClearCurveFittingFieldset()
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
                    'ScriptManager.RegisterStartupScript(Me, Page.GetType, "ShowMessageForNewWorker", "ShowMessageForNewWorker();", True)
                Case "ClearNormalize"
                    GoToNormalizationAnchor()
                    ClearNormalizationFieldset()
                    EstimateBleachingDepthAndGapRatio()
                    DrawRawDataGraphs()
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
                Case "ClearCurve"
                    GoToCurveFittingAnchor()
                    ClearCurveFittingFieldset()
                    EstimateBleachingDepthAndGapRatio()
                    DrawRawDataGraphs()
                    DrawNormalizationGraphs()
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
                Case "BleachingParametersReset"
                    GoToBD_GRAnchor()
                    ResetBD_GP_Parameters()
                    RestoreInitialValues()
                    DrawRawDataGraphs()
                    ClearNormalizationFieldset()
                    ClearCurveFittingFieldset()
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
            End Select
        End If

        '''TODELETE WITH SESSION DIV
        'If Not IsNothing(Session("GUID")) Then
        '    sCurrentSession = "There's an active session with ID: " + Session("GUID").ToString
        'Else
        '    sCurrentSession = "No active session found."
        'End If
    End Sub
    Protected Sub ExportRoiDatabtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        ExportROIData()
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub
    Protected Sub SaveFinalDataToExcelFilebtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        SaveToExcel()
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub
    Protected Sub ExportNormDatabtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        ExportNormData()
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub
    Protected Sub RemoveAllbtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        RemoveAll()
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub

    Protected Sub RestoreAllbtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        RestoreAll()
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub
    Protected Sub Normalizebtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        If NormalizationRadioButtonList.SelectedValue = "Double" Then
            RestoreInitialValues()
            DiscardInitialValues(DiscardValuestxtbox.Text.ToString)
            EstimateBleachingDepthAndGapRatio()
            DrawRawDataGraphs()
            DrawNormalizationGraphs(sPostBackArgumentEnding)
            ClearCurveFittingFieldset()
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
        ElseIf NormalizationRadioButtonList.SelectedValue = "FullScale" Then
            RestoreInitialValues()
            DiscardInitialValues(DiscardValuestxtbox.Text.ToString)
            EstimateBleachingDepthAndGapRatio()
            DrawRawDataGraphs()
            DrawNormalizationGraphs(sPostBackArgumentEnding)
            ClearCurveFittingFieldset()
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
        End If
    End Sub
    Protected Sub BleachingParametersCompute_Click(ByVal sender As Object, ByVal e As EventArgs)
        RestoreInitialValues()
        DiscardInitialValues(DiscardValuestxtbox.Text.ToString)
        EstimateBleachingDepthAndGapRatio("BleachingParametersCompute_Click")
        DrawRawDataGraphs()
        DrawNormalizationGraphs()
        ClearCurveFittingFieldset()
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub
    Protected Sub grdFiles_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grdFiles.RowDataBound
        If (e.Row.Cells.Item(4).Text.ToString = "Enabled") Then
            e.Row.Cells.Item(4).ForeColor = Color.Green
            Dim linkbutton As LinkButton
            linkbutton = e.Row.Controls(0).Controls(0)
            linkbutton.Text = "Remove"
        ElseIf (e.Row.Cells.Item(4).Text.ToString = "Disabled") Then
            e.Row.Cells.Item(4).ForeColor = Color.Red
            Dim linkbutton As LinkButton
            linkbutton = e.Row.Controls(0).Controls(0)
            linkbutton.Text = "Restore"
        End If
    End Sub
    Protected Sub grdFiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdFiles.SelectedIndexChanged
        Dim index As Integer = grdFiles.SelectedIndex
        UpdateFileNameTable(grdFiles.DataKeys(index).Value.ToString())
        FillFileNameTable(grdFiles.DataKeys(index).Value.ToString())
        FillCurveFittingTableAndExpFinalDataTable(grdFiles.DataKeys(index).Value.ToString())
        DrawRawDataGraphs()
        ShowBleachingParameters()
        ShowInfoMessage(grdFiles.DataKeys(index).Value.ToString())
        For Each li As ListItem In NormalizationRadioButtonList.Items
            If li.Value = "Double" And li.Selected = True Then
                DrawNormalizationGraphs("Double")
            ElseIf li.Value = "FullScale" And li.Selected = True Then
                DrawNormalizationGraphs("FullScale")
            End If
        Next
        If (PreBleachValuestxtbox.Text <> "" And BleachValuestxtbox.Text <> "" And PostBleachValuestxtbox.Text <> "") Then
            EstimateBleachingDepthAndGapRatio()
        Else
            ClearBleachingFieldSetValues()
        End If
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub
    Protected Sub grdFitCurve_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grdFitCurve.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Cells(0).Attributes.Add("onclick", "return DoActions('FitCurve');")
        End If
    End Sub
    Protected Sub grdFitCurve_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdFitCurve.SelectedIndexChanged
        Dim index As Integer = grdFitCurve.SelectedIndex
        RestoreInitialValues()
        DiscardInitialValues(DiscardValuestxtbox.Text.ToString)
        EstimateBleachingDepthAndGapRatio()
        DrawRawDataGraphs()
        DrawNormalizationGraphs()
        EstimateMobFractionThalfRsquareDrawCurves("FitSelectedCurve_Click", grdFiles.DataKeys(index).Value.ToString())
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub

    Protected Sub FitAllCurve_Click(ByVal sender As Object, ByVal e As EventArgs)
        RestoreInitialValues()
        DiscardInitialValues(DiscardValuestxtbox.Text.ToString)
        EstimateBleachingDepthAndGapRatio()
        DrawRawDataGraphs()
        DrawNormalizationGraphs()
        EstimateMobFractionThalfRsquareDrawCurves("FitAllCurve_Click")
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designRawGraphs", "designRawGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designNormGraphs", "designNormGraphs();", True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "designCurveFittingGraphs", "designCurveFittingGraphs();", True)
    End Sub
    Protected Sub RemoveAll()
        UpdateAllFileNameTable("Remove")
        FillFileNameTable()
        FillCurveFittingTableAndExpFinalDataTable()
        DrawRawDataGraphs()
        ShowBleachingParameters()
        ShowInfoMessage("Remove")
        If (PreBleachValuestxtbox.Text <> "" And BleachValuestxtbox.Text <> "" And PostBleachValuestxtbox.Text <> "") Then
            EstimateBleachingDepthAndGapRatio()
        Else
            ClearBleachingFieldSetValues()
        End If
        For Each li As ListItem In NormalizationRadioButtonList.Items
            If li.Value = "Double" And li.Selected = True Then
                DrawNormalizationGraphs("Double")
            ElseIf li.Value = "FullScale" And li.Selected = True Then
                DrawNormalizationGraphs("FullScale")
            End If
        Next
    End Sub
    Private Sub RestoreAll()
        UpdateAllFileNameTable("Restore")
        FillFileNameTable()
        FillCurveFittingTableAndExpFinalDataTable()
        DrawRawDataGraphs()
        ShowBleachingParameters()
        ShowInfoMessage("Restore")
        For Each li As ListItem In NormalizationRadioButtonList.Items
            If li.Value = "Double" And li.Selected = True Then
                DrawNormalizationGraphs("Double")
            ElseIf li.Value = "FullScale" And li.Selected = True Then
                DrawNormalizationGraphs("FullScale")
            End If
        Next
        If (PreBleachValuestxtbox.Text <> "" And BleachValuestxtbox.Text <> "" And PostBleachValuestxtbox.Text <> "") Then
            EstimateBleachingDepthAndGapRatio()
        Else
            ClearBleachingFieldSetValues()
        End If
    End Sub
    Private Sub UploadButtonClicked(ByVal columnOrder As String)
        Dim checkOK As Boolean = False
        Dim fileExtension As String = ""
        Dim fileNameArray As String() = New String(0) {}
        Dim successtext = String.Empty

        If Not IsNothing(Session("GUID")) Then
            Session.Remove("GUID")
        End If

        fileNameArray = New String(FileUpload1.PostedFiles.Count - 1) {}
        ''CHECK IF ALL FILES HAVE THE SAME EXTENSION (FILETYPE)
        Dim counter As Integer = 0
        For Each postedFile As HttpPostedFile In FileUpload1.PostedFiles
            fileNameArray(counter) = Path.GetFileName(postedFile.FileName)
            counter += 1
            If counter > 100 Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('You are trying to upload more than 100 files that is not allowed in this version of easyFRAP. For batch-file mode please try the standalone version.');", True)
                Return
            End If
            fileExtension = Path.GetExtension(postedFile.FileName)
            If fileExtension <> FileExtensionList1.SelectedValue Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Invalid file type detected. All files should have the declared extention. Please try uploading the proper files.');", True)
                Return
            End If
            If counter > 100 Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('You have exceeded the limit of 100 files per dataset. Please try uploading less files.');", True)
                Return
            End If
        Next

        checkOK = InsertIntoDB(fileExtension, columnOrder)
        If checkOK = True Then
            checkOK = CheckRowsOfFiles(Session("GUID"))
        Else
            Return
        End If


        If checkOK = True Then
            FillFileNameTable()
            FillCurveFittingTableAndExpFinalDataTable()
            If FileUpload1.PostedFiles.Count = 1 Then
                successtext = String.Format("Well done! {0} file has been successfully uploaded.", FileUpload1.PostedFiles.Count)
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowSuccessMessage('" + successtext + "');", True)
            Else
                successtext = String.Format("Well done! {0} files have been successfully uploaded.", FileUpload1.PostedFiles.Count)
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowSuccessMessage('" + successtext + "');", True)
            End If
            DrawRawDataGraphs()
            ShowSections()
        Else
            Return
        End If

    End Sub

#End Region

#Region " Private Functions "

    Private Sub ResetButtonClicked()
        ExpName.Text = ""
        FileExtensionList1.SelectedIndex = 0
        If Not IsNothing(Session("GUID")) Then
            Session.Remove("GUID")
        End If

    End Sub

    Private Sub FillFileNameTable(Optional ByVal RowFileID As String = "")

        Dim sSQL As String
        Dim dtData As DataTable
        Dim dbHandler As New DALDBHandler
        Dim RowFilesGuid As String = Session("GUID")

        If (RowFileID <> "") Then
            RowFilesGuid = GetRowFilesGuid(RowFileID)
            Session("GUID") = RowFilesGuid 'Refresh Session
        End If

        sSQL = "Select RF_ID, RF_FileName, 
                case when RF_IsEnabled = 1 then 'Enabled' 
                else 'Disabled' 
                end as RF_IsEnabled,
                count([vw_Raw_Data].RD_ID) as RD_RowCounter
                from  [dbo].[vw_Imported_Files] 
                inner join [dbo].[vw_Raw_Data] 
                ON [vw_Imported_Files].RF_ID = [vw_Raw_Data].RD_RF_ID
                where [vw_Imported_Files].RF_GUID_ID = " & RowFilesGuid & "
                GROUP BY RF_FileName, RF_IsEnabled, RF_ID
                order by RF_ID "

        dtData = dbHandler.FillTable("", sSQL)

        If IsNothing(dtData) Then
            Exit Sub
        End If

        grdFiles.DataSource = dtData
        grdFiles.DataBind()

        ShowFileNameTable()

    End Sub

    Private Sub FillCurveFittingTableAndExpFinalDataTable(Optional ByVal RowFileID As String = "")

        Dim sSQL As String
        Dim dtData As DataTable
        Dim dbHandler As New DALDBHandler
        Dim RowFilesGuid As String = Session("GUID")

        If (RowFileID <> "") Then
            RowFilesGuid = GetRowFilesGuid(RowFileID)
            Session("GUID") = RowFilesGuid 'Refresh Session
        End If

        sSQL = "Select RF_ID, RF_FileName 
                from  [dbo].[vw_Imported_Files] 
                where [vw_Imported_Files].RF_GUID_ID = " & RowFilesGuid & "
                and RF_IsEnabled = 1
                order by RF_ID "

        dtData = dbHandler.FillTable("", sSQL)

        If IsNothing(dtData) Then
            Exit Sub
        End If

        If dtData.Rows.Count = 0 Then
            ''Create datatable and columns
            Dim dtable As New DataTable
            dtable.Columns.Add(New DataColumn("RF_ID"))
            dtable.Columns.Add(New DataColumn("RF_FileName"))

            'Create object for RowValues
            Dim RowValues As Object() = {"", "There is no file to select."}

            'create new data row
            Dim dRow As DataRow
            dRow = dtable.Rows.Add(RowValues)
            dtable.AcceptChanges()

            'Now bind datatable to gridview... 
            grdFitCurve.DataSource = dtable
            grdFitCurve.DataBind()
            grdFitCurve.Columns(0).Visible = False
            grdFitCurve.Columns(2).ItemStyle.ForeColor = Color.Red
            '
            grdExportFinalData.DataSource = dtable
            grdExportFinalData.DataBind()
            'grdExportFinalData.Columns(0).Visible = False
            'grdExportFinalData.Columns(2).ItemStyle.ForeColor = Color.Red
        Else
            grdFitCurve.DataSource = dtData
            grdFitCurve.DataBind()
            grdFitCurve.Columns(0).Visible = True
            grdFitCurve.Columns(2).ItemStyle.ForeColor = ColorTranslator.FromHtml("#717171")
            '
            grdExportFinalData.DataSource = dtData
            grdExportFinalData.DataBind()
            'grdExportFinalData.Columns(0).Visible = True
            'grdExportFinalData.Columns(2).ItemStyle.ForeColor = ColorTranslator.FromHtml("#717171")
        End If


    End Sub

    Private Function InsertIntoDB(ByVal filetype As String, ByVal fourcolumnorder As String) As Boolean

        Dim sep As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
        Dim NewFileID As String = String.Empty
        Dim filename As String = String.Empty
        Dim FirstColumnName As String = String.Empty
        Dim SecondColumnName As String = String.Empty
        Dim ThirdColumnName As String = String.Empty
        Dim FourthColumnName As String = String.Empty
        'Dim xlApp As Excel.Application
        'Dim xlWorkBook As Excel.Workbook
        'Dim xlWorkSheet As Excel.Worksheet
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn(8) {New DataColumn("RD_ID", GetType(Integer)), New DataColumn("RD_GUID_ID", GetType(Integer)), New DataColumn("RD_RF_ID", GetType(Integer)), New DataColumn("RD_Axis", GetType(String)), New DataColumn("RD_ROI1", GetType(String)), New DataColumn("RD_ROI2", GetType(String)), New DataColumn("RD_ROI3", GetType(String)), New DataColumn("RD_DTInserted", GetType(DateTime)), New DataColumn("RD_IsEnabled", GetType(Integer))})

        Guid = GenerateGUID()
        Session("GUID") = Guid
        ''INSERT RAW DATA INTO DB BY READING ONE FILE PER TIME
        Try
            Dim hfc As HttpFileCollection = Request.Files
            For j As Integer = 0 To hfc.Count - 1
                Dim hpf As HttpPostedFile = hfc(j)

                'get the file name
                filename = Path.GetFileName(hpf.FileName)

                'Generate New File ID - concerns Row_File table
                NewFileID = CreateNewFileID(Guid, filename)
                If NewFileID = 0 Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while uploading your data. Please try again. - (ErrorCode003)');", True)
                    Return False
                End If

                Select Case filetype
                    Case ".csv"
                        Dim str As String = New StreamReader(hpf.InputStream).ReadToEnd()
                        For Each row As String In str.Split(ControlChars.Lf)
                            If Not (row.Contains("Channel") Or row.Contains("ROI")) Then
                                If Not String.IsNullOrEmpty(row) Then
                                    dt.Rows.Add()
                                    dt.Rows(dt.Rows.Count - 1)(1) = Convert.ToInt32(Guid)
                                    dt.Rows(dt.Rows.Count - 1)(2) = Convert.ToInt32(NewFileID)
                                    Dim i As Integer = 3
                                    For Each cell As String In row.Split(","c)
                                        dt.Rows(dt.Rows.Count - 1)(i) = GetDoubleNullable(cell)
                                        i += 1
                                    Next
                                    dt.Rows(dt.Rows.Count - 1)(7) = DateTime.Now
                                    dt.Rows(dt.Rows.Count - 1)(8) = 1
                                End If
                            End If
                        Next
                    Case ".txt"
                        Dim str As String = New StreamReader(hpf.InputStream).ReadToEnd()
                        For Each row As String In str.Split(ControlChars.Lf)
                            If Not (row.Contains("Time") Or row.Contains("Region") Or row.Contains("Intensity")) Then
                                If Not String.IsNullOrEmpty(row) Then
                                    dt.Rows.Add()
                                    dt.Rows(dt.Rows.Count - 1)(1) = Convert.ToInt32(Guid)
                                    dt.Rows(dt.Rows.Count - 1)(2) = Convert.ToInt32(NewFileID)
                                    Dim i As Integer = 3
                                    For Each cell As String In row.Split(ControlChars.Tab)
                                        dt.Rows(dt.Rows.Count - 1)(i) = GetDoubleNullable(cell)
                                        i += 1
                                    Next
                                    dt.Rows(dt.Rows.Count - 1)(7) = DateTime.Now
                                    dt.Rows(dt.Rows.Count - 1)(8) = 1
                                End If
                            End If
                        Next
                    Case ".xlsx"
                        Using excel = New ExcelPackage(hpf.InputStream)
                            Dim ws = excel.Workbook.Worksheets.First()
                            For rowNum = 1 To ws.Dimension.End.Row
                                Dim wsRow = ws.Cells(rowNum, 1, rowNum, ws.Dimension.End.Column)
                                For Each cell In wsRow
                                    If Not IsNumeric(cell.Value) Then
                                        GoTo SkipRow
                                    End If
                                Next
                                Dim row As DataRow = dt.Rows.Add()
                                dt.Rows(dt.Rows.Count - 1)(1) = Convert.ToInt32(Guid)
                                dt.Rows(dt.Rows.Count - 1)(2) = Convert.ToInt32(NewFileID)
                                Dim i As Integer = 3
                                For Each cell In wsRow
                                    dt.Rows(dt.Rows.Count - 1)(i) = GetDoubleNullable(cell.Value)
                                    i += 1
                                Next
                                dt.Rows(dt.Rows.Count - 1)(7) = DateTime.Now
                                dt.Rows(dt.Rows.Count - 1)(8) = 1
SkipRow:                    Next
                        End Using
                    Case ".xls"
                        Dim wb As Net.SourceForge.Koogra.Excel.Workbook = New Net.SourceForge.Koogra.Excel.Workbook(hpf.InputStream)
                        Dim ws As Net.SourceForge.Koogra.Excel.Worksheet = wb.Sheets(0)
                            ws = wb.Sheets.First()
                            Dim rowNum = ws.Rows.MinRow
                            For rowNum = 0 To ws.Rows.MaxRow
                                Dim row As Net.SourceForge.Koogra.Excel.Row = ws.Rows(rowNum)
                                Dim colNum = row.Cells.MinCol
                                If Not IsNumeric(row.Cells(colNum).Value) Then
                                    GoTo SkipRow2
                                End If
                            Dim row2 As DataRow = dt.Rows.Add()
                            dt.Rows(dt.Rows.Count - 1)(1) = Convert.ToInt32(Guid)
                            dt.Rows(dt.Rows.Count - 1)(2) = Convert.ToInt32(NewFileID)
                            Dim i As Integer = 3
                            For colNum = 0 To row.Cells.MaxCol
                                'Console.WriteLine(row.Cells(colNum).Value)
                                dt.Rows(dt.Rows.Count - 1)(i) = GetDoubleNullable(row.Cells(colNum).Value)
                                i += 1
                            Next
                            dt.Rows(dt.Rows.Count - 1)(7) = DateTime.Now
                            dt.Rows(dt.Rows.Count - 1)(8) = 1
SkipRow2:                   Next



                End Select

                Dim consString As String = ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString
                Using con As New SqlConnection(consString)
                    Using sqlBulkCopy As New SqlBulkCopy(con)
                        'Set the database table name
                        sqlBulkCopy.DestinationTableName = "dbo.Raw_Data"
                        sqlBulkCopy.BulkCopyTimeout = 3500
                        Select Case fourcolumnorder
                            Case "1234"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(3, 3)
                                sqlBulkCopy.ColumnMappings.Add(4, 4)
                                sqlBulkCopy.ColumnMappings.Add(5, 5)
                                sqlBulkCopy.ColumnMappings.Add(6, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "1243"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(3, 3)
                                sqlBulkCopy.ColumnMappings.Add(4, 4)
                                sqlBulkCopy.ColumnMappings.Add(6, 5)
                                sqlBulkCopy.ColumnMappings.Add(5, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "1324"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(3, 3)
                                sqlBulkCopy.ColumnMappings.Add(5, 4)
                                sqlBulkCopy.ColumnMappings.Add(4, 5)
                                sqlBulkCopy.ColumnMappings.Add(6, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "1342"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(3, 3)
                                sqlBulkCopy.ColumnMappings.Add(5, 4)
                                sqlBulkCopy.ColumnMappings.Add(6, 5)
                                sqlBulkCopy.ColumnMappings.Add(4, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "1423"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(3, 3)
                                sqlBulkCopy.ColumnMappings.Add(6, 4)
                                sqlBulkCopy.ColumnMappings.Add(4, 5)
                                sqlBulkCopy.ColumnMappings.Add(5, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "1432"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(3, 3)
                                sqlBulkCopy.ColumnMappings.Add(6, 4)
                                sqlBulkCopy.ColumnMappings.Add(5, 5)
                                sqlBulkCopy.ColumnMappings.Add(4, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "2134"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(4, 3)
                                sqlBulkCopy.ColumnMappings.Add(3, 4)
                                sqlBulkCopy.ColumnMappings.Add(5, 5)
                                sqlBulkCopy.ColumnMappings.Add(6, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "2143"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(4, 3)
                                sqlBulkCopy.ColumnMappings.Add(3, 4)
                                sqlBulkCopy.ColumnMappings.Add(6, 5)
                                sqlBulkCopy.ColumnMappings.Add(5, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "2314"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(4, 3)
                                sqlBulkCopy.ColumnMappings.Add(5, 4)
                                sqlBulkCopy.ColumnMappings.Add(3, 5)
                                sqlBulkCopy.ColumnMappings.Add(6, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "2341"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(4, 3)
                                sqlBulkCopy.ColumnMappings.Add(5, 4)
                                sqlBulkCopy.ColumnMappings.Add(6, 5)
                                sqlBulkCopy.ColumnMappings.Add(3, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "2431"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(4, 3)
                                sqlBulkCopy.ColumnMappings.Add(6, 4)
                                sqlBulkCopy.ColumnMappings.Add(5, 5)
                                sqlBulkCopy.ColumnMappings.Add(3, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "2413"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(4, 3)
                                sqlBulkCopy.ColumnMappings.Add(6, 4)
                                sqlBulkCopy.ColumnMappings.Add(3, 5)
                                sqlBulkCopy.ColumnMappings.Add(5, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "3124"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(5, 3)
                                sqlBulkCopy.ColumnMappings.Add(3, 4)
                                sqlBulkCopy.ColumnMappings.Add(4, 5)
                                sqlBulkCopy.ColumnMappings.Add(6, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "3142"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(5, 3)
                                sqlBulkCopy.ColumnMappings.Add(3, 4)
                                sqlBulkCopy.ColumnMappings.Add(6, 5)
                                sqlBulkCopy.ColumnMappings.Add(4, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "3214"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(5, 3)
                                sqlBulkCopy.ColumnMappings.Add(4, 4)
                                sqlBulkCopy.ColumnMappings.Add(3, 5)
                                sqlBulkCopy.ColumnMappings.Add(6, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "3241"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(5, 3)
                                sqlBulkCopy.ColumnMappings.Add(4, 4)
                                sqlBulkCopy.ColumnMappings.Add(6, 5)
                                sqlBulkCopy.ColumnMappings.Add(3, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "3412"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(5, 3)
                                sqlBulkCopy.ColumnMappings.Add(6, 4)
                                sqlBulkCopy.ColumnMappings.Add(3, 5)
                                sqlBulkCopy.ColumnMappings.Add(4, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "3421"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(5, 3)
                                sqlBulkCopy.ColumnMappings.Add(6, 4)
                                sqlBulkCopy.ColumnMappings.Add(4, 5)
                                sqlBulkCopy.ColumnMappings.Add(3, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "4123"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(6, 3)
                                sqlBulkCopy.ColumnMappings.Add(3, 4)
                                sqlBulkCopy.ColumnMappings.Add(4, 5)
                                sqlBulkCopy.ColumnMappings.Add(5, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "4132"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(6, 3)
                                sqlBulkCopy.ColumnMappings.Add(3, 4)
                                sqlBulkCopy.ColumnMappings.Add(5, 5)
                                sqlBulkCopy.ColumnMappings.Add(4, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "4213"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(6, 3)
                                sqlBulkCopy.ColumnMappings.Add(4, 4)
                                sqlBulkCopy.ColumnMappings.Add(3, 5)
                                sqlBulkCopy.ColumnMappings.Add(5, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "4231"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(6, 3)
                                sqlBulkCopy.ColumnMappings.Add(4, 4)
                                sqlBulkCopy.ColumnMappings.Add(5, 5)
                                sqlBulkCopy.ColumnMappings.Add(3, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "4312"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(6, 3)
                                sqlBulkCopy.ColumnMappings.Add(5, 4)
                                sqlBulkCopy.ColumnMappings.Add(3, 5)
                                sqlBulkCopy.ColumnMappings.Add(4, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                            Case "4321"
                                sqlBulkCopy.ColumnMappings.Add(0, 0)
                                sqlBulkCopy.ColumnMappings.Add(1, 1)
                                sqlBulkCopy.ColumnMappings.Add(2, 2)
                                sqlBulkCopy.ColumnMappings.Add(6, 3)
                                sqlBulkCopy.ColumnMappings.Add(5, 4)
                                sqlBulkCopy.ColumnMappings.Add(4, 5)
                                sqlBulkCopy.ColumnMappings.Add(3, 6)
                                sqlBulkCopy.ColumnMappings.Add(7, 7)
                                sqlBulkCopy.ColumnMappings.Add(8, 8)
                        End Select
                        con.Open()
                        sqlBulkCopy.WriteToServer(dt)
                        con.Close()
                    End Using
                End Using
                dt.Clear()
            Next
            Return True
        Catch ex As Exception
            LogToFile("InsertIntoDB()", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while uploading your data. Please check your files. They probably contain either more than 4 colums or more than 3 delimiters in certain lines. Please try again. - (ErrorCode001)');", True)
            Return False
        End Try

        Return False
    End Function

    Public Shared Function GetDoubleNullable(ByVal doublestring As String) As Double
        Dim retval As Double
        Dim sep As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

        If Double.TryParse(Replace(Replace(doublestring, ".", sep), ",", sep), retval) Then
            Return retval
        Else
            Return Nothing
        End If
    End Function
    Private Function GenerateGUID() As String

        Dim textGUID As String = String.Empty
        Dim ExperimentName As String = String.Empty
        Dim query As String = String.Empty
        query &= "INSERT INTO [dbo].[GUID] ([GUID_Text], [GUID_ExperimentName], [GUID_DTCreated]) OUTPUT INSERTED.GUID_ID "
        query &= "VALUES (@GUID_Text, @GUID_ExperimentName, @GUID_DTCreated) "
        textGUID = System.Guid.NewGuid.ToString()
        ExperimentName = ExpName.Text
        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@GUID_Text", textGUID)
                        .Parameters.AddWithValue("@GUID_ExperimentName", ExperimentName)
                        .Parameters.AddWithValue("@GUID_DTCreated", DateTime.Now)
                    End With
                    conn.Open()
                    Guid = comm.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            LogToFile("GenerateGUID() - " + query, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while uploading your data. Please try again. - (ErrorCode002)');", True)
            Return Nothing
        End Try
        Return Guid
    End Function

    Private Function CreateNewFileID(ByVal guid As String, ByVal filename As String) As String
        Dim FileID As String = String.Empty
        Dim query As String = String.Empty
        query &= "INSERT INTO [dbo].[Imported_Files] ([RF_GUID_ID], [RF_FileName], [RF_DTCreated], [RF_IsEnabled]) OUTPUT INSERTED.RF_ID "
        query &= "VALUES (@RF_GUID_ID, @RF_FileName, @RF_DTCreated, @RF_IsEnabled) "

        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@RF_GUID_ID", guid)
                        .Parameters.AddWithValue("@RF_FileName", filename)
                        .Parameters.AddWithValue("@RF_DTCreated", DateTime.Now)
                        .Parameters.AddWithValue("@RF_IsEnabled", 1)
                    End With
                    conn.Open()
                    FileID = comm.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            LogToFile("CreateNewFileID() " + query, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while uploading your data. Please try again. - (ErrorCode011)');", True)
            FileID = "0"
        End Try
        Return FileID
    End Function

    Private Sub UpdateFileNameTable(ByVal RowFileID As String)
        Dim query As String = String.Empty
        Dim ExecID As String = String.Empty
        Dim Param As String = String.Empty

        Param = CheckIfActiveOrInactive(RowFileID)

        Select Case Param
            Case "True"
                Param = "0"
            Case "False"
                Param = "1"
        End Select

        query &= "UPDATE [dbo].[Imported_Files] Set RF_IsEnabled = @RF_IsEnabled WHERE RF_ID = @RF_ID"

        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@RF_IsEnabled", Param)
                        .Parameters.AddWithValue("@RF_ID", RowFileID)
                    End With
                    conn.Open()
                    comm.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            LogToFile("UpdateFileNameTable() " + query, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode004)');", True)
        End Try
    End Sub

    Private Sub UpdateAllFileNameTable(ByVal sCommand As String)
        Dim query As String = String.Empty
        Dim ExecID As String = String.Empty
        Dim Param As String = String.Empty
        Dim nGuid As String = String.Empty

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        If sCommand = "Remove" Then
            Param = 0
        ElseIf sCommand = "Restore" Then
            Param = 1
        End If

        query &= "UPDATE [dbo].[Imported_Files] Set RF_IsEnabled = @RF_IsEnabled WHERE RF_GUID_ID = @RF_GUID_ID"

        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@RF_IsEnabled", Param)
                        .Parameters.AddWithValue("@RF_GUID_ID", nGuid)
                    End With
                    conn.Open()
                    comm.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            LogToFile("UpdateAllFileNameTable() " + query, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode004)');", True)
        End Try
    End Sub

    Private Sub RestoreInitialValues()
        Dim query As String = String.Empty
        Dim ExecID As String = String.Empty
        Dim nGuid = String.Empty

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        query &= "UPDATE [dbo].[Raw_Data] Set RD_IsEnabled = @RD_IsEnabled WHERE RD_GUID_ID = @RD_GUID_ID"

        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@RD_IsEnabled", 1)
                        .Parameters.AddWithValue("@RD_GUID_ID", nGuid)
                    End With
                    conn.Open()
                    comm.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            LogToFile("RestoreInitialValues()" + query, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode010)');", True)
        End Try
    End Sub
    Private Sub DiscardInitialValues(ByVal InitValues As String)
        Dim sProcName As String = String.Empty
        Dim ExecID As String = String.Empty
        Dim nGuid = String.Empty

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        If (InitValues = "" Or InitValues = "0") Then
            Return
        End If

        Try
            sProcName = "proc_DiscardInitialValues"

            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = sProcName
                        .Parameters.AddWithValue("@sGUID", nGuid)
                        .Parameters.AddWithValue("@INITIALVALUESTODISCARD", InitValues)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            LogToFile("DiscardInitialValues() - " + sProcName, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode012)');", True)
        End Try
    End Sub

    Private Function CheckIfActiveOrInactive(ByVal RowFileID As String) As String
        Dim sSQL As String
        Dim return_value As String = String.Empty

        sSQL = "SELECT RF_IsEnabled FROM [dbo].[vw_Imported_Files] WHERE RF_ID = @RF_ID"
        'Return dbHandler.ExecuteSQL(sSQL)
        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = sSQL
                        .Parameters.AddWithValue("@RF_ID", RowFileID)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            return_value = dr("RF_IsEnabled").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("CheckIfActiveOrInactive() - " + sSQL, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode005)');", True)
            Return Nothing
        End Try

        Return return_value
    End Function

    Private Function GetRowFilesGuid(ByVal RowFileID As String) As String
        Dim sSQL As String
        Dim return_value As String = String.Empty

        sSQL = "SELECT RF_GUID_ID FROM [dbo].[vw_Imported_Files] WHERE RF_ID = @RF_ID"
        'Return dbHandler.ExecuteSQL(sSQL)
        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = sSQL
                        .Parameters.AddWithValue("@RF_ID", RowFileID)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            return_value = dr("RF_GUID_ID").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("GetRowFilesGuid() - " + sSQL, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode006)');", True)
            Return Nothing
        End Try
        Return return_value
    End Function
    Public Function GetDataForCurveFitGraph(ByVal nIndex As String, ByVal nPreBleachValue As String, ByVal nBleachValue As String, ByVal nInitialValues As String, ByVal nNormType As String) As DataTable
        Dim dsData As New DataSet
        Dim sProcName As String = String.Empty
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim IntPreBleachValues As Integer = 0
        Dim IntInitialValuesToDiscard As Integer = 0

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand

        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        If nNormType = "Double" Then
            sProcName = "proc_ReturnDataForCurveGraphsDoubleNorm"
        ElseIf nNormType = "FullScale" Then
            sProcName = "proc_ReturnDataForCurveGraphsFullScaleNorm"
        End If

        Try

            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = sProcName


            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("sRAWFILEID", nIndex)
            m_cmSQL.Parameters.AddWithValue("@PREBLEACHVALUES", nPreBleachValue)
            m_cmSQL.Parameters.AddWithValue("@BLEACHVALUES", nBleachValue)
            m_cmSQL.Parameters.AddWithValue("@INITIALBLEACHVALUES", nInitialValues)

            m_cnSQL.Open()
            m_daSQL.Fill(dsData, sProcName)

            If (dsData.Tables.Count > 0) Then
                Return dsData.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            LogToFile("GetDataForCurveFitGraph() - " + sProcName & "( " & nIndex & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode031)');", True)
            Return Nothing
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try
    End Function

    Public Function GetDataForStandardDevDoubleNormGraph(ByVal nGuid As String, ByVal nPreBleachValue As String, ByVal nInitialValues As String) As DataTable
        Dim dsData As New DataSet
        Dim sProcName As String
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim IntPreBleachValues As Integer = 0
        Dim IntInitialValuesToDiscard As Integer = 0

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand

        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        sProcName = "proc_ReturnStandardDevForDoubleNormGraphs"

        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = sProcName


            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", nGuid)
            m_cmSQL.Parameters.AddWithValue("@PREBLEACHVALUES", nPreBleachValue)
            m_cmSQL.Parameters.AddWithValue("@INITIALBLEACHVALUES", nInitialValues)

            m_cnSQL.Open()
            m_daSQL.Fill(dsData, sProcName)

            If (dsData.Tables.Count > 0) Then
                Return dsData.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            LogToFile("GetDataForStandardDevDoubleNormGraph() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode017)');", True)
            Return Nothing
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try
    End Function

    Public Function GetDataForStandardDevFullScaleNormGraph(ByVal nGuid As String, ByVal nPreBleachValue As String, ByVal nBleachValue As String, ByVal nInitialValues As String) As DataTable
        Dim dsData As New DataSet
        Dim sProcName As String
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim IntPreBleachValues As Integer = 0
        Dim IntInitialValuesToDiscard As Integer = 0

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand

        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        sProcName = "proc_ReturnStandardDevForFullScaleNormGraphs"

        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = sProcName


            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", nGuid)
            m_cmSQL.Parameters.AddWithValue("@PREBLEACHVALUES", nPreBleachValue)
            m_cmSQL.Parameters.AddWithValue("@BLEACHVALUES", nBleachValue)
            m_cmSQL.Parameters.AddWithValue("@INITIALBLEACHVALUES", nInitialValues)

            m_cnSQL.Open()
            m_daSQL.Fill(dsData, sProcName)

            If (dsData.Tables.Count > 0) Then
                Return dsData.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            LogToFile("GetDataForStandardDevFullScaleNormGraph() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode021)');", True)
            Return Nothing
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try
    End Function

    Private Sub DrawStandardDevForDoubleNormGraph(ByVal DtTable As DataTable)

        sDataForStandardDevNormalizationGraph = ""
        Dim sDataForStandardDevNormalizationGraphTemp As String = ""

        If DtTable.Rows.Count = 0 Then
            sDataForStandardDevNormalizationGraph = "[ , ]"
            Exit Sub
        End If

        For Each row As DataRow In DtTable.Rows
            sDataForStandardDevNormalizationGraphTemp = ""
            For Each item In row.Table.Columns
                Dim decimalwithdot As String = row.Item(item.Ordinal).ToString
                decimalwithdot = decimalwithdot.Replace(",", ".")
                If decimalwithdot = "" Then
                    decimalwithdot = "null"
                End If
                If (item.ColumnName.ToString = "Low") Then
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + "["
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + decimalwithdot + ", "
                ElseIf (item.ColumnName.ToString = "High") Then
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + decimalwithdot
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + "]"
                Else
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + decimalwithdot + ", "
                End If

            Next
            sDataForStandardDevNormalizationGraph = sDataForStandardDevNormalizationGraph + "[ " + sDataForStandardDevNormalizationGraphTemp + "], "
        Next
        sDataForStandardDevNormalizationGraph = sDataForStandardDevNormalizationGraph.Remove(sDataForStandardDevNormalizationGraph.Length - 2)
        sDataForStandardDevNormalizationGraph = "[ " + sDataForStandardDevNormalizationGraph + " ]"
        'LogGraphPointsToFile(sDataForStandardDevNormalizationGraph)
    End Sub

    Private Sub DrawStandardDevForFullScaleNormGraph(ByVal DtTable As DataTable)

        sDataForStandardDevNormalizationGraph = ""
        Dim sDataForStandardDevNormalizationGraphTemp As String = ""

        If DtTable.Rows.Count = 0 Then
            sDataForStandardDevNormalizationGraph = "[ , ]"
            Exit Sub
        End If

        For Each row As DataRow In DtTable.Rows
            sDataForStandardDevNormalizationGraphTemp = ""
            For Each item In row.Table.Columns
                Dim decimalwithdot As String = row.Item(item.Ordinal).ToString
                decimalwithdot = decimalwithdot.Replace(",", ".")
                If decimalwithdot = "" Then
                    decimalwithdot = "null"
                End If
                If (item.ColumnName.ToString = "Low") Then
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + "["
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + decimalwithdot + ", "
                ElseIf (item.ColumnName.ToString = "High") Then
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + decimalwithdot
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + "]"
                Else
                    sDataForStandardDevNormalizationGraphTemp = sDataForStandardDevNormalizationGraphTemp + decimalwithdot + ", "
                End If

            Next
            sDataForStandardDevNormalizationGraph = sDataForStandardDevNormalizationGraph + "[ " + sDataForStandardDevNormalizationGraphTemp + "], "
        Next
        sDataForStandardDevNormalizationGraph = sDataForStandardDevNormalizationGraph.Remove(sDataForStandardDevNormalizationGraph.Length - 2)
        sDataForStandardDevNormalizationGraph = "[ " + sDataForStandardDevNormalizationGraph + " ]"
        'LogGraphPointsToFile(sDataForStandardDevNormalizationGraph)
    End Sub

    Private Sub ShowInfoMessage(ByVal RowFileID As String)
        Dim sSQL As String
        Dim return_status As String = String.Empty
        Dim return_filename As String = String.Empty

        If RowFileID = "Remove" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowInfoMessage('All the files were removed from the current dataset.');", True)
            Return
        ElseIf RowFileID = "Restore" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowInfoMessage('All the files were successfully restored.');", True)
            Return
        End If

        sSQL = "Select RF_FileName, RF_IsEnabled FROM [dbo].[vw_Imported_Files] WHERE RF_ID = @RF_ID"
        'Return dbHandler.ExecuteSQL(sSQL)
        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = sSQL
                        .Parameters.AddWithValue("@RF_ID", RowFileID)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            return_filename = dr("RF_FileName").ToString()
                            return_status = dr("RF_IsEnabled").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("ShowInfoMessage() - " + sSQL, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode014)');", True)
            Return
        End Try
        If return_status = "True" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowInfoMessage('The file " & return_filename & " was successfully restored.');", True)
            Return
        ElseIf return_status = "False" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowInfoMessage('The file " & return_filename & " was removed from the current dataset.');", True)
            Return
        End If
    End Sub
    Private Sub ConcatDataForDoubleNorm(ByVal nDataForNormalizationGraphTemp As String)
        sDataForNormalizationGraph = sDataForNormalizationGraph + nDataForNormalizationGraphTemp
    End Sub

    Private Sub ConcatDataForCurveFitting(ByVal nDataForCurveFittingTemp As String)
        sDataForCurveFittingDots = sDataForCurveFittingDots + nDataForCurveFittingTemp
    End Sub
    Private Sub ConcatDataForResiduals(ByVal nDataForResidualsTemp As String)
        sDataForResiduals = sDataForResiduals + nDataForResidualsTemp
    End Sub

    Private Sub ConcatDataForStandardDevDoubleNorm(ByVal nDataForStandardDevNormalizationGraphTemp As String)
        sDataForStandardDevNormalizationGraph = sDataForStandardDevNormalizationGraph + nDataForStandardDevNormalizationGraphTemp
    End Sub

    Private Function LogToFile(ByVal sSQL As String, ByVal sMessage As String) As Boolean
        Dim NextFile As Short

        Try
            NextFile = FreeFile()
            FileOpen(NextFile, HttpContext.Current.Server.MapPath("~") & "\ErrorLOG.log.txt.config", OpenMode.Append)
            PrintLine(NextFile, Now & vbCrLf & "SQL: " &
                          sSQL & vbCrLf & "Error: " & sMessage & vbCrLf)
            FileClose(NextFile)
        Catch MyEx As System.UnauthorizedAccessException
            'MsgBox(MyEx.ToString, MsgBoxStyle.OkOnly)
        End Try
        Return 1
    End Function

    Private Function LogGraphPointsToFile(ByVal sMessage As String) As Boolean
        Dim NextFile As Short

        Try
            NextFile = FreeFile()
            FileOpen(NextFile, HttpContext.Current.Server.MapPath("~") & "\ROI.log.txt.config", OpenMode.Append)
            PrintLine(NextFile, Now & vbCrLf & sMessage & vbCrLf)
            FileClose(NextFile)
        Catch MyEx As System.UnauthorizedAccessException
            'MsgBox(MyEx.ToString, MsgBoxStyle.OkOnly)
        End Try
        Return 1
    End Function

    Private Function CheckRowsOfFiles(ByVal nGuid As String) As Boolean

        Dim sProcName As String
        Dim return_problematicfiles As String = String.Empty
        Dim return_problematicfilescounter As String = String.Empty

        sProcName = "proc_CheckNumbersOfRowsPerFile"
        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = sProcName
                        .Parameters.AddWithValue("@sGUID", nGuid)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            return_problematicfiles = dr("Problematic Files").ToString()
                            return_problematicfilescounter = dr("Problematic Files Counter").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("CheckRowsOfFiles() - " + sProcName, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode015)');", True)
            Return Nothing
        End Try
        If return_problematicfilescounter = "0" Then
            Return True
        ElseIf return_problematicfilescounter = "1" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('All the files should have the same number of rows. The file " + return_problematicfiles + " has different number of rows compared to the other files. Please check your files and try again.');", True)
            Return False
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('All files should have the same number of rows. The files " + return_problematicfiles + " have different number of rows compared with the other files. Please check your files and try again.');", True)
            Return False
        End If

    End Function

    Private Sub DeleteSession()

        Dim sProcName As String
        Dim return_message As String = String.Empty
        Dim nGuid As String

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('There is no active session. Please restart your analysis by uploading new files.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        sProcName = "proc_DeleteSession"

        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandTimeout = 3500
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = sProcName
                        .Parameters.AddWithValue("@sGUID", nGuid)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            return_message = dr("strMessage").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("DeleteSession() - " + sProcName, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('A problem occured while deleting your dataset. Your files will be automatically deleted within the next few hours. - (ErrorCode034)');", True)
            Return
        End Try
        If return_message = "Success" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowSuccessMessage('The uploaded data were successfully deleted!');", True)
            Return
        ElseIf return_message = "There is no file with this GUID" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowWarningMessage('There is nothing to delete. Your dataset has been deleted.');", True)
            Return
        ElseIf return_message = "Failed to delete files" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('A problem occured while deleting your dataset. Your files will be automatically deleted within the next few hours.');", True)
            Return
        End If

    End Sub

    Private Sub HideFileNameTable()
        sFileNametbl = "style='display:none;'"
    End Sub

    Private Sub ShowFileNameTable()
        sFileNametbl = ""
    End Sub

    Private Sub HideRawDataVisualization()
        sRawDataVisualization = "style='display:none;'"
    End Sub

    Private Sub ShowRawDataVisualization()
        sRawDataVisualization = ""
    End Sub

    Private Sub HideBleachingParameters()
        sBleachingParameters = "style='display:none;'"
    End Sub

    Private Sub ShowBleachingParameters()
        sBleachingParameters = ""
    End Sub

    Private Sub HideNormalization()
        sNormalization = "style='display:none;'"
    End Sub

    Private Sub ShowNormalization()
        sNormalization = ""
    End Sub

    Private Sub HideCurveFitting()
        sCurveFitting = "style='display:none;'"
    End Sub

    Private Sub ShowCurveFitting()
        sCurveFitting = ""
    End Sub

    Private Sub HideDeleteSessionButton()
        sDeleteSessionButton = "style='display:none;'"
    End Sub

    Private Sub ShowDeleteSessionButton()
        sDeleteSessionButton = ""
    End Sub

    Private Sub HideSaveResultsSession()
        sSaveResultsSession = "style='display:none;'"
    End Sub

    Private Sub ShowSaveResultsSession()
        sSaveResultsSession = ""
    End Sub



    Private Sub ClearBleachingFieldSetValues()
        PreBleachValuestxtbox.Text = String.Empty
        BleachValuestxtbox.Text = String.Empty
        PostBleachValuestxtbox.Text = String.Empty
        DiscardValuestxtbox.Text = String.Empty
        sBleachingDepthScore = "N/A"
        sGapRatioScore = "N/A"
    End Sub

    Private Sub ClearNormalizationRadioButtons()
        NormalizationRadioButtonList.SelectedIndex = -1
    End Sub
    Private Sub ClearCurveFittingRadioButtons()
        CurveFittingRadioButtonList.SelectedIndex = -1
    End Sub

    Private Sub EstimateBleachingDepthAndGapRatio(Optional ByVal ButtonPressed As String = "")
        Dim PreBleachValues As String = String.Empty
        Dim BleachValues As String = String.Empty
        Dim PostBleachValues As String = String.Empty
        Dim IntPreBleachValues As Integer = 0
        Dim IntBleachValues As Integer = 0
        Dim IntPostBleachValues As Integer = 0
        Dim InitialValues As String = String.Empty
        Dim bleaching_depth As String = String.Empty
        Dim gap_ratio As String = String.Empty
        Dim nGuid As String = String.Empty
        Dim sProcName As String
        Dim return_problematicfiles As String = String.Empty
        Dim return_problematicfilescounter As String = String.Empty
        Dim returnedsumofrows As String = String.Empty
        Dim IntReturnedSumofRows As Integer = 0
        Dim SuccessEstimationFlag As Boolean = False

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        PreBleachValues = PreBleachValuestxtbox.Text
        Int32.TryParse(PreBleachValues, IntPreBleachValues)
        BleachValues = BleachValuestxtbox.Text
        Int32.TryParse(BleachValues, IntBleachValues)
        PostBleachValues = PostBleachValuestxtbox.Text
        Int32.TryParse(PostBleachValues, IntPostBleachValues)
        If (DiscardValuestxtbox.Text <> "") Then
            InitialValues = DiscardValuestxtbox.Text
        Else
            InitialValues = "0"
        End If

        If ((PreBleachValues = "") Or (BleachValues = "") Or (PostBleachValues = "")) Then
            Return
        End If

        sProcName = "proc_SimpleCheckNumberOfRowsPerFile"
        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = sProcName
                        .Parameters.AddWithValue("@sGUID", nGuid)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            returnedsumofrows = dr("Rows_Per_File").ToString()
                            Int32.TryParse(returnedsumofrows, IntReturnedSumofRows)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("EstimateBleachingDepthAndGapRatio() - " + sProcName, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode016)');", True)
            ClearNormalizationFieldset()
            Return
        End Try
        If (IntReturnedSumofRows <> IntPreBleachValues + IntBleachValues + IntPostBleachValues) Then
            sBleachingDepthScore = "N/A"
            sGapRatioScore = "N/A"
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('The sum of values that specified does not match the total number of rows per file. Please check and correct the given values.');", True)
            ClearNormalizationFieldset()
            Return
        End If

        sProcName = "proc_BleachingDepth"
        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = sProcName
                        .Parameters.AddWithValue("@sGUID", nGuid)
                        .Parameters.AddWithValue("@PREBLEACHVALUES", PreBleachValues)
                        .Parameters.AddWithValue("@BLEACHVALUES", BleachValues)
                        .Parameters.AddWithValue("@POSTBLEACHVALUES", PostBleachValues)
                        .Parameters.AddWithValue("@INITIALBLEACHVALUES", InitialValues)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            bleaching_depth = dr("Bleaching_Depth").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("EstimateBleachingDepthAndGapRatio() - " + sProcName, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode018)');", True)
            ClearNormalizationFieldset()
            Return
        End Try
        If (bleaching_depth <> "") Then
            sBleachingDepthScore = bleaching_depth
            SuccessEstimationFlag = True
        Else
            sBleachingDepthScore = "N/A"
        End If

        sProcName = "proc_GapRatio"
        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = sProcName
                        .Parameters.AddWithValue("@sGUID", nGuid)
                        .Parameters.AddWithValue("@PREBLEACHVALUES", PreBleachValues)
                        .Parameters.AddWithValue("@BLEACHVALUES", BleachValues)
                        .Parameters.AddWithValue("@POSTBLEACHVALUES", PostBleachValues)
                        .Parameters.AddWithValue("@INITIALBLEACHVALUES", InitialValues)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            gap_ratio = dr("Gap_Ratio").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("EstimateBleachingDepthAndGapRatio() - " + sProcName, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode019)');", True)
            ClearNormalizationFieldset()
            Return
        End Try
        If (gap_ratio <> "") Then
            sGapRatioScore = gap_ratio
            SuccessEstimationFlag = True
        Else
            sGapRatioScore = "N/A"
        End If

        If ((SuccessEstimationFlag = True) And (ButtonPressed = "BleachingParametersCompute_Click")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowSuccessMessage('Bleaching Depth and Gap Ratio were successfully calculated.');", True)
        End If

    End Sub

    Protected Sub DrawRawDataGraphs()
        sDataForROI1Graph = ReturnROIData(Session("Guid"), "proc_ReturnDataForROI1Graphs")
        sLabelsForROI1Graph = ReturnLabels(Session("Guid"), "proc_ReturnDataForROI1Graphs")
        sDataForROI2Graph = ReturnROIData(Session("Guid"), "proc_ReturnDataForROI2Graphs")
        sLabelsForROI2Graph = ReturnLabels(Session("Guid"), "proc_ReturnDataForROI2Graphs")
        sDataForROI3Graph = ReturnROIData(Session("Guid"), "proc_ReturnDataForROI3Graphs")
        sLabelsForROI3Graph = ReturnLabels(Session("Guid"), "proc_ReturnDataForROI3Graphs")
    End Sub

    Private Sub ExportNormData()
        Dim DoubleNormDataPrint As DataTable = New DataTable("proc_ExportDataForDoubleNorm")
        Dim FullScaleNormDataPrint As DataTable = New DataTable("proc_ExportDataForFullScaleNorm")
        Dim sReportName As String = "easyFRAP_Normalized_Data"
        Dim DoubleNormColumnsCounter, DoubleNormRowsCounter, FullScaleNormColumnsCounter, FullScaleNormRowsCounter As Integer
        Dim nGuid As String = String.Empty
        Dim nPreBleachValues As String = String.Empty
        Dim nBleachValues As String = String.Empty
        Dim nPostBleachValues As String = String.Empty
        Dim nInitialValues As String = String.Empty
        Dim dsData As New DataSet
        Dim sProcName, sProcName2 As String
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim pck As New ExcelPackage
        Dim ws, ws2 As ExcelWorksheet

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        nPreBleachValues = PreBleachValuestxtbox.Text
        nBleachValues = BleachValuestxtbox.Text
        nPostBleachValues = PostBleachValuestxtbox.Text
        If (DiscardValuestxtbox.Text <> "") Then
            nInitialValues = DiscardValuestxtbox.Text
        Else
            nInitialValues = "0"
        End If

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500
        sProcName = "proc_ExportDataForDoubleNorm"
        sProcName2 = "proc_ExportDataForFullScaleNorm"

        ''CREATE A DATASET FOR DOUBLE NORMALIZATION DATA
        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = sProcName
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", nGuid)
            m_cmSQL.Parameters.AddWithValue("@PREBLEACHVALUES", nPreBleachValues)
            m_cmSQL.Parameters.AddWithValue("@INITIALBLEACHVALUES", nInitialValues)
            m_cnSQL.Open()
            Dim numofrows As Integer = m_daSQL.Fill(dsData, sProcName)

            If numofrows = 0 Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowWarningMessage('The dataset is empty. Please select at least one file to export.');", True)
                Return
            End If

            'Convert DataSet to DataTable (concerns Double Normalization method)
            DoubleNormDataPrint = dsData.Tables(0)
            DoubleNormColumnsCounter = dsData.Tables(0).Columns.Count
            DoubleNormRowsCounter = dsData.Tables(0).Rows.Count

        Catch ex As Exception
            LogToFile("ExportNormData() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode022)');", True)
            Return
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        ''CREATE A DATASET FOR FULL SCALE NORMALIZATION DATA
        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = sProcName2
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", nGuid)
            m_cmSQL.Parameters.AddWithValue("@PREBLEACHVALUES", nPreBleachValues)
            m_cmSQL.Parameters.AddWithValue("@BLEACHVALUES", nBleachValues)
            m_cmSQL.Parameters.AddWithValue("@INITIALBLEACHVALUES", nInitialValues)
            m_cnSQL.Open()
            m_daSQL.Fill(dsData, sProcName2)

            'Convert DataSet to DataTable (concerns Full Scale Normalization method)
            FullScaleNormDataPrint = dsData.Tables(1)
            FullScaleNormColumnsCounter = dsData.Tables(1).Columns.Count
            FullScaleNormRowsCounter = dsData.Tables(1).Rows.Count

        Catch ex As Exception
            LogToFile("ExportNormData() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode023)');", True)
            Return
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try


        ''
        ''1st Excel Sheet for Double Normalization is created here
        ws = pck.Workbook.Worksheets.Add("Double Normalization")
        ws.Cells.AutoFitColumns()
        ws.Cells.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws.Cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White)

        'add logo
        Dim logo As System.Drawing.Image
        logo = System.Drawing.Image.FromFile(Server.MapPath("img/easyFRAP-logo-Web-JPG.jpg"))
        Dim pic = ws.Drawings.AddPicture("logo", logo)
        pic.From.Column = 0
        pic.From.Row = 0
        pic.SetSize(11)

        'add info rows
        ws.Cells("A4").Value = "Date: " & Now.ToShortDateString
        ws.Cells("A5").Value = "Report: Normalized Data using Double Method"
        ws.Cells("A6").Value = "Prebleach Values: " & nPreBleachValues
        ws.Cells("A7").Value = "Bleach Values: " & nBleachValues
        ws.Cells("A8").Value = "Postbleach Values: " & nPostBleachValues
        ws.Cells("A9").Value = "Initial Deleted Values: " & nInitialValues
        ws.Cells("A4:A5").Style.Font.Bold = True
        ws.Cells("A11").LoadFromDataTable(DoubleNormDataPrint, True)

        'header
        ws.Cells(11, 1, 11, DoubleNormDataPrint.Columns.Count).Style.Font.Bold = True
        ws.Cells(11, 1, 11, DoubleNormDataPrint.Columns.Count).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        ws.Cells(11, 1, 11, DoubleNormDataPrint.Columns.Count).Style.WrapText() = True
        ws.Cells(11, 1, 11, DoubleNormDataPrint.Columns.Count).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Bottom
        ws.Cells(11, 1, 11, DoubleNormDataPrint.Columns.Count).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws.Cells(11, 1, 11, DoubleNormColumnsCounter).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 155, 187, 89))

        'borders
        ws.Cells(11, 1, DoubleNormRowsCounter + 11, DoubleNormColumnsCounter).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(11, 1, DoubleNormRowsCounter + 11, DoubleNormColumnsCounter).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(11, 1, DoubleNormRowsCounter + 11, DoubleNormColumnsCounter).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(11, 1, DoubleNormRowsCounter + 11, DoubleNormColumnsCounter).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(11, 1, 11, DoubleNormColumnsCounter).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)

        ws.Cells.AutoFitColumns(15)
        ws.Column(1).Width = 15


        ''
        ''2nd Excel Sheet for Full Scale Normalization is created here
        ws2 = pck.Workbook.Worksheets.Add("Full Scale Normalization")
        ws2.Cells.AutoFitColumns()
        ws2.Cells.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws2.Cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White)

        'add logo
        Dim logo2 As System.Drawing.Image
        logo2 = System.Drawing.Image.FromFile(Server.MapPath("img/easyFRAP-logo-Web-JPG.jpg"))
        Dim pic2 = ws2.Drawings.AddPicture("logo2", logo2)
        pic2.From.Column = 0
        pic2.From.Row = 0
        pic2.SetSize(11)

        'add info rows
        ws2.Cells("A4").Value = "Date: " & Now.ToShortDateString
        ws2.Cells("A5").Value = "Report: Normalized Data using Full Scale Method"
        ws2.Cells("A6").Value = "Prebleach Values: " & nPreBleachValues
        ws2.Cells("A7").Value = "Bleach Values: " & nBleachValues
        ws2.Cells("A8").Value = "Postbleach Values: " & nPostBleachValues
        ws2.Cells("A9").Value = "Initial Deleted Values: " & nInitialValues
        ws2.Cells("A4:A5").Style.Font.Bold = True
        ws2.Cells("A11").LoadFromDataTable(FullScaleNormDataPrint, True)

        'header
        ws2.Cells(11, 1, 11, FullScaleNormDataPrint.Columns.Count).Style.Font.Bold = True
        ws2.Cells(11, 1, 11, FullScaleNormDataPrint.Columns.Count).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        ws2.Cells(11, 1, 11, FullScaleNormDataPrint.Columns.Count).Style.WrapText() = True
        ws2.Cells(11, 1, 11, FullScaleNormDataPrint.Columns.Count).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Bottom
        ws2.Cells(11, 1, 11, FullScaleNormDataPrint.Columns.Count).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws2.Cells(11, 1, 11, FullScaleNormColumnsCounter).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 155, 187, 89))

        'borders
        ws2.Cells(11, 1, FullScaleNormRowsCounter + 11, FullScaleNormColumnsCounter).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws2.Cells(11, 1, FullScaleNormRowsCounter + 11, FullScaleNormColumnsCounter).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
        ws2.Cells(11, 1, FullScaleNormRowsCounter + 11, FullScaleNormColumnsCounter).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
        ws2.Cells(11, 1, FullScaleNormRowsCounter + 11, FullScaleNormColumnsCounter).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
        ws2.Cells(11, 1, 11, FullScaleNormColumnsCounter).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)

        ws2.Cells.AutoFitColumns(15)
        ws2.Column(1).Width = 15


        Try
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment; filename=" & sReportName & ".xlsx")
            Response.BinaryWrite(pck.GetAsByteArray())
            'Response.Flush()
            'Response.End()
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            LogToFile("ExportNormData() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode030)');", True)
            Return
        End Try

    End Sub

    Private Sub ExportROIData()
        Dim ROI1DataPrint As DataTable = New DataTable("proc_ExportDataForROI1")
        Dim ROI2DataPrint As DataTable = New DataTable("proc_ExportDataForROI2")
        Dim ROI3DataPrint As DataTable = New DataTable("proc_ExportDataForROI3")
        Dim sReportName As String = "easyFRAP_ROI_Data"
        Dim ROI1ColumnsCounter, ROI1RowsCounter, ROI2ColumnsCounter, ROI2RowsCounter, ROI3ColumnsCounter, ROI3RowsCounter As Integer
        Dim nGuid As String = String.Empty
        Dim dsData As New DataSet
        Dim sProcName, sProcName2, sProcName3 As String
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim pck As New ExcelPackage
        Dim ws, ws2, ws3 As ExcelWorksheet

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500
        sProcName = "proc_ExportDataForROI1"
        sProcName2 = "proc_ExportDataForROI2"
        sProcName3 = "proc_ExportDataForROI3"

        ''CREATE A DATASET FOR ROI 1 DATA
        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = sProcName
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", nGuid)
            m_cnSQL.Open()
            Dim numofrows As Integer = m_daSQL.Fill(dsData, sProcName)

            If numofrows = 0 Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowWarningMessage('The dataset is empty. Please select at least one file to export.');", True)
                Return
            End If

            'Convert DataSet to DataTable 
            ROI1DataPrint = dsData.Tables(0)
            ROI1ColumnsCounter = dsData.Tables(0).Columns.Count
            ROI1RowsCounter = dsData.Tables(0).Rows.Count

        Catch ex As Exception
            LogToFile("ExportROIData() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode024)');", True)
            Return
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        ''CREATE A DATASET FOR ROI 2 DATA
        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = sProcName2
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", nGuid)
            m_cnSQL.Open()
            Dim numofrows As Integer = m_daSQL.Fill(dsData, sProcName2)

            If numofrows = 0 Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowWarningMessage('The dataset is empty. Please select at least one file to export.');", True)
                Return
            End If

            'Convert DataSet to DataTable 
            ROI2DataPrint = dsData.Tables(1)
            ROI2ColumnsCounter = dsData.Tables(1).Columns.Count
            ROI2RowsCounter = dsData.Tables(1).Rows.Count

        Catch ex As Exception
            LogToFile("ExportROIData() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode024)');", True)
            Return
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        ''CREATE A DATASET FOR ROI 3 DATA
        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = sProcName3
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", nGuid)
            m_cnSQL.Open()
            Dim numofrows As Integer = m_daSQL.Fill(dsData, sProcName3)

            If numofrows = 0 Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowWarningMessage('The dataset is empty. Please select at least one file to export.');", True)
                Return
            End If

            'Convert DataSet to DataTable 
            ROI3DataPrint = dsData.Tables(2)
            ROI3ColumnsCounter = dsData.Tables(2).Columns.Count
            ROI3RowsCounter = dsData.Tables(2).Rows.Count

        Catch ex As Exception
            LogToFile("ExportROIData() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode024)');", True)
            Return
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try


        ''
        ''1st Excel Sheet for ROI 1 is created here
        ws = pck.Workbook.Worksheets.Add("ROI 1 Report")
        ws.Cells.AutoFitColumns()
        ws.Cells.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws.Cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White)

        'add logo
        Dim logo As System.Drawing.Image
        logo = System.Drawing.Image.FromFile(Server.MapPath("img/easyFRAP-logo-Web-JPG.jpg"))
        Dim pic = ws.Drawings.AddPicture("logo", logo)
        pic.From.Column = 0
        pic.From.Row = 0
        pic.SetSize(11)

        'add info rows
        ws.Cells("A4").Value = "Date: " & Now.ToShortDateString
        ws.Cells("A5").Value = "Report: Raw Data - Region of Interest 1"
        ws.Cells("A4:A5").Style.Font.Bold = True
        ws.Cells("A7").LoadFromDataTable(ROI1DataPrint, True)

        'header
        ws.Cells(7, 1, 7, ROI1DataPrint.Columns.Count).Style.Font.Bold = True
        ws.Cells(7, 1, 7, ROI1DataPrint.Columns.Count).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        ws.Cells(7, 1, 7, ROI1DataPrint.Columns.Count).Style.WrapText() = True
        ws.Cells(7, 1, 7, ROI1DataPrint.Columns.Count).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Bottom
        ws.Cells(7, 1, 7, ROI1DataPrint.Columns.Count).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws.Cells(7, 1, 7, ROI1ColumnsCounter).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 155, 187, 89))

        'borders
        ws.Cells(7, 1, ROI1RowsCounter + 7, ROI1ColumnsCounter).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(7, 1, ROI1RowsCounter + 7, ROI1ColumnsCounter).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(7, 1, ROI1RowsCounter + 7, ROI1ColumnsCounter).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(7, 1, ROI1RowsCounter + 7, ROI1ColumnsCounter).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(7, 1, 7, ROI1ColumnsCounter).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)

        ws.Cells.AutoFitColumns(15)
        ws.Column(1).Width = 15

        ''
        ''2nd Excel Sheet for ROI 2 is created here
        ws2 = pck.Workbook.Worksheets.Add("ROI 2 Report")
        ws2.Cells.AutoFitColumns()
        ws2.Cells.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws2.Cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White)

        'add logo
        Dim logo2 As System.Drawing.Image
        logo2 = System.Drawing.Image.FromFile(Server.MapPath("img/easyFRAP-logo-Web-JPG.jpg"))
        Dim pic2 = ws2.Drawings.AddPicture("logo", logo2)
        pic2.From.Column = 0
        pic2.From.Row = 0
        pic2.SetSize(11)

        'add info rows
        ws2.Cells("A4").Value = "Date: " & Now.ToShortDateString
        ws2.Cells("A5").Value = "Report: Raw Data - Region of Interest 2"
        ws2.Cells("A4:A5").Style.Font.Bold = True
        ws2.Cells("A7").LoadFromDataTable(ROI2DataPrint, True)

        'header
        ws2.Cells(7, 1, 7, ROI2DataPrint.Columns.Count).Style.Font.Bold = True
        ws2.Cells(7, 1, 7, ROI2DataPrint.Columns.Count).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        ws2.Cells(7, 1, 7, ROI2DataPrint.Columns.Count).Style.WrapText() = True
        ws2.Cells(7, 1, 7, ROI2DataPrint.Columns.Count).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Bottom
        ws2.Cells(7, 1, 7, ROI2DataPrint.Columns.Count).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws2.Cells(7, 1, 7, ROI2ColumnsCounter).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 155, 187, 89))

        'borders
        ws2.Cells(7, 1, ROI2RowsCounter + 7, ROI2ColumnsCounter).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws2.Cells(7, 1, ROI2RowsCounter + 7, ROI2ColumnsCounter).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
        ws2.Cells(7, 1, ROI2RowsCounter + 7, ROI2ColumnsCounter).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
        ws2.Cells(7, 1, ROI2RowsCounter + 7, ROI2ColumnsCounter).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
        ws2.Cells(7, 1, 7, ROI2ColumnsCounter).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)

        ws2.Cells.AutoFitColumns(15)
        ws2.Column(1).Width = 15

        ''
        ''3rd Excel Sheet for ROI 3 is created here
        ws3 = pck.Workbook.Worksheets.Add("ROI 3 Report")
        ws3.Cells.AutoFitColumns()
        ws3.Cells.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws3.Cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White)

        'add logo
        Dim logo3 As System.Drawing.Image
        logo3 = System.Drawing.Image.FromFile(Server.MapPath("img/easyFRAP-logo-Web-JPG.jpg"))
        Dim pic3 = ws3.Drawings.AddPicture("logo", logo3)
        pic3.From.Column = 0
        pic3.From.Row = 0
        pic3.SetSize(11)

        'add info rows
        ws3.Cells("A4").Value = "Date: " & Now.ToShortDateString
        ws3.Cells("A5").Value = "Report: Raw Data - Region of Interest 3"
        ws3.Cells("A4:A5").Style.Font.Bold = True
        ws3.Cells("A7").LoadFromDataTable(ROI3DataPrint, True)

        'header
        ws3.Cells(7, 1, 7, ROI3DataPrint.Columns.Count).Style.Font.Bold = True
        ws3.Cells(7, 1, 7, ROI3DataPrint.Columns.Count).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
        ws3.Cells(7, 1, 7, ROI3DataPrint.Columns.Count).Style.WrapText() = True
        ws3.Cells(7, 1, 7, ROI3DataPrint.Columns.Count).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Bottom
        ws3.Cells(7, 1, 7, ROI3DataPrint.Columns.Count).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        ws3.Cells(7, 1, 7, ROI3ColumnsCounter).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 155, 187, 89))

        'borders
        ws3.Cells(7, 1, ROI3RowsCounter + 7, ROI3ColumnsCounter).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws3.Cells(7, 1, ROI3RowsCounter + 7, ROI3ColumnsCounter).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
        ws3.Cells(7, 1, ROI3RowsCounter + 7, ROI3ColumnsCounter).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
        ws3.Cells(7, 1, ROI3RowsCounter + 7, ROI3ColumnsCounter).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
        ws3.Cells(7, 1, 7, ROI3ColumnsCounter).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)

        ws3.Cells.AutoFitColumns(15)
        ws3.Column(1).Width = 15

        Try
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment; filename=" & sReportName & ".xlsx")
            Response.BinaryWrite(pck.GetAsByteArray())
            'Response.Flush()
            'Response.End()
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            LogToFile("ExportROIData() - " + sProcName & "( " & nGuid & ")", ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode028)');", True)
            Return
        End Try

    End Sub
    Private Sub EstimateMobFractionThalfRsquareDrawCurves(Optional ByVal ButtonPressed As String = "", Optional ByVal nRawFileID As String = "")
        Dim nGuid As String = String.Empty
        Dim nNormalizationMethod As String = String.Empty
        Dim nExponentialEquation As String = String.Empty
        Dim sProcName As String = String.Empty
        Dim PreBleachValues As String = String.Empty
        Dim BleachValues As String = String.Empty
        Dim InitialValues As String = String.Empty
        Dim mobile_fraction As String = String.Empty
        Dim t_half As String = String.Empty
        Dim r_square As String = String.Empty
        Dim SuccessEstimationFlag As Boolean = False
        Dim sSQL As String = String.Empty
        Dim dsData As New DataSet
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        nNormalizationMethod = NormalizationRadioButtonList.SelectedItem.Value.ToString
        nExponentialEquation = CurveFittingRadioButtonList.SelectedItem.Value.ToString
        PreBleachValues = PreBleachValuestxtbox.Text
        BleachValues = BleachValuestxtbox.Text
        InitialValues = DiscardValuestxtbox.Text

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand

        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500


        If (ButtonPressed = "FitAllCurve_Click") Then
            sProcName = "proc_ReturnParametersForCurveFittingGraphsMean"
            Try
                m_cmSQL.CommandType = CommandType.StoredProcedure
                m_cmSQL.CommandText = sProcName


                m_cmSQL.Parameters.Clear()
                m_cmSQL.Parameters.AddWithValue("@sGUID", nGuid)
                m_cmSQL.Parameters.AddWithValue("@sPREBLEACHVALUES", PreBleachValues)
                m_cmSQL.Parameters.AddWithValue("@sBLEACHVALUES", BleachValues)
                m_cmSQL.Parameters.AddWithValue("@sINITIALBLEACHVALUES", InitialValues)
                m_cmSQL.Parameters.AddWithValue("@sNORMALIZATIONMETHOD", nNormalizationMethod)
                m_cmSQL.Parameters.AddWithValue("@sFITTINGMETHOD", nExponentialEquation)

                m_cnSQL.Open()
                m_daSQL.Fill(dsData, sProcName)

                If (dsData.Tables.Count > 0) Then
                    'DrawCurveFittingDots(dsData.Tables(0))
                    PublishCurveFittingData(nGuid, PreBleachValues, BleachValues, InitialValues, nNormalizationMethod, nExponentialEquation, "proc_ReturnParametersForCurveFittingGraphsMean")
                Else
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode040)');", True)
                    Return
                End If
            Catch ex As Exception
                LogToFile("EstimateMobFractionThalfRsquareDrawCurves() - " + sProcName, ex.Message)
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode038)');", True)
                Return
            End Try

        ElseIf (ButtonPressed = "FitSelectedCurve_Click") Then

            sProcName = "proc_ReturnParametersForCurveFittingGraphs"
            Try
                m_cmSQL.CommandType = CommandType.StoredProcedure
                m_cmSQL.CommandText = sProcName


                m_cmSQL.Parameters.Clear()
                m_cmSQL.Parameters.AddWithValue("@sRAWFILEID", nRawFileID)
                m_cmSQL.Parameters.AddWithValue("@sPREBLEACHVALUES", PreBleachValues)
                m_cmSQL.Parameters.AddWithValue("@sBLEACHVALUES", BleachValues)
                m_cmSQL.Parameters.AddWithValue("@sINITIALBLEACHVALUES", InitialValues)
                m_cmSQL.Parameters.AddWithValue("@sNORMALIZATIONMETHOD", nNormalizationMethod)
                m_cmSQL.Parameters.AddWithValue("@sFITTINGMETHOD", nExponentialEquation)

                m_cnSQL.Open()
                m_daSQL.Fill(dsData, sProcName)

                If (dsData.Tables.Count > 0) Then
                    'DrawCurveFittingDots(dsData.Tables(0))
                    PublishCurveFittingData(nRawFileID, PreBleachValues, BleachValues, InitialValues, nNormalizationMethod, nExponentialEquation, "proc_ReturnParametersForCurveFittingGraphs")
                Else
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode039)');", True)
                    Return
                End If
            Catch ex As Exception
                LogToFile("EstimateMobFractionThalfRsquareDrawCurves() - " + sProcName, ex.Message)
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your data. Please try again. - (ErrorCode035)');", True)
                Return
            End Try
        End If


        If ((dsData.Tables(0).Rows(0)("mobile_fraction") <> "") And (dsData.Tables(0).Rows(0)("t_half") <> "") And (dsData.Tables(0).Rows(0)("r_square") <> "")) Then
            sMobileFractionScore = dsData.Tables(0).Rows(0)("mobile_fraction") 'mobile_fraction
            sThalfScore = dsData.Tables(0).Rows(0)("t_half") 't_half
            sRsquareScore = dsData.Tables(0).Rows(0)("r_square") 'r_square
            SuccessEstimationFlag = True
        Else
            sMobileFractionScore = "N/A"
            sThalfScore = "N/A"
            sRsquareScore = "N/A"
            SuccessEstimationFlag = False
        End If


        If (((SuccessEstimationFlag = True) And (ButtonPressed = "FitSelectedCurve_Click")) Or ((SuccessEstimationFlag = True) And (ButtonPressed = "FitAllCurve_Click"))) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowSuccessMessage('The Mobile Fraction, the T-half and the R-square were successfully calculated.');", True)
        End If

    End Sub
    Private Sub DrawNormalizationGraphs(Optional ByVal NormType As String = "")
        Dim PreBleachValues As String = String.Empty
        Dim BleachValues As String = String.Empty
        Dim IntPreBleachValues As Integer = 0
        Dim InitialValues As String = String.Empty
        Dim nGuid As String = String.Empty

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        PreBleachValues = PreBleachValuestxtbox.Text
        BleachValues = BleachValuestxtbox.Text

        If (DiscardValuestxtbox.Text <> "") Then
            InitialValues = DiscardValuestxtbox.Text
        Else
            InitialValues = "0"
        End If

        'If is postback from another button (not the normalization btn)
        If (NormalizationRadioButtonList.SelectedItem) IsNot Nothing Then
            If ((NormType = "") And (NormalizationRadioButtonList.SelectedItem.Value = "Double")) Then
                NormType = "Double"
                sNormType = "Double"
            ElseIf ((NormType = "") And (NormalizationRadioButtonList.SelectedItem.Value = "FullScale")) Then
                NormType = "FullScale"
                sNormType = "FullScale"
            End If
        Else
            NormType = ""
            sNormType = ""
        End If


        If NormType = "Double" Then
            sDataForNormalizationGraph = ReturnNormalizationData(nGuid, PreBleachValues, BleachValues, InitialValues, "proc_ReturnDataForDoubleNormGraphs")
            sLabelsForNormalizationGraph = ReturnNormalizationLabels(nGuid, PreBleachValues, BleachValues, InitialValues, "proc_ReturnDataForDoubleNormGraphs")
            StandardDevDoubleNormDataTable = GetDataForStandardDevDoubleNormGraph(nGuid, PreBleachValues, InitialValues)
            DrawStandardDevForDoubleNormGraph(StandardDevDoubleNormDataTable)
        ElseIf NormType = "FullScale" Then
            sDataForNormalizationGraph = ReturnNormalizationData(nGuid, PreBleachValues, BleachValues, InitialValues, "proc_ReturnDataForFullScaleNormGraphs")
            sLabelsForNormalizationGraph = ReturnNormalizationLabels(nGuid, PreBleachValues, BleachValues, InitialValues, "proc_ReturnDataForFullScaleNormGraphs")
            StandardDevFullScaleNormDataTable = GetDataForStandardDevFullScaleNormGraph(nGuid, PreBleachValues, BleachValues, InitialValues)
            DrawStandardDevForFullScaleNormGraph(StandardDevFullScaleNormDataTable)
        ElseIf NormType = "" Then
            Return
        End If

    End Sub
    Private Function ReturnROIData(ByVal GUID As String, ByVal procname As String) As String
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim dsData As New DataSet

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = procname
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", GUID)
            m_cnSQL.Open()
            m_daSQL.Fill(dsData, procname)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('" + ex.Message + "');", True)
            Return 0
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        Dim dict As New Dictionary(Of String, Object)

        For Each dt As DataTable In dsData.Tables
            Dim arr(dt.Rows.Count) As Object

            For i As Integer = 0 To dt.Rows.Count - 1
                arr(i) = dt.Rows(i).ItemArray
            Next

            dict.Add(dt.TableName, arr)
        Next

        Dim js As JavaScriptSerializer = New JavaScriptSerializer()
        js.MaxJsonLength = Int32.MaxValue
        Dim sJSON As String = js.Serialize(dict)
        If sJSON.Length > 2 Then
            sJSON = sJSON.Replace("""", "")
            sJSON = sJSON.Substring(30)
            sJSON = sJSON.Substring(0, sJSON.Length - 7)
            sJSON = sJSON + "]"
            'LogGraphPointsToFile(sJSON)
            Return sJSON
        Else
            Return "[ , ]"
        End If
    End Function

    Private Function ReturnLabels(ByVal GUID As String, ByVal procname As String) As String

        Dim sLabelsForROIGraph As String = ""
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim dsData As New DataSet

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = procname
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", GUID)
            m_cnSQL.Open()
            m_daSQL.Fill(dsData, procname)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('" + ex.Message + "');", True)
            Return 0
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        If IsNothing(dsData.Tables) Then
            Return "[ , ]"
        End If

        'For Labels of ROI
        For Each dt As DataTable In dsData.Tables
            For Each column As DataColumn In dt.Columns
                sLabelsForROIGraph = sLabelsForROIGraph + ControlChars.Quote + column.ColumnName.ToString + ControlChars.Quote + ", "
            Next
            sLabelsForROIGraph = sLabelsForROIGraph.Remove(sLabelsForROIGraph.Length - 2)
            sLabelsForROIGraph = "[ " + sLabelsForROIGraph + " ]"
        Next
        'LogGraphPointsToFile(sLabelsForROIGraph)
        Return sLabelsForROIGraph

    End Function

    Private Function ReturnNormalizationData(ByVal GUID As String, ByVal PreBleachValues As String, ByVal BleachValues As String, ByVal InitialValues As String, ByVal procname As String) As String
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim dsData As New DataSet

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = procname
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", GUID)
            m_cmSQL.Parameters.AddWithValue("@PREBLEACHVALUES", PreBleachValues)
            If procname = "proc_ReturnDataForFullScaleNormGraphs" Then
                m_cmSQL.Parameters.AddWithValue("@BLEACHVALUES", BleachValues)
            End If
            m_cmSQL.Parameters.AddWithValue("@INITIALBLEACHVALUES", InitialValues)
            m_cnSQL.Open()
            m_daSQL.Fill(dsData, procname)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('" + ex.Message + "');", True)
            Return 0
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        Dim dict As New Dictionary(Of String, Object)

        For Each dt As DataTable In dsData.Tables
            Dim arr(dt.Rows.Count) As Object

            For i As Integer = 0 To dt.Rows.Count - 1
                arr(i) = dt.Rows(i).ItemArray
            Next

            dict.Add(dt.TableName, arr)
        Next

        Dim js As JavaScriptSerializer = New JavaScriptSerializer()
        js.MaxJsonLength = Int32.MaxValue
        Dim sJSON As String = js.Serialize(dict)
        If sJSON.Length > 2 Then
            sJSON = sJSON.Replace("""", "")
            If procname = "proc_ReturnDataForDoubleNormGraphs" Then
                sJSON = sJSON.Substring(36)
            ElseIf procname = "proc_ReturnDataForFullScaleNormGraphs" Then
                sJSON = sJSON.Substring(39)
            End If
            sJSON = sJSON.Substring(0, sJSON.Length - 7)
            sJSON = sJSON + "]"
            'LogGraphPointsToFile(sJSON)
            Return sJSON
        Else
            Return "[ , ]"
        End If
    End Function

    Private Function ReturnNormalizationLabels(ByVal GUID As String, ByVal PreBleachValues As String, ByVal BleachValues As String, ByVal InitialValues As String, ByVal procname As String) As String

        Dim sLabelsForNormalizationGraph As String = ""
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim dsData As New DataSet

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = procname
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUID", GUID)
            m_cmSQL.Parameters.AddWithValue("@PREBLEACHVALUES", PreBleachValues)
            If procname = "proc_ReturnDataForFullScaleNormGraphs" Then
                m_cmSQL.Parameters.AddWithValue("@BLEACHVALUES", BleachValues)
            End If
            m_cmSQL.Parameters.AddWithValue("@INITIALBLEACHVALUES", InitialValues)
            m_cnSQL.Open()
            m_daSQL.Fill(dsData, procname)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('" + ex.Message + "');", True)
            Return 0
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        If IsNothing(dsData.Tables) Then
            Return "[ , ]"
        End If

        'For Labels of Normalization Data
        For Each dt As DataTable In dsData.Tables
            For Each column As DataColumn In dt.Columns
                sLabelsForNormalizationGraph = sLabelsForNormalizationGraph + ControlChars.Quote + column.ColumnName.ToString + ControlChars.Quote + ", "
            Next
            sLabelsForNormalizationGraph = sLabelsForNormalizationGraph.Remove(sLabelsForNormalizationGraph.Length - 2)
            sLabelsForNormalizationGraph = "[ " + sLabelsForNormalizationGraph + " ]"
        Next
        'LogGraphPointsToFile(sLabelsForROIGraph)
        Return sLabelsForNormalizationGraph

    End Function
    Private Sub PublishCurveFittingData(ByVal index As String, ByVal PreBleachValues As String, ByVal BleachValues As String, ByVal InitialValues As String, ByVal NormalizationMethod As String, ByVal ExponentialEquation As String, ByVal procname As String)
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim dsData As New DataSet

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = procname
            m_cmSQL.Parameters.Clear()
            If procname = "proc_ReturnParametersForCurveFittingGraphs" Then
                m_cmSQL.Parameters.AddWithValue("@sRAWFILEID", index)
            ElseIf procname = "proc_ReturnParametersForCurveFittingGraphsMean" Then
                m_cmSQL.Parameters.AddWithValue("@sGUID", index)
            End If
            m_cmSQL.Parameters.AddWithValue("@sPREBLEACHVALUES", PreBleachValues)
            m_cmSQL.Parameters.AddWithValue("@sBLEACHVALUES", BleachValues)
            m_cmSQL.Parameters.AddWithValue("@sINITIALBLEACHVALUES", InitialValues)
            m_cmSQL.Parameters.AddWithValue("@sNORMALIZATIONMETHOD", NormalizationMethod)
            m_cmSQL.Parameters.AddWithValue("@sFITTINGMETHOD", ExponentialEquation)
            m_cnSQL.Open()
            m_daSQL.Fill(dsData, procname)

        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('" + ex.Message + "');", True)
            'Return
            LogToFile("PublishCurveFittingData() - " + procname, ex.Message)
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        If IsNothing(dsData.Tables) Then
            sDataForCurveFittingDots = "[ , ]"
            sDataForResiduals = "[ , ]"
            sLabelsForCurveFittingDots = "[ , ]"
            sLabelsForCurveFittingDots = "[ , ]"
            Return
        End If

        Dim dict As New Dictionary(Of String, Object)
        Dim dict2 As New Dictionary(Of String, Object)
        For Each dt As DataTable In dsData.Tables
            Dim arr(dt.Rows.Count) As Object
            Dim arr2(dt.Rows.Count) As Object

            For i As Integer = 0 To dt.Rows.Count - 1
                'For Curve Fitting Data
                arr(i) = "[" + dt.Rows(i).ItemArray(0) + "," + dt.Rows(i).ItemArray(1) + "," + dt.Rows(i).ItemArray(2) + "]"
                'For Residuals Data
                arr2(i) = "[" + dt.Rows(i).ItemArray(0) + "," + dt.Rows(i).ItemArray(3) + "]"
            Next

            dict.Add(dt.TableName, arr)
            dict2.Add(dt.TableName, arr2)
        Next

        Dim js As JavaScriptSerializer = New JavaScriptSerializer()
        js.MaxJsonLength = Int32.MaxValue
        sDataForCurveFittingDots = js.Serialize(dict)
        sDataForResiduals = js.Serialize(dict2)

        If (sDataForCurveFittingDots.Length > 2) And (sDataForResiduals.Length > 2) Then
            sDataForCurveFittingDots = sDataForCurveFittingDots.Replace("""", "")
            sDataForResiduals = sDataForResiduals.Replace("""", "")
            If procname = "proc_ReturnParametersForCurveFittingGraphs" Then
                sDataForCurveFittingDots = sDataForCurveFittingDots.Substring(44)
                sDataForResiduals = sDataForResiduals.Substring(44)
            ElseIf procname = "proc_ReturnParametersForCurveFittingGraphsMean" Then
                sDataForCurveFittingDots = sDataForCurveFittingDots.Substring(48)
                sDataForResiduals = sDataForResiduals.Substring(48)
            End If
            sDataForCurveFittingDots = sDataForCurveFittingDots.Substring(0, sDataForCurveFittingDots.Length - 7)
            sDataForResiduals = sDataForResiduals.Substring(0, sDataForResiduals.Length - 7)
            sDataForCurveFittingDots = sDataForCurveFittingDots + "]"
            sDataForResiduals = sDataForResiduals + "]"
            'LogGraphPointsToFile(sJSON)
        Else
            sDataForCurveFittingDots = "[ , ]"
            sDataForResiduals = "[ , ]"
        End If

        For Each dt As DataTable In dsData.Tables
            For Each column As DataColumn In dt.Columns
                If (column.ColumnName = "x") Or (column.ColumnName = "actual point") Or (column.ColumnName = "theoritical point") Then
                    sLabelsForCurveFittingDots = sLabelsForCurveFittingDots + ControlChars.Quote + column.ColumnName.ToString + ControlChars.Quote + ", "
                End If
                If (column.ColumnName = "x") Or (column.ColumnName = "residuals") Then
                    sLabelsForResiduals = sLabelsForResiduals + ControlChars.Quote + column.ColumnName.ToString + ControlChars.Quote + ", "
                End If
            Next
            sLabelsForCurveFittingDots = sLabelsForCurveFittingDots.Remove(sLabelsForCurveFittingDots.Length - 2)
            sLabelsForCurveFittingDots = "[ " + sLabelsForCurveFittingDots + " ]"
            sLabelsForResiduals = sLabelsForResiduals.Remove(sLabelsForResiduals.Length - 2)
            sLabelsForResiduals = "[ " + sLabelsForResiduals + " ]"
        Next

    End Sub
    Private Sub SaveToExcel()
        Dim dt1, dt2, dt3 As New DataTable()
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim ds As New DataSet()
        Dim sProcName As String = String.Empty
        Dim NormalizationMethod As String = String.Empty
        Dim ExponentialEquation As String = String.Empty
        Dim PreBleachValues As String = String.Empty
        Dim BleachValues As String = String.Empty
        Dim PostBleachValues As String = String.Empty
        Dim InitialValues As String = String.Empty
        Dim sReportName As String = "easyFRAP_Final_Data"
        Dim pck As New ExcelPackage
        Dim ws As ExcelWorksheet
        Dim nGuid = String.Empty

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        NormalizationMethod = NormalizationRadioButtonList.SelectedItem.Value.ToString
        ExponentialEquation = CurveFittingRadioButtonList.SelectedItem.Value.ToString
        PreBleachValues = PreBleachValuestxtbox.Text
        BleachValues = BleachValuestxtbox.Text
        PostBleachValues = PostBleachValuestxtbox.Text
        If (DiscardValuestxtbox.Text <> "") Then
            InitialValues = DiscardValuestxtbox.Text
        Else
            InitialValues = "0"
        End If

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500

        Try
            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = "proc_SaveToExcel"
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sGUIDID", nGuid)
            m_cnSQL.Open()
            Using m_drSQL As SqlDataReader = m_cmSQL.ExecuteReader()
                dt1.Load(m_drSQL)
                dt2.Load(m_drSQL)
                dt3.Load(m_drSQL)
            End Using
            ''1st Excel Sheet for Final Data is created here
            ws = pck.Workbook.Worksheets.Add("Final Data Report")


            'add logo
            Dim logo As System.Drawing.Image
            logo = System.Drawing.Image.FromFile(Server.MapPath("img/easyFRAP-logo-Web-JPG.jpg"))
            Dim pic = ws.Drawings.AddPicture("logo", logo)
            pic.From.Column = 0
            pic.From.Row = 0
            pic.SetSize(11)

            'add info rows
            ws.Cells("A4").Value = "Date: " & Now.ToShortDateString
            ws.Cells("A5").Value = "Export: Mobile Fraction | T-half | R-square"
            ws.Cells("A6").Value = "Prebleach Values: " & PreBleachValues
            ws.Cells("A7").Value = "Bleach Values: " & BleachValues
            ws.Cells("A8").Value = "Postbleach Values: " & PostBleachValues
            ws.Cells("A9").Value = "Initial Deleted Values: " & InitialValues
            ws.Cells("A10").Value = "Normalization Method: " & NormalizationMethod
            ws.Cells("A11").Value = "Exponential Equation: " & ExponentialEquation
            ws.Cells("A4:A11").Style.Font.Bold = True
            ws.Cells("A13").LoadFromDataTable(dt1, True)
            ws.InsertColumn(3, 2)
            ws.Cells("C13").LoadFromDataTable(dt2, True)
            ws.InsertColumn(6, 2)
            ws.Cells("F13").LoadFromDataTable(dt3, True)


            ws.Cells.AutoFitColumns()
            ws.Cells.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            ws.Cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White)
            ws.Column(4).Width = 20
            ws.Column(7).Width = 20


            'header
            ws.Cells(13, 1, 13, dt1.Columns.Count + 4).Style.Font.Bold = True
            ws.Cells(12, 1, dt1.Rows.Count + 13, dt1.Columns.Count + 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
            ws.Cells(13, 1, 13, dt1.Columns.Count + 4).Style.WrapText() = True
            ws.Cells(13, 1, 13, dt1.Columns.Count + 4).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Bottom
            ws.Cells(13, 1, 13, dt1.Columns.Count + 4).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            ws.Cells(13, 1, 13, dt1.Columns.Count + 4).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 155, 187, 89))
            ws.Cells(12, 3).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0.81, 217, 219, 215))
            ws.Cells(12, 4).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0.81, 217, 219, 215))
            ws.Cells(12, 6).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0.81, 217, 219, 215))
            ws.Cells(12, 7).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0.81, 217, 219, 215))


            'borders
            ws.Cells(13, 1, dt1.Rows.Count + 13, dt1.Columns.Count + 4).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
            ws.Cells(13, 1, dt1.Rows.Count + 13, dt1.Columns.Count + 4).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
            ws.Cells(13, 1, dt1.Rows.Count + 13, dt1.Columns.Count + 4).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
            ws.Cells(13, 1, dt1.Rows.Count + 13, dt1.Columns.Count + 4).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
            ws.Cells(13, 1, 13, dt1.Columns.Count + 4).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)
            ws.Cells(12, 3).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)
            ws.Cells(12, 4).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)
            ws.Cells(12, 6).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)
            ws.Cells(12, 7).Style.Border.BorderAround(Style.ExcelBorderStyle.Thick)

            ws.Cells(13, 2, 13, 4).Merge() = True
            ws.Cells(13, 5, 13, 7).Merge() = True
            ws.Cells("B13").Value = "T-half"
            ws.Cells("E13").Value = "Mobile Fraction"
            ws.Cells("H13").Value = "R square"

            ws.Cells("C12").Value = "Mean"
            ws.Cells("D12").Value = "Standard Deviation"

            ws.Cells("F12").Value = "Mean"
            ws.Cells("G12").Value = "Standard Deviation"

        Catch ex As Exception
            LogToFile("SaveToExcel() - " + sProcName, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode043)');", True)
            Return
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowSuccessMessage('The file has been downloaded successfully.');", True)
        End Try

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.ClearHeaders()
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & sReportName & ".xlsx")
        HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray())
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.SuppressContent = True
        HttpContext.Current.ApplicationInstance.CompleteRequest()

    End Sub
    Private Sub ExportFinalData()

        Dim RF_IDs As String = String.Empty
        Dim counter As Integer = 0
        Dim dt1, dt2, dt3 As New DataTable()
        Dim ds As New DataSet()
        Dim sProcName As String = String.Empty
        Dim NormalizationMethod As String = String.Empty
        Dim ExponentialEquation As String = String.Empty
        Dim PreBleachValues As String = String.Empty
        Dim BleachValues As String = String.Empty
        Dim PostBleachValues As String = String.Empty
        Dim InitialValues As String = String.Empty
        Dim return_message As String = String.Empty
        Dim nGuid As String = String.Empty

        If IsNothing(Session("GUID")) Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Your session has expired. Please restart your analysis by uploading your files again.');", True)
            Initialization()
            Return
        Else
            nGuid = Session("GUID").ToString
        End If

        NormalizationMethod = NormalizationRadioButtonList.SelectedItem.Value.ToString
        ExponentialEquation = CurveFittingRadioButtonList.SelectedItem.Value.ToString
        PreBleachValues = PreBleachValuestxtbox.Text
        BleachValues = BleachValuestxtbox.Text
        PostBleachValues = PostBleachValuestxtbox.Text
        If (DiscardValuestxtbox.Text <> "") Then
            InitialValues = DiscardValuestxtbox.Text
        Else
            InitialValues = "0"
        End If

        'Try
        For Each row As GridViewRow In grdExportFinalData.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRow"), CheckBox)
                Dim lbl As Label = TryCast(row.Cells(0).FindControl("lblID"), Label)
                If chkRow.Checked Then
                    RF_IDs = RF_IDs + "," + lbl.Text
                End If
            End If
        Next
        'Remove the first comma (delimiter)
        RF_IDs = RF_IDs.Remove(0, 1)
        'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowSuccessMessage('" + RF_IDs + "');", True)

        sProcName = "proc_ExportFinalData"

        Try
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
                Using comm As SqlCommand = New SqlCommand
                    With comm
                        .Connection = conn
                        .CommandTimeout = 3500
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = sProcName
                        .Parameters.AddWithValue("@sGUIDID", nGuid)
                        .Parameters.AddWithValue("@sRAWFILEID", RF_IDs)
                        .Parameters.AddWithValue("@sPREBLEACHVALUES", PreBleachValues)
                        .Parameters.AddWithValue("@sBLEACHVALUES", BleachValues)
                        .Parameters.AddWithValue("@sINITIALBLEACHVALUES", InitialValues)
                        .Parameters.AddWithValue("@sNORMALIZATIONMETHOD", NormalizationMethod)
                        .Parameters.AddWithValue("@sFITTINGMETHOD", ExponentialEquation)
                    End With
                    conn.Open()
                    Using dr As IDataReader = comm.ExecuteReader()
                        If dr.Read() Then
                            return_message = dr("strMessage").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogToFile("ExportFinalData() - " + sProcName, ex.Message)
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('Something went wrong while processing your request. Please try again. - (ErrorCode041)');", True)
            Return
        End Try

        If return_message = "Success" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowPopUpForDownload();", True)
            Return
        ElseIf return_message = "Something went wrong" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowErrorMessage('A problem occured while downloading your results set. Please try again.');", True)
            Return
        End If

    End Sub

    Private Sub Initialization()
        HideSections()
        ResetButtonClicked()
        ClearBleachingFieldSetValues()
        ClearNormalizationRadioButtons()
        ClearCurveFittingRadioButtons()
    End Sub

    Private Sub HideSections()
        HideFileNameTable()
        HideRawDataVisualization()
        HideBleachingParameters()
        HideNormalization()
        HideCurveFitting()
        HideDeleteSessionButton()
        HideSaveResultsSession()
    End Sub

    Private Sub ShowSections()
        ShowRawDataVisualization()
        ShowBleachingParameters()
        ShowNormalization()
        ShowCurveFitting()
        ShowDeleteSessionButton()
        ShowSaveResultsSession()
    End Sub

    Private Sub ResetBD_GP_Parameters()
        PreBleachValuestxtbox.Text = ""
        BleachValuestxtbox.Text = ""
        PostBleachValuestxtbox.Text = ""
        DiscardValuestxtbox.Text = ""
        sBleachingDepthScore = "N/A"
        sGapRatioScore = "N/A"
    End Sub

    Private Sub ClearNormalizationFieldset()
        NormalizationRadioButtonList.ClearSelection()
        sDataForNormalizationGraph = ""
        sLabelsForNormalizationGraph = ""
    End Sub

    Private Sub ClearCurveFittingFieldset()
        'CurveFittingRadioButtonList.ClearSelection()
        sDataForCurveGraph = ""
        sLabelsForCurveFittingDots = ""
    End Sub

    Private Sub GoToBD_GRAnchor()
        Page.ClientScript.RegisterStartupScript(Me.[GetType](),
                "anchor", "location.hash = '#bd_gr_position';", True)
    End Sub

    Private Sub GoToDataSetSelectionAnchor()
        Page.ClientScript.RegisterStartupScript(Me.[GetType](),
                "anchor", "location.hash = '#dataset_selection';", True)
    End Sub

    Private Sub GoToNormalizationAnchor()
        Page.ClientScript.RegisterStartupScript(Me.[GetType](),
            "anchor", "location.hash = '#normalization';", True)
    End Sub
    Private Sub GoToCurveFittingAnchor()
        Page.ClientScript.RegisterStartupScript(Me.[GetType](),
            "anchor", "location.hash = '#curvefitting';", True)
    End Sub

#End Region

End Class