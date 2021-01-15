
namespace DataCollectionApp2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.b_start = new System.Windows.Forms.Button();
            this.b_stop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.b_addSensor = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.S_Start = new System.Windows.Forms.ToolStripMenuItem();
            this.S_Stop = new System.Windows.Forms.ToolStripMenuItem();
            this.S_AddNewSensor = new System.Windows.Forms.ToolStripMenuItem();
            this.S_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.b_save = new System.Windows.Forms.Button();
            this.t_time = new System.Windows.Forms.TextBox();
            this.t_part03 = new System.Windows.Forms.TextBox();
            this.t_temp = new System.Windows.Forms.TextBox();
            this.t_no = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label_no = new System.Windows.Forms.Label();
            this.label_temp = new System.Windows.Forms.Label();
            this.t_humid = new System.Windows.Forms.TextBox();
            this.t_part05 = new System.Windows.Forms.TextBox();
            this.label_humid = new System.Windows.Forms.Label();
            this.label_part03 = new System.Windows.Forms.Label();
            this.label_part05 = new System.Windows.Forms.Label();
            this.label_time = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.b_dataCollection_status = new System.Windows.Forms.Button();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.t_hahan1 = new System.Windows.Forms.TextBox();
            this.t_hahan2 = new System.Windows.Forms.TextBox();
            this.t_sanghan1 = new System.Windows.Forms.TextBox();
            this.t_sanghan2 = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // b_start
            // 
            resources.ApplyResources(this.b_start, "b_start");
            this.b_start.Name = "b_start";
            this.b_start.UseVisualStyleBackColor = true;
            this.b_start.Click += new System.EventHandler(this.b_start_Click);
            // 
            // b_stop
            // 
            resources.ApplyResources(this.b_stop, "b_stop");
            this.b_stop.Name = "b_stop";
            this.b_stop.UseVisualStyleBackColor = true;
            this.b_stop.Click += new System.EventHandler(this.b_stop_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // b_addSensor
            // 
            resources.ApplyResources(this.b_addSensor, "b_addSensor");
            this.b_addSensor.Name = "b_addSensor";
            this.b_addSensor.UseVisualStyleBackColor = true;
            this.b_addSensor.Click += new System.EventHandler(this.b_add_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.Name = "listView1";
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.sensorToolStripMenuItem,
            this.aboutToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.F_Exit_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.cutToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
            // 
            // sensorToolStripMenuItem
            // 
            this.sensorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.S_Start,
            this.S_Stop,
            this.S_AddNewSensor,
            this.S_Save});
            this.sensorToolStripMenuItem.Name = "sensorToolStripMenuItem";
            resources.ApplyResources(this.sensorToolStripMenuItem, "sensorToolStripMenuItem");
            // 
            // S_Start
            // 
            this.S_Start.Name = "S_Start";
            resources.ApplyResources(this.S_Start, "S_Start");
            this.S_Start.Click += new System.EventHandler(this.b_start_Click);
            // 
            // S_Stop
            // 
            this.S_Stop.Name = "S_Stop";
            resources.ApplyResources(this.S_Stop, "S_Stop");
            this.S_Stop.Click += new System.EventHandler(this.b_stop_Click);
            // 
            // S_AddNewSensor
            // 
            this.S_AddNewSensor.Name = "S_AddNewSensor";
            resources.ApplyResources(this.S_AddNewSensor, "S_AddNewSensor");
            this.S_AddNewSensor.Click += new System.EventHandler(this.b_add_Click);
            // 
            // S_Save
            // 
            this.S_Save.Name = "S_Save";
            resources.ApplyResources(this.S_Save, "S_Save");
            this.S_Save.Click += new System.EventHandler(this.b_save_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1,
            this.contactToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            resources.ApplyResources(this.aboutToolStripMenuItem1, "aboutToolStripMenuItem1");
            // 
            // contactToolStripMenuItem
            // 
            this.contactToolStripMenuItem.Name = "contactToolStripMenuItem";
            resources.ApplyResources(this.contactToolStripMenuItem, "contactToolStripMenuItem");
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // textBox3
            // 
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            // 
            // textBox4
            // 
            resources.ApplyResources(this.textBox4, "textBox4");
            this.textBox4.Name = "textBox4";
            // 
            // textBox5
            // 
            resources.ApplyResources(this.textBox5, "textBox5");
            this.textBox5.Name = "textBox5";
            this.textBox5.Click += new System.EventHandler(this.textBox5_Click);
            // 
            // b_save
            // 
            resources.ApplyResources(this.b_save, "b_save");
            this.b_save.Name = "b_save";
            this.b_save.UseVisualStyleBackColor = true;
            this.b_save.Click += new System.EventHandler(this.b_save_Click);
            // 
            // t_time
            // 
            resources.ApplyResources(this.t_time, "t_time");
            this.t_time.Name = "t_time";
            // 
            // t_part03
            // 
            resources.ApplyResources(this.t_part03, "t_part03");
            this.t_part03.Name = "t_part03";
            // 
            // t_temp
            // 
            resources.ApplyResources(this.t_temp, "t_temp");
            this.t_temp.Name = "t_temp";
            // 
            // t_no
            // 
            resources.ApplyResources(this.t_no, "t_no");
            this.t_no.Name = "t_no";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label_no
            // 
            resources.ApplyResources(this.label_no, "label_no");
            this.label_no.Name = "label_no";
            // 
            // label_temp
            // 
            resources.ApplyResources(this.label_temp, "label_temp");
            this.label_temp.Name = "label_temp";
            // 
            // t_humid
            // 
            resources.ApplyResources(this.t_humid, "t_humid");
            this.t_humid.Name = "t_humid";
            // 
            // t_part05
            // 
            resources.ApplyResources(this.t_part05, "t_part05");
            this.t_part05.Name = "t_part05";
            // 
            // label_humid
            // 
            resources.ApplyResources(this.label_humid, "label_humid");
            this.label_humid.Name = "label_humid";
            // 
            // label_part03
            // 
            resources.ApplyResources(this.label_part03, "label_part03");
            this.label_part03.Name = "label_part03";
            // 
            // label_part05
            // 
            resources.ApplyResources(this.label_part05, "label_part05");
            this.label_part05.Name = "label_part05";
            // 
            // label_time
            // 
            resources.ApplyResources(this.label_time, "label_time");
            this.label_time.Name = "label_time";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // b_dataCollection_status
            // 
            resources.ApplyResources(this.b_dataCollection_status, "b_dataCollection_status");
            this.b_dataCollection_status.Name = "b_dataCollection_status";
            this.b_dataCollection_status.UseVisualStyleBackColor = true;
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // columnHeader7
            // 
            resources.ApplyResources(this.columnHeader7, "columnHeader7");
            // 
            // columnHeader8
            // 
            resources.ApplyResources(this.columnHeader8, "columnHeader8");
            // 
            // columnHeader9
            // 
            resources.ApplyResources(this.columnHeader9, "columnHeader9");
            // 
            // t_hahan1
            // 
            resources.ApplyResources(this.t_hahan1, "t_hahan1");
            this.t_hahan1.Name = "t_hahan1";
            // 
            // t_hahan2
            // 
            resources.ApplyResources(this.t_hahan2, "t_hahan2");
            this.t_hahan2.Name = "t_hahan2";
            // 
            // t_sanghan1
            // 
            resources.ApplyResources(this.t_sanghan1, "t_sanghan1");
            this.t_sanghan1.Name = "t_sanghan1";
            // 
            // t_sanghan2
            // 
            resources.ApplyResources(this.t_sanghan2, "t_sanghan2");
            this.t_sanghan2.Name = "t_sanghan2";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.t_sanghan2);
            this.Controls.Add(this.t_sanghan1);
            this.Controls.Add(this.t_hahan2);
            this.Controls.Add(this.t_hahan1);
            this.Controls.Add(this.b_dataCollection_status);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_time);
            this.Controls.Add(this.label_part05);
            this.Controls.Add(this.label_part03);
            this.Controls.Add(this.label_humid);
            this.Controls.Add(this.t_part05);
            this.Controls.Add(this.t_humid);
            this.Controls.Add(this.label_temp);
            this.Controls.Add(this.label_no);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.t_time);
            this.Controls.Add(this.t_part03);
            this.Controls.Add(this.t_temp);
            this.Controls.Add(this.t_no);
            this.Controls.Add(this.b_save);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_addSensor);
            this.Controls.Add(this.b_stop);
            this.Controls.Add(this.b_start);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_start;
        private System.Windows.Forms.Button b_stop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button b_addSensor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem contactToolStripMenuItem;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.TextBox t_time;
        private System.Windows.Forms.TextBox t_part03;
        private System.Windows.Forms.TextBox t_temp;
        private System.Windows.Forms.TextBox t_no;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_no;
        private System.Windows.Forms.Label label_temp;
        private System.Windows.Forms.TextBox t_humid;
        private System.Windows.Forms.TextBox t_part05;
        private System.Windows.Forms.Label label_humid;
        private System.Windows.Forms.Label label_part03;
        private System.Windows.Forms.Label label_part05;
        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.ToolStripMenuItem sensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem S_Start;
        private System.Windows.Forms.ToolStripMenuItem S_Stop;
        private System.Windows.Forms.ToolStripMenuItem S_AddNewSensor;
        private System.Windows.Forms.ToolStripMenuItem S_Save;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button b_dataCollection_status;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.TextBox t_hahan1;
        private System.Windows.Forms.TextBox t_hahan2;
        private System.Windows.Forms.TextBox t_sanghan1;
        private System.Windows.Forms.TextBox t_sanghan2;
    }
}

