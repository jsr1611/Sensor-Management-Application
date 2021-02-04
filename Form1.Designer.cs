
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
            this.label1 = new System.Windows.Forms.Label();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.b_dataCollection_status = new System.Windows.Forms.Button();
            this.b_stop = new System.Windows.Forms.Button();
            this.b_start = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.b_save = new System.Windows.Forms.Button();
            this.b_addSensor = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel7 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.label13 = new System.Windows.Forms.Label();
            this.panel_left = new System.Windows.Forms.Panel();
            this.panel_right = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.s_p100HigherLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p100HigherLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p100LowerLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p100LowerLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p50HigherLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p50HigherLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p50LowerLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p50LowerLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p25HigherLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p25HigherLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p25LowerLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p25LowerLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p10HigherLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p10HigherLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p10LowerLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p10LowerLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p05HigherLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p05HigherLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p05LowerLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p05LowerLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p03HigherLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p03HigherLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_p03LowerLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_p03LowerLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_hLowerLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_hLowerLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_hHigherLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_hHigherLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_tLowerLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_tLowerLimit1 = new System.Windows.Forms.NumericUpDown();
            this.s_tHigherLimit2 = new System.Windows.Forms.NumericUpDown();
            this.s_tHigherLimit1 = new System.Windows.Forms.NumericUpDown();
            this.sID = new System.Windows.Forms.NumericUpDown();
            this.c_p100Usage = new System.Windows.Forms.CheckBox();
            this.c_p50Usage = new System.Windows.Forms.CheckBox();
            this.c_p25Usage = new System.Windows.Forms.CheckBox();
            this.c_p10Usage = new System.Windows.Forms.CheckBox();
            this.l_p100Usage = new System.Windows.Forms.Label();
            this.l_p5Usage = new System.Windows.Forms.Label();
            this.l_p25Usage = new System.Windows.Forms.Label();
            this.l_p10Usage = new System.Windows.Forms.Label();
            this.l_p05Usage = new System.Windows.Forms.Label();
            this.c_p05Usage = new System.Windows.Forms.CheckBox();
            this.l_p03Usage = new System.Windows.Forms.Label();
            this.c_p03Usage = new System.Windows.Forms.CheckBox();
            this.l_hUsage = new System.Windows.Forms.Label();
            this.c_hUsage = new System.Windows.Forms.CheckBox();
            this.l_tUsage = new System.Windows.Forms.Label();
            this.c_tUsage = new System.Windows.Forms.CheckBox();
            this.l_sHigherLimit2 = new System.Windows.Forms.Label();
            this.l_sHigherLimit1 = new System.Windows.Forms.Label();
            this.l_sLowerLimit2 = new System.Windows.Forms.Label();
            this.l_sLowerLimit1 = new System.Windows.Forms.Label();
            this.l_sUsage = new System.Windows.Forms.Label();
            this.l_sDescription = new System.Windows.Forms.Label();
            this.l_sLocation = new System.Windows.Forms.Label();
            this.l_sName = new System.Windows.Forms.Label();
            this.l_sID = new System.Windows.Forms.Label();
            this.sDescription = new System.Windows.Forms.TextBox();
            this.sLocation = new System.Windows.Forms.TextBox();
            this.sName = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.s_p100HigherLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p100HigherLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p100LowerLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p100LowerLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p50HigherLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p50HigherLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p50LowerLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p50LowerLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p25HigherLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p25HigherLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p25LowerLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p25LowerLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p10HigherLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p10HigherLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p10LowerLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p10LowerLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p05HigherLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p05HigherLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p05LowerLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p05LowerLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p03HigherLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p03HigherLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p03LowerLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p03LowerLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_hLowerLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_hLowerLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_hHigherLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_hHigherLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_tLowerLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_tLowerLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_tHigherLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_tHigherLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sID)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.panel4, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel6, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel7, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.b_dataCollection_status);
            this.panel4.Controls.Add(this.b_stop);
            this.panel4.Controls.Add(this.b_start);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // b_dataCollection_status
            // 
            resources.ApplyResources(this.b_dataCollection_status, "b_dataCollection_status");
            this.b_dataCollection_status.Image = global::DataCollectionApp2.Properties.Resources.light_off_26;
            this.b_dataCollection_status.Name = "b_dataCollection_status";
            this.b_dataCollection_status.UseVisualStyleBackColor = true;
            // 
            // b_stop
            // 
            resources.ApplyResources(this.b_stop, "b_stop");
            this.b_stop.Image = global::DataCollectionApp2.Properties.Resources.pause_button_26;
            this.b_stop.Name = "b_stop";
            this.b_stop.UseVisualStyleBackColor = true;
            this.b_stop.Click += new System.EventHandler(this.b_stop_Click);
            // 
            // b_start
            // 
            resources.ApplyResources(this.b_start, "b_start");
            this.b_start.Image = global::DataCollectionApp2.Properties.Resources.start_26;
            this.b_start.Name = "b_start";
            this.b_start.UseVisualStyleBackColor = true;
            this.b_start.Click += new System.EventHandler(this.b_start_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.b_save);
            this.panel5.Controls.Add(this.b_addSensor);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // b_save
            // 
            this.b_save.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.b_save, "b_save");
            this.b_save.Image = global::DataCollectionApp2.Properties.Resources.save_26;
            this.b_save.Name = "b_save";
            this.b_save.UseVisualStyleBackColor = false;
            this.b_save.Click += new System.EventHandler(this.b_save_Click);
            // 
            // b_addSensor
            // 
            resources.ApplyResources(this.b_addSensor, "b_addSensor");
            this.b_addSensor.Image = global::DataCollectionApp2.Properties.Resources.add_26;
            this.b_addSensor.Name = "b_addSensor";
            this.b_addSensor.UseVisualStyleBackColor = true;
            this.b_addSensor.Click += new System.EventHandler(this.b_add_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.splitter2);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.splitter1);
            this.panel7.Controls.Add(this.label13);
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Name = "panel7";
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // panel_left
            // 
            resources.ApplyResources(this.panel_left, "panel_left");
            this.panel_left.Name = "panel_left";
            // 
            // panel_right
            // 
            resources.ApplyResources(this.panel_right, "panel_right");
            this.panel_right.Name = "panel_right";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.s_p100HigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p100HigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p100LowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p100LowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p50HigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p50HigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p50LowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p50LowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p25HigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p25HigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p25LowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p25LowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p10HigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p10HigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p10LowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p10LowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p05HigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p05HigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p05LowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p05LowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p03HigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p03HigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_p03LowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_p03LowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_hLowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_hLowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_hHigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_hHigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_tLowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_tLowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.s_tHigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.s_tHigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.c_p100Usage);
            this.splitContainer1.Panel2.Controls.Add(this.c_p50Usage);
            this.splitContainer1.Panel2.Controls.Add(this.c_p25Usage);
            this.splitContainer1.Panel2.Controls.Add(this.c_p10Usage);
            this.splitContainer1.Panel2.Controls.Add(this.l_p100Usage);
            this.splitContainer1.Panel2.Controls.Add(this.l_p5Usage);
            this.splitContainer1.Panel2.Controls.Add(this.l_p25Usage);
            this.splitContainer1.Panel2.Controls.Add(this.l_p10Usage);
            this.splitContainer1.Panel2.Controls.Add(this.l_p05Usage);
            this.splitContainer1.Panel2.Controls.Add(this.c_p05Usage);
            this.splitContainer1.Panel2.Controls.Add(this.l_p03Usage);
            this.splitContainer1.Panel2.Controls.Add(this.c_p03Usage);
            this.splitContainer1.Panel2.Controls.Add(this.l_hUsage);
            this.splitContainer1.Panel2.Controls.Add(this.c_hUsage);
            this.splitContainer1.Panel2.Controls.Add(this.l_tUsage);
            this.splitContainer1.Panel2.Controls.Add(this.c_tUsage);
            this.splitContainer1.Panel2.Controls.Add(this.l_sHigherLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.l_sHigherLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.l_sLowerLimit2);
            this.splitContainer1.Panel2.Controls.Add(this.l_sLowerLimit1);
            this.splitContainer1.Panel2.Controls.Add(this.l_sUsage);
            this.splitContainer1.Panel2.Controls.Add(this.l_sDescription);
            this.splitContainer1.Panel2.Controls.Add(this.l_sLocation);
            this.splitContainer1.Panel2.Controls.Add(this.l_sName);
            this.splitContainer1.Panel2.Controls.Add(this.l_sID);
            this.splitContainer1.Panel2.Controls.Add(this.sDescription);
            this.splitContainer1.Panel2.Controls.Add(this.sLocation);
            this.splitContainer1.Panel2.Controls.Add(this.sName);
            this.splitContainer1.Panel2.Controls.Add(this.sID);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader0
            // 
            resources.ApplyResources(this.columnHeader0, "columnHeader0");
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
            // s_p100HigherLimit2
            // 
            resources.ApplyResources(this.s_p100HigherLimit2, "s_p100HigherLimit2");
            this.s_p100HigherLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p100HigherLimit2.Name = "s_p100HigherLimit2";
            // 
            // s_p100HigherLimit1
            // 
            resources.ApplyResources(this.s_p100HigherLimit1, "s_p100HigherLimit1");
            this.s_p100HigherLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p100HigherLimit1.Name = "s_p100HigherLimit1";
            // 
            // s_p100LowerLimit2
            // 
            resources.ApplyResources(this.s_p100LowerLimit2, "s_p100LowerLimit2");
            this.s_p100LowerLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p100LowerLimit2.Name = "s_p100LowerLimit2";
            // 
            // s_p100LowerLimit1
            // 
            resources.ApplyResources(this.s_p100LowerLimit1, "s_p100LowerLimit1");
            this.s_p100LowerLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p100LowerLimit1.Name = "s_p100LowerLimit1";
            // 
            // s_p50HigherLimit2
            // 
            resources.ApplyResources(this.s_p50HigherLimit2, "s_p50HigherLimit2");
            this.s_p50HigherLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p50HigherLimit2.Name = "s_p50HigherLimit2";
            // 
            // s_p50HigherLimit1
            // 
            resources.ApplyResources(this.s_p50HigherLimit1, "s_p50HigherLimit1");
            this.s_p50HigherLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p50HigherLimit1.Name = "s_p50HigherLimit1";
            // 
            // s_p50LowerLimit2
            // 
            resources.ApplyResources(this.s_p50LowerLimit2, "s_p50LowerLimit2");
            this.s_p50LowerLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p50LowerLimit2.Name = "s_p50LowerLimit2";
            // 
            // s_p50LowerLimit1
            // 
            resources.ApplyResources(this.s_p50LowerLimit1, "s_p50LowerLimit1");
            this.s_p50LowerLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p50LowerLimit1.Name = "s_p50LowerLimit1";
            // 
            // s_p25HigherLimit2
            // 
            resources.ApplyResources(this.s_p25HigherLimit2, "s_p25HigherLimit2");
            this.s_p25HigherLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p25HigherLimit2.Name = "s_p25HigherLimit2";
            // 
            // s_p25HigherLimit1
            // 
            resources.ApplyResources(this.s_p25HigherLimit1, "s_p25HigherLimit1");
            this.s_p25HigherLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p25HigherLimit1.Name = "s_p25HigherLimit1";
            // 
            // s_p25LowerLimit2
            // 
            resources.ApplyResources(this.s_p25LowerLimit2, "s_p25LowerLimit2");
            this.s_p25LowerLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p25LowerLimit2.Name = "s_p25LowerLimit2";
            // 
            // s_p25LowerLimit1
            // 
            resources.ApplyResources(this.s_p25LowerLimit1, "s_p25LowerLimit1");
            this.s_p25LowerLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p25LowerLimit1.Name = "s_p25LowerLimit1";
            // 
            // s_p10HigherLimit2
            // 
            resources.ApplyResources(this.s_p10HigherLimit2, "s_p10HigherLimit2");
            this.s_p10HigherLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p10HigherLimit2.Name = "s_p10HigherLimit2";
            // 
            // s_p10HigherLimit1
            // 
            resources.ApplyResources(this.s_p10HigherLimit1, "s_p10HigherLimit1");
            this.s_p10HigherLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p10HigherLimit1.Name = "s_p10HigherLimit1";
            // 
            // s_p10LowerLimit2
            // 
            resources.ApplyResources(this.s_p10LowerLimit2, "s_p10LowerLimit2");
            this.s_p10LowerLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p10LowerLimit2.Name = "s_p10LowerLimit2";
            // 
            // s_p10LowerLimit1
            // 
            resources.ApplyResources(this.s_p10LowerLimit1, "s_p10LowerLimit1");
            this.s_p10LowerLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p10LowerLimit1.Name = "s_p10LowerLimit1";
            // 
            // s_p05HigherLimit2
            // 
            resources.ApplyResources(this.s_p05HigherLimit2, "s_p05HigherLimit2");
            this.s_p05HigherLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p05HigherLimit2.Name = "s_p05HigherLimit2";
            // 
            // s_p05HigherLimit1
            // 
            resources.ApplyResources(this.s_p05HigherLimit1, "s_p05HigherLimit1");
            this.s_p05HigherLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p05HigherLimit1.Name = "s_p05HigherLimit1";
            // 
            // s_p05LowerLimit2
            // 
            resources.ApplyResources(this.s_p05LowerLimit2, "s_p05LowerLimit2");
            this.s_p05LowerLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p05LowerLimit2.Name = "s_p05LowerLimit2";
            // 
            // s_p05LowerLimit1
            // 
            resources.ApplyResources(this.s_p05LowerLimit1, "s_p05LowerLimit1");
            this.s_p05LowerLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p05LowerLimit1.Name = "s_p05LowerLimit1";
            // 
            // s_p03HigherLimit2
            // 
            resources.ApplyResources(this.s_p03HigherLimit2, "s_p03HigherLimit2");
            this.s_p03HigherLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p03HigherLimit2.Name = "s_p03HigherLimit2";
            // 
            // s_p03HigherLimit1
            // 
            resources.ApplyResources(this.s_p03HigherLimit1, "s_p03HigherLimit1");
            this.s_p03HigherLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p03HigherLimit1.Name = "s_p03HigherLimit1";
            // 
            // s_p03LowerLimit2
            // 
            resources.ApplyResources(this.s_p03LowerLimit2, "s_p03LowerLimit2");
            this.s_p03LowerLimit2.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p03LowerLimit2.Name = "s_p03LowerLimit2";
            // 
            // s_p03LowerLimit1
            // 
            resources.ApplyResources(this.s_p03LowerLimit1, "s_p03LowerLimit1");
            this.s_p03LowerLimit1.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.s_p03LowerLimit1.Name = "s_p03LowerLimit1";
            // 
            // s_hLowerLimit2
            // 
            this.s_hLowerLimit2.AllowDrop = true;
            this.s_hLowerLimit2.DecimalPlaces = 2;
            resources.ApplyResources(this.s_hLowerLimit2, "s_hLowerLimit2");
            this.s_hLowerLimit2.Name = "s_hLowerLimit2";
            // 
            // s_hLowerLimit1
            // 
            this.s_hLowerLimit1.DecimalPlaces = 2;
            resources.ApplyResources(this.s_hLowerLimit1, "s_hLowerLimit1");
            this.s_hLowerLimit1.Name = "s_hLowerLimit1";
            // 
            // s_hHigherLimit2
            // 
            this.s_hHigherLimit2.AllowDrop = true;
            this.s_hHigherLimit2.DecimalPlaces = 2;
            resources.ApplyResources(this.s_hHigherLimit2, "s_hHigherLimit2");
            this.s_hHigherLimit2.Name = "s_hHigherLimit2";
            // 
            // s_hHigherLimit1
            // 
            this.s_hHigherLimit1.AllowDrop = true;
            this.s_hHigherLimit1.DecimalPlaces = 2;
            resources.ApplyResources(this.s_hHigherLimit1, "s_hHigherLimit1");
            this.s_hHigherLimit1.Name = "s_hHigherLimit1";
            // 
            // s_tLowerLimit2
            // 
            this.s_tLowerLimit2.AllowDrop = true;
            this.s_tLowerLimit2.DecimalPlaces = 2;
            resources.ApplyResources(this.s_tLowerLimit2, "s_tLowerLimit2");
            this.s_tLowerLimit2.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.s_tLowerLimit2.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.s_tLowerLimit2.Name = "s_tLowerLimit2";
            // 
            // s_tLowerLimit1
            // 
            this.s_tLowerLimit1.DecimalPlaces = 2;
            resources.ApplyResources(this.s_tLowerLimit1, "s_tLowerLimit1");
            this.s_tLowerLimit1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.s_tLowerLimit1.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.s_tLowerLimit1.Name = "s_tLowerLimit1";
            // 
            // s_tHigherLimit2
            // 
            this.s_tHigherLimit2.AllowDrop = true;
            this.s_tHigherLimit2.DecimalPlaces = 2;
            resources.ApplyResources(this.s_tHigherLimit2, "s_tHigherLimit2");
            this.s_tHigherLimit2.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.s_tHigherLimit2.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.s_tHigherLimit2.Name = "s_tHigherLimit2";
            // 
            // s_tHigherLimit1
            // 
            this.s_tHigherLimit1.AllowDrop = true;
            this.s_tHigherLimit1.DecimalPlaces = 2;
            resources.ApplyResources(this.s_tHigherLimit1, "s_tHigherLimit1");
            this.s_tHigherLimit1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.s_tHigherLimit1.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.s_tHigherLimit1.Name = "s_tHigherLimit1";
            // 
            // sID
            // 
            resources.ApplyResources(this.sID, "sID");
            this.sID.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.sID.Name = "sID";
            // 
            // c_p100Usage
            // 
            resources.ApplyResources(this.c_p100Usage, "c_p100Usage");
            this.c_p100Usage.Name = "c_p100Usage";
            this.c_p100Usage.UseVisualStyleBackColor = true;
            this.c_p100Usage.CheckedChanged += new System.EventHandler(this.c_p100Usage_CheckedChanged);
            // 
            // c_p50Usage
            // 
            resources.ApplyResources(this.c_p50Usage, "c_p50Usage");
            this.c_p50Usage.Name = "c_p50Usage";
            this.c_p50Usage.UseVisualStyleBackColor = true;
            this.c_p50Usage.CheckedChanged += new System.EventHandler(this.c_p50Usage_CheckedChanged);
            // 
            // c_p25Usage
            // 
            resources.ApplyResources(this.c_p25Usage, "c_p25Usage");
            this.c_p25Usage.Name = "c_p25Usage";
            this.c_p25Usage.UseVisualStyleBackColor = true;
            this.c_p25Usage.CheckedChanged += new System.EventHandler(this.c_p25Usage_CheckedChanged);
            // 
            // c_p10Usage
            // 
            resources.ApplyResources(this.c_p10Usage, "c_p10Usage");
            this.c_p10Usage.Name = "c_p10Usage";
            this.c_p10Usage.UseVisualStyleBackColor = true;
            this.c_p10Usage.CheckedChanged += new System.EventHandler(this.c_p10Usage_CheckedChanged);
            // 
            // l_p100Usage
            // 
            this.l_p100Usage.AutoEllipsis = true;
            resources.ApplyResources(this.l_p100Usage, "l_p100Usage");
            this.l_p100Usage.Name = "l_p100Usage";
            // 
            // l_p5Usage
            // 
            this.l_p5Usage.AutoEllipsis = true;
            resources.ApplyResources(this.l_p5Usage, "l_p5Usage");
            this.l_p5Usage.Name = "l_p5Usage";
            // 
            // l_p25Usage
            // 
            this.l_p25Usage.AutoEllipsis = true;
            resources.ApplyResources(this.l_p25Usage, "l_p25Usage");
            this.l_p25Usage.Name = "l_p25Usage";
            // 
            // l_p10Usage
            // 
            this.l_p10Usage.AutoEllipsis = true;
            resources.ApplyResources(this.l_p10Usage, "l_p10Usage");
            this.l_p10Usage.Name = "l_p10Usage";
            // 
            // l_p05Usage
            // 
            this.l_p05Usage.AutoEllipsis = true;
            resources.ApplyResources(this.l_p05Usage, "l_p05Usage");
            this.l_p05Usage.Name = "l_p05Usage";
            // 
            // c_p05Usage
            // 
            resources.ApplyResources(this.c_p05Usage, "c_p05Usage");
            this.c_p05Usage.Name = "c_p05Usage";
            this.c_p05Usage.UseVisualStyleBackColor = true;
            this.c_p05Usage.CheckedChanged += new System.EventHandler(this.c_p05Usage_CheckedChanged);
            // 
            // l_p03Usage
            // 
            this.l_p03Usage.AutoEllipsis = true;
            resources.ApplyResources(this.l_p03Usage, "l_p03Usage");
            this.l_p03Usage.Name = "l_p03Usage";
            // 
            // c_p03Usage
            // 
            resources.ApplyResources(this.c_p03Usage, "c_p03Usage");
            this.c_p03Usage.Name = "c_p03Usage";
            this.c_p03Usage.UseVisualStyleBackColor = true;
            this.c_p03Usage.CheckedChanged += new System.EventHandler(this.c_p03Usage_CheckedChanged);
            // 
            // l_hUsage
            // 
            this.l_hUsage.AutoEllipsis = true;
            resources.ApplyResources(this.l_hUsage, "l_hUsage");
            this.l_hUsage.Name = "l_hUsage";
            // 
            // c_hUsage
            // 
            resources.ApplyResources(this.c_hUsage, "c_hUsage");
            this.c_hUsage.Name = "c_hUsage";
            this.c_hUsage.UseVisualStyleBackColor = true;
            this.c_hUsage.CheckedChanged += new System.EventHandler(this.c_hUsage_CheckedChanged);
            // 
            // l_tUsage
            // 
            this.l_tUsage.AutoEllipsis = true;
            resources.ApplyResources(this.l_tUsage, "l_tUsage");
            this.l_tUsage.Name = "l_tUsage";
            // 
            // c_tUsage
            // 
            resources.ApplyResources(this.c_tUsage, "c_tUsage");
            this.c_tUsage.Name = "c_tUsage";
            this.c_tUsage.UseVisualStyleBackColor = true;
            this.c_tUsage.CheckedChanged += new System.EventHandler(this.c_tUsage_CheckedChanged);
            // 
            // l_sHigherLimit2
            // 
            resources.ApplyResources(this.l_sHigherLimit2, "l_sHigherLimit2");
            this.l_sHigherLimit2.Name = "l_sHigherLimit2";
            // 
            // l_sHigherLimit1
            // 
            resources.ApplyResources(this.l_sHigherLimit1, "l_sHigherLimit1");
            this.l_sHigherLimit1.Name = "l_sHigherLimit1";
            // 
            // l_sLowerLimit2
            // 
            resources.ApplyResources(this.l_sLowerLimit2, "l_sLowerLimit2");
            this.l_sLowerLimit2.Name = "l_sLowerLimit2";
            // 
            // l_sLowerLimit1
            // 
            resources.ApplyResources(this.l_sLowerLimit1, "l_sLowerLimit1");
            this.l_sLowerLimit1.Name = "l_sLowerLimit1";
            // 
            // l_sUsage
            // 
            resources.ApplyResources(this.l_sUsage, "l_sUsage");
            this.l_sUsage.Name = "l_sUsage";
            // 
            // l_sDescription
            // 
            resources.ApplyResources(this.l_sDescription, "l_sDescription");
            this.l_sDescription.Name = "l_sDescription";
            // 
            // l_sLocation
            // 
            resources.ApplyResources(this.l_sLocation, "l_sLocation");
            this.l_sLocation.Name = "l_sLocation";
            // 
            // l_sName
            // 
            resources.ApplyResources(this.l_sName, "l_sName");
            this.l_sName.Name = "l_sName";
            // 
            // l_sID
            // 
            resources.ApplyResources(this.l_sID, "l_sID");
            this.l_sID.Name = "l_sID";
            // 
            // sDescription
            // 
            resources.ApplyResources(this.sDescription, "sDescription");
            this.sDescription.Name = "sDescription";
            // 
            // sLocation
            // 
            resources.ApplyResources(this.sLocation, "sLocation");
            this.sLocation.Name = "sLocation";
            // 
            // sName
            // 
            resources.ApplyResources(this.sName, "sName");
            this.sName.Name = "sName";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_right);
            this.Controls.Add(this.panel_left);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.s_p100HigherLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p100HigherLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p100LowerLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p100LowerLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p50HigherLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p50HigherLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p50LowerLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p50LowerLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p25HigherLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p25HigherLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p25LowerLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p25LowerLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p10HigherLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p10HigherLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p10LowerLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p10LowerLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p05HigherLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p05HigherLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p05LowerLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p05LowerLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p03HigherLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p03HigherLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p03LowerLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_p03LowerLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_hLowerLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_hLowerLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_hHigherLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_hHigherLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_tLowerLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_tLowerLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_tHigherLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_tHigherLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.ToolStripMenuItem sensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem S_Start;
        private System.Windows.Forms.ToolStripMenuItem S_Stop;
        private System.Windows.Forms.ToolStripMenuItem S_AddNewSensor;
        private System.Windows.Forms.ToolStripMenuItem S_Save;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button b_dataCollection_status;
        private System.Windows.Forms.Button b_stop;
        private System.Windows.Forms.Button b_start;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel_left;
        private System.Windows.Forms.Panel panel_right;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.Button b_addSensor;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ColumnHeader columnHeader0;
        private System.Windows.Forms.CheckBox c_tUsage;
        private System.Windows.Forms.Label l_sHigherLimit2;
        private System.Windows.Forms.Label l_sHigherLimit1;
        private System.Windows.Forms.Label l_sLowerLimit2;
        private System.Windows.Forms.Label l_sLowerLimit1;
        private System.Windows.Forms.Label l_sUsage;
        private System.Windows.Forms.Label l_sDescription;
        private System.Windows.Forms.Label l_sLocation;
        private System.Windows.Forms.Label l_sName;
        private System.Windows.Forms.Label l_sID;
        private System.Windows.Forms.TextBox sDescription;
        private System.Windows.Forms.TextBox sLocation;
        private System.Windows.Forms.TextBox sName;
        private System.Windows.Forms.Label l_p100Usage;
        private System.Windows.Forms.Label l_p5Usage;
        private System.Windows.Forms.Label l_p25Usage;
        private System.Windows.Forms.Label l_p10Usage;
        private System.Windows.Forms.Label l_p05Usage;
        private System.Windows.Forms.CheckBox c_p05Usage;
        private System.Windows.Forms.Label l_p03Usage;
        private System.Windows.Forms.CheckBox c_p03Usage;
        private System.Windows.Forms.Label l_hUsage;
        private System.Windows.Forms.CheckBox c_hUsage;
        private System.Windows.Forms.Label l_tUsage;
        private System.Windows.Forms.CheckBox c_p100Usage;
        private System.Windows.Forms.CheckBox c_p50Usage;
        private System.Windows.Forms.CheckBox c_p25Usage;
        private System.Windows.Forms.CheckBox c_p10Usage;
        private System.Windows.Forms.NumericUpDown sID;
        private System.Windows.Forms.NumericUpDown s_tHigherLimit1;
        private System.Windows.Forms.NumericUpDown s_tLowerLimit2;
        private System.Windows.Forms.NumericUpDown s_tLowerLimit1;
        private System.Windows.Forms.NumericUpDown s_tHigherLimit2;
        private System.Windows.Forms.NumericUpDown s_hLowerLimit2;
        private System.Windows.Forms.NumericUpDown s_hLowerLimit1;
        private System.Windows.Forms.NumericUpDown s_hHigherLimit2;
        private System.Windows.Forms.NumericUpDown s_hHigherLimit1;
        private System.Windows.Forms.NumericUpDown s_p100HigherLimit2;
        private System.Windows.Forms.NumericUpDown s_p100HigherLimit1;
        private System.Windows.Forms.NumericUpDown s_p100LowerLimit2;
        private System.Windows.Forms.NumericUpDown s_p100LowerLimit1;
        private System.Windows.Forms.NumericUpDown s_p50HigherLimit2;
        private System.Windows.Forms.NumericUpDown s_p50HigherLimit1;
        private System.Windows.Forms.NumericUpDown s_p50LowerLimit2;
        private System.Windows.Forms.NumericUpDown s_p50LowerLimit1;
        private System.Windows.Forms.NumericUpDown s_p25HigherLimit2;
        private System.Windows.Forms.NumericUpDown s_p25HigherLimit1;
        private System.Windows.Forms.NumericUpDown s_p25LowerLimit2;
        private System.Windows.Forms.NumericUpDown s_p25LowerLimit1;
        private System.Windows.Forms.NumericUpDown s_p10HigherLimit2;
        private System.Windows.Forms.NumericUpDown s_p10HigherLimit1;
        private System.Windows.Forms.NumericUpDown s_p10LowerLimit2;
        private System.Windows.Forms.NumericUpDown s_p10LowerLimit1;
        private System.Windows.Forms.NumericUpDown s_p05HigherLimit2;
        private System.Windows.Forms.NumericUpDown s_p05HigherLimit1;
        private System.Windows.Forms.NumericUpDown s_p05LowerLimit2;
        private System.Windows.Forms.NumericUpDown s_p05LowerLimit1;
        private System.Windows.Forms.NumericUpDown s_p03HigherLimit2;
        private System.Windows.Forms.NumericUpDown s_p03HigherLimit1;
        private System.Windows.Forms.NumericUpDown s_p03LowerLimit2;
        private System.Windows.Forms.NumericUpDown s_p03LowerLimit1;
    }
}

