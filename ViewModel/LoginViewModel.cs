using System.Net;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DPiddyLibrary.Model;
using DPiddyLibrary.Repositories;

namespace DPiddyLibrary.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private ILoginRepo loginRepo;

        //Properties
        public string Username 
            { get => _username; set { _username = value; OnPropertyChanged(nameof(Username)); } }
        public SecureString Password 
            { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        public string ErrorMessage 
            { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }
        public bool IsViewVisible 
            { get => _isViewVisible; set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); } }

        //-> Commands
        public ICommand LoginCommand { get; }

        //Constructor
        public LoginViewModel()
        {
            loginRepo = new LoginRepo();
            LoginCommand = new CommandViewModel(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = loginRepo.AuthenticateUser(new NetworkCredential(Username, Password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else 
            {
                ErrorMessage = "Invalid Username or Password!";
                Task.Delay(2500);
                ErrorMessage = null;
            }
        }
    }
}
