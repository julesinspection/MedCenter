using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedCenter {
    public partial class login : Form {
        public login()
        {
            InitializeComponent();
            using (MedCenterEntities db = new MedCenterEntities()) {
                List<Клиент> clients = db.Клиент.ToList();
                comboBox1.DataSource = clients;
                comboBox1.DisplayMember = "ФИОклиента";
                comboBox1.ValueMember = "ID_Клиента";
                comboBox1.SelectedIndex = -1;
            }

            using (MedCenterEntities db = new MedCenterEntities()) {
                List<Врач> doctors = db.Врач.ToList();
                comboBox2.DataSource = doctors;
                comboBox2.DisplayMember = "ФИОврача";
                comboBox2.ValueMember = "ID_Врача";
                comboBox2.SelectedIndex = -1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "") {
                doctor form = new doctor(Convert.ToInt32(comboBox2.SelectedValue));
                form.Show();
            } else MessageBox.Show("Выберите пользователя");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "") {
                client form = new client(Convert.ToInt32(comboBox1.SelectedValue));
                form.Show();
            } else MessageBox.Show("Выберите пользователя");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            @operator form = new @operator();
            form.Show();
        }
    }
}
