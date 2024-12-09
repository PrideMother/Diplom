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
    public partial class zayavi : Form
    {
#pragma warning disable CS0414 // Полю "zayavi.adapter" присвоено значение, но оно ни разу не использовано.
        private MySqlDataAdapter adapter = null;
#pragma warning restore CS0414 // Полю "zayavi.adapter" присвоено значение, но оно ни разу не использовано.
#pragma warning disable CS0414 // Полю "zayavi.table" присвоено значение, но оно ни разу не использовано.
        private DataTable table = null;
#pragma warning restore CS0414 // Полю "zayavi.table" присвоено значение, но оно ни разу не использовано.
        db db = new db();
#pragma warning disable CS0649 // Полю "zayavi.currentUserID" нигде не присваивается значение, поэтому оно всегда будет иметь значение по умолчанию 0.
        private int currentUserID; // Хранит ID текущего пользователя
#pragma warning restore CS0649 // Полю "zayavi.currentUserID" нигде не присваивается значение, поэтому оно всегда будет иметь значение по умолчанию 0.
        public zayavi()
        {
            InitializeComponent();
        }

        private void zayavi_Load(object sender, EventArgs e)
        {
            // Получаем заявки для текущего пользователя
            ZayavkaService service = new ZayavkaService("SELECT * FROM Zayavki");
            List<Zayavka> userZayavki = service.GetUserZayavki(currentUserID);

            // Заполняем DataGridView
            dataGridView1.DataSource = userZayavki;
        }
    }
}
