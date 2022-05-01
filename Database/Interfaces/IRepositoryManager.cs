using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IRepositoryManager
    {
        IGenericRepository<User> UsersRepository { get; }
        IGenericRepository<Car> CarsRepository { get; }
        IGenericRepository<Period> PeriodsRepository { get; }
        IGenericRepository<Reservation> ReservationsRepository { get; }

        Task<IEnumerable<TResult>> QueryAsync<TResult>(string query) where TResult : class, new();
        Task<TResult> ExecuteScalarAsync<TResult>(string query);
    }
}
