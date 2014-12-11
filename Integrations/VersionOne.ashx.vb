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
    Dim v1Query As v1OAuth

    Dim queryRelatedCmd As V1Query
    Dim queryExplicitCmd As V1Query
    Dim myResults As Object
    Dim output As String

    Dim out As New StringBuilder
    Dim v1Return As v1ReturnInfo
    Dim _password As String = ConfigurationManager.AppSettings("voPWD")         'read the password in from web.config
    Dim decodedBytes() As Byte = Convert.FromBase64String(_password)            'decode the password


    'process the request
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        _password = Encoding.UTF8.GetString(decodedBytes)                       'store the decoded password
        query = New v1QueryBuilder
        v1Query = New v1OAuth

        'handle any exceptions
        Try

            v1Return = New v1ReturnInfo()

            'get the action
            Dim action As String = context.Request.Params("action")

           If action = "listprograms" Then
                doPrograms(context)
            ElseIf action = "liststories" Then
                doStories(context, "Story")
            ElseIf action = "listepics" Then
                doStories(context, "Epic")
            Else
                context.Response.Write("Error: Action parameter undefined")
            End If

            If v1Return.wasError = False Then

                'check the length of the return object
                Dim serializer As New JavaScriptSerializer
                myResults = serializer.DeserializeObject(v1Return.returnString)

                If v1Return.queryCount = 1 Then       'one-element, such as program list
                    'print it out
                    output = v1Return.getSingleArray
                Else                                'multiple elements, such as stories/epics
                    'get unique list
                    output = v1Return.getUniqueJSON

                End If

                context.Response.ContentType = "application/json"
                context.Response.Write(output)

            Else
                context.Response.ContentType = "text/plain"
                context.Response.Write(v1Return.returnString & vbCrLf & vbCrLf & "Query:" & vbCrLf & v1Return.queryString)
            End If


        Catch webEx As WebException
            context.Response.ContentType = "text/plain"
            context.Response.Write(webEx.ToString())
        End Try


    End Sub


    ''' <summary>
    ''' doPrograms: Get a list of programs in JSON format, use the jsTree JSON format for rendering in a treeview
    ''' </summary>
    ''' <param name="context"></param>
    ''' <remarks></remarks>
    Private Sub doPrograms(ByVal context As HttpContext)
        Try
            v1Return.queryCount = 1
            v1Return.returnString = v1Query.runV1Query(query.getProgramJSON)
        Catch ex As Exception
            v1Return.wasError = True
            v1Return.returnString = ex.Message & vbCrLf & ex.ToString() & vbCrLf
        End Try
    End Sub


    ''' <summary>
    ''' doStories: Get a list of stories in JSON format
    ''' </summary>
    ''' <param name="context"></param>
    ''' <remarks></remarks>
    Public Sub doStories(ByVal context As HttpContext, ByVal voType As String)

        Try
            v1Return.queryCount = 2
            v1Return.returnString = v1Query.runV1Query(query.getStoryJSON(context.Request("reldate"), context.Request("progname")))
        Catch ex As Exception
            v1Return.wasError = True
            v1Return.returnString = ex.Message & vbCrLf & ex.ToString() & vbCrLf
        End Try

    End Sub


    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class