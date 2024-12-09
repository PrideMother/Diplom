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
    public partial class NewUser : Form
    {
        public NewUser()
        {
            InitializeComponent();

        }

        private void Reg_Click(object sender, EventArgs e)
        {
            db db = new db();
            try
            {
                // Проверяем, заполнены ли все поля
                if (NameField.Text == "Введите имя" || PassField.Text == "Введите пароль" || OrgField.Text == "Введите организацию пользователя")
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                // Получаем название организации
                string orgName = OrgField.Text;

                // Находим org_id по названию организации
                int orgId = GetOrgIdByName(orgName, db);

                // Если org_id не найден, добавляем организацию
                if (orgId == 0)
                {
                    orgId = AddNewOrganization(orgName, db);
                    if (orgId == 0)
                    {
                        MessageBox.Show("Ошибка: Не удалось добавить организацию");
                        return;
                    }
                }

                // Добавляем пользователя в таблицу users
                string queryUser = "INSERT INTO users (name, password, org_id) VALUES (@name, @password, @org_id)";
                using (MySqlConnection connection = db.getConnection())
                {
                    using (MySqlCommand commandUser = new MySqlCommand(queryUser, connection))
                    {
                        commandUser.Parameters.AddWithValue("@name", NameField.Text);
                        commandUser.Parameters.AddWithValue("@password", PassField.Text);
                        commandUser.Parameters.AddWithValue("@org_id", orgId);

                        if (commandUser.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Регистрация прошла успешно");
                        }
                        else
                        {
                            MessageBox.Show("Регистрация не прошла успешно");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private int GetOrgIdByName(string orgName, db db)
        {
            int orgId = 0;
            string query = "SELECT id FROM org WHERE name = @name";
            using (MySqlConnection connection = db.getConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", orgName);
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out orgId))
                    {
                        return orgId;
                    }
                }
            }
            return 0;
        }

        // Метод для добавления новой организации
        private int AddNewOrganization(string orgName, db db)
        {
            int orgId = 0;
            string query = "INSERT INTO org (name) VALUES (@name); SELECT LAST_INSERT_ID()";
            using (MySqlConnection connection = db.getConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", orgName);
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out orgId))
                    {
                        return orgId;
                    }
                }
            }
            return 0;
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


        private void OrgField_Enter(object sender, EventArgs e)
        {
            if (OrgField.Text == "Введите организацию пользователя")
            {
                OrgField.Text = "";
                OrgField.ForeColor = Color.Black;
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form2 = new Form1();
            form2.ShowDialog();
        }

        private void NameField_Leave(object sender, EventArgs e)
        {
            if (NameField.Text == "")
            {
                NameField.Text = "Введите имя";
                NameField.ForeColor = Color.Gray;
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

        private void OrgField_Leave(object sender, EventArgs e)
        {
            if (OrgField.Text == "")
            {
                OrgField.Text = "Введите организацию пользователя";
                OrgField.ForeColor = Color.Gray;
            }
        }
    }
}
