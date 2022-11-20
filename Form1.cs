using Npgsql;
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
    public partial class Form1 : Form
    {
        private NpgsqlConnection connection = null;
        private DataSet dataSet = null;

        private NpgsqlDataAdapter policemanDataAdapter = null;
        private NpgsqlDataAdapter intruderDataAdapter = null;
        private NpgsqlDataAdapter sexDataAdapter = null;
        private NpgsqlDataAdapter departmentDataAdapter = null;
        private NpgsqlDataAdapter rankDataAdapter = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connection.Close();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConnect formConnect = getFormConnect();
            formConnect.Visible = true;
        }
        public NpgsqlConnection Connect(string host, int port, string database, string user, string pass)
        {
            NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
            stringBuilder.Host = host;
            stringBuilder.Port = port;
            stringBuilder.Database = database;
            stringBuilder.Username = user;
            stringBuilder.Password = pass;
            stringBuilder.Timeout = 30;
            NpgsqlConnection connection =
            new NpgsqlConnection(stringBuilder.ConnectionString);
            connection.Open();
            return connection;
        }
        public void FillDataGridViewPolicemen()
        {
            getDataSet().Tables["Policemen"].Clear();
            policemanDataAdapter = new NpgsqlDataAdapter("SELECT * FROM policeman", connection);
            new NpgsqlCommandBuilder(policemanDataAdapter);
            policemanDataAdapter.Fill(getDataSet(), "Policemen");
            getDataSet().Tables["Policemen"].PrimaryKey = new DataColumn[] { getDataSet().Tables["Policemen"].Columns["id"] };
            dataGridViewPolicemen.DataSource = getDataSet().Tables["Policemen"];
            if (getDataSet().Relations.Count == 0)
            {
                getDataSet().Tables["Intruders"].Clear();
            intruderDataAdapter = new NpgsqlDataAdapter(
            "SELECT * " +
            "FROM intruder ", connection);
            new NpgsqlCommandBuilder(intruderDataAdapter);
            intruderDataAdapter.Fill(getDataSet(), "Intruders");


                getDataSet().Relations.Add("IntrudersOfPoliceman", dataSet.Tables["Policemen"].Columns["id"], dataSet.Tables["Intruders"].Columns["policemanid"]);
            }

            dataGridViewPolicemen.Columns[4].Visible = false;
            dataGridViewPolicemen.Columns[5].Visible = false;
            dataGridViewPolicemen.Columns[6].Visible = false;

            if (dataGridViewPolicemen.Columns.Count == 7)
            {
                DataGridViewTextBoxColumn sexCol = new DataGridViewTextBoxColumn();
                sexCol.HeaderText = "sex";
                sexCol.ValueType = typeof(string);
                dataGridViewPolicemen.Columns.Add(sexCol);

                DataGridViewTextBoxColumn rankCol = new DataGridViewTextBoxColumn();
                rankCol.HeaderText = "Department";
                rankCol.ValueType = typeof(string);
                dataGridViewPolicemen.Columns.Add(rankCol);

                DataGridViewTextBoxColumn departmentCol = new DataGridViewTextBoxColumn();
                departmentCol.HeaderText = "Rank";
                departmentCol.ValueType = typeof(string);
                dataGridViewPolicemen.Columns.Add(departmentCol);
            }
            if (sexDataAdapter == null)
            {
                sexDataAdapter = new NpgsqlDataAdapter("SELECT * FROM sex", connection);
                new NpgsqlCommandBuilder(sexDataAdapter);
                sexDataAdapter.Fill(getDataSet(), "Sex");
            }
                for (int i = 0; i < dataGridViewPolicemen.RowCount; i++)
                {
                    foreach (DataRow row in getDataSet().Tables["Sex"].Rows)
                    {
                        if (Convert.ToInt32(row[0]) == Convert.ToInt32(dataGridViewPolicemen.Rows[i].Cells[4].Value))
                            dataGridViewPolicemen.Rows[i].Cells[7].Value = Convert.ToString(row[0]) + ") " + Convert.ToString(row[1]);
                    }
                }
            if (departmentDataAdapter == null)
            {
                departmentDataAdapter = new NpgsqlDataAdapter("SELECT * FROM department", connection);
                new NpgsqlCommandBuilder(departmentDataAdapter);
                departmentDataAdapter.Fill(getDataSet(), "Department");
            }
                for (int i = 0; i < dataGridViewPolicemen.RowCount; i++)
                {
                    foreach (DataRow row in getDataSet().Tables["Department"].Rows)
                    {
                        if (Convert.ToInt32(row[0]) == Convert.ToInt32(dataGridViewPolicemen.Rows[i].Cells[5].Value))
                            dataGridViewPolicemen.Rows[i].Cells[8].Value = Convert.ToString(row[0]) + ") " + Convert.ToString(row[1]) + ", " + Convert.ToString(row[2]);
                    }
                }

            if (rankDataAdapter == null)
            {
                rankDataAdapter = new NpgsqlDataAdapter("SELECT * FROM rank", connection);
                new NpgsqlCommandBuilder(rankDataAdapter);
                rankDataAdapter.Fill(getDataSet(), "Rank");
            }
                for (int i = 0; i < dataGridViewPolicemen.RowCount; i++)
                {
                    foreach (DataRow row in getDataSet().Tables["Rank"].Rows)
                    {
                        if (Convert.ToInt32(row[0]) == Convert.ToInt32(dataGridViewPolicemen.Rows[i].Cells[6].Value))
                            dataGridViewPolicemen.Rows[i].Cells[9].Value = Convert.ToString(row[0]) + ") " + Convert.ToString(row[1]);
                    }
                }
            
        }
        public void FillDataGridViewIntruders(object policemanid)
        {

            getDataSet().Tables["Intruders"].Clear();
            intruderDataAdapter = new NpgsqlDataAdapter(
            "SELECT * " +
            "FROM intruder ", connection);
            new NpgsqlCommandBuilder(intruderDataAdapter);
            intruderDataAdapter.Fill(getDataSet(), "Intruders");


            dataGridViewIntruders.Columns.Clear();
            DataRow[] dr = getDataSet().Tables["Policemen"].Rows.Find(policemanid).GetChildRows("IntrudersOfPoliceman");
            DataTable intrud = new DataTable();
            intrud = getDataSet().Tables["Intruders"].Clone();
            foreach (DataRow datarow in dr) {
                intrud.ImportRow(datarow);
            }
            

            dataGridViewIntruders.DataSource = intrud;
            if (dr != null)
            {
                dataGridViewIntruders.Columns[4].Visible = false;
                dataGridViewIntruders.Columns[7].Visible = false;
                if (dataGridViewIntruders.Columns.Count == 8)
                {
                    DataGridViewTextBoxColumn sexCol = new DataGridViewTextBoxColumn();
                    sexCol.HeaderText = "sex";
                    sexCol.ValueType = typeof(string);
                    dataGridViewIntruders.Columns.Add(sexCol);

                    DataGridViewTextBoxColumn policemanCol = new DataGridViewTextBoxColumn();
                    policemanCol.HeaderText = "policemanid";
                    policemanCol.ValueType = typeof(string);
                    dataGridViewIntruders.Columns.Add(policemanCol);
                }

                for (int i = 0; i < dataGridViewIntruders.RowCount; i++)
                {
                    foreach (DataRow row in getDataSet().Tables["Sex"].Rows)
                    {
                        if (Convert.ToInt32(row[0]) == Convert.ToInt32(dataGridViewIntruders.Rows[i].Cells[4].Value))
                            dataGridViewIntruders.Rows[i].Cells[8].Value = Convert.ToString(row[0]) + ") " + Convert.ToString(row[1]);
                    }
                }
                for (int i = 0; i < dataGridViewIntruders.RowCount; i++)
                {
                    foreach (DataRow row in getDataSet().Tables["Policemen"].Rows)
                    {
                        if (Convert.ToInt32(row[0]) == Convert.ToInt32(dataGridViewIntruders.Rows[i].Cells[7].Value))
                            dataGridViewIntruders.Rows[i].Cells[9].Value = Convert.ToString(row[0]) + ") " + Convert.ToString(row[1]) + " " + Convert.ToString(row[2]);
                    }
                }
            }
        }
        public void setConnection(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        private DataSet getDataSet()
        {
            if (dataSet == null)
            {
                dataSet = new DataSet();
                dataSet.Tables.Add("Policemen");
                dataSet.Tables.Add("Intruders");
                dataSet.Tables.Add("Sex");
                dataSet.Tables.Add("Department");
                dataSet.Tables.Add("Rank");
            }
            
            return dataSet;
        }

        public FormConnect getFormConnect()
        {
            FormConnect formConnect = new FormConnect();
            formConnect.setParent(this);
            return formConnect;
        }
        public FormIntruderCU getFormIntruderCU()
        {
            FormIntruderCU formIntruder = new FormIntruderCU();
            formIntruder.setParent(this);
            return formIntruder;
        }

        public FormPolicemanCU getFormPolicemanCU()
        {
            FormPolicemanCU formPoliceman = new FormPolicemanCU();
            formPoliceman.setParent(this);
            return formPoliceman;
        }
        private void dataGridViewPolicemen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*            int selectedRow = dataGridViewPolicemen.SelectedCells[0].RowIndex;
                        int key = Convert.ToInt32(dataGridViewPolicemen.Rows[selectedRow].Cells[0].Value);
                        FillDataGridViewIntruders(key);*/
            object selectedRow = dataGridViewPolicemen.SelectedCells[0].Value;
            FillDataGridViewIntruders(selectedRow);
        }
        public void AddIntruder(string firstName, string lastName, int age, long sexId, string description, string phone, long policemanId)
        {
            getDataSet().Tables["Intruders"].Rows.Add(0, firstName, lastName, age, sexId, description, phone, policemanId);
            intruderDataAdapter.Update(getDataSet(), "Intruders");
        }

        public void UpdateIntruder(int row, string firstName, string lastName, int age, long sexId, string description, string phone, long policemanId)
        {
            getDataSet().Tables["Intruders"].Rows[row]["fisrt_name"] = firstName;
            getDataSet().Tables["Intruders"].Rows[row]["last_name"] = lastName;
            getDataSet().Tables["Intruders"].Rows[row]["age"] = age;
            getDataSet().Tables["Intruders"].Rows[row]["sexid"] = sexId;
            getDataSet().Tables["Intruders"].Rows[row]["description"] = description;
            getDataSet().Tables["Intruders"].Rows[row]["phone"] = phone;
            getDataSet().Tables["Intruders"].Rows[row]["policemanid"] = policemanId;
            intruderDataAdapter.Update(getDataSet(), "Intruders");
        }

        public void AddPoliceman(string firstName, string lastName, int age, long sexId, long departmentId, long rankId)
        {
            getDataSet().Tables["Policemen"].Rows.Add(0, firstName, lastName, age, sexId, departmentId ,rankId);
            policemanDataAdapter.Update(getDataSet(), "Policemen");
        }

        public void UpdatePoliceman(int row, string firstName, string lastName, int age, long sexId, long departmentId, long rankId)
        {
            getDataSet().Tables["Policemen"].Rows[row]["first_name"] = firstName;
            getDataSet().Tables["Policemen"].Rows[row]["last_name"] = lastName;
            getDataSet().Tables["Policemen"].Rows[row]["age"] = age;
            getDataSet().Tables["Policemen"].Rows[row]["sexid"] = sexId;
            getDataSet().Tables["Policemen"].Rows[row]["departmentid"] = departmentId;
            getDataSet().Tables["Policemen"].Rows[row]["rankid"] = rankId;
            policemanDataAdapter.Update(getDataSet(), "Policemen");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPolicemanCU formPolicemanCU = getFormPolicemanCU();
            formPolicemanCU.Visible = true;
            formPolicemanCU.setTextBoxFName("");
            formPolicemanCU.setTextBoxLName("");
            formPolicemanCU.setComboBoxSex("");
            formPolicemanCU.setTextBoxAge("");
            formPolicemanCU.setComboBoxDepartment("");
            formPolicemanCU.setComboBoxRank("");
            formPolicemanCU.setIsAdd(true);

            DataTable departmentCollection = getDataSet().Tables["Department"];
            formPolicemanCU.setDepartmentCollection(departmentCollection);

            DataTable sexCollection = getDataSet().Tables["Sex"];
            formPolicemanCU.setSexCollection(sexCollection);

            DataTable rankCollection = getDataSet().Tables["Rank"];
            formPolicemanCU.setRankCollection(rankCollection);

            formPolicemanCU.initializeComboBoxes();
            formPolicemanCU.setLabels();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridViewPolicemen.SelectedCells[0].RowIndex;
            DialogResult dr = MessageBox.Show("Delete policeman?", "Deleting", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                getDataSet().Tables["Policemen"].Rows[selectedRow].Delete();
                policemanDataAdapter.Update(getDataSet(), "Policemen");
                FillDataGridViewPolicemen();
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridViewPolicemen.SelectedCells[0].RowIndex;
            string firstName = (string)getDataSet().Tables["Policemen"].Rows[selectedRow].ItemArray[1];
            string lastName = (string)getDataSet().Tables["Policemen"].Rows[selectedRow].ItemArray[2];
            string age = Convert.ToString((int)getDataSet().Tables["Policemen"].Rows[selectedRow].ItemArray[3]);
            string sexId = Convert.ToString((long)getDataSet().Tables["Policemen"].Rows[selectedRow].ItemArray[4]);
            string departmentId = Convert.ToString((long)getDataSet().Tables["Policemen"].Rows[selectedRow].ItemArray[5]);
            string rankId = Convert.ToString((long)getDataSet().Tables["Policemen"].Rows[selectedRow].ItemArray[6]);



            FormPolicemanCU formPolicemanCU = getFormPolicemanCU();
            formPolicemanCU.Visible = true;
            formPolicemanCU.setIsAdd(false);
            formPolicemanCU.setTextBoxFName(firstName);
            formPolicemanCU.setTextBoxLName(lastName);
            formPolicemanCU.setTextBoxAge(age);
            formPolicemanCU.setComboBoxSex(sexId);
            formPolicemanCU.setComboBoxDepartment(departmentId);
            formPolicemanCU.setComboBoxRank(rankId);

            DataTable departmentCollection = getDataSet().Tables["Department"];
            formPolicemanCU.setDepartmentCollection(departmentCollection);

            DataTable sexCollection = getDataSet().Tables["Sex"];
            formPolicemanCU.setSexCollection(sexCollection);

            DataTable rankCollection = getDataSet().Tables["Rank"];
            formPolicemanCU.setRankCollection(rankCollection);

            formPolicemanCU.initializeComboBoxes();
            formPolicemanCU.setLabels();
            formPolicemanCU.setRow(selectedRow);
        }

/*
        Buttons actions for Intruder

*/
        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormIntruderCU formIntruderCU = getFormIntruderCU();
            formIntruderCU.Visible = true;
            formIntruderCU.setTextBoxFName("");
            formIntruderCU.setTextBoxLName("");
            formIntruderCU.setComboBoxSex("");
            formIntruderCU.setTextBoxAge("");
            formIntruderCU.setTextBoxDescription("");
            formIntruderCU.setTextBoxPhone("");
            formIntruderCU.setIsAdd(true);

            DataTable policemenCollection = getDataSet().Tables["Policemen"];
            formIntruderCU.setPolicemenCollection(policemenCollection);
            DataTable sexCollection = getDataSet().Tables["Sex"];
            formIntruderCU.setSexCollection(sexCollection);
            formIntruderCU.initializeComboBoxes();
            formIntruderCU.setLabels();

            int selectedRow = dataGridViewPolicemen.SelectedCells[0].RowIndex;
            long policemanId = (long)getDataSet().Tables["Policemen"].Rows[selectedRow].ItemArray[0];
            formIntruderCU.setPolicemanId(policemanId);

           
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int selectedRowTemp = dataGridViewIntruders.SelectedCells[0].RowIndex;
            int key = Convert.ToInt32(dataGridViewIntruders.Rows[selectedRowTemp].Cells[0].Value);

            int selectedRow = 0;
            foreach (DataRow drow in getDataSet().Tables["Intruders"].Rows)
            {
                if (Convert.ToInt32(drow[0]) == key)
                {
                    break;
                }
                selectedRow++;
            }
            DialogResult dr = MessageBox.Show("Delete intruder?", "Deleting", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                int keyDel = Convert.ToInt32(dataGridViewIntruders.Rows[selectedRowTemp].Cells[7].Value);
                getDataSet().Tables["Intruders"].Rows[selectedRow].Delete();
                intruderDataAdapter.Update(getDataSet(), "Intruders");
                
                FillDataGridViewIntruders(keyDel);
            }
        }

        private void replaceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int selectedRowTemp = dataGridViewIntruders.SelectedCells[0].RowIndex;
            int key = Convert.ToInt32(dataGridViewIntruders.Rows[selectedRowTemp].Cells[0].Value);

            int selectedRow = 0;
            foreach(DataRow dr in getDataSet().Tables["Intruders"].Rows)
            {
                if (Convert.ToInt32(dr[0]) == key) {
                    break;
                }
                selectedRow++;
            }

            string firstName = (string)getDataSet().Tables["Intruders"].Rows[selectedRow].ItemArray[1];
            string lastName = (string)getDataSet().Tables["Intruders"].Rows[selectedRow].ItemArray[2];
            string age = Convert.ToString((int)getDataSet().Tables["Intruders"].Rows[selectedRow].ItemArray[3]);
            string sexId = Convert.ToString((long)getDataSet().Tables["Intruders"].Rows[selectedRow].ItemArray[4]);
            string description = (string)getDataSet().Tables["Intruders"].Rows[selectedRow].ItemArray[5];
            string phone = (string)getDataSet().Tables["Intruders"].Rows[selectedRow].ItemArray[6];
            string policemanidId = Convert.ToString((long)getDataSet().Tables["Intruders"].Rows[selectedRow].ItemArray[7]);
            
            FormIntruderCU formIntruderCU = getFormIntruderCU();
            formIntruderCU.Visible = true;
            formIntruderCU.setIsAdd(false);
            formIntruderCU.setTextBoxFName(firstName);
            formIntruderCU.setTextBoxLName(lastName);
            formIntruderCU.setTextBoxAge(age);
            formIntruderCU.setComboBoxSex(sexId);
            formIntruderCU.setComboBoxPoliceman(policemanidId);
            formIntruderCU.setTextBoxDescription(description);
            formIntruderCU.setTextBoxPhone(phone);

            DataTable policemenCollection = getDataSet().Tables["Policemen"];
            formIntruderCU.setPolicemenCollection(policemenCollection);
            DataTable sexCollection = getDataSet().Tables["Sex"];
            formIntruderCU.setSexCollection(sexCollection);

            formIntruderCU.initializeComboBoxes();
            formIntruderCU.setLabels();

            formIntruderCU.setRow(selectedRow);
            int selectedRow1 = dataGridViewPolicemen.SelectedCells[0].RowIndex;
            long policemanId = (long)getDataSet().Tables["Policemen"].Rows[selectedRow1].ItemArray[0];
            formIntruderCU.setPolicemanId(policemanId);
        }
    }

}
