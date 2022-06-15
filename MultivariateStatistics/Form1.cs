using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MultivariateStatistics
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        string[] lines;
        string[] colnames;
        string[][] strValues;
        double[][] dblValues;
        string fileName;

        private static FrmMain instance = null;

        public static FrmMain Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        private void correlationCoefficientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                string q = "No data is loaded. Load data?";
                MessageBoxButtons b = MessageBoxButtons.YesNo;
                DialogResult r = MessageBox.Show(q, "Missing Data", b);
                if (r == DialogResult.Yes)
                    LoadData();
            }
            else
            {
                FrmCoef.Instance = new FrmCoef();
                FrmCoef.Instance.GetVList(colnames);
                FrmCoef.Instance.Show();
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            int c = e.ColumnIndex;

            //update dblValues from data grid
            dblValues[r][c] = Convert.ToDouble(dgvData.Rows[r].Cells[c].Value);

            //update originally read data lines
            lines[r+1] = dblValues[r][0].ToString();
            for (int i = 1; i < dblValues[r].Length; i++)
                lines[r+1] += "\t" + dblValues[r][i];
        }

        public void GetCoefficient(int i, int j)
        {
            double result = Mathtool.GetCoefficient(dblValues, i, j);
            txtStat.Text = "Correlation Coefficient between " + colnames[i] + " and " + colnames[j] + " is: " + result.ToString("#.##");
            tcStat.SelectedTab = tp2;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string q = "Do you want to save data?";
            MessageBoxButtons b = MessageBoxButtons.YesNo;
            DialogResult r = MessageBox.Show(q, "Save Data?", b);

            //write lines to the original text file
            if (r == DialogResult.Yes)
            {
                if (lines!=null)
                    File.WriteAllLines(fileName, lines);
            }
            Application.Exit();
        }

        public void LoadData()
        {
            OpenFileDialog mydialog = new OpenFileDialog();
            mydialog.InitialDirectory = @"c:\";
            mydialog.Filter = "Text Files (*.txt)|*.txt|All Files(*.*)|*.*";

            if (mydialog.ShowDialog() == DialogResult.OK)
            {
                fileName = mydialog.FileName;
                lines = File.ReadAllLines(fileName);

            }


            //use Linq to remove blank lines
            //lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //use list as intermediate step to remove blank lines
            List<string> temp = new List<string>();

            foreach (string x in lines)
            {
                if (!string.IsNullOrEmpty(x))
                    temp.Add(x);
            }
            lines = temp.ToArray();

            //get total lines in the file
            int h = lines.Length;

            //put invidual data item into strValues
            strValues = new string[h - 1][];
            //the first row makes up column names
            colnames = lines[0].Split('\t');

            foreach (string x in colnames)
                dgvData.Columns.Add(x, x);

            for (int i = 1; i < h; i++)
            {
                strValues[i - 1] = lines[i].Split('\t');
            }

            int w = colnames.Length;
            dblValues = new double[h - 1][];

            dgvData.ColumnCount = w;
            for (int i = 0; i < h - 1; i++)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvData);
                dblValues[i] = new double[w];
                for (int j = 0; j < strValues[i].Length; j++)
                {
                    r.Cells[j].Value = strValues[i][j];
                    dblValues[i][j] = Convert.ToDouble(strValues[i][j]);
                }
                dgvData.Rows.Add(r);

            }
            tcStat.SelectedTab = tp1;
        }
    }
}
