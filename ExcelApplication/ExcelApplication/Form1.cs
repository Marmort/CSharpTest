using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using OfficeOpenXml;

namespace ExcelApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取当前工作簿的所有表名称
        /// </summary>
        /// <param name="path">当前工作簿路径</param>
        /// <returns>所有表名称</returns>
        public static List<string> GetSheetNames(string path)
        {
            List<string> sheets = new List<string>();
            string connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""", path);
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            System.Data.DataTable tbl = connection.GetSchema("Tables");
            connection.Close();
            foreach (DataRow row in tbl.Rows)
            {
                string sheetName = (string)row["TABLE_NAME"];
                if (sheetName.EndsWith("$"))
                {
                    sheetName = sheetName.Substring(0, sheetName.Length - 1);
                }
                sheets.Add(sheetName);
            }
            return sheets;
        }      

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult dlgResult = dlg.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                txtPath.Text = dlg.FileName;
            }
            else
            {
                return;
            }
            comboBox1.DataSource = GetSheetNames(txtPath.Text);
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process Proc;

            try
            {
                Proc = System.Diagnostics.Process.Start(@"c:\windows\system32\calc.exe");
            }
            catch
            {
                MessageBox.Show("File Not Found!", "Error");
                return;
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelLib.IExcel tmp = ExcelLib.ExcelLib.GetExcel(txtPath.Text);
                if (tmp == null)
                    MessageBox.Show("打开文件错误");
                if (!tmp.Open())
                    MessageBox.Show("打开文件错误");

                tmp.CurrentSheetIndex = comboBox1.SelectedIndex;
                int columnCount = tmp.GetColumnCount();
                dataGridView1.ColumnCount = columnCount;
                int rowCount = tmp.GetRowCount();
                dataGridView1.RowCount = rowCount;

                for (int i = 0; i < columnCount; i++)
                {
                    dataGridView1.Columns[i].HeaderCell.Value = tmp.GetCellValue(1, i + 1);
                }

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = tmp.GetCellValue(i + 2, j + 1);
                    }
                }

                tmp.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }            
        }
             
    }
}
