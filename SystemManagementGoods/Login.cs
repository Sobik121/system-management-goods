using Bunifu.Framework.UI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemManagementGoods
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(@"Datasource=127.0.0.1;Port=3306;SslMode=none;Username=root;Password=root;Database=dbForCourseWork;");

                string username = bunifuTextBox2.Text.Trim();
                string password = SHA256HashMethod(bunifuTextBox1.Text.Trim());

                MySqlDataAdapter sqldataadapter = new MySqlDataAdapter("SELECT * FROM useraccounts WHERE username='" + username + "'AND password='" + password + "'", conn);
                DataTable datatable = new DataTable();
                sqldataadapter.Fill(datatable);

                if (datatable.Rows.Count == 1)
                {
                    MySqlCommand queryToDb = new MySqlCommand("SELECT * FROM useraccounts WHERE username='" + username + "'AND password='" + password + "'", conn);
                    conn.Open();
                    queryToDb.ExecuteNonQuery();
                    MySqlDataReader reader = queryToDb.ExecuteReader();
                    while (reader.Read())
                    {
                        userInfo user = new userInfo();
                        user.SetName(reader["username"].ToString());
                        switch (reader["type"].ToString())
                        {
                            case "user":
                                var userHome = new UserHome();
                                this.Hide();
                                userHome.Show();
                                break;

                            case "manager":
                                var manager = new ManagerForm();
                                this.Hide();
                                manager.Show();
                                break;

                            case "admin":
                                var admin = new AdminForm();
                                this.Hide();
                                admin.Show();
                                break;

                            default:
                                MessageBox.Show("Недійсний тип користувача");
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Неправильний логін або пароль");
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Сталася помилка: " + x.Message);
            }

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Hide();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static string SHA256HashMethod(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
