using MySql.Data.MySqlClient;

class PackageDispatcher
{
    private static string connectionString = "your_connection_string_here";

    public static void PackageDispatch()
    {
        Console.Clear();
        Console.WriteLine("--- Package Dispatch ---");
        Console.Write("Enter Tracking Number: ");
        string trackingNumber = Console.ReadLine();
        Console.Write("Enter Sender: ");
        string sender = Console.ReadLine();
        Console.Write("Enter Receiver: ");
        string recipient = Console.ReadLine();
        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO packages (tracking_number, sender, recipient, destination, status, dispatch_date) VALUES (@trackingNumber, @sender, @recipient, @destination, 'Dispatched', NOW())";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                    command.Parameters.AddWithValue("@sender", sender);
                    command.Parameters.AddWithValue("@recipient", recipient);
                    command.Parameters.AddWithValue("@destination", destination);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Package dispatched successfully.");
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"An error occurred dispatching package: {ex.Message}");
        }
    }
}

