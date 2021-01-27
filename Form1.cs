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
        public string dbServer = "127.0.0.1";    //"10.1.55.174";
        public string dbName = "SensorDataDB";
        public string dbUID = "dlitdb01";
        public string dbPWD = "dlitdb01";
        public string connectionTimeout = "180";
        public ModbusClient modbusClient { get; set; }
        public SqlConnection myConnection { get; set; }
        public List<int> S_IDs { get; set; }
        public Int64 dataCount { get; set; }
        public DateTime startTime { get; set; }
        public List<TextBox> textBoxes_UpdSensorInfo { get; set; }
        public List<TextBox> textBoxes_LiveData { get; set; }

        public List<CheckBox> S_UsageCheckers { get; set; }
        public List<NumericUpDown> S_Ranges { get; set; }
        public List<NumericUpDown> t_Ranges { get; set; }
        public List<NumericUpDown> h_Ranges { get; set; }
        public List<NumericUpDown> p03_Ranges { get; set; }
        public List<NumericUpDown> p05_Ranges { get; set; }
        public List<NumericUpDown> p10_Ranges { get; set; }
        public List<NumericUpDown> p25_Ranges { get; set; }
        public List<NumericUpDown> p50_Ranges { get; set; }
        public List<NumericUpDown> p100_Ranges { get; set; }

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
           /* try
            {*/

                String[] sensordata = { "ID", "Temp", "Humidity", "Part03", "Part05", "DateTime" };
                Console.WriteLine("Count\t" + string.Join("\t", sensordata) + "\t\t Run Time");
                DataSet sensorInfoTable = GetSensorInfo();
                S_IDs = new List<int>(sensorInfoTable.Tables[0].AsEnumerable().Where(r => r.Field<string>("sUsage") == "YES").Select(r=>r.Field<int>("sID")).ToList());

                //ModBus and myConnection initialization
                ConnectionSettings(false);
                
                dataCount = 0;

                string[] rows = new string[sensorInfoTable.Tables[0].Columns.Count];

            int num = 1;
                foreach(DataRow row in sensorInfoTable.Tables[0].Rows)
                {
                    Console.WriteLine(row["sID"]);

                    ListViewItem listViewItem = new ListViewItem(num.ToString());
                    for (int i=0; i<row.ItemArray.Length; i++)
                    {
                        //if(row.ItemArray[i].ToString())
                        //rows[i] = row.ItemArray[i].ToString();
                        listViewItem.SubItems.Add(row.ItemArray[i].ToString());
                        listView1.Columns[i].TextAlign = HorizontalAlignment.Center;
                    }
                    listView1.Items.Add(listViewItem);
                num += 1;
                }


                //display listView1 sensor info in listView2
                //Display_listView2();
                //Display_GridView();

                textBoxes_UpdSensorInfo = new List<TextBox>() { sName, sLocation, sDescription  }; 
                textBoxes_LiveData = new List<TextBox>() { t_no, t_temp, t_humid, t_part03, t_part05, t_time };
                List<ColumnHeader> lvColHeaders = new List<ColumnHeader>() { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 };
                S_UsageCheckers = new List<CheckBox>() { c_tUsage, c_hUsage, c_p03Usage, c_p05Usage, c_p10Usage, c_p25Usage, c_p50Usage, c_p100Usage };
            t_Ranges = new List<NumericUpDown>() { s_tLowerLimit1, s_tLowerLimit2, s_tHigherLimit1, s_tHigherLimit2 };
            h_Ranges = new List<NumericUpDown>() { s_hLowerLimit1, s_hLowerLimit2, s_hHigherLimit1, s_hHigherLimit2 };
            p03_Ranges = new List<NumericUpDown>() { s_p03LowerLimit1, s_p03LowerLimit2, s_p03HigherLimit1, s_p03HigherLimit2 };
            p05_Ranges = new List<NumericUpDown>() { s_p05LowerLimit1, s_p05LowerLimit2, s_p05HigherLimit1, s_p05HigherLimit2 };
            p10_Ranges = new List<NumericUpDown>() { s_p10LowerLimit1, s_p10LowerLimit2, s_p10HigherLimit1, s_p10HigherLimit2 };
            p25_Ranges = new List<NumericUpDown>() { s_p25LowerLimit1, s_p25LowerLimit2, s_p25HigherLimit1, s_p25HigherLimit2 };
            p50_Ranges = new List<NumericUpDown>() { s_p50LowerLimit1, s_p50LowerLimit2, s_p50HigherLimit1, s_p50HigherLimit2 };
            p100_Ranges = new List<NumericUpDown>() { s_p100LowerLimit1, s_p100LowerLimit2, s_p100HigherLimit1, s_p100HigherLimit2 };





            S_Ranges = new List<NumericUpDown>();
            
                /*DataSet rangesWithUsage = GetRangesWithUsage(Convert.ToInt32(sID.Value));
                S_Ranges.AddRange((IEnumerable<NumericUpDown>)rangesWithUsage.Tables[0].Columns.Cast<DataColumn>()
                .Select(x=>x.ColumnName)
                .ToList());
            textBoxes_UpdSensorInfo.Select(r => r.TextAlign = HorizontalAlignment.Center);*/
            startTime = DateTime.Now;
            /*}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러 메시지");
            }*/
                
                    
            
        }




       /*private void Display_GridView()
        {
            for(int i=0; i<listView1.Columns.Count; i++)
            {
                string[] row = { listView1.Columns[i].Text, "" };
                dataGridView1.Rows.Add(row);
            }
        }*/




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

                myConnection = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20"); // ; Integrated Security=True ");
            }                                                                                                                              //myConnection.Open();
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
            catch(Exception ex)
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
            string IdCheckCmd = "SELECT sID FROM SensorDataDB.dbo.SENSOR_INFO WHERE sUsage='YES'";
            using (SqlConnection conn = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20")) // ; Integrated Security=True
            {
                Console.WriteLine("Usable sensor IDs:");
                SqlCommand sqlCommand = new SqlCommand(IdCheckCmd, conn);
                conn.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        S_IDs.Add(Convert.ToInt32(sqlDataReader["sID"]));
                        Console.WriteLine(Convert.ToInt32(sqlDataReader["sID"]));
                    }
                }
                conn.Close();
            }
            return S_IDs;
        }
        

        
        /// <summary>
        /// 주어진 센서 ID가 SENSOR_INFO테이블에 있는지 확인하고 bool형태의 값을 반환해주는 함수
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool GetSensorID(int id)
        {
            bool idExists = false;
            using (SqlConnection con = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
            {
                string sqlStrChecker = $"SELECT sID FROM [{dbName}].[dbo].[SENSOR_INFO] WHERE sID = {id};";
                using (SqlCommand sqlIdCheckerCmd = new SqlCommand(sqlStrChecker, con))
                {
                    con.Open();
                    using (SqlDataReader oReader = sqlIdCheckerCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            if (Convert.ToInt32(oReader["sID"].ToString()) == Convert.ToInt32(id))
                            {
                                idExists = true;
                            }
                        }
                    }
                }
            }
            return idExists;
        }



        /// <summary>
        /// SENSOR_INFO테이블에 있는 모든 정보를 DataSet형태로 불러오는 함수
        /// </summary>
        /// <returns></returns>
        private DataSet GetSensorInfo()
        {
            //List<string> sensorInfoTable = new List<string>();
            string sqlStr = "SELECT * FROM SensorDataDB.dbo.SENSOR_INFO";
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection($@"Data Source = {dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
            { //Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20")) // ; Integrated Security=True
                //con.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlStr, con);
                sqlDataAdapter.Fill(ds);
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
                DisplayLiveData(allData);
                string sql_str_temp = "INSERT INTO DEV_TEMP_" + s_id.ToString() + " (Temperature, DateAndTime) Values (@Temperature, @DateAndTime)";
                string sql_str_humid = "INSERT INTO DEV_HUMID_" + s_id.ToString() + " (Humidity, DateAndTime) Values (@Humidity, @DateAndTime)";
                string sql_str_part03 = "INSERT INTO DEV_PART03_" + s_id.ToString() + " (Particle03, DateAndTime) Values (@Particle03, @DateAndTime)";
                string sql_str_part05 = "INSERT INTO DEV_PART05_" + s_id.ToString() + " (Particle05, DateAndTime) Values (@Particle05, @DateAndTime)";

                SqlCommand myCommand_temp = new SqlCommand(sql_str_temp, myConnection);
                SqlCommand myCommand_humid = new SqlCommand(sql_str_humid, myConnection);
                SqlCommand myCommand_part03 = new SqlCommand(sql_str_part03, myConnection);
                SqlCommand myCommand_part05 = new SqlCommand(sql_str_part05, myConnection);

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
            
            for(int i=0; i<listView1.Columns.Count; i++)
            {
                ListViewItem listViewItem = new ListViewItem(listView1.Columns[i].Text);
                listViewItem.SubItems.Add("");
                //listView2.Items.Add(listViewItem);
            }
            //listView2.SmallImageList = imgList;
        }








        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView1.SelectedItems)
            {
                sID.Value = Convert.ToInt32(item.SubItems[1].Text);
                for(int i=0;i<textBoxes_UpdSensorInfo.Count; i++)
                {
                    textBoxes_UpdSensorInfo[i].Text = item.SubItems[i+2].Text;
                    textBoxes_UpdSensorInfo[i].TextAlign = HorizontalAlignment.Center;
                }
                DataSet rangesWithUsage = GetRangesWithUsage(Convert.ToInt32(sID.Value));

                // first time use
                if(rangesWithUsage.Tables[0].Rows.Count == 0)
                {
                    for(int i = 0; i<S_UsageCheckers.Count; i++)
                    {
                        S_UsageCheckers[i].Checked = false;
                    }
                    for(int i=0; i<4; i++)
                    {
                        t_Ranges[i].Enabled = false;
                        h_Ranges[i].Enabled = false;
                        p03_Ranges[i].Enabled = false;
                        p05_Ranges[i].Enabled = false;
                        p10_Ranges[i].Enabled = false;
                        p25_Ranges[i].Enabled = false;
                        p50_Ranges[i].Enabled = false;
                        p100_Ranges[i].Enabled = false;
                    }
                }
                else
                {
                    //do smth else;
                }
            }
        }

        private DataSet GetRangesWithUsage(int s_Id)
        {
            DataSet ds = new DataSet(); 
            bool idExists = GetSensorID(s_Id);

            if (!idExists)
            {
                MessageBox.Show("DB에 존재하지 않는 ID입니다.", "Status info");
            }
            else
            {
                bool sUsageInfoExists = CheckUsageInfoAll();
                if (sUsageInfoExists)
                {
                    string sqlStr = $"SELECT * FROM [dbo].[sUsageInfoAll]; ";
                    using (SqlConnection con = new SqlConnection($@"Data Source = {dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
                    { //Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20")) // ; Integrated Security=True
                      //con.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlStr, con);
                        sqlDataAdapter.Fill(ds);
                    }

                }
            }
            return ds;
        }

        private bool CheckUsageInfoAll()
        {
            bool exists = false;
            using (SqlConnection con = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
            {
                string sqlStr = "SELECT COUNT(*) " +
                "FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_NAME = 'sUsageInfoAll'";
                using(SqlCommand cmd = new SqlCommand(sqlStr, con))
                {
                    con.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exists = Convert.ToInt32(reader.GetValue(0)) == 1;
                        }
                    }
                }
                con.Close();

            }

            return exists;
        }

        private void b_save_Click(object sender, EventArgs e)
        {
            bool emptyColumn = false;
            for (int i = 0; i < textBoxes_UpdSensorInfo.Count; i++)
            {
                if (textBoxes_UpdSensorInfo[i].Text.Length < 1)
                {
                    emptyColumn = true;
                }
            }
            if (listView1.SelectedItems.Count > 0)
            {
                if (emptyColumn)
                {
                    MessageBox.Show($"빈칸이 있어요.");
                }
                else
                {
                    Console.WriteLine("ID:" + listView1.SelectedItems[0].Text);
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        for (int i = 0; i < textBoxes_UpdSensorInfo.Count; i++)
                        {
                            item.SubItems[i].Text = textBoxes_UpdSensorInfo[i].Text;
                        }
                    }

                    UpdateDB(new List<TextBox>(textBoxes_UpdSensorInfo));
                    clearFields(new List<TextBox>(textBoxes_UpdSensorInfo));
                }

            }
            else
            {
                if (emptyColumn)
                {
                    MessageBox.Show($"빈칸이 있어요.");
                }
                else
                {
                    ListViewItem listViewItem = new ListViewItem(textBoxes_UpdSensorInfo[0].Text);

                    listViewItem.SubItems.Add(textBoxes_UpdSensorInfo[1].Text);
                    listViewItem.SubItems.Add(textBoxes_UpdSensorInfo[2].Text);
                    listViewItem.SubItems.Add(textBoxes_UpdSensorInfo[3].Text);
                    listViewItem.SubItems.Add(textBoxes_UpdSensorInfo[4].Text);
                    listView1.Items.Add(listViewItem);

                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        Console.WriteLine("listView IDs:" + listView1.Items[i].Text);
                    }
                    AddToDB(new List<TextBox>(textBoxes_UpdSensorInfo));
                    clearFields(new List<TextBox>(textBoxes_UpdSensorInfo));
                }
            }
        }



        private void clearFields(List<TextBox> textBoxes)
        {
            for(int i=0; i<textBoxes.Count; i++)
            {
                textBoxes[i].Text = "";
            }
        }


        /// <summary>
        /// SENSOR_INFO테이블에 있는 센서 정보를 업데이트해 주는 함수.
        /// ID (textBoxes[0].Text) 기준으로 센서 정보가 업데이트가 됨.
        /// </summary>
        /// <param name="textBoxes">업데이트되는 정보를 가지고 있음.  </param>
        private void UpdateDB(List<TextBox> textBoxes)
        {
            bool idExists = GetSensorID(Convert.ToInt32(textBoxes[0].Text));
                
            if (!idExists)
            {
                MessageBox.Show("DB에 존재하지 않는 ID입니다.", "Status info");
            }
            else
            {
                using (SqlConnection con = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
                {
                    string sqlStr = $"UPDATE {dbName}.dbo.SENSOR_INFO " +
                                        $"SET Name = '{textBoxes[1].Text}', " +
                                            $"Location = '{textBoxes[2].Text}', " +
                                            $"Description = '{textBoxes[3].Text}', " +
                                            $"Usage = '{textBoxes[4].Text}' " +
                                        $"WHERE ID = '{textBoxes[0].Text}'; ";
                    using (SqlCommand sqlCommand = con.CreateCommand())
                    {
                        sqlCommand.CommandText = sqlStr;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("DB Update Successful.", "Status info");
                        con.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 새로운 센서 정보를 SENSOR_INFO테이블에 추가해주는 함수.
        /// </summary>
        /// <param name="textBoxes">센서 정보를 가지고 있음.</param>
        private void AddToDB(List<TextBox> textBoxes)
        {
            using (SqlConnection con = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
            {
                string sqlStr = $"INSERT INTO {dbName}.dbo.SENSOR_INFO (ID, Name, Location, Description, Usage) " +
                    $"VALUES ('{textBoxes[0].Text}', '{textBoxes[1].Text}', '{textBoxes[2].Text}', '{textBoxes[3].Text}', '{textBoxes[4].Text}');";
                using(SqlCommand sqlCommand = con.CreateCommand())
                {
                    sqlCommand.CommandText = sqlStr;
                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("New sensor info has been successfully saved", "Status info");
                    con.Close();
                }
            }
            
        }


        
        private void b_add_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                item.Selected = false;
            }
            for (int i=0; i<textBoxes_UpdSensorInfo.Count; i++)
            {
                if (i == 0)
                {
                    textBoxes_UpdSensorInfo[i].Text = (listView1.Items.Count + 1).ToString();
                }
                else if(i == textBoxes_UpdSensorInfo.Count - 1)
                {
                    textBoxes_UpdSensorInfo[i].Text = "NO";
                    textBoxes_UpdSensorInfo[i].TextAlign = HorizontalAlignment.Center;
                }
                else
                {
                    textBoxes_UpdSensorInfo[i].Text = "";
                }

            }
            RangeSetNew();

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
        


        private void DisplayLiveData(string[] data)
        {
            for(int i=0; i<data.Length; i++)
            {
                textBoxes_LiveData[i].Text = data[i];
            }
        }

        
        
        private void b_deleteSensor_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                DelFromDB(textBoxes_UpdSensorInfo);
                
                ListViewItem item = listView1.SelectedItems[0];
                item.Remove();

                clearFields(textBoxes_UpdSensorInfo);
            }
            else
            {
                MessageBox.Show("선택된 센서가 없습니다!", "Warning Message");
            }
        }
        
        
        
        
        private void DelFromDB(List<TextBox> textBoxes)
        {
            bool idExists = GetSensorID(Convert.ToInt32(textBoxes[0].Text));

            if (!idExists)
            {
                MessageBox.Show("DB에 존재하지 않는 ID입니다.", "Status info");
            }
            else
            {
                using (SqlConnection con = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
                {
                    string sqlStr = $"DELETE FROM {dbName}.dbo.SENSOR_INFO " +
                                        $"WHERE ID = '{textBoxes[0].Text}';";
                    using (SqlCommand sqlCommand = con.CreateCommand())
                    {
                        sqlCommand.CommandText = sqlStr;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Sensor Deletion Successful.", "Status info");
                        con.Close();
                    }
                }
            }
        }

       


        private void c_tUsage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_tUsage, t_Ranges);
        }


        private void c_hUsage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_hUsage, h_Ranges);
        }


        private void c_p03Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p03Usage, p03_Ranges);
        }


        private void c_p05Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p05Usage, p05_Ranges);

        }

        private void c_p10Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p10Usage, p10_Ranges);

        }

        private void c_p25Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p25Usage, p25_Ranges);

        }

        private void c_p50Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p50Usage, p50_Ranges);

        }

        private void c_p100Usage_CheckedChanged(object sender, EventArgs e)
        {
            c_xUsage_Checker(c_p100Usage, p100_Ranges);

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
                if (item.Value > 0)
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
            for(int i=0; i < S_UsageCheckers.Count;i++)
            {
                S_UsageCheckers[i].Checked = false;
            }
            for(int i=0; i<4; i++)
            {
                t_Ranges[i].Enabled = false;
                h_Ranges[i].Enabled = false;
                p03_Ranges[i].Enabled = false;
                p05_Ranges[i].Enabled = false;
                p10_Ranges[i].Enabled = false;
                p25_Ranges[i].Enabled = false;
                p50_Ranges[i].Enabled = false;
                p100_Ranges[i].Enabled = false;
            }
        }

    }
}
