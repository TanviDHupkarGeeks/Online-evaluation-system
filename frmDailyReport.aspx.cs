using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineAssessment
{
    public partial class frmDailyReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadSubjects();
                    LoadAllDailyReport();
                }
            }
            catch
            { }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (docFile.HasFile)  //if file uploaded
            {
                string fileExt = System.IO.Path.GetExtension(docFile.FileName);
                string filename = string.Empty;

                if (checkFileType(fileExt))  //Check for file types
                {
                    try
                    {
                        filename = ddlSubjects.SelectedItem.Value + "_" + DateTime.Today.ToShortDateString() + fileExt;
                        //Save File
                        if (System.IO.File.Exists(Server.MapPath("~/uploadFiles/" + Session["Id"] + @"/" + filename)))
                            lblMessage.Text = "File Already Exists";
                        else
                        {
                            docFile.SaveAs(MapPath("~/uploadFiles/" + Session["Id"] + @"/" + filename));
                            lblMessage.Text = "File Uploaded Successfully";

                            // Send uploaded Assignment for Evaluation
                            SaveDailyReportforEvaluation(filename);

                            // Load Grid
                            LoadAllDailyReport();
                        }
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

        private void createDir(string fileName)
        {
            System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo(MapPath("~/uploadFiles/" + Session["Id"]));
            myDir.Create();

            //Save file
            docFile.SaveAs(MapPath("~/uploadFiles/" + Session["Id"] + @"/" + fileName));
            lblMessage.Text = "File Uploaded Successfully";
            btnUpload.Visible = false;

            SaveDailyReportforEvaluation(fileName);
        }

        private void SaveDailyReportforEvaluation(string fileName)
        {
            try
            {
                // Assignment_Id = 0 Means DailyReport

                SqlParameter[] parameterList = {
                    new SqlParameter("@Student_Id", Session["Id"].ToString()),
                    new SqlParameter("@Assignment_Id", (object)0),
                    new SqlParameter("@Subject_Id", ddlSubjects.SelectedItem.Value),
                    new SqlParameter("@FileName", fileName.Trim()),
                };

                DbConnection db = new DbConnection();
                int i = db.ExecuteNonQuery(CommandType.StoredProcedure, "usp_SaveFileforEvaluate", parameterList);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadAllDailyReport()
        {
            try
            {
                // Assignment_Id = 0 Means DailyReport

                SqlParameter[] parameterList = {
                    new SqlParameter("@Student_Id", Session["Id"].ToString())
                };

                DbConnection db = new DbConnection();
                DataSet ds = new DataSet();
                ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_EvaluateDailyReport", parameterList);
                grvDailyReport.DataSource = ds;
                grvDailyReport.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadSubjects()
        {
            DataSet ds = new DataSet();
            DbConnection db = new DbConnection();

            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadSubjects");

            foreach (DataRow dr in ds.Tables[0].Rows)
                ddlSubjects.Items.Add(new ListItem(dr["Subject"].ToString(), dr["Id"].ToString()));
        }
    }
}