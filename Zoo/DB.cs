using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;

namespace Zoo
{
    class DB
    {
        public static string dbFileName;
        public static SQLiteConnection Conn;
        public static SQLiteCommand Cmd;

        public static void Connect()
        {
            Conn = new SQLiteConnection(); //переменные для подключения к БД
            Cmd = new SQLiteCommand();

            dbFileName = "zootel.sqlite";//название БД

            try
            {
                Conn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;"); //создаем с
                Conn.Open();//открываем связь
                Cmd.Connection = Conn;//создаем команду
                Conn.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
        public static void CreateTables()
        {
            Conn.Open();
            try
            {
                Cmd.CommandText = "CREATE TABLE IF NOT EXISTS DataBase (id integer PRIMARY KEY," + //таблица Базы данных хозяев
                    "numOfDogovor text DEFAULT(0)," + //номер договора
                    "data text DEFAULT(0)," + //дата создания
                    "surname text DEFAULT(0) ," + //фамилия
                    "name text DEFAULT(0)," + //имя
                    "lastname text DEFAULT(0)," +
                    "number text DEFAULT(0)," + //номер
                    "comment text DEFAULT(0)," + //комментарий
                    "sale text DEFAULT(0) )"; //скидка
                Cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw;
            }

            try
            {
                Cmd.CommandText = "CREATE TABLE IF NOT EXISTS AnimalData (id integer PRIMARY KEY," + //таблица Животных
                    "idOfMaster NOT NULL REFERENCES DataBase(id) text DEFAULT(0)," + // айди хозяина
                    "animal text DEFAULT(0)," +//вид
                    "breed text DEFAULT(0)," +//порода
                    "name text DEFAULT(0)," +//имя
                    "walk text DEFAULT(0)," +//прогулка
                    "feed text DEFAULT(0)," +//корм
                    "special text DEFAULT(0)," +//особенности
                    "comment text DEFAULT(0) )";//комменто
                Cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                //throw;
            }

            try
            {
                Cmd.CommandText = "CREATE TABLE IF NOT EXISTS Schedule (id integer PRIMARY KEY," + //таблица дня
                    "day text DEFAULT(0)," + //день недели
                    "comment text DEFAULT(0) )"; //коммент дня
                Cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            try
            {
                Cmd.CommandText = "CREATE TABLE IF NOT EXISTS BScheduleBox (id integer PRIMARY KEY," + //таблица расписание
                    "dayId NOT NULL REFERENCES Schedule(id) text DEFAULT(0)," + //день недели
                    "comment text DEFAULT(0), " + // прочее
                    "nameOfBox text DEFAULT(0) )"; //название бокса
                Cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                //throw;
            }

            try
            {
                Cmd.CommandText = "CREATE TABLE IF NOT EXISTS WScheduleWalking (id integer PRIMARY KEY," + //таблица выгулов
                    "idBox NOT NULL REFERENCES ScheduleBox(id) text DEFAULT(0)," + // айди бокса
                    "morning text DEFAULT(0)," + //утро
                    "dinner text DEFAULT(0)," +//день
                    "evening text DEFAULT(0)," +//вечер
                    "comment text DEFAULT(0) )"; //комменты про выгулы, какашки
                Cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            try
            {
                Cmd.CommandText = "CREATE TABLE IF NOT EXISTS EScheduleEating (id integer PRIMARY KEY," + //таблица кормления
                    "idBox NOT NULL REFERENCES ScheduleBox(id) text DEFAULT(0)," + // айди бокса
                    "morning text DEFAULT(0)," + //утро
                    "dinner text DEFAULT(0) )"; //вечер
                Cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            try
            {
                Cmd.CommandText = "CREATE TABLE IF NOT EXISTS CScheduleCleaning (id integer PRIMARY KEY," + //таблица уборок
                    "idBox NOT NULL REFERENCES ScheduleBox(id) text DEFAULT(0)," + // айди бокса
                    "morning text DEFAULT(0)," + //утро
                    "dinner text DEFAULT(0)," +//день
                    "evening text DEFAULT(0) )";
                Cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            Conn.Close();
        }

        public static void InsertNewAnimal()
        {
            Conn.Open();
            /*Cmd.CommandText = "INSERT INTO OneDayCassa ('day', 'kindOfMoney', 'summ', 'comment') values ('" +
                idThisDay + "' , '" +
                kindOfMoney + "' , '" +
                newSumm + "' , '" +
                comment + "')";*/
        }
    }
}
