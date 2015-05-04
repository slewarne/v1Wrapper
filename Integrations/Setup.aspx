<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Setup.aspx.vb" Inherits="Serena_Integrations.Setup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 34px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Panel ID="panelSecret" runat="server" Visible="False">
            <table>
                <tr><td>
                    <asp:Label ID="Label1" runat="server" Text="Please paste the contents of the clients_secret file below:"></asp:Label>
                    </td><td class="auto-style1"></td></tr>
                <tr><td>
                    <asp:TextBox ID="txtSecrets" runat="server" Height="144px" Rows="20" Width="616px" TextMode="MultiLine"></asp:TextBox>
                    </td><td class="auto-style1"></td></tr>
                <tr><td>
                    <asp:Button ID="BtnSaveSecrets" runat="server" Text="Save Clients_Secrets" />
                    </td><td class="auto-style1">
                        <asp:HiddenField ID="cmdAction" runat="server" Visible="False" />
                    </td></tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="panelRequestToken" runat="server" Visible="False">
            <table>
                <tr><td>
                    <asp:Label ID="Label5" runat="server" Text="Clients_Secret info has been found.  Follow instructions below to request an access token."></asp:Label>
                    </td><td class="auto-style1"></td></tr>
                <tr><td>
                    <asp:Label ID="Label2" runat="server" Text="1. Click "></asp:Label>
                    <asp:HyperLink ID="lnkV1" runat="server" Target="_blank">here </asp:HyperLink>
                    <asp:Label ID="Label4" runat="server" Text="to request a VersionOne access token.  You will be prompted to login and confirm API access.  "></asp:Label>
                    </td><td class="auto-style1"></td></tr>
                <tr><td>
                    <asp:Label ID="Label7" runat="server" Text="2. Once you login to V1 and confirm access, a token will appear."></asp:Label>
                    &nbsp; You have 60 seconds to complete step 3.</td><td class="auto-style1"></td></tr>
                <tr><td>
                    <asp:Label ID="Label9" runat="server" Text="3. Paste the V1 token below and click &quot;Save Request Token&quot; to continue."></asp:Label>
                    </td><td class="auto-style1"></td></tr>
                <tr><td>
                    <asp:Label ID="lblTokenError" runat="server" Text="Please paste the V1 token below to continue." Visible="False"></asp:Label>
                    </td><td class="auto-style1"></td></tr>

                <tr><td>
                    <asp:TextBox ID="txtRequestToken" runat="server" Height="144px" Rows="20" Width="616px" TextMode="MultiLine"></asp:TextBox>
                    </td><td class="auto-style1"></td></tr>
                <tr><td>
                    <asp:Button ID="Button2" runat="server" Text="Save Request Token" />
                    </td><td class="auto-style1">
                        &nbsp;</td></tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlReady" runat="server" Visible="False">
            <table>
                <tr><td>
                    <asp:Label ID="Label3" runat="server" Text="Secrets and Credentials Files found, continue to test page."></asp:Label>
                    </td><td class="auto-style1"></td></tr>
                <tr><td>
                    <asp:Button ID="btnContinue" runat="server" Text="Continue" />
                    </td><td class="auto-style1">
                        &nbsp;</td></tr>
            </table>
        </asp:Panel>
    
    </div>
    </form>
</body>
</html>
