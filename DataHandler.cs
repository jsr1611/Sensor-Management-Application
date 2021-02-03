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
        public DataHandler()
        {

        }

        public void UpdateLimitRangeInfo(decimal sID, CheckBox targetCheckBox, List<NumericUpDown> rangeInfoList, SqlConnection myConn)
        {
            int sensorId = Convert.ToInt32(sID);
            string tableName = targetCheckBox.Name;
            List<float> rangeLimitData = rangeInfoList.AsEnumerable().Select(r => Convert.ToSingle(r.Value)).ToList();
            string sensorUsage = rangeLimitData.AsEnumerable().Where(x => x != 0).ToList().Count > 0 ? "YES" : "NO";

            string sqlCheckStr = $"SELECT COUNT(*) FROM {tableName} WHERE sID = {sensorId}";
            SqlCommand checkCmd = new SqlCommand(sqlCheckStr, myConn);

            string sqlUpdStr = $"UPDATE {tableName} SET LowerLimit1 = {rangeLimitData[0]}, LowerLimit2 = {rangeLimitData[1]}, HigherLimit1 = {rangeLimitData[2]}, HigherLimit2 = {rangeLimitData[3]}, sUsage = {sensorUsage};";
            SqlCommand updCmd = new SqlCommand(sqlUpdStr, myConn);



            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }

                bool idExists = false;
                using (SqlDataReader reader = checkCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idExists = Convert.ToInt32(reader.GetValue(0)) == 1;
                    }
                }
                if (idExists)
                {
                    updCmd.ExecuteNonQuery();
                    MessageBox.Show("Data Successfully Updated", "Status Info", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("센서 정보 업데이트가 잘 이루어지지 않았습니다. ", "Status Info", MessageBoxButtons.OK);

                    /*
                    DialogResult SaveOrNot = MessageBox.Show("하한 및 상한 정보를 저음으로 업데이트하시는 것 같아요. 업데이트를 진행하시겠습니까?", "Status Info", MessageBoxButtons.YesNo);
                    if (SaveOrNot == DialogResult.Yes)
                    {

                    }*/
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
