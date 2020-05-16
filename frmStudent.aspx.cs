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
    public partial class frmStudent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadSubjects();
                    LoadAssignmentsBySubject();
                }
            }
            catch
            {

            }
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

        private void LoadAssignmentsBySubject()
        {
            SqlParameter[] parameterList = {
                new SqlParameter("@Student_Id", Session["Id"].ToString()),
                new SqlParameter("@Subject_Id", ddlSubjects.SelectedItem.Value),
                new SqlParameter("@Status_Id", ddlStatus.SelectedItem.Value)
            };

            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();
            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadAssignment", parameterList);
            grvAssignment.DataSource = ds;
            grvAssignment.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAssignmentsBySubject();
        }
    }
}