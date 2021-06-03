using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace MedCenter {
    
    public partial class doctor : Form {

        public int id;
        Лечение modelLech = new Лечение();
        Зуб modelZub = new Зуб();
        Прием modelP = new Прием();
        public doctor(int id)
        {
            InitializeComponent();
            this.id = id;
            Clear();
        }
        private void doctor_Load(object sender, EventArgs e)
        {
            PopulateDG();
        }

        void Clear()
        {
            textBox5.Text = "";
            textBox3.Text = "";
            comboBox5.SelectedIndex = -1;
            button9.Text = "Сохранить";
            button7.Enabled = false;

            textBox2.Text = "";
            textBox4.Text = "";
            comboBox6.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            button6.Text = "Сохранить";
            button4.Enabled = false;

            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.Text = "";
            button2.Text = "Сохранить";
            button3.Enabled = false;
        }
        void PopulateDG()
        {
            using(MedCenterEntities db = new MedCenterEntities()) {
                zapDGV.DataSource = (from Запись in db.Запись
                                     where Запись.Дата >= DateTime.Now
                                     where Запись.ID_Врача == id
                                      join Клиент in db.Клиент on Запись.ID_Клиента equals Клиент.ID_Клиента
                                      orderby Запись.Дата
                                     select new {
                                          НомерЗаписи = Запись.НомерЗаписи,
                                          Клиент = Клиент.ФИОклиента,
                                          Дата = Запись.Дата
                                      }).ToList();

                lechDG.DataSource = (from Лечение in db.Лечение
                                      join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                      where Прием.ID_Врача == id
                                      join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                      join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                     join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента
                                     select new {
                                          Лечение = Лечение.ID_Лечения,
                                          НомерПриема = Лечение.НомерПриема,
                                          Услуга = Услуга.НазваниеУслуги,
                                          Клиент = Клиент.ФИОклиента,
                                          Зуб = Зуб.НомерЗуба
                                      }).ToList();
                priDG.DataSource = (from Прием in db.Прием
                                    where Прием.ID_Врача == id
                                    join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента
                                    select new {
                                        НомерПриема = Прием.НомерПриема,
                                        Клиент = Клиент.ФИОклиента,
                                        Дата = Прием.Дата,
                                        Заключение = Прием.Заключение
                                    }).ToList();
                zubDG.DataSource = (from Зуб in db.Зуб
                                    join Клиент in db.Клиент on Зуб.ID_Клиента equals Клиент.ID_Клиента
                                    select new {
                                        ID = Зуб.ID_Зуба,
                                        Номер = Зуб.НомерЗуба,
                                        Клиент = Клиент.ФИОклиента,
                                        Состояние = Зуб.Состояние,
                                    }).ToList();

                List<Прием> priem = db.Прием.Where(x => x.ID_Врача == id).ToList();
                comboBox1.DataSource = priem;
                comboBox1.DisplayMember = "НомерПриема";
                comboBox1.ValueMember = "НомерПриема";
                comboBox1.SelectedIndex = -1;

                List<Услуга> usl = db.Услуга.ToList();
                comboBox2.DataSource = usl;
                comboBox2.DisplayMember = "НазваниеУслуги";
                comboBox2.ValueMember = "НазваниеУслуги";
                comboBox2.SelectedIndex = -1;

                List<Клиент> client = db.Клиент.ToList();
                comboBox5.DataSource = client;
                comboBox5.DisplayMember = "ФИОклиента";
                comboBox5.ValueMember = "ID_Клиента";
                comboBox5.SelectedIndex = -1;

                comboBox4.DataSource = client;
                comboBox4.DisplayMember = "ФИОклиента";
                comboBox4.ValueMember = "ID_Клиента";
                comboBox4.SelectedIndex = -1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           try {
                modelLech.НомерПриема = Convert.ToInt32(comboBox1.Text);
                
                using (MedCenterEntities db = new MedCenterEntities()) {
                    if (comboBox2.Text != "") {
                        var a = db.Услуга.Where(x => x.НазваниеУслуги == comboBox2.Text).Select(x => x.ID_Услуги).FirstOrDefault();
                        modelLech.ID_Услуги = a;
                    }
                    int z = Convert.ToInt32(comboBox3.Text);
                    modelZub = db.Зуб.Where(x => x.НомерЗуба == z).FirstOrDefault();
                    modelLech.ID_Зуба = modelZub.ID_Зуба;


                    if (button2.Text == "Сохранить") {
                        modelLech.ID_Лечения = RGen.GenRandomString();
                        db.Лечение.Add(modelLech);
                    } else {
                        modelLech.ID_Лечения = Convert.ToInt32(textBox1.Text);
                        db.Entry(modelLech).State = EntityState.Modified;
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                using (MedCenterEntities db = new MedCenterEntities()) {
                    var entry = db.Entry(modelLech);
                    if (entry.State == EntityState.Detached)
                        db.Лечение.Attach(modelLech);
                    db.Лечение.Remove(modelLech);
                    db.SaveChanges();
                    Clear();
                    PopulateDG();
                    MessageBox.Show("Запись удалена");
                }
            }
        }

        private void lechDG_DoubleClick(object sender, EventArgs e)
        {
            if (lechDG.CurrentRow.Index != -1) {
                Clear();
                modelLech.ID_Лечения = (int)lechDG.CurrentRow.Cells["Лечение"].Value;
                using (MedCenterEntities db = new MedCenterEntities()) {
                    modelLech = db.Лечение.Where(x => x.ID_Лечения == modelLech.ID_Лечения).FirstOrDefault();
                    textBox1.Text = modelLech.ID_Лечения.ToString();
                    comboBox1.Text = modelLech.НомерПриема.ToString();
                    var a = db.Услуга.Where(x => x.ID_Услуги == modelLech.ID_Услуги).Select(x => x.НазваниеУслуги).FirstOrDefault();
                    comboBox2.Text = a;
                    //comboBox3.Text = db.Зуб.Where(x => x.НомерЗуба == Convert.ToInt32(modelLech.ID_Зуба)).FirstOrDefault();
                    comboBox3.Text = modelLech.ID_Зуба.ToString();
                    //modelZub = db.Зуб.Where(x => x.НомерЗуба == modelLech.ID_Зуба).FirstOrDefault();
                    //comboBox3.Text = modelZub.НомерЗуба.ToString();

                }
                button2.Text = "Обновить";
                button3.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try {
                if (comboBox4.Text != "") {
                    modelZub.НомерЗуба = Convert.ToInt32(comboBox6.Text);
                }

                if (textBox4.Text != "")
                    modelZub.Состояние = textBox4.Text;
                else modelZub.Состояние = null;
                using (MedCenterEntities db = new MedCenterEntities()) {
                    if (comboBox4.Text != "") {
                        var a = db.Клиент.Where(x => x.ФИОклиента == comboBox4.Text).Select(x => x.ID_Клиента).FirstOrDefault();
                        modelZub.ID_Клиента = a;
                    }


                    if (button6.Text == "Сохранить") {
                        modelZub.ID_Зуба = RGen.GenRandomString();
                        db.Зуб.Add(modelZub);
                    } else {
                        modelZub.ID_Зуба = Convert.ToInt32(textBox2.Text);
                        db.Entry(modelZub).State = EntityState.Modified;
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

        private void button9_Click(object sender, EventArgs e)
        {
            try {
                modelP.ID_Врача = id;
                if (textBox3.Text != "")
                    modelP.Заключение = textBox3.Text;
                else modelP.Заключение = null;

                using (MedCenterEntities db = new MedCenterEntities()) {
                    if (comboBox5.Text != "") {
                        var a = db.Клиент.Where(x => x.ФИОклиента == comboBox5.Text).Select(x => x.ID_Клиента).FirstOrDefault();
                        modelP.ID_Клиента = a;
                    }

                    if (button9.Text == "Сохранить") {
                        modelP.Дата = DateTime.Now;
                        modelP.НомерПриема = RGen.GenRandomString();
                        db.Прием.Add(modelP);
                    } else {
                        modelP.НомерПриема = Convert.ToInt32(textBox5.Text);
                        db.Entry(modelP).State = EntityState.Modified;
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

        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                using (MedCenterEntities db = new MedCenterEntities()) {
                    var entry = db.Entry(modelP);
                    if (entry.State == EntityState.Detached)
                        db.Прием.Attach(modelP);
                    db.Прием.Remove(modelP);
                    db.SaveChanges();
                    Clear();
                    PopulateDG();
                    MessageBox.Show("Запись удалена");
                }
            }
        }

        private void priDG_DoubleClick(object sender, EventArgs e)
        {
            if (priDG.CurrentRow.Index != -1) {
                Clear();
                modelP.НомерПриема = (int)priDG.CurrentRow.Cells["НомерПриема"].Value;
                using (MedCenterEntities db = new MedCenterEntities()) {
                    modelP = db.Прием.Where(x => x.НомерПриема == modelP.НомерПриема).FirstOrDefault();
                    textBox5.Text = modelP.НомерПриема.ToString();
                    var a = db.Клиент.Where(x => x.ID_Клиента == modelP.ID_Клиента).Select(x => x.ФИОклиента).FirstOrDefault();
                    comboBox5.Text = a;
                    textBox3.Text = modelP.Заключение;

                }
                button9.Text = "Обновить";
                button7.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                using (MedCenterEntities db = new MedCenterEntities()) {
                    var entry = db.Entry(modelZub);
                    if (entry.State == EntityState.Detached)
                        db.Зуб.Attach(modelZub);
                    db.Зуб.Remove(modelZub);
                    db.SaveChanges();
                    Clear();
                    PopulateDG();
                    MessageBox.Show("Запись удалена");
                }
            }
        }



        private void zubDG_DoubleClick(object sender, EventArgs e)
        {
            if (zubDG.CurrentRow.Index != -1) {
                Clear();
                modelZub.ID_Зуба = (int)zubDG.CurrentRow.Cells["ID"].Value;
                using (MedCenterEntities db = new MedCenterEntities()) {
                    modelZub = db.Зуб.Where(x => x.ID_Зуба == modelZub.ID_Зуба).FirstOrDefault();
                    textBox2.Text = modelZub.ID_Зуба.ToString();
                    var a = db.Клиент.Where(x => x.ID_Клиента == modelZub.ID_Клиента).Select(x => x.ФИОклиента).FirstOrDefault();
                    comboBox4.Text = a;
                    comboBox6.Text = modelZub.НомерЗуба.ToString();
                    textBox4.Text = modelZub.Состояние;

                }
                button6.Text = "Обновить";
                button4.Enabled = true;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                zapDGV.DataSource = (from Запись in db.Запись
                                     where Запись.Дата >= DateTime.Now
                                     where Запись.ID_Врача == id
                                     join Клиент in db.Клиент on Запись.ID_Клиента equals Клиент.ID_Клиента
                                     orderby Запись.Дата
                                     select new {
                                         НомерЗаписи = Запись.НомерЗаписи,
                                         Клиент = Клиент.ФИОклиента,
                                         Дата = Запись.Дата
                                     }).ToList();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    switch (comboBox7.SelectedIndex) {
                        case -1:
                        case 0:
                            int a = Convert.ToInt32(textBox6.Text);
                            zapDGV.DataSource = (from Запись in db.Запись
                                                 where Запись.НомерЗаписи == a
                                                 where Запись.Дата >= DateTime.Now
                                                 where Запись.ID_Врача == id
                                                 join Клиент in db.Клиент on Запись.ID_Клиента equals Клиент.ID_Клиента
                                                 orderby Запись.Дата
                                                 select new {
                                                     НомерЗаписи = Запись.НомерЗаписи,
                                                     Клиент = Клиент.ФИОклиента,
                                                     Дата = Запись.Дата
                                                 }).ToList();
                            break;
                        case 1:
                            zapDGV.DataSource = (from Запись in db.Запись
                                                join Клиент in db.Клиент on Запись.ID_Клиента equals Клиент.ID_Клиента
                                                where Клиент.ФИОклиента.Contains(textBox6.Text)
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

        private void button12_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                priDG.DataSource = (from Прием in db.Прием
                                    where Прием.ID_Врача == id
                                    join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента

                                    select new {
                                        НомерПриема = Прием.НомерПриема,
                                        Клиент = Клиент.ФИОклиента,
                                        Дата = Прием.Дата,
                                        Заключение = Прием.Заключение
                                    }).ToList();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    switch (comboBox8.SelectedIndex) {
                        case -1:
                        case 0:
                            int a = Convert.ToInt32(textBox7.Text);
                            priDG.DataSource = (from Прием in db.Прием
                                                where Прием.ID_Врача == id
                                                where Прием.НомерПриема == a
                                                join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента
                                                select new {
                                                    НомерПриема = Прием.НомерПриема,
                                                    Клиент = Клиент.ФИОклиента,
                                                    Дата = Прием.Дата,
                                                    Заключение = Прием.Заключение
                                                }).ToList();
                            break;
                        case 1:
                            priDG.DataSource = (from Прием in db.Прием
                                                where Прием.ID_Врача == id
                                                join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента
                                                where Клиент.ФИОклиента.Contains(textBox7.Text)
                                                select new {
                                                    НомерПриема = Прием.НомерПриема,
                                                    Клиент = Клиент.ФИОклиента,
                                                    Дата = Прием.Дата,
                                                    Заключение = Прием.Заключение
                                                }).ToList();
                            break;
                    }
                }
                catch (Exception) {
                    MessageBox.Show("Некорректно введенные данные. Повторите попытку");
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                zubDG.DataSource = (from Зуб in db.Зуб
                                    join Клиент in db.Клиент on Зуб.ID_Клиента equals Клиент.ID_Клиента
                                    select new {
                                        ID = Зуб.ID_Зуба,
                                        Номер = Зуб.НомерЗуба,
                                        Клиент = Клиент.ФИОклиента,
                                        Состояние = Зуб.Состояние,
                                    }).ToList();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    switch (comboBox9.SelectedIndex) {
                        case -1:
                        case 0:
                            int a = Convert.ToInt32(textBox8.Text);
                            zubDG.DataSource = (from Зуб in db.Зуб
                                                where Зуб.ID_Зуба == a
                                                join Клиент in db.Клиент on Зуб.ID_Клиента equals Клиент.ID_Клиента
                                                select new {
                                                    ID = Зуб.ID_Зуба,
                                                    Номер = Зуб.НомерЗуба,
                                                    Клиент = Клиент.ФИОклиента,
                                                    Состояние = Зуб.Состояние,
                                                }).ToList();
                            break;
                        case 1:
                            zubDG.DataSource = (from Зуб in db.Зуб

                                                join Клиент in db.Клиент on Зуб.ID_Клиента equals Клиент.ID_Клиента
                                                where Клиент.ФИОклиента.Contains(textBox8.Text)
                                                select new {
                                                    ID = Зуб.ID_Зуба,
                                                    Номер = Зуб.НомерЗуба,
                                                    Клиент = Клиент.ФИОклиента,
                                                    Состояние = Зуб.Состояние,
                                                }).ToList();
                            break;
                    }
                }
                catch (Exception) {
                    MessageBox.Show("Некорректно введенные данные. Повторите попытку");
                }
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                lechDG.DataSource = (from Лечение in db.Лечение
                                     join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                     where Прием.ID_Врача == id
                                     join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                     join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                     join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента
                                     select new {
                                         Лечение = Лечение.ID_Лечения,
                                         НомерПриема = Лечение.НомерПриема,
                                         Услуга = Услуга.НазваниеУслуги,
                                         Клиент = Клиент.ФИОклиента,
                                         Зуб = Зуб.НомерЗуба
                                     }).ToList();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    switch (comboBox10.SelectedIndex) {
                        case -1:
                        case 0:
                            int a = Convert.ToInt32(textBox9.Text);
                            lechDG.DataSource = (from Лечение in db.Лечение
                                                 where Лечение.ID_Лечения == a
                                                 join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                                 where Прием.ID_Врача == id
                                                 join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                                 join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                                 join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента
                                                 select new {
                                                     Лечение = Лечение.ID_Лечения,
                                                     НомерПриема = Лечение.НомерПриема,
                                                     Услуга = Услуга.НазваниеУслуги,
                                                     Клиент = Клиент.ФИОклиента,
                                                     Зуб = Зуб.НомерЗуба
                                                 }).ToList();
                            break;
                        case 1:
                            a = Convert.ToInt32(textBox9.Text);
                            lechDG.DataSource = (from Лечение in db.Лечение
                                                 join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                                 where Прием.ID_Врача == id && Прием.НомерПриема == a

                                                 join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                                 join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                                 join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента
                                                 select new {
                                                     Лечение = Лечение.ID_Лечения,
                                                     НомерПриема = Лечение.НомерПриема,
                                                     Услуга = Услуга.НазваниеУслуги,
                                                     Клиент = Клиент.ФИОклиента,
                                                     Зуб = Зуб.НомерЗуба
                                                 }).ToList();
                            break;
                        case 2:
                            lechDG.DataSource = (from Лечение in db.Лечение
                                                 join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                                 where Прием.ID_Врача == id
                                                 join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                                 join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                                 join Клиент in db.Клиент on Прием.ID_Клиента equals Клиент.ID_Клиента
                                                 where Клиент.ФИОклиента.Contains(textBox9.Text)
                                                 select new {
                                                     Лечение = Лечение.ID_Лечения,
                                                     НомерПриема = Лечение.НомерПриема,
                                                     Услуга = Услуга.НазваниеУслуги,
                                                     Клиент = Клиент.ФИОклиента,
                                                     Зуб = Зуб.НомерЗуба
                                                 }).ToList();
                            break;
                    }
                }
                catch (Exception) {
                    MessageBox.Show("Некорректно введенные данные. Повторите попытку");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                int a = 0;
                try {
                    if (comboBox1.Text != "")
                        a = Convert.ToInt32(comboBox1.Text);
                }
                catch (Exception) {

                }
                comboBox3.DataSource = (from Зуб in db.Зуб
                                        join Прием in db.Прием on Зуб.ID_Клиента equals Прием.ID_Клиента
                                        where Прием.НомерПриема == a
                                        select Зуб.НомерЗуба
                                   ).ToList();
            }
        }
    }
}
