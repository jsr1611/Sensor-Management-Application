﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using AdminPage.Properties;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using AdminPage.Models;

namespace AdminPage
{
    public partial class Form1 : Form
    {
        public string dbServerAddress = string.Empty;
        public string dbName = string.Empty; 
        public string dbUID = string.Empty; 
        public string dbPWD = string.Empty; 
        public string S_DeviceTable = string.Empty; 
        public string S_DeviceTable_p = string.Empty; 
        public string S_UsageTable = string.Empty; 
        public string S_UsageTable_p = string.Empty;
        public string S_DataTable { get; set; }
        public string S_DataTable_p { get; set; }

        public ValueTuple<string, string[]> S_SanghanHahanTable { get; set; }


        /// <summary>
        /// [0]: RetryLimit time, [1]: RetryTotal limit time, [2]: SharpOnTime (yes/no)
        /// </summary>
        public ValueTuple<int, int, int> S_TimeoutSettings { get; set; }
        public ValueTuple<int, int> S_TimeoutSettings_p { get; set; }
        public ValueTuple<string, bool> S_TimeTable { get; set; }
        public ValueTuple<string, bool> S_TimeTable_p { get; set; }



        /// <summary>
        /// [0]:sID, [1]:sName, [2]:sZone, [3]:sLocation, [4]:sDescription, [4]: sIPAddress, [5]: sPortNumber, [6]:sUsage 들어가 있음
        /// </summary>
        public List<string> S_DeviceInfoColumns { get; set; }

        /// <summary>
        /// [0]: higherLimit2, [1]: higherLimit1, [2]: lowerLimit1, [3]: lowerLimit2
        /// </summary>
        public List<string> S_FourRangeColumns { get; set; }
        public List<string> S_UsageTableColumns { get; set; }

        /// <summary>
        /// SqlConnection.ConnectionString 속성
        /// </summary>
        public string sqlConString { get; set; }
        public List<int> D_IDs { get; set; }
        public List<int> D_IDs_p { get; set; }

        public DateTime startTime { get; set; }

        /// <summary>
        /// (온습도 및 파티클 관련함)[0]: sName, [1]: sZone, [2]: sLocation, [3]: sDescription, [4]: sIPAddress, [5]: sPortNumber 정보가 들어가 있음.
        /// </summary>
        public List<TextBox> S_DeviceInfo { get; set; }


        /// <summary>
        /// (차압 관련함) [0]: sName, [1]: sZone, [2]: sLocation, [3]: sDescription 들이 들어가 있음.
        /// </summary>
        public List<TextBox> S_DeviceInfo_p { get; set; }

        public Dictionary<CheckBox, List<NumericUpDown>> S_UsageCheckerRangePairs { get; set; }

        public Dictionary<CheckBox, List<NumericUpDown>> S_UsageCheckerRangePairs_p { get; set; }

        public DbTableHandler g_DbTableHandler;

        public string appAddress = string.Empty; //@"C:\Program Files (x86)\DLIT Inc\Sensor Data Collection App\SensorData Collection Application.exe";
        public string appAddress2 = string.Empty;

        public string DataCollectionAppName { get; set; }
        public string DataCollectionAppName2 { get; set; }
        public bool appAlreadyRunning { get; set; }
        public bool appAlreadyRunning2 { get; set; }
        public Process applicationProcess { get; set; }
        public Process applicationProcess2 { get; set; }
        public int S_UsageCheckInterval { get; set; }
        public List<string> TimeTableColumns { get; set; }

        public string[] tableNamesKo = null;
        public string[] tableNamesKo_p = null;




        public Form1()
        {
            InitializeComponent();





            // ini 읽기 //////
            IniFile ini = new IniFile();
            try
            {
                ini.Load(AppInfo.StartupPath + "\\" + "Setting.ini");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading ini file. {ex.Message}. {ex.StackTrace}");
            }


            string D_IP = ini["DBSetting"]["IP"].ToString();
            string D_SERVERNAME = ini["DBSetting"]["SERVERNAME"].ToString();
            string D_NAME = ini["DBSetting"]["DBNAME"].ToString();
            string D_ID = ini["DBSetting"]["ID"].ToString();
            string D_PW = ini["DBSetting"]["PW"].ToString();
            string D_UsageCheckInterVal = ini["DBSetting"]["UsageCheckInterval"].ToString();


            string A_NAME1 = ini["DataCollectionAppSettings"]["APPNAME1"].ToString();
            string A_NAME2 = ini["DataCollectionAppSettings"]["APPNAME2"].ToString();
            string A_ADDRESS1 = ini["DataCollectionAppSettings"]["ADDRESS1"].ToString();
            string A_ADDRESS2 = ini["DataCollectionAppSettings"]["ADDRESS2"].ToString();
            //C:\Program Files (x86)\DLIT Inc\Sensor Data Collection App\SensorData Collection Application.exe";

            ////////////////////////////////////////////////////////





            dbServerAddress = D_SERVERNAME; // "localhost\\SQLEXPRESS";//"127.0.0.1";    //"10.1.55.174"; 
            //Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
            dbName = D_NAME; // "SensorData2";
            dbUID = D_ID; // "admin";
            dbPWD = D_PW; // "admin";
            S_SanghanHahanTable = (dbName[0] + "_SanghanHahan",
                                    new string[] { "sensorCategory"
                                                    , "sID"
                                                    , "settingCategory"
                                                    , "higherLimit2"
                                                    , "higherLimit1"
                                                    , "lowerLimit1"
                                                    , "lowerLimit2"
                                                    , "sUsage"
                                                    , "settingLastChanged"
                                                    , "Remarks"
                                    });

            S_DeviceTable = dbName[0] + "_DEVICES";
            S_DeviceTable_p = S_DeviceTable + "_p";
            S_UsageTable = dbName[0] + "_Usage";  // D_USAGETABLENAME; // "SensorUsage";
            S_UsageTable_p = S_UsageTable + "_p";

            int usageCHeck = 0;
            int.TryParse(D_UsageCheckInterVal, out usageCHeck);
            S_UsageCheckInterval = usageCHeck > 0 ? usageCHeck : 120;
            DataCollectionAppName = A_NAME1; //"SensorData Collection Application";
            DataCollectionAppName2 = A_NAME2; //"Pressure Data Collection App";
            appAddress = A_ADDRESS1;
            appAddress2 = A_ADDRESS2;


            applicationProcess = new Process();
            pTrackerTimer.Enabled = true;
            pTrackerTimer.Start();
            //applicationProcess = GetAppProcess(DataCollectionAppName);


            dateTimePicker1.Value = DateTime.Today.AddDays(-1);
            dateTimePicker1.Visible = true;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker2.Visible = true;

            dateTimePicker1_p.Value = DateTime.Today.AddDays(-1);
            dateTimePicker1_p.Visible = true;
            dateTimePicker2_p.Value = DateTime.Today;
            dateTimePicker2_p.Visible = true;

            S_DeviceInfo = new List<TextBox>() { sName, sZone, sLocation, sDescription, sIPAddress, sPortNumber };
            S_DeviceInfo_p = new List<TextBox>() { sName_p, sZone_p, sLocation_p, sDescription_p };
            S_DeviceInfoColumns = new List<string>() { sID.Name, S_DeviceInfo[0].Name, S_DeviceInfo[1].Name, S_DeviceInfo[2].Name, S_DeviceInfo[3].Name, S_DeviceInfo[4].Name, S_DeviceInfo[5].Name, "sUsage" };
            //S_FourRangeColumns = new List<string>() { R_RangeHigh2, R_RangeHigh1, R_RangeLow1, R_RangeLow2 };
            tableNamesKo = new string[] { "온도(°C)", "습도(%)", "파티클(0.1μm)", "파티클(0.3μm)", "파티클(0.5μm)", "파티클(1.0μm)", "파티클(3.0μm)", "파티클(5.0μm)", "파티클(10.0μm)", "파티클(25.0μm)" };



            // 온습도 및 파티클 센서 정보
            List<CheckBox> S_UsageCheckers = new List<CheckBox>() { temperature, humidity, particle01, particle03, particle05, particle10, particle30, particle50, particle100, particle250 };

            List<NumericUpDown> t_Ranges = new List<NumericUpDown>() { s_tHigherLimit2, s_tHigherLimit1, s_tLowerLimit1, s_tLowerLimit2 };
            List<NumericUpDown> h_Ranges = new List<NumericUpDown>() { s_hHigherLimit2, s_hHigherLimit1, s_hLowerLimit1, s_hLowerLimit2 };
            List<NumericUpDown> p01_Ranges = new List<NumericUpDown>() { s_p01HigherLimit2, s_p01HigherLimit1, s_p01LowerLimit1, s_p01LowerLimit2 };
            List<NumericUpDown> p03_Ranges = new List<NumericUpDown>() { s_p03HigherLimit2, s_p03HigherLimit1, s_p03LowerLimit1, s_p03LowerLimit2 };
            List<NumericUpDown> p05_Ranges = new List<NumericUpDown>() { s_p05HigherLimit2, s_p05HigherLimit1, s_p05LowerLimit1, s_p05LowerLimit2 };
            List<NumericUpDown> p10_Ranges = new List<NumericUpDown>() { s_p10HigherLimit2, s_p10HigherLimit1, s_p10LowerLimit1, s_p10LowerLimit2 };
            List<NumericUpDown> p30_Ranges = new List<NumericUpDown>() { s_p30HigherLimit2, s_p30HigherLimit1, s_p30LowerLimit1, s_p30LowerLimit2 };
            List<NumericUpDown> p50_Ranges = new List<NumericUpDown>() { s_p50HigherLimit2, s_p50HigherLimit1, s_p50LowerLimit1, s_p50LowerLimit2 };
            List<NumericUpDown> p100_Ranges = new List<NumericUpDown>() { s_p100HigherLimit2, s_p100HigherLimit1, s_p100LowerLimit1, s_p100LowerLimit2 };
            List<NumericUpDown> p250_Ranges = new List<NumericUpDown>() { s_p250HigherLimit2, s_p250HigherLimit1, s_p250LowerLimit1, s_p250LowerLimit2 };

            List<List<NumericUpDown>> S_Ranges = new List<List<NumericUpDown>>() { t_Ranges, h_Ranges, p01_Ranges, p03_Ranges, p05_Ranges, p10_Ranges, p30_Ranges, p50_Ranges, p100_Ranges, p250_Ranges };


            // 차압 센서 정보
            List<CheckBox> S_UsageCheckers_p = new List<CheckBox>() { pa, mbar, kpa, hpa, mmh2o, inchh2o, mmhg, inchhg };

            List<NumericUpDown> pa_Ranges = new List<NumericUpDown>() { s_paHigherLimit2, s_paHigherLimit1, s_paLowerLimit1, s_paLowerLimit2 };
            List<NumericUpDown> mbar_Ranges = new List<NumericUpDown>() { s_mbarHigherLimit2, s_mbarHigherLimit1, s_mbarLowerLimit1, s_mbarLowerLimit2 };
            List<NumericUpDown> kPa_Ranges = new List<NumericUpDown>() { s_kpaHigherLimit2, s_kpaHigherLimit1, s_kpaLowerLimit1, s_kpaLowerLimit2 };
            List<NumericUpDown> hPa_Ranges = new List<NumericUpDown>() { s_hpaHigherLimit2, s_hpaHigherLimit1, s_hpaLowerLimit1, s_hpaLowerLimit2 };
            List<NumericUpDown> mmH2O_Ranges = new List<NumericUpDown>() { s_mmh2oHigherLimit2, s_mmh2oHigherLimit1, s_mmh2oLowerLimit1, s_mmh2oLowerLimit2 };
            List<NumericUpDown> inchH2O_Ranges = new List<NumericUpDown>() { s_inchh2oHigherLimit2, s_inchh2oHigherLimit1, s_inchh2oLowerLimit1, s_inchh2oLowerLimit2 };
            List<NumericUpDown> mmHg_Ranges = new List<NumericUpDown>() { s_mmhgHigherLimit2, s_mmhgHigherLimit1, s_mmhgLowerLimit1, s_mmhgLowerLimit2 };
            List<NumericUpDown> inchHg_Ranges = new List<NumericUpDown>() { s_inchhgHigherLimit2, s_inchhgHigherLimit1, s_inchhgLowerLimit1, s_inchhgLowerLimit2 };

            List<List<NumericUpDown>> S_Ranges_p = new List<List<NumericUpDown>>() { pa_Ranges, mbar_Ranges, kPa_Ranges, hPa_Ranges, mmH2O_Ranges, inchH2O_Ranges, mmHg_Ranges, inchHg_Ranges };


            sqlConString = $@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20"; // ; Integrated Security=True ");
            SqlConnection myConn = new SqlConnection(sqlConString);

            g_DbTableHandler = new DbTableHandler(new List<string>() { dbServerAddress, dbName, dbUID, dbUID });

            g_DbTableHandler.sqlConString = sqlConString;
            g_DbTableHandler.S_DeviceInfoColumns = S_DeviceInfoColumns;
            g_DbTableHandler.S_DeviceTable = this.S_DeviceTable;


            // DB존재 여부 확인
            bool checkDbExists = g_DbTableHandler.IfDatabaseExists(dbName);
            if (!checkDbExists)
            {
                string sqlCreateDb = $@"CREATE DATABASE {dbName};";
                checkDbExists = g_DbTableHandler.CreateDatabase(dbName, sqlCreateDb);
            }


            if (checkDbExists)
            {
                try
                {
                    myConn.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while SQL connection: {ex.Message}. {ex.StackTrace}");
                    dbServerAddress = D_IP;
                    sqlConString = $@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20";
                    myConn = new SqlConnection(sqlConString);
                    try
                    {
                        myConn.Open();
                        Console.WriteLine("Connection Successful!");
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"Error while SQL connection 2: {ex2.Message}. {ex2.StackTrace}");
                    }
                }





                //S_FourRangeColumns = new List<string>() { R_RangeHigh2, R_RangeHigh1, R_RangeLow1, R_RangeLow2 };
                S_FourRangeColumns = CheckFourRangeNamesTable();
                S_DataTable = CheckDataTableName();
                S_DataTable_p = CheckDataTableName_p();
                S_TimeTable = CheckTimeTable("particle");
                S_TimeTable_p = CheckTimeTable("pressure");
                DataSet DeviceTable = GetDeviceInfo(dbName, S_DeviceTable);
                DataSet DeviceTable_p = GetDeviceInfo(dbName, S_DeviceTable_p);
                //S_UsageTableColumns = g_DbTableHandler.GetTableColumnNames(S_UsageTable);
                g_DbTableHandler.S_FourRangeColumns = S_FourRangeColumns;
                g_DbTableHandler.sqlConString = sqlConString;



                if (DeviceTable.Tables.Count > 0)
                {
                    D_IDs = new List<int>(DeviceTable.Tables[0].AsEnumerable().Where(r => r.Field<string>(S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]) == "YES").Select(r => r.Field<int>(S_DeviceInfoColumns[0])).ToList());

                    string[] rows = new string[DeviceTable.Tables[0].Columns.Count];

                    int num = 1;
                    foreach (DataRow row in DeviceTable.Tables[0].Rows)
                    {
                        ListViewItem listViewItem = new ListViewItem(num.ToString());
                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            if (i < 5 || i > 6)
                                listViewItem.SubItems.Add(row.ItemArray[i].ToString());
                            //listView1_thp.Columns[i].TextAlign = HorizontalAlignment.Center;
                        }
                        listView1_thp.Items.Add(listViewItem);
                        num += 1;
                    }
                    listView1_thp.Scrollable = true;
                    listView1_thp.Sort();

                }
                else
                {
                    //MessageBox.Show("프로그램 다시 실행해 주세요!", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // if User selects not to create  new DB
                }

                if (DeviceTable_p.Tables.Count > 0)
                {
                    D_IDs_p = new List<int>(DeviceTable_p.Tables[0].AsEnumerable().Where(r => r.Field<string>(S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]) == "YES").Select(r => r.Field<int>(S_DeviceInfoColumns[0])).ToList());

                    //ModBus and myConnection initialization
                    //ConnectionSettings(true);


                    string[] rows = new string[DeviceTable_p.Tables[0].Columns.Count];

                    int num = 1;
                    foreach (DataRow row in DeviceTable_p.Tables[0].Rows)
                    {
                        ListViewItem listViewItem = new ListViewItem(num.ToString());
                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            listViewItem.SubItems.Add(row.ItemArray[i].ToString());
                            //listView1_thp.Columns[i].TextAlign = HorizontalAlignment.Center;
                        }
                        listView2_pressure.Items.Add(listViewItem);
                        num += 1;
                    }
                    listView2_pressure.Scrollable = true;
                    listView2_pressure.Sort();
                }
                else { }



                S_UsageCheckerRangePairs = new Dictionary<CheckBox, List<NumericUpDown>>();

                for (int i = 0; i < S_UsageCheckers.Count; i++)
                {
                    S_UsageCheckerRangePairs.Add(S_UsageCheckers[i], S_Ranges[i]);
                }

                S_UsageCheckerRangePairs_p = new Dictionary<CheckBox, List<NumericUpDown>>();

                for (int i = 0; i < S_UsageCheckers_p.Count; i++)
                {
                    S_UsageCheckerRangePairs_p.Add(S_UsageCheckers_p[i], S_Ranges_p[i]);
                }


                startTime = DateTime.Now;

                clearFields(S_DeviceInfo);
                clearFields(S_DeviceInfo_p);
            }

        }

        private string CheckDataTableName_p()
        {
            List<string> S_DTColumns = new List<string>() { "DateAndTime", "sID", "sCode", "sDataValue", "Remarks" };
            string tbName = S_DeviceTable[0] + "_DATATABLE_p";
            string sqlCreateTb = $"IF NOT EXISTS ( SELECT * FROM sysobjects " +
                                                $" WHERE name = '{tbName}' AND xtype = 'U') " +
                                                $"CREATE TABLE {tbName}( " +
                                                    $" {S_DTColumns[0]} NVARCHAR(25) NOT NULL" +
                                                    $", {S_DTColumns[1]} INT NOT NULL" +
                                                    $", {S_DTColumns[2]} NVARCHAR(25) NOT NULL" +
                                                    $", {S_DTColumns[3]} NVARCHAR(25) NOT NULL" +
                                                    $", {S_DTColumns[4]} NVARCHAR(255) NULL" +
                                                    $", INDEX IX_{S_DTColumns[0]} NONCLUSTERED({S_DTColumns[0]})" +
                                                    $", INDEX IX_{S_DTColumns[1]} NONCLUSTERED({S_DTColumns[2]})" +
                                                    $", INDEX IX_{S_DTColumns[2]} NONCLUSTERED({S_DTColumns[3]}))";
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlCreateTb, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating a table: {tbName} {ex.Message} {ex.StackTrace}");
            }

            return tbName;
        }

        private (string, bool) CheckTimeTable(string sensorCategory)
        {
            string tbName = S_DeviceTable[0] + "_TimeSettings";
            bool dataExists = false;
            string sqlString = string.Empty;
            TimeTableColumns = new List<string>() { "sensorCategory", "settingCategory", "settingName", "settingValue", "settingLastChanged", "Remarks" };

            try
            {

                bool tbExists = g_DbTableHandler.IfTableExists(tbName);
                if (!tbExists)
                {


                    sqlString = $"IF NOT EXISTS ( SELECT * FROM sysobjects " +
                                                        $" WHERE name = '{tbName}' AND xtype = 'U') " +
                                                        $"CREATE TABLE {tbName}( ";

                    for (int i = 0; i < TimeTableColumns.Count - 1; i++)
                    {
                        sqlString += $" {TimeTableColumns[i]} NVARCHAR (50) NOT NULL ,";

                    }
                    sqlString += $" {TimeTableColumns[TimeTableColumns.Count - 1]} NVARCHAR(255) NULL)";

                    using (SqlConnection con = new SqlConnection(sqlConString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlString, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                else
                {
                    sqlString = $"SELECT COUNT(*) FROM {tbName} WHERE {TimeTableColumns[0]} = '{sensorCategory}';";
                    using (SqlConnection con = new SqlConnection(sqlConString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlString, con))
                        {
                            using (SqlDataReader r = cmd.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    if (Convert.ToInt32(r.GetValue(0)) > 0)
                                    {
                                        dataExists = true;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating the '{tbName}' table. \n{ex.Message}. \n{ex.StackTrace}");
            }

            return (tbName, dataExists);
        }



        /// <summary>
        /// 데이터 저장용 테이블 생성 및 테이블명 반환
        /// </summary>
        /// <returns>데이터가 저장이 되는 테이블명</returns>
        private string CheckDataTableName()
        {
            List<string> S_DTColumns = new List<string>() { "DateAndTime", "sDateTime", "sID", "sCode", "sDataValue", "Remarks" };
            string tbName = S_DeviceTable[0] + "_DATATABLE";
            string sqlCreateTb = $"IF NOT EXISTS ( SELECT * FROM sysobjects " +
                                                $" WHERE name = '{tbName}' AND xtype = 'U') " +
                                                $"CREATE TABLE {tbName}( " +
                                                    $" {S_DTColumns[0]} NVARCHAR(25) NOT NULL" +
                                                    $", {S_DTColumns[1]} NVARCHAR(25) NULL" +
                                                    $", {S_DTColumns[2]} INT NOT NULL" +
                                                    $", {S_DTColumns[3]} NVARCHAR(25) NOT NULL" +
                                                    $", {S_DTColumns[4]} NVARCHAR(25) NOT NULL" +
                                                    $", {S_DTColumns[5]} NVARCHAR(255) NULL" +
                                                    $", INDEX IX_{S_DTColumns[0]} NONCLUSTERED({S_DTColumns[0]})" +
                                                    $", INDEX IX_{S_DTColumns[1]} NONCLUSTERED({S_DTColumns[1]})" +
                                                    $", INDEX IX_{S_DTColumns[2]} NONCLUSTERED({S_DTColumns[2]})" +
                                                    $", INDEX IX_{S_DTColumns[3]} NONCLUSTERED({S_DTColumns[3]}))";
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlCreateTb, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating a table: {tbName} {ex.Message} {ex.StackTrace}");
            }

            return tbName;
        }

        private List<string> CheckFourRangeNamesTable()
        {
            string tbName = S_DeviceTable[0] + "_FOUR_RANGES";
            List<string> fourRanges = new List<string>();
            List<string> _fourRanges = new List<string>() { "higherlimit2", "higherlimit1", "lowerlimit1", "lowerlimit2" };

            string sqlCreateIfNotExists = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tbName}' and xtype='U')" +
                                                $"CREATE TABLE {tbName}(" +
                                                $" {_fourRanges[0]} NVARCHAR(50) NOT NULL" +
                                                $", {_fourRanges[1]} NVARCHAR(50) NOT NULL" +
                                                $", {_fourRanges[2]} NVARCHAR(50) NOT NULL" +
                                                $", {_fourRanges[3]} NVARCHAR(50) NOT NULL) " +
                                            $" ELSE " +
                                                $" BEGIN SELECT * FROM {tbName} END;";
            try
            {

                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlCreateIfNotExists, con))
                    {
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                if (r.HasRows && r.FieldCount == 4)
                                {
                                    fourRanges.Add(r.GetValue(0).ToString());
                                    fourRanges.Add(r.GetValue(1).ToString());
                                    fourRanges.Add(r.GetValue(2).ToString());
                                    fourRanges.Add(r.GetValue(3).ToString());
                                    break;
                                }
                            }
                        }
                        if (fourRanges.Count == 0)
                        {
                            sqlCreateIfNotExists = $"INSERT INTO {tbName} VALUES ('{_fourRanges[0]}', '{_fourRanges[1]}', '{_fourRanges[2]}', '{_fourRanges[3]}'); SELECT * FROM {tbName};";
                            cmd.CommandText = sqlCreateIfNotExists;
                            using (SqlDataReader r = cmd.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    if (r.HasRows && r.FieldCount == 4)
                                    {
                                        fourRanges.Add(r.GetValue(0).ToString());
                                        fourRanges.Add(r.GetValue(1).ToString());
                                        fourRanges.Add(r.GetValue(2).ToString());
                                        fourRanges.Add(r.GetValue(3).ToString());
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while getting FourRangesColumnNames: {ex.Message} {ex.StackTrace}");
            }

            return fourRanges;
        }


        /// <summary>
        /// Application Process를 반환함
        /// </summary>
        /// <param name="dataCollectionAppName"></param>
        /// <param name="SomeAppAlreadyRunning"></param>
        /// <returns></returns>
        public Process GetAppProcess(string dataCollectionAppName, bool SomeAppAlreadyRunning)
        {
            Process SomeAppProcess = null;
            int myCounter = 0;
            try
            {

                while (myCounter < 2)
                {
                    //dataCollectionAppName = "SensorData Collection Application";
                    Process[] processes = Process.GetProcessesByName(dataCollectionAppName);
                    if (processes.Length != 0)
                    {
                        SomeAppAlreadyRunning = true;
                        b_dataCollection_status.Image = Resources.light_on_26_color;
                        SomeAppProcess = processes[0];
                        break;
                    }
                    else
                    {
                        myCounter += 1;
                        //MessageBox.Show("찾으신 어플리케이션의 정확한 이름을 찾아서 입력해 주세요!", "Application Status", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAppProcess Function Error. {ex.Message}. {ex.StackTrace}");
            }
            return SomeAppProcess;
        }



        /// <summary>
        /// 센서 장비 테이블에 있는 모든 장비에 대한 ID를 List<int> 형태로 불러오는 함수
        /// </summary>
        /// <param name="S_IDs"></param>
        /// <returns></returns>
        private List<int> GetSensorIDs(List<int> S_IDs)
        {
            string DeviceTable = tabControl1.SelectedTab == tabPage1 ? S_DeviceTable : S_DeviceTable_p;
            string IdCheckCmd = $"SELECT {S_DeviceInfoColumns[0]} FROM {dbName}.dbo.{DeviceTable} WHERE {S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]}='YES'";
            //Console.WriteLine("Usable sensor IDs:");
            using (SqlConnection myConn = new SqlConnection(sqlConString))
            {
                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    using (SqlCommand sqlCommand = new SqlCommand(IdCheckCmd, myConn))
                    {
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                S_IDs.Add(Convert.ToInt32(sqlDataReader[$"{S_DeviceInfoColumns[0]}"]));
                                //Console.WriteLine(Convert.ToInt32(sqlDataReader["sID"]));
                            }
                            sqlDataReader.Close();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return S_IDs;
        }

        /// <summary>
        /// 주어진 센서 ID가 SENSOR_INFO테이블에 있는지 확인하고 bool형태의 값을 반환해주는 함수
        /// </summary>
        /// <param name="id">센서 ID</param>
        /// <returns>return true or false </returns>
        private bool GetSensorID(int id)
        {
            string DeviceTable = tabControl1.SelectedTab == tabPage1 ? S_DeviceTable : S_DeviceTable_p;
            bool idExists = false;
            string sqlStrChecker = $"SELECT {S_DeviceInfoColumns[0]} FROM [{DeviceTable}] WHERE {S_DeviceInfoColumns[0]} = {id};";
            using (SqlConnection myConn = new SqlConnection(sqlConString))
            {
                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    using (SqlCommand sqlIdCheckerCmd = new SqlCommand(sqlStrChecker, myConn))
                    {
                        using (SqlDataReader reader = sqlIdCheckerCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (Convert.ToInt32(reader[$"{S_DeviceInfoColumns[0]}"].ToString()) == id)
                                {
                                    idExists = true;
                                    break;
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return idExists;
        }



        /// <summary>
        /// 주어진 센서 ID가 주어진 table에 있는지 확인하고 bool형태의 값을 반환해주는 함수
        /// </summary>
        /// <param name="id">센서 ID</param>
        /// <param name="tableName">테이블명</param>
        /// <returns>return true or false </returns>
        private bool GetSensorID(int id, string tableName)
        {
            bool idExists = false;
            string sqlStrChecker = $"SELECT {S_DeviceInfoColumns[0]} FROM {tableName} WHERE {S_DeviceInfoColumns[0]} = {id};";

            using (SqlConnection myConn = new SqlConnection($@"Data Source={dbServerAddress};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20"))
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                using (SqlCommand sqlIdCheckerCmd = new SqlCommand(sqlStrChecker, myConn))
                {
                    try
                    {
                        using (SqlDataReader reader = sqlIdCheckerCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idExists = Convert.ToInt32(reader[$"{S_DeviceInfoColumns[0]}"].ToString()) == id;
                                break;
                            }
                            reader.Close();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return idExists;
        }



        /// <summary>
        /// SENSOR_INFO테이블에 있는 모든 정보를 DataSet형태로 불러오는 함수
        /// </summary>
        /// <returns></returns>
        private DataSet GetDeviceInfo(string sensorData_dbName, string Devices_tbName)
        {
            // Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
            //SqlConnection myConn_master = new SqlConnection($@"Data Source = {DbServer};Initial Catalog=master;Trusted_Connection=True");
            DataSet ds = new DataSet();
            bool checkDbExists = g_DbTableHandler.IfDatabaseExists(sensorData_dbName);


            string sqlCreateDeviceTb = $"IF NOT EXISTS ( SELECT * FROM sysobjects " +
                                                        $"WHERE name = '{Devices_tbName}' and xtype = 'U' ) " +
                                                            $"CREATE TABLE {Devices_tbName} (" +
                                                            $"{S_DeviceInfoColumns[0]} INT NOT NULL" +
                                                            $", CONSTRAINT PK_{Devices_tbName}_{S_DeviceInfoColumns[0]} PRIMARY KEY ({S_DeviceInfoColumns[0]}) ";

            //  S_DeviceInfo_txtB 크기만큼은 loop를 통해 스트링에 추가
            for (int i = 1; i < S_DeviceInfoColumns.Count - 1; i++)
            {
                if (Devices_tbName.Contains("_p") && (i < 5 || i > 6))
                    sqlCreateDeviceTb += $", {S_DeviceInfoColumns[i]} NVARCHAR(250) NULL ";
                else if (!Devices_tbName.Contains("_p"))
                    sqlCreateDeviceTb += $", {S_DeviceInfoColumns[i]} NVARCHAR(250) NULL ";
            }
            sqlCreateDeviceTb += $",{S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} NVARCHAR(20) NOT NULL, INDEX IX_{S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} NONCLUSTERED({S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]})); ";

            if (checkDbExists)
            {
                bool Check_Device_tableExists = g_DbTableHandler.IfTableExists(Devices_tbName);
                if (Check_Device_tableExists)
                {
                    string sqlStr = $"SELECT * FROM {Devices_tbName} ORDER BY {S_DeviceInfoColumns[0]}";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(sqlConString))
                        {
                            con.Open();
                            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlStr, con))
                            {
                                sqlDataAdapter.Fill(ds);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlConString = $@"Data Source={dbServerAddress};Initial Catalog={dbName};Integrated Security=True";
                        g_DbTableHandler.sqlConString = sqlConString;
                        try
                        {
                            using (SqlConnection con = new SqlConnection(sqlConString))
                            {
                                con.Open();
                                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlStr, con))
                                {
                                    sqlDataAdapter.Fill(ds);
                                }
                            }
                        }
                        catch (System.Exception ex_1)
                        {
                            MessageBox.Show(ex_1.Message + ex_1.StackTrace, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                }
                else
                {
                    bool Device_tableCreated = g_DbTableHandler.CreateTable(dbName, Devices_tbName, sqlCreateDeviceTb);
                    if (!Device_tableCreated)
                    {
                        MessageBox.Show($"DB가 생성되었지만, 센서 정보 테이블이 성공적으로 생성되지 않았습니다!\nDB명 = {dbName}\n센서 정보 테이블명 = {Devices_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                string sqlCreateDb = $@"CREATE DATABASE {dbName};";


                bool dataBase_Created = g_DbTableHandler.CreateDatabase(dbName, sqlCreateDb);
                if (dataBase_Created)
                {
                    bool Device_tableCreated = g_DbTableHandler.CreateTable(dbName, Devices_tbName, sqlCreateDeviceTb);
                    if (!Device_tableCreated)
                    {
                        MessageBox.Show($"DB가 생성되었지만, 센서 정보 테이블이 성공적으로 생성되지 않았습니다!\nDB명 = {dbName}\n센서 정보 테이블명 = {Devices_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show($"센서 정보 DB가 성공적으로 생성되지 않았습니다!\nDB명 = {dbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return ds;
        }


        /// <summary>
        /// SENSOR_INFO테이블에 있는 특정 센서 장비 정보를 DataSet형태로 불러오는 함수
        /// </summary>
        /// <param name="sensorData_dbName">DB명</param>
        /// <param name="Devices_tbName">Table명</param>
        /// <param name="sensorId">센서장비 ID번호</param>
        /// <returns>DataSet형태의 반환값</returns>
        private DataSet GetDeviceInfo(string sensorData_dbName, string Devices_tbName, int sensorId)
        {
            DataSet ds = new DataSet();
            bool checkDbExists = g_DbTableHandler.IfDatabaseExists(sensorData_dbName);

            if (checkDbExists)
            {
                bool CheckDeviceTableExists = g_DbTableHandler.IfTableExists(Devices_tbName);
                if (CheckDeviceTableExists)
                {
                    string sqlStr = $"SELECT * FROM {Devices_tbName} WHERE {S_DeviceInfoColumns[0]} = '{sensorId}';";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(sqlConString))
                        {
                            con.Open();
                            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlStr, con))
                            {
                                sqlDataAdapter.Fill(ds);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }

            }

            return ds;
        }


        /// <summary>
        /// 데이터 수집 시작 센서 제어 부문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_start_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                if (!appAlreadyRunning)
                {

                    /*dataCollectionApp = FlaUI.Core.Application.Launch(appAddress);
                    using (var automation = new UIA3Automation())
                    {
                        var window = dataCollectionApp.GetMainWindow(automation);
                        //MessageBox.Show("Hello, " + window.Title, window.Title);

                    }*/
                    try
                    {
                        Process.Start(appAddress);
                        b_dataCollection_status.Image = Resources.light_on_26_color;
                        applicationProcess = GetAppProcess(DataCollectionAppName, appAlreadyRunning);
                        appAlreadyRunning = true;
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("데이터 수집 프로그램이 컴퓨터에 설치되어 있는지 확인 후 다시 실행해 주세요.", "Application Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        /*                    appAddress = @"C:\Program Files\DLIT Inc\Sensor Data Collection App\SensorData Collection Application.exe";
                                            Process.Start(appAddress);*/
                        //string[] filePaths = System.IO.Directory.GetFiles(@"C:\Program Files\DLIT Inc\", "SensorData Collection Application.exe", SearchOption.TopDirectoryOnly);
                        /*appAddress = @"C:\Users\" + System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1] + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\DLIT Inc\Sensor Data Collection App";
                        Process.Start(appAddress);
                        b_dataCollection_status.Image = Resources.light_on_26_color;
                        applicationProcess = GetAppProcess(DataCollectionAppName);
                        appAlreadyRunning = true;*/
                    }
                }
                else
                {
                    MessageBox.Show("데이터 수집 프로그램이 이미 실행중입니다.", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                if (!appAlreadyRunning2)
                {

                    /*dataCollectionApp = FlaUI.Core.Application.Launch(appAddress);
                    using (var automation = new UIA3Automation())
                    {
                        var window = dataCollectionApp.GetMainWindow(automation);
                        //MessageBox.Show("Hello, " + window.Title, window.Title);

                    }*/
                    try
                    {
                        Process.Start(appAddress2);
                        b_dataCollection_status.Image = Resources.light_on_26_color;
                        applicationProcess2 = GetAppProcess(DataCollectionAppName2, appAlreadyRunning2);
                        appAlreadyRunning2 = true;
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("데이터 수집 프로그램이 컴퓨터에 설치되어 있는지 확인후 다시 실행해 주세요.", "Application Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        /*                    appAddress = @"C:\Program Files\DLIT Inc\Sensor Data Collection App\SensorData Collection Application.exe";
                                            Process.Start(appAddress);*/
                        //string[] filePaths = System.IO.Directory.GetFiles(@"C:\Program Files\DLIT Inc\", "SensorData Collection Application.exe", SearchOption.TopDirectoryOnly);
                        /*appAddress = @"C:\Users\" + System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1] + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\DLIT Inc\Sensor Data Collection App";
                        Process.Start(appAddress);
                        b_dataCollection_status.Image = Resources.light_on_26_color;
                        applicationProcess = GetAppProcess(DataCollectionAppName);
                        appAlreadyRunning = true;*/
                    }
                }
                else
                {
                    MessageBox.Show("차압 센서 데이터 수집 프로그램은 아직 구현이 안되어 있어요!", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }



        /// <summary>
        /// 데이터 수집 중지 센서 제어 부문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_stop_Click(object sender, EventArgs e)
        {

            if (tabControl1.SelectedTab == tabPage1)
            {
                if (appAlreadyRunning)
                {
                    DialogResult dialog = MessageBox.Show("데이터 수집 프로그램을 중지하시겠습니까?", "Application status", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            applicationProcess = GetAppProcess(DataCollectionAppName, appAlreadyRunning);
                            applicationProcess.Kill();
                            b_dataCollection_status.Image = Resources.light_off_26;
                            appAlreadyRunning = false;
                            applicationProcess.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + ex.StackTrace, "Application status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("데이터 수집 프로그램이 이미 중지되어 있습니다", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                if (appAlreadyRunning2)
                {
                    DialogResult dialog = MessageBox.Show("데이터 수집 프로그램을 중지하시겠습니까?", "Application status", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            applicationProcess2 = GetAppProcess(DataCollectionAppName2, appAlreadyRunning2);
                            applicationProcess2.Kill();
                            b_dataCollection_status.Image = Resources.light_off_26;
                            appAlreadyRunning2 = false;
                            applicationProcess2.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + ex.StackTrace, "Application status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("차압 센서 데이터 수집 프로그램은 아직 구현이 안되어 있어요!", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void F_Exit_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.Exit();
            System.Windows.Forms.Application.Exit();
        }




        private DataSet GetRangesWithUsage(int s_Id, string sensorType) // c=checkBox, RangeTb = 범위테이블
        {
            string UsageTable = tabControl1.SelectedTab == tabPage1 ? S_UsageTable : S_UsageTable_p;
            string sensorCategory = tabControl1.SelectedTab == tabPage1 ? "particle" : "pressure";

            DataSet ds = new DataSet();
            bool idExists = GetSensorID(s_Id);
            if (!idExists)
            {
                MessageBox.Show("DB에 존재하지 않는 ID입니다.", "Status info");
            }
            else
            {
                /*bool UsageTableExists = g_DbTableHandler.IfTableExists(UsageTable);
                if (UsageTableExists)
                {*/
                /*bool sUsageInfoExists = g_DbTableHandler.IfTableExists(c_xRangesTb);
                if (sUsageInfoExists)
                {*/
                string sqlGetRanges = $"SELECT {S_SanghanHahanTable.Item2[2]}" +
                                                    $", {S_FourRangeColumns[0]}" +
                                                    $", {S_FourRangeColumns[1]}" +
                                                    $", {S_FourRangeColumns[2]}" +
                                                    $", {S_FourRangeColumns[3]}" +
                                                    $", {S_SanghanHahanTable.Item2[7]}" +
                                                $" FROM {S_SanghanHahanTable.Item1} " +
                                                $" WHERE {S_DeviceInfoColumns[0]} = {s_Id} AND {S_SanghanHahanTable.Item2[0]} = '{sensorCategory}'; ";
                //string sqlStr = $"SELECT * FROM [dbo].[{S_UsageInfo}]; ";
                using (SqlConnection myConn = new SqlConnection(sqlConString))
                {
                    try
                    {
                        if (myConn.State != ConnectionState.Open)
                        {
                            myConn.Open();
                        }
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlGetRanges, myConn))
                        {
                            sqlDataAdapter.Fill(ds);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                //}
                //}
            }
            return ds;
        }


        private void b_save_Click(object sender, EventArgs e)
        {

            Dictionary<CheckBox, List<NumericUpDown>> dataToBeSaved = new Dictionary<CheckBox, List<NumericUpDown>>();
            List<CheckBox> checkedItems;
            string sensor_usage;
            int deviceId;
            //List<string> SanghanHahanTableColumns = g_DbTableHandler.GetTableColumnNames(dbName[0] + "_SanghanHahan");



            if (tabControl1.SelectedTab == tabPage1)
            {
                checkedItems = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => x.Checked).ToList();
                sensor_usage = (checkedItems.Count > 0) ? "YES" : "NO";

                String input = S_DeviceInfo[4].Text; //@"var product_pic_fn=;var firmware_ver='20.02.024';var wan_ip='92.75.120.206';if (parent.location.href != window.location.href)";
                Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                MatchCollection ipIsValid = ip.Matches(input);
                int value;
                bool portIsValid = int.TryParse(S_DeviceInfo[5].Text, out value);
                deviceId = Convert.ToInt32(sID.Text);
                int newOrderNumber;
                if (listView1_thp.Items.Count > 0)
                {
                    newOrderNumber = Convert.ToInt32(listView1_thp.Items[listView1_thp.Items.Count - 1].Text) + 1;
                }
                else
                {
                    newOrderNumber = 1;
                }

                //기존 센서 정보를 update하는 부분
                /*                if (listView1_thp.SelectedItems.Count > 0)
                                {

                                    String input = S_DeviceInfo[4].Text; //@"var product_pic_fn=;var firmware_ver='20.02.024';var wan_ip='92.75.120.206';if (parent.location.href != window.location.href)";
                                    Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                                    MatchCollection ipIsValid = ip.Matches(input);
                                    int value;
                                    bool portIsValid = int.TryParse(S_DeviceInfo[5].Text, out value);


                                    deviceId = Convert.ToInt32(listView1_thp.SelectedItems[0].SubItems[1].Text);*/
                //Console.WriteLine("ID:" + listView1.SelectedItems[0].Text);
                if (S_DeviceInfo[0].Text.Length > 1 && S_DeviceInfo[1].Text.Length > 1 && portIsValid)
                {
                    bool updOrIns = AddToDB(sensor_usage, deviceId);
                    //bool updated = UpdateDB(deviceId);
                    if (updOrIns)
                    {
                        if (listView1_thp.SelectedItems.Count > 0)
                        {
                            foreach (ListViewItem item in listView1_thp.SelectedItems)
                            {
                                item.SubItems[1].Text = sID.Text;
                                for (int i = 0; i < S_DeviceInfo.Count; i++)
                                {
                                    if (i < 4 || i > 5)
                                        item.SubItems[i + 2].Text = S_DeviceInfo[i].Text;
                                }
                                item.SubItems[item.SubItems.Count - 1].Text = sensor_usage;
                            }
                        }
                        else
                        {
                            ListViewItem listViewItem = new ListViewItem(newOrderNumber.ToString());
                            listViewItem.SubItems.Add(sID.Text);
                            for (int i = 0; i < S_DeviceInfo.Count - 2; i++)
                            {
                                if (i < 4 || i > 5)
                                    listViewItem.SubItems.Add(S_DeviceInfo[i].Text);
                            }
                            listViewItem.SubItems.Add(sensor_usage);
                            listView1_thp.Items.Add(listViewItem);
                        }

                        clearFields(S_DeviceInfo);
                        MessageBox.Show("센서 정보 DB 업데이트가 성공적으로 이루어졌습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("센서명 및 Zone 정보를 꼭 입력해 주세요.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                //}

                //새 장비 추가하는 부분
                /*else
                {
                    int newOrderNumber;
                    if (listView1_thp.Items.Count > 0)
                    {
                        newOrderNumber = Convert.ToInt32(listView1_thp.Items[listView1_thp.Items.Count - 1].Text) + 1;
                    }
                    else
                    {
                        newOrderNumber = 1;
                    }
                    if (sID.Text.Length > 0)
                    {
                        deviceId = Convert.ToInt32(sID.Text);
                        if (S_DeviceInfo[0].Text.Length > 1 && S_DeviceInfo[1].Text.Length > 1 && S_DeviceInfo[4].Text.Length > 1 && S_DeviceInfo[5].Text.Length > 1)
                        {
                            //Added should return true if data added to DB.
                            bool added = AddToDB(sUsage, deviceId);
                            if (added)
                            {
                                ListViewItem listViewItem = new ListViewItem(newOrderNumber.ToString());
                                listViewItem.SubItems.Add(sID.Text);
                                for (int i = 0; i < S_DeviceInfo.Count - 2; i++)
                                {
                                    if (i < 4 || i > 5)
                                        listViewItem.SubItems.Add(S_DeviceInfo[i].Text);
                                }
                                listViewItem.SubItems.Add(sUsage);
                                listView1_thp.Items.Add(listViewItem);
                                clearFields(S_DeviceInfo);
                                MessageBox.Show("새 센서 장비 정보가 DB에 성공적으로 추가 되었습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show(" (*)라고 표시된 항목의 정보를 꼭 입력해 주세요.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }
                    }
                    else
                    {
                        MessageBox.Show("새 센서 장비 정보를 등록하시려면 '센서 추가' 버튼을 눌러주세요.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }

                }*/

                S_TimeTable = CheckTimeTable("particle");
                if (!S_TimeTable.Item2)
                {
                    TimeSettings timeSettings1 = new TimeSettings(sqlConString, S_TimeTable.Item1, TimeTableColumns);
                    //timeoutSettings.sqlConString = sqlConString;
                    //timeoutSettings.S_TimeoutTable = S_TimeoutTable.Item1;
                    timeSettings1.Show();


                    //timeoutSettings.UpdateTimeoutTable();
                }


            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                checkedItems = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Where(x => x.Checked).ToList();
                sensor_usage = (checkedItems.Count > 0) ? "YES" : "NO";

                //기존 센서 정보를 update하는 부분
                /*if (listView2_pressure.SelectedItems.Count > 0)
                {*/
                deviceId = Convert.ToInt32(sID_p.Text); // listView2_pressure.SelectedItems[0].SubItems[1].Text);
                                                        //Console.WriteLine("ID:" + listView1.SelectedItems[0].Text);
                if (S_DeviceInfo_p[0].Text.Length > 1 && S_DeviceInfo_p[1].Text.Length > 1)
                {
                    bool updated = AddToDB(sensor_usage, deviceId); //UpdateDB(deviceId);          // FIX UpdateDB 부문 
                    if (updated)
                    {
                        if (listView2_pressure.SelectedItems.Count > 0)
                        {


                            foreach (ListViewItem item in listView2_pressure.SelectedItems)
                            {
                                item.SubItems[1].Text = sID_p.Text;
                                for (int i = 0; i < S_DeviceInfo_p.Count; i++)
                                {
                                    item.SubItems[i + 2].Text = S_DeviceInfo_p[i].Text;
                                }
                                item.SubItems[item.SubItems.Count - 1].Text = sensor_usage;
                            }
                        }
                        else
                        {
                            ListViewItem listViewItem = new ListViewItem(deviceId.ToString());
                            listViewItem.SubItems.Add(sID_p.Text);
                            for (int i = 0; i < S_DeviceInfo_p.Count; i++)
                            {
                                listViewItem.SubItems.Add(S_DeviceInfo_p[i].Text);
                            }
                            listViewItem.SubItems.Add(sensor_usage);
                            listView2_pressure.Items.Add(listViewItem);
                        }
                        clearFields(S_DeviceInfo_p);
                        MessageBox.Show("센서 정보 DB 업데이트가 성공적으로 이루어졌습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("센서명 및 Zone 정보를 꼭 입력해 주세요.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                /* }

                 //새 장비 추가하는 부분
                 else
                 {*/
                /*bool idExists = GetSensorID(Convert.ToInt32(sID_p.Text));      /// FIX GetSensorID 부문
                if (idExists)
                {
                    MessageBox.Show("DB에 이미 존재하는 센서 장비 ID입니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {*/
                /*int newOrderNumber;
                if (listView2_pressure.Items.Count > 0)
                {
                    newOrderNumber = Convert.ToInt32(listView2_pressure.Items[listView2_pressure.Items.Count - 1].Text) + 1;
                }
                else
                {
                    newOrderNumber = 1;
                }

                if (sID_p.Text.Length > 0)
                {
                    deviceId = Convert.ToInt32(sID_p.Text);
                    if (S_DeviceInfo_p[0].Text.Length > 1 && S_DeviceInfo_p[1].Text.Length > 1)
                    {

                        //Added should return true if data added to DB.
                        bool added = AddToDB(sensor_usage, deviceId);               /// FIX AddToDB 부문
                        if (added)
                        {
                            ListViewItem listViewItem = new ListViewItem(newOrderNumber.ToString());
                            listViewItem.SubItems.Add(sID_p.Text);
                            for (int i = 0; i < S_DeviceInfo_p.Count; i++)
                            {
                                listViewItem.SubItems.Add(S_DeviceInfo_p[i].Text);
                            }
                            listViewItem.SubItems.Add(sensor_usage);
                            listView2_pressure.Items.Add(listViewItem);
                            clearFields(S_DeviceInfo_p);
                            MessageBox.Show("새 센서 장비 정보가 DB에 성공적으로 추가 되었습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //}
                    }
                    else
                    {
                        MessageBox.Show("센서명 및 Zone 정보를 꼭 입력해 주세요.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
                else
                {
                    MessageBox.Show("새 센서 장비 정보를 등록하시려면 '센서 추가' 버튼을 눌러주세요.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }*/
                // }

                S_TimeTable_p = CheckTimeTable("pressure");
                if (!S_TimeTable_p.Item2)
                {
                    TimeSettings_p timeSettings = new TimeSettings_p(sqlConString, S_TimeTable_p.Item1, TimeTableColumns);
                    /*timeSettings.sqlConString = sqlConString;
                    timeSettings.S_TimeTable = S_TimeTable_p.Item1;*/
                    timeSettings.Show();



                    //timeoutSettings.UpdateTimeoutTable();
                }



            }
            else
            {
                MessageBox.Show("아직 구현이 안되어 있는 부문입니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void clearFields(List<TextBox> textBoxes)
        {
            string deviceTable = string.Empty;
            int defaultID = 0;
            TextBox sID_ref = new TextBox();
            for (int i = 0; i < textBoxes.Count; i++)
            {
                textBoxes[i].Text = string.Empty;
            }
            List<CheckBox> isCheked;
            List<List<NumericUpDown>> listNumbers;

            if (tabControl1.SelectedTab == tabPage1)
            {

                isCheked = S_UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
                listNumbers = S_UsageCheckerRangePairs.Values.AsEnumerable().ToList();
                deviceTable = S_DeviceTable;
                defaultID = 1;
                sID_ref = sID;

            }
            else if (tabControl1.SelectedTab == tabPage2)
            {

                isCheked = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().ToList();
                listNumbers = S_UsageCheckerRangePairs_p.Values.AsEnumerable().ToList();
                defaultID = 51;
                deviceTable = S_DeviceTable_p;
                sID_ref = sID_p;
            }
            else
            {
                isCheked = new List<CheckBox>();
                listNumbers = new List<List<NumericUpDown>>();
            }
            for (int i = 0; i < isCheked.Count; i++)
            {
                isCheked[i].Checked = false;
                for (int j = 0; j < listNumbers[i].Count; j++)
                {
                    listNumbers[i][j].Value = 0;
                    listNumbers[i][j].Enabled = false;
                }
            }
            string getMaxId = $"SELECT MAX({S_DeviceInfoColumns[0]}) AS {S_DeviceInfoColumns[0]} FROM {deviceTable};";
            DataSet ds = GetDataSet(getMaxId);
            int sensorId = (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != "") ? (Convert.ToInt32(ds.Tables[0].Rows[0][0]) + 1) : defaultID;
            sID_ref.Text = sensorId.ToString();

        }



        /// <summary>
        /// 센서 장비 관련 데이터 테이블에 있는 센서 정보를 업데이트해 주는 함수.
        /// ID (textBoxes[0].Text) 기준으로 센서 정보가 업데이트가 됨.
        /// </summary>
        /// <param name="deviceIdOld">장비 ID </param>
        private bool UpdateDB(int deviceIdOld)
        {
            bool result_UPD = false;
            TextBox sID_txtB;
            List<TextBox> sDeviceInfo_txtB;
            string DeviceTable = string.Empty, UsageTable = string.Empty;
            bool sUsage = false;
            int deviceIdNew = 0;
            Dictionary<CheckBox, List<NumericUpDown>> UsageCheckerRangePairs;  //   One reference for all (S_UsageCheckerRangePairs or S_UsageCheckerRangePairs_p, etc...)

            if (tabControl1.SelectedTab == tabPage1)
            {
                sID_txtB = sID;
                DeviceTable = S_DeviceTable;
                UsageTable = S_UsageTable;
                sDeviceInfo_txtB = S_DeviceInfo;
                deviceIdNew = Convert.ToInt32(this.sID.Text);
                UsageCheckerRangePairs = S_UsageCheckerRangePairs;

            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                sID_txtB = sID_p;
                sDeviceInfo_txtB = S_DeviceInfo_p;
                DeviceTable = S_DeviceTable_p;
                UsageTable = S_UsageTable_p;
                deviceIdNew = Convert.ToInt32(sID_p.Text);
                UsageCheckerRangePairs = S_UsageCheckerRangePairs_p;
            }
            else
            {
                sID_txtB = new TextBox();
                sDeviceInfo_txtB = new List<TextBox>();
                deviceIdNew = 00;
                UsageCheckerRangePairs = new Dictionary<CheckBox, List<NumericUpDown>>();
            }

            bool idExists = GetSensorID(Convert.ToInt32(sID_txtB.Text));


            if (idExists && deviceIdOld != deviceIdNew)
            {
                MessageBox.Show("DB에 이미 존재하는 센서 장비 ID입니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                List<CheckBox> S_UsageCheckersChecked = UsageCheckerRangePairs.Keys.AsEnumerable().Where(r => r.Checked).ToList();

                List<CheckBox> S_UsageCheckers = UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
                List<string> UsageTableColumns = UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Name).ToList();
                List<string> UsageInfo = S_UsageCheckers.Select(x => x.Checked == true ? "YES" : "NO").ToList();


                bool UpdLimitRangeInfoNotUpdated = false;
                if (S_UsageCheckersChecked.Count > 0)
                {
                    sUsage = true;
                    //IfDbExistsChecker ifDbExists = new IfDbExistsChecker();

                    //targetChBoxes = 사용중인 (checkBox에서 체크된 항목들) 하한 및 상한 범위 정보를 저장하는 DB 테이블명들이 들어간 List
                    List<CheckBox> targetChBoxes = g_DbTableHandler.CheckTablesExistHandler(S_UsageCheckersChecked);

                    for (int i = 0; i < targetChBoxes.Count; i++)
                    {
                        //List<CheckBox> target = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(r => r.Name == targetTbNames[i]).ToList();

                        result_UPD = g_DbTableHandler.UpdateLimitRangeInfo(deviceIdOld, deviceIdNew, targetChBoxes[i], UsageCheckerRangePairs[targetChBoxes[i]]);

                        if (!result_UPD)
                        {
                            i = targetChBoxes.Count;
                        }
                    }

                }
                else
                {
                    sUsage = false;
                    UpdLimitRangeInfoNotUpdated = true;
                    //g_dataHandler.UpdateUsageTable(myConn, S_UsageTable, UsageTableColumns, deviceIdOld,)
                }
                result_UPD = (result_UPD || UpdLimitRangeInfoNotUpdated);
                if (result_UPD)
                {
                    bool upd_SensorUsageTable = g_DbTableHandler.UpdateUsageTable(UsageTable, UsageTableColumns, deviceIdOld, deviceIdNew, UsageInfo);
                    if (upd_SensorUsageTable)
                    {
                        bool deviceInfoTbUpd = g_DbTableHandler.UpdateDeviceInfoTable(DeviceTable, deviceIdOld, deviceIdNew, sDeviceInfo_txtB, sUsage);
                    }
                }

                result_UPD = result_UPD ? true : false;
            }
            return result_UPD;
        }



        /// <summary>
        /// 새로운 센서 정보를 DB테이블에 추가해주는 함수.
        /// </summary>
        /// <param name="g_sensorUsage">(g = general) 전체적인 센서 사용여부를 결정하는 파라메터</param>
        private bool AddToDB(string g_sensorUsage, int sensorId)
        {
            bool result = false;
            bool dbExists = g_DbTableHandler.IfDatabaseExists(dbName);
            if (dbExists)
            {
                //int sensorId = 0; // = Convert.ToInt32(sID.Text);
                string DeviceTable = string.Empty;
                string UsageTable = string.Empty;
                string sensorCategory = string.Empty;
                string settingCategory = string.Empty;
                string settingName = string.Empty;
                string settingLastChanged = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                List<string> DeviceInfoColumns = new List<string>();
                Dictionary<CheckBox, List<NumericUpDown>> UsageCheckerRangePairs = new Dictionary<CheckBox, List<NumericUpDown>>();
                // generic reference for more than one type: UsageCheckerRangePairs, UsageCheckerRangePairs_p, etc.
                List<TextBox> DeviceInfo_txt = new List<TextBox>();

                if (tabControl1.SelectedTab == tabPage1)
                {
                    //sensorId = Convert.ToInt32(sID.Text);
                    DeviceTable = S_DeviceTable;
                    UsageTable = S_UsageTable;
                    DeviceInfo_txt = S_DeviceInfo;
                    UsageCheckerRangePairs = S_UsageCheckerRangePairs;
                    DeviceInfoColumns = S_DeviceInfoColumns;
                    sensorCategory = "particle";
                }
                else if (tabControl1.SelectedTab == tabPage2)
                {
                    //sensorId = Convert.ToInt32(sID_p.Text);
                    DeviceTable = S_DeviceTable_p;
                    UsageTable = S_UsageTable_p;
                    DeviceInfo_txt = S_DeviceInfo_p;
                    UsageCheckerRangePairs = S_UsageCheckerRangePairs_p;
                    sensorCategory = "pressure";

                    for (int q = 0; q < S_DeviceInfoColumns.Count; q++)
                    {
                        if (q < 5 || q > 6)
                            DeviceInfoColumns.Add(S_DeviceInfoColumns[q]);
                    }
                }


                List<CheckBox> sRangeTablesAll = UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
                List<string> sUsageResults = UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Checked ? "YES" : "NO").ToList();



                bool[] result_for_checked = new bool[sRangeTablesAll.Count];
                //sanghangtb_columns = new ValueTuple<string, string[]>();

                string[] sanghangtb_columns = S_SanghanHahanTable.Item2;
                string SanghanHahanTable = S_SanghanHahanTable.Item1;
                bool checkRangesTb = g_DbTableHandler.IfTableExists(SanghanHahanTable);

                if (!checkRangesTb)
                {
                    string sqlCreateTb = $"IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                    $"WHERE TABLE_NAME = N'{SanghanHahanTable}') " +
                    $"CREATE TABLE {SanghanHahanTable} ( " +
                            $" {sanghangtb_columns[0]} NVARCHAR(50) NOT NULL, " +
                            $" {sanghangtb_columns[1]} int NOT NULL, " +
                            $" {sanghangtb_columns[2]} NVARCHAR(50) NULL, " +
                            $" {sanghangtb_columns[3]} NVARCHAR(20) NOT NULL, " +
                            $" {sanghangtb_columns[4]} NVARCHAR(20) NOT NULL, " +
                            $" {sanghangtb_columns[5]} NVARCHAR(20) NOT NULL, " +
                            $" {sanghangtb_columns[6]} NVARCHAR(20) NOT NULL, " +
                            $" {sanghangtb_columns[7]} NVARCHAR(10) NOT NULL, " +
                            $" {sanghangtb_columns[8]} NVARCHAR(50) NULL, " +
                            $" {sanghangtb_columns[9]} NVARCHAR(255) NULL " +
                            $")";

                    checkRangesTb = g_DbTableHandler.CreateTable(dbName, SanghanHahanTable, sqlCreateTb);
                }

                string Update_Insert = string.Empty;
                // 적정범위 정보를 DB에 저장하는 부분

                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    try
                    {
                        con.Open();
                        using (SqlTransaction db_transaction = con.BeginTransaction())
                        {
                            using (SqlCommand InsertCmd = new SqlCommand())
                            {
                                InsertCmd.Connection = con;
                                InsertCmd.Transaction = db_transaction;
                                InsertCmd.CommandType = CommandType.Text;
                                for (int i = 0; i < sRangeTablesAll.Count; i++)
                                {
                                    List<Decimal> sFourRangeTbVals = UsageCheckerRangePairs[sRangeTablesAll[i]].Select(x => x.Value).ToList();
                                    string usage = sRangeTablesAll[i].Checked ? "Yes" : "No";

                                    /*for (int k = 0; k < S_FourRangeColumns.Count; k++)
                                    {*/
                                    Update_Insert = $"IF EXISTS ( SELECT * FROM {SanghanHahanTable} " +
                                                                        $"WHERE {sanghangtb_columns[0]} = '{sensorCategory}' " +
                                                                        $"AND {sanghangtb_columns[1]} = {sensorId} " +
                                                                        $"AND {sanghangtb_columns[2]} = '{sRangeTablesAll[i].Name}' " +
                                                                    //$"AND {sanghangtb_columns[3]} = '{S_FourRangeColumns[k]}'" +
                                                                    $") " +
                                                                $" UPDATE {SanghanHahanTable} " +
                                                                $" SET {sanghangtb_columns[3]} = {sFourRangeTbVals[0]}" +
                                                                $", {sanghangtb_columns[4]} = '{sFourRangeTbVals[1]}' " +
                                                                $", {sanghangtb_columns[5]} = '{sFourRangeTbVals[2]}' " +
                                                                $", {sanghangtb_columns[6]} = '{sFourRangeTbVals[3]}' " +
                                                                $", {sanghangtb_columns[7]} = '{usage}' " +
                                                                        $", {sanghangtb_columns[8]} = '{settingLastChanged}' " +
                                                                $" WHERE {sanghangtb_columns[1]} = {sensorId} " +
                                                                        $"AND {sanghangtb_columns[0]} = '{sensorCategory}'  " +
                                                                        $"AND {sanghangtb_columns[2]} = '{sRangeTablesAll[i].Name}'" +
                                                        $" ELSE " +
                                                            $"INSERT INTO [{SanghanHahanTable}] " +
                                                            $"VALUES('{sensorCategory}'" +
                                                                    $", {sensorId}" +
                                                                    $", '{sRangeTablesAll[i].Name}'" +
                                                                    $", '{sFourRangeTbVals[0]}'" +
                                                                    $", '{sFourRangeTbVals[1]}'" +
                                                                    $", '{sFourRangeTbVals[2]}'" +
                                                                    $", '{sFourRangeTbVals[3]}'" +
                                                                    $", '{usage}'" +
                                                                    $", '{settingLastChanged}'" +
                                                                    $", ''" +
                                                                    $");";

                                    InsertCmd.CommandText = Update_Insert;
                                    InsertCmd.ExecuteNonQuery();
                                    result_for_checked[i] = true;
                                    //}

                                }
                            }
                            db_transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


                }


                result = !result_for_checked.Contains(false);


                bool tbExists = g_DbTableHandler.IfTableExists(UsageTable);
                if (!tbExists)
                {
                    /// #bookmark Table Name Creation 

                    List<string> sRangesTbNames = UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Name).ToList();
                    string sqlCreateUsageTb = $"IF NOT EXISTS ( SELECT * FROM sysobjects " +
                                                        $"WHERE name = '{UsageTable}' and xtype = 'U' ) " +
                                                        $" CREATE TABLE {UsageTable}(" +
                        $" {S_DeviceInfoColumns[0]} INT NOT NULL  ";
                    //, CONSTRAINT PK_{UsageTable}_{S_DeviceInfoColumns[0]} PRIMARY KEY ({S_DeviceInfoColumns[0]}
                    foreach (var item in sRangesTbNames)
                    {
                        sqlCreateUsageTb += $", {item} NVARCHAR(20) NOT NULL ";
                    }

                    sqlCreateUsageTb += " );";
                    tbExists = g_DbTableHandler.CreateTable(dbName, UsageTable, sqlCreateUsageTb);
                }
                if (tbExists)
                {

                    string sqlInsertUsage = $"IF NOT EXISTS(SELECT * FROM {UsageTable} WHERE {S_DeviceInfoColumns[0]} = {sensorId}) " +
                        $"INSERT INTO {UsageTable} VALUES({sensorId} ";

                    foreach (var item in sUsageResults)
                    {
                        sqlInsertUsage += $", '{item}' ";
                    }

                    sqlInsertUsage += $" ) ELSE UPDATE {UsageTable} SET {sRangeTablesAll[0].Name} = '{sUsageResults[0]}'";

                    for (int r = 1; r < sRangeTablesAll.Count; r++)
                    {
                        sqlInsertUsage += $", {sRangeTablesAll[r].Name} = '{sUsageResults[r]}'";
                    }

                    sqlInsertUsage += $" WHERE {S_DeviceInfoColumns[0]} = {sensorId};";
                    using (SqlConnection myConn = new SqlConnection(sqlConString))
                    {
                        try
                        {
                            if (myConn.State != ConnectionState.Open)
                            {
                                myConn.Open();
                            }
                            using (SqlCommand sqlInsertUsageCmd = new SqlCommand(sqlInsertUsage, myConn))
                            {
                                sqlInsertUsageCmd.ExecuteNonQuery();
                            }
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.Message + ex.StackTrace, "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("센서 사용여부 정보 테이블이 정상적으로 생성되지 않았어요.", "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    result = false;
                }

                // 관련 테이블도 같이 업데이트함.


                string DevTblINSRTorUPD = $"IF EXISTS ( SELECT * FROM {DeviceTable} WHERE {S_DeviceInfoColumns[0]} = { sensorId}) " +
                        $" UPDATE {DeviceTable} SET ";
                int index2 = 0;
                for (int i = 1; i < DeviceInfoColumns.Count - 1; i++)
                {
                    DevTblINSRTorUPD += $" {DeviceInfoColumns[i]} = '{DeviceInfo_txt[index2].Text}', ";
                    index2 += 1;
                }
                DevTblINSRTorUPD += $"{S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} = '{g_sensorUsage}' WHERE {S_DeviceInfoColumns[0]} = { sensorId} ELSE ";
                /*                DevTblINSRTorUPD += $" SET {S_DeviceInfoColumns[1]} = '{DeviceInfo_txt[0].Text}', {S_DeviceInfoColumns[2]} = '{DeviceInfo_txt[1].Text}', " +
                                        $"{S_DeviceInfoColumns[3]} = '{DeviceInfo_txt[2].Text}', {S_DeviceInfoColumns[4]} = '{DeviceInfo_txt[3].Text}', " +
                                        $"{S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} = '{g_sensorUsage}' ELSE ";*/

                DevTblINSRTorUPD += $"INSERT INTO {DeviceTable} " +
                    $"VALUES ('{sensorId}', ";
                for (int i = 0; i < DeviceInfo_txt.Count; i++)
                {
                    DevTblINSRTorUPD += $"'{DeviceInfo_txt[i].Text}', ";
                }
                DevTblINSRTorUPD += $" '{g_sensorUsage}');";
                //DevTblINSRTorUPD += $"'{DeviceInfo_txt[2].Text}','{DeviceInfo_txt[3].Text}', '{g_sensorUsage}');";


                /////////////////////////////////////////////////////////////////////bookmark

                using (SqlConnection myConn = new SqlConnection(sqlConString))
                {
                    //myConn.ConnectionString = sqlConString;
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    using (SqlCommand sqlCommand = new SqlCommand(DevTblINSRTorUPD, myConn))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                MessageBox.Show("센서 정보 DB를 찾을 수 없거나 연결을 못했습니다. 프르그램을 다시 실행한 후 DB부터 생성해 주세요.", "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return result;
        }





        private void b_add_Click(object sender, EventArgs e)
        {
            string DeviceTable = string.Empty;
            string sensorId = string.Empty;
            if (tabControl1.SelectedTab == tabPage1)
            {
                sensorId = "1";
                DeviceTable = S_DeviceTable;
                if (listView1_thp.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView1_thp.SelectedItems[0];
                    item.Selected = false;

                }
                //S_DeviceInfo_txtB[0].Text = "PSU650";
                S_DeviceInfo[1].Text = "ZONE";

                for (int i = 2; i < S_DeviceInfo.Count; i++)
                {
                    //textBoxes_UpdSensorInfo[i].TextAlign = HorizontalAlignment.Center;
                    S_DeviceInfo[i].Text = string.Empty;
                }


                if (listView1_thp.Items.Count > 0)
                {
                    string getMaxID = $"SELECT MAX(sID) AS {S_DeviceInfoColumns[0]} FROM {DeviceTable};";
                    DataSet ds = GetDataSet(getMaxID);
                    sID.Text = ds.Tables.Count > 0 ? (Convert.ToInt32(ds.Tables[0].Rows[0].Field<int>(S_DeviceInfoColumns[0])) + 1).ToString() : sensorId;
                }
                else
                {
                    sID.Text = "1";
                }

                RangeSetNew();
                S_DeviceInfo[0].Focus();
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                sensorId = "51";
                DeviceTable = S_DeviceTable_p;
                if (listView2_pressure.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView2_pressure.SelectedItems[0];
                    item.Selected = false;

                }
                //S_DeviceInfo_txtB[0].Text = "PSU650";
                S_DeviceInfo_p[1].Text = "ZONE";

                for (int i = 2; i < S_DeviceInfo_p.Count; i++)
                {
                    //textBoxes_UpdSensorInfo[i].TextAlign = HorizontalAlignment.Center;
                    S_DeviceInfo_p[i].Text = string.Empty;
                }


                if (listView2_pressure.Items.Count > 0)
                {
                    string getMaxID = $"SELECT MAX({S_DeviceInfoColumns[0]}) AS {S_DeviceInfoColumns[0]} FROM {DeviceTable};";
                    DataSet ds = GetDataSet(getMaxID);
                    sID_p.Text = ds.Tables.Count > 0 ? (Convert.ToInt32(ds.Tables[0].Rows[0].Field<int>(S_DeviceInfoColumns[0])) + 1).ToString() : sensorId;
                }
                else
                {
                    sID_p.Text = "51";
                }

                RangeSetNew();
                S_DeviceInfo_p[0].Focus();
            }
        }


        /// <summary>
        /// 주어진 쿼리문에 맞는 DataSet을 반환해줌
        /// </summary>
        /// <param name="sqlStr">쿼리문</param>
        /// <returns></returns>
        private DataSet GetDataSet(string sqlStr)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(sqlConString))
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand(sqlStr, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
            }
            return ds;
        }



        /// <summary>
        /// Custom function for Checking/Unchecking the checkbox control
        /// </summary>
        /// <param name="sender">Chechbox object</param>
        /// <param name="e"></param>
        private void xCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chBox = (CheckBox)sender;
            List<NumericUpDown> x_Ranges;

            //온습도 및 파티클 센서
            if (tabControl1.SelectedTab == tabPage1)
            {
                x_Ranges = S_UsageCheckerRangePairs[chBox];
            }
            //차압 센서 
            else if (tabControl1.SelectedTab == tabPage2)
            {
                x_Ranges = S_UsageCheckerRangePairs_p[chBox];
            }
            // empty case;
            else
            {
                x_Ranges = new List<NumericUpDown>();
            }



            if (chBox.Checked)
            {
                foreach (var item in x_Ranges)
                {
                    item.Enabled = true;
                }
            }
            else if (!chBox.Checked)  // || !NullOrNotChecker(x_Ranges)
            {
                foreach (var item in x_Ranges)
                {
                    item.Enabled = false;
                }
            }
        }




        /// <summary>
        /// 센서 추가 시 범위 설정 컨트롤러를 재세팅 해줌
        /// </summary>
        private void RangeSetNew()
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Checked = false);
                for (int j = 0; j < S_UsageCheckerRangePairs.Keys.Count; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        S_UsageCheckerRangePairs.Values.AsEnumerable().ToList()[j][i].Enabled = false;

                    }
                }
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Select(x => x.Checked = false);
                for (int j = 0; j < S_UsageCheckerRangePairs_p.Keys.Count; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        S_UsageCheckerRangePairs_p.Values.AsEnumerable().ToList()[j][i].Enabled = false;

                    }
                }
            }
        }



        /// <summary>
        /// 데이터 수집 프로그램 프로세스를 트레킹하는 함수.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pTrackerTimer_Tick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                appAlreadyRunning = CheckAppRunning(DataCollectionAppName);
                if (!appAlreadyRunning)
                {
                    b_dataCollection_status.Image = Resources.light_off_26;
                }
                else
                {
                    if (applicationProcess == null)
                    {
                        applicationProcess = GetAppProcess(DataCollectionAppName, appAlreadyRunning);
                    }
                    b_dataCollection_status.Image = Resources.light_on_26_color;
                }

            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                appAlreadyRunning2 = CheckAppRunning(DataCollectionAppName2);
                if (!appAlreadyRunning2)
                {
                    b_dataCollection_status.Image = Resources.light_off_26;
                }
                else
                {
                    if (applicationProcess2 == null)
                    {
                        applicationProcess2 = GetAppProcess(DataCollectionAppName2, appAlreadyRunning2);
                    }
                    b_dataCollection_status.Image = Resources.light_on_26_color;
                }

            }




        }



        /// <summary>
        /// 수집 프로그램이 잘 실행되고 있는지 확인함.
        /// </summary>
        /// <param name="dataCollectionAppName"></param>
        /// <returns></returns>
        private bool CheckAppRunning(string dataCollectionAppName)
        {
            bool res = false;
            Process[] processes = Process.GetProcessesByName(dataCollectionAppName);
            if (processes.Length > 0)
            {
                res = true;
            }
            return res;
        }

        private void DownloadToExcel_Click(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker2.Value = dateTimePicker2.Value.AddSeconds(-1).AddDays(1);
            string startTime = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //DownToExcel downToExcel = new DownToExcel(tbName: "d_p03Usage", sqlConStr: sqlConString, (startTime, endTime));

            List<string> tableNames = new List<string>();
            
            if (tabControl1.SelectedTab == tabPage1)
            {
                startTime = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
                endTime = dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");
                tableNames = new List<string>() { S_DataTable };
                //tableNames =  S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Name).Select(x => "d" + x.Substring(1)).ToList();
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                startTime = dateTimePicker1_p.Value.ToString("yyyy-MM-dd HH:mm:ss");
                endTime = dateTimePicker2_p.Value.ToString("yyyy-MM-dd HH:mm:ss");
                tableNames = new List<string>() { S_DataTable_p };
                //tableNames = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Select(x => x.Name).Select(x => "d" + x.Substring(1)).ToList();
            }
            else
            {
                // space for new category of devices
            }
            DownToExcel toExcel = new DownToExcel(tableNames, sqlConString, (startTime, endTime));
            System.Threading.Thread downloaderThread = new System.Threading.Thread(toExcel.StartDownload);
            downloaderThread.IsBackground = true;
            downloaderThread.Start();

        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                clearFields(S_DeviceInfo);
                if (pTrackerTimer.Enabled == false)
                {
                    pTrackerTimer.Enabled = true;
                    pTrackerTimer.Start();
                    CheckAppRunning(DataCollectionAppName);
                }

                if (appAlreadyRunning)
                {
                    b_dataCollection_status.Image = Resources.light_on_26_color;
                }
                else
                {
                    b_dataCollection_status.Image = Resources.light_off_26;
                }
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                clearFields(S_DeviceInfo_p);
                // Further FIX is Needed after Pressure sensor data collection is added

                //pTrackerTimer.Enabled = false;
                CheckAppRunning(DataCollectionAppName2);
                if (appAlreadyRunning2)
                {
                    b_dataCollection_status.Image = Resources.light_on_26_color;

                }
                else
                {
                    b_dataCollection_status.Image = Resources.light_off_26;
                }

            }

        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1_thp.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1_thp.SelectedItems)
                {
                    int sensorId = Convert.ToInt32(item.SubItems[1].Text);
                    DataSet SensorDeviceInfo = GetDeviceInfo(dbName, S_DeviceTable, sensorId);

                    sID.Text = sensorId.ToString();

                    if (SensorDeviceInfo.Tables.Count > 0 && SensorDeviceInfo.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = SensorDeviceInfo.Tables[0].Rows[0];
                        List<string> SensorDevInfo = new List<string>();
                        foreach (var column in row.ItemArray)
                        {
                            SensorDevInfo.Add(column.ToString());
                        }

                        for (int i = 1; i < SensorDevInfo.Count - 1; i++)
                        {
                            S_DeviceInfo[i - 1].Text = SensorDevInfo[i];
                            //S_DeviceInfo_txtB[i].TextAlign = HorizontalAlignment.Center;
                        }

                        List<CheckBox> sUsageRangesCh = S_UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
                        List<string> sUsageRangesTables = S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Name).ToList();

                        for (int i = 0; i < sUsageRangesTables.Count; i++)
                        {

                            DataSet rangesWithUsage = GetRangesWithUsage(sensorId, sUsageRangesTables[i]);

                            // first time use
                            if (rangesWithUsage.Tables.Count == 0 || rangesWithUsage.Tables[0].Rows.Count == 0)
                            {
                                S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Checked = false);
                                S_UsageCheckerRangePairs.Values.AsEnumerable().Select(list => list.Select(x => x.Enabled = false));
                            }
                            else
                            {
                                List<decimal> dataFromDB = new List<decimal>();

                                //List<decimal> dataFromDB = dataFromDB.Select()
                                for (int j = 0; j < S_FourRangeColumns.Count; j++)
                                {
                                    dataFromDB.Add(rangesWithUsage.Tables[0].AsEnumerable().Where(x => x.Field<string>(S_SanghanHahanTable.Item2[2]).Contains(sUsageRangesTables[i])).Select(x => Convert.ToDecimal(x.Field<string>(S_FourRangeColumns[j]))).ToList()[0]);
                                    //dataFromDB.Add(Convert.ToDecimal(rangesWithUsage.Tables[0].Rows[0][S_FourRangeColumns[j]]));
                                }

                                bool sUsage = rangesWithUsage.Tables[0].AsEnumerable().Where(x => x.Field<string>(S_SanghanHahanTable.Item2[2]).Contains(sUsageRangesTables[i])).Select(x => x.Field<string>(S_SanghanHahanTable.Item2[7])).ToList()[0].ToString().Contains("Yes") ? true : false; // rangesWithUsage.Tables[0].Rows[0][sUsageRangesTables[i]].ToString() == "YES";

                                List<NumericUpDown> checkers = S_UsageCheckerRangePairs[sUsageRangesCh[i]];

                                for (int j = 0; j < checkers.Count; j++)
                                {
                                    //checkers[i].Value = dataFromDB[i];
                                    S_UsageCheckerRangePairs[sUsageRangesCh[i]][j].Value = dataFromDB[j];

                                }
                                CheckBox currentCheckBox = S_UsageCheckerRangePairs.Keys.Where(x => x.Name == sUsageRangesTables[i]).FirstOrDefault();
                                currentCheckBox.Checked = sUsage;

                            }
                        }

                    }


                    else
                    {
                        clearFields(S_DeviceInfo);
                    }
                }
            }
        }



        private void listView2_pressure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2_pressure.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView2_pressure.SelectedItems)
                {
                    int sensorId = Convert.ToInt32(item.SubItems[1].Text);
                    sID_p.Text = sensorId.ToString();
                    for (int i = 0; i < S_DeviceInfo_p.Count; i++)
                    {
                        S_DeviceInfo_p[i].Text = item.SubItems[i + 2].Text;
                        //S_DeviceInfo_txtB[i].TextAlign = HorizontalAlignment.Center;
                    }
                    List<CheckBox> sUsageRangesCh_p = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().ToList();
                    List<string> sUsageRangesTables_p = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Select(x => x.Name).ToList();

                    for (int i = 0; i < sUsageRangesTables_p.Count; i++)
                    {

                        DataSet rangesWithUsage_p = GetRangesWithUsage(sensorId, sUsageRangesTables_p[i]);

                        // first time use
                        if (rangesWithUsage_p.Tables.Count == 0 || rangesWithUsage_p.Tables[0].Rows.Count == 0)
                        {
                            S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Select(x => x.Checked = false);
                            S_UsageCheckerRangePairs_p.Values.AsEnumerable().Select(list => list.Select(x => x.Enabled = false));
                        }
                        else
                        {
                            List<decimal> dataFromDB = new List<decimal>();
                            for (int j = 0; j < S_FourRangeColumns.Count; j++)
                            {
                                dataFromDB.Add(rangesWithUsage_p.Tables[0].AsEnumerable().Where(x => x.Field<string>(S_SanghanHahanTable.Item2[2]).Contains(sUsageRangesTables_p[i])).Select(x => Convert.ToDecimal(x.Field<string>(S_FourRangeColumns[j]))).ToList()[0]);
                                //dataFromDB.Add(Convert.ToDecimal(rangesWithUsage_p.Tables[0].Rows[0][S_FourRangeColumns[j]]));
                            }

                            bool sUsage = rangesWithUsage_p.Tables[0].AsEnumerable().Where(x => x.Field<string>(S_SanghanHahanTable.Item2[2]).Contains(sUsageRangesTables_p[i])).Select(x => x.Field<string>(S_SanghanHahanTable.Item2[7])).ToList()[0].ToString().Contains("Yes") ? true : false;
                            //bool sUsage = rangesWithUsage_p.Tables[0].Rows[0][sUsageRangesTables_p[i]].ToString() == "YES";

                            List<NumericUpDown> checkers = S_UsageCheckerRangePairs_p[sUsageRangesCh_p[i]];

                            for (int j = 0; j < checkers.Count; j++)
                            {
                                //checkers[i].Value = dataFromDB[i];
                                S_UsageCheckerRangePairs_p[sUsageRangesCh_p[i]][j].Value = dataFromDB[j];

                            }
                            CheckBox currentCheckBox = S_UsageCheckerRangePairs_p.Keys.Where(x => x.Name == sUsageRangesTables_p[i]).FirstOrDefault();
                            currentCheckBox.Checked = sUsage;

                        }
                    }
                }
            }
            else
            {
                clearFields(S_DeviceInfo_p);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool CheckSensorRunning()
        {
            bool res = false;
            List<string> sensorNames = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Select(x => x.Name).ToList();
            SqlTransaction transaction_p;
            string tbName = string.Empty;
            for (int i = 0; i < sensorNames.Count; i++)
            {
                tbName = "d" + sensorNames[i].Substring(1);
                string pressureSensorSql = $"SELECT COUNT(*) as COUNT FROM {tbName} WHERE DateAndTime > DATEADD(SS, -{S_UsageCheckInterval}, GETDATE())";
                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    con.Open();
                    transaction_p = con.BeginTransaction();
                    using (SqlCommand cmd = new SqlCommand(pressureSensorSql, con))
                    {
                        cmd.Transaction = transaction_p;
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (r.Read())
                                {
                                    res = Convert.ToInt32(r.GetValue(0)) > 0;
                                    if (res)
                                    {
                                        r.Close();
                                        transaction_p.Commit();
                                        return res;
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                Console.WriteLine($"Error Message: {ex.Message}. {ex.StackTrace}");
                                try
                                {
                                    transaction_p.Rollback();
                                }
                                catch (Exception ex2)
                                {
                                    // This catch block will handle any errors that may have occurred
                                    // on the server that would cause the rollback to fail, such as
                                    // a closed connection.
                                    Console.WriteLine($"\nRollback Exception Type: {ex2.GetType()}");
                                    Console.WriteLine($"\nError Message: {ex2.Message}. {ex2.StackTrace}");
                                }
                            }
                        }

                    }
                    transaction_p.Dispose();

                }
            }
            return res;

        }



        public class AppInfo
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            private static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);
            private static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
            public static string StartupPath
            {
                get
                {
                    StringBuilder stringBuilder = new StringBuilder(260);
                    GetModuleFileName(NullHandleRef, stringBuilder, stringBuilder.Capacity);
                    return Path.GetDirectoryName(stringBuilder.ToString());
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                TimeSettings timeoutSettings = new TimeSettings(sqlConString, S_TimeTable.Item1, TimeTableColumns);
                //timeoutSettings.sqlConString = sqlConString;
                //timeoutSettings.S_TimeoutTable = S_TimeoutTable.Item1;
                timeoutSettings.Show();

            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                TimeSettings_p timeoutSettings = new TimeSettings_p(sqlConString, S_TimeTable.Item1, TimeTableColumns);
                /*timeoutSettings.sqlConString = sqlConString;
                timeoutSettings.S_TimeTable = S_TimeTable_p.Item1;*/
                timeoutSettings.Show();
            }

        }
    }
}
