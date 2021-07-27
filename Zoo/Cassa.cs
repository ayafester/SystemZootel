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

        private string todayDay;

        private double todayStart;
        private double todayCash;
        private double todayNotcash;
        private double todayFinish;
        private double todayDiff;
        private double todaySumm;
        private double todayCheck;

        private double inFacts;

        List<List<String>> AllData = new List<List<String>>(); //все данные

        List<String> todayData = new List<String>();//данные за день

        int idThisDay;
        private double newSumm;
        private string comment;
        private string kindOfMoney;


        public Cassa()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Cassa_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today; //получаем сегоднешний день
            todayDay = today.ToString("dd.MM.yyyy");
            MessageBox.Show(todayDay);

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

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Cassa (id integer PRIMARY KEY," + 
                    "thisDay text NOT NULL DEFAULT(0)," + //год месяц день
                    "start text DEFAULT(0), " +//стартовая сумма
                    "cash text DEFAULT(0), " +//наличные
                    "notcash text DEFAULT(0), " +//безнал
                    "finish text DEFAULT(0), " +//конец дня
                    "diff text DEFAULT(0), " +//всего
                    "summ text DEFAULT(0), " + // по факту
                    "checksDay text DEFAULT(0), " +//разница начала и конца
                    "inFacts text DEFAULT(0)" +//количество чеков 
                    ")";
                m_sqlCmd.ExecuteNonQuery();
                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS OneDayCassa (id integer PRIMARY KEY," + //таблица доходов и расходов за день, прикручена к таблице дней
                    "day text NOT NULL REFERENCES Cassa(id) DEFAULT(0)," + //день
                    "kindOfMoney text DEFAULT(0), " +//вид оплаты
                    "summ text DEFAULT(0), " +//сумма
                    "comment text DEFAULT(0)" +//комментарий
                    ")";
                m_sqlCmd.ExecuteNonQuery();
                lbStatusText.Text = "Connected";


                for(int i = 0; i < AllData.Count; i++)
                {
                    if (SearchString(AllData[i], todayDay) == true) //проверка сегодняшнего дня
                    {
                        MessageBox.Show("Сегодня уже есть");
                        break;
                    }
                    else
                    {
                        m_sqlCmd.CommandText = "INSERT INTO Cassa (thisDay) VALUES ( '" + todayDay + "') ";
                        m_sqlCmd.ExecuteNonQuery();
                    }
                }

                m_sqlCmd.CommandText = "SELECT id FROM Cassa WHERE thisDay = '" + todayDay + "' "; //искать по нескольким параметрам день месяц год
                SQLiteDataReader read2 = m_sqlCmd.ExecuteReader();

                while (read2.Read())
                {
                    idThisDay = read2.GetInt32(0);
                }
                read2.Close();
            }
            catch (SQLiteException ex)
            {
                lbStatusText.Text = "Disconnected";
                MessageBox.Show("Error: " + ex.Message);
            }
            m_dbConn.Close();

            ReadAllData();
            OutputDataToTable();

            DataToShowInTable();
        }

        private void ReadAllData() //чтение всех данных в таблице Cassa СОХРАНЯЕТ В ГЛОБАЛЬНУЮ ПЕРЕМЕННУЮ
        {
            m_dbConn.Open();
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
                
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    AllData.Add(new List<String>()); //add 2 rows

                    for (int k = 0; k < dTable.Columns.Count; k++)
                    {
                        AllData[i].Add(dTable.Rows[i].ItemArray[k].ToString());
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            m_dbConn.Close();
        }

        private void OutputDataToTable() //ВЫВОДИТ ДАННЫЕ ИЗ СЕГОДНЯШНЕГО ДНЯ, ДАННЫЕ ВСЕГДА НОВЫЕ ПОЛУЧАЕТ
        {
            todayData = readTodayData();

            dataGridView1.RowCount = 1;
            dataGridView1.ColumnCount = 7;

            int m = 0;
            for (int i = 1; i < 8; i++ )
            {
                dataGridView1.Rows[0].Cells[m].Value = todayData[i];
                m++;
            }
            textBox7.Text = todayData[8];
            label8.Text = todayData[0];

            m_dbConn.Open();


            DataTable dTable = new DataTable();
            string sqlQuery = "SELECT kindOfMoney, summ, comment FROM OneDayCassa WHERE day = '" + idThisDay + "' ";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            adapter.Fill(dTable);

            if (dTable.Rows.Count > 0)
            {
                dataGridView2.Rows.Clear();

                for (int i = 0; i < dTable.Rows.Count; i++)
                    dataGridView2.Rows.Add(dTable.Rows[i].ItemArray);
            }
            else
                MessageBox.Show("Database is empty");

            /*for (int i = 0; i < 1; i++) //вывод таблицы с использованием всех данных в БД
            {
                dataGridView1.RowCount = 1;
                dataGridView1.ColumnCount = 7;
                int m = 0;
                for (int k = 2; k < 9; k++)
                {
                    dataGridView1.Rows[i].Cells[m].Value = AllData[i][k];
                    m++;
                }
            }*/
            m_dbConn.Close();
        }
        private bool SearchString(List<String> AllData, string searchStr) // принимает массив строк и искомую строку и возвращает ДА, ЕСЛИ ЕСТЬ
        {
            for (int i = 0; i < AllData.Count; i++)
            {
                if(AllData[i] == searchStr)
                {
                    return true;
                }
            }
            return false;
        }
        private List<String> readTodayData() //ВОЗВРАЩАЕТ СПИСОК ДАННЫХ ЗА СЕГОДНЯШНИЙ ДЕНЬ
        {
            m_dbConn.Open();
            List<String> thisData = new List<String>();

            m_sqlCmd.CommandText = "SELECT start, cash, notcash, finish, diff, summ, checksDay, inFacts FROM Cassa WHERE thisDay = '" + todayDay + "' "; //искать по нескольким параметрам день месяц год
            SQLiteDataReader read1 = m_sqlCmd.ExecuteReader();
            
            while (read1.Read())
            {
                thisData.Add(todayDay);

                todayStart = Convert.ToDouble(read1.GetString(0));
                thisData.Add(read1.GetString(0));;

                todayCash = Convert.ToDouble(read1.GetString(1));
                thisData.Add(read1.GetString(1));

                todayNotcash = Convert.ToDouble(read1.GetString(2));
                thisData.Add(read1.GetString(2));

                todayFinish = Convert.ToDouble(read1.GetString(3));
                thisData.Add(read1.GetString(3));

                todayDiff = Convert.ToDouble(read1.GetString(4));
                thisData.Add(read1.GetString(4));

                todaySumm = Convert.ToDouble(read1.GetString(5));
                thisData.Add(read1.GetString(5));

                todayCheck = Convert.ToDouble(read1.GetString(6));
                thisData.Add(read1.GetString(6));

                inFacts = Convert.ToDouble(read1.GetString(7));
                thisData.Add(read1.GetString(7));
            }
            read1.Close();
            m_dbConn.Close();

            return thisData;
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) //ДОПУСТИМЫ ТОЛЬКО ЦИФРЫ И УДАЛИТЬ
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

        private void button4_Click(object sender, EventArgs e) //СОХРАНЯЕМ ДЕНЕГ ПО ФАКТУ, ОБНОВЛЯЕТ ТАБЛИЦЫ
        {
            inFacts = Convert.ToDouble(textBox7.Text);

            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;

            if (m_dbConn.State != ConnectionState.Open)
            {
                MessageBox.Show("Open connection with database");
                return;
            }

            m_sqlCmd.CommandText = "UPDATE Cassa SET ('inFacts') = '" + inFacts.ToString() + "'  WHERE thisDay = '" + todayDay + "' ";

            m_sqlCmd.ExecuteNonQuery();
            m_dbConn.Close();

            OutputDataToTable();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void Cash()
        {
            int idThisDay = 1;

            newSumm = Convert.ToDouble(textBox2.Text);
            kindOfMoney = "наличные";

            todayFinish += newSumm;
            todayCash += newSumm;
            todaySumm += newSumm;

            todayDiff = todayStart - todayFinish; //под вопросом

            todayCheck++;
            m_dbConn.Open(); // m_sqlCmd.CommandText = "INSERT INTO Cassa (thisDay) VALUES ( '" + todayDay + "') ";


            m_sqlCmd.CommandText = "INSERT INTO OneDayCassa ('day', 'kindOfMoney', 'summ', 'comment') values ('" +
                idThisDay + "' , '" +
                kindOfMoney + "' , '" +
                newSumm + "' , '" +
                comment + "')";

            m_sqlCmd.ExecuteNonQuery();

            m_sqlCmd.CommandText = "UPDATE Cassa SET ('cash') = '" + todayCash.ToString() + 
                "', ('finish')  = '" + todayFinish.ToString() +
                "', ('diff')  = '" + todayDiff.ToString() +
                "', ('summ')  = '" + todaySumm.ToString() +
                "', ('checksDay')  = '" + todayCheck.ToString() +
                "'  WHERE thisDay = '" + todayDay + "' ";
            m_sqlCmd.ExecuteNonQuery();

            m_dbConn.Close();

            OutputDataToTable();
        }

        private void NotCash()
        {
            int idThisDay = 1;

            newSumm = Convert.ToDouble(textBox2.Text);
            kindOfMoney = "безнал";

            todayNotcash += newSumm;
            todaySumm += newSumm;
            todayCheck++;
            m_dbConn.Open(); 

            m_sqlCmd.CommandText = "INSERT INTO OneDayCassa ('day', 'kindOfMoney', 'summ', 'comment') values ('" +
                idThisDay + "' , '" +
                kindOfMoney + "' , '" +
                newSumm + "' , '" +
                comment + "')";

            m_sqlCmd.ExecuteNonQuery();

            m_sqlCmd.CommandText = "UPDATE Cassa SET ('notcash') = '" + todayNotcash.ToString() +
                "', ('finish')  = '" + todayFinish.ToString() +
                "', ('summ')  = '" + todaySumm.ToString() +
                "', ('checksDay')  = '" + todayCheck.ToString() +
                "'  WHERE thisDay = '" + todayDay + "' ";
            m_sqlCmd.ExecuteNonQuery();

            m_dbConn.Close();

            OutputDataToTable();
        }

        private void Issue()
        {
            int idThisDay = 1;

            newSumm = Convert.ToDouble(textBox2.Text);
            kindOfMoney = "выдача";

            todayFinish -= newSumm;
            todaySumm -= newSumm;
            
            m_dbConn.Open();

            m_sqlCmd.CommandText = "INSERT INTO OneDayCassa ('day', 'kindOfMoney', 'summ', 'comment') values ('" +
                idThisDay + "' , '" +
                kindOfMoney + "' , '" +
                newSumm + "' , '" +
                comment + "')";

            m_sqlCmd.ExecuteNonQuery();

            m_sqlCmd.CommandText = "UPDATE Cassa SET ('finish') = '" + todayFinish.ToString() +
                "', ('summ')  = '" + todaySumm.ToString() +
                "'  WHERE thisDay = '" + todayDay + "' ";
            m_sqlCmd.ExecuteNonQuery();

            m_dbConn.Close();

            OutputDataToTable();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                Cash();
            } else if(comboBox1.SelectedIndex == 1)
            {
                NotCash();
            } else if (comboBox1.SelectedIndex == 2)
            {
                Issue();
            }
        }


        private void DataToShowInTable()
        {
            for (int i = 0; i < 1; i++) //вывод таблицы с использованием всех данных в БД
            {
                dataGridView3.RowCount = AllData.Count;
                dataGridView3.ColumnCount = 10;
                int m = 0;
                for (int k = 1; k < 10; k++)
                {
                    dataGridView3.Rows[i].Cells[m].Value = AllData[i][k];
                    m++;
                }

                m_dbConn.Open();
                m_sqlCmd.CommandText = "SELECT day, kindOfMoney, summ, comment FROM OneDayCassa";

                //string newcomment = "";

                SQLiteDataReader read3 = m_sqlCmd.ExecuteReader();

                while(read3.Read())
                {
                    //newcomment = 
                }
                read3.Close();
                m_dbConn.Close();
            }
        }

        private void dataGridView2_KeyUp(object sender, KeyEventArgs e) //мб менять таблицу
        {


        }
    }
}
