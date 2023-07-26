using System;
using System.IO.Ports;
using System.Windows.Forms;
using WeightBalance.Properties;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace WeightBalance
{
    public partial class Form1 : Form
    {
        float s,total,current,last;
        bool zero, minus;
        int counter = 0, counter2 = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            connection con1 = new connection();
            //con1.query = "insert into WeightData (Weight, DateData) Values (400.0, datetime('now', 'localtime'));";
            //con1.editdata();

            //-------Showing on grid view
            con1.query = "SELECT * FROM WeightData;";
            dataGridView1.DataSource = con1.getdata().Tables[0];
            float totalFloatData = 0;
            foreach (DataRow row in con1.ds.Tables[0].Rows)
            {
                float floatData = float.Parse(row[1].ToString());
                totalFloatData += floatData;
            }
            textBox2.Text = last.ToString();
            total = last + total;
            textBox3.Text = totalFloatData.ToString();

            zero = false; minus = false;last = 0;
            try
            {
                string[] ports = SerialPort.GetPortNames();
                serialPort1.PortName = "COM1";
                serialPort1.BaudRate = 9600;
                serialPort1.Parity = Parity.None;

                if (serialPort1 != null && serialPort1.IsOpen)
                {
                    serialPort1.Close(); // Ensure the COM port is closed when the application exits
                }

                serialPort1.Open();
               // if(serialPort1.IsOpen) { MessageBox.Show("Connected"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Form Load"+ex.Message);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    string check;
                    string f = serialPort1.ReadExisting();
                    //MessageBox.Show(f);
                    //char[] stringArray = f.ToCharArray();
                    //if(stringArray.Length != 0)
                    //{
                    //    stringArray[0] = '0';
                    //}
                    //Array.Reverse(stringArray);
                    //string d = new string(stringArray);
                    string d = "";
                    for(int i = 5; i > 0; i--)
                    {
                        d += f[i];
                    }
                    float.TryParse(d, out s);
                    if (f[7] == '-' || f[8] == '-' || f[9] == '-') { s = s * -1; }
                    if(s != 0)txtReceive.Text = s.ToString($"F{2}");
                    current = float.Parse(txtReceive.Text);


                    zero = false;

                    if (current < -130 && current > -140)
                    {
                        minus = true;
                        counter = 0;
                    }
                    if (current>last) 
                    { 
                        last = current;
                    }
                    if(current<last)
                    {
                        counter++;
                        if (counter >= 5)
                        {
                            last = current;
                            counter = 0;
                        }
                    }
                    else
                    {
                        counter = 0;
                    }
                    if(minus && current > 10)
                    {
                        counter2++;
                        if (counter2 > 5)
                        {
                            minus=false;
                            counter2 = 0;
                        }
                    }
                    else
                    {
                        counter2 = 0;
                    }
                    check = last.ToString();
                    
                    check = check + minus.ToString();
                    if(current> -7 && current < 7 && minus)
                    {
                        zero = true;
                    }
                    check = check  + zero.ToString();
                    check += counter.ToString() + "||" + counter2.ToString();
                    textBox1.Text = check;
                    if(zero && last>10)
                    {
                        connection con1 = new connection();
                        con1.query = "insert into WeightData (Weight, DateData) Values ("+last+ ", datetime('now','localtime'));";
                        con1.editdata();

                        string connectionString = "Data Source=192.168.1.4;Initial Catalog=Boiler;User ID=sa;Password=123;";
                        string createTableQuery = "insert into WeightData (Weight, DateData) Values (" + last + ", GETDATE());";
                        //string createTableQuery = "CREATE TABLE WeightData(" +
                        //    "SRNo INT IDENTITY(1,1) PRIMARY KEY," +
                        //    "Weight REAL," +
                        //    "DateData DATETIME DEFAULT (GETDATE()) );";

                        //SqlConnection connection = new SqlConnection(connectionString);
                        //connection.Open();
                        //if(connection.State == ConnectionState.Open) 
                        //{
                        //    SqlCommand command = new SqlCommand(createTableQuery, connection);
                        //    command.ExecuteNonQuery();
                        //}



                        //-------Showing on grid view
                        con1.query = "SELECT * FROM WeightData;";
                        dataGridView1.DataSource = con1.getdata().Tables[0];
                        float totalFloatData = 0;
                        foreach (DataRow row in con1.ds.Tables[0].Rows)
                        {
                            float floatData = float.Parse(row[1].ToString());
                            totalFloatData += floatData;
                        }
                        textBox2.Text = last.ToString();
                        total = last + total;
                        textBox3.Text = totalFloatData.ToString();
                        last = 0;minus = false; zero = false;
                    }
                }
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                DialogResult result = MessageBox.Show(ex.Message+"\nCheck the Weight and Then Press OK", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (result == DialogResult.OK)
                {
                    timer1.Enabled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            Form2.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }

        private void savbt_Click(object sender, EventArgs e)
        {

        }
    }
}
