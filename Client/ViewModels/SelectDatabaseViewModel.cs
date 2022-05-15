using Client.BussinesModels;
using Client.Commands;
using Client.Helpers;
using Client.Settings;
using Database.Enums;
using MicroinvestProject.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class SelectDatabaseViewModel : BaseViewModel
    {
        #region Declarations

        private BaseViewModel selectedViewModel;

        private bool isBusy;

        private bool isCreatingNewDb;
        private string serverName;
        private string username;
        private string password;
        private string databaseName;
        private ObservableCollection<string> databases;

        private bool isPasswordVisible;

        private ICommand changeModeCommand;
        private ICommand createDatabaseCommand;
        private ICommand saveCommand;
        private ICommand loadDatabasesCommand;
        private ICommand showHidePasswordCommand;

        #endregion

        #region Constructors

        public SelectDatabaseViewModel()
        {
            SetLanguage();
            IsPasswordVisible = false;
            IsCreatingNewDb = false;
            Databases = new ObservableCollection<string>();
        }

        #endregion

        #region Properties

        #region Commands

        public ICommand ChangeModeCommand => changeModeCommand ?? (changeModeCommand = new RelayCommand((e) => { IsCreatingNewDb = !isCreatingNewDb; }, (e) => { return true; }));
        public ICommand CreateDatabaseCommand => createDatabaseCommand ?? (createDatabaseCommand = new RelayCommand(CreateDatabase, (e) => { return true; }));
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new RelayCommand(Save, (e) => { return true; }));
        public ICommand LoadDatabasesCommand => loadDatabasesCommand ?? (loadDatabasesCommand = new RelayCommand(LoadDatabases, (e) => { return true; }));
        public ICommand ShowHidePasswordCommand => showHidePasswordCommand ?? (showHidePasswordCommand = new RelayCommand(ShowHidePassword, (e) => { return true; }));

        #endregion

        public BaseViewModel SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public bool IsCreatingNewDb
        {
            get => isCreatingNewDb;

            set
            {
                isCreatingNewDb = value;
                NotifyPropertyChanged(nameof(IsCreatingNewDb));
            }
        }

        public string ServerName
        {
            get => serverName;

            set
            {
                if (serverName != value)
                {
                    serverName = value;
                    Databases.Clear();
                    DatabaseName = "";
                    NotifyPropertyChanged(nameof(ServerName));
                }
            }
        }

        public string Username
        {
            get => username;

            set
            {
                if (username != value)
                {
                    username = value;
                    Databases.Clear();
                    DatabaseName = "";
                    NotifyPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => password;

            set
            {
                if (password != value)
                {
                    password = value;
                    NotifyPropertyChanged(nameof(Password));
                }
            }
        }

        public string DatabaseName
        {
            get => databaseName;

            set
            {
                if (databaseName != value)
                {
                    databaseName = value;
                    NotifyPropertyChanged(nameof(DatabaseName));
                }
            }
        }

        public bool IsPasswordVisible
        {
            get => isPasswordVisible;

            set
            {
                if (isPasswordVisible != value)
                {
                    isPasswordVisible = value;
                    NotifyPropertyChanged(nameof(IsPasswordVisible));
                }
            }
        }

        public bool IsBusy
        {
            get => isBusy;

            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    NotifyPropertyChanged(nameof(IsBusy));
                }
            }
        }

        public ObservableCollection<string> Databases
        {
            get => databases;

            set
            {
                databases = value;
                NotifyPropertyChanged(nameof(Databases));
            }
        }

        #endregion

        #region Methods

        private void CreateDatabase(object parameter)
        {
            if (Validate())
            {
                Task.Run(async () =>
                {
                    IsBusy = true;
                    try
                    {
                        DatabaseModel database = new DatabaseModel()
                        {
                            DatabaseName = DatabaseName,
                            Server = ServerName,
                            Username = Username,
                            Password = Password
                        };

                        await DatabaseHelper.Instance.CreateDatabase(database);
                        SelectedViewModel = new MainViewModel();
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show(Resources.Resources.ResourceManager.GetString("strNoConnection", TranslationSource.Instance.CurrentCulture));
                    }
                    finally
                    {
                        IsBusy = false;
                    }

                });
            }
            else
            {
                MessageBox.Show(Resources.Resources.ResourceManager.GetString("strInvalidData", TranslationSource.Instance.CurrentCulture));
            }
        }

        private void Save(object parameter)
        {
            if (Validate())
            {
                IsBusy = true;
                Task.Run(() =>
                {
                    try
                    {
                        DatabaseModel database = new DatabaseModel()
                        {
                            DatabaseName = DatabaseName,
                            Server = ServerName,
                            Username = Username,
                            Password = Password
                        };

                        DatabaseHelper.Instance.ConnectToServer(database);

                        SelectedViewModel = new MainViewModel();
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show(Resources.Resources.ResourceManager.GetString("strNoConnection", TranslationSource.Instance.CurrentCulture));
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                });
            }
            else
            {
                MessageBox.Show(Resources.Resources.ResourceManager.GetString("strInvalidData", TranslationSource.Instance.CurrentCulture));
            }
        }

        private void LoadDatabases(object e)
        {
            if (!string.IsNullOrEmpty(ServerName) || !string.IsNullOrEmpty(DatabaseName))
            {
                if (Databases.Count == 0)
                {
                    Task.Run(() =>
                    {
                        DatabaseModel database = new DatabaseModel()
                        {
                            DatabaseName = DatabaseName,
                            Server = ServerName,
                            Username = Username,
                            Password = Password
                        };

                        Databases = new ObservableCollection<string>(DatabaseHelper.Instance.LoadDatabasesFromServer(database));
                    });
                }
            }
        }

        private void ShowHidePassword(object e)
        {
            IsPasswordVisible = !isPasswordVisible;
        }

        private bool Validate()
        {
            return !string.IsNullOrEmpty(ServerName) && !string.IsNullOrEmpty(DatabaseName);
        }


        private void SetLanguage()
        {
            if(string.IsNullOrEmpty(SettingsManager.Language))
            {
                SettingsManager.Language = "English";
            }

            var lang = Enum.Parse(typeof(LanguageEnum), SettingsManager.Language);

            FieldInfo fieldInfo = lang.GetType().GetField(lang.ToString());

            DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(descriptionAttributes[0].Description);
        }
        #endregion
    }
}
