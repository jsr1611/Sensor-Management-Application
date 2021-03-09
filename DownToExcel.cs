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

namespace AdminPage
{
    public class DownToExcel
    {
        public string tableName { get; set; }
        public string dbName { get; set; }
        public SqlConnection myConn { get; set; }
        public (string, string) startEndTime { get; set; }

        public DownToExcel()
        {

        }
        public DownToExcel(string tbName, SqlConnection sqlConnection, (string, string) StartEndTime)
        {
            tableName = tbName;
            myConn = sqlConnection;
            startEndTime = StartEndTime;

            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +
    "ExcelReport.xlsx";

            //The Excel file will be created on the desktop. If a previous file with the same name exists, it will be deleted first and then the new file is created.

            Excel.Application xlsApp;
            Excel.Workbook xlsWorkbook;
            Excel.Worksheet xlsWorksheet;
            object misValue = System.Reflection.Missing.Value;

            // Remove the old excel report file
            try
            {
                FileInfo oldFile = new FileInfo(fileName);
                if (oldFile.Exists)
                {
                    File.SetAttributes(oldFile.FullName, FileAttributes.Normal);
                    oldFile.Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing old Excel report: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            // We create the Excel file using code like this:
            try
            {
                xlsApp = new Excel.Application();
                xlsWorkbook = xlsApp.Workbooks.Add(misValue);
                xlsWorksheet = (Excel.Worksheet)xlsWorkbook.Sheets[1];

                // Create the header for Excel file
                xlsWorksheet.Cells[1, 1] = "Example of Excel report. Get data from pubs database, table authors";
                Excel.Range range = xlsWorksheet.get_Range("A1", "E1");
                range.Merge(1);
                range.Borders.Color = Color.Black.ToArgb();
                range.Interior.Color = Color.Yellow.ToArgb();
                dynamic dbschema = new JObject();


                //The next step is to export the table to an Excel file. This can be done with the following code:

                try
                {
                    if (myConn.State != System.Data.ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                }
                catch
                {
                    throw new Exception();
                }

                int i = 3;
                string sqlselect = $"SELECT * FROM {tableName} WHERE dateandtime >= '{startEndTime.Item1}' and dateandtime <= '{startEndTime.Item2}' ORDER BY dateandtime;";
                SqlCommand cmd = new SqlCommand(sqlselect, myConn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    for (int j = 0; j < dr.FieldCount; ++j)
                    {
                        xlsWorksheet.Cells[i, j + 1] = dr.GetName(j);
                    }
                    ++i;
                }

                while (dr.Read())
                {
                    for (int j = 1; j <= dr.FieldCount; ++j)
                        xlsWorksheet.Cells[i, j] = dr.GetValue(j - 1);
                    ++i;
                }


                range = xlsWorksheet.get_Range("A2", "I" + (i + 2).ToString());
                range.Columns.AutoFit();

                xlsWorkbook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookDefault, misValue, misValue,
                    misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, Excel.XlSaveConflictResolution.xlLocalSessionChanges,
                    misValue, misValue, misValue, misValue);
                xlsWorkbook.Close(true, misValue, misValue);
                xlsApp.Quit();

                ReleaseObject(xlsWorksheet);
                ReleaseObject(xlsWorkbook);
                ReleaseObject(xlsApp);


                if (MessageBox.Show("Excel report has been created on your desktop\nWould you like to open it?",
                    "Created Excel report",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) ==
                        DialogResult.Yes)
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating Excel report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }


        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
