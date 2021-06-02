using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminPage
{
    public partial class TimeoutSettings : Form
    {

        public string sqlConString { get; set; }
        public ValueTuple<int, int, int> S_TimeoutSettings { get; set; }
        public string S_TimeoutTable { get; set; }
        public TimeoutSettings()
        {
            InitializeComponent();



        }

        internal void UpdateTimeoutTable()
        {
            
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool finished = false;
            try
            {
                int sharpOn = sharpOnTime.Checked ? 1 : 0;
                S_TimeoutSettings = (Convert.ToInt32(RetrLimit1.Value), Convert.ToInt32(RetryTotal1.Value), sharpOn);

                string InsertSql = $"INSERT INTO {S_TimeoutTable} VALUES({S_TimeoutSettings.Item1},{S_TimeoutSettings.Item2}, {S_TimeoutSettings.Item3}, '');";
                using (SqlConnection con = new SqlConnection(sqlConString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(InsertSql, con))
                    {
                        cmd.ExecuteNonQuery();
                        finished = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            string msg = string.Empty;
            if (finished)
            {
                msg = "Timeout Settings Successfully Updated.";
            }
            else
            {
                msg = "Timeout Settings Failed.";
            }
            MessageBox.Show(msg, "Update status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
