Public Class V1Query

    Public Property from As String
    Public ReadOnly Property [select] As String()
        Get
            Return m_select.ToArray
        End Get
    End Property

    Public ReadOnly Property where As Dictionary(Of String, String)
        Get
            Return m_where
        End Get
    End Property

    Private m_where As Dictionary(Of String, String)
    Private m_select As List(Of String)


    Public Sub New(v1From As String)
        from = v1From
        m_select = New List(Of String)
        m_where = New Dictionary(Of String, String)
    End Sub

    Public Sub New()
        m_select = New List(Of String)
        m_where = New Dictionary(Of String, String)
    End Sub

    Public Sub addSelect(ByVal field As String)
        m_select.Add(field)
    End Sub

    Public Sub addWhere(ByVal name As String, ByVal value As String)
        m_where.Add(name, value)
    End Sub

End Class


