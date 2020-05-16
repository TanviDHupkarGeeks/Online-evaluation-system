<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDownload.aspx.cs" Inherits="OnlineAssessment.frmDownload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download</title>
    <link href="css/onlineAssessment.css" rel="stylesheet" />
    <link href="css/navbargreen.css" rel="stylesheet" />
    <style>
        .button {
            background-color: #4CAF50;
        }
    </style>
   <script>
       function Checknumeric() {
           var inputtxt = document.getElementById('txtMark');
           var numbers = /^[0-9]+$/;
           if (inputtxt.value.match(numbers)) {
               return true;
           }
           else {
               alert('Please input numeric characters only');
               return false;
           }
       }

         function onlyNumbers(evt) {
             var e = event || evt; // for trans-browser compatibility
             var charCode = e.which || e.keyCode;
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                 return false;
             return true;
         }

         function ValidateRateGrid()
         {
             var bool;
             var Grid_Table = document.getElementById('<%= grvRate.ClientID %>');
             var numbers = /^[0-9]+$/;
             for (var row = 1; row < Grid_Table.rows.length; row++) {
                 var inputtxt = (Grid_Table.rows[row].cells[3].childNodes[1].value);
                 
                 if (inputtxt.match(numbers)) {
                     if (Number(inputtxt) <= 10) {
                         bool = true;
                     }
                     else {
                         alert("Please enter the points out of 10");
                         return false;
                     }
                 }
                 else {
                     alert('Please input numeric characters only');
                     bool = false;
                     break;
                 }
             }
             return bool;
         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <ul>
              <li><a href="frmEvaluate.aspx" class="active">Evaluate</a></li>
              <li><a href="frmAssignment.aspx">Add Assignment</a></li>
              <li><a href="frmReportGeneration.aspx">Report Generation</a></li>
              <li style="float:right"><a href="frmLogin.aspx">Logout</a></li>
            </ul>
        </div>

        <!-- ----------------------------------------------- Download ------------------------------------------------------------------------ -->

        <div>
            <div style="margin-top:6%;">
            <div style="float:left;font-weight:bold; margin-left:38%;">
                <div>Student Id</div>
                <div>File</div>
                <div>Date</div>
                <div id="divlblMark" runat="server">Mark</div>
            </div>
            <div style="margin-left:47%; background-color:lightgreen;width:15%;font-size:larger">
                <div><asp:Label ID="lblStudentId" runat="server"></asp:Label></div>
                <div><asp:Label ID="lblFile" runat="server"></asp:Label></div>
                <div style="display:none;"><asp:Label ID="lblFileName" runat="server" Text="Label"></asp:Label></div>
                <div><asp:Label ID="lblDate" runat="server"></asp:Label></div>
                <div><asp:Label ID="lblMark" runat="server"></asp:Label></div>
            </div>
            </div>
            <div style="margin-top:2%;margin-left:40%;"><asp:Button ID="btnDownload" runat="server" Text="Download File" CssClass="button" OnClick="btnDownload_Click" /></div>
        </div>

        <!-- ----------------------------------------------- Mark ------------------------------------------------------------------------ -->

        <div id="divMark" runat="server" style="margin-top:6%;margin-left:38%;border:ridge;width:20%;">
        <div style="float:left;font-weight:bold;margin-top:2%;margin-left:5%">
            Update the Points
        </div>
        <div style="margin-top:2%;margin-left:47%">
            <asp:TextBox ID="txtMark" runat="server" Width="30%" MaxLength="2" AutoCompleteType="Disabled" onkeypress="return onlyNumbers(this);"></asp:TextBox>
        </div>
        <div style="margin-top:2%;margin-left:5%; margin-bottom:5%"><asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button" OnClick="btnUpdate_Click" OnClientClick="return Checknumeric()" /></div>
        </div>

        <!-- ----------------------------------------------- Rating ------------------------------------------------------------------------ -->

        <div id="divRating" runat="server" style="margin-top:6%;">
            <div>
                <asp:GridView ID="grvRate" runat="server" Width="32%" AutoGenerateColumns="false" HorizontalAlign="Center" HeaderStyle-BackColor="#4CAF50" HeaderStyle-ForeColor="White">
                <Columns>
                <asp:TemplateField HeaderText="Sl.No" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                <asp:BoundField DataField="Description" HeaderText="Qualities" />
                <asp:TemplateField HeaderText="Point" HeaderStyle-Width="2%">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRate" runat="server" MaxLength="2" AutoCompleteType="Disabled" Text='<%# Eval("Mark") %>' onkeypress="return onlyNumbers(this);"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
                </asp:GridView>
            </div>
            <div style="margin-left:42%; margin-top:2%">
                <asp:Button ID="btnRateUpdate" runat="server" Text="Update" CssClass="button" OnClick="btnRateUpdate_Click" OnClientClick="return ValidateRateGrid()" />
            </div>
        </div>
        <div style="margin-left:35%; margin-top:2%;font-weight:bold;"><asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label></div>
    </div>
    </form>
</body>
</html>
