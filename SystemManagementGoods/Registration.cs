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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemManagementGoods
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if ( !string.IsNullOrEmpty(bunifuTextBox6.Text) && !string.IsNullOrEmpty(bunifuTextBox5.Text) && !string.IsNullOrEmpty(bunifuTextBox4.Text) && !string.IsNullOrEmpty(bunifuTextBox3.Text) &&
        !string.IsNullOrEmpty(bunifuTextBox2.Text) && !string.IsNullOrEmpty(bunifuTextBox1.Text) )
            {
                if (bunifuTextBox2.Text == bunifuTextBox1.Text)
                {
                    Regex validatePhoneNumberRegex = new Regex("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$");
                    if (validatePhoneNumberRegex.IsMatch(bunifuTextBox3.Text))
                    {
                        try
                        {
                            using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                            {
                                connection.Open();
                                using (var cmd = new MySqlCommand("insert into `userdetails` (`firstName`, `secondName`, `phone`) values (@firstName, @lastName, @number)", connection))
                                {
                                    cmd.Parameters.AddWithValue("@firstName", bunifuTextBox6.Text.Trim());
                                    cmd.Parameters.AddWithValue("@lastName", bunifuTextBox5.Text.Trim());
                                    cmd.Parameters.AddWithValue("@number", bunifuTextBox3.Text.Trim());

                                    cmd.ExecuteNonQuery();
                                }

                                using (var cmd = new MySqlCommand("insert into `useraccounts` (`username`, `password`) values (@username, @password)", connection))
                                {
                                    cmd.Parameters.AddWithValue("@username", bunifuTextBox4.Text.Trim());
                                    cmd.Parameters.AddWithValue("@password", SHA256HashMethod(bunifuTextBox2.Text.Trim()));

                                    cmd.ExecuteNonQuery();
                                }

                                connection.Close();

                                MessageBox.Show("Ваш обліковий запис успішно створено.");
                                Login login = new Login();
                                login.Show();
                                this.Hide();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Сталася помилка під час створення вашого облікового запису: " + ex.Message);
                        }
                    } else
                    {
                        Color redColor = Color.FromArgb(255, 0, 0);
                        bunifuTextBox3.BorderColorIdle = redColor;
                    }
                }
                else
                {
                    MessageBox.Show("Введені вами паролі не збігаються.");
                }
            }
            else
            {
                MessageBox.Show("Заповніть усі обов'язкові поля, перш ніж створити обліковий запис.");
            }
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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
