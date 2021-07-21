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
        private double walking;
        private double food;

        private string dayOne;
        private string dayEnd;


        private double summ;
        public Calc()
        {
            InitializeComponent();
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)//кол-во животных цифры только
        {
            char number = e.KeyChar;

            if(!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
    
        private double saleForm(double thisPrice, double daysSale)
        {
            thisPrice = thisPrice - System.Math.Ceiling((double)(thisPrice * (daysSale + constSale + formSite) / 100));
            return thisPrice;
        }

        private void button1_Click(object sender, EventArgs e)//сумма
        {
            if (checkBox1.Checked == true) //проверка чекбокса постоянного
            {
                constSale = 10;
            }
            else constSale = 0;

            if (checkBox2.Checked == true) //проверка чебокса скидки на сайте
            {
                formSite = 5;
            }
            else formSite = 0;

            if (checkBox3.Checked == true)//проверка чекбокса на натуральный корм
            {
                food = 1;
            }
            else food = 0;

            if (comboBox1.SelectedIndex == 0) //проверка выбранного бокса
            {
                price = 550;
            }
            else if (comboBox1.SelectedIndex == 1)
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
            else
            {
                price = 0;
            }

           
            dayOne = dateTimePicker1.Value.ToString("dd.MM.yyyy HH:mm");//дата заезда
            dayEnd = dateTimePicker2.Value.ToString("dd.MM.yyyy HH:mm");//дата выезда


            DateTime start = DateTime.ParseExact(dayOne, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(dayEnd, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            TimeSpan result = end - start; //период

            double days = result.TotalDays;
            double hours = result.TotalHours;

            double modHours = hours - (days * 24);
            MessageBox.Show("Столько дней: " + days.ToString());

            MessageBox.Show("Столько часов: " + hours.ToString());
            MessageBox.Show("Столько остаток часов от полных суток: " + modHours.ToString());

            if (comboBox1.SelectedIndex == -1)//алярмы на заполнение форм
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
            if (textBox2.Text.Length == 0) //проверка на дополнительные выгулы
            {
                walking = 0;
            }
            else
            {
                walking = Convert.ToDouble(textBox2.Text);
            }

            if (days >= 5 && days < 10) //скидки
            {
                price = saleForm(price, 10);

            } else if(days > 9 && days < 15)
            {
                price = saleForm(price, 15);

            } else if(days >= 15)
            {
                price = saleForm(price, 20);
            } else
            {
               price = saleForm(price, 0);
            }

            MessageBox.Show("Итого сумма за сутки " + price.ToString());

            summ = Math.Ceiling((price/24 * numOfAnimals * hours) + (walking * 250*days)+ (food*days));
            label11.Text = summ.ToString(); //по часам

            double sum2;
            double pricetwice = 0;

            if(modHours>0 && modHours<12)
            {
                pricetwice = price / 2;
                MessageBox.Show("добавляю эту сумму " + pricetwice.ToString() + "за столько оставшихся часов " + modHours.ToString());
            } 
            else if(modHours>=12)
            {
                pricetwice = price;
                MessageBox.Show("добавляю эту сумму" + pricetwice.ToString() + "за столько оставшихся часов " + modHours.ToString());
            }
            //MessageBox.Show((price * days * numOfAnimals).ToString());
            //MessageBox.Show((walking * 250 * days).ToString());
            //MessageBox.Show((food * days).ToString());
            sum2 = (price * Math.Round(days) * numOfAnimals) + pricetwice + walking * 250 * days + food * days;

            label14.Text = sum2.ToString();
        }


        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) //выгулы
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
    }
}
