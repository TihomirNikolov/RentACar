using Database.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Reservation : IEntity
    {
        #region Declaration

        [Column("Id")]
        public int? Id { get; set; }
        [Column("CarId")]
        public int CarId { get; set; }
        [Column("PeriodId")]
        public int PeriodId { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
   
        #endregion
    }
}
