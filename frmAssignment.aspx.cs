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
    public partial class frmAssignment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadSubjects();
                    LoadAllAssignment(Convert.ToInt32(Session["Id"]), Convert.ToInt32(ddlSubjects.SelectedValue));
                }
            }
            catch
            { }
        }

        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllAssignment(Convert.ToInt32(Session["Id"]), Int32.Parse(ddlSubjects.SelectedItem.Value.ToString()));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (docFile.HasFile)  //if file uploaded
                {
                    SqlParameter[] parameterList = {
                    new SqlParameter("@TeacherId", Session["Id"].ToString()),
                    new SqlParameter("@SubjectId", ddlSubjects.SelectedItem.Value.Trim()),
                    new SqlParameter("@Description", txtAssignment.Text.Trim()),
                    new SqlParameter("@EndDate", txtEndDate.Text.Trim())
                    };

                    DbConnection db = new DbConnection();
                    int Id = db.ExecuteNonQueryWithOutputParameter(CommandType.StoredProcedure, "usp_SaveAssignment", SqlDbType.BigInt, "@Assignment_Id", parameterList);

                    if (Id > 0)
                    {
                        string fileName = UploadAssignment(Id);
                        UpdateFileName(Id, fileName);
                        LoadAllAssignment(Convert.ToInt32(Session["Id"]), Convert.ToInt32(ddlSubjects.SelectedValue));
                    }
                }
                else
                {
                    lblMessage.Text = "Please choose the file";
                }
            }
            catch (SqlException ex)
            {
                // 2601 : Violation in unique index
                // 2627 : Violation in unique constraint
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    lblMessage.Text = "Assignment Name '" + txtAssignment.Text.Trim() + "' already Exists";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
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

        private void LoadAllAssignment(int TeacherId, int SubjectId)
        {
            DataSet ds = new DataSet();
            SqlParameter[] parameterList = {
                new SqlParameter("@TeacherId", TeacherId),
                new SqlParameter("@SubjectId",  SubjectId),
            };

            DbConnection db = new DbConnection();
            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadAllAssignment", parameterList);

            grvAssignment.DataSource = ds;
            grvAssignment.DataBind();
        }

        private string UploadAssignment(int AssignmentId)
        {
            string fileName = string.Empty;

            if (docFile.HasFile)  //if file uploaded
            {
                string fileExt = System.IO.Path.GetExtension(docFile.FileName);

                if (CheckFileType(fileExt))  //Check for file types
                {
                    try
                    {
                        //Save File
                        fileName = CreatefileName(AssignmentId, fileExt);
                        docFile.SaveAs(MapPath("~/uploadFiles/" + Session["Id"] + @"/" + fileName));
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        CreateDir(fileName);
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
            return fileName;
        }

        private void UpdateFileName(int AssignmentId, string fileName)
        {
            try
            {
                SqlParameter[] parameterList = {
                    new SqlParameter("@Id", AssignmentId),
                    new SqlParameter("@FileName", fileName),
                };

                DbConnection db = new DbConnection();
                int id = db.ExecuteNonQuery(CommandType.StoredProcedure, "usp_UpdateAssignmentFileName", parameterList);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private bool CheckFileType(string fileExtension)
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

        private string CreatefileName(int AssignmentId, string fileExt)
        {
            string fileName = txtAssignment.Text.Trim();

            if (fileName != string.Empty)
            {
                if (fileName.Length > 25)
                    fileName = fileName.Substring(0, 25);

                fileName = AssignmentId.ToString() + "_" + fileName + fileExt;
            }
            else
            {
                lblMessage.Text = "Please enter the fileName";
            }

            return fileName;
        }

        private void CreateDir(string fileName)
        {
            System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo(MapPath("~/uploadFiles/" + Session["Id"]));
            myDir.Create();

            //Save File
            docFile.SaveAs(MapPath("~/uploadFiles/" + Session["Id"] + @"/" + fileName));
        }
    }
}