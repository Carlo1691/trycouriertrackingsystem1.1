using MySql.Data.MySqlClient;

class RealTimeStatusChecker
{
    private static string connectionString = "your_connection_string_here";

    public static void RealtimeStatus()
    {
        Console.Clear();
        Console.WriteLine("--- Real-time Status ---");
        Console.Write("Enter Tracking Number: ");
        string trackingNumber = Console.ReadLine();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT status, dispatch_date, updated_at
                    FROM packages
                    WHERE tracking_number = @trackingNumber";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine($"Tracking Number: {trackingNumber}");
                            while (reader.Read())
                            {
                                Console.WriteLine($"Status: {reader["status"]}");
                                Console.WriteLine($"Dispatched On: {reader.GetDateTime("dispatch_date")}");
                                if (!reader.IsDBNull(reader.GetOrdinal("updated_at")))
                                {
                                    Console.WriteLine($"Last Updated On: {reader.GetDateTime("updated_at")}");
                                }
                                Console.WriteLine("--------------------");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Package not found.");
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error checking status: {ex.Message}");
        }
    }
}
