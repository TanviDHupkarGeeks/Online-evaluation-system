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
    public partial class frmEvaluate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadSubjects();
                    LoadAllAssignment(Convert.ToInt32(Session["Id"]), Convert.ToInt32(ddlSubjects.SelectedValue));
                    LoadAllDailyReport();
                }
            }
            catch
            { }
        }

        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllAssignment(Convert.ToInt32(Session["Id"]), Int32.Parse(ddlSubjects.SelectedItem.Value));
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

        private void LoadAllAssignment(int TeacherId, int SubjectId)
        {
            DataSet ds = new DataSet();
            SqlParameter[] parameterList = {
                new SqlParameter("@Teacher_Id", TeacherId),
                new SqlParameter("@Subject_Id",  SubjectId),
            };

            DbConnection db = new DbConnection();
            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_EvaluateAssignment", parameterList);

            grvAssignment.DataSource = ds;
            grvAssignment.DataBind();
        }

        private void LoadAllDailyReport()
        {
            DataSet ds = new DataSet();
            SqlParameter[] parameterList = {
                new SqlParameter("@Student_Id", (object)0)
            };

            DbConnection db = new DbConnection();
            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_EvaluateDailyReport", parameterList);

            grvDailyReport.DataSource = ds;
            grvDailyReport.DataBind();
        }
    }
}