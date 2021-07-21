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
        private SQLiteConnection cnt;
        private SQLiteCommand cmd;

        public Cassa()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Cassa_Load(object sender, EventArgs e)
        {
            cnt = new SQLiteConnection();
            cmd = new SQLiteCommand();

            dbFileName = "zootel";

            if(!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
            } try
            {
                cnt = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                cnt.Open();
                cmd.Connection = cnt;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Days (id INTEGER PRIMARY KEY AUTOINCREMENT, day INT, book TEXT)";
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
