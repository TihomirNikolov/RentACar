using Database.Models;
using Database.Repositories;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public class UserHelper
    {
        #region Declarations

        private RepositoryManager repositoryManager;

        #endregion

        #region Constructors

        public UserHelper(RepositoryManager repositoryManager)
        {
            this.repositoryManager = repositoryManager;
        }

        #endregion

        #region Methods

        public int? SaveUser(User user)
        {
            User dbUser = new User();

            Task.Run(async () =>
            {
                dbUser = await repositoryManager.UsersRepository.SelectAsync(u => u.Email == user.Email);
            });

            if (dbUser != null)
            {
                return repositoryManager.UsersRepository.Insert(user);
            }

            return dbUser.Id;
        }

        #endregion
    }
}
