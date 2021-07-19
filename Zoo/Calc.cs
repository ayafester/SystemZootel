using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zoo
{
    public partial class Calc : Form
    {
        private double price;

        private double constSale;
        private double formSite;

        private double numOfAnimals;

        private string dayOne;
        private string dayEnd;

        private string timeStart;
        private string timeFinish;

        private double summ;
        public Calc()
        {
            InitializeComponent();
        }

        private void Calc_Load(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//выбор бокса
        {
            if(comboBox1.SelectedIndex == 0)
            {
                price = 550;
            } else if(comboBox1.SelectedIndex == 1)
            {
                price = 700;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                price = 1200;
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                price = 250;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)//кол-во животных цифры только
        {
            char number = e.KeyChar;

            if(!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
       

       
        private void checkBox1_CheckedChanged(object sender, EventArgs e)//постоянный
        {
            if (checkBox1.Checked == true)
            {
                constSale = 10;
            }
            else constSale = 0;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)//форма на сайте
        {
            if (checkBox2.Checked == true)
            {
                formSite = 5;
            }
            else formSite = 0;
        }
        

        private void button1_Click(object sender, EventArgs e)//сумма
        {
            
            timeStart = maskedTextBox1.Text;
            dayOne = dateTimePicker1.Value.ToString("dd.MM.yyyy");

            MessageBox.Show(timeStart);

            timeFinish = maskedTextBox2.Text;
            dayEnd = dateTimePicker2.Value.ToString("dd.MM.yyyy");

            dayOne = dayOne + " " + timeStart;
            MessageBox.Show(dayOne);
            dayEnd = dayEnd + " " + timeFinish;

            DateTime start = DateTime.ParseExact(dayOne, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(dayEnd, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            TimeSpan result = end - start;

            MessageBox.Show(result.ToString());
            if(comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите бокс");
            }
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите количество животных");
            } else
            {
                numOfAnimals = Convert.ToDouble(textBox1.Text);
            }


            summ = price * numOfAnimals;

            label11.Text = summ.ToString();
        }


        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if (maskedTextBox1.Text[0] > 2)
            {
               
            }
        }
    }
}
