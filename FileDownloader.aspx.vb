Imports System.Data
Imports System.Data.SqlClient
Imports OfficeOpenXml
Partial Class FileDownloader
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim RF_IDs As String = String.Empty
        Dim counter As Integer = 0
        Dim m_cnSQL As SqlConnection
        Dim m_daSQL As SqlDataAdapter
        Dim m_cmSQL As SqlCommand
        Dim dt1, dt2, dt3 As New DataTable()
        Dim ds As New DataSet()
        Dim sProcName As String = String.Empty
        Dim NormalizationMethod As String = String.Empty
        Dim ExponentialEquation As String = String.Empty
        Dim PreBleachValues As String = String.Empty
        Dim BleachValues As String = String.Empty
        Dim PostBleachValues As String = String.Empty
        Dim InitialValues As String = String.Empty
        Dim sDataForKernelDensityThalf As String = String.Empty
        Dim sReportName As String = "easyFRAP_Final_Data"
        Dim pck As New ExcelPackage
        Dim ws As ExcelWorksheet

        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand
        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500


        NormalizationMethod = context.Request.QueryString("NormalizationMethod")
        ExponentialEquation = context.Request.QueryString("ExponentialEquation")
        PreBleachValues = context.Request.QueryString("PreBleachValues")
        BleachValues = context.Request.QueryString("BleachValues")
        PostBleachValues = context.Request.QueryString("PostBleachValues")
        InitialValues = context.Request.QueryString("InitialValues")
        RF_IDs = context.Request.QueryString("RF_IDs")

        Try

            m_cmSQL.CommandType = CommandType.StoredProcedure
            m_cmSQL.CommandText = "proc_ExportFinalData"
            m_cmSQL.Parameters.Clear()
            m_cmSQL.Parameters.AddWithValue("@sRAWFILEID", RF_IDs)
            m_cmSQL.Parameters.AddWithValue("@sPREBLEACHVALUES", PreBleachValues)
            m_cmSQL.Parameters.AddWithValue("@sBLEACHVALUES", BleachValues)
            m_cmSQL.Parameters.AddWithValue("@sINITIALBLEACHVALUES", InitialValues)
            m_cmSQL.Parameters.AddWithValue("@sNORMALIZATIONMETHOD", NormalizationMethod)
            m_cmSQL.Parameters.AddWithValue("@sFITTINGMETHOD", ExponentialEquation)
            m_cnSQL.Open()
            Using m_drSQL As SqlDataReader = m_cmSQL.ExecuteReader()
                dt1.Load(m_drSQL)
                dt2.Load(m_drSQL)
                dt3.Load(m_drSQL)
                For i As Integer = 0 To dt1.Rows.Count - 1
                    sDataForKernelDensityThalf = sDataForKernelDensityThalf + "," + dt1.Rows(i).ItemArray(1)
                Next
            End Using
            sDataForKernelDensityThalf = sDataForKernelDensityThalf.Remove(0, 1)
            sDataForKernelDensityThalf = "[" + sDataForKernelDensityThalf + "]"

            ''1st Excel Sheet for Final Data is created here
            ws = pck.Workbook.Worksheets.Add("Final Data Report")


            'add logo
            Dim logo As System.Drawing.Image
            logo = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("img/easyFRAP-logo-Web-JPG.jpg"))
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
            If NormalizationMethod = "FullScale" Then
                NormalizationMethod = "Full Scale"
            End If
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
            Return
        Finally
            m_cmSQL.CommandType = CommandType.Text
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & sReportName & ".xlsx")
        HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray())
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.SuppressContent = True
        HttpContext.Current.ApplicationInstance.CompleteRequest()
        HttpContext.Current.Response.Close()


    End Sub
End Class
