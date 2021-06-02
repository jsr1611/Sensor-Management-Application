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

namespace AdminPage.Models
{
    public partial class TimeoutSettings_p : Form
    {
        public string sqlConString { get; set; }
        public ValueTuple<int, int> S_TimeoutSettings_p { get; set; }
        public string S_TimeoutTable_p { get; set; }

        public TimeoutSettings_p()
        {
            InitializeComponent();


        


    }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool finished = false;
            try
            {
                S_TimeoutSettings_p = (Convert.ToInt32(DelayTime1.Value), Convert.ToInt32(RetryCount1.Value));

                string InsertSql = $"IF EXISTS(SELECT * FROM sysobjects " +
                                                $" WHERE name = '{S_TimeoutTable_p}' AND xtype = 'U')  INSERT INTO {S_TimeoutTable_p} VALUES({S_TimeoutSettings_p.Item1},{S_TimeoutSettings_p.Item2}, '');";
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
                msg = "Extra Settings Successfully Updated.";
            }
            else
            {
                msg = "Extra Settings Failed.";
            }
            MessageBox.Show(msg, "Update status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
