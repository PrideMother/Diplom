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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace diplom2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Login.Text = "Введите логин";
            Login.ForeColor = Color.Gray;
            Password.UseSystemPasswordChar = false;
            Password.Text = "Введите пароль";
            Password.ForeColor = Color.Gray;

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            String login = Login.Text;
            String password = Password.Text;

            db db = new db();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * from avtor where login = @uL and password = @uP", db.getConnection());

            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = login;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = password;

            adapter.SelectCommand = command;
            // adapter.SelectCommand = command1;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                this.Hide();

                Form4 form4 = new Form4();
                form4.Show();
            }
            else
            {
                MySqlCommand command1 = new MySqlCommand("SELECT * from users where name = @uL and password = @uP", db.getConnection());
                command1.Parameters.Add("@uL", MySqlDbType.VarChar).Value = login;
                command1.Parameters.Add("@uP", MySqlDbType.VarChar).Value = password;
                adapter.SelectCommand = command1;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    this.Hide();
                    User userForm = new User(); // Форма для обычного пользователя
                    userForm.Show();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void reg_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registracia FormMainMenu = new Registracia();
            FormMainMenu.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            User FormMainMenu = new User();
            FormMainMenu.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            NewUser newUser = new NewUser();
            newUser.ShowDialog();
        }

        private void reg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            NewUser registracia = new NewUser();
            registracia.ShowDialog();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (Login.Text == "")
            {
                Login.Text = "Введите логин";
                Login.ForeColor = Color.Gray;
            }
            
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (Login.Text == "Введите логин")
            {
                Login.Text = "";
                Login.ForeColor = Color.Black;
            }
        }

        private void Password_Leave(object sender, EventArgs e)
        {
            if (Password.Text == "")
            {
                Password.Text = "Введите пароль";
                Password.ForeColor = Color.Gray;
            }
        }

        private void Password_Enter(object sender, EventArgs e)
        {
            if (Password.Text == "Введите пароль")
            {
                Password.Text = "";
                Password.ForeColor = Color.Black;
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            NewUser newUser = new NewUser();
            newUser.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            User FormMainMenu = new User();
            FormMainMenu.ShowDialog();
        }

        private void pictureBox6_MouseHover(object sender, EventArgs e)
        {
            Point mousePosition = pictureBox6.PointToClient(Cursor.Position);

            // Определение положения подсказки
            int x = mousePosition.X - 10; // Смещение по горизонтали
            int y = mousePosition.Y - 10; // Смещение по вертикали

            // Отображение подсказки
            toolTip1.SetToolTip(pictureBox6, "Далее");

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
