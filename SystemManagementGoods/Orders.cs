using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemManagementGoods
{
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
        }

        private void ShowMyOrders_Load(object sender, EventArgs e)
        {
            userInfo user = new userInfo();
            label3.Text = user.GetName();

            ShowUserOrderData();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        void ShowUserOrderData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                {
                    connection.Open();
                    string sqlQuery = "SELECT id AS 'Номер замовлення', Деталі, Ціна, Оплачено FROM orders WHERE Користувач = '" + label3.Text + "' AND Оплачено = 'Ні'";
                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        using (MySqlDataAdapter sqldataadapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            sqldataadapter.Fill(dataTable);
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void newOrderBtn_Click(object sender, EventArgs e)
        {
            UserHome userHome = new UserHome();
            userHome.Show();
            this.Hide();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            UserHome userHome = new UserHome();
            this.Close();
            userHome.Show();
        }

        private void deleteOrderBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string orderId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    using (MySqlConnection connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();
                        string sqlQuery = "DELETE FROM orders WHERE id = '" + orderId + "'";
                        using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                            command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Замовлення успішно видалено!");
                    ShowUserOrderData();
                }
                else
                {
                    MessageBox.Show("Будь ласка, виберіть замовлення для видалення.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка: " + ex.ToString());
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            PastOrders pastOrders = new PastOrders();
            pastOrders.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
