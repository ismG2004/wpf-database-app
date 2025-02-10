using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace Login_WPF
{
    public partial class Form2 :  Window
    {
        public Dictionary<string, string> MyDictionary { get; set; }
        public List<string> MyDictionaryKeys { get; set; }

        public string bukva = "";
        public string nomerPrikaza = "";
        public int nomer = 1;

        CustomDict customDict = new CustomDict();

        public DateTime selectedDate;

        public Form2()
        {
            InitializeComponent();
            PopulateDataGrid();
            SetDatePickerDateRange();
            ComboBoxBinding();
            
        }

        private void ComboBoxBinding()
        {
            // Initialize the dictionary with some sample data
            MyDictionary = customDict.dictionary;

            // Extract keys from the dictionary
            MyDictionaryKeys = new List<string>(MyDictionary.Keys);

            // Set the DataContext to the current instance (this), so the ComboBox can bind to the collection
            DataContext = this;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            insertMethod1();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            PopulateDataGrid();
        }

        public void PopulateDataGrid()
        {
            string sqlQueryForTableInForm2 = "SELECT [Тип приказа], [Номер приказа], [Дата приказа], [Исполнитель] FROM Table1 ORDER BY [Номер] DESC";

            OleDbConnection connectionForm2 = new OleDbConnection(DatabaseInfo.ConnectionString);
            connectionForm2.Open();

            ShowTable(sqlQueryForTableInForm2, connectionForm2);
        }

        public void ShowTable(string sqlQueryForTableInForm2, OleDbConnection connectionForm2)
        {
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQueryForTableInForm2, connectionForm2))
            {
                // Create a DataSet to hold the data
                DataSet dataSet = new DataSet();

                // Fill the DataSet with data from the Access table
                adapter.Fill(dataSet, "Table1");

                // Set the DataGrid's ItemsSource to the DataTable
                dataGrid.ItemsSource = dataSet.Tables["Table1"].DefaultView;

                // ?
                connectionForm2.Close();
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Retrieve the selected date from the DatePicker
            selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;

            // Now you can use the selectedDate variable for further processing
            // For example, you can use selectedDate in your INSERT query
            // string insertQuery = $"INSERT INTO YourTable (DateColumn) VALUES ('{selectedDate.ToString("yyyy-MM-dd")}')";
        }

        private void SetDatePickerDateRange()
        {
            // Get the current date and time
            DateTime currentDate = DateTime.Now;

            // Set the date range for 7 days before and 7 days after the current date
            DateTime startDate = currentDate.AddDays(-7);
            DateTime endDate = currentDate.AddDays(7);

            // Set the properties of the DatePicker to limit the selectable date range
            datePicker.DisplayDateStart = startDate;
            datePicker.DisplayDateEnd = endDate;

            // Set the selected date to the current date as a default
            datePicker.SelectedDate = currentDate;
        }

        public void insertMethod1()
        {
            using (OleDbConnection connection = new OleDbConnection(DatabaseInfo.ConnectionString))
            {
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction transaction = null;

                // Set the Connection to the new OleDbConnection.
                command.Connection = connection;

                // Open the connection and execute the transaction.
                try
                {
                    connection.Open();

                    // Start a local transaction
                    transaction = connection.BeginTransaction();

                    // Assign transaction object for a pending local transaction.
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Execute the commands.
                    command.CommandText =
                        "SELECT * FROM Table1 WHERE [Дата приказа] >= DateSerial(Year(Date()), 1, 1) AND [Номер] = (SELECT MAX([Номер]) AS MaxValue FROM Table1 WHERE [Дата приказа] >= DateSerial(Year(Date()), 1, 1););";
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        //reader.Read();
                        //if (reader.Read() == false)
                        //{
                        //    MyDictionary = customDict.dictionary;
                        //    foreach (var kvp in MyDictionary)
                        //    {
                        //        if (stringComboBox.Text == kvp.Key)
                        //        {
                        //            bukva = kvp.Value;
                        //        }
                        //    }
                        //    nomer = 1;
                        //    nomerPrikaza = nomer.ToString() + bukva;
                        //}
                        while (reader.Read())
                        {
                                MyDictionary = customDict.dictionary;
                                foreach (var kvp in MyDictionary)
                                {
                                    if (stringComboBox.Text == kvp.Key)
                                    {
                                        bukva = kvp.Value;
                                    }
                                }
                            
                            if (!reader.IsDBNull(2))
                            {
                                nomer = Convert.ToInt32(reader[2]) + 1;
                                nomerPrikaza = nomer.ToString() + bukva;
                            }

                            if (reader.IsDBNull(2))
                            {
                                nomer = 1;
                                nomerPrikaza = nomer.ToString() + bukva;
                            }
                        }
                    }

                    command.CommandText =
                        "INSERT INTO Table1 ([Тип приказа], [Номер], [Буква приказа], [Номер приказа], [Дата приказа], [Исполнитель]) VALUES (@type, @nomer, @bukva, @nomerPrikaza, @date, @executor)";
                    command.Parameters.AddWithValue("@type", stringComboBox.Text);
                    command.Parameters.AddWithValue("@nomer", nomer);
                    command.Parameters.AddWithValue("@bukva", bukva);
                    command.Parameters.AddWithValue("@nomerPrikaza", nomerPrikaza);
                    command.Parameters.AddWithValue("@date", selectedDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@executor", textBoxId.Text);
                    command.ExecuteNonQuery();

                    // Commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    try
                    {
                        // Attempt to roll back the transaction.
                        transaction.Rollback();
                    }
                    catch
                    {
                        // Do nothing here; transaction is not active.
                    }
                }
                // The connection is automatically closed when the
                // code exits the using block.
            }
            PopulateDataGrid();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }

        
    }
}
