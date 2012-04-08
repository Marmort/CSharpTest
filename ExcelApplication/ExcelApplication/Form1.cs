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
            btnLoadData.Enabled = true;
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
                ExcelLib.IExcel tmp = ExcelLib.PreExcel.GetExcel(txtPath.Text);
                if (tmp == null)
                    MessageBox.Show("文件不存在");
                if (!tmp.Open())
                    MessageBox.Show("文件不能打开");

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
            btnLoadData.Enabled = false;
        }
      
        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("http://epplus.codeplex.com/discussions");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoadData.Enabled = true;
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelLib.IExcel tmp = ExcelLib.PreExcel.GetExcel(txtPath.Text);
                if (tmp == null)
                    MessageBox.Show("文件不存在");
                tmp.CurrentSheetIndex = tmp.SheetCount;

                int columnCount = dataGridView1.ColumnCount;
                int rowCount = dataGridView1.RowCount;

                string[,] array = new string[rowCount, columnCount];

                for (int i = 0; i < columnCount; i++)
                {
                    array[0, i] = (string)dataGridView1.Columns[i].HeaderCell.Value;
                }

                for (int i = 1; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        array[i, j] = (string)dataGridView1.Rows[i - 1].Cells[j].Value;
                    }
                }

                string sheetName = "";
                if (tmp.Save(sheetName, array))
                    MessageBox.Show("文件保存成功");
                tmp.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void columnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int columnCount = dataGridView1.ColumnCount;
            
            if (columnCount != 66)
                return;

            dataGridView1.Columns.RemoveAt(65);
            dataGridView1.Columns.RemoveAt(64);
            dataGridView1.Columns.RemoveAt(63);
            dataGridView1.Columns.RemoveAt(62);
            dataGridView1.Columns.RemoveAt(61);
            dataGridView1.Columns.RemoveAt(60);
            dataGridView1.Columns.RemoveAt(59);
            dataGridView1.Columns.RemoveAt(58);
            dataGridView1.Columns.RemoveAt(57);
            dataGridView1.Columns.RemoveAt(56);
            dataGridView1.Columns.RemoveAt(55);
            dataGridView1.Columns.RemoveAt(54);
            dataGridView1.Columns.RemoveAt(53);
            dataGridView1.Columns.RemoveAt(52);
            dataGridView1.Columns.RemoveAt(51);
            dataGridView1.Columns.RemoveAt(50);
            dataGridView1.Columns.RemoveAt(49);
            
            dataGridView1.Columns.RemoveAt(47);
            dataGridView1.Columns.RemoveAt(45);
            dataGridView1.Columns.RemoveAt(43);
            dataGridView1.Columns.RemoveAt(41);
            dataGridView1.Columns.RemoveAt(39);
            dataGridView1.Columns.RemoveAt(37);
            dataGridView1.Columns.RemoveAt(35);
            dataGridView1.Columns.RemoveAt(33);
            dataGridView1.Columns.RemoveAt(31);
            dataGridView1.Columns.RemoveAt(29);
            dataGridView1.Columns.RemoveAt(27);
            dataGridView1.Columns.RemoveAt(25);
            dataGridView1.Columns.RemoveAt(23);
            dataGridView1.Columns.RemoveAt(21);
            dataGridView1.Columns.RemoveAt(19);
            dataGridView1.Columns.RemoveAt(17);
            dataGridView1.Columns.RemoveAt(15);
            dataGridView1.Columns.RemoveAt(13);

            dataGridView1.Columns.RemoveAt(7);
            dataGridView1.Columns.RemoveAt(5);
            dataGridView1.Columns.RemoveAt(2);
        }

    }
}
