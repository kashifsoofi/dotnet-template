#r "nuget: MySql.Data, 8.0.17"

using System.Threading;
using MySql.Data.MySqlClient;

string connectionString = Args[0];
Console.WriteLine("Connection String=[" + connectionString + "]");
int retries = 120;
int interval = 500;
MySqlConnection connection = null;
for (int i = 0; i < retries; i++) {
    try {
        connection = new MySqlConnection(connectionString);
        connection.Open();
        Console.WriteLine("Database connected");
        connection.Close();
        Environment.Exit(0);
    }
    catch (MySqlException) {
        Console.WriteLine("Cannot establish connection to the DB");
    }
    Thread.Sleep(interval);
}
Console.WriteLine("Failed to connect to the DB, QUIT!");
Environment.Exit(1);