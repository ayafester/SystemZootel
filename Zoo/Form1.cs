using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;

namespace Zoo
{
    public partial class Form1 : Form
    {
        public static string lastDay;
        public static string todayDay;
        public static string todayMonth;
        public static string todayYear;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            todayDay = DateTime.Today.ToString("dd"); //сегодняшний день
            todayMonth = DateTime.Today.ToString("MM"); //сегодняшний месяц
            todayYear = DateTime.Today.ToString("yyyy"); //сегодняшний год
            lastDay = DateTime.Today.AddDays(-1).ToString("dd.MM.yyyy");//вчерашний день

            DB.Connect();
            DB.CreateTables();
        }

        private void calc_Button_Click(object sender, EventArgs e)
        {
            Calc newFormCalc = new Calc();
            newFormCalc.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cassa cassa = new Cassa();
            cassa.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Schedule sched = new Schedule();
            sched.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataBase db = new DataBase();
            db.Show();
        }
    }
}
