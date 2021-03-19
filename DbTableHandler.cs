using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminPage
{
    /// <summary>
    /// DB에서 틱정 
    /// </summary>
    public class DbTableHandler
    {
        public List<CheckBox> UsageCheckers { get; set; }
        public string DbServer { get => dbServer; set => dbServer = value; }
        public string DbName { get => dbName; set => dbName = value; }
        public string DbUID { get => dbUID; set => dbUID = value; }
        public string DbPWD { get; set; }

        public DbTableHandler(string dbPWD)
        {
            DbPWD = dbPWD;
        }

        public string S_DeviceTable { get; set; }
        public List<string> S_DeviceInfoColumns { get; set; }
        public List<string> S_FourRangeColumns { get; set; }

        public SqlConnection MyConn { get; set; }

        private string dbServer;
        private string dbName;
        private string dbUID;

        public List<string> ConnStr { get; set; }

        public string sqlConString { get; internal set; }

        public DbTableHandler()
        {
        }
        public DbTableHandler(List<CheckBox> _usageCheckers)
        {
            _usageCheckers = UsageCheckers;
        }
        public DbTableHandler(List<string> _connStr)
        {
            ConnStr = _connStr;
            DbServer = ConnStr[0];
            DbName = ConnStr[1];
            DbUID = ConnStr[2];
            DbPWD = ConnStr[3];

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
            SqlConnection myConn = new SqlConnection($@"Data Source ={ ConnStr[0] }; Initial Catalog = { ConnStr[1] }; User id = { ConnStr[2] }; Password ={ ConnStr[3]}; Min Pool Size = 20");

            /*          tbCreateCmdStr = $"Create TABLE {tableName} ( "+
                            " sID INT NOT NULL, "+
                            " LowerLimit1 float NOT NULL, " +
                            " LowerLimit2 float NOT NULL, " +
                            " HigherLimit1 float NOT NULL, " +
                            " HigherLimit2 float NOT NULL, " + 
                            " sUsage nvarchar(10) NOT NULL );";

*/
            // Create Table command 
            #region
            string tbCreateCmdStr = "";
            tbCreateCmdStr = $"Create TABLE {tableName} ( " +
                        $" {S_DeviceInfoColumns[0]} INT NOT NULL, " +
                        $" CONSTRAINT PK_{tableName}_{S_DeviceInfoColumns[0]} PRIMARY KEY ({S_DeviceInfoColumns[0]}), " +
                        $" {S_FourRangeColumns[0]} decimal(7,2) NULL, " +
                        $" {S_FourRangeColumns[1]} decimal(7,2) NULL, " +
                        $" {S_FourRangeColumns[2]} decimal(7,2) NULL, " +
                        $" {S_FourRangeColumns[3]} decimal(7,2) NULL);";

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
                    reader.Close();
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
                    myConn.Dispose();
                    tbCheckCmd.Dispose();
                    tbCreateCmd.Dispose();
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
            if (myConn == null || myConn.ConnectionString == String.Empty)
            {
                myConn = new SqlConnection($@"Data Source={DbServer};Initial Catalog={DbName};User id={DbUID};Password={DbPWD}; Min Pool Size=20"); // ; Integrated Security=True ");
            }

            SqlCommand createTbCmd = new SqlCommand(sqlCreateTable, myConn);
            try
            {
                myConn.Open();
                createTbCmd.ExecuteNonQuery();
                /*bool tbCreated = IfTableExists(tableName);
                if (tbCreated)
                {
                    res = true;
                }*/
                res = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                res = false;
            }
            finally
            {
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                    myConn.Dispose();
                    createTbCmd.Dispose();
                }
            }


            return res;
        }


        /// <summary>
        /// DB가 존재하는지 체크하고 존재하면 true를 반환함.
        /// </summary>
        /// <param name="sql_dbExists"></param>
        /// <param name="myConn_master"></param>
        /// <returns></returns>
        public bool IfDatabaseExists(string dbName, SqlConnection myConn_master)
        {
            bool result = false;
            string sql_dbExists = $"IF DB_ID('{dbName}') IS NOT NULL SELECT 1";

            if (myConn_master == null || myConn_master.ConnectionString == String.Empty)
            {
                myConn_master = new SqlConnection($@"Data Source = {DbServer};Initial Catalog=master;Trusted_Connection=True");
            }

            SqlCommand dbExistsCmd = new SqlCommand(sql_dbExists, myConn_master);
            DataSet ds = new DataSet();
            try
            {

                if (myConn_master.State != ConnectionState.Open)
                {
                    myConn_master.Open();
                }
                SqlDataAdapter sqlData = new SqlDataAdapter(sql_dbExists, myConn_master);

                sqlData.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    result = true;
                }
                /*using (SqlDataReader reader = dbExistsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res = Convert.ToInt32(reader.GetValue(0)) == 1;
                    }
                }*/
                sqlData.Dispose();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myConn_master.State == System.Data.ConnectionState.Open)
                {
                    myConn_master.Close();
                    myConn_master.Dispose();
                }
                dbExistsCmd.Dispose();
            }
            return result;
        }

        public bool IfDatabaseExists(string dbName)
        {
            SqlConnection myConn_master = new SqlConnection($@"Data Source = {DbServer};Initial Catalog=master;Trusted_Connection=True"); 
                                                        //($@"Data Source = {DbServer};Initial Catalog=master;User id={DbUID};Password={DbPWD};Min Pool Size=20");
            bool result = false;
            string sql_dbExists = $"IF DB_ID('{dbName}') IS NOT NULL SELECT 1";
            SqlCommand dbExistsCmd = new SqlCommand(sql_dbExists, myConn_master);
            DataSet ds = new DataSet();
            try
            {

                if (myConn_master.State != ConnectionState.Open)
                {
                    myConn_master.Open();
                }
                SqlDataAdapter sqlData = new SqlDataAdapter(sql_dbExists, myConn_master);

                sqlData.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    result = true;
                }
                /*using (SqlDataReader reader = dbExistsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res = Convert.ToInt32(reader.GetValue(0)) == 1;
                    }
                }*/

                sqlData.Dispose();
                dbExistsCmd.Dispose();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myConn_master.State == System.Data.ConnectionState.Open)
                {
                    myConn_master.Close();
                    myConn_master.Dispose();
                }
                
            }
            return result;
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
            SqlConnection myConn_master = new SqlConnection($@"Data Source = {DbServer};Initial Catalog=master;Trusted_Connection=True");
            //dbName = connStr[1];
            bool dbExists = IfDatabaseExists(dbName, myConn_master);
            if (dbExists)
            {
                SqlCommand createTbCmd = new SqlCommand(sqlCreateTable, myConn);
                try
                {
                    bool tbAlreadyExists = IfTableExists(tableName);
                    if (tbAlreadyExists)
                    {
                        target = true;
                    }
                    else
                    {
                        if (myConn.State != ConnectionState.Open)
                        {
                            myConn.Open();
                        }
                        createTbCmd.ExecuteNonQuery();
                        target = true;
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    if (myConn.State == System.Data.ConnectionState.Open)
                    {
                        myConn.Close();
                        myConn.Dispose();
                    }
                    createTbCmd.Dispose();
                }

            }
            else
            {
                DialogResult createDbCheck = MessageBox.Show($"DB를 찾을 수 없습니다.\nDB명은 {dbName}", "Status Info", MessageBoxButtons.OK);
            }

            return target;
        }




        public bool CreateDatabase(string dbName, string sqlStr_CreateDb)
        {
            SqlConnection myConn_master = new SqlConnection($@"Data Source = {DbServer};Initial Catalog=master;Trusted_Connection=True");
            bool res = false;
            SqlCommand dbCreateCmd = new SqlCommand(sqlStr_CreateDb, myConn_master);
            try
            {
                if (myConn_master.State != ConnectionState.Open)
                {
                    myConn_master.Open();
                }

                dbCreateCmd.ExecuteNonQuery();

                res = IfDatabaseExists(dbName, myConn_master);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (myConn_master.State == System.Data.ConnectionState.Open)
                {
                    myConn_master.Close();
                    myConn_master.Dispose();
                }
                dbCreateCmd.Dispose();
            }

            return res;

        }



        public bool IfTableExists(string tableName)
        {
            //string checkBoxName = targetCheckBoxName;
            bool target = false;
            string dbCheckCmdStr;
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = sqlConString;
            dbCheckCmdStr = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}';";
            SqlCommand tbCheckCmd = new SqlCommand(dbCheckCmdStr, myConn);

            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }

                using (SqlDataReader reader = tbCheckCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(Convert.ToInt32(reader.GetValue(0)) == 1)
                        {
                            target = true;
                            break;
                        }
                    }
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
                    myConn.Dispose();
                }
            }
            return target;
        }




        public bool UpdateDeviceInfoTable(SqlConnection myConn, string tableName, int deviceId, int deviceIdNew, List<TextBox> txtB_DeviceInfo, bool sUsage)
        {
            bool updSuccessful = false;
            myConn = new SqlConnection();
            myConn.ConnectionString = sqlConString;
            string sensorUsage = sUsage ? "YES" : "NO";
            string sqlUpdStr;
            string sqlCheckStr;
            if (deviceId == deviceIdNew)
            {
                sqlUpdStr = $"UPDATE {S_DeviceTable} " +
                                        $"SET {S_DeviceInfoColumns[1]} = '{txtB_DeviceInfo[0].Text}' " +
                                            $", {S_DeviceInfoColumns[2]} = '{txtB_DeviceInfo[1].Text}' " +
                                            $", {S_DeviceInfoColumns[3]} = '{txtB_DeviceInfo[2].Text}' " +
                                            $", {S_DeviceInfoColumns[4]} = '{txtB_DeviceInfo[3].Text}' " +
                                            $", {S_DeviceInfoColumns[S_DeviceInfoColumns.Count-1]} = '{sensorUsage}' " +
                                        $" WHERE {S_DeviceInfoColumns[0]} = {deviceId}; ";
                sqlCheckStr = $"SELECT 1 FROM {S_DeviceTable} " +
                            $" WHERE {S_DeviceInfoColumns[0]} = {deviceId} " +
                            $"and {S_DeviceInfoColumns[1]} = '{txtB_DeviceInfo[0].Text}' " +
                            $" and {S_DeviceInfoColumns[2]} = '{txtB_DeviceInfo[1].Text}' " +
                           $" and {S_DeviceInfoColumns[3]} = '{txtB_DeviceInfo[2].Text}' " +
                           $" and {S_DeviceInfoColumns[4]} = '{txtB_DeviceInfo[3].Text}' " +
                           $" and {S_DeviceInfoColumns[S_DeviceInfoColumns.Count-1]} = '{sensorUsage}';";
            }
            else
            {
                sqlUpdStr = $"UPDATE {S_DeviceTable} " +
                                        $"SET {S_DeviceInfoColumns[0]} = {deviceIdNew}" +
                                        $", {S_DeviceInfoColumns[1]} = '{txtB_DeviceInfo[0].Text}'" +
                                            $", {S_DeviceInfoColumns[2]} = '{txtB_DeviceInfo[1].Text}'" +
                                            $", {S_DeviceInfoColumns[3]} = '{txtB_DeviceInfo[2].Text}'" +
                                            $", {S_DeviceInfoColumns[4]} = '{txtB_DeviceInfo[3].Text}'" +
                                            $", {S_DeviceInfoColumns[S_DeviceInfoColumns.Count-1]} = '{sensorUsage}' " +
                                        $"WHERE {S_DeviceInfoColumns[0]} = {deviceId}; ";

                sqlCheckStr = $"SELECT 1 FROM {S_DeviceTable} " +
                            $" WHERE {S_DeviceInfoColumns[0]} = {deviceIdNew} ";
                for(int i=0; i<txtB_DeviceInfo.Count; i++)
                {
                    sqlCheckStr += $"and {S_DeviceInfoColumns[i+1]} = '{txtB_DeviceInfo[i].Text}' ";
                }
                sqlCheckStr += $" and {S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} = '{sensorUsage}';";
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
                        if (Convert.ToInt32(reader.GetValue(0)) == 1)
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
                    myConn.Dispose();
                }
            }
            return updSuccessful;
        }




        public bool UpdateLimitRangeInfo(int deviceId, int deviceIdNew, CheckBox targetCheckBox, List<NumericUpDown> rangeInfoList, SqlConnection myConn)
        {
            myConn = new SqlConnection();
            myConn.ConnectionString = sqlConString;
            bool updSuccessful = false;
            //deviceId = Convert.ToInt32(sID);
            string tableName = targetCheckBox.Name;
            List<decimal> rangeLimitData = rangeInfoList.AsEnumerable().Select(r => r.Value).ToList();
            string idCheckStr = $"SELECT 1 FROM {tableName} WHERE { S_DeviceInfoColumns[0]} = { deviceId};";
            bool idExists = false;
            string sqlInsert;
            string sqlUpdStr;
            string sqlUpdCheckStr;
            if (deviceId == deviceIdNew)
            {
                sqlInsert = $"INSERT INTO {tableName} VALUES({deviceId} ";
                sqlUpdStr = $"UPDATE {tableName} SET ";
                sqlUpdCheckStr = $"SELECT 1 FROM {tableName} WHERE {S_DeviceInfoColumns[0]} = {deviceId} ";
                for (int i = 0; i < rangeLimitData.Count; i++)
                {
                    sqlInsert += $", {rangeLimitData[i]}";
                    sqlUpdStr += $" { S_FourRangeColumns[i]} = { rangeLimitData[i]} ";
                    sqlUpdCheckStr += $" and { S_FourRangeColumns[i]} = { rangeLimitData[i]} ";

                    if (i + 1 != rangeLimitData.Count)
                    {
                        sqlUpdStr += ",";
                    }
                }

                sqlInsert += ");";
                sqlUpdStr += $" WHERE { S_DeviceInfoColumns[0]} = { deviceId}; ";
                sqlUpdCheckStr += ";";

            }
            else
            {

                sqlInsert = $"INSERT INTO {tableName} VALUES({deviceId} ";
                sqlUpdStr = $"UPDATE {tableName} SET {S_DeviceInfoColumns[0]} = {deviceIdNew} ";
                sqlUpdCheckStr = $"SELECT 1 FROM {tableName} WHERE {S_DeviceInfoColumns[0]} = {deviceIdNew} ";
                for (int i = 0; i < rangeLimitData.Count; i++)
                {
                    sqlInsert += $", { rangeLimitData[i]}";
                    sqlUpdStr += $", { S_FourRangeColumns[i]} = { rangeLimitData[i]} ";
                    sqlUpdCheckStr += $" and { S_FourRangeColumns[i]} = { rangeLimitData[i]} ";
                }
                sqlInsert += ");";
                sqlUpdStr += $" WHERE { S_DeviceInfoColumns[0]} = { deviceId}; "; // WHERE {S_DeviceColmn[0]} = {deviceId};";
                sqlUpdCheckStr += ";";

            }
            SqlCommand idExistsCmd = new SqlCommand(idCheckStr, myConn);
            SqlCommand updCmd = new SqlCommand(sqlUpdStr, myConn);
            SqlCommand updCheckCmd = new SqlCommand(sqlUpdCheckStr, myConn);
            SqlCommand insertCmd = new SqlCommand(sqlInsert, myConn);
            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }

                using (SqlDataReader reader = idExistsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader.GetValue(0)) == 1)
                        {
                            idExists = true;
                            break;
                        }
                    }
                }

                if (!idExists)
                {
                    insertCmd.ExecuteNonQuery();
                }
                else
                {
                    updCmd.ExecuteNonQuery();
                }
                using (SqlDataReader reader = updCheckCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader.GetValue(0)) == 1)
                        {
                            updSuccessful = true;
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
                    myConn.Dispose();
                }
            }
            return updSuccessful;
        }

        private bool AddInfoToDB(decimal sID, CheckBox targetCheckBox, List<NumericUpDown> rangeInfoList, SqlConnection myConn)
        {
            bool target = false;

            return target;
        }





        public bool UpdateUsageTable(SqlConnection myConn, string UsageTable, List<string> usageTableColmn, int deviceIdOld, int deviceIdNew, List<string> usageInfo)
        {
            //Check if ID exists
            myConn = new SqlConnection();
            myConn.ConnectionString = sqlConString;
            string sqlCheckNoData = "";
            string sqlInsert = "";
            bool dataExists = false;

            bool updSuccessful = false;
            string sqlUpd = "";
            string sqlUpdCheck = "";

            if (deviceIdNew == deviceIdOld)
            {
                sqlUpd = $"UPDATE {UsageTable} SET ";
                sqlUpdCheck = $"SELECT 1 FROM {UsageTable} WHERE ";
                //for corner case of NonExisting ID => Insert data!
                sqlInsert = $"INSERT INTO {UsageTable} VALUES({deviceIdOld} ";
                for (int i = 0; i < usageInfo.Count; i++)
                {
                    sqlUpd += $" {usageTableColmn[i]} = '{usageInfo[i]}' ";
                    sqlUpdCheck += $" {usageTableColmn[i]} = '{usageInfo[i]}' and ";
                    sqlInsert += $", '{usageInfo[i]}' ";
                    if (i + 1 != usageInfo.Count)
                    {
                        sqlUpd += ",";
                    }
                }

                sqlUpd += $" WHERE {S_DeviceInfoColumns[0]} = {deviceIdOld};";
                sqlUpdCheck += $"{S_DeviceInfoColumns[0]} = {deviceIdOld}";

                //corner case if data doesn't exist=>insert it!
                sqlCheckNoData = $"SELECT COUNT(*) FROM {UsageTable} WHERE {S_DeviceInfoColumns[0]} = {deviceIdOld};";
                sqlInsert += ");";

            }
            else
            {
                sqlUpd = $"UPDATE {UsageTable} SET {S_DeviceInfoColumns[0]} = {deviceIdNew}";
                sqlUpdCheck = $"SELECT 1 FROM {UsageTable} WHERE {S_DeviceInfoColumns[0]} = {deviceIdNew}";
                //for corner case of NonExisting ID => Insert data!
                sqlInsert = $"INSERT INTO {UsageTable} VALUES({deviceIdNew} ";
                for (int i = 0; i < usageInfo.Count; i++)
                {
                    sqlUpd += $", {usageTableColmn[i]} = '{usageInfo[i]}' ";
                    sqlUpdCheck += $" and {usageTableColmn[i]} = '{usageInfo[i]}'";
                    sqlInsert += $", '{usageInfo[i]}' ";
                }

                sqlUpd += $" WHERE {S_DeviceInfoColumns[0]} = {deviceIdOld};";
                sqlUpdCheck += ";";
                //corner case if data doesn't exist=>insert it!
                sqlCheckNoData = $"SELECT COUNT(*) FROM {UsageTable} WHERE {S_DeviceInfoColumns[0]} = {deviceIdOld};";
                sqlInsert += ");";
            }
            SqlCommand CheckIdExistsCmd = new SqlCommand(sqlCheckNoData, myConn);
            SqlCommand InsertCmd = new SqlCommand(sqlInsert, myConn);
            SqlCommand updCmd = new SqlCommand(sqlUpd, myConn);
            SqlCommand checkUpdCmd = new SqlCommand(sqlUpdCheck, myConn);
            try
            {
                if (myConn.State != ConnectionState.Open)
                {
                    myConn.Open();
                }
                using (SqlDataReader reader = CheckIdExistsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader.GetValue(0)) == 1)
                        {
                            dataExists = true;
                            break;
                        }
                    }
                }
                if (!dataExists)
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }

                    InsertCmd.ExecuteNonQuery();
                }
                else
                {
                    updCmd.ExecuteNonQuery();
                }

                using (SqlDataReader reader = checkUpdCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader.GetValue(0)) == 1)
                        {
                            updSuccessful = true;
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
                    myConn.Dispose();
                }
            }
            return updSuccessful;
        }















        /*
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
                        if(myConn.State != ConnectionState.Open)
                        {
                            myConn.Open();
                        }
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


        */



    }
}
