using Database.Enums;
using Database.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Car : IEntity
    {
        #region Declaration

        [Column("Id")]
        public int? Id { get; set; }
        [Column("Brand")]
        public string Brand { get; set; }
        [Column("Model")]
        public string Model { get; set; }
        [Column("Year")]
        public string Year { get; set; }
        [Column("FuelConsumption")]
        public decimal FuelConsumption { get; set; }
        [Column("CarType")]
        public CarType CarType { get; set; }
        [Column("Price")]
        public decimal Price { get; set; }
        [Column("Capacity")]
        public int Capacity { get; set; }
        [Column("ImagePath")]
        public string ImagePath { get; set; }

        #endregion
    }
}
