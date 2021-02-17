using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using FlaUI.UIA3;
using AdminPage.Properties;
using System.Diagnostics;

namespace AdminPage
{
    public partial class Form1 : Form
    {
        public string DbServer = "";// 127.0.0.1
        public string DbName = ""; // SensorDataDB
        public string DbUID = ""; //dlitdb01
        public string DbPWD = ""; // dlitdb01
        public string connectionTimeout = ""; // 180
        public string S_DeviceTable = ""; // Devices
        public string S_UsageTable = ""; // SensorUsage

        private List<string> _deviceInfoColmn;
        public List<string> S_DeviceInfoColumns 
        { 
            get { return _deviceInfoColmn; } 
            set { _deviceInfoColmn = value; } 
        }
        
        
        private List<string> _fourRangeColmn;
        public List<string> S_FourRangeColumns 
        { get { return _fourRangeColmn; }
          set { _fourRangeColmn = value; } 
        }



        private SqlConnection _myConn;
        public SqlConnection myConn
        {
            get { return _myConn; }
            set { _myConn = value; }
        }
        
        
        private List<int> _IDs;
        public List<int> D_IDs
        {
            get { return _IDs; }
            set { _IDs = value; }
        }

        public Int64 dataCount { get; set; }
        public DateTime startTime { get; set; }

        private List<TextBox> _SensorDeviceInfo;
        /// <summary>
        /// sName, sLocation, sDescription 들이 들어가 있음.
        /// </summary>
        public List<TextBox> S_DeviceInfo_txtB
        {
            get { return _SensorDeviceInfo; }
            set { _SensorDeviceInfo = value; }
        }
        public Dictionary<CheckBox, List<NumericUpDown>> S_UsageCheckerRangePairs { get; set; }

        public DbTableHandler g_DbTableHandler;
        
        public string appAddress = @"C:\Users\JIMMY\source\repos\0DataCollectionAppNew\DataCollectionApp\bin\Release\Modbus_RTU_SensorData.EXE";
        public string DataCollectionAppName { get; set; }
        public FlaUI.Core.Application dataCollectionApp { get; set; }
        public bool appAlreadyRunning { get; set; }
        public Process applicationProcess { get; set; }

        public Form1()
        {
            InitializeComponent();

            DbServer = "localhost\\SQLEXPRESS";//"127.0.0.1";    //"10.1.55.174"; 
            //Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
            DbName = "SensorDataDB";
            DbUID = "dlitadmin";
            DbPWD = "dlitadmin";

            S_DeviceTable = "SENSOR_INFO";
            S_UsageTable = "SensorUsage";
            connectionTimeout = "180";

            DataCollectionAppName = "Modbus_RTU_SensorData";
            
            applicationProcess = new Process();
            pTrackerTimer.Enabled = true;
            pTrackerTimer.Start();
            //applicationProcess = GetAppProcess(DataCollectionAppName);
            
            listView1.Scrollable = true;

            S_DeviceInfo_txtB = new List<TextBox>() { sName, sZone, sLocation,  sDescription };
            S_DeviceInfoColumns = new List<string>() { sID.Name, S_DeviceInfo_txtB[0].Name, S_DeviceInfo_txtB[1].Name, S_DeviceInfo_txtB[2].Name, S_DeviceInfo_txtB[3].Name, "sUsage" };
            S_FourRangeColumns = new List<string>() { "higherLimit2", "higherLimit1", "lowerLimit1", "lowerLimit2" };


            List<CheckBox> S_UsageCheckers = new List<CheckBox>() { c_tUsage, c_hUsage, c_p03Usage, c_p05Usage, c_p10Usage, c_p25Usage, c_p50Usage, c_p100Usage };
            List<NumericUpDown> t_Ranges = new List<NumericUpDown>() { s_tHigherLimit2, s_tHigherLimit1, s_tLowerLimit1, s_tLowerLimit2 };
            List<NumericUpDown> h_Ranges = new List<NumericUpDown>() { s_hHigherLimit2, s_hHigherLimit1, s_hLowerLimit1, s_hLowerLimit2 };
            List<NumericUpDown> p03_Ranges = new List<NumericUpDown>() { s_p03HigherLimit2, s_p03HigherLimit1, s_p03LowerLimit1, s_p03LowerLimit2 };
            List<NumericUpDown> p05_Ranges = new List<NumericUpDown>() { s_p05HigherLimit2, s_p05HigherLimit1, s_p05LowerLimit1, s_p05LowerLimit2 };
            List<NumericUpDown> p10_Ranges = new List<NumericUpDown>() { s_p10HigherLimit2, s_p10HigherLimit1, s_p10LowerLimit1, s_p10LowerLimit2 };
            List<NumericUpDown> p25_Ranges = new List<NumericUpDown>() { s_p25HigherLimit2, s_p25HigherLimit1, s_p25LowerLimit1, s_p25LowerLimit2 };
            List<NumericUpDown> p50_Ranges = new List<NumericUpDown>() { s_p50HigherLimit2, s_p50HigherLimit1, s_p50LowerLimit1, s_p50LowerLimit2 };
            List<NumericUpDown> p100_Ranges = new List<NumericUpDown>() { s_p100HigherLimit2, s_p100HigherLimit1, s_p100LowerLimit1, s_p100LowerLimit2 };

            List<List<NumericUpDown>> S_Ranges = new List<List<NumericUpDown>>() { t_Ranges, h_Ranges, p03_Ranges, p05_Ranges, p10_Ranges, p25_Ranges, p50_Ranges, p100_Ranges };


            myConn = new SqlConnection($@"Data Source={DbServer};Initial Catalog={DbName};User id={DbUID};Password={DbPWD}; Min Pool Size=20"); // ; Integrated Security=True ");
            g_DbTableHandler = new DbTableHandler(new List<string>() { DbServer, DbName, DbUID, DbUID });

            g_DbTableHandler.MyConn = myConn;
            g_DbTableHandler.S_DeviceInfoColumns = S_DeviceInfoColumns;
            g_DbTableHandler.S_DeviceTable = this.S_DeviceTable;
            g_DbTableHandler.S_FourRangeColumns = S_FourRangeColumns;

            DataSet DeviceTable = GetDeviceInfo(DbName, S_DeviceTable);
            if (DeviceTable.Tables.Count > 0)
            {
                D_IDs = new List<int>(DeviceTable.Tables[0].AsEnumerable().Where(r => r.Field<string>(S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]) == "YES").Select(r => r.Field<int>(S_DeviceInfoColumns[0])).ToList());

                //ModBus and myConnection initialization
                //ConnectionSettings(true);

                dataCount = 0;

                string[] rows = new string[DeviceTable.Tables[0].Columns.Count];

                int num = 1;
                foreach (DataRow row in DeviceTable.Tables[0].Rows)
                {
                    ListViewItem listViewItem = new ListViewItem(num.ToString());
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
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

            clearFields(S_DeviceInfo_txtB);

        }


        /// <summary>
        /// Application Process를 반환함
        /// </summary>
        /// <param name="dataCollectionAppName"></param>
        /// <returns></returns>
        public Process GetAppProcess(string dataCollectionAppName)
        {
            int myCounter = 0;
            while (true)
            {
                if (myCounter > 2)
                {
                    break;
                }
                Process[] processes = Process.GetProcessesByName(dataCollectionAppName);
                if (processes.Length != 0)
                {
                    appAlreadyRunning = true;
                    b_dataCollection_status.Image = Resources.light_on_26_color;
                    applicationProcess = processes[0];
                    break;
                }
                else
                {
                    dataCollectionAppName = Microsoft.VisualBasic.Interaction.InputBox("찾으신 어플리케이션의 정확한 이름을 찾아서 입력해 주세요!", "Application Status", ".exe를 제외한 Application명만 입력", -1, -1);

                    //MessageBox.Show("찾으신 어플리케이션의 정확한 이름을 찾아서 입력해 주세요!", "Application Status", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                }
            }
            return applicationProcess;
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
        /// 센서 장비 테이블에 있는 모든 장비에 대한 ID를 List<int> 형태로 불러오는 함수
        /// </summary>
        /// <param name="S_IDs"></param>
        /// <returns></returns>
        private List<int> GetSensorIDs(List<int> S_IDs)
        {
            string IdCheckCmd = $"SELECT {S_DeviceInfoColumns[0]} FROM {DbName}.dbo.{S_DeviceTable} WHERE {S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]}='YES'";
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
                        S_IDs.Add(Convert.ToInt32(sqlDataReader[$"{S_DeviceInfoColumns[0]}"]));
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
                string sqlStrChecker = $"SELECT {S_DeviceInfoColumns[0]} FROM [{S_DeviceTable}] WHERE {S_DeviceInfoColumns[0]} = {id};";
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
                                if(Convert.ToInt32(reader[$"{S_DeviceInfoColumns[0]}"].ToString()) == id)
                                {
                                    idExists = true;
                                    break;
                                }
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
            string sqlStrChecker = $"SELECT {S_DeviceInfoColumns[0]} FROM {tableName} WHERE {S_DeviceInfoColumns[0]} = {id};";
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
                            idExists = Convert.ToInt32(reader[$"{S_DeviceInfoColumns[0]}"].ToString()) == id;
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
        private DataSet GetDeviceInfo(string sensorData_dbName, string Devices_tbName)
        {
            // Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
            //SqlConnection myConn_master = new SqlConnection($@"Data Source = {DbServer};Initial Catalog=master;Trusted_Connection=True");
            DataSet ds = new DataSet();
            bool checkDbExists = g_DbTableHandler.IfDatabaseExists(sensorData_dbName);
            
            string sqlCreateDeviceTb = $"CREATE TABLE {Devices_tbName} ({S_DeviceInfoColumns[0]} INT NOT NULL ";
            //  S_DeviceInfo_txtB 크기만큼은 loop를 통해 스트링에 추가
            for (int i=1; i<S_DeviceInfoColumns.Count-1; i++)
            {
                sqlCreateDeviceTb += $", {S_DeviceInfoColumns[i]} NVARCHAR(250) NULL ";                 
            }
                sqlCreateDeviceTb += $",{S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} NVARCHAR(20) NOT NULL);";



            if (checkDbExists)
            {
                bool Check_Device_tableExists = g_DbTableHandler.IfTableExists(Devices_tbName);
                if (Check_Device_tableExists)
                {
                    string sqlStr = $"SELECT * FROM {Devices_tbName}";
                    using (SqlConnection con = new SqlConnection($@"Data Source = {DbServer};Initial Catalog={DbName};User id={DbUID};Password={DbPWD};Min Pool Size=20"))
                    { //Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20")) // ; Integrated Security=True
                      //con.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlStr, con);
                        sqlDataAdapter.Fill(ds);
                    }
                }
                else
                {
                    /*DialogResult createTbOrNot = MessageBox.Show($"센서 정보 테이블을 생성합니다. \nDB명은 {dbName}, \n센서정보 테이블명 = {sensorInfo_tbName}. \n진행하시겠습니까?", "Status Info", MessageBoxButtons.YesNo);
                    if (createTbOrNot == DialogResult.Yes)
                    {*/
                        
                        bool Device_tableCreated = g_DbTableHandler.CreateTable(DbName, Devices_tbName, sqlCreateDeviceTb, myConn);
                        if (Device_tableCreated)
                        {
                            //MessageBox.Show($"센서 정보 DB와 테이블이 성공적으로 생성되었습니다!\nDB명 = {dbName}\n센서 정보 테이블명 = {sensorInfo_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"DB가 생성되었지만, 센서 정보 테이블이 성공적으로 생성되지 않았습니다!\nDB명 = {DbName}\n센서 정보 테이블명 = {Devices_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
/*                    }
                    else
                    {
                        MessageBox.Show("센서 정보 테이블이 생성되어 있지 않습니다.", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }*/
                }
            }
            else
            {
                /*DialogResult createTbOrNot = MessageBox.Show($"관리페이지에 오신 것을 환영합니다. \n센서 정보 DB와 테이블을 생성합니다. \nDB명은 {dbName}, \n센서정보 테이블명 = {sensorInfo_tbName}. \n진행하시겠습니까?", "Status Info", MessageBoxButtons.YesNo);
                if (createTbOrNot == DialogResult.Yes)
                {*/
                    string sqlCreateDb = $@"CREATE DATABASE {DbName};";

                    bool dataBase_Created = g_DbTableHandler.CreateDatabase(DbName, sqlCreateDb);
                    if (dataBase_Created)
                    {

                    bool Device_tableCreated = g_DbTableHandler.CreateTable(DbName, Devices_tbName, sqlCreateDeviceTb, myConn);
                        if (Device_tableCreated)
                        {
                            //MessageBox.Show($"센서 정보 DB와 테이블이 성공적으로 생성되었습니다!\nDB명 = {dbName}\n센서 정보 테이블명 = {sensorInfo_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"DB가 생성되었지만, 센서 정보 테이블이 성공적으로 생성되지 않았습니다!\nDB명 = {DbName}\n센서 정보 테이블명 = {Devices_tbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"센서 정보 DB가 성공적으로 생성되지 않았습니다!\nDB명 = {DbName}", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                /*}
                else
                {
                    MessageBox.Show("센서 정보 DB가 생성되어 있지 않습니다.", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }*/
            }
            return ds;
        }



        private void b_start_Click(object sender, EventArgs e)
        {

            if (!appAlreadyRunning)
            {

                dataCollectionApp = FlaUI.Core.Application.Launch(appAddress);
                using (var automation = new UIA3Automation())
                {
                    var window = dataCollectionApp.GetMainWindow(automation);
                    //MessageBox.Show("Hello, " + window.Title, window.Title);

                }
                b_dataCollection_status.Image = Resources.light_on_26_color;
                applicationProcess = GetAppProcess(DataCollectionAppName);
                appAlreadyRunning = true;
            }
            else
            {
                MessageBox.Show("데이터 수집 프로그램이 이미 실행중입니다.", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            if (appAlreadyRunning)
            {
                DialogResult dialog = MessageBox.Show("데이터 수집 프로그램을 중지하시겠습니까?", "Application status", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    applicationProcess = GetAppProcess(DataCollectionAppName);
                    applicationProcess.Kill();
                    b_dataCollection_status.Image = Resources.light_off_26;
                    appAlreadyRunning = false;
                    applicationProcess.Dispose();
                }
            }
            else
            {
                MessageBox.Show("데이터 수집 프로그램이 이미 중지되어 있습니다", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void F_Exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    int sensorId = Convert.ToInt32(item.SubItems[1].Text);
                    sID.Text = sensorId.ToString();
                    for (int i = 0; i < S_DeviceInfo_txtB.Count; i++)
                    {
                        S_DeviceInfo_txtB[i].Text = item.SubItems[i + 2].Text;
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
                            for (int j = 0; j < S_FourRangeColumns.Count; j++)
                            {
                                dataFromDB.Add(Convert.ToDecimal(rangesWithUsage.Tables[0].Rows[0][S_FourRangeColumns[j]]));
                            }

                            bool sUsage = rangesWithUsage.Tables[0].Rows[0][sUsageRangesTables[i]].ToString() == "YES";

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
            }
            else
            {
                clearFields(S_DeviceInfo_txtB);
            }
        }



        private DataSet GetRangesWithUsage(int s_Id, string c_xRangesTb) // c=checkBox, RangeTb = 범위테이블
        {
            DataSet ds = new DataSet();
            bool idExists = GetSensorID(s_Id);

            if (!idExists)
            {
                MessageBox.Show("DB에 존재하지 않는 ID입니다.", "Status info");
            }
            else
            {
                bool UsageTableExists = g_DbTableHandler.IfTableExists(S_UsageTable);
                if (UsageTableExists)
                {
                    bool sUsageInfoExists = g_DbTableHandler.IfTableExists(c_xRangesTb);
                    if (sUsageInfoExists)
                    {
                        string sqlGetRanges = $"SELECT DISTINCT ch.{S_FourRangeColumns[0]}, ch.{S_FourRangeColumns[1]}, ch.{S_FourRangeColumns[2]}, ch.{S_FourRangeColumns[3]}, su.{c_xRangesTb} FROM {S_UsageTable} su JOIN {c_xRangesTb} ch ON ch.{S_DeviceInfoColumns[0]} = su.{S_DeviceInfoColumns[0]} WHERE su.{S_DeviceInfoColumns[0]} = {s_Id}; ";
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
            }
            return ds;
        }



        private void b_save_Click(object sender, EventArgs e)
        {
            
            List<CheckBox> checkedItems = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => x.Checked).ToList();
            string sUsage = (checkedItems.Count > 0) ? "YES" : "NO";

            Dictionary<CheckBox, List<NumericUpDown>> dataToBeSaved = new Dictionary<CheckBox, List<NumericUpDown>>();
            

            //기존 센서 정보를 update하는 부분
            if (listView1.SelectedItems.Count > 0)
            {

                    int deviceId = Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text);
                    //Console.WriteLine("ID:" + listView1.SelectedItems[0].Text);
                    bool updated = UpdateDB(deviceId);
                    if (updated)
                    {
                        foreach (ListViewItem item in listView1.SelectedItems)
                        {
                            item.SubItems[1].Text = sID.Text;
                            for (int i = 0; i < S_DeviceInfo_txtB.Count; i++)
                            {
                                item.SubItems[i + 2].Text = S_DeviceInfo_txtB[i].Text;
                            }
                            item.SubItems[item.SubItems.Count - 1].Text = sUsage;
                        }
                    clearFields(S_DeviceInfo_txtB);
                        MessageBox.Show("센서 정보 DB 업데이트가 성공적으로 이루어졌습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
            }
            
            //새 장비 추가하는 부분
            else
            {
                    bool idExists = GetSensorID(Convert.ToInt32(sID.Text));
                    if (idExists)
                    {
                        MessageBox.Show("DB에 이미 존재하는 센서 장비 ID입니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else { 
                        int newOrderNumber;
                        if (listView1.Items.Count > 0)
                        {
                            newOrderNumber = Convert.ToInt32(listView1.Items[listView1.Items.Count - 1].Text) + 1;
                        }
                        else
                        {
                            newOrderNumber = 1;
                        }


                        //Added should return true if data added to DB.
                        bool added = AddToDB(sUsage);
                        if (added)
                        {
                            ListViewItem listViewItem = new ListViewItem(newOrderNumber.ToString());
                            listViewItem.SubItems.Add(sID.Text);
                            for(int i=0; i<S_DeviceInfo_txtB.Count; i++)
                        {
                            listViewItem.SubItems.Add(S_DeviceInfo_txtB[i].Text);
                        }
                            listViewItem.SubItems.Add(sUsage);
                            listView1.Items.Add(listViewItem);
                            clearFields(S_DeviceInfo_txtB);
                            MessageBox.Show("새 센서 장비 정보가 DB에 성공적으로 추가 되었습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(checkBox => checkBox.Checked = true);
            //S_UsageCheckerRangePairs.Values.AsEnumerable().Select(list => list.Select(item => item.Enabled = false));

            List<CheckBox> isCheked = S_UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
            List<List<NumericUpDown>> listNumbers = S_UsageCheckerRangePairs.Values.AsEnumerable().ToList();
            for(int i=0; i<isCheked.Count; i++)
            {
                isCheked[i].Checked = false;
                for(int j = 0; j < listNumbers[i].Count; j++)
                {
                    listNumbers[i][j].Value = 0;
                    listNumbers[i][j].Enabled = false;
                }
            }

        }



        /// <summary>
        /// 센서 장비 관련 데이터 테이블에 있는 센서 정보를 업데이트해 주는 함수.
        /// ID (textBoxes[0].Text) 기준으로 센서 정보가 업데이트가 됨.
        /// </summary>
        /// <param name="deviceIdOld">장비 ID </param>
        private bool UpdateDB(int deviceIdOld)
        {
            bool result_UPD = false;
            bool idExists = GetSensorID(Convert.ToInt32(sID.Text));
            bool sUsage;
            int deviceIdNew = Convert.ToInt32(sID.Text);
            if (idExists && deviceIdOld != deviceIdNew)
            {
                MessageBox.Show("DB에 이미 존재하는 센서 장비 ID입니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                List<CheckBox> S_UsageCheckersChecked = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(r => r.Checked).ToList();

                List<CheckBox> S_UsageCheckers = S_UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
                List<string> UsageTableColumns = S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Name).ToList();
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
                        result_UPD = g_DbTableHandler.UpdateLimitRangeInfo(deviceIdOld, deviceIdNew, targetChBoxes[i], S_UsageCheckerRangePairs[targetChBoxes[i]], myConn);
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
                    bool upd_SensorUsageTable = g_DbTableHandler.UpdateUsageTable(myConn, S_UsageTable, UsageTableColumns, deviceIdOld, deviceIdNew, UsageInfo);
                    if (upd_SensorUsageTable)
                    {
                        bool deviceInfoTbUpd = g_DbTableHandler.UpdateDeviceInfoTable(myConn, S_DeviceTable, deviceIdOld, deviceIdNew, S_DeviceInfo_txtB, sUsage);
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
        private bool AddToDB(string g_sensorUsage)
        {
            
            //SqlConnection myConn = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20");
            bool result = false;

            bool dbExists = g_DbTableHandler.IfDatabaseExists(DbName);
            if (dbExists)
            {
                int sensorId = Convert.ToInt32(sID.Text);

                //List<string> sRangeTablesChecked = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x=>x.Checked).Select(x => x.Name).ToList();
                List<CheckBox> sRangeTablesAll = S_UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
                //List<CheckBox> sRangeTablesChecked = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => x.Checked).ToList();
                List<string> sUsageResults = S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Checked ? "YES" : "NO").ToList();
                
                bool[] result_for_checked = new bool[sRangeTablesAll.Count];

                
                // 적정범위 정보를 DB에 저장하는 부분
                for (int i = 0; i < sRangeTablesAll.Count; i++)
                {
                    List<Decimal> sFourRangeTbVals = S_UsageCheckerRangePairs[sRangeTablesAll[i]].Select(x=>x.Value).ToList();
                    string sRangeTable = sRangeTablesAll[i].Name;

                    
                    bool checkRangesTb = g_DbTableHandler.IfTableExists(sRangeTable);

                    string sqlCreateTb = $"Create TABLE [{sRangeTable}] ( " +
                                $" {S_DeviceInfoColumns[0]} INT NOT NULL, " +
                                $" {S_FourRangeColumns[0]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColumns[1]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColumns[2]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColumns[3]} decimal(7,2) NULL);";



                    string sqlStrInsert = $"INSERT INTO [{sRangeTable}] " +
                                            $"({S_DeviceInfoColumns[0]}, {S_FourRangeColumns[0]}, {S_FourRangeColumns[1]}, {S_FourRangeColumns[2]}, {S_FourRangeColumns[3]}) " +
                                            $"VALUES({sensorId}, {sFourRangeTbVals[0]},{sFourRangeTbVals[1]},{sFourRangeTbVals[2]},{sFourRangeTbVals[3]});";

                    SqlCommand InsertCmd = new SqlCommand(sqlStrInsert, myConn);
                    

                    string sqlCheckUpd = $"SELECT 1 " +
                        $" FROM [{sRangeTable}]  " +
                        $" WHERE {S_DeviceInfoColumns[0]} = { sensorId} and " +
                        $" {S_FourRangeColumns[0]} = { sFourRangeTbVals[0]} and " +
                        $" {S_FourRangeColumns[1]} = { sFourRangeTbVals[1]} and " +
                        $" {S_FourRangeColumns[2]} = { sFourRangeTbVals[2]} and " +
                        $" {S_FourRangeColumns[3]} = { sFourRangeTbVals[3]}";
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
                                            result_for_checked[i] = Convert.ToInt32(reader.GetValue(0)) == 1;
                                        }
                                    }

                                        break;
                                }
                                else
                                {
                                    while (true)
                                    {
                                        counter += 1;
                                        int newID = GetUserInput();
                                        if (sensorId != newID)
                                        {
                                            sensorId = newID;
                                            sID.Text= newID.ToString();
                                            break;
                                        }
                                        if (errorLimit <= counter)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {                   // table이 존재하지 않는다면 CreateTable를 통해 테이블 생성한 후 Insert함
                            bool tbCreated = g_DbTableHandler.CreateTable(DbName, sRangeTable, sqlCreateTb, myConn);
                            
                            if (tbCreated)
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
                                       result_for_checked[i]  = Convert.ToInt32(reader.GetValue(0)) == 1;
                                    }
                                }
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
                
                
                result = !result_for_checked.Contains(false);


                bool tbExists = g_DbTableHandler.IfTableExists(S_UsageTable);
                if (!tbExists)
                {
                    List<string> sRangesTbNames = S_UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Name).ToList();
                    string sqlCreateUsageTb = $"CREATE TABLE {S_UsageTable}(" +
                        $" {S_DeviceInfoColumns[0]} INT NOT NULL ";
                    foreach(var item in sRangesTbNames)
                    {
                        sqlCreateUsageTb += $", {item} NVARCHAR(20) NOT NULL ";
                    }
                    
                        sqlCreateUsageTb += " );";
                   tbExists = g_DbTableHandler.CreateTable(DbName, S_UsageTable, sqlCreateUsageTb, myConn);
                }
                if (tbExists)
                {

                    string sqlInsertUsage = $"INSERT INTO {S_UsageTable} VALUES({sensorId} ";

                    foreach (var item in sUsageResults)
                    {
                        sqlInsertUsage += $", '{item}' ";
                    }

                    sqlInsertUsage += " );";
                    SqlCommand sqlInsertUsageCmd = new SqlCommand(sqlInsertUsage, myConn);
                    try
                    {
                        if (myConn.State != ConnectionState.Open)
                        {
                            myConn.Open();
                        }




                        sqlInsertUsageCmd.ExecuteNonQuery();


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
                    MessageBox.Show("센서 사용여부 정보 테이블이 정상적으로 생성되지 않았어요.", "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    result = false;
                }
                
                // 관련 테이블도 같이 업데이트함.
                string sqlInsert_DevicesTable = $"INSERT INTO {S_DeviceTable} ({S_DeviceInfoColumns[0]}, {S_DeviceInfoColumns[1]}, {S_DeviceInfoColumns[2]}, {S_DeviceInfoColumns[3]}, {S_DeviceInfoColumns[4]}, {S_DeviceInfoColumns[S_DeviceInfoColumns.Count-1]}) " +
                    $"VALUES ('{sensorId}', '{S_DeviceInfo_txtB[0].Text}', '{S_DeviceInfo_txtB[1].Text}', '{S_DeviceInfo_txtB[2].Text}','{S_DeviceInfo_txtB[3].Text}', '{g_sensorUsage}');";

                SqlCommand sqlCommand = new SqlCommand(sqlInsert_DevicesTable, myConn);
                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    sqlCommand.ExecuteNonQuery();
                    
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
                MessageBox.Show("센서 정보 DB를 찾을 수 없었습니다. 프르그램을 다시 실행한 후 DB부터 생성해 주세요.", "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return result;
        }



        private int GetUserInput()
        {
            int sensorId;
            //MessageBox.Show("추가하시려는 센서는 이미 DB에서 존재합니다. 기존에 있는 센서 정보를 수정하시고나 센서 ID를 바꿔 보세요. \n센서ID를 수정하시려면 'Y' 버튼을 누르세요.", "Status Info", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            //react to dialogResults, yes, no, cancel
            // title, message, textBox enter info, etc. #Fix needed
            string newSensorId = Microsoft.VisualBasic.Interaction.InputBox("같은 ID의 센서 정보가 존재합니다. 다른 ID번호를 입력해 주세요", "새 ID를 입력", "숫자만 입력해 주세요", -1, -1);
            try
            {
                sensorId = Convert.ToInt32(newSensorId);
            }
            catch
            {
                MessageBox.Show("숫자만 입력 가능합니다. 새로운 센서 장비 ID번호를 다시 입력해 주세요.", "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            S_DeviceInfo_txtB[0].Text = "PSU650";
            S_DeviceInfo_txtB[1].Text = "ZONE N";

            for (int i = 2; i < S_DeviceInfo_txtB.Count; i++)
            {
                //textBoxes_UpdSensorInfo[i].TextAlign = HorizontalAlignment.Center;
                S_DeviceInfo_txtB[i].Text = "";
            }


            if(listView1.Items.Count > 0) { 
                sID.Text= (Convert.ToInt32(listView1.Items[listView1.Items.Count - 1].SubItems[0].Text) + 1).ToString(); 
            }
            else
            {
                sID.Text = "1";
            }
            
            RangeSetNew();
            S_DeviceInfo_txtB[0].Focus();
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






        /// <summary>
        /// Custom function for Checking/Unchecking the checkbox control
        /// </summary>
        /// <param name="sender">Chechbox object</param>
        /// <param name="e"></param>
        private void xCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chBox = (CheckBox)sender;
            List<NumericUpDown> x_Ranges = S_UsageCheckerRangePairs[chBox];


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

        private void pTrackerTimer_Tick(object sender, EventArgs e)
        {
            appAlreadyRunning = CheckAppRunning(DataCollectionAppName);
            if (!appAlreadyRunning)
            {
                b_dataCollection_status.Image = Resources.light_off_26;
            }
            else
            {
                if(applicationProcess == null)
                {
                    applicationProcess = GetAppProcess(DataCollectionAppName);
                }
                b_dataCollection_status.Image = Resources.light_on_26_color;
            }
        }

        private bool CheckAppRunning(string dataCollectionAppName)
        {
            bool res = false;
            if(Process.GetProcessesByName(dataCollectionAppName).Length > 0)
            {
                res = true;
            }
            return res;
        }
    }
}
