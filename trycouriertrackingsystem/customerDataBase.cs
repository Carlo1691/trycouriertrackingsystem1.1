using MySql.Data.MySqlClient;

class CustomerDatabaseManager
{
    private static string connectionString = "your_connection_string_here";

    public static void CustomerDatabase()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Customer Database ---");
            Console.WriteLine("1. View Customers");
            Console.WriteLine("2. Add Customer");
            Console.WriteLine("3. Update Customer");
            Console.WriteLine("4. Delete Customer");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewCustomers();
                    break;
                case "2":
                    AddCustomer();
                    break;
                case "3":
                    UpdateCustomer();
                    break;
                case "4":
                    DeleteCustomer();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private static void ViewCustomers()
    {
        Console.Clear();
        Console.WriteLine("--- Customer List ---");

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name, phone, email FROM customers";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}, Phone: {reader["phone"]}, Email: {reader["email"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No customers found.");
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error viewing customers: {ex.Message}");
        }
    }

    private static void AddCustomer()
    {
        Console.Clear();
        Console.WriteLine("--- Add New Customer ---");
        Console.Write("Enter Customer Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Phone Number: ");
        string phone = Console.ReadLine();
        Console.Write("Enter Email Address: ");
        string email = Console.ReadLine();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO customers (name, phone, email) VALUES (@name, @phone, @email)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@email", email);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Customer added successfully!");
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error adding customer: {ex.Message}");
        }
    }

    private static void UpdateCustomer()
    {
        Console.Clear();
        Console.WriteLine("--- Update Customer ---");
        Console.Write("Enter Customer ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the customer exists
                string checkQuery = "SELECT COUNT(*) FROM customers WHERE id = @customerId";
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@customerId", customerId);
                    long customerCount = (long)checkCommand.ExecuteScalar();
                    if (customerCount == 0)
                    {
                        Console.WriteLine("Customer not found.");
                        return;
                    }
                }

                Console.Write("Enter new Customer Name (leave blank to keep current): ");
                string name = Console.ReadLine();
                Console.Write("Enter new Phone Number (leave blank to keep current): ");
                string phone = Console.ReadLine();
                Console.Write("Enter new Email Address (leave blank to keep current): ");
                string email = Console.ReadLine();

                string updateQuery = "UPDATE customers SET name = COALESCE(@name, name), phone = COALESCE(@phone, phone), email = COALESCE(@email, email) WHERE id = @customerId";
                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@customerId", customerId);
                    updateCommand.Parameters.AddWithValue("@name", string.IsNullOrEmpty(name) ? (object)DBNull.Value : name);
                    updateCommand.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone);
                    updateCommand.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Customer updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update customer.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error updating customer: {ex.Message}");
        }
    }

    private static void DeleteCustomer()
    {
        Console.Clear();
        Console.WriteLine("--- Delete Customer ---");
        Console.Write("Enter Customer ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the customer exists
                string checkQuery = "SELECT COUNT(*) FROM customers WHERE id = @customerId";
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@customerId", customerId);
                    long customerCount = (long)checkCommand.ExecuteScalar();
                    if (customerCount == 0)
                    {
                        Console.WriteLine("Customer not found.");
                        return;
                    }
                }

                string deleteQuery = "DELETE FROM customers WHERE id = @customerId";
                using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@customerId", customerId);
                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Customer deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete customer.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error deleting customer: {ex.Message}");
        }
    }
}
