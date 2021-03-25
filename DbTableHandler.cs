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

        public List<string> CheckTablesExist(List<string> usageCheckerNames)
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

        public string CheckTablesExist(string usageCheckerName)
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
            // Create Table command 

            string tbCreateCmdStr = "";
            tbCreateCmdStr = $"Create TABLE {tableName} ( " +
                        $" {S_DeviceInfoColumns[0]} INT NOT NULL, " +
                        $" CONSTRAINT PK_{tableName}_{S_DeviceInfoColumns[0]} PRIMARY KEY ({S_DeviceInfoColumns[0]}), " +
                        $" {S_FourRangeColumns[0]} decimal(7,2) NULL, " +
                        $" {S_FourRangeColumns[1]} decimal(7,2) NULL, " +
                        $" {S_FourRangeColumns[2]} decimal(7,2) NULL, " +
                        $" {S_FourRangeColumns[3]} decimal(7,2) NULL);";

            // Check if table created correctly 
            /*string tbCheckCmdStr;
            tbCheckCmdStr = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}';";
*/
            using (SqlConnection myConn = new SqlConnection(sqlConString))
            {
                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }

                    using (SqlCommand tbCreateCmd = new SqlCommand(tbCreateCmdStr, myConn))
                    {
                        tbCreateCmd.ExecuteNonQuery();
                        tbCreated = true;
                    }
                    /*using (SqlCommand tbCheckCmd = new SqlCommand(tbCheckCmdStr, myConn))
                    {
                        using (SqlDataReader reader = tbCheckCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tbCreated = Convert.ToInt32(reader.GetValue(0)) == 1;
                            }
                            reader.Close();
                        }
                    }*/
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private bool CreateTb(string tableName, string sqlCreateTable)
        {
            bool res = false;

            using (SqlConnection myConn = new SqlConnection($@"Data Source={DbServer};Initial Catalog={DbName};User id={DbUID};Password={DbPWD}; Min Pool Size=20"))
            {
                try
                {
                    if (myConn.State != ConnectionState.Open)
                    {
                        myConn.Open();
                    }
                    using (SqlCommand createTbCmd = new SqlCommand(sqlCreateTable, myConn))
                    {
                        createTbCmd.ExecuteNonQuery();
                        res = true;
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "에러 매시지", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    res = false;
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
        public bool IfDatabaseExists(string dbName)
        {
            bool result = false;
            string sql_dbExists = $"IF DB_ID('{dbName}') IS NOT NULL SELECT 1";

            using (SqlConnection myConn_master = new SqlConnection($@"Data Source = {DbServer};Initial Catalog=master;Trusted_Connection=True"))
            {
                //($@"Data Source = {DbServer};Initial Catalog=master;User id={DbUID};Password={DbPWD};Min Pool Size=20");
                if (myConn_master.State != ConnectionState.Open)
                {
                    myConn_master.Open();
                }
                using (SqlCommand dbExistsCmd = new SqlCommand(sql_dbExists, myConn_master))
                {

                    using (SqlDataAdapter sqlData = new SqlDataAdapter(sql_dbExists, myConn_master))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            sqlData.Fill(ds);
                            if (ds.Tables.Count > 0)
                            {
                                result = true;
                            }
                        }
                    }
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
        public bool CreateTable(string dbName, string tableName, string sqlCreateTable)
        {
            bool target = false;
            bool dbExists = IfDatabaseExists(dbName);
            if (dbExists)
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = sqlConString;
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    using (SqlCommand createTbCmd = new SqlCommand(sqlCreateTable, con))
                    {
                        bool tbAlreadyExists = IfTableExists(tableName);
                        if (tbAlreadyExists)
                        {
                            target = true;
                        }
                        else
                        {
                            createTbCmd.ExecuteNonQuery();
                            target = true;
                        }
                    }
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
            bool res = false;
            res = IfDatabaseExists(dbName);
            if (!res)
            {
                using (SqlConnection myConn_master = new SqlConnection($@"Data Source = {DbServer};Initial Catalog=master;Trusted_Connection=True"))
                {
                    if (myConn_master.State != ConnectionState.Open)
                    {
                        myConn_master.Open();
                    }
                    using (SqlCommand dbCreateCmd = new SqlCommand(sqlStr_CreateDb, myConn_master))
                    {
                        dbCreateCmd.ExecuteNonQuery();
                        res = true;
                    }
                }
            }
            return res;

        }



        public bool IfTableExists(string tableName)
        {
            //string checkBoxName = targetCheckBoxName;
            bool target = false;
            string dbCheckCmdStr;
            dbCheckCmdStr = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}';";
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = sqlConString;
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    using (SqlCommand tbCheckCmd = new SqlCommand(dbCheckCmdStr, con))
                    {
                        using (SqlDataReader reader = tbCheckCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (Convert.ToInt32(reader.GetValue(0)) == 1)
                                {
                                    target = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return target;
        }




        public bool UpdateDeviceInfoTable(string tableName, int deviceId, int deviceIdNew, List<TextBox> txtB_DeviceInfo, bool sUsage)
        {
            bool updSuccessful = false;
            SqlConnection myConn = new SqlConnection(sqlConString);
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
                                            $", {S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} = '{sensorUsage}' " +
                                        $" WHERE {S_DeviceInfoColumns[0]} = {deviceId}; ";
                sqlCheckStr = $"SELECT 1 FROM {S_DeviceTable} " +
                            $" WHERE {S_DeviceInfoColumns[0]} = {deviceId} " +
                            $"and {S_DeviceInfoColumns[1]} = '{txtB_DeviceInfo[0].Text}' " +
                            $" and {S_DeviceInfoColumns[2]} = '{txtB_DeviceInfo[1].Text}' " +
                           $" and {S_DeviceInfoColumns[3]} = '{txtB_DeviceInfo[2].Text}' " +
                           $" and {S_DeviceInfoColumns[4]} = '{txtB_DeviceInfo[3].Text}' " +
                           $" and {S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} = '{sensorUsage}';";
            }
            else
            {
                sqlUpdStr = $"UPDATE {S_DeviceTable} " +
                                        $"SET {S_DeviceInfoColumns[0]} = {deviceIdNew}" +
                                        $", {S_DeviceInfoColumns[1]} = '{txtB_DeviceInfo[0].Text}'" +
                                            $", {S_DeviceInfoColumns[2]} = '{txtB_DeviceInfo[1].Text}'" +
                                            $", {S_DeviceInfoColumns[3]} = '{txtB_DeviceInfo[2].Text}'" +
                                            $", {S_DeviceInfoColumns[4]} = '{txtB_DeviceInfo[3].Text}'" +
                                            $", {S_DeviceInfoColumns[S_DeviceInfoColumns.Count - 1]} = '{sensorUsage}' " +
                                        $"WHERE {S_DeviceInfoColumns[0]} = {deviceId}; ";

                sqlCheckStr = $"SELECT 1 FROM {S_DeviceTable} " +
                            $" WHERE {S_DeviceInfoColumns[0]} = {deviceIdNew} ";
                for (int i = 0; i < txtB_DeviceInfo.Count; i++)
                {
                    sqlCheckStr += $"and {S_DeviceInfoColumns[i + 1]} = '{txtB_DeviceInfo[i].Text}' ";
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




        public bool UpdateLimitRangeInfo(int deviceId, int deviceIdNew, CheckBox targetCheckBox, List<NumericUpDown> rangeInfoList)
        {
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
            SqlConnection myConn = new SqlConnection(sqlConString);
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





        public bool UpdateUsageTable(string UsageTable, List<string> usageTableColmn, int deviceIdOld, int deviceIdNew, List<string> usageInfo)
        {
            //Check if ID exists
            SqlConnection myConn = new SqlConnection(sqlConString);
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


    }
}
