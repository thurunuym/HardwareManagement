using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HardwareManagementTool
{
    public partial class Items : Form
    {
        public Items()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadItems()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False"))
            {
                string query = "SELECT * FROM Items";
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

        private void PopulateCategories()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False"))
            {
                string query = "SELECT CategoryName, CategoryID FROM Category";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open(); // Open the connection
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Clear the comboBox1 before adding new items
                    comboBox1.Items.Clear();

                    while (reader.Read())
                    {
                        // Add each CategoryName to the comboBox1
                        comboBox1.Items.Add(new ComboBoxItem(reader["CategoryName"].ToString(), reader["CategoryID"].ToString()));
                    }
                }
            }
        }

        private void Items_Load_1(object sender, EventArgs e)
        {
            PopulateCategories();
            LoadItems();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string itemName = Item_textbox.Text;
            string manufacturer = Manufacturer_textbox.Text;
            string categoryID = comboBox1.SelectedItem != null ? ((ComboBoxItem)comboBox1.SelectedItem).Value : null;
            float price;
            float stock;

            if (string.IsNullOrWhiteSpace(itemName))
            {
                MessageBox.Show("Please enter an item name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(manufacturer))
            {
                MessageBox.Show("Please enter an item manufacturer.");
                return;
            }

            if (categoryID == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            if (!float.TryParse(Price_textbox.Text, out price)) // Converting strings to numeric types
            {
                MessageBox.Show("Please enter a valid price.");
                Price_textbox.Clear();
                return;
            }

            if (!float.TryParse(Stock_textbox.Text, out stock))
            {
                MessageBox.Show("Please enter a valid stock quantity.");
                Stock_textbox.Clear();
                return;
            }

            string itemID = GenerateItemID(); // You need to implement this method to generate a unique ItemID

            using (SqlConnection conn = new SqlConnection("Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False"))
            {
                string query = "INSERT INTO Items (ItemID, ItemName, ItemManufacturer, ItemCategory, ItemPrice, ItemStock) VALUES (@ItemID, @ItemName, @ItemManufacturer, @ItemCategory, @ItemPrice, @ItemStock)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    cmd.Parameters.AddWithValue("@ItemName", itemName);
                    cmd.Parameters.AddWithValue("@ItemManufacturer", manufacturer);
                    cmd.Parameters.AddWithValue("@ItemCategory", categoryID);
                    cmd.Parameters.AddWithValue("@ItemPrice", price);
                    cmd.Parameters.AddWithValue("@ItemStock", stock);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            MessageBox.Show("Item added successfully!", "Add Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadItems();

            //  clear the inputs after successful insertion
            Item_textbox.Clear();
            Manufacturer_textbox.Clear();
            Price_textbox.Clear();
            Stock_textbox.Clear();
            comboBox1.SelectedIndex = -1; //clear the selection in the ComboBox
        }


        private int GetNextItemID()
        {
            // Implement a logic to get the next available ID from the database
            using (SqlConnection conn = new SqlConnection("Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False"))
            {
                string query = "SELECT MAX(CAST(SUBSTRING(ItemID, 4, LEN(ItemID) - 3) AS INT)) + 1 AS NextID FROM Items";
                //format - SUBSTRING(expression, start, length)
                //CAST(...AS INT): This converts the extracted substring to an integer
                //it will return a result set with one column, and that column will be named NextID.


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    conn.Close();

                    return result != DBNull.Value ? Convert.ToInt32(result) : 1;
                    //This is a ternary operator in C#, which is a shorthand way of writing an if-else statement.
                    //condition ? expression_if_true : expression_if_false
                    //Convert.ToInt32() is a method in C# that converts a value to a 32-bit signed integer.
                }
            }
        }
        private string GenerateItemID()
        {
            // Implement a logic to generate a unique ItemID, e.g., "ITM001", "ITM002"

            int nextID = GetNextItemID();
            return "ITM" + nextID.ToString("D3");
        }

        private void Price_textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {
            Categories ctg = new Categories();
            ctg.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Billing bl = new Billing();
            bl.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }



    public class ComboBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public ComboBoxItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
