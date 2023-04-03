using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PetClinicManagement
{
    public partial class Pets : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=MSI\TWINKLE;Initial Catalog=PetDB;User ID =sa;Password=twinkle");
        public Pets()
        {
            InitializeComponent();
           pictureBoxPets.BorderStyle= BorderStyle.FixedSingle;
           // populate();
        }
        private void populate()
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM PETS";
                SqlDataAdapter sda = new SqlDataAdapter(query, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                petsDGV.DataSource = ds.Tables[0];
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void MeExit()
        {
            DialogResult iExit;
            iExit = MessageBox.Show("Confirm if you want to exit", "Save Data", MessageBoxButtons.YesNo);
            if(iExit == DialogResult.Yes)
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

        private bool CheckIfTableHasRecords()
        {
            bool result = false;
            string query = "SELECT COUNT(*) FROM PETS" ;
           
                SqlCommand command = new SqlCommand(query, connection);

            try {
                connection.Open();
                try
                {
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        result = true;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }
    



        private int _currentId;

        private void setCurrentID()
        {
            try
            {
                connection.Open();
                try
                {
                   string query1 = "SELECT TOP 1 pid FROM PETS ORDER BY pid DESC";
                   // string query1 = "SELECT MAX(pid) FROM PETS";
                 //  string query1="SELECT MAX(CAST(pid AS INT)) FROM PETS";
                   // string query1= "SELECT TOP 1 CAST(pid AS INT) FROM PETS ORDER BY pid DESC";
                    SqlCommand command1 = new SqlCommand(query1, connection);
                    
                    SqlDataReader reader = command1.ExecuteReader();

                    if (reader.Read())
                    {
                        string current = reader["pid"].ToString();
                        _currentId = Convert.ToInt32(current);
                       

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Pets_Load(object sender, EventArgs e)
        {
            if (MyConnection.type == "A"||MyConnection.type=="R")
            {
                buttonAdd.Enabled = true;
                buttonClear.Enabled = true;
                buttonDelete.Enabled = true;
                buttonEdit.Enabled = true;
                buttonFetch.Enabled = true;
                populate();

                // Check if the DOCTORS table has any records
                bool hasRecords = CheckIfTableHasRecords();
                if (hasRecords)
                {
                    // Do something if the table has records
                    setCurrentID();
                }
                else
                {
                    // DBCC CHECKIDENT('DOCTORS', RESEED, 0);

                    // Do something if the table has no records
                    _currentId = 0;
                }




            }
            else if (MyConnection.type == "D")
            {
                buttonAdd.Enabled = false;
                buttonClear.Enabled = false;
                buttonDelete.Enabled = false;
                buttonEdit.Enabled = false;
                buttonFetch.Enabled = true;
                pictureBoxPrescriptions.Enabled = true;
                populate();
            }
           
        }
        private void clear()
        {
            textBoxName.Text = "";
            textBoxAge.Text = "";
            comboBoxCategory.SelectedIndex = -1;
            richTextBoxAddress.Text = "";
            textBoxPhone.Text = "";
            comboBoxSex.SelectedIndex = -1;
            richTextBoxAllergies.Text = "";
            textBoxOwnerName.Text = "";
        }
       

       // SqlConnection connection = new SqlConnection(@"Data Source=MSI\SQLEXPRESS;Initial Catalog=PetManagementSystemDB;Integrated Security=True");
      // private int _currentId = 0;


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A"||MyConnection.type=="R")
            {
                if (textBoxName.Text == "" || textBoxPhone.Text == "" || richTextBoxAddress.Text == "" || richTextBoxAllergies.Text == "" || comboBoxSex.SelectedIndex == -1 || textBoxAge.Text == "" || comboBoxCategory.SelectedIndex == -1|| textBoxOwnerName.Text=="")
                {
                    MessageBox.Show("Missing details. All fields are mandatory");
                }
                else
                {
                    try
                    {
                        // Open the database connection
                        connection.Open();

                        try
                        {
                           
                            // Create the insert command
                            string query = "INSERT INTO PETS (pid,pet_name, category, sex,age_months, allergies,owner_address,owner_phone, owner_name) values(@pid,@pet_name,@category,@sex,@age_months,@allergies,@owner_address,@owner_phone,@owner_name)";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            _currentId++;
                            // Set the command parameters
                            cmd.Parameters.AddWithValue("@pid", _currentId);
                            cmd.Parameters.AddWithValue("@pet_name", textBoxName.Text);
                            cmd.Parameters.AddWithValue("@category", comboBoxCategory.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@sex", comboBoxSex.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@age_months", textBoxAge.Text);
                            cmd.Parameters.AddWithValue("@allergies", richTextBoxAllergies.Text);
                            cmd.Parameters.AddWithValue("@owner_address", richTextBoxAddress.Text);
                            cmd.Parameters.AddWithValue("@owner_phone", textBoxPhone.Text);
                            cmd.Parameters.AddWithValue("@owner_name",textBoxOwnerName.Text);
                           
                            // Execute the insert command
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Inserted successfully!");
                        }
                        catch (Exception en)
                        {
                            MessageBox.Show(en.Message);


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

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            string id = textBoxSearchId.Text.Trim();
           
            if (MyConnection.type == "A"||MyConnection.type=="R")
            {
                if (textBoxName.Text == "" || textBoxPhone.Text == "" || richTextBoxAddress.Text == "" || richTextBoxAllergies.Text == "" || comboBoxSex.SelectedIndex == -1 || textBoxAge.Text == "" || comboBoxCategory.SelectedIndex == -1 || textBoxOwnerName.Text == "")
                {
                    MessageBox.Show("Missing details. All fields are mandatory");
                }
                else
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("UPDATE PETS SET pet_name=@pet_name,category=@category,sex=@sex,age_months=@age_months, allergies=@allergies, owner_address=@owner_address,owner_phone=@owner_phone, owner_name=@owner_name  WHERE pid=@pid", connection);
                        cmd.Parameters.AddWithValue("@pid", id);
                        cmd.Parameters.AddWithValue("@pet_name", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@category", comboBoxCategory.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@sex", comboBoxSex.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@age_months", textBoxAge.Text);
                        cmd.Parameters.AddWithValue("@allergies", richTextBoxAllergies.Text);
                        cmd.Parameters.AddWithValue("@owner_address", richTextBoxAddress.Text);
                        cmd.Parameters.AddWithValue("@owner_phone", textBoxPhone.Text);
                        cmd.Parameters.AddWithValue("@owner_name", textBoxOwnerName.Text);
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

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A"||MyConnection.type=="R")
            {
                // Check if a row is selected
                if (petsDGV.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row to delete.");
                    return;
                }
                else
                {
                    // Get the ID of the selected row
                    int pid = Convert.ToInt32(petsDGV.SelectedRows[0].Cells["pid"].Value);
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
                            SqlCommand cmd = new SqlCommand("DELETE FROM PETS WHERE pid = @pid", connection);
                            cmd.Parameters.AddWithValue("@pid", pid);
                            cmd.ExecuteNonQuery();

                            // Remove the row from the DataGridView
                            petsDGV.Rows.RemoveAt(petsDGV.SelectedRows[0].Index);

                            MessageBox.Show("Row deleted successfully.");
                            connection.Close();

                            // setCurrentID();
                            // Check if the DOCTORS table has any records
                            bool hasRecords = CheckIfTableHasRecords();
                            if (hasRecords)
                            {
                                // Do something if the table has records
                                setCurrentID();
                            }
                            else
                            {
                                // DBCC CHECKIDENT('DOCTORS', RESEED, 0);

                                // Do something if the table has no records
                                _currentId = 0;
                            }
                        }catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                       
                    }
                }

            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void pictureBoxLogout_Click(object sender, EventArgs e)
        {
            logout();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            MeExit();
        }

        private void pictureBoxReceptionists_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                Receptionists receptionists=new Receptionists();
                receptionists.Show();
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

        private void pictureBoxPrescriptions_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A"||MyConnection.type=="R"||MyConnection.type=="D")
            {
                Treatments prescriptions = new Treatments();
                prescriptions.Show();
                this.Hide();

            }
            
           
        }
        SqlDataReader dr;
        SqlCommand cmd;
        private void buttonFetch_Click(object sender, EventArgs e)
        {
            string id = textBoxSearchId.Text.Trim();
            if (id != "")
            {
                try
                {
                    connection.Open();
                    cmd = new SqlCommand("SELECT * FROM PETS WHERE pid=@pid", connection);
                    cmd.Parameters.AddWithValue("@pid", id);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBoxName.Text = dr["pet_name"].ToString();
                        textBoxAge.Text = dr["age_months"].ToString();
                        textBoxOwnerName.Text = dr["owner_name"].ToString();
                        textBoxPhone.Text = dr["owner_phone"].ToString();
                        richTextBoxAddress.Text = dr["owner_address"].ToString();
                        richTextBoxAllergies.Text = dr["allergies"].ToString();
                        comboBoxSex.Text = dr["sex"].ToString();
                        comboBoxCategory.Text = dr["category"].ToString();

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
            if (MyConnection.type == "A"|| MyConnection.type == "D" || MyConnection.type == "R" )
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Hide();

            }
        }
    }
}
