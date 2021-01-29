using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace DataCollectionApp2
{
    /// <summary>
    /// DB에서 틱정 
    /// </summary>
    public class DbTableHandler
    {
        public List<CheckBox> usageCheckers { get; set; }
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
        public List<string> CheckTablesExistHandler(List<CheckBox> usageCheckers)
        {
            List<string> target = new List<string>();
            string tbName;
            for(int i=0; i<usageCheckers.Count; i++)
            {
                tbName = usageCheckers[i].Name;
                if (IfExists(tbName) == true)
                {
                    target.Add(tbName);
                }
                else
                {
                    if(CreateTb(tbName) == true)
                    {
                        target.Add(tbName);
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
                if (IfExists(tbName) == true)
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
            if (IfExists(tbName) == true)
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

        
        private bool CreateTb(string tbName)
        {
            bool tbCreated = false;
            SqlConnection myConn = new SqlConnection($@"Data Source ={ connStr[0] }; Initial Catalog = { connStr[1] }; User id = { connStr[2] }; Password ={ connStr[3]}; Min Pool Size = 20");
            // Create Table command 
            #region
            string tbCreateCmdStr;
            tbCreateCmdStr = $"Create TABLE [{connStr[1]}].[dbo].[{tbName}] ( "+
                            " sID INT IDENTITY NOT NULL, "+
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
                if(myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return tbCreated;
        }


        private bool IfExists(string checkBoxName)
        {
            bool target = false;
            string dbCheckCmdStr;
            SqlConnection myConn = new SqlConnection($@"Data Source ={ connStr[0] }; Initial Catalog = { connStr[1] }; User id = { connStr[2] }; Password ={ connStr[3]}; Min Pool Size = 20");
            dbCheckCmdStr = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{checkBoxName}';";
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

        public void UpdateLimitRangeInfo(List<string> tbNames, List<NumericUpDown> RangeInfo)
        {
            for(int i =0; i<tbNames.Count; i++)
            {



            }
        }

        private bool AddInfoToDB(string tbName)
        {
            bool target = false;

            return target;
        }



    }
}
