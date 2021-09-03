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
using OfficeOpenXml;


namespace Zoo
{
    public partial class Cassa : Form
    {
        private string dbFileName;
        private SQLiteConnection m_dbConn;
        private SQLiteCommand m_sqlCmd;

        private string todayDay;
        private string todayMonth;
        private string todayYear;
        private string lastDay;

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

        

        private void Cassa_Load(object sender, EventArgs e)
        {
            todayDay = DateTime.Today.ToString("dd.MM.yyyy"); //сегодняшний день
            todayMonth = DateTime.Today.ToString("MM");
            todayYear = DateTime.Today.ToString("yyyy");

            lastDay = DateTime.Today.AddDays(-1).ToString("dd.MM.yyyy");//вчерашний день

            m_dbConn = new SQLiteConnection(); //переменные для подключения к БД
            m_sqlCmd = new SQLiteCommand();

            dbFileName = "zootel.sqlite";//название БД
            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;"); //создаем связь
                m_dbConn.Open();//открываем связь
                m_sqlCmd.Connection = m_dbConn;//создаем команду

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Cassa (id integer PRIMARY KEY," +
                    "year text DEFAULT(0), " +  //год
                    "month text DEFAULT(0), " +  //месяц
                    "thisDay text NOT NULL DEFAULT(0)," + //сегодня
                    "start text DEFAULT(0), " +//стартовая сумма
                    "cash text DEFAULT(0), " +//наличные
                    "notcash text DEFAULT(0), " +//безнал
                    "finish text DEFAULT(0), " +//конец дня
                    "diff text DEFAULT(0), " +//всего
                    "summ text DEFAULT(0), " + // по факту
                    "checksDay text DEFAULT(0), " +//разница начала и конца
                    "inFacts text DEFAULT(0)," +
                    "commento text DEFAULT(0)" +//количество чеков 
                    ")";
                m_sqlCmd.ExecuteNonQuery();
                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS OneDayCassa (id integer PRIMARY KEY," + //таблица доходов и расходов за день, прикручена к таблице дней
                    "day text NOT NULL REFERENCES Cassa(id) DEFAULT(0)," + //день
                    "kindOfMoney text DEFAULT(0), " +//вид оплаты
                    "summ text DEFAULT(0), " +//сумма
                    "comment text DEFAULT(0)" +//комментарий
                    ")";
                m_sqlCmd.ExecuteNonQuery();
                m_dbConn.Close();

                AllData = ReadAllData();
                if (SearchString(AllData, todayDay) == true) //проверка сегодняшнего дня
                {
                    MessageBox.Show("Сегодня уже есть");
                }
                else
                {
                    MessageBox.Show("Сегодня нет");
                    m_dbConn.Open();
                    string finishLastDay = "0";
                    if(SearchString(AllData, lastDay))
                    {
                        m_sqlCmd.CommandText = "SELECT finish FROM Cassa WHERE thisDay = '" + lastDay + "' "; 
                        SQLiteDataReader read5 = m_sqlCmd.ExecuteReader();

                        while (read5.Read())
                        {
                            finishLastDay = read5.GetString(0);
                        }
                        read5.Close();
                    } else
                    {
                        finishLastDay = "0";
                    }
                    m_sqlCmd.CommandText = "INSERT INTO Cassa (year, month, thisDay, start) VALUES ( '" + todayYear + "', '" + todayMonth + "', '" + todayDay + "', '" + finishLastDay + "') ";
                    m_sqlCmd.ExecuteNonQuery();
                    m_dbConn.Close();
                    }
                m_dbConn.Open();
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
                MessageBox.Show("Error: " + ex.Message);
            }

            m_dbConn.Close();

            OutputDataToTable();

            DataToShowInTable(todayMonth);
        }

        private List<List<String>> ReadAllData() //чтение всех данных в таблице Cassa ВОЗВРАЩАЕТ СПИСОК ИЗ СПИСКОВ СТРОК
        {
            List<List<String>> tempData = new List<List<String>>();
            m_dbConn.Open();

            DataTable dTable = new DataTable();
            String sqlQuery;

            if (m_dbConn.State != ConnectionState.Open)
            {
                MessageBox.Show("Open connection with database");
            }

            try
            {
                sqlQuery = "SELECT * FROM Cassa";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);
                
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    tempData.Add(new List<String>());

                    for (int k = 0; k < dTable.Columns.Count; k++)
                    {
                        tempData[i].Add(dTable.Rows[i].ItemArray[k].ToString());
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            m_dbConn.Close();
            return tempData;
            
        }

       
        
        private string GetCommentFromDay(int day)
        {
            m_dbConn.Open();
            DataTable dTable = new DataTable();
            string sqlQuery = "SELECT kindOfMoney, summ, comment FROM OneDayCassa WHERE day = '" + day + "' "; //получить айдишки всех дней за месяц
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            adapter.Fill(dTable);

            string oneComment = "";
            List<String> array = new List<String>();
            string allComment = "";


            if (dTable.Rows.Count > 0)
            {
                dataGridView2.Rows.Clear();

                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    dataGridView2.Rows.Add(dTable.Rows[i].ItemArray);
                    oneComment = dTable.Rows[i].ItemArray[0].ToString() + "/" + dTable.Rows[i].ItemArray[1].ToString() + "/" + dTable.Rows[i].ItemArray[2].ToString() + ";";
                    array.Add(oneComment);
                }
                allComment = string.Join(" ", array);
            }
            else
                MessageBox.Show("Данных нет");
            m_dbConn.Close();
            return allComment;
        }
        private void OutputDataToTable() //ВЫВОДИТ ДАННЫЕ ИЗ СЕГОДНЯШНЕГО ДНЯ, ДАННЫЕ ВСЕГДА НОВЫЕ ПОЛУЧАЕТ
        {
            DataToShowInTable(todayMonth);
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
                {
                    dataGridView2.Rows.Add(dTable.Rows[i].ItemArray);
                }
            }
            else
                MessageBox.Show("Данных нет");

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
        private bool SearchString(List<List<String>> ThisData, string searchStr) // принимает массив строк и искомую строку и возвращает ДА, ЕСЛИ ЕСТЬ
        {
            List<String> onlyDatas = new List<String>();

            for (int i = 0; i < ThisData.Count; i++)
            {
                onlyDatas.Add(ThisData[i][3]);
            }

            for (int i = 0; i < onlyDatas.Count; i++)
            {
                if(onlyDatas[i] == searchStr)
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
            string tempCom = GetCommentFromDay(idThisDay);
            newSumm = Convert.ToDouble(textBox2.Text);
            kindOfMoney = "наличные";
            comment = textBox1.Text;

            todayFinish += newSumm + todayStart;
            todayCash += newSumm;
            todaySumm += newSumm + todayStart;

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
                "', ('commento')  = '" + tempCom +
                "'  WHERE thisDay = '" + todayDay + "' ";
            m_sqlCmd.ExecuteNonQuery();

            m_dbConn.Close();

            OutputDataToTable();
        }

        private void NotCash()
        {
            string tempCom = GetCommentFromDay(idThisDay);
            newSumm = Convert.ToDouble(textBox2.Text);
            kindOfMoney = "безнал";
            comment = textBox1.Text;

            todayNotcash += newSumm;
            todaySumm += newSumm + todayStart;
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
                "', ('commento')  = '" + tempCom +
                "'  WHERE thisDay = '" + todayDay + "' ";
            m_sqlCmd.ExecuteNonQuery();

            m_dbConn.Close();

            OutputDataToTable();
        }

        private void Issue()
        {
            string tempCom = GetCommentFromDay(idThisDay);
            newSumm = Convert.ToDouble(textBox2.Text);
            kindOfMoney = "выдача";
            comment = textBox1.Text;

            todayFinish -= newSumm + todayStart;
            todaySumm -= newSumm + todayStart;
            
            m_dbConn.Open();

            m_sqlCmd.CommandText = "INSERT INTO OneDayCassa ('day', 'kindOfMoney', 'summ', 'comment') values ('" +
                idThisDay + "' , '" +
                kindOfMoney + "' , '" +
                newSumm + "' , '" +
                comment + "')";

            m_sqlCmd.ExecuteNonQuery();

            m_sqlCmd.CommandText = "UPDATE Cassa SET ('finish') = '" + todayFinish.ToString() +
                "', ('summ')  = '" + todaySumm.ToString() +
                "', ('commento')  = '" + tempCom +
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


        private DataTable DataToShowInTable(string month)
        {
            m_dbConn.Open();
            DataTable dTable = new DataTable();
            string sqlQuery = "SELECT thisDay, start, cash, notcash, finish, diff, summ, checksDay, inFacts, commento FROM Cassa WHERE month = '" + month + "' ";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            adapter.Fill(dTable);

            if (dTable.Rows.Count > 0)
            {
                dataGridView3.Rows.Clear();

                for (int i = 0; i < dTable.Rows.Count; i++)
                    dataGridView3.Rows.Add(dTable.Rows[i].ItemArray);
            }
            else
                MessageBox.Show("Данных нет");

            m_dbConn.Close();
            return dTable;
            /*AllData = ReadAllData();
            if(AllData.Count != 0)
            {
                for (int i = 0; i < AllData.Count; i++) //вывод таблицы с использованием всех данных в БД
                {
                    dataGridView3.RowCount = AllData.Count;
                    dataGridView3.ColumnCount = 10;
                    int m = 0;
                    for (int k = 3; k < 12; k++)
                    {
                        dataGridView3.Rows[i].Cells[m].Value = AllData[i][k];
                        m++;
                    }

                    m_dbConn.Open();
                    m_sqlCmd.CommandText = "SELECT day, kindOfMoney, summ, comment FROM OneDayCassa";

                    //string newcomment = "";

                    SQLiteDataReader read3 = m_sqlCmd.ExecuteReader();

                    while (read3.Read())
                    {
                        //newcomment = 
                    }
                    read3.Close();
                    m_dbConn.Close();
                }
            }*/

        }


        private void button2_Click(object sender, EventArgs e)
        {
            string searchMonth = idThisDay.ToString();
            if (textBox3.Text == "январь")
            {
                searchMonth = "01";
            } else if (textBox3.Text =="февраль")
            {
                searchMonth = "02";
            }
            else if (textBox3.Text == "март")
            {
                searchMonth = "03";
            }
            else if (textBox3.Text == "апрель")
            {
                searchMonth = "04";
            }
            else if (textBox3.Text == "май")
            {
                searchMonth = "05";
            }
            else if (textBox3.Text == "июнь")
            {
                searchMonth = "06";
            }
            else if (textBox3.Text == "июль")
            {
                searchMonth = "07";
            }
            else if (textBox3.Text == "август")
            {
                searchMonth = "08";
            }
            else if (textBox3.Text == "сентябрь")
            {
                searchMonth = "09";
            }
            else if (textBox3.Text == "октябрь")
            {
                searchMonth = "10";
            }
            else if (textBox3.Text == "ноябрь")
            {
                searchMonth = "11";
            }
            else if (textBox3.Text == "декабрь")
            {
                searchMonth = "12";
            } else
            {
                MessageBox.Show("Введите месяц");
            }

            DataToShowInTable(searchMonth);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            number = char.ToLower(number);
           
            if (Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void SaveEXLSX(string nameOfMonth, DataTable dataTable)
        {
            FileInfo newFile = new FileInfo(nameOfMonth + @"\table.xlsx");
            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Accounts");
                ws.Cells["A1"].LoadFromDataTable(dataTable, true);
                pck.Save();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //SaveEXLSX(textBox3.Text, DataToShowInTable(todayMonth));
        }
    }
}
