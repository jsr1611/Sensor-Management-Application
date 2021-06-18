
namespace AdminPage.Models
{
    partial class TimeSettings_p
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.RetryCount_4 = new System.Windows.Forms.NumericUpDown();
            this.RetryCount = new System.Windows.Forms.Label();
            this.DelayTime_3 = new System.Windows.Forms.NumericUpDown();
            this.DelayTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ConnectionTimeout_2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.COM_Port_1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.AvgDataQuery_7 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.RealTimeDataQuery_6 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ChartRefreshInterval_5 = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RetryCount_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayTime_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionTimeout_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvgDataQuery_7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RealTimeDataQuery_6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartRefreshInterval_5)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(100, 322);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(113, 46);
            this.SaveButton.TabIndex = 147;
            this.SaveButton.Text = "저장";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Clicked);
            // 
            // RetryCount_4
            // 
            this.RetryCount_4.Location = new System.Drawing.Point(169, 135);
            this.RetryCount_4.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.RetryCount_4.Name = "RetryCount_4";
            this.RetryCount_4.Size = new System.Drawing.Size(92, 21);
            this.RetryCount_4.TabIndex = 144;
            this.RetryCount_4.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // RetryCount
            // 
            this.RetryCount.AutoSize = true;
            this.RetryCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RetryCount.Location = new System.Drawing.Point(36, 137);
            this.RetryCount.Name = "RetryCount";
            this.RetryCount.Size = new System.Drawing.Size(69, 12);
            this.RetryCount.TabIndex = 143;
            this.RetryCount.Text = "재시도 갯수";
            // 
            // DelayTime_3
            // 
            this.DelayTime_3.Location = new System.Drawing.Point(169, 102);
            this.DelayTime_3.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.DelayTime_3.Name = "DelayTime_3";
            this.DelayTime_3.Size = new System.Drawing.Size(92, 21);
            this.DelayTime_3.TabIndex = 142;
            this.DelayTime_3.Value = new decimal(new int[] {
            55,
            0,
            0,
            0});
            // 
            // DelayTime
            // 
            this.DelayTime.AutoSize = true;
            this.DelayTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DelayTime.Location = new System.Drawing.Point(36, 104);
            this.DelayTime.Name = "DelayTime";
            this.DelayTime.Size = new System.Drawing.Size(57, 12);
            this.DelayTime.TabIndex = 141;
            this.DelayTime.Text = "지연 시간";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(267, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 12);
            this.label1.TabIndex = 148;
            this.label1.Text = "s";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(267, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 12);
            this.label2.TabIndex = 149;
            this.label2.Text = "times";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(267, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 152;
            this.label3.Text = "ms";
            // 
            // ConnectionTimeout_2
            // 
            this.ConnectionTimeout_2.Location = new System.Drawing.Point(169, 73);
            this.ConnectionTimeout_2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ConnectionTimeout_2.Name = "ConnectionTimeout_2";
            this.ConnectionTimeout_2.Size = new System.Drawing.Size(92, 21);
            this.ConnectionTimeout_2.TabIndex = 151;
            this.ConnectionTimeout_2.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(36, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 12);
            this.label4.TabIndex = 150;
            this.label4.Text = "연결 Timeout 시간";
            // 
            // COM_Port_1
            // 
            this.COM_Port_1.FormattingEnabled = true;
            this.COM_Port_1.Location = new System.Drawing.Point(169, 45);
            this.COM_Port_1.Name = "COM_Port_1";
            this.COM_Port_1.Size = new System.Drawing.Size(92, 20);
            this.COM_Port_1.TabIndex = 153;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(36, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 12);
            this.label5.TabIndex = 154;
            this.label5.Text = "COM 포트";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(23, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(296, 19);
            this.label6.TabIndex = 155;
            this.label6.Text = "데이터 수집 관련 시간 설정";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(286, 279);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 12);
            this.label10.TabIndex = 171;
            this.label10.Text = "분(m)";
            // 
            // AvgDataQuery_7
            // 
            this.AvgDataQuery_7.Location = new System.Drawing.Point(188, 276);
            this.AvgDataQuery_7.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.AvgDataQuery_7.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.AvgDataQuery_7.Name = "AvgDataQuery_7";
            this.AvgDataQuery_7.Size = new System.Drawing.Size(92, 21);
            this.AvgDataQuery_7.TabIndex = 170;
            this.AvgDataQuery_7.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(22, 279);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 12);
            this.label11.TabIndex = 169;
            this.label11.Text = "평균 데이터 쿼리 시간";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(285, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 12);
            this.label8.TabIndex = 168;
            this.label8.Text = "초(s)";
            // 
            // RealTimeDataQuery_6
            // 
            this.RealTimeDataQuery_6.Location = new System.Drawing.Point(187, 244);
            this.RealTimeDataQuery_6.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.RealTimeDataQuery_6.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.RealTimeDataQuery_6.Name = "RealTimeDataQuery_6";
            this.RealTimeDataQuery_6.Size = new System.Drawing.Size(92, 21);
            this.RealTimeDataQuery_6.TabIndex = 167;
            this.RealTimeDataQuery_6.Value = new decimal(new int[] {
            120,
            0,
            0,
            -2147483648});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(21, 247);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 12);
            this.label9.TabIndex = 166;
            this.label9.Text = "실시간 데이터 쿼리 시간";
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(23, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(296, 19);
            this.label7.TabIndex = 165;
            this.label7.Text = "데이터 시각화 관련 시간 설정";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(285, 215);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 12);
            this.label12.TabIndex = 164;
            this.label12.Text = "초(s)";
            // 
            // ChartRefreshInterval_5
            // 
            this.ChartRefreshInterval_5.Location = new System.Drawing.Point(187, 212);
            this.ChartRefreshInterval_5.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ChartRefreshInterval_5.Name = "ChartRefreshInterval_5";
            this.ChartRefreshInterval_5.Size = new System.Drawing.Size(92, 21);
            this.ChartRefreshInterval_5.TabIndex = 163;
            this.ChartRefreshInterval_5.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(21, 215);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 12);
            this.label13.TabIndex = 162;
            this.label13.Text = "차트 Refresh 간격";
            // 
            // TimeSettings_p
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 380);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.AvgDataQuery_7);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.RealTimeDataQuery_6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ChartRefreshInterval_5);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.COM_Port_1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ConnectionTimeout_2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RetryCount_4);
            this.Controls.Add(this.RetryCount);
            this.Controls.Add(this.DelayTime_3);
            this.Controls.Add(this.DelayTime);
            this.Name = "TimeSettings_p";
            this.Text = "시간 설정";
            ((System.ComponentModel.ISupportInitialize)(this.RetryCount_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayTime_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionTimeout_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvgDataQuery_7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RealTimeDataQuery_6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartRefreshInterval_5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.NumericUpDown RetryCount_4;
        private System.Windows.Forms.Label RetryCount;
        private System.Windows.Forms.NumericUpDown DelayTime_3;
        private System.Windows.Forms.Label DelayTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ConnectionTimeout_2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox COM_Port_1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown AvgDataQuery_7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown RealTimeDataQuery_6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown ChartRefreshInterval_5;
        private System.Windows.Forms.Label label13;
    }
}