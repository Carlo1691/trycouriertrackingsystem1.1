using MySql.Data.MySqlClient;

class PersonnelAssignmentManager
{
    private static string connectionString = "server=localhost;user id=root;password=;database=courier_db";

    public static void PersonnelAssignment()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Personnel Assignment ---");
            Console.WriteLine("1. View Packages Awaiting Assignment");
            Console.WriteLine("2. Assign Personnel to Package");
            Console.WriteLine("3. View Assigned Personnel for a Package");
            Console.WriteLine("4. Add Personnel");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewUnassignedPackages();
                    break;
                case "2":
                    AssignPersonnelToPackage();
                    break;
                case "3":
                    ViewAssignedPersonnelForPackage();
                    break;
                case "4":
                    AddPersonnel();
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

    private static void ViewUnassignedPackages()
    {
        Console.Clear();
        Console.WriteLine("--- Packages Awaiting Assignment ---");

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT p.tracking_number, p.recipient, p.destination
                    FROM packages p
                    LEFT JOIN package_personnel pp ON p.tracking_number = pp.tracking_number
                    WHERE pp.personnel_id IS NULL";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Tracking Number: {reader["tracking_number"]}, Recipient: {reader["recipient"]}, Destination: {reader["destination"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No packages currently awaiting assignment.");
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error viewing unassigned packages: {ex.Message}");
        }
    }

    private static void AssignPersonnelToPackage()
    {
        Console.Clear();
        Console.WriteLine("--- Assign Personnel to Package ---");
        Console.Write("Enter Tracking Number of the package: ");
        string trackingNumber = Console.ReadLine();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the package exists
                string checkPackageQuery = "SELECT COUNT(*) FROM packages WHERE tracking_number = @trackingNumber";
                using (MySqlCommand checkPackageCommand = new MySqlCommand(checkPackageQuery, connection))
                {
                    checkPackageCommand.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                    if ((long)checkPackageCommand.ExecuteScalar() == 0)
                    {
                        Console.WriteLine("Package not found.");
                        return;
                    }
                }

                Console.WriteLine("\nAvailable Personnel:");
                string personnelQuery = "SELECT id, name FROM personnel";
                using (MySqlCommand personnelCommand = new MySqlCommand(personnelQuery, connection))
                {
                    using (MySqlDataReader personnelReader = personnelCommand.ExecuteReader())
                    {
                        if (personnelReader.HasRows)
                        {
                            while (personnelReader.Read())
                            {
                                Console.WriteLine($"{personnelReader["id"]}. {personnelReader["name"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No personnel available.");
                            return;
                        }
                    }
                }

                Console.Write("Enter the ID of the personnel to assign: ");
                if (!int.TryParse(Console.ReadLine(), out int personnelId))
                {
                    Console.WriteLine("Invalid personnel ID format.");
                    return;
                }

                // Check if the personnel exists
                string checkPersonnelQuery = "SELECT COUNT(*) FROM personnel WHERE id = @personnelId";
                using (MySqlCommand checkPersonnelCommand = new MySqlCommand(checkPersonnelQuery, connection))
                {
                    checkPersonnelCommand.Parameters.AddWithValue("@personnelId", personnelId);
                    if ((long)checkPersonnelCommand.ExecuteScalar() == 0)
                    {
                        Console.WriteLine("Personnel not found.");
                        return;
                    }
                }

                string assignQuery = "INSERT INTO package_personnel (tracking_number, personnel_id) VALUES (@trackingNumber, @personnelId)";
                using (MySqlCommand assignCommand = new MySqlCommand(assignQuery, connection))
                {
                    assignCommand.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                    assignCommand.Parameters.AddWithValue("@personnelId", personnelId);
                    int rowsAffected = assignCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Personnel assigned to package successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to assign personnel to package.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error assigning personnel to package: {ex.Message}");
        }
    }

    private static void ViewAssignedPersonnelForPackage()
    {
        Console.Clear();
        Console.WriteLine("--- View Assigned Personnel for Package ---");
        Console.Write("Enter Tracking Number of the package: ");
        string trackingNumber = Console.ReadLine();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT p.tracking_number, p.recipient, p.destination, pr.name AS personnel_name
                    FROM packages p
                    JOIN package_personnel pp ON p.tracking_number = pp.tracking_number
                    JOIN personnel pr ON pp.personnel_id = pr.id
                    WHERE p.tracking_number = @trackingNumber";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@trackingNumber", trackingNumber);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Tracking Number: {reader["tracking_number"]}, Recipient: {reader["recipient"]}, Destination: {reader["destination"]}, Assigned Personnel: {reader["personnel_name"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No personnel assigned to this package.");
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error viewing assigned personnel for package: {ex.Message}");
        }
    }

    private static void AddPersonnel()
    {
        Console.Clear();
        Console.WriteLine("--- Add Personnel ---");
        Console.Write("Enter Personnel ID: ");
        if (!int.TryParse(Console.ReadLine(), out int personnelId))
        {
            Console.WriteLine("Invalid personnel ID format.");
            return;
        }
        Console.Write("Enter Personnel Name: ");
        string name = Console.ReadLine();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO personnel (id, name) VALUES (@id, @name)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", personnelId);
                    command.Parameters.AddWithValue("@name", name);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Personnel added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add personnel.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error adding personnel: {ex.Message}");
        }
    }
}


