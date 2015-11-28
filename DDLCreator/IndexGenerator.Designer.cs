namespace DDLCreator
{
    partial class IndexGenerator
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.quantity = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.startValue = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.seperation = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.valueList = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.prefix = new System.Windows.Forms.TextBox();
            this.cancel = new System.Windows.Forms.Button();
            this.generate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quantity)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startValue)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seperation)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.quantity);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(90, 46);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Quantity";
            // 
            // quantity
            // 
            this.quantity.Location = new System.Drawing.Point(6, 19);
            this.quantity.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.quantity.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.quantity.Name = "quantity";
            this.quantity.Size = new System.Drawing.Size(78, 20);
            this.quantity.TabIndex = 0;
            this.quantity.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.quantity.ValueChanged += new System.EventHandler(this.valueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.startValue);
            this.groupBox2.Location = new System.Drawing.Point(12, 64);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(90, 46);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Start Value";
            // 
            // startValue
            // 
            this.startValue.Location = new System.Drawing.Point(6, 19);
            this.startValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.startValue.Name = "startValue";
            this.startValue.Size = new System.Drawing.Size(78, 20);
            this.startValue.TabIndex = 0;
            this.startValue.ValueChanged += new System.EventHandler(this.valueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.seperation);
            this.groupBox3.Location = new System.Drawing.Point(12, 116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(90, 46);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Seperation";
            // 
            // seperation
            // 
            this.seperation.Location = new System.Drawing.Point(6, 19);
            this.seperation.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.seperation.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seperation.Name = "seperation";
            this.seperation.Size = new System.Drawing.Size(78, 20);
            this.seperation.TabIndex = 0;
            this.seperation.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seperation.ValueChanged += new System.EventHandler(this.valueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.valueList);
            this.groupBox4.Location = new System.Drawing.Point(146, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(88, 200);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output";
            // 
            // valueList
            // 
            this.valueList.BackColor = System.Drawing.SystemColors.Control;
            this.valueList.FormattingEnabled = true;
            this.valueList.Location = new System.Drawing.Point(6, 19);
            this.valueList.Name = "valueList";
            this.valueList.Size = new System.Drawing.Size(75, 173);
            this.valueList.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.prefix);
            this.groupBox5.Location = new System.Drawing.Point(12, 168);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(90, 46);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Prefix";
            // 
            // prefix
            // 
            this.prefix.Location = new System.Drawing.Point(7, 20);
            this.prefix.Name = "prefix";
            this.prefix.Size = new System.Drawing.Size(77, 20);
            this.prefix.TabIndex = 0;
            this.prefix.Text = "Index";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(146, 247);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(90, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // generate
            // 
            this.generate.Location = new System.Drawing.Point(146, 218);
            this.generate.Name = "generate";
            this.generate.Size = new System.Drawing.Size(90, 23);
            this.generate.TabIndex = 6;
            this.generate.Text = "OK";
            this.generate.UseVisualStyleBackColor = true;
            this.generate.Click += new System.EventHandler(this.generate_Click);
            // 
            // IndexGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 282);
            this.Controls.Add(this.generate);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IndexGenerator";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Index Generator";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.quantity)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.startValue)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seperation)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown quantity;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown startValue;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown seperation;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox valueList;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox prefix;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button generate;
    }
}