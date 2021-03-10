using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using AdminPage.Properties;
using System.Diagnostics;
using System.IO;

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
        {
            get { return _fourRangeColmn; }
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
        /// (온습도 및 파티클 관련함)sName, sLocation, sDescription 들이 들어가 있음.
        /// </summary>
        public List<TextBox> S_DeviceInfo_txtB
        {
            get { return _SensorDeviceInfo; }
            set { _SensorDeviceInfo = value; }
        }


        /// <summary>
        /// (차압 관련함)sName, sLocation, sDescription 들이 들어가 있음.
        /// </summary>
        public List<TextBox> S_DeviceInfo_txtB_p { get; set; }

        public Dictionary<CheckBox, List<NumericUpDown>> S_UsageCheckerRangePairs { get; set; }

        public Dictionary<CheckBox, List<NumericUpDown>> S_UsageCheckerRangePairs_p { get; set; }

        public DbTableHandler g_DbTableHandler;

        public string appAddress = @"C:\Program Files (x86)\DLIT Inc\Sensor Data Collection App\SensorData Collection Application.exe";


        public string DataCollectionAppName { get; set; }
        public bool appAlreadyRunning { get; set; }
        public Process applicationProcess { get; set; }


        public Form1()
        {
            InitializeComponent();

            DbServer = "localhost\\SQLEXPRESS";//"127.0.0.1";    //"10.1.55.174"; 
            //Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
            DbName = "SensorDataNewDB";
            DbUID = "admin";
            DbPWD = "admin";

            S_DeviceTable = "SENSOR_INFO";
            S_UsageTable = "SensorUsage";
            connectionTimeout = "180";

            DataCollectionAppName = "SensorData Collection Application";

            applicationProcess = new Process();
            pTrackerTimer.Enabled = true;
            pTrackerTimer.Start();
            //applicationProcess = GetAppProcess(DataCollectionAppName);



            S_DeviceInfo_txtB = new List<TextBox>() { sName, sZone, sLocation, sDescription };
            S_DeviceInfo_txtB_p = new List<TextBox>() { sName_p, sZone_p, sLocation_p, sDescription_p };
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


            List<CheckBox> S_UsageCheckers_p = new List<CheckBox>() { c_paUsage, c_hpaUsage, c_kpaUsage, c_mmh2oUsage, c_inchh2oUsage, c_mmhgUsage, c_inchhgUsage };

            List<NumericUpDown> pa_Ranges = new List<NumericUpDown>() { s_paHigherLimit2, s_paHigherLimit1, s_paLowerLimit1, s_paLowerLimit2 };
            List<NumericUpDown> hPa_Ranges = new List<NumericUpDown>() { s_hpaHigherLimit2, s_hpaHigherLimit1, s_hpaLowerLimit1, s_hpaLowerLimit2 };
            List<NumericUpDown> kPa_Ranges = new List<NumericUpDown>() { s_kpaHigherLimit2, s_kpaHigherLimit1, s_kpaLowerLimit1, s_kpaLowerLimit2 };
            List<NumericUpDown> mmH2O_Ranges = new List<NumericUpDown>() { s_mmh2oHigherLimit2, s_mmh2oHigherLimit1, s_mmh2oLowerLimit1, s_mmh2oLowerLimit2 };
            List<NumericUpDown> inchH2O_Ranges = new List<NumericUpDown>() { s_inchh2oHigherLimit2, s_inchh2oHigherLimit1, s_inchh2oLowerLimit1, s_inchh2oLowerLimit2 };
            List<NumericUpDown> mmHg_Ranges = new List<NumericUpDown>() { s_mmhgHigherLimit2, s_mmhgHigherLimit1, s_mmhgLowerLimit1, s_mmhgLowerLimit2 };
            List<NumericUpDown> inchHg_Ranges = new List<NumericUpDown>() { s_inchhgHigherLimit2, s_inchhgHigherLimit1, s_inchhgLowerLimit1, s_inchhgLowerLimit2 };

            List<List<NumericUpDown>> S_Ranges_p = new List<List<NumericUpDown>>() { pa_Ranges, hPa_Ranges, kPa_Ranges, mmH2O_Ranges, inchH2O_Ranges, mmHg_Ranges, inchHg_Ranges };


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
                        listView1_thp.Columns[i].TextAlign = HorizontalAlignment.Center;
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

            clearFields(S_DeviceInfo_txtB);
            clearFields(S_DeviceInfo_txtB_p);

        }


        /// <summary>
        /// Application Process를 반환함
        /// </summary>
        /// <param name="dataCollectionAppName"></param>
        /// <returns></returns>
        public Process GetAppProcess(string dataCollectionAppName)
        {
            int myCounter = 0;
            while (myCounter < 2)
            {
                //dataCollectionAppName = "SensorData Collection Application";
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
                    myCounter += 1;
                    //MessageBox.Show("찾으신 어플리케이션의 정확한 이름을 찾아서 입력해 주세요!", "Application Status", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                }
            }
            return applicationProcess;
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
                if (myConn.State != ConnectionState.Open)
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
                            if (Convert.ToInt32(reader[$"{S_DeviceInfoColumns[0]}"].ToString()) == id)
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

            string sqlCreateDeviceTb = $"CREATE TABLE {Devices_tbName} ({S_DeviceInfoColumns[0]} INT NOT NULL, CONSTRAINT PK_{Devices_tbName}_{S_DeviceInfoColumns[0]} PRIMARY KEY ({S_DeviceInfoColumns[0]}) ";
            //  S_DeviceInfo_txtB 크기만큼은 loop를 통해 스트링에 추가
            for (int i = 1; i < S_DeviceInfoColumns.Count - 1; i++)
            {
                sqlCreateDeviceTb += $", {S_DeviceInfoColumns[i]} NVARCHAR(250) NULL ";

            }
            sqlCreateDeviceTb += $",{S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} NVARCHAR(20) NOT NULL, INDEX IX_{S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} NONCLUSTERED({S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]})); ";



            if (checkDbExists)
            {
                bool Check_Device_tableExists = g_DbTableHandler.IfTableExists(Devices_tbName);
                if (Check_Device_tableExists)
                {
                    string sqlStr = $"SELECT * FROM {Devices_tbName} ORDER BY {S_DeviceInfoColumns[0]}";
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
                        applicationProcess = GetAppProcess(DataCollectionAppName);
                        appAlreadyRunning = true;
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
                    MessageBox.Show("데이터 수집 프로그램이 이미 실행중입니다.", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                MessageBox.Show("차압 센서 데이터 수집 프로그램은 아직 구현이 안되어 있어요!", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if (tabControl1.SelectedTab == tabPage2)
            {
                MessageBox.Show("차압 센서 데이터 수집 프로그램은 아직 구현이 안되어 있어요!", "Application status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void F_Exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1_thp.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1_thp.SelectedItems)
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

            Dictionary<CheckBox, List<NumericUpDown>> dataToBeSaved = new Dictionary<CheckBox, List<NumericUpDown>>();
            List<CheckBox> checkedItems;
            string sUsage;


            if (tabControl1.SelectedTab == tabPage1)
            {
                checkedItems = S_UsageCheckerRangePairs.Keys.AsEnumerable().Where(x => x.Checked).ToList();
                sUsage = (checkedItems.Count > 0) ? "YES" : "NO";

                //기존 센서 정보를 update하는 부분
                if (listView1_thp.SelectedItems.Count > 0)
                {

                    int deviceId = Convert.ToInt32(listView1_thp.SelectedItems[0].SubItems[1].Text);
                    //Console.WriteLine("ID:" + listView1.SelectedItems[0].Text);
                    bool updated = UpdateDB(deviceId);
                    if (updated)
                    {
                        foreach (ListViewItem item in listView1_thp.SelectedItems)
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
                    else
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


                        //Added should return true if data added to DB.
                        bool added = AddToDB(sUsage);
                        if (added)
                        {
                            ListViewItem listViewItem = new ListViewItem(newOrderNumber.ToString());
                            listViewItem.SubItems.Add(sID.Text);
                            for (int i = 0; i < S_DeviceInfo_txtB.Count; i++)
                            {
                                listViewItem.SubItems.Add(S_DeviceInfo_txtB[i].Text);
                            }
                            listViewItem.SubItems.Add(sUsage);
                            listView1_thp.Items.Add(listViewItem);
                            clearFields(S_DeviceInfo_txtB);
                            MessageBox.Show("새 센서 장비 정보가 DB에 성공적으로 추가 되었습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                checkedItems = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Where(x => x.Checked).ToList();
                sUsage = (checkedItems.Count > 0) ? "YES" : "NO";

                //기존 센서 정보를 update하는 부분
                if (listView2_pressure.SelectedItems.Count > 0)
                {
                    int deviceId = Convert.ToInt32(listView2_pressure.SelectedItems[0].SubItems[1].Text);
                    //Console.WriteLine("ID:" + listView1.SelectedItems[0].Text);
                    bool updated = UpdateDB(deviceId);          // FIX UpdateDB 부문 
                    if (updated)
                    {
                        foreach (ListViewItem item in listView2_pressure.SelectedItems)
                        {
                            item.SubItems[1].Text = sID_p.Text;
                            for (int i = 0; i < S_DeviceInfo_txtB_p.Count; i++)
                            {
                                item.SubItems[i + 2].Text = S_DeviceInfo_txtB_p[i].Text;
                            }
                            item.SubItems[item.SubItems.Count - 1].Text = sUsage;
                        }
                        clearFields(S_DeviceInfo_txtB_p);
                        MessageBox.Show("센서 정보 DB 업데이트가 성공적으로 이루어졌습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                //새 장비 추가하는 부분
                else
                {
                    bool idExists = GetSensorID(Convert.ToInt32(sID_p.Text));      /// FIX GetSensorID 부문
                    if (idExists)
                    {
                        MessageBox.Show("DB에 이미 존재하는 센서 장비 ID입니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        int newOrderNumber;
                        if (listView2_pressure.Items.Count > 0)
                        {
                            newOrderNumber = Convert.ToInt32(listView2_pressure.Items[listView2_pressure.Items.Count - 1].Text) + 1;
                        }
                        else
                        {
                            newOrderNumber = 1;
                        }


                        //Added should return true if data added to DB.
                        bool added = AddToDB(sUsage);               /// FIX AddToDB 부문
                        if (added)
                        {
                            ListViewItem listViewItem = new ListViewItem(newOrderNumber.ToString());
                            listViewItem.SubItems.Add(sID_p.Text);
                            for (int i = 0; i < S_DeviceInfo_txtB_p.Count; i++)
                            {
                                listViewItem.SubItems.Add(S_DeviceInfo_txtB_p[i].Text);
                            }
                            listViewItem.SubItems.Add(sUsage);
                            listView2_pressure.Items.Add(listViewItem);
                            clearFields(S_DeviceInfo_txtB_p);
                            MessageBox.Show("새 센서 장비 정보가 DB에 성공적으로 추가 되었습니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("아직 구현이 안되어 있는 부문입니다.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void clearFields(List<TextBox> textBoxes)
        {
            for (int i = 0; i < textBoxes.Count; i++)
            {
                textBoxes[i].Text = "";
            }
            List<CheckBox> isCheked;
            List<List<NumericUpDown>> listNumbers;

            if (tabControl1.SelectedTab == tabPage1)
            {

                isCheked = S_UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
                listNumbers = S_UsageCheckerRangePairs.Values.AsEnumerable().ToList();

            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                isCheked = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().ToList();
                listNumbers = S_UsageCheckerRangePairs_p.Values.AsEnumerable().ToList();

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
            bool sUsage;
            int deviceIdNew;
            Dictionary<CheckBox, List<NumericUpDown>> UsageCheckerRangePairs;  //   One reference for all (S_UsageCheckerRangePairs or S_UsageCheckerRangePairs_p, etc...)

            if (tabControl1.SelectedTab == tabPage1)
            {
                sID_txtB = sID;
                sDeviceInfo_txtB = S_DeviceInfo_txtB;
                deviceIdNew = Convert.ToInt32(this.sID.Text);
                UsageCheckerRangePairs = S_UsageCheckerRangePairs;

            }
            else if(tabControl1.SelectedTab == tabPage2)
            {
                sID_txtB = sID_p;
                sDeviceInfo_txtB = S_DeviceInfo_txtB_p;
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
                        
                        result_UPD = g_DbTableHandler.UpdateLimitRangeInfo(deviceIdOld, deviceIdNew, targetChBoxes[i], UsageCheckerRangePairs[targetChBoxes[i]], myConn);
                        
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
                        bool deviceInfoTbUpd = g_DbTableHandler.UpdateDeviceInfoTable(myConn, S_DeviceTable, deviceIdOld, deviceIdNew, sDeviceInfo_txtB, sUsage);
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
                Dictionary<CheckBox, List<NumericUpDown>> UsageCheckerRangePairs;       // generic reference for more than one type: UsageCheckerRangePairs, UsageCheckerRangePairs_p, etc.
                if (tabControl1.SelectedTab == tabPage1)
                {
                    UsageCheckerRangePairs = S_UsageCheckerRangePairs;
                }
                else
                {
                    UsageCheckerRangePairs = S_UsageCheckerRangePairs_p;
                }



                List<CheckBox> sRangeTablesAll = UsageCheckerRangePairs.Keys.AsEnumerable().ToList();
                List<string> sUsageResults = UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Checked ? "YES" : "NO").ToList();

                

                bool[] result_for_checked = new bool[sRangeTablesAll.Count];


                // 적정범위 정보를 DB에 저장하는 부분
                for (int i = 0; i < sRangeTablesAll.Count; i++)
                {
                    List<Decimal> sFourRangeTbVals = UsageCheckerRangePairs[sRangeTablesAll[i]].Select(x => x.Value).ToList();
                    string sRangeTable = sRangeTablesAll[i].Name;


                    bool checkRangesTb = g_DbTableHandler.IfTableExists(sRangeTable);

                    string sqlCreateTb = $"Create TABLE {sRangeTable} ( " +
                                $" {S_DeviceInfoColumns[0]} INT NOT NULL, " +
                                $" {S_FourRangeColumns[0]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColumns[1]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColumns[2]} decimal(7,2) NULL, " +
                                $" {S_FourRangeColumns[3]} decimal(7,2) NULL, " +
                                $" CONSTRAINT PK_{sRangeTable}_{S_DeviceInfoColumns[0]} PRIMARY KEY ({S_DeviceInfoColumns[0]}))";



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
                                            sID.Text = newID.ToString();
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
                                        result_for_checked[i] = Convert.ToInt32(reader.GetValue(0)) == 1;
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
                    List<string> sRangesTbNames = UsageCheckerRangePairs.Keys.AsEnumerable().Select(x => x.Name).ToList();
                    string sqlCreateUsageTb = $"CREATE TABLE {S_UsageTable}(" +
                        $" {S_DeviceInfoColumns[0]} INT NOT NULL, CONSTRAINT PK_{S_UsageTable}_{S_DeviceInfoColumns[0]} PRIMARY KEY ({S_DeviceInfoColumns[0]})  ";
                    foreach (var item in sRangesTbNames)
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
                string sqlInsert_DevicesTable = $"INSERT INTO {S_DeviceTable} ({S_DeviceInfoColumns[0]}, {S_DeviceInfoColumns[1]}, {S_DeviceInfoColumns[2]}, {S_DeviceInfoColumns[3]}, {S_DeviceInfoColumns[4]}, {S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]}) " +
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
                MessageBox.Show("센서 정보 DB를 찾을 수 없거나 연결을 못했습니다. 프르그램을 다시 실행한 후 DB부터 생성해 주세요.", "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
            if (tabControl1.SelectedTab == tabPage1)
            {
                if (listView1_thp.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView1_thp.SelectedItems[0];
                    item.Selected = false;

                }
                //S_DeviceInfo_txtB[0].Text = "PSU650";
                S_DeviceInfo_txtB[1].Text = "ZONE";

                for (int i = 2; i < S_DeviceInfo_txtB.Count; i++)
                {
                    //textBoxes_UpdSensorInfo[i].TextAlign = HorizontalAlignment.Center;
                    S_DeviceInfo_txtB[i].Text = "";
                }


                if (listView1_thp.Items.Count > 0)
                {
                    sID.Text = (Convert.ToInt32(listView1_thp.Items[listView1_thp.Items.Count - 1].SubItems[0].Text) + 1).ToString();
                }
                else
                {
                    sID.Text = "1";
                }

                RangeSetNew();
                S_DeviceInfo_txtB[0].Focus();
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                if (listView2_pressure.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView2_pressure.SelectedItems[0];
                    item.Selected = false;

                }
                //S_DeviceInfo_txtB[0].Text = "PSU650";
                S_DeviceInfo_txtB_p[1].Text = "ZONE";

                for (int i = 2; i < S_DeviceInfo_txtB_p.Count; i++)
                {
                    //textBoxes_UpdSensorInfo[i].TextAlign = HorizontalAlignment.Center;
                    S_DeviceInfo_txtB_p[i].Text = "";
                }


                if (listView2_pressure.Items.Count > 0)
                {
                    sID_p.Text = (Convert.ToInt32(listView2_pressure.Items[listView2_pressure.Items.Count - 1].SubItems[0].Text) + 1).ToString();
                }
                else
                {
                    sID_p.Text = "1";
                }

                RangeSetNew();
                S_DeviceInfo_txtB_p[0].Focus();
            }
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
            appAlreadyRunning = CheckAppRunning(DataCollectionAppName);
            if (!appAlreadyRunning)
            {
                b_dataCollection_status.Image = Resources.light_off_26;
            }
            else
            {
                if (applicationProcess == null)
                {
                    applicationProcess = GetAppProcess(DataCollectionAppName);
                }
                b_dataCollection_status.Image = Resources.light_on_26_color;
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
            if (Process.GetProcessesByName(dataCollectionAppName).Length > 0)
            {
                res = true;
            }
            return res;
        }

        private void DownloadToExcel_Click(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            string startTime = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");
            DownToExcel downToExcel = new DownToExcel(tbName: "d_p03Usage", sqlConnection: myConn, (startTime, endTime));

        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                clearFields(S_DeviceInfo_txtB);
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
                clearFields(S_DeviceInfo_txtB_p);
                // Further FIX is Needed after Pressure sensor data collection is added

                if (appAlreadyRunning && "온습도" == "차압")
                {
                    b_dataCollection_status.Image = Resources.light_on_26_color;
                }
                else
                {
                    b_dataCollection_status.Image = Resources.light_off_26;
                    pTrackerTimer.Stop();
                    pTrackerTimer.Enabled = false;
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
                    for (int i = 0; i < S_DeviceInfo_txtB.Count; i++)
                    {
                        S_DeviceInfo_txtB_p[i].Text = item.SubItems[i + 2].Text;
                        //S_DeviceInfo_txtB[i].TextAlign = HorizontalAlignment.Center;
                    }
                    List<CheckBox> sUsageRangesCh = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().ToList();
                    List<string> sUsageRangesTables = S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Select(x => x.Name).ToList();

                    for (int i = 0; i < sUsageRangesTables.Count; i++)
                    {

                        DataSet rangesWithUsage = GetRangesWithUsage(sensorId, sUsageRangesTables[i]);

                        // first time use
                        if (rangesWithUsage.Tables.Count == 0 || rangesWithUsage.Tables[0].Rows.Count == 0)
                        {
                            S_UsageCheckerRangePairs_p.Keys.AsEnumerable().Select(x => x.Checked = false);
                            S_UsageCheckerRangePairs_p.Values.AsEnumerable().Select(list => list.Select(x => x.Enabled = false));
                        }
                        else
                        {
                            List<decimal> dataFromDB = new List<decimal>();
                            for (int j = 0; j < S_FourRangeColumns.Count; j++)
                            {
                                dataFromDB.Add(Convert.ToDecimal(rangesWithUsage.Tables[0].Rows[0][S_FourRangeColumns[j]]));
                            }

                            bool sUsage = rangesWithUsage.Tables[0].Rows[0][sUsageRangesTables[i]].ToString() == "YES";

                            List<NumericUpDown> checkers = S_UsageCheckerRangePairs_p[sUsageRangesCh[i]];

                            for (int j = 0; j < checkers.Count; j++)
                            {
                                //checkers[i].Value = dataFromDB[i];
                                S_UsageCheckerRangePairs_p[sUsageRangesCh[i]][j].Value = dataFromDB[j];

                            }
                            CheckBox currentCheckBox = S_UsageCheckerRangePairs_p.Keys.Where(x => x.Name == sUsageRangesTables[i]).FirstOrDefault();
                            currentCheckBox.Checked = sUsage;

                        }
                    }
                }
            }
            else
            {
                clearFields(S_DeviceInfo_txtB_p);
            }
        }
    }
}
