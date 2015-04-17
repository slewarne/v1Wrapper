Public Class Test
    Inherits System.Web.UI.Page

    Dim m_builder As New v1QueryBuilder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub lnkProg_Click(sender As Object, e As EventArgs) Handles lnkProg.Click
        txtCMD.Text = m_builder.getProgramJSON
    End Sub

    Protected Sub lnkStory_Click(sender As Object, e As EventArgs) Handles lnkStory.Click
        If validateParam() Then
            'txtCMD.Text = m_builder.getStoryJSON(txtRelDate.Text, txtProgName.Text)
            txtCMD.Text = m_builder.getQueryJSON("v1getStories")

            txtCMD.Text = Replace(txtCMD.Text, "%%REL_DATE%%", txtRelDate.Text)
            txtCMD.Text = Replace(txtCMD.Text, "%%PROG_NAME%%", txtProgName.Text)
        Else
            txtCMD.Text = "Please enter a Release Date and Program Name"
        End If

    End Sub

    Private Function validateParam() As Boolean
        If txtRelDate.Text <> "" And txtProgName.Text <> "" Then
            Return True
        Else
            Return False
        End If
    End Function
    Protected Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        'run the query
        Dim v1Query As New v1OAuth(Server.MapPath("~/config"))

        Try
            txtResults.Text = v1Query.runV1Query(txtCMD.Text)
        Catch ex As Exception

            txtResults.Text = ex.Message & vbCrLf & ex.ToString() & vbCrLf
        End Try
    End Sub
End Class