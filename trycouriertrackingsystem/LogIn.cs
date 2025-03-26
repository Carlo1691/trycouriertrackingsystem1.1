using MySql.Data.MySqlClient;

class UserLogin
{
    private static string connectionString = "your_connection_string_here";
    private static string loggedInUsername = null;

    public static void LogIn()
    {
        Console.Clear();
        Console.WriteLine("--- Log In ---");
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT username FROM users WHERE username = @username AND password = @password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        loggedInUsername = result.ToString();
                        Console.WriteLine($"Logged in successfully, {loggedInUsername}.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"An error occurred logging in: {ex.Message}");
        }
    }
}


