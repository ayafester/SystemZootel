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
    public partial class Schedule : Form
    {
        

        public Schedule()
        {
            InitializeComponent();
        }

        private void Schedule_Load(object sender, EventArgs e)
        {

            /*
            Form1.m_dbConn.Open();
            Form1.m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Schedule (id INTEGER PRIMARY KEY," +
                "nameOfBox text DEFAULT(0)," +
                "walkTimeMorning text DEFAULT(0)," +
                "checkWalk1 text DEFAULT(0)," +
                "walkTimeDinner text DEFAULT(0)," +
                "checkWalk2 text DEFAULT(0)," +
                "walkTimeEvening text DEFAULT(0)," +
                "checkWalk3 text DEFAULT(0)," +
                "eatTimeMorning text DEFAULT(0)," +
                "checkEat1 text DEFAULT(0)," +
                "eatTimeDinner text DEFAULT(0)," +
                "checkEat2 text DEFAULT(0)," +
                "eatTimeEvening text DEFAULT(0)," +
                "checkEat3 text DEFAULT(0)," +
                "cleanTimeMorning text DEFAULT(0)," +
                "checkClean1 text DEFAULT(0)," +
                "cleanTimeDinner text DEFAULT(0)," +
                "checkClean2 text DEFAULT(0)," +
                "cleanTimeEvening text DEFAULT(0)," +
                "checkClean3 text DEFAULT(0)," +
                "other text DEFAULT(0)" +
                ")";
            Form1.m_sqlCmd.ExecuteNonQuery();

            for (int i = 0; i < 9; i++)
            {
                    Form1.m_sqlCmd.CommandText = "INSERT INTO Schedule ('nameOfBox', 'walkTimeMorning', 'checkWalk1', 'walkTimeDinner', 'checkWalk2', " +
                "'walkTimeEvening', 'checkWalk3', 'eatTimeMorning', 'checkEat1', 'eatTimeDinner', 'checkEat2', 'eatTimeEvening', 'checkEat3', " +
                "'cleanTimeMorning', 'checkClean1', 'cleanTimeDinner', 'checkClean2', 'cleanTimeEvening', 'checkClean3', 'other') values " +
                "('" + sched.Rows[i].Cells[0].Value + "', ' ', '', ' ', '', ' ', '', ' ', '', ' '," +
                "'', ' ', '', ' ', '', ' ', '', ' ', '', ' ')";
                    Form1.m_sqlCmd.ExecuteNonQuery();
            }
            

            Form1.m_dbConn.Close();*/


            
        }

        private void GetDataTODB(int i, int j, int k)
        {
            /*Form1.m_dbConn.Open();
            Form1.m_sqlCmd.CommandText = "UPDATE Schedule SET ('nameOfBox') = '" + sched.Rows[i].Cells[j].Value.ToString() + "', ('walkTimeMorning')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkWalk1')  = '" + sched.Rows[i].Cells[j].Value.ToString() +  
                   "', ('walkTimeDinner')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkWalk2')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('walkTimeEvening')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkWalk3')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('eatTimeMorning')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkEat1')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('eatTimeDinner')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkEat2')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('eatTimeEvening')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkEat3')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('cleanTimeMorning')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkClean1')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('cleanTimeDinner')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkClean2')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('cleanTimeEvening')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('checkClean3')  = '" + sched.Rows[i].Cells[j].Value.ToString() +
                   "', ('other) =  '" + sched.Rows[i].Cells[j].Value.ToString() + "' WHERE id = '" + k + "' ";
            Form1.m_sqlCmd.ExecuteNonQuery();
            Form1.m_dbConn.Close();*/
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
