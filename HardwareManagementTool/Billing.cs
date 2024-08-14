
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;


namespace HardwareManagementTool
{

    public partial class Billing : Form
    {

        private string connectionString = "Data Source=THURUNU_YM\\SQLEXPRESS;Initial Catalog=HardwareDB;Integrated Security=True;Encrypt=False";

        public Billing()
        {
            InitializeComponent();
            initializeDataGridView();

        }

        private void initializeDataGridView()
        {
            dataGridView1.Columns.Add("No", "");

            dataGridView1.Columns.Add("Item", "Item-Manufacturer");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("Total", "Total");
        }

        private void AddToDataGridView()
        {
            string item = comboBox2.Text;
            float price = float.Parse(price_tbb.Text);
            float quantity = float.Parse(quantity_tb.Text);
            float total = price * quantity;

            //adding row number
            int rowNumber = dataGridView1.Rows.Count;
            string formattedRowNumber = rowNumber.ToString("D2");
            dataGridView1.Rows.Add(formattedRowNumber, item, price.ToString("F2"), quantity.ToString("F2"), total.ToString("F2"));

        }

        private void ClearData()
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            quantity_tb.Text = "";
            label6.Text = "";
        }

        // Load categories into comboBox1 when the form loads
        private void Billing_Load_1(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CategoryID, CategoryName FROM Category";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBox1.Items.Clear(); // Clear existing items

                    // Create a new AutoCompleteStringCollection
                    AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();

                    while (reader.Read())
                    {
                        string categoryName = reader["CategoryName"].ToString();
                        comboBox1.Items.Add(new ComboBoxItemm(categoryName, reader["CategoryID"].ToString()));
                        autoCompleteCollection.Add(categoryName); // Add to autocomplete collection
                    }

                    comboBox1.DisplayMember = "Text";
                    comboBox1.ValueMember = "Value";

                    // Set AutoComplete properties
                    comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    comboBox1.AutoCompleteCustomSource = autoCompleteCollection;
                }
            }
        }

        // Load related items into comboBox2 based on selected category
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedCategoryID = ((ComboBoxItemm)comboBox1.SelectedItem).Value;
                LoadItems(selectedCategoryID);
            }
        }

        private void LoadItems(string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ItemID, ItemName, ItemManufacturer FROM Items WHERE ItemCategory = @CategoryID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBox2.Items.Clear(); // Clear existing items
                    AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();


                    while (reader.Read())
                    {
                        // Format: ItemName - ItemManufacturer with a space around the hyphen
                        string displayText = $"{reader["ItemName"]} - {reader["ItemManufacturer"]}";
                        comboBox2.Items.Add(new ComboBoxItemm(displayText, reader["ItemID"].ToString()));
                        autoCompleteCollection.Add(displayText); // Add to autocomplete collection
                    }

                    comboBox2.DisplayMember = "Text";
                    comboBox2.ValueMember = "Value";

                    // Set AutoComplete properties
                    comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    comboBox2.AutoCompleteCustomSource = autoCompleteCollection;
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Please select an item.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the quantity is a valid float
            if (!float.TryParse(quantity_tb.Text, out float quantity))
            {
                MessageBox.Show("Please enter a valid number for quantity.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            AddToDataGridView();
            ClearData();
        }

        // Other event handlers remain unchanged
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label5_Click_1(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label2_Click_1(object sender, EventArgs e) { }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                // Get the selected item's ID
                string selectedItemID = ((ComboBoxItemm)comboBox2.SelectedItem).Value;

                // Load and display the item price
                LoadItemPrice(selectedItemID);
            }
        }

        private void LoadItemPrice(string itemID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ItemPrice FROM Items WHERE ItemID = @ItemID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    conn.Open();
                    object result = cmd.ExecuteScalar(); // Use ExecuteScalar to get a single value

                    if (result != null && result != DBNull.Value)
                    //DBNull.Value is a special singleton object used to represent a null value in the database.
                    {
                        // Convert.ToSingle() method is a built-in .NET method used to convert an object to a float.
                        float itemPrice = Convert.ToSingle(result);

                        // Update price_tbb label with the price
                        price_tbb.Text = itemPrice.ToString("F2"); // Format to 2 decimal places
                    }
                    else
                    {
                        price_tbb.Text = "N/A"; // Handle case where item price is not found
                    }
                }
            }
        }

        private bool IsGroupBoxSelected()
        {
            // Check if any RadioButton or CheckBox in groupBox1 is selected
            foreach (Control control in groupBox1.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                    return true;
                if (control is CheckBox checkBox && checkBox.Checked)
                    return true;
            }
            return false;
        }

        private void ExportDataGridViewToPdf(string filePath)
        {
            try
            {
                // Create a PDF document with A4 page size and margins
                using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f))
                {
                    // Use PdfWriter to write the document to the file stream
                    using (PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create)))
                    {
                        pdfDoc.Open();

                        // Add Title
                        Paragraph title = new Paragraph("Invoice");
                        title.Alignment = Element.ALIGN_CENTER;
                        pdfDoc.Add(title);

                        // Add a line break
                        pdfDoc.Add(new Paragraph("\n"));

                        // Add current date and time
                        Paragraph date = new Paragraph("Date: " + DateTime.Now.ToString("g"));
                        date.Alignment = Element.ALIGN_RIGHT;
                        pdfDoc.Add(date);

                        // Add another line break
                        pdfDoc.Add(new Paragraph("\n"));

                        // Create a table with the same number of columns as the DataGridView
                        PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                        table.WidthPercentage = 100;

                        // Set column widths (optional)
                        float[] widths = new float[] { 1f, 3f, 2f, 2f, 2f }; // Adjust based on your columns
                        table.SetWidths(widths);

                        // Add headers
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }

                        // Add data rows
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue;

                            foreach (DataGridViewCell dgvCell in row.Cells)
                            {
                                string cellText = dgvCell.Value != null ? dgvCell.Value.ToString() : string.Empty;
                                PdfPCell cell = new PdfPCell(new Phrase(cellText));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell);
                            }
                        }

                        pdfDoc.Add(table);

                        // Add total amount
                        float totalAmount = 0f;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue;
                            float.TryParse(row.Cells["Total"].Value?.ToString(), out float rowTotal);
                            totalAmount += rowTotal;
                        }

                        Paragraph total = new Paragraph("\nTotal Amount: Rs." + totalAmount.ToString("F2"));
                        total.Alignment = Element.ALIGN_RIGHT;
                        pdfDoc.Add(total);

                        // Add payment option
                        Paragraph paymentOption = new Paragraph("\nPayment Option: " + GetSelectedPaymentOption());
                        paymentOption.Alignment = Element.ALIGN_LEFT;
                        pdfDoc.Add(paymentOption);

                        pdfDoc.Close();
                        writer.Close();
                    }
                }

                MessageBox.Show("Data exported successfully to " + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string GetSelectedPaymentOption()
        {
            foreach (Control control in groupBox1.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                    return radioButton.Text;
                if (control is CheckBox checkBox && checkBox.Checked)
                    return checkBox.Text;
            }
            return "None";
        }

        

        private void label9_Click(object sender, EventArgs e)
        {
            Categories ctg = new Categories();
            this.Hide();
            ctg.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Items itm = new Items();
            itm.Show(); 
            this.Hide();    
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            if (!IsGroupBoxSelected())
            {
                MessageBox.Show("Please select a payment option.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Define the file path for the PDF
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Invoice_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

            try
            {
                ExportDataGridViewToPdf(filePath);
                MessageBox.Show("Data exported successfully to " + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class ComboBoxItemm
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public ComboBoxItemm(string text, string value)
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