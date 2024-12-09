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
    public partial class Userrrr : Form
    {
        public Userrrr()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            db db = new db();
            // 1. Получение id пользователя из таблицы users по введенному имени
            int userId = GetUserIdByName(NameField.Text, db);

            if (userId == -1) // Если пользователь не найден
            {
                MessageBox.Show("Пользователь с таким именем не найден.");
                return;
            }

            // 2. Вставка записи в таблицу zayavki с полученным id пользователя
            MySqlCommand command = new MySqlCommand("insert into zayavki(name_id, theme, descr, org, date) values (@name_id, @theme, @descr, @org, @date)", db.getConnection());

            command.Parameters.Add("@name_id", MySqlDbType.Int32).Value = userId; // Передаем id пользователя
            command.Parameters.Add("@theme", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@descr", MySqlDbType.VarChar).Value = DescField.Text;
            command.Parameters.Add("@org", MySqlDbType.VarChar).Value = OrgField.Text;
            command.Parameters.AddWithValue("@date", Convert.ToDateTime(dateTimePicker1.Text));

            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Заявка оставлена");
            }
            else
            {
                MessageBox.Show("Ошибка");
            }

            db.closeConnection();
        }

        private int GetUserIdByName(string name, db database)
        {
            MySqlCommand command = new MySqlCommand("SELECT id FROM users WHERE name = @name", database.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;

            database.openConnection();
            object result = command.ExecuteScalar();
            database.closeConnection();

            if (result != null)
            {
                return Convert.ToInt32(result); // Возвращаем id пользователя
            }
            else
            {
                return -1; // Возвращаем -1, если пользователь не найден
            }
        }

        private void NameField_Enter(object sender, EventArgs e)
        {
            if (NameField.Text == "Введите имя")
                NameField.Text = "";
            NameField.ForeColor = Color.Black;
        }

        private void OrgField_Enter(object sender, EventArgs e)
        {
            if (NameField.Text == "Введите организацию")
                NameField.Text = "";
            NameField.ForeColor = Color.Black;
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            if (dateTimePicker1.Text == "")
                dateTimePicker1.Text = "";
            dateTimePicker1.ForeColor = Color.Black;
        }

        private void DescField_Enter(object sender, EventArgs e)
        {
            if (DescField.Text == "")
                DescField.Text = "";
            DescField.ForeColor = Color.Black;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.Show();
        }
    }
}
