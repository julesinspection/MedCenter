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
    public partial class client : Form {
        public int id;
        public client(int id)
        {
            InitializeComponent();
            this.id = id;
            PopulateDG();
            using (MedCenterEntities db = new MedCenterEntities()) {
                List<Услуга> usl = db.Услуга.ToList();
                comboBox1.DataSource = usl;
                comboBox1.DisplayMember = "НазваниеУслуги";
                comboBox1.ValueMember = "НазваниеУслуги";
                comboBox1.SelectedIndex = -1;
            }
        }
        void PopulateDG()
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                uslDG.DataSource = db.Услуга.Select(x => new {
                    НазваниеУслуги = x.НазваниеУслуги,
                    Цена = x.Цена
                }).ToList();
                docDG.DataSource = db.Врач.Select(x => new {
                    ФИО = x.ФИОврача,
                    Телефон = x.ТелефонВрача,
                    Стаж = DateTime.Now.Year - x.Стаж,
                    Специальность = x.Специальность
                }).ToList();

                myDG.DataSource = (from Лечение in db.Лечение
                                   join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                   where Прием.ID_Клиента == id
                                   join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                   join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                   orderby Прием.Дата
                                   select new {
                                       Дата = Прием.Дата,
                                       Зуб = Зуб.НомерЗуба,
                                       Услуга = Услуга.НазваниеУслуги,
                                       Цена = Услуга.Цена,
                                       Состояне = Зуб.Состояние
                                   }).ToList();
            }
        }

        // Показать услуги
        private void button2_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                uslDG.DataSource = db.Услуга.Select(x => new {
                    НазваниеУслуги = x.НазваниеУслуги,
                    Цена = x.Цена
                }).ToList();
            }
        }
        // Поиск услуги
        private void button1_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    int Ot;
                    int Do;
                    if (textBox4.Text != "")
                        Ot = Convert.ToInt32(textBox4.Text);
                    else Ot = 0;
                    if (textBox5.Text != "")
                        Do = Convert.ToInt32(textBox5.Text);
                    else Do = Int32.MaxValue;
                    uslDG.DataSource = db.Услуга.Where(x => x.НазваниеУслуги.Contains(textBox1.Text) && x.Цена >= Ot && x.Цена <= Do).Select(x => new {
                        НазваниеУслуги = x.НазваниеУслуги,
                        Цена = x.Цена
                    }).ToList();
                }
                catch (Exception) {
                    MessageBox.Show("Некорректно введенные данные. Повторите попытку");
                }
            }
        }
        // Показать врачей
        private void button3_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                docDG.DataSource = db.Врач.Select(x => new {
                    ФИО = x.ФИОврача,
                    Телефон = x.ТелефонВрача,
                    Стаж = DateTime.Now.Year - x.Стаж,
                    Специальность = x.Специальность
                }).ToList();
            }
        }
        // Поиск врачей
        private void button4_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    int stage;
                    if (textBox6.Text != "")
                        stage = Convert.ToInt32(textBox6.Text);
                    else stage = 0;
                    docDG.DataSource = db.Врач.Where(x => x.ФИОврача.Contains(textBox2.Text) && (DateTime.Now.Year - x.Стаж) >= stage).Select(x => new {
                        ФИО = x.ФИОврача,
                        Телефон = x.ТелефонВрача,
                        Стаж = DateTime.Now.Year - x.Стаж,
                        Специальность = x.Специальность
                    }).ToList();
                }
                catch (Exception) {
                    MessageBox.Show("Некорректно введенные данные. Повторите попытку");
                }
            }
        }
        // Показать полученные
        private void button5_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                myDG.DataSource = (from Лечение in db.Лечение
                                   join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                   where Прием.ID_Клиента == id
                                   join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                   join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                   select new {
                                       НомерПриема = Лечение.НомерПриема,
                                       Зуб = Зуб.НомерЗуба,
                                       Услуга = Услуга.НазваниеУслуги,
                                       Цена = Услуга.Цена,
                                       Состояне = Зуб.Состояние
                                   }).ToList();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (MedCenterEntities db = new MedCenterEntities()) {
                try {
                    if (comboBox3.SelectedIndex == -1) {
                        
                        myDG.DataSource = (from Лечение in db.Лечение
                                           join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                           where Прием.ID_Клиента == id
                                           join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                           where Услуга.НазваниеУслуги.Contains(comboBox1.Text)
                                           join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                           select new {
                                               НомерПриема = Лечение.НомерПриема,
                                               Зуб = Зуб.НомерЗуба,
                                               Услуга = Услуга.НазваниеУслуги,
                                               Цена = Услуга.Цена,
                                               Состояне = Зуб.Состояние
                                           }).ToList();
                    } else if (comboBox3.SelectedIndex == 0) {
                        int a = Convert.ToInt32(textBox3.Text);
                        myDG.DataSource = (from Лечение in db.Лечение
                                           where Лечение.НомерПриема == a
                                           join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                           where Прием.ID_Клиента == id
                                           join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                           where Услуга.НазваниеУслуги.Contains(comboBox1.Text)
                                           join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                           select new {
                                               НомерПриема = Лечение.НомерПриема,
                                               Зуб = Зуб.НомерЗуба,
                                               Услуга = Услуга.НазваниеУслуги,
                                               Цена = Услуга.Цена,
                                               Состояне = Зуб.Состояние
                                           }).ToList();
                    } else if (comboBox3.SelectedIndex == 1) {
                        int a = Convert.ToInt32(textBox3.Text);
                        myDG.DataSource = (from Лечение in db.Лечение

                                           join Прием in db.Прием on Лечение.НомерПриема equals Прием.НомерПриема
                                           where Прием.ID_Клиента == id
                                           join Услуга in db.Услуга on Лечение.ID_Услуги equals Услуга.ID_Услуги
                                           where Услуга.НазваниеУслуги.Contains(comboBox1.Text)
                                           join Зуб in db.Зуб on Лечение.ID_Зуба equals Зуб.ID_Зуба
                                           where Зуб.НомерЗуба == a
                                           select new {
                                               НомерПриема = Лечение.НомерПриема,
                                               Зуб = Зуб.НомерЗуба,
                                               Услуга = Услуга.НазваниеУслуги,
                                               Цена = Услуга.Цена,
                                               Состояне = Зуб.Состояние
                                           }).ToList();
                    }
                }
                catch (Exception) {
                    MessageBox.Show("Некорректно введенные данные. Повторите попытку");
                }
            }
        }
    }
}

