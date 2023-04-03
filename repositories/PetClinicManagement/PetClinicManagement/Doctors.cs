using System;
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
using System.Windows.Media.TextFormatting;

namespace PetClinicManagement
{
    public partial class Doctors : Form
    {
        public Doctors()
        {
            InitializeComponent();
            pictureBoxDoctors.BorderStyle = BorderStyle.FixedSingle;
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
                string query = "SELECT * FROM DOCTORS";
                SqlDataAdapter sda = new SqlDataAdapter(query, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                doctorsDGV.DataSource = ds.Tables[0];
                connection.Close();

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void clear()
        {
            textBoxName.Text = "";
            textBoxPhone.Text = "";
            richTextBoxAddress.Text = "";
            textBoxExp.Text = "";
            comboBoxSex.SelectedIndex = -1;
            comboBoxSpecialisation.SelectedIndex = -1;
            textBoxUsername.Text = "";
            textBoxPassword.Text = "";
            dateTimePickerDob.Text = DateTime.Now.ToString();
        }

        private bool CheckIfTableHasRecords()
        {
            bool result = false;
            string query = "SELECT COUNT(*) FROM DOCTORS";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                try
                {
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        result = true;
                    }
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
                    //string query1 = "SELECT TOP 1 did FROM DOCTORS ORDER BY did DESC";
                    string query1 = "SELECT TOP 1 did FROM DOCTORS ORDER BY did DESC";
                    SqlCommand command1 = new SqlCommand(query1, connection);
                  
                    SqlDataReader reader = command1.ExecuteReader();

                    if (reader.Read())
                    {
                        string current = reader["did"].ToString();
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
        private void Doctors_Load(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                buttonAdd.Enabled = true;
                buttonClear.Enabled = true;
                buttonDelete.Enabled = true;
                buttonEdit.Enabled = true;
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
                doctorsDGV.Hide();
            }
            else if (MyConnection.type == "R")
            {
                buttonAdd.Enabled = false;
                buttonClear.Enabled = false;
                buttonDelete.Enabled = false;
                buttonEdit.Enabled = false;
                doctorsDGV.Hide();
            }
            
        }
        //private int _currentId=0;
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                if (textBoxName.Text == "" || textBoxPhone.Text == "" || textBoxUsername.Text == "" || richTextBoxAddress.Text == "" || comboBoxSex.SelectedIndex == -1 || comboBoxSpecialisation.SelectedIndex == -1 || textBoxPassword.Text == "" || textBoxExp.Text == "")
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
                                string query = "INSERT INTO DOCTORS (did,doc_name, username, password,dob,sex,specialisation,experience_years,doc_address,doc_phone )values(@did,@doc_name,@username,@password,@dob,@sex,@specialisation,@experience_years,@doc_address,@doc_phone)";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                _currentId++;
                            // Set the command parameters
                                cmd.Parameters.AddWithValue("@did", _currentId);
                                cmd.Parameters.AddWithValue("@doc_name", textBoxName.Text);
                                cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);
                                cmd.Parameters.AddWithValue("@password", textBoxPassword.Text);
                                cmd.Parameters.AddWithValue("@dob", dateTimePickerDob.Value);
                                cmd.Parameters.AddWithValue("@sex", comboBoxSex.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@specialisation", comboBoxSpecialisation.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@experience_years", textBoxExp.Text);
                                cmd.Parameters.AddWithValue("@doc_address", richTextBoxAddress.Text);
                                cmd.Parameters.AddWithValue("@doc_phone", textBoxPhone.Text);
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

       

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clear();
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

        private void pictureBoxPrescriptions_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                Treatments prescriptions = new Treatments();
                prescriptions.Show();
                this.Hide();
            }
        }

        private void pictureBoxreceptionists_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {
                Receptionists receptionists = new Receptionists();
                receptionists.Show();
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
                if (doctorsDGV.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row to delete.");
                    return;
                }
                else
                {
                    // Get the ID of the selected row
                    int did = Convert.ToInt32(doctorsDGV.SelectedRows[0].Cells["did"].Value);
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
                            SqlCommand cmd = new SqlCommand("DELETE FROM DOCTORS WHERE did = @did", connection);
                            cmd.Parameters.AddWithValue("@did", did);
                            cmd.ExecuteNonQuery();

                            // Remove the row from the DataGridView
                            doctorsDGV.Rows.RemoveAt(doctorsDGV.SelectedRows[0].Index);

                            MessageBox.Show("Row deleted successfully.");
                            connection.Close();
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
                    cmd = new SqlCommand("SELECT * FROM DOCTORS WHERE did=@did", connection);
                    cmd.Parameters.AddWithValue("@did", id);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBoxName.Text = dr["doc_name"].ToString();
                        textBoxUsername.Text = dr["username"].ToString();
                        textBoxPassword.Text = dr["password"].ToString();
                        dateTimePickerDob.Text = dr["dob"].ToString();
                        comboBoxSex.Text = dr["sex"].ToString();
                        comboBoxSpecialisation.Text = dr["specialisation"].ToString();
                        textBoxExp.Text = dr["experience_years"].ToString();
                        richTextBoxAddress.Text = dr["doc_address"].ToString();
                        textBoxPhone.Text = dr["doc_phone"].ToString();
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

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            string id = textBoxSearchId.Text.Trim();

            if (MyConnection.type == "A" )
            {
                if (textBoxName.Text == "" || textBoxPhone.Text == "" || textBoxUsername.Text == "" || richTextBoxAddress.Text == "" || comboBoxSex.SelectedIndex == -1 || comboBoxSpecialisation.SelectedIndex == -1 || textBoxPassword.Text == "" || textBoxExp.Text == "")
                {
                    MessageBox.Show("Missing details. All fields are mandatory");
                }
                else
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("UPDATE DOCTORS SET doc_name=@doc_name,username=@username,password=@password,dob=@dob, sex=@sex, specialisation=@specialisation,experience_years=@experience_years, doc_address=@doc_address, doc_phone=@doc_phone  WHERE did=@did", connection);
                        cmd.Parameters.AddWithValue("@did", id);
                        cmd.Parameters.AddWithValue("@doc_name", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);
                        cmd.Parameters.AddWithValue("@password", textBoxPassword.Text);
                        cmd.Parameters.AddWithValue("@dob", dateTimePickerDob.Value);
                        cmd.Parameters.AddWithValue("@sex", comboBoxSex.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@specialisation", comboBoxSpecialisation.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@experience_years", textBoxExp.Text);
                        cmd.Parameters.AddWithValue("@doc_address", richTextBoxAddress.Text);
                        cmd.Parameters.AddWithValue("@doc_phone", textBoxPhone.Text);
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

        private void pictureBoxDashboard_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A"|| MyConnection.type == "D" || MyConnection.type == "R")
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Hide();

            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
