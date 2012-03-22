using System;
using System.Windows.Forms;

namespace ZipApplication
{
    public partial class Form1 : Form
    {
        string fullName = "";
        string destPath = "";
        ZipClass zp = new ZipClass();
        UnZipClass UZp = new UnZipClass(); 

        public Form1()
        {
            InitializeComponent();
        }
       
        private void btnZipBroswer_Click(object sender, EventArgs e)
        {
            //待压缩文件  
            folderBrowserDialog1.ShowDialog();
            fullName = folderBrowserDialog1.SelectedPath;
            textBox1.Text = fullName;
            //压缩到的路径  
            destPath = System.IO.Path.GetDirectoryName(fullName);
            //压缩后的目标文件  
            destPath = destPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullName) + ".zip";  
        }

        private void btnUzipBroswer_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            fullName = openFileDialog1.FileName;
            textBox2.Text = fullName;
            //解压到的路径  
            destPath = System.IO.Path.GetDirectoryName(fullName);  
        }

        private void btnZip_Click(object sender, EventArgs e)
        {
            zp.Zip(fullName, destPath);
            MessageBox.Show("压缩成功！"); 
        }

        private void btnUnZip_Click(object sender, EventArgs e)
        {
            UZp.UnZip(fullName, destPath);            
            MessageBox.Show("解压成功！"); 
        }  
    }
}
