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
    public partial class OrderConfirmation : Form
    {
        public UserHome userWin { get; set; }
        public OrderConfirmation()
        {
            InitializeComponent();
        }

        private void OrderConfirmation_FormClosed(object sender, FormClosedEventArgs e)
        {
            userWin.Enabled = true;
        }

        private void OrderConfirmation_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = userWin.listItems;
            userInfo user = new userInfo();
            label3.Text = user.GetName();
            label8.Text = userWin.total.ToString() + " грн.";
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            userWin.total = 0;
            userWin.listItems = "";
            userWin.updateConn = "";
            this.Close();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {

                userInfo user = new userInfo();

                using (MySqlConnection connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                {
                    string query = "INSERT INTO `orders`(`Користувач`,`Деталі`,`Ціна`) VALUES('" + user.GetName() + "','" + userWin.listItems + "'," + userWin.total + ");" + userWin.updateConn + "";
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                    MessageBox.Show("Замовлення успішно оформлено!");
                    bunifuButton1_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка під час оформлення замовлення: " + ex.ToString());
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            userWin.Enabled = true;
            this.Close();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
