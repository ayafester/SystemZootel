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


namespace Zoo
{
    public partial class Cassa : Form
    {
        private string dbFileName;
        private SQLiteConnection m_dbConn;
        private SQLiteCommand m_sqlCmd;

        public Cassa()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Cassa_Load(object sender, EventArgs e)
        {
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();

            dbFileName = "zootel.sqlite";
            lbStatusText.Text = "Disconnected";

            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);

            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Cassa (id integer PRIMARY KEY," + //это таблица берет данные из онлайн календаря и автоматически создает строку
                    "year text NOT NULL," + //год
                    "month text NOT NULL, " +//месяц
                    "day text NOT NULL, " +//день
                    "start text, " +//стартовая сумма
                    "cash text, " +//наличные
                    "notcash text, " +//безнал
                    "finish text, " +//конец дня
                    "summ text, " +//всего
                    "diff text, " +//разница начала и конца
                    "checksDay text" +//количество чеков
                    ")";
                m_sqlCmd.ExecuteNonQuery();
                m_dbConn.Close();

                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS OneDayCassa (id integer PRIMARY KEY," + //таблица доходов и расходов за день, прикручена к таблице дней
                    "day text NOT NULL REFERENCES Cassa(id)," + //день
                    "kindOfMoney text, " +//вид оплаты
                    "summ text NOT NULL, " +//сумма
                    "comment text" +//комментарий
                    ")";
                m_sqlCmd.ExecuteNonQuery();
                lbStatusText.Text = "Connected";
            }
            catch (SQLiteException ex)
            {
                lbStatusText.Text = "Disconnected";
                MessageBox.Show("Error: " + ex.Message);
            }

            readData();
        }

        private void readData()
        {
            DataTable dTable = new DataTable();
            String sqlQuery;

            if (m_dbConn.State != ConnectionState.Open)
            {
                MessageBox.Show("Open connection with database");
                return;
            }

            try
            {
                sqlQuery = "SELECT * FROM Cassa";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();

                    for (int i = 0; i < dTable.Rows.Count; i++)
                        dataGridView1.Rows.Add(dTable.Rows[i].ItemArray);
                }
                else
                    MessageBox.Show("Database is empty");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
    }
}
