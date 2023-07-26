using System;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace WeightBalance
{
    internal class connection
    {

        public SQLiteConnection con;
        public SQLiteDataAdapter da;
        public SQLiteCommand cmd = new SQLiteCommand();
        public DataSet ds = new DataSet();
        SQLiteCommandBuilder cmbl;
        public string query { set; get; }
        public connection(string query = "", string db = "Database1")
        {
            con = new SQLiteConnection(@"Data Source=./" + db + ".db;Version=3;");
            if (!File.Exists("./" + db + ".db"))
            {
                databasecreater(db);
            }
            this.query = query;
        }

        private void databasecreater(string db)
        {
            SQLiteConnection.CreateFile("./" + db + ".db");

            this.query =
                ////            /////////////////////1
                "CREATE TABLE WeightData(" +
                "SRNo INTEGER PRIMARY KEY," +
                "Weight REAL," +
                "DateData TEXT DEFAULT (datetime('now', 'localtime')) );";

            editdata();
            this.query = "";
            MessageBox.Show("Database Created");
        }

        public bool editdata()
        {
            try
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = query;
                int bo = cmd.ExecuteNonQuery();
                if (bo != 0)
                {
                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("From SQLite:  " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return false;
        }

        public DataSet getdata()
        {
            try
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = query;
                da = new SQLiteDataAdapter(cmd);
                da.SelectCommand.Connection = con;
                da.Fill(ds);
                cmbl = new SQLiteCommandBuilder(da);
            }
            catch (Exception ex)
            {
                MessageBox.Show("From SQLite:  " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ds;
        }
    }
}
