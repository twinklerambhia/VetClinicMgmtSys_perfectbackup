using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace PetClinicManagement
{
    public partial class Treatments : Form
    {
        public Treatments()
        {
            InitializeComponent();
            pictureBoxTreatments.BorderStyle = BorderStyle.FixedSingle;
        }

        

        private void Prescriptions_Load(object sender, EventArgs e)
        {
            if (MyConnection.type == "A")
            {

                buttonAdd.Enabled = true;
                buttonClear.Enabled = true;
                buttonDelete.Enabled = true;
                buttonEdit.Enabled = true;
                
                pictureBoxPets.Enabled = true;
                
                pictureBoxDoctors.Enabled = true;
                //pictureBoxBills.Enabled = true;
                pictureBoxReceptionists.Enabled = true;
                pictureBoxLogout.Enabled = true;
                populate();
                
            }
            else if (MyConnection.type == "D")
            {
                buttonAdd.Enabled = false;
                buttonClear.Enabled = false;
                buttonDelete.Enabled = false;
                buttonEdit.Enabled = false;
                buttonFetch.Enabled = true;
                pictureBoxPets.Enabled = true;
                pictureBoxDoctors.Enabled = false;
                //pictureBoxBills.Enabled = false;
                pictureBoxReceptionists.Enabled = false;
                pictureBoxLogout.Enabled = true;
                populate();
            }
            else if (MyConnection.type == "R")
            {
                buttonAdd.Enabled = true;
                buttonClear.Enabled = true;
                buttonDelete.Enabled = true;
                buttonEdit.Enabled = true;
                pictureBoxPets.Enabled = true;
                 

                pictureBoxDoctors.Enabled = true;
                //pictureBoxBills.Enabled = true;
                pictureBoxReceptionists.Enabled = true;
                pictureBoxLogout.Enabled = true;
                populate();
            }

        }

        public void MeExit()
        {
            DialogResult iExit;
            iExit = MessageBox.Show("Confirm if you want to exit", "Save Data", MessageBoxButtons.YesNo);
            if (iExit == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void clear()
        {
            textBoxPid.Text = "";
            textBoxDid.Text = "";
            richTextBoxDiet.Text = "";
            richTextBoxDiagnosis.Text = "";
            comboBoxTests.SelectedIndex = -1;
            richTextBoxMedicines.Text = "";
            richTextBoxRemarks.Text = "";
            textBoxFees.Text = "";
        }
        private void populate()
        {
            try {
                connection.Open();
                string query = "SELECT * FROM TREATMENTS";
                SqlDataAdapter sda = new SqlDataAdapter(query, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                treatmentsDGV.DataSource = ds.Tables[0];
                connection.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void pictureBoxPets_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A"|| MyConnection.type=="R"||MyConnection.type=="D")
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

        private void pictureBoxReceptionists_Click(object sender, EventArgs e)
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
            this.Close();
            LoginPage login = new LoginPage();
            login.Show();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=MSI\TWINKLE;Initial Catalog=PetDB;User ID =sa;Password=twinkle");
       
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A" || MyConnection.type == "R")
            {
                if (textBoxPid.Text == "" || textBoxDid.Text == "" || richTextBoxDiet.Text == "" || richTextBoxDiagnosis.Text == "" || comboBoxTests.SelectedIndex == -1 || richTextBoxMedicines.Text == "" || richTextBoxRemarks.Text== ""|| textBoxFees.Text == "")
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
                            string query = "INSERT INTO TREATMENTS (pid,did, diagnosis,tests, medicines,diet, remarks,fees) values(@pid,@did,@diagnosis,@tests,@medicines,@diet,@remarks,@fees)";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            //_currentId++;
                            // Set the command parameters
                            cmd.Parameters.AddWithValue("@pid", textBoxPid.Text);
                            cmd.Parameters.AddWithValue("@did", textBoxDid.Text);
                            cmd.Parameters.AddWithValue("@diagnosis", richTextBoxDiagnosis.Text);
                            cmd.Parameters.AddWithValue("@tests", comboBoxTests.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@medicines", richTextBoxMedicines.Text);
                            cmd.Parameters.AddWithValue("@diet", richTextBoxDiet.Text);
                            cmd.Parameters.AddWithValue("@remarks", richTextBoxRemarks.Text);
                            cmd.Parameters.AddWithValue("@fees", textBoxFees.Text);
                           

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

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A" || MyConnection.type == "R")
            {
                // Check if a row is selected
                if (treatmentsDGV.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row to delete.");
                    return;
                }
                else
                {
                    // Get the ID of the selected row
                    int pid = Convert.ToInt32(treatmentsDGV.SelectedRows[0].Cells["pid"].Value);
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
                            SqlCommand cmd = new SqlCommand("DELETE FROM TREATMENTS WHERE pid = @pid", connection);
                            cmd.Parameters.AddWithValue("@pid", pid);
                            cmd.ExecuteNonQuery();

                            // Remove the row from the DataGridView
                            treatmentsDGV.Rows.RemoveAt(treatmentsDGV.SelectedRows[0].Index);

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

        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            MeExit();
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
                        cmd = new SqlCommand("SELECT * FROM TREATMENTS WHERE pid=@pid", connection);
                        cmd.Parameters.AddWithValue("@pid", id);
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                        textBoxPid.Text = dr["pid"].ToString();
                        textBoxDid.Text = dr["did"].ToString();
                            richTextBoxDiagnosis.Text = dr["diagnosis"].ToString();
                            comboBoxTests.Text = dr["tests"].ToString();
                            richTextBoxDiet.Text = dr["diet"].ToString();
                            richTextBoxMedicines.Text = dr["medicines"].ToString();
                            richTextBoxRemarks.Text = dr["remarks"].ToString();
                            textBoxFees.Text = dr["fees"].ToString();
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

            if (MyConnection.type == "A" || MyConnection.type == "R")
            {
                if (textBoxPid.Text == "" || textBoxDid.Text == "" || richTextBoxDiet.Text == "" || richTextBoxDiagnosis.Text == "" || comboBoxTests.SelectedIndex == -1 || richTextBoxMedicines.Text == "" || richTextBoxRemarks.Text == "" || textBoxFees.Text == "")
                {
                    MessageBox.Show("Missing details. All fields are mandatory");
                }
                else
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("UPDATE TREATMENTS SET pid=@pid,did=@did,diagnosis=@diagnosis,tests=@tests, medicines=@medicines, diet=@diet,remarks=@remarks, fees=@fees  WHERE pid=@pid", connection);
                        cmd.Parameters.AddWithValue("@pid", id);
                        cmd.Parameters.AddWithValue("@did", textBoxDid.Text);
                        cmd.Parameters.AddWithValue("@diagnosis", richTextBoxDiagnosis.Text);
                        cmd.Parameters.AddWithValue("@tests", comboBoxTests.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@medicines", richTextBoxMedicines.Text);
                        cmd.Parameters.AddWithValue("@diet", richTextBoxDiet.Text);
                        cmd.Parameters.AddWithValue("@remarks", richTextBoxRemarks.Text);
                        cmd.Parameters.AddWithValue("@fees", textBoxFees.Text);
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

        private void pictureBoxPrescriptions_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBoxDashboard_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A" || MyConnection.type == "D" || MyConnection.type == "R")
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Hide();

            }
        }
    }
}
