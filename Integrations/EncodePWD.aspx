<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EncodePWD.aspx.vb" Inherits="Serena_Integrations.EncodePWD" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style="width: 100%;">
            <tr>
                <td>Password to Encode:&nbsp;
        <asp:TextBox ID="txtPWD" runat="server" Width="266px" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnEncode" runat="server" Text="Encode" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEncoded" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        
    
    </div>
        
    </form>
</body>
</html>
