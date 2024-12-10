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
    /// Interaction logic for RecordWindow.xaml
    /// </summary>
    public partial class RecordWindow : UserControl
    {
        public RecordWindow()
        {
            InitializeComponent();
            GetRecords();
        }

        private readonly SqlConnection con = new SqlConnection("Data Source=127.0.0.1;Initial Catalog=DPiddyLibrary;Persist Security Info=True;User ID=sqlserver;Password=dpiddylibrary;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True");

        private void GetRecords()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("EXEC ReadRecords", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            RecordsDataGrid.ItemsSource = dt.DefaultView;
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
                RecordsDataGrid.ItemsSource = null;

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("EXEC SearchRecords @SearchType, @SearchValue", con);
                    cmd.Parameters.AddWithValue("@SearchType", combo_SearchType.SelectionBoxItem);
                    cmd.Parameters.AddWithValue("@SearchValue", txt_SearchText.Text);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);
                    RecordsDataGrid.ItemsSource = dt.DefaultView;
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
            RecordsDataGrid.UnselectAll();

            GetRecords();
        }
        private async void WaitSetCursor()
        {
            Task WaitTask;
            await (WaitTask = Task.Delay(500));
            Cursor = null;
        }
    }
}
