<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="OnlineAssessment.frmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="css/onlineAssessment.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="divCenterAlign">
            <div><img src="images/imgAssessment.jpg" /></div>
            <div>
                <div>Email</div>
                <div><asp:TextBox ID="txtEmail" runat="server" style="width:75%;"></asp:TextBox></div>
                <div>Password</div>
                <div><asp:TextBox ID="txtPassword" runat="server" style="width:75%;" TextMode="Password"></asp:TextBox></div>
                <div id="divMessage" runat="server" style="display:none; margin-top:3%; margin-bottom:3%">
                    <asp:Label ID="lblMessage" runat="server" Text="Wrong Credentials" Font-Size="Large" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
            </div>
            <div><asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /></div>
        </div>
    </div>
    </form>
</body>
</html>

