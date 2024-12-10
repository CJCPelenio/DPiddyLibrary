using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for RegWindow.xaml
    /// </summary>
    public partial class RegWindow : UserControl
    {
        public RegWindow()
        {
            InitializeComponent();
            GetReg();
        }

        private readonly SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=DPiddyLibrary;Integrated Security=True;TrustServerCertificate=True");

        private void GetReg()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("EXEC ReadReg", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            RegDataGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            DateTime expiryDate = currentDate.AddYears(1);
            string formattedExpiryDate = expiryDate.ToString("yyyy-MM-dd");

            bool validAdd;
            if (txt_Index.Text.Length == 0 &&
                txt_FirstName.Text.Length > 0 &&
                txt_LastName.Text.Length > 0 &&
                txt_IDNum.Text.Length > 0 &&
                combo_IDType.Text.Length > 0 &&
                txt_ContactNum.Text.Length > 0 &&
                txt_Email.Text.Length > 0) validAdd = true;
            else validAdd = false;

            if (validAdd == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC InsertReg @FirstName, @LastName, @IDNum, @IDType, @ContactNum, @DateOfReg, @Email, @DateOfExpiry", con);
                cmd.Parameters.AddWithValue("@FirstName", txt_FirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txt_LastName.Text);
                cmd.Parameters.AddWithValue("@IDNum", txt_IDNum.Text);
                cmd.Parameters.AddWithValue("@IDType", combo_IDType.SelectionBoxItem);
                cmd.Parameters.AddWithValue("@ContactNum", txt_ContactNum.Text);
                cmd.Parameters.AddWithValue("@DateOfReg", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                cmd.Parameters.AddWithValue("@DateOfExpiry", formattedExpiryDate);
                cmd.ExecuteNonQuery();
                con.Close();

                GetReg();
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
            if (txt_Index.Text.Length > 0 &&
                txt_FirstName.Text.Length > 0 &&
                txt_LastName.Text.Length > 0 &&
                txt_IDNum.Text.Length > 0 &&
                combo_IDType.Text.Length > 0 &&
                txt_ContactNum.Text.Length > 0 &&
                txt_Email.Text.Length > 0) validEdit = true;
            else validEdit = false;

            if (validEdit == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC UpdateReg @RegistrationUK, @FirstName, @LastName, @IDNum, @IDType, @ContactNum, @Email", con);
                cmd.Parameters.AddWithValue("@RegistrationUK", int.Parse(txt_Index.Text));
                cmd.Parameters.AddWithValue("@FirstName", txt_FirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txt_LastName.Text);
                cmd.Parameters.AddWithValue("@IDNum", txt_IDNum.Text);
                cmd.Parameters.AddWithValue("@IDType", combo_IDType.SelectionBoxItem);
                cmd.Parameters.AddWithValue("@ContactNum", txt_ContactNum.Text);
                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                GetReg();
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
            if (txt_Index.Text.Length > 0) validDelete = true;
            else validDelete = false;

            if (validDelete == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC DeleteReg @RegistrationUK", con);
                cmd.Parameters.AddWithValue("@RegistrationUK", int.Parse(txt_Index.Text));
                cmd.ExecuteNonQuery();
                con.Close();

                GetReg();
            }
            else
            {
                Cursor = Cursors.No;
                WaitSetCursor();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txt_Index.Clear();
            txt_FirstName.Clear();
            txt_LastName.Clear();
            txt_IDNum.Clear();
            combo_IDType.SelectedItem = null;
            txt_ContactNum.Clear();
            txt_Email.Clear();
            RegDataGrid.UnselectAll();
        }

        private async void WaitSetCursor()
        {
            Task WaitTask;
            await (WaitTask = Task.Delay(500));
            Cursor = null;
        }
    }

}
