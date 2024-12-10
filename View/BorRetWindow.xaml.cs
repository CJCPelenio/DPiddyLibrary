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
    /// Interaction logic for BorRetWindow.xaml
    /// </summary>
    public partial class BorRetWindow : UserControl
    {
        public BorRetWindow()
        {
            InitializeComponent();
            GetBorRet();
        }

        private readonly SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=DPiddyLibrary;Integrated Security=True;TrustServerCertificate=True");
        private void GetBorRet()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("EXEC ReadBorRet", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            BorRetDataGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentDate = DateTime.Now;

            bool validAdd;
            if (txt_RefNum.Text.Length == 0 &&
                txt_UserID.Text.Length > 0 &&
                txt_BookCode.Text.Length > 0 &&
                txt_StaffID.Text.Length > 0 &&
                combo_ReturnStatus.Text.Length > 0) validAdd = true;
            else validAdd = false;

            if (validAdd == true)
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("EXEC InsertBorRet @UserID, @BookCode, @StaffID, @ReturnStatus, @DateOfBorrow", con);
                    cmd.Parameters.AddWithValue("@UserID", int.Parse(txt_UserID.Text));
                    cmd.Parameters.AddWithValue("@BookCode", int.Parse(txt_BookCode.Text));
                    cmd.Parameters.AddWithValue("@StaffID", int.Parse(txt_StaffID.Text));
                    cmd.Parameters.AddWithValue("@ReturnStatus", combo_ReturnStatus.SelectionBoxItem);
                    cmd.Parameters.AddWithValue("@DateOfBorrow", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                    con.Close();

                    GetBorRet();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547) // Foreign key error
                    {
                        System.Windows.MessageBox.Show("Sorry, the entered UserID/BookCode/StaffID doesn't exist in the database. Please make sure to double check for any mistypes in your entries.",
                                                       "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        con.Close();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("An error occurred while adding the record: " + ex.Message,
                                                       "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        con.Close();
                    }
                }
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
            if (txt_RefNum.Text.Length > 0 &&
                txt_UserID.Text.Length > 0 &&
                txt_BookCode.Text.Length > 0 &&
                txt_StaffID.Text.Length > 0 &&
                combo_ReturnStatus.Text.Length > 0) validEdit = true;
            else validEdit = false;

            if (validEdit == true)
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("EXEC UpdateBorRet @ReferenceNum, @UserID, @BookCode, @StaffID, @ReturnStatus, @DateOfReturn", con);
                    cmd.Parameters.AddWithValue("@ReferenceNum", int.Parse(txt_RefNum.Text));
                    cmd.Parameters.AddWithValue("@UserID", int.Parse(txt_UserID.Text));
                    cmd.Parameters.AddWithValue("@BookCode", int.Parse(txt_BookCode.Text));
                    cmd.Parameters.AddWithValue("@StaffID", int.Parse(txt_StaffID.Text));
                    cmd.Parameters.AddWithValue("@ReturnStatus", combo_ReturnStatus.SelectionBoxItem);

                    if (combo_ReturnStatus.SelectionBoxItem.ToString() == "RETURNED")
                    {
                        cmd.Parameters.AddWithValue("@DateOfReturn", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DateOfReturn", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                    con.Close();

                    GetBorRet();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547) // Foreign key error
                    {
                        System.Windows.MessageBox.Show("Sorry, the entered UserID/BookCode/StaffID doesn't exist in the database. Please make sure to double check for any mistypes in your entries.",
                                                       "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        con.Close();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("An error occurred while adding the record: " + ex.Message,
                                                       "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        con.Close();
                    }
                }
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
            if (txt_RefNum.Text.Length > 0) validDelete = true;
            else validDelete = false;

            if (validDelete == true)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("EXEC DeleteBorRet @ReferenceNum", con);
                cmd.Parameters.AddWithValue("@ReferenceNum", int.Parse(txt_RefNum.Text));
                cmd.ExecuteNonQuery();
                con.Close();

                GetBorRet();
            }
            else
            {
                Cursor = Cursors.No;
                WaitSetCursor();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txt_RefNum.Clear();
            txt_UserID.Clear();
            txt_BookCode.Clear();
            txt_StaffID.Clear();
            combo_ReturnStatus.SelectedItem = null;
            BorRetDataGrid.UnselectAll();
        }

        private async void WaitSetCursor()
        {
            Task WaitTask;
            await (WaitTask = Task.Delay(500));
            Cursor = null;
        }
    }
}
