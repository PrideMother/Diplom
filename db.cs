using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace diplom2
{
    internal class db
    {
        private string connectionString = "server=localhost;port=3306;username=root;password=root;database=diplom";
        private MySqlConnection connection; // Сохранение подключения

        // Конструктор для инициализации подключения
        public db()
        {
            connection = new MySqlConnection(connectionString);
        }

        public void openConnection()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
        }

        public void closeConnection()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public MySqlConnection getConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open(); // Откройте соединение
            return connection;
        }
    }
}
