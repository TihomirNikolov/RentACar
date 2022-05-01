using Database.Enums;
using Database.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Period : IEntity
    {
        #region Declaration

        [Column("Id")]
        public int? Id { get; set; }
        [Column("StartPeriod")]
        public DateTime StartPeriod { get; set; }
        [Column("EndPeriod")]
        public DateTime EndPeriod { get; set; }
        [Column("Location")]
        public Location Location { get; set; }
        [Column("CarId")]
        public int CarId { get; set; }

        #endregion
    }
}
