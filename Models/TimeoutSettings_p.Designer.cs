
namespace AdminPage.Models
{
    partial class TimeoutSettings_p
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
            this.RetryCount1 = new System.Windows.Forms.NumericUpDown();
            this.RetryCount = new System.Windows.Forms.Label();
            this.DelayTime1 = new System.Windows.Forms.NumericUpDown();
            this.DelayTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RetryCount1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayTime1)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(107, 177);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(113, 46);
            this.SaveButton.TabIndex = 147;
            this.SaveButton.Text = "저장";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RetryCount1
            // 
            this.RetryCount1.Location = new System.Drawing.Point(159, 97);
            this.RetryCount1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.RetryCount1.Name = "RetryCount1";
            this.RetryCount1.Size = new System.Drawing.Size(92, 21);
            this.RetryCount1.TabIndex = 144;
            this.RetryCount1.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // RetryCount
            // 
            this.RetryCount.AutoSize = true;
            this.RetryCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RetryCount.Location = new System.Drawing.Point(83, 100);
            this.RetryCount.Name = "RetryCount";
            this.RetryCount.Size = new System.Drawing.Size(69, 12);
            this.RetryCount.TabIndex = 143;
            this.RetryCount.Text = "재시도 한도";
            // 
            // DelayTime1
            // 
            this.DelayTime1.Location = new System.Drawing.Point(159, 49);
            this.DelayTime1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.DelayTime1.Name = "DelayTime1";
            this.DelayTime1.Size = new System.Drawing.Size(92, 21);
            this.DelayTime1.TabIndex = 142;
            this.DelayTime1.Value = new decimal(new int[] {
            55,
            0,
            0,
            0});
            // 
            // DelayTime
            // 
            this.DelayTime.AutoSize = true;
            this.DelayTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DelayTime.Location = new System.Drawing.Point(83, 52);
            this.DelayTime.Name = "DelayTime";
            this.DelayTime.Size = new System.Drawing.Size(57, 12);
            this.DelayTime.TabIndex = 141;
            this.DelayTime.Text = "지연 시간";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(257, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 148;
            this.label1.Text = "초";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(257, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 149;
            this.label2.Text = "배";
            // 
            // TimeoutSettings_p
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 268);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RetryCount1);
            this.Controls.Add(this.RetryCount);
            this.Controls.Add(this.DelayTime1);
            this.Controls.Add(this.DelayTime);
            this.Name = "TimeoutSettings_p";
            this.Text = "Extra Settings";
            ((System.ComponentModel.ISupportInitialize)(this.RetryCount1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayTime1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.NumericUpDown RetryCount1;
        private System.Windows.Forms.Label RetryCount;
        private System.Windows.Forms.NumericUpDown DelayTime1;
        private System.Windows.Forms.Label DelayTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}