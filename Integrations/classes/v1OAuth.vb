Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports OAuth2Client


Public Class v1OAuth

    Dim m_uri As Uri
    Dim credentials As IStorage = New Storage.JsonFileStorage("../../client_secrets_serena2.json", "../../stored_credentials_serena2.json")
    Dim scopes As String = "query-api-1.0 apiv1"
    Dim url As String = "https://www14.v1host.com/Serena/query.v1"
    Dim queryBody As String = ""


    Dim client As WebClient
    'set encoding to utf-8

    Public Sub New()


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
            Dim returnStr As String = client.UploadString(m_uri.ToString, cmd)
            Return returnStr
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




