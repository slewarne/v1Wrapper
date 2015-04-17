Public Class v1SOAPReturn

    Private m_namevalues As List(Of v1NameValue)

    Public ReadOnly Property nameValuePairs As v1NameValue()
        Get
            Return m_namevalues.ToArray
        End Get
    End Property

    Public Sub New()
        m_namevalues = New List(Of v1NameValue)
    End Sub

    Public Sub addNameValue(ByVal name As String, ByVal value As String)
        m_namevalues.Add(New v1NameValue(name, value))
    End Sub

End Class

Public Class v1NameValue

    Public Property [Name] As String
    Public Property [Value] As String

    Public Sub New(ByVal name As String, ByVal value As String)
        Me.Name = name
        Me.Value = value
    End Sub
End Class
