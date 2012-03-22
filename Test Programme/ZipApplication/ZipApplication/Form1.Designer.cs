namespace ZipApplication
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnZipBroswer = new System.Windows.Forms.Button();
            this.btnZip = new System.Windows.Forms.Button();
            this.btnUzipBroswer = new System.Windows.Forms.Button();
            this.btnUnZip = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnZipBroswer
            // 
            this.btnZipBroswer.Location = new System.Drawing.Point(22, 92);
            this.btnZipBroswer.Name = "btnZipBroswer";
            this.btnZipBroswer.Size = new System.Drawing.Size(75, 23);
            this.btnZipBroswer.TabIndex = 0;
            this.btnZipBroswer.Text = "ZipBroswer";
            this.btnZipBroswer.UseVisualStyleBackColor = true;
            this.btnZipBroswer.Click += new System.EventHandler(this.btnZipBroswer_Click);
            // 
            // btnZip
            // 
            this.btnZip.Location = new System.Drawing.Point(187, 92);
            this.btnZip.Name = "btnZip";
            this.btnZip.Size = new System.Drawing.Size(75, 23);
            this.btnZip.TabIndex = 1;
            this.btnZip.Text = "Zip";
            this.btnZip.UseVisualStyleBackColor = true;
            this.btnZip.Click += new System.EventHandler(this.btnZip_Click);
            // 
            // btnUzipBroswer
            // 
            this.btnUzipBroswer.Location = new System.Drawing.Point(22, 175);
            this.btnUzipBroswer.Name = "btnUzipBroswer";
            this.btnUzipBroswer.Size = new System.Drawing.Size(75, 23);
            this.btnUzipBroswer.TabIndex = 2;
            this.btnUzipBroswer.Text = "UzipBroswer";
            this.btnUzipBroswer.UseVisualStyleBackColor = true;
            this.btnUzipBroswer.Click += new System.EventHandler(this.btnUzipBroswer_Click);
            // 
            // btnUnZip
            // 
            this.btnUnZip.Location = new System.Drawing.Point(187, 175);
            this.btnUnZip.Name = "btnUnZip";
            this.btnUnZip.Size = new System.Drawing.Size(75, 23);
            this.btnUnZip.TabIndex = 3;
            this.btnUnZip.Text = "UnZip";
            this.btnUnZip.UseVisualStyleBackColor = true;
            this.btnUnZip.Click += new System.EventHandler(this.btnUnZip_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(22, 65);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(240, 21);
            this.textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(22, 148);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(240, 21);
            this.textBox2.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnUnZip);
            this.Controls.Add(this.btnUzipBroswer);
            this.Controls.Add(this.btnZip);
            this.Controls.Add(this.btnZipBroswer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnZipBroswer;
        private System.Windows.Forms.Button btnZip;
        private System.Windows.Forms.Button btnUzipBroswer;
        private System.Windows.Forms.Button btnUnZip;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

