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
    public partial class FormConnect : Form
    {
        private Form1 parent;
        public void setParent(Form1 parent)
        {
            this.parent = parent;
        }
        public FormConnect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string host = textBoxHost.Text;
            int port = Int32.Parse(textBoxPort.Text);
            string database = textBoxBase.Text;
            string user = textBoxUser.Text;
            string pass = textBoxPass.Text;
            NpgsqlConnection connection =
            parent.Connect(host, port, database, user, pass);
            parent.setConnection(connection);
            parent.FillDataGridViewPolicemen();
            this.Visible = false;
        }
    }
}
