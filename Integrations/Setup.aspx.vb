Imports System.IO
Imports System.Net
Imports OAuth2Client
'

Public Class Setup
    Inherits System.Web.UI.Page

    Dim myPath As String = Context.Server.MapPath("~/config")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim myFile As FileStream


        If Page.IsPostBack = False Then

            'check to see if we have the two files we need.  First is the secrets file.
            If File.Exists(myPath & "\client_secrets.json") And File.Exists(myPath & "\stored_credentials.json") Then
                pnlReady.Visible = True
            Else
                If Not File.Exists(myPath & "\client_secrets.json") Then
                    'create the directory if it doesn't exist
                    If Not Directory.Exists(myPath) Then
                        Directory.CreateDirectory(myPath)
                    End If
                    'we have no client secrets file, prompt the user for it.
                    panelSecret.Visible = True
                    cmdAction.Value = "save_secrets"
                Else
                    'prep the request URL
                    showRequestPanel()
                End If
            End If
           
        Else
            'postback
            If cmdAction.Value = "save_secrets" Then

                If txtSecrets.Text = "" Then
                    panelSecret.Visible = True
                    cmdAction.Value = "save_secrets"
                Else
                    'store the secrets file
                    myFile = File.OpenWrite(myPath & "\client_secrets.json")
                    Dim secrets As Byte() = New UTF8Encoding(True).GetBytes(txtSecrets.Text)
                    myFile.Write(secrets, 0, secrets.Length)
                    myFile.Close()
                    panelSecret.Visible = False
                    showRequestPanel()
                End If

            ElseIf cmdAction.Value = "request_cert" Then
                'validate that a cert was entered
                If txtRequestToken.Text = "" Then
                    lblTokenError.Visible = True
                Else
                    'attempt to get the token.
                    Dim client As New webclient
                    Dim reqparm As New Specialized.NameValueCollection
                    Dim credentials As IStorage = New Storage.JsonFileStorage(myPath & "\client_secrets.json", myPath & "\client_secrets.json")
                    Dim url As String = ConfigurationManager.AppSettings("voURL") & "oauth.v1/token"
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded")

                    reqparm.Add("client_id", credentials.GetSecrets.client_id)
                    reqparm.Add("client_secret", credentials.GetSecrets.client_secret)
                    reqparm.Add("code", txtRequestToken.Text)
                    reqparm.Add("grant_type", "authorization_code")
                    reqparm.Add("redirect_uri", credentials.GetSecrets.redirect_uris(0).ToString)


                    Dim responsebytes = client.UploadValues(url, "POST", reqparm)
                    Dim responsebody = (New Text.UTF8Encoding).GetString(responsebytes)
                    'store it
                    myFile = File.OpenWrite(myPath & "\stored_credentials.json")
                    myFile.Write(New UTF8Encoding(True).GetBytes(responsebody), 0, responsebody.Length)
                    myFile.Close()
                    panelRequestToken.Visible = False
                    pnlReady.Visible = True
                    cmdAction.Value = "done"

                End If
               
            End If
        End If


    End Sub

    Private Sub showRequestPanel()
        'set the URL for V1
        Dim credentials As IStorage = New Storage.JsonFileStorage(myPath & "\client_secrets.json", myPath & "\client_secrets.json")

        Dim url As String = ConfigurationManager.AppSettings("voURL") & "oauth.v1/auth?" & _
                            "response_type=code&client_id=" & credentials.GetSecrets.client_id & "&redirect_uri=" & _
                            credentials.GetSecrets.redirect_uris(0).ToString & "&scope=apiv1 query-api-1.0"

        lnkV1.NavigateUrl = url
        'show validation panel
        panelRequestToken.Visible = True
        cmdAction.Value = "request_cert"
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles btnContinue.Click
        Response.Redirect("test.aspx", True)
    End Sub

   
End Class