using MySql.Data.MySqlClient;
using MailKit.Net.Smtp;
using MimeKit;

class DeliveryStatusUpdater
{
    private static string connectionString = "server=localhost;user id=root;password=;database=courier_db";

    public static void UpdateDeliveryStatus()
    {
        Console.Clear();
        Console.WriteLine("--- Update Delivery Status ---");
        Console.Write("Enter Tracking Number: ");
        string trackingNumber = Console.ReadLine();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the package exists
                string checkQuery = "SELECT recipient_email FROM packages WHERE tracking_number = @trackingNumber";
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                    object result = checkCommand.ExecuteScalar();
                    if (result == null)
                    {
                        Console.WriteLine("Package not found.");
                        return;
                    }

                    string recipientEmail = result.ToString();

                    Console.WriteLine("Available Statuses:");
                    Console.WriteLine("1. In Transit");
                    Console.WriteLine("2. Out for Delivery");
                    Console.WriteLine("3. Delivered");
                    Console.WriteLine("4. Delayed");
                    Console.Write("Select new status (1-4): ");
                    string statusChoice = Console.ReadLine();

                    string newStatus = "";
                    switch (statusChoice)
                    {
                        case "1":
                            newStatus = "In Transit";
                            break;
                        case "2":
                            newStatus = "Out for Delivery";
                            break;
                        case "3":
                            newStatus = "Delivered";
                            break;
                        case "4":
                            newStatus = "Delayed";
                            break;
                        default:
                            Console.WriteLine("Invalid status choice.");
                            return;
                    }

                    string updateQuery = "UPDATE packages SET status = @newStatus, updated_at = NOW() WHERE tracking_number = @trackingNumber";
                    using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@newStatus", newStatus);
                        updateCommand.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Delivery status updated successfully!");
                            SendEmailNotification(recipientEmail, trackingNumber, newStatus);
                        }
                        else
                        {
                            Console.WriteLine("Failed to update delivery status.");
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error updating delivery status: {ex.Message}");
        }
    }

    private static void SendEmailNotification(string recipientEmail, string trackingNumber, string newStatus)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Courier Tracking System", "your_email@gmail.com"));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "Package Delivery Status Update";
            message.Body = new TextPart("plain")
            {
                Text = $"The status of your package with tracking number {trackingNumber} has been updated to: {newStatus}."
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("johncarloyacial6@gmail.com", "zegz nwbu kzwx loaq");
                client.Send(message);
                client.Disconnect(true);
            }

            Console.WriteLine("Email notification sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}

