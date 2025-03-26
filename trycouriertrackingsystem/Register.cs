using MySql.Data.MySqlClient;

class UserRegistration
{
    private static string connectionString = "your_connection_string_here";

    public static void Register()
    {
        Console.Clear();
        Console.WriteLine("--- Register ---");
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();
        Console.Write("Confirm Password: ");
        string confirmPassword = Console.ReadLine();

        if (password != confirmPassword)
        {
            Console.WriteLine("Passwords do not match.");
            return;
        }

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();

                    Console.WriteLine("User registered successfully.");
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error registering user: {ex.Message}");
        }
    }
}
