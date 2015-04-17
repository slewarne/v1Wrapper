Imports System.Web.Script.Serialization
Imports System.Collections.Generic


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

    Public Function getUniqueFields(ByVal fieldList As String) As String

        Dim flds() As String = Split(fieldList, ",")
        Dim tmpFld As String = ""

        Dim tmpList As New List(Of v1SingleUniqueValue)
        Dim tmpHash As New Hashtable
        Dim returnArr As Object
        Dim curObj As Object
        Dim v1Obj As Object
        Dim tmpDict As Dictionary(Of String, Object)

        Dim serializer As New JavaScriptSerializer
        returnArr = serializer.DeserializeObject(returnString)

        For Each curObj In returnArr        ' this is the array of arrays that is returned
            For Each v1Obj In curObj        ' this is each V1 object in the specific array
                tmpDict = CType(v1Obj, Dictionary(Of String, Object))
                For Each tmpFld In flds     ' for each field in the field list, look it up
                    If tmpDict.ContainsKey(tmpFld) Then
                        If Not tmpDict(tmpFld) Is Nothing Then
                            If Not tmpHash.Contains(tmpDict(tmpFld)) Then
                                'add the object to the list
                                tmpHash.Add(tmpDict(tmpFld), "")
                                tmpList.Add(New v1SingleUniqueValue(tmpDict(tmpFld)))
                            End If
                        End If
                    End If
                Next
            Next
        Next

        tmpHash.Clear()
        tmpHash = Nothing
        Return serializer.Serialize(tmpList)

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

Public Class v1SingleUniqueValue

    Public Property fieldValue As String

    Public Sub New(ByVal inVal As String)
        fieldValue = inVal
    End Sub
End Class
