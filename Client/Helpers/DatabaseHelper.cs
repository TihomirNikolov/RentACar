using Client.BussinesModels;
using Client.Settings;
using Database;
using Database.Repositories;
using MicroinvestProject.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Helpers
{
    public class DatabaseHelper
    {
        #region Declarations

        private static readonly object lockObj = new object();
        private static DatabaseHelper instance;
        private RepositoryManager repositoryManager;
        private List<string> databases;
        private DatabaseModel database;

        #endregion

        #region Constructors

        private DatabaseHelper()
        {
            databases = new List<string>();
        }

        #endregion

        #region Properties

        public static DatabaseHelper Instance
        {
            get
            {
                lock (lockObj)
                {
                    return instance ?? (instance = new DatabaseHelper());
                }
            }
        }

        public string ConnectionString => string.Format(@"Server={0}; User = {1}; Password = {2}; Database={3};Trusted_Connection=True;", database.Server, database.Username, CryptHelper.Instance.Decrypt(database.Password), database.DatabaseName);
        public string ConnectionStringDefaultDatabase => string.Format(@"Server={0}; User = {1}; Password = {2}; Database= master;Trusted_Connection=True;", database.Server, database.Username, CryptHelper.Instance.Decrypt(database.Password));

        #endregion

        #region Methods

        #region Public Methods

        public void ConnectToServer(DatabaseModel database)
        {
            this.database = database;
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    Constants.ConnectionString = ConnectionString;
                    repositoryManager = new RepositoryManager();

                    SettingsManager.DbServer = database.Server;
                    SettingsManager.DbUsername = database.Username;
                    SettingsManager.DbPassword = database.Password;
                    SettingsManager.DbName = database.DatabaseName;


                }
            }
            catch(SqlException)
            {
                throw;
            }

        }

        public async Task CreateDatabase(DatabaseModel database)
        {
            this.database = database;
            try
            {
                await LoadAllDatabaseAsync();

                if (!databases.Contains(database.DatabaseName))
                {
                    using (var connection = new SqlConnection(ConnectionStringDefaultDatabase))
                    {

                        connection.Open();

                        Constants.ConnectionString = ConnectionString;
                        repositoryManager = new RepositoryManager();

                        await repositoryManager.MigrateAsync();

                        using (var con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            string query = string.Format(@"INSERT INTO Cars(Brand,Model,Year,FuelConsumption,CarType,Price,Capacity,ImagePath) VALUES('Renault','Clio','2021',5.2,1,40,5,'/Images/Renault_Clio.png');
                                                               INSERT INTO Periods(StartPeriod,EndPeriod,Location,CarId) VALUES(SYSDATETIME(),'99991231 23:59:59 PM',0,1);
                                                               INSERT INTO Cars(Brand,Model,Year,FuelConsumption,CarType,Price,Capacity,ImagePath) VALUES('Nissan','Qashqai','2021',7.2,1,60,5,'/Images/Nissan_Qashqai.png');
                                                               INSERT INTO Periods(StartPeriod,EndPeriod,Location,CarId) VALUES(SYSDATETIME(),'99991231 23:59:59 PM',0,2);
                                                               INSERT INTO Cars(Brand,Model,Year,FuelConsumption,CarType,Price,Capacity,ImagePath) VALUES('Toyota','Corolla','2021',6.5,1,45,5,'/Images/Toyota_Corolla.png');
                                                               INSERT INTO Periods(StartPeriod,EndPeriod,Location,CarId) VALUES(SYSDATETIME(),'99991231 23:59:59 PM',0,3);");

                            using (var sc = new SqlCommand(query, con))
                            {
                                await sc.ExecuteNonQueryAsync();
                            }
                        }

                        SettingsManager.DbServer = database.Server;
                        SettingsManager.DbUsername = database.Username;
                        SettingsManager.DbPassword = database.Password;
                        SettingsManager.DbName = database.DatabaseName;
                    }
                }
                else
                {
                    MessageBox.Show(Resources.Resources.ResourceManager.GetString("strDatabaseExists", TranslationSource.Instance.CurrentCulture));
                    throw new Exception();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public List<string> LoadDatabasesFromServer(DatabaseModel database)
        {
            this.database = database;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = string.Format(@"CREATE TABLE #T
                                                   (Name varchar(255) NOT NULL PRIMARY KEY)
                                                   
                                                   DECLARE @Script NVARCHAR(MAX) = ''
                                                   
                                                   SELECT @Script = @Script + '
                                                   
                                                   USE ' + QUOTENAME(name) + '
                                                   IF EXISTS(SELECT * FROM sys.tables WHERE OBJECT_ID=OBJECT_ID(''dbo.Cars''))
                                                     INSERT INTO #T
                                                     SELECT DB_NAME();
                                                     '
                                                   FROM sys.databases 
                                                   WHERE state=0 AND user_access=0 and has_dbaccess(name) = 1
                                                   ORDER BY [name]
                                                   
                                                   IF (@@ROWCOUNT > 0)
                                                    BEGIN
                                                    --PRINT @Script
                                                    EXEC (@Script)
                                                    SELECT * FROM #T
                                                    END
                                                   
                                                    DROP TABLE #T");
                                                   
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    databases.Add(dr[0].ToString());
                                });
                            }
                        }
                    }
                    return databases;
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion

        #region Private Methods

        private async Task LoadAllDatabaseAsync()
        {
            if (databases.Count == 0)
            {
                await Task.Run(() =>
                {
                    LoadAllDatabasesFromServer();
                });
            }
        }

        private void LoadAllDatabasesFromServer()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(string.Format(@"SELECT * FROM sys.databases "), con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    databases.Add(dr[0].ToString());
                                });
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        #endregion

        #endregion
    }
}
