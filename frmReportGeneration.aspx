<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportGeneration.aspx.cs" Inherits="OnlineAssessment.frmReportGeneration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Generation</title>
    <link href="css/onlineAssessment.css" rel="stylesheet" />
    <link href="css/navbargreen.css" rel="stylesheet" />
</head>
<body>
  <script>
      function Checknumeric() {
          var inputtxt1 = document.getElementById('txtLessThan');
          var inputtxt2 = document.getElementById('txtGreaterThan');

          if (inputtxt1.value == '')
              inputtxt1 = 0;

          if (inputtxt2.value == '')
              inputtxt2 = 0;

          if (inputtxt1.value != '' || inputtxt2.value != '') {
              var numbers = /^[0-9]+$/;
              if (inputtxt1.value.match(numbers) && inputtxt2.value.match(numbers)) {
                  return true;
              }
              else {
                  alert('Please input numeric characters only');
                  return false;
              }
          }
      }

      function onlyNumbers(evt) {
          var e = event || evt; // for trans-browser compatibility
          var charCode = e.which || e.keyCode;
          if (charCode > 31 && (charCode < 48 || charCode > 57))
              return false;
          return true;
      }

      function PrintReport() {
          printData();
      }

      function printData() {
          var e = document.getElementById("ddlReport");
          var value = e.options[e.selectedIndex].value;
          var divToPrint;

          if (value == 0) {
              var divToPrint = document.getElementById("divRatingReport");
          }
          if (value == 1) {
              divToPrint = document.getElementById("divAssignment");
          }
          newWin = window.open("");
          newWin.document.write(divToPrint.outerHTML);
          newWin.print();
          newWin.close();
      }
  </script>
    <form id="form1" runat="server">
    <div>
        <div>
            <ul>
              <li><a href="frmEvaluate.aspx">Evaluate</a></li>
              <li><a href="frmAssignment.aspx">Add Assignment</a></li>
              <li><a class="active">Report Generation</a></li>
              <li style="float:right"><a href="frmLogin.aspx">Logout</a></li>
            </ul>
        </div>

        <div style="margin-top:1%;float:right;margin-right:10%;">
            <input type="button" id="btnPrint" value='Print' onclick="PrintReport();" />
        </div>
        <div id="divReport">
        <div style="margin-left:40%; margin-top:3%">
        <div style="float:left">Generate</div>
        <div style="margin-left:15%;margin-bottom:1%;">
            <asp:DropDownList ID="ddlReport" runat="server" Width="25%" AutoPostBack="true" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged">
                <asp:ListItem Value="0">Skill Report</asp:ListItem>
                <asp:ListItem Value="1">Student Report</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div style="float:left">Student Id</div>
        <div style="margin-left:15%">
            <asp:DropDownList ID="ddlStudents" runat="server" Width="10%" AutoPostBack="true" OnSelectedIndexChanged="ddlStudents_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div id="divSubject" runat="server">
        <div style="float:left; margin-top:1%;">Subject</div>
        <div style="margin-left:15%; margin-top:1%;">
            <asp:DropDownList ID="ddlSubjects" runat="server" Width="25%" AutoPostBack="true" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged"></asp:DropDownList>
        </div>
        </div>

        </div>
        <div id="divPointFilter" runat="server" style="margin-left:40%;">
            <div>
                <div style="float:left; margin-top:1%;">Point Less than</div>
                <div style="margin-top:1%; margin-left:15%;"><asp:TextBox ID="txtLessThan" runat="server" Width="5%" MaxLength="2" Font-Bold="true" AutoCompleteType="Disabled" onkeypress="return onlyNumbers(this);"></asp:TextBox></div>
            </div>
            <div>
                <div style="float:left; margin-top:1%;">Point Greater than</div>
                <div style="margin-top:1%; margin-left:15%;"><asp:TextBox ID="txtGreaterThan" runat="server" Width="5%" MaxLength="2" Font-Bold="true" AutoCompleteType="Disabled" onkeypress="return onlyNumbers(this);"></asp:TextBox></div>
            </div>
            <div style="margin-top:2%;margin-left:5%"><asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" OnClientClick="return Checknumeric()" /></div>
        </div>
        <div id="divRatingReport">
            <asp:GridView ID="grvOverallSkillReport" runat="server" Width="25%" AutoGenerateColumns="false" style="margin-top:3%;" HeaderStyle-BackColor="#4CAF50" HeaderStyle-ForeColor="White" HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Student_Id" HeaderText="Student Id" />
                <asp:BoundField DataField="Average_Mark" HeaderText="Overall Points" />
            </Columns>
            </asp:GridView>

            <asp:GridView ID="grvSkillReport" runat="server" Width="25%" AutoGenerateColumns="false" style="margin-top:3%;" HeaderStyle-BackColor="#4CAF50"
                 OnRowDataBound="grvSkillReport_RowDataBound" HorizontalAlign="Center" HeaderStyle-ForeColor="White">
            <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Description" HeaderText="Qualities" />
                <asp:BoundField DataField="Average_Mark" HeaderText="Average Mark" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                <asp:BoundField HeaderText="Rating" />
            </Columns>
            </asp:GridView>
        </div>
        <div id="divAssignment" style="margin-top:3%;">
                <asp:GridView ID="grvAssignment" runat="server" Width="50%" AutoGenerateColumns="false" HorizontalAlign="Center" HeaderStyle-BackColor="#4CAF50" HeaderStyle-ForeColor="White">
                <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Student_Id" HeaderText="Student_Id" />
                <asp:HyperLinkField DataTextField="Subject"  HeaderText="Subject" />
                <asp:HyperLinkField DataTextField="Description"  HeaderText="Assignment" />
                <asp:HyperLinkField DataTextField="Subject"  HeaderText="Subject" />
                <asp:BoundField DataField="Mark" HeaderText="Mark" />
                <asp:BoundField DataField="Sys_Date" HeaderText="Submitted Date" DataFormatString="{0:d}" />
                </Columns>
                </asp:GridView>
        </div>
        <div style="margin-left:42%; margin-top:2%;font-weight:bold;"><asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label></div>
    </div>
    </div>
    </form>
</body>
</html>