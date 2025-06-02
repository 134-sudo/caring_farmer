using Npgsql;
using System;
using System.Windows.Forms;

namespace caring_farmer
{
    public partial class AuthForm : Form
    {
        NpgsqlConnection connection;
        public static bool authSuccessful = false;

        public AuthForm()
        {
            InitializeComponent();

            try
            {
                connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=caring_farmer;Username=postgres;Password=1111;");
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private void AuthBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string username = loginInput.Text;
                string password = passInput.Text;

                if (UserExists(username, password))
                {
                    connection.Close();
                    MessageBox.Show("Добро пожаловать, " + username + "!");
                    authSuccessful = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Допущена ошибка в данных пользователя.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка аутентификации: {ex.Message}");
            }
        }

        private bool UserExists(string username, string password)
        {
            try
            {
                using (var command = new NpgsqlCommand("SELECT count(*) FROM сотрудники WHERE логин = @Username AND пароль = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки пользователя: {ex.Message}");
                return false;
            }
        }
    }
}