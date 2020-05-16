using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnlineAssessment
{
    public class DbConnection
    {
        #region Declaration
        private SqlConnection con;
        #endregion

        public void Open()
        {
            if (con != null && con.State == ConnectionState.Closed)
                con.Open();
            else
                Open(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
        }

        public void Open(string myConnection)
        {
            try
            {
                con = new SqlConnection(myConnection);
                con.Open();
            }
            catch
            {
                throw;
            }
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            int affectedRows = 0;
            try
            {
                Open();
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(commandParameters);
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                Close();
                Dispose();
            }
            return affectedRows;
        }

        public int ExecuteNonQueryWithOutputParameter(CommandType commandType, string commandText, SqlDbType outParamType, string outParamName, params SqlParameter[] commandParameters)
        {
            int affectedRows = 0;
            int Id = 0;
            try
            {
                Open();
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(commandParameters);
                cmd.Parameters.Add(outParamName, outParamType);
                cmd.Parameters[outParamName].Direction = ParameterDirection.Output;
                affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows == 1)
                    Id = Convert.ToInt32(cmd.Parameters[outParamName].Value);
            }
            catch
            {
                throw;
            }
            finally
            {
                Close();
                Dispose();
            }
            return Id;
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, string structuredParamName, DataTable structuredParamValue)
        {
            int affectedRows = 0;
            try
            {
                Open();
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.CommandType = commandType;

                SqlParameter paramStructured = new SqlParameter();
                paramStructured.SqlDbType = SqlDbType.Structured;
                paramStructured.ParameterName = structuredParamName;
                paramStructured.Value = structuredParamValue;
                cmd.Parameters.Add(paramStructured);

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                Close();
                Dispose();
            }
            return affectedRows;
        }

        public DataSet ExecuteQuery(CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            try
            {
                Open();
                SqlCommand cmd = new SqlCommand(commandText, con);
                DataSet ds = new DataSet();
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                Close();
                Dispose();
            }
        }

        public void Close()
        {
            con.Close();
        }

        public void Dispose()
        {
            con.Dispose();
        }
    }
}
