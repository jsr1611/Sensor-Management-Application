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
        public DownToExcel(List<string> tbName, string sqlConStr, (string, string) StartEndTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            //엑셀 저장 경로
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Excelsheets\" +
    "ExcelReport " + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xlsx";

            //Create an excel instance
            Excel.Application excel = new Excel.Application();
            //excelApp.Workbooks.Add();

            //Add a workbook
            Excel.Workbook wb = excel.Workbooks.Add();
            //Excel._Worksheet workSheet = (Excel._Worksheet)excelApp.ActiveSheet;

            //Add a worksheet
            Excel.Worksheet[] ws = new Excel.Worksheet[tbName.Count];


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

            string sqlselect;
            DataSet ds = new DataSet();
            SqlDataAdapter da;


            for (int k = 0; k < tbName.Count; k++)
            {
                ws[k] = wb.Worksheets.Add();
                ws[k].Name = tbName[k];
                tableName = tbName[k];
                sqlselect = $"SELECT * FROM {tableName} WHERE dateandtime >= '{startEndTime.Item1}' and dateandtime <= '{startEndTime.Item2}' ORDER BY dateandtime;";
                SqlCommand cmd = new SqlCommand(sqlselect, myConn);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, tableName);
            }
            try
            {
               /*
                
                for (int k=0; k < tbName.Count; k++)
                {
                    ws[k] = wb.Worksheets.Add();
                    ws[k].Name = tbName[k];

                }*/
                //wb.Worksheets.Add(ws, ws1, ws1.Length);
                Console.WriteLine(wb.Sheets.Count);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                for (int a = 0; a < tbName.Count; a++)
                {
                    
                    System.Data.DataTable dt = ds.Tables[a];
                    if(dt.Rows.Count ==0)
                    {
                        continue;
                    }
                    int i = 0;
                    string[,] data = new string[dt.Rows.Count, dt.Columns.Count];
                    foreach (DataRow row in dt.Rows)
                    {
                        int j = 0;
                        foreach (DataColumn column in dt.Columns)
                        {
                            data[i, j] = row[column].ToString();
                            j++;
                        }
                        i++;
                    }


                    ws[a].Cells[1, 1].value = "ID";
                    ws[a].Cells[1, 2].value = tbName[a];
                    ws[a].Cells[1, 3].value = "DateAndTime";


                    if(ws[a].Name.Contains("tUsage") || ws[a].Name.Contains("hUsage"))
                    {
                        ws[a].Columns[1].NumberFormat = "0.00";
                    }
                    else
                    {
                        ws[a].Columns[1].NumberFormat = "#,##0";
                    }
                    
                    ws[a].Columns[2].NumberFormat = "yyyy - MM - dd HH: mm: ss.SSS";

/*                    var rngCelStr1 = (Excel.Range)ws[a].Cells[1];
                    var rng1 = rngCelStr1.EntireColumn;
                    rng1.NumberFormat = "0";

                    var rngCelStr2 = (Excel.Range)ws[a].Cells[2];
                    var rng2 = rngCelStr2.EntireColumn;
                    rng2.NumberFormat = "yyyy-MM-dd HH:mm:ss.SSS";
*/                    


                    ws[a].Range[ws[a].Cells[2, 1], ws[a].Cells[dt.Rows.Count + 1, dt.Columns.Count]].value = data;

                //ws[a].SaveAs(path);
                }
                excel.Columns.AutoFit();
                excel.Rows.AutoFit();
                //excel.Visible = true;

                wb.SaveAs(path);
                excel.Quit();
                stopwatch.Stop();

                MessageBox.Show($"Exporting SQL data to Excel was successful.\nElapsed time: {stopwatch.Elapsed}", "Finished", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }

        }

    }
}
