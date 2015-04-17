<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Test.aspx.vb" Inherits="Serena_Integrations.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Release Date (YYYY-MM-DD):"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lnkProg" runat="server">Generate Program Query</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtRelDate" runat="server" Width="203px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lnkStory" runat="server">Generate Story Query</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Program Name:"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lnkEpic" runat="server">Generate Epic Query</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtProgName" runat="server" Width="334px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtCMD" runat="server" Width="350px"  Height="300px" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>     <asp:Button ID="btnRun" runat="server" Text="Run" />    </td>
                <td>
                    <asp:TextBox ID="txtResults" runat="server" Height="300px" Width="350px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
    <div>
    
        Note: This test page generates raw output from V1.&nbsp; For program listing, the extra brackets are removed by the wrapper when<br />
        returned to SBM.&nbsp; For Epics/Stories, the multiple lists returned are merged into a single unique list when returned to SBM.&nbsp;
        <br />
        Neither behavior is reflected in the output within this test page.&nbsp; This page is intended to test connectivity to V1,
        <br />
        to view the specific JSON returned to SBM use the URL-format within the browser.<br />
        (i.e. /VersionOne.ashx?action=liststories&amp;reldate=11/28/2014&amp;progname=Sample: Next Release)</div>
    </form>
</body>
</html>
