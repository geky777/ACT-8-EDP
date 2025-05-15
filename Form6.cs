using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GuiForDentalA
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            DatabaseHelper.TestDatabaseConnection();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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

                    // Clear the textboxes after successful insertion
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
            Form1 form1 = new Form1();
            this.Close();
            form1.Show();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // email
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // phone
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // first name
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // last name
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Close();
            form1.Show();
        }
    }
}