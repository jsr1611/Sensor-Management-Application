using EasyModbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlaUI.UIA3;
using FlaUI.Core;
using DataCollectionApp2.Properties;
using System.Data.Odbc;

namespace DataCollectionApp2
{
    public partial class Form1 : Form
    {

        public string dbServer = "127.0.0.1";
        public string dbName = "SensorDataDB";
        public string dbUID = "dlitdb01";
        public string dbPWD = "dlitdb01";
        public string connectionTimeout = "180";
        public string S_SensorInfo = "SensorInfo";
        public string S_UsageInfo = "UsageInfo";

        private List<string> _sensorInfoColmn;
        public List<string> S_SensorInfoColmn 
        { 
            get { return _sensorInfoColmn; } 
            set { _sensorInfoColmn = value; } 
        }
        
        
        
        private List<string> _fourRangeColmn;
        public List<string> S_FourRangeColmn 
        { get { return _fourRangeColmn; }
          set { _fourRangeColmn = value; } 
        }




        private ModbusClient _modbusClient;
        public ModbusClient modbusClient 
        { get { return _modbusClient; }
          set { _modbusClient = value; } 
        }

        private SqlConnection _myConn;
        public SqlConnection myConn
        {
            get { return _myConn; }
            set { _myConn = value; }
        }
        
        
        private List<int> _IDs;
        public List<int> S_IDs
        {
            get { return _IDs; }
            set { _IDs = value; }
        }

        public Int64 dataCount { get; set; }
        public DateTime startTime { get; set; }



        private List<TextBox> _txtB_SensorInfo;
        /// <summary>
        /// sName, sLocation, sDescription 들이 들어가 있음.
        /// </summary>
        public List<TextBox> txtB_SensorInfo
        {
            get { return _txtB_SensorInfo; }
            set { _txtB_SensorInfo = value; }
        }
        public Dictionary<CheckBox, List<NumericUpDown>> S_UsageCheckerRangePairs { get; set; }

        public DbTableHandler g_DbTableHandler;

        public string appAddress = @"C:\Users\JIMMY\source\repos\0DataCollectionAppNew\DataCollectionApp\bin\Release\Modbus_RTU_SensorData.EXE";
        public FlaUI.Core.Application dataCollectionApp { get; set; }
        
        public Form1()
        {
            InitializeComponent();
            listView1.Scrollable = true;
            MyFunc();


        }





        private void MyFunc()
        {
            dbServer = "127.0.0.1";    //"10.1.55.174";
            dbName = "SensorDataDB2";
            S_SensorInfo = "SENSOR_INFO2";
            
            txtB_SensorInfo = new List<TextBox>() { sName, sLocation, sDescription };
            List<ColumnHeader> lvColHeaders = new List<ColumnHeader>() { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 };

            S_SensorInfoColmn = new List<string>() { sID.Name, txtB_SensorInfo[0].Name, txtB_SensorInfo[1].Name, txtB_SensorInfo[2].Name, "sUsage" };
            S_UsageInfo = "UsageInfo";
            S_FourRangeColmn = new List<string>() { "higherLimit2", "higherLimit1", "lowerLimit1", "lowerLimit2"};


            List<CheckBox> S_UsageCheckers = new List<CheckBox>() { c_tUsage, c_hUsage, c_p03Usage, c_p05Usage, c_p10Usage, c_p25Usage, c_p50Usage, c_p100Usage };
            List<NumericUpDown> t_Ranges = new List<NumericUpDown>() { s_tHigherLimit2, s_tHigherLimit1, s_tLowerLimit1, s_tLowerLimit2 };
            List<NumericUpDown> h_Ranges = new List<NumericUpDown>() { s_hHigherLimit2, s_hHigherLimit1, s_hLowerLimit1, s_hLowerLimit2};
            List<NumericUpDown> p03_Ranges = new List<NumericUpDown>() { s_p03HigherLimit2, s_p03HigherLimit1, s_p03LowerLimit1, s_p03LowerLimit2};
            List<NumericUpDown> p05_Ranges = new List<NumericUpDown>() { s_p05HigherLimit2, s_p05HigherLimit1, s_p05LowerLimit1, s_p05LowerLimit2};
            List<NumericUpDown> p10_Ranges = new List<NumericUpDown>() { s_p10HigherLimit2, s_p10HigherLimit1, s_p10LowerLimit1, s_p10LowerLimit2};
            List<NumericUpDown> p25_Ranges = new List<NumericUpDown>() { s_p25HigherLimit2, s_p25HigherLimit1, s_p25LowerLimit1, s_p25LowerLimit2};
            List<NumericUpDown> p50_Ranges = new List<NumericUpDown>() { s_p50HigherLimit2, s_p50HigherLimit1, s_p50LowerLimit1, s_p50LowerLimit2};
            List<NumericUpDown> p100_Ranges = new List<NumericUpDown>() { s_p100HigherLimit2, s_p100HigherLimit1, s_p100LowerLimit1, s_p100LowerLimit2};

            List<List<NumericUpDown>> S_Ranges = new List<List<NumericUpDown>>() { t_Ranges, h_Ranges, p03_Ranges, p05_Ranges, p10_Ranges, p25_Ranges, p50_Ranges, p100_Ranges };



            /* try
             {*/

            //String[] sensordata = { "ID", "Temp", "Humidity", "Part03", "Part05", "DateTime" };
            //Console.WriteLine("Count\t" + string.Join("\t", sensordata) + "\t\t Run Time");
            
            
            
            myConn = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20"); // ; Integrated Security=True ");
            g_DbTableHandler = new DbTableHandler(new List<string>() { dbServer, dbName, dbUID, dbUID });
            //g_DbTableHandler.connStr = new List<string>() { dbServer, dbName, dbUID, dbUID };

            DataSet sensorInfoTable = GetSensorInfo(dbName, S_SensorInfo);
            if (sensorInfoTable.Tables.Count > 0)
            {
                S_IDs = new List<int>(sensorInfoTable.Tables[0].AsEnumerable().Where(r => r.Field<string>("sUsage") == "YES").Select(r => r.Field<int>("sID")).ToList());

                //ModBus and myConnection initialization
                ConnectionSettings(false);

                dataCount = 0;

                string[] rows = new string[sensorInfoTable.Tables[0].Columns.Count];

                int num = 1;
                foreach (DataRow row in sensorInfoTable.Tables[0].Rows)
                {
                    Console.WriteLine(row["sID"]);

                    ListViewItem listViewItem = new ListViewItem(num.ToString());
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        //if(row.ItemArray[i].ToString())
                        //rows[i] = row.ItemArray[i].ToString();
                        listViewItem.SubItems.Add(row.ItemArray[i].ToString());
                        listView1.Columns[i].TextAlign = HorizontalAlignment.Center;
                    }
                    listView1.Items.Add(listViewItem);
                    num += 1;
                }

            }
            else
            {
                //MessageBox.Show("프로그램 다시 실행해 주세요!", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                // if User selects not to create  new DB
            }

            S_UsageCheckerRangePairs = new Dictionary<CheckBox, List<NumericUpDown>>();
            for (int i = 0; i < S_UsageCheckers.Count; i++)
            {
                S_UsageCheckerRangePairs.Add(S_UsageCheckers[i], S_Ranges[i]);
            }
            startTime = DateTime.Now;



        }





        /// <summary>
        /// ModBus Connection settings and initialization of myConnection SQLConnection object
        /// </summary>
        /// <param name="modbusClient"></param>
        private void ConnectionSettings(bool flag)
        {
            if (flag)
            {
                modbusClient = new ModbusClient("COM3");
                modbusClient.Baudrate = 115200; // Not necessary since default baudrate = 9600
                modbusClient.Parity = System.IO.Ports.Parity.None;
                modbusClient.StopBits = System.IO.Ports.StopBits.Two;
                modbusClient.ConnectionTimeout = 5000;
                //modbusClient.Connect();
                Console.WriteLine("Device Connection Successful");

                //myConnection.Open();
            }                                                                                                                              
        }




        private void MinimizeToTray()
        {
            try
            {
                notifyIcon1.BalloonTipTitle = "Sample text";
                notifyIcon1.BalloonTipText = "Form is minimized";

                if (FormWindowState.Minimized == this.WindowState)
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(500);
                    this.Hide();
                }
                else if (FormWindowState.Normal == this.WindowState)
                {
                    notifyIcon1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                notifyIcon1.Visible = true;
                this.Hide();
                e.Cancel = true;
            }
        }



        /// <summary>
        /// SENSOR_INFO테이블에 있는 모든 센서에 대한 ID를 List<int> 형태로 불러오는 함수
        /// </summary>
        /// <param name="S_IDs"></param>
        /// <returns></returns>
        private List<int> GetSensorIDs(List<int> S_IDs)
        {
            string IdCheckCmd = $"SELECT {S_SensorInfoColmn[0]} FROM {dbName}.dbo.{S_SensorInfo} WHERE {S_SensorInfoColmn[4]}='YES'";
            //Console.WriteLine("Usable sensor IDs:");
            SqlCommand sqlCommand = new SqlCommand(IdCheckCmd, myConn);
            try
            {
                if(myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        S_IDs.Add(Convert.ToInt32(sqlDataReader[$"{S_SensorInfoColmn[0]}"]));
                        //Console.WriteLine(Convert.ToInt32(sqlDataReader["sID"]));
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
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
            
                bool idExists = false;
                string sqlStrChecker = $"SELECT {S_SensorInfoColmn[0]} FROM [{dbName}].[dbo].[{S_SensorInfo}] WHERE {S_SensorInfoColmn[0]} = {id};";
                using (SqlCommand sqlIdCheckerCmd = new SqlCommand(sqlStrChecker, myConn))
                {
                    try
                    {
                        if (myConn.State != ConnectionState.Open)
                        {
                            myConn.Open();
                        }
                        using (SqlDataReader reader = sqlIdCheckerCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idExists = Convert.ToInt32(reader[$"{S_SensorInfoColmn[0]}"].ToString()) == id;
                                break;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (myConn.State == ConnectionState.Open)
                        {
                            myConn.Close();
                        }
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
            string sqlStrChecker = $"SELECT {S_SensorInfoColmn[0]} FROM [{dbName}].[dbo].[{tableName}] WHERE {S_SensorInfoColmn[0]} = {id};";
            using (SqlCommand sqlIdCheckerCmd = new SqlCommand(sqlStrChecker, myConn))
            {
                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    using (SqlDataReader reader = sqlIdCheckerCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idExists = Convert.ToInt32(reader[$"{S_SensorInfoColmn[0]}"].ToString()) == id;
                            break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
            return idExists;
        }








        /// <summary>
        /// SENSOR_INFO테이블에 있는 모든 정보를 DataSet형태로 불러오는 함수
        /// </summary>
        /// <returns></returns>
        private DataSet GetSensorInfo(string sensorData_dbName, string sensorInfo_tbName)
        {
            SqlConnection myConn_master = new SqlConnection($@"Data Source = {dbServer};Initial Catalog=master;User id={dbUID};Password={dbPWD};Min Pool Size=20");
            DataSet ds = new DataSet();
            bool checkDbExists = g_DbTableHandler.IfDatabaseExists(sensorData_dbName, myConn_master);
            if (checkDbExists)
            {
                bool Check_SENSOR_INFO_tableExists = g_DbTableHandler.IfTableExists(sensorInfo_tbName);
                if (Check_SENSOR_INFO_tableExists)
                {
                    string sqlStr = $"SELECT * FROM {sensorData_dbName}.dbo.{sensorInfo_tbName}";
                    using (SqlConnection con = new SqlConnection($@"Data Source = {dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
                    { //Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20")) // ; Integrated Security=True
                      //con.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlStr, con);
                        sqlDataAdapter.Fill(ds);
                    }
                }
                else
                {
                    DialogResult createTbOrNot = MessageBox.Show($"센서 정보 테이블을 생성합니다. \nDB명은 {dbName}, \n센서정보 테이블명 = {sensorInfo_tbName}. \n진행하시겠습니까?", "Status Info", MessageBoxButtons.YesNo);
                    if (createTbOrNot == DialogResult.Yes)
                    {
                        string sqlCreateTb = $"CREATE TABLE {sensorInfo_tbName} ({S_SensorInfoColmn[0]} INT NOT NULL, {S_SensorInfoColmn[1]} NVARCHAR(20) NOT NULL, {S_SensorInfoColmn[2]} NVARCHAR(150) NULL, {S_SensorInfoColmn[3]} NVARCHAR(255) NULL, {S_SensorInfoColmn[4]} NVARCHAR(10) NOT NULL);";
                        bool SENSOR_INFO_tableCreated = g_DbTableHandler.CreateTable(dbName, sensorInfo_tbName, sqlCreateTb, myConn);
                        if (SENSOR_INFO_tableCreated)
                        {
                            MessageBox.Show($"센서 정보 DB와 테이블이 성공적으로 생성되었습니다!\nDB명 = {dbName}\n센서 정보 테이블명 = {sensorInfo_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"DB가 생성되었지만, 센서 정보 테이블이 성공적으로 생성되지 않았습니다!\nDB명 = {dbName}\n센서 정보 테이블명 = {sensorInfo_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("센서 정보 테이블이 생성되어 있지 않습니다.", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                DialogResult createTbOrNot = MessageBox.Show($"관리페이지에 오신 것을 환영합니다. \n센서 정보 DB와 테이블을 생성합니다. \nDB명은 {dbName}, \n센서정보 테이블명 = {sensorInfo_tbName}. \n진행하시겠습니까?", "Status Info", MessageBoxButtons.YesNo);
                if (createTbOrNot == DialogResult.Yes)
                {
                    string sqlCreateDb = $"CREATE DATABASE {dbName};";

                    bool dataBase_Created = g_DbTableHandler.CreateDatabase(myConn_master, dbName, sqlCreateDb);
                    if (dataBase_Created)
                    {
                        string sqlCreateTb = $"CREATE TABLE {sensorInfo_tbName} ({S_SensorInfoColmn[0]} INT NOT NULL, {S_SensorInfoColmn[1]} NVARCHAR(20) NOT NULL, {S_SensorInfoColmn[2]} NVARCHAR(150) NULL, {S_SensorInfoColmn[3]} NVARCHAR(255) NULL, {S_SensorInfoColmn[4]} NVARCHAR(10) NOT NULL);";
                        bool SENSOR_INFO_tableCreated = g_DbTableHandler.CreateTable(dbName, sensorInfo_tbName, sqlCreateTb, myConn);
                        if (SENSOR_INFO_tableCreated)
                        {
                            MessageBox.Show($"센서 정보 DB와 테이블이 성공적으로 생성되었습니다!\nDB명 = {dbName}\n센서 정보 테이블명 = {sensorInfo_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"DB가 생성되었지만, 센서 정보 테이블이 성공적으로 생성되지 않았습니다!\nDB명 = {dbName}\n센서 정보 테이블명 = {sensorInfo_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"센서 정보 DB가 성공적으로 생성되지 않았습니다!\nDB명 = {dbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("센서 정보 DB가 생성되어 있지 않습니다.", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            return ds;
        }



        private void b_start_Click(object sender, EventArgs e)
        {

            dataCollectionApp = FlaUI.Core.Application.Launch(appAddress);
            using (var automation = new UIA3Automation())
            {
                var window = dataCollectionApp.GetMainWindow(automation);
                //MessageBox.Show("Hello, " + window.Title, window.Title);

            }
            b_dataCollection_status.Image = Resources.light_on_26_color;

            /* 
            if (!modbusClient.Connected && myConnection.State == ConnectionState.Closed)
            {
                modbusClient.Connect();
                myConnection.Open();
            }
            timer1.Enabled = true;
            timer1.Start();
*/
        }

/*

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime timestamp = DateTime.Now;
            foreach (int s_id in S_IDs)
            {
                if (dataCount >= 2147483646)
                {
                    dataCount = 1;
                }
                dataCount += 1;
                modbusClient.UnitIdentifier = (byte)s_id;
                //string timestamp0 = now.ToString("yyyy-MM-dd HH:mm:ss.fff"); // 

                int[] test1 = modbusClient.ReadInputRegisters(10, 6);

                var timeCount = DateTime.Now - startTime;
                string timestamp0 = timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                decimal temp = test1[0];
                decimal humid = test1[1];
                int[] part03_arr = { 0, 0 };
                int[] part05_arr = { 0, 0 };
                part03_arr[0] = test1[3];
                part03_arr[1] = test1[2];
                part05_arr[0] = test1[5];
                part05_arr[1] = test1[4];
                Int64 part03 = ModbusClient.ConvertRegistersToInt(part03_arr);
                Int64 part05 = ModbusClient.ConvertRegistersToInt(part05_arr);
                Console.WriteLine(dataCount + "\t" + s_id + "\t" + (temp / 100m).ToString("F", CultureInfo.InvariantCulture) + "\t" + (humid / 100m).ToString("F", CultureInfo.InvariantCulture) + "\t\t" + String.Format("{0:n0}", part03) + "\t" + String.Format("{0:n0}", part05) + "\t" + timestamp0 + "\t  {0}일 {1}시간 {2}분 {3}초", timeCount.Days, timeCount.Hours, timeCount.Minutes, timeCount.Seconds);
                string[] allData = { s_id.ToString(), (temp / 100m).ToString("F", CultureInfo.InvariantCulture), (humid / 100m).ToString("F", CultureInfo.InvariantCulture), String.Format("{0:n0}", part03), String.Format("{0:n0}", part05), timestamp0 };
                string sql_str_temp = "INSERT INTO DEV_TEMP_" + s_id.ToString() + " (Temperature, DateAndTime) Values (@Temperature, @DateAndTime)";
                string sql_str_humid = "INSERT INTO DEV_HUMID_" + s_id.ToString() + " (Humidity, DateAndTime) Values (@Humidity, @DateAndTime)";
                string sql_str_part03 = "INSERT INTO DEV_PART03_" + s_id.ToString() + " (Particle03, DateAndTime) Values (@Particle03, @DateAndTime)";
                string sql_str_part05 = "INSERT INTO DEV_PART05_" + s_id.ToString() + " (Particle05, DateAndTime) Values (@Particle05, @DateAndTime)";

                SqlCommand myCommand_temp = new SqlCommand(sql_str_temp, myConn);
                SqlCommand myCommand_humid = new SqlCommand(sql_str_humid, myConn);
                SqlCommand myCommand_part03 = new SqlCommand(sql_str_part03, myConn);
                SqlCommand myCommand_part05 = new SqlCommand(sql_str_part05, myConn);

                myCommand_temp.Parameters.AddWithValue("@Temperature ", (temp / 100m).ToString("F", CultureInfo.InvariantCulture));
                myCommand_temp.Parameters.AddWithValue("@DateAndTime", timestamp0);
                myCommand_temp.ExecuteNonQuery();

                myCommand_humid.Parameters.AddWithValue("@Humidity", (humid / 100m).ToString("F", CultureInfo.InvariantCulture));
                myCommand_humid.Parameters.AddWithValue("@DateAndTime", timestamp0);
                myCommand_humid.ExecuteNonQuery();

                myCommand_part03.Parameters.AddWithValue("@Particle03", part03);
                myCommand_part03.Parameters.AddWithValue("@DateAndTime", timestamp0);
                myCommand_part03.ExecuteNonQuery();

                myCommand_part05.Parameters.AddWithValue("@Particle05", part05);
                myCommand_part05.Parameters.AddWithValue("@DateAndTime", timestamp0);
                myCommand_part05.ExecuteNonQuery();
            }

        }
*/


        private void b_stop_Click(object sender, EventArgs e)
        {
            //FlaUI.Core.Application application = FlaUI.Core.Application.Launch(appAddress);

            // code to interact with the UI

            //
            dataCollectionApp.Close();
            dataCollectionApp.Dispose();
            MessageBox.Show("Anything happened", "Application status");

            /*
            timer1.Stop();
            myConnection.Close();
            modbusClient.Disconnect();
            */
        }



        private void F_Exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }



        /// <summary>
        /// Display listView1 Column headers and sensor info in the below listView2
        /// </summary>
        private void Display_listView2()
        {
            int itemHight = 20;
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, itemHight);

            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                ListViewItem listViewItem = new ListViewItem(listView1.Columns[i].Text);
                listViewItem.SubItems.Add("");
                //listView2.Items.Add(listViewItem);
            }
            //listView2.SmallImageList = imgList;
        }








        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    int sensorId = Convert.ToInt32(item.SubItems[1].Text);
                    sID.Value = sensorId;
                    sID.Enabled = false;
                    for (int i = 0; i < txtB_SensorInfo.Count; i++)
                    {
                        txtB_SensorInfo[i].Text = item.SubItems[i + 2].Text;
                        txtB_SensorInfo[i].TextAlign = HorizontalAlignment.Center;
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
                            for (int j = 0; j < S_FourRangeColmn.Count; j++)
                            {
                                dataFromDB.Add(Convert.ToDecimal(rangesWithUsage.Tables[0].Rows[0][S_FourRangeColmn[j]]));
                            }

                            bool sUsage = rangesWithUsage.Tables[0].Rows[0][S_SensorInfoColmn[4]].ToString() == "YES";

                            List<NumericUpDown> checkers = S_UsageCheckerRangePairs[sUsageRangesCh[i]];

                            for (int j = 0; j < checkers.Count; j++)
                            {
                                //checkers[i].Value = dataFromDB[i];
                                S_UsageCheckerRangePairs[sUsageRangesCh[i]][j].Value = dataFromDB[j];

                            }
                            S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => x.Name == sUsageRangesTables[i]).Select(x => x.Checked = sUsage);

                        }
                    }
                }
            }
            else
            {
                clearFields(txtB_SensorInfo);
            }
        }

        private DataSet GetRangesWithUsage(int s_Id, string c_xRangesTb)
        {
            DataSet ds = new DataSet();
            bool idExists = GetSensorID(s_Id);

            if (!idExists)
            {
                MessageBox.Show("DB에 존재하지 않는 ID입니다.", "Status info");
            }
            else
            {
               
                
                bool sUsageInfoExists = g_DbTableHandler.IfTableExists(c_xRangesTb);       
                if (sUsageInfoExists)
                {
                    string sqlGetRanges = $"SELECT DISTINCT ch.{S_FourRangeColmn[0]}, ch.{S_FourRangeColmn[1]}, ch.{S_FourRangeColmn[2]}, ch.{S_FourRangeColmn[3]}, si.{S_SensorInfoColmn[4]} FROM {S_SensorInfo} si JOIN {c_xRangesTb} ch ON ch.{S_SensorInfoColmn[0]} = si.{S_SensorInfoColmn[0]} WHERE si.{S_SensorInfoColmn[0]} = {s_Id}; ";
                    //string sqlStr = $"SELECT * FROM [dbo].[{S_UsageInfo}]; ";
                    try
                    {
                        if (myConn.State != ConnectionState.Open)
                        {
                            myConn.Open();
                        }
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlGetRanges, myConn);
                        sqlDataAdapter.Fill(ds);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (myConn.State == ConnectionState.Open)
                        {
                            myConn.Close();
                        }
                    }
                }
                /*else
                {
                    string createTb_S_SensorInfo = $"CREATE TABLE {S_UsageInfo}"; 
                    bool tbCreated = g_DbTableHandler.CreateTable(dbName, S_UsageInfo, createTb_S_SensorInfo, myConn);
                    
                }    */
            }
            return ds;
        }



        private void b_save_Click(object sender, EventArgs e)
        {
            bool emptyColumn = false;

            Dictionary<CheckBox, List<NumericUpDown>> dataToBeSaved = new Dictionary<CheckBox, List<NumericUpDown>>();
            for (int i = 0; i < txtB_SensorInfo.Count; i++)
            {
                if (txtB_SensorInfo[i].Text.Length < 1)
                {
                    emptyColumn = true;
                }
            }

            //기존 센서 정보를 update하는 부분
            if (listView1.SelectedItems.Count > 0)
            {
                if (emptyColumn)
                {
                    MessageBox.Show($"빈칸이 있어요.", "Status Info", MessageBoxButtons.OK);
                }
                else
                {
                    //Console.WriteLine("ID:" + listView1.SelectedItems[0].Text);
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        item.SubItems[1].Text = sID.Value.ToString();
                        for (int i = 0; i < txtB_SensorInfo.Count; i++)
                        {
                            item.SubItems[i + 2].Text = txtB_SensorInfo[i].Text;
                        }
                    }

                    UpdateDB();
                    clearFields(txtB_SensorInfo);
                }

            }
            
            //새 장비 추가하는 부분
            else
            {
                if (emptyColumn)
                {
                    MessageBox.Show($"빈칸이 있어요.", "Status Info", MessageBoxButtons.OK);
                }
                else
                {
                    int newOrderNumber;
                    if (listView1.Items.Count > 0)
                    {
                        newOrderNumber = Convert.ToInt32(listView1.Items[listView1.Items.Count - 1].Text) + 1;
                    }
                    else
                    {
                        newOrderNumber = 1;
                    }

                    List<CheckBox> checkedItems = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => x.Checked).ToList();
                    List<CheckBox> unCheckedItems = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => !x.Checked).ToList();
                    string sUsage = (checkedItems.Count > 0) ? "YES" : "NO";

                    /*for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        Console.WriteLine("listView IDs:" + listView1.Items[i].Text);
                    }*/
                    bool added = AddToDB(sUsage);
                    if (added)
                    {
                        ListViewItem listViewItem = new ListViewItem(newOrderNumber.ToString());
                        listViewItem.SubItems.Add(sID.Value.ToString());
                        listViewItem.SubItems.Add(txtB_SensorInfo[0].Text);
                        listViewItem.SubItems.Add(txtB_SensorInfo[1].Text);
                        listViewItem.SubItems.Add(txtB_SensorInfo[2].Text);
                        listViewItem.SubItems.Add(sUsage);
                        listView1.Items.Add(listViewItem);
                        clearFields(txtB_SensorInfo);
                    }
                    
                }
            }

            
        }



        private void clearFields(List<TextBox> textBoxes)
        {
            for (int i = 0; i < textBoxes.Count; i++)
            {
                textBoxes[i].Text = "";
            }
            List<CheckBox> checkers = S_UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
            
            for (int i=0; i < checkers.Count; i++)
            {
                checkers[i].Checked = false;
                S_UsageCheckerRangePairs.Values.AsEnumerable().Select(list => list.Select(item => item.Value = 0));
            }
        }


        /// <summary>
        /// SENSOR_INFO테이블에 있는 센서 정보를 업데이트해 주는 함수.
        /// ID (textBoxes[0].Text) 기준으로 센서 정보가 업데이트가 됨.
        /// </summary>
        /// <param name="textBoxes_SensorInfo">업데이트되는 정보를 가지고 있음.  </param>
        private void UpdateDB()
        {
            bool idExists = GetSensorID(Convert.ToInt32(sID.Value));
            bool sUsage;
            if (!idExists)
            {
                MessageBox.Show("DB에 존재하지 않는 센서 ID입니다.", "Status info");
            }
            else
            {
                DataHandler g_dataHandler = new DataHandler();
                g_dataHandler.S_FourRangeColmn = S_FourRangeColmn;
                g_dataHandler.S_SensorInfoColmn = S_SensorInfoColmn;
                g_dataHandler.S_SensorInfo = S_SensorInfo;
                List<CheckBox> S_UsageCheckersChecked = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(r => r.Checked).ToList();
                int sensorId = Convert.ToInt32(sID.Value);
                if (S_UsageCheckersChecked.Count > 0)
                {
                    sUsage = true;
                    //IfDbExistsChecker ifDbExists = new IfDbExistsChecker();

                    //targetChBoxes = 사용중인 (checkBox에서 체크된 항목들) 하한 및 상한 범위 정보를 저장하는 DB 테이블명들이 들어간 List
                    List<CheckBox> targetChBoxes = g_DbTableHandler.CheckTablesExistHandler(S_UsageCheckersChecked);

                    for (int i = 0; i < targetChBoxes.Count; i++)
                    {
                        //List<CheckBox> target = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(r => r.Name == targetTbNames[i]).ToList();
                        g_dataHandler.UpdateLimitRangeInfo(sID.Value, targetChBoxes[i], S_UsageCheckerRangePairs[targetChBoxes[i]], myConn);
                    }

                }
                else
                {
                    sUsage = false;
                }

                g_dataHandler.UpdateSensorInfo(myConn, S_SensorInfo, sensorId, txtB_SensorInfo, sUsage);
            }
        }


        /// <summary>
        /// 새로운 센서 정보를 DB테이블에 추가해주는 함수.
        /// </summary>
        /// <param name="g_sensorUsage">(g = general) 전체적인 센서 사용여부를 결정하는 파라메터</param>
        private bool AddToDB(string g_sensorUsage)
        {
            //SqlConnection myConn = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20");
            bool result = false;

            bool dbExists = g_DbTableHandler.IfDatabaseExists(dbName);
            if (dbExists)
            {
                int sensorId = Convert.ToInt32(sID.Value);

                //List<string> sRangeTablesChecked = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x=>x.Checked).Select(x => x.Name).ToList();
                List<CheckBox> sRangeTablesChecked = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => x.Checked).ToList();
                List<CheckBox> sRangeTablesUnchecked = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => !x.Checked).ToList();
                 
                for (int i = 0; i < sRangeTablesChecked.Count; i++)
                {
                    List<Decimal> sFourRangeTbVals = S_UsageCheckerRangePairs[sRangeTablesChecked[i]].Select(x=>x.Value).ToList();
                    string sRangeTable = sRangeTablesChecked[i].Name;

                    
                    bool checkRangesTb = g_DbTableHandler.IfTableExists(sRangeTable);
                    string sqlStrInsert = $"INSERT INTO {dbName}.dbo.{sRangeTable}" +
                                            $"({S_SensorInfoColmn[0]}, {S_FourRangeColmn[0]}, {S_FourRangeColmn[1]}, {S_FourRangeColmn[2]}, {S_FourRangeColmn[3]}) " +
                                            $"VALUES({sensorId}, {sFourRangeTbVals[0]},{sFourRangeTbVals[1]},{sFourRangeTbVals[2]},{sFourRangeTbVals[3]});";

                    SqlCommand InsertCmd = new SqlCommand(sqlStrInsert, myConn);
                    string sqlCreateTb = $"Create TABLE [{dbName}].[dbo].[{sRangeTable}] ( " +
                                $" {S_SensorInfoColmn[0]} INT NOT NULL, " +
                                $" {S_FourRangeColmn[0]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColmn[1]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColmn[2]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColmn[3]} decimal(7,2) NULL);";

                    string sqlCheckUpd = $"SELECT 1 " +
                        $" FROM {dbName}.dbo.{sRangeTable} " +
                        $" WHERE {S_SensorInfoColmn[0]} = { sensorId} and " +
                        $" {S_FourRangeColmn[0]} = { sFourRangeTbVals[0]} and " +
                        $" {S_FourRangeColmn[1]} = { sFourRangeTbVals[1]} and " +
                        $" {S_FourRangeColmn[2]} = { sFourRangeTbVals[2]} and " +
                        $" {S_FourRangeColmn[3]} = { sFourRangeTbVals[3]}";
                    SqlCommand sqlUpdateCheck = new SqlCommand(sqlCheckUpd, myConn);

                    try
                    {
                        if (checkRangesTb)      // table이 존재한다면 Insert함
                        {
                            int errorLimit = 2;
                            int counter = 0;
                            while (true)
                            {
                                if (errorLimit <= counter)
                                {
                                    break;
                                }
                                counter += 1;
                                bool check_sIDduplicate = GetSensorID(sensorId, sRangeTable);
                                if (!check_sIDduplicate)
                                {
                                    if (myConn.State != ConnectionState.Open)
                                    {
                                        myConn.Open();
                                    }
                                    InsertCmd.ExecuteNonQuery();

                                    using (SqlDataReader reader = sqlUpdateCheck.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            result = Convert.ToInt32(reader.GetValue(0)) == 1;
                                        }
                                    }

                                        break;
                                }
                                else
                                {
                                    while (true)
                                    {
                                        int newID = GetUserInput();
                                        if (sensorId != newID)
                                        {
                                            sensorId = newID;
                                            sID.Value = newID;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {                   // table이 존재하지 않는다면 CreateTable를 통해 테이블 생성한 후 Insert함
                            bool tbCreated = g_DbTableHandler.CreateTable(dbName, sRangeTable, sqlCreateTb, myConn);

                            if (tbCreated)
                            {
                                if (myConn.State != ConnectionState.Open)
                                {
                                    myConn.Open();
                                }
                                InsertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (myConn.State == ConnectionState.Open)
                        {
                            myConn.Close();
                        }
                    }

                }

                for(int i = 0; i<sRangeTablesUnchecked.Count; i++)
                {

                }
                // 관련 테이블도 같이 업데이트함.
                string sqlInsert_SensorInfo = $"INSERT INTO {dbName}.dbo.{S_SensorInfo} ({S_SensorInfoColmn[0]}, {S_SensorInfoColmn[1]}, {S_SensorInfoColmn[2]}, {S_SensorInfoColmn[3]}, {S_SensorInfoColmn[4]}) " +
                    $"VALUES ('{sensorId}', '{txtB_SensorInfo[0].Text}', '{txtB_SensorInfo[1].Text}', '{txtB_SensorInfo[2].Text}', '{g_sensorUsage}');";

                //string sqlInsert_c_xUsage = $"INSERT INTO {dbName}.dbo.{S_SensorInfo} ({S_SensorInfoColmn[0]}, {S_FourRangeColmn[0]}, {S_FourRangeColmn[1]}, {S_FourRangeColmn[2]}, {S_FourRangeColmn[3]}) VALUES({sensorId}, {g_sensorUsage});";

                //string sqlCheck_sUsageInfo = $"SELECT COUNT(*) FROM {dbName}.dbo.{S_UsageInfo};";
                //string sqlInsert_sUsageInfo = $"INSERT INTO {dbName}.dbo.{S_UsageInfo}(sID, tUsage, hUsage, p03Usage, p05Usage, p10Usage, p25Usage, p50Usage, p100Usage) VALUES({sensorId}, 'NO', 'NO', 'NO', 'NO', 'NO', 'NO', 'NO', 'NO');";



                SqlCommand sqlCommand = new SqlCommand(sqlInsert_SensorInfo, myConn);
                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("New sensor info has been successfully saved", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("센서 정보 DB를 찾을 수 없었습니다. 프르그램을 다시 실행한 후 DB부터 생성해 주세여.", "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return result;
        }

        private int GetUserInput()
        {
            int sensorId;
            //MessageBox.Show("추가하시려는 센서는 이미 DB에서 존재합니다. 기존에 있는 센서 정보를 수정하시고나 센서 ID를 바꿔 보세요. \n센서ID를 수정하시려면 'Y' 버튼을 누르세요.", "Status Info", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            //react to dialogResults, yes, no, cancel
            // title, message, textBox enter info, etc. #Fix needed
            string newSensorId = Microsoft.VisualBasic.Interaction.InputBox("새 ID를 입력", "원하시는 센서 ID를 기존에 있는것보다 다르게 입력해 주세요", "Desired Default", -1, -1);
            try
            {
                sensorId = Convert.ToInt32(newSensorId);
            }
            catch
            {
                MessageBox.Show("숫자만 입력 가능합니다. 다시 입력해 주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                sensorId = 0;
            }

            return sensorId;
        }


        private void b_add_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                item.Selected = false;
            }
            for (int i = 0; i < txtB_SensorInfo.Count; i++)
            {
                //textBoxes_UpdSensorInfo[i].TextAlign = HorizontalAlignment.Center;
                txtB_SensorInfo[i].Text = "";
            }
            RangeSetNew();
            txtB_SensorInfo[0].Focus();
        }



        private void textBox5_Click(object sender, EventArgs e)
        {
            /*if(textBox5.Text == "NO")
            {
                textBox5.Text = "YES";
            }
            else
            {
                textBox5.Text = "NO";
            }*/
        }




        private void c_tUsage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_tUsage, S_UsageCheckerRangePairs[c_tUsage]);
        }


        private void c_hUsage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_hUsage, S_UsageCheckerRangePairs[c_hUsage]);
        }


        private void c_p03Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p03Usage, S_UsageCheckerRangePairs[c_p03Usage]);
        }


        private void c_p05Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p05Usage, S_UsageCheckerRangePairs[c_p05Usage]);

        }

        private void c_p10Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p10Usage, S_UsageCheckerRangePairs[c_p10Usage]);

        }

        private void c_p25Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p25Usage, S_UsageCheckerRangePairs[c_p25Usage]);

        }

        private void c_p50Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p50Usage, S_UsageCheckerRangePairs[c_p50Usage]);

        }

        private void c_p100Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p100Usage, S_UsageCheckerRangePairs[c_p100Usage]);

        }



        /// <summary>
        /// "사용안함"이 선텍되거나 어느 하나 센서의 모든 범위 설정이 0으로 되어있을 때 사용됨.
        /// </summary>
        /// <param name="x_Ranges"></param>
        /// <returns></returns>
        private bool NullOrNotChecker(List<NumericUpDown> x_Ranges)
        {
            bool res = false;
            foreach (var item in x_Ranges)
            {
                if (item.Value != 0)
                {
                    res = true;
                }
            }
            return res;
        }



        /// <summary>
        /// 체크 버튼 눌을 때 실행되는 코드
        /// </summary>
        /// <param name="c_xUsage"></param>
        /// <param name="x_Ranges"></param>
        private void c_xUsage_Checker(CheckBox c_xUsage, List<NumericUpDown> x_Ranges)
        {
            if (c_xUsage.Checked)
            {
                foreach (var item in x_Ranges)
                {
                    item.Enabled = true;
                }
            }
            else if (c_xUsage.Checked == false || NullOrNotChecker(x_Ranges) == false)
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
            S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(x=>x.Checked = false);
            for (int j = 0; j < S_UsageCheckerRangePairs.Keys.Count; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    S_UsageCheckerRangePairs.Values.AsEnumerable().ToList()[j][i].Enabled = false;

                }
            }
        }

    }
}
