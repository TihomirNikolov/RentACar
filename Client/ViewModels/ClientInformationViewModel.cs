using Client.BussinesModels;
using Client.Commands;
using Client.Helpers;
using Database.Enums;
using Database.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class ClientInformationViewModel : BaseViewModel
    {
        #region Declarations

        public event Action<CarPeriodWrapper, CarType> ReturnToSelectEvent;
        public event Action ChangeViewEvent;

        private const string emailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private const string phoneRegex = @"^((08)+[0-9]{8})$|^((\+359)+([0-9]){9})$";

        private CarPeriodWrapper carPeriodWrapper;
        private DateTime birthDate;
        private User user;
        private string name;
        private string lastName;
        private string phone;
        private string email;
        private bool isEmailValid;
        private bool isNameValid;
        private bool isLastNameValid;
        private bool isPhoneValid;
        private bool isBirthDateValid;

        private ReservationHelper reservationHelper;

        #endregion

        #region Constructor

        public ClientInformationViewModel(CarPeriodWrapper car)
        {
            reservationHelper = new ReservationHelper();
            User = new User();
            CarPeriodWrapper = car;
            BirthDate = DateTime.Now;
            ReserveCarCommand = new RelayCommand(ReserveCar, (e) => { return true; });
            ReturnSelectCommand = new RelayCommand(ReturnSelect, (e) => { return true; });
        }

        #endregion

        #region Properties

        #region Commands

        public ICommand ReserveCarCommand { get; set; }

        public ICommand ReturnSelectCommand { get; set; }

        #endregion

        public User User
        {
            get => user;
            set
            {
                user = value;
                NotifyPropertyChanged(nameof(User));
            }
        }
        public CarPeriodWrapper CarPeriodWrapper
        {
            get => carPeriodWrapper;
            set
            {
                carPeriodWrapper = value;
                NotifyPropertyChanged(nameof(CarPeriodWrapper));
            }
        }

        public DateTime BirthDate
        {
            get => birthDate;
            set
            {
                birthDate = value;
                if (!(birthDate.AddYears(18) < DateTime.Now))
                {
                    IsBirthDateValid = false;
                }
                else
                {
                    IsBirthDateValid = true;
                }
                NotifyPropertyChanged(nameof(BirthDate));
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                if (name.Length < 2 && name != null)
                {
                    IsNameValid = false;
                }
                else
                {
                    IsNameValid = true;
                }
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                if (lastName.Length < 2 && lastName != null)
                {
                    IsLastNameValid = false;
                }
                else
                {
                    IsLastNameValid = true;
                }
                NotifyPropertyChanged(nameof(LastName));
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                if (!Regex.IsMatch(email, emailRegex))
                {
                    IsEmailValid = false;
                }
                else
                {
                    IsEmailValid = true;
                }
                NotifyPropertyChanged(nameof(Email));
            }
        }

        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                if (!Regex.IsMatch(phone, phoneRegex) && phone != null)
                {
                    IsPhoneValid = false;
                }
                else
                {
                    IsPhoneValid = true;
                }
                NotifyPropertyChanged(nameof(Phone));
            }
        }

        public bool IsEmailValid
        {
            get => isEmailValid;
            set
            {
                isEmailValid = value;
                NotifyPropertyChanged(nameof(IsEmailValid));
            }
        }

        public bool IsNameValid
        {
            get => isNameValid;
            set
            {
                isNameValid = value;
                NotifyPropertyChanged(nameof(IsNameValid));
            }
        }

        public bool IsLastNameValid
        {
            get => isLastNameValid;
            set
            {
                isLastNameValid = value;
                NotifyPropertyChanged(nameof(IsLastNameValid));
            }
        }

        public bool IsPhoneValid
        {
            get => isPhoneValid;
            set
            {
                isPhoneValid = value;
                NotifyPropertyChanged(nameof(IsPhoneValid));
            }
        }

        public bool IsBirthDateValid
        {
            get => isBirthDateValid;
            set
            {
                isBirthDateValid = value;
                NotifyPropertyChanged(nameof(IsBirthDateValid));
            }
        }

        #endregion

        #region Methods

        private void OnReturnToSelectEvent()
        {
            ReturnToSelectEvent?.Invoke(carPeriodWrapper, carPeriodWrapper.Car.CarType);
        }

        private void OnChangeViewEvent()
        {
            ChangeViewEvent?.Invoke();
        }

        private void ReserveCar(object e)
        {
            if (Validate())
            {
                User.Email = Email;
                User.FirstName = Name;
                User.LastName = LastName;
                User.Phone = Phone;

                reservationHelper.Save(User, CarPeriodWrapper);

                OnChangeViewEvent();
            }
        }

        private void ReturnSelect(object e)
        {
            OnReturnToSelectEvent();
        }

        private bool Validate()
        {
            return IsEmailValid && IsNameValid && IsLastNameValid && IsPhoneValid && IsBirthDateValid;
        }

        #endregion
    }
}
