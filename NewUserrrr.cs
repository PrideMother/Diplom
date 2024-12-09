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
    public partial class NewUserrrr : Form
    {
        public NewUserrrr()
        {
            InitializeComponent();
            NameField.ForeColor = Color.Gray;
            PassField.ForeColor = Color.Gray;
 
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            db db = new db();

            MySqlCommand command = new MySqlCommand("insert into avtor(login, password, org_id, rab_id) values (@login, @password, DEFAULT, DEFAULT)", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = NameField.Text;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = PassField.Text;
            command.Parameters.Add("@org_id", MySqlDbType.Int32).Value = 1; // Замените '1' на ID организации
            command.Parameters.Add("@rab_id", MySqlDbType.Int32).Value = 2; // Замените '1' на ID организации
            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Регистрация прошла успешно");
            }
            else
            {
                MessageBox.Show("Регистрация не прошла успешно");
            }


            db.closeConnection();
        }

        private void NameField_Enter(object sender, EventArgs e)
        {
            if (NameField.Text == "Введите имя")
            {
                NameField.Text = "";
                NameField.ForeColor = Color.Black;
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




        private void Avtor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void PassField_Leave(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }
    }
}
