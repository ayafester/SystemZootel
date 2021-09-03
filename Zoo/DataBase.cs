using System;
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
    public partial class DataBase : Form
    {
        public DataBase()
        {
            InitializeComponent();
        }

        private void DataBase_Load(object sender, EventArgs e)
        {
            try
            {
                Form1.m_dbConn.Open();
                Form1.m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS DataBase (id integer PRIMARY KEY," +
                    "numOfDogovor text DEFAULT(0)," +
                    "data text DEFAULT(0)," +
                    "surname text DEFAULT(0) ," +
                    "name text DEFAULT(0)," +
                    "animal text DEFAULT(0)," +
                    "nameOfAnimal text DEFAULT(0)," +
                    "number text DEFAULT(0)," +
                    "comment text DEFAULT(0)," +
                    "sale text DEFAULT(0) )";
                Form1.m_sqlCmd.ExecuteNonQuery();

                Form1.m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Animal (id integer PRIMARY KEY," +
                    "idOfMaster text NOT NULL DEFAULT(0) REFERENCES DataBase(id)," +
                    "breed text DEFAULT(0)," +
                    "name text DEFAULT(0)," +
                    "food text DEFAULT(0)," +
                    "walking text DEFAULT(0)," +
                    "comment text DEFAULT(0) )";
                Form1.m_sqlCmd.ExecuteNonQuery();
                Form1.m_dbConn.Close();
                FillDGV1();

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }



        }

        private void FillDGV1()
        {
            Form1.m_dbConn.Open();
            DataTable dTable = new DataTable();

            string sqlQuery = "SELECT numOfDogovor, data, surname, name, animal, nameOfAnimal, number, comment, sale FROM DataBase ";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, Form1.m_dbConn);
            adapter.Fill(dTable);

            if (dTable.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();

                for (int i = 0; i < dTable.Rows.Count; i++)
                    dataGridView1.Rows.Add(dTable.Rows[i].ItemArray);
            }
            else
                MessageBox.Show("Данных нет");

            Form1.m_dbConn.Close();
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) //сохраняет автоматически данные в базе
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
            for (int i = 0; i < 9; i++) //обработка нуллов
            {
                if (dataGridView1.Rows[rowIndex].Cells[i].Value == null)
                {
                    dataGridView1.Rows[rowIndex].Cells[i].Value = "";
                }
            }

            for (int i = 0; i < 9; i++)
            {
                if(dataGridView1.Rows[rowIndex].Cells[i].Value.ToString().Length != 0)
                {
                    //найти совпадение по всем параметрам в БД строчке
                }
            }
            string thisDogo = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            //MessageBox.Show("номер договора: " + thisDogo);
            
            if (thisDogo.Length == 0)
            {
                insertData(rowIndex);
            } else
            {
                if (dataGridView1.Rows[rowIndex].Cells[1].Value.ToString().Length == 0) //новая строка начинания с договора
                {
                    insertData(rowIndex);
                }

                UpdateData(rowIndex, thisDogo);
            }

            // MessageBox.Show("e" + rowIndex.ToString());
        }

        private List<int> SearchRow(string thisStr) { 

            List<int> allRowsCount = new List<int>();
            List<string> oneRow = new List<string>();
            List<List<string>> allRowsinDB = new List<List<string>>();

            if (thisStr == "") //если пустая строчка
            {
                return allRowsCount;
            }
            Form1.m_dbConn.Open();
            Form1.m_sqlCmd.CommandText = "SELECT * FROM DataBase";
            SQLiteDataReader read = Form1.m_sqlCmd.ExecuteReader();
            while (read.Read())
            {
                string id = read["id"].ToString();
                oneRow.Add(id);
                string numOfDogovor = read["numOfDogovor"].ToString();
                oneRow.Add(numOfDogovor);
                string data = read["data"].ToString();
                oneRow.Add(data);
                string surname = read["surname"].ToString();
                oneRow.Add(surname);
                string name = read["name"].ToString();
                oneRow.Add(name);
                string animal = read["animal"].ToString();
                oneRow.Add(animal);
                string nameOfAnimal = read["nameOfAnimal"].ToString();
                oneRow.Add(nameOfAnimal);
                string number = read["number"].ToString();
                oneRow.Add(number);
                string comment = read["comment"].ToString();
                oneRow.Add(comment);
                string sale = read["sale"].ToString();
                oneRow.Add(sale);

                for (int i = 0; i < 9; i++)
                {
                    if (oneRow[i] == thisStr)
                    {
                        int z = 0;
                        for (int m = 0; m < allRowsCount.Count; m++)
                        {
                            if (allRowsCount[m] == Convert.ToInt32(id))
                            {
                                z++;
                            }
                        }
                        if (z == 0)
                        {
                            allRowsCount.Add(Convert.ToInt32(id));
                        }
                    } 
                }
                
                allRowsinDB.Add(oneRow);
                oneRow.Clear();
            }
            read.Close();
            Form1.m_dbConn.Close();

            if (allRowsCount.Count == 0)//если ничего не найдено
            {
                return allRowsCount;
            } else
            {
                return allRowsCount;
            }
        }
        private void insertData(int rowIndex)
        {
            Form1.m_dbConn.Open();
            MessageBox.Show("вставляем строку в таблицу");
            Form1.m_sqlCmd.CommandText = "INSERT INTO DataBase ('numOfDogovor', 'data', 'surname', 'name', 'animal', 'nameOfAnimal', 'number', 'comment', 'sale') VALUES " +
                "('" + dataGridView1.Rows[rowIndex].Cells[0].Value.ToString() + "'," +
                "' " + dataGridView1.Rows[rowIndex].Cells[1].Value.ToString() + " '," +
                "' " + dataGridView1.Rows[rowIndex].Cells[2].Value.ToString() + "'," +
                "' " + dataGridView1.Rows[rowIndex].Cells[3].Value.ToString() + "'," +
                "' " + dataGridView1.Rows[rowIndex].Cells[4].Value.ToString() + "'," +
                "' " + dataGridView1.Rows[rowIndex].Cells[5].Value.ToString() + "'," +
                "' " + dataGridView1.Rows[rowIndex].Cells[6].Value.ToString() + "'," +
                "' " + dataGridView1.Rows[rowIndex].Cells[7].Value.ToString() + "'," +
                "' " + dataGridView1.Rows[rowIndex].Cells[8].Value.ToString() + "') ";
            Form1.m_sqlCmd.ExecuteNonQuery();
            Form1.m_dbConn.Close();
        }

        private void UpdateData(int rowIndex, string id)
        {
            MessageBox.Show("обновляем таблицу");
            Form1.m_dbConn.Open();
            Form1.m_sqlCmd.CommandText = "UPDATE DataBase SET ('numOfDogovor') = '" + dataGridView1.Rows[rowIndex].Cells[0].Value.ToString() + "', " +
                 "('data') = '" + dataGridView1.Rows[rowIndex].Cells[1].Value.ToString() + "', " +
                 "('surname') = '" + dataGridView1.Rows[rowIndex].Cells[2].Value.ToString() + "', " +
                 "('name') = '" + dataGridView1.Rows[rowIndex].Cells[3].Value.ToString() + "', " +
                 "('animal') = '" + dataGridView1.Rows[rowIndex].Cells[4].Value.ToString() + "', " +
                 "('nameOfAnimal') = '" + dataGridView1.Rows[rowIndex].Cells[5].Value.ToString() + "', " +
                 "('number') = '" + dataGridView1.Rows[rowIndex].Cells[6].Value.ToString() + "', " +
                 "('comment') = '" + dataGridView1.Rows[rowIndex].Cells[7].Value.ToString() + "', " +
                 "('sale') = '" + dataGridView1.Rows[rowIndex].Cells[8].Value.ToString() + "' WHERE numOfDogovor = '" + id + "'";
            Form1.m_sqlCmd.ExecuteNonQuery();
            Form1.m_dbConn.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //если в одной строчке повторяется слова, то два раза вывохидт строчка
            string findText = textBox1.Text;
            dataGridView1.Rows.Clear();
            List<int> findRows = SearchRow(findText);

            if (findRows.Count > 0)
            {
                MessageBox.Show(findRows.Count.ToString() + " Это сколько найдено совпадений");
                for (int i = 0; i < findRows.Count; i++)
                {
                    string id = findRows[i].ToString();
                    DataTable dTable2 = new DataTable();
                    string sqlQuery2 = "SELECT numOfDogovor, data, surname, name, animal, nameOfAnimal, number, comment, sale FROM DataBase WHERE id = '" + id + "' ";
                    SQLiteDataAdapter adapter2 = new SQLiteDataAdapter(sqlQuery2, Form1.m_dbConn);
                    adapter2.Fill(dTable2);

                    for (int k = 0; k < dTable2.Rows.Count; k++)
                        dataGridView1.Rows.Add(dTable2.Rows[k].ItemArray);
                }
            } else
            {
                MessageBox.Show("Результатов нет");
                FillDGV1();
            }
        }

       
        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            int thisRow = dataGridView1.CurrentRow.Index;

            for (int i = 0; i < 9; i++)
            {
                if(dataGridView1.Rows[thisRow].Cells[i].Value.ToString().Length != 0)
                {
                    string firstStr = dataGridView1.Rows[thisRow].Cells[i].Value.ToString();
                    MessageBox.Show(firstStr);
                    List<int> thisRowForDel = SearchRow(firstStr);
                    MessageBox.Show(thisRowForDel.Count.ToString() + " количество совпадений " + thisRowForDel[0].ToString());
                    if (thisRowForDel.Count != 0)
                    {
                        string id = thisRowForDel[0].ToString();
                        Form1.m_dbConn.Open();
                        Form1.m_sqlCmd.CommandText = "DELETE FROM DataBase where id = '" + id + "' ";
                        Form1.m_sqlCmd.ExecuteNonQuery();
                        Form1.m_dbConn.Close();
                        MessageBox.Show("Строчку удалили с айди: " + id );
                    } else
                    {
                        MessageBox.Show("Строчку не удалили");
                    }
                    break;
                }
            }
        }
    }
}
