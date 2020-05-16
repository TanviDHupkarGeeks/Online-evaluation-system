<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUpload.aspx.cs" Inherits="OnlineAssessment.frmUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="css/navbarblue.css" rel="stylesheet" />
    <link href="css/onlineAssessment.css" rel="stylesheet" />
    <title>Upload</title>
    <script lang="javascript" type="text/javascript">
    var size = 2;
    var id= 0;
		
    function ProgressBar() 
    {
        if(document.getElementById('<%=docFile.ClientID %>').value != "")
        {
            document.getElementById("divProgress").style.display = "block";
            document.getElementById("divUpload").style.display = "block";
            id = setInterval("progress()", 20);
            return true;
        }
        else
        {
            alert("Select a file to upload");
            return false;
        }    
        
    }

    function progress()
    {
        size = size + 1;
        if(size > 299) 
        {
            clearTimeout(id);
         }
        document.getElementById("divProgress").style.width =  size + "pt";
        document.getElementById("<%=lblPercentage.ClientID %>").firstChild.data = parseInt(size / 3) +  "%";
    }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <ul>
              <li><a href="frmStudent.aspx" class="active">Assignment</a></li>
              <li><a href="frmDailyReport.aspx">Daily Report</a></li>
              <li style="float:right"><a href="frmLogin.aspx">Logout</a></li>
            </ul>
        </div>
        <div id="divHeader" runat="server" style="background-color:skyblue;width:100%;font-size:large;"></div>

        <!-- File Upload -->

    <div>
        <div style="margin-left:25%;margin-top:2%;">
            <div style="margin-left:5%;"><asp:Button ID="btnAssignmentDownload" runat="server" Text="Download Assignment Details" CssClass="button" OnClick="btnAssignmentDownload_Click" /></div>
        </div>
        <div id="divFileUpload" runat="server" style="margin-left:25%;margin-top:2%;">
            <div style="border-style: double;width:50%;">
                <div>Submit Assignment</div>
                <div style="float:left;margin-top:2%;margin-left:5%;"><asp:FileUpload ID="docFile" runat="server" /></div>
                <div style="margin-top:2%; margin-bottom:2%; margin-left:55%;">
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClientClick="return ProgressBar()" CssClass="button" OnClick="btnUpload_Click" />
                </div>
            </div>
        </div>
        <div style="margin-top:2%; font-weight:bold;"><asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label></div>
        <div id="divFileDownload" runat="server" style="margin-left:25%;margin-top:5%;">
            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="button" OnClick="btnDownload_Click" />
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
    </div>
    </form>
</body>
</html>
