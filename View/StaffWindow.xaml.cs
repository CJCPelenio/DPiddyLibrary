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
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : UserControl
    {
        public StaffWindow()
        {
            InitializeComponent();
            GetStaff();
        }

        private readonly SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=DPiddyLibrary;Integrated Security=True;TrustServerCertificate=True");

        private void GetStaff()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("EXEC ReadStaff", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            StaffDataGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool validAdd;
            if (txt_StaffID.Text.Length == 0 &&
                txt_FirstName.Text.Length > 0 &&
                txt_LastName.Text.Length > 0 &&
                txt_ContactNum.Text.Length > 0 &&
                txt_Email.Text.Length > 0) validAdd = true;
            else validAdd = false;

            if (validAdd == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC InsertStaff @FirstName, @LastName, @ContactNum, @Email", con);
                cmd.Parameters.AddWithValue("@FirstName", txt_FirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txt_LastName.Text);
                cmd.Parameters.AddWithValue("@ContactNum", txt_ContactNum.Text);
                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                GetStaff();
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
            if (txt_StaffID.Text.Length > 0 &&
                txt_FirstName.Text.Length > 0 &&
                txt_LastName.Text.Length > 0 &&
                txt_ContactNum.Text.Length > 0 &&
                txt_Email.Text.Length > 0) validEdit = true;
            else validEdit = false;

            if (validEdit == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC UpdateStaff @StaffID, @FirstName, @LastName, @ContactNum, @Email", con);
                cmd.Parameters.AddWithValue("@StaffID", int.Parse(txt_StaffID.Text));
                cmd.Parameters.AddWithValue("@FirstName", txt_FirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txt_LastName.Text);
                cmd.Parameters.AddWithValue("@ContactNum", txt_ContactNum.Text);
                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                GetStaff();
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
            if (txt_StaffID.Text.Length > 0) validDelete = true;
            else validDelete = false;

            if (validDelete == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC DeleteStaff @StaffID", con);
                cmd.Parameters.AddWithValue("@StaffID", int.Parse(txt_StaffID.Text));
                cmd.ExecuteNonQuery();
                con.Close();

                GetStaff();
            }
            else
            {
                Cursor = Cursors.No;
                WaitSetCursor();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txt_StaffID.Clear();
            txt_FirstName.Clear();
            txt_LastName.Clear();
            txt_ContactNum.Clear();
            txt_Email.Clear();
            StaffDataGrid.UnselectAll();
        }

        private async void WaitSetCursor()
        {
            Task WaitTask;
            await (WaitTask = Task.Delay(500));
            Cursor = null;
        }
    }
}
