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
    public partial class frmDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFileDetails();

                if (Request.QueryString["Eval"] == "D")
                {
                    LoadQualities(Int32.Parse(Request.QueryString["Id"]));
                    divMark.Attributes["class"] = "hidden";
                    divlblMark.Attributes["class"] = "hidden";
                }
                else
                    divRating.Attributes["class"] = "hidden";
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = lblFileName.Text;
                string filePath = Server.MapPath("~/uploadFiles/" + Request.QueryString["SId"] + @"/" + fileName);

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
            catch(Exception ex)
            {
                lblMessage.Text = ex.Message;
                Response.End();
                Response.Close();
            }
        }

        private void LoadFileDetails()
        {
            string strFileName = string.Empty;

            SqlParameter[] parameterList = {
                    new SqlParameter("@Id", Request.QueryString["Id"])
            };

            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();
            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadFileDetails", parameterList);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblStudentId.Text = ds.Tables[0].Rows[0]["Student_Id"].ToString();
                lblDate.Text = ds.Tables[0].Rows[0]["Sys_Date"].ToString();
                lblFile.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                lblFileName.Text = ds.Tables[0].Rows[0]["FileName"].ToString();
                lblMark.Text = ds.Tables[0].Rows[0]["Mark"].ToString();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] parameterList = {
                new SqlParameter("@Id", Request.QueryString["Id"]),
                new SqlParameter("@Mark", txtMark.Text.Trim())
            };

                DbConnection db = new DbConnection();
                int i = db.ExecuteNonQuery(CommandType.StoredProcedure, "usp_UpdateMark", parameterList);

                if (i > 0)
                {
                    LoadFileDetails();
                    lblMessage.Text = "Mark Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnRateUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Save all gridvalues at a time
                DbConnection db = new DbConnection();
                int i = db.ExecuteNonQuery(CommandType.StoredProcedure, "usp_UpdateRating", "@tvpNewRating", CreateRateTable());

                if (i > 0)
                    lblMessage.Text = "Mark Updated Successfully";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadQualities(int DailyReportId)
        {
            DbConnection db = new DbConnection();
            DataSet ds = new DataSet();

            SqlParameter[] parameterList = {
                new SqlParameter("@Id", DailyReportId)
            };

            ds = db.ExecuteQuery(CommandType.StoredProcedure, "usp_LoadQualities", parameterList);
            grvRate.DataSource = ds;
            grvRate.DataBind();
        }

        private DataTable CreateRateTable()
        {
            DataTable dtTable = new DataTable();
            dtTable.Columns.AddRange(new DataColumn[3] { new DataColumn("DailyReportId", typeof(int)),
                                                    new DataColumn("QualityId", typeof(int)),
                                                    new DataColumn("Mark",typeof(int)) });

            //  add each of the data rows to the table
            foreach (GridViewRow row in grvRate.Rows)
            {
                DataRow dr;
                dr = dtTable.NewRow();
                dr["DailyReportId"] = Request.QueryString["Id"];
                dr["QualityId"] = row.Cells[1].Text;
                dr["Mark"] = ((TextBox)row.Cells[3].FindControl("txtRate")).Text;
                dtTable.Rows.Add(dr);
            }
            return dtTable;
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}