using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using NBA_Sponsor;

namespace CSS475_Database_UI
{
    public partial class HomePage : Form
    {
        private SqlConnectionStringBuilder builder;
        private String table1 = "";
        private String atrb11 = "";
        private String atrb12 = "";
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private BindingSource bindingSource1 = new BindingSource();

        /// <summary>
        /// Initializes form and creates connection with database.
        /// </summary>
        public HomePage()
        {
            InitializeComponent();

            try
            {
                // Create new SqlConnectionStringBuilder using database login information.
                builder = new SqlConnectionStringBuilder();
                builder.DataSource = "css475-10.database.windows.net";
                builder.UserID = "css475";
                builder.Password = "475cssqaz#";
                builder.InitialCatalog = "CSS475";

                // Use SqlConnectionStringBuilder to create connection.
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    // Get tables from database.
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT TABLE_NAME ");
                    sb.Append("FROM INFORMATION_SCHEMA.TABLES ");
                    sb.Append("WHERE TABLE_TYPE = 'BASE TABLE' and TABLE_CATALOG = 'CSS475'");
                    String sql = sb.ToString();

                    // Read tables into ComboBox for tables.
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                    table1Combo.Items.Add(reader.GetValue(i));
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString());
            }

            // Connect DataGridView to the bindingSource.
            dataGrid.DataSource = bindingSource1;

            // Initilize ComboBox values.
            attribute1_1.Items.Add("None");
            attribute1_2.Items.Add("None");
            attribute1_1.SelectedItem = "None";
            attribute1_2.SelectedItem = "None";
            table1Combo.Items.Add("None");
            table1Combo.SelectedItem = "None";
        }

        /// <summary>
        /// Updates current table string and values contained in the attribute ComboBoxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Store currently selected table as string in table1.
            table1 = table1Combo.SelectedItem.ToString();

            // Clear and initilize attribute ComboBoxes.
            attribute1_2.Items.Clear();
            attribute1_1.Items.Clear();
            attribute1_1.Items.Add("None");
            attribute1_2.Items.Add("None");
            attribute1_1.SelectedItem = "None";
            attribute1_2.SelectedItem = "None";
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                // Get all attributes associated to currently selected table.
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COLUMN_NAME ");
                sb.Append("FROM INFORMATION_SCHEMA.COLUMNS ");
                sb.AppendFormat("WHERE TABLE_NAME = '{0}'", table1);
                String sql = sb.ToString();

                // Set attribute ComboBoxes to the attributes retrieved by the SQL statement stored in sql.
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                attribute1_2.Items.Add(reader.GetValue(i));
                                attribute1_1.Items.Add(reader.GetValue(i));
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

        /// <summary>
        /// Updates the first attribute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void attribue1_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update String atrb11 to the first currently selected attribute.
            atrb11 = attribute1_1.SelectedItem.ToString();
        }

        /// <summary>
        /// Updates the second attribute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void attribute1_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update String atrb12 to the second currently selected attribute.
            atrb12 = attribute1_2.SelectedItem.ToString();
        }

        /// <summary>
        /// Executes a query using the values currently contained in the table and attribute ComboBoxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void execute_Click(object sender, EventArgs e)
        {
            if (table1 == "None")
                return;

            // Build and execute query
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                // Combine attribute selections into SQL syntax.
                String attributes = "";
                if (atrb11 != "None" && atrb12 != "None")
                {
                    attributes = atrb11 + ", " + atrb12 + " ";
                }
                else if (atrb11 != "None")
                    attributes = atrb11;
                else if (atrb12 != "None")
                    attributes = atrb12;
                else
                    attributes = "*";

                // Build SQL statement from currently selected table and attributes.
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT {0}", attributes);
                sb.Append(" FROM " + table1);
                String sql = sb.ToString();

                // Call method to display query in the DataGridView.
                GetData(sql);

                connection.Close();
            }
        }

        // Used https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/how-to-bind-data-to-the-windows-forms-datagridview-control
        // to help create this method.
        /// <summary>
        /// Public interface method that displays inputed command to the DataGridView.
        /// </summary>
        /// <param name="selectCommand"></param>
        public void GetData(string selectCommand)
        {
            try
            {
                // Create a new data adapter based on the specified query.
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                dataAdapter = new SqlDataAdapter(selectCommand, connection);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Opens the complex, Sponsor related queries form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moreQueries_Click(object sender, EventArgs e)
        {
            Form sponsorQuery = new SponsorQuery();
            sponsorQuery.Show();
        }

        /// <summary>
        /// Opens an instance of the form containing miscellaneous complex queries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otherQueries_Click(object sender, EventArgs e)
        {
            MiscQueries miscQuery = new MiscQueries();
            miscQuery.SetActiveHome(this);
            miscQuery.Show();
        }
    }
}
