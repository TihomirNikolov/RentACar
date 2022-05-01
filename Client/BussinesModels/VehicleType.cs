using Client.Commands;
using Client.Utilities;
using Database.Enums;
using MicroinvestProject.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Client.BussinesModels
{
    public class VehicleType : INotifyPropertyChanged
    {
        #region Declaration

        public event Action<string> SelectEvent;

        #endregion

        #region Fields

        private string strCarType;
        private bool isSelected;

        #endregion

        #region Properties

        public string StrCarType
        {
            get => strCarType;
            set
            {
                strCarType = value;
                NotifyPropertyChanged(nameof(StrCarType));
            }
        }

        public CarType CarType { get; set; }

        public string IconPath { get; set; }
        public string BlueIconPath { get; set; }
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                NotifyPropertyChanged(nameof(IsSelected));
            }
        }

        #endregion

        #region Constructor

        private VehicleType(CarType carType, string iconPath, string blueIconPath, bool isSelected)
        {
            CarType = carType;
            IconPath = iconPath;
            BlueIconPath = blueIconPath;
            IsSelected = isSelected;
            SelectButtonCommand = new RelayCommand(SelectButton, CanSelectButton);
            StrCarType = Resources.Resources.ResourceManager.GetString(EnumExtension.GetDescription(carType), TranslationSource.Instance.CurrentCulture);
            TranslationSource.Instance.LanguageChangeEvent += LanguageHandler;
        }

        #region Commands

        public ICommand SelectButtonCommand { get; set; }

        private bool CanSelectButton(object parameter)
        {
            return true;
        }

        private void SelectButton(object parameter)
        {
            OnSelectEvent();
        }

        #endregion

        #endregion

        #region Methods

        public void LanguageHandler()
        {
            StrCarType = Resources.Resources.ResourceManager.GetString(EnumExtension.GetDescription(CarType), TranslationSource.Instance.CurrentCulture);
        }

        public void OnSelectEvent()
        {
            SelectEvent?.Invoke(StrCarType);
        }

        public static List<VehicleType> LoadVehicleTypeList()
        {
            List<VehicleType> vehicleTypes = new List<VehicleType>();

            vehicleTypes.Add(new VehicleType(CarType.Car, @"/Images/Car_Icon.png", @"/Images/Car_Icon_Blue.png", true));
            vehicleTypes.Add(new VehicleType(CarType.Electic, @"/Images/ECar_Icon.png", @"/Images/ECar_Icon_Blue.png", false));
            vehicleTypes.Add(new VehicleType(CarType.Freight, @"/Images/Truck_Icon.png", @"/Images/Truck_Icon_Blue.png", false));

            return vehicleTypes;
        }

        #endregion

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
