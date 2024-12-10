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
    /// Interaction logic for RecordWindow.xaml
    /// </summary>
    public partial class RecordWindow : UserControl
    {
        public RecordWindow()
        {
            InitializeComponent();
            GetRecords();
        }

        private readonly SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=DPiddyLibrary;Integrated Security=True;TrustServerCertificate=True");

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
