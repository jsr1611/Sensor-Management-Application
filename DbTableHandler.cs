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
    /// <summary>
    /// DB에서 틱정 
    /// </summary>
    public class DbTableHandler
    {
        public List<CheckBox> usageCheckers { get; set; }


        /// <summary>
        /// (0) dbServer, (1) dbName, (2) dbUID, (3) dbUID
        /// </summary>
        public List<string> connStr { get; set; }
        
        public DbTableHandler()
        {
            
        }
        public DbTableHandler(List<CheckBox> _usageCheckers)
        {
            _usageCheckers = usageCheckers;
        }
        public DbTableHandler(List<string> _connStr)
        {
            _connStr = connStr;
        }



        /// <summary>
        /// Return table names if they exist, else create tables for the given names and return the names
        /// 데이터를 저장할 Table이 존재하는지 체크함, 존재하지 않을 경우 생성하고 테이블명을 반환함.
        /// </summary>
        /// <param name="usageCheckers"></param>
        /// <returns></returns>
        public List<CheckBox> CheckTablesExistHandler(List<CheckBox> usageCheckers)
        {
            List<CheckBox> target = new List<CheckBox>();
            CheckBox chbName;
            string tbName;
            for (int i = 0; i < usageCheckers.Count; i++)
            {
                tbName = usageCheckers[i].Name;
                chbName = usageCheckers[i];
                if (IfTableExists(tbName) == true)
                {
                    target.Add(chbName);
                }
                else
                {
                    if (CreateTb(tbName) == true)
                    {
                        target.Add(chbName);
                    }
                }
            }
            
            return target;
        }
        
        public List<string> CheckTablesExistHandler(List<string> usageCheckerNames)
        {
            List<string> target = new List<string>();
            string tbName;
            for (int i = 0; i < usageCheckerNames.Count; i++)
            {
                tbName = usageCheckerNames[i];
                if (IfTableExists(tbName) == true)
                {
                    target.Add(tbName);
                }
                else
                {
                    if (CreateTb(tbName) == true)
                    {
                        target.Add(tbName);
                    }
                }
            }
            return target;
        }

        public string CheckTablesExistHandler(string usageCheckerName)
        {
            string tbName;
            tbName = usageCheckerName;
            if (IfTableExists(tbName) == true)
            {
                return tbName;
            }
            else
            {
                if (CreateTb(tbName) == true)
                {
                    return tbName;
                }
                else
                {
                    throw new Exception("DB 테이블 생성 시 에러가 발생했습니다");
                }
            }
        }

        
        private bool CreateTb(string tableName)
        {
            //string tbName = targetCheckBox.Name;
            bool tbCreated = false;
            SqlConnection myConn = new SqlConnection($@"Data Source ={ connStr[0] }; Initial Catalog = { connStr[1] }; User id = { connStr[2] }; Password ={ connStr[3]}; Min Pool Size = 20");
            // Create Table command 
            #region
            string tbCreateCmdStr;
            tbCreateCmdStr = $"Create TABLE [{connStr[1]}].[dbo].[{tableName}] ( "+
                            " sID INT NOT NULL, "+
                            " LowerLimit1 float NOT NULL, " +
                            " LowerLimit2 float NOT NULL, " +
                            " HigherLimit1 float NOT NULL, " +
                            " HigherLimit2 float NOT NULL, " + 
                            " sUsage nvarchar(10) NOT NULL );";
            SqlCommand tbCreateCmd = new SqlCommand(tbCreateCmdStr, myConn);
            #endregion
            
            // Check if table created correctly 
            #region 
            string tbCheckCmdStr;
            tbCheckCmdStr = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}';";
            SqlCommand tbCheckCmd = new SqlCommand(tbCheckCmdStr, myConn);
            #endregion

            try
            {
                myConn.Open();
                tbCreateCmd.ExecuteNonQuery();

                using (SqlDataReader reader = tbCheckCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tbCreated = Convert.ToInt32(reader.GetValue(0)) == 1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if(myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return tbCreated;
        }





        /// <summary>
        /// Table 생성해주는 함수.
        /// </summary>
        /// <param name="tableName">Table명</param>
        /// <param name="sqlCreateTable">Table생성을 위한 Sql쿼리 </param>
        /// <param name="myConn">SqlConnection명</param>
        /// <returns></returns>
        private bool CreateTb(string tableName, string sqlCreateTable, SqlConnection myConn)
        {
            bool res = false;

            SqlCommand createTbCmd = new SqlCommand(sqlCreateTable, myConn);
            try
            {
                myConn.Open();
                createTbCmd.ExecuteNonQuery();
                bool tbCreated = IfTableExists(tableName);
                if(tbCreated)
                {
                    res = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK);
            }
            finally
            {
                if(myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }


            return res;
        }


        /// <summary>
        /// DB가 존재하는지 체크하고 존재하면 true를 반환함.
        /// </summary>
        /// <param name="sql_dbExists"></param>
        /// <param name="myConn"></param>
        /// <returns></returns>
        public bool IfDatabaseExists(string dbName, SqlConnection myConn)
        {
            bool res = false;
            string sql_dbExists = $"IF DB_ID('{dbName}') IS NOT NULL SELECT 1";
            SqlCommand dbExistsCmd = new SqlCommand(sql_dbExists, myConn);
            DataSet ds = new DataSet();
            try
            {
                myConn.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter(sql_dbExists, myConn);

                sqlData.Fill(ds);
                if(ds.Tables.Count > 0)
                {
                    res = true;
                }
                /*using (SqlDataReader reader = dbExistsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res = Convert.ToInt32(reader.GetValue(0)) == 1;
                    }
                }*/
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK);
            }
            finally
            {
                if(myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return res;
        }




        /// <summary>
        /// 지정한 DB에서 테이블 생성해주는 함수.
        /// </summary>
        /// <param name="dbName">DB명</param>
        /// <param name="tableName">생성할 테이블명</param>
        /// <param name="sqlCreateTable">테이블 생성을 위한 SQL쿼리 </param>
        /// <param name="myConn">SqlConnection명</param>
        /// <returns></returns>
        public bool CreateTable(string dbName, string tableName, string sqlCreateTable, SqlConnection myConn)
        {
            bool target = false;

            //dbName = connStr[1];
            bool dbExists = IfDatabaseExists(dbName, myConn);
            if (dbExists)
            {
                SqlCommand createTbCmd = new SqlCommand(sqlCreateTable, myConn);
                try
                {
                    myConn.Open();
                    createTbCmd.ExecuteNonQuery();
                    bool tbAlreadyExists = IfTableExists(tableName);
                    if (tbAlreadyExists)
                    {
                        target = true;
                    }
                    else
                    {
                        bool tbCreated = CreateTb(tableName, sqlCreateTable, myConn);
                        if (tbCreated)
                        {
                            target = true;
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK);
                }
                finally
                {
                    if(myConn.State == System.Data.ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
                
            }
            else
            {
                DialogResult createDbCheck = MessageBox.Show($"DB를 찾을 수 없습니다.\nDB명은 {dbName}", "Status Info", MessageBoxButtons.OK);
            }

            return target;
        }




        public bool CreateDatabase(SqlConnection myConn, string dbName, string sqlStr_CreateDb)
        {
            bool res = false;
            SqlCommand dbCreateCmd = new SqlCommand(sqlStr_CreateDb, myConn);
            try
            {
                myConn.Open();
                dbCreateCmd.ExecuteNonQuery();

                res = IfDatabaseExists(dbName, myConn);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러 매지시", MessageBoxButtons.OK);
            }
            finally
            {
                if(myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

            return res;

        }



        public bool IfTableExists(string tableName)
        {
            //string checkBoxName = targetCheckBoxName;
            bool target = false;
            string dbCheckCmdStr;
            SqlConnection myConn = new SqlConnection($@"Data Source ={ connStr[0] }; Initial Catalog = { connStr[1] }; User id = { connStr[2] }; Password ={ connStr[3]}; Min Pool Size = 20");
            dbCheckCmdStr = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}';";
            SqlCommand tbCheckCmd = new SqlCommand(dbCheckCmdStr, myConn);

            try
            {
                myConn.Open();
                using(SqlDataReader reader = tbCheckCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        target = Convert.ToInt32(reader.GetValue(0)) == 1;
                    }
                }
            }
            catch(System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if(myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return target;
        }


        



        private bool CreateTb(CheckBox targetCheckBox)
        {
            string tbName = targetCheckBox.Name;
            bool tbCreated = false;
            SqlConnection myConn = new SqlConnection($@"Data Source ={ connStr[0] }; Initial Catalog = { connStr[1] }; User id = { connStr[2] }; Password ={ connStr[3]}; Min Pool Size = 20");
            // Create Table command 
            #region
            string tbCreateCmdStr;
            tbCreateCmdStr = $"Create TABLE [{connStr[1]}].[dbo].[{tbName}] ( " +
                            " sID INT IDENTITY NOT NULL, " +
                            " LowerLimit1 float NOT NULL, " +
                            " LowerLimit2 float NOT NULL, " +
                            " HigherLimit1 float NOT NULL, " +
                            " HigherLimit2 float NOT NULL, " +
                            " sUsage nvarchar(10) NOT NULL );";
            SqlCommand tbCreateCmd = new SqlCommand(tbCreateCmdStr, myConn);
            #endregion

            // Check if table created correctly 
            #region 
            string tbCheckCmdStr;
            tbCheckCmdStr = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tbName}';";
            SqlCommand tbCheckCmd = new SqlCommand(tbCheckCmdStr, myConn);
            #endregion

            try
            {
                myConn.Open();
                tbCreateCmd.ExecuteNonQuery();

                using (SqlDataReader reader = tbCheckCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tbCreated = Convert.ToInt32(reader.GetValue(0)) == 1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return tbCreated;
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
                myConn.Open();
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
