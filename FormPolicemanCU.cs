using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetProj
{
    public partial class FormPolicemanCU : Form
    {
        private Form1 parent = null;
        private int row;
        private bool isAdd;

        DataTable departmentCollection;
        DataTable rankCollection;
        DataTable sexCollection;
        public FormPolicemanCU()
        {
            InitializeComponent();
        }

        public void setParent(Form1 parent)
        {
            this.parent = parent;
        }
        public void setRow(int row)
        {
            this.row = row;
        }
        public void setLabels()
        {
            if (isAdd)
            {
                button1.Text = "Add policeman";
            }
            else
            {
                button1.Text = "Update policeman";
            }
        }
        public void setIsAdd(bool isAdd)
        {
            this.isAdd = isAdd;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isAdd)
            {
                string sexString = comboBoxSex.SelectedItem.ToString();
                sexString = sexString.Substring(0, sexString.IndexOf(")"));

                string departmentString = comboBoxDepartment.SelectedItem.ToString();
                departmentString = departmentString.Substring(0, departmentString.IndexOf(")"));

                string rankString = comboBoxRank.SelectedItem.ToString();
                rankString = rankString.Substring(0, rankString.IndexOf(")"));

                parent.AddPoliceman(textBoxFName.Text, textBoxLName.Text, Convert.ToInt16(textBoxAge.Text), Convert.ToInt32(sexString), Convert.ToInt32(departmentString), Convert.ToInt32(rankString));
                parent.FillDataGridViewPolicemen();
                this.Visible = false;
            }
            else
            {
                string sexString = comboBoxSex.SelectedItem.ToString();
                sexString = sexString.Substring(0, sexString.IndexOf(")"));

                string departmentString = comboBoxDepartment.SelectedItem.ToString();
                departmentString = departmentString.Substring(0, departmentString.IndexOf(")"));

                string rankString = comboBoxRank.SelectedItem.ToString();
                rankString = rankString.Substring(0, rankString.IndexOf(")"));

                parent.UpdatePoliceman(row, textBoxFName.Text, textBoxLName.Text, Convert.ToInt16(textBoxAge.Text), Convert.ToInt32(sexString), Convert.ToInt32(departmentString), Convert.ToInt32(rankString)); ;
                parent.FillDataGridViewPolicemen();
                this.Visible = false;
            }
        }

        public void setTextBoxFName(string text)
        {
            textBoxFName.Text = text;
        }
        public void setTextBoxLName(string text)
        {
            textBoxLName.Text = text;
        }
        public void setTextBoxAge(string text)
        {
            textBoxAge.Text = text;
        }
        public void setComboBoxSex(string text)
        {
            comboBoxSex.Text = text;
        }
        public void setComboBoxDepartment(string department) {
            comboBoxDepartment.Text = department;
        }
        public void setComboBoxRank(string rank)
        {
            comboBoxRank.Text = rank;
        }
        public void setSexCollection(DataTable sexCollection)
        {
            this.sexCollection = sexCollection;
        }
        public void setDepartmentCollection(DataTable departmentCollection)
        {
            this.departmentCollection = departmentCollection;
        }
        public void setRankCollection(DataTable rankCollection)
        {
            this.rankCollection = rankCollection;
        }
        public void initializeComboBoxes()
        {

            foreach (DataRow row in departmentCollection.Rows)
            {
                comboBoxDepartment.Items.Add(row[0] + ")" + row[1] + "," + row[2]);
            }
            foreach (DataRow row in sexCollection.Rows)
            {
                comboBoxSex.Items.Add(row[0] + ")" + row[1]);
            }
            foreach (DataRow row in rankCollection.Rows)
            {
                comboBoxRank.Items.Add(row[0] + ")" + row[1]);
            }
            if (!isAdd)
            {
                comboBoxDepartment.SelectedIndex = comboBoxDepartment.FindString(comboBoxDepartment.Text.ToString());
                comboBoxSex.SelectedIndex = comboBoxSex.FindString(comboBoxSex.Text.ToString());
                comboBoxRank.SelectedIndex = comboBoxRank.FindString(comboBoxRank.Text.ToString());
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (isAdd)
            {
                string sexString = comboBoxSex.SelectedItem.ToString();
                sexString = sexString.Substring(0, sexString.IndexOf(")"));

                string departmentString = comboBoxDepartment.SelectedItem.ToString();
                departmentString = departmentString.Substring(0, departmentString.IndexOf(")"));

                string rankString = comboBoxRank.SelectedItem.ToString();
                rankString = rankString.Substring(0, rankString.IndexOf(")"));

                parent.AddPoliceman(textBoxFName.Text, textBoxLName.Text, Convert.ToInt16(textBoxAge.Text), Convert.ToInt32(sexString), Convert.ToInt32(departmentString), Convert.ToInt32(rankString));
                parent.FillDataGridViewPolicemen();
                this.Visible = false;
            }
            else
            {
                string sexString = comboBoxSex.SelectedItem.ToString();
                sexString = sexString.Substring(0, sexString.IndexOf(")"));

                string departmentString = comboBoxDepartment.SelectedItem.ToString();
                departmentString = departmentString.Substring(0, departmentString.IndexOf(")"));

                string rankString = comboBoxRank.SelectedItem.ToString();
                rankString = rankString.Substring(0, rankString.IndexOf(")"));

                parent.UpdatePoliceman(row, textBoxFName.Text, textBoxLName.Text, Convert.ToInt16(textBoxAge.Text), Convert.ToInt32(sexString), Convert.ToInt32(departmentString), Convert.ToInt32(rankString)) ;
                parent.FillDataGridViewPolicemen();
                this.Visible = false;
            }
        }
    }
}
