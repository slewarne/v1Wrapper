Imports System.Web.Script.Serialization

Public Class v1QueryBuilder

    Private m_query As V1Query
    Private m_query2 As V1Query
    Private m_serializer As JavaScriptSerializer


    Public Sub New()
        m_query = New V1Query()
        m_query2 = New V1Query()
        m_serializer = New JavaScriptSerializer
    End Sub

    Public Function getProgramJSON() As String
        m_query.from = "ScopeLabel"
        m_query.addSelect("Name")
        Return m_serializer.Serialize(m_query)
    End Function

    Public Function getStoryJSON(ByVal releaseDate As String, ByVal progName As String) As String

        Dim m_select As String
        Dim m_relDate As String
        Dim relDate As Date
        Dim cmdArr(1) As V1Query

        Try
            'get release date
            relDate = DateTime.Parse(releaseDate)
            m_relDate = relDate.ToString("yyyy-MM-dd")

            m_query.from = "Story"
            m_query2.from = "Story"

            'get the Select fields
            Dim selects() As String = Split(ConfigurationManager.AppSettings("voStorySelect"), ",")
            For Each m_select In selects
                m_query.addSelect(m_select)
                m_query2.addSelect(m_select)
            Next

            'add the release date where
            m_query.addWhere("Super." & ConfigurationManager.AppSettings("voEpicRelDate"), m_relDate)
            m_query2.addWhere(ConfigurationManager.AppSettings("voStoryRelDate"), m_relDate)
            'add the program where
            m_query.addWhere("Super.Scope.ScopeLabels.Name", progName)
            m_query2.addWhere("Scope.ScopeLabels.Name", progName)

            'add the commands to the array
            cmdArr(0) = m_query
            cmdArr(1) = m_query2

            Return m_serializer.Serialize(cmdArr)
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

End Class
