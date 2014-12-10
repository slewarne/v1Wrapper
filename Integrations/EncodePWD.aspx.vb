Public Class EncodePWD
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnEncode_Click(sender As Object, e As EventArgs) Handles btnEncode.Click
        'encode the textbox
        Dim bytesToEncode As Byte()
        bytesToEncode = Encoding.UTF8.GetBytes(txtPWD.Text)

        Dim encodedText As String
        encodedText = Convert.ToBase64String(bytesToEncode)
        lblEncoded.Text = "Encoded Password:   " & encodedText
    End Sub
End Class