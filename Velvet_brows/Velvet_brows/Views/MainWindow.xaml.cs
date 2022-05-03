using System;
using System.Collections.Generic;
using System.Windows;
using Velvet_brows.Views;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;

namespace Velvet_brows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly String connectionString = "server=(LocalDB)\\MSSQLLocalDB; Initial Catalog=C:\\USERS\\ДНС\\BROWSVELVET.MDF;Integrated Security=SSPI";
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private DataSet ds;
        public MainWindow()
        {
            
            InitializeComponent();
            FillList();
            Count.IsEnabled = false;
            Count1.IsEnabled = false;
        }

        private void UpcomingEntries(object sender, RoutedEventArgs e)
        {
            UpcomingEntriesPage UEP = new UpcomingEntriesPage();
            UEP.Show();
            this.Close();
        }

        private void AdminLogin(object sender, RoutedEventArgs e)
        {
            AdminLoginPage ALP = new AdminLoginPage();
            ALP.Show();
            this.Close();
        }
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
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service", con);
                adapter = new SqlDataAdapter(cmd);
                string qqq = cmd.ExecuteScalar().ToString();
                Count.Text = qqq;
                ds = new DataSet();
                adapter.Fill(ds, "Service");

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
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT Count (*) FROM Service WHERE Title LIKE '" + intext.ToString() + "%' OR Cost LIKE '" + intext.ToString() + "%' ", con);
                adapter = new SqlDataAdapter(cmd);
                string qqq = cmd.ExecuteScalar().ToString();
                Count1.Text = qqq;
                ds = new DataSet();
                adapter.Fill(ds, "Service");

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

        private void Removal(object sender, RoutedEventArgs e)
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
        }

        private void Edit_Clik(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /// <summary>
            ///берем значение из комбобокса
            /// </summary>
            ComboBox cmBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)cmBox.SelectedItem;
            string intext = selectedItem.Content.ToString();


            try
            {
                if (intext == "до 5%")
                    intext = "0.05";
                else if (intext == "от 10% до 15%")
                    intext = "0.1";
                else if (intext == "от 20% до 25%")
                    intext = "0.2";
                else if (intext == "Любая скидка")
                    intext = "0";


                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service WHERE Discount LIKE '" + intext.ToString() + "%'", con);
                adapter = new SqlDataAdapter(cmd);

                ds = new DataSet();
                adapter.Fill(ds, "Service");
                Service co = new Service();
                IList<Service> co1 = new List<Service>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    co1.Add(new Service
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
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT Count (*) FROM Service WHERE Discount LIKE '" + intext.ToString() + "%'", con);
                adapter = new SqlDataAdapter(cmd);
                string qqq = cmd.ExecuteScalar().ToString();
                Count1.Text = qqq;
                ds = new DataSet();
                adapter.Fill(ds, "Service");
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                ds = null;
                adapter.Dispose();
                con.Close();
                con.Dispose();
            }
        }
    }
}

