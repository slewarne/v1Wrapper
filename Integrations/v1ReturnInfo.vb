Imports System.Web.Script.Serialization

Public Class v1ReturnInfo
    Public Property wasError As Boolean
    Public Property queryString As String
    Public Property returnString As String
    Public Property queryCount As Integer



    Public Sub New()
        wasError = False
    End Sub

    Public Function getUniqueJSON() As String

        Dim tmpList As New List(Of Object)
        Dim tmpHash As New Hashtable
        Dim returnArr As Object
        Dim curObj As Object
        Dim v1Obj As Object

        Dim serializer As New JavaScriptSerializer
        returnArr = serializer.DeserializeObject(returnString)

        For Each curObj In returnArr
            For Each v1Obj In curObj
                If Not tmpHash.ContainsKey(v1Obj("_oid")) Then
                    tmpHash.Add(v1Obj("_oid"), "")
                    tmpList.Add(v1Obj)
                End If
            Next
        Next

        tmpHash.Clear()
        tmpHash = Nothing
        Return serializer.Serialize(tmpList)

    End Function


End Class
