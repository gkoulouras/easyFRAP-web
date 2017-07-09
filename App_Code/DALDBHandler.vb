Imports System.Data.SqlClient
Imports System.Data

Public Class DALDBHandler

#Region " Class Variables "
    Protected m_cnSQL As SqlConnection
    Protected m_daSQL As SqlDataAdapter
    Protected m_cmSQL As SqlCommand
#End Region

#Region " Class Public Methods "

    'This function is used to the validation of the SMS Message, 
    'in order to define if the last parameter concerns the geo-location
    'The geo-location parameter is added by another program, always last, 
    'with the format: ~Lng~Lat~Alt~Provider~ (always with 5 delimeters)
    'So, if we substract the length of the original message with the length of the message 
    'without the delimeters, the result must always be 5.
    'If not, the last parameter is something else and wrong...
    Public Function CountChars(ByVal value As String, ByVal delim As String) As Integer
        Return Len(value) - Len(Replace(value, delim, ""))
    End Function

    Public Function FillTable(ByVal sTableName As String, ByVal sSQL As String) As DataTable
        Try
            Dim dtTable As New DataTable(sTableName)

            m_cmSQL.CommandText = sSQL

            m_cnSQL.Open()
            m_daSQL.Fill(dtTable)

            Return dtTable
        Catch ex As Exception
            LogToFile(sSQL, ex.Message)
            Return Nothing
        Finally
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try
    End Function

    Public Function GetDataReader(ByVal sSQL As String) As SqlDataReader
        Dim drData As SqlDataReader

        Try
            m_cmSQL.CommandText = sSQL

            m_cnSQL.Open()
            drData = m_cmSQL.ExecuteReader

            Return drData
        Catch ex As Exception
            LogToFile(sSQL, ex.Message)
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
            Return Nothing
        End Try
    End Function

    Public Sub CloseDataReader(ByVal drData As SqlDataReader)
        If Not IsNothing(drData) Then
            drData.Close()
        End If
        If m_cnSQL.State = ConnectionState.Open Then
            m_cnSQL.Close()
        End If
    End Sub

    Public Function ExecuteSQL(ByVal sSQLString As String) As Integer
        Try
            m_cmSQL.CommandText = sSQLString
            m_cnSQL.Open()
            Return m_cmSQL.ExecuteScalar
        Catch ex As Exception
            LogToFile(sSQLString, ex.Message)
            Return -1
        Finally
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try
    End Function

    Public Function ExecuteMultiSQL(ByVal sSQLString As String) As Integer
        Try
            m_cmSQL.CommandText = sSQLString
            m_cnSQL.Open()
            Return m_cmSQL.ExecuteNonQuery
        Catch ex As Exception
            LogToFile(sSQLString, ex.Message)
            Return -1
        Finally
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try
    End Function

    Public Function FillSchemaBySQL(ByVal sTableName As String, ByVal sSQL As String) As DataTable
        Try
            Dim dtTable As New DataTable(sTableName)

            m_cmSQL.CommandText = sSQL

            m_cnSQL.Open()
            m_daSQL.FillSchema(dtTable, SchemaType.Source)

            Return dtTable
        Catch ex As Exception
            LogToFile(sSQL, ex.Message)
            Return Nothing
        Finally
            If m_cnSQL.State = ConnectionState.Open Then
                m_cnSQL.Close()
            End If
        End Try
    End Function

    Public Function LogToFile(ByVal sSQL As String, ByVal sMessage As String) As Boolean
        Dim NextFile As Short

        Try
            NextFile = FreeFile()
            FileOpen(NextFile, HttpContext.Current.Server.MapPath("~") & "\SQLErrorLOG.log.txt.config", OpenMode.Append)
            PrintLine(NextFile, Now & vbCrLf & "SQL: " & _
                      sSQL & vbCrLf & "Error: " & sMessage & vbCrLf)
            FileClose(NextFile)
        Catch MyEx As System.UnauthorizedAccessException
            'MsgBox(MyEx.ToString, MsgBoxStyle.OkOnly)
        End Try
        Return 1
    End Function

    Public Sub New()
        m_cnSQL = New SqlConnection(ConfigurationManager.ConnectionStrings("sConnectionStringName").ConnectionString)
        m_daSQL = New SqlDataAdapter
        m_cmSQL = New SqlCommand

        m_daSQL.SelectCommand = m_cmSQL
        m_cmSQL.Connection = m_cnSQL
        m_cmSQL.CommandTimeout = 3500
    End Sub

    Protected Overrides Sub Finalize()
        m_cnSQL = Nothing
        m_daSQL = Nothing
        m_cmSQL = Nothing

        MyBase.Finalize()
    End Sub

#End Region

End Class
