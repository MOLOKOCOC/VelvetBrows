using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using System.Windows;

namespace Velvet_brows.Views
{
    /// <summary>
    /// Логика взаимодействия для UpcomingEntriesPage.xaml
    /// </summary>
    public partial class UpcomingEntriesPage : Window
    {
        static readonly string connectionString = "server=(LocalDB)\\MSSQLLocalDB; Initial Catalog=C:\\USERS\\ДНС\\BROWSVELVET.MDF;Integrated Security=SSPI";
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private DataSet ds;
        public UpcomingEntriesPage()
        {
            InitializeComponent();
            Timer timer = new Timer(30000)
            {
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += new ElapsedEventHandler(timerTick);
            timer.Start();
            FillList();
        }
        private void UpcomingEntries(object sender, RoutedEventArgs e)
        {
            MainWindow MW = new MainWindow();
            MW.Show();
            Close();
        }
        private void AdminLogin(object sender, RoutedEventArgs e)
        {
            AdminLoginPage ALP = new AdminLoginPage();
            ALP.Show();
            Close();
        }
        public void FillList()
        {
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT ClientService.ID,ClientService.ClientID, ClientService.ServiceID, ClientService.StartTime, ClientService.Comment, Client.FirstName, [Service].Title FROM ClientService, Client, [Service] WHERE Client.ID = ClientService.ClientID AND [Service].ID = ClientService.ServiceID", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "ClientService");
                Clients co = new Clients();
                IList<Clients> co1 = new List<Clients>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    co1.Add(new Clients
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        ClientID = Convert.ToInt32(dr[1].ToString()),
                        ServiceID = Convert.ToInt32(dr[2].ToString()),
                        StartTime = Convert.ToDateTime(dr[3].ToString()),
                        Comment = dr[4].ToString(),
                        FirstName = dr[5].ToString(),
                        Title = dr[6].ToString()
                    });
                }
                lstBox.ItemsSource = co1;
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message); }
            finally
            {
                ds = null;
                adapter.Dispose();
                con.Close();
                con.Dispose();
            }
        }
        private void timerTick(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                lstBox.Visibility = Visibility.Visible;
                if (lstBox.SelectedItems != null)
                {
                    lstBox.Items.Refresh();
                }
            });
        }
        public class Clients
        {
            public int ID { get; set; }
            public int ClientID { get; set; }
            public int ServiceID { get; set; }
            public DateTime StartTime { get; set; }
            public string Comment { get; set; }
            public string FirstName { get; set; }
            public string Title { get; set; }
        }
    }
}
