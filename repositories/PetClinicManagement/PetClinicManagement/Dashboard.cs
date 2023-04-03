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
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media.Media3D;

namespace PetClinicManagement
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();
            // Highlight the picture box
            pictureBoxDashboard.BorderStyle = BorderStyle.FixedSingle;
        }
        SqlConnection connection = new SqlConnection(@"Data Source=MSI\TWINKLE;Initial Catalog=PetDB;User ID=sa;Password=twinkle");
        private void Dashboard_Load(object sender, EventArgs e)
        {
            if(MyConnection.type == "D" || MyConnection.type == "R")
            {
                pictureBoxDoc.Hide();
                pictureBoxRec.Hide();
                TotalDoctorsLabel.Hide();
                TotalReceptionistsLabel.Hide();
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
        public void logout()
        {
            this.Close();
            LoginPage login = new LoginPage();
            login.Show();
        }

        
       
        private void LoadDashboardData()
        {
            // Load total number of patients, doctors, and receptionists from the database
            int totalPatients = GetTotalPatients();
            int totalDoctors = GetTotalDoctors();
            int totalReceptionists = GetTotalReceptionists();

            // Display the total number of patients, doctors, and receptionists in labels
            TotalPatientsLabel.Text += totalPatients;
            if (MyConnection.type == "A") {
                TotalDoctorsLabel.Text = "Total Doctors: " + totalDoctors;
                TotalReceptionistsLabel.Text = "Total Receptionists: " + totalReceptionists;
            }
            

            // Load total number of cats, dogs, and birds from the database
            int totalCats = GetTotalCats();
            int totalDogs = GetTotalDogs();
            int totalBirds = GetTotalBirds();

            // Display the total number of cats, dogs, and birds in a pie chart
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "PieChartArea";
            DashboardPieChart.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            series.ChartArea = "PieChartArea";
            series.Points.AddXY("Cats", totalCats);
            series.Points.AddXY("Dogs", totalDogs);
            series.Points.AddXY("Birds", totalBirds);
            // Apply a 3D effect to the chart
            DashboardPieChart.ChartAreas["PieChartArea"].Area3DStyle.Enable3D = true;
            DashboardPieChart.ChartAreas["PieChartArea"].Area3DStyle.Inclination = 40;
            DashboardPieChart.ChartAreas["PieChartArea"].Area3DStyle.Rotation = 30;
            DashboardPieChart.BackColor = Color.Transparent;
            DashboardPieChart.ChartAreas["PieChartArea"].BackColor=Color.Transparent;

            // Set the size of the chart area
            DashboardPieChart.ChartAreas["PieChartArea"].InnerPlotPosition.Width =100 ;
            DashboardPieChart.ChartAreas["PieChartArea"].InnerPlotPosition.Height = 100;

            // Set the size of the legend area
            DashboardPieChart.Legends[0].Position.Width = 30;
            DashboardPieChart.Legends[0].Position.Height = 20;

            //  DashboardPieChart.Legends[0].BackColor=Color.Transparent;
            // Show the percentage occupied by each type of animal
            DashboardPieChart.Series["Dashboard"].Label = "#VALX\n#PERCENT{P0}";

            DashboardPieChart.Series.Add(series);
          
        }

        private int GetTotalPatients()
        {
            int total = 0;
            try
            {
                string query = "SELECT COUNT(*) FROM PETS";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                total = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return total;

        }

        private int GetTotalDoctors()
        {
            int total = 0;
            try {
                string query = "SELECT COUNT(*) FROM DOCTORS";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                total = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return total;
        }

        private int GetTotalReceptionists()
        {
            int total = 0;

            try
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM RECEPTIONISTS";
                SqlCommand command = new SqlCommand(query, connection);
               
                total = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return total;
        }

        private int GetTotalCats()
        {
            int total = 0;

            try
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM PETS WHERE category = 'Cats'";
                SqlCommand command = new SqlCommand(query, connection);
                
                total = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



            return total;
        }
        private int GetTotalDogs()
        {
            int total = 0;

            try
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM PETS WHERE category = 'Dogs'";
                SqlCommand command = new SqlCommand(query, connection);
              
                total = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return total;
        }

        private int GetTotalBirds()
        {
            int total = 0;
            try
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM PETS WHERE category = 'Birds'";
                SqlCommand command = new SqlCommand(query, connection);

                total = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }




            return total;
        }

        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            MeExit();
        }

        private void pictureBoxLogout_Click(object sender, EventArgs e)
        {
            logout();
        }

        private void pictureBoxPets_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A"||MyConnection.type=="R"||MyConnection.type=="D")
            {

                Pets pets = new Pets();
                pets.Show();
                this.Hide();

            }
        }

        private void pictureBoxTreatments_Click(object sender, EventArgs e)
        {
            if (MyConnection.type == "A" || MyConnection.type == "R" || MyConnection.type == "D")
            {
                Treatments treatments = new Treatments();
                treatments.Show();
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

        private void pictureBoxDashboard_Click(object sender, EventArgs e)
        {

        }
    }
}
