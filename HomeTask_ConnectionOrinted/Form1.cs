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

namespace HomeTask_ConnectionOrinted
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = txtName.Text.Trim(); // use name for search

            if (string.IsNullOrEmpty(searchValue))
            {
                LoadCustomers();
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(CentralizedClass.ConnectionString))
                {
                    string query = "SELECT * FROM Customers WHERE Name LIKE @Name OR Email LIKE @Name OR Phone LIKE @Name";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@Name", "%" + searchValue + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CentralizedClass.ConnectionString))
                {
                    string query = "INSERT INTO Customers (Name, Email, Phone, Address) VALUES (@Name, @Email, @Phone, @Address)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAdress.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer added successfully!");
                    con.Close();
                }
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CentralizedClass.ConnectionString))
                {
                    string query = "SELECT * FROM Customers";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
        }


        private void btnDashboard_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(CentralizedClass.ConnectionString))
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Customers", con);
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Orders", con);

                int totalCustomers = (int)cmd1.ExecuteScalar();
                int totalOrders = (int)cmd2.ExecuteScalar();

                MessageBox.Show($"Total Customers: {totalCustomers}\nTotal Orders: {totalOrders}",
                    "Dashboard Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnCustomer_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CentralizedClass.ConnectionString))
                {
                    string query = "SELECT * FROM Orders";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }

        private void btnFAQs_Click(object sender, EventArgs e)
        {
            MessageBox.Show("FAQs:\n1. Add → Inserts a new record.\n2. Load → Shows all records.\n3. Search → Filters customers.\n4. Dashboard → Shows summary.",
                "FAQs", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
