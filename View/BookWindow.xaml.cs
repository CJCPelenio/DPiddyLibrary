using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DPiddyLibrary.View
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : UserControl
    {
        public BookWindow()
        {
            InitializeComponent();
            GetBooks();
        }

        private readonly SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=DPiddyLibrary;Integrated Security=True;TrustServerCertificate=True");

        private void GetBooks()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("EXEC ReadBooks", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            BookDataGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool validAdd;
            if (txt_BookCode.Text.Length == 0 &&
                txt_Title.Text.Length > 0 &&
                txt_ISBN.Text.Length > 0 &&
                txt_Author.Text.Length > 0 &&
                txt_Publisher.Text.Length > 0 &&
                txt_YearPublished.Text.Length > 0) validAdd = true;
            else validAdd = false;

            if (validAdd == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC InsertBooks @Title, @ISBN, @Author, @Publisher, @YearPublished", con);
                cmd.Parameters.AddWithValue("@Title", txt_Title.Text);
                cmd.Parameters.AddWithValue("@ISBN", txt_ISBN.Text);
                cmd.Parameters.AddWithValue("@Author", txt_Author.Text);
                cmd.Parameters.AddWithValue("@Publisher", txt_Publisher.Text);
                cmd.Parameters.AddWithValue("@YearPublished", int.Parse(txt_YearPublished.Text));
                cmd.ExecuteNonQuery();
                con.Close();

                GetBooks();
            }
            else
            {
                Cursor = Cursors.No;
                WaitSetCursor();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            bool validEdit;
            if (txt_BookCode.Text.Length > 0 &&
                txt_Title.Text.Length > 0 &&
                txt_ISBN.Text.Length > 0 &&
                txt_Author.Text.Length > 0 &&
                txt_Publisher.Text.Length > 0 &&
                txt_YearPublished.Text.Length > 0) validEdit = true;
            else validEdit = false;

            if (validEdit == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC UpdateBooks @BookCode, @Title, @ISBN, @Author, @Publisher, @YearPublished", con);
                cmd.Parameters.AddWithValue("@BookCode", int.Parse(txt_BookCode.Text));
                cmd.Parameters.AddWithValue("@Title", txt_Title.Text);
                cmd.Parameters.AddWithValue("@ISBN", txt_ISBN.Text);
                cmd.Parameters.AddWithValue("@Author", txt_Author.Text);
                cmd.Parameters.AddWithValue("@Publisher", txt_Publisher.Text);
                cmd.Parameters.AddWithValue("@YearPublished", int.Parse(txt_YearPublished.Text));
                cmd.ExecuteNonQuery();
                con.Close();

                GetBooks();
            }
            else
            {
                Cursor = Cursors.No;
                WaitSetCursor();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            bool validDelete;
            if (txt_BookCode.Text.Length > 0) validDelete = true;
            else validDelete = false;

            if (validDelete == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC DeleteBooks @BookCode", con);
                cmd.Parameters.AddWithValue("@BookCode", int.Parse(txt_BookCode.Text));
                cmd.ExecuteNonQuery();
                con.Close();

                GetBooks();
            }
            else
            {
                Cursor = Cursors.No;
                WaitSetCursor();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txt_BookCode.Clear();
            txt_Title.Clear();
            txt_ISBN.Clear();
            txt_Author.Clear();
            txt_Publisher.Clear();
            txt_YearPublished.Clear();
            BookDataGrid.UnselectAll();
        }

        private async void WaitSetCursor()
        {
            Task WaitTask;
            await (WaitTask = Task.Delay(500));
            Cursor = null;
        }
    }
}
