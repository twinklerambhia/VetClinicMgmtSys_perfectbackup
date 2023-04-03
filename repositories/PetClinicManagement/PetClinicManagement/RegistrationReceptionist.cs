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

namespace PetClinicManagement
{
    public partial class RegistrationReceptionist : Form
    {
        public RegistrationReceptionist()
        {
            InitializeComponent();
        }
        SqlConnection connection= new SqlConnection(@"Data Source=MSI\TWINKLE;Initial Catalog=PetDB;User ID =sa;Password=twinkle");
      

        
        private void RegistrationReceptionist_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            textBoxName.Text = "";
            textBoxUName.Text = "";
            textBoxPass.Text = "";
            textBoxPhone.Text = "";
            textBoxAddress.Text = "";
            comboBoxGen.Text = "";
        }



        public static int _currentId = 0;
        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (textBoxAddress.Text==""||textBoxName.Text==""||textBoxPass.Text==""||textBoxPhone.Text==""||textBoxUName.Text==""|| comboBoxGen.SelectedIndex == -1)
            {
                MessageBox.Show("All the fields are mandatory!");
            }
            else
            {
                try
                {
                    // Open the database connection
                    connection.Open();

                    // Generate a unique ID
                    int id = new Random().Next(10000, 99999);
                    bool inserted = false;

                    // Keep generating a new ID until a unique one is found
                    while (!inserted)
                    {
                        try
                        {
                            // Create the insert command
                            string query = "INSERT INTO RECEPTIONISTS (rid,rname, rsex, dob,rphone,raddress,username,password )values(@rid,@rname,@rsex,@dob,@rphone,@raddress,@username,@password)";
                            SqlCommand cmd = new SqlCommand(query, connection);

                            // Set the command parameters
                            cmd.Parameters.AddWithValue("@rid", id);
                            cmd.Parameters.AddWithValue("@rname", textBoxName.Text);
                            cmd.Parameters.AddWithValue("@rphone", textBoxPhone.Text);
                            cmd.Parameters.AddWithValue("@raddress", textBoxAddress.Text);
                            cmd.Parameters.AddWithValue("@rsex", comboBoxGen.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@username", textBoxUName.Text);
                            cmd.Parameters.AddWithValue("@password", textBoxPass.Text);

                            // Execute the insert command
                            cmd.ExecuteNonQuery();

                            // If no exception is thrown, the insert was successful
                            inserted = true;

                            // Display success message
                            MessageBox.Show("Inserted successfully!");
                        }
                        catch (SqlException ex)
                        {
                            // Check if the exception was due to a unique constraint violation
                            if (ex.Number == 2627 || ex.Number == 2601)
                            {
                                // Generate a new unique ID and try again
                                id = new Random().Next(10000, 99999);
                            }
                            else
                            {
                                // Display error message and exit the loop
                                MessageBox.Show("Error: " + ex.Message);
                                break;
                            }
                        }
                    }

                    // Close the database connection
                    //connection.Close();

                    // Refresh the data grid view
                   // populate();
                    //clear();
                }
                catch (Exception en)
                {
                    MessageBox.Show(en.Message);
                }
                finally
                {
                    // Close the database connection
                    connection.Close();
                }


            }



        }

        private void buttonGoToLogin_Click(object sender, EventArgs e)
        {
            LoginPage login = new LoginPage();
            login.Show();
            this.Close();
        }
    }
}
