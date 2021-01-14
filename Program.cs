using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataCollectionApp2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*var main = new Form1();
            main.FormClosing += new FormClosingEventHandler(FormClosing);
            main.Show();*/
            Application.Run(new Form1());
        }

        /*static void FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if(e.CloseReason == CloseReason.UserClosing)
            {
                //(NotifyIcon)notifyIcon1.Visible = true;
                ((Form)sender).Hide();
                e.Cancel = true;
                //((Form)sender).WindowState = FormWindowState.Minimized;
                
            }
            //((Form)sender).FormClosed -= FormClosed;
            *//*if (Application.OpenForms.Count == 0) Application.ExitThread();
            else Application.OpenForms[0].FormClosed += FormClosed;*//*
        }*/
        
    }
}
