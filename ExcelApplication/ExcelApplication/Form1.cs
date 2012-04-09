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

        public bool dataTotal(int col)
        {
            try
            {
                dataGridView1.Sort(dataGridView1.Columns[col], ListSortDirection.Ascending);

                int columnCount = dataGridView1.ColumnCount;
                int rowCount = dataGridView1.RowCount;

                int[,] tempArry = new int[rowCount - 1, columnCount - 5];

                for (int i = 0; i < rowCount - 1; i++)
                {
                    for (int j = 0; j < columnCount - 5; j++)
                    {
                        tempArry[i, j] = int.Parse(dataGridView1.Rows[i].Cells[j + 5].Value.ToString());
                    }
                }

                string tempA = (string)dataGridView1.Rows[rowCount - 2].Cells[col].Value;
                for (int i = rowCount - 3; i >= 0; i--)
                {
                    string tempB = (string)dataGridView1.Rows[i].Cells[col].Value;
                    if (tempA.GetHashCode() == tempB.GetHashCode())
                    {
                        for (int j = 5; j < columnCount; j++)
                        {
                            tempArry[i, j - 5] += tempArry[i + 1, j - 5];
                            dataGridView1.Rows[i].Cells[j].Value = tempArry[i, j - 5].ToString();
                        }
                        dataGridView1.Rows.RemoveAt(i + 1);
                    }
                    tempA = tempB;
                }
            }
            catch
            {
                return false;
            }
            return true;           
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

                string sheetName = "W" + comboBox1.SelectedItem.ToString();
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (sheetName.GetHashCode() == comboBox1.GetItemText(comboBox1.Items[i]).GetHashCode())
                    {
                        sheetName = "W" + sheetName;
                    }
                }                
                
                if (tmp.Save(sheetName, array))
                {
                    comboBox1.DataSource = GetSheetNames(txtPath.Text);
                    MessageBox.Show("文件保存成功");
                }
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

        private void removeColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int columnCount = dataGridView1.ColumnCount;

            if (columnCount != 66)
                return;

            for (int i = 65; i > 48; i--)
            {
                dataGridView1.Columns.RemoveAt(i);
            }
            for (int i = 47; i > 12; )
            {
                dataGridView1.Columns.RemoveAt(i);
                i -= 2;
            }
            dataGridView1.Columns.RemoveAt(7);
            dataGridView1.Columns.RemoveAt(5);

            int rowCount = dataGridView1.RowCount;
            string[] rowValue = new string[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                rowValue[i] = (string)dataGridView1.Rows[i].Cells[1].Value;
                rowValue[i] = rowValue[i].TrimStart('M');
                dataGridView1.Rows[i].Cells[2].Value  = (rowValue[i].Length > 6) ? rowValue[i].Substring(0, 6) : rowValue[i];
            }
            dataGridView1.Columns[2].HeaderCell.Value = "铸锭编号";
            MessageBox.Show("删除了百分比和人员记录，整理出铸锭编号");
        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectCol = 3;
            if (dataTotal(selectCol))
                MessageBox.Show("数据处理完毕");
        }

        private void ingotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectCol = 2;
            if (dataTotal(selectCol))
                MessageBox.Show("数据处理完毕");
        }

        private void cuttingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectCol = 4;
            if (dataTotal(selectCol))
                MessageBox.Show("数据处理完毕");
        }

    }
}
