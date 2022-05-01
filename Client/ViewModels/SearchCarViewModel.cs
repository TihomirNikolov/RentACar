using Client.BussinesModels;
using Client.Commands;
using Database.Enums;
using Database.Interfaces;
using Database.Models;
using Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class SearchCarViewModel : BaseViewModel
    {
        #region Declarations

        public event Action<CarPeriodWrapper, List<CarPeriodWrapper>, CarType> SearchEvent;

        private List<LocationWrapper> locations;
        private List<Hour> hoursList;
        private DateTime dateFrom;
        private DateTime dateTo;
        private CarPeriodWrapper carPeriodWrapper;
        private Hour selectedHourFrom;
        private Hour selectedHourTo;
        private LocationWrapper selectedLocationFrom;
        private LocationWrapper selectedLocationTo;
        private List<VehicleType> vehicleTypes;
        private CarType carType;
        private bool isFromDateValid;
        private bool isToDateValid;
        private bool isPeriodValid;
        public List<CarPeriodWrapper> cars;
        private IRepositoryManager repositoryManager;

        #endregion

        #region Constructor

        public SearchCarViewModel()
        {
            repositoryManager = new RepositoryManager();
            HoursList = Hour.LoadHoursList();
            Locations = LocationWrapper.LoadLocationList();
            VehicleTypes = VehicleType.LoadVehicleTypeList();
            VehicleTypes[0].SelectEvent += SelectionHandler;
            VehicleTypes[1].SelectEvent += SelectionHandler;
            VehicleTypes[2].SelectEvent += SelectionHandler;
            SetInitialInfo();
            ChangeViewCommand = new RelayCommand(ChangeView, (e) => { return true; });
        }

        #endregion

        #region Properties

        #region

        public RelayCommand ChangeViewCommand { get; set; }

        #endregion

        public List<LocationWrapper> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                locations = value;
                NotifyPropertyChanged(nameof(Location));
            }
        }

        public List<Hour> HoursList
        {
            get
            {
                return hoursList;
            }
            set
            {
                hoursList = value;
                NotifyPropertyChanged(nameof(HoursList));
            }
        }

        public DateTime DateFrom
        {
            get
            {
                return dateFrom;
            }
            set
            {
                dateFrom = value;
                if (SelectedHourFrom != null)
                {
                    FromHourToDate();
                }
                if (dateFrom < DateTime.Now)
                {
                    IsFromDateValid = false;
                }
                else
                {
                    IsFromDateValid = true;
                }
                if ((DateTo - DateFrom) <= TimeSpan.Zero || !IsToDateValid || !IsFromDateValid)
                {
                    IsPeriodValid = false;
                }
                else
                {
                    IsPeriodValid = true;
                }
                NotifyPropertyChanged(nameof(DateFrom));
            }
        }

        public DateTime DateTo
        {
            get
            {
                return dateTo;
            }
            set
            {
                dateTo = value;
                if (SelectedHourTo != null)
                {
                    ToHourToDate();
                }

                if (dateTo < DateTime.Now)
                {
                    IsToDateValid = false;
                }
                else
                {
                    IsToDateValid = true;
                }

                if ((DateTo - DateFrom) <= TimeSpan.Zero || !IsToDateValid || !IsFromDateValid)
                {
                    IsPeriodValid = false;
                }
                else
                {
                    IsPeriodValid = true;
                }
                NotifyPropertyChanged(nameof(DateTo));
            }
        }

        public CarPeriodWrapper CarPeriodWrapper
        {
            get
            {
                return carPeriodWrapper;
            }
            set
            {
                carPeriodWrapper = value;
            }
        }

        public Hour SelectedHourFrom
        {
            get
            {
                return selectedHourFrom;
            }
            set
            {
                selectedHourFrom = value;
                FromHourToDate();

                if (DateFrom < DateTime.Now)
                {
                    IsFromDateValid = false;
                }
                else
                {
                    IsFromDateValid = true;
                }

                if ((DateTo - DateFrom) <= TimeSpan.Zero || !IsToDateValid || !IsFromDateValid)
                {
                    IsPeriodValid = false;
                }
                else
                {
                    IsPeriodValid = true;
                }

                NotifyPropertyChanged(nameof(SelectedHourFrom));
            }
        }

        public Hour SelectedHourTo
        {
            get
            {
                return selectedHourTo;
            }
            set
            {
                selectedHourTo = value;
                ToHourToDate();

                if (DateTo < DateTime.Now)
                {
                    IsToDateValid = false;
                }
                else
                {
                    IsToDateValid = true;
                }
                if ((DateTo - DateFrom) <= TimeSpan.Zero || !IsToDateValid || !IsFromDateValid)
                {
                    IsPeriodValid = false;
                }
                else
                {
                    IsPeriodValid = true;
                }
                NotifyPropertyChanged(nameof(SelectedHourTo));
            }
        }

        public LocationWrapper SelectedLocationFrom
        {
            get
            {
                return selectedLocationFrom;
            }
            set
            {
                selectedLocationFrom = value;
                NotifyPropertyChanged(nameof(SelectedLocationFrom));
            }
        }

        public LocationWrapper SelectedLocationTo
        {
            get
            {
                return selectedLocationTo;
            }
            set
            {
                selectedLocationTo = value;
                NotifyPropertyChanged(nameof(SelectedLocationTo));
            }
        }

        public List<VehicleType> VehicleTypes
        {
            get => vehicleTypes;
            set
            {
                vehicleTypes = value;
                NotifyPropertyChanged(nameof(VehicleTypes));
            }
        }

        public bool IsFromDateValid
        {
            get => isFromDateValid;
            set
            {
                isFromDateValid = value;
                NotifyPropertyChanged(nameof(IsFromDateValid));
            }
        }

        public bool IsToDateValid
        {
            get => isToDateValid;
            set
            {
                isToDateValid = value;
                NotifyPropertyChanged(nameof(IsToDateValid));
            }
        }

        public bool IsPeriodValid
        {
            get => isPeriodValid;
            set
            {
                isPeriodValid = value;
                NotifyPropertyChanged(nameof(IsPeriodValid));
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        public void OnSearchHandler()
        {
            SearchEvent?.Invoke(CarPeriodWrapper, cars, carType);
        }

        public void SetInitialInfo()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            SelectedHourFrom = HoursList[10];
            SelectedHourTo = HoursList[10];
            SelectedLocationFrom = Locations[0];
            SelectedLocationTo = Locations[0];
            carType = CarType.Car;
        }

        public void SelectionHandler(string car)
        {
            if (car == "Car" || car == "Кола")
            {
                carType = CarType.Car;
                VehicleTypes[0].IsSelected = true;
                VehicleTypes[1].IsSelected = false;
                VehicleTypes[2].IsSelected = false;
            }
            else if (car == "Electrical car" || car == "Електромобил")
            {
                carType = CarType.Electic;
                VehicleTypes[0].IsSelected = false;
                VehicleTypes[1].IsSelected = true;
                VehicleTypes[2].IsSelected = false;
            }
            else if (car == "Freight" || car == "Товарни")
            {
                carType = CarType.Freight;
                VehicleTypes[0].IsSelected = false;
                VehicleTypes[1].IsSelected = false;
                VehicleTypes[2].IsSelected = true;
            }
        }

        #endregion

        #region Private Methods

        private void FromHourToDate()
        {
            string[] tempStringFrom = SelectedHourFrom.HourString.Split(':');
            TimeSpan tempFrom = new TimeSpan(Convert.ToInt32(tempStringFrom[0]), Convert.ToInt32(tempStringFrom[1]), 0);
            dateFrom = DateFrom.Date + tempFrom;
        }

        private void ToHourToDate()
        {

            string[] tempStringTo = SelectedHourTo.HourString.Split(':');
            TimeSpan tempTo = new TimeSpan(Convert.ToInt32(tempStringTo[0]), Convert.ToInt32(tempStringTo[1]), 0);
            dateTo = DateTo.Date + tempTo;
        }

        private bool Validate()
        {
            return IsPeriodValid && IsFromDateValid && IsToDateValid;
        }


        private void LoadCars()
        {
            try
            {
                List<Car> tempCars = new List<Car>();

                string query = string.Format(@"SELECT 
                                           Cars.Id as Id,
                                           Brand,
		                                   Model,
		                                   Year,
		                                   FuelConsumption,
		                                   CarType,
		                                   Price,
		                                   Capacity,
		                                   ImagePath FROM Cars
                                           JOIN Periods on Cars.Id = Periods.CarId
                                           WHERE StartPeriod <= '{0}' AND EndPeriod >= '{0}'
                                           AND CarType = {2} AND Location = {3}", CarPeriodWrapper.Period.StartPeriod.ToString("yyMMdd HH:mm:ss"),
                                                                                      CarPeriodWrapper.Period.EndPeriod.ToString("yyMMdd HH:mm:ss"),
                                                                                      (int)carType, (int)SelectedLocationFrom.Location);

                Task.Run(async () =>
                {
                    tempCars = (await repositoryManager.QueryAsync<Car>(query)).ToList();
                }).Wait();

                cars = new List<CarPeriodWrapper>();
                for (int i = 0; i < tempCars.Count; i++)
                {
                    cars.Add(new CarPeriodWrapper(tempCars[i], CarPeriodWrapper.Period, CarPeriodWrapper.LocationFrom, CarPeriodWrapper.LocationTo));
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void ChangeView(object e)
        {
            if (Validate())
            {
                Period tempPeriod = new Period();
                tempPeriod.StartPeriod = DateFrom;
                tempPeriod.EndPeriod = DateTo;
                tempPeriod.Location = SelectedLocationFrom.Location;
                CarPeriodWrapper = new CarPeriodWrapper(null, tempPeriod, SelectedLocationFrom, SelectedLocationTo);
                LoadCars();
                OnSearchHandler();
            }
        }

        #endregion

        #endregion
    }
}
