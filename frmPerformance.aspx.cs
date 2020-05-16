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
    public partial class frmPerformance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSubjects();
                LoadReport();
            }
        }

        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReport();
        }

        protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadSubjects()
        {
            DataSet ds = new DataSet();
            DbConnection db = new DbConnection();

            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadSubjects");

            ddlSubjects.Items.Add(new ListItem("All", "0"));

            foreach (DataRow dr in ds.Tables[0].Rows)
                ddlSubjects.Items.Add(new ListItem(dr["Subject"].ToString(), dr["Id"].ToString()));
        }

        private void LoadOverallSkillReport(int StudentId, int SubjectId)
        {
            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();

            int LessThanValue = ToInt32(txtLessThan.Text.Trim());
            int GreaterThanValue = ToInt32(txtGreaterThan.Text.Trim());

            if (LessThanValue == 0 && GreaterThanValue != 0)
            {
                LessThanValue = GreaterThanValue;
                GreaterThanValue = 10;
            }

            if (LessThanValue != 0 && GreaterThanValue == 0)
            {
                GreaterThanValue = LessThanValue;
                LessThanValue = 0;
            }

            if (LessThanValue == 0 && GreaterThanValue == 0)
            {
                LessThanValue = 0;
                GreaterThanValue = 10;
            }

            SqlParameter[] parameterList = {
                new SqlParameter("@Student_Id", StudentId),
                new SqlParameter("@Subject_Id", SubjectId),
                new SqlParameter("@LessThan_Value", LessThanValue),
                new SqlParameter("@GreaterThan_Value", GreaterThanValue)
            };

            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadOverallSkillReport", parameterList);
            grvOverallSkillReport.DataSource = ds;
            grvOverallSkillReport.DataBind();

            if (ds.Tables[0].Rows.Count == 0)
                lblMessage.Text = "No Records Found";
            else
                lblMessage.Text = string.Empty;
        }

        private void LoadSkillReport(int StudentId, int SubjectId)
        {
            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();

            SqlParameter[] parameterList = {
                new SqlParameter("@Student_Id", StudentId),
                new SqlParameter("@Subject_Id", SubjectId)
            };

            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadSkillReport", parameterList);
            grvSkillReport.DataSource = ds;
            grvSkillReport.DataBind();

            if (ds.Tables[0].Rows.Count == 0)
                lblMessage.Text = "No Records Found";
            else
                lblMessage.Text = string.Empty;
        }

        private void LoadAssignmentReport(int TeacherId, int StudentId, int SubjectId)
        {
            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();
            int LessThanValue = ToInt32(txtLessThan.Text.Trim());
            int GreaterThanValue = ToInt32(txtGreaterThan.Text.Trim());

           

            if (LessThanValue == 0 && GreaterThanValue != 0)
            {
                LessThanValue = GreaterThanValue;
                GreaterThanValue = 100;
            }

            if (LessThanValue != 0 && GreaterThanValue == 0)
            {
                GreaterThanValue = LessThanValue;
                LessThanValue = 0;
            }

            SqlParameter[] parameterList = {
                new SqlParameter("@Teacher_Id", TeacherId),
                new SqlParameter("@Student_Id", StudentId),
                new SqlParameter("@Subject_Id", SubjectId),
                new SqlParameter("@LessThan_Value", LessThanValue),
                new SqlParameter("@GreaterThan_Value", GreaterThanValue)
            };

            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadAssignmentReport", parameterList);
            grvAssignment.DataSource = ds;
            grvAssignment.DataBind();

            if (ds.Tables[0].Rows.Count == 0)
                lblMessage.Text = "No Records Found";
            else
                lblMessage.Text = string.Empty;
        }


        protected void grvSkillReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int i = Int32.Parse(e.Row.Cells[2].Text);

                    if (i >= 1 && i <= 2)
                        AddImage(1, e);
                    else if (i >= 2 && i <= 4)
                        AddImage(2, e);
                    else if (i >= 4 && i <= 6)
                        AddImage(3, e);
                    else if (i >= 6 && i <= 8)
                        AddImage(4, e);
                    else if (i >= 8 && i <= 10)
                        AddImage(5, e);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void AddImage(int n, GridViewRowEventArgs e)
        {
            for (int i = 0; i < n; i++)
            {
                Image imgStar = new Image();
                imgStar.ImageUrl = "~/images/star.png";
                e.Row.Cells[3].Controls.Add(imgStar);
            }
        }

        private void LoadReport()
        {
            grvAssignment.Visible = false;

            if (Int32.Parse(ddlReport.SelectedValue) == 0)
            {
                LoadOverallSkillReport(Convert.ToInt32(Session["Id"]), Int32.Parse(ddlSubjects.SelectedValue));
                grvOverallSkillReport.Visible = true;

                LoadSkillReport(Convert.ToInt32(Session["Id"]), Int32.Parse(ddlSubjects.SelectedValue));
                grvSkillReport.Visible = true;
                divPointFilter.Attributes["class"] = "hidden";
            }
            else
            {
                LoadAssignmentReport(0, Convert.ToInt32(Session["Id"]), Int32.Parse(ddlSubjects.SelectedValue));
                divPointFilter.Attributes["class"] = "visible";
                grvOverallSkillReport.Visible = false;
                grvSkillReport.Visible = false;
                grvAssignment.Visible = true;
            }
        }

        public int ToInt32(string value)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            return int.Parse(value);
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            LoadReport();
        }
    }
}