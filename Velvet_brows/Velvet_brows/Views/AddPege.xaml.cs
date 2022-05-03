using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Velvet_brows.Views
{
    /// <summary>
    /// Логика взаимодействия для AddPege.xaml
    /// </summary>
    public partial class AddPege : Window
    {
        static readonly String connectionString = "server=(LocalDB)\\MSSQLLocalDB; Initial Catalog=C:\\USERS\\ДНС\\BROWSVELVET.MDF;Integrated Security=SSPI";
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private DataSet ds;
        public AddPege()
        {
            InitializeComponent();
        }
        //Запрос на добавление в базу данных
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Countries cr = new Countries();
                cr.Title = Convert.ToString(TitleText.Text.ToString());
                cr.Cost = Convert.ToDecimal(CostTetxt.Text.ToString());
                cr.DurationInSeconds = Convert.ToInt32(DurText.Text.ToString());
                cr.Description = Convert.ToString(DescText.Text.ToString());
                cr.Discount = Convert.ToDouble(DiscountText.Text.ToString());
                cr.MainImagePath = Convert.ToString(ImgText.Text.ToString());
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("INSERT INTO Service (Title, Cost, DurationInSeconds, Description, Discount, MainImagePath) VALUES ('" + cr.Title + "', " + cr.Cost + " , " + cr.DurationInSeconds + ", '" + cr.Description + "', '" + "0." + cr.Discount + "', '" + cr.MainImagePath + "')", con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Service");
                ds = null;
                adapter.Dispose();
                con.Close();
                con.Dispose();
                System.Windows.MessageBox.Show("Данные успешно добавленны");
            }
            catch
            {
                System.Windows.MessageBox.Show("Проверьте правильность ввода данных");
            }
        }
        //Класс для обращение к базе и присваивание имен полям
        public class Countries
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public decimal Cost { get; set; }
            public int DurationInSeconds { get; set; }
            public string Description { get; set; }
            public double Discount { get; set; }
            public string MainImagePath { get; set; }
        }
        //Переход на другую страницу
        private void Button_ClickNazad(object sender, RoutedEventArgs e)
        {
            AdminPanelPage zap = new AdminPanelPage();
            zap.Show();
            Close();
        }

        private void TitleText_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (int.TryParse(e.Text, out int i))
            {
                e.Handled = true;
            }
        }
        private void CostTetxt_PreviewTextInput(object sender, TextCompositionEventArgs e){}
        private void DescText_PreviewTextInput(object sender, TextCompositionEventArgs e){}
        private void DurTetxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
        private string imgName;
        private void add_img(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*jpg|All Files (*.*)|*.*";
            ofd.ShowDialog();
            string imgPath = ofd.FileName;
            string[] splitter = imgPath.Split('\\');
            imgName = @"C:\Users\Mvideo\source\repos\BarhatniyeBrovki\Images\" + splitter[splitter.Length - 1];
            var di = new DirectoryInfo(@"C:\Users\Mvideo\source\repos\BarhatniyeBrovki\Images\");
            File.Copy(imgPath, imgName, true);
            ImgText.Text = imgPath;
            MessageBox.Show("ok!");
        }
    }
}
