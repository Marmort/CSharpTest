﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;

namespace ExcelApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Methods
        public bool dataTotal(int col)
        {
            try
            {
                if (col == 2 || col == 3 || col == 4 || col == 5)
                {
                    dataGridView1.Sort(dataGridView1.Columns[col], ListSortDirection.Ascending);

                    int columnCount = dataGridView1.ColumnCount;
                    int rowCount = dataGridView1.RowCount;

                    string tempA = (string)dataGridView1.Rows[rowCount - 2].Cells[col].Value;
                    string tempB;
                    int total = 0;
                    for (int i = rowCount - 3; i >= 0; i--)
                    {
                        tempB = (string)dataGridView1.Rows[i].Cells[col].Value;
                        if (tempA.GetHashCode() == tempB.GetHashCode())
                        {
                            for (int j = 6; j < 12; j++)
                            {
                                if (j == 7) continue;
                                total = int.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString()) + int.Parse(dataGridView1.Rows[i + 1].Cells[j].Value.ToString());
                                dataGridView1.Rows[i].Cells[j].Value = total.ToString();
                            }
                            for (int j = 12; j < 50; j += 2)
                            {
                                if (j == 50) continue;
                                total = int.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString()) + int.Parse(dataGridView1.Rows[i + 1].Cells[j].Value.ToString());
                                dataGridView1.Rows[i].Cells[j].Value = total.ToString();
                            }
                            dataGridView1.Rows.RemoveAt(i + 1);
                        }
                        else
                        {
                            tempA = tempB;
                        }
                    }

                    columnCount = dataGridView1.ColumnCount;
                    rowCount = dataGridView1.RowCount;

                    for (int j = 0; j < rowCount - 1; j++)
                    {
                        int tempD = int.Parse(dataGridView1.Rows[j].Cells[6].Value.ToString());
                        int tempC = int.Parse(dataGridView1.Rows[j].Cells[12].Value.ToString());
                        if (tempD == 0)
                            dataGridView1.Rows[j].Cells[13].Value = "0";
                        else
                            dataGridView1.Rows[j].Cells[13].Value = (tempC * 10000 / tempD / 100.0).ToString();
                    }

                    for (int i = 15; i < 50; i += 2)
                    {
                        for (int j = 0; j < rowCount - 1; j++)
                        {
                            int tempD = int.Parse(dataGridView1.Rows[j].Cells[9].Value.ToString());
                            int tempC = int.Parse(dataGridView1.Rows[j].Cells[i - 1].Value.ToString());
                            if (tempD == 0)
                                dataGridView1.Rows[j].Cells[i].Value = "0";
                            else
                                dataGridView1.Rows[j].Cells[i].Value = (tempC * 10000 / tempD / 100.0).ToString();
                        }
                    }
                    return true;
                }

                if (col == 999)
                {
                    dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);

                    int columnCount = dataGridView1.ColumnCount;
                    int rowCount = dataGridView1.RowCount;

                    string tempA = (string)dataGridView1.Rows[rowCount - 2].Cells[0].Value;
                    string tempB;
                    int total = 0;
                    for (int i = rowCount - 3; i >= 0; i--)
                    {
                        tempB = (string)dataGridView1.Rows[i].Cells[0].Value;
                        if (tempA.GetHashCode() == tempB.GetHashCode())
                        {
                            for (int j = 4; j < 6; j++)
                            {
                                total = int.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString()) + int.Parse(dataGridView1.Rows[i + 1].Cells[j].Value.ToString());
                                dataGridView1.Rows[i].Cells[j].Value = total.ToString();
                            }
                            dataGridView1.Rows.RemoveAt(i + 1);
                        }
                        else
                        {
                            tempA = tempB;
                        }                    
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }        
        }

        #endregion

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel文件(*.xlsx)|*.xlsx|所有文件|*.*";
            DialogResult dlgResult = dlg.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                txtPath.Text = dlg.FileName;
                ExcelLib.IExcel tmp = ExcelLib.PreExcel.GetExcel(txtPath.Text);
                if (tmp == null & !tmp.Open())
                {
                    MessageBox.Show("File Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                comboBox1.DataSource = tmp.GetWorkSheets();
                tmp.Close();
                btnLoadData.Enabled = true;
                txtColumn.Text = "0";
            }
            else
            {
                return;
            }            
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
                MessageBox.Show("File Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelLib.IExcel tmp = ExcelLib.PreExcel.GetExcel(txtPath.Text);
                if (tmp == null & !tmp.Open())
                {
                    MessageBox.Show("File Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                tmp.CurrentSheetIndex = comboBox1.SelectedIndex;
                int columnCount = tmp.GetColumnCount();
                dataGridView1.ColumnCount = columnCount;
                int rowCount = tmp.GetRowCount() - 1;
                dataGridView1.RowCount = rowCount;
                int startRow = int.Parse(txtColumn.Text);
                
                for (int i = 0; i < columnCount; i++)
                {
                    dataGridView1.Columns[i].HeaderCell.Value = tmp.GetCellValue(startRow+1, i + 1);
                }

                for (int j = 0; j < rowCount-1; j++)
                {
                    for (int i = 0; i < columnCount; i++)
                    {
                        if (string.IsNullOrEmpty(tmp.GetCellValue(j + startRow + 2, i + 1)))
                        {
                            dataGridView1.Rows[j].Cells[i].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[j].Cells[i].Value = tmp.GetCellValue(j + startRow + 2, i + 1);
                        }
                    }
                }
                tmp.Close();
                                
                if (dataGridView1.Columns[1].HeaderCell.Value.GetHashCode() == "晶棒号".GetHashCode())
                {
                    string[] rowValue = new string[dataGridView1.RowCount];
                    for (int i = 0; i < rowCount - 1; i++)
                    {
                        rowValue[i] = (string)dataGridView1.Rows[i].Cells[1].Value;
                        rowValue[i] = rowValue[i].TrimStart('M');
                        dataGridView1.Rows[i].Cells[2].Value = (rowValue[i].Length > 6) ? rowValue[i].Substring(0, 6) : rowValue[i];
                    }
                    dataGridView1.Columns[2].HeaderCell.Value = "铸锭编号";
                }

                if (dataGridView1.Columns[3].HeaderCell.Value.GetHashCode() == "塑料盒号".GetHashCode())
                {
                    string[] rowValue = new string[dataGridView1.RowCount];
                    for (int i = 0; i < rowCount - 1; i++)
                    {
                        rowValue[i] = (string)dataGridView1.Rows[i].Cells[3].Value;
                        rowValue[i] = rowValue[i].TrimStart('M');
                        dataGridView1.Rows[i].Cells[0].Value = (rowValue[i].Length > 6) ? rowValue[i].Substring(0, 6) : rowValue[i];
                    }
                    dataGridView1.Columns[0].HeaderCell.Value = "晶锭编号";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnLoadData.Enabled = false;
        }
      
        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("http://epplus.codeplex.com/discussions");
        }
        
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelLib.IExcel tmp = ExcelLib.PreExcel.GetExcel(txtPath.Text);
                if (tmp == null)
                    MessageBox.Show("File Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

                string sheetName = comboBox1.SelectedItem.ToString();
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (sheetName.GetHashCode() == comboBox1.GetItemText(comboBox1.Items[i]).GetHashCode())
                    {
                        sheetName += "(2)";
                    }
                }                
                
                if (tmp.Save(sheetName, array))
                    MessageBox.Show("File Save Success!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tmp.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font, rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void removeColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int columnCount = dataGridView1.ColumnCount;
            int rowCount = dataGridView1.RowCount;
            
            if (columnCount > 50)
            {
                for (int i = columnCount - 1; i > 49; i--)
                {
                    switch (i)
                    {
                        case 55:
                        case 54:
                        case 53:
                            continue;
                            //break;
                    }
                    dataGridView1.Columns.RemoveAt(i);
                }

                columnCount = dataGridView1.ColumnCount;
                rowCount = dataGridView1.RowCount;
                string[] rowValue = new string[rowCount];

                for (int j = 0; j < rowCount-1; j++)
                {
                    int tempA = int.Parse(dataGridView1.Rows[j].Cells[6].Value.ToString());
                    int tempB = int.Parse(dataGridView1.Rows[j].Cells[12].Value.ToString());
                    if (tempA == 0)
                        dataGridView1.Rows[j].Cells[13].Value = "0";
                    else
                        dataGridView1.Rows[j].Cells[13].Value =  (tempB* 10000 / tempA/100.0).ToString();
                }
               
                for (int i = 15; i < 50; i += 2)
                {
                    for (int j = 0; j < rowCount-1; j++)
                    {
                        int tempA = int.Parse(dataGridView1.Rows[j].Cells[9].Value.ToString());
                        int tempB = int.Parse(dataGridView1.Rows[j].Cells[i - 1].Value.ToString());
                        if (tempA == 0)
                            dataGridView1.Rows[j].Cells[i].Value = "0";
                        else
                            dataGridView1.Rows[j].Cells[i].Value = (tempB * 10000 / tempA/100.0).ToString();
                    }
                }                
            }
            MessageBox.Show("Records Delete And Sort Success!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectCol = 3;
            if (dataTotal(selectCol))
                MessageBox.Show("Processed Data Success!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ingotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectCol = 2;
            if (dataTotal(selectCol))
                MessageBox.Show("Processed Data Success!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cuttingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectCol = 4;
            if (dataTotal(selectCol))
                MessageBox.Show("Processed Data Success!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cutNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectCol = 5;
            if (dataTotal(selectCol))
                MessageBox.Show("Processed Data Success!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoadData.Enabled = true;
            txtColumn.Text = "0";
        }

        private void txtColumn_TextChanged(object sender, EventArgs e)
        {
            btnLoadData.Enabled = true;
        }

        private void boxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectCol = 999;
            if (dataTotal(selectCol))
                MessageBox.Show("Processed Data Success!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }   

    }
}
