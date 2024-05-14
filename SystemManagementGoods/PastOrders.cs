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
    public partial class PastOrders : Form
    {
        public PastOrders()
        {
            InitializeComponent();

            userInfo user = new userInfo();
            label3.Text = user.GetName();

            ShowUserPastOrdersData();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        void ShowUserPastOrdersData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                {
                    connection.Open();
                    string sqlQuery1 = "SELECT orders.id AS 'Номер замовлення', orders.Деталі, orders.Ціна, shipment.shipmentDate AS 'Дата доставки' from orders INNER JOIN shipment ON orders.id = shipment.shipment_id WHERE orders.Користувач = '" + label3.Text + "' AND orders.Оплачено = 'Так' AND orders.Доставлено = 'Так'";
                    string sqlQuery = "SELECT orders.id, orders.Деталі, orders.Ціна, shipment.shipmentDate INNER JOIN shipment ON orders.id = shipment.shipment_id WHERE orders.Користувач = '" + label3.Text + "' AND orders.Оплачено = 'Так' AND orders.Доставлено = 'Ні'";
                    using (MySqlCommand command = new MySqlCommand(sqlQuery1, connection))
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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Orders orders = new Orders();
            this.Close();
            orders.Show();
        }
    }
}
