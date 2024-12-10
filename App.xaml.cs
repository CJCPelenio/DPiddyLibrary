using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DPiddyLibrary.View;
using DPiddyLibrary.ViewModel;

namespace DPiddyLibrary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            loginWindow.IsVisibleChanged += (s, ev) =>
            {
                if (loginWindow.IsVisible == false && loginWindow.IsLoaded)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    loginWindow.Close();
                }
            };  
        }
    }
}
