using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace HardwareManagementTool
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=THURUNU_YM\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False");

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            String username, pass;

            username = username_tb.Text;
            pass = pass_tb.Text;

            try
            {
                string query = "SELECT * FROM [User] WHERE username COLLATE Latin1_General_CS_AS = @username AND password COLLATE Latin1_General_CS_AS = @password";

                //An SqlCommand object is created using the query and the connection object conn
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", pass);

                //An SqlDataAdapter is created to execute the command and fill the results into a DataTable named dt.
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    //username = username_tb.Text;
                   // pass = pass_tb.Text;

                    //page that needed to be load next
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                    this.Hide();

                }

                else
                {
                    MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    username_tb.Clear();
                    pass_tb.Clear();

                    //focus on username
                    username_tb.Focus();
                }

            }
            catch
            {
                MessageBox.Show("Error");

            }
            finally
            {
                conn.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            username_tb.Clear();
            pass_tb.Clear();

            username_tb.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res;
            res = MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
            else { this.Show(); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Items itm = new Items();
            itm.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Categories ctg = new Categories();
            ctg.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Billing bill = new Billing();
            bill.Show();
            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}