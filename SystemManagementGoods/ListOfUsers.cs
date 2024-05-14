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
    public partial class ListOfUsers : Form
    {
        public ListOfUsers()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListOfUsers_Load(object sender, EventArgs e)
        {
            using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
            {
                connection.Open();
                var datatable = new DataTable();
                using (var msda = new MySqlDataAdapter("SELECT userdetails.firstName AS 'Ім*я', userdetails.secondName AS 'Прізвище', userdetails.phone AS 'Номер телефону', useraccounts.username AS 'Логін', useraccounts.type AS 'Тип користувача' FROM useraccounts LEFT JOIN userdetails ON usersaccounts.id = userdetails.id", connection))
                {
                    msda.Fill(datatable);
                }
                dataGridView1.DataSource = datatable;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
