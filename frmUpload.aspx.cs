using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineAssessment
{
    public partial class frmUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string UploadStatus = Request.QueryString["Status"];
            if (UploadStatus.Trim() == "Pending")
            {
                divFileDownload.Attributes["class"] = "hidden";
                divHeader.InnerText = "Upload File";

                if (!ValidateEndDate())
                {
                    divFileUpload.Attributes["class"] = "hidden";
                    lblMessage.Text = "Last date ended for the evaluation";
                }
            }
            else
            {
                divFileUpload.Attributes["class"] = "hidden";
                divHeader.InnerText = "Download File";
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (docFile.HasFile)  //if file uploaded
            {
                string fileExt = Path.GetExtension(docFile.FileName);
                string filename = string.Empty;

                if (checkFileType(fileExt))  //Check for file types
                {
                    try
                    {
                        filename = Request.QueryString["Id"] + "_" + Request.QueryString["name"] + fileExt;
                        //save file
                        docFile.SaveAs(MapPath("~/uploadFiles/" + Session["Id"] + @"/" + filename));
                        lblMessage.Text = "File Uploaded Successfully";
                        divFileUpload.Attributes["class"] = "hidden";

                        // Send uploaded Assignment for Evaluation
                        SaveAssignmentforEvaluation(filename);
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        createDir(filename);
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid File Format";
                }
            }
            else
            {
                lblMessage.Text = "Please choose the file";
            }
        }

        private bool checkFileType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".doc":
                    return true;
                case ".docx":
                    return true;
                case ".txt":
                    return true;
                case ".rtf":
                    return true;
                default:
                    return false;
            }

        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = LoadFileName();
                string filePath = Server.MapPath("~/uploadFiles/" + Session["Id"] + @"/" + fileName);

                if (File.Exists(filePath))
                {
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + "");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
                else
                {
                    lblMessage.Text = "File Not Found. Please contact the Administrator";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Response.End();
                Response.Close();
            }
        }

        protected void btnAssignmentDownload_Click(object sender, EventArgs e)
        {
            string fileName = LoadAssignmentFileDetails().FileName;
            string teacherId = LoadAssignmentFileDetails().TeacherId;
            string filePath = Server.MapPath("~/uploadFiles/" + teacherId + @"/" + fileName);

            if ((fileName != string.Empty) && (File.Exists(filePath)))
            {
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + "");
                Response.TransmitFile(filePath);
                Response.End();
            }
            else
            {
                lblMessage.Text = "File Not Found. Please contact the Administrator";
            }
        }

        private void createDir(string fileName)
        {
            DirectoryInfo myDir = new DirectoryInfo(MapPath("~/uploadFiles/" + Session["Id"]));
            myDir.Create();

            //Save file
            docFile.SaveAs(MapPath("~/uploadFiles/" + Session["Id"] + @"/" + fileName));
            lblMessage.Text = "File Uploaded Successfully";
            btnUpload.Visible = false;

            SaveAssignmentforEvaluation(fileName);
        }

        private void SaveAssignmentforEvaluation(string fileName)
        {
            try
            {
                // Type 1 = Assignment
                // Type 2 = DailyReport

                SqlParameter[] parameterList = {
                    new SqlParameter("@Student_Id", Session["Id"].ToString()),
                    new SqlParameter("@Assignment_Id", Request.QueryString["Id"]),
                    new SqlParameter("@Subject_Id", (object)0),
                    new SqlParameter("@FileName", fileName.Trim())
                };

                DbConnection db = new DbConnection();
                int i = db.ExecuteNonQuery(CommandType.StoredProcedure, "usp_SaveFileforEvaluate", parameterList);
            }
            catch
            {

            }
        }

        private string LoadFileName()
        {
            string strFileName = string.Empty;

            SqlParameter[] parameterList = {
                    new SqlParameter("@Id", Request.QueryString["DId"]),
                    new SqlParameter("@Student_Id", Session["Id"].ToString()),
                    new SqlParameter("@Assignment_Id", Request.QueryString["Id"]),
            };

            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();
            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadFileName", parameterList);

            if (ds.Tables[0].Rows.Count > 0)
                strFileName = ds.Tables[0].Rows[0]["FileName"].ToString();

            return strFileName;
        }

        private AssignmentFileDetails LoadAssignmentFileDetails()
        {
            string strFileName = string.Empty;

            SqlParameter[] parameterList = {
                    new SqlParameter("@Id", Request.QueryString["Id"])
            };

            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();
            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadAssignmentFileName", parameterList);

            if (ds.Tables[0].Rows.Count > 0)
                strFileName = ds.Tables[0].Rows[0]["FileName"].ToString();

            AssignmentFileDetails FileDetails = new AssignmentFileDetails { TeacherId = ds.Tables[0].Rows[0]["Teacher_Id"].ToString(), 
                                    FileName = ds.Tables[0].Rows[0]["FileName"].ToString() };

            return FileDetails;
        }

        private bool ValidateEndDate()
        {
            DateTime Today = DateTime.Now.Date;
            DateTime EndDate = Today.Subtract(TimeSpan.FromDays(1));

            SqlParameter[] parameterList = {
                    new SqlParameter("@Id", Request.QueryString["Id"])
            };

            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();
            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadEndDate", parameterList);

            if (ds.Tables[0].Rows.Count > 0)
                EndDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["End_Date"].ToString());

            if (EndDate >= Today)
                return true;
            else
                return false;

        }
        public class AssignmentFileDetails
        {
            public string TeacherId { get; set; }
            public string FileName { get; set; }
        }
    }
}