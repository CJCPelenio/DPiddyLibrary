using System.Windows.Input;

namespace DPiddyLibrary.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        //Fields
        private BaseViewModel _currentChildView;

        public BaseViewModel CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {

                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        //Commands
        public ICommand ShowLibUserViewCommand { get; }
        public ICommand ShowRecordViewCommand { get; }
        public ICommand ShowRegViewCommand { get; }
        public ICommand ShowBorRetViewCommand { get; }
        public ICommand ShowBookViewCommand { get; }
        public ICommand ShowStaffViewCommand { get; }

        public MainViewModel()
        {
            //Init
            ShowLibUserViewCommand = new CommandViewModel(ExecuteLibUserViewCommand);
            ShowRecordViewCommand = new CommandViewModel(ExecuteShowRecordViewCommand);
            ShowRegViewCommand = new CommandViewModel(ExecuteShowRegViewCommand);
            ShowBorRetViewCommand = new CommandViewModel(ExecuteBorRetViewCommand);
            ShowBookViewCommand = new CommandViewModel(ExecuteShowBookViewModel);
            ShowStaffViewCommand = new CommandViewModel(ExecuteShowStaffViewCommand);

            //Default
            ExecuteLibUserViewCommand(null);
        }

        private void ExecuteLibUserViewCommand(object obj)
        {
            CurrentChildView = new LibUserViewModel();
        }

        private void ExecuteShowStaffViewCommand(object obj)
        {
            CurrentChildView = new StaffViewModel();
        }

        private void ExecuteShowBookViewModel(object obj)
        {
            CurrentChildView = new BookViewModel();
        }

        private void ExecuteBorRetViewCommand(object obj)
        {
            CurrentChildView = new BorRetViewModel();
        }

        private void ExecuteShowRegViewCommand(object obj)
        {
            CurrentChildView = new RegViewModel();
        }

        private void ExecuteShowRecordViewCommand(object obj)
        {
            CurrentChildView = new RecordViewModel();
        }
    }

}
