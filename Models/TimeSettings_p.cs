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

namespace AdminPage.Models
{
    public partial class TimeSettings_p : Form
    {
        public string sqlConString { get; set; }
        public ValueTuple<string, int, int, int> S_TimeoutSettings_p { get; set; }
        public string S_TimeTable { get; set; }
        public List<string> S_TimeTableColumns { get; set; }
        public int counter = 0;

        public TimeSettings_p()
        {
            InitializeComponent();
        }



        public TimeSettings_p(string sqlConString, string timeTable, List<string> timeTableColumns)
        {
            InitializeComponent();
            this.sqlConString = sqlConString;
            this.S_TimeTable = timeTable;
            this.S_TimeTableColumns = timeTableColumns;
            if (COM_Port_1.Items.Count == 0)
            {
                for (int i = 1; i < 100; i++)
                {
                    COM_Port_1.Items.Add($"COM{i}");
                }
            }

            LoadTimeSettings();
        }

        internal void LoadTimeSettings()
        {
            string InsertSql = string.Empty;
            try
            {
                InsertSql = $"SELECT {S_TimeTableColumns[2]},{S_TimeTableColumns[3]} FROM {S_TimeTable} WHERE {S_TimeTableColumns[0]} = 'pressure';";
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
                                if (r.GetString(0).Equals(COM_Port_1.Name))
                                    COM_Port_1.Text = r.GetString(1);
                                else if (r.GetString(0).Equals(ConnectionTimeout_2.Name))
                                    ConnectionTimeout_2.Value = Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(DelayTime_3.Name))
                                    DelayTime_3.Value= Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(RetryCount_4.Name))
                                    RetryCount_4.Value = Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(ChartRefreshInterval_5.Name))
                                    ChartRefreshInterval_5.Value = Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(RealTimeDataQuery_6.Name))
                                    RealTimeDataQuery_6.Value = Convert.ToDecimal(r.GetValue(1));
                                else if (r.GetString(0).Equals(AvgDataQuery_7.Name))
                                    AvgDataQuery_7.Value = Convert.ToDecimal(r.GetValue(1));
                            }

                            //c_AvgQueryLimitTime.Value = Convert.ToDecimal(r[c_RetryTotal1.Name]);

                            //}

                        }
                        
                        S_TimeoutSettings_p = (COM_Port_1.Text, Convert.ToInt32(ConnectionTimeout_2.Value), Convert.ToInt32(DelayTime_3.Value), Convert.ToInt32(RetryCount_4.Value));
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            counter += 1;
            bool finished = false;
            try
            {
                string COM_PORT_Selected = "COM3";
                if (COM_Port_1.SelectedItem != null)
                {
                    COM_PORT_Selected = COM_Port_1.SelectedItem.ToString();
                }

                // { "sensorCategory", "settingCategory", "settingName", "settingValue", "settingLastChanged", "Remarks" }; 
                string LastChanged = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                List<string> S_TimeTableDataFormat = new List<string>() { "pressure", "categoryHere", "labelHere", "valueHere", LastChanged, "" };

                List<ValueTuple<string, string, string>> controls = new List<ValueTuple<string, string, string>> { ("collection",COM_Port_1.Name, COM_PORT_Selected),
                                                                                                    ("collection",ConnectionTimeout_2.Name, ConnectionTimeout_2.Value.ToString()),
                                                                                                    ("collection",DelayTime_3.Name, DelayTime_3.Value.ToString()),
                                                                                                    ("collection",RetryCount_4.Name, RetryCount_4.Value.ToString()),
                                                                                                    ("chart",ChartRefreshInterval_5.Name, ChartRefreshInterval_5.Value.ToString()),
                                                                                                    ("chart",RealTimeDataQuery_6.Name, RealTimeDataQuery_6.Value.ToString()),
                                                                                                    ("chart",AvgDataQuery_7.Name, AvgDataQuery_7.Value.ToString())};

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


        private void SaveButton_Click(object sender, EventArgs e)
        {
            counter += 1;
            bool finished = false;
            try
            {
                S_TimeoutSettings_p = (COM_Port_1.SelectedItem.ToString(), Convert.ToInt32(ConnectionTimeout_2.Value), Convert.ToInt32(DelayTime_3.Value), Convert.ToInt32(RetryCount_4.Value));

                string InsertSql = $"IF EXISTS(SELECT * FROM sysobjects " +
                                                $" WHERE name = '{S_TimeTable}' AND xtype = 'U')  INSERT INTO {S_TimeTable} VALUES('{S_TimeoutSettings_p.Item1}',{S_TimeoutSettings_p.Item2},{S_TimeoutSettings_p.Item3},{S_TimeoutSettings_p.Item4}, '');";
                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(InsertSql, con))
                    {
                        cmd.ExecuteNonQuery();
                        finished = true;
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
                msg = "Extra Settings Successfully Updated.";
            }
            else
            {
                msg = "Extra Settings Failed. Please, try again.";
                string sqlCmd = $"IF EXISTS(SELECT * FROM sysobjects " +
                                                $" WHERE name = '{S_TimeTable}' AND xtype = 'U') DROP TABLE {S_TimeTable};";
                List<string> TimeoutTableColumns = new List<string>() { "COM_PORT", "ConnectionTimeout", "DelayTime", "RetryCount", "Remarks" };

                string sqlCreateString = $"IF NOT EXISTS ( SELECT * FROM sysobjects " +
                                                    $" WHERE name = '{S_TimeTable}' AND xtype = 'U') " +
                                                    $"CREATE TABLE {S_TimeTable}(" +
                                                    $"{TimeoutTableColumns[0]} NVARCHAR(50) NOT NULL" +
                                                    $", {TimeoutTableColumns[1]} int NOT NULL" +
                                                    $", {TimeoutTableColumns[2]} int NOT NULL" +
                                                    $", {TimeoutTableColumns[3]} int NOT NULL" +
                                                    $", {TimeoutTableColumns[4]} NVARCHAR(50) NULL);";
                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
                    {
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = sqlCreateString;
                        cmd.ExecuteNonQuery();
                    }
                }



            }
            MessageBox.Show(msg, "Update status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (finished || counter > 3)
                this.Close();
        }

    }
}
