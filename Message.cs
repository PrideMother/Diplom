using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplom2
{
    public partial class Message : Form
    {
        private int zayavkaId;
        private db db = new db();
        public Message(int zayavkaId)
        {
            InitializeComponent();
            this.zayavkaId = zayavkaId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем текст комментария из текстового поля
            string commentText = textBox1.Text;

            // Сохранение комментария в БД
            string query = "INSERT INTO message (mess) VALUES (@mess)";
            using (MySqlConnection connection = db.getConnection())
            {
                //connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@mess", commentText);
                    command.ExecuteNonQuery();
                }
            }

            // Закрытие формы
            this.Close();
        }
    }
}
