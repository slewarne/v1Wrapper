Imports System.Web
Imports System.Web.Services
Imports System.Net
Imports System.Xml
Imports System.IO
Imports System.Web.Script.Serialization

''' <summary>
''' VersionOne wrapper
''' </summary>
''' <remarks> 
''' </remarks>
Public Class VersionOne
    Implements System.Web.IHttpHandler

    ' Dim client As New WebClient
    Dim query As v1QueryBuilder
    Dim queryJSON As String
    Dim v1Query As v1OAuth
    Dim v1Return As v1ReturnInfo

    'process the request
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim out As String = ""

        Try

            query = New v1QueryBuilder
            v1Query = New v1OAuth(context.Server.MapPath("~/config"))
            v1Return = New v1ReturnInfo()

            'get the action
            Dim action As String = context.Request.Params("action")

            'lookup the action from the web.config
            queryJSON = query.getQueryJSON(context.Request.Params("action"))

            If Left(queryJSON, 5) = "Error" Then
                context.Response.ContentType = "text/plain"
                context.Response.Write("Error Occurred" & vbCrLf & queryJSON)
            Else
                'do we have an appsettings node for this action?
                If (ConfigurationManager.AppSettings.AllKeys.Contains(context.Request.Params("action"))) Then
                    Dim vars() As String
                    Dim singleVar As String

                    vars = Split(ConfigurationManager.AppSettings.Get(context.Request.Params("action")), ",")
                    For Each singleVar In vars
                        queryJSON = Replace(queryJSON, "%%" & singleVar & "%%", context.Request.Params(singleVar))
                    Next
                End If

                'got the query - run it.
                v1Return.returnString = v1Query.runV1Query(queryJSON)
                context.Response.ContentType = "application/json;charset=UTF-8"

                If ConfigurationManager.AppSettings.Get("debugOut") = "1" Then
                    context.Response.Write(Trim(v1Return.returnString))
                Else
                    If ConfigurationManager.AppSettings.AllKeys.Contains(context.Request.Params("action") & "_unqfieldlist") Then
                        'we are returning unique fields
                        out = v1Return.getUniqueFields(ConfigurationManager.AppSettings.Get(context.Request.Params("action") & "_fields"))
                        context.Response.Write(Trim(Replace(Trim(out), vbCrLf, "")))
                    Else
                        out = v1Return.getUniqueJSON
                        context.Response.Write(Trim(Replace(Trim(out), vbCrLf, "")))
                    End If

                End If

            End If
        Catch webEx As WebException
            context.Response.ContentType = "text/plain"
            context.Response.Write(webEx.ToString())
        End Try


    End Sub


    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class