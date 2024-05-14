using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace SystemManagementGoods
{
    public partial class ManagerForm : Form
    {

        
        public ManagerForm()
        {
            InitializeComponent();
            addGoodPanel.BringToFront();
            transPanel.Top = addBtn.Top;
            transPanel.Height = addBtn.Height;
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {
            fillData(dataGridView1);
        }

        void fillData(DataGridView dataGridViewDigit)
        {
            using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
            {
                connection.Open();
                var datatable = new DataTable();
                using (var msda = new MySqlDataAdapter("select * from goods", connection))
                {
                    msda.Fill(datatable);
                }
                dataGridViewDigit.DataSource = datatable;
            }
        }

        void fillOrdersDataGView(DataGridView dataGridViewOrder, string s)
        {

            using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
            {
                connection.Open();
                var datatable = new DataTable();
                using (var msda = new MySqlDataAdapter($"select * from orders where `Оплачено` = '{s}' ", connection))
                {
                    msda.Fill(datatable);
                }
                dataGridViewOrder.DataSource = datatable;
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

        // Додання товару

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow clickedRow = this.dataGridView1.Rows[e.RowIndex];
                string type, part, price, inStock;

                type = clickedRow.Cells[1].Value.ToString();
                part = clickedRow.Cells[2].Value.ToString();
                price = clickedRow.Cells[4].Value.ToString();
                inStock = clickedRow.Cells[5].Value.ToString();

                typeTextBox.Text = type;
                detailTextBox.Text = part;
                priceTextBox.Text = price;
                inStockTextBox.Text = inStock;

            }
        }

        private void addGoodBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(typeTextBox.Text) && !string.IsNullOrEmpty(detailTextBox.Text) && !string.IsNullOrEmpty(categoryTextBox.Text) && !string.IsNullOrEmpty(priceTextBox.Text) && !string.IsNullOrEmpty(inStockTextBox.Text))
            {
                try
                {
                    using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();

                        using (var command = new MySqlCommand("insert into `goods`(`Тип`,`Деталі`,`Категорія`,`Ціна`,`Наявність`) values('" + typeTextBox.Text.Trim() + "','" + detailTextBox.Text.Trim() + "','" + categoryTextBox.Text.Trim() + "','" + priceTextBox.Text.Trim() + "','" + inStockTextBox.Text.Trim() + "')", connection))
                        {
                            command.ExecuteNonQuery();

                            MessageBox.Show("Товар додано успішно!");

                            typeTextBox.Text = "";
                            detailTextBox.Text = "";
                            priceTextBox.Text = "";
                            inStockTextBox.Text = "";
                            categoryTextBox.Text = "";

                            fillData(dataGridView1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Ви повинні заповнити всі поля!");
            }
        }



        // Обновлення товару

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var clickedRow = this.dataGridView2.Rows[e.RowIndex];
                string id, type, detail, category, price, inStock;

                id = clickedRow.Cells[0].Value.ToString();
                type = clickedRow.Cells[1].Value.ToString();
                detail = clickedRow.Cells[2].Value.ToString();
                category = clickedRow.Cells[3].Value.ToString();
                price = clickedRow.Cells[4].Value.ToString();
                inStock = clickedRow.Cells[5].Value.ToString();

                refreshIdGoodTBox.PlaceholderText = id;
                refreshTypeTBox.Text = type;
                refreshCategoryTextBox.Text = category;
                refreshDetailTBox.Text = detail;
                refreshPriceTBox.Text = price;
                refreshInStockTBox.Text = inStock;
            }
        }

        private void refreshGoodBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(refreshTypeTBox.Text) && !string.IsNullOrEmpty(refreshDetailTBox.Text) && !string.IsNullOrEmpty(refreshCategoryTextBox.Text) && !string.IsNullOrEmpty(refreshPriceTBox.Text) && !string.IsNullOrEmpty(refreshInStockTBox.Text))
            {
                try
                {
                    using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();

                        using (var command = new MySqlCommand("update `goods` set `Тип`= '" + refreshTypeTBox.Text + "',`Деталі`= '" + refreshDetailTBox.Text + "',`Категорія`= '" + refreshCategoryTextBox.Text + "',`Ціна`='" + refreshPriceTBox.Text + "', `Наявність`= '" + refreshInStockTBox.Text + "'where `id`= '" + refreshIdGoodTBox.PlaceholderText + "' ", connection))
                        {
                            command.ExecuteNonQuery();

                            MessageBox.Show("Товар оновлено успішно!");

                            refreshIdGoodTBox.PlaceholderText = "Авто";
                            refreshTypeTBox.PlaceholderText = "Виберіть";
                            refreshDetailTBox.PlaceholderText = "Виберіть";
                            refreshPriceTBox.PlaceholderText = "Виберіть";
                            refreshInStockTBox.PlaceholderText = "Виберіть";
                            refreshCategoryTextBox.PlaceholderText = "Виберіть";

                            fillData(dataGridView2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Ви повинні вибрати рядок перед оновленням!");
            }
        }

        private void generatePDF_Click(object sender, EventArgs e)
        {
            exportToPdfMethod(dataGridView2);
        }


        // Видалення товару

        private void deleteGoodBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(deleteTypeTBox.PlaceholderText) && !string.IsNullOrEmpty(deleteDetailTBox.PlaceholderText) && !string.IsNullOrEmpty(deleteCategoryTBox.PlaceholderText) && !string.IsNullOrEmpty(deletePriceTBox.PlaceholderText) && !string.IsNullOrEmpty(deleteInStockTBox.PlaceholderText))
            {
                try
                {
                    using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();

                        using (var command = new MySqlCommand("delete from `goods` where `id`= '" + deleteIdTBox.PlaceholderText + "' ", connection))
                        {
                            command.ExecuteNonQuery();

                            MessageBox.Show("Товар видалено успішно!");

                            deleteIdTBox.PlaceholderText = "Авто";
                            deleteTypeTBox.PlaceholderText = "Виберіть";
                            deleteDetailTBox.PlaceholderText = "Виберіть";
                            deleteCategoryTBox.PlaceholderText = "Виберіть";
                            deletePriceTBox.PlaceholderText = "Виберіть";
                            deleteInStockTBox.PlaceholderText = "Виберіть";

                            fillData(dataGridView3);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Ви повинні вибрати рядок перед видаленням!");
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var clickedRow = this.dataGridView3.Rows[e.RowIndex];
                string id, type, detail, category, price, inStock;

                id = clickedRow.Cells[0].Value.ToString();
                type = clickedRow.Cells[1].Value.ToString();
                detail = clickedRow.Cells[2].Value.ToString();
                category = clickedRow.Cells[3].Value.ToString();
                price = clickedRow.Cells[4].Value.ToString();
                inStock = clickedRow.Cells[5].Value.ToString();

                deleteIdTBox.PlaceholderText = id;
                deleteTypeTBox.PlaceholderText = type;
                deleteDetailTBox.PlaceholderText = detail;
                deleteCategoryTBox.PlaceholderText = category;
                deletePriceTBox.PlaceholderText = price;
                deleteInStockTBox.PlaceholderText = inStock;
            }
        }

        // Оплачені замовлення

        private void makeUnpaidBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(paidOrdersIdTBox.PlaceholderText) && !string.IsNullOrEmpty(paidOrdersDescrTBox.PlaceholderText))
            {
                try
                {
                    using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();

                        using (var command = new MySqlCommand("update `orders` set `Оплачено`= '" + "Ні" + "'where `id`= '" + paidOrdersIdTBox.PlaceholderText + "' ", connection))
                        {
                            command.ExecuteNonQuery();

                            MessageBox.Show("Замовлення було відмічено та переміщено до таблиці неоплачених замовлень!");

                            deleteIdTBox.PlaceholderText = "Авто";
                            paidOrdersDescrTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersPartTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersPriceTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersStatusTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersIdTBox.PlaceholderText = "Авто";

                            fillData(dataGridView4);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Виберіть товар, щоб змінити статус оплати!");
            }
        }

        private void paidCancelOrderBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(paidOrdersIdTBox.PlaceholderText) && !string.IsNullOrEmpty(paidOrdersDescrTBox.PlaceholderText))
            {
                try
                {
                    using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();

                        using (var command = new MySqlCommand("update `orders` set `Оплачено`= '" + "Скасовано" + "'where `id`= '" + paidOrdersIdTBox.PlaceholderText + "' ", connection))
                        {
                            command.ExecuteNonQuery();

                            MessageBox.Show("Замовлення було відмічено та переміщено до таблиці неоплачених замовлень!");

                            paidOrdersDescrTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersPartTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersPriceTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersStatusTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersIdTBox.PlaceholderText = "Авто";

                            fillOrdersDataGView(dataGridView4, "yes");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Виберіть товар, щоб скасувати замовлення!");
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var clickedRow = this.dataGridView4.Rows[e.RowIndex];
                string id, descr, detail, price, status;

                id = clickedRow.Cells[0].Value.ToString();
                descr = clickedRow.Cells[1].Value.ToString();
                detail = clickedRow.Cells[2].Value.ToString();
                price = clickedRow.Cells[3].Value.ToString();
                status = clickedRow.Cells[4].Value.ToString();

                paidOrdersIdTBox.PlaceholderText = id;
                paidOrdersDescrTBox.PlaceholderText = descr;
                paidOrdersPartTBox.PlaceholderText = detail;
                paidOrdersPriceTBox.PlaceholderText = price;
                paidOrdersStatusTBox.PlaceholderText = status;
            }
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            exportToPdfMethod(dataGridView4);
        }

        // Неоплачені замовлення

        private void makePaidBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bunifuTextBox1.PlaceholderText) && !string.IsNullOrEmpty(unpaidDescrTBox.PlaceholderText))
            {
                try
                {
                    using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();

                        using (var command = new MySqlCommand("update `orders` set `Оплачено`= '" + "Так" + "'where `id`= '" + bunifuTextBox1.PlaceholderText + "' ", connection))
                        {
                            command.ExecuteNonQuery();

                            MessageBox.Show("Замовлення було відмічено та переміщено до таблиці неоплачених замовлень!");

                            unpaidDescrTBox.PlaceholderText = "Виберіть замовлення";
                            unpaidPartTBox.PlaceholderText = "Виберіть замовлення";
                            unpaidPriceTBox.PlaceholderText = "Виберіть замовлення";
                            unpaidStatusTBox.PlaceholderText = "Виберіть замовлення";
                            bunifuTextBox1.PlaceholderText = "Авто";

                            fillOrdersDataGView(dataGridView5, "no");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Виберіть товар, щоб скасувати замовлення!");
            }
        }

        private void cancelUnpaidBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bunifuTextBox1.PlaceholderText) && !string.IsNullOrEmpty(unpaidDescrTBox.PlaceholderText))
            {
                try
                {
                    using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();

                        using (var command = new MySqlCommand("update `orders` set `Оплачено`= '" + "Скасовано" + "'where `id`= '" + bunifuTextBox1.PlaceholderText + "' ", connection))
                        {
                            command.ExecuteNonQuery();

                            MessageBox.Show("Замовлення було відмічено та переміщено до таблиці неоплачених замовлень!");

                            unpaidDescrTBox.PlaceholderText = "Виберіть замовлення";
                            unpaidPartTBox.PlaceholderText = "Виберіть замовлення";
                            unpaidPriceTBox.PlaceholderText = "Виберіть замовлення";
                            unpaidStatusTBox.PlaceholderText = "Виберіть замовлення";
                            bunifuTextBox1.PlaceholderText = "Авто";

                            fillOrdersDataGView(dataGridView5, "no");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Виберіть товар, щоб скасувати замовлення!");
            }
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var clickedRow = this.dataGridView5.Rows[e.RowIndex];
                string id, descr, detail, price, status;

                id = clickedRow.Cells[0].Value.ToString();
                descr = clickedRow.Cells[1].Value.ToString();
                detail = clickedRow.Cells[2].Value.ToString();
                price = clickedRow.Cells[3].Value.ToString();
                status = clickedRow.Cells[4].Value.ToString();

                bunifuTextBox1.PlaceholderText = id;
                unpaidDescrTBox.PlaceholderText = descr;
                unpaidPartTBox.PlaceholderText = detail;
                unpaidPriceTBox.PlaceholderText = price;
                unpaidStatusTBox.PlaceholderText = status;
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            this.addGoodPanel.Location = new Point(230, 76);
            forFixCursorBug(refreshGoodPanel, deleteGoodPanel, paidOrdersPanel, unpaidOrdersPanel);
            addGoodPanel.BringToFront();
            fillData(dataGridView1);
            typeTextBox.PlaceholderText = "Впишіть";
            detailTextBox.PlaceholderText = "Впишіть";
            priceTextBox.PlaceholderText = "Впишіть";
            inStockTextBox.PlaceholderText = "Впишіть";
            transPanel.Top = addBtn.Top;
            transPanel.Height = addBtn.Height;
            categoryTextBox.PlaceholderText = "Впишіть";
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            this.refreshGoodPanel.Location = new Point(230, 76);
            forFixCursorBug(addGoodPanel, deleteGoodPanel, paidOrdersPanel, unpaidOrdersPanel);
            refreshGoodPanel.BringToFront();
            fillData(dataGridView2);
            refreshTypeTBox.PlaceholderText = "Впишіть";
            refreshDetailTBox.PlaceholderText = "Впишіть";
            refreshPriceTBox.PlaceholderText = "Впишіть";
            refreshInStockTBox.PlaceholderText = "Впишіть";
            transPanel.Top = refreshBtn.Top;
            transPanel.Height = refreshBtn.Height;
            refreshCategoryTextBox.PlaceholderText = "Впишіть";
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            this.deleteGoodPanel.Location = new Point(230, 76);
            forFixCursorBug(addGoodPanel, refreshGoodPanel, paidOrdersPanel, unpaidOrdersPanel);
            deleteGoodPanel.BringToFront();
            fillData(dataGridView3);
            transPanel.Top = deleteBtn.Top;
            transPanel.Height = deleteBtn.Height;
        }

        private void paidOrdersBtn_Click(object sender, EventArgs e)
        {
            this.paidOrdersPanel.Location = new Point(230, 76);
            forFixCursorBug(addGoodPanel, refreshGoodPanel, deleteGoodPanel, unpaidOrdersPanel);
            paidOrdersPanel.BringToFront();
            fillOrdersDataGView(dataGridView4, "Так");
            paidOrdersDescrTBox.PlaceholderText = "Виберіть замовлення";
            paidOrdersPartTBox.PlaceholderText = "Виберіть замовлення";
            paidOrdersPriceTBox.PlaceholderText = "Виберіть замовлення";
            paidOrdersStatusTBox.PlaceholderText = "Виберіть замовлення";
            transPanel.Top = paidOrdersBtn.Top;
            transPanel.Height = paidOrdersBtn.Height;
            foreach (DataGridViewColumn column in dataGridView4.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void unpaidOrdersBtn_Click(object sender, EventArgs e)
        {
            this.unpaidOrdersPanel.Location = new Point(230, 76);
            forFixCursorBug(addGoodPanel, refreshGoodPanel, deleteGoodPanel, paidOrdersPanel);
            unpaidOrdersPanel.BringToFront();
            fillOrdersDataGView(dataGridView5, "Ні");
            unpaidDescrTBox.PlaceholderText = "Виберіть замовлення";
            unpaidPartTBox.PlaceholderText = "Виберіть замовлення";
            unpaidPriceTBox.PlaceholderText = "Виберіть замовлення";
            unpaidStatusTBox.PlaceholderText = "Виберіть замовлення";
            transPanel.Top = unpaidOrdersBtn.Top;
            transPanel.Height = unpaidOrdersBtn.Height;
            foreach (DataGridViewColumn column in dataGridView5.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void unpaidOrsExportToPDFBtn_Click(object sender, EventArgs e)
        {
            exportToPdfMethod(dataGridView5);
        }

        void forFixCursorBug(Panel panel, Panel panel1, Panel panel2, Panel panel3)
        {
            panel.Location = new Point(230, 627);
            panel1.Location = new Point(230, 627);
            panel2.Location = new Point(230, 627);
            panel3.Location = new Point(230, 627);
        }
        //


        void exportToPdfMethod(DataGridView datagview)
        {
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
            if (datagview.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Result.pdf";
                bool ErrorMessage = false;
                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Неможливо записати дані на диск:" + ex.Message);
                        }
                    }
                    if (!ErrorMessage)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(datagview.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            foreach (DataGridViewColumn col in datagview.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText, font));
                                pTable.AddCell(pCell);
                            }
                            foreach (DataGridViewRow viewRow in datagview.Rows)
                            {
                                foreach (DataGridViewCell dcell in viewRow.Cells)
                                {
                                    if (dcell.Value != null)
                                    {
                                        pTable.AddCell(new Phrase(dcell.Value.ToString(), font));
                                    }
                                }
                            }
                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("Експорт даних успішно завершено.", "info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Помилка під час експорту даних:" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Запис не знайдено", "Info");
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(paidOrdersIdTBox.PlaceholderText) && !string.IsNullOrEmpty(paidOrdersDescrTBox.PlaceholderText))
            {
                try
                {
                    using (var connection = new MySqlConnection(@"datasource=127.0.0.1;port=3306;SslMode=none;username=root;password=root;database=dbForCourseWork;"))
                    {
                        connection.Open();

                        using (var command = new MySqlCommand("update `orders` set `Доставлено`= '" + "Так" + "'where `id`= '" + paidOrdersIdTBox.PlaceholderText + "' ", connection))
                        {
                            command.ExecuteNonQuery();

                            MessageBox.Show("Замовлення було доставлено!");

                            deleteIdTBox.PlaceholderText = "Авто";
                            paidOrdersDescrTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersPartTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersPriceTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersStatusTBox.PlaceholderText = "Виберіть замовлення";
                            paidOrdersIdTBox.PlaceholderText = "Авто";

                            fillData(dataGridView4);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Виберіть товар, щоб змінити статус оплати!");
            }
        }
    }
}
