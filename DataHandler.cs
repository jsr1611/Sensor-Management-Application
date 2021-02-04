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
        private string _SensorInfo;
        public string S_SensorInfo { 
            get { return _SensorInfo; }
            set { _SensorInfo = value; }
        }
        private List<string> _sensorInfoColmn;
        public List<string> S_SensorInfoColmn
        {
            get { return _sensorInfoColmn; }
            set { _sensorInfoColmn = value; }
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

        public void UpdateSensorInfo(SqlConnection myConn, string tableName, decimal sensorId, List<TextBox> txtB_SensorInfo, bool sUsage)
        {

            string sensorUsage = sUsage ? "YES" : "NO";
            string sqlStr = $"UPDATE {dbName}.dbo.{S_SensorInfo} " +
                                    $"SET {S_SensorInfoColmn[1]} = '{txtB_SensorInfo[0].Text}', " +
                                        $"{S_SensorInfoColmn[2]} = '{txtB_SensorInfo[1].Text}', " +
                                        $"{S_SensorInfoColmn[3]} = '{txtB_SensorInfo[2].Text}', " +
                                        $"{S_SensorInfoColmn[4]} = '{sensorUsage}' " +
                                    $"WHERE {S_SensorInfoColmn[0]} = {sensorId}; ";
            SqlCommand sqlCommand = new SqlCommand(sqlStr, myConn);
            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }

                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("DB Update Successful.", "Status info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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





        public void UpdateLimitRangeInfo(decimal sID, CheckBox targetCheckBox, List<NumericUpDown> rangeInfoList, SqlConnection myConn)
        {
            int sensorId = Convert.ToInt32(sID);
            string tableName = targetCheckBox.Name;
            List<decimal> rangeLimitData = rangeInfoList.AsEnumerable().Select(r => r.Value).ToList();
            
           
            string sqlUpdStr = $"UPDATE {tableName} SET {S_FourRangeColmn[0]} = {rangeLimitData[0]}, {S_FourRangeColmn[1]} = {rangeLimitData[1]}, {S_FourRangeColmn[2]} = {rangeLimitData[2]}, {S_FourRangeColmn[3]} = {rangeLimitData[3]};";
            SqlCommand updCmd = new SqlCommand(sqlUpdStr, myConn);

            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                updCmd.ExecuteNonQuery();
                //MessageBox.Show("Data Successfully Updated", "Status Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
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
        }

        private bool AddInfoToDB(decimal sID, CheckBox targetCheckBox, List<NumericUpDown> rangeInfoList, SqlConnection myConn)
        {
            bool target = false;

            return target;
        }
    }
}
