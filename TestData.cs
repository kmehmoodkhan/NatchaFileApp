using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace NatchaFileApp
{
    public class TestData
    {
        public DataSet GetData()
        {
            string filename = @"C:\Users\kausar\source\repos\NachaFile\NachaFile\bin\Debug\ach_ccd_entry_detail_record.xlsx";
            System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection(
                        "Provider=Microsoft.ACE.OLEDB.12.0; " +
                         "data source='" + filename + "';" +
                            "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\" ");
            myConnection.Open();
            DataTable mySheets = myConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            DataSet ds = new DataSet();
            DataTable dt;

            dt = makeDataTableFromSheetName(filename, mySheets.Rows[0]["TABLE_NAME"].ToString());
            ds.Tables.Add(dt);
            return ds;
        }

        private DataTable makeDataTableFromSheetName(string filename, string sheetName)
        {
            System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection(
            "Provider=Microsoft.ACE.OLEDB.12.0; " +
            "data source='" + filename + "';" +
            "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\" ");

            DataTable dtImport = new DataTable();
            System.Data.OleDb.OleDbDataAdapter myImportCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + sheetName + "]", myConnection);
            myImportCommand.Fill(dtImport);
            return dtImport;
        }
    }
}
