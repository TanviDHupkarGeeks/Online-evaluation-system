<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmStudent.aspx.cs" Inherits="OnlineAssessment.frmStudent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student</title>
    <link href="css/navbarblue.css" rel="stylesheet" />
    <link href="css/onlineAssessment.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <ul>
              <li><a class="active">Assignment</a></li>
              <li><a href="frmDailyReport.aspx">Daily Report</a></li>
              <li><a href="frmPerformance.aspx">Overall Performance</a></li>
              <li style="float:right"><a href="frmLogin.aspx">Logout</a></li>
            </ul>
        </div>
        <div style="margin-left:30%;margin-top:2%;">
            <div>Subject</div>
            <div><asp:DropDownList ID="ddlSubjects" runat="server" style="width:30%;"></asp:DropDownList></div>
            <div>Status</div>
            <div>
                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Value="1">Pending</asp:ListItem>
                    <asp:ListItem Value="0">Done</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
            </div>
        </div>
        <div style="margin-top:2%;">
            <asp:GridView ID="grvAssignment" runat="server" Width="80%" AutoGenerateColumns="false" HorizontalAlign="Center" HeaderStyle-BackColor="DodgerBlue" HeaderStyle-ForeColor="White">
            <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Assigned Date" HeaderText="Assigned Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="End Date" HeaderText="Last Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Uploaded Date" HeaderText="Submitted Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                <asp:HyperLinkField DataNavigateUrlFields="Id, Description, Status" DataNavigateUrlFormatString="frmUpload.aspx?Id={0}&name={1}&status={2}&DId=0" DataTextField="Description"  HeaderText="Assignment" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
            </Columns>
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>