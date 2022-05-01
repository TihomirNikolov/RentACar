using Database.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class User : IEntity
    {
        #region Declaration

        [Column("Id")]
        public int? Id { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Phone")]
        public string Phone { get; set; }

        #endregion
    }
}
