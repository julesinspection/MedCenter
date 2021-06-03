using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedCenter {
    public partial class @operator : Form {
        Клиент modelC = new Клиент();
        Запись modelZ = new Запись();
        public @operator()
        {
            InitializeComponent();
            PopulateDG();
            using (MedCenterEntities db = new MedCenterEntities()) {
                List<Клиент> client = db.Клиент.ToList();
                comboBox1.DataSource = client;
                comboBox1.DisplayMember = "ФИОклиента";
                comboBox1.ValueMember = "ID_Клиента";
                comboBox1.SelectedIndex = -1;

                List<Врач> doctor = db.Врач.ToList();
                comboBox2.DataSource = doctor;
                comboBox2.DisplayMember = "ФИОврача";
                comboBox2.ValueMember = "ID_Врача";
                comboBox2.SelectedIndex = -1;
            }
            Clear();
        }
        void PopulateDG()
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                zapDG.DataSource = (from Запись in db.Запись
                                    join Клиент in db.Клиент on Запись.ID_Клиента equals Клиент.ID_Клиента
                                    join Врач in db.Врач on Запись.ID_Врача equals Врач.ID_Врача
                                    select new {
                                       НомерЗаписи = Запись.НомерЗаписи,
                                        Клиент = Клиент.ФИОклиента,
                                        Врач = Врач.ФИОврача,
                                        Дата = Запись.Дата
                                    }).ToList();
                cliDG.DataSource = db.Клиент.Select(x => new {
                    ID = x.ID_Клиента,
                    ФИО = x.ФИОклиента,
                    Телефон = x.ТелефонКлиента
                }).ToList();
            }
        }

        void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox6.Text = "";

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            button2.Text = "Сохранить";
            button3.Enabled = false;

            button6.Text = "Сохранить";
            button4.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                modelC.ФИОклиента = textBox2.Text;
                modelC.ТелефонКлиента = textBox3.Text;
                using (MedCenterEntities db = new MedCenterEntities()) {
                    if (button2.Text == "Сохранить") {
                        modelC.ID_Клиента = RGen.GenRandomString();
                        db.Клиент.Add(modelC);
                    } else {
                        modelC.ID_Клиента = Convert.ToInt32(textBox1.Text);
                        db.Entry(modelC).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                Clear();
                PopulateDG();
                MessageBox.Show("Данные сохранены");
            }
            catch (Exception) {
                MessageBox.Show("Некорректно введенные данные");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void cliDG_DoubleClick(object sender, EventArgs e)
        {
            if (cliDG.CurrentRow.Index != -1) {
                Clear();
                modelC.ID_Клиента = (int)cliDG.CurrentRow.Cells["ID"].Value;
                using (MedCenterEntities db = new MedCenterEntities()) {
                    modelC = db.Клиент.Where(x => x.ID_Клиента == modelC.ID_Клиента).FirstOrDefault();
                    textBox1.Text = modelC.ID_Клиента.ToString();
                    textBox2.Text = modelC.ФИОклиента;
                    textBox3.Text = modelC.ТелефонКлиента;

                }
                button2.Text = "Обновить";
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                using (MedCenterEntities db = new MedCenterEntities()) {
                    var entry = db.Entry(modelC);
                    if (entry.State == EntityState.Detached)
                        db.Клиент.Attach(modelC);
                    db.Клиент.Remove(modelC);
                    db.SaveChanges();
                    Clear();
                    PopulateDG();
                    MessageBox.Show("Запись удалена");
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                using (MedCenterEntities db = new MedCenterEntities()) {
                    var entry = db.Entry(modelZ);
                    if (entry.State == EntityState.Detached)
                        db.Запись.Attach(modelZ);
                    db.Запись.Remove(modelZ);
                    db.SaveChanges();
                    Clear();
                    PopulateDG();
                    MessageBox.Show("Запись удалена");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try {

                modelZ.Дата = dateTimePicker1.Value;
                
                using (MedCenterEntities db = new MedCenterEntities()) {
                    if (comboBox1.Text != "") {
                        var a = db.Клиент.Where(x => x.ФИОклиента == comboBox1.Text).Select(x => x.ID_Клиента).FirstOrDefault();
                        modelZ.ID_Клиента = a;
                    }
                    if (comboBox2.Text != "") {
                        var a = db.Врач.Where(x => x.ФИОврача == comboBox2.Text).Select(x => x.ID_Врача).FirstOrDefault();
                        modelZ.ID_Врача = a;
                    }

                    if (button6.Text == "Сохранить") {
                        modelZ.НомерЗаписи = RGen.GenRandomString();
                        db.Запись.Add(modelZ);
                    } else {
                        modelZ.НомерЗаписи = Convert.ToInt32(textBox6.Text);
                        db.Entry(modelZ).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                Clear();
                PopulateDG();
                MessageBox.Show("Данные сохранены");
            }
            catch (Exception) {
                MessageBox.Show("Некорректно введенные данные");
            }
        }

        private void zapDG_DoubleClick(object sender, EventArgs e)
        {
            if (zapDG.CurrentRow.Index != -1) {
                Clear();
                modelZ.НомерЗаписи = (int)zapDG.CurrentRow.Cells["НомерЗаписи"].Value;
                using (MedCenterEntities db = new MedCenterEntities()) {
                    modelZ = db.Запись.Where(x => x.НомерЗаписи == modelZ.НомерЗаписи).FirstOrDefault();
                    
                    textBox6.Text = modelZ.НомерЗаписи.ToString();
                    var a = db.Клиент.Where(x => x.ID_Клиента == modelZ.ID_Клиента).Select(x => x.ФИОклиента).FirstOrDefault();
                    comboBox1.Text = a;
                    var b = db.Врач.Where(x => x.ID_Врача == modelZ.ID_Врача).Select(x => x.ФИОврача).FirstOrDefault();
                    comboBox2.Text = b;
                    dateTimePicker1.Value = modelZ.Дата;
                }
                button6.Text = "Обновить";
                button4.Enabled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                zapDG.DataSource = (from Запись in db.Запись
                                    join Клиент in db.Клиент on Запись.ID_Клиента equals Клиент.ID_Клиента
                                    join Врач in db.Врач on Запись.ID_Врача equals Врач.ID_Врача
                                    select new {
                                        НомерЗаписи = Запись.НомерЗаписи,
                                        Клиент = Клиент.ФИОклиента,
                                        Врач = Врач.ФИОврача,
                                        Дата = Запись.Дата
                                    }).ToList();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    switch (comboBox3.SelectedIndex) {
                        case -1:
                        case 0:
                            int a = Convert.ToInt32(textBox4.Text);
                            zapDG.DataSource = (from Запись in db.Запись
                                                where Запись.НомерЗаписи == a
                                                join Клиент in db.Клиент on Запись.ID_Клиента equals Клиент.ID_Клиента
                                                join Врач in db.Врач on Запись.ID_Врача equals Врач.ID_Врача
                                                select new {
                                                    НомерЗаписи = Запись.НомерЗаписи,
                                                    Клиент = Клиент.ФИОклиента,
                                                    Врач = Врач.ФИОврача,
                                                    Дата = Запись.Дата
                                                }).ToList();
                            break;
                        case 1:
                            zapDG.DataSource = (from Запись in db.Запись
                                                join Клиент in db.Клиент on Запись.ID_Клиента equals Клиент.ID_Клиента
                                                where Клиент.ФИОклиента.Contains(textBox4.Text)
                                                join Врач in db.Врач on Запись.ID_Врача equals Врач.ID_Врача
                                                select new {
                                                    НомерЗаписи = Запись.НомерЗаписи,
                                                    Клиент = Клиент.ФИОклиента,
                                                    Врач = Врач.ФИОврача,
                                                    Дата = Запись.Дата
                                                }).ToList();
                            break;
                    }
                }
                catch (Exception) {
                    MessageBox.Show("Некорректно введенные данные. Повторите попытку");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                cliDG.DataSource = db.Клиент.Select(x => new {
                    ID = x.ID_Клиента,
                    ФИО = x.ФИОклиента,
                    Телефон = x.ТелефонКлиента
                }).ToList();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    switch (comboBox4.SelectedIndex) {
                        case -1:
                        case 0:
                            int a = Convert.ToInt32(textBox5.Text);
                            cliDG.DataSource = db.Клиент.Where(x => x.ID_Клиента == a).Select(x => new {
                                ID = x.ID_Клиента,
                                ФИО = x.ФИОклиента,
                                Телефон = x.ТелефонКлиента
                            }).ToList();
                            break;
                        case 1:
                            cliDG.DataSource = db.Клиент.Where(x => x.ФИОклиента.Contains(textBox5.Text)).Select(x => new {
                                ID = x.ID_Клиента,
                                ФИО = x.ФИОклиента,
                                Телефон = x.ТелефонКлиента
                            }).ToList();
                            break;
                    }
                }
                catch (Exception) {
                    MessageBox.Show("Некорректно введенные данные. Повторите попытку");
                }
            }
        }
    }
}
