namespace DDLCreator
{
    partial class Preview
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
            this.done = new System.Windows.Forms.Button();
            this.prev = new System.Windows.Forms.Button();
            this.next = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.pageLabel = new System.Windows.Forms.Label();
            this.trait1 = new System.Windows.Forms.Label();
            this.trait2 = new System.Windows.Forms.Label();
            this.trait3 = new System.Windows.Forms.Label();
            this.trait4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // done
            // 
            this.done.Location = new System.Drawing.Point(397, 311);
            this.done.Name = "done";
            this.done.Size = new System.Drawing.Size(75, 23);
            this.done.TabIndex = 0;
            this.done.Text = "Done";
            this.done.UseVisualStyleBackColor = true;
            this.done.Click += new System.EventHandler(this.done_Click);
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(12, 49);
            this.prev.Name = "prev";
            this.prev.Size = new System.Drawing.Size(40, 40);
            this.prev.TabIndex = 2;
            this.prev.Text = "<";
            this.prev.UseVisualStyleBackColor = true;
            this.prev.Click += new System.EventHandler(this.prev_Click);
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(432, 49);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(40, 40);
            this.next.TabIndex = 3;
            this.next.Text = ">";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.next_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.Control;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(58, 96);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(83, 173);
            this.listBox1.TabIndex = 4;
            // 
            // listBox2
            // 
            this.listBox2.BackColor = System.Drawing.SystemColors.Control;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(153, 96);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(83, 173);
            this.listBox2.TabIndex = 5;
            // 
            // listBox3
            // 
            this.listBox3.BackColor = System.Drawing.SystemColors.Control;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(248, 96);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(83, 173);
            this.listBox3.TabIndex = 6;
            // 
            // listBox4
            // 
            this.listBox4.BackColor = System.Drawing.SystemColors.Control;
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Location = new System.Drawing.Point(343, 95);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(83, 173);
            this.listBox4.TabIndex = 7;
            // 
            // pageLabel
            // 
            this.pageLabel.Location = new System.Drawing.Point(58, 19);
            this.pageLabel.Name = "pageLabel";
            this.pageLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pageLabel.Size = new System.Drawing.Size(368, 16);
            this.pageLabel.TabIndex = 8;
            this.pageLabel.Text = "Page 1";
            this.pageLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // trait1
            // 
            this.trait1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.trait1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.trait1.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trait1.Location = new System.Drawing.Point(58, 49);
            this.trait1.Name = "trait1";
            this.trait1.Size = new System.Drawing.Size(83, 40);
            this.trait1.TabIndex = 9;
            this.trait1.Text = "label1";
            this.trait1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trait2
            // 
            this.trait2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.trait2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.trait2.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trait2.Location = new System.Drawing.Point(153, 49);
            this.trait2.Name = "trait2";
            this.trait2.Size = new System.Drawing.Size(83, 40);
            this.trait2.TabIndex = 10;
            this.trait2.Text = "label1";
            this.trait2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trait3
            // 
            this.trait3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.trait3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.trait3.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trait3.Location = new System.Drawing.Point(248, 49);
            this.trait3.Name = "trait3";
            this.trait3.Size = new System.Drawing.Size(83, 40);
            this.trait3.TabIndex = 11;
            this.trait3.Text = "label1";
            this.trait3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trait4
            // 
            this.trait4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.trait4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.trait4.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trait4.Location = new System.Drawing.Point(343, 49);
            this.trait4.Name = "trait4";
            this.trait4.Size = new System.Drawing.Size(83, 40);
            this.trait4.TabIndex = 12;
            this.trait4.Text = "label1";
            this.trait4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Preview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 346);
            this.Controls.Add(this.trait4);
            this.Controls.Add(this.trait3);
            this.Controls.Add(this.trait2);
            this.Controls.Add(this.trait1);
            this.Controls.Add(this.pageLabel);
            this.Controls.Add(this.listBox4);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.next);
            this.Controls.Add(this.prev);
            this.Controls.Add(this.done);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Preview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preview";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button done;
        private System.Windows.Forms.Button prev;
        private System.Windows.Forms.Button next;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.Label pageLabel;
        private System.Windows.Forms.Label trait1;
        private System.Windows.Forms.Label trait2;
        private System.Windows.Forms.Label trait3;
        private System.Windows.Forms.Label trait4;
    }
}