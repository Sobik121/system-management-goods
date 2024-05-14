using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SystemManagementGoods
{
    public partial class UserHome : Form
    {

        public UserHome()
        {
            InitializeComponent();
        }
        public string listItems = "";
        public float total = 0;
        public string updateConn = "";

        private void UserHome_Load(object sender, EventArgs e)
        {
            using (var connection = new MySqlConnection(@"Datasource=127.0.0.1;Port=3306;SslMode=none;Username=root;Password=root;Database=dbForCourseWork;"))
            {
                connection.Open();

                var typeDataAdapter = new MySqlDataAdapter("SELECT DISTINCT Тип FROM goods", connection);
                var detailDataAdapter = new MySqlDataAdapter("SELECT DISTINCT Деталі FROM goods", connection);

                var typeDataSet = new DataSet();
                var detailDataSet = new DataSet();

                typeDataAdapter.Fill(typeDataSet);
                detailDataAdapter.Fill(detailDataSet);

                var emptyModelRow = typeDataSet.Tables[0].NewRow();
                emptyModelRow["Тип"] = "Всі";
                typeDataSet.Tables[0].Rows.InsertAt(emptyModelRow, 0);

                var emptyPartRow = detailDataSet.Tables[0].NewRow();
                emptyPartRow["Деталі"] = "Всі";
                detailDataSet.Tables[0].Rows.InsertAt(emptyPartRow, 0);

                bunifuDropdown1.DataSource = typeDataSet.Tables[0];
                bunifuDropdown1.DisplayMember = "Тип";
                bunifuDropdown1.ValueMember = "Тип";

                bunifuDropdown2.DataSource = detailDataSet.Tables[0];
                bunifuDropdown2.DisplayMember = "Деталі";
                bunifuDropdown2.ValueMember = "Деталі";
            }

            userInfo user = new userInfo();
            label3.Text = user.GetName();
            using (var connection = new MySqlConnection(@"Datasource=127.0.0.1;Port=3306;SslMode=none;Username=root;Password=root;Database=dbForCourseWork;"))
            {
                using (var queryToDb = new MySqlCommand($"SELECT * FROM goods", connection))
                {
                    using (var sqldataadapter = new MySqlDataAdapter(queryToDb))
                    {
                        var datatable = new DataTable();
                        sqldataadapter.Fill(datatable);

                        dataGridView1.DataSource = datatable;
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow clickedRow = this.dataGridView1.Rows[e.RowIndex];
                string id, type, detail, category, price, inStock;

                id = clickedRow.Cells[0].Value.ToString();
                type = clickedRow.Cells[1].Value.ToString();
                detail = clickedRow.Cells[2].Value.ToString();
                category = clickedRow.Cells[3].Value.ToString();
                price = clickedRow.Cells[4].Value.ToString();
                inStock = clickedRow.Cells[5].Value.ToString();

                idTextBox.PlaceholderText = id;
                typeTextBox.PlaceholderText = type;
                detailTextBox.PlaceholderText = detail;
                categoryTextBox.PlaceholderText = category;
                priceTextBox.PlaceholderText = price;
                inStockTextBox.PlaceholderText = inStock;
            }
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }

        private void minimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new MySqlConnection(@"Datasource=127.0.0.1;Port=3306;SslMode=none;Username=root;Password=root;Database=dbForCourseWork;"))
                {
                    connection.Open();
                    if (bunifuDropdown1.Text == "Всі" && bunifuDropdown2.Text == "Всі")
                    {
                        using (var queryToDb = new MySqlCommand($"SELECT * FROM goods", connection))
                        {
                            using (var sqldataadapter = new MySqlDataAdapter(queryToDb))
                            {
                                var datatable = new DataTable();
                                sqldataadapter.Fill(datatable);

                                dataGridView1.DataSource = datatable;
                            }
                        }
                    }
                    else if (bunifuDropdown1.Text == "Всі" && bunifuDropdown2.Text != "Всі")
                    {
                        using (var queryToDb = new MySqlCommand($"SELECT * FROM goods WHERE Деталі='{bunifuDropdown2.Text}'", connection))
                        {
                            using (var sqldataadapter = new MySqlDataAdapter(queryToDb))
                            {
                                var datatable = new DataTable();
                                sqldataadapter.Fill(datatable);

                                dataGridView1.DataSource = datatable;
                            }
                        }
                    }
                    else if (bunifuDropdown1.Text != "Всі" && bunifuDropdown2.Text == "Всі")
                    {
                        using (var queryToDb = new MySqlCommand($"SELECT * FROM goods WHERE Тип='{bunifuDropdown1.Text}'", connection))
                        {
                            using (var sqldataadapter = new MySqlDataAdapter(queryToDb))
                            {
                                var datatable = new DataTable();
                                sqldataadapter.Fill(datatable);

                                dataGridView1.DataSource = datatable;
                            }
                        }
                    }
                    else
                    {
                        using (var queryToDb = new MySqlCommand($"SELECT * FROM goods WHERE Тип='{bunifuDropdown1.Text}' AND Деталі='{bunifuDropdown2.Text}'", connection))
                        {
                            using (var sqldataadapter = new MySqlDataAdapter(queryToDb))
                            {
                                var datatable = new DataTable();
                                sqldataadapter.Fill(datatable);

                                dataGridView1.DataSource = datatable;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            if (this.listItems == "")
            {
                MessageBox.Show("Немає вибраних елементів.");
            }
            else
            {
                var confWin = new OrderConfirmation();
                confWin.userWin = this;
                confWin.Show();
                this.Enabled = false;
            }
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(quantityTextBox.Text) <= int.Parse(inStockTextBox.PlaceholderText))
                {
                    listItems += typeTextBox.PlaceholderText + ": " + detailTextBox.PlaceholderText + " (" + categoryTextBox.PlaceholderText + ") - " + quantityTextBox.Text + " x " + priceTextBox.PlaceholderText + "₴ \n";
                    total += int.Parse(priceTextBox.PlaceholderText) * int.Parse(quantityTextBox.Text);
                    updateConn += "UPDATE goods SET Наявність='" + (int.Parse(inStockTextBox.PlaceholderText) - int.Parse(quantityTextBox.Text)) + "' WHERE id='" + idTextBox.PlaceholderText + "';";
                    string message = "Додано " + detailTextBox.PlaceholderText + ", типу " + typeTextBox.PlaceholderText + ". Категорія: " + categoryTextBox.PlaceholderText + ". \nЦіна: " + priceTextBox.PlaceholderText + "грн. Кількість: " + quantityTextBox.Text + " шт.";
                    MessageBox.Show(message + "\nУспішно додано до кошику.");
                }
                else
                {
                    MessageBox.Show("Недостатньо товарів на складі");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            var orders = new Orders();
            this.Hide();
            orders.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.Show();
        }
    }
}
