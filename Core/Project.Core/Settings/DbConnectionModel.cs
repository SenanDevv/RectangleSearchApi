namespace Project.Core.Settings
{
    public class DbConnectionModel
    {
        public string ServerName { get; set; }

        public string Database { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool TrustedConnection { get; set; }

        public override string ToString()
        {
            return $"Data Source={ServerName};Database={Database};"
                + $"User Id={Username};Password={Password}; Trusted_Connection ={TrustedConnection}; Encrypt=False";
        }
    }
}
