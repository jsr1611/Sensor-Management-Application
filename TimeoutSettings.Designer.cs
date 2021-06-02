
namespace AdminPage
{
    partial class TimeoutSettings
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
            this.sharpOnTime = new System.Windows.Forms.CheckBox();
            this.label27 = new System.Windows.Forms.Label();
            this.RetryTotal1 = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.RetrLimit1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RetryTotal1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RetrLimit1)).BeginInit();
            this.SuspendLayout();
            // 
            // sharpOnTime
            // 
            this.sharpOnTime.AutoSize = true;
            this.sharpOnTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sharpOnTime.Location = new System.Drawing.Point(182, 160);
            this.sharpOnTime.Name = "sharpOnTime";
            this.sharpOnTime.Size = new System.Drawing.Size(15, 14);
            this.sharpOnTime.TabIndex = 139;
            this.sharpOnTime.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label27.Location = new System.Drawing.Point(93, 160);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(83, 12);
            this.label27.TabIndex = 138;
            this.label27.Text = "SharpOnTime";
            // 
            // RetryTotal1
            // 
            this.RetryTotal1.Location = new System.Drawing.Point(169, 114);
            this.RetryTotal1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.RetryTotal1.Name = "RetryTotal1";
            this.RetryTotal1.Size = new System.Drawing.Size(92, 21);
            this.RetryTotal1.TabIndex = 137;
            this.RetryTotal1.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(93, 117);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(71, 12);
            this.label17.TabIndex = 136;
            this.label17.Text = "ReTry Total";
            // 
            // RetrLimit1
            // 
            this.RetrLimit1.Location = new System.Drawing.Point(169, 66);
            this.RetrLimit1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.RetrLimit1.Name = "RetrLimit1";
            this.RetrLimit1.Size = new System.Drawing.Size(92, 21);
            this.RetrLimit1.TabIndex = 135;
            this.RetrLimit1.Value = new decimal(new int[] {
            55,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(93, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 12);
            this.label7.TabIndex = 134;
            this.label7.Text = "ReTry Limit";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(107, 197);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(113, 46);
            this.SaveButton.TabIndex = 140;
            this.SaveButton.Text = "저장";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // TimeoutSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 268);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.sharpOnTime);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.RetryTotal1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.RetrLimit1);
            this.Controls.Add(this.label7);
            this.Name = "TimeoutSettings";
            this.Text = "TimeoutSettings";
            ((System.ComponentModel.ISupportInitialize)(this.RetryTotal1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RetrLimit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox sharpOnTime;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.NumericUpDown RetryTotal1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown RetrLimit1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button SaveButton;
    }
}