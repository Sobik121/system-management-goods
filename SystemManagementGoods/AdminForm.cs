using Bunifu.UI.WinForms;
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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            transPanel.Height = homeBtn.Height;
            panel2.BringToFront();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Close();
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
        private void homeBtn_Click(object sender, EventArgs e)
        {
            transPanel.Height = homeBtn.Height;
            transPanel.Top = homeBtn.Top;
            panel2.BringToFront();
        }

        private void createUserBtn_Click(object sender, EventArgs e)
        {
            transPanel.Height = createUserBtn.Height;
            transPanel.Top = createUserBtn.Top;
            panel3.BringToFront();
        }

        private void listUsersBtn_Click(object sender, EventArgs e)
        {
            transPanel.Height = listUsersBtn.Height;
            transPanel.Top = listUsersBtn.Top;
            var users = new ListOfUsers();
            users.Show();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            var userhome = new UserHome();
            userhome.Show();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            var manager = new ManagerForm();
            manager.Show();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            var manager = new ManagerForm();
            manager.Show();
        }

        private void makePaidBtn_Click(object sender, EventArgs e)
        {
            var manager = new ManagerForm();
            manager.Show();
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

        private void registrationBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(firstName.Text) && !string.IsNullOrEmpty(surname.Text) && !string.IsNullOrEmpty(phone.Text) && !string.IsNullOrEmpty(logintbox.Text) &&
        !string.IsNullOrEmpty(password.Text) && !string.IsNullOrEmpty(typeDropDown.Text) && !string.IsNullOrEmpty(repeatPassword.Text))
            {
                if (password.Text == repeatPassword.Text)
                {
                    Regex validatePhoneNumberRegex = new Regex("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$");
                    if (validatePhoneNumberRegex.IsMatch(phone.Text))
                    {
                        try
                        {
                            using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                            {
                                connection.Open();
                                using (var cmd = new MySqlCommand($"insert into `userdetails` (`firstName`, `secondName`, `phone`) values ('{firstName.Text.Trim()}', '{surname.Text.Trim()}', '{phone.Text.Trim()}')", connection))
                                {
                                    cmd.ExecuteNonQuery();
                                }

                                using (var cmd = new MySqlCommand($"insert into `useraccounts` (`username`, `password`, `type`) values ('{logintbox.Text.Trim()}', '{SHA256HashMethod(password.Text.Trim())}', '{typeDropDown.Text.Trim()}')", connection))
                                {
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
                    }
                    else
                    {
                        var redColor = Color.FromArgb(255, 0, 0);
                        phone.BorderColorIdle = redColor;
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
    }
}
