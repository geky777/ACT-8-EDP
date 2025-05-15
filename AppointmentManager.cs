using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace GuiForDentalA
{
    public static class AppointmentManager
    {
        // Get appointments for the currently logged-in user
        public static List<Appointment> GetAppointmentsForCurrentUser()
        {
            var appointments = new List<Appointment>();

            string query = @"SELECT appointment_id, patient_id, dentist_id, appointment_date, status, notes 
                             FROM appointments 
                             WHERE patient_id = @PatientId";

            try
            {
                using (var reader = DatabaseHelper.ExecuteQuery(query,
                    new MySqlParameter("@PatientId", UserSession.CurrentUserId)))
                {
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentId = Convert.ToInt32(reader["appointment_id"]),
                            PatientId = Convert.ToInt32(reader["patient_id"]),
                            DentistId = Convert.ToInt32(reader["dentist_id"]),
                            AppointmentDate = Convert.ToDateTime(reader["appointment_date"]),
                            Status = reader["status"].ToString() ?? "",
                            Notes = reader["notes"].ToString() ?? ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching appointments: {ex.Message}");
            }

            return appointments;
        }

    

        // Delete appointment
        public static void DeleteAppointment(int appointmentId)
        {
            string query = "DELETE FROM appointments WHERE appointment_id = @AppointmentId";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@AppointmentId", appointmentId));
        }
    }
}
