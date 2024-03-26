using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static WinFormsApp1.authenticate;

namespace MiniProj
{
    public partial class homePage : Form
    {

        private authenticate.User AuthenticateUser;
        private string connectionString = @"Data Source=LAB109PC07\SQLEXPRESS; Initial Catalog=Cafe; Integrated Security=True;";

        public homePage(authenticate.User authenticateUser)
        {
            InitializeComponent();
            AuthenticateUser = authenticateUser;
            Hello.Text = $"Welcome, {AuthenticateUser.Username}";
        }

        private void homePage_Load(object sender, EventArgs e)
        {
            Number.SelectedIndex = 0;
            Type.SelectedIndex = 0;
            Location.SelectedIndex = 0;
            Server.SelectedIndex = 0;
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
        {
          
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void All_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM Tables ORDER BY Number ASC";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Clear previous data
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    // Bind data to DataGridView
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=LAB109PC07\SQLEXPRESS; Initial Catalog=Cafe; Integrated Security=True;"))
            {
                sqlCon.Open(); // Open SQL connection

                // Start with a base SQL query
                string query = "SELECT * FROM Tables WHERE 1=1";

                // Create a list to store the conditions for filtering
                List<string> conditions = new List<string>();

                // Check each ComboBox and add a condition if an item is selected
                if (Number.SelectedItem != null)
                    conditions.Add("Number = @Number");

                if (Type.SelectedItem != null)
                    conditions.Add("Type = @Type");

                if (Location.SelectedItem != null)
                    conditions.Add("Location = @Location");

                if (Server.SelectedItem != null)
                    conditions.Add("Server = @Server");

                // combine the conditions into the SQL query
                if (conditions.Count > 0)
                {
                    query += " AND " + string.Join(" AND ", conditions);
                }

                try
                {
                    SqlCommand cmd = new SqlCommand(query, sqlCon);

                    // Set parameters based on selected values
                    if (Number.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@Number", Number.SelectedItem.ToString());
                    if (Type.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@Type", Type.SelectedItem.ToString());
                    if (Location.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@Location", Location.SelectedItem.ToString());
                    if (Server.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@Server", Server.SelectedItem.ToString());

                    // Use SqlDataAdapter to fetch data and populate DataGridView
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string updateQuery = "UPDATE Tables SET Availability = 'Unavailable' WHERE Number = @Number AND Type = @Type AND Location = @Location AND Server = @Server AND Availability = 'Available';";

            try
            {
                // Create a new SQL connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a SQL command with parameters
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        // Add parameters to the SQL command
                        command.Parameters.AddWithValue("@Number", Number.Text);
                        command.Parameters.AddWithValue("@Type", Type.Text);
                        command.Parameters.AddWithValue("@Location", Location.Text);
                        command.Parameters.AddWithValue("@Server", Server.Text);

                        // Execute the SQL command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if any rows were affected (i.e., if the update was successful)
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Table reserved successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Table is already reserved or does not exist.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

