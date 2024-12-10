using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPiddyLibrary.View
{
    /// <summary>
    /// Interaction logic for LibUserWindow.xaml
    /// </summary>
    public partial class LibUserWindow : UserControl
    {
        public LibUserWindow()
        {
            InitializeComponent();
            GetLibUser();
        }

        private readonly SqlConnection con = new SqlConnection("Data Source=127.0.0.1;Initial Catalog=DPiddyLibrary;Persist Security Info=True;User ID=sqlserver;Password=dpiddylibrary;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True");

        private void GetLibUser()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("EXEC ReadLibUser", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            LibUserDataGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            bool validSearch;
            if (combo_SearchType.SelectedItem == null &&
                txt_SearchText.Text.Length == 0) validSearch = false;
            else validSearch = true;

            if (validSearch == true)
            {
                LibUserDataGrid.ItemsSource = null;

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("EXEC SearchLibUser @SearchType, @SearchValue", con);
                    cmd.Parameters.AddWithValue("@SearchType", combo_SearchType.SelectionBoxItem);
                    cmd.Parameters.AddWithValue("@SearchValue", txt_SearchText.Text);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);
                    LibUserDataGrid.ItemsSource = dt.DefaultView;
                }
                catch (Exception)
                {
                    MessageBox.Show("No Results Found");
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                Cursor = Cursors.No;
                WaitSetCursor();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            combo_SearchType.SelectedItem = null;
            txt_SearchText.Clear();
            LibUserDataGrid.UnselectAll();

            GetLibUser();
        }

        private void btnExtend_Click(object sender, RoutedEventArgs e)
        {
            bool validExtend;
            if (txt_UserID.Text.Length > 0) validExtend = true;
            else validExtend = false;

            if (validExtend == true)
            {
                int userID;
                int.TryParse(txt_UserID.Text, out userID);

                DateTime expiryDate = GetExpiryDate(userID);
                DateTime today = DateTime.Today;
                DateTime newExpiryDate;

                if (expiryDate > today)
                {
                    newExpiryDate = expiryDate.AddYears(1);
                }
                else
                {
                    newExpiryDate = today.AddYears(1);
                }

                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE LibraryUser SET DateOfExpiry = @NewExpiryDate WHERE UserID = @UserID", con);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@NewExpiryDate", newExpiryDate);
                cmd.ExecuteNonQuery();
                con.Close();

                GetLibUser();
            }
            else
            {
                Cursor = Cursors.No;
                WaitSetCursor();
            }
        }

        private DateTime GetExpiryDate(int userID)
        {
            DateTime expiryDate = DateTime.MinValue;

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT DateOfExpiry FROM LibraryUser WHERE UserID = @UserID", con);
            cmd.Parameters.AddWithValue("@UserID", userID);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            expiryDate = reader.GetDateTime(0);
            con.Close();

            return expiryDate;
        }

        private async void WaitSetCursor()
        {
            Task WaitTask;
            await (WaitTask = Task.Delay(500));
            Cursor = null;
        }
    }
}
