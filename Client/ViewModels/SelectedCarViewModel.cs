using Client.BussinesModels;
using Client.Commands;
using Database.Enums;
using System;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class SelectedCarViewModel : BaseViewModel
    {
        #region Declarations

        public event Action<CarPeriodWrapper> SelectedEvent;
        public event Action<CarPeriodWrapper, CarType> ReturnToSelectEvent;

        private CarPeriodWrapper carPeriodWrapper;
        private Hour hourFrom;
        private Hour hourTo;

        #endregion

        #region Constructor

        public SelectedCarViewModel(CarPeriodWrapper carPeriodWrapper)
        {
            CarPeriodWrapper = carPeriodWrapper;
            HourFrom = new Hour(CarPeriodWrapper.Period.StartPeriod.Hour.ToString() + ":00");
            HourTo = new Hour(CarPeriodWrapper.Period.EndPeriod.Hour.ToString() + ":00");

            ChangeViewCommand = new RelayCommand(ChangeView, (e) => { return true; });
            ReturnSelectCommand = new RelayCommand(ReturnSelect, (e) => { return true; });
        }

        #endregion

        #region Properties

        #region Commands
        public ICommand ChangeViewCommand { get; set; }

        public ICommand ReturnSelectCommand { get; set; }

        #endregion

        public CarPeriodWrapper CarPeriodWrapper
        {
            get
            {
                return carPeriodWrapper;
            }
            set
            {
                carPeriodWrapper = value;
                NotifyPropertyChanged(nameof(CarPeriodWrapper));
            }
        }

        public Hour HourFrom
        {
            get
            {
                return hourFrom;
            }
            set
            {
                hourFrom = value;
                NotifyPropertyChanged(nameof(HourFrom));
            }
        }

        public Hour HourTo
        {
            get
            {
                return hourTo;
            }
            set
            {
                hourTo = value;
                NotifyPropertyChanged(nameof(HourTo));
            }
        }

        #endregion

        #region Methods

        public void OnSelectEvent()
        {
            SelectedEvent?.Invoke(CarPeriodWrapper);
        }

        public void OnReturnToSelectEvent()
        {
            ReturnToSelectEvent?.Invoke(carPeriodWrapper,carPeriodWrapper.Car.CarType);
        }

        private void ChangeView(object e)
        {
            OnSelectEvent();
        }

        private void ReturnSelect(object e)
        {
            OnReturnToSelectEvent();
        }

        #endregion

    }
}
