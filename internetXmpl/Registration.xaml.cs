using Microsoft.Win32;
using System.Data.OleDb;
using System.IO;
using System.Windows;

namespace Login_WPF
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            textBoxId.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDbFilePath.Text))
            {
                errormessage.Text = "Please enter a directory path";
                textBoxDbFilePath.Focus();
            }
            else if (textBoxId.Text.Length == 0)
            {
                errormessage.Text = "Enter a username.";
                textBoxId.Focus();
            }
            else
            {
                string idNum = textBoxId.Text;
                string password = passwordBox1.Password;
                string name = textBoxUser.Text;

                if (passwordBox1.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    passwordBox1.Focus();
                }
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                    passwordBoxConfirm.Focus();
                }
                else if (passwordBox1.Password != passwordBoxConfirm.Password)
                {
                    errormessage.Text = "Confirm password must be same as password.";
                    passwordBoxConfirm.Focus();
                }
                else
                {
                    errormessage.Text = "";

                    using (OleDbConnection connectionSelectValidation = new OleDbConnection(DatabaseInfo.ConnectionString))
                    {
                        connectionSelectValidation.Open();
                        string sqlQuery = "SELECT COUNT(*) FROM users WHERE idNum = @LoginId";

                        using (OleDbCommand commandSelectValidation = new OleDbCommand(sqlQuery, connectionSelectValidation))
                        {
                            commandSelectValidation.Parameters.AddWithValue("@LoginId", idNum);

                            object result = commandSelectValidation.ExecuteScalar();

                            if (result != null)
                            {
                                if (result.ToString() == "1")
                                {
                                    errormessage.Text = "This user already exists in the database.";
                                    textBoxId.Focus();
                                }
                                else
                                {
                                    connectionSelectValidation.Close();
                                    using (OleDbConnection connection = new OleDbConnection(DatabaseInfo.ConnectionString))
                                    {
                                        connection.Open();
                                        string insertQuery = "INSERT INTO Users ([idNum], [user], [password], [type]) values (?, ?, ?, \"user\")";

                                        using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection))
                                        {
                                            insertCommand.Parameters.AddWithValue("@IdNum", idNum);
                                            insertCommand.Parameters.AddWithValue("@User", name);
                                            insertCommand.Parameters.AddWithValue("@Password", password);
                                            //insertCommand.Parameters.AddWithValue("@Type", );

                                            int rowsAffected = insertCommand.ExecuteNonQuery();

                                            if (rowsAffected == 0)
                                            {
                                                throw new Exception("Failed to insert the user");
                                            }
                                            else
                                            {
                                                errormessage.Text = "You have Registered successfully.";
                                                Reset();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                errormessage.Text = "Sorry! Result set is empty!";
                            }
                        }
                    }
                }
            }
        }
    }
}