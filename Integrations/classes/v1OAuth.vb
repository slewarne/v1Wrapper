Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Text
'Imports Newtonsoft.Json
'Imports Newtonsoft.Json.Linq
Imports OAuth2Client


Public Class v1OAuth

    Dim m_configPath As String
    Dim m_uri As Uri
    Dim credentials As IStorage
    Dim scopes As String = "query-api-1.0 apiv1"
    Dim url As String
    Dim queryBody As String = ""

    Dim client As WebClient
    'set encoding to utf-8

    Public Sub New(ByVal configPath As String)
        Try
            m_configPath = configPath
            url = ConfigurationManager.AppSettings("voURL") & "query.v1"
            credentials = New Storage.JsonFileStorage(configPath & "\client_secrets.json", configPath & "\stored_credentials.json")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function getCreds() As IStorage
        Return credentials
    End Function

    Public Function runV1Query(ByVal cmd As String) As String

        client = New WebClient
        client.Encoding = Encoding.UTF8
        m_uri = New Uri(url)

        client.Headers.Add("Authorization", "Bearer " & credentials.GetCredentials.AccessToken)

        Try

            Return client.UploadString(m_uri.ToString, cmd)

        Catch ex As WebException
            If ex.Status = WebExceptionStatus.ProtocolError Then
                If Not CType(ex.Response, HttpWebResponse).StatusCode = HttpStatusCode.Unauthorized Then
                    Throw
                Else
                    'refresh the access token
                    Dim secrets As OAuth2Client.Secrets = credentials.GetSecrets
                    Dim m_authClient As AuthClient = New AuthClient(secrets, scopes, Nothing, Nothing)
                    Dim newcreds As Credentials = m_authClient.refreshAuthCode(credentials.GetCredentials)
                    Dim storedcreds As Credentials = credentials.StoreCredentials(newcreds)
                    client.Headers.Clear()
                    client.Headers.Add("Authorization", "Bearer " & storedcreds.AccessToken)
                    Return client.UploadString(m_uri.ToString, cmd)

                End If
            End If
        End Try
        Return ""
    End Function

    End Class




