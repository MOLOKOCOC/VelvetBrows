using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Velvet_brows.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminPanelPage.xaml
    /// </summary>
    public partial class AdminPanelPage : Window
    {
        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        static readonly String connectionString = "server=(LocalDB)\\MSSQLLocalDB; Initial Catalog=C:\\USERS\\ДНС\\BROWSVELVET.MDF;Integrated Security=SSPI";
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private DataSet ds;
        public AdminPanelPage()
        {

            InitializeComponent();
            /// <summary>
            /// При инициализации выводим наш метод с с выводом в лист
            /// </summary>
            FillList();
        }

        private void UpcomingEntries(object sender, RoutedEventArgs e)
        {
            UpcomingEntriesPage UEP = new UpcomingEntriesPage();
            UEP.Show();
            this.Close();
        }

        private void AdminLogin(object sender, RoutedEventArgs e)
        {
            AddPege AP = new AddPege();
            AP.Show();
            this.Close();
        }
        /// <summary>
        /// Запрос на вывод всех услуг из базы
        /// </summary>
        public void FillList()
        {
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Service");
                Services co = new Services();
                IList<Services> co1 = new List<Services>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    co1.Add(new Services
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString(),
                        Cost = Convert.ToDecimal(dr[2].ToString()),
                        DurationInSeconds = Convert.ToInt32(dr[3].ToString()),
                        Description = dr[4].ToString(),
                        Discount = Convert.ToDouble(dr[5].ToString()),
                        MainImagePath = Convert.ToString(dr[6].ToString())
                    });

                }
                lstBox.ItemsSource = co1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR DATABASE");
            }
            finally
            {
                ds = null;
                adapter.Dispose();
                con.Close();
                con.Dispose();
            }
        }
        /// <summary>
        /// Переход на другую страницу
        /// </summary>
        public class Services
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public decimal Cost { get; set; }
            public int DurationInSeconds { get; set; }
            public string Description { get; set; }
            public double Discount { get; set; }
            public string MainImagePath { get; set; }
        }
        /// <summary>
        /// Запрос на поиск по услугам
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string intext = text.Text.ToString();
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service WHERE Title LIKE '" + intext.ToString() + "%' OR Cost LIKE '" + intext.ToString() + "%' ", con);
                adapter = new SqlDataAdapter(cmd);

                ds = new DataSet();
                adapter.Fill(ds, "Service");
                Services co = new Services();
                IList<Services> co1 = new List<Services>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    co1.Add(new Services
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString(),
                        Cost = Convert.ToDecimal(dr[2].ToString()),
                        DurationInSeconds = Convert.ToInt32(dr[3].ToString()),
                        Description = dr[4].ToString(),
                        Discount = Convert.ToDouble(dr[5].ToString()),
                        MainImagePath = Convert.ToString(dr[6].ToString())
                    });

                }
                lstBox.ItemsSource = co1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR DATABASE");
            }
            finally
            {
                ds = null;
                adapter.Dispose();
                con.Close();
                con.Dispose();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddPage UEPa = new AddPage();
            UEPa.Show();
            this.Close();
        }
        /// <summary>
        /// Функция удаления из базы
        /// </summary>
        private void Removal(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Удалить услугу",
                      "Предупреждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    int a = Convert.ToInt32((sender as Button).Uid);
                    con = new SqlConnection(connectionString);
                    con.Open();
                    cmd = new SqlCommand("DELETE FROM Service WHERE ID = " + a + "", con);
                    adapter = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    adapter.Fill(ds, "Service");
                    ds = null;
                    adapter.Dispose();
                    con.Close();
                    con.Dispose();
                    FillList();

                }
                else
                {
                    MessageBox.Show("Ошибка со стороны сервера");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        /// <summary>
        /// Переход на другую страницу
        /// </summary>
        private void UpClick(object sender, RoutedEventArgs e)
        {
            int a = Convert.ToInt32((sender as Button).Uid);
            AddPage UEPa = new AddPage(a);
            UEPa.Show();
            this.Close();
        }
    }
}
