<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEvaluate.aspx.cs" Inherits="OnlineAssessment.frmEvaluate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Evaluate</title>
    <link href="css/navbargreen.css" rel="stylesheet" />
</head>
<body>
    <script>
        window.onload = function () {
            var x = document.getElementById("ddlEvaluate");
            divSelect(x);
        };

        function divSelect(SelectedDiv) {
            if (SelectedDiv.value == 1) {
                document.getElementById("divAssignment").style.display = "block";
                document.getElementById("divDailyReport").style.display = "none";
                document.getElementById("divSubjects").style.display = "block";
            }
            else {
                document.getElementById("divAssignment").style.display = "none";
                document.getElementById("divSubjects").style.display = "none";
                document.getElementById("divDailyReport").style.display = "block";
            }
        }
    </script>

    <form id="form1" runat="server">
    <div>
        <div>
            <ul>
              <li><a class="active">Evaluate</a></li>
              <li><a href="frmAssignment.aspx">Add Assignment</a></li>
              <li><a href="frmReportGeneration.aspx">Report Generation</a></li>
              <li style="float:right"><a href="frmLogin.aspx">Logout</a></li>
            </ul>
        </div>
        <div style="margin-left:35%;margin-top:2%;">
            <div>Evaluate</div>
            <select id="ddlEvaluate" onchange="divSelect(this);">
                <option value="1">Assignment</option>
                <option value="2">DailyReport</option>
            </select>
        </div>
        <div id="divSubjects" style="margin-left:35%;">
            <div>Subject</div>
            <div><asp:DropDownList ID="ddlSubjects" runat="server" style="width:30%;" AutoPostBack="True" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged"></asp:DropDownList></div>
        </div>
        <div>
            <div id="divAssignment" style="margin-top:2%;">
                <asp:GridView ID="grvAssignment" runat="server" Width="50%" AutoGenerateColumns="false" HorizontalAlign="Center" HeaderStyle-BackColor="#4CAF50" HeaderStyle-ForeColor="White">
                <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Student_Id" HeaderText="Student_Id" />
                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                <asp:BoundField DataField="Assigned_Date" HeaderText="Assigned Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="End_Date" HeaderText="Last Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Uploaded_Date" HeaderText="Submitted Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                <asp:HyperLinkField DataNavigateUrlFields="Id, Student_Id" DataNavigateUrlFormatString="frmDownload.aspx?Id={0}&SId={1}" DataTextField="Description"  HeaderText="Assignment" />
                <asp:BoundField DataField="Mark" HeaderText="Point" />
                </Columns>
                </asp:GridView>
            </div>
            <div id="divDailyReport" style="display:none; margin-top:2%;">
                <asp:GridView ID="grvDailyReport" runat="server" Width="50%" AutoGenerateColumns="false" HorizontalAlign="Center" HeaderStyle-BackColor="#4CAF50" HeaderStyle-ForeColor="White">
                <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Student_Id" HeaderText="Student_Id" />
                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                <asp:HyperLinkField DataNavigateUrlFields="Id, Student_Id" DataNavigateUrlFormatString="frmDownload.aspx?Id={0}&SId={1}&Eval=D" DataTextField="FileName"  HeaderText="Daily Report" />
                <asp:BoundField DataField="Sys_Date" HeaderText="Submitted_Date" DataFormatString="{0:d}" />
                </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
