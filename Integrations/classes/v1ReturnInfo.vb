Imports System.Web.Script.Serialization

Public Class v1ReturnInfo
    Public Property wasError As Boolean
    Public Property queryString As String
    Public Property returnString As String
    Public Property queryCount As Integer



    Public Sub New()
        wasError = False
    End Sub

    Public Function getSingleArray() As String

        Dim tmpStr As String = returnString
        'cut the first and last character off
        tmpStr = Left(tmpStr, Len(tmpStr) - 1)
        tmpStr = Right(tmpStr, Len(tmpStr) - 1)
        Return tmpStr

    End Function

    Public Function getUniqueJSON() As String

        Dim tmpList As New List(Of Object)
        Dim tmpHash As New Hashtable
        Dim returnArr As Object
        Dim curObj As Object
        Dim v1Obj As Object

        Dim serializer As New JavaScriptSerializer
        returnArr = serializer.DeserializeObject(returnString)

        If returnArr.length = 1 Then
            'single array of results, assume they are unique and strip the extra [] for SBM to handle
            Return getSingleArray()
        Else
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
        End If

    End Function


End Class
