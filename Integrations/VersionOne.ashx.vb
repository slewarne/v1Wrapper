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

    Dim client As New WebClient
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
            ElseIf action = "listepicsbynum" Then
                doObjectList(context, "Epic")
            ElseIf action = "liststoriesbynum" Then
                doObjectList(context, "Story")
            Else
                context.Response.Write("Error: Action parameter undefined")
            End If

            If v1Return.wasError = False Then

                'check the length of the return object
                Dim serializer As New JavaScriptSerializer
                myResults = serializer.DeserializeObject(v1Return.returnString)

                If v1Return.queryCount = 1 Then       'one-element, such as program list
                    'print it out
                    output = v1Return.returnString
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
    ''' doObjectList: Given a list of Epic numbers, return the associated epics or stories depending on input type
    ''' </summary>
    ''' <param name="context"></param>
    ''' <remarks></remarks>
    Private Sub doObjectList(ByVal context As HttpContext, ByVal voType As String)

        Dim url As String = ""
        Dim id As String
        Dim numberStr As String = ""

        Try
            Dim childNode As XmlElement

            out.Append("{""assets"": {""asset"": [")                                    'start the JSON output

            client.Credentials = New NetworkCredential(ConfigurationManager.AppSettings("voUID"), _password)   'set client credentials for VersionOne

            Dim ids() As String = Split(context.Request("ids"), ",")

            If voType = "Epic" Then
                numberStr = "Number="
            ElseIf voType = "Story" Then
                numberStr = "Super.Number="
            End If

            For Each id In ids
                numberStr = numberStr & "'" & id & "',"
            Next

            'trim last comma
            numberStr = Left(numberStr, Len(numberStr) - 1)

            If voType = "Epic" Then
                url = ConfigurationManager.AppSettings("voURL") & "rest-1.v1/Data/Epic?select=" & ConfigurationManager.AppSettings("voEpicSelect") & "&where=" & numberStr
            ElseIf voType = "Story" Then
                url = ConfigurationManager.AppSettings("voURL") & "rest-1.v1/Data/Story?select=" & ConfigurationManager.AppSettings("voStorySelect") & "&where=" & numberStr
            End If

            'Get the XML data from VersionOne
            Dim pageData As [Byte]() = client.DownloadData(url)

            Dim voXML As String = Encoding.ASCII.GetString(pageData)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(voXML)

            Dim assets = doc.GetElementsByTagName("Asset")

            'loop through the XML returned from VersionOne
            For Each asset As System.Xml.XmlElement In assets


                'start the asset, put on the ID and HREF
                out.Append("{""id"":""" & asset.Attributes("id").Value & """,""href"":""" & asset.Attributes("href").Value & """,")

                'for each attribute returned in the VersionOne output, create JSON name/values with the name of the attribute as name and value as value
                For Each childNode In asset.ChildNodes
                    out.Append("""" & childNode.Attributes("name").Value & """:""" & childNode.InnerText & """,")
                Next

                'we have one extra comma on the end due to the loop above, remove it and then end this asset
                out.Remove(out.Length - 1, 1)
                out.Append("},")

            Next

            'we have one extra comma on the end due to the loop above, remove it and then end the assets array
            If out.ToString <> "{""assets"": {""asset"": [" Then
                out.Remove(out.Length - 1, 1)
            End If

            out.Append("]}}")

            context.Response.ContentType = "application/json"
            context.Response.Write(out.ToString)

        Catch ex As Exception
            context.Response.ContentType = "text/plain"
            context.Response.Write(ex.Message & vbCrLf & ex.ToString() & vbCrLf & "URL: " & url)
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
            queryExplicitCmd = New V1Query("ScopeLabel")
            queryExplicitCmd.addSelect("Name")

            client.Credentials = New NetworkCredential(ConfigurationManager.AppSettings("voUID"), _password)   'set client credentials for VersionOne

            'Get the data from VersionOne
            Dim serializer As New JavaScriptSerializer()
            Dim serializedResult = serializer.Serialize(queryExplicitCmd)

            Dim pageData As String = Replace(client.UploadString(ConfigurationManager.AppSettings("voURL") & "/query.v1", "POST", serializedResult), _
                                             vbCrLf, "")
            'cut the first and last character off
            pageData = Left(pageData, Len(pageData) - 1)
            pageData = Right(pageData, Len(pageData) - 1)

            v1Return.returnString = pageData

        Catch ex As Exception
            v1Return.wasError = True
            v1Return.returnString = ex.Message & vbCrLf & ex.ToString() & vbCrLf
        End Try

    End Sub


    ''' <summary>
    ''' doStories: Get a list of stories or Epics in JSON format
    ''' </summary>
    ''' <param name="context"></param>
    ''' <remarks></remarks>
    Public Sub doStories(ByVal context As HttpContext, ByVal voType As String)

        Dim m_select As String

        Try

            v1Return.queryCount = 2

            client.Credentials = New NetworkCredential(ConfigurationManager.AppSettings("voUID"), _password)   'set client credentials for VersionOne

            Dim m_relDate As String
            Dim relDate As Date
            Dim cmdArr(1) As V1Query


            'get release date
            relDate = DateTime.Parse(context.Request("reldate"))
            m_relDate = relDate.ToString("yyyy-MM-dd")


            If voType = "Story" Then

                'create the query objects - used to build the query.v1 json command
                queryRelatedCmd = New V1Query("Story")
                queryExplicitCmd = New V1Query("Story")

                'get the Select fields
                Dim selects() As String = Split(ConfigurationManager.AppSettings("voStorySelect"), ",")
                For Each m_select In selects
                    queryExplicitCmd.addSelect(m_select)
                    queryRelatedCmd.addSelect(m_select)
                Next

                'add the release date where
                queryRelatedCmd.addWhere("Super." & ConfigurationManager.AppSettings("voEpicRelDate"), m_relDate)
                queryExplicitCmd.addWhere(ConfigurationManager.AppSettings("voStoryRelDate"), m_relDate)
                'add the program where
                queryRelatedCmd.addWhere("Super.Scope.ScopeLabels.Name", context.Request("progname"))
                queryExplicitCmd.addWhere("Scope.ScopeLabels.Name", context.Request("progname"))

            Else
                ' epic

            End If

            'add the commands to the array
            cmdArr(0) = queryRelatedCmd
            cmdArr(1) = queryExplicitCmd

            'Get the data from VersionOne
            Dim serializer As New JavaScriptSerializer()
            Dim serializedResult = serializer.Serialize(cmdArr)

            Dim v1Query As New v1OAuth
            v1Return.returnString = v1Query.runV1Query(serializedResult)

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