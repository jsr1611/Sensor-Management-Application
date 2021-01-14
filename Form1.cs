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
        public List<TextBox> textBoxes { get; set; }

        public Form1()
        {
            InitializeComponent();

            MyFunc();

        }
        private void MyFunc()
        {
            try
            {

                String[] sensordata = { "ID", "Temp", "Humidity", "Part03", "Part05", "DateTime" };
                Console.WriteLine("Count\t" + string.Join("\t", sensordata) + "\t\t Run Time");
                DataSet sensorInfoTable = GetSensorInfo();
                S_IDs = new List<int>(sensorInfoTable.Tables[0].AsEnumerable().Where(r => r.Field<string>("Usage") == "YES").Select(r=>r.Field<int>("ID")).ToList());
                
                modbusClient = new ModbusClient("COM3");
                modbusClient.Baudrate = 115200;	// Not necessary since default baudrate = 9600
                modbusClient.Parity = System.IO.Ports.Parity.None;
                modbusClient.StopBits = System.IO.Ports.StopBits.Two;
                modbusClient.ConnectionTimeout = 5000;
                //modbusClient.Connect();
                Console.WriteLine("Device Connection Successful");
                myConnection = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20"); // ; Integrated Security=True ");
                //myConnection.Open();
                dataCount = 0;
                
                listView1.Scrollable = true;
                
                //listView1.Columns[0].TextAlign = HorizontalAlignment.Center;

                string[] rows = new string[sensorInfoTable.Tables[0].Columns.Count];
                foreach(DataRow row in sensorInfoTable.Tables[0].Rows)
                {
                    Console.WriteLine(row["ID"]);

                    ListViewItem listViewItem = new ListViewItem(row.ItemArray[0].ToString());
                    for (int i=1; i<row.ItemArray.Length; i++)
                    {
                        //rows[i] = row.ItemArray[i].ToString();
                        listViewItem.SubItems.Add(row.ItemArray[i].ToString());
                    }
                    listView1.Items.Add(listViewItem);
                }

                textBoxes = new List<TextBox>() { textBox1, textBox2, textBox3, textBox4, textBox5 };
                List<ColumnHeader> lvColHeaders = new List<ColumnHeader>() { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 };
                textBoxes[0].Left = listView1.Bounds.X; //lvColHeaders[i].ListView.Bounds.X;
                textBoxes[0].Width = listView1.Columns[0].Width;
                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    listView1.Columns[i].TextAlign = HorizontalAlignment.Center;
                    if (i != 0)
                    {
                        textBoxes[i].Left = textBoxes[i - 1].Bounds.Right;
                        textBoxes[i].Width = listView1.Columns[i].Width;
                    }
                    
                        Console.WriteLine("txtBox:", textBoxes[i].Left, textBoxes[i].Width);
                    
                }
                Console.WriteLine(rows);
                
                textBoxes.Select(r => r.TextAlign = HorizontalAlignment.Center);
                
                startTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
                
                    
            
        }

        private void SelectedIndexChanged(object sender, EventHandler e)
        {

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

        private List<int> GetSensorIDs(List<int> S_IDs)
        {
            string IdCheckCmd = "SELECT ID FROM SensorDataDB.dbo.SENSOR_INFO WHERE Usage='YES'";
            using (SqlConnection conn = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD}; Min Pool Size=20")) // ; Integrated Security=True
            {
                Console.WriteLine("Usable sensor IDs:");
                SqlCommand sqlCommand = new SqlCommand(IdCheckCmd, conn);
                conn.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        S_IDs.Add(Convert.ToInt32(sqlDataReader["ID"]));
                        Console.WriteLine(Convert.ToInt32(sqlDataReader["ID"]));
                    }
                }
                conn.Close();
            }
            return S_IDs;
        }
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




        private void button1_Click(object sender, EventArgs e)
        {
            if (!modbusClient.Connected && myConnection.State == ConnectionState.Closed)
            {
                modbusClient.Connect();
                myConnection.Open();
            }
            timer1.Enabled = true;
            timer1.Start();
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

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            myConnection.Close();
            modbusClient.Disconnect();
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView1.SelectedItems)
            {
                for(int i=0;i<textBoxes.Count; i++)
                {
                    textBoxes[i].Text = item.SubItems[i].Text;
                    textBoxes[i].TextAlign = HorizontalAlignment.Center;
                }
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 1)
            {
                Console.WriteLine("ID:" + listView1.SelectedItems[0].Text);
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    for (int i = 0; i < textBoxes.Count; i++)
                    {
                        item.SubItems[i].Text = textBoxes[i].Text;
                        //textBoxes[i].TextAlign = HorizontalAlignment.Center;
                        //Console.WriteLine(listView1.SelectedIndices[i]);

                    }
                }

                UpdateDB(new List<TextBox>(textBoxes));
            }
            else
            {
                MessageBox.Show("New sensor info has been successfully saved", "Status");
            }
        }

        private void UpdateDB(List<TextBox> textBoxes)
        {
            
            using(SqlConnection con = new SqlConnection($@"Data Source={dbServer};Initial Catalog={dbName};User id={dbUID};Password={dbPWD};Min Pool Size=20"))
            {
                string sqlStr = $"UPDATE {dbName}.dbo.SENSOR_INFO " +
                                    $"SET Name = '{textBoxes[1].Text}', " +
                                        $"Location = '{textBoxes[2].Text}', " +
                                        $"Description = '{textBoxes[3].Text}', " +
                                        $"Usage = '{textBoxes[4].Text}' " +
                                    $"WHERE ID = {textBoxes[0].Text}; ";
                using (SqlCommand sqlCommand = con.CreateCommand())
                {
                    sqlCommand.CommandText = sqlStr;
                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("DB Update Successful.", "Status");
                    con.Close();
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            //Do whatever you need to with item
            item.Selected = false;
            for (int i=0; i<textBoxes.Count; i++)
            {
                if (i == 0)
                {
                    textBoxes[i].Text = (listView1.Items.Count + 1).ToString();
                }
                else
                {
                    textBoxes[i].Text = "";
                }
            }
            
            ListViewItem listViewItem = new ListViewItem(textBoxes[0].Text);

            listViewItem.SubItems.Add(textBoxes[1].Text);
            listViewItem.SubItems.Add(textBoxes[2].Text);
            listViewItem.SubItems.Add(textBoxes[3].Text);
            listViewItem.SubItems.Add(textBoxes[4].Text);
            listView1.Items.Add(listViewItem);

        }
    }
}
