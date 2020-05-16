<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDailyReport.aspx.cs" Inherits="OnlineAssessment.frmDailyReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Daily Report</title>
    <link href="css/navbarblue.css" rel="stylesheet" />
    <link href="css/onlineAssessment.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <ul>
              <li><a href="frmStudent.aspx">Assignment</a></li>
              <li><a class="active">Daily Report</a></li>
              <li><a href="frmPerformance.aspx">Overall Performance</a></li>
              <li style="float:right"><a href="frmLogin.aspx">Logout</a></li>
            </ul>
        </div>
        
        <!-- File Upload -->

    <div>
        <div id="divFileUpload" runat="server" style="margin-left:25%;margin-top:5%">
            <div>Choose Subject</div>
            <div><asp:DropDownList ID="ddlSubjects" runat="server"></asp:DropDownList></div>
            <div style="margin-top:2%;">Upload Daily Report</div>
            <div style="margin-top:1%;"><asp:FileUpload ID="docFile" runat="server" /></div>
            <div style="margin-top:2%"><asp:Button ID="btnUpload" runat="server" Text="Upload" OnClientClick="return ProgressBar()" CssClass="button" OnClick="btnUpload_Click" /></div>
            <div style="margin-top:2%;"><asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label></div>
        </div>
        
        <!-- Progress Bar -->

        <div id="divUpload" style="display: none; margin-left:25%;margin-top:7%;">
            <div style="width: 300pt; text-align: center;">Uploading...</div>
            <div style="width: 300pt; height: 20px; border: solid 1pt gray">
                <div id="divProgress" runat="server" style="width: 1pt; height: 20px; background-color: orange;display: none"></div>
            </div> 
            <div style="width: 300pt; text-align: center;">
                <asp:Label ID="lblPercentage" runat="server" Text="Label"></asp:Label></div>
            <br />
        </div>
    </div>
<div>
    <div style="margin-top:2%;">
            <asp:GridView ID="grvDailyReport" runat="server" Width="50%" AutoGenerateColumns="false" HorizontalAlign="Center" HeaderStyle-BackColor="DodgerBlue" HeaderStyle-ForeColor="White">
            <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Sys_Date" HeaderText="Submitted_Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="frmUpload.aspx?DId={0}&Id=0&status=Done" DataTextField="FileName"  HeaderText="FileName" />
            </Columns>
        </asp:GridView>
        </div>
</div>
    </div>
    </form>
</body>
</html>
