using MySql.Data.MySqlClient;

class DeliveryStatusUpdater
{
    private static string connectionString = "your_connection_string_here";

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
                string checkQuery = "SELECT COUNT(*) FROM packages WHERE tracking_number = @trackingNumber";
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                    long packageCount = (long)checkCommand.ExecuteScalar();
                    if (packageCount == 0)
                    {
                        Console.WriteLine("Package not found.");
                        return;
                    }
                }

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
                    }
                    else
                    {
                        Console.WriteLine("Failed to update delivery status.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error updating delivery status: {ex.Message}");
        }
    }
}
