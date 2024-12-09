using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class ZayavkaService
{
    private readonly string db;

    public ZayavkaService(string connectionString)
    {
        db = connectionString;
    }

    public List<Zayavka> GetUserZayavki(int userId)
    {
        List<Zayavka> userZayavki = new List<Zayavka>();

        using (SqlConnection connection = new SqlConnection(db))
        {
            string sqlQuery = "SELECT * FROM Zayavki WHERE id = @id";

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@id", userId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Zayavka zayavka = new Zayavka
                        {
                            Id = reader.GetInt32(0),
                            // ... другие свойства из таблицы Zayavki
                        };

                        userZayavki.Add(zayavka);
                    }
                }
            }
        }

        return userZayavki;
    }
}

// Класс Zayavka
public class Zayavka
{
    public int Id { get; set; }
    // ... другие свойства
}