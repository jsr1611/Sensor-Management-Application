
namespace AdminPage
{
    partial class TimeSettings
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
            this.c_sharpOnTime = new System.Windows.Forms.CheckBox();
            this.label27 = new System.Windows.Forms.Label();
            this.c_RetryTotal1 = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.c_RetryLimit1 = new System.Windows.Forms.NumericUpDown();
            this.label_RetrLimit1 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.c_chartRefreshInterval = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.c_RTQueryLimitTime = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.c_AvgQueryLimitTime = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.c_RetryTotal1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_RetryLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_chartRefreshInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_RTQueryLimitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_AvgQueryLimitTime)).BeginInit();
            this.SuspendLayout();
            // 
            // c_sharpOnTime
            // 
            this.c_sharpOnTime.AutoSize = true;
            this.c_sharpOnTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.c_sharpOnTime.Location = new System.Drawing.Point(215, 114);
            this.c_sharpOnTime.Name = "c_sharpOnTime";
            this.c_sharpOnTime.Size = new System.Drawing.Size(15, 14);
            this.c_sharpOnTime.TabIndex = 139;
            this.c_sharpOnTime.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label27.Location = new System.Drawing.Point(21, 116);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(133, 12);
            this.label27.TabIndex = 138;
            this.label27.Text = "칼순환 (SharpOnTime)";
            // 
            // c_RetryTotal1
            // 
            this.c_RetryTotal1.Location = new System.Drawing.Point(187, 82);
            this.c_RetryTotal1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.c_RetryTotal1.Name = "c_RetryTotal1";
            this.c_RetryTotal1.Size = new System.Drawing.Size(92, 21);
            this.c_RetryTotal1.TabIndex = 137;
            this.c_RetryTotal1.Value = new decimal(new int[] {
            118,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(21, 85);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(153, 12);
            this.label17.TabIndex = 136;
            this.label17.Text = "센서 전체에 대한 재한 시간";
            // 
            // c_RetryLimit1
            // 
            this.c_RetryLimit1.Location = new System.Drawing.Point(187, 51);
            this.c_RetryLimit1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.c_RetryLimit1.Name = "c_RetryLimit1";
            this.c_RetryLimit1.Size = new System.Drawing.Size(92, 21);
            this.c_RetryLimit1.TabIndex = 135;
            this.c_RetryLimit1.Value = new decimal(new int[] {
            55,
            0,
            0,
            0});
            // 
            // label_RetrLimit1
            // 
            this.label_RetrLimit1.AutoSize = true;
            this.label_RetrLimit1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label_RetrLimit1.Location = new System.Drawing.Point(21, 54);
            this.label_RetrLimit1.Name = "label_RetrLimit1";
            this.label_RetrLimit1.Size = new System.Drawing.Size(113, 12);
            this.label_RetrLimit1.TabIndex = 134;
            this.label_RetrLimit1.Text = "센서 개별 재한 시간";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(99, 312);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(113, 46);
            this.SaveButton.TabIndex = 140;
            this.SaveButton.Text = "저장";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(285, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 12);
            this.label1.TabIndex = 149;
            this.label1.Text = "초(s)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(285, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 12);
            this.label2.TabIndex = 150;
            this.label2.Text = "초(s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(285, 214);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 12);
            this.label3.TabIndex = 153;
            this.label3.Text = "초(s)";
            // 
            // c_chartRefreshInterval
            // 
            this.c_chartRefreshInterval.Location = new System.Drawing.Point(187, 211);
            this.c_chartRefreshInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.c_chartRefreshInterval.Name = "c_chartRefreshInterval";
            this.c_chartRefreshInterval.Size = new System.Drawing.Size(92, 21);
            this.c_chartRefreshInterval.TabIndex = 152;
            this.c_chartRefreshInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(21, 214);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 12);
            this.label4.TabIndex = 151;
            this.label4.Text = "차트 Refresh 간격";
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(23, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(296, 19);
            this.label5.TabIndex = 154;
            this.label5.Text = "데이터 수집 관련 시간 설정";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(23, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(296, 19);
            this.label6.TabIndex = 155;
            this.label6.Text = "데이터 시각화 관련 시간 설정";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(285, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 12);
            this.label8.TabIndex = 158;
            this.label8.Text = "초(s)";
            // 
            // c_RTQueryLimitTime
            // 
            this.c_RTQueryLimitTime.Location = new System.Drawing.Point(187, 243);
            this.c_RTQueryLimitTime.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.c_RTQueryLimitTime.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.c_RTQueryLimitTime.Name = "c_RTQueryLimitTime";
            this.c_RTQueryLimitTime.Size = new System.Drawing.Size(92, 21);
            this.c_RTQueryLimitTime.TabIndex = 157;
            this.c_RTQueryLimitTime.Value = new decimal(new int[] {
            120,
            0,
            0,
            -2147483648});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(21, 246);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 12);
            this.label9.TabIndex = 156;
            this.label9.Text = "실시간 데이터 쿼리 시간";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(286, 278);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 12);
            this.label10.TabIndex = 161;
            this.label10.Text = "분(m)";
            // 
            // c_AvgQueryLimitTime
            // 
            this.c_AvgQueryLimitTime.Location = new System.Drawing.Point(188, 275);
            this.c_AvgQueryLimitTime.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.c_AvgQueryLimitTime.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.c_AvgQueryLimitTime.Name = "c_AvgQueryLimitTime";
            this.c_AvgQueryLimitTime.Size = new System.Drawing.Size(92, 21);
            this.c_AvgQueryLimitTime.TabIndex = 160;
            this.c_AvgQueryLimitTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(22, 278);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 12);
            this.label11.TabIndex = 159;
            this.label11.Text = "평균 데이터 쿼리 시간";
            // 
            // TimeSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 380);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.c_AvgQueryLimitTime);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.c_RTQueryLimitTime);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.c_chartRefreshInterval);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.c_sharpOnTime);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.c_RetryTotal1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.c_RetryLimit1);
            this.Controls.Add(this.label_RetrLimit1);
            this.Name = "TimeSettings";
            this.Text = "시간 설정";
            ((System.ComponentModel.ISupportInitialize)(this.c_RetryTotal1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_RetryLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_chartRefreshInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_RTQueryLimitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c_AvgQueryLimitTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox c_sharpOnTime;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.NumericUpDown c_RetryTotal1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown c_RetryLimit1;
        private System.Windows.Forms.Label label_RetrLimit1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown c_chartRefreshInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown c_RTQueryLimitTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown c_AvgQueryLimitTime;
        private System.Windows.Forms.Label label11;
    }
}