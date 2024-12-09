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
    public partial class Registracia : Form
    {
        public Registracia()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            db db = new db();

            MySqlCommand command = new MySqlCommand("insert into avtor(login, password) values (@login, @password)", db.getConnection());

            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = LoginField.Text;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = PassField.Text;

            db.openConnection();

            if (LoginField.TextLength <= 0)
            {
                MessageBox.Show("Не заполнено поле Логин");

            }

            if (PassField.TextLength <= 0)
            {
                MessageBox.Show("Не заполнено поле Пароль");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 FormMainMenu = new Form1();
            FormMainMenu.Show();
        }

        private void LoginField_Enter(object sender, EventArgs e)
        {
            if (LoginField.Text == "Введите логин")
            {
                LoginField.Text = "";
                LoginField.ForeColor = Color.Black;
            }
        }

        private void PassField_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginField_Leave(object sender, EventArgs e)
        {
            if (LoginField.Text == "")
            {
                LoginField.Text = "Введите логин";
                LoginField.ForeColor = Color.Gray;
            }
        }

        private void PassField_Leave(object sender, EventArgs e)
        {
            if (PassField.Text == "")
            {
                PassField.Text = "Введите пароль";
                PassField.ForeColor = Color.Gray;
            }
        }

        private void PassField_Enter(object sender, EventArgs e)
        {
            if (PassField.Text == "Введите пароль")
            {
                PassField.Text = "";
                PassField.ForeColor = Color.Black;
            }
        }
    }
}
