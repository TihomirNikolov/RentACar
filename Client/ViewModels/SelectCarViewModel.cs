using Client.BussinesModels;
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
    public class SelectCarViewModel : BaseViewModel
    {
        #region Declarations

        public event Action<CarPeriodWrapper> SelectEvent;

        private CarType carType;
        private List<CarPeriodWrapper> cars;
        private CarPeriodWrapper selectedCar;
        private CarPeriodWrapper carPeriodWrapper;
        private IRepositoryManager repositoryManager;

        #endregion

        #region Constructor

        public SelectCarViewModel(CarPeriodWrapper carPeriodWrapper, CarType carType)
        {
            repositoryManager = new RepositoryManager();
            CarPeriodWrapper = carPeriodWrapper;
            this.carType = carType;
            LoadCars();
        }

        #endregion

        #region Properties

        public List<CarPeriodWrapper> Cars
        {
            get => cars;
            set
            {
                cars = value;
                NotifyPropertyChanged(nameof(Cars));
            }
        }

        public CarPeriodWrapper SelectedCar
        {
            get => selectedCar;
            set
            {
                selectedCar = value;
                CarPeriodWrapper.Car = selectedCar.Car;
                OnSelectHandler();
                NotifyPropertyChanged(nameof(SelectedCar));
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

        #endregion

        #region Methods

        public void OnSelectHandler()
        {
            SelectEvent?.Invoke(CarPeriodWrapper);
        }

        public void LoadCars()
        {
            List<Car> tempCars = new List<Car>();

            string query = string.Format(@"SELECT
                                           Cars.Id AS Id,
                                           Brand,
		                                   Model,
		                                   Year,
		                                   FuelConsumption,
		                                   CarType,
		                                   Price,
		                                   Capacity,
		                                   ImagePath FROM Cars
                                           JOIN Periods on Cars.Id = Periods.CarId
                                           WHERE StartPeriod <= '{0}' AND EndPeriod >= '{1}'
                                           AND CarType = {2} AND Location = {3}
                                           ORDER BY Brand", CarPeriodWrapper.Period.StartPeriod.ToString("yyMMdd HH:mm:ss"),
                                                            CarPeriodWrapper.Period.EndPeriod.ToString("yyMMdd HH:mm:ss"),
                                                            (int)carType, (int)CarPeriodWrapper.LocationFrom.Location);

            Task.Run(async () =>
            {
                tempCars = (await repositoryManager.QueryAsync<Car>(query)).ToList();
            }).Wait();

            Cars = new List<CarPeriodWrapper>();
            for (int i = 0; i < tempCars.Count; i++)
            {
                Cars.Add(new CarPeriodWrapper(tempCars[i], CarPeriodWrapper.Period, CarPeriodWrapper.LocationFrom, CarPeriodWrapper.LocationTo));
            }
        }

        #endregion
    }
}
