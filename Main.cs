using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace diplom2
{
    public partial class Main : Form
    {
#pragma warning disable CS0414 // Полю "Main.adapter" присвоено значение, но оно ни разу не использовано.
        private MySqlDataAdapter adapter = null;
#pragma warning restore CS0414 // Полю "Main.adapter" присвоено значение, но оно ни разу не использовано.
        private DataTable table = null;
        db db = new db();

        private string sortColumn = null;
        private SortOrder sortOrder = SortOrder.None;

        public Main()
        {
            InitializeComponent();

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем ID заявки из выбранной строки
                if (dataGridView1.SelectedRows[0].Cells["id"].Value != null)
                {
                    int zayavkaId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                    //  ... можете здесь выполнять дополнительные действия с ID
                }
                else
                {
                    // Обработка случая, когда ячейка "id" пуста (по желанию)
                    MessageBox.Show("ID заявки не задано.");
                }

            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }

        private void LoadGridData()
        {
            using (MySqlConnection connection = db.getConnection()) // Используйте using для автоматического закрытия соединения
            {
                //connection.Open();
                // Используем JOIN для получения информации из связанных таблиц
                string query = @"SELECT 
                                    zayavki.id,
                                    users.name AS Пользователь,
                                    zayavki.*, 
                                    COALESCE(message.mess, '') AS Сообщение
                                FROM 
                                    zayavki
                                LEFT JOIN 
                                    message ON zayavki.message_id = message.id
                                LEFT JOIN 
                                    users ON zayavki.name_id = users.id";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    table = new DataTable();
                    adapter.Fill(table);

                    // Скрываем столбцы name_id и message_id
                    table.Columns.Remove("name_id");
                    table.Columns.Remove("message_id");
                    table.Columns.Remove("id1");

                    // Применение сортировки при загрузке данных (если она была установлена)
                    if (sortColumn != null)
                    {
                        table.DefaultView.Sort = $"{sortColumn} {(sortOrder == SortOrder.Ascending ? "ASC" : "DESC")}";
                    }

                    dataGridView1.DataSource = table.DefaultView;
                } 
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Фильтрация по полю "org"
            FilterGridData(textBox4.Text);
        }

        private void FilterGridData(string orgName)
        {
            using (MySqlConnection connection = db.getConnection())
            {
                //connection.Open();

                string query = @"SELECT 
                            zayavki.id,
                            users.name AS Пользователь,
                            zayavki.*, 
                            COALESCE(message.mess, '') AS Сообщение
                        FROM 
                            zayavki
                        LEFT JOIN 
                            message ON zayavki.message_id = message.id
                        LEFT JOIN 
                            users ON zayavki.name_id = users.id
                        WHERE 
                            zayavki.org LIKE @org";

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@org", "%" + orgName + "%");
                    DataTable zayavkiDataTable = new DataTable();
                    adapter.Fill(zayavkiDataTable);

                    // Скрываем столбцы name_id и message_id
                    zayavkiDataTable.Columns.Remove("name_id");
                    zayavkiDataTable.Columns.Remove("message_id");
                    zayavkiDataTable.Columns.Remove("id1");

                    // Применение сортировки после фильтрации (если она была установлена)
                    if (sortColumn != null)
                    {
                        zayavkiDataTable.DefaultView.Sort = $"{sortColumn} {(sortOrder == SortOrder.Ascending ? "ASC" : "DESC")}";
                    }

                    dataGridView1.DataSource = zayavkiDataTable.DefaultView;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FilterGridData(textBox1.Text);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            FilterGridData(dateTimePicker1.Text);
        }


        private void UpdateMessageText(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["message_text"].Index)
            {
                // Получаем ID заявки из текущей строки
                int zayavkaId = Convert.ToInt32(e.RowIndex);

                // Получаем текст сообщения из базы данных
                string messageText = GetMessageTextForZayavka(zayavkaId);

                // Заменяем текст в ячейке DataGridView
                e.Value = messageText;
            }
        }

        // Метод для получения текста сообщения по ID заявки
        private string GetMessageTextForZayavka(int zayavkaId)
        {
            using (MySqlConnection connection = db.getConnection())
            {
                connection.Open();
                string query = "SELECT mess FROM message WHERE id = (SELECT message_id FROM zayavki WHERE id = @zayavkaId)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@zayavkaId", zayavkaId);

                object result = command.ExecuteScalar();
                connection.Close();

                return result != null && result != DBNull.Value ? result.ToString() : "";
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["message_text"].Index)
            {
                // Получаем ID заявки из текущей строки DataGridView
                int zayavkaId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                // Получаем текст сообщения по ID заявки
                string messageText = GetMessageTextForZayavka(zayavkaId);
                // Устанавливаем текст сообщения в ячейку
                e.Value = messageText;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            FilterGridData(textBox3.Text);
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            FilterGridData(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            FilterGridData(textBox5.Text);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            UpdateDataGridView();

        }

        private void UpdateDataGridView()
        {
            string connection = "server=localhost; user id = root; password=root;database=diplom";
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();
                string query = "select * from zayavki";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, con))
                {
                    var ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
                con.Close();
            }
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns["id"].HeaderText = "Номер заявки";
            //dataGridView1.Columns["name"].HeaderText = "ФИО";
            dataGridView1.Columns["theme"].HeaderText = "Тема";
            dataGridView1.Columns["descr"].HeaderText = "Описание";
            dataGridView1.Columns["org"].HeaderText = "Организация";
            dataGridView1.Columns["status"].HeaderText = "Статус";
            dataGridView1.Columns["date"].HeaderText = "Дата";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void buttonInWork_Click(object sender, EventArgs e)
        {
            ChangeStatus("В работе");
        }

        private void buttonCompleted_Click(object sender, EventArgs e)
        {
            ChangeStatus("Выполнена");
            
        }

        private void buttonWaiting_Click(object sender, EventArgs e)
        {
            ChangeStatus("В ожидании");
        }


        private void UpdateStatus(int zayavkaId, string newStatus)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost; user id = root; password=root;database=diplom"))
            {
                connection.Open();
                string query = "UPDATE zayavki SET status = @newStatus WHERE id = @zayavkaId";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newStatus", newStatus);
                    command.Parameters.AddWithValue("@zayavkaId", zayavkaId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Общий метод для изменения статуса заявки
        private void ChangeStatus(string newStatus)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int zayavkaId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                // Обновляем статус заявки в базе данных
                UpdateStatus(zayavkaId, newStatus);

                // Обновляем dataGridView1
               // RefreshDataGridView(); // Добавляем метод RefreshDataGridView

                //  Дополнительная проверка, что строка была обновлена:
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToInt32(row.Cells["id"].Value) == zayavkaId)
                    {
                        row.Cells["status"].Value = newStatus; // Устанавливаем новый статус
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите строку с заявкой!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void dataGridView1_MouseHover(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            Point mousePosition = pictureBox2.PointToClient(Cursor.Position);

            // Определение положения подсказки
            int x = mousePosition.X - 10; // Смещение по горизонтали
            int y = mousePosition.Y - 10; // Смещение по вертикали

            // Отображение подсказки
            toolTip1.SetToolTip(pictureBox2, "В работе");

        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            Point mousePosition = pictureBox4.PointToClient(Cursor.Position);

            // Определение положения подсказки
            int x = mousePosition.X - 10; // Смещение по горизонтали
            int y = mousePosition.Y - 10; // Смещение по вертикали

            // Отображение подсказки
            toolTip1.SetToolTip(pictureBox4, "В ожидании");
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            Point mousePosition = pictureBox3.PointToClient(Cursor.Position);

            // Определение положения подсказки
            int x = mousePosition.X - 10; // Смещение по горизонтали
            int y = mousePosition.Y - 10; // Смещение по вертикали

            // Отображение подсказки
            toolTip1.SetToolTip(pictureBox3, "Выполнено");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем ID заявки из выбранной строки
                int zayavkaId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                // Создаем форму для комментария
                Message message = new Message(zayavkaId);
                message.ShowDialog();

                // После закрытия формы обновления данных
            }
            else
            {
                MessageBox.Show("Выберите заявку для добавления комментария.");
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string clickedColumn = dataGridView1.Columns[e.ColumnIndex].Name;

            // Изменяем порядок сортировки, если кликнули по уже отсортированному столбцу
            if (sortColumn == clickedColumn)
            {
                sortOrder = (sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                sortColumn = clickedColumn;
                sortOrder = SortOrder.Ascending;
            }

            // Перезагружаем данные, применяя сортировку
            LoadGridData();
        }
    }
}
