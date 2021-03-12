using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Data;

namespace AdminPage
{
    public class DownToExcel
    {

        /// <summary>
        /// SqlConnection.ConnectionString property
        /// </summary>
        public string sqlConString { get; set; }

        public string tableName { get; set; }
        public string dbName { get; set; }
        public SqlConnection myConn { get; set; }
        public (string, string) startEndTime { get; set; }

        public DownToExcel()
        {

        }
        public DownToExcel(string tbName, string sqlConStr, (string, string) StartEndTime)
        {
            tableName = tbName;
            myConn = new SqlConnection();
            sqlConString = sqlConStr;
            myConn.ConnectionString = sqlConStr;
            startEndTime = StartEndTime;
               
            if(myConn == null ||  myConn.ConnectionString == String.Empty)
            {
                myConn.ConnectionString = sqlConString;
                myConn.Open();
            }
            else if (myConn.State != System.Data.ConnectionState.Open)
            {
                myConn.Open();
            }

            DataSet ds = new DataSet();
            string sqlselect = $"SELECT * FROM {tableName} WHERE dateandtime >= '{startEndTime.Item1}' and dateandtime <= '{startEndTime.Item2}' ORDER BY dateandtime;";
            SqlCommand cmd = new SqlCommand(sqlselect, myConn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);

            ExportToExcel(ds.Tables[0]);
        }




        private void ExportToExcel(System.Data.DataTable ds)
        {
            //엑셀 저장 경로
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +
    "ExcelReport" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";

            try
            {
                var excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel._Worksheet)excelApp.ActiveSheet;

/*
                for (var i = 0; i < ds.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1] = ds.Columns[i].ColumnName;
                }

                for (var i = 0; i < ds.Rows.Count; i++)
                {
                    for (var j = 0; j < ds.Columns.Count; j++)
                    {
                        workSheet.Cells[i + 2, j + 1] = ds.Rows[i][j];
                    }
                }*/

                int rowCount = ds.Rows.Count;
                int columnCount = ds.Columns.Count;

                Excel.Range range = (Excel.Range)workSheet.Cells[1,1];
                range.Columns.AutoFit();
                //range = range.Resize(rowCount, columnCount);

                range.Value(Excel.XlRangeValueDataType.xlRangeValueDefault, ds.Rows);


                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                workSheet.SaveAs(path);
                excelApp.Quit();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

    }
}
