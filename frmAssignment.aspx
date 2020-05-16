<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAssignment.aspx.cs" Inherits="OnlineAssessment.frmAssignment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Assignment</title>
    <link href="css/onlineAssessment.css" rel="stylesheet" />
    <link href="css/navbargreen.css" rel="stylesheet" />
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
    <script src="js/jquery-3.5.0.min.js"></script>
    <script src="js/jquery-ui.min.js"></script>
</head>
<body>
    <script>
        $(document).ready(function () {
            $("#txtEndDate").datepicker({ minDate: 0 });
        });

        // Checking txtEndDate is Empty
        $(document).ready(function () {
            $('#btnAdd').click(function () {
                if (!$.trim($('#txtAssignment').val())) {
                    alert("Please Enter the Assignment Name");
                    return false;
                }

                if (!$.trim($('#txtEndDate').val())) {
                    alert("Please Enter the Last Date");
                    return false;
                }
            });
        });
  </script>  
    <form id="form1" runat="server">
    <div>
        <div>
            <ul>
              <li><a href="frmEvaluate.aspx">Evaluate</a></li>
              <li><a class="active" href="frmAssignment">Add Assignment</a></li>
              <li><a href="frmReportGeneration.aspx">Report Generation</a></li>
              <li style="float:right"><a href="frmLogin.aspx">Logout</a></li>
            </ul>
        </div>
        <div style="margin-left:35%;margin-top:2%;">
            <div>Subject</div>
            <div><asp:DropDownList ID="ddlSubjects" runat="server" style="width:30%;" AutoPostBack="True" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged"></asp:DropDownList></div>
            <div style="margin-top:1%;">Assignment Name</div>
            <div><asp:TextBox ID="txtAssignment" runat="server" Width="30%" Height="25%" MaxLength="50" AutoCompleteType="Disabled"></asp:TextBox></div>
            <div style="margin-top:1%;">Last Date</div>
            <div><asp:TextBox ID="txtEndDate" Width="30%" runat="server" AutoCompleteType="Disabled"></asp:TextBox></div>
            <div style="margin-top:1%;">Upload Assignment Details</div>
            <div style="margin-top:1%;"><asp:FileUpload ID="docFile" runat="server" /></div>
            <div style="margin-top:1%;"><asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" /></div>
            <div style="margin-top:1%;"><asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></div>
        </div>
        <div style="margin-top:1%;">
            <asp:GridView ID="grvAssignment" runat="server" Width="50%" AutoGenerateColumns="false" HorizontalAlign="Center" HeaderStyle-BackColor="#4CAF50" HeaderStyle-ForeColor="White">
                <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                <asp:BoundField DataField="Assignment" HeaderText="Assignment" />
                <asp:BoundField DataField="End Date" HeaderText="Last Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Assigned Date" HeaderText="Assigned Date" DataFormatString="{0:d}" />
                </Columns>
                </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
