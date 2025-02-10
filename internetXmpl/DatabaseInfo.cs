namespace Login_WPF
{
    class DatabaseInfo
    {
        private static string dbFilePath = "default_path.mdb";

        public static string DbFilePath
        {
            get { return dbFilePath; }
            set { dbFilePath = value; }
        }

        public static string ConnectionString
        {
            get { return $"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {dbFilePath}"; }
        }

    }
}
