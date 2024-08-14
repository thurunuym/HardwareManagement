using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace HardwareManagementTool
{
    public partial class Categories : Form
    {
        public Categories()
        {
            InitializeComponent();
        }

        private void Categories_Load_1(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Category_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Use 'using' statements to ensure proper disposal of resources
            using (SqlConnection conn = new SqlConnection("Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False"))
            {
                // Generate the CategoryID 
                string categoryID = GenerateCategoryID();

                // Check if the categorytb text is null or empty
                if (string.IsNullOrWhiteSpace(categorytb.Text))
                {
                    MessageBox.Show("Please enter a category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return; // Exit the method if validation fails
                }

                // Connection object is passed to the SqlCommand constructor to associate the command with the connection.
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO [dbo].[Category]([CategoryID], [CategoryName]) VALUES (@CategoryID, @CategoryName)", conn))
                {
                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@CategoryName", categorytb.Text);

                    try
                    {
                        conn.Open(); // Open the connection
                        cmd.ExecuteNonQuery(); // Execute the command
                        MessageBox.Show("Category added successfully!", "Add New Category", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        categorytb.Clear();
                        LoadCategories();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
       
        private string GenerateCategoryID()
        {
            string categoryID = "C001"; // Default ID
            using (SqlConnection conn = new SqlConnection("Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False"))
            {
                conn.Open();
                string query = "SELECT MAX(CategoryID) FROM Category"; // select the maximum value of CategoryID from the Category table
                SqlCommand cmd = new SqlCommand(query, conn);
                var result = cmd.ExecuteScalar();//called on the command to execute the query and retrieve the result, which is stored in the result variable.

                if (result != DBNull.Value)
                {
                    string maxID = result.ToString();
                    int newIDNumber = int.Parse(maxID.Substring(1)) + 1; // Extract number and increment
                    categoryID = "C" + newIDNumber.ToString("D3"); // This ensures that the ID is always 3 digits long as C001, C002, etc.
                }
            }
            return categoryID;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False"))
            {
                string query = "SELECT * FROM Category";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open(); // Open the connection
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt); // Fill the DataTable with the results

                    dataGridView1.DataSource = dt; // Bind the DataTable to the DataGridView
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Items itm = new Items();
            itm.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False"))
            {
                // Check if the categorytb text is null or empty
                if (string.IsNullOrWhiteSpace(categorytb.Text))
                {
                    MessageBox.Show("Please enter a category to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return; // Exit the method if validation fails
                }

                // Connection object is passed to the SqlCommand constructor to associate the command with the connection.
                string query = "DELETE FROM Category WHERE CategoryName = @CategoryName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", categorytb.Text);

                    try
                    {
                        conn.Open(); // Open the connection
                        int rowsDeleted = cmd.ExecuteNonQuery(); // Execute the command

                        if (rowsDeleted > 0)
                        {
                            MessageBox.Show("Category deleted successfully!", "Delete Category", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            categorytb.Clear();
                            LoadCategories(); // Refresh the categories list
                        }
                        else
                        {
                            MessageBox.Show("Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                
            }
        }
    }

        private void label10_Click(object sender, EventArgs e)
        {
            Billing bl = new Billing();
            bl.Show();
            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
        
        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}