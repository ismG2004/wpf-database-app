using Microsoft.Win32;
using System.Data.OleDb;
using System.IO;
using System.Windows;

namespace Login_WPF
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            textBoxId.Focus();
        }

        Registration registration = new Registration();

        private void SelectFileLogic()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Select a File";
            openFileDialog.Filter = "All files (*.accdb;*.mdb)|*.accdb;*.mdb";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                if (!string.IsNullOrWhiteSpace(selectedFilePath))
                {
                    if (!File.Exists(selectedFilePath))
                    {
                        MessageBox.Show($"File '{selectedFilePath}' does not exist.");
                        openFileDialog.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a directory path.");
                }

                textBoxDbFilePath.Text = selectedFilePath;

                DatabaseInfo.DbFilePath = selectedFilePath;
                //string connectionString = DatabaseInfo.ConnectionString;
            }
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            SelectFileLogic();
        }

        public string idNum = "";
        public string name = "";
        public string type = "";

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDbFilePath.Text))
            {
                loginErrorMessage.Text = "Please enter a directory path";
                textBoxDbFilePath.Focus();
            }
            else if (textBoxId.Text.Length == 0)
            {
                loginErrorMessage.Text = "Enter an ID.";
                textBoxId.Focus();
            }
            else
            {
                string loginId = textBoxId.Text;
                string password = passwordBox1.Password;                

                using (OleDbConnection connection = new OleDbConnection(DatabaseInfo.ConnectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT COUNT(*) FROM users WHERE idNum = @LoginId AND password = @Password";

                    using (OleDbCommand command = new OleDbCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LoginId", loginId);
                        command.Parameters.AddWithValue("@Password", password);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            if (result.ToString() == "0")
                            {
                                loginErrorMessage.Text = "Sorry! Please enter existing username and password.";
                            }
                            else
                            {
                                string fetchDataSqlQuery = "SELECT idNum, user, type FROM users WHERE idNum = @LoginId";
                                using (OleDbCommand fetchDataSqlCommand = new OleDbCommand(fetchDataSqlQuery, connection))
                                {
                                    fetchDataSqlCommand.Parameters.AddWithValue("@LoginId", loginId);
                                    using (OleDbDataReader reader = fetchDataSqlCommand.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            // accessing columns using reader methods -- reader["ColumnName"]
                                            // idNum = Convert.ToInt32(reader["idNum"]);
                                            // idNum = reader["idNum"].ToString();

                                            if (!reader.IsDBNull(0)) 
                                            {
                                                idNum = reader[0].ToString();
                                            }

                                            if (!reader.IsDBNull(1))
                                            {
                                                name = reader[1].ToString();
                                            }

                                            if (!reader.IsDBNull(2))
                                            {
                                                type = reader[2].ToString();
                                            }

                                            Console.WriteLine($"ID: {idNum}, Name: {name}, Type: {type}");
                                        }
                                    }
                                }
                                Form2 form2 = new Form2();
                                form2.Show();
                                form2.textBoxId.Text = idNum;
                                form2.textBoxName.Text = name;
                                Close();
                            }
                        }
                        else
                        {
                            loginErrorMessage.Text = "Sorry! Please enter existing username and password.";
                        }
                    }
                }
            }
        }
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            registration.Show();
            Close();
        }

       
    }
}