using Client.BussinesModels;
using Database.Enums;
using Database.Models;
using Database.Repositories;
using System;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public class PeriodHelper
    {
        #region Declarations

        private RepositoryManager repositoryManager;

        #endregion

        #region Constructor

        public PeriodHelper(RepositoryManager repositoryManager)
        {
            this.repositoryManager = repositoryManager;
        }

        #endregion

        #region Methods

        public void SavePeriods(CarPeriodWrapper car)
        {
            try
            {
                Period period = new Period();

                Task.Run(async () =>
                {
                    period = await GetPeriodAsync(car);
                }).Wait();

                car.Period.Id = SavePeriod(car, Location.OnTrip);

                Period temp = new Period()
                {
                    StartPeriod = car.Period.StartPeriod,
                    EndPeriod = car.Period.EndPeriod
                };

                car.Period.StartPeriod = period.StartPeriod;
                car.Period.EndPeriod = temp.StartPeriod;

                SavePeriod(car, car.LocationFrom.Location);

                car.Period.StartPeriod = temp.EndPeriod;
                car.Period.EndPeriod = period.EndPeriod;

                SavePeriod(car, car.LocationTo.Location);

                DeletePeriod(period);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int SavePeriod(CarPeriodWrapper car, Location location)
        {
            Period period = new Period()
            {
                CarId = car.Car.Id.Value,
                StartPeriod = car.Period.StartPeriod,
                EndPeriod = car.Period.EndPeriod,
                Location = location
            };

            return repositoryManager.PeriodsRepository.Insert(period);
        }

        private void DeletePeriod(Period period)
        {
            repositoryManager.PeriodsRepository.Delete(period);
        }

        private async Task<Period> GetPeriodAsync(CarPeriodWrapper car)
        {
            return await repositoryManager.PeriodsRepository.SelectAsync(p => p.StartPeriod < car.Period.StartPeriod && p.EndPeriod > car.Period.EndPeriod
                                                                                && p.Location == car.Period.Location && p.CarId == car.Car.Id);
        }

        #endregion
    }
}
