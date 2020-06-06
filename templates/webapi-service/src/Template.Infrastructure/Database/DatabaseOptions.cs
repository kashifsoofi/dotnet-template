namespace Template.Infrastructure.Database
{
    public class DatabaseOptions
    {
        public string Server { get; set; }
        public int? Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
