using System;
using MySql.Data.MySqlClient;

namespace CourierTrackingSystem
{
    class Program
    {
        private static string connectionString = "server=localhost;user id=root;password=;database=courier_db";
        private static string loggedInUsername = null;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Courier Tracking System ---");
                Console.WriteLine();

                if (loggedInUsername == null)
                {
                    Console.WriteLine("1. Register");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3. Exit");
                }
                else
                {
                    Console.WriteLine("1. Package Dispatch");
                    Console.WriteLine("2. Customer Database");
                    Console.WriteLine("3. Real Time Status");
                    Console.WriteLine("4. Personnel Assignment");
                    Console.WriteLine("5. Update Delivery Status");
                    Console.WriteLine("6. Logout");
                    Console.WriteLine("7. Exit");
                }

                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (loggedInUsername == null)
                        {
                            UserRegistration.Register();
                        }
                        else
                        {
                            PackageDispatch();
                        }
                        break;
                    case "2":
                        if (loggedInUsername == null)
                        {
                            Login();
                        }
                        else
                        {
                            CustomerDatabaseManager.CustomerDatabase();
                        }
                        break;
                    case "3":
                        if (loggedInUsername == null)
                        {
                            Exit();
                        }
                        else
                        {
                            RealTimeStatusChecker.RealtimeStatus();
                        }
                        break;
                    case "4":
                        PersonnelAssignmentManager.PersonnelAssignment();
                        break;
                    case "5":
                        DeliveryStatusUpdater.UpdateDeliveryStatus();
                        break;
                    case "6":
                        Logout();
                        break;
                    case "7":
                        Exit();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("--- Login ---");
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

        private static void Logout()
        {
            loggedInUsername = null;
            Console.WriteLine("Logged out successfully.");
        }

        private static void Exit()
        {
            Environment.Exit(0);
        }

        public static void PackageDispatch()
        {
            Console.Clear();
            Console.WriteLine("--- Package Dispatch ---");
            Console.Write("Enter Tracking Number: ");
            string trackingNumber = Console.ReadLine();
            Console.Write("Enter Sender: ");
            string sender = Console.ReadLine();
            Console.Write("Enter Receiver Email: ");
            string recipientEmail = Console.ReadLine();
            Console.Write("Enter Receiver: ");
            string recipient = Console.ReadLine();
            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO packages (tracking_number, sender, recipient, recipient_email, destination, status, dispatch_date) VALUES (@trackingNumber, @sender, @recipient, @recipientEmail, @destination, 'Dispatched', NOW())";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                        command.Parameters.AddWithValue("@sender", sender);
                        command.Parameters.AddWithValue("@recipient", recipient);
                        command.Parameters.AddWithValue("@recipientEmail", recipientEmail);
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
}
