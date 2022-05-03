using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Win32;
using System.IO;

namespace Velvet_brows.Views
{
    /// <summary>
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Window
    {
        static String connectionString = "server=(LocalDB)\\MSSQLLocalDB; Initial Catalog=C:\\USERS\\ДНС\\BROWSVELVET.MDF;Integrated Security=SSPI";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        Service srv;
        /// <summary>
        ///конструктор который берет id услуги
        /// </summary>
        public int id { get; set; }

        public AddPage(int id = 0)
        {
            InitializeComponent();
            this.id = id;
            FillList();
            try
            {
                /// <summary>
                ///вывод услуги в текстовые поля для редактирования из базы
                /// </summary>
                TitleText.Text = srv.Title;
                CostTetxt.Text = Convert.ToString(srv.Cost);
                DurText.Text = Convert.ToString(srv.DurationInSeconds);
                DesciptionTetxt.Text = srv.Description;
                DiscountTetxt.Text = Convert.ToString(srv.Discount);
                ImgText.Text = srv.MainImagePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void FillList()
        {
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM Service WHERE ID = " + id + "", con);
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
                    srv = new Service
                    {
                        ID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString(),
                        Cost = Convert.ToDecimal(dr[2].ToString()),
                        DurationInSeconds = Convert.ToInt32(dr[3].ToString()),
                        Description = dr[4].ToString(),
                        Discount = Convert.ToDouble(dr[5].ToString()),
                        MainImagePath = Convert.ToString(dr[6].ToString())
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        ///Класс для обращение к базе и присваивание имен полям
        /// </summary>
        public class Service
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
        /// Запрос на редактирование товара в базе
        /// </summary>
        private void Button_ClickUp(object sender, RoutedEventArgs e)
        {
            Service cr = new Service();
            cr.Title = Convert.ToString(TitleText.Text.ToString());
            cr.Cost = Convert.ToDecimal(CostTetxt.Text.ToString());
            cr.DurationInSeconds = Convert.ToInt32(DurText.Text.ToString());
            cr.MainImagePath = Convert.ToString(ImgText.Text.ToString());
            con = new SqlConnection(connectionString);
            con.Open();

            cmd = new SqlCommand("UPDATE Service SET Title = '" + cr.Title + "', Cost = '" + cr.Cost + "', DurationInSeconds = " + cr.DurationInSeconds + ", MainImagePath = '" + cr.MainImagePath + "' WHERE ID= " + id + " ", con);
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "Service");
            ds = null;
            adapter.Dispose();
            con.Close();
            con.Dispose();


            MessageBox.Show("Услуга успешно обновлена!", "Уведомление");
        }
        private void Button_ClickNazad(object sender, RoutedEventArgs e)
        {
            AdminPanelPage APP = new AdminPanelPage();
            APP.Show();
            this.Close();
        }
        private string imgName;
        /// <summary>
        /// Добавление нового изображения
        /// </summary>
        private void openImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*jpg|All Files (*.*)|*.*";
            ofd.ShowDialog();
            string imgPath = ofd.FileName;

            string[] splitter = imgPath.Split('\\');

            imgName = @"C:\Users\ДНС\OneDrive\Рабочий стол\test\" + splitter[splitter.Length - 1];

            var di = new DirectoryInfo(@"C:\Users\ДНС\OneDrive\Рабочий стол\test\");


            System.IO.File.Copy(imgPath, imgName, true);

            MessageBox.Show("ok!");
        }
    }
}
