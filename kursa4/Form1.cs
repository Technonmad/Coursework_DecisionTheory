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

namespace kursa4
{
    public partial class Form1 : Form
    {
        int x = 10;
        int y = 10;
        int val;
        int age;

        TextBox[,] mas;
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox1.Text != "" && comboBox1.SelectedItem != null && textBox3.Text != "")
            {
                try
                {
                    if (Convert.ToInt32(textBox1.Text) > 0 && Convert.ToInt32(textBox2.Text) > 0)
                    {
                        if (Convert.ToInt32(textBox2.Text) > Convert.ToInt32(textBox1.Text) && Convert.ToInt32(textBox3.Text) <= Convert.ToInt32(comboBox1.SelectedItem))
                        {
                            button2.Enabled = true;
                            button3.Enabled = true;
                            button1.Enabled = false;

                            val = Convert.ToInt32(comboBox1.Text);
                            mas = new TextBox[val + 2, 3];

                            for (int i = 0; i < val + 2; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    mas[i, j] = new TextBox();
                                    mas[i, j].Location = new Point(x + 50 * i, y + 50 * j);
                                    mas[i, j].Name = i + " " + j;
                                    mas[i, j].Size = new Size(40, 20);
                                    panel1.Controls.Add(mas[i, j]);
                                    mas[0, 0].Enabled = false;

                                    for (int f = 1; f < val + 2; f++)
                                    {
                                        if (j == 0 && i == f)
                                        {
                                            mas[i, j].Text = Convert.ToString(i - 1);
                                            mas[i, j].Enabled = false;
                                        }
                                    }
                                }
                            }
                            mas[0, 1].Text = "R ( t )";
                            mas[0, 2].Text = "r ( t )";
                            mas[0, 1].Enabled = false;
                            mas[0, 2].Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Стоимость покупки не может быть выше стоимости продажи");
                            textBox1.Clear();
                            textBox2.Clear();
                        } 
                    }
                    else MessageBox.Show("Введите целое неотрицательное число");
                }
                catch
                {
                    MessageBox.Show("Введите число, а не что вы там ввели");
                }
                
            }
            else MessageBox.Show("Вы ввели не все значения");          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            button1.Enabled = true;
            button3.Enabled = false;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                listBox1.Items.Clear();

                int check = 0;


                val = Convert.ToInt32(comboBox1.Text);

                for (int i = 1; i < val + 2; i++)
                {
                    for (int j = 1; j < 3; j++)
                    {
                        if (mas[i, j].Text == "") check = 1;
                    }
                }

                if (check != 1)
                {
                    for (int i = 0; i <= val; i++)
                    {
                        dataGridView1.Columns.Add("S" + (i), "S" + (i));
                    }

                    dataGridView1.Rows.Add(val - 1);

                    for (int i = 0; i < val; i++)
                    {
                        dataGridView1.Rows[i].HeaderCell.Value = string.Format((i + 1).ToString(), "0");
                    }
                    dataGridView1.RowHeadersWidth = 50;

                    int izd = Convert.ToInt32(textBox1.Text) - Convert.ToInt32(textBox2.Text) + Convert.ToInt32(mas[1, 1].Text) - Convert.ToInt32(mas[1, 2].Text);

                    int[] razn = new int[val + 1];

                    for (int i = 1; i < val + 2; i++)
                    {
                        int prib = Convert.ToInt32(mas[i, 1].Text) - Convert.ToInt32(mas[i, 2].Text);
                        razn[i - 1] = prib;

                        if (prib.CompareTo(izd) > 0)
                        {
                            dataGridView1[i - 1, 0].Value = prib;
                            dataGridView1[i - 1, 0].Style.ForeColor = Color.Green;
                        }
                        if (prib.CompareTo(Math.Abs(izd)) < 0)
                        {
                            dataGridView1[i - 1, 0].Value = izd;
                            dataGridView1[i - 1, 0].Style.ForeColor = Color.Red;
                        }
                        if (prib.CompareTo(Math.Abs(izd)) == 0)
                        {
                            dataGridView1[i - 1, 0].Value = prib;
                            dataGridView1[i - 1, 0].Style.ForeColor = Color.Green;
                        }

                    }

                    int pribnext;

                    for (int i = 1; i < val; i++)
                    {


                        int izdnext = izd + Convert.ToInt32(dataGridView1[1, i - 1].Value);


                        for (int j = 0; j < val + 1; j++)
                        {
                            pribnext = razn[j] + Convert.ToInt32(dataGridView1[j + 1, i - 1].Value);

                            if (pribnext >= izdnext)
                            {
                                dataGridView1[j, i].Value = pribnext;
                                dataGridView1[j, i].Style.ForeColor = Color.Green;
                            }
                            else
                            {
                                dataGridView1[j, i].Value = Math.Abs(izdnext);
                                dataGridView1[j, i].Style.ForeColor = Color.Red;
                                break;
                            }
                        }
                    }

                    age = Convert.ToInt32(textBox3.Text);
                    int year;
                    int b;

                    if (dataGridView1[age, val - 1].Style.ForeColor == Color.Red || Convert.ToString(dataGridView1[age, val - 1].Value) == "")
                    {
                        listBox1.Items.Add(val + ")" + " " + "Замена");
                        b = 1;
                    }
                    else
                    {
                        listBox1.Items.Add(val + ")" + " " + "Сохранение");
                        b = age;
                    }

                    year = val - 1;

                    for (int j = val - 2; j >= 0; j--)
                    {
                        if (dataGridView1[b, j].Style.ForeColor == Color.Red)
                        {
                            listBox1.Items.Add((year) + ")" + " " + "Замена");
                            b = 1;
                            year--;
                        }
                        else
                        {
                            listBox1.Items.Add((year) + ")" + " " + "Сохранение");
                            year--;
                            b++;
                        }
                    }
                }
                else MessageBox.Show("Неправильно введены данные");
            }
            catch
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                MessageBox.Show("Введены некорректные данные");
            }
            
        }

        public void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Для начала выберите нужный вам временной промежуток в списке 'Интервал'." + "\n" +
                "Затем введите затраты на новое оборудование, остаточную стоимость(доход с продажи старого оборудования) и возраст вашего оборудования. Нажмите кнопку 'Готово'" + "\n" +
                "В появившейся таблице введите доход с продажи продукции, производимой на оборудовании, в год ( R(t) )" + "\n" +
                "и расходы, связанные с этим оборудованием (в год) ( r (t) )." + "\n" +
                "Затем нажмите на кнопку 'Решения'." + "\n" + "\n" +
                "! Обратите внимание, что вы не сможете изменить параметры таблицы пока не нажмете на кнопку 'Очистить' !" + "\n" + "\n" +
                "Выведется таблица с возрастом оборудования и доходом от него (строки - год из интервала, а столбцы - возраст оборудования)." + "\n" +
                "Также, рядом выведется оптимальная политика замены оборудования." + "\n" +
                "Таким образом вы сможете определить максимальную прибыль за каждый год вашего интервала, а также определить оптимальную политику замены оборудования.");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Спасибо за пользование моей программой!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Спасибо за пользование моей программой!");
        }
    }
}
