using System.Windows;

namespace Velvet_brows.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminLoginPage.xaml
    /// </summary>
    public partial class AdminLoginPage : Window
    {
        public AdminLoginPage()
        {
            InitializeComponent();
        }

        private void Entrance(object sender, RoutedEventArgs e)
        {
            string login = LoginField.Password;

            if (login == "0000")
            {
                AdminPanelPage APP = new AdminPanelPage();
                APP.Show();
                this.Close();
            }
            else
            {
                _ = MessageBox.Show("Пароль не верный");
            }

        }
    }
}
