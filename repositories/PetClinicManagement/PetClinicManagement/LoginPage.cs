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
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
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

        private void label5_Click(object sender, EventArgs e)
        {
            RegistrationReceptionist registrationReceptionist = new RegistrationReceptionist();
            registrationReceptionist.Show();
            this.Hide();
        }

       

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=MSI\TWINKLE;Initial Catalog=PetDB;User ID =sa;Password=twinkle");
            if (comboBoxRole.SelectedIndex == -1 || textBoxUsername.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("All the fields are mandatory!");
            }
            if (comboBoxRole.SelectedIndex==0)
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM ADMINS WHERE username = @username AND password = @password", connection);
                    command.Parameters.AddWithValue("@username", textBoxUsername.Text);
                    command.Parameters.AddWithValue("@password", textBoxPassword.Text);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        MessageBox.Show("Login successful!");
                        MyConnection.type = "A";
                        //Pets pets = new Pets();
                        //pets.Show();
                        // this.Hide();
                        Treatments treatments = new Treatments();
                        treatments.Show();
                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password.");
                    }
                    reader.Close();
                    connection.Close();
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }
            else if(comboBoxRole.SelectedIndex==1)
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM DOCTORS WHERE username = @username AND password = @password", connection);
                command.Parameters.AddWithValue("@username", textBoxUsername.Text);
                command.Parameters.AddWithValue("@password", textBoxPassword.Text);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    MessageBox.Show("Login successful!");
                    MyConnection.type = "D";
                    Pets pets= new Pets();
                    pets.Show();
                    this.Hide();
                   
                }
                else
                {
                    MessageBox.Show("Incorrect username or password.");
                }
                reader.Close();
                connection.Close();
            }
            // login code for Receptionists role
            else if(comboBoxRole.SelectedIndex==2)
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM RECEPTIONISTS WHERE username = @username AND password = @password", connection);
                command.Parameters.AddWithValue("@username", textBoxUsername.Text);
                command.Parameters.AddWithValue("@password", textBoxPassword.Text);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    MessageBox.Show("Login successful!");
                    MyConnection.type = "R";
                    Pets pets=new Pets();
                    pets.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Incorrect username or password.");
                }
                reader.Close();
                connection.Close();
            }
               
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            MeExit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
