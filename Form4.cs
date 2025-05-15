using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using System.Data;

namespace GuiForDentalA
{
    public partial class Form4 : Form
    {
        private Label welcomeLabel;

        public Form4()
        {
            InitializeComponent();
            DatabaseHelper.TestDatabaseConnection();
            InitializeWelcomeLabel();
        }

        // Display welcome message
        private void InitializeWelcomeLabel()
        {
            try
            {
                welcomeLabel = new Label
                {
                    AutoSize = false,
                    Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Bold),
                    Location = new Point(150, 20),
                    Name = "welcomeLabel",
                    Size = new Size(600, 60),
                    Text = $"Welcome, {UserSession.FirstName} {UserSession.LastName}!",
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.LightBlue,
                    ForeColor = Color.DarkBlue,
                    BorderStyle = BorderStyle.FixedSingle
                };
                this.Controls.Add(welcomeLabel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load appointments into dataGridView1 when it is clicked
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Add Appointment
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect input values from the textboxes and the date picker
                string dentistId = textBox2.Text.Trim();      // Dentist ID
                string status = textBox3.Text.Trim();         // Status
                string appointmentDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Format date as yyyy-MM-dd
                string patientId = textBox5.Text.Trim();      // Patient ID
                string notes = textBox4.Text.Trim();          // Notes

                // Validate inputs
                if (string.IsNullOrWhiteSpace(dentistId) || string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(patientId))
                {
                    MessageBox.Show("Dentist ID, Patient ID, and Status are required fields. Please provide valid inputs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL query to insert the new appointment
                string query = "INSERT INTO appointments (dentist_id, patient_id, status, appointment_date, notes) VALUES (@DentistId, @PatientId, @Status, @AppointmentDate, @Notes)";

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query,
                    new MySqlParameter("@DentistId", dentistId),
                    new MySqlParameter("@PatientId", patientId),
                    new MySqlParameter("@Status", status),
                    new MySqlParameter("@AppointmentDate", appointmentDate),
                    new MySqlParameter("@Notes", notes));

                // Check if the insertion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Appointment added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally clear the input fields
                    textBox2.Clear(); // Dentist ID
                    textBox3.Clear(); // Status
                    textBox4.Clear(); // Notes
                    textBox5.Clear(); // Patient ID
                    dateTimePicker1.Value = DateTime.Now; // Reset to today's date
                }
                else
                {
                    MessageBox.Show("Failed to add appointment. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect input values from the textboxes and the date picker
                string appointmentId = textBox1.Text.Trim();  // Appointment ID
                string dentistId = textBox2.Text.Trim();      // Dentist ID
                string status = textBox3.Text.Trim();         // Status
                string appointmentDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Format date as yyyy-MM-dd
                string notes = textBox4.Text.Trim();          // Notes

                // Validate inputs
                if (string.IsNullOrWhiteSpace(appointmentId))
                {
                    MessageBox.Show("Appointment ID is required to update an appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(dentistId) && string.IsNullOrWhiteSpace(status) && string.IsNullOrWhiteSpace(appointmentDate) && string.IsNullOrWhiteSpace(notes))
                {
                    MessageBox.Show("At least one field (Dentist ID, Status, Date, or Notes) must be filled to update the appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Build the SQL query dynamically based on provided inputs
                string query = "UPDATE appointments SET ";
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(dentistId))
                {
                    query += "dentist_id = @DentistId, ";
                    parameters.Add(new MySqlParameter("@DentistId", dentistId));
                }
                if (!string.IsNullOrWhiteSpace(status))
                {
                    query += "status = @Status, ";
                    parameters.Add(new MySqlParameter("@Status", status));
                }
                if (!string.IsNullOrWhiteSpace(appointmentDate))
                {
                    query += "appointment_date = @AppointmentDate, ";
                    parameters.Add(new MySqlParameter("@AppointmentDate", appointmentDate));
                }
                if (!string.IsNullOrWhiteSpace(notes))
                {
                    query += "notes = @Notes, ";
                    parameters.Add(new MySqlParameter("@Notes", notes));
                }

                // Remove the trailing comma and space
                query = query.TrimEnd(',', ' ');

                // Add the WHERE clause to target the specific appointment
                query += " WHERE appointment_id = @AppointmentId";
                parameters.Add(new MySqlParameter("@AppointmentId", appointmentId));

                // Execute the query using DatabaseHelper
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters.ToArray());

                // Check if the update was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Appointment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally, clear the input fields (but retain the selected DateTimePicker value)
                    textBox1.Clear(); // Appointment ID
                    textBox2.Clear(); // Dentist ID
                    textBox3.Clear(); // Status
                    textBox4.Clear(); // Notes
                }
                else
                {
                    MessageBox.Show("No records were updated. Please check the Appointment ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        // Delete Appointment
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get the selected appointment
                var selectedRow = dataGridView1.SelectedRows[0];
                var appointment = (Appointment)selectedRow.DataBoundItem;

                // Confirm deletion
                var confirmResult = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    // Call the DeleteAppointment method
                    AppointmentManager.DeleteAppointment(appointment.AppointmentId);

                    MessageBox.Show("Appointment deleted successfully.");
                    LoadAppointments(); // Refresh the grid
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // View Appointments (for the logged-in user)
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch appointments for the logged-in user
                List<Appointment> appointments = AppointmentManager.GetAppointmentsForCurrentUser();

                if (appointments.Count == 0)
                {
                    MessageBox.Show("No appointments found for the logged-in user.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dataGridView1.DataSource = appointments;
                    MessageBox.Show("Appointments loaded successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to load appointments (used after Add, Update, or Delete)
        private void LoadAppointments()
        {
            try
            {
                List<Appointment> appointments = AppointmentManager.GetAppointmentsForCurrentUser();
                dataGridView1.DataSource = appointments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // SQL query to fetch all data from dentist_contact_info table
                string query = "SELECT * FROM dental_clinic.dentist_contact_info";

                // Execute the query using DatabaseHelper and fetch the data
                using (var reader = DatabaseHelper.ExecuteQuery(query))
                {
                    // Create a new DataTable object
                    DataTable table = new DataTable();

                    // Load the data from the reader into the DataTable
                    table.Load(reader);

                    // Clear any existing data in the DataGridView
                    dataGridView1.DataSource = null;

                    // Bind the DataTable to the DataGridView for display
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                // Display an error message if something goes wrong
                MessageBox.Show($"An error occurred while fetching data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the database connection is closed
                DatabaseHelper.CloseConnection();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //appointment id 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //dentist id 
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //status 
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //date
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_2(object sender, EventArgs e)
        {
           
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Clear the UserSession
            UserSession.ClearSession();

            // Log out message to the console (optional)
            Console.WriteLine("User has been logged out.");

            // Redirect to the login form (Form2)
            Form2 loginForm = new Form2();
            loginForm.Show();

            // Close the current form
            this.Close();
        }
    }
}