using Client.Settings;

namespace Client.ViewModels
{
    public class StartupViewModel : BaseViewModel
    {
        #region Declarations

        private BaseViewModel selectedViewModel;

        #endregion

        #region Constructor

        public StartupViewModel()
        {
            SelectView();
        }

        #endregion

        #region Properties

        public BaseViewModel SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        #endregion

        #region Methods

        private void SelectView()
        {
            if (string.IsNullOrEmpty(SettingsManager.DbName))
            {
                SelectedViewModel = new SelectDatabaseViewModel();
            }
            else
            {
                SelectedViewModel = new MainViewModel();
            }
        }

        #endregion
    }
}
