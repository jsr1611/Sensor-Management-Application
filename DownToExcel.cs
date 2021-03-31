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
        private string sqlConString { get; set; }

        private string tableName { get; set; }
        private List<string> tbName { get; set; }
        public string dbName { get; set; }
        private (string, string) startEndTime { get; set; }

        public DownToExcel()
        {

        }
        public DownToExcel(List<string> tbNames, string sqlConStr, (string, string) StartEndTime)
        {
            sqlConString = sqlConStr;
            startEndTime = StartEndTime;
            tbName = tbNames;

        }
        public void StartDownload()
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




            SqlConnection myConn = new SqlConnection(sqlConString);
            if (myConn.State != System.Data.ConnectionState.Open)
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
                sqlselect = $"SELECT TOP 100 * FROM {tableName} WHERE dateandtime >= '{startEndTime.Item1}' and dateandtime <= '{startEndTime.Item2}' ORDER BY dateandtime DESC;";
                SqlCommand cmd = new SqlCommand(sqlselect, myConn);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, tableName);
            }

            /*
                        foreach (Excel.Worksheet sheet in wb.Worksheets)
                        {
                            if (sheet.UsedRange.Count < 2)
                            {
                                sheet.Delete();
                            }
                        }
            */


            try
            {
                Console.WriteLine(wb.Sheets.Count);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                for (int index = 0; index < tbName.Count; index++)
                {

                    System.Data.DataTable dt = ds.Tables[index];
                    if (dt.Rows.Count == 0)
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

                    if (ws[index].Name.Contains("tUsage") || ws[index].Name.Contains("hUsage"))
                    {
                        ws[index].Columns[1].NumberFormat = "0.00";
                    }
                    else
                    {
                        ws[index].Columns[1].NumberFormat = "#,##0.00";
                    }

                    //ws[index].Columns[2].NumberFormat = "yyyy - MM - dd HH: mm: ss.SSS";

                    ws[index].Cells[1, 1].value = "ID";
                    ws[index].Cells[1, 2].value = tbName[index];
                    ws[index].Cells[1, 3].value = "DateAndTime";



                    /*                    var rngCelStr1 = (Excel.Range)ws[a].Cells[1];
                                        var rng1 = rngCelStr1.EntireColumn;
                                        rng1.NumberFormat = "0";

                                        var rngCelStr2 = (Excel.Range)ws[a].Cells[2];
                                        var rng2 = rngCelStr2.EntireColumn;
                                        rng2.NumberFormat = "yyyy-MM-dd HH:mm:ss.SSS";
                    */


                    ws[index].Range[ws[index].Cells[2, 1], ws[index].Cells[dt.Rows.Count + 1, dt.Columns.Count]].value = data;


                    //Excel.Range rangeofVals0 = ws[index].Cells[0];
                    //rangeofVals0.EntireColumn.NumberFormat = "#0";

                    if (ws[index].Name.Contains("tUsage") || ws[index].Name.Contains("hUsage"))
                    {
                        //ws[index].Columns[1].NumberFormat = "0.00";
                        Excel.Range rangeofVals = ws[index].Cells[1];
                        rangeofVals.EntireColumn.NumberFormat = "#0";
                    }
                    else
                    {
                        //ws[index].Columns[1].NumberFormat = "#,##0.00";
                        Excel.Range rangeofVals2 = ws[index].Cells[1];
                        rangeofVals2.EntireColumn.NumberFormat = "#,##0";
                    }

                    Excel.Range rangeofVals3 = ws[index].Cells[2];
                    rangeofVals3.EntireColumn.NumberFormat = "#,##0";
                    //ws[index].Columns[2].NumberFormat = "yyyy - MM - dd HH: mm: ss.SSS";

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
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }

        }

    }
}
