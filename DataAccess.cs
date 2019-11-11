using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatchaFileApp
{
    public class DataAccess
    {
        private readonly string connectionString = string.Empty;

        public DataAccess()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }

        public DataSet GetNachaData()
        {
          
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
           
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("proc_GetNachaFileData", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
            }
            catch (Exception x)
            {
                throw x;
            }
            return ds;
        }

        public DataSet RecordAudit(string id, string acctNum, DateTime acctDate, string acctRevNum)
        {
            var spName = "auditResultRecordAuditID";
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(spName, connection);
                command.CommandType = CommandType.StoredProcedure;
                AddParam(command, "aoID", SqlDbType.VarChar, id);
                AddParam(command, "AcctNum", SqlDbType.VarChar, acctNum);
                AddParam(command, "AcctDate", SqlDbType.SmallDateTime, acctDate);
                AddParam(command, "AcctRevNum", SqlDbType.VarChar, acctRevNum);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            return dataSet;
        }


        private static void AddParam(SqlCommand command, string name, SqlDbType type, object val)
        {
            var sqlParam = new SqlParameter(name, type);
            sqlParam.Value = val;
            command.Parameters.Add(sqlParam);
        }
    }
}
