using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zoo
{
    public partial class Card : Form
    {
        public Card()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox8.Text.Length == 0)
            {
                MessageBox.Show("Сначала сохраните первого животного");
            }
            AddAnimal newAnimal = new AddAnimal();
            newAnimal.Show();
        }

    }
}
