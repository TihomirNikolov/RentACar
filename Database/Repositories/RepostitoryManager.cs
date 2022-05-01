using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        #region Delclarations

        protected object lockObject = new object();
        DataContext database;

        private IGenericRepository<User> usersRepository;
        private IGenericRepository<Car> carsRepository;
        private IGenericRepository<Period> periodsRepository;
        private IGenericRepository<Reservation> reservationsRepository;

        #endregion

        #region Constructor

        public RepositoryManager()
        {
            database = new DataContext();
        }

        #endregion

        #region Properties

        public IGenericRepository<User> UsersRepository => usersRepository ?? (usersRepository = new GenericRepository<User>(lockObject, database));
        public IGenericRepository<Car> CarsRepository => carsRepository ?? (carsRepository = new GenericRepository<Car>(lockObject, database));
        public IGenericRepository<Period> PeriodsRepository => periodsRepository ?? (periodsRepository = new GenericRepository<Period>(lockObject, database));
        public IGenericRepository<Reservation> ReservationsRepository => reservationsRepository ?? (reservationsRepository = new GenericRepository<Reservation>(lockObject, database));

        #endregion

        #region Methods

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string query) where TResult : class, new()
        {
            try
            {
                var result = new List<TResult>();
                var type = typeof(TResult);

                var properties = type.GetProperties();

                var connection = database.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        var propertyColumnMapDictionary = new Dictionary<string, PropertyInfo>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var column = reader.GetName(i);
                            var property = properties.FirstOrDefault(p => ((ColumnAttribute)p.GetCustomAttributes(typeof(ColumnAttribute), true)[0]).Name == column);
                            if (property != null)
                            {
                                propertyColumnMapDictionary.Add(column, property);
                            }
                        }

                        while (await reader.ReadAsync())
                        {
                            var entity = (TResult)Activator.CreateInstance(type);
                            foreach (var item in propertyColumnMapDictionary)
                            {
                                var value = reader[item.Key];
                                if (value == DBNull.Value)
                                    value = null;
                                item.Value.SetValue(entity, value);
                            }

                            result.Add(entity);
                        }
                    }
                }

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<TResult> ExecuteScalarAsync<TResult>(string query)
        {
            try
            {
                var connection = database.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    var result = command.ExecuteScalar();
                    if (result == null)
                    {
                        return default;
                    }

                    return await Task.FromResult((TResult)result);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task MigrateAsync()
        {
            try
            {
                await database.Database.MigrateAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void BeginTransaction()
        {
            database.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            database.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            database.Database.RollbackTransaction();
        }

        public void SaveChanges()
        {
            database.SaveChanges();
        }

        #endregion
    }
}
