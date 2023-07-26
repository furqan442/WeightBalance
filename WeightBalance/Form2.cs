using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WeightBalance
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection con1 = new connection();
            //-------Showing on grid view
            con1.query = "SELECT * FROM WeightData WHERE DateData >= '"+dateTimePicker1.Text+"' AND DateData < '"+dateTimePicker2.Text+"'";
            dataGridView1.DataSource = con1.getdata().Tables[0];
            float totalFloatData = 0;
            foreach (DataRow row in con1.ds.Tables[0].Rows)
            {
                float floatData = float.Parse(row[1].ToString());
                totalFloatData += floatData;
            }
            textBox1.Text = totalFloatData.ToString();

        }
    }
}
