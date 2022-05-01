using Client.BussinesModels;
using Database.Models;
using Database.Repositories;
using System;

namespace Client.Helpers
{
    public class ReservationHelper
    {
        #region Declarations

        private RepositoryManager repositoryManager;
        private PeriodHelper periodHelper;
        private UserHelper userHelper;

        #endregion

        #region Constructors

        public ReservationHelper()
        {
            repositoryManager = new RepositoryManager();
            periodHelper = new PeriodHelper(repositoryManager);
            userHelper = new UserHelper(repositoryManager);
        }

        #endregion

        #region Methods

        public void Save(User user, CarPeriodWrapper car)
        {
            try
            {
                repositoryManager.BeginTransaction();

                user.Id = userHelper.SaveUser(user);

                periodHelper.SavePeriods(car);

                SaveRegistration(car.Car.Id.Value, car.Period.Id.Value, user.Id.Value);
                repositoryManager.SaveChanges();
                repositoryManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                repositoryManager.RollbackTransaction();
            }
        }

        private void SaveRegistration(int carId, int periodId, int userId)
        {
            Reservation reservation = new Reservation()
            {
                CarId = carId,
                PeriodId = periodId,
                UserId = userId
            };

            repositoryManager.ReservationsRepository.Insert(reservation);
        }

        #endregion
    }
}
