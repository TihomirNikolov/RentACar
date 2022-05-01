using Client.BussinesModels;
using Client.Commands;
using Client.Settings;
using Database;
using MicroinvestProject.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Declarations

        public event Action ReturnToHomeEvent;
        private List<Language> languages;
        private Language selectedLanguage;
        private BaseViewModel selectedViewModel;
        private UpdateView updateView;

        #endregion

        #region Constructor

        public MainViewModel()
        {
            Constants.ConnectionString = string.Format(@"Server={0}; User = {1}; Password = {2}; Database={3};Trusted_Connection=True;",
                                                         SettingsManager.DbServer, SettingsManager.DbUsername, SettingsManager.DbPassword, SettingsManager.DbName);
            Languages = new List<Language>();
            Languages.Add(new Language("English", @"/Images/EN.png"));
            Languages.Add(new Language("Български", @"/Images/BG.png"));
            if (!string.IsNullOrEmpty(SettingsManager.Language))
            {
                SelectedLanguage = Languages.Where(l => l.Lang.ToString() == SettingsManager.Language).FirstOrDefault();
            }
            else
            {
                SelectedLanguage = Languages[0];
            }

           
            updateView = new UpdateView(this);
            ReturnToHomeCommand = new RelayCommand(ReturnToHome, (e) => { return true; });
        }

        #endregion

        #region Properties

        #region Commands


        public ICommand ReturnToHomeCommand { get; set; }

        public ICommand GoToAdminLoginCommand { get; set; }

        #endregion

        public List<Language> Languages
        {
            get
            {
                return languages;
            }
            set
            {
                languages = value;
                NotifyPropertyChanged(nameof(Languages));
            }
        }

        public Language SelectedLanguage
        {
            get
            {
                return selectedLanguage;
            }
            set
            {
                selectedLanguage = value;
                FieldInfo fieldInfo = SelectedLanguage.Lang.GetType().GetField(SelectedLanguage.Lang.ToString());

                DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(descriptionAttributes[0].Description);

                SettingsManager.Language = value.Lang.ToString();

                NotifyPropertyChanged(nameof(SelectedLanguage));
            }
        }

        public BaseViewModel SelectedViewModel
        {
            get
            {
                return selectedViewModel;
            }
            set
            {
                selectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public string LogoImagePath
        {
            get
            {
                return @"/Images/Logo.png";
            }
        }

        #endregion

        #region Methods

        private void OnReturnToHomeEvent()
        {
            ReturnToHomeEvent?.Invoke();
        }

        private void ReturnToHome(object e)
        {
            OnReturnToHomeEvent();
        }

        #endregion
    }
}
