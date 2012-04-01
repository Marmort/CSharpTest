using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;

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

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txtPath.Text))
            {
                string connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""", txtPath.Text);
                string query = String.Format("select * from [{0}$]", comboBox1.SelectedItem);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connectionString);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            else
            {
                MessageBox.Show("No File is Selected");
            }
        }       

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult dlgResult = dlg.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                txtPath.Text = dlg.FileName;
            }
            comboBox1.DataSource = GetSheetNames(txtPath.Text);
        }

        private void closeCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult dlgResult = dlg.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                txtPath.Text = dlg.FileName;
            }

            DialogResult ret = MessageBox.Show("Copy to " + txtPath.Text, "", MessageBoxButtons.OKCancel);
            if (ret == DialogResult.OK)
            {
                ApplicationClass ExcelApp = new ApplicationClass();
                ExcelApp.Application.Workbooks.Add(Type.Missing);
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    ExcelApp.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        ExcelApp.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                ExcelApp.ActiveWorkbook.SaveCopyAs(txtPath.Text);
                ExcelApp.ActiveWorkbook.Saved = true;
                ExcelApp.Quit();
                MessageBox.Show("Sheet is Saved!");
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
                MessageBox.Show("File Not Found!", "Error");
                return;
            }
        }

        private void addressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string address = @"http://www.dotneter.com/";
            MessageBox.Show(address);
        }

    }
}
