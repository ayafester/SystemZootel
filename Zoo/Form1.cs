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
        public static string dbFileName;
        public static SQLiteConnection m_dbConn;
        public static SQLiteCommand m_sqlCmd;

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
            todayMonth = DateTime.Today.ToString("MM");
            todayYear = DateTime.Today.ToString("yyyy");
            lastDay = DateTime.Today.AddDays(-1).ToString("dd.MM.yyyy");

            m_dbConn = new SQLiteConnection(); //переменные для подключения к БД
            m_sqlCmd = new SQLiteCommand();

            dbFileName = "zootel.sqlite";//название БД

            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;"); //создаем с
                m_dbConn.Open();//открываем связь
                m_sqlCmd.Connection = m_dbConn;//создаем команду
                m_dbConn.Close();
                label1.Text = "дб создана";
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
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
