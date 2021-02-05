using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataCollectionApp2
{
    public class DataHandler : DbTableHandler
    {
        private string _DeviceInfoTable;
        public string S_DeviceTable { 
            get { return _DeviceInfoTable; }
            set { _DeviceInfoTable = value; }
        }
        private List<string> _DeviceColmn;
        public List<string> S_DeviceColmn
        {
            get { return _DeviceColmn; }
            set { _DeviceColmn = value; }
        }



        private List<string> _fourRangeColmn;
        public List<string> S_FourRangeColmn
        {
            get { return _fourRangeColmn; }
            set { _fourRangeColmn = value; }
        }

        public DataHandler()
        {

        }

        public bool UpdateDeviceInfoTable(SqlConnection myConn, string tableName, int deviceId, int deviceIdNew, List<TextBox> txtB_DeviceInfo, bool sUsage)
        {
            bool updSuccessful = false;
            string sensorUsage = sUsage ? "YES" : "NO";
            string sqlUpdStr;
            string sqlCheckStr;
            if (deviceId == deviceIdNew)
            {
                sqlUpdStr = $"UPDATE {S_DeviceTable} " +
                                        $"SET {S_DeviceColmn[1]} = '{txtB_DeviceInfo[0].Text}' " +
                                            $", {S_DeviceColmn[2]} = '{txtB_DeviceInfo[1].Text}' " +
                                            $", {S_DeviceColmn[3]} = '{txtB_DeviceInfo[2].Text}' " +
                                            $", {S_DeviceColmn[4]} = '{sensorUsage}' " +
                                        $" WHERE {S_DeviceColmn[0]} = {deviceId}; ";
                sqlCheckStr = $"SELECT 1 FROM {S_DeviceTable} " +
                            $" WHERE {S_DeviceColmn[0]} = {deviceId} " +
                            $"and {S_DeviceColmn[1]} = '{txtB_DeviceInfo[0].Text}' " +
                            $" and {S_DeviceColmn[2]} = '{txtB_DeviceInfo[1].Text}' " +
                           $" and {S_DeviceColmn[3]} = '{txtB_DeviceInfo[2].Text}' " +
                           $" and {S_DeviceColmn[4]} = '{sensorUsage}';";
            }
            else
            {
                sqlUpdStr = $"UPDATE {S_DeviceTable} " +
                                        $"SET {S_DeviceColmn[0]} = {deviceIdNew}" +
                                        $", {S_DeviceColmn[1]} = '{txtB_DeviceInfo[0].Text}'" +
                                            $", {S_DeviceColmn[2]} = '{txtB_DeviceInfo[1].Text}'" +
                                            $", {S_DeviceColmn[3]} = '{txtB_DeviceInfo[2].Text}'" +
                                            $", {S_DeviceColmn[4]} = '{sensorUsage}' " +
                                        $"WHERE {S_DeviceColmn[0]} = {deviceId}; ";
                
                sqlCheckStr = $"SELECT 1 FROM {S_DeviceTable} " +
                            $" WHERE {S_DeviceColmn[0]} = {deviceIdNew} " +
                            $"and {S_DeviceColmn[1]} = '{txtB_DeviceInfo[0].Text}' " +
                            $" and {S_DeviceColmn[2]} = '{txtB_DeviceInfo[1].Text}' " +
                           $" and {S_DeviceColmn[3]} = '{txtB_DeviceInfo[2].Text}' " +
                           $" and {S_DeviceColmn[4]} = '{sensorUsage}';";
            }
                
            SqlCommand sqlUpdCmd = new SqlCommand(sqlUpdStr, myConn);
            SqlCommand sqlCheckCmd = new SqlCommand(sqlCheckStr, myConn);
            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }

                sqlUpdCmd.ExecuteNonQuery();
                using (SqlDataReader reader = sqlCheckCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(Convert.ToInt32(reader.GetValue(0)) == 1)
                        {
                            updSuccessful = true;
                            
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
            return updSuccessful;
        }





        public bool UpdateLimitRangeInfo(int deviceId, int deviceIdNew, CheckBox targetCheckBox, List<NumericUpDown> rangeInfoList, SqlConnection myConn)
        {
            bool updSuccessful = false;
            //deviceId = Convert.ToInt32(sID);
            string tableName = targetCheckBox.Name;
            List<decimal> rangeLimitData = rangeInfoList.AsEnumerable().Select(r => r.Value).ToList();
            string idCheckStr;
            string sqlUpdStr;
            string sqlUpdCheckStr;
            if (deviceId == deviceIdNew)
            {
                idCheckStr = $"SELECT 1 FROM {tableName} WHERE { S_DeviceColmn[0]} = { deviceId};";
                sqlUpdStr = $"UPDATE {tableName} SET {S_FourRangeColmn[0]} = {rangeLimitData[0]}, {S_FourRangeColmn[1]} = {rangeLimitData[1]}, {S_FourRangeColmn[2]} = {rangeLimitData[2]}, {S_FourRangeColmn[3]} = {rangeLimitData[3]}  WHERE {S_DeviceColmn[0]} = {deviceId};";
                sqlUpdCheckStr = $"SELECT 1 FROM {tableName} WHERE {S_DeviceColmn[0]} = {deviceId} and {S_FourRangeColmn[0]} = {rangeLimitData[0]} and {S_FourRangeColmn[1]} = {rangeLimitData[1]} and {S_FourRangeColmn[2]} = {rangeLimitData[2]} and {S_FourRangeColmn[3]} = {rangeLimitData[3]};";
                
            }
            else
            {
                idCheckStr = $"SELECT 1 FROM {tableName} WHERE { S_DeviceColmn[0]} = { deviceIdNew};";
                sqlUpdStr = $"UPDATE {tableName} SET {S_DeviceColmn[0]} = {deviceIdNew}, {S_FourRangeColmn[0]} = {rangeLimitData[0]}, {S_FourRangeColmn[1]} = {rangeLimitData[1]}, {S_FourRangeColmn[2]} = {rangeLimitData[2]}, {S_FourRangeColmn[3]} = {rangeLimitData[3]}  WHERE {S_DeviceColmn[0]} = {deviceId};";
                sqlUpdCheckStr = $"SELECT 1 FROM {tableName} WHERE {S_DeviceColmn[0]} = {deviceIdNew} and {S_FourRangeColmn[0]} = {rangeLimitData[0]} and {S_FourRangeColmn[1]} = {rangeLimitData[1]} and {S_FourRangeColmn[2]} = {rangeLimitData[2]} and {S_FourRangeColmn[3]} = {rangeLimitData[3]};";
            }

            SqlCommand idCheckCmd = new SqlCommand(idCheckStr, myConn);
            SqlCommand updCmd = new SqlCommand(sqlUpdStr, myConn);
            SqlCommand updCheckCmd = new SqlCommand(sqlUpdCheckStr, myConn);

            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                updCmd.ExecuteNonQuery();
                using (SqlDataReader reader = updCheckCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        updSuccessful = Convert.ToInt32(reader.GetValue(0)) == 1;
                        if (updSuccessful)
                        {
                            break;
                        }
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return updSuccessful;
        }

        private bool AddInfoToDB(decimal sID, CheckBox targetCheckBox, List<NumericUpDown> rangeInfoList, SqlConnection myConn)
        {
            bool target = false;

            return target;
        }

        public bool UpdateUsageTable(SqlConnection myConn, string UsageTable, int deviceIdOld, int deviceIdNew, List<string> usageInfo)
        {
            bool updSuccessful = false;
            string sqlUpd;
            if (deviceIdNew == deviceIdOld)
            {
                sqlUpd = $"UPDATE {UsageTable} VALUES({deviceIdOld} ";
                    foreach(var item in usageInfo)
                {
                    sqlUpd += $", {item}";
                }

                sqlUpd += ");";
            }
            SqlCommand updCmd = new SqlCommand();
            if(myConn.State != ConnectionState.Open)
            {
                myConn.Open();
            }
            updCmd.ExecuteNonQuery();
            

            return updSuccessful;
        }
    }
}
