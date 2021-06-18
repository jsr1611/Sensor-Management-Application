using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminPage
{
    public partial class TimeSettings : Form
    {

        public string sqlConString { get; set; }
        public ValueTuple<int, int, int> S_TimeoutSettings { get; set; }
        public string S_TimeTable { get; set; }
        public List<string> S_TimeTableColumns { get; set; }
        public int counter = 0;

        public TimeSettings() { }

        public TimeSettings(string _sqlConString, string _TimeoutTable, List<string> _TimeTableColumns)
        {
            InitializeComponent();
            this.S_TimeTable = _TimeoutTable;
            this.sqlConString = _sqlConString;
            this.S_TimeTableColumns = _TimeTableColumns;

            DbTableHandler G_DBClass = new DbTableHandler();
            G_DBClass.sqlConString = sqlConString;

            S_TimeTableColumns = G_DBClass.GetTableColumnNames(S_TimeTable);
            LoadTimeSettings();
        }

        internal void LoadTimeSettings()
        {
            string InsertSql = string.Empty;
            try
            {
                int sharpOn = 0; // sharpOnTime.Checked ? 1 : 0;

                InsertSql = $"SELECT {S_TimeTableColumns[2]},{S_TimeTableColumns[3]} FROM {S_TimeTable} WHERE {S_TimeTableColumns[0]} = 'particle';";
                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(InsertSql, con))
                    {
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                /*if (r.FieldCount > 0 && r.HasRows)
                                {*/
                                if (r.GetString(0).Equals(c_RetryLimit1.Name))
                                    c_RetryLimit1.Value = Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(c_RetryTotal1.Name))
                                    c_RetryTotal1.Value = Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(c_sharpOnTime.Name))
                                    c_sharpOnTime.Checked = Convert.ToInt32(r.GetValue(1)) == 1;
                                else if (r.GetString(0).Equals(c_chartRefreshInterval.Name))
                                    c_chartRefreshInterval.Value = Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(c_RTQueryLimitTime.Name))
                                    c_RTQueryLimitTime.Value = Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(c_AvgQueryLimitTime.Name))
                                    c_AvgQueryLimitTime.Value = Convert.ToDecimal(r.GetValue(1));
                            }

                            //c_AvgQueryLimitTime.Value = Convert.ToDecimal(r[c_RetryTotal1.Name]);

                            //}

                        }
                        sharpOn = c_sharpOnTime.Checked ? 1 : 0;
                        S_TimeoutSettings = (Convert.ToInt32(c_RetryLimit1.Value), Convert.ToInt32(c_RetryTotal1.Value), sharpOn);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            counter += 1;
            bool finished = false;
            try
            {
                // { "sensorCategory", "settingCategory", "settingName", "settingValue", "settingLastChanged", "Remarks" }; 
                string LastChanged = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                List<string> S_TimeTableDataFormat = new List<string>() { "particle", "categoryHere", "labelHere", "valueHere", LastChanged, "" };

                int sharpOn = c_sharpOnTime.Checked ? 1 : 0;
                List<ValueTuple<string, string, string>> controls = new List<ValueTuple<string, string, string>> { ("collection", c_RetryLimit1.Name, c_RetryLimit1.Value.ToString()),
                                                                                                    ("collection", c_RetryTotal1.Name, c_RetryTotal1.Value.ToString()),
                                                                                                    ("collection", c_sharpOnTime.Name, sharpOn.ToString()),
                                                                                                    ("chart", c_chartRefreshInterval.Name, c_chartRefreshInterval.Value.ToString()),
                                                                                                    ("chart",c_RTQueryLimitTime.Name, c_RTQueryLimitTime.Value.ToString()),
                                                                                                    ("chart",c_AvgQueryLimitTime.Name, c_AvgQueryLimitTime.Value.ToString()) };

                for (int num = 0; num < controls.Count; num++)
                {
                    S_TimeTableDataFormat[1] = controls[num].Item1;
                    S_TimeTableDataFormat[2] = controls[num].Item2;
                    S_TimeTableDataFormat[3] = controls[num].Item3;

                    string sqlInsOrSel;
                    sqlInsOrSel = $"IF EXISTS(" +
                                                $"SELECT * FROM {S_TimeTable} " +
                                                $"WHERE " +
                                                    $"{S_TimeTableColumns[0]} = '{S_TimeTableDataFormat[0]}' AND  " +
                                                    $"{S_TimeTableColumns[1]} = '{S_TimeTableDataFormat[1]}' AND " +
                                                    $"{S_TimeTableColumns[2]} = '{S_TimeTableDataFormat[2]}' " +
                                            $") " +
                                        $" BEGIN " +
                                            $" UPDATE {S_TimeTable} " +
                                                $" SET {S_TimeTableColumns[3]} = '{S_TimeTableDataFormat[3]}', {S_TimeTableColumns[4]} = '{S_TimeTableDataFormat[4]}' " +
                                            $" WHERE " +
                                                $" {S_TimeTableColumns[0]} = '{S_TimeTableDataFormat[0]}' AND " +
                                                $" {S_TimeTableColumns[1]} = '{S_TimeTableDataFormat[1]}' AND " +
                                                $" {S_TimeTableColumns[2]} = '{S_TimeTableDataFormat[2]}' ";

                    sqlInsOrSel += $" END " +
                                $"ELSE " +
                                    $" BEGIN " +
                                        $"INSERT INTO {S_TimeTable} " +
                                        $"VALUES( ";

                    for (int i = 0; i < S_TimeTableColumns.Count - 1; i++)
                    {
                        sqlInsOrSel += $" '{S_TimeTableDataFormat[i]}', ";
                    }

                    sqlInsOrSel += " '' ) END;";
                    using (SqlConnection con = new SqlConnection(sqlConString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlInsOrSel, con))
                        {
                            cmd.ExecuteNonQuery();
                            finished = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            string msg = string.Empty;
            if (finished)
            {
                msg = "Time Settings Successfully Updated.";
            }
            else
            {
                msg = "Time Settings Failed.";
            }
            MessageBox.Show(msg, "Update status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (finished || counter > 3)
                this.Close();
        }
    }
}
