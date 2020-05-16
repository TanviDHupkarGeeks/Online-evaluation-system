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
    public partial class frmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Session.RemoveAll();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                int RoleId = ValidateLogin(txtEmail.Text.Trim(), txtPassword.Text.Trim());
                if (RoleId == 1)
                    Response.Redirect("frmStudent.aspx");
                else if (RoleId == 2)
                    Response.Redirect("frmEvaluate.aspx");
                else
                    divMessage.Style.Add("display", "visible");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                divMessage.Style.Add("display", "visible");
            }
        }

        private int ValidateLogin(string Username, string Password)
        {
            DataSet ds = new DataSet();
            SqlParameter[] parameterList = {
                new SqlParameter("@Email", Username),
                new SqlParameter("@Pwd",  Password),
            };

            DbConnection db = new DbConnection();

            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_Login", parameterList);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["Id"] = ds.Tables[0].Rows[0]["Id"];
                return (int)ds.Tables[0].Rows[0]["Role"];
            }
            else
                return 0;
        }
    }
}