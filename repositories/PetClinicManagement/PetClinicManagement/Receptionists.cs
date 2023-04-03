using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PetClinicManagement
{
    public partial class Receptionists : Form
    {
        public Receptionists()
        {
            InitializeComponent();
            populate();
            pictureBoxReceptionists.BorderStyle= BorderStyle.FixedSingle;   
        }
        SqlConnection connection = new SqlConnection(@"Data Source=MSI\TWINKLE;Initial Catalog=PetDB;User ID =sa;Password=twinkle");
        public void MeExit()
        {
            DialogResult iExit;
            iExit = MessageBox.Show("Confirm if you want to exit", "Save Data", MessageBoxButtons.YesNo);
            if (iExit == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        public void logout()
        {
            this.Close();
            LoginPage login = new LoginPage();
            login.Show();
        }

        private void populate()
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM RECEPTIONISTS";
                SqlDataAdapter sda = new SqlDataAdapter(query, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                receptionistsDGV.DataSource = ds.Tables[0];
                connection.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void clear()
        {
            textBoxRname.Text = "";
            textBoxRpass.Text = "";
            textBoxRPhone.Text = "";
            textBoxRusername.Text = "";
            richTextBoxRAddress.Text = "";
            comboBoxRSex.SelectedIndex= -1;
            dateTimePickerRec.Text = DateTime.Now.ToString();  
        }
        //private int _currentId = 0;
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                if (textBoxRname.Text == "" || textBoxRpass.Text == "" || textBoxRPhone.Text == "" || textBoxRusername.Text == "" || richTextBoxRAddress.Text == "" || comboBoxRSex.SelectedIndex == -1)
                {
                    MessageBox.Show("Missing details. All fields are mandatory");
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
                                cmd.Parameters.AddWithValue("@rname", textBoxRname.Text);
                                cmd.Parameters.AddWithValue("@rphone", textBoxRPhone.Text);
                                cmd.Parameters.AddWithValue("@raddress", richTextBoxRAddress.Text);
                                cmd.Parameters.AddWithValue("@rsex", comboBoxRSex.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@dob", dateTimePickerRec.Value);
                                cmd.Parameters.AddWithValue("@username", textBoxRusername.Text);
                                cmd.Parameters.AddWithValue("@password", textBoxRpass.Text);

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
                        connection.Close();

                        // Refresh the data grid view
                        populate();
                        clear();
                    }
                    catch (Exception en)
                    {
                        MessageBox.Show(en.Message);
                    }
                }
            }

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clear();
        }
        

        

        private void Receptionists_Load(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
              buttonAdd.Enabled = true;
                buttonClear.Enabled = true;
                buttonDelete.Enabled = true;
                buttonEdit.Enabled = true;
                buttonFetch.Enabled = true;

                populate();
              
            }
        
            else if(MyConnection.type == "D") {
                buttonAdd.Enabled = false;
                buttonClear.Enabled = false;
                buttonDelete.Enabled = false;
                buttonEdit.Enabled = false;
                buttonFetch.Enabled = false;
            }
            else if (MyConnection.type == "R")
            {
                buttonAdd.Enabled = false;
                buttonClear.Enabled = false;
                buttonDelete.Enabled = false;
                buttonEdit.Enabled = false;
                buttonFetch.Enabled = false;
            }
        }

        private void pictureBoxPets_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                Pets pets = new Pets();
                pets.Show();
                this.Hide();
            }



        }

       

        private void pictureBoxDoctors_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                Doctors doctors = new Doctors();
                doctors.Show();
                this.Hide();
            }
        }

        private void pictureBoxLogout_Click(object sender, EventArgs e)
        {
            logout();
        }

        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            MeExit();
        }
       
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A") {
                // Check if a row is selected
                if (receptionistsDGV.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row to delete.");
                    return;
                }
                else
                {
                    // Get the ID of the selected row
                    int rid = Convert.ToInt32(receptionistsDGV.SelectedRows[0].Cells["rid"].Value);
                    // Confirm deletion
                    DialogResult result = MessageBox.Show("Are you sure you want to delete the selected row?", "Confirmation", MessageBoxButtons.YesNo);

                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        try
                        {
                            // Delete the row from the database
                            connection.Open();
                            SqlCommand cmd = new SqlCommand("DELETE FROM RECEPTIONISTS WHERE rid = @rid", connection);
                            cmd.Parameters.AddWithValue("@rid", rid);
                            cmd.ExecuteNonQuery();

                            // Remove the row from the DataGridView
                            receptionistsDGV.Rows.RemoveAt(receptionistsDGV.SelectedRows[0].Index);

                            MessageBox.Show("Row deleted successfully.");
                            connection.Close();
                        }catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        
                    }
                }
            }
        }
        SqlDataReader dr;
        SqlCommand cmd;
        private void buttonEdit_Click(object sender, EventArgs e)
        {
           string rid = textBoxSearchId.Text.Trim();
            string name = textBoxRname.Text.Trim();
            string address = richTextBoxRAddress.Text.Trim();
            string phone = textBoxRPhone.Text.Trim();
            string username = textBoxRusername.Text.Trim();
            string password = textBoxRpass.Text.Trim();
            //string dob = dateTimePickerRec.Value;  
            //string sex= comboBoxRSex.Text.Trim();
            if (MyConnection.type == "A") 
            {
                if (textBoxRname.Text == "" || textBoxRpass.Text == "" || textBoxRPhone.Text == "" || textBoxRusername.Text == "" || richTextBoxRAddress.Text == "" || comboBoxRSex.SelectedIndex == -1)
                {
                    MessageBox.Show("Missing details. All fields are mandatory");
                }
                else
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("UPDATE RECEPTIONISTS SET rname=@rname,rsex=@rsex,dob=@dob,rphone=@rphone, raddress=@raddress, username=@username,password=@password  WHERE rid=@rid", connection);
                       cmd.Parameters.AddWithValue("@rid", rid); //rid,rname, rsex, dob,rphone,raddress,username,password 
                        cmd.Parameters.AddWithValue("@rname", name);
                        cmd.Parameters.AddWithValue("@raddress", address);
                        cmd.Parameters.AddWithValue("@rphone", phone);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@dob", dateTimePickerRec.Value);
                        cmd.Parameters.AddWithValue("@rsex", comboBoxRSex.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record updated successfully!");
                        // Close the database connection
                        connection.Close();

                        // Refresh the data grid view
                        populate();
                        clear();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                   
                }
            }
            }
            
            
   
        

        private void buttonFetch_Click(object sender, EventArgs e)
        {
            string id = textBoxSearchId.Text.Trim();
            if (id != "")
            {
                try
                {
                    connection.Open();
                    cmd = new SqlCommand("SELECT * FROM RECEPTIONISTS WHERE rid=@rid", connection);
                    cmd.Parameters.AddWithValue("@rid", id);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBoxRname.Text = dr["rname"].ToString();
                        textBoxRpass.Text = dr["password"].ToString();
                        textBoxRPhone.Text = dr["rphone"].ToString();
                        richTextBoxRAddress.Text = dr["raddress"].ToString();
                        textBoxRusername.Text = dr["username"].ToString();
                        comboBoxRSex.Text = dr["rsex"].ToString();
                        dateTimePickerRec.Text = dr["dob"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Record not found!");
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
               
            }
            else
            {
                MessageBox.Show("Please enter an ID!");
            }
        }

        private void pictureBoxDashboard_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A"||MyConnection.type == "D" || MyConnection.type == "R")
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Hide();

            }
        }

        private void pictureBoxTreatments_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                Treatments prescriptions = new Treatments();
                prescriptions.Show();
                this.Hide();

            }
        }
    }
}
