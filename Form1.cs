using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GuiForDentalA;
using MySql.Data.MySqlClient;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ClosedXML.Excel;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Label welcomeLabel;

        public Form1()
        {
            InitializeComponent();
            welcomeLabel = new Label(); // Initialize the field
            DatabaseHelper.TestDatabaseConnection(); // Test the database connection
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch all patients and display in DataGridView
                string query = "SELECT * FROM patients";
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch all appointments and display in DataGridView
                string query = "SELECT * FROM appointments";
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch all dentists and display in DataGridView
                string query = "SELECT * FROM dentists";
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch all payments and display in DataGridView
                string query = "SELECT * FROM payments";
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch all appointment logs and display in DataGridView
                string query = "SELECT * FROM appointment_logs";
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            // Log out and redirect to the login form
            Form2 loginForm = new Form2();
            this.Hide();
            loginForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)

        {
            try
            {
                // Collect details from the textboxes
                string firstName = textBox1.Text.Trim();
                string lastName = textBox3.Text.Trim();
                string phone = textBox2.Text.Trim();
                string email = textBox4.Text.Trim();
                string specializationId = textBox5.Text.Trim();

                // Validate inputs
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                    string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(specializationId))
                {
                    MessageBox.Show("All fields are required. Please provide valid inputs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL query to insert dentist details
                string query = "INSERT INTO dentists (first_name, last_name, phone, email, specialization_id) VALUES (@FirstName, @LastName, @Phone, @Email, @SpecializationId)";

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query,
                    new MySqlParameter("@FirstName", firstName),
                    new MySqlParameter("@LastName", lastName),
                    new MySqlParameter("@Phone", phone),
                    new MySqlParameter("@Email", email),
                    new MySqlParameter("@SpecializationId", specializationId));

                // Check if the insertion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Dentist added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox1.Clear();
                    textBox3.Clear();
                    textBox2.Clear();
                    textBox4.Clear();
                    textBox5.Clear();

                }
                else
                {
                    MessageBox.Show("Failed to add dentist. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }


        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show($"Cell clicked at row {e.RowIndex}, column {e.ColumnIndex}.");
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Form1 loaded successfully.");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM payment_summary;";
            using (var reader = DatabaseHelper.ExecuteQuery(query))
            {
                DataTable table = new DataTable();
                table.Load(reader);
                dataGridView1.DataSource = table;
            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch data from the DataGridView
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Assume the table name is stored in a variable (you can adjust this logic as needed)
                string tableName = "exported_table"; // Default table name
                if (dataGridView1.DataSource is DataTable dataTableSource)
                {
                    tableName = dataTableSource.TableName; // Use the table name if available
                }

                // Create a DataTable from the DataGridView
                DataTable dataTable = new DataTable();
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    dataTable.Columns.Add(column.HeaderText); // Add column headers to the DataTable
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dataRow[cell.ColumnIndex] = cell.Value?.ToString(); // Add cell values to the DataTable
                    }
                    dataTable.Rows.Add(dataRow);
                }

                // Prompt the user to select a save location
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save as Excel File";
                    saveFileDialog.FileName = $"{tableName}.xlsx"; // Set file name to the table name

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Create an Excel file using ClosedXML
                        using (var workbook = new ClosedXML.Excel.XLWorkbook())
                        {
                            workbook.Worksheets.Add(dataTable, tableName); // Name the worksheet after the table
                            workbook.SaveAs(saveFileDialog.FileName);
                        }

                        MessageBox.Show("Data exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect details from the textboxes
                string dentistId = textBox6.Text.Trim(); 
                string firstName = textBox1.Text.Trim();
                string lastName = textBox3.Text.Trim();
                string phone = textBox2.Text.Trim();
                string email = textBox4.Text.Trim();
                string specializationId = textBox5.Text.Trim();

                // Validate inputs
                if (string.IsNullOrWhiteSpace(dentistId))
                {
                    MessageBox.Show("Dentist ID is required to update the record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) &&
                    string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email) &&
                    string.IsNullOrWhiteSpace(specializationId))
                {
                    MessageBox.Show("At least one field must be provided to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Build the SQL query dynamically based on provided inputs
                string query = "UPDATE dentists SET ";
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    query += "first_name = @FirstName, ";
                    parameters.Add(new MySqlParameter("@FirstName", firstName));
                }
                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    query += "last_name = @LastName, ";
                    parameters.Add(new MySqlParameter("@LastName", lastName));
                }
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    query += "phone = @Phone, ";
                    parameters.Add(new MySqlParameter("@Phone", phone));
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    query += "email = @Email, ";
                    parameters.Add(new MySqlParameter("@Email", email));
                }
                if (!string.IsNullOrWhiteSpace(specializationId))
                {
                    query += "specialization_id = @SpecializationId, ";
                    parameters.Add(new MySqlParameter("@SpecializationId", specializationId));
                }

                // Remove the trailing comma and space
                query = query.TrimEnd(',', ' ');

                // Add the WHERE clause to target the specific dentist
                query += " WHERE dentist_id = @DentistId";
                parameters.Add(new MySqlParameter("@DentistId", dentistId));

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());

                // Check if the update was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Dentist details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally clear the textboxes
                    textBox6.Clear(); // Dentist ID
                    textBox1.Clear(); // First Name
                    textBox3.Clear(); // Last Name
                    textBox2.Clear(); // Phone
                    textBox4.Clear(); // Email
                    textBox5.Clear(); // Specialization ID
                }
                else
                {
                    MessageBox.Show("No records were updated. Please check the Dentist ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect Patient ID from the textbox
                string patientId = textBox6.Text.Trim();

                // Validate input
                if (string.IsNullOrWhiteSpace(patientId))
                {
                    MessageBox.Show("Patient ID is required to delete a record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the Patient with ID {patientId}?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }

                // SQL query to delete patient by ID
                string query = "DELETE FROM patients WHERE patient_id = @PatientId";

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@PatientId", patientId));

                // Check if the deletion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Patient deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the Patient ID textbox
                    textBox6.Clear();
                }
                else
                {
                    MessageBox.Show("No record was found with the given Patient ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect Dentist ID from the textbox
                string dentistId = textBox6.Text.Trim(); // Assuming textBox6 is used to input Dentist ID

                // Validate input
                if (string.IsNullOrWhiteSpace(dentistId))
                {
                    MessageBox.Show("Dentist ID is required to delete a record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show($"Are you sure you want to delete Dentist with ID {dentistId}?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }

                // SQL query to delete dentist by ID
                string query = "DELETE FROM dentists WHERE dentist_id = @DentistId";

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@DentistId", dentistId));

                // Check if the deletion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Dentist deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the Dentist ID textbox
                    textBox6.Clear();
                }
                else
                {
                    MessageBox.Show("No record was found with the given Dentist ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }


        }
    }
}
